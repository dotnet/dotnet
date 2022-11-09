# Arcade Conventions

These are the conventions used in the Arcade repository.

### File Naming

Filenames should be all lowercase, for instance: `build.cmd` or `nuget.config`. Only exceptions are for files that needs to be cased a particular way for an existing set of tools to read them (Example: `Directory.Build.props` which MSBuild expects exactly that case on Linux).

Filenames with multiple words should use kebab-casing like `init-tools.sh`.

MSBuild based targets and props files for a particular library should match the exact casing of the library package they belong to, like `Microsoft.DotNet.Build.Tasks.Feed.targets`.

### Dependent Packages Version

Package versions are stored in MSBuild properties in the [`eng\Versions.props`](https://github.com/dotnet/arcade/blob/master/eng/Versions.props) file. Use these properties to include the correct version of the package. New properties will be included as needed. 

If your project depend on a package which is also part of the .NET SDK used by Arcade (check `global.json` to see which version is currently in use) the project should use the version of the package available in the SDK. Otherwise, the latest stable version of the package should be used. For instance, the `Newtonsoft.Json` (version 9.0.1) is present on the .NET SDK and the version is exposed in Arcade through the `$(NewtonsoftJsonVersion)` property in [`eng\Versions.props`](https://github.com/dotnet/arcade/blob/master/eng/Versions.props). Therefore, to include Newtonsoft.Json in your project do the following:

`<PackageReference Include="Newtonsoft.Json" Version="$(NewtonsoftJsonVersion)" />`












<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CArcadeConventions.md)](https://helix.dot.net/f/p/5?p=Documentation%5CArcadeConventions.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CArcadeConventions.md)</sub>
<!-- End Generated Content-->
