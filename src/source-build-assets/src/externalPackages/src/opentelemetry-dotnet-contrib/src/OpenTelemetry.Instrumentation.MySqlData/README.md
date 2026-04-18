# MySqlData Instrumentation for OpenTelemetry

[![NuGet version badge](https://img.shields.io/nuget/v/OpenTelemetry.Instrumentation.MySqlData)](https://www.nuget.org/packages/OpenTelemetry.Instrumentation.MySqlData)
[![NuGet download count badge](https://img.shields.io/nuget/dt/OpenTelemetry.Instrumentation.MySqlData)](https://www.nuget.org/packages/OpenTelemetry.Instrumentation.MySqlData)

This is an
[Instrumentation Library](https://github.com/open-telemetry/opentelemetry-specification/blob/main/specification/glossary.md#instrumentation-library),
which instruments [MySql.Data](https://www.nuget.org/packages/MySql.Data)
and collects telemetry about database operations.

## Deprecated

> [!IMPORTANT]
> **This only works with Mysql.Data v8.0.32 (and earlier, where supported)**.
> Mysql.Data v8.1.0 and later have built-in direct support for Open Telemetry
> via `ActivitySource`.

To instrument Mysql.Data v8.1.0+ you need to configure the OpenTelemetry SDK
to listen to the `ActivitySource` used by the library by calling
`AddSource("connector-net")` on the `TracerProviderBuilder`. Alternatively,
you can add the nuget package [MySQL.Data.OpenTelemetry](https://www.nuget.org/packages/MySql.Data.OpenTelemetry)
and call extension method `AddConnectorNet()` on the `TracerProviderBuilder`.

## Steps to enable OpenTelemetry.Instrumentation.MySqlData

### Step 1: Install Package

Add a reference to the
[`OpenTelemetry.Instrumentation.MySqlData`](https://www.nuget.org/packages/OpenTelemetry.Instrumentation.MySqlData)
package. Also, add any other instrumentations & exporters you will need.

```shell
dotnet add package OpenTelemetry.Instrumentation.MySqlData
```

### Step 2: Enable MySqlData Instrumentation at application startup

MySqlData instrumentation must be enabled at application startup.

The following example demonstrates adding MySqlData instrumentation to a
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
        using var tracerProvider = Sdk.CreateTracerProviderBuilder()
            .AddMySqlDataInstrumentation()
            .AddConsoleExporter()
            .Build();
    }
}
```

For an ASP.NET Core application, adding instrumentation is typically done in
the `ConfigureServices` of your `Startup` class. Refer to documentation for
[OpenTelemetry.Instrumentation.AspNetCore](../OpenTelemetry.Instrumentation.AspNetCore/README.md).

For an ASP.NET application, adding instrumentation is typically done in the
`Global.asax.cs`. Refer to documentation for [OpenTelemetry.Instrumentation.AspNet](../OpenTelemetry.Instrumentation.AspNet/README.md).

> [!NOTE]
> If you are using `Mysql.Data` 8.0.31 or later, please add
option `Logging=true` in your connection string to enable tracing.
See issue #691 for details.

## Advanced configuration

This instrumentation can be configured to change the default behavior by using
`MySqlDataInstrumentationOptions`.

### Capturing 'db.statement'

The `MySqlDataInstrumentationOptions` class exposes several properties that can
be used to configure how the [`db.statement`](https://github.com/open-telemetry/semantic-conventions/blob/main/docs/database/mysql.md)
attribute is captured upon execution of a query.

#### SetDbStatement

The `SetDbStatement` property can be used to control whether this instrumentation
should set the [`db.statement`](https://github.com/open-telemetry/semantic-conventions/blob/main/docs/database/mysql.md)
attribute to the text of the `MySqlCommand` being executed.

Since `CommandType.Text` might contain sensitive data, SQL capturing is
_disabled_ by default to protect against accidentally sending full query text
to a telemetry backend. If you are only using stored procedures or have no
sensitive data in your `sqlCommand.CommandText`, you can enable SQL capturing
using the options like below:

```csharp
using var tracerProvider = Sdk.CreateTracerProviderBuilder()
    .AddMySqlDataInstrumentation(
        options => options.SetDbStatement = true)
    .AddConsoleExporter()
    .Build();
```

## EnableConnectionLevelAttributes

By default, `EnabledConnectionLevelAttributes` is disabled and this
instrumentation sets the `peer.service` attribute to the
[`DataSource`](https://docs.microsoft.com/dotnet/api/system.data.common.dbconnection.datasource)
property of the connection. If `EnabledConnectionLevelAttributes` is enabled,
the `DataSource` will be parsed and the server name will be sent as the
`net.peer.name` or `net.peer.ip` attribute, and the port will be sent as the
`net.peer.port` attribute.

The following example shows how to use `EnableConnectionLevelAttributes`.

```csharp
using var tracerProvider = Sdk.CreateTracerProviderBuilder()
    .AddMySqlDataInstrumentation(
        options => options.EnableConnectionLevelAttributes = true)
    .AddConsoleExporter()
    .Build();
```

### RecordException

This option can be set to instruct the instrumentation to record Exceptions
as Activity [events](https://github.com/open-telemetry/semantic-conventions/blob/main/docs/exceptions/exceptions-spans.md).

> Due to the limitation of this library's implementation, We cannot get the raw
> `MysqlException`, only exception message is available.

The default value is `false` and can be changed by the code like below.

```csharp
using var tracerProvider = Sdk.CreateTracerProviderBuilder()
    .AddMySqlDataInstrumentation(
        options => options.RecordException = true)
    .AddConsoleExporter()
    .Build();
```

## References

* [OpenTelemetry Project](https://opentelemetry.io/)

* [OpenTelemetry semantic conventions for MySQL](https://github.com/open-telemetry/semantic-conventions/blob/main/docs/database/mysql.md)
