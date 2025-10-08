# 🤝 Contributing to Louisepizdon

Спасибо за интерес к проекту! Мы приветствуем любые вклады.

## 🏗️ Архитектура проекта

Проект построен по модульному принципу:

### Модули

- **Platform** - Конфигурация и базовые утилиты
- **Persistence** - База данных (Entity Framework Core)
- **Caching** - Redis кеширование
- **Throttler** - Управление лимитами
- **Artificial** - AI Agent интеграция
- **Telegram** - Telegram Bot API
- **Texting** - Константы сообщений
- **Tracing** - Логирование

### Принципы

1. **Один класс = один файл** (максимум 2 класса в файле)
2. **Короткие имена файлов** (1-2 слова)
3. **Простые namespace** (1 слово)
4. **Dependency Injection** для всех зависимостей
5. **Структурное логирование** везде

## 📝 Coding Style

### C# Conventions

- Используйте современный C# 13 синтаксис
- Nullable reference types включены
- File-scoped namespaces предпочтительны
- Async/await для всех I/O операций

### Именование

```csharp
// Классы - PascalCase
public class UserRepository { }

// Методы - PascalCase
public async Task CreateUserAsync() { }

// Приватные поля - camelCase с _
private readonly ILogger _logger;

// Параметры и локальные переменные - camelCase
public void ProcessUser(long userId) { }
```

### Структура файла

```csharp
using System;
using Microsoft.Extensions.DependencyInjection;
// ... другие using

namespace Louisepizdon.Module;

// Интерфейсы
public interface IService { }

// Реализация
public class Service : IService
{
    private readonly IDependency _dependency;
    
    public Service(IDependency dependency)
    {
        _dependency = dependency;
    }
    
    public async Task DoWorkAsync()
    {
        // Implementation
    }
}
```

## 🧪 Тестирование

(TODO: Добавить unit tests с xUnit)

## 🔀 Git Workflow

### Коммиты

Используйте эмодзи префиксы на русском языке:

- `✨ Добавлена новая фича`
- `🐛 Исправлен баг с лимитами`
- `📝 Обновлена документация`
- `♻️ Рефакторинг модуля Telegram`
- `🔧 Изменена конфигурация`
- `🚀 Улучшена производительность`
- `🔒 Исправление безопасности`

### Branches

- `master` - стабильная версия
- `develop` - разработка
- `feature/*` - новые фичи
- `bugfix/*` - исправления багов

### Pull Request

1. Fork репозиторий
2. Создайте feature branch
3. Сделайте изменения
4. Напишите тесты (если применимо)
5. Убедитесь что код компилируется
6. Создайте PR с описанием изменений

## 📋 Чеклист PR

- [ ] Код следует coding style проекта
- [ ] Добавлено логирование где необходимо
- [ ] Обновлена документация
- [ ] Нет предупреждений компилятора
- [ ] Проверено локально
- [ ] Commit messages следуют конвенции

## 🐛 Reporting Bugs

Используйте GitHub Issues с шаблоном:

```markdown
## Описание бага
Краткое описание проблемы

## Шаги для воспроизведения
1. Шаг 1
2. Шаг 2
3. ...

## Ожидаемое поведение
Что должно было произойти

## Фактическое поведение
Что произошло на самом деле

## Окружение
- OS: Linux/Windows/macOS
- .NET Version: 9.0
- Docker: Yes/No
```

## 💡 Идеи для улучшения

### High Priority
- [ ] Unit тесты
- [ ] Integration тесты
- [ ] Healthcheck endpoint
- [ ] Metrics и мониторинг
- [ ] Graceful shutdown

### Medium Priority
- [ ] Поддержка нескольких языков
- [ ] Web dashboard для статистики
- [ ] Админ команды в боте
- [ ] Webhook режим вместо polling

### Low Priority
- [ ] Поддержка голосовых сообщений
- [ ] Поддержка видео
- [ ] Экспорт истории в CSV
- [ ] Telegram mini app

## 📞 Контакты

- Telegram: [@MairwunNx](https://t.me/MairwunNx)
- Issues: GitHub Issues

Спасибо за вклад! 🥀