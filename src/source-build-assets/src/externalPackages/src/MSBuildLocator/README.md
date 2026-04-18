# Microsoft.Build.Locator

## What is Locator for?

MSBuild offers a .NET API surface that allows you to [evaluate](https://docs.microsoft.com/dotnet/api/microsoft.build.evaluation) and [build](https://docs.microsoft.com/dotnet/api/microsoft.build.execution) MSBuild projects from an application. MSBuild is available as a set of NuGet packages that can provide the implementation of the MSBuild programming language to your application. But it's generally not enough to be able to load *some* MSBuild project: you want your application to be able to load the `.csproj`, `.vbproj`, `.fsproj`, `.sqlproj`, `.ccproj` and other project types that you can build in Visual Studio (or Visual Studio Build Tools) or with the .NET SDK. To do that, you need more than just MSBuild's assemblies--you must also have access to all of the SDKs and build logic that are imported into those projects.

That additional build logic is distributed with Visual Studio, with Visual Studio extensions, or as part of the .NET SDK. So to correctly load projects, you need to load them in the context of one of those MSBuild installations.

Loading MSBuild from Visual Studio also ensures that your application gets the same view of projects as `MSBuild.exe`, `dotnet build`, or Visual Studio, including bug fixes, feature additions, and performance improvements that may come from a newer MSBuild release.

## How Locator searches for .NET SDK?

MSBuild.Locator searches for the locally installed SDK based on the following priority:

1. DOTNET_ROOT
2. Current process path if MSBuild.Locator is called from dotnet.exe
3. DOTNET_HOST_PATH
4. DOTNET_MSBUILD_SDK_RESOLVER_CLI_DIR
5. PATH

Note that probing stops when the first dotnet executable is found among the listed variables.

Documentation describing the definition of these variables can be found here: [.NET Environment Variables](https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-environment-variables).

## Documentation

Documentation is located on the official Microsoft documentation site: [Use Microsoft.Build.Locator](https://docs.microsoft.com/visualstudio/msbuild/updating-an-existing-application#use-microsoftbuildlocator).

## Build status

CI status: ![Build and test](https://github.com/microsoft/MSBuildLocator/workflows/Build%20and%20test/badge.svg)

Official build: [![Build Status](https://dev.azure.com/dnceng/public/_apis/build/status/Microsoft/MSBuildLocator/MSBuildLocator-ci)](https://dev.azure.com/dnceng/public/_build/latest?definitionId=80)

## Contributing

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/). For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.
