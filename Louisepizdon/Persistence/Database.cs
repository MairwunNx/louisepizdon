using Microsoft.EntityFrameworkCore;
using Louisepizdon.Platform;

namespace Louisepizdon.Persistence;

public class AppDbContext : DbContext
{
    private readonly string _connectionString;

    public AppDbContext(AppConfig config)
    {
        _connectionString = config.Database.ConnectionString;
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Usage> Usages { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(_connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(u => u.TelegramUserId)
            .IsUnique();
        
        modelBuilder.Entity<Usage>()
            .HasOne(u => u.User)
            .WithMany()
            .HasForeignKey(u => u.UserId);
    }

    public async Task MigrateAsync()
    {
        await Database.MigrateAsync();
    }
}