# dotnet-mage

dotnet-mage (previously also known as Mage.NET) is the open-sourced version of the familiar .NET FX tool Mage.

You can find the latest version of [dotnet-mage on Nuget.org](https://www.nuget.org/packages/Microsoft.DotNet.Mage).

dotnet-mage supports all existing command-line options of the old Mage tool, with few exceptions:
- no support for partial trust
- no support for sha1 hashing
- no support for ia64 architecture

For the full list of Mage command line options please visit https://docs.microsoft.com/en-us/dotnet/framework/tools/mage-exe-manifest-generation-and-editing-tool

There is one new option, to add launcher. Here's the short documentation for this option:

Adds Launcher to target directory and sets binary to be launched.

`-AddLauncher <binary_to_launch>` or short `-al`

Example:
`-AddLauncher myapp.dll -TargetDirectory bin/release`

Launcher is required for all modern .NET (including .NET 5+, .NET Core 3.1) apps in ClickOnce.

You can obtain all command-line options by running `dotnet mage` or for verbose help `dotnet mage -help verbose`.

## Prerequisites for using this tool

* [Install .NET SDK](https://dotnet.microsoft.com/download)

* Install dotnet-mage global tool:

`dotnet tool install --global microsoft.dotnet.mage`

dotnet-mage is fully supported on Windows (we are gathering feedback about scenarios for dotnet-mage usage on Linux build agents).

## Common usage scenario

* Build the project and copy the produced project output (binaries, json files, etc.) to a new folder
* Add launcher
* Create application manifest
* Create deployment manifest

## Example steps

Suppose that we have copied project output to a sub-folder `files` and our .NET application entry point is `myapp.exe`

* Add Launcher

`dotnet mage -al myapp.exe -td files`

* Create application manifest

`dotnet mage -new Application -t files\MyApp.manifest -fd files -v 1.0.0.1`

* Create deployment manifest

`dotnet mage -new Deployment -Install true -pub "My Publisher" -v 1.0.0.1 -AppManifest files\MyApp.manifest -t MyApp.application`

### Update an existing application

* Update application manifest

`dotnet mage -update files\MyApp.manifest -v 1.0.0.2`

* Update deployment manifest

`dotnet mage -update MyApp.Application -v 1.0.0.2 -AppManifest files\MyApp.manifest`

