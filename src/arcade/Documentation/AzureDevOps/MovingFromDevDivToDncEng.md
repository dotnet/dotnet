# Moving Official Builds from DevDiv to DncEng

## Guidance

These are the general steps for moving an official build from https://dev.azure.com/devdiv (DevDiv) to https://dev.azure.com/dnceng (DncEng).  This document is geared slightly towards .NET Core 3 component repos.  This document does not discuss converting builds from a task based definition to a YAML based definition.  YAML based builds are not a strict requirement for moving to DncEng though we should take note of repos which are not using YAML based builds and decide upon a timeline for moving to YAML (YAML based builds are a requirement for .NET Core 3 component repos).

1. Update the [agent pools](#agent-pools) used by your build.  Example: https://github.com/dotnet/dotnet-cli-archiver/pull/18/files

2. Create an internal code repository for your [source code](https://github.com/dotnet/arcade/blob/master/Documentation/AzureDevOps/AzureDevOpsGuidance.md#source-code) in https://dev.azure.com/dnceng/internal.

3. Add your repo to the dnceng/internal mirror. Example: https://github.com/dotnet/versions/pull/361/files

4. Create a Pipeline following the documented [guidance](https://github.com/dotnet/arcade/blob/master/Documentation/AzureDevOps/AzureDevOpsGuidance.md#pipelines).

5. Request signing approval

   To do this: 

    a. Go to Preview Features and turn off "New builds hub" (the "Request signing approval" link does not work in the "New builds hub")

    b. Navigate to the Pipeline and select "Request signing approval" from "..."

      - M2's name: [Your M2's name]

      - M2's alias: [Your M2's alias]

      - Justification: .Net Core product repo

    c. Your M2 will be notified of the request and will have to approve the request before the MicroBuild team will review the request enable signing of your Pipeline.

## Known issues

- RepoToolset based repos cannot build from an internal Git repo with SourceLink enabled (the default).  Builds will fail with an error like

```
2018-09-12T22:25:57.4923150Z E:\A\_work\7\s\.packages\microsoft.sourcelink.common\1.0.0-beta-62911-01\build\Microsoft.SourceLink.Common.targets(60,5): error : SourceRoot.SourceLinkUrl is empty: 'E:\A\_work\7\s\' [E:\A\_work\7\s\src\Tasks\Microsoft.NET.Build.Tasks\Microsoft.NET.Build.Tasks.csproj]
```

This is because RepoToolset is expecting a GitHub url but internal builds are building from an Azure Git repo.  This issue is fixed in Arcade SDK and it is recommended that you move to the Arcade SDK for your builds.  If moving to the Arcade SDK is not an immediate option, then you can work around the issue by [disabling SourceLink](https://github.com/dotnet/sourcelink/blob/master/docs/README.md#enablesourcelink)

## Agent pools

See [Azure DevOps Onboarding documentation](./AzureDevOpsOnboarding.md#agent-queues)


<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CAzureDevOps%5CMovingFromDevDivToDncEng.md)](https://helix.dot.net/f/p/5?p=Documentation%5CAzureDevOps%5CMovingFromDevDivToDncEng.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CAzureDevOps%5CMovingFromDevDivToDncEng.md)</sub>
<!-- End Generated Content-->
