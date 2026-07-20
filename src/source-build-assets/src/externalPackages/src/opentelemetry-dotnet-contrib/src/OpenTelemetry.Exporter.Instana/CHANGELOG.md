# Changelog

## Deprecated

* This package is deprecated. Please check [README.md](README.md#deprecated)
  for more details.

## 1.1.0

Released 2026-Apr-27

* Add `net8.0` and `net10.0` target frameworks.
  ([#4153](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/4153))

* Add support for configuring the Instana exporter using `InstanaExporterOptions`.
  ([#4153](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/4153))

* **Breaking change**: TLS certificate validation is no longer unconditionally
  disabled when a proxy is configured using the `INSTANA_ENDPOINT_PROXY` environment
  variable. To restore the previous behaviour and disable TLS certificate validation
  use the `InstanaExporterOptions.HttpClientFactory` property to configure a custom
  `HttpClient` for the exporter to use.
  ([#4153](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/4153))

* Use System.Text.Json for JSON serialization.
  ([#4293](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/4293))

## 1.0.7

Released 2026-Apr-21

* Updated OpenTelemetry core component version(s) to `1.15.3`.
  ([#4166](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/4166))

## 1.0.6

Released 2026-Jan-21

* Updated OpenTelemetry core component version(s) to `1.15.0`.
  ([#3721](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3721))

## 1.0.5

Released 2025-Nov-13

* Updated OpenTelemetry core component version(s) to `1.14.0`.
  ([#3403](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3403))

## 1.0.4

Released 2025-Oct-23

* Drop support for .NET Framework 4.6.1.
  The lowest supported version is .NET Framework 4.6.2.
  ([#1050](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1050))

* Updated OpenTelemetry core component version(s) to `1.13.1`.
  ([#3218](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3218))

## 1.0.3

Released 2023-Feb-21

* Fixes issue in span serialization process introduced in 1.0.2 version.
  ([#979](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/979))

* Update OpenTelemetry SDK version to `1.3.2`.
  ([#917](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/917))

## 1.0.2

Released 2022-Dec-20

* Updated `Transport` with exception-handling and a couple of bug fixes ([#747](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/issues/747)):
  * Adds `InstanaExporterEventSource` to provide for error logging.
  * Adds exception-handling to `Transport` with logging via `InstanaExporterEventSource`.
  * Fixes `Transport` buffering to prevent exceeding underlying array capacity.
  * Fixes `Transport` to prevent lost spans due to buffer length.

## 1.0.1

Released 2022-Nov-02

* Instana span duration was not calculated correctly
  [376](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/376)

* Application is crashing if environment variables are not defined
  [385](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/385)

* Update OpenTelemetry SDK version to `1.3.1`.
  ([#749](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/749))

## 1.0.0

Released 2022-May-24

* This is the first release of the `OpenTelemetry.Exporter.Instana`
project.
