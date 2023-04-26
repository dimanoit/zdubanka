using Application.Interfaces;
using Application.Providers;
using Application.Providers.Interfaces;
using Domain.Entities;
using Infrastructure.Handlers;
using Infrastructure.Options;
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
        LogInjector.AddLogging();
        services.AddScoped<IEmailService, EmailService>();
        services.AddTransient<IDateTimeProvider, DateTimeProvider>();
        services.AddTransient<ITokenHandler, JwtTokenHandler>();
        
        AddOptions(services, configuration);
        AddDb(services, configuration);
    }

    private static void AddDb(IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddIdentityCore<Account>(options =>
            {
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

    private static void AddOptions(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<GoogleOptions>(configuration.GetSection("GoogleOptions"));
        services.Configure<TokenOptions>(configuration.GetSection("TokenOptions"));
    }
}