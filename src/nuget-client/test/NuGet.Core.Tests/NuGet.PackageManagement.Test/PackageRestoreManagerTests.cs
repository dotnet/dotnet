// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NuGet.Configuration;
using NuGet.PackageManagement;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Test.Utility;
using Xunit;
using Xunit.Abstractions;

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

        private readonly ITestOutputHelper _output;

        public PackageRestoreManagerTests(ITestOutputHelper output)
        {
            _output = output;
        }

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

                packagesFromSolutionList.Should().ContainSingle();
                missingPackagesFromSolutionList.Should().BeEmpty();

                // Delete packages folder
                TestFileSystemUtility.DeleteRandomTestFolder(Path.Combine(testSolutionManager.SolutionDirectory, "packages"));

                packagesFromSolution = (await packageRestoreManager.GetPackagesInSolutionAsync(testSolutionManager.SolutionDirectory, token));

                packagesFromSolution.Where(p => p.IsMissing).Should().ContainSingle();
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

                packagesFromSolution.Any().Should().BeFalse();
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

                nuGetPackageManager.PackageExistsInPackagesFolder(packageIdentity).Should().BeTrue();

                // Delete packages folder
                TestFileSystemUtility.DeleteRandomTestFolder(Path.Combine(testSolutionManager.SolutionDirectory, "packages"));

                nuGetPackageManager.PackageExistsInPackagesFolder((packageIdentity)).Should().BeFalse();

                // Act
                await packageRestoreManager.RestoreMissingPackagesInSolutionAsync(testSolutionManager.SolutionDirectory,
                    testNuGetProjectContext,
                    new TestLogger(),
                    CancellationToken.None);

                restoredPackages.Should().ContainSingle();
                nuGetPackageManager.PackageExistsInPackagesFolder((packageIdentity)).Should().BeTrue();
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
                packagesMissingEventCount.Should().Be(1);
                packagesMissing.Should().BeFalse();

                // Delete packages folder
                TestFileSystemUtility.DeleteRandomTestFolder(Path.Combine(testSolutionManager.SolutionDirectory, "packages"));

                // Act
                await packageRestoreManager.RaisePackagesMissingEventForSolutionAsync(testSolutionManager.SolutionDirectory, token);

                // Assert
                packagesMissingEventCount.Should().Be(2);
                packagesMissing.Should().BeTrue();
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
                nuGetPackageManager.PackageExistsInPackagesFolder(packageIdentity).Should().BeTrue();

                // Delete packages folder
                TestFileSystemUtility.DeleteRandomTestFolder(Path.Combine(testSolutionManager.SolutionDirectory, "packages"));

                nuGetPackageManager.PackageExistsInPackagesFolder((packageIdentity)).Should().BeFalse();

                // Act
                await packageRestoreManager.RestoreMissingPackagesInSolutionAsync(testSolutionManager.SolutionDirectory,
                    testNuGetProjectContext,
                    new TestLogger(),
                    CancellationToken.None);

                nuGetPackageManager.PackageExistsInPackagesFolder((packageIdentity)).Should().BeTrue();
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
                var restoredPackages = new ConcurrentBag<PackageIdentity>();
                packageRestoreManager.PackageRestoredEvent += delegate (object sender, PackageRestoredEventArgs args)
                {
                    _output.WriteLine($"PackageRestoredEvent: args.Package={args.Package};\n" +
                        $"args.Restored={args.Restored}\n---\n");
                    restoredPackages.Add(args.Package);
                };

                var restoreFailedPackages = new ConcurrentDictionary<Packaging.PackageReference, IEnumerable<string>>(PackageReferenceComparer.Instance);
                packageRestoreManager.PackageRestoreFailedEvent += delegate (object sender, PackageRestoreFailedEventArgs args)
                {
                    _output.WriteLine($"PackageRestoreFailedEvent: {args.RestoreFailedPackageReference}\n" +
                        $"ProjectNames: {args.ProjectNames}\n---\n");
                    restoreFailedPackages.AddOrUpdate(args.RestoreFailedPackageReference,
                        args.ProjectNames,
                        (Packaging.PackageReference packageReference, IEnumerable<string> oldValue) => { return oldValue; });
                };

                nuGetPackageManager.PackageExistsInPackagesFolder(jQueryValidation).Should().BeTrue();

                // Delete packages folder
                TestFileSystemUtility.DeleteRandomTestFolder(Path.Combine(testSolutionManager.SolutionDirectory, "packages"));

                nuGetPackageManager.PackageExistsInPackagesFolder(jQuery144).Should().BeFalse();
                nuGetPackageManager.PackageExistsInPackagesFolder(jQueryValidation).Should().BeFalse();
                nuGetPackageManager.PackageExistsInPackagesFolder(testPackage1).Should().BeFalse();
                nuGetPackageManager.PackageExistsInPackagesFolder(testPackage2).Should().BeFalse();

                // Act
                PackageRestoreResult result = await packageRestoreManager.RestoreMissingPackagesInSolutionAsync(testSolutionManager.SolutionDirectory,
                    testNuGetProjectContext,
                    new TestLogger(),
                    CancellationToken.None);

                // Assert
                nuGetPackageManager.PackageExistsInPackagesFolder(jQuery144).Should().BeTrue();
                nuGetPackageManager.PackageExistsInPackagesFolder(jQueryValidation).Should().BeTrue();
                nuGetPackageManager.PackageExistsInPackagesFolder(testPackage1).Should().BeFalse();
                nuGetPackageManager.PackageExistsInPackagesFolder(testPackage2).Should().BeFalse();

                restoredPackages.Should().BeEquivalentTo(new[] { jQuery144, jQueryValidation, testPackage1, testPackage2 }, (options) => options.WithoutStrictOrdering());

                restoreFailedPackages.Select(i => i.Key.PackageIdentity).Should().BeEquivalentTo(new[] { testPackage1, testPackage2 });

                restoreFailedPackages[restoreFailedPackages.Keys.First(r => r.PackageIdentity.Equals(testPackage1))].Should().BeEquivalentTo(new[] { "projectB", "projectC" });

                restoreFailedPackages[restoreFailedPackages.Keys.First(r => r.PackageIdentity.Equals(testPackage2))].Should().BeEquivalentTo(new[] { "projectA", "projectC" });
            }
        }

        [Fact]
        public async Task RestoreMissingPackagesInSolutionAsync_WithProjectsSharingSameName_Succeeds()
        {
            // Arrange
            using var simpleTestPathContext = new SimpleTestPathContext();
            using var testSolutionManager = new TestSolutionManager(simpleTestPathContext);

            var projectA = testSolutionManager.AddNewMSBuildProject("Project");
            var projectBPath = Path.Combine(testSolutionManager.SolutionDirectory, "subfolder", "Project");
            var projectB = testSolutionManager.AddNewMSBuildProject("Project", projectPath: projectBPath, validateExistingProject: false);

            var packageA = new SimpleTestPackageContext("a", "1.0.0");
            await SimpleTestPackageUtility.CreateFolderFeedV3Async(simpleTestPathContext.PackageSource, packageA);

            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(new PackageSource(simpleTestPathContext.PackageSource));
            var testSettings = NullSettings.Instance;
            var resolutionContext = new ResolutionContext();

            var nuGetPackageManager = new NuGetPackageManager(
                sourceRepositoryProvider,
                testSettings,
                testSolutionManager,
                new TestDeleteOnRestartManager());

            await nuGetPackageManager.InstallPackageAsync(projectA, packageA.Identity,
                resolutionContext, new TestNuGetProjectContext(), sourceRepositoryProvider.GetRepositories().First(), null, CancellationToken.None);
            await nuGetPackageManager.InstallPackageAsync(projectB, packageA.Identity,
                resolutionContext, new TestNuGetProjectContext(), sourceRepositoryProvider.GetRepositories().First(), null, CancellationToken.None);

            var packageRestoreManager = new PackageRestoreManager(
                sourceRepositoryProvider,
                testSettings,
                testSolutionManager);

            // pre-conditions
            nuGetPackageManager.PackageExistsInPackagesFolder(packageA.Identity).Should().BeTrue();
            TestFileSystemUtility.DeleteRandomTestFolder(Path.Combine(testSolutionManager.SolutionDirectory, "packages"));
            nuGetPackageManager.PackageExistsInPackagesFolder((packageA.Identity)).Should().BeFalse();

            // Act
            PackageRestoreResult result = await packageRestoreManager.RestoreMissingPackagesInSolutionAsync(testSolutionManager.SolutionDirectory,
                new TestNuGetProjectContext(),
                new TestLogger(),
                CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            nuGetPackageManager.PackageExistsInPackagesFolder((packageA.Identity)).Should().BeTrue();
        }

        private static DownloadResourceResult GetDownloadResult(string source, FileInfo packageFileInfo)
        {
            return new DownloadResourceResult(packageFileInfo.OpenRead(), source);
        }
    }
}
