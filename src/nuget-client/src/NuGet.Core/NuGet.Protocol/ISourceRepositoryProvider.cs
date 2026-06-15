// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using NuGet.Configuration;

namespace NuGet.Protocol.Core.Types
{
    /// <summary>
    /// SourceRepositoryProvider composes resource providers into source repositories.
    /// </summary>
    public interface ISourceRepositoryProvider
    {
        /// <summary>
        /// Retrieve repositories
        /// </summary>
        IEnumerable<SourceRepository> GetRepositories();

        /// <summary>
        /// Create a repository for one time use.
        /// </summary>
        SourceRepository CreateRepository(PackageSource source);

        /// <summary>
        /// Create a repository for one time use.
        /// </summary>
        SourceRepository CreateRepository(PackageSource source, FeedType type);

        /// <summary>
        /// Gets the package source provider
        /// </summary>
        IPackageSourceProvider PackageSourceProvider { get; }
    }
}
