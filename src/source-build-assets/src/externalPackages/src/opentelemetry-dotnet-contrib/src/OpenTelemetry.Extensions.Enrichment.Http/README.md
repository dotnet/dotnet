# OpenTelemetry .NET SDK HTTP telemetry enrichment

| Status | |
| ------ | --- |
| Stability | [Development](../../README.md#development) |
| Code Owners | [@evgenyfedorov2](https://github.com/evgenyfedorov2), [@dariusclay](https://github.com/dariusclay) |

[![NuGet version badge](https://img.shields.io/nuget/v/OpenTelemetry.Extensions.Enrichment.Http)](https://www.nuget.org/packages/OpenTelemetry.Extensions.Enrichment.Http)
[![NuGet download count badge](https://img.shields.io/nuget/dt/OpenTelemetry.Extensions.Enrichment.Http)](https://www.nuget.org/packages/OpenTelemetry.Extensions.Enrichment.Http)
[![codecov.io](https://codecov.io/gh/open-telemetry/opentelemetry-dotnet-contrib/branch/main/graphs/badge.svg?flag=unittests-Extensions.Enrichment.Http)](https://app.codecov.io/gh/open-telemetry/opentelemetry-dotnet-contrib?flags[0]=unittests-Extensions.Enrichment.Http)

Contains OpenTelemetry .NET SDK HTTP telemetry enrichment extensions
which are used for enrichment of logs, metrics, and traces in outbound HTTP requests.

## Introduction

HTTP Telemetry enrichment attaches various types of information to traces
generated for outgoing HTTP requests.
You can use the HTTP Telemetry enrichment framework to attach any custom
information that you would like to be present in traces for outgoing HTTP requests.

With the HTTP Telemetry enrichment framework, you don't need to worry
about attaching the information carefully to each telemetry object you touch.
Instead, if you implement your enricher class inherited from `HttpClientTraceEnricher`,
it  takes care of the details automatically. You simply register your class with
the enrichment framework and the enrichment framework will make sure to call the
enrichment methods of your class for every outgoing HTTP request in your app.

## Traces

Currently this package supports trace enrichment only.

### Steps to enable OpenTelemetry.Extensions.Enrichment.Http

TBD
