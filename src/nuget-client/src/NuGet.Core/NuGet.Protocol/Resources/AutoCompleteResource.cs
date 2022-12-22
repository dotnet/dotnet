// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Versioning;

namespace NuGet.Protocol.Core.Types
{
    public abstract class AutoCompleteResource : INuGetResource
    {
        public abstract Task<IEnumerable<string>> IdStartsWith(
            string packageIdPrefix,
            bool includePrerelease,
            Common.ILogger log,
            CancellationToken token);

        public abstract Task<IEnumerable<NuGetVersion>> VersionStartsWith(
            string packageId,
            string versionPrefix,
            bool includePrerelease,
            SourceCacheContext sourceCacheContext,
            Common.ILogger log,
            CancellationToken token);
    }
}
