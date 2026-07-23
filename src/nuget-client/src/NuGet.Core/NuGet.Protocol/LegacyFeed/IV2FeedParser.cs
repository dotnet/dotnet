// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Protocol.Core.Types;

namespace NuGet.Protocol
{
    public interface IV2FeedParser
    {
        Task<V2FeedPage> GetPackagesPageAsync(
            string searchTerm,
            SearchFilter filters,
            int skip,
            int take,
            ILogger log,
            CancellationToken token);

        Task<V2FeedPage> GetSearchPageAsync(
            string searchTerm,
            SearchFilter filters,
            int skip,
            int take,
            ILogger log,
            CancellationToken token);
    }
}
