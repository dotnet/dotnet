# Instana Exporter for OpenTelemetry .NET

| Status | |
| ------ | --- |
| Stability | [Deprecated](../../README.md#deprecated) |

[![NuGet version badge](https://img.shields.io/nuget/v/OpenTelemetry.Exporter.Instana)](https://www.nuget.org/packages/OpenTelemetry.Exporter.Instana)
[![NuGet download count badge](https://img.shields.io/nuget/dt/OpenTelemetry.Exporter.Instana)](https://www.nuget.org/packages/OpenTelemetry.Exporter.Instana)
[![codecov.io](https://codecov.io/gh/open-telemetry/opentelemetry-dotnet-contrib/branch/main/graphs/badge.svg?flag=unittests-Exporter.Instana)](https://app.codecov.io/gh/open-telemetry/opentelemetry-dotnet-contrib?flags[0]=unittests-Exporter.Instana)

The Instana Exporter exports telemetry to an Instana backend.

## Deprecated

> [!IMPORTANT]
> Development of this component is halted. No new package versions
> are planned, and new issues will not be worked on.

Consider using [`OpenTelemetry.Exporter.OpenTelemetryProtocol`](https://github.com/open-telemetry/opentelemetry-dotnet/blob/main/src/OpenTelemetry.Exporter.OpenTelemetryProtocol/README.md)
as a replacement.

## Installation

```shell
dotnet add package OpenTelemetry.Exporter.Instana
```

## Configuration

> [!NOTE]
> The Instana exporter only supports traces.

To report to an Instana backend the correct agent key and backend URL must be
configured.

These values can be configured either by the environment variables `INSTANA_AGENT_KEY`
and `INSTANA_ENDPOINT_URL`, or using the `InstanaExporterOptions` class.

```csharp
using var tracerProvider = Sdk.CreateTracerProviderBuilder()
    .AddSource("DemoSource")
    .AddInstanaExporter((options) =>
    {
        options.AgentKey = "instana-agent-key";
        options.EndpointUrl = "https://instana.local";
    })
    .Build();
```

Optionally backend communication timeout can be configured using the environment
variable `INSTANA_TIMEOUT` or the
`InstanaExporterOptions.BatchExportProcessorOptions.ExporterTimeoutMilliseconds`
property.

## Troubleshooting

Before digging into a problem, check if you hit a known issue by looking at the
[GitHub issues](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/issues).
