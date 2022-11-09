// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Threading;
using NuGet.Protocol.Core.Types;
using NuGet.VisualStudio.Internal.Contracts;

namespace NuGet.PackageManagement.VisualStudio
{
    public interface ISharedServiceState : IDisposable
    {
        AsyncLazy<IVsSolutionManager> SolutionManager { get; }
        ISourceRepositoryProvider SourceRepositoryProvider { get; }
        AsyncLazy<IReadOnlyCollection<SourceRepository>> SourceRepositories { get; }

        ValueTask<NuGetPackageManager> GetPackageManagerAsync(CancellationToken cancellationToken);
        ValueTask<IReadOnlyCollection<SourceRepository>> GetRepositoriesAsync(
            IReadOnlyCollection<PackageSourceContextInfo> packageSourceContextInfos,
            CancellationToken cancellationToken);
    }
}
