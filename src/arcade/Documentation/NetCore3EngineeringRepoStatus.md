# .NET Core 3 Engineering transition status

## Primary Deliverables / Work Streams

1. Using Azure DevOps for CI  (no Jenkins)
2. Arcade SDK Adoption (build uses supported tools/infra)
3. Engineering dependency flow (requirement product construction)
4. Internal builds from dnceng (signing)

Target completion date for these workstreams is 12/14/2018.

## Status Overview

| Repo                       | Owner            | Status   | Risk Assessment                                                                       | Plan | Notes |
| ---------------------------| ---------------- | -------- |:--------------------------------------------------------------------------------------| ----- |----- |
| Arcade                     | mawilkie         | Complete | ![done](https://findicons.com/files/icons/767/wp_woothemes_ultimate/16/checkmark.png) | | |
| aspnet-AspNetCore          | namc             | Complete | ![done](https://findicons.com/files/icons/767/wp_woothemes_ultimate/16/checkmark.png) | | Not building using Arcade SDK |
| aspnet-AspNetCore-Tooling  | namc             | Complete | ![done](https://findicons.com/files/icons/767/wp_woothemes_ultimate/16/checkmark.png) | | |
| aspnet-EntityFrameworkCore | namc             | Complete | ![done](https://findicons.com/files/icons/767/wp_woothemes_ultimate/16/checkmark.png) | | |
| aspnet-Extensions          | namc             | Complete | ![done](https://findicons.com/files/icons/767/wp_woothemes_ultimate/16/checkmark.png) | | Not building using Arcade SDK |
| CLI                        | licavalc         | Complete | ![done](https://findicons.com/files/icons/767/wp_woothemes_ultimate/16/checkmark.png) | | |
| CLICommandLineParser       | licavalc         | Complete | ![done](https://findicons.com/files/icons/767/wp_woothemes_ultimate/16/checkmark.png) | | |
| CLI-Migrate                | licavalc         | Complete | ![done](https://findicons.com/files/icons/767/wp_woothemes_ultimate/16/checkmark.png) | | |
| CoreClr                    | russellk         | Complete | ![done](https://findicons.com/files/icons/767/wp_woothemes_ultimate/16/checkmark.png) | [plan](MigrationPlan/CoreClrPlan.md) | CI using Azure DevOps and Jenkins (outerloop), not building using the Arcade SDK |
| CoreFx                     | danmose          | Complete | ![done](https://findicons.com/files/icons/767/wp_woothemes_ultimate/16/checkmark.png) | [plan](https://github.com/dotnet/arcade/blob/master/Documentation/MigrationPlan/CoreFxPlan.md) | |
| Core-SDK                   | licavalc         | Complete | ![done](https://findicons.com/files/icons/767/wp_woothemes_ultimate/16/checkmark.png) | [plan](https://github.com/dotnet/cli/blob/master/Documentation/MigrationPlan/CLISDKPlan.md) | |
| Core-Setup                 | dleeapho         | Complete | ![done](https://findicons.com/files/icons/767/wp_woothemes_ultimate/16/checkmark.png) | [plan](https://microsoft.sharepoint.com/teams/dotNETDeployment/_layouts/15/WopiFrame.aspx?sourcedoc={55410205-ac38-469b-81b0-9a93cc71b07c}&action=edit&wd=target%28Syncs.one%7C0a903b24-10b7-4c18-918c-5a380ba66433%2FCore-Setup%20%20pipebuild%20to%20yaml%7C4fb71b1d-1f36-41ee-8438-f1ea531c99e2%2F%29)| Not building using Arcade SDK |
| MSBuild                    | licavalc         | Complete | ![done](https://findicons.com/files/icons/767/wp_woothemes_ultimate/16/checkmark.png) | | Official builds are still building out of devdiv |
| Roslyn                     | jaredpar         | Complete | ![done](https://findicons.com/files/icons/767/wp_woothemes_ultimate/16/checkmark.png) | [plan](https://github.com/dotnet/arcade/blob/master/Documentation/MigrationPlan/roslyn.md) | Official builds are still building out of devdiv |
| SDK                        | licavalc         | Complete | ![done](https://findicons.com/files/icons/767/wp_woothemes_ultimate/16/checkmark.png) | | |
| Standard                   | danmose          | Complete | ![done](https://findicons.com/files/icons/767/wp_woothemes_ultimate/16/checkmark.png) | [plan](https://github.com/dotnet/arcade/blob/master/Documentation/MigrationPlan/CoreFxPlan.md) |  |
| SymReader                  | tmat             | Complete | ![done](https://findicons.com/files/icons/767/wp_woothemes_ultimate/16/checkmark.png) | [plan](https://github.com/dotnet/symreader/issues/157) | |
| SymReader-Portable         | tmat             | Complete | ![done](https://findicons.com/files/icons/767/wp_woothemes_ultimate/16/checkmark.png) | [plan](https://github.com/dotnet/symreader-portable/issues/144) | |
| Templating                 | vramak           | Complete | ![done](https://findicons.com/files/icons/767/wp_woothemes_ultimate/16/checkmark.png) | [plan](https://github.com/dotnet/arcade/blob/master/Documentation/MigrationPlan/TemplatingAndWebSdkPlan.md) | |
| Test-Templates             | sasin            | Complete | ![done](https://findicons.com/files/icons/767/wp_woothemes_ultimate/16/checkmark.png) | [plan](https://github.com/dotnet/arcade/blob/master/Documentation/NetCore3EngineeringRepoStatus.md#test-templates) | |
| Toolset                    | licavalc         | Complete | ![done](https://findicons.com/files/icons/767/wp_woothemes_ultimate/16/checkmark.png) | [plan](https://github.com/dotnet/cli/blob/master/Documentation/MigrationPlan/CLISDKPlan.md) | |
| Visual FSharp              | brettfo          | Complete | ![done](https://findicons.com/files/icons/767/wp_woothemes_ultimate/16/checkmark.png) | [plan](https://github.com/dotnet/arcade/blob/master/Documentation/MigrationPlan/VisualFSharp.md) | Not using Arcade SDK, official builds are being produced out of devdiv. |
| WebSDK                     | vramak           | Complete | ![done](https://findicons.com/files/icons/767/wp_woothemes_ultimate/16/checkmark.png) | [plan](https://github.com/dotnet/arcade/blob/master/Documentation/MigrationPlan/TemplatingAndWebSdkPlan.md) | |
| WinForms                   | mmcgaw           | Complete | ![done](https://findicons.com/files/icons/767/wp_woothemes_ultimate/16/checkmark.png) | | Also building out of dotnet-trusted |
| WPF                        | vatsan-madhavan  | Complete | ![done](https://findicons.com/files/icons/767/wp_woothemes_ultimate/16/checkmark.png) | | Also building out of dotnet-trusted |

| Status   | Description |
| -------- | ----------- |
| Complete | All work streams are complete |
| On track | Work streams are on track for completion by the target completion date or exceptions are understood / acceptable |
| At risk  | One or more work streams are not on track to be completed by the target completion date and may impact business decisions |


<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CNetCore3EngineeringRepoStatus.md)](https://helix.dot.net/f/p/5?p=Documentation%5CNetCore3EngineeringRepoStatus.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CNetCore3EngineeringRepoStatus.md)</sub>
<!-- End Generated Content-->
