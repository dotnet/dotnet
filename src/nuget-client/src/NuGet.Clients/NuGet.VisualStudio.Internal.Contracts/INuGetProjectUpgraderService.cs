// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Packaging.Core;

namespace NuGet.VisualStudio.Internal.Contracts
{
    public interface INuGetProjectUpgraderService : IDisposable
    {
        ValueTask<bool> IsProjectUpgradeableAsync(string projectId, CancellationToken cancellationToken);
        ValueTask<IReadOnlyCollection<IProjectContextInfo>> GetUpgradeableProjectsAsync(
            IReadOnlyCollection<string> projectIds,
            CancellationToken cancellationToken);
        ValueTask<string> BackupProjectAsync(string projectId, CancellationToken cancellationToken);
        ValueTask SaveProjectAsync(string projectId, CancellationToken cancellationToken);
        ValueTask InstallPackagesAsync(
            string projectId,
            IReadOnlyList<PackageIdentity> packageIdentities,
            CancellationToken cancellationToken);
        ValueTask UninstallPackagesAsync(
            string projectId,
            IReadOnlyList<PackageIdentity> packageIdentities,
            CancellationToken cancellationToken);
        ValueTask<IProjectContextInfo> UpgradeProjectToPackageReferenceAsync(
            string projectId,
            CancellationToken cancellationToken);
    }
}
