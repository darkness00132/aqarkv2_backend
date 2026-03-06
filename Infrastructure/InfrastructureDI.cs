using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Presistance;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Infrastructure.Repositories;
using Infrastructure.Interfaces;

namespace Infrastructure
{
    public static class InfrastructureDI
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection service, IConfiguration config)
        {
            string connectionString = config.GetConnectionString("Default") ?? throw new Exception("Connection string cannot be empty");

            service.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));
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

            return service;
        }
    }
}
