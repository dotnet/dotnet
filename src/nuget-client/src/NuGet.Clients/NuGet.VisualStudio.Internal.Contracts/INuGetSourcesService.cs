// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NuGet.VisualStudio.Internal.Contracts
{
    public interface INuGetSourcesService : IDisposable
    {
        /// <remarks> First available in version 1.0.1 </remarks>
        event EventHandler<IReadOnlyList<PackageSourceContextInfo>>? PackageSourcesChanged;

        /// <remarks> First available in version 1.0.1 </remarks>
        ValueTask<string?> GetActivePackageSourceNameAsync(CancellationToken cancellationToken);

        /// <remarks> First available in version 1.0.1 </remarks>
        ValueTask SavePackageSourceContextInfosAsync(IReadOnlyList<PackageSourceContextInfo> sources, CancellationToken cancellationToken);

        ValueTask<IReadOnlyList<PackageSourceContextInfo>> GetPackageSourcesAsync(CancellationToken cancellationToken);
    }
}
