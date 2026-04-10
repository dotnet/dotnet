# Changelog

## Unreleased

## 0.1.0-alpha.5

Released 2026-Jan-21

* Updated OpenTelemetry core component version(s) to `1.15.0`.
  ([#3721](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3721))

## 0.1.0-alpha.4

Released 2025-Nov-13

* Add support for .NET 10.0.
  ([#2822](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2822))

* Update .NET 10.0 NuGet package versions from `10.0.0-rc.2.25502.107` to `10.0.0`.
  ([#3403](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3403))

* Updated OpenTelemetry core component version(s) to `1.14.0`.
  ([#3403](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3403))

## 0.1.0-alpha.3

Released 2025-Oct-23

* Drop support for .NET 6 as this target is no longer supported
  and add .NET Standard 2.0 target.
  ([#2142](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2142))

* Trace instrumentation will now call the [Activity.SetStatus](https://learn.microsoft.com/dotnet/api/system.diagnostics.activity.setstatus)
  API instead of the deprecated OpenTelemetry API package extension when setting
  span status. For details see: [Setting Status](https://github.com/open-telemetry/opentelemetry-dotnet/blob/main/src/OpenTelemetry.Api/README.md#setting-status).
  ([#2358](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2358))

* The `messaging.receive.duration` and `messaging.publish.duration` histograms
  (measured in seconds) produced by the metrics instrumentation in this package
  now uses the [Advice API](https://github.com/open-telemetry/opentelemetry-dotnet/blob/core-1.10.0/docs/metrics/customizing-the-sdk/README.md#explicit-bucket-histogram-aggregation)
  to set default explicit buckets following the [OpenTelemetry Specification](https://github.com/open-telemetry/semantic-conventions/blob/v1.24.0/docs/messaging/messaging-metrics.md).
  ([#2430](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2430))

* Rethrow exception on consume and process.
  ([#2847](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2847))

* Updated OpenTelemetry core component version(s) to `1.13.1`.
  ([#3218](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3218))

## 0.1.0-alpha.2

Released 2024-Sep-18

* Add named instrumentation support
  ([#2074](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2074))

## 0.1.0-alpha.1

Released 2024-Sep-16

* Initial release
