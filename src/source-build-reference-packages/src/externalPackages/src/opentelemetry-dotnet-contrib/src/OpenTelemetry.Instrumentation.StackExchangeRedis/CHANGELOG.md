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

Released 2025-Oct-22

* **Breaking change** Introduce `RedisInstrumentationContext` and use it
  as context for `Filter`
  ([#2977](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2977))

* Removed the `db.redis.flags` attribute from the implementation
  as it is not part of the Semantic Conventions for Database Client Calls.
 ([#2982](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2982))

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
  semantic conventions are marked stable. At which time this
  instrumentation can receive a stable release, and the old database
  semantic conventions will no longer be supported. Refer to the
  specification for more information regarding the new database
  semantic conventions for
  [spans](https://github.com/open-telemetry/semantic-conventions/blob/v1.28.0/docs/database/database-spans.md).
 ([#3084](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3084))

* Updated OpenTelemetry core component version(s) to `1.13.1`.
  ([#3218](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3218))

## 1.12.0-beta.2

Released 2025-Jul-28

* Add support for early filtering of telemetry collection via a `Filter` option
  ([#2804](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2804))

## 1.12.0-beta.1

Released 2025-May-06

* `System.Reflection.Emit.Lightweight` is referenced only by `netstandard2.0`.
  ([#2667](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2667))

* Updated OpenTelemetry core component version(s) to `1.12.0`.
  ([#2725](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2725))

## 1.11.0-beta.2

Released 2025-Mar-05

* Updated OpenTelemetry core component version(s) to `1.11.2`.
  ([#2582](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2582))

## 1.11.0-beta.1

Released 2025-Jan-27

* Rename span network attributes to comply with
  [v1.23.0 of Semantic Conventions for Database Client Calls](https://github.com/open-telemetry/semantic-conventions/blob/release/v1.23.x/docs/database/database-spans.md)
  ([#2468](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2468))

* Updated OpenTelemetry core component version(s) to `1.11.1`.
  ([#2477](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2477))

## 1.10.0-beta.1

Released 2024-Dec-09

* Drop support for .NET 6 as this target is no longer supported and add .NET 8 target.
  ([#2160](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2160))

* Updated OpenTelemetry core component version(s) to `1.10.0`.
  ([#2317](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2317))

## 1.9.0-beta.1

Released 2024-Jul-23

* Add support for instrumenting `IConnectionMultiplexer`
  which is added with service key.
  ([#1885](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1885))

* Update `StackExchange.Redis` version to `2.6.122`, resolving warnings about
  [CVE-2021-24112](https://github.com/advisories/GHSA-rxg9-xrhp-64gj).
  ([#1961](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1961))

## 1.0.0-rc9.15

Released 2024-Jun-18

* Update `Microsoft.Extensions.Options` to `8.0.0`.
  ([#1830](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1830))

* Updated OpenTelemetry core component version(s) to `1.9.0`.
  ([#1888](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1888))

## 1.0.0-rc9.14

Released 2024-Apr-05

* Update `OpenTelemetry.Api.ProviderBuilderExtensions` version to `1.8.0`.
  ([#1635](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1635))

* `ActivitySource.Version` is set to NuGet package version.
  ([#1624](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1624))

## 1.0.0-rc9.13

Released 2024-Jan-03

* Update `OpenTelemetry.Api.ProviderBuilderExtensions` version to `1.7.0`.
  ([#1486](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1486))

## 1.0.0-rc9.12

Released 2023-Nov-01

* Fix an issue in the trimming annotations to refer to the correct Type
  ([#1420](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1420))

## 1.0.0-rc9.11

Released 2023-Oct-31

* Update `OpenTelemetry.Api.ProviderBuilderExtensions` version to `1.6.0`.
  ([#1344](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1344))

* Add `net6.0` target framework and make library AOT and trimming compatible
  ([#1415](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1415))

## 1.0.0-rc9.10

Released 2023-Jun-09

* Update `OpenTelemetry.Api.ProviderBuilderExtensions` version to `1.5.0`.
  ([#1220](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1220))

## 1.0.0-rc9.9

Released 2023-May-25

* Added a dependency on `OpenTelemetry.Api.ProviderBuilderExtensions` and
  updated `TracerProviderBuilder.AddRedisInstrumentation` to support named
  options.
  ([#1183](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1183))

* **\*\*BREAKING CHANGE\*\*** Renamed the
  `StackExchangeRedisCallsInstrumentationOptions` class to
  `StackExchangeRedisInstrumentationOptions`.
  ([#1193](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1193))

* Added a new extension `TracerProviderBuilder.ConfigureRedisInstrumentation`
  which can be used to obtain the `StackExchangeRedisInstrumentation` instance
  in order to dynamically add connections for instrumentation after the
  `TracerProvider` has been created.
  ([#1193](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1193))

* When using named options the name will now be applied to the background thread
  created for each instrumented connection in the format
  `OpenTelemetry.Redis{OPTIONS_NAME_HERE}`.
  ([#1205](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1205))

## 1.0.0-rc9.8

Released 2023-Feb-27

* Update OTel API version to `1.4.0`.
  ([#1038](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1038))

* Added a direct dependency on System.Reflection.Emit.Lightweight which
  previously came transitively through the OpenTelemetry API.
  ([#1038](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1038))

## 1.0.0-rc9.7

Released 2022-Jul-25

* Update the `ActivitySource` name used to the assembly name: `OpenTelemetry.Instrumentation.StackExchangeRedis`.
([#485](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/485))

* Drain thread is marked as background. It allows to close the application
  even if the instrumentation is not disposed.
([#528](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/528))

## 1.0.0-rc9.6

Released 2022-Jun-29

* Added the EnrichActivityWithTimingEvents option to
  StackExchangeRedisCallsInstrumentationOptions to be able to disable adding
  ActivityEvents (Enqueued, Sent, ResponseReceived) for Redis commands to
  Activities since there is no way to clear these after they have been added.
  This defaults to true to maintain current functionality.

## 1.0.0-rc9.5 (source code moved to contrib repo)

Released 2022-Jun-06

* From this version onwards, the source code for this package would be hosted in
  the
  [contrib](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/tree/main/src/OpenTelemetry.Instrumentation.StackExchangeRedis)
  repo. The source code for this package before this version was hosted on the
  [main](https://github.com/open-telemetry/opentelemetry-dotnet/tree/core-1.3.0/src/OpenTelemetry.Instrumentation.StackExchangeRedis)
  repo.

## 1.0.0-rc9.4

Released 2022-Jun-03

## 1.0.0-rc9.3

Released 2022-Apr-15

* Removes .NET Framework 4.6.1. The minimum .NET Framework version supported is
  .NET 4.6.2.
  ([#3190](https://github.com/open-telemetry/opentelemetry-dotnet/issues/3190))

* Bumped minimum required version of `Microsoft.Extensions.Options` to 3.1.0.
  ([#2582](https://github.com/open-telemetry/opentelemetry-dotnet/pull/3196))

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

* Adds SetVerboseDatabaseStatements option to allow setting more detailed
  database statement tag values.

* Adds Enrich option to allow enriching activities from the source profiled
  command objects.

* Removes upper constraint for Microsoft.Extensions.Options dependency.
  ([#2179](https://github.com/open-telemetry/opentelemetry-dotnet/pull/2179))

## 1.0.0-rc7

Released 2021-Jul-12

## 1.0.0-rc6

Released 2021-Jun-25

* `AddRedisInstrumentation` extension will now resolve `IConnectionMultiplexer`
  & `StackExchangeRedisCallsInstrumentationOptions` through DI when
  OpenTelemetry.Extensions.Hosting is in use.
  ([#2110](https://github.com/open-telemetry/opentelemetry-dotnet/pull/2110))

## 1.0.0-rc5

Released 2021-Jun-09

## 1.0.0-rc4

Released 2021-Apr-23

* Activities are now created with the `db.system` attribute set for usage during
  sampling.
  ([#1984](https://github.com/open-telemetry/opentelemetry-dotnet/pull/1984))

## 1.0.0-rc3

Released 2021-Mar-19

## 1.0.0-rc2

Released 2021-Jan-29

## 1.0.0-rc1.1

Released 2020-Nov-17

## 0.8.0-beta.1

Released 2020-Nov-5

## 0.7.0-beta.1

Released 2020-Oct-16

* Span Status is populated as per new spec
  ([#1313](https://github.com/open-telemetry/opentelemetry-dotnet/pull/1313))

## 0.6.0-beta.1

Released 2020-Sep-15

## 0.5.0-beta.2

Released 2020-08-28

## 0.4.0-beta.2

Released 2020-07-24

* First beta release

## 0.3.0-beta

Released 2020-07-23

* Initial release
