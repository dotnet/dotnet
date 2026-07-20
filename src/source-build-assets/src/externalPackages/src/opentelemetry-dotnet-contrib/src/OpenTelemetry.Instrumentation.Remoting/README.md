# .NET Remoting Instrumentation for OpenTelemetry .NET

| Status | |
| ------ | --- |
| Stability | [Alpha](../../README.md#alpha) |
| Code Owners | [@lewis800](https://github.com/lewis800) |

[![NuGet version badge](https://img.shields.io/nuget/v/OpenTelemetry.Instrumentation.Remoting)](https://www.nuget.org/packages/OpenTelemetry.Instrumentation.Remoting)
[![NuGet download count badge](https://img.shields.io/nuget/dt/OpenTelemetry.Instrumentation.Remoting)](https://www.nuget.org/packages/OpenTelemetry.Instrumentation.Remoting)
[![codecov.io](https://codecov.io/gh/open-telemetry/opentelemetry-dotnet-contrib/branch/main/graphs/badge.svg?flag=unittests-Instrumentation.Remoting)](https://app.codecov.io/gh/open-telemetry/opentelemetry-dotnet-contrib?flags[0]=unittests-Instrumentation.Remoting)

This is an [Instrumentation
Library](https://github.com/open-telemetry/opentelemetry-specification/blob/main/specification/glossary.md#instrumentation-library),
which instruments [.NET Remoting](https://docs.microsoft.com/previous-versions/dotnet/netframework-3.0/72x4h507(v=vs.85))
and collects telemetry about incoming and outgoing requests on client
and server objects.

.NET Remoting is a [legacy technology](https://docs.microsoft.com/previous-versions/dotnet/netframework-3.0/kwdt6w2k(v=vs.85))
that shouldn't be used for new .NET applications and [doesn't exist](https://docs.microsoft.com/dotnet/core/porting/net-framework-tech-unavailable#remoting)
in .NET 6 and later versions. However, if you do have a legacy application you are
looking to instrument, consider using this package.

## Installation

Add a reference to the
[`OpenTelemetry.Instrumentation.Remoting`](https://www.nuget.org/packages/OpenTelemetry.Instrumentation.Remoting)
package. Also, add any other instrumentations & exporters you will need.

```shell
dotnet add package --prerelease OpenTelemetry.Instrumentation.Remoting
```

## Configuration

To enable .NET remoting instrumentation, call `AddRemotingInstrumentation()` on
the `TracerProviderBuilder` during the application startup in both client and
server code.

The following example demonstrates adding .NET Framework remoting
instrumentation to a client console application. This example also
sets up the OpenTelemetry Console exporter, which requires adding
the [`OpenTelemetry.Exporter.Console`](https://github.com/open-telemetry/opentelemetry-dotnet/blob/main/src/OpenTelemetry.Exporter.Console/README.md)
package to the project.

```csharp
using OpenTelemetry;
using OpenTelemetry.Trace;

namespace ExampleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            using var tracerProvider = Sdk.CreateTracerProviderBuilder()
                .AddRemotingInstrumentation()
                .AddConsoleExporter()
                .Build();
        }
    }
}
```

When hosting server objects in IIS, adding instrumentation should typically
be done in `Global.asax.cs` like in the below example.

This example also sets up the OpenTelemetry OTLP exporter, which requires
adding the package [`OpenTelemetry.Exporter.OpenTelemetryProtocol`](https://github.com/open-telemetry/opentelemetry-dotnet/blob/main/src/OpenTelemetry.Exporter.OpenTelemetryProtocol/README.md)
to the project.

```csharp
using System;
using OpenTelemetry;
using OpenTelemetry.Trace;

namespace ServerAspNet
{
    public class Global : System.Web.HttpApplication
    {
        private IDisposable _tracerProvider;

        protected void Application_Start(object sender, EventArgs e)
        {
            _tracerProvider = Sdk.CreateTracerProviderBuilder()
                .AddRemotingInstrumentation()
                .AddOtlpExporter(o =>
                {
                    o.Endpoint = new Uri("http://localhost:4317");
                })
                .Build();
        }

        // ...

        protected void Application_End(object sender, EventArgs e)
        {
            _tracerProvider?.Dispose();
        }
    }
}
```

Additionally, when using [`HttpChannel`](https://docs.microsoft.com/dotnet/api/system.runtime.remoting.channels.http.httpchannel)
for remoting, consider registering [`OpenTelemetry.Instrumentation.Http`](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/tree/main/src/OpenTelemetry.Instrumentation.Http)
on the client and [`OpenTelemetry.Instrumentation.AspNet`](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/tree/main/src/OpenTelemetry.Instrumentation.AspNet)
on the server.

## Filtering

By default `AddRemotingInstrumentation` will capture all calls leaving
or entering current `AppDomain`. If you are only interested in calls on
specific remote objects, you can use a `Filter` like below:

```csharp
var tracerProvider = Sdk.CreateTracerProviderBuilder()
    .AddRemotingInstrumentation(options =>
        options.Filter = message =>
        {
            // Only capture calls to and from "RemoteObject"
            if (message is IMethodMessage methodMessage)
            {
                return methodMessage.TypeName.Contains("RemoteObject");
            }

            return false;
        })
    .Build()
```

The `Filter` takes an [`IMessage`](https://docs.microsoft.com/dotnet/api/system.runtime.remoting.messaging.imessage)
and returns a boolean. You can inspect the message to decide if you
want to instrument it or not.

## Implementation Details

The instrumentation is implemented via custom
[`IDynamicMessageSink`](https://docs.microsoft.com/dotnet/api/system.runtime.remoting.contexts.idynamicmessagesink)
implementation, that is registered in the current `AppDomain` when you call
`AddRemotingInstrumentation` and unregistered when the constructed
`TracerProvider` is disposed.

## References

* [.NET Remoting Overview](https://docs.microsoft.com/previous-versions/dotnet/articles/ms973857(v=msdn.10))
* [Remoting Sinks and Dynamic Sinks](https://docs.microsoft.com/previous-versions/dotnet/netframework-1.1/xec2wbt4(v=vs.71))
* [OpenTelemetry Project](https://opentelemetry.io/)
