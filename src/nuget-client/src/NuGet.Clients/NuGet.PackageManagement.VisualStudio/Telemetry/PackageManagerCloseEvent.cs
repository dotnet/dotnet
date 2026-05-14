// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.Common;

namespace NuGet.PackageManagement.VisualStudio
{
    public sealed class PackageManagerCloseEvent : TelemetryEvent
    {
        private const string EventName = "PMUIClose";

        public PackageManagerCloseEvent(
            Guid parentId,
            bool isSolutionLevel,
            string tab,
            PackageManagerInstalledTabData installedTabData) : base(EventName)
        {
            base["ParentId"] = parentId.ToString();
            base["IsSolutionLevel"] = isSolutionLevel;
            base["Tab"] = tab;

            string prefix = PackageManagerInstalledTabData.PropertyPrefix;
            base[prefix + nameof(installedTabData.TopLevelPackageSelectedCount)] = installedTabData.TopLevelPackageSelectedCount;
            base[prefix + nameof(installedTabData.TransitivePackageSelectedCount)] = installedTabData.TransitivePackageSelectedCount;
            base[prefix + nameof(installedTabData.TopLevelPackagesExpandedCount)] = installedTabData.TopLevelPackagesExpandedCount;
            base[prefix + nameof(installedTabData.TopLevelPackagesCollapsedCount)] = installedTabData.TopLevelPackagesCollapsedCount;
            base[prefix + nameof(installedTabData.TransitivePackagesExpandedCount)] = installedTabData.TransitivePackagesExpandedCount;
            base[prefix + nameof(installedTabData.TransitivePackagesCollapsedCount)] = installedTabData.TransitivePackagesCollapsedCount;
        }
    }
}
