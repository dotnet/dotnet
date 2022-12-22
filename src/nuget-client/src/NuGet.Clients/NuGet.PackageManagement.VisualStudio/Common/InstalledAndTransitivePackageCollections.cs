// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace NuGet.PackageManagement.VisualStudio
{
    /// <summary>
    /// Struct containing collections of installed and transitive packages
    /// </summary>
    public struct InstalledAndTransitivePackageCollections
    {
        public PackageCollection InstalledPackages { get; }
        public PackageCollection TransitivePackages { get; }

        public InstalledAndTransitivePackageCollections(PackageCollection installedPackages, PackageCollection transitivePackages)
        {
            InstalledPackages = installedPackages;
            TransitivePackages = transitivePackages;
        }

        public override bool Equals(object obj)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        public static bool operator ==(InstalledAndTransitivePackageCollections left, InstalledAndTransitivePackageCollections right)
        {
            throw new NotImplementedException();
        }

        public static bool operator !=(InstalledAndTransitivePackageCollections left, InstalledAndTransitivePackageCollections right)
        {
            throw new NotImplementedException();
        }
    }
}
