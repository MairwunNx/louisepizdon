using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Louisepizdon.Tracing;

namespace Louisepizdon.Telegram;

public interface IBotPoller
{
    Task StartAsync(CancellationToken cancellationToken);
}

public class BotPoller : IBotPoller
{
    private readonly TelegramBotClient _client;
    private readonly IUpdateHandler _handler;
    private readonly IAppLogger _logger;

    public BotPoller(
        TelegramBotClient client,
        IUpdateHandler handler,
        IAppLogger logger)
    {
        _client = client;
        _handler = handler;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            var me = await _client.GetMeAsync(cancellationToken);
            _logger.Info("Bot started: @{BotUsername} ({BotId})", me.Username, me.Id);

            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = [UpdateType.Message, UpdateType.CallbackQuery],
                ThrowPendingUpdates = true
            };

            await _client.ReceiveAsync(
                _handler,
                receiverOptions,
                cancellationToken
            );
        }
        catch (Exception ex)
        {
            _logger.Error("Error in bot poller", ex);
            throw;
        }
    }
}