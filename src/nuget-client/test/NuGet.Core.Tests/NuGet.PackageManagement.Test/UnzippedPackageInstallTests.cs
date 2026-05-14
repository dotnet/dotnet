// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.PackageManagement;
using NuGet.Packaging.Core;
using NuGet.ProjectManagement;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Test.Utility;
using Xunit;

namespace NuGet.Test
{
    public class UnzippedPackageInstallTests
    {
        private readonly List<PackageIdentity> NoDependencyLibPackages = new List<PackageIdentity>
            {
                new PackageIdentity("Microsoft.AspNet.Razor", new NuGetVersion("2.0.30506")),
                new PackageIdentity("Microsoft.AspNet.Razor", new NuGetVersion("3.0.0")),
                new PackageIdentity("Microsoft.AspNet.Razor", new NuGetVersion("3.2.0-rc")),
                new PackageIdentity("Antlr", new NuGetVersion("3.5.0.2"))
            };

        [Fact]
        public async Task UnzippedPackageInstall_Basic()
        {
            // Arrange
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV3OnlySourceRepositoryProvider();

            using (var testSolutionManager = new TestSolutionManager())
            using (var randomPackagesConfigFolderPath = TestDirectory.Create())
            {
                var testSettings = NullSettings.Instance;
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    testSettings,
                    testSolutionManager,
                    deleteOnRestartManager);

                var packagesFolderPath = PackagesFolderPathUtility.GetPackagesFolderPath(testSolutionManager, testSettings);

                var randomPackagesConfigPath = Path.Combine(randomPackagesConfigFolderPath, "packages.config");
                var token = CancellationToken.None;

                var projectTargetFramework = NuGetFramework.Parse("net45");
                var msBuildNuGetProjectSystem = new TestMSBuildNuGetProjectSystem(projectTargetFramework, new TestNuGetProjectContext());
                var msBuildNuGetProject = new MSBuildNuGetProject(msBuildNuGetProjectSystem, packagesFolderPath, randomPackagesConfigFolderPath);
                var packageIdentity = NoDependencyLibPackages[0];

                // Pre-Assert
                // Check that the packages.config file does not exist
                Assert.False(File.Exists(randomPackagesConfigPath));
                // Check that there are no packages returned by PackagesConfigProject
                var packagesInPackagesConfig = (await msBuildNuGetProject.PackagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(0, packagesInPackagesConfig.Count);
                Assert.Equal(0, msBuildNuGetProjectSystem.References.Count);

                // Act
                await nuGetPackageManager.InstallPackageAsync(msBuildNuGetProject, packageIdentity,
                    new ResolutionContext(), new TestNuGetProjectContext(), sourceRepositoryProvider.GetRepositories().First(), null, token);

                // Assert
                // Check that the packages.config file exists after the installation
                Assert.True(File.Exists(randomPackagesConfigPath));
                // Check the number of packages and packages returned by PackagesConfigProject after the installation
                packagesInPackagesConfig = (await msBuildNuGetProject.PackagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(1, packagesInPackagesConfig.Count);
                Assert.Equal(packageIdentity, packagesInPackagesConfig[0].PackageIdentity);
                Assert.Equal(projectTargetFramework, packagesInPackagesConfig[0].TargetFramework);
            }
        }
    }
}
