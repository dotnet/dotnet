## Motivation

Due to corporate policy we are required to migrate our buildpool queues to 1ES Hosted pools. The deadlines include: new pools won’t be created after June 1st. Self-hosted pools will stop working after Sep 30th.

Reference documentation for 1ES pools:
- [1ES hosted AzureDevOps Agents/Guidance](https://www.1eswiki.com//wiki/1ES_hosted_AzureDevOps_Agents%2fGuidance)
- [1ES hosted AzureDevOps Agents](https://www.1eswiki.com/wiki/1ES_hosted_AzureDevOps_Agents)
- [CloudTest Onboarding Guide](https://1esdocs.azurewebsites.net/test/CloudTest/How-Tos/Create-Update-Pool.html)

High level migration plan looks like following:
- Create new definitions for each of our existing buildpools that will exist in parallel with the Helix queues
- Migrate customer Yamls to new 1ES based pools
- Delete old buildpool queues from Helix
- Clean up definitions for buildpools (i.e. remove all Helix-specific artifacts)
- Decomission all instances of pool provider and all related resources (key vaults, CI pipelines, release pipelines)

We need to extend OSOB to create two new types of Azure resources:
- `Microsoft.CloudTest/image` resource for each image that we want to enable on the build pool. This resource contains reference to our SharedImageGallery image version.
- `Microsoft.CloudTest/hostedpool` resource for each pool. The pools will reference the CloudTest images mentioned above.

## New AzDo pool distribution

Right now, there are two pools (Prod and staging) for our internal project and two for external. We will keep one pool for staging per project and split prod pools into 3 different ones: XAML, servicing and R&D. 


| AzDo Project | Enviroment | Current pool | New pools | Subscription |
| ------------ | ---------- | ------------ | --------- | ------------ |
| Internal | Staging | NetCoreInternal-Int-Pool | NetCore1ES-Internal-Int-Pool | HelixStaging |
| Internal | Prod | NetCoreInternal-Pool | NetCore1ES-Internal-Pool | HelixProd |
| Internal | Prod | NetCoreInternal-Pool | NetCore1ES-Xaml-Internal-Pool | DEP-UXP-WinUI-Helix |
| Internal | Prod | NetCoreInternal-Pool | NetCore1ES-Svc-Internal-Pool | dncenghelix-02 |
| Public | Staging | NetCorePublic-Int-Pool | NetCore1ES-Public-Int-Pool | HelixStaging |
| Public | Prod | NetCorePublic-Pool | NetCore1ES-Public-Pool | HelixProd |
| Public | Prod | NetCorePublic-Pool | NetCore1ES-Xaml-Public-Pool | DEP-UXP-WinUI-Helix |
| Public | Prod | NetCorePublic-Pool | NetCore1ES-Svc-Public-Pool | dncenghelix-02 |

## 1ES Managed images

1ES Managed images are stored in the resource group [1ESManagedImages](https://ms.portal.azure.com/#@microsoft.onmicrosoft.com/resource/subscriptions/84a65c9a-787d-45da-b10a-3a1cefce8060/resourcegroups/1ESManagedImages/overview) in **dnceng-internaltooling** subscription

Each Managed image points to an Azure image in the Shared Gallery ([HelixImages](https://ms.portal.azure.com/#@microsoft.onmicrosoft.com/resource/subscriptions/84a65c9a-787d-45da-b10a-3a1cefce8060/resourceGroups/HelixImages/providers/Microsoft.Compute/galleries/HelixImages/overview)) where we store the images we build in **CreateCustomImage.exe** this includes Prod and Staging images.

1ES Managed images used in prod are tagged the same way Azure images are, the tag is **IsProductionImage** and its value is **true**. The tagging happens during the deployment of the pools in **DeployHostedPools.exe**. We will keep the lasted 3 prod images the same way we do for Azure images, the clean up will happen in CleanPRs.exe

## Customer impact

All customers must change their yaml files to start using 1ES Host pools. Proper documentation will be shared through our Partners DL.

The old syntax:
```yaml
pool:
    name: NetCoreInternal-Pool
    queue: BuildPool.Server.Amd64.VS2017
```

Will be replaced by:
```yaml
pool:
    name: NetCore1ES-Internal-Int-Pool
    demands: ImageOverride -equals BuildPool.Server.Amd64.VS2017
```

## OSOB changes

- We will need to create new definitions in the YAML inheriting from existing `BuildPool.` queues. This new definitions will have the same properties as existing ones but we will have to change the names because we can't have two definitions with the same name. We can do it for example by changing prefix (e.g. `Build.Windows.10.Amd64` instead of `BuildPool.Windows.10.Amd64`). Keeping the old names would also be technically possible but it would require more changes in the OSOB deployment steps.

- We will use the `Purpose` property to mark definitions used for 1ES hosted pool images. `DeployQueues` will skip this queue but it will be processed in the new `DeployManagedPools` step instead. It shouldn't require any changes to CreateCustomImages.

- A new file called hostedpools.yaml will be create under definition-base folder. It will contain pools' metadata that later will be used during their deployment. The file will look like this:

```yaml
HostedPools:
- Name: NetCore1ES-Internal-Pool
  Subscription: HelixProd
  VMSku: Standard_Dav4
  Region: westus2
  Size: 100
- Name: NetCore1ES-Xaml-Internal-Pool
  Subscription: DEP-UXP-WinUI-Helix
  VMSku: Standard_Dav4
  Region: westus2
  Size: 100
```

## 1ES hosted pool ARM template

We will need to add a new step to OSOB build pipeline that will generate and deploy ARM template that will provision 1ES hosted pool. We could extend `DeployQueues` but it will probably be better idea to create new tool `DeployHostedPools` that can be run in parallel with `DeployQueues`.

Example ARM template for 1ES hosted pool looks like following:

```json
{
  "$schema": "https://schema.management.azure.com/schemas/2019-04-01/deploymentTemplate.json#",
  "contentVersion": "1.0.0.0",
  "resources": [
    {
      "name": "BuildPool.Windows.10.Amd64.Open",
      "type": "Microsoft.CloudTest/images",
      "apiVersion": "2020-05-07",
      "location": "westus2",
      "properties": {
        "imageType": "Gallery",
        "resourceId": "/HelixImages/BuildPool.Windows.10.Amd64.Open/2021.0423.210713"
      }
    },
    {
      "name": "BuildPool.Ubuntu.1804.Amd64.Open",
      "type": "Microsoft.CloudTest/images",
      "apiVersion": "2020-05-07",
      "location": "westus2",
      "properties": {
        "imageType": "Gallery",
        "resourceId": "/HelixImages/BuildPool.Ubuntu.1804.Amd64/2021.0503.000052"
      }
    },
    // ...
    {
      "name": "NetCorePublic-1ESPool",
      "type": "Microsoft.CloudTest/hostedpools",
      "dependsOn": [
        "[resourceId('Microsoft.CloudTest/images', 'BuildPool.Windows.10.Amd64.Open')]"
        "[resourceId('Microsoft.CloudTest/images', 'BuildPool.Ubuntu.1804.Amd64.Open')]"
        // ...
      ],
      "apiVersion": "2020-05-07",
      "location": "westus2",
      "properties": {
        "organization": "https://dev.azure.com/dnceng",
        "sku": {
          "name": "Standard_Dav4",
          "tier": "Standard"
        },
        "images": [
          {
            "imageName": "BuildPool.Windows.10.Amd64.Open",
            "poolBufferPercentage": "*"
          },
          {
            "imageName": "BuildPool.Ubuntu.1804.Amd64.Open",
            "poolBufferPercentage": "*"
          }
          // ...
        ],
        "maxPoolSize": "100",
        "agentProfile": { "type": "Stateless" }
      }
    }
  ]
}
```



<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CProject-Docs%5CServicingJobRedirection%5C1ESManagedPoolsDesign.md)](https://helix.dot.net/f/p/5?p=Documentation%5CProject-Docs%5CServicingJobRedirection%5C1ESManagedPoolsDesign.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CProject-Docs%5CServicingJobRedirection%5C1ESManagedPoolsDesign.md)</sub>
<!-- End Generated Content-->
