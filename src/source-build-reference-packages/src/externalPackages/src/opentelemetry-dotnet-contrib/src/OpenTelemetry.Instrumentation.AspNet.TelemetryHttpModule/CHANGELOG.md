# Changelog

## Unreleased

* Updated OpenTelemetry core component version(s) to `1.15.0`.
  ([#3721](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3721))

## 1.14.0

Released 2025-Nov-27

## 1.14.0-rc.1

Released 2025-Nov-13

* Updated OpenTelemetry core component version(s) to `1.14.0`.
  ([#3403](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3403))

## 1.13.0-rc.1

Released 2025-Oct-28

## 1.13.0-beta.2

Released 2025-Oct-22

## 1.13.0-beta.1

Released 2025-Oct-15

* **Breaking Change**: This module is no longer responsible for creating activities.
  The contract of `TelemetryHttpModuleOptions.OnRequestStartedCallback` was changed
  to `Func<HttpContextBase, ActivityContext, Activity?>?`. The consumer is now
  responsible for providing function returning `Activity`.
  ([#3151](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3151))

* Fixed an inaccurate span end of time.
  ([#3171](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3171))

* Updated OpenTelemetry core component version(s) to `1.13.1`.
  ([#3218](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3218))

## 1.12.0-beta.2

Released 2025-Sep-18

* **Breaking Change**: Modified request lifecycle callbacks to always fire.
  This is required as part of making ASP.NET metrics generation
  independent from traces.
  ([#2970](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2970))

* **Breaking Change**: Activity source name changed from
  `OpenTelemetry.Instrumentation.AspNet.Telemetry` to
  `OpenTelemetry.Instrumentation.AspNet`.
  ([#3071](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3071))

* **Breaking Change**: Following constants was removed from the public API
  * `TelemetryHttpModule.AspNetActivityName`,
  * `TelemetryHttpModule.AspNetSourceName`.
  ([#3071](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3071))

* **Breaking Change**: Change in public API contract.
  All usages of `HttpContext` replaced by `HttpContextBase`.
  ([#3110](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3110))

## 1.12.0-beta.1

Released 2025-May-05

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

* `TelemetryHttpModule` will now pass the `url.path` tag (set to
  [Request.Unvalidated.Path](https://learn.microsoft.com/dotnet/api/system.web.unvalidatedrequestvalues.path))
  when starting `Activity` instances for incoming requests so that it is
  available to samplers and may be used to influence the sampling decision made
  by [custom
  implementations](https://github.com/open-telemetry/opentelemetry-dotnet/tree/main/docs/trace/extending-the-sdk#sampler).
  ([#1871](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1871))

* Updated OpenTelemetry core component version(s) to `1.10.0`.
  ([#2317](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2317))

## 1.9.0-beta.1

Released 2024-Jun-18

* Updated OpenTelemetry core component version(s) to `1.9.0`.
  ([#1888](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1888))

## 1.8.0-beta.1

Released 2024-Apr-05

* `Meter.Version` is set to NuGet package version.
  ([#1624](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1624))

* Update `OpenTelemetry.Api` to `1.8.0`.
  ([#1635](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1635))

## 1.7.0-beta.2

Released 2024-Feb-07

## 1.7.0-beta.1

Released 2023-Dec-20

* Update `OpenTelemetry.Api` to `1.7.0`.
  ([#1486](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1486))

## 1.6.0-beta.2

Released 2023-Nov-06

## 1.6.0-beta.1

Released 2023-Oct-11

* Fixed an issue where activities were stopped incorrectly before processing completed.
  Activity processor's `OnEnd` will now happen after `AspNetInstrumentationOptions.Enrich`.
  ([#1388](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1388))

* Update `OpenTelemetry.Api` to `1.6.0`.
  ([#1344](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1344))

## 1.0.0-rc9.9

Released 2023-Jun-09

* Update `OpenTelemetry.Api` to `1.5.0`.
  ([#1220](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1220))

## 1.0.0-rc9.8

Released 2023-Feb-27

* Update `OpenTelemetry.Api` to `1.4.0`.
  ([#1038](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1038))

## 1.0.0-rc9.7

Released 2022-Nov-28

* Restore Activity.Current before all IIS Lifecycle events.
  ([#761](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/761))

## 1.0.0-rc9.6

Released 2022-Sep-28

* Update `OpenTelemetry.Api` to `1.3.1`.
([#665](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/665))

## 1.0.0-rc9.5 (source code moved to contrib repo)

Released 2022-Jun-21

* From this version onwards, the source code for this package would be hosted in
  the
  [contrib](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/tree/main/src/OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule)
  repo. The source code for this package before this version was hosted on the
  [main](https://github.com/open-telemetry/opentelemetry-dotnet/tree/core-1.3.0/src/OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule)
  repo.

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

* Adopted the donation
  [Microsoft.AspNet.TelemetryCorrelation](https://github.com/aspnet/Microsoft.AspNet.TelemetryCorrelation)
  from [.NET Foundation](https://dotnetfoundation.org/).
  ([#2223](https://github.com/open-telemetry/opentelemetry-dotnet/pull/2223))

* Renamed the module, refactored existing code
  ([#2224](https://github.com/open-telemetry/opentelemetry-dotnet/pull/2224)
  [#2225](https://github.com/open-telemetry/opentelemetry-dotnet/pull/2225)
  [#2226](https://github.com/open-telemetry/opentelemetry-dotnet/pull/2226)
  [#2229](https://github.com/open-telemetry/opentelemetry-dotnet/pull/2229)
  [#2231](https://github.com/open-telemetry/opentelemetry-dotnet/pull/2231)
  [#2235](https://github.com/open-telemetry/opentelemetry-dotnet/pull/2235)
  [#2238](https://github.com/open-telemetry/opentelemetry-dotnet/pull/2238)
  [#2240](https://github.com/open-telemetry/opentelemetry-dotnet/pull/2240))

* Updated to use
  [ActivitySource](https://docs.microsoft.com/dotnet/api/system.diagnostics.activitysource)
  & OpenTelemetry.API
  ([#2249](https://github.com/open-telemetry/opentelemetry-dotnet/pull/2249) &
  follow-ups (linked to #2249))

* TelemetryHttpModule will now restore Baggage on .NET 4.7.1+ runtimes when IIS
  switches threads
  ([#2314](https://github.com/open-telemetry/opentelemetry-dotnet/pull/2314))
