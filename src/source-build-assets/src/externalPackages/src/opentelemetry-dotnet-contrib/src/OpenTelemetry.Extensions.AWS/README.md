# Tracing with AWS Distro for OpenTelemetry .Net SDK

| Status | |
| ------ | --- |
| Stability | [Stable](../../README.md#stable) |
| Code Owners | [@srprash](https://github.com/srprash), [@normj](https://github.com/normj), [@lukeina2z](https://github.com/lukeina2z) |

[![NuGet version badge](https://img.shields.io/nuget/v/OpenTelemetry.Extensions.AWS)](https://www.nuget.org/packages/OpenTelemetry.Extensions.AWS)
[![NuGet download count badge](https://img.shields.io/nuget/dt/OpenTelemetry.Extensions.AWS)](https://www.nuget.org/packages/OpenTelemetry.Extensions.AWS)
[![codecov.io](https://codecov.io/gh/open-telemetry/opentelemetry-dotnet-contrib/branch/main/graphs/badge.svg?flag=unittests-Extensions.AWS)](https://app.codecov.io/gh/open-telemetry/opentelemetry-dotnet-contrib?flags[0]=unittests-Extensions.AWS)

If you want to send the traces to AWS X-Ray, you can do so
by using AWS Distro with the OpenTelemetry SDK.

## Getting Started

The OpenTelemetry SDK generates traces with W3C random ID which X-Ray
backend doesn't currently support. You need to install the
`OpenTelemetry.Extensions.AWS` to be able to use the
AWS X-Ray id generator which generates X-Ray compatible trace IDs.
If you plan to call another application instrumented with AWS X-Ray SDK,
you'll need to configure the AWS X-Ray propagator as well.

```shell
dotnet add package OpenTelemetry.Extensions.AWS
```

## Usage

### AWS X-Ray Id Generator and Propagator

Configure AWS X-Ray ID generator and propagator globally in your
application as follows. Make sure to call `AddXRayTraceId()` in the
very beginning when creating `TracerProvider`.

```csharp
using OpenTelemetry;
using OpenTelemetry.Extensions.AWS.Trace;
using OpenTelemetry.Trace;

var tracerProvider = Sdk.CreateTracerProviderBuilder()
                        .AddXRayTraceId()
                        // other instrumentations
                        ...
                        .Build();

Sdk.SetDefaultTextMapPropagator(new AWSXRayPropagator());
```

## References

- [OpenTelemetry Project](https://opentelemetry.io/)
- [AWS Distro for OpenTelemetry .NET](https://aws-otel.github.io/docs/getting-started/dotnet-sdk)
