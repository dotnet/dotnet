This directory contains code that is copied from https://github.com/NuGet/NuGet.Client/tree/dev/src/NuGet.Core/NuGet.Frameworks, with the namespaces changed
and class visibility changed. This is done to ensure we are providing the same functionality as Nuget.Frameworks, without depending on the package explicitly.

The files in this folder are coming from tag 6.8.0.117, on commit 7fb5ed8.

To update this code, run the script in: \scripts\update-nuget-frameworks.ps1 , with -VersionTag <theDesiredVersion>.

Exception to this is the Strings.cs, this is coming from Strings.Designer.cs because we don't localize those messages here. These messages
go only into logs.

