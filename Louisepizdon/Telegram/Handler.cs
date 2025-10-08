using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Louisepizdon.Persistence;
using Louisepizdon.Artificial;
using Louisepizdon.Throttler;
using Louisepizdon.Texting;
using Louisepizdon.Tracing;

namespace Louisepizdon.Telegram;

public class BotUpdateHandler : IUpdateHandler
{
    private readonly IUserRepository _userRepository;
    private readonly IUsageRepository _usageRepository;
    private readonly IUsageLimiter _limiter;
    private readonly IAIAgent _agent;
    private readonly IAppLogger _logger;

    public BotUpdateHandler(
        IUserRepository userRepository,
        IUsageRepository usageRepository,
        IUsageLimiter limiter,
        IAIAgent agent,
        IAppLogger logger)
    {
        _userRepository = userRepository;
        _usageRepository = usageRepository;
        _limiter = limiter;
        _agent = agent;
        _logger = logger;
    }

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        try
        {
            switch (update.Type)
            {
                case UpdateType.Message when update.Message != null:
                    await HandleMessageAsync(botClient, update.Message, cancellationToken);
                    break;
                
                case UpdateType.CallbackQuery when update.CallbackQuery != null:
                    await HandleCallbackQueryAsync(botClient, update.CallbackQuery, cancellationToken);
                    break;
            }
        }
        catch (Exception ex)
        {
            _logger.Error("Error handling update {UpdateId}", ex, update.Id);
        }
    }

    private async Task HandleMessageAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        if (message.From == null) return;

        var telegramUserId = message.From.Id;
        var userName = $"{message.From.FirstName} {message.From.LastName}".Trim();
        var nickname = message.From.Username;

        // Get or create user
        var user = await _userRepository.GetByTelegramIdAsync(telegramUserId);

        if (user == null)
        {
            user = await _userRepository.CreateAsync(telegramUserId, userName, nickname);
            await SendWelcomeMessageAsync(botClient, message.Chat.Id, cancellationToken);
            return;
        }

        // Check if user is banned
        if (!user.IsActive)
        {
            await botClient.SendTextMessageAsync(
                message.Chat.Id,
                Phrases.UserBanned,
                cancellationToken: cancellationToken
            );
            return;
        }

        // Handle commands
        if (message.Type == MessageType.Text && message.Text != null)
        {
            if (message.Text.StartsWith("/start"))
            {
                if (!user.IsAccepted)
                {
                    await SendWelcomeMessageAsync(botClient, message.Chat.Id, cancellationToken);
                }
                else
                {
                    await botClient.SendTextMessageAsync(
                        message.Chat.Id,
                        Phrases.AgreementAccepted,
                        cancellationToken: cancellationToken
                    );
                }
                return;
            }

            if (message.Text.StartsWith("/help"))
            {
                var helpText = Phrases.FormatHelp(5, 30); // From config
                await botClient.SendTextMessageAsync(
                    message.Chat.Id,
                    helpText,
                    parseMode: ParseMode.Markdown,
                    cancellationToken: cancellationToken
                );
                return;
            }
        }

        // Check if user accepted terms
        if (!user.IsAccepted)
        {
            await botClient.SendTextMessageAsync(
                message.Chat.Id,
                Phrases.AgreementRequired,
                cancellationToken: cancellationToken
            );
            return;
        }

        // Handle photo
        if (message.Type == MessageType.Photo && message.Photo != null)
        {
            await HandlePhotoAsync(botClient, message, user, cancellationToken);
            return;
        }

        // Default response
        await botClient.SendTextMessageAsync(
            message.Chat.Id,
            Phrases.PhotoRequired,
            cancellationToken: cancellationToken
        );
    }

    private async Task HandlePhotoAsync(ITelegramBotClient botClient, Message message, User user, CancellationToken cancellationToken)
    {
        try
        {
            // Check limits
            var limitCheck = await _limiter.CheckAndIncrementAsync(user.TelegramUserId);
            
            if (limitCheck.Exceeded)
            {
                var limitMessage = limitCheck.IsDaily
                    ? Phrases.FormatDailyLimit(limitCheck.CurrentCount, limitCheck.Limit)
                    : Phrases.FormatMonthlyLimit(limitCheck.CurrentCount, limitCheck.Limit);

                await botClient.SendTextMessageAsync(
                    message.Chat.Id,
                    limitMessage,
                    cancellationToken: cancellationToken
                );
                return;
            }

            // Send "processing" message
            await botClient.SendTextMessageAsync(
                message.Chat.Id,
                Phrases.PhotoReceived,
                cancellationToken: cancellationToken
            );

            // Get photo file
            var photo = message.Photo.OrderByDescending(p => p.FileSize).First();
            var file = await botClient.GetFileAsync(photo.FileId, cancellationToken);

            if (file.FilePath == null)
            {
                throw new InvalidOperationException("File path is null");
            }

            var fileUrl = $"https://api.telegram.org/file/bot{botClient.BotToken}/{file.FilePath}";

            // Analyze image
            var analysis = await _agent.AnalyzeImageAsync(fileUrl);

            // Send result
            await botClient.SendTextMessageAsync(
                message.Chat.Id,
                analysis,
                parseMode: ParseMode.Markdown,
                cancellationToken: cancellationToken
            );

            // Record usage
            await _usageRepository.AddUsageAsync(user.Id);
        }
        catch (Exception ex)
        {
            _logger.Error("Error handling photo for user {UserId}", ex, user.Id);
            
            await botClient.SendTextMessageAsync(
                message.Chat.Id,
                Phrases.PhotoProcessingError,
                cancellationToken: cancellationToken
            );
        }
    }

    private async Task HandleCallbackQueryAsync(ITelegramBotClient botClient, CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        if (callbackQuery.From == null || callbackQuery.Data == null) return;

        try
        {
            if (callbackQuery.Data == "accept_terms")
            {
                await _userRepository.UpdateAcceptanceAsync(callbackQuery.From.Id, true);

                await botClient.AnswerCallbackQueryAsync(
                    callbackQuery.Id,
                    "✅ Условия приняты!",
                    cancellationToken: cancellationToken
                );

                if (callbackQuery.Message != null)
                {
                    await botClient.SendTextMessageAsync(
                        callbackQuery.Message.Chat.Id,
                        Phrases.AgreementAccepted,
                        cancellationToken: cancellationToken
                    );
                }
            }
        }
        catch (Exception ex)
        {
            _logger.Error("Error handling callback query", ex);
        }
    }

    private async Task SendWelcomeMessageAsync(ITelegramBotClient botClient, long chatId, CancellationToken cancellationToken)
    {
        var keyboard = new InlineKeyboardMarkup(new[]
        {
            InlineKeyboardButton.WithCallbackData(Phrases.AcceptButton, "accept_terms")
        });

        await botClient.SendTextMessageAsync(
            chatId,
            Phrases.WelcomeMessage,
            parseMode: ParseMode.Markdown,
            replyMarkup: keyboard,
            cancellationToken: cancellationToken
        );
    }

    public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        _logger.Error("Telegram polling error", exception);
        return Task.CompletedTask;
    }
}