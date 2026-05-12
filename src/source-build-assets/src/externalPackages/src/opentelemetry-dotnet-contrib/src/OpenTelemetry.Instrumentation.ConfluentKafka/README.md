# Confluent.Kafka client instrumentation for OpenTelemetry

| Status | |
| ------ | --- |
| Stability | [Development](../../README.md#development) |
| Code Owners | [@g7ed6e](https://github.com/g7ed6e) |

[![NuGet version badge](https://img.shields.io/nuget/v/OpenTelemetry.Instrumentation.ConfluentKafka)](https://www.nuget.org/packages/OpenTelemetry.Instrumentation.ConfluentKafka)
[![NuGet download count badge](https://img.shields.io/nuget/dt/OpenTelemetry.Instrumentation.ConfluentKafka)](https://www.nuget.org/packages/OpenTelemetry.Instrumentation.ConfluentKafka)
[![codecov.io](https://codecov.io/gh/open-telemetry/opentelemetry-dotnet-contrib/branch/main/graphs/badge.svg?flag=unittests-Instrumentation.ConfluentKafka)](https://app.codecov.io/gh/open-telemetry/opentelemetry-dotnet-contrib?flags[0]=unittests-Instrumentation.ConfluentKafka)

## Usage

To use the `OpenTelemetry.Instrumentation.ConfluentKafka` package, follow these
steps:

1. **Install the package**:

    ```shell
    dotnet add package OpenTelemetry.Instrumentation.ConfluentKafka --prerelease
    ```

2. **Configure OpenTelemetry in your application**:

    ```csharp
    using Confluent.Kafka;
    using OpenTelemetry.Metrics;
    using OpenTelemetry.Trace;

    var builder = Host.CreateApplicationBuilder(args);

    const string bootstrapServers = "localhost:9092";

    builder.Services.AddSingleton(_ =>
    {
        ProducerConfig producerConfig = new() { BootstrapServers = bootstrapServers };
        return new InstrumentedProducerBuilder<string, string>(producerConfig);
    });
    builder.Services.AddSingleton(_ =>
    {
        ConsumerConfig consumerConfigA = new()
        {
            BootstrapServers = bootstrapServers,
            GroupId = "group-a",
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnablePartitionEof = true,
        };
        return new InstrumentedConsumerBuilder<string, string>(consumerConfigA);
    });

    builder.Services.AddOpenTelemetry()
        .WithTracing(tracing =>
        {
            tracing.AddConsoleExporter()
                .AddOtlpExporter()
                .AddKafkaProducerInstrumentation<string, string>()
                .AddKafkaConsumerInstrumentation<string, string>();
        })
        .WithMetrics(metering =>
        {
            metering.AddConsoleExporter()
                .AddOtlpExporter()
                .AddKafkaProducerInstrumentation<string, string>()
                .AddKafkaConsumerInstrumentation<string, string>();
        });

    builder.Services.AddHostedService<ProduceConsumeHostedService>();

    var app = builder.Build();
    await app.RunAsync();
    ```

This will set up OpenTelemetry instrumentation for Confluent.Kafka producers
and consumers, allowing you to collect and export telemetry data.

## Extending `ConsumerBuilder` or `ProducerBuilder` instances

To extend an already built `ConsumerBuilder<TKey, TValue>`
or `ProducerBuilder<TKey, TValue>`
instance with OpenTelemetry instrumentation, you can use the `AsInstrumentedConsumerBuilder`
and `AsInstrumentedProducerBuilder` extension methods.

### Example for `ConsumerBuilder<TKey, TValue>`

```csharp
using Confluent.Kafka;
using OpenTelemetry.Instrumentation.ConfluentKafka;

var consumerConfig = new ConsumerConfig
{
    BootstrapServers = "localhost:9092",
    GroupId = "my-group",
    AutoOffsetReset = AutoOffsetReset.Earliest
};

var consumerBuilder = new ConsumerBuilder<string, string>(consumerConfig);

// Set various handlers and properties
consumerBuilder.SetErrorHandler((consumer, error) => Console.WriteLine($"Error: {error.Reason}"));
consumerBuilder.SetLogHandler((consumer, logMessage) => Console.WriteLine($"Log: {logMessage.Message}"));
consumerBuilder.SetStatisticsHandler((consumer, statistics) => Console.WriteLine($"Statistics: {statistics}"));

// Convert to InstrumentedConsumerBuilder
var instrumentedConsumerBuilder = consumerBuilder.AsInstrumentedConsumerBuilder();

// Build the consumer
var consumer = instrumentedConsumerBuilder.Build();
```

### Example for `ProducerBuilder<TKey, TValue>`

```csharp
using Confluent.Kafka;
using OpenTelemetry.Instrumentation.ConfluentKafka;

var producerConfig = new ProducerConfig
{
    BootstrapServers = "localhost:9092"
};

var producerBuilder = new ProducerBuilder<string, string>(producerConfig);

// Set various handlers and properties
producerBuilder.SetErrorHandler((producer, error) => Console.WriteLine($"Error: {error.Reason}"));
producerBuilder.SetLogHandler((producer, logMessage) => Console.WriteLine($"Log: {logMessage.Message}"));
producerBuilder.SetStatisticsHandler((producer, statistics) => Console.WriteLine($"Statistics: {statistics}"));

// Convert to InstrumentedProducerBuilder
var instrumentedProducerBuilder = producerBuilder.AsInstrumentedProducerBuilder();

// Build the producer
var producer = instrumentedProducerBuilder.Build();
```
