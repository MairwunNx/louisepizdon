# 🚀 Louisepizdon Setup Guide

## Быстрый старт

### 1. Получите необходимые токены

#### Telegram Bot Token
1. Откройте [@BotFather](https://t.me/botfather) в Telegram
2. Отправьте `/newbot`
3. Следуйте инструкциям
4. Сохраните полученный токен

#### OpenAI API Key
1. Зарегистрируйтесь на [platform.openai.com](https://platform.openai.com)
2. Перейдите в [API Keys](https://platform.openai.com/api-keys)
3. Создайте новый ключ
4. Сохраните ключ (он показывается только один раз!)

### 2. Подготовьте промпт для AI

Создайте файл с промптом (например `my-prompt.txt`) или используйте пример из `scripts/example-prompt.txt`.

Закодируйте промпт в base64:
```bash
chmod +x scripts/encode-prompt.sh
./scripts/encode-prompt.sh scripts/example-prompt.txt
```

Или вручную:
```bash
base64 -w 0 my-prompt.txt
```

### 3. Создайте конфигурацию

Скопируйте пример:
```bash
cp .env.example .env
```

Создайте JSON конфигурацию (замените значения на свои):
```json
{
  "telegram": {
    "botToken": "1234567890:ABCdefGHIjklMNOpqrsTUVwxyz"
  },
  "redis": {
    "connectionString": "localhost:6379"
  },
  "database": {
    "connectionString": "Data Source=data/louisepizdon.db"
  },
  "artificial": {
    "openAIToken": "sk-proj-...",
    "chatModel": "gpt-4o",
    "visionPromptBase64": "ВАШBASE64ПРОМПТ"
  },
  "limits": {
    "dailyLimit": 5,
    "monthlyLimit": 30
  },
  "proxy": null
}
```

**Важно:** JSON должен быть в одну строку без переносов!

Можно использовать `jq`:
```bash
cat config.json | jq -c '.' > config-oneline.json
```

Добавьте в `.env`:
```bash
TZ=Europe/Moscow
APPLICATION_CONFIG='{"telegram":{"botToken":"..."},...}'
```

### 4. (Опционально) Настройте прокси

Если нужен SOCKS5 прокси для Telegram API:

```json
{
  ...
  "proxy": {
    "host": "proxy.example.com",
    "port": 1080,
    "username": "user",
    "password": "pass"
  }
}
```

Для HTTP прокси измените только хост и порт.

### 5. Запустите бота

#### С Docker (рекомендуется):

```bash
docker-compose up -d
```

Логи:
```bash
docker-compose logs -f bot
```

Остановка:
```bash
docker-compose down
```

#### Без Docker:

Требования:
- .NET 9.0 SDK
- Redis сервер

```bash
# Установите зависимости
dotnet restore Louisepizdon/Louisepizdon.csproj

# Запустите Redis (если не запущен)
redis-server

# Запустите бота
cd Louisepizdon
dotnet run
```

### 6. Проверьте работу

1. Найдите вашего бота в Telegram
2. Отправьте `/start`
3. Примите условия
4. Отправьте фотографию
5. Получите анализ! 🎉

## 🔧 Настройка лимитов

Измените значения в конфигурации:

```json
{
  "limits": {
    "dailyLimit": 10,    // 10 фото в день на пользователя
    "monthlyLimit": 100  // 100 фото в месяц на пользователя
  }
}
```

Лимиты хранятся в Redis и сбрасываются автоматически.

## 🗄️ База данных

База данных SQLite создается автоматически при первом запуске в:
- Docker: `/app/data/louisepizdon.db`
- Локально: `louisepizdon.db` (или путь из конфигурации)

Миграции применяются автоматически при старте.

## 📊 Мониторинг

### Логи

Логи сохраняются в:
- Консоль (stdout)
- Файл: `logs/louisepizdon-YYYY-MM-DD.log`

Формат логов - структурированный (JSON).

### Redis

Проверить подключение:
```bash
redis-cli ping
```

Посмотреть лимиты пользователя:
```bash
redis-cli keys "usage:vision:*"
redis-cli get "usage:vision:daily:123456789:2025-10-08"
```

## 🔒 Безопасность

1. **Не коммитьте `.env` файл!** Он в `.gitignore`
2. **Храните токены в безопасности**
3. **Используйте сильные пароли для Redis** в продакшене
4. **Ограничьте доступ к базе данных**

## 🐛 Troubleshooting

### Бот не отвечает
1. Проверьте токен бота
2. Проверьте подключение к Redis
3. Проверьте логи: `docker-compose logs -f bot`

### Ошибка OpenAI API
1. Проверьте API ключ
2. Проверьте баланс на аккаунте OpenAI
3. Убедитесь что модель доступна (gpt-4o)

### База данных не создается
1. Проверьте права на запись в директорию
2. Проверьте путь в конфигурации
3. Посмотрите логи миграций

### Redis connection refused
1. Убедитесь что Redis запущен
2. Проверьте connectionString в конфигурации
3. Для Docker: используйте имя сервиса `redis:6379`

## 📚 Дополнительно

- [Telegram Bot API Docs](https://core.telegram.org/bots/api)
- [OpenAI API Docs](https://platform.openai.com/docs)
- [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/)

## 💡 Советы

1. **Тестирование промпта**: Отправляйте разные фото и корректируйте промпт
2. **Лимиты**: Начните с малых лимитов и увеличивайте по необходимости
3. **Мониторинг**: Следите за использованием OpenAI API и стоимостью
4. **Backup**: Регулярно делайте бэкап базы данных SQLite

Удачи! 🥀