# Resource Detectors for Azure cloud environments

| Status | |
| ------ | --- |
| Stability | [Beta](../../README.md#beta) |
| Code Owners | [@rajkumar-rangaraj](https://github.com/rajkumar-rangaraj) |

[![NuGet version badge](https://img.shields.io/nuget/v/OpenTelemetry.Resources.Azure)](https://www.nuget.org/packages/OpenTelemetry.Resources.Azure)
[![NuGet download count badge](https://img.shields.io/nuget/dt/OpenTelemetry.Resources.Azure)](https://www.nuget.org/packages/OpenTelemetry.Resources.Azure)
[![codecov.io](https://codecov.io/gh/open-telemetry/opentelemetry-dotnet-contrib/branch/main/graphs/badge.svg?flag=unittests-Resources.Azure)](https://app.codecov.io/gh/open-telemetry/opentelemetry-dotnet-contrib?flags[0]=unittests-Resources.Azure)

This package contains [Resource
Detectors](https://github.com/open-telemetry/opentelemetry-specification/blob/main/specification/resource/sdk.md#detecting-resource-information-from-the-environment)
for applications running in Azure environment.

## Installation

```shell
dotnet add package --prerelease OpenTelemetry.Resources.Azure
```

## App Service Resource Detector

Adds resource attributes for the applications running in Azure App Service.
The following example shows how to add `AppServiceResourceDetector` to
the `ResourceBuilder`.

```csharp
using OpenTelemetry;
using OpenTelemetry.Resources;

using var tracerProvider = Sdk.CreateTracerProviderBuilder()
    .ConfigureResource(resource => resource.AddAzureAppServiceDetector())
    // other configurations
    .Build();

using var meterProvider = Sdk.CreateMeterProviderBuilder()
    .ConfigureResource(resource => resource.AddAzureAppServiceDetector())
    // other configurations
    .Build();

using var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddOpenTelemetry(options =>
    {
        options.SetResourceBuilder(ResourceBuilder.CreateDefault().AddAzureAppServiceDetector());
    });
});
```

| Attribute                   | Description                                                                                                                                                                                               |
| --------------------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| azure.app.service.stamp     | The specific "stamp" cluster within Azure where the App Service is running, e.g., "waws-prod-sn1-001".                                                                                                    |
| cloud.platform              | The cloud platform. Here, it's always "azure_app_service".                                                                                                                                                |
| cloud.provider              | The cloud service provider. In this context, it's always "azure".                                                                                                                                         |
| cloud.resource_id           | The Azure Resource Manager URI uniquely identifying the Azure App Service. Typically in the format "/subscriptions/{subscriptionId}/resourceGroups/{groupName}/providers/Microsoft.Web/sites/{siteName}". |
| cloud.region                | The Azure region where the App Service is hosted, e.g., "East US", "West Europe", etc.                                                                                                                    |
| deployment.environment.name | The deployment slot where the Azure App Service is running, such as "staging", "production", etc.                                                                                                         |
| host.id                     | The primary hostname for the app, excluding any custom hostnames.                                                                                                                                         |
| service.instance.id         | The specific instance of the Azure App Service, useful in a scaled-out configuration.                                                                                                                     |
| service.name                | The name of the Azure App Service.                                                                                                                                                                        |

## VM Resource Detector

Adds resource attributes for the applications running in an Azure virtual machine.
The following example shows how to add `AzureVMResourceDetector` to
the `ResourceBuilder`.

```csharp
using OpenTelemetry;
using OpenTelemetry.Resources;

using var tracerProvider = Sdk.CreateTracerProviderBuilder()
    .ConfigureResource(resource => resource.AddAzureVMDetector())
    // other configurations
    .Build();

using var meterProvider = Sdk.CreateMeterProviderBuilder()
    .ConfigureResource(resource => resource.AddAzureVMDetector())
    // other configurations
    .Build();
```

| Attribute              | Description                                                                                                                                                                                                                         |
| ---------------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| azure.vm.scaleset.name | The name of the Virtual Machine Scale Set if the VM is part of one.                                                                                                                                                                 |
| cloud.platform         | The cloud platform, which is always set to "azure_vm" in this context.                                                                                                                                                              |
| cloud.provider         | The cloud service provider, which is always set to "azure" in this context.                                                                                                                                                         |
| cloud.region           | The Azure region where the Virtual Machine is hosted, such as "East US", "West Europe", etc.                                                                                                                                        |
| cloud.resource_id      | The Azure Resource Manager URI uniquely identifying the Azure Virtual Machine. It typically follows this format: "/subscriptions/{subscriptionId}/resourceGroups/{groupName}/providers/Microsoft.Compute/virtualMachines/{vmName}". |
| host.id                | A unique identifier for the VM host, for instance, "02aab8a4-74ef-476e-8182-f6d2ba4166a6".                                                                                                                                          |
| host.name              | The name of the host machine.                                                                                                                                                                                                       |
| host.type              | The size of the VM instance, for example, "Standard_D2s_v3".                                                                                                                                                                        |
| os.type                | The type of operating system running on the VM, such as "Linux" or "Windows".                                                                                                                                                       |
| os.version             | The version of the operating system running on the VM.                                                                                                                                                                              |
| service.instance.id    | An identifier for a specific instance of the service running on the Azure VM, for example, "02aab8a4-74ef-476e-8182-f6d2ba4166a6".                                                                                                  |

## Azure Container Apps Resource Detector

Adds resource attributes for the applications running in Azure Container Apps
or Azure Container App jobs. The following example shows how to add
`AzureContainerAppsResourceDetector` to the `ResourceBuilder`.

```csharp
using OpenTelemetry;
using OpenTelemetry.Resources;

using var tracerProvider = Sdk.CreateTracerProviderBuilder()
    .ConfigureResource(resource => resource.AddAzureContainerAppsDetector())
    // other configurations
    .Build();

using var meterProvider = Sdk.CreateMeterProviderBuilder()
    .ConfigureResource(resource => resource.AddAzureContainerAppsDetector())
    // other configurations
    .Build();
```

| Attribute           | Description                                                                                                                 |
| ------------------- | --------------------------------------------------------------------------------------------------------------------------- |
| cloud.platform      | The cloud platform. Here, it's always "azure_container_apps".                                                               |
| cloud.provider      | The cloud service provider. In this context, it's always "azure".                                                           |
| service.instance.id | Represents the specific instance ID of Azure Container Apps, useful in scaled-out configurations.                           |
| service.name        | The name of the Azure Container Apps or Azure Container Apps job.                                                           |
| service.version     | The current revision or version of Azure Container Apps, or in case of a Azure Container Apps job - the job execution name. |
