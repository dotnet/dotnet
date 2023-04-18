// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NuGet.Protocol.Core.Types
{
    /// <summary>
    /// INuGetResourceProviders are imported by SourceRepository. They exist as singletons which span all sources,
    /// and are responsible
    /// for determining if they should be used for the given source when TryCreate is called.
    /// The provider determines the caching. Resources may be cached per source, but they are normally created new
    /// each time
    /// to allow for caching within the context they were created in.
    /// Providers may retrieve other resources from the source repository and pass them to the resources they
    /// create in order
    /// to build on them.
    /// </summary>
    public interface INuGetResourceProvider
    {
        /// <summary>
        /// Attempts to create a resource for this source.
        /// </summary>
        /// <remarks>
        /// The provider may return true but null for the resource if the
        /// provider determines that it should not exist.
        /// </remarks>
        /// <param name="source">Source repository</param>
        /// <returns>True if this provider handles the input source.</returns>
        Task<Tuple<bool, INuGetResource?>> TryCreate(SourceRepository source, CancellationToken token);

        /// <summary>
        /// Resource type provided
        /// </summary>
        Type ResourceType { get; }

        /// <summary>
        /// Name of the provider. This is used for ordering.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Names of providers this should be ordered BEFORE
        /// Gives this instance a higher priority.
        /// </summary>
        /// <remarks>If provider: default is named here, this provider will be called BEFORE default</remarks>
        IEnumerable<string> Before { get; }

        /// <summary>
        /// Names of providers this should be ordered AFTER.
        /// Gives this instance a lower priority.
        /// </summary>
        /// <remarks>If provider: default is named here, this provider will be called AFTER default</remarks>
        IEnumerable<string> After { get; }
    }
}
