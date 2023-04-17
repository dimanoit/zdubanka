using Application.Interfaces;
using Infrastructure.Identity;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class ConfigureServices
{
    public static void AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
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
        
        services
            .AddDefaultIdentity<ApplicationUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        services.AddIdentityServer()
            .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();

        services.AddTransient<IIdentityService, IdentityService>();

        services.AddAuthentication()
            .AddIdentityServerJwt();

        services.AddAuthorization(options =>
            options.AddPolicy("CanPurge", policy => policy.RequireRole("Administrator")));
    }
}