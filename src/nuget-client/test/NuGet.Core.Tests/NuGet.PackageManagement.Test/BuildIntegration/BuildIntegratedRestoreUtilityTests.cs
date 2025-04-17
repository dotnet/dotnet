// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NuGet.Commands;
using NuGet.Commands.Test;
using NuGet.Configuration;
using NuGet.PackageManagement;
using NuGet.ProjectManagement;
using NuGet.ProjectModel;
using NuGet.Protocol.Core.Types;
using NuGet.Protocol.VisualStudio;
using NuGet.Test.Utility;
using Test.Utility;
using Xunit;

namespace NuGet.Test
{
    public class BuildIntegratedRestoreUtilityTests
    {
        [Fact]
        public async Task BuildIntegratedRestoreUtility_BasicRestoreTest()
        {
            // Arrange
            var projectName = "testproj";

            using (var pathContext = new SimpleTestPathContext())
            {
                var packageSpec = ProjectTestHelpers.GetPackageSpec(Settings.LoadDefaultSettings(pathContext.SolutionRoot), projectName);

                var sources = new List<SourceRepository>
                {
                    Repository.Factory.GetVisualStudio(pathContext.PackageSource)
                };

                var msBuildNuGetProjectSystem = new TestMSBuildNuGetProjectSystem(packageSpec.TargetFrameworks[0].FrameworkName,
                    new TestNuGetProjectContext());
                var project = new TestPackageReferenceNuGetProject(packageSpec, msBuildNuGetProjectSystem);

                using (var solutionManager = new TestSolutionManager(pathContext))
                {
                    solutionManager.NuGetProjects.Add(project);

                    var testLogger = new TestLogger();

                    var restoreContext = new DependencyGraphCacheContext(testLogger, NullSettings.Instance);

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
                        testLogger,
                        CancellationToken.None);

                    // Assert
                    Assert.True(File.Exists(Path.Combine(packageSpec.RestoreMetadata.OutputPath, LockFileFormat.AssetsFileName)));
                    Assert.True(testLogger.Errors == 0);
                }
            }
        }

        [Fact]
        public async Task BuildIntegratedRestoreUtility_RestoreToRelativePathGlobalPackagesFolder()
        {
            // Arrange
            var projectName = "testproj";

            using (var pathContext = new SimpleTestPathContext())
            using (var solutionManager = new TestSolutionManager(pathContext))
            {
                SimpleTestSettingsContext.AddSetting(pathContext.Settings.XML, ConfigurationConstants.GlobalPackagesFolder, @"..\NuGetPackages");
                pathContext.Settings.Save();

                var settings = Settings.LoadDefaultSettings(pathContext.SolutionRoot);
                var packageSpec = ProjectTestHelpers.GetPackageSpec(settings, projectName, pathContext.SolutionRoot, "net5.0", "a");
                var sources = new List<SourceRepository>
                {
                    Repository.Factory.GetVisualStudio(pathContext.PackageSource)
                };

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(pathContext.PackageSource, new SimpleTestPackageContext("a", "1.0.0"));

                var project = new TestPackageReferenceNuGetProject(packageSpec, new TestMSBuildNuGetProjectSystem(packageSpec.TargetFrameworks[0].FrameworkName,
                    new TestNuGetProjectContext()));

                solutionManager.NuGetProjects.Add(project);
                var testLogger = new TestLogger();
                var restoreContext = new DependencyGraphCacheContext(testLogger, settings);

                // Act
                IReadOnlyList<RestoreSummary> results = await DependencyGraphRestoreUtility.RestoreAsync(
                    solutionManager,
                    await DependencyGraphRestoreUtility.GetSolutionRestoreSpec(solutionManager, restoreContext),
                    restoreContext,
                    new RestoreCommandProvidersCache(),
                    (c) => { },
                    sources,
                    Guid.Empty,
                    false,
                    true,
                    testLogger,
                    CancellationToken.None);

                // Assert
                Assert.True(File.Exists(Path.Combine(packageSpec.RestoreMetadata.OutputPath, LockFileFormat.AssetsFileName)));
                results.Should().HaveCount(1);

                results[0].Success.Should().BeTrue(testLogger.ShowMessages());

                var packagesFolder = Path.GetFullPath(Path.Combine(pathContext.WorkingDirectory, @"..\NuGetPackages"));

                Assert.True(Directory.Exists(packagesFolder), testLogger.ShowMessages());
                Assert.True(File.Exists(Path.Combine(
                    packagesFolder,
                    "a",
                    "1.0.0",
                    "a.1.0.0.nupkg")));

                Assert.True(testLogger.Errors == 0, testLogger.ShowErrors());
            }
        }
    }
}
