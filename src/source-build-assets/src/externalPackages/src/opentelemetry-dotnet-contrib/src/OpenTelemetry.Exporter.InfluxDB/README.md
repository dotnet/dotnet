# InfluxDB Exporter for OpenTelemetry .NET

| Status | |
| ------ | --- |
| Stability | [Alpha](../../README.md#alpha) |
| Code Owners | [@havret](https://github.com/havret) |

[![NuGet version badge](https://img.shields.io/nuget/v/OpenTelemetry.Exporter.InfluxDB)](https://www.nuget.org/packages/OpenTelemetry.Exporter.InfluxDB)
[![NuGet download count badge](https://img.shields.io/nuget/dt/OpenTelemetry.Exporter.InfluxDB)](https://www.nuget.org/packages/OpenTelemetry.Exporter.InfluxDB)
[![codecov.io](https://codecov.io/gh/open-telemetry/opentelemetry-dotnet-contrib/branch/main/graphs/badge.svg?flag=unittests-Exporter.InfluxDB)](https://app.codecov.io/gh/open-telemetry/opentelemetry-dotnet-contrib?flags[0]=unittests-Exporter.InfluxDB)

The InfluxDB exporter converts OpenTelemetry metrics into the InfluxDB model
following the [OpenTelemetry->InfluxDB conversion schema](https://github.com/influxdata/influxdb-observability/blob/main/docs/index.md).

This exporter can be used with InfluxDB 2.x and InfluxDB 1.8+ ([see details](#influxdb-18-api-compatibility)).

## Prerequisite

* [Get InfluxDB](https://portal.influxdata.com/downloads/)

### Step 1: Install Package

```shell
dotnet add package --prerelease OpenTelemetry.Exporter.InfluxDB
```

### Step 2: Configure OpenTelemetry MeterProvider

* When using the [OpenTelemetry.Extensions.Hosting](https://github.com/open-telemetry/opentelemetry-dotnet/blob/main/src/OpenTelemetry.Extensions.Hosting/README.md)
package on .NET 6.0+:

```csharp
  services.AddOpenTelemetry()
    .WithMetrics(builder => builder
        .AddInfluxDBMetricsExporter(options =>
        {
            options.Org = "org";
            options.Bucket = "bucket";
            options.Token = "token";
            options.Endpoint = new Uri("http://localhost:8086");
            options.MetricsSchema = MetricsSchema.TelegrafPrometheusV2;
        }));
```

* Or configure directly:

  Call the `MeterProviderBuilder.AddInfluxDBMetricsExporter` extension to
  register the Prometheus exporter.

```csharp
    var meterProvider = Sdk.CreateMeterProviderBuilder()
    .AddInfluxDBMetricsExporter(options =>
    {
        options.Org = "org";
        options.Bucket = "bucket";
        options.Token = "token";
        options.Endpoint = new Uri("http://localhost:8086");
        options.MetricsSchema = MetricsSchema.TelegrafPrometheusV2;
    })
    .Build();
    builder.Services.AddSingleton(meterProvider);
```

## Configuration

You can configure the `InfluxDBMetricsExporter` through
`InfluxDBMetricsExporterOptions`.

### Endpoint

HTTP/S destination for the line protocol.

### Org

The name of the InfluxDB organization that owns the destination bucket.

### Bucket

The name of the InfluxDB bucket to which signals will be written.

### Token

The authentication token for InfluxDB.

### MetricsSchema

The chosen metrics schema to write. Default value is
`MetricsSchema.TelegrafPrometheusV1`.

### FlushInterval

The time to wait at most (in milliseconds) with the write. Default value
is 1000.

## InfluxDB 1.8 API Compatibility

InfluxDB 1.8.0 introduced forward compatibility APIs for InfluxDB 2.0,
allowing you to easily transition from InfluxDB 1.x to InfluxDB 2.0 Cloud
or open source.

Here's a summary of the client API usage differences:

* Token

Use the format `username:password` for an authentication token, e.g.,
`my-user:my-password`. If the server doesn't require authentication,
use an empty string ("").

* Org

The organization parameter is not used in InfluxDB 1.8.
Use a hyphen ("-") where necessary.

* Bucket

Use the format database/retention-policy where a bucket is required. If the
default retention policy should be used, skip the retention policy.
Examples: `telegraf/autogen`, `telegraf`.

When using InfluxDB 1.8, modify the AddInfluxDBMetricsExporter options
accordingly:

```csharp
    services.AddOpenTelemetry()
    .WithMetrics(builder => builder
        .AddInfluxDBMetricsExporter(options =>
        {
            options.Org = "-";
            options.Bucket = "telegraf/autogen";
            options.Token = "my-user:my-password";
            options.Endpoint = new Uri("http://localhost:8086");
            options.MetricsSchema = MetricsSchema.TelegrafPrometheusV2;
        }));
```

## Troubleshooting

This component uses an
[EventSource](https://docs.microsoft.com/dotnet/api/system.diagnostics.tracing.eventsource)
with the name "OpenTelemetry-Exporter-InfluxDB" for its internal logging.
Please refer to [SDK troubleshooting](https://github.com/open-telemetry/opentelemetry-dotnet/blob/main/src/OpenTelemetry/README.md#troubleshooting)
for instructions on seeing these internal logs.
