# OneCollector Exporter for OpenTelemetry .NET

| Status | |
| ------ | --- |
| Stability | [Stable](../../README.md#stable) |
| Code Owners | [@rajkumar-rangaraj](https://github.com/rajkumar-rangaraj/) |

[![NuGet version badge](https://img.shields.io/nuget/v/OpenTelemetry.Exporter.OneCollector)](https://www.nuget.org/packages/OpenTelemetry.Exporter.OneCollector)
[![NuGet download count badge](https://img.shields.io/nuget/dt/OpenTelemetry.Exporter.OneCollector)](https://www.nuget.org/packages/OpenTelemetry.Exporter.OneCollector)
[![codecov.io](https://codecov.io/gh/open-telemetry/opentelemetry-dotnet-contrib/branch/main/graphs/badge.svg?flag=unittests-Exporter.OneCollector)](https://app.codecov.io/gh/open-telemetry/opentelemetry-dotnet-contrib?flags[0]=unittests-Exporter.OneCollector)

The OneCollectorExporter is designed for Microsoft products to send data to
public-facing end-points which route to Microsoft's internal data pipeline. It
is not meant to be used outside of Microsoft products and is open sourced to
demonstrate best practices and to be transparent about what is being collected.

## Installation

```shell
dotnet add package OpenTelemetry.Exporter.OneCollector
```

## Basic usage

```csharp
using var logFactory = LoggerFactory.Create(builder => builder
    .AddOpenTelemetry(builder =>
    {
        builder.IncludeScopes = true;
        builder.AddOneCollectorExporter("InstrumentationKey=instrumentation-key-here");
    }));

var logger = logFactory.CreateLogger<MyService>();

using var scope = logger.BeginScope("{requestContext}", Guid.NewGuid());

logger.LogInformation("Request received {requestId}!", 1);
logger.LogWarning("Warning encountered {error_code}!", 0xBAADBEEF);
```

## Table name resolution

By default when sending logs/events `OneCollectorExporter` generates fully
qualifed names using the `LogRecord` `CategoryName` and `EventId.Name`
properties (ex: `$"{CategoryName}.{EventId.Name}"`). When `EventId.Name` is not
supplied the `OneCollectorLogExporterOptions.DefaultEventName` property is used
(the default value is `Log`). Event full names are used by the OneCollector
service to create tables to store logs/events. The final table name may change
casing and will change `.` characters to `_` characters.

The default behavior is designed so that each unique log/event is mapped to its
own table with its own schema. Attributes supplied on logs/events are promoted
to columns on the table.

### Event full name mappings

In the event that the default full name generation behavior does not result in
the desired table configuration, `EventFullNameMappings` may be supplied to
`OneCollectorExporter` to customize the final event full names sent to the
OneCollector service.

> [!NOTE]
> When a log/event is mapped `OneCollectorExporter` will automatically add the
> `namespace` and/or `name` attribute(s) to preserve the original values.

Mappings can be specified using a default wild card rule, exact-match rules,
and/or prefix-based (`StartsWith`) rules. In the event multiple prefix matches
are made the one matching the most characters is selected.

* To change the default event full name for all logs add an entry with the key
  `*` and set the value to the desired event full name. Only a single `*`
  default entry may exist.

* To change the event full name for a specific
  namespace/[category](https://docs.microsoft.com/dotnet/core/extensions/logging#log-category)
  of log records add an entry with the key set to a prefix that will match from
  the starting portion of the namespace/category. Set the value to the desired
  event full name.

  For example, given the configuration...

  ```csharp
  logging.AddOneCollectorExporter(
    "InstrumentationKey=instrumentation-key-here",
    builder => builder.SetEventFullNameMappings(
        new Dictionary<string, string>()
        {
            { "*", "DefaultLogs" },
            { "MyCompany", "InternalLogs" },
            { "MyCompany.Product1", "InternalProduct1Logs" },
            { "MyCompany.Product2", "InternalProduct2Logs" },
            { "MyCompany.Product2.Security", "InternalSecurityLogs" },
        });
  ```

  ...log event full name mapping would be performed as such:

  * `ILogger<ThirdParty.Thing>`: All logs emitted through this logger will use
    the "DefaultLogs" event full name

  * `ILogger<MyCompany.ProductX.Thing>`: All logs emitted through this logger
    will use the "InternalLogs" event full name

  * `ILogger<MyCompany.Product1.Thing>`: All logs emitted through this logger
    will use the "InternalProduct1Logs" event full name

  * `ILogger<MyCompany.Product2.Thing>`: All logs emitted through this logger
    will use the "InternalProduct2Logs" event full name

  * `ILogger<MyCompany.Product2.Security.Alert>`: All logs emitted through this
    logger will use the "InternalSecurityLogs" event full name

#### Schema management

The default mapping behavior used by `OneCollectorExporter` is designed so that
each unique log/event is mapped to its own table with its own schema. Using
`EventFullNameMappings` may lead to many disparate logs/events with differing
schema going into the same table. This could lead to wide tables with many
columns or, in the case of logs/events sending attributes with the same name but
different data types, lost or corrupt data.

`OneCollectorExporter` does not currently provide any features to automatically
flatten or stringify attributes into a single column to prevent "schema
explosion" issues as described above. Users are encouraged to manually stringify
attributes which should not become columns into JSON strings and log them into a
standard attribute:

```csharp
logger.LogInformation(
    "Hello world {ColumnA} {ColumnB} {Attributes}",
    "A", // ColumnA
    "B", // ColumnB
    // Attributes:
    JsonSerializer.Serialize(
        new Dictionary<string, object?>()
        {
            ["id"] = 1,
            ["name"] = "name_goes_here",
        }));
```

#### Pass-through mappings

Pass-through mappings that preserve the orignal event namespace and/or name are
also possible.

For example, given the configuration...

```csharp
logging.AddOneCollectorExporter(
   "InstrumentationKey=instrumentation-key-here",
   builder => builder.SetEventFullNameMappings(
       new Dictionary<string, string>()
       {
           { "*", "DefaultLogs" },
           { "MyCompany", "*" },
       });
```

...log event full name mapping would be performed as such:

* `ILogger<ThirdParty.Thing>`: All logs emitted through this logger will use
  the "DefaultLogs" event full name via the wild-card default rule.

* `ILogger<MyCompany.OtherThing>`: All logs emitted through this logger will
  have event full names generated as `$"MyCompany.OtherThing.{EventId.Name}"`.
  `OneCollectorLogExporterOptions.DefaultEventName` is used (default value is
  `Log`) if a log/event does not have `EventId.Name` specified.
