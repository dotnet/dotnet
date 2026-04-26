# Changelog

## Unreleased

* Metrics and spans report telemetry schema URL v1.36.0.
  ([#3686](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3686))

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

* Improved performance of replacing static tokens with actual values
  in the route template.
  ([#3241](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3241))

## 1.13.0-beta.1

Released 2025-Oct-15

* Following attributes are available while sampling:
  * `http.request.method`,
  * `server.address`,
  * `server.port`,
  * `url.path`,
  * `url.query`,
  * `url.scheme`,
  * `user_agent.original`.
  ([#3151](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3151))

* Replace static routing tokens with actual values in the route template.
  ([#3160](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3160))

* Updated OpenTelemetry core component version(s) to `1.13.1`.
  ([#3218](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3218))

## 1.12.0-beta.2

Released 2025-Sep-18

* **Breaking Change**: Renamed `MeterProviderBuilderExtensions` and
  `TracerProviderBuilderExtensions` to
  `AspNetInstrumentationMeterProviderBuilderExtensions`
  and `AspNetInstrumentationTracerProviderBuilderExtensions` respectively.
  ([#2910](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2910))

* **Breaking Change**: Made metrics generation independent from traces.
  Tracing must no longer be enabled to calculate metrics. A compatible version
  of `OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule` is required.
  ([#2970](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2970))

* **Breaking Change**: Metrics related option renamed:
  * delegate `AspNetMetricsInstrumentationOptions.EnrichFunc` to
    `AspNetMetricsInstrumentationOptions.EnrichWithHttpContextAction`,
  * property `AspNetMetricsInstrumentationOptions.Enrich` to
    `AspNetMetricsInstrumentationOptions.EnrichWithHttpContext`.
  ([#3070](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3070))

* **Breaking Change**: Change in public API contract.
  All usages of `HttpRequest`, `HttpResponse` and `HttpContext` replaced by
 `HttpRequestBase`, `HttpResponseBase` and `HttpContextBase` respectively.
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

* The `http.server.request.duration` histogram (measured in seconds) produced by
  the metrics instrumentation in this package now uses the [Advice API](https://github.com/open-telemetry/opentelemetry-dotnet/blob/core-1.10.0/docs/metrics/customizing-the-sdk/README.md#explicit-bucket-histogram-aggregation)
  to set default explicit buckets following the [OpenTelemetry Specification](https://github.com/open-telemetry/semantic-conventions/blob/v1.29.0/docs/http/http-metrics.md).
  ([#2430](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2430))

* Updated OpenTelemetry core component version(s) to `1.11.1`.
  ([#2477](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2477))

## 1.10.0-beta.1

Released 2024-Dec-09

* Updated registration extension code to retrieve environment variables through
  `IConfiguration`.
  ([#1976](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1976))

* Updated OpenTelemetry core component version(s) to `1.10.0`.
  ([#2317](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2317))

* Fixed an issue in ASP.NET instrumentation where route extraction failed for
  attribute-based routing with multiple HTTP methods sharing the same route template.
  ([#2250](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2250))

## 1.9.0-beta.1

Released 2024-Jun-18

## 1.8.0-beta.3

Released 2024-May-23

* **Breaking change** The `Enrich` callback option has been removed.
  For better usability, it has been replaced by three separate options:
  `EnrichWithHttpRequest`, `EnrichWithHttpResponse` and `EnrichWithException`.
  Previously, the single `Enrich` callback required the consumer to detect
  which event triggered the callback to be invoked (e.g., request start,
  response end, or an exception) and then cast the object received to the
  appropriate type: `HttpRequest`, `HttpResponse`, or `Exception`. The separate
  callbacks make it clear what event triggers them and there is no longer the
  need to cast the argument to the expected type.
  ([#1824](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1824))

## 1.8.0-beta.2

Released 2024-Apr-17

* **Breaking Change**: Fixed tracing instrumentation so that by default any
  values detected in the query string component of requests are replaced with
  the text `Redacted` when building the `url.query` attribute. For example,
  `?key1=value1&key2=value2` becomes `?key1=Redacted&key2=Redacted`. You can
  disable this redaction by setting the environment variable
  `OTEL_DOTNET_EXPERIMENTAL_ASPNET_DISABLE_URL_QUERY_REDACTION` to `true`.
  ([#1656](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1656))

## 1.8.0-beta.1

Released 2024-Apr-05

* **Breaking Change**: Renamed `AspNetInstrumentationOptions` to
  `AspNetTraceInstrumentationOptions`.
  ([#1604](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1604))

* **Breaking Change**: `server.address` and `server.port` no longer added
  for `http.server.request.duration` metric.
  ([#1606](https://github.com/open-telemetry/opentelemetry-dotnet/pull/1606))

* **Breaking change** Spans names and attributes
 will be set as per [HTTP semantic convention v1.24.0](https://github.com/open-telemetry/semantic-conventions/blob/v1.24.0/docs/http/http-spans.md):
  * span names follows: `{HTTP method} [route name if available]` pattern
  * `error.type` added when exception occurred while processing request,
  * `http.request.method` replaces `http.method`,
  * `http.request.method_original` added when `http.request.method` is not in
    canonical form,
  * `http.response.status_code` replaces `http.status_code`,
  * `network.protocol.version` added with HTTP version value,
  * `server.address` and `server.port` replace `http.host`,
  * `url.path` replaces `http.target`,
  * `url.query` added when query url part is not empty,
  * `url.scheme` added with `http` or `https` value,
  * `user_agent.original` replaces `http.user_agent`.
  ([#1607](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1607))

* `ActivitySource.Version` is set to NuGet package version.
  ([#1624](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1624))

## 1.7.0-beta.2

Released 2024-Feb-07

* Fix description for `http.server.request.duration` metric.
  ([#1538](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1538))

## 1.7.0-beta.1

Released 2023-Dec-20

* Added enrich functionality to metric instrumentation
  ([#1407](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1407)).

  * New overload of `AddAspNetInstrumentation` now accepts a configuration delegate.
  * The `Enrich` can be used to add additional metric attributes.

* BREAKING: HTTP server metrics now follow stable
  [semantic conventions](https://github.com/open-telemetry/semantic-conventions/blob/v1.23.0/docs/http/http-metrics.md#http-server)
  ([#1429](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1429)).

  * New metric: `http.server.request.duration`
    * Unit: `s` (seconds)
    * Histogram Buckets: `0.005, 0.01, 0.025, 0.05, 0.075, 0.1, 0.25, 0.5,
    0.75, 1,  2.5, 5, 7.5, 10`
  * Old metric: `http.server.duration`
    * Unit: `ms` (milliseconds)
    * Histogram Buckets: `0, 5, 10, 25, 50, 75, 100, 250, 500, 750, 1000, 2500,
    5000, 7500, 10000`

  Note that the bucket changes are part of the 1.7.0-rc.1 release of the
  `OpenTelemetry` SDK.

  The following metric attributes has been added:

  * `http.request.method` (previously `http.method`)
  * `http.response.status_code` (previously `http.status_code`)
  * `url.scheme` (previously `http.scheme`)
  * `server.address`
  * `server.port`
  * `network.protocol.version` (`1.1`, `2`, `3`)
  * `http.route`

## 1.6.0-beta.2

Released 2023-Nov-06

* Fixed an issue that caused `http.server.duration` metric value to always be set
  to `0`. The issue exists in `1.6.0-beta.1` version.
  ([#1425](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1425))

## 1.6.0-beta.1

Released 2023-Oct-11

* Updated `OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule` dependency
  to `1.6.0-beta.1` which brings in the following changes.:

  * Fixed an issue where activities were stopped incorrectly before processing completed.
    Activity processor's `OnEnd` will now happen after `AspNetInstrumentationOptions.Enrich`.
    ([#1388](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1388))
  * Update `OpenTelemetry.Api` to `1.6.0`.
    ([#1344](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1344))

## 1.0.0-rc9.9

Released 2023-Jun-09

* Release together with `OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule`
  due to update `OpenTelemetry.Api` to `1.5.0`.
  ([#1220](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1220))

## 1.0.0-rc9.8

Released 2023-Feb-27

* Removes `AddAspNetInstrumentation` method with default configure parameter.
  ([#942](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/942))

## 1.0.0-rc9.7

Released 2022-Nov-28

## 1.0.0-rc9.6

Released 2022-Sep-28

* Migrate to native Activity `Status` and `StatusDescription`.
  ([#651](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/651))

## 1.0.0-rc9.5 (source code moved to contrib repo)

Released 2022-Jun-21

* From this version onwards, the source code for this package would be hosted in
  the
  [contrib](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/tree/main/src/OpenTelemetry.Instrumentation.AspNet)
  repo. The source code for this package before this version was hosted on the
  [main](https://github.com/open-telemetry/opentelemetry-dotnet/tree/core-1.3.0/src/OpenTelemetry.Instrumentation.AspNet)
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

* Added ASP.NET metrics instrumentation to collect `http.server.duration`.
  ([#2985](https://github.com/open-telemetry/opentelemetry-dotnet/pull/2985))

* Fix: Http server span status is now unset for `400`-`499`.
  ([#2904](https://github.com/open-telemetry/opentelemetry-dotnet/pull/2904))

## 1.0.0-rc10 (broken. use 1.0.0-rc9.1 and newer)

Released 2022-Mar-04

## 1.0.0-rc9

Released 2022-Feb-02

## 1.0.0-rc8

Released 2021-Oct-08

* Removes .NET Framework 4.5.2, .NET 4.6 support. The minimum .NET Framework
  version supported is .NET 4.6.1.
  ([#2138](https://github.com/open-telemetry/opentelemetry-dotnet/issues/2138))

* Replaced `http.path` tag on activity with `http.target`.
  ([#2266](https://github.com/open-telemetry/opentelemetry-dotnet/pull/2266))

* ASP.NET instrumentation now uses
  [OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule](https://www.nuget.org/packages/OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule/)
  instead of
  [Microsoft.AspNet.TelemetryCorrelation](https://www.nuget.org/packages/Microsoft.AspNet.TelemetryCorrelation/)
  to listen for incoming http requests to the process. Please see the (Step 2:
  Modify
  Web.config)[https://github.com/open-telemetry/opentelemetry-dotnet-contrib/tree/main/src/OpenTelemetry.Instrumentation.AspNet#step-2-modify-webconfig]
  README section for details on the new HttpModule definition required.
  ([#2222](https://github.com/open-telemetry/opentelemetry-dotnet/issues/2222))

* Added `RecordException` option. Specify `true` to have unhandled exception
  details automatically captured on spans.
  ([#2256](https://github.com/open-telemetry/opentelemetry-dotnet/pull/2256))

## 1.0.0-rc7

Released 2021-Jul-12

## 1.0.0-rc6

Released 2021-Jun-25

## 1.0.0-rc5

Released 2021-Jun-09

## 1.0.0-rc4

Released 2021-Apr-23

* Sanitize `http.url` attribute.
  ([#1961](https://github.com/open-telemetry/opentelemetry-dotnet/pull/1961))

## 1.0.0-rc3

Released 2021-Mar-19

* Leverages added AddLegacySource API from OpenTelemetry SDK to trigger Samplers
  and ActivityProcessors. Samplers, ActivityProcessor.OnStart will now get the
  Activity before any enrichment done by the instrumentation.
  ([#1836](https://github.com/open-telemetry/opentelemetry-dotnet/pull/1836))

* Performance optimization by leveraging sampling decision and short circuiting
  activity enrichment.
  ([#1903](https://github.com/open-telemetry/opentelemetry-dotnet/pull/1903))

## 1.0.0-rc2

Released 2021-Jan-29

## 1.0.0-rc1.1

Released 2020-Nov-17

* AspNetInstrumentation sets ActivitySource to activities created outside
  ActivitySource.
  ([#1515](https://github.com/open-telemetry/opentelemetry-dotnet/pull/1515/))

## 0.8.0-beta.1

Released 2020-Nov-5

* Renamed TextMapPropagator to TraceContextPropagator, CompositePropagator to
  CompositeTextMapPropagator. IPropagator is renamed to TextMapPropagator and
  changed from interface to abstract class.
  ([#1427](https://github.com/open-telemetry/opentelemetry-dotnet/pull/1427))

* Propagators.DefaultTextMapPropagator will be used as the default Propagator.
  ([#1427](https://github.com/open-telemetry/opentelemetry-dotnet/pull/1428))

* Removed Propagator from Instrumentation Options. Instrumentation now always
  respect the Propagator.DefaultTextMapPropagator.
  ([#1448](https://github.com/open-telemetry/opentelemetry-dotnet/pull/1448))

## 0.7.0-beta.1

Released 2020-Oct-16

* Instrumentation no longer store raw objects like `HttpRequest` in
  Activity.CustomProperty. To enrich activity, use the Enrich action on the
  instrumentation.
  ([#1261](https://github.com/open-telemetry/opentelemetry-dotnet/pull/1261))

* Span Status is populated as per new spec
  ([#1313](https://github.com/open-telemetry/opentelemetry-dotnet/pull/1313))

## 0.6.0-beta.1

Released 2020-Sep-15

## 0.5.0-beta.2

Released 2020-08-28

* Added Filter public API on AspNetInstrumentationOptions to allow filtering of
  instrumentation based on HttpContext.

* Asp.Net Instrumentation automatically populates HttpRequest, HttpResponse in
  Activity custom property

* Changed the default propagation to support W3C Baggage
  ([#1048](https://github.com/open-telemetry/opentelemetry-dotnet/pull/1048))
  * The default ITextFormat is now `CompositePropagator(TraceContextFormat,
    BaggageFormat)`. Baggage sent via the [W3C
    Baggage](https://github.com/w3c/baggage/blob/master/baggage/HTTP_HEADER_FORMAT.md)
    header will now be parsed and set on incoming Http spans.

* Renamed `ITextPropagator` to `IPropagator`
  ([#1190](https://github.com/open-telemetry/opentelemetry-dotnet/pull/1190))

## 0.4.0-beta.2

Released 2020-07-24

* First beta release

## 0.3.0-beta

Released 2020-07-23

* Initial release
