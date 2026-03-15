using Amazon.S3;
using Application.Interfaces;
using Application.Options;
using Application.Services;
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
                var jwtOptions = config.GetSection("Jwt").Get<JwtOptions>();
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
                options.ClientId = config.GetValue<string>("Google:ClientId")!; options.ClientSecret = config.GetValue<string>("Google:ClientSecret")!; 
                options.CallbackPath = "/signin-google";
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

            //add customed services
            service.AddScoped<ITokenService, TokenService>();
            service.AddScoped<IAuthService, AuthService>();
            service.AddTransient<IEmailService, EmailService>();
            service.AddScoped<IStorageService, S3StorageService>();
            service.AddScoped<IAdService, AdService>();
            service.AddSingleton<LocationService>();

            return service;
        }
    }
}
