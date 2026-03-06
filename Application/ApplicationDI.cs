using Application.Interfaces;
using Application.Options;
using Application.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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

            service.AddAutoMapper(config => { } 
            ,AppDomain.CurrentDomain.GetAssemblies());

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
                options.ClientId = config.GetValue<string>("Google:ClientId")!; options.ClientSecret = config.GetValue<string>("Google:ClientSecret")!; options.CallbackPath = "/Auth/google/callback";
            });

            //add services
            service.AddScoped<ICurrentUser, CurrentUser>();
            service.AddScoped<ITokenService, TokenService>();
            service.AddScoped<IAuthService, AuthService>();
            service.AddTransient<IEmailService, EmailService>();

            return service;
        }
    }
}
