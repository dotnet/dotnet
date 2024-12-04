// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.ProjectSystem;
using Microsoft.VisualStudio.Sdk.TestFramework;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Threading;
using Moq;
using NuGet.Commands;
using NuGet.Commands.Test;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.Packaging.Core;
using NuGet.ProjectManagement;
using NuGet.ProjectModel;
using NuGet.Test.Utility;
using NuGet.Versioning;
using NuGet.VisualStudio;
using Test.Utility;
using Xunit;
using static NuGet.PackageManagement.VisualStudio.Test.ProjectFactories;
using Task = System.Threading.Tasks.Task;

namespace NuGet.PackageManagement.VisualStudio.Test
{
    [Collection(MockedVS.Collection)]
    public class LegacyPackageReferenceProjectTests : MockedVSCollectionTests
    {
        private readonly IVsProjectThreadingService _threadingService;

        public LegacyPackageReferenceProjectTests(GlobalServiceProvider globalServiceProvider)
            : base(globalServiceProvider)
        {
            globalServiceProvider.Reset();

            _threadingService = new TestProjectThreadingService(NuGetUIThreadHelper.JoinableTaskFactory);

            var componentModel = new Mock<IComponentModel>();
            AddService<SComponentModel>(Task.FromResult((object)componentModel.Object));
        }

        [Fact]
        public async Task GetAssetsFilePathAsync_WithValidMSBuildProjectExtensionsPath_Succeeds()
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            // Arrange
            using (var testDirectory = TestDirectory.Create())
            {
                var testMSBuildProjectExtensionsPath = Path.Combine(testDirectory, "obj");
                Directory.CreateDirectory(testMSBuildProjectExtensionsPath);
                var projectAdapter = Mock.Of<IVsProjectAdapter>();
                Mock.Get(projectAdapter)
                    .Setup(x => x.GetMSBuildProjectExtensionsPath())
                    .Returns(testMSBuildProjectExtensionsPath);

                var testProject = new LegacyPackageReferenceProject(
                    projectAdapter,
                    Guid.NewGuid().ToString(),
                    new TestProjectSystemServices(),
                    _threadingService);

                // Act
                var assetsPath = await testProject.GetAssetsFilePathAsync();

                // Assert
                Assert.Equal(Path.Combine(testMSBuildProjectExtensionsPath, "project.assets.json"), assetsPath);

                // Verify
                Mock.Get(projectAdapter)
                    .Verify(x => x.GetMSBuildProjectExtensionsPath(), Times.AtLeastOnce);
            }
        }

        [Fact]
        public async Task GetAssetsFilePathAsync_WithNoMSBuildProjectExtensionsPath_Throws()
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            // Arrange
            using (TestDirectory.Create())
            {
                var testProject = new LegacyPackageReferenceProject(
                    Mock.Of<IVsProjectAdapter>(),
                    Guid.NewGuid().ToString(),
                    new TestProjectSystemServices(),
                    _threadingService);

                // Act & Assert
                await Assert.ThrowsAsync<InvalidDataException>(
                    () => testProject.GetAssetsFilePathAsync());
            }
        }

        [Fact]
        public async Task GetCacheFilePathAsync_WithValidMSBuildProjectExtensionsPath_Succeeds()
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            // Arrange
            using (var testDirectory = TestDirectory.Create())
            {
                var testProj = "project.csproj";
                var testMSBuildProjectExtensionsPath = Path.Combine(testDirectory, "obj");
                Directory.CreateDirectory(testMSBuildProjectExtensionsPath);
                var projectAdapter = Mock.Of<IVsProjectAdapter>();
                Mock.Get(projectAdapter)
                    .Setup(x => x.GetMSBuildProjectExtensionsPath())
                    .Returns(testMSBuildProjectExtensionsPath);

                Mock.Get(projectAdapter)
                    .SetupGet(x => x.FullProjectPath)
                    .Returns(Path.Combine(testDirectory, testProj));

                var testProject = new LegacyPackageReferenceProject(
                    projectAdapter,
                    Guid.NewGuid().ToString(),
                    new TestProjectSystemServices(),
                    _threadingService);

                // Act
                var cachePath = await testProject.GetCacheFilePathAsync();

                // Assert
                Assert.Equal(Path.Combine(testMSBuildProjectExtensionsPath, NoOpRestoreUtilities.NoOpCacheFileName), cachePath);

                // Verify
                Mock.Get(projectAdapter)
                    .Verify(x => x.GetMSBuildProjectExtensionsPath(), Times.AtLeastOnce);
            }
        }

        [Fact]
        public async Task GetCacheFilePathAsync_WithNoMSBuildProjectExtensionsPath_Throws()
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            // Arrange
            using (TestDirectory.Create())
            {
                var testProject = new LegacyPackageReferenceProject(
                    Mock.Of<IVsProjectAdapter>(),
                    Guid.NewGuid().ToString(),
                    new TestProjectSystemServices(),
                    _threadingService);

                // Act & Assert
                await Assert.ThrowsAsync<InvalidDataException>(
                    () => testProject.GetCacheFilePathAsync());
            }
        }

        [Fact]
        public async Task GetCacheFilePathAsync_SwitchesToMainThread_Succeeds()
        {
            // Arrange
            using (var testDirectory = TestDirectory.Create())
            {
                var testProj = "project.csproj";
                var testMSBuildProjectExtensionsPath = Path.Combine(testDirectory, "obj");
                Directory.CreateDirectory(testMSBuildProjectExtensionsPath);
                var projectAdapter = Mock.Of<IVsProjectAdapter>();
                Mock.Get(projectAdapter)
                    .Setup(x => x.GetMSBuildProjectExtensionsPath())
                    .Returns(testMSBuildProjectExtensionsPath);

                Mock.Get(projectAdapter)
                    .SetupGet(x => x.FullProjectPath)
                    .Returns(Path.Combine(testDirectory, testProj));

                var testProject = new LegacyPackageReferenceProject(
                    projectAdapter,
                    Guid.NewGuid().ToString(),
                    new TestProjectSystemServices(),
                    _threadingService);

                // Act
                var assetsPath = await testProject.GetCacheFilePathAsync();

                // Assert
                Assert.Equal(Path.Combine(testMSBuildProjectExtensionsPath, NoOpRestoreUtilities.NoOpCacheFileName), assetsPath);

                // Verify
                Mock.Get(projectAdapter)
                    .Verify(x => x.GetMSBuildProjectExtensionsPath(), Times.AtLeastOnce);
            }
        }

        [Fact]
        public async Task GetPackageSpecsAsync_WithDefaultVersion_Succeeds()
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            // Arrange
            using (var testDirectory = TestDirectory.Create())
            {
                var projectAdapter = CreateProjectAdapter(testDirectory);
                var projectServices = new TestProjectSystemServices();

                var testProject = new LegacyPackageReferenceProject(
                    projectAdapter,
                    Guid.NewGuid().ToString(),
                    projectServices,
                    _threadingService);

                var testDependencyGraphCacheContext = new DependencyGraphCacheContext();

                // Act
                var packageSpecs = await testProject.GetPackageSpecsAsync(testDependencyGraphCacheContext);

                // Assert
                Assert.NotNull(packageSpecs);

                var actualRestoreSpec = packageSpecs.Single();
                SpecValidationUtility.ValidateProjectSpec(actualRestoreSpec);

                Assert.Equal("1.0.0", actualRestoreSpec.Version.ToString());

                // Verify
                Mock.Get(projectAdapter)
                    .VerifyGet(x => x.Version, Times.AtLeastOnce);
                Mock.Get(projectAdapter)
                    .VerifyGet(x => x.ProjectName, Times.AtLeastOnce);
                Mock.Get(projectAdapter)
                    .VerifyGet(x => x.FullProjectPath, Times.AtLeastOnce);
                Mock.Get(projectAdapter)
                    .Verify(x => x.GetTargetFramework(), Times.AtLeastOnce);
            }
        }

        [Fact]
        public async Task GetPackageSpecsAsync_WithVersion_Succeeds()
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            // Arrange
            using (var testDirectory = TestDirectory.Create())
            {
                var projectAdapter = CreateProjectAdapter(testDirectory);
                Mock.Get(projectAdapter)
                    .SetupGet(x => x.Version)
                    .Returns("2.2.3");

                var projectServices = new TestProjectSystemServices();

                var testProject = new LegacyPackageReferenceProject(
                    projectAdapter,
                    Guid.NewGuid().ToString(),
                    projectServices,
                    _threadingService);

                var testDependencyGraphCacheContext = new DependencyGraphCacheContext();

                // Act
                var packageSpecs = await testProject.GetPackageSpecsAsync(testDependencyGraphCacheContext);

                // Assert
                Assert.NotNull(packageSpecs);

                var actualRestoreSpec = packageSpecs.Single();
                SpecValidationUtility.ValidateProjectSpec(actualRestoreSpec);

                Assert.Equal("2.2.3", actualRestoreSpec.Version.ToString());

                // Verify
                Mock.Get(projectAdapter)
                    .Verify(x => x.Version, Times.AtLeastOnce);
            }
        }

        [Theory]
        [InlineData("RestorePackagesPath", "Source1;Source2", "Fallback1,Fallback2")]
        [InlineData("RestorePackagesPath", "Source2", "Fallback2")]
        [InlineData(null, "Source2", "Fallback2")]
        [InlineData("RestorePackagesPath", null, "Fallback2")]
        [InlineData("RestorePackagesPath", "Source1;Source2", null)]
        public async Task GetPackageSpecsAsync_ReadSettingsWithRelativePaths(string restorePackagesPath, string sources, string fallbackFolders)
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            // Arrange
            using (var testDirectory = TestDirectory.Create())
            {
                var projectBuildProperties = new Mock<IVsProjectBuildProperties>();
                var projectAdapter = CreateProjectAdapter(testDirectory, projectBuildProperties);

#pragma warning disable CS0618 // Type or member is obsolete
                projectBuildProperties
                    .Setup(x => x.GetPropertyValueWithDteFallback(It.Is<string>(x => x.Equals(ProjectBuildProperties.RestorePackagesPath))))
                    .Returns(restorePackagesPath);

                projectBuildProperties
                    .Setup(x => x.GetPropertyValueWithDteFallback(It.Is<string>(x => x.Equals(ProjectBuildProperties.RestoreSources))))
                    .Returns(sources);

                projectBuildProperties
                    .Setup(x => x.GetPropertyValueWithDteFallback(It.Is<string>(x => x.Equals(ProjectBuildProperties.RestoreFallbackFolders))))
                    .Returns(fallbackFolders);
#pragma warning restore CS0618 // Type or member is obsolete

                var projectServices = new TestProjectSystemServices();

                var testProject = new LegacyPackageReferenceProject(
                    projectAdapter,
                    Guid.NewGuid().ToString(),
                    projectServices,
                    _threadingService);

                var settings = NullSettings.Instance;
                var testDependencyGraphCacheContext = new DependencyGraphCacheContext(NullLogger.Instance, settings);

                // Act
                var packageSpecs = await testProject.GetPackageSpecsAsync(testDependencyGraphCacheContext);

                // Assert
                Assert.NotNull(packageSpecs);
                var actualRestoreSpec = packageSpecs.Single();
                SpecValidationUtility.ValidateProjectSpec(actualRestoreSpec);

                // Assert packagespath
                Assert.Equal(restorePackagesPath != null ? Path.Combine(testDirectory, restorePackagesPath) : SettingsUtility.GetGlobalPackagesFolder(settings), actualRestoreSpec.RestoreMetadata.PackagesPath);

                // assert sources
                var specSources = actualRestoreSpec.RestoreMetadata.Sources.Select(e => e.Source);
                var expectedSources = sources != null ? MSBuildStringUtility.Split(sources).Select(e => Path.Combine(testDirectory, e)) : SettingsUtility.GetEnabledSources(settings).Select(e => e.Source);
                Assert.True(Enumerable.SequenceEqual(expectedSources.OrderBy(t => t), specSources.OrderBy(t => t)));

                // assert fallbackfolders
                var specFallback = actualRestoreSpec.RestoreMetadata.FallbackFolders;
                var expectedFolders = fallbackFolders != null ? MSBuildStringUtility.Split(fallbackFolders).Select(e => Path.Combine(testDirectory, e)) : SettingsUtility.GetFallbackPackageFolders(settings);
                Assert.True(Enumerable.SequenceEqual(expectedFolders.OrderBy(t => t), specFallback.OrderBy(t => t)));

                // Verify
                projectBuildProperties.VerifyAll();
            }
        }

        [Theory]
        [InlineData(@"C:\RestorePackagesPath", @"C:\Source1;C:\Source2", @"C:\Fallback1;C:\Fallback2")]
        [InlineData(null, @"C:\Source1;C:\Source2", @"C:\Fallback1;C:\Fallback2")]
        [InlineData(@"C:\RestorePackagesPath", null, @"C:\Fallback1;C:\Fallback2")]
        [InlineData(@"C:\RestorePackagesPath", @"C:\Source1;C:\Source2", null)]
        public async Task GetPackageSpecsAsync_ReadSettingsWithFullPaths(string restorePackagesPath, string sources, string fallbackFolders)
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            // Arrange
            using (var testDirectory = TestDirectory.Create())
            {
                var projectBuildProperties = new Mock<IVsProjectBuildProperties>();
                var projectAdapter = CreateProjectAdapter(testDirectory, projectBuildProperties);

#pragma warning disable CS0618 // Type or member is obsolete
                projectBuildProperties
                    .Setup(x => x.GetPropertyValueWithDteFallback(It.Is<string>(x => x.Equals(ProjectBuildProperties.RestorePackagesPath))))
                    .Returns(restorePackagesPath);

                projectBuildProperties
                    .Setup(x => x.GetPropertyValueWithDteFallback(It.Is<string>(x => x.Equals(ProjectBuildProperties.RestoreSources))))
                    .Returns(sources);

                projectBuildProperties
                    .Setup(x => x.GetPropertyValueWithDteFallback(It.Is<string>(x => x.Equals(ProjectBuildProperties.RestoreFallbackFolders))))
                    .Returns(fallbackFolders);
#pragma warning restore CS0618 // Type or member is obsolete

                var projectServices = new TestProjectSystemServices();

                var testProject = new LegacyPackageReferenceProject(
                    projectAdapter,
                    Guid.NewGuid().ToString(),
                    projectServices,
                    _threadingService);

                var settings = NullSettings.Instance;
                var testDependencyGraphCacheContext = new DependencyGraphCacheContext(NullLogger.Instance, settings);

                // Act
                var packageSpecs = await testProject.GetPackageSpecsAsync(testDependencyGraphCacheContext);

                // Assert
                Assert.NotNull(packageSpecs);
                var actualRestoreSpec = packageSpecs.Single();
                SpecValidationUtility.ValidateProjectSpec(actualRestoreSpec);

                // Assert packagespath
                Assert.Equal(restorePackagesPath != null ? restorePackagesPath : SettingsUtility.GetGlobalPackagesFolder(settings), actualRestoreSpec.RestoreMetadata.PackagesPath);

                // assert sources
                var specSources = actualRestoreSpec.RestoreMetadata.Sources.Select(e => e.Source);
                var expectedSources = sources != null ? MSBuildStringUtility.Split(sources) : SettingsUtility.GetEnabledSources(settings).Select(e => e.Source);
                Assert.True(Enumerable.SequenceEqual(expectedSources.OrderBy(t => t), specSources.OrderBy(t => t)));

                // assert fallbackfolders
                var specFallback = actualRestoreSpec.RestoreMetadata.FallbackFolders;
                var expectedFolders = fallbackFolders != null ? MSBuildStringUtility.Split(fallbackFolders) : SettingsUtility.GetFallbackPackageFolders(settings);
                Assert.True(Enumerable.SequenceEqual(expectedFolders.OrderBy(t => t), specFallback.OrderBy(t => t)));

                // Verify
                projectBuildProperties.VerifyAll();
            }
        }

        [Fact]
        public async Task GetPackageSpecsAsync_WithPackageTargetFallback_Succeeds()
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            // Arrange
            using (var testDirectory = TestDirectory.Create())
            {
                var projectBuildProperties = new Mock<IVsProjectBuildProperties>();
                var projectAdapter = CreateProjectAdapter(testDirectory, projectBuildProperties);

#pragma warning disable CS0618 // Type or member is obsolete
                projectBuildProperties
                    .Setup(x => x.GetPropertyValueWithDteFallback(It.Is<string>(x => x.Equals(ProjectBuildProperties.PackageTargetFallback))))
                    .Returns("portable-net45+win8;dnxcore50");
#pragma warning restore CS0618 // Type or member is obsolete

                var testProject = new LegacyPackageReferenceProject(
                    projectAdapter,
                    Guid.NewGuid().ToString(),
                    new TestProjectSystemServices(),
                    _threadingService);

                var testDependencyGraphCacheContext = new DependencyGraphCacheContext();

                // Act
                var packageSpecs = await testProject.GetPackageSpecsAsync(testDependencyGraphCacheContext);

                // Assert
                Assert.NotNull(packageSpecs);

                var actualRestoreSpec = packageSpecs.Single();
                SpecValidationUtility.ValidateProjectSpec(actualRestoreSpec);

                var actualTfi = actualRestoreSpec.TargetFrameworks.First();
                Assert.NotNull(actualTfi);
                Assert.Equal(
                    new NuGetFramework[]
                    {
                        NuGetFramework.Parse("portable-net45+win8"),
                        NuGetFramework.Parse("dnxcore50")
                    },
                    actualTfi.Imports);
                Assert.IsType<FallbackFramework>(actualTfi.FrameworkName);
                Assert.Equal(
                    new NuGetFramework[]
                    {
                        NuGetFramework.Parse("portable-net45+win8"),
                        NuGetFramework.Parse("dnxcore50")
                    },
                    ((FallbackFramework)actualTfi.FrameworkName).Fallback);

                // Verify
                projectBuildProperties.VerifyAll();
            }
        }

        [Fact]
        public async Task GetPackageSpecsAsync_WithPackageReference_Succeeds()
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            // Arrange
            using (var randomTestFolder = TestDirectory.Create())
            {
                var framework = NuGetFramework.Parse("netstandard13");

                var projectAdapter = CreateProjectAdapter(randomTestFolder);

                var projectServices = new TestProjectSystemServices();
                projectServices.SetupInstalledPackages(
                    framework,
                    new LibraryDependency
                    {
                        LibraryRange = new LibraryRange(
                            "packageA",
                            VersionRange.Parse("1.*"),
                            LibraryDependencyTarget.Package)
                    });

                var testProject = new LegacyPackageReferenceProject(
                    projectAdapter,
                    Guid.NewGuid().ToString(),
                    projectServices,
                    _threadingService);

                var testDependencyGraphCacheContext = new DependencyGraphCacheContext();

                // Act
                var packageSpecs = await testProject.GetPackageSpecsAsync(testDependencyGraphCacheContext);

                // Assert
                Assert.NotNull(packageSpecs);

                var actualRestoreSpec = packageSpecs.Single();
                SpecValidationUtility.ValidateProjectSpec(actualRestoreSpec);
                //No top level dependencies
                Assert.Equal(0, actualRestoreSpec.Dependencies.Count);

                var actualDependency = actualRestoreSpec.TargetFrameworks.SingleOrDefault().Dependencies.Single();
                Assert.NotNull(actualDependency);
                Assert.Equal("packageA", actualDependency.LibraryRange.Name);
                Assert.Equal(VersionRange.Parse("1.*"), actualDependency.LibraryRange.VersionRange);

                // Verify
                Mock.Get(projectServices.ReferencesReader)
                    .Verify(
                        x => x.GetPackageReferencesAsync(framework, CancellationToken.None),
                        Times.AtLeastOnce);
            }
        }

        [Fact]
        public async Task GetPackageSpecsAsync_WithProjectReference_Succeeds()
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            // Arrange
            using (var randomTestFolder = TestDirectory.Create())
            {
                var framework = NuGetFramework.Parse("netstandard13");

                var projectAdapter = CreateProjectAdapter(randomTestFolder);

                var projectServices = new TestProjectSystemServices();
                projectServices.SetupProjectDependencies(
                    new ProjectRestoreReference
                    {
                        ProjectUniqueName = "TestProjectA",
                        ProjectPath = Path.Combine(randomTestFolder, "TestProjectA")
                    });

                var testProject = new LegacyPackageReferenceProject(
                    projectAdapter,
                    Guid.NewGuid().ToString(),
                    projectServices,
                    _threadingService);

                var testDependencyGraphCacheContext = new DependencyGraphCacheContext();

                // Act
                var packageSpecs = await testProject.GetPackageSpecsAsync(testDependencyGraphCacheContext);

                // Assert
                Assert.NotNull(packageSpecs);

                var actualRestoreSpec = packageSpecs.Single();
                SpecValidationUtility.ValidateProjectSpec(actualRestoreSpec);

                var actualDependency = actualRestoreSpec.RestoreMetadata.TargetFrameworks.Single().ProjectReferences.Single();
                Assert.NotNull(actualDependency);
                Assert.Equal("TestProjectA", actualDependency.ProjectUniqueName);

                // Verify
                Mock.Get(projectServices.ReferencesReader)
                    .Verify(
                        x => x.GetProjectReferencesAsync(It.IsAny<Common.ILogger>(), CancellationToken.None),
                        Times.AtLeastOnce);
            }
        }

        [Fact]
        public async Task GetInstalledPackagesAsync_WhenValid_ReturnsPackageReferences()
        {
            // Arrange
            using (var randomTestFolder = TestDirectory.Create())
            {
                var framework = NuGetFramework.Parse("netstandard13");

                var projectAdapter = CreateProjectAdapter(randomTestFolder);

                var projectServices = new TestProjectSystemServices();
                projectServices.SetupInstalledPackages(
                    framework,
                    new LibraryDependency
                    {
                        LibraryRange = new LibraryRange(
                            "packageA",
                            VersionRange.Parse("1.*"),
                            LibraryDependencyTarget.Package)
                    });

                var testProject = new LegacyPackageReferenceProject(
                    projectAdapter,
                    Guid.NewGuid().ToString(),
                    projectServices,
                    _threadingService);

                // Act
                var packageReferences = await testProject.GetInstalledPackagesAsync(CancellationToken.None);

                // Assert
                var packageReference = packageReferences.Single();
                Assert.NotNull(packageReference);
                Assert.Equal(
                    "packageA.1.0.0",
                    packageReference.PackageIdentity.ToString());

                // Verify
                Mock.Get(projectServices.ReferencesReader)
                    .Verify(
                        x => x.GetPackageReferencesAsync(framework, CancellationToken.None),
                        Times.AtLeastOnce);
            }
        }

        [Fact]
        public async Task InstallPackageAsync_AddsPackageReference()
        {
            // Arrange
            using (var randomTestFolder = TestDirectory.Create())
            {
                var projectAdapter = CreateProjectAdapter(randomTestFolder);

                var projectServices = new TestProjectSystemServices();

                LibraryDependency actualDependency = null;
                Mock.Get(projectServices.References)
                    .Setup(x => x.AddOrUpdatePackageReferenceAsync(
                        It.IsAny<LibraryDependency>(), CancellationToken.None))
                    .Callback<LibraryDependency, CancellationToken>((d, _) => actualDependency = d)
                    .Returns(Task.CompletedTask);

                var testProject = new LegacyPackageReferenceProject(
                    projectAdapter,
                    Guid.NewGuid().ToString(),
                    projectServices,
                    _threadingService);

                var buildIntegratedInstallationContext = new BuildIntegratedInstallationContext(
                    Enumerable.Empty<NuGetFramework>(),
                    Enumerable.Empty<NuGetFramework>(),
                    new Dictionary<NuGetFramework, string>());

                // Act
                var result = await testProject.InstallPackageAsync(
                    "packageA",
                    VersionRange.Parse("1.*"),
                    null,
                    buildIntegratedInstallationContext,
                    CancellationToken.None);

                // Assert
                Assert.True(result);

                Assert.NotNull(actualDependency);
                Assert.Equal("packageA", actualDependency.LibraryRange.Name);
                Assert.Equal(VersionRange.Parse("1.*"), actualDependency.LibraryRange.VersionRange);

                // Verify
                Mock.Get(projectServices.References)
                    .Verify(
                        x => x.AddOrUpdatePackageReferenceAsync(It.IsAny<LibraryDependency>(), CancellationToken.None),
                        Times.Once);
            }
        }

        [Fact]
        public async Task UninstallPackageAsync_Always_RemovesPackageReference()
        {
            // Arrange
            using (var randomTestFolder = TestDirectory.Create())
            {
                var projectAdapter = CreateProjectAdapter(randomTestFolder);

                var projectServices = new TestProjectSystemServices();

                string actualPackageId = null;
                Mock.Get(projectServices.References)
                    .Setup(x => x.RemovePackageReferenceAsync(It.IsAny<string>()))
                    .Callback<string>(p => actualPackageId = p)
                    .Returns(Task.CompletedTask);

                var testProject = new LegacyPackageReferenceProject(
                    projectAdapter,
                    Guid.NewGuid().ToString(),
                    projectServices,
                    _threadingService);

                // Act
                var result = await testProject.UninstallPackageAsync(
                    new PackageIdentity("packageA", NuGetVersion.Parse("1.0.0")),
                    null,
                    CancellationToken.None);

                // Assert
                Assert.True(result);
                Assert.Equal("packageA", actualPackageId);

                // Verify
                Mock.Get(projectServices.References)
                    .Verify(
                        x => x.RemovePackageReferenceAsync(It.IsAny<string>()),
                        Times.Once);
            }
        }

        [Fact]
        public async Task GetPackageSpecsAsync_SkipContentFilesAlwaysTrue()
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            // Arrange
            using (var randomTestFolder = TestDirectory.Create())
            {
                var framework = NuGetFramework.Parse("netstandard13");

                var projectAdapter = CreateProjectAdapter(randomTestFolder);

                var projectServices = new TestProjectSystemServices();
                projectServices.SetupInstalledPackages(
                    framework,
                    new LibraryDependency
                    {
                        LibraryRange = new LibraryRange(
                            "packageA",
                            VersionRange.Parse("1.*"),
                            LibraryDependencyTarget.Package)
                    });

                var testProject = new LegacyPackageReferenceProject(
                    projectAdapter,
                    Guid.NewGuid().ToString(),
                    projectServices,
                    _threadingService);

                var testDependencyGraphCacheContext = new DependencyGraphCacheContext();

                // Act
                var packageSpecs = await testProject.GetPackageSpecsAsync(testDependencyGraphCacheContext);

                // Assert
                Assert.NotNull(packageSpecs);

                var actualRestoreSpec = packageSpecs.Single();
                SpecValidationUtility.ValidateProjectSpec(actualRestoreSpec);

                Assert.True(actualRestoreSpec.RestoreMetadata.SkipContentFileWrite);
            }
        }

        [Theory]
        [InlineData("true", null, false)]
        [InlineData(null, "packages.A.lock.json", false)]
        [InlineData("true", null, true)]
        [InlineData("false", null, false)]
        public async Task GetPackageSpecsAsync_ReadLockFileSettings(
            string restorePackagesWithLockFile,
            string lockFilePath,
            bool restoreLockedMode)
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            // Arrange
            using (var testDirectory = TestDirectory.Create())
            {
                var projectBuildProperties = new Mock<IVsProjectBuildProperties>();
                var projectAdapter = CreateProjectAdapter(testDirectory, projectBuildProperties);

#pragma warning disable CS0618 // Type or member is obsolete
                projectBuildProperties
                    .Setup(x => x.GetPropertyValueWithDteFallback(It.Is<string>(x => x.Equals(ProjectBuildProperties.RestorePackagesWithLockFile))))
                    .Returns(restorePackagesWithLockFile);

                projectBuildProperties
                    .Setup(x => x.GetPropertyValueWithDteFallback(It.Is<string>(x => x.Equals(ProjectBuildProperties.NuGetLockFilePath))))
                    .Returns(lockFilePath);

                projectBuildProperties
                    .Setup(x => x.GetPropertyValueWithDteFallback(It.Is<string>(x => x.Equals(ProjectBuildProperties.RestoreLockedMode))))
                    .Returns(restoreLockedMode.ToString());
#pragma warning restore CS0618 // Type or member is obsolete

                var projectServices = new TestProjectSystemServices();

                var testProject = new LegacyPackageReferenceProject(
                    projectAdapter,
                    Guid.NewGuid().ToString(),
                    projectServices,
                    _threadingService);

                var settings = NullSettings.Instance;
                var testDependencyGraphCacheContext = new DependencyGraphCacheContext(NullLogger.Instance, settings);

                // Act
                var packageSpecs = await testProject.GetPackageSpecsAsync(testDependencyGraphCacheContext);

                // Assert
                Assert.NotNull(packageSpecs);
                var actualRestoreSpec = packageSpecs.Single();
                SpecValidationUtility.ValidateProjectSpec(actualRestoreSpec);

                // Assert restorePackagesWithLockFile
                Assert.Equal(restorePackagesWithLockFile, actualRestoreSpec.RestoreMetadata.RestoreLockProperties.RestorePackagesWithLockFile);

                // assert lockFilePath
                Assert.Equal(lockFilePath, actualRestoreSpec.RestoreMetadata.RestoreLockProperties.NuGetLockFilePath);

                // assert restoreLockedMode
                Assert.Equal(restoreLockedMode, actualRestoreSpec.RestoreMetadata.RestoreLockProperties.RestoreLockedMode);
            }
        }

        [Fact]
        public async Task GetPackageSpecAsync_CentralPackageVersionsRemovedDuplicates()
        {
            // Arrange
            var packageAv1 = (PackageId: "packageA", Version: "1.2.3");
            var packageB = (PackageId: "packageB", Version: "3.4.5");
            var packageAv5 = (PackageId: "packageA", Version: "5.0.0");

            var projectNames = new ProjectNames(
                        fullName: "projectName",
                        uniqueName: "projectName",
                        shortName: "projectName",
                        customUniqueName: "projectName",
                        projectId: Guid.NewGuid().ToString());

            var vsProjectAdapter = new TestVSProjectAdapter(
                        "projectPath",
                        projectNames,
                        "framework",
                        restorePackagesWithLockFile: null,
                        nuGetLockFilePath: null,
                        restoreLockedMode: false,
                        projectPackageVersions: new List<(string Id, string Version)>() { packageAv1, packageB, packageAv5 });

            var legacyPRProject = new LegacyPackageReferenceProject(
                       vsProjectAdapter,
                       Guid.NewGuid().ToString(),
                       new TestProjectSystemServices(),
                       _threadingService);

            var settings = NullSettings.Instance;
            var context = new DependencyGraphCacheContext(NullLogger.Instance, settings);

            var packageSpecs = await legacyPRProject.GetPackageSpecsAsync(context);

            Assert.Equal(1, packageSpecs.Count);
            Assert.True(packageSpecs.First().RestoreMetadata.CentralPackageVersionsEnabled);
            var centralPackageVersions = packageSpecs.First().TargetFrameworks.First().CentralPackageVersions;

            Assert.Equal(2, centralPackageVersions.Count);
            Assert.Equal(VersionRange.Parse(packageAv1.Version), centralPackageVersions[packageAv1.PackageId].VersionRange);
            Assert.Equal(VersionRange.Parse(packageB.Version), centralPackageVersions[packageB.PackageId].VersionRange);
        }

        [Theory]
        [InlineData(null, true)]
        [InlineData("", true)]
        [InlineData("                     ", true)]
        [InlineData("true", true)]
        [InlineData("invalid", true)]
        [InlineData("false", false)]
        [InlineData("           false    ", false)]
        [InlineData("FaLsE", false)]
        public async Task GetPackageSpecAsync_CentralPackageVersionOverride_DisabedWhenSpecified(string isCentralPackageVersionOverrideEnabled, bool expected)
        {
            // Arrange
            var packageAv1 = (PackageId: "packageA", Version: "1.2.3");
            var packageB = (PackageId: "packageB", Version: "3.4.5");
            var packageAv5 = (PackageId: "packageA", Version: "5.0.0");

            var projectNames = new ProjectNames(
                        fullName: "projectName",
                        uniqueName: "projectName",
                        shortName: "projectName",
                        customUniqueName: "projectName",
                        projectId: Guid.NewGuid().ToString());

            var vsProjectAdapter = new TestVSProjectAdapter(
                        "projectPath",
                        projectNames,
                        "framework",
                        restorePackagesWithLockFile: null,
                        nuGetLockFilePath: null,
                        restoreLockedMode: false,
                        projectPackageVersions: new List<(string Id, string Version)>() { packageAv1, packageB, packageAv5 },
                        isCentralPackageVersionOverrideEnabled: isCentralPackageVersionOverrideEnabled);

            var legacyPRProject = new LegacyPackageReferenceProject(
                       vsProjectAdapter,
                       Guid.NewGuid().ToString(),
                       new TestProjectSystemServices(),
                       _threadingService);

            var settings = NullSettings.Instance;
            var context = new DependencyGraphCacheContext(NullLogger.Instance, settings);

            var packageSpecs = await legacyPRProject.GetPackageSpecsAsync(context);

            Assert.Equal(1, packageSpecs.Count);

            if (expected)
            {
                Assert.False(packageSpecs.First().RestoreMetadata.CentralPackageVersionOverrideDisabled);
            }
            else
            {
                Assert.True(packageSpecs.First().RestoreMetadata.CentralPackageVersionOverrideDisabled);
            }
        }

        [Theory]
        [InlineData(null, false)]
        [InlineData("", false)]
        [InlineData("  ", false)]
        [InlineData("invalid", false)]
        [InlineData("false", false)]
        [InlineData("           false    ", false)]
        [InlineData("FaLsE", false)]
        [InlineData("true", true)]
        [InlineData("  true  ", true)]
        public async Task GetPackageSpecAsync_TransitiveDependencyPinning_CanBeEnabled(string transitiveDependencyPinning, bool expected)
        {
            // Arrange
            var projectNames = new ProjectNames(
                        fullName: "projectName",
                        uniqueName: "projectName",
                        shortName: "projectName",
                        customUniqueName: "projectName",
                        projectId: Guid.NewGuid().ToString());

            var vsProjectAdapter = new TestVSProjectAdapter(
                        "projectPath",
                        projectNames,
                        "framework",
                        restorePackagesWithLockFile: null,
                        nuGetLockFilePath: null,
                        restoreLockedMode: false,
                        projectPackageVersions: new List<(string Id, string Version)>() { },
                        CentralPackageTransitivePinningEnabled: transitiveDependencyPinning);

            var legacyPRProject = new LegacyPackageReferenceProject(
                       vsProjectAdapter,
                       Guid.NewGuid().ToString(),
                       new TestProjectSystemServices(),
                       _threadingService);

            var settings = NullSettings.Instance;
            var context = new DependencyGraphCacheContext(NullLogger.Instance, settings);

            var packageSpecs = await legacyPRProject.GetPackageSpecsAsync(context);

            Assert.Equal(1, packageSpecs.Count);

            if (expected)
            {
                Assert.True(packageSpecs.First().RestoreMetadata.CentralPackageTransitivePinningEnabled);
            }
            else
            {
                Assert.False(packageSpecs.First().RestoreMetadata.CentralPackageTransitivePinningEnabled);
            }
        }

        [Fact]
        public async Task GetInstalledVersion_WithAssetsFile_ReturnsVersionsFromAssetsSpecs()
        {
            using (var testDirectory = TestDirectory.Create())
            {
                // Setup
                LegacyPackageReferenceProject testProject = CreateLegacyPackageReferenceProject(testDirectory, "[1.0.0, )");

                var settings = NullSettings.Instance;
                var context = new DependencyGraphCacheContext(NullLogger.Instance, settings);

                var packageSpecs = await testProject.GetPackageSpecsAsync(context);

                // Package directories
                var sources = new List<PackageSource>();
                var packagesDir = new DirectoryInfo(Path.Combine(testDirectory, "globalPackages"));
                var packageSource = new DirectoryInfo(Path.Combine(testDirectory, "packageSource"));
                packagesDir.Create();
                packageSource.Create();
                sources.Add(new PackageSource(packageSource.FullName));

                var logger = new TestLogger();
                var request = new TestRestoreRequest(packageSpecs[0], sources, packagesDir.FullName, logger)
                {
                    LockFilePath = Path.Combine(testDirectory, "obj", "project.assets.json")
                };

                await SimpleTestPackageUtility.CreateFullPackageAsync(packageSource.FullName, "packageA", "2.15.3");

                // Act
                var command = new RestoreCommand(request);
                var result = await command.ExecuteAsync();
                await result.CommitAsync(logger, CancellationToken.None);
                Assert.True(result.Success);
                var packages = await testProject.GetInstalledPackagesAsync(CancellationToken.None);

                // Asert
                packages.Should().Contain(a => a.PackageIdentity.Equals(new PackageIdentity("packageA", new NuGetVersion("2.15.3"))));
            }
        }

        [Fact]
        public async Task GetInstalledVersion_WithAssetsFile_ReturnsVersionsFromAssetsSpecs_ValidateCache()
        {
            using (var testDirectory = TestDirectory.Create())
            {
                // Setup
                LegacyPackageReferenceProject testProject = CreateLegacyPackageReferenceProject(testDirectory, "[1.0.0, )");

                var settings = NullSettings.Instance;
                var context = new DependencyGraphCacheContext(NullLogger.Instance, settings);

                var packageSpecs = await testProject.GetPackageSpecsAsync(context);

                // Package directories
                var sources = new List<PackageSource>();
                var packagesDir = new DirectoryInfo(Path.Combine(testDirectory, "globalPackages"));
                var packageSource = new DirectoryInfo(Path.Combine(testDirectory, "packageSource"));
                packagesDir.Create();
                packageSource.Create();
                sources.Add(new PackageSource(packageSource.FullName));

                var logger = new TestLogger();
                var request = new TestRestoreRequest(packageSpecs[0], sources, packagesDir.FullName, logger)
                {
                    LockFilePath = Path.Combine(testDirectory, "obj", "project.assets.json")
                };

                await SimpleTestPackageUtility.CreateFullPackageAsync(packageSource.FullName, "packageA", "2.15.3");

                // Act
                var command = new RestoreCommand(request);
                var result = await command.ExecuteAsync();
                await result.CommitAsync(logger, CancellationToken.None);
                Assert.True(result.Success);
                var packages = await testProject.GetInstalledPackagesAsync(CancellationToken.None);

                // Asert
                packages.Should().Contain(a => a.PackageIdentity.Equals(new PackageIdentity("packageA", new NuGetVersion("2.15.3"))));

                var cache_packages = await testProject.GetInstalledPackagesAsync(CancellationToken.None);
                cache_packages.Should().Contain(a => a.PackageIdentity.Equals(new PackageIdentity("packageA", new NuGetVersion("2.15.3"))));
            }
        }

        [Fact]
        public async Task GetInstalledVersion_WithFloating_WithAssetsFile_ReturnsVersionsFromAssetsSpecs()
        {
            using (var testDirectory = TestDirectory.Create())
            {
                // Setup
                LegacyPackageReferenceProject testProject = CreateLegacyPackageReferenceProject(testDirectory, "[2.*, )");

                var settings = NullSettings.Instance;
                var context = new DependencyGraphCacheContext(NullLogger.Instance, settings);

                var packageSpecs = await testProject.GetPackageSpecsAsync(context);

                // Package directories
                var sources = new List<PackageSource>();
                var packagesDir = new DirectoryInfo(Path.Combine(testDirectory, "globalPackages"));
                var packageSource = new DirectoryInfo(Path.Combine(testDirectory, "packageSource"));
                packagesDir.Create();
                packageSource.Create();
                sources.Add(new PackageSource(packageSource.FullName));

                var logger = new TestLogger();
                var request = new TestRestoreRequest(packageSpecs[0], sources, packagesDir.FullName, logger)
                {
                    LockFilePath = Path.Combine(testDirectory, "obj", "project.assets.json")
                };

                await SimpleTestPackageUtility.CreateFullPackageAsync(packageSource.FullName, "packageA", "4.0.0");

                // Act
                var command = new RestoreCommand(request);
                var result = await command.ExecuteAsync();
                await result.CommitAsync(logger, CancellationToken.None);
                Assert.True(result.Success);
                var packages = await testProject.GetInstalledPackagesAsync(CancellationToken.None);

                // Asert
                packages.Should().Contain(a => a.PackageIdentity.Equals(new PackageIdentity("packageA", new NuGetVersion("4.0.0"))));

                var cache_packages = await testProject.GetInstalledPackagesAsync(CancellationToken.None);
                cache_packages.Should().Contain(a => a.PackageIdentity.Equals(new PackageIdentity("packageA", new NuGetVersion("4.0.0"))));
            }
        }

        [Fact]
        public async Task GetInstalledVersion_WithoutAssetsFile_ReturnsVersionsFromPackageSpecs()
        {
            using (var testDirectory = TestDirectory.Create())
            {
                // Setup
                LegacyPackageReferenceProject testProject = CreateLegacyPackageReferenceProject(testDirectory, "[2.0.0, )");

                var settings = NullSettings.Instance;
                var context = new DependencyGraphCacheContext(NullLogger.Instance, settings);

                var packageSpecs = await testProject.GetPackageSpecsAsync(context);

                // Package directories
                var sources = new List<PackageSource>();
                var packagesDir = new DirectoryInfo(Path.Combine(testDirectory, "globalPackages"));
                var packageSource = new DirectoryInfo(Path.Combine(testDirectory, "packageSource"));
                packagesDir.Create();
                packageSource.Create();
                sources.Add(new PackageSource(packageSource.FullName));

                var logger = new TestLogger();
                var request = new TestRestoreRequest(packageSpecs[0], sources, packagesDir.FullName, logger);

                // Act
                var command = new RestoreCommand(request);
                var packages = await testProject.GetInstalledPackagesAsync(CancellationToken.None);

                // Asert
                packages.Should().Contain(a => a.PackageIdentity.Equals(new PackageIdentity("packageA", new NuGetVersion("2.0.0"))));
            }
        }

        [Fact]
        public async Task GetInstalledVersion_WithoutPackages_ReturnsEmpty()
        {
            using (var testDirectory = TestDirectory.Create())
            {
                // Setup
                LegacyPackageReferenceProject testProject = CreateLegacyPackageReferenceProjectNoPackages(testDirectory);

                var settings = NullSettings.Instance;
                var context = new DependencyGraphCacheContext(NullLogger.Instance, settings);

                var packageSpecs = await testProject.GetPackageSpecsAsync(context);

                // Package directories
                var sources = new List<PackageSource>();
                var packagesDir = new DirectoryInfo(Path.Combine(testDirectory, "globalPackages"));
                var packageSource = new DirectoryInfo(Path.Combine(testDirectory, "packageSource"));
                packagesDir.Create();
                packageSource.Create();
                sources.Add(new PackageSource(packageSource.FullName));

                var logger = new TestLogger();
                var request = new TestRestoreRequest(packageSpecs[0], sources, packagesDir.FullName, logger);

                // Act
                var command = new RestoreCommand(request);
                var result = await command.ExecuteAsync();
                await result.CommitAsync(logger, CancellationToken.None);
                var packages = await testProject.GetInstalledPackagesAsync(CancellationToken.None);

                // Asert
                packages.Should().BeEmpty();
            }
        }

        [Fact]
        public async Task GetInstalledVersion_WithAssetsFile_ReturnsVsersionsFromAssets()
        {
            using (var testDirectory = TestDirectory.Create())
            {
                // Setup
                LegacyPackageReferenceProject testProject = CreateLegacyPackageReferenceProject(testDirectory, "[2.0.0, )");

                var settings = NullSettings.Instance;
                var context = new DependencyGraphCacheContext(NullLogger.Instance, settings);

                var packageSpecs = await testProject.GetPackageSpecsAsync(context);

                // Package directories
                var sources = new List<PackageSource>();
                var packagesDir = new DirectoryInfo(Path.Combine(testDirectory, "globalPackages"));
                var packageSource = new DirectoryInfo(Path.Combine(testDirectory, "packageSource"));
                packagesDir.Create();
                packageSource.Create();
                sources.Add(new PackageSource(packageSource.FullName));

                var logger = new TestLogger();
                var request = new TestRestoreRequest(packageSpecs[0], sources, packagesDir.FullName, logger)
                {
                    LockFilePath = Path.Combine(testDirectory, "obj", "project.assets.json")
                };

                await SimpleTestPackageUtility.CreateFullPackageAsync(packageSource.FullName, "packageA", "4.0.0");

                // Act
                var command = new RestoreCommand(request);
                var result = await command.ExecuteAsync();
                await result.CommitAsync(logger, CancellationToken.None);
                var packages = await testProject.GetInstalledPackagesAsync(CancellationToken.None);

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
                LegacyPackageReferenceProject testProject = CreateLegacyPackageReferenceProjectNoPackages(testDirectory);

                var settings = NullSettings.Instance;
                var context = new DependencyGraphCacheContext(NullLogger.Instance, settings);

                var packageSpecs = await testProject.GetPackageSpecsAsync(context);

                // Package directories
                var sources = new List<PackageSource>();
                var packagesDir = new DirectoryInfo(Path.Combine(testDirectory, "globalPackages"));
                var packageSource = new DirectoryInfo(Path.Combine(testDirectory, "packageSource"));
                packagesDir.Create();
                packageSource.Create();
                sources.Add(new PackageSource(packageSource.FullName));

                var logger = new TestLogger();
                var request = new TestRestoreRequest(packageSpecs[0], sources, packagesDir.FullName, logger)
                {
                    LockFilePath = Path.Combine(testDirectory, "obj", "project.assets.json")
                };

                await SimpleTestPackageUtility.CreateFullPackageAsync(packageSource.FullName, "packageA", "1.0.0");

                // Act
                var command = new RestoreCommand(request);
                var result = await command.ExecuteAsync();
                await result.CommitAsync(logger, CancellationToken.None);
                var packages = await testProject.GetInstalledPackagesAsync(CancellationToken.None);

                // Asert
                packages.Should().BeEmpty();
            }
        }

        [Fact]
        public async Task GetTransitivePackagesAsync_WithTransitivePackageReferences_ReturnsPackageIdentities()
        {
            using (TestDirectory testDirectory = TestDirectory.Create())
            {
                // Setup
                LegacyPackageReferenceProject testProject = CreateLegacyPackageReferenceProject(testDirectory, "[1.0.0, )");

                NullSettings settings = NullSettings.Instance;
                var context = new DependencyGraphCacheContext(NullLogger.Instance, settings);

                IReadOnlyList<PackageSpec> packageSpecs = await testProject.GetPackageSpecsAsync(context);

                // Package directories
                var sources = new List<PackageSource>();
                var packagesDir = new DirectoryInfo(Path.Combine(testDirectory, "globalPackages"));
                var packageSource = new DirectoryInfo(Path.Combine(testDirectory, "packageSource"));
                packagesDir.Create();
                packageSource.Create();
                sources.Add(new PackageSource(packageSource.FullName));

                var logger = new TestLogger();
                var request = new TestRestoreRequest(packageSpecs[0], sources, packagesDir.FullName, logger)
                {
                    LockFilePath = Path.Combine(testDirectory, "obj", "project.assets.json")
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
                Assert.True(result.Success);
                ProjectPackages packages = await testProject.GetInstalledAndTransitivePackagesAsync(includeTransitiveOrigins: false, CancellationToken.None);

                // Assert
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
                LegacyPackageReferenceProject testProject = CreateLegacyPackageReferenceProject(testDirectory, "[1.0.0, )");

                NullSettings settings = NullSettings.Instance;
                var context = new DependencyGraphCacheContext(NullLogger.Instance, settings);

                var packageSpecs = await testProject.GetPackageSpecsAsync(context);

                // Package directories
                var sources = new List<PackageSource>();
                var packagesDir = new DirectoryInfo(Path.Combine(testDirectory, "globalPackages"));
                var packageSource = new DirectoryInfo(Path.Combine(testDirectory, "packageSource"));
                packagesDir.Create();
                packageSource.Create();
                sources.Add(new PackageSource(packageSource.FullName));

                var logger = new TestLogger();
                var request = new TestRestoreRequest(packageSpecs[0], sources, packagesDir.FullName, logger)
                {
                    LockFilePath = Path.Combine(testDirectory, "obj", "project.assets.json")
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
                Assert.True(result.Success);
                ProjectPackages packages = await testProject.GetInstalledAndTransitivePackagesAsync(includeTransitiveOrigins: false, CancellationToken.None);

                // Assert
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
                LegacyPackageReferenceProject testProject = CreateLegacyPackageReferenceProject(testDirectory, "[1.0.0, )");

                NullSettings settings = NullSettings.Instance;
                var context = new DependencyGraphCacheContext(NullLogger.Instance, settings);

                IReadOnlyList<PackageSpec> packageSpecs = await testProject.GetPackageSpecsAsync(context);

                // Package directories
                var sources = new List<PackageSource>();
                var packagesDir = new DirectoryInfo(Path.Combine(testDirectory, "globalPackages"));
                var packageSource = new DirectoryInfo(Path.Combine(testDirectory, "packageSource"));
                packagesDir.Create();
                packageSource.Create();
                sources.Add(new PackageSource(packageSource.FullName));

                var logger = new TestLogger();
                var request = new TestRestoreRequest(packageSpecs[0], sources, packagesDir.FullName, logger)
                {
                    LockFilePath = Path.Combine(testDirectory, "obj", "project.assets.json")
                };

                await SimpleTestPackageUtility.CreateFullPackageAsync(packageSource.FullName, "packageA", "2.15.3");

                // Act
                var command = new RestoreCommand(request);
                RestoreResult result = await command.ExecuteAsync();
                await result.CommitAsync(logger, CancellationToken.None);
                Assert.True(result.Success);
                ProjectPackages packages = await testProject.GetInstalledAndTransitivePackagesAsync(includeTransitiveOrigins: false, CancellationToken.None);

                // Assert
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
                LegacyPackageReferenceProject testProject = CreateLegacyPackageReferenceProject(testDirectory, "[1.0.0, )");

                NullSettings settings = NullSettings.Instance;
                var context = new DependencyGraphCacheContext(NullLogger.Instance, settings);

                IReadOnlyList<PackageSpec> packageSpecs = await testProject.GetPackageSpecsAsync(context);

                // Package directories
                var sources = new List<PackageSource>();
                var packagesDir = new DirectoryInfo(Path.Combine(testDirectory, "globalPackages"));
                var packageSource = new DirectoryInfo(Path.Combine(testDirectory, "packageSource"));
                packagesDir.Create();
                packageSource.Create();
                sources.Add(new PackageSource(packageSource.FullName));
                string lockFilePath = Path.Combine(testDirectory, "obj", "project.assets.json");

                var logger = new TestLogger();
                var request = new TestRestoreRequest(packageSpecs[0], sources, packagesDir.FullName, logger)
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
                Assert.True(result.Success);
                ProjectPackages packages = await testProject.GetInstalledAndTransitivePackagesAsync(includeTransitiveOrigins: false, CancellationToken.None);
                DateTime lastWriteTime = File.GetLastWriteTimeUtc(lockFilePath);
                File.SetLastWriteTimeUtc(lockFilePath, lastWriteTime);
                ProjectPackages cache_packages = await testProject.GetInstalledAndTransitivePackagesAsync(includeTransitiveOrigins: false, CancellationToken.None);

                // Assert
                cache_packages.InstalledPackages.Should().Contain(a => a.PackageIdentity.Equals(new PackageIdentity("packageA", new NuGetVersion("2.15.3"))));
                cache_packages.TransitivePackages.Should().Contain(a => a.PackageIdentity.Equals(new PackageIdentity("packageB", new NuGetVersion("1.0.0"))));
                Assert.True(lastWriteTime == File.GetLastWriteTimeUtc(lockFilePath));
            }
        }

        [Theory]
        [InlineData(null, null, null, 0, 0)]
        [InlineData("win-x64", null, null, 1, 0)]
        [InlineData("win-x64", "win-x86", null, 2, 0)]
        [InlineData("win-x64", "win-x86;win-x64", null, 2, 0)]
        [InlineData("win-x64", "win-x86;win-x64", "win", 2, 1)]
        public async Task GetPackageSpecsAsync_WithRuntimeIdentifiers_GeneratesRuntimeGraph(string runtimeIdentifier, string runtimeIdentifiers, string runtimeSupports, int runtimeCount, int supportsCount)
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            // Arrange
            using (var testDirectory = TestDirectory.Create())
            {
                var projectBuildProperties = new Mock<IVsProjectBuildProperties>();
                var projectAdapter = CreateProjectAdapter(testDirectory, projectBuildProperties);

#pragma warning disable CS0618 // Type or member is obsolete
                projectBuildProperties
                    .Setup(x => x.GetPropertyValueWithDteFallback(It.Is<string>(x => x.Equals(ProjectBuildProperties.RuntimeIdentifier))))
                    .Returns(runtimeIdentifier);

                projectBuildProperties
                    .Setup(x => x.GetPropertyValueWithDteFallback(It.Is<string>(x => x.Equals(ProjectBuildProperties.RuntimeIdentifiers))))
                    .Returns(runtimeIdentifiers);

                projectBuildProperties
                    .Setup(x => x.GetPropertyValueWithDteFallback(It.Is<string>(x => x.Equals(ProjectBuildProperties.RuntimeSupports))))
                    .Returns(runtimeSupports);
#pragma warning restore CS0618 // Type or member is obsolete

                var projectServices = new TestProjectSystemServices();

                var testProject = new LegacyPackageReferenceProject(
                    projectAdapter,
                    Guid.NewGuid().ToString(),
                    projectServices,
                    _threadingService);

                var settings = NullSettings.Instance;
                var testDependencyGraphCacheContext = new DependencyGraphCacheContext(NullLogger.Instance, settings);

                // Act
                var packageSpecs = await testProject.GetPackageSpecsAsync(testDependencyGraphCacheContext);

                // Assert
                Assert.NotNull(packageSpecs);
                var actualRestoreSpec = packageSpecs.Single();
                SpecValidationUtility.ValidateProjectSpec(actualRestoreSpec);

                // Assert runtime graph
                actualRestoreSpec.RuntimeGraph.Runtimes.Count.Should().Be(runtimeCount);
                actualRestoreSpec.RuntimeGraph.Supports.Count.Should().Be(supportsCount);

                // Verify
                projectBuildProperties.VerifyAll();
            }
        }

        [Theory]
        [InlineData(null, null, "")]
        [InlineData("win-x64", null, "win-x64")]
        [InlineData("win-x64", "win-x64", "win-x64")]
        [InlineData(null, "win-x64", "win-x64")]
        [InlineData("win-x86", "win-x64", "win-x86;win-x64")]
        public void GetRuntimeIdentifiers_WithVariousInputs(string runtimeIdentifier, string runtimeIdentifiers, string expected)
        {
            var actual = LegacyPackageReferenceProject.GetRuntimeIdentifiers(runtimeIdentifier, runtimeIdentifiers);
            Assert.Equal(expected, string.Join(";", actual.Select(e => e.RuntimeIdentifier)));
        }

        [Theory]
        [InlineData(null, "")]
        [InlineData("net46.app;win8.app", "net46.app;win8.app")]
        [InlineData("net46.app;win10.app;net46.app;win10.app", "net46.app;win10.app;net46.app;win10.app")]
        public void GetRuntimeSupports_WithVariousInputs(string runtimeSupports, string expected)
        {
            var actual = LegacyPackageReferenceProject.GetRuntimeSupports(runtimeSupports);
            Assert.Equal(expected, string.Join(";", actual.Select(e => e.Name.ToString())));
        }

        [Fact]
        public async Task GetPackageSpecs_WithWarningProperties()
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            // Arrange
            using var testDirectory = TestDirectory.Create();

            var projectBuildProperties = new Mock<IVsProjectBuildProperties>();
            var projectAdapter = CreateProjectAdapter(testDirectory, projectBuildProperties);

#pragma warning disable CS0618 // Type or member is obsolete
            projectBuildProperties
                .Setup(x => x.GetPropertyValueWithDteFallback(It.Is<string>(x => x.Equals(ProjectBuildProperties.NoWarn))))
                .Returns("NU1504");
            projectBuildProperties
               .Setup(x => x.GetPropertyValueWithDteFallback(It.Is<string>(x => x.Equals(ProjectBuildProperties.TreatWarningsAsErrors))))
               .Returns("true");
            projectBuildProperties
                .Setup(x => x.GetPropertyValueWithDteFallback(It.Is<string>(x => x.Equals(ProjectBuildProperties.WarningsNotAsErrors))))
                .Returns("NU1801");
            projectBuildProperties
                .Setup(x => x.GetPropertyValueWithDteFallback(It.Is<string>(x => x.Equals(ProjectBuildProperties.WarningsAsErrors))))
                .Returns("NU1803");
#pragma warning restore CS0618 // Type or member is obsolete

            var projectServices = new TestProjectSystemServices();

            var testProject = new LegacyPackageReferenceProject(
                projectAdapter,
                Guid.NewGuid().ToString(),
                projectServices,
                _threadingService);

            var settings = NullSettings.Instance;
            var testDependencyGraphCacheContext = new DependencyGraphCacheContext(NullLogger.Instance, settings);

            // Act
            var packageSpecs = await testProject.GetPackageSpecsAsync(testDependencyGraphCacheContext);

            // Assert
            Assert.NotNull(packageSpecs);
            var actualRestoreSpec = packageSpecs.Single();
            SpecValidationUtility.ValidateProjectSpec(actualRestoreSpec);

            var warningProperties = actualRestoreSpec.RestoreMetadata.ProjectWideWarningProperties;
            warningProperties.AllWarningsAsErrors.Should().BeTrue();
            warningProperties.NoWarn.Should().Contain(NuGetLogCode.NU1504);
            warningProperties.NoWarn.Should().HaveCount(1);
            warningProperties.WarningsNotAsErrors.Should().Contain(NuGetLogCode.NU1801);
            warningProperties.WarningsNotAsErrors.Should().HaveCount(1);
            warningProperties.WarningsAsErrors.Should().Contain(NuGetLogCode.NU1803);
            warningProperties.WarningsAsErrors.Should().HaveCount(1);
            // Verify
            projectBuildProperties.VerifyAll();
        }

        [Fact]
        public async Task GetPackageSpec_WithNuGetAuditSuppress()
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            // Arrange
            using var testDirectory = TestDirectory.Create();

            var projectBuildProperties = new Mock<IVsProjectBuildProperties>();
            var projectAdapter = CreateProjectAdapter(testDirectory, projectBuildProperties);

            Mock<IVsProjectAdapter> projectAdapterMock = Mock.Get(projectAdapter);
            projectAdapterMock.Setup(m => m.GetBuildItemInformation(ProjectItems.NuGetAuditSuppress, It.IsAny<string[]>()))
                .Returns([("https://cve.test/1", Array.Empty<string>())]);

            var projectServices = new TestProjectSystemServices();
            var testProject = new LegacyPackageReferenceProject(
                projectAdapter,
                Guid.NewGuid().ToString(),
                projectServices,
                _threadingService);

            var settings = NullSettings.Instance;
            var testDependencyGraphCacheContext = new DependencyGraphCacheContext(NullLogger.Instance, settings);

            // Act
            var packageSpecs = await testProject.GetPackageSpecsAsync(testDependencyGraphCacheContext);

            // Assert
            Assert.NotNull(packageSpecs);
            var actualRestoreSpec = packageSpecs.Single();
            SpecValidationUtility.ValidateProjectSpec(actualRestoreSpec);

            var auditProperties = actualRestoreSpec.RestoreMetadata.RestoreAuditProperties;
            auditProperties.SuppressedAdvisories.Should().NotBeNull();
            auditProperties.SuppressedAdvisories.Should().BeEquivalentTo(["https://cve.test/1"]);
        }

        [Fact]
        public async Task GetPackageSpec_WithValidSdkAnalysisLevel_ReadsSdkAnalysisLevelValue()
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            // Arrange
            string sdkAnalysisLevel = "9.0.809";
            using var testDirectory = TestDirectory.Create();
            NuGetVersion expectedVersion = new NuGetVersion(sdkAnalysisLevel);
            var projectBuildProperties = new Mock<IVsProjectBuildProperties>();
            projectBuildProperties.Setup(b => b.GetPropertyValue(ProjectBuildProperties.SdkAnalysisLevel))
                .Returns(sdkAnalysisLevel);
            var projectAdapter = CreateProjectAdapter(testDirectory, projectBuildProperties);

            Mock<IVsProjectAdapter> projectAdapterMock = Mock.Get(projectAdapter);

            var projectServices = new TestProjectSystemServices();
            var testProject = new LegacyPackageReferenceProject(
                projectAdapter,
                Guid.NewGuid().ToString(),
                projectServices,
                _threadingService);

            var settings = NullSettings.Instance;
            var testDependencyGraphCacheContext = new DependencyGraphCacheContext(NullLogger.Instance, settings);

            // Act
            var packageSpecs = await testProject.GetPackageSpecsAsync(testDependencyGraphCacheContext);

            // Assert
            Assert.NotNull(packageSpecs);
            var actualRestoreSpec = packageSpecs.Single();
            SpecValidationUtility.ValidateProjectSpec(actualRestoreSpec);
            actualRestoreSpec.RestoreMetadata.SdkAnalysisLevel.Should().Be(expectedVersion);
        }

        [Theory]
        [InlineData("False")]
        [InlineData("FaLse")]
        [InlineData("false")]
        public async Task GetPackageSpec_WithFalseUsingMicrosoftNetSdk_ReadsFalse(string usingSdk)
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            // Arrange
            using var testDirectory = TestDirectory.Create();
            var projectBuildProperties = new Mock<IVsProjectBuildProperties>();
            projectBuildProperties.Setup(b => b.GetPropertyValue(ProjectBuildProperties.UsingMicrosoftNETSdk))
                .Returns(usingSdk);
            var projectAdapter = CreateProjectAdapter(testDirectory, projectBuildProperties);

            Mock<IVsProjectAdapter> projectAdapterMock = Mock.Get(projectAdapter);

            var projectServices = new TestProjectSystemServices();
            var testProject = new LegacyPackageReferenceProject(
                projectAdapter,
                Guid.NewGuid().ToString(),
                projectServices,
                _threadingService);

            var settings = NullSettings.Instance;
            var testDependencyGraphCacheContext = new DependencyGraphCacheContext(NullLogger.Instance, settings);

            // Act
            var packageSpecs = await testProject.GetPackageSpecsAsync(testDependencyGraphCacheContext);

            // Assert
            Assert.NotNull(packageSpecs);
            var actualRestoreSpec = packageSpecs.Single();
            SpecValidationUtility.ValidateProjectSpec(actualRestoreSpec);

            Assert.False(actualRestoreSpec.RestoreMetadata.UsingMicrosoftNETSdk);
        }

        [Theory]
        [InlineData("True")]
        [InlineData("true")]
        [InlineData("TrUe")]
        public async Task GetPackageSpec_WithTrueUsingMicrosoftNetSdk_ReadsTrue(string usingSdk)
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            // Arrange
            using var testDirectory = TestDirectory.Create();
            var projectBuildProperties = new Mock<IVsProjectBuildProperties>();
            projectBuildProperties.Setup(b => b.GetPropertyValue(ProjectBuildProperties.UsingMicrosoftNETSdk))
                .Returns(usingSdk);
            var projectAdapter = CreateProjectAdapter(testDirectory, projectBuildProperties);

            Mock<IVsProjectAdapter> projectAdapterMock = Mock.Get(projectAdapter);

            var projectServices = new TestProjectSystemServices();
            var testProject = new LegacyPackageReferenceProject(
                projectAdapter,
                Guid.NewGuid().ToString(),
                projectServices,
                _threadingService);

            var settings = NullSettings.Instance;
            var testDependencyGraphCacheContext = new DependencyGraphCacheContext(NullLogger.Instance, settings);

            // Act
            var packageSpecs = await testProject.GetPackageSpecsAsync(testDependencyGraphCacheContext);

            // Assert
            Assert.NotNull(packageSpecs);
            var actualRestoreSpec = packageSpecs.Single();
            SpecValidationUtility.ValidateProjectSpec(actualRestoreSpec);

            Assert.True(actualRestoreSpec.RestoreMetadata.UsingMicrosoftNETSdk);
        }

        [Fact]
        public async Task GetPackageSpec_WithInvalidSdkAnalysisLevel_ThrowsAnException()
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            // Arrange
            using var testDirectory = TestDirectory.Create();
            var projectBuildProperties = new Mock<IVsProjectBuildProperties>();
            projectBuildProperties.Setup(b => b.GetPropertyValue(ProjectBuildProperties.SdkAnalysisLevel))
                .Returns("9.ainvlaid");
            var projectAdapter = CreateProjectAdapter(testDirectory, projectBuildProperties);

            Mock<IVsProjectAdapter> projectAdapterMock = Mock.Get(projectAdapter);

            var projectServices = new TestProjectSystemServices();
            var testProject = new LegacyPackageReferenceProject(
                projectAdapter,
                Guid.NewGuid().ToString(),
                projectServices,
                _threadingService);

            var settings = NullSettings.Instance;
            var testDependencyGraphCacheContext = new DependencyGraphCacheContext(NullLogger.Instance, settings);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await testProject.GetPackageSpecsAsync(testDependencyGraphCacheContext));
        }

        [Fact]
        public async Task GetPackageSpec_WithInvalidUsingMicrosoftNetSdk_ThrowsAnException()
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            // Arrange
            using var testDirectory = TestDirectory.Create();
            var projectBuildProperties = new Mock<IVsProjectBuildProperties>();
            projectBuildProperties.Setup(b => b.GetPropertyValue(ProjectBuildProperties.UsingMicrosoftNETSdk))
                .Returns("falsetrue");
            var projectAdapter = CreateProjectAdapter(testDirectory, projectBuildProperties);

            Mock<IVsProjectAdapter> projectAdapterMock = Mock.Get(projectAdapter);

            var projectServices = new TestProjectSystemServices();
            var testProject = new LegacyPackageReferenceProject(
                projectAdapter,
                Guid.NewGuid().ToString(),
                projectServices,
                _threadingService);

            var settings = NullSettings.Instance;
            var testDependencyGraphCacheContext = new DependencyGraphCacheContext(NullLogger.Instance, settings);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await testProject.GetPackageSpecsAsync(testDependencyGraphCacheContext));
        }

        [Theory]
        [InlineData("False", false)]
        [InlineData("true", true)]
        [InlineData(null, false)]
        public async Task GetPackageSpec_WithUseLegacyDependencyResolver(string restoreUseLegacyDependencyResolver, bool expected)
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            // Arrange
            using var testDirectory = TestDirectory.Create();
            var projectBuildProperties = new Mock<IVsProjectBuildProperties>();
            projectBuildProperties.Setup(b => b.GetPropertyValue(ProjectBuildProperties.RestoreUseLegacyDependencyResolver))
                .Returns(restoreUseLegacyDependencyResolver);
            var projectAdapter = CreateProjectAdapter(testDirectory, projectBuildProperties);

            var testProject = new LegacyPackageReferenceProject(
                projectAdapter,
                Guid.NewGuid().ToString(),
                new TestProjectSystemServices(),
                _threadingService);

            // Act
            var packageSpecs = await testProject.GetPackageSpecsAsync(new DependencyGraphCacheContext(NullLogger.Instance, NullSettings.Instance));

            // Assert
            Assert.NotNull(packageSpecs);
            var actualRestoreSpec = packageSpecs.Single();
            SpecValidationUtility.ValidateProjectSpec(actualRestoreSpec);

            actualRestoreSpec.RestoreMetadata.UseLegacyDependencyResolver.Should().Be(expected);
        }

        [Fact]
        public async Task GetPackageSpecAsync_WithVariousCentralPackageVersions_AppliesFlagsCorreclty()
        {
            // Arrange
            var tfm = NuGetFramework.Parse("net472");
            var packageA = (PackageId: "packageA", Version: "1.2.3");
            var packageB = (PackageId: "packageB", Version: "3.4.5");
            var packageC = (PackageId: "packageC", Version: "6.7.8");
            var packageD = (PackageId: "packageD", Version: "9.9.9");

            var projectNames = new ProjectNames(
                        fullName: "projectName",
                        uniqueName: "projectName",
                        shortName: "projectName",
                        customUniqueName: "projectName",
                        projectId: Guid.NewGuid().ToString());

            var projectServices = new TestProjectSystemServices();
            projectServices.SetupInstalledPackages(
                    tfm,
                    new LibraryDependency
                    {
                        LibraryRange = new LibraryRange(
                            packageA.PackageId,
                            null,
                            LibraryDependencyTarget.Package),
                    },
                    new LibraryDependency
                    {
                        LibraryRange = new LibraryRange(
                            packageB.PackageId,
                            null,
                            LibraryDependencyTarget.Package),
                    },
                    new LibraryDependency
                    {
                        LibraryRange = new LibraryRange(
                            packageC.PackageId,
                            VersionRange.Parse("2.0.0"),
                            LibraryDependencyTarget.Package),
                        AutoReferenced = true,
                    },
                    new LibraryDependency
                    {
                        LibraryRange = new LibraryRange(
                            packageD.PackageId,
                            null,
                            LibraryDependencyTarget.Package),
                        VersionOverride = VersionRange.Parse("3.0.0"),
                    });

            var vsProjectAdapter = new TestVSProjectAdapter(
                        "projectPath",
                        projectNames,
                        "net472",
                        restorePackagesWithLockFile: null,
                        nuGetLockFilePath: null,
                        restoreLockedMode: false,
                        projectPackageVersions: new List<(string Id, string Version)>() { packageA, packageB, packageC, packageD },
                        isCentralPackageVersionOverrideEnabled: "true");

            var legacyPRProject = new LegacyPackageReferenceProject(
                       vsProjectAdapter,
                       Guid.NewGuid().ToString(),
                       projectServices,
                       _threadingService);

            var settings = NullSettings.Instance;
            var context = new DependencyGraphCacheContext(NullLogger.Instance, settings);

            var packageSpecs = await legacyPRProject.GetPackageSpecsAsync(context);

            packageSpecs.Should().HaveCount(1);
            PackageSpec spec = packageSpecs[0];
            spec.TargetFrameworks.Should().HaveCount(1);
            TargetFrameworkInformation tfi = spec.TargetFrameworks[0];
            tfi.FrameworkName.Should().Be(tfm);
            tfi.CentralPackageVersions.Should().HaveCount(4);
            tfi.Dependencies.Should().HaveCount(4);

            tfi.Dependencies[0].Name.Should().Be(packageA.PackageId);
            tfi.Dependencies[0].VersionCentrallyManaged.Should().BeTrue();
            tfi.Dependencies[0].LibraryRange.VersionRange.Should().Be(VersionRange.Parse(packageA.Version));
            tfi.Dependencies[0].VersionOverride.Should().BeNull();

            tfi.Dependencies[1].Name.Should().Be(packageB.PackageId);
            tfi.Dependencies[1].VersionCentrallyManaged.Should().BeTrue();
            tfi.Dependencies[1].LibraryRange.VersionRange.Should().Be(VersionRange.Parse(packageB.Version));
            tfi.Dependencies[1].VersionOverride.Should().BeNull();

            tfi.Dependencies[2].Name.Should().Be(packageC.PackageId);
            tfi.Dependencies[2].VersionCentrallyManaged.Should().BeFalse();
            tfi.Dependencies[2].LibraryRange.VersionRange.Should().Be(VersionRange.Parse("2.0.0"));
            tfi.Dependencies[2].AutoReferenced.Should().BeTrue();
            tfi.Dependencies[2].VersionOverride.Should().BeNull();

            tfi.Dependencies[2].LibraryRange.VersionRange.Should().NotBe(tfi.CentralPackageVersions[packageC.PackageId].VersionRange);

            tfi.Dependencies[3].Name.Should().Be(packageD.PackageId);
            tfi.Dependencies[3].VersionCentrallyManaged.Should().BeFalse();
            tfi.Dependencies[3].LibraryRange.VersionRange.Should().Be(VersionRange.Parse("3.0.0"));
            tfi.Dependencies[3].AutoReferenced.Should().BeFalse();
            tfi.Dependencies[3].VersionOverride.Should().Be(VersionRange.Parse("3.0.0"));
        }

        [Fact]
        public async Task GetPackageSpecAsync_WithDifferentCasingPackageVersionAndPackageReference_CombinesCorrectly()
        {
            // Arrange
            var tfm = NuGetFramework.Parse("net472");
            var packageA = (PackageId: "packageA", Version: "1.2.3");
            var packageAUpperCase = (PackageId: "PACKAGEA", Version: "1.2.3");

            var projectNames = new ProjectNames(
                        fullName: "projectName",
                        uniqueName: "projectName",
                        shortName: "projectName",
                        customUniqueName: "projectName",
                        projectId: Guid.NewGuid().ToString());

            var projectServices = new TestProjectSystemServices();
            projectServices.SetupInstalledPackages(
                    tfm,
                    new LibraryDependency
                    {
                        LibraryRange = new LibraryRange(
                            packageA.PackageId,
                            null,
                            LibraryDependencyTarget.Package),
                    });

            var vsProjectAdapter = new TestVSProjectAdapter(
                        "projectPath",
                        projectNames,
                        "net472",
                        restorePackagesWithLockFile: null,
                        nuGetLockFilePath: null,
                        restoreLockedMode: false,
                        projectPackageVersions: new List<(string Id, string Version)>() { packageAUpperCase },
                        isCentralPackageVersionOverrideEnabled: "true");

            var legacyPRProject = new LegacyPackageReferenceProject(
                       vsProjectAdapter,
                       Guid.NewGuid().ToString(),
                       projectServices,
                       _threadingService);

            var settings = NullSettings.Instance;
            var context = new DependencyGraphCacheContext(NullLogger.Instance, settings);

            var packageSpecs = await legacyPRProject.GetPackageSpecsAsync(context);

            packageSpecs.Should().HaveCount(1);
            PackageSpec spec = packageSpecs[0];
            spec.TargetFrameworks.Should().HaveCount(1);
            TargetFrameworkInformation tfi = spec.TargetFrameworks[0];
            tfi.FrameworkName.Should().Be(tfm);
            tfi.CentralPackageVersions.Should().HaveCount(1);
            tfi.Dependencies.Should().HaveCount(1);

            tfi.Dependencies[0].Name.Should().Be(packageA.PackageId);
            tfi.Dependencies[0].VersionCentrallyManaged.Should().BeTrue();
            tfi.Dependencies[0].LibraryRange.VersionRange.Should().Be(VersionRange.Parse(packageA.Version));
            tfi.Dependencies[0].VersionOverride.Should().BeNull();
        }

        public static readonly List<object[]> PrunePackageReferenceData
            = new List<object[]>
            {
                new object[] { "true", new (string, string[])[] { ("PackageA", ["1.0.0"]) }, true },
                new object[] { "false", new (string, string[])[] { ("PackageA", ["1.0.0"]) }, false },
            };

        [Theory]
        [MemberData(nameof(PrunePackageReferenceData))]
        public async Task GetPackageSpec_WithPrunePackageReferences(string restoreEnablePackagePruning, IEnumerable<(string ItemId, string[] ItemMetadata)> buildIteminfo, bool hasPrunedReferences)
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            using var testDirectory = TestDirectory.Create();
            IReadOnlyList<PackageSpec> packageSpecs = await SetupPrunePackageReferenceDataAndAct(restoreEnablePackagePruning, buildIteminfo, testDirectory);

            // Assert
            Assert.NotNull(packageSpecs);
            var actualRestoreSpec = packageSpecs.Single();
            SpecValidationUtility.ValidateProjectSpec(actualRestoreSpec);


            if (hasPrunedReferences)
            {
                actualRestoreSpec.TargetFrameworks[0].PackagesToPrune.Should().HaveCount(1);
                KeyValuePair<string, PrunePackageReference> result = actualRestoreSpec.TargetFrameworks[0].PackagesToPrune.Single();
                result.Key.Should().Be("PackageA");
                result.Value.Name.Should().Be("PackageA");
                result.Value.VersionRange.Should().Be(VersionRange.Parse("(, 1.0.0]"));
            }
            else
            {
                actualRestoreSpec.TargetFrameworks[0].PackagesToPrune.Should().BeEmpty();
            }
        }

        [Fact]
        public async Task GetPackageSpec_WithPrunePackageReferenceAndMissingVersion_Throws()
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            using var testDirectory = TestDirectory.Create();
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => SetupPrunePackageReferenceDataAndAct("true", new (string, string[])[] { ("PackageA", [null]) }, testDirectory));
            exception.Message.Should().Contain("PrunePackageReference");
        }

        private async Task<IReadOnlyList<PackageSpec>> SetupPrunePackageReferenceDataAndAct(string restoreEnablePackagePruning, IEnumerable<(string ItemId, string[] ItemMetadata)> buildIteminfo, TestDirectory testDirectory)
        {
            // Arrange
            var projectBuildProperties = new Mock<IVsProjectBuildProperties>();
            var projectAdapter = CreateProjectAdapter(testDirectory, projectBuildProperties);
            projectBuildProperties
                .Setup(x => x.GetPropertyValue(It.Is<string>(x => x.Equals(ProjectBuildProperties.RestoreEnablePackagePruning))))
                .Returns(restoreEnablePackagePruning);
            Mock<IVsProjectAdapter> projectAdapterMock = Mock.Get(projectAdapter);
            projectAdapterMock.Setup(m => m.GetBuildItemInformation(ProjectItems.PrunePackageReference, It.IsAny<string[]>()))
                .Returns(buildIteminfo);

            var testProject = new LegacyPackageReferenceProject(
                projectAdapter,
                Guid.NewGuid().ToString(),
                new TestProjectSystemServices(),
                _threadingService);

            var settings = NullSettings.Instance;
            var testDependencyGraphCacheContext = new DependencyGraphCacheContext(NullLogger.Instance, settings);

            // Act
            return await testProject.GetPackageSpecsAsync(testDependencyGraphCacheContext);
        }

        private LegacyPackageReferenceProject CreateLegacyPackageReferenceProject(TestDirectory testDirectory, string range)
        {
            return ProjectFactories.CreateLegacyPackageReferenceProject(testDirectory, Guid.NewGuid().ToString(), range, _threadingService);
        }

        private LegacyPackageReferenceProject CreateLegacyPackageReferenceProjectNoPackages(TestDirectory testDirectory)
        {
            var projectAdapter = CreateProjectAdapter(testDirectory);

            var projectServices = new TestProjectSystemServices();

            var testProject = new LegacyPackageReferenceProject(
                projectAdapter,
                Guid.NewGuid().ToString(),
                projectServices,
                _threadingService);
            return testProject;
        }
    }
}
