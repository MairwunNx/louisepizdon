# 🏗️ Louisepizdon Architecture

## Обзор системы

Louisepizdon — это Telegram-бот для анализа изображений с использованием AI, построенный на модульной архитектуре с применением современных паттернов .NET.

## 📊 Диаграмма компонентов

```
┌─────────────────────────────────────────────────────────────────┐
│                         Program.cs (Host)                        │
│                    Microsoft.Extensions.Hosting                  │
└───────────────────────────────┬─────────────────────────────────┘
                                │
                                │ Dependency Injection
                                │
        ┌───────────────────────┴───────────────────────┐
        │                                               │
        ▼                                               ▼
┌───────────────┐                              ┌────────────────┐
│   Telegram    │◄────────────────────────────►│   Artificial   │
│   (Bot API)   │                              │  (AI Agent)    │
└───────┬───────┘                              └────────────────┘
        │                                               │
        │ Uses                                          │ Uses
        │                                               │
        ▼                                               ▼
┌───────────────┐         ┌───────────────┐    ┌────────────────┐
│ Persistence   │         │   Throttler   │    │    Caching     │
│  (Database)   │         │   (Limits)    │    │    (Redis)     │
└───────────────┘         └───────┬───────┘    └────────────────┘
        │                         │                     │
        │                         └─────────────────────┘
        │                               Uses
        ▼
┌───────────────┐         ┌───────────────┐    ┌────────────────┐
│   Platform    │         │    Texting    │    │    Tracing     │
│   (Config)    │         │   (Phrases)   │    │   (Logging)    │
└───────────────┘         └───────────────┘    └────────────────┘
```

## 🧩 Модули

### Platform
**Назначение:** Базовая конфигурация и системные утилиты

**Файлы:**
- `Config.cs` - Классы конфигурации и загрузчик из JSON
- `Manifest.cs` - Информация о версии и времени запуска

**Зависимости:** DotNetEnv

**Используется:** Всеми модулями

### Persistence
**Назначение:** Управление базой данных SQLite

**Файлы:**
- `Entities.cs` - Сущности User и Usage
- `Database.cs` - AppDbContext для Entity Framework Core
- `Migrations.cs` - Автоматические миграции
- `Repository.cs` - Репозитории для работы с данными

**Зависимости:** 
- Microsoft.EntityFrameworkCore.Sqlite
- Platform (конфигурация)
- Tracing (логирование)

**Паттерны:** Repository, Unit of Work

### Caching
**Назначение:** Redis клиент для кеширования

**Файлы:**
- `Redis.cs` - Обертка над StackExchange.Redis

**Зависимости:**
- StackExchange.Redis
- Platform (конфигурация)
- Tracing (логирование)

### Throttler
**Назначение:** Управление лимитами использования

**Файлы:**
- `Limiter.cs` - Проверка и инкремент дневных/месячных лимитов

**Зависимости:**
- Caching (Redis)
- Platform (конфигурация)
- Tracing (логирование)

**Алгоритм:**
1. Проверить месячный лимит в Redis
2. Проверить дневной лимит в Redis
3. Инкрементировать оба счетчика атомарно
4. Установить TTL для автоматического сброса

**Ключи Redis:**
- `usage:vision:daily:{userId}:{yyyy-MM-dd}` (TTL: 25 часов)
- `usage:vision:monthly:{userId}:{yyyy-MM}` (TTL: 32 дня)

### Artificial
**Назначение:** Интеграция с Microsoft Agent Framework и OpenAI

**Файлы:**
- `Agent.cs` - VisionAgent для анализа изображений

**Зависимости:**
- Microsoft.Agents.AI.OpenAI
- OpenAI SDK
- Platform (конфигурация)
- Tracing (логирование)

**Процесс анализа:**
1. Создать ChatMessage с текстом и URL изображения
2. Отправить в AI Agent с инструкциями
3. Получить markdown ответ
4. Вернуть результат

### Telegram
**Назначение:** Взаимодействие с Telegram Bot API

**Файлы:**
- `Client.cs` - Фабрика TelegramBotClient с поддержкой прокси
- `Poller.cs` - Long polling для получения обновлений
- `Handler.cs` - Обработка сообщений и callback query

**Зависимости:**
- Telegram.Bot
- Telegram.Bot.Extensions.Socks5
- Persistence (пользователи и usage)
- Throttler (проверка лимитов)
- Artificial (анализ изображений)
- Texting (сообщения)
- Tracing (логирование)

**Поток обработки сообщения:**
```
Message → Handler → Check/Create User → Check Ban
    ↓
Is Accepted? → No → Send Welcome
    ↓ Yes
Is Photo? → No → Send Help
    ↓ Yes
Check Limits → Exceeded → Send Limit Message
    ↓ OK
Get Photo URL → AI Analysis → Send Result → Record Usage
```

### Texting
**Назначение:** Константы сообщений бота

**Файлы:**
- `Phrases.cs` - Все текстовые сообщения с эмодзи

**Особенности:**
- Централизованное хранение текстов
- Поддержка форматирования с параметрами
- Markdown разметка

### Tracing
**Назначение:** Структурное логирование

**Файлы:**
- `Logger.cs` - Обертка над Serilog

**Зависимости:**
- Serilog
- Serilog.Sinks.Console
- Serilog.Sinks.File

**Уровни логирования:**
- Debug - детальная отладочная информация
- Info - общая информация о работе
- Warning - предупреждения
- Error - ошибки с исключениями

## 🔄 Жизненный цикл приложения

```
1. Program.Main()
   ↓
2. Load .env (DotNetEnv)
   ↓
3. Configure Serilog
   ↓
4. Load APPLICATION_CONFIG (JSON)
   ↓
5. Build Host with DI
   ↓
6. Run Database Migrations
   ↓
7. Start BotHostedService
   ↓
8. Start Long Polling
   ↓
9. Handle Updates (loop)
   ↓
10. On Shutdown → Cleanup
```

## 🎯 Dependency Injection

Все компоненты регистрируются в DI контейнере:

```csharp
services.AddSingleton<AppConfig>(config);          // Конфигурация
services.AddSingleton<IAppLogger, AppLogger>();    // Логирование
services.AddSingleton<AppDbContext>();             // База данных
services.AddSingleton<IUserRepository>();          // Репозитории
services.AddSingleton<IRedisClient, RedisClient>(); // Redis
services.AddSingleton<IUsageLimiter>();            // Лимиты
services.AddSingleton<IAIAgent, VisionAgent>();    // AI
services.AddSingleton<TelegramBotClient>();        // Telegram
services.AddSingleton<IBotPoller>();               // Polling
```

## 🔐 Безопасность

### Данные пользователей
- Хранятся только необходимые данные (ID, имя, ник)
- SQLite файл должен быть защищен правами доступа
- Нет хранения изображений

### Токены и ключи
- Хранятся в переменных окружения
- Не логируются
- Не коммитятся в репозиторий

### Redis
- Используется только для счетчиков
- Данные временные (TTL)
- В продакшене должен быть защищен паролем

## 📈 Масштабирование

### Горизонтальное
- Можно запустить несколько инстансов бота
- Telegram API автоматически распределит обновления
- Redis используется для синхронизации лимитов
- База данных SQLite - bottleneck (см. вертикальное)

### Вертикальное
- Для больших нагрузок заменить SQLite на PostgreSQL
- Использовать Connection Pooling для Redis
- Добавить кеширование пользователей в памяти

### Производительность
- Async/await везде для I/O операций
- Redis для быстрого доступа к лимитам
- Long polling вместо webhook (проще в деплое)

## 🔮 Будущие улучшения

1. **Мониторинг**
   - Prometheus metrics
   - Healthcheck endpoint
   - Application Insights

2. **Тестирование**
   - Unit tests с xUnit
   - Integration tests
   - Test coverage > 80%

3. **Функциональность**
   - Поддержка видео
   - Голосовые сообщения
   - Multi-language support

4. **Инфраструктура**
   - Kubernetes deployment
   - CI/CD pipeline
   - Автоматический rollback

## 📚 Ссылки

- [.NET Documentation](https://docs.microsoft.com/dotnet/)
- [Telegram Bot API](https://core.telegram.org/bots/api)
- [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/)
- [Entity Framework Core](https://docs.microsoft.com/ef/core/)
- [Serilog](https://serilog.net/)