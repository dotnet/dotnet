// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace NuGet.VisualStudio.Internal.Contracts
{
    /// <summary>
    /// Contains information about installed and transitive packages
    /// </summary>
    public interface IInstalledAndTransitivePackages
    {
        IReadOnlyCollection<IPackageReferenceContextInfo> InstalledPackages { get; }
        IReadOnlyCollection<ITransitivePackageReferenceContextInfo> TransitivePackages { get; }
    }
}
