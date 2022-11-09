// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Frameworks;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;

namespace NuGet.Protocol
{
    /// <summary>
    /// Retrieves all packages and dependencies from a V3 source.
    /// </summary>
    public sealed class DependencyInfoResourceV3 : DependencyInfoResource
    {
        private readonly HttpSource _client;
        private readonly RegistrationResourceV3 _regResource;
        private readonly SourceRepository _source;

        /// <summary>
        /// Dependency info resource
        /// </summary>
        /// <param name="client">Http client</param>
        /// <param name="regResource">Registration blob resource</param>
        public DependencyInfoResourceV3(HttpSource client, RegistrationResourceV3 regResource, SourceRepository source)
        {
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            if (regResource == null)
            {
                throw new ArgumentNullException(nameof(regResource));
            }

            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            _client = client;
            _regResource = regResource;
            _source = source;
        }

        /// <summary>
        /// Retrieve dependency info for a single package.
        /// </summary>
        /// <param name="package">package id and version</param>
        /// <param name="projectFramework">project target framework. This is used for finding the dependency group</param>
        /// <param name="token">cancellation token</param>
        /// <returns>
        /// Returns dependency info for the given package if it exists. If the package is not found null is
        /// returned.
        /// </returns>
        public override async Task<SourcePackageDependencyInfo> ResolvePackage(PackageIdentity package, NuGetFramework projectFramework, SourceCacheContext cacheContext, Common.ILogger log, CancellationToken token)
        {
            try
            {
                SourcePackageDependencyInfo result = null;

                // Construct the registration index url
                var uri = _regResource.GetUri(package.Id);

                // Retrieve the registration blob
                var singleVersion = new VersionRange(minVersion: package.Version, includeMinVersion: true, maxVersion: package.Version, includeMaxVersion: true);
                var regInfo = await ResolverMetadataClient.GetRegistrationInfo(_client, uri, package.Id, singleVersion, cacheContext, projectFramework, log, token);

                // regInfo is null if the server returns a 404 for the package to indicate that it does not exist
                if (regInfo != null)
                {
                    // Parse package and dependeny info from the blob
                    result = GetPackagesFromRegistration(regInfo, token).FirstOrDefault();
                }

                return result;
            }
            catch (Exception ex)
            {
                // Wrap exceptions coming from the server with a user friendly message
                var error = String.Format(CultureInfo.CurrentCulture, Strings.Protocol_PackageMetadataError, package, _source);

                throw new FatalProtocolException(error, ex);
            }
        }

        /// <summary>
        /// Retrieve the available packages and their dependencies.
        /// </summary>
        /// <remarks>Includes prerelease packages</remarks>
        /// <param name="packageId">package Id to search</param>
        /// <param name="projectFramework">project target framework. This is used for finding the dependency group</param>
        /// <param name="token">cancellation token</param>
        /// <returns>available packages and their dependencies</returns>
        public override async Task<IEnumerable<SourcePackageDependencyInfo>> ResolvePackages(string packageId, NuGetFramework projectFramework, SourceCacheContext cacheContext, Common.ILogger log, CancellationToken token)
        {
            try
            {
                var results = new List<SourcePackageDependencyInfo>();

                // Construct the registration index url
                var uri = _regResource.GetUri(packageId);

                // Retrieve the registration blob
                var regInfo = await ResolverMetadataClient.GetRegistrationInfo(_client, uri, packageId, VersionRange.All, cacheContext, projectFramework, log, token);

                // regInfo is null if the server returns a 404 for the package to indicate that it does not exist
                if (regInfo != null)
                {
                    // Parse package and dependeny info from the blob
                    var packages = GetPackagesFromRegistration(regInfo, token);

                    // Filter on prerelease
                    results.AddRange(packages);
                }

                return results;
            }
            catch (Exception ex)
            {
                // Wrap exceptions coming from the server with a user friendly message
                var error = string.Format(CultureInfo.CurrentCulture, Strings.Protocol_PackageMetadataError, packageId, _source);

                throw new FatalProtocolException(error, ex);
            }
        }

        /// <summary>
        /// Retrieve the available packages and their dependencies.
        /// </summary>
        /// <remarks>Includes prerelease packages</remarks>
        /// <param name="packageId">package Id to search</param>
        /// <param name="token">cancellation token</param>
        /// <returns>available packages and their dependencies</returns>
        public override Task<IEnumerable<RemoteSourceDependencyInfo>> ResolvePackages(string packageId, SourceCacheContext cacheContext, Common.ILogger log, CancellationToken token)
        {
            try
            {
                // Construct the registration index url
                var uri = _regResource.GetUri(packageId);

                // Retrieve the registration blob
                return ResolverMetadataClient.GetDependencies(_client, uri, packageId, VersionRange.All, cacheContext, log, token);
            }
            catch (Exception ex)
            {
                // Wrap exceptions coming from the server with a user friendly message
                var error = string.Format(CultureInfo.CurrentCulture, Strings.Protocol_PackageMetadataError, packageId, _source);

                throw new FatalProtocolException(error, ex);
            }
        }

        /// <summary>
        /// Retrieve dependency info from a registration blob
        /// </summary>
        private IEnumerable<SourcePackageDependencyInfo> GetPackagesFromRegistration(
            RegistrationInfo registration,
            CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            foreach (var pkgInfo in registration.Packages)
            {
                var dependencies = pkgInfo.Dependencies.Select(dep => new PackageDependency(dep.Id, dep.Range));

                yield return new SourcePackageDependencyInfo(
                    registration.Id,
                    pkgInfo.Version,
                    dependencies,
                    pkgInfo.Listed,
                    _source,
                    pkgInfo.PackageContent,
                    packageHash: null);
            }

            yield break;
        }
    }
}
