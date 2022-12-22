// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;

namespace NuGet.Protocol
{
    public class LocalPackageMetadataResource : PackageMetadataResource
    {
        private readonly FindLocalPackagesResource _localResource;

        public LocalPackageMetadataResource(FindLocalPackagesResource localResource)
        {
            if (localResource == null)
            {
                throw new ArgumentNullException(nameof(localResource));
            }

            _localResource = localResource;
        }

        public override Task<IEnumerable<IPackageSearchMetadata>> GetMetadataAsync(
            string packageId,
            bool includePrerelease,
            bool includeUnlisted,
            SourceCacheContext sourceCacheContext,
            ILogger log,
            CancellationToken token)
        {
            // All packages are considered listed within a local repo

            return Task.Run<IEnumerable<IPackageSearchMetadata>>(() =>
            {
                var metadataCache = new MetadataReferenceCache();
                return _localResource.FindPackagesById(packageId, log, token)
                    .Where(p => includePrerelease || !p.Identity.Version.IsPrerelease)
                    .Select(GetPackageMetadata)
                    .Select(p => metadataCache.GetObject(p))
                    .ToList();
            },
            token);
        }

        public override Task<IPackageSearchMetadata> GetMetadataAsync(
            PackageIdentity package,
            SourceCacheContext sourceCacheContext,
            ILogger log,
            CancellationToken token)
        {
            return Task.Run<IPackageSearchMetadata>(() =>
                {
                    var packageInfo = _localResource.GetPackage(package, log, token);
                    if (packageInfo != null)
                    {
                        return GetPackageMetadata(packageInfo);
                    }
                    return null;
                },
            token);
        }

        private static IPackageSearchMetadata GetPackageMetadata(LocalPackageInfo package) => new LocalPackageSearchMetadata(package);
    }
}
