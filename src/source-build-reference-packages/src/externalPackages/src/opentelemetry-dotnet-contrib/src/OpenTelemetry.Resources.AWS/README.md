# AWS Resource Detectors

| Status | |
| ------ | --- |
| Stability | [Stable](../../README.md#stable) |
| Code Owners | [@srprash](https://github.com/srprash), [@normj](https://github.com/normj), [@lukeina2z](https://github.com/lukeina2z) |

[![NuGet version badge](https://img.shields.io/nuget/v/OpenTelemetry.Resources.AWS)](https://www.nuget.org/packages/OpenTelemetry.Resources.AWS)
[![NuGet download count badge](https://img.shields.io/nuget/dt/OpenTelemetry.Resources.AWS)](https://www.nuget.org/packages/OpenTelemetry.Resources.AWS)
[![codecov.io](https://codecov.io/gh/open-telemetry/opentelemetry-dotnet-contrib/branch/main/graphs/badge.svg?flag=unittests-Resources.AWS)](https://app.codecov.io/gh/open-telemetry/opentelemetry-dotnet-contrib?flags[0]=unittests-Resources.AWS)

## Getting Started

You need to install the
`OpenTelemetry.Resources.AWS` to be able to use the
AWS Resource Detectors.

The ADOT .NET SDK supports automatically recording metadata in
EC2, Elastic Beanstalk, ECS, and EKS environments.

```shell
dotnet add package OpenTelemetry.Resources.AWS
```

## Usage

You can configure AWS resource detector to
the `ResourceBuilder` with the following EC2 example.

```csharp
using OpenTelemetry;
using OpenTelemetry.Resources;

using var tracerProvider = Sdk.CreateTracerProviderBuilder()
    .ConfigureResource(resource => resource.AddAWSEC2Detector())
    // other configurations
    .Build();

using var meterProvider = Sdk.CreateMeterProviderBuilder()
    .ConfigureResource(resource => resource.AddAWSEC2Detector())
    // other configurations
    .Build();

using var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddOpenTelemetry(options =>
    {
        options.SetResourceBuilder(ResourceBuilder.CreateDefault().AddAWSEC2Detector());
    });
});
```

The resource detectors will record the following metadata based on where
your application is running:

- **AWSEC2Detector**: cloud provider, cloud platform, account id,
cloud availability zone, host id, host type, aws region, host name.
- **AWSEBSDetector**: cloud provider, cloud platform, service name,
service namespace, instance id, service version.
- **AWSECSDetector**: cloud provider, cloud platform, cloud resource id,
account id, cloud availability zone, cloud region, container id, cluster arn,
task arn, task family, task revision, launch type, container arn, log group names,
log group ids, log stream names, log stream ids.
- **AWSEKSDetector**: cloud provider, cloud platform, cluster name,
container id.

## Semantic Conventions

_For an overview on Semantic Conventions, see
[Open Telemetery - Semantic Conventions](https://opentelemetry.io/docs/concepts/semantic-conventions/)_.

While this library is intended for production use, it relies on several
Semantic Conventions that are still considered Experimental, meaning
they may undergo additional changes before becoming Stable.  This can impact
the aggregation and analysis of telemetry signals in environments with
multiple applications or microservices.

For example, a microservice using an older version of the Semantic Conventions
for Http Attributes may emit `"http.method"` with a value of GET, while a
different microservice, using a new version of Semantic Convention may instead
emit the GET as `"http.request.method"`.

Future versions the OpenTelemetry.*.AWS libraries will include updates to the
Semantic Convention, which may break compatibility with a previous version.

The default will remain as `V1_28_0` until the next major version bump.

To opt in to automatic upgrades, you can use `SemanticConventionVersion.Latest`
or you can specify a specific version:

```csharp
using OpenTelemetry;
using OpenTelemetry.Resources;

using var tracerProvider = Sdk.CreateTracerProviderBuilder()
    .ConfigureResource(resource => resource.AddAWSEC2Detector(
        opt => {
            // pin to a specific Semantic Convention version
            opt.SemanticConventionVersion = SemanticConventionVersion.V1_29_0;
        }
    ))
    // other configurations
    .Build();
```

**NOTE:** Once a Semantic Convention becomes Stable, OpenTelemetry.*.AWS
libraries will remain on that version until the
next major version bump.

## References

- [OpenTelemetry Project](https://opentelemetry.io/)
- [AWS Distro for OpenTelemetry .NET](https://aws-otel.github.io/docs/getting-started/dotnet-sdk)
