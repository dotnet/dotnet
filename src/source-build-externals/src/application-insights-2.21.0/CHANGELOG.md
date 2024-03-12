# Changelog

## VNext


## Version 2.21.0
- no changes since beta.

## Version 2.21.0-beta3
- [Remove two unnecessary .NET Standard 1.x dependencies.](https://github.com/microsoft/ApplicationInsights-dotnet/pull/2613)
- Address vulnerability in `Newtonsoft.Json` ([GHSA-5crp-9r3c-p9vr](https://github.com/advisories/GHSA-5crp-9r3c-p9vr)). 
  Mitigation is to upgrade dependencies in `Microsoft.ApplicationInsights.AspNetCore` ([#2615](https://github.com/microsoft/ApplicationInsights-dotnet/pull/2615))
  - Upgrade `Microsoft.Extensions.Configuration.Json` from v2.1.0 to v3.1.0. 
  - Upgrade `System.Text.Encodings.Web` from 4.5.1 to 4.7.2.

## Version 2.21.0-beta2
- [LOGGING: Make TelemetryConfiguration configurable in ApplicationInsightsLoggingBuilderExtensions](https://github.com/microsoft/ApplicationInsights-dotnet/issues/1944)
- [Added support for distributed tracing with Azure.Messaging.ServiceBus](https://github.com/microsoft/ApplicationInsights-dotnet/pull/2593)
- [Move internal type from `Shared` to `Microsoft` namespace](https://github.com/microsoft/ApplicationInsights-dotnet/pull/2442)
- [Extension methods to retrive specific operation details.](https://github.com/microsoft/ApplicationInsights-dotnet/issues/1350)
- [Mark Instrumentation Key based APIs as Obsolete](https://github.com/microsoft/ApplicationInsights-dotnet/issues/2560).
  - See also: https://docs.microsoft.com/azure/azure-monitor/app/migrate-from-instrumentation-keys-to-connection-strings
- [Fix: Livelock in MetricValuesBuffer.](https://github.com/microsoft/ApplicationInsights-dotnet/pull/2612)
  Mitigation for TelemetryClient.Flush deadlocks ([#1186](https://github.com/microsoft/ApplicationInsights-dotnet/issues/1186))

## Version 2.21.0-beta1
- [Support IPv6 in request headers](https://github.com/microsoft/ApplicationInsights-dotnet/issues/2521)
- [Validate exception stack trace line numbers to comply with endpoint restrictions.](https://github.com/microsoft/ApplicationInsights-dotnet/issues/2482)
- [Removed redundant memory allocations and processing in PartialSuccessTransmissionPolicy for ingestion sampling cases](https://github.com/microsoft/ApplicationInsights-dotnet/issues/2445)
- [Validate exception message length to comply with endpoint restrictions.](https://github.com/microsoft/ApplicationInsights-dotnet/issues/2284)
- Update JavaScript snippet to sv5 which includes the cdn endpoint.
- [Make operation detail name constants public.](https://github.com/microsoft/ApplicationInsights-dotnet/issues/1326)

## Version 2.20
- [Allow Control of Documents sampling for QuickPulse telemetry](https://github.com/microsoft/ApplicationInsights-dotnet/pull/2425)

## Version 2.20.0-beta1
- [Allow Control of Documents sampling for QuickPulse telemetry](https://github.com/microsoft/ApplicationInsights-dotnet/pull/2425)

## Version 2.19.0
- Support for .Net6

## Version 2.19.0-beta1
- [The `{OriginalFormat}` field in ILogger Scope will be emitted as `OriginalFormat` with the braces removed](https://github.com/microsoft/ApplicationInsights-dotnet/pull/2362)
- [Enable SDK to create package for auto-instrumentation](https://github.com/microsoft/ApplicationInsights-dotnet/pull/2365)
- [Suppress long running SignalR connections](https://github.com/microsoft/ApplicationInsights-dotnet/pull/2372)
- [Support for redirect response from Ingestion service](https://github.com/microsoft/ApplicationInsights-dotnet/issues/2327)
- [NetCore2.1 has reached end of life and all references have been removed](https://github.com/microsoft/ApplicationInsights-dotnet/issues/2251)

## Version 2.18.0
- [Change Self-Diagnostics to include datetimestamp in filename](https://github.com/microsoft/ApplicationInsights-dotnet/pull/2325)
- [AAD: Add logging to AuthenticationTransmissionPolicy](https://github.com/microsoft/ApplicationInsights-dotnet/pull/2319)
- [QuickPulse: Bump the number of custom dimensions included into full documents for QuickPulse from 3 to 10, make the selection consistent](https://github.com/microsoft/ApplicationInsights-dotnet/pull/2341)

## Version 2.18.0-beta3
- [Enable the self diagnostics and fix a NullReferenceException bug](https://github.com/microsoft/ApplicationInsights-dotnet/pull/2302)
- AAD Breaking change: [CredentialEnvelope.GetToken() now returns type AuthToken instead of string. This is to expose the Expiration value with the token.](https://github.com/microsoft/ApplicationInsights-dotnet/pull/2306)

## Version 2.18.0-beta2
- Reduce technical debt: Use pattern matching
- [Improve Self Diagnostics and support setting configuration in file](https://github.com/microsoft/ApplicationInsights-dotnet/issues/2238)
- [Fix AzureSdkDiagnosticListener from crashing user app.](https://github.com/microsoft/ApplicationInsights-dotnet/pull/2294)
- [Add support for Azure Active Directory ](https://github.com/microsoft/ApplicationInsights-dotnet/issues/2190)

## Version 2.18.0-beta1
- [Fix PropertyFetcher error when used with multiple types](https://github.com/microsoft/ApplicationInsights-dotnet/issues/2194)
- [New Task Based Flush API - FlushAsync](https://github.com/microsoft/ApplicationInsights-dotnet/issues/1743)
- [End support for net45 and net46 in AspNetCore](https://github.com/microsoft/ApplicationInsights-dotnet/pull/2252)

## Version 2.17.0
- [Fix: telemetry parent id when using W3C activity format in TelemetryDiagnosticSourceListener](https://github.com/microsoft/ApplicationInsights-dotnet/issues/2142)
- [Add ingestion response duration for transmission to data delivery status - TransmissionStatusEventArgs](https://github.com/microsoft/ApplicationInsights-dotnet/pull/2157)
- [Update default Profiler and Snapshot Debugger endpoints](https://github.com/microsoft/ApplicationInsights-dotnet/issues/2166)

## Version 2.17.0-beta1
- [Fix: Missing Dependencies when using Microsoft.Data.SqlClient v2.0.0](https://github.com/microsoft/ApplicationInsights-dotnet/issues/2032)
- [Add RoleName as a header with Ping Requests to QuickPulse.](https://github.com/microsoft/ApplicationInsights-dotnet/pull/2113)
- [QuickPulseTelemetryModule takes hints from the service regarding the endpoint to ping and how often to ping it](https://github.com/microsoft/ApplicationInsights-dotnet/pull/2120)
- [Upgrade Log4Net to version 2.0.10 to address CVE-2018-1285](https://github.com/microsoft/ApplicationInsights-dotnet/issues/2149)

## Version 2.16.0
- [QuickPulseTelemetryModule and MonitoringDataPoint have a new Cloud Role Name field for sending with ping and post requests to QuickPulse Service.](https://github.com/microsoft/ApplicationInsights-dotnet/pull/2100)
- [Upgrade to System.Diagnostics.DiagnosticSource v5.0.0](https://github.com/microsoft/ApplicationInsights-dotnet/issues/1960)

## Version 2.16.0-beta1
- [ILogger LogError and LogWarning variants write exception `ExceptionStackTrace` when `TrackExceptionsAsExceptionTelemetry` flag is set to `false`](https://github.com/microsoft/ApplicationInsights-dotnet/pull/2065)
- [Upgrade to System.Diagnostics.DiagnosticSource v5.0.0-rc.2.20475.5](https://github.com/microsoft/ApplicationInsights-dotnet/pull/2091)
- [The `{OriginalFormat}` field in ILogger will be emitted as `OriginalFormat` with the braces removed](https://github.com/microsoft/ApplicationInsights-dotnet/pull/2071)
- ApplicationInsightsLoggerProvider populates structured logging key/values irrespective of whether Scopes are enabled or not.

## Version 2.15.0
- EventCounterCollector module does not add AggregationInterval as a dimension to the metric.

## Version 2.15.0-beta3
- [Support Request.PathBase for AspNetCore telemetry](https://github.com/microsoft/ApplicationInsights-dotnet/pull/1983)
- [End support for .NET Framework 4.5 / 4.5.1, Add support for .NET Framework 4.5.2](https://github.com/microsoft/ApplicationInsights-dotnet/issues/1161)
- [Create single request telemetry when URL-rewrite rewrites a request](https://github.com/microsoft/ApplicationInsights-dotnet/issues/1744)
- [Remove legacy TelemetryConfiguration.Active from AspNetCore SDK](https://github.com/microsoft/ApplicationInsights-dotnet/issues/1953)
- [Refactor AspNetCore and WorkerService use of Heartbeat (DiagnosticTelemetryModule)](https://github.com/microsoft/ApplicationInsights-dotnet/issues/1954)
- [Fix broken correlation and missing in-proc dependency Azure Blob SDK v12](https://github.com/microsoft/ApplicationInsights-dotnet/issues/1915)
- [Fix Heartbeat interval not applied until after first heartbeat](https://github.com/microsoft/ApplicationInsights-dotnet/issues/1298)
- [Fix: ApplicationInsightsLoggerProvider does not catch exceptions](https://github.com/microsoft/ApplicationInsights-dotnet/issues/1969)
- [Update AppInsights JS snippet used in the code to latest version](https://github.com/microsoft/ApplicationInsights-JS)
- [ServerTelemetryChannel does not fall back to any default directory if user explicitly configures StorageFolder, and have trouble read/write to it](https://github.com/microsoft/ApplicationInsights-dotnet/pull/2002)
- [Fixed a bug which caused ApplicationInsights.config file being read for populating TelemetryConfiguration in .NET Core projects](https://github.com/microsoft/ApplicationInsights-dotnet/issues/1795)
- [Remove System.RunTime EventCounters by default](https://github.com/microsoft/ApplicationInsights-dotnet/issues/2009)
- [Ingestion service data delivery status](https://github.com/microsoft/ApplicationInsights-dotnet/pull/1887)
- [Update version of Microsoft.AspNetCore.Hosting to 2.1.0](https://github.com/microsoft/ApplicationInsights-dotnet/issues/1902)

## Version 2.15.0-beta2
- [Read all properties of ApplicationInsightsServiceOptions from IConfiguration](https://github.com/microsoft/ApplicationInsights-dotnet/issues/1882)
- [End support for NetStandard 1.x, Add support for NetStandard 2.0](https://github.com/microsoft/ApplicationInsights-dotnet/issues/1160)
- [Add support for SourceLink.Github to all SDKs.](https://github.com/microsoft/ApplicationInsights-dotnet/issues/1760)
- [ServerTelemetryChannel by default uses local disk storage in non Windows, to store telemetry during transient errors](https://github.com/microsoft/ApplicationInsights-dotnet/issues/1792)
- [Make TelemetryConfiguration property of TelemetryClient public accessible to be able to read the configuration](https://github.com/microsoft/ApplicationInsights-dotnet/issues/581)

## Version 2.15.0-beta1
- [WorkerService package is modified to depend on 2.1.1 on Microsoft.Extensions.DependencyInjection so that it can be used in .NET Core 2.1 projects without nuget errors.](https://github.com/microsoft/ApplicationInsights-dotnet/issues/1677)
- [Adding a flag to EventCounterCollector to enable/disable storing the EventSource name in the MetricNamespace and simplify the metric name](https://github.com/microsoft/ApplicationInsights-dotnet/issues/1341)
- [New: EventCounter to track Ingestion Endpoint Response Time](https://github.com/microsoft/ApplicationInsights-dotnet/pull/1796)

## Version 2.14.0
- no changes since beta.

## Version 2.14.0-beta5
- [Stop collecting EventCounters from Microsoft.AspNetCore.Hosting](https://github.com/microsoft/ApplicationInsights-dotnet/issues/1759)
- [Enable ILogger provider for apps targeting .NET Framework](https://github.com/microsoft/ApplicationInsights-dotnet/issues/1312)

## Version 2.14.0-beta4
- [Fix: SQL dependency names are bloated when running under the .NET Framework and using Microsoft.Data.SqlClient package](https://github.com/microsoft/ApplicationInsights-dotnet/pull/1723)
- [Fix: Disabling HeartBeats in Asp.Net Core projects causes Error traces every heart beat interval (15 minutes default)](https://github.com/microsoft/ApplicationInsights-dotnet/issues/1681)
- [Standard Metric extractor (Exception,Trace) populates all standard dimensions.](https://github.com/microsoft/ApplicationInsights-dotnet/issues/1738)
- [Add an explicit reference to System.Memory v4.5.4. This fixes a bug in System.Diagnostics.DiagnosticSource. We will remove this dependency when DiagnosticSource is re-released.](https://github.com/microsoft/ApplicationInsights-dotnet/issues/1707)

## Version 2.14.0-beta3
- [New: JavaScript Property to support Content Security Policy](https://github.com/microsoft/ApplicationInsights-dotnet/issues/1443)
- [Fix: All perf counters stop being collected when any of faulty counters are specified to collect on Azure WebApps](https://github.com/microsoft/ApplicationInsights-dotnet/issues/1686)
- [Fix: Some perf counters aren't collected when app is hosted on Azure WebApp](https://github.com/microsoft/ApplicationInsights-dotnet/issues/1685)
- [Fix: Update dependencies to fix incompatibility with NetCore 3.0 and 3.1](https://github.com/microsoft/ApplicationInsights-dotnet/issues/1699)
    - removing dependency on System.Data.SqlClient
    - System.Diagnostics.PerformanceCounter v4.5.0 -> v4.7.0
    - System.IO.FileSystem.AccessControl v4.5.0 -> 4.7.0

## Version 2.14.0-beta2
- [Fix: AspNetCore AddApplicationInsightsSettings() and MissingMethodException](https://github.com/microsoft/ApplicationInsights-dotnet/issues/1702)

## Version 2.14.0-beta1
- [Support new conventions for EventHubs from Azure.Messaging.EventHubs and processor.](https://github.com/microsoft/ApplicationInsights-dotnet/pull/1674)
- [Adding a flag to DependencyTrackingTelemetryModule to enable/disable collection of SQL Command text.](https://github.com/microsoft/ApplicationInsights-dotnet/pull/1514)
   Collecting SQL Command Text will now be opt-in, so this value will default to false. This is a change from the current behavior on .NET Core. To see how to collect SQL Command Text see here for details: https://docs.microsoft.com/azure/azure-monitor/app/asp-net-dependencies#advanced-sql-tracking-to-get-full-sql-query
- [change references to log4net to version 2.0.8](https://github.com/microsoft/ApplicationInsights-dotnet/issues/1675)
- [Fix: PerformanceCounter implementation is taking large memory allocation](https://github.com/microsoft/ApplicationInsights-dotnet/issues/1694)

## Version 2.13.1
- [Fix: AspNetCore AddApplicationInsightsSettings() and MissingMethodException](https://github.com/microsoft/ApplicationInsights-dotnet/issues/1702)

## Version 2.13.0
- no changes since beta.

## Version 2.13.0-beta2
- [Move FileDiagnosticTelemetryModule to Microsoft.ApplicationInsights assembly.](https://github.com/microsoft/ApplicationInsights-dotnet/issues/1059)
- [Do not track exceptions from HttpClient on .NET Core](https://github.com/microsoft/ApplicationInsights-dotnet/issues/1362)
- [Copy tags if we create new Activity in ASP.NET Core listener](https://github.com/microsoft/ApplicationInsights-dotnet/pull/1660)

## Version 2.13.0-beta1
- [All product sdks are now building the same symbols (DebugType = FULL) and we're including symbols in the nuget package.](https://github.com/microsoft/ApplicationInsights-dotnet/issues/1527)
- [Upgraded FxCop and fixed several issues related to null checks and disposing objects.](https://github.com/microsoft/ApplicationInsights-dotnet/pull/1499)
- [Fix Environment read permission Exception](https://github.com/microsoft/ApplicationInsights-dotnet/issues/657)
- [Exceptions are not correlated to requests when customErrors=Off and Request-Id is passed](https://github.com/microsoft/ApplicationInsights-dotnet/issues/1493)
- [Switch to compact Id format in W3C mode](https://github.com/microsoft/ApplicationInsights-dotnet/pull/1498)
- [Sanitizing Message in Exception](https://github.com/microsoft/ApplicationInsights-dotnet/issues/546)
- [Fix CreateRequestTelemetryPrivate throwing System.ArgumentOutOfRangeException](https://github.com/microsoft/ApplicationInsights-dotnet/issues/1513)
- [NLog supports TargetFramework NetStandard2.0 and reduces dependencies](https://github.com/microsoft/ApplicationInsights-dotnet/pull/1522)

## Version 2.12.2
- [Fix: AspNetCore AddApplicationInsightsSettings() and MissingMethodException](https://github.com/microsoft/ApplicationInsights-dotnet/issues/1702)

## Version 2.12.1
- [Fix Endpoint configuration bug affecting ServerTelemetryChannel and QuickPulseTelemetryModule](https://github.com/microsoft/ApplicationInsights-dotnet/issues/1648)

## Version 2.12.0
- [Fix IndexOutOfRangeException in W3CUtilities.TryGetTraceId](https://github.com/microsoft/ApplicationInsights-dotnet/pull/1327)
- [Fix UpdateRequestTelemetryFromRequest throwing UriFormatException](https://github.com/microsoft/ApplicationInsights-dotnet/issues/1328)
- [Add ETW log for missing Instrumentation Key](https://github.com/microsoft/ApplicationInsights-dotnet/pull/1331)
- [Remove HttpContext lock from AzureAppServiceRoleNameFromHostNameHeaderInitializer](https://github.com/microsoft/ApplicationInsights-dotnet/pull/1340)

## Version 2.12.0-beta4
- [Add support for collecting convention-based Azure SDK activities.](https://github.com/microsoft/ApplicationInsights-dotnet/pull/1300)
- [Log4Net includes Message for ExceptionTelemetry](https://github.com/microsoft/ApplicationInsights-dotnet/pull/1315)
- [NLog includes Message for ExceptionTelemetry](https://github.com/microsoft/ApplicationInsights-dotnet/pull/1315)
- [Fix RouteData not set in ASP.Net Core 3.0](https://github.com/microsoft/ApplicationInsights-dotnet/pull/1318)
- [Fix dependency tracking for Microsoft.Azure.EventHubs SDK 4.1.](https://github.com/microsoft/ApplicationInsights-dotnet/issues/1317)

## Version 2.12.0-beta3
- [Standard Metric extractor for Dependency) add Dependency.ResultCode dimension.](https://github.com/microsoft/ApplicationInsights-dotnet/issues/1233)

## Version 2.12.0-beta2
- [Enable Metric DimensionCapping API for Internal use with standard metric aggregation.](https://github.com/microsoft/ApplicationInsights-dotnet/issues/1244)
- [ILogger - Flush TelemetryChannel when the ILoggerProvider is Disposed.](https://github.com/microsoft/ApplicationInsights-dotnet/pull/1289)
- [Standard Metric extractor (Request,Dependency) populates all standard dimensions.](https://github.com/microsoft/ApplicationInsights-dotnet/issues/1233)

## Version 2.12.0-beta1
- BASE: [New: TelemetryConfiguration now supports Connection Strings]https://github.com/microsoft/ApplicationInsights-dotnet/issues/1221)
- WEB: [Enhancement to how QuickPulseTelemetryModule shares its ServiceEndpoint with QuickPulseTelemetryProcessor.](https://github.com/microsoft/ApplicationInsights-dotnet-server/pull/1266)
- WEB: [QuickPulse will support SDK Connection String](https://github.com/microsoft/ApplicationInsights-dotnet/issues/1221)
- WEB: [Add support for storing EventCounter Metadata as properties of MetricTelemetry](https://github.com/microsoft/ApplicationInsights-dotnet-server/issues/1287)
- WEB: [New RoleName initializer for Azure Web App to accurately populate RoleName.](https://github.com/microsoft/ApplicationInsights-dotnet-server/issues/1207)
- NETCORE: Skipping version numbers to keep in sync with Base SDK.
- NETCORE: [Fix Null/Empty Ikey from ApplicationInsightsServiceOptions overrding one from appsettings.json](https://github.com/microsoft/ApplicationInsights-aspnetcore/issues/989)
- NETCORE: [Provide ApplicationInsightsServiceOptions for easy disabling of any default TelemetryModules](https://github.com/microsoft/ApplicationInsights-aspnetcore/issues/988)
- NETCORE: [Added support for SDK Connection String](https://github.com/microsoft/ApplicationInsights-dotnet/issues/1221)
- NETCORE: [New RoleName initializer for Azure Web App to accurately populate RoleName.](https://github.com/microsoft/ApplicationInsights-dotnet-server/issues/1207)
- NETCORE: Update to Base/Web/Logging SDK to 2.12.0-beta1

----------
# Before 2.12.0, our SDKs had separate repositories. 
----------

## Version 2.11.0
- BASE: Upgrade to System.Diagnostics.DiagnosticSource v4.6
- BASE: [Fix: StartOperation(Activity) does not check for Ids compatibility](https://github.com/microsoft/ApplicationInsights-dotnet/pull/1213)

## Version 2.11.0-beta2
- BASE: [Fix: Emit warning if user sets both Sampling IncludedTypes and ExcludedTypes. Excluded will take precedence.](https://github.com/microsoft/ApplicationInsights-dotnet/issues/1166)
- BASE: [Minor perf improvement by reading Actity.Tag only if required.](https://github.com/microsoft/ApplicationInsights-dotnet/pull/1170)
- BASE: [Fix: Channels not handling AggregateException, not logging full HttpRequestException.](https://github.com/microsoft/ApplicationInsights-dotnet/issues/1173)
- BASE: [Metric Aggregator background thread safeguards added to never throw unhandled exception.](https://github.com/microsoft/ApplicationInsights-dotnet/issues/1179)
- BASE: [Updated version of System.Diagnostics.DiagnosticSource to 4.6.0-preview7.19362.9. Also remove marking SDK as CLS-Compliant](https://github.com/microsoft/ApplicationInsights-dotnet/pull/1183)
- BASE: [Enhancement: Exceptions thrown by the TelemetryConfiguration will now specify the exact name of the property that could not be parsed from a config file.](https://github.com/microsoft/ApplicationInsights-dotnet/issues/1194)
- BASE: [Fix: ServerTelemetryChannel constructor exception when network info API throws.](https://github.com/microsoft/ApplicationInsights-dotnet/issues/1184)
- BASE: [Make BaseSDK use W3C Trace Context based correlation by default. Set TelemetryConfiguration.EnableW3CCorrelation=false to disable this.](https://github.com/microsoft/ApplicationInsights-dotnet/pull/1193)
- BASE: [Removed TelemetryConfiguration.EnableW3CCorrelation. Users should do Activity.DefaultIdFormat = ActivityIdFormat.Hierarchical; Activity.ForceDefaultIdFormat = true; to disable W3C Format](https://github.com/microsoft/ApplicationInsights-dotnet/issues/1198)
- BASE: [Enable sampling based on upstream sampling decision for adaptive sampling](https://github.com/microsoft/ApplicationInsights-dotnet/pull/1200)
- BASE: [Fix: StartOperation ignores user-provided custom Ids in scope of Activity](https://github.com/microsoft/ApplicationInsights-dotnet/pull/1205)
- BASE: [Set tracestate if available on requests and dependencies](https://github.com/microsoft/ApplicationInsights-dotnet/pull/1207)

## Version 2.11.0-beta1
- BASE: [Performance fixes: Support Head Sampling; Remove NewGuid(); Sampling Flags; etc... ](https://github.com/microsoft/ApplicationInsights-dotnet/pull/1158)
- BASE: [Deprecate TelemetryConfiguration.Active on .NET Core in favor of dependency injection pattern](https://github.com/microsoft/ApplicationInsights-dotnet/pull/1152)
- BASE: [Make TrackMetric() visible. This method is not recommended unless you are sending pre-aggregated metrics.](https://github.com/microsoft/ApplicationInsights-dotnet/pull/1149)

## Version 2.10.0
- BASE: [SDKVersion modified to be dotnetc for NetCore. This helps identify the source of code path, as implementations are slightly different for NetCore.](https://github.com/microsoft/ApplicationInsights-dotnet/pull/1125)
- BASE: [Fix telemetry timestamp precision on .NET Framework](https://github.com/microsoft/ApplicationInsights-dotnet-server/issues/1175)
- BASE: [Fix: catch SecurityException when ApplicationInsights config is read in partial trust](https://github.com/microsoft/ApplicationInsights-dotnet/pull/1119)

## Version 2.10.0-beta4
- BASE: [Fix NullReferenceException in DiagnosticsEventListener.OnEventWritten](https://github.com/Microsoft/ApplicationInsights-dotnet/issues/1106)
- BASE: [Fix RichPayloadEventSource can get enabled at Verbose level](https://github.com/Microsoft/ApplicationInsights-dotnet/issues/1108)
- BASE: [Fix DiagnosticsTelemetryModule can get added more than once](https://github.com/Microsoft/ApplicationInsights-dotnet/issues/1111)
- BASE: [Modify default settings for ServerTelemetryChannel to reduce disk usage. MaxTransmissionSenderCapacity is changed from 3 to 10, MaxTransmissionBufferCapacity is changed from 1MB to 5MB](https://github.com/Microsoft/ApplicationInsights-dotnet/pull/1113)

## Version 2.10.0-beta3
- BASE: No changes. Bumping version to match WebSDK release.

## Version 2.10.0-beta2
- BASE: [Fix Transmission in NETCORE to handle partial success (206) response from backend.](https://github.com/Microsoft/ApplicationInsights-dotnet/issues/1047)
- BASE: [Fix data losses in ServerTelemetryChannel.](https://github.com/Microsoft/ApplicationInsights-dotnet/issues/1049)
- BASE: [InitialSamplingRate is now correctly applied if set in applicationInsights.config] (https://github.com/Microsoft/ApplicationInsights-dotnet/pull/1048)
- BASE: Added new target for NetStandard2.0.

## Version 2.9.1
- BASE: [Aggregation thread is now a background thread](https://github.com/Microsoft/ApplicationInsights-dotnet/issues/1080)

## Version 2.9.0
- BASE: [Move W3C methods from Web SDK](https://github.com/Microsoft/ApplicationInsights-dotnet/pull/1064)

## Version 2.9.0-beta3
- BASE: [Flatten IExtension and Unknown ITelemetry implementations for Rich Payload Event Source consumption](https://github.com/Microsoft/ApplicationInsights-dotnet/pull/1017)
- BASE: [Fix: Start/StopOperation with W3C distributed tracing enabled does not track telemetry](https://github.com/Microsoft/ApplicationInsights-dotnet/pull/1031)
- BASE: [Fix: Do not run metric aggregation on a thread pool thread](https://github.com/Microsoft/ApplicationInsights-dotnet/pull/1028)

## Version 2.9.0-beta2
- BASE: [Remove unused reference to System.Web.Extensions](https://github.com/Microsoft/ApplicationInsights-dotnet/pull/956)
- BASE: [PageViewTelemetry](https://github.com/Microsoft/ApplicationInsights-dotnet/blob/8673ed1d15005713755e0bb9594acfe0ee00b869/src/Microsoft.ApplicationInsights/DataContracts/PageViewTelemetry.cs) now supports [ISupportMetrics](https://github.com/Microsoft/ApplicationInsights-dotnet/blob/39a5ef23d834777eefdd72149de705a016eb06b0/src/Microsoft.ApplicationInsights/DataContracts/ISupportMetrics.cs)
- BASE: [Fixed a bug in TelemetryContext which prevented rawobject store to be not available in all sinks.](https://github.com/Microsoft/ApplicationInsights-dotnet/issues/974)
- BASE: [Fixed a bug where TelemetryContext would have missing values on secondary sinks](https://github.com/Microsoft/ApplicationInsights-dotnet/pull/993)
- BASE: [Fixed race condition in BroadcastProcessor which caused it to drop TelemetryItems](https://github.com/Microsoft/ApplicationInsights-dotnet/pull/995)
- BASE: [Custom Telemetry Item that implements ITelemetry is no longer dropped, bur rather serialized as EventTelemetry and handled by the channels accordingly](https://github.com/Microsoft/ApplicationInsights-dotnet/issues/988)
- BASE: [IExtension is now serialized into the Properties and Metrics](https://github.com/Microsoft/ApplicationInsights-dotnet/issues/1000)

Perf Improvements.
- BASE: [Improved Perf of ITelemetry JsonSerialization](https://github.com/Microsoft/ApplicationInsights-dotnet/issues/997)
- BASE: [Added new method on TelemetryClient to initialize just instrumentation. This is to be used by autocollectors to avoid calling TelemetryInitializers twice.](https://github.com/Microsoft/ApplicationInsights-dotnet/issues/966)
- BASE: [RequestTelemetry modified to lazily instantiate ConcurrentDictionary for Properties](https://github.com/Microsoft/ApplicationInsights-dotnet/issues/969)
- BASE: [RequestTelemetry modified to not service public fields with data class to avoid converting between types.](https://github.com/Microsoft/ApplicationInsights-dotnet/issues/965)
- BASE: [Dependency Telemetry modified to lazily instantiate ConcurrentDictionary for Properties](https://github.com/Microsoft/ApplicationInsights-dotnet/issues/1002)
- BASE: [Avoid string allocations in Metrics hot path](https://github.com/Microsoft/ApplicationInsights-dotnet/issues/1004)

## Version 2.8.1
[Patch release addressing perf regression.](https://github.com/Microsoft/ApplicationInsights-dotnet/issues/952)

## Version 2.8.0
- BASE: [New API to store/retrieve any raw objects on TelemetryContext to enable AutoCollectors to pass additional information for use by TelemetryInitializers.](https://github.com/Microsoft/ApplicationInsights-dotnet/issues/937)
- BASE: Perf Improvements
	https://github.com/Microsoft/ApplicationInsights-dotnet/issues/927
	https://github.com/Microsoft/ApplicationInsights-dotnet/issues/930
	https://github.com/Microsoft/ApplicationInsights-dotnet/issues/934
- BASE: Fix: [Response code shouldn't be overwritten to 200 if not set](https://github.com/Microsoft/ApplicationInsights-dotnet/pull/918)

## Version 2.8.0-beta2
- BASE: [TelemetryProcessors (sampling, autocollectedmetricaggregator), TelemetryChannel (ServerTelemetryChannel) added automatically to the default ApplicationInsights.config are moved under the default telemetry sink.](https://github.com/Microsoft/ApplicationInsights-dotnet/issues/907)
	If you are upgrading, and have added/modified TelemetryProcessors/TelemetryChannel, make sure to copy them to the default sink section.

## Version 2.8.0-beta1
- BASE: [Add a new distinct properties collection, GlobalProperties, on TelemetryContext, and obsolete the Properties on TelemetryContext.](https://github.com/Microsoft/ApplicationInsights-dotnet/issues/820)
- BASE: [Added support for strongly typed extensibility for Telemetry types using IExtension.](https://github.com/Microsoft/ApplicationInsights-dotnet/issues/871)
- BASE: [New method SerializeData(ISerializationWriter writer) defined in ITelemetry. All existing types implement this method to emit information about it's fields to channels who can serialize this data]
   (continuation of https://github.com/Microsoft/ApplicationInsights-dotnet/issues/871)
- BASE: [Allow to track PageViewPerformance data type](https://github.com/Microsoft/ApplicationInsights-dotnet/issues/673).
- BASE: Added method `ExceptionDetailsInfoList` on `ExceptionTelemetry` class that gives control to user to update exception
message and exception type of underlying `System.Exception` object that user wants to send to telemetry. Related discussion is [here](https://github.com/Microsoft/ApplicationInsights-dotnet/issues/498).
- BASE: Added an option of creating ExceptionTelemetry object off of custom exception information rather than a System.Exception object.
- BASE: [Add support for hex values in config](https://github.com/Microsoft/ApplicationInsights-dotnet/issues/551)

## Version 2.7.2
- BASE: Metrics: Renamed TryTrackValue(..) into TrackValue(..).
- BASE: Metrics: Removed some superfluous public constants.

## Version 2.7.0-beta3
- BASE: [Allow to set flags on event](https://github.com/Microsoft/ApplicationInsights-dotnet/issues/844). It will be used in conjunction with the feature that will allow to keep IP addresses.
- BASE: [Fix: SerializationException resolving Activity in cross app-domain calls](https://github.com/Microsoft/ApplicationInsights-dotnet-server/issues/613)
- BASE: [Make HttpClient instance static to avoid re-creating with every transmission. This had caused connection/memory leaks in .net core 2.1](https://github.com/Microsoft/ApplicationInsights-dotnet/issues/594)
  Related: (https://github.com/Microsoft/ApplicationInsights-aspnetcore/issues/690)

## Version 2.7.0-beta2
- BASE: [Fix: NullReferenceException if telemetry is tracked after TelemetryConfiguration is disposed](https://github.com/Microsoft/ApplicationInsights-dotnet-server/issues/928)
- BASE: [Move the implementation of the extraction of auto-collected (aka standard) metrics from internal legacy APIs to the recently shipped metric aggregation APIs.](https://github.com/Microsoft/ApplicationInsights-dotnet/issues/806)
- BASE: [Fix: NullReferenceException in ExceptionConverter.GetStackFrame if StackFrame.GetMethod() is null](https://github.com/Microsoft/ApplicationInsights-dotnet/issues/819)

## Version 2.7.0-beta1 
- BASE: [Extend the Beta period for Metrics Pre-Aggregation features shipped in 2.6.0-beta3.](https://github.com/Microsoft/ApplicationInsights-dotnet/issues/785)
- BASE: [New: Added TryGetOperationDetail to DependencyTelemetry to facilitate advanced ITelemetryInitializer scenarios.  Allows ITelemetryInitializer implementations to specify fields that would otherwise not be sent automatically to the backend.](https://github.com/Microsoft/ApplicationInsights-dotnet-server/issues/900)

## version 2.6.4
- BASE: [Revert changed namespace: `SamplingPercentageEstimatorSettings`, `AdaptiveSamplingPercentageEvaluatedCallback`](https://github.com/Microsoft/ApplicationInsights-dotnet/issues/727)
- BASE: [Add netstandard2.0 target for TelemetryChannel which doesn't have a dependency on Newtonsoft.Json ](https://github.com/Microsoft/ApplicationInsights-dotnet/issues/800)

## Version 2.6.1 
- BASE: [Extend the Beta period for Metrics Pre-Aggregation features shipped in 2.6.0-beta3.](https://github.com/Microsoft/ApplicationInsights-dotnet/issues/785)
- BASE: [Fix: changed namespace SamplingPercentageEstimatorSettings](https://github.com/Microsoft/ApplicationInsights-dotnet/issues/727)

## Version 2.6.0-beta4
- BASE: [New: Enable ExceptionTelemetry.SetParsedStack for .Net Standard](https://github.com/Microsoft/ApplicationInsights-dotnet/issues/763)
- BASE: [Fix: TelemetryClient throws NullReferenceException on Flush if the underlying configuration was disposed](https://github.com/Microsoft/ApplicationInsights-dotnet/issues/755)

## Version 2.6.0-beta3
- BASE: [Report internal errors from Microsoft.AspNet.TelemteryCorrelation module](https://github.com/Microsoft/ApplicationInsights-dotnet/pull/744)
- BASE: [Fix: Telemetry tracked with StartOperation is tracked outside of corresponding activity's scope](https://github.com/Microsoft/ApplicationInsights-dotnet-server/issues/864)
- BASE: [Fix: TelemetryProcessor chain building should also initialize Modules.](https://github.com/Microsoft/ApplicationInsights-dotnet/issues/549)
- BASE: [Fix: Wrong error message in AutocollectedMetricsExtractor.](https://github.com/Microsoft/ApplicationInsights-dotnet/issues/611)
- BASE: [NEW: Interface and Configuration: IApplicationIdProvider.](https://github.com/Microsoft/ApplicationInsights-dotnet/issues/739)
- BASE: [NEW: Metrics Pre-Aggregation: New `TelemetryClient.GetMetric(..).TrackValue(..)` and related APIs always locally pre-aggregate metrics before sending. They are replacing the legacy `TelemetryClient.TrackMetric(..)` APIs.](https://github.com/Microsoft/ApplicationInsights-dotnet/pull/735) ([More info](https://github.com/Microsoft/ApplicationInsights-dotnet/issues/643).)

## Version 2.6.0-beta2
- BASE: [Changed signature of TelemetryClient.TrackDependency](https://github.com/Microsoft/ApplicationInsights-dotnet/issues/684)
- BASE: [Added overload of TelemetryClientExtensions.StartOperation(Activity activity).](https://github.com/Microsoft/ApplicationInsights-dotnet/issues/644)
- BASE: [Finalize the architecture for adding default heartbeat properties (supporting proposal from Issue #636).](https://github.com/Microsoft/ApplicationInsights-dotnet/issues/666).

## Version 2.5.1
- BASE: Fix for missing TelemetryContext. Thank you to our community for discovering and reporting this issue! 
  [Logic bug within Initialize() in TelemetryContext](https://github.com/Microsoft/ApplicationInsights-dotnet/issues/692),
  [Dependency correlation is broken after upgrade to .NET SDK 2.5](https://github.com/Microsoft/ApplicationInsights-dotnet/issues/706),
  [Lost many context fields in 2.5.0](https://github.com/Microsoft/ApplicationInsights-dotnet/issues/708)

## Version 2.5.0-beta2
- BASE: Remove calculation of sampling-score based on Context.User.Id [Issue #625](https://github.com/Microsoft/ApplicationInsights-dotnet/issues/625)
- BASE: New sdk-driven "heartbeat" functionality added which sends health status at pre-configured intervals. See [extending heartbeat properties doc for more information](./docs/ExtendingHeartbeatProperties.md)
- BASE: Fixes a bug in ServerTelemetryChannel which caused application to crash on non-windows platforms. 
			[Details on fix and workaround #654](https://github.com/Microsoft/ApplicationInsights-dotnet/issues/654)
			Original issue (https://github.com/Microsoft/ApplicationInsights-aspnetcore/issues/551)
- BASE: [Fixed a bug with the `AdaptiveSamplingTelemetryProcessor` that would cause starvation over time. Issue #756 (dotnet-server)](https://github.com/Microsoft/ApplicationInsights-dotnet-server/issues/756)
- BASE: Updated solution to build on Mac!

## Version 2.5.0-beta1
- BASE: Method `Sanitize` on classes implementing `ITelemetry` no longer modifies the `TelemetryContext` fields. Serialized event json and ETW event will still have context tags sanitized.
- BASE: Application Insights SDK supports multiple telemetry sinks. You can configure more than one channel for telemetry now.
- BASE: New method `DeepClone` on `ITelemetry` interface. Implemented by all supported telemetry items.
- BASE: Server telemetry channel NuGet support a netstandard1.3 target with fixed rate sampling and adaptive sampling telemetry processors.
- BASE: Instrumentation key is no longer required for TelemetryClient to send data to channel(s). This makes it easier to use the SDK with channels other than native Application Insights channels.
- BASE: .NET 4.0 targets were removed. Please use the version 2.4.0 if you cannot upgrade your application to the latest framework version.
- BASE: Removed `wp8`, `portable-win81+wpa81` and `uap10.0` targets.

## Version 2.4.0
- BASE: Updated version of DiagnosticSource to 4.4.0 stable

## Version 2.4.0-beta5
- BASE: Updated version of DiagnosticSource referenced.

## Version 2.4.0-beta4
- BASE: Made Metric class private and fixed various metrics related issues.

## Version 2.4.0-beta3

## Version 2.4.0-beta2
- BASE: Removed metric aggregation functionality as there is not enough feedback on the API surface yet.

## Version 2.4.0-beta1
- BASE: Event telemetry is set to be sampled separately from all other telemetry types. It potentially can double the bill. The reason for this change is that Events are mostly used for usage analysis and should not be subject to sampling on high load of requests and dependencies. Edit `ApplicationInsights.config` file to revert to the previous behavior.
- BASE: Added dependency on System.Diagnostics.DiagnosticsSource package. It is still possible to use standalone Microsoft.ApplicationInsights.dll to track telemetry.
- BASE: StartOperation starts a new System.Diagnostics.Activity and stores operation context in it. StartOperation overwrites OperationTelemetry.Id set before or during telemetry initialization for the dependency correlation purposes.
- BASE: OperationCorrelationTelemetryInitializer initializes telemetry from the Activity.Current. Please refer to https://github.com/dotnet/corefx/blob/master/src/System.Diagnostics.DiagnosticSource/src/ActivityUserGuide.md for more details about Activity and how to use it
- BASE: `Request.Success` field will not be populated based on `ResponseCode`. It needs to be set explicitly.
- BASE: New "ProblemId" property on ExceptionTelemetry. It can be used to set a custom ProblemId value.
- BASE: Metric Aggregation functionality (originally added in 2.3.0-beta1 but removed in 2.3.0) is re-introduced.
- BASE: Improved exception stack trace data collection for .NET Core applications.

## Version 2.3.0
- BASE: Includes all changes since 2.2.0 stable release.
- BASE: Removed metric aggregation functionality added in 2.3.0-beta1 release.
- BASE: [Fixed a bug which caused SDK to stop sending telemetry.](https://github.com/Microsoft/ApplicationInsights-dotnet-server/issues/480)

## Version 2.3.0-beta3
- BASE: [Added overloads of TelemetryClientExtensions.StartOperation.](https://github.com/Microsoft/ApplicationInsights-dotnet/issues/163)
- BASE: Fire new ETW events for Operation Start/Stop.

## Version 2.3.0-beta2
- BASE: Added constructor overloads for TelemetryConfiguration and added creation of a default InMemoryChannel when no channel is specified for a new instance.
  TelemetryClient will no longer create an InMemoryChannel on the configuration instance if TelemetryChannel is null, instead the configuration instances will always have a channel when created.
- BASE: TelemetryConfiguration will no longer dispose of user provided ITelemetryChannel instances.  Users must properly dispose of any channel instances which they create, the configuration will only auto-dispose of default channel instances it creates when none are specified by the user.

## Version 2.3.0-beta1
- BASE: Added metric aggregation functionality via MetricManager and Metric classes.
- BASE: Exposed a source field on RequestTelemetry. This can be used to store a representation of the component that issued the incoming http request. 

## Version 2.2.0
- BASE: Includes all changes since 2.1.0 stable release.

## Version 2.2.0-beta6
- BASE: Added serialization of the "source" property.
- BASE: Downgraded package dependencies to Microsoft.NETCore.App 1.0.1 level.
- BASE: [Fixed the priority of getting an iKey from an environment variable](https://github.com/Microsoft/ApplicationInsights-dotnet-server/issues/236)

## Version 2.2.0-beta5
- BASE: Moved from PCL dotnet5.4 to .NET Core NetStandard1.5.
- BASE: Updated dependency versions.

## Version 2.2.0-beta4
- BASE: Payload sanitization for RichPayloadEventSource.
- BASE: Fix to fallback to an environment variable for instrumentation key when not specified when initializing TelemetryConfiguration.
- BASE: RoleInstance and NodeName are initialized with the machine name by default.

## Version 2.2.0-beta3

- BASE: Read InstrumentationKey from environment variable APPINSIGHTS_INSTRUMENTATIONKEY if it is was not provided inline. If provided it overrides what is set though configuration file. (Feature is not available in PCL version of SDK).
- BASE: Context properties `NetworkType`, `ScreenResolution` and `Language` marked as obsolete. Please use custom properties to report network type, screen resolution and language. Values stored in these properties will be send as custom properties. 
- BASE: Dependency type was updated to reflect the latest developments in Application Insights Application Map feature. You can set a new field - `Target`. `CommandName` was renamed to `Data` for consistency with the Application Analytics schema. `DependencyKind` will never be send any more and will not be set to "Other" by default. Also there are two more constructors for `DependencyTelemetry` item.
- BASE: Type `SessionStateTelemetry` was marked obsolete. Use `IsFirst` flag in `SessionContext` to indicate that the session is just started.
- BASE: Type `PerformanceCounterTelemetry` was marked obsolete. Use `MetricTelemetry` instead.
- BASE: Marked `RequestTelemetry.HttpMethod` as obsolete. Put http verb as part of the name for the better grouping by name and use custom properties to report http verb as a dimension.
- BASE: Marked `RequestTelemetry.StartTime` as obsolete. Use `TimeStamp` instead.
- BASE: [Removed BCL dependency](https://github.com/Microsoft/ApplicationInsights-dotnet/issues/175)
- BASE: [Added IPv6 support](https://github.com/Microsoft/ApplicationInsights-dotnet/issues/316)
- BASE: [Fixed an issue where channels sent expired data from storage](https://github.com/Microsoft/ApplicationInsights-dotnet/issues/278)
- BASE: [Fixed an issue where the clock implementation would accumulate error](https://github.com/Microsoft/ApplicationInsights-dotnet/issues/271)
- BASE: [Fixed an issue where telemetry with emptry properties would be dropped](https://github.com/Microsoft/ApplicationInsights-dotnet/issues/319)
- BASE: [Added support for SDK-side throttling](https://github.com/Microsoft/ApplicationInsights-dotnet/issues/318)

## Version 2.2.0-beta2

- BASE: InMemoryChannel has a new override for Flush method that accepts timeout.
- BASE: Local storage folder name was changed. That means that when the application stopped, and the application was updated to the new SDK, then the telemetry from the old local folder will not be send.
- BASE: Allow all characters in property names and measurements names.
- BASE: AdaptiveTelemetryProcessor has a new property IncludedTypes. It gets or sets a semicolon separated list of telemetry types that should be sampled. If left empty all types are included implicitly. Types are not included if they are set in ExcludedTypes.
- BASE: Richpayload event source event is generated for all framework versions of SDK (before it was supported in 4.6 only)
- BASE: TelemetryClient has a new method TrackAvailability. Data posted using this method would be available in AppAnalitics only, Azure portal UI is not available at this moment.

## Version 2.2.0-beta1

- BASE: Add ExceptionTelemetry.Message property. If it is provided it is used instead of Exception.Message property for the outer-most exception.
- BASE: Telemetry types can be excluded from sampling by specifing ExcludedTypes property. 
- BASE: ServerTelemetryChannel: changed backoff logic to be less aggressive, added diagnostics event when backoff logic kicks in and added more tracing. (Done to address issues when data stops flowing till application gets restarted)

## Version 2.1.0-beta4
- BASE: [Bug fix](https://github.com/Microsoft/ApplicationInsights-dotnet-server/issues/76)

## Version 2.1.0-beta3
- BASE: Support partial success (206) from the Application Insights backend. Before this change SDK may have lost data because some items of the batch were accepted and some items of the batch were asked to be retried (because of burst throttling or intermittent issues).
- BASE: Bug fixes

## Version 2.1.0-beta1

- BASE: Upgraded to depend on EventSource.Redist nuget version 1.1.28
- BASE: Upgraded to depend on Microsoft.Bcl nuget version 1.1.10

## Version 2.0.1

- BASE: Add Win Phone, Win Store and UWP targets that include 1.2.3 version of ApplicationInsights.dll. It is included to prevent applications that upgrade to 2.0.0 from crashing. In any case using this nuget for Win Phone, Win Store and UWP targets is not recommended and not supported. 

## Version 2.0.0

- BASE: Disallow Nan, +-Infinity measurements. Value will be replaced on 0.
- BASE: Disallow Nan, +-Infinity metrics (Value, Min, Max and StandardDeviation). Values will be replaced on 0.

## Version 2.0.0-rc1

- BASE: Writing telemetry items to debug output can be disabled with ```IsTracingDisabled``` property on ```TelemetryDebugWriter```. 
Telemetry items that were filtered out by sampling are now indicated in debug output. Custom telemetry processors can now invoke
method ```WriteTelemetry``` on ```TelemetryDebugWriter``` with ```filteredBy``` parameter to indicate in debug output that an
item is being filtered out.
- BASE: DependencyTelemetry.Async property was removed.
- BASE: DependencyTelemetry.Count property was removed.
- BASE: When configuration is loaded from ApplicationInsights.config incorrect and broken elements are skipped. That includes both high level elements like TelemetryInitializers as well as individual properties.  
- BASE: Internal Application Insights SDK traces will be marked as synthetic and have `SyntheticSource` equals to 'SDKTelemetry'.
- BASE: UserContext.AcquisitionDate property was removed.
- BASE: UserContext.StoreRegion property was removed.
- BASE: InMemoryChannel.DataUploadIntervalInSeconds was removed. Use SendingInterval instead.
- BASE: DeviceContext.RoleName was removed. Use DeviceContext.Cloud.RoleName instead.
- BASE: DeviceContext.RoleInstance was removed. Use DeviceContext.Cloud.RoleInstance instead.

## Version 2.0.0-beta4

- BASE: UseSampling and UseAdaptiveSampling extensions were moved to Microsoft.ApplicationInsights.Extensibility
- BASE: Cut Phone and Store support
- BASE: Updated ```DependencyTelemetry``` to have new properties ```ResultCode``` and ```Id```
- BASE: If ``ServerTelemetryChannel`` is initialized programmatically it is required to call ServerTelemetryChannel.Initialize() method. Otherwise persistent storage will not be initialized (that means that if telemetry cannot be sent because of temporary connectivity issues it will be dropped).
- BASE: ``ServerTelemetryChannel`` has new property ``StorageFolder`` that can be set either through code or though configuration. If this property is set ApplicationInsights uses provided location to store telemetry that was not sent because of temporary connectivity issues. If property is not set or provided folder is inaccessible ApplicationInsights will try to use LocalAppData or Temp as it was done before.
- BASE: TelemetryConfiguration.GetTelemetryProcessorChainBuilder extension method is removed. Instead of this method use TelemetryConfiguration.TelemetryProcessorChainBuilder instance method.
- BASE: TelemetryConfiguration has a new property TelemetryProcessors that gives readonly access to TelemetryProcessors collection.
- BASE: `Use`, `UseSampling` and `UseAdaptiveSampling` preserves TelemetryProcessors loaded from configuration.

## Version 2.0.0-beta3
- BASE: Adaptive sampling turned on by default in server telemetry channel. Details can be found in [#80](https://github.com/Microsoft/ApplicationInsights-dotnet/issues/80).
- BASE: Fixed signature of ```UseSampling``` to allow chaining with other calls to ```Use``` of telemetry processors.
- BASE: Property ```Request.ID``` returned back. ```OperationContext``` now has a property ```ParentId``` for end-to-end correlation.
- BASE: ```TimestampTelemetryInitializer``` is removed. Timestamp will be added automatically by ```TelemetryClient```.
- BASE: ```OperationCorrelationTelemetryInitializer``` is added by default to enable operaitons correlation.

## Version 2.0.0-beta2
- BASE: Fix UI thread locking when initializing InMemoryChannel (default channel) from UI thread.
- BASE: Added support for ```ITelemetryProcessor``` and ability to construct chain of TelemetryProcessors via code or config.
- BASE: Version of ```Microsoft.ApplicationInsights.dll``` for the framework 4.6 is now part of the package.
- BASE: IContextInitializer interface is not supported any longer. ContextInitializers collection was removed from TelemetryConfiguration object.
- BASE: The max length limit for the ```Name``` property of ```EventTelemetry``` was set to 512.
- BASE: Property ```Name``` of ```OperationContext``` was renamed to ```RootName```
- BASE: Property ```Id``` of ```RequestTelemetry``` was removed.
- BASE: Property ```Id``` and ```Context.Operation.Id``` of ```RequestTelemetry``` would not be initialized when creating new ```RequestTelemetry```.
- BASE: New properties of ```OperationContext```: ```CorrelationVector```, ```ParentId``` and ```RootId``` to support end-to-end telemetry items correlation.

## Version 2.0.0-beta1

- BASE: TrackDependency will produce valid JSON when not all required fields were specified.
- BASE: Redundant property ```RequestTelemetry.ID``` is now just a proxy for ```RequestTelemetry.Operation.Id```.
- BASE: New interface ```ISupportSampling``` and explicit implementation of it by most of data item types.
- BASE: ```Count``` property on DependencyTelemetry marked as Obsolete. Use ```SamplingPercentage``` instead.
- BASE: New ```CloudContext``` introduced and properties ```RoleName``` and ```RoleInstance``` moved to it from ```DeviceContext```.
- BASE: New property ```AuthenticatedUserId``` on ```UserContext``` to specify authenticated user identity.

## Version 1.2.3
- BASE: Bug fixes.
- BASE: Telemetry item will be serialized to Debug Output even when Instrumentation Key was not set.

## Version 1.2
- BASE: First version shipped from github

## Version 1.1

- BASE: SDK now introduces new telemetry type ```DependencyTelemetry``` which contains information about dependency call from application
- BASE: New method ```TelemetryClient.TrackDependency``` allows to send information about dependency calls from application

## Version 0.17

- BASE: Application Insights now distributes separate binaries for framework 4.0 and 4.5. Library for the framework 4.5 will not require EventSource and BCL nuget dependencies. You need to ensure you refer the correct library in ```packages.config```. It should be ```<package id="Microsoft.ApplicationInsights" version="0.17.*" targetFramework="net45" />```
- BASE: Diagnostics telemetry module is not registered in ApplicationInsights.config and no self-diagnostics messages will be sent to portal for non-web applications. Insert ```<Add Type="Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing.DiagnosticsTelemetryModule, Microsoft.ApplicationInsights" />``` to ```<TelemetryModules>``` node to get SDK self-diagnostics messages from your application.
- BASE: ApplicationInsights.config file search order was changed. File from the bin/ folder will no longer be used even if exists for the web applications.
- BASE: Nullable properties are now supported in ApplicationInsights.config.
- BASE: DeveloperMode property of ```ITelemetryChannel``` interface became a nullable bool.

## Version 0.16

- BASE: SDK now supports dnx target platform to enable monitoring of [.NET Core framework](http://www.dotnetfoundation.org/NETCore5) applications.
- BASE: Instance of ```TelemetryClient``` do not cache Instrumentation Key anymore. Now if instrumentation key wasn't set to ```TelemetryClient``` explicitly ```InstrumentationKey``` will return null. It fixes an issue when you set ```TelemetryConfiguration.Active.InstrumentationKey``` after some telemetry was already collected, telemetry modules like dependency collector, web requests data collection and performance counters collector will use new instrumentation key.

## Version 0.15

- BASE: New property ```Operation.SyntheticSource``` now available on ```TelemetryContext```. Now you can mark your telemetry items as not a real user traffic and specify how this traffic was generated. As an example by setting this property you can distinguish traffic from your test automation from load test traffic.
- BASE: Channel logic was moved to the separate NuGet called Microsoft.ApplicationInsights.PersistenceChannel. Default channel is now called InMemoryChannel
- BASE: New method ```TelemetryClient.Flush``` allows to flush telemetry items from the buffer synchronously

## Version 0.13

No release notes for older versions available.



----------

## Version 2.11.2
- WEB: [Fix Sql dependency collection bug in .NET Core 3.0 with Microsoft.Data.SqlClient.](https://github.com/Microsoft/ApplicationInsights-dotnet-server/issues/1291)

## Version 2.11.1
- WEB: [Fix Sql dependency parent id to match W3CTraceContext format](https://github.com/Microsoft/ApplicationInsights-dotnet-server/issues/1277)
- WEB: [Fix EventCounters so that it appear as CustomMetrics as opposed to PerformanceCounters.](https://github.com/Microsoft/ApplicationInsights-dotnet-server/issues/1280)

## Version 2.11.0
- WEB: [Fix Sql dependency tracking in .NET Core 3.0 which uses Microsoft.Data.SqlClient instead of System.Data.SqlClient](https://github.com/Microsoft/ApplicationInsights-dotnet-server/issues/1263)
- WEB: Updated Base SDK to 2.11.0
- WEB: Updated Microsoft.AspNet.TelemetryCorrelation to 1.0.7
- WEB: Updated System.Diagnostics.DiagnosticSource to 4.6.0

## Version 2.11.0-beta2
- WEB: Updated Base SDK to 2.11.0-beta2
- WEB: [Add NetStandard2.0 Target for WindowsServerPackage](https://github.com/Microsoft/ApplicationInsights-dotnet-server/issues/1212)
- WEB: [Add NetStandard2.0 Target for DependencyCollector](https://github.com/Microsoft/ApplicationInsights-dotnet-server/issues/1212)
- WEB: [QuickPulse/LiveMetrics background thread safeguards added to never throw unhandled exception.](https://github.com/microsoft/ApplicationInsights-dotnet-server/issues/1088)
- WEB: [Make QuickPulse server id configurable to distinguish multiple role instances running on the same host](https://github.com/microsoft/ApplicationInsights-dotnet-server/issues/1253)
- WEB: [Switch W3C Trace-Context on by default and leverage implementation from .NET in requests and depedencies collectors](https://github.com/microsoft/ApplicationInsights-dotnet-server/pull/1252)
- WEB: [Support correlation-context in absence of Request-Id or traceparent](https://github.com/microsoft/ApplicationInsights-dotnet-server/issues/1215)

## Version 2.11.0-beta1
- WEB: [Add support for Event Counter collection.](https://github.com/Microsoft/ApplicationInsights-dotnet-server/issues/1222)
- WEB: [Support for Process CPU and Process Memory perf counters in all platforms including Linux.](https://github.com/microsoft/ApplicationInsights-dotnet-server/issues/1189)
- WEB: [Azure Web App for Windows Containers to use regular PerfCounter mechanism.](https://github.com/microsoft/ApplicationInsights-dotnet-server/pull/1167) 
- WEB: Experimental: [Defer populating RequestTelemetry properties.](https://github.com/Microsoft/ApplicationInsights-dotnet-server/issues/1173) 
- WEB: [Fix: Replaced non-threadsafe HashSet with ConcurrentDictionary in RequestTrackingTelemetryModule.IsHandlerToFilter](https://github.com/microsoft/ApplicationInsights-dotnet-server/pull/1211)
- WEB: SDL: [Guard against malicious headers in quickpulse](https://github.com/microsoft/ApplicationInsights-dotnet-server/pull/1191)

## Version 2.10.0
- WEB: Updated Base SDK to 2.10.0

## Version 2.10.0-beta4
- WEB: Updated Base SDK to 2.10.0-beta4

## Version 2.10.0-beta3
- WEB: [Fix: QuickPulseTelemetryModule.Dispose should not throw if module was not initialized](https://github.com/Microsoft/ApplicationInsights-dotnet-server/pull/1170)
- WEB: Added NetStandard2.0 Target for PerfCounter project.
- WEB: Added support for PerfCounters for .Net Core Apps in Windows.
- WEB: Updated Base SDK to 2.10.0-beta3

## Version 2.10.0-beta2
- WEB: Updated Base SDK to 2.10.0-beta2

## Version 2.9.1
- WEB: Updates Base SDK to version 2.9.1

## Version 2.9.0
- WEB: [Fix: remove unused reference to Microsoft.AspNet.TelemetryCorrelation package from DependencyCollector](https://github.com/Microsoft/ApplicationInsights-dotnet-server/pull/1136)
- WEB: [Move W3C support from DependencyCollector package to base SDK, deprecate W3C support in DependencyCollector](https://github.com/Microsoft/ApplicationInsights-dotnet-server/pull/1138)

## Version 2.9.0-beta3
- WEB: Update Base SDK to version 2.9.0-beta3
- WEB: [Fix: Correlation doesn't work for localhost](https://github.com/Microsoft/ApplicationInsights-dotnet-server/issues/1120). If you are upgrading and have previously opted into legacy header injection via `DependencyTrackingTelemetryModule.EnableLegacyCorrelationHeadersInjection` and run app locally with Azure Storage Emulator, make sure you manually exclude localhost from correlation headers injection in the `ExcludeComponentCorrelationHttpHeadersOnDomains` under `DependencyCollector`
    ```xml
        <Add>localhost</Add>
        <Add>127.0.0.1</Add>
    ```
- WEB: [Fix: Non-default port is not included into the target for Http dependencies on .NET Core](https://github.com/Microsoft/ApplicationInsights-dotnet-server/issues/1121)
- WEB: [When Activity has root id compatible with W3C trace Id, use it as trace id](https://github.com/Microsoft/ApplicationInsights-dotnet-server/pull/1107)

## Version 2.9.0-beta1
- WEB: [Prevent duplicate dependency collection in multi-host apps](https://github.com/Microsoft/ApplicationInsights-aspnetcore/issues/621)
- WEB: [Fix missing transactions Sql dependencies](https://github.com/Microsoft/ApplicationInsights-dotnet-server/pull/1031)
- WEB: [Fix: Do not stop Activity in the Stop events, set end time instead](https://github.com/Microsoft/ApplicationInsights-dotnet-server/issues/1038)
- WEB: [Fix: Add appSrv_ResourceGroup field to heartbeat properties from App Service](https://github.com/Microsoft/ApplicationInsights-dotnet-server/issues/1046)
- WEB: [Add Azure Search dependency telemetry](https://github.com/Microsoft/ApplicationInsights-dotnet-server/issues/1048)
- WEB: [Fix: Sql dependency tracking broken in 2.8.0+. Dependency operation is not stopped and becomes parent of subsequent operations](https://github.com/Microsoft/ApplicationInsights-dotnet-server/issues/1090)
- WEB: [Fix: Wrong parentId reported on the SqlClient dependency on .NET Core](https://github.com/Microsoft/ApplicationInsights-aspnetcore/issues/778)
- WEB: [Perf Fix - Replace TelemetryClient.Initialize() with TelemetryClient.InitializeInstrumentationKey() to avoid calling initializers more than once. ](https://github.com/Microsoft/ApplicationInsights-dotnet-server/issues/1094)

## Version 2.8.0-beta2
- WEB: [LiveMetrics (QuickPulse) TelemetryProcessor added automatically to the default ApplicationInsights.config are moved under the default telemetry sink.](https://github.com/Microsoft/ApplicationInsights-dotnet-server/pull/987)
	If you are upgrading, and have added/modified TelemetryProcessors, make sure to copy them to the default sink section.
- WEB: [Microsoft.AspNet.TelemetryCorrelaiton package update to 1.0.4](https://github.com/Microsoft/ApplicationInsights-dotnet-server/issues/991)
- WEB: Add vmScaleSetName field to heartbeat properties collected by AzureInstanceMetadataTelemetryModule to allow navigation to right Azure VM Scale Set
- WEB: [Allow users to ignore specific UnobservedTaskExceptions](https://github.com/Microsoft/ApplicationInsights-dotnet-server/issues/1026)

## Version 2.8.0-beta1
- WEB: [Adds opt-in support for W3C distributed tracing standard](https://github.com/Microsoft/ApplicationInsights-dotnet-server/pull/945)
- WEB: Update Base SDK to version 2.8.0-beta1

## Version 2.7.2
- WEB: [Fix ServiceBus requests correlation](https://github.com/Microsoft/ApplicationInsights-dotnet-server/issues/970)

## Version 2.7.0-beta4
- WEB: [When there is no parent operation, generate W3C compatible operation Id](https://github.com/Microsoft/ApplicationInsights-dotnet-server/pull/952)

## Version 2.7.0-beta3
- WEB: [Fix: SerializationException resolving Activity in cross app-domain calls](https://github.com/Microsoft/ApplicationInsights-dotnet-server/issues/613)
- WEB: [Fix: Race condition in generic diagnostic source listener](https://github.com/Microsoft/ApplicationInsights-dotnet-server/pull/948)

## Version 2.7.0-beta1
- WEB: [Add operation details for HTTP and SQL operation to the dependency telemetry.](https://github.com/Microsoft/ApplicationInsights-dotnet-server/issues/900)
- WEB: [Fix: Do not call base HandleErrorAttribute.OnException in MVC unhandled exception filter](https://github.com/Microsoft/ApplicationInsights-dotnet-server/issues/921)
- WEB: [Send UserActionable event about correlation issue with HTTP request with body when .NET 4.7.1 is not installed](https://github.com/Microsoft/ApplicationInsights-dotnet-server/pull/903)
- WEB: [Added support to collect Perf Counters for .NET Core Apps if running inside Azure WebApps](https://github.com/Microsoft/ApplicationInsights-dotnet-server/issues/889)
- WEB: [Opt-in legacy correlation headers (x-ms-request-id and x-ms-request-root-id) extraction and injection](https://github.com/Microsoft/ApplicationInsights-dotnet-server/issues/887)
- WEB: [Fix: Correlation is not working for POST requests](https://github.com/Microsoft/ApplicationInsights-dotnet-server/pull/898) when .NET 4.7.1 runtime is installed.
- WEB: [Fix: Tracking mixed HTTP responses with and without content](https://github.com/Microsoft/ApplicationInsights-dotnet-server/pull/919)


## Version 2.6.0-beta4
- WEB: [Remove CorrelationIdLookupHelper. Use TelemetryConfiguration.ApplicationIdProvider instead.](https://github.com/Microsoft/ApplicationInsights-dotnet-server/pull/880) With this change you can update URL to query application ID from which enables environments with reverse proxy configuration to access Application Insights ednpoints.
- WEB: [Update Microsoft.AspNet.TelemetryCorrelation package to 1.0.1: Fix endless loop when activity stack is broken](https://github.com/aspnet/Microsoft.AspNet.TelemetryCorrelation/issues/22)
- WEB: [Fix: Failed HTTP outgoing requests are not tracked on .NET Core](https://github.com/Microsoft/ApplicationInsights-dotnet-server/issues/780)
- WEB: [Enable collection of Available Memory counter on Azure Web Apps](https://github.com/Microsoft/ApplicationInsights-dotnet-server/issues/585)


## Version 2.6.0-beta3
- WEB: [Ignore Deprecated events if running under netcore20](https://github.com/Microsoft/ApplicationInsights-dotnet-server/issues/848)
- WEB: [Implement unhandled exception auto-tracking (500 requests) for MVC 5 and WebAPI 2 applications.](https://github.com/Microsoft/ApplicationInsights-dotnet-server/pull/847)
- WEB: [Enable .NET Core platform in WindowsServer SDK. This enables the following modules in .NET Standard applications:](https://github.com/Microsoft/ApplicationInsights-dotnet-server/issues/854)
  - `AzureInstanceMetadataTelemetryModule` *(used in heartbeats)*
  - `AzureWebAppRoleEnvironmentTelemetryInitializer`
  - `BuildInfoConfigComponentVersionTelemetryInitializer`
  - `DeveloperModeWithDebuggerAttachedTelemetryModule`
  - `UnobservedExceptionTelemetryModule`
- WEB: [Add default heartbeat properties for Azure App Services (web apps).](https://github.com/Microsoft/ApplicationInsights-dotnet-server/issues/868)

## Version 2.6.0-beta2
- WEB: [Added a max length restriction to values passed in through requests.](https://github.com/Microsoft/ApplicationInsights-dotnet-server/pull/810)
- WEB: [Fix: Dependency Telemetry is not collected with DiagnosticSource when response does not have content.](https://github.com/Microsoft/ApplicationInsights-dotnet-server/issues/739)
- WEB: [Expose Request-Context in Access-Control-Expose-Headers header, and that allows cross-component correlation between AJAX dependencies and server-side requests.](https://github.com/Microsoft/ApplicationInsights-dotnet-server/issues/802)
- WEB: [Improve DependencyCollectorEventSource.Log.CurrentActivityIsNull](https://github.com/Microsoft/ApplicationInsights-dotnet-server/issues/799)
- WEB: A significant number of upgrades to our testing infrastructure.
- WEB: Add Azure Instance Metadata information to heartbeat properties in WindowsServer package (full framework only). [Completes issue #666 from -dotnet repo](https://github.com/Microsoft/ApplicationInsights-dotnet/issues/666)


## Version 2.5.0
- WEB: [Fix: System.InvalidCastException for SQL Dependency](https://github.com/Microsoft/ApplicationInsights-dotnet-server/issues/782)


## Version 2.5.0-beta2
- WEB: [Fix: When debugging netcoreapp2.0 in VS, http dependencies are tracked twice](https://github.com/Microsoft/ApplicationInsights-dotnet-server/issues/723)
- WEB: [Fix: DependencyCollector check if exits before add](https://github.com/Microsoft/ApplicationInsights-dotnet-server/issues/724)
- WEB: [Track requests and dependencies from ServiceBus .NET Client (Microsoft.Azure.ServiceBus 3.0.0](https://github.com/Microsoft/ApplicationInsights-dotnet-server/issues/740)
- WEB: [Fix: REST API Request filter bug](https://github.com/Microsoft/ApplicationInsights-dotnet-server/issues/175)
- WEB: [Fix: SyntheticUserAgentTelemetryInitializer null check](https://github.com/Microsoft/ApplicationInsights-dotnet-server/issues/750)
- WEB: [Track dependencies from EventHubs .NET Client (Microsoft.Azure.EventHubs 1.1.0)](https://github.com/Microsoft/ApplicationInsights-dotnet-server/issues/753)

**Project**
- WEB: [Moved common files to Shared projects](https://github.com/Microsoft/ApplicationInsights-dotnet-server/pull/730)
- WEB: [Stabilizing QuickPulse tests](https://github.com/Microsoft/ApplicationInsights-dotnet-server/pull/736)
- WEB: [Make local debug of DependencyCollector functional tests easier](https://github.com/Microsoft/ApplicationInsights-dotnet-server/pull/738)
- WEB: [More DependencyCollector tests](https://github.com/Microsoft/ApplicationInsights-dotnet-server/pull/741)
- WEB: [Increase max timeout for QuickPulse tests](https://github.com/Microsoft/ApplicationInsights-dotnet-server/pull/744)
- WEB: [Increase tests codecoverage](https://github.com/Microsoft/ApplicationInsights-dotnet-server/pull/745)
- WEB: [More DependencyCollector functional tests](https://github.com/Microsoft/ApplicationInsights-dotnet-server/pull/746)


## Version 2.5.0-beta1
- WEB: Removed `net40` targets from all packages. Use the version 2.4 of SDK if your application is still compiled with the framework 4.0.
- WEB: Adds ADO SQL dependency collection for SqlClient (System.Data.SqlClient) on .NET Core versions 1.0 and 2.0.
- WEB: /ping calls to Live Metrics Stream (aka QuickPulse) now contain the invariant version of the agent.
- WEB: [Fix App Id Lookup bug](https://github.com/Microsoft/ApplicationInsights-dotnet-server/issues/683)
- WEB: [Fix DiagnosticsListener should have safe OnNext](https://github.com/Microsoft/ApplicationInsights-dotnet-server/issues/649)
- WEB: [Fix PerfCounterCollector module may go into endless loop (ASP.NET Core on Full Framework)](https://github.com/Microsoft/ApplicationInsights-dotnet-server/issues/678)
- WEB: [Fix Start Timestamp is not set for Http dependency telemetry in dotnet core 2.0](https://github.com/Microsoft/ApplicationInsights-dotnet-server/issues/658)
- WEB: [Support collecting non-HTTP dependency calls from 3rd party libraries](https://github.com/Microsoft/ApplicationInsights-dotnet-server/issues/665)
- WEB: Bugfix for CorrelationIdLookup NullRef Ex
- WEB: [Added Test App for testing DependencyCollector on .NET Core 2.0](https://github.com/Microsoft/ApplicationInsights-dotnet-server/issues/572)

**Project**
- WEB: install.ps1 is now signed
- WEB: increase max allowed runtime of functional tests
- WEB: fix for "project system has encountered an error"


## Version 2.4.1
- WEB: [Hotfix to address the issue where DependencyCollection breaks Azure Storage Emulator calls](https://github.com/Microsoft/ApplicationInsights-dotnet-server/issues/640)

## Version 2.4.0
- WEB: Updated version of DiagnosticSource to 4.4.0

## Version 2.4.0-beta5
- WEB: Updated version of DiagnosticSource referenced.

## Version 2.4.0-beta4
- WEB: Bug fixes.

## Version 2.4.0-beta3
- WEB: Exceptions statistics feature is not enabled by default
- WEB: [Parse AppId from HTTP response headers when dependency collection is facilitated with Http Desktop DiagnosticSource](https://github.com/Microsoft/ApplicationInsights-dotnet-server/issues/509)
- WEB: [Fix double correlation header injection with latest DiagnosticSource](https://github.com/Microsoft/ApplicationInsights-dotnet-server/issues/530)
- WEB: `DisableDiagnosticSourceInstrumentation` configuration flag was added to `DependencyTrackingTelemetryModule`.
  * By default `DisableDiagnosticSourceInstrumentation` is set to false, this enables correlation of telemetry items and [application map](http://aka.ms/AiAppMapPreview) in multi-tier applications.
  * When `DisableDiagnosticSourceInstrumentation` is set to true (so that the instrumentation is off)
    * correlation between requests, dependencies, and other telemetry items is limited,
    * telemetry correlation between multiple services involved in the operation processing is not possible,
    * and the cross-component correlation feature and application map experience is limited.
  * **Note**: this configuration option has no effect for applications that run in an Azure Web Application with the [ApplicationInsights site extension](https://docs.microsoft.com/en-us/azure/application-insights/app-insights-azure-web-apps) or have [runtime instrumentation](https://github.com/Microsoft/ApplicationInsights-Home/tree/master/Samples/AzureEmailService/WorkerRoleA#report-dependencies).
- WEB: [Fix memory leak in Dependency collector](https://github.com/Microsoft/ApplicationInsights-dotnet-server/issues/554)

## Version 2.4.0-beta2
- WEB: [Handle breaking changes from DiagnosticSource](https://github.com/Microsoft/ApplicationInsights-dotnet-server/issues/480)
- WEB: [Exceptions statistics metrics uses `.Context.Operation.Name` instead of custom property `operationName`](https://github.com/Microsoft/ApplicationInsights-dotnet-server/issues/394)
- WEB: [Separate event source names for Web and Dependency Modules to fix the bug](https://github.com/Microsoft/ApplicationInsights-dotnet-server/issues/508)
- WEB: [Fix DependencyCollector memory leak on netcoreapp1.1 and prior](https://github.com/Microsoft/ApplicationInsights-dotnet-server/issues/514)

## Version 2.4.0-beta1
- WEB: Report status code for the dependencies failed with non-protocol issue like DNS resolution or SSL shakeup problems.
- WEB: Implemented automatic telemetry correlation: all telemetry reported within the scope of the request is correlated to RequestTelemetry reported for the request.
- WEB: Implemented [Correlation HTTP protocol](https://github.com/lmolkova/correlation/blob/master/http_protocol_proposal_v1.md): default headers to pass Operation Root Id and Parent Id were changed. This version is backward compatible with previously supported headers. 
- WEB: Implemented injection into the HTTP stack for .NET 4.6 to leverage DiagnosticSource to gain access to the WebRequest and WebResponse objects for header injections, without the need of using the profiler.
- WEB: Dependency to System.Diagnostics.DiagnosticsSource package is added for Web SDK on .NET 4.5.
- WEB: Improvements to exception statistics, e.g. 2 of each type of exception will be output via TrackException
- WEB: New ```AspNetDiagnosticTelemetryModule``` introduced for Web SDK on .NET 4.5, it consumes events from [Microsoft.AspNet.TelemetryCorrelation package](https://github.com/aspnet/AspNetCorrelationIdTracker) about incoming Http requests.
- WEB: Dependency to Microsoft.AspNet.TelemetryCorrelation package is added for Web SDK on .NET 4.5.
- WEB: Report new performance counter \Process(??APP_WIN32_PROC??)\% Processor Time Normalized that represents process CPU normalized by the processors count

## Version 2.3.0
- WEB: Includes all changes since 2.2.0 stable release.
- WEB: Exception statistics feature introduced in beta version is removed.

## Version 2.3.0-beta3
- WEB: Exception statistics improvements and other minor bug fixes. [Full list.] (https://github.com/Microsoft/ApplicationInsights-dotnet-server/milestone/19?closed=1)
- WEB: Cross Components Correlation ID changed from SHA(instrumentation key) to Application ID retrieved from http endpoint `api/profiles/{ikey}/appId`.

## Version 2.3.0-beta2
- WEB: Automatic collection of first chance exceptions statistics. Use a query like this in Application Analytics to query for this statistics:
  ```
  customMetrics
  | where timestamp > ago(5d)
  | where name == "Exceptions thrown" 
  | extend type = tostring(customDimensions.type), method = tostring(customDimensions.method), operation = tostring(customDimensions.operation) 
  | summarize sum(value), sum(valueCount) by type, method, operation 
  ```
- WEB: Add dependency collection for System.Data.SqlClient.SqlConnection.Open and System.Data.SqlClient.SqlConnection.OpenAsync by Profiler instrumentation. Dependencies are sent only for failed connections.
- WEB: Top 5 CPU reporting for Live Metrics Stream (aka QuickPulse). QuickPulseTelemetryModule now reports the names and CPU consumption values of top 5 CPU consuming processes.

## Version 2.3.0-beta1
- WEB: Added the ability to correlate http request made between different components represented by different application insights resources. This feeds into the improved [application map experience](http://aka.ms/AiAppMapPreview).

## Version 2.2.0
- WEB: Includes all changes since 2.1.0 stable release.
- WEB: [Fixed issue with identifying which environment generated an event](https://github.com/Microsoft/ApplicationInsights-dotnet-server/issues/248)

## Version 2.2.0-beta6
- WEB: [Fixed redundant dependency items issue](https://github.com/Microsoft/ApplicationInsights-dotnet-server/issues/196)
- WEB: [Fixed issue reporting CPU Metric](https://github.com/Microsoft/ApplicationInsights-dotnet-server/issues/225)
- WEB: [Fixed source of web app instance identification](https://github.com/Microsoft/ApplicationInsights-dotnet-server/issues/226)
- WEB: Updated package references.
- WEB: DependencyCollection nuget package was updated to Agent.Intercept nuget version 2.0.6.

## Version 2.2.0-beta4
- WEB: ```DomainNameRoleInstanceTelemetryInitializer``` is obsolete. Role instance is still populated with machine name as it was before.
- WEB: New ```AzureWebAppRoleEnvironmentTelemetryInitializer``` telemetry initializer that populates role name and role instance name for Azure Web Apps.
- WEB: Support of performance collection and live metrics for Azure Web Apps is enabled.

## Version 2.2.0-beta3
- WEB: New property `DefaultCounters` in `PerformanceCollectorModule` to control the list of standard counters that will be collected
- WEB: Default performance counters will be reported as metrics
- WEB: When you instantiate `DependencyTrackingTelemetryModule` in code it will not detect certain http dependencies as Azure Storage calls. You need to register a telemetry initializer `HttpDependenciesParsingTelemetryInitializer` to enable this functionality. This telemetry initializer will be registered automatically during NuGet installation.
- WEB: DependencyCollection nuget package was updated to Agent.Intercept nuget version 2.0.5.
- WEB: The list of userAgent substrings that indicate that traffic is from a synthetic source was minimized for performance reasons. If you want to include more substrings please add them under SyntheticUserAgentTelemetryInitializer/Filters. (List of filters that were used before is saved as a comment in the configuration file)
- WEB: Added HTTP dependencies parsing support for Azure tables, queues, and services (.svc & .asmx).
- WEB: Added automatic collection of source component correlation id (instrumenation key hash) for incoming requests and target component correlation id for dependencies.

## Version 2.2.0-beta2

- WEB: DependencyCollection nuget package was updated to Agent.Intercept nuget version 2.0.1. Agent.Intercept nuget was updated to EventSource.Redist version 1.1.28. 
- WEB: SQL dependencies will have SQL error message being added to custom properties collection if application uses profiler instrumentation (either instrumented with StatusMonitor or just have StatusMonitor on the box with the app)
- WEB: Allow all characters in custom counters ReportAs property.
- WEB: QuickPulse (Live Metrics Stream) was updated to include Live Failures

## Version 2.2.0-beta1

- WEB: ResultCode for successful Sql calls will be collected as 0 (before it was not sent).
- WEB: Fixed ResultCode sometimes not being collected for failed dependencies
- WEB: RequestTelemetry.UserAgent is collected automatically. 

## Version 2.1.0-beta4

- WEB: No code changes. Updated to Core 2.1-beta4.

## Version 2.1.0-beta3
- WEB: Remove support for HTTP dependencies in .NET 4.5.2 (4.5.2 applications running on 4.5.2; 4.5.2 applications running on 4.6 are still supported) without Status Monitor on the box.
- WEB: Add http verb to dependency name collected by SDK without Status Monitor on the box


## Version 2.1.0-beta2
- WEB: Http requests to LiveMetricsStream (Feature not surfaced in UI yet) backend were tracked as dependencies. They will be filtered out starting this version.
- WEB: There are no other changes 

## Version 2.1.0-beta1

- WEB: Upgraded to depend on EventSource.Redist nuget version 1.1.28
- WEB: Upgraded to depend on Microsoft.Bcl nuget version 1.1.10
- WEB: LiveMetricsStream feature is introduced (Not surfaced in UI yet)

## Version 2.0.0 
- WEB: Performance counter collection is no longer supported when running under IIS Express.

## Version 2.0.0-rc1

**Dependencies:**

- WEB: Http dependency success is determined on the base of http status code. Before it was true if there was no exception. But when one uses HttpClient there is no exceptions so all dependencies were marked as successful. Also in case if response is not available status code was set to -1. Now now status code will be reported.

## Version 2.0.0-beta4

**Web:**

- WEB: WebApps AlwaysOn requests with ResponseCode less than 400 will be filtered out. 
- WEB: User agent and request handler filters can be configured. Previous behavior filtered out only a default set of request handlers and user agent strings, 
  now custom filters can be added to the ApplicationInsights.config file through the ```TelemetryProcessors``` section. 
  Telemetry for requests with HttpContext.Current that matches these filters will not be sent.
- WEB: If multiple simultaneous calls are made on a ```SqlCommand``` object, only one dependency is recorded. The second
  call will be failed immediately by ```SqlCommand``` and will not be recorded as a dependency.

## Version 2.0.0-beta3
**Web:**

- WEB: Use ```OperationCorrelationTelemetryInitializer``` instead of ```OperationIdTelemetryInitializer```
- WEB: User Agent and Client IP will not be collected by default. User Agent telemetry initializer was removed
- WEB: ```DependencyTelemetry.Async``` field will not be collected by dependency collector telemetry module
- WEB: Static content and diagnostics requests will not be collected by request telemetry module. Use ```HandlersToFilter``` of ```RequestTrackingTelemetryModule``` collection to filter out requests generated by certain http handlers
- WEB: Autogenerated request telemetry is accessible though HttpContext extension method: System.Web.HttpContextExtension.GetRequestTelemetry

## Version 2.0.0-beta2
**Web:**

- WEB: RequestTelemetry.Name is not initialized any longer. RequestTelemetry.Context.Operaiton.Name will be used instead.
- WEB: Response code 401 is part of the normal authentication handshake and will result in a succesfull request.

**WindowsServer**

- WEB: DeviceTelemetryInitializer is not installed by default any more.

## Version 2.0.0-beta1

**Web:**

- WEB: Added `Microsoft.ApplicationInsights.Web.AccountIdTelemetryInitializer`, `Microsoft.ApplicationInsights.Web.AuthenticatedUserIdTelemetryInitializer` that initialize authenticated user context as set by Javascript SDK.
- WEB: Added `Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.ITelemetryProcessor` and fixed-rate Sampling support as an implementation of it.
- WEB: Added `Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.TelemetryChannelBuilder` that allows creation of a `Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.ServerTelemetryChannel` with a set of `Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.ITelemetryProcessor`.

## Version 1.2.0
**Web:**

- WEB: Telemetry initializers that do not have dependencies on ASP.NET libraries were moved to the new dependency nuget "Microsoft.ApplicationInsights.WindowsServer"
- WEB: Microsoft.ApplicationInsights.Web.dll was renamed on Microsoft.AI.Web.dll
- WEB: Microsoft.Web.TelemetryChannel nuget was renamed on Microsoft.WindowsServer.TelemetryChannel. TelemetryChannel assembly was also renamed.
- WEB: All namespaces that are part of Web SDK were changed to exlude "Extensibility" part. That incudes all telemetry initializers in applicationinsights.config and ApplicationInsightsWebTracking module in web.config.

**Dependencies:**

- WEB: Dependencies collected using runtime instrumentaiton agent (enabled via Status Monitor or Azure WebSite extension) will not be marked as asynchronous if there are no HttpContext.Current on the thread.
- WEB: Property ```SamplingRatio``` of ```DependencyTrackingTelemetryModule``` does nothing and marked as obsolete.

**Performance Counters**

- WEB: Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector assembly was renamed on Microsoft.AI.PerfCounterCollector

**WindowsServer**

- WEB: First version of the package. The package has common logic that will be shared between Web and Non-Web Windows applications.

## Version 1.0.0

**Web:**

- WEB: Moved telemetry initializers and telemetry modules from separate sub-namespaces to the root 
  `Microsoft.ApplicationInsights.Extensibility.Web` namespace.
- WEB: Removed "Web" prefix from names of telemetry initializers and telemetry modules because it is already included in the 
  `Microsoft.ApplicationInsights.Extensibility.Web` namespace name.
- WEB: Moved `DeviceContextInitializer` from the `Microsoft.ApplicationInsights` assembly to the 
  `Microsoft.ApplicationInsights.Extensibility.Web` assembly and converted it to an `ITelemetryInitializer`.

**Dependencies:**

- WEB: Change namespace and assembly names from `Microsoft.ApplicationInsights.Extensibility.RuntimeTelemetry` to 
  `Microsoft.ApplicationInsights.Extensibility.DependencyCollector` for consistency with the name of the NuGet package.
- WEB: Rename `RemoteDependencyModule` to `DependencyTrackingTelemetryModule`.

**Performance Counters**

- WEB: Rename 'CustomPerformanceCounterCollectionRequest' to 'PerformanceCounterCollectionRequest'.

## Version 0.17

**Web:**

- WEB: Removed dependency to EventSource NuGet for the framework 4.5 applications.
- WEB: Anonymous User and Session cookies will not be generated on server side. Telemetry modules ```WebSessionTrackingTelemetryModule``` and ```WebUserTrackingTelemetryModule``` are no longer supported and were removed from ApplicationInsights.config file. Cookies from JavaScript SDK will be respected.
- WEB: Persistence channel optimized for high-load scenarios is used for web SDK. "Spiral of death" issue fixed. Spiral of death is a condition when spike in telemetry items count that greatly exceeds throttling limit on endpoint will lead to retry after certain time and will be throttled during retry again.
- WEB: Developer Mode is optimized for production. If left by mistake it will not cause as big overhead as before attempting to output additional information.
- WEB: Developer Mode by default will only be enabled when application is under debugger. You can override it using ```DeveloperMode``` property of  ```ITelemetryChannel``` interface.

**Dependencies:**

- WEB: Removed dependency to EventSource NuGet for the framework 4.5 applications.

**Performance Counters**

- WEB: Diagnostic messages pertaining to performance counter collection are now merged into a single unified message that is logged at application start-up. Detailed failure information is still available through PerfView.

## Version 0.15

**Web:**

- WEB: Application Insights Web package now detects the traffic from Availability monitoring of Application Insights and marks it with specific ```SyntheticSource``` property.

## Version 0.13

No release notes for older versions available.

----------

## Version 2.11.0
- LOGGING: Update Base SDK to 2.11.0
- LOGGING: Update System.Diagnostics.DiagnosticSource to 4.6.0. 

## Version 2.11.0-beta2
- LOGGING: Update Base SDK to version 2.11.0-beta2
- LOGGING: Update System.Diagnostics.DiagnosticSource to 4.6.0-preview7. 

### Version 2.11.0-beta1
- LOGGING: Update Base SDK to version 2.11.0-beta1

### Version 2.10.0
- LOGGING: Update Base SDK to version 2.10.0

### Version 2.10.0-beta4
- LOGGING: Update Base SDK to version 2.10.0-beta4
- LOGGING: [ILogger - If an exception is passed to log, then Exception.Message is populated as ExceptionTelemetry.Message. If TrackExceptionsAsExceptionTelemetry is false, then Exception.Message is stored as custom property "ExceptionMessage"](https://github.com/Microsoft/ApplicationInsights-dotnet-logging/pull/282)

### Version 2.9.1
- LOGGING: Update Base SDK to version 2.9.1

### Version 2.9.0
- LOGGING: Update Base SDK to version 2.9.0

### Version 2.9.0-beta3
- LOGGING: Update Base SDK to version 2.9.0-beta3
- LOGGING: [ILogger implementation for ApplicationInsights](https://github.com/Microsoft/ApplicationInsights-dotnet-logging/pull/239)
- LOGGING: Update log4net reference to [2.0.7](https://www.nuget.org/packages/log4net/2.0.7)

### Version 2.8.1
- LOGGING: Update BaseSdk reference to 2.8.1. See [release notes](https://github.com/Microsoft/ApplicationInsights-dotnet/releases) for more information.

### Version 2.7.2
- LOGGING: [NLog can perform Layout of InstrumentationKey](https://github.com/Microsoft/ApplicationInsights-dotnet-logging/pull/203)
- LOGGING: Upgrade `System.Diagnostics.DiagnosticSource` to version 4.5.0
- LOGGING: [Event Source telemetry module: Microsoft-ApplicationInsights-Data id disabled by default to work around CLR bug](https://github.com/Microsoft/ApplicationInsights-dotnet-logging/pull/206)

### Version 2.6.4
- LOGGING: [Log4Net new supports NetStandard 1.3!](https://github.com/Microsoft/ApplicationInsights-dotnet-logging/pull/167)
- LOGGING: [NLog Flush should include async delay](https://github.com/Microsoft/ApplicationInsights-dotnet-logging/pull/176)
- LOGGING: [NLog can include additional ContextProperties](https://github.com/Microsoft/ApplicationInsights-dotnet-logging/pull/183)
- LOGGING: [DiagnosticSourceTelemetryModule supports onEventWrittenHandler](https://github.com/Microsoft/ApplicationInsights-dotnet-logging/pull/184)
- LOGGING: [Fix: Prevent double telemetry if DiagnosticSourceTelemetryModule is initialized twice](https://github.com/Microsoft/ApplicationInsights-dotnet-logging/pull/181)

### Version 2.6.0-beta3
- LOGGING: [NetStandard Support for TraceListener](https://github.com/Microsoft/ApplicationInsights-dotnet-logging/pull/166)
- LOGGING: [NetStandard Support for NLog and log4net](https://github.com/Microsoft/ApplicationInsights-dotnet-logging/pull/167)
- LOGGING: [NLog and log4net can Flush](https://github.com/Microsoft/ApplicationInsights-dotnet-logging/pull/167)
- LOGGING: Update log4net reference to [2.0.6](https://www.nuget.org/packages/log4net/2.0.6)

### Version 2.6.0-beta2
- LOGGING: [Include NLog GlobalDiagnosticsContext properties](https://github.com/Microsoft/ApplicationInsights-dotnet-logging/pull/152)
- LOGGING: [Remove automatic collection of User Id](https://github.com/Microsoft/ApplicationInsights-dotnet-logging/issues/153)

### Version 2.5.0
- LOGGING: Update Application Insights API reference to 2.5.0
- LOGGING: Removed framework 4.0 support
- LOGGING: For EventSourceTelemetryModule, allows black list the event sources. Drops the events to those in the list.
- LOGGING: [Fix Deadlock over EventSourceTelemetryModule](https://github.com/Microsoft/ApplicationInsights-dotnet-logging/issues/109)
- LOGGING: [Extensibel payload handler](https://github.com/Microsoft/ApplicationInsights-dotnet-logging/pull/111)
- LOGGING: [Add ProviderName and ProviderGuid properties to TraceTelemetry](https://github.com/Microsoft/ApplicationInsights-dotnet-logging/pull/120)
- LOGGING: [Add support for disabledEventSourceNamePrefix configuration](https://github.com/Microsoft/ApplicationInsights-dotnet-logging/issues/122)
- LOGGING: [Fix ApplicationInsights TraceListener does not respect Flush](https://github.com/Microsoft/ApplicationInsights-dotnet-logging/issues/67)
- LOGGING: [Fix NullReferenceException in DiagnosticSourceListener](https://github.com/Microsoft/ApplicationInsights-dotnet-logging/pull/143)
- LOGGING: [Use InvariantCulture to convert property values](https://github.com/Microsoft/ApplicationInsights-dotnet-logging/pull/144)
- LOGGING: Update NLog reference to [4.4.12](https://github.com/NLog/NLog/releases/tag/v4.4.12)

### Version 2.4.0
- LOGGING: Update Application Insights API reference to [2.4.0]

### Version 2.4.0-beta1/2
- LOGGING: Update Application Insights API reference to [2.4.0-beta3]
- LOGGING: Added support for logs from EventSource, ETW and Diagnostic Source.

### Version 2.1.1

- LOGGING: Update NLog reference to [4.3.8](https://github.com/NLog/NLog/releases/tag/4.3.8)

### Version 2.1.0

- LOGGING: For NLog and Log4Net when exception is traced with a custom message, custom message is added to the properties collection and reported to ApplicationInsights.
- LOGGING: Update Application Insights API reference to [2.1.0](https://github.com/Microsoft/ApplicationInsights-dotnet/releases/tag/v2.1.0)
- LOGGING: Update NLog reference to [4.3.5](https://github.com/NLog/NLog/releases/tag/4.3.5)

### Version 2.0.0

- LOGGING: Update Application Insights API reference to [2.0.0](https://github.com/Microsoft/ApplicationInsights-dotnet/releases/tag/v2.0.0)
- LOGGING: Update NLog reference to [4.2.3](https://github.com/NLog/NLog/releases/tag/4.2.3)
- LOGGING: Update Log4Net reference to [2.0.5 (1.2.15)](http://logging.apache.org/log4net/release/release-notes.html)
- LOGGING: NLog: support [Layout](https://github.com/nlog/NLog/wiki/Layouts)

### Version 1.2.6

- LOGGING: Bug fixes
- LOGGING: log4Net: Collect log4net properties as custom properties. UserName is not a custom property any more (It is collected as telemetry.Context.User.Id). Timestamp is not a custom property any more.
- LOGGING: NLog: Collect NLog properties as custom properties. SequenceID is not a custom property any more (It is collected as telemetry.Sequence). Timestamp is not a custom property any more. 

### Version 1.2.5
- LOGGING: First open source version: References Application Insights API version 1.2.3 or higher.

----------




## Version 2.8.2
- NETCORE: Updated Web SDK to 2.11.2

## Version 2.8.1
- NETCORE: Updated Web SDK to 2.11.1

## Version 2.8.0
- NETCORE: Updated Base SDK/Web SDK/Logging Adaptor SDK to 2.11.0
- NETCORE: Updated System.Diagnostics.DiagnosticSource to 4.6.0

## Version 2.8.0-beta3
- NETCORE: [Make W3C Correlation default and leverage native W3C support from Activity.](https://github.com/microsoft/ApplicationInsights-aspnetcore/pull/958)
- NETCORE: [Make W3C Correlation default and leverage native W3C support from Activity for Asp.Net Core 3.0.](https://github.com/microsoft/ApplicationInsights-aspnetcore/pull/958)
- NETCORE: [Fix: Azure Functions performance degradation when W3C enabled.](https://github.com/microsoft/ApplicationInsights-aspnetcore/issues/900)
- NETCORE: [Fix: AppId is never set is Response Headers.](https://github.com/microsoft/ApplicationInsights-aspnetcore/issues/956)
- NETCORE: [Support correlation-context in absence of request-id or traceparent.](https://github.com/microsoft/ApplicationInsights-aspnetcore/issues/901)
- NETCORE: [Non Product - Asp.Net Core 3.0 Functional Tests Added. This leverages the built-in integration test capability of ASP.NET Core via Microsoft.AspNetCore.MVC.Testing](https://github.com/microsoft/ApplicationInsights-aspnetcore/issues/539)
- NETCORE: [Fix: System.NullReferenceException in WebSessionTelemetryInitializer.](https://github.com/microsoft/ApplicationInsights-aspnetcore/issues/903)
- NETCORE: Updated Base SDK/Web SDK/Logging Adaptor SDK version dependency to 2.11.0-beta2
- NETCORE: Updated System.Diagnostics.DiagnosticSource to 4.6.0-preview8.

- NETCORE: [Add new package for .NET Core WorkerServices (Adds GenericHost support)](https://github.com/microsoft/ApplicationInsights-aspnetcore/issues/708)

## Version 2.8.0-beta2
- NETCORE: [Fix MVCBeforeAction property fetcher to work with .NET Core 3.0 changes.](https://github.com/microsoft/ApplicationInsights-aspnetcore/issues/936)
- NETCORE: [Catch generic exception from DiagnosticSourceListeners and log instead of failing user request.](https://github.com/microsoft/ApplicationInsights-aspnetcore/issues/957)
- NETCORE: [Correct names for Asp.Net Core EventCounters.](https://github.com/microsoft/ApplicationInsights-aspnetcore/issues/945)
- NETCORE: [Obsolete extension methods on IWebHostBuilder in favor of AddApplicationInsights extension method on IServiceCollection.](https://github.com/microsoft/ApplicationInsights-aspnetcore/issues/919)
- NETCORE: [Remove support for deprecated x-ms based correlation headers.](https://github.com/microsoft/ApplicationInsights-aspnetcore/issues/939)
- NETCORE: [Uri for multiple hosts headers is set to "Multiple-Host".](https://github.com/Microsoft/ApplicationInsights-aspnetcore/issues/862)
- NETCORE: [LogLevel changed to Error and stack trace added for generic unknown exception within SDK.](https://github.com/microsoft/ApplicationInsights-aspnetcore/pull/946)

## Version 2.8.0-beta1
- NETCORE: [Add EventCounter collection.](https://github.com/microsoft/ApplicationInsights-aspnetcore/issues/913)
- NETCORE: [Performance fixes: One DiagSource Listener; Head Sampling Feature; No Concurrent Dictionary; etc...](https://github.com/microsoft/ApplicationInsights-aspnetcore/pull/907)
- NETCORE: [Fix: Add `IJavaScriptSnippet` service interface and update the `IServiceCollection` extension to register it for `JavaScriptSnippet`.](https://github.com/microsoft/ApplicationInsights-aspnetcore/issues/890)
- NETCORE: [Make JavaScriptEncoder optional and Fallback to JavaScriptEncoder.Default.](https://github.com/microsoft/ApplicationInsights-aspnetcore/pull/918)
- NETCORE: Updated Web/Base SDK version dependency to 2.10.0-beta4
- NETCORE: Updated Microsoft.Extensions.Logging.ApplicationInsights to 2.10.0-beta4

## Version 2.7.1
- NETCORE: [Fix - ApplicationInsights StartupFilter should not swallow exceptions from downstream ApplicationBuilder.](https://github.com/microsoft/ApplicationInsights-aspnetcore/issues/897)

## Version 2.7.0
- NETCORE: Updated Web/Base SDK version dependency to 2.10.0
- NETCORE: [Remove unused reference to System.Net.Http](https://github.com/microsoft/ApplicationInsights-aspnetcore/pull/879)

## Version 2.7.0-beta4
- NETCORE: [RequestTrackingTelemetryModule is modified to stop tracking exceptions by default, as exceptions are captured by ApplicationInsightsLoggerProvider.](https://github.com/Microsoft/ApplicationInsights-aspnetcore/issues/861)
- NETCORE: Updated Web/Base SDK version dependency to 2.10.0-beta4
- NETCORE: Updated Microsoft.Extensions.Logging.ApplicationInsights to 2.10.0-beta4
- NETCORE: Reliability improvements with additional exception handling.

## Version 2.7.0-beta3
- NETCORE: [Enables Microsoft.Extensions.Logging.ApplicationInsights.ApplicationInsightsLoggerProvider by default. If ApplicationInsightsLoggerProvider was enabled previously using ILoggerFactory extension method, please remove it to prevent duplicate logs.](https://github.com/Microsoft/ApplicationInsights-aspnetcore/issues/854)
- NETCORE: [Remove reference to Microsoft.Extensions.DiagnosticAdapter and use DiagnosticSource subscription APIs directly](https://github.com/Microsoft/ApplicationInsights-aspnetcore/pull/852) 
- NETCORE: [Fix: NullReferenceException in ApplicationInsightsLogger.Log when exception contains a Data entry with a null value](https://github.com/Microsoft/ApplicationInsights-aspnetcore/issues/848)
- NETCORE: [Performance fixes for GetUri, SetKeyHeaderValue, ConcurrentDictionary use and Telemetry Initializers](https://github.com/Microsoft/ApplicationInsights-aspnetcore/pull/864)

## Version 2.7.0-beta2
- NETCORE: Added NetStandard2.0 target.
- NETCORE: Updated Web/Base SDK version dependency to 2.10.0-beta2

## Version 2.6.1
- NETCORE: Updated Web/Base SDK version dependency to 2.9.1

## Version 2.6.0
- NETCORE: Updated Web/Base SDK version dependency to 2.9.0
- NETCORE: [Fix: TypeInitializationException when Microsoft.AspNetCore.Hosting and Microsoft.AspNetCore.Hosting.Abstractions versions do not match](https://github.com/Microsoft/ApplicationInsights-aspnetcore/pull/821)

## Version 2.6.0-beta3
- NETCORE: Updated Web/Base SDK version dependency to 2.9.0-beta3
- NETCORE: [Deprecate ApplicationInsightsLoggerFactoryExtensions.AddApplicationInsights logging extensions in favor of Microsoft.Extensions.Logging.ApplicationInsights package](https://github.com/Microsoft/ApplicationInsights-aspnetcore/issues/817)
- NETCORE: [Fix: Do not track requests by each host in the process](https://github.com/Microsoft/ApplicationInsights-aspnetcore/issues/621)
- NETCORE: [Fix: Correlation doesn't work for localhost](https://github.com/Microsoft/ApplicationInsights-dotnet-server/issues/1120)

## Version 2.6.0-beta2
- NETCORE: Updated Web/Base SDK version dependency to 2.9.0-beta2

## Version 2.6.0-beta1
- NETCORE: Updated Web/Base SDK version dependency to 2.9.0-beta1

## Version 2.5.1
- NETCORE: Update Web/Base SDK version dependency to 2.8.1

## Version 2.5.0
- NETCORE: Traces logged via ILogger is marked with SDK version prefix ilc (.net core) or ilf (.net framework).
- NETCORE: Update Web/Base SDK version dependency to 2.8.0

## Version 2.5.0-beta2
- NETCORE: ComVisible attribute is set to false for the project for compliance reasons.
- NETCORE: [Log exception.Data properties as additional telemetry data](https://github.com/Microsoft/ApplicationInsights-aspnetcore/issues/754)
- NETCORE: Update Web/Base SDK version dependency to 2.8.0-beta2
Applicable if using additional Sinks to forward telemetry to:
- NETCORE: [Default TelemetryProcessors are added to the DefaultSink instead of common TelemetryProcessor pipeline.](https://github.com/Microsoft/ApplicationInsights-aspnetcore/issues/752)
- NETCORE: [TelemetryProcessors added via AddTelemetryProcesor extension method are added to the DefaultSink instead of common TelemetryProcessor pipeline.](https://github.com/Microsoft/ApplicationInsights-aspnetcore/issues/752)
  

## Version 2.5.0-beta1
- NETCORE: [Adds opt-in support for W3C distributed tracing standard](https://github.com/Microsoft/ApplicationInsights-aspnetcore/pull/735)
- NETCORE: Updated Web/Base SDK version dependency to 2.8.0-beta1

## Version 2.4.1
- NETCORE: Patch release to update Web/Base SDK version dependency to 2.7.2 which fixed a bug (https://github.com/Microsoft/ApplicationInsights-dotnet-server/issues/970)

## Version 2.4.0
- NETCORE: Updated Web/Base SDK version dependency to 2.7.1

## Version 2.4.0-beta4
- NETCORE: [Generate W3C compatible operation Id when there is no parent operation](https://github.com/Microsoft/ApplicationInsights-dotnet-server/pull/952)
- NETCORE: Updated Web/Base SDK version dependency to 2.7.0-beta4

## Version 2.4.0-beta3
- NETCORE: [Allow configuring exception tracking in RequestTrackingTelemetryModule and merge OperationCorrelationTelemetryInitializer with RequestTrackingTelemetryModule](https://github.com/Microsoft/ApplicationInsights-aspnetcore/issues/709)
- NETCORE: [Allow disabling response headers injection](https://github.com/Microsoft/ApplicationInsights-aspnetcore/issues/613)
- NETCORE: Updated Web/Base SDK version dependency to 2.7.0-beta3
- NETCORE: The above referenced base SDK contains fix for leaky HttpConnections. (https://github.com/Microsoft/ApplicationInsights-aspnetcore/issues/690)

## Version 2.4.0-beta2
- NETCORE: Updated Web/Base SDK version dependency to 2.7.0-beta2

## Version 2.4.0-beta1
- NETCORE: Updated Web/Base SDK version dependency to 2.7.0-beta1
- NETCORE: Enables Performance Counters for Asp.Net Core Apps running in Azure Web Apps. (https://github.com/Microsoft/ApplicationInsights-dotnet-server/issues/889)
- NETCORE: Added null check on ContentRootPath of the hostingenvironment. (https://github.com/Microsoft/ApplicationInsights-aspnetcore/issues/696)

## Version 2.3.0
- NETCORE: [Fix a bug which caused Requests to fail when Hostname was empty.] (https://github.com/Microsoft/ApplicationInsights-aspnetcore/issues/278)
- NETCORE: [Fix reading of instrumentation key from appsettings.json file when using AddApplicationInsightsTelemetry() extension to add ApplicationInsights ](https://github.com/Microsoft/ApplicationInsights-aspnetcore/issues/605)
- NETCORE: [Bring back DomainNameRoleInstanceTelemetryInitializer without which NodeName and RoleInstance will be empty in Ubuntu](https://github.com/Microsoft/ApplicationInsights-aspnetcore/issues/671)
- NETCORE: [RequestTelemetry is no longer populated with HttpMethod which is obsolete.](https://github.com/Microsoft/ApplicationInsights-aspnetcore/issues/675)
- NETCORE: Fixed a bug which caused AutoCollectedMetricExtractor flag to be always true.
- NETCORE: Updated Web/Base SDK version dependency to 2.6.4

## Version 2.3.0-beta2
- NETCORE: [Update System.Net.Http version referred to 4.3.2 as older version has known security vulnerability. ](https://github.com/Microsoft/ApplicationInsights-aspnetcore/issues/666)
- NETCORE: [Added ApplicationInsightsServiceOptions flag to turn off AutoCollectedMetricExtractor. ](https://github.com/Microsoft/ApplicationInsights-aspnetcore/issues/664)
- NETCORE: [Added two AdaptiveSamplingTelemetryProcessors one for Event and one for non Event types to be consistent with default Web SDK behaviour. ](https://github.com/Microsoft/ApplicationInsights-aspnetcore/issues/663)
- NETCORE: [RequestCollection is refactored to be implemented as an ITelemetryModule. This makes it possible to configure it like every other auto-collection modules. ](https://github.com/Microsoft/ApplicationInsights-aspnetcore/issues/650)
- NETCORE: [Fixed race condition on dispose to close #651](https://github.com/Microsoft/ApplicationInsights-aspnetcore/pull/652)
- NETCORE: Removed DomainNameRoleInstanceTelemetryInitializer as it is deprecated.
- NETCORE: Reuse AzureWebAppRoleEnvironmentTelemetryInitializer from WindowsServer repo instead of outdated implementation in this repo.
- NETCORE: Updated Web/Base SDK version dependency to 2.6.0-beta4

## Version 2.3.0-beta1
- NETCORE: Changed behavior for `TelemetryConfiguration.Active` and `TelemetryConfiguration` dependency injection singleton: with this version every WebHost has its own `TelemetryConfiguration` instance. Changes done for `TelemetryConfiguration.Active` do not affect telemetry reported by the SDK; use `TelemetryConfiguration` instance obtained through the dependency injection. [Fix NullReferenceException when sending http requests in scenario with multiple web hosts sharing the same process](https://github.com/Microsoft/ApplicationInsights-dotnet/issues/613)
- NETCORE: Updated Javascript Snippet with latest from [Github/ApplicationInsights-JS](https://github.com/Microsoft/ApplicationInsights-JS)
- NETCORE: [Make all built-in TelemetryInitializers public to allow easy removal from DI Container.](https://github.com/Microsoft/ApplicationInsights-aspnetcore/issues/351)
- NETCORE: [Enforced limits of values read from incoming http requests to prevent security vulnerability](https://github.com/Microsoft/ApplicationInsights-aspnetcore/pull/608)
- NETCORE: [ApplicationInsightsLogger adds EventId into telemetry properties. It is off by default for compatibility. It can be switched on by configuring ApplicationInsightsLoggerOptions.](https://github.com/Microsoft/ApplicationInsights-aspnetcore/issues/569)
- NETCORE: [ApplicationInsightsLogger logs exceptions as ExceptionTelemetry by default. This can now be configured with ApplicationInsightsLoggerOptions.TrackExceptionsAsExceptionTelemetry] (https://github.com/Microsoft/ApplicationInsights-aspnetcore/pull/574)
- NETCORE: [Add App Services and Azure Instance Metedata heartbeat provider modules by default, allow user to disable via configuration object.](https://github.com/Microsoft/ApplicationInsights-aspnetcore/pull/627)
- NETCORE: [Added extension method to allow configuration of any Telemetry Module.](https://github.com/Microsoft/ApplicationInsights-aspnetcore/issues/634)
- NETCORE: [Added ability to remove any default Telemetry Module.](https://github.com/Microsoft/ApplicationInsights-aspnetcore/issues/633)
- NETCORE: [TelemetryChannel is configured via DI, making it easier to override channel](https://github.com/Microsoft/ApplicationInsights-aspnetcore/issues/641)
- NETCORE: [Fixed a bug which caused QuickPulse and Sampling to be enabled only if ServerTelemetryChannel was used](https://github.com/Microsoft/ApplicationInsights-aspnetcore/issues/642)
- NETCORE: [QuickPulseTelemetryModule is constructed via DI, make it possible for users to configure it.] (https://github.com/Microsoft/ApplicationInsights-aspnetcore/issues/639)
- NETCORE: [Remove CorrelationIdLookupHelper. Use TelemetryConfiguration.ApplicationIdProvider instead.](https://github.com/Microsoft/ApplicationInsights-aspnetcore/pull/636) With this change you can update URL to query application ID from which enables environments with reverse proxy configuration to access Application Insights ednpoints.
- NETCORE: [AutocollectedMetricsExtractor is added by default to the TelemetryConfiguration](https://github.com/Microsoft/ApplicationInsights-aspnetcore/issues/604)
- NETCORE: Updated Web/Base SDK version dependency to 2.6.0-beta3

## Version 2.2.1
- NETCORE: Updated Web/Base SDK version dependency to 2.5.1 which addresses a bug.

## Version 2.2.0
- NETCORE: Updated Web/Base SDK version dependency to 2.5.0

## Version 2.2.0-beta3
- NETCORE: Updated Web/Base SDK version dependency to 2.5.0-beta2.
- NETCORE: This version of Base SDK referred contains fix to a bug in ServerTelemetryChannel which caused application to crash on non-windows platforms. Details on fix and workaround(https://github.com/Microsoft/ApplicationInsights-dotnet/issues/654) Original issue (https://github.com/Microsoft/ApplicationInsights-aspnetcore/issues/551)

## Version 2.2.0-beta2
- NETCORE: Same bits as beta1. Only change is that the symbols for the binaries are indexed in Microsoft symbol servers. Beta1 symbols will not be available.

## Version 2.2.0-beta1

- NETCORE: Project is upgraded to work with Visual Studio 2017. Also projects are modified to use csproj instead of project.json.
- NETCORE: Adaptive sampling enabled for both - full framework and .NET Core applications.
- NETCORE: ServerTelemetryChannel is enabled and set as default channel for both - full framework and .NET Core applications.
- NETCORE: Live metrics collection is enabled by default for .NET Core applications (was already enabled for full .NET applications).
- NETCORE: Updated Web/Base SDK version dependency to 2.5.0-beta1.
- NETCORE: DependencyCollector referred from 2.5.0-beta1 supports collecting SQL dependency calls in .NET Core Applications using EntityFramework.

## Version 2.1.1

- NETCORE: [Address the issue where DependencyCollection breaks Azure Storage Emulator calls](https://github.com/Microsoft/ApplicationInsights-aspnetcore/issues/488)
- NETCORE: [Support setting request operation name based on executing Razor Page](https://github.com/Microsoft/ApplicationInsights-aspnetcore/issues/430)
- NETCORE: [Fixed ITelemetryProcessor dependency injection failure when using 3rd party IoC Container](https://github.com/Microsoft/ApplicationInsights-aspnetcore/issues/482)
- NETCORE: [Logging exceptions when using ILogger if an exception is present](https://github.com/Microsoft/ApplicationInsights-aspnetcore/issues/393)
- NETCORE: [Syncronize access to HttpContext properties](https://github.com/Microsoft/ApplicationInsights-aspnetcore/issues/373)
- NETCORE: Updated SDK version dependency to 2.4.1 for DependencyCollector.

## Version 2.1.0

- NETCORE: Updated SDK version dependency to 2.4.0.
- NETCORE: Fixed a minor logging message issue.
- NETCORE: Fixed unit test reliability issues.

## Version 2.1.0-beta6

- NETCORE: Updated SDK version dependency to 2.4.0-beta5.

## Version 2.1.0-beta5

- NETCORE: Added support for adding telemetry processors through dependency injection; see #344, #445, #447
- NETCORE: [Added support for environment specifc appsettings under default configuration](https://github.com/Microsoft/ApplicationInsights-aspnetcore/issues/449)
- NETCORE: Updated SDK version dependency to 2.4.0-beta4.

## Version 2.1.0-beta4

- NETCORE: [Made package meta-data URLs use HTTPS](https://github.com/Microsoft/ApplicationInsights-aspnetcore/issues/390)
- NETCORE: Updated SDK version dependency to 2.4.0-beta3.

## Version 2.1.0-beta3

- NETCORE: [Removed the use of Platform Abstractions](https://github.com/Microsoft/ApplicationInsights-aspnetcore/issues/410)
- NETCORE: [Correlation header injection disabled for standard Azure storage calls](https://github.com/Microsoft/ApplicationInsights-aspnetcore/issues/416)
- NETCORE: [Made UseApplicationInsights and AddApplicationInsightsTelemetry calls idempotent](https://github.com/Microsoft/ApplicationInsights-aspnetcore/pull/419)

## Version 2.1.0-beta2

- NETCORE: Updated to use the new correlation headers and changed the correlationId to use appId instead of hashed ikey to match other SDKs.
- NETCORE: Fixed null reference exception for unitialized ILogger.
- NETCORE: Unit test bug fixes.
- NETCORE: Upgraded NETStandard.Library dependency to 1.6.1.
- NETCORE: Updated to reference base SDK 2.4.0-beta2.
- NETCORE: Included Microsoft.ApplicationInsights.DependencyCollector for .NET Core.

## Version 2.1.0-beta1

- NETCORE: Bug fixes
- NETCORE: Removed UserAgentTelemetryInitializer and associated tests.
- NETCORE: Added instrumentation key header
- NETCORE: [Added OperationCorrelationTelemetryInitializer](https://github.com/Microsoft/ApplicationInsights-aspnetcore/issues/333)
- NETCORE: [Set Id instead of OperationId for request dependency correlation](https://github.com/Microsoft/ApplicationInsights-aspnetcore/issues/340)
- NETCORE: [Set Id in thread-safe location](https://github.com/Microsoft/ApplicationInsights-aspnetcore/issues/342)
- NETCORE: Updated SDK version dependency to 2.3.0-beta3.

## Version 2.0.0

- NETCORE: Added a configuration overload for AddApplicationInsightsTelemetry.
- NETCORE: Updated test projects to reference .NET Core 1.1.0.
- NETCORE: [Fixed debug trace logging issue](https://github.com/Microsoft/ApplicationInsights-aspnetcore/issues/315)
- NETCORE: [Stopped logging extra debug traces to AI](https://github.com/Microsoft/ApplicationInsights-aspnetcore/issues/314)
- NETCORE: [JS snippet is empty if telemetry is disabled](https://github.com/Microsoft/ApplicationInsights-aspnetcore/issues/313)
- NETCORE: [Added an initializer to provide the environment name as a custom property](https://github.com/Microsoft/ApplicationInsights-aspnetcore/issues/312)
- NETCORE: [Added an option to emit JS to track authenticated users](https://github.com/Microsoft/ApplicationInsights-aspnetcore/issues/311)
- NETCORE: Minor bug fixes and cleanup.

## Version 2.0.0-beta1

- NETCORE: This release contains a rewrite of the SDK internals for better .NET Core integration and initialization.
- NETCORE: The methods UseApplicationInsightsRequestTelemetry and UseApplicationInsightsExceptionTelemetry are obsolete, the work those methods did is handled automatically internally now.  You can just delete any existing references to them from Startup.cs.
- NETCORE: The MVC dependency for the JavaScript snippet has been removed so in order to include the JavaScript snippet now you need to insert the following lines at the very top of the _Layout.cshtml file:
```cshtml
    @using Microsoft.ApplicationInsights.AspNetCore
    @inject JavaScriptSnippet snippet
```
- NETCORE: and insert the following line before the closing `</head>` tag:
```cshtml
    @Html.Raw(snippet.FullScript)
```

## Version 1.0.3-beta1

- NETCORE: New ```AzureWebAppRoleEnvironmentTelemetryInitializer``` telemetry initializer that populates role name and role instance name for Azure Web Apps.

## Version 1.0.2

- NETCORE: Marked code analysis packages as only for build and not NuGet package dependencies.

## Version 1.0.1

- NETCORE: Added code analysis packages.
- NETCORE: Updated JavaScript snippet.
- NETCORE: Updated project link and added privacy statement link.
- NETCORE: Added culture to string operations.
- NETCORE: Switched TelemetryClient service registration to Singleton.
- NETCORE: Added after build target to patch XML doc files with language attribute.
- NETCORE: Updated .NET Core references to 1.0.1.

## Version 1.0.0

- NETCORE: [Stable 1.0.0 release](http://www.nuget.org/packages/Microsoft.ApplicationInsights.AspNetCore/1.0.0).
- NETCORE: Supports .NET framework and [.NET Core](https://www.microsoft.com/net/core).

Features:
- NETCORE: request tracking
- NETCORE: exception tracking
- NETCORE: diagnostic tracing
- NETCORE: dependency collection (.NET framework only)
- NETCORE: performance counter collection (.NET framework only)
- NETCORE: adaptive sampling (.NET framework only)
- NETCORE: telemetry processors (.NET framework only)
- NETCORE: metrics stream (.NET framework only)

Depends on:
- NETCORE: [Application Insights Core 2.1.0 SDK](http://www.nuget.org/packages/Microsoft.ApplicationInsights/2.1.0)
- NETCORE: [AI Dependency Collector](http://www.nuget.org/packages/Microsoft.ApplicationInsights.DependencyCollector/2.1.0) (.NET framework only)
- NETCORE: [AI Performance Counter Collector](http://www.nuget.org/packages/Microsoft.ApplicationInsights.PerfCounterCollector/2.1.0) (.NET framework only)
- NETCORE: [AI Windows Server Telemetry Channel](http://www.nuget.org/packages/Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel/2.1.0) (.NET framework only)

## Version 1.0.0-rc2-final

All the changes from [1.0.0-rc1-update4](https://github.com/Microsoft/ApplicationInsights-aspnetcore/releases/tag/v1.0.0-rc1-update4), including the following updates:
- NETCORE: Renaming: Microsoft.ApplicationInsights.AspNet is changed to Microsoft.ApplicationInsights.AspNetCore
- NETCORE: Runtime: Supports .NET Core CLI runtime. Does not support DNX runtime and the associated RC1 bits.
- NETCORE: Supports ASP.NET Core on .NET Core and the .NET Framework
- NETCORE: Dependencies are updated to the latest RC2 bits.
- NETCORE: Metrics Stream functionality is enabled by default in .NET Framework
- NETCORE: Install from [https://www.nuget.org/packages/Microsoft.ApplicationInsights.AspNetCore](https://www.nuget.org/packages/Microsoft.ApplicationInsights.AspNetCore)

## Version 1.0.0-rc1-update4

- NETCORE: Windows Server Telemetry Channel is enabled in full framework to send telemetry, and it uses Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel, version: 2.1.0-beta3
- NETCORE: Diagnostic tracing using EventSource is enabled
- NETCORE: TelemetryConfiguration.Active is used as the default telemetry configuration
- NETCORE: Adaptive Sampling by default is enabled in full framework
- NETCORE: Using telemetry processors is enabled in full framework
- NETCORE: ApplicationInsightsServiceOptions is available to configure default adaptive sampling behavior
- NETCORE: ComponentVersionTelemetryInitializer is added, that reads the application version from project.json and assigns it to telemetry.Context.Component.Version
- NETCORE: All Microsoft.ApplicationInsights.* dependencies are updated to the latest version (2.1.0-beta3)

## Version 1.0.0-rc1-update3

- NETCORE: Update Application Insights Core dependency (Microsoft.ApplicationInsights) to the latest stable version (2.0.0).

## Version 1.0.0-rc1-update2

- NETCORE: Fix the dependencies of previously published NuGet package (v1.0.0-rc1-update1)

## Version 1.0.0-rc1-update1

- NETCORE: Support the latest version of Application Insights core sdk (2.0.0-beta4 or greater)
- NETCORE: Support dependency and performance counter collection in full framework (dnx 4.5.1)

## Version 1.0.0-rc1

- NETCORE: Support ASP.Net 5 RC1 release.
- NETCORE: Binaries are now strong name signed.

## Version 1.0.0-beta8

- NETCORE: Support Asp.Net 5 beta8

## Version 1.0.0-beta7

- NETCORE: Support ASP.Net5 Beta7
- NETCORE: Minor bug fixes

## Version 1.0.0-beta6

- NETCORE: Support ASP.Net 5 Beta6
- NETCORE: Updates to build infrastructure
- NETCORE: Switch to 1.1 version of Microsoft.ApplicationInsights API

## Version 1.0.0-beta5

- NETCORE: Support ASP.Net 5 Beta5
- NETCORE: Minor bug fixes
- NETCORE: Switch to 0.17 version of Microsoft.ApplicationInsights API

## Version 0.32.0-beta4

- NETCORE: Support dnxcore50 applications
- NETCORE: Change integration points with Visual Studio
- NETCORE: Minor bug fixes
- NETCORE: Switch to 0.16 version of Microsoft.ApplicationInsights API

## Version 0.31.0-beta4

- NETCORE: Fixed references to ASP.NET runtime packages.

## Version 0.30.0.1-beta

- NETCORE: Preview version of Application Insights. Supports only full framework. Will compile for core framework, but no events will be sent.

