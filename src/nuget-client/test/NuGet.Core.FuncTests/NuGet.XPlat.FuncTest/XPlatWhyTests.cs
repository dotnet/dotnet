// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Internal.NuGet.Testing.SignedPackages;
using Moq;
using NuGet.CommandLine.XPlat;
using NuGet.CommandLine.XPlat.Commands.Why;
using NuGet.Common;
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

        [Theory]
        [InlineData(false, 10)]
        [InlineData(false, 11)]
        [InlineData(true, 10)]
        [InlineData(true, 11)]
        public async Task WhyCommand_ProjectHasTransitiveDependency_DependencyPathExists(bool fileBasedApp, int dotnetVersion)
        {
            // Arrange
            var pathContext = new SimpleTestPathContext();
            var projectFramework = "net472";
            var project = XPlatTestUtils.CreateProject(ProjectName, pathContext, projectFramework, fileBasedApp);

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
            using var builder = TestVirtualProjectBuilder.From(project);
            var msbuild = new MSBuildAPIUtility(logger, builder);
            var addPackageArgs = XPlatTestUtils.GetPackageReferenceArgs(logger, packageX.Id, packageX.Version, project);
            var addPackageCommandRunner = new AddPackageReferenceCommandRunner();
            var addPackageResult = await addPackageCommandRunner.ExecuteCommand(addPackageArgs, msbuild);

            var console = new TestConsole();
            console.Width(100);

            var versionChecker = new Mock<IDotnetVersionChecker>();
            versionChecker.Setup(v => v.DotnetVersion).Returns(dotnetVersion);

            var whyCommandArgs = new WhyCommandArgs
            {
                Path = project.ProjectPath,
                Package = packageY.Id,
                Frameworks = [projectFramework],
                Logger = console,
                CancellationToken = CancellationToken.None,
                DotnetVersionChecker = versionChecker.Object,
            };

            // Act
            var result = await new WhyCommandRunner(msbuild).ExecuteCommand(whyCommandArgs);

            // Assert
            Assert.Equal(ExitCodes.Success, result);

            string[] packageLines = dotnetVersion >= 11
                ? [
                    "  └── PackageX@1.0.0 (>= 1.0.0)",
                    "      └── PackageY@1.0.1 (>= 1.0.1)",
                ]
                : [
                    "  └── PackageX (v1.0.0)",
                    "      └── PackageY (v1.0.1)",
                ];

            string expected = string.Join("\n", (string[])
                [
                "Project 'Test.Project.DotnetNugetWhy' has the following dependency graph(s) for 'PackageY':",
                "",
                "  [net472]",
                .. packageLines,
                ]);
            string actual = string.Join("\n", console.Lines.Select(line => line.TrimEnd()));
            actual.Should().Be(expected);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(11)]
        public async Task WhyCommand_TransitiveDependencyWithMultipleRequestedVersions_ShowsRequestedAndResolvedVersions(int dotnetVersion)
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
            var addPackageResult = await addPackageCommandRunner.ExecuteCommand(addPackageArgs, new MSBuildAPIUtility(logger, virtualProjectBuilder: null));

            addPackageArgs = XPlatTestUtils.GetPackageReferenceArgs(logger, packageB.Id, packageB.Version, project);
            addPackageResult = await addPackageCommandRunner.ExecuteCommand(addPackageArgs, new MSBuildAPIUtility(logger, virtualProjectBuilder: null));

            var console = new TestConsole();
            console.Width(100);

            var versionChecker = new Mock<IDotnetVersionChecker>();
            versionChecker.Setup(v => v.DotnetVersion).Returns(dotnetVersion);

            var whyCommandArgs = new WhyCommandArgs
            {
                Path = project.ProjectPath,
                Package = packageDepV1.Id,
                Frameworks = [projectFramework],
                Logger = console,
                CancellationToken = CancellationToken.None,
                DotnetVersionChecker = versionChecker.Object,
            };

            // Act
            var result = await new WhyCommandRunner(new MSBuildAPIUtility(logger, virtualProjectBuilder: null)).ExecuteCommand(whyCommandArgs);

            // Assert
            Assert.Equal(ExitCodes.Success, result);

            string[] packageLines = dotnetVersion >= 11
                ? [
                    "  ├── PackageA@1.2.3 (>= 1.2.3)",
                    "  │   └── Some.Dependency@2.0.0 (>= 1.0.0)",
                    "  └── PackageB@3.2.1 (>= 3.2.1)",
                    "      └── Some.Dependency@2.0.0 (>= 2.0.0)",
                ]
                : [
                    "  ├── PackageA (v1.2.3)",
                    "  │   └── Some.Dependency (v2.0.0)",
                    "  └── PackageB (v3.2.1)",
                    "      └── Some.Dependency (v2.0.0)",
                ];

            string expected = string.Join("\n", (string[])
                [
                "Project 'Test.Project.DotnetNugetWhy' has the following dependency graph(s) for 'Some.Dependency':",
                "",
                "  [net472]",
                .. packageLines,
                ]);
            string actual = string.Join("\n", console.Lines.Select(line => line.TrimEnd()));
            actual.Should().Be(expected);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(11)]
        public async Task WhyCommand_WithFloatingVersion_ShowsFloatingVersion(int dotnetVersion)
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
            var addPackageResult = await addPackageCommandRunner.ExecuteCommand(addPackageArgs, new MSBuildAPIUtility(logger, virtualProjectBuilder: null));

            var console = new TestConsole();
            console.Width(100);

            var versionChecker = new Mock<IDotnetVersionChecker>();
            versionChecker.Setup(v => v.DotnetVersion).Returns(dotnetVersion);

            var whyCommandArgs = new WhyCommandArgs
            {
                Path = project.ProjectPath,
                Package = packageA.Id,
                Frameworks = [projectFramework],
                Logger = console,
                CancellationToken = CancellationToken.None,
                DotnetVersionChecker = versionChecker.Object,
            };

            // Act
            var result = await new WhyCommandRunner(new MSBuildAPIUtility(logger, virtualProjectBuilder: null)).ExecuteCommand(whyCommandArgs);

            // Assert
            Assert.Equal(ExitCodes.Success, result);

            string[] packageLines = dotnetVersion >= 11
                ? [
                    "  └── PackageA@1.2.3 (>= 1.*)",
                ]
                : [
                    "  └── PackageA (v1.2.3)",
                ];

            string expected = string.Join("\n", (string[])
                [
                "Project 'Test.Project.DotnetNugetWhy' has the following dependency graph(s) for 'PackageA':",
                "",
                "  [net472]",
                .. packageLines,
                ]);
            string actual = string.Join("\n", console.Lines.Select(line => line.TrimEnd()));
            actual.Should().Be(expected);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(11)]
        public async Task WhyCommand_ProjectHasNoDependencyOnTargetPackage_PathDoesNotExist(int dotnetVersion)
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
            var addPackageResult = await addPackageCommandRunner.ExecuteCommand(addPackageArgs, new MSBuildAPIUtility(logger, virtualProjectBuilder: null));

            var console = new TestConsole();
            console.Width(500);

            var versionChecker = new Mock<IDotnetVersionChecker>();
            versionChecker.Setup(v => v.DotnetVersion).Returns(dotnetVersion);

            var whyCommandArgs = new WhyCommandArgs
            {
                Path = project.ProjectPath,
                Package = packageZ.Id,
                Frameworks = [projectFramework],
                Logger = console,
                CancellationToken = CancellationToken.None,
                DotnetVersionChecker = versionChecker.Object,
            };

            // Act
            var result = await new WhyCommandRunner(new MSBuildAPIUtility(logger, virtualProjectBuilder: null)).ExecuteCommand(whyCommandArgs);

            // Assert
            var output = console.Output;

            Assert.Equal(ExitCodes.Success, result);
            Assert.Contains($"Project '{ProjectName}' does not have a dependency on '{packageZ.Id}'", output);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(11)]
        public async Task WhyCommand_ProjectDidNotRunRestore_Fails(int dotnetVersion)
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

            var versionChecker = new Mock<IDotnetVersionChecker>();
            versionChecker.Setup(v => v.DotnetVersion).Returns(dotnetVersion);

            var whyCommandArgs = new WhyCommandArgs
            {
                Path = project.ProjectPath,
                Package = packageY.Id,
                Frameworks = [projectFramework],
                Logger = logger,
                CancellationToken = CancellationToken.None,
                DotnetVersionChecker = versionChecker.Object,
            };

            // Act
            var result = await new WhyCommandRunner(new MSBuildAPIUtility(NullLogger.Instance, virtualProjectBuilder: null)).ExecuteCommand(whyCommandArgs);

            // Assert
            var output = logger.Lines;

            Assert.Equal(ExitCodes.Success, result);
            Assert.Contains($"No assets file was found for `{project.ProjectPath}`. Please run restore before running this command.", output);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(11)]
        public async Task WhyCommand_EmptyProjectArgument_Fails(int dotnetVersion)
        {
            // Arrange
            var logger = new TestConsole();
            logger.Width(500);

            var versionChecker = new Mock<IDotnetVersionChecker>();
            versionChecker.Setup(v => v.DotnetVersion).Returns(dotnetVersion);

            var whyCommandArgs = new WhyCommandArgs
            {
                Path = "",
                Package = "PackageX",
                Frameworks = [],
                Logger = logger,
                CancellationToken = CancellationToken.None,
                DotnetVersionChecker = versionChecker.Object,
            };

            // Act
            var result = await new WhyCommandRunner(new MSBuildAPIUtility(NullLogger.Instance, virtualProjectBuilder: null)).ExecuteCommand(whyCommandArgs);

            // Assert
            var errorOutput = logger.Lines;

            Assert.Equal(ExitCodes.InvalidArguments, result);
            errorOutput.Should().Contain($"Unable to run 'dotnet nuget why'. The 'PROJECT|SOLUTION' argument cannot be empty.");
        }

        [Theory]
        [InlineData(10)]
        [InlineData(11)]
        public async Task WhyCommand_EmptyPackageArgument_Fails(int dotnetVersion)
        {
            // Arrange
            var logger = new TestConsole();

            var pathContext = new SimpleTestPathContext();
            var projectFramework = "net472";
            var project = XPlatTestUtils.CreateProject(ProjectName, pathContext, projectFramework);

            var versionChecker = new Mock<IDotnetVersionChecker>();
            versionChecker.Setup(v => v.DotnetVersion).Returns(dotnetVersion);

            var whyCommandArgs = new WhyCommandArgs
            {
                Path = project.ProjectPath,
                Package = "",
                Frameworks = [],
                Logger = logger,
                CancellationToken = CancellationToken.None,
                DotnetVersionChecker = versionChecker.Object,
            };

            // Act
            var result = await new WhyCommandRunner(new MSBuildAPIUtility(NullLogger.Instance, virtualProjectBuilder: null)).ExecuteCommand(whyCommandArgs);

            // Assert
            var errorOutput = logger.Lines;

            Assert.Equal(ExitCodes.InvalidArguments, result);
            Assert.Contains($"Unable to run 'dotnet nuget why'. The 'PACKAGE' argument cannot be empty.", errorOutput);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(11)]
        public async Task WhyCommand_InvalidProject_Fails(int dotnetVersion)
        {
            // Arrange
            var logger = new TestConsole();
            logger.Width(500);

            string fakeProjectPath = "FakeProjectPath.csproj";

            var versionChecker = new Mock<IDotnetVersionChecker>();
            versionChecker.Setup(v => v.DotnetVersion).Returns(dotnetVersion);

            var whyCommandArgs = new WhyCommandArgs
            {
                Path = fakeProjectPath,
                Package = "PackageX",
                Frameworks = [],
                Logger = logger,
                CancellationToken = CancellationToken.None,
                DotnetVersionChecker = versionChecker.Object,
            };

            // Act
            var result = await new WhyCommandRunner(new MSBuildAPIUtility(NullLogger.Instance, virtualProjectBuilder: null)).ExecuteCommand(whyCommandArgs);

            // Assert
            var errorOutput = logger.Lines;

            Assert.Equal(ExitCodes.InvalidArguments, result);
            Assert.Contains($"Unable to run 'dotnet nuget why'. Missing or invalid path '{fakeProjectPath}'. Please provide a path to a project, solution file, file-based app, or project directory.", errorOutput);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(11)]
        public async Task WhyCommand_InvalidFrameworksOption_WarnsCorrectly(int dotnetVersion)
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
            var addPackageResult = await addPackageCommandRunner.ExecuteCommand(addPackageCommandArgs, new MSBuildAPIUtility(logger, virtualProjectBuilder: null));

            var console = new TestConsole();
            console.Width(500);

            var versionChecker = new Mock<IDotnetVersionChecker>();
            versionChecker.Setup(v => v.DotnetVersion).Returns(dotnetVersion);

            var whyCommandArgs = new WhyCommandArgs
            {
                Path = project.ProjectPath,
                Package = packageY.Id,
                Frameworks = [inputFrameworksOption, projectFramework],
                Logger = console,
                CancellationToken = CancellationToken.None,
                DotnetVersionChecker = versionChecker.Object,
            };

            // Act
            var result = await new WhyCommandRunner(new MSBuildAPIUtility(logger, virtualProjectBuilder: null)).ExecuteCommand(whyCommandArgs);

            // Assert
            var output = console.Output;

            Assert.Equal(ExitCodes.Success, result);
            Assert.Contains($"The assets file '{project.AssetsFileOutputPath}' for project '{ProjectName}' does not contain a target for the specified input framework '{inputFrameworksOption}'.", output);
            Assert.Contains($"Project '{ProjectName}' has the following dependency graph(s) for '{packageY.Id}'", output);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(11)]
        public async Task WhyCommand_WithLegacyProjectAssetsFile_OutputsPackageGraph(int dotnetVersion)
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

            var versionChecker = new Mock<IDotnetVersionChecker>();
            versionChecker.Setup(v => v.DotnetVersion).Returns(dotnetVersion);

            var whyCommandArgs = new WhyCommandArgs
            {
                Path = assetsPath,
                Package = "PackageC",
                Frameworks = [],
                Logger = console,
                CancellationToken = CancellationToken.None,
                DotnetVersionChecker = versionChecker.Object,
            };

            // Act
            var result = await new WhyCommandRunner(new MSBuildAPIUtility(NullLogger.Instance, virtualProjectBuilder: null)).ExecuteCommand(whyCommandArgs);

            // Assert
            var output = console.Output;

            string[] packageLines = dotnetVersion >= 11
                ? [
                    "  └── PackageA@1.0.0 (>= 1.0.0)",
                    "      ├── PackageB@1.0.0 (>= 1.0.0)",
                    "      │   └── PackageC@2.0.0 (>= 1.0.0)",
                    "      └── PackageC@2.0.0 (>= 2.0.0)",
                ]
                : [
                    "  └── PackageA (v1.0.0)",
                    "      ├── PackageB (v1.0.0)",
                    "      │   └── PackageC (v2.0.0)",
                    "      └── PackageC (v2.0.0)",
                ];

            string expected = string.Join(Environment.NewLine,
                (string[])[
                    "Project 'ConsoleApp17' has the following dependency graph(s) for 'PackageC':",
                    "",
                    "  [net481]",
                    "  [net481/win]",
                    "  [net481/win-arm64]",
                    "  [net481/win-x64]",
                    "  [net481/win-x86]",
                    .. packageLines,
                ]);
            string actual = string.Join(Environment.NewLine, console.Lines.Select(line => line.TrimEnd()));
            actual.Should().BeEquivalentTo(expected, $"Full output: {actual}");
        }
    }
}
