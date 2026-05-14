# .NET Core 3 repos move to Release Pipelines status

## What is needed?

*  Repositories do signing and symbol validation as well as publishing of symbols and packages in an 
async release pipeline independent from the build. Specifics on how this works can be found [here](https://github.com/dotnet/arcade/blob/master/Documentation/CorePackages/AsyncPublishing_HowToUse.md).

## Batches

The order in which we'll move things to release pipelines will be by batches mostly defined 
by how easy would the move be. Also, we won't move to the next batch until all repos in the 
previous batch have been moved and we have validated that things are working for them.

## Arcade-like repositories

These repositories publish using the packages provided by arcade and make use of the shared 
templates following the same processes as Arcade, so, since using release pipelines is already on 
for the arcade-* repos, moving the following should, in theory, be straight forward. For details
on what changes are needed check ["How do I use it?"](https://github.com/dotnet/arcade/blob/master/Documentation/CorePackages/AsyncPublishing_HowToUse.md#how-do-i-use-it).

### Potential ETAs*

- Batch 1: 4/10/2019 
- Batch 2: 4/15/2019
- Batch 3: 4/17/2019

| Repo                       | Owner            |  Status   | Batch | Notes              |
| ---------------------------| ---------------- | --------- | ----- | -------------------|
| Arcade                     | mawilkie         |  Moved    |       |                    |
| Arcade-Validation          | mawilkie         |  Moved    |       |                    |
| Arcade-Services            | mawilkie         |  Moved    |       |                    |
| Standard                   | danmose          |  Moved    |   1   |                    |
| SymReader                  | tmat             |  Moved    |   1   |                    |
| SymReader-Portable         | tmat             |  Moved    |   1   |                    |
| CoreFx                     | danmose          | Not-Moved |   1   | Direct access to Task.Feed tasks                   |
| Templating                 | vramak           |  Moved    |   1   |                    |
| Test-Templates             | singhsarab       |  Moved    |   1   |                    |
| Toolset                    | licavalc         | Not-Moved |   1   | Direct access to Task.Feed tasks   |
| CoreClr                    | russellk         | Not-Moved |   2   | Direct access to Task.Feed tasks                   |
| WebSDK                     | vramak           |  Moved    |   2   |                    |
| WinForms                   | mmcgaw           |  Moved    |   2   |    |
| WPF                        | vatsan-madhavan  |  Moved    |   2   |   |
| aspnet-EntityFrameworkCore | namc             |  Moved    |   2   |                    |
| aspnet-Extensions          | namc             |  Moved    |   2   |                    |
| aspnet-AspNetCore-Tooling  | namc             |  Moved    |   2   |                    |
| CLI                        | licavalc         |  Moved    |   3   |                    |
| CLI-Migrate                | licavalc         |  Moved    |   3   |                    |
| CLICommandLineParser       | licavalc         |  Moved    |   3   |                    |

*If a given batch is completed before its planned ETA, we'll move on to the next right away. Also, 
if the move takes more than what was estimated we'll move the rest of the batches forward

## Special repositories

Repositories which don't use Arcade SDK, are still building on devdiv or execute other actions different 
from signing and publishing packages and symbols i.e. push assets to two different feeds in the same task.

For these, we'd need to do extra work since the current implementation won't work for them. Issues 
currently tracking this work are:

* https://github.com/dotnet/arcade/issues/2371 for those repos not using Arcade SDK
* https://github.com/dotnet/arcade/issues/2398 for repos publishing to more than one feed
* Need to determine what is the plan for those repos building in devdiv. We could maybe support them by "cloning" the release
pipelines there but we'd need to special case the code and probably modify some table schemas on the DB

### Potential ETAs*

- Batch 4: TBD (depending on the completion of the issues above)

| Repo                       | Owner            |  Status   | Batch | Notes                 |
| ---------------------------| ---------------- | --------- | ----- | ----------------------|
| nuget-NugetClient          | dtivel           | Not-Moved |   4   | Not using Arcade SDK  |
| aspnet-AspNetCore          | namc             | Not-Moved |   4   | Not using Arcade SDK  |
| Core-Setup                 | dleeapho         | Not-Moved |   4   | Not using Arcade SDK  |
| MSBuild                    | licavalc         | Not-Moved |   4   | Building from devdiv  |
| Roslyn                     | jaredpar         | Not-Moved |   4   | Building from devdiv and doing VS Insertion |
| Visual-FSharp              | brettfo          | Not-Moved |   4   | Not using Arcade SDK and building from devdiv |
| Core-SDK                   | licavalc         | Not-Moved |   4   | Publishes to two feeds in the same task |


<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CNetCore3ReleasePipelinesMoveStatus.md)](https://helix.dot.net/f/p/5?p=Documentation%5CNetCore3ReleasePipelinesMoveStatus.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CNetCore3ReleasePipelinesMoveStatus.md)</sub>
<!-- End Generated Content-->
