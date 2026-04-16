# SqlClient Instrumentation for OpenTelemetry

| Status | |
| ------ | --- |
| Stability | [Release candidate](../../README.md#release-candidate) |
| Code Owners | [@open-telemetry/dotnet-contrib-maintainers](https://github.com/orgs/open-telemetry/teams/dotnet-contrib-maintainers) |

[![NuGet](https://img.shields.io/nuget/v/OpenTelemetry.Instrumentation.SqlClient.svg)](https://www.nuget.org/packages/OpenTelemetry.Instrumentation.SqlClient)
[![NuGet](https://img.shields.io/nuget/dt/OpenTelemetry.Instrumentation.SqlClient.svg)](https://www.nuget.org/packages/OpenTelemetry.Instrumentation.SqlClient)
[![codecov.io](https://codecov.io/gh/open-telemetry/opentelemetry-dotnet-contrib/branch/main/graphs/badge.svg?flag=unittests-Instrumentation.SqlClient)](https://app.codecov.io/gh/open-telemetry/opentelemetry-dotnet-contrib?flags[0]=unittests-Instrumentation.SqlClient)

This is an [Instrumentation
Library](https://github.com/open-telemetry/opentelemetry-specification/blob/main/specification/glossary.md#instrumentation-library),
which instruments
[Microsoft.Data.SqlClient](https://www.nuget.org/packages/Microsoft.Data.SqlClient)
and
[System.Data.SqlClient](https://www.nuget.org/packages/System.Data.SqlClient)
and collects traces about database operations.

This component is based on
[v1.33](https://github.com/open-telemetry/semantic-conventions/blob/v1.33.0/docs/database/README.md)
of database semantic conventions. For details on the default set of
attributes that are added, check out the [Traces](#traces) and
[Metrics](#metrics) sections below.

> [!WARNING]
> Instrumentation is not working with `Microsoft.Data.SqlClient` v3.* due to
the [issue](https://github.com/dotnet/SqlClient/pull/1258). It was fixed in 4.0
and later.

## Steps to enable OpenTelemetry.Instrumentation.SqlClient

### Step 1: Install Package

Add a reference to the
[`OpenTelemetry.Instrumentation.SqlClient`](https://www.nuget.org/packages/OpenTelemetry.Instrumentation.SqlClient)
package. Also, add any other instrumentations & exporters you will need.

```shell
dotnet add package --prerelease OpenTelemetry.Instrumentation.SqlClient
```

### Step 2: Enable SqlClient Instrumentation at application startup

SqlClient instrumentation must be enabled at application startup.

#### Traces

The following example demonstrates adding SqlClient traces instrumentation
to a console application. This example also sets up the OpenTelemetry Console
exporter, which requires adding the package
[`OpenTelemetry.Exporter.Console`](https://github.com/open-telemetry/opentelemetry-dotnet/blob/main/src/OpenTelemetry.Exporter.Console/README.md)
to the application.

```csharp
using OpenTelemetry.Trace;

public class Program
{
    public static void Main(string[] args)
    {
        using var tracerProvider = Sdk.CreateTracerProviderBuilder()
            .AddSqlClientInstrumentation()
            .AddConsoleExporter()
            .Build();
    }
}
```

The instrumentation adheres to the
[semantic conventions for database client spans][semconv-spans].
An activity emitted by the instrumentation will include the following list of
attributes:

* `error.type`
* `db.namespace`
* `db.operation.name`
* `db.query.summary`
* `db.query.text`
* `db.response.status_code`
* `db.stored_procedure.name`
* `db.system.name`
* `server.address`
* `server.port`

#### Metrics

The following example demonstrates adding SqlClient metrics instrumentation
to a console application. This example also sets up the OpenTelemetry Console
exporter, which requires adding the package
[`OpenTelemetry.Exporter.Console`](https://github.com/open-telemetry/opentelemetry-dotnet/blob/main/src/OpenTelemetry.Exporter.Console/README.md)
to the application.

```csharp
using OpenTelemetry.Metrics;

public class Program
{
    public static void Main(string[] args)
    {
        using var meterProvider = Sdk.CreateMeterProviderBuilder()
            .AddSqlClientInstrumentation()
            .AddConsoleExporter()
            .Build();
    }
}
```

The instrumentation adheres to the
[semantic conventions for database client metrics][semconv-metrics].

Currently, the instrumentation supports the following metric and attributes.

| Name | Instrument Type | Unit | Description |
| ---- | --------------- | ---- | ----------- |
| `db.client.operation.duration` | Histogram | `s` | Duration of database client operations. |

* `error.type`
* `db.namespace`
* `db.operation.name`
* `db.query.summary`
* `db.response.status_code`
* `db.stored_procedure.name`
* `db.system.name`
* `server.address`
* `server.port`

#### ASP.NET Core

For an ASP.NET Core application, adding instrumentation is typically done in the
`ConfigureServices` of your `Startup` class. Refer to documentation for
[OpenTelemetry.Instrumentation.AspNetCore](../OpenTelemetry.Instrumentation.AspNetCore/README.md).

#### ASP.NET

For an ASP.NET application, adding instrumentation is typically done in the
`Global.asax.cs`. Refer to the documentation for
[OpenTelemetry.Instrumentation.AspNet](../OpenTelemetry.Instrumentation.AspNet/README.md).

## Advanced configuration

This instrumentation can be configured to change the default behavior by using
`SqlClientTraceInstrumentationOptions`.

### EnrichWithSqlCommand

> [!NOTE]
> EnrichWithSqlCommand is available on .NET runtimes only.

This option can be used to enrich the activity with additional information from
the raw `SqlCommand` object. The `EnrichWithSqlCommand` action is called only
when `activity.IsAllDataRequested` is `true`. It contains the activity itself
(which can be enriched), the name of the event, and the actual raw object.

Currently there is only one event name reported, "OnCustom". The actual object
is `Microsoft.Data.SqlClient.SqlCommand` for `Microsoft.Data.SqlClient` and
`System.Data.SqlClient.SqlCommand` for `System.Data.SqlClient`.

The following code snippet shows how to add additional tags using
`EnrichWithSqlCommand`.

```csharp
using var tracerProvider = Sdk.CreateTracerProviderBuilder()
    .AddSqlClientInstrumentation(opt => opt.EnrichWithSqlCommand
        = (activity, obj) =>
    {
        if (obj is SqlCommand cmd)
        {
            activity.SetTag("db.commandTimeout", cmd.CommandTimeout);
        }
    })
    .Build();
```

[Processor](https://github.com/open-telemetry/opentelemetry-dotnet/tree/main/docs/trace/extending-the-sdk/README.md#processor),
is the general extensibility point to add additional properties to any activity.
The `EnrichWithSqlCommand` option is specific to this instrumentation, and is
provided to get access to `SqlCommand` object.

### RecordException

> [!NOTE]
> RecordException is available on .NET runtimes only.

This option can be set to instruct the instrumentation to record SqlExceptions
as Activity
[events](https://github.com/open-telemetry/semantic-conventions/blob/main/docs/exceptions/exceptions-spans.md).

The default value is `false` and can be changed by the code like below.

```csharp
using var tracerProvider = Sdk.CreateTracerProviderBuilder()
    .AddSqlClientInstrumentation(
        options => options.RecordException = true)
    .AddConsoleExporter()
    .Build();
```

### Filter

> [!NOTE]
> Filter is available on .NET runtimes only.

This option can be used to filter out activities based on the properties of the
`SqlCommand` object being instrumented using a `Func<object, bool>`. The
function receives an instance of the raw `SqlCommand` and should return `true`
if the telemetry is to be collected, and `false` if it should not. The parameter
of the Func delegate is of type `object` and needs to be cast to the appropriate
type of `SqlCommand`, either `Microsoft.Data.SqlClient.SqlCommand` or
`System.Data.SqlClient.SqlCommand`. The example below filters out all commands
that are not stored procedures.

```csharp
using var traceProvider = Sdk.CreateTracerProviderBuilder()
   .AddSqlClientInstrumentation(
       opt =>
       {
           opt.Filter = cmd =>
           {
               if (cmd is SqlCommand command)
               {
                   return command.CommandType == CommandType.StoredProcedure;
               }

               return false;
           };
       })
   .AddConsoleExporter()
   .Build();
```

## Experimental features

> [!NOTE]
> Experimental features are not enabled by default and can only be activated with
> environment variables. They are subject to change or removal in future releases.

### DB query parameters

> [!NOTE]
> This feature is available on .NET runtimes only.

The `OTEL_DOTNET_EXPERIMENTAL_SQLCLIENT_ENABLE_TRACE_DB_QUERY_PARAMETERS` environment
variable controls whether `db.query.parameter.<key>` attributes are emitted.

Query parameters may contain sensitive data, so only enable this experimental feature
if your queries and/or environment are appropriate for enabling this option.

`OTEL_DOTNET_EXPERIMENTAL_SQLCLIENT_ENABLE_TRACE_DB_QUERY_PARAMETERS` is implicitly
`false` by default. When set to `true`, the instrumentation will set
[`db.query.parameter.<key>`](https://github.com/open-telemetry/semantic-conventions/blob/main/docs/database/database-spans.md#span-definition)
attributes for each of the query parameters associated with a database command.

### Trace Context Propagation

> [!NOTE]
> Only `CommandType.Text` commands are supported for trace context propagation.
> Only .NET runtimes are supported.

Database trace context propagation can be enabled by setting
`OTEL_DOTNET_EXPERIMENTAL_SQLCLIENT_ENABLE_TRACE_CONTEXT_PROPAGATION`
environment variable to `true`.
This uses the [SET CONTEXT_INFO](https://learn.microsoft.com/en-us/sql/t-sql/statements/set-context-info-transact-sql?view=sql-server-ver16)
command to set [traceparent](https://www.w3.org/TR/trace-context/#traceparent-header)
information for the current connection, which results in
**an additional round-trip to the database**.

## Activity Duration calculation

`Activity.Duration` represents the time the underlying connection takes to
execute the command/query. Completing the operation includes the time up to
determining that the request was successful. It doesn't include the time spent
reading the results from a query set (for example enumerating all the rows
returned by a data reader).

This is illustrated by the code snippet below:

```csharp
using var connection = new SqlConnection("...");
connection.Open();

using var command = connection.CreateCommand();
command.CommandText = "select top 100000 * from Users";

// Activity duration starts
using var reader = command.ExecuteReader();
// Activity duration ends

// Not included in the Activity duration
while (reader.Read())
{
}
```

## References

* [OpenTelemetry Project](https://opentelemetry.io/)
* [Semantic conventions for database client spans][semconv-spans]
* [Semantic conventions for database client metrics][semconv-metrics]
* [Semantic conventions for Microsoft SQL Server client operations](https://github.com/open-telemetry/semantic-conventions/blob/v1.33.0/docs/database/sql-server.md)

[semconv-metrics]: https://github.com/open-telemetry/semantic-conventions/blob/v1.33.0/docs/database/database-metrics.md
[semconv-spans]: https://github.com/open-telemetry/semantic-conventions/blob/v1.33.0/docs/database/database-spans.md
