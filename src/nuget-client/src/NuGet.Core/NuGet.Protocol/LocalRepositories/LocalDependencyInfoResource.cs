// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Frameworks;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;

namespace NuGet.Protocol
{
    public class LocalDependencyInfoResource : DependencyInfoResource
    {
        private readonly FindLocalPackagesResource _localResource;
        private readonly SourceRepository _source;

        public LocalDependencyInfoResource(FindLocalPackagesResource localResource, SourceRepository source)
        {
            if (localResource == null)
            {
                throw new ArgumentNullException(nameof(localResource));
            }

            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            _localResource = localResource;
            _source = source;
        }

        /// <summary>
        /// Retrieve dependency info for a single package.
        /// </summary>
        /// <param name="package">package id and version</param>
        /// <param name="projectFramework">project target framework. This is used for finding the dependency group</param>
        /// <param name="token">cancellation token</param>
        public override Task<SourcePackageDependencyInfo> ResolvePackage(
            PackageIdentity package,
            NuGetFramework projectFramework,
            SourceCacheContext sourceCacheContext,
            ILogger log,
            CancellationToken token)
        {
            if (package == null)
            {
                throw new ArgumentNullException(nameof(package));
            }

            if (projectFramework == null)
            {
                throw new ArgumentNullException(nameof(projectFramework));
            }

            SourcePackageDependencyInfo result = null;

            try
            {
                // Retrieve all packages
                var repoPackage = _localResource.GetPackage(package, log, token);

                if (repoPackage != null)
                {
                    // convert to v3 type
                    result = CreateDependencyInfo(repoPackage, projectFramework);
                }
            }
            catch (Exception ex)
            {
                // Wrap exceptions coming from the server with a user friendly message
                var error = string.Format(CultureInfo.CurrentCulture, Strings.Protocol_PackageMetadataError, package, _localResource.Root);

                throw new FatalProtocolException(error, ex);
            }

            return Task.FromResult(result);
        }

        /// <summary>
        /// Retrieve dependency info for a single package.
        /// </summary>
        /// <param name="packageId">package id</param>
        /// <param name="projectFramework">project target framework. This is used for finding the dependency group</param>
        /// <param name="token">cancellation token</param>
        public override Task<IEnumerable<SourcePackageDependencyInfo>> ResolvePackages(
            string packageId,
            NuGetFramework projectFramework,
            SourceCacheContext sourceCacheContext,
            ILogger log,
            CancellationToken token)
        {
            if (packageId == null)
            {
                throw new ArgumentNullException(nameof(packageId));
            }

            if (projectFramework == null)
            {
                throw new ArgumentNullException(nameof(projectFramework));
            }

            var results = new List<SourcePackageDependencyInfo>();

            try
            {
                // Retrieve all packages
                foreach (var package in _localResource.FindPackagesById(packageId, log, token))
                {
                    // Convert to dependency info type
                    results.Add(CreateDependencyInfo(package, projectFramework));
                }
            }
            catch (NuGetProtocolException)
            {
                throw;
            }
            catch (Exception ex)
            {
                // Wrap exceptions coming from the server with a user friendly message
                var error = string.Format(
                    CultureInfo.CurrentCulture,
                    Strings.Protocol_PackageMetadataError,
                    packageId,
                    _localResource.Root);

                throw new FatalProtocolException(error, ex);
            }

            return Task.FromResult<IEnumerable<SourcePackageDependencyInfo>>(results);
        }

        /// <summary>
        /// Convert a package into a PackageDependencyInfo
        /// </summary>
        private SourcePackageDependencyInfo CreateDependencyInfo(LocalPackageInfo package, NuGetFramework projectFramework)
        {
            // Take only the dependency group valid for the project TFM
            var group = NuGetFrameworkUtility.GetNearest<PackageDependencyGroup>(package.Nuspec.GetDependencyGroups(), projectFramework);
            var dependencies = group?.Packages ?? Enumerable.Empty<PackageDependency>();

            var result = new SourcePackageDependencyInfo(
                package.Identity,
                dependencies,
                listed: true,
                source: _source,
                downloadUri: UriUtility.CreateSourceUri(package.Path, UriKind.Absolute),
                packageHash: null);

            return result;
        }
    }
}
