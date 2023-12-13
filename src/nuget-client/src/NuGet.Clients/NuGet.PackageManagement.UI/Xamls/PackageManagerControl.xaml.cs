// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft;
using Microsoft.ServiceHub.Framework;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Threading;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.PackageManagement.Telemetry;
using NuGet.PackageManagement.UI.ViewModels;
using NuGet.PackageManagement.VisualStudio;
using NuGet.Packaging.Core;
using NuGet.ProjectManagement;
using NuGet.Protocol.Core.Types;
using NuGet.Resolver;
using NuGet.Versioning;
using NuGet.VisualStudio;
using NuGet.VisualStudio.Internal.Contracts;
using NuGet.VisualStudio.Telemetry;
using Resx = NuGet.PackageManagement.UI;
using Task = System.Threading.Tasks.Task;

namespace NuGet.PackageManagement.UI
{
    /// <summary>
    /// Interaction logic for PackageManagerControl.xaml
    /// </summary>
    public partial class PackageManagerControl : UserControl, IVsWindowSearch, IDisposable, IPackageManagerControlViewModel
    {
        internal event EventHandler _actionCompleted;
        internal DetailControlModel _detailModel;
        internal CancellationTokenSource _loadCts;
        private CancellationTokenSource _cancelSelectionChangedSource;
        private bool _initialized;
        private IVsWindowSearchHost _windowSearchHost;
        private IVsWindowSearchHostFactory _windowSearchHostFactory;
        private INuGetUILogger _uiLogger;
        private readonly Guid _sessionGuid = Guid.NewGuid();
        private Stopwatch _sinceLastRefresh;
        private CancellationTokenSource _refreshCts;
        // used to prevent starting new search when we update the package sources
        // list in response to PackageSourcesChanged event.
        private bool _dontStartNewSearch;
        // When executing a UI operation, we disable the PM UI and ignore any refresh requests.
        // This tells the operation execution part that it needs to trigger a refresh when done.
        private bool _isRefreshRequired;
        private bool _isExecutingAction; // Signifies where an action is being executed. Should be updated in a coordinated fashion with IsEnabled
        private RestartRequestBar _restartBar;
        private PRMigratorBar _migratorBar;
        private bool _missingPackageStatus;
        private bool _loadedAndInitialized = false;
        private bool _recommendPackages = false;
        private string _settingsKey;
        private IServiceBroker _serviceBroker;
        private bool _disposed = false;
        private IPackageVulnerabilityService _packageVulnerabilityService;

        private PackageManagerInstalledTabData _installedTabTelemetryData;

        private PackageManagerControl()
        {
            InitializeComponent();
        }

        public static async ValueTask<PackageManagerControl> CreateAsync(PackageManagerModel model, INuGetUILogger uiLogger)
        {
            Assumes.NotNull(model);

            var packageManagerControl = new PackageManagerControl();
            await packageManagerControl.InitializeAsync(model, uiLogger);
            return packageManagerControl;
        }

        private async ValueTask InitializeAsync(PackageManagerModel model, INuGetUILogger uiLogger)
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            _sinceLastRefresh = Stopwatch.StartNew();

            _installedTabTelemetryData = new PackageManagerInstalledTabData();

            Model = model;
            _uiLogger = uiLogger;
            Settings = await ServiceLocator.GetComponentModelServiceAsync<ISettings>();

            _windowSearchHostFactory = await ServiceLocator.GetGlobalServiceAsync<SVsWindowSearchHostFactory, IVsWindowSearchHostFactory>();
            _serviceBroker = model.Context.ServiceBroker;

            if (Model.IsSolution)
            {
                _detailModel = await PackageSolutionDetailControlModel.CreateAsync(
                    Model.Context.ServiceBroker,
                    Model.Context.SolutionManagerService,
                    Model.Context.Projects,
                    Model.UIController,
                    CancellationToken.None);
            }
            else
            {
                _detailModel = new PackageDetailControlModel(
                    Model.Context.ServiceBroker,
                    Model.Context.SolutionManagerService,
                    Model.Context.Projects,
                    Model.UIController);
            }

            if (_windowSearchHostFactory != null)
            {
                _windowSearchHost = _windowSearchHostFactory.CreateWindowSearchHost(_topPanel.SearchControlParent);
                _windowSearchHost.SetupSearch(this);
                _windowSearchHost.IsVisible = true;
            }

            AddRestoreBar();
            await AddRestartRequestBarAsync();

            _packageDetail.Control = this;
            _packageDetail.Visibility = Visibility.Hidden;

            await SetTitleAsync();

            _topPanel.IsSolution = Model.IsSolution;

            if (_topPanel.IsSolution)
            {
                _topPanel.CreateAndAddConsolidateTab();
            }

            _settingsKey = await GetSettingsKeyAsync(CancellationToken.None);
            UserSettings settings = LoadSettings();
            InitializeFilterList(settings);
            await InitPackageSourcesAsync(settings, CancellationToken.None);
            ApplySettings(settings, Settings);
            _initialized = true;

            await IsCentralPackageManagementEnabledAsync(CancellationToken.None);

            NuGetExperimentationService = await ServiceLocator.GetComponentModelServiceAsync<INuGetExperimentationService>();

            // UI is initialized. Start the first search
            _packageList.CheckBoxesEnabled = _topPanel.Filter == ItemFilter.UpdatesAvailable;
            _packageList.IsSolution = Model.IsSolution;

            Loaded += PackageManagerLoaded;

            // register with the UI controller
            var controller = model.UIController as NuGetUI;
            if (controller != null)
            {
                controller.PackageManagerControl = this;
            }

            ISourceRepositoryProvider sourceRepositoryProvider = await ServiceLocator.GetComponentModelServiceAsync<ISourceRepositoryProvider>();
            var sourceRepositories = sourceRepositoryProvider.GetRepositories();
            _packageVulnerabilityService = new PackageVulnerabilityService(sourceRepositories, _uiLogger);

            var solutionManager = Model.Context.SolutionManagerService;
            solutionManager.ProjectAdded += OnProjectChanged;
            solutionManager.ProjectRemoved += OnProjectChanged;
            solutionManager.ProjectUpdated += OnProjectUpdated;
            solutionManager.ProjectRenamed += OnProjectRenamed;
            solutionManager.AfterNuGetCacheUpdated += OnNuGetCacheUpdated;

            Model.Context.ProjectActionsExecuted += OnProjectActionsExecuted;

            Model.Context.SourceService.PackageSourcesChanged += PackageSourcesChanged;

            Unloaded += PackageManagerUnloaded;

            if (IsUILegalDisclaimerSuppressed())
            {
                _legalDisclaimer.Visibility = Visibility.Collapsed;
            }

            _missingPackageStatus = false;

            Settings.SettingsChanged += Settings_SettingsChanged;
        }

        private void Settings_SettingsChanged(object sender, EventArgs e)
        {
            _detailModel.PackageSourceMappingViewModel.SettingsChanged();
            _detailModel.SetInstalledOrUpdateButtonIsEnabled();
        }

        public PackageRestoreBar RestoreBar { get; private set; }
        public PackageManagerModel Model { get; private set; }

        public ISettings Settings { get; private set; }

        public ItemFilter ActiveFilter { get => _topPanel.Filter; set => _topPanel.SelectFilter(value); }

        public bool IsSolution => Model.IsSolution;

        internal InfiniteScrollList PackageList => _packageList;

        internal PackageSourceMoniker SelectedSource
        {
            get => _topPanel.SourceRepoList.SelectedItem as PackageSourceMoniker;
            set => _topPanel.SourceRepoList.SelectedItem = value;
        }

        internal IEnumerable<PackageSourceMoniker> PackageSources => _topPanel.PackageSources;

        public bool IncludePrerelease => _topPanel.CheckboxPrerelease.IsChecked == true;

        public INuGetExperimentationService NuGetExperimentationService { get; private set; }

        public bool IsVulnerableFilteringApplied => _topPanel?.CheckBoxVulnerabilities?.IsChecked == true;

        private void OnProjectUpdated(object sender, IProjectContextInfo project)
        {
            Model.Context.Projects = _detailModel.NuGetProjects;
        }

        private void OnProjectRenamed(object sender, IProjectContextInfo project)
        {
            OnProjectChanged(sender, project);
            if (!Model.IsSolution)
            {
                NuGetUIThreadHelper.JoinableTaskFactory.RunAsync(() => SolutionManager_ProjectRenamedAsync(project))
                    .PostOnFailure(nameof(PackageManagerControl), nameof(OnProjectRenamed));
            }
        }

        private async Task SolutionManager_ProjectRenamedAsync(IProjectContextInfo project)
        {
            Model.Context.Projects = new[] { project };

            IProjectContextInfo currentNugetProject = Model.Context.Projects.First();

            IProjectMetadataContextInfo currentProjectMetadata = await currentNugetProject.GetMetadataAsync(
                Model.Context.ServiceBroker,
                CancellationToken.None);
            IProjectMetadataContextInfo renamedProjectMetadata = await project.GetMetadataAsync(
                Model.Context.ServiceBroker,
                CancellationToken.None);

            if (currentProjectMetadata.FullPath == renamedProjectMetadata.FullPath)
            {
                _settingsKey = GetProjectSettingsKey(renamedProjectMetadata.Name);

                await SetTitleAsync(currentProjectMetadata);
            }
        }

        private void OnProjectChanged(object sender, IProjectContextInfo project)
        {
            var timeSpan = GetTimeSinceLastRefreshAndRestart();

            // Do not refresh if the UI is not visible. It will be refreshed later when the loaded event is called.
            if (IsVisible && Model.IsSolution)
            {
                var solutionModel = _detailModel as PackageSolutionDetailControlModel;
                if (solutionModel == null)
                {
                    return;
                }

                // get the list of projects
                IEnumerable<IProjectContextInfo> projects = solutionModel.Projects.Select(p => p.NuGetProject);
                Model.Context.Projects = projects;

                NuGetUIThreadHelper.JoinableTaskFactory.RunAsync(async () =>
                {
                    await RefreshWhenNotExecutingActionAsync(RefreshOperationSource.ProjectsChanged, timeSpan);
                }).PostOnFailure(nameof(PackageManagerControl), nameof(OnProjectChanged));
            }
            else
            {
                EmitRefreshEvent(timeSpan, RefreshOperationSource.ProjectsChanged, RefreshOperationStatus.NoOp, isUIFiltering: false, duration: 0);
            }
        }

        private void OnProjectActionsExecuted(object sender, IReadOnlyCollection<string> projectIds)
        {
            var timeSpan = GetTimeSinceLastRefreshAndRestart();
            // Do not refresh if the UI is not visible. It will be refreshed later when the loaded event is called.
            if (IsVisible)
            {
                NuGetUIThreadHelper.JoinableTaskFactory.RunAsync(async () =>
                {
                    if (Model.IsSolution)
                    {
                        await RefreshWhenNotExecutingActionAsync(RefreshOperationSource.ActionsExecuted, timeSpan);
                    }
                    else
                    {
                        await RefreshProjectAfterActionAsync(timeSpan, projectIds);
                    }
                }).PostOnFailure(nameof(PackageManagerControl), nameof(OnProjectActionsExecuted));
            }
            else
            {
                EmitRefreshEvent(timeSpan, RefreshOperationSource.ActionsExecuted, RefreshOperationStatus.NoOp);
            }
        }

        private async ValueTask RefreshProjectAfterActionAsync(TimeSpan timeSpan, IReadOnlyCollection<string> projectIds)
        {
            // this is a project package manager, so there is one and only one project.
            var project = Model.Context.Projects.First();

            if (projectIds.Contains(project.ProjectId, StringComparer.OrdinalIgnoreCase))
            {
                await RefreshWhenNotExecutingActionAsync(RefreshOperationSource.ActionsExecuted, timeSpan);
            }
            else
            {
                EmitRefreshEvent(timeSpan, RefreshOperationSource.ActionsExecuted, RefreshOperationStatus.NotApplicable);
            }
        }

        private void OnNuGetCacheUpdated(object sender, string e)
        {
            var timeSpan = GetTimeSinceLastRefreshAndRestart();
            // Do not refresh if the UI is not visible. It will be refreshed later when the loaded event is called.
            if (IsVisible)
            {
                NuGetUIThreadHelper.JoinableTaskFactory
                    .RunAsync(() => SolutionManager_CacheUpdatedAsync(timeSpan, e))
                    .PostOnFailure(nameof(PackageManagerControl), nameof(OnNuGetCacheUpdated));
            }
            else
            {
                EmitRefreshEvent(timeSpan, RefreshOperationSource.CacheUpdated, RefreshOperationStatus.NoOp);
            }
        }

        private async Task SolutionManager_CacheUpdatedAsync(TimeSpan timeSpan, string eventProjectFullName)
        {
            if (Model.IsSolution)
            {
                await RefreshWhenNotExecutingActionAsync(RefreshOperationSource.CacheUpdated, timeSpan);
            }
            else
            {
                // This is a project package manager, so there is one and only one project.
                IProjectContextInfo project = Model.Context.Projects.First();
                IProjectMetadataContextInfo projectMetadata = await project.GetMetadataAsync(
                    Model.Context.ServiceBroker,
                    CancellationToken.None);

                // This ensures that we refresh the UI only if the event.project.FullName matches the NuGetProject.FullName.
                // We also refresh the UI if projectFullPath is not present.
                if (projectMetadata.FullPath == eventProjectFullName)
                {
                    await RefreshWhenNotExecutingActionAsync(RefreshOperationSource.CacheUpdated, timeSpan);
                }
                else
                {
                    EmitRefreshEvent(timeSpan, RefreshOperationSource.CacheUpdated, RefreshOperationStatus.NotApplicable);
                }
            }
        }

        private async ValueTask RefreshWhenNotExecutingActionAsync(RefreshOperationSource source, TimeSpan timeSpanSinceLastRefresh)
        {
            // Only refresh if there is no executing action. Tell the operation execution to refresh when done otherwise.
            if (_isExecutingAction)
            {
                _isRefreshRequired = true;
                EmitRefreshEvent(timeSpanSinceLastRefresh, source, RefreshOperationStatus.NoOp, isUIFiltering: false, duration: 0);
            }
            else
            {
                await RunAndEmitRefreshAsync(async () => await RefreshAsync(), source, timeSpanSinceLastRefresh, Stopwatch.StartNew());
            }
        }

        private void EmitRefreshEvent(TimeSpan timeSpan, RefreshOperationSource refreshOperationSource, RefreshOperationStatus status, bool isUIFiltering = false, double? duration = null)
        {
            if (Model.IsSolution)
            {
                TelemetryActivity.EmitTelemetryEvent(PackageManagerUIRefreshEvent.ForSolution(
                    _sessionGuid,
                    refreshOperationSource,
                    status,
                    UIUtility.ToContractsItemFilter(_topPanel.Filter),
                    isUIFiltering,
                    timeSpan,
                    duration));
            }
            else
            {
                IProjectContextInfo project = Model.Context.Projects.First();
                TelemetryActivity.EmitTelemetryEvent(PackageManagerUIRefreshEvent.ForProject(
                    _sessionGuid,
                    refreshOperationSource,
                    status,
                    UIUtility.ToContractsItemFilter(_topPanel.Filter),
                    isUIFiltering,
                    timeSpan,
                    duration,
                    project.ProjectId,
                    project.ProjectKind));
            }
        }

        private void EmitPMUIClosingTelemetry()
        {
            TelemetryActivity.EmitTelemetryEvent(
                new PackageManagerCloseEvent(
                    _sessionGuid,
                    Model.IsSolution,
                    _topPanel.Filter.ToString(),
                    _installedTabTelemetryData));
        }

        private TimeSpan GetTimeSinceLastRefreshAndRestart()
        {
            TimeSpan elapsed;
            lock (_sinceLastRefresh)
            {
                elapsed = _sinceLastRefresh.Elapsed;
                _sinceLastRefresh.Restart();
            }
            return elapsed;
        }

        private void InitializeFilterList(UserSettings settings)
        {
            if (settings != null)
            {
                _topPanel.SelectFilter(settings.SelectedFilter);
            }
        }

        private void PackageManagerLoaded(object sender, RoutedEventArgs e)
        {
            NuGetUIThreadHelper.JoinableTaskFactory.RunAsync(() => PackageManagerLoadedAsync())
                .PostOnFailure(nameof(PackageManagerControl), nameof(PackageManagerLoaded));
        }

        private async Task PackageManagerLoadedAsync()
        {
            var sw = Stopwatch.StartNew();
            var timeSpan = GetTimeSinceLastRefreshAndRestart();
            // Do not trigger a refresh if this is not the first load of the control.
            // The loaded event is triggered once all the data binding has occurred, which effectively means we'll just display what was loaded earlier and not trigger another search
            if (!_loadedAndInitialized)
            {
                await RunAndEmitRefreshAsync(async () =>
                {
                    _loadedAndInitialized = true;
                    await SearchPackagesAndRefreshUpdateCountAsync(useCacheForUpdates: false);
                },
                RefreshOperationSource.PackageManagerLoaded, timeSpan, sw);
            }
            else
            {
                EmitRefreshEvent(timeSpan, RefreshOperationSource.PackageManagerLoaded, RefreshOperationStatus.NoOp, isUIFiltering: false, 0);
            }
            await RefreshConsolidatablePackagesCountAsync();
        }

        private void PackageManagerUnloaded(object sender, RoutedEventArgs e)
        {
            Unloaded -= PackageManagerUnloaded;
        }

        private static bool IsUILegalDisclaimerSuppressed()
        {
            return RegistrySettingUtility.GetBooleanSetting(Constants.SuppressUIDisclaimerRegistryName);
        }

        protected static DependencyBehavior? GetDependencyBehaviorFromConfig(ISettings nugetSettings)
        {
            if (nugetSettings != null)
            {
                var configSection = nugetSettings.GetSection(ConfigurationConstants.Config);
                var dependencySetting = configSection?.GetFirstItemWithAttribute<AddItem>(ConfigurationConstants.KeyAttribute, ConfigurationConstants.DependencyVersion);

                DependencyBehavior behavior;
                var success = Enum.TryParse(dependencySetting?.Value, ignoreCase: true, result: out behavior);
                if (success)
                {
                    return behavior;
                }
            }

            return null;
        }

        private void SetSelectedDepencyBehavior(DependencyBehavior dependencyBehavior)
        {
            var selectedDependencyBehavior = _detailModel.Options.DependencyBehaviors
                .FirstOrDefault(d => d.Behavior == dependencyBehavior);
            if (selectedDependencyBehavior != null)
            {
                _detailModel.Options.SelectedDependencyBehavior = selectedDependencyBehavior;
            }
        }

        public void ApplyShowPreviewSetting(bool show)
        {
            _detailModel.Options.ShowPreviewWindow = show;
        }

        public void ApplyShowDeprecatedFrameworkSetting(bool show)
        {
            _detailModel.Options.ShowDeprecatedFrameworkWindow = show;
        }

        private void ApplySettings(UserSettings settings, ISettings nugetSettings)
        {
            var dependencySetting = GetDependencyBehaviorFromConfig(nugetSettings);
            if (settings == null)
            {
                // set depency behavior to the value from nugetSettings
                SetSelectedDepencyBehavior(dependencySetting ?? DependencyBehavior.Lowest);
                return;
            }

            _detailModel.Options.ShowPreviewWindow = settings.ShowPreviewWindow;
            _detailModel.Options.ShowDeprecatedFrameworkWindow = settings.ShowDeprecatedFrameworkWindow;
            _detailModel.Options.RemoveDependencies = settings.RemoveDependencies;
            _detailModel.Options.ForceRemove = settings.ForceRemove;
            _topPanel.CheckboxPrerelease.IsChecked = settings.IncludePrerelease;
            _packageDetail._optionsControl.IsExpanded = settings.OptionsExpanded;
            _packageDetail._solutionView.RestoreUserSettings(settings);

            SetSelectedDepencyBehavior(dependencySetting ?? settings.DependencyBehavior);

            var selectedFileConflictAction = _detailModel.Options.FileConflictActions.
                FirstOrDefault(a => a.Action == settings.FileConflictAction);
            if (selectedFileConflictAction != null)
            {
                _detailModel.Options.SelectedFileConflictAction = selectedFileConflictAction;
            }
        }

        private void PackageSourcesChanged(object sender, IReadOnlyCollection<PackageSourceContextInfo> e)
        {
            // Set _dontStartNewSearch to true to prevent a new search started in
            // _sourceRepoList_SelectionChanged(). This method will start the new
            // search when needed by itself.
            _dontStartNewSearch = true;
            TimeSpan timeSpan = GetTimeSinceLastRefreshAndRestart();

            NuGetUIThreadHelper.JoinableTaskFactory.RunAsync(() => PackageSourcesChangedAsync(e, timeSpan))
                .PostOnFailure(nameof(PackageManagerControl), nameof(PackageSourcesChanged));
        }

        private async Task PackageSourcesChangedAsync(IReadOnlyCollection<PackageSourceContextInfo> packageSources, TimeSpan timeSpan)
        {
            try
            {
                var sw = Stopwatch.StartNew();
                IReadOnlyCollection<PackageSourceMoniker> list = await PackageSourceMoniker.PopulateListAsync(packageSources, CancellationToken.None);

                await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                // We access UI components in these calls
                PackageSourceMoniker prevSelectedItem = SelectedSource;

                await PopulatePackageSourcesAsync(list, optionalSelectSourceName: null, cancellationToken: CancellationToken.None);

                // force a new search explicitly only if active source has changed
                if (prevSelectedItem == SelectedSource)
                {
                    sw.Stop();
                    EmitRefreshEvent(timeSpan, RefreshOperationSource.PackageSourcesChanged, RefreshOperationStatus.NotApplicable, isUIFiltering: false, duration: sw.Elapsed.TotalMilliseconds);
                }
                else
                {
                    await RunAndEmitRefreshAsync(async () =>
                    {
                        SaveSettings();
                        await SearchPackagesAndRefreshUpdateCountAsync(useCacheForUpdates: false);
                    }, RefreshOperationSource.PackageSourcesChanged, timeSpan, sw);
                }
            }
            finally
            {
                _dontStartNewSearch = false;
            }
        }

        private async Task IsCentralPackageManagementEnabledAsync(CancellationToken cancellationToken)
        {
            if (!Model.IsSolution)
            {
                await NuGetUIThreadHelper.JoinableTaskFactory.RunAsync(async delegate
                {
                    // Go off the UI thread to perform non-UI operations
                    await TaskScheduler.Default;
                    _detailModel.IsCentralPackageManagementEnabled = await Model.Context.Projects.First().IsCentralPackageManagementEnabledAsync(Model.Context.ServiceBroker, cancellationToken);
                });
            }
        }

        private async Task<string> GetSettingsKeyAsync(CancellationToken cancellationToken)
        {
            string key;

            if (Model.IsSolution)
            {
                key = "solution";
            }
            else
            {
                IProjectContextInfo project = Model.Context.Projects.First();
                IProjectMetadataContextInfo projectMetadata = await project.GetMetadataAsync(
                    Model.Context.ServiceBroker,
                    cancellationToken);

                return GetProjectSettingsKey(projectMetadata.Name);
            }

            return key;
        }

        private static string GetProjectSettingsKey(string projectName)
        {
            string value;

            if (string.IsNullOrEmpty(projectName))
            {
                value = "unknown";
            }
            else
            {
                value = projectName;
            }

            return "project:" + value;
        }

        // Save the settings of this doc window in the UIContext. Note that the settings
        // are not guaranteed to be persisted. We need to call Model.Context.SaveSettings()
        // to persist the settings.
        public void SaveSettings()
        {
            Assumes.NotNullOrEmpty(_settingsKey);

            var settings = new UserSettings
            {
                SourceRepository = SelectedSource?.SourceName,
                ShowPreviewWindow = _detailModel.Options.ShowPreviewWindow,
                ShowDeprecatedFrameworkWindow = _detailModel.Options.ShowDeprecatedFrameworkWindow,
                RemoveDependencies = _detailModel.Options.RemoveDependencies,
                ForceRemove = _detailModel.Options.ForceRemove,
                DependencyBehavior = _detailModel.Options.SelectedDependencyBehavior.Behavior,
                FileConflictAction = _detailModel.Options.SelectedFileConflictAction.Action,
                IncludePrerelease = _topPanel.CheckboxPrerelease.IsChecked == true,
                SelectedFilter = _topPanel.Filter,
                OptionsExpanded = _packageDetail._optionsControl.IsExpanded
            };
            _packageDetail._solutionView.SaveSettings(settings);

            Model.Context.UserSettingsManager.AddSettings(_settingsKey, settings);
        }

        private UserSettings LoadSettings()
        {
            Assumes.NotNullOrEmpty(_settingsKey);

            UserSettings settings = Model.Context.UserSettingsManager.GetSettings(_settingsKey);

            if (PreviewWindow.IsDoNotShowPreviewWindowEnabled())
            {
                settings.ShowPreviewWindow = false;
            }

            if (DotnetDeprecatedPrompt.GetDoNotShowPromptState())
            {
                settings.ShowDeprecatedFrameworkWindow = false;
            }

            return settings;
        }

        private void AddRestoreBar()
        {
            if (Model.Context.PackageRestoreManager != null)
            {
                var projectContextInfo = !Model.IsSolution ? Model.Context.Projects.FirstOrDefault() : null;
                RestoreBar = new PackageRestoreBar(Model.Context.SolutionManagerService, Model.Context.PackageRestoreManager, projectContextInfo);
                DockPanel.SetDock(RestoreBar, Dock.Top);

                _root.Children.Insert(0, RestoreBar);

                Model.Context.PackageRestoreManager.PackagesMissingStatusChanged += PackageRestoreManager_PackagesMissingStatusChanged;
            }
        }

        private void RemoveRestoreBar()
        {
            if (RestoreBar != null)
            {
                RestoreBar.CleanUp();
                Model.Context.PackageRestoreManager.PackagesMissingStatusChanged -= PackageRestoreManager_PackagesMissingStatusChanged;
            }
        }

        private async Task AddRestartRequestBarAsync()
        {
            if (Model.Context.PackageManager.DeleteOnRestartManager != null)
            {
                var vsShell = await ServiceLocator.GetGlobalServiceAsync<SVsShell, IVsShell4>();
                Assumes.NotNull(vsShell);

                _restartBar = new RestartRequestBar(Model.Context.PackageManager.DeleteOnRestartManager, vsShell);
                DockPanel.SetDock(_restartBar, Dock.Top);

                _root.Children.Insert(0, _restartBar);
            }
        }

        private void RemoveRestartBar()
        {
            if (_restartBar != null)
            {
                _restartBar.CleanUp();

                Model.Context.PackageRestoreManager.PackagesMissingStatusChanged
                    -= PackageRestoreManager_PackagesMissingStatusChanged;
            }
        }

        private void AddMigratorBar()
        {
            _migratorBar = new PRMigratorBar(Model);

            DockPanel.SetDock(_migratorBar, Dock.Top);

            _root.Children.Insert(0, _migratorBar);
        }

        private void PackageRestoreManager_PackagesMissingStatusChanged(object sender, PackagesMissingStatusEventArgs e)
        {
            // TODO: PackageRestoreManager fires this event even when solution is closed.
            // Don't do anything if solution is closed.
            // Add MissingPackageStatus to keep previous packageMissing status to avoid unnecessarily refresh
            // only when package is missing last time and is not missing this time, we need to refresh
            // Note: This event is not triggered on PackageReference projects. This triggers in packages.config projects
            if (!e.PackagesMissing && _missingPackageStatus)
            {
                NuGetUIThreadHelper.JoinableTaskFactory.RunAsync(async () =>
                {
                    await RunAndEmitRefreshAsync(async () => await RefreshAsync(), RefreshOperationSource.PackagesMissingStatusChanged, GetTimeSinceLastRefreshAndRestart(), Stopwatch.StartNew());
                }).PostOnFailure(nameof(PackageManagerControl), nameof(PackageRestoreManager_PackagesMissingStatusChanged));
            }

            _missingPackageStatus = e.PackagesMissing;
        }

        private async Task SetTitleAsync(IProjectMetadataContextInfo projectMetadata = null)
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            if (Model.IsSolution)
            {
                _topPanel.Title = Resx.Resources.Label_SolutionPackageManager;
            }
            else
            {
                string projectName;

                if (projectMetadata is null)
                {
                    IProjectContextInfo project = Model.Context.Projects.First();
                    IProjectMetadataContextInfo metadata = await project.GetMetadataAsync(
                        Model.Context.ServiceBroker,
                        CancellationToken.None);

                    projectName = metadata.Name;
                }
                else
                {
                    projectName = projectMetadata.Name;
                }

                if (string.IsNullOrWhiteSpace(projectName))
                {
                    projectName = "unknown";
                }

                _topPanel.Title = string.Format(
                    CultureInfo.CurrentCulture,
                    Resx.Resources.Label_PackageManager,
                    projectName);
            }
        }

        private async Task InitPackageSourcesAsync(UserSettings settings, CancellationToken cancellationToken)
        {
            // get active source name.
            string activeSourceName;

            // try saved user settings first.
            if (!string.IsNullOrEmpty(settings?.SourceRepository))
            {
                activeSourceName = settings.SourceRepository;
            }
            else
            {
                // no user settings found. Then use the active source from PackageSourceProvider.
                activeSourceName = await Model.Context.SourceService.GetActivePackageSourceNameAsync(cancellationToken);
            }

            await PopulatePackageSourcesAsync(activeSourceName, cancellationToken);
        }

        private ValueTask PopulatePackageSourcesAsync(IReadOnlyCollection<PackageSourceMoniker> packageSourceMonikers, string optionalSelectSourceName, CancellationToken cancellationToken)
        {
            // init source repo list
            _topPanel.PackageSources.Clear();

            var selectedSourceName = optionalSelectSourceName ?? SelectedSource?.SourceName;

            foreach (PackageSourceMoniker packageSourceMoniker in packageSourceMonikers)
            {
                _topPanel.PackageSources.Add(packageSourceMoniker);
            }

            if (selectedSourceName != null)
            {
                SelectedSource = PackageSources
                    // if the old active source still exists. Keep it as the active source.
                    .FirstOrDefault(i => StringComparer.OrdinalIgnoreCase.Equals(i.SourceName, selectedSourceName))
                    // If the old active source does not exist any more. In this case,
                    // use the first (non-aggregate) enabled source as the active source.
                    ?? PackageSources.FirstOrDefault(psm => !psm.IsAggregateSource);
            }
            else
            {
                // use the first enabled source as the active source by default, but do not choose "All sources"!
                SelectedSource = PackageSources.FirstOrDefault(psm => !psm.IsAggregateSource);
            }

            return new ValueTask();
        }

        private async ValueTask PopulatePackageSourcesAsync(string optionalSelectSourceName, CancellationToken cancellationToken)
        {
            IReadOnlyCollection<PackageSourceMoniker> packageSourceMonikers = await PackageSourceMoniker.PopulateListAsync(
                _serviceBroker,
                cancellationToken);

            await PopulatePackageSourcesAsync(packageSourceMonikers, optionalSelectSourceName, cancellationToken);
        }

        private async ValueTask SearchPackagesAndRefreshUpdateCountAsync(bool useCacheForUpdates)
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            // Set a new cancellation token source which will be used to cancel this task in case
            // new loading task starts or manager ui is closed while loading packages.
            var loadCts = new CancellationTokenSource();
            var oldCts = Interlocked.Exchange(ref _loadCts, loadCts);
            oldCts?.Cancel();
            oldCts?.Dispose();

            await SearchPackagesAndRefreshUpdateCountAsync(
                searchText: _windowSearchHost.SearchQuery.SearchString,
                useCachedPackageMetadata: useCacheForUpdates,
                pSearchCallback: null,
                searchTask: null);
        }

        /// <summary>
        /// This method is called from several event handlers. So, consolidating the use of JTF.Run in this method
        /// </summary>
        internal async Task SearchPackagesAndRefreshUpdateCountAsync(string searchText, bool useCachedPackageMetadata, IVsSearchCallback pSearchCallback, IVsSearchTask searchTask)
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            var sw = Stopwatch.StartNew();
            ItemFilter filterToRender = _topPanel.Filter;

            var loadContext = new PackageLoadContext(Model.IsSolution, Model.Context);

            if (useCachedPackageMetadata)
            {
                loadContext.CachedPackages = Model.CachedUpdates;
            }
            else // Invalidate cache
            {
                Model.CachedUpdates = null;
            }

            try
            {
                _packageList.ClearPackageLevelGrouping();

                bool useRecommender = GetUseRecommendedPackages(loadContext, searchText);
                var loader = await PackageItemLoader.CreateAsync(
                    Model.Context.ServiceBroker,
                    Model.Context.NuGetSearchService,
                    loadContext,
                    SelectedSource.PackageSources,
                    (NuGet.VisualStudio.Internal.Contracts.ItemFilter)_topPanel.Filter,
                    searchText: searchText,
                    includePrerelease: IncludePrerelease,
                    useRecommender: useRecommender,
                    _packageVulnerabilityService);

                var loadingMessage = string.IsNullOrWhiteSpace(searchText)
                    ? Resx.Resources.Text_Loading
                    : string.Format(CultureInfo.CurrentCulture, Resx.Resources.Text_Searching, searchText);

                // Set a new cancellation token source which will be used to cancel this task in case
                // new loading task starts or manager ui is closed while loading packages.
                _loadCts = new CancellationTokenSource();

                // start SearchAsync task for initial loading of packages
                var searchResultTask = loader.SearchAsync(cancellationToken: _loadCts.Token);
                // this will wait for searchResultTask to complete instead of creating a new task
                await _packageList.LoadItemsAsync(loader, loadingMessage, _uiLogger, searchResultTask, _loadCts.Token);

                if (ActiveFilter == ItemFilter.Installed)
                {
                    _packageList.AddPackageLevelGrouping();
                }

                if (pSearchCallback != null && searchTask != null)
                {
                    var searchResult = await searchResultTask;
                    pSearchCallback.ReportComplete(searchTask, (uint)searchResult.PackageSearchItems.Count);
                }

                // When not using Cache, refresh all Counts.
                if (!useCachedPackageMetadata)
                {
                    await RefreshInstalledAndUpdatesTabsAsync();
                }
            }
            catch (OperationCanceledException)
            {
                // Invalidate cache.
                Model.CachedUpdates = null;
            }
        }

        private bool GetUseRecommendedPackages(PackageLoadContext loadContext, string searchText)
        {
            // only make recommendations when
            //   one of the source repositories is nuget.org,
            //   the package manager was opened for a project, not a solution,
            //   this is the Browse tab,
            //   and the search text is an empty string
            return (loadContext.IsSolution == false
                    && _topPanel.Filter == ItemFilter.All
                    && searchText == string.Empty
                    && SelectedSource.PackageSources.Any(item => UriUtility.IsNuGetOrg(item.Source)));
        }

        private async ValueTask RefreshInstalledAndUpdatesTabsAsync()
        {
            // clear existing caches
            Model.CachedUpdates = null;

            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            _topPanel.UpdateWarningStatusOnInstalledTab(installedVulnerablePackagesCount: 0, installedDeprecatedPackagesCount: 0);
            _topPanel.UpdateCountOnUpdatesTab(count: 0);
            var loadContext = new PackageLoadContext(Model.IsSolution, Model.Context);
            var loader = await PackageItemLoader.CreateAsync(
                Model.Context.ServiceBroker,
                Model.Context.NuGetSearchService,
                loadContext,
                SelectedSource.PackageSources,
                NuGet.VisualStudio.Internal.Contracts.ItemFilter.UpdatesAvailable,
                includePrerelease: IncludePrerelease,
                useRecommender: false);

            // cancel previous refresh tabs task, if any and start a new one.
            var refreshCts = new CancellationTokenSource();
            Interlocked.Exchange(ref _refreshCts, refreshCts)?.Cancel();

            // Update installed tab warning icon
            (int vulnerablePackages, int deprecatedPackages) = await GetInstalledVulnerableAndDeprecatedPackagesCountAsync(loadContext, SelectedSource.PackageSources, refreshCts.Token);
            _topPanel.UpdateWarningStatusOnInstalledTab(vulnerablePackages, deprecatedPackages);

            // Update updates tab count
            Model.CachedUpdates = new PackageSearchMetadataCache
            {
                Packages = await loader.GetInstalledAndTransitivePackagesAsync(refreshCts.Token),
                IncludePrerelease = IncludePrerelease
            };

            // Update updates tab count
            _topPanel.UpdateCountOnUpdatesTab(Model.CachedUpdates.Packages.Count);
        }

        private async Task<(int, int)> GetInstalledVulnerableAndDeprecatedPackagesCountAsync(PackageLoadContext loadContext, IReadOnlyCollection<PackageSourceContextInfo> packageSources, CancellationToken token)
        {
            // Switch off the UI thread before fetching installed packages and deprecation metadata.
            await TaskScheduler.Default;

            int vulnerablePackagesCount = 0;
            int deprecatedPackagesCount = 0;
            PackageCollection installedPackageCollection = null;

            // Transitive dependencies are only displayed in Project-level PM UI.
            if (Model.IsSolution)
            {
                installedPackageCollection = await loadContext.GetInstalledPackagesAsync();
            }
            else
            {
                IInstalledAndTransitivePackages installedAndTransitivePackages = await PackageCollection.GetInstalledAndTransitivePackagesAsync(loadContext.ServiceBroker, loadContext.Projects, includeTransitiveOrigins: true, token);
                installedPackageCollection = PackageCollection.FromPackageReferences(installedAndTransitivePackages.InstalledPackages);
                PackageCollection transitivePackageCollection = PackageCollection.FromPackageReferences(installedAndTransitivePackages.TransitivePackages.Where(p => p.TransitiveOrigins.Any()));
                IEnumerable<PackageVulnerabilityMetadataContextInfo>[] transitivePackageVulnerabilities = await Task.WhenAll(transitivePackageCollection.Select(p => _packageVulnerabilityService.GetVulnerabilityInfoAsync(p, token)));

                foreach (IEnumerable<PackageVulnerabilityMetadataContextInfo> vulnerabilityInfo in transitivePackageVulnerabilities)
                {
                    if (vulnerabilityInfo != null && vulnerabilityInfo.Any())
                    {
                        vulnerablePackagesCount++;
                    }
                }
            }

            var installedPackageMetadata = await Task.WhenAll(installedPackageCollection.Select(p => GetPackageMetadataAsync(p, packageSources, token)));

            foreach ((PackageSearchMetadataContextInfo s, PackageDeprecationMetadataContextInfo d) in installedPackageMetadata)
            {
                if (s.Vulnerabilities != null && s.Vulnerabilities.Any())
                {
                    vulnerablePackagesCount++;
                }
                if (d != null)
                {
                    deprecatedPackagesCount++;
                }
            }

            return (vulnerablePackagesCount, deprecatedPackagesCount);
        }

        private async Task<(PackageSearchMetadataContextInfo, PackageDeprecationMetadataContextInfo)> GetPackageMetadataAsync(PackageCollectionItem package, IReadOnlyCollection<PackageSourceContextInfo> packageSources, CancellationToken cancellationToken)
        {
            using (INuGetSearchService searchService = await _serviceBroker.GetProxyAsync<INuGetSearchService>(NuGetServices.SearchService, cancellationToken: cancellationToken))
            {
                Assumes.NotNull(searchService);
                return await searchService.GetPackageMetadataAsync(package, packageSources, true, cancellationToken);
            }
        }

        private async ValueTask RefreshConsolidatablePackagesCountAsync()
        {
            if (Model.IsSolution)
            {
                await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                _topPanel.UpdateCountOnConsolidateTab(count: 0);
                var loadContext = new PackageLoadContext(Model.IsSolution, Model.Context);
                var loader = await PackageItemLoader.CreateAsync(
                    Model.Context.ServiceBroker,
                    Model.Context.NuGetSearchService,
                    loadContext,
                    SelectedSource.PackageSources,
                    NuGet.VisualStudio.Internal.Contracts.ItemFilter.Consolidate,
                    includePrerelease: IncludePrerelease,
                    useRecommender: false);

                _topPanel.UpdateCountOnConsolidateTab(await loader.GetTotalCountAsync(maxCount: 100, CancellationToken.None));
            }
        }

        private void SettingsButtonClicked(object sender, EventArgs e)
        {
            Model.UIController.LaunchNuGetOptionsDialog(OptionsPage.PackageSources);
        }

        private void PackageList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IncrementInstalledPackageSelectionCount();

            var loadCts = new CancellationTokenSource();
            var oldCts = Interlocked.Exchange(ref _cancelSelectionChangedSource, loadCts);
            oldCts?.Cancel();
            oldCts?.Dispose();

            NuGetUIThreadHelper.JoinableTaskFactory
                .RunAsync(async () =>
                {
                    try
                    {
                        await UpdateDetailPaneAsync(loadCts.Token);
                    }
                    catch (OperationCanceledException) when (loadCts.Token.IsCancellationRequested)
                    {
                        // Expected
                    }
                })
                .PostOnFailure(nameof(PackageManagerControl), nameof(PackageList_SelectionChanged));
        }

        /// <summary>
        /// Updates the detail pane based on the selected package
        /// </summary>
        internal async Task UpdateDetailPaneAsync(CancellationToken cancellationToken)
        {
            PackageItemViewModel selectedItem = _packageList.SelectedItem;
            IReadOnlyCollection<PackageSourceContextInfo> packageSources = SelectedSource.PackageSources;
            int selectedIndex = _packageList.SelectedIndex;
            int recommendedCount = _packageList.PackageItems.Where(item => item.Recommended == true).Count();

            if (selectedItem == null)
            {
                _packageDetail.Visibility = Visibility.Hidden;
            }
            else
            {
                _packageDetail.Visibility = Visibility.Visible;
                _packageDetail.DataContext = _detailModel;

                EmitSearchSelectionTelemetry(selectedItem);

                await _detailModel.SetCurrentPackageAsync(selectedItem, _topPanel.Filter, () => _packageList.SelectedItem);
                Model.UIController.SelectedPackageId = selectedItem.Id;
                _detailModel.SetCurrentSelectionInfo(selectedIndex, recommendedCount, _recommendPackages, selectedItem.RecommenderVersion);

                _packageDetail.ScrollToHome();

                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }
            }
        }

        private void EmitSearchSelectionTelemetry(PackageItemViewModel selectedPackage)
        {
            var operationId = _packageList.OperationId;
            var selectedIndex = _packageList.SelectedIndex;
            var recommendedCount = _packageList.PackageItems.Where(item => item.Recommended == true).Count();
            var hasDeprecationAlternative = selectedPackage.DeprecationMetadata?.AlternatePackage != null;

            if (_topPanel.Filter == ItemFilter.All
                && operationId.HasValue
                && selectedIndex >= 0)
            {
                TelemetryActivity.EmitTelemetryEvent(new SearchSelectionTelemetryEvent(
                    operationId.Value,
                    recommendedCount,
                    selectedIndex,
                    selectedPackage.Id,
                    selectedPackage.Version,
                    selectedPackage.IsPackageVulnerable,
                    selectedPackage.IsPackageDeprecated,
                    hasDeprecationAlternative));
            }
        }

        private void IncrementInstalledPackageSelectionCount()
        {
            PackageItemViewModel selectedItem = _packageList.SelectedItem;
            if (selectedItem == null || ActiveFilter != ItemFilter.Installed)
            {
                return;
            }

            if (selectedItem.PackageLevel == PackageLevel.TopLevel)
            {
                _installedTabTelemetryData.TopLevelPackageSelectedCount++;
            }
            else if (selectedItem.PackageLevel == PackageLevel.Transitive)
            {
                _installedTabTelemetryData.TransitivePackageSelectedCount++;
            }
        }

        private void PackageList_GroupExpansionChanged(object sender, RoutedEventArgs e)
        {
            if (ActiveFilter == ItemFilter.Installed && sender is Expander expander && expander.Tag is PackageLevel pkgLevel)
            {
                if (pkgLevel == PackageLevel.TopLevel)
                {
                    if (expander.IsExpanded)
                    {
                        _installedTabTelemetryData.TopLevelPackagesExpandedCount++;
                    }
                    else
                    {
                        _installedTabTelemetryData.TopLevelPackagesCollapsedCount++;
                    }
                }
                else if (pkgLevel == PackageLevel.Transitive)
                {
                    if (expander.IsExpanded)
                    {
                        _installedTabTelemetryData.TransitivePackagesExpandedCount++;
                    }
                    else
                    {
                        _installedTabTelemetryData.TransitivePackagesCollapsedCount++;
                    }
                }
            }
        }

        private void SourceRepoList_SelectionChanged(object sender, EventArgs e)
        {
            var timeSpan = GetTimeSinceLastRefreshAndRestart();

            _detailModel.PackageSourceMappingViewModel.SettingsChanged();

            if (_dontStartNewSearch || !_initialized)
            {
                EmitRefreshEvent(timeSpan, RefreshOperationSource.SourceSelectionChanged, RefreshOperationStatus.NoOp);
                return;
            }

            if (SelectedSource != null)
            {
                _topPanel.SourceToolTip.Visibility = Visibility.Visible;
                _topPanel.SourceToolTip.DataContext = SelectedSource.GetTooltip();

                NuGetUIThreadHelper.JoinableTaskFactory.RunAsync(() => SourceRepoList_SelectionChangedAsync(timeSpan))
                    .PostOnFailure(nameof(PackageManagerControl), nameof(SourceRepoList_SelectionChanged));
            }
        }

        private async Task SourceRepoList_SelectionChangedAsync(TimeSpan timeSpan)
        {
            await RunAndEmitRefreshAsync(async () =>
            {
                SaveSettings();
                await SearchPackagesAndRefreshUpdateCountAsync(useCacheForUpdates: false);
            }, RefreshOperationSource.SourceSelectionChanged, timeSpan, Stopwatch.StartNew());
        }

        private void Filter_SelectionChanged(object sender, FilterChangedEventArgs e)
        {
            if (_initialized)
            {
                var timeSpan = GetTimeSinceLastRefreshAndRestart();
                _packageList.ResetLoadingStatusIndicator();
                var sw = Stopwatch.StartNew();
                // Collapse the Update controls when the current tab is not "Updates".
                _packageList.CheckBoxesEnabled = _topPanel.Filter == ItemFilter.UpdatesAvailable;
                _packageList._updateButtonContainer.Visibility = _topPanel.Filter == ItemFilter.UpdatesAvailable ? Visibility.Visible : Visibility.Collapsed;

                // Set a new cancellation token source which will be used to cancel this task in case
                // new loading task starts or manager ui is closed while loading packages.
                var loadCts = new CancellationTokenSource();
                var oldCts = Interlocked.Exchange(ref _loadCts, loadCts);
                oldCts?.Cancel();
                oldCts?.Dispose();

                NuGetUIThreadHelper.JoinableTaskFactory.RunAsync(async () =>
                {
                    await RunAndEmitRefreshAsync(async () =>
                    {
                        await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                        _packageList.ClearPackageLevelGrouping();
                        await SearchPackagesAndRefreshUpdateCountAsync(useCacheForUpdates: true);
                    }, RefreshOperationSource.FilterSelectionChanged, timeSpan, sw);
                    _detailModel.OnFilterChanged(e.PreviousFilter, _topPanel.Filter);
                }).PostOnFailure(nameof(PackageManagerControl), nameof(Filter_SelectionChanged));
            }
        }

        /// <summary>
        /// Refreshes the control after packages are installed or uninstalled.
        /// </summary>
        private async ValueTask RefreshAsync()
        {
            if (_topPanel.Filter != ItemFilter.All)
            {
                // refresh the whole package list
                await SearchPackagesAndRefreshUpdateCountAsync(useCacheForUpdates: false);
            }
            else
            {
                await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                PackageCollection installedPackages = await PackageCollection.FromProjectsAsync(
                    Model.Context.ServiceBroker,
                    Model.Context.Projects,
                    CancellationToken.None);
                _packageList.UpdatePackageStatus(installedPackages.ToArray());

                await RefreshInstalledAndUpdatesTabsAsync();
            }

            await RefreshConsolidatablePackagesCountAsync();

            _packageDetail?.Refresh();
        }

        private void CheckboxPrerelease_CheckChanged(object sender, EventArgs e)
        {
            if (!_initialized)
            {
                return;
            }

            var sw = Stopwatch.StartNew();
            var timeSpan = GetTimeSinceLastRefreshAndRestart();
            RegistrySettingUtility.SetBooleanSetting(Constants.IncludePrereleaseRegistryName, _topPanel.CheckboxPrerelease.IsChecked == true);
            NuGetUIThreadHelper.JoinableTaskFactory.RunAsync(async () =>
            {
                await RunAndEmitRefreshAsync(async () => await SearchPackagesAndRefreshUpdateCountAsync(useCacheForUpdates: false),
                    RefreshOperationSource.CheckboxPrereleaseChanged, timeSpan, sw);
            }).PostOnFailure(nameof(PackageManagerControl), nameof(CheckboxPrerelease_CheckChanged));
        }

        private void CheckboxVulnerabilties_CheckChanged(object sender, EventArgs e)
        {
            if (!_initialized)
            {
                return;
            }

            if (IsVulnerableFilteringApplied)
            {
                _packageList.AddVulnerabilitiesFiltering();
            }
            else
            {
                _packageList.RemoveVulnerabilitiesFiltering();
            }
        }

        private async Task RunAndEmitRefreshAsync(Func<Task> runner, RefreshOperationSource source, TimeSpan lastRefresh, Stopwatch sw, bool isUIFiltering = false)
        {
            var refreshStatus = RefreshOperationStatus.NoOp;
            try
            {
                await runner();
                refreshStatus = RefreshOperationStatus.Success;
            }
            catch
            {
                refreshStatus = RefreshOperationStatus.Failed;
                throw;
            }
            finally
            {
                sw.Stop();
                EmitRefreshEvent(lastRefresh, source, refreshStatus, isUIFiltering, sw.Elapsed.TotalMilliseconds);
            }
        }

        internal class SearchQuery : IVsSearchQuery
        {
            public uint GetTokens(uint dwMaxTokens, IVsSearchToken[] rgpSearchTokens)
            {
                return 0;
            }

            public uint ParseError
            {
                get { return 0; }
            }

            public string SearchString { get; set; }
        }

        public Guid Category
        {
            get { return Guid.Empty; }
        }

        public void ClearSearch()
        {
            var sw = Stopwatch.StartNew();
            TimeSpan timeSpan = GetTimeSinceLastRefreshAndRestart();
            NuGetUIThreadHelper.JoinableTaskFactory.RunAsync(async () =>
            {
                await RunAndEmitRefreshAsync(async () => await SearchPackagesAndRefreshUpdateCountAsync(useCacheForUpdates: true), RefreshOperationSource.ClearSearch, timeSpan, sw);
            }).PostOnFailure(nameof(PackageManagerControl), nameof(ClearSearch));
        }

        public IVsSearchTask CreateSearch(uint dwCookie, IVsSearchQuery pSearchQuery, IVsSearchCallback pSearchCallback)
        {
            var searchTask = new NuGetPackageManagerControlSearchTask(this, dwCookie, pSearchQuery, pSearchCallback);
            return searchTask;
        }

        public bool OnNavigationKeyDown(uint dwNavigationKey, uint dwModifiers)
        {
            // We are not interesting in intercepting navigation keys, so return "not handled"
            return false;
        }

        public void ProvideSearchSettings(IVsUIDataSource pSearchSettings)
        {
            // pSearchSettings is of type SearchSettingsDataSource. We use dynamic here
            // so that the code can be run on both dev12 & dev14. If we use the type directly,
            // there will be type mismatch error.
            dynamic settings = pSearchSettings;
            settings.ControlMinWidth = (uint)_topPanel.SearchControlParent.MinWidth;
            settings.ControlMaxWidth = uint.MaxValue;
            settings.SearchWatermark = GetSearchText();
        }

        // Returns the text to be displayed in the search box.
        private string GetSearchText()
        {
            var focusOnSearchKeyGesture = (KeyGesture)InputBindings.OfType<KeyBinding>().First(
                x => x.Command == Commands.FocusOnSearchBox).Gesture;
            return string.Format(CultureInfo.CurrentCulture,
                Resx.Resources.Text_SearchBoxText,
                focusOnSearchKeyGesture.GetDisplayStringForCulture(CultureInfo.CurrentCulture));
        }

        public bool SearchEnabled
        {
            get { return true; }
        }

        public IVsEnumWindowSearchFilters SearchFiltersEnum
        {
            get { return null; }
        }

        public IVsEnumWindowSearchOptions SearchOptionsEnum
        {
            get { return null; }
        }

        private void FocusOnSearchBox_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            NuGetUIThreadHelper.JoinableTaskFactory.RunAsync(async () =>
            {
                await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                _windowSearchHost.Activate();
            })
            .PostOnFailure(nameof(PackageManagerControl), nameof(FocusOnSearchBox_Executed));
        }

        public void Search(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                return;
            }

            NuGetUIThreadHelper.JoinableTaskFactory.Run(async () =>
            {
                await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                _windowSearchHost.Activate();
                _windowSearchHost.SearchAsync(new SearchQuery { SearchString = searchText });
            });
        }

        public void ShowUpdatePackages(ShowUpdatePackageOptions updatePackageOptions)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (updatePackageOptions == null)
            {
                return;
            }

            if (_topPanel.Filter == ItemFilter.UpdatesAvailable)
            {
                SelectMatchingUpdatePackages(updatePackageOptions);
            }
            else if (_topPanel.Filter == ItemFilter.Installed)
            {
                _topPanel.SelectFilter(ItemFilter.UpdatesAvailable);
                SelectMatchingUpdatePackages(updatePackageOptions);
            }
            else
            {
                _topPanel.SelectFilter(ItemFilter.UpdatesAvailable);

                // Hook up an event handler to delay selecting packages until they've all loaded, then unhook
                // the event handler so it doesn't keep happening on every refresh, only the initial load.
                EventHandler handler = null;
                handler = (s, e) =>
                {
                    _packageList.LoadItemsCompleted -= handler;
                    SelectMatchingUpdatePackages(updatePackageOptions);
                };
                _packageList.LoadItemsCompleted += handler;
            }
        }

        private void SelectMatchingUpdatePackages(ShowUpdatePackageOptions updatePackageOptions)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (updatePackageOptions == null)
            {
                return;
            }

            if (updatePackageOptions.ShouldUpdateAllPackages)
            {
                foreach (var packageItem in _packageList.PackageItems)
                {
                    packageItem.IsSelected = true;
                }
            }
            else if (updatePackageOptions.PackagesToUpdate.Any())
            {
                var packagesToSelect = new HashSet<string>(updatePackageOptions.PackagesToUpdate);
                PackageItemViewModel firstSelectedItem = null;
                foreach (var packageItem in _packageList.PackageItems)
                {
                    packageItem.IsSelected = packagesToSelect.Contains(packageItem.Id, StringComparer.OrdinalIgnoreCase);

                    if (packageItem.IsSelected && firstSelectedItem is null)
                    {
                        firstSelectedItem = packageItem;
                        _packageList._list.ScrollIntoView(firstSelectedItem);
                    }
                }
            }
        }

        private void CleanUp()
        {
            NuGetUIThreadHelper.JoinableTaskFactory.Run(async () =>
            {
                await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                _windowSearchHost.TerminateSearch();
            });

            RemoveRestoreBar();
            RemoveRestartBar();

            INuGetSolutionManagerService solutionManager = Model.Context.SolutionManagerService;
            solutionManager.ProjectAdded -= OnProjectChanged;
            solutionManager.ProjectRemoved -= OnProjectChanged;
            solutionManager.ProjectUpdated -= OnProjectUpdated;
            solutionManager.ProjectRenamed -= OnProjectRenamed;
            solutionManager.AfterNuGetCacheUpdated -= OnNuGetCacheUpdated;

            Model.Context.ProjectActionsExecuted -= OnProjectActionsExecuted;

            Model.Context.SourceService.PackageSourcesChanged -= PackageSourcesChanged;

            Settings.SettingsChanged -= Settings_SettingsChanged;

            Model.Dispose();

            // make sure to cancel currently running load or refresh tasks
            _loadCts?.Cancel();
            _refreshCts?.Cancel();
            _cancelSelectionChangedSource?.Cancel();

            // make sure to dispose cancellation token source
            _loadCts?.Dispose();
            _refreshCts?.Dispose();
            _cancelSelectionChangedSource?.Dispose();

            _detailModel.Dispose();
            _packageList.SelectionChanged -= PackageList_SelectionChanged;

            EmitPMUIClosingTelemetry();
        }

        private void SuppressDisclaimerChecked(object sender, RoutedEventArgs e)
        {
            _legalDisclaimer.Visibility = Visibility.Collapsed;
            RegistrySettingUtility.SetBooleanSetting(Constants.SuppressUIDisclaimerRegistryName, true);
        }

        /// <summary>
        /// This method is called after user clicks a button to execute an action, e.g. install a package.
        /// </summary>
        /// <param name="performAction">A function that returns the task that is performing the action
        /// thru UIActionEngine.</param>
        /// <param name="setOptions">A method that is called to set the action options,
        /// such as dependency behavior.</param>
        public void ExecuteAction(Func<Task> performAction, Action<NuGetUI> setOptions)
        {
            NuGetUIThreadHelper.JoinableTaskFactory.RunAsync(async delegate
            {
                var sw = Stopwatch.StartNew();
                IsEnabled = false;
                _isExecutingAction = true;

                try
                {
                    var nugetUi = Model.UIController as NuGetUI;
                    if (nugetUi != null)
                    {
                        setOptions(nugetUi);
                    }

                    var restoreSucceded = await RestoreBar.UIRestorePackagesAsync(CancellationToken.None);
                    if (restoreSucceded)
                    {
                        // Note that the task returned by performAction() will call something like
                        // UIActionEngine.PerformActionAsync(), which has to be called from a background thread.
                        // Thus, we need to use Task.Run() here.
                        await Task.Run(() => performAction());
                    }
                }
                finally
                {
                    //Invalidate cache.
                    Model.CachedUpdates = null;

                    IsEnabled = true;
                    _isExecutingAction = false;
                    if (_isRefreshRequired)
                    {
                        await RunAndEmitRefreshAsync(async () => await RefreshAsync(), RefreshOperationSource.ExecuteAction, GetTimeSinceLastRefreshAndRestart(), sw);
                        _isRefreshRequired = false;
                    }

                    _actionCompleted?.Invoke(this, EventArgs.Empty);
                }
            })
            .PostOnFailure(nameof(PackageManagerControl), nameof(ExecuteAction));
        }

        private void ExecuteUninstallPackageCommand(object sender, ExecutedRoutedEventArgs e)
        {
            var package = e.Parameter as PackageItemViewModel;
            if (Model.IsSolution
                || package == null
                || package.Status == PackageStatus.NotInstalled)
            {
                return;
            }

            UninstallPackage(package.Id, new[] { package });
        }

        private void SetOptions(NuGetUI nugetUi, NuGetActionType actionType, IEnumerable<PackageItemViewModel> packages)
        {
            var options = _detailModel.Options;

            int topLevelVulnerablePackagesCount = 0;
            List<int> topLevelVulnerablePackagesMaxSeverities = new();
            int transitiveVulnerablePackagesCount = 0;
            List<int> transitiveVulnerablePackagesMaxSeverities = new();
            if (packages != null)
            {
                foreach (PackageItemViewModel package in packages)
                {
                    if (package.PackageLevel == PackageLevel.TopLevel && package.IsPackageVulnerable)
                    {
                        topLevelVulnerablePackagesCount++;
                        topLevelVulnerablePackagesMaxSeverities.Add(package.VulnerabilityMaxSeverity);
                    }
                    else if (package.PackageLevel == PackageLevel.Transitive && package.IsPackageVulnerable)
                    {
                        transitiveVulnerablePackagesCount++;
                        transitiveVulnerablePackagesMaxSeverities.Add(package.VulnerabilityMaxSeverity);
                    }
                }
            }

            nugetUi.FileConflictAction = options.SelectedFileConflictAction.Action;
            nugetUi.DependencyBehavior = options.SelectedDependencyBehavior.Behavior;
            nugetUi.RemoveDependencies = options.RemoveDependencies;
            nugetUi.ForceRemove = options.ForceRemove;
            nugetUi.DisplayPreviewWindow = options.ShowPreviewWindow;
            nugetUi.DisplayDeprecatedFrameworkWindow = options.ShowDeprecatedFrameworkWindow;
            nugetUi.Projects = Model.Context.Projects;
            nugetUi.ProjectContext.ActionType = actionType;
            nugetUi.TopLevelVulnerablePackagesCount = topLevelVulnerablePackagesCount;
            nugetUi.TopLevelVulnerablePackagesMaxSeverities = topLevelVulnerablePackagesMaxSeverities;
            nugetUi.TransitiveVulnerablePackagesCount = transitiveVulnerablePackagesCount;
            nugetUi.TransitiveVulnerablePackagesMaxSeverities = transitiveVulnerablePackagesMaxSeverities;
        }

        private void ExecuteInstallPackageCommand(object sender, ExecutedRoutedEventArgs e)
        {
            var package = e.Parameter as PackageItemViewModel;
            if (package == null || Model.IsSolution)
            {
                return;
            }
            var versionToInstall = package.LatestVersion ?? package.Version;
            InstallPackage(package.Id, versionToInstall, new[] { package });
        }

        private void PackageList_UpdateButtonClicked(PackageItemViewModel[] selectedPackages)
        {
            var packagesToUpdate = selectedPackages
                .Select(package => new PackageIdentity(package.Id, package.Version))
                .ToList();

            UpdatePackage(packagesToUpdate, selectedPackages);
        }

        private void ExecuteRestartSearchCommand(object sender, ExecutedRoutedEventArgs e)
        {
            NuGetUIThreadHelper.JoinableTaskFactory.RunAsync(async () =>
            {
                await RunAndEmitRefreshAsync(async () => await ExecuteRestartSearchCommandAsync(), RefreshOperationSource.RestartSearchCommand, GetTimeSinceLastRefreshAndRestart(), Stopwatch.StartNew());
            }).PostOnFailure(nameof(PackageManagerControl), nameof(ExecuteRestartSearchCommand));
        }

        private void ExecuteSearchPackageCommand(object sender, ExecutedRoutedEventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            var tupleParam = e.Parameter as Tuple<string, HyperlinkType>;
            var alternatePackageId = tupleParam?.Item1;
            if (tupleParam != null && !string.IsNullOrWhiteSpace(alternatePackageId))
            {
                if (_windowSearchHost?.IsEnabled == true)
                {
                    if (_windowSearchHost.SearchTask != null)
                    {
                        // Terminate current PM UI search task
                        _windowSearchHost.SearchAsync(pSearchQuery: null);
                    }
                    if (_windowSearchHost.SearchTask == null)
                    {
                        NuGet.VisualStudio.Internal.Contracts.ItemFilter currentTab = UIUtility.ToContractsItemFilter(ActiveFilter);

                        _topPanel.SelectFilter(ItemFilter.All);
                        var searchQuery = UIUtility.CreateSearchQuery(alternatePackageId);
                        Search(searchQuery);

                        var hyperlinkType = tupleParam.Item2;
                        var evt = NavigatedTelemetryEvent.CreateWithAlternatePackageNavigation(hyperlinkType, currentTab, Model.IsSolution, alternatePackageId);
                        TelemetryActivity.EmitTelemetryEvent(evt);
                    }
                }
            }

            e.Handled = true;
        }

        private async Task ExecuteRestartSearchCommandAsync()
        {
            await SearchPackagesAndRefreshUpdateCountAsync(useCacheForUpdates: false);
            await RefreshConsolidatablePackagesCountAsync();
        }

        /// <summary>
        /// Install a package in open project(s)
        /// </summary>
        /// <param name="packageId">Package ID to install</param>
        /// <param name="version">Package Version to install</param>
        /// <param name="packagesInfo">Corresponding Package ViewModels from PM UI. Only needed for vulnerability telemetry counts. Can be <see langword="null" /></param>
        internal void InstallPackage(string packageId, NuGetVersion version, IEnumerable<PackageItemViewModel> packagesInfo)
        {
            var sourceMappingSourceName = PackageSourceMappingUtility.GetNewSourceMappingSourceName(Model.UIController.UIContext.PackageSourceMapping, Model.UIController.ActivePackageSourceMoniker);
            UserAction action = UserAction.CreateInstallAction(packageId, version, Model.IsSolution, UIUtility.ToContractsItemFilter(_topPanel.Filter), sourceMappingSourceName);

            ExecuteAction(
                () =>
                {
                    return Model.Context.UIActionEngine.PerformInstallOrUninstallAsync(
                        Model.UIController,
                        action,
                        CancellationToken.None);
                },
                nugetUi => SetOptions(nugetUi, NuGetActionType.Install, packagesInfo));
        }

        /// <summary>
        /// Uninstall a package in open project(s)
        /// </summary>
        /// <param name="packageId">Package ID to uninstall</param>
        /// <param name="packagesInfo">Corresponding Package ViewModels from PM UI. Only needed for vulnerability telemetry counts. Can be <see langword="null" /></param>
        internal void UninstallPackage(string packageId, IEnumerable<PackageItemViewModel> packagesInfo)
        {
            var action = UserAction.CreateUnInstallAction(packageId, Model.IsSolution, UIUtility.ToContractsItemFilter(_topPanel.Filter));

            ExecuteAction(
                () =>
                {
                    return Model.Context.UIActionEngine.PerformInstallOrUninstallAsync(
                        Model.UIController,
                        action,
                        CancellationToken.None);
                },
                nugetUi => SetOptions(nugetUi, NuGetActionType.Uninstall, packagesInfo));
        }

        /// <summary>
        /// Updates packages in open project(s)
        /// </summary>
        /// <param name="packages">Packages identities to update</param>
        /// <param name="packagesInfo">Corresponding Package ViewModels from PM UI. Only needed for vulnerability telemetry counts. Can be <see langword="null" /></param>
        internal void UpdatePackage(List<PackageIdentity> packages, IEnumerable<PackageItemViewModel> packagesInfo)
        {
            if (packages.Count == 0)
            {
                return;
            }

            ExecuteAction(
                () =>
                {
                    return Model.Context.UIActionEngine.PerformUpdateAsync(
                        Model.UIController,
                        packages,
                        CancellationToken.None);
                },
                nugetUi => SetOptions(nugetUi, NuGetActionType.Update, packagesInfo));
        }

        private void UpgradeButton_Click(object sender, RoutedEventArgs e)
        {
            NuGetUIThreadHelper.JoinableTaskFactory.RunAsync(async () =>
            {
                IProjectContextInfo project = Model.Context.Projects.FirstOrDefault();
                Debug.Assert(project != null);
                await Model.Context.UIActionEngine.UpgradeNuGetProjectAsync(Model.UIController, project: null);
            })
            .PostOnFailure(nameof(PackageManagerControl), nameof(UpgradeButton_Click));
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                CleanUp();
            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
