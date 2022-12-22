# Microsoft.DotNet.CMake.Sdk

MSBuild SDK wrapper for invoking CMake and copying native artifacts for specific directories out of the CMake build tree.

This package enables easy Arcade integration of native product and test builds.

## Using the Microsoft.DotNet.CMake.Sdk

### Features

- Thin MSBuild abstraction over the CMake command.
- Support for custom compiler location.
- Automatic resolution of VS native tools for target architecture for both Visual Studio and other generators without building from within a Developer Command Prompt.
- Simple copying of subsets of native assets to the output folder of a managed project.
- Support to output the generated configure and build script for usage in bringup scenarios.

### Common Items

Here are a list of common MSBuild items that this SDK consumes:

- CMakeDefines
  - Any defines passed directly to CMake via `-D`.
- CMakeArguments
  - Any additional arguments to pass directly to CMake that are not supported via other properties or items.
- CMakeNativeToolArguments
  - Any arguments to pass to the native build tool invoked via `cmake --build`.
  
### Common Properties

Here are a list of MSBuild properties that this SDK consumes:

- CMakeLists
  - The root CMakeLists.txt of the native build. This is required.
- CMakeGenerator
  - The CMake generator to use. To use a Visual Studio based generator, you can specify "Visual Studio" and this SDK will resolve a VS that supports the target platform.
- CMakeCompilerToolchain
  - The compiler to use for the build. Defaults to MSVC on Windows and Clang on non-Windows.
- CMakeCompilerMajorVersion (optional)
  - The major version of the compiler to use. Ignored on the MSVC toolchain.
- CMakeCompilerMinorVersion (optional)
  - The minor version of the compiler to use. Ignored on the MSVC toolchain.
- Configuration
  - The configuration for which to generate and build the CMake project.
- Platform
  - The target architecture of the native project.
- CMakeOutputDir
  - The output directory for the CMake generated build files. Defaults to the IntermediateOutputPath.
- CMakeInstallDir
  - The output directory for files "installed" by CMake. Defaults to OutputPath.
- CMakeToolset
  - Toolset to pass to cmake via the `-T` parameter.
- CMakeConfigureCommandWrapper
  - Command line to wrap the cmake command, such as `scan-build` or `emcmake`.
- CMakeBuildTarget
  - The target to build. Defaults to `install`.
- CMakeParallelization
  - Amount of parallelization to tell `cmake --build` to use.
- CMakeCompilerSearchScript
  - Script to run before configuration or building to locate compilers or libraries for CMake or the native build tool.
- CMakeConfigureDependsOn
  - Targets that must be run before the GetConfigScript target is run.
- CMakeBuildDependsOn
  - Targets that must be run before the GetBuildScript target is run.

### Simple Example Project

To get started, add `Microsoft.DotNet.CMake.Sdk` under `msbuild-sdks` in `global.json`:
```json
{
  ...
  "msbuild-sdks": {
    ...
    "Microsoft.DotNet.CMake.Sdk": "version",
    ...
  },
  ...
}
```

To use this SDK, you need to define a native project and then reference it from your managed code project using NativeProjectReference.

#### Defining Native Project
If you want to drive a native CMake build with this SDK, add a .proj file next to your CMakeLists.txt with the following content:

```xml
<Project Sdk="Microsoft.DotNet.CMake.Sdk">
  <PropertyGroup>
    <CMakeLists>CMakeLists.txt</CMakeLists>
  </PropertyGroup>
</Project>
```

Building this project will configure and build the CMakeLists.txt project with the default generator provided by the SDK (Visual Studio on Windows, Unix Makefiles off-Windows).

#### Referencing Native assets with NativeProjectReference

An important feature of the Microsoft.DotNet.CMake.Sdk is that it enables the user to reference their native assets from a managed project. This is the most valuable in a testing scenario such as the CoreCLR test tree since it does not currently do any special handling of multiple architectures.

To add a native project reference from a managed project, add the following line to your `.csproj` file to include the targets:

```xml
<Import Project="ProjectReference.targets" Sdk="Microsoft.DotNet.CMake.Sdk" />
```

To add a native project reference to a given CMakeLists.txt from a managed project, add a `NativeProjectReference` item to the managed project as shown below:

```xml
<ItemGroup>
    <NativeProjectReference Include="path/to/my/CMakeLists.txt" CMakeProject="path/to/CMake/Project/UsingThisSdk.proj" />
</ItemGroup>
```

The project in the `CMakeProject` metadata is the project using this SDK that drives the CMake build of the provided CMakeLists.txt.

If you have many native project references and don't want to specify the `CMakeProject` metadata on all of them, you can also set the `DefaultCMakeProject` property to point to the project.

All assets that are a direct output of this CMakeLists.txt will be copied to your output folder. This includes all library and executable targets defined in this CMakeLists.txt. This CMakeLists.txt does not have to be the root of your CMake tree, but it must be transitively included by the CMakeLists.txt specified in the referenced `CMakeProject` project.

By default, a NativeProjectReference will not build the native project. It assumes that the project has already been built. To build the project as part of the reference, you can opt-in by setting the `BuildNative="true"` metadata on the `NativeProjectReference`.

### Generating a raw build script for bringup scenarios

This SDK also supports outputting the script that the SDK runs to configure and build your CMake project. This feature can be used to generate a simple script for use in bringup scenarios where we don't have MSBuild available for the device we are building on. This would enable teams to generate bringup build scripts when needed instead of using bringup-style scripts at all times or having unused bringup scripts that quickly bitrot.

To generate the bringup script, call the `GenerateBringupScript` target and pass in the `BringupScriptOutputFile` path. The target will output a rudimentary script that runs the compiler location script, the CMake configure command, and the CMake build command. This script can then be hand-edited as needed for the custom bringup scenario until enough of the stack is online to switch back to using MSBuild and reintegrate any of the changes.
