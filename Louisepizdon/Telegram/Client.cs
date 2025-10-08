using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using MihaZupan;
using Louisepizdon.Platform;
using Louisepizdon.Tracing;

namespace Louisepizdon.Telegram;

public interface ITelegramBotClient : ITelegramBotClient
{
}

public static class TelegramClientFactory
{
    public static TelegramBotClient Create(AppConfig config, IAppLogger logger)
    {
        try
        {
            logger.Info("Creating Telegram bot client");

            if (config.Proxy != null && !string.IsNullOrEmpty(config.Proxy.Host))
            {
                logger.Info("Using SOCKS5 proxy: {Host}:{Port}", config.Proxy.Host, config.Proxy.Port);

                var proxy = new HttpToSocks5Proxy(
                    config.Proxy.Host,
                    config.Proxy.Port,
                    config.Proxy.Username,
                    config.Proxy.Password
                );

                var httpClient = new HttpClient(
                    new HttpClientHandler { Proxy = proxy, UseProxy = true }
                );

                return new TelegramBotClient(config.Telegram.BotToken, httpClient);
            }
            else
            {
                logger.Info("Creating Telegram bot client without proxy");
                return new TelegramBotClient(config.Telegram.BotToken);
            }
        }
        catch (Exception ex)
        {
            logger.Error("Failed to create Telegram bot client", ex);
            throw;
        }
    }
}