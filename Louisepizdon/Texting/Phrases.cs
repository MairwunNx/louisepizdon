namespace Louisepizdon.Texting;

public static class Phrases
{
    // Welcome & Agreement
    public const string WelcomeMessage = @"🌹 **Добро пожаловать в Louisepizdon!**

Я — ваш персональный AI-оценщик изображений! 

📸 Отправьте мне фото, и я предоставлю детальный ценовой разбор всех видимых предметов с остроумными комментариями.

⚠️ **Важное соглашение:**
Вся характеризация изображений является AI-генерируемым контентом и может содержать неточности, ироничные замечания и не всегда соответствовать действительности. Пожалуйста, воспринимайте это как развлечение и не принимайте близко к сердцу.

Нажмите кнопку ниже, чтобы принять условия и начать использование.";

    public const string AcceptButton = "✅ Принять условия";
    
    public const string AgreementAccepted = "✅ Отлично! Теперь вы можете отправлять мне фотографии для анализа.";
    
    public const string AgreementRequired = "⚠️ Для использования бота необходимо принять условия. Используйте /start";
    
    // Photo Processing
    public const string PhotoReceived = "📸 Фото получено! Анализирую изображение...";
    
    public const string PhotoRequired = "📸 Пожалуйста, отправьте фотографию для анализа.";
    
    public const string PhotoProcessingError = "❌ Произошла ошибка при обработке изображения. Пожалуйста, попробуйте позже.";
    
    // Limits
    public const string DailyLimitExceeded = "🚫 Вы исчерпали дневной лимит использования ({0}/{1}). Попробуйте завтра!";
    
    public const string MonthlyLimitExceeded = "🚫 Вы исчерпали месячный лимит использования ({0}/{1}). Попробуйте в следующем месяце!";
    
    // Errors
    public const string InternalError = "❌ Произошла внутренняя ошибка. Пожалуйста, попробуйте позже.";
    
    public const string UserBanned = "🚫 Ваш аккаунт заблокирован. Обратитесь к администратору.";
    
    // Status
    public const string BotStarted = "✅ Louisepizdon успешно запущен";
    
    public const string BotStopped = "⏹️ Louisepizdon остановлен";
    
    // Help
    public const string HelpMessage = @"📖 **Справка по боту Louisepizdon**

**Команды:**
/start - Начать работу с ботом
/help - Показать эту справку

**Как использовать:**
1. Примите условия использования (при первом запуске)
2. Отправьте фотографию
3. Получите детальный анализ с ценами и комментариями

💡 Лимиты: {0} фото в день, {1} фото в месяц";

    public static string FormatDailyLimit(int current, int max) => 
        string.Format(DailyLimitExceeded, current, max);
    
    public static string FormatMonthlyLimit(int current, int max) => 
        string.Format(MonthlyLimitExceeded, current, max);
    
    public static string FormatHelp(int dailyLimit, int monthlyLimit) => 
        string.Format(HelpMessage, dailyLimit, monthlyLimit);
}