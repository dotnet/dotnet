// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.ServiceHub.Framework;
using Microsoft.VisualStudio.Sdk.TestFramework;
using Microsoft.VisualStudio.Threading;
using Moq;
using NuGet.Configuration;
using NuGet.PackageManagement.UI.ViewModels;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using NuGet.Test.Utility;
using NuGet.Versioning;
using NuGet.VisualStudio.Internal.Contracts;
using Test.Utility;
using Xunit;

namespace NuGet.PackageManagement.UI.Test
{
    [Collection(MockedVS.Collection)]
    public class PackageItemLoaderTests
    {
        private const string TestSearchTerm = "nuget";

        public PackageItemLoaderTests(GlobalServiceProvider sp)
        {
            sp.Reset();
        }

        [Fact]
        public async Task MultipleSourcesPrefixReserved_Works()
        {
            var solutionManager = Mock.Of<INuGetSolutionManagerService>();
            var uiContext = new Mock<INuGetUIContext>();
            var searchService = new Mock<INuGetSearchService>(MockBehavior.Strict);

            var packageSearchMetadata = new PackageSearchMetadataBuilder.ClonedPackageSearchMetadata()
            {
                Identity = new PackageIdentity("NuGet.org", new NuGetVersion("1.0")),
                PrefixReserved = true
            };

            var packageSearchMetadataContextInfo = new List<PackageSearchMetadataContextInfo>()
            {
                PackageSearchMetadataContextInfo.Create(packageSearchMetadata)
            };

            var searchResult = new SearchResultContextInfo(packageSearchMetadataContextInfo, new Dictionary<string, LoadingStatus> { { "Completed", LoadingStatus.Ready } }, false);

            searchService.Setup(x =>
                x.SearchAsync(
                    It.IsAny<IReadOnlyCollection<IProjectContextInfo>>(),
                    It.IsAny<IReadOnlyCollection<PackageSourceContextInfo>>(),
                    It.IsAny<IReadOnlyCollection<string>>(),
                    It.IsAny<string>(),
                    It.IsAny<SearchFilter>(),
                    It.IsAny<NuGet.VisualStudio.Internal.Contracts.ItemFilter>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>(),
                    It.IsAny<CancellationToken>()))
                .Returns(new ValueTask<SearchResultContextInfo>(searchResult));

            searchService.Setup(x =>
                x.RefreshSearchAsync(It.IsAny<CancellationToken>()))
                .Returns(new ValueTask<SearchResultContextInfo>(searchResult));

            var packageFileService = new Mock<INuGetPackageFileService>();

            uiContext.Setup(x => x.SolutionManagerService)
                .Returns(solutionManager);

            uiContext.Setup(x => x.ServiceBroker)
                .Returns(Mock.Of<IServiceBroker>());

            var source1 = new PackageSourceContextInfo("https://pkgs.dev.azure.com/dnceng/public/_packaging/nuget-build/nuget/v3/index.json", "NuGetBuild");
            var source2 = new PackageSourceContextInfo("https://api.nuget.org/v3/index.json", "NuGet.org");

            var context = new PackageLoadContext(false, uiContext.Object);
            var loader = await PackageItemLoader.CreateAsync(
                Mock.Of<IServiceBroker>(),
                context,
                new List<PackageSourceContextInfo> { source1, source2 },
                NuGet.VisualStudio.Internal.Contracts.ItemFilter.All,
                searchService.Object,
                packageFileService.Object,
                TestSearchTerm);

            await loader.LoadNextAsync(null, CancellationToken.None);
            var items = loader.GetCurrent();

            Assert.NotEmpty(items);

            // All items should not have a prefix reserved because the feed is multisource
            foreach (var item in items)
            {
                Assert.False(item.PrefixReserved);
            }
        }

        [Fact]
        public async Task PackagePath_NotNull()
        {
            // Prepare
            var solutionManager = Mock.Of<INuGetSolutionManagerService>();
            var uiContext = new Mock<INuGetUIContext>();

            uiContext.Setup(x => x.SolutionManagerService)
                .Returns(solutionManager);
            var searchService = new Mock<INuGetSearchService>(MockBehavior.Strict);

            var packageSearchMetadata = new PackageSearchMetadataBuilder.ClonedPackageSearchMetadata()
            {
                Identity = new PackageIdentity("NuGet.org", new NuGetVersion("1.0")),
                PrefixReserved = true,
                PackagePath = "somesillypath",
            };

            var packageSearchMetadataContextInfo = new List<PackageSearchMetadataContextInfo>()
            {
                PackageSearchMetadataContextInfo.Create(packageSearchMetadata)
            };

            var searchResult = new SearchResultContextInfo(packageSearchMetadataContextInfo, new Dictionary<string, LoadingStatus> { { "Search", LoadingStatus.Loading } }, false);

            searchService.Setup(x =>
                x.SearchAsync(
                    It.IsAny<IReadOnlyCollection<IProjectContextInfo>>(),
                    It.IsAny<IReadOnlyCollection<PackageSourceContextInfo>>(),
                    It.IsAny<IReadOnlyCollection<string>>(),
                    It.IsAny<string>(),
                    It.IsAny<SearchFilter>(),
                    It.IsAny<NuGet.VisualStudio.Internal.Contracts.ItemFilter>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>(),
                    It.IsAny<CancellationToken>()))
                .Returns(new ValueTask<SearchResultContextInfo>(searchResult));

            var packageFileService = new Mock<INuGetPackageFileService>();

            uiContext.Setup(x => x.ServiceBroker)
                .Returns(Mock.Of<IServiceBroker>());

            using (var localFeedDir = TestDirectory.Create()) // local feed
            {
                // create test package
                var pkgId = new PackageIdentity("nuget.lpsm.test", new NuGetVersion(0, 0, 1));
                var pkg = new SimpleTestPackageContext(pkgId.Id, pkgId.Version.ToNormalizedString());
                await SimpleTestPackageUtility.CreatePackagesAsync(localFeedDir.Path, pkg);

                // local test source
                var localUri = new Uri(localFeedDir.Path, UriKind.Absolute);
                var localSource = new PackageSource(localUri.ToString(), "LocalSource");

                var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(new[] { localSource });
                var repositories = sourceRepositoryProvider.GetRepositories();

                var context = new PackageLoadContext(isSolution: false, uiContext.Object);

                var loader = await PackageItemLoader.CreateAsync(
                    Mock.Of<IServiceBroker>(),
                    context,
                    new List<PackageSourceContextInfo> { PackageSourceContextInfo.Create(localSource) },
                    NuGet.VisualStudio.Internal.Contracts.ItemFilter.All,
                    searchService.Object,
                    packageFileService.Object,
                    TestSearchTerm);

                // Act
                await loader.LoadNextAsync(progress: null, CancellationToken.None);
                var results = loader.GetCurrent();

                // Assert
                Assert.Single(results);
                Assert.NotNull(results.First().PackagePath);
            }
        }

        [Theory]
        [InlineData(new object[] { new string[] { } })]
        [InlineData(new object[] { new[] { "A", "B", "C" } })]
        [InlineData(new object[] { new[] { "A", "C", "B" } })]
        [InlineData(new object[] { new[] { "B", "A", "C" } })]
        [InlineData(new object[] { new[] { "B", "C", "A" } })]
        [InlineData(new object[] { new[] { "C", "A", "B" } })]
        [InlineData(new object[] { new[] { "C", "B", "A" } })]
        [InlineData(new object[] { new[] { "A", "C", "B", "D" } })]
        [InlineData(new object[] { new[] { "A" } })]
        [InlineData(new object[] { new[] { "pkg2", "pkg3", "__pkg__" } })]
        public async Task GetCurrent_WithAnySearchResults_PreservesSearchResultsOrderAsync(string[] inputIds)
        {
            // Arrange
            var psmContextInfos = new List<PackageSearchMetadataContextInfo>();
            foreach (var id in inputIds)
            {
                psmContextInfos.Add(PackageSearchMetadataContextInfo.Create(new PackageSearchMetadataBuilder.ClonedPackageSearchMetadata()
                {
                    Identity = new PackageIdentity(id, new NuGetVersion("1.0")),
                }));
            }
            var searchResult = new SearchResultContextInfo(psmContextInfos, new Dictionary<string, LoadingStatus> { { "Search", LoadingStatus.Loading } }, hasMoreItems: false);

            var serviceBroker = Mock.Of<IServiceBroker>();
            var packageFileService = new Mock<INuGetPackageFileService>();
            var searchService = new Mock<INuGetSearchService>(MockBehavior.Strict);
            searchService.Setup(s => s.SearchAsync(It.IsAny<IReadOnlyCollection<IProjectContextInfo>>(),
                    It.IsAny<IReadOnlyCollection<PackageSourceContextInfo>>(),
                    It.IsAny<IReadOnlyCollection<string>>(),
                    It.IsAny<string>(),
                    It.IsAny<SearchFilter>(),
                    It.IsAny<NuGet.VisualStudio.Internal.Contracts.ItemFilter>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>(),
                    It.IsAny<CancellationToken>()))
                .Returns(new ValueTask<SearchResultContextInfo>(searchResult));
            var uiContext = new Mock<INuGetUIContext>();
            uiContext.Setup(ui => ui.ServiceBroker).Returns(serviceBroker);
            var context = new PackageLoadContext(isSolution: false, uiContext.Object);
            var mockProgress = Mock.Of<IProgress<IItemLoaderState>>();

            using var localFeedDir = TestDirectory.Create(); // local feed
            var localSource = new PackageSource(localFeedDir);
            var loader = await PackageItemLoader.CreateAsync(
                serviceBroker,
                context,
                new List<PackageSourceContextInfo>() { PackageSourceContextInfo.Create(localSource) },
                NuGet.VisualStudio.Internal.Contracts.ItemFilter.All,
                searchService.Object,
                packageFileService.Object,
                TestSearchTerm);

            // Act
            await loader.LoadNextAsync(progress: mockProgress, CancellationToken.None);
            IEnumerable<PackageItemViewModel> items = loader.GetCurrent();

            // Assert
            string[] result = items.Select(pkg => pkg.Id).ToArray();
            Assert.Equal(inputIds, result);
        }

        [Fact]
        public async Task GetCurrent_HasKnownOwners_CreatesKnownOwnerViewModelsAsync()
        {
            var version = NuGetVersion.Parse("4.3.0");
            var packageSearchMetadata = new PackageSearchMetadataBuilder.ClonedPackageSearchMetadata()
            {
                Identity = new PackageIdentity("NuGet.Versioning", version),
                OwnersList = new List<string> { "owner1", "owner2" },
            };
            PackageSource packageSource = new PackageSource("https://nuget.test/v3/index.json");
            Mock<IOwnerDetailsUriService> ownerDetailsUriService = new Mock<IOwnerDetailsUriService>();
            ownerDetailsUriService.Setup(x => x.SupportsKnownOwners).Returns(true);
            ownerDetailsUriService.Setup(x => x.GetOwnerDetailsUri(It.IsAny<string>())).Returns((string owner) => new Uri($"https://nuget.test/profiles/{owner}?_src=template"));

            var knownOwner1 = new KnownOwner("owner1", new Uri("https://nuget.test/profiles/owner1?_src=template"));
            var knownOwner2 = new KnownOwner("owner2", new Uri("https://nuget.test/profiles/owner2?_src=template"));
            IReadOnlyList<KnownOwner> knownOwners = new List<KnownOwner>(capacity: 2)
            {
                knownOwner1,
                knownOwner2
            };

            var packageSearchMetadataContextInfo = PackageSearchMetadataContextInfo.Create(packageSearchMetadata, knownOwners);
            var searchResult = new SearchResultContextInfo(new[] { packageSearchMetadataContextInfo }, new Dictionary<string, LoadingStatus> { { "Search", LoadingStatus.Loading } }, hasMoreItems: false);
            var serviceBroker = Mock.Of<IServiceBroker>();
            var testVersions = new List<VersionInfoContextInfo>() {
                new VersionInfoContextInfo(version),
            };

            var searchService = new Mock<INuGetSearchService>(MockBehavior.Strict);
            searchService.Setup(ss => ss.GetPackageVersionsAsync(
                It.IsAny<PackageIdentity>(),
                It.IsAny<IReadOnlyCollection<PackageSourceContextInfo>>(),
                It.IsAny<bool>(),
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(testVersions);
            searchService.Setup(ss => ss.GetPackageMetadataAsync(It.IsAny<PackageIdentity>(), It.IsAny<IReadOnlyCollection<PackageSourceContextInfo>>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((packageSearchMetadataContextInfo, It.IsAny<PackageDeprecationMetadataContextInfo>()));
            searchService.Setup(s => s.SearchAsync(It.IsAny<IReadOnlyCollection<IProjectContextInfo>>(),
                It.IsAny<IReadOnlyCollection<PackageSourceContextInfo>>(),
                It.IsAny<IReadOnlyCollection<string>>(),
                It.IsAny<string>(),
                It.IsAny<SearchFilter>(),
                It.IsAny<NuGet.VisualStudio.Internal.Contracts.ItemFilter>(),
                It.IsAny<bool>(),
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()))
                .Returns(new ValueTask<SearchResultContextInfo>(searchResult));
            searchService.Setup(s => s.RefreshSearchAsync(It.IsAny<CancellationToken>()))
                .Returns(new ValueTask<SearchResultContextInfo>(searchResult));
            var uiContext = new Mock<INuGetUIContext>();
            uiContext.Setup(ui => ui.ServiceBroker).Returns(serviceBroker);
            var context = new PackageLoadContext(isSolution: false, uiContext.Object);
            var mockProgress = Mock.Of<IProgress<IItemLoaderState>>();

            var loader = await PackageItemLoader.CreateAsync(
                serviceBroker,
                context,
                new List<PackageSourceContextInfo>() { PackageSourceContextInfo.Create(packageSource) },
                NuGet.VisualStudio.Internal.Contracts.ItemFilter.All,
                searchService.Object,
                Mock.Of<INuGetPackageFileService>(),
                TestSearchTerm);

            // Act
            await loader.UpdateStateAndReportAsync(searchResult, Mock.Of<IProgress<IItemLoaderState>>(), CancellationToken.None);
            IEnumerable<PackageItemViewModel> viewModels = loader.GetCurrent();

            // Assert
            ImmutableList<KnownOwnerViewModel> knownOwnerViewModels = viewModels.Single().KnownOwnerViewModels;
            knownOwnerViewModels.Should()
                .NotBeNull()
                .And.HaveCount(2)
                .And.BeEquivalentTo(new[]
                {
                    knownOwner1,
                    knownOwner2
                });
        }

        [Fact]
        public async Task GetCurrent_HasKnownOwners_NotOnBrowseTab_DoesNotCreateKnownOwnerViewModels()
        {
            var version = NuGetVersion.Parse("4.3.0");
            var packageSearchMetadata = new PackageSearchMetadataBuilder.ClonedPackageSearchMetadata()
            {
                Identity = new PackageIdentity("NuGet.Versioning", version),
                OwnersList = new List<string> { "owner1", "owner2" },
            };
            PackageSource packageSource = new PackageSource("https://nuget.test/v3/index.json");
            Mock<IOwnerDetailsUriService> ownerDetailsUriService = new Mock<IOwnerDetailsUriService>();
            ownerDetailsUriService.Setup(x => x.SupportsKnownOwners).Returns(true);
            ownerDetailsUriService.Setup(x => x.GetOwnerDetailsUri(It.IsAny<string>())).Returns((string owner) => new Uri($"https://nuget.test/profiles/{owner}?_src=template"));

            var knownOwner1 = new KnownOwner("owner1", new Uri("https://nuget.test/profiles/owner1?_src=template"));
            var knownOwner2 = new KnownOwner("owner2", new Uri("https://nuget.test/profiles/owner2?_src=template"));
            IReadOnlyList<KnownOwner> knownOwners = new List<KnownOwner>(capacity: 2)
            {
                knownOwner1,
                knownOwner2
            };

            var packageSearchMetadataContextInfo = PackageSearchMetadataContextInfo.Create(packageSearchMetadata, knownOwners);
            var searchResult = new SearchResultContextInfo(new[] { packageSearchMetadataContextInfo }, new Dictionary<string, LoadingStatus> { { "Search", LoadingStatus.Loading } }, hasMoreItems: false);
            var serviceBroker = Mock.Of<IServiceBroker>();
            var testVersions = new List<VersionInfoContextInfo>() {
                new VersionInfoContextInfo(version),
            };

            var searchService = new Mock<INuGetSearchService>(MockBehavior.Strict);
            searchService.Setup(ss => ss.GetPackageVersionsAsync(
                It.IsAny<PackageIdentity>(),
                It.IsAny<IReadOnlyCollection<PackageSourceContextInfo>>(),
                It.IsAny<bool>(),
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(testVersions);
            searchService.Setup(ss => ss.GetPackageMetadataAsync(It.IsAny<PackageIdentity>(), It.IsAny<IReadOnlyCollection<PackageSourceContextInfo>>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((packageSearchMetadataContextInfo, It.IsAny<PackageDeprecationMetadataContextInfo>()));
            searchService.Setup(s => s.SearchAsync(It.IsAny<IReadOnlyCollection<IProjectContextInfo>>(),
                It.IsAny<IReadOnlyCollection<PackageSourceContextInfo>>(),
                It.IsAny<IReadOnlyCollection<string>>(),
                It.IsAny<string>(),
                It.IsAny<SearchFilter>(),
                It.IsAny<NuGet.VisualStudio.Internal.Contracts.ItemFilter>(),
                It.IsAny<bool>(),
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()))
                .Returns(new ValueTask<SearchResultContextInfo>(searchResult));
            searchService.Setup(s => s.RefreshSearchAsync(It.IsAny<CancellationToken>()))
                .Returns(new ValueTask<SearchResultContextInfo>(searchResult));
            var uiContext = new Mock<INuGetUIContext>();
            uiContext.Setup(ui => ui.ServiceBroker).Returns(serviceBroker);
            var context = new PackageLoadContext(isSolution: false, uiContext.Object);
            var mockProgress = Mock.Of<IProgress<IItemLoaderState>>();

            var loader = await PackageItemLoader.CreateAsync(
                serviceBroker,
                context,
                new List<PackageSourceContextInfo>() { PackageSourceContextInfo.Create(packageSource) },
                NuGet.VisualStudio.Internal.Contracts.ItemFilter.Installed,
                searchService.Object,
                Mock.Of<INuGetPackageFileService>(),
                TestSearchTerm);

            // Act
            await loader.UpdateStateAndReportAsync(searchResult, Mock.Of<IProgress<IItemLoaderState>>(), CancellationToken.None);
            IEnumerable<PackageItemViewModel> viewModels = loader.GetCurrent();

            // Assert
            ImmutableList<KnownOwnerViewModel> knownOwnerViewModels = viewModels.Single().KnownOwnerViewModels;
            knownOwnerViewModels.Should().BeNull();
        }

        [Fact]
        public async Task GetCurrent_HasKnownOwners_IsRecommendedPackage_DoesNotCreateKnownOwnerViewModels()
        {
            var versionString = "4.3.0";
            var version = NuGetVersion.Parse(versionString);
            var recommendedPackageSearchMetadata = new RecommendedPackageSearchMetadata(
            new PackageSearchMetadataBuilder.ClonedPackageSearchMetadata()
            {
                Identity = new PackageIdentity("NuGet.Versioning", version),
                OwnersList = new List<string> { "owner1", "owner2" },
            },
            recommenderVersion: (versionString, versionString));

            PackageSource packageSource = new PackageSource("https://nuget.test/v3/index.json");
            Mock<IOwnerDetailsUriService> ownerDetailsUriService = new Mock<IOwnerDetailsUriService>();
            ownerDetailsUriService.Setup(x => x.SupportsKnownOwners).Returns(true);
            ownerDetailsUriService.Setup(x => x.GetOwnerDetailsUri(It.IsAny<string>())).Returns((string owner) => new Uri($"https://nuget.test/profiles/{owner}?_src=template"));

            var knownOwner1 = new KnownOwner("owner1", new Uri("https://nuget.test/profiles/owner1?_src=template"));
            var knownOwner2 = new KnownOwner("owner2", new Uri("https://nuget.test/profiles/owner2?_src=template"));
            IReadOnlyList<KnownOwner> knownOwners = new List<KnownOwner>(capacity: 2)
            {
                knownOwner1,
                knownOwner2
            };

            var packageSearchMetadataContextInfo = PackageSearchMetadataContextInfo.Create(recommendedPackageSearchMetadata, knownOwners);
            var searchResult = new SearchResultContextInfo(new[] { packageSearchMetadataContextInfo }, new Dictionary<string, LoadingStatus> { { "Search", LoadingStatus.Loading } }, hasMoreItems: false);
            var serviceBroker = Mock.Of<IServiceBroker>();
            var testVersions = new List<VersionInfoContextInfo>() {
                new VersionInfoContextInfo(version),
            };

            var searchService = new Mock<INuGetSearchService>(MockBehavior.Strict);
            searchService.Setup(ss => ss.GetPackageVersionsAsync(
                It.IsAny<PackageIdentity>(),
                It.IsAny<IReadOnlyCollection<PackageSourceContextInfo>>(),
                It.IsAny<bool>(),
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(testVersions);
            searchService.Setup(ss => ss.GetPackageMetadataAsync(It.IsAny<PackageIdentity>(), It.IsAny<IReadOnlyCollection<PackageSourceContextInfo>>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((packageSearchMetadataContextInfo, It.IsAny<PackageDeprecationMetadataContextInfo>()));
            searchService.Setup(s => s.SearchAsync(It.IsAny<IReadOnlyCollection<IProjectContextInfo>>(),
                It.IsAny<IReadOnlyCollection<PackageSourceContextInfo>>(),
                It.IsAny<IReadOnlyCollection<string>>(),
                It.IsAny<string>(),
                It.IsAny<SearchFilter>(),
                It.IsAny<NuGet.VisualStudio.Internal.Contracts.ItemFilter>(),
                It.IsAny<bool>(),
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()))
                .Returns(new ValueTask<SearchResultContextInfo>(searchResult));
            searchService.Setup(s => s.RefreshSearchAsync(It.IsAny<CancellationToken>()))
                .Returns(new ValueTask<SearchResultContextInfo>(searchResult));
            var uiContext = new Mock<INuGetUIContext>();
            uiContext.Setup(ui => ui.ServiceBroker).Returns(serviceBroker);
            var context = new PackageLoadContext(isSolution: false, uiContext.Object);
            var mockProgress = Mock.Of<IProgress<IItemLoaderState>>();

            var loader = await PackageItemLoader.CreateAsync(
                serviceBroker,
                context,
                new List<PackageSourceContextInfo>() { PackageSourceContextInfo.Create(packageSource) },
                NuGet.VisualStudio.Internal.Contracts.ItemFilter.All,
                searchService.Object,
                Mock.Of<INuGetPackageFileService>(),
                TestSearchTerm);

            // Act
            await loader.UpdateStateAndReportAsync(searchResult, Mock.Of<IProgress<IItemLoaderState>>(), CancellationToken.None);
            IEnumerable<PackageItemViewModel> viewModels = loader.GetCurrent();

            // Assert
            ImmutableList<KnownOwnerViewModel> knownOwnerViewModels = viewModels.Single().KnownOwnerViewModels;
            knownOwnerViewModels.Should().BeNull();
        }

        [Fact]
        public async Task GetCurrent_DoesNotHaveKnownOwners_DoesNotCreateKnownOwnerViewModelsAsync()
        {
            var version = NuGetVersion.Parse("4.3.0");
            var packageSearchMetadata = new PackageSearchMetadataBuilder.ClonedPackageSearchMetadata()
            {
                Identity = new PackageIdentity("NuGet.Versioning", version),
                OwnersList = new List<string> { "owner1", "owner2" },
            };
            PackageSource packageSource = new PackageSource("https://nuget.test/v3/index.json");

            var packageSearchMetadataContextInfo = PackageSearchMetadataContextInfo.Create(packageSearchMetadata, knownOwners: null);
            var searchResult = new SearchResultContextInfo(new[] { packageSearchMetadataContextInfo }, new Dictionary<string, LoadingStatus> { { "Search", LoadingStatus.Loading } }, hasMoreItems: false);
            var serviceBroker = Mock.Of<IServiceBroker>();
            var testVersions = new List<VersionInfoContextInfo>() {
                new VersionInfoContextInfo(version),
            };

            var searchService = new Mock<INuGetSearchService>(MockBehavior.Strict);
            searchService.Setup(ss => ss.GetPackageVersionsAsync(
                It.IsAny<PackageIdentity>(),
                It.IsAny<IReadOnlyCollection<PackageSourceContextInfo>>(),
                It.IsAny<bool>(),
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(testVersions);
            searchService.Setup(ss => ss.GetPackageMetadataAsync(It.IsAny<PackageIdentity>(), It.IsAny<IReadOnlyCollection<PackageSourceContextInfo>>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((packageSearchMetadataContextInfo, It.IsAny<PackageDeprecationMetadataContextInfo>()));
            searchService.Setup(s => s.SearchAsync(It.IsAny<IReadOnlyCollection<IProjectContextInfo>>(),
                It.IsAny<IReadOnlyCollection<PackageSourceContextInfo>>(),
                It.IsAny<IReadOnlyCollection<string>>(),
                It.IsAny<string>(),
                It.IsAny<SearchFilter>(),
                It.IsAny<NuGet.VisualStudio.Internal.Contracts.ItemFilter>(),
                It.IsAny<bool>(),
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()))
                .Returns(new ValueTask<SearchResultContextInfo>(searchResult));
            searchService.Setup(s => s.RefreshSearchAsync(It.IsAny<CancellationToken>()))
                .Returns(new ValueTask<SearchResultContextInfo>(searchResult));
            var uiContext = new Mock<INuGetUIContext>();
            uiContext.Setup(ui => ui.ServiceBroker).Returns(serviceBroker);
            var context = new PackageLoadContext(isSolution: false, uiContext.Object);
            var mockProgress = Mock.Of<IProgress<IItemLoaderState>>();

            var loader = await PackageItemLoader.CreateAsync(
                serviceBroker,
                context,
                new List<PackageSourceContextInfo>() { PackageSourceContextInfo.Create(packageSource) },
                NuGet.VisualStudio.Internal.Contracts.ItemFilter.All,
                searchService.Object,
                Mock.Of<INuGetPackageFileService>(),
                TestSearchTerm);

            // Act
            await loader.UpdateStateAndReportAsync(searchResult, Mock.Of<IProgress<IItemLoaderState>>(), CancellationToken.None);
            IEnumerable<PackageItemViewModel> viewModels = loader.GetCurrent();

            // Assert
            ImmutableList<KnownOwnerViewModel> knownOwnerViewModels = viewModels.Single().KnownOwnerViewModels;
            knownOwnerViewModels.Should().BeNull();
        }
    }
}
