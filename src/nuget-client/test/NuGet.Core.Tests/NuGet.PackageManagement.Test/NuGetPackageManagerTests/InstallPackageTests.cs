// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Moq;
using NuGet.Commands;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.ProjectManagement;
using NuGet.Protocol.Core.Types;
using NuGet.Resolver;
using NuGet.Test;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Test.Utility;
using Xunit;
using Xunit.Abstractions;

namespace NuGet.PackageManagement.Test.NuGetPackageManagerTests
{
    public class InstallPackageTests
    {
        // Following are the various sets of packages that are small in size. To be used by the functional tests
        private readonly List<PackageIdentity> _noDependencyLibPackages = new List<PackageIdentity>
            {
                new PackageIdentity("Microsoft.AspNet.Razor", new NuGetVersion("2.0.30506")),
                new PackageIdentity("Microsoft.AspNet.Razor", new NuGetVersion("3.0.0")),
                new PackageIdentity("Microsoft.AspNet.Razor", new NuGetVersion("3.2.0-rc")),
                new PackageIdentity("Antlr", new NuGetVersion("3.5.0.2"))
            };

        private readonly List<PackageIdentity> _packageWithDependents = new List<PackageIdentity>
            {
                new PackageIdentity("jQuery", new NuGetVersion("1.4.4")),
                new PackageIdentity("jQuery", new NuGetVersion("1.6.4")),
                new PackageIdentity("jQuery.Validation", new NuGetVersion("1.13.1")),
                new PackageIdentity("jQuery.UI.Combined", new NuGetVersion("1.11.2"))
            };

        private readonly List<PackageIdentity> _packageWithDeepDependency = new List<PackageIdentity>
            {
                new PackageIdentity("Microsoft.Data.Edm", new NuGetVersion("5.6.2")),
                new PackageIdentity("Microsoft.WindowsAzure.ConfigurationManager", new NuGetVersion("1.8.0.0")),
                new PackageIdentity("Newtonsoft.Json", new NuGetVersion("5.0.8")),
                new PackageIdentity("System.Spatial", new NuGetVersion("5.6.2")),
                new PackageIdentity("Microsoft.Data.OData", new NuGetVersion("5.6.2")),
                new PackageIdentity("Microsoft.Data.Services.Client", new NuGetVersion("5.6.2")),
                new PackageIdentity("WindowsAzure.Storage", new NuGetVersion("4.3.0"))
            };

        private readonly List<PackageIdentity> _morePackageWithDependents = new List<PackageIdentity>
            {
                new PackageIdentity("Microsoft.Bcl.Build", new NuGetVersion("1.0.14")),
                new PackageIdentity("Microsoft.Bcl.Build", new NuGetVersion("1.0.21")),
                new PackageIdentity("Microsoft.Bcl", new NuGetVersion("1.1.9")),
                new PackageIdentity("Microsoft.Net.Http", new NuGetVersion("2.2.22")),
                new PackageIdentity("Microsoft.Net.Http", new NuGetVersion("2.2.28"))
            };

        private readonly List<PackageIdentity> _latestAspNetPackages = new List<PackageIdentity>
            {
                new PackageIdentity("Microsoft.AspNet.Mvc", new NuGetVersion("6.0.0-beta3")),
                new PackageIdentity("Microsoft.AspNet.Mvc.Razor", new NuGetVersion("6.0.0-beta3")),
                new PackageIdentity("Microsoft.AspNet.Mvc.Core", new NuGetVersion("6.0.0-beta3"))
            };

        private readonly XunitLogger _logger;

        public InstallPackageTests(ITestOutputHelper output)
        {
            _logger = new XunitLogger(output);
        }

        [Fact]
        public async Task InstallAndUninstallPackages_RunningOnMultipleThreads_CompletesWithoutThrowingException()
        {
            using var packageSource = TestDirectory.Create();
            // Arrange
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(
                new List<PackageSource>()
                {
                    new PackageSource(packageSource.Path)
                });

            using var testSolutionManager = new TestSolutionManager();

            var testSettings = NullSettings.Instance;
            var token = CancellationToken.None;
            var resolutionContext = new ResolutionContext();
            var testNuGetProjectContext = new TestNuGetProjectContext();
            var deleteOnRestartManager = new TestDeleteOnRestartManager();
            var nuGetPackageManager = new NuGetPackageManager(
                sourceRepositoryProvider,
                testSettings,
                testSolutionManager,
                deleteOnRestartManager);
            var packagesFolderPath = PackagesFolderPathUtility.GetPackagesFolderPath(testSolutionManager, testSettings);
            var packagePathResolver = new PackagePathResolver(packagesFolderPath);
            var projectA = testSolutionManager.AddNewMSBuildProject();

            var packageContext = new SimpleTestPackageContext("packageA");
            packageContext.AddFile("lib/net45/a.dll");
            SimpleTestPackageUtility.CreateOPCPackage(packageContext, packageSource);

            var run = true;

            var getInstalledTask = Task.Run(async () =>
            {
                // Get the list of installed packages
                while (run)
                {
                    var projectAInstalled = (await projectA.GetInstalledPackagesAsync(token)).ToList();
                }
            });

            // Act
            // Install and Uninstall 50 times while polling for installed packages
            for (var i = 0; i < 50; i++)
            {
                // Install
                await nuGetPackageManager.InstallPackageAsync(projectA, "packageA",
                    resolutionContext, testNuGetProjectContext, sourceRepositoryProvider.GetRepositories().First(), null, token);

                // Uninstall
                await nuGetPackageManager.UninstallPackageAsync(
                    projectA,
                    "packageA",
                    new UninstallationContext(removeDependencies: false, forceRemove: true),
                    testNuGetProjectContext,
                    token);
            }

            // Check for exceptions thrown by the get installed task
            run = false;
            await getInstalledTask;

            var installed = (await projectA.GetInstalledPackagesAsync(token)).ToList();

            // Assert
            // Verify no exceptions and that the final package was removed
            Assert.Equal(0, installed.Count);
        }

        [Fact]
        public async Task InstallPackage()
        {
            // Arrange
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV3OnlySourceRepositoryProvider();
            using (var testSolutionManager = new TestSolutionManager())
            {
                var testSettings = NullSettings.Instance;
                var token = CancellationToken.None;
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    testSettings,
                    testSolutionManager,
                    deleteOnRestartManager);
                var packagesFolderPath = PackagesFolderPathUtility.GetPackagesFolderPath(testSolutionManager, testSettings);

                var installationCompatibility = new Mock<IInstallationCompatibility>();
                nuGetPackageManager.InstallationCompatibility = installationCompatibility.Object;

                var msBuildNuGetProject = testSolutionManager.AddNewMSBuildProject();
                var msBuildNuGetProjectSystem = msBuildNuGetProject.ProjectSystem as TestMSBuildNuGetProjectSystem;
                var packagesConfigPath = msBuildNuGetProject.PackagesConfigNuGetProject.FullPath;
                var packageIdentity = _noDependencyLibPackages[0];

                // Pre-Assert
                // Check that the packages.config file does not exist
                Assert.False(File.Exists(packagesConfigPath));
                // Check that there are no packages returned by PackagesConfigProject
                var packagesInPackagesConfig = (await msBuildNuGetProject.PackagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(0, packagesInPackagesConfig.Count);
                Assert.Equal(0, msBuildNuGetProjectSystem.References.Count);

                // Act
                await nuGetPackageManager.InstallPackageAsync(msBuildNuGetProject, packageIdentity,
                    new ResolutionContext(), new TestNuGetProjectContext(), sourceRepositoryProvider.GetRepositories().First(), null, token);

                // Assert
                // Check that the packages.config file exists after the installation
                Assert.True(File.Exists(packagesConfigPath));

                // Check the number of packages and packages returned by PackagesConfigProject after the installation
                packagesInPackagesConfig = (await msBuildNuGetProject.PackagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(1, packagesInPackagesConfig.Count);
                Assert.Equal(packageIdentity, packagesInPackagesConfig[0].PackageIdentity);
                Assert.Equal(msBuildNuGetProject.ProjectSystem.TargetFramework, packagesInPackagesConfig[0].TargetFramework);

                // Ensure that installation compatibility was checked.
                installationCompatibility.Verify(
                    x => x.EnsurePackageCompatibilityAsync(
                        msBuildNuGetProject,
                        packageIdentity,
                        It.IsAny<DownloadResourceResult>(),
                        It.IsAny<CancellationToken>()),
                    Times.Once);
                installationCompatibility.Verify(
                    x => x.EnsurePackageCompatibility(
                        It.IsAny<NuGetProject>(),
                        It.IsAny<INuGetPathContext>(),
                        It.IsAny<IEnumerable<NuGetProjectAction>>(),
                        It.IsAny<RestoreResult>()),
                    Times.Never);
            }
        }

        [Fact]
        public async Task InstallPackageAlreadyInstalledException()
        {
            // Arrange
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV3OnlySourceRepositoryProvider();
            using (var testSolutionManager = new TestSolutionManager())
            {
                var testSettings = NullSettings.Instance;
                var token = CancellationToken.None;
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    testSettings,
                    testSolutionManager,
                    deleteOnRestartManager);
                var packagesFolderPath = PackagesFolderPathUtility.GetPackagesFolderPath(testSolutionManager, testSettings);

                var msBuildNuGetProject = testSolutionManager.AddNewMSBuildProject();
                var msBuildNuGetProjectSystem = msBuildNuGetProject.ProjectSystem as TestMSBuildNuGetProjectSystem;
                var packagesConfigPath = msBuildNuGetProject.PackagesConfigNuGetProject.FullPath;
                var packageIdentity = _noDependencyLibPackages[0];

                // Pre-Assert
                // Check that the packages.config file does not exist
                Assert.False(File.Exists(packagesConfigPath));
                // Check that there are no packages returned by PackagesConfigProject
                var packagesInPackagesConfig = (await msBuildNuGetProject.PackagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(0, packagesInPackagesConfig.Count);
                Assert.Equal(0, msBuildNuGetProjectSystem.References.Count);

                // Act
                await nuGetPackageManager.InstallPackageAsync(msBuildNuGetProject, packageIdentity,
                    new ResolutionContext(), new TestNuGetProjectContext(), sourceRepositoryProvider.GetRepositories().First(), null, token);

                // Assert
                // Check that the packages.config file exists after the installation
                Assert.True(File.Exists(packagesConfigPath));
                // Check the number of packages and packages returned by PackagesConfigProject after the installation
                packagesInPackagesConfig = (await msBuildNuGetProject.PackagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(1, packagesInPackagesConfig.Count);
                Assert.Equal(packageIdentity, packagesInPackagesConfig[0].PackageIdentity);
                Assert.Equal(msBuildNuGetProject.ProjectSystem.TargetFramework, packagesInPackagesConfig[0].TargetFramework);

                InvalidOperationException alreadyInstalledException = null;
                try
                {
                    await nuGetPackageManager.InstallPackageAsync(msBuildNuGetProject, packageIdentity,
                        new ResolutionContext(), new TestNuGetProjectContext(), sourceRepositoryProvider.GetRepositories().First(), null, token);
                }
                catch (InvalidOperationException ex)
                {
                    alreadyInstalledException = ex;
                }

                Assert.NotNull(alreadyInstalledException);
                Assert.Equal(string.Format("Package '{0}' already exists in project '{1}'", packageIdentity, msBuildNuGetProjectSystem.ProjectName),
                    alreadyInstalledException.Message);
                Assert.Equal(alreadyInstalledException.InnerException.GetType(), typeof(PackageAlreadyInstalledException));
            }
        }

        [Fact]
        public async Task InstallDifferentPackageAfterInstall()
        {
            // Arrange
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV3OnlySourceRepositoryProvider();
            using (var testSolutionManager = new TestSolutionManager())
            {
                var testSettings = NullSettings.Instance;
                var token = CancellationToken.None;
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    testSettings,
                    testSolutionManager,
                    deleteOnRestartManager);
                var packagesFolderPath = PackagesFolderPathUtility.GetPackagesFolderPath(testSolutionManager, testSettings);

                var msBuildNuGetProject = testSolutionManager.AddNewMSBuildProject();
                var msBuildNuGetProjectSystem = msBuildNuGetProject.ProjectSystem as TestMSBuildNuGetProjectSystem;
                var packagesConfigPath = msBuildNuGetProject.PackagesConfigNuGetProject.FullPath;
                var firstPackageIdentity = _noDependencyLibPackages[0];

                // Pre-Assert
                // Check that the packages.config file does not exist
                Assert.False(File.Exists(packagesConfigPath));
                // Check that there are no packages returned by PackagesConfigProject
                var packagesInPackagesConfig = (await msBuildNuGetProject.PackagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(0, packagesInPackagesConfig.Count);
                Assert.Equal(0, msBuildNuGetProjectSystem.References.Count);

                // Act
                await nuGetPackageManager.InstallPackageAsync(msBuildNuGetProject, firstPackageIdentity,
                    new ResolutionContext(), new TestNuGetProjectContext(), sourceRepositoryProvider.GetRepositories().First(), null, token);

                var secondPackageIdentity = _noDependencyLibPackages[3];
                await nuGetPackageManager.InstallPackageAsync(msBuildNuGetProject, secondPackageIdentity,
                    new ResolutionContext(), new TestNuGetProjectContext(), sourceRepositoryProvider.GetRepositories().First(), null, token);

                // Assert
                // Check that the packages.config file exists after the installation
                Assert.True(File.Exists(packagesConfigPath));
                // Check the number of packages and packages returned by PackagesConfigProject after the installation
                packagesInPackagesConfig = (await msBuildNuGetProject.PackagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(2, packagesInPackagesConfig.Count);
                Assert.Equal(firstPackageIdentity, packagesInPackagesConfig[1].PackageIdentity);
                Assert.Equal(msBuildNuGetProject.ProjectSystem.TargetFramework, packagesInPackagesConfig[1].TargetFramework);
                Assert.Equal(secondPackageIdentity, packagesInPackagesConfig[0].PackageIdentity);
                Assert.Equal(msBuildNuGetProject.ProjectSystem.TargetFramework, packagesInPackagesConfig[0].TargetFramework);
            }
        }

        [Fact]
        public async Task InstallSamePackageAfterInstall()
        {
            // Arrange
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV3OnlySourceRepositoryProvider();
            using (var testSolutionManager = new TestSolutionManager())
            {
                var testSettings = NullSettings.Instance;
                var token = CancellationToken.None;
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    testSettings,
                    testSolutionManager,
                    deleteOnRestartManager);
                var packagesFolderPath = PackagesFolderPathUtility.GetPackagesFolderPath(testSolutionManager, testSettings);

                var msBuildNuGetProject = testSolutionManager.AddNewMSBuildProject();
                var msBuildNuGetProjectSystem = msBuildNuGetProject.ProjectSystem as TestMSBuildNuGetProjectSystem;
                var packagesConfigPath = msBuildNuGetProject.PackagesConfigNuGetProject.FullPath;
                var firstPackageIdentity = _noDependencyLibPackages[0];

                // Pre-Assert
                // Check that the packages.config file does not exist
                Assert.False(File.Exists(packagesConfigPath));
                // Check that there are no packages returned by PackagesConfigProject
                var packagesInPackagesConfig = (await msBuildNuGetProject.PackagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(0, packagesInPackagesConfig.Count);
                Assert.Equal(0, msBuildNuGetProjectSystem.References.Count);

                // Act
                await nuGetPackageManager.InstallPackageAsync(msBuildNuGetProject, firstPackageIdentity,
                    new ResolutionContext(), new TestNuGetProjectContext(), sourceRepositoryProvider.GetRepositories().First(), null, token);

                var secondPackageIdentity = _noDependencyLibPackages[1];
                await nuGetPackageManager.InstallPackageAsync(msBuildNuGetProject, secondPackageIdentity,
                    new ResolutionContext(), new TestNuGetProjectContext(), sourceRepositoryProvider.GetRepositories().First(), null, token);

                // Assert
                // Check that the packages.config file exists after the installation
                Assert.True(File.Exists(packagesConfigPath));
                // Check the number of packages and packages returned by PackagesConfigProject after the installation
                packagesInPackagesConfig = (await msBuildNuGetProject.PackagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(1, packagesInPackagesConfig.Count);
                Assert.Equal(secondPackageIdentity, packagesInPackagesConfig[0].PackageIdentity);
                Assert.Equal(msBuildNuGetProject.ProjectSystem.TargetFramework, packagesInPackagesConfig[0].TargetFramework);
            }
        }

        [Fact]
        public async Task InstallPackageWithDependents()
        {
            // Arrange
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV3OnlySourceRepositoryProvider();
            using (var testSolutionManager = new TestSolutionManager())
            {
                var testSettings = NullSettings.Instance;
                var token = CancellationToken.None;
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    testSettings,
                    testSolutionManager,
                    deleteOnRestartManager);
                var packagesFolderPath = PackagesFolderPathUtility.GetPackagesFolderPath(testSolutionManager, testSettings);

                var msBuildNuGetProject = testSolutionManager.AddNewMSBuildProject();
                var msBuildNuGetProjectSystem = msBuildNuGetProject.ProjectSystem as TestMSBuildNuGetProjectSystem;
                var packagesConfigPath = msBuildNuGetProject.PackagesConfigNuGetProject.FullPath;
                var packageIdentity = _packageWithDependents[2];

                // Pre-Assert
                // Check that the packages.config file does not exist
                Assert.False(File.Exists(packagesConfigPath));
                // Check that there are no packages returned by PackagesConfigProject
                var packagesInPackagesConfig = (await msBuildNuGetProject.PackagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(0, packagesInPackagesConfig.Count);
                Assert.Equal(0, msBuildNuGetProjectSystem.References.Count);

                // Act
                await nuGetPackageManager.InstallPackageAsync(msBuildNuGetProject, packageIdentity,
                    new ResolutionContext(), new TestNuGetProjectContext(), sourceRepositoryProvider.GetRepositories().First(), null, token);

                // Assert
                // Check that the packages.config file exists after the installation
                Assert.True(File.Exists(packagesConfigPath));
                // Check the number of packages and packages returned by PackagesConfigProject after the installation
                packagesInPackagesConfig = (await msBuildNuGetProject.PackagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(2, packagesInPackagesConfig.Count);
                Assert.Equal(packageIdentity, packagesInPackagesConfig[1].PackageIdentity);
                Assert.Equal(msBuildNuGetProject.ProjectSystem.TargetFramework, packagesInPackagesConfig[1].TargetFramework);
                Assert.Equal(_packageWithDependents[0], packagesInPackagesConfig[0].PackageIdentity);
                Assert.Equal(msBuildNuGetProject.ProjectSystem.TargetFramework, packagesInPackagesConfig[0].TargetFramework);
            }
        }

        [Fact]
        public async Task InstallHigherSpecificVersion()
        {
            // Arrange
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV3OnlySourceRepositoryProvider();
            using (var testSolutionManager = new TestSolutionManager())
            {
                var testSettings = NullSettings.Instance;
                var token = CancellationToken.None;
                var resolutionContext = new ResolutionContext();
                var testNuGetProjectContext = new TestNuGetProjectContext();
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    testSettings,
                    testSolutionManager,
                    deleteOnRestartManager);
                var packagesFolderPath = PackagesFolderPathUtility.GetPackagesFolderPath(testSolutionManager, testSettings);
                var packagePathResolver = new PackagePathResolver(packagesFolderPath);

                var projectA = testSolutionManager.AddNewMSBuildProject();
                var packageIdentity0 = _packageWithDependents[0];
                var packageIdentity1 = _packageWithDependents[1];

                // Act
                await nuGetPackageManager.InstallPackageAsync(projectA, packageIdentity0,
                    resolutionContext, testNuGetProjectContext, sourceRepositoryProvider.GetRepositories().First(), null, token);

                // Assert
                var projectAInstalled = (await projectA.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(1, projectAInstalled.Count);
                Assert.Equal(packageIdentity0, projectAInstalled[0].PackageIdentity);
                Assert.True(File.Exists(packagePathResolver.GetInstalledPackageFilePath(packageIdentity0)));
                Assert.False(File.Exists(packagePathResolver.GetInstalledPackageFilePath(packageIdentity1)));

                // Main Act
                await nuGetPackageManager.InstallPackageAsync(projectA, packageIdentity1,
                    resolutionContext, testNuGetProjectContext, sourceRepositoryProvider.GetRepositories().First(), null, token);

                // Assert
                projectAInstalled = (await projectA.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(1, projectAInstalled.Count);
                Assert.Equal(packageIdentity1, projectAInstalled[0].PackageIdentity);
                Assert.False(File.Exists(packagePathResolver.GetInstalledPackageFilePath(packageIdentity0)));
                Assert.True(File.Exists(packagePathResolver.GetInstalledPackageFilePath(packageIdentity1)));
            }
        }

        [Fact]
        public async Task InstallLowerSpecificVersion()
        {
            // Arrange
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV3OnlySourceRepositoryProvider();
            using (var testSolutionManager = new TestSolutionManager())
            {
                var testSettings = NullSettings.Instance;
                var token = CancellationToken.None;
                var resolutionContext = new ResolutionContext();
                var testNuGetProjectContext = new TestNuGetProjectContext();
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    testSettings,
                    testSolutionManager,
                    deleteOnRestartManager);
                var packagesFolderPath = PackagesFolderPathUtility.GetPackagesFolderPath(testSolutionManager, testSettings);
                var packagePathResolver = new PackagePathResolver(packagesFolderPath);

                var projectA = testSolutionManager.AddNewMSBuildProject();
                var packageIdentity0 = _packageWithDependents[0];
                var packageIdentity1 = _packageWithDependents[1];

                // Act
                await nuGetPackageManager.InstallPackageAsync(projectA, packageIdentity1,
                    resolutionContext, testNuGetProjectContext, sourceRepositoryProvider.GetRepositories().First(), null, token);

                // Assert
                var projectAInstalled = (await projectA.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(1, projectAInstalled.Count);
                Assert.Equal(packageIdentity1, projectAInstalled[0].PackageIdentity);
                Assert.True(File.Exists(packagePathResolver.GetInstalledPackageFilePath(packageIdentity1)));
                Assert.False(File.Exists(packagePathResolver.GetInstalledPackageFilePath(packageIdentity0)));

                // Main Act
                await nuGetPackageManager.InstallPackageAsync(projectA, packageIdentity0,
                    resolutionContext, testNuGetProjectContext, sourceRepositoryProvider.GetRepositories().First(), null, token);

                // Assert
                projectAInstalled = (await projectA.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(1, projectAInstalled.Count);
                Assert.Equal(packageIdentity0, projectAInstalled[0].PackageIdentity);
                Assert.False(File.Exists(packagePathResolver.GetInstalledPackageFilePath(packageIdentity1)));
                Assert.True(File.Exists(packagePathResolver.GetInstalledPackageFilePath(packageIdentity0)));
            }
        }

        [Fact]
        public async Task InstallLatestVersion()
        {
            // Arrange
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV3OnlySourceRepositoryProvider();
            using (var testSolutionManager = new TestSolutionManager())
            {
                var testSettings = NullSettings.Instance;
                var token = CancellationToken.None;
                var resolutionContext = new ResolutionContext();
                var testNuGetProjectContext = new TestNuGetProjectContext();
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    testSettings,
                    testSolutionManager,
                    deleteOnRestartManager);
                var packagesFolderPath = PackagesFolderPathUtility.GetPackagesFolderPath(testSolutionManager, testSettings);
                var packagePathResolver = new PackagePathResolver(packagesFolderPath);

                var projectA = testSolutionManager.AddNewMSBuildProject();
                var packageIdentity0 = _packageWithDependents[0];

                var resolvedPackage = await NuGetPackageManager.GetLatestVersionAsync(
                    packageIdentity0.Id,
                    projectA,
                    resolutionContext,
                    sourceRepositoryProvider.GetRepositories().First(),
                    NullLogger.Instance,
                    token);

                var packageLatest = new PackageIdentity(packageIdentity0.Id, resolvedPackage.LatestVersion);

                // Act
                await nuGetPackageManager.InstallPackageAsync(projectA, packageIdentity0,
                    resolutionContext, testNuGetProjectContext, sourceRepositoryProvider.GetRepositories().First(), null, token);

                // Assert
                var projectAInstalled = (await projectA.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(1, projectAInstalled.Count);
                Assert.Equal(packageIdentity0, projectAInstalled[0].PackageIdentity);
                Assert.True(File.Exists(packagePathResolver.GetInstalledPackageFilePath(packageIdentity0)));
                Assert.False(File.Exists(packagePathResolver.GetInstalledPackageFilePath(packageLatest)));

                // Main Act
                await nuGetPackageManager.InstallPackageAsync(projectA, packageIdentity0.Id,
                    resolutionContext, testNuGetProjectContext, sourceRepositoryProvider.GetRepositories().First(), null, token);

                // Assert
                projectAInstalled = (await projectA.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(1, projectAInstalled.Count);
                Assert.Equal(packageLatest, projectAInstalled[0].PackageIdentity);
                Assert.False(File.Exists(packagePathResolver.GetInstalledPackageFilePath(packageIdentity0)));
                Assert.True(File.Exists(packagePathResolver.GetInstalledPackageFilePath(packageLatest)));
            }
        }

        [Fact]
        public async Task InstallLatestVersionForPackageReference()
        {
            // Arrange
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV3OnlySourceRepositoryProvider();

            var testSettings = NullSettings.Instance;
            var token = CancellationToken.None;
            var resolutionContext = new ResolutionContext();

            var packageIdentity0 = _packageWithDependents[0];

            // Act
            var resolvedPackage = await NuGetPackageManager.GetLatestVersionAsync(
                new PackageReference(packageIdentity0, NuGetFramework.AnyFramework),
                NuGetFramework.AnyFramework,
                resolutionContext,
                sourceRepositoryProvider.GetRepositories(),
                NullLogger.Instance,
                token);

            // Assert
            Assert.NotNull(resolvedPackage.LatestVersion);
        }

        [Fact]
        public async Task InstallLatestVersionOfDependencyPackage()
        {
            // Arrange
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV3OnlySourceRepositoryProvider();
            using (var testSolutionManager = new TestSolutionManager())
            {
                var testSettings = NullSettings.Instance;
                var token = CancellationToken.None;
                var resolutionContext = new ResolutionContext();
                var testNuGetProjectContext = new TestNuGetProjectContext();
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    testSettings,
                    testSolutionManager,
                    deleteOnRestartManager);
                var packagesFolderPath = PackagesFolderPathUtility.GetPackagesFolderPath(testSolutionManager, testSettings);
                var packagePathResolver = new PackagePathResolver(packagesFolderPath);

                var projectA = testSolutionManager.AddNewMSBuildProject();
                var packageIdentity0 = _packageWithDependents[0];
                var dependentPackage = _packageWithDependents[2];

                var resolvedPackage = await NuGetPackageManager.GetLatestVersionAsync(
                    packageIdentity0.Id,
                    projectA,
                    resolutionContext,
                    sourceRepositoryProvider.GetRepositories().First(),
                    NullLogger.Instance,
                    token);

                var packageLatest = new PackageIdentity(packageIdentity0.Id, resolvedPackage.LatestVersion);

                // Act
                await nuGetPackageManager.InstallPackageAsync(projectA, dependentPackage,
                    resolutionContext, testNuGetProjectContext, sourceRepositoryProvider.GetRepositories().First(), null, token);

                // Assert
                var projectAInstalled = (await projectA.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(2, projectAInstalled.Count);
                Assert.Equal(packageIdentity0, projectAInstalled[0].PackageIdentity);
                Assert.True(File.Exists(packagePathResolver.GetInstalledPackageFilePath(packageIdentity0)));
                Assert.Equal(dependentPackage, projectAInstalled[1].PackageIdentity);
                Assert.True(File.Exists(packagePathResolver.GetInstalledPackageFilePath(dependentPackage)));
                Assert.False(File.Exists(packagePathResolver.GetInstalledPackageFilePath(packageLatest)));

                // Main Act
                await nuGetPackageManager.InstallPackageAsync(projectA, packageIdentity0.Id,
                    resolutionContext, testNuGetProjectContext, sourceRepositoryProvider.GetRepositories().First(), null, token);

                // Assert
                projectAInstalled = (await projectA.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(2, projectAInstalled.Count);
                Assert.Equal(packageLatest, projectAInstalled[0].PackageIdentity);
                Assert.Equal(dependentPackage, projectAInstalled[1].PackageIdentity);
                Assert.True(File.Exists(packagePathResolver.GetInstalledPackageFilePath(dependentPackage)));
                Assert.False(File.Exists(packagePathResolver.GetInstalledPackageFilePath(packageIdentity0)));
                Assert.True(File.Exists(packagePathResolver.GetInstalledPackageFilePath(packageLatest)));
            }
        }

        [Fact]
        public async Task InstallHigherSpecificVersionOfDependencyPackage()
        {
            // Arrange
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV3OnlySourceRepositoryProvider();
            using (var testSolutionManager = new TestSolutionManager())
            {
                var testSettings = NullSettings.Instance;
                var token = CancellationToken.None;
                var resolutionContext = new ResolutionContext();
                var testNuGetProjectContext = new TestNuGetProjectContext();
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    testSettings,
                    testSolutionManager,
                    deleteOnRestartManager);
                var packagesFolderPath = PackagesFolderPathUtility.GetPackagesFolderPath(testSolutionManager, testSettings);
                var packagePathResolver = new PackagePathResolver(packagesFolderPath);

                var projectA = testSolutionManager.AddNewMSBuildProject();
                var packageIdentity0 = _packageWithDependents[0];
                var packageIdentity1 = _packageWithDependents[1];
                var dependentPackage = _packageWithDependents[2];

                // Act
                await nuGetPackageManager.InstallPackageAsync(projectA, dependentPackage,
                    resolutionContext, testNuGetProjectContext, sourceRepositoryProvider.GetRepositories().First(), null, token);

                // Assert
                var projectAInstalled = (await projectA.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(2, projectAInstalled.Count);
                Assert.Equal(packageIdentity0, projectAInstalled[0].PackageIdentity);
                Assert.True(File.Exists(packagePathResolver.GetInstalledPackageFilePath(packageIdentity0)));
                Assert.Equal(dependentPackage, projectAInstalled[1].PackageIdentity);
                Assert.True(File.Exists(packagePathResolver.GetInstalledPackageFilePath(dependentPackage)));
                Assert.False(File.Exists(packagePathResolver.GetInstalledPackageFilePath(packageIdentity1)));

                // Main Act
                await nuGetPackageManager.InstallPackageAsync(projectA, packageIdentity1,
                    resolutionContext, testNuGetProjectContext, sourceRepositoryProvider.GetRepositories().First(), null, token);

                // Assert
                projectAInstalled = (await projectA.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(2, projectAInstalled.Count);
                Assert.Equal(packageIdentity1, projectAInstalled[0].PackageIdentity);
                Assert.Equal(dependentPackage, projectAInstalled[1].PackageIdentity);
                Assert.True(File.Exists(packagePathResolver.GetInstalledPackageFilePath(dependentPackage)));
                Assert.False(File.Exists(packagePathResolver.GetInstalledPackageFilePath(packageIdentity0)));
                Assert.True(File.Exists(packagePathResolver.GetInstalledPackageFilePath(packageIdentity1)));
            }
        }

        [Fact]
        public async Task InstallLowerSpecificVersionOfDependencyPackage()
        {
            // Arrange
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV3OnlySourceRepositoryProvider();
            using (var testSolutionManager = new TestSolutionManager())
            {
                var testSettings = NullSettings.Instance;
                var token = CancellationToken.None;
                var resolutionContext = new ResolutionContext(DependencyBehavior.Highest, false, true, VersionConstraints.None);
                var testNuGetProjectContext = new TestNuGetProjectContext();
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    testSettings,
                    testSolutionManager,
                    deleteOnRestartManager);
                var packagesFolderPath = PackagesFolderPathUtility.GetPackagesFolderPath(testSolutionManager, testSettings);
                var packagePathResolver = new PackagePathResolver(packagesFolderPath);

                var projectA = testSolutionManager.AddNewMSBuildProject();
                var packageIdentity0 = _packageWithDependents[0];
                var dependentPackage = _packageWithDependents[2];

                var resolvedPackage = await NuGetPackageManager.GetLatestVersionAsync(
                    packageIdentity0.Id,
                    projectA,
                    resolutionContext,
                    sourceRepositoryProvider.GetRepositories().First(),
                    NullLogger.Instance,
                    token);

                var packageLatest = new PackageIdentity(packageIdentity0.Id, resolvedPackage.LatestVersion);

                // Act
                await nuGetPackageManager.InstallPackageAsync(projectA, dependentPackage,
                    resolutionContext, testNuGetProjectContext, sourceRepositoryProvider.GetRepositories().First(), null, token);

                // Assert
                var projectAInstalled = (await projectA.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(2, projectAInstalled.Count);
                Assert.Equal(packageLatest, projectAInstalled[0].PackageIdentity);
                Assert.True(File.Exists(packagePathResolver.GetInstalledPackageFilePath(packageLatest)));
                Assert.Equal(dependentPackage, projectAInstalled[1].PackageIdentity);
                Assert.True(File.Exists(packagePathResolver.GetInstalledPackageFilePath(dependentPackage)));
                Assert.False(File.Exists(packagePathResolver.GetInstalledPackageFilePath(packageIdentity0)));

                // Main Act
                await nuGetPackageManager.InstallPackageAsync(projectA, packageIdentity0,
                    resolutionContext, testNuGetProjectContext, sourceRepositoryProvider.GetRepositories().First(), null, token);

                // Assert
                projectAInstalled = (await projectA.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(2, projectAInstalled.Count);
                Assert.Equal(packageIdentity0, projectAInstalled[0].PackageIdentity);
                Assert.Equal(dependentPackage, projectAInstalled[1].PackageIdentity);
                Assert.True(File.Exists(packagePathResolver.GetInstalledPackageFilePath(dependentPackage)));
                Assert.False(File.Exists(packagePathResolver.GetInstalledPackageFilePath(packageLatest)));
                Assert.True(File.Exists(packagePathResolver.GetInstalledPackageFilePath(packageIdentity0)));
            }
        }

        [Fact]
        public async Task InstallPackageWhichUpdatesParent()
        {
            // https://github.com/NuGet/Home/issues/127
            // Repro step:
            // 1.Install-Package jquery.validation -Version 1.8
            // 2.Update-package jquery -version 2.0.3
            // Expected: jquery.validation was updated to 1.8.0.1
            // jquery 1.8 is unique because it allows only a single version of jquery

            // Arrange
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV3OnlySourceRepositoryProvider();
            using (var testSolutionManager = new TestSolutionManager())
            {
                var testSettings = NullSettings.Instance;
                var token = CancellationToken.None;
                var resolutionContext = new ResolutionContext();
                var testNuGetProjectContext = new TestNuGetProjectContext();
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    testSettings,
                    testSolutionManager,
                    deleteOnRestartManager);
                var packagesFolderPath = PackagesFolderPathUtility.GetPackagesFolderPath(testSolutionManager, testSettings);
                var packagePathResolver = new PackagePathResolver(packagesFolderPath);

                var projectA = testSolutionManager.AddNewMSBuildProject();
                var jqueryValidation18 = new PackageIdentity("jquery.validation", NuGetVersion.Parse("1.8"));
                var jquery203 = new PackageIdentity("jquery", NuGetVersion.Parse("2.0.3"));

                // Act
                await nuGetPackageManager.InstallPackageAsync(projectA, jqueryValidation18,
                    resolutionContext, testNuGetProjectContext, sourceRepositoryProvider.GetRepositories().First(), null, token);

                // Assert
                var projectAInstalled = (await projectA.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(2, projectAInstalled.Count);
                Assert.Equal(jqueryValidation18, projectAInstalled[1].PackageIdentity);
                Assert.True(File.Exists(packagePathResolver.GetInstalledPackageFilePath(jqueryValidation18)));

                // Main Act
                await nuGetPackageManager.InstallPackageAsync(projectA, jquery203,
                    resolutionContext, testNuGetProjectContext, sourceRepositoryProvider.GetRepositories().First(), null, token);

                // Assert
                projectAInstalled = (await projectA.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(2, projectAInstalled.Count);
                Assert.Equal(new PackageIdentity("jquery.validation", NuGetVersion.Parse("1.8.0.1")), projectAInstalled[1].PackageIdentity);
                Assert.False(File.Exists(packagePathResolver.GetInstalledPackageFilePath(jqueryValidation18)));
            }
        }

        [Fact]
        public async Task InstallPackageWhichUpdatesDependency()
        {
            // Arrange
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV3OnlySourceRepositoryProvider();
            using (var testSolutionManager = new TestSolutionManager())
            {
                var testSettings = NullSettings.Instance;
                var token = CancellationToken.None;
                var resolutionContext = new ResolutionContext();
                var testNuGetProjectContext = new TestNuGetProjectContext();
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    testSettings,
                    testSolutionManager,
                    deleteOnRestartManager);
                var packagesFolderPath = PackagesFolderPathUtility.GetPackagesFolderPath(testSolutionManager, testSettings);
                var packagePathResolver = new PackagePathResolver(packagesFolderPath);

                var projectA = testSolutionManager.AddNewMSBuildProject();
                var packageIdentity0 = _packageWithDependents[0];
                var packageIdentity1 = _packageWithDependents[1];
                var packageIdentity2 = _packageWithDependents[2];
                var packageIdentity3 = _packageWithDependents[3];

                // Act
                await nuGetPackageManager.InstallPackageAsync(projectA, packageIdentity2,
                    resolutionContext, testNuGetProjectContext, sourceRepositoryProvider.GetRepositories().First(), null, token);

                // Assert
                var projectAInstalled = (await projectA.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(2, projectAInstalled.Count);
                Assert.Equal(packageIdentity0, projectAInstalled[0].PackageIdentity);
                Assert.Equal(packageIdentity2, projectAInstalled[1].PackageIdentity);
                Assert.True(File.Exists(packagePathResolver.GetInstalledPackageFilePath(packageIdentity0)));
                Assert.True(File.Exists(packagePathResolver.GetInstalledPackageFilePath(packageIdentity2)));

                // Main Act
                await nuGetPackageManager.InstallPackageAsync(projectA, packageIdentity3,
                    resolutionContext, testNuGetProjectContext, sourceRepositoryProvider.GetRepositories().First(), null, token);

                // Assert
                projectAInstalled = (await projectA.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(3, projectAInstalled.Count);
                Assert.Equal(packageIdentity1, projectAInstalled[0].PackageIdentity);
                Assert.Equal(packageIdentity2, projectAInstalled[2].PackageIdentity);
                Assert.Equal(packageIdentity3, projectAInstalled[1].PackageIdentity);
                Assert.False(File.Exists(packagePathResolver.GetInstalledPackageFilePath(packageIdentity0)));
                Assert.True(File.Exists(packagePathResolver.GetInstalledPackageFilePath(packageIdentity1)));
                Assert.True(File.Exists(packagePathResolver.GetInstalledPackageFilePath(packageIdentity2)));
                Assert.True(File.Exists(packagePathResolver.GetInstalledPackageFilePath(packageIdentity3)));
            }
        }

        [Fact]
        public async Task InstallPackageWhichUsesExistingDependency()
        {
            // Arrange
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV3OnlySourceRepositoryProvider();
            using (var testSolutionManager = new TestSolutionManager())
            {
                var testSettings = NullSettings.Instance;
                var token = CancellationToken.None;
                var resolutionContext = new ResolutionContext();
                var testNuGetProjectContext = new TestNuGetProjectContext();
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    testSettings,
                    testSolutionManager,
                    deleteOnRestartManager);
                var packagesFolderPath = PackagesFolderPathUtility.GetPackagesFolderPath(testSolutionManager, testSettings);
                var packagePathResolver = new PackagePathResolver(packagesFolderPath);

                var projectA = testSolutionManager.AddNewMSBuildProject();
                var packageIdentity0 = _packageWithDependents[0];
                var packageIdentity1 = _packageWithDependents[1];
                var packageIdentity2 = _packageWithDependents[2];
                var packageIdentity3 = _packageWithDependents[3];

                // Act
                await nuGetPackageManager.InstallPackageAsync(projectA, packageIdentity3,
                    resolutionContext, testNuGetProjectContext, sourceRepositoryProvider.GetRepositories().First(), null, token);

                // Assert
                var projectAInstalled = (await projectA.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(2, projectAInstalled.Count);
                Assert.Equal(packageIdentity1, projectAInstalled[0].PackageIdentity);
                Assert.Equal(packageIdentity3, projectAInstalled[1].PackageIdentity);
                Assert.True(File.Exists(packagePathResolver.GetInstalledPackageFilePath(packageIdentity1)));
                Assert.True(File.Exists(packagePathResolver.GetInstalledPackageFilePath(packageIdentity3)));

                // Main Act
                await nuGetPackageManager.InstallPackageAsync(projectA, packageIdentity2,
                    resolutionContext, testNuGetProjectContext, sourceRepositoryProvider.GetRepositories().First(), null, token);

                // Assert
                projectAInstalled = (await projectA.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(3, projectAInstalled.Count);
                Assert.Equal(packageIdentity1, projectAInstalled[0].PackageIdentity);
                Assert.Equal(packageIdentity2, projectAInstalled[2].PackageIdentity);
                Assert.Equal(packageIdentity3, projectAInstalled[1].PackageIdentity);
                Assert.False(File.Exists(packagePathResolver.GetInstalledPackageFilePath(packageIdentity0)));
                Assert.True(File.Exists(packagePathResolver.GetInstalledPackageFilePath(packageIdentity1)));
                Assert.True(File.Exists(packagePathResolver.GetInstalledPackageFilePath(packageIdentity2)));
                Assert.True(File.Exists(packagePathResolver.GetInstalledPackageFilePath(packageIdentity3)));
            }
        }

        [Fact]
        public async Task InstallPackageWhichUpdatesExistingDependencyDueToDependencyBehavior()
        {
            // Arrange
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV3OnlySourceRepositoryProvider();
            using (var testSolutionManager = new TestSolutionManager())
            {
                var testSettings = NullSettings.Instance;
                var token = CancellationToken.None;
                var resolutionContext = new ResolutionContext(DependencyBehavior.Highest, false, true, VersionConstraints.None);
                var testNuGetProjectContext = new TestNuGetProjectContext();
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    testSettings,
                    testSolutionManager,
                    deleteOnRestartManager);
                var packagesFolderPath = PackagesFolderPathUtility.GetPackagesFolderPath(testSolutionManager, testSettings);
                var packagePathResolver = new PackagePathResolver(packagesFolderPath);

                var projectA = testSolutionManager.AddNewMSBuildProject();
                var packageIdentity0 = _packageWithDependents[0];
                var packageIdentity1 = _packageWithDependents[1];
                var packageIdentity2 = _packageWithDependents[2];
                var packageIdentity3 = _packageWithDependents[3];

                var resolvedPackage = await NuGetPackageManager.GetLatestVersionAsync(
                    packageIdentity0.Id,
                    projectA,
                    resolutionContext,
                    sourceRepositoryProvider.GetRepositories().First(),
                    NullLogger.Instance,
                    token);

                var packageLatest = new PackageIdentity(packageIdentity0.Id, resolvedPackage.LatestVersion);

                // Act
                await nuGetPackageManager.InstallPackageAsync(projectA, packageIdentity3,
                    resolutionContext, testNuGetProjectContext, sourceRepositoryProvider.GetRepositories().First(), null, token);

                // Assert
                var projectAInstalled = (await projectA.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(2, projectAInstalled.Count);
                Assert.Equal(packageLatest, projectAInstalled[0].PackageIdentity);
                Assert.Equal(packageIdentity3, projectAInstalled[1].PackageIdentity);
                Assert.True(File.Exists(packagePathResolver.GetInstalledPackageFilePath(packageLatest)));
                Assert.True(File.Exists(packagePathResolver.GetInstalledPackageFilePath(packageIdentity3)));

                // Main Act
                await nuGetPackageManager.InstallPackageAsync(projectA, packageIdentity2,
                    resolutionContext, testNuGetProjectContext, sourceRepositoryProvider.GetRepositories().First(), null, token);

                // Assert
                projectAInstalled = (await projectA.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(3, projectAInstalled.Count);
                Assert.Equal(packageLatest, projectAInstalled[0].PackageIdentity);
                Assert.Equal(packageIdentity2, projectAInstalled[2].PackageIdentity);
                Assert.Equal(packageIdentity3, projectAInstalled[1].PackageIdentity);
                Assert.True(File.Exists(packagePathResolver.GetInstalledPackageFilePath(packageLatest)));
                Assert.True(File.Exists(packagePathResolver.GetInstalledPackageFilePath(packageIdentity2)));
                Assert.True(File.Exists(packagePathResolver.GetInstalledPackageFilePath(packageIdentity3)));
            }
        }

        [Fact]
        public async Task InstallWithIgnoreDependencies()
        {
            // Arrange
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV3OnlySourceRepositoryProvider();
            using (var testSolutionManager = new TestSolutionManager())
            {
                var testSettings = NullSettings.Instance;
                var token = CancellationToken.None;
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    testSettings,
                    testSolutionManager,
                    deleteOnRestartManager);
                var packagesFolderPath = PackagesFolderPathUtility.GetPackagesFolderPath(testSolutionManager, testSettings);

                var msBuildNuGetProject = testSolutionManager.AddNewMSBuildProject();
                var msBuildNuGetProjectSystem = msBuildNuGetProject.ProjectSystem as TestMSBuildNuGetProjectSystem;
                var packagesConfigPath = msBuildNuGetProject.PackagesConfigNuGetProject.FullPath;
                var packageIdentity = _packageWithDependents[2];

                // Pre-Assert
                // Check that the packages.config file does not exist
                Assert.False(File.Exists(packagesConfigPath));
                // Check that there are no packages returned by PackagesConfigProject
                var packagesInPackagesConfig = (await msBuildNuGetProject.PackagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(0, packagesInPackagesConfig.Count);
                Assert.Equal(0, msBuildNuGetProjectSystem.References.Count);

                // Act
                await nuGetPackageManager.InstallPackageAsync(msBuildNuGetProject, packageIdentity,
                    new ResolutionContext(DependencyBehavior.Ignore, false, true, VersionConstraints.None), new TestNuGetProjectContext(), sourceRepositoryProvider.GetRepositories().First(), null, token);

                // Assert
                // Check that the packages.config file exists after the installation
                Assert.True(File.Exists(packagesConfigPath));
                // Check the number of packages and packages returned by PackagesConfigProject after the installation
                packagesInPackagesConfig = (await msBuildNuGetProject.PackagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(1, packagesInPackagesConfig.Count);
                Assert.Equal(packageIdentity, packagesInPackagesConfig[0].PackageIdentity);
                Assert.Equal(msBuildNuGetProject.ProjectSystem.TargetFramework, packagesInPackagesConfig[0].TargetFramework);
            }
        }

        [Fact]
        public async Task ThrowsPackageNotFound()
        {
            // Arrange
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV3OnlySourceRepositoryProvider();
            using (var testSolutionManager = new TestSolutionManager())
            {
                var testSettings = NullSettings.Instance;
                var token = CancellationToken.None;
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    testSettings,
                    testSolutionManager,
                    deleteOnRestartManager);
                var packagesFolderPath = PackagesFolderPathUtility.GetPackagesFolderPath(testSolutionManager, testSettings);

                var msBuildNuGetProject = testSolutionManager.AddNewMSBuildProject();
                var msBuildNuGetProjectSystem = msBuildNuGetProject.ProjectSystem as TestMSBuildNuGetProjectSystem;
                var packagesConfigPath = msBuildNuGetProject.PackagesConfigNuGetProject.FullPath;
                var packageIdentity = new PackageIdentity("DoesNotExist", new NuGetVersion("1.0.0"));

                // Pre-Assert
                // Check that the packages.config file does not exist
                Assert.False(File.Exists(packagesConfigPath));
                // Check that there are no packages returned by PackagesConfigProject
                var packagesInPackagesConfig = (await msBuildNuGetProject.PackagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(0, packagesInPackagesConfig.Count);
                Assert.Equal(0, msBuildNuGetProjectSystem.References.Count);

                // Act
                Exception exception = null;
                try
                {
                    await nuGetPackageManager.InstallPackageAsync(msBuildNuGetProject, packageIdentity,
                        new ResolutionContext(), new TestNuGetProjectContext(), sourceRepositoryProvider.GetRepositories().First(), null, token);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                Assert.NotNull(exception);
                Assert.True(exception is InvalidOperationException);
                Assert.Contains("Package 'DoesNotExist 1.0.0' is not found", exception.Message);
            }
        }

        [Fact]
        public async Task ThrowsLatestVersionNotFound()
        {
            // Arrange
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV3OnlySourceRepositoryProvider();
            using (var testSolutionManager = new TestSolutionManager())
            {
                var testSettings = NullSettings.Instance;
                var token = CancellationToken.None;
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    testSettings,
                    testSolutionManager,
                    deleteOnRestartManager);
                var packagesFolderPath = PackagesFolderPathUtility.GetPackagesFolderPath(testSolutionManager, testSettings);

                var msBuildNuGetProject = testSolutionManager.AddNewMSBuildProject();
                var msBuildNuGetProjectSystem = msBuildNuGetProject.ProjectSystem as TestMSBuildNuGetProjectSystem;
                var packagesConfigPath = msBuildNuGetProject.PackagesConfigNuGetProject.FullPath;
                var packageIdentity = "DoesNotExist";

                // Pre-Assert
                // Check that the packages.config file does not exist
                Assert.False(File.Exists(packagesConfigPath));
                // Check that there are no packages returned by PackagesConfigProject
                var packagesInPackagesConfig = (await msBuildNuGetProject.PackagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(0, packagesInPackagesConfig.Count);
                Assert.Equal(0, msBuildNuGetProjectSystem.References.Count);

                // Act
                Exception exception = null;
                try
                {
                    await nuGetPackageManager.InstallPackageAsync(msBuildNuGetProject, packageIdentity,
                        new ResolutionContext(), new TestNuGetProjectContext(), sourceRepositoryProvider.GetRepositories().First(), null, token);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                Assert.NotNull(exception);
                Assert.True(exception is InvalidOperationException);
                Assert.Equal("No latest version found for 'DoesNotExist' for the given source repositories and resolution context", exception.Message);
            }
        }

        [Fact]
        public async Task InstallPackageWithDeepDependency()
        {
            // Arrange
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV3OnlySourceRepositoryProvider();
            using (var testSolutionManager = new TestSolutionManager())
            {
                var testSettings = NullSettings.Instance;
                var token = CancellationToken.None;
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    testSettings,
                    testSolutionManager,
                    deleteOnRestartManager);
                var packagesFolderPath = PackagesFolderPathUtility.GetPackagesFolderPath(testSolutionManager, testSettings);

                var msBuildNuGetProject = testSolutionManager.AddNewMSBuildProject();
                var msBuildNuGetProjectSystem = msBuildNuGetProject.ProjectSystem as TestMSBuildNuGetProjectSystem;
                var packagesConfigPath = msBuildNuGetProject.PackagesConfigNuGetProject.FullPath;
                var packageIdentity = _packageWithDeepDependency[6]; // WindowsAzure.Storage.4.3.0

                // Pre-Assert
                // Check that the packages.config file does not exist
                Assert.False(File.Exists(packagesConfigPath));
                // Check that there are no packages returned by PackagesConfigProject
                var packagesInPackagesConfig = (await msBuildNuGetProject.PackagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(0, packagesInPackagesConfig.Count);
                Assert.Equal(0, msBuildNuGetProjectSystem.References.Count);

                // Act
                await nuGetPackageManager.InstallPackageAsync(msBuildNuGetProject, packageIdentity,
                    new ResolutionContext(), new TestNuGetProjectContext(), sourceRepositoryProvider.GetRepositories().First(), null, token);

                // Assert
                // Check that the packages.config file exists after the installation
                Assert.True(File.Exists(packagesConfigPath));
                // Check the number of packages and packages returned by PackagesConfigProject after the installation
                packagesInPackagesConfig = (await msBuildNuGetProject.PackagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(7, packagesInPackagesConfig.Count);

                var installedPackages = _packageWithDeepDependency.OrderBy(f => f.Id).ToList();

                for (var i = 0; i < 7; i++)
                {
                    Assert.Equal(installedPackages[i], packagesInPackagesConfig[i].PackageIdentity);
                    Assert.Equal(msBuildNuGetProject.ProjectSystem.TargetFramework, packagesInPackagesConfig[i].TargetFramework);
                }
            }
        }

        [Fact]
        public async Task InstallPackageBindingRedirectsWithDeepDependency()
        {
            // Arrange
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV3OnlySourceRepositoryProvider();
            using (var testSolutionManager = new TestSolutionManager())
            {
                var testSettings = NullSettings.Instance;
                var token = CancellationToken.None;
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    testSettings,
                    testSolutionManager,
                    deleteOnRestartManager);
                var packagesFolderPath = PackagesFolderPathUtility.GetPackagesFolderPath(testSolutionManager, testSettings);

                var msBuildNuGetProject = testSolutionManager.AddNewMSBuildProject();
                var msBuildNuGetProjectSystem = msBuildNuGetProject.ProjectSystem as TestMSBuildNuGetProjectSystem;
                var packagesConfigPath = msBuildNuGetProject.PackagesConfigNuGetProject.FullPath;
                var packageIdentity = _packageWithDeepDependency[6]; // WindowsAzure.Storage.4.3.0

                // Pre-Assert
                // Check that the packages.config file does not exist
                Assert.False(File.Exists(packagesConfigPath));
                // Check that there are no packages returned by PackagesConfigProject
                var packagesInPackagesConfig = (await msBuildNuGetProject.PackagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(0, packagesInPackagesConfig.Count);
                Assert.Equal(0, msBuildNuGetProjectSystem.References.Count);

                // Act
                await nuGetPackageManager.InstallPackageAsync(msBuildNuGetProject, packageIdentity,
                    new ResolutionContext(), new TestNuGetProjectContext(), sourceRepositoryProvider.GetRepositories().First(), null, token);

                // Assert
                // Check that the packages.config file exists after the installation
                Assert.True(File.Exists(packagesConfigPath));
                // Check the number of packages and packages returned by PackagesConfigProject after the installation
                packagesInPackagesConfig = (await msBuildNuGetProject.PackagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(7, packagesInPackagesConfig.Count);
                Assert.Equal(1, msBuildNuGetProjectSystem.BindingRedirectsCallCount);
            }
        }

        [Fact]
        public async Task InstallPackageBindingRedirectsDisabledWithDeepDependency()
        {
            // Arrange
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV3OnlySourceRepositoryProvider();
            using (var testSolutionManager = new TestSolutionManager())
            {
                var testSettings = NullSettings.Instance;
                var token = CancellationToken.None;
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    testSettings,
                    testSolutionManager,
                    deleteOnRestartManager);
                var packagesFolderPath = PackagesFolderPathUtility.GetPackagesFolderPath(testSolutionManager, testSettings);

                var msBuildNuGetProject = testSolutionManager.AddNewMSBuildProject();
                var msBuildNuGetProjectSystem = msBuildNuGetProject.ProjectSystem as TestMSBuildNuGetProjectSystem;
                var packagesConfigPath = msBuildNuGetProject.PackagesConfigNuGetProject.FullPath;
                var packageIdentity = _packageWithDeepDependency[6]; // WindowsAzure.Storage.4.3.0

                // Pre-Assert
                // Check that the packages.config file does not exist
                Assert.False(File.Exists(packagesConfigPath));
                // Check that there are no packages returned by PackagesConfigProject
                var packagesInPackagesConfig = (await msBuildNuGetProject.PackagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(0, packagesInPackagesConfig.Count);
                Assert.Equal(0, msBuildNuGetProjectSystem.References.Count);

                // Act
                await nuGetPackageManager.InstallPackageAsync(msBuildNuGetProject, packageIdentity,
                    new ResolutionContext(), new TestNuGetProjectContext { BindingRedirectsDisabled = true }, sourceRepositoryProvider.GetRepositories().First(), null, token);

                // Assert
                // Check that the packages.config file exists after the installation
                Assert.True(File.Exists(packagesConfigPath));
                // Check the number of packages and packages returned by PackagesConfigProject after the installation
                packagesInPackagesConfig = (await msBuildNuGetProject.PackagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(7, packagesInPackagesConfig.Count);
                Assert.Equal(0, msBuildNuGetProjectSystem.BindingRedirectsCallCount);
            }
        }

        [Fact]
        public Task GetInstalledPackagesByDependencyOrder()
        {
            return GetInstalledPackagesByDependencyOrderInternal(deletePackages: false);
        }

        [Fact]
        public Task GetUnrestoredPackagesByDependencyOrderDeleteTrue()
        {
            return GetInstalledPackagesByDependencyOrderInternal(deletePackages: true);
        }

        private async Task GetInstalledPackagesByDependencyOrderInternal(bool deletePackages)
        {
            // Arrange
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV3OnlySourceRepositoryProvider();
            using (var testSolutionManager = new TestSolutionManager())
            {
                var testSettings = NullSettings.Instance;
                var token = CancellationToken.None;
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    testSettings,
                    testSolutionManager,
                    deleteOnRestartManager);
                var packagesFolderPath = PackagesFolderPathUtility.GetPackagesFolderPath(testSolutionManager, testSettings);

                var msBuildNuGetProject = testSolutionManager.AddNewMSBuildProject();
                var msBuildNuGetProjectSystem = msBuildNuGetProject.ProjectSystem as TestMSBuildNuGetProjectSystem;
                var packagesConfigPath = msBuildNuGetProject.PackagesConfigNuGetProject.FullPath;
                var packageIdentity = _packageWithDeepDependency[6]; // WindowsAzure.Storage.4.3.0

                // Pre-Assert
                // Check that the packages.config file does not exist
                Assert.False(File.Exists(packagesConfigPath));
                // Check that there are no packages returned by PackagesConfigProject
                var packagesInPackagesConfig = (await msBuildNuGetProject.PackagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(0, packagesInPackagesConfig.Count);
                Assert.Equal(0, msBuildNuGetProjectSystem.References.Count);

                // Act
                await nuGetPackageManager.InstallPackageAsync(msBuildNuGetProject, packageIdentity,
                    new ResolutionContext(), new TestNuGetProjectContext(), sourceRepositoryProvider.GetRepositories().First(), null, token);

                if (deletePackages)
                {
                    TestFileSystemUtility.DeleteRandomTestFolder(packagesFolderPath);
                }

                // Assert
                // Check that the packages.config file exists after the installation
                Assert.True(File.Exists(packagesConfigPath));
                // Check the number of packages and packages returned by PackagesConfigProject after the installation
                packagesInPackagesConfig = (await msBuildNuGetProject.PackagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(7, packagesInPackagesConfig.Count);
                var installedPackages = _packageWithDeepDependency.OrderBy(f => f.Id).ToList();
                for (var i = 0; i < 7; i++)
                {
                    Assert.True(installedPackages[i].Equals(packagesInPackagesConfig[i].PackageIdentity));
                    Assert.Equal(msBuildNuGetProject.ProjectSystem.TargetFramework, packagesInPackagesConfig[i].TargetFramework);
                }

                // Main Assert
                var installedPackagesInDependencyOrder = (await nuGetPackageManager.GetInstalledPackagesInDependencyOrder
                    (msBuildNuGetProject, token)).ToList();
                if (deletePackages)
                {
                    Assert.Equal(0, installedPackagesInDependencyOrder.Count);
                }
                else
                {
                    Assert.Equal(7, installedPackagesInDependencyOrder.Count);
                    for (var i = 0; i < 7; i++)
                    {
                        Assert.Equal(_packageWithDeepDependency[i], installedPackagesInDependencyOrder[i], PackageIdentity.Comparer);
                    }
                }
            }
        }

        [Fact(Skip = "Test is dependent on latest nuget.org packages, which means it's really difficult to get them to work.")]
        public async Task InstallPackageTargetingASPNetCore50()
        {
            // Arrange
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV3OnlySourceRepositoryProvider();
            using (var testSolutionManager = new TestSolutionManager())
            {
                var testSettings = NullSettings.Instance;
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var token = CancellationToken.None;
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    testSettings,
                    testSolutionManager,
                    deleteOnRestartManager);
                var packagesFolderPath = PackagesFolderPathUtility.GetPackagesFolderPath(testSolutionManager, testSettings);

                var msBuildNuGetProject = testSolutionManager.AddNewMSBuildProject("projectName", NuGetFramework.Parse("aspenetcore50"));
                var msBuildNuGetProjectSystem = msBuildNuGetProject.ProjectSystem as TestMSBuildNuGetProjectSystem;
                var packagesConfigPath = msBuildNuGetProject.PackagesConfigNuGetProject.FullPath;
                var packageIdentity = _latestAspNetPackages[0]; // Microsoft.AspNet.Mvc.6.0.0-beta3

                // Pre-Assert
                // Check that the packages.config file does not exist
                Assert.False(File.Exists(packagesConfigPath));
                // Check that there are no packages returned by PackagesConfigProject
                var packagesInPackagesConfig = (await msBuildNuGetProject.PackagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(0, packagesInPackagesConfig.Count);
                Assert.Equal(0, msBuildNuGetProjectSystem.References.Count);

                var resolutionContext = new ResolutionContext(DependencyBehavior.Lowest, includePrelease: true, includeUnlisted: true, versionConstraints: VersionConstraints.None);
                // Act
                await nuGetPackageManager.InstallPackageAsync(msBuildNuGetProject, packageIdentity,
                    resolutionContext, new TestNuGetProjectContext(), sourceRepositoryProvider.GetRepositories().First(), null, token);

                // Assert
                // Check that the packages.config file exists after the installation
                Assert.True(File.Exists(packagesConfigPath));
                // Check the number of packages and packages returned by PackagesConfigProject after the installation
                packagesInPackagesConfig = (await msBuildNuGetProject.PackagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(1, packagesInPackagesConfig.Count);
                Assert.Equal(packageIdentity, packagesInPackagesConfig[0].PackageIdentity);
                Assert.Equal(msBuildNuGetProject.ProjectSystem.TargetFramework, packagesInPackagesConfig[0].TargetFramework);
            }
        }

        [Fact]
        public async Task InstallMvcTargetingNet45()
        {
            // Arrange
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV3OnlySourceRepositoryProvider();
            using (var testSolutionManager = new TestSolutionManager())
            {
                var testSettings = NullSettings.Instance;
                var token = CancellationToken.None;
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    testSettings,
                    testSolutionManager,
                    deleteOnRestartManager);
                var packagesFolderPath = PackagesFolderPathUtility.GetPackagesFolderPath(testSolutionManager, testSettings);

                var msBuildNuGetProject = testSolutionManager.AddNewMSBuildProject();
                var msBuildNuGetProjectSystem = msBuildNuGetProject.ProjectSystem as TestMSBuildNuGetProjectSystem;
                var packagesConfigPath = msBuildNuGetProject.PackagesConfigNuGetProject.FullPath;
                var packageIdentity = _latestAspNetPackages[0]; // Microsoft.AspNet.Mvc.6.0.0-beta3

                // Pre-Assert
                // Check that the packages.config file does not exist
                Assert.False(File.Exists(packagesConfigPath));
                // Check that there are no packages returned by PackagesConfigProject
                var packagesInPackagesConfig = (await msBuildNuGetProject.PackagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(0, packagesInPackagesConfig.Count);
                Assert.Equal(0, msBuildNuGetProjectSystem.References.Count);

                var resolutionContext = new ResolutionContext(DependencyBehavior.Lowest, includePrelease: true, includeUnlisted: true, versionConstraints: VersionConstraints.None);

                // Act
                var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                    async () => await nuGetPackageManager.InstallPackageAsync(
                        msBuildNuGetProject, packageIdentity, resolutionContext, new TestNuGetProjectContext(), sourceRepositoryProvider.GetRepositories().First(), null, token));

                var errorMessage = string.Format(CultureInfo.CurrentCulture,
                    "Could not install package '{0}'. You are trying to install this package into a project that targets '{1}', but the package does not contain any assembly references or content files that are compatible with that framework. For more information, contact the package author.",
                    packageIdentity.Id + " " + packageIdentity.Version.ToNormalizedString(), msBuildNuGetProject.ProjectSystem.TargetFramework);
                Assert.Equal(errorMessage, exception.Message);

            }
        }

        [Fact]
        public async Task ReinstallPackages()
        {
            // Arrange
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV2OnlySourceRepositoryProvider();
            using (var testSolutionManager = new TestSolutionManager())
            {
                var testSettings = NullSettings.Instance;
                var token = CancellationToken.None;
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    testSettings,
                    testSolutionManager,
                    deleteOnRestartManager);
                var packagesFolderPath = PackagesFolderPathUtility.GetPackagesFolderPath(testSolutionManager, testSettings);
                var packagePathResolver = new PackagePathResolver(packagesFolderPath);

                var msBuildNuGetProject = testSolutionManager.AddNewMSBuildProject();
                var msBuildNuGetProjectSystem = msBuildNuGetProject.ProjectSystem as TestMSBuildNuGetProjectSystem;
                var packagesConfigPath = msBuildNuGetProject.PackagesConfigNuGetProject.FullPath;
                var folderNuGetProject = msBuildNuGetProject.FolderNuGetProject;
                var packageIdentity = _morePackageWithDependents[3]; // Microsoft.Net.Http.2.2.22

                // Pre-Assert
                // Check that the packages.config file does not exist
                Assert.False(File.Exists(packagesConfigPath));
                // Check that there are no packages returned by PackagesConfigProject
                var packagesInPackagesConfig = (await msBuildNuGetProject.PackagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(0, packagesInPackagesConfig.Count);
                Assert.Equal(0, msBuildNuGetProjectSystem.References.Count);

                // Act
                await nuGetPackageManager.InstallPackageAsync(msBuildNuGetProject, packageIdentity,
                    new ResolutionContext(), new TestNuGetProjectContext(), sourceRepositoryProvider.GetRepositories().First(), null, token);

                // Assert
                // Check that the packages.config file exists after the installation
                Assert.True(File.Exists(packagesConfigPath));
                // Check the number of packages and packages returned by PackagesConfigProject after the installation
                packagesInPackagesConfig = (await msBuildNuGetProject.PackagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(3, packagesInPackagesConfig.Count);
                Assert.Equal(packageIdentity, packagesInPackagesConfig[2].PackageIdentity);
                Assert.Equal(msBuildNuGetProject.ProjectSystem.TargetFramework, packagesInPackagesConfig[2].TargetFramework);
                Assert.Equal(_morePackageWithDependents[0], packagesInPackagesConfig[1].PackageIdentity);
                Assert.Equal(msBuildNuGetProject.ProjectSystem.TargetFramework, packagesInPackagesConfig[1].TargetFramework);
                Assert.Equal(_morePackageWithDependents[2], packagesInPackagesConfig[0].PackageIdentity);
                Assert.Equal(msBuildNuGetProject.ProjectSystem.TargetFramework, packagesInPackagesConfig[0].TargetFramework);
                var installedPackageIdentities = (await msBuildNuGetProject.GetInstalledPackagesAsync(token))
                    .Select(pr => pr.PackageIdentity);

                var resolutionContext = new ResolutionContext(
                    DependencyBehavior.Highest,
                    false,
                    true,
                    VersionConstraints.ExactMajor | VersionConstraints.ExactMinor | VersionConstraints.ExactPatch | VersionConstraints.ExactRelease);

                // Act
                var packageActions = (await nuGetPackageManager.PreviewUpdatePackagesAsync(
                    new List<NuGetProject> { msBuildNuGetProject },
                    resolutionContext,
                    new TestNuGetProjectContext(),
                    sourceRepositoryProvider.GetRepositories(),
                    sourceRepositoryProvider.GetRepositories(),
                    token)).ToList();

                // Assert
                var singlePackageSource = sourceRepositoryProvider.GetRepositories().Single().PackageSource.Source;
                Assert.Equal(6, packageActions.Count);
                Assert.True(_morePackageWithDependents[3].Equals(packageActions[0].PackageIdentity));
                Assert.Equal(NuGetProjectActionType.Uninstall, packageActions[0].NuGetProjectActionType);
                Assert.True(_morePackageWithDependents[2].Equals(packageActions[1].PackageIdentity));
                Assert.Equal(NuGetProjectActionType.Uninstall, packageActions[1].NuGetProjectActionType);
                Assert.True(_morePackageWithDependents[0].Equals(packageActions[2].PackageIdentity));
                Assert.Equal(NuGetProjectActionType.Uninstall, packageActions[2].NuGetProjectActionType);

                Assert.True(_morePackageWithDependents[0].Equals(packageActions[3].PackageIdentity));
                Assert.Equal(NuGetProjectActionType.Install, packageActions[3].NuGetProjectActionType);
                Assert.Equal(singlePackageSource, packageActions[3].SourceRepository.PackageSource.Source);
                Assert.True(_morePackageWithDependents[2].Equals(packageActions[4].PackageIdentity));
                Assert.Equal(NuGetProjectActionType.Install, packageActions[4].NuGetProjectActionType);
                Assert.Equal(singlePackageSource, packageActions[4].SourceRepository.PackageSource.Source);
                Assert.True(_morePackageWithDependents[3].Equals(packageActions[5].PackageIdentity));
                Assert.Equal(NuGetProjectActionType.Install, packageActions[5].NuGetProjectActionType);
                Assert.Equal(singlePackageSource, packageActions[5].SourceRepository.PackageSource.Source);

                // Main Act
                await nuGetPackageManager.ExecuteNuGetProjectActionsAsync(msBuildNuGetProject, packageActions, new TestNuGetProjectContext(), NullSourceCacheContext.Instance, token);

                // Check that the packages.config file exists after the installation
                Assert.True(File.Exists(packagesConfigPath));
                // Check the number of packages and packages returned by PackagesConfigProject after the installation
                packagesInPackagesConfig = (await msBuildNuGetProject.PackagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(3, packagesInPackagesConfig.Count);
                Assert.Equal(packageIdentity, packagesInPackagesConfig[2].PackageIdentity);
                Assert.Equal(msBuildNuGetProject.ProjectSystem.TargetFramework, packagesInPackagesConfig[2].TargetFramework);
                Assert.Equal(_morePackageWithDependents[0], packagesInPackagesConfig[1].PackageIdentity);
                Assert.Equal(msBuildNuGetProject.ProjectSystem.TargetFramework, packagesInPackagesConfig[1].TargetFramework);
                Assert.Equal(_morePackageWithDependents[2], packagesInPackagesConfig[0].PackageIdentity);
                Assert.Equal(msBuildNuGetProject.ProjectSystem.TargetFramework, packagesInPackagesConfig[0].TargetFramework);
                Assert.True(File.Exists(folderNuGetProject.GetInstalledPackageFilePath(packageIdentity)));
                Assert.True(File.Exists(folderNuGetProject.GetInstalledPackageFilePath(_morePackageWithDependents[0])));
                Assert.True(File.Exists(folderNuGetProject.GetInstalledPackageFilePath(_morePackageWithDependents[2])));
            }
        }

        [Fact]
        public async Task ReinstallSpecificPackage()
        {
            // Arrange

            // Set up Package Source
            var packages = new List<SourcePackageDependencyInfo>
            {
                new SourcePackageDependencyInfo("a", new NuGetVersion(1, 0, 0), new PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("a", new NuGetVersion(2, 0, 0), new PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("a", new NuGetVersion(3, 0, 0), new PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("b", new NuGetVersion(1, 0, 0), new[]
                {
                    new PackageDependency("a", new VersionRange(new NuGetVersion(1, 0, 0))),
                    new PackageDependency("c", new VersionRange(new NuGetVersion(2, 0, 0))),
                }, true, null),
                new SourcePackageDependencyInfo("b", new NuGetVersion(2, 0, 0), new[]
                {
                    new PackageDependency("a", new VersionRange(new NuGetVersion(2, 0, 0))),
                    new PackageDependency("c", new VersionRange(new NuGetVersion(2, 0, 0))),
                }, true, null),
                new SourcePackageDependencyInfo("b", new NuGetVersion(3, 0, 0), new[]
                {
                    new PackageDependency("a", new VersionRange(new NuGetVersion(3, 0, 0))),
                }, true, null),
                new SourcePackageDependencyInfo("c", new NuGetVersion(1, 0, 0), new[]
                {
                    new PackageDependency("d", new VersionRange(new NuGetVersion(2, 0, 0))),
                }, true, null),
                new SourcePackageDependencyInfo("c", new NuGetVersion(2, 0, 0), new[]
                {
                    new PackageDependency("d", new VersionRange(new NuGetVersion(2, 0, 0))),
                }, true, null),
                new SourcePackageDependencyInfo("c", new NuGetVersion(3, 0, 0), new PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("d", new NuGetVersion(1, 0, 0), new PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("d", new NuGetVersion(2, 0, 0), new PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("d", new NuGetVersion(3, 0, 0), new PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("d", new NuGetVersion(4, 0, 0), new PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("e", new NuGetVersion(1, 0, 0), new PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("e", new NuGetVersion(2, 0, 0), new PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("f", new NuGetVersion(1, 0, 0), new PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("f", new NuGetVersion(2, 0, 0), new PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("f", new NuGetVersion(3, 0, 0), new PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("f", new NuGetVersion(4, 0, 0), new PackageDependency[] { }, true, null),
            };

            var sourceRepositoryProvider = CreateSource(packages);

            // Set up NuGetProject
            var fwk45 = NuGetFramework.Parse("net45");

            var installedPackages = new List<PackageReference>
            {
                new PackageReference(new PackageIdentity("a", new NuGetVersion(1, 0, 0)), fwk45, true),
                new PackageReference(new PackageIdentity("b", new NuGetVersion(1, 0, 0)), fwk45, true),
                new PackageReference(new PackageIdentity("c", new NuGetVersion(2, 0, 0)), fwk45, true),
                new PackageReference(new PackageIdentity("d", new NuGetVersion(2, 0, 0)), fwk45, true),
                new PackageReference(new PackageIdentity("e", new NuGetVersion(1, 0, 0)), fwk45, true),
                new PackageReference(new PackageIdentity("f", new NuGetVersion(3, 0, 0)), fwk45, true),
            };

            var nuGetProject = new TestNuGetProject(installedPackages);

            // Create Package Manager

            using (var solutionManager = new TestSolutionManager())
            {
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    NullSettings.Instance,
                    solutionManager,
                    new TestDeleteOnRestartManager());

                // Main Act

                var resolutionContext = new ResolutionContext(
                    DependencyBehavior.Highest,
                    false,
                    true,
                    VersionConstraints.ExactMajor | VersionConstraints.ExactMinor | VersionConstraints.ExactPatch | VersionConstraints.ExactRelease);

                var result = await nuGetPackageManager.PreviewUpdatePackagesAsync(
                    "b",
                    new List<NuGetProject> { nuGetProject },
                    resolutionContext,
                    new TestNuGetProjectContext(),
                    sourceRepositoryProvider.GetRepositories(),
                    sourceRepositoryProvider.GetRepositories(),
                    CancellationToken.None);

                // Assert
                var resulting = result.Select(a => Tuple.Create(a.PackageIdentity, a.NuGetProjectActionType)).ToArray();

                var expected = new List<Tuple<PackageIdentity, NuGetProjectActionType>>();
                Expected(expected, "a", new NuGetVersion(1, 0, 0), new NuGetVersion(1, 0, 0));
                Expected(expected, "b", new NuGetVersion(1, 0, 0), new NuGetVersion(1, 0, 0));
                Expected(expected, "c", new NuGetVersion(2, 0, 0), new NuGetVersion(2, 0, 0));
                Expected(expected, "d", new NuGetVersion(2, 0, 0), new NuGetVersion(2, 0, 0));
                // note e and f are not touched

                Assert.True(Compare(resulting, expected));
            }
        }

        [Fact]
        public async Task UpdateDependencyToPrereleaseVersion()
        {
            // Arrange
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV3OnlySourceRepositoryProvider();
            using (var testSolutionManager = new TestSolutionManager())
            {
                var testSettings = NullSettings.Instance;
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var token = CancellationToken.None;
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    testSettings,
                    testSolutionManager,
                    deleteOnRestartManager);
                var packagesFolderPath = PackagesFolderPathUtility.GetPackagesFolderPath(testSolutionManager, testSettings);

                var primarySourceRepository = sourceRepositoryProvider.GetRepositories().First();
                var msBuildNuGetProject = testSolutionManager.AddNewMSBuildProject();
                var msBuildNuGetProjectSystem = msBuildNuGetProject.ProjectSystem as TestMSBuildNuGetProjectSystem;
                var packagesConfigPath = msBuildNuGetProject.PackagesConfigNuGetProject.FullPath;
                var webgreasePackageIdentity = new PackageIdentity("WebGrease", new NuGetVersion("1.6.0"));
                var resolutionContext = new ResolutionContext(DependencyBehavior.Lowest, includePrelease: true, includeUnlisted: true, versionConstraints: VersionConstraints.None);

                var newtonsoftJsonPackageId = "newtonsoft.json";

                // Act
                var resolvedPackage = await NuGetPackageManager.GetLatestVersionAsync(
                    newtonsoftJsonPackageId,
                    msBuildNuGetProject,
                    resolutionContext,
                    primarySourceRepository,
                    NullLogger.Instance,
                    CancellationToken.None);

                var newtonsoftJsonLatestPrereleasePackageIdentity = new PackageIdentity(newtonsoftJsonPackageId, resolvedPackage.LatestVersion);

                await nuGetPackageManager.InstallPackageAsync(msBuildNuGetProject, webgreasePackageIdentity, resolutionContext,
                    new TestNuGetProjectContext(), primarySourceRepository, null, CancellationToken.None);

                // Assert
                // Check that the packages.config file exists after the installation
                Assert.True(File.Exists(packagesConfigPath));
                // Check the number of packages and packages returned by PackagesConfigProject after the installation
                var packagesInPackagesConfig = (await msBuildNuGetProject.PackagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(3, packagesInPackagesConfig.Count);

                // Main Act - Update newtonsoft.json to latest pre-release
                await nuGetPackageManager.InstallPackageAsync(msBuildNuGetProject, newtonsoftJsonLatestPrereleasePackageIdentity, resolutionContext,
                    new TestNuGetProjectContext(), primarySourceRepository, null, CancellationToken.None);
            }
        }

        [Fact]
        public async Task InstallAspNetRazorJa()
        {
            // Arrange
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV3OnlySourceRepositoryProvider();
            using (var testSolutionManager = new TestSolutionManager())
            {
                var testSettings = NullSettings.Instance;
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var token = CancellationToken.None;
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    testSettings,
                    testSolutionManager,
                    deleteOnRestartManager);
                var packagesFolderPath = PackagesFolderPathUtility.GetPackagesFolderPath(testSolutionManager, testSettings);

                var msBuildNuGetProject = testSolutionManager.AddNewMSBuildProject();
                var msBuildNuGetProjectSystem = msBuildNuGetProject.ProjectSystem as TestMSBuildNuGetProjectSystem;
                var packagesConfigPath = msBuildNuGetProject.PackagesConfigNuGetProject.FullPath;
                var aspnetrazorjaPackageIdentity = new PackageIdentity("Microsoft.AspNet.Razor.ja", new NuGetVersion("3.2.3"));

                // Pre-Assert
                // Check that the packages.config file does not exist
                Assert.False(File.Exists(packagesConfigPath));
                // Check that there are no packages returned by PackagesConfigProject
                var packagesInPackagesConfig = (await msBuildNuGetProject.PackagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(0, packagesInPackagesConfig.Count);
                Assert.Equal(0, msBuildNuGetProjectSystem.References.Count);

                // Act
                await nuGetPackageManager.InstallPackageAsync(msBuildNuGetProject, aspnetrazorjaPackageIdentity,
                    new ResolutionContext(), new TestNuGetProjectContext(), sourceRepositoryProvider.GetRepositories().First(), null, token);

                // Assert
                // Check that the packages.config file exists after the installation
                Assert.True(File.Exists(packagesConfigPath));
                // Check the number of packages and packages returned by PackagesConfigProject after the installation
                packagesInPackagesConfig = (await msBuildNuGetProject.PackagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(2, packagesInPackagesConfig.Count);
                Assert.Equal(aspnetrazorjaPackageIdentity, packagesInPackagesConfig[1].PackageIdentity);
                Assert.Equal(msBuildNuGetProject.ProjectSystem.TargetFramework, packagesInPackagesConfig[1].TargetFramework);
            }
        }

        [Fact]
        public async Task InstallMicrosoftWebInfrastructure1000FromV2()
        {
            // Arrange
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV2OnlySourceRepositoryProvider();
            using (var testSolutionManager = new TestSolutionManager())
            {
                var testSettings = NullSettings.Instance;
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var token = CancellationToken.None;
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    testSettings,
                    testSolutionManager,
                    deleteOnRestartManager);
                var packagesFolderPath = PackagesFolderPathUtility.GetPackagesFolderPath(testSolutionManager, testSettings);

                var msBuildNuGetProject = testSolutionManager.AddNewMSBuildProject();
                var msBuildNuGetProjectSystem = msBuildNuGetProject.ProjectSystem as TestMSBuildNuGetProjectSystem;
                var packagesConfigPath = msBuildNuGetProject.PackagesConfigNuGetProject.FullPath;
                var version = new NuGetVersion("1.0.0.0");
                var microsoftWebInfrastructurePackageIdentity = new PackageIdentity("Microsoft.Web.Infrastructure", version);

                // Pre-Assert
                // Check that the packages.config file does not exist
                Assert.False(File.Exists(packagesConfigPath));
                // Check that there are no packages returned by PackagesConfigProject
                var packagesInPackagesConfig = (await msBuildNuGetProject.PackagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(0, packagesInPackagesConfig.Count);
                Assert.Equal(0, msBuildNuGetProjectSystem.References.Count);

                // Act
                await nuGetPackageManager.InstallPackageAsync(msBuildNuGetProject, microsoftWebInfrastructurePackageIdentity,
                    new ResolutionContext(), new TestNuGetProjectContext(), sourceRepositoryProvider.GetRepositories().First(), null, token);

                // Assert
                // Check that the packages.config file exists after the installation
                Assert.True(File.Exists(packagesConfigPath));
                // Check the number of packages and packages returned by PackagesConfigProject after the installation
                packagesInPackagesConfig = (await msBuildNuGetProject.PackagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(1, packagesInPackagesConfig.Count);
                Assert.Equal(microsoftWebInfrastructurePackageIdentity, packagesInPackagesConfig[0].PackageIdentity);
                Assert.Equal(msBuildNuGetProject.ProjectSystem.TargetFramework, packagesInPackagesConfig[0].TargetFramework);

                var microsoftWebInfrastructure1000FolderPath = Path.Combine(packagesFolderPath, "Microsoft.Web.Infrastructure.1.0.0.0");
                Assert.True(Directory.Exists(microsoftWebInfrastructure1000FolderPath));
            }
        }

        [Fact]
        public async Task InstallMicrosoftWebInfrastructure1000FromV3()
        {
            // Arrange
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV3OnlySourceRepositoryProvider();
            using (var testSolutionManager = new TestSolutionManager())
            {
                var testSettings = NullSettings.Instance;
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var token = CancellationToken.None;
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    testSettings,
                    testSolutionManager,
                    deleteOnRestartManager);
                var packagesFolderPath = PackagesFolderPathUtility.GetPackagesFolderPath(testSolutionManager, testSettings);

                var msBuildNuGetProject = testSolutionManager.AddNewMSBuildProject();
                var msBuildNuGetProjectSystem = msBuildNuGetProject.ProjectSystem as TestMSBuildNuGetProjectSystem;
                var packagesConfigPath = msBuildNuGetProject.PackagesConfigNuGetProject.FullPath;
                var version = new NuGetVersion("1.0.0.0");
                var microsoftWebInfrastructurePackageIdentity = new PackageIdentity("Microsoft.Web.Infrastructure", version);

                // Pre-Assert
                // Check that the packages.config file does not exist
                Assert.False(File.Exists(packagesConfigPath));
                // Check that there are no packages returned by PackagesConfigProject
                var packagesInPackagesConfig = (await msBuildNuGetProject.PackagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(0, packagesInPackagesConfig.Count);
                Assert.Equal(0, msBuildNuGetProjectSystem.References.Count);

                // Act
                await nuGetPackageManager.InstallPackageAsync(msBuildNuGetProject, microsoftWebInfrastructurePackageIdentity,
                    new ResolutionContext(), new TestNuGetProjectContext(), sourceRepositoryProvider.GetRepositories().First(), null, token);

                // Assert
                // Check that the packages.config file exists after the installation
                Assert.True(File.Exists(packagesConfigPath));
                // Check the number of packages and packages returned by PackagesConfigProject after the installation
                packagesInPackagesConfig = (await msBuildNuGetProject.PackagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(1, packagesInPackagesConfig.Count);
                Assert.Equal(microsoftWebInfrastructurePackageIdentity, packagesInPackagesConfig[0].PackageIdentity);
                Assert.Equal(msBuildNuGetProject.ProjectSystem.TargetFramework, packagesInPackagesConfig[0].TargetFramework);

                var microsoftWebInfrastructure1000FolderPath = Path.Combine(packagesFolderPath, "Microsoft.Web.Infrastructure.1.0.0.0");
                Assert.True(Directory.Exists(microsoftWebInfrastructure1000FolderPath));
            }
        }

        [Fact]
        public async Task InstallElmah11FromV2()
        {
            // Arrange
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV2OnlySourceRepositoryProvider();
            using (var testSolutionManager = new TestSolutionManager())
            {
                var testSettings = NullSettings.Instance;
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var token = CancellationToken.None;
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    testSettings,
                    testSolutionManager,
                    deleteOnRestartManager);
                var packagesFolderPath = PackagesFolderPathUtility.GetPackagesFolderPath(testSolutionManager, testSettings);

                var msBuildNuGetProject = testSolutionManager.AddNewMSBuildProject();
                var msBuildNuGetProjectSystem = msBuildNuGetProject.ProjectSystem as TestMSBuildNuGetProjectSystem;
                var packagesConfigPath = msBuildNuGetProject.PackagesConfigNuGetProject.FullPath;
                var version = new NuGetVersion("1.1");
                var elmahPackageIdentity = new PackageIdentity("elmah", version);

                // Pre-Assert
                // Check that the packages.config file does not exist
                Assert.False(File.Exists(packagesConfigPath));
                // Check that there are no packages returned by PackagesConfigProject
                var packagesInPackagesConfig = (await msBuildNuGetProject.PackagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(0, packagesInPackagesConfig.Count);
                Assert.Equal(0, msBuildNuGetProjectSystem.References.Count);

                // Act
                await nuGetPackageManager.InstallPackageAsync(msBuildNuGetProject, elmahPackageIdentity,
                    new ResolutionContext(), new TestNuGetProjectContext(), sourceRepositoryProvider.GetRepositories().First(), null, token);

                // Assert
                // Check that the packages.config file exists after the installation
                Assert.True(File.Exists(packagesConfigPath));
                // Check the number of packages and packages returned by PackagesConfigProject after the installation
                packagesInPackagesConfig = (await msBuildNuGetProject.PackagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(1, packagesInPackagesConfig.Count);
                Assert.Equal(elmahPackageIdentity, packagesInPackagesConfig[0].PackageIdentity);
                Assert.Equal(msBuildNuGetProject.ProjectSystem.TargetFramework, packagesInPackagesConfig[0].TargetFramework);

                var microsoftWebInfrastructure1000FolderPath = Path.Combine(packagesFolderPath, "elmah.1.1");
                Assert.True(Directory.Exists(microsoftWebInfrastructure1000FolderPath));
            }
        }

        [Fact]
        public async Task InstallElmah11FromV3()
        {
            // Arrange
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV3OnlySourceRepositoryProvider();
            using (var testSolutionManager = new TestSolutionManager())
            {
                var testSettings = NullSettings.Instance;
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var token = CancellationToken.None;
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    testSettings,
                    testSolutionManager,
                    deleteOnRestartManager);
                var packagesFolderPath = PackagesFolderPathUtility.GetPackagesFolderPath(testSolutionManager, testSettings);

                var msBuildNuGetProject = testSolutionManager.AddNewMSBuildProject();
                var msBuildNuGetProjectSystem = msBuildNuGetProject.ProjectSystem as TestMSBuildNuGetProjectSystem;
                var packagesConfigPath = msBuildNuGetProject.PackagesConfigNuGetProject.FullPath;
                var version = new NuGetVersion("1.1");
                var elmahPackageIdentity = new PackageIdentity("elmah", version);

                // Pre-Assert
                // Check that the packages.config file does not exist
                Assert.False(File.Exists(packagesConfigPath));
                // Check that there are no packages returned by PackagesConfigProject
                var packagesInPackagesConfig = (await msBuildNuGetProject.PackagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(0, packagesInPackagesConfig.Count);
                Assert.Equal(0, msBuildNuGetProjectSystem.References.Count);

                // Act
                await nuGetPackageManager.InstallPackageAsync(msBuildNuGetProject, elmahPackageIdentity,
                    new ResolutionContext(), new TestNuGetProjectContext(), sourceRepositoryProvider.GetRepositories().First(), null, token);

                // Assert
                // Check that the packages.config file exists after the installation
                Assert.True(File.Exists(packagesConfigPath));
                // Check the number of packages and packages returned by PackagesConfigProject after the installation
                packagesInPackagesConfig = (await msBuildNuGetProject.PackagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(1, packagesInPackagesConfig.Count);
                Assert.Equal(elmahPackageIdentity, packagesInPackagesConfig[0].PackageIdentity);
                Assert.Equal(msBuildNuGetProject.ProjectSystem.TargetFramework, packagesInPackagesConfig[0].TargetFramework);

                var microsoftWebInfrastructure1000FolderPath = Path.Combine(packagesFolderPath, "elmah.1.1");
                Assert.True(Directory.Exists(microsoftWebInfrastructure1000FolderPath));
            }
        }

        [Fact]
        public async Task Install_SharpDX_DXGI_v263_WithNonReferencesInLibFolder()
        {
            // Arrange
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV3OnlySourceRepositoryProvider();
            using (var testSolutionManager = new TestSolutionManager())
            {
                var testSettings = NullSettings.Instance;
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var token = CancellationToken.None;
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    testSettings,
                    testSolutionManager,
                    deleteOnRestartManager);
                var packagesFolderPath = PackagesFolderPathUtility.GetPackagesFolderPath(testSolutionManager, testSettings);

                var msBuildNuGetProject = testSolutionManager.AddNewMSBuildProject();
                var msBuildNuGetProjectSystem = msBuildNuGetProject.ProjectSystem as TestMSBuildNuGetProjectSystem;
                var packagesConfigPath = msBuildNuGetProject.PackagesConfigNuGetProject.FullPath;
                var version = new NuGetVersion("2.6.3");
                var sharpDXDXGIv263Package = new PackageIdentity("SharpDX.DXGI", version);

                // Pre-Assert
                // Check that the packages.config file does not exist
                Assert.False(File.Exists(packagesConfigPath));
                // Check that there are no packages returned by PackagesConfigProject
                var packagesInPackagesConfig = (await msBuildNuGetProject.PackagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(0, packagesInPackagesConfig.Count);
                Assert.Equal(0, msBuildNuGetProjectSystem.References.Count);

                // Act
                await nuGetPackageManager.InstallPackageAsync(msBuildNuGetProject, sharpDXDXGIv263Package,
                    new ResolutionContext(), new TestNuGetProjectContext(), sourceRepositoryProvider.GetRepositories().First(), null, token);

                // Assert
                // Check that the packages.config file exists after the installation
                Assert.True(File.Exists(packagesConfigPath));
                // Check the number of packages and packages returned by PackagesConfigProject after the installation
                packagesInPackagesConfig = (await msBuildNuGetProject.PackagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(2, packagesInPackagesConfig.Count);
                Assert.Contains(packagesInPackagesConfig, p => p.PackageIdentity.Equals(sharpDXDXGIv263Package));
            }
        }

        [Fact]
        public async Task InstallPackageUnlistedFromV3()
        {
            // Arrange
            var packages = new List<SourcePackageDependencyInfo>
            {
                new SourcePackageDependencyInfo("a", new NuGetVersion(1, 0, 0), new[] { new PackageDependency("b", new VersionRange(new NuGetVersion(1, 0, 0))) }, true, null),
                new SourcePackageDependencyInfo("b", new NuGetVersion(1, 0, 0), new PackageDependency[] { }, false, null),
                new SourcePackageDependencyInfo("b", new NuGetVersion(2, 0, 0), new PackageDependency[] { }, false, null),
                new SourcePackageDependencyInfo("b", new NuGetVersion(3, 0, 0), new PackageDependency[] { }, true, null),
            };

            var resourceProviders = new List<Lazy<INuGetResourceProvider>>();
            resourceProviders.Add(new Lazy<INuGetResourceProvider>(() => new TestDependencyInfoProvider(packages)));
            resourceProviders.Add(new Lazy<INuGetResourceProvider>(() => new TestMetadataProvider(packages)));

            var packageSource = new PackageSource("http://a");
            var packageSourceProvider = new TestPackageSourceProvider(new[] { packageSource });

            var sourceRepositoryProvider = new SourceRepositoryProvider(packageSourceProvider, resourceProviders);

            using (var testSolutionManager = new TestSolutionManager())
            {
                var testSettings = NullSettings.Instance;
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var token = CancellationToken.None;
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    testSettings,
                    testSolutionManager,
                    deleteOnRestartManager);
                var packagesFolderPath = PackagesFolderPathUtility.GetPackagesFolderPath(testSolutionManager, testSettings);

                var msBuildNuGetProject = testSolutionManager.AddNewMSBuildProject();
                var msBuildNuGetProjectSystem = msBuildNuGetProject.ProjectSystem as TestMSBuildNuGetProjectSystem;
                var packagesConfigPath = msBuildNuGetProject.PackagesConfigNuGetProject.FullPath;
                var target = "a";

                // Pre-Assert
                // Check that the packages.config file does not exist
                Assert.False(File.Exists(packagesConfigPath));
                // Check that there are no packages returned by PackagesConfigProject
                var packagesInPackagesConfig = (await msBuildNuGetProject.PackagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(0, packagesInPackagesConfig.Count);
                Assert.Equal(0, msBuildNuGetProjectSystem.References.Count);

                // Act
                var nugetProjectActions = await nuGetPackageManager.PreviewInstallPackageAsync(msBuildNuGetProject, target,
                    new ResolutionContext(), new TestNuGetProjectContext(), sourceRepositoryProvider.GetRepositories().First(), null, token);

                var result = nugetProjectActions.ToList();

                var resultIdentities = result.Select(p => p.PackageIdentity);

                Assert.Contains(new PackageIdentity("a", new NuGetVersion(1, 0, 0)), resultIdentities);
                Assert.Contains(new PackageIdentity("b", new NuGetVersion(3, 0, 0)), resultIdentities);

                //  and all the actions are Install
                foreach (var nugetProjectAction in result)
                {
                    Assert.Equal(nugetProjectAction.NuGetProjectActionType, NuGetProjectActionType.Install);
                }
            }
        }

        [Fact]
        public async Task InstallPackageListedFromV3()
        {
            // Arrange
            var packages = new List<SourcePackageDependencyInfo>
            {
                new SourcePackageDependencyInfo("a", new NuGetVersion(1, 0, 0), new[] { new PackageDependency("b", new VersionRange(new NuGetVersion(1, 0, 0))) }, true, null),
                new SourcePackageDependencyInfo("a", new NuGetVersion(2, 0, 0), new[] { new PackageDependency("b", new VersionRange(new NuGetVersion(1, 0, 0))) }, true, null),
                new SourcePackageDependencyInfo("b", new NuGetVersion(1, 0, 0), new PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("b", new NuGetVersion(2, 0, 0), new PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("b", new NuGetVersion(3, 0, 0), new PackageDependency[] { }, true, null),
            };

            var resourceProviders = new List<Lazy<INuGetResourceProvider>>();
            resourceProviders.Add(new Lazy<INuGetResourceProvider>(() => new TestDependencyInfoProvider(packages)));
            resourceProviders.Add(new Lazy<INuGetResourceProvider>(() => new TestMetadataProvider(packages)));

            var packageSource = new PackageSource("http://a");
            var packageSourceProvider = new TestPackageSourceProvider(new[] { packageSource });

            var sourceRepositoryProvider = new SourceRepositoryProvider(packageSourceProvider, resourceProviders);

            using (var testSolutionManager = new TestSolutionManager())
            {
                var testSettings = NullSettings.Instance;
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var token = CancellationToken.None;
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    testSettings,
                    testSolutionManager,
                    deleteOnRestartManager);
                var packagesFolderPath = PackagesFolderPathUtility.GetPackagesFolderPath(testSolutionManager, testSettings);

                var msBuildNuGetProject = testSolutionManager.AddNewMSBuildProject();
                var msBuildNuGetProjectSystem = msBuildNuGetProject.ProjectSystem as TestMSBuildNuGetProjectSystem;
                var packagesConfigPath = msBuildNuGetProject.PackagesConfigNuGetProject.FullPath;
                var target = "a";

                // Act
                var nugetProjectActions = await nuGetPackageManager.PreviewInstallPackageAsync(msBuildNuGetProject, target,
                    new ResolutionContext(), new TestNuGetProjectContext(), sourceRepositoryProvider.GetRepositories().First(), null, token);

                var result = nugetProjectActions.ToList();

                var resultIdentities = result.Select(p => p.PackageIdentity);

                Assert.Contains(new PackageIdentity("a", new NuGetVersion(2, 0, 0)), resultIdentities);
                Assert.Contains(new PackageIdentity("b", new NuGetVersion(1, 0, 0)), resultIdentities);

                //  and all the actions are Install
                foreach (var nugetProjectAction in result)
                {
                    Assert.Equal(nugetProjectAction.NuGetProjectActionType, NuGetProjectActionType.Install);
                }
            }
        }

        [Fact]
        public async Task InstallPackage571FromV3()
        {
            // Arrange
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV3OnlySourceRepositoryProvider();

            using (var testSolutionManager = new TestSolutionManager())
            {
                var testSettings = NullSettings.Instance;
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var token = CancellationToken.None;
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    testSettings,
                    testSolutionManager,
                    deleteOnRestartManager);
                var packagesFolderPath = PackagesFolderPathUtility.GetPackagesFolderPath(testSolutionManager, testSettings);

                var msBuildNuGetProject = testSolutionManager.AddNewMSBuildProject();
                var msBuildNuGetProjectSystem = msBuildNuGetProject.ProjectSystem as TestMSBuildNuGetProjectSystem;
                var packagesConfigPath = msBuildNuGetProject.PackagesConfigNuGetProject.FullPath;
                var target = new PackageIdentity("Umbraco", NuGetVersion.Parse("5.1.0.175"));

                // Act
                var nugetProjectActions = await nuGetPackageManager.PreviewInstallPackageAsync(msBuildNuGetProject, target,
                    new ResolutionContext(), new TestNuGetProjectContext(), sourceRepositoryProvider.GetRepositories().First(), null, token);

                var result = nugetProjectActions.ToList();

                var resultIdentities = result.Select(p => p.PackageIdentity);

                Assert.Contains(new PackageIdentity("Umbraco", new NuGetVersion("5.1.0.175")), resultIdentities);

                //  and all the actions are Install
                foreach (var nugetProjectAction in result)
                {
                    Assert.Equal(nugetProjectAction.NuGetProjectActionType, NuGetProjectActionType.Install);
                }
            }
        }

        [Fact]
        public async Task InstallPackageEFFromV3()
        {
            // Arrange
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(new[]
            {
                TestSourceRepositoryUtility.V3PackageSource,
                new PackageSource("https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet6/nuget/v3/index.json"),
            });

            using (var testSolutionManager = new TestSolutionManager())
            {
                var testSettings = NullSettings.Instance;
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var token = CancellationToken.None;
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    testSettings,
                    testSolutionManager,
                    deleteOnRestartManager);
                var packagesFolderPath = PackagesFolderPathUtility.GetPackagesFolderPath(testSolutionManager, testSettings);

                var msBuildNuGetProject = testSolutionManager.AddNewMSBuildProject("TestProject", NuGetFramework.Parse("net452"));
                var target = new PackageIdentity("EntityFramework", NuGetVersion.Parse("7.0.0-beta4"));

                // Act
                var nugetProjectActions = await nuGetPackageManager.PreviewInstallPackageAsync(
                    msBuildNuGetProject,
                    target,
                    new ResolutionContext(DependencyBehavior.Lowest, true, false, VersionConstraints.None),
                    new TestNuGetProjectContext(),
                    sourceRepositoryProvider.GetRepositories().First(),
                    sourceRepositoryProvider.GetRepositories(),
                    token);

                var result = nugetProjectActions.ToList();

                var resultIdentities = result.Select(p => p.PackageIdentity);

                Assert.Contains(target, resultIdentities);

                //  and all the actions are Install
                foreach (var nugetProjectAction in result)
                {
                    Assert.Equal(nugetProjectAction.NuGetProjectActionType, NuGetProjectActionType.Install);
                }
            }
        }

        [Fact]
        public async Task InstallPackagePrereleaseDependenciesFromV2()
        {
            // Arrange
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV2OnlySourceRepositoryProvider();

            using (var testSolutionManager = new TestSolutionManager())
            {
                var testSettings = NullSettings.Instance;
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var token = CancellationToken.None;
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    testSettings,
                    testSolutionManager,
                    deleteOnRestartManager);
                var packagesFolderPath = PackagesFolderPathUtility.GetPackagesFolderPath(testSolutionManager, testSettings);

                var msBuildNuGetProject = testSolutionManager.AddNewMSBuildProject();
                var msBuildNuGetProjectSystem = msBuildNuGetProject.ProjectSystem as TestMSBuildNuGetProjectSystem;
                var packagesConfigPath = msBuildNuGetProject.PackagesConfigNuGetProject.FullPath;
                var target = new PackageIdentity("DependencyTestA", NuGetVersion.Parse("1.0.0"));

                // Act
                var nugetProjectActions = await nuGetPackageManager.PreviewInstallPackageAsync(
                    msBuildNuGetProject,
                    target,
                    new ResolutionContext(DependencyBehavior.Lowest, false, false, VersionConstraints.None),
                    new TestNuGetProjectContext(),
                    sourceRepositoryProvider.GetRepositories().First(),
                    null,
                    token);

                var result = nugetProjectActions.ToList();

                var resultIdentities = result.Select(p => p.PackageIdentity);

                Assert.Contains(target, resultIdentities);
                Assert.Contains(new PackageIdentity("DependencyTestB", NuGetVersion.Parse("1.0.0")), resultIdentities);

                //  and all the actions are Install
                foreach (var nugetProjectAction in result)
                {
                    Assert.Equal(nugetProjectAction.NuGetProjectActionType, NuGetProjectActionType.Install);
                }
            }
        }

        [Fact]
        public async Task InstallPackagePrereleaseDependenciesFromV2IncludePrerelease()
        {
            // Arrange
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV2OnlySourceRepositoryProvider();

            using (var testSolutionManager = new TestSolutionManager())
            {
                var testSettings = NullSettings.Instance;
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var token = CancellationToken.None;
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    testSettings,
                    testSolutionManager,
                    deleteOnRestartManager);
                var packagesFolderPath = PackagesFolderPathUtility.GetPackagesFolderPath(testSolutionManager, testSettings);

                var msBuildNuGetProject = testSolutionManager.AddNewMSBuildProject();
                var msBuildNuGetProjectSystem = msBuildNuGetProject.ProjectSystem as TestMSBuildNuGetProjectSystem;
                var packagesConfigPath = msBuildNuGetProject.PackagesConfigNuGetProject.FullPath;
                var target = new PackageIdentity("DependencyTestA", NuGetVersion.Parse("1.0.0"));

                // Act
                var nugetProjectActions = await nuGetPackageManager.PreviewInstallPackageAsync(
                    msBuildNuGetProject,
                    target,
                    new ResolutionContext(DependencyBehavior.Lowest, true, false, VersionConstraints.None),
                    new TestNuGetProjectContext(),
                    sourceRepositoryProvider.GetRepositories().First(),
                    null,
                    token);

                var result = nugetProjectActions.ToList();

                var resultIdentities = result.Select(p => p.PackageIdentity);

                Assert.Contains(target, resultIdentities);
                Assert.Contains(new PackageIdentity("DependencyTestB", NuGetVersion.Parse("1.0.0-a")), resultIdentities);

                //  and all the actions are Install
                foreach (var nugetProjectAction in result)
                {
                    Assert.Equal(nugetProjectAction.NuGetProjectActionType, NuGetProjectActionType.Install);
                }
            }
        }

        [Fact]
        public async Task InstallPackagePrerelease()
        {
            // Arrange
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV2OnlySourceRepositoryProvider();

            using (var testSolutionManager = new TestSolutionManager())
            {
                var testSettings = NullSettings.Instance;
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var token = CancellationToken.None;
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    testSettings,
                    testSolutionManager,
                    deleteOnRestartManager); new NuGetPackageManager(
                     sourceRepositoryProvider,
                     testSettings,
                     testSolutionManager,
                     deleteOnRestartManager);
                var packagesFolderPath = PackagesFolderPathUtility.GetPackagesFolderPath(testSolutionManager, testSettings);

                var msBuildNuGetProject = testSolutionManager.AddNewMSBuildProject();
                var msBuildNuGetProjectSystem = msBuildNuGetProject.ProjectSystem as TestMSBuildNuGetProjectSystem;
                var packagesConfigPath = msBuildNuGetProject.PackagesConfigNuGetProject.FullPath;
                var target = new PackageIdentity("Microsoft.ApplicationInsights.Web", NuGetVersion.Parse("0.16.1-build00418"));

                // Act
                var nugetProjectActions = await nuGetPackageManager.PreviewInstallPackageAsync(
                    msBuildNuGetProject,
                    target,
                    new ResolutionContext(DependencyBehavior.Lowest, false, false, VersionConstraints.None),
                    new TestNuGetProjectContext(),
                    sourceRepositoryProvider.GetRepositories().First(),
                    null,
                    token);

                var result = nugetProjectActions.ToList();

                var resultIdentities = result.Select(p => p.PackageIdentity);

                Assert.Contains(target, resultIdentities);

                //  and all the actions are Install
                foreach (var nugetProjectAction in result)
                {
                    Assert.Equal(nugetProjectAction.NuGetProjectActionType, NuGetProjectActionType.Install);
                }
            }
        }

        [Fact]
        public async Task InstallPackageOverExisting()
        {
            // Arrange
            var fwk46 = NuGetFramework.Parse("net46");
            var fwk45 = NuGetFramework.Parse("net45");
            var fwk4 = NuGetFramework.Parse("net4");

            var installedPackages = new List<PackageReference>
            {
                new PackageReference(new PackageIdentity("51Degrees.mobi", NuGetVersion.Parse("2.1.15.1")), fwk4, true),
                new PackageReference(new PackageIdentity("AspNetMvc", NuGetVersion.Parse("4.0.20710.0")), fwk45, true),
                new PackageReference(new PackageIdentity("AttributeRouting", NuGetVersion.Parse("3.5.6")), fwk4, true),
                new PackageReference(new PackageIdentity("AttributeRouting.Core", NuGetVersion.Parse("3.5.6")), fwk4, true),
                new PackageReference(new PackageIdentity("AttributeRouting.Core.Web", NuGetVersion.Parse("3.5.6")), fwk4, true),
                new PackageReference(new PackageIdentity("AutoMapper", NuGetVersion.Parse("3.3.1")), fwk45, true),
                new PackageReference(new PackageIdentity("Castle.Core", NuGetVersion.Parse("1.1.0")), fwk4, true),
                new PackageReference(new PackageIdentity("Castle.DynamicProxy", NuGetVersion.Parse("2.1.0")), fwk4, true),
                new PackageReference(new PackageIdentity("Clay", NuGetVersion.Parse("1.0")), fwk4, true),
                new PackageReference(new PackageIdentity("colorbox", NuGetVersion.Parse("1.4.29")), fwk45, true),
                new PackageReference(new PackageIdentity("elmah", NuGetVersion.Parse("1.2.0.1")), fwk4, true),
                new PackageReference(new PackageIdentity("elmah.corelibrary", NuGetVersion.Parse("1.2")), fwk4, true),
                new PackageReference(new PackageIdentity("EntityFramework", NuGetVersion.Parse("6.1.3")), fwk45, true),
                new PackageReference(new PackageIdentity("fasterflect", NuGetVersion.Parse("2.1.0")), fwk4, true),
                new PackageReference(new PackageIdentity("foolproof", NuGetVersion.Parse("0.9.4517")), fwk45, true),
                new PackageReference(new PackageIdentity("Glimpse", NuGetVersion.Parse("0.87")), fwk4, true),
                new PackageReference(new PackageIdentity("Glimpse.Elmah", NuGetVersion.Parse("0.9.3")), fwk4, true),
                new PackageReference(new PackageIdentity("Glimpse.Mvc3", NuGetVersion.Parse("0.87")), fwk4, true),
                new PackageReference(new PackageIdentity("jQuery", NuGetVersion.Parse("1.4.1")), fwk45, true),
                new PackageReference(new PackageIdentity("knockout.mapper.TypeScript.DefinitelyTyped", NuGetVersion.Parse("0.0.4")), fwk45, true),
                new PackageReference(new PackageIdentity("Knockout.Mapping", NuGetVersion.Parse("2.4.0")), fwk45, true),
                new PackageReference(new PackageIdentity("knockout.mapping.TypeScript.DefinitelyTyped", NuGetVersion.Parse("0.0.9")), fwk45, true),
                new PackageReference(new PackageIdentity("knockout.TypeScript.DefinitelyTyped", NuGetVersion.Parse("0.5.1")), fwk45, true),
                new PackageReference(new PackageIdentity("Knockout.Validation", NuGetVersion.Parse("1.0.1")), fwk45, true),
                new PackageReference(new PackageIdentity("knockoutjs", NuGetVersion.Parse("2.0.0")), fwk45, true),
                new PackageReference(new PackageIdentity("LINQtoCSV", NuGetVersion.Parse("1.2.0.0")), fwk4, true),
                new PackageReference(new PackageIdentity("log4net", NuGetVersion.Parse("2.0.3")), fwk45, true),
                new PackageReference(new PackageIdentity("Microsoft.AspNet.Mvc", NuGetVersion.Parse("4.0.40804.0")), fwk45, true),
                new PackageReference(new PackageIdentity("Microsoft.AspNet.Razor", NuGetVersion.Parse("2.0.30506.0")), fwk45, true),
                new PackageReference(new PackageIdentity("Microsoft.AspNet.WebPages", NuGetVersion.Parse("2.0.30506.0")), fwk45, true),
                new PackageReference(new PackageIdentity("Microsoft.Web.Infrastructure", NuGetVersion.Parse("1.0.0.0")), fwk4, true),
                new PackageReference(new PackageIdentity("MiniProfiler", NuGetVersion.Parse("3.1.1.140")), fwk45, true),
                new PackageReference(new PackageIdentity("MiniProfiler.EF6", NuGetVersion.Parse("3.0.11")), fwk45, true),
                new PackageReference(new PackageIdentity("MiniProfiler.MVC4", NuGetVersion.Parse("3.0.11")), fwk45, true),
                new PackageReference(new PackageIdentity("Mvc3CodeTemplatesCSharp", NuGetVersion.Parse("3.0.11214.0")), fwk4, true),
                new PackageReference(new PackageIdentity("MvcDiagnostics", NuGetVersion.Parse("3.0.10714.0")), fwk4, true),
                new PackageReference(new PackageIdentity("Newtonsoft.Json", NuGetVersion.Parse("6.0.8")), fwk45, true),
                new PackageReference(new PackageIdentity("Ninject", NuGetVersion.Parse("3.2.2.0")), fwk45, true),
                new PackageReference(new PackageIdentity("Ninject.Web.Common", NuGetVersion.Parse("3.2.3.0")), fwk45, true),
                new PackageReference(new PackageIdentity("OpenPop.NET", NuGetVersion.Parse("2.0.5.1063")), fwk45, true),
                new PackageReference(new PackageIdentity("PreMailer.Net", NuGetVersion.Parse("1.1.2")), fwk4, true),
                new PackageReference(new PackageIdentity("Rejuicer", NuGetVersion.Parse("1.3.0")), fwk45, true),
                new PackageReference(new PackageIdentity("T4MVCExtensions", NuGetVersion.Parse("3.15.2")), fwk46, true),
                new PackageReference(new PackageIdentity("T4MvcJs", NuGetVersion.Parse("1.0.13")), fwk45, true),
                new PackageReference(new PackageIdentity("Twia.ReSharper", NuGetVersion.Parse("9.0.0")), fwk45, true),
                new PackageReference(new PackageIdentity("valueinjecter", NuGetVersion.Parse("2.3.3")), fwk45, true),
                new PackageReference(new PackageIdentity("WebActivator", NuGetVersion.Parse("1.5")), fwk4, true),
                new PackageReference(new PackageIdentity("YUICompressor.NET", NuGetVersion.Parse("1.6.0.2")), fwk45, true),
            };

            var nuGetProject = new TestNuGetProject(installedPackages);

            var target = "t4mvc";

            // Act
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV2OnlySourceRepositoryProvider();
            var testSettings = NullSettings.Instance;
            using (var testSolutionManager = new TestSolutionManager())
            {
                var deleteOnRestartManager = new TestDeleteOnRestartManager();

                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    testSettings,
                    testSolutionManager,
                    deleteOnRestartManager);

                var nugetProjectActions = await nuGetPackageManager.PreviewInstallPackageAsync(
                    nuGetProject,
                    new PackageIdentity(target, new NuGetVersion(3, 17, 5)),
                    new ResolutionContext(DependencyBehavior.Lowest, false, false, VersionConstraints.None),
                    new TestNuGetProjectContext(),
                    sourceRepositoryProvider.GetRepositories().First(),
                    null,
                    CancellationToken.None);

                Assert.Contains(target, nugetProjectActions.Select(pa => pa.PackageIdentity.Id), StringComparer.OrdinalIgnoreCase);
            }
        }

        [Fact(Skip = "Test was skipped as part of 475ad399 and is currently broken.")]
        public async Task InstallPackageDowngrade()
        {
            // Arrange
            var fwk46 = NuGetFramework.Parse("net46");
            var fwk45 = NuGetFramework.Parse("net45");
            var fwk4 = NuGetFramework.Parse("net4");

            var installedPackages = new List<PackageReference>
            {
                new PackageReference(new PackageIdentity("ResolverTestA", NuGetVersion.Parse("3.0.0")), fwk45, true),
                new PackageReference(new PackageIdentity("ResolverTestB", NuGetVersion.Parse("3.0.0")), fwk45, true),
            };

            var nuGetProject = new TestNuGetProject(installedPackages);

            var target = "FixedTestA";

            // Act
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV2OnlySourceRepositoryProvider();
            var testSettings = NullSettings.Instance;
            using (var testSolutionManager = new TestSolutionManager())
            {
                var deleteOnRestartManager = new TestDeleteOnRestartManager();

                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    testSettings,
                    testSolutionManager,
                    deleteOnRestartManager);

                var nugetProjectActions = await nuGetPackageManager.PreviewInstallPackageAsync(
                    nuGetProject,
                    target,
                    new ResolutionContext(DependencyBehavior.Lowest, false, false, VersionConstraints.None),
                    new TestNuGetProjectContext(),
                    sourceRepositoryProvider.GetRepositories().First(),
                    null,
                    CancellationToken.None);

                Assert.Contains(target, nugetProjectActions.Select(pa => pa.PackageIdentity.Id), StringComparer.OrdinalIgnoreCase);
            }
        }

        [Fact]
        public async Task UpdatePackagePreservePackagesConfigAttributes()
        {
            // Arrange
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV3OnlySourceRepositoryProvider();
            using (var testSolutionManager = new TestSolutionManager())
            {
                var testSettings = NullSettings.Instance;
                var token = CancellationToken.None;
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    testSettings,
                    testSolutionManager,
                    deleteOnRestartManager);
                var packagesFolderPath =
                    PackagesFolderPathUtility.GetPackagesFolderPath(testSolutionManager, testSettings);

                var msBuildNuGetProject = testSolutionManager.AddNewMSBuildProject();
                var packagesConfigPath = msBuildNuGetProject.PackagesConfigNuGetProject.FullPath;

                using (var writer = new StreamWriter(packagesConfigPath))
                {
                    writer.WriteLine(@"<packages>
                <package id=""NuGet.Versioning"" version=""1.0.1"" targetFramework=""net45""
                    allowedVersions=""[1.0.0, 2.0.0]"" developmentDependency=""true"" future=""abc"" />
                </packages>");
                }

                var packageIdentity = new PackageIdentity("nuget.versioning", NuGetVersion.Parse("1.0.5"));
                var packageOld = new PackageIdentity("nuget.versioning", NuGetVersion.Parse("1.0.1"));

                // Act
                using (var cacheContext = new SourceCacheContext())
                {
                    await nuGetPackageManager.RestorePackageAsync(
                        packageOld,
                        new TestNuGetProjectContext(),
                        new PackageDownloadContext(cacheContext),
                        sourceRepositoryProvider.GetRepositories(),
                        token);

                    var actions = await nuGetPackageManager.PreviewInstallPackageAsync(
                        msBuildNuGetProject,
                        packageIdentity,
                        new ResolutionContext(),
                        new TestNuGetProjectContext(),
                        sourceRepositoryProvider.GetRepositories().First(),
                        null,
                        token);

                    await nuGetPackageManager.InstallPackageAsync(
                        msBuildNuGetProject,
                        packageIdentity,
                        new ResolutionContext(),
                        new TestNuGetProjectContext(),
                        sourceRepositoryProvider.GetRepositories(),
                        sourceRepositoryProvider.GetRepositories(),
                        token);

                    var packagesInPackagesConfig = (await msBuildNuGetProject.PackagesConfigNuGetProject
                        .GetInstalledPackagesAsync(token))
                        .ToList();

                    var packagesConfigXML = XDocument.Load(packagesConfigPath);
                    var entry = packagesConfigXML.Element(XName.Get("packages")).Elements(XName.Get("package")).Single();

                    // Assert
                    Assert.Equal(2, actions.Count());
                    Assert.Equal(1, packagesInPackagesConfig.Count);
                    Assert.Equal(packageIdentity, packagesInPackagesConfig[0].PackageIdentity);
                    Assert.Equal(msBuildNuGetProject.ProjectSystem.TargetFramework, packagesInPackagesConfig[0].TargetFramework);

                    Assert.Equal("[1.0.0, 2.0.0]", entry.Attribute(XName.Get("allowedVersions")).Value);
                    Assert.Equal("true", entry.Attribute(XName.Get("developmentDependency")).Value);
                    Assert.Equal("abc", entry.Attribute(XName.Get("future")).Value);
                }
            }
        }

        [Fact]
        public async Task UpdatePackagePreservePackagesConfigAttributesMultiplePackages()
        {
            // Arrange
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV3OnlySourceRepositoryProvider();
            using (var testSolutionManager = new TestSolutionManager())
            {
                var testSettings = NullSettings.Instance;
                var token = CancellationToken.None;
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    testSettings,
                    testSolutionManager,
                    deleteOnRestartManager);
                var packagesFolderPath =
                    PackagesFolderPathUtility.GetPackagesFolderPath(testSolutionManager, testSettings);

                var msBuildNuGetProject = testSolutionManager.AddNewMSBuildProject();
                var packagesConfigPath = msBuildNuGetProject.PackagesConfigNuGetProject.FullPath;

                using (var writer = new StreamWriter(packagesConfigPath))
                {
                    writer.WriteLine(@"<packages>
                <package id=""NuGet.Versioning"" version=""1.0.1"" targetFramework=""net45""
                    allowedVersions=""[1.0.0, 2.0.0]"" developmentDependency=""true"" future=""abc"" />
                <package id=""newtonsoft.json"" version=""6.0.8"" targetFramework=""net45"" />
                </packages>");
                }

                var packageIdentity = new PackageIdentity("nuget.versioning", NuGetVersion.Parse("1.0.5"));
                var packageOld = new PackageIdentity("nuget.versioning", NuGetVersion.Parse("1.0.1"));

                // Act
                using (var cacheContext = new SourceCacheContext())
                {
                    var packageDownloadContext = new PackageDownloadContext(cacheContext);

                    await nuGetPackageManager.RestorePackageAsync(
                        packageOld,
                        new TestNuGetProjectContext(),
                        packageDownloadContext,
                        sourceRepositoryProvider.GetRepositories(),
                        token);

                    await nuGetPackageManager.RestorePackageAsync(
                        new PackageIdentity("newtonsoft.json", NuGetVersion.Parse("6.0.8")),
                        new TestNuGetProjectContext(),
                        packageDownloadContext,
                        sourceRepositoryProvider.GetRepositories(),
                        token);

                    var actions = await nuGetPackageManager.PreviewInstallPackageAsync(
                        msBuildNuGetProject,
                        packageIdentity,
                        new ResolutionContext(),
                        new TestNuGetProjectContext(),
                        sourceRepositoryProvider.GetRepositories().First(),
                        null,
                        token);

                    await nuGetPackageManager.InstallPackageAsync(
                        msBuildNuGetProject,
                        packageIdentity,
                        new ResolutionContext(),
                        new TestNuGetProjectContext(),
                        sourceRepositoryProvider.GetRepositories(),
                        sourceRepositoryProvider.GetRepositories(),
                        token);

                    var packagesInPackagesConfig = (await msBuildNuGetProject.PackagesConfigNuGetProject
                        .GetInstalledPackagesAsync(token))
                        .OrderBy(package => package.PackageIdentity.Id)
                        .ToList();

                    var packagesConfigXML = XDocument.Load(packagesConfigPath);
                    var entry = packagesConfigXML.Element(XName.Get("packages"))
                        .Elements(XName.Get("package"))
                        .Single(package => package.Attribute(XName.Get("id")).Value
                            .Equals("nuget.versioning", StringComparison.OrdinalIgnoreCase));

                    // Assert
                    Assert.Equal(2, actions.Count());
                    Assert.Equal(2, packagesInPackagesConfig.Count);
                    Assert.Equal(packageIdentity, packagesInPackagesConfig[1].PackageIdentity);
                    Assert.Equal(msBuildNuGetProject.ProjectSystem.TargetFramework, packagesInPackagesConfig[1].TargetFramework);

                    Assert.Equal("[1.0.0, 2.0.0]", entry.Attribute(XName.Get("allowedVersions")).Value);
                    Assert.Equal("true", entry.Attribute(XName.Get("developmentDependency")).Value);
                    Assert.Equal("abc", entry.Attribute(XName.Get("future")).Value);
                }
            }
        }

        private SourceRepositoryProvider CreateSource(List<SourcePackageDependencyInfo> packages)
        {
            var resourceProviders = new List<Lazy<INuGetResourceProvider>>();
            resourceProviders.Add(new Lazy<INuGetResourceProvider>(() => new TestDependencyInfoProvider(packages)));
            resourceProviders.Add(new Lazy<INuGetResourceProvider>(() => new TestMetadataProvider(packages)));

            var packageSource = new PackageSource("http://temp");
            var packageSourceProvider = new TestPackageSourceProvider(new[] { packageSource });

            return new SourceRepositoryProvider(packageSourceProvider, resourceProviders);
        }

        private static void Expected(List<Tuple<PackageIdentity, NuGetProjectActionType>> expected, string id, NuGetVersion oldVersion, NuGetVersion newVersion)
        {
            expected.Add(Tuple.Create(new PackageIdentity(id, oldVersion), NuGetProjectActionType.Uninstall));
            expected.Add(Tuple.Create(new PackageIdentity(id, newVersion), NuGetProjectActionType.Install));
        }

        private static bool Compare(
            IEnumerable<Tuple<PackageIdentity, NuGetProjectActionType>> lhs,
            IEnumerable<Tuple<PackageIdentity, NuGetProjectActionType>> rhs)
        {
            var ok = true;
            ok &= RhsContainsAllLhs(lhs, rhs);
            ok &= RhsContainsAllLhs(rhs, lhs);
            return ok;
        }

        private static bool RhsContainsAllLhs(
            IEnumerable<Tuple<PackageIdentity, NuGetProjectActionType>> lhs,
            IEnumerable<Tuple<PackageIdentity, NuGetProjectActionType>> rhs)
        {
            foreach (var item in lhs)
            {
                if (!rhs.Contains(item, new ActionComparer()))
                {
                    return false;
                }
            }
            return true;
        }

        private class ActionComparer : IEqualityComparer<Tuple<PackageIdentity, NuGetProjectActionType>>
        {
            public bool Equals(Tuple<PackageIdentity, NuGetProjectActionType> x, Tuple<PackageIdentity, NuGetProjectActionType> y)
            {
                var f1 = x.Item1.Equals(y.Item1);
                var f2 = x.Item2 == y.Item2;
                return f1 && f2;
            }

            public int GetHashCode(Tuple<PackageIdentity, NuGetProjectActionType> obj)
            {
                return obj.GetHashCode();
            }
        }
    }
}
