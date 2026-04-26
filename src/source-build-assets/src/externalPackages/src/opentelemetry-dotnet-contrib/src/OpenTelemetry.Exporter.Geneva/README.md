# Geneva Exporter for OpenTelemetry .NET

| Status | |
| ------ | --- |
| Stability | [Stable](../../README.md#stable) |
| Code Owners | [@rajkumar-rangaraj](https://github.com/rajkumar-rangaraj/), [@xiang17](https://github.com/xiang17) |

[![NuGet version badge](https://img.shields.io/nuget/v/OpenTelemetry.Exporter.Geneva)](https://www.nuget.org/packages/OpenTelemetry.Exporter.Geneva)
[![NuGet download count badge](https://img.shields.io/nuget/dt/OpenTelemetry.Exporter.Geneva)](https://www.nuget.org/packages/OpenTelemetry.Exporter.Geneva)
[![codecov.io](https://codecov.io/gh/open-telemetry/opentelemetry-dotnet-contrib/branch/main/graphs/badge.svg?flag=unittests-Exporter.Geneva)](https://app.codecov.io/gh/open-telemetry/opentelemetry-dotnet-contrib?flags[0]=unittests-Exporter.Geneva)

The Geneva Exporter exports telemetry to
[Event Tracing for Windows (ETW)](https://docs.microsoft.com/windows/win32/etw/about-event-tracing)
or to a
[Unix Domain Socket (UDS)](https://en.wikipedia.org/wiki/Unix_domain_socket)
on the local machine.

## Installation

```shell
dotnet add package OpenTelemetry.Exporter.Geneva
```

## Configuration

The three types of telemetry are handled separately in OpenTelemetry.
Therefore, each type of telemetry **must be** enabled separately.

### Enable Logs

This snippet shows how to configure the Geneva Exporter for Logs

```csharp
using var loggerFactory = LoggerFactory.Create(loggingBuilder => loggingBuilder
    .AddOpenTelemetry(openTelemetryLoggerOptions =>
    {
        openTelemetryLoggerOptions.AddGenevaLogExporter(genevaExporterOptions =>
        {
            genevaExporterOptions.ConnectionString = "EtwSession=OpenTelemetry";
        });
    }));
```

The above code must be in application startup. In case of ASP.NET Core
applications, this should be in `ConfigureServices` of `Startup` class.
For ASP.NET applications, this should be in `Global.aspx.cs`.

Since OpenTelemetry .NET SDK is a
[LoggingProvider](https://docs.microsoft.com/dotnet/core/extensions/logging-providers),
use the built-in mechanism to apply [Log
filtering](https://docs.microsoft.com/dotnet/core/extensions/logging?tabs=command-line#how-filtering-rules-are-applied).
This filtering lets you control the Logs that are sent to each registered
provider, including the OpenTelemetry provider. `OpenTelemetry` is the
[alias](https://docs.microsoft.com/dotnet/api/microsoft.extensions.logging.provideraliasattribute)
for `OpenTelemetryLoggerProvider`, that may be used when configuring filtering
rules.

> [!NOTE]
> Some application types (e.g. [ASP.NET
Core](https://docs.microsoft.com/aspnet/core/fundamentals/logging/#configure-logging-1))
have default logging settings. Please review them to make sure
`OpenTelemetryLoggingProvider` is configured to receive Logs of appropriate
levels and category.

#### AFD CorrelationId Enrichment

An experimental feature flag is available to opt-into enriching logs with Azure
Front Door (AFD) Correlation IDs when present in the `RuntimeContext`. This
helps track requests as they flow through Azure Front Door services, making it
easier to correlate logs across different components.

To enable this feature, add
`PrivatePreviewEnableAFDCorrelationIdEnrichment=true` to your connection string:

```csharp
options.AddGenevaLogExporter(exporterOptions =>
{
    exporterOptions.ConnectionString = "PrivatePreviewEnableAFDCorrelationIdEnrichment=true";
});
```

When enabled, the exporter automatically adds an `AFDCorrelationId` attribute to
each log record if the value is present in `RuntimeContext`.

### Enable Traces

This snippet shows how to configure the Geneva Exporter for Traces

```csharp
using var tracerProvider = Sdk.CreateTracerProviderBuilder()
    .SetSampler(new AlwaysOnSampler())
    .AddSource("DemoSource")
    .AddGenevaTraceExporter(options => {
        options.ConnectionString = "EtwSession=OpenTelemetry";
    })
    .Build();
```

The above code must be in application startup. In case of ASP.NET Core
applications, this should be in `ConfigureServices` of `Startup` class.
For ASP.NET applications, this should be in `Global.aspx.cs`.

### GenevaExporterOptions (for Logs and Traces)

`GenevaExporterOptions` contains various options to configure the Geneva
Exporter.

#### `ConnectionString` (required for Logs and Traces)

On Linux the connection string has the format `Endpoint=unix:{UDS Path}`.

On Windows the connection string has the format `EtwSession={ETW session}`.

#### `CustomFields` (optional)

A list of fields which should be stored as individual table columns.

* If null, all fields will be stored as individual columns.
* If non-null, only those fields named in the list will be stored as individual
  columns.

#### `ResourceFieldNames` (optional)

A list of resource attribute keys which should be sent to Geneva.

* If null, no resource attributes will be stored.
* If non-null, only those resource attributes named in the list will be stored.
  This will disable PrepopulatedFields (see below).

#### `PrepopulatedFields` (optional)

This is a collection of fields that will be applied to all the Logs and Traces
sent through this exporter. If ResourceFieldNames is also specified,
PrepopulatedFields has no effect.

#### `IncludeTraceStateForSpan` (optional)

Export `activity.TraceStateString` as the value for Part B `traceState` field for
Spans when the `IncludeTraceStateForSpan` option is set to `true`.
This is an opt-in feature and the default value is `false`.
Note that this is for Spans only and not for LogRecord.

#### `TableNameMappings` (optional)

This defines the mapping for the table name used to store Logs and Traces.

##### Trace table name mappings

The default table name used for Traces is `Span`. To change the table name for
Traces add an entry with the key `Span` and set the value to the desired custom
table name.

> [!NOTE]
> Only a single table name is supported for Traces.

##### Log table name mappings

The default table name used for Logs is `Log`. Mappings can be specified
universally or for individual log message
[category](https://docs.microsoft.com/dotnet/core/extensions/logging#log-category)
values.

* To change the default table name for Logs add an entry with the key `*` and
  set the value to the desired custom table name. To enable "pass-through"
  mapping set the value of the `*` key to `*`. For details on "pass-through"
  mode see below.

* To change the table name for a specific log
  [category](https://docs.microsoft.com/dotnet/core/extensions/logging#log-category)
  add an entry with the key set to the full "category" of the log messages or a
  prefix that will match the starting portion of the log message "category". Set
  the value of the key to either the desired custom table name or `*` to enable
  "pass-through" mapping. For details on "pass-through" mode see below.

  For example, given the configuration...

  ```csharp
    var options = new GenevaExporterOptions
    {
        TableNameMappings = new Dictionary<string, string>()
        {
            ["*"] = "DefaultLogs",
            ["MyCompany"] = "InternalLogs",
            ["MyCompany.Product1"] = "InternalProduct1Logs",
            ["MyCompany.Product2"] = "InternalProduct2Logs",
            ["MyCompany.Product2.Security"] = "InternalSecurityLogs",
            ["MyPartner"] = "*",
        },
    };
  ```

  ...log category mapping would be performed as such:

  * `ILogger<ThirdParty.Thing>`: This would go to "DefaultLogs"
  * `ILogger<MyCompany.ProductX.Thing>`: This would go to "InternalLogs"
  * `ILogger<MyCompany.Product1.Thing>`: This would go to "InternalProduct1Logs"
  * `ILogger<MyCompany.Product2.Thing>`: This would go to "InternalProduct2Logs"
  * `ILogger<MyCompany.Product2.Security.Alert>`: This would go to
    "InternalSecurityLogs"
  * `ILogger<MyPartner.Product.Thing>`: This is marked as pass-through ("*") so
    it will be sanitized as "MyPartnerProductThing" table name

###### Pass-through table name mapping rules

When "pass-through" mapping is enabled for a given log message the runtime
[category](https://docs.microsoft.com/dotnet/core/extensions/logging#log-category)
value will be converted into a valid table name.

* The first character MUST be an ASCII letter. If it is lower-case, it will be
  converted into an upper-case letter. If the first character is invalid all log
  messages for the "category" will be dropped.

* Any non-ASCII letter or number will be removed.

* Only the first 50 valid characters will be used.

#### How to configure GenevaExporterOptions using dependency injection

##### Tracing

> [!NOTE]
> In this example named options ('GenevaTracing') are used. This is because
  `GenevaExporterOptions` is shared by both logging & tracing. In a future
  version named options will also be supported in logging so it is recommended
  to use named options now for tracing in order to future-proof this code.

```csharp
// Step 1: Turn on tracing and register GenevaTraceExporter.
builder.Services.AddOpenTelemetry()
    .WithTracing(builder => builder
        .AddGenevaTraceExporter(
            "GenevaTracing", // Tell GenevaTraceExporter to retrieve options using the 'GenevaTracing' name
            _ => { }));

// Step 2: Use Options API to configure GenevaExporterOptions using services
// retrieved from the dependency injection container
builder.Services
    .AddOptions<GenevaExporterOptions>("GenevaTracing") // Register options with the 'GenevaTracing' name
    .Configure<IConfiguration>((exporterOptions, configuration) =>
    {
        exporterOptions.ConnectionString = configuration.GetValue<string>("OpenTelemetry:Tracing:GenevaConnectionString")
            ?? throw new InvalidOperationException("GenevaConnectionString was not found in application configuration");
    });
```

##### Logging

```csharp
// Step 1: Turn on logging.
builder.Logging.AddOpenTelemetry();

// Step 2: Use Options API to configure OpenTelemetryLoggerOptions using
// services retrieved from the dependency injection container
builder.Services
    .AddOptions<OpenTelemetryLoggerOptions>()
    .Configure<IConfiguration>((loggerOptions, configuration) =>
    {
        // Add GenevaLogExporter and configure GenevaExporterOptions using
        // services retrieved from the dependency injection container
        loggerOptions.AddGenevaLogExporter(exporterOptions =>
        {
            exporterOptions.ConnectionString = configuration.GetValue<string>("OpenTelemetry:Logging:GenevaConnectionString")
                ?? throw new InvalidOperationException("GenevaConnectionString was not found in application configuration");
        });
    });
```

### Enable Metrics

This snippet shows how to configure the Geneva Exporter for Metrics

```csharp
using var meterProvider = Sdk.CreateMeterProviderBuilder()
    .AddMeter("TestMeter")
    .AddGenevaMetricExporter(options =>
    {
        options.ConnectionString = "Account=OTelMonitoringAccount;Namespace=OTelMetricNamespace";
    })
    .Build();
```

The above code must be in application startup. In case of ASP.NET Core
applications, this should be in `ConfigureServices` of `Startup` class.
For ASP.NET applications, this should be in `Global.aspx.cs`.

### GenevaMetricExporterOptions (for Metrics)

`GenevaMetricExporterOptions` contains various options which are required to
configure the GenevaMetricExporter.

#### `ConnectionString` (required for Metrics)

On Windows **DO NOT** provide an ETW session name for Metrics, only specify
Account and Namespace. For example:
`Account={MetricAccount};Namespace={MetricNamespace}`.

On Linux provide an `Endpoint` in addition to the `Account` and `Namespace`.
For example:
`Endpoint=unix:{UDS Path};Account={MetricAccount};Namespace={MetricNamespace}`.

##### OtlpProtobufEncoding

An experimental feature flag is available to opt-into changing the underlying
serialization format to binary protobuf following the schema defined in [OTLP
specification](https://github.com/open-telemetry/opentelemetry-proto/blob/v1.1.0/opentelemetry/proto/metrics/v1/metrics.proto).

When using OTLP protobuf encoding `Account` and `Namespace` are **NOT** required
to be set on the `ConnectionString`. The recommended approach is to use
OpenTelemetry Resource instead:

```csharp
using var meterProvider = Sdk.CreateMeterProviderBuilder()
    // Other configuration not shown
    .ConfigureResource(r => r.AddAttributes(
        new Dictionary<string, object>()
        {
            ["_microsoft_metrics_account"] = "MetricsAccountGoesHere",
            ["_microsoft_metrics_namespace"] = "MetricsNamespaceGoesHere",
        }))
    .AddGenevaMetricExporter(options =>
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            options.ConnectionString = "PrivatePreviewEnableOtlpProtobufEncoding=true";
        }
        else
        {
            // Note: 1.10.0+ version required to use OTLP protobuf encoding on Linux

            // Use Unix domain socket mode
            options.ConnectionString = "Endpoint=unix:{OTLP UDS Path};PrivatePreviewEnableOtlpProtobufEncoding=true";

            // Use user_events mode (preferred but considered experimental as this is a new capability in Linux kernel)
            // options.ConnectionString = "PrivatePreviewEnableOtlpProtobufEncoding=true";
        }
    })
    .Build();
```

###### Windows

To send metric data over ETW using OTLP protobuf encoding set
`PrivatePreviewEnableOtlpProtobufEncoding=true` on the `ConnectionString`.

###### Linux

As of `1.10.0` `PrivatePreviewEnableOtlpProtobufEncoding=true` is also supported
on Linux.

###### When using unix domain socket

To send metric data over UDS using OTLP protobuf encoding set the `Endpoint` to
use the correct `OtlpSocketPath` path and set
`PrivatePreviewEnableOtlpProtobufEncoding=true` on the `ConnectionString`:
`Endpoint=unix:{OTLP UDS Path};PrivatePreviewEnableOtlpProtobufEncoding=true`.

> [!IMPORTANT]
> OTLP over UDS requires a different socket path than TLV over UDS.

###### When using user_events

> [!IMPORTANT]
> [user_events](https://docs.kernel.org/trace/user_events.html) are a newer
> feature of the Linux kernel and require a distro with the feature enabled.

To send metric data over user_events using OTLP protobuf encoding do **NOT**
specify an `Endpoint` and set `PrivatePreviewEnableOtlpProtobufEncoding=true` on
the `ConnectionString`.

#### `MetricExportIntervalMilliseconds` (optional)

Set the exporter's periodic time interval to export Metrics. The default value
is 60000 milliseconds.

#### `PrepopulatedMetricDimensions` (optional)

This is a collection of the dimensions that will be applied to _every_ metric
exported by the exporter.

#### How to configure GenevaMetricExporterOptions using dependency injection

```csharp
// Step 1: Turn on metrics and register GenevaMetricExporter.
builder.Services.AddOpenTelemetry()
    .WithMetrics(builder => builder.AddGenevaMetricExporter());

// Step 2: Use Options API to configure GenevaMetricExporterOptions using
// services retrieved from the dependency injection container
builder.Services
    .AddOptions<GenevaMetricExporterOptions>()
    .Configure<IConfiguration>((exporterOptions, configuration) =>
    {
        exporterOptions.ConnectionString = configuration.GetValue<string>("OpenTelemetry:Metrics:GenevaConnectionString")
            ?? throw new InvalidOperationException("GenevaConnectionString was not found in application configuration");
    });
```

## Troubleshooting

Before digging into a problem, check if you hit a known issue by looking at the
[CHANGELOG.md](./CHANGELOG.md) and [GitHub
issues](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/issues).

Geneva Exporters uses an
[EventSource](https://docs.microsoft.com/dotnet/api/system.diagnostics.tracing.eventsource)
with the name "OpenTelemetry-Exporter-Geneva" for its internal logging. Please
follow the [troubleshooting guide for OpenTelemetry
.NET](https://github.com/open-telemetry/opentelemetry-dotnet/tree/main/src/OpenTelemetry#troubleshooting)
for instructions on seeing Logs from the geneva exporter, as well as other
OpenTelemetry components.
