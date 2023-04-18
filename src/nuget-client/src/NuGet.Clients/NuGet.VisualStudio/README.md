# NuGet.VisualStudio

This package provides APIs for invoking NuGet services in Visual Studio. It contains NuGet’s older services that are available via the [Managed Extensibility Framework (MEF)](https://learn.microsoft.com/dotnet/framework/mef/).

## Usage

After installing the package, you can use its services to interact with NuGet in Visual Studio. This can be used to install and uninstall packages, and to obtain information about installed packages.

### MEF Services
From NuGet 6.0, all of these APIs are available in the package [NuGet.VisualStudio](https://www.nuget.org/packages/NuGet.VisualStudio/). In NuGet 5.11 and earlier, the APIs in the namespace [`NuGet.VisualStudio`](https://learn.microsoft.com/dotnet/api/microsoft.visualstudio.componentmodelhost.icomponentmodel?view=visualstudiosdk-2022) are available in the package [NuGet.VisualStudio](https://www.nuget.org/packages/NuGet.VisualStudio/), and APIs in the namespace [`NuGet.SolutionRestoreManager`](https://learn.microsoft.com/dotnet/api/microsoft.visualstudio.componentmodelhost.icomponentmodel?view=visualstudiosdk-2022) are available in the package [NuGet.SolutionRestoreManager.Interop](https://www.nuget.org/packages/NuGet.SolutionRestoreManager.Interop/).

## Example

### Get installed packages

To use a service, import it through the [MEF Import attribute](https://learn.microsoft.com/dotnet/framework/mef/#imports-and-exports-with-attributes), or through the [IComponentModel service](https://learn.microsoft.com/dotnet/api/microsoft.visualstudio.componentmodelhost.icomponentmodel?view=visualstudiosdk-2022).

```c#
// Using the Import attribute
[Import(typeof(IVsPackageInstaller2))]
public IVsPackageInstaller2 packageInstaller;
packageInstaller.InstallLatestPackage(source: null, currentProject,
    "Newtonsoft.Json", includePrerelease: false, ignoreDependencies: false);

// Using the IComponentModel service
var componentModel = (IComponentModel)GetService(typeof(SComponentModel));
IVsPackageUninstaller uninstallerServices =
    componentModel.GetService<IVsPackageUninstaller>();

var uninstallerServices = installerServices.UninstallPackage(currentProject, "Newtonsoft.json", removeDependencies: true);
```

## Additional documentation

More information about the NuGet.VisualStudio library can be found on the [NuGet API docs](https://learn.microsoft.com/nuget/visual-studio-extensibility/nuget-api-in-visual-studio).