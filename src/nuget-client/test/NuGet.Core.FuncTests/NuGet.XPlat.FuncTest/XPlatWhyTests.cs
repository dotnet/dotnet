// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.CommandLine.XPlat;
using NuGet.Packaging;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.XPlat.FuncTest
{
    [Collection("NuGet XPlat Test Collection")]
    public class XPlatWhyTests
    {
        private static readonly string ProjectName = "Test.Project.DotnetNugetWhy";
        private static MSBuildAPIUtility MsBuild => new MSBuildAPIUtility(new TestCommandOutputLogger());

        [Fact]
        public async void WhyCommand_ProjectHasTransitiveDependency_DependencyPathExists()
        {
            // Arrange
            var logger = new TestCommandOutputLogger();

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

            var addPackageArgs = XPlatTestUtils.GetPackageReferenceArgs(packageX.Id, packageX.Version, project);
            var addPackageCommandRunner = new AddPackageReferenceCommandRunner();
            var addPackageResult = await addPackageCommandRunner.ExecuteCommand(addPackageArgs, MsBuild);

            var whyCommandArgs = new WhyCommandArgs(
                    project.ProjectPath,
                    packageY.Id,
                    [projectFramework],
                    logger);

            // Act
            var result = WhyCommandRunner.ExecuteCommand(whyCommandArgs);

            // Assert
            var output = logger.ShowMessages();

            Assert.Equal(ExitCodes.Success, result);
            Assert.Contains($"Project '{ProjectName}' has the following dependency graph(s) for '{packageY.Id}'", output);
            Assert.Contains($"{packageX.Id} (v{packageX.Version})", output);
            Assert.Contains($"{packageY.Id} (v{packageY.Version})", output);
        }

        [Fact]
        public async void WhyCommand_ProjectHasNoDependencyOnTargetPackage_PathDoesNotExist()
        {
            // Arrange
            var logger = new TestCommandOutputLogger();

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

            var addPackageArgs = XPlatTestUtils.GetPackageReferenceArgs(packageX.Id, packageX.Version, project);
            var addPackageCommandRunner = new AddPackageReferenceCommandRunner();
            var addPackageResult = await addPackageCommandRunner.ExecuteCommand(addPackageArgs, MsBuild);

            var whyCommandArgs = new WhyCommandArgs(
                    project.ProjectPath,
                    packageZ.Id,
                    [projectFramework],
                    logger);

            // Act
            var result = WhyCommandRunner.ExecuteCommand(whyCommandArgs);

            // Assert
            var output = logger.ShowMessages();

            Assert.Equal(ExitCodes.Success, result);
            Assert.Contains($"Project '{ProjectName}' does not have a dependency on '{packageZ.Id}'", output);
        }

        [Fact]
        public void WhyCommand_ProjectDidNotRunRestore_Fails()
        {
            // Arrange
            var logger = new TestCommandOutputLogger();

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
                    logger);

            // Act
            var result = WhyCommandRunner.ExecuteCommand(whyCommandArgs);

            // Assert
            var output = logger.ShowMessages();

            Assert.Equal(ExitCodes.Success, result);
            Assert.Contains($"No assets file was found for `{project.ProjectPath}`. Please run restore before running this command.", output);
        }

        [Fact]
        public void WhyCommand_EmptyProjectArgument_Fails()
        {
            // Arrange
            var logger = new TestCommandOutputLogger();

            var whyCommandArgs = new WhyCommandArgs(
                    "",
                    "PackageX",
                    [],
                    logger);

            // Act
            var result = WhyCommandRunner.ExecuteCommand(whyCommandArgs);

            // Assert
            var errorOutput = logger.ShowErrors();

            Assert.Equal(ExitCodes.InvalidArguments, result);
            Assert.Contains($"Unable to run 'dotnet nuget why'. The 'PROJECT|SOLUTION' argument cannot be empty.", errorOutput);
        }

        [Fact]
        public void WhyCommand_EmptyPackageArgument_Fails()
        {
            // Arrange
            var logger = new TestCommandOutputLogger();

            var pathContext = new SimpleTestPathContext();
            var projectFramework = "net472";
            var project = XPlatTestUtils.CreateProject(ProjectName, pathContext, projectFramework);

            var whyCommandArgs = new WhyCommandArgs(
                    project.ProjectPath,
                    "",
                    [],
                    logger);

            // Act
            var result = WhyCommandRunner.ExecuteCommand(whyCommandArgs);

            // Assert
            var errorOutput = logger.ShowErrors();

            Assert.Equal(ExitCodes.InvalidArguments, result);
            Assert.Contains($"Unable to run 'dotnet nuget why'. The 'PACKAGE' argument cannot be empty.", errorOutput);
        }

        [Fact]
        public void WhyCommand_InvalidProject_Fails()
        {
            // Arrange
            var logger = new TestCommandOutputLogger();

            string fakeProjectPath = "FakeProjectPath.csproj";

            var whyCommandArgs = new WhyCommandArgs(
                    fakeProjectPath,
                    "PackageX",
                    [],
                    logger);

            // Act
            var result = WhyCommandRunner.ExecuteCommand(whyCommandArgs);

            // Assert
            var errorOutput = logger.ShowErrors();

            Assert.Equal(ExitCodes.InvalidArguments, result);
            Assert.Contains($"Unable to run 'dotnet nuget why'. Missing or invalid path '{fakeProjectPath}'. Please provide a path to a project, solution file, or directory.", errorOutput);
        }

        [Fact]
        public async void WhyCommand_InvalidFrameworksOption_WarnsCorrectly()
        {
            // Arrange
            var logger = new TestCommandOutputLogger();

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

            var addPackageCommandArgs = XPlatTestUtils.GetPackageReferenceArgs(packageX.Id, packageX.Version, project);
            var addPackageCommandRunner = new AddPackageReferenceCommandRunner();
            var addPackageResult = await addPackageCommandRunner.ExecuteCommand(addPackageCommandArgs, MsBuild);

            var whyCommandArgs = new WhyCommandArgs(
                    project.ProjectPath,
                    packageY.Id,
                    [inputFrameworksOption, projectFramework],
                    logger);

            // Act
            var result = WhyCommandRunner.ExecuteCommand(whyCommandArgs);

            // Assert
            var output = logger.ShowMessages();

            Assert.Equal(ExitCodes.Success, result);
            Assert.Contains($"The assets file '{project.AssetsFileOutputPath}' for project '{ProjectName}' does not contain a target for the specified input framework '{inputFrameworksOption}'.", output);
            Assert.Contains($"Project '{ProjectName}' has the following dependency graph(s) for '{packageY.Id}'", output);
        }
    }
}
