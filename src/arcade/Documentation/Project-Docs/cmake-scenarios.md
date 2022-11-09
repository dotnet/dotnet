This document presents the scenarios for CMake in .NET Core. 

# Summary
[CMake](https://cmake.org/overview/) is a prerequisite for building .NET Core repositories such as CoreFx and CoreCLR. When a developer attempts to build, a repository using the build script, the script probes for CMake on the machine, and if CMake is not found then, the script terminates and the build fails. There is no guidance for developer on which version of CMake to install. Not stating a version of CMake for a repository can lead to further challenges for example, when a servicing a release. 

Thus, there is need to improve CMake usage and acquisition experience. Hence the purpose of this document is to define the scenarios for CMake in .NET Core, and lay a foundation for designing the improved experience.

# CMake Scenarios
Each subheading below is a name of a scenario. Under each subheading there will be a short description of the scenario and a narrative. The narrative includes an actor, flow of events, and the resulting outcomes. An outcome consists of one or more pairs of the current and desired experiences.

 * [Build a .NET Core repository on a clean machine](#build-a-net-core-repository-on-a-clean-machine)
 * [Build a .NET Core repository on an existing development machine](#build-a-net-core-repository-on-an-existing-development-machine)
 * [Setup an official build for a .NET Core repository](#setup-an-official-build-for-a-net-core-repository)
 * [Service a .NET Core release](#service-a-net-core-release)
 * [Revise the CMake version of a .NET Core repository](#revise-the-cmake-version-of-a-net-core-repository)
 * [A new version of CMake can be tested against a .NET Core repository](#a-new-version-of-cmake-can-be-tested-against-a-net-core-repository)
 * [Guidance for setting up official builds for a .NET Core repository is available](#guidance-for-setting-up-official-builds-for-a-net-core-repository-is-available)

## Build a .NET Core repository on a clean machine
A .NET Core user (Microsoft employee or someone in the open) clones a .NET Core repository and attempts to build.

### Narrative
I am an IT Manager from Contoso who was inspired by .NET Core demos at [Connect(); 2016](https://msdn.microsoft.com/en-us/magazine/connect16mag.aspx), and I would like to contribute to .NET Core. 

Flow of events:
 1. I setup a clean Windows 10 VM through my Azure subscription.
 2. I cloned CoreFx repository from [dotnet/corefx](https://github.com/dotnet/corefx.git)
 3. I attempted to build the repository using the build command (build.cmd)

### Outcome #1 Build fails
**Current**: Build fails saying CMake, which is a prerequisite, is missing on the VM.  Though the error message provides an URL from where CMake can be downloaded, it does not list a specific version or a range of supported versions. I am not certain on what version to download.

**Desired**: 
Build probes for CMake in .NET Core sandbox tools folder in addition to the current probing locations. If CMake is not found, then the build attempts to acquire the declared version of CMake from the ***tools*** cache. If acquisition fails, then the build presents an error message that informs the user the specific version of CMake to download, and the suggested source to download it from. I have two options from here:

 - I download the specific version and perform a default install. *(TBD: Should the user restart Command or Terminal window?).* 
 - Perform a gesture described in the error message to acquire CMake. I perform the gesture so that a tool downloads and extracts the declared version of CMake to a .NET Core sandbox tools folder.

Either of the above options allow me to run build command successfully.

## Build a .NET Core repository on an existing development machine
A .NET Core user would like to clone a .NET Core repository to his/her existing development machine, and build that repository.

### Narrative
I am an IT Manager from Contoso who was inspired by .NET Core demos at [Connect(); 2016](https://msdn.microsoft.com/en-us/magazine/connect16mag.aspx), and I would like to contribute to .NET Core. 

Flow of events:
 1. On my existing development machine, I cloned CoreFx repository from
    [dotnet/corefx](https://github.com/dotnet/corefx.git)    
 2. I attempted to build the repository using the build command (build.cmd)

### Outcome #1: Build succeeds
**Current**: I have no indication about the CMake version used.

**Desired**: I can refer to a build artifact to find the version of CMake used in the build.

Note: Since an existing development machine is being used, the machine could have the declared version of CMake.

### Outcome #2: Build fails 
**Current**:  

 - Build fails saying CMake, which is a prerequisite is not available on the machine. I try to download CMake based on the error message. I am not certain on what version to download.
 - Build fails in strange ways due to a version of CMake present on the machine, and thus making it difficult to trace back.

**Desired**: 

 1. On a clean machine, the desired experience should be same as earlier scenario [Build a .NET Core repository on a clean machine](#build-a-net-core-repository-on-a-clean-machine)
 2. On an existing machine that has a version of CMake, which is different than the declared version, there are two outcomes:
  1. Default outcome is build consumes the available CMake version.
  2. I can ensure the build consumes the declared version of CMake. This means if the build detects that the version of CMake available is not the declared version, then the build attempts to acquire the declared version. This acquisition experience should be same as in the earlier scenario [Build a .NET Core repository on a clean machine](#build-a-net-core-repository-on-a-clean-machine). 

Either of the above options allow me to run build command successfully.
 
## Setup an official build for a .NET Core repository
A .NET Core repository owner would like to setup a reliable, repeatable and trustable process of producing official builds.

### Narrative
I am the owner of [.NET CoreFx](https://github.com/dotnet/corefx) repository.  I would like to setup a process that will produce reliable, repeatable and trustable builds for this repository. 

Flow of events:

 1. Created a new VSTS build definition that runs the build command of the repository. 
 2. Ensured the builds succeed

### Outcome #1: Official builds consume the declared version of CMake.

**Current**: .NET Core repository is built using a version of CMake installed while setting up the VM. Version of CMake is not logged in build artifact. 

**Desired**:
 1. I can find the declared version of CMake for CoreFx or any .NET Core repository within the repository itself.
 2. I can setup official build of a .NET Core repository such that the build acquires the declared version of CMake from OSS Tools repository, and places it in a sandbox location.
 3. I can ensure that the official build utilizes CMake tool from the sandbox folder.

Note: OSS Tool repository will download CMake source code, security audit the source code, build and then host. Thus, minimizing any security risks that might arise when CMake is consumed directly from internet.

### Outcome #2: Official builds setup and maintenance is reliable and costs lowered
**Current**: Build agents are tied to a specific version of CMake.

**Desired**: Build agents are more contained and not dependent on a particular build agent for CMake installations. Thus, the same agent can build multiple repositories and branches.

## Service a .NET Core release
A .NET Core team member who would like to service a release, and needs the release configuration to rebuild.

### Narrative
I am a .NET Core team member who is assigned to service a [CoreFx](https://github.com/dotnet/corefx) release to address an issue reported in the product. 

Flow of events:
 1. I checked out the release branch on my local developer machine. Understood the root cause of the reported issue.
 2. I created a service branch i.e., fork from release branch, and have a fix ready.
 3. I followed the developer guidelines available for that branch to perform a build.

### Outcome: Service branch builds consume the declared version of CMake
**Current**: Since a declared version of CMake is not available within the repository, I will build the service branch using the latest version of CMake available. This latest version of CMake might introduce new product behaviors that I will have to resolve. Thus, lack of declared version introduces uncertainty and additional costs in servicing a branch.

**Desired**:
 1. I can run the build such that if the required toolset does not match then, the build acquires the required toolset.
 2. I can find out the CMake version used to build the release branch. Declared version is available within the repository itself.
 3. I can acquire and run build with tools in a sandbox folder.

## Revise the CMake version of a .NET Core repository
A .NET Core contributor can update the declared CMake version for a given .NET Core repository.

### Narrative
I am a .NET Core contributor working on new features in CoreFx. These features require the latest version of CMake.

Flow of events:
 1. I verified the compatibility of existing features with the new version of CMake.
 2. I would like to update the product to be built using this new version of CMake.

### Outcome: All build scenarios are aware of the new version of CMake
**Current**: Though as a .NET Core contributor I can build a local repository using different versions of CMake, doing the same with official builds involves cost such as updating build definition, notifying repository owners, and finally inform the open community of users about the new version of CMake.

**Desired**: As a .NET Core contributor I can modify declared CMake version, and submit this change as a pull request (PR). The change to declared CMake version is reflected in all build scenarios. This means, the official builds will pick up the new version from OSS Tools repository, and other scenarios reflect the change to the declared version of CMake.

Note: A requirement for this scenario is that the new version of CMake should be available on OSS Tool repository.

## A new version of CMake can be tested against a .NET Core repository
A .NET Core team member can try a new version of CMake to build a .NET Core repository.

### Narrative
As a team member of .NET Core or Engineering Services, I would like to check the applicability of a new version of CMake in a .NET Core repository. For instance, verify if a new version of CMake is compatible with a .NET Core repository, and no unexpected regressions in the product are introduced.

Flow of events:
 1. I built the local repository using the new version of CMake. Ensure build succeeds.
 2. I would like to test official builds produced with this new version of CMake.

### Outcome: A test run of official build consuming the new CMake package can be performed
**Current**: Updating an official build to test a new version of CMake involves the following steps -
 1. Creating a new agent pool
 2. Adding VMs to the pool where each VM has the new version of CMake installed
 3. Setting up new build definitions that point to this pool

**Desired**:
 1. I'm able to build a .NET Core repository using a CMake version, which is not in the declared version.
 2. I can perform a test run of official build and ensure that the CMake package is consumed, and build succeeded.

## Guidance for setting up official builds for a .NET Core repository is available
A .NET Core repository owner refers to documentation that describes the procedure to setup up official builds.

### Narrative
I'm team member on Red Hat.  I would like to setup official builds of our CoreFx repository using the same CMake version used in .NET Core official builds.

Flow of events:
 1. I forked CoreFx repository from [dotnet/corefx](https://github.com/dotnet/corefx.git)
 2. I followed the developer guidelines to setup official builds

### Outcome: .NET Core users refer to documentation about setting up official builds
**Current**: In my attempt to setup official builds, I will use the latest version of CMake in official builds since CoreFx documentation does not provide any guidance on declared CMake version. Native binaries from Red Hat build is now significantly different from those produced in .NET Core product team's official builds. This difference is due to the different versions of CMake used in the respective builds.

**Desired**: 
 1. Declared version of the .NET Core repository is available within the repository itself.  I can find it, and enforce it with my toolset story
 2. I can specify a version of CMake that should be consumed in the build command of my .NET Core repository.


<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CProject-Docs%5Ccmake-scenarios.md)](https://helix.dot.net/f/p/5?p=Documentation%5CProject-Docs%5Ccmake-scenarios.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CProject-Docs%5Ccmake-scenarios.md)</sub>
<!-- End Generated Content-->
