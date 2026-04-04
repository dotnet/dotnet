# StackExchangeRedis Instrumentation for OpenTelemetry .NET - Example

An example application that shows how to use
`OpenTelemetry.Instrumentation.StackExchangeRedis` to capture traces of outgoing
calls to Redis. You should see the following output on the Console when you run
this application (please look at the prerequisite for running the application in
[Program.cs](./Program.cs):

```text
Activity.TraceId:            ebf13ff051d73072f87d07ebf08d5f26
Activity.SpanId:             326c619dbda12231
Activity.TraceFlags:         Recorded
Activity.ActivitySourceName: OpenTelemetry.Instrumentation.StackExchangeRedis
Activity.DisplayName:        SET
Activity.Kind:               Client
Activity.StartTime:          2024-03-14T09:35:28.1473251Z
Activity.Duration:           00:00:00.0050600
Activity.Tags:
    db.system: redis
    db.redis.flags: DemandMaster
    db.statement: SET
    net.peer.name: localhost
    net.peer.port: 6379
    db.redis.database_index: 0
Activity.Events:
    Enqueued [3/14/2024 9:35:28 AM +00:00]
    Sent [3/14/2024 9:35:28 AM +00:00]
    ResponseReceived [3/14/2024 9:35:28 AM +00:00]
Resource associated with Activity:
    telemetry.sdk.name: opentelemetry
    telemetry.sdk.language: dotnet
    telemetry.sdk.version: 1.7.0
    service.name: unknown_service:Examples.StackExchangeRedis

Activity.TraceId:            1824275fc972c4f0f3180a175ba38acc
Activity.SpanId:             734196993db5b8dd
Activity.TraceFlags:         Recorded
Activity.ActivitySourceName: OpenTelemetry.Instrumentation.StackExchangeRedis
Activity.DisplayName:        GET
Activity.Kind:               Client
Activity.StartTime:          2024-03-14T09:35:29.1575041Z
Activity.Duration:           00:00:00.0031683
Activity.Tags:
    db.system: redis
    db.redis.flags: None
    db.statement: GET
    net.peer.name: localhost
    net.peer.port: 6379
    db.redis.database_index: 0
Activity.Events:
    Enqueued [3/14/2024 9:35:29 AM +00:00]
    Sent [3/14/2024 9:35:29 AM +00:00]
    ResponseReceived [3/14/2024 9:35:29 AM +00:00]
Resource associated with Activity:
    telemetry.sdk.name: opentelemetry
    telemetry.sdk.language: dotnet
    telemetry.sdk.version: 1.7.0
    service.name: unknown_service:Examples.StackExchangeRedis
```
