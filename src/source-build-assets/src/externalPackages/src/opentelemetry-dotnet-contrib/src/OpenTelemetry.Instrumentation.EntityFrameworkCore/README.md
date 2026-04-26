# EntityFrameworkCore Instrumentation for OpenTelemetry .NET

| Status | |
| ------ | --- |
| Stability | [Beta](../../README.md#beta) |
| Code Owners | [@martincostello](https://github.com/martincostello), [@matt-hensley](https://github.com/matt-hensley) |

[![NuGet version badge](https://img.shields.io/nuget/v/OpenTelemetry.Instrumentation.EntityFrameworkCore)](https://www.nuget.org/packages/OpenTelemetry.Instrumentation.EntityFrameworkCore)
[![NuGet download count badge](https://img.shields.io/nuget/dt/OpenTelemetry.Instrumentation.EntityFrameworkCore)](https://www.nuget.org/packages/OpenTelemetry.Instrumentation.EntityFrameworkCore)
[![codecov.io](https://codecov.io/gh/open-telemetry/opentelemetry-dotnet-contrib/branch/main/graphs/badge.svg?flag=unittests-Instrumentation.EntityFrameworkCore)](https://app.codecov.io/gh/open-telemetry/opentelemetry-dotnet-contrib?flags[0]=unittests-Instrumentation.EntityFrameworkCore)

This is an [Instrumentation
Library](https://github.com/open-telemetry/opentelemetry-specification/blob/main/specification/glossary.md#instrumentation-library),
which instruments
[Microsoft.EntityFrameworkCore](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore)
and collects traces about outgoing requests.

> [!IMPORTANT]
> Entity Framework Core Instrumentation only currently supports tracing for
> relational databases such as Microsoft SQL Server or PostgreSQL.
>
> It does not currently support NoSQL databases such as
> [Azure Cosmos DB](https://learn.microsoft.com/ef/core/providers/cosmos/).

**Note: This component is based on the OpenTelemetry semantic conventions for
[metrics](https://github.com/open-telemetry/semantic-conventions/blob/main/docs/database/database-metrics.md)
and
[traces](https://github.com/open-telemetry/semantic-conventions/blob/main/docs/database/database-spans.md).
These conventions are
[Experimental](https://github.com/open-telemetry/opentelemetry-specification/blob/main/specification/document-status.md),
and hence, this package is a
[pre-release](https://github.com/open-telemetry/opentelemetry-dotnet/blob/main/VERSIONING.md#pre-releases).
Until a [stable
version](https://github.com/open-telemetry/opentelemetry-specification/blob/main/specification/telemetry-stability.md)
is released, there can be [breaking changes](./CHANGELOG.md).**

## Steps to enable OpenTelemetry.Instrumentation.EntityFrameworkCore

### Step 1: Install Package

Add a reference to the
[`OpenTelemetry.Instrumentation.EntityFrameworkCore`](https://www.nuget.org/packages/OpenTelemetry.Instrumentation.EntityFrameworkCore)
package. Also, add any other instrumentations & exporters you will need.

```shell
dotnet add package --prerelease OpenTelemetry.Instrumentation.EntityFrameworkCore
```

### Step 2: Enable EntityFrameworkCore Instrumentation at application startup

`EntityFrameworkCore` instrumentation must be enabled at application startup.

The following example demonstrates adding `EntityFrameworkCore`
instrumentation to a console application. This example also sets up the
OpenTelemetry Console exporter, which requires adding the package
[`OpenTelemetry.Exporter.Console`](https://github.com/open-telemetry/opentelemetry-dotnet/blob/main/src/OpenTelemetry.Exporter.Console/README.md)
to the application.

```csharp
using OpenTelemetry;
using OpenTelemetry.Trace;

public class Program
{
    public static void Main(string[] args)
    {
        using var tracerProvider = Sdk.CreateTracerProviderBuilder()
            .AddEntityFrameworkCoreInstrumentation()
            .AddConsoleExporter()
            .Build();
    }
}
```

For an ASP.NET Core application, adding instrumentation is typically done in
the `ConfigureServices` of your `Startup` class. Refer to documentation for
[OpenTelemetry.Instrumentation.AspNetCore](../OpenTelemetry.Instrumentation.AspNetCore/README.md).

For an ASP.NET application, adding instrumentation is typically done in the
`Global.asax.cs`. Refer to the documentation for
[OpenTelemetry.Instrumentation.AspNet](../OpenTelemetry.Instrumentation.AspNet/README.md).

## Advanced configuration

This instrumentation can be configured to change the default behavior by using
`EntityFrameworkInstrumentationOptions`.

```csharp
services.AddOpenTelemetry()
    .WithTracing(builder => builder
        .AddEntityFrameworkCoreInstrumentation(options =>
        {
            options.EnrichWithIDbCommand = (activity, command) =>
            {
                var stateDisplayName = $"{command.CommandType} main";
                activity.DisplayName = stateDisplayName;
                activity.SetTag("db.name", stateDisplayName);
            };
        })
        .AddConsoleExporter());
```

When used with
[`OpenTelemetry.Extensions.Hosting`](https://github.com/open-telemetry/opentelemetry-dotnet/blob/main/src/OpenTelemetry.Extensions.Hosting/README.md),
all configurations to `EntityFrameworkInstrumentationOptions`
can be done in the `ConfigureServices` method of you applications `Startup`
class as shown below.

```csharp
// Configure
services.Configure<EntityFrameworkInstrumentationOptions>(options =>
{
    options.EnrichWithIDbCommand = (activity, command) =>
    {
        var stateDisplayName = $"{command.CommandType} main";
        activity.DisplayName = stateDisplayName;
        activity.SetTag("db.name", stateDisplayName);
    };
});

services.AddOpenTelemetry()
    .WithTracing(builder => builder
        .AddEntityFrameworkCoreInstrumentation()
        .AddConsoleExporter());
```

### Filter

This option can be used to filter out activities based on the provider name and
the properties of the db command object being instrumented
using a `Func<string, IDbCommand, bool>`. The function receives a provider name
and an instance of the db command and should return `true`
if the telemetry is to be collected, and `false` if it should not.

The following code snippet shows how to use `Filter` to collect traces
for stored procedures only.

```csharp
services.AddOpenTelemetry()
    .WithTracing(builder => builder
        .AddEntityFrameworkCoreInstrumentation(options =>
        {
            options.Filter = (providerName, command) =>
            {
                return command.CommandType == CommandType.StoredProcedure;
            };
        })
        .AddConsoleExporter());
```

## Experimental features

> [!NOTE]
> Experimental features are not enabled by default and can only be activated with
> environment variables. They are subject to change or removal in future releases.

### DB query parameters

The `OTEL_DOTNET_EXPERIMENTAL_EFCORE_ENABLE_TRACE_DB_QUERY_PARAMETERS` environment
variable controls whether `db.query.parameter.<key>` attributes are emitted.

Query parameters may contain sensitive data, so only enable this experimental feature
if your queries and/or environment are appropriate for enabling this option.

`OTEL_DOTNET_EXPERIMENTAL_EFCORE_ENABLE_TRACE_DB_QUERY_PARAMETERS` is implicitly
`false` by default. When set to `true`, the instrumentation will set
[`db.query.parameter.<key>`](https://github.com/open-telemetry/semantic-conventions/blob/main/docs/database/database-spans.md#span-definition)
attributes for each of the query parameters associated with a database command.

## Activity Duration calculation

`Activity.Duration` represents the time the underlying connection takes to
execute the command or query, including the time needed to determine
that the request was successful. **It does not** include the time spent
reading the results from a query set (for example enumerating all the rows
returned from an `IEnumerable`/`IQueryable`).

This is illustrated by the code snippet below:

```csharp
var activities = new List<Activity>();

using var tracerProvider = Sdk.CreateTracerProviderBuilder()
    .AddInMemoryExporter(activities)
    .AddEntityFrameworkCoreInstrumentation()
    .Build();

var builder = new DbContextOptionsBuilder<ItemsContext>()
    .UseSqlServer("...");

await using var context = new ItemsContext(builder.Options);

var stopwatch = Stopwatch.StartNew();

await context.Items.ForEachAsync((_) => Thread.Sleep(1));

stopwatch.Stop();

// The activity duration should be much less than the total elapsed time
// as the duration does not include time spent enumerating the query set.
Assert.True(activity[^1].Duration < stopwatch.Elapsed);
```

## References

* [OpenTelemetry Project](https://opentelemetry.io/)
