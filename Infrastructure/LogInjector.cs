using Serilog;
using Serilog.Configuration;
using Serilog.Events;
using Serilog.Filters;

namespace Infrastructure;

public static class LogInjector
{
    public static void AddLogging()
    {
        var loggerConfiguration =
            new LoggerConfiguration()
                .MinimumLevel.Is(LogEventLevel.Information)
                .Enrich.FromLogContext()
                .Enrich.WithProperty("AppName", "Zdubanka")
                .WriteTo.Console();

#if !DEBUG
        loggerConfiguration.FilterLogs();
#endif

        Log.Logger = loggerConfiguration.CreateLogger();
    }

    private static void FilterLogs(this LoggerConfiguration loggerConfiguration)
    {
        loggerConfiguration
            .Filter.ByExcludingLogsFrom("Microsoft.EntityFrameworkCore", LogEventLevel.Information)
            .Filter.ByExcludingLogsFrom("Microsoft.AspNetCore.Routing.EndpointMiddleware", LogEventLevel.Information);
    }

    private static LoggerConfiguration ByExcludingLogsFrom(this LoggerFilterConfiguration filterConfiguration,
        string sourceContext, LogEventLevel logLevel)
    {
        return filterConfiguration.ByExcluding(LogsFrom(sourceContext, logLevel));
    }

    private static Func<LogEvent, bool> LogsFrom(string sourceContext, LogEventLevel level)
    {
        var sourceContextFilter = Matching.FromSource(sourceContext);
        return logEvent => logEvent.Level == level && sourceContextFilter(logEvent);
    }
}