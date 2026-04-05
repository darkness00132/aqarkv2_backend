using Amazon.S3;
using Application.Constants;
using Application.Interfaces;
using Application.Interfaces.ThirdPartyService;
using Application.Options;
using Application.Services;
using Application.Services.ThirdPartyService;
using Domain.Enums;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Resend;
using System.Text;

namespace Application
{
    public static class ApplicationDI
    {
        public static IServiceCollection AddApplication(this IServiceCollection service, IConfiguration config) 
        {
            service.AddOptions();

            service.AddHttpClient<ResendClient>();
            service.Configure<ResendClientOptions>(opt=>opt.ApiToken = config.GetValue<string>("EmailService:ApiKey")!);
            service.AddTransient<IResend,ResendClient>();

            service.AddAutoMapper(cfg => cfg.AddMaps(typeof(ApplicationDI).Assembly));

            service.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                var jwtOptions = config.GetSection("Jwt").Get<JwtSettings>();
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtOptions!.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key)),
                };
            }).AddGoogle(options => {
                options.ClientId = config.GetValue<string>("Google:ClientId")!;
                options.ClientSecret = config.GetValue<string>("Google:ClientSecret")!; 
                options.CallbackPath = "/signin-google";
                options.ClaimActions.MapJsonKey("urn:google:picture", "picture", "url");
            });

            //add S3 storage
            service.Configure<S3Settings>(config.GetSection("S3Settings"));
            service.AddSingleton<IAmazonS3>(sp =>
            {
                var s3Settings = sp.GetRequiredService<IOptions<S3Settings>>().Value;
                var s3Config = new AmazonS3Config
                {
                    RegionEndpoint = Amazon.RegionEndpoint.GetBySystemName(s3Settings.Region)
                };

                if (!string.IsNullOrWhiteSpace(s3Settings.AccessKey) &&
                    !string.IsNullOrWhiteSpace(s3Settings.SecretKey))
                {
                    return new AmazonS3Client(s3Settings.AccessKey, s3Settings.SecretKey, s3Config);
                }

                return new AmazonS3Client(s3Config);
            });

            service.AddAuthorization(options =>
            {
                options.AddPolicy(Policies.BrokerOnly, policy =>
                    policy.RequireRole(nameof(UserRoles.Broker)));

                options.AddPolicy(Policies.AdminOnly, policy =>
                    policy.RequireRole(nameof(UserRoles.Admin), nameof(UserRoles.SuperAdmin)));

                options.AddPolicy(Policies.AdminOrBroker, policy =>
                    policy.RequireRole(nameof(UserRoles.SuperAdmin), nameof(UserRoles.Admin), nameof(UserRoles.Broker)));

                options.AddPolicy(Policies.SuperAdminOnly, policy =>
                    policy.RequireRole(nameof(UserRoles.SuperAdmin)));
            });

            //add customed services
            service.AddScoped<ITokenService, JwtTokenService>();
            service.AddScoped<IAuthService, AuthService>();
            service.AddTransient<IEmailService, ResendEmailService>();
            service.AddScoped<IImageService, S3ImageService>();
            service.AddScoped<IAdService, AdService>();
            service.AddSingleton<LocationService>();
            service.AddScoped<IBrokerService, BrokerService>();
            service.AddScoped<IUserService, UserService>();
            service.AddScoped<IReviewService, ReviewService>();

            return service;
        }
    }
}
