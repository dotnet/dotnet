// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.ServiceHub.Framework;
using Microsoft.ServiceHub.Framework.Services;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Sdk.TestFramework;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Threading;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.Commands;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Packaging.Core;
using NuGet.ProjectManagement;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;
using NuGet.VisualStudio;
using NuGet.VisualStudio.Internal.Contracts;
using Test.Utility;
using Test.Utility.VisualStudio;
using Xunit;
using Task = System.Threading.Tasks.Task;

namespace NuGet.PackageManagement.VisualStudio.Test
{
    [Collection(MockedVS.Collection)]
    public class NuGetPackageSearchServiceTests : MockedVSCollectionTests
    {
        private readonly SourceRepository _sourceRepository;
        private readonly IEnumerable<IPackageReferenceContextInfo> _installedPackages;
        private readonly IEnumerable<ITransitivePackageReferenceContextInfo> _transitivePackages;
        private readonly IReadOnlyCollection<IProjectContextInfo> _projects;
        private readonly Mock<IComponentModel> _componentModel;
        private readonly Mock<IOutputConsoleProvider> _outputConsoleProviderMock;
        private readonly Lazy<IOutputConsoleProvider> _outputConsoleProvider;

        public NuGetPackageSearchServiceTests(GlobalServiceProvider globalServiceProvider)
            : base(globalServiceProvider)
        {
            _installedPackages = new List<IPackageReferenceContextInfo>();
            _transitivePackages = new List<ITransitivePackageReferenceContextInfo>();
            _projects = new List<IProjectContextInfo>
            {
                new ProjectContextInfo(
                    Guid.NewGuid().ToString(),
                    ProjectModel.ProjectStyle.PackageReference,
                    NuGetProjectKind.PackageReference)
            };
            var testFeedUrl = "https://testsource.test/v3/index.json";
            var query = "https://api-v2v3search-0.nuget.org/query";
            var responses = new Dictionary<string, string>
            {
                { testFeedUrl, ProtocolUtility.GetResource("NuGet.PackageManagement.VisualStudio.Test.compiler.resources.index.json", GetType()) },
                { query + "?q=nuget&skip=0&take=26&prerelease=true&semVerLevel=2.0.0", ProtocolUtility.GetResource("NuGet.PackageManagement.VisualStudio.Test.compiler.resources.nugetSearchPage1.json", GetType()) },
                { query + "?q=nuget&skip=25&take=26&prerelease=true&semVerLevel=2.0.0", ProtocolUtility.GetResource("NuGet.PackageManagement.VisualStudio.Test.compiler.resources.nugetSearchPage2.json", GetType()) },
                { query + "?q=&skip=0&take=26&prerelease=true&semVerLevel=2.0.0", ProtocolUtility.GetResource("NuGet.PackageManagement.VisualStudio.Test.compiler.resources.blankSearchPage.json", GetType()) },
                { "https://api.nuget.org/v3/registration3-gz-semver2/nuget.core/index.json", ProtocolUtility.GetResource("NuGet.PackageManagement.VisualStudio.Test.compiler.resources.nugetCoreIndex.json", GetType()) },
                { "https://api.nuget.org/v3/registration3-gz-semver2/microsoft.extensions.logging.abstractions/index.json", ProtocolUtility.GetResource("NuGet.PackageManagement.VisualStudio.Test.compiler.resources.loggingAbstractions.json", GetType()) }
            };
            _componentModel = new Mock<IComponentModel>();
            var mockOutputConsoleUtility = OutputConsoleUtility.GetMock();
            _outputConsoleProviderMock = mockOutputConsoleUtility.mockIOutputConsoleProvider;
            _outputConsoleProvider = new Lazy<IOutputConsoleProvider>(() => _outputConsoleProviderMock.Object);
            var service = new NuGetExperimentationService(Mock.Of<IEnvironmentVariableReader>(), NuGetExperimentationServiceUtility.GetMock(_experimentationFlags), _outputConsoleProvider);

            service.IsExperimentEnabled(ExperimentationConstants.TransitiveDependenciesInPMUI).Should().Be(false);
            _componentModel.Setup(x => x.GetService<INuGetExperimentationService>()).Returns(service);

            globalServiceProvider.AddService(typeof(SComponentModel), _componentModel.Object);

            _sourceRepository = StaticHttpHandler.CreateSource(testFeedUrl, Repository.Provider.GetCoreV3(), responses);
        }

        [Fact(Skip = "https://github.com/NuGet/Home/issues/10451")]
        public async Task GetTotalCountAsync_WithGreaterThanOrEqualToMaxCountResults_ReturnsMaxCount()
        {
            var source1 = new PackageSource("https://dotnet.myget.org/F/nuget-volatile/api/v3/index.json", "NuGetVolatile");
            var source2 = new PackageSource("https://api.nuget.org/v3/index.json", "NuGet.org");
            var sources = new List<PackageSource> { source1, source2 };
            var sourceRepository = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(new[] { source1, source2 });

            var solutionManager = new Mock<IVsSolutionManager>();
            solutionManager.SetupGet(x => x.SolutionDirectory).Returns("z:\\SomeRandomPath");
            var settings = new Mock<ISettings>();
            var deleteOnRestartManager = new Mock<IDeleteOnRestartManager>();

            AddService<IDeleteOnRestartManager>(Task.FromResult<object>(deleteOnRestartManager.Object));
            AddService<IVsSolutionManager>(Task.FromResult<object>(solutionManager.Object));
            AddService<ISettings>(Task.FromResult<object>(settings.Object));
            AddService<ISourceRepositoryProvider>(Task.FromResult<object>(sourceRepository));

            var serviceActivationOptions = default(ServiceActivationOptions);
            var serviceBroker = new Mock<IServiceBroker>();
            var authorizationService = new AuthorizationServiceClient(Mock.Of<IAuthorizationService>());

            var sharedState = new SharedServiceState(sourceRepository);

            var projectManagerService = new Mock<INuGetProjectManagerService>();

            var installedPackages = new List<IPackageReferenceContextInfo>();
            var projects = new List<IProjectContextInfo>
            {
                new ProjectContextInfo(
                    Guid.NewGuid().ToString(),
                    ProjectModel.ProjectStyle.PackageReference,
                    NuGetProjectKind.PackageReference)
            };

            projectManagerService.Setup(x => x.GetInstalledPackagesAsync(
                    It.IsAny<IReadOnlyCollection<string>>(),
                    It.IsAny<CancellationToken>()))
                .Returns(new ValueTask<IReadOnlyCollection<IPackageReferenceContextInfo>>(installedPackages));

#pragma warning disable ISB001 // Dispose of proxies
            serviceBroker.Setup(x => x.GetProxyAsync<INuGetProjectManagerService>(
                    NuGetServices.ProjectManagerService,
                    It.IsAny<ServiceActivationOptions>(),
                    It.IsAny<CancellationToken>()))
                .Returns(new ValueTask<INuGetProjectManagerService>(projectManagerService.Object));
#pragma warning restore ISB001 // Dispose of proxies

            using (var searchService = new NuGetPackageSearchService(
                serviceActivationOptions,
                serviceBroker.Object,
                authorizationService,
                sharedState))
            {
                const int MaxCount = 100;

                int totalCount = await searchService.GetTotalCountAsync(
                    MaxCount,
                    projects,
                    sources.Select(s => PackageSourceContextInfo.Create(s)).ToList(),
                    targetFrameworks: new List<string>() { "net45", "net5.0" },
                    new SearchFilter(includePrerelease: true),
                    NuGet.VisualStudio.Internal.Contracts.ItemFilter.All,
                    false,
                    CancellationToken.None);

                Assert.Equal(MaxCount, totalCount);
            }
        }

        [Fact]
        public async Task GetTotalCountAsync_WithLessThanMaxCountResults_ReturnsResultsCount()
        {
            using (NuGetPackageSearchService searchService = SetupSearchService())
            {
                int totalCount = await searchService.GetTotalCountAsync(
                    maxCount: 100,
                    _projects,
                    new List<PackageSourceContextInfo> { PackageSourceContextInfo.Create(_sourceRepository.PackageSource) },
                    targetFrameworks: new List<string>() { "net45", "net5.0" },
                    new SearchFilter(includePrerelease: true),
                    NuGet.VisualStudio.Internal.Contracts.ItemFilter.All,
                    false,
                    CancellationToken.None);

                Assert.Equal(1, totalCount);
            }
        }

        [Fact]
        public async Task GetAllPackagesAsync_WithValidArguments_ReturnsMatchingPackages()
        {
            using (NuGetPackageSearchService searchService = SetupSearchService())
            {
                IReadOnlyCollection<PackageSearchMetadataContextInfo> allPackages = await searchService.GetAllPackagesAsync(
                    _projects,
                    new List<PackageSourceContextInfo> { PackageSourceContextInfo.Create(_sourceRepository.PackageSource) },
                    targetFrameworks: new List<string>() { "net45", "net5.0" },
                    new SearchFilter(includePrerelease: true),
                    NuGet.VisualStudio.Internal.Contracts.ItemFilter.All,
                    It.IsAny<bool>(),
                    CancellationToken.None);

                Assert.Equal(1, allPackages.Count);
            }
        }

        [Fact]
        public async Task GetPackageMetadataListAsync_WithValidArguments_ReturnsMatchingResults()
        {
            using (NuGetPackageSearchService searchService = SetupSearchService())
            {
                IReadOnlyCollection<PackageSearchMetadataContextInfo> packageMetadataList = await searchService.GetPackageMetadataListAsync(
                    id: "NuGet.Core",
                    new List<PackageSourceContextInfo> { PackageSourceContextInfo.Create(_sourceRepository.PackageSource) },
                    includePrerelease: true,
                    includeUnlisted: true,
                    CancellationToken.None);

                Assert.Equal(57, packageMetadataList.Count);
            }
        }

        [Fact]
        public async Task GetDeprecationMetadataAsync_WhenDeprecationMetadataExists_ReturnsDeprecationMetadata()
        {
            using (NuGetPackageSearchService searchService = SetupSearchService())
            {
                PackageDeprecationMetadataContextInfo deprecationMetadata = await searchService.GetDeprecationMetadataAsync(
                    new PackageIdentity("microsoft.extensions.logging.abstractions", new Versioning.NuGetVersion("5.0.0-rc.2.20475.5")),
                    new List<PackageSourceContextInfo> { PackageSourceContextInfo.Create(_sourceRepository.PackageSource) },
                    includePrerelease: true,
                    CancellationToken.None);

                Assert.NotNull(deprecationMetadata);
                Assert.Equal("This is deprecated.", deprecationMetadata.Message);
                Assert.Equal("Legacy", deprecationMetadata.Reasons.First());
            }
        }

        [Fact]
        public async Task GetPackageVersionsAsync_WhenPackageVersionsExist_ReturnsPackageVersions()
        {
            using (NuGetPackageSearchService searchService = SetupSearchService())
            {
                IReadOnlyCollection<VersionInfoContextInfo> result = await searchService.GetPackageVersionsAsync(
                    new PackageIdentity("microsoft.extensions.logging.abstractions", new Versioning.NuGetVersion("5.0.0-rc.2.20475.5")),
                    new List<PackageSourceContextInfo> { PackageSourceContextInfo.Create(_sourceRepository.PackageSource) },
                    includePrerelease: true,
                    isTransitive: false,
                    CancellationToken.None); ;

                Assert.Equal(60, result.Count);
                Assert.True(result.Last().Version.Version.Equals(new Version("1.0.0.0")));
            }
        }

        [Fact]
        public async Task GetPackageVersionsAsync_WithProjectAndPackageVersionsExist_ReturnsPackageVersionsAsync()
        {
            using (NuGetPackageSearchService searchService = SetupSearchService())
            {
                IReadOnlyCollection<VersionInfoContextInfo> result = await searchService.GetPackageVersionsAsync(
                    new PackageIdentity("microsoft.extensions.logging.abstractions", new Versioning.NuGetVersion("5.0.0-rc.2.20475.5")),
                    new List<PackageSourceContextInfo> { PackageSourceContextInfo.Create(_sourceRepository.PackageSource) },
                    includePrerelease: true,
                    isTransitive: false,
                    _projects,
                    CancellationToken.None); ;

                Assert.Equal(60, result.Count);
                Assert.True(result.Last().Version.Version.Equals(new Version("1.0.0.0")));
            }
        }

        [Fact]
        public async Task GetPackageVersionsAsync_WhenIsTransitiveAndCacheIsNotPopulatedAsync()
        {
            using (NuGetPackageSearchService searchService = SetupSearchService())
            {
                PackageIdentity transitivePackage = new PackageIdentity("microsoft.extensions.logging.abstractions", new Versioning.NuGetVersion("5.0.0-rc.2.20475.5"));
                var packageSources = new List<PackageSourceContextInfo> { PackageSourceContextInfo.Create(_sourceRepository.PackageSource) };
                var metadataProvider = Mock.Of<IPackageMetadataProvider>();

                CacheItemPolicy _cacheItemPolicy = new CacheItemPolicy
                {
                    SlidingExpiration = ObjectCache.NoSlidingExpiration,
                    AbsoluteExpiration = ObjectCache.InfiniteAbsoluteExpiration,
                };

                Mock.Get(metadataProvider)
                    .Setup(m => m.GetPackageMetadataAsync(It.IsAny<PackageIdentity>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                    .Returns(() => Task.FromResult(PackageSearchMetadataBuilder.FromIdentity(new PackageIdentity("microsoft.extensions.logging.abstractions", NuGetVersion.Parse("5.0.0-rc.2.20475.5"))).Build()));

                IPackageSearchMetadata packageMetadata = await metadataProvider.GetPackageMetadataAsync(transitivePackage, true, CancellationToken.None);

                string cacheId = PackageSearchMetadataCacheItem.GetCacheId(transitivePackage.Id, true, packageSources);
                var cacheEntry = new PackageSearchMetadataCacheItem(packageMetadata, metadataProvider);

                NuGetPackageSearchService.PackageSearchMetadataMemoryCache.AddOrGetExisting(cacheId, cacheEntry, _cacheItemPolicy);

                IReadOnlyCollection<VersionInfoContextInfo> result = await searchService.GetPackageVersionsAsync(
                    transitivePackage,
                    packageSources,
                    includePrerelease: true,
                    isTransitive: true,
                    CancellationToken.None);

                Assert.Equal(60, result.Count);
                Assert.True(result.Last().Version.Version.Equals(new Version("1.0.0.0")));
            }
        }

        [Fact]
        public async Task GetPackageVersionsAsync_WithProjectAndIsTransitiveAndCacheIsNotPopulatedAsync()
        {
            using (NuGetPackageSearchService searchService = SetupSearchService())
            {
                PackageIdentity transitivePackage = new PackageIdentity("microsoft.extensions.logging.abstractions", new Versioning.NuGetVersion("5.0.0-rc.2.20475.5"));
                var packageSources = new List<PackageSourceContextInfo> { PackageSourceContextInfo.Create(_sourceRepository.PackageSource) };
                var metadataProvider = Mock.Of<IPackageMetadataProvider>();

                CacheItemPolicy _cacheItemPolicy = new CacheItemPolicy
                {
                    SlidingExpiration = ObjectCache.NoSlidingExpiration,
                    AbsoluteExpiration = ObjectCache.InfiniteAbsoluteExpiration,
                };

                Mock.Get(metadataProvider)
                    .Setup(m => m.GetPackageMetadataAsync(It.IsAny<PackageIdentity>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                    .Returns(() => Task.FromResult(PackageSearchMetadataBuilder.FromIdentity(new PackageIdentity("microsoft.extensions.logging.abstractions", NuGetVersion.Parse("5.0.0-rc.2.20475.5"))).Build()));

                IPackageSearchMetadata packageMetadata = await metadataProvider.GetPackageMetadataAsync(transitivePackage, true, CancellationToken.None);

                string cacheId = PackageSearchMetadataCacheItem.GetCacheId(transitivePackage.Id, true, packageSources);
                var cacheEntry = new PackageSearchMetadataCacheItem(packageMetadata, metadataProvider);

                NuGetPackageSearchService.PackageSearchMetadataMemoryCache.AddOrGetExisting(cacheId, cacheEntry, _cacheItemPolicy);

                IReadOnlyCollection<VersionInfoContextInfo> result = await searchService.GetPackageVersionsAsync(
                    transitivePackage,
                    packageSources,
                    includePrerelease: true,
                    isTransitive: true,
                    _projects,
                    CancellationToken.None);

                Assert.Equal(60, result.Count);
                Assert.True(result.Last().Version.Version.Equals(new Version("1.0.0.0")));
            }
        }

        [Fact]
        public async Task ContinueSearchAsync_WhenSearchIsContinuable_Continues()
        {
            var telemetryService = new Mock<INuGetTelemetryService>();
            var eventsQueue = new ConcurrentQueue<TelemetryEvent>();
            telemetryService
                .Setup(x => x.EmitTelemetryEvent(It.IsAny<TelemetryEvent>()))
                .Callback<TelemetryEvent>(e => eventsQueue.Enqueue(e));

            TelemetryActivity.NuGetTelemetryService = telemetryService.Object;

            using (NuGetPackageSearchService searchService = SetupSearchService())
            {
                SearchResultContextInfo searchResult = await searchService.SearchAsync(
                    _projects,
                    new List<PackageSourceContextInfo> { PackageSourceContextInfo.Create(_sourceRepository.PackageSource) },
                    targetFrameworks: new List<string>() { "net45", "net5.0" },
                    searchText: "nuget",
                    new SearchFilter(includePrerelease: true),
                    NuGet.VisualStudio.Internal.Contracts.ItemFilter.All,
                    isSolution: false,
                    useRecommender: false,
                    CancellationToken.None);
                SearchResultContextInfo continueSearchResult = await searchService.ContinueSearchAsync(CancellationToken.None);

                Assert.True(searchResult.PackageSearchItems.First().Title.Equals("NuGet.Core1", StringComparison.OrdinalIgnoreCase));
                Assert.True(continueSearchResult.PackageSearchItems.First().Title.Equals("NuGet.Core27", StringComparison.OrdinalIgnoreCase));

                TelemetryEvent[] events = eventsQueue.ToArray();
                Assert.True(4 == events.Length, string.Join(Environment.NewLine, events.Select(e => e.Name)));

                TelemetryEvent search = Assert.Single(events, e => e.Name == "Search");
                Assert.Equal(true, search["IncludePrerelease"]);
                Assert.Equal("nuget", search.GetPiiData().First(p => p.Key == "Query").Value);
                string operationId = Assert.IsType<string>(search["OperationId"]);
                Guid parsedOperationId = Guid.ParseExact(operationId, "D");

                TelemetryEvent sources = Assert.Single(events, e => e.Name == "SearchPackageSourceSummary");
                Assert.Equal(1, sources["NumHTTPv3Feeds"]);
                Assert.Equal("NotPresent", sources["NuGetOrg"]);
                Assert.Equal(operationId, sources["ParentId"]);

                TelemetryEvent page0 = Assert.Single(events, e => e.Name == "SearchPage" && e["PageIndex"] is int && (int)e["PageIndex"] == 0);
                Assert.Equal("Ready", page0["LoadingStatus"]);
                Assert.Equal(operationId, page0["ParentId"]);
                Assert.IsType<int>(page0["ResultCount"]);
                Assert.IsType<double>(page0["Duration"]);
                Assert.IsType<double>(page0["ResultsAggregationDuration"]);
                Assert.IsType<string>(page0["IndividualSourceDurations"]);
                Assert.Equal(1, ((JArray)JsonConvert.DeserializeObject((string)page0["IndividualSourceDurations"])).Values<double>().Count());

                TelemetryEvent page1 = Assert.Single(events, e => e.Name == "SearchPage" && e["PageIndex"] is int && (int)e["PageIndex"] == 1);
                Assert.Equal("Ready", page1["LoadingStatus"]);
                Assert.Equal(operationId, page1["ParentId"]);
                Assert.IsType<int>(page1["ResultCount"]);
                Assert.IsType<double>(page1["Duration"]);
                Assert.IsType<double>(page1["ResultsAggregationDuration"]);
                Assert.IsType<string>(page1["IndividualSourceDurations"]);
                Assert.Equal(1, ((JArray)JsonConvert.DeserializeObject((string)page1["IndividualSourceDurations"])).Values<double>().Count());
            }
        }

        [Theory]
        [InlineData(false, ItemFilter.All, true, typeof(MultiSourcePackageFeed))]
        [InlineData(false, ItemFilter.All, false, typeof(MultiSourcePackageFeed))]
        [InlineData(false, ItemFilter.Installed, true, typeof(InstalledPackageFeed))]
        [InlineData(false, ItemFilter.Installed, false, typeof(InstalledPackageFeed))]
        [InlineData(false, ItemFilter.UpdatesAvailable, true, typeof(UpdatePackageFeed))]
        [InlineData(false, ItemFilter.UpdatesAvailable, false, typeof(UpdatePackageFeed))]
        [InlineData(false, ItemFilter.Consolidate, true, typeof(ConsolidatePackageFeed))]
        [InlineData(false, ItemFilter.Consolidate, false, typeof(ConsolidatePackageFeed))]
        [InlineData(true, ItemFilter.All, true, typeof(MultiSourcePackageFeed))]
        [InlineData(true, ItemFilter.All, false, typeof(MultiSourcePackageFeed))]
        [InlineData(true, ItemFilter.Installed, true, typeof(InstalledPackageFeed))]
        [InlineData(true, ItemFilter.Installed, false, typeof(InstalledAndTransitivePackageFeed))] // Only when transitive experiment is enabled, show Transitive Dependencies in Installed Tab
        [InlineData(true, ItemFilter.UpdatesAvailable, true, typeof(UpdatePackageFeed))]
        [InlineData(true, ItemFilter.UpdatesAvailable, false, typeof(UpdatePackageFeed))]
        [InlineData(true, ItemFilter.Consolidate, true, typeof(ConsolidatePackageFeed))]
        [InlineData(true, ItemFilter.Consolidate, false, typeof(ConsolidatePackageFeed))]
        public async Task CreatePackageFeedAsync_WithTransitiveOriginsExpFlag_OnlyInstalledFeedOnSolutionViewAsync(bool transitiveDependenciesExperimentEnabled, ItemFilter itemFilter, bool isSolution, Type expectedFeedType)
        {
            // Arrange
            // Recreate async lazy on each test
            _experimentationFlags[ExperimentationConstants.TransitiveDependenciesInPMUI.FlightFlag] = transitiveDependenciesExperimentEnabled;
            ExperimentUtility.ResetAsyncValues();

            using NuGetPackageSearchService searchService = SetupSearchService();
            bool expValue = await ExperimentUtility.IsTransitiveOriginExpEnabled.GetValueAsync();
            Assert.Equal(transitiveDependenciesExperimentEnabled, expValue);

            // Act
            (IPackageFeed main, IPackageFeed recommender) = await searchService.CreatePackageFeedAsync(
                projectContextInfos: _projects,
                targetFrameworks: new List<string>() { "net45" },
                itemFilter: itemFilter,
                isSolution: isSolution,
                recommendPackages: false,
                sourceRepositories: new List<SourceRepository>() { _sourceRepository },
                cancellationToken: CancellationToken.None);

            // Assert
            Assert.IsType(expectedFeedType, main);
            Assert.Null(recommender);
        }

        [Fact]
        public async Task CreatePackageFeedAsync_ProjectPMUIInstalledTab_EmitsCounterfactualTelemetryAsync()
        {
            // Arrange
            var telemetrySession = new Mock<ITelemetrySession>();
            var telemetryEvents = new ConcurrentQueue<TelemetryEvent>();
            telemetrySession
                .Setup(x => x.PostEvent(It.IsAny<TelemetryEvent>()))
                .Callback<TelemetryEvent>(x => telemetryEvents.Enqueue(x));
            TelemetryActivity.NuGetTelemetryService = new NuGetVSTelemetryService(telemetrySession.Object);

            using NuGetPackageSearchService searchService = SetupSearchService();
            CounterfactualLoggers.PMUITransitiveDependencies.Reset();

            // Act
            _ = await searchService.CreatePackageFeedAsync(
                projectContextInfos: _projects,
                targetFrameworks: new List<string>() { "net45" },
                itemFilter: ItemFilter.Installed,
                isSolution: false,
                recommendPackages: It.IsAny<bool>(),
                sourceRepositories: new List<SourceRepository>() { _sourceRepository },
                cancellationToken: CancellationToken.None);

            // Assert
            Assert.Contains(telemetryEvents, evt => evt.Name == CounterfactualLoggers.PMUITransitiveDependencies.EventName);
        }

        [Theory] // Installed tab and and project PM UI emits counterfactual, proved in test above
        [InlineData(ItemFilter.All, true)]
        [InlineData(ItemFilter.Installed, true)]
        [InlineData(ItemFilter.UpdatesAvailable, true)]
        [InlineData(ItemFilter.Consolidate, true)]
        [InlineData(ItemFilter.All, false)]
        [InlineData(ItemFilter.UpdatesAvailable, false)]
        [InlineData(ItemFilter.Consolidate, false)]
        public async Task CreatePackageFeedAsync_NotInProjectPMUIInstalledTab_DoesNotEmitCounterfactualTelemetryAsync(ItemFilter itemFilter, bool isSolution)
        {
            // Arrange
            var telemetrySession = new Mock<ITelemetrySession>();
            var telemetryEvents = new ConcurrentQueue<TelemetryEvent>();
            telemetrySession
                .Setup(x => x.PostEvent(It.IsAny<TelemetryEvent>()))
                .Callback<TelemetryEvent>(x => telemetryEvents.Enqueue(x));
            TelemetryActivity.NuGetTelemetryService = new NuGetVSTelemetryService(telemetrySession.Object);

            using NuGetPackageSearchService searchService = SetupSearchService();
            CounterfactualLoggers.PMUITransitiveDependencies.Reset();

            // Act
            _ = await searchService.CreatePackageFeedAsync(
                projectContextInfos: _projects,
                targetFrameworks: new List<string>() { "net45" },
                itemFilter: itemFilter,
                isSolution: isSolution,
                recommendPackages: It.IsAny<bool>(),
                sourceRepositories: new List<SourceRepository>() { _sourceRepository },
                cancellationToken: CancellationToken.None);

            // Assert
            Assert.DoesNotContain(telemetryEvents, evt => evt.Name == CounterfactualLoggers.PMUITransitiveDependencies.EventName);
        }

        private NuGetPackageSearchService SetupSearchService()
        {
            ClearSearchCache();

            var packageSourceProvider = new Mock<IPackageSourceProvider>();
            packageSourceProvider.Setup(x => x.LoadPackageSources()).Returns(new List<PackageSource> { _sourceRepository.PackageSource });
            var sourceRepositoryProvider = new Mock<ISourceRepositoryProvider>();
            sourceRepositoryProvider.Setup(x => x.CreateRepository(It.IsAny<PackageSource>())).Returns(_sourceRepository);
            sourceRepositoryProvider.Setup(x => x.CreateRepository(It.IsAny<PackageSource>(), It.IsAny<FeedType>())).Returns(_sourceRepository);
            sourceRepositoryProvider.SetupGet(x => x.PackageSourceProvider).Returns(packageSourceProvider.Object);
            var solutionManager = new Mock<IVsSolutionManager>();
            solutionManager.SetupGet(x => x.SolutionDirectory).Returns("z:\\SomeRandomPath");
            var settings = new Mock<ISettings>();
            var deleteOnRestartManager = new Mock<IDeleteOnRestartManager>();

            _componentModel.Setup(x => x.GetService<IDeleteOnRestartManager>()).Returns(deleteOnRestartManager.Object);
            _componentModel.Setup(x => x.GetService<IVsSolutionManager>()).Returns(solutionManager.Object);
            _componentModel.Setup(x => x.GetService<ISolutionManager>()).Returns(solutionManager.Object);
            _componentModel.Setup(x => x.GetService<ISettings>()).Returns(settings.Object);
            _componentModel.Setup(x => x.GetService<ISourceRepositoryProvider>()).Returns(sourceRepositoryProvider.Object);
            _componentModel.Setup(x => x.GetService<INuGetProjectContext>()).Returns(new Mock<INuGetProjectContext>().Object);
            _componentModel.Setup(x => x.GetService<IRestoreProgressReporter>()).Returns(new Mock<IRestoreProgressReporter>().Object);

            var service = Package.GetGlobalService(typeof(SAsyncServiceProvider)) as IAsyncServiceProvider;
            ServiceLocator.InitializePackageServiceProvider(service);

            var serviceActivationOptions = default(ServiceActivationOptions);
            var serviceBroker = new Mock<IServiceBroker>();
            var authorizationService = new AuthorizationServiceClient(Mock.Of<IAuthorizationService>());

            var sharedState = new SharedServiceState(sourceRepositoryProvider.Object);

            var projectManagerService = new Mock<INuGetProjectManagerService>();

            projectManagerService.Setup(x => x.GetInstalledPackagesAsync(
                    It.IsAny<IReadOnlyCollection<string>>(),
                    It.IsAny<CancellationToken>()))
                .Returns(new ValueTask<IReadOnlyCollection<IPackageReferenceContextInfo>>(_installedPackages.ToList()));
            projectManagerService.Setup(x => x.GetInstalledAndTransitivePackagesAsync(
                    It.IsAny<IReadOnlyCollection<string>>(),
                    It.IsAny<CancellationToken>()))
                .Returns(new ValueTask<IInstalledAndTransitivePackages>(new InstalledAndTransitivePackages(_installedPackages.ToList(), _transitivePackages.ToList())));
            projectManagerService.Setup(x => x.GetInstalledAndTransitivePackagesAsync(
                    It.IsAny<IReadOnlyCollection<string>>(),
                    It.IsAny<bool>(),
                    It.IsAny<CancellationToken>()))
                .Returns(new ValueTask<IInstalledAndTransitivePackages>(new InstalledAndTransitivePackages(_installedPackages.ToList(), _transitivePackages.ToList())));
            projectManagerService.Setup(x => x.GetPackageFoldersAsync(
                    It.IsAny<IReadOnlyCollection<string>>(),
                    It.IsAny<CancellationToken>()))
                .Returns(new ValueTask<IReadOnlyCollection<string>>(Array.Empty<string>()));

#pragma warning disable ISB001 // Dispose of proxies
            serviceBroker.Setup(x => x.GetProxyAsync<INuGetProjectManagerService>(
                    NuGetServices.ProjectManagerService,
                    It.IsAny<ServiceActivationOptions>(),
                    It.IsAny<CancellationToken>()))
                .Returns(new ValueTask<INuGetProjectManagerService>(projectManagerService.Object));
#pragma warning restore ISB001 // Dispose of proxies

            return new NuGetPackageSearchService(serviceActivationOptions, serviceBroker.Object, authorizationService, sharedState);
        }

        private static void ClearSearchCache()
        {
            List<string> searchCacheKeys = NuGetPackageSearchService.PackageSearchMetadataMemoryCache.Select(kvp => kvp.Key).ToList();
            foreach (string cacheKey in searchCacheKeys)
            {
                NuGetPackageSearchService.PackageSearchMetadataMemoryCache.Remove(cacheKey);
            }
        }
    }
}
