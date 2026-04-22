# OpenTelemetry .NET SDK preview features and extensions

| Status | |
| ------ | --- |
| Stability | [Beta](../../README.md#beta) |
| Code Owners | [@mikegoldsmith](https://github.com/mikegoldsmith) |

[![NuGet version badge](https://img.shields.io/nuget/v/OpenTelemetry.Extensions)](https://www.nuget.org/packages/OpenTelemetry.Extensions)
[![NuGet download count badge](https://img.shields.io/nuget/dt/OpenTelemetry.Extensions)](https://www.nuget.org/packages/OpenTelemetry.Extensions)
[![codecov.io](https://codecov.io/gh/open-telemetry/opentelemetry-dotnet-contrib/branch/main/graphs/badge.svg?flag=unittests-Extensions)](https://app.codecov.io/gh/open-telemetry/opentelemetry-dotnet-contrib?flags[0]=unittests-Extensions)

Contains useful features and extensions to the OpenTelemetry .NET SDK that are
not part of the official OpenTelemetry specification but might be added in the
future.

## Logging

### AttachLogsToActivityEvent

Adds a log processor which will convert log messages into events and attach them
to the currently running `Activity`.

### AddBaggageProcessor

Adds a log processor which will copy baggage entries to log records.
The method takes an optional predicate to filter the copied baggage entries
based on the entry key. If no predicate is provided, all entries are copied.

Example of AddBaggageProcessor usage with a predicate:

```csharp
var regex = new Regex("^allow", RegexOptions.Compiled);
using var loggerFactory = LoggerFactory.Create(builder => builder
.AddOpenTelemetry(options =>
{
    options.AddBaggageProcessor(regex.IsMatch);
    // other set up (exporters, processors)
})
```

Warning: The baggage key predicate is executed for every baggage entry for each
log record.
Do not use slow or intensive operations.

## Traces

### AutoFlushActivityProcessor

Processor that flushes its containing `TracerProvider` if an ended `Activity`
matches a predicate. Note that this processor must be added *after* exporter
related `Activity` processors.

Example of AutoFlushActivityProcessor usage:

```cs
public static TracerProviderBuilder AddMyExporter(this TracerProviderBuilder builder, MyExporterOptions options)
{
    return builder
        .AddProcessor(new BatchActivityExportProcessor(
                    new MyExporter(options),
                    options.BatchExportProcessorOptions.MaxQueueSize,
                    options.BatchExportProcessorOptions.ScheduledDelayMilliseconds,
                    options.BatchExportProcessorOptions.ExporterTimeoutMilliseconds,
                    options.BatchExportProcessorOptions.MaxExportBatchSize))
        .AddAutoFlushActivityProcessor(a => a.Parent == null && (a.Kind == ActivityKind.Server || a.Kind == ActivityKind.Consumer), 5000);
}
```

### BaggageActivityProcessor

The BaggageActivityProcessor reads entries stored in Baggage from the current `Baggage`
and adds the baggage keys and values to the `Activity` as tags (attributes) on start.

Add this activity processor to a tracer provider.

For example, to add all baggage entries to new activities:

```cs
var tracerProvider = Sdk.CreateTracerProviderBuilder()
    .AddBaggageActivityProcessor(BaggageActivityProcessor.AllowAllBaggageKeys)
    .Build();
```

Alternatively, you can select which baggage keys you want to copy using a
custom predicate function.

For example, to only copy baggage entries where the key start with `my-key`
using a custom function:

```cs
var tracerProvider = Sdk.CreateTracerProviderBuilder()
  .AddBaggageActivityProcessor((baggageKey) => baggageKey.StartWith("my-key", System.StringComparison.Ordinal))
  .Build();
```

For example, to only copy baggage entries where the key matches the regular
expression `^my-key`:

```cs
var baggageKeyRegex = new Regex("^mykey", RegexOptions.Compiled);
var tracerProvider = Sdk.CreateTracerProviderBuilder()
  .AddBaggageActivityProcessor((baggageKey) => baggageKeyRegex.IsMatch(baggageKey))
  .Build();
```

Warning: The baggage key predicate is executed for every baggage entry for each
started activity.
Do not use slow or intensive operations.

### RateLimitingSampler

The rate limiting sampler is a sampler that will limit the number of samples to
the specified rate per second. It is typically used in conjunction with the ParentBasedSampler
to ensure that the rate limiting sampler is only applied to the root spans. When
using the ParentBasedSampler, when an Activity creation request comes in without
a sampling decision, it will delegate to the rate limiting sampler which will
make a decision based on the rate limit, that way all spans in the trace will use
the same sampling decision, and the rate will effectively become the number of
traces per second, irrespective of the number of spans within each trace.

Example of RateLimitingSampler usage:

```cs
builder.Services.AddOpenTelemetry()
    .WithTracing(tracing =>
    {
        tracing.AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            // Add the rate limiting sampler with a limit of 3 traces per second
            .SetSampler(new ParentBasedSampler(new RateLimitingSampler(3)))
    });
```
