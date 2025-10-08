using StackExchange.Redis;
using Louisepizdon.Caching;
using Louisepizdon.Platform;
using Louisepizdon.Tracing;

namespace Louisepizdon.Throttler;

public interface IUsageLimiter
{
    Task<LimitCheckResult> CheckAndIncrementAsync(long telegramUserId);
}

public class LimitCheckResult
{
    public bool Exceeded { get; set; }
    public bool IsDaily { get; set; }
    public int CurrentCount { get; set; }
    public int Limit { get; set; }
}

public class UsageLimiter : IUsageLimiter
{
    private readonly IRedisClient _redis;
    private readonly AppConfig _config;
    private readonly IAppLogger _logger;

    public UsageLimiter(IRedisClient redis, AppConfig config, IAppLogger logger)
    {
        _redis = redis;
        _config = config;
        _logger = logger;
    }

    public async Task<LimitCheckResult> CheckAndIncrementAsync(long telegramUserId)
    {
        var dailyKey = GetUsageKey("daily", telegramUserId);
        var monthlyKey = GetUsageKey("monthly", telegramUserId);

        var db = _redis.Database;

        // Check monthly limit first
        var monthlyCount = await GetCountAsync(db, monthlyKey);
        if (monthlyCount >= _config.Limits.MonthlyLimit)
        {
            _logger.Warning("Monthly limit exceeded for user {TelegramUserId}: {Count}/{Limit}", 
                telegramUserId, monthlyCount, _config.Limits.MonthlyLimit);
            
            return new LimitCheckResult
            {
                Exceeded = true,
                IsDaily = false,
                CurrentCount = monthlyCount,
                Limit = _config.Limits.MonthlyLimit
            };
        }

        // Check daily limit
        var dailyCount = await GetCountAsync(db, dailyKey);
        if (dailyCount >= _config.Limits.DailyLimit)
        {
            _logger.Warning("Daily limit exceeded for user {TelegramUserId}: {Count}/{Limit}", 
                telegramUserId, dailyCount, _config.Limits.DailyLimit);
            
            return new LimitCheckResult
            {
                Exceeded = true,
                IsDaily = true,
                CurrentCount = dailyCount,
                Limit = _config.Limits.DailyLimit
            };
        }

        // Increment both counters
        var transaction = db.CreateTransaction();
        
        var dailyIncr = transaction.StringIncrementAsync(dailyKey);
        var monthlyIncr = transaction.StringIncrementAsync(monthlyKey);
        
        // Set TTL for keys
        transaction.KeyExpireAsync(dailyKey, TimeSpan.FromHours(25));
        transaction.KeyExpireAsync(monthlyKey, TimeSpan.FromDays(32));

        await transaction.ExecuteAsync();

        var newDailyCount = (int)await dailyIncr;
        var newMonthlyCount = (int)await monthlyIncr;

        _logger.Info("Usage incremented for user {TelegramUserId}: Daily {DailyCount}/{DailyLimit}, Monthly {MonthlyCount}/{MonthlyLimit}",
            telegramUserId, newDailyCount, _config.Limits.DailyLimit, newMonthlyCount, _config.Limits.MonthlyLimit);

        return new LimitCheckResult
        {
            Exceeded = false,
            IsDaily = false,
            CurrentCount = newDailyCount,
            Limit = _config.Limits.DailyLimit
        };
    }

    private async Task<int> GetCountAsync(IDatabase db, string key)
    {
        var value = await db.StringGetAsync(key);
        return value.HasValue ? (int)value : 0;
    }

    private string GetUsageKey(string period, long telegramUserId)
    {
        var now = DateTime.UtcNow;
        var timePart = period switch
        {
            "daily" => now.ToString("yyyy-MM-dd"),
            "monthly" => now.ToString("yyyy-MM"),
            _ => throw new ArgumentException($"Invalid period: {period}")
        };

        return $"usage:vision:{period}:{telegramUserId}:{timePart}";
    }
}