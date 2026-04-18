# Changelog

## Unreleased

## 1.15.0-rc.1

Released 2026-Jan-21

* Updated OpenTelemetry core component version(s) to `1.15.0`.
  ([#3721](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3721))

## 1.14.0-rc.1

Released 2026-Jan-13

* Remove support for the `OTEL_SEMCONV_STABILITY_OPT_IN` configuration option.
  Previously, the old conventions were emitted by default with the option to emit
  the stable conventions. Now, only the stable conventions are supported.
  ([#3592](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3592))

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

* Make additional attributes available on `Activity` start. The full list of
  attributes available at time of sampling now conforms with the
  [semantic conventions](https://github.com/open-telemetry/semantic-conventions/blob/v1.33.0/docs/database/database-spans.md#span-definition).
  ([#3675](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3675))

* Metrics and spans report telemetry schema URL v1.33.0.
  ([#3680](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3680))

## 1.14.0-beta.1

Released 2025-Nov-13

* Add support for .NET 10.0.
  ([#2822](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2822))

* Update .NET 10.0 NuGet package versions from `10.0.0-rc.2.25502.107` to `10.0.0`.
  ([#3403](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3403))

* Updated OpenTelemetry core component version(s) to `1.14.0`.
  ([#3403](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3403))

## 1.13.0-beta.2

Released 2025-Nov-03

* Fix diacritic identifier parsing hang and improve FROM clause parsing.
([#3368](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3368))

## 1.13.0-beta.1

Released 2025-Oct-22

* Updated OpenTelemetry core component version(s) to `1.13.1`.
  ([#3218](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3218))

## 1.12.0-beta.3

Released 2025-Sep-25

* Add `db.query.parameter.<key>` attribute(s) to query spans if opted into using
  the `OTEL_DOTNET_EXPERIMENTAL_SQLCLIENT_ENABLE_TRACE_DB_QUERY_PARAMETERS`
  environment variable. Not supported on .NET Framework.
  ([#3015](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3015),
  [#3081](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3081))

* Fix activities not being stopped on .NET Framework when using a global activity
  listener.
  ([#3041](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3041))

* **Breaking change**: The `SetDbStatementForText` property has been removed.
  Behaviors related to this option are now always enabled.
  ([#3072](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3072))

* **Breaking change**: The `Enrich`, `Filter` and `RecordException` properties have
  been removed for .NET Framework where they were non-functional.
  ([#3079](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3079))

* **Breaking change**: The `Enrich` property has been renamed to
  `EnrichWithSqlCommand` and no longer passes an event name to the delegate.
  ([#3080](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3080))

## 1.12.0-beta.2

Released 2025-Jul-15

* Add the `db.operation.name` attribute when `CommandType` is
  `StoredProcedure` to conform to the new semantic conventions.
  This affects you if you have opted into the new conventions.
  ([#2800](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2800))

* Add the `db.query.summary` attribute. This affects you if you have opted into
  the new conventions.
  ([#2811](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2811))

* Added `OTEL_DOTNET_EXPERIMENTAL_SQLCLIENT_ENABLE_TRACE_CONTEXT_PROPAGATION`
  environment variable to propagate trace context to SQL Server databases.
  This will remain experimental while the [specification](https://github.com/open-telemetry/semantic-conventions/blob/main/docs/database/sql-server.md#context-propagation)
  remains in development.
  It is now only available on .NET 8 and newer.
  ([#2709](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2709))

  > Propagate `traceparent` information to SQL Server databases
    (see [SET CONTEXT_INFO](https://learn.microsoft.com/en-us/sql/t-sql/statements/set-context-info-transact-sql?view=sql-server-ver16)).
    Note that this option incurs an additional round-trip to the database.

## 1.12.0-beta.1

Released 2025-May-06

* Fix issue where IPv6 addresses were improperly parsed from the the
  connection's `DataSource` when used to populate the `server.address`
  attribute.
  ([#2674](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2674))

* Updates have been made to adhere to recent changes in the new semantic
  conventions. These affect you if you have opted in to the new conventions.
  When `CommandType` is `StoredProcedure`, the `db.stored_procedure.name` has
  been added and the `db.query.text`, `db.operation.name`, and
  `db.collection.name` attributes have been removed.
  ([#2693](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2693))

* Updated OpenTelemetry core component version(s) to `1.12.0`.
  ([#2725](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2725))

* Fixes an issue that throws `IndexOutOfRangeException` in `SqlProcessor` when the
  SQL statement ends with the beginning of a keyword such as `UPDATE`.
  ([#2674](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2674))

## 1.11.0-beta.2

Released 2025-Mar-05

* Updated OpenTelemetry core component version(s) to `1.11.2`.
  ([#2582](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2582))

## 1.11.0-beta.1

Released 2025-Jan-27

* **Breaking change** The `EnableConnectionLevelAttributes` option has been removed.
  ([#2414](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2414))

* The `db.client.operation.duration` histogram (measured in seconds) produced by
  the metrics instrumentation in this package now uses the [Advice API](https://github.com/open-telemetry/opentelemetry-dotnet/blob/core-1.10.0/docs/metrics/customizing-the-sdk/README.md#explicit-bucket-histogram-aggregation)
  to set default explicit buckets following the [OpenTelemetry Specification](https://github.com/open-telemetry/semantic-conventions/blob/v1.29.0/docs/database/database-metrics.md).
  ([#2430](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2430))

* Enabling `SetDbStatementForText` will no longer capture the raw query text.
  The query is now sanitized. Literal values in the query text are replaced
  by a `?` character.
  ([#2446](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2446))

* Updated OpenTelemetry core component version(s) to `1.11.1`.
  ([#2477](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2477))

## 1.10.0-beta.1

Released 2024-Dec-09

* Drop support for .NET 6 as this target is no longer supported.
  ([#2159](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2159))

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
  ([#2229](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2229),
   [#2277](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2277),
   [#2262](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2262),
   [#2279](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2279))

* **Breaking change**: The `peer.service` and `server.socket.address` attributes
  are no longer emitted. Users should rely on the `server.address` attribute
  for the same information. Note that `server.address` is only included when
  the `EnableConnectionLevelAttributes` option is enabled.
  ([#2229](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2229))

* **Breaking change**: When `EnableConnectionLevelAttributes` is enabled, the
  `server.port` attribute will now be written as an integer to be compliant with
  the [semantic conventions](https://github.com/open-telemetry/semantic-conventions/blob/main/docs/attributes-registry/server.md).
  Previously, it was written as a string.
  ([#2233](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2233))

* The `EnableConnectionLevelAttributes` option is now enabled by default.
  ([#2249](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2249))

* The following attributes are now provided when starting an activity for a database
  call: `db.system`, `db.name` (old conventions), `db.namespace` (new conventions),
  `server.address`, and `server.port`. These attributes are now available for sampling
  decisions.
  ([#2277](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2277))

* **Breaking change**: The `SetDbStatementForStoredProcedure` option has been removed.
  ([#2284](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2284))

* Add support for metric `db.client.operation.duration`
  from [new database semantic conventions](https://github.com/open-telemetry/semantic-conventions/blob/v1.28.0/docs/database/database-metrics.md#metric-dbclientoperationduration)
  on .NET 8+.
  ([#2309](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2309))

* Add support for metric `db.client.operation.duration`
  from [new database semantic conventions](https://github.com/open-telemetry/semantic-conventions/blob/v1.28.0/docs/database/database-metrics.md#metric-dbclientoperationduration)
  on .NET Framework.
  ([#2311](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2311))
  * Only the following attributes are available when a trace is not captured:
  `db.system`, `db.response.status_code`, and `error.type`

* Updated OpenTelemetry core component version(s) to `1.10.0`.
  ([#2317](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2317))

## 1.9.0-beta.1

Released 2024-Jun-17

* `ActivitySource.Version` is set to NuGet package version.
  ([#5498](https://github.com/open-telemetry/opentelemetry-dotnet/pull/5498))

* Updated OpenTelemetry core component version(s) to `1.9.0`.
  ([#1888](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1888))

## 1.8.0-beta.1

Released 2024-Apr-04

## 1.7.0-beta.1

Released 2024-Feb-09

* Removed support for the `OTEL_SEMCONV_STABILITY_OPT_IN` environment variable
  which toggled the use of the new conventions for the
  [server, client, and shared network attributes](https://github.com/open-telemetry/semantic-conventions/blob/v1.23.0/docs/general/attributes.md#server-client-and-shared-network-attributes).
  Now that this suite of attributes are stable, this instrumentation will only
  emit the new attributes.
  ([#5270](https://github.com/open-telemetry/opentelemetry-dotnet/pull/5270))

* **Breaking Change**: Renamed `SqlClientInstrumentationOptions` to
  `SqlClientTraceInstrumentationOptions`.
  ([#5285](https://github.com/open-telemetry/opentelemetry-dotnet/pull/5285))

* **Breaking Change**: Stop emitting `db.statement_type` attribute.
  This attribute was never a part of the [semantic conventions](https://github.com/open-telemetry/semantic-conventions/blob/v1.24.0/docs/database/database-spans.md#call-level-attributes).
  ([#5301](https://github.com/open-telemetry/opentelemetry-dotnet/pull/5301))

## 1.6.0-beta.3

Released 2023-Nov-17

* Updated `Microsoft.Extensions.Configuration` and
  `Microsoft.Extensions.Options` package version to `8.0.0`.
  ([#5051](https://github.com/open-telemetry/opentelemetry-dotnet/pull/5051))

## 1.6.0-beta.2

Released 2023-Oct-26

## 1.5.1-beta.1

Released 2023-Jul-20

* The new network semantic conventions can be opted in to by setting
  the `OTEL_SEMCONV_STABILITY_OPT_IN` environment variable. This allows for a
  transition period for users to experiment with the new semantic conventions
  and adapt as necessary. The environment variable supports the following
  values:
  * `http` - emit the new, frozen (proposed for stable) networking
  attributes, and stop emitting the old experimental networking
  attributes that the instrumentation emitted previously.
  * `http/dup` - emit both the old and the frozen (proposed for stable)
  networking attributes, allowing for a more seamless transition.
  * The default behavior (in the absence of one of these values) is to continue
  emitting the same network semantic conventions that were emitted in
  `1.5.0-beta.1`.
  * Note: this option will eventually be removed after the new
  network semantic conventions are marked stable. Refer to the
  specification for more information regarding the new network
  semantic conventions for
  [spans](https://github.com/open-telemetry/semantic-conventions/blob/v1.21.0/docs/database/database-spans.md).
  ([#4644](https://github.com/open-telemetry/opentelemetry-dotnet/pull/4644))

## 1.5.0-beta.1

Released 2023-Jun-05

* Bumped the package version to `1.5.0-beta.1` to keep its major and minor
  version in sync with that of the core packages. This would make it more
  intuitive for users to figure out what version of core packages would work
  with a given version of this package. The pre-release identifier has also been
  changed from `rc` to `beta` as we believe this more accurately reflects the
  status of this package. We believe the `rc` identifier will be more
  appropriate as semantic conventions reach stability.

## 1.0.0-rc9.14

Released 2023-Feb-24

* Updated OpenTelemetry.Api.ProviderBuilderExtensions dependency to 1.4.0

## 1.4.0-rc9.13

Released 2023-Feb-10

## 1.0.0-rc9.12

Released 2023-Feb-01

## 1.0.0-rc9.11

Released 2023-Jan-09

## 1.0.0-rc9.10

Released 2022-Dec-12

* **Breaking change**: The same API is now exposed for `net462` and
  `netstandard2.0` targets. `SetDbStatement` has been removed. Use
  `SetDbStatementForText` to capture command text and stored procedure names on
  .NET Framework. Note: `Enrich`, `Filter`, `RecordException`, and
  `SetDbStatementForStoredProcedure` options are NOT supported on .NET
  Framework.
  ([#3900](https://github.com/open-telemetry/opentelemetry-dotnet/pull/3900))

* Added overloads which accept a name to the `TracerProviderBuilder`
  `AddSqlClientInstrumentation` extension to allow for more fine-grained options
  management
  ([#3994](https://github.com/open-telemetry/opentelemetry-dotnet/pull/3994))

## 1.0.0-rc9.9

Released 2022-Nov-07

## 1.0.0-rc9.8

Released 2022-Oct-17

* Use `Activity.Status` and `Activity.StatusDescription` properties instead of
  `OpenTelemetry.Trace.Status` and `OpenTelemetry.Trace.Status.Description`
  respectively to set activity status.
  ([#3118](https://github.com/open-telemetry/opentelemetry-dotnet/issues/3118))
  ([#3751](https://github.com/open-telemetry/opentelemetry-dotnet/pull/3751))

* Add support for Filter option for non .NET Framework Targets
  ([#3743](https://github.com/open-telemetry/opentelemetry-dotnet/pull/3743))

## 1.0.0-rc9.7

Released 2022-Sep-29

## 1.0.0-rc9.6

Released 2022-Aug-18

## 1.0.0-rc9.5

Released 2022-Aug-02

* Update the `ActivitySource.Name` from "OpenTelemetry.SqlClient" to
  "OpenTelemetry.Instrumentation.SqlClient".
  ([#3435](https://github.com/open-telemetry/opentelemetry-dotnet/issues/3435))

## 1.0.0-rc9.4

Released 2022-Jun-03

## 1.0.0-rc9.3

Released 2022-Apr-15

* Removes .NET Framework 4.6.1. The minimum .NET Framework version supported is
  .NET 4.6.2.
  ([#3190](https://github.com/open-telemetry/opentelemetry-dotnet/issues/3190))

## 1.0.0-rc9.2

Released 2022-Apr-12

## 1.0.0-rc9.1

Released 2022-Mar-30

## 1.0.0-rc10 (broken. use 1.0.0-rc9.1 and newer)

Released 2022-Mar-04

## 1.0.0-rc9

Released 2022-Feb-02

## 1.0.0-rc8

Released 2021-Oct-08

* Removes .NET Framework 4.5.2 support. The minimum .NET Framework version
  supported is .NET 4.6.1.
  ([#2138](https://github.com/open-telemetry/opentelemetry-dotnet/issues/2138))

## 1.0.0-rc7

Released 2021-Jul-12

## 1.0.0-rc6

Released 2021-Jun-25

## 1.0.0-rc5

Released 2021-Jun-09

## 1.0.0-rc4

Released 2021-Apr-23

* Instrumentation modified to depend only on the API.

* Activities are now created with the `db.system` attribute set for usage during
  sampling.
  ([#1979](https://github.com/open-telemetry/opentelemetry-dotnet/pull/1979))

## 1.0.0-rc3

Released 2021-Mar-19

## 1.0.0-rc2

Released 2021-Jan-29

* Microsoft.Data.SqlClient v2.0.0 and higher is now properly instrumented on
  .NET Framework.
  ([#1599](https://github.com/open-telemetry/opentelemetry-dotnet/pull/1599))

* SqlClientInstrumentationOptions API changes: `SetStoredProcedureCommandName`
  and `SetTextCommandContent` have been renamed to
  `SetDbStatementForStoredProcedure` and `SetDbStatementForText`. They are now
  only available on .NET Core. On .NET Framework they are replaced by a single
  `SetDbStatement` property.

* On .NET Framework, "db.statement_type" attribute is no longer set for
  activities created by the instrumentation.

* New setting on SqlClientInstrumentationOptions on .NET Core: `RecordException`
  can be set to instruct the instrumentation to record SqlExceptions as Activity
  [events](https://github.com/open-telemetry/semantic-conventions/blob/main/docs/exceptions/exceptions-spans.md).
  ([#1592](https://github.com/open-telemetry/opentelemetry-dotnet/pull/1592))

## 1.0.0-rc1.1

Released 2020-Nov-17

* SqlInstrumentation sets ActivitySource to activities created outside
  ActivitySource.
  ([#1515](https://github.com/open-telemetry/opentelemetry-dotnet/pull/1515/))

## 0.8.0-beta.1

Released 2020-Nov-5

## 0.7.0-beta.1

Released 2020-Oct-16

* Instrumentation no longer store raw objects like `object` in
  Activity.CustomProperty. To enrich activity, use the Enrich action on the
  instrumentation.
  ([#1261](https://github.com/open-telemetry/opentelemetry-dotnet/pull/1261))

* Span Status is populated as per new spec
  ([#1313](https://github.com/open-telemetry/opentelemetry-dotnet/pull/1313))

## 0.6.0-beta.1

Released 2020-Sep-15

## 0.5.0-beta.2

Released 2020-08-28

* .NET Core SqlClient instrumentation will now add the raw Command object to the
  Activity it creates
  ([#1099](https://github.com/open-telemetry/opentelemetry-dotnet/pull/1099))

* Renamed from `AddSqlClientDependencyInstrumentation` to
  `AddSqlClientInstrumentation`

## 0.4.0-beta.2

Released 2020-07-24

* First beta release

## 0.3.0-beta

Released 2020-07-23

* Initial release
