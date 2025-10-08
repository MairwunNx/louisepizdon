using Microsoft.EntityFrameworkCore;
using Louisepizdon.Tracing;

namespace Louisepizdon.Persistence;

public class DatabaseMigrator
{
    private readonly AppDbContext _context;
    private readonly IAppLogger _logger;

    public DatabaseMigrator(AppDbContext context, IAppLogger logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task MigrateAsync()
    {
        try
        {
            _logger.Info("Starting database migration...");
            
            var pendingMigrations = await _context.Database.GetPendingMigrationsAsync();
            
            if (pendingMigrations.Any())
            {
                _logger.Info("Applying {Count} pending migrations", pendingMigrations.Count());
                await _context.Database.MigrateAsync();
                _logger.Info("Database migration completed successfully");
            }
            else
            {
                _logger.Info("Database is up to date, no migrations needed");
            }
        }
        catch (Exception ex)
        {
            _logger.Error("Failed to migrate database", ex);
            throw;
        }
    }

    public async Task EnsureCreatedAsync()
    {
        try
        {
            _logger.Info("Ensuring database is created...");
            await _context.Database.EnsureCreatedAsync();
            _logger.Info("Database creation check completed");
        }
        catch (Exception ex)
        {
            _logger.Error("Failed to ensure database creation", ex);
            throw;
        }
    }
}