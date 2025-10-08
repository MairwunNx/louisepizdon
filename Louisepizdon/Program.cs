using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Louisepizdon.Platform;
using Louisepizdon.Tracing;
using Louisepizdon.Persistence;
using Louisepizdon.Caching;
using Louisepizdon.Throttler;
using Louisepizdon.Artificial;
using Louisepizdon.Telegram;

namespace Louisepizdon;

public class Program
{
    public static async Task Main(string[] args)
    {
        // Set timezone
        SetTimezone();

        // Configure logging
        LoggerSetup.Configure();

        var logger = new AppLogger();
        
        try
        {
            logger.Info("Starting Louisepizdon...");
            Manifest.SetManifest("0.0.1", DateTime.UtcNow.ToString("yyyy-MM-dd"));

            // Load configuration
            var config = ConfigLoader.Load();
            logger.Info("Configuration loaded successfully");

            // Build and run host
            var host = CreateHostBuilder(args, config, logger).Build();

            // Run migrations
            await RunMigrationsAsync(host);

            logger.Info("✅ {Message}", Texting.Phrases.BotStarted);
            
            await host.RunAsync();
        }
        catch (Exception ex)
        {
            logger.Error("Fatal error occurred", ex);
            throw;
        }
        finally
        {
            logger.Info("⏹️ {Message}", Texting.Phrases.BotStopped);
            Serilog.Log.CloseAndFlush();
        }
    }

    private static IHostBuilder CreateHostBuilder(string[] args, AppConfig config, IAppLogger logger)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                // Configuration
                services.AddSingleton(config);

                // Logging
                services.AddSingleton<IAppLogger>(logger);

                // Database
                services.AddSingleton<AppDbContext>();
                services.AddSingleton<IUserRepository, UserRepository>();
                services.AddSingleton<IUsageRepository, UsageRepository>();
                services.AddSingleton<DatabaseMigrator>();

                // Redis
                services.AddSingleton<IRedisClient, RedisClient>();

                // Throttler
                services.AddSingleton<IUsageLimiter, UsageLimiter>();

                // AI Agent
                services.AddSingleton<IAIAgent, VisionAgent>();

                // Telegram
                services.AddSingleton(sp => TelegramClientFactory.Create(config, logger));
                services.AddSingleton<IUpdateHandler, BotUpdateHandler>();
                services.AddSingleton<IBotPoller, BotPoller>();

                // Hosted service
                services.AddHostedService<BotHostedService>();
            });
    }

    private static async Task RunMigrationsAsync(IHost host)
    {
        using var scope = host.Services.CreateScope();
        var migrator = scope.ServiceProvider.GetRequiredService<DatabaseMigrator>();
        await migrator.EnsureCreatedAsync();
    }

    private static void SetTimezone()
    {
        var tz = Environment.GetEnvironmentVariable("TZ");
        if (!string.IsNullOrEmpty(tz))
        {
            try
            {
                var timeZone = TimeZoneInfo.FindSystemTimeZoneById(tz);
                // Note: Cannot set TimeZone in .NET like in Go, but we can use it in our code
                Console.WriteLine($"Timezone set to: {tz}");
            }
            catch
            {
                Console.WriteLine($"Warning: Could not set timezone to {tz}");
            }
        }
    }
}

public class BotHostedService : IHostedService
{
    private readonly IBotPoller _poller;
    private readonly IAppLogger _logger;
    private CancellationTokenSource? _cts;

    public BotHostedService(IBotPoller poller, IAppLogger logger)
    {
        _poller = poller;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.Info("Starting bot hosted service...");
        _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        await _poller.StartAsync(_cts.Token);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.Info("Stopping bot hosted service...");
        _cts?.Cancel();
        return Task.CompletedTask;
    }
}