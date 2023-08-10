using Application.Interfaces;
using Application.Providers;
using Application.Providers.Interfaces;
using Domain.Entities;
using Infrastructure.Handlers;
using Infrastructure.Handlers.Interfaces;
using Infrastructure.Options;
using Infrastructure.Persistence;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SendGrid;
using SendGrid.Extensions.DependencyInjection;
using TokenOptions = Infrastructure.Options.TokenOptions;

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

        var sendGridKey = configuration["SendGrid:ApiKey"];
        services.AddSendGrid(options => options.ApiKey = sendGridKey);

        AddOptions(services, configuration);
        AddDb(services, configuration);
    }

    private static void AddDb(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<IdentityOptions>(configuration.GetSection("IdentityOptions"));
        services
            .AddIdentityCore<Account>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

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
        services.Configure<SendGridSettings>(configuration.GetSection("SendGrid"));
    }
}