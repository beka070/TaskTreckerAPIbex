# TaskTracker API

ASP.NET Core 8 REST API для управления задачами двух типов: баг-репорты и запросы на фичи.

## Запуск

```bash
dotnet run
```

Swagger UI: http://localhost:5000/swagger

## Эндпоинты

| Метод | URL | Описание |
|-------|-----|----------|
| GET | `/api/tasks` | Получить все задачи |
| GET | `/api/tasks/stats` | Статистика: баги с высоким severity + суммарные часы |
| POST | `/api/tasks/bug` | Создать баг-репорт |
| POST | `/api/tasks/feature` | Создать запрос на фичу |
| PUT | `/api/tasks/{id}/complete` | Завершить задачу |

## Примеры запросов

Создать баг:
```json
POST /api/tasks/bug
{
  "title": "Null reference on login",
  "severityLevel": 2
}
```

Создать фичу:
```json
POST /api/tasks/feature
{
  "title": "Dark mode",
  "estimatedHours": 8.5
}
```

## Структура проекта

```
src/
  Controllers/   — TasksController
  Models/        — BaseTask, BugReportTask, FeatureRequestTask, DTOs
  Repository/    — ITaskRepository, InMemoryTaskRepository
  Services/      — TaskFilterService
```

## Архитектура

- Хранилище в памяти (in-memory), без базы данных
- События через `delegate` + `event` при завершении задачи
- Дизайн интеграции с NotificationService через RabbitMQ описан в [INTEGRATION_DESIGN.md](INTEGRATION_DESIGN.md)
