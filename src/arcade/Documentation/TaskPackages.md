Task Packages
=============

Task packages provides a set of commonly used MSBuild tasks that are not included in MSBuild itself.
Each task package must be self-contained in that it cannot define dependencies on other task packages (see [below](#no-dependencies).)
Task packages should avoid grouping too many tasks into the same place.

## Usage

Tasks packages are distributed as a NuGet package using existing NuGet mechanisms.
Developers can use them in MSBuild projects in the following ways:

### Sdk element (recommended)

Requires: MSBuild 15.6+

Reference the package as an "SDK" in your MSBuild project. MSBuild will automatically restore and extract this package.

```xml
<Project>
    <Sdk Name="Microsoft.DotNet.Build.Tasks.Banana"/>

    <Target Name="CustomStep">
        <Peel Color="yellow" />
    </Target>
</Packages>
```

```js
// global.json
{
   "msbuild-sdks": {
      "Microsoft.DotNet.Build.Tasks.Banana": "1.0.0"
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
        <PackageReference id="Microsoft.DotNet.Build.Tasks.Banana" Version="1.0.0" PrivateAssets="All" />
    </ItemGroup>

    <Target Name="CustomStep">
        <Peel Color="yellow" />
    </Target>
</Packages>
```

### packages.config (NuGet 2/MSBuild 14)

Use `NuGet.exe install packages.config` to download the package
```xml
<packages>
    <package id="Microsoft.DotNet.Build.Tasks.Banana" version="1.0.0" />
</packages>
```

From your MSBuild project, import `Sdk.props` and `Sdk.targets` from the extract package location.
```xml
<Project ToolsVersion="14.0" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
    <Import Project="packages/Microsoft.DotNet.Build.Tasks.Banana/1.0.0/Sdk/Sdk.props" />

    <Target Name="CustomStep">
        <Peel Color="yellow" />
    </Target>

    <Import Project="packages/Microsoft.DotNet.Build.Tasks.Banana/1.0.0/Sdk/Sdk.targets" />
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


<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CTaskPackages.md)](https://helix.dot.net/f/p/5?p=Documentation%5CTaskPackages.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CTaskPackages.md)</sub>
<!-- End Generated Content-->
