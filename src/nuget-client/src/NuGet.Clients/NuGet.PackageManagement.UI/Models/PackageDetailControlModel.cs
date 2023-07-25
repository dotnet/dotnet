// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Data;
using Microsoft.ServiceHub.Framework;
using NuGet.Packaging.Core;
using NuGet.Versioning;
using NuGet.VisualStudio;
using NuGet.VisualStudio.Internal.Contracts;
using Task = System.Threading.Tasks.Task;

namespace NuGet.PackageManagement.UI
{
    // used to manage packages in one project.
    public class PackageDetailControlModel : DetailControlModel
    {
        // This class does not own this instance, so do not dispose of it in this class.
        private readonly INuGetSolutionManagerService _solutionManager;

        public PackageDetailControlModel(
            IServiceBroker serviceBroker,
            INuGetSolutionManagerService solutionManager,
            IEnumerable<IProjectContextInfo> projects,
            INuGetUI uiController)
            : base(serviceBroker, projects, uiController)
        {
            _solutionManager = solutionManager;
            _solutionManager.ProjectUpdated += ProjectChanged;
        }

        public async override Task SetCurrentPackageAsync(
            PackageItemViewModel searchResultPackage,
            ItemFilter filter,
            Func<PackageItemViewModel> getPackageItemViewModel)
        {
            // Set InstalledVersion before fetching versions list.
            PackageLevel = searchResultPackage.PackageLevel;
            VersionOverride = searchResultPackage.VersionOverride;
            InstalledVersion = searchResultPackage.InstalledVersion;
            InstalledVersionRange = searchResultPackage.AllowedVersions;

            VersionsView = new CollectionViewSource() { Source = Versions }.View;

            if (IsProjectPackageReference)
            {
                VersionsView.Filter += VersionsFilter;
            }

            OnPropertyChanged(nameof(VersionsView));

            await base.SetCurrentPackageAsync(searchResultPackage, filter, getPackageItemViewModel);

            // SetCurrentPackage can take long time to return, user might changed selected package.
            // Check selected package.
            if (getPackageItemViewModel() != searchResultPackage)
            {
                return;
            }
            PreviousSelectedVersion = null;
            PackageLevel = searchResultPackage.PackageLevel;
            InstalledVersion = searchResultPackage.InstalledVersion;
            InstalledVersionRange = searchResultPackage.AllowedVersions;
            SelectedVersion.IsCurrentInstalled = InstalledVersion == SelectedVersion.Version && InstalledVersionRange == SelectedVersion.Range;
        }

        public override bool IsSolution
        {
            get { return false; }
        }

        private void ProjectChanged(object sender, IProjectContextInfo project)
        {
            _nugetProjects = new List<IProjectContextInfo> { project };

            UpdateInstalledVersion();
        }

        private void UpdateInstalledVersion()
        {
            IOrderedEnumerable<PackageDependency> installed = InstalledPackageDependencies.Where(p =>
                StringComparer.OrdinalIgnoreCase.Equals(p.Id, Id)).OrderByDescending(p => p.VersionRange?.MinVersion, VersionComparer.Default);

            PackageDependency dependency = installed.FirstOrDefault(package => package.VersionRange != null && package.VersionRange.HasLowerBound);

            if (dependency != null)
            {
                if (dependency.Id == _searchResultPackage.Id && _searchResultPackage.InstalledVersion != null)
                {
                    InstalledVersion = _searchResultPackage.InstalledVersion;
                }
                else
                {
                    InstalledVersion = dependency.VersionRange.MinVersion;
                }
                InstalledVersionRange = dependency.VersionRange;
            }
            else
            {
                InstalledVersion = null;
            }
        }

        public override async Task RefreshAsync(CancellationToken cancellationToken)
        {
            UpdateInstalledVersion();
            await CreateVersionsAsync(cancellationToken);
        }

        private static bool HasId(string id, IEnumerable<PackageIdentity> packages)
        {
            return packages.Any(p =>
                StringComparer.OrdinalIgnoreCase.Equals(p.Id, id));
        }

        public override void CleanUp()
        {
            _solutionManager.ProjectUpdated -= ProjectChanged;
        }

        protected override Task CreateVersionsAsync(CancellationToken cancellationToken)
        {
            // The value will be null if the server does not return any versions.
            if (_allPackageVersions == null || _allPackageVersions.Count == 0)
            {
                return Task.CompletedTask;
            }

            Versions.Clear();

            var installedDependency = InstalledPackageDependencies.Where(p =>
                StringComparer.OrdinalIgnoreCase.Equals(p.Id, Id) && p.VersionRange != null && p.VersionRange.HasLowerBound)
                .OrderByDescending(p => p.VersionRange.MinVersion)
                .FirstOrDefault();

            // installVersion is null if the package is not installed
            var installedVersion = installedDependency?.VersionRange;

            List<(NuGetVersion version, bool isDeprecated, bool isVulnerable)> allVersions = _allPackageVersions?.OrderByDescending(v => v.version).ToList();

            // null, if no version constraint defined in package.config
            VersionRange allowedVersions = _projectVersionConstraints.Select(e => e.VersionRange).FirstOrDefault();
            // null, if all versions are allowed to be install or update
            var blockedVersions = new List<NuGetVersion>(allVersions.Count);

            List<(NuGetVersion version, bool isDeprecated, bool isVulnerable)> allVersionsAllowed;
            if (allowedVersions == null)
            {
                allowedVersions = VersionRange.All;
                allVersionsAllowed = allVersions;
            }
            else
            {
                allVersionsAllowed = allVersions.Where(v => allowedVersions.Satisfies(v.version)).ToList();
                foreach ((NuGetVersion version, bool isDeprecated, bool isVulnerable) in allVersions)
                {
                    if (!allVersionsAllowed.Any(a => a.version.Version.Equals(version.Version)))
                    {
                        blockedVersions.Add(version);
                    }
                }
            }

            var latestPrerelease = allVersionsAllowed.FirstOrDefault(v => v.version.IsPrerelease);
            var latestStableVersion = allVersionsAllowed.FirstOrDefault(v => !v.version.IsPrerelease);

            // Add installed version if the project is PackageReference
            if (_nugetProjects.Any() && installedDependency != null && installedDependency.VersionRange.OriginalString != null && _nugetProjects.First().ProjectStyle.Equals(ProjectModel.ProjectStyle.PackageReference))
            {
                VersionRange installedVersionRange = VersionRange.Parse(installedDependency.VersionRange.OriginalString, true);
                NuGetVersion bestVersion = installedVersionRange.FindBestMatch(allVersionsAllowed.Select(v => v.version));
                var deprecationInfo = allVersionsAllowed.FirstOrDefault(v => v.version == bestVersion).isDeprecated;
                var vulnerableInfo = allVersionsAllowed.FirstOrDefault(v => v.version == bestVersion).isVulnerable;
                DisplayVersion displayVersion = new DisplayVersion(installedVersionRange, bestVersion, additionalInfo: string.Empty, isDeprecated: deprecationInfo, isVulnerable: vulnerableInfo);

                _versions.Add(displayVersion);
            }

            var isInstalledFloatingOrRange = installedVersion != null && installedVersion.IsFloating || (installedVersion?.OriginalString != null &&
                (installedVersion.OriginalString.StartsWith("(", StringComparison.OrdinalIgnoreCase) || installedVersion.OriginalString.StartsWith("[", StringComparison.OrdinalIgnoreCase)));
            // Add latest prerelease if neeeded
            if (latestPrerelease.version != null
                && (latestStableVersion.version == null || latestPrerelease.version > latestStableVersion.version) &&
                (isInstalledFloatingOrRange || !latestPrerelease.version.Equals(installedVersion?.MinVersion)))
            {
                VersionRange latestPrereleaseVersionRange = VersionRange.Parse(latestPrerelease.version.ToString(), allowFloating: false);
                _versions.Add(new DisplayVersion(latestPrereleaseVersionRange, latestPrerelease.version, Resources.Version_LatestPrerelease, isDeprecated: latestPrerelease.isDeprecated, isVulnerable: latestPrerelease.isVulnerable));
            }

            // Add latest stable if needed
            if (latestStableVersion.version != null &&
                (isInstalledFloatingOrRange || !latestStableVersion.version.Equals(InstalledVersion)))
            {
                VersionRange latestStableVersionRange = VersionRange.Parse(latestStableVersion.version.ToString(), allowFloating: false);
                _versions.Add(new DisplayVersion(latestStableVersionRange, latestStableVersion.version, Resources.Version_LatestStable, isDeprecated: latestStableVersion.isDeprecated, isVulnerable: latestStableVersion.isVulnerable));
            }

            // add a separator
            if (_versions.Count > 0)
            {
                _versions.Add(null);
            }

            // first add all the available versions to be updated
            foreach (var version in allVersionsAllowed)
            {
                var installed = version.version.Equals(InstalledVersion);
                var autoReferenced = false;

                if (installed && _projectVersionConstraints.Any(e => e.IsAutoReferenced && e.VersionRange?.Satisfies(version.version) == true))
                {
                    // do not allow auto referenced packages
                    autoReferenced = true;
                }

                VersionRange versionRange = VersionRange.Parse(version.version.ToString(), allowFloating: false);
                _versions.Add(new DisplayVersion(versionRange, version.version, additionalInfo: null, isCurrentInstalled: installed, autoReferenced: autoReferenced, isDeprecated: version.isDeprecated, isVulnerable: version.isVulnerable));
            }

            // Disable controls if this is an auto referenced package.
            SetAutoReferencedCheck(InstalledVersion);

            // Add disabled versions
            AddBlockedVersions(blockedVersions);

            var latestVersion = latestPrerelease.version > latestStableVersion.version ? latestPrerelease.version : latestStableVersion.version;

            SelectVersion(latestVersion);

            return Task.CompletedTask;
        }

        private bool VersionsFilter(object o)
        {
            var version = o as DisplayVersion;
            // If the text is empty or is the insalled version we should show all the versions like if there where no filtering
            if (string.IsNullOrEmpty(UserInput) ||
                UserInput.Equals(FirstDisplayedVersion?.Range.OriginalString, StringComparison.OrdinalIgnoreCase) ||
                UserInput.Equals(FirstDisplayedVersion?.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            // Filter out the installed version
            if (version != null && version.AdditionalInfo == string.Empty) return false;

            // If the user typed a version range, show only the versions that are in the range
            if ((UserInput.StartsWith("(", StringComparison.OrdinalIgnoreCase) || UserInput.StartsWith("[", StringComparison.OrdinalIgnoreCase)) &&
               VersionRange.TryParse(UserInput, out VersionRange userRange))
            {
                if (o != null && userRange.Satisfies(version.Version))
                {
                    return true;
                }

                return false;
            }

            // Check if the version starts with the user input
            if (o != null && (o.ToString()).StartsWith(Regex.Replace(UserInput, @"[\*]", ""), StringComparison.OrdinalIgnoreCase)) return true;

            return false;
        }

        private NuGetVersion _installedVersion;

        public NuGetVersion InstalledVersion
        {
            get { return _installedVersion; }
            private set
            {
                _installedVersion = value;
                OnPropertyChanged(nameof(InstalledVersion));
            }
        }

        private VersionRange _installedVersionRange;
        public VersionRange InstalledVersionRange
        {
            get { return _installedVersionRange; }
            private set
            {
                _installedVersionRange = value;
                OnPropertyChanged(nameof(InstalledVersionRange));
                OnPropertyChanged(nameof(IsSelectedVersionInstalled));
                OnPropertyChanged(nameof(IsInstalledVersionTopLevel));
            }
        }

        private PackageLevel _packageLevel;

        public PackageLevel PackageLevel
        {
            get => _packageLevel;
            private set
            {
                _packageLevel = value;
                OnPropertyChanged(nameof(PackageLevel));
            }
        }

        private VersionRange _versionOverride;

        public VersionRange VersionOverride
        {
            get => _versionOverride;
            private set
            {
                _versionOverride = value;
                OnPropertyChanged(nameof(VersionOverride));
            }
        }

        public override void OnSelectedVersionChanged()
        {
            base.OnSelectedVersionChanged();
            OnPropertyChanged(nameof(IsInstallorUpdateButtonEnabled));
            OnPropertyChanged(nameof(ShowVersionOverrideTooltip));
            OnPropertyChanged(nameof(IsSelectedVersionInstalled));
            OnPropertyChanged(nameof(IsInstalledVersionTopLevel));
        }


        public bool IsSelectedVersionInstalled
        {
            get
            {
                return SelectedVersion != null
                    && InstalledVersion != null
                    && (IsProjectPackageReference ? SelectedVersion?.Range?.OriginalString == InstalledVersionRange?.OriginalString : true)
                    && SelectedVersion.Version == InstalledVersion;
            }
        }

        public bool IsInstallorUpdateButtonEnabled
        {
            get
            {
                return SelectedVersion != null
                    && !IsSelectedVersionInstalled
                    && !InstalledVersionIsAutoReferenced
                    && VersionOverride == null
                    && CanInstallWithPackageSourceMapping;
            }
        }

        public bool ShowVersionOverrideTooltip
        {
            get
            {
                return !IsInstallorUpdateButtonEnabled && VersionOverride != null;
            }
        }

        public bool IsProjectPackageReference
        {
            get
            {
                return _nugetProjects.Any() && _nugetProjects.FirstOrDefault().ProjectStyle.Equals(ProjectModel.ProjectStyle.PackageReference);
            }
        }

        public bool IsFloatingVersionSupported
        {
            get
            {
                return IsProjectPackageReference && !IsCentralPackageManagementEnabled;
            }
        }

        public bool IsInstalledVersionTopLevel => InstalledVersion != null && PackageLevel == PackageLevel.TopLevel;

        public override IEnumerable<IProjectContextInfo> GetSelectedProjects(UserAction action)
        {
            return _nugetProjects;
        }

        public override void SetInstalledOrUpdateButtonIsEnabled()
        {
            OnPropertyChanged(nameof(IsInstallorUpdateButtonEnabled));
        }
    }
}
