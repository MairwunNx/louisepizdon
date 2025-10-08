# 🥀 Louisepizdon

[![AI Capable](https://img.shields.io/badge/AI-Capable-brightgreen?style=flat&logo=openai&logoColor=white)](https://github.com/mairwunnx/louisepizdon)
[![Docker](https://img.shields.io/badge/Docker-Available-2496ED?style=flat&logo=docker&logoColor=white)](https://github.com/MairwunNx/louisepizdon/pkgs/container/louisepizdon)
[![GitHub Release](https://img.shields.io/github/v/release/mairwunnx/louisepizdon?style=flat&logo=github&color=blue)](https://github.com/mairwunnx/louisepizdon/releases)
[![.NET Version](https://img.shields.io/badge/.NET-9.0+-512BD4?style=flat&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)

**Louisepizdon** — 🥀 Telegram-бот с ИИ, который честнее чем твоя бабушка. Оценит тебя по достоинству, разборка ценообразования твоих шмоток с фотографии!

> **Внимание**: Это развлекательный проект, созданный для юмора и развлечения. Все оценки генерируются ИИ и носят шуточный характер.

## Фичи

### Основной функционал

- **Анализ изображений с AI** — отправьте фото, получите подробный разбор ценообразования с юмором.
- **Vision AI** — мультимодальный анализ с использованием Microsoft.Agents.AI.
- **Остроумные комментарии** — бот не просто оценивает, но и добавляет забавные комментарии.
- **Быстрая обработка** — результат за секунды.

## Использование

### Начало работы
1. Найдите бота в Telegram: `@louisepizdon_bot`
2. Начните диалог командой `/start`
3. Отправьте фотографию
4. Получите забавный разбор ценообразования!

## Деплоймент

### Dev Containers (рекомендуется для разработки)

Самый простой способ запуска для разработки — использование Dev Containers в VS Code или аналогичных IDE:

1. Клонируйте репозиторий:
```bash
git clone https://github.com/mairwunnx/louisepizdon
cd louisepizdon
```

2. Настройте обязательные переменные окружения (в файле `.env.dev`):
```bash
TELEGRAM_BOT_TOKEN="ваш_telegram_bot_token"
OPENROUTER_API_KEY="ваш_openrouter_api_key"

REDIS_HOST="louisepizdon-redis"
REDIS_PORT="6379"
REDIS_PASSWORD="ваш_redis_пароль"
```

3. Откройте проект в VS Code и выберите "Reopen in Container"

Всё остальное (Redis, зависимости) настроится автоматически. Просто и быстро!

### Docker Compose (для продакшена)

1. Клонируйте репозиторий:
```bash
git clone https://github.com/mairwunnx/louisepizdon
cd louisepizdon
```

2. Настройте обязательные переменные окружения (в файле `.env`):
```bash
TELEGRAM_BOT_TOKEN="ваш_telegram_bot_token"
OPENROUTER_API_KEY="ваш_openrouter_api_key"

REDIS_HOST="louisepizdon-redis"
REDIS_PORT="6379"
REDIS_PASSWORD="ваш_redis_пароль"
```

3. Запустите сервисы:
```bash
docker compose up -d
```

4. Проверьте статус:
```bash
docker compose logs -f louisepizdon
```

### Docker Compose с готовым образом

Если хотите использовать готовый образ и управлять Redis самостоятельно:

1. Задекларируйте сервис louisepizdon в `docker-compose.yml`:

```yaml
services:
  louisepizdon:
    image: ghcr.io/mairwunnx/louisepizdon:0.1.0
    env_file: .env
    environment:
      TELEGRAM_BOT_TOKEN: ${TELEGRAM_BOT_TOKEN}
      OPENROUTER_API_KEY: ${OPENROUTER_API_KEY}
      REDIS_HOST: ${REDIS_HOST}
      REDIS_PORT: ${REDIS_PORT}
      REDIS_PASSWORD: ${REDIS_PASSWORD}
    restart: unless-stopped
```

> **Примечание**: Вам потребуется передать все переменные окружения из файла `.env` или любым другим удобным способом.

2. Задекларируйте Redis в `docker-compose.yml`. 
> Важно, версия **Redis** должна быть `8.0+`.

3. Запустите сервисы:
```bash
docker compose up -d
```

### Сборка из исходников

#### Ручной и прямой способ сборки

Требования:
- .NET SDK 9.0+

```bash
dotnet build -c Release
dotnet run -c Release --project Louisepizdon
```

#### Сборка с помощью Docker

Требования:
- Docker

```bash
docker build -t louisepizdon .
```

## Стек

- **.NET 9.0** — основной фреймворк разработки
- **C# 13** — современный язык программирования
- **Telegram.Bot** — интеграция с Telegram Bot API
- **Microsoft.Agents.AI** — AI Agent Framework для работы с LLM
- **Redis + StackExchange.Redis** — кэширование и rate limiting
- **Microsoft.Extensions.Logging** — structured logging
- **Docker + Docker Compose** — контейнеризация и оркестрация

## Участие AI

AI использовался для генерации промптов для анализа изображений, оптимизации архитектуры и создания документации.

## Ссылки на связанные проекты

[Xi Manager](https://github.com/mairwunnx/xi) — 🀄️ Telegram-бот с ИИ, стилизованный под личного помощника Xi. Личный помощник великого лидера, готовый отвечать на вопросы простого народа.

[Dickobrazz](https://github.com/mairwunnx/dickobrazz) — 🌶️ Дикобраз бот, он же дикобот, способен в точности до сантиметра выдать размер вашего агрегата. Современный и технологичный кокомер с системой сезонов и геймификацией.

## Из серии "от того же автора"

[Mo'Bosses](https://github.com/mairwunnx/mobosses) — 🏆 **Mo'Bosses** — это лучший RPG плагин, который превращает обычных мобов в эпических боссов с **продвинутой системой прогрессии игрока**. В отличие от других плагинов, здесь каждый бой имеет значение, а каждый уровень открывает новые возможности! ⚔

[Mo'Joins](https://github.com/mairwunnx/mojoins) — 🎉 Кастомные входы/выходы: сообщения, звуки, частицы, фейерверки и защита после входа. Все для PaperMC.

[Mo'Afks](https://github.com/mairwunnx/moafks) — 🛡️ Пауза в онлайне — теперь возможна. Плагин для PaperMC, который даёт игроку безопасный режим AFK: иммунитет к урону, отсутствие коллизий, игнор мобами, авто-детект неактивности и аккуратные визуальные эффекты.

[McBuddy Server](https://github.com/mcbuddy-ai/mcbuddy-server) — 🛠️⚡ Бэкенд для AI-ассистента MCBuddy с интеграцией OpenRouter и обработкой запросов

[McBuddy Telegram](https://github.com/mcbuddy-ai/mcbuddy-bot) — 🤖📱 Telegram-бот для общения с MCBuddy за пределами игры

[McBuddy Spigot](https://github.com/mcbuddy-ai/mcbuddy-spigot) — 💬 Spigot-плагин для интеграции MCBuddy — добавляет команду `/ask` для вопросов к AI-ассистенту прямо в чате Minecraft сервера! 🎮

---

![image](./media.png)

🇷🇺 **Сделано в России с любовью.** ❤️

**Louisepizdon** — это про юмор, AI и современные технологии. Оценим всё и всех!

> 🫡 Made by Pavel Erokhin (Павел Ерохин), aka mairwunnx.
