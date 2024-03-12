# SDK Version

SDK version is a field you can specify on every telemetry item. This field represent the specific SDK collected this particular item. This field is used for troubleshooting.

To review version numbers click [here](https://github.com/Microsoft/ApplicationInsights-Home/wiki/SDK-Release-Schedule).

## SDK Version Specification

SDKs are required to include their name and version in the telemetry item using the `ai.internal.sdkVersion` tag conforming to the format below.

```
{
  "tags": {
    "ai.internal.sdkVersion:" "dotnet:2.0.0"
  }
}
```

### SDK Version Format

```
  [PREFIX_]SDKNAME:SEMVER
```  



| Section          | Required | Description                                                             | Example |
|------------------|----------|-------------------------------------------------------------------------|---------|
| Prefix           | No       | An optional single lowercase letter (a-z) followed by an underscore (_) | a_      |
| SDK Name         | Yes      | An alpha lowercase string (a-z)                                         | dotnet  |
| Semantic Version | Yes      | A [Semantic Versioning](http://semver.org/) compatible version string   | 2.0.0   |

SDK name and semver are delimited by a single colon (:).

### Examples

```
  r_dotnet:2.0.0-12345
    dotnet:2.0.0-beta.1
  | ------ ------------
  |    |        |
  |    |        +-------> Semantic Version Format
  |    |
  |    +----------------> SDK Name
  |
  +---------------------> Prefix (optional)
```

## SDK Names

Define your own SDK name and send PR to update the list below. Please do not re-use the same SDK name.

| Name | Description | Links |
| --- | --- | --- |
| ai-k8s | Kubrnetes module | [github](https://github.com/Microsoft/ApplicationInsights-Kubernetes/blob/578f20e824e6248029554a1f8990b29c4a7c6d11/src/ApplicationInsights.Kubernetes/Utilities/SDKVersionUtils.cs#L34)
| angular | Unofficial Angular telemetry collection module for Application Insights | [github](https://github.com/VladimirRybalko/angular-applicationinsights/blob/244a003a6df2df487d903c99f75fd497d698dede/src/ApplicationInsights.ts#L47) [npmjs](https://www.npmjs.com/package/angular-applicationinsights)
| ap | Application Insights Profiler: Getting call traces, diagnose application performance | [github](https://github.com/Microsoft/ApplicationInsights-Profiler-AspNetCore) [nuget](https://www.nuget.org/packages/Microsoft.ApplicationInsights.Profiler.AspNetCore)
| apim | Telemetry sent from Azure API Management | |
| aspnet5f | ASP.NET Core SDK targetting .NET Framework | [github](https://github.com/Microsoft/ApplicationInsights-aspnetcore/releases)
| aspnet5c | ASP.NET Core SDK targetting .NET Core | [github](https://github.com/Microsoft/ApplicationInsights-aspnetcore/releases)
| azurefunctions | Telemetry produced by Azure Functions Host instrumentation | [github](https://github.com/Azure/azure-functions-host/blob/1f243e9febc4d431af3f0341bc8af74975d51659/src/WebJobs.Script/Host/ScriptTelemetryClientFactory.cs#L28)
| azurefunctionscoretools | Azure Functions Core Tools for local development experience | [github](https://github.com/Azure/azure-functions-core-tools/blob/acb5fd3b8d8fd77420ec500861c995ade2cead69/src/Azure.Functions.Cli/Diagnostics/ConsoleTelemetryClientFactory.cs#L22)
| azwapc | Performance counters collected via Azure App Services extensibility | [github](https://github.com/Microsoft/ApplicationInsights-dotnet-server/blob/eb884b81c568b1054f9b7168ea4b0ec61f9e3506/Src/PerformanceCollector/Perf.Shared/Implementation/PerformanceCounterUtility.cs#L27)
| azwapccore | .Net Core apps running in azure webapp | |
| dotnet | Base .NET SDK API was used to Track telemetry item, either manually, or from SDK that does not supply its own version. | [github](https://github.com/Microsoft/ApplicationInsights-dotnet/releases)
| dotnetc | Base .NET Core SDK API was used to Track telemetry item, either manually, or from SDK that does not supply its own version. | [github](https://github.com/Microsoft/ApplicationInsights-dotnet/releases)
| dsl | DiagnosticSource listener (Microsoft.ApplicationInsights.DiagnosticSourceListener) | [github](https://github.com/Microsoft/ApplicationInsights-dotnet-logging) [nuget](https://www.nuget.org/packages/Microsoft.ApplicationInsights.DiagnosticSourceListener)
| etw | ETW listener (Microsoft.ApplicationInsights.EtwCollector) | [github](https://github.com/Microsoft/ApplicationInsights-dotnet-logging) [nuget](https://www.nuget.org/packages/Microsoft.ApplicationInsights.EtwCollector)
| evl | EventSource listener (Microsoft.ApplicationInsights.EventSourceListener) | [github](https://github.com/Microsoft/ApplicationInsights-dotnet-logging) [nuget](https://www.nuget.org/packages/Microsoft.ApplicationInsights.EventSourceSourceListener)
| evtc | EventCounter collector | [github](https://github.com/microsoft/ApplicationInsights-dotnet/tree/develop/WEB/Src/EventCounterCollector)
| exstat | Experimental exceptions statistics feature | [github](https://github.com/Microsoft/ApplicationInsights-dotnet-server/blob/eb884b81c568b1054f9b7168ea4b0ec61f9e3506/Src/WindowsServer/WindowsServer.Net45/FirstChanceExceptionStatisticsTelemetryModule.cs#L102)
| go-oc | Opencensus for Go | [github](https://github.com/census-instrumentation/opencensus-go)
| hbnet | Heartbeat telemetry sent in intervals reported this metric item for the dotnet SDK | [github](https://github.com/Microsoft/ApplicationInsights-dotnet/releases)
| ios / osx | |
| ilf | Old ILogger adapter for ILogger (.NET Framework) | [github](https://github.com/Microsoft/ApplicationInsights-aspnetcore/wiki/Logging)
| ilc | Old ILogger adapter for ILogger (.NET Core) | [github](https://github.com/Microsoft/ApplicationInsights-aspnetcore/wiki/Logging)
| il | ILogger adapter for ILogger (.NET Core) | [github](https://github.com/microsoft/ApplicationInsights-dotnet-logging/tree/develop/src/ILogger)
| java | java SDK | [github](https://github.com/Microsoft/ApplicationInsights-java/releases)
| javascript | JavaScript SDK | [github](https://github.com/Microsoft/ApplicationInsights-js/releases)
| log4net | .NET logging adapter for log4net (Microsoft.ApplicationInsights.Log4NetAppender) | [github](https://github.com/Microsoft/ApplicationInsights-dotnet-logging) [nuget](https://www.nuget.org/packages/Microsoft.ApplicationInsights.Log4NetAppender)
| logary | Telemetry produced by F# logging library Logary | [github](https://github.com/logary/logary/blob/f86bdf05c66ab0387598f0bb3040c0dafe1f92b8/src/targets/Logary.Targets.ApplicationInsights/Targets_AppInsights.fs#L72-L74)
| m-agg / m-agg2 | metric aggregation pipeline reported this metric | [github](https://github.com/Microsoft/ApplicationInsights-dotnet/releases)
| m-agg2c | metric aggregation pipeline (.net core) reported this metric | [github](https://github.com/Microsoft/ApplicationInsights-dotnet/releases)
| nlog | .NET logging adapter for nlog (Microsoft.ApplicationInsights.NLogTarget) | [github](https://github.com/Microsoft/ApplicationInsights-dotnet-logging) [nuget](https://www.nuget.org/packages/Microsoft.ApplicationInsights.NLogTarget)
| node| node.js SDK | [github](https://github.com/Microsoft/ApplicationInsights-node.js/releases)
| one-line-ps | | [apmtips](http://apmtips.com/blog/2017/03/27/oneliner-to-send-event-to-application-insights/)
| owin | May point to unofficial OWIN telemetry module | [github](https://github.com/MatthewRudolph/Airy-ApplicationInsights-Owin/blob/a555ddc810edb5b9e8d4866c41ba18ddf793bc1d/src/Dematt.Airy.ApplicationInsights.Owin/ExceptionTracking/MvcExceptionHandler.cs#L38)
| pc | performance counters | [github](https://github.com/Microsoft/ApplicationInsights-dotnet-server/releases)
| pccore | performance counters from .Net Core | [github](https://github.com/Microsoft/ApplicationInsights-dotnet-server/releases)
| py2 | python SDK for py2 application | [github](https://github.com/Microsoft/ApplicationInsights-Python/blob/7ac535f451383d78d63bfc2b8aad518cdde598c7/applicationinsights/channel/TelemetryChannel.py#L9-L15)
| py3 | python SDK for py3 application | [github](https://github.com/Microsoft/ApplicationInsights-Python/blob/7ac535f451383d78d63bfc2b8aad518cdde598c7/applicationinsights/channel/TelemetryChannel.py#L9-L15)
| python-oc | Opencensus for Python | [github](https://github.com/census-instrumentation/opencensus-python)
| rddf | Remote dependency telemetry collected via Framework instrumentation (Event Source) | [github](https://github.com/Microsoft/ApplicationInsights-dotnet-server/releases)
| rddfd | Telemetry was processed via framework and diagnosticsource paths. Deprecated in latest versions of SDK |
| rddp | Remote dependency telemetry collected via Profiler instrumentation | [github](https://github.com/Microsoft/ApplicationInsights-dotnet-server/releases)
| rddsr | Azure Service Fabric service remoting call - Client side | [github](https://github.com/Microsoft/ApplicationInsights-ServiceFabric/blob/275166d8034f1b94881982073e304166fbaef6bd/src/ApplicationInsights.ServiceFabric.Native.Shared/DependencyTrackingModule/ServiceRemotingClientEventListener.cs#L41)
| rdddsc | Remote dependency telemetry collected via Diagnostic Source for .NET Core | [github](https://github.com/Microsoft/ApplicationInsights-dotnet-server/releases)
| rdddsd | Remote dependency telemetry collected via Diagnostic Source for Desktop framework | [github](https://github.com/Microsoft/ApplicationInsights-dotnet-server/releases)
| rb | Ruby SDK | [github](https://github.com/Microsoft/ApplicationInsights-Ruby/blob/c78bb54c8b5c0f70218482219fb8447416cfe550/lib/application_insights/channel/telemetry_channel.rb#L89)
| sc | Snapshot Debugger (Microsoft.ApplicationInsights.SnapshotCollector) | [nuget](https://www.nuget.org/packages/Microsoft.ApplicationInsights.SnapshotCollector)
| sd | System diagnostics trace (Microsoft.ApplicationInsights.TraceListener) | [github](https://github.com/Microsoft/ApplicationInsights-dotnet-logging) [nuget](https://www.nuget.org/packages/Microsoft.ApplicationInsights.TraceListener)
| serviceremoting | Azure Service Fabric service remoting call - Server side | [github](https://github.com/Microsoft/ApplicationInsights-ServiceFabric/blob/275166d8034f1b94881982073e304166fbaef6bd/src/ApplicationInsights.ServiceFabric.Native.Shared/RequestTrackingModule/ServiceRemotingServerEventListener.cs#L29)
| unobs | unobserved exceptions - part of web SDK | [github](https://github.com/Microsoft/ApplicationInsights-dotnet-server/releases)
| unhnd | unhandled exceptions – part of web SDK | [github](https://github.com/Microsoft/ApplicationInsights-dotnet-server/releases)
| wad | Windows Azure Diagnostics reporting through AI | |
| wad2ai | Application Insight's Azure Diagnostics sink | [MicrosoftDocs](https://docs.microsoft.com/azure/monitoring-and-diagnostics/azure-diagnostics-configure-application-insights)
| wcf | WCF Application Insights lab project |  [github](https://github.com/Microsoft/ApplicationInsights-SDK-Labs/tree/master/WCF) [myget](https://www.myget.org/feed/applicationinsights-sdk-labs/package/nuget/Microsoft.ApplicationInsights.Wcf) [blog](https://azure.microsoft.com/en-us/blog/wcf-monitoring-with-application-insights/)
| web | telemetry that was collected by AI Web SDK, mostly is found on requests | [github](https://github.com/Microsoft/ApplicationInsights-dotnet-server/releases)
| webjobs | Azure Web Jobs hosting | [github](https://github.com/Azure/azure-webjobs-sdk/blob/5d3952d010c0981477e8b09f60b62312f85d4e1f/src/Microsoft.Azure.WebJobs.Logging.ApplicationInsights/DefaultTelemetryClientFactory.cs#L54)


## Prefixes
Define the prefixes for the SDK.

| SDK Name  | Prefix | Description                         |
|-----------|:------:|-------------------------------------|
| Redfield<sup>1</sup> |   ad_  | Telemetry from Redfield AppServices attach, using the **default** configuration |
| Redfield<sup>1</sup> |   ar_  | Telemetry from Redfield AppServices attach, using the **recommended** configuration |
| Redfield<sup>1</sup> |  csd_  | Telemetry from Redfield CloudServices attach, using the **default** configuration |
| Redfield<sup>1</sup> |  csr_  | Telemetry from Redfield CloudServices attach, using the **recommended** configuration |
| Redfield<sup>1</sup> |   ud_  | Telemetry from Redfield unknown environment attach, using the **default** configuration |
| Redfield<sup>1</sup> |   ur_  | Telemetry from Redfield unknown environment attach, using the **recommended** configuration |
| ap        |   w_   | Telemetry from **Windows** Platform |
| ap        |   l_   | Telemetry from **Linux** Platform   |
| python-oc |   lf_  | Telemetry captured by LocalForwarder |
| go-oc     |   lf_  | Telemetry captured by LocalForwarder |
| java      |   lf_  | Telemetry captured by LocalForwarder |


## Footnotes
1. Redfield attach applications are: Azure AppService Extension, Azure CloudService Extension, Azure VM Extension, and StatusMonitor.
