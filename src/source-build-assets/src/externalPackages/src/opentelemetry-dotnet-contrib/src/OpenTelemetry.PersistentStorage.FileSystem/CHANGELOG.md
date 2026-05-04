# Changelog - OpenTelemetry.PersistentStorage.FileSystem

## Unreleased

## 1.0.2

Released 2025-Nov-13

## 1.0.1

Released 2025-Feb-14

* Fix `System.FormatException` thrown by `PersistentStorageEventSource`.
  [#1613](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1613)

* Switch to deterministic builds.
  ([#1397](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1397))

## 1.0.0

Released 2023-Aug-28

## 1.0.0-beta.2

Released 2023-Apr-17

* Fix a bug affecting the directory size when multiple `FileBlobProvider`s
  were in a single process. [(#1133)](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1133)

* `FileBlobProvider` will now use the path provided during initialization as is
  for storing blobs, without adding additional hash of current user and process.
([#1110](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1110))

* Going forward the NuGet package will be
  [`OpenTelemetry.PersistentStorage.FileSystem`](https://www.nuget.org/packages/OpenTelemetry.PersistentStorage.FileSystem).
  Older versions will remain at
  [`OpenTelemetry.Extensions.PersistentStorage`](https://www.nuget.org/packages/OpenTelemetry.Extensions.PersistentStorage)
  [(#1079)](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1079)

  Migration:

  * In code update namespaces (e.g. `using
    OpenTelemetry.Extensions.PersistentStorage` -> `using
    OpenTelemetry.Extensions.FileSystem`)

## 1.0.0-beta.1

* Invalid path or permissions issues will now result in `FileBlobProvider`
  initialization failure by throwing exception.
  ([#578](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/578))

## 1.0.0-alpha.4

* Update implementation to use
  Opentelemetry.Extensions.PersistentStorage.Abstractions.
  [363](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/363)

## 1.0.0-alpha.3

* Going forward the NuGet package will be
  [`OpenTelemetry.Extensions.PersistentStorage`](https://www.nuget.org/packages/OpenTelemetry.Extensions.PersistentStorage).
  Older versions will remain at
  [`OpenTelemetry.Contrib.Extensions.PersistentStorage`](https://www.nuget.org/packages/OpenTelemetry.Contrib.Extensions.PersistentStorage)
  [(#258)](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/258)

  Migration:

  * In code update namespaces (eg `using
    OpenTelemetry.Contrib.Extensions.PersistentStorage` -> `using
    OpenTelemetry.Extensions.PersistentStorage`)

## 1.0.0-alpha2

This is the first release for the
`OpenTelemetry.Contrib.Extensions.PersistentStorage` project.

For more details, please refer to the [README](README.md)
