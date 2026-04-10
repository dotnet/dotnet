# AWS SDK client instrumentation for OpenTelemetry

| Status | |
| ------ | --- |
| Stability | [Stable](../../README.md#stable) |
| Code Owners | [@srprash](https://github.com/srprash), [@normj](https://github.com/normj), [@lukeina2z](https://github.com/lukeina2z) |

[![NuGet version badge](https://img.shields.io/nuget/v/OpenTelemetry.Instrumentation.AWS)](https://www.nuget.org/packages/OpenTelemetry.Instrumentation.AWS)
[![NuGet download count badge](https://img.shields.io/nuget/dt/OpenTelemetry.Instrumentation.AWS)](https://www.nuget.org/packages/OpenTelemetry.Instrumentation.AWS)
[![codecov.io](https://codecov.io/gh/open-telemetry/opentelemetry-dotnet-contrib/branch/main/graphs/badge.svg?flag=unittests-Instrumentation.AWS)](https://app.codecov.io/gh/open-telemetry/opentelemetry-dotnet-contrib?flags[0]=unittests-Instrumentation.AWS)

Download the `OpenTelemetry.Instrumentation.AWS` package:

```shell
dotnet add package OpenTelemetry.Instrumentation.AWS
```

Add the AWSXRayIdGenerator and AWSInstrumentation
to your application. The below example is for an ASP.Net Core application.

```csharp
using OpenTelemetry;
using OpenTelemetry.Contrib.Extensions.AWSXRay.Trace;
using OpenTelemetry.Trace;

public void ConfigureServices(IServiceCollection services)
{
    services.AddControllers();
    services.AddOpenTelemetryTracing((builder) => builder
        // for tracing calls to AWS services via AWS SDK for .Net
        .AddAWSInstrumentation()
        .AddAspNetCoreInstrumentation()
        .AddOtlpExporter());
}
```

## Semantic Conventions

_For an overview on Semantic Conventions, see
[OpenTelemetery - Semantic Conventions](https://opentelemetry.io/docs/concepts/semantic-conventions/)_.

While this library is intended for production use, it relies on several
Semantic Conventions that are still considered Experimental, meaning
they may undergo additional changes before becoming Stable.  This can impact
the aggregation and analysis of telemetry signals in environments with
multiple applications or microservices.

For example, a microservice using an older version of the Semantic Conventions
for Http Attributes may emit `"http.method"` with a value of GET, while a
different microservice, using a new version of Semantic Convention may instead
emit the GET as `"http.request.method"`.

Future versions the OpenTelemetry.*.AWS libraries will include updates to the
Semantic Convention, which may break compatibility with a previous version.

The default will remain as `V1_28_0` until the next major version bump.

To opt in to automatic upgrades, you can use `SemanticConventionVersion.Latest`
or you can specify a specific version:

```csharp
using OpenTelemetry;
using OpenTelemetry.AWS;
using OpenTelemetry.Contrib.Extensions.AWSXRay.Trace;
using OpenTelemetry.Trace;

public void ConfigureServices(IServiceCollection services)
{
    services.AddControllers();
    services.AddOpenTelemetryTracing((builder) => builder
        .AddAWSInstrumentation(opt => {
            // pin to a specific Semantic Convention version
            opt.SemanticConventionVersion = SemanticConventionVersion.V1_29_0;
        });
}
```

**NOTE:** Once a Semantic Convention becomes Stable, OpenTelemetry.*.AWS
libraries will remain on that version until the
next major version bump.
