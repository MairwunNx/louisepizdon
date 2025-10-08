using StackExchange.Redis;
using Louisepizdon.Platform;
using Louisepizdon.Tracing;

namespace Louisepizdon.Caching;

public interface IRedisClient
{
    IDatabase Database { get; }
}

public class RedisClient : IRedisClient, IDisposable
{
    private readonly ConnectionMultiplexer _connection;
    private readonly IAppLogger _logger;

    public RedisClient(AppConfig config, IAppLogger logger)
    {
        _logger = logger;
        
        try
        {
            _logger.Info("Connecting to Redis: {ConnectionString}", config.Redis.ConnectionString);
            _connection = ConnectionMultiplexer.Connect(config.Redis.ConnectionString);
            _logger.Info("Successfully connected to Redis");
        }
        catch (Exception ex)
        {
            _logger.Error("Failed to connect to Redis", ex);
            throw;
        }
    }

    public IDatabase Database => _connection.GetDatabase();

    public void Dispose()
    {
        _connection?.Dispose();
        GC.SuppressFinalize(this);
    }
}