# 📝 Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [0.0.1] - 2025-10-08

### ✨ Added
- 🤖 Initial bot implementation with Telegram Bot API
- 🧠 Microsoft Agent Framework integration for image analysis
- 🗄️ SQLite database with Entity Framework Core
- 🔴 Redis caching for usage limits
- 📊 Daily and monthly usage limits per user
- 🔒 User agreement flow with callback buttons
- 🌐 HTTP/SOCKS5 proxy support for Telegram API
- 📝 Structured logging with Serilog
- 🐳 Docker and docker-compose configuration
- 📖 Comprehensive documentation (README, SETUP, CONTRIBUTING)
- 🔧 Configuration via environment variables (APPLICATION_CONFIG)
- 🌍 Timezone support (TZ environment variable)

### 🏗️ Architecture
- Modular design with clear separation of concerns
- Dependency Injection with Microsoft.Extensions.DependencyInjection
- Repository pattern for data access
- Clean architecture principles

### 📦 Dependencies
- .NET 9.0
- Telegram.Bot 22.7.2
- Microsoft.Agents.AI (preview)
- Microsoft.EntityFrameworkCore.Sqlite 9.0.1
- StackExchange.Redis 2.9.25
- Serilog 4.2.0
- DotNetEnv 3.2.1

[Unreleased]: https://github.com/yourusername/louisepizdon/compare/v0.0.1...HEAD
[0.0.1]: https://github.com/yourusername/louisepizdon/releases/tag/v0.0.1