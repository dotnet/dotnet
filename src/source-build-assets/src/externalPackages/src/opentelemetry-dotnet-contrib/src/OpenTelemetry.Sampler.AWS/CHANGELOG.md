# Changelog - OpenTelemetry.Sampler.AWS

## Unreleased

## 0.1.0-alpha.7

Released 2026-Jan-21

* Updated OpenTelemetry core component version(s) to `1.15.0`.
  ([#3721](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3721))

## 0.1.0-alpha.6

Released 2025-Nov-13

* Add support for .NET 10.0.
  ([#2822](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2822))

* Update .NET 10.0 NuGet package versions from `10.0.0-rc.2.25502.107` to `10.0.0`.
  ([#3403](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3403))

* Updated OpenTelemetry core component version(s) to `1.14.0`.
  ([#3403](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3403))

## 0.1.0-alpha.5

Released 2025-Oct-28

* Updated OpenTelemetry core component version(s) to `1.13.1`.
  ([#3218](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3218))

## 0.1.0-alpha.4

Released 2025-Mar-05

* Updated OpenTelemetry core component version(s) to `1.11.2`.
  ([#2582](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2582))

## 0.1.0-alpha.3

Released 2024-Dec-12

* Drop support for .NET 6 as this target is no longer supported and add .NET 8 target.
  ([#2172](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2172))

* Bumped the `System.Text.Json` reference to `6.0.10` for runtimes older than
  `net8.0` and added a direct reference for `System.Text.Json` at `8.0.5` on
  `net8.0` in response to
  [CVE-2024-43485](https://msrc.microsoft.com/update-guide/vulnerability/CVE-2024-43485).
  ([#2197](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2197))

* Removed the `System.Net.Http` package reference from all targets.
  ([#2197](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2197))

* Updated OpenTelemetry core component version(s) to `1.10.0`.
  ([#2317](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2317))

* `AWSXRayRemoteSamplerBuilder.Build()` now returns `ParentBasedSampler` which
  which is of type `Sampler` instead of `AWSXRayRemoteSampler`.
  ([#2188](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2188))

## 0.1.0-alpha.2

Released 2024-Sep-09

* Performance problem fix for calling event source when required.
  ([#2046](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2046))

## 0.1.0-alpha.1

Released 2024-Jun-20

Initial release of `OpenTelemetry.Sampler.AWS`.

* Feature - AWSXRayRemoteSampler - Add support for AWS X-Ray remote sampling
  ([#1091](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1091),
   [#1124](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1124))

* Make OpenTelemetry.Sampler.AWS native AoT compatible.
  ([#1541](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1541))

* Updated OpenTelemetry core component version(s) to `1.9.0`.
  ([#1888](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1888))
