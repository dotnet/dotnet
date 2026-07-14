# Changelog

## Unreleased

## 1.1.0

Released 2026-May-05

* Add `net8.0` and `net10.0` target frameworks.
  ([#4128](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/4128))

* Add support for `ReadOnlySpan<byte>` as buffers.
  Obsoletes `byte[]` buffers in the API.
  ([#4128](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/4128))

## 1.0.3

Released 2026-Apr-21

## 1.0.2

Released 2025-Nov-13

## 1.0.1

Released 2025-Feb-14

* Switch to deterministic builds.
  ([#1397](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1397))

## 1.0.0

Released 2023-Aug-28

## 1.0.0-beta.2

Released 2023-Apr-14

* Going forward the NuGet package will be
 [`OpenTelemetry.PersistentStorage.Abstractions`](https://www.nuget.org/packages/OpenTelemetry.PersistentStorage.Abstractions).
 Older versions will remain at
 [`OpenTelemetry.Extensions.PersistentStorage.Abstractions`](https://www.nuget.org/packages/OpenTelemetry.Extensions.PersistentStorage.Abstractions)
 [#1079](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1079)

  Migration:

  * In code update namespaces (e.g. `using
    OpenTelemetry.Extensions.PersistentStorage.Abstractions` -> `using
    OpenTelemetry.PersistentStorage.Abstractions`)

## 1.0.0-beta.1

## 1.0.0-alpha.4

This is the first release for the `OpenTelemetry.Extensions.PersistentStorage.Abstractions`
project.

For more details, please refer to the
[README](README.md)
