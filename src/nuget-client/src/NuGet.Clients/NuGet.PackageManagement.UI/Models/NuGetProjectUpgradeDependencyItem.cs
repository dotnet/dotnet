// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;

using NuGet.Common;
using NuGet.Packaging.Core;

namespace NuGet.PackageManagement.UI
{
    public class NuGetProjectUpgradeDependencyItem : INotifyPropertyChanged
    {
        private bool _installAsTopLevel;

        private PackageWithDependants _packageWithDependants;

        public PackageIdentity Identity { get; }

        public IReadOnlyList<PackageIdentity> DependantPackages => _packageWithDependants.DependantPackages;

        public IList<PackagingLogMessage> Issues { get; }

        public string Id { get; }

        public string Version { get; }

        public bool InstallAsTopLevel
        {
            get
            {
                return _installAsTopLevel;
            }
            set
            {
                _installAsTopLevel = value;
                OnPropertyChanged(nameof(InstallAsTopLevel));
            }
        }

        public bool IsTopLevel => _packageWithDependants.IsTopLevelPackage;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public NuGetProjectUpgradeDependencyItem(PackageIdentity package, PackageWithDependants packageWithDependants)
        {
            Identity = package;
            Id = package.Id;
            Version = package.Version.ToNormalizedString();
            Issues = new List<PackagingLogMessage>();
            _packageWithDependants = packageWithDependants;
            _installAsTopLevel = packageWithDependants.IsTopLevelPackage;
        }

        public override string ToString()
        {
            return !DependantPackages.Any()
                ? Identity.ToString()
                : $"{Identity} {string.Format(CultureInfo.CurrentCulture, Resources.NuGetUpgrade_PackageDependencyOf, string.Join(", ", DependantPackages))}";
        }
    }
}
