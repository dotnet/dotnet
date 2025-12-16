// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using NuGet.DependencyResolver;
using NuGet.Protocol;
using NuGet.Repositories;

namespace NuGet.Commands
{
    /// <summary>
    /// Feed providers
    /// </summary>
    public class RestoreCommandProviders
    {

        internal RestoreCommandProviders(
            NuGetv3LocalRepository globalPackages,
            IReadOnlyList<NuGetv3LocalRepository> fallbackPackageFolders,
            IReadOnlyList<IRemoteDependencyProvider> localProviders,
            IReadOnlyList<IRemoteDependencyProvider> remoteProviders,
            LocalPackageFileCache packageFileCache,
            IReadOnlyList<IVulnerabilityInformationProvider> vulnerabilityInformationProviders)
        {
            GlobalPackages = globalPackages ?? throw new ArgumentNullException(nameof(globalPackages));
            LocalProviders = localProviders ?? throw new ArgumentNullException(nameof(localProviders));
            RemoteProviders = remoteProviders ?? throw new ArgumentNullException(nameof(remoteProviders));
            FallbackPackageFolders = fallbackPackageFolders ?? throw new ArgumentNullException(nameof(fallbackPackageFolders));
            PackageFileCache = packageFileCache ?? throw new ArgumentNullException(nameof(packageFileCache));
            VulnerabilityInfoProviders = vulnerabilityInformationProviders;
        }

        /// <summary>
        /// A <see cref="NuGetv3LocalRepository"/> repository may be passed in as part of the request.
        /// This allows multiple restores to share the same cache for the global packages folder
        /// and reduce disk hits.
        /// </summary>
        public NuGetv3LocalRepository GlobalPackages { get; }

        public IReadOnlyList<NuGetv3LocalRepository> FallbackPackageFolders { get; }

        public IReadOnlyList<IRemoteDependencyProvider> LocalProviders { get; }

        public IReadOnlyList<IRemoteDependencyProvider> RemoteProviders { get; }

        public LocalPackageFileCache PackageFileCache { get; }

        internal IReadOnlyList<IVulnerabilityInformationProvider> VulnerabilityInfoProviders { get; }
    }
}
