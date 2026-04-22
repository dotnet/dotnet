# Changelog

## Unreleased

## 1.15.0-beta.1

Released 2026-Jan-21

* Updated OpenTelemetry core component version(s) to `1.15.0`.
  ([#3721](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3721))

## 1.14.0-beta.2

Released 2025-Nov-14

* Added `net10.0` and `net8.0` target frameworks.
  ([#3519](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3519))

## 1.14.0-beta.1

Released 2025-Nov-13

* Updated OpenTelemetry core component version(s) to `1.14.0`.
  ([#3403](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3403))

## 1.13.0-beta.1

Released 2025-Oct-22

* Updated OpenTelemetry core component version(s) to `1.13.1`.
  ([#3218](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3218))

## 1.12.0-beta.1

Released 2025-May-05

* Updated OpenTelemetry core component version(s) to `1.12.0`.
  ([#2725](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2725))

## 1.11.0-beta.2

Released 2025-Mar-05

* Updated OpenTelemetry core component version(s) to `1.11.2`.
  ([#2582](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2582))

## 1.11.0-beta.1

Released 2025-Jan-27

* Updated OpenTelemetry core component version(s) to `1.11.1`.
  ([#2477](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2477))

## 1.10.0-beta.1

Released 2024-Dec-09

* Updated OpenTelemetry core component version(s) to `1.10.0`.
  ([#2317](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2317))

* Trace instrumentation will now call the [Activity.SetStatus](https://learn.microsoft.com/dotnet/api/system.diagnostics.activity.setstatus)
  API instead of the deprecated OpenTelemetry API package extension when setting
  span status. For details see: [Setting Status](https://github.com/open-telemetry/opentelemetry-dotnet/blob/main/src/OpenTelemetry.Api/README.md#setting-status).
  ([#2358](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2358))

## 1.0.0-beta.3

Released 2024-Jun-18

* Updated OpenTelemetry core component version(s) to `1.9.0`.
  ([#1888](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1888))

## 1.0.0-beta.2

Released 2024-Apr-05

* `ActivitySource.Version` is set to NuGet package version.
  ([#1624](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1624))

* Update `OpenTelemetry.Api` to `1.8.0`.
  ([#1635](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1635))

## 1.0.0-beta.1

Released 2024-Jan-03

* Fix issue of multiple instances of OpenTelemetry-Instrumentation EventSource
  being created
  ([#1362](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1362))

* Update `OpenTelemetry.Api` to `1.7.0`.
  ([#1486](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1486))

## 1.0.0-alpha.3

Released 2023-Jun-09

* Update OpenTelemetry.Api to 1.5.0.
  ([#1220](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1220))

## 1.0.0-alpha.2

Released 2023-Feb-27

* Update OpenTelemetry.Api to 1.4.0.
  ([#1038](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1038))

* Removes .NET Framework 4.7.2. It is distributed as .NET Standard 2.0.
  ([#911](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/911))

* Removes `AddQuartzInstrumentation` method with default configure default parameter.
  ([#914](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/914))

## 1.0.0-alpha.1

* This is the first release of `OpenTelemetry.Instrumentation.Quartz` package.

For more details, please refer to the [README](README.md).
