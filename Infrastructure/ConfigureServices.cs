using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using Infrastructure.Migrations;
using Infrastructure.Persistence;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class ConfigureServices
{
    public static void AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IEmailService, EmailService>();
        
        services
            .AddIdentityCore<Account>(options => {
                options.SignIn.RequireConfirmedAccount = true;
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>();
        
        var dbConfiguration = configuration
            .GetSection("DbConfiguration")
            .Get<DbConfiguration>()!;

        var connectionString = dbConfiguration.GetConnectionString();
        services
            .AddDbContext<IApplicationDbContext, ApplicationDbContext>(builder =>
            {
                builder.UseNpgsql(connectionString, o =>
                {
                    o.EnableRetryOnFailure(
                        dbConfiguration.RetryCount,
                        TimeSpan.FromSeconds(dbConfiguration.RetryTime),
                        null);
                });
            });
    }
}