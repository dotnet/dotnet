// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using FluentAssertions;
using Moq;
using Newtonsoft.Json.Linq;
using NuGet.Commands;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Configuration.Test;
using NuGet.Frameworks;
using NuGet.PackageManagement;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.ProjectManagement;
using NuGet.ProjectManagement.Projects;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Resolver;
using NuGet.Test.Utility;
using NuGet.Versioning;
#if IS_DESKTOP
using NuGet.VisualStudio;
#endif
using Test.Utility;
using Xunit;
using Xunit.Abstractions;
using ExceptionUtility = NuGet.PackageManagement.ExceptionUtility;

namespace NuGet.Test
{
    public class NuGetPackageManagerTests
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

        public NuGetPackageManagerTests(ITestOutputHelper output)
        {
            _logger = new XunitLogger(output);
        }

        [Fact]
        public void Constructor_WithNullISourceRepositoryProvider_Throws()
        {
            var solutionManager = new Mock<ISolutionManager>();
            solutionManager.Setup(e => e.SolutionDirectory).Returns(@"C:\");
            Assert.Throws<ArgumentNullException>(() => new NuGetPackageManager(
                null,
                new Mock<ISettings>().Object,
                solutionManager.Object,
                null,
                new Mock<IRestoreProgressReporter>().Object
                ));
        }

        [Fact]
        public void Constructor_WithNullISettings_Throws()
        {
            var solutionManager = new Mock<ISolutionManager>();
            solutionManager.Setup(e => e.SolutionDirectory).Returns(@"C:\");
            Assert.Throws<ArgumentNullException>(() => new NuGetPackageManager(
                new Mock<ISourceRepositoryProvider>().Object,
                null,
                solutionManager.Object,
                new Mock<IDeleteOnRestartManager>().Object,
                new Mock<IRestoreProgressReporter>().Object
                ));
        }

        [Fact]
        public void Constructor_WithNullISolutionManager_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new NuGetPackageManager(
                new Mock<ISourceRepositoryProvider>().Object,
                new Mock<ISettings>().Object,
                null,
                new Mock<IDeleteOnRestartManager>().Object,
                new Mock<IRestoreProgressReporter>().Object
                ));
        }

        [Fact]
        public void Constructor_WithNullIDeleteOnRestartManager_Throws()
        {
            var solutionManager = new Mock<ISolutionManager>();
            solutionManager.Setup(e => e.SolutionDirectory).Returns(@"C:\");
            Assert.Throws<ArgumentNullException>(() => new NuGetPackageManager(
                new Mock<ISourceRepositoryProvider>().Object,
                new Mock<ISettings>().Object,
                solutionManager.Object,
                null,
                new Mock<IRestoreProgressReporter>().Object
                ));
        }

        [Fact]
        public void Constructor_WithNullProgressReporter_Throws()
        {
            var solutionManager = new Mock<ISolutionManager>();
            solutionManager.Setup(e => e.SolutionDirectory).Returns(@"C:\");
            Assert.Throws<ArgumentNullException>(() => new NuGetPackageManager(
                new Mock<ISourceRepositoryProvider>().Object,
                new Mock<ISettings>().Object,
                solutionManager.Object,
                new Mock<IDeleteOnRestartManager>().Object,
                reporter: null
                ));
        }

        [Fact]
        public void Constructor_With6Arguments_WithNullProgressReporter_DoesNotThrows()
        {
            var solutionManager = new Mock<ISolutionManager>();
            solutionManager.Setup(e => e.SolutionDirectory).Returns(@"C:\");
            _ = new NuGetPackageManager(
                new Mock<ISourceRepositoryProvider>().Object,
                new Mock<ISettings>().Object,
                solutionManager.Object,
                new Mock<IDeleteOnRestartManager>().Object,
                reporter: null,
                excludeVersion: true
                );
        }

        [Fact]
        public void Constructor_With6Arguments_WithNullISourceRepositoryProvider_Throws()
        {
            var solutionManager = new Mock<ISolutionManager>();
            solutionManager.Setup(e => e.SolutionDirectory).Returns(@"C:\");
            Assert.Throws<ArgumentNullException>(() => new NuGetPackageManager(
                null,
                new Mock<ISettings>().Object,
                solutionManager.Object,
                null,
                new Mock<IRestoreProgressReporter>().Object,
                excludeVersion: true
                ));
        }

        [Fact]
        public void Constructor_With6Arguments_WithNullISettings_Throws()
        {
            var solutionManager = new Mock<ISolutionManager>();
            solutionManager.Setup(e => e.SolutionDirectory).Returns(@"C:\");
            Assert.Throws<ArgumentNullException>(() => new NuGetPackageManager(
                new Mock<ISourceRepositoryProvider>().Object,
                null,
                solutionManager.Object,
                new Mock<IDeleteOnRestartManager>().Object,
                new Mock<IRestoreProgressReporter>().Object,
                excludeVersion: true
                ));
        }

        [Fact]
        public void Constructor_With6Arguments_WithNullISolutionManager_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new NuGetPackageManager(
                new Mock<ISourceRepositoryProvider>().Object,
                new Mock<ISettings>().Object,
                null,
                new Mock<IDeleteOnRestartManager>().Object,
                new Mock<IRestoreProgressReporter>().Object,
                excludeVersion: true
                ));
        }

        [Fact]
        public void Constructor_With6Arguments_WithNullIDeleteOnRestartManager_Throws()
        {
            var solutionManager = new Mock<ISolutionManager>();
            solutionManager.Setup(e => e.SolutionDirectory).Returns(@"C:\");
            Assert.Throws<ArgumentNullException>(() => new NuGetPackageManager(
                new Mock<ISourceRepositoryProvider>().Object,
                new Mock<ISettings>().Object,
                solutionManager.Object,
                null,
                new Mock<IRestoreProgressReporter>().Object,
                excludeVersion: true
                ));
        }


        // Install and uninstall a package while calling get installed on another thread
        [Fact]
        public async Task TestPacManInstallAndRequestInstalledPackages()
        {
            using (var packageSource = TestDirectory.Create())
            {
                // Arrange
                var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(
                    new List<Configuration.PackageSource>()
                    {
                        new Configuration.PackageSource(packageSource.Path)
                    });

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
            }
        }

        [Fact]
        public async Task TestPacManInstallPackage()
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
        public async Task PackagesConfigNuGetProjectGetInstalledPackagesListInvalidXml()
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

                // Create packages.config that is an invalid xml
                using (var w = new StreamWriter(File.Create(packagesConfigPath)))
                {
                    w.Write("abc");
                }

                // Act and Assert
                var ex = await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                {
                    await msBuildNuGetProject.PackagesConfigNuGetProject.GetInstalledPackagesAsync(token);
                });

                Assert.True(ex.Message.StartsWith("An error occurred while reading file"));
            }
        }

        [Fact]
        public async Task TestPacManInstallPackageAlreadyInstalledException()
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
        public async Task TestPacManInstallDifferentPackageAfterInstall()
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
        public async Task TestPacManInstallSamePackageAfterInstall()
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
        public async Task TestPacManInstallPackageWithDependents()
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
        public async Task TestPacManPreviewInstallOrderOfDependencies()
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

                var msBuildNuGetProject = testSolutionManager.AddNewMSBuildProject();
                var msBuildNuGetProjectSystem = msBuildNuGetProject.ProjectSystem as TestMSBuildNuGetProjectSystem;
                var packagesConfigPath = msBuildNuGetProject.PackagesConfigNuGetProject.FullPath;
                var packageIdentity = _morePackageWithDependents[3];

                // Pre-Assert
                // Check that the packages.config file does not exist
                Assert.False(File.Exists(packagesConfigPath));
                // Check that there are no packages returned by PackagesConfigProject
                var packagesInPackagesConfig = (await msBuildNuGetProject.PackagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(0, packagesInPackagesConfig.Count);
                Assert.Equal(0, msBuildNuGetProjectSystem.References.Count);

                // Act
                var packageActions = (await nuGetPackageManager.PreviewInstallPackageAsync(msBuildNuGetProject, packageIdentity,
                    new ResolutionContext(), new TestNuGetProjectContext(), sourceRepositoryProvider.GetRepositories().First(), null, token)).ToList();

                // Assert
                Assert.Equal(3, packageActions.Count);
                Assert.True(_morePackageWithDependents[0].Equals(packageActions[0].PackageIdentity));
                Assert.Equal(NuGetProjectActionType.Install, packageActions[0].NuGetProjectActionType);
                Assert.Equal(sourceRepositoryProvider.GetRepositories().Single().PackageSource.Source,
                    packageActions[0].SourceRepository.PackageSource.Source);
                Assert.True(_morePackageWithDependents[2].Equals(packageActions[1].PackageIdentity));
                Assert.Equal(NuGetProjectActionType.Install, packageActions[1].NuGetProjectActionType);
                Assert.Equal(sourceRepositoryProvider.GetRepositories().Single().PackageSource.Source,
                    packageActions[0].SourceRepository.PackageSource.Source);
                Assert.True(_morePackageWithDependents[3].Equals(packageActions[2].PackageIdentity));
                Assert.Equal(NuGetProjectActionType.Install, packageActions[2].NuGetProjectActionType);
                Assert.Equal(sourceRepositoryProvider.GetRepositories().Single().PackageSource.Source,
                    packageActions[0].SourceRepository.PackageSource.Source);
            }
        }

        [Fact]
        public async Task TestPacManPreviewInstallMvcPackageWithPrereleaseFlagFalse()
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

                var resolutionContext = new ResolutionContext(DependencyBehavior.Lowest, includePrelease: false, includeUnlisted: true, versionConstraints: VersionConstraints.None);

                // Act
                var packageActions = (await nuGetPackageManager.PreviewInstallPackageAsync(msBuildNuGetProject, packageIdentity,
                    resolutionContext, new TestNuGetProjectContext(), sourceRepositoryProvider.GetRepositories().First(), null, token)).ToList();

                // Assert
                Assert.Equal(1, packageActions.Count);
                Assert.True(_latestAspNetPackages[0].Equals(packageActions[0].PackageIdentity));
                Assert.Equal(NuGetProjectActionType.Install, packageActions[0].NuGetProjectActionType);
                Assert.Equal(sourceRepositoryProvider.GetRepositories().Single().PackageSource.Source,
                    packageActions[0].SourceRepository.PackageSource.Source);
            }
        }

        [Fact]
        public async Task TestPacManUninstallPackage()
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
                    resolutionContext, testNuGetProjectContext, sourceRepositoryProvider.GetRepositories().First(), null, token);

                // Assert
                // Check that the packages.config file exists after the installation
                Assert.True(File.Exists(packagesConfigPath));
                // Check the number of packages and packages returned by PackagesConfigProject after the installation
                packagesInPackagesConfig = (await msBuildNuGetProject.PackagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(1, packagesInPackagesConfig.Count);
                Assert.Equal(packageIdentity, packagesInPackagesConfig[0].PackageIdentity);
                Assert.Equal(msBuildNuGetProject.ProjectSystem.TargetFramework, packagesInPackagesConfig[0].TargetFramework);

                // Main Act
                var uninstallationContext = new UninstallationContext();
                await nuGetPackageManager.UninstallPackageAsync(msBuildNuGetProject, packageIdentity.Id,
                    uninstallationContext, testNuGetProjectContext, token);

                // Assert
                // Check that the packages.config file exists after the installation
                Assert.True(!File.Exists(packagesConfigPath));
                // Check the number of packages and packages returned by PackagesConfigProject after the installation
                packagesInPackagesConfig = (await msBuildNuGetProject.PackagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(0, packagesInPackagesConfig.Count);
                Assert.False(File.Exists(packagePathResolver.GetInstalledPackageFilePath(packageIdentity)));
            }
        }

        [Fact]
        public async Task TestPacManUninstallDependencyPackage()
        {
            // Arrange
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV3OnlySourceRepositoryProvider();
            using (var testSolutionManager = new TestSolutionManager())
            {
                var testSettings = NullSettings.Instance;
                var token = CancellationToken.None;
                var resolutionContext = new ResolutionContext();
                var testNuGetProjectContext = new TestNuGetProjectContext();
                var packagesFolderPath = PackagesFolderPathUtility.GetPackagesFolderPath(testSolutionManager, testSettings);
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    testSettings,
                    testSolutionManager,
                    deleteOnRestartManager);

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
                    resolutionContext, testNuGetProjectContext, sourceRepositoryProvider.GetRepositories().First(), null, token);

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

                // Main Act
                Exception exception = null;
                try
                {
                    var uninstallationContext = new UninstallationContext();
                    await nuGetPackageManager.UninstallPackageAsync(msBuildNuGetProject, "jQuery",
                        uninstallationContext, testNuGetProjectContext, token);
                }
                catch (InvalidOperationException ex)
                {
                    exception = ex;
                }
                catch (AggregateException ex)
                {
                    exception = ExceptionUtility.Unwrap(ex);
                }

                Assert.NotNull(exception);
                Assert.True(exception is InvalidOperationException);
                Assert.Equal("Unable to uninstall 'jQuery.1.4.4' because 'jQuery.Validation.1.13.1' depends on it.",
                    exception.Message);
            }
        }

        [Fact]
        public async Task TestPacManPreviewUninstallDependencyPackage()
        {
            // Arrange
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV3OnlySourceRepositoryProvider();
            using (var testSolutionManager = new TestSolutionManager())
            {
                var testSettings = NullSettings.Instance;
                var token = CancellationToken.None;
                var resolutionContext = new ResolutionContext();
                var testNuGetProjectContext = new TestNuGetProjectContext();
                var packagesFolderPath = PackagesFolderPathUtility.GetPackagesFolderPath(testSolutionManager, testSettings);
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    testSettings,
                    testSolutionManager,
                    deleteOnRestartManager);

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
                    resolutionContext, testNuGetProjectContext, sourceRepositoryProvider.GetRepositories().First(), null, token);

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

                // Main Act
                Exception exception = null;
                try
                {
                    var uninstallationContext = new UninstallationContext();
                    var packageActions = await nuGetPackageManager.PreviewUninstallPackageAsync(msBuildNuGetProject, "jQuery",
                        uninstallationContext, testNuGetProjectContext, token);
                }
                catch (InvalidOperationException ex)
                {
                    exception = ex;
                }
                catch (AggregateException ex)
                {
                    exception = ExceptionUtility.Unwrap(ex);
                }

                Assert.NotNull(exception);
                Assert.True(exception is InvalidOperationException);
                Assert.Equal("Unable to uninstall 'jQuery.1.4.4' because 'jQuery.Validation.1.13.1' depends on it.",
                    exception.Message);
            }
        }

        [Fact]
        public async Task TestPacManUninstallPackageOnMultipleProjects()
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
                var projectB = testSolutionManager.AddNewMSBuildProject();
                var packageIdentity = _noDependencyLibPackages[0];

                // Act
                await nuGetPackageManager.InstallPackageAsync(projectA, packageIdentity,
                    resolutionContext, testNuGetProjectContext, sourceRepositoryProvider.GetRepositories().First(), null, token);
                await nuGetPackageManager.InstallPackageAsync(projectB, packageIdentity,
                    resolutionContext, testNuGetProjectContext, sourceRepositoryProvider.GetRepositories().First(), null, token);

                // Assert
                var projectAInstalled = (await projectA.GetInstalledPackagesAsync(token)).ToList();
                var projectBInstalled = (await projectB.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(1, projectAInstalled.Count);
                Assert.Equal(1, projectBInstalled.Count);

                // Main Act
                var uninstallationContext = new UninstallationContext();
                await nuGetPackageManager.UninstallPackageAsync(projectA, packageIdentity.Id,
                    uninstallationContext, testNuGetProjectContext, token);

                // Assert
                projectAInstalled = (await projectA.GetInstalledPackagesAsync(token)).ToList();
                projectBInstalled = (await projectB.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(0, projectAInstalled.Count);
                Assert.Equal(1, projectBInstalled.Count);
                Assert.True(File.Exists(packagePathResolver.GetInstalledPackageFilePath(packageIdentity)));
            }
        }

        [Fact]
        public async Task TestPacManInstallHigherSpecificVersion()
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
        public async Task TestPacManInstallLowerSpecificVersion()
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
        public async Task TestPacManInstallLatestVersion()
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
                    Common.NullLogger.Instance,
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
        public async Task TestPacManInstallLatestVersionForPackageReference()
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
        public async Task TestPacManInstallLatestVersionOfDependencyPackage()
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
                    Common.NullLogger.Instance,
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
        public async Task TestPacManInstallHigherSpecificVersionOfDependencyPackage()
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
        public async Task TestPacManInstallLowerSpecificVersionOfDependencyPackage()
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
                    Common.NullLogger.Instance,
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
        public async Task TestPacManInstallPackageWhichUpdatesParent()
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
        public async Task TestPacManInstallPackageWhichUpdatesDependency()
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
        public async Task TestPacManPreviewUpdatePackageFollowingForceUninstall()
        {
            // Arrange

            // Set up Package Source
            var packages = new List<SourcePackageDependencyInfo>
            {
                new SourcePackageDependencyInfo("a", new NuGetVersion(1, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("a", new NuGetVersion(2, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("a", new NuGetVersion(3, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("b", new NuGetVersion(1, 0, 0), new[] { new Packaging.Core.PackageDependency("a", new VersionRange(new NuGetVersion(1, 0, 0))) }, true, null),
                new SourcePackageDependencyInfo("b", new NuGetVersion(2, 0, 0), new[] { new Packaging.Core.PackageDependency("a", new VersionRange(new NuGetVersion(2, 0, 0))) }, true, null),
                new SourcePackageDependencyInfo("b", new NuGetVersion(3, 0, 0), new[] { new Packaging.Core.PackageDependency("a", new VersionRange(new NuGetVersion(2, 0, 0))) }, true, null),
                new SourcePackageDependencyInfo("c", new NuGetVersion(1, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("c", new NuGetVersion(2, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("c", new NuGetVersion(3, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("d", new NuGetVersion(1, 0, 0), new[] { new Packaging.Core.PackageDependency("e", new VersionRange(new NuGetVersion(1, 0, 0))) }, true, null),
                new SourcePackageDependencyInfo("d", new NuGetVersion(2, 0, 0), new[] { new Packaging.Core.PackageDependency("e", new VersionRange(new NuGetVersion(1, 0, 0))) }, true, null),
                new SourcePackageDependencyInfo("d", new NuGetVersion(3, 0, 0), new[] { new Packaging.Core.PackageDependency("e", new VersionRange(new NuGetVersion(1, 0, 0))) }, true, null),
                new SourcePackageDependencyInfo("d", new NuGetVersion(4, 0, 0), new[] { new Packaging.Core.PackageDependency("e", new VersionRange(new NuGetVersion(1, 0, 0))) }, true, null),
                new SourcePackageDependencyInfo("e", new NuGetVersion(1, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("e", new NuGetVersion(2, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("f", new NuGetVersion(1, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("f", new NuGetVersion(2, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("f", new NuGetVersion(3, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("f", new NuGetVersion(4, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
            };

            var sourceRepositoryProvider = CreateSource(packages);

            // Set up NuGetProject
            var fwk45 = NuGetFramework.Parse("net45");

            var installedPackages = new List<NuGet.Packaging.PackageReference>
            {
                new NuGet.Packaging.PackageReference(new PackageIdentity("a", new NuGetVersion(1, 0, 0)), fwk45, true),
                new NuGet.Packaging.PackageReference(new PackageIdentity("b", new NuGetVersion(1, 0, 0)), fwk45, true),
                new NuGet.Packaging.PackageReference(new PackageIdentity("c", new NuGetVersion(2, 0, 0)), fwk45, true),
                new NuGet.Packaging.PackageReference(new PackageIdentity("d", new NuGetVersion(2, 0, 0)), fwk45, true),
                // No package "e" even though "d" depends on it (the user must have done an uninstall-package with a -force option)
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
                var targets = new List<PackageIdentity>
                  {
                    new PackageIdentity("b", new NuGetVersion(2, 0, 0)),
                    new PackageIdentity("c", new NuGetVersion(3, 0, 0)),
                  };

                var result = await nuGetPackageManager.PreviewUpdatePackagesAsync(
                    targets,
                    new List<NuGetProject> { nuGetProject },
                    new ResolutionContext(),
                    new TestNuGetProjectContext(),
                    sourceRepositoryProvider.GetRepositories(),
                    sourceRepositoryProvider.GetRepositories(),
                    CancellationToken.None);

                // Assert
                var resulting = result.Select(a => Tuple.Create(a.PackageIdentity, a.NuGetProjectActionType)).ToArray();

                var expected = new List<Tuple<PackageIdentity, NuGetProjectActionType>>();
                Expected(expected, "a", new NuGetVersion(1, 0, 0), new NuGetVersion(2, 0, 0));
                Expected(expected, "b", new NuGetVersion(1, 0, 0), new NuGetVersion(2, 0, 0));
                Expected(expected, "c", new NuGetVersion(2, 0, 0), new NuGetVersion(3, 0, 0));

                Assert.True(Compare(resulting, expected));
            }
        }

        [Fact]
        public async Task TestPacManInstallPackageWhichUsesExistingDependency()
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
        public async Task TestPacManInstallPackageWhichUpdatesExistingDependencyDueToDependencyBehavior()
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
                    Common.NullLogger.Instance,
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
        public async Task TestPacManPreviewUninstallWithRemoveDependencies()
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
                var uninstallationContext = new UninstallationContext(removeDependencies: true);
                var packageActions = (await nuGetPackageManager.PreviewUninstallPackageAsync(projectA,
                    packageIdentity2.Id, uninstallationContext, testNuGetProjectContext, token)).ToList();

                Assert.Equal(2, packageActions.Count);
                Assert.Equal(packageIdentity2, packageActions[0].PackageIdentity);
                Assert.Equal(NuGetProjectActionType.Uninstall, packageActions[0].NuGetProjectActionType);
                Assert.Null(packageActions[0].SourceRepository);
                Assert.Equal(packageIdentity0, packageActions[1].PackageIdentity);
                Assert.Equal(NuGetProjectActionType.Uninstall, packageActions[1].NuGetProjectActionType);
                Assert.Null(packageActions[1].SourceRepository);
            }
        }

        [Fact]
        public async Task TestPacManUninstallWithRemoveDependenciesWithVDependency()
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

                // Main Act
                Exception exception = null;
                try
                {
                    var uninstallationContext = new UninstallationContext(removeDependencies: true);
                    await nuGetPackageManager.UninstallPackageAsync(projectA, packageIdentity2.Id,
                        uninstallationContext, testNuGetProjectContext, token);
                }
                catch (InvalidOperationException ex)
                {
                    exception = ex;
                }
                catch (AggregateException ex)
                {
                    exception = ExceptionUtility.Unwrap(ex);
                }

                Assert.NotNull(exception);
                Assert.True(exception is InvalidOperationException);
                Assert.Equal("Unable to uninstall 'jQuery.1.6.4' because 'jQuery.UI.Combined.1.11.2' depends on it.",
                    exception.Message);
            }
        }

        [Fact]
        public async Task TestPacManUninstallWithForceRemove()
        {
            // Arrange
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV3OnlySourceRepositoryProvider();
            using (var testSolutionManager = new TestSolutionManager())
            {
                var testSettings = NullSettings.Instance;
                var token = CancellationToken.None;
                var resolutionContext = new ResolutionContext();
                var testNuGetProjectContext = new TestNuGetProjectContext();
                var packagesFolderPath = PackagesFolderPathUtility.GetPackagesFolderPath(testSolutionManager, testSettings);
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    testSettings,
                    testSolutionManager,
                    deleteOnRestartManager);

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
                    resolutionContext, testNuGetProjectContext, sourceRepositoryProvider.GetRepositories().First(), null, token);

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

                // Main Act
                var uninstallationContext = new UninstallationContext(removeDependencies: false, forceRemove: true);
                await nuGetPackageManager.UninstallPackageAsync(msBuildNuGetProject, "jQuery",
                    uninstallationContext, testNuGetProjectContext, token);

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
        public async Task TestPacManInstallWithIgnoreDependencies()
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
        public async Task TestPacManThrowsPackageNotFound()
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
        public async Task TestPacManThrowsLatestVersionNotFound()
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
        public async Task TestPacManInstallPackageWithDeepDependency()
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
        public async Task TestPacManInstallPackageBindingRedirectsWithDeepDependency()
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
        public async Task TestPacManInstallPackageBindingRedirectsDisabledWithDeepDependency()
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
        public Task TestPacManGetInstalledPackagesByDependencyOrder()
        {
            return TestPacManGetInstalledPackagesByDependencyOrderInternal(deletePackages: false);
        }

        [Fact]
        public Task TestPacManGetUnrestoredPackagesByDependencyOrderDeleteTrue()
        {
            return TestPacManGetInstalledPackagesByDependencyOrderInternal(deletePackages: true);
        }

        private async Task TestPacManGetInstalledPackagesByDependencyOrderInternal(bool deletePackages)
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

        [Fact]
        public async Task TestPacManPreviewInstallPackageWithDeepDependency()
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
                var packageIdentity = _packageWithDeepDependency[6]; // WindowsAzure.Storage.4.3.0

                // Pre-Assert
                // Check that the packages.config file does not exist
                Assert.False(File.Exists(packagesConfigPath));
                // Check that there are no packages returned by PackagesConfigProject
                var packagesInPackagesConfig = (await msBuildNuGetProject.PackagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(0, packagesInPackagesConfig.Count);
                Assert.Equal(0, msBuildNuGetProjectSystem.References.Count);

                // Act
                var packageActions = (await nuGetPackageManager.PreviewInstallPackageAsync(msBuildNuGetProject, packageIdentity,
                    new ResolutionContext(), new TestNuGetProjectContext(), sourceRepositoryProvider.GetRepositories().First(), null, token)).ToList();

                // Assert
                Assert.Equal(7, packageActions.Count);
                var soleSourceRepository = sourceRepositoryProvider.GetRepositories().Single();
                for (var i = 0; i < 7; i++)
                {
                    Assert.Equal(_packageWithDeepDependency[i], packageActions[i].PackageIdentity, PackageIdentity.Comparer);
                    Assert.Equal(NuGetProjectActionType.Install, packageActions[i].NuGetProjectActionType);
                    Assert.Equal(soleSourceRepository.PackageSource.Source,
                        packageActions[i].SourceRepository.PackageSource.Source);
                }
            }
        }

        [Fact]
        public async Task TestPacManPreviewUninstallPackageWithDeepDependency()
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
                Assert.Equal(packageIdentity, packagesInPackagesConfig[6].PackageIdentity);
                Assert.Equal(msBuildNuGetProject.ProjectSystem.TargetFramework, packagesInPackagesConfig[6].TargetFramework);

                // Main Act
                var packageActions = (await nuGetPackageManager.PreviewUninstallPackageAsync(msBuildNuGetProject, _packageWithDeepDependency[6],
                    new UninstallationContext(removeDependencies: true), new TestNuGetProjectContext(), token)).ToList();
                Assert.Equal(7, packageActions.Count);
                var soleSourceRepository = sourceRepositoryProvider.GetRepositories().Single();
                Assert.Equal(_packageWithDeepDependency[6], packageActions[0].PackageIdentity);
                Assert.Equal(NuGetProjectActionType.Uninstall, packageActions[0].NuGetProjectActionType);
                Assert.Equal(_packageWithDeepDependency[2], packageActions[1].PackageIdentity);
                Assert.Equal(NuGetProjectActionType.Uninstall, packageActions[1].NuGetProjectActionType);
                Assert.Equal(_packageWithDeepDependency[5], packageActions[2].PackageIdentity);
                Assert.Equal(NuGetProjectActionType.Uninstall, packageActions[2].NuGetProjectActionType);
                Assert.Equal(_packageWithDeepDependency[4], packageActions[3].PackageIdentity);
                Assert.Equal(NuGetProjectActionType.Uninstall, packageActions[3].NuGetProjectActionType);
                Assert.Equal(_packageWithDeepDependency[1], packageActions[4].PackageIdentity);
                Assert.Equal(NuGetProjectActionType.Uninstall, packageActions[4].NuGetProjectActionType);
                Assert.Equal(_packageWithDeepDependency[3], packageActions[5].PackageIdentity);
                Assert.Equal(NuGetProjectActionType.Uninstall, packageActions[5].NuGetProjectActionType);
                Assert.Equal(_packageWithDeepDependency[0], packageActions[6].PackageIdentity);
                Assert.Equal(NuGetProjectActionType.Uninstall, packageActions[6].NuGetProjectActionType);
            }
        }

        //[Fact]
        public async Task TestPacManInstallPackageTargetingASPNetCore50()
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
        public async Task TestPacManInstallMvcTargetingNet45()
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
        public async Task TestPacManPreviewUpdatePackagesSimple()
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

                var msBuildNuGetProject = testSolutionManager.AddNewMSBuildProject();
                var msBuildNuGetProjectSystem = msBuildNuGetProject.ProjectSystem as TestMSBuildNuGetProjectSystem;
                var packagesConfigPath = msBuildNuGetProject.PackagesConfigNuGetProject.FullPath;
                var packageIdentity0 = _packageWithDependents[0]; // jQuery.1.4.4

                var resolutionContext = new ResolutionContext();
                var resolvedPackage = await NuGetPackageManager.GetLatestVersionAsync(
                    packageIdentity0.Id,
                    msBuildNuGetProject,
                    new ResolutionContext(),
                    sourceRepositoryProvider.GetRepositories().First(),
                    Common.NullLogger.Instance,
                    token);

                var packageLatest = new PackageIdentity(packageIdentity0.Id, resolvedPackage.LatestVersion);

                // Pre-Assert
                // Check that the packages.config file does not exist
                Assert.False(File.Exists(packagesConfigPath));
                // Check that there are no packages returned by PackagesConfigProject
                var packagesInPackagesConfig = (await msBuildNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(0, packagesInPackagesConfig.Count);
                Assert.Equal(0, msBuildNuGetProjectSystem.References.Count);

                // Act
                await nuGetPackageManager.InstallPackageAsync(msBuildNuGetProject, packageIdentity0,
                    resolutionContext, new TestNuGetProjectContext(), sourceRepositoryProvider.GetRepositories().First(), null, token);

                // Assert
                // Check that the packages.config file exists after the installation
                Assert.True(File.Exists(packagesConfigPath));
                // Check the number of packages and packages returned by PackagesConfigProject after the installation
                packagesInPackagesConfig = (await msBuildNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(1, packagesInPackagesConfig.Count);
                Assert.Equal(packageIdentity0, packagesInPackagesConfig[0].PackageIdentity);
                Assert.Equal(msBuildNuGetProject.ProjectSystem.TargetFramework, packagesInPackagesConfig[0].TargetFramework);

                // Main Act
                var packageActions = (await nuGetPackageManager.PreviewUpdatePackagesAsync(
                    new List<NuGetProject> { msBuildNuGetProject },
                    new ResolutionContext(DependencyBehavior.Highest, false, true, VersionConstraints.None),
                    new TestNuGetProjectContext(),
                    sourceRepositoryProvider.GetRepositories(),
                    sourceRepositoryProvider.GetRepositories(),
                    token)).ToList();

                // Assert
                Assert.Equal(2, packageActions.Count);
                Assert.True(packageIdentity0.Equals(packageActions[0].PackageIdentity));
                Assert.Equal(NuGetProjectActionType.Uninstall, packageActions[0].NuGetProjectActionType);
                Assert.True(packageLatest.Equals(packageActions[1].PackageIdentity));
                Assert.Equal(NuGetProjectActionType.Install, packageActions[1].NuGetProjectActionType);
                Assert.Equal(sourceRepositoryProvider.GetRepositories().Single().PackageSource.Source,
                    packageActions[1].SourceRepository.PackageSource.Source);
            }
        }

        [Fact]
        public async Task TestPacManPreviewUpdatePackageWithTargetPrereleaseInProject()
        {
            // Arrange

            // Set up Package Source
            var packages = new List<SourcePackageDependencyInfo>
            {
                new SourcePackageDependencyInfo("a", new NuGetVersion(1, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("a", new NuGetVersion(2, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("a", new NuGetVersion(3, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("b", new NuGetVersion(1, 0, 0, "beta"), new[] { new Packaging.Core.PackageDependency("a", new VersionRange(new NuGetVersion(1, 0, 0))) }, true, null),
                new SourcePackageDependencyInfo("b", new NuGetVersion(2, 0, 0, "beta"), new[] { new Packaging.Core.PackageDependency("a", new VersionRange(new NuGetVersion(1, 0, 0))) }, true, null),
            };

            var sourceRepositoryProvider = CreateSource(packages);

            // Set up NuGetProject

            var fwk45 = NuGetFramework.Parse("net45");

            var installedPackages = new List<NuGet.Packaging.PackageReference>
            {
                new NuGet.Packaging.PackageReference(new PackageIdentity("a", new NuGetVersion(1, 0, 0)), fwk45, true),
                new NuGet.Packaging.PackageReference(new PackageIdentity("b", new NuGetVersion(1, 0, 0, "beta")), fwk45, true),
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
                var result = await nuGetPackageManager.PreviewUpdatePackagesAsync(
                    "a",
                    new List<NuGetProject> { nuGetProject },
                    new ResolutionContext(),
                    new TestNuGetProjectContext(),
                    sourceRepositoryProvider.GetRepositories(),
                    sourceRepositoryProvider.GetRepositories(),
                    CancellationToken.None);

                // Assert
                var resulting = result.Select(a => Tuple.Create(a.PackageIdentity, a.NuGetProjectActionType)).ToArray();

                var expected = new List<Tuple<PackageIdentity, NuGetProjectActionType>>();
                Expected(expected, "a", new NuGetVersion(1, 0, 0), new NuGetVersion(3, 0, 0));

                Assert.True(Compare(resulting, expected));
            }
        }

        [Fact]
        public async Task TestPacManPreviewUpdatePackageNotExistsInProject()
        {
            // Arrange

            // Set up Package Source
            var packages = new List<SourcePackageDependencyInfo>
            {
                new SourcePackageDependencyInfo("a", new NuGetVersion(1, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("a", new NuGetVersion(2, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("a", new NuGetVersion(3, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("b", new NuGetVersion(1, 0, 0, "beta"), new[] { new Packaging.Core.PackageDependency("a", new VersionRange(new NuGetVersion(1, 0, 0))) }, true, null),
                new SourcePackageDependencyInfo("b", new NuGetVersion(2, 0, 0, "beta"), new[] { new Packaging.Core.PackageDependency("a", new VersionRange(new NuGetVersion(1, 0, 0))) }, true, null),
            };

            var sourceRepositoryProvider = CreateSource(packages);

            // Set up NuGetProject

            var fwk45 = NuGetFramework.Parse("net45");

            var installedPackages = new List<NuGet.Packaging.PackageReference>
            {
                new NuGet.Packaging.PackageReference(new PackageIdentity("a", new NuGetVersion(1, 0, 0)), fwk45, true),
                new NuGet.Packaging.PackageReference(new PackageIdentity("b", new NuGetVersion(1, 0, 0, "beta")), fwk45, true),
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
                var result = await nuGetPackageManager.PreviewUpdatePackagesAsync(
                    "c",
                    new List<NuGetProject> { nuGetProject },
                    new ResolutionContext(
                        dependencyBehavior: DependencyBehavior.Lowest,
                        includePrelease: false,
                        includeUnlisted: false,
                        versionConstraints: VersionConstraints.ExactMajor | VersionConstraints.ExactMinor | VersionConstraints.ExactPatch | VersionConstraints.ExactRelease,
                        gatherCache: new GatherCache(),
                        sourceCacheContext: NullSourceCacheContext.Instance),
                    new TestNuGetProjectContext(),
                    sourceRepositoryProvider.GetRepositories(),
                    sourceRepositoryProvider.GetRepositories(),
                    CancellationToken.None);

                // Assert
                Assert.True(!result.Any());
            }
        }

        [Fact]
        public async Task TestPacManPreviewUpdatePackageALLPrereleaseInProject()
        {
            // Arrange

            // Set up Package Source
            var packages = new List<SourcePackageDependencyInfo>
            {
                new SourcePackageDependencyInfo("a", new NuGetVersion(1, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("a", new NuGetVersion(2, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("a", new NuGetVersion(3, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("b", new NuGetVersion(1, 0, 0, "beta"), new[] { new Packaging.Core.PackageDependency("a", new VersionRange(new NuGetVersion(1, 0, 0))) }, true, null),
                new SourcePackageDependencyInfo("b", new NuGetVersion(2, 0, 0, "beta"), new[] { new Packaging.Core.PackageDependency("a", new VersionRange(new NuGetVersion(1, 0, 0))) }, true, null),
            };

            var sourceRepositoryProvider = CreateSource(packages);

            // Set up NuGetProject
            var fwk45 = NuGetFramework.Parse("net45");

            var installedPackages = new List<NuGet.Packaging.PackageReference>
            {
                new NuGet.Packaging.PackageReference(new PackageIdentity("a", new NuGetVersion(1, 0, 0)), fwk45, true),
                new NuGet.Packaging.PackageReference(new PackageIdentity("b", new NuGetVersion(1, 0, 0, "beta")), fwk45, true),
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
                var result = await nuGetPackageManager.PreviewUpdatePackagesAsync(
                    new List<NuGetProject> { nuGetProject },
                    new ResolutionContext(),
                    new TestNuGetProjectContext(),
                    sourceRepositoryProvider.GetRepositories(),
                    sourceRepositoryProvider.GetRepositories(),
                    CancellationToken.None);

                // Assert
                var resulting = result.Select(a => Tuple.Create(a.PackageIdentity, a.NuGetProjectActionType)).ToArray();

                var expected = new List<Tuple<PackageIdentity, NuGetProjectActionType>>();
                Expected(expected, "a", new NuGetVersion(1, 0, 0), new NuGetVersion(3, 0, 0));
                Expected(expected, "b", new NuGetVersion(1, 0, 0, "beta"), new NuGetVersion(2, 0, 0, "beta"));

                Assert.True(Compare(resulting, expected));
            }
        }

        [Fact]
        public async Task TestPacManPreviewUpdatePrereleasePackageNoPreFlagSpecified()
        {
            // Arrange

            // Set up Package Source
            var packages = new List<SourcePackageDependencyInfo>
            {
                new SourcePackageDependencyInfo("a", new NuGetVersion(1, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("a", new NuGetVersion(2, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("a", new NuGetVersion(3, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("b", new NuGetVersion(1, 0, 0, "beta"), new[] { new Packaging.Core.PackageDependency("a", new VersionRange(new NuGetVersion(1, 0, 0))) }, true, null),
                new SourcePackageDependencyInfo("b", new NuGetVersion(2, 0, 0, "beta"), new[] { new Packaging.Core.PackageDependency("a", new VersionRange(new NuGetVersion(1, 0, 0))) }, true, null),
            };

            var sourceRepositoryProvider = CreateSource(packages);

            // Set up NuGetProject
            var fwk45 = NuGetFramework.Parse("net45");

            var installedPackages = new List<NuGet.Packaging.PackageReference>
            {
                new NuGet.Packaging.PackageReference(new PackageIdentity("a", new NuGetVersion(1, 0, 0)), fwk45, true),
                new NuGet.Packaging.PackageReference(new PackageIdentity("b", new NuGetVersion(1, 0, 0, "beta")), fwk45, true),
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
                var result = await nuGetPackageManager.PreviewUpdatePackagesAsync(
                    "b",
                    new List<NuGetProject> { nuGetProject },
                    new ResolutionContext(),
                    new TestNuGetProjectContext(),
                    sourceRepositoryProvider.GetRepositories(),
                    sourceRepositoryProvider.GetRepositories(),
                    CancellationToken.None);

                // Assert
                var resulting = result.Select(a => Tuple.Create(a.PackageIdentity, a.NuGetProjectActionType)).ToArray();

                var expected = new List<Tuple<PackageIdentity, NuGetProjectActionType>>();

                Assert.True(Compare(resulting, expected));
            }
        }

        [Fact]
        public async Task TestPacManPreviewUpdateMulti()
        {
            // Arrange

            // Set up Package Source
            var packages = new List<SourcePackageDependencyInfo>
            {
                new SourcePackageDependencyInfo("a", new NuGetVersion(1, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("a", new NuGetVersion(2, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("a", new NuGetVersion(3, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("b", new NuGetVersion(1, 0, 0), new[] { new Packaging.Core.PackageDependency("a", new VersionRange(new NuGetVersion(1, 0, 0))) }, true, null),
                new SourcePackageDependencyInfo("b", new NuGetVersion(2, 0, 0), new[] { new Packaging.Core.PackageDependency("a", new VersionRange(new NuGetVersion(2, 0, 0))) }, true, null),
                new SourcePackageDependencyInfo("b", new NuGetVersion(3, 0, 0), new[] { new Packaging.Core.PackageDependency("a", new VersionRange(new NuGetVersion(2, 0, 0))) }, true, null),
                new SourcePackageDependencyInfo("c", new NuGetVersion(1, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("c", new NuGetVersion(2, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("c", new NuGetVersion(3, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("d", new NuGetVersion(1, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("d", new NuGetVersion(2, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("d", new NuGetVersion(3, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("d", new NuGetVersion(4, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("e", new NuGetVersion(1, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("e", new NuGetVersion(2, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("f", new NuGetVersion(1, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("f", new NuGetVersion(2, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("f", new NuGetVersion(3, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("f", new NuGetVersion(4, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
            };

            var sourceRepositoryProvider = CreateSource(packages);

            // Set up NuGetProject
            var fwk45 = NuGetFramework.Parse("net45");

            var installedPackages = new List<NuGet.Packaging.PackageReference>
            {
                new NuGet.Packaging.PackageReference(new PackageIdentity("a", new NuGetVersion(1, 0, 0)), fwk45, true),
                new NuGet.Packaging.PackageReference(new PackageIdentity("b", new NuGetVersion(1, 0, 0)), fwk45, true),
                new NuGet.Packaging.PackageReference(new PackageIdentity("c", new NuGetVersion(2, 0, 0)), fwk45, true),
                new NuGet.Packaging.PackageReference(new PackageIdentity("d", new NuGetVersion(2, 0, 0)), fwk45, true),
                new NuGet.Packaging.PackageReference(new PackageIdentity("e", new NuGetVersion(1, 0, 0)), fwk45, true),
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

                var targets = new List<PackageIdentity>
            {
                new PackageIdentity("b", new NuGetVersion(2, 0, 0)),
                new PackageIdentity("c", new NuGetVersion(3, 0, 0)),
                new PackageIdentity("d", new NuGetVersion(3, 0, 0)),
            };

                var result = await nuGetPackageManager.PreviewUpdatePackagesAsync(
                    targets,
                    new List<NuGetProject> { nuGetProject },
                    new ResolutionContext(),
                    new TestNuGetProjectContext(),
                    sourceRepositoryProvider.GetRepositories(),
                    sourceRepositoryProvider.GetRepositories(),
                    CancellationToken.None);

                // Assert
                var resulting = result.Select(a => Tuple.Create(a.PackageIdentity, a.NuGetProjectActionType)).ToArray();

                var expected = new List<Tuple<PackageIdentity, NuGetProjectActionType>>();
                Expected(expected, "a", new NuGetVersion(1, 0, 0), new NuGetVersion(2, 0, 0));
                Expected(expected, "b", new NuGetVersion(1, 0, 0), new NuGetVersion(2, 0, 0));
                Expected(expected, "c", new NuGetVersion(2, 0, 0), new NuGetVersion(3, 0, 0));
                Expected(expected, "d", new NuGetVersion(2, 0, 0), new NuGetVersion(3, 0, 0));

                Assert.True(Compare(resulting, expected));
            }
        }

        [Fact]
        public async Task TestPacMan_PreviewUpdatePackagesAsync_MultiProjects()
        {
            // Arrange

            // Set up Package Source
            var packages = new List<SourcePackageDependencyInfo>
            {
                new SourcePackageDependencyInfo("a", new NuGetVersion(1, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("a", new NuGetVersion(2, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("b", new NuGetVersion(1, 0, 0), new[] { new Packaging.Core.PackageDependency("a", new VersionRange(new NuGetVersion(1, 0, 0))) }, true, null),
                new SourcePackageDependencyInfo("b", new NuGetVersion(2, 0, 0), new[] { new Packaging.Core.PackageDependency("a", new VersionRange(new NuGetVersion(2, 0, 0))) }, true, null),
                new SourcePackageDependencyInfo("c", new NuGetVersion(1, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("c", new NuGetVersion(2, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("d", new NuGetVersion(1, 0, 0), new[] { new Packaging.Core.PackageDependency("c", new VersionRange(new NuGetVersion(1, 0, 0))) }, true, null),
                new SourcePackageDependencyInfo("d", new NuGetVersion(2, 0, 0), new[] { new Packaging.Core.PackageDependency("c", new VersionRange(new NuGetVersion(2, 0, 0))) }, true, null),
            };

            var sourceRepositoryProvider = CreateSource(packages);

            // Set up NuGetProject
            var fwk45 = NuGetFramework.Parse("net45");

            var installedPackagesA = new List<NuGet.Packaging.PackageReference>
            {
                new NuGet.Packaging.PackageReference(new PackageIdentity("a", new NuGetVersion(1, 0, 0)), fwk45, true),
                new NuGet.Packaging.PackageReference(new PackageIdentity("b", new NuGetVersion(1, 0, 0)), fwk45, true),
            };

            var installedPackagesB = new List<NuGet.Packaging.PackageReference>
            {
                new NuGet.Packaging.PackageReference(new PackageIdentity("d", new NuGetVersion(1, 0, 0)), fwk45, true),
            };

            var nuGetProjectA = new TestNuGetProject(installedPackagesA);
            var nuGetProjectB = new TestNuGetProject(installedPackagesB);

            // Create Package Manager
            using (var solutionManager = new TestSolutionManager())
            {
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    NullSettings.Instance,
                    solutionManager,
                    new TestDeleteOnRestartManager());

                // Main Act
                var targets = new List<PackageIdentity>
                  {
                    new PackageIdentity("b", new NuGetVersion(2, 0, 0))
                  };

                var result = await nuGetPackageManager.PreviewUpdatePackagesAsync(
                    targets,
                    new List<NuGetProject> { nuGetProjectA, nuGetProjectB },
                    new ResolutionContext(),
                    new TestNuGetProjectContext(),
                    sourceRepositoryProvider.GetRepositories(),
                    sourceRepositoryProvider.GetRepositories(),
                    CancellationToken.None);

                // Assert
                var resulting = result.Select(a => Tuple.Create(a.PackageIdentity, a.NuGetProjectActionType)).ToArray();

                var expected = new List<Tuple<PackageIdentity, NuGetProjectActionType>>();
                Expected(expected, "a", new NuGetVersion(1, 0, 0), new NuGetVersion(2, 0, 0));
                Expected(expected, "b", new NuGetVersion(1, 0, 0), new NuGetVersion(2, 0, 0));

                Assert.True(Compare(resulting, expected));
            }
        }

        [Fact]
        public async Task TestPacMan_PreviewUpdatePackagesAsync_MultiProjects_MultiDependencies()
        {
            // Arrange

            // Set up Package Source
            var packages = new List<SourcePackageDependencyInfo>
            {
                new SourcePackageDependencyInfo("a", new NuGetVersion(1, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("a", new NuGetVersion(2, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("b", new NuGetVersion(1, 0, 0), new[] { new Packaging.Core.PackageDependency("a", new VersionRange(new NuGetVersion(1, 0, 0))) }, true, null),
                new SourcePackageDependencyInfo("b", new NuGetVersion(2, 0, 0), new[] { new Packaging.Core.PackageDependency("a", new VersionRange(new NuGetVersion(2, 0, 0))) }, true, null),
                new SourcePackageDependencyInfo("c", new NuGetVersion(1, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("c", new NuGetVersion(2, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("d", new NuGetVersion(1, 0, 0), new[] { new Packaging.Core.PackageDependency("c", new VersionRange(new NuGetVersion(1, 0, 0))) }, true, null),
                new SourcePackageDependencyInfo("d", new NuGetVersion(2, 0, 0), new[] { new Packaging.Core.PackageDependency("c", new VersionRange(new NuGetVersion(2, 0, 0))) }, true, null),
            };

            var sourceRepositoryProvider = CreateSource(packages);

            // Set up NuGetProject
            var fwk45 = NuGetFramework.Parse("net45");

            var installedPackagesA = new List<NuGet.Packaging.PackageReference>
            {
                new NuGet.Packaging.PackageReference(new PackageIdentity("a", new NuGetVersion(1, 0, 0)), fwk45, true),
                new NuGet.Packaging.PackageReference(new PackageIdentity("b", new NuGetVersion(1, 0, 0)), fwk45, true),
            };

            var installedPackagesB = new List<NuGet.Packaging.PackageReference>
            {
                new NuGet.Packaging.PackageReference(new PackageIdentity("c", new NuGetVersion(1, 0, 0)), fwk45, true),
                new NuGet.Packaging.PackageReference(new PackageIdentity("d", new NuGetVersion(1, 0, 0)), fwk45, true),
            };

            var nuGetProjectA = new TestNuGetProject("projectA", installedPackagesA);
            var nuGetProjectB = new TestNuGetProject("projectB", installedPackagesB);

            // Create Package Manager
            using (var solutionManager = new TestSolutionManager())
            {
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    NullSettings.Instance,
                    solutionManager,
                    new TestDeleteOnRestartManager());

                // Main Act
                var targets = new List<PackageIdentity>
                  {
                    new PackageIdentity("b", new NuGetVersion(2, 0, 0)),
                    new PackageIdentity("d", new NuGetVersion(2, 0, 0))
                  };

                var result = await nuGetPackageManager.PreviewUpdatePackagesAsync(
                    targets,
                    new List<NuGetProject> { nuGetProjectA, nuGetProjectB },
                    new ResolutionContext(),
                    new TestNuGetProjectContext(),
                    sourceRepositoryProvider.GetRepositories(),
                    sourceRepositoryProvider.GetRepositories(),
                    CancellationToken.None);

                // Assert
                var resulting = result.Where(a => a.NuGetProjectActionType == NuGetProjectActionType.Install)
                    .Select(a => Tuple.Create(a.Project as TestNuGetProject, a.PackageIdentity)).ToArray();

                var expected = new List<Tuple<TestNuGetProject, PackageIdentity>>();
                expected.Add(Tuple.Create(nuGetProjectA, new PackageIdentity("a", new NuGetVersion(2, 0, 0))));
                expected.Add(Tuple.Create(nuGetProjectA, new PackageIdentity("b", new NuGetVersion(2, 0, 0))));
                expected.Add(Tuple.Create(nuGetProjectB, new PackageIdentity("c", new NuGetVersion(2, 0, 0))));
                expected.Add(Tuple.Create(nuGetProjectB, new PackageIdentity("d", new NuGetVersion(2, 0, 0))));

                Assert.Equal(4, resulting.Length);
                Assert.True(PreviewResultsCompare(resulting, expected));
            }
        }

        [Fact]
        public async Task TestPacManPreviewInstallPackageFollowingForceUninstall()
        {
            // Arrange

            // Set up Package Source
            var packages = new List<SourcePackageDependencyInfo>
            {
                new SourcePackageDependencyInfo("a", new NuGetVersion(1, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("a", new NuGetVersion(2, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("a", new NuGetVersion(3, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("b", new NuGetVersion(1, 0, 0), new[] { new Packaging.Core.PackageDependency("a", new VersionRange(new NuGetVersion(1, 0, 0))) }, true, null),
                new SourcePackageDependencyInfo("b", new NuGetVersion(2, 0, 0), new[] { new Packaging.Core.PackageDependency("a", new VersionRange(new NuGetVersion(2, 0, 0))) }, true, null),
                new SourcePackageDependencyInfo("b", new NuGetVersion(3, 0, 0), new[] { new Packaging.Core.PackageDependency("a", new VersionRange(new NuGetVersion(2, 0, 0))) }, true, null),
                new SourcePackageDependencyInfo("c", new NuGetVersion(1, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("c", new NuGetVersion(2, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("c", new NuGetVersion(3, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("d", new NuGetVersion(1, 0, 0), new[] { new Packaging.Core.PackageDependency("e", new VersionRange(new NuGetVersion(1, 0, 0))) }, true, null),
                new SourcePackageDependencyInfo("d", new NuGetVersion(2, 0, 0), new[] { new Packaging.Core.PackageDependency("e", new VersionRange(new NuGetVersion(1, 0, 0))) }, true, null),
                new SourcePackageDependencyInfo("d", new NuGetVersion(3, 0, 0), new[] { new Packaging.Core.PackageDependency("e", new VersionRange(new NuGetVersion(1, 0, 0))) }, true, null),
                new SourcePackageDependencyInfo("d", new NuGetVersion(4, 0, 0), new[] { new Packaging.Core.PackageDependency("e", new VersionRange(new NuGetVersion(1, 0, 0))) }, true, null),
                new SourcePackageDependencyInfo("e", new NuGetVersion(1, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("e", new NuGetVersion(2, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("f", new NuGetVersion(1, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("f", new NuGetVersion(2, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("f", new NuGetVersion(3, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("f", new NuGetVersion(4, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
            };

            var sourceRepositoryProvider = CreateSource(packages);

            // Set up NuGetProject
            var fwk45 = NuGetFramework.Parse("net45");

            var installedPackages = new List<NuGet.Packaging.PackageReference>
            {
                new NuGet.Packaging.PackageReference(new PackageIdentity("a", new NuGetVersion(1, 0, 0)), fwk45, true),
                new NuGet.Packaging.PackageReference(new PackageIdentity("b", new NuGetVersion(1, 0, 0)), fwk45, true),
                new NuGet.Packaging.PackageReference(new PackageIdentity("c", new NuGetVersion(2, 0, 0)), fwk45, true),
                new NuGet.Packaging.PackageReference(new PackageIdentity("d", new NuGetVersion(2, 0, 0)), fwk45, true),
                // No package "e" even though "d" depends on it (the user must have done an uninstall-package with a -force option)
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

                var target = new PackageIdentity("f", new NuGetVersion(3, 0, 0));

                var result = await nuGetPackageManager.PreviewInstallPackageAsync(
                    nuGetProject,
                    target,
                    new ResolutionContext(),
                    new TestNuGetProjectContext(),
                    sourceRepositoryProvider.GetRepositories(),
                    sourceRepositoryProvider.GetRepositories(),
                    CancellationToken.None);

                // Assert
                var resulting = result.Select(a => Tuple.Create(a.PackageIdentity, a.NuGetProjectActionType)).ToArray();

                var expected = new List<Tuple<PackageIdentity, NuGetProjectActionType>>();
                Expected(expected, target.Id, target.Version);

                Assert.True(Compare(resulting, expected));
            }
        }

        [Fact]
        public async Task TestPacManPreviewInstallPackageWithNonTargetDependency()
        {
            // Arrange

            // Set up Package Source
            var packages = new List<SourcePackageDependencyInfo>
            {
                new SourcePackageDependencyInfo("a", new NuGetVersion(1, 0, 0), new[] { new Packaging.Core.PackageDependency("b", new VersionRange(new NuGetVersion(1, 0, 0), true, new NuGetVersion(1, 0, 0), true)) }, true, null),
                new SourcePackageDependencyInfo("a", new NuGetVersion(2, 0, 0), new[] { new Packaging.Core.PackageDependency("b", new VersionRange(new NuGetVersion(2, 0, 0), true, new NuGetVersion(2, 0, 0), true)),
                    new Packaging.Core.PackageDependency("c", new VersionRange(new NuGetVersion(2, 0, 0), true, new NuGetVersion(2, 0, 0), true))}, true, null),
                new SourcePackageDependencyInfo("b", new NuGetVersion(1, 0, 0), new[] { new Packaging.Core.PackageDependency("d", new VersionRange(new NuGetVersion(1, 0, 0), true, new NuGetVersion(1, 0, 0), true)) }, true, null),
                new SourcePackageDependencyInfo("b", new NuGetVersion(2, 0, 0), new[] { new Packaging.Core.PackageDependency("d", new VersionRange(new NuGetVersion(2, 0, 0), true, new NuGetVersion(2, 0, 0), true)) }, true, null),
                new SourcePackageDependencyInfo("c", new NuGetVersion(1, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("c", new NuGetVersion(2, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("d", new NuGetVersion(1, 0, 0), new[] { new Packaging.Core.PackageDependency("e", new VersionRange(new NuGetVersion(1, 0, 0), true, new NuGetVersion(1, 0, 0), true)) }, true, null),
                new SourcePackageDependencyInfo("d", new NuGetVersion(2, 0, 0), new[] { new Packaging.Core.PackageDependency("e", new VersionRange(new NuGetVersion(2, 0, 0), true, new NuGetVersion(2, 0, 0), true)) }, true, null),
                new SourcePackageDependencyInfo("e", new NuGetVersion(1, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("e", new NuGetVersion(2, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("f", new NuGetVersion(1, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("f", new NuGetVersion(2, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("f", new NuGetVersion(3, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("f", new NuGetVersion(4, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
            };

            var sourceRepositoryProvider = CreateSource(packages);

            // Set up NuGetProject
            var fwk45 = NuGetFramework.Parse("net45");

            var installedPackages = new List<NuGet.Packaging.PackageReference>
            {
                new NuGet.Packaging.PackageReference(new PackageIdentity("a", new NuGetVersion(1, 0, 0)), fwk45, true),
                new NuGet.Packaging.PackageReference(new PackageIdentity("b", new NuGetVersion(1, 0, 0)), fwk45, true),
                new NuGet.Packaging.PackageReference(new PackageIdentity("d", new NuGetVersion(1, 0, 0)), fwk45, true),
                new NuGet.Packaging.PackageReference(new PackageIdentity("e", new NuGetVersion(1, 0, 0)), fwk45, true),
                new NuGet.Packaging.PackageReference(new PackageIdentity("f", new NuGetVersion(1, 0, 0)), fwk45, true),
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

                var target = new PackageIdentity("d", new NuGetVersion(2, 0, 0));

                var result = await nuGetPackageManager.PreviewInstallPackageAsync(
                    nuGetProject,
                    target,
                    new ResolutionContext(),
                    new TestNuGetProjectContext(),
                    sourceRepositoryProvider.GetRepositories(),
                    sourceRepositoryProvider.GetRepositories(),
                    CancellationToken.None);

                // Assert
                var resulting = result.Select(a => Tuple.Create(a.PackageIdentity, a.NuGetProjectActionType)).ToList();

                var expected = new List<Tuple<PackageIdentity, NuGetProjectActionType>>();
                Expected(expected, target.Id, new NuGetVersion(1, 0, 0), target.Version);
                Expected(expected, "e", new NuGetVersion(1, 0, 0), new NuGetVersion(2, 0, 0));
                Expected(expected, "b", new NuGetVersion(1, 0, 0), new NuGetVersion(2, 0, 0));
                Expected(expected, "a", new NuGetVersion(1, 0, 0), new NuGetVersion(2, 0, 0));
                Expected(expected, "c", new NuGetVersion(2, 0, 0));

                Assert.True(Compare(resulting, expected));
            }
        }

        [Fact]
        public async Task TestPacManPreviewUpdateMultiWithConflict()
        {
            // Arrange

            // Set up Package Source
            var packages = new List<SourcePackageDependencyInfo>
            {
                new SourcePackageDependencyInfo("a", new NuGetVersion(1, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("a", new NuGetVersion(2, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("a", new NuGetVersion(3, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("b", new NuGetVersion(1, 0, 0), new[] { new Packaging.Core.PackageDependency("a", new VersionRange(new NuGetVersion(1, 0, 0), true, new NuGetVersion(1, 0, 0), true)) }, true, null),
                new SourcePackageDependencyInfo("b", new NuGetVersion(2, 0, 0), new[] { new Packaging.Core.PackageDependency("a", new VersionRange(new NuGetVersion(2, 0, 0), true, new NuGetVersion(2, 0, 0), true)) }, true, null),
                new SourcePackageDependencyInfo("b", new NuGetVersion(3, 0, 0), new[] { new Packaging.Core.PackageDependency("a", new VersionRange(new NuGetVersion(3, 0, 0), true, new NuGetVersion(3, 0, 0), true)) }, true, null),
                new SourcePackageDependencyInfo("c", new NuGetVersion(1, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("c", new NuGetVersion(2, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("c", new NuGetVersion(3, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
            };

            var sourceRepositoryProvider = CreateSource(packages);

            // Set up NuGetProject
            var fwk45 = NuGetFramework.Parse("net45");

            var installedPackages = new List<NuGet.Packaging.PackageReference>
            {
                new NuGet.Packaging.PackageReference(new PackageIdentity("a", new NuGetVersion(1, 0, 0)), fwk45, true),
                new NuGet.Packaging.PackageReference(new PackageIdentity("b", new NuGetVersion(1, 0, 0)), fwk45, true),
                new NuGet.Packaging.PackageReference(new PackageIdentity("c", new NuGetVersion(2, 0, 0)), fwk45, true),
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

                var targets = new List<PackageIdentity>
                {
                    new PackageIdentity("a", new NuGetVersion(2, 0, 0)),
                    new PackageIdentity("b", new NuGetVersion(3, 0, 0)),
                };

                try
                {
                    await nuGetPackageManager.PreviewUpdatePackagesAsync(
                        targets,
                        new List<NuGetProject> { nuGetProject },
                        new ResolutionContext(),
                        new TestNuGetProjectContext(),
                        sourceRepositoryProvider.GetRepositories(),
                        sourceRepositoryProvider.GetRepositories(),
                        CancellationToken.None);

                    Assert.True(false);
                }
                catch (Exception e)
                {
                    Assert.IsType(typeof(InvalidOperationException), e);
                }
            }
        }

        [Fact]
        public async Task TestPacManPreviewUpdateMultiWithDowngradeConflict()
        {
            // Arrange

            // Set up Package Source
            var packages = new List<SourcePackageDependencyInfo>
            {
                new SourcePackageDependencyInfo("a", new NuGetVersion(1, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("a", new NuGetVersion(2, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("a", new NuGetVersion(3, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("b", new NuGetVersion(1, 0, 0), new[] { new Packaging.Core.PackageDependency("a", new VersionRange(new NuGetVersion(1, 0, 0), true, new NuGetVersion(1, 0, 0), true)) }, true, null),
                new SourcePackageDependencyInfo("b", new NuGetVersion(2, 0, 0), new[] { new Packaging.Core.PackageDependency("a", new VersionRange(new NuGetVersion(2, 0, 0), true, new NuGetVersion(3, 0, 0), true)) }, true, null),
                new SourcePackageDependencyInfo("b", new NuGetVersion(3, 0, 0), new[] { new Packaging.Core.PackageDependency("a", new VersionRange(new NuGetVersion(2, 0, 0), true, new NuGetVersion(3, 0, 0), false)) }, true, null),
                new SourcePackageDependencyInfo("c", new NuGetVersion(1, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("c", new NuGetVersion(2, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("c", new NuGetVersion(3, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
            };

            var sourceRepositoryProvider = CreateSource(packages);

            // Set up NuGetProject
            var fwk45 = NuGetFramework.Parse("net45");

            var installedPackages = new List<NuGet.Packaging.PackageReference>
            {
                new NuGet.Packaging.PackageReference(new PackageIdentity("a", new NuGetVersion(2, 0, 0)), fwk45, true),
                new NuGet.Packaging.PackageReference(new PackageIdentity("b", new NuGetVersion(3, 0, 0)), fwk45, true),
                new NuGet.Packaging.PackageReference(new PackageIdentity("c", new NuGetVersion(2, 0, 0)), fwk45, true),
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

                var targets = new List<PackageIdentity>
                {
                    new PackageIdentity("a", new NuGetVersion(3, 0, 0)),
                    new PackageIdentity("c", new NuGetVersion(3, 0, 0)),
                };

                try
                {
                    await nuGetPackageManager.PreviewUpdatePackagesAsync(
                        targets,
                        new List<NuGetProject> { nuGetProject },
                        new ResolutionContext(),
                        new TestNuGetProjectContext(),
                        sourceRepositoryProvider.GetRepositories(),
                        sourceRepositoryProvider.GetRepositories(),
                        CancellationToken.None);

                    Assert.True(false);
                }
                catch (Exception e)
                {
                    Assert.IsType(typeof(InvalidOperationException), e);
                }
            }
        }

        // [Fact] -- This test performs update but verifies for a specific version
        //           This is not going to work as newer versions are uploaded
        public async Task TestPacManPreviewUpdatePackages()
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

                var msBuildNuGetProject = testSolutionManager.AddNewMSBuildProject();
                var msBuildNuGetProjectSystem = msBuildNuGetProject.ProjectSystem as TestMSBuildNuGetProjectSystem;
                var packagesConfigPath = msBuildNuGetProject.PackagesConfigNuGetProject.FullPath;
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

                // Main Act
                var packageActions = (await nuGetPackageManager.PreviewUpdatePackagesAsync(
                    new List<NuGetProject> { msBuildNuGetProject },
                    new ResolutionContext(DependencyBehavior.Highest, false, true, VersionConstraints.None),
                    new TestNuGetProjectContext(),
                    sourceRepositoryProvider.GetRepositories(),
                    sourceRepositoryProvider.GetRepositories(),
                    token)).ToList();

                // Assert
                Assert.Equal(4, packageActions.Count);
                Assert.True(_morePackageWithDependents[0].Equals(packageActions[0].PackageIdentity));
                Assert.Equal(NuGetProjectActionType.Uninstall, packageActions[0].NuGetProjectActionType);
                Assert.True(_morePackageWithDependents[3].Equals(packageActions[1].PackageIdentity));
                Assert.Equal(NuGetProjectActionType.Uninstall, packageActions[1].NuGetProjectActionType);
                Assert.True(_morePackageWithDependents[1].Equals(packageActions[2].PackageIdentity));
                Assert.Equal(NuGetProjectActionType.Install, packageActions[2].NuGetProjectActionType);
                Assert.Equal(sourceRepositoryProvider.GetRepositories().Single().PackageSource.Source,
                    packageActions[2].SourceRepository.PackageSource.Source);
                Assert.True(_morePackageWithDependents[4].Equals(packageActions[3].PackageIdentity));
                Assert.Equal(NuGetProjectActionType.Install, packageActions[3].NuGetProjectActionType);
                Assert.Equal(sourceRepositoryProvider.GetRepositories().Single().PackageSource.Source,
                    packageActions[3].SourceRepository.PackageSource.Source);
            }
        }

        [Fact]
        public async Task TestPacManPreviewReinstallPackages()
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

                var msBuildNuGetProject = testSolutionManager.AddNewMSBuildProject();
                var msBuildNuGetProjectSystem = msBuildNuGetProject.ProjectSystem as TestMSBuildNuGetProjectSystem;
                var packagesConfigPath = msBuildNuGetProject.PackagesConfigNuGetProject.FullPath;
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

                var resolutionContext = new ResolutionContext(
                    DependencyBehavior.Highest,
                    false,
                    true,
                    VersionConstraints.ExactMajor | VersionConstraints.ExactMinor | VersionConstraints.ExactPatch | VersionConstraints.ExactRelease);

                // Main Act
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
            }
        }

        [Fact]
        public async Task TestPacManReinstallPackages()
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
        public async Task TestPacManReinstallSpecificPackage()
        {
            // Arrange

            // Set up Package Source
            var packages = new List<SourcePackageDependencyInfo>
            {
                new SourcePackageDependencyInfo("a", new NuGetVersion(1, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("a", new NuGetVersion(2, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("a", new NuGetVersion(3, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("b", new NuGetVersion(1, 0, 0), new[]
                {
                    new Packaging.Core.PackageDependency("a", new VersionRange(new NuGetVersion(1, 0, 0))),
                    new Packaging.Core.PackageDependency("c", new VersionRange(new NuGetVersion(2, 0, 0))),
                }, true, null),
                new SourcePackageDependencyInfo("b", new NuGetVersion(2, 0, 0), new[]
                {
                    new Packaging.Core.PackageDependency("a", new VersionRange(new NuGetVersion(2, 0, 0))),
                    new Packaging.Core.PackageDependency("c", new VersionRange(new NuGetVersion(2, 0, 0))),
                }, true, null),
                new SourcePackageDependencyInfo("b", new NuGetVersion(3, 0, 0), new[]
                {
                    new Packaging.Core.PackageDependency("a", new VersionRange(new NuGetVersion(3, 0, 0))),
                }, true, null),
                new SourcePackageDependencyInfo("c", new NuGetVersion(1, 0, 0), new[]
                {
                    new Packaging.Core.PackageDependency("d", new VersionRange(new NuGetVersion(2, 0, 0))),
                }, true, null),
                new SourcePackageDependencyInfo("c", new NuGetVersion(2, 0, 0), new[]
                {
                    new Packaging.Core.PackageDependency("d", new VersionRange(new NuGetVersion(2, 0, 0))),
                }, true, null),
                new SourcePackageDependencyInfo("c", new NuGetVersion(3, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("d", new NuGetVersion(1, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("d", new NuGetVersion(2, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("d", new NuGetVersion(3, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("d", new NuGetVersion(4, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("e", new NuGetVersion(1, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("e", new NuGetVersion(2, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("f", new NuGetVersion(1, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("f", new NuGetVersion(2, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("f", new NuGetVersion(3, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("f", new NuGetVersion(4, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
            };

            var sourceRepositoryProvider = CreateSource(packages);

            // Set up NuGetProject
            var fwk45 = NuGetFramework.Parse("net45");

            var installedPackages = new List<NuGet.Packaging.PackageReference>
            {
                new NuGet.Packaging.PackageReference(new PackageIdentity("a", new NuGetVersion(1, 0, 0)), fwk45, true),
                new NuGet.Packaging.PackageReference(new PackageIdentity("b", new NuGetVersion(1, 0, 0)), fwk45, true),
                new NuGet.Packaging.PackageReference(new PackageIdentity("c", new NuGetVersion(2, 0, 0)), fwk45, true),
                new NuGet.Packaging.PackageReference(new PackageIdentity("d", new NuGetVersion(2, 0, 0)), fwk45, true),
                new NuGet.Packaging.PackageReference(new PackageIdentity("e", new NuGetVersion(1, 0, 0)), fwk45, true),
                new NuGet.Packaging.PackageReference(new PackageIdentity("f", new NuGetVersion(3, 0, 0)), fwk45, true),
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
        public async Task TestPacManOpenReadmeFile()
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
                var packagePathResolver = new PackagePathResolver(packagesFolderPath);

                var msBuildNuGetProject = testSolutionManager.AddNewMSBuildProject();
                var msBuildNuGetProjectSystem = msBuildNuGetProject.ProjectSystem as TestMSBuildNuGetProjectSystem;
                var packagesConfigPath = msBuildNuGetProject.PackagesConfigNuGetProject.FullPath;
                var packageIdentity = new PackageIdentity("elmah", new NuGetVersion("1.2.2"));

                // Pre-Assert
                // Check that the packages.config file does not exist
                Assert.False(File.Exists(packagesConfigPath));
                // Check that there are no packages returned by PackagesConfigProject
                var packagesInPackagesConfig = (await msBuildNuGetProject.PackagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(0, packagesInPackagesConfig.Count);
                Assert.Equal(0, msBuildNuGetProjectSystem.References.Count);

                // Act
                // Set the direct install on the execution context of INuGetProjectContext before installing a package
                var testNuGetProjectContext = new TestNuGetProjectContext();
                testNuGetProjectContext.TestExecutionContext = new TestExecutionContext(packageIdentity);
                await nuGetPackageManager.InstallPackageAsync(msBuildNuGetProject, packageIdentity,
                    new ResolutionContext(), testNuGetProjectContext, sourceRepositoryProvider.GetRepositories().First(), null, token);

                // Assert
                // Check that the packages.config file exists after the installation
                Assert.True(File.Exists(packagesConfigPath));
                // Check the number of packages and packages returned by PackagesConfigProject after the installation
                packagesInPackagesConfig = (await msBuildNuGetProject.PackagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(2, packagesInPackagesConfig.Count);
                Assert.Equal(packageIdentity, packagesInPackagesConfig[0].PackageIdentity);
                Assert.Equal(msBuildNuGetProject.ProjectSystem.TargetFramework, packagesInPackagesConfig[0].TargetFramework);
                Assert.Equal(1, testNuGetProjectContext.TestExecutionContext.FilesOpened.Count);
                Assert.True(string.Equals(Path.Combine(packagePathResolver.GetInstallPath(packageIdentity), "ReadMe.txt"),
                    testNuGetProjectContext.TestExecutionContext.FilesOpened.First(), StringComparison.OrdinalIgnoreCase));
            }
        }

        [Fact]
        public async Task TestPacManPreviewInstallPackageIdUnexpectedDowngrade()
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

                var msBuildNuGetProject = testSolutionManager.AddNewMSBuildProject("TestProjectName");
                var msBuildNuGetProjectSystem = msBuildNuGetProject.ProjectSystem as TestMSBuildNuGetProjectSystem;
                var packagesConfigPath = msBuildNuGetProject.PackagesConfigNuGetProject.FullPath;
                var packageId = "Newtonsoft.Json";
                var testNuGetProjectContext = new TestNuGetProjectContext();
                var primarySourceRepository = sourceRepositoryProvider.GetRepositories().First();

                // Pre-Assert
                // Check that the packages.config file does not exist
                Assert.False(File.Exists(packagesConfigPath));
                // Check that there are no packages returned by PackagesConfigProject
                var packagesInPackagesConfig = (await msBuildNuGetProject.PackagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(0, packagesInPackagesConfig.Count);
                Assert.Equal(0, msBuildNuGetProjectSystem.References.Count);

                var resolutionContext = new ResolutionContext(
                    DependencyBehavior.Lowest,
                    includePrelease: false,
                    includeUnlisted: false,
                    versionConstraints: VersionConstraints.None);

                // Act
                await nuGetPackageManager.InstallPackageAsync(msBuildNuGetProject, packageId, resolutionContext,
                    testNuGetProjectContext, primarySourceRepository, null, token);

                // Check that the packages.config file does not exist
                Assert.True(File.Exists(packagesConfigPath));

                // Check that there are no packages returned by PackagesConfigProject
                packagesInPackagesConfig = (await msBuildNuGetProject.PackagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(1, packagesInPackagesConfig.Count);
                Assert.Equal(1, msBuildNuGetProjectSystem.References.Count);

                Exception exception = null;
                try
                {
                    var packageActions = (await nuGetPackageManager.PreviewInstallPackageAsync(msBuildNuGetProject, packageId,
                        resolutionContext, testNuGetProjectContext, primarySourceRepository, null, token)).ToList();
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                Assert.NotNull(exception);
                Assert.True(exception is InvalidOperationException);
                Assert.Contains("Package 'Newtonsoft.Json.", exception.Message);
                Assert.Contains("already exists in project 'TestProjectName'", exception.Message);
            }
        }

        [Fact]
        public async Task TestPacManPreviewInstallPackageThrowsDependencyDowngrade()
        {
            // Arrange
            var packageIdentityA = new PackageIdentity("DotNetOpenAuth.OAuth.Core", new NuGetVersion("4.3.2.13293"));
            var packageIdentityB1 = new PackageIdentity("DotNetOpenAuth.Core", new NuGetVersion("4.3.2.13293"));
            var packageIdentityB2 = new PackageIdentity("DotNetOpenAuth.Core", new NuGetVersion("4.3.4.13329"));
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

                var msBuildNuGetProject = testSolutionManager.AddNewMSBuildProject();
                var msBuildNuGetProjectSystem = msBuildNuGetProject.ProjectSystem as TestMSBuildNuGetProjectSystem;
                var packagesConfigPath = msBuildNuGetProject.PackagesConfigNuGetProject.FullPath;
                var testNuGetProjectContext = new TestNuGetProjectContext();
                var primarySourceRepository = sourceRepositoryProvider.GetRepositories().First();

                // Pre-Assert
                // Check that the packages.config file does not exist
                Assert.False(File.Exists(packagesConfigPath));
                // Check that there are no packages returned by PackagesConfigProject
                var packagesInPackagesConfig = (await msBuildNuGetProject.PackagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(0, packagesInPackagesConfig.Count);
                Assert.Equal(0, msBuildNuGetProjectSystem.References.Count);

                // Act
                await nuGetPackageManager.InstallPackageAsync(msBuildNuGetProject, packageIdentityB2, new ResolutionContext(DependencyBehavior.Lowest, includePrelease: true, includeUnlisted: false, versionConstraints: VersionConstraints.None),
                    testNuGetProjectContext, primarySourceRepository, null, token);

                // Check that the packages.config file does not exist
                Assert.True(File.Exists(packagesConfigPath));
                // Check that there are no packages returned by PackagesConfigProject
                packagesInPackagesConfig = (await msBuildNuGetProject.PackagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(2, packagesInPackagesConfig.Count);

                Exception exception = null;
                try
                {
                    var packageActions = (await nuGetPackageManager.PreviewInstallPackageAsync(msBuildNuGetProject, packageIdentityA,
                        new ResolutionContext(), testNuGetProjectContext, primarySourceRepository, null, token)).ToList();
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                Assert.NotNull(exception);
                Assert.True(exception is InvalidOperationException);
                Assert.Equal(
                    string.Format("Unable to resolve dependencies. '{0} {1}' is not compatible with '{2} {3} constraint: {4} (= {5})'.",
                        packageIdentityB2.Id,
                        packageIdentityB2.Version,
                        packageIdentityA.Id,
                        packageIdentityA.Version,
                        packageIdentityB1.Id,
                        packageIdentityB1.Version),
                    exception.Message);
            }
        }

        [Fact]
        public async Task TestPacManPreviewInstallDependencyVersionHighestAndPrerelease()
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
                var dotnetrdfPackageIdentity = new PackageIdentity("dotnetrdf", new NuGetVersion("1.0.8-prerelease1"));
                var resolutionContext = new ResolutionContext(DependencyBehavior.Highest, includePrelease: true, includeUnlisted: false, versionConstraints: VersionConstraints.None);

                var newtonsoftJsonPackageId = "newtonsoft.json";

                // Act
                var resolvedPackage = await NuGetPackageManager.GetLatestVersionAsync(
                    newtonsoftJsonPackageId,
                    msBuildNuGetProject,
                    resolutionContext,
                    primarySourceRepository,
                    Common.NullLogger.Instance,
                    CancellationToken.None);

                var newtonsoftJsonPackageIdentity = new PackageIdentity(newtonsoftJsonPackageId, resolvedPackage.LatestVersion);

                var nuGetProjectActions = (await nuGetPackageManager.PreviewInstallPackageAsync(msBuildNuGetProject, dotnetrdfPackageIdentity, resolutionContext,
                    new TestNuGetProjectContext(), primarySourceRepository, null, CancellationToken.None)).ToList();

                // Assert
                Assert.Equal(4, nuGetProjectActions.Count);
                var newtonsoftJsonAction = nuGetProjectActions.Where(a => a.PackageIdentity.Equals(newtonsoftJsonPackageIdentity)).FirstOrDefault();

                Assert.NotNull(newtonsoftJsonAction);
            }
        }

        [Fact]
        public async Task TestPacManUpdateDependencyToPrereleaseVersion()
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
                    Common.NullLogger.Instance,
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
        public async Task TestPacManPreviewInstallWithAllowedVersionsConstraint()
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
                var newtonsoftJsonPackageId = "newtonsoft.json";
                var newtonsoftJsonPackageIdentity = new PackageIdentity(newtonsoftJsonPackageId, NuGetVersion.Parse("4.5.11"));
                var primarySourceRepository = sourceRepositoryProvider.GetRepositories().Single();
                var resolutionContext = new ResolutionContext();
                var testNuGetProjectContext = new TestNuGetProjectContext();

                // Act
                await nuGetPackageManager.InstallPackageAsync(msBuildNuGetProject, newtonsoftJsonPackageIdentity,
                    resolutionContext, testNuGetProjectContext, primarySourceRepository, null, token);

                // Assert
                // Check that the packages.config file exists after the installation
                Assert.True(File.Exists(packagesConfigPath));
                // Check the number of packages and packages returned by PackagesConfigProject after the installation
                var packagesInPackagesConfig = (await msBuildNuGetProject.PackagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(1, packagesInPackagesConfig.Count);
                Assert.Equal(newtonsoftJsonPackageIdentity, packagesInPackagesConfig[0].PackageIdentity);
                Assert.Equal(msBuildNuGetProject.ProjectSystem.TargetFramework, packagesInPackagesConfig[0].TargetFramework);
                var installedPackages = await msBuildNuGetProject.GetInstalledPackagesAsync(token);
                var newtonsoftJsonPackageReference = installedPackages.Where(pr => pr.PackageIdentity.Equals(newtonsoftJsonPackageIdentity)).FirstOrDefault();

                Assert.Null(newtonsoftJsonPackageReference.AllowedVersions);

                const string newPackagesConfig = @"<?xml version='1.0' encoding='utf-8'?>
  <packages>
    <package id='Newtonsoft.Json' version='4.5.11' allowedVersions='[4.0,5.0)' targetFramework='net45' />
  </packages> ";

                File.WriteAllText(packagesConfigPath, newPackagesConfig);

                // Check that the packages.config file exists after the installation
                Assert.True(File.Exists(packagesConfigPath));
                // Check the number of packages and packages returned by PackagesConfigProject after the installation
                packagesInPackagesConfig = (await msBuildNuGetProject.PackagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(1, packagesInPackagesConfig.Count);
                Assert.Equal(newtonsoftJsonPackageIdentity, packagesInPackagesConfig[0].PackageIdentity);
                Assert.Equal(msBuildNuGetProject.ProjectSystem.TargetFramework, packagesInPackagesConfig[0].TargetFramework);
                installedPackages = await msBuildNuGetProject.GetInstalledPackagesAsync(token);
                newtonsoftJsonPackageReference = installedPackages.Where(pr => pr.PackageIdentity.Equals(newtonsoftJsonPackageIdentity)).FirstOrDefault();

                Assert.NotNull(newtonsoftJsonPackageReference.AllowedVersions);

                Exception exception = null;
                try
                {
                    // Main Act
                    await nuGetPackageManager.PreviewInstallPackageAsync(msBuildNuGetProject, newtonsoftJsonPackageId,
                        resolutionContext, testNuGetProjectContext, primarySourceRepository, null, token);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                Assert.NotNull(exception);
            }
        }

        [Fact]
        public async Task TestPacManPreviewUpdateWithAllowedVersionsConstraintAsync()
        {
            // Arrange
            using var localPackageSourceDir = TestDirectory.Create();
            using var testSolutionManager = new TestSolutionManager();
            // create packages
            var testPackageId = new Dictionary<string, IEnumerable<string>>
            {
                ["new.json"] = new[] { "4.5.11", "5.0.8" },
                ["web.Infrastructure"] = new[] { "0.0.0.1", "1.0.0.0" },
            };
            await SimpleTestPackageUtility.CreateFullPackagesAsync(localPackageSourceDir, testPackageId);
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(new PackageSource(localPackageSourceDir));

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
            var newtonsoftJsonPackageIdentity = new PackageIdentity("new.json", NuGetVersion.Parse("4.5.11"));
            var primarySourceRepository = sourceRepositoryProvider.GetRepositories().Single();
            var resolutionContext = new ResolutionContext(DependencyBehavior.Highest, false, true, VersionConstraints.None);
            var testNuGetProjectContext = new TestNuGetProjectContext();

            // Act
            await nuGetPackageManager.InstallPackageAsync(msBuildNuGetProject, newtonsoftJsonPackageIdentity,
                resolutionContext, testNuGetProjectContext, primarySourceRepository, null, token);

            await nuGetPackageManager.InstallPackageAsync(msBuildNuGetProject, new PackageIdentity("web.infrastructure", new NuGetVersion("1.0.0.0")),
                resolutionContext, testNuGetProjectContext, primarySourceRepository, null, token);

            // Assert
            // Check that the packages.config file exists after the installation
            Assert.True(File.Exists(packagesConfigPath));
            // Check the number of packages and packages returned by PackagesConfigProject after the installation
            var packagesInPackagesConfig = (await msBuildNuGetProject.PackagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
            Assert.Equal(2, packagesInPackagesConfig.Count);
            Assert.Contains(packagesInPackagesConfig, pr => pr.PackageIdentity.Equals(newtonsoftJsonPackageIdentity) && pr.TargetFramework == msBuildNuGetProject.ProjectSystem.TargetFramework);
            var installedPackages = await msBuildNuGetProject.GetInstalledPackagesAsync(token);
            var newtonsoftJsonPackageReference = installedPackages.Where(pr => pr.PackageIdentity.Equals(newtonsoftJsonPackageIdentity)).FirstOrDefault();

            Assert.Null(newtonsoftJsonPackageReference.AllowedVersions);

            const string newPackagesConfig = @"<?xml version='1.0' encoding='utf-8'?>
  <packages>
    <package id='web.Infrastructure' version='1.0.0.0' targetFramework='net45' />
    <package id='New.Json' version='4.5.11' allowedVersions='[4.0,5.0)' targetFramework='net45' />
  </packages> ";

            File.WriteAllText(packagesConfigPath, newPackagesConfig);

            // Check that the packages.config file exists after the installation
            Assert.True(File.Exists(packagesConfigPath));
            // Check the number of packages and packages returned by PackagesConfigProject after the installation
            packagesInPackagesConfig = (await msBuildNuGetProject.PackagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
            Assert.Equal(2, packagesInPackagesConfig.Count);
            Assert.Contains(packagesInPackagesConfig, pr => pr.PackageIdentity.Equals(newtonsoftJsonPackageIdentity) && pr.TargetFramework == msBuildNuGetProject.ProjectSystem.TargetFramework);
            installedPackages = await msBuildNuGetProject.GetInstalledPackagesAsync(token);
            newtonsoftJsonPackageReference = installedPackages.Where(pr => pr.PackageIdentity.Equals(newtonsoftJsonPackageIdentity)).FirstOrDefault();

            Assert.NotNull(newtonsoftJsonPackageReference.AllowedVersions);

            // Main Act
            List<NuGetProjectAction> nuGetProjectActions = (await nuGetPackageManager.PreviewUpdatePackagesAsync(
                new List<NuGetProject> { msBuildNuGetProject },
                resolutionContext,
                testNuGetProjectContext,
                sourceRepositoryProvider.GetRepositories(),
                sourceRepositoryProvider.GetRepositories(),
                token)).ToList();

            // web.infrastructure has no updates. However, newtonsoft.json has updates but does not satisfy the version range
            // Hence, no nuget project actions to perform
            Assert.Empty(nuGetProjectActions);
        }

        [Fact]
        public async Task TestPacManPreviewUpdate_AllowedVersionsConstraint_RestrictHighestVersionAsync()
        {
            // Arrange
            using var localPackageSourceDir = TestDirectory.Create();
            using var testSolutionManager = new TestSolutionManager();
            var testPackageId = new Dictionary<string, IEnumerable<string>>
            {
                ["new.json"] = new[] { "4.5.11", "5.0.8" },
                ["web.Infrastructure"] = new[] { "0.0.0.1", "1.0.0.0" },
            };
            await SimpleTestPackageUtility.CreateFullPackagesAsync(localPackageSourceDir, testPackageId);
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(new PackageSource(localPackageSourceDir));

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
            var newJsonPackageId = "new.json";
            var newJsonPackageIdentity = new PackageIdentity(newJsonPackageId, NuGetVersion.Parse("4.5.11"));
            var primarySourceRepository = sourceRepositoryProvider.GetRepositories().Single();
            var resolutionContext = new ResolutionContext(DependencyBehavior.Lowest, false, true, VersionConstraints.None);
            var testNuGetProjectContext = new TestNuGetProjectContext();

            // Act
            await nuGetPackageManager.InstallPackageAsync(msBuildNuGetProject, newJsonPackageIdentity,
                resolutionContext, testNuGetProjectContext, primarySourceRepository, null, token);

            await nuGetPackageManager.InstallPackageAsync(msBuildNuGetProject, new PackageIdentity("web.infrastructure", new NuGetVersion("1.0.0.0")),
                resolutionContext, testNuGetProjectContext, primarySourceRepository, null, token);

            // Assert
            // Check that the packages.config file exists after the installation
            Assert.True(File.Exists(packagesConfigPath));
            // Check the number of packages and packages returned by PackagesConfigProject after the installation
            var packagesInPackagesConfig = (await msBuildNuGetProject.PackagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
            Assert.Equal(2, packagesInPackagesConfig.Count);
            Assert.Contains(packagesInPackagesConfig, pr => pr.PackageIdentity.Equals(newJsonPackageIdentity) && pr.TargetFramework == msBuildNuGetProject.ProjectSystem.TargetFramework);
            var installedPackages = await msBuildNuGetProject.GetInstalledPackagesAsync(token);
            var newtonsoftJsonPackageReference = installedPackages.Where(pr => pr.PackageIdentity.Equals(newJsonPackageIdentity)).FirstOrDefault();

            Assert.Null(newtonsoftJsonPackageReference.AllowedVersions);

            const string newPackagesConfig = @"<?xml version='1.0' encoding='utf-8'?>
  <packages>
    <package id='web.infrastructure' version='1.0.0.0' targetFramework='net45' />
    <package id='new.json' version='4.5.11' allowedVersions='[4.0,6.0)' targetFramework='net45' />
  </packages>";

            File.WriteAllText(packagesConfigPath, newPackagesConfig);

            // Check that the packages.config file exists after the installation
            Assert.True(File.Exists(packagesConfigPath));
            // Check the number of packages and packages returned by PackagesConfigProject after the installation
            packagesInPackagesConfig = (await msBuildNuGetProject.PackagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
            Assert.Equal(2, packagesInPackagesConfig.Count);
            Assert.Contains(packagesInPackagesConfig, pr => pr.PackageIdentity.Equals(newJsonPackageIdentity) && pr.TargetFramework == msBuildNuGetProject.ProjectSystem.TargetFramework);
            installedPackages = await msBuildNuGetProject.GetInstalledPackagesAsync(token);
            newtonsoftJsonPackageReference = installedPackages.Where(pr => pr.PackageIdentity.Equals(newJsonPackageIdentity)).FirstOrDefault();

            Assert.NotNull(newtonsoftJsonPackageReference.AllowedVersions);

            var newJsonPackageIdentityAfterUpdate = new PackageIdentity(newJsonPackageId, NuGetVersion.Parse("5.0.8"));

            // Main Act
            IEnumerable<NuGetProjectAction> nuGetProjectActions = await nuGetPackageManager.PreviewUpdatePackagesAsync(
                new List<NuGetProject> { msBuildNuGetProject },
                resolutionContext,
                testNuGetProjectContext,
                sourceRepositoryProvider.GetRepositories(),
                sourceRepositoryProvider.GetRepositories(),
                token);

            // web.infrastructure has no updates. However, new.json has updates but should pick it as per the version constraint
            // Hence, 4.5.11 will be uninstalled and 5.0.8 will be installed
            Assert.Equal(2, nuGetProjectActions.Count());
            Assert.Contains(nuGetProjectActions, pr => pr.PackageIdentity.Equals(newJsonPackageIdentityAfterUpdate));
        }

        [Fact]
        public async Task TestPacManPreviewUpdateWithNoSource()
        {
            // Arrange
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(new List<NuGet.Configuration.PackageSource>());
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
                var newtonsoftJsonPackageId = "newtonsoft.json";
                var newtonsoftJsonPackageIdentity = new PackageIdentity(newtonsoftJsonPackageId, NuGetVersion.Parse("4.5.11"));

                var resolutionContext = new ResolutionContext(DependencyBehavior.Highest, false, true, VersionConstraints.None);
                var testNuGetProjectContext = new TestNuGetProjectContext();

                // Act

                // Update ALL - this should not fail - it should no-op

                var nuGetProjectActions = (await nuGetPackageManager.PreviewUpdatePackagesAsync(
                    new List<NuGetProject> { msBuildNuGetProject },
                    resolutionContext,
                    testNuGetProjectContext,
                    Enumerable.Empty<SourceRepository>(),
                    Enumerable.Empty<SourceRepository>(),
                    token)).ToList();

                // Hence, no nuget project actions to perform
                Assert.Equal(0, nuGetProjectActions.Count);
            }
        }

        [Fact]
        public async Task TestPacManInstallAspNetRazorJa()
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
        public async Task TestPacManInstallMicrosoftWebInfrastructure1000FromV2()
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
        public async Task TestPacManInstallMicrosoftWebInfrastructure1000FromV3()
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
        public async Task TestPacManInstallElmah11FromV2()
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
        public async Task TestPacManInstallElmah11FromV3()
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
        public async Task TestPacManInstall_SharpDX_DXGI_v263_WithNonReferencesInLibFolder()
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
                Assert.True(packagesInPackagesConfig.Any(p => p.PackageIdentity.Equals(sharpDXDXGIv263Package)));
            }
        }

        [Fact]
        public async Task TestPacManInstallPackageUnlistedFromV3()
        {
            // Arrange
            var packages = new List<SourcePackageDependencyInfo>
            {
                new SourcePackageDependencyInfo("a", new NuGetVersion(1, 0, 0), new[] { new Packaging.Core.PackageDependency("b", new VersionRange(new NuGetVersion(1, 0, 0))) }, true, null),
                new SourcePackageDependencyInfo("b", new NuGetVersion(1, 0, 0), new Packaging.Core.PackageDependency[] { }, false, null),
                new SourcePackageDependencyInfo("b", new NuGetVersion(2, 0, 0), new Packaging.Core.PackageDependency[] { }, false, null),
                new SourcePackageDependencyInfo("b", new NuGetVersion(3, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
            };

            var resourceProviders = new List<Lazy<INuGetResourceProvider>>();
            resourceProviders.Add(new Lazy<INuGetResourceProvider>(() => new TestDependencyInfoProvider(packages)));
            resourceProviders.Add(new Lazy<INuGetResourceProvider>(() => new TestMetadataProvider(packages)));

            var packageSource = new Configuration.PackageSource("http://a");
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

                Assert.True(resultIdentities.Contains(new PackageIdentity("a", new NuGetVersion(1, 0, 0))));
                Assert.True(resultIdentities.Contains(new PackageIdentity("b", new NuGetVersion(3, 0, 0))));

                //  and all the actions are Install
                foreach (var nugetProjectAction in result)
                {
                    Assert.Equal(nugetProjectAction.NuGetProjectActionType, NuGetProjectActionType.Install);
                }
            }
        }

        [Fact]
        public async Task TestPacManInstallPackageListedFromV3()
        {
            // Arrange
            var packages = new List<SourcePackageDependencyInfo>
            {
                new SourcePackageDependencyInfo("a", new NuGetVersion(1, 0, 0), new[] { new Packaging.Core.PackageDependency("b", new VersionRange(new NuGetVersion(1, 0, 0))) }, true, null),
                new SourcePackageDependencyInfo("a", new NuGetVersion(2, 0, 0), new[] { new Packaging.Core.PackageDependency("b", new VersionRange(new NuGetVersion(1, 0, 0))) }, true, null),
                new SourcePackageDependencyInfo("b", new NuGetVersion(1, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("b", new NuGetVersion(2, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("b", new NuGetVersion(3, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
            };

            var resourceProviders = new List<Lazy<INuGetResourceProvider>>();
            resourceProviders.Add(new Lazy<INuGetResourceProvider>(() => new TestDependencyInfoProvider(packages)));
            resourceProviders.Add(new Lazy<INuGetResourceProvider>(() => new TestMetadataProvider(packages)));

            var packageSource = new Configuration.PackageSource("http://a");
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

                Assert.True(resultIdentities.Contains(new PackageIdentity("a", new NuGetVersion(2, 0, 0))));
                Assert.True(resultIdentities.Contains(new PackageIdentity("b", new NuGetVersion(1, 0, 0))));

                //  and all the actions are Install
                foreach (var nugetProjectAction in result)
                {
                    Assert.Equal(nugetProjectAction.NuGetProjectActionType, NuGetProjectActionType.Install);
                }
            }
        }

        [Fact]
        public async Task TestPacManInstallPackage571FromV3()
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

                Assert.True(resultIdentities.Contains(new PackageIdentity("Umbraco", new NuGetVersion("5.1.0.175"))));

                //  and all the actions are Install
                foreach (var nugetProjectAction in result)
                {
                    Assert.Equal(nugetProjectAction.NuGetProjectActionType, NuGetProjectActionType.Install);
                }
            }
        }

        [Fact]
        public async Task TestPacManInstallPackageEFFromV3()
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

                Assert.True(resultIdentities.Contains(target));

                //  and all the actions are Install
                foreach (var nugetProjectAction in result)
                {
                    Assert.Equal(nugetProjectAction.NuGetProjectActionType, NuGetProjectActionType.Install);
                }
            }
        }

        [Fact]
        public async Task TestPacManInstallPackagePrereleaseDependenciesFromV2()
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

                Assert.True(resultIdentities.Contains(target));
                Assert.True(resultIdentities.Contains(new PackageIdentity("DependencyTestB", NuGetVersion.Parse("1.0.0"))));

                //  and all the actions are Install
                foreach (var nugetProjectAction in result)
                {
                    Assert.Equal(nugetProjectAction.NuGetProjectActionType, NuGetProjectActionType.Install);
                }
            }
        }

        [Fact]
        public async Task TestPacManInstallPackagePrereleaseDependenciesFromV2IncludePrerelease()
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

                Assert.True(resultIdentities.Contains(target));
                Assert.True(resultIdentities.Contains(new PackageIdentity("DependencyTestB", NuGetVersion.Parse("1.0.0-a"))));

                //  and all the actions are Install
                foreach (var nugetProjectAction in result)
                {
                    Assert.Equal(nugetProjectAction.NuGetProjectActionType, NuGetProjectActionType.Install);
                }
            }
        }

        [Fact]
        public async Task TestPacManInstallPackagePrerelease()
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

                Assert.True(resultIdentities.Contains(target));

                //  and all the actions are Install
                foreach (var nugetProjectAction in result)
                {
                    Assert.Equal(nugetProjectAction.NuGetProjectActionType, NuGetProjectActionType.Install);
                }
            }
        }

        [Fact]
        public async Task TestPacManInstallPackageOverExisting()
        {
            // Arrange
            var fwk46 = NuGetFramework.Parse("net46");
            var fwk45 = NuGetFramework.Parse("net45");
            var fwk4 = NuGetFramework.Parse("net4");

            var installedPackages = new List<NuGet.Packaging.PackageReference>
            {
                new NuGet.Packaging.PackageReference(new PackageIdentity("51Degrees.mobi", NuGetVersion.Parse("2.1.15.1")), fwk4, true),
                new NuGet.Packaging.PackageReference(new PackageIdentity("AspNetMvc", NuGetVersion.Parse("4.0.20710.0")), fwk45, true),
                new NuGet.Packaging.PackageReference(new PackageIdentity("AttributeRouting", NuGetVersion.Parse("3.5.6")), fwk4, true),
                new NuGet.Packaging.PackageReference(new PackageIdentity("AttributeRouting.Core", NuGetVersion.Parse("3.5.6")), fwk4, true),
                new NuGet.Packaging.PackageReference(new PackageIdentity("AttributeRouting.Core.Web", NuGetVersion.Parse("3.5.6")), fwk4, true),
                new NuGet.Packaging.PackageReference(new PackageIdentity("AutoMapper", NuGetVersion.Parse("3.3.1")), fwk45, true),
                new NuGet.Packaging.PackageReference(new PackageIdentity("Castle.Core", NuGetVersion.Parse("1.1.0")), fwk4, true),
                new NuGet.Packaging.PackageReference(new PackageIdentity("Castle.DynamicProxy", NuGetVersion.Parse("2.1.0")), fwk4, true),
                new NuGet.Packaging.PackageReference(new PackageIdentity("Clay", NuGetVersion.Parse("1.0")), fwk4, true),
                new NuGet.Packaging.PackageReference(new PackageIdentity("colorbox", NuGetVersion.Parse("1.4.29")), fwk45, true),
                new NuGet.Packaging.PackageReference(new PackageIdentity("elmah", NuGetVersion.Parse("1.2.0.1")), fwk4, true),
                new NuGet.Packaging.PackageReference(new PackageIdentity("elmah.corelibrary", NuGetVersion.Parse("1.2")), fwk4, true),
                new NuGet.Packaging.PackageReference(new PackageIdentity("EntityFramework", NuGetVersion.Parse("6.1.3")), fwk45, true),
                new NuGet.Packaging.PackageReference(new PackageIdentity("fasterflect", NuGetVersion.Parse("2.1.0")), fwk4, true),
                new NuGet.Packaging.PackageReference(new PackageIdentity("foolproof", NuGetVersion.Parse("0.9.4517")), fwk45, true),
                new NuGet.Packaging.PackageReference(new PackageIdentity("Glimpse", NuGetVersion.Parse("0.87")), fwk4, true),
                new NuGet.Packaging.PackageReference(new PackageIdentity("Glimpse.Elmah", NuGetVersion.Parse("0.9.3")), fwk4, true),
                new NuGet.Packaging.PackageReference(new PackageIdentity("Glimpse.Mvc3", NuGetVersion.Parse("0.87")), fwk4, true),
                new NuGet.Packaging.PackageReference(new PackageIdentity("jQuery", NuGetVersion.Parse("1.4.1")), fwk45, true),
                new NuGet.Packaging.PackageReference(new PackageIdentity("knockout.mapper.TypeScript.DefinitelyTyped", NuGetVersion.Parse("0.0.4")), fwk45, true),
                new NuGet.Packaging.PackageReference(new PackageIdentity("Knockout.Mapping", NuGetVersion.Parse("2.4.0")), fwk45, true),
                new NuGet.Packaging.PackageReference(new PackageIdentity("knockout.mapping.TypeScript.DefinitelyTyped", NuGetVersion.Parse("0.0.9")), fwk45, true),
                new NuGet.Packaging.PackageReference(new PackageIdentity("knockout.TypeScript.DefinitelyTyped", NuGetVersion.Parse("0.5.1")), fwk45, true),
                new NuGet.Packaging.PackageReference(new PackageIdentity("Knockout.Validation", NuGetVersion.Parse("1.0.1")), fwk45, true),
                new NuGet.Packaging.PackageReference(new PackageIdentity("knockoutjs", NuGetVersion.Parse("2.0.0")), fwk45, true),
                new NuGet.Packaging.PackageReference(new PackageIdentity("LINQtoCSV", NuGetVersion.Parse("1.2.0.0")), fwk4, true),
                new NuGet.Packaging.PackageReference(new PackageIdentity("log4net", NuGetVersion.Parse("2.0.3")), fwk45, true),
                new NuGet.Packaging.PackageReference(new PackageIdentity("Microsoft.AspNet.Mvc", NuGetVersion.Parse("4.0.40804.0")), fwk45, true),
                new NuGet.Packaging.PackageReference(new PackageIdentity("Microsoft.AspNet.Razor", NuGetVersion.Parse("2.0.30506.0")), fwk45, true),
                new NuGet.Packaging.PackageReference(new PackageIdentity("Microsoft.AspNet.WebPages", NuGetVersion.Parse("2.0.30506.0")), fwk45, true),
                new NuGet.Packaging.PackageReference(new PackageIdentity("Microsoft.Web.Infrastructure", NuGetVersion.Parse("1.0.0.0")), fwk4, true),
                new NuGet.Packaging.PackageReference(new PackageIdentity("MiniProfiler", NuGetVersion.Parse("3.1.1.140")), fwk45, true),
                new NuGet.Packaging.PackageReference(new PackageIdentity("MiniProfiler.EF6", NuGetVersion.Parse("3.0.11")), fwk45, true),
                new NuGet.Packaging.PackageReference(new PackageIdentity("MiniProfiler.MVC4", NuGetVersion.Parse("3.0.11")), fwk45, true),
                new NuGet.Packaging.PackageReference(new PackageIdentity("Mvc3CodeTemplatesCSharp", NuGetVersion.Parse("3.0.11214.0")), fwk4, true),
                new NuGet.Packaging.PackageReference(new PackageIdentity("MvcDiagnostics", NuGetVersion.Parse("3.0.10714.0")), fwk4, true),
                new NuGet.Packaging.PackageReference(new PackageIdentity("Newtonsoft.Json", NuGetVersion.Parse("6.0.8")), fwk45, true),
                new NuGet.Packaging.PackageReference(new PackageIdentity("Ninject", NuGetVersion.Parse("3.2.2.0")), fwk45, true),
                new NuGet.Packaging.PackageReference(new PackageIdentity("Ninject.Web.Common", NuGetVersion.Parse("3.2.3.0")), fwk45, true),
                new NuGet.Packaging.PackageReference(new PackageIdentity("OpenPop.NET", NuGetVersion.Parse("2.0.5.1063")), fwk45, true),
                new NuGet.Packaging.PackageReference(new PackageIdentity("PreMailer.Net", NuGetVersion.Parse("1.1.2")), fwk4, true),
                new NuGet.Packaging.PackageReference(new PackageIdentity("Rejuicer", NuGetVersion.Parse("1.3.0")), fwk45, true),
                new NuGet.Packaging.PackageReference(new PackageIdentity("T4MVCExtensions", NuGetVersion.Parse("3.15.2")), fwk46, true),
                new NuGet.Packaging.PackageReference(new PackageIdentity("T4MvcJs", NuGetVersion.Parse("1.0.13")), fwk45, true),
                new NuGet.Packaging.PackageReference(new PackageIdentity("Twia.ReSharper", NuGetVersion.Parse("9.0.0")), fwk45, true),
                new NuGet.Packaging.PackageReference(new PackageIdentity("valueinjecter", NuGetVersion.Parse("2.3.3")), fwk45, true),
                new NuGet.Packaging.PackageReference(new PackageIdentity("WebActivator", NuGetVersion.Parse("1.5")), fwk4, true),
                new NuGet.Packaging.PackageReference(new PackageIdentity("YUICompressor.NET", NuGetVersion.Parse("1.6.0.2")), fwk45, true),
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

                Assert.True(nugetProjectActions.Select(pa => pa.PackageIdentity.Id).Contains(target, StringComparer.OrdinalIgnoreCase));
            }
        }

        [Fact(Skip = "Test was skipped as part of 475ad399 and is currently broken.")]
        public async Task TestPacManInstallPackageDowngrade()
        {
            // Arrange
            var fwk46 = NuGetFramework.Parse("net46");
            var fwk45 = NuGetFramework.Parse("net45");
            var fwk4 = NuGetFramework.Parse("net4");

            var installedPackages = new List<NuGet.Packaging.PackageReference>
            {
                new NuGet.Packaging.PackageReference(new PackageIdentity("ResolverTestA", NuGetVersion.Parse("3.0.0")), fwk45, true),
                new NuGet.Packaging.PackageReference(new PackageIdentity("ResolverTestB", NuGetVersion.Parse("3.0.0")), fwk45, true),
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

                Assert.True(nugetProjectActions.Select(pa => pa.PackageIdentity.Id).Contains(target, StringComparer.OrdinalIgnoreCase));
            }
        }

        // [Fact]
        public async Task TestPacManUpdatePackagePreservePackagesConfigAttributes()
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
        public async Task TestPacManUpdatePackagePreservePackagesConfigAttributesMultiplePackages()
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

        [Fact]
        public async Task TestPacManGetLatestVersion_GatherCache()
        {
            // Arrange
            var packageIdentity = new PackageIdentity("a", new NuGetVersion(1, 0, 0));
            var bVersionRange = VersionRange.Parse("[0.5.0, 2.0.0)");
            var packages = new List<SourcePackageDependencyInfo>
            {
                new SourcePackageDependencyInfo(
                    packageIdentity.Id,
                    packageIdentity.Version,
                    new[]
                    {
                        new Packaging.Core.PackageDependency("b", bVersionRange)
                    },
                    listed: true,
                    source: null),
            };

            var resourceProviders = new List<Lazy<INuGetResourceProvider>>();
            resourceProviders.Add(new Lazy<INuGetResourceProvider>(() => new TestDependencyInfoProvider(packages)));
            resourceProviders.Add(new Lazy<INuGetResourceProvider>(() => new TestMetadataProvider(packages)));

            var packageSource = new Configuration.PackageSource("http://a");
            var packageSourceProvider = new TestPackageSourceProvider(new[] { packageSource });

            var sourceRepositoryProvider = new SourceRepositoryProvider(packageSourceProvider, resourceProviders);
            var resolutionContext = new ResolutionContext();

            // Act
            var latestVersion = await NuGetPackageManager.GetLatestVersionAsync(
                "a",
                NuGetFramework.AnyFramework,
                resolutionContext,
                sourceRepositoryProvider.GetRepositories().First(),
                Common.NullLogger.Instance,
                CancellationToken.None);

            // Assert
            var gatherCache = resolutionContext.GatherCache;
            var gatherCacheResult = gatherCache.GetPackage(packageSource, packageIdentity, NuGetFramework.AnyFramework);
            Assert.Single(gatherCacheResult.Packages);
            var packageInfo = gatherCacheResult.Packages.Single();
            Assert.Single(packageInfo.Dependencies);
            var packageDependency = packageInfo.Dependencies.Single();
            Assert.Equal("b", packageDependency.Id);
            Assert.Equal(bVersionRange.ToString(), packageDependency.VersionRange.ToString());
        }

        [Fact]
        public async Task TestDirectDownloadByPackagesConfig()
        {
            // Arrange
            using (var testFolderPath = TestDirectory.Create())
            using (var directDownloadDirectory = TestDirectory.Create())
            {
                // Create a nuget.config file with a test global packages folder
                var globalPackageFolderPath = Path.Combine(testFolderPath, "GlobalPackagesFolder");
                File.WriteAllText(
                    Path.Combine(testFolderPath, "nuget.config"),
                    @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
  <config>
    <add key=""globalPackagesFolder"" value=""" + globalPackageFolderPath + @""" />
  </config >
</configuration>");

                // Create a packages.config
                var packagesConfigPath = Path.Combine(testFolderPath, "packages.config");
                using (var writer = new StreamWriter(packagesConfigPath))
                {
                    writer.WriteLine(@"<packages><package id=""Newtonsoft.Json"" version=""6.0.8"" /></packages>");
                }

                var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV3OnlySourceRepositoryProvider();
                var settings = new Settings(testFolderPath);
                var packagesFolderPath = Path.Combine(testFolderPath, "packages");
                var token = CancellationToken.None;
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    settings,
                    packagesFolderPath);
                var packageIdentity = new PackageIdentity("Newtonsoft.Json", NuGetVersion.Parse("6.0.8"));

                // Act
                using (var cacheContext = new SourceCacheContext())
                {
                    var downloadContext = new PackageDownloadContext(
                        cacheContext,
                        directDownloadDirectory,
                        directDownload: true);

                    await nuGetPackageManager.RestorePackageAsync(
                        packageIdentity,
                        new TestNuGetProjectContext(),
                        downloadContext,
                        sourceRepositoryProvider.GetRepositories(),
                        token);
                }

                // Assert
                // Verify that the package was not cached in the Global Packages Folder
                var globalPackage = GlobalPackagesFolderUtility.GetPackage(packageIdentity, globalPackageFolderPath);
                Assert.Null(globalPackage);
            }
        }

        [Fact]
        public async Task TestPacMan_InstallPackage_BatchEvent_Raised()
        {
            using (var packageSource = TestDirectory.Create())
            {
                // Arrange
                var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(
                    new List<PackageSource>()
                    {
                        new PackageSource(packageSource.Path)
                    });

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
                    var projectA = testSolutionManager.AddNewMSBuildProject("testA");

                    // Add package
                    var target = new PackageIdentity("packageA", NuGetVersion.Parse("1.0.0"));
                    AddToPackagesFolder(target, packageSource);

                    // batch handlers
                    var batchStartIds = new List<string>();
                    var batchEndIds = new List<string>();
                    var projectName = string.Empty;

                    // add batch events handler
                    nuGetPackageManager.BatchStart += (o, args) =>
                    {
                        batchStartIds.Add(args.Id);
                        projectName = args.Name;
                    };

                    nuGetPackageManager.BatchEnd += (o, args) =>
                    {
                        batchEndIds.Add(args.Id);
                    };

                    // Act
                    await nuGetPackageManager.InstallPackageAsync(projectA, target,
                        new ResolutionContext(), new TestNuGetProjectContext(),
                        sourceRepositoryProvider.GetRepositories().First(), null, token);

                    // Assert
                    // Check that the packages.config file exists after the installation
                    Assert.True(File.Exists(projectA.PackagesConfigNuGetProject.FullPath));

                    // Check the number of packages and packages returned by PackagesConfigProject after the installation
                    var packagesInPackagesConfig =
                        (await projectA.PackagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                    Assert.Equal(1, packagesInPackagesConfig.Count);

                    // Check batch events data
                    Assert.True(batchStartIds.Count == 1);
                    Assert.True(batchEndIds.Count == 1);
                    Assert.Equal(batchStartIds[0], batchEndIds[0]);
                    Assert.Equal("testA", projectName);
                }
            }
        }

        [Fact]
        public async Task TestPacMan_UpdatePackage_BatchEvent_Raised()
        {
            // Arrange
            using (var packageSource = TestDirectory.Create())
            {
                // Arrange
                var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(
                    new List<PackageSource>()
                    {
                        new PackageSource(packageSource.Path)
                    });

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
                    var projectA = testSolutionManager.AddNewMSBuildProject("testA");

                    // Add package
                    var target = new PackageIdentity("packageA", NuGetVersion.Parse("1.0.0"));
                    AddToPackagesFolder(target, packageSource);

                    // batch handlers
                    var batchStartIds = new List<string>();
                    var batchEndIds = new List<string>();

                    // add batch events handler
                    nuGetPackageManager.BatchStart += (o, args) =>
                    {
                        batchStartIds.Add(args.Id);
                    };

                    nuGetPackageManager.BatchEnd += (o, args) =>
                    {
                        batchEndIds.Add(args.Id);
                    };

                    // Act
                    await nuGetPackageManager.InstallPackageAsync(projectA, target,
                        new ResolutionContext(), new TestNuGetProjectContext(),
                        sourceRepositoryProvider.GetRepositories().First(), null, token);

                    // Assert
                    // Check that the packages.config file exists after the installation
                    Assert.True(File.Exists(projectA.PackagesConfigNuGetProject.FullPath));

                    // Check the number of packages and packages returned by PackagesConfigProject after the installation
                    var packagesInPackagesConfig =
                        (await projectA.PackagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                    Assert.Equal(1, packagesInPackagesConfig.Count);

                    // Check batch events data
                    Assert.True(batchStartIds.Count == 1);
                    Assert.True(batchEndIds.Count == 1);
                    Assert.Equal(batchStartIds[0], batchEndIds[0]);

                    // Update
                    var updatePackage = new PackageIdentity("packageA", NuGetVersion.Parse("2.0.0"));
                    AddToPackagesFolder(updatePackage, packageSource);

                    // Act
                    await nuGetPackageManager.InstallPackageAsync(projectA, updatePackage,
                        new ResolutionContext(), new TestNuGetProjectContext(),
                        sourceRepositoryProvider.GetRepositories().First(), null, token);

                    // Check the number of packages and packages returned by PackagesConfigProject after the installation
                    packagesInPackagesConfig =
                        (await projectA.PackagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                    Assert.Equal(1, packagesInPackagesConfig.Count);

                    // Check batch events data
                    Assert.True(batchStartIds.Count == 2);
                    Assert.True(batchEndIds.Count == 2);
                    Assert.Equal(batchStartIds[1], batchEndIds[1]);
                    Assert.NotEqual(batchStartIds[0], batchStartIds[1]);
                }
            }
        }

        [Fact]
        public async Task TestPacMan_UninstallPackage_BatchEvent_Raised()
        {
            // Arrange
            using (var packageSource = TestDirectory.Create())
            {
                // Arrange
                var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(
                    new List<Configuration.PackageSource>()
                    {
                        new Configuration.PackageSource(packageSource.Path)
                    });

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
                    var projectA = testSolutionManager.AddNewMSBuildProject("testA");

                    // Add package
                    var target = new PackageIdentity("packageA", NuGetVersion.Parse("1.0.0"));
                    AddToPackagesFolder(target, packageSource);

                    // batch handlers
                    var batchStartIds = new List<string>();
                    var batchEndIds = new List<string>();

                    // add batch events handler
                    nuGetPackageManager.BatchStart += (o, args) =>
                    {
                        batchStartIds.Add(args.Id);
                    };

                    nuGetPackageManager.BatchEnd += (o, args) =>
                    {
                        batchEndIds.Add(args.Id);
                    };

                    // Act
                    await nuGetPackageManager.InstallPackageAsync(projectA, target,
                        new ResolutionContext(), new TestNuGetProjectContext(), sourceRepositoryProvider.GetRepositories().First(),
                        null, token);

                    // Main Act
                    var uninstallationContext = new UninstallationContext();
                    await nuGetPackageManager.UninstallPackageAsync(projectA, target.Id,
                        uninstallationContext, new TestNuGetProjectContext(), token);

                    // Assert

                    // Check batch events data
                    Assert.True(batchStartIds.Count == 2);
                    Assert.True(batchEndIds.Count == 2);
                    Assert.Equal(batchStartIds[0], batchEndIds[0]);
                    Assert.Equal(batchStartIds[1], batchEndIds[1]);
                    Assert.NotEqual(batchStartIds[0], batchStartIds[1]);
                }
            }
        }

        [Fact]
        public async Task TestPacMan_ExecuteMultipleNugetActions_BatchEvent_Raised()
        {
            // Arrange
            using (var packageSource = TestDirectory.Create())
            {
                // Arrange
                var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(
                    new List<Configuration.PackageSource>()
                    {
                        new Configuration.PackageSource(packageSource.Path)
                    });

                using (var testSolutionManager = new TestSolutionManager())
                {
                    var actions = new List<NuGetProjectAction>();
                    var testSettings = NullSettings.Instance;
                    var token = CancellationToken.None;
                    var deleteOnRestartManager = new TestDeleteOnRestartManager();
                    var nuGetPackageManager = new NuGetPackageManager(
                        sourceRepositoryProvider,
                        testSettings,
                        testSolutionManager,
                        deleteOnRestartManager);
                    var projectA = testSolutionManager.AddNewMSBuildProject("testA");

                    // Add package
                    var packageA1 = new PackageIdentity("packageA", NuGetVersion.Parse("1.0.0"));
                    var packageA2 = new PackageIdentity("packageA", NuGetVersion.Parse("2.0.0"));
                    var packageB1 = new PackageIdentity("packageB", NuGetVersion.Parse("1.0.0"));
                    AddToPackagesFolder(packageA1, packageSource);
                    AddToPackagesFolder(packageA2, packageSource);
                    AddToPackagesFolder(packageB1, packageSource);

                    // batch handlers
                    var batchStartIds = new List<string>();
                    var batchEndIds = new List<string>();
                    var projectName = string.Empty;

                    await nuGetPackageManager.InstallPackageAsync(projectA, packageA1,
                        new ResolutionContext(), new TestNuGetProjectContext(),
                        sourceRepositoryProvider.GetRepositories().First(), null, token);

                    // add batch events handler
                    nuGetPackageManager.BatchStart += (o, args) =>
                    {
                        batchStartIds.Add(args.Id);
                        projectName = args.Name;
                    };

                    nuGetPackageManager.BatchEnd += (o, args) =>
                    {
                        batchEndIds.Add(args.Id);
                    };

                    // nuget actions
                    actions.Add(NuGetProjectAction.CreateInstallProjectAction(packageA2,
                        sourceRepositoryProvider.GetRepositories().First(), projectA));
                    actions.Add(NuGetProjectAction.CreateUninstallProjectAction(packageB1, projectA));

                    // Main Act
                    await
                        nuGetPackageManager.ExecuteNuGetProjectActionsAsync(projectA, actions,
                            new TestNuGetProjectContext(), NullSourceCacheContext.Instance, token);

                    //Assert
                    // Check batch events data
                    Assert.True(batchStartIds.Count == 1);
                    Assert.True(batchEndIds.Count == 1);
                    Assert.Equal(batchStartIds[0], batchEndIds[0]);
                    Assert.Equal("testA", projectName);
                }
            }
        }

        [Fact]
        public async Task TestPacMan_InstallPackagesInMultipleProjects_BatchEvent_Raised()
        {
            // Arrange
            using (var packageSource = TestDirectory.Create())
            {
                // Arrange
                var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(
                    new List<Configuration.PackageSource>()
                    {
                        new Configuration.PackageSource(packageSource.Path)
                    });

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
                    var projectA = testSolutionManager.AddNewMSBuildProject("testA");
                    var projectB = testSolutionManager.AddNewMSBuildProject("testB");

                    // Add package
                    var target = new PackageIdentity("packageA", NuGetVersion.Parse("1.0.0"));
                    AddToPackagesFolder(target, packageSource);

                    // batch handlers
                    var batchStartIds = new List<string>();
                    var batchEndIds = new List<string>();
                    var projectNames = new List<string>();

                    // add batch events handler
                    nuGetPackageManager.BatchStart += (o, args) =>
                    {
                        batchStartIds.Add(args.Id);
                        projectNames.Add(args.Name);
                    };

                    nuGetPackageManager.BatchEnd += (o, args) =>
                    {
                        batchEndIds.Add(args.Id);
                    };

                    // Act
                    await nuGetPackageManager.InstallPackageAsync(projectA, target,
                        new ResolutionContext(), new TestNuGetProjectContext(),
                        sourceRepositoryProvider.GetRepositories().First(), null, token);
                    await nuGetPackageManager.InstallPackageAsync(projectB, target,
                        new ResolutionContext(), new TestNuGetProjectContext(),
                        sourceRepositoryProvider.GetRepositories().First(), null, token);

                    // Assert Project1
                    // Check that the packages.config file exists after the installation
                    Assert.True(File.Exists(projectA.PackagesConfigNuGetProject.FullPath));
                    Assert.True(File.Exists(projectB.PackagesConfigNuGetProject.FullPath));

                    // Check the number of packages and packages returned by PackagesConfigProject after the installation
                    var packagesInPackagesConfig =
                        (await projectA.PackagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                    Assert.Equal(1, packagesInPackagesConfig.Count);
                    var packagesInPackagesConfigB =
                        (await projectB.PackagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                    Assert.Equal(1, packagesInPackagesConfigB.Count);

                    // Check batch events data
                    Assert.True(batchStartIds.Count == 2);
                    Assert.True(batchEndIds.Count == 2);
                    Assert.Equal(batchStartIds[0], batchEndIds[0]);
                    Assert.Equal(batchStartIds[1], batchEndIds[1]);
                    Assert.NotEqual(batchStartIds[0], batchStartIds[1]);
                    Assert.True(projectNames.Count == 2);
                    Assert.Equal("testA", projectNames[0]);
                    Assert.Equal("testB", projectNames[1]);
                }
            }
        }

        [Fact]
        public async Task TestPacMan_ExecuteNugetActions_NoOP_BatchEvent()
        {
            // Arrange
            using (var packageSource = TestDirectory.Create())
            {
                // Arrange
                var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(
                    new List<Configuration.PackageSource>()
                    {
                        new Configuration.PackageSource(packageSource.Path)
                    });

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
                    var projectA = testSolutionManager.AddNewMSBuildProject("testA");

                    // batch handlers
                    var batchStartIds = new List<string>();
                    var batchEndIds = new List<string>();

                    // add batch events handler
                    nuGetPackageManager.BatchStart += (o, args) =>
                    {
                        batchStartIds.Add(args.Id);
                    };

                    nuGetPackageManager.BatchEnd += (o, args) =>
                    {
                        batchEndIds.Add(args.Id);
                    };

                    // Main Act
                    await
                        nuGetPackageManager.ExecuteNuGetProjectActionsAsync(projectA, new List<NuGetProjectAction>(),
                            new TestNuGetProjectContext(), NullSourceCacheContext.Instance, token);

                    // Check that the packages.config file exists after the installation
                    Assert.False(File.Exists(projectA.PackagesConfigNuGetProject.FullPath));
                    // Check that there are no packages returned by PackagesConfigProject
                    var packagesInPackagesConfig =
                        (await projectA.PackagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                    Assert.Equal(0, packagesInPackagesConfig.Count);

                    // Check batch events data
                    Assert.True(batchStartIds.Count == 1);
                    Assert.True(batchEndIds.Count == 1);
                    Assert.Equal(batchStartIds[0], batchEndIds[0]);
                }
            }
        }

        [Fact]
        public async Task TestPacMan_InstallPackage_Fail_BatchEvent_Raised()
        {
            // Arrange
            using (var packageSource = TestDirectory.Create())
            {
                // Arrange
                var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(
                    new List<Configuration.PackageSource>()
                    {
                        new Configuration.PackageSource(packageSource.Path)
                    });

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
                    var projectA = testSolutionManager.AddNewMSBuildProject("testA",
                        NuGetFramework.Parse("netcoreapp10"));

                    // Add package
                    var target = new PackageIdentity("packageA", NuGetVersion.Parse("1.0.0"));
                    AddToPackagesFolder(target, packageSource);

                    // batch handlers
                    var batchStartIds = new List<string>();
                    var batchEndIds = new List<string>();

                    // add batch events handler
                    nuGetPackageManager.BatchStart += (o, args) =>
                    {
                        batchStartIds.Add(args.Id);
                    };

                    nuGetPackageManager.BatchEnd += (o, args) =>
                    {
                        batchEndIds.Add(args.Id);
                    };

                    Exception exception = null;
                    try
                    {
                        // Act
                        await nuGetPackageManager.InstallPackageAsync(projectA, target,
                            new ResolutionContext(), new TestNuGetProjectContext(),
                            sourceRepositoryProvider.GetRepositories().First(), null, token);
                    }
                    catch (Exception ex)
                    {
                        exception = ex;
                    }

                    // Assert
                    Assert.NotNull(exception);

                    // Check batch events data
                    Assert.True(batchStartIds.Count == 1);
                    Assert.True(batchEndIds.Count == 1);
                    Assert.Equal(batchStartIds[0], batchEndIds[0]);
                }
            }
        }

        [Fact]
        public async Task TestPacMan_DownloadPackageTask_Fail_BatchEvent_NotRaised()
        {
            // Arrange
            using (var packageSource = TestDirectory.Create())
            {
                // Arrange
                var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(
                    new List<Configuration.PackageSource>()
                    {
                        new Configuration.PackageSource(packageSource.Path)
                    });

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
                    var projectA = testSolutionManager.AddNewMSBuildProject("testA");

                    // Add package
                    var target = new PackageIdentity("packageA", NuGetVersion.Parse("1.0.0"));
                    AddToPackagesFolder(target, packageSource);

                    var projectActions = new List<NuGetProjectAction>();
                    projectActions.Add(
                        NuGetProjectAction.CreateInstallProjectAction(target, null, projectA));

                    // batch handlers
                    var batchStartIds = new List<string>();
                    var batchEndIds = new List<string>();

                    // add batch events handler
                    nuGetPackageManager.BatchStart += (o, args) =>
                    {
                        batchStartIds.Add(args.Id);
                    };

                    nuGetPackageManager.BatchEnd += (o, args) =>
                    {
                        batchEndIds.Add(args.Id);
                    };

                    Exception exception = null;
                    try
                    {
                        // Act
                        await nuGetPackageManager.ExecuteNuGetProjectActionsAsync(projectA, projectActions,
                            new TestNuGetProjectContext(), NullSourceCacheContext.Instance, token);
                    }
                    catch (Exception ex)
                    {
                        exception = ex;
                    }

                    // Assert
                    Assert.NotNull(exception);

                    // Check batch events data
                    Assert.True(batchStartIds.Count == 0);
                    Assert.True(batchEndIds.Count == 0);

                }
            }
        }

        [Fact]
        public async Task TestPacMan_DownloadPackageResult_Fail_BatchEvent_Raised()
        {
            // Arrange
            using (var packageSource = TestDirectory.Create())
            {
                // Arrange
                var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(
                    new List<Configuration.PackageSource>()
                    {
                        new Configuration.PackageSource(packageSource.Path)
                    });

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
                    var projectA = testSolutionManager.AddNewMSBuildProject("testA");

                    var projectActions = new List<NuGetProjectAction>();
                    projectActions.Add(
                        NuGetProjectAction.CreateInstallProjectAction(
                            new PackageIdentity("inValidPackageA", new NuGetVersion("1.0.0")),
                            sourceRepositoryProvider.GetRepositories().First(),
                            projectA));

                    // batch handlers
                    var batchStartIds = new List<string>();
                    var batchEndIds = new List<string>();

                    // add batch events handler
                    nuGetPackageManager.BatchStart += (o, args) =>
                    {
                        batchStartIds.Add(args.Id);
                    };

                    nuGetPackageManager.BatchEnd += (o, args) =>
                    {
                        batchEndIds.Add(args.Id);
                    };

                    Exception exception = null;
                    try
                    {
                        // Act
                        await nuGetPackageManager.ExecuteNuGetProjectActionsAsync(projectA, projectActions,
                            new TestNuGetProjectContext(), NullSourceCacheContext.Instance, token);
                    }
                    catch (Exception ex)
                    {
                        exception = ex;
                    }

                    // Assert
                    Assert.NotNull(exception);

                    // Check batch events data
                    Assert.True(batchStartIds.Count == 1);
                    Assert.True(batchEndIds.Count == 1);
                    Assert.Equal(batchStartIds[0], batchEndIds[0]);
                }
            }
        }

        [Fact]
        public async Task TestPacMan_InstallPackage_BuildIntegratedProject_BatchEvent_NotRaised()
        {
            // Arrange
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV3OnlySourceRepositoryProvider();
            using (var settingsdir = TestDirectory.Create())
            using (var testSolutionManager = new TestSolutionManager())
            {
                var Settings = new Settings(settingsdir);
                foreach (var source in sourceRepositoryProvider.GetRepositories())
                {
                    Settings.AddOrUpdate(ConfigurationConstants.PackageSources, source.PackageSource.AsSourceItem());
                }

                var token = CancellationToken.None;
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    Settings,
                    testSolutionManager,
                    deleteOnRestartManager);

                var installationCompatibility = new Mock<IInstallationCompatibility>();
                nuGetPackageManager.InstallationCompatibility = installationCompatibility.Object;

                var buildIntegratedProject = testSolutionManager.AddBuildIntegratedProject();
                var packageIdentity = _packageWithDependents[0];

                // batch handlers
                var batchStartIds = new List<string>();
                var batchEndIds = new List<string>();

                // add batch events handler
                nuGetPackageManager.BatchStart += (o, args) =>
                {
                    batchStartIds.Add(args.Id);
                };

                nuGetPackageManager.BatchEnd += (o, args) =>
                {
                    batchEndIds.Add(args.Id);
                };

                // Act
                await nuGetPackageManager.InstallPackageAsync(buildIntegratedProject, packageIdentity,
                    new ResolutionContext(), new TestNuGetProjectContext(),
                    sourceRepositoryProvider.GetRepositories().First(), null, token);

                // Assert
                // Check batch events data
                Assert.True(batchStartIds.Count == 0);
                Assert.True(batchEndIds.Count == 0);
            }
        }

        [Fact]
        public async Task TestPacMan_PreviewUpdatePackage_DeepDependencies()
        {
            // Arrange

            // Set up Package Dependencies
            var dependencies = new List<PackageDependency>();
            for (var j = 1; j < 3; j++)
            {
                for (var i = 2; i <= 30; i++)
                {
                    dependencies.Add(new PackageDependency($"Package{i}", new VersionRange(new NuGetVersion(j, 0, 0))));
                }
            }

            // Set up Package Source
            var packages = new List<SourcePackageDependencyInfo>();
            var next = 1;
            for (var i = 1; i < 3; i++)
            {
                for (var j = 1; j < 30; j++)
                {
                    next = j + 1;
                    packages.Add(new SourcePackageDependencyInfo($"Package{j}", new NuGetVersion(i, 0, 0),
                        dependencies.Where(
                            dep =>
                                dep.Id.CompareTo($"Package{j}") > 0 &&
                                dep.VersionRange.MinVersion.Equals(new NuGetVersion(i, 0, 0))),
                        true,
                        null));
                }

                packages.Add(new SourcePackageDependencyInfo($"Package{next}", new NuGetVersion(i, 0, 0),
                    new PackageDependency[] { },
                    true,
                    null));
            }

            var sourceRepositoryProvider = CreateSource(packages);

            // Set up NuGetProject
            var fwk45 = NuGetFramework.Parse("net45");

            var installedPackages = new List<PackageReference>();
            for (var i = 1; i <= 30; i++)
            {
                installedPackages.Add(new PackageReference(
                    new PackageIdentity($"Package{i}", new NuGetVersion(1, 0, 0)), fwk45, true));
            }

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
                var targets = new List<PackageIdentity>
                {
                    new PackageIdentity("Package1", new NuGetVersion(2, 0, 0)),
                    new PackageIdentity("Package2", new NuGetVersion(2, 0, 0)),
                    new PackageIdentity("Package3", new NuGetVersion(2, 0, 0)),
                    new PackageIdentity("Package4", new NuGetVersion(2, 0, 0)),
                    new PackageIdentity("Package5", new NuGetVersion(2, 0, 0)),
                    new PackageIdentity("Package6", new NuGetVersion(2, 0, 0)),
                    new PackageIdentity("Package7", new NuGetVersion(2, 0, 0)),
                    new PackageIdentity("Package8", new NuGetVersion(2, 0, 0)),
                };

                var result = await nuGetPackageManager.PreviewUpdatePackagesAsync(
                    targets,
                    new[] { nuGetProject },
                    new ResolutionContext(),
                    new TestNuGetProjectContext(),
                    sourceRepositoryProvider.GetRepositories(),
                    sourceRepositoryProvider.GetRepositories(),
                    CancellationToken.None);

                // Assert
                var resulting = result.Select(a => Tuple.Create(a.PackageIdentity, a.NuGetProjectActionType)).ToArray();

                var expected = new List<Tuple<PackageIdentity, NuGetProjectActionType>>();
                for (var i = 1; i <= 30; i++)
                {
                    Expected(expected, $"Package{i}", new NuGetVersion(1, 0, 0), new NuGetVersion(2, 0, 0));
                }

                Assert.True(Compare(resulting, expected));
            }
        }

        public async Task TestPacMan_ExecuteNuGetProjectActionsAsync_MultipleBuildIntegratedProjects()
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

                var installationCompatibility = new Mock<IInstallationCompatibility>();
                nuGetPackageManager.InstallationCompatibility = installationCompatibility.Object;

                var buildIntegratedProjectA = testSolutionManager.AddBuildIntegratedProject("projectA") as BuildIntegratedNuGetProject;
                var buildIntegratedProjectB = testSolutionManager.AddBuildIntegratedProject("projectB") as BuildIntegratedNuGetProject;
                var buildIntegratedProjectC = testSolutionManager.AddBuildIntegratedProject("projectC") as BuildIntegratedNuGetProject;

                var packageIdentity = _packageWithDependents[0];

                var projectActions = new List<NuGetProjectAction>();
                projectActions.Add(
                    NuGetProjectAction.CreateInstallProjectAction(
                        packageIdentity,
                        sourceRepositoryProvider.GetRepositories().First(),
                        buildIntegratedProjectA));
                projectActions.Add(
                    NuGetProjectAction.CreateInstallProjectAction(
                        packageIdentity,
                        sourceRepositoryProvider.GetRepositories().First(),
                        buildIntegratedProjectB));

                // Act
                await nuGetPackageManager.ExecuteNuGetProjectActionsAsync(
                    new List<NuGetProject>() { buildIntegratedProjectA, buildIntegratedProjectB },
                    projectActions,
                    new TestNuGetProjectContext(),
                    NullSourceCacheContext.Instance,
                    token);

                // Assert
                var projectAPackages = (await buildIntegratedProjectA.GetInstalledPackagesAsync(token)).ToList();
                var projectBPackages = (await buildIntegratedProjectB.GetInstalledPackagesAsync(token)).ToList();
                var projectCPackages = (await buildIntegratedProjectC.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(2, projectAPackages.Count);
                Assert.Equal(2, projectBPackages.Count);
                Assert.Equal(1, projectCPackages.Count);
            }
        }

        [Fact]
        public async Task TestPacMan_ExecuteNuGetProjectActionsAsync_MixedProjects()
        {
            // Arrange
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV3OnlySourceRepositoryProvider();
            using (var settingsdir = TestDirectory.Create())
            using (var testSolutionManager = new TestSolutionManager())
            {
                var Settings = new Settings(settingsdir);
                foreach (var source in sourceRepositoryProvider.GetRepositories())
                {
                    Settings.AddOrUpdate(ConfigurationConstants.PackageSources, source.PackageSource.AsSourceItem());
                }

                var token = CancellationToken.None;
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    Settings,
                    testSolutionManager,
                    deleteOnRestartManager);

                var installationCompatibility = new Mock<IInstallationCompatibility>();
                nuGetPackageManager.InstallationCompatibility = installationCompatibility.Object;

                var projectA = testSolutionManager.AddBuildIntegratedProject("projectA") as BuildIntegratedNuGetProject;
                var projectB = testSolutionManager.AddNewMSBuildProject("projectB");
                var projectC = testSolutionManager.AddBuildIntegratedProject("projectC") as BuildIntegratedNuGetProject;

                var packageIdentity = _packageWithDependents[0];

                var projectActions = new List<NuGetProjectAction>();
                projectActions.Add(
                    NuGetProjectAction.CreateInstallProjectAction(
                        packageIdentity,
                        sourceRepositoryProvider.GetRepositories().First(),
                        projectA));
                projectActions.Add(
                    NuGetProjectAction.CreateInstallProjectAction(
                        packageIdentity,
                        sourceRepositoryProvider.GetRepositories().First(),
                        projectB));
                projectActions.Add(
                    NuGetProjectAction.CreateInstallProjectAction(
                        packageIdentity,
                        sourceRepositoryProvider.GetRepositories().First(),
                        projectC));

                // Act
                await nuGetPackageManager.ExecuteNuGetProjectActionsAsync(
                    new List<NuGetProject>() { projectA, projectB, projectC },
                    projectActions,
                    new TestNuGetProjectContext(),
                    NullSourceCacheContext.Instance,
                    token);

                // Assert
                var projectAPackages = (await projectA.GetInstalledPackagesAsync(token)).ToList();
                var projectBPackages = (await projectB.GetInstalledPackagesAsync(token)).ToList();
                var projectCPackages = (await projectC.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(2, projectAPackages.Count);
                Assert.Equal(1, projectBPackages.Count);
                Assert.Equal(2, projectCPackages.Count);
            }
        }

        [Fact]
        public async Task TestPacMan_PreviewUpdatePackage_IgnoreDependency()
        {
            // Arrange

            // Set up Package Source
            var packages = new List<SourcePackageDependencyInfo>
            {
                new SourcePackageDependencyInfo("a", new NuGetVersion(1, 0, 0), new[] { new Packaging.Core.PackageDependency("b", new VersionRange(new NuGetVersion(1, 0, 0))) }, true, null),
                new SourcePackageDependencyInfo("a", new NuGetVersion(2, 0, 0), new[] { new Packaging.Core.PackageDependency("b", new VersionRange(new NuGetVersion(2, 0, 0))),  new Packaging.Core.PackageDependency("c", new VersionRange(new NuGetVersion(1, 0, 0)))}, true, null),
                new SourcePackageDependencyInfo("b", new NuGetVersion(1, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null)
            };

            var sourceRepositoryProvider = CreateSource(packages);

            // Set up NuGetProject
            var fwk45 = NuGetFramework.Parse("net45");

            var installedPackage1 = new PackageIdentity("a", new NuGetVersion(1, 0, 0));
            var installedPackage2 = new PackageIdentity("b", new NuGetVersion(1, 0, 0));

            var installedPackages = new List<NuGet.Packaging.PackageReference>
            {
                new NuGet.Packaging.PackageReference(installedPackage1, fwk45, true),
                new NuGet.Packaging.PackageReference(installedPackage2, fwk45, true)
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
                var targetPackage = new PackageIdentity("a", new NuGetVersion(2, 0, 0));

                var result = (await nuGetPackageManager.PreviewUpdatePackagesAsync(
                    new List<PackageIdentity> { targetPackage },
                    new List<NuGetProject> { nuGetProject },
                    new ResolutionContext(DependencyBehavior.Ignore, false, true, VersionConstraints.None),
                    new TestNuGetProjectContext(),
                    sourceRepositoryProvider.GetRepositories(),
                    sourceRepositoryProvider.GetRepositories(),
                    CancellationToken.None)).ToList();

                // Assert
                Assert.Equal(2, result.Count);
                Assert.True(installedPackage1.Equals(result[0].PackageIdentity));
                Assert.Equal(NuGetProjectActionType.Uninstall, result[0].NuGetProjectActionType);
                Assert.True(targetPackage.Equals(result[1].PackageIdentity));
                Assert.Equal(NuGetProjectActionType.Install, result[1].NuGetProjectActionType);
            }
        }

#if IS_DESKTOP
        [Fact]
        public async Task TestPacMan_PreviewInstallPackage_PackagesConfig_RaiseTelemetryEvents()
        {
            // Arrange

            // Set up Package Source
            var packages = new List<SourcePackageDependencyInfo>
            {
                new SourcePackageDependencyInfo("a", new NuGetVersion(1, 0, 0), new[] { new Packaging.Core.PackageDependency("b", new VersionRange(new NuGetVersion(1, 0, 0))) }, true, null),
                new SourcePackageDependencyInfo("b", new NuGetVersion(1, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null)
            };

            var sourceRepositoryProvider = CreateSource(packages);

            // set up telemetry service
            var telemetrySession = new Mock<ITelemetrySession>();

            var telemetryEvents = new ConcurrentQueue<TelemetryEvent>();
            telemetrySession
                .Setup(x => x.PostEvent(It.IsAny<TelemetryEvent>()))
                .Callback<TelemetryEvent>(x => telemetryEvents.Enqueue(x));

            var nugetProjectContext = new TestNuGetProjectContext();
            var telemetryService = new NuGetVSTelemetryService(telemetrySession.Object);
            TelemetryActivity.NuGetTelemetryService = telemetryService;

            // Create Package Manager
            using (var solutionManager = new TestSolutionManager())
            {
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    NullSettings.Instance,
                    solutionManager,
                    new TestDeleteOnRestartManager());

                var nugetProject = solutionManager.AddNewMSBuildProject();

                // Main Act
                var target = new PackageIdentity("a", new NuGetVersion(1, 0, 0));

                await nuGetPackageManager.PreviewInstallPackageAsync(
                    nugetProject,
                    target,
                    new ResolutionContext(),
                    nugetProjectContext,
                    sourceRepositoryProvider.GetRepositories(),
                    sourceRepositoryProvider.GetRepositories(),
                    CancellationToken.None);

                // Assert
                Assert.Equal(3, telemetryEvents.Count);
                VerifyPreviewActionsTelemetryEvents_PackagesConfig(telemetryEvents.Select(p => (string)p["SubStepName"]));
            }
        }

        [Fact]
        public async Task TestPacMan_PreviewInstallPackage_BuildIntegrated_RaiseTelemetryEvents()
        {
            // Arrange
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV3OnlySourceRepositoryProvider();

            // set up telemetry service
            var telemetrySession = new Mock<ITelemetrySession>();

            var telemetryEvents = new ConcurrentQueue<TelemetryEvent>();
            telemetrySession
                .Setup(x => x.PostEvent(It.IsAny<TelemetryEvent>()))
                .Callback<TelemetryEvent>(x => telemetryEvents.Enqueue(x));

            var nugetProjectContext = new TestNuGetProjectContext();
            var telemetryService = new NuGetVSTelemetryService(telemetrySession.Object);

            TelemetryActivity.NuGetTelemetryService = telemetryService;

            // Create Package Manager
            using (var solutionManager = new TestSolutionManager())
            {
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    NullSettings.Instance,
                    solutionManager,
                    new TestDeleteOnRestartManager());

                var buildIntegratedProject = solutionManager.AddBuildIntegratedProject();

                // Main Act
                var target = _packageWithDependents[0];

                await nuGetPackageManager.PreviewInstallPackageAsync(
                    buildIntegratedProject,
                    target,
                    new ResolutionContext(),
                    nugetProjectContext,
                    sourceRepositoryProvider.GetRepositories(),
                    sourceRepositoryProvider.GetRepositories(),
                    CancellationToken.None);

                // Assert
                Assert.Equal(3, telemetryEvents.Count);
                Assert.Equal(2, telemetryEvents.Where(p => p.Name == "ProjectRestoreInformation").Count());
                Assert.Equal(1, telemetryEvents.Where(p => p.Name == "NugetActionSteps").Count());

                Assert.True(telemetryEvents.Where(p => p.Name == "NugetActionSteps").
                    Any(p => (string)p["SubStepName"] == TelemetryConstants.PreviewBuildIntegratedStepName));

                var projectFilePaths = telemetryEvents.Where(p => p.Name == "ProjectRestoreInformation").SelectMany(x => x.GetPiiData()).Where(x => x.Key == "ProjectFilePath");
                Assert.Equal(2, projectFilePaths.Count());
                Assert.True(projectFilePaths.All(p => p.Value is string y && File.Exists(y) && (y.EndsWith(".csproj") || y.EndsWith("project.json") || y.EndsWith("proj"))));
            }
        }

        [Fact]
        public async Task TestPacMan_PreviewInstallPackage_BuildIntegrated_RaiseTelemetryEventsWithErrorCode()
        {

            // Arrange
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV3OnlySourceRepositoryProvider();

            // set up telemetry service
            var telemetrySession = new Mock<ITelemetrySession>();

            var telemetryEvents = new ConcurrentQueue<TelemetryEvent>();
            telemetrySession
                .Setup(x => x.PostEvent(It.IsAny<TelemetryEvent>()))
                .Callback<TelemetryEvent>(x => telemetryEvents.Enqueue(x));

            var nugetProjectContext = new TestNuGetProjectContext();
            var telemetryService = new NuGetVSTelemetryService(telemetrySession.Object);
            TelemetryActivity.NuGetTelemetryService = telemetryService;

            // Create Package Manager
            using (var solutionManager = new TestSolutionManager())
            {
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    Settings.LoadSpecificSettings(solutionManager.SolutionDirectory, "NuGet.Config"),
                    solutionManager,
                    new TestDeleteOnRestartManager());

                var json = new JObject
                {
                    ["dependencies"] = new JObject(),
                    ["frameworks"] = new JObject
                    {
                        ["net46"] = new JObject()
                    }
                };

                var buildIntegratedProject = solutionManager.AddBuildIntegratedProject(json: json);

                // Act
                var target = new PackageIdentity("NuGet.Versioning", new NuGetVersion("99.9.9"));

                await nuGetPackageManager.PreviewInstallPackageAsync(
                    buildIntegratedProject,
                    target,
                    new ResolutionContext(),
                    nugetProjectContext,
                    sourceRepositoryProvider.GetRepositories(),
                    sourceRepositoryProvider.GetRepositories(),
                    CancellationToken.None);

                // Assert
                Assert.Equal(3, telemetryEvents.Count);
                Assert.Equal(2, telemetryEvents.Where(p => p.Name == "ProjectRestoreInformation").Count());
                Assert.Equal(1, telemetryEvents.Where(p => p.Name == "NugetActionSteps").Count());

                Assert.True(telemetryEvents.Where(p => p.Name == "NugetActionSteps").
                    Any(p => (string)p["SubStepName"] == TelemetryConstants.PreviewBuildIntegratedStepName));

                Assert.True((string)telemetryEvents
                    .Where(p => p.Name == "ProjectRestoreInformation").
                    Last()["ErrorCodes"] == NuGetLogCode.NU1102.ToString());

                var projectFilePaths = telemetryEvents.Where(p => p.Name == "ProjectRestoreInformation").SelectMany(x => x.GetPiiData()).Where(x => x.Key == "ProjectFilePath");
                Assert.Equal(2, projectFilePaths.Count());
                Assert.True(projectFilePaths.All(p => p.Value is string y && File.Exists(y) && (y.EndsWith(".csproj") || y.EndsWith("project.json") || y.EndsWith("proj"))));
            }
        }

        [Fact]
        public async Task TestPacMan_PreviewInstallPackage_BuildIntegrated_RaiseTelemetryEventsWithDupedErrorCodes()
        {

            // Arrange
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV3OnlySourceRepositoryProvider();

            // set up telemetry service
            var telemetrySession = new Mock<ITelemetrySession>();

            var telemetryEvents = new ConcurrentQueue<TelemetryEvent>();
            telemetrySession
                .Setup(x => x.PostEvent(It.IsAny<TelemetryEvent>()))
                .Callback<TelemetryEvent>(x => telemetryEvents.Enqueue(x));

            var nugetProjectContext = new TestNuGetProjectContext();
            var telemetryService = new NuGetVSTelemetryService(telemetrySession.Object);
            TelemetryActivity.NuGetTelemetryService = telemetryService;

            // Create Package Manager
            using (var solutionManager = new TestSolutionManager())
            {
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    Settings.LoadSpecificSettings(solutionManager.SolutionDirectory, "NuGet.Config"),
                    solutionManager,
                    new TestDeleteOnRestartManager());

                var json = new JObject
                {
                    ["dependencies"] = new JObject()
                    {
                        new JProperty("NuGet.Frameworks", "99.0.0")
                    },
                    ["frameworks"] = new JObject
                    {
                        ["net46"] = new JObject()
                    }
                };

                var buildIntegratedProject = solutionManager.AddBuildIntegratedProject(json: json);

                // Act
                var target = new PackageIdentity("NuGet.Versioning", new NuGetVersion("99.9.9"));

                await nuGetPackageManager.PreviewInstallPackageAsync(
                    buildIntegratedProject,
                    target,
                    new ResolutionContext(),
                    nugetProjectContext,
                    sourceRepositoryProvider.GetRepositories(),
                    sourceRepositoryProvider.GetRepositories(),
                    CancellationToken.None);

                // Assert
                Assert.Equal(3, telemetryEvents.Count);
                Assert.Equal(2, telemetryEvents.Where(p => p.Name == "ProjectRestoreInformation").Count());
                Assert.Equal(1, telemetryEvents.Where(p => p.Name == "NugetActionSteps").Count());

                Assert.True(telemetryEvents.Where(p => p.Name == "NugetActionSteps").
                    Any(p => (string)p["SubStepName"] == TelemetryConstants.PreviewBuildIntegratedStepName));

                Assert.True((string)telemetryEvents
                    .Where(p => p.Name == "ProjectRestoreInformation").
                    Last()["ErrorCodes"] == NuGetLogCode.NU1102.ToString());

                var projectFilePaths = telemetryEvents.Where(p => p.Name == "ProjectRestoreInformation").SelectMany(x => x.GetPiiData()).Where(x => x.Key == "ProjectFilePath");
                Assert.Equal(2, projectFilePaths.Count());
                Assert.True(projectFilePaths.All(p => p.Value is string y && File.Exists(y) && (y.EndsWith(".csproj") || y.EndsWith("project.json") || y.EndsWith("proj"))));
            }
        }


        [Fact(Skip = "https://github.com/NuGet/Home/issues/10093")]
        public async Task TestPacMan_PreviewInstallPackage_BuildIntegrated_RaiseTelemetryEventsWithWarningCode()
        {
            // Arrange
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV3OnlySourceRepositoryProvider();

            // set up telemetry service
            var telemetrySession = new Mock<ITelemetrySession>();

            var telemetryEvents = new ConcurrentQueue<TelemetryEvent>();
            telemetrySession
                .Setup(x => x.PostEvent(It.IsAny<TelemetryEvent>()))
                .Callback<TelemetryEvent>(x => telemetryEvents.Enqueue(x));

            var nugetProjectContext = new TestNuGetProjectContext();
            var telemetryService = new TestNuGetVSTelemetryService(telemetrySession.Object, _logger);
            TelemetryActivity.NuGetTelemetryService = telemetryService;

            // Create Package Manager
            using (var solutionManager = new TestSolutionManager())
            {
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    Settings.LoadSpecificSettings(solutionManager.SolutionDirectory, "NuGet.Config"),
                    solutionManager,
                    new TestDeleteOnRestartManager());

                var json = new JObject
                {
                    ["dependencies"] = new JObject(),
                    ["frameworks"] = new JObject
                    {
                        ["net46"] = new JObject()
                    }
                };

                var buildIntegratedProject = solutionManager.AddBuildIntegratedProject(json: json);

                // Act
                var target = new PackageIdentity("NuGet.Versioning", new NuGetVersion("4.6.9"));

                lock (_logger)
                {
                    // telemetry count has been flaky, these xunit logs should help track the extra source of events on CI
                    // for issue https://github.com/NuGet/Home/issues/7105
                    _logger.LogInformation("Begin PreviewInstallPackageAsync");
                }

                await nuGetPackageManager.PreviewInstallPackageAsync(
                    buildIntegratedProject,
                    target,
                    new ResolutionContext(),
                    nugetProjectContext,
                    sourceRepositoryProvider.GetRepositories(),
                    sourceRepositoryProvider.GetRepositories(),
                    CancellationToken.None);

                lock (_logger)
                {
                    _logger.LogInformation("End PreviewInstallPackageAsync");
                }

                // Assert
                Assert.Equal(19, telemetryEvents.Count);
                Assert.Equal(2, telemetryEvents.Where(p => p.Name == "ProjectRestoreInformation").Count());
                Assert.Equal(2, telemetryEvents.Where(p => p.Name == "GenerateRestoreGraph").Count());
                Assert.Equal(2, telemetryEvents.Where(p => p.Name == "GenerateAssetsFile").Count());
                Assert.Equal(2, telemetryEvents.Where(p => p.Name == "ValidateRestoreGraphs").Count());
                Assert.Equal(2, telemetryEvents.Where(p => p.Name == "CreateRestoreResult").Count());
                Assert.Equal(2, telemetryEvents.Where(p => p.Name == "RestoreNoOpInformation").Count());
                Assert.Equal(2, telemetryEvents.Where(p => p.Name == "CreateRestoreTargetGraph").Count());
                Assert.Equal(1, telemetryEvents.Where(p => p.Name == "NugetActionSteps").Count());

                Assert.True(telemetryEvents.Where(p => p.Name == "NugetActionSteps").
                    Any(p => (string)p["SubStepName"] == TelemetryConstants.PreviewBuildIntegratedStepName));

                Assert.True((string)telemetryEvents
                    .Where(p => p.Name == "ProjectRestoreInformation").
                    Last()["WarningCodes"] == NuGetLogCode.NU1603.ToString());

                var projectFilePaths = telemetryEvents.Where(p => p.Name == "ProjectRestoreInformation").SelectMany(x => x.GetPiiData()).Where(x => x.Key == "ProjectFilePath");
                Assert.Equal(2, projectFilePaths.Count());
                Assert.True(projectFilePaths.All(p => p.Value is string y && File.Exists(y) && (y.EndsWith(".csproj") || y.EndsWith("project.json") || y.EndsWith("proj"))));

            }
        }

        [Fact]
        public async Task TestPacMan_PreviewInstallPackage_BuildIntegrated_RaiseTelemetryEventsWithDupedWarningCodes()
        {

            // Arrange
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV3OnlySourceRepositoryProvider();

            // set up telemetry service
            var telemetrySession = new Mock<ITelemetrySession>();

            var telemetryEvents = new ConcurrentQueue<TelemetryEvent>();
            telemetrySession
                .Setup(x => x.PostEvent(It.IsAny<TelemetryEvent>()))
                .Callback<TelemetryEvent>(x => telemetryEvents.Enqueue(x));

            var nugetProjectContext = new TestNuGetProjectContext();
            var telemetryService = new TestNuGetVSTelemetryService(telemetrySession.Object, _logger);
            TelemetryActivity.NuGetTelemetryService = telemetryService;

            // Create Package Manager
            using (var solutionManager = new TestSolutionManager())
            {
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    Settings.LoadSpecificSettings(solutionManager.SolutionDirectory, "NuGet.Config"),
                    solutionManager,
                    new TestDeleteOnRestartManager());

                var json = new JObject
                {
                    ["dependencies"] = new JObject()
                    {
                        new JProperty("NuGet.Frameworks", "4.6.9")
                    },
                    ["frameworks"] = new JObject
                    {
                        ["net46"] = new JObject()
                    }
                };

                var buildIntegratedProject = solutionManager.AddBuildIntegratedProject(json: json);

                // Act
                var target = new PackageIdentity("NuGet.Versioning", new NuGetVersion("4.6.9"));

                await nuGetPackageManager.PreviewInstallPackageAsync(
                    buildIntegratedProject,
                    target,
                    new ResolutionContext(),
                    nugetProjectContext,
                    sourceRepositoryProvider.GetRepositories(),
                    sourceRepositoryProvider.GetRepositories(),
                    CancellationToken.None);

                // Assert
                Assert.Equal(7, telemetryEvents.Count);
                Assert.Equal(2, telemetryEvents.Where(p => p.Name == "ProjectRestoreInformation").Count());
                Assert.Equal(1, telemetryEvents.Where(p => p.Name == "NugetActionSteps").Count());

                Assert.True(telemetryEvents.Where(p => p.Name == "NugetActionSteps").
                    Any(p => (string)p["SubStepName"] == TelemetryConstants.PreviewBuildIntegratedStepName));

                Assert.True((string)telemetryEvents
                    .Where(p => p.Name == "ProjectRestoreInformation").
                    Last()["WarningCodes"] == NuGetLogCode.NU1603.ToString());

                var projectFilePaths = telemetryEvents.Where(p => p.Name == "ProjectRestoreInformation").SelectMany(x => x.GetPiiData()).Where(x => x.Key == "ProjectFilePath");
                Assert.Equal(2, projectFilePaths.Count());
                Assert.True(projectFilePaths.All(p => p.Value is string y && File.Exists(y) && (y.EndsWith(".csproj") || y.EndsWith("project.json") || y.EndsWith("proj"))));
            }
        }

        [Fact]
        public async Task TestPacMan_PreviewUpdatePackage_PackagesConfig_RaiseTelemetryEvents()
        {
            // Set up Package Source
            var packages = new List<SourcePackageDependencyInfo>
            {
                new SourcePackageDependencyInfo("a", new NuGetVersion(1, 0, 0), new[] { new Packaging.Core.PackageDependency("b", new VersionRange(new NuGetVersion(1, 0, 0))) }, true, null),
                new SourcePackageDependencyInfo("a", new NuGetVersion(2, 0, 0), new[] { new Packaging.Core.PackageDependency("b", new VersionRange(new NuGetVersion(2, 0, 0))) }, true, null),
                new SourcePackageDependencyInfo("b", new NuGetVersion(1, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("b", new NuGetVersion(2, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null)
            };

            var sourceRepositoryProvider = CreateSource(packages);

            // Set up NuGetProject
            var fwk45 = NuGetFramework.Parse("net45");

            var installedPackage1 = new PackageIdentity("a", new NuGetVersion(1, 0, 0));
            var installedPackage2 = new PackageIdentity("b", new NuGetVersion(1, 0, 0));

            var installedPackages = new List<NuGet.Packaging.PackageReference>
            {
                new NuGet.Packaging.PackageReference(installedPackage1, fwk45, true),
                new NuGet.Packaging.PackageReference(installedPackage2, fwk45, true)
            };

            var nuGetProject = new TestNuGetProject(installedPackages);

            // set up telemetry service
            var telemetrySession = new Mock<ITelemetrySession>();

            var telemetryEvents = new ConcurrentQueue<TelemetryEvent>();
            telemetrySession
                .Setup(x => x.PostEvent(It.IsAny<TelemetryEvent>()))
                .Callback<TelemetryEvent>(x => telemetryEvents.Enqueue(x));

            var nugetProjectContext = new TestNuGetProjectContext();
            var telemetryService = new NuGetVSTelemetryService(telemetrySession.Object);
            TelemetryActivity.NuGetTelemetryService = telemetryService;

            // Create Package Manager
            using (var solutionManager = new TestSolutionManager())
            {
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    Settings.LoadSpecificSettings(solutionManager.SolutionDirectory, "NuGet.Config"),
                    solutionManager,
                    new TestDeleteOnRestartManager());

                // Main Act
                var target = new PackageIdentity("a", new NuGetVersion(2, 0, 0));

                await nuGetPackageManager.PreviewUpdatePackagesAsync(
                    new List<PackageIdentity> { target },
                    new List<NuGetProject> { nuGetProject },
                    new ResolutionContext(),
                    nugetProjectContext,
                    sourceRepositoryProvider.GetRepositories(),
                    sourceRepositoryProvider.GetRepositories(),
                    CancellationToken.None);

                // Assert
                Assert.Equal(3, telemetryEvents.Count);
                VerifyPreviewActionsTelemetryEvents_PackagesConfig(telemetryEvents.Select(p => (string)p["SubStepName"]));
            }
        }

        [Fact]
        public async Task TestPacMan_ExecuteNuGetProjectActions_PackagesConfig_RaiseTelemetryEvents()
        {
            // Arrange
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV3OnlySourceRepositoryProvider();

            // set up telemetry service
            var telemetrySession = new Mock<ITelemetrySession>();

            var telemetryEvents = new ConcurrentQueue<TelemetryEvent>();
            telemetrySession
                .Setup(x => x.PostEvent(It.IsAny<TelemetryEvent>()))
                .Callback<TelemetryEvent>(x => telemetryEvents.Enqueue(x));

            var nugetProjectContext = new TestNuGetProjectContext();
            var telemetryService = new NuGetVSTelemetryService(telemetrySession.Object);
            TelemetryActivity.NuGetTelemetryService = telemetryService;

            // Create Package Manager
            using (var solutionManager = new TestSolutionManager())
            {
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    Settings.LoadSpecificSettings(solutionManager.SolutionDirectory, "NuGet.Config"),
                    solutionManager,
                    new TestDeleteOnRestartManager());

                var nugetProject = solutionManager.AddNewMSBuildProject();
                var target = _packageWithDependents[0];

                var projectActions = new List<NuGetProjectAction>();
                projectActions.Add(
                    NuGetProjectAction.CreateInstallProjectAction(
                        target,
                        sourceRepositoryProvider.GetRepositories().First(),
                        nugetProject));

                // Act
                await nuGetPackageManager.ExecuteNuGetProjectActionsAsync(
                    new List<NuGetProject>() { nugetProject },
                    projectActions,
                    nugetProjectContext,
                    NullSourceCacheContext.Instance,
                    CancellationToken.None);

                // Assert
                Assert.Equal(5, telemetryEvents.Count);
                Assert.Equal(1, telemetryEvents.Where(p => p.Name == "PackagePreFetcherInformation").Count());
                Assert.Equal(2, telemetryEvents.Where(p => p.Name == "PackageExtractionInformation").Count());
                Assert.Equal(1, telemetryEvents.Where(p => p.Name == "NugetActionSteps").Count());
                Assert.True(telemetryEvents.Where(p => p.Name == "NugetActionSteps").
                     Any(p => (string)p["SubStepName"] == TelemetryConstants.ExecuteActionStepName));
            }
        }

        [Fact]
        public async Task TestPacMan_ExecuteNuGetProjectActions_BuildIntegrated_RaiseTelemetryEvents()
        {
            // Arrange
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV3OnlySourceRepositoryProvider();

            // set up telemetry service
            var telemetrySession = new Mock<ITelemetrySession>();

            var telemetryEvents = new ConcurrentQueue<TelemetryEvent>();
            telemetrySession
                .Setup(x => x.PostEvent(It.IsAny<TelemetryEvent>()))
                .Callback<TelemetryEvent>(x => telemetryEvents.Enqueue(x));

            var nugetProjectContext = new TestNuGetProjectContext();
            var telemetryService = new NuGetVSTelemetryService(telemetrySession.Object);
            TelemetryActivity.NuGetTelemetryService = telemetryService;

            using (var settingsdir = TestDirectory.Create())
            using (var testSolutionManager = new TestSolutionManager())
            {
                var settings = Settings.LoadSpecificSettings(testSolutionManager.SolutionDirectory, "NuGet.Config");
                foreach (var source in sourceRepositoryProvider.GetRepositories())
                {
                    settings.AddOrUpdate(ConfigurationConstants.PackageSources, source.PackageSource.AsSourceItem());
                }

                var token = CancellationToken.None;
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    settings,
                    testSolutionManager,
                    deleteOnRestartManager);

                var installationCompatibility = new Mock<IInstallationCompatibility>();
                nuGetPackageManager.InstallationCompatibility = installationCompatibility.Object;

                var buildIntegratedProject = testSolutionManager.AddBuildIntegratedProject();

                var packageIdentity = _packageWithDependents[0];

                var projectActions = new List<NuGetProjectAction>();
                projectActions.Add(
                    NuGetProjectAction.CreateInstallProjectAction(
                        packageIdentity,
                        sourceRepositoryProvider.GetRepositories().First(),
                        buildIntegratedProject));

                // Act
                await nuGetPackageManager.ExecuteNuGetProjectActionsAsync(
                    new List<NuGetProject>() { buildIntegratedProject },
                    projectActions,
                    nugetProjectContext,
                    NullSourceCacheContext.Instance,
                    token);

                // Assert
                Assert.Equal(24, telemetryEvents.Count);

                Assert.Equal(2, telemetryEvents.Where(p => p.Name == "ProjectRestoreInformation").Count());
                Assert.Equal(2, telemetryEvents.Where(p => p.Name == "NugetActionSteps").Count());

                Assert.True(telemetryEvents.Where(p => p.Name == "NugetActionSteps").
                    Any(p => (string)p["SubStepName"] == TelemetryConstants.PreviewBuildIntegratedStepName));
                Assert.True(telemetryEvents.Where(p => p.Name == "NugetActionSteps").
                    Any(p => (string)p["SubStepName"] == TelemetryConstants.ExecuteActionStepName));

                var projectFilePaths = telemetryEvents.Where(p => p.Name == "ProjectRestoreInformation").SelectMany(x => x.GetPiiData()).Where(x => x.Key == "ProjectFilePath");
                Assert.Equal(2, projectFilePaths.Count());
                Assert.True(projectFilePaths.All(p => p.Value is string y && File.Exists(y) && (y.EndsWith(".csproj") || y.EndsWith("project.json") || y.EndsWith("proj"))));
            }
        }
#endif

        [Fact]
        public async Task TestPacManPreviewInstallPackage_WithGlobalPackageFolder()
        {
            using (
                var packageSource1 = TestDirectory.Create())
            {
                // Arrange
                var sourceRepositoryProvider1 = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(
                    new List<Configuration.PackageSource>()
                    {
                        new Configuration.PackageSource(packageSource1.Path)
                    });

                using (var testSolutionManager = new TestSolutionManager())
                {
                    var testSettings = NullSettings.Instance;
                    var token = CancellationToken.None;
                    var resolutionContext = new ResolutionContext();
                    var testNuGetProjectContext = new TestNuGetProjectContext();
                    var deleteOnRestartManager = new TestDeleteOnRestartManager();
                    var nuGetPackageManager = new NuGetPackageManager(
                        sourceRepositoryProvider1,
                        testSettings,
                        testSolutionManager,
                        deleteOnRestartManager);
                    var packagesFolderPath = PackagesFolderPathUtility.GetPackagesFolderPath(testSolutionManager, testSettings);
                    var projectA = testSolutionManager.AddBuildIntegratedProject();

                    var target = new PackageIdentity("packageA", NuGetVersion.Parse("1.0.0"));

                    var packageAContext = new SimpleTestPackageContext()
                    {
                        Id = "packageA",
                        Version = "1.0.0"
                    };

                    var saveMode = PackageSaveMode.Nuspec | PackageSaveMode.Files | PackageSaveMode.Nupkg;

                    await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                        packagesFolderPath,
                        saveMode,
                        packageAContext);

                    // ACT
                    var result = await nuGetPackageManager.PreviewInstallPackageAsync(
                        projectA,
                        target,
                        new ResolutionContext(),
                        new TestNuGetProjectContext(),
                        sourceRepositoryProvider1.GetRepositories(),
                        sourceRepositoryProvider1.GetRepositories(),
                        token);

                    // Assert
                    var resulting = result.Select(a => Tuple.Create(a.PackageIdentity, a.NuGetProjectActionType)).ToArray();

                    var expected = new List<Tuple<PackageIdentity, NuGetProjectActionType>>();
                    Expected(expected, target.Id, target.Version);

                    Assert.True(Compare(resulting, expected));
                }
            }
        }

        [Fact]
        public async Task TestPacMan_PreviewUpdatePackage_UnlistedPackage()
        {
            // Arrange

            // Set up Package Source
            var packages = new List<SourcePackageDependencyInfo>
            {
                new SourcePackageDependencyInfo("a", new NuGetVersion(1, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("a", new NuGetVersion(2, 0, 0), new Packaging.Core.PackageDependency[] { }, false, null)
            };

            var sourceRepositoryProvider = CreateSource(packages);

            // Set up NuGetProject
            var fwk45 = NuGetFramework.Parse("net45");

            var installedPackage1 = new PackageIdentity("a", new NuGetVersion(1, 0, 0));

            var installedPackages = new List<NuGet.Packaging.PackageReference>
            {
                new NuGet.Packaging.PackageReference(installedPackage1, fwk45, true)
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
                var targetPackageId = "a";

                var result = (await nuGetPackageManager.PreviewUpdatePackagesAsync(
                    targetPackageId,
                    new List<NuGetProject> { nuGetProject },
                    new ResolutionContext(DependencyBehavior.Lowest, false, false, VersionConstraints.None),
                    new TestNuGetProjectContext(),
                    sourceRepositoryProvider.GetRepositories(),
                    sourceRepositoryProvider.GetRepositories(),
                    CancellationToken.None)).ToList();

                // Assert
                Assert.Equal(0, result.Count);
            }
        }

        [Fact]
        public async Task TestPacMan_BuildIntegratedProject_PreviewUpdatePackage()
        {
            // Arrange

            // Set up Package Source
            var packages = new List<SourcePackageDependencyInfo>
            {
                new SourcePackageDependencyInfo("a", new NuGetVersion(1, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("a", new NuGetVersion(2, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null)
            };

            var sourceRepositoryProvider = CreateSource(packages);

            var json = new JObject
            {
                ["dependencies"] = new JObject()
                    {
                        new JProperty("a", "1.0.0")
                    },
                ["frameworks"] = new JObject
                {
                    ["net45"] = new JObject()
                }
            };

            // Create Package Manager
            using (var solutionManager = new TestSolutionManager())
            {
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    NullSettings.Instance,
                    solutionManager,
                    new TestDeleteOnRestartManager());

                var buildIntegratedProject = solutionManager.AddBuildIntegratedProject(json: json);

                // Main Act
                var targetPackageId = "a";

                var result = (await nuGetPackageManager.PreviewUpdatePackagesAsync(
                    targetPackageId,
                    new List<NuGetProject> { buildIntegratedProject },
                    new ResolutionContext(DependencyBehavior.Lowest, false, false, VersionConstraints.None),
                    new TestNuGetProjectContext(),
                    sourceRepositoryProvider.GetRepositories(),
                    sourceRepositoryProvider.GetRepositories(),
                    CancellationToken.None)).ToList();

                // Assert
                Assert.Equal(1, result.Count);
                Assert.Equal(NuGetProjectActionType.Install, result[0].NuGetProjectActionType);
                Assert.Equal(new PackageIdentity("a", new NuGetVersion(2, 0, 0)), result[0].PackageIdentity);
            }
        }

        [Fact]
        public async Task TestPacMan_MultipleBuildIntegratedProjects_PreviewUpdatePackage()
        {
            // This test was created after a multithreading bug was found. Like most multithreading bugs, it depends
            // very much on timing of exactly when different threads the relevant parts of the code, so it's difficult
            // to reproduce in a test.  Therefore, if this test fails randomly, it's not flaky, it's a product bug!
            // It's very unusual for a test to use Environment.ProcessorCount, but this is why.

            // Arrange

            // Set up Package Source
            var packages = new List<SourcePackageDependencyInfo>
            {
                new SourcePackageDependencyInfo("a", new NuGetVersion(1, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("a", new NuGetVersion(2, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null)
            };

            var sourceRepositoryProvider = CreateSource(packages);

            var json = new JObject
            {
                ["dependencies"] = new JObject()
                    {
                        new JProperty("a", "1.0.0")
                    },
                ["frameworks"] = new JObject
                {
                    ["net45"] = new JObject()
                }
            };

            // Create Package Manager
            using (var solutionManager = new TestSolutionManager())
            {
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    NullSettings.Instance,
                    solutionManager,
                    new TestDeleteOnRestartManager());

                var projectCount = Environment.ProcessorCount * 2;
                var projects = new List<NuGetProject>(projectCount);
                for (int i = 0; i < projectCount; i++)
                {
                    var project = solutionManager.AddBuildIntegratedProject(json: json);
                    projects.Add(project);
                }

                // Main Act
                var results = (await nuGetPackageManager.PreviewUpdatePackagesAsync(
                    "a",
                    projects,
                    new ResolutionContext(DependencyBehavior.Lowest, false, false, VersionConstraints.None),
                    new TestNuGetProjectContext(),
                    sourceRepositoryProvider.GetRepositories(),
                    sourceRepositoryProvider.GetRepositories(),
                    CancellationToken.None)).ToList();

                // Assert
                Assert.Equal(projectCount, results.Count);
                Assert.All(results,
                    result =>
                    {
                        Assert.Equal(NuGetProjectActionType.Install, result.NuGetProjectActionType);
                        Assert.Equal(new PackageIdentity("a", new NuGetVersion(2, 0, 0)), result.PackageIdentity);
                    });
            }
        }

        [Fact]
        public async Task TestPacMan_PreviewInstallPackage_BuildIntegrated_MissingPath_Throws()
        {
            // Arrange
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV3OnlySourceRepositoryProvider();

            var nugetProjectContext = new TestNuGetProjectContext();

            // Create Package Manager
            using (var solutionManager = new TestSolutionManager())
            {
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    NullSettings.Instance,
                    solutionManager,
                    new TestDeleteOnRestartManager());

                var buildIntegratedProjectA = solutionManager.AddBuildIntegratedProject("projectA") as BuildIntegratedNuGetProject;

                // Act
                var primarySources = sourceRepositoryProvider.GetRepositories() as IReadOnlyCollection<SourceRepository>;
                var target = _packageWithDependents[0];
                IReadOnlyList<BuildIntegratedNuGetProject> projects = new List<BuildIntegratedNuGetProject>()
                {
                    buildIntegratedProjectA
                };

                var nugetAction = NuGetProjectAction.CreateInstallProjectAction(target, primarySources.First(), buildIntegratedProjectA);
                NuGetProjectAction[] actions = new NuGetProjectAction[] { nugetAction };

                Dictionary<string, NuGetProjectAction[]> nugetProjectActionsLookup =
                    new Dictionary<string, NuGetProjectAction[]>(PathUtility.GetStringComparerBasedOnOS())
                {
                    { "wrong path", actions }
                };

                var ex = await Assert.ThrowsAsync<ArgumentException>(async () =>
                {
                    await nuGetPackageManager.PreviewBuildIntegratedProjectsActionsAsync(
                        projects,
                        nugetProjectActionsLookup,
                        packageIdentity: null,
                        primarySources,
                        nugetProjectContext,
                        versionRange: null,
                        CancellationToken.None);
                });

                // Assert
                Assert.Contains("Either should have value in", ex.Message);
                Assert.Contains(buildIntegratedProjectA.MSBuildProjectPath, ex.Message);
            }
        }

        /// <summary>
        /// Repro for a bug caused by a NullReferenceException being thrown due to a null <see cref="PackageIdentity.Version"/>
        /// (https://github.com/NuGet/Home/issues/9882).
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task TestPacMan_PreviewInstallPackage_BuildIntegrated_NullVersion_Throws()
        {
            // Arrange

            // Set up Package Source
            var packages = new List<SourcePackageDependencyInfo>
            {
                new SourcePackageDependencyInfo("a", new NuGetVersion(1, 0, 0), new PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("a", new NuGetVersion(2, 0, 0), new PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("a", new NuGetVersion(3, 0, 0), new PackageDependency[] { }, true, null),
            };

            SourceRepositoryProvider sourceRepositoryProvider = CreateSource(packages);

            // Set up NuGetProject
            var fwk45 = NuGetFramework.Parse("net45");

            var installedPackages = new List<NuGet.Packaging.PackageReference>
            {
                new PackageReference(new PackageIdentity("a", new NuGetVersion(1, 0, 0)), fwk45, true),
            };

            var packageIdentity = _packageWithDependents[0];

            // Create Package Manager
            using (var solutionManager = new TestSolutionManager())
            {
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    NullSettings.Instance,
                    solutionManager,
                    new TestDeleteOnRestartManager());

                var buildIntegratedProjectA = new Mock<BuildIntegratedNuGetProject>();
                buildIntegratedProjectA.Setup(p => p.GetInstalledPackagesAsync(CancellationToken.None))
                    .Returns(() => Task.FromResult(installedPackages.AsEnumerable()));

                var projectList = new List<NuGetProject> { buildIntegratedProjectA.Object };
                solutionManager.NuGetProjects = projectList;

                // Main Act
                var targets = new List<PackageIdentity>
                {
                    new PackageIdentity("a", null)
                };

                // Assert
                var ex = await Assert.ThrowsAsync<NullReferenceException>(async () =>
                {
                    IEnumerable<NuGetProjectAction> result = await nuGetPackageManager.PreviewUpdatePackagesAsync(
                        targets,
                        projectList,
                        new ResolutionContext(),
                        new TestNuGetProjectContext(),
                        sourceRepositoryProvider.GetRepositories(),
                        sourceRepositoryProvider.GetRepositories(),
                        CancellationToken.None);
                });
            }
        }

        [Fact]
        public async Task TestPacManPreview_InstallForPC_PackageSourceMapping_WithSingleFeed_Succeeds()
        {
            // Arrange
            using (var testSolutionManager = new TestSolutionManager())
            {
                // Set up Package Source
                var sources = new List<PackageSource>();

                var privateRepositoryPath = Path.Combine(testSolutionManager.SolutionDirectory, "PrivateRepository");
                Directory.CreateDirectory(privateRepositoryPath);

                // Replace the default nuget.config with custom one.
                SettingsTestUtils.CreateConfigurationFile(testSolutionManager.NuGetConfigPath, $@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSources>
    <!--To inherit the global NuGet package sources remove the <clear/> line below -->
    <clear />
    <add key=""PrivateRepository"" value=""{privateRepositoryPath}"" />
    </packageSources>
    <packageSourceMapping>
        <packageSource key=""PrivateRepository"">
            <package pattern=""Contoso.*"" />
            <package pattern=""Test.*"" />
        </packageSource>
    </packageSourceMapping>
</configuration>");
                var contosoPackageIdentity = new PackageIdentity("Contoso.A", new NuGetVersion("1.0.0"));

                var ContosoReal = new SimpleTestPackageContext()
                {
                    Id = contosoPackageIdentity.Id,
                    Version = "1.0.0"
                };
                ContosoReal.AddFile("lib/net461/contosoA.dll");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    privateRepositoryPath,
                    PackageSaveMode.Defaultv3,
                    ContosoReal);

                sources.Add(new PackageSource(privateRepositoryPath, "PrivateRepository"));

                SourceRepositoryProvider sourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(sources);

                ISettings testSettings = Settings.LoadSpecificSettings(testSolutionManager.SolutionDirectory, "NuGet.Config");
                var token = CancellationToken.None;
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    testSettings,
                    testSolutionManager,
                    deleteOnRestartManager);

                var msBuildNuGetProject = testSolutionManager.AddNewMSBuildProject();
                var msBuildNuGetProjectSystem = msBuildNuGetProject.ProjectSystem as TestMSBuildNuGetProjectSystem;
                var packagesConfigPath = msBuildNuGetProject.PackagesConfigNuGetProject.FullPath;

                // Pre-Assert
                // Check that the packages.config file does not exist
                Assert.False(File.Exists(packagesConfigPath));
                // Check that there are no packages returned by PackagesConfigProject
                var packagesInPackagesConfig = (await msBuildNuGetProject.PackagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(0, packagesInPackagesConfig.Count);
                Assert.Equal(0, msBuildNuGetProjectSystem.References.Count);

                // Act
                var packageActions = (await nuGetPackageManager.PreviewInstallPackageAsync(msBuildNuGetProject, contosoPackageIdentity,
                    new ResolutionContext(), new TestNuGetProjectContext(), sourceRepositoryProvider.GetRepositories(), null, token)).ToList();

                // Assert
                Assert.Equal(1, packageActions.Count());
                Assert.True(contosoPackageIdentity.Equals(packageActions[0].PackageIdentity));
                Assert.Equal(NuGetProjectActionType.Install, packageActions[0].NuGetProjectActionType);
                Assert.Equal(privateRepositoryPath,
                    packageActions[0].SourceRepository.PackageSource.Source);
            }
        }

        [Fact]
        public async Task TestPacManPreview_InstallForPC_PackageSourceMapping_WithMultipleFeedsWithIdenticalPackages_RestoresCorrectPackage()
        {
            // This test same as having multiple source repositories and `All` option is selected in PMUI.
            // Arrange
            using (var testSolutionManager = new TestSolutionManager())
            {
                // Set up Package Source
                var sources = new List<PackageSource>();
                var externalRepositoryPath = Path.Combine(testSolutionManager.SolutionDirectory, "ExternalRepository");
                Directory.CreateDirectory(externalRepositoryPath);

                var privateRepositoryPath = Path.Combine(testSolutionManager.SolutionDirectory, "PrivateRepository");
                Directory.CreateDirectory(privateRepositoryPath);

                // Replace the default nuget.config with custom one.
                SettingsTestUtils.CreateConfigurationFile(testSolutionManager.NuGetConfigPath, $@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSources>
    <!--To inherit the global NuGet package sources remove the <clear/> line below -->
    <clear />
    <add key=""ExternalRepository"" value=""{externalRepositoryPath}"" />
    <add key=""PrivateRepository"" value=""{privateRepositoryPath}"" />
    </packageSources>
    <packageSourceMapping>
        <packageSource key=""externalRepository"">
            <package pattern=""External.*"" />
            <package pattern=""Others.*"" />
        </packageSource>
        <packageSource key=""PrivateRepository"">
            <package pattern=""Contoso.*"" />
            <package pattern=""Test.*"" />
        </packageSource>
    </packageSourceMapping>
</configuration>");
                var contosoPackageIdentity = new PackageIdentity("Contoso.A", new NuGetVersion("1.0.0"));

                var ExternalA = new SimpleTestPackageContext()
                {
                    Id = contosoPackageIdentity.Id,  // Package id conflict with PrivateRepository
                    Version = "1.0.0"
                };
                ExternalA.AddFile("lib/net461/externalA.dll");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    externalRepositoryPath,
                    PackageSaveMode.Defaultv3,
                    ExternalA);

                sources.Add(new PackageSource(externalRepositoryPath, "ExternalRepository"));

                var ContosoReal = new SimpleTestPackageContext()
                {
                    Id = contosoPackageIdentity.Id,
                    Version = "1.0.0"
                };
                ContosoReal.AddFile("lib/net461/contosoA.dll");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    privateRepositoryPath,
                    PackageSaveMode.Defaultv3,
                    ContosoReal);

                sources.Add(new PackageSource(privateRepositoryPath, "PrivateRepository"));

                SourceRepositoryProvider sourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(sources);

                ISettings testSettings = Settings.LoadSpecificSettings(testSolutionManager.SolutionDirectory, "NuGet.Config");
                var token = CancellationToken.None;
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    testSettings,
                    testSolutionManager,
                    deleteOnRestartManager);

                var msBuildNuGetProject = testSolutionManager.AddNewMSBuildProject();
                var msBuildNuGetProjectSystem = msBuildNuGetProject.ProjectSystem as TestMSBuildNuGetProjectSystem;
                var packagesConfigPath = msBuildNuGetProject.PackagesConfigNuGetProject.FullPath;

                // Pre-Assert
                // Check that the packages.config file does not exist
                Assert.False(File.Exists(packagesConfigPath));
                // Check that there are no packages returned by PackagesConfigProject
                var packagesInPackagesConfig = (await msBuildNuGetProject.PackagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(0, packagesInPackagesConfig.Count);
                Assert.Equal(0, msBuildNuGetProjectSystem.References.Count);

                // Act
                var packageActions = (await nuGetPackageManager.PreviewInstallPackageAsync(msBuildNuGetProject, contosoPackageIdentity,
                    new ResolutionContext(), new TestNuGetProjectContext(), sourceRepositoryProvider.GetRepositories(), null, token)).ToList();

                // Assert
                Assert.Equal(1, packageActions.Count());
                Assert.True(contosoPackageIdentity.Equals(packageActions[0].PackageIdentity));
                Assert.Equal(NuGetProjectActionType.Install, packageActions[0].NuGetProjectActionType);
                Assert.Equal(privateRepositoryPath,
                    packageActions[0].SourceRepository.PackageSource.Source);
            }
        }

        [Fact]
        public async Task TestPacManPreview_InstallForPC_PackageSourceMapping_WithMultipleFeeds_SecondarySourcesNotConsidered()
        {
            // This test same as having multiple source repositories but only 1 sourcerepository is selected in PMUI.
            // Direct package dependencies doesn't consider secondary sources(not selected sources on UI).
            // Arrange
            using (var testSolutionManager = new TestSolutionManager())
            {
                // Set up Package Source
                var primarySources = new List<PackageSource>();
                var secondarySources = new List<PackageSource>();
                var externalRepositoryPath = Path.Combine(testSolutionManager.SolutionDirectory, "ExternalRepository");
                Directory.CreateDirectory(externalRepositoryPath);

                var privateRepositoryPath = Path.Combine(testSolutionManager.SolutionDirectory, "PrivateRepository");
                Directory.CreateDirectory(privateRepositoryPath);

                // Replace the default nuget.config with custom one.
                SettingsTestUtils.CreateConfigurationFile(testSolutionManager.NuGetConfigPath, $@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSources>
    <!--To inherit the global NuGet package sources remove the <clear/> line below -->
    <clear />
    <add key=""ExternalRepository"" value=""{externalRepositoryPath}"" />
    <add key=""PrivateRepository"" value=""{privateRepositoryPath}"" />
    </packageSources>
    <packageSourceMapping>
        <packageSource key=""externalRepository"">
            <package pattern=""External.*"" />
            <package pattern=""Others.*"" />
        </packageSource>
        <packageSource key=""PrivateRepository"">
            <package pattern=""Contoso.*"" />
            <package pattern=""Test.*"" />
        </packageSource>
    </packageSourceMapping>
</configuration>");
                var contosoPackageIdentity = new PackageIdentity("Contoso.A", new NuGetVersion("1.0.0"));

                var ExternalA = new SimpleTestPackageContext()
                {
                    Id = contosoPackageIdentity.Id,  // Package id conflict with PrivateRepository
                    Version = "1.0.0"
                };
                ExternalA.AddFile("lib/net461/externalA.dll");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    externalRepositoryPath,
                    PackageSaveMode.Defaultv3,
                    ExternalA);

                var primarySource = new PackageSource(externalRepositoryPath, "ExternalRepository");

                var ContosoReal = new SimpleTestPackageContext()
                {
                    Id = contosoPackageIdentity.Id,
                    Version = "1.0.0"
                };
                ContosoReal.AddFile("lib/net461/contosoA.dll");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    privateRepositoryPath,
                    PackageSaveMode.Defaultv3,
                    ContosoReal);

                primarySources.Add(primarySource);
                secondarySources.AddRange(primarySources);
                secondarySources.Add(new PackageSource(privateRepositoryPath, "PrivateRepository"));

                SourceRepositoryProvider primarySourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(primarySources);
                SourceRepositoryProvider secondarySourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(secondarySources);

                ISettings testSettings = Settings.LoadSpecificSettings(testSolutionManager.SolutionDirectory, "NuGet.Config");
                var token = CancellationToken.None;
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    secondarySourceRepositoryProvider,
                    testSettings,
                    testSolutionManager,
                    deleteOnRestartManager);

                var msBuildNuGetProject = testSolutionManager.AddNewMSBuildProject();
                var msBuildNuGetProjectSystem = msBuildNuGetProject.ProjectSystem as TestMSBuildNuGetProjectSystem;
                var packagesConfigPath = msBuildNuGetProject.PackagesConfigNuGetProject.FullPath;

                // Pre-Assert
                // Check that the packages.config file does not exist
                Assert.False(File.Exists(packagesConfigPath));
                // Check that there are no packages returned by PackagesConfigProject
                var packagesInPackagesConfig = (await msBuildNuGetProject.PackagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(0, packagesInPackagesConfig.Count);
                Assert.Equal(0, msBuildNuGetProjectSystem.References.Count);

                // Act and Assert
                var ex = await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                {
                    await nuGetPackageManager.PreviewInstallPackageAsync(msBuildNuGetProject, contosoPackageIdentity.Id,
                    new ResolutionContext(), new TestNuGetProjectContext(), primarySourceRepositoryProvider.GetRepositories(), secondarySourceRepositoryProvider.GetRepositories(), token);
                });

                // Even though Contoso.A 1.0.0 exist in ExternalRepository it wouldn't be picked for install.
                // PrivateRepository is passed in secondary sources but for packages.config project not considered.
                Assert.True(ex.Message.StartsWith("Package 'Contoso.A 1.0.0' is not found in the following primary source(s): "));
            }
        }

        [Fact]
        public async Task TestPacManPreview_InstallForPC_PackageSourceMapping_WithMultipleFeeds_ForTransitiveDepency_SecondarySourcesConsidered()
        {
            // This test same as having multiple source repositories but only 1 sourcerepository is selected in PMUI.
            // Even though direct package dependencies doesn't consider secondary sources(not selected sources on UI), but transitive dependencies do consider it.
            // Arrange
            using (var testSolutionManager = new TestSolutionManager())
            {
                // Set up Package Source
                var primarySources = new List<PackageSource>();
                var secondarySources = new List<PackageSource>();
                var externalRepositoryPath = Path.Combine(testSolutionManager.SolutionDirectory, "ExternalRepository");
                Directory.CreateDirectory(externalRepositoryPath);

                var privateRepositoryPath = Path.Combine(testSolutionManager.SolutionDirectory, "PrivateRepository");
                Directory.CreateDirectory(privateRepositoryPath);

                // Replace the default nuget.config with custom one.
                SettingsTestUtils.CreateConfigurationFile(testSolutionManager.NuGetConfigPath, $@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSources>
    <!--To inherit the global NuGet package sources remove the <clear/> line below -->
    <clear />
    <add key=""ExternalRepository"" value=""{externalRepositoryPath}"" />
    <add key=""PrivateRepository"" value=""{privateRepositoryPath}"" />
    </packageSources>
    <packageSourceMapping>
        <packageSource key=""externalRepository"">
            <package pattern=""Direct.*"" />
            <package pattern=""External.*"" />
            <package pattern=""Others.*"" />
        </packageSource>
        <packageSource key=""PrivateRepository"">
            <package pattern=""Contoso.*"" />
            <package pattern=""Test.*"" />
        </packageSource>
    </packageSourceMapping>
</configuration>");
                var contosoPackageIdentity = new PackageIdentity("Contoso.A", new NuGetVersion("1.0.0"));
                var directPackageIdentity = new PackageIdentity("Direct.A", new NuGetVersion("1.0.0"));

                var ExternalContosoA = new SimpleTestPackageContext()
                {
                    Id = contosoPackageIdentity.Id,  // Package id conflict with PrivateRepository
                    Version = "1.0.0"
                };
                ExternalContosoA.AddFile("lib/net461/externalA.dll");

                var ExternalDirectA = new SimpleTestPackageContext()
                {
                    Id = directPackageIdentity.Id,
                    Version = "1.0.0",
                    Dependencies = new List<SimpleTestPackageContext>() { ExternalContosoA } // We set Contoso.A as dependent package.
                };
                ExternalDirectA.AddFile("lib/net461/directA.dll");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    externalRepositoryPath,
                    PackageSaveMode.Defaultv3,
                    ExternalContosoA, ExternalDirectA);

                var primarySource = new PackageSource(externalRepositoryPath, "ExternalRepository");

                var ContosoReal = new SimpleTestPackageContext()
                {
                    Id = contosoPackageIdentity.Id,
                    Version = "1.0.0"
                };
                ContosoReal.AddFile("lib/net461/contosoA.dll");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    privateRepositoryPath,
                    PackageSaveMode.Defaultv3,
                    ContosoReal);

                primarySources.Add(primarySource);
                secondarySources.AddRange(primarySources);
                secondarySources.Add(new PackageSource(privateRepositoryPath, "PrivateRepository"));

                SourceRepositoryProvider primarySourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(primarySources);
                SourceRepositoryProvider secondarySourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(secondarySources);

                ISettings testSettings = Settings.LoadSpecificSettings(testSolutionManager.SolutionDirectory, "NuGet.Config");
                var token = CancellationToken.None;
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    secondarySourceRepositoryProvider,
                    testSettings,
                    testSolutionManager,
                    deleteOnRestartManager);

                var msBuildNuGetProject = testSolutionManager.AddNewMSBuildProject();
                var msBuildNuGetProjectSystem = msBuildNuGetProject.ProjectSystem as TestMSBuildNuGetProjectSystem;
                var packagesConfigPath = msBuildNuGetProject.PackagesConfigNuGetProject.FullPath;

                // Pre-Assert
                // Check that the packages.config file does not exist
                Assert.False(File.Exists(packagesConfigPath));
                // Check that there are no packages returned by PackagesConfigProject
                var packagesInPackagesConfig = (await msBuildNuGetProject.PackagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(0, packagesInPackagesConfig.Count);
                Assert.Equal(0, msBuildNuGetProjectSystem.References.Count);

                // Act
                var packageActions = (await nuGetPackageManager.PreviewInstallPackageAsync(msBuildNuGetProject, directPackageIdentity.Id,
                    new ResolutionContext(), new TestNuGetProjectContext(), primarySourceRepositoryProvider.GetRepositories(), secondarySourceRepositoryProvider.GetRepositories(), token)).ToList();

                // Assert
                Assert.Equal(2, packageActions.Count());
                Assert.True(contosoPackageIdentity.Equals(packageActions[0].PackageIdentity));
                Assert.Equal(NuGetProjectActionType.Install, packageActions[0].NuGetProjectActionType);
                // Contoso.A comes from PrivateRepository due to package source mapping filtering even though same package Id exist in Externalrepository.
                Assert.Equal(privateRepositoryPath,
                    packageActions[0].SourceRepository.PackageSource.Source);

                Assert.True(directPackageIdentity.Equals(packageActions[1].PackageIdentity));
                Assert.Equal(NuGetProjectActionType.Install, packageActions[1].NuGetProjectActionType);
                // Direct.A comes from Externalrepository.
                Assert.Equal(externalRepositoryPath,
                    packageActions[1].SourceRepository.PackageSource.Source);
            }
        }

        [Fact]
        public async Task TestPacManPreview_UpdateForPC_PackageSourceMapping_WithMultipleFeeds_SecondarySourcesNotConsidered()
        {
            // This test same as having multiple source repositories but only 1 sourcerepository is selected in PMUI.
            // Arrange
            using (var testSolutionManager = new TestSolutionManager())
            {
                // Set up Package Source
                var primarySources = new List<PackageSource>();
                var secondarySources = new List<PackageSource>();
                var externalRepositoryPath = Path.Combine(testSolutionManager.SolutionDirectory, "ExternalRepository");
                Directory.CreateDirectory(externalRepositoryPath);

                var privateRepositoryPath = Path.Combine(testSolutionManager.SolutionDirectory, "PrivateRepository");
                Directory.CreateDirectory(privateRepositoryPath);

                // Replace the default nuget.config with custom one.
                SettingsTestUtils.CreateConfigurationFile(testSolutionManager.NuGetConfigPath, $@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSources>
    <!--To inherit the global NuGet package sources remove the <clear/> line below -->
    <clear />
    <add key=""ExternalRepository"" value=""{externalRepositoryPath}"" />
    <add key=""PrivateRepository"" value=""{privateRepositoryPath}"" />
    </packageSources>
    <packageSourceMapping>
        <packageSource key=""externalRepository"">
            <package pattern=""External.*"" />
            <package pattern=""Others.*"" />
        </packageSource>
        <packageSource key=""PrivateRepository"">
            <package pattern=""Contoso.*"" />
            <package pattern=""Test.*"" />
        </packageSource>
    </packageSourceMapping>
</configuration>");
                var contosoPackageIdentity = new PackageIdentity("Contoso.A", new NuGetVersion("1.0.0"));

                var ExternalA_v1 = new SimpleTestPackageContext()
                {
                    Id = contosoPackageIdentity.Id,  // Package id conflict with PrivateRepository
                    Version = "1.0.0"
                };
                ExternalA_v1.AddFile("lib/net461/externalA.dll");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    externalRepositoryPath,
                    PackageSaveMode.Defaultv3,
                    ExternalA_v1);

                var ExternalA_v2 = new SimpleTestPackageContext()
                {
                    Id = contosoPackageIdentity.Id,  // Package id conflict with PrivateRepository
                    Version = "2.0.0"
                };
                ExternalA_v2.AddFile("lib/net461/externalA.dll");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    externalRepositoryPath,
                    PackageSaveMode.Defaultv3,
                    ExternalA_v2);

                var primarySource = new PackageSource(externalRepositoryPath, "ExternalRepository");

                var ContosoReal_V1 = new SimpleTestPackageContext()
                {
                    Id = contosoPackageIdentity.Id,
                    Version = "1.0.0"
                };
                ContosoReal_V1.AddFile("lib/net461/contosoA.dll");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    privateRepositoryPath,
                    PackageSaveMode.Defaultv3,
                    ContosoReal_V1);

                var ContosoReal_V2 = new SimpleTestPackageContext()
                {
                    Id = contosoPackageIdentity.Id,
                    Version = "2.0.0"
                };
                ContosoReal_V2.AddFile("lib/net461/contosoA.dll");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    privateRepositoryPath,
                    PackageSaveMode.Defaultv3,
                    ContosoReal_V2);

                primarySources.Add(primarySource);
                secondarySources.AddRange(primarySources);
                secondarySources.Add(new PackageSource(privateRepositoryPath, "PrivateRepository"));

                SourceRepositoryProvider primarySourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(primarySources);
                SourceRepositoryProvider secondarySourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(secondarySources);

                ISettings testSettings = Settings.LoadSpecificSettings(testSolutionManager.SolutionDirectory, "NuGet.Config");
                var token = CancellationToken.None;
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    secondarySourceRepositoryProvider,
                    testSettings,
                    testSolutionManager,
                    deleteOnRestartManager);

                var packagesFolderPath = PackagesFolderPathUtility.GetPackagesFolderPath(testSolutionManager, testSettings);

                var msBuildNuGetProject = testSolutionManager.AddNewMSBuildProject();
                var msBuildNuGetProjectSystem = msBuildNuGetProject.ProjectSystem as TestMSBuildNuGetProjectSystem;
                var packagesConfigPath = msBuildNuGetProject.PackagesConfigNuGetProject.FullPath;

                var resolutionContext = new ResolutionContext();
                var resolvedPackage = await NuGetPackageManager.GetLatestVersionAsync(
                    contosoPackageIdentity.Id,
                    msBuildNuGetProject,
                    new ResolutionContext(),
                    secondarySourceRepositoryProvider.GetRepositories().First(),
                    Common.NullLogger.Instance,
                    token);

                var packageLatest = new PackageIdentity(contosoPackageIdentity.Id, resolvedPackage.LatestVersion);
                var nugetProjectContext = new TestNuGetProjectContext();

                // Pre-Assert
                // Check that the packages.config file does not exist
                Assert.False(File.Exists(packagesConfigPath));
                // Check that there are no packages returned by PackagesConfigProject
                var packagesInPackagesConfig = (await msBuildNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(0, packagesInPackagesConfig.Count);
                Assert.Equal(0, msBuildNuGetProjectSystem.References.Count);

                // Act
                // Passing both ExternalRepository and PrivateRepository via secondarySourceRepositoryProvider as primary sources, so it can install.
                await nuGetPackageManager.InstallPackageAsync(msBuildNuGetProject, contosoPackageIdentity,
                    resolutionContext, nugetProjectContext, secondarySourceRepositoryProvider.GetRepositories(), null, token);

                // Assert
                // Check that the packages.config file exists after the installation
                Assert.True(File.Exists(packagesConfigPath));
                // Check the number of packages and packages returned by PackagesConfigProject after the installation
                packagesInPackagesConfig = (await msBuildNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(1, packagesInPackagesConfig.Count);
                Assert.Equal(contosoPackageIdentity, packagesInPackagesConfig[0].PackageIdentity);
                Assert.Equal(msBuildNuGetProject.ProjectSystem.TargetFramework, packagesInPackagesConfig[0].TargetFramework);

                // Main Act and Assert
                var ex = await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                {
                    await nuGetPackageManager.PreviewUpdatePackagesAsync(
                        new List<NuGetProject> { msBuildNuGetProject },
                        new ResolutionContext(DependencyBehavior.Highest, false, true, VersionConstraints.None),
                        nugetProjectContext,
                        primarySourceRepositoryProvider.GetRepositories(),
                        secondarySourceRepositoryProvider.GetRepositories(),
                        token);
                });

                // Even though Contoso.A 2.0.0 exist in ExternalRepository it wouldn't be picked for install.
                // PrivateRepository is passed in secondary sources but for packages.config project not considered.
                // Since we requested bulk upgrade the error message would be generic.
                Assert.True(ex.Message.StartsWith("Unable to gather dependency information for multiple packages"));
            }
        }

        [Fact]
        public async Task TestPacManPreview_UpdateForPC_PackageSourceMapping_WithMultipleFeeds_Fails()
        {
            // Arrange
            using (var testSolutionManager = new TestSolutionManager())
            {
                // Set up Package Source
                var sources = new List<PackageSource>();
                var externalRepositoryPath = Path.Combine(testSolutionManager.SolutionDirectory, "ExternalRepository");
                Directory.CreateDirectory(externalRepositoryPath);

                var privateRepositoryPath = Path.Combine(testSolutionManager.SolutionDirectory, "PrivateRepository");
                Directory.CreateDirectory(privateRepositoryPath);

                // Replace the default nuget.config with custom one.
                SettingsTestUtils.CreateConfigurationFile(testSolutionManager.NuGetConfigPath, $@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSources>
    <!--To inherit the global NuGet package sources remove the <clear/> line below -->
    <clear />
    <add key=""ExternalRepository"" value=""{externalRepositoryPath}"" />
    <add key=""PrivateRepository"" value=""{privateRepositoryPath}"" />
    </packageSources>
    <packageSourceMapping>
        <packageSource key=""externalRepository"">
            <package pattern=""External.*"" />
            <package pattern=""Others.*"" />
        </packageSource>
        <packageSource key=""PrivateRepository"">
            <package pattern=""Contoso.*"" />
            <package pattern=""Test.*"" />
        </packageSource>
    </packageSourceMapping>
</configuration>");
                var contosoPackageIdentity = new PackageIdentity("Contoso.A", new NuGetVersion("1.0.0"));

                var ExternalA = new SimpleTestPackageContext()
                {
                    Id = contosoPackageIdentity.Id,  // Initial version had package id conflict with Contoso repository
                    Version = "1.0.0"
                };
                ExternalA.AddFile("lib/net461/externalA.dll");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    externalRepositoryPath,
                    PackageSaveMode.Defaultv3,
                    ExternalA);

                var ExternalB = new SimpleTestPackageContext()
                {
                    Id = "External.B",  // name conflict resolved.
                    Version = "2.0.0"
                };
                ExternalB.AddFile("lib/net461/externalB.dll");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    externalRepositoryPath,
                    PackageSaveMode.Defaultv3,
                    ExternalB);

                sources.Add(new PackageSource(externalRepositoryPath, "ExternalRepository"));

                var ContosoReal = new SimpleTestPackageContext()
                {
                    Id = contosoPackageIdentity.Id,
                    Version = "2.0.0"
                };
                ContosoReal.AddFile("lib/net461/contosoA.dll");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    privateRepositoryPath,
                    PackageSaveMode.Defaultv3,
                    ContosoReal);

                sources.Add(new PackageSource(privateRepositoryPath, "PrivateRepository"));

                SourceRepositoryProvider sourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(sources);

                ISettings testSettings = Settings.LoadSpecificSettings(testSolutionManager.SolutionDirectory, "NuGet.Config");
                var token = CancellationToken.None;
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    testSettings,
                    testSolutionManager,
                    deleteOnRestartManager);

                var msBuildNuGetProject = testSolutionManager.AddNewMSBuildProject();
                var msBuildNuGetProjectSystem = msBuildNuGetProject.ProjectSystem as TestMSBuildNuGetProjectSystem;
                var packagesConfigPath = msBuildNuGetProject.PackagesConfigNuGetProject.FullPath;

                // Pre-Assert
                // Check that the packages.config file does not exist
                Assert.False(File.Exists(packagesConfigPath));
                // Check that there are no packages returned by PackagesConfigProject
                var packagesInPackagesConfig = (await msBuildNuGetProject.PackagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(0, packagesInPackagesConfig.Count);
                Assert.Equal(0, msBuildNuGetProjectSystem.References.Count);

                // Act and Assert
                var ex = await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                {
                    await nuGetPackageManager.PreviewInstallPackageAsync(msBuildNuGetProject, contosoPackageIdentity,
                    new ResolutionContext(), new TestNuGetProjectContext(), sourceRepositoryProvider.GetRepositories(), null, token);
                });

                // Even though Contoso.A 1.0.0 exist in ExternalRepository it wouldn't be picked for install.
                Assert.True(ex.Message.StartsWith("Package 'Contoso.A 1.0.0' is not found in the following primary source(s): "));
            }
        }

        private void VerifyPreviewActionsTelemetryEvents_PackagesConfig(IEnumerable<string> actual)
        {
            Assert.True(actual.Contains(TelemetryConstants.GatherDependencyStepName));
            Assert.True(actual.Contains(TelemetryConstants.ResolveDependencyStepName));
            Assert.True(actual.Contains(TelemetryConstants.ResolvedActionsStepName));
        }

        private static void AddToPackagesFolder(PackageIdentity package, string root)
        {
            var dir = Path.Combine(root, $"{package.Id}.{package.Version.ToString()}");
            Directory.CreateDirectory(dir);

            var context = new SimpleTestPackageContext()
            {
                Id = package.Id,
                Version = package.Version.ToString()
            };

            context.AddFile("lib/net45/a.dll");
            SimpleTestPackageUtility.CreateOPCPackage(context, dir);
        }

        private SourceRepositoryProvider CreateSource(List<SourcePackageDependencyInfo> packages)
        {
            var resourceProviders = new List<Lazy<INuGetResourceProvider>>();
            resourceProviders.Add(new Lazy<INuGetResourceProvider>(() => new TestDependencyInfoProvider(packages)));
            resourceProviders.Add(new Lazy<INuGetResourceProvider>(() => new TestMetadataProvider(packages)));

            var packageSource = new Configuration.PackageSource("http://temp");
            var packageSourceProvider = new TestPackageSourceProvider(new[] { packageSource });

            return new SourceRepositoryProvider(packageSourceProvider, resourceProviders);
        }

        private static void Expected(List<Tuple<PackageIdentity, NuGetProjectActionType>> expected, string id, NuGetVersion oldVersion, NuGetVersion newVersion)
        {
            expected.Add(Tuple.Create(new PackageIdentity(id, oldVersion), NuGetProjectActionType.Uninstall));
            expected.Add(Tuple.Create(new PackageIdentity(id, newVersion), NuGetProjectActionType.Install));
        }

        private static void Expected(List<Tuple<PackageIdentity, NuGetProjectActionType>> expected, string id, NuGetVersion newVersion)
        {
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

        private static bool PreviewResultsCompare(
            IEnumerable<Tuple<TestNuGetProject, PackageIdentity>> lhs,
            IEnumerable<Tuple<TestNuGetProject, PackageIdentity>> rhs)
        {
            var ok = true;
            ok &= RhsContainsAllLhs(lhs, rhs);
            ok &= RhsContainsAllLhs(rhs, lhs);
            return ok;
        }

        private static bool RhsContainsAllLhs(
            IEnumerable<Tuple<TestNuGetProject, PackageIdentity>> lhs,
            IEnumerable<Tuple<TestNuGetProject, PackageIdentity>> rhs)
        {
            foreach (var item in lhs)
            {
                if (!rhs.Contains(item, new PreviewResultComparer()))
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

        private class PreviewResultComparer : IEqualityComparer<Tuple<TestNuGetProject, PackageIdentity>>
        {
            public bool Equals(Tuple<TestNuGetProject, PackageIdentity> x, Tuple<TestNuGetProject, PackageIdentity> y)
            {
                var f1 = x.Item1.Metadata[NuGetProjectMetadataKeys.Name].ToString().Equals(
                    y.Item1.Metadata[NuGetProjectMetadataKeys.Name].ToString());
                var f2 = x.Item2.Equals(y.Item2);
                return f1 && f2;
            }

            public int GetHashCode(Tuple<TestNuGetProject, PackageIdentity> obj)
            {
                return obj.GetHashCode();
            }
        }

#if IS_DESKTOP
        private class TestNuGetVSTelemetryService : NuGetVSTelemetryService
        {
            private ITelemetrySession _telemetrySession;
            private XunitLogger _logger;

            public TestNuGetVSTelemetryService(ITelemetrySession telemetrySession, XunitLogger logger)
            {
                _telemetrySession = telemetrySession ?? throw new ArgumentNullException(nameof(telemetrySession));
                _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            }

            public override void EmitTelemetryEvent(TelemetryEvent telemetryData)
            {
                if (telemetryData == null)
                {
                    throw new ArgumentNullException(nameof(telemetryData));
                }

                lock (_logger)
                {
                    var operationId = telemetryData["OperationId"];
                    var parentId = telemetryData["ParentId"];

                    _logger.LogInformation("--------------------------");
                    _logger.LogInformation($"Name: {telemetryData.Name}");
                    _logger.LogInformation($"OperationId: {operationId}");
                    _logger.LogInformation($"ParentId: {parentId}");
                    _logger.LogInformation($"Json: {telemetryData.ToJson()}");
                    _logger.LogInformation($"Stack: {Environment.StackTrace}");
                    _logger.LogInformation("--------------------------");
                }

                _telemetrySession.PostEvent(telemetryData);
            }
        }
#endif
    }
}
