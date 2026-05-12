# gRPC Core-based Client and Server Interceptors for OpenTelemetry .NET

| Status | |
| ------ | --- |
| Stability | [Beta](../../README.md#beta) |
| Code Owners | [@IliaBrahinets](https://github.com/IliaBrahinets), [@pcwiese](https://github.com/pcwiese) |

[![NuGet version badge](https://img.shields.io/nuget/v/OpenTelemetry.Instrumentation.GrpcCore)](https://www.nuget.org/packages/OpenTelemetry.Instrumentation.GrpcCore)
[![NuGet download count badge](https://img.shields.io/nuget/dt/OpenTelemetry.Instrumentation.GrpcCore)](https://www.nuget.org/packages/OpenTelemetry.Instrumentation.GrpcCore)
[![codecov.io](https://codecov.io/gh/open-telemetry/opentelemetry-dotnet-contrib/branch/main/graphs/badge.svg?flag=unittests-Instrumentation.GrpcCore)](https://app.codecov.io/gh/open-telemetry/opentelemetry-dotnet-contrib?flags[0]=unittests-Instrumentation.GrpcCore)

Adds OpenTelemetry instrumentation for gRPC Core-based client and server calls.

gRPC Core is the predecessor to ASP.NET Core gRPC. See <https://github.com/grpc/grpc/tree/master/src/csharp>

For ASP.NET Core gRPC client instrumentation see
[OpenTelemetry.Instrumentation.GrpcNetClient](../OpenTelemetry.Instrumentation.GrpcNetClient/README.md).

For ASP.NET Core gRPC server instrumentation is bundled within the AspNetCore
instrumentation.

Each inbound or outbound gRPC call will generate a Span which follows the
[semantic RPC specification](https://github.com/open-telemetry/semantic-conventions/blob/main/docs/rpc/rpc-spans.md).

## Installation

```shell
dotnet add package OpenTelemetry.Instrumentation.GrpcCore
```

## Configuration

ASP.NET Core instrumentation example:

```csharp
// Add OpenTelemetry and gRPC Core instrumentation
services.AddOpenTelemetry().WithTracing(x =>
{
    x.AddGrpcCoreInstrumentation();
    ...
    // Add exporter, etc.
});
```

Once configured, the OpenTelemetry SDK will listen for the activities created
by the client and server interceptors.

Create a client interceptor:

```csharp
var clientInterceptor = new ClientTracingInterceptor(new ClientTracingInterceptorOptions());
```

Create a server interceptor:

```csharp
var serverInterceptor = new ServerTracingInterceptor(new ServerTracingInterceptorOptions());
```

Then simply add them as you would any other gRPC Core interceptor.
