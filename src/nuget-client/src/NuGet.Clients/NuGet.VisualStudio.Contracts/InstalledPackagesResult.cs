// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace NuGet.VisualStudio.Contracts
{
    /// <summary>Result of a call to <see cref="INuGetProjectService.GetInstalledPackagesAsync"/></summary>
    /// <remarks>To create an instance, use <see cref="NuGetContractsFactory.CreateInstalledPackagesResult"/>.</remarks>
    public sealed class InstalledPackagesResult
    {
        /// <summary>The status of the result</summary>
        public InstalledPackageResultStatus Status { get; }

        /// <summary>List of packages in the project</summary>
        /// <remarks>May be null if <see cref="Status"/> was not successful</remarks>
        public IReadOnlyCollection<NuGetInstalledPackage> Packages { get; }

        // This class will hopefully use C# record types when that language feature becomes available, so make the constructor not-public, to prevent breaking change when records come out.
        internal InstalledPackagesResult(InstalledPackageResultStatus status, IReadOnlyCollection<NuGetInstalledPackage> packages)
        {
            Status = status;
            Packages = packages;
        }
    }
}
