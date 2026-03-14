# Block 3 — NotificationService Integration Design

## Scenario
When `OnTaskCompleted` fires in **TaskService**, a **NotificationService** must send an email.

## Chosen Pattern: Asynchronous (Message Broker)

### Why asynchronous?

| Concern | Sync (HTTP/REST) | Async (Message Broker) |
|---|---|---|
| Coupling | Tight — TaskService must know NotificationService URL | Loose — services only know the broker |
| Resilience | If NotificationService is down, task completion fails | Message is queued; delivery retried automatically |
| Latency | Caller waits for email to be sent | Caller returns immediately; email sent in background |
| Scalability | Hard to fan-out to multiple consumers | Multiple consumers (email, SMS, push) subscribe independently |

Email sending is inherently slow and non-critical to the task completion transaction.
Blocking the HTTP response while waiting for SMTP is bad UX and bad design.

## Technology Choice: RabbitMQ + MassTransit

1. **TaskService** publishes a `TaskCompletedEvent` message to a RabbitMQ exchange
   inside the `CompleteTask()` method (or via a domain event dispatcher).

2. **NotificationService** subscribes to that exchange and consumes the message,
   then sends the email via SMTP / SendGrid.

```
TaskService  ──publish──►  RabbitMQ Exchange  ──route──►  NotificationService
                                                    └──►  AuditService (future)
```

### Implementation sketch (MassTransit)

```csharp
// TaskService — publish
await _publishEndpoint.Publish(new TaskCompletedEvent(task.Id, task.Title));

// NotificationService — consume
public class TaskCompletedConsumer : IConsumer<TaskCompletedEvent>
{
    public async Task Consume(ConsumeContext<TaskCompletedEvent> ctx)
    {
        await _emailService.SendAsync($"Task '{ctx.Message.Title}' is done!");
    }
}
```

### Alternatives considered
- **HTTP/REST (sync)**: simpler setup, but creates tight coupling and fragility.
- **Azure Service Bus / AWS SQS**: viable for cloud-native deployments; same async pattern.
- **SignalR**: good for real-time UI notifications, not for email.

## Conclusion
Use **RabbitMQ** (self-hosted, easy Docker setup) with **MassTransit** abstraction layer.
This keeps TaskService and NotificationService fully decoupled and independently deployable.
