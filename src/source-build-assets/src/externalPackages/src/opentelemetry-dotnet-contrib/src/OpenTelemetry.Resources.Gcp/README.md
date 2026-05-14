# Resource Detectors for Google Cloud Platform environments

| Status | |
| ------ | --- |
| Stability | [Development](../../README.md#development) |
| Code Owners | [@matt-hensley](https://github.com/matt-hensley) |

[![NuGet version badge](https://img.shields.io/nuget/v/OpenTelemetry.Resources.Gcp)](https://www.nuget.org/packages/OpenTelemetry.Resources.Gcp)
[![NuGet download count badge](https://img.shields.io/nuget/dt/OpenTelemetry.Resources.Gcp)](https://www.nuget.org/packages/OpenTelemetry.Resources.Gcp)
[![codecov.io](https://codecov.io/gh/open-telemetry/opentelemetry-dotnet-contrib/branch/main/graphs/badge.svg?flag=unittests-Resources.Gcp)](https://app.codecov.io/gh/open-telemetry/opentelemetry-dotnet-contrib?flags[0]=unittests-Resources.Gcp)

This package contains [Resource
Detectors](https://github.com/open-telemetry/opentelemetry-specification/blob/main/specification/resource/sdk.md#detecting-resource-information-from-the-environment)
for applications running in Google Cloud Platform environments.

## Getting Started

You need to install the
`OpenTelemetry.Resources.Gcp` package to be able to use the
Google Cloud Platform Resource Detectors.

```shell
dotnet add package --prerelease OpenTelemetry.Resources.Gcp
```

## Usage

You can configure Google Cloud Platform resource detector to
the `ResourceBuilder` with the following example.

```csharp
using OpenTelemetry;
using OpenTelemetry.Resources;

using var tracerProvider = Sdk.CreateTracerProviderBuilder()
    .ConfigureResource(resource => resource.AddGcpDetector())
    // other configurations
    .Build();

using var meterProvider = Sdk.CreateMeterProviderBuilder()
    .ConfigureResource(resource => resource.AddGcpDetector())
    // other configurations
    .Build();

using var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddOpenTelemetry(options =>
    {
        options.SetResourceBuilder(ResourceBuilder.CreateDefault().AddGcpDetector());
    });
});
```

## Resource Attributes

The following OpenTelemetry semantic conventions will be detected depending on
which Google Cloud Platform environment an application is running in.

### Google Kubernetes Engine

| Attribute               | Value                 |
| ----------------------- | --------------------- |
| cloud.provider          | gcp                   |
| cloud.platform          | gcp_kubernetes_engine |
| cloud.account.id        | auto                  |
| cloud.availability_zone | auto                  |
| host.id                 | auto                  |
| k8s.cluster.name        | auto                  |
| k8s.namespace.name      | auto                  |
| k8s.pod.name            | auto                  |

### Google App Engine

| Attribute               | Value          |
| ----------------------- | -------------- |
| cloud.provider          | gcp            |
| cloud.platform          | gcp_app_engine |
| cloud.account.id        | auto           |

### Google Cloud Run

| Attribute               | Value         |
| ----------------------- | ------------- |
| cloud.provider          | gcp           |
| cloud.platform          | gcp_cloud_run |
| cloud.account.id        | auto          |
| cloud.availability_zone | auto          |
| cloud.region            | auto          |

### Google Compute Engine

| Attribute               | Value              |
| ----------------------- | ------------------ |
| cloud.provider          | gcp                |
| cloud.platform          | gcp_compute_engine |
| cloud.account.id        | auto               |
| cloud.availability_zone | auto               |
| host.id                 | auto               |
