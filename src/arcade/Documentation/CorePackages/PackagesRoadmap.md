# Packages Roadmap
The goal of this document is to provide guidance on the definition and implementation of the "Core" packages that should be in the
Arcade SDK and that should be consumed by the [tier 1 repositories](..\TierOneRepos.md)

## Packages that should belong in the Arcade SDK
The following list provides a summary of the state of each package. For more information, the work is being tracked under the Epic [Publish core "set" of shared packages](https://github.com/dotnet/arcade/issues/46) where every issue is marked according to the package it belongs.

**Note:** As we start allocating resources to work on the different packages, specific issues/documentation will be created for each package.

### Versioning
- **Goal**: Have a consistent and reliable way of versioning .NET Core 3.0 repositories assets.
- **Where we are**:
  - Most of the work is completed.
  - There is documentation about it.
  - Already included in the Arcade SDK.
- **Principal TODOs**:
  - Finish implementing what is described in the [documentation](Versioning.md).
- **When could start being consumed by other repositories**: S139 => 8/10

### Signing
- **Goal**: Sign .NET Core 3.0 binaries in a uniform, consistent and reliable way
- **Where we are**:
  - Signing plan [here](SigningPlan.md).
  - Signtool from Reprotoolset ported to Arcade and it is part of the Arcade SDK.
  - SignTool can be consumed as an MsBuild task.
  - Arcade is consuming SingTool to sign its binaries.
- **Principal TODOs**:
  - Implement the ability to pass an ItemGroup to SignTool, instead of using SingToolData.json.
  - Extract the certificates and strong names from the metadata files.
  - Add documentation on how to consume the SignTool.
- **When could start being consumed by other repositories**: S140 => 8/31

### Publishing
- **Goal**: Standardized the way to publish packages and symbols to the respective channels for all .NET Core 3.0 repositories.
- **Where we are**: 
  - Publishing plan [here](PublishingPlan.md)
  - Arcade is consuming the Publishing package.
  - Transport feeds from Buildtools is now in Arcade.
- **Principal TODOs**:
  - Send information to Darc/Maestro.
  - Add documentation to Arcade.
- **When could start being consumed by other repositories**: S141 => 9/21

### Packaging
- **Goal**: Pack the binaries produced by the .NET Core 3.0 repositories in a unified way.
- **Where we are**: Initial code is already part of Arcade SDK => only for NuGet packages.
- **Principal TODOs**: 
  - Make sure what we currently have will work for most repositories. Refactor where necessary.
  - Add documentation.
- **When could start being consumed by other repositories**: S141 => 9/21

### Telemetry
- **Goal**: Provide telemetry about every task executed as part of building .NET Core 3.0.
- **Where we are**: Scripts to consume the Telemetry infrastructure are already in Arcade => eng/common
- **Principal TODOs**:
  - Make sure what we currently have will work for most repositories. Refactor where necessary.
  - Add documentation.
- **When could start being consumed by other repositories**: S141 => 9/21


<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CCorePackages%5CPackagesRoadmap.md)](https://helix.dot.net/f/p/5?p=Documentation%5CCorePackages%5CPackagesRoadmap.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CCorePackages%5CPackagesRoadmap.md)</sub>
<!-- End Generated Content-->
