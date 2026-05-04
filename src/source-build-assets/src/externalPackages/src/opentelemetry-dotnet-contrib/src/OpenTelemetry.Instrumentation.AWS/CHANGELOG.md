# Changelog - OpenTelemetry.Instrumentation.AWS

## Unreleased

## 1.14.2

Released 2026-Jan-14

* Update minimal supported version of `AWSSDK.Core` to `4.0.3.3`.
  Older versions are affected by [CVE-2026-22611](https://github.com/advisories/GHSA-9cvc-h2w8-phrp).
  ([#3683](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3683))

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

## 1.12.1

Released 2025-Sep-03

## 1.12.0

Released 2025-May-06

* BREAKING: Update AWSSDK dependencies to v4.
  ([#2720](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2720))

## 1.11.3

Released 2025-May-01

* Update package references to AWS SDK to indicate that this library is not
  compatible with AWS SDK v4.
   ([#2726](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2726))

## 1.11.2

Released 2025-Mar-18

* Set initial capacity for AWS Semantic Convention Attribute Builder
  ([#2653](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2653))

## 1.11.1

Released 2025-Mar-05

## 1.11.0

Released 2025-Jan-29

## 1.10.0-rc.2

Released 2025-Jan-15

* Context propagation data is always added to SQS and SNS requests regardless of
  sampling decision. This enables downstream services to make consistent sampling
  decisions and prevents incomplete traces.
  ([#2447](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2447))

## 1.10.0-rc.1

Released 2025-Jan-06

* BREAKING: Change default Semantic Convention to 1.28
* BREAKING: Remove option to use Legacy semantic conventions (the old default)

## 1.10.0-beta.3

Released 2024-Dec-20

* Introduce `AWSClientInstrumentationOptions.SemanticConventionVersion` which
  provides a mechanism for developers to opt-in to newer versions of the
  of the OpenTelemetry Semantic Conventions. Currently, you need to opt-in
  to these new conventions. In the upcoming stable release of this library,
  the new conventions will be enabled by default, and the conventions this library
  currently emit will no longer be supported.
  ([#2367](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2367))

## 1.10.0-beta.2

Released 2024-Dec-12

* Trace instrumentation will now call the [Activity.SetStatus](https://learn.microsoft.com/dotnet/api/system.diagnostics.activity.setstatus)
  API instead of the deprecated OpenTelemetry API package extension when setting
  span status. For details see: [Setting Status](https://github.com/open-telemetry/opentelemetry-dotnet/blob/main/src/OpenTelemetry.Api/README.md#setting-status).
  ([#2358](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2358))

## 1.10.0-beta.1

Released 2024-Nov-23

* Move adding request and response info to AWSTracingPipelineHandler
  ([#2137](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2137))
* Drop support for .NET 6 as this target is no longer supported and add .NET 8 target.
  ([#2139](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2139))

## 1.1.0-beta.6

Released 2024-Sep-10

* Fix Memory Leak by Reusing ActivitySources, Meters, and Instruments
  ([#2039](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2039))
* Added instrumentation support for AWS Bedrock, BedrockRuntime, BedrockAgent, BedrockAgentRuntime.
  ([#1979](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1979))

## 1.1.0-beta.5

Released 2024-Aug-22

* BREAKING: Update the instrumentation logic to use AWS TracerProvider.
  ([#1974](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1974))
* Add AWS metrics instrumentation.
  ([#1980](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1980))
* Updated dependency on AWS .NET SDK to version 3.7.400.
  ([#1974](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1980))
* Added `rpc.system`, `rpc.service`, and `rpc.method` to activity tags based on
  [semantic convention v1.26.0](https://github.com/open-telemetry/semantic-conventions/blob/v1.26.0/docs/cloud-providers/aws-sdk.md#common-attributes).
  ([#1865](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1865))

## 1.1.0-beta.4

Released 2024-Apr-12

* BREAKING: Switched AWSServiceName tag to use ServiceId instead of ServiceName
  ([#1572](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1572))
* `ActivitySource.Version` is set to NuGet package version.
  ([#1624](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1624))

## 1.1.0-beta.3

Released 2024-Jan-26

* Updated dependency on AWS .NET SDK to version 3.7.300.
  ([#1542](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1542))
* Add target for `net6.0`.
  ([#1547](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1547))
* Add support for native AoT.
  ([#1547](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1547))

## 1.1.0-beta.2

Released 2023-Dec-01

* Updated dependency on AWS .NET SDK to version 3.7.100.
  ([#1454](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1454))

## 1.1.0-beta.1

Released 2023-Aug-07

* BREAKING (renaming): renamed `OpenTelemetry.Contrib.Instrumentation.AWS` to `OpenTelemetry.Instrumentation.AWS`
  ([#1275](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1275))
* Raised the minimum .NET Framework version to `net462`
  ([#1095](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1095))
* Removes `AddAWSInstrumentation` method with default configure default parameter.
  ([#1117](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1117))
* Global propagator is now used to inject into sent SQS and SNS message
  attributes (in addition to X-Ray propagation).
  ([#1051](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1051))
* Change dependency from `OpenTelemetry.Contrib.Extensions.AWSXRay` to `OpenTelemetry.Extensions.AWS`
  ([#1288](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/1288))

## 1.0.2

Released 2022-Nov-11

* Fixed issue when using version 3.7.100 of the AWS SDK for .NET triggering an
  EndpointResolver not found exception.
  ([#726](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/726))

## 1.0.1

Released 2021-Feb-24

This is the first release for the `OpenTelemetry.Contrib.Instrumentation.AWS`
project. The release targets v1.0.1 of the
[OpenTelemetry.Contrib.Extensions.AWSXRay](https://www.nuget.org/packages/OpenTelemetry.Contrib.Extensions.AWSXRay/)

For more details, please refer to the
[README](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/blob/main/src/OpenTelemetry.Instrumentation.AWS/README.md)
