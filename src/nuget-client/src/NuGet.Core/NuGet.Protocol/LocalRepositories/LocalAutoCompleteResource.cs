// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;

namespace NuGet.Protocol
{
    public class LocalAutoCompleteResource : AutoCompleteResource
    {
        private readonly FindLocalPackagesResource _localResource;

        public LocalAutoCompleteResource(FindLocalPackagesResource localResource)
        {
            if (localResource == null)
            {
                throw new ArgumentNullException(nameof(localResource));
            }

            _localResource = localResource;
        }

        public override Task<IEnumerable<string>> IdStartsWith(
                    string packageIdPrefix,
                    bool includePrerelease,
                    ILogger log,
                    CancellationToken token)
        {
            return GetPackageIdsFromLocalPackageRepository(
                packageIdPrefix,
                includePrerelease: includePrerelease,
                log: log,
                token: token);
        }

        public override Task<IEnumerable<NuGetVersion>> VersionStartsWith(
            string packageId,
            string versionPrefix,
            bool includePrerelease,
            SourceCacheContext sourceCacheContext,
            ILogger log,
            CancellationToken token)
        {
            return GetPackageVersionsFromLocalPackageRepository(
                packageId,
                versionPrefix,
                includePrerelease,
                log,
                token);
        }

        private async Task<IEnumerable<string>> GetPackageIdsFromLocalPackageRepository(
            string searchFilter,
            bool includePrerelease,
            ILogger log,
            CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            return await Task.Run(() =>
            {
                var packages = _localResource.GetPackages(log, token);

                if (!string.IsNullOrEmpty(searchFilter))
                {
                    packages = packages.Where(p =>
                        p.Identity.Id.StartsWith(searchFilter, StringComparison.OrdinalIgnoreCase));
                }

                if (!includePrerelease)
                {
                    packages = packages.Where(p => !p.Identity.Version.IsPrerelease);
                }

                return packages.Select(p => p.Identity.Id)
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .Take(30)
                    .ToList();
            },
            token);
        }

        protected async Task<IEnumerable<NuGetVersion>> GetPackageVersionsFromLocalPackageRepository(
            string packageId,
            string versionPrefix,
            bool includePrerelease,
            ILogger log,
            CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            return await Task.Run(() =>
            {
                var packages = _localResource.FindPackagesById(packageId, log, token);

                if (!includePrerelease)
                {
                    packages = packages.Where(p => !p.Identity.Version.IsPrerelease);
                }

                // Check both the non-normalized and full string versions
                return packages.Where(p => p.Identity.Version.ToString()
                    .StartsWith(versionPrefix, StringComparison.OrdinalIgnoreCase)
                    || p.Identity.Version.ToFullString().StartsWith(versionPrefix, StringComparison.OrdinalIgnoreCase))
                    .Select(p => p.Identity.Version)
                    .ToList();
            },
            token);
        }
    }
}
