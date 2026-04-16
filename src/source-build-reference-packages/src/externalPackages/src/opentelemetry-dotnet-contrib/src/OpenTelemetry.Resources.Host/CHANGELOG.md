# Changelog

## Unreleased

## 1.15.0-beta.1

Released 2026-Jan-21

* Updated OpenTelemetry core component version(s) to `1.15.0`.
  ([#3721](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3721))

## 1.14.0-beta.1

Released 2025-Nov-13

* Add support for .NET 10.0.
  ([#2822](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2822))

* Update .NET 10.0 NuGet package versions from `10.0.0-rc.2.25502.107` to `10.0.0`.
  ([#3403](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3403))

* Updated OpenTelemetry core component version(s) to `1.14.0`.
  ([#3403](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3403))

## 1.13.0-beta.1

Released 2025-Oct-10

* Added support for the `host.arch` resource attribute in `HostDetector`
  for .NET only.
  ([#3147](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3147))

* Updated OpenTelemetry core component version(s) to `1.13.1`.
  ([#3218](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3218))

## 1.12.0-beta.1

Released 2025-May-06

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

* Drop support for .NET 6 as this target is no longer supported
  and add .NET 8/.NET Standard 2.0 targets.
  ([#2168](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2168))

* Updated OpenTelemetry core component version(s) to `1.10.0`.
  ([#2317](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2317))

## 0.1.0-beta.3

Released 2024-Aug-30

* Fix the bug where macOS was detected as Linux
  ([#1985](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1985))

## 0.1.0-beta.2

Released 2024-Jun-18

* Updated OpenTelemetry core component version(s) to `1.9.0`.
  ([#1888](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1888))

## 0.1.0-beta.1

Released 2024-Jun-04

* **Breaking Change**: Renamed package from `OpenTelemetry.ResourceDetectors.Host`
  to `OpenTelemetry.Resources.Host`.
  ([#1820](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1820))

* **Breaking Change**: `HostDetector` type is now internal, use `ResourceBuilder`
  extension method `AddHostDetector` to enable the detector.
  ([#1820](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1820))

* Adds support for `host.id` resource attribute on non-containerized systems.
`host.id` will be set per [semantic convention rules](https://github.com/open-telemetry/semantic-conventions/blob/v1.24.0/docs/resource/host.md)
  ([#1631](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1631))

* Update OpenTelemetry SDK version to `1.8.1`.
  ([#1668](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1668))

## 0.1.0-alpha.3

Released 2024-Apr-05

* Update OpenTelemetry SDK version to `1.8.0`.
  ([#1635](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1635))

## 0.1.0-alpha.2

Released 2024-Jan-03

* Update OpenTelemetry SDK version to `1.7.0`.
  ([#1518](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1518))

## 0.1.0-alpha.1

Released 2023-Dec-21

* Initial release of `OpenTelemetry.ResourceDetectors.Host` project
  [1507](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1507)
