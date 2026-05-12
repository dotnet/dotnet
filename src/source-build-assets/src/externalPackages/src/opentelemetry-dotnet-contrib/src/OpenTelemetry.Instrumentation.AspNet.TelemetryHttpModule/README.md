# ASP.NET Telemetry HttpModule for OpenTelemetry

| Status | |
| ------ | --- |
| Stability | [Stable](../../README.md#stable) |
| Code Owners | [@open-telemetry/dotnet-contrib-maintainers](https://github.com/orgs/open-telemetry/teams/dotnet-contrib-maintainers) |

[![NuGet version badge](https://img.shields.io/nuget/v/OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule)](https://www.nuget.org/packages/OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule/)
[![NuGet download count badge](https://img.shields.io/nuget/dt/OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule)](https://www.nuget.org/packages/OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule/)
[![codecov.io](https://codecov.io/gh/open-telemetry/opentelemetry-dotnet-contrib/branch/main/graphs/badge.svg?flag=unittests-Instrumentation.AspNet)](https://app.codecov.io/gh/open-telemetry/opentelemetry-dotnet-contrib?flags[0]=unittests-Instrumentation.AspNet)

The ASP.NET Telemetry HttpModule is a skeleton to enable distributed tracing
and metrics of incoming ASP.NET requests using the OpenTelemetry API.

## Usage

### Step 1: Install NuGet package

If you are using the traditional `packages.config` reference style, a
`web.config` transform should run automatically and configure the
`TelemetryHttpModule` for you. If you are using the more modern PackageReference
style, this may need to be done manually. For more information, see:
[Migrate from packages.config to
PackageReference](https://docs.microsoft.com/nuget/consume-packages/migrate-packages-config-to-package-reference).

To configure your `web.config` manually, add this:

```xml
<system.webServer>
    <modules>
        <add
            name="TelemetryHttpModule"
            type="OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule,
                OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule"
            preCondition="integratedMode,managedHandler" />
    </modules>
</system.webServer>
```

### Step 2: Register hooks

`TelemetryHttpModule` provides hooks to create and manage activities and metrics.

To automatically register the entire infrastructure using OpenTelemetry, please
use the [OpenTelemetry.Instrumentation.AspNet](https://www.nuget.org/packages/OpenTelemetry.Instrumentation.AspNet/)
NuGet package.

## Options

`TelemetryHttpModule` provides a static options property
(`TelemetryHttpModule.Options`) which can be used to configure the
`TelemetryHttpModule` and listen to events it fires.

### TextMapPropagator

`TextMapPropagator` controls how trace context will be extracted from incoming
HTTP request messages. By default, [W3C Trace
Context](https://www.w3.org/TR/trace-context/) is enabled.

The OpenTelemetry API ships with a handful of [standard
implementations](https://github.com/open-telemetry/opentelemetry-dotnet/tree/main/src/OpenTelemetry.Api/Context/Propagation)
which may be used, or you can write your own by deriving from the
`TextMapPropagator` class.

To add support for
[Baggage](https://github.com/open-telemetry/opentelemetry-specification/blob/main/specification/baggage/api.md)
propagation in addition to W3C Trace Context, use:

```csharp
TelemetryHttpModuleOptions.TextMapPropagator = new CompositeTextMapPropagator(
    new TextMapPropagator[]
    {
        new TraceContextPropagator(),
        new BaggagePropagator(),
    });
```

> [!NOTE]
> When using the `OpenTelemetry.Instrumentation.AspNet`
`TelemetryHttpModuleOptions.TextMapPropagator` is automatically initialized to
the SDK default propagator (`Propagators.DefaultTextMapPropagator`) which by
default supports W3C Trace Context & Baggage.

### Events

`OnRequestStartedCallback`, `OnRequestStoppedCallback`, and `OnExceptionCallback`
are provided on `TelemetryHttpModuleOptions` and will be fired by the
`TelemetryHttpModule` as requests are processed.

A typical use case for the `OnRequestStartedCallback` event is to create an activity
based on the `HttpContextBase` and `ActivityContext`.

`OnRequestStoppedCallback` and `OnExceptionCallback` are needed to add
information (tags, events, and/or links) to the created `Activity` based on the
request, response, and/or exception event being fired.
