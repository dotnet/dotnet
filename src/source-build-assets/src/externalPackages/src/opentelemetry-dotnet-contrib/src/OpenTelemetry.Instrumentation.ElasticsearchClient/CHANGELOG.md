# Changelog

## Unreleased

## 1.15.0-beta.1

Released 2026-Jan-21

* Add `net8.0` and `net10.0` target frameworks.
  ([#3717](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3717))

* Updated OpenTelemetry core component version(s) to `1.15.0`.
  ([#3721](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3721))

## 1.14.0-beta.1

Released 2025-Nov-13

* Updated OpenTelemetry core component version(s) to `1.14.0`.
  ([#3403](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3403))

## 1.13.0-beta.1

Released 2025-Oct-23

* Updated OpenTelemetry core component version(s) to `1.13.1`.
  ([#3218](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3218))

## 1.12.0-beta.1

Released 2025-Jul-30

* Span status is set based on [semantic convention for client spans](https://github.com/open-telemetry/semantic-conventions/blob/v1.24.0/docs/http/http-spans.md#status).
  ([#1538](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1538))

* `ActivitySource.Version` is set to NuGet package version.
  ([#1624](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1624))

* Replace `db.url` attribute with `url.full` to comply with [semantic conventions](https://github.com/open-telemetry/semantic-conventions/blob/v1.25.0/docs/database/elasticsearch.md#attributes).
  Redact `username` and `password` part of the `url.full`.
  ([#1684](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1684))

* Lowered the `System.Text.Json` reference to `4.7.2` for `net462` and
  `netstandard2.0` targets in response to
  [CVE-2024-43485](https://msrc.microsoft.com/update-guide/vulnerability/CVE-2024-43485).
  ([#2198](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2198))

* Updated OpenTelemetry core component version(s) to `1.12.0`.
  ([#2725](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2725))

## 1.0.0-beta.5

Released 2023-Oct-24

* Fix issue of multiple instances of OpenTelemetry-Instrumentation EventSource
  being created
  ([#1362](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1362))

* Updated OpenTelemetry SDK package version to 1.6.0
  ([#1344](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1344))

## 1.0.0-beta.4

Released 2023-Mar-06

* Updated OpenTelemetry SDK package version to 1.4.0
  ([#1019](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1019))

* Update minimum full framework support to net462

* Requests that get an HTTP status code of 404 are not marked as an error span status

* Add MaxDbStatementLength option with default of 4096

* Remove duplicated HTTP method and URL from db.statement attribute value

* Fix faulty logic of MaxDbStatementLength option
  ([#425](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/425))

* Remove method with default attribute
  ([#1019](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1019))

* Added overloads which accept a name to the `TracerProviderBuilder`
  `AddElasticsearchClientInstrumentation` extension to allow for more fine-grained
  options management
  ([#1019](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1019))

## 1.0.0-beta.3

* Going forward the NuGet package will be
  [`OpenTelemetry.Instrumentation.ElasticsearchClient`](https://www.nuget.org/packages/OpenTelemetry.Instrumentation.ElasticsearchClient).
  Older versions will remain at
  [`OpenTelemetry.Contrib.Instrumentation.ElasticsearchClient`](https://www.nuget.org/packages/OpenTelemetry.Contrib.Instrumentation.ElasticsearchClient)
  [(#248)](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/248)

  Migration:

  * In code update namespaces (eg `using
    OpenTelemetry.Contrib.Instrumentation.ElasticsearchClient` -> `using
    OpenTelemetry.Instrumentation.ElasticsearchClient`)

## 1.0.0-beta2

Released 2021-June-17

* Updated OpenTelemetry SDK package version to 1.1.0-beta4
  ([#136](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/136))

## Initial Release

* Updated OpenTelemetry SDK package version to 1.1.0-beta1
  ([#100](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/100))
