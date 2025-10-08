# 🥀 Louisepizdon

AI-powered Telegram bot that evaluates photos and provides funny, detailed pricing breakdowns of items/clothing in images with witty commentary.

## 🚀 Features

- 📸 **Image Analysis**: AI-powered photo evaluation using GPT-4 Vision
- 💰 **Price Breakdown**: Detailed itemized pricing with witty commentary
- 🔒 **User Agreement**: Terms acceptance flow for new users
- 📊 **Usage Limits**: Daily and monthly limits per user
- 🗄️ **SQLite Database**: User management and usage tracking
- 🔴 **Redis Caching**: High-performance limit tracking
- 🌐 **Proxy Support**: HTTP/SOCKS5 proxy for Telegram API
- 📝 **Structured Logging**: Comprehensive logging with Serilog

## 🛠️ Tech Stack

- **.NET 9.0** - Modern C# runtime
- **Telegram.Bot** - Telegram Bot API client
- **Microsoft.Agents.AI** - AI Agent Framework with OpenAI
- **Entity Framework Core** - SQLite database with migrations
- **StackExchange.Redis** - Redis caching
- **Serilog** - Structured logging
- **DotNetEnv** - Environment configuration

## 📦 Setup

### Prerequisites

- .NET 9.0 SDK
- Redis server
- Telegram Bot Token (from [@BotFather](https://t.me/botfather))
- OpenAI API Key

### Installation

1. Clone the repository:
```bash
git clone <repository-url>
cd Louisepizdon
```

2. Copy and configure `.env`:
```bash
cp .env.example .env
```

3. Update `APPLICATION_CONFIG` in `.env` with your credentials:
```json
{
  "telegram": {
    "botToken": "YOUR_BOT_TOKEN"
  },
  "redis": {
    "connectionString": "localhost:6379"
  },
  "database": {
    "connectionString": "Data Source=louisepizdon.db"
  },
  "artificial": {
    "openAIToken": "YOUR_OPENAI_API_KEY",
    "chatModel": "gpt-4o",
    "visionPromptBase64": "BASE64_ENCODED_PROMPT"
  },
  "limits": {
    "dailyLimit": 5,
    "monthlyLimit": 30
  },
  "proxy": null
}
```

4. Restore packages:
```bash
dotnet restore
```

5. Run the bot:
```bash
dotnet run
```

## 🐳 Docker

Build and run with Docker Compose:

```bash
docker-compose up -d
```

## 📖 Usage

1. Start the bot: `/start`
2. Accept terms and conditions
3. Send a photo
4. Receive detailed price breakdown with witty commentary

## 🏗️ Project Structure

```
Louisepizdon/
├── Platform/         # Configuration and utilities
├── Persistence/      # Database entities and repositories
├── Caching/          # Redis client
├── Throttler/        # Usage limits
├── Artificial/       # AI Agent integration
├── Telegram/         # Telegram bot logic
├── Texting/          # Message constants
├── Tracing/          # Logging
└── Program.cs        # Application entry point
```

## 🔧 Configuration

Configuration is provided via `APPLICATION_CONFIG` environment variable as JSON:

- `telegram.botToken` - Telegram bot token
- `redis.connectionString` - Redis connection string
- `database.connectionString` - SQLite connection string
- `artificial.openAIToken` - OpenAI API key
- `artificial.chatModel` - OpenAI model (e.g., gpt-4o)
- `artificial.visionPromptBase64` - Base64-encoded system prompt
- `limits.dailyLimit` - Daily usage limit per user
- `limits.monthlyLimit` - Monthly usage limit per user
- `proxy` - Optional SOCKS5 proxy configuration

## 📊 Database Schema

### Users Table
- `id` - Auto-increment primary key
- `telegram_user_id` - Telegram user ID
- `telegram_user_name` - User full name
- `telegram_user_nickname` - User nickname
- `is_accepted` - Terms acceptance status
- `is_active` - Ban status (true = not banned)
- `created_at` - Creation timestamp

### Usage Table
- `id` - Auto-increment primary key
- `user_id` - Foreign key to users table
- `created_at` - Usage timestamp

## 📝 License

This project is licensed under the MIT License.

## 👨‍💻 Author

Created with ❤️ by [@MairwunNx](https://t.me/MairwunNx)