using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.VisualStudio;
using NuGet.VisualStudio.Internal.Contracts;
using NuGet.VisualStudio.Telemetry;

namespace NuGet.PackageManagement.UI.ViewModels
{
    public class PackageDetailsTabViewModel : ViewModelBase, IDisposable
    {
        private bool _disposed = false;
        private bool _readmeTabEnabled;

        public ReadmePreviewViewModel ReadmePreviewViewModel { get; private set; }

        private DetailControlModel DetailControlModel { get; set; }

        public ObservableCollection<TitledPageViewModelBase> Tabs { get; private set; }

        private TitledPageViewModelBase _selectedTab;
        public TitledPageViewModelBase SelectedTab
        {
            get => _selectedTab;
            set => SetAndRaisePropertyChanged(ref _selectedTab, value);
        }

        public PackageDetailsTabViewModel()
        {
            _readmeTabEnabled = true;
            Tabs = new ObservableCollection<TitledPageViewModelBase>();
        }

        public async Task InitializeAsync(DetailControlModel detailControlModel, INuGetPackageFileService nugetPackageFileService, ItemFilter currentFilter, PackageMetadataTab initialSelectedTab)
        {
            var nuGetFeatureFlagService = await ServiceLocator.GetComponentModelServiceAsync<INuGetFeatureFlagService>();
            _readmeTabEnabled = await nuGetFeatureFlagService.IsFeatureEnabledAsync(NuGetFeatureFlagConstants.RenderReadmeInPMUI);

            ReadmePreviewViewModel = new ReadmePreviewViewModel(nugetPackageFileService, currentFilter, _readmeTabEnabled);
            DetailControlModel = detailControlModel;

            if (_readmeTabEnabled)
            {
                Tabs.Add(ReadmePreviewViewModel);
            }

            Tabs.Add(DetailControlModel);

            SelectedTab = Tabs.FirstOrDefault(t => t.IsVisible && ConvertFromTabType(t) == initialSelectedTab) ?? Tabs.FirstOrDefault(t => t.IsVisible);

            DetailControlModel.PropertyChanged += DetailControlModel_PropertyChanged;

            foreach (var tab in Tabs)
            {
                tab.PropertyChanged += IsVisible_PropertyChanged;
            }
        }

        public static PackageMetadataTab ConvertFromTabType(TitledPageViewModelBase vm)
        {
            if (vm is DetailControlModel)
            {
                return PackageMetadataTab.PackageDetails;
            }
            return PackageMetadataTab.Readme;
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }
            _disposed = true;
            DetailControlModel.PropertyChanged -= DetailControlModel_PropertyChanged;

            foreach (var tab in Tabs)
            {
                tab.PropertyChanged -= IsVisible_PropertyChanged;
            }
        }

        private void IsVisible_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(TitledPageViewModelBase.IsVisible))
            {
                if (_readmeTabEnabled && sender is ReadmePreviewViewModel)
                {
                    if (ReadmePreviewViewModel.IsVisible)
                    {
                        Tabs.Insert(0, ReadmePreviewViewModel);
                    }
                    else
                    {
                        Tabs.Remove(ReadmePreviewViewModel);
                    }
                }

                if (SelectedTab == null || (SelectedTab == sender && !SelectedTab.IsVisible))
                {
                    SelectedTab = Tabs.FirstOrDefault(t => t.IsVisible);
                }
            }
        }

        private void DetailControlModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            NuGetUIThreadHelper.JoinableTaskFactory.RunAsync(async () =>
            {
                if (_readmeTabEnabled)
                {
                    await ReadmePreviewViewModel.SetPackageMetadataAsync(DetailControlModel.PackageMetadata, CancellationToken.None);
                }
            }).PostOnFailure(nameof(PackageDetailsTabViewModel), nameof(DetailControlModel_PropertyChanged));
        }
    }
}
