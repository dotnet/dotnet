// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable disable

using System;
using System.IO;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using FluentAssertions;
using Microsoft.Internal.NuGet.Testing.SignedPackages.ChildProcess;
using NuGet.CommandLine.XPlat;
using NuGet.Test.Utility;
using NuGet.XPlat.FuncTest;
using Test.Utility;
using Xunit;
using Xunit.Abstractions;

namespace Dotnet.Integration.Test
{
    [Collection(DotnetIntegrationCollection.Name)]
    public class DotnetWhyTests
    {
        private static readonly string ProjectName = "Test.Project.DotnetNugetWhy";

        private readonly DotnetIntegrationTestFixture _testFixture;
        private readonly ITestOutputHelper _testOutputHelper;

        public DotnetWhyTests(DotnetIntegrationTestFixture testFixture, ITestOutputHelper testOutputHelper)
        {
            _testFixture = testFixture;
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task WhyCommand_ProjectHasTransitiveDependency_DependencyPathExists()
        {
            // Arrange
            var pathContext = new SimpleTestPathContext();
            var project = XPlatTestUtils.CreateProject(ProjectName, pathContext, TestConstants.ProjectTargetFramework);

            var packageX = XPlatTestUtils.CreatePackage("PackageX", "1.0.0", TestConstants.ProjectTargetFramework);
            var packageY = XPlatTestUtils.CreatePackage("PackageY", "1.0.1", TestConstants.ProjectTargetFramework);

            packageX.Dependencies.Add(packageY);

            project.AddPackageToFramework(TestConstants.ProjectTargetFramework, packageX);

            await SimpleTestPackageUtility.CreatePackagesAsync(
                pathContext.PackageSource,
                packageX,
                packageY);

            string addPackageCommandArgs = $"add {project.ProjectPath} package {packageX.Id}";
            CommandRunnerResult addPackageResult = _testFixture.RunDotnetExpectSuccess(pathContext.SolutionRoot, addPackageCommandArgs, testOutputHelper: _testOutputHelper);

            string whyCommandArgs = $"nuget why {project.ProjectPath} {packageY.Id}";

            // Act
            CommandRunnerResult result = _testFixture.RunDotnetExpectSuccess(pathContext.SolutionRoot, whyCommandArgs, testOutputHelper: _testOutputHelper);

            // Assert
            Assert.Equal(ExitCodes.Success, result.ExitCode);
            Assert.Contains($"Project '{ProjectName}' has the following dependency graph(s) for '{packageY.Id}'", result.AllOutput.Replace("\n", "").Replace("\r", ""));
        }

        [Fact]
        public async Task WhyCommand_ProjectHasNoDependencyOnTargetPackage_PathDoesNotExist()
        {
            // Arrange
            var pathContext = new SimpleTestPathContext();
            var project = XPlatTestUtils.CreateProject(ProjectName, pathContext, TestConstants.ProjectTargetFramework);

            var packageX = XPlatTestUtils.CreatePackage("PackageX", "1.0.0", TestConstants.ProjectTargetFramework);
            project.AddPackageToFramework(TestConstants.ProjectTargetFramework, packageX);

            var packageZ = XPlatTestUtils.CreatePackage("PackageZ", "1.0.0", TestConstants.ProjectTargetFramework);

            await SimpleTestPackageUtility.CreatePackagesAsync(
                pathContext.PackageSource,
                packageX,
                packageZ);

            string addPackageCommandArgs = $"add {project.ProjectPath} package {packageX.Id}";
            CommandRunnerResult addPackageResult = _testFixture.RunDotnetExpectSuccess(pathContext.SolutionRoot, addPackageCommandArgs, testOutputHelper: _testOutputHelper);

            string whyCommandArgs = $"nuget why {project.ProjectPath} {packageZ.Id}";

            // Act
            CommandRunnerResult result = _testFixture.RunDotnetExpectSuccess(pathContext.SolutionRoot, whyCommandArgs, testOutputHelper: _testOutputHelper);

            // Assert
            Assert.Equal(ExitCodes.Success, result.ExitCode);
            Assert.Contains($"Project '{ProjectName}' does not have a dependency on '{packageZ.Id}'", result.AllOutput);
        }

        [Fact]
        public async Task WhyCommand_WithFrameworksOption_OptionParsedSuccessfully()
        {
            // Arrange
            var pathContext = new SimpleTestPathContext();
            var project = XPlatTestUtils.CreateProject(ProjectName, pathContext, TestConstants.ProjectTargetFramework);

            var packageX = XPlatTestUtils.CreatePackage("PackageX", "1.0.0", TestConstants.ProjectTargetFramework);
            var packageY = XPlatTestUtils.CreatePackage("PackageY", "1.0.1", TestConstants.ProjectTargetFramework);

            packageX.Dependencies.Add(packageY);

            project.AddPackageToFramework(TestConstants.ProjectTargetFramework, packageX);

            await SimpleTestPackageUtility.CreatePackagesAsync(
                pathContext.PackageSource,
                packageX,
                packageY);

            string addPackageCommandArgs = $"add {project.ProjectPath} package {packageX.Id}";
            CommandRunnerResult addPackageResult = _testFixture.RunDotnetExpectSuccess(pathContext.SolutionRoot, addPackageCommandArgs, testOutputHelper: _testOutputHelper);

            string whyCommandArgs = $"nuget why {project.ProjectPath} {packageY.Id} --framework {TestConstants.ProjectTargetFramework}";

            // Act
            CommandRunnerResult result = _testFixture.RunDotnetExpectSuccess(pathContext.SolutionRoot, whyCommandArgs, testOutputHelper: _testOutputHelper);

            // Assert
            Assert.Equal(ExitCodes.Success, result.ExitCode);
            Assert.Contains($"Project '{ProjectName}' has the following dependency graph(s) for '{packageY.Id}'", result.AllOutput.Replace("\n", "").Replace("\r", ""));
        }

        [Fact]
        public async Task WhyCommand_WithFrameworksOptionAlias_OptionParsedSuccessfully()
        {
            // Arrange
            var pathContext = new SimpleTestPathContext();
            var project = XPlatTestUtils.CreateProject(ProjectName, pathContext, TestConstants.ProjectTargetFramework);

            var packageX = XPlatTestUtils.CreatePackage("PackageX", "1.0.0", TestConstants.ProjectTargetFramework);
            var packageY = XPlatTestUtils.CreatePackage("PackageY", "1.0.1", TestConstants.ProjectTargetFramework);

            packageX.Dependencies.Add(packageY);

            project.AddPackageToFramework(TestConstants.ProjectTargetFramework, packageX);

            await SimpleTestPackageUtility.CreatePackagesAsync(
                pathContext.PackageSource,
                packageX,
                packageY);

            string addPackageCommandArgs = $"add {project.ProjectPath} package {packageX.Id}";
            CommandRunnerResult addPackageResult = _testFixture.RunDotnetExpectSuccess(pathContext.SolutionRoot, addPackageCommandArgs, testOutputHelper: _testOutputHelper);

            string whyCommandArgs = $"nuget why {project.ProjectPath} {packageY.Id} -f {TestConstants.ProjectTargetFramework}";

            // Act
            CommandRunnerResult result = _testFixture.RunDotnetExpectSuccess(pathContext.SolutionRoot, whyCommandArgs, testOutputHelper: _testOutputHelper);

            // Assert
            Assert.Equal(ExitCodes.Success, result.ExitCode);
            Assert.Contains($"Project '{ProjectName}' has the following dependency graph(s) for '{packageY.Id}'", result.AllOutput.Replace("\n", "").Replace("\r", ""));
        }

        [Fact]
        public void WhyCommand_EmptyProjectArgument_Fails()
        {
            // Arrange
            var pathContext = new SimpleTestPathContext();

            string whyCommandArgs = $"nuget why";

            // Act
            CommandRunnerResult result = _testFixture.RunDotnetExpectFailure(pathContext.SolutionRoot, whyCommandArgs, testOutputHelper: _testOutputHelper);

            // Assert
            Assert.Equal(ExitCodes.InvalidArguments, result.ExitCode);
            Assert.Contains($"Required argument missing for command: 'why'.", result.Errors);
        }

        [Fact]
        public void WhyCommand_EmptyPackageArgument_Fails()
        {
            // Arrange
            var pathContext = new SimpleTestPathContext();
            var project = XPlatTestUtils.CreateProject(ProjectName, pathContext, TestConstants.ProjectTargetFramework);

            string whyCommandArgs = $"nuget why {project.ProjectPath}";

            // Act
            CommandRunnerResult result = _testFixture.RunDotnetExpectFailure(pathContext.SolutionRoot, whyCommandArgs, testOutputHelper: _testOutputHelper);

            // Assert
            Assert.Equal(ExitCodes.InvalidArguments, result.ExitCode);
            Assert.Contains($"Required argument missing for command: 'why'.", result.Errors);
        }

        [Fact]
        public async Task WhyCommand_DirectoryWithProject_HasTransitiveDependency_DependencyPathExists()
        {
            // Arrange
            var pathContext = new SimpleTestPathContext();
            var project = XPlatTestUtils.CreateProject(ProjectName, pathContext, TestConstants.ProjectTargetFramework);

            var packageX = XPlatTestUtils.CreatePackage("PackageX", "1.0.0", TestConstants.ProjectTargetFramework);
            var packageY = XPlatTestUtils.CreatePackage("PackageY", "1.0.1", TestConstants.ProjectTargetFramework);

            packageX.Dependencies.Add(packageY);

            project.AddPackageToFramework(TestConstants.ProjectTargetFramework, packageX);

            await SimpleTestPackageUtility.CreatePackagesAsync(
                pathContext.PackageSource,
                packageX,
                packageY);

            string addPackageCommandArgs = $"add {project.ProjectPath} package {packageX.Id}";
            CommandRunnerResult addPackageResult = _testFixture.RunDotnetExpectSuccess(pathContext.SolutionRoot, addPackageCommandArgs, testOutputHelper: _testOutputHelper);

            var projectDirectory = Path.GetDirectoryName(project.ProjectPath);
            string whyCommandArgs = $"nuget why {projectDirectory} {packageY.Id}";

            // Act
            CommandRunnerResult result = _testFixture.RunDotnetExpectSuccess(pathContext.SolutionRoot, whyCommandArgs, testOutputHelper: _testOutputHelper);

            // Assert
            Assert.Equal(ExitCodes.Success, result.ExitCode);
            result.AllOutput.Replace("\n", "").Replace("\r", "").Should().Contain($"Project '{ProjectName}' has the following dependency graph(s) for '{packageY.Id}'");
        }

        [Fact]
        public async Task WhyCommand_AssetsFileWithoutProject_Succeeds()
        {
            // Arrange
            var pathContext = new SimpleTestPathContext();
            var project = XPlatTestUtils.CreateProject(ProjectName, pathContext, TestConstants.ProjectTargetFramework);

            var packageX = XPlatTestUtils.CreatePackage("PackageX", "1.0.0", TestConstants.ProjectTargetFramework);
            var packageY = XPlatTestUtils.CreatePackage("PackageY", "1.0.1", TestConstants.ProjectTargetFramework);

            packageX.Dependencies.Add(packageY);

            project.AddPackageToFramework(TestConstants.ProjectTargetFramework, packageX);

            await SimpleTestPackageUtility.CreatePackagesAsync(
                pathContext.PackageSource,
                packageX,
                packageY);

            string addPackageCommandArgs = $"add {project.ProjectPath} package {packageX.Id}";
            CommandRunnerResult addPackageResult = _testFixture.RunDotnetExpectSuccess(pathContext.SolutionRoot, addPackageCommandArgs, testOutputHelper: _testOutputHelper);

            var assetsFile = Path.Combine(Path.GetDirectoryName(project.ProjectPath), "obj", "project.assets.json");

            // Act
            string whyCommandArgs = $"nuget why {assetsFile} {packageY.Id}";
            CommandRunnerResult result = _testFixture.RunDotnetExpectSuccess(pathContext.SolutionRoot, whyCommandArgs, testOutputHelper: _testOutputHelper);

            // Assert
            result.AllOutput.Should().Contain(packageX.Id);
        }

        [Fact]
        public void WhyCommand_EmptyJsonFile_OutputsError()
        {
            // Arrange
            using TestDirectory testDirectory = TestDirectory.Create();
            var jsonFilePath = Path.Combine(testDirectory, "test.json");
            File.WriteAllText(jsonFilePath, "{}");

            // Act
            string whyCommandArgs = $"nuget why {jsonFilePath} packageId";
            CommandRunnerResult result = _testFixture.RunDotnetExpectFailure(testDirectory, whyCommandArgs, testOutputHelper: _testOutputHelper);

            // Assert
            result.AllOutput.Should().Contain("https://aka.ms/dotnet/nuget/why");
        }

        [Fact]
        public async Task WhyCommand_ProjectReference_Succeeds()
        {
            // Arrange
            var pathContext = new SimpleTestPathContext();
            var projectA = XPlatTestUtils.CreateProject("ProjectA", pathContext, TestConstants.ProjectTargetFramework);
            var projectB = XPlatTestUtils.CreateProject("ProjectB", pathContext, TestConstants.ProjectTargetFramework);
            var projectC = XPlatTestUtils.CreateProject("ProjectC", pathContext, TestConstants.ProjectTargetFramework);

            var packageX = XPlatTestUtils.CreatePackage("PackageX", "1.0.0", TestConstants.ProjectTargetFramework);

            projectA.AddPackageToFramework(TestConstants.ProjectTargetFramework, packageX);
            projectA.Save();
            projectB.AddProjectToAllFrameworks(projectA);
            projectB.Save();
            projectC.AddProjectToAllFrameworks(projectB);
            projectC.Save();

            await SimpleTestPackageUtility.CreatePackagesAsync(
                pathContext.PackageSource,
                packageX);

            string addPackageCommandArgs = $"add {projectA.ProjectPath} package {packageX.Id}";
            CommandRunnerResult addPackageResult = _testFixture.RunDotnetExpectSuccess(pathContext.SolutionRoot, addPackageCommandArgs, testOutputHelper: _testOutputHelper);

            CommandRunnerResult restoreResult = _testFixture.RunDotnetExpectSuccess(pathContext.SolutionRoot, $"restore {projectC.ProjectPath}", testOutputHelper: _testOutputHelper);

            // Act
            string whyCommandArgs = $"nuget why {projectC.ProjectPath} {packageX.Id}";
            CommandRunnerResult result = _testFixture.RunDotnetExpectSuccess(pathContext.SolutionRoot, whyCommandArgs, testOutputHelper: _testOutputHelper);

            // Assert
            // project references should not have version numbers
            string[] expected =
                [
                "Project 'ProjectC' has the following dependency graph(s) for 'PackageX':",
                "",
                $"  [{TestConstants.ProjectTargetFramework}]                                                                     ",
                "  └── ProjectB                                                                  ",
                "      └── ProjectA                                                              ",
                "          └── PackageX@1.0.0 (>= 1.0.0)                                         ",
                "",
                "",
                ""
                ];
            StripAnsiCodes(result.AllOutput).Should().Be(string.Join(Environment.NewLine, expected));
        }

        private static string StripAnsiCodes(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            return Regex.Replace(input, @"\x1B\[[0-?]*[ -/]*[@-~]", string.Empty);
        }
    }
}
