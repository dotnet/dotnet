// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;

namespace NuGet.Packaging
{
    /// <summary>
    /// RepositorySignatureInfoProvdier is a static cache for repository signature information for package source.
    /// </summary>
    public class RepositorySignatureInfoProvider
    {
        private ConcurrentDictionary<string, RepositorySignatureInfo> _dict = new ConcurrentDictionary<string, RepositorySignatureInfo>();

        public static RepositorySignatureInfoProvider Instance { get; } = new RepositorySignatureInfoProvider();

        /// <summary>
        /// Try to get repository signature information for the source.
        /// </summary>
        /// <param name="source">Package source URL.</param>
        /// <param name="repositorySignatureInfo">Contains the RepositorySignatureInfo when the method returns. It is null if repository signature information is unavailable.</param>
        /// <returns>True if the repository signature information was found. Otherwise, False.</returns>
        public bool TryGetRepositorySignatureInfo(string source, out RepositorySignatureInfo repositorySignatureInfo)
        {
            repositorySignatureInfo = null;

            return _dict.TryGetValue(source, out repositorySignatureInfo);
        }

        /// <summary>
        /// Add or update the repository signature information for the source.
        /// </summary>
        /// <param name="source">Package source URL.</param>
        /// <param name="repositorySignatureInfo">RepositorySignatureInfo for the source url.</param>
        public void AddOrUpdateRepositorySignatureInfo(string source, RepositorySignatureInfo repositorySignatureInfo)
        {
            _dict[source] = repositorySignatureInfo ?? throw new ArgumentNullException(nameof(repositorySignatureInfo));
        }
    }
}
