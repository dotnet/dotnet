# Onboarding onto Arcade

- Onboard onto the Arcade SDK, which provides templates (building blocks) for
  interacting with Azure DevOps, as well as shared tooling for signing,
  packaging, publishing and general build infrastructure.  
  
  Resources: [Reference documentation](ArcadeSdk.md), [walkthough video](https://msit.microsoftstream.com/video/e22d2dad-ef72-4cca-9b62-7e33621f86a1), [feature documentation](CorePackages/).

   Steps:
    1. Add a [global.json](https://github.com/dotnet/arcade/blob/main/global.json).
    2. Add (or copy) [Directory.Build.props](https://github.com/dotnet/arcade/blob/main/Directory.Build.props)
       and [Directory.Build.targets](https://github.com/dotnet/arcade/blob/main/Directory.Build.targets).
    3. Copy `eng\common` from [Arcade](https://github.com/dotnet/arcade/tree/main/eng/common) into repo.
       Before committing to make sure that all `.sh` files have executable permission ([`git add --chmod=+x *.sh`](https://stackoverflow.com/questions/14267441/automatically-apply-git-update-index-chmod-x-to-executable-files)).
    4. Add (or copy) the
       [Versions.props](https://github.com/dotnet/arcade/blob/main/eng/Versions.props)
       and
       [Version.Details.xml](https://github.com/dotnet/arcade/blob/main/eng/Version.Details.xml)
       files to your `eng\` folder. Adjust the version prefix and prerelease label
       as necessary. Only include versions for dependencies required by the repository.
    5. Add the following feeds to your nuget.config:
       * `https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet-public/nuget/v3/index.json`
       * `https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet-tools/nuget/v3/index.json`
       * `https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet-eng/nuget/v3/index.json`
       * `https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet8/nuget/v3/index.json` (replace "8" with the latest .NET version)
       along with any other feeds your repo needs to restore packages from. You can see which feeds Arcade uses at: [NuGet.config](https://github.com/dotnet/arcade/blob/main/NuGet.config).

    See [documentation](CorePackages/) for information on using various Arcade SDK sub-systems.

- Set up pipelines
  - Add pipeline to https://dnceng-public.visualstudio.com/public for public PR validation CI.
  - Add pipeline to https://dev.azure.com/dnceng/internal for internal validation CI and official builds.
  - Add pipeline to https://dev.azure.com/dnceng/internal for CodeQL compliance validation.

  See [Onboarding Azure DevOps](AzureDevOps/AzureDevOpsOnboarding.md) and [Moving Official Builds from DevDiv to DncEng](AzureDevOps/MovingFromDevDivToDncEng.md) for details.

- Onboard onto dependency flow using `darc` tool in order to:
  - Push scheduled updates with latest Arcade bits
  - Flow changes between GitHub and an internal Azure DevOps mirror
  - Subscribe the repo to publish nuget packages to a target feed

  See [Dependency Flow Onboarding](DependencyFlowOnboarding.md) for details.

- Use [Helix](/Documentation/Helix.md) for testing where possible 

  See [Sending Jobs to Helix](https://github.com/dotnet/arcade/blob/main/Documentation/AzureDevOps/SendingJobsToHelix.md) for details.

<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5COnboarding.md)](https://helix.dot.net/f/p/5?p=Documentation%5COnboarding.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5COnboarding.md)</sub>
<!-- End Generated Content-->
