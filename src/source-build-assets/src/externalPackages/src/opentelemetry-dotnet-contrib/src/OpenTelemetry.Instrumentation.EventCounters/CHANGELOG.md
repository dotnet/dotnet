# Changelog

## Unreleased

## 1.15.0-alpha.1

Released 2026-Jan-21

* Updated OpenTelemetry core component version(s) to `1.15.0`.
  ([#3721](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3721))

## 1.14.0-alpha.1

Released 2025-Nov-13

* Updated OpenTelemetry core component version(s) to `1.14.0`.
  ([#3403](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3403))

## 1.13.0-alpha.1

Released 2025-Oct-23

* `Meter.Version` is set to NuGet package version.
  ([#1624](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1624))

* Updated OpenTelemetry core component version(s) to `1.13.1`.
  ([#3218](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3218))

## 1.5.1-alpha.1

Released 2023-Jul-11

* Bumped the package version to `1.5.1-alpha.1` to keep its major and minor
  version in sync with that of the core packages. This would make it more
  intuitive for users to figure out what version of core packages would work
  with a given version of this package.

* Added a static constructor to ensure `EventCountersInstrumentationEventSource`
got initialized when `EventCountersMetrics` was accessed for the first time to
prevent potential deadlock;
e.g.: <https://github.com/open-telemetry/opentelemetry-dotnet-contrib/issues/1024>.
  ([#1260](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1260))

* Update OpenTelemetry.Api to 1.5.1.
  ([#1255](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1255))

## 1.0.0-alpha.2

Released 2022-Nov-10

* Update OpenTelemetry.Api to 1.3.1.

* Change `EventCounter` prefix to `ec` and trim the event source name to keep
  instrument name under 63 characters.
  ([#740](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/740))

## 1.0.0-alpha.1

Released 2022-Oct-17

* Simplified implementation. EventSources must be explicitly configured to be
  listened to now.
  ([#620](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/620))

## 0.1.0-alpha.1

Released 2022-Jul-12

* This is the first release of `OpenTelemetry.Instrumentation.EventCounters` package.

For more details, please refer to the [README](README.md).
