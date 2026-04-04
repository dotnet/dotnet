# Changelog

## Unreleased

## 1.15.0

Released 2026-Jan-21

* Updated OpenTelemetry core component version(s) to `1.15.0`.
  ([#3721](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3721))

## 1.14.0

Released 2025-Nov-13

* Add support for .NET 10.0.
  ([#2822](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2822))

* Update .NET 10.0 NuGet package versions from `10.0.0-rc.2.25502.107` to `10.0.0`.
  ([#3403](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3403))

* Updated OpenTelemetry core component version(s) to `1.14.0`.
  ([#3403](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3403))

## 1.13.0

Released 2025-Oct-29

* Set initial capacity for AWS Semantic Convention Attribute Builder
  ([#2653](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2653))

* Suppress OTel HTTP instrumentation in AWS Resource Detector
  ([#3044](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3044))

* Updated OpenTelemetry core component version(s) to `1.13.1`.
  ([#3218](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3218))

## 1.11.1

Released 2025-Mar-05

* Updated OpenTelemetry core component version(s) to `1.11.2`.
  ([#2582](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2582))

## 1.11.0

Released 2025-Jan-29

* Updated OpenTelemetry core component version(s) to `1.11.1`.
  ([#2477](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2477))

## 1.10.0-rc.1

Released 2025-Jan-06

* BREAKING: Change default Semantic Convention to 1.28

* BREAKING: Remove option to use Legacy semantic conventions (the old default)

## 1.10.0-beta.1

Released 2024-Dec-20

* Introduce `AWSResourceBuilderOptions.SemanticConventionVersion` which
  provides a mechanism for developers to opt-in to newer versions of the
  of the Open Telemetry Semantic Conventions.
  ([#2367](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2367))

* Drop support for .NET 6 as this target is no longer supported
  and add .NET Standard 2.0 target.
  ([#2164](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2164))

* Bumped the `System.Text.Json` reference to `6.0.10` for runtimes older than
  `net8.0` and added a direct reference for `System.Text.Json` at `8.0.5` on
  `net8.0` in response to
  [CVE-2024-43485](https://msrc.microsoft.com/update-guide/vulnerability/CVE-2024-43485).
  ([#2196](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2196))

* Updated OpenTelemetry core component version(s) to `1.10.0`.
  ([#2317](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2317))

## 1.5.0-beta.1

Released 2024-Jun-04

* Implement support for cloud.{account.id,availability_zone,region} attributes in
  AWS ECS detector.
  ([#1552](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1552))

* Implement support for `cloud.resource_id` attribute in AWS ECS detector.
  ([#1576](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1576))

* Update OpenTelemetry SDK version to `1.8.1`.
  ([#1668](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1668))

* **Breaking Change**: Renamed package from `OpenTelemetry.ResourceDetectors.AWS`
  to `OpenTelemetry.Resources.AWS`.
  ([#1839](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1839))

* **Breaking Change**: `AWSEBSResourceDetector`, `AWSEC2ResourceDetector`,
`AWSECSResourceDetector` and `AWSEKSResourceDetector` types are now internal,
use `ResourceBuilder` extension methods `AddAWSEBSDetector`,
`AddAWSEC2Detector`, `AddAWSECSDetector`
and `AddAWSEKSDetector` respectively to enable the detectors.
  ([#1839](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1839))

* **Breaking Change**: Renamed EventSource
from `OpenTelemetry-ResourceDetectors-AWS`
to `OpenTelemetry-Resources-AWS`.
  ([#1839](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1839))

## 1.4.0-beta.1

Released 2024-Jan-26

* Update OpenTelemetry SDK version to `1.7.0`.
  ([#1486](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1486))

* Fix AWS EBS Resource Detector working on linux.
  ([#1350](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1350))

* BREAKING: All Resource Detector classes marked as `sealed`.
  ([#1510](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1510))

* Make OpenTelemetry.ResourceDetectors.AWS native AoT compatible.
  ([#1541](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1541))

## 1.3.0-beta.1

Released 2023-Aug-02

* Initial release. Previously it was part of `OpenTelemetry.Contrib.Extensions.AWSXRay`
  package.
  ([#1140](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1140))

* Update OpenTelemetry SDK version to `1.5.1`.
  ([#1255](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1255))
