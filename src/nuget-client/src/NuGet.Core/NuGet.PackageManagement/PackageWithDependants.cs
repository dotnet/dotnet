// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using NuGet.Packaging.Core;

namespace NuGet.PackageManagement
{
    public class PackageWithDependants
    {
        public PackageIdentity Identity { get; }

        public IReadOnlyList<PackageIdentity> DependantPackages { get; }

        public PackageWithDependants(PackageIdentity identity, IReadOnlyList<PackageIdentity> dependingPackages)
        {
            Identity = identity ?? throw new ArgumentNullException(nameof(identity));
            DependantPackages = dependingPackages ?? throw new ArgumentNullException(nameof(dependingPackages));
        }

        public bool IsTopLevelPackage => !DependantPackages.Any();
    }
}
