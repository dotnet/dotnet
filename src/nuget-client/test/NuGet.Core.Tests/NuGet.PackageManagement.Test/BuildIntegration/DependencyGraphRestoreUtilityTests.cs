// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NuGet.Commands;
using NuGet.Commands.Test;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.ProjectManagement;
using NuGet.ProjectModel;
using NuGet.Protocol.Core.Types;
using NuGet.Protocol.VisualStudio;
using NuGet.Shared;
using NuGet.Test.Utility;
using Test.Utility;
using Test.Utility.ProjectManagement;
using Xunit;

namespace NuGet.PackageManagement.Test
{
    public class DependencyGraphRestoreUtilityTests
    {
        [Fact]
        public async Task DependencyGraphRestoreUtility_NoopRestoreTest()
        {
            // Arrange
            var projectName = "testproj";
            var logger = new TestLogger();

            using (var rootFolder = TestDirectory.Create())
            {
                var projectFolder = new DirectoryInfo(Path.Combine(rootFolder, projectName));
                projectFolder.Create();
                var msbuildProjectPath = new FileInfo(Path.Combine(projectFolder.FullName, $"{projectName}.csproj"));

                var sources = new[]
                {
                    Repository.Factory.GetVisualStudio(new PackageSource("https://www.nuget.org/api/v2/"))
                };

                var targetFramework = NuGetFramework.Parse("net46");

                var msBuildNuGetProjectSystem = new TestMSBuildNuGetProjectSystem(targetFramework, new TestNuGetProjectContext());
                var project = new TestMSBuildNuGetProject(msBuildNuGetProjectSystem, rootFolder, projectFolder.FullName);

                var effectiveGlobalPackagesFolder = SettingsUtility.GetGlobalPackagesFolder(NullSettings.Instance);

                var restoreContext = new DependencyGraphCacheContext(logger, NullSettings.Instance);

                var projects = new List<IDependencyGraphProject>() { project };

                using (var solutionManager = new TestSolutionManager())
                {
                    solutionManager.NuGetProjects.Add(project);

                    // Act
                    await DependencyGraphRestoreUtility.RestoreAsync(
                        solutionManager,
                        await DependencyGraphRestoreUtility.GetSolutionRestoreSpec(solutionManager, restoreContext),
                        restoreContext,
                        new RestoreCommandProvidersCache(),
                        (c) => { },
                        sources,
                        Guid.Empty,
                        false,
                        true,
                        logger,
                        CancellationToken.None);

                    // Assert
                    Assert.Equal(0, logger.Errors);
                    Assert.Equal(0, logger.Warnings);
                }
            }
        }

        [Fact]
        public async Task RestoreAsync_WithMinimalProjectAndAdditionalErrorMessage_WritesErrorsToAssetsFile()
        {
            // Arrange
            var projectName = "testproj";
            var logger = new TestLogger();

            using (var rootFolder = TestDirectory.Create())
            {
                var projectFolder = new DirectoryInfo(Path.Combine(rootFolder, projectName));
                projectFolder.Create();
                var objFolder = projectFolder.CreateSubdirectory("obj");
                var msbuildProjectPath = new FileInfo(Path.Combine(projectFolder.FullName, $"{projectName}.csproj"));
                var globalPackagesFolder = Path.Combine(rootFolder, "gpf");

                var sources = new SourceRepository[0];
                var restoreContext = new DependencyGraphCacheContext(logger, NullSettings.Instance);
                var solutionManager = new Mock<ISolutionManager>();
                var restoreCommandProvidersCache = new RestoreCommandProvidersCache();

                // When a VS nomination results in an exception, we use this minimal DGSpec to do a restore.
                var dgSpec = DependencyGraphSpecTestUtilities.CreateMinimalDependencyGraphSpec(msbuildProjectPath.FullName, objFolder.FullName);
                dgSpec.AddRestore(dgSpec.Projects[0].FilePath);
                // CpsPackageReferenceProject sets some additional properties, from settings, in GetPackageSpecsAndAdditionalMessages(...)
                dgSpec.Projects[0].RestoreMetadata.PackagesPath = globalPackagesFolder;

                // Having an "additional" error message is also critical
                var restoreLogMessage = new RestoreLogMessage(LogLevel.Error, NuGetLogCode.NU1000, "Test error")
                {
                    FilePath = msbuildProjectPath.FullName,
                    ProjectPath = msbuildProjectPath.FullName
                };
                var additionalMessages = new List<IAssetsLogMessage>()
                {
                    AssetsLogMessage.Create(restoreLogMessage)
                };

                // Act
                await DependencyGraphRestoreUtility.RestoreAsync(
                    dgSpec,
                    restoreContext,
                    restoreCommandProvidersCache,
                    cacheContextModifier: _ => { },
                    sources,
                    parentId: Guid.Empty,
                    forceRestore: false,
                    isRestoreOriginalAction: true,
                    additionalMessages,
                    progressReporter: null,
                    logger,
                    CancellationToken.None);

                // Assert
                var assetsFilePath = Path.Combine(objFolder.FullName, "project.assets.json");
                Assert.True(File.Exists(assetsFilePath), "Assets file does not exist");
                LockFile assetsFile = new LockFileFormat().Read(assetsFilePath);
                IAssetsLogMessage actualMessage = Assert.Single(assetsFile.LogMessages);
                Assert.Equal(restoreLogMessage.Level, actualMessage.Level);
                Assert.Equal(restoreLogMessage.Code, actualMessage.Code);
                Assert.Equal(restoreLogMessage.Message, actualMessage.Message);
            }
        }

        [Fact]
        public async Task RestoreAsync_WithProgressReporter_WhenFilesAreWriten_ProgressIsReported()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();
            var settings = Settings.LoadDefaultSettings(pathContext.SolutionRoot);
            var packageSpec = ProjectTestHelpers.GetPackageSpec(settings, projectName: "projectName", rootPath: pathContext.SolutionRoot);
            var progressReporter = new Mock<IRestoreProgressReporter>();

            // Act
            IReadOnlyList<RestoreSummary> result = await DependencyGraphRestoreUtility.RestoreAsync(
                ProjectTestHelpers.GetDGSpecFromPackageSpecs(packageSpec),
                new DependencyGraphCacheContext(),
                new RestoreCommandProvidersCache(),
                cacheContextModifier: _ => { },
                sources: new SourceRepository[0],
                parentId: Guid.Empty,
                forceRestore: false,
                isRestoreOriginalAction: true,
                additionalMessages: null,
                progressReporter: progressReporter.Object,
                new TestLogger(),
                CancellationToken.None);

            // Assert
            result.Should().HaveCount(1);
            RestoreSummary restoreSummary = result[0];
            restoreSummary.Success.Should().BeTrue();
            restoreSummary.NoOpRestore.Should().BeFalse();
            var assetsFilePath = Path.Combine(packageSpec.RestoreMetadata.OutputPath, LockFileFormat.AssetsFileName);
            File.Exists(assetsFilePath).Should().BeTrue(because: $"{assetsFilePath}");

            var propsFile = BuildAssetsUtils.GetMSBuildFilePath(packageSpec, BuildAssetsUtils.PropsExtension);
            var targetsFile = BuildAssetsUtils.GetMSBuildFilePath(packageSpec, BuildAssetsUtils.TargetsExtension);

            IReadOnlyList<string> expectedFileList = new string[] { assetsFilePath, propsFile, targetsFile };
            var pathComparer = PathUtility.GetStringComparerBasedOnOS();

            progressReporter.Verify(r =>
                r.StartProjectUpdate(
                    It.Is<string>(e => e.Equals(packageSpec.FilePath)),
                    It.Is<IReadOnlyList<string>>(e => e.OrderedEquals(expectedFileList, (f) => f, pathComparer, pathComparer))),
                    Times.Once);
            progressReporter.Verify(r =>
                r.EndProjectUpdate(
                    It.Is<string>(e => e.Equals(packageSpec.FilePath)),
                    It.Is<IReadOnlyList<string>>(e => e.OrderedEquals(expectedFileList, (f) => f, pathComparer, pathComparer))),
                    Times.Once);
        }

        [Fact]
        public async Task RestoreAsync_WithProgressReporter_WhenProjectNoOps_ProgressIsNotReported()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();
            var settings = Settings.LoadDefaultSettings(pathContext.SolutionRoot);
            var packageSpec = ProjectTestHelpers.GetPackageSpec(settings, projectName: "projectName", rootPath: pathContext.SolutionRoot);
            var dgSpec = ProjectTestHelpers.GetDGSpecFromPackageSpecs(packageSpec);
            var progressReporter = new Mock<IRestoreProgressReporter>();

            // Pre-Conditions
            IReadOnlyList<RestoreSummary> result = await DependencyGraphRestoreUtility.RestoreAsync(
                dgSpec,
                new DependencyGraphCacheContext(),
                new RestoreCommandProvidersCache(),
                cacheContextModifier: _ => { },
                sources: new SourceRepository[0],
                parentId: Guid.Empty,
                forceRestore: false,
                isRestoreOriginalAction: true,
                additionalMessages: null,
                progressReporter: progressReporter.Object,
                new TestLogger(),
                CancellationToken.None);

            // Pre-Conditions
            result.Should().HaveCount(1);
            RestoreSummary restoreSummary = result[0];
            restoreSummary.Success.Should().BeTrue();
            restoreSummary.NoOpRestore.Should().BeFalse();

            progressReporter.Verify(r =>
                r.StartProjectUpdate(
                    It.IsAny<string>(),
                    It.IsAny<IReadOnlyList<string>>()),
                    Times.Once);
            progressReporter.Verify(r =>
                r.EndProjectUpdate(
                    It.IsAny<string>(),
                    It.IsAny<IReadOnlyList<string>>()),
                    Times.Once);

            var noopProgressReporter = new Mock<IRestoreProgressReporter>();

            // Act
            result = await DependencyGraphRestoreUtility.RestoreAsync(
               dgSpec,
               new DependencyGraphCacheContext(),
               new RestoreCommandProvidersCache(),
               cacheContextModifier: _ => { },
               sources: new SourceRepository[0],
               parentId: Guid.Empty,
               forceRestore: false,
               isRestoreOriginalAction: true,
               additionalMessages: null,
               progressReporter: noopProgressReporter.Object,
               new TestLogger(),
               CancellationToken.None);

            // Assert
            result.Should().HaveCount(1);
            restoreSummary = result[0];
            restoreSummary.Success.Should().BeTrue();
            restoreSummary.NoOpRestore.Should().BeTrue();

            noopProgressReporter.Verify(r =>
                r.StartProjectUpdate(
                    It.IsAny<string>(),
                    It.IsAny<IReadOnlyList<string>>()),
                    Times.Never);
            noopProgressReporter.Verify(r =>
                r.EndProjectUpdate(
                    It.IsAny<string>(),
                    It.IsAny<IReadOnlyList<string>>()),
                    Times.Never);
        }

        [Fact]
        public async Task RestoreAsync_WithProgressReporter_WithMultipleProjects_ProgressIsNotReportedForChangedProjetsOnly()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();
            var settings = Settings.LoadDefaultSettings(pathContext.SolutionRoot);
            var project1 = ProjectTestHelpers.GetPackageSpec(settings, projectName: "project1", rootPath: pathContext.SolutionRoot);
            var project2 = ProjectTestHelpers.GetPackageSpec(settings, projectName: "project2", rootPath: pathContext.SolutionRoot);
            var progressReporter = new Mock<IRestoreProgressReporter>();

            // Pre-Conditions
            IReadOnlyList<RestoreSummary> result = await DependencyGraphRestoreUtility.RestoreAsync(
                ProjectTestHelpers.GetDGSpecFromPackageSpecs(project1, project2),
                new DependencyGraphCacheContext(),
                new RestoreCommandProvidersCache(),
                cacheContextModifier: _ => { },
                sources: new SourceRepository[0],
                parentId: Guid.Empty,
                forceRestore: false,
                isRestoreOriginalAction: true,
                additionalMessages: null,
                progressReporter: progressReporter.Object,
                new TestLogger(),
                CancellationToken.None);

            // Pre-Conditions
            result.Should().HaveCount(2);
            foreach (RestoreSummary restoreSummary in result)
            {
                restoreSummary.Success.Should().BeTrue();
                restoreSummary.NoOpRestore.Should().BeFalse();
            }

            progressReporter.Verify(r =>
                r.StartProjectUpdate(
                    It.Is<string>(e => e.Equals(project1.FilePath)),
                    It.IsAny<IReadOnlyList<string>>()),
                    Times.Once);
            progressReporter.Verify(r =>
                r.EndProjectUpdate(
                    It.Is<string>(e => e.Equals(project1.FilePath)),
                    It.IsAny<IReadOnlyList<string>>()),
                    Times.Once);

            progressReporter.Verify(r =>
              r.StartProjectUpdate(
                  It.Is<string>(e => e.Equals(project2.FilePath)),
                  It.IsAny<IReadOnlyList<string>>()),
                  Times.Once);
            progressReporter.Verify(r =>
                r.EndProjectUpdate(
                    It.Is<string>(e => e.Equals(project2.FilePath)),
                    It.IsAny<IReadOnlyList<string>>()),
                    Times.Once);

            progressReporter.Verify(r =>
                r.StartProjectUpdate(
                    It.IsAny<string>(),
                    It.IsAny<IReadOnlyList<string>>()),
                    Times.Exactly(2));
            progressReporter.Verify(r =>
                r.EndProjectUpdate(
                    It.IsAny<string>(),
                    It.IsAny<IReadOnlyList<string>>()),
                    Times.Exactly(2));

            var secondaryProgressReporter = new Mock<IRestoreProgressReporter>();

            // Act
            result = await DependencyGraphRestoreUtility.RestoreAsync(
               ProjectTestHelpers.GetDGSpecFromPackageSpecs(project1.WithTestProjectReference(project2), project2),
               new DependencyGraphCacheContext(),
               new RestoreCommandProvidersCache(),
               cacheContextModifier: _ => { },
               sources: new SourceRepository[0],
               parentId: Guid.Empty,
               forceRestore: false,
               isRestoreOriginalAction: true,
               additionalMessages: null,
               progressReporter: secondaryProgressReporter.Object,
               new TestLogger(),
               CancellationToken.None);

            // Assert
            result.Should().HaveCount(2);
            var project1Summary = result.Single(e => e.InputPath.Equals(project1.FilePath));
            var project2Summary = result.Single(e => e.InputPath.Equals(project2.FilePath));

            project1Summary.Success.Should().BeTrue();
            project1Summary.NoOpRestore.Should().BeFalse();

            // One no-op
            project2Summary.Success.Should().BeTrue();
            project2Summary.NoOpRestore.Should().BeTrue();

            progressReporter.Verify(r =>
                r.StartProjectUpdate(
                    It.Is<string>(e => e.Equals(project1.FilePath)),
                    It.IsAny<IReadOnlyList<string>>()),
                    Times.Once);
            progressReporter.Verify(r =>
                r.EndProjectUpdate(
                    It.Is<string>(e => e.Equals(project1.FilePath)),
                    It.IsAny<IReadOnlyList<string>>()),
                    Times.Once);
        }
    }
}
