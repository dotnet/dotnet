# Application Insights for .NET Apps
[![Build And Test, BASE](https://github.com/microsoft/ApplicationInsights-dotnet/actions/workflows/build-and-test-BASE.yml/badge.svg)](https://github.com/microsoft/ApplicationInsights-dotnet/actions/workflows/build-and-test-BASE.yml)
[![Build And Test, WEB](https://github.com/microsoft/ApplicationInsights-dotnet/actions/workflows/build-and-test-WEB.yml/badge.svg)](https://github.com/microsoft/ApplicationInsights-dotnet/actions/workflows/build-and-test-WEB.yml)
[![Build And Test, NETCORE](https://github.com/microsoft/ApplicationInsights-dotnet/actions/workflows/build-and-test-NETCORE.yml/badge.svg)](https://github.com/microsoft/ApplicationInsights-dotnet/actions/workflows/build-and-test-NETCORE.yml)
[![Build And Test, LOGGING](https://github.com/microsoft/ApplicationInsights-dotnet/actions/workflows/build-and-test-LOGGING.yml/badge.svg)](https://github.com/microsoft/ApplicationInsights-dotnet/actions/workflows/build-and-test-LOGGING.yml)
[![Redfield Validation](https://github.com/microsoft/ApplicationInsights-dotnet/actions/workflows/redfield-sanity-check.yml/badge.svg)](https://github.com/microsoft/ApplicationInsights-dotnet/actions/workflows/redfield-sanity-check.yml)
[![Sanity Build](https://github.com/microsoft/ApplicationInsights-dotnet/actions/workflows/sanity.yml/badge.svg)](https://github.com/microsoft/ApplicationInsights-dotnet/actions/workflows/sanity.yml)
[![Nightly](https://github.com/microsoft/ApplicationInsights-dotnet/actions/workflows/nightly.yml/badge.svg)](https://github.com/microsoft/ApplicationInsights-dotnet/actions/workflows/nightly.yml)


This is the .NET SDK for sending data to [Azure Monitor](https://docs.microsoft.com/azure/azure-monitor/overview) & [Application Insights](https://docs.microsoft.com/azure/azure-monitor/app/app-insights-overview).

## Getting Started

Please review our How-to guides to review which packages are appropriate for your project:

* [Console App](https://docs.microsoft.com/azure/azure-monitor/app/console)
* [ASP.NET](https://docs.microsoft.com/azure/azure-monitor/app/asp-net)
* [ASP.NET Core](https://docs.microsoft.com/azure/azure-monitor/app/asp-net-core)
* [ILogger](https://docs.microsoft.com/azure/azure-monitor/app/ilogger)
* [WorkerService](https://docs.microsoft.com/azure/azure-monitor/app/worker-service)

### Understanding our SDK

We've gathered a list of concepts, code examples, and links to full guides [here](docs/concepts.md).

## Contributing

We strongly welcome and encourage contributions to this project.
Please review our [Contributing guide](.github/CONTRIBUTING.md).

## Branches

- [main](https://github.com/Microsoft/ApplicationInsights-dotnet/tree/master) is the default branch for all development and releases.

## Releases

- Refer to our [Milestones](https://github.com/microsoft/ApplicationInsights-dotnet/milestones) for progress on our next releases.
- [Releases](https://github.com/microsoft/ApplicationInsights-dotnet/releases)
- [Tags](https://github.com/microsoft/ApplicationInsights-dotnet/tags) all releases have a matching tag.

## NuGet packages

The following packages are published from this repository:

|  | Nightly Build | Latest Official Release | Latest Official Release (including pre-release) |
|---|---|---|---|
| **Base SDKs** |  |  |  |
| - [Microsoft.ApplicationInsights](https://www.nuget.org/packages/Microsoft.ApplicationInsights/) | [![Nightly](https://img.shields.io/myget/applicationinsights-dotnet-nightly/v/Microsoft.ApplicationInsights?label=)](https://www.myget.org/feed/applicationinsights-dotnet-nightly/package/nuget/Microsoft.ApplicationInsights) | [![Nuget](https://img.shields.io/nuget/v/Microsoft.ApplicationInsights.svg)](https://www.nuget.org/packages/Microsoft.ApplicationInsights/) | [![Nuget](https://img.shields.io/nuget/vpre/Microsoft.ApplicationInsights.svg)](https://www.nuget.org/packages/Microsoft.ApplicationInsights/) |
| - [Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel](https://www.nuget.org/packages/Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel) | [![Nightly](https://img.shields.io/myget/applicationinsights-dotnet-nightly/v/Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel?label=)](https://www.myget.org/feed/applicationinsights-dotnet-nightly/package/nuget/Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel) | [![Nuget](https://img.shields.io/nuget/v/Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.svg)](https://www.nuget.org/packages/Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel/) | [![Nuget](https://img.shields.io/nuget/vpre/Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.svg)](https://www.nuget.org/packages/Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel/) |
| **Auto Collectors (Generic)** |  |  |  |
| - [Microsoft.ApplicationInsights.DependencyCollector](https://www.nuget.org/packages/Microsoft.ApplicationInsights.DependencyCollector/) | [![Nightly](https://img.shields.io/myget/applicationinsights-dotnet-nightly/v/Microsoft.ApplicationInsights.DependencyCollector?label=)](https://www.myget.org/feed/applicationinsights-dotnet-nightly/package/nuget/Microsoft.ApplicationInsights.DependencyCollector) | [![Nuget](https://img.shields.io/nuget/v/Microsoft.ApplicationInsights.DependencyCollector.svg)](https://nuget.org/packages/Microsoft.ApplicationInsights.DependencyCollector) | [![Nuget](https://img.shields.io/nuget/vpre/Microsoft.ApplicationInsights.DependencyCollector.svg)](https://nuget.org/packages/Microsoft.ApplicationInsights.DependencyCollector) |
| - [Microsoft.ApplicationInsights.EventCounterCollector](https://www.nuget.org/packages/Microsoft.ApplicationInsights.EventCounterCollector) | [![Nightly](https://img.shields.io/myget/applicationinsights-dotnet-nightly/v/Microsoft.ApplicationInsights.EventCounterCollector?label=)](https://www.myget.org/feed/applicationinsights-dotnet-nightly/package/nuget/Microsoft.ApplicationInsights.EventCounterCollector) | [![Nuget](https://img.shields.io/nuget/v/Microsoft.ApplicationInsights.EventCounterCollector.svg)](https://nuget.org/packages/Microsoft.ApplicationInsights.EventCounterCollector) | [![Nuget](https://img.shields.io/nuget/vpre/Microsoft.ApplicationInsights.EventCounterCollector.svg)](https://nuget.org/packages/Microsoft.ApplicationInsights.EventCounterCollector) |
| - [Microsoft.ApplicationInsights.PerfCounterCollector](https://www.nuget.org/packages/Microsoft.ApplicationInsights.PerfCounterCollector/) | [![Nightly](https://img.shields.io/myget/applicationinsights-dotnet-nightly/v/Microsoft.ApplicationInsights.PerfCounterCollector?label=)](https://www.myget.org/feed/applicationinsights-dotnet-nightly/package/nuget/Microsoft.ApplicationInsights.PerfCounterCollector) | [![Nuget](https://img.shields.io/nuget/v/Microsoft.ApplicationInsights.PerfCounterCollector.svg)](https://nuget.org/packages/Microsoft.ApplicationInsights.PerfCounterCollector) | [![Nuget](https://img.shields.io/nuget/vpre/Microsoft.ApplicationInsights.PerfCounterCollector.svg)](https://nuget.org/packages/Microsoft.ApplicationInsights.PerfCounterCollector) |
| - [Microsoft.ApplicationInsights.WindowsServer](https://www.nuget.org/packages/Microsoft.ApplicationInsights.WindowsServer/) | [![Nightly](https://img.shields.io/myget/applicationinsights-dotnet-nightly/v/Microsoft.ApplicationInsights.WindowsServer?label=)](https://www.myget.org/feed/applicationinsights-dotnet-nightly/package/nuget/Microsoft.ApplicationInsights.WindowsServer) | [![Nuget](https://img.shields.io/nuget/v/Microsoft.ApplicationInsights.WindowsServer.svg)](https://nuget.org/packages/Microsoft.ApplicationInsights.WindowsServer) | [![Nuget](https://img.shields.io/nuget/vpre/Microsoft.ApplicationInsights.WindowsServer.svg)](https://nuget.org/packages/Microsoft.ApplicationInsights.WindowsServer) |
| **Auto Collectors (ASP.NET)** |  |  |  |
| - [Microsoft.ApplicationInsights.Web](https://www.nuget.org/packages/Microsoft.ApplicationInsights.Web/) | [![Nightly](https://img.shields.io/myget/applicationinsights-dotnet-nightly/v/Microsoft.ApplicationInsights.Web?label=)](https://www.myget.org/feed/applicationinsights-dotnet-nightly/package/nuget/Microsoft.ApplicationInsights.Web) | [![Nuget](https://img.shields.io/nuget/v/Microsoft.ApplicationInsights.Web.svg)](https://nuget.org/packages/Microsoft.ApplicationInsights.Web) | [![Nuget](https://img.shields.io/nuget/vpre/Microsoft.ApplicationInsights.Web.svg)](https://nuget.org/packages/Microsoft.ApplicationInsights.Web) |
| **Auto Collectors (ASP.NET Core)** |  |  |  |
| - [Microsoft.ApplicationInsights.AspNetCore](https://www.nuget.org/packages/Microsoft.ApplicationInsights.AspNetCore/) | [![Nightly](https://img.shields.io/myget/applicationinsights-dotnet-nightly/v/Microsoft.ApplicationInsights.AspNetCore?label=)](https://www.myget.org/feed/applicationinsights-dotnet-nightly/package/nuget/Microsoft.ApplicationInsights.AspNetCore) | [![Nuget](https://img.shields.io/nuget/v/Microsoft.ApplicationInsights.AspNetCore.svg)](https://nuget.org/packages/Microsoft.ApplicationInsights.AspNetCore) | [![Nuget](https://img.shields.io/nuget/vpre/Microsoft.ApplicationInsights.AspNetCore.svg)](https://nuget.org/packages/Microsoft.ApplicationInsights.AspNetCore) |
| **Auto Collectors (WorkerService, Console Application, etc.)** |  |  |  |
| - [Microsoft.ApplicationInsights.WorkerService](https://www.nuget.org/packages/Microsoft.ApplicationInsights.WorkerService/) | [![Nightly](https://img.shields.io/myget/applicationinsights-dotnet-nightly/v/Microsoft.ApplicationInsights.WorkerService?label=)](https://www.myget.org/feed/applicationinsights-dotnet-nightly/package/nuget/Microsoft.ApplicationInsights.WorkerService) | [![Nuget](https://img.shields.io/nuget/v/Microsoft.ApplicationInsights.WorkerService.svg)](https://nuget.org/packages/Microsoft.ApplicationInsights.WorkerService) | [![Nuget](https://img.shields.io/nuget/vpre/Microsoft.ApplicationInsights.WorkerService.svg)](https://nuget.org/packages/Microsoft.ApplicationInsights.WorkerService) |
| **Logging Adapters** |  |  |  |
| - For `ILogger`: [Microsoft.Extensions.Logging.ApplicationInsights](https://www.nuget.org/packages/Microsoft.Extensions.Logging.ApplicationInsights/) | [![Nightly](https://img.shields.io/myget/applicationinsights-dotnet-nightly/v/Microsoft.Extensions.Logging.ApplicationInsights?label=)](https://www.myget.org/feed/applicationinsights-dotnet-nightly/package/nuget/Microsoft.Extensions.Logging.ApplicationInsights) | [![Nuget](https://img.shields.io/nuget/v/Microsoft.Extensions.Logging.ApplicationInsights.svg)](https://www.nuget.org/packages/Microsoft.Extensions.Logging.ApplicationInsights/) | [![Nuget](https://img.shields.io/nuget/vpre/Microsoft.Extensions.Logging.ApplicationInsights.svg)](https://www.nuget.org/packages/Microsoft.Extensions.Logging.ApplicationInsights/) |
| - For `NLog`: [Microsoft.ApplicationInsights.NLogTarget](http://www.nuget.org/packages/Microsoft.ApplicationInsights.NLogTarget/) | [![Nightly](https://img.shields.io/myget/applicationinsights-dotnet-nightly/v/Microsoft.ApplicationInsights.NLogTarget?label=)](https://www.myget.org/feed/applicationinsights-dotnet-nightly/package/nuget/Microsoft.ApplicationInsights.NLogTarget) | [![Nuget](https://img.shields.io/nuget/v/Microsoft.ApplicationInsights.NLogTarget.svg)](https://www.nuget.org/packages/Microsoft.ApplicationInsights.NLogTarget/) | [![Nuget](https://img.shields.io/nuget/vpre/Microsoft.ApplicationInsights.NLogTarget.svg)](https://www.nuget.org/packages/Microsoft.ApplicationInsights.NLogTarget/) |
| - For `Log4Net`: [Microsoft.ApplicationInsights.Log4NetAppender](http://www.nuget.org/packages/Microsoft.ApplicationInsights.Log4NetAppender/) | [![Nightly](https://img.shields.io/myget/applicationinsights-dotnet-nightly/v/Microsoft.ApplicationInsights.Log4NetAppender?label=)](https://www.myget.org/feed/applicationinsights-dotnet-nightly/package/nuget/Microsoft.ApplicationInsights.Log4NetAppender) | [![Nuget](https://img.shields.io/nuget/v/Microsoft.ApplicationInsights.Log4NetAppender.svg)](https://www.nuget.org/packages/Microsoft.ApplicationInsights.Log4NetAppender/) | [![Nuget](https://img.shields.io/nuget/vpre/Microsoft.ApplicationInsights.Log4NetAppender.svg)](https://www.nuget.org/packages/Microsoft.ApplicationInsights.Log4NetAppender/) |
| - For `System.Diagnostics`: [Microsoft.ApplicationInsights.TraceListener](http://www.nuget.org/packages/Microsoft.ApplicationInsights.TraceListener/) | [![Nightly](https://img.shields.io/myget/applicationinsights-dotnet-nightly/v/Microsoft.ApplicationInsights.TraceListener?label=)](https://www.myget.org/feed/applicationinsights-dotnet-nightly/package/nuget/Microsoft.ApplicationInsights.TraceListener) | [![Nuget](https://img.shields.io/nuget/v/Microsoft.ApplicationInsights.TraceListener.svg)](https://www.nuget.org/packages/Microsoft.ApplicationInsights.TraceListener/) | [![Nuget](https://img.shields.io/nuget/vpre/Microsoft.ApplicationInsights.TraceListener.svg)](https://www.nuget.org/packages/Microsoft.ApplicationInsights.TraceListener/) |
| - [Microsoft.ApplicationInsights.DiagnosticSourceListener](http://www.nuget.org/packages/Microsoft.ApplicationInsights.DiagnosticSourceListener/) | [![Nightly](https://img.shields.io/myget/applicationinsights-dotnet-nightly/v/Microsoft.ApplicationInsights.DiagnosticSourceListener?label=)](https://www.myget.org/feed/applicationinsights-dotnet-nightly/package/nuget/Microsoft.ApplicationInsights.DiagnosticSourceListener) | [![Nuget](https://img.shields.io/nuget/v/Microsoft.ApplicationInsights.DiagnosticSourceListener.svg)](https://www.nuget.org/packages/Microsoft.ApplicationInsights.DiagnosticSourceListener/) | [![Nuget](https://img.shields.io/nuget/vpre/Microsoft.ApplicationInsights.DiagnosticSourceListener.svg)](https://www.nuget.org/packages/Microsoft.ApplicationInsights.DiagnosticSourceListener/) |
| - [Microsoft.ApplicationInsights.EtwCollector](http://www.nuget.org/packages/Microsoft.ApplicationInsights.EtwCollector/) | [![Nightly](https://img.shields.io/myget/applicationinsights-dotnet-nightly/v/Microsoft.ApplicationInsights.EtwCollector?label=)](https://www.myget.org/feed/applicationinsights-dotnet-nightly/package/nuget/Microsoft.ApplicationInsights.EtwCollector) | [![Nuget](https://img.shields.io/nuget/v/Microsoft.ApplicationInsights.EtwCollector.svg)](https://www.nuget.org/packages/Microsoft.ApplicationInsights.EtwCollector/) | [![Nuget](https://img.shields.io/nuget/vpre/Microsoft.ApplicationInsights.EtwCollector.svg)](https://www.nuget.org/packages/Microsoft.ApplicationInsights.EtwCollector/) |
| - [Microsoft.ApplicationInsights.EventSourceListener](http://www.nuget.org/packages/Microsoft.ApplicationInsights.EventSourceListener/) | [![Nightly](https://img.shields.io/myget/applicationinsights-dotnet-nightly/v/Microsoft.ApplicationInsights.EventSourceListener?label=)](https://www.myget.org/feed/applicationinsights-dotnet-nightly/package/nuget/Microsoft.ApplicationInsights.EventSourceListener) | [![Nuget](https://img.shields.io/nuget/v/Microsoft.ApplicationInsights.EventSourceListener.svg)](https://www.nuget.org/packages/Microsoft.ApplicationInsights.EventSourceListener/) | [![Nuget](https://img.shields.io/nuget/vpre/Microsoft.ApplicationInsights.EventSourceListener.svg)](https://www.nuget.org/packages/Microsoft.ApplicationInsights.EventSourceListener/) |

Customers are encouraged to use the latest stable version as that is the version that will get fixes and updates. 
Beta packages are not recommended for use in production & support is limited. 
Upon release of the new stable version, the old Beta packages will be unlisted & the old minor version will be marked as deprecated. 
Application Insights follows the [Azure SDK Lifecycle & support policy](https://azure.github.io/azure-sdk/policies_support.html). 
(For example: When 2.20.0 is released, 2.20.0-beta1 will be unlisted and 2.19.0 will be deprecated.)

### Nightly

Nightly Builds are available on our MyGet feed:
`https://www.myget.org/F/applicationinsights-dotnet-nightly/api/v3/index.json`

These builds come from the main branch. These are not signed and are not intended for production workloads.

## Support

A guide on common troubleshooting topics is available [here](troubleshooting).

For immediate support relating to the Application Insights .NET SDK we encourage you to file an [Azure Support Request](https://docs.microsoft.com/azure/azure-portal/supportability/how-to-create-azure-support-request) with Microsoft Azure instead of filing a GitHub Issue in this repository. 
You can do so by going online to the [Azure portal](https://portal.azure.com/) and submitting a support request. Access to subscription management and billing support is included with your Microsoft Azure subscription, and technical support is provided through one of the [Azure Support Plans](https://azure.microsoft.com/support/plans/). For step-by-step guidance for the Azure portal, see [How to create an Azure support request](https://docs.microsoft.com/azure/azure-portal/supportability/how-to-create-azure-support-request). Alternatively, you can create and manage your support tickets programmatically using the [Azure Support ticket REST API](https://docs.microsoft.com/rest/api/support/).

