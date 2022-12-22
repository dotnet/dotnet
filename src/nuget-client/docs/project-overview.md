---
date-generated: 2022-11-22T14:33:34
tool: NuGetTasks.GenerateMarkdownDoc
---


# NuGet Project Overview

Below is a list of projects contained in the NuGet.Client repo, organized by product and test projects.

## Product Projects

All shipped NuGet libraries and clients in `src/` folder.

Projects in section: 37

### src\NuGet.Clients

- [`NuGet.CommandLine.csproj`](../src/NuGet.Clients/NuGet.CommandLine/NuGet.CommandLine.csproj): NuGet Command Line Interface.
- [`NuGet.Console.csproj`](../src/NuGet.Clients/NuGet.Console/NuGet.Console.csproj): Package Manager PowerShell Console implementation.
- [`NuGet.Indexing.csproj`](../src/NuGet.Clients/NuGet.Indexing/NuGet.Indexing.csproj): NuGet's indexing library for the Visual Studio client search functionality.
- [`NuGet.MSSigning.Extensions.csproj`](../src/NuGet.Clients/NuGet.MSSigning.Extensions/NuGet.MSSigning.Extensions.csproj): NuGet Command Line Interface for repository signing.
- [`NuGet.PackageManagement.PowerShellCmdlets.csproj`](../src/NuGet.Clients/NuGet.PackageManagement.PowerShellCmdlets/NuGet.PackageManagement.PowerShellCmdlets.csproj): Package Manager Console PowerShell host implementation and NuGet's PowerShell cmdlets for the Visual Studio client.
- [`NuGet.PackageManagement.UI.csproj`](../src/NuGet.Clients/NuGet.PackageManagement.UI/NuGet.PackageManagement.UI.csproj): Package Management UI elements for Visual Studio, Package Manager UI, Migrator, Options dialog.
- [`NuGet.PackageManagement.VisualStudio.csproj`](../src/NuGet.Clients/NuGet.PackageManagement.VisualStudio/NuGet.PackageManagement.VisualStudio.csproj): NuGet's Visual Studio functionalities, integrations and utilities.
- [`NuGet.SolutionRestoreManager.csproj`](../src/NuGet.Clients/NuGet.SolutionRestoreManager/NuGet.SolutionRestoreManager.csproj): NuGet's Visual Studio Solution Restore Manager.
- [`NuGet.Tools.csproj`](../src/NuGet.Clients/NuGet.Tools/NuGet.Tools.csproj): NuGet's Visual Studio extension Package.
- [`NuGet.VisualStudio.Client.csproj`](../src/NuGet.Clients/NuGet.VisualStudio.Client/NuGet.VisualStudio.Client.csproj): NuGet Visual Studio extension package project.
- [`NuGet.VisualStudio.Common.csproj`](../src/NuGet.Clients/NuGet.VisualStudio.Common/NuGet.VisualStudio.Common.csproj): NuGet's Visual Studio common types and interfaces used for both Package Manager UI, Package Manager Console, restore and install functionalities.
- [`NuGet.VisualStudio.Contracts.csproj`](../src/NuGet.Clients/NuGet.VisualStudio.Contracts/NuGet.VisualStudio.Contracts.csproj): RPC contracts for NuGet's Visual Studio Service Broker extensibility APIs.
- [`NuGet.VisualStudio.Implementation.csproj`](../src/NuGet.Clients/NuGet.VisualStudio.Implementation/NuGet.VisualStudio.Implementation.csproj): Implementation of the NuGet.VisualStudio extensibility APIs.
- [`NuGet.VisualStudio.Internal.Contracts.csproj`](../src/NuGet.Clients/NuGet.VisualStudio.Internal.Contracts/NuGet.VisualStudio.Internal.Contracts.csproj): 
- [`NuGet.VisualStudio.Interop.csproj`](../src/NuGet.Clients/NuGet.VisualStudio.Interop/NuGet.VisualStudio.Interop.csproj): NuGet's Visual Studio client Template Wizard interop implementation.
- [`NuGet.VisualStudio.OnlineEnvironment.Client.csproj`](../src/NuGet.Clients/NuGet.VisualStudio.OnlineEnvironment.Client/NuGet.VisualStudio.OnlineEnvironment.Client.csproj): 
- [`NuGet.VisualStudio.csproj`](../src/NuGet.Clients/NuGet.VisualStudio/NuGet.VisualStudio.csproj): APIs for invoking NuGet services in Visual Studio.

### src\NuGet.Core

- [`Microsoft.Build.NuGetSdkResolver.csproj`](../src/NuGet.Core/Microsoft.Build.NuGetSdkResolver/Microsoft.Build.NuGetSdkResolver.csproj): MSBuild SDK resolver for NuGet packages.
- [`NuGet.Build.Tasks.Console.csproj`](../src/NuGet.Core/NuGet.Build.Tasks.Console/NuGet.Build.Tasks.Console.csproj): NuGet Build tasks for MSBuild and dotnet restore. Contains restore logic using the MSBuild static graph functionality.
- [`NuGet.Build.Tasks.Pack.csproj`](../src/NuGet.Core/NuGet.Build.Tasks.Pack/NuGet.Build.Tasks.Pack.csproj): NuGet tasks for MSBuild and dotnet pack.
- [`NuGet.Build.Tasks.csproj`](../src/NuGet.Core/NuGet.Build.Tasks/NuGet.Build.Tasks.csproj): NuGet tasks for MSBuild and dotnet restore.
- [`NuGet.CommandLine.XPlat.csproj`](../src/NuGet.Core/NuGet.CommandLine.XPlat/NuGet.CommandLine.XPlat.csproj): NuGet executable wrapper for the dotnet CLI nuget functionality.
- [`NuGet.Commands.csproj`](../src/NuGet.Core/NuGet.Commands/NuGet.Commands.csproj): Complete commands common to command-line and GUI NuGet clients.
- [`NuGet.Common.csproj`](../src/NuGet.Core/NuGet.Common/NuGet.Common.csproj): Common utilities and interfaces for all NuGet libraries.
- [`NuGet.Configuration.csproj`](../src/NuGet.Core/NuGet.Configuration/NuGet.Configuration.csproj): NuGet's configuration settings implementation.
- [`NuGet.Credentials.csproj`](../src/NuGet.Core/NuGet.Credentials/NuGet.Credentials.csproj): NuGet client's authentication models.
- [`NuGet.DependencyResolver.Core.csproj`](../src/NuGet.Core/NuGet.DependencyResolver.Core/NuGet.DependencyResolver.Core.csproj): NuGet's PackageReference dependency resolver implementation.
- [`NuGet.Frameworks.csproj`](../src/NuGet.Core/NuGet.Frameworks/NuGet.Frameworks.csproj): NuGet's understanding of target frameworks.
- [`NuGet.LibraryModel.csproj`](../src/NuGet.Core/NuGet.LibraryModel/NuGet.LibraryModel.csproj): NuGet's types and interfaces for understanding dependencies.
- [`NuGet.Localization.csproj`](../src/NuGet.Core/NuGet.Localization/NuGet.Localization.csproj): NuGet localization package for dotnet CLI.
- [`NuGet.PackageManagement.csproj`](../src/NuGet.Core/NuGet.PackageManagement/NuGet.PackageManagement.csproj): NuGet Package Management functionality for Visual Studio installation flow.
- [`NuGet.Packaging.Core.csproj`](../src/NuGet.Core/NuGet.Packaging.Core/NuGet.Packaging.Core.csproj): The (former home to) core data structures for NuGet.Packaging. Contains only the type forwarders to the new assembly.
- [`NuGet.Packaging.csproj`](../src/NuGet.Core/NuGet.Packaging/NuGet.Packaging.csproj): NuGet's understanding of packages. Reading nuspec, nupkgs and package signing.
- [`NuGet.ProjectModel.csproj`](../src/NuGet.Core/NuGet.ProjectModel/NuGet.ProjectModel.csproj): NuGet's core types and interfaces for PackageReference-based restore, such as lock files, assets file and internal restore models.
- [`NuGet.Protocol.csproj`](../src/NuGet.Core/NuGet.Protocol/NuGet.Protocol.csproj): NuGet's implementation for interacting with feeds. Contains functionality for all feed types.
- [`NuGet.Resolver.csproj`](../src/NuGet.Core/NuGet.Resolver/NuGet.Resolver.csproj): NuGet's dependency resolver for packages.config based projects.
- [`NuGet.Versioning.csproj`](../src/NuGet.Core/NuGet.Versioning/NuGet.Versioning.csproj): NuGet's implementation of Semantic Versioning.


## Core Unit Test Projects

Unit tests for libraries and some clients. Located in `test/` folder.

Projects in section: 18

### test\NuGet.Core.Tests

- [`Microsoft.Build.NuGetSdkResolver.Test.csproj`](../test/NuGet.Core.Tests/Microsoft.Build.NuGetSdkResolver.Test/Microsoft.Build.NuGetSdkResolver.Test.csproj): Unit tests for Microsoft.Build.NuGetSdkResolver.
- [`NuGet.Build.Tasks.Console.Test.csproj`](../test/NuGet.Core.Tests/NuGet.Build.Tasks.Console.Test/NuGet.Build.Tasks.Console.Test.csproj): Unit tests for NuGet.Build.Tasks.Console.
- [`NuGet.Build.Tasks.Pack.Test.csproj`](../test/NuGet.Core.Tests/NuGet.Build.Tasks.Pack.Test/NuGet.Build.Tasks.Pack.Test.csproj): Unit tests for NuGet.Build.Tasks.Pack.
- [`NuGet.Build.Tasks.Test.csproj`](../test/NuGet.Core.Tests/NuGet.Build.Tasks.Test/NuGet.Build.Tasks.Test.csproj): Unit tests for NuGet.Build.Tasks.
- [`NuGet.CommandLine.Xplat.Tests.csproj`](../test/NuGet.Core.Tests/NuGet.CommandLine.Xplat.Tests/NuGet.CommandLine.Xplat.Tests.csproj): Unit tests for NuGet.CommandLine.XPlat.
- [`NuGet.Commands.Test.csproj`](../test/NuGet.Core.Tests/NuGet.Commands.Test/NuGet.Commands.Test.csproj): Unit tests for NuGet.Commands.
- [`NuGet.Common.Test.csproj`](../test/NuGet.Core.Tests/NuGet.Common.Test/NuGet.Common.Test.csproj): Unit tests for NuGet.Common.
- [`NuGet.Configuration.Test.csproj`](../test/NuGet.Core.Tests/NuGet.Configuration.Test/NuGet.Configuration.Test.csproj): Unit tests for NuGet.Configuration.
- [`NuGet.Credentials.Test.csproj`](../test/NuGet.Core.Tests/NuGet.Credentials.Test/NuGet.Credentials.Test.csproj): Unit tests for NuGet.Credentials.
- [`NuGet.DependencyResolver.Core.Tests.csproj`](../test/NuGet.Core.Tests/NuGet.DependencyResolver.Core.Tests/NuGet.DependencyResolver.Core.Tests.csproj): Unit tests for NuGet.DependencyResolver.Core.
- [`NuGet.Frameworks.Test.csproj`](../test/NuGet.Core.Tests/NuGet.Frameworks.Test/NuGet.Frameworks.Test.csproj): Unit tests for NuGet.Frameworks.
- [`NuGet.LibraryModel.Tests.csproj`](../test/NuGet.Core.Tests/NuGet.LibraryModel.Tests/NuGet.LibraryModel.Tests.csproj): Unit tests for NuGet.LibraryModel.
- [`NuGet.Packaging.Test.csproj`](../test/NuGet.Core.Tests/NuGet.Packaging.Test/NuGet.Packaging.Test.csproj): Unit tests for NuGet.Packaging.
- [`NuGet.ProjectModel.Test.csproj`](../test/NuGet.Core.Tests/NuGet.ProjectModel.Test/NuGet.ProjectModel.Test.csproj): Unit tests for NuGet.ProjectModel.
- [`NuGet.Protocol.Tests.csproj`](../test/NuGet.Core.Tests/NuGet.Protocol.Tests/NuGet.Protocol.Tests.csproj): Unit tests for NuGet.Protocol.
- [`NuGet.Resolver.Test.csproj`](../test/NuGet.Core.Tests/NuGet.Resolver.Test/NuGet.Resolver.Test.csproj): Unit tests for NuGet.Resolver.
- [`NuGet.Shared.Tests.csproj`](../test/NuGet.Core.Tests/NuGet.Shared.Tests/NuGet.Shared.Tests.csproj): Unit tests for the utilities included using shared compilation.
- [`NuGet.Versioning.Test.csproj`](../test/NuGet.Core.Tests/NuGet.Versioning.Test/NuGet.Versioning.Test.csproj): Unit tests for NuGet.Versioning.


## Visual Studio Test Projects

Projects in section: 11

### test\NuGet.Clients.Tests

- [`NuGet.Indexing.Test.csproj`](../test/NuGet.Clients.Tests/NuGet.Indexing.Test/NuGet.Indexing.Test.csproj): Unit tests for NuGet.Indexing.
- [`NuGet.MSSigning.Extensions.Test.csproj`](../test/NuGet.Clients.Tests/NuGet.MSSigning.Extensions.Test/NuGet.MSSigning.Extensions.Test.csproj): An end-to-end test suite for NuGet.MSSigning.Extensions. Overlaps in tests with NuGet.MSSigning.Extensions.FuncTest.
- [`NuGet.PackageManagement.UI.Test.csproj`](../test/NuGet.Clients.Tests/NuGet.PackageManagement.UI.Test/NuGet.PackageManagement.UI.Test.csproj): Unit and integration tests for NuGet.PackageManagement.UI.
- [`NuGet.PackageManagement.VisualStudio.Test.csproj`](../test/NuGet.Clients.Tests/NuGet.PackageManagement.VisualStudio.Test/NuGet.PackageManagement.VisualStudio.Test.csproj): Unit and integration tests for NuGet.PackageManagement.VisualStudio.
- [`NuGet.SolutionRestoreManager.Test.csproj`](../test/NuGet.Clients.Tests/NuGet.SolutionRestoreManager.Test/NuGet.SolutionRestoreManager.Test.csproj): Unit and integration tests for NuGet.SolutionRestoreManager.
- [`NuGet.Tools.Test.csproj`](../test/NuGet.Clients.Tests/NuGet.Tools.Test/NuGet.Tools.Test.csproj): Unit tests for NuGet.Tools.
- [`NuGet.VisualStudio.Common.Test.csproj`](../test/NuGet.Clients.Tests/NuGet.VisualStudio.Common.Test/NuGet.VisualStudio.Common.Test.csproj): Unit and integration tests for NuGet.VisualStudio.Common.
- [`NuGet.VisualStudio.Implementation.Test.csproj`](../test/NuGet.Clients.Tests/NuGet.VisualStudio.Implementation.Test/NuGet.VisualStudio.Implementation.Test.csproj): Unit and integration tests for NuGet.VisualStudio.Implementation.
- [`NuGet.VisualStudio.Internal.Contracts.Test.csproj`](../test/NuGet.Clients.Tests/NuGet.VisualStudio.Internal.Contracts.Test/NuGet.VisualStudio.Internal.Contracts.Test.csproj): Unit and integration tests for NuGet.VisualStudio.Internal.Contracts.
- [`NuGet.VisualStudio.Test.csproj`](../test/NuGet.Clients.Tests/NuGet.VisualStudio.Test/NuGet.VisualStudio.Test.csproj): Unit and integration tests for NuGet.VisualStudio.
- [`NuGetConsole.Host.PowerShell.Test.csproj`](../test/NuGet.Clients.Tests/NuGetConsole.Host.PowerShell.Test/NuGetConsole.Host.PowerShell.Test.csproj): Unit and integration tests for NuGet.PackageManagement.PowerShellCmdlets.


## Functional Test Projects

Projects in section: 10

### test\NuGet.Clients.FuncTests

- [`NuGet.CommandLine.FuncTest.csproj`](../test/NuGet.Clients.FuncTests/NuGet.CommandLine.FuncTest/NuGet.CommandLine.FuncTest.csproj): A functional (end-to-end) test suite for NuGet.CommandLine. Contains tests for every nuget.exe command.
- [`NuGet.MSSigning.Extensions.FuncTest.csproj`](../test/NuGet.Clients.FuncTests/NuGet.MSSigning.Extensions.FuncTest/NuGet.MSSigning.Extensions.FuncTest.csproj): A functional (end-to-end) test suite for NuGet.MSSigning.Extensions.

### test\NuGet.Clients.Tests

- [`NuGet.CommandLine.Test.csproj`](../test/NuGet.Clients.Tests/NuGet.CommandLine.Test/NuGet.CommandLine.Test.csproj): An end-to-end test suite for NuGet.CommandLine. Contains tests for every nuget.exe CLI command. Overlaps in tests with NuGet.CommandLine.FuncTest.

### test\NuGet.Core.FuncTests

- [`Dotnet.Integration.Test.csproj`](../test/NuGet.Core.FuncTests/Dotnet.Integration.Test/Dotnet.Integration.Test.csproj): Integration tests for NuGet-powered dotnet CLI commands such as pack/restore/list package and dotnet nuget.
- [`Msbuild.Integration.Test.csproj`](../test/NuGet.Core.FuncTests/Msbuild.Integration.Test/Msbuild.Integration.Test.csproj): Integration tests for NuGet powered msbuild functionalities (restore/pack).
- [`NuGet.Commands.FuncTest.csproj`](../test/NuGet.Core.FuncTests/NuGet.Commands.FuncTest/NuGet.Commands.FuncTest.csproj): Integration tests for the more involved NuGet.Commands, such as restore.
- [`NuGet.Packaging.FuncTest.csproj`](../test/NuGet.Core.FuncTests/NuGet.Packaging.FuncTest/NuGet.Packaging.FuncTest.csproj): Integration tests for the more involved NuGet.Packaging functionality, such as signing.
- [`NuGet.Protocol.FuncTest.csproj`](../test/NuGet.Core.FuncTests/NuGet.Protocol.FuncTest/NuGet.Protocol.FuncTest.csproj): Integration tests for the more involved NuGet.Protocol functionality, such as plugins.
- [`NuGet.XPlat.FuncTest.csproj`](../test/NuGet.Core.FuncTests/NuGet.XPlat.FuncTest/NuGet.XPlat.FuncTest.csproj): Functional tests for nuget in dotnet CLI scenarios, using the NuGet.CommandLine.XPlat assembly.

### test\NuGet.Core.Tests

- [`NuGet.PackageManagement.Test.csproj`](../test/NuGet.Core.Tests/NuGet.PackageManagement.Test/NuGet.PackageManagement.Test.csproj): Unit tests for NuGet.PackageManagement.
