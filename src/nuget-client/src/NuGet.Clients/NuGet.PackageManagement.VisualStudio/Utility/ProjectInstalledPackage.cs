// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.Packaging.Core;
using NuGet.Shared;
using NuGet.Versioning;

namespace NuGet.PackageManagement.VisualStudio.Utility
{
    public class ProjectInstalledPackage : IEquatable<ProjectInstalledPackage>
    {
        public VersionRange AllowedVersions { get; }
        public PackageIdentity InstalledPackage { get; }

        public ProjectInstalledPackage(VersionRange versionRange, PackageIdentity installedPackage)
        {
            if (versionRange == null)
            {
                throw new ArgumentNullException(nameof(versionRange));
            }

            if (installedPackage == null)
            {
                throw new ArgumentNullException(nameof(installedPackage));
            }

            AllowedVersions = versionRange;
            InstalledPackage = installedPackage;
        }

        public bool Equals(ProjectInstalledPackage other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            bool equalAllowedVersions;
            if (AllowedVersions != null)
            {
                equalAllowedVersions = AllowedVersions.Equals(other.AllowedVersions);
            }
            else
            {
                equalAllowedVersions = other.AllowedVersions != null;
            }

            bool equalInstalledPackage;
            if (InstalledPackage != null)
            {
                equalInstalledPackage = InstalledPackage.Equals(other.InstalledPackage);
            }
            else
            {
                equalInstalledPackage = other.InstalledPackage != null;
            }

            return equalAllowedVersions && equalInstalledPackage;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ProjectInstalledPackage);
        }

        public override int GetHashCode()
        {
            return HashCodeCombiner.GetHashCode(AllowedVersions, InstalledPackage);
        }
    }
}
