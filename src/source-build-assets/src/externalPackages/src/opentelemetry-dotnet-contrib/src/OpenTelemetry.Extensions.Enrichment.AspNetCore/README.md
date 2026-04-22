# OpenTelemetry .NET SDK ASP.NET Core telemetry enrichment

| Status | |
| ------ | --- |
| Stability | [Development](../../README.md#development) |
| Code Owners | [@evgenyfedorov2](https://github.com/evgenyfedorov2), [@dariusclay](https://github.com/dariusclay) |

[![NuGet version badge](https://img.shields.io/nuget/v/OpenTelemetry.Extensions.Enrichment.AspNetCore)](https://www.nuget.org/packages/OpenTelemetry.Extensions.Enrichment.AspNetCore)
[![NuGet download count badge](https://img.shields.io/nuget/dt/OpenTelemetry.Extensions.Enrichment.AspNetCore)](https://www.nuget.org/packages/OpenTelemetry.Extensions.Enrichment.AspNetCore)
[![codecov.io](https://codecov.io/gh/open-telemetry/opentelemetry-dotnet-contrib/branch/main/graphs/badge.svg?flag=unittests-Extensions.Enrichment.AspNetCore)](https://app.codecov.io/gh/open-telemetry/opentelemetry-dotnet-contrib?flags[0]=unittests-Extensions.Enrichment.AspNetCore)

Contains OpenTelemetry .NET SDK ASP.NET Core telemetry enrichment extensions
which are used for enrichment of logs, metrics, and traces in inbound HTTP requests.

## Introduction

ASP.NET Core Telemetry enrichment attaches various types of information to traces
generated for incoming HTTP requests.
You can use the ASP.NET Core Telemetry enrichment framework to attach any custom
information that you would like to be present in traces for incoming HTTP requests.

With the ASP.NET Core Telemetry enrichment framework, you don't need to worry
about attaching the information carefully to each telemetry object you touch.
Instead, if you implement your enricher class inherited from `AspNetCoreTraceEnricher`,
it  takes care of the details automatically. You simply register your class with
the enrichment framework and the enrichment framework will make sure to call the
enrichment methods of your class for every incoming HTTP request in your app.

## Traces

Currently this package supports trace enrichment only.

### Steps to enable OpenTelemetry.Extensions.Enrichment.AspNetCore

TBD
