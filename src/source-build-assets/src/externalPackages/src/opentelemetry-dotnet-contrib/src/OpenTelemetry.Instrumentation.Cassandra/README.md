# Cassandra Instrumentation for OpenTelemetry

| Status | |
| ------ | --- |
| Stability | [Beta](../../README.md#beta) |
| Code Owners | [@xsoheilalizadeh](https://github.com/xsoheilalizadeh) |

[![NuGet version badge](https://img.shields.io/nuget/v/OpenTelemetry.Instrumentation.Cassandra)](https://www.nuget.org/packages/OpenTelemetry.Instrumentation.Cassandra)
[![NuGet download count badge](https://img.shields.io/nuget/dt/OpenTelemetry.Instrumentation.Cassandra)](https://www.nuget.org/packages/OpenTelemetry.Instrumentation.Cassandra)
[![codecov.io](https://codecov.io/gh/open-telemetry/opentelemetry-dotnet-contrib/branch/main/graphs/badge.svg?flag=unittests-Instrumentation.Cassandra)](https://app.codecov.io/gh/open-telemetry/opentelemetry-dotnet-contrib?flags[0]=unittests-Instrumentation.Cassandra)

This is an
[Instrumentation Library](https://github.com/open-telemetry/opentelemetry-specification/blob/main/specification/glossary.md#instrumentation-library),
which instruments [CassandraCSharpDriver](https://github.com/datastax/csharp-driver)
and collects telemetry about cassandra metrics.

> [!NOTE]
> This package provides support for metrics only.
  You can enable tracing using [`Cassandra.OpenTelemetry`](https://docs.datastax.com/en/developer/csharp-driver/3.22/features/opentelemetry/index.html)
  package.

## Steps to enable OpenTelemetry.Instrumentation.Cassandra

### Step 1: Install Package

Add a reference to the
[`OpenTelemetry.Instrumentation.Cassandra`](https://www.nuget.org/packages/OpenTelemetry.Instrumentation.Cassandra)
package. Also, add any other instrumentations & exporters you will need.

```shell
dotnet add package OpenTelemetry.Instrumentation.Cassandra
```

### Step 2: Enable Cassandra Instrumentation at application startup

Cassandra instrumentation must be enabled at application startup.

The following example demonstrates adding Cassandra instrumentation to a
console application. This example also sets up the OpenTelemetry Console
exporter, which requires adding the package
[`OpenTelemetry.Exporter.Console`](https://www.nuget.org/packages/OpenTelemetry.Exporter.Console)
to the application.

```csharp
using OpenTelemetry.Trace;

public class Program
{
    public static void Main(string[] args)
    {
        using var metricProvider = Sdk.CreateMeterProviderBuilder()
            .AddCassandraInstrumentation()
            .AddConsoleExporter()
            .Build();

        var cluster = new Builder()
            .WithConnectionString(yourCassandraConnectionString)
            .WithOpenTelemetryMetrics()
            .Build();
    }
}
```

For an ASP.NET Core application, adding instrumentation is typically done in
the `ConfigureServices` of your `Startup` class. Refer to documentation for
[OpenTelemetry.Instrumentation.AspNetCore](../OpenTelemetry.Instrumentation.AspNetCore/README.md).

For an ASP.NET application, adding instrumentation is typically done in the
`Global.asax.cs`. Refer to documentation for [OpenTelemetry.Instrumentation.AspNet](../OpenTelemetry.Instrumentation.AspNet/README.md).

## Configuration options

You are able to configure your own cassandra driver metrics options in
`WithOpenTelemetryMetrics` the by default configuration includes all the metrics
 that the driver can provide.

```csharp
var options = new DriverMetricsOptions();

options.SetEnabledNodeMetrics(new[] {NodeMetric.Gauges.InFlight});

var cluster = new Builder()
    .WithConnectionString(yourCassandraConnectionString)
    .WithOpenTelemetryMetrics(options)
    .Build();
```

## List of metrics produced

| Name | Instrument Type | Unit | Description |
| ---- | --------------- | ---- | ----------- |
| `cassandra.cql-requests` | Histogram | `ms` | Measures the duration of Cassandra CQL requests from the client's perspective. |
| `cassandra.bytes-sent` | Histogram | `bytes` | Measures the amount of bytes sent by the client to Cassandra. |
| `cassandra.bytes-received` | Histogram | `bytes` | Measures the amount of bytes received by the client from Cassandra. |
| `cassandra.cql-messages` | Histogram | `ms` | Measures the duration of CQL messages from the client's perspective. |
| `cassandra.connected-nodes` | Gauge | `nodes` | Represents the number of nodes in Cassandra to which the client is connected. |
| `cassandra.pool.open-connections` | Gauge | `connections` | Represents the number of open connections from the client to Cassandra. |
| `cassandra.pool.in-flight` | Gauge | `requests` | Represents the number of in-flight requests from the client to Cassandra. |

## References

* [OpenTelemetry Project](https://opentelemetry.io/)

* [OpenTelemetry semantic conventions for Cassandra](https://github.com/open-telemetry/semantic-conventions/blob/main/docs/database/cassandra.md)

* [Cassandra C# Driver](https://github.com/datastax/csharp-driver)
