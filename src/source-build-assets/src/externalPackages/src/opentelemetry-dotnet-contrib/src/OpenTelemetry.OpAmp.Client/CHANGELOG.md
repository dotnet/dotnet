# Changelog

## Unreleased

## 0.1.0-alpha.4

Released 2026-Jan-14

* Add setting to configure the factory used to create `HttpClient` instances
  used for the OpAMP Plain HTTP transport.
  ([#3589](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3589))

* Add support for subscribing and unsubscribing to messages from the OpAMP server.
  ([#3593](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3593))

* Clean up directories and namespaces for public API.
  ([#3612](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3612))

* Expose public `RemoteConfigMessage`.
  ([#3614](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3614))

* Add settings for remote configuration and update advertised capabilities.
  ([#3618](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3618))

* Expose public `CustomCapabilitiesMessage` and `CustomMessageMessage`.
  ([#3681](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3681))

## 0.1.0-alpha.3

Released 2025-Nov-13

* Update .NET 10.0 NuGet package versions from `10.0.0-rc.2.25502.107` to `10.0.0`.
  ([#3403](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3403))

* Updated OpenTelemetry core component version(s) to `1.14.0`.
  ([#3403](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3403))

## 0.1.0-alpha.2

Released 2025-Nov-03

* Drop reference to `System.Collections.Immutable`.
  ([#3154](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3154))

* Add support for .NET 10.0.
  ([#2822](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2822))

## 0.1.0-alpha.1

Released 2025-Sep-23

* Initial release of `OpenTelemetry.OpAmp.Client` project.
  ([#2917](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2917))
* Added support for OpAMP Plain HTTP transport.
  ([#2926](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2926))
* Added support for OpAMP WebSocket transport.
  ([#3064](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3064),
  [#3092](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3092),
  [#3121](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3121))
* Added support for heartbeat messages.
  ([#3095](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3095))
* Added EventSource `OpenTelemetry-OpAmp-Client` for diagnostics.
  ([#3102](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3102))
* Added support for agent identification messages.
  ([#3112](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3112))
