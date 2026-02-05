// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Internal.NuGet.Testing.SignedPackages;
using NuGet.CommandLine.XPlat;
using NuGet.CommandLine.XPlat.Commands.Why;
using NuGet.Packaging;
using NuGet.ProjectModel;
using NuGet.Test.Utility;
using Spectre.Console.Testing;
using Xunit;
using Xunit.Abstractions;

namespace NuGet.XPlat.FuncTest
{
    [Collection(XPlatCollection.Name)]
    public class XPlatWhyTests
    {
        private static readonly string ProjectName = "Test.Project.DotnetNugetWhy";

        private readonly ITestOutputHelper _testOutputHelper;

        public XPlatWhyTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task WhyCommand_ProjectHasTransitiveDependency_DependencyPathExists()
        {
            // Arrange
            var pathContext = new SimpleTestPathContext();
            var projectFramework = "net472";
            var project = XPlatTestUtils.CreateProject(ProjectName, pathContext, projectFramework);

            var packageX = XPlatTestUtils.CreatePackage("PackageX", "1.0.0");
            var packageY = XPlatTestUtils.CreatePackage("PackageY", "1.0.1");

            packageX.Dependencies.Add(packageY);

            project.AddPackageToFramework(projectFramework, packageX);

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                pathContext.PackageSource,
                PackageSaveMode.Defaultv3,
                packageX,
                packageY);

            var logger = new TestCommandOutputLogger(_testOutputHelper);
            var addPackageArgs = XPlatTestUtils.GetPackageReferenceArgs(logger, packageX.Id, packageX.Version, project);
            var addPackageCommandRunner = new AddPackageReferenceCommandRunner();
            var addPackageResult = await addPackageCommandRunner.ExecuteCommand(addPackageArgs, new MSBuildAPIUtility(logger));

            var console = new TestConsole();
            console.Width(100);

            var whyCommandArgs = new WhyCommandArgs(
                    project.ProjectPath,
                    packageY.Id,
                    [projectFramework],
                    console,
                    CancellationToken.None);

            // Act
            var result = await WhyCommandRunner.ExecuteCommand(whyCommandArgs);

            // Assert
            Assert.Equal(ExitCodes.Success, result);

            string expected = string.Join("\n", (string[])
                [
                "Project 'Test.Project.DotnetNugetWhy' has the following dependency graph(s) for 'PackageY':",
                "",
                "  [net472]",
                "  └── PackageX@1.0.0 (>= 1.0.0)",
                "      └── PackageY@1.0.1 (>= 1.0.1)",
                ]);
            string actual = string.Join("\n", console.Lines.Select(line => line.TrimEnd()));
            actual.Should().Be(expected);
        }

        [Fact]
        public async Task WhyCommand_TransitiveDependencyWithMultipleRequestedVersions_ShowsRequestedAndResolvedVersions()
        {
            // Arrange
            var pathContext = new SimpleTestPathContext();
            var projectFramework = "net472";
            var project = XPlatTestUtils.CreateProject(ProjectName, pathContext, projectFramework);

            var packageA = XPlatTestUtils.CreatePackage("PackageA", "1.2.3");
            var packageB = XPlatTestUtils.CreatePackage("PackageB", "3.2.1");
            var packageDepV1 = XPlatTestUtils.CreatePackage("Some.Dependency", "1.0.0");
            var packageDepV2 = XPlatTestUtils.CreatePackage("Some.Dependency", "2.0.0");

            packageA.Dependencies.Add(packageDepV1);
            packageB.Dependencies.Add(packageDepV2);

            project.AddPackageToFramework(projectFramework, packageA, packageB);

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                pathContext.PackageSource,
                PackageSaveMode.Defaultv3,
                packageA,
                packageB,
                packageDepV1,
                packageDepV2);

            var logger = new TestCommandOutputLogger(_testOutputHelper);
            var addPackageArgs = XPlatTestUtils.GetPackageReferenceArgs(logger, packageA.Id, packageA.Version, project);
            var addPackageCommandRunner = new AddPackageReferenceCommandRunner();
            var addPackageResult = await addPackageCommandRunner.ExecuteCommand(addPackageArgs, new MSBuildAPIUtility(logger));

            addPackageArgs = XPlatTestUtils.GetPackageReferenceArgs(logger, packageB.Id, packageB.Version, project);
            addPackageResult = await addPackageCommandRunner.ExecuteCommand(addPackageArgs, new MSBuildAPIUtility(logger));

            var console = new TestConsole();
            console.Width(100);

            var whyCommandArgs = new WhyCommandArgs(
                    project.ProjectPath,
                    packageDepV1.Id,
                    [projectFramework],
                    console,
                    CancellationToken.None);

            // Act
            var result = await WhyCommandRunner.ExecuteCommand(whyCommandArgs);

            // Assert
            Assert.Equal(ExitCodes.Success, result);

            string expected = string.Join("\n", (string[])
                [
                "Project 'Test.Project.DotnetNugetWhy' has the following dependency graph(s) for 'Some.Dependency':",
                "",
                "  [net472]",
                "  ├── PackageA@1.2.3 (>= 1.2.3)",
                "  │   └── Some.Dependency@2.0.0 (>= 1.0.0)",
                "  └── PackageB@3.2.1 (>= 3.2.1)",
                "      └── Some.Dependency@2.0.0 (>= 2.0.0)",
                ]);
            string actual = string.Join("\n", console.Lines.Select(line => line.TrimEnd()));
            actual.Should().Be(expected);
        }

        [Fact]
        public async Task WhyCommand_WithFloatingVersion_ShowsFloatingVersion()
        {
            // Arrange
            var pathContext = new SimpleTestPathContext();
            var projectFramework = "net472";
            var project = XPlatTestUtils.CreateProject(ProjectName, pathContext, projectFramework);

            var packageA = XPlatTestUtils.CreatePackage("PackageA", "1.2.3");

            project.AddPackageToFramework(projectFramework, packageA);

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                pathContext.PackageSource,
                PackageSaveMode.Defaultv3,
                packageA);

            var logger = new TestCommandOutputLogger(_testOutputHelper);
            var addPackageArgs = XPlatTestUtils.GetPackageReferenceArgs(logger, packageA.Id, "1.*", project);
            var addPackageCommandRunner = new AddPackageReferenceCommandRunner();
            var addPackageResult = await addPackageCommandRunner.ExecuteCommand(addPackageArgs, new MSBuildAPIUtility(logger));

            var console = new TestConsole();
            console.Width(100);

            var whyCommandArgs = new WhyCommandArgs(
                    project.ProjectPath,
                    packageA.Id,
                    [projectFramework],
                    console,
                    CancellationToken.None);

            // Act
            var result = await WhyCommandRunner.ExecuteCommand(whyCommandArgs);

            // Assert
            Assert.Equal(ExitCodes.Success, result);

            string expected = string.Join("\n", (string[])
                [
                "Project 'Test.Project.DotnetNugetWhy' has the following dependency graph(s) for 'PackageA':",
                "",
                "  [net472]",
                "  └── PackageA@1.2.3 (>= 1.*)",
                ]);
            string actual = string.Join("\n", console.Lines.Select(line => line.TrimEnd()));
            actual.Should().Be(expected);
        }

        [Fact]
        public async Task WhyCommand_ProjectHasNoDependencyOnTargetPackage_PathDoesNotExist()
        {
            // Arrange
            var pathContext = new SimpleTestPathContext();
            var projectFramework = "net472";
            var project = XPlatTestUtils.CreateProject(ProjectName, pathContext, projectFramework);

            var packageX = XPlatTestUtils.CreatePackage("PackageX", "1.0.0");
            project.AddPackageToFramework(projectFramework, packageX);

            var packageZ = XPlatTestUtils.CreatePackage("PackageZ", "1.0.0"); // not added to project

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                pathContext.PackageSource,
                PackageSaveMode.Defaultv3,
                packageX,
                packageZ);

            var logger = new TestCommandOutputLogger(_testOutputHelper);
            var addPackageArgs = XPlatTestUtils.GetPackageReferenceArgs(logger, packageX.Id, packageX.Version, project);
            var addPackageCommandRunner = new AddPackageReferenceCommandRunner();
            var addPackageResult = await addPackageCommandRunner.ExecuteCommand(addPackageArgs, new MSBuildAPIUtility(logger));

            var console = new TestConsole();
            console.Width(500);

            var whyCommandArgs = new WhyCommandArgs(
                    project.ProjectPath,
                    packageZ.Id,
                    [projectFramework],
                    console,
                    CancellationToken.None);

            // Act
            var result = await WhyCommandRunner.ExecuteCommand(whyCommandArgs);

            // Assert
            var output = console.Output;

            Assert.Equal(ExitCodes.Success, result);
            Assert.Contains($"Project '{ProjectName}' does not have a dependency on '{packageZ.Id}'", output);
        }

        [Fact]
        public async Task WhyCommand_ProjectDidNotRunRestore_Fails()
        {
            // Arrange
            var logger = new TestConsole();
            logger.Width(500);

            var pathContext = new SimpleTestPathContext();
            var projectFramework = "net472";
            var project = XPlatTestUtils.CreateProject(ProjectName, pathContext, projectFramework);

            var packageX = XPlatTestUtils.CreatePackage("PackageX", "1.0.0");
            var packageY = XPlatTestUtils.CreatePackage("PackageY", "1.0.1");

            packageX.Dependencies.Add(packageY);

            project.AddPackageToFramework(projectFramework, packageX);

            var whyCommandArgs = new WhyCommandArgs(
                    project.ProjectPath,
                    packageY.Id,
                    [projectFramework],
                    logger,
                    CancellationToken.None);

            // Act
            var result = await WhyCommandRunner.ExecuteCommand(whyCommandArgs);

            // Assert
            var output = logger.Lines;

            Assert.Equal(ExitCodes.Success, result);
            Assert.Contains($"No assets file was found for `{project.ProjectPath}`. Please run restore before running this command.", output);
        }

        [Fact]
        public async Task WhyCommand_EmptyProjectArgument_Fails()
        {
            // Arrange
            var logger = new TestConsole();
            logger.Width(500);

            var whyCommandArgs = new WhyCommandArgs(
                    "",
                    "PackageX",
                    [],
                    logger,
                    CancellationToken.None);

            // Act
            var result = await WhyCommandRunner.ExecuteCommand(whyCommandArgs);

            // Assert
            var errorOutput = logger.Lines;

            Assert.Equal(ExitCodes.InvalidArguments, result);
            errorOutput.Should().Contain($"Unable to run 'dotnet nuget why'. The 'PROJECT|SOLUTION' argument cannot be empty.");
        }

        [Fact]
        public async Task WhyCommand_EmptyPackageArgument_Fails()
        {
            // Arrange
            var logger = new TestConsole();

            var pathContext = new SimpleTestPathContext();
            var projectFramework = "net472";
            var project = XPlatTestUtils.CreateProject(ProjectName, pathContext, projectFramework);

            var whyCommandArgs = new WhyCommandArgs(
                    project.ProjectPath,
                    "",
                    [],
                    logger,
                    CancellationToken.None);

            // Act
            var result = await WhyCommandRunner.ExecuteCommand(whyCommandArgs);

            // Assert
            var errorOutput = logger.Lines;

            Assert.Equal(ExitCodes.InvalidArguments, result);
            Assert.Contains($"Unable to run 'dotnet nuget why'. The 'PACKAGE' argument cannot be empty.", errorOutput);
        }

        [Fact]
        public async Task WhyCommand_InvalidProject_Fails()
        {
            // Arrange
            var logger = new TestConsole();
            logger.Width(500);

            string fakeProjectPath = "FakeProjectPath.csproj";

            var whyCommandArgs = new WhyCommandArgs(
                    fakeProjectPath,
                    "PackageX",
                    [],
                    logger,
                    CancellationToken.None);

            // Act
            var result = await WhyCommandRunner.ExecuteCommand(whyCommandArgs);

            // Assert
            var errorOutput = logger.Lines;

            Assert.Equal(ExitCodes.InvalidArguments, result);
            Assert.Contains($"Unable to run 'dotnet nuget why'. Missing or invalid path '{fakeProjectPath}'. Please provide a path to a project, solution file, or directory.", errorOutput);
        }

        [Fact]
        public async Task WhyCommand_InvalidFrameworksOption_WarnsCorrectly()
        {
            // Arrange
            var pathContext = new SimpleTestPathContext();
            var projectFramework = "net472";
            var inputFrameworksOption = "invalidFrameworkAlias";
            var project = XPlatTestUtils.CreateProject(ProjectName, pathContext, projectFramework);

            var packageX = XPlatTestUtils.CreatePackage("PackageX", "1.0.0", projectFramework);
            var packageY = XPlatTestUtils.CreatePackage("PackageY", "1.0.1", projectFramework);

            packageX.Dependencies.Add(packageY);

            project.AddPackageToFramework(projectFramework, packageX);

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                pathContext.PackageSource,
                PackageSaveMode.Defaultv3,
                packageX,
                packageY);

            var logger = new TestCommandOutputLogger(_testOutputHelper);
            var addPackageCommandArgs = XPlatTestUtils.GetPackageReferenceArgs(logger, packageX.Id, packageX.Version, project);
            var addPackageCommandRunner = new AddPackageReferenceCommandRunner();
            var addPackageResult = await addPackageCommandRunner.ExecuteCommand(addPackageCommandArgs, new MSBuildAPIUtility(logger));

            var console = new TestConsole();
            console.Width(500);

            var whyCommandArgs = new WhyCommandArgs(
                    project.ProjectPath,
                    packageY.Id,
                    [inputFrameworksOption, projectFramework],
                    console,
                    CancellationToken.None);

            // Act
            var result = await WhyCommandRunner.ExecuteCommand(whyCommandArgs);

            // Assert
            var output = console.Output;

            Assert.Equal(ExitCodes.Success, result);
            Assert.Contains($"The assets file '{project.AssetsFileOutputPath}' for project '{ProjectName}' does not contain a target for the specified input framework '{inputFrameworksOption}'.", output);
            Assert.Contains($"Project '{ProjectName}' has the following dependency graph(s) for '{packageY.Id}'", output);
        }

        [Fact]
        public async Task WhyCommand_WithLegacyProjectAssetsFile_OutputsPackageGraph()
        {
            // Arrange
            var pathContext = new SimpleTestPathContext();

            // Legacy projects' assets files don't have a target alias
            string assetsContent = ResourceTestUtility.GetResource(
                "NuGet.XPlat.FuncTest.compiler.resources.legacy.project.assets.json",
                GetType());
            string assetsPath = Path.Combine(pathContext.SolutionRoot, LockFileFormat.AssetsFileName);
            File.WriteAllText(assetsPath, assetsContent);

            var console = new TestConsole();
            console.Width(100);

            var whyCommandArgs = new WhyCommandArgs(
                    assetsPath,
                    "PackageC",
                    [],
                    console,
                    CancellationToken.None);

            // Act
            var result = await WhyCommandRunner.ExecuteCommand(whyCommandArgs);

            // Assert
            var output = console.Output;

            string expected = string.Join(Environment.NewLine,
                (string[])[
                    "Project 'ConsoleApp17' has the following dependency graph(s) for 'PackageC':",
                    "",
                    "  [net481]",
                    "  [net481/win]",
                    "  [net481/win-arm64]",
                    "  [net481/win-x64]",
                    "  [net481/win-x86]",
                    "  └── PackageA@1.0.0 (>= 1.0.0)",
                    "      ├── PackageB@1.0.0 (>= 1.0.0)",
                    "      │   └── PackageC@2.0.0 (>= 1.0.0)",
                    "      └── PackageC@2.0.0 (>= 2.0.0)",
                ]);
            string actual = string.Join(Environment.NewLine, console.Lines.Select(line => line.TrimEnd()));
            actual.Should().BeEquivalentTo(expected, $"Full output: {actual}");
        }
    }
}
