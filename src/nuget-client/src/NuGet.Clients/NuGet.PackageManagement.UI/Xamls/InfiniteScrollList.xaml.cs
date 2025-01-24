// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Threading;
using NuGet.Common;
using NuGet.PackageManagement.VisualStudio;
using NuGet.VisualStudio;
using NuGet.VisualStudio.Internal.Contracts;
using NuGet.VisualStudio.Telemetry;
using Mvs = Microsoft.VisualStudio.Shell;
using Resx = NuGet.PackageManagement.UI;
using Task = System.Threading.Tasks.Task;

namespace NuGet.PackageManagement.UI
{
    /// <summary>
    /// Interaction logic for InfiniteScrollList.xaml
    /// </summary>
    public partial class InfiniteScrollList : UserControl
    {
        private readonly LoadingStatusIndicator _loadingStatusIndicator = new LoadingStatusIndicator();
        private readonly LoadingStatusIndicator _loadingVulnerabilitiesStatusIndicator = new LoadingStatusIndicator();
        private ScrollViewer _scrollViewer;
        private static TimeSpan PollingDelay = TimeSpan.FromMilliseconds(100);

        public event SelectionChangedEventHandler SelectionChanged;
        public event RoutedEventHandler GroupExpansionChanged;

        public delegate void UpdateButtonClickEventHandler(PackageItemViewModel[] selectedPackages);
        public event UpdateButtonClickEventHandler UpdateButtonClicked;

        /// <summary>
        /// Fires when the items in the list have finished loading.
        /// It is triggered at <see cref="RepopulatePackageList(PackageItemViewModel, IPackageItemLoader, CancellationToken) " />, just before it is finished
        /// </summary>
        internal event EventHandler LoadItemsCompleted;

        private CancellationTokenSource _loadCts;
        private IPackageItemLoader _loader;
        private INuGetUILogger _logger;
        private Task<SearchResultContextInfo> _initialSearchResultTask;
        private readonly Lazy<JoinableTaskFactory> _joinableTaskFactory;
        private bool _checkBoxesEnabled;

        private const string LogEntrySource = "NuGet Package Manager";

        private bool _filterByVulnerabilities = false;

        // The count of packages that are selected
        private int _selectedCount;

        public InfiniteScrollList()
            : this(new Lazy<JoinableTaskFactory>(() => NuGetUIThreadHelper.JoinableTaskFactory))
        {
        }

        internal InfiniteScrollList(Lazy<JoinableTaskFactory> joinableTaskFactory)
        {
            if (joinableTaskFactory == null)
            {
                throw new ArgumentNullException(nameof(joinableTaskFactory));
            }

            _joinableTaskFactory = joinableTaskFactory;

            InitializeComponent();

            _list.ItemsLock = ReentrantSemaphore.Create(
                initialCount: 1,
                joinableTaskContext: _joinableTaskFactory.Value.Context,
                mode: ReentrantSemaphore.ReentrancyMode.Stack);

            BindingOperations.EnableCollectionSynchronization(Items, _list.ItemsLock);

            ItemsView = new CollectionViewSource() { Source = Items }.View;
            ICollectionViewLiveShaping itemsView = (ICollectionViewLiveShaping)ItemsView;
            itemsView.IsLiveFiltering = true;
            itemsView.IsLiveGrouping = true;
            itemsView.LiveFilteringProperties.Add(nameof(PackageItemViewModel.IsPackageVulnerable));
            itemsView.LiveGroupingProperties.Add(nameof(PackageItemViewModel.PackageLevel));
            ItemsView.Filter = item =>
            {
                return FilterLoadingIndicator(item)
                    && FilterVulnerabilitiesIndicator(item)
                    && FilterVulnerablePackage(item);
            };

            DataContext = itemsView;
            CheckBoxesEnabled = false;

            _loadingStatusIndicator.PropertyChanged += LoadingStatusIndicator_PropertyChanged;
        }

        private bool FilterVulnerabilitiesIndicator(object item)
        {
            if (item.Equals(_loadingVulnerabilitiesStatusIndicator))
            {
                return _filterByVulnerabilities && !(_loadingVulnerabilitiesStatusIndicator.Status == LoadingStatus.NoItemsFound && VulnerablePackagesCount > 0);
            }

            return true;
        }

        private bool FilterLoadingIndicator(object item)
        {
            if (item.Equals(_loadingStatusIndicator))
            {
                return !_filterByVulnerabilities;
            }

            return true;
        }

        private bool FilterVulnerablePackage(object item)
        {
            if (_filterByVulnerabilities && item is PackageItemViewModel vm && !vm.IsPackageVulnerable)
            {
                return false;
            }

            return true;
        }

        private void LoadingStatusIndicator_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            _joinableTaskFactory.Value.Run(async delegate
            {
                await _joinableTaskFactory.Value.SwitchToMainThreadAsync();
                if (e.PropertyName == nameof(LoadingStatusIndicator.Status)
                    && _ltbLoading.Text != _loadingStatusIndicator.LocalizedStatus)
                {
                    _ltbLoading.Text = _loadingStatusIndicator.LocalizedStatus;
                }
            });
        }

        public bool CheckBoxesEnabled
        {
            get => _checkBoxesEnabled;
            set
            {
                if (_checkBoxesEnabled != value)
                {
                    _checkBoxesEnabled = value;
                    _list.IsItemSelectionEnabled = value;
                }
            }
        }

        public bool IsSolution { get; set; }

        public ObservableCollection<object> Items { get; } = new ObservableCollection<object>();

        public ICollectionView ItemsView { get; private set; }

        /// <summary>
        /// Count of Items (excluding Loading indicator) that are currently shown after applying any UI filtering.
        /// </summary>
        private int FilteredItemsCount
        {
            get
            {
                return PackageItems.Count();
            }
        }

        /// <summary>
        /// All loaded Items (excluding Loading indicator) regardless of filtering.
        /// </summary>
        public IEnumerable<PackageItemViewModel> PackageItems => Items.OfType<PackageItemViewModel>().ToArray();

        private int VulnerablePackagesCount => Items.OfType<PackageItemViewModel>().Count(i => i.IsPackageVulnerable);

        public PackageItemViewModel SelectedPackageItem => _list.SelectedItem as PackageItemViewModel;

        public int SelectedIndex => _list.SelectedIndex;

        public Guid? OperationId => _loader?.State.OperationId;

        public int TopLevelPackageCount
        {
            get
            {
                var group = ItemsView.Groups.FirstOrDefault(g => (g as CollectionViewGroup).Name.ToString().Equals(PackageLevel.TopLevel.ToString(), StringComparison.OrdinalIgnoreCase));
                return group is not null ? (group as CollectionViewGroup).ItemCount : 0;
            }
        }

        public int TransitivePackageCount
        {
            get
            {
                var group = ItemsView.Groups.FirstOrDefault(g => (g as CollectionViewGroup).Name.ToString().Equals(PackageLevel.Transitive.ToString(), StringComparison.OrdinalIgnoreCase));
                return group is not null ? (group as CollectionViewGroup).ItemCount : 0;
            }
        }

        // Load items using the specified loader
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
            _loadingStatusBar.Visibility = Visibility.Hidden;
            _loadingStatusBar.Reset(loadingMessage, loader.IsMultiSource);

            var selectedPackageItem = SelectedPackageItem;

            await _list.ItemsLock.ExecuteAsync(() =>
            {
                ClearPackageList();
                return Task.CompletedTask;
            });

            _selectedCount = 0;

            // triggers the package list loader
            await LoadItemsAsync(selectedPackageItem, token);
        }

        /// <summary>
        /// Keep the previously selected package after a search.
        /// Otherwise, select the first on the search if none was selected before.
        /// </summary>
        /// <param name="selectedItem">Previously selected item</param>
        internal void UpdateSelectedItem(PackageItemViewModel selectedItem)
        {
            if (selectedItem != null)
            {
                // select the the previously selected item if it still exists.
                selectedItem = PackageItems
                    .FirstOrDefault(item => item.Id.Equals(selectedItem.Id, StringComparison.OrdinalIgnoreCase));
            }

            // select the first item if none was selected before
            _list.SelectedItem = selectedItem ?? PackageItems.FirstOrDefault();
        }

        private async Task LoadItemsAsync(PackageItemViewModel selectedPackageItem, CancellationToken token)
        {
            // If there is another async loading process - cancel it.
            var loadCts = CancellationTokenSource.CreateLinkedTokenSource(token);
            Interlocked.Exchange(ref _loadCts, loadCts)?.Cancel();

            await RepopulatePackageListAsync(selectedPackageItem, _loader, loadCts);
        }

        private async Task RepopulatePackageListAsync(PackageItemViewModel selectedPackageItem, IPackageItemLoader currentLoader, CancellationTokenSource loadCts)
        {
            await TaskScheduler.Default;

            var addedLoadingIndicator = false;

            try
            {
                // add Loading... indicator if not present
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
                    UpdateSelectedItem(selectedPackageItem);
                }
            }
            catch (OperationCanceledException) when (!loadCts.IsCancellationRequested)
            {
                loadCts.Cancel();
                loadCts.Dispose();
                currentLoader.Reset();

                await _joinableTaskFactory.Value.SwitchToMainThreadAsync();

                // The user cancelled the login, but treat as a load error in UI
                // So the retry button and message is displayed
                // Do not log to the activity log, since it is not a NuGet error
                _logger.Log(new LogMessage(LogLevel.Error, Resx.Resources.Text_UserCanceled));

                _loadingStatusIndicator.SetError(Resx.Resources.Text_UserCanceled);

                _loadingStatusBar.SetCancelled();
                _loadingStatusBar.Visibility = Visibility.Visible;
            }
            catch (Exception ex) when (!loadCts.IsCancellationRequested)
            {
                loadCts.Cancel();
                loadCts.Dispose();
                currentLoader.Reset();

                // Write stack to activity log
                Mvs.ActivityLog.LogError(LogEntrySource, ex.ToString());

                await _joinableTaskFactory.Value.SwitchToMainThreadAsync();

                var errorMessage = ExceptionUtilities.DisplayMessage(ex);
                _logger.Log(new LogMessage(LogLevel.Error, errorMessage));

                _loadingStatusIndicator.SetError(errorMessage);

                _loadingStatusBar.SetError();
                _loadingStatusBar.Visibility = Visibility.Visible;
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
                    // Ideally, after a search, it should report its status, and
                    // do not keep the LoadingStatus.Loading forever.
                    // This is a workaround.
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

            UpdateCheckBoxStatus();

            LoadItemsCompleted?.Invoke(this, EventArgs.Empty);
        }

        private async Task LoadItemsCoreAsync(IPackageItemLoader currentLoader, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            var loadedItems = await LoadNextPageAsync(currentLoader, token);
            token.ThrowIfCancellationRequested();

            // multiple loads may occur at the same time as a result of multiple instances,
            // makes sure we update using the relevant one.
            if (currentLoader == _loader)
            {
                UpdatePackageList(loadedItems, refresh: false);
            }

            token.ThrowIfCancellationRequested();

            await _joinableTaskFactory.Value.RunAsync(async () =>
            {
                await _joinableTaskFactory.Value.SwitchToMainThreadAsync();

                _loadingStatusBar.ItemsLoaded = currentLoader.State.ItemsCount;
            });

            token.ThrowIfCancellationRequested();

            // keep waiting till completion
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

            // if searchResultTask is in progress then just wait for it to complete
            // without creating new load task
            if (_initialSearchResultTask != null)
            {
                token.ThrowIfCancellationRequested();

                // update initial progress
                await currentLoader.UpdateStateAndReportAsync(new SearchResultContextInfo(), progress, token);

                var results = await _initialSearchResultTask;

                token.ThrowIfCancellationRequested();

                // update state and progress
                await currentLoader.UpdateStateAndReportAsync(results, progress, token);

                _initialSearchResultTask = null;
            }
            else
            {
                // trigger loading
                await currentLoader.LoadNextAsync(progress, token);
            }

            await WaitForInitialResultsAsync(currentLoader, progress, token);

            return currentLoader.GetCurrent();
        }

        private async Task WaitForCompletionAsync(IItemLoader<PackageItemViewModel> currentLoader, CancellationToken token)
        {
            var progress = new Progress<IItemLoaderState>(
                s => HandleItemLoaderStateChange(currentLoader, s));

            // run to completion
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
        /// Shows the Loading status bar, if necessary. Also, it inserts the Loading... indicator, if necesary
        /// </summary>
        /// <param name="loader">Current loader</param>
        /// <param name="state">Progress reported by the <c>Progress</c> callback</param>
        private void HandleItemLoaderStateChange(IItemLoader<PackageItemViewModel> loader, IItemLoaderState state)
        {
            NuGetUIThreadHelper.JoinableTaskFactory.RunAsync(async () =>
            {
                if (loader == _loader)
                {
                    await _joinableTaskFactory.Value.SwitchToMainThreadAsync();

                    _loadingStatusBar.UpdateLoadingState(state);

                    // decide when to show status bar
                    var desiredVisibility = EvaluateStatusBarVisibility(loader, state);

                    if (_loadingStatusBar.Visibility != Visibility.Visible
                        && desiredVisibility == Visibility.Visible)
                    {
                        _loadingStatusBar.Visibility = desiredVisibility;
                    }

                    _loadingStatusIndicator.Status = state.LoadingStatus;

                    if (!Items.Contains(_loadingStatusIndicator))
                    {
                        await _list.ItemsLock.ExecuteAsync(() =>
                        {
                            Items.Add(_loadingStatusIndicator);
                            return Task.CompletedTask;
                        });
                    }
                }
            }).PostOnFailure(nameof(InfiniteScrollList), nameof(HandleItemLoaderStateChange));
        }

        private Visibility EvaluateStatusBarVisibility(IItemLoader<PackageItemViewModel> loader, IItemLoaderState state)
        {
            var statusBarVisibility = Visibility.Hidden;

            if (state.LoadingStatus == LoadingStatus.Cancelled
                || state.LoadingStatus == LoadingStatus.ErrorOccurred)
            {
                statusBarVisibility = Visibility.Visible;
            }

            if (loader.IsMultiSource)
            {
                var hasMore = _loadingStatusBar.ItemsLoaded != 0 && state.ItemsCount > _loadingStatusBar.ItemsLoaded;
                if (hasMore)
                {
                    statusBarVisibility = Visibility.Visible;
                }

                if (state.LoadingStatus == LoadingStatus.Loading && state.ItemsCount > 0)
                {
                    statusBarVisibility = Visibility.Visible;
                }
            }

            return statusBarVisibility;
        }

        /// <summary>
        /// Appends <c>packages</c> to the internal <see cref="Items"> list
        /// </summary>
        /// <param name="packages">Packages collection to add</param>
        /// <param name="refresh">Clears <see cref="Items"> list if set to <see langword="true" /></param>
        private void UpdatePackageList(IEnumerable<PackageItemViewModel> packages, bool refresh)
        {
            _joinableTaskFactory.Value.Run(async () =>
            {
                // Synchronize updating Items list
                await _list.ItemsLock.ExecuteAsync(() =>
                {
                    // remove the loading status indicator if it's in the list
                    bool removed = Items.Remove(_loadingStatusIndicator);

                    if (refresh)
                    {
                        ClearPackageList();
                    }

                    // add newly loaded items
                    foreach (var package in packages)
                    {
                        package.PropertyChanged += Package_PropertyChanged;
                        Items.Add(package);
                        _selectedCount = package.IsSelected ? _selectedCount + 1 : _selectedCount;
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
        /// Clear <c>Items</c> list and removes the event handlers for each element
        /// </summary>
        private void ClearPackageList()
        {
            foreach (var package in PackageItems)
            {
                package.PropertyChanged -= Package_PropertyChanged;
                package.Dispose();
            }

            Items.Clear();
            _loadingStatusBar.ItemsLoaded = 0;
        }

        public void UpdatePackageStatus(PackageCollectionItem[] installedPackages, bool clearCache = false)
        {
            // in this case, we only need to update PackageStatus of
            // existing items in the package list
            foreach (var package in PackageItems)
            {
                if (package.PackageLevel == PackageLevel.TopLevel)
                {
                    package.UpdatePackageStatus(installedPackages, clearCache);
                }
                else
                {
                    package.UpdateTransitivePackageStatus(package.InstalledVersion);
                }
            }
        }

        private void Package_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var package = sender as PackageItemViewModel;
            if (e.PropertyName == nameof(package.IsSelected))
            {
                if (package.IsSelected)
                {
                    _selectedCount++;
                }
                else
                {
                    _selectedCount--;
                }

                UpdateCheckBoxStatus();
            }
        }

        // Update the status of the _selectAllPackages check box and the Update button.
        private void UpdateCheckBoxStatus()
        {
            // The current tab is not "Updates".
            if (!CheckBoxesEnabled)
            {
                _updateButtonContainer.Visibility = Visibility.Collapsed;
                return;
            }

            //Are any packages shown with the current filter?
            int packageCount = FilteredItemsCount;

            _updateButtonContainer.Visibility =
                packageCount > 0 ?
                Visibility.Visible :
                Visibility.Collapsed;

            if (_selectedCount == 0)
            {
                _selectAllPackages.IsChecked = false;
                _updateButton.IsEnabled = false;
            }
            else if (_selectedCount < packageCount)
            {
                _selectAllPackages.IsChecked = null;
                _updateButton.IsEnabled = true;
            }
            else
            {
                _selectAllPackages.IsChecked = true;
                _updateButton.IsEnabled = true;
            }
        }

        public PackageItemViewModel SelectedItem
        {
            get
            {
                return _list.SelectedItem as PackageItemViewModel;
            }
            internal set
            {
                _list.SelectedItem = value;
            }
        }

        private void List_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0
                && e.AddedItems[0] is LoadingStatusIndicator)
            {
                // make the loading object not selectable
                if (e.RemovedItems.Count > 0)
                {
                    _list.SelectedItem = e.RemovedItems[0];
                }
                else
                {
                    _list.SelectedIndex = -1;
                }
            }
            else
            {
                if (SelectionChanged != null)
                {
                    SelectionChanged(this, e);
                }
            }
        }

        private void List_Loaded(object sender, RoutedEventArgs e)
        {
            _list.Loaded -= List_Loaded;

            var c = VisualTreeHelper.GetChild(_list, 0) as Border;
            if (c == null)
            {
                return;
            }

            c.Padding = new Thickness(0);
            _scrollViewer = VisualTreeHelper.GetChild(c, 0) as ScrollViewer;
            if (_scrollViewer == null)
            {
                return;
            }

            _scrollViewer.Padding = new Thickness(0);
            _scrollViewer.ScrollChanged += ScrollViewer_ScrollChanged;
        }

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            int packagesCount = Items.Count;

            if (Items.Contains(_loadingStatusIndicator))
            {
                packagesCount--;
            }

            if (Items.Contains(_loadingVulnerabilitiesStatusIndicator))
            {
                packagesCount--;
            }

            if (_loader?.State.LoadingStatus == LoadingStatus.Ready)
            {
                var first = _scrollViewer.VerticalOffset;
                var last = _scrollViewer.ViewportHeight + first;
                if (_scrollViewer.ViewportHeight > 0 && last >= packagesCount)
                {
                    NuGetUIThreadHelper.JoinableTaskFactory.RunAsync(() =>
                        LoadItemsAsync(selectedPackageItem: null, token: CancellationToken.None)
                    ).PostOnFailure(nameof(InfiniteScrollList));
                }
            }
        }

        private void SelectAllPackagesCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            foreach (var item in _list.Items)
            {
                var package = item as PackageItemViewModel;

                // note that item could be the loading indicator, thus we need to check
                // for null here.
                if (package != null)
                {
                    package.IsSelected = true;
                }
            }
        }

        private void SelectAllPackagesCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            foreach (var item in _list.Items)
            {
                var package = item as PackageItemViewModel;
                if (package != null)
                {
                    package.IsSelected = false;
                }
            }
        }

        private void _updateButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedPackages = PackageItems.Where(p => p.IsSelected).ToArray();
            UpdateButtonClicked(selectedPackages);
        }

        private void List_PreviewKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Space && e.OriginalSource is ListBoxItem && _list.SelectedItem is PackageItemViewModel package)
            {
                // toggle the selection state when user presses the space bar
                package.IsSelected = !package.IsSelected;
                e.Handled = true;
            }
        }

        private void _loadingStatusBar_ShowMoreResultsClick(object sender, RoutedEventArgs e)
        {
            var packageItems = _loader?.GetCurrent() ?? Enumerable.Empty<PackageItemViewModel>();
            UpdatePackageList(packageItems, refresh: true);
            _loadingStatusBar.ItemsLoaded = _loader?.State.ItemsCount ?? 0;

            var desiredVisibility = EvaluateStatusBarVisibility(_loader, _loader.State);
            if (_loadingStatusBar.Visibility != desiredVisibility)
            {
                _loadingStatusBar.Visibility = desiredVisibility;
            }
        }

        private void _loadingStatusBar_DismissClick(object sender, RoutedEventArgs e)
        {
            _loadingStatusBar.Visibility = Visibility.Hidden;
        }

        public void ResetLoadingStatusIndicator()
        {
            _loadingStatusIndicator.Reset(string.Empty);
        }

        internal void ClearPackageLevelGrouping()
        {
            ItemsView.GroupDescriptions.Clear();
        }

        internal void AddVulnerabilitiesFiltering()
        {
            _filterByVulnerabilities = true;
            ItemsView.Refresh();
        }

        internal void RemoveVulnerabilitiesFiltering()
        {
            _filterByVulnerabilities = false;
            ItemsView.Refresh();
        }

        internal void AddPackageLevelGrouping()
        {
            ItemsView.Refresh();
            if (Items
                    .OfType<PackageItemViewModel>()
                    .Any(p => p.PackageLevel == PackageLevel.Transitive))
            {
                ItemsView.GroupDescriptions.Add(new PropertyGroupDescription(nameof(PackageItemViewModel.PackageLevel)));
            }
        }

        private void Expander_ExpansionStateToggled(object sender, RoutedEventArgs e)
        {
            GroupExpansionChanged?.Invoke(sender, e);
        }
    }
}
