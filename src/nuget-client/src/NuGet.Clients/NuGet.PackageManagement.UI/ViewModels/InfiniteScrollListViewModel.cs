// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Threading;
using NuGet.Common;
using NuGet.PackageManagement.VisualStudio;
using NuGet.VisualStudio;
using NuGet.VisualStudio.Internal.Contracts;
using NuGet.VisualStudio.Telemetry;
using Resx = NuGet.PackageManagement.UI;
using Task = System.Threading.Tasks.Task;

namespace NuGet.PackageManagement.UI.ViewModels
{
    /// <summary>
    /// ViewModel for <see cref="InfiniteScrollList"/>, containing all business logic
    /// for loading, paging, filtering, selection, and collection management.
    /// This class has no references to WPF types and is fully unit-testable.
    /// </summary>
    public class InfiniteScrollListViewModel : ViewModelBase, IDisposable
    {
        private readonly LoadingStatusIndicator _loadingStatusIndicator = new LoadingStatusIndicator();
        private readonly LoadingStatusIndicator _loadingVulnerabilitiesStatusIndicator = new LoadingStatusIndicator();
        private static readonly TimeSpan PollingDelay = TimeSpan.FromMilliseconds(100);

        private CancellationTokenSource _loadCts;
        private IPackageItemLoader _loader;
        private INuGetUILogger _logger;
        private Task<SearchResultContextInfo> _initialSearchResultTask;
        private readonly Lazy<JoinableTaskFactory> _joinableTaskFactory;

        private bool _isUpdateMode;
        private bool _isSolution;
        private bool _filterByVulnerabilities;
        private int _selectedCount;

        // These delegates are set by the codebehind to perform UI-specific operations.
        // They are null in unit tests, making all UI operations no-ops.
        internal Action<IItemLoaderState> StatusBarUpdateLoadingState { get; set; }
        internal Action StatusBarSetCancelled { get; set; }
        internal Action StatusBarSetError { get; set; }
        internal Action<string, bool> StatusBarReset { get; set; }
        internal Action<string> LogActivityError { get; set; }

        /// <summary>
        /// Fires when items in the list have finished loading.
        /// </summary>
        internal event EventHandler LoadItemsCompleted;

        internal ReentrantSemaphore ItemsLock { get; private set; }

        internal ObservableCollection<object> Items { get; } = new ObservableCollection<object>();

        internal InfiniteScrollListViewModel(Lazy<JoinableTaskFactory> joinableTaskFactory)
        {
            if (joinableTaskFactory == null)
            {
                throw new ArgumentNullException(nameof(joinableTaskFactory));
            }

            _joinableTaskFactory = joinableTaskFactory;

            ItemsLock = ReentrantSemaphore.Create(
                initialCount: 1,
                joinableTaskContext: _joinableTaskFactory.Value.Context,
                mode: ReentrantSemaphore.ReentrancyMode.Stack);

            _loadingStatusIndicator.PropertyChanged += LoadingStatusIndicator_PropertyChanged;
        }

        public bool IsUpdateMode
        {
            get => _isUpdateMode;
            set
            {
                if (SetAndRaisePropertyChanged(ref _isUpdateMode, value))
                {
                    UpdateSelectionState();
                }
            }
        }

        public bool IsSolution
        {
            get => _isSolution;
            set => SetAndRaisePropertyChanged(ref _isSolution, value);
        }

        /// <summary>
        /// All loaded items (excluding loading indicators) regardless of filtering.
        /// </summary>
        public IEnumerable<PackageItemViewModel> PackageItems => Items.OfType<PackageItemViewModel>().ToArray();

        /// <summary>
        /// Count of package items (excluding loading indicators).
        /// </summary>
        public int PackageItemsCount => Items.OfType<PackageItemViewModel>().Count();

        public int VulnerablePackagesCount => Items.OfType<PackageItemViewModel>().Count(i => i.IsPackageVulnerable);

        public Guid? OperationId => _loader?.State.OperationId;

        public int SelectedCount => Interlocked.CompareExchange(ref _selectedCount, 0, 0);

        internal LoadingStatusIndicator LoadingStatusIndicator => _loadingStatusIndicator;

        internal LoadingStatusIndicator LoadingVulnerabilitiesStatusIndicator => _loadingVulnerabilitiesStatusIndicator;

        internal IPackageItemLoader Loader => _loader;

        private bool _hasUpdatablePackages;
        private bool? _selectionState;
        private bool _hasSelectedPackages;

        public bool HasUpdatablePackages
        {
            get => _hasUpdatablePackages;
            private set => SetAndRaisePropertyChanged(ref _hasUpdatablePackages, value);
        }

        public bool? SelectionState
        {
            get => _selectionState;
            private set => SetAndRaisePropertyChanged(ref _selectionState, value);
        }

        public bool HasSelectedPackages
        {
            get => _hasSelectedPackages;
            private set => SetAndRaisePropertyChanged(ref _hasSelectedPackages, value);
        }

        private int _itemsLoaded;
        private bool _hasStatusBarContent;

        public int ItemsLoaded
        {
            get => _itemsLoaded;
            private set => SetAndRaisePropertyChanged(ref _itemsLoaded, value);
        }

        public bool HasStatusBarContent
        {
            get => _hasStatusBarContent;
            internal set => SetAndRaisePropertyChanged(ref _hasStatusBarContent, value);
        }

        /// <summary>
        /// Load items using the specified loader. This is the main entry point for starting a new search/load.
        /// </summary>
        internal async Task LoadItemsAsync(
            IPackageItemLoader loader,
            string loadingMessage,
            INuGetUILogger logger,
            Task<SearchResultContextInfo> searchResultTask,
            CancellationToken token)
        {
            if (loader == null)
            {
                throw new ArgumentNullException(nameof(loader));
            }

            if (string.IsNullOrEmpty(loadingMessage))
            {
                throw new ArgumentException(Strings.Argument_Cannot_Be_Null_Or_Empty, nameof(loadingMessage));
            }

            if (searchResultTask == null)
            {
                throw new ArgumentNullException(nameof(searchResultTask));
            }

            token.ThrowIfCancellationRequested();

            _loader = loader;
            _logger = logger;
            _initialSearchResultTask = searchResultTask;
            _loadingStatusIndicator.Reset(loadingMessage);
            _loadingVulnerabilitiesStatusIndicator.Reset(string.Format(CultureInfo.CurrentCulture, Resx.Resources.Vulnerabilities_Loading));
            _loadingVulnerabilitiesStatusIndicator.Status = LoadingStatus.Loading;

            HasStatusBarContent = false;
            StatusBarReset?.Invoke(loadingMessage, loader.IsMultiSource);

            var selectedPackageItem = SelectedPackageItem;

            await ItemsLock.ExecuteAsync(() =>
            {
                ClearPackageList();
                return Task.CompletedTask;
            }, token);

            Interlocked.Exchange(ref _selectedCount, 0);

            await LoadItemsAsync(selectedPackageItem, token);
        }

        private PackageItemViewModel _selectedPackageItem;

        public PackageItemViewModel SelectedPackageItem
        {
            get => _selectedPackageItem;
            set => SetAndRaisePropertyChanged(ref _selectedPackageItem, value);
        }

        /// <summary>
        /// Keep the previously selected package after a search.
        /// Otherwise, select the first on the search if none was selected before.
        /// </summary>
        internal PackageItemViewModel ResolveSelectedItem(PackageItemViewModel selectedItem)
        {
            if (selectedItem != null)
            {
                selectedItem = PackageItems
                    .FirstOrDefault(item => item.Id.Equals(selectedItem.Id, StringComparison.OrdinalIgnoreCase));
            }

            return selectedItem ?? PackageItems.FirstOrDefault();
        }

        private async Task LoadItemsAsync(PackageItemViewModel selectedPackageItem, CancellationToken token)
        {
            var loadCts = CancellationTokenSource.CreateLinkedTokenSource(token);
            var oldCts = Interlocked.Exchange(ref _loadCts, loadCts);
            oldCts?.Cancel();
            oldCts?.Dispose();

            await RepopulatePackageListAsync(selectedPackageItem, _loader, loadCts);
        }

        private async Task RepopulatePackageListAsync(PackageItemViewModel selectedPackageItem, IPackageItemLoader currentLoader, CancellationTokenSource loadCts)
        {
            await TaskScheduler.Default;

            var addedLoadingIndicator = false;

            try
            {
                if (!Items.Contains(_loadingStatusIndicator))
                {
                    Items.Add(_loadingStatusIndicator);
                    addedLoadingIndicator = true;
                }

                if (!Items.Contains(_loadingVulnerabilitiesStatusIndicator))
                {
                    Items.Add(_loadingVulnerabilitiesStatusIndicator);
                }

                await LoadItemsCoreAsync(currentLoader, loadCts.Token);

                await _joinableTaskFactory.Value.SwitchToMainThreadAsync();

                if (selectedPackageItem != null)
                {
                    SelectedPackageItem = ResolveSelectedItem(selectedPackageItem);
                }
            }
            catch (OperationCanceledException) when (!loadCts.IsCancellationRequested)
            {
                loadCts.Cancel();
                loadCts.Dispose();
                currentLoader.Reset();

                await _joinableTaskFactory.Value.SwitchToMainThreadAsync();

                _logger?.Log(new LogMessage(LogLevel.Error, Resx.Resources.Text_UserCanceled));

                _loadingStatusIndicator.SetError(Resx.Resources.Text_UserCanceled);

                StatusBarSetCancelled?.Invoke();
                HasStatusBarContent = true;
            }
            catch (Exception ex) when (!loadCts.IsCancellationRequested)
            {
                loadCts.Cancel();
                loadCts.Dispose();
                currentLoader.Reset();

                LogActivityError?.Invoke(ex.ToString());

                await _joinableTaskFactory.Value.SwitchToMainThreadAsync();

                var errorMessage = ExceptionUtilities.DisplayMessage(ex);
                _logger?.Log(new LogMessage(LogLevel.Error, errorMessage));

                _loadingStatusIndicator.SetError(errorMessage);

                StatusBarSetError?.Invoke();
                HasStatusBarContent = true;
            }
            finally
            {
                if (VulnerablePackagesCount == 0)
                {
                    _loadingVulnerabilitiesStatusIndicator.Status = LoadingStatus.NoItemsFound;
                }
                else
                {
                    Items.Remove(_loadingVulnerabilitiesStatusIndicator);
                }

                if (_loadingStatusIndicator.Status != LoadingStatus.NoItemsFound
                    && _loadingStatusIndicator.Status != LoadingStatus.ErrorOccurred)
                {
                    var emptyListCount = addedLoadingIndicator ? 1 : 0;
                    if (Items.Count == emptyListCount)
                    {
                        _loadingStatusIndicator.Status = LoadingStatus.NoItemsFound;
                    }
                    else
                    {
                        Items.Remove(_loadingStatusIndicator);
                    }
                }
            }

            UpdateSelectionState();

            LoadItemsCompleted?.Invoke(this, EventArgs.Empty);
        }

        private async Task LoadItemsCoreAsync(IPackageItemLoader currentLoader, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            var loadedItems = await LoadNextPageAsync(currentLoader, token);
            token.ThrowIfCancellationRequested();

            if (currentLoader == _loader)
            {
                UpdatePackageList(loadedItems, refresh: false);
            }

            token.ThrowIfCancellationRequested();

            await _joinableTaskFactory.Value.RunAsync(async () =>
            {
                await _joinableTaskFactory.Value.SwitchToMainThreadAsync();
                ItemsLoaded = currentLoader.State.ItemsCount;
            });

            token.ThrowIfCancellationRequested();

            await WaitForCompletionAsync(currentLoader, token);

            token.ThrowIfCancellationRequested();

            if (currentLoader == _loader
                && !loadedItems.Any()
                && currentLoader.State.LoadingStatus == LoadingStatus.Ready)
            {
                UpdatePackageList(currentLoader.GetCurrent(), refresh: false);
            }

            token.ThrowIfCancellationRequested();
        }

        private async Task<IEnumerable<PackageItemViewModel>> LoadNextPageAsync(IPackageItemLoader currentLoader, CancellationToken token)
        {
            var progress = new Progress<IItemLoaderState>(
                s => HandleItemLoaderStateChange(currentLoader, s));

            if (_initialSearchResultTask != null)
            {
                token.ThrowIfCancellationRequested();

                await currentLoader.UpdateStateAndReportAsync(new SearchResultContextInfo(), progress, token);

                var results = await _initialSearchResultTask;

                token.ThrowIfCancellationRequested();

                await currentLoader.UpdateStateAndReportAsync(results, progress, token);

                _initialSearchResultTask = null;
            }
            else
            {
                await currentLoader.LoadNextAsync(progress, token);
            }

            await WaitForInitialResultsAsync(currentLoader, progress, token);

            return currentLoader.GetCurrent();
        }

        private async Task WaitForCompletionAsync(IItemLoader<PackageItemViewModel> currentLoader, CancellationToken token)
        {
            var progress = new Progress<IItemLoaderState>(
                s => HandleItemLoaderStateChange(currentLoader, s));

            while (currentLoader.State.LoadingStatus == LoadingStatus.Loading)
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(PollingDelay, token);
                await currentLoader.UpdateStateAsync(progress, token);
            }
        }

        private async Task WaitForInitialResultsAsync(
            IItemLoader<PackageItemViewModel> currentLoader,
            IProgress<IItemLoaderState> progress,
            CancellationToken token)
        {
            while (currentLoader.State.LoadingStatus == LoadingStatus.Loading &&
                currentLoader.State.ItemsCount == 0)
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(PollingDelay, token);
                await currentLoader.UpdateStateAsync(progress, token);
            }
        }

        /// <summary>
        /// Called when the scroll position indicates more items should be loaded.
        /// </summary>
        internal async Task LoadMoreItemsAsync()
        {
            if (_loader?.State.LoadingStatus == LoadingStatus.Ready)
            {
                await LoadItemsAsync(selectedPackageItem: null, token: CancellationToken.None);
            }
        }

        private void HandleItemLoaderStateChange(IItemLoader<PackageItemViewModel> loader, IItemLoaderState state)
        {
            _joinableTaskFactory.Value.RunAsync(async () =>
            {
                if (loader == _loader)
                {
                    await _joinableTaskFactory.Value.SwitchToMainThreadAsync();

                    StatusBarUpdateLoadingState?.Invoke(state);

                    var shouldShow = ShouldShowStatusBar(state);

                    if (shouldShow)
                    {
                        HasStatusBarContent = true;
                    }

                    _loadingStatusIndicator.Status = state.LoadingStatus;

                    if (!Items.Contains(_loadingStatusIndicator))
                    {
                        await ItemsLock.ExecuteAsync(() =>
                        {
                            Items.Add(_loadingStatusIndicator);
                            return Task.CompletedTask;
                        });
                    }
                }
            }).PostOnFailure(nameof(InfiniteScrollListViewModel), nameof(HandleItemLoaderStateChange));
        }

        private void LoadingStatusIndicator_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(LoadingStatusIndicator.Status))
            {
                RaisePropertyChanged(nameof(LoadingStatusLocalizedText));
            }
        }

        /// <summary>
        /// The localized status text for accessibility/narrator use.
        /// </summary>
        public string LoadingStatusLocalizedText => _loadingStatusIndicator.LocalizedStatus;

        /// <summary>
        /// Appends packages to the internal Items list.
        /// </summary>
        internal void UpdatePackageList(IEnumerable<PackageItemViewModel> packages, bool refresh)
        {
            _joinableTaskFactory.Value.Run(async () =>
            {
                await ItemsLock.ExecuteAsync(() =>
                {
                    bool removed = Items.Remove(_loadingStatusIndicator);

                    if (refresh)
                    {
                        ClearPackageList();
                    }

                    foreach (var package in packages)
                    {
                        package.PropertyChanged += Package_PropertyChanged;
                        Items.Add(package);
                        if (package.IsSelected)
                        {
                            Interlocked.Increment(ref _selectedCount);
                        }
                    }

                    if (removed)
                    {
                        Items.Add(_loadingStatusIndicator);
                    }

                    return Task.CompletedTask;
                });
            });
        }

        /// <summary>
        /// Clears the Items list and removes event handlers for each element.
        /// </summary>
        internal void ClearPackageList()
        {
            foreach (var package in PackageItems)
            {
                package.PropertyChanged -= Package_PropertyChanged;
                package.Dispose();
            }

            Items.Clear();
            ItemsLoaded = 0;
        }

        public async Task UpdatePackageStatusAsync(PackageCollectionItem[] installedPackages, CancellationToken cancellationToken, bool clearCache = false)
        {
            foreach (var package in PackageItems)
            {
                if (package.PackageLevel == PackageLevel.TopLevel)
                {
                    await package.UpdatePackageStatusAsync(installedPackages, cancellationToken, clearCache);
                }
                else
                {
                    await package.UpdateTransitivePackageStatusAsync(cancellationToken);
                }
            }
        }

        /// <summary>
        /// Handles "Show more results" click by refreshing the package list with all current loader items.
        /// </summary>
        internal void ShowMoreResults()
        {
            var packageItems = _loader?.GetCurrent() ?? Enumerable.Empty<PackageItemViewModel>();
            UpdatePackageList(packageItems, refresh: true);
            ItemsLoaded = _loader?.State.ItemsCount ?? 0;

            HasStatusBarContent = ShouldShowStatusBar(_loader?.State);
        }

        internal bool ShouldShowStatusBar(IItemLoaderState state)
        {
            if (state == null)
            {
                return false;
            }

            if (state.LoadingStatus == LoadingStatus.Cancelled
                || state.LoadingStatus == LoadingStatus.ErrorOccurred)
            {
                return true;
            }

            if (_loader?.IsMultiSource == true)
            {
                var hasMore = ItemsLoaded != 0 && state.ItemsCount > ItemsLoaded;
                if (hasMore)
                {
                    return true;
                }

                if (state.LoadingStatus == LoadingStatus.Loading && state.ItemsCount > 0)
                {
                    return true;
                }
            }

            return false;
        }

        private void Package_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var package = sender as PackageItemViewModel;
            if (e.PropertyName == nameof(package.IsSelected))
            {
                if (package.IsSelected)
                {
                    Interlocked.Increment(ref _selectedCount);
                }
                else
                {
                    Interlocked.Decrement(ref _selectedCount);
                }

                UpdateSelectionState();
            }
        }

        /// <summary>
        /// Evaluates the current selection and updates the selection state properties.
        /// </summary>
        internal void UpdateSelectionState()
        {
            if (!IsUpdateMode)
            {
                HasUpdatablePackages = false;
                SelectionState = false;
                HasSelectedPackages = false;
                return;
            }

            int packageCount = PackageItemsCount;

            HasUpdatablePackages = packageCount > 0;

            if (_selectedCount == 0)
            {
                SelectionState = false;
                HasSelectedPackages = false;
            }
            else if (_selectedCount < packageCount)
            {
                SelectionState = null; // indeterminate
                HasSelectedPackages = true;
            }
            else
            {
                SelectionState = true;
                HasSelectedPackages = true;
            }
        }

        public void SelectAll()
        {
            foreach (var package in PackageItems)
            {
                package.IsSelected = true;
            }
        }

        public void DeselectAll()
        {
            foreach (var package in PackageItems)
            {
                package.IsSelected = false;
            }
        }

        public PackageItemViewModel[] GetSelectedPackages()
        {
            return PackageItems.Where(p => p.IsSelected).ToArray();
        }

        /// <summary>
        /// Combined filter predicate for use with CollectionViewSource.Filter.
        /// </summary>
        public bool FilterItem(object item)
        {
            return FilterLoadingIndicator(item)
                && FilterVulnerabilitiesIndicator(item)
                && FilterVulnerablePackage(item);
        }

        internal bool FilterVulnerabilitiesIndicator(object item)
        {
            if (item.Equals(_loadingVulnerabilitiesStatusIndicator))
            {
                return _filterByVulnerabilities && !(_loadingVulnerabilitiesStatusIndicator.Status == LoadingStatus.NoItemsFound && VulnerablePackagesCount > 0);
            }

            return true;
        }

        internal bool FilterLoadingIndicator(object item)
        {
            if (item.Equals(_loadingStatusIndicator))
            {
                return !_filterByVulnerabilities;
            }

            return true;
        }

        internal bool FilterVulnerablePackage(object item)
        {
            if (_filterByVulnerabilities && item is PackageItemViewModel vm && !vm.IsPackageVulnerable)
            {
                return false;
            }

            return true;
        }

        internal void SetVulnerabilitiesFiltering(bool enabled)
        {
            _filterByVulnerabilities = enabled;
        }

        /// <summary>
        /// Returns true if any items have transitive package level, indicating grouping should be applied.
        /// </summary>
        public bool HasTransitiveItems()
        {
            return Items
                .OfType<PackageItemViewModel>()
                .Any(p => p.PackageLevel == PackageLevel.Transitive);
        }

        public void ResetLoadingStatusIndicator()
        {
            _loadingStatusIndicator.Reset(string.Empty);
        }

        public void Dispose()
        {
            _loadingStatusIndicator.PropertyChanged -= LoadingStatusIndicator_PropertyChanged;
            ClearPackageList();
            _loadCts?.Cancel();
            _loadCts?.Dispose();
            _loadCts = null;
            ItemsLock?.Dispose();
        }
    }
}
