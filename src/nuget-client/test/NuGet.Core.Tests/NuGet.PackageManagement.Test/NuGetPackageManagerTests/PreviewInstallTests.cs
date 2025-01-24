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
using Newtonsoft.Json.Linq;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.ProjectManagement;
using NuGet.ProjectManagement.Projects;
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
    public class PreviewInstallTests
    {
        // Following are the various sets of packages that are small in size. To be used by the functional tests
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

        public PreviewInstallTests(ITestOutputHelper output)
        {
            _logger = new XunitLogger(output);
        }

        [Fact]
        public async Task PreviewInstallOrderOfDependencies()
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
        public async Task PreviewInstallMvcPackageWithPrereleaseFlagFalse()
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
        public async Task PreviewUninstallDependencyPackage()
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
        public async Task PreviewUpdatePackageFollowingForceUninstall()
        {
            // Arrange

            // Set up Package Source
            var packages = new List<SourcePackageDependencyInfo>
            {
                new SourcePackageDependencyInfo("a", new NuGetVersion(1, 0, 0), new PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("a", new NuGetVersion(2, 0, 0), new PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("a", new NuGetVersion(3, 0, 0), new PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("b", new NuGetVersion(1, 0, 0), new[] { new PackageDependency("a", new VersionRange(new NuGetVersion(1, 0, 0))) }, true, null),
                new SourcePackageDependencyInfo("b", new NuGetVersion(2, 0, 0), new[] { new PackageDependency("a", new VersionRange(new NuGetVersion(2, 0, 0))) }, true, null),
                new SourcePackageDependencyInfo("b", new NuGetVersion(3, 0, 0), new[] { new PackageDependency("a", new VersionRange(new NuGetVersion(2, 0, 0))) }, true, null),
                new SourcePackageDependencyInfo("c", new NuGetVersion(1, 0, 0), new PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("c", new NuGetVersion(2, 0, 0), new PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("c", new NuGetVersion(3, 0, 0), new PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("d", new NuGetVersion(1, 0, 0), new[] { new PackageDependency("e", new VersionRange(new NuGetVersion(1, 0, 0))) }, true, null),
                new SourcePackageDependencyInfo("d", new NuGetVersion(2, 0, 0), new[] { new PackageDependency("e", new VersionRange(new NuGetVersion(1, 0, 0))) }, true, null),
                new SourcePackageDependencyInfo("d", new NuGetVersion(3, 0, 0), new[] { new PackageDependency("e", new VersionRange(new NuGetVersion(1, 0, 0))) }, true, null),
                new SourcePackageDependencyInfo("d", new NuGetVersion(4, 0, 0), new[] { new PackageDependency("e", new VersionRange(new NuGetVersion(1, 0, 0))) }, true, null),
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
        public async Task PreviewUninstallWithRemoveDependencies()
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
        public async Task PreviewInstallPackageWithDeepDependency()
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
        public async Task PreviewUninstallPackageWithDeepDependency()
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

        [Fact]
        public async Task PreviewUpdatePackagesSimple()
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
                    NullLogger.Instance,
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
        public async Task PreviewUpdatePackageWithTargetPrereleaseInProject()
        {
            // Arrange

            // Set up Package Source
            var packages = new List<SourcePackageDependencyInfo>
            {
                new SourcePackageDependencyInfo("a", new NuGetVersion(1, 0, 0), new PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("a", new NuGetVersion(2, 0, 0), new PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("a", new NuGetVersion(3, 0, 0), new PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("b", new NuGetVersion(1, 0, 0, "beta"), new[] { new PackageDependency("a", new VersionRange(new NuGetVersion(1, 0, 0))) }, true, null),
                new SourcePackageDependencyInfo("b", new NuGetVersion(2, 0, 0, "beta"), new[] { new PackageDependency("a", new VersionRange(new NuGetVersion(1, 0, 0))) }, true, null),
            };

            var sourceRepositoryProvider = CreateSource(packages);

            // Set up NuGetProject

            var fwk45 = NuGetFramework.Parse("net45");

            var installedPackages = new List<PackageReference>
            {
                new PackageReference(new PackageIdentity("a", new NuGetVersion(1, 0, 0)), fwk45, true),
                new PackageReference(new PackageIdentity("b", new NuGetVersion(1, 0, 0, "beta")), fwk45, true),
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
        public async Task PreviewUpdatePackageNotExistsInProject()
        {
            // Arrange

            // Set up Package Source
            var packages = new List<SourcePackageDependencyInfo>
            {
                new SourcePackageDependencyInfo("a", new NuGetVersion(1, 0, 0), new PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("a", new NuGetVersion(2, 0, 0), new PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("a", new NuGetVersion(3, 0, 0), new PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("b", new NuGetVersion(1, 0, 0, "beta"), new[] { new PackageDependency("a", new VersionRange(new NuGetVersion(1, 0, 0))) }, true, null),
                new SourcePackageDependencyInfo("b", new NuGetVersion(2, 0, 0, "beta"), new[] { new PackageDependency("a", new VersionRange(new NuGetVersion(1, 0, 0))) }, true, null),
            };

            var sourceRepositoryProvider = CreateSource(packages);

            // Set up NuGetProject

            var fwk45 = NuGetFramework.Parse("net45");

            var installedPackages = new List<PackageReference>
            {
                new PackageReference(new PackageIdentity("a", new NuGetVersion(1, 0, 0)), fwk45, true),
                new PackageReference(new PackageIdentity("b", new NuGetVersion(1, 0, 0, "beta")), fwk45, true),
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
        public async Task PreviewUpdatePackageALLPrereleaseInProject()
        {
            // Arrange

            // Set up Package Source
            var packages = new List<SourcePackageDependencyInfo>
            {
                new SourcePackageDependencyInfo("a", new NuGetVersion(1, 0, 0), new PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("a", new NuGetVersion(2, 0, 0), new PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("a", new NuGetVersion(3, 0, 0), new PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("b", new NuGetVersion(1, 0, 0, "beta"), new[] { new PackageDependency("a", new VersionRange(new NuGetVersion(1, 0, 0))) }, true, null),
                new SourcePackageDependencyInfo("b", new NuGetVersion(2, 0, 0, "beta"), new[] { new PackageDependency("a", new VersionRange(new NuGetVersion(1, 0, 0))) }, true, null),
            };

            var sourceRepositoryProvider = CreateSource(packages);

            // Set up NuGetProject
            var fwk45 = NuGetFramework.Parse("net45");

            var installedPackages = new List<PackageReference>
            {
                new PackageReference(new PackageIdentity("a", new NuGetVersion(1, 0, 0)), fwk45, true),
                new PackageReference(new PackageIdentity("b", new NuGetVersion(1, 0, 0, "beta")), fwk45, true),
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
        public async Task PreviewUpdatePrereleasePackageNoPreFlagSpecified()
        {
            // Arrange

            // Set up Package Source
            var packages = new List<SourcePackageDependencyInfo>
            {
                new SourcePackageDependencyInfo("a", new NuGetVersion(1, 0, 0), new PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("a", new NuGetVersion(2, 0, 0), new PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("a", new NuGetVersion(3, 0, 0), new PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("b", new NuGetVersion(1, 0, 0, "beta"), new[] { new PackageDependency("a", new VersionRange(new NuGetVersion(1, 0, 0))) }, true, null),
                new SourcePackageDependencyInfo("b", new NuGetVersion(2, 0, 0, "beta"), new[] { new PackageDependency("a", new VersionRange(new NuGetVersion(1, 0, 0))) }, true, null),
            };

            var sourceRepositoryProvider = CreateSource(packages);

            // Set up NuGetProject
            var fwk45 = NuGetFramework.Parse("net45");

            var installedPackages = new List<PackageReference>
            {
                new PackageReference(new PackageIdentity("a", new NuGetVersion(1, 0, 0)), fwk45, true),
                new PackageReference(new PackageIdentity("b", new NuGetVersion(1, 0, 0, "beta")), fwk45, true),
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
        public async Task PreviewUpdateMulti()
        {
            // Arrange

            // Set up Package Source
            var packages = new List<SourcePackageDependencyInfo>
            {
                new SourcePackageDependencyInfo("a", new NuGetVersion(1, 0, 0), new PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("a", new NuGetVersion(2, 0, 0), new PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("a", new NuGetVersion(3, 0, 0), new PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("b", new NuGetVersion(1, 0, 0), new[] { new PackageDependency("a", new VersionRange(new NuGetVersion(1, 0, 0))) }, true, null),
                new SourcePackageDependencyInfo("b", new NuGetVersion(2, 0, 0), new[] { new PackageDependency("a", new VersionRange(new NuGetVersion(2, 0, 0))) }, true, null),
                new SourcePackageDependencyInfo("b", new NuGetVersion(3, 0, 0), new[] { new PackageDependency("a", new VersionRange(new NuGetVersion(2, 0, 0))) }, true, null),
                new SourcePackageDependencyInfo("c", new NuGetVersion(1, 0, 0), new PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("c", new NuGetVersion(2, 0, 0), new PackageDependency[] { }, true, null),
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
        public async Task PreviewUpdatePackagesAsync_MultiProjects()
        {
            // Arrange

            // Set up Package Source
            var packages = new List<SourcePackageDependencyInfo>
            {
                new SourcePackageDependencyInfo("a", new NuGetVersion(1, 0, 0), new PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("a", new NuGetVersion(2, 0, 0), new PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("b", new NuGetVersion(1, 0, 0), new[] { new PackageDependency("a", new VersionRange(new NuGetVersion(1, 0, 0))) }, true, null),
                new SourcePackageDependencyInfo("b", new NuGetVersion(2, 0, 0), new[] { new PackageDependency("a", new VersionRange(new NuGetVersion(2, 0, 0))) }, true, null),
                new SourcePackageDependencyInfo("c", new NuGetVersion(1, 0, 0), new PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("c", new NuGetVersion(2, 0, 0), new PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("d", new NuGetVersion(1, 0, 0), new[] { new PackageDependency("c", new VersionRange(new NuGetVersion(1, 0, 0))) }, true, null),
                new SourcePackageDependencyInfo("d", new NuGetVersion(2, 0, 0), new[] { new PackageDependency("c", new VersionRange(new NuGetVersion(2, 0, 0))) }, true, null),
            };

            var sourceRepositoryProvider = CreateSource(packages);

            // Set up NuGetProject
            var fwk45 = NuGetFramework.Parse("net45");

            var installedPackagesA = new List<PackageReference>
            {
                new PackageReference(new PackageIdentity("a", new NuGetVersion(1, 0, 0)), fwk45, true),
                new PackageReference(new PackageIdentity("b", new NuGetVersion(1, 0, 0)), fwk45, true),
            };

            var installedPackagesB = new List<PackageReference>
            {
                new PackageReference(new PackageIdentity("d", new NuGetVersion(1, 0, 0)), fwk45, true),
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
        public async Task PreviewUpdatePackagesAsync_MultiProjects_MultiDependencies()
        {
            // Arrange

            // Set up Package Source
            var packages = new List<SourcePackageDependencyInfo>
            {
                new SourcePackageDependencyInfo("a", new NuGetVersion(1, 0, 0), new PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("a", new NuGetVersion(2, 0, 0), new PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("b", new NuGetVersion(1, 0, 0), new[] { new PackageDependency("a", new VersionRange(new NuGetVersion(1, 0, 0))) }, true, null),
                new SourcePackageDependencyInfo("b", new NuGetVersion(2, 0, 0), new[] { new PackageDependency("a", new VersionRange(new NuGetVersion(2, 0, 0))) }, true, null),
                new SourcePackageDependencyInfo("c", new NuGetVersion(1, 0, 0), new PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("c", new NuGetVersion(2, 0, 0), new PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("d", new NuGetVersion(1, 0, 0), new[] { new PackageDependency("c", new VersionRange(new NuGetVersion(1, 0, 0))) }, true, null),
                new SourcePackageDependencyInfo("d", new NuGetVersion(2, 0, 0), new[] { new PackageDependency("c", new VersionRange(new NuGetVersion(2, 0, 0))) }, true, null),
            };

            var sourceRepositoryProvider = CreateSource(packages);

            // Set up NuGetProject
            var fwk45 = NuGetFramework.Parse("net45");

            var installedPackagesA = new List<PackageReference>
            {
                new PackageReference(new PackageIdentity("a", new NuGetVersion(1, 0, 0)), fwk45, true),
                new PackageReference(new PackageIdentity("b", new NuGetVersion(1, 0, 0)), fwk45, true),
            };

            var installedPackagesB = new List<PackageReference>
            {
                new PackageReference(new PackageIdentity("c", new NuGetVersion(1, 0, 0)), fwk45, true),
                new PackageReference(new PackageIdentity("d", new NuGetVersion(1, 0, 0)), fwk45, true),
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
        public async Task PreviewInstallPackageFollowingForceUninstall()
        {
            // Arrange

            // Set up Package Source
            var packages = new List<SourcePackageDependencyInfo>
            {
                new SourcePackageDependencyInfo("a", new NuGetVersion(1, 0, 0), new PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("a", new NuGetVersion(2, 0, 0), new PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("a", new NuGetVersion(3, 0, 0), new PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("b", new NuGetVersion(1, 0, 0), new[] { new PackageDependency("a", new VersionRange(new NuGetVersion(1, 0, 0))) }, true, null),
                new SourcePackageDependencyInfo("b", new NuGetVersion(2, 0, 0), new[] { new PackageDependency("a", new VersionRange(new NuGetVersion(2, 0, 0))) }, true, null),
                new SourcePackageDependencyInfo("b", new NuGetVersion(3, 0, 0), new[] { new PackageDependency("a", new VersionRange(new NuGetVersion(2, 0, 0))) }, true, null),
                new SourcePackageDependencyInfo("c", new NuGetVersion(1, 0, 0), new PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("c", new NuGetVersion(2, 0, 0), new PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("c", new NuGetVersion(3, 0, 0), new PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("d", new NuGetVersion(1, 0, 0), new[] { new PackageDependency("e", new VersionRange(new NuGetVersion(1, 0, 0))) }, true, null),
                new SourcePackageDependencyInfo("d", new NuGetVersion(2, 0, 0), new[] { new PackageDependency("e", new VersionRange(new NuGetVersion(1, 0, 0))) }, true, null),
                new SourcePackageDependencyInfo("d", new NuGetVersion(3, 0, 0), new[] { new PackageDependency("e", new VersionRange(new NuGetVersion(1, 0, 0))) }, true, null),
                new SourcePackageDependencyInfo("d", new NuGetVersion(4, 0, 0), new[] { new PackageDependency("e", new VersionRange(new NuGetVersion(1, 0, 0))) }, true, null),
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
        public async Task PreviewInstallPackageWithNonTargetDependency()
        {
            // Arrange

            // Set up Package Source
            var packages = new List<SourcePackageDependencyInfo>
            {
                new SourcePackageDependencyInfo("a", new NuGetVersion(1, 0, 0), new[] { new PackageDependency("b", new VersionRange(new NuGetVersion(1, 0, 0), true, new NuGetVersion(1, 0, 0), true)) }, true, null),
                new SourcePackageDependencyInfo("a", new NuGetVersion(2, 0, 0), new[] { new PackageDependency("b", new VersionRange(new NuGetVersion(2, 0, 0), true, new NuGetVersion(2, 0, 0), true)),
                    new PackageDependency("c", new VersionRange(new NuGetVersion(2, 0, 0), true, new NuGetVersion(2, 0, 0), true))}, true, null),
                new SourcePackageDependencyInfo("b", new NuGetVersion(1, 0, 0), new[] { new PackageDependency("d", new VersionRange(new NuGetVersion(1, 0, 0), true, new NuGetVersion(1, 0, 0), true)) }, true, null),
                new SourcePackageDependencyInfo("b", new NuGetVersion(2, 0, 0), new[] { new PackageDependency("d", new VersionRange(new NuGetVersion(2, 0, 0), true, new NuGetVersion(2, 0, 0), true)) }, true, null),
                new SourcePackageDependencyInfo("c", new NuGetVersion(1, 0, 0), new PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("c", new NuGetVersion(2, 0, 0), new PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("d", new NuGetVersion(1, 0, 0), new[] { new PackageDependency("e", new VersionRange(new NuGetVersion(1, 0, 0), true, new NuGetVersion(1, 0, 0), true)) }, true, null),
                new SourcePackageDependencyInfo("d", new NuGetVersion(2, 0, 0), new[] { new PackageDependency("e", new VersionRange(new NuGetVersion(2, 0, 0), true, new NuGetVersion(2, 0, 0), true)) }, true, null),
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
                new PackageReference(new PackageIdentity("d", new NuGetVersion(1, 0, 0)), fwk45, true),
                new PackageReference(new PackageIdentity("e", new NuGetVersion(1, 0, 0)), fwk45, true),
                new PackageReference(new PackageIdentity("f", new NuGetVersion(1, 0, 0)), fwk45, true),
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
        public async Task PreviewUpdateMultiWithConflict()
        {
            // Arrange

            // Set up Package Source
            var packages = new List<SourcePackageDependencyInfo>
            {
                new SourcePackageDependencyInfo("a", new NuGetVersion(1, 0, 0), new PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("a", new NuGetVersion(2, 0, 0), new PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("a", new NuGetVersion(3, 0, 0), new PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("b", new NuGetVersion(1, 0, 0), new[] { new PackageDependency("a", new VersionRange(new NuGetVersion(1, 0, 0), true, new NuGetVersion(1, 0, 0), true)) }, true, null),
                new SourcePackageDependencyInfo("b", new NuGetVersion(2, 0, 0), new[] { new PackageDependency("a", new VersionRange(new NuGetVersion(2, 0, 0), true, new NuGetVersion(2, 0, 0), true)) }, true, null),
                new SourcePackageDependencyInfo("b", new NuGetVersion(3, 0, 0), new[] { new PackageDependency("a", new VersionRange(new NuGetVersion(3, 0, 0), true, new NuGetVersion(3, 0, 0), true)) }, true, null),
                new SourcePackageDependencyInfo("c", new NuGetVersion(1, 0, 0), new PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("c", new NuGetVersion(2, 0, 0), new PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("c", new NuGetVersion(3, 0, 0), new PackageDependency[] { }, true, null),
            };

            var sourceRepositoryProvider = CreateSource(packages);

            // Set up NuGetProject
            var fwk45 = NuGetFramework.Parse("net45");

            var installedPackages = new List<PackageReference>
            {
                new PackageReference(new PackageIdentity("a", new NuGetVersion(1, 0, 0)), fwk45, true),
                new PackageReference(new PackageIdentity("b", new NuGetVersion(1, 0, 0)), fwk45, true),
                new PackageReference(new PackageIdentity("c", new NuGetVersion(2, 0, 0)), fwk45, true),
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
                    Assert.IsType<InvalidOperationException>(e);
                }
            }
        }

        [Fact]
        public async Task PreviewUpdateMultiWithDowngradeConflict()
        {
            // Arrange

            // Set up Package Source
            var packages = new List<SourcePackageDependencyInfo>
            {
                new SourcePackageDependencyInfo("a", new NuGetVersion(1, 0, 0), new PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("a", new NuGetVersion(2, 0, 0), new PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("a", new NuGetVersion(3, 0, 0), new PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("b", new NuGetVersion(1, 0, 0), new[] { new PackageDependency("a", new VersionRange(new NuGetVersion(1, 0, 0), true, new NuGetVersion(1, 0, 0), true)) }, true, null),
                new SourcePackageDependencyInfo("b", new NuGetVersion(2, 0, 0), new[] { new PackageDependency("a", new VersionRange(new NuGetVersion(2, 0, 0), true, new NuGetVersion(3, 0, 0), true)) }, true, null),
                new SourcePackageDependencyInfo("b", new NuGetVersion(3, 0, 0), new[] { new PackageDependency("a", new VersionRange(new NuGetVersion(2, 0, 0), true, new NuGetVersion(3, 0, 0), false)) }, true, null),
                new SourcePackageDependencyInfo("c", new NuGetVersion(1, 0, 0), new PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("c", new NuGetVersion(2, 0, 0), new PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("c", new NuGetVersion(3, 0, 0), new PackageDependency[] { }, true, null),
            };

            var sourceRepositoryProvider = CreateSource(packages);

            // Set up NuGetProject
            var fwk45 = NuGetFramework.Parse("net45");

            var installedPackages = new List<PackageReference>
            {
                new PackageReference(new PackageIdentity("a", new NuGetVersion(2, 0, 0)), fwk45, true),
                new PackageReference(new PackageIdentity("b", new NuGetVersion(3, 0, 0)), fwk45, true),
                new PackageReference(new PackageIdentity("c", new NuGetVersion(2, 0, 0)), fwk45, true),
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
                    Assert.IsType<InvalidOperationException>(e);
                }
            }
        }

        [Fact(Skip = "This test performs update but verifies for a specific version. This is not going to work as newer versions are uploaded.")]
        public async Task PreviewUpdatePackages()
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
        public async Task PreviewReinstallPackages()
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
        public async Task PreviewInstallPackageIdUnexpectedDowngrade()
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
        public async Task PreviewInstallPackageThrowsDependencyDowngrade()
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
        public async Task PreviewInstallDependencyVersionHighestAndPrerelease()
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
                    NullLogger.Instance,
                    CancellationToken.None);

                var newtonsoftJsonPackageIdentity = new PackageIdentity(newtonsoftJsonPackageId, resolvedPackage.LatestVersion);

                var nuGetProjectActions = (await nuGetPackageManager.PreviewInstallPackageAsync(msBuildNuGetProject, dotnetrdfPackageIdentity, resolutionContext,
                    new TestNuGetProjectContext(), primarySourceRepository, null, CancellationToken.None)).ToList();

                // Assert
                Assert.Equal(4, nuGetProjectActions.Count);
                var newtonsoftJsonAction = nuGetProjectActions.FirstOrDefault(a => a.PackageIdentity.Equals(newtonsoftJsonPackageIdentity));

                Assert.NotNull(newtonsoftJsonAction);
            }
        }

        [Fact]
        public async Task PreviewInstallWithAllowedVersionsConstraint()
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
                var newtonsoftJsonPackageReference = installedPackages.FirstOrDefault(pr => pr.PackageIdentity.Equals(newtonsoftJsonPackageIdentity));

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
                newtonsoftJsonPackageReference = installedPackages.FirstOrDefault(pr => pr.PackageIdentity.Equals(newtonsoftJsonPackageIdentity));

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
        public async Task PreviewUpdateWithAllowedVersionsConstraintAsync()
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
            var newtonsoftJsonPackageReference = installedPackages.FirstOrDefault(pr => pr.PackageIdentity.Equals(newtonsoftJsonPackageIdentity));

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
            newtonsoftJsonPackageReference = installedPackages.FirstOrDefault(pr => pr.PackageIdentity.Equals(newtonsoftJsonPackageIdentity));

            Assert.NotNull(newtonsoftJsonPackageReference.AllowedVersions);

            // Main Act
            var nuGetProjectActions = (await nuGetPackageManager.PreviewUpdatePackagesAsync(
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
        public async Task PreviewUpdate_AllowedVersionsConstraint_RestrictHighestVersionAsync()
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
            var newtonsoftJsonPackageReference = installedPackages.FirstOrDefault(pr => pr.PackageIdentity.Equals(newJsonPackageIdentity));

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
            newtonsoftJsonPackageReference = installedPackages.FirstOrDefault(pr => pr.PackageIdentity.Equals(newJsonPackageIdentity));

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
        public async Task PreviewUpdateWithNoSource()
        {
            // Arrange
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(new List<PackageSource>());
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
        public async Task PreviewUpdatePackage_DeepDependencies()
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

        [Fact]
        public async Task PreviewUpdatePackage_IgnoreDependency()
        {
            // Arrange

            // Set up Package Source
            var packages = new List<SourcePackageDependencyInfo>
            {
                new SourcePackageDependencyInfo("a", new NuGetVersion(1, 0, 0), new[] { new PackageDependency("b", new VersionRange(new NuGetVersion(1, 0, 0))) }, true, null),
                new SourcePackageDependencyInfo("a", new NuGetVersion(2, 0, 0), new[] { new PackageDependency("b", new VersionRange(new NuGetVersion(2, 0, 0))),  new PackageDependency("c", new VersionRange(new NuGetVersion(1, 0, 0)))}, true, null),
                new SourcePackageDependencyInfo("b", new NuGetVersion(1, 0, 0), new PackageDependency[] { }, true, null)
            };

            var sourceRepositoryProvider = CreateSource(packages);

            // Set up NuGetProject
            var fwk45 = NuGetFramework.Parse("net45");

            var installedPackage1 = new PackageIdentity("a", new NuGetVersion(1, 0, 0));
            var installedPackage2 = new PackageIdentity("b", new NuGetVersion(1, 0, 0));

            var installedPackages = new List<PackageReference>
            {
                new PackageReference(installedPackage1, fwk45, true),
                new PackageReference(installedPackage2, fwk45, true)
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

        [Fact]
        public async Task PreviewInstallPackage_WithGlobalPackageFolder()
        {
            using (
                var packageSource1 = TestDirectory.Create())
            {
                // Arrange
                var sourceRepositoryProvider1 = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(
                    new List<PackageSource>()
                    {
                        new PackageSource(packageSource1.Path)
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
        public async Task PreviewUpdatePackage_UnlistedPackage()
        {
            // Arrange

            // Set up Package Source
            var packages = new List<SourcePackageDependencyInfo>
            {
                new SourcePackageDependencyInfo("a", new NuGetVersion(1, 0, 0), new PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("a", new NuGetVersion(2, 0, 0), new PackageDependency[] { }, false, null)
            };

            var sourceRepositoryProvider = CreateSource(packages);

            // Set up NuGetProject
            var fwk45 = NuGetFramework.Parse("net45");

            var installedPackage1 = new PackageIdentity("a", new NuGetVersion(1, 0, 0));

            var installedPackages = new List<PackageReference>
            {
                new PackageReference(installedPackage1, fwk45, true)
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
        public async Task PreviewInstallPackage_BuildIntegrated_MissingPath_Throws()
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
                var actions = new NuGetProjectAction[] { nugetAction };

                var nugetProjectActionsLookup =
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
                        newMappingID: null,
                        newMappingSource: null,
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
        public async Task PreviewInstallPackage_BuildIntegrated_NullVersion_Throws()
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

            var installedPackages = new List<PackageReference>
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
        public async Task BuildIntegratedProject_PreviewUpdatePackage()
        {
            // Arrange

            // Set up Package Source
            var packages = new List<SourcePackageDependencyInfo>
            {
                new SourcePackageDependencyInfo("a", new NuGetVersion(1, 0, 0), new PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("a", new NuGetVersion(2, 0, 0), new PackageDependency[] { }, true, null)
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
        public async Task MultipleBuildIntegratedProjects_PreviewUpdatePackage()
        {
            // This test was created after a multithreading bug was found. Like most multithreading bugs, it depends
            // very much on timing of exactly when different threads the relevant parts of the code, so it's difficult
            // to reproduce in a test.  Therefore, if this test fails randomly, it's not flaky, it's a product bug!
            // It's very unusual for a test to use Environment.ProcessorCount, but this is why.

            // Arrange

            // Set up Package Source
            var packages = new List<SourcePackageDependencyInfo>
            {
                new SourcePackageDependencyInfo("a", new NuGetVersion(1, 0, 0), new PackageDependency[] { }, true, null),
                new SourcePackageDependencyInfo("a", new NuGetVersion(2, 0, 0), new PackageDependency[] { }, true, null)
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
                for (var i = 0; i < projectCount; i++)
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
    }
}
