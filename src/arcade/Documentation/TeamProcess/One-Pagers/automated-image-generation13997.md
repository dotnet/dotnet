# Automated Image Generation

[Link to Automated Image Generation Epic](https://github.com/dotnet/core-eng/issues/13997)

## Summary

### Motivation
Currently only way how to generate Helix custom images for our Windows queues is to ask DDFUN. This happens on a weekly basis, always when we want to update Visual Studio versions or include the newest Windows patches. This process has several drawbacks from allocating someone from DDFUN team to introduction of typos by manual steps.


### Goal
The goal of this epic is to automate the process of custom image generation, so this can be done by our team or vendors, without the need of external teams. These are images which contains various versions of Visual Studio and various versions of Windows. We will introduce a new automated and monitored process which removes the need for manual steps done by DDFUN.

When this is completed we will be able to:
* generate images ourselves without spending time on coordination with DDFUN, similarly to other teams using Image Factory
* specify new versions at one place instead of tens of configuration files
* automatically regenerate images on change of configuration files
* monitor completion of process instead of relying on notifications from DDFUN

Part of this epic is to take the ownership of [custom image definitions](https://devdiv.visualstudio.com/XlabImageFactory/_git/ImageConfigurations?path=%2FMonthly%2FHelixBaseImages) that currently reside with DDFUN to get complete control over the definitions and to be able to run our automation against them seamlessly. This will be part of new documentation so maintenance of these definitions can be done also by vendors.

### Implementation Details

Let's start with a scenario where we need to update Visual Studio 2019 Preview version per our [schedule](https://dev.azure.com/dnceng/internal/_wiki/wikis/DNCEng%20Services%20Wiki/107/VS2019-Upgrade-Schedule) which has to be done almost every week.

Currently this change requires update of same values in six [image definitions](https://devdiv.visualstudio.com/XlabImageFactory/_git/ImageConfigurations?path=/Monthly/HelixBaseImages/VS2019Preview). Specifically:
* update artifact windows-vs-willowreleased, set parameter VSBootstrapperURL to a new value.
* update version in parameter CustomImageName under Destination.

To simplify this, we will introduce templating, so variables are defined at exactly one place and are not duplicated across multiple files, similarly to what we have in OSOB.

#### Example:
Instead of hardcoding the same version and the same URL at [six places](https://devdiv.visualstudio.com/XlabImageFactory/_git/ImageConfigurations?path=/Monthly/HelixBaseImages/VS2019Preview), we introduce template variable {VS_2019_PREVIEW_URL} which declares an URL to Visual Studio artifact and the template variable {VS_2019_PREVIEW_VERSION} which declares version of Visual Studio.

All templated variables will be stored in one file.


1. Given our example scenario, we will update variables VS_2019_PREVIEW_URL and VS_2019_PREVIEW_VERSION. And this change will be pushed into the repository. This change triggers our new build pipeline under Azure DevOps.

2. The pipeline executes command line tool which processes all payloads and substitutes all template variables from the variable file and call Image Factory with this payload. To prevent duplication we will calculate hash of payload and store it a simple Azure storage table and call Image Factory only for payloads which haven't been processed yet. If needed it will be possible to force rebuilding of images.

3. The pipeline won't block until all images are completed. It will post Image Factory jobs, store tracking ids in Azure storage table mentioned above and finish.

4. To get results, the same pipeline will be executed periodically (e.g. every hour) and check states of all pending Image Factory jobs and provide summary report so it's clear if all images that were requested are ready. In case any image build fails, the pipeline will fail and FR will be notified by email.

5. Once Helix custom images are generated, FR has to create an OSOB PR with updated image names. Existing OSOB post validation performs version test of Visual Studio.

Documentation of the Image Factory API can be found [here](https://devdiv.visualstudio.com/XlabImageFactory/_wiki/wikis/XlabImageFactory.wiki/6330/AccessingImageFactory).


## Take ownership of Helix custom image definitions

Making custom image definitions templated requires modifications. This is why we need to move all [definitions](https://devdiv.visualstudio.com/XlabImageFactory/_git/ImageConfigurations?path=%2FMonthly%2FHelixBaseImages) under our repository. It was confirmed by DDFUN (Casey) that these definitions aren't shared with any other team. Beside changing URL and version the structure is left unchanged, so there isn't any additional maintenance cost related to owning these definitions.

Currently the definitions with DDFUN use YAML only to be converted to JSON payloads. As part of the move I would suggest to start using JSON file format as it's expected input of the Image Factory. The only benefit of YAML are comments, but in our case these comments are copy pasted across all definitions and don't add any additional value.

Here is an example of a fragment of a templated image definition with template variables VS_2019_PREVIEW_URL and VS_2019_PREVIEW_VERSION:
```
{
    "Artifacts": [
        {
            "Name": "windows-vs-willowreleased",
            "Parameters": {
                "WorkLoads": "reduced",
                "Sku": "Enterprise",
                "VSBootstrapperURL": "{VS_2019_PREVIEW_URL}/vs_Enterprise.exe"
            }
        }
    ],
    "Destination": [
        {
            "StorageAccountName": "heliximgfctdncwus2",
            "CustomImageName": "Helix-Server-DataCenter-19H1-ES-ES-VS2019-Preview-Enterprise_{VS_2019_PREVIEW_VERSION}",
            "SubId": "84a65c9a-787d-45da-b10a-3a1cefce8060"
        }
    ]
}
```

_Note: Nested templated variables won't be supported._

Variables file:
```
{
    "VS_2019_PREVIEW_URL":"https://aka.ms/vs/16/pre/133508311_-1151188015",
    "VS_2019_PREVIEW_VERSION":"16_11_2_1",
}
```


## Stakeholders

- .NET Engineering Services

## Risk

- Are there any POCs (proof of concepts) required to be built for this work?

    POC for Visual Studio images was done before creating this one pager.

- What dependencies will this epic have? Are the dependencies currently in a state that the functionality in the epic can consume them now, or will they need to be updated?

    This epic depends on external service Image Factory. It's functionality is currently stable and no breaking changes are planned. I asked to notify our team in a case of changes.

- Will the new implementation of any existing functionality cause breaking changes for existing consumers?

    Implementation of this shouldn't cause breaking changes as all issues with images should be detected during OSOB validation phase.


## Serviceability

- How will the components that make up this epic be tested?

    CLI will be tested by unit tests. There will be also scenario test which generates one image and verifies it.

- Identifying secrets (e.g. PATs, certificates, et cetera) that will be used (new ones to be created; existing ones to be used).

    Existing secrets:
	* image-factory-tenant-id
	* image-factory-client-id
	* image-factory-client-secret
	* image-factory-resource-id

    New secret:
    * image-factory-state-connection-string

- Does this change any existing SDL threat or data privacy models? (models can be found
in [core-eng/SDL](https://github.com/dotnet/core-eng/SDL) folder)

    It doesn't change SDL threat or data privacy models.


## Rollout and Deployment

- A new Azure DevOps build pipeline will take specific version of this tool from artifacts and executes it.

- In a case of issues, it is still possible generate images manually.


## Usage Telemetry
- This tool is internal only. Basic information about runs will be available from the pipeline history. We don't plan to include any additional data, unless they are requested. If we start experience problems with the image generation though, we might need to start gathering some reliability data.

## Monitoring
- Monitoring is based on result of the build pipeline. It will send email notification on any failure. If there is any issue, it should be picked up by FR.

## FR Hand off
- We will create documentation about
    - How to generate custom images with Visual Studio
    - How to use the tool in manual mode
    - What to do when pipeline fails the build

- The policies of generating images overlaps with Matrix of Truth epic and will be further discussed with the epic owner.

## Future outlook
- Execution of the process should be possible to be done by vendors with minimal cost.




<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CTeamProcess%5COne-Pagers%5Cautomated-image-generation13997.md)](https://helix.dot.net/f/p/5?p=Documentation%5CTeamProcess%5COne-Pagers%5Cautomated-image-generation13997.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CTeamProcess%5COne-Pagers%5Cautomated-image-generation13997.md)</sub>
<!-- End Generated Content-->
