# OpenTelemetry .NET SDK telemetry enrichment framework

| Status | |
| ------ | --- |
| Stability | [Beta](../../README.md#beta) |
| Code Owners | [@evgenyfedorov2](https://github.com/evgenyfedorov2), [@dariusclay](https://github.com/dariusclay) |

[![NuGet version badge](https://img.shields.io/nuget/v/OpenTelemetry.Extensions.Enrichment)](https://www.nuget.org/packages/OpenTelemetry.Extensions.Enrichment)
[![NuGet download count badge](https://img.shields.io/nuget/dt/OpenTelemetry.Extensions.Enrichment)](https://www.nuget.org/packages/OpenTelemetry.Extensions.Enrichment)
[![codecov.io](https://codecov.io/gh/open-telemetry/opentelemetry-dotnet-contrib/branch/main/graphs/badge.svg?flag=unittests-Extensions.Enrichment)](https://app.codecov.io/gh/open-telemetry/opentelemetry-dotnet-contrib?flags[0]=unittests-Extensions.Enrichment)

Contains OpenTelemetry .NET SDK telemetry enrichment framework
which is used for enrichment of traces.

## Introduction

Telemetry enrichment attaches various types of information to traces.
You can use the telemetry enrichment framework to attach any custom information
that you would like to be present in all of your traces.

With telemetry enrichment framework, you don't need to worry about attaching
the information carefully to each telemetry object you touch.
Instead, if you implement your enricher class, it  takes care of the details
automatically. You simply register your class with the enrichment framework
and the enrichment framework will make sure to call the `Enrich()` method of your
class every time there is an `Activity` in your app.

## Traces

Currently this package supports trace enrichment only.

### Steps to enable OpenTelemetry.Extensions.Enrichment

You can view an example project using Enrichment at
[Examples.Enrichment](../../examples/enrichment/Examples.Enrichment/Program.cs).

### Step 1: Install package

Download the `OpenTelemetry.Extensions.Enrichment` package:

```shell
dotnet add package OpenTelemetry.Extensions.Enrichment --prerelease
```

### Step 2: Create enricher class

Create your custom enricher class that inherits from the `TraceEnricher` class
and override the `public abstract void Enrich(in TraceEnrichmentBag bag)` method.
Optionally, inject other services your enricher class depends on:

```csharp
internal sealed class MyTraceEnricher : TraceEnricher
{
    private readonly IMyService myService;

    public MyTraceEnricher(IMyService myService)
    {
        this.myService = myService;
    }

    public override void Enrich(in TraceEnrichmentBag bag)
    {
        var (service, status) = this.myService.MyDailyStatus();

        bag.Add(service, status);
    }
}
```

An example of IMyService implementation is available
[here](../../examples/enrichment/Examples.Enrichment/MyService.cs).

For every `Activity`, the `Enrich()`
method is guaranteed to be called exactly once. Semantically,
for the example above it means that a new [tag object](https://learn.microsoft.com/dotnet/api/system.diagnostics.activity.tagobjects?view=net-7.0)
with the service key and the status  value will be added to every `Activity`
in your application.

### Step 3: Register enricher class

Add your custom enricher class to the `TracerProviderBuilder` by calling the
`TryAddTraceEnricher<T>()` method. Configure other services via
`ConfigureServices()`, add `ActivitySource` and an exporter as usual:

```csharp
using var MyActivitySource = new ActivitySource("MyCompany.MyProduct.MyLibrary");
using var tracerProvider = Sdk.CreateTracerProviderBuilder()
    .ConfigureServices(services => services.AddSingleton<IMyService, MyService>())
    .AddSource("MyCompany.MyProduct.MyLibrary")
    .TryAddTraceEnricher<MyTraceEnricher>()
    .AddConsoleExporter()
    .Build();
```

Alternatively, you can add your custom enricher to the `IServiceCollection`
(as well as `ActivitySource` and exporter), typically
this is done inside the [ConfigureServices()](https://learn.microsoft.com/dotnet/api/microsoft.aspnetcore.hosting.startupbase.configureservices)
method:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddSingleton<IMyService, MyService>();
    services.AddOpenTelemetry().WithTracing((builder) => builder
        .AddSource("MyCompany.MyProduct.MyLibrary")
        .TryAddTraceEnricher<MyTraceEnricher>()
        .AddConsoleExporter());
}
```

> [!NOTE]
> The `AddTraceEnricher()` method call should be done *before* registering exporter
related Activity processors.

### Step 4: Usage

Create an `Activity` and add tags as usual:

```csharp
using var activity = myActivitySource.StartActivity("SayHello");
activity?.SetTag("hello", "world");
```

Run your application and verify that the `MyService` tag is added to `Activity`:

```shell
Activity.TraceId:            0e1dc24e2e63796bfc8186e24f916f5f
Activity.SpanId:             bfcbf9d5746009d6
Activity.TraceFlags:         Recorded
Activity.ActivitySourceName: MyCompany.MyProduct.MyLibrary
Activity.DisplayName:        SayHello
Activity.Kind:               Internal
Activity.StartTime:          2023-03-20T09:39:21.9642338Z
Activity.Duration:           00:00:00.0016887
Activity.Tags:
    hello: world
    MyService: No blockers
Resource associated with Activity:
    service.name: unknown_service:Examples.Enrichment
```

### Extension methods

Extension methods with different signatures are provided to enable common registration
styles. The methods relying on `TracerProviderBuilder` are for OpenTelemetry
.NET component authors. Conversely, the methods that utilize `IServiceCollection`
are for general library authors who may not have a reference to
`TracerProviderBuilder` or who want to register enrichers with other general services.
Anyway, both ways can be used within the same app.

In case you would like to use a comprehensive enricher class that may require
injection or interaction with other classes, you may utilize either of these
two methods. Both methods take a type T parameter to specify the type of your
enricher class. In this case, the enricher class will be created for you by
Dependency Injection:

```csharp
public static TryAddTraceEnricher<T>(this IServiceCollection services)
public static TryAddTraceEnricher<T>(this TracerProviderBuilder builder)
```

If you prefer to instantiate your enricher class on your own, you may use one of
these methods which allow for the usage of pre-existing enricher objects:

```csharp
public static TryAddTraceEnricher(this IServiceCollection services, TraceEnricher enricher)
public static TryAddTraceEnricher(this TracerProviderBuilder builder, TraceEnricher enricher)
```

If you only need to enrich a small amount of data, it may not be necessary to create
an enricher class. Instead, you can make use of the following methods which accept
an `Action<TraceEnrichmentBag>` delegate:

```csharp
public static AddTraceEnricher(this IServiceCollection services, Action<TraceEnrichmentBag> enrichmentAction)
public static AddTraceEnricher(this TracerProviderBuilder builder, Action<TraceEnrichmentBag> enrichmentAction)
```

If you would rather use a factory method to instantiate your enricher class,
with the possibility of interacting with `IServiceProvider,` you can utilize one
of these two methods:

```csharp
public static AddTraceEnricher(this IServiceCollection services, Func<IServiceProvider, TraceEnricher> enricherImplementationFactory)
public static AddTraceEnricher(this TracerProviderBuilder builder, Func<IServiceProvider, TraceEnricher> enricherImplementationFactory)
```

## Recommendations and best practices

You can add any number of custom enrichers, but it is advisable to only include
properties that are truly beneficial to prevent an excessive increase in the
number of tags associated with each `Activity`.
