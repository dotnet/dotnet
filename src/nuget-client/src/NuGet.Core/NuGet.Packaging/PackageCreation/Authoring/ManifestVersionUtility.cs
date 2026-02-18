// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Linq;

namespace NuGet.Packaging
{
    public static class ManifestVersionUtility
    {
        public const int DefaultVersion = 1;
        public const int SemverVersion = 3;

        public const int TargetFrameworkSupportForDependencyContentsAndToolsVersion = 4;
        public const int TargetFrameworkSupportForReferencesVersion = 5;
        public const int XdtTransformationVersion = 6;

        public static int GetManifestVersion(ManifestMetadata metadata)
        {
            if (metadata == null)
            {
                throw new ArgumentNullException(nameof(metadata));
            }

            // Important: always add newer version checks at the top

            if ((metadata.PackageAssemblyReferences != null &&
                metadata.PackageAssemblyReferences.Any(r => r.TargetFramework != null && r.TargetFramework.IsSpecificFramework)) ||
                !string.IsNullOrEmpty(metadata.MinClientVersionString))
            {
                return 5;
            }

            if (metadata.DependencyGroups != null &&
                metadata.DependencyGroups.Any(d => d.TargetFramework != null && d.TargetFramework.IsSpecificFramework))
            {
                return 4;
            }

            if (metadata.Version != null && metadata.Version.IsPrerelease)
            {
                return 3;
            }

            if (!string.IsNullOrEmpty(metadata.ReleaseNotes) ||
                !string.IsNullOrEmpty(metadata.Copyright) ||
                (metadata.PackageAssemblyReferences?.Any() ?? false))
            {
                return 2;
            }

            return DefaultVersion;
        }
    }
}
