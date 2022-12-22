// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;

namespace NuGet.PackageManagement.UI
{
    public class ShowUpdatePackageOptions
    {
        private static ShowUpdatePackageOptions UpdateAllPackagesInstance;

        private ShowUpdatePackageOptions(bool shouldUpdateAllPackages = false, IEnumerable<string> packagesToUpdate = null)
        {
            ShouldUpdateAllPackages = shouldUpdateAllPackages;
            PackagesToUpdate = packagesToUpdate ?? Enumerable.Empty<string>();
        }

        public static ShowUpdatePackageOptions UpdateAllPackages()
        {
            if (UpdateAllPackagesInstance == null)
            {
                UpdateAllPackagesInstance = new ShowUpdatePackageOptions(shouldUpdateAllPackages: true);
            }

            return UpdateAllPackagesInstance;
        }

        public static ShowUpdatePackageOptions UpdatePackages(IEnumerable<string> packagesToUpdate)
        {
            return new ShowUpdatePackageOptions(shouldUpdateAllPackages: false, packagesToUpdate);
        }

        public bool ShouldUpdateAllPackages { get; }

        public IEnumerable<string> PackagesToUpdate { get; }
    }
}
