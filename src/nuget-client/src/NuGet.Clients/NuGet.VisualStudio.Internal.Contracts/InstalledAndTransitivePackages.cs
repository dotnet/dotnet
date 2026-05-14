// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace NuGet.VisualStudio.Internal.Contracts
{
    public sealed class InstalledAndTransitivePackages : IInstalledAndTransitivePackages
    {
        public InstalledAndTransitivePackages(IReadOnlyCollection<IPackageReferenceContextInfo>? installedPackages, IReadOnlyCollection<ITransitivePackageReferenceContextInfo>? transitivePackages)
        {
            InstalledPackages = installedPackages ?? Array.Empty<IPackageReferenceContextInfo>();
            TransitivePackages = transitivePackages ?? Array.Empty<ITransitivePackageReferenceContextInfo>();
        }

        public IReadOnlyCollection<IPackageReferenceContextInfo> InstalledPackages { get; }
        public IReadOnlyCollection<ITransitivePackageReferenceContextInfo> TransitivePackages { get; }
    }
}
