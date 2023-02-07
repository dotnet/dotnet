// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.ServiceHub.Framework;
using Microsoft.VisualStudio.PlatformUI;
using NuGet.PackageManagement.UI;
using NuGet.VisualStudio;
using NuGet.VisualStudio.Common;
using NuGet.VisualStudio.Internal.Contracts;
using Task = System.Threading.Tasks.Task;

namespace NuGet.Options
{
    public partial class AddMappingDialog : DialogWindow
    {
        public ICommand HideDialogCommand { get; set; }

        public ICommand AddMappingCommand { get; set; }

        public ItemsChangeObservableCollection<PackageSourceViewModel> SourcesCollection { get; private set; }

        private IReadOnlyList<PackageSourceContextInfo> _originalPackageSources;

        private PackageSourceMappingOptionsControl _parent;

        public AddMappingDialog(PackageSourceMappingOptionsControl parent)
        {
            _parent = parent;

            // The use of a `JoinableTaskFactory` is not relevant as `CanExecuteChanged` will never be raised on this command.
#pragma warning disable VSTHRD012 // Provide JoinableTaskFactory where allowed
            HideDialogCommand = new DelegateCommand(ExecuteHideDialog);
#pragma warning restore VSTHRD012 // Provide JoinableTaskFactory where allowed
            AddMappingCommand = new DelegateCommand(ExecuteAddMapping, CanExecuteAddMapping, NuGetUIThreadHelper.JoinableTaskFactory);
            SourcesCollection = new ItemsChangeObservableCollection<PackageSourceViewModel>();
            DataContext = this;
            InitializeComponent();
            CancellationToken cancellationToken = new CancellationToken(false);
            NuGetUIThreadHelper.JoinableTaskFactory.Run(async () => await InitializeOnActivatedAsync(cancellationToken));
        }

        internal async Task InitializeOnActivatedAsync(CancellationToken cancellationToken)
        {
            IServiceBrokerProvider serviceBrokerProvider = await ServiceLocator.GetComponentModelServiceAsync<IServiceBrokerProvider>();
            IServiceBroker serviceBroker = await serviceBrokerProvider.GetAsync();

            using (INuGetSourcesService _nugetSourcesService = await serviceBroker.GetProxyAsync<INuGetSourcesService>(
                    NuGetServices.SourceProviderService,
                    cancellationToken: cancellationToken))
            {
                //show package sources on open
                _originalPackageSources = await _nugetSourcesService.GetPackageSourcesAsync(cancellationToken);
            }

            SourcesCollection.Clear();
            foreach (PackageSourceContextInfo source in _originalPackageSources)
            {
                var viewModel = new PackageSourceViewModel(source);
                SourcesCollection.Add(viewModel);
            }
        }

        private void ExecuteHideDialog(object parameter)
        {
            Close();
        }

        private void ExecuteAddMapping(object parameter)
        {
            if (string.IsNullOrWhiteSpace(_packageID.Text))
            {
                return;
            }

            string packageId = _packageID.Text;
            List<PackageSourceContextInfo> packageSources = new List<PackageSourceContextInfo>();
            foreach (PackageSourceViewModel source in _sourcesListBox.Items)
            {
                if (source.IsSelected)
                {
                    packageSources.Add(source.SourceInfo);
                }
            }
            var viewModel = new SourceMappingViewModel(packageId, packageSources);
            _parent.SourceMappingsCollection.Add(viewModel);

            (_parent.ShowAddDialogCommand as DelegateCommand).RaiseCanExecuteChanged();
            (_parent.RemoveMappingCommand as DelegateCommand).RaiseCanExecuteChanged();
            (_parent.RemoveAllMappingsCommand as DelegateCommand).RaiseCanExecuteChanged();
            Close();
        }

        private bool CanExecuteAddMapping(object parameter)
        {
            if (string.IsNullOrWhiteSpace(_packageID.Text))
            {
                return false;
            }

            foreach (PackageSourceViewModel source in _sourcesListBox.Items)
            {
                if (source.IsSelected)
                {
                    return true;
                }
            }
            return false;
        }

        // Allows the user to drag the window around
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var itemCheckBox = sender as CheckBox;
            var itemContainer = itemCheckBox?.FindAncestor<ListViewItem>();
            if (itemContainer is null)
            {
                return;
            }

            var newValue = (e.RoutedEvent == CheckBox.CheckedEvent);
            var oldValue = !newValue; // Assume the state has actually toggled.
            AutomationPeer peer = UIElementAutomationPeer.FromElement(itemContainer);
            peer?.RaisePropertyChangedEvent(
                TogglePatternIdentifiers.ToggleStateProperty,
                oldValue ? ToggleState.On : ToggleState.Off,
                newValue ? ToggleState.On : ToggleState.Off);

            (AddMappingCommand as DelegateCommand).RaiseCanExecuteChanged();
        }

        private void PackageID_TextChanged(object sender, TextChangedEventArgs e)
        {
            (AddMappingCommand as DelegateCommand).RaiseCanExecuteChanged();
        }

        private void SourcesListBox_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space
                && ((ListViewItem)(_sourcesListBox.ItemContainerGenerator.ContainerFromItem(_sourcesListBox.SelectedItem))).IsFocused)
            {
                // toggle the selection state when user presses the space bar when focus is on the ListViewItem
                var viewModel = _sourcesListBox.SelectedItem as PackageSourceViewModel;
                if (viewModel != null)
                {
                    viewModel.IsSelected = !viewModel.IsSelected;
                    e.Handled = true;
                }
            }
        }

        private void AddMappingDialogWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(_packageID);
        }
    }
}
