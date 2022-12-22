// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.PackageManagement;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.ProjectManagement;
using NuGet.ProjectManagement.Projects;
using NuGet.ProjectModel;
using NuGet.Protocol.Core.Types;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Test.Utility;
using Xunit;
using static NuGetConsole.Host.PowerShell.InstalledPackageEnumerator;

namespace NuGetConsole.Host.PowerShell.Test
{
    public class InstalledPackageEnumeratorTests
    {
        [Fact]
        public async Task EnumeratePackagesAsync_ForBuildIntegratedProject_ReturnsOrderedItems()
        {
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV3OnlySourceRepositoryProvider();

            using (var testDirectory = TestDirectory.Create())
            {
                var testSettings = PopulateSettingsWithSources(sourceRepositoryProvider, testDirectory);

                var solutionDirectory = Path.Combine(testDirectory.Path, "solutionA");
                Directory.CreateDirectory(solutionDirectory);
                using (var testSolutionManager = new TestSolutionManager(solutionDirectory))
                {
                    testSolutionManager.NuGetProjects.Add(Mock.Of<BuildIntegratedNuGetProject>());

                    var userPackageFolder = Path.Combine(testDirectory.Path, "userPackages");
                    Directory.CreateDirectory(userPackageFolder);

                    await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                        userPackageFolder,
                        new PackageIdentity("Foo", NuGetVersion.Parse("1.0.1")));

                    var fallbackPackageFolder = Path.Combine(testDirectory.Path, "fallbackPackages");
                    Directory.CreateDirectory(fallbackPackageFolder);

                    await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                        fallbackPackageFolder,
                        new PackageIdentity("Bar", NuGetVersion.Parse("1.0.2")));

                    var target = new InstalledPackageEnumerator(
                        testSolutionManager,
                        testSettings,
                        getLockFileOrNullAsync: _ => Task.FromResult(
                            new LockFile
                            {
                                PackageFolders = new[]
                                {
                                    new LockFileItem(userPackageFolder),
                                    new LockFileItem(fallbackPackageFolder)
                                },
                                Targets = new[]
                                {
                                    new LockFileTarget
                                    {
                                        TargetFramework = NuGetFramework.Parse("netcoreapp2.0"),
                                        Libraries = new[]
                                        {
                                            new LockFileTargetLibrary
                                            {
                                                Type = LibraryType.Package,
                                                Name = "Foo",
                                                Version = NuGetVersion.Parse("1.0.1"),
                                                Dependencies = new[]
                                                {
                                                    new PackageDependency("Bar")
                                                }
                                            },
                                            new LockFileTargetLibrary
                                            {
                                                Type = LibraryType.Package,
                                                Name = "Bar",
                                                Version = NuGetVersion.Parse("1.0.2")
                                            }
                                        }
                                    }
                                },
                                Libraries = new[]
                                {
                                    new LockFileLibrary
                                    {
                                        Type = LibraryType.Package,
                                        Name = "Foo",
                                        Version = NuGetVersion.Parse("1.0.1")
                                    },
                                    new LockFileLibrary
                                    {
                                        Type = LibraryType.Package,
                                        Name = "Bar",
                                        Version = NuGetVersion.Parse("1.0.2")
                                    }
                                }
                            }));

                    var packagesFolderPath = PackagesFolderPathUtility.GetPackagesFolderPath(
                        solutionDirectory, testSettings);

                    var testPackageManager = new NuGetPackageManager(
                        sourceRepositoryProvider,
                        testSettings,
                        packagesFolderPath);

                    // Act
                    var installedPackages = await target.EnumeratePackagesAsync(
                        testPackageManager,
                        CancellationToken.None);

                    // Assert: Order is important!
                    installedPackages.Should().Equal(
                        new PackageItem(
                            new PackageIdentity("Bar", NuGetVersion.Parse("1.0.2")),
                            Path.Combine(fallbackPackageFolder, "bar", "1.0.2")),
                        new PackageItem(
                            new PackageIdentity("Foo", NuGetVersion.Parse("1.0.1")),
                            Path.Combine(userPackageFolder, "foo", "1.0.1")));
                }
            }
        }

        [Fact]
        public async Task EnumeratePackagesAsync_ForPackagesConfig_ReturnsOrderedItems()
        {
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV3OnlySourceRepositoryProvider();

            using (var testDirectory = TestDirectory.Create())
            {
                var testSettings = PopulateSettingsWithSources(sourceRepositoryProvider, testDirectory);

                var solutionDirectory = Path.Combine(testDirectory.Path, "solutionA");

                Directory.CreateDirectory(solutionDirectory);
                using (var testSolutionManager = new TestSolutionManager(solutionDirectory))
                {
                    var userPackageFolder = Path.Combine(testDirectory.Path, "packagesA");
                    Directory.CreateDirectory(userPackageFolder);

                    testSettings.AddOrUpdate("config", new AddItem("repositoryPath", userPackageFolder));

                    var packagesFolderPath = PackagesFolderPathUtility.GetPackagesFolderPath(
                        solutionDirectory, testSettings);

                    var packageBar = new SimpleTestPackageContext
                    {
                        Id = "Bar",
                        Version = "1.0.2"
                    };

                    var packageFoo = new SimpleTestPackageContext
                    {
                        Id = "Foo",
                        Version = "1.0.1",
                        Dependencies = new List<SimpleTestPackageContext> { packageBar }
                    };

                    await SimpleTestPackageUtility.CreateFolderFeedPackagesConfigAsync(
                        packagesFolderPath,
                        packageFoo,
                        packageBar);

                    var target = new InstalledPackageEnumerator(
                        testSolutionManager,
                        testSettings,
                        getLockFileOrNullAsync: _ => Task.FromResult<LockFile>(null));

                    var projectSystem = Mock.Of<IMSBuildProjectSystem>();
                    Mock.Get(projectSystem)
                        .SetupGet(x => x.TargetFramework)
                        .Returns(NuGetFramework.Parse("net45"));

                    var project = new Mock<MSBuildNuGetProject>(
                        projectSystem, packagesFolderPath, testDirectory.Path);

                    project
                        .Setup(x => x.GetInstalledPackagesAsync(It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new[]
                        {
                            new PackageReference(
                                new PackageIdentity("Foo", NuGetVersion.Parse("1.0.1")),
                                NuGetFramework.Parse("net45")),
                            new PackageReference(
                                new PackageIdentity("Bar", NuGetVersion.Parse("1.0.2")),
                                NuGetFramework.Parse("net45"))
                        })
                        .Verifiable();

                    testSolutionManager.NuGetProjects.Add(project.Object);

                    var testPackageManager = new NuGetPackageManager(
                        sourceRepositoryProvider,
                        testSettings,
                        packagesFolderPath);

                    // Act
                    var installedPackages = await target.EnumeratePackagesAsync(
                        testPackageManager,
                        CancellationToken.None);

                    // Assert: Order is important!
                    installedPackages.Should().Equal(
                        new PackageItem(
                            new PackageIdentity("Bar", NuGetVersion.Parse("1.0.2")),
                            Path.Combine(packagesFolderPath, "Bar.1.0.2")),
                        new PackageItem(
                            new PackageIdentity("Foo", NuGetVersion.Parse("1.0.1")),
                            Path.Combine(packagesFolderPath, "Foo.1.0.1")));

                    project.Verify();
                }
            }
        }

        private static ISettings PopulateSettingsWithSources(
            ISourceRepositoryProvider sourceRepositoryProvider, TestDirectory settingsDirectory)
        {
            var settings = new Settings(settingsDirectory);
            foreach (var source in sourceRepositoryProvider.GetRepositories())
            {
                settings.AddOrUpdate(ConfigurationConstants.PackageSources, source.PackageSource.AsSourceItem());
            }

            return settings;
        }
    }
}
