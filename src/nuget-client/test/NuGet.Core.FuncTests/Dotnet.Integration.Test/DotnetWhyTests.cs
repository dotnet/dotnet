// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Internal.NuGet.Testing.SignedPackages.ChildProcess;
using NuGet.CommandLine.XPlat;
using NuGet.Test.Utility;
using NuGet.XPlat.FuncTest;
using Xunit;

namespace Dotnet.Integration.Test
{
    [Collection(DotnetIntegrationCollection.Name)]
    public class DotnetWhyTests
    {
        private static readonly string ProjectName = "Test.Project.DotnetNugetWhy";

        private readonly DotnetIntegrationTestFixture _testFixture;

        public DotnetWhyTests(DotnetIntegrationTestFixture testFixture)
        {
            _testFixture = testFixture;
        }

        [Fact]
        public async Task WhyCommand_ProjectHasTransitiveDependency_DependencyPathExists()
        {
            // Arrange
            var pathContext = new SimpleTestPathContext();
            var projectFramework = "net7.0";
            var project = XPlatTestUtils.CreateProject(ProjectName, pathContext, projectFramework);

            var packageX = XPlatTestUtils.CreatePackage("PackageX", "1.0.0", projectFramework);
            var packageY = XPlatTestUtils.CreatePackage("PackageY", "1.0.1", projectFramework);

            packageX.Dependencies.Add(packageY);

            project.AddPackageToFramework(projectFramework, packageX);

            await SimpleTestPackageUtility.CreatePackagesAsync(
                pathContext.PackageSource,
                packageX,
                packageY);

            string addPackageCommandArgs = $"add {project.ProjectPath} package {packageX.Id}";
            CommandRunnerResult addPackageResult = _testFixture.RunDotnetExpectSuccess(pathContext.SolutionRoot, addPackageCommandArgs);

            string whyCommandArgs = $"nuget why {project.ProjectPath} {packageY.Id}";

            // Act
            CommandRunnerResult result = _testFixture.RunDotnetExpectSuccess(pathContext.SolutionRoot, whyCommandArgs);

            // Assert
            Assert.Equal(ExitCodes.Success, result.ExitCode);
            Assert.Contains($"Project '{ProjectName}' has the following dependency graph(s) for '{packageY.Id}'", result.AllOutput);
        }

        [Fact]
        public async Task WhyCommand_ProjectHasNoDependencyOnTargetPackage_PathDoesNotExist()
        {
            // Arrange
            var pathContext = new SimpleTestPathContext();
            var projectFramework = "net7.0";
            var project = XPlatTestUtils.CreateProject(ProjectName, pathContext, projectFramework);

            var packageX = XPlatTestUtils.CreatePackage("PackageX", "1.0.0", projectFramework);
            project.AddPackageToFramework(projectFramework, packageX);

            var packageZ = XPlatTestUtils.CreatePackage("PackageZ", "1.0.0", projectFramework);

            await SimpleTestPackageUtility.CreatePackagesAsync(
                pathContext.PackageSource,
                packageX,
                packageZ);

            string addPackageCommandArgs = $"add {project.ProjectPath} package {packageX.Id}";
            CommandRunnerResult addPackageResult = _testFixture.RunDotnetExpectSuccess(pathContext.SolutionRoot, addPackageCommandArgs);

            string whyCommandArgs = $"nuget why {project.ProjectPath} {packageZ.Id}";

            // Act
            CommandRunnerResult result = _testFixture.RunDotnetExpectSuccess(pathContext.SolutionRoot, whyCommandArgs);

            // Assert
            Assert.Equal(ExitCodes.Success, result.ExitCode);
            Assert.Contains($"Project '{ProjectName}' does not have a dependency on '{packageZ.Id}'", result.AllOutput);
        }

        [Fact]
        public async Task WhyCommand_WithFrameworksOption_OptionParsedSuccessfully()
        {
            // Arrange
            var pathContext = new SimpleTestPathContext();
            var projectFramework = "net7.0";
            var project = XPlatTestUtils.CreateProject(ProjectName, pathContext, projectFramework);

            var packageX = XPlatTestUtils.CreatePackage("PackageX", "1.0.0", projectFramework);
            var packageY = XPlatTestUtils.CreatePackage("PackageY", "1.0.1", projectFramework);

            packageX.Dependencies.Add(packageY);

            project.AddPackageToFramework(projectFramework, packageX);

            await SimpleTestPackageUtility.CreatePackagesAsync(
                pathContext.PackageSource,
                packageX,
                packageY);

            string addPackageCommandArgs = $"add {project.ProjectPath} package {packageX.Id}";
            CommandRunnerResult addPackageResult = _testFixture.RunDotnetExpectSuccess(pathContext.SolutionRoot, addPackageCommandArgs);

            string whyCommandArgs = $"nuget why {project.ProjectPath} {packageY.Id} --framework {projectFramework}";

            // Act
            CommandRunnerResult result = _testFixture.RunDotnetExpectSuccess(pathContext.SolutionRoot, whyCommandArgs);

            // Assert
            Assert.Equal(ExitCodes.Success, result.ExitCode);
            Assert.Contains($"Project '{ProjectName}' has the following dependency graph(s) for '{packageY.Id}'", result.AllOutput);
        }

        [Fact]
        public async Task WhyCommand_WithFrameworksOptionAlias_OptionParsedSuccessfully()
        {
            // Arrange
            var pathContext = new SimpleTestPathContext();
            var projectFramework = "net7.0";
            var project = XPlatTestUtils.CreateProject(ProjectName, pathContext, projectFramework);

            var packageX = XPlatTestUtils.CreatePackage("PackageX", "1.0.0", projectFramework);
            var packageY = XPlatTestUtils.CreatePackage("PackageY", "1.0.1", projectFramework);

            packageX.Dependencies.Add(packageY);

            project.AddPackageToFramework(projectFramework, packageX);

            await SimpleTestPackageUtility.CreatePackagesAsync(
                pathContext.PackageSource,
                packageX,
                packageY);

            string addPackageCommandArgs = $"add {project.ProjectPath} package {packageX.Id}";
            CommandRunnerResult addPackageResult = _testFixture.RunDotnetExpectSuccess(pathContext.SolutionRoot, addPackageCommandArgs);

            string whyCommandArgs = $"nuget why {project.ProjectPath} {packageY.Id} -f {projectFramework}";

            // Act
            CommandRunnerResult result = _testFixture.RunDotnetExpectSuccess(pathContext.SolutionRoot, whyCommandArgs);

            // Assert
            Assert.Equal(ExitCodes.Success, result.ExitCode);
            Assert.Contains($"Project '{ProjectName}' has the following dependency graph(s) for '{packageY.Id}'", result.AllOutput);
        }

        [Fact]
        public void WhyCommand_EmptyProjectArgument_Fails()
        {
            // Arrange
            var pathContext = new SimpleTestPathContext();

            string whyCommandArgs = $"nuget why";

            // Act
            CommandRunnerResult result = _testFixture.RunDotnetExpectFailure(pathContext.SolutionRoot, whyCommandArgs);

            // Assert
            Assert.Equal(ExitCodes.InvalidArguments, result.ExitCode);
            Assert.Contains($"Required argument missing for command: 'why'.", result.Errors);
        }

        [Fact]
        public void WhyCommand_EmptyPackageArgument_Fails()
        {
            // Arrange
            var pathContext = new SimpleTestPathContext();
            var projectFramework = "net7.0";
            var project = XPlatTestUtils.CreateProject(ProjectName, pathContext, projectFramework);

            string whyCommandArgs = $"nuget why {project.ProjectPath}";

            // Act
            CommandRunnerResult result = _testFixture.RunDotnetExpectFailure(pathContext.SolutionRoot, whyCommandArgs);

            // Assert
            Assert.Equal(ExitCodes.InvalidArguments, result.ExitCode);
            Assert.Contains($"Required argument missing for command: 'why'.", result.Errors);
        }

        [Fact]
        public async Task WhyCommand_InvalidFrameworksOption_WarnsCorrectly()
        {
            // Arrange
            var pathContext = new SimpleTestPathContext();
            var projectFramework = "net7.0";
            var inputFrameworksOption = "invalidFrameworkAlias";
            var project = XPlatTestUtils.CreateProject(ProjectName, pathContext, projectFramework);

            var packageX = XPlatTestUtils.CreatePackage("PackageX", "1.0.0", projectFramework);
            var packageY = XPlatTestUtils.CreatePackage("PackageY", "1.0.1", projectFramework);

            packageX.Dependencies.Add(packageY);

            project.AddPackageToFramework(projectFramework, packageX);

            await SimpleTestPackageUtility.CreatePackagesAsync(
                pathContext.PackageSource,
                packageX,
                packageY);

            string addPackageCommandArgs = $"add {project.ProjectPath} package {packageX.Id}";
            CommandRunnerResult addPackageResult = _testFixture.RunDotnetExpectSuccess(pathContext.SolutionRoot, addPackageCommandArgs);

            string whyCommandArgs = $"nuget why {project.ProjectPath} {packageY.Id} -f {inputFrameworksOption} -f {projectFramework}";

            // Act
            CommandRunnerResult result = _testFixture.RunDotnetExpectSuccess(pathContext.SolutionRoot, whyCommandArgs);

            // Assert
            Assert.Equal(ExitCodes.Success, result.ExitCode);
            Assert.Contains($"warn : The assets file '{project.AssetsFileOutputPath}' for project '{ProjectName}' does not contain a target for the specified input framework '{inputFrameworksOption}'.", result.AllOutput);
            Assert.Contains($"Project '{ProjectName}' has the following dependency graph(s) for '{packageY.Id}'", result.AllOutput);
        }

        [Fact]
        public async Task WhyCommand_DirectoryWithProject_HasTransitiveDependency_DependencyPathExists()
        {
            // Arrange
            var pathContext = new SimpleTestPathContext();
            var projectFramework = "net7.0";
            var project = XPlatTestUtils.CreateProject(ProjectName, pathContext, projectFramework);

            var packageX = XPlatTestUtils.CreatePackage("PackageX", "1.0.0", projectFramework);
            var packageY = XPlatTestUtils.CreatePackage("PackageY", "1.0.1", projectFramework);

            packageX.Dependencies.Add(packageY);

            project.AddPackageToFramework(projectFramework, packageX);

            await SimpleTestPackageUtility.CreatePackagesAsync(
                pathContext.PackageSource,
                packageX,
                packageY);

            string addPackageCommandArgs = $"add {project.ProjectPath} package {packageX.Id}";
            CommandRunnerResult addPackageResult = _testFixture.RunDotnetExpectSuccess(pathContext.SolutionRoot, addPackageCommandArgs);

            var projectDirectory = Path.GetDirectoryName(project.ProjectPath);
            string whyCommandArgs = $"nuget why {projectDirectory} {packageY.Id}";

            // Act
            CommandRunnerResult result = _testFixture.RunDotnetExpectSuccess(pathContext.SolutionRoot, whyCommandArgs);

            // Assert
            Assert.Equal(ExitCodes.Success, result.ExitCode);
            Assert.Contains($"Project '{ProjectName}' has the following dependency graph(s) for '{packageY.Id}'", result.AllOutput);
        }

        [Fact]
        public async Task WhyCommand_AssetsFileWithoutProject_Succeeds()
        {
            // Arrange
            var pathContext = new SimpleTestPathContext();
            var projectFramework = "net7.0";
            var project = XPlatTestUtils.CreateProject(ProjectName, pathContext, projectFramework);

            var packageX = XPlatTestUtils.CreatePackage("PackageX", "1.0.0", projectFramework);
            var packageY = XPlatTestUtils.CreatePackage("PackageY", "1.0.1", projectFramework);

            packageX.Dependencies.Add(packageY);

            project.AddPackageToFramework(projectFramework, packageX);

            await SimpleTestPackageUtility.CreatePackagesAsync(
                pathContext.PackageSource,
                packageX,
                packageY);

            string addPackageCommandArgs = $"add {project.ProjectPath} package {packageX.Id}";
            CommandRunnerResult addPackageResult = _testFixture.RunDotnetExpectSuccess(pathContext.SolutionRoot, addPackageCommandArgs);

            var assetsFile = Path.Combine(Path.GetDirectoryName(project.ProjectPath), "obj", "project.assets.json");

            // Act
            string whyCommandArgs = $"nuget why {assetsFile} {packageY.Id}";
            CommandRunnerResult result = _testFixture.RunDotnetExpectSuccess(pathContext.SolutionRoot, whyCommandArgs);

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
            CommandRunnerResult result = _testFixture.RunDotnetExpectFailure(testDirectory, whyCommandArgs);

            // Assert
            result.AllOutput.Should().Contain("https://aka.ms/dotnet/nuget/why");
        }
    }
}
