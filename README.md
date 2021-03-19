# TrackerApi
a tracking, notifications api and sdk with service bus implemenation for managing state between distributed services, queue's architecture

# Usage

```csharp
var tracker = new Tracker("tracking api url", QueueHandler);
tracker.CreateAsync(new TrackerRequest());
```

```csharp
var notification = new Notification("tracking api url");
notification.CreateAsync(new Notification());
```
