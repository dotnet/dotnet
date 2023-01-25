Task Packages
=============

Task packages provides a set of commonly used MSBuild tasks that are not included in MSBuild itself.

## Tasks

Each task package must be self-contained in that it cannot define dependencies on other task packages (see [below](#no-dependencies).)
Task packages should avoid grouping too many tasks into the same place.

### Reducing duplicate effort

One reason to put tasks into shared packages is to reduce duplication in infrastructure code.
We currently have multiple implementations of similar tasks in use by .NET Core and ASP.NET Core projects.
Each is maintaining their own version.

Examples:
 - https://github.com/dotnet/cli/blob/master/build/Microsoft.DotNet.Cli.tasks
 - https://github.com/aspnet/BuildTools/tree/dev/modules/KoreBuild.Tasks
 - https://github.com/dotnet/buildtools/tree/master/src/Microsoft.DotNet.Build.Tasks
 - https://github.com/dotnet/core-setup/tree/master/tools-local/tasks

Some of these tasks of nearly identical behavior but just use different names. Some examples:

 - `GenerateFileFromTemplate` / `ReplaceFileContents` / `PreprocessFile`
 - `SetEnvVar` / `SetEnvironmentVariable`
 - `UnzipArchive` / `ZipFileExtractToDirectory`

## Usage

Tasks packages are distributed as a NuGet package using existing NuGet mechanisms. Developers can use them in MSBuild projects in the following ways:

### Sdk element (recommended)

Reference the package as an "SDK" in your MSBuild project. MSBuild 15.6 and up will automatically restore and extract this package.

```xml
<Project>
    <Sdk Name="Microsoft.DotNet.Build.Tasks.IO"/>

    <Target Name="CustomStep">
        <DownloadFile Url="$(DownloadUri)" OutputPath="$(DownloadOutput)" />
    </Target>
</Packages>
```

```js
// global.json
{
   "msbuild-sdks": {
      "Microsoft.DotNet.Build.Tasks.IO": "1.0.0"
   }
}
```

**Best practice**: although SDK versions can be specified in .proj files, it is recommended to use global.json to ensure the SDK version
is consistent within a solution.

### PackageReference (pre MSBuild 15.6)

Reference the project as a PackageReference in csproj files. It is strongly recommended to set `PrivateAssets="All"` to avoid this package ending up in generated nuspec files.

```xml
<Project Sdk="Microsoft.NET.Sdk">
    <ItemGroup>
        <PackageReference id="Microsoft.DotNet.Build.Tasks.IO" Version="1.0.0" PrivateAssets="All" />
    </ItemGroup>

    <Target Name="CustomStep">
        <DownloadFile Url="$(DownloadUri)" OutputPath="$(DownloadOutput)" />
    </Target>
</Packages>
```

### packages.config (NuGet 2/MSBuild 14)

Use `NuGet.exe install packages.config` to download the package
```xml
<packages>
    <package id="Microsoft.DotNet.Build.Tasks.IO" version="1.0.0" />
</packages>
```

From your MSBuild project, import `Sdk.props` and `Sdk.targets` from the extract package location.
```xml
<Project ToolsVersion="14.0" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
    <Import Project="packages/Microsoft.DotNet.Build.Tasks.IO/1.0.0/Sdk/Sdk.props" />

    <Target Name="CustomStep">
        <DownloadFile Url="$(DownloadUri)" OutputPath="$(DownloadOutput)" />
    </Target>

    <Import Project="packages/Microsoft.DotNet.Build.Tasks.IO/1.0.0/Sdk/Sdk.targets" />
</Project>
```

## Implementation details

Task packages have this layout

```
(root)
  - sdk/
     + Sdk.props
     + Sdk.targets
  - build/
    + $packageId.props
    + $packageId.targets
    - netstandard1.5/
        + $taskAssembly.dll
    - net46/
        + $taskAssembly.dll
```

Packages have the following metadata in their nuspec.

```xml
<packageTypes>
    <packageType name="MSBuildSdk" />
</packageTypes>
```

### No dependencies

MSBuild task packages cannot have dependencies (due to the current design of the NuGet SDK resolver: https://github.com/Microsoft/msbuild/issues/2803).

```xml
<package>
  <metadata>
    <dependencies>
        <!-- Must be empty -->
    </dependencies>
  </metadata>
</package>
```

### Examples

The following are examples of tasks that we would like to build into common shared packages.
The implementation and naming is still subject to further review.
This list contains a set of tasks that appear to be commonly used across several repos.

Microsoft.DotNet.Build.Tasks.IO
 - `DownloadFile` - downloads a file.
 - `ZipArchive` - creates a .zip file
 - `UnzipArchive` - unzips a .zip file
 - `GenerateFileFromTemplate` - supports a very simple templating format for key/value substitutions in a file
 - `ComputeChecksum` - computes the SHA256 or SHA512 checksum for files
 - `Chmod` - change Unix permissions

Microsoft.DotNet.Build.Tasks.Git
 - `GetGitCommitHash` - reads the current commit hash from a .git folder without needing git.exe installed
 - `GetGitCommitBranch` - reads the current brancn name

Microsoft.DotNet.Build.Tasks.Shell
 - `Run` - like `Exec`, but it handles the complexity of escaping quotes and spaces in arguments
 - `RunDotNet` - like `Run`, but launches a process using the same `dotnet.exe` file used to launch the current MSBuild process. Espcially useful for longing .NET Core console build tools
 - `FindDotNetPath` - finds the `dotnet.exe` path on a machine
 - `SetEnvironmentVariable` - sets an environment variable

Microsoft.DotNet.Build.Tasks.NuGet
 - `PackNuspec` - packages a .nuspec file
 - `DownloadNuGetPackage` - fetches a package from a NuGet feed
 - `PushNuGetPackages` - pushes NuGet packages in parallel
 - `ReadNuGetPackageIdentity` - opens a .nupkg file and reads the package ID and version from its metadata

Microsoft.DotNet.Build.Tasks.AzureStorage
 - `UploadBlobToAzure` - pushes a blob to Azure Storage account

### Packages (by team) which should be shared to start

**ASP**
 - Tasks
    - DownloadFile
    - ZipArchive
    - UnzipArchive
    - GenerateFileFromTemplate
    - ComputeChecksum
    - Chmod

**Roslyn/CLI**
 - Repack
 - Signtool

 **CoreFx/CoreCLR**
 - BlobFeed
	- VersionTools/Dependency update
	- Repo Tools
	- ILAsm
 - ILLinker


<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CProject-Docs%5CToolset%5CTaskPackages.md)](https://helix.dot.net/f/p/5?p=Documentation%5CProject-Docs%5CToolset%5CTaskPackages.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CProject-Docs%5CToolset%5CTaskPackages.md)</sub>
<!-- End Generated Content-->
