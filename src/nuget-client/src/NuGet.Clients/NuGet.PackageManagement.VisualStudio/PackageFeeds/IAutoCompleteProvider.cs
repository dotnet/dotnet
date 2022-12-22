// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Versioning;

namespace NuGet.PackageManagement.VisualStudio
{
    internal interface IAutoCompleteProvider
    {
        Task<IEnumerable<string>> IdStartsWithAsync(string packageIdPrefix, bool includePrerelease, CancellationToken cancellationToken);
        Task<IEnumerable<NuGetVersion>> VersionStartsWithAsync(string packageId, string versionPrefix, bool includePrerelease, CancellationToken cancellationToken);
    }
}
