// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceHub.Framework;
using Moq;
using NuGet.Common;
using NuGet.Frameworks;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;
using NuGet.VisualStudio.Internal.Contracts;
using Xunit;

namespace NuGet.PackageManagement.VisualStudio.Test
{
    public class UpdatePackageFeedTests
    {
        private readonly MultiSourcePackageMetadataProvider _metadataProvider;
        private readonly PackageMetadataResource _metadataResource;

        public UpdatePackageFeedTests()
        {
            // dependencies and data
            _metadataResource = Mock.Of<PackageMetadataResource>();

            INuGetResourceProvider provider = FeedTestUtils.CreateTestResourceProvider(_metadataResource);

            var packageSource = new Configuration.PackageSource("http://fake-source");
            var source = new SourceRepository(packageSource, new[] { provider });

            // target
            _metadataProvider = new MultiSourcePackageMetadataProvider(
                new[] { source },
                optionalLocalRepository: null,
                optionalGlobalLocalRepositories: null,
                logger: NullLogger.Instance);
        }

        [Fact]
        public async Task GetPackagesWithUpdatesAsync_WithAllVersions_RetrievesSingleUpdate()
        {
            // Arrange
            var testPackageIdentity = new PackageCollectionItem("FakePackage", new NuGetVersion("1.0.0"), installedReferences: null);
            var serviceBroker = new Mock<IServiceBroker>();
            Mock<INuGetProjectManagerService> projectManagerService = SetupProjectManagerService(serviceBroker);

            IProjectContextInfo projectA = SetupProject(projectManagerService, "FakePackage", "1.0.0");
            SetupRemotePackageMetadata("FakePackage", "0.0.1", "1.0.0", "2.0.1", "2.0.0", "1.0.1");

            var _target = new UpdatePackageFeed(
                serviceBroker.Object,
                new[] { testPackageIdentity },
                _metadataProvider,
                new[] { projectA });

            // Act
            IEnumerable<IPackageSearchMetadata> packages = await _target.GetPackagesWithUpdatesAsync(
                searchText: "fake",
                new SearchFilter(includePrerelease: false),
                CancellationToken.None);

            // Assert
            Assert.Single(packages);
            IPackageSearchMetadata updateCandidate = packages.Single();
            Assert.Equal("2.0.1", updateCandidate.Identity.Version.ToString());

            IEnumerable<VersionInfo> actualVersions = await updateCandidate.GetVersionsAsync();
            Assert.NotEmpty(actualVersions);
            Assert.Equal(
                new[] { "2.0.1", "2.0.0", "1.0.1", "1.0.0", "0.0.1" },
                actualVersions.Select(v => v.Version.ToString()).ToArray());
        }

        [Fact]
        public async Task GetPackagesWithUpdatesAsync_WithAllowedVersions_RetrievesSingleUpdate()
        {
            // Arrange
            var testPackageIdentity = new PackageCollectionItem("FakePackage", new NuGetVersion("1.0.0"), installedReferences: null);
            var serviceBroker = new Mock<IServiceBroker>();
            Mock<INuGetProjectManagerService> projectManagerService = SetupProjectManagerService(serviceBroker);

            IProjectContextInfo projectA = SetupProject(projectManagerService, "FakePackage", "1.0.0", "[1,2)");
            SetupRemotePackageMetadata("FakePackage", "0.0.1", "1.0.0", "2.0.1", "2.0.0", "1.0.1");

            var _target = new UpdatePackageFeed(
                serviceBroker.Object,
                new[] { testPackageIdentity },
                _metadataProvider,
                new[] { projectA });

            // Act
            IEnumerable<IPackageSearchMetadata> packages = await _target.GetPackagesWithUpdatesAsync(
                searchText: "fake",
                new SearchFilter(includePrerelease: false),
                CancellationToken.None);

            // Assert
            Assert.Single(packages);
            IPackageSearchMetadata updateCandidate = packages.Single();
            Assert.Equal("1.0.1", updateCandidate.Identity.Version.ToString());

            IEnumerable<VersionInfo> actualVersions = await updateCandidate.GetVersionsAsync();
            Assert.NotEmpty(actualVersions);
            Assert.Equal(
                new[] { "2.0.1", "2.0.0", "1.0.1", "1.0.0", "0.0.1" },
                actualVersions.Select(v => v.Version.ToString()).ToArray());
        }

        [Fact]
        public async Task GetPackagesWithUpdatesAsync_WithMultipleProjects_RetrievesSingleUpdate1()
        {
            // Arrange
            var testPackageIdentity = new PackageCollectionItem("FakePackage", new NuGetVersion("1.0.0"), installedReferences: null);
            var serviceBroker = new Mock<IServiceBroker>();
            Mock<INuGetProjectManagerService> projectManagerService = SetupProjectManagerService(serviceBroker);

            // Both projects need to be updated to different versions
            // projectA: 1.0.0 => 2.0.1
            // projectB: 1.0.0 => 1.0.1
            IProjectContextInfo projectA = SetupProject(projectManagerService, "FakePackage", "1.0.0");
            IProjectContextInfo projectB = SetupProject(projectManagerService, "FakePackage", "1.0.0", "[1,2)");
            SetupRemotePackageMetadata("FakePackage", "0.0.1", "1.0.0", "2.0.1", "2.0.0", "1.0.1");

            var _target = new UpdatePackageFeed(
                serviceBroker.Object,
                new[] { testPackageIdentity },
                _metadataProvider,
                new[] { projectA, projectB });

            // Act
            IEnumerable<IPackageSearchMetadata> packages = await _target.GetPackagesWithUpdatesAsync(
                searchText: "fake",
                new SearchFilter(includePrerelease: false),
                CancellationToken.None);

            // Assert
            // Should retrieve a single update item with the lowest version and full list of available versions
            Assert.Single(packages);
            IPackageSearchMetadata updateCandidate = packages.Single();
            Assert.Equal("1.0.1", updateCandidate.Identity.Version.ToString());

            IEnumerable<VersionInfo> actualVersions = await updateCandidate.GetVersionsAsync();
            Assert.NotEmpty(actualVersions);
            Assert.Equal(
                new[] { "2.0.1", "2.0.0", "1.0.1", "1.0.0", "0.0.1" },
                actualVersions.Select(v => v.Version.ToString()).ToArray());
        }

        [Fact]
        public async Task GetPackagesWithUpdatesAsync_WithMultipleProjects_RetrievesSingleUpdate2()
        {
            // Arrange
            var testPackageIdentity = new PackageCollectionItem("FakePackage", NuGetVersion.Parse("1.0.0"), installedReferences: null);
            var serviceBroker = new Mock<IServiceBroker>();
            Mock<INuGetProjectManagerService> projectManagerService = SetupProjectManagerService(serviceBroker);

            // Only one project needs to be updated
            // projectA: 2.0.0 => 2.0.1
            // projectB: 1.0.1 => None
            IProjectContextInfo projectA = SetupProject(projectManagerService, "FakePackage", "2.0.0");
            IProjectContextInfo projectB = SetupProject(projectManagerService, "FakePackage", "1.0.1", "[1,2)");
            SetupRemotePackageMetadata("FakePackage", "0.0.1", "1.0.0", "2.0.1", "2.0.0", "1.0.1");

            var _target = new UpdatePackageFeed(
                serviceBroker.Object,
                new[] { testPackageIdentity },
                _metadataProvider,
                new[] { projectA, projectB });

            // Act
            IEnumerable<IPackageSearchMetadata> packages = await _target.GetPackagesWithUpdatesAsync(
                searchText: "fake",
                new SearchFilter(includePrerelease: false),
                CancellationToken.None);

            // Assert
            // Should retrieve a single update item with the lowest version and full list of available versions
            Assert.Single(packages);
            IPackageSearchMetadata updateCandidate = packages.Single();
            Assert.Equal("2.0.1", updateCandidate.Identity.Version.ToString());

            IEnumerable<VersionInfo> actualVersions = await updateCandidate.GetVersionsAsync();
            Assert.NotEmpty(actualVersions);
            Assert.Equal(
                new[] { "2.0.1", "2.0.0", "1.0.1", "1.0.0", "0.0.1" },
                actualVersions.Select(v => v.Version.ToString()).ToArray());
        }

        [Fact]
        public async Task GetPackagesWithUpdatesAsync_WithMultipleProjects_RetrievesSingleUpdate3()
        {
            // Arrange
            var testPackageIdentity = new PackageCollectionItem("FakePackage", NuGetVersion.Parse("1.0.0"), installedReferences: null);
            var serviceBroker = new Mock<IServiceBroker>();
            Mock<INuGetProjectManagerService> projectManagerService = SetupProjectManagerService(serviceBroker);

            // Only one project needs to be updated
            // projectA: 2.0.1 => None
            // projectB: 1.0.0 => 1.0.1
            IProjectContextInfo projectA = SetupProject(projectManagerService, "FakePackage", "2.0.1");
            IProjectContextInfo projectB = SetupProject(projectManagerService, "FakePackage", "1.0.0", "[1,2)");
            SetupRemotePackageMetadata("FakePackage", "0.0.1", "1.0.0", "2.0.1", "2.0.0", "1.0.1");

            var _target = new UpdatePackageFeed(
                serviceBroker.Object,
                new[] { testPackageIdentity },
                _metadataProvider,
                new[] { projectA, projectB });

            // Act
            IEnumerable<IPackageSearchMetadata> packages = await _target.GetPackagesWithUpdatesAsync(
                searchText: "fake",
                new SearchFilter(includePrerelease: false),
                CancellationToken.None);

            // Assert
            // Should retrieve a single update item with the lowest version and full list of available versions
            Assert.Single(packages);
            IPackageSearchMetadata updateCandidate = packages.Single();
            Assert.Equal("1.0.1", updateCandidate.Identity.Version.ToString());

            IEnumerable<VersionInfo> actualVersions = await updateCandidate.GetVersionsAsync();
            Assert.NotEmpty(actualVersions);
            Assert.Equal(
                new[] { "2.0.1", "2.0.0", "1.0.1", "1.0.0", "0.0.1" },
                actualVersions.Select(v => v.Version.ToString()).ToArray());
        }

        [Fact]
        public async Task GetPackagesWithUpdatesAsync_WithMultipleProjects_RetrievesNoUpdates()
        {
            // Arrange
            var testPackageIdentity = new PackageCollectionItem("FakePackage", NuGetVersion.Parse("1.0.0"), installedReferences: null);
            var serviceBroker = new Mock<IServiceBroker>();
            Mock<INuGetProjectManagerService> projectManagerService = SetupProjectManagerService(serviceBroker);

            IProjectContextInfo projectA = SetupProject(projectManagerService, "FakePackage", "2.0.1");
            IProjectContextInfo projectB = SetupProject(projectManagerService, "FakePackage", "1.0.1", "[1,2)");
            SetupRemotePackageMetadata("FakePackage", "0.0.1", "1.0.0", "2.0.1", "2.0.0", "1.0.1");

            var _target = new UpdatePackageFeed(
                serviceBroker.Object,
                new[] { testPackageIdentity },
                _metadataProvider,
                new[] { projectA, projectB });

            // Act
            IEnumerable<IPackageSearchMetadata> packages = await _target.GetPackagesWithUpdatesAsync(
                searchText: "fake",
                new SearchFilter(includePrerelease: false),
                CancellationToken.None);

            // Assert
            Assert.Empty(packages);
        }

        [Fact]
        public async Task GetPackagesWithUpdatesAsync_WithHttpCache_RetrievesUpdate()
        {
            // Arrange
            var testPackageIdentity = new PackageCollectionItem("FakePackage", new NuGetVersion("1.0.0"), installedReferences: null);
            var serviceBroker = new Mock<IServiceBroker>();
            Mock<INuGetProjectManagerService> projectManagerService = SetupProjectManagerService(serviceBroker);

            IProjectContextInfo projectA = SetupProject(projectManagerService, "FakePackage", "1.0.0");
            SetupRemotePackageMetadata("FakePackage", "0.0.1", "1.0.0", "2.0.0");

            var _target = new UpdatePackageFeed(
                serviceBroker.Object,
                new[] { testPackageIdentity },
                _metadataProvider,
                new[] { projectA });

            // Act
            IEnumerable<IPackageSearchMetadata> packages = await _target.GetPackagesWithUpdatesAsync(
                searchText: "fake",
                new SearchFilter(includePrerelease: false),
                CancellationToken.None);

            Assert.Single(packages);
            IPackageSearchMetadata updateCandidate = packages.Single();
            Assert.Equal("2.0.0", updateCandidate.Identity.Version.ToString());

            SetupRemotePackageMetadata("FakePackage", "0.0.1", "1.0.0", "2.0.1", "2.0.0", "1.0.1");

            packages = await _target.GetPackagesWithUpdatesAsync(
                searchText: "fake",
                new SearchFilter(includePrerelease: false),
                CancellationToken.None);

            Assert.Single(packages);
            updateCandidate = packages.Single();
            Assert.Equal("2.0.1", updateCandidate.Identity.Version.ToString());

            IEnumerable<VersionInfo> actualVersions = await updateCandidate.GetVersionsAsync();
            Assert.NotEmpty(actualVersions);
            Assert.Equal(
                new[] { "2.0.1", "2.0.0", "1.0.1", "1.0.0", "0.0.1" },
                actualVersions.Select(v => v.Version.ToString()).ToArray());
        }

        private IProjectContextInfo SetupProject(
            Mock<INuGetProjectManagerService> projectManagerService,
            string packageId,
            string packageVersion,
            string allowedVersions = null)
        {
            var packageIdentity = new PackageIdentity(packageId, NuGetVersion.Parse(packageVersion));

            var installedPackages = new PackageReferenceContextInfo[]
            {
                PackageReferenceContextInfo.Create(
                    new PackageReference(
                        packageIdentity,
                        NuGetFramework.Parse("net45"),
                        userInstalled: true,
                        developmentDependency: false,
                        requireReinstallation: false,
                        allowedVersions: allowedVersions != null ? VersionRange.Parse(allowedVersions) : null))
            };

            string projectId = Guid.NewGuid().ToString();

            projectManagerService.Setup(
                    x => x.GetInstalledPackagesAsync(
                        It.Is<IReadOnlyCollection<string>>(projectIds => projectIds.Single() == projectId),
                        It.IsAny<CancellationToken>()))
                .Returns(new ValueTask<IReadOnlyCollection<IPackageReferenceContextInfo>>(installedPackages));

            var project = new Mock<IProjectContextInfo>();

            project.SetupGet(x => x.ProjectId)
                .Returns(projectId);

            return project.Object;
        }

        private void SetupRemotePackageMetadata(string id, params string[] versions)
        {
            IEnumerable<IPackageSearchMetadata> metadata = versions
                .Select(v => PackageSearchMetadataBuilder
                    .FromIdentity(new PackageIdentity(id, new NuGetVersion(v)))
                    .Build());

            Mock.Get(_metadataResource)
                .Setup(x => x.GetMetadataAsync(id, It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<SourceCacheContext>(), It.IsAny<Common.ILogger>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(metadata));
        }

        [Fact]
        public async Task GetPackagesWithUpdatesAsync_WithMultiplePackages_SortedByPackageId()
        {
            // Arrange
            var testPackageIdentity = new PackageCollectionItem("FakePackage", NuGetVersion.Parse("1.0.0"), installedReferences: null);
            var testPackageIdentity2 = new PackageCollectionItem("AFakePackage", NuGetVersion.Parse("1.0.0"), installedReferences: null);
            var testPackageIdentity3 = new PackageCollectionItem("ZFakePackage", NuGetVersion.Parse("1.0.0"), installedReferences: null);
            var serviceBroker = new Mock<IServiceBroker>();
            Mock<INuGetProjectManagerService> projectManagerService = SetupProjectManagerService(serviceBroker);

            IProjectContextInfo projectA = SetupProject(projectManagerService, "FakePackage", "1.0.0");
            IProjectContextInfo projectB = SetupProject(projectManagerService, "ZFakePackage", "1.0.0");
            IProjectContextInfo projectC = SetupProject(projectManagerService, "AFakePackage", "1.0.0");
            SetupRemotePackageMetadata("FakePackage", "0.0.1", "1.0.0", "2.0.1", "2.0.0", "1.0.1");
            SetupRemotePackageMetadata("ZFakePackage", "0.0.1", "1.0.0", "4.0.0");
            SetupRemotePackageMetadata("AFakePackage", "1.0.0", "3.0.1");

            var _target = new UpdatePackageFeed(
                serviceBroker.Object,
                new[] { testPackageIdentity, testPackageIdentity2, testPackageIdentity3 },
                _metadataProvider,
                new[] { projectA, projectB, projectC });

            // Act
            IEnumerable<IPackageSearchMetadata> packages = await _target.GetPackagesWithUpdatesAsync(
                searchText: "fake",
                new SearchFilter(includePrerelease: false),
                CancellationToken.None);

            IEnumerable<string> actualPackageIds = packages.Select(p => p.Identity.Id);

            // Assert
            var expectedPackageIdsSorted = new List<string>() { "AFakePackage", "FakePackage", "ZFakePackage" };
            Assert.Equal(expectedPackageIdsSorted, actualPackageIds); //Equal considers sort order of collections.
        }

        private static Mock<INuGetProjectManagerService> SetupProjectManagerService(Mock<IServiceBroker> serviceBroker)
        {
            var projectManagerService = new Mock<INuGetProjectManagerService>();

            serviceBroker.Setup(
#pragma warning disable ISB001 // Dispose of proxies
                    x => x.GetProxyAsync<INuGetProjectManagerService>(
                        It.Is<ServiceRpcDescriptor>(s => s == NuGetServices.ProjectManagerService),
                        It.IsAny<ServiceActivationOptions>(),
                        It.IsAny<CancellationToken>()))
#pragma warning restore ISB001 // Dispose of proxies
                .Returns(new ValueTask<INuGetProjectManagerService>(projectManagerService.Object));

            return projectManagerService;
        }
    }
}
