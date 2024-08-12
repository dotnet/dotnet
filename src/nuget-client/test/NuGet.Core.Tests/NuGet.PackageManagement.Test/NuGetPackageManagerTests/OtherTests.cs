// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.ProjectManagement;
using NuGet.ProjectManagement.Projects;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Test.Utility;
using Xunit;
using Xunit.Abstractions;

namespace NuGet.PackageManagement.Test.NuGetPackageManagerTests
{
    public class OtherTests
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


        private readonly XunitLogger _logger;

        public OtherTests(ITestOutputHelper output)
        {
            _logger = new XunitLogger(output);
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

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task GetInstalledPackagesByDependencyOrderInternal(bool deletePackages)
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
        public async Task OpenReadmeFile()
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
                Assert.Equal(Path.Combine(packagePathResolver.GetInstallPath(packageIdentity), "ReadMe.txt"), testNuGetProjectContext.TestExecutionContext.FilesOpened.First(), ignoreCase: true);
            }
        }

        [Fact]
        public async Task GetLatestVersion_GatherCache()
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
                        new PackageDependency("b", bVersionRange)
                    },
                    listed: true,
                    source: null),
            };

            var resourceProviders = new List<Lazy<INuGetResourceProvider>>();
            resourceProviders.Add(new Lazy<INuGetResourceProvider>(() => new TestDependencyInfoProvider(packages)));
            resourceProviders.Add(new Lazy<INuGetResourceProvider>(() => new TestMetadataProvider(packages)));

            var packageSource = new PackageSource("http://a");
            var packageSourceProvider = new TestPackageSourceProvider(new[] { packageSource });

            var sourceRepositoryProvider = new SourceRepositoryProvider(packageSourceProvider, resourceProviders);
            var resolutionContext = new ResolutionContext();

            // Act
            var latestVersion = await NuGetPackageManager.GetLatestVersionAsync(
                "a",
                NuGetFramework.AnyFramework,
                resolutionContext,
                sourceRepositoryProvider.GetRepositories().First(),
                NullLogger.Instance,
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

        [Fact(Skip = "https://github.com/NuGet/Home/issues/13185")]
        public async Task ExecuteNuGetProjectActionsAsync_MultipleBuildIntegratedProjects()
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
        public async Task ExecuteNuGetProjectActionsAsync_MixedProjects()
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
    }
}
