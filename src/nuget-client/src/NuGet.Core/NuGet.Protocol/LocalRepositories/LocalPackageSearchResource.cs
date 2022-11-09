// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;

namespace NuGet.Protocol
{
    public class LocalPackageSearchResource : PackageSearchResource
    {
        private readonly FindLocalPackagesResource _localResource;

        public LocalPackageSearchResource(FindLocalPackagesResource localResource)
        {
            if (localResource == null)
            {
                throw new ArgumentNullException(nameof(localResource));
            }

            _localResource = localResource;
        }

        public async override Task<IEnumerable<IPackageSearchMetadata>> SearchAsync(
            string searchTerm,
            SearchFilter filters,
            int skip,
            int take,
            ILogger log,
            CancellationToken token)
        {
            return await Task.Factory.StartNew(() =>
            {
                // Check if source is available.
                if (!IsLocalOrUNC(_localResource.Root))
                {
                    throw new InvalidOperationException(string.Format(
                        CultureInfo.CurrentCulture,
                        Strings.Protocol_Search_LocalSourceNotFound,
                        _localResource.Root));
                }

                var query = _localResource.GetPackages(log, token);

                // Filter on prerelease
                query = query.Where(package => filters.IncludePrerelease || !package.Identity.Version.IsPrerelease);

                // Filter on search terms
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    var terms = searchTerm.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    query = query.Where(package => ContainsAnyTerm(terms, package));
                }

                // Collapse to the highest version per id, if necessary
                var collapsedQuery = filters?.Filter == SearchFilterType.IsLatestVersion ||
                                     filters?.Filter == SearchFilterType.IsAbsoluteLatestVersion
                                     ? CollapseToHighestVersion(query) : query;

                // execute the query
                var packages = collapsedQuery
                    .Skip(skip)
                    .Take(take)
                    .ToArray();

                // Create final results and retrieve all versions for each package.
                return packages
                    .Select(package => CreatePackageSearchResult(package, filters, log, token))
                    .ToArray();
            }, token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        /// <summary>
        /// Search Id, Tags, and Description to match the legacy local search behavior.
        /// </summary>
        private static bool ContainsAnyTerm(string[] terms, LocalPackageInfo package)
        {
            var id = package.Identity.Id;
            var tags = package.Nuspec.GetTags();
            var description = package.Nuspec.GetDescription();

            foreach (var term in terms)
            {
                if (ContainsTerm(term, id)
                    || ContainsTerm(term, tags)
                    || ContainsTerm(term, description))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool ContainsTerm(string search, string property)
        {
            int? pos = property?.IndexOf(search, StringComparison.OrdinalIgnoreCase);

            return (pos.HasValue && pos.Value > -1);
        }

        private IPackageSearchMetadata CreatePackageSearchResult(
            LocalPackageInfo package,
            SearchFilter filter,
            ILogger log,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var metadata = new LocalPackageSearchMetadata(package);

            return metadata
                .WithVersions(() => GetVersions(_localResource, package, filter, log, CancellationToken.None));
        }

        private static List<VersionInfo> GetVersions(
            FindLocalPackagesResource localResource,
            LocalPackageInfo package,
            SearchFilter filter,
            ILogger log,
            CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            // apply the filters to the version list returned
            var versions = localResource.FindPackagesById(package.Identity.Id, log, token)
                .Where(v => filter.IncludePrerelease || !v.Identity.Version.IsPrerelease)
                .Select(p => new VersionInfo(p.Identity.Version, downloadCount: 0))
                .OrderByDescending(v => v.Version, VersionComparer.Default)
                .ToList();

            // Add in the current package if it does not already exist
            if (!versions.Any(v => v.Version == package.Identity.Version))
            {
                var packageVersionInfo = new VersionInfo(package.Identity.Version, downloadCount: 0)
                {
                    PackageSearchMetadata = new LocalPackageSearchMetadata(package)
                };

                versions.Add(packageVersionInfo);
            }

            return versions;
        }

        private static bool IsLocalOrUNC(string currentSource)
        {
            Uri currentURI = UriUtility.TryCreateSourceUri(currentSource, UriKind.Absolute);
            if (currentURI != null)
            {
                if (currentURI.IsFile || currentURI.IsUnc)
                {
                    if (Directory.Exists(UriUtility.GetLocalPath(currentSource)))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Returns a distinct set of elements using the comparer specified. This implementation will pick the last occurrence
        /// of each element instead of picking the first. This method assumes that similar items occur in order.
        /// </summary>        
        private static IEnumerable<LocalPackageInfo> CollapseToHighestVersion(IEnumerable<LocalPackageInfo> source)
        {
            bool first = true;
            bool maxElementHasValue = false;
            LocalPackageInfo previousElement = null;
            LocalPackageInfo maxElement = null;

            foreach (LocalPackageInfo element in source)
            {
                // If we're starting a new group then return the max element from the last group
                if (!first && !StringComparer.OrdinalIgnoreCase.Equals(element.Identity.Id, previousElement.Identity.Id))
                {
                    yield return maxElement;

                    // Reset the max element
                    maxElementHasValue = false;
                }

                // If the current max element has a value and is bigger or doesn't have a value then update the max
                if (!maxElementHasValue
                    || (maxElementHasValue
                        && VersionComparer.VersionRelease.Compare(maxElement.Identity.Version, element.Identity.Version) < 0))
                {
                    maxElement = element;
                    maxElementHasValue = true;
                }

                previousElement = element;
                first = false;
            }

            if (!first)
            {
                yield return maxElement;
            }

            yield break;
        }
    }
}
