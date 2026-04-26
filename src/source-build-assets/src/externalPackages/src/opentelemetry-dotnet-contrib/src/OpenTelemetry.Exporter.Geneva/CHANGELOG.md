# Changelog

## Unreleased

* Add ResourceFieldNames to filter resource attributes to send to Geneva
  ([#3552](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3552),
   [#3646](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3646))

* Updated OpenTelemetry core component version(s) to `1.15.0`.
  ([#3721](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3721))

## 1.14.0

Released 2025-Nov-13

* Add support for .NET 10.0.
  ([#2822](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2822))

* Update .NET 10.0 NuGet package versions from `10.0.0-rc.2.25502.107` to `10.0.0`.
  ([#3403](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3403))

* Updated OpenTelemetry core component version(s) to `1.14.0`.
  ([#3403](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3403))

## 1.13.1

Released 2025-Oct-29

* Support for specifying resource attributes, including
  `service.name`, `service.instanceId`, and custom attributes.
  ([#3214](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3214))

* Allow custom string size limit in custom fields.
  ([#3360](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3360))

## 1.13.0

Released 2025-Oct-13

* Updated OpenTelemetry core component version(s) to `1.13.1`.
  ([#3218](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3218))

## 1.13.0-alpha.1

Released 2025-Jun-07

* **Experimental**: Added an option for configuring a custom string size limit in
  the MessagePack serializer. The maximum string length, in characters, can be
  set using the `PrivatePreviewLogMessagePackStringSizeLimit=<CharCount>`
  connection string parameter.
  ([#2813](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2813))

* Add httpUrl for HTTP server spans mapped from multiple attributes.
  ([#2818](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2818))

## 1.12.0

Released 2025-May-06

* Updated OpenTelemetry core component version(s) to `1.12.0`.
  ([#2725](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2725))

## 1.11.3

Released 2025-Apr-22

* Fixed an issue where accessing an unset `AFDCorrelationId` in `RuntimeContext`
  would throw unhandled exceptions.
  ([#2708](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2708))

## 1.11.2

Released 2025-Apr-16

* Added support for enriching logs with `AFDCorrelationId` when present in
  `RuntimeContext`. This can be enabled via the
  `PrivatePreviewEnableAFDCorrelationIdEnrichment=true` connection string
  parameter.
  ([#2698](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2698))

## 1.11.1

Released 2025-Mar-05

* Updated OpenTelemetry core component version(s) to `1.11.2`.
  ([#2582](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2582))

## 1.11.0

Released 2025-Feb-03

* Added support for exporting exception stack traces using
  `Exception.StackTrace`. This can be enabled via the
  `ExceptionStackExportMode.ExportAsStackTraceString` enum. Applicable only to
  the LogExporter.
  ([#2422](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2422))

* Updated OpenTelemetry core component version(s) to `1.11.1`.
  ([#2477](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2477))

## 1.10.0

Released 2024-Nov-18

* Drop support for .NET 6 as this target is no longer supported.
  ([#2117](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2117))

* Added support for exporting metrics on Linux when OTLP protobuf encoding is
  enabled via the `PrivatePreviewEnableOtlpProtobufEncoding=true` connection
  string switch. `PrivatePreviewEnableOtlpProtobufEncoding=true` is now
  supported on both Windows and Linux.

  * `user_events` transport:
    [#2113](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2113).

  * Unix domain socket transport:
    [#2261](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2261).

  For configuration details see:
  [OtlpProtobufEncoding](./README.md#otlpprotobufencoding).

* Update OpenTelemetry SDK version to `1.10.0`.
  ([#2317](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2317))

## 1.9.0

Released 2024-Jun-21

* Exemplars are now supported as a stable feature. Please note that
  OpenTelemetry SDK has Exemplars disabled by default. Check [OpenTelemetry
  Metrics
  docs](https://github.com/open-telemetry/opentelemetry-dotnet/tree/main/docs/metrics/customizing-the-sdk#exemplars)
  to learn how to enable them.

## 1.9.0-rc.2

Released 2024-Jun-17

* Update GenevaTraceExporter to export `activity.TraceStateString` as the value
  for Part B `traceState` field for Spans when the `IncludeTraceStateForSpan`
  option is set to `true`. This is an opt-in feature and the default value is `false`.
  Note that this is for Spans only and not for LogRecord.
  ([#1850](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1850))

* Updated OpenTelemetry core component version(s) to `1.9.0`.
  ([#1888](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1888))

## 1.9.0-rc.1

Released 2024-Jun-12

* Update OpenTelemetry SDK version to `1.9.0-rc.1`.
  ([#1869](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1869))

* Added `LoggerProviderBuilder.AddGenevaLogExporter` registration extensions.
  Added `TracerProviderBuilder.AddGenevaTraceExporter()` registration extension.
  ([#1880](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1880))

## 1.9.0-alpha.1

Released 2024-May-22

* Update OpenTelemetry SDK version to `1.9.0-alpha.1`.
  ([#1834](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1834))

## 1.8.0

Released 2024-May-15

* Update OpenTelemetry SDK version to `1.8.1`.
  ([#1798](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1798))

## 1.8.0-rc.2

Released 2024-May-13

* **Experimental (pre-release builds only)**: Add support for exporting
  exemplars when OTLP protobuf encoding is enabled via
  `PrivatePreviewEnableOtlpProtobufEncoding=true` in the connection string.
  ([#1703](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1703))

* Add support for exporting
  exponential histograms when OTLP protobuf encoding is enabled via
  `PrivatePreviewEnableOtlpProtobufEncoding=true` in the connection string.
  ([#1705](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1705))

## 1.8.0-rc.1

Released 2024-May-02

* Update OpenTelemetry SDK version to `1.8.0-rc.1`.
  ([#1689](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1689))

## 1.8.0-beta.1

**(This version has been unlisted due to incorrect dependency on stable sdk
version 1.8.1 that prevents ability to use exemplars)**

Released 2024-Apr-23

* Fix a bug in `GenevaMetricExporter` where the `MetricEtwDataTransport` singleton
  is disposed.
  ([#1537](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1537))

* Update OpenTelemetry SDK version to `1.8.1`.
  ([#1668](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1668))

* Add OTLP protobuf encoding support for metric exporter in Windows
  environment. Use `PrivatePreviewEnableOtlpProtobufEncoding=true` in the
  connection string to opt-in.
  ([#1596](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1596),
   [#1626](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1626),
   [#1629](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1629),
   [#1634](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1634))

* Native AOT compatibility.
  ([#1666](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1666))

## 1.7.0

Released 2023-Dec-11

* Update OpenTelemetry SDK version to `1.7.0`.
  ([#1486](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1486))

## 1.7.0-rc.1

Released 2023-Dec-05

* Update Part B mapping to add Http related tags based on the new Semantic
  Conventions.
  ([#1402](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1402))

* Fix serialization bug in `TldTraceExporter` and `TldLogExporter` when there
  are no Part C fields.
  ([#1396](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1396))

* Fix a serialization issue for `TldTraceExporter` and `TldLogExporter` when any
  non-primitive types have to be serialized for `env_properties`.
  ([#1424](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1424))

* Add `net8.0` target.
  ([#1329](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1329))

* Update OpenTelemetry SDK version to `1.7.0-rc.1`.
  ([#1329](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1329))

## 1.7.0-alpha.1

Released 2023-Sep-22

* Allow overriding the Part B "name" field value in GenevaLogExporter.
  ([#1367](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1367))

## 1.6.0

Released 2023-Sep-09

* Update OpenTelemetry SDK version to `1.6.0`.
  ([#1346](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1346))

## 1.6.0-rc.1

Released 2023-Aug-28

* Update OpenTelemetry SDK version to `1.6.0-rc.1`.
  ([#1329](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1329))

## 1.6.0-alpha.1

Released 2023-Jul-12

* Update OpenTelemetry SDK version to `1.6.0-alpha.1`.
  ([#1264](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1264))

* Add back support for exporting `Exemplar`.
  ([#1264](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1264))

## 1.5.1

Released 2023-Jun-29

* Update OpenTelemetry SDK version to `1.5.1`.
  ([#1255](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1255))

## 1.5.0

Released 2023-Jun-14

* **Important Note:** Starting `1.5.0` version, `GenevaExporter` uses a newer
  format for exporting metrics. Please use `>= v2.2.2023.316.006` version of the
  MetricsExtension if you are using the metric exporter.

* Update OpenTelemetry SDK version to `1.5.0`.
  ([#1238](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1238))

* Removed support for exporting `Exemplars`. This would be added back in the
  `1.6.*` prerelease versions right after `1.5.0` stable version is released.
  ([#1238](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1238))

* Add named options support for `GenevaTraceExporter` and
  `GenevaMetricExporter`.
  ([#1218](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1218))

* Add a new overload for `AddGenevaMetricExporter` without any parameters to
  avoid warning
  [RS0026](https://github.com/dotnet/roslyn-analyzers/blob/main/src/PublicApiAnalyzers/Microsoft.CodeAnalysis.PublicApiAnalyzers.md#rs0026-do-not-add-multiple-public-overloads-with-optional-parameters).
  ([#1218](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1218))

* Fix the issue of running into the `ArgumentException`: `An instance of
  EventSource with Guid edc24920-e004-40f6-a8e1-0e6e48f39d84 already exists.`
  when using multiple instances of `GenevaMetricExporter`.
  ([#1225](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1225))

## 1.5.0-rc.1

Released 2023-Jun-05

* Fix an issue with getting sanitized category name in pass-through table name
  mapping cases for `TldLogExporter`.
  ([#1175](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1175))

* TldLogExporter to export `SpanId` value in `ext_dt_spanId` field instead of
  `TraceId` value.
  ([#1184](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1184))

* Add support for abstract domain sockets.
  ([#1199](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1199))

* Update OpenTelemetry SDK version to `1.5.0-rc.1`.
  ([#1210](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1210))

## 1.5.0-alpha.3

Released 2023-Apr-19

* TldLogExporter to export `eventId.Id` as a Part B field instead of Part C
  field.
  ([#1134](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1134))

* Update GenevaLogExporter to export `eventId.Name` as the value for Part A
  `name` field when the `EventNameExportMode` option is set to
  `ExportAsPartAName`.
  ([#1135](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1135))

## 1.4.1

Released 2023-Mar-29

* Relaxed table name mapping validation rules to restore the previous behavior
  from version 1.3.0.
  ([#1120](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1120))

## 1.5.0-alpha.2

Released 2023-Mar-29

* Fix a bug where metrics without exemplars were not getting exported.
  ([#1099](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1099))

* Relaxed table name mapping validation rules to restore the previous behavior
  from version 1.3.0.
  ([#1109](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1109))

* Add support for exporting metrics to more than a single account/namespace
  combination using a single GenevaMetricExporter instance. Users can now export
  individual metric streams to:
  * An account of their choice by adding the dimension
    `_microsoft_metrics_account` and providing a `string` value for it as the
    account name.
  * A metric namespace of their choice by adding the dimension
    `_microsoft_metrics_namespace` and providing a `string` value for it as the
    namespace name.
  ([#1111](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1111))

* Fix a bug in TldTraceExporter for incorrect serialization of special tags.
  ([#1115](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1115))

## 1.5.0-alpha.1

Released 2023-Mar-13

* Changed the behavior of Unix domain socket connection at startup. Before this
  change, the exporter initialization would throw exception if the target Unix
  Domain Socket does not exist. After this change, the exporter initialization
  would return success and the exporting background thread will try to establish
  the connection.
  ([#935](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/935))

* Update OpenTelemetry SDK version to `1.5.0-alpha.1`.

* Update GenevaMetricExporter to use TLV format serialization.

* Add support for exporting exemplars.
  ([#1069](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1069))

## 1.4.0

Released 2023-Feb-27

* Update OpenTelemetry to 1.4.0
  ([#1038](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1038))

* Add `DisableMetricNameValidation` connection string flag for controlling
  metric name validation performed by the OpenTelemetry SDK.
  ([#1006](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1006))

## 1.4.0-rc.4

Released 2023-Feb-13

* Update OpenTelemetry to 1.4.0-rc.4
  ([#990](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/990))

## 1.4.0-rc.3

Released 2023-Feb-08

* Update OpenTelemetry to 1.4.0-rc.3
  ([#944](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/944))

## 1.4.0-rc.2

Released 2023-Jan-30

* Update OpenTelemetry to 1.4.0-rc.2
  ([#880](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/880))

* Add TldTraceExporter and TldLogExporter. Use
  `"PrivatePreviewEnableTraceLoggingDynamic=true"` in the connection string to
  use these exporters.
  ([#662](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/662))
  ([#874](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/874))

* Add support for configuring BatchActivityExportProcessor parameters (via
  environment variables) used by GenevaTraceExporter in Linux.
  ([#925](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/925))

## 1.4.0-rc.1

Released 2022-Dec-19

* Update OpenTelemetry to 1.4.0-rc.1
  ([#820](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/820))

* Add support in logs for prefix-based table name mapping configuration.
  [#796](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/796)

* Updated the trace exporter to use the new performance APIs introduced in
  `System.Diagnostics.DiagnosticSource` v7.0.
  [#838](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/838)

* Avoid allocation when serializing scopes.
  ([#818](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/818))

## 1.4.0-beta.6

Released 2022-Dec-09

* Added support for
  [DateTimeOffset](https://learn.microsoft.com/dotnet/api/system.datetimeoffset).
  ([#797](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/797))

* Fix the overflow bucket value serialization for Histogram.
  ([#805](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/805))

* Fix EventSource logging.
  ([#813](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/813))

* Update `MessagePackSerializer` to use
  [BinaryPrimitives](https://learn.microsoft.com/dotnet/api/system.buffers.binary.binaryprimitives)
  to serialize scalar types more efficiently by avoiding repeated bound checks.
  Add support for serializing `ISpanFormattable` types.
  ([#803](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/803))

## 1.3.1

Released 2022-Dec-07

* Fix the overflow bucket value serialization for Histogram.
  ([#807](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/807))

## 1.4.0-beta.5

Released 2022-Nov-21

* Update OpenTelemetry to 1.4.0-beta.3
  ([#774](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/774))

## 1.4.0-beta.4

Released 2022-Oct-28

* Updated export logic for scopes
  * Users upgrading from `1.4.0-beta.1`, `1.4.0-beta.2` or `1.4.0-beta.3` to
    this version will see a **breaking change**
  * Export scopes which have a non-null key as individual columns (each
    key-value pair from the scopes is exported as its own column; these columns
    would also be taken into consideration when the CustomFields option is
    applied).
  * When using formatted strings for scopes, the templated string
    (`"{OriginalFormat"}`) will not be exported.
  [#736](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/736)

## 1.4.0-beta.3

Released 2022-Oct-20

* Add support for exporting `UpDownCounter` and `ObservableUpDownCounter`.
  [#685](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/685)

* Export `MetricType.LongGauge` as a double metric as it might return negative
  values.
  [#721](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/721)

* Add support for exporting exception stack.
  [#672](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/672)

* Change the default MetricExportInterval from 20 seconds to 60 seconds.
  [#722](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/722)

## 1.4.0-beta.2

Released 2022-Oct-17

* The option `TableNameMappings` of `GenevaExporterOptions` will not support
  string values that are null, empty, or consist only of white-space characters.
  It will also not support string values that contain non-ASCII characters.
  [646](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/646)

* Update OpenTelemetry SDK version to `1.4.0-beta.2`. Add support for exporting
  Histogram Min and Max. If the histogram does not contain min and max,
  the exporter exports both the values as zero.
  [#704](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/704)

## 1.4.0-beta.1

Released 2022-Aug-01

## 1.3.1-alpha.0.5 (unlisted due to incorrect version)

Released 2022-Jul-29

* Add support for exporting `ILogger` scopes.
[545](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/545)

## 1.3.0

Released 2022-Jul-28

* Supports `OpenTelemetry.Extensions.Hosting` based configuration for
`GenevaMetricExporter`.
[397](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/397)

* Update OpenTelemetry SDK version to `1.3.0`.
[427](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/427)

* Remove support for .NET Framework 4.6.1. The minimum .NET Framework version
supported now is .NET 4.6.2.
[441](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/441)

* Fix the incorrect `ExportResult` issue on Linux:
[422](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/issues/422)
by throwing any exception caught by `UnixDomainSocketDataTransport.Send` so that
`Export` methods cn catch it and correctly set `ExportResult` to
`ExportResult.Failure`.
[444](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/444)

* The option `PrepopulatedFields` of `GenevaExporterOptions` will only support
values of type: `bool`, `byte`, `sbyte`, `short`, `ushort`, `int`, `uint`,
`long`, `ulong`, `float`, `double`, and `string`. It will also not accept `null`
values.
[514](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/514)
[537](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/537)

* The option `MetricExportIntervalMilliseconds` of `GenevaMetricExporterOptions`
will not accept a value less than 1000.
[527](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/527)

* Remove support for exporting `ILogger` scopes that was added in `1.3.0-beta.2`
version.
[541](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/541)

## 1.3.0-beta.2

Released 2022-Jun-03

* Add support for exporting `ILogger` scopes.
[390](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/390)

## 1.3.0-beta.1

Released 2022-May-27

* Enable PassThru TableNameMappings using the logger category name.
[345](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/345)

* Throw exception when `TableNameMappings` contains a `null` value.
[322](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/322)

* TraceExporter bug fix to not export non-recorded Activities.
[352](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/352)

* Add support for the native `Activity` properties `Status` and
`StatusDescription`.
[359](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/359)

* Allow serialization of non-ASCII characters for
`LogRecord.Exception.GetType().FullName`.
[375](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/375)

## 1.2.6

Released 2022-Apr-21

* Set GenevaMetricExporter temporality preference back to Delta.
[323](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/323)

## 1.2.5 Broken

Released 2022-Apr-20

Note: This release was broken due to the GenevaMetricExporter using a
TemporalityPreference of Cumulative instead of Delta, it has been unlisted from
NuGet.
[303](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/303)
is the PR that introduced this bug to GenevaMetricExporterExtensions.cs

* Update OpenTelemetry SDK version to `1.2.0`.
[319](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/319)

## 1.2.4 Broken

Released 2022-Apr-20

This is the first release of the `OpenTelemetry.Exporter.Geneva` project. Note:
This release was broken due to using OpenTelemetry 1.2.0-rc5. Therefore, it has
been unlisted on NuGet.

* LogExporter modified to stop calling `ToString()` on `LogRecord.State` to
obtain Log body. It now obtains body from `LogRecord.FormattedMessage` or
special casing "{OriginalFormat}" only.
[295](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/295)

* Fixed a bug which causes LogExporter to not serialize if the `LogRecord.State`
had a single KeyValuePair.
[295](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/295)

* Update OpenTelemetry SDK version to `1.2.0-rc5`.
[308](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/308)
