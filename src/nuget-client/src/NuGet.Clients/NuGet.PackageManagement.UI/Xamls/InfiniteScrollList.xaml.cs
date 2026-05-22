// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable disable

using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.VisualStudio.Threading;
using NuGet.PackageManagement.UI.ViewModels;
using NuGet.VisualStudio;
using NuGet.VisualStudio.Internal.Contracts;
using Mvs = Microsoft.VisualStudio.Shell;

namespace NuGet.PackageManagement.UI
{
    /// <summary>
    /// Interaction logic for InfiniteScrollList.xaml.
    /// This codebehind handles only WPF-specific concerns; all business logic
    /// is in <see cref="InfiniteScrollListViewModel"/>.
    /// </summary>
    public partial class InfiniteScrollList : UserControl, IDisposable
    {
        private ScrollViewer _scrollViewer;
        private const string LogEntrySource = "NuGet Package Manager";
        private bool _disposed;

        public InfiniteScrollListViewModel ViewModel { get; }

        public event SelectionChangedEventHandler SelectionChanged;
        public event RoutedEventHandler GroupExpansionChanged;

        public delegate void UpdateButtonClickEventHandler(PackageItemViewModel[] selectedPackages);
        public event UpdateButtonClickEventHandler UpdateButtonClicked;

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

            ViewModel = new InfiniteScrollListViewModel(joinableTaskFactory);

            InitializeComponent();

            // Share the ViewModel's semaphore with the ListBox for synchronized collection access
            _list.ItemsLock = ViewModel.ItemsLock;

            BindingOperations.EnableCollectionSynchronization(ViewModel.Items, _list.ItemsLock);

            ItemsView = new CollectionViewSource() { Source = ViewModel.Items }.View;
            ICollectionViewLiveShaping itemsView = (ICollectionViewLiveShaping)ItemsView;
            itemsView.IsLiveFiltering = true;
            itemsView.IsLiveGrouping = true;
            itemsView.LiveFilteringProperties.Add(nameof(PackageItemViewModel.IsPackageVulnerable));
            itemsView.LiveGroupingProperties.Add(nameof(PackageItemViewModel.PackageLevel));
            ItemsView.Filter = item => ViewModel.FilterItem(item);

            // Set the ListBox's DataContext to the filtered/grouped view so ItemsSource="{Binding}" works.
            // This must be done in codebehind (not XAML) because ItemsView is created after InitializeComponent.
            _list.DataContext = ItemsView;

            ViewModel.IsUpdateMode = false;

            // Wire UI callback delegates for operations that cannot be expressed in XAML.
            // These must be set before any LoadItemsAsync call (PackageManagerControl calls it during init).
            ViewModel.StatusBarUpdateLoadingState = state => _loadingStatusBar.UpdateLoadingState(state);
            ViewModel.StatusBarSetCancelled = () => _loadingStatusBar.SetCancelled();
            ViewModel.StatusBarSetError = () => _loadingStatusBar.SetError();
            ViewModel.StatusBarReset = (msg, isMulti) => _loadingStatusBar.Reset(msg, isMulti);
            ViewModel.LogActivityError = msg => Mvs.ActivityLog.LogError(LogEntrySource, msg);

            // Sync IsUpdateMode to the ListBox's IsItemSelectionEnabled (not a DependencyProperty, can't bind)
            ViewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(InfiniteScrollListViewModel.IsUpdateMode))
                {
                    _list.IsItemSelectionEnabled = ViewModel.IsUpdateMode;
                }
            };
        }

        public ICollectionView ItemsView { get; private set; }

        public int SelectedIndex => _list.SelectedIndex;

        public int TopLevelPackageCount
        {
            get
            {
                var group = ItemsView.Groups?.FirstOrDefault(
                    g => (g as CollectionViewGroup)?.Name?.ToString()
                        .Equals(PackageLevel.TopLevel.ToString(), StringComparison.OrdinalIgnoreCase) == true);
                return group is CollectionViewGroup cvg ? cvg.ItemCount : 0;
            }
        }

        public int TransitivePackageCount
        {
            get
            {
                var group = ItemsView.Groups?.FirstOrDefault(
                    g => (g as CollectionViewGroup)?.Name?.ToString()
                        .Equals(PackageLevel.Transitive.ToString(), StringComparison.OrdinalIgnoreCase) == true);
                return group is CollectionViewGroup cvg ? cvg.ItemCount : 0;
            }
        }

        internal void ClearPackageLevelGrouping()
        {
            ItemsView.GroupDescriptions.Clear();
        }

        internal void AddVulnerabilitiesFiltering()
        {
            ViewModel.SetVulnerabilitiesFiltering(true);
            ItemsView.Refresh();
        }

        internal void RemoveVulnerabilitiesFiltering()
        {
            ViewModel.SetVulnerabilitiesFiltering(false);
            ItemsView.Refresh();
        }

        internal void AddPackageLevelGrouping()
        {
            ItemsView.Refresh();
            if (ViewModel.HasTransitiveItems())
            {
                ItemsView.GroupDescriptions.Add(new PropertyGroupDescription(nameof(PackageItemViewModel.PackageLevel)));
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
                SelectionChanged?.Invoke(this, e);
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
            if (ViewModel.Loader?.State.LoadingStatus == LoadingStatus.Ready)
            {
                var first = _scrollViewer.VerticalOffset;
                var last = _scrollViewer.ViewportHeight + first;

                int packagesCount = ViewModel.PackageItemsCount;

                if (_scrollViewer.ViewportHeight > 0 && last >= packagesCount)
                {
                    _ = ViewModel.LoadMoreItemsAsync();
                }
            }
        }

        private void SelectAllPackagesCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            ViewModel.SelectAll();
        }

        private void SelectAllPackagesCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            ViewModel.DeselectAll();
        }

        private void _updateButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedPackages = ViewModel.GetSelectedPackages();
            UpdateButtonClicked?.Invoke(selectedPackages);
        }

        private void List_PreviewKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Space && e.OriginalSource is ListBoxItem && _list.SelectedItem is PackageItemViewModel package)
            {
                package.IsSelected = !package.IsSelected;
                e.Handled = true;
            }
        }

        private void _loadingStatusBar_ShowMoreResultsClick(object sender, RoutedEventArgs e)
        {
            ViewModel.ShowMoreResults();
        }

        private void _loadingStatusBar_DismissClick(object sender, RoutedEventArgs e)
        {
            ViewModel.HasStatusBarContent = false;
        }

        private void Expander_ExpansionStateToggled(object sender, RoutedEventArgs e)
        {
            GroupExpansionChanged?.Invoke(sender, e);
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            ViewModel.Dispose();
            _disposed = true;
        }
    }
}
