using Domain.Enums;
using Domain.Identity;
using Infrastructure.Interfaces;
using Infrastructure.Presistance;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class InfrastructureDI
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection service, IConfiguration config)
        {
            string connectionString = config.GetConnectionString("Default") ?? throw new Exception("Connection string cannot be empty");


            service.AddDbContext<AppDbContext>(options => 
                options.UseNpgsql(connectionString, o =>
                {
                    o.MapEnum<AdType>();
                    o.MapEnum<AdState>();
                    o.MapEnum<PropertyType>();
                    o.MapEnum<AdAction>();
                    o.MapEnum<CreditsLogAction>();
                    o.MapEnum<PaymentStatus>();
                })
            );
            service.AddDataProtection();
            service.AddIdentity<User,Role>(options =>
            {
                    options.Password.RequireDigit = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireNonAlphanumeric = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequiredLength = 6;
            })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            // Register repositories
            service.AddScoped<IRefreshTokenRepo, RefreshTokenRepo>();
            service.AddScoped<IUnitOfWork, UnitOfWork>();
            service.AddScoped<IImageRepo, ImageRepo>();
            service.AddScoped<IAdRepo, AdRepo>();
            service.AddScoped<IAdLogRepo, AdLogRepo>();
            service.AddScoped<ICreditsLogRepo, CreditsLogRepo>();
            service.AddScoped<ICreditsRepo, CreditsRepo>();
            service.AddScoped<IBrokerRepo, BrokerRepo>();

            return service;
        }
    }
}
