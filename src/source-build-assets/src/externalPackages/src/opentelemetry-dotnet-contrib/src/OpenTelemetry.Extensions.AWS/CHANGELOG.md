# Changelog - OpenTelemetry.Extensions.AWS

## Unreleased

* Updated OpenTelemetry core component version(s) to `1.15.0`.
  ([#3721](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3721))

## 1.14.2

Released 2026-Jan-14

## 1.14.1

Released 2025-Dec-19

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

* Updated OpenTelemetry core component version(s) to `1.13.1`.
  ([#3218](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3218))

## 1.12.1

Released 2025-Sep-03

## 1.12.0

Released 2025-May-06

* Updated OpenTelemetry core component version(s) to `1.12.0`.
  ([#2725](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2725))

## 1.11.3

Released 2025-May-01

## 1.11.2

Released 2025-Mar-18

## 1.11.1

Released 2025-Mar-05

* Updated OpenTelemetry core component version(s) to `1.11.2`.
  ([#2582](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2582))

## 1.11.0

Released 2025-Jan-29

* Updated OpenTelemetry core component version(s) to `1.11.1`.
  ([#2477](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2477))

## 1.10.0-rc.2

Released 2025-Jan-15

## 1.10.0-rc.1

Released 2025-Jan-06

## 1.10.0-beta.3

Released 2024-Dec-20

## 1.10.0-beta.2

Released 2024-Dec-12

## 1.10.0-beta.1

Released 2024-Nov-23

* Drop support for .NET 6 as this target is no longer supported and add .NET 8 target.
  ([#2125](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2125))

* Removed the unused `System.Text.Json` reference.
  ([#2209](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2209))

* Updated OpenTelemetry core component version(s) to `1.10.0`.
  ([#2317](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2317))

## 1.3.0-beta.2

Released 2024-Sep-24

* Remove NuGet reference to `System.Net.Http`
  ([#1713](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1713))

* Updated OpenTelemetry core component version(s) to `1.9.0`.
  ([#1888](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1888))

## 1.3.0-beta.1

Released 2023-Aug-02

* Rename package from `OpenTelemetry.Contrib.Extensions.AWSXRay`
  to `OpenTelemetry.Extensions.AWS`
  ([#1232](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1232))

* Updates to 1.5.1 of OpenTelemetry SDK.
  ([#1255](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1255))

* Enhancement - AWSXRayIdGenerator - Generate X-Ray IDs with global Random
  instance instead of recreating with ThreadLocal
  ([#380](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/380))

* Raised minimum .NET version to `net462`
  ([#875](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/875))

* Replaced Newtonsoft.Json dependency with System.Text.Json
  ([#1092](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1092))

* Enhancement - AWSECSResourceDetector - Implement `aws.{ecs.*,log.*}` resource
  attributes with data from ECS Metadata endpoint v4
  ([#875](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/875))

* Removal - IResourceDetector - Remove local IResourceDetector interface and its
  supporting ResourceBuilderExtensions extension, and migrate all detectors to
  implement OpenTelemetry.Resources.IResourceDetector
  ([#875](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/875))

* Add a `net6.0` build with optimized trace ID generation using the new
  `Activity.TraceIdGenerator` API.
  ([#1096](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1096))

* Drop support for `AWSLambdaResourceDetector`.
  AWS Lambda Resources are detected by `OpenTelemetry.Instrumentation.AWSLambda`
  package
  ([#1140](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1140))

* Extract AWS Resource Detectors to dedicated package `OpenTelemetry.ResourceDetectors.AWS`
  ([#1140](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1140))

## 1.2.0

Released 2022-May-18

* Enhancement - AWSEKSResourceDetector - Validate ClusterName/ContainerID
  independently before adding it to the resource
  ([#205](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/205))

* Fix - AWSEKSResourceDetector fails to detect resources due to exception
  "The SSL connection could not be established"
  ([#208](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/208))

## 1.1.0

Released 2021-Sep-20

* Added AWS resource detectors ([#149](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/149))

* Updated OTel SDK package version to 1.1.0
  ([#100](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/100))

## 1.0.1

Released 2021-Feb-24

This is the first release for the `OpenTelemetry.Contrib.Extensions.AWSXRay`
project. The project targets v1.0.1 of the [OpenTelemetry
SDK](https://www.nuget.org/packages/OpenTelemetry/).

The AWSXRay extensions include plugin to generate X-Ray format trace-ids and a
propagator to propagate the X-Ray trace header to downstream. For more details,
please refer to the
[README](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/blob/main/src/OpenTelemetry.Extensions.AWS/README.md)
