# Changelog

## Unreleased

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

Released 2025-Oct-23

* Updated OpenTelemetry core component version(s) to `1.13.1`.
  ([#3218](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3218))

## 1.11.0-beta.1

Released 2025-Mar-05

* Updated OpenTelemetry core component version(s) to `1.11.2`.
  ([#2582](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2582))

## 1.10.0-beta.1

Released 2024-Dec-30

* Dropped support for the `net7.0` target because .NET 7 is no longer supported.
  ([#2038](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/issues/2038))

* Update BaggageActivityProcessor to require baggage key predicate.
  ([#1816](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1816))

* Added rate limiting sampler which limits the number of traces to the specified
rate per second. For details see
  [RateLimitingSampler](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/tree/main/src/OpenTelemetry.Extensions#ratelimitingsampler).
  ([#1996](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1996))

* Drop support for .NET 6 as this target is no longer supported and add .NET 8 target.
  ([#2124](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2124))

* Updated OpenTelemetry core component version(s) to `1.10.0`.
  ([#2317](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2317))

* Adds Baggage LogRecord Processor.
  ([#2354](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2354))

## 1.0.0-beta.5

Released 2024-May-08

* Add LogToActivityEventConversionOptions.Filter callback
  ([#1059](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1059))

* Update OpenTelemetry SDK version to `1.8.1`.
  ([#1668](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1668))

* Add Baggage Activity Processor.
  ([#1659](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1659))

## 1.0.0-beta.4

Released 2023-Feb-27

* Update OpenTelemetry to 1.4.0
  ([#1038](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1038))

## 1.0.0-beta.3

Released 2022-Nov-09

* Update OpenTelemetry to 1.4.0-beta.2
  ([#680](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/680))

* Implemented auto flush activity processor
  ([#297](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/297))

* Removes .NET Framework 4.6.1. The minimum .NET Framework version
  supported is .NET 4.6.2.

* Removes net5.0 target as .NET 5.0 is going out
  of support. The package keeps netstandard2.0 target, so it
  can still be used with .NET5.0 apps.
  ([#617](https://github.com/open-telemetry/opentelemetry-dotnet/pull/617))

* Going forward the NuGet package will be
  [`OpenTelemetry.Extensions`](https://www.nuget.org/packages/OpenTelemetry.Extensions).
  Older versions will remain at
  [`OpenTelemetry.Contrib.Preview`](https://www.nuget.org/packages/OpenTelemetry.Contrib.Preview)
  [(#266)](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/266)

## 1.0.0-beta2

* This is the first release of `OpenTelemetry.Contrib.Preview` package.

For more details, please refer to the [README](README.md).
