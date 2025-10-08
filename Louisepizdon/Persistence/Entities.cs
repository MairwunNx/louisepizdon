using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Louisepizdon.Persistence;

[Table("users")]
public class User
{
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Column("telegram_user_id")]
    public long TelegramUserId { get; set; }

    [Column("telegram_user_name")]
    [MaxLength(256)]
    public string TelegramUserName { get; set; } = string.Empty;

    [Column("telegram_user_nickname")]
    [MaxLength(256)]
    public string? TelegramUserNickname { get; set; }

    [Column("is_accepted")]
    public bool IsAccepted { get; set; } = false;

    [Column("is_active")]
    public bool IsActive { get; set; } = true;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

[Table("usage")]
public class Usage
{
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(UserId))]
    public User? User { get; set; }
}