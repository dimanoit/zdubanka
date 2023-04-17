using Npgsql;

namespace Infrastructure.Persistence;

public static class DbConfigurationExtensions
{
    public static string GetConnectionString(this DbConfiguration options)
    {
        var csBuilder = new NpgsqlConnectionStringBuilder
        {
            Host = options.Host,
            Database = options.Name,
            Username = options.User,
            Port = options.Port,
            Password = options.Password,
            Timeout = options.Timeout,
            CommandTimeout = options.CommandTimeout,
            Pooling = options.Pooling
        };

        return csBuilder.ConnectionString;
    }
}