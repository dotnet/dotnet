// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
#if NETFRAMEWORK

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NuGet.Commands.Test;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.Packaging.Core;
using NuGet.ProjectManagement;
using NuGet.ProjectModel;
using NuGet.Protocol.Core.Types;
using NuGet.Resolver;
using NuGet.Test.Utility;
using NuGet.Versioning;
using NuGet.VisualStudio;
using Test.Utility;
using Xunit;

namespace NuGet.PackageManagement.Test
{
    public class PackageReferenceBasedNuGetPackageManagerTests
    {
        [Fact]
        public async Task PreviewInstallPackageAsync_WithMultiTargettedPackageReferenceProjectAndConditionalPackages_UpdatesOnlyConditionalPackages()
        {
            // Arrange
            using SimpleTestPathContext pathContext = new();
            using var solutionManager = new TestSolutionManager(pathContext);
            SourceRepositoryProvider sourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(new PackageSource(pathContext.PackageSource));
            var settings = Settings.LoadDefaultSettings(solutionManager.SolutionDirectory);
            var nuGetPackageManager = new NuGetPackageManager(sourceRepositoryProvider, settings, solutionManager, new TestDeleteOnRestartManager());

            string referenceSpec = @"
                {
                    ""frameworks"": {
                        ""net472"": {
                            ""dependencies"": {
                            }
                        },
                        ""net5.0"": {
                            ""dependencies"": {
                            }
                        }
                    }
                }";
            NuGetProject buildIntegratedProject = CreateBuildIntegratedProjectAndAddToSolutionManager(solutionManager, settings, referenceSpec);

            var packageID = "A";
            var packageToInstall = new PackageIdentity(packageID, new NuGetVersion(1, 0, 0));
            await SimpleTestPackageUtility.CreateFolderFeedV3Async(pathContext.PackageSource,
                packageToInstall);

            // Main Act
            var result = (await nuGetPackageManager.PreviewInstallPackageAsync(
                buildIntegratedProject,
                packageToInstall,
                new ResolutionContext(DependencyBehavior.Lowest, false, false, VersionConstraints.None),
                new TestNuGetProjectContext(),
                sourceRepositoryProvider.GetRepositories(),
                sourceRepositoryProvider.GetRepositories(),
                CancellationToken.None)).ToList();

            // Assert
            result.Should().HaveCount(1);
            result[0].NuGetProjectActionType.Should().Be(NuGetProjectActionType.Install);
            result[0].PackageIdentity.Should().Be(packageToInstall);
            var buildIntegratedAction = (BuildIntegratedProjectAction)result[0];
            buildIntegratedAction.RestoreResult.Success.Should().BeTrue(because: string.Join(",", buildIntegratedAction.RestoreResult.LogMessages.Select(e => e.Message)));

            buildIntegratedAction.OriginalLockFile.Libraries.Should().HaveCount(0);

            var assetsFile = buildIntegratedAction.RestoreResult.LockFile;
            assetsFile.Libraries.Should().HaveCount(1);
            assetsFile.Libraries[0].Name.Should().Be(packageToInstall.Id);
            assetsFile.Libraries[0].Version.Should().Be(packageToInstall.Version);
            assetsFile.Targets.Should().HaveCount(2);
            var net472Target = assetsFile.Targets.Single(e => e.TargetFramework.Equals(NuGetFramework.Parse("net472")));
            var net50Target = assetsFile.Targets.Single(e => e.TargetFramework.Equals(NuGetFramework.Parse("net50")));
            net472Target.Libraries.Should().HaveCount(1);
            net472Target.Libraries[0].Name.Should().Be(packageToInstall.Id);
            net472Target.Libraries[0].Version.Should().Be(packageToInstall.Version);
            net50Target.Libraries.Should().HaveCount(1);
            net50Target.Libraries[0].Name.Should().Be(packageToInstall.Id);
            net50Target.Libraries[0].Version.Should().Be(packageToInstall.Version);
        }

        [Fact]
        public async Task PreviewUpdatePackagesAsync_WithMultiTargettedPackageReferenceProjectAndConditionalPackages_UpdatesOnlyConditionalPackages()
        {
            // Arrange
            using SimpleTestPathContext pathContext = new();
            using var solutionManager = new TestSolutionManager(pathContext);
            SourceRepositoryProvider sourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(new PackageSource(pathContext.PackageSource));
            var settings = Settings.LoadDefaultSettings(solutionManager.SolutionDirectory);
            var nuGetPackageManager = new NuGetPackageManager(sourceRepositoryProvider, settings, solutionManager, new TestDeleteOnRestartManager());

            string referenceSpec = @"
                {
                    ""frameworks"": {
                        ""net472"": {
                            ""dependencies"": {
                                ""A"" : ""1.0.0""
                            }
                        },
                        ""net5.0"": {
                            ""dependencies"": {
                            }
                        }
                    }
                }";
            NuGetProject buildIntegratedProject = CreateBuildIntegratedProjectAndAddToSolutionManager(solutionManager, settings, referenceSpec);

            var packageID = "A";
            var before = new PackageIdentity(packageID, new NuGetVersion(1, 0, 0));
            var after = new PackageIdentity(packageID, new NuGetVersion(2, 0, 0));
            await SimpleTestPackageUtility.CreateFolderFeedV3Async(pathContext.PackageSource,
                before,
                after);

            // Main Act
            var result = (await nuGetPackageManager.PreviewUpdatePackagesAsync(
                packageID,
                new List<NuGetProject> { buildIntegratedProject },
                new ResolutionContext(DependencyBehavior.Lowest, false, false, VersionConstraints.None),
                new TestNuGetProjectContext(),
                sourceRepositoryProvider.GetRepositories(),
                sourceRepositoryProvider.GetRepositories(),
                CancellationToken.None)).ToList();

            // Assert
            result.Should().HaveCount(1);
            result[0].NuGetProjectActionType.Should().Be(NuGetProjectActionType.Install);
            result[0].PackageIdentity.Should().Be(after);
            var buildIntegratedAction = (BuildIntegratedProjectAction)result[0];
            buildIntegratedAction.RestoreResult.Success.Should().BeTrue(because: string.Join(",", buildIntegratedAction.RestoreResult.LogMessages.Select(e => e.Message)));

            buildIntegratedAction.OriginalLockFile.Libraries[0].Name.Should().Be(before.Id);
            buildIntegratedAction.OriginalLockFile.Libraries[0].Version.Should().Be(before.Version);

            var assetsFile = buildIntegratedAction.RestoreResult.LockFile;
            assetsFile.Libraries.Should().HaveCount(1);
            assetsFile.Libraries[0].Name.Should().Be(after.Id);
            assetsFile.Libraries[0].Version.Should().Be(after.Version);
            assetsFile.Targets.Should().HaveCount(2);
            var net472Target = assetsFile.Targets.Single(e => e.TargetFramework.Equals(NuGetFramework.Parse("net472")));
            var net50Target = assetsFile.Targets.Single(e => e.TargetFramework.Equals(NuGetFramework.Parse("net50")));
            net472Target.Libraries.Should().HaveCount(1);
            net472Target.Libraries[0].Name.Should().Be(after.Id);
            net472Target.Libraries[0].Version.Should().Be(after.Version);
            net50Target.Libraries.Should().HaveCount(0);
        }

        [Fact]
        public async Task PreviewUpdatePackagesAsync_WithMultiTargettedPackageReferenceProject_UpdatesAllPackages()
        {
            // Arrange
            using SimpleTestPathContext pathContext = new();
            using var solutionManager = new TestSolutionManager(pathContext);
            SourceRepositoryProvider sourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(new PackageSource(pathContext.PackageSource));
            var settings = Settings.LoadDefaultSettings(solutionManager.SolutionDirectory);
            var nuGetPackageManager = new NuGetPackageManager(sourceRepositoryProvider, settings, solutionManager, new TestDeleteOnRestartManager());

            string referenceSpec = @"
                {
                    ""frameworks"": {
                        ""net472"": {
                            ""dependencies"": {
                                ""A"" : ""1.0.0""
                            }
                        },
                        ""net5.0"": {
                            ""dependencies"": {
                                ""A"" : ""1.0.0""
                            }
                        }
                    }
                }";
            NuGetProject buildIntegratedProject = CreateBuildIntegratedProjectAndAddToSolutionManager(solutionManager, settings, referenceSpec);

            var packageID = "A";
            var before = new PackageIdentity(packageID, new NuGetVersion(1, 0, 0));
            var after = new PackageIdentity(packageID, new NuGetVersion(2, 0, 0));
            await SimpleTestPackageUtility.CreateFolderFeedV3Async(pathContext.PackageSource,
                before,
                after);

            // Main Act
            var result = (await nuGetPackageManager.PreviewUpdatePackagesAsync(
                packageID,
                new List<NuGetProject> { buildIntegratedProject },
                new ResolutionContext(DependencyBehavior.Lowest, false, false, VersionConstraints.None),
                new TestNuGetProjectContext(),
                sourceRepositoryProvider.GetRepositories(),
                sourceRepositoryProvider.GetRepositories(),
                CancellationToken.None)).ToList();

            // Assert
            result.Should().HaveCount(1);
            result[0].NuGetProjectActionType.Should().Be(NuGetProjectActionType.Install);
            result[0].PackageIdentity.Should().Be(after);
            var buildIntegratedAction = (BuildIntegratedProjectAction)result[0];
            buildIntegratedAction.RestoreResult.Success.Should().BeTrue(because: string.Join(",", buildIntegratedAction.RestoreResult.LogMessages.Select(e => e.Message)));

            buildIntegratedAction.OriginalLockFile.Libraries[0].Name.Should().Be(before.Id);
            buildIntegratedAction.OriginalLockFile.Libraries[0].Version.Should().Be(before.Version);

            var assetsFile = buildIntegratedAction.RestoreResult.LockFile;
            assetsFile.Libraries.Should().HaveCount(1);
            assetsFile.Libraries[0].Name.Should().Be(after.Id);
            assetsFile.Libraries[0].Version.Should().Be(after.Version);
            assetsFile.Targets.Should().HaveCount(2);
            var net472Target = assetsFile.Targets.Single(e => e.TargetFramework.Equals(NuGetFramework.Parse("net472")));
            var net50Target = assetsFile.Targets.Single(e => e.TargetFramework.Equals(NuGetFramework.Parse("net50")));
            net472Target.Libraries.Should().HaveCount(1);
            net472Target.Libraries[0].Name.Should().Be(after.Id);
            net472Target.Libraries[0].Version.Should().Be(after.Version);
            net50Target.Libraries.Should().HaveCount(1);
            net50Target.Libraries[0].Name.Should().Be(after.Id);
            net50Target.Libraries[0].Version.Should().Be(after.Version);
        }

        private static NuGetProject CreateBuildIntegratedProjectAndAddToSolutionManager(TestSolutionManager solutionManager, ISettings settings, string referenceSpec)
        {
            var packageSpec = ProjectTestHelpers.GetPackageSpecWithProjectNameAndSpec("project", solutionManager.SolutionDirectory, referenceSpec).WithSettingsBasedRestoreMetadata(settings);
            var dependencyGraphSpec = ProjectTestHelpers.GetDGSpecForAllProjects(packageSpec);
            var mockProjectCache = new Mock<IProjectSystemCache>();
            mockProjectCache.Setup(pc => pc.AddProject(It.IsAny<ProjectNames>(), It.IsAny<IVsProjectAdapter>(), It.IsAny<NuGetProject>())).Returns(true);
            mockProjectCache.Setup(pc => pc.TryGetProjectRestoreInfo(It.IsAny<string>(), out dependencyGraphSpec, out It.Ref<IReadOnlyList<IAssetsLogMessage>>.IsAny)).Returns(true);
            var buildIntegratedProject = solutionManager.AddCPSPackageReferenceBasedProject(mockProjectCache.Object, packageSpec);
            return buildIntegratedProject;
        }
    }
}
#endif
