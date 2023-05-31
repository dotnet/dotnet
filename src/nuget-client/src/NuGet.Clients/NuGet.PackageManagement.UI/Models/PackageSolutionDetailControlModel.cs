// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft;
using Microsoft.ServiceHub.Framework;
using Microsoft.VisualStudio.Shell;
using NuGet.PackageManagement.UI.ViewModels;
using NuGet.PackageManagement.VisualStudio;
using NuGet.ProjectModel;
using NuGet.Versioning;
using NuGet.VisualStudio;
using NuGet.VisualStudio.Internal.Contracts;
using NuGet.VisualStudio.Telemetry;
using Task = System.Threading.Tasks.Task;

namespace NuGet.PackageManagement.UI
{
    internal class PackageSolutionDetailControlModel : DetailControlModel
    {
        // This class does not own this instance, so do not dispose of it in this class.
        private INuGetSolutionManagerService _solutionManager;
        private string _installedVersions; // The text describing the installed versions information, such as "not installed", "multiple versions installed" etc.
        private int _installedVersionsCount; // the count of different installed versions
        private bool _canUninstall;
        private bool _canInstall;
        // Indicates whether the SelectCheckBoxState is being updated in code. True means the state is being updated by code, while false means the state is changed by user clicking the checkbox.
        private bool _updatingSelectCheckBoxState;
        private bool? _selectCheckBoxState;
        private bool _isInBatchUpdate;
        private List<PackageInstallationInfo> _projects; // List of projects in the solution

        private PackageSolutionDetailControlModel(
            IServiceBroker serviceBroker,
            IEnumerable<IProjectContextInfo> projects,
            INuGetUI uiController)
            : base(serviceBroker, projects, uiController)
        {
            IsRequestedVisible = projects.Any(p => p.ProjectStyle == ProjectStyle.PackageReference);
        }

        private async ValueTask InitializeAsync(
            INuGetSolutionManagerService solutionManager,
            CancellationToken cancellationToken)
        {
            _solutionManager = solutionManager;
            _solutionManager.ProjectAdded += SolutionProjectChanged;
            _solutionManager.ProjectRemoved += SolutionProjectChanged;
            _solutionManager.ProjectUpdated += SolutionProjectChanged;
            _solutionManager.ProjectRenamed += SolutionProjectChanged;

            // when the SelectedVersion is changed, we need to update CanInstall and CanUninstall.
            PropertyChanged += (_, e) =>
            {
                if (e.PropertyName == nameof(SelectedVersion))
                {
                    UpdateCanInstallAndCanUninstall();
                }
            };

            await CreateProjectListsAsync(cancellationToken);
        }

        public static async ValueTask<PackageSolutionDetailControlModel> CreateAsync(
            IServiceBroker serviceBroker,
            INuGetSolutionManagerService solutionManager,
            IEnumerable<IProjectContextInfo> projects,
            INuGetUI uiController,
            CancellationToken cancellationToken)
        {
            var packageSolutionDetailControlModel = new PackageSolutionDetailControlModel(serviceBroker, projects, uiController);
            await packageSolutionDetailControlModel.InitializeAsync(solutionManager, cancellationToken);
            return packageSolutionDetailControlModel;
        }

        internal static async ValueTask<PackageSolutionDetailControlModel> CreateAsync(
            INuGetSolutionManagerService solutionManager,
            IEnumerable<IProjectContextInfo> projects,
            IServiceBroker serviceBroker,
            INuGetUI uiController,
            CancellationToken cancellationToken)
        {
            var packageSolutionDetailControlModel = new PackageSolutionDetailControlModel(serviceBroker, projects, uiController);
            await packageSolutionDetailControlModel.InitializeAsync(solutionManager, cancellationToken);
            return packageSolutionDetailControlModel;
        }

        public List<PackageInstallationInfo> Projects
        {
            get => _projects;
            set
            {
                _projects = value;
                OnPropertyChanged(nameof(Projects));
            }
        }

        public override bool IsSolution => true;

        public override async Task RefreshAsync(CancellationToken cancellationToken)
        {
            await UpdateInstalledVersionsAsync(cancellationToken);
        }

        public string InstalledVersions
        {
            get => _installedVersions;
            set
            {
                _installedVersions = value;
                OnPropertyChanged(nameof(InstalledVersions));
            }
        }

        public bool IsRequestedVisible { get; private set; }

        private async Task UpdateInstalledVersionsAsync(CancellationToken cancellationToken)
        {
            var hash = new HashSet<NuGetVersion>();

            foreach (var project in _projects)
            {
                try
                {
                    IPackageReferenceContextInfo installedVersion = await GetInstalledPackageAsync(project.NuGetProject, Id, cancellationToken);
                    if (installedVersion != null)
                    {
                        project.InstalledVersion = installedVersion.Identity.Version;
                        hash.Add(installedVersion.Identity.Version);
                        project.AutoReferenced = installedVersion.IsAutoReferenced;

                        if (project.NuGetProject.ProjectStyle.Equals(ProjectStyle.PackageReference))
                        {
                            project.RequestedVersion = installedVersion?.AllowedVersions?.OriginalString;
                        }
                    }
                    else
                    {
                        project.RequestedVersion = null;
                        project.InstalledVersion = null;
                        project.AutoReferenced = false;
                    }
                }
                catch (Exception ex)
                {
                    project.InstalledVersion = null;

                    // we don't expect it to throw any exception here. But in some edge case when opening manager ui at solution is the
                    // first NuGet operation, and packages.config file is not valid for any of the project, then it will throw here which
                    // should be ignored since we already show a error bar on manager ui to show this exact error.
                    ActivityLog.LogError(NuGetUI.LogEntrySource, ex.ToString());
                }
            }

            InstalledVersionsCount = hash.Count;

            if (hash.Count == 0)
            {
                InstalledVersions = Resources.Text_NotInstalled;
            }
            else if (hash.Count == 1)
            {
                var displayVersion = new DisplayVersion(
                    hash.First(),
                    string.Empty);
                InstalledVersions = displayVersion.ToString();
            }
            else
            {
                InstalledVersions = Resources.Text_MultipleVersionsInstalled;
            }

            UpdateCanInstallAndCanUninstall();
            AutoSelectProjects();
        }

        /// <summary>
        /// This method is called from several methods that are called from properties and LINQ queries
        /// It is likely not called more than once in an action.
        /// </summary>
        private async Task<IPackageReferenceContextInfo> GetInstalledPackageAsync(
            IProjectContextInfo project,
            string packageId,
            CancellationToken cancellationToken)
        {
            IEnumerable<IPackageReferenceContextInfo> installedPackages = await project.GetInstalledPackagesAsync(
                ServiceBroker,
                cancellationToken);
            IPackageReferenceContextInfo installedPackage = installedPackages
                .Where(p => StringComparer.OrdinalIgnoreCase.Equals(p.Identity.Id, packageId))
                .FirstOrDefault();
            return installedPackage;
        }

        protected override async Task CreateVersionsAsync(CancellationToken cancellationToken)
        {
            // The value will be null if the server does not return any versions.
            if (_allPackageVersions == null || _allPackageVersions.Count == 0)
            {
                return;
            }

            _versions.Clear();
            List<(NuGetVersion version, bool isDeprecated, bool isVulnerable)> allVersions = _allPackageVersions?.Where(v => v.version != null).OrderByDescending(v => v.version).ToList();

            // null, if no version constraint defined in package.config
            VersionRange allowedVersions = await GetAllowedVersionsAsync(cancellationToken);
            var allVersionsAllowed = allVersions.Where(v => allowedVersions.Satisfies(v.version)).ToArray();

            var blockedVersions = new List<NuGetVersion>(allVersions.Count);
            foreach ((NuGetVersion version, bool isDeprecated, bool isVulnerable) in allVersions)
            {
                if (!allVersionsAllowed.Any(a => a.version.Version.Equals(version.Version)))
                {
                    blockedVersions.Add(version);
                }
            }

            // get latest prerelease or stable based on allowed versions
            var latestPrerelease = allVersionsAllowed.FirstOrDefault(v => v.version.IsPrerelease);
            var latestStableVersion = allVersionsAllowed.FirstOrDefault(v => !v.version.IsPrerelease);

            if (latestPrerelease.version != null
                && (latestStableVersion.version == null || latestPrerelease.version > latestStableVersion.version))
            {
                _versions.Add(new DisplayVersion(latestPrerelease.version, Resources.Version_LatestPrerelease, isDeprecated: latestPrerelease.isDeprecated));
            }

            if (latestStableVersion.version != null)
            {
                _versions.Add(new DisplayVersion(latestStableVersion.version, Resources.Version_LatestStable, isDeprecated: latestStableVersion.isDeprecated));
            }

            // add a separator
            if (_versions.Count > 0)
            {
                _versions.Add(null);
            }

            // first add all the available versions to be updated
            foreach (var version in allVersionsAllowed)
            {
                _versions.Add(new DisplayVersion(version.version, null, isDeprecated: version.isDeprecated));
            }

            ProjectVersionConstraint[] selectedProjects = (await GetConstraintsForSelectedProjectsAsync(cancellationToken)).ToArray();

            bool autoReferenced = selectedProjects.Length > 0 && selectedProjects.All(e => e.IsAutoReferenced);

            // Disable controls if this is an auto referenced package.
            SetAutoReferencedCheck(autoReferenced);

            // Add disabled versions
            AddBlockedVersions(blockedVersions);

            var latestVersion = latestPrerelease.version > latestStableVersion.version ? latestPrerelease.version : latestStableVersion.version;

            SelectVersion(latestVersion);

            OnPropertyChanged(nameof(Versions));
        }

        public int InstalledVersionsCount
        {
            get => _installedVersionsCount;
            set
            {
                _installedVersionsCount = value;
                OnPropertyChanged(nameof(InstalledVersionsCount));
            }
        }

        // The event handler that is called when a project is added, removed or renamed.
        private void SolutionProjectChanged(object sender, IProjectContextInfo project)
        {
            NuGetUIThreadHelper.JoinableTaskFactory.RunAsync(() => CreateProjectListsAsync(CancellationToken.None))
                .PostOnFailure(nameof(PackageSolutionDetailControlModel), nameof(SolutionProjectChanged));
        }

        protected override void DependencyBehavior_SelectedChanged(object sender, EventArgs e)
        {
            NuGetUIThreadHelper.JoinableTaskFactory.RunAsync(() => CreateVersionsAndUpdateInstallUninstallAsync())
                .PostOnFailure(nameof(PackageSolutionDetailControlModel), nameof(DependencyBehavior_SelectedChanged));
        }

        public override void CleanUp()
        {
            _solutionManager.ProjectAdded -= SolutionProjectChanged;
            _solutionManager.ProjectRemoved -= SolutionProjectChanged;
            _solutionManager.ProjectRenamed -= SolutionProjectChanged;
            _solutionManager.ProjectUpdated -= SolutionProjectChanged;

            if (Projects != null)
            {
                foreach (var project in Projects)
                {
                    project.SelectedChanged -= Project_SelectedChanged;
                }
            }
        }

        // Creates the project lists. Also called after a project is added/removed/renamed.
        private async Task CreateProjectListsAsync(CancellationToken cancellationToken)
        {
            // unhook event handler
            if (Projects != null)
            {
                foreach (PackageInstallationInfo project in Projects)
                {
                    project.SelectedChanged -= Project_SelectedChanged;
                }
            }

            IReadOnlyCollection<IProjectContextInfo> projectContexts;

            using (INuGetProjectManagerService projectManagerService = await ServiceBroker.GetProxyAsync<INuGetProjectManagerService>(
                NuGetServices.ProjectManagerService))
            {
                Assumes.NotNull(projectManagerService);
                projectContexts = await projectManagerService.GetProjectsAsync(cancellationToken);
            }

            var packageInstallationInfos = new List<PackageInstallationInfo>();
            foreach (IProjectContextInfo project in projectContexts)
            {
                PackageInstallationInfo packageInstallationInfo = await PackageInstallationInfo.CreateAsync(
                    ServiceBroker,
                    project,
                    cancellationToken);
                packageInstallationInfos.Add(packageInstallationInfo);
            }

            Projects = packageInstallationInfos;

            // hook up event handler
            foreach (PackageInstallationInfo project in Projects)
            {
                project.SelectedChanged += Project_SelectedChanged;
            }

            await UpdateInstalledVersionsAsync(cancellationToken);
            UpdateSelectCheckBoxState();
            CanUninstall = false;
            CanInstall = false;
        }

        public bool? SelectCheckBoxState
        {
            get
            {
                return _selectCheckBoxState;
            }
            set
            {
                _selectCheckBoxState = value;
                OnPropertyChanged(nameof(SelectCheckBoxState));
            }
        }

        public bool CanInstall
        {
            get
            {
                return _canInstall;
            }
            set
            {
                _canInstall = value;
                OnPropertyChanged(nameof(CanInstall));
            }
        }

        public bool CanUninstall
        {
            get
            {
                return _canUninstall;
            }
            set
            {
                _canUninstall = value;
                OnPropertyChanged(nameof(CanUninstall));
            }
        }

        private void Project_SelectedChanged(object sender, EventArgs e)
        {
            if (_isInBatchUpdate)
            {
                return;
            }

            UpdateSelectAllAfterProjectSelectionChanged();
        }

        private void UpdateSelectAllAfterProjectSelectionChanged()
        {
            UpdateSelectCheckBoxState();

            NuGetUIThreadHelper.JoinableTaskFactory.RunAsync(() => CreateVersionsAndUpdateInstallUninstallAsync())
                .PostOnFailure(nameof(PackageSolutionDetailControlModel), nameof(UpdateSelectAllAfterProjectSelectionChanged));
        }

        private async Task CreateVersionsAndUpdateInstallUninstallAsync()
        {
            // update versions list everytime selected projects change
            await CreateVersionsAsync(CancellationToken.None);

            UpdateCanInstallAndCanUninstall();
        }

        private void UpdateSelectCheckBoxState()
        {
            _updatingSelectCheckBoxState = true;
            int countSelected = Projects.Count(project => project.IsSelected);
            if (countSelected == 0)
            {
                SelectCheckBoxState = false;
            }
            else if (countSelected == Projects.Count)
            {
                SelectCheckBoxState = true;
            }
            else
            {
                SelectCheckBoxState = null;
            }

            _updatingSelectCheckBoxState = false;
        }

        private void UpdateCanInstallAndCanUninstall()
        {
            CanUninstall = Projects.Any(project => project.IsSelected && project.InstalledVersion != null && !project.AutoReferenced);

            CanInstall = SelectedVersion != null && Projects.Any(
                project => project.IsSelected &&
                    VersionComparer.Default.Compare(SelectedVersion.Version, project.InstalledVersion) != 0);
        }

        private async ValueTask<IEnumerable<ProjectVersionConstraint>> GetConstraintsForSelectedProjectsAsync(
            CancellationToken cancellationToken)
        {
            var selectedProjectsNames = new List<string>();
            foreach (PackageInstallationInfo project in Projects.Where(p => p.IsSelected).ToList())
            {
                IProjectMetadataContextInfo projectMetadata = await project.NuGetProject.GetMetadataAsync(
                    ServiceBroker,
                    cancellationToken);

                selectedProjectsNames.Add(projectMetadata.Name);
            }

            return _projectVersionConstraints.Where(e => selectedProjectsNames.Contains(e.ProjectName, StringComparer.OrdinalIgnoreCase));
        }

        private async ValueTask<VersionRange> GetAllowedVersionsAsync(CancellationToken cancellationToken)
        {
            // allowed version ranges for selected list of projects
            IEnumerable<VersionRange> allowedVersionsRange = (await GetConstraintsForSelectedProjectsAsync(cancellationToken))
                .Select(e => e.VersionRange)
                .Where(v => v != null);

            // if version constraints exist then merge all the ranges and return common range which satisfies all
            return allowedVersionsRange.Any() ? VersionRange.CommonSubSet(allowedVersionsRange) : VersionRange.All;
        }

        internal async Task SelectAllProjectsAsync(bool select)
        {
            if (_updatingSelectCheckBoxState)
            {
                return;
            }

            await BatchUpdateIsSelectedAsync(select);
        }

        protected override async Task OnCurrentPackageChanged()
        {
            if (_searchResultPackage == null)
            {
                return;
            }

            await UpdateInstalledVersionsAsync(CancellationToken.None);
        }

        // auto select projects based on the current tab and currently selected package
        private void AutoSelectProjects()
        {
            if (_filter == ItemFilter.Consolidate ||
                _filter == ItemFilter.UpdatesAvailable)
            {
                BatchUpdateIsSelectedBasedOnInstalledVersion();
            }
        }

        public override void OnFilterChanged(ItemFilter? previousFilter, ItemFilter currentFilter)
        {
            base.OnFilterChanged(previousFilter, currentFilter);

            // clear selection if filter is changed from Consolidate/UpdateAvailable
            // to Browse/Install.
            if ((previousFilter == ItemFilter.Consolidate || previousFilter == ItemFilter.UpdatesAvailable) &&
                (_filter == ItemFilter.All || _filter == ItemFilter.Installed))
            {
                BatchUnselectAllProjects();
            }
        }

        public override IEnumerable<IProjectContextInfo> GetSelectedProjects(UserAction action)
        {
            var selectedProjects = new List<IProjectContextInfo>();

            foreach (var project in _projects)
            {
                if (project.IsSelected == false)
                {
                    continue;
                }

                if (action.Action == NuGetProjectActionType.Install)
                {
                    // for install, the installed version can't be the same as the version to be installed.
                    // AutoReferenced packages should be ignored
                    if (!project.AutoReferenced &&
                        VersionComparer.Default.Compare(
                        project.InstalledVersion,
                        action.Version) != 0)
                    {
                        selectedProjects.Add(project.NuGetProject);
                    }
                }
                else
                {
                    // for uninstall, the package must be already installed
                    if (project.InstalledVersion != null && !project.AutoReferenced)
                    {
                        selectedProjects.Add(project.NuGetProject);
                    }
                }
            }

            return selectedProjects;
        }

        private void BatchUnselectAllProjects()
        {
            _isInBatchUpdate = true;

            try
            {
                foreach (PackageInstallationInfo project in Projects)
                {
                    project.IsSelected = false;
                }

                UpdateSelectCheckBoxState();
                UpdateSelectAllAfterProjectSelectionChanged();
            }
            finally
            {
                _isInBatchUpdate = false;
            }
        }

        private void BatchUpdateIsSelectedBasedOnInstalledVersion()
        {
            _isInBatchUpdate = true;

            try
            {
                foreach (PackageInstallationInfo project in Projects)
                {
                    project.IsSelected = project.InstalledVersion != null;
                }
            }
            finally
            {
                _isInBatchUpdate = false;
            }
        }

        private async ValueTask BatchUpdateIsSelectedAsync(bool isSelected)
        {
            _isInBatchUpdate = true;

            try
            {
                foreach (PackageInstallationInfo project in Projects)
                {
                    project.IsSelected = isSelected;
                }

                UpdateSelectCheckBoxState();
                await CreateVersionsAndUpdateInstallUninstallAsync();
            }
            finally
            {
                _isInBatchUpdate = false;
            }
        }
    }
}
