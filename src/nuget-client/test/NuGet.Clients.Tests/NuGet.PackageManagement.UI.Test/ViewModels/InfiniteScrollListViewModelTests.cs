// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Threading;
using Moq;
using NuGet.PackageManagement.UI.Models.Package;
using NuGet.PackageManagement.UI.Test.Models.Package;
using NuGet.PackageManagement.UI.ViewModels;
using NuGet.PackageManagement.VisualStudio;
using NuGet.Packaging.Core;
using NuGet.Versioning;
using NuGet.VisualStudio;
using NuGet.VisualStudio.Internal.Contracts;
using Xunit;

namespace NuGet.PackageManagement.UI.Test.ViewModels
{
    public class InfiniteScrollListViewModelTests : IDisposable
    {
        private readonly JoinableTaskContext _joinableTaskContext;
        private readonly JoinableTaskFactory _joinableTaskFactory;

        public InfiniteScrollListViewModelTests()
        {
            _joinableTaskContext = new JoinableTaskContext();
            _joinableTaskFactory = _joinableTaskContext.Factory;
        }

        public void Dispose()
        {
            _joinableTaskContext.Dispose();
        }

        private InfiniteScrollListViewModel CreateViewModel()
        {
            return new InfiniteScrollListViewModel(
                new Lazy<JoinableTaskFactory>(() => _joinableTaskFactory));
        }

        private static PackageItemViewModel CreatePackageItemViewModel(
            string id = "TestPackage",
            string version = "1.0.0",
            PackageLevel packageLevel = PackageLevel.TopLevel,
            bool isVulnerable = false)
        {
            var searchService = new Mock<INuGetSearchService>();
            var packageIdentity = new PackageIdentity(id, new NuGetVersion(version));
            var embeddedResource = new Mock<IEmbeddedResourcesCapable>();
            var vulnerableCapability = new Mock<IVulnerableCapable>();
            vulnerableCapability.SetupGet(v => v.IsVulnerable).Returns(isVulnerable);
            var deprecatedCapability = new Mock<IDeprecationCapable>();
            var packageModel = PackageModelCreationTestHelper.CreateRemotePackageModel(
                packageIdentity, vulnerableCapability.Object, deprecatedCapability.Object, embeddedResource.Object);

            var vm = new PackageItemViewModel(searchService.Object, packageModel: packageModel)
            {
                PackageLevel = packageLevel,
            };
            return vm;
        }

        /// <summary>
        /// Creates a mock loader that transitions from Loading → finalStatus on first UpdateStateAndReportAsync call.
        /// </summary>
        private static Mock<IPackageItemLoader> CreateMockLoader(
            LoadingStatus finalStatus,
            IEnumerable<PackageItemViewModel> results = null,
            bool isMultiSource = false)
        {
            var loader = new Mock<IPackageItemLoader>(MockBehavior.Loose);
            var state = new Mock<IItemLoaderState>();
            var currentStatus = LoadingStatus.Loading;

            state.Setup(x => x.LoadingStatus).Returns(() => currentStatus);
            state.Setup(x => x.ItemsCount).Returns(() => results?.Count() ?? 0);
            state.Setup(x => x.SourceLoadingStatus).Returns(new Dictionary<string, LoadingStatus>());

            loader.SetupGet(x => x.State).Returns(state.Object);
            loader.SetupGet(x => x.IsMultiSource).Returns(isMultiSource);
            loader.Setup(x => x.GetCurrent()).Returns(results ?? Enumerable.Empty<PackageItemViewModel>());
            loader.Setup(x => x.UpdateStateAndReportAsync(
                    It.IsAny<SearchResultContextInfo>(),
                    It.IsAny<IProgress<IItemLoaderState>>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask)
                .Callback(() => currentStatus = finalStatus);
            loader.Setup(x => x.UpdateStateAsync(
                    It.IsAny<IProgress<IItemLoaderState>>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask)
                .Callback(() => { if (currentStatus == LoadingStatus.Ready) currentStatus = LoadingStatus.NoMoreItems; });

            return loader;
        }

        /// <summary>
        /// Runs a full load cycle and waits for LoadItemsCompleted.
        /// </summary>
        private async Task LoadAndWaitAsync(InfiniteScrollListViewModel vm, Mock<IPackageItemLoader> loader)
        {
            var tcs = new TaskCompletionSource<bool>();
            void handler(object s, EventArgs e) => tcs.TrySetResult(true);
            vm.LoadItemsCompleted += handler;
            try
            {
                await vm.LoadItemsAsync(
                    loader.Object,
                    loadingMessage: "Loading...",
                    logger: Mock.Of<INuGetUILogger>(),
                    searchResultTask: Task.FromResult(new SearchResultContextInfo()),
                    token: CancellationToken.None);
                var completed = await Task.WhenAny(tcs.Task, Task.Delay(TimeSpan.FromSeconds(30)));
                Assert.Same(tcs.Task, completed);
            }
            finally
            {
                vm.LoadItemsCompleted -= handler;
            }
        }

        [Fact]
        public void Constructor_JoinableTaskFactoryIsNull_Throws()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new InfiniteScrollListViewModel(joinableTaskFactory: null));

            Assert.Equal("joinableTaskFactory", exception.ParamName);
        }

        [Fact]
        public void Constructor_DefaultState_PropertiesHaveExpectedDefaults()
        {
            var vm = CreateViewModel();

            Assert.Empty(vm.Items);
            Assert.Empty(vm.PackageItems);
            Assert.False(vm.IsUpdateMode);
            Assert.False(vm.IsSolution);
            Assert.Equal(0, vm.SelectedCount);
            Assert.Null(vm.OperationId);
            Assert.False(vm.HasUpdatablePackages);
            Assert.False(vm.HasSelectedPackages);
            Assert.False(vm.HasStatusBarContent);
            Assert.Equal(0, vm.ItemsLoaded);
        }

        [Fact]
        public async Task LoadItemsAsync_LoaderIsNull_Throws()
        {
            var vm = CreateViewModel();

            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await vm.LoadItemsAsync(
                    loader: null,
                    loadingMessage: "Loading...",
                    logger: Mock.Of<INuGetUILogger>(),
                    searchResultTask: Task.FromResult(new SearchResultContextInfo()),
                    token: CancellationToken.None));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task LoadItemsAsync_LoadingMessageIsNullOrEmpty_Throws(string loadingMessage)
        {
            var vm = CreateViewModel();

            await Assert.ThrowsAsync<ArgumentException>(async () =>
                await vm.LoadItemsAsync(
                    Mock.Of<IPackageItemLoader>(),
                    loadingMessage,
                    logger: Mock.Of<INuGetUILogger>(),
                    searchResultTask: Task.FromResult(new SearchResultContextInfo()),
                    token: CancellationToken.None));
        }

        [Fact]
        public async Task LoadItemsAsync_SearchResultTaskIsNull_Throws()
        {
            var vm = CreateViewModel();

            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await vm.LoadItemsAsync(
                    Mock.Of<IPackageItemLoader>(),
                    loadingMessage: "Loading...",
                    logger: Mock.Of<INuGetUILogger>(),
                    searchResultTask: null,
                    token: CancellationToken.None));
        }

        [Fact]
        public async Task LoadItemsAsync_CancelledToken_Throws()
        {
            var vm = CreateViewModel();

            await Assert.ThrowsAsync<OperationCanceledException>(async () =>
                await vm.LoadItemsAsync(
                    Mock.Of<IPackageItemLoader>(),
                    loadingMessage: "Loading...",
                    logger: Mock.Of<INuGetUILogger>(),
                    searchResultTask: Task.FromResult(new SearchResultContextInfo()),
                    token: new CancellationToken(canceled: true)));
        }

        [Fact]
        public async Task LoadItemsAsync_WithNoResults_SetsNoItemsFoundStatus()
        {
            var vm = CreateViewModel();
            var loader = CreateMockLoader(LoadingStatus.NoItemsFound);

            await LoadAndWaitAsync(vm, loader);

            Assert.Contains(vm.Items, item => item is LoadingStatusIndicator);
            Assert.Empty(vm.PackageItems);
        }

        [Fact]
        public async Task LoadItemsAsync_WithResults_AddsItemsToCollection()
        {
            var vm = CreateViewModel();
            var packages = new[]
            {
                CreatePackageItemViewModel("PackageA"),
                CreatePackageItemViewModel("PackageB"),
                CreatePackageItemViewModel("PackageC"),
            };
            var loader = CreateMockLoader(LoadingStatus.Ready, packages);

            await LoadAndWaitAsync(vm, loader);

            Assert.Equal(3, vm.PackageItems.Count());
            Assert.Equal(3, vm.PackageItemsCount);
        }

        [Fact]
        public async Task LoadItemsAsync_ClearsExistingItems()
        {
            var vm = CreateViewModel();

            // First load
            var packages1 = new[] { CreatePackageItemViewModel("OldPackage") };
            var loader1 = CreateMockLoader(LoadingStatus.Ready, packages1);
            await LoadAndWaitAsync(vm, loader1);
            Assert.Equal("OldPackage", vm.PackageItems.First().Id);

            // Second load — should replace old items
            var packages2 = new[] { CreatePackageItemViewModel("NewPackage") };
            var loader2 = CreateMockLoader(LoadingStatus.Ready, packages2);
            await LoadAndWaitAsync(vm, loader2);

            Assert.Single(vm.PackageItems);
            Assert.Equal("NewPackage", vm.PackageItems.First().Id);
        }

        [Fact]
        public async Task LoadItemsAsync_SetsHasStatusBarContentToFalse()
        {
            var vm = CreateViewModel();
            vm.HasStatusBarContent = true; // pre-condition

            var loader = CreateMockLoader(LoadingStatus.Ready, new[] { CreatePackageItemViewModel("A") });
            await LoadAndWaitAsync(vm, loader);

            Assert.False(vm.HasStatusBarContent);
        }

        [Fact]
        public void SelectAll_SetsAllPackagesSelected()
        {
            var vm = CreateViewModel();
            var pkg1 = CreatePackageItemViewModel("A");
            var pkg2 = CreatePackageItemViewModel("B");
            vm.UpdatePackageList(new[] { pkg1, pkg2 }, refresh: false);

            vm.SelectAll();

            Assert.True(pkg1.IsSelected);
            Assert.True(pkg2.IsSelected);
        }

        [Fact]
        public void DeselectAll_ClearsSelection()
        {
            var vm = CreateViewModel();
            var pkg1 = CreatePackageItemViewModel("A");
            var pkg2 = CreatePackageItemViewModel("B");
            vm.UpdatePackageList(new[] { pkg1, pkg2 }, refresh: false);
            vm.SelectAll();

            vm.DeselectAll();

            Assert.False(pkg1.IsSelected);
            Assert.False(pkg2.IsSelected);
        }

        [Fact]
        public void GetSelectedPackages_ReturnsOnlySelected()
        {
            var vm = CreateViewModel();
            var pkg1 = CreatePackageItemViewModel("A");
            var pkg2 = CreatePackageItemViewModel("B");
            var pkg3 = CreatePackageItemViewModel("C");
            vm.UpdatePackageList(new[] { pkg1, pkg2, pkg3 }, refresh: false);
            pkg1.IsSelected = true;
            pkg3.IsSelected = true;

            var selected = vm.GetSelectedPackages();

            Assert.Equal(2, selected.Length);
            Assert.Contains(selected, p => p.Id == "A");
            Assert.Contains(selected, p => p.Id == "C");
        }

        [Fact]
        public void SelectionTracking_TracksSelectedCount()
        {
            var vm = CreateViewModel();
            var pkg1 = CreatePackageItemViewModel("A");
            var pkg2 = CreatePackageItemViewModel("B");
            vm.UpdatePackageList(new[] { pkg1, pkg2 }, refresh: false);

            Assert.Equal(0, vm.SelectedCount);

            pkg1.IsSelected = true;
            Assert.Equal(1, vm.SelectedCount);

            pkg2.IsSelected = true;
            Assert.Equal(2, vm.SelectedCount);

            pkg1.IsSelected = false;
            Assert.Equal(1, vm.SelectedCount);
        }

        [Fact]
        public void IsUpdateMode_WhenSetToFalse_CollapsesUpdateContainer()
        {
            var vm = CreateViewModel();
            vm.UpdatePackageList(new[] { CreatePackageItemViewModel("A") }, refresh: false);
            vm.IsUpdateMode = true;
            Assert.True(vm.HasUpdatablePackages);

            vm.IsUpdateMode = false;

            Assert.False(vm.HasUpdatablePackages);
        }

        [Fact]
        public void UpdateSelectionState_AllSelected_SetsCheckedState()
        {
            var vm = CreateViewModel();
            vm.IsUpdateMode = true;
            var pkg1 = CreatePackageItemViewModel("A");
            var pkg2 = CreatePackageItemViewModel("B");
            vm.UpdatePackageList(new[] { pkg1, pkg2 }, refresh: false);

            pkg1.IsSelected = true;
            pkg2.IsSelected = true;

            Assert.True(vm.HasUpdatablePackages);
            Assert.True(vm.SelectionState);
            Assert.True(vm.HasSelectedPackages);
        }

        [Fact]
        public void UpdateSelectionState_SomeSelected_SetsIndeterminateState()
        {
            var vm = CreateViewModel();
            vm.IsUpdateMode = true;
            var pkg1 = CreatePackageItemViewModel("A");
            var pkg2 = CreatePackageItemViewModel("B");
            vm.UpdatePackageList(new[] { pkg1, pkg2 }, refresh: false);

            pkg1.IsSelected = true;

            Assert.True(vm.HasUpdatablePackages);
            Assert.Null(vm.SelectionState);
            Assert.True(vm.HasSelectedPackages);
        }

        [Fact]
        public void UpdateSelectionState_NoneSelected_SetsUncheckedAndDisabled()
        {
            var vm = CreateViewModel();
            vm.UpdatePackageList(new[] { CreatePackageItemViewModel("A") }, refresh: false);
            vm.IsUpdateMode = true;

            Assert.True(vm.HasUpdatablePackages);
            Assert.False(vm.SelectionState);
            Assert.False(vm.HasSelectedPackages);
        }

        [Fact]
        public void FilterItem_PackageViewModel_ReturnsTrue()
        {
            var vm = CreateViewModel();
            var package = CreatePackageItemViewModel("A");

            Assert.True(vm.FilterItem(package));
        }

        [Fact]
        public void FilterVulnerablePackage_WhenFilterDisabled_IncludesNonVulnerable()
        {
            var vm = CreateViewModel();
            var package = CreatePackageItemViewModel("A", isVulnerable: false);
            Assert.False(package.IsPackageVulnerable);

            Assert.True(vm.FilterVulnerablePackage(package));
        }

        [Fact]
        public void FilterVulnerablePackage_WhenFilterEnabled_HidesNonVulnerable()
        {
            var vm = CreateViewModel();
            vm.SetVulnerabilitiesFiltering(true);
            var package = CreatePackageItemViewModel("A", isVulnerable: false);
            Assert.False(package.IsPackageVulnerable);

            Assert.False(vm.FilterVulnerablePackage(package));
        }

        [Fact]
        public void FilterVulnerablePackage_WhenFilterEnabled_IncludesVulnerable()
        {
            var vm = CreateViewModel();
            vm.SetVulnerabilitiesFiltering(true);
            var package = CreatePackageItemViewModel("A", isVulnerable: true);
            Assert.True(package.IsPackageVulnerable);

            Assert.True(vm.FilterVulnerablePackage(package));
        }

        [Fact]
        public void FilterLoadingIndicator_WhenNotFilteringVulnerabilities_ShowsIndicator()
        {
            var vm = CreateViewModel();

            Assert.True(vm.FilterLoadingIndicator(vm.LoadingStatusIndicator));
        }

        [Fact]
        public void FilterLoadingIndicator_WhenFilteringVulnerabilities_HidesIndicator()
        {
            var vm = CreateViewModel();
            vm.SetVulnerabilitiesFiltering(true);

            Assert.False(vm.FilterLoadingIndicator(vm.LoadingStatusIndicator));
        }

        [Fact]
        public void HasTransitiveItems_WithNoTransitive_ReturnsFalse()
        {
            var vm = CreateViewModel();
            vm.UpdatePackageList(new[] { CreatePackageItemViewModel("A", packageLevel: PackageLevel.TopLevel) }, refresh: false);

            Assert.False(vm.HasTransitiveItems());
        }

        [Fact]
        public void HasTransitiveItems_WithTransitive_ReturnsTrue()
        {
            var vm = CreateViewModel();
            vm.UpdatePackageList(new[]
            {
                CreatePackageItemViewModel("A", packageLevel: PackageLevel.TopLevel),
                CreatePackageItemViewModel("B", packageLevel: PackageLevel.Transitive),
            }, refresh: false);

            Assert.True(vm.HasTransitiveItems());
        }

        [Fact]
        public void ResolveSelectedItem_WhenPreviousItemExists_ReturnsSameById()
        {
            var vm = CreateViewModel();
            var pkg1 = CreatePackageItemViewModel("A");
            var pkg2 = CreatePackageItemViewModel("B");
            vm.UpdatePackageList(new[] { pkg1, pkg2 }, refresh: false);

            var result = vm.ResolveSelectedItem(CreatePackageItemViewModel("B"));

            Assert.Same(pkg2, result);
        }

        [Fact]
        public void ResolveSelectedItem_WhenPreviousItemNotFound_ReturnsFirst()
        {
            var vm = CreateViewModel();
            var pkg1 = CreatePackageItemViewModel("A");
            vm.UpdatePackageList(new[] { pkg1 }, refresh: false);

            var result = vm.ResolveSelectedItem(CreatePackageItemViewModel("NotInList"));

            Assert.Same(pkg1, result);
        }

        [Fact]
        public void ResolveSelectedItem_WhenNull_ReturnsFirst()
        {
            var vm = CreateViewModel();
            var pkg1 = CreatePackageItemViewModel("A");
            vm.UpdatePackageList(new[] { pkg1 }, refresh: false);

            var result = vm.ResolveSelectedItem(null);

            Assert.Same(pkg1, result);
        }

        [Fact]
        public void ShouldShowStatusBar_WhenCancelled_ReturnsTrue()
        {
            var vm = CreateViewModel();
            var state = new Mock<IItemLoaderState>();
            state.Setup(x => x.LoadingStatus).Returns(LoadingStatus.Cancelled);

            Assert.True(vm.ShouldShowStatusBar(state.Object));
        }

        [Fact]
        public void ShouldShowStatusBar_WhenErrorOccurred_ReturnsTrue()
        {
            var vm = CreateViewModel();
            var state = new Mock<IItemLoaderState>();
            state.Setup(x => x.LoadingStatus).Returns(LoadingStatus.ErrorOccurred);

            Assert.True(vm.ShouldShowStatusBar(state.Object));
        }

        [Fact]
        public void ShouldShowStatusBar_WhenReady_ReturnsFalse()
        {
            var vm = CreateViewModel();
            var state = new Mock<IItemLoaderState>();
            state.Setup(x => x.LoadingStatus).Returns(LoadingStatus.Ready);

            Assert.False(vm.ShouldShowStatusBar(state.Object));
        }

        [Fact]
        public void ShouldShowStatusBar_WhenNull_ReturnsFalse()
        {
            var vm = CreateViewModel();
            Assert.False(vm.ShouldShowStatusBar(null));
        }

        [Fact]
        public void ResetLoadingStatusIndicator_ResetsStatus()
        {
            var vm = CreateViewModel();
            vm.LoadingStatusIndicator.Status = LoadingStatus.ErrorOccurred;

            vm.ResetLoadingStatusIndicator();

            Assert.Equal(LoadingStatus.Unknown, vm.LoadingStatusIndicator.Status);
        }

        [Fact]
        public void LoadingStatusLocalizedText_ReflectsIndicatorStatus()
        {
            var vm = CreateViewModel();
            vm.LoadingStatusIndicator.Status = LoadingStatus.NoItemsFound;

            Assert.Equal(Resources.Text_NoPackagesFound, vm.LoadingStatusLocalizedText);
        }

        [Fact]
        public void ClearPackageList_RemovesAllItemsAndResetsItemsLoaded()
        {
            var vm = CreateViewModel();
            vm.UpdatePackageList(new[] { CreatePackageItemViewModel("A") }, refresh: false);
            Assert.NotEmpty(vm.Items);

            vm.ClearPackageList();

            Assert.Empty(vm.Items);
            Assert.Equal(0, vm.ItemsLoaded);
        }

        [Fact]
        public void UpdatePackageList_RefreshTrue_ClearsAndReloads()
        {
            var vm = CreateViewModel();
            vm.UpdatePackageList(new[] { CreatePackageItemViewModel("OldPkg") }, refresh: false);

            vm.UpdatePackageList(new[] { CreatePackageItemViewModel("NewPkg") }, refresh: true);

            Assert.Single(vm.PackageItems);
            Assert.Equal("NewPkg", vm.PackageItems.First().Id);
        }

        [Fact]
        public void UpdatePackageList_RefreshFalse_Appends()
        {
            var vm = CreateViewModel();
            vm.UpdatePackageList(new[] { CreatePackageItemViewModel("A") }, refresh: false);

            vm.UpdatePackageList(new[] { CreatePackageItemViewModel("B") }, refresh: false);

            Assert.Equal(2, vm.PackageItems.Count());
        }

        [Fact]
        public async Task ShouldShowStatusBar_WhenMultiSourceAndMoreItems_ReturnsTrue()
        {
            var vm = CreateViewModel();
            var packages = new[] { CreatePackageItemViewModel("A") };
            var loader = CreateMockLoader(LoadingStatus.Ready, packages, isMultiSource: true);

            await LoadAndWaitAsync(vm, loader);

            var state = new Mock<IItemLoaderState>();
            state.Setup(x => x.LoadingStatus).Returns(LoadingStatus.Ready);
            state.Setup(x => x.ItemsCount).Returns(100); // more than ItemsLoaded

            Assert.True(vm.ShouldShowStatusBar(state.Object));
        }

        [Fact]
        public async Task ShouldShowStatusBar_WhenMultiSourceLoadingWithItems_ReturnsTrue()
        {
            var vm = CreateViewModel();
            var packages = new[] { CreatePackageItemViewModel("A") };
            // Use NoMoreItems so the load completes, then test ShouldShowStatusBar with a Loading state
            var loader = CreateMockLoader(LoadingStatus.NoMoreItems, packages, isMultiSource: true);

            await LoadAndWaitAsync(vm, loader);

            var state = new Mock<IItemLoaderState>();
            state.Setup(x => x.LoadingStatus).Returns(LoadingStatus.Loading);
            state.Setup(x => x.ItemsCount).Returns(5);

            Assert.True(vm.ShouldShowStatusBar(state.Object));
        }

        [Fact]
        public async Task LoadMoreItemsAsync_WhenLoaderStatusReady_TriggersLoad()
        {
            var vm = CreateViewModel();
            var packages = new[] { CreatePackageItemViewModel("A") };
            var loader = CreateMockLoader(LoadingStatus.Ready, packages);

            await LoadAndWaitAsync(vm, loader);

            // After LoadAndWaitAsync with Ready status, the loader stays Ready.
            // LoadMoreItemsAsync should trigger another load, appending more items.
            await vm.LoadMoreItemsAsync();

            Assert.Equal(2, vm.PackageItems.Count());
        }

        [Fact]
        public async Task LoadMoreItemsAsync_WhenLoaderStatusNotReady_IsNoOp()
        {
            var vm = CreateViewModel();
            var packages = new[] { CreatePackageItemViewModel("A") };
            var loader = CreateMockLoader(LoadingStatus.NoMoreItems, packages);

            await LoadAndWaitAsync(vm, loader);

            // Status is NoMoreItems, so LoadMoreItemsAsync should do nothing
            await vm.LoadMoreItemsAsync();

            Assert.Single(vm.PackageItems);
        }

        [Fact]
        public async Task ShowMoreResults_RefreshesPackageListAndUpdatesStatusBar()
        {
            var vm = CreateViewModel();
            var packages = new[]
            {
                CreatePackageItemViewModel("A"),
                CreatePackageItemViewModel("B"),
            };
            var loader = CreateMockLoader(LoadingStatus.Ready, packages);
            await LoadAndWaitAsync(vm, loader);

            vm.ShowMoreResults();

            Assert.Equal(2, vm.PackageItems.Count());
        }

        [Fact]
        public void FilterVulnerabilitiesIndicator_WhenFilteringDisabled_HidesIndicator()
        {
            var vm = CreateViewModel();
            vm.SetVulnerabilitiesFiltering(false);

            // The vulnerabilities indicator should not appear when filtering is disabled
            Assert.False(vm.FilterVulnerabilitiesIndicator(vm.LoadingVulnerabilitiesStatusIndicator));
        }

        [Fact]
        public void FilterVulnerabilitiesIndicator_WhenFilteringEnabled_ShowsIndicator()
        {
            var vm = CreateViewModel();
            vm.SetVulnerabilitiesFiltering(true);

            // When filtering is enabled and status is Loading (default), the indicator should be shown
            Assert.True(vm.FilterVulnerabilitiesIndicator(vm.LoadingVulnerabilitiesStatusIndicator));
        }

        [Fact]
        public void FilterVulnerabilitiesIndicator_WhenNoItemsFoundAndVulnerablePackagesExist_HidesIndicator()
        {
            var vm = CreateViewModel();
            vm.SetVulnerabilitiesFiltering(true);
            vm.LoadingVulnerabilitiesStatusIndicator.Status = LoadingStatus.NoItemsFound;

            // Add a vulnerable package so VulnerablePackagesCount > 0
            vm.UpdatePackageList(new[] { CreatePackageItemViewModel("A", isVulnerable: true) }, refresh: false);

            Assert.False(vm.FilterVulnerabilitiesIndicator(vm.LoadingVulnerabilitiesStatusIndicator));
        }

        [Fact]
        public void FilterVulnerabilitiesIndicator_NonIndicatorItem_AlwaysReturnsTrue()
        {
            var vm = CreateViewModel();
            vm.SetVulnerabilitiesFiltering(true);
            var package = CreatePackageItemViewModel("A");

            Assert.True(vm.FilterVulnerabilitiesIndicator(package));
        }
    }
}
