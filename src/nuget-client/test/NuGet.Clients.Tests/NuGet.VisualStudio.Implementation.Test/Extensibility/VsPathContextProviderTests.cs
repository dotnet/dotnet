// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Threading;
using Moq;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.PackageManagement.VisualStudio;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.ProjectManagement;
using NuGet.ProjectManagement.Projects;
using NuGet.ProjectModel;
using NuGet.Test.Utility;
using NuGet.Versioning;
using NuGet.VisualStudio.Implementation.Exceptions;
using NuGet.VisualStudio.Implementation.Extensibility;
using NuGet.VisualStudio.Implementation.Resources;
using NuGet.VisualStudio.Telemetry;
using Xunit;

namespace NuGet.VisualStudio.Implementation.Test.Extensibility
{
    public class VsPathContextProviderTests
    {
        // known/expected errors should not be reported to telemetry, hence use MockBehavior.Strict
        private Mock<INuGetTelemetryProvider> _telemetryProvider = new Mock<INuGetTelemetryProvider>(MockBehavior.Strict);

        [Fact]
        public void GetSolutionPathContext_WithConfiguredUserPackageFolder()
        {
            // Arrange
            var currentDirectory = Directory.GetCurrentDirectory();

            var settings = Mock.Of<ISettings>();
            Mock.Get(settings)
                .Setup(x => x.GetSection("config"))
                .Returns(() => new VirtualSettingSection("config",
                    new AddItem("globalPackagesFolder", "solution/packages")));
            Mock.Get(settings)
                .Setup(s => s.GetConfigFilePaths())
                .Returns(new List<string>());

            var target = new VsPathContextProvider(
                settings,
                Mock.Of<IVsSolutionManager>(),
                Mock.Of<ILogger>(),
                getLockFileOrNullAsync: null,
                _telemetryProvider.Object);

            // Act
            var actual = target.GetSolutionPathContext();

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(Path.Combine(currentDirectory, "solution", "packages"), actual.UserPackageFolder);
        }

        [Fact]
        public void GetSolutionPathContext_WithConfiguredFallbackPackageFolders()
        {
            // Arrange
            var currentDirectory = Directory.GetCurrentDirectory();

            var settings = new Mock<ISettings>();
            settings
                .Setup(x => x.GetSection("fallbackPackageFolders"))
                .Returns(() => new VirtualSettingSection("fallbackPackageFolders",
                    new AddItem("a", "solution/packagesA"),
                    new AddItem("b", "solution/packagesB")
                ));
            settings
                .Setup(x => x.GetConfigFilePaths())
                .Returns(new List<string>());

            var target = new VsPathContextProvider(
                settings.Object,
                Mock.Of<IVsSolutionManager>(),
                Mock.Of<ILogger>(),
                getLockFileOrNullAsync: null,
                _telemetryProvider.Object);

            // Act
            var actual = target.GetSolutionPathContext();

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(
                new[]
                {
                    Path.Combine(currentDirectory, "solution", "packagesA"),
                    Path.Combine(currentDirectory, "solution", "packagesB")
                },
                actual.FallbackPackageFolders.Cast<string>().ToArray());
        }

        [Fact]
        public async Task CreatePathContextAsync_FromAssetsFile()
        {
            // Arrange
            using (var testDirectory = TestDirectory.Create())
            {
                var userPackageFolder = Path.Combine(testDirectory.Path, "packagesA");
                Directory.CreateDirectory(userPackageFolder);

                var fallbackPackageFolder = Path.Combine(testDirectory.Path, "packagesB");
                Directory.CreateDirectory(fallbackPackageFolder);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    userPackageFolder,
                    new PackageIdentity("Foo", NuGetVersion.Parse("1.0.1")));

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    fallbackPackageFolder,
                    new PackageIdentity("Bar", NuGetVersion.Parse("1.0.2")));

                var target = new VsPathContextProvider(
                    Mock.Of<ISettings>(),
                    Mock.Of<IVsSolutionManager>(),
                    Mock.Of<ILogger>(),
                    getLockFileOrNullAsync: _ =>
                    {
                        var lockFile = new LockFile
                        {
                            PackageFolders = new[]
                            {
                                new LockFileItem(userPackageFolder),
                                new LockFileItem(fallbackPackageFolder)
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
                        };

                        return Task.FromResult(lockFile);
                    },
                    _telemetryProvider.Object);

                var project = Mock.Of<BuildIntegratedNuGetProject>();

                // Act
                var actual = await target.CreatePathContextAsync(project, CancellationToken.None);

                // Assert
                Assert.NotNull(actual);
                Assert.Equal(userPackageFolder, actual.UserPackageFolder);
                Assert.Equal(
                    new[] { fallbackPackageFolder },
                    actual.FallbackPackageFolders.Cast<string>().ToArray());

                string actualPackageDirectory = null;

                var packageRootA = Path.Combine(userPackageFolder, "Foo", "1.0.1");
                var assetFileA = Path.Combine(packageRootA, "lib", "net40", "a.dll");
                Assert.True(actual.TryResolvePackageAsset(assetFileA, out actualPackageDirectory));
                Assert.Equal(packageRootA, actualPackageDirectory, ignoreCase: true);

                var packageRootB = Path.Combine(fallbackPackageFolder, "Bar", "1.0.2");
                var assetFileB = Path.Combine(packageRootB, "lib", "net46", "b.dll");
                Assert.True(actual.TryResolvePackageAsset(assetFileB, out actualPackageDirectory));
                Assert.Equal(packageRootB, actualPackageDirectory, ignoreCase: true);
            }
        }

        [Fact]
        public async Task CreatePathContextAsync_FromPackagesConfig()
        {
            // Arrange
            using (var testDirectory = TestDirectory.Create())
            {
                var userPackageFolder = Path.Combine(testDirectory.Path, "packagesA");
                Directory.CreateDirectory(userPackageFolder);

                await SimpleTestPackageUtility.CreateFolderFeedPackagesConfigAsync(
                    userPackageFolder,
                    new PackageIdentity("Foo", NuGetVersion.Parse("1.0.1")));

                var settings = Mock.Of<ISettings>();
                Mock.Get(settings)
                    .Setup(x => x.GetSection("config"))
                    .Returns(() => new VirtualSettingSection("config",
                        new AddItem("globalPackagesFolder", userPackageFolder),
                        new AddItem("repositoryPath", userPackageFolder)));

                var target = new VsPathContextProvider(
                    settings,
                    Mock.Of<IVsSolutionManager>(),
                    Mock.Of<ILogger>(),
                    getLockFileOrNullAsync: null,
                    _telemetryProvider.Object);

                var project = new Mock<MSBuildNuGetProject>(
                    Mock.Of<IMSBuildProjectSystem>(), userPackageFolder, testDirectory.Path);

                project
                    .Setup(x => x.GetInstalledPackagesAsync(It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new[]
                    {
                        new PackageReference(
                            new PackageIdentity("Foo", NuGetVersion.Parse("1.0.1")),
                            NuGetFramework.AnyFramework)
                    });

                // Act
                var actual = await target.CreatePathContextAsync(project.Object, CancellationToken.None);

                // Assert
                Assert.NotNull(actual);
                Assert.Equal(userPackageFolder, actual.UserPackageFolder);

                string actualPackageDirectory = null;

                var packageRootA = Path.Combine(userPackageFolder, "Foo.1.0.1");
                var assetFileA = Path.Combine(packageRootA, "lib", "net45", "a.dll");
                Assert.True(actual.TryResolvePackageAsset(assetFileA, out actualPackageDirectory));
                Assert.Equal(packageRootA, actualPackageDirectory, ignoreCase: true);
            }
        }

        [Fact]
        public async Task CreatePathContextAsync_WithUnrestoredPackagesConfig_Throws()
        {
            // Arrange
            using (var testDirectory = TestDirectory.Create())
            {
                var userPackageFolder = Path.Combine(testDirectory.Path, "packagesA");

                var settings = Mock.Of<ISettings>();
                Mock.Get(settings)
                    .Setup(x => x.GetSection("config"))
                    .Returns(() => new VirtualSettingSection("config",
                        new AddItem("globalPackagesFolder", userPackageFolder),
                        new AddItem("repositoryPath", userPackageFolder)));

                var target = new VsPathContextProvider(
                    settings,
                    Mock.Of<IVsSolutionManager>(),
                    Mock.Of<ILogger>(),
                    getLockFileOrNullAsync: null,
                    _telemetryProvider.Object);

                var projectUniqueName = Guid.NewGuid().ToString();

                var projectSystem = Mock.Of<IMSBuildProjectSystem>();
                Mock.Get(projectSystem)
                    .SetupGet(x => x.ProjectUniqueName)
                    .Returns(projectUniqueName);

                var project = new Mock<MSBuildNuGetProject>(
                    projectSystem, userPackageFolder, testDirectory.Path);

                project
                    .Setup(x => x.GetInstalledPackagesAsync(It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new[]
                    {
                        new PackageReference(
                            new PackageIdentity("Foo", NuGetVersion.Parse("1.0.1")),
                            NuGetFramework.AnyFramework)
                    });

                // Act & Assert
                var exception = await Assert.ThrowsAsync<ProjectNotRestoredException>(() => target.CreatePathContextAsync(project.Object, CancellationToken.None));
                Assert.Equal(0, _telemetryProvider.Invocations.Count);
                Assert.Contains(projectUniqueName, exception.Message);
            }
        }

        [Fact]
        public void CreateSolutionContext_WithConfiguredUserPackageFolder()
        {
            // Arrange
            using (var testDirectory = TestDirectory.Create())
            {
                var currentDirectory = Directory.GetCurrentDirectory();

                var settings = Mock.Of<ISettings>();
                Mock.Get(settings)
                    .Setup(x => x.GetSection("config"))
                    .Returns(() => new VirtualSettingSection("config",
                        new AddItem("globalPackagesFolder", "solution/packages")));
                Mock.Get(settings);


                var solutionManager = new Mock<IVsSolutionManager>();
                solutionManager
                    .Setup(x => x.SolutionDirectory)
                    .Returns(testDirectory.Path);

                var target = new VsPathContextProvider(
                    settings,
                    solutionManager.Object,
                    Mock.Of<ILogger>(),
                    getLockFileOrNullAsync: null,
                    _telemetryProvider.Object);

                // Act
                var result = target.TryCreateSolutionContext(out var actual);

                // Assert
                Assert.True(result);
                Assert.NotNull(actual);
                Assert.Equal(Path.Combine(currentDirectory, "solution", "packages"), actual.UserPackageFolder);
            }
        }

        [Fact]
        public void CreateSolutionContext_WithConfiguredFallbackPackageFolders()
        {
            // Arrange
            using (var testDirectory = TestDirectory.Create())
            {
                var currentDirectory = Directory.GetCurrentDirectory();

                var settings = new Mock<ISettings>();
                settings
                .Setup(x => x.GetSection("fallbackPackageFolders"))
                .Returns(() => new VirtualSettingSection("fallbackPackageFolders",
                    new AddItem("a", "solution/packagesA"),
                    new AddItem("b", "solution/packagesB")
                ));
                settings.Setup(s => s.GetConfigFilePaths())
                .Returns(new List<string>());

                var solutionManager = new Mock<IVsSolutionManager>();
                solutionManager
                    .Setup(x => x.SolutionDirectory)
                    .Returns(testDirectory.Path);

                var target = new VsPathContextProvider(
                    settings.Object,
                    solutionManager.Object,
                    Mock.Of<ILogger>(),
                    getLockFileOrNullAsync: null,
                    _telemetryProvider.Object);

                // Act
                var result = target.TryCreateSolutionContext(out var actual);

                // Assert
                Assert.True(result);
                Assert.NotNull(actual);
                Assert.Equal(
                    new[]
                    {
                    Path.Combine(currentDirectory, "solution", "packagesA"),
                    Path.Combine(currentDirectory, "solution", "packagesB")
                    },
                    actual.FallbackPackageFolders.Cast<string>().ToArray());
            }
        }

        [Fact]
        public void CreateSolutionContext_WithPackagesConfig()
        {
            // Arrange
            using (var testDirectory = TestDirectory.Create())
            {
                var solutionPackageFolder = Path.Combine(testDirectory.Path, "packagesA");
                Directory.CreateDirectory(solutionPackageFolder);

                var solutionManager = new Mock<IVsSolutionManager>();
                solutionManager
                    .Setup(x => x.SolutionDirectory)
                    .Returns(testDirectory.Path);

                var settings = Mock.Of<ISettings>();
                Mock.Get(settings)
                    .Setup(x => x.GetSection("config"))
                    .Returns(() => new VirtualSettingSection("config",
                        new AddItem("repositoryPath", solutionPackageFolder)));

                var target = new VsPathContextProvider(
                settings,
                solutionManager.Object,
                Mock.Of<ILogger>(),
                getLockFileOrNullAsync: null,
                _telemetryProvider.Object);

                // Act
                var result = target.TryCreateSolutionContext(out var actual);

                // Assert
                Assert.True(result);
                Assert.NotNull(actual);
                Assert.Equal(solutionPackageFolder, actual.SolutionPackageFolder);
            }
        }

        [Fact]
        public void CreateSolutionContext_WithSolutionDirectory()
        {
            // Arrange
            using (var testDirectory = TestDirectory.Create())
            {
                var solutionPackageFolder = Path.Combine(testDirectory.Path, "packages");
                Directory.CreateDirectory(solutionPackageFolder);

                var target = new VsPathContextProvider(
                Mock.Of<ISettings>(),
                Mock.Of<IVsSolutionManager>(),
                Mock.Of<ILogger>(),
                getLockFileOrNullAsync: null,
                _telemetryProvider.Object);

                // Act
                var result = target.TryCreateSolutionContext(testDirectory.Path, out var actual);

                // Assert
                Assert.True(result);
                Assert.NotNull(actual);
                Assert.Equal(solutionPackageFolder, actual.SolutionPackageFolder);
            }

        }

        [Fact]
        public async Task CreatePathContextAsync_WithUnrestoredPackageReference_Throws()
        {
            // Arrange
            var target = new VsPathContextProvider(
                Mock.Of<ISettings>(),
                Mock.Of<IVsSolutionManager>(),
                Mock.Of<ILogger>(),
                getLockFileOrNullAsync: _ => Task.FromResult(null as LockFile),
                _telemetryProvider.Object);

            var projectUniqueName = Guid.NewGuid().ToString();

            var project = new TestPackageReferenceProject(projectUniqueName);

            // Assert
            var exception = await Assert.ThrowsAsync<ProjectNotRestoredException>(() => target.CreatePathContextAsync(project, CancellationToken.None));
            Assert.Contains(projectUniqueName, exception.Message);
        }

        [Fact]
        public async Task CreatePathContextAsync_CancellationToken_ThrowsAsync()
        {
            // Prepare
            var target = new VsPathContextProvider(
                Mock.Of<ISettings>(),
                Mock.Of<IVsSolutionManager>(),
                Mock.Of<ILogger>(),
                getLockFileOrNullAsync: _ => Task.FromResult(null as LockFile),
                _telemetryProvider.Object);
            var project = new TestPackageReferenceProject(Guid.NewGuid().ToString()); // non-packages.config project
            var cts = new CancellationTokenSource();

            cts.Cancel();
            var task = target.CreatePathContextAsync(project, cts.Token);

            // Act and Assert
            await Assert.ThrowsAsync<OperationCanceledException>(async () => await task);
        }

        private class TestPackageReferenceProject : BuildIntegratedNuGetProject
        {
            private readonly string _projectName;

            public TestPackageReferenceProject(string projectUniqueName)
            {
                _projectName = projectUniqueName;

                InternalMetadata.Add(NuGetProjectMetadataKeys.Name, _projectName);
                InternalMetadata.Add(NuGetProjectMetadataKeys.UniqueName, _projectName);
            }

            public override string ProjectName => _projectName;

            public override string MSBuildProjectPath
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public override Task<string> GetAssetsFilePathAsync()
            {
                throw new NotImplementedException();
            }

            public override Task<string> GetAssetsFilePathOrNullAsync()
            {
                throw new NotImplementedException();
            }

            public override Task AddFileToProjectAsync(string filePath)
            {
                throw new NotImplementedException();
            }

            public override Task<string> GetCacheFilePathAsync()
            {
                throw new NotImplementedException();
            }

            public override Task<IEnumerable<PackageReference>> GetInstalledPackagesAsync(CancellationToken token)
            {
                throw new NotImplementedException();
            }

            public override Task<IReadOnlyList<PackageSpec>> GetPackageSpecsAsync(DependencyGraphCacheContext context)
            {
                throw new NotImplementedException();
            }

            public override Task<(IReadOnlyList<PackageSpec> dgSpecs, IReadOnlyList<IAssetsLogMessage> additionalMessages)> GetPackageSpecsAndAdditionalMessagesAsync(DependencyGraphCacheContext context)
            {
                throw new NotImplementedException();
            }

            public override Task<bool> InstallPackageAsync(string packageId, VersionRange range, INuGetProjectContext nuGetProjectContext, BuildIntegratedInstallationContext installationContext, CancellationToken token)
            {
                throw new NotImplementedException();
            }

            public override Task<bool> UninstallPackageAsync(PackageIdentity packageIdentity, INuGetProjectContext nuGetProjectContext, CancellationToken token)
            {
                throw new NotImplementedException();
            }
        }
    }
}
