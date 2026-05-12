# MassTransit Instrumentation for OpenTelemetry .NET

[![NuGet version badge](https://img.shields.io/nuget/v/OpenTelemetry.Instrumentation.MassTransit)](https://www.nuget.org/packages/OpenTelemetry.Instrumentation.MassTransit)
[![NuGet download count badge](https://img.shields.io/nuget/dt/OpenTelemetry.Instrumentation.MassTransit)](https://www.nuget.org/packages/OpenTelemetry.Instrumentation.MassTransit)

Automatically instruments
[DiagnosticSource](https://masstransit-project.com/advanced/monitoring/diagnostic-source.html)
events emitted by [MassTransit](https://masstransit-project.com/) library.

## Deprecated

> [!IMPORTANT]
> **This only works with MassTransit v7 (and earlier, where supported)**.
> MassTransit v8.0.0 and later have built-in direct support for Open Telemetry
> via `ActivitySource`.

To instrument MassTransit v8.0.0+ you need to configure the OpenTelemetry SDK
to listen to the `ActivitySource` used by the library by calling
`AddSource("MassTransit")` on the `TracerProviderBuilder`.

## Installation

```shell
dotnet add package OpenTelemetry.Instrumentation.MassTransit
```

## Configuration

ASP.NET Core instrumentation example:

```csharp
// Add MassTransit inside ConfigureServices
services.AddMassTransit(x =>
{
    x.AddConsumer<OrderConsumer>();

    x.AddBus(context => Bus.Factory.CreateUsingRabbitMq(cfg =>
    {
        cfg.Host("rabbitmq://localhost");

        cfg.ReceiveEndpoint("submit-order", ep =>
        {
            ep.PrefetchCount = 16;
            ep.UseMessageRetry(r => r.Interval(2, 100));

            ep.ConfigureConsumer<OrderConsumer>(context);
        });
    }));
});
services.AddMassTransitHostedService();

// Add OpenTelemetry and MassTransit instrumentation
services.AddOpenTelemetrySdk(x =>
{
    x.AddMassTransitInstrumentation();
    x.AddOtlpExporter(config => {
      // Configure OTLP
    });
});
```

## Filter traced operations

For example you can trace only consume and handle operations using this snippet:

```csharp
// ...
using OpenTelemetry.Instrumentation.MassTransit.Implementation;
// ...
x.AddMassTransitInstrumentation(
    opts =>
        opts.TracedOperations = new HashSet<string>(new[] {
            OperationName.Consumer.Consume,
            OperationName.Consumer.Handle
}));
```

For full operation list please see: [OperationName](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/blob/Instrumentation.MassTransit-1.0.0-beta.3/src/OpenTelemetry.Instrumentation.MassTransit/OperationName.cs).

All operations are enabled by default.

## References

* [OpenTelemetry Project](https://opentelemetry.io/)
* [MassTransit Project](https://masstransit-project.com/)
