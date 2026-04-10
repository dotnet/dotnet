# Changelog

## Unreleased

## 1.0.0-beta.9

Released 2026-Jan-21

* Updated OpenTelemetry core component version(s) to `1.15.0`.
  ([#3721](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3721))

## 1.0.0-beta.8

Released 2025-Nov-13

* Updated OpenTelemetry core component version(s) to `1.14.0`.
  ([#3403](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3403))

## 1.0.0-beta.7

Released 2025-Oct-23

* Updated minimal supported version of `Grpc.Core.Api` to `2.46.6`.
  ([#1936](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1936),
   [#1940](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1940))

* Updated OpenTelemetry core component version(s) to `1.13.1`.
  ([#3218](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3218))

## 1.0.0-beta.6

Released 2024-May-21

* Make the context propagation extraction case insensitive.
  ([#483](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/483))

* Update minimal supported  version of `Google.Protobuf` to `3.15.0`.
  ([#1456](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1456))

* `ActivitySource.Version` is set to NuGet package version.
  ([#1624](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1624))

* Update `OpenTelemetry.Api` to `1.8.1`.
  ([#1668](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1668))

* `RecordException` option for both client and server interceptors.
  ([#1648](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1648))

* Use `Activity.Status` and `Activity.StatusDescription`
  instead of `otel.status_code` and `otel.status_description` tags.
  ([#1799](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1799))

## 1.0.0-beta.5

Released 2022-Mar-25

* Switched Grpc.Core package dependency to Grpc.Core.Api in the same range.
  No functional change, just less exposure to unnecessary packages.

## 1.0.0-beta.4

Released 2022-Mar-18

* Going forward the NuGet package will be
  [`OpenTelemetry.Instrumentation.GrpcCore`](https://www.nuget.org/packages/OpenTelemetry.Instrumentation.GrpcCore).
  Older versions will remain at
  [`OpenTelemetry.Contrib.Instrumentation.GrpcCore`](https://www.nuget.org/packages/OpenTelemetry.Contrib.Instrumentation.GrpcCore)
  [(#255)](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/255)

  Migration:

  * In code update namespaces (eg `using
    OpenTelemetry.Contrib.Instrumentation.GrpcCore` -> `using
    OpenTelemetry.Instrumentation.GrpcCore`)

## 1.0.0-beta3

Released 2021-Jun-11

* Updated OpenTelemetry SDK package version to 1.1.0-beta1
  ([#100](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/100))

* Do NOT mutate incoming call headers, copy them before propagation
  ([#143](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/143))

## 1.0.0-beta2

Released 2021-Mar-17

* This is the first release of `OpenTelemetry.Contrib.Instrumentation.GrpcCore`
  package.

For more details, please refer to the [README](README.md).
