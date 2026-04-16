# Changelog

## Unreleased

## 1.15.0

Released 2026-Jan-21

* Updated OpenTelemetry core component version(s) to `1.15.0`.
  ([#3721](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3721))

## 1.14.0

Released 2025-Nov-13

## 1.14.0-beta.1

Released 2025-Nov-13

* Add support for .NET 10.0.
  ([#2822](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2822))

* Update .NET 10.0 NuGet package versions from `10.0.0-rc.2.25502.107` to `10.0.0`.
  ([#3403](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3403))

* Updated OpenTelemetry core component version(s) to `1.14.0`.
  ([#3403](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3403))

## 1.13.0

Released 2025-Oct-22

* Updated OpenTelemetry core component version(s) to `1.13.1`.
  ([#3218](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3218))

## 1.12.0

Released 2025-May-05

* Updated OpenTelemetry core component version(s) to `1.12.0`.
  ([#2725](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2725))

## 1.11.1

Released 2025-Mar-05

* Updated OpenTelemetry core component version(s) to `1.11.2`.
  ([#2582](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2582))

## 1.11.0

Released 2025-Jan-27

* Updated OpenTelemetry core component version(s) to `1.11.1`.
  ([#2477](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2477))

## 1.10.0

Released 2024-Dec-05

* Drop support for .NET 6 as this target is no longer supported and add .NET 8 target.
  ([#2155](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2155)),
  ([#2325](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2325))

* Updated OpenTelemetry core component version(s) to `1.10.0`.
  ([#2317](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2317))

* Built-in .NET `System.Runtime` metrics are reported for .NET 9 and greater.
  For details about each individual metric check [.NET Runtime metrics docs page](https://learn.microsoft.com/en-us/dotnet/core/diagnostics/built-in-metrics-runtime).
  ([#2339](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2339))

## 1.9.0

Released 2024-Jun-18

* Updated OpenTelemetry core component version(s) to `1.9.0`.
  ([#1888](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1888))

## 1.8.1

Released 2024-May-20

* Update `OpenTelemetry.Api` to `1.8.1`.
  ([#1668](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1668))

## 1.8.0

Released 2024-Apr-05

* `Meter.Version` is set to NuGet package version.
  ([#1624](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1624))

* Update `OpenTelemetry.Api` to `1.8.0`.
  ([#1635](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1635))

## 1.7.0

Released 2024-Jan-03

* Update `OpenTelemetry.Api` to `1.7.0`.
  ([#1486](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1486))

## 1.5.1

Released 2023-Sep-06

* Add a metric `process.runtime.dotnet.gc.duration` for total paused duration in
  GC for .NET 7 and greater versions
  ([#1239](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1239))

* Update OpenTelemetry API to 1.5.1
  ([#1255](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1255))

## 1.5.0

Released 2023-Jun-06

* Update OpenTelemetry API to 1.5.0
  ([#1220](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1220))

## 1.4.0

Released 2023-Jun-01

* Bumped the version to `1.4.0` to keep it in sync with the release versions of
  `OpenTelemetry.API`. This makes it more intuitive for the users to figure out
  what version of core packages would work with a given version of this package.

## 1.1.0-rc.2

Released 2023-Feb-27

* Update OpenTelemetry API to 1.4.0
  ([#1038](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1038))

## 1.1.0-rc.1

Released 2023-Feb-13

* Update OpenTelemetry API to 1.4.0-rc.4
  ([#990](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/990))

## 1.1.0-beta.4

Released 2023-Feb-02

* Update OpenTelemetry API to 1.4.0-rc.3
  ([#944](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/944))

## 1.1.0-beta.3

Released 2023-Jan-11

* Update OpenTelemetry API to 1.4.0-rc.2
  ([#880](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/880))

## 1.1.0-beta.2

Released 2022-Dec-13

* Update OpenTelemetry API to 1.4.0-rc.1
  ([#820](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/820))

## 1.1.0-beta.1

Released 2022-Nov-22

* Update OpenTelemetry API to 1.4.0-beta.3
  ([#774](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/774))

* Change ObservableGauge to ObservableUpDownCounter for the below metrics (which
  better fit UpDownCounter semantics as they are additive.)

  "process.runtime.dotnet.gc.heap.size",
  "process.runtime.dotnet.gc.heap.fragmentation.size",
  "process.runtime.dotnet.thread_pool.threads.count",
  "process.runtime.dotnet.thread_pool.queue.length",
  "process.runtime.dotnet.timer.count",
  "process.runtime.dotnet.assemblies.count"
  ([#675](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/675))

  If your backend system distinguishes between ObservableUpDownCounter and
  ObservableGauge, then you may need to adjust your queries. Systems like
  Prometheus are unaffected by this.

* Removes NETCoreApp3.1 target as .NET Core 3.1 and .NET 5 are going out of
  support. The package keeps `netstandard2.0` target, so it can still be used
  with .NET Core 3.1/.NET 5 apps, however certain metrics will not be available
  there. Additionally, apps targeting .NET 5 and lower will receive a warning at
  build time as described [here](https://github.com/dotnet/runtime/pull/72518)
  (note: building using older versions of the .NET SDK produces an error at
  build time). This is because .NET 5 reached EOL in May 2022 and .NET Core 3.1
  reaches EOL in December 2022.

  The build warning can be suppressed by setting the
  SuppressTfmSupportBuildWarnings MSBuild property, but there is no guarantee
  that this package will continue to work on older versions of .NET.

  This does not affect applications targeting .NET Framework.

* Add "process.runtime.dotnet.gc.objects.size" metric
  ([#683](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/683))

## 1.0.0

Released 2022-Aug-03

* Rename `RuntimeInstrumentOptions` to `RuntimeInstrumentationOptions`
  ([#556](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/556))

## 1.0.0-rc.3

Released 2022-Jul-25

* Add gc.heap.fragmentation.size back for .NET 7 and later
  ([#524](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/524))

## 1.0.0-rc.2

Released 2022-Jul-19

* Refined some metrics names (assembly.count->assemblies.count,
  exception.count-> exceptions.count, attribute name: gen->generation) and
  descriptions
  ([#475](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/475))

* Change API for GC Heap Size for .NET 6 where the API has a bug
  ([#495](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/495))

* Remove gc.heap.fragmentation.size metrics due to buggy API on .NET 6
  ([#509](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/509))

## 1.0.0-rc.1

Released 2022-Jun-29

Major refactor of the runtime instrumentation. Renamed API signature and
metrics. Removed the options to turn off certain metrics.

## 1.0.0-beta.1

Major redesign of the runtime instrumentation. Renamed metrics to be more
user-friendly and better logical grouping. Removed the process related metrics
which are not .NET Runtime specific.

## 0.2.0-alpha.1

* Updated OpenTelemetry SDK package version to 1.3.0
  ([#411](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/411))

* Fix some bugs in Runtime metrics
  ([#409](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/409))

* Add GC heap size and refactor GC count as multi-dimensional metrics in Runtime
  metrics
  ([#412](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/412))

## 0.1.0-alpha.1

* This is the first release of `OpenTelemetry.Instrumentation.Runtime` package.

For more details, please refer to the [README](README.md).
