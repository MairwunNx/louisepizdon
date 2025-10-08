# ⚡ Quick Start Guide

Быстрый запуск Louisepizdon за 5 минут!

## 📋 Чек-лист перед запуском

- [ ] Установлен Docker и docker-compose
- [ ] Получен Telegram Bot Token от [@BotFather](https://t.me/botfather)
- [ ] Получен OpenAI API Key
- [ ] Redis доступен (или используйте docker-compose)

## 🚀 Запуск за 5 шагов

### 1. Клонируйте и перейдите в директорию

```bash
git clone <your-repo>
cd louisepizdon
```

### 2. Создайте промпт и закодируйте его

Используйте пример или создайте свой:

```bash
# Закодировать пример промпта
./scripts/encode-prompt.sh scripts/example-prompt.txt

# Или свой файл
./scripts/encode-prompt.sh my-custom-prompt.txt
```

Скопируйте полученный Base64 string.

### 3. Создайте `.env` файл

```bash
cat > .env << 'EOF'
TZ=Europe/Moscow
APPLICATION_CONFIG={"telegram":{"botToken":"YOUR_BOT_TOKEN"},"redis":{"connectionString":"redis:6379"},"database":{"connectionString":"Data Source=data/louisepizdon.db"},"artificial":{"openAIToken":"YOUR_OPENAI_KEY","chatModel":"gpt-4o","visionPromptBase64":"YOUR_BASE64_PROMPT"},"limits":{"dailyLimit":5,"monthlyLimit":30},"proxy":null}
EOF
```

**Важно:** Замените:
- `YOUR_BOT_TOKEN` - ваш Telegram bot token
- `YOUR_OPENAI_KEY` - ваш OpenAI API key  
- `YOUR_BASE64_PROMPT` - ваш Base64 промпт из шага 2

### 4. Запустите через Docker Compose

```bash
docker-compose up -d
```

### 5. Проверьте работу

```bash
# Просмотр логов
docker-compose logs -f bot

# Должны увидеть:
# [13:00:00 INF] Bot started: @YourBot (123456789)
# [13:00:00 INF] ✅ Louisepizdon успешно запущен
```

## 🎉 Готово!

Теперь найдите вашего бота в Telegram и отправьте `/start`

## 🔧 Быстрая настройка без Docker

Если Docker не доступен:

### 1. Установите зависимости

```bash
# .NET 9.0 SDK
wget https://dot.net/v1/dotnet-install.sh
chmod +x dotnet-install.sh
./dotnet-install.sh --channel 9.0

# Redis
sudo apt-get install redis-server
sudo systemctl start redis
```

### 2. Создайте `.env` с локальной конфигурацией

```bash
cat > .env << 'EOF'
TZ=Europe/Moscow
APPLICATION_CONFIG={"telegram":{"botToken":"YOUR_BOT_TOKEN"},"redis":{"connectionString":"localhost:6379"},"database":{"connectionString":"Data Source=louisepizdon.db"},"artificial":{"openAIToken":"YOUR_OPENAI_KEY","chatModel":"gpt-4o","visionPromptBase64":"YOUR_BASE64_PROMPT"},"limits":{"dailyLimit":5,"monthlyLimit":30},"proxy":null}
EOF
```

### 3. Запустите

```bash
cd Louisepizdon
dotnet restore
dotnet run
```

## 🌐 Настройка прокси (опционально)

Если нужен SOCKS5 прокси:

```json
{
  "proxy": {
    "host": "proxy.example.com",
    "port": 1080,
    "username": "user",
    "password": "pass"
  }
}
```

Замените `null` на этот объект в `APPLICATION_CONFIG`.

## 📊 Полезные команды

```bash
# Остановить бота
docker-compose down

# Перезапустить
docker-compose restart bot

# Логи в реальном времени
docker-compose logs -f bot

# Очистить все (включая volumes)
docker-compose down -v

# Зайти в Redis
docker-compose exec redis redis-cli

# Посмотреть лимиты пользователя
docker-compose exec redis redis-cli keys "usage:*"
docker-compose exec redis redis-cli get "usage:vision:daily:123456789:2025-10-08"
```

## 🗄️ Доступ к базе данных

```bash
# Если используете Docker
docker-compose exec bot ls -la /app/data/

# Локально
sqlite3 louisepizdon.db "SELECT * FROM users;"
sqlite3 louisepizdon.db "SELECT * FROM usage;"
```

## ❗ Troubleshooting

### Бот не запускается

```bash
# Проверьте логи
docker-compose logs bot

# Проверьте что Redis запущен
docker-compose ps redis

# Проверьте конфигурацию
cat .env
```

### Redis connection refused

```bash
# Если запускаете локально без Docker
redis-cli ping
# Должно вернуть: PONG

# Если Redis не запущен
sudo systemctl start redis
```

### OpenAI API ошибки

```bash
# Проверьте баланс: https://platform.openai.com/usage
# Проверьте что ключ правильный
# Проверьте что модель доступна (gpt-4o)
```

## 📖 Дальше

- Прочитайте [SETUP.md](SETUP.md) для детальной настройки
- Изучите [ARCHITECTURE.md](ARCHITECTURE.md) для понимания структуры
- См. [CONTRIBUTING.md](CONTRIBUTING.md) для разработки

## 💬 Поддержка

Telegram: [@MairwunNx](https://t.me/MairwunNx)

---

**Happy coding! 🥀**