# Servicing Readiness Adoption status

## Primary Deliverables

1. Transition to [YAML stages based publishing](./CorePackages/YamlStagesPublishing.md) and the post-build YAML template.

Target completion date is 8/13/2019.

## Status Overview

| Repo                       | Owner            | Status   | Risk Assessment                                                                      | Notes|
| ---------------------------| ---------------- | -------- |--------------------------------------------------------------------------------------| -----|
| Arcade                     | mawilkie         | Complete | ✔️ | |
| Arcade-Services            | mawilkie         | Complete | ✔️ | |
| Arcade-Validation          | mawilkie         | Complete | ✔️ | |
| aspnet-AspNetCore          | dougbu           | Complete | ✔️ | |
| aspnet-AspNetCore-Tooling  | dougbu           | Complete | ✔️ | |
| aspnet-EntityFramework6    | dougbu/wtgodbe   | Complete | ✔️ | |
| aspnet-EntityFrameworkCore | dougbu/wtgodbe   | Complete | ✔️ | |
| aspnet-Blazor              | dougbu           | Complete | ✔️ | |
| aspnet-Extensions          | dougbu/wtgodbe   | Complete | ✔️ | |
| CLI                        | licavalc         | Complete | ✔️ | |
| CLICommandLineParser       | licavalc         | N/A | |  This repo is not being developed anymore. We are taking a pinned version of it |
| CoreClr                    | jeffschw/arobins | Complete | ✔️ | |
| CoreFx                     | danmose/safern   | Complete | ✔️ | SourceLink validation disabled: https://github.com/dotnet/arcade/issues/3603 |
| IoT                        | joperezr         | Complete | ✔️ | |
| Core-SDK                   | licavalc         | In progress | ➖ |  Working in parallel.Will need https://github.com/dotnet/arcade/issues/3607 to be done before completing. |
| Core-Setup                 | dleeapho         | Complete | ✔️ | Uses workarounds and skips most validation. Uses custom publish steps. |
| FSharp                     | brettfo          | Complete | ✔️ | |
| MSBuild                    | licavalc         | Complete | ✔️ | |
| Roslyn                     | jaredpar         | Complete | ✔️ |  Complete with source link validation disabled |
| SDK                        | licavalc         | Complete | ✔️ | |
| SourceLink                 | tmat             | Complete | ✔️ | |
| Standard                   | danmose/wigodbe  | Complete | ✔️ | |
| SymReader                  | tmat             | Complete | ✔️ | |
| SymReader-Portable         | tmat             | Complete | ✔️ | |
| Templating                 | vramak           | Complete | ✔️ | |
| Test-Templates             | sasin            | At risk  | ❌ |  No plan available |
| Toolset                    | licavalc/riarenas| Complete | ✔️ | |
| WebSDK                     | vramak           | Complete | ✔️ | |
| WinForms                   | mmcgaw           | Complete | ✔️ | |
| WPF                        | vatsan-madhavan  | Complete | ✔️ | Some reliability problems being observed, for e.g., https://github.com/dotnet/arcade/issues/3609| 

| Status     | Description |
| ---------- | ----------- |
| Complete ✔️| All work streams are complete |
| On track ➖| Work streams are on track for completion by the target completion date or exceptions are understood / acceptable |
| At risk  ❌| One or more work streams are not on track to be completed by the target completion date and may impact business decisions |


<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CYamlStagesRepoStatus.md)](https://helix.dot.net/f/p/5?p=Documentation%5CYamlStagesRepoStatus.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CYamlStagesRepoStatus.md)</sub>
<!-- End Generated Content-->
