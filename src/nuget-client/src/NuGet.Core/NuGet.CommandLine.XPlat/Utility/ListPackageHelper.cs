// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace NuGet.CommandLine.XPlat.Utility
{
    /// <summary>
    /// A static class containing helper functions for the list package command
    /// </summary>
    internal static class ListPackageHelper
    {
        internal static readonly Func<InstalledPackageReference, bool> TopLevelPackagesFilterForOutdated =
            p => !p.AutoReference && (p.LatestPackageMetadata == null
                 || p.ResolvedPackageMetadata.Identity.Version < p.LatestPackageMetadata.Identity.Version);
        internal static readonly Func<InstalledPackageReference, bool> TransitivePackagesFilterForOutdated =
            p => p.LatestPackageMetadata == null
                 || p.ResolvedPackageMetadata.Identity.Version < p.LatestPackageMetadata.Identity.Version;

        internal static readonly Func<InstalledPackageReference, bool> PackagesFilterForDeprecated =
            p => p.ResolvedPackageMetadata.GetDeprecationMetadataAsync().Result != null;
        internal static readonly Func<InstalledPackageReference, bool> LatestPackagesFilterForDeprecated =
            p => p.LatestPackageMetadata.GetDeprecationMetadataAsync().Result != null;

        internal static readonly Func<InstalledPackageReference, bool> PackagesFilterForVulnerable =
            p => p.ResolvedPackageMetadata.Vulnerabilities != null;
        internal static readonly Func<InstalledPackageReference, bool> LatestPackagesFilterForVulnerable =
            p => p.LatestPackageMetadata.Vulnerabilities != null;
    }
}
