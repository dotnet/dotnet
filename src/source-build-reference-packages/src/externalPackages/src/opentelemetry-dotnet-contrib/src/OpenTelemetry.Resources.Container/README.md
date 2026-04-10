# Container Resource Detectors

| Status | |
| ------ | --- |
| Stability | [Beta](../../README.md#beta) |
| Code Owners | [@iskiselev](https://github.com/iskiselev) |

[![NuGet version badge](https://img.shields.io/nuget/v/OpenTelemetry.Resources.Container)](https://www.nuget.org/packages/OpenTelemetry.Resources.Container)
[![NuGet download count badge](https://img.shields.io/nuget/dt/OpenTelemetry.Resources.Container)](https://www.nuget.org/packages/OpenTelemetry.Resources.Container)
[![codecov.io](https://codecov.io/gh/open-telemetry/opentelemetry-dotnet-contrib/branch/main/graphs/badge.svg?flag=unittests-Resources.Container)](https://app.codecov.io/gh/open-telemetry/opentelemetry-dotnet-contrib?flags[0]=unittests-Resources.Container)

## Getting Started

You need to install the
`OpenTelemetry.Resources.Container` package to be able to use the
Container Resource Detectors.

```shell
dotnet add package OpenTelemetry.Resources.Container --prerelease
```

## Usage

You can configure Container resource detector to
the `ResourceBuilder` with the following example.

```csharp
using OpenTelemetry;
using OpenTelemetry.Resources.Container;

using var tracerProvider = Sdk.CreateTracerProviderBuilder()
    .ConfigureResource(resource => resource.AddContainerDetector())
    // other configurations
    .Build();

using var meterProvider = Sdk.CreateMeterProviderBuilder()
    .ConfigureResource(resource => resource.AddContainerDetector())
    // other configurations
    .Build();

using var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddOpenTelemetry(options =>
    {
        options.SetResourceBuilder(ResourceBuilder.CreateDefault().AddContainerDetector());
    });
});
```

The resource detectors will record the following metadata based on where
your application is running:

- **ContainerDetector**: container.id.

## References

- [OpenTelemetry Project](https://opentelemetry.io/)
