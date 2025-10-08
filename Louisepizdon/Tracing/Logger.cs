using Serilog;
using Serilog.Events;

namespace Louisepizdon.Tracing;

public interface IAppLogger
{
    void Info(string message, params object[] args);
    void Warning(string message, params object[] args);
    void Error(string message, Exception? exception = null, params object[] args);
    void Debug(string message, params object[] args);
}

public class AppLogger : IAppLogger
{
    private readonly ILogger _logger;

    public AppLogger()
    {
        _logger = Log.Logger;
    }

    public void Info(string message, params object[] args)
    {
        _logger.Information(message, args);
    }

    public void Warning(string message, params object[] args)
    {
        _logger.Warning(message, args);
    }

    public void Error(string message, Exception? exception = null, params object[] args)
    {
        if (exception != null)
        {
            _logger.Error(exception, message, args);
        }
        else
        {
            _logger.Error(message, args);
        }
    }

    public void Debug(string message, params object[] args)
    {
        _logger.Debug(message, args);
    }
}

public static class LoggerSetup
{
    public static void Configure()
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("System", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .Enrich.WithProperty("Application", "Louisepizdon")
            .WriteTo.Console(
                outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
            .WriteTo.File(
                "logs/louisepizdon-.log",
                rollingInterval: RollingInterval.Day,
                outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} {Level:u3}] {Message:lj}{NewLine}{Exception}")
            .CreateLogger();
    }
}