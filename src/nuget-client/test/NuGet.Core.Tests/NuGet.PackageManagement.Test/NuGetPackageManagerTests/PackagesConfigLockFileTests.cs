// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Configuration;
using NuGet.ProjectModel;
using NuGet.Test.Utility;
using Test.Utility;
using Xunit;

namespace NuGet.PackageManagement.Test.NuGetPackageManagerTests
{
    public class PackagesConfigLockFileTests
    {
        [Fact]
        public async Task DoNotCreateLockFileWhenFeatureDisabled()
        {
            // Arrange
            using (var packageSource = TestDirectory.Create())
            using (var testSolutionManager = new TestSolutionManager())
            {
                var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(
                    new List<PackageSource>()
                    {
                        new PackageSource(packageSource.Path)
                    });

                var testSettings = NullSettings.Instance;
                var token = CancellationToken.None;
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    testSettings,
                    testSolutionManager,
                    deleteOnRestartManager);
                var packagesFolderPath = PackagesFolderPathUtility.GetPackagesFolderPath(testSolutionManager, testSettings);

                var packageContext = new SimpleTestPackageContext("packageA");
                packageContext.AddFile("lib/net45/a.dll");
                SimpleTestPackageUtility.CreateOPCPackage(packageContext, packageSource);

                var msBuildNuGetProject = testSolutionManager.AddNewMSBuildProject();
                var msBuildNuGetProjectSystem = msBuildNuGetProject.ProjectSystem as TestMSBuildNuGetProjectSystem;
                msBuildNuGetProjectSystem.SetPropertyValue("RestorePackagesWithLockFile", "false");
                var packagesConfigPath = msBuildNuGetProject.PackagesConfigNuGetProject.FullPath;
                var packagesLockPath = packagesConfigPath.Replace("packages.config", "packages.lock.json");
                var packageIdentity = packageContext.Identity;

                // Pre-Assert
                // Check that the packages.lock.json file does not exist
                Assert.False(File.Exists(packagesLockPath));

                // Act
                await nuGetPackageManager.InstallPackageAsync(msBuildNuGetProject, packageIdentity,
                    new ResolutionContext(), new TestNuGetProjectContext(), sourceRepositoryProvider.GetRepositories().First(), null, token);

                // Assert
                // Check that the packages.lock.json still does not exist after the installation
                Assert.False(File.Exists(packagesLockPath));
            }
        }

        [Fact]
        public async Task InstallingPackageShouldCreateLockFile_PackagesLockJson()
        {
            // Arrange
            using (var packageSource = TestDirectory.Create())
            using (var testSolutionManager = new TestSolutionManager())
            {
                var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(
                    new List<PackageSource>()
                    {
                        new PackageSource(packageSource.Path)
                    });

                var testSettings = NullSettings.Instance;
                var token = CancellationToken.None;
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    testSettings,
                    testSolutionManager,
                    deleteOnRestartManager);
                var packagesFolderPath = PackagesFolderPathUtility.GetPackagesFolderPath(testSolutionManager, testSettings);

                var packageContext = new SimpleTestPackageContext("packageA");
                packageContext.AddFile("lib/net45/a.dll");
                SimpleTestPackageUtility.CreateOPCPackage(packageContext, packageSource);

                var msBuildNuGetProject = testSolutionManager.AddNewMSBuildProject();
                var msBuildNuGetProjectSystem = msBuildNuGetProject.ProjectSystem as TestMSBuildNuGetProjectSystem;
                msBuildNuGetProjectSystem.SetPropertyValue("RestorePackagesWithLockFile", "true");
                var packagesConfigPath = msBuildNuGetProject.PackagesConfigNuGetProject.FullPath;
                var packagesLockPath = packagesConfigPath.Replace("packages.config", "packages.lock.json");
                var packageIdentity = packageContext.Identity;

                // Pre-Assert
                // Check that the packages.lock.json file does not exist
                Assert.False(File.Exists(packagesLockPath));

                // Act
                await nuGetPackageManager.InstallPackageAsync(msBuildNuGetProject, packageIdentity,
                    new ResolutionContext(), new TestNuGetProjectContext(), sourceRepositoryProvider.GetRepositories().First(), null, token);

                // Assert
                // Check that the packages.lock.json file exists after the installation
                Assert.True(File.Exists(packagesLockPath));
                Assert.True(msBuildNuGetProjectSystem.FileExistsInProject("packages.lock.json"));
                // Check the number of target frameworks and dependencies in the lock file
                var lockFile = PackagesLockFileFormat.Read(packagesLockPath);
                Assert.Equal(1, lockFile.Targets.Count);
                Assert.Equal(1, lockFile.Targets[0].Dependencies.Count);
            }
        }

        [Fact]
        public async Task InstallingPackageShouldCreateLockFile_CustomLockFileName()
        {
            // Arrange
            using (var packageSource = TestDirectory.Create())
            using (var testSolutionManager = new TestSolutionManager())
            {
                var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(
                    new List<PackageSource>()
                    {
                        new PackageSource(packageSource.Path)
                    });

                var testSettings = NullSettings.Instance;
                var token = CancellationToken.None;
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    testSettings,
                    testSolutionManager,
                    deleteOnRestartManager);
                var packagesFolderPath = PackagesFolderPathUtility.GetPackagesFolderPath(testSolutionManager, testSettings);

                var packageContext = new SimpleTestPackageContext("packageA");
                packageContext.AddFile("lib/net45/a.dll");
                SimpleTestPackageUtility.CreateOPCPackage(packageContext, packageSource);

                var msBuildNuGetProject = testSolutionManager.AddNewMSBuildProject();
                var msBuildNuGetProjectSystem = msBuildNuGetProject.ProjectSystem as TestMSBuildNuGetProjectSystem;
                msBuildNuGetProjectSystem.SetPropertyValue("RestorePackagesWithLockFile", "true");
                msBuildNuGetProjectSystem.SetPropertyValue("NuGetLockFilePath", "my.lock.json");
                var packagesConfigPath = msBuildNuGetProject.PackagesConfigNuGetProject.FullPath;
                var packagesLockPath = packagesConfigPath.Replace("packages.config", "my.lock.json");
                var packageIdentity = packageContext.Identity;

                // Pre-Assert
                // Check that the packages.lock.json file does not exist
                Assert.False(File.Exists(packagesLockPath));

                // Act
                await nuGetPackageManager.InstallPackageAsync(msBuildNuGetProject, packageIdentity,
                    new ResolutionContext(), new TestNuGetProjectContext(), sourceRepositoryProvider.GetRepositories().First(), null, token);

                // Assert
                // Check that the packages.lock.json file exists after the installation
                Assert.True(File.Exists(packagesLockPath));
                Assert.True(msBuildNuGetProjectSystem.FileExistsInProject("my.lock.json"));
                // Check the number of target frameworks and dependencies in the lock file
                var lockFile = PackagesLockFileFormat.Read(packagesLockPath);
                Assert.Equal(1, lockFile.Targets.Count);
                Assert.Equal(1, lockFile.Targets[0].Dependencies.Count);
            }
        }
    }
}
