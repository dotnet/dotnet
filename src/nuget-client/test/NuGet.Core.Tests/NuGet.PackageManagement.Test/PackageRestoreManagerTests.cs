// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.PackageManagement;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Test.Utility;
using Xunit;

namespace NuGet.Test
{
    public class PackageRestoreManagerTests
    {
        private readonly List<PackageIdentity> Packages = new List<PackageIdentity>
            {
                new PackageIdentity("jQuery", new NuGetVersion("1.4.4")),
                new PackageIdentity("jQuery", new NuGetVersion("1.6.4")),
                new PackageIdentity("jQuery.Validation", new NuGetVersion("1.13.1")),
                new PackageIdentity("jQuery.UI.Combined", new NuGetVersion("1.11.2"))
            };

        [Fact]
        public async Task TestGetMissingPackagesForSolution()
        {
            // Arrange
            using (var testSolutionManager = new TestSolutionManager())
            using (var randomPackageSourcePath = TestDirectory.Create())
            {
                var projectA = testSolutionManager.AddNewMSBuildProject();
                var projectB = testSolutionManager.AddNewMSBuildProject();

                var packageIdentity = new PackageIdentity("packageA", new NuGetVersion("1.0.0"));
                var packageFileInfo = TestPackagesGroupedByFolder.GetLegacyTestPackage(randomPackageSourcePath,
                    packageIdentity.Id, packageIdentity.Version.ToNormalizedString());
                var testNuGetProjectContext = new TestNuGetProjectContext();
                var token = CancellationToken.None;

                using (var packageStream = GetDownloadResult(randomPackageSourcePath.Path, packageFileInfo))
                {
                    // Act
                    await projectA.InstallPackageAsync(packageIdentity, packageStream, testNuGetProjectContext, token);
                    await projectB.InstallPackageAsync(packageIdentity, packageStream, testNuGetProjectContext, token);
                }

                var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV3OnlySourceRepositoryProvider();
                var testSettings = Configuration.NullSettings.Instance;
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var packageRestoreManager = new PackageRestoreManager(
                    sourceRepositoryProvider,
                    testSettings,
                    testSolutionManager);

                // Act
                var packagesFromSolution = (await packageRestoreManager.GetPackagesInSolutionAsync(testSolutionManager.SolutionDirectory, token));

                var packagesFromSolutionList = packagesFromSolution.ToList();
                var missingPackagesFromSolutionList = packagesFromSolution.Where(p => p.IsMissing).ToList();

                Assert.Equal(1, packagesFromSolutionList.Count);
                Assert.Equal(0, missingPackagesFromSolutionList.Count);

                // Delete packages folder
                TestFileSystemUtility.DeleteRandomTestFolder(Path.Combine(testSolutionManager.SolutionDirectory, "packages"));

                packagesFromSolution = (await packageRestoreManager.GetPackagesInSolutionAsync(testSolutionManager.SolutionDirectory, token));

                packagesFromSolutionList = packagesFromSolution.ToList();
                missingPackagesFromSolutionList = packagesFromSolution.Where(p => p.IsMissing).ToList();

                Assert.Equal(1, missingPackagesFromSolutionList.Count);
            }
        }

        [Fact]
        public async Task TestGetMissingPackagesForSolution_NoPackagesInstalled()
        {
            // Arrange
            using (var testSolutionManager = new TestSolutionManager())
            {
                var projectA = testSolutionManager.AddNewMSBuildProject();
                var projectB = testSolutionManager.AddNewMSBuildProject();

                var testNuGetProjectContext = new TestNuGetProjectContext();
                var token = CancellationToken.None;

                var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV3OnlySourceRepositoryProvider();
                var testSettings = Configuration.NullSettings.Instance;
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var packageRestoreManager = new PackageRestoreManager(
                    sourceRepositoryProvider,
                    testSettings,
                    testSolutionManager);

                // Act
                var packagesFromSolution = (await packageRestoreManager.GetPackagesInSolutionAsync(testSolutionManager.SolutionDirectory, token));

                Assert.False(packagesFromSolution.Any());
            }
        }

        [Fact]
        public async Task TestPackageRestoredEvent()
        {
            // Arrange
            using (var testSolutionManager = new TestSolutionManager())
            {
                var projectA = testSolutionManager.AddNewMSBuildProject();
                var projectB = testSolutionManager.AddNewMSBuildProject();

                var packageIdentity = Packages[0];
                var testNuGetProjectContext = new TestNuGetProjectContext();
                var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV3OnlySourceRepositoryProvider();
                var testSettings = Configuration.NullSettings.Instance;
                var resolutionContext = new ResolutionContext();
                var token = CancellationToken.None;

                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    testSettings,
                    testSolutionManager,
                    deleteOnRestartManager);

                await nuGetPackageManager.InstallPackageAsync(projectA, packageIdentity,
                    resolutionContext, new TestNuGetProjectContext(), sourceRepositoryProvider.GetRepositories().First(), null, token);
                await nuGetPackageManager.InstallPackageAsync(projectB, packageIdentity,
                    resolutionContext, new TestNuGetProjectContext(), sourceRepositoryProvider.GetRepositories().First(), null, token);

                var packageRestoreManager = new PackageRestoreManager(
                    sourceRepositoryProvider,
                    testSettings,
                    testSolutionManager);
                var restoredPackages = new List<PackageIdentity>();
                packageRestoreManager.PackageRestoredEvent += delegate (object sender, PackageRestoredEventArgs args)
                    {
                        if (args.Restored)
                        {
                            restoredPackages.Add(args.Package);
                        }
                    };

                Assert.True(nuGetPackageManager.PackageExistsInPackagesFolder(packageIdentity));

                // Delete packages folder
                TestFileSystemUtility.DeleteRandomTestFolder(Path.Combine(testSolutionManager.SolutionDirectory, "packages"));

                Assert.False(nuGetPackageManager.PackageExistsInPackagesFolder((packageIdentity)));

                // Act
                await packageRestoreManager.RestoreMissingPackagesInSolutionAsync(testSolutionManager.SolutionDirectory,
                    testNuGetProjectContext,
                    new TestLogger(),
                    CancellationToken.None);

                Assert.Equal(1, restoredPackages.Count);
                Assert.True(nuGetPackageManager.PackageExistsInPackagesFolder((packageIdentity)));
            }
        }

        [Fact]
        public async Task TestCheckForMissingPackages()
        {
            // Arrange
            using (var testSolutionManager = new TestSolutionManager())
            using (var randomPackageSourcePath = TestDirectory.Create())
            {
                var projectA = testSolutionManager.AddNewMSBuildProject();
                var projectB = testSolutionManager.AddNewMSBuildProject();

                var packageIdentity = new PackageIdentity("packageA", new NuGetVersion("1.0.0"));
                var packageFileInfo = TestPackagesGroupedByFolder.GetLegacyTestPackage(randomPackageSourcePath,
                    packageIdentity.Id, packageIdentity.Version.ToNormalizedString());
                var testNuGetProjectContext = new TestNuGetProjectContext();
                var token = CancellationToken.None;

                using (var packageStream = GetDownloadResult(randomPackageSourcePath.Path, packageFileInfo))
                {
                    // Act
                    await projectA.InstallPackageAsync(packageIdentity, packageStream, testNuGetProjectContext, token);
                    await projectB.InstallPackageAsync(packageIdentity, packageStream, testNuGetProjectContext, token);
                }

                var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV3OnlySourceRepositoryProvider();
                var testSettings = Configuration.NullSettings.Instance;
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var packageRestoreManager = new PackageRestoreManager(
                    sourceRepositoryProvider,
                    testSettings,
                    testSolutionManager);

                var packagesMissingEventCount = 0;
                var packagesMissing = false;
                packageRestoreManager.PackagesMissingStatusChanged += delegate (object sender, PackagesMissingStatusEventArgs args)
                    {
                        packagesMissingEventCount++;
                        packagesMissing = args.PackagesMissing;
                    };

                // Act
                await packageRestoreManager.RaisePackagesMissingEventForSolutionAsync(testSolutionManager.SolutionDirectory, token);

                // Assert
                Assert.Equal(1, packagesMissingEventCount);
                Assert.False(packagesMissing);

                // Delete packages folder
                TestFileSystemUtility.DeleteRandomTestFolder(Path.Combine(testSolutionManager.SolutionDirectory, "packages"));

                // Act
                await packageRestoreManager.RaisePackagesMissingEventForSolutionAsync(testSolutionManager.SolutionDirectory, token);

                // Assert
                Assert.Equal(2, packagesMissingEventCount);
                Assert.True(packagesMissing);
            }
        }

        [Fact]
        public async Task TestRestoreMissingPackages()
        {
            // Arrange
            using (var testSolutionManager = new TestSolutionManager())
            {
                var projectA = testSolutionManager.AddNewMSBuildProject();
                var projectB = testSolutionManager.AddNewMSBuildProject();

                var packageIdentity = Packages[0];
                var testNuGetProjectContext = new TestNuGetProjectContext();
                var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV3OnlySourceRepositoryProvider();
                var testSettings = Configuration.NullSettings.Instance;
                var resolutionContext = new ResolutionContext();
                var token = CancellationToken.None;

                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    testSettings,
                    testSolutionManager,
                    deleteOnRestartManager);

                await nuGetPackageManager.InstallPackageAsync(projectA, packageIdentity,
                    resolutionContext, new TestNuGetProjectContext(), sourceRepositoryProvider.GetRepositories().First(), null, token);
                await nuGetPackageManager.InstallPackageAsync(projectB, packageIdentity,
                    resolutionContext, new TestNuGetProjectContext(), sourceRepositoryProvider.GetRepositories().First(), null, token);

                var packageRestoreManager = new PackageRestoreManager(
                    sourceRepositoryProvider,
                    testSettings,
                    testSolutionManager);
                Assert.True(nuGetPackageManager.PackageExistsInPackagesFolder(packageIdentity));

                // Delete packages folder
                TestFileSystemUtility.DeleteRandomTestFolder(Path.Combine(testSolutionManager.SolutionDirectory, "packages"));

                Assert.False(nuGetPackageManager.PackageExistsInPackagesFolder((packageIdentity)));

                // Act
                await packageRestoreManager.RestoreMissingPackagesInSolutionAsync(testSolutionManager.SolutionDirectory,
                    testNuGetProjectContext,
                    new TestLogger(),
                    CancellationToken.None);

                Assert.True(nuGetPackageManager.PackageExistsInPackagesFolder((packageIdentity)));
            }
        }

        /// <summary>
        /// This test installs 2 packages that can be restored into projectA and projectB
        /// Install 1 test package which cannot be restored into projectB and projectC
        /// Another one that cannot be restored into projectA and projectC
        /// </summary>
        [Fact]
        public async Task Test_PackageRestoreFailure_WithRaisedEvents()
        {
            // Arrange
            using (var testSolutionManager = new TestSolutionManager())
            using (var randomTestPackageSourcePath = TestDirectory.Create())
            {
                var projectA = testSolutionManager.AddNewMSBuildProject("projectA");
                var projectB = testSolutionManager.AddNewMSBuildProject("projectB");
                var projectC = testSolutionManager.AddNewMSBuildProject("projectC");

                var jQuery144 = Packages[0];
                var jQueryValidation = Packages[2];
                var testNuGetProjectContext = new TestNuGetProjectContext();
                var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV3OnlySourceRepositoryProvider();
                var testSettings = Configuration.NullSettings.Instance;
                var resolutionContext = new ResolutionContext();
                var token = CancellationToken.None;

                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    testSettings,
                    testSolutionManager,
                    deleteOnRestartManager);

                await nuGetPackageManager.InstallPackageAsync(projectA, jQueryValidation,
                    resolutionContext, testNuGetProjectContext, sourceRepositoryProvider.GetRepositories().First(), null, token);
                await nuGetPackageManager.InstallPackageAsync(projectB, jQueryValidation,
                    resolutionContext, testNuGetProjectContext, sourceRepositoryProvider.GetRepositories().First(), null, token);

                var testPackage1 = new PackageIdentity("package1A", new NuGetVersion("1.0.0"));
                var testPackage2 = new PackageIdentity("package1B", new NuGetVersion("1.0.0"));

                var packageFileInfo = TestPackagesGroupedByFolder.GetLegacyTestPackage(randomTestPackageSourcePath,
                    testPackage1.Id, testPackage1.Version.ToNormalizedString());
                using (var packageStream = GetDownloadResult(randomTestPackageSourcePath.Path, packageFileInfo))
                {
                    // Act
                    await projectB.InstallPackageAsync(testPackage1, packageStream, testNuGetProjectContext, token);
                    await projectC.InstallPackageAsync(testPackage1, packageStream, testNuGetProjectContext, token);
                }

                packageFileInfo = TestPackagesGroupedByFolder.GetLegacyTestPackage(randomTestPackageSourcePath,
                    testPackage2.Id, testPackage2.Version.ToNormalizedString());
                using (var packageStream = GetDownloadResult(randomTestPackageSourcePath.Path, packageFileInfo))
                {
                    // Act
                    await projectA.InstallPackageAsync(testPackage2, packageStream, testNuGetProjectContext, token);
                    await projectC.InstallPackageAsync(testPackage2, packageStream, testNuGetProjectContext, token);
                }

                var packageRestoreManager = new PackageRestoreManager(
                    sourceRepositoryProvider,
                    testSettings,
                    testSolutionManager);
                var restoredPackages = new List<PackageIdentity>();
                packageRestoreManager.PackageRestoredEvent += delegate (object sender, PackageRestoredEventArgs args) { restoredPackages.Add(args.Package); };

                var restoreFailedPackages = new ConcurrentDictionary<Packaging.PackageReference, IEnumerable<string>>(PackageReferenceComparer.Instance);
                packageRestoreManager.PackageRestoreFailedEvent += delegate (object sender, PackageRestoreFailedEventArgs args)
                {
                    restoreFailedPackages.AddOrUpdate(args.RestoreFailedPackageReference,
                        args.ProjectNames,
                        (Packaging.PackageReference packageReference, IEnumerable<string> oldValue) => { return oldValue; });
                };

                Assert.True(nuGetPackageManager.PackageExistsInPackagesFolder(jQueryValidation));

                // Delete packages folder
                TestFileSystemUtility.DeleteRandomTestFolder(Path.Combine(testSolutionManager.SolutionDirectory, "packages"));

                Assert.False(nuGetPackageManager.PackageExistsInPackagesFolder(jQuery144));
                Assert.False(nuGetPackageManager.PackageExistsInPackagesFolder(jQueryValidation));
                Assert.False(nuGetPackageManager.PackageExistsInPackagesFolder(testPackage1));
                Assert.False(nuGetPackageManager.PackageExistsInPackagesFolder(testPackage2));

                // Act
                await packageRestoreManager.RestoreMissingPackagesInSolutionAsync(testSolutionManager.SolutionDirectory,
                    testNuGetProjectContext,
                    new TestLogger(),
                    CancellationToken.None);

                // Assert
                Assert.True(nuGetPackageManager.PackageExistsInPackagesFolder(jQuery144));
                Assert.True(nuGetPackageManager.PackageExistsInPackagesFolder(jQueryValidation));
                Assert.False(nuGetPackageManager.PackageExistsInPackagesFolder(testPackage1));
                Assert.False(nuGetPackageManager.PackageExistsInPackagesFolder(testPackage2));

                Assert.Equal(4, restoredPackages.Count);
                // The ordering is not guaranteed and can vary. Do not assert based on that
                Assert.True(restoredPackages.Contains(jQuery144));
                Assert.True(restoredPackages.Contains(jQueryValidation));
                Assert.True(restoredPackages.Contains(testPackage1));
                Assert.True(restoredPackages.Contains(testPackage2));

                Assert.Equal(2, restoreFailedPackages.Count);

                // The ordering is not guaranteed and can vary. Do not assert based on that
                var restoreFailedPackageKeys = restoreFailedPackages.Keys;
                var testPackage1Key = restoreFailedPackageKeys.Where(r => r.PackageIdentity.Equals(testPackage1)).First();
                var testPackage1ProjectNames = restoreFailedPackages[testPackage1Key].ToList();

                Assert.Equal(2, testPackage1ProjectNames.Count);
                Assert.True(testPackage1ProjectNames.Contains("projectB", StringComparer.OrdinalIgnoreCase));
                Assert.True(testPackage1ProjectNames.Contains("projectC", StringComparer.OrdinalIgnoreCase));

                var testPackage2Key = restoreFailedPackageKeys.Where(r => r.PackageIdentity.Equals(testPackage2)).First();
                var testPackage2ProjectNames = restoreFailedPackages[testPackage2Key].ToList();

                Assert.Equal(2, testPackage2ProjectNames.Count);
                Assert.True(testPackage2ProjectNames.Contains("projectA", StringComparer.OrdinalIgnoreCase));
                Assert.True(testPackage2ProjectNames.Contains("projectC", StringComparer.OrdinalIgnoreCase));
            }
        }

        private static DownloadResourceResult GetDownloadResult(string source, FileInfo packageFileInfo)
        {
            return new DownloadResourceResult(packageFileInfo.OpenRead(), source);
        }
    }
}
