# Changelog

## Unreleased

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

## 1.13.0

Released 2025-Oct-24

* Updated OpenTelemetry core component version(s) to `1.13.1`.
  ([#3218](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3218))

## 1.11.1

Released 2025-Mar-05

* Updated OpenTelemetry core component version(s) to `1.11.2`.
  ([#2582](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2582))

## 1.11.0

Released 2025-Feb-11

## 1.11.0-beta.1

Released 2025-Jan-29

* Updated OpenTelemetry core component version(s) to `1.11.1`.
  ([#2477](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2477))

* Event full name validation can now be bypassed by supplying `{EventFullName}`
  as the first attribute on logs.
  ([#2529](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2529))

## 1.10.0

Released 2024-Nov-18

* Drop support for .NET 6 as this target is no longer supported.
  ([#2123](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2123))

* Bumped the `System.Text.Json` reference to `6.0.10` for runtimes older than
  `net8.0` and added a direct reference for `System.Text.Json` at `8.0.5` on
  `net8.0` in response to
  [CVE-2024-43485](https://msrc.microsoft.com/update-guide/vulnerability/CVE-2024-43485).
  ([#2196](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2196))

* Fixed a bug causing extension data specified on `LogRecord`s in a batch to
  also be applied to subsequent `LogRecord`s in the same batch.
  ([#2205](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2205))

* Update OpenTelemetry SDK version to `1.10.0` and removed the direct reference
  to `Microsoft.Extensions.Configuration.Binder`.
  ([#2295](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2295),
  [#2317](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2317))

## 1.9.3

Released 2024-Oct-11

* Fixed a bug causing extension data specified on `LogRecord`s in a batch to
  also be applied to subsequent `LogRecord`s in the same batch.
  ([#2208](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2208))

* Bumped the `System.Text.Json` reference to `6.0.10` for the `net462`,
  `netstandard2.0`, and `netstandard2.1` targets in response to
  [CVE-2024-43485](https://github.com/advisories/GHSA-8g4q-xg66-9fp4).
  ([#2208](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2208))

## 1.10.0-alpha.1

Released 2024-Sep-06

* Dropped support for the `net7.0` target because .NET 7 is no longer supported.
  Added a `net8.0` target.
  ([#2038](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/issues/2038))

* Added `SetEventFullNameMappings` API on
  `OneCollectorLogExportProcessorBuilder` which can be used to change the event
  full name sent to the OneCollector service for a given `LogRecord`
  (`CategoryName` and `EventId.Name` are used to derive the event full name by
  default).
  ([#2021](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2021))

## 1.9.2

Released 2024-Aug-12

* Fixed `PlatformNotSupportedException`s being thrown during export when running
  on mobile platforms which caused telemetry to be dropped silently.
  ([#1992](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1992))

* Fixed a bug which caused remaining records in a batch to be dropped silently
  once the max payload size for a transmission (default 4 KiB) has been
  reached.
  ([#1999](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1999))

## 1.9.1

Released 2024-Aug-01

* Fixed a bug preventing `HttpTransportErrorResponseReceived` events from firing
  on .NET Framework.
  ([#1987](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1987))

## 1.9.0

Released 2024-Jun-17

* Updated OpenTelemetry core component version(s) to `1.9.0`.
  ([#1888](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1888))

## 1.9.0-rc.1

Released 2024-Jun-11

* Update OpenTelemetry SDK version to `1.9.0-rc.1`.
  ([#1876](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1876))

* Added `LoggerProviderBuilder.AddOneCollectorExporter` registration extension.
  ([#1876](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1876))

## 1.8.0

Released 2024-Apr-22

* Native AOT compatibility.
  ([#1670](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1670))

* Update OpenTelemetry SDK version to `1.8.1`.
  ([#1668](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1668))

## 1.6.0

Released 2023-Oct-25

## 1.6.0-rc.1

Released 2023-Oct-10

## 1.6.0-beta.1

Released 2023-Sep-20

* Update OpenTelemetry to 1.6.0
  ([#1344](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1344))

* Added support for receiving transmission failures via the
  `RegisterPayloadTransmittedCallback` API.
  ([#1309](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1309))

* Added support for sending `LogRecord.Body` as common schema `body` if
  `{OriginalFormat}` key is not found.
  ([#1321](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1321))

* Added support for sending `LogRecord.FormattedMessage` (if set) as common
  schema `formattedMessage` if it differs from the detected template (either
  `{OriginalFormat}` key or `LogRecord.Body`).
  ([#1321](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1321))

* Removed `traceFlags` from the common schema `dt` (Distributed Tracing)
  extension because it is not currently supported by the OneCollector service.
  ([#1345](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1345))

* Added dedicated handling for `IReadOnlyList<KeyValuePair<string, object>>`
  types during serialization to improve performance.
  ([#1361](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1361))

* Added caching of extension property UTF8 JSON strings to improve performance.
  ([#1361](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1361))

## 1.5.1

Released 2023-Aug-07

## 1.5.1-rc.1

Released 2023-Jun-29

* Added support for sending common schema extensions using `ext.[name].[field]`
  syntax.
  ([#1073](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1073))

* Added support for sending common schema `dt` (Distributed Tracing) extension
  when trace context is present on `LogRecord`s.
  ([#1073](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1073))

* Added support for sending common schema `ex` (Exception) extension when
  exception is present on `LogRecord`s.
  ([#1082](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1082))

* Added support for sending common schema `eventId` field when `EventId.Id` is
  non-zero on `LogRecord`s.
  ([#1127](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1127))

* Update OpenTelemetry to 1.5.1
  ([#1255](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1255))

## 0.1.0-alpha.2

Released 2023-Mar-6

* Update OpenTelemetry to 1.4.0
  ([#1038](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1038))

* Tenant token is no longer exposed on `OneCollectorExporterOptions` and will be
  set automatically from the instrumentation key. Added new registration
  overloads and a builder to help with configuration.
  ([#1032](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1032))

* Switched to using a connection string design instead of passing
  instrumentation key directly.
  ([#1037](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1037))

* Added `RegisterPayloadTransmittedCallback` API on `OneCollectorExporter<T>`.
  ([#1058](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1058))

## 0.1.0-alpha.1

Released 2023-Feb-16

* Initial release.
