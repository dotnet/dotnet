# Debugging with experimental Roslyn bits

Sometimes it may be necessary to make changes in [`dotnet/roslyn`](https://github.com/dotnet/roslyn), and react to the changes in this repo. The following are steps which outline the general process in using Roslyn development `dll`s and binaries with Razor Tooling.

## Steps

1. Checkout [`dotnet/roslyn`](https://github.com/dotnet/roslyn).
2. `./Restore.cmd`
3. Make the desired changes in `dotnet/roslyn`.
4. `./Build.cmd -pack`. The `-pack` option causes the creation of NuGet packages.
5. You should see the generated packages in the `<PATH_TO_ROSLYN_REPO>\artifacts\packages\Debug\Debug` directory. Take note of the package versions (ie. `Microsoft.CodeAnalysis.Workspaces.Common.3.8.0-dev.nupkg` => `3.8.0-dev`).
6. Open `NuGet.config` and add the local package source `<add key="Roslyn Local Package source" value="<PATH_TO_ROSLYN_REPO>\artifacts\packages\Debug\Debug" />` and package source below under the `packageSourceMapping` tag:

```xml
    <packageSource key="Roslyn Local Package source">
      <package pattern="microsoft.*" />
      <package pattern="microsoft.commonlanguageserverProtocol.*" />
    </packageSource>
```

7. Open `eng/Versions.props` and update `RoslynPackageVersion` to the version noted in step 5.
8. To get the end-to-end local debugging working, running `./Build.cmd -deploy` script from roslyn repository. this will copy over the right binaries from roslyn to the shared local roslyn/razor hive.

## Troubleshooting

Use the steps below to do a clean build if the dlls/binaries need to get cleaned out:

- Shut down all instances of VS.
- Run the following scripts to kill processes running in a bad state:
```
> TASKKILL /IM devenv.exe /F
> TASKKILL /IM dotnet.exe /F
> TASKKILL /IM MSBuild.exe /F
```
- Delete the hive folder by navigating to `%LocalAppData%\Microsoft\VisualStudio` and deleting the 17.0_xxxxxxxxRoslynDev folder
- Delete the artifacts folder under the root folder of razor repository
- Launch VS with the Razor solution
- Make sure `Microsoft.VisualStudio.RazorExtension` is set as the start up project.
- Build and Rebuild Solution from the menu or run script `.\build.cmd -deploy`
- Check to make sure the hive folder is there
- F5 (or CTRL+F5) the razor solution.

## Notes

- If you're familiar with _Visual Studio Hives_ the `dotnet/roslyn` project uses the `RoslynDev` root suffix .
- [Building Roslyn on Windows](https://github.com/dotnet/roslyn/blob/main/docs/contributing/Building,%20Debugging,%20and%20Testing%20on%20Windows.md)
- [Building Roslyn on Linux and Mac](https://github.com/dotnet/roslyn/blob/main/docs/infrastructure/cross-platform.md)
- If you find the old packages are still being used after this change, purge the nuget cache here: `~\.nuget\packages`
