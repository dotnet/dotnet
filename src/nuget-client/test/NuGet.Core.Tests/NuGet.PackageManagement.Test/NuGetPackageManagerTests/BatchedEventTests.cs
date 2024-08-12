// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Test.Utility;
using Xunit;
using Xunit.Abstractions;

namespace NuGet.PackageManagement.Test.NuGetPackageManagerTests
{
    public class BatchedEventTests
    {
        // Following are the various sets of packages that are small in size. To be used by the functional tests
        private readonly List<PackageIdentity> _packageWithDependents = new List<PackageIdentity>
            {
                new PackageIdentity("jQuery", new NuGetVersion("1.4.4")),
                new PackageIdentity("jQuery", new NuGetVersion("1.6.4")),
                new PackageIdentity("jQuery.Validation", new NuGetVersion("1.13.1")),
                new PackageIdentity("jQuery.UI.Combined", new NuGetVersion("1.11.2"))
            };

        private readonly XunitLogger _logger;

        public BatchedEventTests(ITestOutputHelper output)
        {
            _logger = new XunitLogger(output);
        }

        [Fact]
        public async Task InstallPackage_BatchEvent_Raised()
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
        public async Task UpdatePackage_BatchEvent_Raised()
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
        public async Task UninstallPackage_BatchEvent_Raised()
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
        public async Task ExecuteMultipleNugetActions_BatchEvent_Raised()
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
        public async Task InstallPackagesInMultipleProjects_BatchEvent_Raised()
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
        public async Task ExecuteNugetActions_NoOP_BatchEvent()
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
        public async Task InstallPackage_Fail_BatchEvent_Raised()
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
        public async Task DownloadPackageTask_Fail_BatchEvent_NotRaised()
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
        public async Task DownloadPackageResult_Fail_BatchEvent_Raised()
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
        public async Task InstallPackage_BuildIntegratedProject_BatchEvent_NotRaised()
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
    }
}
