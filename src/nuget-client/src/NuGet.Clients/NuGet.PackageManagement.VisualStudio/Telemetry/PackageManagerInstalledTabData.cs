// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.PackageManagement.VisualStudio
{
    public class PackageManagerInstalledTabData
    {
        public const string PropertyPrefix = "Installed.";

        public uint TopLevelPackageSelectedCount { get; set; }

        public uint TransitivePackageSelectedCount { get; set; }

        public uint TopLevelPackagesExpandedCount { get; set; }

        public uint TopLevelPackagesCollapsedCount { get; set; }

        public uint TransitivePackagesExpandedCount { get; set; }

        public uint TransitivePackagesCollapsedCount { get; set; }
    }
}
