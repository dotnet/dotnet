# Microsoft.DotNet.SourceBuild.Tasks

This task package contains tasks required to run source-build, but have
dependencies that can't be added to `Microsoft.DotNet.Arcade.Sdk`, such as NuGet
libraries. See [#6014](https://github.com/dotnet/arcade/issues/6014) for more
details on dependencies.

The package is restored and used by the Arcade SDK when necessary to perform
specialized source-build tasks.
