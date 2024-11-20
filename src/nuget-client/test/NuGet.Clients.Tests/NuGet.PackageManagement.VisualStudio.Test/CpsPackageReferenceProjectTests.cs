// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.ProjectSystem;
using Microsoft.VisualStudio.Sdk.TestFramework;
using Microsoft.VisualStudio.Threading;
using Moq;
using NuGet.Commands;
using NuGet.Commands.Test;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.LibraryModel;
using NuGet.PackageManagement.VisualStudio.Exceptions;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.ProjectManagement;
using NuGet.ProjectModel;
using NuGet.Protocol.Core.Types;
using NuGet.Test.Utility;
using NuGet.Versioning;
using NuGet.VisualStudio;
using Test.Utility;
using Test.Utility.VisualStudio;
using Xunit;
using Xunit.Abstractions;
using static NuGet.PackageManagement.VisualStudio.Test.ProjectFactories;

namespace NuGet.PackageManagement.VisualStudio.Test
{
    [Collection(MockedVS.Collection)]
    public class CpsPackageReferenceProjectTests : MockedVSCollectionTests
    {
        private readonly Mock<IOutputConsoleProvider> _outputConsoleProviderMock;
        private readonly Lazy<IOutputConsoleProvider> _outputConsoleProvider;
        public CpsPackageReferenceProjectTests(GlobalServiceProvider globalServiceProvider)
            : base(globalServiceProvider)
        {
            var mockOutputConsoleUtility = OutputConsoleUtility.GetMock();
            _outputConsoleProviderMock = mockOutputConsoleUtility.mockIOutputConsoleProvider;
            _outputConsoleProvider = new Lazy<IOutputConsoleProvider>(() => _outputConsoleProviderMock.Object);
        }

        [Fact]
        public async Task GetInstalledVersion_WithAssetsFile_ReturnsVersionsFromAssetsSpecs()
        {
            using (var testDirectory = TestDirectory.Create())
            {
                // Setup
                var projectName = "project1";
                var projectFullPath = Path.Combine(testDirectory.Path, projectName + ".csproj");

                // Project
                var projectCache = new ProjectSystemCache();
                IVsProjectAdapter projectAdapter = (new Mock<IVsProjectAdapter>()).Object;
                var project = CreateCpsPackageReferenceProject(projectName, projectFullPath, projectCache);

                var projectNames = GetTestProjectNames(projectFullPath, projectName);
                var packageSpec = GetPackageSpec(projectName, projectFullPath, "[2.0.0, )");

                // Restore info
                var projectRestoreInfo = ProjectTestHelpers.GetDGSpecForAllProjects(packageSpec);
                projectCache.AddProjectRestoreInfo(projectNames, projectRestoreInfo, new List<IAssetsLogMessage>());
                projectCache.AddProject(projectNames, projectAdapter, project).Should().BeTrue();

                // Package directories
                var sources = new List<PackageSource>();
                var packagesDir = new DirectoryInfo(Path.Combine(testDirectory, "globalPackages"));
                var packageSource = new DirectoryInfo(Path.Combine(testDirectory, "packageSource"));
                packagesDir.Create();
                packageSource.Create();
                sources.Add(new PackageSource(packageSource.FullName));

                var logger = new TestLogger();
                var request = new TestRestoreRequest(packageSpec, sources, packagesDir.FullName, logger)
                {
                    LockFilePath = Path.Combine(testDirectory, "project.assets.json")
                };

                await SimpleTestPackageUtility.CreateFullPackageAsync(packageSource.FullName, "packageA", "3.0.0");

                // Act
                var command = new RestoreCommand(request);
                var result = await command.ExecuteAsync();
                await result.CommitAsync(logger, CancellationToken.None);
                var packages = await project.GetInstalledPackagesAsync(CancellationToken.None);

                // Asert
                Assert.True(result.Success);
                packages.Should().Contain(a => a.PackageIdentity.Equals(new PackageIdentity("packageA", new NuGetVersion("3.0.0"))));
            }
        }

        [Fact]
        public async Task GetInstalledVersion_WithFloating_WithAssetsFile_ReturnsVersionsFromAssetsSpecs()
        {
            using (var testDirectory = TestDirectory.Create())
            {
                // Setup
                var projectName = "project1";
                var projectFullPath = Path.Combine(testDirectory.Path, projectName + ".csproj");

                // Project
                var projectCache = new ProjectSystemCache();
                IVsProjectAdapter projectAdapter = (new Mock<IVsProjectAdapter>()).Object;
                var project = CreateCpsPackageReferenceProject(projectName, projectFullPath, projectCache);

                var projectNames = GetTestProjectNames(projectFullPath, projectName);
                var packageSpec = GetPackageSpec(projectName, projectFullPath, "[*, )");

                // Restore info
                var projectRestoreInfo = ProjectTestHelpers.GetDGSpecForAllProjects(packageSpec);
                projectCache.AddProjectRestoreInfo(projectNames, projectRestoreInfo, new List<IAssetsLogMessage>());
                projectCache.AddProject(projectNames, projectAdapter, project).Should().BeTrue();

                // Package directories
                var sources = new List<PackageSource>();
                var packagesDir = new DirectoryInfo(Path.Combine(testDirectory, "globalPackages"));
                var packageSource = new DirectoryInfo(Path.Combine(testDirectory, "packageSource"));
                packagesDir.Create();
                packageSource.Create();
                sources.Add(new PackageSource(packageSource.FullName));

                var logger = new TestLogger();
                var request = new TestRestoreRequest(packageSpec, sources, packagesDir.FullName, logger)
                {
                    LockFilePath = Path.Combine(testDirectory, "project.assets.json")
                };

                await SimpleTestPackageUtility.CreateFullPackageAsync(packageSource.FullName, "packageA", "4.0.0");

                // Act
                var command = new RestoreCommand(request);
                var result = await command.ExecuteAsync();
                await result.CommitAsync(logger, CancellationToken.None);
                var packages = await project.GetInstalledPackagesAsync(CancellationToken.None);

                // Asert
                Assert.True(result.Success);
                packages.Should().Contain(a => a.PackageIdentity.Equals(new PackageIdentity("packageA", new NuGetVersion("4.0.0"))));

                var cache_packages = await project.GetInstalledPackagesAsync(CancellationToken.None);
                cache_packages.Should().Contain(a => a.PackageIdentity.Equals(new PackageIdentity("packageA", new NuGetVersion("4.0.0"))));
            }
        }

        [Fact]
        public async Task GetInstalledVersion_WithoutAssetsFile_ReturnsVersionsFromPackageSpecs()
        {
            using (var testDirectory = TestDirectory.Create())
            {
                // Setup
                var projectName = "project1";
                var projectFullPath = Path.Combine(testDirectory.Path, projectName + ".csproj");

                // Project
                var projectCache = new ProjectSystemCache();
                IVsProjectAdapter projectAdapter = (new Mock<IVsProjectAdapter>()).Object;
                var project = CreateCpsPackageReferenceProject(projectName, projectFullPath, projectCache);

                var projectNames = GetTestProjectNames(projectFullPath, projectName);
                var packageSpec = GetPackageSpec(projectName, projectFullPath, "[2.0.0, )");

                // Restore info
                var projectRestoreInfo = ProjectTestHelpers.GetDGSpecForAllProjects(packageSpec);
                projectCache.AddProjectRestoreInfo(projectNames, projectRestoreInfo, new List<IAssetsLogMessage>());
                projectCache.AddProject(projectNames, projectAdapter, project).Should().BeTrue();

                // Package directories
                var sources = new List<PackageSource>();
                var packagesDir = new DirectoryInfo(Path.Combine(testDirectory, "globalPackages"));
                var packageSource = new DirectoryInfo(Path.Combine(testDirectory, "packageSource"));
                packagesDir.Create();
                packageSource.Create();
                sources.Add(new PackageSource(packageSource.FullName));

                var logger = new TestLogger();

                // Act
                var packages = await project.GetInstalledPackagesAsync(CancellationToken.None);

                // Asert
                packages.Should().Contain(a => a.PackageIdentity.Equals(new PackageIdentity("packageA", new NuGetVersion("2.0.0"))));

                var cache_packages = await project.GetInstalledPackagesAsync(CancellationToken.None);
                cache_packages.Should().Contain(a => a.PackageIdentity.Equals(new PackageIdentity("packageA", new NuGetVersion("2.0.0"))));
            }
        }

        [Fact]
        public async Task GetInstalledVersion_WithoutPackages_ReturnsEmpty()
        {
            using (var testDirectory = TestDirectory.Create())
            {
                // Setup
                var projectName = "project1";
                var projectFullPath = Path.Combine(testDirectory.Path, projectName + ".csproj");

                // Project
                var projectCache = new ProjectSystemCache();
                IVsProjectAdapter projectAdapter = (new Mock<IVsProjectAdapter>()).Object;
                var project = CreateCpsPackageReferenceProject(projectName, projectFullPath, projectCache);

                var projectNames = GetTestProjectNames(projectFullPath, projectName);
                var packageSpec = GetPackageSpecNoPackages(projectName, projectFullPath);

                // Restore info
                var projectRestoreInfo = ProjectTestHelpers.GetDGSpecForAllProjects(packageSpec);
                projectCache.AddProjectRestoreInfo(projectNames, projectRestoreInfo, new List<IAssetsLogMessage>());
                projectCache.AddProject(projectNames, projectAdapter, project).Should().BeTrue();

                // Package directories
                var sources = new List<PackageSource>();
                var packagesDir = new DirectoryInfo(Path.Combine(testDirectory, "globalPackages"));
                var packageSource = new DirectoryInfo(Path.Combine(testDirectory, "packageSource"));
                packagesDir.Create();
                packageSource.Create();
                sources.Add(new PackageSource(packageSource.FullName));

                var logger = new TestLogger();
                var request = new TestRestoreRequest(packageSpec, sources, packagesDir.FullName, logger);

                // Act
                var command = new RestoreCommand(request);
                var result = await command.ExecuteAsync();
                await result.CommitAsync(logger, CancellationToken.None);
                var packages = await project.GetInstalledPackagesAsync(CancellationToken.None);

                // Asert
                packages.Should().BeEmpty();
            }
        }

        [Fact]
        public async Task GetInstalledVersion_WithASpecificVersionLowerThanAvailableOne_ReturnsVersionFromAssetsFile()
        {
            using (var testDirectory = TestDirectory.Create())
            {
                // Setup
                var projectName = "project1";
                var projectFullPath = Path.Combine(testDirectory.Path, projectName + ".csproj");

                // Project
                var projectCache = new ProjectSystemCache();
                IVsProjectAdapter projectAdapter = (new Mock<IVsProjectAdapter>()).Object;
                var project = CreateCpsPackageReferenceProject(projectName, projectFullPath, projectCache);

                var projectNames = GetTestProjectNames(projectFullPath, projectName);
                var packageSpec = GetPackageSpec(projectName, projectFullPath, "[2.0.0, )");

                // Restore info
                var projectRestoreInfo = ProjectTestHelpers.GetDGSpecForAllProjects(packageSpec);
                projectCache.AddProjectRestoreInfo(projectNames, projectRestoreInfo, new List<IAssetsLogMessage>());
                projectCache.AddProject(projectNames, projectAdapter, project).Should().BeTrue();

                // Package directories
                var sources = new List<PackageSource>();
                var packagesDir = new DirectoryInfo(Path.Combine(testDirectory, "globalPackages"));
                var packageSource = new DirectoryInfo(Path.Combine(testDirectory, "packageSource"));
                packagesDir.Create();
                packageSource.Create();
                sources.Add(new PackageSource(packageSource.FullName));

                var logger = new TestLogger();
                var request = new TestRestoreRequest(packageSpec, sources, packagesDir.FullName, logger)
                {
                    LockFilePath = Path.Combine(testDirectory, "project.assets.json")
                };

                await SimpleTestPackageUtility.CreateFullPackageAsync(packageSource.FullName, "packageA", "4.0.0");

                // Act
                var command = new RestoreCommand(request);
                var result = await command.ExecuteAsync();
                await result.CommitAsync(logger, CancellationToken.None);
                var packages = await project.GetInstalledPackagesAsync(CancellationToken.None);

                // Asert
                var exists = packages.Where(a => a.PackageIdentity.Equals(new PackageIdentity("packageA", new NuGetVersion("4.0.0"))));
                Assert.True(exists.Count() == 1);
            }
        }

        [Fact]
        public async Task GetInstalledVersion_WithoutPackages_WithAssets_ReturnsEmpty()
        {
            using (var testDirectory = TestDirectory.Create())
            {
                // Setup
                var projectName = "project1";
                var projectFullPath = Path.Combine(testDirectory.Path, projectName + ".csproj");

                // Project
                var projectCache = new ProjectSystemCache();
                IVsProjectAdapter projectAdapter = (new Mock<IVsProjectAdapter>()).Object;
                var project = CreateCpsPackageReferenceProject(projectName, projectFullPath, projectCache);

                var projectNames = GetTestProjectNames(projectFullPath, projectName);
                var packageSpec = GetPackageSpecNoPackages(projectName, projectFullPath);

                // Restore info
                var projectRestoreInfo = ProjectTestHelpers.GetDGSpecForAllProjects(packageSpec);
                projectCache.AddProjectRestoreInfo(projectNames, projectRestoreInfo, new List<IAssetsLogMessage>());
                projectCache.AddProject(projectNames, projectAdapter, project).Should().BeTrue();

                // Package directories
                var sources = new List<PackageSource>();
                var packagesDir = new DirectoryInfo(Path.Combine(testDirectory, "globalPackages"));
                var packageSource = new DirectoryInfo(Path.Combine(testDirectory, "packageSource"));
                packagesDir.Create();
                packageSource.Create();
                sources.Add(new PackageSource(packageSource.FullName));

                var logger = new TestLogger();
                var request = new TestRestoreRequest(packageSpec, sources, packagesDir.FullName, logger)
                {
                    LockFilePath = Path.Combine(testDirectory, "project.assets.json")
                };

                await SimpleTestPackageUtility.CreateFullPackageAsync(packageSource.FullName, "packageA", "1.0.0");

                // Act
                var command = new RestoreCommand(request);
                var result = await command.ExecuteAsync();
                await result.CommitAsync(logger, CancellationToken.None);
                var packages = await project.GetInstalledPackagesAsync(CancellationToken.None);

                // Asert
                packages.Should().BeEmpty();
            }
        }

        [Fact]
        public async Task GetInstalledVersions_WhenCalledMultipleTimes_ReturnsSameResult()
        {
            using (var testDirectory = TestDirectory.Create())
            {
                // Setup
                var projectName = "project1";
                var projectFullPath = Path.Combine(testDirectory.Path, projectName + ".csproj");

                // Project
                var projectCache = new ProjectSystemCache();
                IVsProjectAdapter projectAdapter = (new Mock<IVsProjectAdapter>()).Object;
                var project = CreateCpsPackageReferenceProject(projectName, projectFullPath, projectCache);

                var projectNames = GetTestProjectNames(projectFullPath, projectName);
                var packageSpec = GetPackageSpecMultipleVersions(projectName, projectFullPath);

                // Restore info
                var projectRestoreInfo = ProjectTestHelpers.GetDGSpecForAllProjects(packageSpec);
                projectCache.AddProjectRestoreInfo(projectNames, projectRestoreInfo, new List<IAssetsLogMessage>());
                projectCache.AddProject(projectNames, projectAdapter, project).Should().BeTrue();

                // Package directories
                var sources = new List<PackageSource>();
                var packagesDir = new DirectoryInfo(Path.Combine(testDirectory, "globalPackages"));
                var packageSource = new DirectoryInfo(Path.Combine(testDirectory, "packageSource"));
                packagesDir.Create();
                packageSource.Create();
                sources.Add(new PackageSource(packageSource.FullName));

                var logger = new TestLogger();
                var request = new TestRestoreRequest(packageSpec, sources, packagesDir.FullName, logger)
                {
                    LockFilePath = Path.Combine(testDirectory, "project.assets.json")
                };

                await SimpleTestPackageUtility.CreateFullPackageAsync(packageSource.FullName, "packageA", "3.0.0");
                await SimpleTestPackageUtility.CreateFullPackageAsync(packageSource.FullName, "packageB", "4.0.0");
                await SimpleTestPackageUtility.CreateFullPackageAsync(packageSource.FullName, "packageC", "1.0.0");

                // Act
                var command = new RestoreCommand(request);
                var result = await command.ExecuteAsync();
                await result.CommitAsync(logger, CancellationToken.None);
                var packages = await project.GetInstalledPackagesAsync(CancellationToken.None);

                // Asert
                Assert.True(result.Success);
                packages.Should().Contain(a => a.PackageIdentity.Equals(new PackageIdentity("packageA", new NuGetVersion("3.0.0"))));
                packages.Should().Contain(a => a.PackageIdentity.Equals(new PackageIdentity("packageB", new NuGetVersion("4.0.0"))));
                packages.Should().Contain(a => a.PackageIdentity.Equals(new PackageIdentity("packageC", new NuGetVersion("1.0.0"))));

                var cache_packages = await project.GetInstalledPackagesAsync(CancellationToken.None);
                cache_packages.Should().Contain(a => a.PackageIdentity.Equals(new PackageIdentity("packageA", new NuGetVersion("3.0.0"))));
                cache_packages = await project.GetInstalledPackagesAsync(CancellationToken.None);
                cache_packages.Should().Contain(a => a.PackageIdentity.Equals(new PackageIdentity("packageB", new NuGetVersion("4.0.0"))));
                cache_packages = await project.GetInstalledPackagesAsync(CancellationToken.None);
                cache_packages.Should().Contain(a => a.PackageIdentity.Equals(new PackageIdentity("packageC", new NuGetVersion("1.0.0"))));
            }
        }

        [Fact]
        public async Task GetInstalledVersion_WithAssetsFile_ChangingPackageSpec_ReturnsVersionsFromAssetsSpecs()
        {
            using (var testDirectory = TestDirectory.Create())
            {
                // Setup
                var projectName = "project1";

                // Project
                var projectCache = new ProjectSystemCache();
                IVsProjectAdapter projectAdapter = (new Mock<IVsProjectAdapter>()).Object;
                var packageSpec = GetPackageSpec(projectName, testDirectory, "[2.0.0, )");
                var projectFullPath = packageSpec.RestoreMetadata.ProjectPath;
                var project = CreateCpsPackageReferenceProject(projectName, projectFullPath, projectCache);
                var projectNames = GetTestProjectNames(projectFullPath, projectName);

                // Restore info
                var projectRestoreInfo = ProjectTestHelpers.GetDGSpecForAllProjects(packageSpec);
                projectCache.AddProjectRestoreInfo(projectNames, projectRestoreInfo, new List<IAssetsLogMessage>());
                projectCache.AddProject(projectNames, projectAdapter, project).Should().BeTrue();

                // Package directories
                var sources = new List<PackageSource>();
                var packagesDir = new DirectoryInfo(Path.Combine(testDirectory, "globalPackages"));
                var packageSource = new DirectoryInfo(Path.Combine(testDirectory, "packageSource"));
                packagesDir.Create();
                packageSource.Create();
                sources.Add(new PackageSource(packageSource.FullName));

                var logger = new TestLogger();
                var request = new TestRestoreRequest(packageSpec, sources, packagesDir.FullName, logger)
                {
                    LockFilePath = Path.Combine(testDirectory, "project.assets.json")
                };

                await SimpleTestPackageUtility.CreateFullPackageAsync(packageSource.FullName, "packageA", "2.0.0");
                await SimpleTestPackageUtility.CreateFullPackageAsync(packageSource.FullName, "packageA", "3.0.0");

                // Act
                var command = new RestoreCommand(request);
                var result = await command.ExecuteAsync();
                await result.CommitAsync(logger, CancellationToken.None);
                var packages = await project.GetInstalledPackagesAsync(CancellationToken.None);

                // Asert
                Assert.True(result.Success);
                packages.Should().Contain(a => a.PackageIdentity.Equals(new PackageIdentity("packageA", new NuGetVersion("2.0.0"))));

                // Setup
                packageSpec = GetPackageSpec(projectName, testDirectory, "[3.0.0, )");

                // Restore info
                projectRestoreInfo = ProjectTestHelpers.GetDGSpecForAllProjects(packageSpec);
                projectCache.AddProjectRestoreInfo(projectNames, projectRestoreInfo, new List<IAssetsLogMessage>());
                projectCache.AddProject(projectNames, projectAdapter, project).Should().BeTrue();

                request = new TestRestoreRequest(packageSpec, sources, packagesDir.FullName, logger)
                {
                    LockFilePath = Path.Combine(testDirectory, "project.assets.json")
                };

                // Act
                command = new RestoreCommand(request);
                result = await command.ExecuteAsync();
                await result.CommitAsync(logger, CancellationToken.None);
                packages = await project.GetInstalledPackagesAsync(CancellationToken.None);

                // Asert
                Assert.True(result.Success);
                packages.Should().Contain(a => a.PackageIdentity.Equals(new PackageIdentity("packageA", new NuGetVersion("3.0.0"))));
            }
        }

        [Theory]
        [InlineData("[2.0,3.4)")]
        [InlineData("[2.*,3.4)")]
        [InlineData("*")]
        public async Task TestPackageManager_InstallCustomVersion_Success(string versionRange)
        {
            using (var testDirectory = TestDirectory.Create())
            using (var testSolutionManager = new TestSolutionManager())
            {
                // Setup PackageSource
                var sources = new List<PackageSource>();
                var packageA_InitialVersion = new SimpleTestPackageContext("packageA", "1.0.0");
                var packageB_V1 = new SimpleTestPackageContext("packageB", "1.0.0");
                var packageB_V2 = new SimpleTestPackageContext("packageB", "2.0.0");
                var packageA = packageA_InitialVersion.Identity;
                var packageB = packageB_V2.Identity;
                var packageSource = Path.Combine(testDirectory, "packageSource");
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    packageSource,
                    PackageSaveMode.Defaultv3,
                    packageA_InitialVersion,
                    packageB_V1,
                    packageB_V2
                    );

                sources.Add(new PackageSource(packageSource));
                var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(sources);

                // Project
                var projectCache = new ProjectSystemCache();
                IVsProjectAdapter projectAdapter = (new Mock<IVsProjectAdapter>()).Object;
                ResolutionContext resolutionContext = (new Mock<ResolutionContext>()).Object;
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    NullSettings.Instance,
                    testSolutionManager,
                    deleteOnRestartManager);

                var testNuGetProjectContext = new TestNuGetProjectContext();
                var netCorePackageReferenceProjects = new List<CpsPackageReferenceProject>();

                // Create project
                var projectName = "a";
                var projectFullPath = Path.Combine(testDirectory.Path, projectName, projectName + ".csproj");
                var project = TestCpsPackageReferenceProject.CreateTestCpsPackageReferenceProject(projectName, projectFullPath, projectCache);

                // We need to treat NU1605 warning as error.
                project.IsNu1605Error = true;
                netCorePackageReferenceProjects.Add(project);
                testSolutionManager.NuGetProjects.Add(project);

                // Let new project pickup my custom package source.
                project.ProjectLocalSources.AddRange(sources);
                var projectNames = GetTestProjectNames(projectFullPath, projectName);
                var packageSpec = GetPackageSpec(
                    projectName,
                    projectFullPath,
                    packageA_InitialVersion.Version);

                // Restore info
                var projectRestoreInfo = ProjectTestHelpers.GetDGSpecForAllProjects(packageSpec);
                projectCache.AddProjectRestoreInfo(projectNames, projectRestoreInfo, new List<IAssetsLogMessage>());
                projectCache.AddProject(projectNames, projectAdapter, project).Should().BeTrue();

                var initialInstalledPackages = (await netCorePackageReferenceProjects[0].GetInstalledPackagesAsync(CancellationToken.None)).ToList();

                // Act
                var results = await nuGetPackageManager.PreviewProjectsInstallPackageAsync(
                    netCorePackageReferenceProjects, // All projects
                    packageB,
                    resolutionContext,
                    testNuGetProjectContext,
                    sourceRepositoryProvider.GetRepositories().ToList(),
                    VersionRange.Parse(versionRange),
                    CancellationToken.None);
                var actions = results.Select(a => a.Action).ToArray();
                await nuGetPackageManager.ExecuteNuGetProjectActionsAsync(
                    netCorePackageReferenceProjects,
                    actions,
                    testNuGetProjectContext,
                    new SourceCacheContext(),
                    CancellationToken.None);

                // This code added because nuGetPackageManager.InstallPackageAsync doesn't do updating ProjectSystemCache
                var installed = new LibraryDependency
                {
                    LibraryRange = new LibraryRange(
                        name: packageB.Id,
                        versionRange: VersionRange.Parse(versionRange),
                        typeConstraint: LibraryDependencyTarget.Package),
                };

                var newDependencies = packageSpec.TargetFrameworks.First().Dependencies.Insert(0, installed);
                packageSpec.TargetFrameworks[0] = new TargetFrameworkInformation(packageSpec.TargetFrameworks[0]) { Dependencies = newDependencies };

                projectRestoreInfo = ProjectTestHelpers.GetDGSpecForAllProjects(packageSpec);
                projectCache.AddProjectRestoreInfo(projectNames, projectRestoreInfo, new List<IAssetsLogMessage>());

                // Assert
                Assert.Equal(initialInstalledPackages.Count(), 1);
                var builtIntegratedActions = actions.OfType<BuildIntegratedProjectAction>().ToList();
                Assert.Equal(actions.Length, builtIntegratedActions.Count);
                Assert.True(builtIntegratedActions.All(b => b.VersionRange.OriginalString == versionRange));
                Assert.True(builtIntegratedActions.All(b => b.RestoreResult.Success));
                Assert.True(builtIntegratedActions.All(b => !b.RestoreResult.LogMessages.Any())); // There should be no error or warning.
                Assert.Equal(builtIntegratedActions[0].VersionRange, VersionRange.Parse(versionRange));

                var finalInstalledPackages = (await netCorePackageReferenceProjects[0].GetInstalledPackagesAsync(CancellationToken.None)).ToList();
                Assert.Contains(finalInstalledPackages, f => f.PackageIdentity.Id == packageA.Id
                && f.PackageIdentity.Version == packageA.Version);
                Assert.Contains(finalInstalledPackages, f => f.PackageIdentity.Id == packageB.Id
                && f.PackageIdentity.Version == packageB.Version);
            }
        }

        [Theory]
        [InlineData("*")]
        [InlineData("[1.*,2.0)")]
        [InlineData("1.*")]
        public async Task TestPackageManager_InstallCustomVersion_ResolvesToAvailableVersion(string versionRange)
        {
            using (var testDirectory = TestDirectory.Create())
            using (var testSolutionManager = new TestSolutionManager())
            {
                // Setup PackageSource
                var sources = new List<PackageSource>();
                var packageA_InitialVersion = new SimpleTestPackageContext("packageA", "1.0.0");
                var packageB_V1 = new SimpleTestPackageContext("packageB", "1.8.0");
                var packageA = packageA_InitialVersion.Identity;
                var packageSource = Path.Combine(testDirectory, "packageSource");
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    packageSource,
                    PackageSaveMode.Defaultv3,
                    packageA_InitialVersion,
                    packageB_V1
                    );

                sources.Add(new PackageSource(packageSource));
                var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(sources);

                // Project
                var projectCache = new ProjectSystemCache();
                IVsProjectAdapter projectAdapter = (new Mock<IVsProjectAdapter>()).Object;
                ResolutionContext resolutionContext = (new Mock<ResolutionContext>()).Object;
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    NullSettings.Instance,
                    testSolutionManager,
                    deleteOnRestartManager);

                var testNuGetProjectContext = new TestNuGetProjectContext();
                var netCorePackageReferenceProjects = new List<CpsPackageReferenceProject>();

                // Create project
                var projectName = "a";
                var projectFullPath = Path.Combine(testDirectory.Path, projectName, projectName + ".csproj");
                var project = TestCpsPackageReferenceProject.CreateTestCpsPackageReferenceProject(projectName, projectFullPath, projectCache);

                // We need to treat NU1605 warning as error.
                project.IsNu1605Error = true;
                netCorePackageReferenceProjects.Add(project);
                testSolutionManager.NuGetProjects.Add(project);

                // Let new project pickup my custom package source.
                project.ProjectLocalSources.AddRange(sources);
                var projectNames = GetTestProjectNames(projectFullPath, projectName);
                var packageSpec = GetPackageSpec(
                    projectName,
                    projectFullPath,
                    packageA_InitialVersion.Version);

                // Restore info
                var projectRestoreInfo = ProjectTestHelpers.GetDGSpecForAllProjects(packageSpec);
                projectCache.AddProjectRestoreInfo(projectNames, projectRestoreInfo, new List<IAssetsLogMessage>());
                projectCache.AddProject(projectNames, projectAdapter, project).Should().BeTrue();

                var initialInstalledPackages = (await netCorePackageReferenceProjects[0].GetInstalledPackagesAsync(CancellationToken.None)).ToList();

                // Act
                var results = await nuGetPackageManager.PreviewProjectsInstallPackageAsync(
                    netCorePackageReferenceProjects,
                    new PackageIdentity(packageB_V1.Id, new NuGetVersion("3.0.0")),
                    resolutionContext,
                    testNuGetProjectContext,
                    sourceRepositoryProvider.GetRepositories().ToList(),
                    VersionRange.Parse(versionRange),
                    CancellationToken.None);
                var actions = results.Select(a => a.Action).ToArray();
                await nuGetPackageManager.ExecuteNuGetProjectActionsAsync(
                    netCorePackageReferenceProjects,
                    actions,
                    testNuGetProjectContext,
                    new SourceCacheContext(),
                    CancellationToken.None);

                // This code added because nuGetPackageManager.InstallPackageAsync doesn't do updating ProjectSystemCache
                var installed = new LibraryDependency
                {
                    LibraryRange = new LibraryRange(
                        name: packageB_V1.Id,
                        versionRange: VersionRange.Parse(versionRange),
                        typeConstraint: LibraryDependencyTarget.Package),
                };

                var newDependencies = packageSpec.TargetFrameworks.First().Dependencies.Insert(0, installed);
                packageSpec.TargetFrameworks[0] = new TargetFrameworkInformation(packageSpec.TargetFrameworks[0]) { Dependencies = newDependencies };

                projectRestoreInfo = ProjectTestHelpers.GetDGSpecForAllProjects(packageSpec);
                projectCache.AddProjectRestoreInfo(projectNames, projectRestoreInfo, new List<IAssetsLogMessage>());

                // Assert
                Assert.Equal(initialInstalledPackages.Count(), 1);
                var builtIntegratedActions = actions.OfType<BuildIntegratedProjectAction>().ToList();
                Assert.Equal(actions.Length, builtIntegratedActions.Count);
                Assert.True(builtIntegratedActions.All(b => b.VersionRange.OriginalString == versionRange));
                Assert.True(builtIntegratedActions.All(b => b.RestoreResult.Success));
                Assert.True(builtIntegratedActions.All(b => !b.RestoreResult.LogMessages.Any())); // There should be no error or warning.
                Assert.Equal(builtIntegratedActions[0].VersionRange, VersionRange.Parse(versionRange));

                var finalInstalledPackages = (await netCorePackageReferenceProjects[0].GetInstalledPackagesAsync(CancellationToken.None)).ToList();
                Assert.Contains(finalInstalledPackages, f => f.PackageIdentity.Id == packageA.Id
                && f.PackageIdentity.Version == packageA.Version);
                Assert.Contains(finalInstalledPackages, f => f.PackageIdentity.Id == packageB_V1.Id
                && f.PackageIdentity.Version.ToString() == "1.8.0");
            }
        }

        [Theory]
        [InlineData("[2.0,3.4)")]
        [InlineData("[2.*,3.4)")]
        [InlineData("*")]
        public async Task TestPackageManager_InstallCustomVersionForAllProjects_Success(string versionRange)
        {
            using (var testDirectory = TestDirectory.Create())
            using (var testSolutionManager = new TestSolutionManager())
            {
                // Setup PackageSource
                var sources = new List<PackageSource>();
                var packageA_InitialVersion = new SimpleTestPackageContext("packageA", "1.0.0");
                var packageB_V1 = new SimpleTestPackageContext("packageB", "1.0.0");
                var packageB_V2 = new SimpleTestPackageContext("packageB", "2.0.0");
                var packageA = packageA_InitialVersion.Identity;
                var packageB = packageB_V2.Identity;
                var packageSource = Path.Combine(testDirectory, "packageSource");
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    packageSource,
                    PackageSaveMode.Defaultv3,
                    packageA_InitialVersion,
                    packageB_V1,
                    packageB_V2
                    );

                sources.Add(new PackageSource(packageSource));
                var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(sources);

                // Project
                int numberOfProjects = 2;
                var projectCache = new ProjectSystemCache();
                IVsProjectAdapter projectAdapter = (new Mock<IVsProjectAdapter>()).Object;
                ResolutionContext resolutionContext = (new Mock<ResolutionContext>()).Object;
                var packageSpecs = new PackageSpec[numberOfProjects];
                var projectFullPaths = new string[numberOfProjects];
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    NullSettings.Instance,
                    testSolutionManager,
                    deleteOnRestartManager);

                var testNuGetProjectContext = new TestNuGetProjectContext();
                var netCorePackageReferenceProjects = new List<CpsPackageReferenceProject>();
                var prevProj = string.Empty;
                PackageSpec prevPackageSpec = null;

                // Create projects
                for (var i = numberOfProjects - 1; i >= 0; i--)
                {
                    var projectName = $"project{i}";
                    var projectFullPath = Path.Combine(testDirectory.Path, projectName, projectName + ".csproj");
                    var project = TestCpsPackageReferenceProject.CreateTestCpsPackageReferenceProject(projectName, projectFullPath, projectCache);

                    // We need to treat NU1605 warning as error.
                    project.IsNu1605Error = true;
                    netCorePackageReferenceProjects.Add(project);
                    testSolutionManager.NuGetProjects.Add(project);

                    // Let new project pickup my custom package source.
                    project.ProjectLocalSources.AddRange(sources);
                    var projectNames = GetTestProjectNames(projectFullPath, projectName);
                    var packageSpec = GetPackageSpec(
                        projectName,
                        projectFullPath,
                        packageA_InitialVersion.Version);

                    if (prevPackageSpec != null)
                    {
                        packageSpec = packageSpec.WithTestProjectReference(prevPackageSpec);
                    }

                    // Restore info
                    var projectRestoreInfo = ProjectTestHelpers.GetDGSpecForAllProjects(packageSpec);
                    projectCache.AddProjectRestoreInfo(projectNames, projectRestoreInfo, new List<IAssetsLogMessage>());
                    projectCache.AddProject(projectNames, projectAdapter, project).Should().BeTrue();
                    prevProj = projectFullPath;
                    packageSpecs[i] = packageSpec;
                    prevPackageSpec = packageSpec;
                    projectFullPaths[i] = projectFullPath;
                }

                var initialInstalledPackages = (await netCorePackageReferenceProjects[numberOfProjects - 1].GetInstalledPackagesAsync(CancellationToken.None)).ToList();

                // Act
                var results = await nuGetPackageManager.PreviewProjectsInstallPackageAsync(
                    netCorePackageReferenceProjects, // All projects
                    packageB,
                    resolutionContext,
                    testNuGetProjectContext,
                    sourceRepositoryProvider.GetRepositories().ToList(),
                    VersionRange.Parse(versionRange),
                    CancellationToken.None);
                var actions = results.Select(a => a.Action).ToArray();
                await nuGetPackageManager.ExecuteNuGetProjectActionsAsync(
                    netCorePackageReferenceProjects,
                    actions,
                    testNuGetProjectContext,
                    new SourceCacheContext(),
                    CancellationToken.None);

                for (int i = 0; i < numberOfProjects; i++)
                {
                    // This code added because nuGetPackageManager.InstallPackageAsync doesn't do updating ProjectSystemCache
                    var installed = new LibraryDependency
                    {
                        LibraryRange = new LibraryRange(
                            name: packageB.Id,
                            versionRange: VersionRange.Parse(versionRange),
                            typeConstraint: LibraryDependencyTarget.Package),
                    };

                    var packageSpec = packageSpecs[i];

                    var newDependencies = packageSpec.TargetFrameworks.First().Dependencies.Insert(0, installed);
                    packageSpec.TargetFrameworks[0] = new TargetFrameworkInformation(packageSpec.TargetFrameworks[0]) { Dependencies = newDependencies };

                    var projectRestoreInfo = ProjectTestHelpers.GetDGSpecForAllProjects(packageSpec);
                    var projectNames = GetTestProjectNames(projectFullPaths[i], $"project{i}");
                    projectCache.AddProjectRestoreInfo(projectNames, projectRestoreInfo, new List<IAssetsLogMessage>());
                }

                // Assert
                Assert.Equal(initialInstalledPackages.Count(), 1);
                var builtIntegratedActions = actions.OfType<BuildIntegratedProjectAction>().ToList();
                for (int i = 0; i < numberOfProjects; i++)
                {
                    Assert.Equal(builtIntegratedActions[i].VersionRange, VersionRange.Parse(versionRange));
                }
                Assert.Equal(actions.Length, builtIntegratedActions.Count);
                Assert.True(builtIntegratedActions.All(b => b.VersionRange.OriginalString == versionRange));
                Assert.True(builtIntegratedActions.All(b => b.RestoreResult.Success));
                Assert.True(builtIntegratedActions.All(b => !b.RestoreResult.LogMessages.Any())); // There should be no error or warning.
                foreach (var netCorePackageReferenceProject in netCorePackageReferenceProjects)
                {
                    var finalInstalledPackages = (await netCorePackageReferenceProject.GetInstalledPackagesAsync(CancellationToken.None)).ToList();
                    Assert.Contains(finalInstalledPackages, f => f.PackageIdentity.Id == packageA.Id
                    && f.PackageIdentity.Version == packageA.Version);
                    Assert.Contains(finalInstalledPackages, f => f.PackageIdentity.Id == packageB.Id
                    && f.PackageIdentity.Version == packageB.Version);
                }
            }
        }

        [Theory]
        [InlineData("[2.0,3.4)")]
        [InlineData("[2.*,3.4)")]
        [InlineData("*")]
        public async Task TestPackageManager_UpdateToCustomVersionForAllProjects_Success(string versionRange)
        {
            using (var testDirectory = TestDirectory.Create())
            using (var testSolutionManager = new TestSolutionManager())
            {
                // Setup PackageSource
                var sources = new List<PackageSource>();
                var packageA_InitialVersion = new SimpleTestPackageContext("packageA", "1.0.0");
                var packageB_V1 = new SimpleTestPackageContext("packageB", "1.0.0");
                var packageB_V2 = new SimpleTestPackageContext("packageB", "2.0.0");
                var packageA = packageA_InitialVersion.Identity;
                var packageB = packageB_V2.Identity;
                var packageSource = Path.Combine(testDirectory, "packageSource");
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    packageSource,
                    PackageSaveMode.Defaultv3,
                    packageA_InitialVersion,
                    packageB_V1,
                    packageB_V2
                    );

                sources.Add(new PackageSource(packageSource));
                var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(sources);

                // Project
                int numberOfProjects = 2;
                var projectCache = new ProjectSystemCache();
                IVsProjectAdapter projectAdapter = (new Mock<IVsProjectAdapter>()).Object;
                ResolutionContext resolutionContext = (new Mock<ResolutionContext>()).Object;
                var packageSpecs = new PackageSpec[numberOfProjects];
                var projectFullPaths = new string[numberOfProjects];
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    NullSettings.Instance,
                    testSolutionManager,
                    deleteOnRestartManager);

                var testNuGetProjectContext = new TestNuGetProjectContext();
                var netCorePackageReferenceProjects = new List<CpsPackageReferenceProject>();
                var prevProj = string.Empty;
                PackageSpec prevPackageSpec = null;

                // Create projects
                for (var i = numberOfProjects - 1; i >= 0; i--)
                {
                    var projectName = $"project{i}";
                    var projectFullPath = Path.Combine(testDirectory.Path, projectName, projectName + ".csproj");
                    var project = TestCpsPackageReferenceProject.CreateTestCpsPackageReferenceProject(projectName, projectFullPath, projectCache);

                    // We need to treat NU1605 warning as error.
                    project.IsNu1605Error = true;
                    netCorePackageReferenceProjects.Add(project);
                    testSolutionManager.NuGetProjects.Add(project);

                    // Let new project pickup my custom package source.
                    project.ProjectLocalSources.AddRange(sources);
                    var projectNames = GetTestProjectNames(projectFullPath, projectName);
                    var packageSpec = GetPackageSpec(
                        projectName,
                        projectFullPath,
                        packageA_InitialVersion.Version);

                    if (prevPackageSpec != null)
                    {
                        packageSpec = packageSpec.WithTestProjectReference(prevPackageSpec);
                    }

                    // Restore info
                    var projectRestoreInfo = ProjectTestHelpers.GetDGSpecForAllProjects(packageSpec);
                    projectCache.AddProjectRestoreInfo(projectNames, projectRestoreInfo, new List<IAssetsLogMessage>());
                    projectCache.AddProject(projectNames, projectAdapter, project).Should().BeTrue();
                    prevProj = projectFullPath;
                    packageSpecs[i] = packageSpec;
                    prevPackageSpec = packageSpec;
                    projectFullPaths[i] = projectFullPath;
                }

                var initialInstalledPackages = (await netCorePackageReferenceProjects[numberOfProjects - 1].GetInstalledPackagesAsync(CancellationToken.None)).ToList();

                for (int i = 0; i < numberOfProjects; i++)
                {
                    // Install packageB since packageA is already there.
                    await nuGetPackageManager.InstallPackageAsync(netCorePackageReferenceProjects[i], packageB, new ResolutionContext(), new TestNuGetProjectContext(),
                            sourceRepositoryProvider.GetRepositories(), sourceRepositoryProvider.GetRepositories(), CancellationToken.None);

                    // This code added because nuGetPackageManager.InstallPackageAsync doesn't do updating ProjectSystemCache
                    var installed = new LibraryDependency
                    {
                        LibraryRange = new LibraryRange(
                            name: packageB.Id,
                            versionRange: new VersionRange(packageB.Version),
                            typeConstraint: LibraryDependencyTarget.Package),
                    };

                    var packageSpec = packageSpecs[i];

                    var newDependencies = packageSpec.TargetFrameworks.First().Dependencies.Add(installed);
                    packageSpec.TargetFrameworks[0] = new TargetFrameworkInformation(packageSpec.TargetFrameworks[0]) { Dependencies = newDependencies };

                    var projectRestoreInfo = ProjectTestHelpers.GetDGSpecForAllProjects(packageSpec);
                    var projectNames = GetTestProjectNames(projectFullPaths[i], $"project{i}");
                    projectCache.AddProjectRestoreInfo(projectNames, projectRestoreInfo, new List<IAssetsLogMessage>());
                }

                // Act
                var results = await nuGetPackageManager.PreviewProjectsInstallPackageAsync(
                    netCorePackageReferenceProjects, // All projects
                    packageB,
                    resolutionContext,
                    testNuGetProjectContext,
                    sourceRepositoryProvider.GetRepositories().ToList(),
                    VersionRange.Parse(versionRange),
                    CancellationToken.None);
                var actions = results.Select(a => a.Action).ToArray();
                await nuGetPackageManager.ExecuteNuGetProjectActionsAsync(
                    netCorePackageReferenceProjects,
                    actions,
                    testNuGetProjectContext,
                    new SourceCacheContext(),
                    CancellationToken.None);

                for (int i = 0; i < numberOfProjects; i++)
                {
                    // This code added because nuGetPackageManager.InstallPackageAsync doesn't do updating ProjectSystemCache
                    var installed = new LibraryDependency
                    {
                        LibraryRange = new LibraryRange(
                            name: packageB.Id,
                            versionRange: VersionRange.Parse(versionRange),
                            typeConstraint: LibraryDependencyTarget.Package),
                    };

                    var packageSpec = packageSpecs[i];

                    var newDependencies = packageSpec.TargetFrameworks.First().Dependencies.Insert(0, installed);
                    packageSpec.TargetFrameworks[0] = new TargetFrameworkInformation(packageSpec.TargetFrameworks[0]) { Dependencies = newDependencies };

                    var projectRestoreInfo = ProjectTestHelpers.GetDGSpecForAllProjects(packageSpec);
                    var projectNames = GetTestProjectNames(projectFullPaths[i], $"project{i}");
                    projectCache.AddProjectRestoreInfo(projectNames, projectRestoreInfo, new List<IAssetsLogMessage>());
                }

                // Assert
                Assert.Equal(initialInstalledPackages.Count(), 1);
                var builtIntegratedActions = actions.OfType<BuildIntegratedProjectAction>().ToList();
                for (int i = 0; i < numberOfProjects; i++)
                {
                    Assert.Equal(builtIntegratedActions[i].VersionRange, VersionRange.Parse(versionRange));
                }
                Assert.True(builtIntegratedActions.All(b => b.VersionRange.OriginalString == versionRange));
                Assert.Equal(actions.Length, builtIntegratedActions.Count);
                Assert.True(builtIntegratedActions.All(b => b.RestoreResult.Success));
                Assert.True(builtIntegratedActions.All(b => !b.RestoreResult.LogMessages.Any())); // There should be no error or warning.
                foreach (var netCorePackageReferenceProject in netCorePackageReferenceProjects)
                {
                    var finalInstalledPackages = (await netCorePackageReferenceProject.GetInstalledPackagesAsync(CancellationToken.None)).ToList();
                    Assert.Contains(finalInstalledPackages, f => f.PackageIdentity.Id == packageA.Id
                    && f.PackageIdentity.Version == packageA.Version);
                    Assert.Contains(finalInstalledPackages, f => f.PackageIdentity.Id == packageB.Id
                    && f.PackageIdentity.Version == packageB.Version);
                }
            }
        }

        [Fact]
        public async Task TestPackageManager_InstallPackageForAllProjects_Success()
        {
            using (var testDirectory = TestDirectory.Create())
            using (var testSolutionManager = new TestSolutionManager())
            {
                // Set up Package Source
                var sources = new List<PackageSource>();
                var packageA_Version100 = new SimpleTestPackageContext("packageA", "1.0.0");
                var packageB_Version100 = new SimpleTestPackageContext("packageB", "1.0.0");
                var packageA = packageA_Version100.Identity;
                var packageB = packageB_Version100.Identity;
                var packageSource = Path.Combine(testDirectory, "packageSource");
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    packageSource,
                    PackageSaveMode.Defaultv3,
                    packageA_Version100,
                    packageB_Version100
                    );

                sources.Add(new PackageSource(packageSource));
                var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(sources);

                // Project
                int numberOfProjects = 4;
                var projectCache = new ProjectSystemCache();
                IVsProjectAdapter projectAdapter = (new Mock<IVsProjectAdapter>()).Object;
                ResolutionContext resolutionContext = (new Mock<ResolutionContext>()).Object;
                var packageSpecs = new PackageSpec[numberOfProjects];
                var projectFullPaths = new string[numberOfProjects];
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    NullSettings.Instance,
                    testSolutionManager,
                    deleteOnRestartManager);

                var testNuGetProjectContext = new TestNuGetProjectContext();
                var netCorePackageReferenceProjects = new List<CpsPackageReferenceProject>();
                var prevProj = string.Empty;
                PackageSpec prevPackageSpec = null;

                // Create projects
                for (var i = numberOfProjects - 1; i >= 0; i--)
                {
                    var projectName = $"project{i}";
                    var projectFullPath = Path.Combine(testDirectory.Path, projectName, projectName + ".csproj");
                    var project = TestCpsPackageReferenceProject.CreateTestCpsPackageReferenceProject(projectName, projectFullPath, projectCache);

                    // We need to treat NU1605 warning as error.
                    project.IsNu1605Error = true;
                    netCorePackageReferenceProjects.Add(project);
                    testSolutionManager.NuGetProjects.Add(project);

                    //Let new project pickup my custom package source.
                    project.ProjectLocalSources.AddRange(sources);
                    var projectNames = GetTestProjectNames(projectFullPath, projectName);
                    var packageSpec = GetPackageSpec(
                        projectName,
                        projectFullPath,
                        packageA_Version100.Version);

                    if (prevPackageSpec != null)
                    {
                        packageSpec = packageSpec.WithTestProjectReference(prevPackageSpec);
                    }

                    // Restore info
                    var projectRestoreInfo = ProjectTestHelpers.GetDGSpecForAllProjects(packageSpec);
                    projectCache.AddProjectRestoreInfo(projectNames, projectRestoreInfo, new List<IAssetsLogMessage>());
                    projectCache.AddProject(projectNames, projectAdapter, project).Should().BeTrue();
                    prevProj = projectFullPath;
                    packageSpecs[i] = packageSpec;
                    prevPackageSpec = packageSpec;
                    projectFullPaths[i] = projectFullPath;
                }

                var initialInstalledPackages = (await netCorePackageReferenceProjects[numberOfProjects - 1].GetInstalledPackagesAsync(CancellationToken.None)).ToList();

                for (int i = 0; i < numberOfProjects; i++)
                {
                    // This code added because nuGetPackageManager.InstallPackageAsync doesn't do updating ProjectSystemCache
                    var installed = new LibraryDependency
                    {
                        LibraryRange = new LibraryRange(
                            name: packageB.Id,
                            versionRange: new VersionRange(packageB.Version),
                            typeConstraint: LibraryDependencyTarget.Package),
                    };

                    var packageSpec = packageSpecs[i];

                    var newDependencies = packageSpec.TargetFrameworks.First().Dependencies.Add(installed);
                    packageSpec.TargetFrameworks[0] = new TargetFrameworkInformation(packageSpec.TargetFrameworks[0]) { Dependencies = newDependencies };

                    var projectRestoreInfo = ProjectTestHelpers.GetDGSpecForAllProjects(packageSpec);
                    var projectNames = GetTestProjectNames(projectFullPaths[i], $"project{i}");
                    projectCache.AddProjectRestoreInfo(projectNames, projectRestoreInfo, new List<IAssetsLogMessage>());
                }

                // Act
                var results = await nuGetPackageManager.PreviewProjectsInstallPackageAsync(
                    netCorePackageReferenceProjects, // All projects
                    packageB,
                    resolutionContext,
                    testNuGetProjectContext,
                    sourceRepositoryProvider.GetRepositories().ToList(),
                    CancellationToken.None);

                var actions = results.Select(a => a.Action).ToArray();

                await nuGetPackageManager.ExecuteNuGetProjectActionsAsync(
                    netCorePackageReferenceProjects,
                    actions,
                    testNuGetProjectContext,
                    new SourceCacheContext(),
                    CancellationToken.None);

                // Assert
                Assert.Equal(initialInstalledPackages.Count(), 1);
                var builtIntegratedActions = actions.OfType<BuildIntegratedProjectAction>().ToList();
                Assert.Equal(actions.Length, builtIntegratedActions.Count);
                Assert.True(builtIntegratedActions.All(b => b.RestoreResult.Success));
                Assert.True(builtIntegratedActions.All(b => !b.RestoreResult.LogMessages.Any())); // There should be no error or warning.
                foreach (var netCorePackageReferenceProject in netCorePackageReferenceProjects)
                {
                    var finalInstalledPackages = (await netCorePackageReferenceProject.GetInstalledPackagesAsync(CancellationToken.None)).ToList();
                    Assert.Contains(finalInstalledPackages, f => f.PackageIdentity.Id == packageA.Id
                    && f.PackageIdentity.Version == packageA.Version);
                    Assert.Contains(finalInstalledPackages, f => f.PackageIdentity.Id == packageB.Id
                    && f.PackageIdentity.Version == packageB.Version);
                }
            }
        }

        [Fact]
        public async Task TestPackageManager_UpgradePackageForAllProjects_Success()
        {
            using (var testDirectory = TestDirectory.Create())
            using (var testSolutionManager = new TestSolutionManager())
            {
                // Set up Package Source
                var sources = new List<PackageSource>();
                var packageA_Version100 = new SimpleTestPackageContext("packageA", "1.0.0");
                var packageB_Version100 = new SimpleTestPackageContext("packageB", "1.0.0");
                var packageB_Version200 = new SimpleTestPackageContext("packageB", "2.0.0");
                var packageA = packageA_Version100.Identity;
                var packageB = packageB_Version100.Identity;
                var packageB_UpgradeVersion = packageB_Version200.Identity;
                var packageSource = Path.Combine(testDirectory, "packageSource");
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    packageSource,
                    PackageSaveMode.Defaultv3,
                    packageA_Version100,
                    packageB_Version100,
                    packageB_Version200
                    );

                sources.Add(new PackageSource(packageSource));
                var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(sources);

                // Project
                int numberOfProjects = 4;
                var projectCache = new ProjectSystemCache();
                IVsProjectAdapter projectAdapter = (new Mock<IVsProjectAdapter>()).Object;
                ResolutionContext resolutionContext = (new Mock<ResolutionContext>()).Object;
                var packageSpecs = new PackageSpec[numberOfProjects];
                var projectFullPaths = new string[numberOfProjects];
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    NullSettings.Instance,
                    testSolutionManager,
                    deleteOnRestartManager);

                var testNuGetProjectContext = new TestNuGetProjectContext() { EnableLogging = true };
                var netCorePackageReferenceProjects = new List<CpsPackageReferenceProject>();
                var prevProj = string.Empty;
                PackageSpec prevPackageSpec = null;

                // Create projects
                for (var i = numberOfProjects - 1; i >= 0; i--)
                {
                    var projectName = $"project{i}";
                    var projectFullPath = Path.Combine(testDirectory.Path, projectName, projectName + ".csproj");
                    var project = TestCpsPackageReferenceProject.CreateTestCpsPackageReferenceProject(projectName, projectFullPath, projectCache);

                    // We need to treat NU1605 warning as error.
                    project.IsNu1605Error = true;
                    netCorePackageReferenceProjects.Add(project);
                    testSolutionManager.NuGetProjects.Add(project);

                    //Let new project pickup my custom package source.
                    project.ProjectLocalSources.AddRange(sources);
                    var projectNames = GetTestProjectNames(projectFullPath, projectName);
                    var packageSpec = GetPackageSpec(
                        projectName,
                        projectFullPath,
                        packageA_Version100.Version);

                    if (prevPackageSpec != null)
                    {
                        packageSpec = packageSpec.WithTestProjectReference(prevPackageSpec);
                    }

                    // Restore info
                    var projectRestoreInfo = ProjectTestHelpers.GetDGSpecForAllProjects(packageSpec);
                    projectCache.AddProjectRestoreInfo(projectNames, projectRestoreInfo, new List<IAssetsLogMessage>());
                    projectCache.AddProject(projectNames, projectAdapter, project).Should().BeTrue();
                    prevProj = projectFullPath;
                    packageSpecs[i] = packageSpec;
                    prevPackageSpec = packageSpec;
                    projectFullPaths[i] = projectFullPath;
                }

                for (int i = 0; i < numberOfProjects; i++)
                {
                    // Install packageB since packageA is already there.
                    await nuGetPackageManager.InstallPackageAsync(netCorePackageReferenceProjects[i], packageB, new ResolutionContext(), new TestNuGetProjectContext(),
                            sourceRepositoryProvider.GetRepositories(), sourceRepositoryProvider.GetRepositories(), CancellationToken.None);

                    // This code added because nuGetPackageManager.InstallPackageAsync doesn't do updating ProjectSystemCache
                    var installed = new LibraryDependency
                    {
                        LibraryRange = new LibraryRange(
                            name: packageB.Id,
                            versionRange: new VersionRange(packageB.Version),
                            typeConstraint: LibraryDependencyTarget.Package),
                    };

                    var packageSpec = packageSpecs[i];

                    var newDependencies = packageSpec.TargetFrameworks.First().Dependencies.Add(installed);
                    packageSpec.TargetFrameworks[0] = new TargetFrameworkInformation(packageSpec.TargetFrameworks[0]) { Dependencies = newDependencies };

                    var projectRestoreInfo = ProjectTestHelpers.GetDGSpecForAllProjects(packageSpec);
                    var projectNames = GetTestProjectNames(projectFullPaths[i], $"project{i}");
                    projectCache.AddProjectRestoreInfo(projectNames, projectRestoreInfo, new List<IAssetsLogMessage>());
                }

                var initialInstalledPackages = (await netCorePackageReferenceProjects[numberOfProjects - 1].GetInstalledPackagesAsync(CancellationToken.None)).ToList();

                // Act
                var results = await nuGetPackageManager.PreviewProjectsInstallPackageAsync(
                    netCorePackageReferenceProjects, // All projects
                    packageB_UpgradeVersion,
                    resolutionContext,
                    testNuGetProjectContext,
                    sourceRepositoryProvider.GetRepositories().ToList(),
                    CancellationToken.None);
                var actions = results.Select(a => a.Action).ToArray();
                await nuGetPackageManager.ExecuteNuGetProjectActionsAsync(
                    netCorePackageReferenceProjects,
                    actions,
                    testNuGetProjectContext,
                    new SourceCacheContext(),
                    CancellationToken.None);

                // Assert
                Assert.Equal(initialInstalledPackages.Count(), 2);
                var builtIntegratedActions = actions.OfType<BuildIntegratedProjectAction>().ToList();
                Assert.Equal(actions.Length, builtIntegratedActions.Count);
                Assert.True(builtIntegratedActions.All(b => b.RestoreResult.Success));
                Assert.True(builtIntegratedActions.All(b => !b.RestoreResult.LogMessages.Any())); // There should be no error or warning.
                // Make sure all 4 project installed packageB upgraded version.
                foreach (var netCorePackageReferenceProject in netCorePackageReferenceProjects)
                {
                    var finalInstalledPackages = (await netCorePackageReferenceProject.GetInstalledPackagesAsync(CancellationToken.None)).ToList();
                    Assert.Contains(finalInstalledPackages, f => f.PackageIdentity.Id == packageB_UpgradeVersion.Id
                    && f.PackageIdentity.Version == packageB_UpgradeVersion.Version);
                }
                var restoringLogs = testNuGetProjectContext.Logs.Value.Where(l => l.StartsWith("Restoring packages for ")).ToList();
                var restoredLogs = testNuGetProjectContext.Logs.Value.Where(l => l.StartsWith("Restored ")).ToList();
                // Making sure project0 restored only once, not many.
                Assert.Equal(restoringLogs.Count(l => l.EndsWith("project0.csproj...")), 1);
                Assert.Equal(restoredLogs.Count(l => l.Contains("project0.csproj")), 1);
                Assert.Equal(restoringLogs.Count(l => l.EndsWith("project1.csproj...")), 1);
                Assert.Equal(restoredLogs.Count(l => l.Contains("project1.csproj")), 1);
                Assert.Equal(restoringLogs.Count(l => l.EndsWith("project2.csproj...")), 1);
                Assert.Equal(restoredLogs.Count(l => l.Contains("project2.csproj")), 1);
                Assert.Equal(restoringLogs.Count(l => l.EndsWith("project3.csproj...")), 1);
                Assert.Equal(restoredLogs.Count(l => l.Contains("project3.csproj")), 1);
                var writingAssetsLogs = testNuGetProjectContext.Logs.Value.Where(l => l.StartsWith("Writing assets file to disk.")).ToList();
                // Only 4 write to assets for above 4 projects, never more than that.
                Assert.Equal(writingAssetsLogs.Count, 4);
            }
        }

        [Fact(Skip = "https://github.com/NuGet/Home/issues/10208")]
        public async Task TestPackageManager_UpgradePackageFor_TopParentProject_Success()
        {
            using (var testDirectory = new SimpleTestPathContext())
            using (var testSolutionManager = new TestSolutionManager())
            {
                // Set up Package Source
                var sources = new List<PackageSource>();
                var packageA_Version100 = new SimpleTestPackageContext("packageA", "1.0.0");
                var packageB_Version100 = new SimpleTestPackageContext("packageB", "1.0.0");
                var packageB_Version200 = new SimpleTestPackageContext("packageB", "2.0.0");
                var packageA = packageA_Version100.Identity;
                var packageB = packageB_Version100.Identity;
                var packageB_UpgradeVersion = packageB_Version200.Identity;
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    testDirectory.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageA_Version100,
                    packageB_Version100,
                    packageB_Version200
                    );

                sources.Add(new PackageSource(testDirectory.PackageSource));
                var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(sources);

                // Project
                int numberOfProjects = 4;
                var projectCache = new ProjectSystemCache();
                IVsProjectAdapter projectAdapter = (new Mock<IVsProjectAdapter>()).Object;
                ResolutionContext resolutionContext = (new Mock<ResolutionContext>()).Object;
                var packageSpecs = new PackageSpec[numberOfProjects];
                var projectFullPaths = new string[numberOfProjects];
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    NullSettings.Instance,
                    testSolutionManager,
                    deleteOnRestartManager);

                var testNuGetProjectContext = new TestNuGetProjectContext();
                var netCorePackageReferenceProjects = new List<CpsPackageReferenceProject>();
                var prevProj = string.Empty;
                PackageSpec prevPackageSpec = null;

                // Create projects
                for (var i = numberOfProjects - 1; i >= 0; i--)
                {
                    var projectName = $"project{i}";
                    var projectFullPath = Path.Combine(testDirectory.SolutionRoot, projectName, projectName + ".csproj");
                    var project = TestCpsPackageReferenceProject.CreateTestCpsPackageReferenceProject(projectName, projectFullPath, projectCache);

                    // We need to treat NU1605 warning as error.
                    project.IsNu1605Error = true;
                    netCorePackageReferenceProjects.Add(project);
                    testSolutionManager.NuGetProjects.Add(project);

                    //Let new project pickup my custom package source.
                    project.ProjectLocalSources.AddRange(sources);
                    var projectNames = GetTestProjectNames(projectFullPath, projectName);
                    var packageSpec = GetPackageSpec(
                        projectName,
                        projectFullPath,
                        packageA_Version100.Version);

                    if (prevPackageSpec != null)
                    {
                        packageSpec = packageSpec.WithTestProjectReference(prevPackageSpec);
                    }

                    // Restore info
                    var projectRestoreInfo = ProjectTestHelpers.GetDGSpecForAllProjects(packageSpec);
                    projectCache.AddProjectRestoreInfo(projectNames, projectRestoreInfo, new List<IAssetsLogMessage>());
                    projectCache.AddProject(projectNames, projectAdapter, project).Should().BeTrue();
                    prevProj = projectFullPath;
                    packageSpecs[i] = packageSpec;
                    prevPackageSpec = packageSpec;
                    projectFullPaths[i] = projectFullPath;
                }

                for (int i = 0; i < numberOfProjects; i++)
                {
                    // Install packageB since packageA is already there.
                    await nuGetPackageManager.InstallPackageAsync(netCorePackageReferenceProjects[i], packageB, new ResolutionContext(), new TestNuGetProjectContext(),
                            sourceRepositoryProvider.GetRepositories(), sourceRepositoryProvider.GetRepositories(), CancellationToken.None);

                    // This code added because nuGetPackageManager.InstallPackageAsync doesn't do updating ProjectSystemCache
                    var installed = new LibraryDependency
                    {
                        LibraryRange = new LibraryRange(
                            name: packageB.Id,
                            versionRange: new VersionRange(packageB.Version),
                            typeConstraint: LibraryDependencyTarget.Package),
                    };

                    var packageSpec = packageSpecs[i];

                    var newDependencies = packageSpec.TargetFrameworks.First().Dependencies.Add(installed);
                    packageSpec.TargetFrameworks[0] = new TargetFrameworkInformation(packageSpec.TargetFrameworks[0]) { Dependencies = newDependencies };

                    var projectRestoreInfo = ProjectTestHelpers.GetDGSpecForAllProjects(packageSpec);
                    var projectNames = GetTestProjectNames(projectFullPaths[i], $"project{i}");
                    projectCache.AddProjectRestoreInfo(projectNames, projectRestoreInfo, new List<IAssetsLogMessage>());
                }

                var initialInstalledPackages = (await netCorePackageReferenceProjects[numberOfProjects - 1].GetInstalledPackagesAsync(CancellationToken.None)).ToList();

                var targetProjects = new List<CpsPackageReferenceProject>()
                {
                    netCorePackageReferenceProjects[numberOfProjects-1] // Top parent project.
                };

                // Act
                var results = await nuGetPackageManager.PreviewProjectsInstallPackageAsync(
                    targetProjects,
                    packageB_UpgradeVersion,
                    resolutionContext,
                    testNuGetProjectContext,
                    sourceRepositoryProvider.GetRepositories().ToList(),
                    CancellationToken.None);
                var actions = results.Select(a => a.Action).ToArray();
                await nuGetPackageManager.ExecuteNuGetProjectActionsAsync(
                    netCorePackageReferenceProjects,
                    actions,
                    testNuGetProjectContext,
                    new SourceCacheContext(),
                    CancellationToken.None);

                // Assert
                Assert.Equal(initialInstalledPackages.Count(), 2);
                var builtIntegratedActions = actions.OfType<BuildIntegratedProjectAction>().ToList();
                Assert.Equal(actions.Length, targetProjects.Count());
                Assert.Equal(actions.Length, builtIntegratedActions.Count);
                // Uprade succeed for this top parent project(no parent but with childs).
                // Keep existing Upgrade/downgrade of individual project logic and making sure that my change is not breaking it.
                Assert.Equal(actions.Length, builtIntegratedActions.Count);
                Assert.True(builtIntegratedActions.All(b => b.RestoreResult.Success));
                Assert.True(builtIntegratedActions.All(b => !b.RestoreResult.LogMessages.Any())); // There should be no error or warning.
                // Make sure top parent project has packageB upgraded version.
                var finalInstalledPackages = (await netCorePackageReferenceProjects[numberOfProjects - 1].GetInstalledPackagesAsync(CancellationToken.None)).ToList();
                Assert.Contains(finalInstalledPackages, f => f.PackageIdentity.Id == packageB_UpgradeVersion.Id
                && f.PackageIdentity.Version == packageB_UpgradeVersion.Version);
                // Make sure middle parent project still have non-upgraded version.
                finalInstalledPackages = (await netCorePackageReferenceProjects[0].GetInstalledPackagesAsync(CancellationToken.None)).ToList();
                Assert.Contains(finalInstalledPackages, f => f.PackageIdentity.Id == packageB.Id
                && f.PackageIdentity.Version == packageB.Version);
                // Make sure bottom project still have non-upgraded version.
                finalInstalledPackages = (await netCorePackageReferenceProjects[0].GetInstalledPackagesAsync(CancellationToken.None)).ToList();
                Assert.Contains(finalInstalledPackages, f => f.PackageIdentity.Id == packageB.Id
                && f.PackageIdentity.Version == packageB.Version);
            }
        }

        [Fact]
        public async Task TestPackageManager_UpgradePackageFor_MiddleParentProject_Success()
        {
            using (var testDirectory = TestDirectory.Create())
            using (var testSolutionManager = new TestSolutionManager())
            {
                // Set up Package Source
                var sources = new List<PackageSource>();
                var packageA_Version100 = new SimpleTestPackageContext("packageA", "1.0.0");
                var packageB_Version100 = new SimpleTestPackageContext("packageB", "1.0.0");
                var packageB_Version200 = new SimpleTestPackageContext("packageB", "2.0.0");
                var packageA = packageA_Version100.Identity;
                var packageB = packageB_Version100.Identity;
                var packageB_UpgradeVersion = packageB_Version200.Identity;
                var packageSource = Path.Combine(testDirectory, "packageSource");
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    packageSource,
                    PackageSaveMode.Defaultv3,
                    packageA_Version100,
                    packageB_Version100,
                    packageB_Version200
                    );

                sources.Add(new PackageSource(packageSource));
                var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(sources);

                // Project
                int numberOfProjects = 4;
                var projectCache = new ProjectSystemCache();
                IVsProjectAdapter projectAdapter = (new Mock<IVsProjectAdapter>()).Object;
                ResolutionContext resolutionContext = (new Mock<ResolutionContext>()).Object;
                var packageSpecs = new PackageSpec[numberOfProjects];
                var projectFullPaths = new string[numberOfProjects];
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    NullSettings.Instance,
                    testSolutionManager,
                    deleteOnRestartManager);

                var testNuGetProjectContext = new TestNuGetProjectContext();
                var netCorePackageReferenceProjects = new List<CpsPackageReferenceProject>();
                var prevProj = string.Empty;
                PackageSpec prevPackageSpec = null;

                // Create projects
                for (var i = numberOfProjects - 1; i >= 0; i--)
                {
                    var projectName = $"project{i}";
                    var projectFullPath = Path.Combine(testDirectory.Path, projectName, projectName + ".csproj");
                    var project = TestCpsPackageReferenceProject.CreateTestCpsPackageReferenceProject(projectName, projectFullPath, projectCache);

                    // We need to treat NU1605 warning as error.
                    project.IsNu1605Error = true;
                    netCorePackageReferenceProjects.Add(project);
                    testSolutionManager.NuGetProjects.Add(project);

                    //Let new project pickup my custom package source.
                    project.ProjectLocalSources.AddRange(sources);
                    var projectNames = GetTestProjectNames(projectFullPath, projectName);
                    var packageSpec = GetPackageSpec(
                        projectName,
                        projectFullPath,
                        packageA_Version100.Version);

                    if (prevPackageSpec != null)
                    {
                        packageSpec = packageSpec.WithTestProjectReference(prevPackageSpec);
                    }

                    // Restore info
                    var projectRestoreInfo = ProjectTestHelpers.GetDGSpecForAllProjects(packageSpec);
                    projectCache.AddProjectRestoreInfo(projectNames, projectRestoreInfo, new List<IAssetsLogMessage>());
                    projectCache.AddProject(projectNames, projectAdapter, project).Should().BeTrue();
                    prevProj = projectFullPath;
                    packageSpecs[i] = packageSpec;
                    prevPackageSpec = packageSpec;
                    projectFullPaths[i] = projectFullPath;
                }

                for (int i = 0; i < numberOfProjects; i++)
                {
                    // Install packageB since packageA is already there.
                    await nuGetPackageManager.InstallPackageAsync(netCorePackageReferenceProjects[i], packageB, new ResolutionContext(), new TestNuGetProjectContext(),
                            sourceRepositoryProvider.GetRepositories(), sourceRepositoryProvider.GetRepositories(), CancellationToken.None);

                    // This code added because nuGetPackageManager.InstallPackageAsync doesn't do updating ProjectSystemCache
                    var installed = new LibraryDependency
                    {
                        LibraryRange = new LibraryRange(
                            name: packageB.Id,
                            versionRange: new VersionRange(packageB.Version),
                            typeConstraint: LibraryDependencyTarget.Package),
                    };

                    var packageSpec = packageSpecs[i];

                    var newDependencies = packageSpec.TargetFrameworks.First().Dependencies.Add(installed);
                    packageSpec.TargetFrameworks[0] = new TargetFrameworkInformation(packageSpec.TargetFrameworks[0]) { Dependencies = newDependencies };

                    var projectRestoreInfo = ProjectTestHelpers.GetDGSpecForAllProjects(packageSpec);
                    var projectNames = GetTestProjectNames(projectFullPaths[i], $"project{i}");
                    projectCache.AddProjectRestoreInfo(projectNames, projectRestoreInfo, new List<IAssetsLogMessage>());
                }

                var initialInstalledPackages = await netCorePackageReferenceProjects[numberOfProjects - 1].GetInstalledPackagesAsync(CancellationToken.None);

                var targetProjects = new List<CpsPackageReferenceProject>()
                {
                    netCorePackageReferenceProjects[numberOfProjects-2] // Middle parent project.
                };

                // Act
                var results = await nuGetPackageManager.PreviewProjectsInstallPackageAsync(
                    targetProjects,
                    packageB_UpgradeVersion,
                    resolutionContext,
                    testNuGetProjectContext,
                    sourceRepositoryProvider.GetRepositories().ToList(),
                    CancellationToken.None);
                var actions = results.Select(a => a.Action).ToArray();
                await nuGetPackageManager.ExecuteNuGetProjectActionsAsync(
                    netCorePackageReferenceProjects,
                    actions,
                    testNuGetProjectContext,
                    new SourceCacheContext(),
                    CancellationToken.None);

                // Assert
                Assert.Equal(initialInstalledPackages.Count(), 2);
                var builtIntegratedActions = actions.OfType<BuildIntegratedProjectAction>().ToList();
                Assert.Equal(actions.Length, targetProjects.Count());
                Assert.Equal(actions.Length, builtIntegratedActions.Count);
                // Upgrade succeed for this middle parent project(with parent and childs).
                // Keep existing Upgrade/downgrade of individual project logic and making sure that my change is not breaking it.
                Assert.Equal(actions.Length, builtIntegratedActions.Count);
                Assert.True(builtIntegratedActions.All(b => b.RestoreResult.Success));
                Assert.True(builtIntegratedActions.All(b => !b.RestoreResult.LogMessages.Any())); // There should be no error or warning.
                // Make sure top parent project still have non-upgraded version.
                var finalInstalledPackages = (await netCorePackageReferenceProjects[numberOfProjects - 1].GetInstalledPackagesAsync(CancellationToken.None)).ToList();
                Assert.Contains(finalInstalledPackages, f => f.PackageIdentity.Id == packageB.Id
                && f.PackageIdentity.Version == packageB.Version);
                // Make sure middle parent project have upgraded version.
                finalInstalledPackages = (await netCorePackageReferenceProjects[numberOfProjects - 2].GetInstalledPackagesAsync(CancellationToken.None)).ToList();
                Assert.Contains(finalInstalledPackages, f => f.PackageIdentity.Id == packageB_UpgradeVersion.Id
                && f.PackageIdentity.Version == packageB_UpgradeVersion.Version);
                // Make sure bottom project still have non-upgraded version.
                finalInstalledPackages = (await netCorePackageReferenceProjects[0].GetInstalledPackagesAsync(CancellationToken.None)).ToList();
                Assert.Contains(finalInstalledPackages, f => f.PackageIdentity.Id == packageB.Id
                && f.PackageIdentity.Version == packageB.Version);
            }
        }

        [Fact]
        public async Task TestPackageManager_UpgradePackageFor_BottomProject_Success()
        {
            using (var testDirectory = TestDirectory.Create())
            using (var testSolutionManager = new TestSolutionManager())
            {
                // Set up Package Source
                var sources = new List<PackageSource>();
                var packageA_Version100 = new SimpleTestPackageContext("packageA", "1.0.0");
                var packageB_Version100 = new SimpleTestPackageContext("packageB", "1.0.0");
                var packageB_Version200 = new SimpleTestPackageContext("packageB", "2.0.0");
                var packageA = packageA_Version100.Identity;
                var packageB = packageB_Version100.Identity;
                var packageB_UpgradeVersion = packageB_Version200.Identity;
                var packageSource = Path.Combine(testDirectory, "packageSource");
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    packageSource,
                    PackageSaveMode.Defaultv3,
                    packageA_Version100,
                    packageB_Version100,
                    packageB_Version200
                    );

                sources.Add(new PackageSource(packageSource));
                var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(sources);

                // Project
                int numberOfProjects = 4;
                var projectCache = new ProjectSystemCache();
                IVsProjectAdapter projectAdapter = (new Mock<IVsProjectAdapter>()).Object;
                ResolutionContext resolutionContext = (new Mock<ResolutionContext>()).Object;
                var packageSpecs = new PackageSpec[numberOfProjects];
                var projectFullPaths = new string[numberOfProjects];
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    NullSettings.Instance,
                    testSolutionManager,
                    deleteOnRestartManager);

                var testNuGetProjectContext = new TestNuGetProjectContext();
                var netCorePackageReferenceProjects = new List<CpsPackageReferenceProject>();
                var prevProj = string.Empty;
                PackageSpec prevPackageSpec = null;

                // Create projects
                for (var i = numberOfProjects - 1; i >= 0; i--)
                {
                    var projectName = $"project{i}";
                    var projectFullPath = Path.Combine(testDirectory.Path, projectName, projectName + ".csproj");
                    var project = TestCpsPackageReferenceProject.CreateTestCpsPackageReferenceProject(projectName, projectFullPath, projectCache);

                    // We need to treat NU1605 warning as error.
                    project.IsNu1605Error = true;
                    netCorePackageReferenceProjects.Add(project);
                    testSolutionManager.NuGetProjects.Add(project);

                    //Let new project pickup my custom package source.
                    project.ProjectLocalSources.AddRange(sources);
                    var projectNames = GetTestProjectNames(projectFullPath, projectName);
                    var packageSpec = GetPackageSpec(
                        projectName,
                        projectFullPath,
                        packageA_Version100.Version);

                    if (prevPackageSpec != null)
                    {
                        packageSpec = packageSpec.WithTestProjectReference(prevPackageSpec);
                    }

                    // Restore info
                    var projectRestoreInfo = ProjectTestHelpers.GetDGSpecForAllProjects(packageSpec);
                    projectCache.AddProjectRestoreInfo(projectNames, projectRestoreInfo, new List<IAssetsLogMessage>());
                    projectCache.AddProject(projectNames, projectAdapter, project).Should().BeTrue();
                    prevProj = projectFullPath;
                    packageSpecs[i] = packageSpec;
                    prevPackageSpec = packageSpec;
                    projectFullPaths[i] = projectFullPath;
                }

                for (int i = 0; i < numberOfProjects; i++)
                {
                    // Install packageB since packageA is already there.
                    await nuGetPackageManager.InstallPackageAsync(netCorePackageReferenceProjects[i], packageB, new ResolutionContext(), new TestNuGetProjectContext(),
                            sourceRepositoryProvider.GetRepositories(), sourceRepositoryProvider.GetRepositories(), CancellationToken.None);

                    // This code added because nuGetPackageManager.InstallPackageAsync doesn't do updating ProjectSystemCache
                    var installed = new LibraryDependency
                    {
                        LibraryRange = new LibraryRange(
                            name: packageB.Id,
                            versionRange: new VersionRange(packageB.Version),
                            typeConstraint: LibraryDependencyTarget.Package),
                    };

                    var packageSpec = packageSpecs[i];

                    var newDependencies = packageSpec.TargetFrameworks.First().Dependencies.Add(installed);
                    packageSpec.TargetFrameworks[0] = new TargetFrameworkInformation(packageSpec.TargetFrameworks[0]) { Dependencies = newDependencies };

                    var projectRestoreInfo = ProjectTestHelpers.GetDGSpecForAllProjects(packageSpec);
                    var projectNames = GetTestProjectNames(projectFullPaths[i], $"project{i}");
                    projectCache.AddProjectRestoreInfo(projectNames, projectRestoreInfo, new List<IAssetsLogMessage>());
                }

                var initialInstalledPackages = await netCorePackageReferenceProjects[numberOfProjects - 1].GetInstalledPackagesAsync(CancellationToken.None);

                var targetProjects = new List<CpsPackageReferenceProject>()
                {
                    netCorePackageReferenceProjects[0] // Bottom child project.
                };

                // Act
                var results = await nuGetPackageManager.PreviewProjectsInstallPackageAsync(
                    targetProjects,
                    packageB_UpgradeVersion,
                    resolutionContext,
                    testNuGetProjectContext,
                    sourceRepositoryProvider.GetRepositories().ToList(),
                    CancellationToken.None);
                var actions = results.Select(a => a.Action).ToArray();
                await nuGetPackageManager.ExecuteNuGetProjectActionsAsync(
                    netCorePackageReferenceProjects,
                    actions,
                    testNuGetProjectContext,
                    new SourceCacheContext(),
                    CancellationToken.None);

                // Assert
                Assert.Equal(initialInstalledPackages.Count(), 2);
                var builtIntegratedActions = actions.OfType<BuildIntegratedProjectAction>().ToList();
                Assert.Equal(actions.Length, targetProjects.Count());
                Assert.Equal(actions.Length, builtIntegratedActions.Count);
                // Upgrade succeed for this bottom project(with parent but no childs).
                // Keep existing Upgrade/downgrade of individual project logic and making sure that my change is not breaking it.
                Assert.Equal(actions.Length, builtIntegratedActions.Count);
                Assert.True(builtIntegratedActions.All(b => b.RestoreResult.Success));
                Assert.True(builtIntegratedActions.All(b => !b.RestoreResult.LogMessages.Any())); // There should be no error or warning.
                // Make sure top parent project still have non-upgraded version.
                var finalInstalledPackages = (await netCorePackageReferenceProjects[numberOfProjects - 1].GetInstalledPackagesAsync(CancellationToken.None)).ToList();
                Assert.Contains(finalInstalledPackages, f => f.PackageIdentity.Id == packageB.Id
                && f.PackageIdentity.Version == packageB.Version);
                // Make sure middle parent project still have non-upgraded version.
                finalInstalledPackages = (await netCorePackageReferenceProjects[numberOfProjects - 2].GetInstalledPackagesAsync(CancellationToken.None)).ToList();
                Assert.Contains(finalInstalledPackages, f => f.PackageIdentity.Id == packageB.Id
                && f.PackageIdentity.Version == packageB.Version);
                // Make sure bottom project have upgraded version.
                finalInstalledPackages = (await netCorePackageReferenceProjects[0].GetInstalledPackagesAsync(CancellationToken.None)).ToList();
                Assert.Contains(finalInstalledPackages, f => f.PackageIdentity.Id == packageB_UpgradeVersion.Id
                && f.PackageIdentity.Version == packageB_UpgradeVersion.Version);
            }
        }

        [Fact]
        public async Task TestPackageManager_DowngradePackageForAllProjects_Success()
        {
            using (var testDirectory = TestDirectory.Create())
            using (var testSolutionManager = new TestSolutionManager())
            {
                // Set up Package Source
                var sources = new List<PackageSource>();
                var packageA_Version100 = new SimpleTestPackageContext("packageA", "1.0.0");
                var packageB_Version100 = new SimpleTestPackageContext("packageB", "1.0.0");
                var packageB_Version200 = new SimpleTestPackageContext("packageB", "2.0.0");
                var packageA = packageA_Version100.Identity;
                var packageB = packageB_Version200.Identity;
                var packageB_DowngradeVersion = packageB_Version100.Identity;
                var packageSource = Path.Combine(testDirectory, "packageSource");
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    packageSource,
                    PackageSaveMode.Defaultv3,
                    packageA_Version100,
                    packageB_Version100,
                    packageB_Version200
                    );

                sources.Add(new PackageSource(packageSource));
                var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(sources);

                // Project
                int numberOfProjects = 4;
                var projectCache = new ProjectSystemCache();
                IVsProjectAdapter projectAdapter = (new Mock<IVsProjectAdapter>()).Object;
                ResolutionContext resolutionContext = (new Mock<ResolutionContext>()).Object;
                var packageSpecs = new PackageSpec[numberOfProjects];
                var projectFullPaths = new string[numberOfProjects];
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    NullSettings.Instance,
                    testSolutionManager,
                    deleteOnRestartManager);

                var testNuGetProjectContext = new TestNuGetProjectContext();
                var netCorePackageReferenceProjects = new List<CpsPackageReferenceProject>();
                var prevProj = string.Empty;
                PackageSpec prevPackageSpec = null;

                // Create projects
                for (var i = numberOfProjects - 1; i >= 0; i--)
                {
                    var projectName = $"project{i}";
                    var projectFullPath = Path.Combine(testDirectory.Path, projectName, projectName + ".csproj");
                    var project = TestCpsPackageReferenceProject.CreateTestCpsPackageReferenceProject(projectName, projectFullPath, projectCache);

                    // We need to treat NU1605 warning as error.
                    project.IsNu1605Error = true;
                    netCorePackageReferenceProjects.Add(project);
                    testSolutionManager.NuGetProjects.Add(project);

                    //Let new project pickup my custom package source.
                    project.ProjectLocalSources.AddRange(sources);
                    var projectNames = GetTestProjectNames(projectFullPath, projectName);
                    var packageSpec = GetPackageSpec(
                        projectName,
                        projectFullPath,
                        packageA_Version100.Version);

                    if (prevPackageSpec != null)
                    {
                        packageSpec = packageSpec.WithTestProjectReference(prevPackageSpec);
                    }

                    // Restore info
                    var projectRestoreInfo = ProjectTestHelpers.GetDGSpecForAllProjects(packageSpec);
                    projectCache.AddProjectRestoreInfo(projectNames, projectRestoreInfo, new List<IAssetsLogMessage>());
                    projectCache.AddProject(projectNames, projectAdapter, project).Should().BeTrue();
                    prevProj = projectFullPath;
                    packageSpecs[i] = packageSpec;
                    prevPackageSpec = packageSpec;
                    projectFullPaths[i] = projectFullPath;
                }

                for (int i = 0; i < numberOfProjects; i++)
                {
                    // Install packageB since packageA is already there.
                    await nuGetPackageManager.InstallPackageAsync(netCorePackageReferenceProjects[i], packageB, new ResolutionContext(), new TestNuGetProjectContext(),
                            sourceRepositoryProvider.GetRepositories(), sourceRepositoryProvider.GetRepositories(), CancellationToken.None);

                    // This code added because nuGetPackageManager.InstallPackageAsync doesn't do updating ProjectSystemCache
                    var installed = new LibraryDependency
                    {
                        LibraryRange = new LibraryRange(
                            name: packageB.Id,
                            versionRange: new VersionRange(packageB.Version),
                            typeConstraint: LibraryDependencyTarget.Package),
                    };

                    var packageSpec = packageSpecs[i];

                    var newDependencies = packageSpec.TargetFrameworks.First().Dependencies.Add(installed);
                    packageSpec.TargetFrameworks[0] = new TargetFrameworkInformation(packageSpec.TargetFrameworks[0]) { Dependencies = newDependencies };

                    var projectRestoreInfo = ProjectTestHelpers.GetDGSpecForAllProjects(packageSpec);
                    var projectNames = GetTestProjectNames(projectFullPaths[i], $"project{i}");
                    projectCache.AddProjectRestoreInfo(projectNames, projectRestoreInfo, new List<IAssetsLogMessage>());
                }

                var initialInstalledPackages = await netCorePackageReferenceProjects[numberOfProjects - 1].GetInstalledPackagesAsync(CancellationToken.None);

                // Act
                var results = await nuGetPackageManager.PreviewProjectsInstallPackageAsync(
                    netCorePackageReferenceProjects, // All projects
                    packageB_DowngradeVersion,
                    resolutionContext,
                    testNuGetProjectContext,
                    sourceRepositoryProvider.GetRepositories().ToList(),
                    CancellationToken.None);

                var actions = results.Select(a => a.Action).ToArray();

                await nuGetPackageManager.ExecuteNuGetProjectActionsAsync(
                    netCorePackageReferenceProjects,
                    actions,
                    testNuGetProjectContext,
                    new SourceCacheContext(),
                    CancellationToken.None);

                // Assert
                Assert.Equal(initialInstalledPackages.Count(), 2);
                var builtIntegratedActions = actions.OfType<BuildIntegratedProjectAction>().ToList();
                Assert.Equal(actions.Length, builtIntegratedActions.Count);
                Assert.True(builtIntegratedActions.All(b => b.RestoreResult.Success));
                Assert.True(builtIntegratedActions.All(b => !b.RestoreResult.LogMessages.Any())); // There should be no error or warning.
                // Make sure all 4 project installed packageB downgrade version.
                foreach (var netCorePackageReferenceProject in netCorePackageReferenceProjects)
                {
                    var finalInstalledPackages = await netCorePackageReferenceProject.GetInstalledPackagesAsync(CancellationToken.None);
                    Assert.Contains(finalInstalledPackages, f => f.PackageIdentity.Id == packageB_DowngradeVersion.Id
                    && f.PackageIdentity.Version == packageB_DowngradeVersion.Version);
                }
            }
        }

        [Fact]
        public async Task TestPackageManager_DowngradePackageFor_TopParentProject_Fail()
        {
            using (var testDirectory = TestDirectory.Create())
            using (var testSolutionManager = new TestSolutionManager())
            {
                // Set up Package Source
                var sources = new List<PackageSource>();
                var packageA_Version100 = new SimpleTestPackageContext("packageA", "1.0.0");
                var packageB_Version100 = new SimpleTestPackageContext("packageB", "1.0.0");
                var packageB_Version200 = new SimpleTestPackageContext("packageB", "2.0.0");
                var packageA = packageA_Version100.Identity;
                var packageB = packageB_Version200.Identity;
                var packageB_DowngradeVersion = packageB_Version100.Identity;
                var packageSource = Path.Combine(testDirectory, "packageSource");
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    packageSource,
                    PackageSaveMode.Defaultv3,
                    packageA_Version100,
                    packageB_Version100,
                    packageB_Version200
                    );

                sources.Add(new PackageSource(packageSource));
                var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(sources);

                // Project
                int numberOfProjects = 4;
                var projectCache = new ProjectSystemCache();
                IVsProjectAdapter projectAdapter = (new Mock<IVsProjectAdapter>()).Object;
                ResolutionContext resolutionContext = (new Mock<ResolutionContext>()).Object;
                var packageSpecs = new PackageSpec[numberOfProjects];
                var projectFullPaths = new string[numberOfProjects];
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    NullSettings.Instance,
                    testSolutionManager,
                    deleteOnRestartManager);

                var testNuGetProjectContext = new TestNuGetProjectContext();
                var netCorePackageReferenceProjects = new List<CpsPackageReferenceProject>();
                var prevProj = string.Empty;
                PackageSpec prevPackageSpec = null;

                // Create projects
                for (var i = numberOfProjects - 1; i >= 0; i--)
                {
                    var projectName = $"project{i}";
                    var projectFullPath = Path.Combine(testDirectory.Path, projectName, projectName + ".csproj");
                    var project = TestCpsPackageReferenceProject.CreateTestCpsPackageReferenceProject(projectName, projectFullPath, projectCache);

                    // We need to treat NU1605 warning as error.
                    project.IsNu1605Error = true;
                    netCorePackageReferenceProjects.Add(project);
                    testSolutionManager.NuGetProjects.Add(project);

                    //Let new project pickup my custom package source.
                    project.ProjectLocalSources.AddRange(sources);
                    var projectNames = GetTestProjectNames(projectFullPath, projectName);
                    var packageSpec = GetPackageSpec(
                        projectName,
                        projectFullPath,
                        packageA_Version100.Version);

                    if (prevPackageSpec != null)
                    {
                        packageSpec = packageSpec.WithTestProjectReference(prevPackageSpec);
                    }

                    // Restore info
                    var projectRestoreInfo = ProjectTestHelpers.GetDGSpecForAllProjects(packageSpec);
                    projectCache.AddProjectRestoreInfo(projectNames, projectRestoreInfo, new List<IAssetsLogMessage>());
                    projectCache.AddProject(projectNames, projectAdapter, project).Should().BeTrue();
                    prevProj = projectFullPath;
                    packageSpecs[i] = packageSpec;
                    prevPackageSpec = packageSpec;
                    projectFullPaths[i] = projectFullPath;
                }

                for (int i = 0; i < numberOfProjects; i++)
                {
                    // Install packageB since packageA is already there.
                    await nuGetPackageManager.InstallPackageAsync(netCorePackageReferenceProjects[i], packageB, new ResolutionContext(), new TestNuGetProjectContext(),
                            sourceRepositoryProvider.GetRepositories(), sourceRepositoryProvider.GetRepositories(), CancellationToken.None);

                    // This code added because nuGetPackageManager.InstallPackageAsync doesn't do updating ProjectSystemCache
                    var installed = new LibraryDependency
                    {
                        LibraryRange = new LibraryRange(
                            name: packageB.Id,
                            versionRange: new VersionRange(packageB.Version),
                            typeConstraint: LibraryDependencyTarget.Package),
                    };

                    var packageSpec = packageSpecs[i];

                    var newDependencies = packageSpec.TargetFrameworks.First().Dependencies.Add(installed);
                    packageSpec.TargetFrameworks[0] = new TargetFrameworkInformation(packageSpec.TargetFrameworks[0]) { Dependencies = newDependencies };

                    var projectRestoreInfo = ProjectTestHelpers.GetDGSpecForAllProjects(packageSpec);
                    var projectNames = GetTestProjectNames(projectFullPaths[i], $"project{i}");
                    projectCache.AddProjectRestoreInfo(projectNames, projectRestoreInfo, new List<IAssetsLogMessage>());
                }

                var initialInstalledPackages = await netCorePackageReferenceProjects[numberOfProjects - 1].GetInstalledPackagesAsync(CancellationToken.None);

                var targetProjects = new List<CpsPackageReferenceProject>()
                {
                    netCorePackageReferenceProjects[numberOfProjects-1] // Top parent project.
                };

                // Act
                var results = await nuGetPackageManager.PreviewProjectsInstallPackageAsync(
                    targetProjects,
                    packageB_DowngradeVersion,
                    resolutionContext,
                    testNuGetProjectContext,
                    sourceRepositoryProvider.GetRepositories().ToList(),
                    CancellationToken.None);
                var actions = results.Select(a => a.Action).ToArray();

                // Assert
                Assert.Equal(initialInstalledPackages.Count(), 2);
                var builtIntegratedActions = actions.OfType<BuildIntegratedProjectAction>().ToList();
                Assert.Equal(actions.Length, targetProjects.Count());
                Assert.Equal(actions.Length, builtIntegratedActions.Count);
                // Downgrade fails for this top parent project(no parent but with childs).
                // Keep existing Upgrade/downgrade of individual project logic and making sure that my change is not breaking it.
                Assert.False(builtIntegratedActions.All(b => b.RestoreResult.Success));
                // Should cause total 1 NU1605 for all childs.
                Assert.Equal(builtIntegratedActions.Sum(b => b.RestoreResult.LogMessages.Count(l => l.Code == NuGetLogCode.NU1605)), 1);
                // There should be no warning other than NU1605.
                Assert.Equal(builtIntegratedActions.Sum(b => b.RestoreResult.LogMessages.Count(l => l.Code != NuGetLogCode.NU1605)), 0);
            }
        }

        [Fact]
        public async Task TestPackageManager_DowngradePackageFor_MiddleParentProject_Fail()
        {
            using (var testDirectory = TestDirectory.Create())
            using (var testSolutionManager = new TestSolutionManager())
            {
                // Set up Package Source
                var sources = new List<PackageSource>();
                var packageA_Version100 = new SimpleTestPackageContext("packageA", "1.0.0");
                var packageB_Version100 = new SimpleTestPackageContext("packageB", "1.0.0");
                var packageB_Version200 = new SimpleTestPackageContext("packageB", "2.0.0");
                var packageA = packageA_Version100.Identity;
                var packageB = packageB_Version200.Identity;
                var packageB_DowngradeVersion = packageB_Version100.Identity;
                var packageSource = Path.Combine(testDirectory, "packageSource");
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    packageSource,
                    PackageSaveMode.Defaultv3,
                    packageA_Version100,
                    packageB_Version100,
                    packageB_Version200
                    );

                sources.Add(new PackageSource(packageSource));
                var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(sources);

                // Project
                int numberOfProjects = 4;
                var projectCache = new ProjectSystemCache();
                IVsProjectAdapter projectAdapter = (new Mock<IVsProjectAdapter>()).Object;
                ResolutionContext resolutionContext = (new Mock<ResolutionContext>()).Object;
                var packageSpecs = new PackageSpec[numberOfProjects];
                var projectFullPaths = new string[numberOfProjects];
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    NullSettings.Instance,
                    testSolutionManager,
                    deleteOnRestartManager);

                var testNuGetProjectContext = new TestNuGetProjectContext();
                var netCorePackageReferenceProjects = new List<CpsPackageReferenceProject>();
                var prevProj = string.Empty;
                PackageSpec prevPackageSpec = null;

                // Create projects
                for (var i = numberOfProjects - 1; i >= 0; i--)
                {
                    var projectName = $"project{i}";
                    var projectFullPath = Path.Combine(testDirectory.Path, projectName, projectName + ".csproj");
                    var project = TestCpsPackageReferenceProject.CreateTestCpsPackageReferenceProject(projectName, projectFullPath, projectCache);

                    // We need to treat NU1605 warning as error.
                    project.IsNu1605Error = true;
                    netCorePackageReferenceProjects.Add(project);
                    testSolutionManager.NuGetProjects.Add(project);

                    //Let new project pickup my custom package source.
                    project.ProjectLocalSources.AddRange(sources);
                    var projectNames = GetTestProjectNames(projectFullPath, projectName);
                    var packageSpec = GetPackageSpec(
                        projectName,
                        projectFullPath,
                        packageA_Version100.Version);

                    if (prevPackageSpec != null)
                    {
                        packageSpec = packageSpec.WithTestProjectReference(prevPackageSpec);
                    }

                    // Restore info
                    var projectRestoreInfo = ProjectTestHelpers.GetDGSpecForAllProjects(packageSpec);
                    projectCache.AddProjectRestoreInfo(projectNames, projectRestoreInfo, new List<IAssetsLogMessage>());
                    projectCache.AddProject(projectNames, projectAdapter, project).Should().BeTrue();
                    prevProj = projectFullPath;
                    packageSpecs[i] = packageSpec;
                    prevPackageSpec = packageSpec;
                    projectFullPaths[i] = projectFullPath;
                }

                for (int i = 0; i < numberOfProjects; i++)
                {
                    // Install packageB since packageA is already there.
                    await nuGetPackageManager.InstallPackageAsync(netCorePackageReferenceProjects[i], packageB, new ResolutionContext(), new TestNuGetProjectContext(),
                            sourceRepositoryProvider.GetRepositories(), sourceRepositoryProvider.GetRepositories(), CancellationToken.None);

                    // This code added because nuGetPackageManager.InstallPackageAsync doesn't do updating ProjectSystemCache here
                    var installed = new LibraryDependency
                    {
                        LibraryRange = new LibraryRange(
                            name: packageB.Id,
                            versionRange: new VersionRange(packageB.Version),
                            typeConstraint: LibraryDependencyTarget.Package),
                    };

                    var packageSpec = packageSpecs[i];

                    var newDependencies = packageSpec.TargetFrameworks.First().Dependencies.Add(installed);
                    packageSpec.TargetFrameworks[0] = new TargetFrameworkInformation(packageSpec.TargetFrameworks[0]) { Dependencies = newDependencies };

                    var projectRestoreInfo = ProjectTestHelpers.GetDGSpecForAllProjects(packageSpec);
                    var projectNames = GetTestProjectNames(projectFullPaths[i], $"project{i}");
                    projectCache.AddProjectRestoreInfo(projectNames, projectRestoreInfo, new List<IAssetsLogMessage>());
                }

                var initialInstalledPackages = await netCorePackageReferenceProjects[numberOfProjects - 2].GetInstalledPackagesAsync(CancellationToken.None);

                var targetProjects = new List<CpsPackageReferenceProject>()
                {
                    netCorePackageReferenceProjects[numberOfProjects-2] // Middle parent project.
                };

                // Act
                var results = await nuGetPackageManager.PreviewProjectsInstallPackageAsync(
                    targetProjects,
                    packageB_DowngradeVersion,
                    resolutionContext,
                    testNuGetProjectContext,
                    sourceRepositoryProvider.GetRepositories().ToList(),
                    CancellationToken.None);
                var actions = results.Select(a => a.Action).ToArray();

                // Assert
                Assert.Equal(initialInstalledPackages.Count(), 2);
                var builtIntegratedActions = actions.OfType<BuildIntegratedProjectAction>().ToList();
                Assert.Equal(actions.Length, targetProjects.Count);
                Assert.Equal(actions.Length, builtIntegratedActions.Count);
                // Downgrade fails for this middle parent project(with both parent and child).
                // Keep existing Upgrade/downgrade of individual project logic and making sure that my change is not breaking it.
                Assert.False(builtIntegratedActions.All(b => b.RestoreResult.Success));
                // Should cause total 1 NU1605 for all childs.
                Assert.Equal(builtIntegratedActions.Sum(b => b.RestoreResult.LogMessages.Count(l => l.Code == NuGetLogCode.NU1605)), 1);
                // There should be no warning other than NU1605.
                Assert.Equal(builtIntegratedActions.Sum(b => b.RestoreResult.LogMessages.Count(l => l.Code != NuGetLogCode.NU1605)), 0);
            }
        }

        [Fact]
        public async Task TestPackageManager_DowngradePackageFor_BottomtProject_Success()
        {
            using (var testDirectory = TestDirectory.Create())
            using (var testSolutionManager = new TestSolutionManager())
            {
                // Set up Package Source
                var sources = new List<PackageSource>();
                var packageA_Version100 = new SimpleTestPackageContext("packageA", "1.0.0");
                var packageB_Version100 = new SimpleTestPackageContext("packageB", "1.0.0");
                var packageB_Version200 = new SimpleTestPackageContext("packageB", "2.0.0");
                var packageA = packageA_Version100.Identity;
                var packageB = packageB_Version200.Identity;
                var packageB_DowngradeVersion = packageB_Version100.Identity;
                var packageSource = Path.Combine(testDirectory, "packageSource");
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    packageSource,
                    PackageSaveMode.Defaultv3,
                    packageA_Version100,
                    packageB_Version100,
                    packageB_Version200
                    );

                sources.Add(new PackageSource(packageSource));
                var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(sources);

                // Project
                int numberOfProjects = 4;
                var projectCache = new ProjectSystemCache();
                IVsProjectAdapter projectAdapter = (new Mock<IVsProjectAdapter>()).Object;
                ResolutionContext resolutionContext = (new Mock<ResolutionContext>()).Object;
                var packageSpecs = new PackageSpec[numberOfProjects];
                var projectFullPaths = new string[numberOfProjects];
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    NullSettings.Instance,
                    testSolutionManager,
                    deleteOnRestartManager);

                var testNuGetProjectContext = new TestNuGetProjectContext();
                var netCorePackageReferenceProjects = new List<CpsPackageReferenceProject>();
                var prevProj = string.Empty;
                PackageSpec prevPackageSpec = null;

                // Create projects
                for (var i = numberOfProjects - 1; i >= 0; i--)
                {
                    var projectName = $"project{i}";
                    var projectFullPath = Path.Combine(testDirectory.Path, projectName, projectName + ".csproj");
                    var project = TestCpsPackageReferenceProject.CreateTestCpsPackageReferenceProject(projectName, projectFullPath, projectCache);

                    // We need to treat NU1605 warning as error.
                    project.IsNu1605Error = true;
                    netCorePackageReferenceProjects.Add(project);
                    testSolutionManager.NuGetProjects.Add(project);
                    //Let new project pickup my custom package source.
                    project.ProjectLocalSources.AddRange(sources);
                    var projectNames = GetTestProjectNames(projectFullPath, projectName);
                    var packageSpec = GetPackageSpec(
                        projectName,
                        projectFullPath,
                        packageA_Version100.Version);

                    if (prevPackageSpec != null)
                    {
                        packageSpec = packageSpec.WithTestProjectReference(prevPackageSpec);
                    }

                    // Restore info
                    var projectRestoreInfo = ProjectTestHelpers.GetDGSpecForAllProjects(packageSpec);
                    projectCache.AddProjectRestoreInfo(projectNames, projectRestoreInfo, new List<IAssetsLogMessage>());
                    projectCache.AddProject(projectNames, projectAdapter, project).Should().BeTrue();
                    prevProj = projectFullPath;
                    packageSpecs[i] = packageSpec;
                    prevPackageSpec = packageSpec;
                    projectFullPaths[i] = projectFullPath;
                }

                for (int i = 0; i < numberOfProjects; i++)
                {
                    // Install packageB since packageA is already there.
                    await nuGetPackageManager.InstallPackageAsync(netCorePackageReferenceProjects[i], packageB, new ResolutionContext(), new TestNuGetProjectContext(),
                            sourceRepositoryProvider.GetRepositories(), sourceRepositoryProvider.GetRepositories(), CancellationToken.None);

                    // This code added because nuGetPackageManager.InstallPackageAsync doesn't do updating ProjectSystemCache here
                    var installed = new LibraryDependency
                    {
                        LibraryRange = new LibraryRange(
                            name: packageB.Id,
                            versionRange: new VersionRange(packageB.Version),
                            typeConstraint: LibraryDependencyTarget.Package),
                    };

                    var packageSpec = packageSpecs[i];

                    var newDependencies = packageSpec.TargetFrameworks.First().Dependencies.Add(installed);
                    packageSpec.TargetFrameworks[0] = new TargetFrameworkInformation(packageSpec.TargetFrameworks[0]) { Dependencies = newDependencies };

                    var projectRestoreInfo = ProjectTestHelpers.GetDGSpecForAllProjects(packageSpec);
                    var projectNames = GetTestProjectNames(projectFullPaths[i], $"project{i}");
                    projectCache.AddProjectRestoreInfo(projectNames, projectRestoreInfo, new List<IAssetsLogMessage>());
                }

                var initialInstalledPackages = await netCorePackageReferenceProjects[numberOfProjects - 2].GetInstalledPackagesAsync(CancellationToken.None);

                var targetProjects = new List<CpsPackageReferenceProject>()
                {
                    netCorePackageReferenceProjects[0] // Bottom child project.
                };

                // Act
                var results = await nuGetPackageManager.PreviewProjectsInstallPackageAsync(
                    targetProjects,
                    packageB_DowngradeVersion,
                    resolutionContext,
                    testNuGetProjectContext,
                    sourceRepositoryProvider.GetRepositories().ToList(),
                    CancellationToken.None);
                var actions = results.Select(a => a.Action).ToArray();
                await nuGetPackageManager.ExecuteNuGetProjectActionsAsync(
                    netCorePackageReferenceProjects,
                    actions,
                    testNuGetProjectContext,
                    new SourceCacheContext(),
                    CancellationToken.None);

                // Assert
                Assert.Equal(initialInstalledPackages.Count(), 2);
                var builtIntegratedActions = actions.OfType<BuildIntegratedProjectAction>().ToList();
                Assert.Equal(actions.Length, targetProjects.Count);
                Assert.Equal(actions.Length, builtIntegratedActions.Count);
                Assert.True(builtIntegratedActions.All(b => b.RestoreResult.Success));
                // There should be no error/warnings
                Assert.Equal(builtIntegratedActions.Sum(b => b.RestoreResult.LogMessages.Count()), 0);
                // Make sure top parent project still have non-downgraded version.
                var finalInstalledPackages = (await netCorePackageReferenceProjects[numberOfProjects - 1].GetInstalledPackagesAsync(CancellationToken.None)).ToList();
                Assert.Contains(finalInstalledPackages, f => f.PackageIdentity.Id == packageB.Id
                && f.PackageIdentity.Version == packageB.Version);
                // Make sure middle parent project still have non-downgraded version.
                finalInstalledPackages = (await netCorePackageReferenceProjects[numberOfProjects - 2].GetInstalledPackagesAsync(CancellationToken.None)).ToList();
                Assert.Contains(finalInstalledPackages, f => f.PackageIdentity.Id == packageB.Id
                && f.PackageIdentity.Version == packageB.Version);
                // Make sure bottom project have downgraded version.
                finalInstalledPackages = (await netCorePackageReferenceProjects[0].GetInstalledPackagesAsync(CancellationToken.None)).ToList();
                Assert.Contains(finalInstalledPackages, f => f.PackageIdentity.Id == packageB_DowngradeVersion.Id
                && f.PackageIdentity.Version == packageB_DowngradeVersion.Version);
            }
        }

        [Fact]
        [UseCulture("en-US")] // We are asserting exception messages in English
        public async Task TestPackageManager_CancellationTokenPassed()
        {
            using (var testDirectory = TestDirectory.Create())
            using (var testSolutionManager = new TestSolutionManager())
            {
                // Set up Package Source
                var sources = new List<PackageSource>();
                var packageA_Version100 = new SimpleTestPackageContext("packageA", "1.0.0");
                var packageA = packageA_Version100.Identity;
                var packageSource = Path.Combine(testDirectory, "packageSource");
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    packageSource,
                    PackageSaveMode.Defaultv3,
                    packageA_Version100
                    );

                sources.Add(new PackageSource(packageSource));
                var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(sources);

                // Project
                var projectCache = new ProjectSystemCache();
                IVsProjectAdapter projectAdapter = (new Mock<IVsProjectAdapter>()).Object;
                ResolutionContext resolutionContext = (new Mock<ResolutionContext>()).Object;
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    NullSettings.Instance,
                    testSolutionManager,
                    deleteOnRestartManager);

                var testNuGetProjectContext = new TestNuGetProjectContext();
                var netCorePackageReferenceProjects = new List<CpsPackageReferenceProject>();
                var prevProj = string.Empty;
                PackageSpec prevPackageSpec = null;

                var source = new CancellationTokenSource();
                var token = source.Token;
                // Create projects
                for (var i = 1; i >= 0; i--)
                {
                    var projectName = $"project{i}";
                    var projectFullPath = Path.Combine(testDirectory.Path, projectName, projectName + ".csproj");
                    var project = TestCpsPackageReferenceProject.CreateTestCpsPackageReferenceProject(projectName, projectFullPath, projectCache);

                    // We need to treat NU1605 warning as error.
                    //project.IsNu1605Error = true;
                    netCorePackageReferenceProjects.Add(project);
                    testSolutionManager.NuGetProjects.Add(project);
                    //Let new project pickup my custom package source.
                    project.ProjectLocalSources.AddRange(sources);
                    var projectNames = GetTestProjectNames(projectFullPath, projectName);
                    var packageSpec = GetPackageSpec(projectName, projectFullPath, packageA_Version100.Version);

                    if (prevPackageSpec != null)
                    {
                        packageSpec = packageSpec.WithTestProjectReference(prevPackageSpec);
                    }

                    // Restore info
                    var projectRestoreInfo = ProjectTestHelpers.GetDGSpecForAllProjects(packageSpec);
                    projectCache.AddProjectRestoreInfo(projectNames, projectRestoreInfo, new List<IAssetsLogMessage>());
                    projectCache.AddProject(projectNames, projectAdapter, project).Should().BeTrue();
                    prevProj = projectFullPath;
                    prevPackageSpec = packageSpec;
                }

                // Act
                source.Cancel();

                var exception = await Assert.ThrowsAsync<OperationCanceledException>(
                    () => nuGetPackageManager.PreviewProjectsInstallPackageAsync(
                        netCorePackageReferenceProjects,
                        packageA,
                        resolutionContext,
                        testNuGetProjectContext,
                        sourceRepositoryProvider.GetRepositories().ToList(),
                        token));

                // Assert
                Assert.NotNull(exception);
                Assert.Equal(exception.Message, "The operation was canceled.");
            }
        }

        [Fact]
        public async Task TestPackageManager_RaiseTelemetryEvents()
        {
            // set up telemetry service
            var telemetrySession = new Mock<ITelemetrySession>();
            var telemetryEvents = new ConcurrentQueue<TelemetryEvent>();
            telemetrySession
                .Setup(x => x.PostEvent(It.IsAny<TelemetryEvent>()))
                .Callback<TelemetryEvent>(x => telemetryEvents.Enqueue(x));
            var telemetryService = new NuGetVSTelemetryService(telemetrySession.Object);
            TelemetryActivity.NuGetTelemetryService = telemetryService;

            using (var testDirectory = TestDirectory.Create())
            using (var testSolutionManager = new TestSolutionManager())
            {
                // Set up Package Source
                var sources = new List<PackageSource>();
                var packageA_Version100 = new SimpleTestPackageContext("packageA", "1.0.0");
                var packageB_Version100 = new SimpleTestPackageContext("packageB", "1.0.0");
                var packageA = packageA_Version100.Identity;
                var packageB = packageB_Version100.Identity;
                var packageSource = Path.Combine(testDirectory, "packageSource");
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    packageSource,
                    PackageSaveMode.Defaultv3,
                    packageA_Version100,
                    packageB_Version100
                    );

                sources.Add(new PackageSource(packageSource));
                var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(sources);

                // Project
                int numberOfProjects = 4;
                var projectCache = new ProjectSystemCache();
                IVsProjectAdapter projectAdapter = (new Mock<IVsProjectAdapter>()).Object;
                ResolutionContext resolutionContext = (new Mock<ResolutionContext>()).Object;
                var packageSpecs = new PackageSpec[numberOfProjects];
                var projectFullPaths = new string[numberOfProjects];
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    NullSettings.Instance,
                    testSolutionManager,
                    deleteOnRestartManager);

                var testNuGetProjectContext = new TestNuGetProjectContext();
                var netCorePackageReferenceProjects = new List<CpsPackageReferenceProject>();
                var prevProj = string.Empty;
                PackageSpec prevPackageSpec = null;

                // Create projects
                for (var i = numberOfProjects - 1; i >= 0; i--)
                {
                    var projectName = $"project{i}";
                    var projectFullPath = Path.Combine(testDirectory.Path, projectName, projectName + ".csproj");
                    var project = TestCpsPackageReferenceProject.CreateTestCpsPackageReferenceProject(projectName, projectFullPath, projectCache);

                    // We need to treat NU1605 warning as error.
                    project.IsNu1605Error = true;
                    netCorePackageReferenceProjects.Add(project);
                    testSolutionManager.NuGetProjects.Add(project);

                    //Let new project pickup my custom package source.
                    project.ProjectLocalSources.AddRange(sources);
                    var projectNames = GetTestProjectNames(projectFullPath, projectName);
                    var packageSpec = GetPackageSpec(
                        projectName,
                        projectFullPath,
                        packageA_Version100.Version);

                    if (prevPackageSpec != null)
                    {
                        packageSpec = packageSpec.WithTestProjectReference(prevPackageSpec);
                    }

                    // Restore info
                    var projectRestoreInfo = ProjectTestHelpers.GetDGSpecForAllProjects(packageSpec);
                    projectCache.AddProjectRestoreInfo(projectNames, projectRestoreInfo, new List<IAssetsLogMessage>());
                    projectCache.AddProject(projectNames, projectAdapter, project).Should().BeTrue();
                    prevProj = projectFullPath;
                    packageSpecs[i] = packageSpec;
                    prevPackageSpec = packageSpec;
                    projectFullPaths[i] = projectFullPath;
                }

                var initialInstalledPackages = await netCorePackageReferenceProjects[numberOfProjects - 1].GetInstalledPackagesAsync(CancellationToken.None);

                // Act
                var results = await nuGetPackageManager.PreviewProjectsInstallPackageAsync(
                    netCorePackageReferenceProjects, // All projects
                    packageB,
                    resolutionContext,
                    testNuGetProjectContext,
                    sourceRepositoryProvider.GetRepositories().ToList(),
                    CancellationToken.None);

                var actions = results.Select(a => a.Action).ToArray();

                await nuGetPackageManager.ExecuteNuGetProjectActionsAsync(
                    netCorePackageReferenceProjects,
                    actions,
                    testNuGetProjectContext,
                    new SourceCacheContext(),
                    CancellationToken.None);

                // Assert
                Assert.True(telemetryEvents.Count > 1);
                var actionTelemetryStepEvents = telemetryEvents.OfType<ActionTelemetryStepEvent>();
                Assert.Contains(actionTelemetryStepEvents, t => t.SubStepName.Contains("Preview build integrated action time"));
                var builtIntegratedActions = actions.OfType<BuildIntegratedProjectAction>().ToList();
                Assert.Equal(actions.Length, netCorePackageReferenceProjects.Count());
                Assert.Equal(actions.Length, builtIntegratedActions.Count);
                Assert.True(builtIntegratedActions.All(b => b.RestoreResult.Success));
            }
        }

        [Fact]
        public async Task TestPackageManager_UninstallPackageFor_TopAndMidParentProject_Success()
        {
            using (var testDirectory = TestDirectory.Create())
            using (var testSolutionManager = new TestSolutionManager())
            {
                // Set up Package Source
                var sources = new List<PackageSource>();
                var packageA_Version100 = new SimpleTestPackageContext("packageA", "1.0.0");
                var packageB_Version100 = new SimpleTestPackageContext("packageB", "1.0.0");
                var packageA = packageA_Version100.Identity;
                var packageB = packageB_Version100.Identity;
                var packageSource = Path.Combine(testDirectory, "packageSource");
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    packageSource,
                    PackageSaveMode.Defaultv3,
                    packageA_Version100,
                    packageB_Version100
                    );

                sources.Add(new PackageSource(packageSource));
                var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(sources);

                // Project
                int numberOfProjects = 3;
                var projectCache = new ProjectSystemCache();
                IVsProjectAdapter projectAdapter = (new Mock<IVsProjectAdapter>()).Object;
                ResolutionContext resolutionContext = (new Mock<ResolutionContext>()).Object;
                var packageSpecs = new PackageSpec[numberOfProjects];
                var projectFullPaths = new string[numberOfProjects];
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    NullSettings.Instance,
                    testSolutionManager,
                    deleteOnRestartManager);

                var testNuGetProjectContext = new TestNuGetProjectContext() { EnableLogging = true };
                var netCorePackageReferenceProjects = new List<CpsPackageReferenceProject>();
                var prevProj = string.Empty;
                PackageSpec prevPackageSpec = null;

                // Create projects
                for (var i = numberOfProjects - 1; i >= 0; i--)
                {
                    var projectName = $"project{i}";
                    var projectFullPath = Path.Combine(testDirectory.Path, projectName, projectName + ".csproj");
                    var project = TestCpsPackageReferenceProject.CreateTestCpsPackageReferenceProject(projectName, projectFullPath, projectCache);

                    // We need to treat NU1605 warning as error.
                    project.IsNu1605Error = true;
                    netCorePackageReferenceProjects.Add(project);
                    testSolutionManager.NuGetProjects.Add(project);

                    //Let new project pickup my custom package source.
                    project.ProjectLocalSources.AddRange(sources);
                    var projectNames = GetTestProjectNames(projectFullPath, projectName);
                    var packageSpec = GetPackageSpec(
                        projectName,
                        projectFullPath,
                        packageA_Version100.Version);

                    if (prevPackageSpec != null)
                    {
                        packageSpec = packageSpec.WithTestProjectReference(prevPackageSpec);
                    }

                    // Restore info
                    var projectRestoreInfo = ProjectTestHelpers.GetDGSpecForAllProjects(packageSpec);
                    projectCache.AddProjectRestoreInfo(projectNames, projectRestoreInfo, new List<IAssetsLogMessage>());
                    projectCache.AddProject(projectNames, projectAdapter, project).Should().BeTrue();
                    prevProj = projectFullPath;
                    packageSpecs[i] = packageSpec;
                    prevPackageSpec = packageSpec;
                    projectFullPaths[i] = projectFullPath;
                }

                for (int i = 0; i < numberOfProjects; i++)
                {
                    // Install packageB since packageA is already there.
                    await nuGetPackageManager.InstallPackageAsync(netCorePackageReferenceProjects[i], packageB, new ResolutionContext(), new TestNuGetProjectContext(),
                            sourceRepositoryProvider.GetRepositories(), sourceRepositoryProvider.GetRepositories(), CancellationToken.None);

                    // This code added because nuGetPackageManager.InstallPackageAsync doesn't do updating ProjectSystemCache
                    var installed = new LibraryDependency
                    {
                        LibraryRange = new LibraryRange(
                            name: packageB.Id,
                            versionRange: new VersionRange(packageB.Version),
                            typeConstraint: LibraryDependencyTarget.Package),
                    };

                    var packageSpec = packageSpecs[i];

                    var newDependencies = packageSpec.TargetFrameworks.First().Dependencies.Add(installed);
                    packageSpec.TargetFrameworks[0] = new TargetFrameworkInformation(packageSpec.TargetFrameworks[0]) { Dependencies = newDependencies };

                    var projectRestoreInfo = ProjectTestHelpers.GetDGSpecForAllProjects(packageSpec);
                    var projectNames = GetTestProjectNames(projectFullPaths[i], $"project{i}");
                    projectCache.AddProjectRestoreInfo(projectNames, projectRestoreInfo, new List<IAssetsLogMessage>());
                }

                var initialInstalledPackages = await netCorePackageReferenceProjects[numberOfProjects - 1].GetInstalledPackagesAsync(CancellationToken.None);

                var targetProjects = new List<CpsPackageReferenceProject>()
                {
                    netCorePackageReferenceProjects[numberOfProjects-1], // Top parent project.
                    netCorePackageReferenceProjects[numberOfProjects-2]  // Middle parent project.
                };

                // Act
                var uninstallationContext = new UninstallationContext(
                    removeDependencies: false,
                    forceRemove: false);

                var results = new List<ResolvedAction>();
                foreach (var target in targetProjects)
                {
                    IEnumerable<NuGetProjectAction> resolvedActions;

                    resolvedActions = await nuGetPackageManager.PreviewUninstallPackageAsync(
                        target, packageB.Id, uninstallationContext, testNuGetProjectContext, CancellationToken.None);

                    results.AddRange(resolvedActions.Select(a => new ResolvedAction(target, a)));
                }

                var actions = results.Select(a => a.Action).ToArray();

                await nuGetPackageManager.ExecuteNuGetProjectActionsAsync(
                    netCorePackageReferenceProjects,
                    actions,
                    testNuGetProjectContext,
                    new SourceCacheContext(),
                    CancellationToken.None);

                // Assert
                Assert.Equal(initialInstalledPackages.Count(), 2);
                var builtIntegratedActions = actions.OfType<BuildIntegratedProjectAction>().ToList();
                Assert.Equal(actions.Length, targetProjects.Count());
                Assert.Equal(actions.Length, builtIntegratedActions.Count);
                Assert.True(builtIntegratedActions.All(b => b.RestoreResult.Success));
                var restoringLogs = testNuGetProjectContext.Logs.Value.Where(l => l.StartsWith("Restoring packages for ")).ToList();
                var restoredLogs = testNuGetProjectContext.Logs.Value.Where(l => l.StartsWith("Restored ")).ToList();
                // Making sure project0 restored only once, not many.
                // https://github.com/NuGet/Home/issues/9932
                Assert.Equal(restoringLogs.Count(l => l.EndsWith("project0.csproj...")), 1);
                Assert.Equal(restoredLogs.Count(l => l.Contains("project0.csproj")), 1);
                // Making sure project1 restored only once, not many. 
                Assert.Equal(restoringLogs.Count(l => l.EndsWith("project1.csproj...")), 1);
                Assert.Equal(restoredLogs.Count(l => l.Contains("project1.csproj")), 1);
                var writingAssetsLogs = testNuGetProjectContext.Logs.Value.Where(l => l.StartsWith("Writing assets file to disk.")).ToList();
                // Only 2 write to assets for above 2 projects, not more than that.
                Assert.Equal(writingAssetsLogs.Count, 2);
                // There should be no warning/error.
                Assert.Equal(builtIntegratedActions.Sum(b => b.RestoreResult.LogMessages.Count()), 0);
            }
        }

        [Fact]
        public async Task TestPackageManager_UninstallPackageFor_MidParentAndBottomProject_Success()
        {
            using (var testDirectory = TestDirectory.Create())
            using (var testSolutionManager = new TestSolutionManager())
            {
                // Set up Package Source
                var sources = new List<PackageSource>();
                var packageA_Version100 = new SimpleTestPackageContext("packageA", "1.0.0");
                var packageB_Version100 = new SimpleTestPackageContext("packageB", "1.0.0");
                var packageA = packageA_Version100.Identity;
                var packageB = packageB_Version100.Identity;
                var packageSource = Path.Combine(testDirectory, "packageSource");
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    packageSource,
                    PackageSaveMode.Defaultv3,
                    packageA_Version100,
                    packageB_Version100
                    );

                sources.Add(new PackageSource(packageSource));
                var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(sources);

                // Project
                int numberOfProjects = 3;
                var projectCache = new ProjectSystemCache();
                IVsProjectAdapter projectAdapter = (new Mock<IVsProjectAdapter>()).Object;
                ResolutionContext resolutionContext = (new Mock<ResolutionContext>()).Object;
                var packageSpecs = new PackageSpec[numberOfProjects];
                var projectFullPaths = new string[numberOfProjects];
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    NullSettings.Instance,
                    testSolutionManager,
                    deleteOnRestartManager);

                var testNuGetProjectContext = new TestNuGetProjectContext() { EnableLogging = true };
                var netCorePackageReferenceProjects = new List<CpsPackageReferenceProject>();
                var prevProj = string.Empty;
                PackageSpec prevPackageSpec = null;

                // Create projects
                for (var i = numberOfProjects - 1; i >= 0; i--)
                {
                    var projectName = $"project{i}";
                    var projectFullPath = Path.Combine(testDirectory.Path, projectName, projectName + ".csproj");
                    var project = TestCpsPackageReferenceProject.CreateTestCpsPackageReferenceProject(projectName, projectFullPath, projectCache);

                    // We need to treat NU1605 warning as error.
                    project.IsNu1605Error = true;
                    netCorePackageReferenceProjects.Add(project);
                    testSolutionManager.NuGetProjects.Add(project);

                    //Let new project pickup my custom package source.
                    project.ProjectLocalSources.AddRange(sources);
                    var projectNames = GetTestProjectNames(projectFullPath, projectName);
                    var packageSpec = GetPackageSpec(
                        projectName,
                        projectFullPath,
                        packageA_Version100.Version);

                    if (prevPackageSpec != null)
                    {
                        packageSpec = packageSpec.WithTestProjectReference(prevPackageSpec);
                    }

                    // Restore info
                    var projectRestoreInfo = ProjectTestHelpers.GetDGSpecForAllProjects(packageSpec);
                    projectCache.AddProjectRestoreInfo(projectNames, projectRestoreInfo, new List<IAssetsLogMessage>());
                    projectCache.AddProject(projectNames, projectAdapter, project).Should().BeTrue();
                    prevProj = projectFullPath;
                    packageSpecs[i] = packageSpec;
                    prevPackageSpec = packageSpec;
                    projectFullPaths[i] = projectFullPath;
                }

                for (int i = 0; i < numberOfProjects; i++)
                {
                    // Install packageB since packageA is already there.
                    await nuGetPackageManager.InstallPackageAsync(netCorePackageReferenceProjects[i], packageB, new ResolutionContext(), new TestNuGetProjectContext(),
                            sourceRepositoryProvider.GetRepositories(), sourceRepositoryProvider.GetRepositories(), CancellationToken.None);

                    // This code added because nuGetPackageManager.InstallPackageAsync doesn't do updating ProjectSystemCache
                    var installed = new LibraryDependency
                    {
                        LibraryRange = new LibraryRange(
                            name: packageB.Id,
                            versionRange: new VersionRange(packageB.Version),
                            typeConstraint: LibraryDependencyTarget.Package),
                    };

                    var packageSpec = packageSpecs[i];

                    var newDependencies = packageSpec.TargetFrameworks.First().Dependencies.Add(installed);
                    packageSpec.TargetFrameworks[0] = new TargetFrameworkInformation(packageSpec.TargetFrameworks[0]) { Dependencies = newDependencies };

                    var projectRestoreInfo = ProjectTestHelpers.GetDGSpecForAllProjects(packageSpec);
                    var projectNames = GetTestProjectNames(projectFullPaths[i], $"project{i}");
                    projectCache.AddProjectRestoreInfo(projectNames, projectRestoreInfo, new List<IAssetsLogMessage>());
                }

                var initialInstalledPackages = await netCorePackageReferenceProjects[numberOfProjects - 1].GetInstalledPackagesAsync(CancellationToken.None);

                var targetProjects = new List<CpsPackageReferenceProject>()
                {
                    netCorePackageReferenceProjects[numberOfProjects-2], // Middle parent project.
                    netCorePackageReferenceProjects[numberOfProjects-3]  // Bottom child project.
                };

                // Act
                var uninstallationContext = new UninstallationContext(
                    removeDependencies: false,
                    forceRemove: false);

                var results = new List<ResolvedAction>();
                foreach (var target in targetProjects)
                {
                    IEnumerable<NuGetProjectAction> resolvedActions;

                    resolvedActions = await nuGetPackageManager.PreviewUninstallPackageAsync(
                        target, packageB.Id, uninstallationContext, testNuGetProjectContext, CancellationToken.None);

                    results.AddRange(resolvedActions.Select(a => new ResolvedAction(target, a)));
                }

                var actions = results.Select(a => a.Action).ToArray();

                await nuGetPackageManager.ExecuteNuGetProjectActionsAsync(
                    netCorePackageReferenceProjects,
                    actions,
                    testNuGetProjectContext,
                    new SourceCacheContext(),
                    CancellationToken.None);

                // Assert
                Assert.Equal(initialInstalledPackages.Count(), 2);
                var builtIntegratedActions = actions.OfType<BuildIntegratedProjectAction>().ToList();
                Assert.Equal(actions.Length, targetProjects.Count());
                Assert.Equal(actions.Length, builtIntegratedActions.Count);
                Assert.True(builtIntegratedActions.All(b => b.RestoreResult.Success));
                var restoringLogs = testNuGetProjectContext.Logs.Value.Where(l => l.StartsWith("Restoring packages for ")).ToList();
                var restoredLogs = testNuGetProjectContext.Logs.Value.Where(l => l.StartsWith("Restored ")).ToList();
                // Making sure project1 restored only once, not many.
                // https://github.com/NuGet/Home/issues/9932
                Assert.Equal(restoringLogs.Count(l => l.EndsWith("project1.csproj...")), 1);
                Assert.Equal(restoredLogs.Count(l => l.Contains("project1.csproj")), 1);
                // Making sure project2 restored only once, not many. 
                Assert.Equal(restoringLogs.Count(l => l.EndsWith("project2.csproj...")), 1);
                Assert.Equal(restoredLogs.Count(l => l.Contains("project2.csproj")), 1);
                var writingAssetsLogs = testNuGetProjectContext.Logs.Value.Where(l => l.StartsWith("Writing assets file to disk.")).ToList();
                // Only 2 write to assets for above 2 projects, never more than that.
                Assert.Equal(writingAssetsLogs.Count, 2);
                // There should be no warning/error.
                Assert.Equal(builtIntegratedActions.Sum(b => b.RestoreResult.LogMessages.Count()), 0);
            }
        }

        [Fact]
        public async Task GetInstalledPackagesAsync_WithCancellationToken_ThrowsAsync()
        {
            // Arrange
            using var testContext = new SimpleTestPathContext();

            var project = await PrepareTestProjectAsync(testContext) as CpsPackageReferenceProject;

            // Act and Assert
            await Assert.ThrowsAsync<OperationCanceledException>(async () => await project.GetInstalledPackagesAsync(new CancellationToken(canceled: true)));
        }

        [Fact]
        public async Task GetInstalledPackagesAsync_FirstCall_NoTransitivePackagesAsync()
        {
            // Arrange
            using var testContext = new SimpleTestPathContext();
            await CreatePackagesAsync(testContext);

            var project = await PrepareTestProjectAsync(testContext) as CpsPackageReferenceProject;

            // Act
            IEnumerable<PackageReference> intalledPackages = await project.GetInstalledPackagesAsync(CancellationToken.None);

            // Assert
            Assert.Collection(intalledPackages,
                elem => Assert.Equal(new PackageIdentity("PackageA", NuGetVersion.Parse("2.15.3")), elem.PackageIdentity));
        }

        [Fact]
        public async Task GetInstalledPackagesAsync_WithGetInstalledAndTransitivePackagesCallAtSameTime_DoesNotDeadlockAsync()
        {
            // Arrange
            using var testContext = new SimpleTestPathContext();
            await CreatePackagesAsync(testContext);

            var project = await PrepareTestProjectAsync(testContext) as CpsPackageReferenceProject;

            IEnumerable<PackageReference> installed = null;
            ProjectPackages installedAndTransitive;
            var tasks = new Task[]
            {
                new Task(async () => installed = await project.GetInstalledPackagesAsync(CancellationToken.None)),
                new Task(async () => installedAndTransitive = await project.GetInstalledAndTransitivePackagesAsync(includeTransitiveOrigins: false, CancellationToken.None)),
            };

            // Act
            Parallel.ForEach(tasks, tsk =>
            {
                if (tsk.Status == TaskStatus.Created)
                {
                    tsk.Start();
                }
            });
            await Task.WhenAll(tasks);

            // Assert
            installed.Should().BeEquivalentTo(installedAndTransitive.InstalledPackages);
            Assert.Collection(installed,
                elem => Assert.Equal(new PackageIdentity("PackageA", NuGetVersion.Parse("2.15.3")), elem.PackageIdentity));
            Assert.Collection(installedAndTransitive.TransitivePackages,
                elem => Assert.Equal(new PackageIdentity("PackageB", NuGetVersion.Parse("1.0.0")), elem.PackageIdentity));
        }

        [Fact]
        public async Task GetInstalledPackagesAsync_WithMultipleCalls_DoesNotDeadlockAsync()
        {
            // Arrange
            using var testContext = new SimpleTestPathContext();
            await CreatePackagesAsync(testContext);

            var project = await PrepareTestProjectAsync(testContext) as CpsPackageReferenceProject;

            var tasks = new[]
            {
                project.GetInstalledPackagesAsync(CancellationToken.None),
                project.GetInstalledPackagesAsync(CancellationToken.None),
                project.GetInstalledPackagesAsync(CancellationToken.None),
                project.GetInstalledPackagesAsync(CancellationToken.None),
                project.GetInstalledPackagesAsync(CancellationToken.None),
            };

            // Act
            Parallel.ForEach(tasks, tsk =>
            {
                if (tsk.Status == TaskStatus.Created)
                {
                    tsk.Start();
                }
            });
            IEnumerable<PackageReference>[] results = await Task.WhenAll(tasks);
            IEnumerable<PackageReference> first = results[0];

            // Assert
            results.Should().AllBeEquivalentTo(first);

            Assert.Collection(first,
                elem => Assert.Equal(new PackageIdentity("PackageA", NuGetVersion.Parse("2.15.3")), elem.PackageIdentity));
        }

        [Fact]
        public async Task GetInstalledPackages_WithNominationUpdate_ReloadsCache()
        {
            using (var testContext = new SimpleTestPathContext())
            {
                // Setup
                var logger = new TestLogger();
                var projectName = "project1";
                var projectFullPath = Path.Combine(testContext.SolutionRoot, projectName + ".csproj");
                var sources = new List<PackageSource>
                {
                    new PackageSource(testContext.PackageSource)
                };
                await SimpleTestPackageUtility.CreateFullPackageAsync(testContext.PackageSource, "packageA", "1.0.0");
                await SimpleTestPackageUtility.CreateFullPackageAsync(testContext.PackageSource, "packageA", "2.0.0");

                // Project
                var projectCache = new ProjectSystemCache();
                var project = CreateCpsPackageReferenceProject(projectName, projectFullPath, projectCache);

                var projectNames = GetTestProjectNames(projectFullPath, projectName);
                var originalSpec = GetPackageSpec(projectName, projectFullPath, "1.0.0");
                projectCache.AddProjectRestoreInfo(projectNames, ProjectTestHelpers.GetDGSpecForAllProjects(originalSpec), new List<IAssetsLogMessage>());
                projectCache.AddProject(projectNames, new Mock<IVsProjectAdapter>().Object, project).Should().BeTrue();

                var request = new TestRestoreRequest(originalSpec, sources, testContext.UserPackagesFolder, logger)
                {
                    LockFilePath = Path.Combine(testContext.SolutionRoot, "obj", "project.assets.json")
                };

                // Pre-conditions
                var command = new RestoreCommand(request);
                var result = await command.ExecuteAsync();
                await result.CommitAsync(logger, CancellationToken.None);
                var packages = await project.GetInstalledPackagesAsync(CancellationToken.None);
                Assert.True(result.Success);
                packages.Should().Contain(a => a.PackageIdentity.Equals(new PackageIdentity("packageA", new NuGetVersion("1.0.0"))));

                // Act - Simulate an installation through restore.
                var updatedSpec = GetPackageSpec(projectName, projectFullPath, "2.0.0");

                var updatedRestore = new TestRestoreRequest(updatedSpec, sources, testContext.UserPackagesFolder, logger)
                {
                    LockFilePath = Path.Combine(testContext.SolutionRoot, "obj", "project.assets.json")
                };

                command = new RestoreCommand(request);
                result = await command.ExecuteAsync();
                await result.CommitAsync(logger, CancellationToken.None);
                // Expectation is that package spec is prefered.
                packages = await project.GetInstalledPackagesAsync(CancellationToken.None);
                packages.Should().Contain(a => a.PackageIdentity.Equals(new PackageIdentity("packageA", new NuGetVersion("1.0.0"))));

                // Act - Simulate a nomination. This should reset the cache and bring it to spec equivalent.
                projectCache.AddProjectRestoreInfo(projectNames, ProjectTestHelpers.GetDGSpecForAllProjects(updatedSpec), new List<IAssetsLogMessage>());
                projectCache.AddProject(projectNames, new Mock<IVsProjectAdapter>().Object, project).Should().BeTrue();

                // Assert
                packages = await project.GetInstalledPackagesAsync(CancellationToken.None);
                packages.Should().Contain(a => a.PackageIdentity.Equals(new PackageIdentity("packageA", new NuGetVersion("2.0.0"))));
            }
        }

        [Fact]
        public async Task TestPackageManager_PreviewProjectsUninstallPackageAsync_AllProjects_UninstallExistingPackage_Success()
        {
            using (var testDirectory = TestDirectory.Create())
            using (var testSolutionManager = new TestSolutionManager())
            {
                // Set up Package Source
                var sources = new List<PackageSource>();
                var packageA_Version100 = new SimpleTestPackageContext("packageA", "1.0.0");
                var packageB_Version100 = new SimpleTestPackageContext("packageB", "1.0.0");
                PackageIdentity packageA = packageA_Version100.Identity;
                PackageIdentity packageB = packageB_Version100.Identity;
                string packageSource = Path.Combine(testDirectory, "packageSource");
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    packageSource,
                    PackageSaveMode.Defaultv3,
                    packageA_Version100,
                    packageB_Version100);

                sources.Add(new PackageSource(packageSource));
                var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(sources);

                // Project
                int numberOfProjects = 4;
                var projectCache = new ProjectSystemCache();
                IVsProjectAdapter projectAdapter = Mock.Of<IVsProjectAdapter>();
                UninstallationContext uninstallationContext = new UninstallationContext();
                var packageSpecs = new PackageSpec[numberOfProjects];
                var projectFullPaths = new string[numberOfProjects];
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    NullSettings.Instance,
                    testSolutionManager,
                    deleteOnRestartManager);

                var testNuGetProjectContext = new TestNuGetProjectContext() { EnableLogging = true };
                var netCorePackageReferenceProjects = new List<CpsPackageReferenceProject>();
                string prevProjectFullPath = null;
                PackageSpec prevPackageSpec = null;

                // Create projects
                for (var i = numberOfProjects - 1; i >= 0; i--)
                {
                    string projectName = $"project{i}";
                    string projectFullPath = Path.Combine(testDirectory.Path, projectName, projectName + ".csproj");
                    TestCpsPackageReferenceProject project = TestCpsPackageReferenceProject.CreateTestCpsPackageReferenceProject(projectName, projectFullPath, projectCache);

                    // We need to treat NU1605 warning as error.
                    project.IsNu1605Error = true;
                    netCorePackageReferenceProjects.Add(project);
                    testSolutionManager.NuGetProjects.Add(project);

                    //Let new project pickup my custom package source.
                    project.ProjectLocalSources.AddRange(sources);
                    ProjectNames projectNames = GetTestProjectNames(projectFullPath, projectName);
                    PackageSpec packageSpec = GetPackageSpec(
                        projectName,
                        projectFullPath,
                        packageA_Version100.Version);

                    if (prevPackageSpec != null)
                    {
                        packageSpec = packageSpec.WithTestProjectReference(prevPackageSpec);
                    }

                    // Restore info
                    DependencyGraphSpec projectRestoreInfo = ProjectTestHelpers.GetDGSpecForAllProjects(packageSpec);
                    projectCache.AddProjectRestoreInfo(projectNames, projectRestoreInfo, new List<IAssetsLogMessage>());
                    projectCache.AddProject(projectNames, projectAdapter, project).Should().BeTrue();
                    prevProjectFullPath = projectFullPath;
                    packageSpecs[i] = packageSpec;
                    prevPackageSpec = packageSpec;
                    projectFullPaths[i] = projectFullPath;
                }

                for (int i = 0; i < numberOfProjects; i++)
                {
                    // Install packageB since packageA is already there.
                    await nuGetPackageManager.InstallPackageAsync(netCorePackageReferenceProjects[i], packageB, new ResolutionContext(), new TestNuGetProjectContext(),
                            sourceRepositoryProvider.GetRepositories(), sourceRepositoryProvider.GetRepositories(), CancellationToken.None);

                    // This code added because nuGetPackageManager.InstallPackageAsync doesn't do updating ProjectSystemCache
                    var installed = new LibraryDependency
                    {
                        LibraryRange = new LibraryRange(
                            name: packageB.Id,
                            versionRange: new VersionRange(packageB.Version),
                            typeConstraint: LibraryDependencyTarget.Package),
                    };

                    PackageSpec packageSpec = packageSpecs[i];

                    var newDependencies = packageSpec.TargetFrameworks.First().Dependencies.Add(installed);
                    packageSpec.TargetFrameworks[0] = new TargetFrameworkInformation(packageSpec.TargetFrameworks[0]) { Dependencies = newDependencies };

                    DependencyGraphSpec projectRestoreInfo = ProjectTestHelpers.GetDGSpecForAllProjects(packageSpec);
                    ProjectNames projectNames = GetTestProjectNames(projectFullPaths[i], $"project{i}");
                    projectCache.AddProjectRestoreInfo(projectNames, projectRestoreInfo, new List<IAssetsLogMessage>());
                }

                IEnumerable<PackageReference> initialInstalledPackages = (await netCorePackageReferenceProjects[numberOfProjects - 1].GetInstalledPackagesAsync(CancellationToken.None)).ToList();

                // Act
                IEnumerable<NuGetProjectAction> actions = await nuGetPackageManager.PreviewProjectsUninstallPackageAsync(
                    netCorePackageReferenceProjects, // All projects
                    packageB.Id,
                    uninstallationContext,
                    testNuGetProjectContext,
                    CancellationToken.None);
                using (SourceCacheContext sourceCacheContext = new SourceCacheContext())
                {
                    await nuGetPackageManager.ExecuteNuGetProjectActionsAsync(
                        netCorePackageReferenceProjects,
                        actions,
                        testNuGetProjectContext,
                        sourceCacheContext,
                        CancellationToken.None);
                }

                // Assert
                Assert.Equal(initialInstalledPackages.Count(), 2);
                List<BuildIntegratedProjectAction> builtIntegratedActions = actions.OfType<BuildIntegratedProjectAction>().ToList();
                Assert.Equal(actions.Count(), builtIntegratedActions.Count);
                Assert.True(builtIntegratedActions.All(b => b.RestoreResult.Success));
                Assert.True(builtIntegratedActions.All(b => !b.RestoreResult.LogMessages.Any())); // There should be no error or warning.
                List<string> uninstalledLogs = testNuGetProjectContext.Logs.Value.Where(l => l.StartsWith("Successfully uninstalled ")).ToList();
                Assert.True(uninstalledLogs.Count() > 0);
                List<string> restoringLogs = testNuGetProjectContext.Logs.Value.Where(l => l.StartsWith("Restoring packages for ")).ToList();
                List<string> restoredLogs = testNuGetProjectContext.Logs.Value.Where(l => l.StartsWith("Restored ")).ToList();
                // Making sure project0 restored only once, not many.
                Assert.Equal(restoringLogs.Count(l => l.EndsWith("project0.csproj...")), 1);
                Assert.Equal(restoredLogs.Count(l => l.Contains("project0.csproj")), 1);
                Assert.Equal(restoringLogs.Count(l => l.EndsWith("project1.csproj...")), 1);
                Assert.Equal(restoredLogs.Count(l => l.Contains("project1.csproj")), 1);
                Assert.Equal(restoringLogs.Count(l => l.EndsWith("project2.csproj...")), 1);
                Assert.Equal(restoredLogs.Count(l => l.Contains("project2.csproj")), 1);
                Assert.Equal(restoringLogs.Count(l => l.EndsWith("project3.csproj...")), 1);
                Assert.Equal(restoredLogs.Count(l => l.Contains("project3.csproj")), 1);
                List<string> writingAssetsLogs = testNuGetProjectContext.Logs.Value.Where(l => l.StartsWith("Writing assets file to disk.")).ToList();
                // Only 4 write to assets for above 4 projects, not more than that.
                Assert.Equal(writingAssetsLogs.Count, 4);
            }
        }

        [Fact]
        public async Task TestPackageManager_PreviewProjectsUninstallPackageAsync_TopParentProject_UninstallExistingPackage_Success()
        {
            using (var testDirectory = TestDirectory.Create())
            using (var testSolutionManager = new TestSolutionManager())
            {
                // Set up Package Source
                var sources = new List<PackageSource>();
                var packageA_Version100 = new SimpleTestPackageContext("packageA", "1.0.0");
                var packageB_Version100 = new SimpleTestPackageContext("packageB", "1.0.0");
                PackageIdentity packageA = packageA_Version100.Identity;
                PackageIdentity packageB = packageB_Version100.Identity;
                string packageSource = Path.Combine(testDirectory, "packageSource");
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    packageSource,
                    PackageSaveMode.Defaultv3,
                    packageA_Version100,
                    packageB_Version100);

                sources.Add(new PackageSource(packageSource));
                var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(sources);

                // Project
                int numberOfProjects = 4;
                var projectCache = new ProjectSystemCache();
                IVsProjectAdapter projectAdapter = Mock.Of<IVsProjectAdapter>();
                UninstallationContext uninstallationContext = new UninstallationContext();
                var packageSpecs = new PackageSpec[numberOfProjects];
                var projectFullPaths = new string[numberOfProjects];
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    NullSettings.Instance,
                    testSolutionManager,
                    deleteOnRestartManager);

                var testNuGetProjectContext = new TestNuGetProjectContext() { EnableLogging = true };
                var netCorePackageReferenceProjects = new List<CpsPackageReferenceProject>();
                string prevProjectFullPath = null;
                PackageSpec prevPackageSpec = null;

                // Create projects
                for (var i = numberOfProjects - 1; i >= 0; i--)
                {
                    string projectName = $"project{i}";
                    string projectFullPath = Path.Combine(testDirectory.Path, projectName, projectName + ".csproj");
                    TestCpsPackageReferenceProject project = TestCpsPackageReferenceProject.CreateTestCpsPackageReferenceProject(projectName, projectFullPath, projectCache);

                    // We need to treat NU1605 warning as error.
                    project.IsNu1605Error = true;
                    netCorePackageReferenceProjects.Add(project);
                    testSolutionManager.NuGetProjects.Add(project);

                    //Let new project pickup my custom package source.
                    project.ProjectLocalSources.AddRange(sources);
                    ProjectNames projectNames = GetTestProjectNames(projectFullPath, projectName);
                    PackageSpec packageSpec = GetPackageSpec(
                        projectName,
                        projectFullPath,
                        packageA_Version100.Version);

                    if (prevPackageSpec != null)
                    {
                        packageSpec = packageSpec.WithTestProjectReference(prevPackageSpec);
                    }

                    // Restore info
                    DependencyGraphSpec projectRestoreInfo = ProjectTestHelpers.GetDGSpecForAllProjects(packageSpec);
                    projectCache.AddProjectRestoreInfo(projectNames, projectRestoreInfo, new List<IAssetsLogMessage>());
                    projectCache.AddProject(projectNames, projectAdapter, project).Should().BeTrue();
                    prevProjectFullPath = projectFullPath;
                    packageSpecs[i] = packageSpec;
                    prevPackageSpec = packageSpec;
                    projectFullPaths[i] = projectFullPath;
                }

                for (int i = 0; i < numberOfProjects; i++)
                {
                    // Install packageB since packageA is already there.
                    await nuGetPackageManager.InstallPackageAsync(netCorePackageReferenceProjects[i], packageB, new ResolutionContext(), new TestNuGetProjectContext(),
                            sourceRepositoryProvider.GetRepositories(), sourceRepositoryProvider.GetRepositories(), CancellationToken.None);

                    // This code added because nuGetPackageManager.InstallPackageAsync doesn't do updating ProjectSystemCache
                    var installed = new LibraryDependency
                    {
                        LibraryRange = new LibraryRange(
                            name: packageB.Id,
                            versionRange: new VersionRange(packageB.Version),
                            typeConstraint: LibraryDependencyTarget.Package),
                    };

                    PackageSpec packageSpec = packageSpecs[i];

                    var newDependencies = packageSpec.TargetFrameworks.First().Dependencies.Add(installed);
                    packageSpec.TargetFrameworks[0] = new TargetFrameworkInformation(packageSpec.TargetFrameworks[0]) { Dependencies = newDependencies };

                    DependencyGraphSpec projectRestoreInfo = ProjectTestHelpers.GetDGSpecForAllProjects(packageSpec);
                    ProjectNames projectNames = GetTestProjectNames(projectFullPaths[i], $"project{i}");
                    projectCache.AddProjectRestoreInfo(projectNames, projectRestoreInfo, new List<IAssetsLogMessage>());
                }

                IEnumerable<PackageReference> initialInstalledPackages = (await netCorePackageReferenceProjects[numberOfProjects - 1].GetInstalledPackagesAsync(CancellationToken.None)).ToList();
                var targetProjects = new List<CpsPackageReferenceProject>()
                {
                    netCorePackageReferenceProjects[numberOfProjects-1] // Top parent project.
                };

                // Act
                IEnumerable<NuGetProjectAction> actions = await nuGetPackageManager.PreviewProjectsUninstallPackageAsync(
                    targetProjects,
                    packageB.Id,
                    uninstallationContext,
                    testNuGetProjectContext,
                    CancellationToken.None);
                using (SourceCacheContext sourceCacheContext = new SourceCacheContext())
                {
                    await nuGetPackageManager.ExecuteNuGetProjectActionsAsync(
                        netCorePackageReferenceProjects,
                        actions,
                        testNuGetProjectContext,
                        sourceCacheContext,
                        CancellationToken.None);
                }

                // Assert
                Assert.Equal(initialInstalledPackages.Count(), 2);
                List<BuildIntegratedProjectAction> builtIntegratedActions = actions.OfType<BuildIntegratedProjectAction>().ToList();
                Assert.Equal(actions.Count(), builtIntegratedActions.Count);
                Assert.True(builtIntegratedActions.All(b => b.RestoreResult.Success));
                Assert.True(builtIntegratedActions.All(b => !b.RestoreResult.LogMessages.Any())); // There should be no error or warning.
                List<string> restoringLogs = testNuGetProjectContext.Logs.Value.Where(l => l.StartsWith("Restoring packages for ")).ToList();
                List<string> restoredLogs = testNuGetProjectContext.Logs.Value.Where(l => l.StartsWith("Restored ")).ToList();
                // Making sure project0 restored only once, not many.
                Assert.Equal(restoringLogs.Count(l => l.EndsWith("project0.csproj...")), 1);
                Assert.Equal(restoredLogs.Count(l => l.Contains("project0.csproj")), 1);
                // It shouldn't trigger restore of child projects.
                Assert.Equal(restoringLogs.Count(l => l.EndsWith("project1.csproj...")), 0);
                Assert.Equal(restoredLogs.Count(l => l.Contains("project1.csproj")), 0);
                Assert.Equal(restoringLogs.Count(l => l.EndsWith("project2.csproj...")), 0);
                Assert.Equal(restoredLogs.Count(l => l.Contains("project2.csproj")), 0);
                Assert.Equal(restoringLogs.Count(l => l.EndsWith("project3.csproj...")), 0);
                Assert.Equal(restoredLogs.Count(l => l.Contains("project3.csproj")), 0);
                List<string> writingAssetsLogs = testNuGetProjectContext.Logs.Value.Where(l => l.StartsWith("Writing assets file to disk.")).ToList();
                // Only 1 write to assets for above 4 projects, not more than that.
                Assert.Equal(writingAssetsLogs.Count, 1);
            }
        }

        [Fact]
        public async Task TestPackageManager_PreviewProjectsUninstallPackageAsync_MiddleParentProject_UninstallExistingPackage_Success()
        {
            using (var testDirectory = TestDirectory.Create())
            using (var testSolutionManager = new TestSolutionManager())
            {
                // Set up Package Source
                var sources = new List<PackageSource>();
                var packageA_Version100 = new SimpleTestPackageContext("packageA", "1.0.0");
                var packageB_Version100 = new SimpleTestPackageContext("packageB", "1.0.0");
                PackageIdentity packageA = packageA_Version100.Identity;
                PackageIdentity packageB = packageB_Version100.Identity;
                string packageSource = Path.Combine(testDirectory, "packageSource");
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    packageSource,
                    PackageSaveMode.Defaultv3,
                    packageA_Version100,
                    packageB_Version100);

                sources.Add(new PackageSource(packageSource));
                var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(sources);

                // Project
                int numberOfProjects = 4;
                var projectCache = new ProjectSystemCache();
                IVsProjectAdapter projectAdapter = Mock.Of<IVsProjectAdapter>();
                UninstallationContext uninstallationContext = new UninstallationContext();
                var packageSpecs = new PackageSpec[numberOfProjects];
                var projectFullPaths = new string[numberOfProjects];
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    NullSettings.Instance,
                    testSolutionManager,
                    deleteOnRestartManager);

                var testNuGetProjectContext = new TestNuGetProjectContext() { EnableLogging = true };
                var netCorePackageReferenceProjects = new List<CpsPackageReferenceProject>();
                string prevProjectFullPath = null;
                PackageSpec prevPackageSpec = null;

                // Create projects
                for (var i = numberOfProjects - 1; i >= 0; i--)
                {
                    string projectName = $"project{i}";
                    string projectFullPath = Path.Combine(testDirectory.Path, projectName, projectName + ".csproj");
                    TestCpsPackageReferenceProject project = TestCpsPackageReferenceProject.CreateTestCpsPackageReferenceProject(projectName, projectFullPath, projectCache);

                    // We need to treat NU1605 warning as error.
                    project.IsNu1605Error = true;
                    netCorePackageReferenceProjects.Add(project);
                    testSolutionManager.NuGetProjects.Add(project);

                    //Let new project pickup my custom package source.
                    project.ProjectLocalSources.AddRange(sources);
                    ProjectNames projectNames = GetTestProjectNames(projectFullPath, projectName);
                    PackageSpec packageSpec = GetPackageSpec(
                        projectName,
                        projectFullPath,
                        packageA_Version100.Version);

                    if (prevPackageSpec != null)
                    {
                        packageSpec = packageSpec.WithTestProjectReference(prevPackageSpec);
                    }

                    // Restore info
                    DependencyGraphSpec projectRestoreInfo = ProjectTestHelpers.GetDGSpecForAllProjects(packageSpec);
                    projectCache.AddProjectRestoreInfo(projectNames, projectRestoreInfo, new List<IAssetsLogMessage>());
                    projectCache.AddProject(projectNames, projectAdapter, project).Should().BeTrue();
                    prevProjectFullPath = projectFullPath;
                    packageSpecs[i] = packageSpec;
                    prevPackageSpec = packageSpec;
                    projectFullPaths[i] = projectFullPath;
                }

                for (int i = 0; i < numberOfProjects; i++)
                {
                    // Install packageB since packageA is already there.
                    await nuGetPackageManager.InstallPackageAsync(netCorePackageReferenceProjects[i], packageB, new ResolutionContext(), new TestNuGetProjectContext(),
                            sourceRepositoryProvider.GetRepositories(), sourceRepositoryProvider.GetRepositories(), CancellationToken.None);

                    // This code added because nuGetPackageManager.InstallPackageAsync doesn't do updating ProjectSystemCache
                    var installed = new LibraryDependency
                    {
                        LibraryRange = new LibraryRange(
                            name: packageB.Id,
                            versionRange: new VersionRange(packageB.Version),
                            typeConstraint: LibraryDependencyTarget.Package),
                    };

                    PackageSpec packageSpec = packageSpecs[i];

                    var newDependencies = packageSpec.TargetFrameworks.First().Dependencies.Add(installed);
                    packageSpec.TargetFrameworks[0] = new TargetFrameworkInformation(packageSpec.TargetFrameworks[0]) { Dependencies = newDependencies };

                    DependencyGraphSpec projectRestoreInfo = ProjectTestHelpers.GetDGSpecForAllProjects(packageSpec);
                    ProjectNames projectNames = GetTestProjectNames(projectFullPaths[i], $"project{i}");
                    projectCache.AddProjectRestoreInfo(projectNames, projectRestoreInfo, new List<IAssetsLogMessage>());
                }

                IEnumerable<PackageReference> initialInstalledPackages = (await netCorePackageReferenceProjects[numberOfProjects - 1].GetInstalledPackagesAsync(CancellationToken.None)).ToList();
                var targetProjects = new List<CpsPackageReferenceProject>()
                {
                    netCorePackageReferenceProjects[numberOfProjects-2] // Middle parent project.
                };

                // Act
                IEnumerable<NuGetProjectAction> actions = await nuGetPackageManager.PreviewProjectsUninstallPackageAsync(
                    targetProjects,
                    packageB.Id,
                    uninstallationContext,
                    testNuGetProjectContext,
                    CancellationToken.None);
                using (SourceCacheContext sourceCacheContext = new SourceCacheContext())
                {
                    await nuGetPackageManager.ExecuteNuGetProjectActionsAsync(
                        netCorePackageReferenceProjects,
                        actions,
                        testNuGetProjectContext,
                        sourceCacheContext,
                        CancellationToken.None);
                }

                // Assert
                Assert.Equal(initialInstalledPackages.Count(), 2);
                List<BuildIntegratedProjectAction> builtIntegratedActions = actions.OfType<BuildIntegratedProjectAction>().ToList();
                Assert.Equal(actions.Count(), builtIntegratedActions.Count);
                Assert.True(builtIntegratedActions.All(b => b.RestoreResult.Success));
                Assert.True(builtIntegratedActions.All(b => !b.RestoreResult.LogMessages.Any())); // There should be no error or warning.
                List<string> restoringLogs = testNuGetProjectContext.Logs.Value.Where(l => l.StartsWith("Restoring packages for ")).ToList();
                List<string> restoredLogs = testNuGetProjectContext.Logs.Value.Where(l => l.StartsWith("Restored ")).ToList();
                Assert.Equal(restoringLogs.Count(l => l.EndsWith("project0.csproj...")), 0);
                Assert.Equal(restoredLogs.Count(l => l.Contains("project0.csproj")), 0);
                // Making sure project1 restored only once, not many.
                Assert.Equal(restoringLogs.Count(l => l.EndsWith("project1.csproj...")), 1);
                Assert.Equal(restoredLogs.Count(l => l.Contains("project1.csproj")), 1);
                Assert.Equal(restoringLogs.Count(l => l.EndsWith("project2.csproj...")), 0);
                Assert.Equal(restoredLogs.Count(l => l.Contains("project2.csproj")), 0);
                Assert.Equal(restoringLogs.Count(l => l.EndsWith("project3.csproj...")), 0);
                Assert.Equal(restoredLogs.Count(l => l.Contains("project3.csproj")), 0);
                List<string> writingAssetsLogs = testNuGetProjectContext.Logs.Value.Where(l => l.StartsWith("Writing assets file to disk.")).ToList();
                // Only 1 write to assets for above 4 projects, not more than that.
                Assert.Equal(writingAssetsLogs.Count, 1);
            }
        }

        [Fact]
        public async Task TestPackageManager_PreviewProjectsUninstallPackageAsync_BottomChildProject_UninstallExistingPackage_Success()
        {
            using (var testDirectory = TestDirectory.Create())
            using (var testSolutionManager = new TestSolutionManager())
            {
                // Set up Package Source
                var sources = new List<PackageSource>();
                var packageA_Version100 = new SimpleTestPackageContext("packageA", "1.0.0");
                var packageB_Version100 = new SimpleTestPackageContext("packageB", "1.0.0");
                PackageIdentity packageA = packageA_Version100.Identity;
                PackageIdentity packageB = packageB_Version100.Identity;
                string packageSource = Path.Combine(testDirectory, "packageSource");
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    packageSource,
                    PackageSaveMode.Defaultv3,
                    packageA_Version100,
                    packageB_Version100);

                sources.Add(new PackageSource(packageSource));
                var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(sources);

                // Project
                int numberOfProjects = 4;
                var projectCache = new ProjectSystemCache();
                IVsProjectAdapter projectAdapter = Mock.Of<IVsProjectAdapter>();
                UninstallationContext uninstallationContext = new UninstallationContext();
                var packageSpecs = new PackageSpec[numberOfProjects];
                var projectFullPaths = new string[numberOfProjects];
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    NullSettings.Instance,
                    testSolutionManager,
                    deleteOnRestartManager);

                var testNuGetProjectContext = new TestNuGetProjectContext() { EnableLogging = true };
                var netCorePackageReferenceProjects = new List<CpsPackageReferenceProject>();
                string prevProjectFullPath = null;
                PackageSpec prevPackageSpec = null;

                // Create projects
                for (var i = numberOfProjects - 1; i >= 0; i--)
                {
                    string projectName = $"project{i}";
                    string projectFullPath = Path.Combine(testDirectory.Path, projectName, projectName + ".csproj");
                    TestCpsPackageReferenceProject project = TestCpsPackageReferenceProject.CreateTestCpsPackageReferenceProject(projectName, projectFullPath, projectCache);

                    // We need to treat NU1605 warning as error.
                    project.IsNu1605Error = true;
                    netCorePackageReferenceProjects.Add(project);
                    testSolutionManager.NuGetProjects.Add(project);

                    //Let new project pickup my custom package source.
                    project.ProjectLocalSources.AddRange(sources);
                    ProjectNames projectNames = GetTestProjectNames(projectFullPath, projectName);
                    PackageSpec packageSpec = GetPackageSpec(
                        projectName,
                        projectFullPath,
                        packageA_Version100.Version);

                    if (prevPackageSpec != null)
                    {
                        packageSpec = packageSpec.WithTestProjectReference(prevPackageSpec);
                    }

                    // Restore info
                    DependencyGraphSpec projectRestoreInfo = ProjectTestHelpers.GetDGSpecForAllProjects(packageSpec);
                    projectCache.AddProjectRestoreInfo(projectNames, projectRestoreInfo, new List<IAssetsLogMessage>());
                    projectCache.AddProject(projectNames, projectAdapter, project).Should().BeTrue();
                    prevProjectFullPath = projectFullPath;
                    packageSpecs[i] = packageSpec;
                    prevPackageSpec = packageSpec;
                    projectFullPaths[i] = projectFullPath;
                }

                for (int i = 0; i < numberOfProjects; i++)
                {
                    // Install packageB since packageA is already there.
                    await nuGetPackageManager.InstallPackageAsync(netCorePackageReferenceProjects[i], packageB, new ResolutionContext(), new TestNuGetProjectContext(),
                            sourceRepositoryProvider.GetRepositories(), sourceRepositoryProvider.GetRepositories(), CancellationToken.None);

                    // This code added because nuGetPackageManager.InstallPackageAsync doesn't do updating ProjectSystemCache
                    var installed = new LibraryDependency
                    {
                        LibraryRange = new LibraryRange(
                            name: packageB.Id,
                            versionRange: new VersionRange(packageB.Version),
                            typeConstraint: LibraryDependencyTarget.Package),
                    };

                    PackageSpec packageSpec = packageSpecs[i];

                    var newDependencies = packageSpec.TargetFrameworks.First().Dependencies.Add(installed);
                    packageSpec.TargetFrameworks[0] = new TargetFrameworkInformation(packageSpec.TargetFrameworks[0]) { Dependencies = newDependencies };

                    DependencyGraphSpec projectRestoreInfo = ProjectTestHelpers.GetDGSpecForAllProjects(packageSpec);
                    ProjectNames projectNames = GetTestProjectNames(projectFullPaths[i], $"project{i}");
                    projectCache.AddProjectRestoreInfo(projectNames, projectRestoreInfo, new List<IAssetsLogMessage>());
                }

                IEnumerable<PackageReference> initialInstalledPackages = (await netCorePackageReferenceProjects[numberOfProjects - 1].GetInstalledPackagesAsync(CancellationToken.None)).ToList();
                var targetProjects = new List<CpsPackageReferenceProject>()
                {
                    netCorePackageReferenceProjects[0] // Bottom child project.
                };

                // Act
                IEnumerable<NuGetProjectAction> actions = await nuGetPackageManager.PreviewProjectsUninstallPackageAsync(
                    targetProjects,
                    packageB.Id,
                    uninstallationContext,
                    testNuGetProjectContext,
                    CancellationToken.None);
                using (SourceCacheContext sourceCacheContext = new SourceCacheContext())
                {
                    await nuGetPackageManager.ExecuteNuGetProjectActionsAsync(
                        netCorePackageReferenceProjects,
                        actions,
                        testNuGetProjectContext,
                        sourceCacheContext,
                        CancellationToken.None);
                }

                // Assert
                Assert.Equal(initialInstalledPackages.Count(), 2);
                List<BuildIntegratedProjectAction> builtIntegratedActions = actions.OfType<BuildIntegratedProjectAction>().ToList();
                Assert.Equal(actions.Count(), builtIntegratedActions.Count);
                Assert.True(builtIntegratedActions.All(b => b.RestoreResult.Success));
                Assert.True(builtIntegratedActions.All(b => !b.RestoreResult.LogMessages.Any())); // There should be no error or warning.
                List<string> uninstalledLogs = testNuGetProjectContext.Logs.Value.Where(l => l.StartsWith("Successfully uninstalled ")).ToList();
                Assert.True(uninstalledLogs.Count() > 0);
                List<string> restoringLogs = testNuGetProjectContext.Logs.Value.Where(l => l.StartsWith("Restoring packages for ")).ToList();
                List<string> restoredLogs = testNuGetProjectContext.Logs.Value.Where(l => l.StartsWith("Restored ")).ToList();
                Assert.Equal(restoringLogs.Count(l => l.EndsWith("project0.csproj...")), 0);
                Assert.Equal(restoredLogs.Count(l => l.Contains("project0.csproj")), 0);
                Assert.Equal(restoringLogs.Count(l => l.EndsWith("project1.csproj...")), 0);
                Assert.Equal(restoredLogs.Count(l => l.Contains("project1.csproj")), 0);
                Assert.Equal(restoringLogs.Count(l => l.EndsWith("project2.csproj...")), 0);
                Assert.Equal(restoredLogs.Count(l => l.Contains("project2.csproj")), 0);
                // Making sure project3 restored only once, not many.
                Assert.Equal(restoringLogs.Count(l => l.EndsWith("project3.csproj...")), 1);
                Assert.Equal(restoredLogs.Count(l => l.Contains("project3.csproj")), 1);
                List<string> writingAssetsLogs = testNuGetProjectContext.Logs.Value.Where(l => l.StartsWith("Writing assets file to disk.")).ToList();
                // Only 1 write to assets for above 4 projects, not more than that.
                Assert.Equal(writingAssetsLogs.Count, 1);
            }
        }

        [Fact]
        public async Task TestPackageManager_PreviewProjectsUninstallPackageAsync_UninstallNonExistingPackage_Fail()
        {
            using (var testDirectory = TestDirectory.Create())
            using (var testSolutionManager = new TestSolutionManager())
            {
                // Set up Package Source
                var sources = new List<PackageSource>();
                var packageA_Version100 = new SimpleTestPackageContext("packageA", "1.0.0");
                var packageB_Version100 = new SimpleTestPackageContext("packageB", "1.0.0");
                PackageIdentity packageA = packageA_Version100.Identity;
                PackageIdentity packageB = packageB_Version100.Identity;
                string packageSource = Path.Combine(testDirectory, "packageSource");
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    packageSource,
                    PackageSaveMode.Defaultv3,
                    packageA_Version100,
                    packageB_Version100);

                sources.Add(new PackageSource(packageSource));
                var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(sources);

                // Project
                int numberOfProjects = 4;
                var projectCache = new ProjectSystemCache();
                IVsProjectAdapter projectAdapter = Mock.Of<IVsProjectAdapter>();
                UninstallationContext uninstallationContext = new UninstallationContext();
                var packageSpecs = new PackageSpec[numberOfProjects];
                var projectFullPaths = new string[numberOfProjects];
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    NullSettings.Instance,
                    testSolutionManager,
                    deleteOnRestartManager);

                var testNuGetProjectContext = new TestNuGetProjectContext() { EnableLogging = true };
                var netCorePackageReferenceProjects = new List<CpsPackageReferenceProject>();
                string prevProjectFullPath = null;
                PackageSpec prevPackageSpec = null;

                // Create projects
                for (var i = numberOfProjects - 1; i >= 0; i--)
                {
                    string projectName = $"project{i}";
                    string projectFullPath = Path.Combine(testDirectory.Path, projectName, projectName + ".csproj");
                    TestCpsPackageReferenceProject project = TestCpsPackageReferenceProject.CreateTestCpsPackageReferenceProject(projectName, projectFullPath, projectCache);

                    // We need to treat NU1605 warning as error.
                    project.IsNu1605Error = true;
                    netCorePackageReferenceProjects.Add(project);
                    testSolutionManager.NuGetProjects.Add(project);

                    //Let new project pickup my custom package source.
                    project.ProjectLocalSources.AddRange(sources);
                    ProjectNames projectNames = GetTestProjectNames(projectFullPath, projectName);
                    PackageSpec packageSpec = GetPackageSpec(
                        projectName,
                        projectFullPath,
                        packageA_Version100.Version);

                    if (prevPackageSpec != null)
                    {
                        packageSpec = packageSpec.WithTestProjectReference(prevPackageSpec);
                    }

                    // Restore info
                    DependencyGraphSpec projectRestoreInfo = ProjectTestHelpers.GetDGSpecForAllProjects(packageSpec);
                    projectCache.AddProjectRestoreInfo(projectNames, projectRestoreInfo, new List<IAssetsLogMessage>());
                    projectCache.AddProject(projectNames, projectAdapter, project).Should().BeTrue();
                    prevProjectFullPath = projectFullPath;
                    packageSpecs[i] = packageSpec;
                    prevPackageSpec = packageSpec;
                    projectFullPaths[i] = projectFullPath;
                }

                for (int i = 0; i < numberOfProjects; i++)
                {
                    // Install packageB since packageA is already there.
                    await nuGetPackageManager.InstallPackageAsync(netCorePackageReferenceProjects[i], packageB, new ResolutionContext(), new TestNuGetProjectContext(),
                            sourceRepositoryProvider.GetRepositories(), sourceRepositoryProvider.GetRepositories(), CancellationToken.None);

                    // This code added because nuGetPackageManager.InstallPackageAsync doesn't do updating ProjectSystemCache
                    var installed = new LibraryDependency
                    {
                        LibraryRange = new LibraryRange(
                            name: packageB.Id,
                            versionRange: new VersionRange(packageB.Version),
                            typeConstraint: LibraryDependencyTarget.Package),
                    };

                    PackageSpec packageSpec = packageSpecs[i];

                    var newDependencies = packageSpec.TargetFrameworks.First().Dependencies.Add(installed);
                    packageSpec.TargetFrameworks[0] = new TargetFrameworkInformation(packageSpec.TargetFrameworks[0]) { Dependencies = newDependencies };

                    DependencyGraphSpec projectRestoreInfo = ProjectTestHelpers.GetDGSpecForAllProjects(packageSpec);
                    ProjectNames projectNames = GetTestProjectNames(projectFullPaths[i], $"project{i}");
                    projectCache.AddProjectRestoreInfo(projectNames, projectRestoreInfo, new List<IAssetsLogMessage>());
                }

                var initialInstalledPackages = (await netCorePackageReferenceProjects[numberOfProjects - 1].GetInstalledPackagesAsync(CancellationToken.None)).ToList();

                // Act
                string nonexistingPackage = packageB.Id + "NonExist";
                ArgumentException exception = await Assert.ThrowsAsync<ArgumentException>(
                    () => nuGetPackageManager.PreviewProjectsUninstallPackageAsync(
                        netCorePackageReferenceProjects,
                        nonexistingPackage,
                        uninstallationContext,
                        testNuGetProjectContext,
                        CancellationToken.None));

                // Assert
                Assert.NotNull(exception);
                Assert.StartsWith($"Package '{nonexistingPackage}' to be uninstalled could not be found in project", exception.Message);
            }
        }

        [Fact]
        public async Task GetTransitivePackagesAsync_WithTransitivePackageReferences_ReturnsPackageIdentities()
        {
            using (TestDirectory testDirectory = TestDirectory.Create())
            {
                // Setup
                var projectName = "project1";
                string projectFullPath = Path.Combine(testDirectory.Path, projectName + ".csproj");

                // Project
                var projectCache = new ProjectSystemCache();
                IVsProjectAdapter projectAdapter = Mock.Of<IVsProjectAdapter>();
                CpsPackageReferenceProject project = CreateCpsPackageReferenceProject(projectName, projectFullPath, projectCache);

                ProjectNames projectNames = GetTestProjectNames(projectFullPath, projectName);
                PackageSpec packageSpec = GetPackageSpec(projectName, projectFullPath, "[2.0.0, )");

                // Restore info
                DependencyGraphSpec projectRestoreInfo = ProjectTestHelpers.GetDGSpecForAllProjects(packageSpec);
                projectCache.AddProjectRestoreInfo(projectNames, projectRestoreInfo, new List<IAssetsLogMessage>());
                projectCache.AddProject(projectNames, projectAdapter, project).Should().BeTrue();

                // Package directories
                var sources = new List<PackageSource>();
                var packagesDir = new DirectoryInfo(Path.Combine(testDirectory, "globalPackages"));
                var packageSource = new DirectoryInfo(Path.Combine(testDirectory, "packageSource"));
                packagesDir.Create();
                packageSource.Create();
                sources.Add(new PackageSource(packageSource.FullName));

                var logger = new TestLogger();
                var request = new TestRestoreRequest(packageSpec, sources, packagesDir.FullName, logger)
                {
                    LockFilePath = Path.Combine(testDirectory, "project.assets.json")
                };

                await SimpleTestPackageUtility.CreateFullPackageAsync(packageSource.FullName, "packageB", "1.0.0");
                await SimpleTestPackageUtility.CreateFullPackageAsync(
                    packageSource.FullName,
                    "packageA",
                    "2.15.3",
                    new Packaging.Core.PackageDependency[]
                    {
                        new Packaging.Core.PackageDependency("packageB", VersionRange.Parse("1.0.0"))
                    });

                // Act
                var command = new RestoreCommand(request);
                RestoreResult result = await command.ExecuteAsync();
                await result.CommitAsync(logger, CancellationToken.None);
                ProjectPackages packages = await project.GetInstalledAndTransitivePackagesAsync(includeTransitiveOrigins: false, CancellationToken.None);

                // Assert
                Assert.True(result.Success);
                packages.InstalledPackages.Should().Contain(a => a.PackageIdentity.Equals(new PackageIdentity("packageA", new NuGetVersion("2.15.3"))));
                packages.TransitivePackages.Should().Contain(a => a.PackageIdentity.Equals(new PackageIdentity("packageB", new NuGetVersion("1.0.0"))));
            }
        }

        [Fact]
        public async Task GetTransitivePackagesAsync_WithNestedTransitivePackageReferences_ReturnsPackageIdentities()
        {
            using (TestDirectory testDirectory = TestDirectory.Create())
            {
                // Setup
                var projectName = "project1";
                string projectFullPath = Path.Combine(testDirectory.Path, projectName + ".csproj");

                // Project
                var projectCache = new ProjectSystemCache();
                IVsProjectAdapter projectAdapter = Mock.Of<IVsProjectAdapter>();
                CpsPackageReferenceProject project = CreateCpsPackageReferenceProject(projectName, projectFullPath, projectCache);

                ProjectNames projectNames = GetTestProjectNames(projectFullPath, projectName);
                PackageSpec packageSpec = GetPackageSpec(projectName, projectFullPath, "[2.0.0, )");

                // Restore info
                DependencyGraphSpec projectRestoreInfo = ProjectTestHelpers.GetDGSpecForAllProjects(packageSpec);
                projectCache.AddProjectRestoreInfo(projectNames, projectRestoreInfo, new List<IAssetsLogMessage>());
                projectCache.AddProject(projectNames, projectAdapter, project).Should().BeTrue();

                // Package directories
                var sources = new List<PackageSource>();
                var packagesDir = new DirectoryInfo(Path.Combine(testDirectory, "globalPackages"));
                var packageSource = new DirectoryInfo(Path.Combine(testDirectory, "packageSource"));
                packagesDir.Create();
                packageSource.Create();
                sources.Add(new PackageSource(packageSource.FullName));

                var logger = new TestLogger();
                var request = new TestRestoreRequest(packageSpec, sources, packagesDir.FullName, logger)
                {
                    LockFilePath = Path.Combine(testDirectory, "project.assets.json")
                };

                await SimpleTestPackageUtility.CreateFullPackageAsync(packageSource.FullName, "packageC", "2.1.43");
                await SimpleTestPackageUtility.CreateFullPackageAsync(
                    packageSource.FullName,
                    "packageB",
                    "1.0.0",
                    new Packaging.Core.PackageDependency[]
                    {
                        new Packaging.Core.PackageDependency("packageC", VersionRange.Parse("2.1.43"))
                    });
                await SimpleTestPackageUtility.CreateFullPackageAsync(
                    packageSource.FullName,
                    "packageA",
                    "2.15.3",
                    new Packaging.Core.PackageDependency[]
                    {
                        new Packaging.Core.PackageDependency("packageB", VersionRange.Parse("1.0.0"))
                    });

                // Act
                var command = new RestoreCommand(request);
                RestoreResult result = await command.ExecuteAsync();
                await result.CommitAsync(logger, CancellationToken.None);
                ProjectPackages packages = await project.GetInstalledAndTransitivePackagesAsync(includeTransitiveOrigins: false, CancellationToken.None);

                // Assert
                Assert.True(result.Success);
                packages.InstalledPackages.Should().Contain(a => a.PackageIdentity.Equals(new PackageIdentity("packageA", new NuGetVersion("2.15.3"))));
                packages.TransitivePackages.Should().Contain(a => a.PackageIdentity.Equals(new PackageIdentity("packageB", new NuGetVersion("1.0.0"))));
                packages.TransitivePackages.Should().Contain(a => a.PackageIdentity.Equals(new PackageIdentity("packageC", new NuGetVersion("2.1.43"))));
            }
        }

        [Fact]
        public async Task GetTransitivePackagesAsync_WithNoTransitivePackageReferences_ReturnsOnlyInstalledPackageIdentities()
        {
            using (TestDirectory testDirectory = TestDirectory.Create())
            {
                // Setup
                var projectName = "project1";
                string projectFullPath = Path.Combine(testDirectory.Path, projectName + ".csproj");

                // Project
                var projectCache = new ProjectSystemCache();
                IVsProjectAdapter projectAdapter = Mock.Of<IVsProjectAdapter>();
                CpsPackageReferenceProject project = CreateCpsPackageReferenceProject(projectName, projectFullPath, projectCache);

                ProjectNames projectNames = GetTestProjectNames(projectFullPath, projectName);
                PackageSpec packageSpec = GetPackageSpec(projectName, projectFullPath, "[2.0.0, )");

                // Restore info
                DependencyGraphSpec projectRestoreInfo = ProjectTestHelpers.GetDGSpecForAllProjects(packageSpec);
                projectCache.AddProjectRestoreInfo(projectNames, projectRestoreInfo, new List<IAssetsLogMessage>());
                projectCache.AddProject(projectNames, projectAdapter, project).Should().BeTrue();

                // Package directories
                var sources = new List<PackageSource>();
                var packagesDir = new DirectoryInfo(Path.Combine(testDirectory, "globalPackages"));
                var packageSource = new DirectoryInfo(Path.Combine(testDirectory, "packageSource"));
                packagesDir.Create();
                packageSource.Create();
                sources.Add(new PackageSource(packageSource.FullName));

                var logger = new TestLogger();
                var request = new TestRestoreRequest(packageSpec, sources, packagesDir.FullName, logger)
                {
                    LockFilePath = Path.Combine(testDirectory, "project.assets.json")
                };

                await SimpleTestPackageUtility.CreateFullPackageAsync(packageSource.FullName, "packageA", "2.15.3");

                // Act
                var command = new RestoreCommand(request);
                RestoreResult result = await command.ExecuteAsync();
                await result.CommitAsync(logger, CancellationToken.None);
                ProjectPackages packages = await project.GetInstalledAndTransitivePackagesAsync(includeTransitiveOrigins: false, CancellationToken.None);

                // Assert
                Assert.True(result.Success);
                packages.InstalledPackages.Should().Contain(a => a.PackageIdentity.Equals(new PackageIdentity("packageA", new NuGetVersion("2.15.3"))));
                packages.TransitivePackages.Should().BeEmpty();
            }
        }

        [Fact]
        public async Task GetTransitivePackagesAsync_WithTransitivePackageReferences_ReturnsPackageIdentitiesFromCache()
        {
            using (TestDirectory testDirectory = TestDirectory.Create())
            {
                // Setup
                var projectName = "project1";
                string projectFullPath = Path.Combine(testDirectory.Path, projectName + ".csproj");

                // Project
                var projectCache = new ProjectSystemCache();
                IVsProjectAdapter projectAdapter = Mock.Of<IVsProjectAdapter>();
                CpsPackageReferenceProject project = CreateCpsPackageReferenceProject(projectName, projectFullPath, projectCache);

                ProjectNames projectNames = GetTestProjectNames(projectFullPath, projectName);
                PackageSpec packageSpec = GetPackageSpec(projectName, projectFullPath, "[2.0.0, )");

                // Restore info
                DependencyGraphSpec projectRestoreInfo = ProjectTestHelpers.GetDGSpecForAllProjects(packageSpec);
                projectCache.AddProjectRestoreInfo(projectNames, projectRestoreInfo, new List<IAssetsLogMessage>());
                projectCache.AddProject(projectNames, projectAdapter, project).Should().BeTrue();

                // Package directories
                var sources = new List<PackageSource>();
                var packagesDir = new DirectoryInfo(Path.Combine(testDirectory, "globalPackages"));
                var packageSource = new DirectoryInfo(Path.Combine(testDirectory, "packageSource"));
                packagesDir.Create();
                packageSource.Create();
                sources.Add(new PackageSource(packageSource.FullName));
                string lockFilePath = Path.Combine(testDirectory, "project.assets.json");

                var logger = new TestLogger();
                var request = new TestRestoreRequest(packageSpec, sources, packagesDir.FullName, logger)
                {
                    LockFilePath = lockFilePath
                };

                await SimpleTestPackageUtility.CreateFullPackageAsync(packageSource.FullName, "packageB", "1.0.0");
                await SimpleTestPackageUtility.CreateFullPackageAsync(
                    packageSource.FullName,
                    "packageA",
                    "2.15.3",
                    new Packaging.Core.PackageDependency[]
                    {
                        new Packaging.Core.PackageDependency("packageB", VersionRange.Parse("1.0.0"))
                    });

                // Act
                var command = new RestoreCommand(request);
                RestoreResult result = await command.ExecuteAsync();
                await result.CommitAsync(logger, CancellationToken.None);
                ProjectPackages packages = await project.GetInstalledAndTransitivePackagesAsync(includeTransitiveOrigins: false, CancellationToken.None);
                DateTime lastWriteTime = File.GetLastWriteTimeUtc(lockFilePath);
                File.SetLastWriteTimeUtc(lockFilePath, lastWriteTime);
                ProjectPackages cache_packages = await project.GetInstalledAndTransitivePackagesAsync(includeTransitiveOrigins: false, CancellationToken.None);

                // Assert
                Assert.True(result.Success);
                cache_packages.InstalledPackages.Should().Contain(a => a.PackageIdentity.Equals(new PackageIdentity("packageA", new NuGetVersion("2.15.3"))));
                cache_packages.TransitivePackages.Should().Contain(a => a.PackageIdentity.Equals(new PackageIdentity("packageB", new NuGetVersion("1.0.0"))));
                Assert.True(lastWriteTime == File.GetLastWriteTimeUtc(lockFilePath));
            }
        }

        internal static async Task<IPackageReferenceProject> PrepareTestProjectAsync(SimpleTestPathContext rootDir, ITestOutputHelper output = null, string projectName = "project1")
        {
            // Setup
            string projectDir = Path.Combine(rootDir.SolutionRoot, projectName);
            string projectFullPath = Path.Combine(projectDir, projectName + ".csproj");
            Directory.CreateDirectory(projectDir);

            // Project
            var projectCache = new ProjectSystemCache();
            CpsPackageReferenceProject project = CreateCpsPackageReferenceProject(projectName, projectFullPath, projectCache);

            ProjectNames projectNames = GetTestProjectNames(projectFullPath, projectName);
            PackageSpec packageSpec = GetPackageSpec(projectName, projectFullPath, "[2.15.3, )");

            // Restore info
            DependencyGraphSpec projectRestoreInfo = ProjectTestHelpers.GetDGSpecForAllProjects(packageSpec);
            projectCache.AddProjectRestoreInfo(projectNames, projectRestoreInfo, new List<IAssetsLogMessage>());
            IVsProjectAdapter projectAdapter = Mock.Of<IVsProjectAdapter>();
            projectCache.AddProject(projectNames, projectAdapter, project).Should().BeTrue();

            var logger = output == null ? new TestLogger() : new TestLogger(output);
            // Package sources
            var sources = new List<PackageSource>
            {
                new PackageSource(rootDir.PackageSource)
            };
            var request = new TestRestoreRequest(packageSpec, sources, rootDir.UserPackagesFolder, logger)
            {
                LockFilePath = Path.Combine(projectDir, "project.assets.json")
            };

            var command = new RestoreCommand(request);
            RestoreResult result = await command.ExecuteAsync();
            await result.CommitAsync(logger, CancellationToken.None);

            return project;
        }

        [Fact]
        public async Task GetInstalledAndTransitivePackagesAsync_AlternateCallsToExcludeAndIncludeTransitiveOriginsInParallel_ReturnsCorrectDataAsync()
        {
            using var rootDir = new SimpleTestPathContext();

            // Arrange
            await CreatePackagesAsync(rootDir);
            IPackageReferenceProject project = await PrepareTestProjectAsync(rootDir);
            List<Task> tasks = new List<Task>();
            for (int i = 0; i < 5; i++)
            {
                tasks.Add(TestTransitiveOriginsAlternateAsync(project));
            }

            // Act
            Parallel.ForEach(tasks, t =>
            {
                if (t.Status == TaskStatus.Created)
                {
                    t.Start();
                }
            });

            // Assert inside tasks
            await Task.WhenAll(tasks);
        }

        [Fact]
        public async Task GetInstalledAndTransitivePackagesAsync_AlternateCallsToExcludeAndIncludeTransitiveOriginsMultipleTimes_ReturnsCorrectDataAsync()
        {
            using var rootDir = new SimpleTestPathContext();

            // Arrange
            await CreatePackagesAsync(rootDir);
            IPackageReferenceProject project = await PrepareTestProjectAsync(rootDir);

            for (int i = 0; i < 20; i++)
            {
                await TestTransitiveOriginsAlternateAsync(project);
            }
        }

        [Fact]
        public async Task GetInstalledAndTransitivePackagesAsync_SimulateBrowseTabAndSwitchToInstalledTab_InstalledTabCallHasTransitiveOriginsDataAsync()
        {
            using var rootDir = new SimpleTestPathContext();

            // Setup
            await CreatePackagesAsync(rootDir);
            IPackageReferenceProject project = await PrepareTestProjectAsync(rootDir);

            // Act I: Browse Tab: No need of transitive origins data
            ProjectPackages packagesBrowseTab = await project.GetInstalledAndTransitivePackagesAsync(includeTransitiveOrigins: false, CancellationToken.None);

            // Assert I: No need of transitive origins data
            Assert.NotEmpty(packagesBrowseTab.InstalledPackages);
            Assert.NotEmpty(packagesBrowseTab.TransitivePackages);
            Assert.All(packagesBrowseTab.TransitivePackages, pkg => Assert.Empty(pkg.TransitiveOrigins));

            // Act II: Suppose we switch to Installed tab. Then, we need transitive origins data
            ProjectPackages packagesInstalledTab = await project.GetInstalledAndTransitivePackagesAsync(includeTransitiveOrigins: true, CancellationToken.None);

            // Assert II: We need transitive data
            Assert.NotEmpty(packagesInstalledTab.InstalledPackages);
            Assert.NotEmpty(packagesInstalledTab.TransitivePackages);
            Assert.All(packagesInstalledTab.TransitivePackages, pkg => Assert.NotEmpty(pkg.TransitiveOrigins));
        }

        [Fact]
        public async Task GetInstalledAndTransitivePackagesAsync_ProjectReferenceWithPackageWithDependencies_ReturnsTransitivePackagesAsync()
        {
            // Project2 -> Project1 -> PackageA (1.0.0)
            // PackageA (1.0.0) -> PackageB (1.0.0)

            // Arrange
            using var rootDir = new SimpleTestPathContext();

            await CreatePackagesAsync(rootDir, packageAVersion: "1.0.0");

            PackageSpec prj1Spec = ProjectTestHelpers.GetPackageSpec("Project1", rootDir.SolutionRoot, framework: "net6.0", dependencyName: "PackageA");
            PackageSpec prj2Spec = ProjectTestHelpers.GetPackageSpec("Project2", rootDir.SolutionRoot, framework: "net6.0").WithTestProjectReference(prj1Spec);

            await RestorePackageSpecsAsync(rootDir, output: null, prj1Spec, prj2Spec);

            CpsPackageReferenceProject project2 = PrepareCpsRestoredProject(prj2Spec);

            // Act
            ProjectPackages result = await project2.GetInstalledAndTransitivePackagesAsync(includeTransitiveOrigins: true, CancellationToken.None);

            // Assert
            Assert.Empty(result.InstalledPackages); // No installed packages
            // Transitive packages have dependency data
            // This algorithm will consider them as transitive packages, since some transitive packages contain dependencies
            // Those transitive packages are filtered out in PM UI
            Assert.NotEmpty(result.TransitivePackages);
        }

        [Fact]
        public async Task GetInstalledAndTransitivePackagesAsync_ProjectReferenceWithSinglePackage_EmptyInstalledAndTransitivePackagesAsync()
        {
            // Project2 -> Project1 -> PackageB (1.0.0)

            // Arrange
            using var rootDir = new SimpleTestPathContext();

            await CreatePackagesAsync(rootDir, packageBVersion: "1.0.0");

            PackageSpec prj1Spec = ProjectTestHelpers.GetPackageSpec("Project1", rootDir.SolutionRoot, framework: "netstandard2.0", dependencyName: "PackageB");
            PackageSpec prj2Spec = ProjectTestHelpers.GetPackageSpec("Project2", rootDir.SolutionRoot, framework: "netstandard2.0").WithTestProjectReference(prj1Spec);

            await RestorePackageSpecsAsync(rootDir, output: null, prj1Spec, prj2Spec);

            CpsPackageReferenceProject project2 = PrepareCpsRestoredProject(prj2Spec);

            // Act
            ProjectPackages result = await project2.GetInstalledAndTransitivePackagesAsync(includeTransitiveOrigins: true, CancellationToken.None);

            // Assert
            Assert.Empty(result.InstalledPackages); // No installed packages
            Assert.Equal(1, result.TransitivePackages.Count);

            var origins = result.TransitivePackages[0].TransitiveOrigins.Select(transitivePackage => transitivePackage.PackageIdentity.Id);
            Assert.Contains("Project1", origins);
        }

        [Fact]
        public async Task GetInstalledAndTransitivePackagesAsync_ProjectReferencesWithSamePackage_InstalledAndEmptyTransitivePackagesAsync()
        {
            // Project2 -> PackageB (1.0.0)
            //          -> Project1 -> PackageB (1.0.0)

            // Arrange
            using var rootDir = new SimpleTestPathContext();

            await CreatePackagesAsync(rootDir, packageAVersion: "1.0.0");

            PackageSpec prj1Spec = ProjectTestHelpers.GetPackageSpec("Project1", rootDir.SolutionRoot, framework: "netstandard2.0", dependencyName: "PackageB");
            PackageSpec prj2Spec = ProjectTestHelpers.GetPackageSpec("Project2", rootDir.SolutionRoot, framework: "netstandard2.0", dependencyName: "PackageB").WithTestProjectReference(prj1Spec);

            await RestorePackageSpecsAsync(rootDir, output: null, prj1Spec, prj2Spec);

            CpsPackageReferenceProject project2 = PrepareCpsRestoredProject(prj2Spec);

            // Act
            ProjectPackages result = await project2.GetInstalledAndTransitivePackagesAsync(includeTransitiveOrigins: true, CancellationToken.None);

            // Assert
            Assert.Equal(1, result.InstalledPackages.Count); // No installed packages
            Assert.Empty(result.TransitivePackages);
        }

        [Fact]
        public async Task GetInstalledAndTransitivePackagesAsync_MutipleProjectReferencesWithSamePackage_EmptyInstalledAndTransitivePackagesAsync()
        {
            // Project3 -> Project2 ->  PackageA (1.0.0) -> PackageB (1.0.0)
            //                          Project1 -> PackageB (1.0.0)

            // Arrange
            using var rootDir = new SimpleTestPathContext();

            await CreatePackagesAsync(rootDir, packageAVersion: "1.0.0");

            PackageSpec prj1Spec = ProjectTestHelpers.GetPackageSpec("Project1", rootDir.SolutionRoot, framework: "netstandard2.0", dependencyName: "PackageB");
            PackageSpec prj2Spec = ProjectTestHelpers.GetPackageSpec("Project2", rootDir.SolutionRoot, framework: "netstandard2.0", dependencyName: "PackageA").WithTestProjectReference(prj1Spec);
            PackageSpec prj3Spec = ProjectTestHelpers.GetPackageSpec("Project3", rootDir.SolutionRoot, framework: "netstandard2.0").WithTestProjectReference(prj2Spec);

            await RestorePackageSpecsAsync(rootDir, output: null, prj1Spec, prj2Spec, prj3Spec);

            CpsPackageReferenceProject project3 = PrepareCpsRestoredProject(prj3Spec);

            // Act
            ProjectPackages result = await project3.GetInstalledAndTransitivePackagesAsync(includeTransitiveOrigins: true, CancellationToken.None);

            // Assert
            Assert.Empty(result.InstalledPackages); // No installed packages
            Assert.Equal(2, result.TransitivePackages.Count);

            foreach (var transitivePackage in result.TransitivePackages)
            {
                var origins = transitivePackage.TransitiveOrigins.Select(transitivePackage => transitivePackage.PackageIdentity.Id).ToList();
                if (transitivePackage.PackageIdentity.Id.Equals("PackageB") && origins.Count == 2)
                {
                    Assert.Contains("Project1", origins[0]);
                    Assert.Contains("Project2", origins[1]);
                }
                if (transitivePackage.PackageIdentity.Id.Equals("PackageA") && origins.Count == 1)
                {
                    Assert.Contains("Project2", origins);
                }
            }
        }

        [Fact]
        public async Task GetInstalledAndTransitivePackagesAsync_WithProjectReference_MultipleTransitivePackagesAsync()
        {
            // Project2 -> Project1 -> PackageA (1.0.0) -> PackageB (1.0.0)

            // Arrange
            using var rootDir = new SimpleTestPathContext();

            await CreatePackagesAsync(rootDir, packageAVersion: "1.0.0");

            PackageSpec prj1Spec = ProjectTestHelpers.GetPackageSpec("Project1", rootDir.SolutionRoot, framework: "netstandard2.0", dependencyName: "PackageA");
            PackageSpec prj2Spec = ProjectTestHelpers.GetPackageSpec("Project2", rootDir.SolutionRoot, framework: "netstandard2.0").WithTestProjectReference(prj1Spec);

            await RestorePackageSpecsAsync(rootDir, output: null, prj1Spec, prj2Spec);

            CpsPackageReferenceProject project2 = PrepareCpsRestoredProject(prj2Spec);

            // Act
            ProjectPackages result = await project2.GetInstalledAndTransitivePackagesAsync(includeTransitiveOrigins: true, CancellationToken.None);

            // Assert
            Assert.Empty(result.InstalledPackages); // No installed packages
            Assert.Equal(2, result.TransitivePackages.Count); // A and B are transitive
        }

        [Fact]
        public async Task GetInstalledAndTransitivePackagesAsync_WithPackagesConfigProjectReferenceAsync()
        {
            // Project2 (PR) -> Project1 (PC)

            // Arrange
            using var rootDir = new SimpleTestPathContext();

            await CreatePackagesAsync(rootDir, packageAVersion: "1.0.0");

            PackageSpec prj1Spec = ProjectTestHelpers.GetPackagesConfigPackageSpec("Project1", rootDir.SolutionRoot, framework: "netstandard2.0");
            PackageSpec prj2Spec = ProjectTestHelpers.GetPackageSpec("Project2", rootDir.SolutionRoot, framework: "netstandard2.0").WithTestProjectReference(prj1Spec);

            await RestorePackageSpecsAsync(rootDir, output: null, prj1Spec, prj2Spec);

            CpsPackageReferenceProject project2 = PrepareCpsRestoredProject(prj2Spec);

            // Act
            ProjectPackages result = await project2.GetInstalledAndTransitivePackagesAsync(includeTransitiveOrigins: true, CancellationToken.None);

            // Assert
            Assert.Empty(result.InstalledPackages); // No installed packages
            Assert.Empty(result.TransitivePackages); // No transitive packages
        }

        [Fact]
        public async Task GetInstalledAndTransitivePackagesAsync_SimulatePackageInstallation_UpdatesDataAsync()
        {
            using var rootDir = new SimpleTestPathContext();
            await CreatePackagesAsync(rootDir, packageAVersion: "1.0.0");
            IProjectSystemCache cache = new ProjectSystemCache();

            // Arrange I: A project with no packages
            PackageSpec initialProjectSpec = ProjectTestHelpers.GetPackageSpec("MyProject", rootDir.SolutionRoot, framework: "net472");
            await RestorePackageSpecsAsync(rootDir, output: null, initialProjectSpec);

            CpsPackageReferenceProject project = PrepareCpsRestoredProject(initialProjectSpec, cache);

            // Act I
            ProjectPackages result = await project.GetInstalledAndTransitivePackagesAsync(includeTransitiveOrigins: true, CancellationToken.None);

            // Assert I
            Assert.Empty(result.InstalledPackages);
            Assert.Empty(result.TransitivePackages);

            // Arrange II: Simulate package install by changing the package spec
            PackageSpec projectSpec = ProjectTestHelpers.GetPackageSpec("MyProject", rootDir.SolutionRoot, framework: "net472", dependencyName: "PackageA");
            await RestorePackageSpecsAsync(rootDir, output: null, projectSpec);
            UpdateProjectSystemCache(cache, projectSpec, project);

            // Act II
            ProjectPackages result2 = await project.GetInstalledAndTransitivePackagesAsync(includeTransitiveOrigins: true, CancellationToken.None);

            // Assert II
            Assert.NotEmpty(result2.InstalledPackages);
            Assert.NotEmpty(result2.TransitivePackages);
            Assert.All(result2.TransitivePackages, pkg => Assert.NotEmpty(pkg.TransitiveOrigins));
        }

        [Fact]
        public async Task GetInstalledAndTransitivePackagesAsync_TransitiveOriginsParamSetToTrue_ReturnsPackagesWithTransitiveOriginsAsync()
        {
            using SimpleTestPathContext rootDir = new();

            // Arrange
            await CreatePackagesAsync(rootDir);
            IPackageReferenceProject project = await PrepareTestProjectAsync(rootDir);

            // Act
            ProjectPackages packages = await project.GetInstalledAndTransitivePackagesAsync(includeTransitiveOrigins: true, CancellationToken.None);

            // Assert
            Assert.NotEmpty(packages.InstalledPackages);
            Assert.NotEmpty(packages.TransitivePackages);
            Assert.All(packages.TransitivePackages, pkg => Assert.NotEmpty(pkg.TransitiveOrigins));
        }

        [Theory]
        [InlineData(false, false, 0, 0)]
        [InlineData(false, true, 0, 0)]
        [InlineData(true, false, 1, 0)]
        [InlineData(true, true, 1, 1)]
        public async Task GetInstalledAndTransitivePackagesAsync_InstalledAndTransitiveComputationNotNeeded_ReturnsExpectedTransitiveData(bool includeTransitivePackages, bool includeTransitiveOrigins, int transitivePackagesCount, int transitiveOriginsCount)
        {
            using var rootDir = new SimpleTestPathContext();
            await CreatePackagesAsync(rootDir, packageAVersion: "1.0.0");
            IProjectSystemCache cache = new ProjectSystemCache();

            // Arrange
            PackageSpec initialProjectSpec = ProjectTestHelpers.GetPackageSpec("MyProject", rootDir.SolutionRoot, framework: "net472", dependencyName: "PackageA");
            await RestorePackageSpecsAsync(rootDir, output: null, initialProjectSpec);

            CpsPackageReferenceProject project = PrepareCpsRestoredProject(initialProjectSpec, cache);
            var installedPackagesBefore = await project.GetInstalledPackagesAsync(CancellationToken.None); // This will change IsInstalledAndTransitiveComputationNeeded to false

            // Act
            ProjectPackages installedAndTransitivePackagesAfter = await project.GetInstalledAndTransitivePackagesAsync(includeTransitivePackages, includeTransitiveOrigins, CancellationToken.None);

            // Assert
            Assert.Equal(installedAndTransitivePackagesAfter.InstalledPackages.Count, installedPackagesBefore.Count());
            Assert.Equal(1, installedAndTransitivePackagesAfter.InstalledPackages.Count);
            Assert.Equal(transitivePackagesCount, installedAndTransitivePackagesAfter.TransitivePackages.Count);
            Assert.All(installedAndTransitivePackagesAfter.TransitivePackages, pkg => Assert.Equal(transitiveOriginsCount, pkg.TransitiveOrigins.Count()));
        }

        [Theory]
        [InlineData(false, false, 0, 0)]
        [InlineData(false, true, 0, 0)]
        [InlineData(true, false, 1, 0)]
        [InlineData(true, true, 1, 1)]
        public async Task GetInstalledAndTransitivePackagesAsync_IsInstalledAndTransitiveComputationNeeded_ReturnsExpectedTransitiveData(bool includeTransitivePackages, bool includeTransitiveOrigins, int transitivePackagesCount, int transitiveOriginsCount)
        {
            using var rootDir = new SimpleTestPathContext();
            await CreatePackagesAsync(rootDir, packageAVersion: "1.0.0");
            IProjectSystemCache cache = new ProjectSystemCache();

            // Arrange
            PackageSpec initialProjectSpec = ProjectTestHelpers.GetPackageSpec("MyProject", rootDir.SolutionRoot, framework: "net472", dependencyName: "PackageA");
            await RestorePackageSpecsAsync(rootDir, output: null, initialProjectSpec);

            CpsPackageReferenceProject project = PrepareCpsRestoredProject(initialProjectSpec, cache);

            // Act
            ProjectPackages installedAndTransitivePackages = await project.GetInstalledAndTransitivePackagesAsync(includeTransitivePackages, includeTransitiveOrigins, CancellationToken.None);

            // Assert
            Assert.Equal(1, installedAndTransitivePackages.InstalledPackages.Count);
            Assert.Equal(transitivePackagesCount, installedAndTransitivePackages.TransitivePackages.Count);
            Assert.All(installedAndTransitivePackages.TransitivePackages, pkg => Assert.Equal(transitiveOriginsCount, pkg.TransitiveOrigins.Count()));
        }

        [Fact]
        public async Task GetInstalledAndTransitivePackagesAsync_NoRestore_ReturnsOnlyTopLevelPackages()
        {
            using var rootDir = new SimpleTestPathContext();
            await CreatePackagesAsync(rootDir, packageAVersion: "1.0.0"); // PackageA is top level package with a dependency on PackageB
            IProjectSystemCache cache = new ProjectSystemCache();

            // Arrange
            PackageSpec initialProjectSpec = ProjectTestHelpers.GetPackageSpec("MyProject", rootDir.SolutionRoot, framework: "net472", dependencyName: "PackageA");

            CpsPackageReferenceProject project = PrepareCpsRestoredProject(initialProjectSpec, cache);

            // Act
            ProjectPackages result = await project.GetInstalledAndTransitivePackagesAsync(true, true, CancellationToken.None);

            // Assert
            Assert.Equal(1, result.InstalledPackages.Count);
            Assert.Empty(result.TransitivePackages);
        }

        [Fact]
        public async Task GetInstalledAndTransitivePackagesAsync_IncludeTransitiveSetToTrue_WhenCacheWasCleared_ReturnsExpectedTransitiveData()
        {
            using var rootDir = new SimpleTestPathContext();
            await CreatePackagesAsync(rootDir, packageAVersion: "1.0.0");
            IProjectSystemCache cache = new ProjectSystemCache();

            // Arrange
            PackageSpec initialProjectSpec = ProjectTestHelpers.GetPackageSpec("MyProject", rootDir.SolutionRoot, framework: "net472", dependencyName: "PackageA");
            await RestorePackageSpecsAsync(rootDir, output: null, initialProjectSpec);

            CpsPackageReferenceProject project = PrepareCpsRestoredProject(initialProjectSpec, cache);

            // Act
            ProjectPackages result = await project.GetInstalledAndTransitivePackagesAsync(true, true, CancellationToken.None);

            // do a restore
            project = PrepareCpsRestoredProject(initialProjectSpec, cache);
            result = await project.GetInstalledAndTransitivePackagesAsync(false, false, CancellationToken.None);

            // Assert I: Cache for transitive was cleared
            Assert.Equal(1, result.InstalledPackages.Count);
            Assert.Equal(0, result.TransitivePackages.Count);
            Assert.All(result.TransitivePackages, pkg => Assert.Equal(0, pkg.TransitiveOrigins.Count()));

            result = await project.GetInstalledAndTransitivePackagesAsync(true, true, CancellationToken.None);

            // Assert II: Transitive packages calculated correctly
            Assert.Equal(1, result.InstalledPackages.Count);
            Assert.Equal(1, result.TransitivePackages.Count);
            Assert.All(result.TransitivePackages, pkg => Assert.Equal(1, pkg.TransitiveOrigins.Count()));
        }

        [Fact]
        public async Task GetInstalledAndTransitivePackagesAsync_TransitiveOriginsParamSetToFalse_ReturnsPackagesWithoutTransitiveOriginsAsync()
        {
            using SimpleTestPathContext rootDir = new();

            // Arrange
            await CreatePackagesAsync(rootDir);
            IPackageReferenceProject project = await PrepareTestProjectAsync(rootDir);

            // Act
            ProjectPackages packages = await project.GetInstalledAndTransitivePackagesAsync(includeTransitiveOrigins: false, CancellationToken.None);

            // Assert
            Assert.NotEmpty(packages.InstalledPackages);
            Assert.NotEmpty(packages.TransitivePackages);
            Assert.All(packages.TransitivePackages, pkg => Assert.Empty(pkg.TransitiveOrigins));
        }

        [Fact]
        public async Task GetPackageSpecsAndAdditionalMessagesAsync_NoRestoreInfoInSolutionManager_ThrowsProjectNotNominatedException()
        {
            // Arrange
            var projectSystemCache = new Mock<IProjectSystemCache>();
            var unconfiguredProject = new Mock<UnconfiguredProject>();
            var nugetProjectServices = new Mock<INuGetProjectServices>();
            var projectGuid = Guid.NewGuid();
            var cacheContext = new DependencyGraphCacheContext();

            // Act & Assert
            var target = new CpsPackageReferenceProject(
                "TestProject",
                @"src\TestProject\TestProject.csproj",
                @"c:\repo\src\TestProject\TestProject.csproj",
                projectSystemCache.Object,
                unconfiguredProject.Object,
                nugetProjectServices.Object,
                projectGuid.ToString());

            // VS extensibility APIs depend on this specific exception type, so if CpsPackageReferenceProject is refactored, either
            // this exception type needs to be maintained, or the VS API implementations need to be updated as well.
            await Assert.ThrowsAnyAsync<ProjectNotNominatedException>(() => target.GetPackageSpecsAndAdditionalMessagesAsync(cacheContext));
        }

        [Fact]
        public async Task TestCheckForMissingAssetsFile()
        {
            using (var testDirectory = TestDirectory.Create())
            using (var testSolutionManager = new TestSolutionManager())
            {
                // Project
                var projectName = "a";
                var projectFullPath = Path.Combine(testDirectory.Path, projectName, projectName + ".csproj");
                var projectCache = new ProjectSystemCache();
                IVsProjectAdapter projectAdapter = (new Mock<IVsProjectAdapter>()).Object;

                var project = TestCpsPackageReferenceProject.CreateTestCpsPackageReferenceProject(projectName, projectFullPath, projectCache);
                testSolutionManager.NuGetProjects.Add(project);

                var projectNames = GetTestProjectNames(projectFullPath, projectName);
                var packageSpec = GetPackageSpec(projectName, projectFullPath, "[1.0.0, )");

                // Restore info
                var projectRestoreInfo = ProjectTestHelpers.GetDGSpecForAllProjects(packageSpec);
                projectCache.AddProjectRestoreInfo(projectNames, projectRestoreInfo, new List<IAssetsLogMessage>());
                projectCache.AddProject(projectNames, projectAdapter, project).Should().BeTrue();

                // Restore Manager setup
                var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV3OnlySourceRepositoryProvider();
                var testSettings = Configuration.NullSettings.Instance;
                var packageRestoreManager = new PackageRestoreManager(
                    sourceRepositoryProvider,
                    testSettings,
                    testSolutionManager);

                var token = CancellationToken.None;

                var assetsFileMissingEventCount = 0;
                var assetsFileMissing = false;

                packageRestoreManager.AssetsFileMissingStatusChanged += delegate (object sender, bool isAssetsFileMissing)
                {
                    assetsFileMissingEventCount++;
                    assetsFileMissing = isAssetsFileMissing;
                };

                // Package directories
                var sources = new List<PackageSource>();
                var packagesDir = new DirectoryInfo(Path.Combine(testDirectory, "globalPackages"));
                var packageSource = new DirectoryInfo(Path.Combine(testDirectory, "packageSource"));
                packagesDir.Create();
                packageSource.Create();
                sources.Add(new PackageSource(packageSource.FullName));

                var logger = new TestLogger();
                var request = new TestRestoreRequest(packageSpec, sources, packagesDir.FullName, logger)
                {
                    LockFilePath = Path.Combine(testDirectory, projectName, "project.assets.json")
                };

                // Act
                var command = new RestoreCommand(request);
                var result = await command.ExecuteAsync();
                await result.CommitAsync(logger, CancellationToken.None);

                // Assert
                packageRestoreManager.RaiseAssetsFileMissingEventForProjectAsync(false);

                Assert.Equal(1, assetsFileMissingEventCount);
                Assert.False(assetsFileMissing);

                // Act
                // Delete assets file
                File.Delete(Path.Combine(testDirectory, projectName, "project.assets.json"));
                packageRestoreManager.RaiseAssetsFileMissingEventForProjectAsync(true);

                Assert.Equal(2, assetsFileMissingEventCount);
                Assert.True(assetsFileMissing);
            }
        }

        [Fact]
        public async Task TestPackageManager_ExecuteNuGetProjectActionsAsync_WithProgressReporter_WhenPackageIsInstalled_AssetsFileWritesAreReported()
        {
            using var pathContext = new SimpleTestPathContext();
            using var testSolutionManager = new TestSolutionManager();

            // Arrange - Setup project 
            var packageA = new SimpleTestPackageContext("packageA", "1.0.0");
            await SimpleTestPackageUtility.CreateFolderFeedV3Async(pathContext.PackageSource, packageA);
            var sources = new PackageSource[] { new PackageSource(pathContext.PackageSource) };
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(sources);
            var settings = Settings.LoadDefaultSettings(pathContext.SolutionRoot);
            var projectCache = new ProjectSystemCache();
            var projectName = "project";
            var dependencyGraphSpec = ProjectTestHelpers.GetDGSpecForAllProjects(ProjectTestHelpers.GetPackageSpec(settings, projectName, rootPath: pathContext.SolutionRoot));
            var projectFullPath = dependencyGraphSpec.Projects[0].FilePath;
            var cpsPackageReferenceProject = TestCpsPackageReferenceProject.CreateTestCpsPackageReferenceProject(projectName, projectFullPath, projectCache);
            testSolutionManager.NuGetProjects.Add(cpsPackageReferenceProject);
            TestCpsPackageReferenceProject.AddProjectDetailsToCache(projectCache, dependencyGraphSpec, cpsPackageReferenceProject, GetTestProjectNames(projectFullPath, projectName));

            //  Setup expectations
            var progressReporter = new Mock<IRestoreProgressReporter>(MockBehavior.Strict);
            progressReporter.Setup(e => e.StartProjectUpdate(It.Is<string>(path => path == projectFullPath), It.IsAny<IReadOnlyList<string>>())).Verifiable();
            progressReporter.Setup(e => e.EndProjectUpdate(It.Is<string>(path => path == projectFullPath), It.IsAny<IReadOnlyList<string>>())).Verifiable();

            var nuGetPackageManager = new NuGetPackageManager(sourceRepositoryProvider, settings, testSolutionManager, new TestDeleteOnRestartManager(), progressReporter.Object);
            var actions = await nuGetPackageManager.PreviewInstallPackageAsync(cpsPackageReferenceProject, packageA.Id, new ResolutionContext(), new TestNuGetProjectContext(),
                    sourceRepositoryProvider.GetRepositories(), sourceRepositoryProvider.GetRepositories(), CancellationToken.None);

            // Preconditions
            actions.Should().HaveCount(1);

            // Act
            await nuGetPackageManager.ExecuteNuGetProjectActionsAsync(
                cpsPackageReferenceProject,
                actions,
                new TestNuGetProjectContext(),
                new SourceCacheContext(),
                CancellationToken.None);

            // Assert 
            progressReporter.VerifyAll();
        }

        [Fact]
        public async Task TestPackageManager_ExecuteNuGetProjectActionsAsync_WithProgressReporter_WhenChildProjectChanges_AssetsFileWritesForParentsAreReported()
        {
            using var pathContext = new SimpleTestPathContext();
            using var testSolutionManager = new TestSolutionManager();

            // Arrange - Setup project
            var packageA = new SimpleTestPackageContext("packageA", "1.0.0");
            await SimpleTestPackageUtility.CreateFolderFeedV3Async(pathContext.PackageSource, packageA);
            var sources = new PackageSource[] { new PackageSource(pathContext.PackageSource) };
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(sources);
            var settings = Settings.LoadDefaultSettings(pathContext.SolutionRoot);
            var projectCache = new ProjectSystemCache();

            // Child project
            var childProject = "childProject";
            var childProjectPackageSpec = ProjectTestHelpers.GetPackageSpec(settings, childProject, rootPath: pathContext.SolutionRoot);
            var childDependencyGraphSpec = ProjectTestHelpers.GetDGSpecForAllProjects(childProjectPackageSpec);
            var childProjectFullPath = childDependencyGraphSpec.Projects[0].FilePath;
            var childPackageReferenceProject = TestCpsPackageReferenceProject.CreateTestCpsPackageReferenceProject(childProject, childProjectFullPath, projectCache);

            // Parent project
            var parentProject = "parentProject";
            var parentProjectPackageSpec = ProjectTestHelpers.GetPackageSpec(settings, parentProject, rootPath: pathContext.SolutionRoot);
            parentProjectPackageSpec = parentProjectPackageSpec.WithTestProjectReference(childProjectPackageSpec);
            var parentDependencyGraphSpec = ProjectTestHelpers.GetDGSpecForAllProjects(parentProjectPackageSpec);
            var parentProjectFullPath = parentDependencyGraphSpec.Projects[0].FilePath;
            var parentPackageReferenceProject = TestCpsPackageReferenceProject.CreateTestCpsPackageReferenceProject(parentProject, parentProjectFullPath, projectCache);

            // Populate caches
            TestCpsPackageReferenceProject.AddProjectDetailsToCache(projectCache, parentDependencyGraphSpec, parentPackageReferenceProject, GetTestProjectNames(parentProjectFullPath, parentProject));
            TestCpsPackageReferenceProject.AddProjectDetailsToCache(projectCache, childDependencyGraphSpec, childPackageReferenceProject, GetTestProjectNames(childProjectFullPath, childProject));
            testSolutionManager.NuGetProjects.Add(parentPackageReferenceProject);
            testSolutionManager.NuGetProjects.Add(childPackageReferenceProject);

            //  Setup expectations
            var progressReporter = new Mock<IRestoreProgressReporter>(MockBehavior.Strict);
            progressReporter.Setup(e => e.StartProjectUpdate(It.Is<string>(path => path == parentProjectFullPath), It.IsAny<IReadOnlyList<string>>())).Verifiable();
            progressReporter.Setup(e => e.EndProjectUpdate(It.Is<string>(path => path == parentProjectFullPath), It.IsAny<IReadOnlyList<string>>())).Verifiable();
            progressReporter.Setup(e => e.StartProjectUpdate(It.Is<string>(path => path == childProjectFullPath), It.IsAny<IReadOnlyList<string>>())).Verifiable();
            progressReporter.Setup(e => e.EndProjectUpdate(It.Is<string>(path => path == childProjectFullPath), It.IsAny<IReadOnlyList<string>>())).Verifiable();

            var nuGetPackageManager = new NuGetPackageManager(sourceRepositoryProvider, settings, testSolutionManager, new TestDeleteOnRestartManager(), progressReporter.Object);
            var actions = await nuGetPackageManager.PreviewInstallPackageAsync(childPackageReferenceProject, packageA.Id, new ResolutionContext(), new TestNuGetProjectContext(),
                    sourceRepositoryProvider.GetRepositories(), sourceRepositoryProvider.GetRepositories(), CancellationToken.None);

            // Preconditions
            actions.Should().HaveCount(1);

            // Act
            await nuGetPackageManager.ExecuteNuGetProjectActionsAsync(
                childPackageReferenceProject,
                actions,
                new TestNuGetProjectContext(),
                new SourceCacheContext(),
                CancellationToken.None);

            // Assert 
            progressReporter.VerifyAll();
        }

        private static PackageSpec GetPackageSpecNoPackages(string projectName, string testDirectory)
        {
            const string referenceSpec = @"
                {
                    ""frameworks"": {
                        ""net5.0"": {
                            ""dependencies"": {
                                }
                            }
                        }
                    }
                }";
            return JsonPackageSpecReader.GetPackageSpec(referenceSpec, projectName, testDirectory).WithTestRestoreMetadata();
        }

        private static PackageSpec GetPackageSpecMultipleVersions(string projectName, string testDirectory)
        {
            const string referenceSpec = @"
                {
                    ""frameworks"": {
                        ""net5.0"": {
                            ""dependencies"": {
                                    ""packageA"": {
                                    ""version"": ""[*, )"",
                                    ""target"": ""Package""
                                },
                                    ""packageB"": {
                                    ""version"": ""[1.0.0, )"",
                                    ""target"": ""Package""
                                },
                                    ""packageC"": {
                                    ""version"": ""[1.0.0, )"",
                                    ""target"": ""Package""
                                }
                            }
                        }
                    }
                }";
            return JsonPackageSpecReader.GetPackageSpec(referenceSpec, projectName, testDirectory).WithTestRestoreMetadata();
        }

        private static async Task TestTransitiveOriginsAlternateAsync(IPackageReferenceProject project)
        {
            // Act I: Transitive packages without transitive origins
            ProjectPackages packagesWithoutOrigins = await project.GetInstalledAndTransitivePackagesAsync(includeTransitiveOrigins: false, CancellationToken.None);
            // Assert I
            Assert.NotEmpty(packagesWithoutOrigins.InstalledPackages);
            Assert.NotEmpty(packagesWithoutOrigins.TransitivePackages);
            Assert.All(packagesWithoutOrigins.TransitivePackages, pkg => Assert.Empty(pkg.TransitiveOrigins));
            // Act II: Transitive packages with transitive origins
            ProjectPackages packagesWithOrigins = await project.GetInstalledAndTransitivePackagesAsync(includeTransitiveOrigins: true, CancellationToken.None);
            // Assert II
            Assert.NotEmpty(packagesWithOrigins.InstalledPackages);
            Assert.NotEmpty(packagesWithOrigins.TransitivePackages);
            Assert.All(packagesWithOrigins.TransitivePackages, pkg => Assert.NotEmpty(pkg.TransitiveOrigins));
        }
    }
}
