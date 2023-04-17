using Application.Interfaces;
using Infrastructure.Persistence;
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
    }
}