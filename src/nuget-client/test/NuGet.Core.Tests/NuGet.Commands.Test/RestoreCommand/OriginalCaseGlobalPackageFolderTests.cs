// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.DependencyResolver;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.ProjectModel;
using NuGet.Protocol.Core.Types;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Test.Utility;
using Xunit;

namespace NuGet.Commands.Test
{
    using LocalPackageArchiveDownloader = Protocol.LocalPackageArchiveDownloader;

    [Collection(nameof(NotThreadSafeResourceCollection))]
    public class OriginalCaseGlobalPackageFolderTests
    {
        [Fact]
        public async Task CopyPackagesToOriginalCaseAsync_WhenPackageMustComeFromProvider_ConvertsPackagesAsync()
        {
            // Arrange
            using (var workingDirectory = TestDirectory.Create())
            {
                var packagesDirectory = Path.Combine(workingDirectory, "packages");
                var sourceDirectory = Path.Combine(workingDirectory, "source");

                // Add the package to the source.
                var identity = new PackageIdentity("PackageA", NuGetVersion.Parse("1.0.0-Beta"));
                var packagePath = await SimpleTestPackageUtility.CreateFullPackageAsync(
                    sourceDirectory,
                    identity.Id,
                    identity.Version.ToString());

                var logger = new TestLogger();
                var graph = GetRestoreTargetGraph(sourceDirectory, identity, packagePath, logger);

                var request = GetRestoreRequest(packagesDirectory, logger);
                var resolver = new VersionFolderPathResolver(packagesDirectory, isLowercase: false);

                var target = new OriginalCaseGlobalPackageFolder(request);

                // Act
                await target.CopyPackagesToOriginalCaseAsync(
                    new[] { graph },
                    CancellationToken.None);

                // Assert
                Assert.True(File.Exists(resolver.GetPackageFilePath(identity.Id, identity.Version)));
                Assert.Equal(1, logger.Messages.Count(x => x.Contains(identity.ToString())));
            }
        }

        [Fact]
        public async Task CopyPackagesToOriginalCaseAsync_WhenPackageComesFromLocalFolder_ConvertsPackagesAsync()
        {
            // Arrange
            using (var workingDirectory = TestDirectory.Create())
            {
                var packagesDirectory = Path.Combine(workingDirectory, "packages");
                var sourceDirectory = Path.Combine(workingDirectory, "source");
                var fallbackDirectory = Path.Combine(workingDirectory, "fallback");

                // Add a different package to the source.
                var identityA = new PackageIdentity("PackageA", NuGetVersion.Parse("1.0.0-Beta"));
                var packagePath = await SimpleTestPackageUtility.CreateFullPackageAsync(
                    sourceDirectory,
                    identityA.Id,
                    identityA.Version.ToString());

                var logger = new TestLogger();
                var identityB = new PackageIdentity("PackageB", NuGetVersion.Parse("2.0.0-Beta"));
                var graph = GetRestoreTargetGraph(sourceDirectory, identityB, packagePath, logger);

                // Add the package to the fallback directory.
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(fallbackDirectory, identityB);

                var request = GetRestoreRequest(packagesDirectory, logger, fallbackDirectory);
                var resolver = new VersionFolderPathResolver(packagesDirectory, isLowercase: false);

                var target = new OriginalCaseGlobalPackageFolder(request);

                // Act
                await target.CopyPackagesToOriginalCaseAsync(
                    new[] { graph },
                    CancellationToken.None);

                // Assert
                Assert.False(File.Exists(resolver.GetPackageFilePath(identityA.Id, identityA.Version)));
                Assert.True(File.Exists(resolver.GetPackageFilePath(identityB.Id, identityB.Version)));
                Assert.Equal(1, logger.Messages.Count(x => x.Contains(identityB.ToString())));
            }
        }

        [Fact]
        public async Task CopyPackagesToOriginalCaseAsync_DoesNothingIfPackageIsAlreadyInstalledAsync()
        {
            // Arrange
            using (var workingDirectory = TestDirectory.Create())
            {
                var packagesDirectory = Path.Combine(workingDirectory, "packages");
                var sourceDirectory = Path.Combine(workingDirectory, "source");

                var identity = new PackageIdentity("PackageA", NuGetVersion.Parse("1.0.0-Beta"));
                var packagePath = await SimpleTestPackageUtility.CreateFullPackageAsync(
                    sourceDirectory,
                    identity.Id,
                    identity.Version.ToString());

                var logger = new TestLogger();
                var graph = GetRestoreTargetGraph(sourceDirectory, identity, packagePath, logger);

                var request = GetRestoreRequest(packagesDirectory, logger);
                var resolver = new VersionFolderPathResolver(packagesDirectory, isLowercase: false);

                var hashPath = resolver.GetNupkgMetadataPath(identity.Id, identity.Version);
                Directory.CreateDirectory(Path.GetDirectoryName(hashPath));

                // The hash file is what determines if the package is installed or not.
                File.WriteAllText(hashPath, string.Empty);

                var target = new OriginalCaseGlobalPackageFolder(request);

                // Act
                await target.CopyPackagesToOriginalCaseAsync(
                    new[] { graph },
                    CancellationToken.None);

                // Assert
                Assert.True(File.Exists(resolver.GetNupkgMetadataPath(identity.Id, identity.Version)));
                Assert.Equal(0, logger.Messages.Count(x => x.Contains(identity.ToString())));
            }
        }

        [Fact]
        public async Task CopyPackagesToOriginalCaseAsync_OnlyInstallsPackagesOnceAsync()
        {
            // Arrange
            using (var workingDirectory = TestDirectory.Create())
            {
                var packagesDirectory = Path.Combine(workingDirectory, "packages");
                var sourceDirectory = Path.Combine(workingDirectory, "source");

                var identity = new PackageIdentity("PackageA", NuGetVersion.Parse("1.0.0-Beta"));
                var packagePath = await SimpleTestPackageUtility.CreateFullPackageAsync(
                    sourceDirectory,
                    identity.Id,
                    identity.Version.ToString());

                var logger = new TestLogger();
                var graphA = GetRestoreTargetGraph(sourceDirectory, identity, packagePath, logger);
                var graphB = GetRestoreTargetGraph(sourceDirectory, identity, packagePath, logger);

                var request = GetRestoreRequest(packagesDirectory, logger);
                var resolver = new VersionFolderPathResolver(packagesDirectory, isLowercase: false);

                var target = new OriginalCaseGlobalPackageFolder(request);

                // Act
                await target.CopyPackagesToOriginalCaseAsync(
                    new[] { graphA, graphB },
                    CancellationToken.None);

                // Assert
                Assert.True(File.Exists(resolver.GetPackageFilePath(identity.Id, identity.Version)));
                Assert.Equal(1, logger.Messages.Count(x => x.Contains(identity.ToString())));
            }
        }

        [Fact]
        public async Task CopyPackagesToOriginalCaseAsync_EmitsTelemetryWithParentIdAsync()
        {
            // Set up telemetry service
            var telemetryEvents = new ConcurrentQueue<TelemetryEvent>();
            var telSvc = new Mock<INuGetTelemetryService>();
            telSvc.Setup(x => x.EmitTelemetryEvent(It.IsAny<TelemetryEvent>()))
                .Callback<TelemetryEvent>(x => telemetryEvents.Enqueue(x));
            TelemetryActivity.NuGetTelemetryService = telSvc.Object;

            var parentIdGuid = Guid.NewGuid();

            // Arrange
            using (var workingDirectory = TestDirectory.Create())
            {
                var packagesDirectory = Path.Combine(workingDirectory, "packages");
                var sourceDirectory = Path.Combine(workingDirectory, "source");

                var identity = new PackageIdentity("PackageA", NuGetVersion.Parse("1.0.0-Beta"));
                var packagePath = await SimpleTestPackageUtility.CreateFullPackageAsync(
                    sourceDirectory,
                    identity.Id,
                    identity.Version.ToString());

                var logger = new TestLogger();
                var graphA = GetRestoreTargetGraph(sourceDirectory, identity, packagePath, logger);
                var graphB = GetRestoreTargetGraph(sourceDirectory, identity, packagePath, logger);

                var request = GetRestoreRequest(packagesDirectory, logger);
                var resolver = new VersionFolderPathResolver(packagesDirectory, isLowercase: false);

                var target = new OriginalCaseGlobalPackageFolder(request, parentIdGuid);

                // Act
                await target.CopyPackagesToOriginalCaseAsync(
                    new[] { graphA, graphB },
                    CancellationToken.None);
            }

            // Assert
            var eventSingleCollection = telemetryEvents.Where(x => x.Name == "PackageExtractionInformation");
            Assert.Single(eventSingleCollection);
            var evt = eventSingleCollection.Single();
            Assert.NotNull(evt["ParentId"]);
            Assert.Equal(evt["ParentId"], parentIdGuid.ToString());
        }

        [Fact]
        public void ConvertLockFileToOriginalCase_ConvertsPackagesPathsInLockFile()
        {
            // Arrange
            var logger = new TestLogger();
            var request = GetRestoreRequest("fake", logger);
            var packageLibrary = new LockFileLibrary
            {
                Name = "PackageA",
                Version = NuGetVersion.Parse("1.0.0-Beta"),
                Path = "packagea/1.0.0-beta",
                Type = LibraryType.Package
            };
            var projectLibrary = new LockFileLibrary
            {
                Name = "Project",
                Version = NuGetVersion.Parse("1.0.0-Beta"),
                Path = "project",
                Type = LibraryType.Project
            };
            var lockFile = new LockFile
            {
                Libraries = { packageLibrary, projectLibrary }
            };

            var target = new OriginalCaseGlobalPackageFolder(request);

            // Act
            target.ConvertLockFileToOriginalCase(lockFile);

            // Assert
            Assert.Equal("PackageA/1.0.0-Beta", packageLibrary.Path);
            Assert.Equal("project", projectLibrary.Path);
        }

        private static RestoreRequest GetRestoreRequest(string packagesDirectory, TestLogger logger, params string[] fallbackDirectories)
        {
            using (var testDirectory = TestDirectory.Create())
            {
                return new TestRestoreRequest(
                    new PackageSpec() { FilePath = testDirectory.Path + "a.csproj" }.EnsureProjectJsonRestoreMetadata(),
                    Enumerable.Empty<PackageSource>(),
                    packagesDirectory,
                    fallbackDirectories,
                    logger)
                {
                    IsLowercasePackagesDirectory = false
                };
            }
        }

        public static RestoreTargetGraph GetRestoreTargetGraph(
            string source,
            PackageIdentity identity,
            FileInfo packagePath,
            TestLogger logger)
        {
            var libraryRange = new LibraryRange { Name = identity.Id };
            var libraryIdentity = new LibraryIdentity(identity.Id, identity.Version, LibraryType.Package);

            var dependencyProvider = new Mock<IRemoteDependencyProvider>();
            IPackageDownloader packageDependency = null;

            dependencyProvider
                .Setup(x => x.GetPackageDownloaderAsync(
                    It.IsAny<PackageIdentity>(),
                    It.IsAny<SourceCacheContext>(),
                    It.IsAny<ILogger>(),
                    It.IsAny<CancellationToken>()))
                .Callback<PackageIdentity, SourceCacheContext, ILogger, CancellationToken>(
                    (callbackIdentity, sourceCacheContext, callbackLogger, cancellationToken) =>
                    {
                        packageDependency = new LocalPackageArchiveDownloader(
                            source,
                            packagePath.FullName,
                            callbackIdentity,
                            callbackLogger);
                    })
                .Returns(() => Task.FromResult(packageDependency));

            var graph = RestoreTargetGraph.Create(
                new[]
                {
                    new GraphNode<RemoteResolveResult>(libraryRange)
                    {
                        Item = new GraphItem<RemoteResolveResult>(libraryIdentity)
                        {
                            Data = new RemoteResolveResult
                            {
                                Match = new RemoteMatch
                                {
                                    Library = libraryIdentity,
                                    Provider = dependencyProvider.Object
                                }
                            }
                        }
                    }
                },
                new TestRemoteWalkContext(),
                logger,
                FrameworkConstants.CommonFrameworks.NetStandard16);

            return graph;
        }
    }
}
