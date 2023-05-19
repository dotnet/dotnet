// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Internal.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Shell;
using NuGet.Configuration;
using NuGet.VisualStudio;
using NuGet.VisualStudio.Internal.Contracts;
using Resx = NuGet.PackageManagement.UI.Resources;

namespace NuGet.PackageManagement.UI.Options
{
    public partial class PackageSourceMappingOptionsControl : UserControl
    {
        private IReadOnlyList<PackageSourceMappingSourceItem> _originalPackageSourceMappings;
        private AddMappingDialog _addMappingDialog;

        public PackageSourceMappingOptionsControl()
        {
            // The use of a `JoinableTaskFactory` is not relevant as `CanExecuteChanged` will never be raised on this command.
#pragma warning disable VSTHRD012 // Provide JoinableTaskFactory where allowed
            ShowAddDialogCommand = new DelegateCommand(ExecuteShowAddDialog);
#pragma warning restore VSTHRD012 // Provide JoinableTaskFactory where allowed
            RemoveMappingCommand = new DelegateCommand(ExecuteRemoveMapping, CanExecuteRemoveMapping, NuGetUIThreadHelper.JoinableTaskFactory);
            RemoveAllMappingsCommand = new DelegateCommand(ExecuteClearMappings, CanExecuteClearMappings, NuGetUIThreadHelper.JoinableTaskFactory);
            SourceMappingsCollection = new ItemsChangeObservableCollection<SourceMappingViewModel>();
            DataContext = this;
            InitializeComponent();
        }

        public ItemsChangeObservableCollection<SourceMappingViewModel> SourceMappingsCollection { get; private set; }
        public ICommand ShowAddDialogCommand { get; set; }
        public ICommand RemoveMappingCommand { get; set; }
        public ICommand RemoveAllMappingsCommand { get; set; }

        internal void InitializeOnActivated(CancellationToken cancellationToken)
        {
            // Show package source mappings on open.
            IComponentModel componentModelMapping = NuGetUIThreadHelper.JoinableTaskFactory.Run(ServiceLocator.GetComponentModelAsync);
            var settings = componentModelMapping.GetService<ISettings>();
            var packageSourceMappingProvider = new PackageSourceMappingProvider(settings);
            _originalPackageSourceMappings = packageSourceMappingProvider.GetPackageSourceMappingItems();

            SourceMappingsCollection.Clear();
            SourceMappingsCollection.AddRange(CreateViewModels(_originalPackageSourceMappings));

            // Make sure all buttons show on open if there are already source mappings.
            (RemoveAllMappingsCommand as DelegateCommand).RaiseCanExecuteChanged();
            (RemoveMappingCommand as DelegateCommand).RaiseCanExecuteChanged();
        }
        private void ExecuteShowAddDialog(object parameter)
        {
            _addMappingDialog = new AddMappingDialog(this);
            IntPtr parent = WindowHelper.GetDialogOwnerHandle();
            WindowHelper.ShowModal(_addMappingDialog, parent);
        }

        private void ExecuteRemoveMapping(object parameter)
        {
            SourceMappingsCollection.Remove((SourceMappingViewModel)_mappingList.SelectedItem);
            (RemoveAllMappingsCommand as DelegateCommand).RaiseCanExecuteChanged();
        }

        private bool CanExecuteRemoveMapping(object parameter)
        {
            return SourceMappingsCollection.Count > 0;
        }

        private void ExecuteClearMappings(object parameter)
        {
            SourceMappingsCollection.Clear();
            (RemoveMappingCommand as DelegateCommand).RaiseCanExecuteChanged();
        }

        private bool CanExecuteClearMappings(object parameter)
        {
            return SourceMappingsCollection.Count > 0;
        }

        internal bool ApplyChangedSettings()
        {
            IReadOnlyDictionary<string, IReadOnlyList<string>> Patterns = new Dictionary<string, IReadOnlyList<string>>();
            PackageSourceMapping packageSourceMappings = new PackageSourceMapping(Patterns);
            var componentModel = NuGetUIThreadHelper.JoinableTaskFactory.Run(ServiceLocator.GetComponentModelAsync);
            var settings = componentModel.GetService<ISettings>();
            PackageSourceMappingProvider packageSourceMappingProvider = new PackageSourceMappingProvider(settings);
            IReadOnlyList<PackageSourceMappingSourceItem> packageSourceMappingsSourceItems = ConvertViewModelToSourceMappingsSourceItems(SourceMappingsCollection);
            try
            {
                if (SourceMappingsChanged(_originalPackageSourceMappings, packageSourceMappingsSourceItems))
                {
                    packageSourceMappingProvider.SavePackageSourceMappings(packageSourceMappingsSourceItems);
                }
            }
            // Thrown during creating or saving NuGet.Config.
            catch (NuGetConfigurationException ex)
            {
                MessageHelper.ShowErrorMessage(ex.Message, Resx.ErrorDialogBoxTitle);
                return false;
            }
            // Thrown if no nuget.config found.
            catch (InvalidOperationException ex)
            {
                MessageHelper.ShowErrorMessage(ex.Message, Resx.ErrorDialogBoxTitle);
                return false;
            }
            catch (UnauthorizedAccessException)
            {
                MessageHelper.ShowErrorMessage(Resx.ShowError_ConfigUnauthorizedAccess, Resx.ErrorDialogBoxTitle);
                return false;
            }
            // Unknown exception.
            catch (Exception ex)
            {
                MessageHelper.ShowErrorMessage(Resx.ShowError_ApplySettingFailed, Resx.ErrorDialogBoxTitle);
                ActivityLog.LogError(NuGetUI.LogEntrySource, ex.ToString());
                return false;
            }
            return true;
        }

        // Returns true if there are changes between existingSourceMappings and packageSourceMappings.
        private static bool SourceMappingsChanged(IReadOnlyList<PackageSourceMappingSourceItem> existingSourceMappings, IReadOnlyList<PackageSourceMappingSourceItem> packageSourceMappings)
        {
            if (existingSourceMappings.Count != packageSourceMappings.Count)
            {
                return true;
            }

            for (int i = 0; i < existingSourceMappings.Count; ++i)
            {
                //checks to see if keys match
                if (existingSourceMappings[i] != packageSourceMappings[i])
                {
                    return true;
                }
                //checks to see if all patterns match
                if (existingSourceMappings[i].Patterns.Count != packageSourceMappings[i].Patterns.Count)
                {
                    return true;
                }
                for (int j = 0; j < existingSourceMappings[i].Patterns.Count; ++j)
                {
                    if (existingSourceMappings[i].Patterns[j] != packageSourceMappings[i].Patterns[j])
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private IReadOnlyList<SourceMappingViewModel> CreateViewModels(IReadOnlyList<PackageSourceMappingSourceItem> originalMappings)
        {
            var uiSourceMappings = new Dictionary<string, List<PackageSourceContextInfo>>();
            foreach (PackageSourceMappingSourceItem sourceItem in originalMappings)
            {
                foreach (PackagePatternItem patternItem in sourceItem.Patterns)
                {
                    if (!uiSourceMappings.ContainsKey(patternItem.Pattern))
                    {
                        uiSourceMappings[patternItem.Pattern] = new List<PackageSourceContextInfo>();
                    }
                    uiSourceMappings[patternItem.Pattern].Add(new PackageSourceContextInfo(sourceItem.Key));
                }
            }
            var mappingsCollection = new List<SourceMappingViewModel>();
            foreach (string packageID in uiSourceMappings.Keys)
            {
                var viewModel = new SourceMappingViewModel(packageID, uiSourceMappings[packageID]);
                mappingsCollection.Add(viewModel);
            }
            mappingsCollection.Sort((a, b) => StringComparer.OrdinalIgnoreCase.Compare(a.ID, b.ID));
            return mappingsCollection.AsReadOnly();
        }

        private IReadOnlyList<PackageSourceMappingSourceItem> ConvertViewModelToSourceMappingsSourceItems(ItemsChangeObservableCollection<SourceMappingViewModel> sourceMappingViewModels)
        {
            var sourceNamesToPackagePatterns = new Dictionary<string, List<PackagePatternItem>>();
            foreach (SourceMappingViewModel viewModel in sourceMappingViewModels)
            {
                foreach (PackageSourceContextInfo source in viewModel.Sources)
                {
                    if (!sourceNamesToPackagePatterns.Keys.Contains(source.Name))
                    {
                        sourceNamesToPackagePatterns[source.Name] = new List<PackagePatternItem>();
                    }
                    var packagePatternItem = new PackagePatternItem(viewModel.ID);
                    if (!sourceNamesToPackagePatterns[source.Name].Any(id => id.Pattern == packagePatternItem.Pattern))
                    {
                        sourceNamesToPackagePatterns[source.Name].Add(packagePatternItem);
                    }
                }
            }

            var packageSourceMappingsSourceItems = new List<PackageSourceMappingSourceItem>();
            foreach (string source in sourceNamesToPackagePatterns.Keys)
            {
                packageSourceMappingsSourceItems.Add(new PackageSourceMappingSourceItem(source, sourceNamesToPackagePatterns[source]));
            }
            return packageSourceMappingsSourceItems.AsReadOnly();
        }
    }
}
