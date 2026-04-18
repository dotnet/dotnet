# Changelog

## Unreleased

## 1.15.0-beta.1

Released 2026-Jan-21

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

## 1.13.0-beta.2

Released 2025-Oct-20

* Fixes possible infinite recursion when WCF is hosted in ASP.NET.
  ([#3248](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3248))

## 1.13.0-beta.1

Released 2025-Oct-15

* Added server instrumentation support for `RecordException` option.
  ([#2880](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2880))

* **Breaking changes** Adjust to breaking changes from
  `OpenTelemetry.Instrumentation.AspNet` version `1.13.0-beta.1`.
  Fixing span hierarchy when hosted in ASP.NET.
  ([#3151](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3151))

* Updated OpenTelemetry core component version(s) to `1.13.1`.
  ([#3218](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3218))

## 1.12.0-beta.1

Released 2025-May-06

* Updated OpenTelemetry core component version(s) to `1.12.0`.
  ([#2725](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2725))

* Preservers `IsReadOnly` state of instrumented HTTP Headers.
  ([#2716](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2716))

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

* Drop support for .NET 6 as this target is no longer supported and add .NET 8 target.
  ([#2263](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2263))

* Updated OpenTelemetry core component version(s) to `1.10.0`.
  ([#2317](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2317))

* Added a `RecordException` property to specify if exceptions should be
  recorded (defaults to `false`). This is only supported by client instrumentation.
  ([#2271](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2271))

* Trace instrumentation will now call the [Activity.SetStatus](https://learn.microsoft.com/dotnet/api/system.diagnostics.activity.setstatus)
  API instead of the deprecated OpenTelemetry API package extension when setting
  span status. For details see: [Setting Status](https://github.com/open-telemetry/opentelemetry-dotnet/blob/main/src/OpenTelemetry.Api/README.md#setting-status).
  ([#2358](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2358))

## 1.0.0-rc.18

Released 2024-Oct-28

* Add target for `net6.0` to ensure that non-vulnerable transient
  dependencies are referenced by default for .NET6+.
  ([#2243](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2243))

## 1.0.0-rc.17

Released 2024-Jun-18

* Updated OpenTelemetry core component version(s) to `1.9.0`.
  ([#1888](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1888))

## 1.0.0-rc.16

Released 2024-Apr-05

* `ActivitySource.Version` is set to NuGet package version.
  ([#1624](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1624))

* Update OpenTelemetry SDK version to `1.8.0`.
  ([#1635](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1635))

## 1.0.0-rc.15

Released 2024-Feb-07

* Fixed [GHSA-vh55-786g-wjwj](https://github.com/advisories/GHSA-vh55-786g-wjwj)
  by bumping the
  [System.Security.Cryptography.Xml](https://www.nuget.org/packages/System.Security.Cryptography.Xml)
  package version to `4.7.1` (dependency chain:
  `OpenTelemetry.Instrumentation.Wcf` -> `System.ServiceModel.Primitives` ->
  `System.Private.ServiceModel` -> `System.Security.Cryptography.Xml`).
  ([#1566](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1566))

## 1.0.0-rc.14

Released 2024-Jan-03

* Update OpenTelemetry SDK version to `1.7.0`.
  ([#1486](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1486))

## 1.0.0-rc.13

Released 2023-Oct-30

* Update OpenTelemetry SDK version to `1.6.0`.
  ([#1344](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1344))

* Fixed span hierarchy when hosted in ASP.NET
  ([#1342](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1342))

* **Breaking Change** `TelemetryClientMessageInspector` and `TelemetryDispatchMessageInspector`
  changed from public to internal
  ([#1376](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1376))

* Added support for `IRequestSessionChannel` and `IDuplexChannel` channel shapes
  ([#1374](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1374))

## 1.0.0-rc.12

Released 2023-Aug-30

* Added support for non-SOAP requests.
  ([#1251](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1251))

## 1.0.0-rc.11

Released 2023-Aug-14

* Update OpenTelemetry SDK version to `1.5.1`.
  ([#1255](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1255))

* Client instrumentation implementation moved to lower-level `BindingElement`.
  ([#1247](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1247))

## 1.0.0-rc.10

Released 2023-Jun-09

* Update OpenTelemetry SDK version to `1.5.0`.
  ([#1220](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1220))

## 1.0.0-rc.9

Released 2023-Feb-27

* Update OpenTelemetry SDK version to `1.4.0`.
  ([#1038](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1038))

* Removes `AddWcfInstrumentation` method with default configure parameter.
  ([#928](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/928))

## 1.0.0-rc.8

Released 2022-Dec-28

* Update OpenTelemetry SDK version to `1.3.1`.
  ([#631](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/631))

* Change value `rpc.system` from `wcf` to `dotnet_wcf`.
  ([#837](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/837))

## 1.0.0-rc.7

Released 2022-Aug-23

* Updated OpenTelemetry SDK package version to 1.3.0
  ([#569](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/569))

* Changed activity source name from `OpenTelemetry.WCF`
  to `OpenTelemetry.Instrumentation.Wcf`
  ([#570](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/570))

## 1.0.0-rc.6

Released 2022-Mar-17

* Going forward the NuGet package will be
  [`OpenTelemetry.Instrumentation.Wcf`](https://www.nuget.org/packages/OpenTelemetry.Instrumentation.Wcf).
  Older versions will remain at
  [`OpenTelemetry.Contrib.Instrumentation.Wcf`](https://www.nuget.org/packages/OpenTelemetry.Contrib.Instrumentation.Wcf)
  [(#247)](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/247)

  Migration:

  * In config files update fully qualified references to not use "Contrib" (eg
    `type="OpenTelemetry.Contrib.Instrumentation.Wcf.TelemetryEndpointBehaviorExtensionElement,
    OpenTelemetry.Contrib.Instrumentation.Wcf"` ->
    `type="OpenTelemetry.Instrumentation.Wcf.TelemetryEndpointBehaviorExtensionElement,
    OpenTelemetry.Instrumentation.Wcf"`)

  * In code update namespaces (eg `using
    OpenTelemetry.Contrib.Instrumentation.Wcf` -> `using
    OpenTelemetry.Instrumentation.Wcf`)

* The minimum supported .NET Framework version is now .NET Framework 4.6.2.
  Previous versions will be going out of support in [April
  2022](https://docs.microsoft.com/en-us/lifecycle/products/microsoft-net-framework)
  [(#247)](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/247)

* Fixed a `NullReferenceException` in
  `TelemetryDispatchMessageInspector.BeforeSendReply` when operation is OneWay
  ([#237](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/237))

## 1.0.0-rc5

Released 2022-Feb-05

* Fixed an `ArgumentNullException` setting `Activity`.`DisplayName` when
  processing service requests with empty actions
  ([#170](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/170))

## 1.0.0-rc4

Released 2021-Oct-22

* Removed `Propagator` property on `WcfInstrumentationOptions`
  ([#163](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/163))

## 1.0.0-rc3

Released 2021-Sep-13

* Added `TelemetryServiceBehavior`. **Breaking change** (config update
  required): Renamed `TelemetryBehaviourExtensionElement` ->
  `TelemetryEndpointBehaviorExtensionElement`
  ([#152](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/152))

* Added `TelemetryContractBehaviorAttribute` which can be used for programmatic
  configuration of WCF services & clients
  ([#153](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/153))

## 1.0.0-rc2

Released 2021-Jun-16

* Updated OpenTelemetry SDK package version to 1.1.0-beta1
  ([#100](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/100))

* Added enricher for WCF activity
  ([#126](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/126))
