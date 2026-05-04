# Changelog

## Unreleased

## 1.15.0-beta.1

Released 2026-Jan-21

* Updated OpenTelemetry core component version(s) to `1.15.0`.
  ([#3721](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3721))

## 1.14.0-beta.1

Released 2025-Nov-13

* Renamed `deployment.environment` attribute to `deployment.environment.name`
  in Azure App Service Resource Detector.
  ([#3366](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3366))

* Add support for .NET 10.0.
  ([#2822](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2822))

* Update .NET 10.0 NuGet package versions from `10.0.0-rc.2.25502.107` to `10.0.0`.
  ([#3403](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3403))

* Updated OpenTelemetry core component version(s) to `1.14.0`.
  ([#3403](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3403))

## 1.13.0-beta.1

Released 2025-Oct-22

* Removed azure.vm.sku attribute from Azure VM resource detector.
  ([#2979](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2979))

* Updated OpenTelemetry core component version(s) to `1.13.1`.
  ([#3218](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3218))

## 1.12.0-beta.1

Released 2025-May-06

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

* Drop support for .NET 6 as this target is no longer supported and add .NET 8 target.
  ([#2165](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2165))

* Added direct reference to `System.Text.Json` for the `net8.0` target with
  minimum version of `8.0.5` in response to
  [CVE-2024-43485](https://msrc.microsoft.com/update-guide/vulnerability/CVE-2024-43485).
  ([#2198](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2198))

* Updated OpenTelemetry core component version(s) to `1.10.0`.
  ([#2317](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2317))

## 1.0.0-beta.9

Released 2024-Sep-24

* Added support for [Azure Container Apps jobs](https://learn.microsoft.com/en-us/azure/container-apps/jobs?tabs=azure-cli).
  ([#2064](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2064))

* Added direct reference to `System.Text.Encodings.Web` with minimum version of
  `4.7.2` in response to [CVE-2021-26701](https://github.com/dotnet/runtime/issues/49377).
  ([#2056](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2056))

## 1.0.0-beta.8

Released 2024-Jun-18

* **Breaking Change**: Renamed method from `AddAppServiceDetector`
  to `AddAzureAppServiceDetector`.
  ([#1883](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1883))

* Updated OpenTelemetry core component version(s) to `1.9.0`.
  ([#1888](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1888))

## 1.0.0-beta.7

Released 2024-Jun-04

* **Breaking Change**: Renamed package from `OpenTelemetry.ResourceDetectors.Azure`
  to `OpenTelemetry.Resources.Azure`.
  ([#1840](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1840))

* **Breaking Change**: `AppServiceResourceDetector` type is now internal, use `ResourceBuilder`
  extension method `AddAppServiceDetector` to enable the detector.
  ([#1840](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1840))

* **Breaking Change**: `AzureVMResourceDetector` type is now internal, use `ResourceBuilder`
  extension method `AddAzureVMResourceDetector` to enable the detector.
  ([#1840](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1840))

* **Breaking Change**: `AzureContainerAppsResourceDetector` type is now
  internal, use `ResourceBuilder` extension method `AddAzureContainerAppsResourceDetector`
  to enable the detector.
  ([#1840](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1840))

* Update OpenTelemetry SDK version to `1.8.1`.
  ([#1668](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1668))

## 1.0.0-beta.6

Released 2024-Apr-05

* Update OpenTelemetry SDK version to `1.8.0`.
  ([#1635](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1635))

## 1.0.0-beta.5

Released 2024-Feb-05

* Added Azure Container Apps Resource Detector to generate attributes:
  `service.name`, `service.version`, `service.instance.id`, `cloud.provider` and
  `cloud.platform`.
  ([#1565](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1565))

## 1.0.0-beta.4

Released 2024-Jan-03

* Added NET6 target framework to support Trimming.
  ([#1405](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1405))

* Update OpenTelemetry SDK version to `1.7.0`.
  ([#1486](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1486))

## 1.0.0-beta.3

Released 2023-Sep-19

* Configured the `HttpClient` used for making the call to metadata service to
  use a `Timeout` to `2` seconds. This is to improve the start-up time of
  applications not running on Azure VMs.
  ([#1358](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1358))

* Updates to 1.6.0 of OpenTelemetry SDK.
  ([#1344](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1344))

* Suppress instrumentation for outgoing http call made to metadata service
  during `Detect()`.
  ([#1297](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1297))

## 1.0.0-beta.2

Released 2023-Jul-26

* Downgraded minimum required version for `System.Text.Json` to `4.7.2`.
  ([#1279](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1279))

## 1.0.0-beta.1

Released 2023-Jul-24

* For Azure VM Resource Detector:
  ([#1272](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1272/files))
  * **Updated attributes**: `azInst_vmId` to `host.id`, `azInst_location` to
    `cloud.region`, `azInst_name` to `host.name`, `azInst_osType` to `os.type`,
    `azInst_resourceId` to `cloud.resource_id`, `azInst_sku` to `azure.vm.sku`,
    `azInst_version` to `os.version`, `azInst_vmSize` to `host.type`,
    `azInst_vmScaleSetName` to `azure.vm.scaleset.name`.
  * **Added attributes**: `cloud.provider` and `cloud.platform`.
  * **Removed attributes**: `azInst_resourceGroupName`, `azInst_subscriptionId`.

* For Azure App Service:
 ([#1272](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1272/files))
  * **Updated attributes**: `appSrv_wsHost` to `host.id`, `appSrv_SlotName` to
    `deployment.environment`, `appSrv_wsStamp` to `azure.app.service.stamp`.
  * **Added attributes**: `cloud.resource_id`, `cloud.provider`,
    `cloud.platform`, `cloud.region`.
  * **Removed attribute**: `appSrv_ResourceGroup`.

* Updates to 1.5.0 of OpenTelemetry SDK.
  ([#1220](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1220))

* Added Azure VM resource detector.
  ([#1182](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1182))

## 1.0.0-alpha.1

Released 2023-Apr-19

* Add AppService resource detector.
  ([#989](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/989))

For more details, please refer to the [README](README.md).
