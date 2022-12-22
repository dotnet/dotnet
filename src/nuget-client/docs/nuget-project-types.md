# NuGet Project Types

NuGet's primary use-case is restoring packages for a project, hence an abstraction for projects is an important part of the software architecture. Especially for features which can install, upgrade, or uninstall packages.

Using Visual Studio's "find implementations" feature and then quickly checking each class to determine the inheritance hierarchy, these are the classes that implement `NuGetProject`:

* [NuGetProject](#nugetproject)
  * [BuildIntegratedNuGetProject](#buildintegratednugetproject)
    * [CpsPackageReferenceProject](#cpspackagereferenceproject)
    * [LegacyPackageReferenceProject](#legacypackagereferenceproject)
  * [FolderNuGetProject](#foldernugetproject)
    * [InstallCommandProject](#installcommandproject)
  * [MSBuildNuGetProject](#msbuildnugetproject)
    * [VsMSBuildNuGetProject](#vsmsbuildnugetproject)
  * [PackagesConfigNuGetProject](#packagesconfignugetproject)
  * [ProjectJsonNuGetProject](#projectjsonnugetproject)
    * [VsProjectJsonNuGetProject](#vsprojectjsonnugetproject)

These classes are implemented in `NuGet.PackageManagement`, `NuGet.PackageManagement.VisualStudio`, with a special mention that `InstallCommandProject` is implemented in `NuGet.CommandLine`.

## Visual Studio architecture

In Visual Studio, only the project system is supposed to interact with the project file and MSBuild, and other components (such as NuGet) should communicate with the project system itself. This means when NuGet needs to write changes (package was installed, upgraded, or uninstalled), NuGet needs to call APIs on the project system to have changes made to the project file.

For reading package information, there are two different designs, a pull and a push model. The push model is used only by `CpsPackageReferenceProject`. The pull model is used by all other Visual Studio `NuGetProject` implementations.

The pull model means that when `GetInstalledPackagesAsync` is called, NuGet will call APIs on the project system. Since NuGet has no way of knowing when the project system has changed, NuGet does not cache this information, and the project system will be called every time `GetInstalledPackagesAsync` is called. Since a project system is typically exposed over COM, rather than being a direct .NET managed object reference, this means that all calls to the project system must be on the main (UI) thread.  Therefore, NuGet cannot query multiple projects in parallel, and if the UI thread is being used by any other VS component, it increases latency of the result.

The push model used by `CpsPackageReferenceProject` means that when the project is loaded, or any time that the CPS project has any change it thinks may be relevant to NuGet, they call NuGet's `IVsSolutionResolveService`'s `NominateProjectAsync` method. You may hear NuGet or CPS/dotnet project system team members talk about project nomination. This is what it's about. This project nomination is how NuGet learns what target frameworks the project is targeting, and what PackageReference, PackageDownload, FrameworkReference and other properties, are defined. NuGet caches this nomination data in its `VsProjectSystemCache` class, and when NuGet APIs like `GetInstalledPackagesAsync` or `GetPackageSpecsAndAdditionalMessagesAsync` is called, NuGet uses the information in the cache.

## Project summaries

### BuildIntegratedNuGetProject

This class was introduced for `project.json`, which was the project format for preview versions of .NET Core 1.0, and then adapted when .NET Core returned to MSBuild `csproj` files (although with much simplified content). The name Build Integrated represents that this is the first time that restore is naturally part of the build system, unlike `packages.config`.

### CpsPackageReferenceProject

The [Common Project System (CPS)](https://github.com/microsoft/VSProjectSystem) is a new framework to build Visual Studio project systems, that simplifies the process compared to the previous sample. It is what the SDK style .NET project system is based on, hence this project system is typically only used for SDK style .NET projects.

It is the only project system to use the push model, described above. Additionally, the NuGet and .NET Project System teams have an agreement that NuGet will not log restore messages to Visual Studio's Error List window. Instead, NuGet must write all messages to the `project.assets.json` file, and the project system will replay the messages in Visual Studio's Error List window. Hence, even in catastrophic failures where NuGet is unable to perform a restore (such as nomination data is invalid), NuGet must still generate a `project.assets.json` file, just to report the errors.

### FolderNuGetProject

This is not actually a project type, but represents a folder/directory where packages are extracted, such as the global packages directory or solution packages directory. I'm not sure why it extends `NuGetProject`, as it violates the [is-a design guideline for object oriented design](https://en.wikipedia.org/wiki/Is-a). My best guess is because `NuGetProject` has a `InstallPackageAsync` and `DeletePackageAsync` method, which are actions that `FolderNuGetProject` need to implement, despite the fact it's not actually a project.

### InstallCommandProject

This project class is implemented in `NuGet.CommandLine`, which is the project for `nuget.exe`. `nuget.exe` supports running `nuget.exe install <package_id>` which will treat the current folder as a `FolderNuGetProject`, and then install (extract) the requested package into it. I'm not sure how its behavior differs from `FolderNuGetProject`.

### LegacyPackageReferenceProject

Although `PackageReference` was added to support .NET Core and the new project format that it introduced, from an MSBuild point of view, the new and old project styles are indistinguishable. Therefore, `PackageReference` could be brought to the older project format with a significant amount of code reuse, and it was implemented. Unlike `CpsPackageReferenceProject`, this project type uses the pull model. Additionally, the project system itself is written in C++, so NuGet sees it via .NET's COM interop. As a result, all interactions between the project system and NuGet must happen on the main (UI) thread.

This project type is typically used by non-SDK style .NET projects that use `PackageReference`, rather than `packages.config`.

### MSBuildNuGetProject

This is the project type that represents a project that supports NuGet, but either uses `packages.config`, or does not have any packages installed and has not explicitly opt into `PackageReference` restore style. It's typically used by non-SDK style .NET projects, but is sometimes also used by packaging projects like Service Fabric or WiX that pretend to support .NET only so MSBuild's project references work.

### NuGetProject

This is the base, abstract type used by all the project implementations. It has some shared code, and defines the abstract/virtual methods that other projects need to implement.

### PackagesConfigNuGetProject

This represents a `packages.config` file itself, not the project that uses `packages.config`. I'm not sure why it extends `NuGetProject`, as it violates the [is-a design guideline for object oriented design](https://en.wikipedia.org/wiki/Is-a). My best guess is because `NuGetProject` has a `InstallPackageAsync` and `DeletePackageAsync` method, which are actions that `PackagesConfigNuGetProject` need to implement, despite the fact it's not actually a project.

### ProjectJsonNuGetProject

In early .NET Core development, the ASP.NET team experimented with replacing the long and complicated MSBuild XML project file with a simplified JSON project file, `project.json`. This is basically what NuGet's `PackageSpec` represents, used by all `PackageReference` via `DependencyGraphSpec` and the `project.assets.json` file. In order to support solution actions like restore and build, the ASP.NET team had to introduce an MSBuild file, and they used the `xproj` file extension

### VsMSBuildNuGetProject

As described in the above [Visual Studio architecture](#visual-studio-architecture) section, components in Visual Studio should not read/write the project file directly, but should go through the appropriate Visual Studio project system. Therefore, NuGet needs to have references to project system packages/assemblies. Since `NuGet.CommandLine` (`nuget.nexe`) references `FolderNuGetProject` and therefore `NuGet.PackageManagement`, `NuGet.PackageManagement` had to remove all the VS specific code and use OO polymorphism to delegate all this Visual Studio specific code to another class in a different project/assembly. Hence, `VsMSBuildNuGetProject` is the implementation of [`MSBuildNuGetProject`](#msbuildnugetproject)'s Visual Studio project system communication.

### VsProjectJsonNuGetProject

See the description for [`VsMSBuildNuGetProject`](#vsmsbuildnugetproject). This class is the Visual Studio communication code for `ProjectJsonNuGetProject`.