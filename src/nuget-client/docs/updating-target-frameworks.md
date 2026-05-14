# Updating target frameworks

Updating the target frameworks in our repository can be challenging.
Our repository builds code for Visual Studio, .NET SDK, NuGet.exe in addition to [shipping packages to NuGet.org](https://www.nuget.org/profiles/nuget).

Given that so many of our projects have different frameworks, these frameworks are defined in [common.projects.props](../build/common.project.props). 
Note that `DotNetBuildSourceOnly` tends to build against different frameworks than the ones our repo builds against.

To make it easier to determine the correct changes were made, you can use utility targets such as `GetAllTargetFrameworks` in [build.proj](../build/build.proj).
It is important to pay attention to that changing frameworks.
The recommended way to invoke these helper commands is by running the following from the root of the repo.

> msbuild .\build\build.proj /t:GetAllTargetFrameworks /v:m /restore:false

or

> msbuild .\build\build.proj /t:GetAllTargetFrameworks /v:m /restore:false /p:DotNetBuildSourceOnly="true"

This would generate an output that you can diff for before/after list of the frameworks.
