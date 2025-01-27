# Debugging

For basics on how to set-up your repo and how to build the product, refer to the [contributing guide](../CONTRIBUTING.md).

## Debugging and testing NuGet.exe (NuGet.Commandline)

Given that it is a .NET Framework based x86 console application, NuGet.exe is straightforward to debug.

Set [NuGet.CommandLine](../src/NuGet.Clients/NuGet.CommandLine/NuGet.CommandLine.csproj) as the start-up project in the [NuGet.sln](../NuGet.sln), and set the parameters in Debug window for the project.

### Testing the build of NuGet.exe

Alternatively, if you build the the repository under a `Debug` configuration, which is the default, you can simply pass a `--debug` argument to NuGet.exe when invoking it to get a debugger prompt.

### Code pointers for NuGet.exe

* The entry point for all commands is [Program.cs](../src/NuGet.Clients/NuGet.CommandLine/Program.cs) in the same project.
* Each command in NuGet.exe has it's own class. For example if you want to debug restore, you can set a breakpoint in [RestoreCommand.cs](../src/NuGet.Clients/NuGet.CommandLine/Commands/RestoreCommand.cs). Note that there are multiple instances of the RestoreCommand type.

## Debugging and testing NuGet in Visual Studio

Testing the NuGet Visual Studio functionality is equally as easy as testing NuGet.exe.
The start-up project is [NuGet.VisualStudio.Client](../src/NuGet.Clients/NuGet.VisualStudio.Client/NuGet.VisualStudio.Client.csproj). Starting this project will build the VSIX, then install that VSIX onto and launch your [experimental instance](https://docs.microsoft.com/en-us/visualstudio/extensibility/the-experimental-instance) of Visual Studio.

> Note: NuGet's integration into Visual Studio depends on Visual Studio components. When testing this integration it is important that the version of NuGet being tested and the Visual Studio instance are compatible. The [release notes](https://learn.microsoft.com/nuget/release-notes/) contain the exact mapping, where the minor version of NuGet matches the minor version of Visual Studio, example 17.12 of Visual Studio matches 6.12 of NuGet.
Testing NuGet 6.12 on top of 17.8 for example is not guaranteed to work. 
Only testing 6.12 on top of 17.12 is expected to consistently work.

### Testing the build of NuGet in Visual Studio

NuGet functions as an extension on top of Visual Studio.
However NuGet is also considered a system component which means it cannot be managed in the same way as other Visual Studio extensions. However, it is possible to install a locally built NuGet in a Visual Studio instance.

#### Installing a custom version of the NuGet extension in Visual Studio

The build generates a vsix artifact in the `artifacts/VS15/` folder. From the [Developer Command Prompt for VS](https://docs.microsoft.com/en-us/dotnet/framework/tools/developer-command-prompt-for-vs) run the `VSIXInstaller.exe` with the vsix path as the first argument. Alternatively VS configures the default action for VSIX files.

#### Uninstalling a custom version of the NuGet extension from Visual Studio

Given that NuGet is a system component, you cannot use the extensions manager in Visual Studio to downgrade your NuGet extension to its original version. Go back to your [Developer Command Prompt for VS](https://docs.microsoft.com/en-us/dotnet/framework/tools/developer-command-prompt-for-vs) and run `VSIXInstaller.exe /d:NuGet.72c5d240-f742-48d4-a0f1-7016671e405b`.

#### NuGet in Visual Studio assembly location

Each Visual Studio instance has its own root install directory. Relative to the root directory, the NuGet assemblies can be found in `Common7/IDE/CommonExtensions/Microsoft/NuGet`.

### Code pointers for NuGet in Visual Studio

Visual Studio extensibility has the concept of VSPackages. This allows extensions to add capabilities to the UI and all of the flows within the IDE. The NuGet client itself ships 2 packages, which can be considered the entry points for the whole component. You wouldn’t normally debug the initialization of these packages of course, but they author the whole NuGet in VS experience in one or another.

The 2 packages in question are:

* [NuGetPackage](../src/NuGet.Clients/NuGet.Tools/NuGetPackage.cs) - enables most of the UI NuGet functionality in Visual Studio.
* [RestoreManagerPackage](../src/NuGet.Clients/NuGet.SolutionRestoreManager/RestoreManagerPackage.cs) - enables the restore functionality in Visual Studio.

The NuGet in Visual Studio experience is scattered across many assemblies as can be seen from the [project overview](project-overview.md).
Some of the other points are summarized below:

* Restore operations - [SolutionRestoreWorker](../src/NuGet.Clients/NuGet.SolutionRestoreManager/SolutionRestoreWorker.cs) - All Visual Studio restores start here. Currently, only a solution restore is possible. Depending on the project package management style, there are different code paths to look into.

* Package Manager UI operations - VS context menu integration is defined in [NuGetTools.vsct](../src/NuGet.Clients/NuGet.Tools/NuGetTools.vsct). The main control for the Package Manager UI is [PackageManagerControl](../src/NuGet.Clients/NuGet.PackageManagement.UI/Xamls/PackageManagerControl.xaml.cs).

* Package Manager Console operations - The PowerShell cmdlets are defined in the [NuGet.PackageManagement.Cmdlets](../src/NuGet.Clients/NuGet.PackageManagement.PowerShellCmdlets/NuGet.PackageManagement.PowerShellCmdlets.csproj) project, more specifically in the [cmdlets](../src/NuGet.Clients/NuGet.PackageManagement.PowerShellCmdlets/Cmdlets) folder.

#### Investigating NuGet and project-system interactions

When investigating Visual Studio PackageReference restore with SDK-based projects, you can set an environment variable respected by the [project-system](https://github.com/dotnet/project-system/pull/3027) that would dump the nomination data each time a project is nominated.

Set and look for a new category `Project` enabled in the Output window.

* 16.7 and earlier - set `PROJECTSYSTEM_PROJECTOUTPUTPANEENABLED=1`
* 16.8 and later - set `CPS_DiagnosticRuntime=1`

## Debugging and testing the NuGet MSBuild functionality

Exactly two NuGet functionalities are available in MSBuild, `restore` and `pack`.

### Restore in MSBuild

The [NuGet.targets](../src/NuGet.Core/NuGet.Build.Tasks/NuGet.targets) from the [NuGet.Build.Tasks](../src/NuGet.Core/NuGet.Build.Tasks/NuGet.Build.Tasks.csproj) project is the entry point for all restore scenarios from MSBuild and by association dotnet.exe.
In these targets there are various tasks involved during the construction of the PackageSpec/DependencyGraphSpec (NuGet's internal model for projects in PackageReference/project.json worlds while the main work is in RestoreTask.

### Pack in MSBuild

The Pack functionality is treated as an SDK and it's not imported in all projects by default like the restore functionality is. The user facing [docs](https://docs.microsoft.com/en-us/nuget/create-packages/creating-a-package-msbuild) cover this in more detail, the short explanation is that Pack functionality is available by default in [SDK based](https://docs.microsoft.com/en-us/dotnet/core/tools/csproj) projects, but not in old style csproj PackageReference based projects. To use pack in old style csproj, you need to install the NuGet.Build.Tasks.Pack package. Installing the package also works for SDK based projects, and overrides the built-in .NET SDK functionality.

 The [NuGet.Build.Tasks.Pack.targets](../src/NuGet.Core/NuGet.Build.Tasks.Pack/NuGet.Build.Tasks.Pack.targets) from the [NuGet.Build.Tasks.Pack](../src/NuGet.Core/NuGet.Build.Tasks.Pack/NuGet.Build.Tasks.Pack.csproj) project are the entry point for all pack scenarios from MSBuild and by association dotnet.exe.

### Testing and debugging restore in MSBuild

Given that all the MSBuild functionality comes with Visual Studio, one approach is to install the extension generated from the build, as described in [Testing the build of NuGet in Visual Studio](#testing-the-build-of-nuget-in-visual-studio).

Alternatively, the NuGet.targets are just wired into the build in a certain way and that can be replicated without a complete installation of the extension through the powershell helper functions as shown in the [PowerShell helper scripts](../scripts/nuget-debug-helpers.ps1). Examples can be found in the linked scripts.

If you are testing a debug build, to debug just set the environment variable defined in [RestoreTask](../src/NuGet.Core/NuGet.Build.Tasks/RestoreTask.cs), currently `DEBUG_RESTORE_TASK`.

### Testing and debugging pack in MSBuild

The naive approach here is to install the package to the project and just run `msbuild -t:pack` on it.

Alternatively, if you are testing on SDK based projects, the NuGet.Build.Tasks.Pack.targets are just wired into the build in a certain way and that can be replicated without a complete installation of the extension through the PowerShell helper functions as shown in the [PowerShell helper scripts](../scripts/nuget-debug-helpers.ps1). Examples can be found in the linked scripts.

If you are testing a debug build, to debug just set the environment variable defined in [PackTask](../src/NuGet.Core/NuGet.Build.Tasks.Pack/PackTask.cs), currently `DEBUG_PACK_TASK`.

## Debugging and testing the NuGet functionality in dotnet.exe

dotnet.exe has a lot of the NuGet functionality available. There are 3 different integrations enabling many scenarios.

* Restore - A wrapper for `msbuild -t:restore`, running on .NET Core.
* Pack - The pack SDK functionality is available in all SDK based projects.
* NuGet Commandline Xplat Functionality - An exe that's bundled with dotnet.exe and dotnet.exe shells out to our app by forwarding a set of arguments. Some of the scenarios enabled by this integration are: add/remove/list package, push, locals.

3 different integration, 3 different entry points and as such 3 different debugging approaches.

### Debugging restore task in dotnet.exe

dotnet.exe restore works the exact same way `msbuild -t:restore` works. The one difference is that .NET Core based MSBuild cannot build the same project types as .NET Framework MSBuild can.
Most of the tips in [Testing and debugging restore in MSBuild](#testing-and-debugging-restore-in-msbuild) still apply.

If you want to test dotnet.exe explicitly, refer to [Patching dotnet.exe to test the NuGet functionality](#patching-dotnetexe-to-test-the-nuget-functionality).

### Debugging pack task in dotnet.exe

dotnet.exe pack works the exact same way `msbuild -t:pack` works. The added benefit is that it is always available in all SDK based projects, which is what dotnet.exe supports.
Most of the tips in [Testing and debugging pack in MSBuild](#testing-and-debugging-pack-in-msbuild) still apply.

The easiest way to test the pack functionality with dotnet.exe is to install the NuGet.Build.Tasks.Pack package to the project you want to test, and run `dotnet.exe pack`.

If you want to test dotnet.exe explicitly, so you don't have to worry about whether you installed the correct package in the project, refer to [Patching dotnet.exe to test the NuGet functionality](#patching-dotnetexe-to-test-the-nuget-functionality).

### Debugging NuGet's integrated `dotnet` CLI commands

All of the `dotnet nuget` commands, in addition to some others, such as `dotnet add package`, `dotnet list package`, and `dotnet search`, are implemented in [NuGet.CommandLine.XPlat](../src/NuGet.Core/NuGet.CommandLine.XPlat/NuGet.CommandLine.XPlat.csproj).

There are 2 ways to debug this project:

* Given that [NuGet.CommandLine.XPlat](../src/NuGet.Core/NuGet.CommandLine.XPlat/NuGet.CommandLine.XPlat.csproj) is an exe, you can set it as the startup project and run it as you would any other command line project in Visual Studio.
   However, if that is not working, please make sure `UseMSBuildLocator` is set to true.
   **Note**: some commands list arguments in a different order to the dotnet cli.

* Patch the SDK by referring to [Patching dotnet.exe to test the NuGet functionality](#patching-dotnetexe-to-test-the-nuget-functionality).

After you have patched it, refer to the environment variable in the [NuGet.CommandLine.XPlat](../src/NuGet.Core/NuGet.CommandLine.XPlat/NuGet.CommandLine.XPlat.csproj) entry point.
If you are testing a debug build, to debug just set the environment variable defined in [Program.cs](../src/NuGet.Core/NuGet.CommandLine.XPlat/NuGet.CommandLine.XPlat.csproj), currently, `DEBUG_NUGET_XPLAT`.

### Patching dotnet.exe to test the NuGet functionality

There are 2 ways to patch dotnet.exe with the latest NuGet bits.

* Refer to the [PowerShell helper scripts](../scripts/nuget-debug-helpers.ps1) for a helper script to patch a zip of the SDK that can be acquired from the [dotnet installer](https://github.com/dotnet/installer/blob/master/README.md#installers-and-binaries). Note that when running you [might](https://github.com/dotnet/runtime/blob/master/docs/project/dogfooding.md) need to disable `DOTNET_MULTILEVEL_LOOKUP`. dotnet.exe has a special logic for the discovering the SDK, and it's possible it's discovering a different SDK from the one you patched.
* Refer to [dotnet/sdk](https://github.com/dotnet/sdk) repo to build it locally and test through that. The sdk consumes NuGet through packages, use `Ctrl + F` in your trusty editor to figure out how to make those changes. Keep in mind that you might need to add a new source in their build.

### Debugging the NuGet and plugin interaction

When working with private feeds, NuGet defers the heavy lifting for authorization and authentication to a plugin.
To investigate issues related to NuGet <-> Plugin interactions, plugin logging was added.
Refer to [Plugin Diagnostic logging](plugin-logging.md) for more details.
