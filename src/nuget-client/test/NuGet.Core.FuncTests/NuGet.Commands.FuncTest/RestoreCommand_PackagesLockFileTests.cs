// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NuGet.Commands.Test;
using NuGet.LibraryModel;
using NuGet.ProjectModel;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Test.Utility.Commands;
using Xunit;
using static NuGet.Frameworks.FrameworkConstants;

namespace NuGet.Commands.FuncTest
{
    [Collection(TestCollection.Name)]
    public class RestoreCommand_PackagesLockFileTests
    {
        [Fact]
        public async Task RestoreCommand_PackagesLockFile_InLockedMode_WhenAPackageReferenceIsRemoved_FailsWithNU1004()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                var logger = new TestLogger();

                var packageA = new SimpleTestPackageContext("a", "1.0.0");
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    packageA);

                var projectName = "TestProject";
                var projectDirectory = Path.Combine(pathContext.SolutionRoot, projectName);
                var packageSpec = PackageReferenceSpecBuilder.Create(projectName, projectDirectory)
                    .WithTargetFrameworks(new string[] { "net46", "net47" })
                    .WithPackagesLockFile()
                    .Build();
                // Add the dependency to all frameworks
                PackageSpecOperations.AddOrUpdateDependency(packageSpec, packageA.Identity, packageSpec.TargetFrameworks.Select(e => e.FrameworkName));

                // Preconditions.
                var result = await new RestoreCommand(ProjectTestHelpers.CreateRestoreRequest(pathContext, logger, packageSpec)).ExecuteAsync();
                await result.CommitAsync(logger, CancellationToken.None);
                result.Success.Should().BeTrue();

                // Enable locked mode, remove a dependency and clear the logger;
                packageSpec.RestoreMetadata.RestoreLockProperties = new RestoreLockProperties(
                    restorePackagesWithLockFile: "true",
                    packageSpec.RestoreMetadata.RestoreLockProperties.NuGetLockFilePath,
                    restoreLockedMode: true);
                PackageSpecOperations.RemoveDependency(packageSpec, packageA.Identity.Id);
                logger.Clear();
                // Act.
                result = await new RestoreCommand(ProjectTestHelpers.CreateRestoreRequest(pathContext, logger, packageSpec)).ExecuteAsync();

                // Assert.
                result.Success.Should().BeFalse();
                logger.ErrorMessages.Count.Should().Be(1);
                logger.ErrorMessages.Single().Should().Contain("NU1004");
                logger.ErrorMessages.Single().Should().Contain("The package references have changed for net46. Lock file's package references: a:[1.0.0, ), project's package references: None.");
            }
        }

        [Fact]
        public async Task RestoreCommand_PackagesLockFile_InLockedMode_WhenATargetFrameworkIsAdded_FailsWithNU1004()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                var logger = new TestLogger();

                var packageA = new SimpleTestPackageContext("a", "1.0.0");
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    packageA);

                var projectName = "TestProject";
                var projectDirectory = Path.Combine(pathContext.SolutionRoot, projectName);
                var packageSpec = PackageReferenceSpecBuilder.Create(projectName, projectDirectory)
                    .WithTargetFrameworks(new string[] { "net46", "net47" })
                    .WithPackagesLockFile()
                    .Build();
                // Add the dependency to all frameworks
                PackageSpecOperations.AddOrUpdateDependency(packageSpec, packageA.Identity, packageSpec.TargetFrameworks.Select(e => e.FrameworkName));

                // Preconditions.
                var result = await (new RestoreCommand(ProjectTestHelpers.CreateRestoreRequest(pathContext, logger, packageSpec))).ExecuteAsync();
                await result.CommitAsync(logger, CancellationToken.None);
                result.Success.Should().BeTrue();

                // Enable locked mode, remove a dependency and clear the logger;
                packageSpec.RestoreMetadata.RestoreLockProperties = new RestoreLockProperties(
                    restorePackagesWithLockFile: "true",
                    packageSpec.RestoreMetadata.RestoreLockProperties.NuGetLockFilePath,
                    restoreLockedMode: true);
                PackageSpecOperationsUtility.AddTargetFramework(packageSpec, "net48");
                logger.Clear();
                // Act.
                result = await new RestoreCommand(ProjectTestHelpers.CreateRestoreRequest(pathContext, logger, packageSpec)).ExecuteAsync();

                // Assert.
                result.Success.Should().BeFalse();
                logger.ErrorMessages.Count.Should().Be(1);
                logger.ErrorMessages.Single().Should().Contain("NU1004");
                logger.ErrorMessages.Single().Should().Contain("Lock file target frameworks: net46,net47,net48. Project target frameworks net46,net47.");
            }
        }

        [Fact]
        public async Task RestoreCommand_PackagesLockFile_InLockedMode_WhenATargetFrameworkIsUpdated_FailsWithNU1004()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                var logger = new TestLogger();

                var packageA = new SimpleTestPackageContext("a", "1.0.0");
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    packageA);

                var projectName = "TestProject";
                var projectDirectory = Path.Combine(pathContext.SolutionRoot, projectName);
                var packageSpec = PackageReferenceSpecBuilder.Create(projectName, projectDirectory)
                    .WithTargetFrameworks(new string[] { "net46", "net47" })
                    .WithPackagesLockFile()
                    .Build();
                // Add the dependency to all frameworks
                PackageSpecOperations.AddOrUpdateDependency(packageSpec, packageA.Identity, packageSpec.TargetFrameworks.Select(e => e.FrameworkName));

                // Preconditions.
                var result = await (new RestoreCommand(ProjectTestHelpers.CreateRestoreRequest(pathContext, logger, packageSpec))).ExecuteAsync();
                await result.CommitAsync(logger, CancellationToken.None);
                result.Success.Should().BeTrue();

                // Enable locked mode, remove a dependency and clear the logger;
                packageSpec.RestoreMetadata.RestoreLockProperties = new RestoreLockProperties(
                    restorePackagesWithLockFile: "true",
                    packageSpec.RestoreMetadata.RestoreLockProperties.NuGetLockFilePath,
                    restoreLockedMode: true);
                PackageSpecOperationsUtility.RemoveTargetFramework(packageSpec, "net47");
                PackageSpecOperationsUtility.AddTargetFramework(packageSpec, "net48");
                logger.Clear();
                // Act.
                result = await new RestoreCommand(ProjectTestHelpers.CreateRestoreRequest(pathContext, logger, packageSpec)).ExecuteAsync();

                // Assert.
                result.Success.Should().BeFalse();
                logger.ErrorMessages.Count.Should().Be(1);
                logger.ErrorMessages.Single().Should().Contain("NU1004");
                logger.ErrorMessages.Single().Should().Contain("The project target framework net48 was not found in the lock file");
            }
        }

        [Fact]
        public async Task RestoreCommand_PackagesLockFile_InLockedMode_WhenADependecyIsAdded_FailsWithNU1004()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                var logger = new TestLogger();

                var packageA = new SimpleTestPackageContext("a", "1.0.0");
                var packageB = new SimpleTestPackageContext("b", "1.0.0");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    packageA,
                    packageB);

                var projectName = "TestProject";
                var projectDirectory = Path.Combine(pathContext.SolutionRoot, projectName);
                var packageSpec = PackageReferenceSpecBuilder.Create(projectName, projectDirectory)
                    .WithTargetFrameworks(new string[] { "net46" })
                    .WithPackagesLockFile()
                    .Build();
                // Add the dependency to all frameworks
                PackageSpecOperations.AddOrUpdateDependency(packageSpec, packageA.Identity, packageSpec.TargetFrameworks.Select(e => e.FrameworkName));

                // Preconditions.
                var result = await (new RestoreCommand(ProjectTestHelpers.CreateRestoreRequest(pathContext, logger, packageSpec))).ExecuteAsync();
                await result.CommitAsync(logger, CancellationToken.None);
                result.Success.Should().BeTrue();

                // Enable locked mode, remove a dependency and clear the logger;
                packageSpec.RestoreMetadata.RestoreLockProperties = new RestoreLockProperties(
                    restorePackagesWithLockFile: "true",
                    packageSpec.RestoreMetadata.RestoreLockProperties.NuGetLockFilePath,
                    restoreLockedMode: true);
                PackageSpecOperations.AddOrUpdateDependency(packageSpec, packageB.Identity, packageSpec.TargetFrameworks.Select(e => e.FrameworkName));

                logger.Clear();
                // Act.
                result = await new RestoreCommand(ProjectTestHelpers.CreateRestoreRequest(pathContext, logger, packageSpec)).ExecuteAsync();

                // Assert.
                result.Success.Should().BeFalse();
                logger.ErrorMessages.Count.Should().Be(1);
                logger.ErrorMessages.Single().Should().Contain("NU1004");
                logger.ErrorMessages.Single().Should().Contain("The package references have changed for net46. Lock file's package references: a:[1.0.0, ), project's package references: a:[1.0.0, ), b:[1.0.0, )");
            }
        }

        [Fact]
        public async Task RestoreCommand_PackagesLockFile_InLockedMode_WhenADependecyIsUpdated_FailsWithNU1004()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                var logger = new TestLogger();

                var packageA100 = new SimpleTestPackageContext("a", "1.0.0");
                var packageA200 = new SimpleTestPackageContext("a", "2.0.0");
                var packageB = new SimpleTestPackageContext("b", "1.0.0");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    packageA100,
                    packageB);

                var projectName = "TestProject";
                var projectDirectory = Path.Combine(pathContext.SolutionRoot, projectName);
                var packageSpec = PackageReferenceSpecBuilder.Create(projectName, projectDirectory)
                    .WithTargetFrameworks(new string[] { "net46" })
                    .WithPackagesLockFile()
                    .Build();
                // Add the dependency to all frameworks
                PackageSpecOperations.AddOrUpdateDependency(packageSpec, packageA100.Identity, packageSpec.TargetFrameworks.Select(e => e.FrameworkName));
                PackageSpecOperations.AddOrUpdateDependency(packageSpec, packageB.Identity, packageSpec.TargetFrameworks.Select(e => e.FrameworkName));

                // Preconditions.
                var result = await (new RestoreCommand(ProjectTestHelpers.CreateRestoreRequest(pathContext, logger, packageSpec))).ExecuteAsync();
                await result.CommitAsync(logger, CancellationToken.None);
                result.Success.Should().BeTrue();

                // Enable locked mode, remove a dependency and clear the logger;
                packageSpec.RestoreMetadata.RestoreLockProperties = new RestoreLockProperties(
                    restorePackagesWithLockFile: "true",
                    packageSpec.RestoreMetadata.RestoreLockProperties.NuGetLockFilePath,
                    restoreLockedMode: true);
                PackageSpecOperations.AddOrUpdateDependency(packageSpec, packageA200.Identity, packageSpec.TargetFrameworks.Select(e => e.FrameworkName));

                logger.Clear();
                // Act.
                result = await new RestoreCommand(ProjectTestHelpers.CreateRestoreRequest(pathContext, logger, packageSpec)).ExecuteAsync();

                // Assert.
                result.Success.Should().BeFalse();
                logger.ErrorMessages.Count.Should().Be(1);
                logger.ErrorMessages.Single().Should().Contain("NU1004");
                logger.ErrorMessages.Single().Should().Contain("The package reference a version has changed from [1.0.0, ) to [2.0.0, )");
            }
        }

        [Fact]
        public async Task RestoreCommand_PackagesLockFile_InLockedMode_WhenADependencyIsReplaced_FailsWithNU1004()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                var logger = new TestLogger();

                var packageA = new SimpleTestPackageContext("a", "1.0.0");
                var packageB = new SimpleTestPackageContext("b", "1.0.0");
                var packageC = new SimpleTestPackageContext("c", "1.0.0");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    packageA,
                    packageB);

                var projectName = "TestProject";
                var projectDirectory = Path.Combine(pathContext.SolutionRoot, projectName);
                var packageSpec = PackageReferenceSpecBuilder.Create(projectName, projectDirectory)
                    .WithTargetFrameworks(new string[] { "net46" })
                    .WithPackagesLockFile()
                    .Build();
                // Add the dependency to all frameworks
                PackageSpecOperations.AddOrUpdateDependency(packageSpec, packageA.Identity, packageSpec.TargetFrameworks.Select(e => e.FrameworkName));
                PackageSpecOperations.AddOrUpdateDependency(packageSpec, packageB.Identity, packageSpec.TargetFrameworks.Select(e => e.FrameworkName));

                // Preconditions.
                var result = await (new RestoreCommand(ProjectTestHelpers.CreateRestoreRequest(pathContext, logger, packageSpec))).ExecuteAsync();
                await result.CommitAsync(logger, CancellationToken.None);
                result.Success.Should().BeTrue();

                // Enable locked mode, remove a dependency and clear the logger;
                packageSpec.RestoreMetadata.RestoreLockProperties = new RestoreLockProperties(
                    restorePackagesWithLockFile: "true",
                    packageSpec.RestoreMetadata.RestoreLockProperties.NuGetLockFilePath,
                    restoreLockedMode: true);
                PackageSpecOperations.RemoveDependency(packageSpec, packageB.Identity.Id);
                PackageSpecOperations.AddOrUpdateDependency(packageSpec, packageC.Identity, packageSpec.TargetFrameworks.Select(e => e.FrameworkName));

                logger.Clear();
                // Act.
                result = await new RestoreCommand(ProjectTestHelpers.CreateRestoreRequest(pathContext, logger, packageSpec)).ExecuteAsync();

                // Assert.
                result.Success.Should().BeFalse();
                logger.ErrorMessages.Count.Should().Be(1);
                logger.ErrorMessages.Single().Should().Contain("NU1004");
                logger.ErrorMessages.Single().Should().Contain("A new package reference was found c for the project target framework net46.");
            }
        }

        [Fact]
        public async Task RestoreCommand_PackagesLockFile_InLockedMode_WhenARuntimeIdentifierIsAdded_FailsWithNU1004()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                var logger = new TestLogger();

                var packageA = new SimpleTestPackageContext("a", "1.0.0");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    packageA);

                var projectName = "TestProject";
                var projectDirectory = Path.Combine(pathContext.SolutionRoot, projectName);
                var packageSpec = PackageReferenceSpecBuilder.Create(projectName, projectDirectory)
                    .WithTargetFrameworks(new string[] { "net46", "net47" })
                    .WithRuntimeIdentifiers(runtimeIdentifiers: new string[] { "win", "unix" }, runtimeSupports: new string[] { })
                    .WithPackagesLockFile()
                    .Build();

                // Add the dependency to all frameworks
                PackageSpecOperations.AddOrUpdateDependency(packageSpec, packageA.Identity, packageSpec.TargetFrameworks.Select(e => e.FrameworkName));

                // Preconditions.
                var result = await (new RestoreCommand(ProjectTestHelpers.CreateRestoreRequest(pathContext, logger, packageSpec))).ExecuteAsync();
                await result.CommitAsync(logger, CancellationToken.None);
                result.Success.Should().BeTrue();

                // Enable locked mode, remove a dependency and clear the logger;
                packageSpec.RestoreMetadata.RestoreLockProperties = new RestoreLockProperties(
                    restorePackagesWithLockFile: "true",
                    packageSpec.RestoreMetadata.RestoreLockProperties.NuGetLockFilePath,
                    restoreLockedMode: true);
                packageSpec.RuntimeGraph = ProjectTestHelpers.GetRuntimeGraph(runtimeIdentifiers: new string[] { "win", "unix", "ios" }, runtimeSupports: new string[] { });
                logger.Clear();
                // Act.
                result = await new RestoreCommand(ProjectTestHelpers.CreateRestoreRequest(pathContext, logger, packageSpec)).ExecuteAsync();

                // Assert.
                result.Success.Should().BeFalse();
                logger.ErrorMessages.Count.Should().Be(1);
                logger.ErrorMessages.Single().Should().Contain("NU1004");
                logger.ErrorMessages.Single().Should().Contain("Project's runtime identifiers: ios;unix;win, lock file's runtime identifiers unix;win.");
            }
        }

        [Fact]
        public async Task RestoreCommand_PackagesLockFile_InLockedMode_WhenANewDirectProjectReferenceIsAdded_FailsWithNU1004()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                var logger = new TestLogger();
                var packageA = new SimpleTestPackageContext("a", "1.0.0");
                var packageB = new SimpleTestPackageContext("b", "1.0.0");
                var packageC = new SimpleTestPackageContext("b", "1.0.0");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    packageA,
                    packageB,
                    packageC);

                var targetFramework = CommonFrameworks.Net46;
                var allPackageSpecs = new List<PackageSpec>();

                var projectName = "RootProject";
                var projectDirectory = Path.Combine(pathContext.SolutionRoot, projectName);
                var rootPackageSpec = PackageReferenceSpecBuilder.Create(projectName, projectDirectory)
                    .WithTargetFrameworks(new string[] { targetFramework.GetShortFolderName() })
                    .WithPackagesLockFile()
                    .Build();

                projectName = "IntermediateProject1";
                projectDirectory = Path.Combine(pathContext.SolutionRoot, projectName);
                var intermediateProject1PackageSpec = PackageReferenceSpecBuilder.Create(projectName, projectDirectory)
                    .WithTargetFrameworks(new string[] { targetFramework.GetShortFolderName() })
                    .Build();

                projectName = "LeafProject";
                projectDirectory = Path.Combine(pathContext.SolutionRoot, projectName);
                var leafProjectPackageSpec = PackageReferenceSpecBuilder.Create(projectName, projectDirectory)
                    .WithTargetFrameworks(new string[] { targetFramework.GetShortFolderName() })
                    .Build();

                // Add the dependency to all frameworks
                PackageSpecOperations.AddOrUpdateDependency(rootPackageSpec, packageA.Identity, rootPackageSpec.TargetFrameworks.Select(e => e.FrameworkName));
                PackageSpecOperations.AddOrUpdateDependency(intermediateProject1PackageSpec, packageB.Identity, intermediateProject1PackageSpec.TargetFrameworks.Select(e => e.FrameworkName));
                PackageSpecOperations.AddOrUpdateDependency(leafProjectPackageSpec, packageC.Identity, leafProjectPackageSpec.TargetFrameworks.Select(e => e.FrameworkName));
                PackageSpecOperationsUtility.AddProjectReference(rootPackageSpec, intermediateProject1PackageSpec, targetFramework);
                PackageSpecOperationsUtility.AddProjectReference(intermediateProject1PackageSpec, leafProjectPackageSpec, targetFramework);

                // Preconditions.
                var result = await new RestoreCommand(ProjectTestHelpers.CreateRestoreRequest(pathContext, logger, rootPackageSpec, intermediateProject1PackageSpec, leafProjectPackageSpec)).ExecuteAsync();
                await result.CommitAsync(logger, CancellationToken.None);
                result.Success.Should().BeTrue();

                // Enable locked mode, remove a dependency and clear the logger;
                rootPackageSpec.RestoreMetadata.RestoreLockProperties = new RestoreLockProperties(
                    restorePackagesWithLockFile: "true",
                    rootPackageSpec.RestoreMetadata.RestoreLockProperties.NuGetLockFilePath,
                    restoreLockedMode: true);
                logger.Clear();

                projectName = "IntermediateProject2";
                projectDirectory = Path.Combine(pathContext.SolutionRoot, projectName);
                var intermediateProject2PackageSpec = PackageReferenceSpecBuilder.Create(projectName, projectDirectory)
                    .WithTargetFrameworks(new string[] { targetFramework.GetShortFolderName() })
                    .Build();
                PackageSpecOperationsUtility.AddProjectReference(rootPackageSpec, intermediateProject2PackageSpec, targetFramework);

                // Act.
                result = await new RestoreCommand(ProjectTestHelpers.CreateRestoreRequest(pathContext, logger, rootPackageSpec, intermediateProject1PackageSpec, leafProjectPackageSpec, intermediateProject2PackageSpec)).ExecuteAsync();

                // Assert.
                result.Success.Should().BeFalse();
                logger.ErrorMessages.Count.Should().Be(1);
                logger.ErrorMessages.Single().Should().Contain("NU1004");
                logger.ErrorMessages.Single().Should().Contain($"A new project reference to IntermediateProject2 was found for {targetFramework.GetShortFolderName()} target framework");
            }
        }

        [Fact]
        public async Task RestoreCommand_PackagesLockFile_InLockedMode_WhenANewTransitiveProjectReferenceIsAdded_FailsWithNU1004()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                var logger = new TestLogger();
                var packageA = new SimpleTestPackageContext("a", "1.0.0");
                var packageB = new SimpleTestPackageContext("b", "1.0.0");
                var packageC = new SimpleTestPackageContext("c", "1.0.0");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    packageA,
                    packageB,
                    packageC);

                var targetFramework = CommonFrameworks.Net46;

                var projectName = "RootProject";
                var projectDirectory = Path.Combine(pathContext.SolutionRoot, projectName);
                var rootPackageSpec = PackageReferenceSpecBuilder.Create(projectName, projectDirectory)
                    .WithTargetFrameworks(new string[] { targetFramework.GetShortFolderName() })
                    .WithPackagesLockFile()
                    .Build();

                projectName = "IntermediateProject";
                projectDirectory = Path.Combine(pathContext.SolutionRoot, projectName);
                var intermediatePackageSpec = PackageReferenceSpecBuilder.Create(projectName, projectDirectory)
                    .WithTargetFrameworks(new string[] { targetFramework.GetShortFolderName() })
                    .Build();

                projectName = "LeafProject1";
                projectDirectory = Path.Combine(pathContext.SolutionRoot, projectName);
                var leafProject1PackageSpec = PackageReferenceSpecBuilder.Create(projectName, projectDirectory)
                    .WithTargetFrameworks(new string[] { targetFramework.GetShortFolderName() })
                    .Build();

                // Add the dependency to all frameworks
                PackageSpecOperations.AddOrUpdateDependency(rootPackageSpec, packageA.Identity, rootPackageSpec.TargetFrameworks.Select(e => e.FrameworkName));
                PackageSpecOperations.AddOrUpdateDependency(intermediatePackageSpec, packageB.Identity, intermediatePackageSpec.TargetFrameworks.Select(e => e.FrameworkName));
                PackageSpecOperations.AddOrUpdateDependency(leafProject1PackageSpec, packageC.Identity, leafProject1PackageSpec.TargetFrameworks.Select(e => e.FrameworkName));
                PackageSpecOperationsUtility.AddProjectReference(rootPackageSpec, intermediatePackageSpec, targetFramework);
                PackageSpecOperationsUtility.AddProjectReference(intermediatePackageSpec, leafProject1PackageSpec, targetFramework);

                // Preconditions.
                var result = await new RestoreCommand(ProjectTestHelpers.CreateRestoreRequest(pathContext, logger, rootPackageSpec, intermediatePackageSpec, leafProject1PackageSpec)).ExecuteAsync();
                await result.CommitAsync(logger, CancellationToken.None);
                result.Success.Should().BeTrue();

                // Enable locked mode, remove a dependency and clear the logger;
                rootPackageSpec.RestoreMetadata.RestoreLockProperties = new RestoreLockProperties(
                    restorePackagesWithLockFile: "true",
                    rootPackageSpec.RestoreMetadata.RestoreLockProperties.NuGetLockFilePath,
                    restoreLockedMode: true);
                logger.Clear();

                projectName = "LeaftProject2";
                projectDirectory = Path.Combine(pathContext.SolutionRoot, projectName);
                var leafProject2PackageSpec = PackageReferenceSpecBuilder.Create(projectName, projectDirectory)
                    .WithTargetFrameworks(new string[] { targetFramework.GetShortFolderName() })
                    .Build();
                PackageSpecOperationsUtility.AddProjectReference(intermediatePackageSpec, leafProject2PackageSpec, targetFramework);

                // Act.
                result = await new RestoreCommand(ProjectTestHelpers.CreateRestoreRequest(pathContext, logger, rootPackageSpec, intermediatePackageSpec, leafProject1PackageSpec, leafProject2PackageSpec)).ExecuteAsync();

                // Assert.
                result.Success.Should().BeFalse();
                logger.ErrorMessages.Count.Should().Be(1);
                logger.ErrorMessages.Single().Should().Contain("NU1004");
                logger.ErrorMessages.Single().Should().Contain($"The project reference intermediateproject has changed. Current dependencies: b,LeafProject1,LeaftProject2 lock file's dependencies: b,LeafProject1.");
            }
        }

        [Fact]
        public async Task RestoreCommand_PackagesLockFile_InLockedMode_WhenANewDirectProjectReferenceChangesFramework_FailsWithNU1004()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                var logger = new TestLogger();
                var packageA = new SimpleTestPackageContext("a", "1.0.0");
                var packageB = new SimpleTestPackageContext("b", "1.0.0");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    packageA);

                var targetFramework = CommonFrameworks.Net46;

                var projectName = "RootProject";
                var projectDirectory = Path.Combine(pathContext.SolutionRoot, projectName);
                var rootPackageSpec = PackageReferenceSpecBuilder.Create(projectName, projectDirectory)
                    .WithTargetFrameworks(new string[] { "net46" })
                    .WithPackagesLockFile()
                    .Build();

                projectName = "IntermediateProject";
                projectDirectory = Path.Combine(pathContext.SolutionRoot, projectName);
                var projectReferenceSpec = PackageReferenceSpecBuilder.Create(projectName, projectDirectory)
                    .WithTargetFrameworks(new string[] { targetFramework.GetShortFolderName() })
                    .Build();

                // Add the dependency to all frameworks
                PackageSpecOperations.AddOrUpdateDependency(rootPackageSpec, packageA.Identity, rootPackageSpec.TargetFrameworks.Select(e => e.FrameworkName));
                PackageSpecOperationsUtility.AddProjectReference(rootPackageSpec, projectReferenceSpec, targetFramework);

                // Preconditions.
                var result = await new RestoreCommand(ProjectTestHelpers.CreateRestoreRequest(pathContext, logger, rootPackageSpec, projectReferenceSpec)).ExecuteAsync();
                await result.CommitAsync(logger, CancellationToken.None);
                result.Success.Should().BeTrue();

                // Enable locked mode, remove a dependency and clear the logger;
                rootPackageSpec.RestoreMetadata.RestoreLockProperties = new RestoreLockProperties(
                    restorePackagesWithLockFile: "true",
                    rootPackageSpec.RestoreMetadata.RestoreLockProperties.NuGetLockFilePath,
                    restoreLockedMode: true);
                logger.Clear();

                PackageSpecOperationsUtility.RemoveTargetFramework(projectReferenceSpec, "net46");
                PackageSpecOperationsUtility.AddTargetFramework(projectReferenceSpec, "net47");

                // Act.
                result = await new RestoreCommand(ProjectTestHelpers.CreateRestoreRequest(pathContext, logger, rootPackageSpec, projectReferenceSpec)).ExecuteAsync();

                // Assert.
                result.Success.Should().BeFalse();
                logger.ErrorMessages.Count.Should().Be(1);
                logger.ErrorMessages.Single().Should().Contain("NU1004");
                logger.ErrorMessages.Single().Should().Contain($"The project IntermediateProject has no compatible target framework.");
            }
        }

        [Fact]
        public async Task RestoreCommand_PackagesLockFile_InLockedMode_WhenANewTransitiveProjectReferenceChangesFramework_FailsWithNU1004()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                var logger = new TestLogger();
                var packageA = new SimpleTestPackageContext("a", "1.0.0");
                var packageB = new SimpleTestPackageContext("b", "1.0.0");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    packageA);

                var targetFramework = CommonFrameworks.Net46;

                var projectName = "RootProject";
                var projectDirectory = Path.Combine(pathContext.SolutionRoot, projectName);
                var rootPackageSpec = PackageReferenceSpecBuilder.Create(projectName, projectDirectory)
                    .WithTargetFrameworks(new string[] { "net46" })
                    .WithPackagesLockFile()
                    .Build();

                projectName = "IntermediateProject";
                projectDirectory = Path.Combine(pathContext.SolutionRoot, projectName);
                var intermediateProjectReferenceSpec = PackageReferenceSpecBuilder.Create(projectName, projectDirectory)
                    .WithTargetFrameworks(new string[] { targetFramework.GetShortFolderName() })
                    .Build();

                projectName = "LeafProject";
                projectDirectory = Path.Combine(pathContext.SolutionRoot, projectName);
                var leafProjectReferenceSpec = PackageReferenceSpecBuilder.Create(projectName, projectDirectory)
                    .WithTargetFrameworks(new string[] { targetFramework.GetShortFolderName() })
                    .Build();

                // Add the dependency to all frameworks
                PackageSpecOperations.AddOrUpdateDependency(rootPackageSpec, packageA.Identity, rootPackageSpec.TargetFrameworks.Select(e => e.FrameworkName));
                PackageSpecOperationsUtility.AddProjectReference(rootPackageSpec, intermediateProjectReferenceSpec, targetFramework);
                PackageSpecOperationsUtility.AddProjectReference(intermediateProjectReferenceSpec, leafProjectReferenceSpec, targetFramework);

                // Preconditions.
                var result = await new RestoreCommand(ProjectTestHelpers.CreateRestoreRequest(pathContext, logger, rootPackageSpec, intermediateProjectReferenceSpec, leafProjectReferenceSpec)).ExecuteAsync();
                await result.CommitAsync(logger, CancellationToken.None);
                result.Success.Should().BeTrue();

                // Enable locked mode, remove a dependency and clear the logger;
                rootPackageSpec.RestoreMetadata.RestoreLockProperties = new RestoreLockProperties(
                    restorePackagesWithLockFile: "true",
                    rootPackageSpec.RestoreMetadata.RestoreLockProperties.NuGetLockFilePath,
                    restoreLockedMode: true);
                logger.Clear();

                PackageSpecOperationsUtility.RemoveTargetFramework(leafProjectReferenceSpec, "net46");
                PackageSpecOperationsUtility.AddTargetFramework(leafProjectReferenceSpec, "net47");

                // Act.
                result = await new RestoreCommand(ProjectTestHelpers.CreateRestoreRequest(pathContext, logger, rootPackageSpec, intermediateProjectReferenceSpec, leafProjectReferenceSpec)).ExecuteAsync();

                // Assert.
                result.Success.Should().BeFalse();
                logger.ErrorMessages.Count.Should().Be(1);
                logger.ErrorMessages.Single().Should().Contain("NU1004");
                logger.ErrorMessages.Single().Should().Contain($"The project LeafProject has no compatible target framework.");
            }
        }

        [Fact]
        public async Task RestoreCommand_PackagesLockFile_InLockedMode_WithFloatingVersions_WhenNoChangesInProjectReferenceDependencies_Success()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                var logger = new TestLogger();
                var packageA = new SimpleTestPackageContext("a", "1.0.0");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    packageA);

                var targetFramework = CommonFrameworks.Net46;

                var projectName = "childProject";
                var projectDirectory = Path.Combine(pathContext.SolutionRoot, projectName);
                var childProject = PackageReferenceSpecBuilder.Create(projectName, projectDirectory)
                    .WithTargetFrameworks(new string[] { "net46" })
                    .WithPackagesLockFile()
                    .Build();

                // Add the dependency
                var dependency = new LibraryDependency
                {
                    LibraryRange = new LibraryRange(packageA.Id, VersionRange.Parse("1.0.*"), LibraryDependencyTarget.Package)
                };

                var newDependencies = childProject.TargetFrameworks.FirstOrDefault().Dependencies.Add(dependency);
                childProject.TargetFrameworks[0] = new TargetFrameworkInformation(childProject.TargetFrameworks[0]) { Dependencies = newDependencies };

                // Enable lock file
                childProject.RestoreMetadata.RestoreLockProperties = new RestoreLockProperties(
                   restorePackagesWithLockFile: "true",
                   childProject.RestoreMetadata.RestoreLockProperties.NuGetLockFilePath,
                   restoreLockedMode: false);

                var result = await new RestoreCommand(ProjectTestHelpers.CreateRestoreRequest(pathContext, logger, childProject)).ExecuteAsync();
                await result.CommitAsync(logger, CancellationToken.None);
                result.Success.Should().BeTrue();

                projectName = "parentProject";
                projectDirectory = Path.Combine(pathContext.SolutionRoot, projectName);
                var parentProject = PackageReferenceSpecBuilder.Create(projectName, projectDirectory)
                    .WithTargetFrameworks(new string[] { targetFramework.GetShortFolderName() })
                    .WithPackagesLockFile()
                    .Build();

                PackageSpecOperationsUtility.AddProjectReference(parentProject, childProject, targetFramework);

                // Enable lock file
                parentProject.RestoreMetadata.RestoreLockProperties = new RestoreLockProperties(
                   restorePackagesWithLockFile: "true",
                   parentProject.RestoreMetadata.RestoreLockProperties.NuGetLockFilePath,
                   restoreLockedMode: false);

                result = await new RestoreCommand(ProjectTestHelpers.CreateRestoreRequest(pathContext, logger, parentProject, childProject)).ExecuteAsync();
                await result.CommitAsync(logger, CancellationToken.None);
                result.Success.Should().BeTrue();

                // Act
                // Enable locked mode
                parentProject.RestoreMetadata.RestoreLockProperties = new RestoreLockProperties(
                   restorePackagesWithLockFile: "true",
                   parentProject.RestoreMetadata.RestoreLockProperties.NuGetLockFilePath,
                   restoreLockedMode: true);

                result = await new RestoreCommand(ProjectTestHelpers.CreateRestoreRequest(pathContext, logger, parentProject, childProject)).ExecuteAsync();
                await result.CommitAsync(logger, CancellationToken.None);

                // Assert.
                result.Success.Should().BeTrue();
            }
        }

        [Fact]
        public async Task RestoreCommand_PackagesLockFile_InLockedMode_WhenADirectProjectReferenceChangesDependencies_FailsWithNU1004()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                var logger = new TestLogger();
                var packageA = new SimpleTestPackageContext("a", "1.0.0");
                var packageB = new SimpleTestPackageContext("b", "1.0.0");
                var packageC = new SimpleTestPackageContext("c", "1.0.0");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    packageA,
                    packageB,
                    packageC);

                var targetFramework = CommonFrameworks.Net46;

                var projectName = "RootProject";
                var projectDirectory = Path.Combine(pathContext.SolutionRoot, projectName);
                var rootPackageSpec = PackageReferenceSpecBuilder.Create(projectName, projectDirectory)
                    .WithTargetFrameworks(new string[] { "net46" })
                    .WithPackagesLockFile()
                    .Build();

                projectName = "IntermediateProject";
                projectDirectory = Path.Combine(pathContext.SolutionRoot, projectName);
                var intermediateProjectReferenceSpec = PackageReferenceSpecBuilder.Create(projectName, projectDirectory)
                    .WithTargetFrameworks(new string[] { targetFramework.GetShortFolderName() })
                    .Build();


                projectName = "LeafProject";
                projectDirectory = Path.Combine(pathContext.SolutionRoot, projectName);
                var leafProjectReferenceSpec = PackageReferenceSpecBuilder.Create(projectName, projectDirectory)
                    .WithTargetFrameworks(new string[] { targetFramework.GetShortFolderName() })
                    .Build();

                // Add the dependency to all frameworks
                PackageSpecOperations.AddOrUpdateDependency(rootPackageSpec, packageA.Identity, rootPackageSpec.TargetFrameworks.Select(e => e.FrameworkName));
                PackageSpecOperations.AddOrUpdateDependency(intermediateProjectReferenceSpec, packageB.Identity, rootPackageSpec.TargetFrameworks.Select(e => e.FrameworkName));
                PackageSpecOperationsUtility.AddProjectReference(rootPackageSpec, intermediateProjectReferenceSpec, targetFramework);
                PackageSpecOperationsUtility.AddProjectReference(intermediateProjectReferenceSpec, leafProjectReferenceSpec, targetFramework);

                // Preconditions.
                var result = await new RestoreCommand(ProjectTestHelpers.CreateRestoreRequest(pathContext, logger, rootPackageSpec, intermediateProjectReferenceSpec, leafProjectReferenceSpec)).ExecuteAsync();
                await result.CommitAsync(logger, CancellationToken.None);
                result.Success.Should().BeTrue();

                // Enable locked mode, remove a dependency and clear the logger;
                rootPackageSpec.RestoreMetadata.RestoreLockProperties = new RestoreLockProperties(
                    restorePackagesWithLockFile: "true",
                    rootPackageSpec.RestoreMetadata.RestoreLockProperties.NuGetLockFilePath,
                    restoreLockedMode: true);
                logger.Clear();

                PackageSpecOperations.AddOrUpdateDependency(intermediateProjectReferenceSpec, packageC.Identity, rootPackageSpec.TargetFrameworks.Select(e => e.FrameworkName));

                // Act.
                result = await new RestoreCommand(ProjectTestHelpers.CreateRestoreRequest(pathContext, logger, rootPackageSpec, intermediateProjectReferenceSpec, leafProjectReferenceSpec)).ExecuteAsync();

                // Assert.
                result.Success.Should().BeFalse();
                logger.ErrorMessages.Count.Should().Be(1);
                logger.ErrorMessages.Single().Should().Contain("NU1004");
                logger.ErrorMessages.Single().Should().Contain($"The project reference intermediateproject has changed. Current dependencies: b,c,LeafProject lock file's dependencies: b,LeafProject.");
            }
        }

        [Fact]
        public async Task RestoreCommand_PackagesLockFile_InLockedMode_WhenATransitiveProjectReferenceChangesDependencies_FailsWithNU1004()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                var logger = new TestLogger();
                var packageA = new SimpleTestPackageContext("a", "1.0.0");
                var packageB = new SimpleTestPackageContext("b", "1.0.0");
                var packageC = new SimpleTestPackageContext("c", "1.0.0");
                var packageD = new SimpleTestPackageContext("d", "1.0.0");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    packageA,
                    packageB,
                    packageC,
                    packageD);

                var targetFramework = CommonFrameworks.Net46;

                var projectName = "RootProject";
                var projectDirectory = Path.Combine(pathContext.SolutionRoot, projectName);
                var rootPackageSpec = PackageReferenceSpecBuilder.Create(projectName, projectDirectory)
                    .WithTargetFrameworks(new string[] { "net46" })
                    .WithPackagesLockFile()
                    .Build();

                projectName = "IntermediateProject";
                projectDirectory = Path.Combine(pathContext.SolutionRoot, projectName);
                var intermediateProjectReferenceSpec = PackageReferenceSpecBuilder.Create(projectName, projectDirectory)
                    .WithTargetFrameworks(new string[] { targetFramework.GetShortFolderName() })
                    .Build();

                projectName = "LeafProject";
                projectDirectory = Path.Combine(pathContext.SolutionRoot, projectName);
                var leafProjectReferenceSpec = PackageReferenceSpecBuilder.Create(projectName, projectDirectory)
                    .WithTargetFrameworks(new string[] { targetFramework.GetShortFolderName() })
                    .Build();

                // Add the dependency to all frameworks
                PackageSpecOperations.AddOrUpdateDependency(rootPackageSpec, packageA.Identity, rootPackageSpec.TargetFrameworks.Select(e => e.FrameworkName));
                PackageSpecOperations.AddOrUpdateDependency(intermediateProjectReferenceSpec, packageB.Identity, rootPackageSpec.TargetFrameworks.Select(e => e.FrameworkName));
                PackageSpecOperations.AddOrUpdateDependency(leafProjectReferenceSpec, packageC.Identity, rootPackageSpec.TargetFrameworks.Select(e => e.FrameworkName));
                PackageSpecOperationsUtility.AddProjectReference(rootPackageSpec, intermediateProjectReferenceSpec, targetFramework);
                PackageSpecOperationsUtility.AddProjectReference(intermediateProjectReferenceSpec, leafProjectReferenceSpec, targetFramework);

                // Preconditions.
                var result = await new RestoreCommand(ProjectTestHelpers.CreateRestoreRequest(pathContext, logger, rootPackageSpec, intermediateProjectReferenceSpec, leafProjectReferenceSpec)).ExecuteAsync();
                await result.CommitAsync(logger, CancellationToken.None);
                result.Success.Should().BeTrue();

                // Enable locked mode, remove a dependency and clear the logger;
                rootPackageSpec.RestoreMetadata.RestoreLockProperties = new RestoreLockProperties(
                    restorePackagesWithLockFile: "true",
                    rootPackageSpec.RestoreMetadata.RestoreLockProperties.NuGetLockFilePath,
                    restoreLockedMode: true);
                logger.Clear();

                PackageSpecOperations.AddOrUpdateDependency(leafProjectReferenceSpec, packageD.Identity, rootPackageSpec.TargetFrameworks.Select(e => e.FrameworkName));

                // Act.
                result = await new RestoreCommand(ProjectTestHelpers.CreateRestoreRequest(pathContext, logger, rootPackageSpec, intermediateProjectReferenceSpec, leafProjectReferenceSpec)).ExecuteAsync();

                // Assert.
                result.Success.Should().BeFalse();
                logger.ErrorMessages.Count.Should().Be(1);
                logger.ErrorMessages.Single().Should().Contain("NU1004");
                logger.ErrorMessages.Single().Should().Contain($"The project reference leafproject has changed. Current dependencies: c,d lock file's dependencies: c");
            }
        }

        [Fact]
        public async Task RestoreCommand_PackagesLockFile_InLockedMode_WhenADirectProjectReferenceUpdatesDependency_FailsWithNU1004()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                var logger = new TestLogger();
                var packageA100 = new SimpleTestPackageContext("a", "1.0.0");
                var packageA200 = new SimpleTestPackageContext("a", "2.0.0");
                var packageB = new SimpleTestPackageContext("b", "1.0.0");
                var packageC = new SimpleTestPackageContext("c", "1.0.0");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    packageA100,
                    packageA200,
                    packageB,
                    packageC);

                var targetFramework = CommonFrameworks.Net46;

                var projectName = "RootProject";
                var projectDirectory = Path.Combine(pathContext.SolutionRoot, projectName);
                var rootPackageSpec = PackageReferenceSpecBuilder.Create(projectName, projectDirectory)
                    .WithTargetFrameworks(new string[] { targetFramework.GetShortFolderName() })
                    .WithPackagesLockFile()
                    .Build();

                projectName = "IntermediateProject";
                projectDirectory = Path.Combine(pathContext.SolutionRoot, projectName);
                var intermediateProjectReferenceSpec = PackageReferenceSpecBuilder.Create(projectName, projectDirectory)
                    .WithTargetFrameworks(new string[] { targetFramework.GetShortFolderName() })
                    .Build();

                projectName = "LeafProject";
                projectDirectory = Path.Combine(pathContext.SolutionRoot, projectName);
                var leafProjectReferenceSpec = PackageReferenceSpecBuilder.Create(projectName, projectDirectory)
                    .WithTargetFrameworks(new string[] { targetFramework.GetShortFolderName() })
                    .Build();

                // Add the dependency to all frameworks
                PackageSpecOperations.AddOrUpdateDependency(rootPackageSpec, packageB.Identity, rootPackageSpec.TargetFrameworks.Select(e => e.FrameworkName));
                PackageSpecOperations.AddOrUpdateDependency(intermediateProjectReferenceSpec, packageA100.Identity, intermediateProjectReferenceSpec.TargetFrameworks.Select(e => e.FrameworkName));
                PackageSpecOperationsUtility.AddProjectReference(rootPackageSpec, intermediateProjectReferenceSpec, targetFramework);
                PackageSpecOperationsUtility.AddProjectReference(intermediateProjectReferenceSpec, leafProjectReferenceSpec, targetFramework);

                // Preconditions.
                var result = await new RestoreCommand(ProjectTestHelpers.CreateRestoreRequest(pathContext, logger, rootPackageSpec, intermediateProjectReferenceSpec, leafProjectReferenceSpec)).ExecuteAsync();
                await result.CommitAsync(logger, CancellationToken.None);
                result.Success.Should().BeTrue();

                // Enable locked mode, remove a dependency and clear the logger;
                rootPackageSpec.RestoreMetadata.RestoreLockProperties = new RestoreLockProperties(
                    restorePackagesWithLockFile: "true",
                    rootPackageSpec.RestoreMetadata.RestoreLockProperties.NuGetLockFilePath,
                    restoreLockedMode: true);
                logger.Clear();

                PackageSpecOperations.AddOrUpdateDependency(intermediateProjectReferenceSpec, packageA200.Identity, rootPackageSpec.TargetFrameworks.Select(e => e.FrameworkName));

                // Act.
                result = await new RestoreCommand(ProjectTestHelpers.CreateRestoreRequest(pathContext, logger, rootPackageSpec, intermediateProjectReferenceSpec, leafProjectReferenceSpec)).ExecuteAsync();

                // Assert.
                result.Success.Should().BeFalse();
                logger.ErrorMessages.Count.Should().Be(1);
                logger.ErrorMessages.Single().Should().Contain("NU1004");
                logger.ErrorMessages.Single().Should().Contain($"The project references intermediateproject whose dependencies has changed.");
            }
        }

        [Fact]
        public async Task RestoreCommand_PackagesLockFile_InLockedMode_WhenADirectProjectReferenceAddsNewProjectReference_FailsWithNU1004()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                var logger = new TestLogger();
                var packageA100 = new SimpleTestPackageContext("a", "1.0.0");
                var packageB = new SimpleTestPackageContext("b", "1.0.0");
                var packageC = new SimpleTestPackageContext("c", "1.0.0");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    packageA100,
                    packageB,
                    packageC);

                var targetFramework = CommonFrameworks.Net46;

                var projectName = "RootProject";
                var projectDirectory = Path.Combine(pathContext.SolutionRoot, projectName);
                var rootPackageSpec = PackageReferenceSpecBuilder.Create(projectName, projectDirectory)
                    .WithTargetFrameworks(new string[] { targetFramework.GetShortFolderName() })
                    .WithPackagesLockFile()
                    .Build();

                projectName = "IntermediateProject";
                projectDirectory = Path.Combine(pathContext.SolutionRoot, projectName);
                var intermediateProjectReferenceSpec = PackageReferenceSpecBuilder.Create(projectName, projectDirectory)
                    .WithTargetFrameworks(new string[] { targetFramework.GetShortFolderName() })
                    .Build();

                projectName = "LeafProject";
                projectDirectory = Path.Combine(pathContext.SolutionRoot, projectName);
                var leafProjectReferenceSpec = PackageReferenceSpecBuilder.Create(projectName, projectDirectory)
                    .WithTargetFrameworks(new string[] { targetFramework.GetShortFolderName() })
                    .Build();

                // Add the dependency to all frameworks
                PackageSpecOperations.AddOrUpdateDependency(rootPackageSpec, packageB.Identity, rootPackageSpec.TargetFrameworks.Select(e => e.FrameworkName));
                PackageSpecOperations.AddOrUpdateDependency(intermediateProjectReferenceSpec, packageA100.Identity, intermediateProjectReferenceSpec.TargetFrameworks.Select(e => e.FrameworkName));
                PackageSpecOperationsUtility.AddProjectReference(rootPackageSpec, intermediateProjectReferenceSpec, targetFramework);

                // Preconditions.
                var result = await new RestoreCommand(ProjectTestHelpers.CreateRestoreRequest(pathContext, logger, rootPackageSpec, intermediateProjectReferenceSpec, leafProjectReferenceSpec)).ExecuteAsync();
                await result.CommitAsync(logger, CancellationToken.None);
                result.Success.Should().BeTrue();

                // Enable locked mode, remove a dependency and clear the logger;
                rootPackageSpec.RestoreMetadata.RestoreLockProperties = new RestoreLockProperties(
                    restorePackagesWithLockFile: "true",
                    rootPackageSpec.RestoreMetadata.RestoreLockProperties.NuGetLockFilePath,
                    restoreLockedMode: true);
                logger.Clear();

                PackageSpecOperations.RemoveDependency(intermediateProjectReferenceSpec, packageA100.Identity.Id);
                PackageSpecOperationsUtility.AddProjectReference(intermediateProjectReferenceSpec, leafProjectReferenceSpec, targetFramework);
                // Act.
                result = await new RestoreCommand(ProjectTestHelpers.CreateRestoreRequest(pathContext, logger, rootPackageSpec, intermediateProjectReferenceSpec, leafProjectReferenceSpec)).ExecuteAsync();

                // Assert.
                result.Success.Should().BeFalse();
                logger.ErrorMessages.Count.Should().Be(1);
                logger.ErrorMessages.Single().Should().Contain("NU1004");
                logger.ErrorMessages.Single().Should().Contain($"The project references intermediateproject whose dependencies has changed.");
            }
        }
    }
}
