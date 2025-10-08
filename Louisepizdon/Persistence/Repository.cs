using Microsoft.EntityFrameworkCore;
using Louisepizdon.Tracing;

namespace Louisepizdon.Persistence;

public interface IUserRepository
{
    Task<User?> GetByTelegramIdAsync(long telegramUserId);
    Task<User> CreateAsync(long telegramUserId, string userName, string? nickname);
    Task UpdateAcceptanceAsync(long telegramUserId, bool accepted);
}

public interface IUsageRepository
{
    Task AddUsageAsync(int userId);
}

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;
    private readonly IAppLogger _logger;

    public UserRepository(AppDbContext context, IAppLogger logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<User?> GetByTelegramIdAsync(long telegramUserId)
    {
        try
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.TelegramUserId == telegramUserId);
        }
        catch (Exception ex)
        {
            _logger.Error("Failed to get user by telegram ID {TelegramUserId}", ex, telegramUserId);
            throw;
        }
    }

    public async Task<User> CreateAsync(long telegramUserId, string userName, string? nickname)
    {
        try
        {
            var user = new User
            {
                TelegramUserId = telegramUserId,
                TelegramUserName = userName,
                TelegramUserNickname = nickname,
                IsAccepted = false,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            _logger.Info("Created new user: {TelegramUserId} ({UserName})", telegramUserId, userName);
            
            return user;
        }
        catch (Exception ex)
        {
            _logger.Error("Failed to create user {TelegramUserId}", ex, telegramUserId);
            throw;
        }
    }

    public async Task UpdateAcceptanceAsync(long telegramUserId, bool accepted)
    {
        try
        {
            var user = await GetByTelegramIdAsync(telegramUserId);
            if (user == null)
            {
                _logger.Warning("Attempted to update acceptance for non-existent user {TelegramUserId}", telegramUserId);
                return;
            }

            user.IsAccepted = accepted;
            await _context.SaveChangesAsync();

            _logger.Info("Updated acceptance for user {TelegramUserId}: {Accepted}", telegramUserId, accepted);
        }
        catch (Exception ex)
        {
            _logger.Error("Failed to update acceptance for user {TelegramUserId}", ex, telegramUserId);
            throw;
        }
    }
}

public class UsageRepository : IUsageRepository
{
    private readonly AppDbContext _context;
    private readonly IAppLogger _logger;

    public UsageRepository(AppDbContext context, IAppLogger logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task AddUsageAsync(int userId)
    {
        try
        {
            var usage = new Usage
            {
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Usages.Add(usage);
            await _context.SaveChangesAsync();

            _logger.Info("Added usage record for user {UserId}", userId);
        }
        catch (Exception ex)
        {
            _logger.Error("Failed to add usage for user {UserId}", ex, userId);
            throw;
        }
    }
}