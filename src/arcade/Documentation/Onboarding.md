# Onboarding onto Arcade

- Onboard onto the Arcade SDK, which provides templates (building blocks) for
  interacting with Azure DevOps, as well as shared tooling for signing,
  packaging, publishing and general build infrastructure.  
  
  Resources: [Reference documentation](ArcadeSdk.md), [walkthough video](https://msit.microsoftstream.com/video/e22d2dad-ef72-4cca-9b62-7e33621f86a1), [feature documentation](CorePackages/).

   Steps:
    1. Add a
       [global.json](https://github.com/dotnet/arcade/blob/main/global.json).
    2. Add (or copy)
       [Directory.Build.props](https://github.com/dotnet/arcade/blob/main/Directory.Build.props)
       and
       [Directory.build.targets](https://github.com/dotnet/arcade/blob/main/Directory.Build.targets).
    3. Copy `eng\common` from
       [Arcade](https://github.com/dotnet/arcade/tree/main/eng/common)
       into repo.
    4. Add (or copy) the
       [Versions.props](https://github.com/dotnet/arcade/blob/main/eng/Versions.props)
       and
       [Version.Details.xml](https://github.com/dotnet/arcade/blob/main/eng/Version.Details.xml)
       files to your eng\ folder. Adjust the version prefix and prerelease label
       as necessary.
    5. Add the following feeds to your nuget.config:
       * https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet-public/nuget/v3/index.json
       * https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet-tools/nuget/v3/index.json
       * https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet-eng/nuget/v3/index.json
       
       along with any other feeds your repo needs to restore packages from. You can see which feeds Arcade uses at: [NuGet.config](https://github.com/dotnet/arcade/blob/main/NuGet.config).

    **Using Arcade packages** - See [documentation](CorePackages/) for
    information on specific packages.

- Move out of .NET CI and into our new Azure DevOps project
  (https://dev.azure.com/dnceng/public) for your public CI. - See [Onboarding Azure DevOps](AzureDevOps/AzureDevOpsOnboarding.md).
- Move out of the devdiv Azure DevOps instance (https://dev.azure.com/devdiv/ or
  https://devdiv.visualstudio.com) and into the internal project for
  (https://dev.azure.com/dnceng/internal) internal CI and official builds. - See
  [Onboarding Azure DevOps](AzureDevOps/AzureDevOpsOnboarding.md) and [Moving Official Builds from
  DevDiv to DncEng](AzureDevOps/MovingFromDevDivToDncEng.md).
- Onboard onto dependency flow (Darc). - See [Dependency Flow
  Onboarding](DependencyFlowOnboarding.md).
- Use [Helix](/Documentation/Helix.md) for testing where possible - See [Sending Jobs to Helix](https://github.com/dotnet/arcade/blob/main/Documentation/AzureDevOps/SendingJobsToHelix.md)

## Which branches should I make these changes in?

Prioritize branches that are producing bits for .NET 5. Given the extended
support lifecycle for .NET Core 3.1, backporting infrastructure to .NET Core 3.1
release branches is desired, but .NET 5 branches should go first.


<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5COnboarding.md)](https://helix.dot.net/f/p/5?p=Documentation%5COnboarding.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5COnboarding.md)</sub>
<!-- End Generated Content-->
