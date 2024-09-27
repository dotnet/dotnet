// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.ComponentModel;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceHub.Framework;
using NuGet.PackageManagement.VisualStudio;
using NuGet.Protocol;
using NuGet.Versioning;
using NuGet.VisualStudio;
using NuGet.VisualStudio.Internal.Contracts;

namespace NuGet.PackageManagement.UI
{
    // This class is used to represent one of the following facts about a package:
    // - A version of the package is installed. In this case, property Version is not null.
    //   Property IsSolution indicates if the package is installed in the solution or in a project.
    // - The package is not installed in a project/solution. In this case, property Version is null.
    public class PackageInstallationInfo : IComparable<PackageInstallationInfo>, INotifyPropertyChanged, ISelectableItem
    {
        public event EventHandler SelectedChanged;

        private bool _autoReferenced;
        private bool _isSelected;
        private string _projectName;
        private readonly IServiceBroker _serviceBroker;

        private PackageInstallationInfo(IServiceBroker serviceBroker, IProjectContextInfo project)
        {
            _serviceBroker = serviceBroker;
            NuGetProject = project;
        }

        public static async ValueTask<PackageInstallationInfo> CreateAsync(
            IServiceBroker serviceBroker,
            IProjectContextInfo project,
            CancellationToken cancellationToken)
        {
            var packageInstallationInfo = new PackageInstallationInfo(serviceBroker, project);
            await packageInstallationInfo.InitializeAsync(cancellationToken);
            return packageInstallationInfo;
        }

        private async ValueTask InitializeAsync(CancellationToken cancellationToken)
        {
            _projectName = await NuGetProject.GetUniqueNameOrNameAsync(_serviceBroker, cancellationToken);
        }

        private NuGetVersion _versionInstalled;
        private string _versionRequested;

        private int _installedVersionMaxVulnerability = -1;
        public int InstalledVersionMaxVulnerability
        {
            get => _installedVersionMaxVulnerability;
            set
            {
                if (_installedVersionMaxVulnerability != value)
                {
                    _installedVersionMaxVulnerability = value;
                    OnPropertyChanged(nameof(InstalledVersionMaxVulnerability));
                    OnPropertyChanged(nameof(IsInstalledVersionVulnerable));
                }
            }
        }

        public bool IsInstalledVersionVulnerable
        {
            get => InstalledVersionMaxVulnerability > -1;
        }

        public NuGetVersion InstalledVersion
        {
            get { return _versionInstalled; }
            set
            {
                _versionInstalled = value;
                OnPropertyChanged(nameof(InstalledVersion));
            }
        }

        public PackageLevel? _packageLevel;
        public PackageLevel? PackageLevel
        {
            get => _packageLevel;
            set
            {
                _packageLevel = value;
                OnPropertyChanged(nameof(PackageLevel));
            }
        }

        public string RequestedVersion
        {
            get { return _versionRequested; }
            set
            {
                _versionRequested = value;
                OnPropertyChanged(nameof(RequestedVersion));
            }
        }

        public bool AutoReferenced
        {
            get => _autoReferenced;
            set
            {
                _autoReferenced = value;
                OnPropertyChanged(nameof(AutoReferenced));
            }
        }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    SelectedChanged?.Invoke(this, EventArgs.Empty);
                    OnPropertyChanged(nameof(IsSelected));
                }
            }
        }

        public IProjectContextInfo NuGetProject { get; }

        public string ProjectName
        {
            get => _projectName;
            set
            {
                _projectName = value;
                OnPropertyChanged(nameof(ProjectName));
            }
        }

        public int CompareTo(PackageInstallationInfo other)
        {
            return string.Compare(_projectName, other._projectName, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            var other = obj as PackageInstallationInfo;

            return other != null && string.Equals(_projectName, other._projectName, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode()
        {
            return StringComparer.OrdinalIgnoreCase.GetHashCode(_projectName);
        }

        public override string ToString()
        {
            var vulnerabilityString = string.Empty;
            if (InstalledVersionMaxVulnerability != -1)
            {
                var converter = new IntToVulnerabilitySeverityConverter();
                var severityString = converter.Convert(InstalledVersionMaxVulnerability, typeof(PackageVulnerabilitySeverity), null, CultureInfo.CurrentCulture) as string;
                vulnerabilityString = string.Format(CultureInfo.CurrentCulture, Resources.Label_PackageVulnerableToolTip, severityString);
            }

            return $"{ProjectName} {InstalledVersion?.Version.ToString() ?? Resources.Text_NotInstalled} {vulnerabilityString} {PackageLevel}";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
