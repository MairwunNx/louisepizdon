using System.Text.Json;

namespace Louisepizdon.Platform;

public class AppConfig
{
    public TelegramConfig Telegram { get; set; } = new();
    public RedisConfig Redis { get; set; } = new();
    public DatabaseConfig Database { get; set; } = new();
    public ArtificialConfig Artificial { get; set; } = new();
    public LimitsConfig Limits { get; set; } = new();
    public ProxyConfig? Proxy { get; set; }
}

public class TelegramConfig
{
    public string BotToken { get; set; } = string.Empty;
}

public class RedisConfig
{
    public string ConnectionString { get; set; } = "localhost:6379";
}

public class DatabaseConfig
{
    public string ConnectionString { get; set; } = "Data Source=louisepizdon.db";
}

public class ArtificialConfig
{
    public string OpenAIToken { get; set; } = string.Empty;
    public string ChatModel { get; set; } = "gpt-4o";
    public string VisionPromptBase64 { get; set; } = string.Empty;
    
    public string GetVisionPrompt()
    {
        try
        {
            var decoded = Convert.FromBase64String(VisionPromptBase64);
            return System.Text.Encoding.UTF8.GetString(decoded);
        }
        catch
        {
            return GetDefaultVisionPrompt();
        }
    }

    private static string GetDefaultVisionPrompt()
    {
        return @"Ты — остроумный эксперт-оценщик изображений.

Твоя задача: проанализировать фото и составить детальный ценовой разбор всех видимых предметов, одежды и аксессуаров с остроумными комментариями.

Требования:
1. Перечисли все видимые предметы и элементы одежды
2. Укажи реалистичную примерную стоимость в рублях
3. Добавь к каждому предмету ироничный, но не оскорбительный комментарий
4. Можешь добавить пару ""бесценных"" элементов для юмора
5. Будь креативным, но уважительным

Формат ответа в markdown:
**Предмет описание** — цена ₽
*Комментарий*

---
Итого: ~общая сумма ₽";
    }
}

public class LimitsConfig
{
    public int DailyLimit { get; set; } = 5;
    public int MonthlyLimit { get; set; } = 30;
}

public class ProxyConfig
{
    public string? Host { get; set; }
    public int Port { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
}

public static class ConfigLoader
{
    public static AppConfig Load()
    {
        DotNetEnv.Env.Load();
        
        var configJson = Environment.GetEnvironmentVariable("APPLICATION_CONFIG");
        
        if (string.IsNullOrWhiteSpace(configJson))
        {
            throw new InvalidOperationException("APPLICATION_CONFIG environment variable is required");
        }

        var config = JsonSerializer.Deserialize<AppConfig>(configJson, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (config == null)
        {
            throw new InvalidOperationException("Failed to deserialize APPLICATION_CONFIG");
        }

        return config;
    }
}