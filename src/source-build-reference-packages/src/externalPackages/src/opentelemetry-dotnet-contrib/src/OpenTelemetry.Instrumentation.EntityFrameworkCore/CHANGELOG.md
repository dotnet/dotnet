# Changelog

## Unreleased

## 1.15.0-beta.1

Released 2026-Jan-21

* Improve SQL parsing for sanitization and summary generation. Support additional
  cases for escaped identifiers. Optimize performance of parsing logic.
  ([#3627](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3627))

* Improve SQL parsing for sanitization for Unicode string literals.
  ([#3662](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3662))

* Sanitize the object name for SQL query text using the LOGIN or USER keywords and
  remove from query summaries.
  ([#3663](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3663))

* Improve SQL parsing to generate query summaries for more T-SQL keywords.
  ([#3671](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3671))

* Updated OpenTelemetry core component version(s) to `1.15.0`.
  ([#3721](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3721))

## 1.14.0-beta.2

Released 2025-Nov-14

* Added `net10.0` and `net8.0` target frameworks.
  ([#3519](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3519))

## 1.14.0-beta.1

Released 2025-Nov-13

* Updated OpenTelemetry core component version(s) to `1.14.0`.
  ([#3403](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3403))

## 1.13.0-beta.2

Released 2025-Nov-03

* Fix diacritic identifier parsing hang and improve FROM clause parsing.
([#3368](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3368))

## 1.13.0-beta.1

Released 2025-Oct-22

* Added support for detecting Snowflake for the `db.system`/`db.system.name` attributes
  when using `EFCore.Snowflake`.
  ([#2980](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2980))

* Fix `db.system.name` values to follow new database semantic conventions when opted
  into using the `OTEL_SEMCONV_STABILITY_OPT_IN` environment variable.
  ([#3004](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3004))

* Add the `server.port` resource attribute when following the new database semantic
  conventions when opted into using the `OTEL_SEMCONV_STABILITY_OPT_IN` environment
  variable.
  ([#3011](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3011))

* Extend `db.system.name` values to identity additional providers related to Couchbase,
  DB2, MongoDB, MySQL, Oracle, PostgreSQL and SQLite.
  ([#3025](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3025))

* Add `db.query.parameter.<key>` attribute(s) to query spans if opted into using
  the `OTEL_DOTNET_EXPERIMENTAL_EFCORE_ENABLE_TRACE_DB_QUERY_PARAMETERS`
  environment variable.
  ([#3015](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3015),
  [#3081](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3081))

* Add the `db.query.summary` attribute and use it for the trace span name when opted
  into using the `OTEL_SEMCONV_STABILITY_OPT_IN` environment variable.
  ([#3022](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3022))

* The `db.statement` and `db.query.text` attributes are now sanitized when using
  specific SQL-like EFCore providers.
  ([#3022](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3022))

* **Breaking change**: The `SetDbStatementForStoredProcedure` and
  `SetDbStatementForText` properties have been removed. Behaviors related to this
  option are now always enabled.
  ([#3072](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3072))

* **Breaking change**: `db.system.name` now only sets names that are explicitly
  defined in the Semantic Conventions for databases.
  ([#3075](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3075))

* Updated OpenTelemetry core component version(s) to `1.13.1`.
  ([#3218](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3218))

## 1.12.0-beta.2

Released 2025-Jun-25

* Support use with `SqlClient` instrumentation.
  ([#2280](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2280),
   [#2829](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2829))

## 1.12.0-beta.1

Released 2025-May-05

* Updated OpenTelemetry core component version(s) to `1.12.0`.
  ([#2725](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2725))

## 1.11.0-beta.2

Released 2025-Mar-05

* Fixed attribute `db.system` for following providers:
  * `Devart.Data.SQLite.Entity.EFCore`,
  * `Devart.Data.MySql.Entity.EFCore`,
  * `Devart.Data.PostgreSql.Entity.EFCore`.
  ([#2571](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2571))

* Updated OpenTelemetry core component version(s) to `1.11.2`.
  ([#2582](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2582))

## 1.11.0-beta.1

Released 2025-Jan-27

* Attribute `db.system` reports `oracle` when
  `Devart.Data.Oracle.Entity.EFCore` is used a provider.
  ([#2465](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2465))

* Updated OpenTelemetry core component version(s) to `1.11.1`.
  ([#2477](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2477))

## 1.10.0-beta.1

Released 2024-Dec-09

* The new database semantic conventions can be opted in to by setting
  the `OTEL_SEMCONV_STABILITY_OPT_IN` environment variable. This allows for a
  transition period for users to experiment with the new semantic conventions
  and adapt as necessary. The environment variable supports the following
  values:
  * `database` - emit the new, frozen (proposed for stable) database
  attributes, and stop emitting the old experimental database
  attributes that the instrumentation emitted previously.
  * `database/dup` - emit both the old and the frozen (proposed for stable) database
  attributes, allowing for a more seamless transition.
  * The default behavior (in the absence of one of these values) is to continue
  emitting the same database semantic conventions that were emitted in
  the previous version.
  * Note: this option will be removed after the new database
  semantic conventions is marked stable. At which time this
  instrumentation can receive a stable release, and the old database
  semantic conventions will no longer be supported. Refer to the
  specification for more information regarding the new database
  semantic conventions for
  [spans](https://github.com/open-telemetry/semantic-conventions/blob/v1.28.0/docs/database/database-spans.md).
  ([#2130](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2130))

* Updated OpenTelemetry core component version(s) to `1.10.0`.
  ([#2317](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2317))

* Trace instrumentation will now call the [Activity.SetStatus](https://learn.microsoft.com/dotnet/api/system.diagnostics.activity.setstatus)
  API instead of the deprecated OpenTelemetry API package extension when setting
  span status. For details see: [Setting Status](https://github.com/open-telemetry/opentelemetry-dotnet/blob/main/src/OpenTelemetry.Api/README.md#setting-status).
  ([#2358](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2358))

## 1.0.0-beta.12

Released 2024-Jun-18

* Update `Microsoft.Extensions.Options` to `8.0.0`.
  ([#1830](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1830))

* Updated OpenTelemetry core component version(s) to `1.9.0`.
  ([#1888](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1888))

## 1.0.0-beta.11

Released 2024-Apr-05

* Update OpenTelemetry SDK version to `1.8.0`.
  ([#1635](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1635))

## 1.0.0-beta.10

Released 2024-Feb-07

* **Breaking Change**: Stop emitting `db.statement_type` attribute.
  This attribute never was part of the [semantic convention](https://github.com/open-telemetry/semantic-conventions/blob/v1.24.0/docs/database/database-spans.md#call-level-attributes).
  ([#1559](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1559))

* `ActivitySource.Version` is set to NuGet package version.
  ([#1624](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1624))

## 1.0.0-beta.9

Released 2024-Jan-03

* Update OpenTelemetry SDK version to `1.7.0`.
  ([#1486](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1486))

## 1.0.0-beta.8

Released 2023-Oct-24

* Added `Filter` public API on `EntityFrameworkInstrumentationOptions` to
  enable filtering of instrumentation.
  ([#1203](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1203))

* Updated OpenTelemetry SDK package version to 1.6.0
  ([#1344](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1344))

* Fix issue of multiple instances of OpenTelemetry-Instrumentation EventSource
  being created
  ([#1362](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1362))

## 1.0.0-beta.7

Released 2023-Jun-09

* Updated OpenTelemetry SDK package version to 1.5.0
  ([#1220](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1220))

## 1.0.0-beta.6

Released 2023-Mar-13

* Added overloads which accept a name to the `TracerProviderBuilder`
  `EntityFrameworkInstrumentationOptions` extension to allow for more fine-grained
  options management
   ([#1020](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1020))

## 1.0.0-beta.5

Released 2023-Feb-27

* Updated OpenTelemetry SDK package version to 1.4.0
  ([#1038](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1038))

## 1.0.0-beta.4

Released 2023-Jan-25

* Updated OpenTelemetry SDK package version to 1.3.2
  ([#917](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/917))

* Update the `ActivitySource` name used to the assembly name:
`OpenTelemetry.Instrumentation.EntityFrameworkCore`
  ([#486](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/486))

* Removes `AddEntityFrameworkCoreInstrumentation` method with default configure
  parameter.
  ([#916](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/916))

* Added support to `EnrichWithIDbCommand`
  ([#868](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/868))

* Map missing dbs to db.system:
`OpenTelemetry.Instrumentation.EntityFrameworkCore`
  [#898](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/898)

## 1.0.0-beta.3

Released 2022-Mar-18

* Going forward the NuGet package will be
  [`OpenTelemetry.Instrumentation.EntityFrameworkCore`](https://www.nuget.org/packages/OpenTelemetry.Instrumentation.EntityFrameworkCore).
  Older versions will remain at
  [`OpenTelemetry.Contrib.Instrumentation.EntityFrameworkCore`](https://www.nuget.org/packages/OpenTelemetry.Contrib.Instrumentation.EntityFrameworkCore)
  ([#261](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/261))

  Migration:

  * In code update namespaces (eg `using
    OpenTelemetry.Contrib.Instrumentation.EntityFrameworkCore` -> `using
    OpenTelemetry.Instrumentation.EntityFrameworkCore`)

## 1.0.0-beta2

Released 2021-Jun-11

* EntityFrameworkCore instrumentation to depend on API and not SDK
  ([#121](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/121))

## 0.6.0-beta

Released 2020-Sep-29

* This is the first release of
  `OpenTelemetry.Contrib.Instrumentation.EntityFrameworkCore` package.

For more details, please refer to the [README](README.md).
