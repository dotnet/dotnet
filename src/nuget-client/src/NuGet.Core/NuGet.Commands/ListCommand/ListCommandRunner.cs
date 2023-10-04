// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Packaging.Core;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;

namespace NuGet.Commands
{
    /// <summary>
    /// Command Runner used to run the business logic for nuget list command
    /// </summary>
    public class ListCommandRunner : IListCommandRunner
    {
        /// <summary>
        /// Executes the logic for nuget list command.
        /// </summary>
        /// <returns></returns>
        public async Task ExecuteCommand(ListArgs listArgs)

        {
            //Create SourceFeed for each packageSource
            var sourceFeeds = new List<ListResource>();
            // this is to avoid duplicate remote calls in case of duplicate final endpoints (Ex. api/index.json and /api/v2/ point to the same target)
            var sources = new HashSet<string>();

            foreach (PackageSource packageSource in listArgs.ListEndpoints)
            {
                var sourceRepository = Repository.Factory.GetCoreV3(packageSource);
                var feed = await sourceRepository.GetResourceAsync<ListResource>(listArgs.CancellationToken);

                if (feed != null)
                {
                    if (sources.Add(feed.Source))
                    {
                        sourceFeeds.Add(feed);
                    }
                }
                else
                {
                    listArgs.Logger.LogWarning(string.Format(CultureInfo.CurrentCulture, listArgs.ListCommandListNotSupported, packageSource.Source));
                }
            }

            WarnForHTTPSources(listArgs);

            var allPackages = new List<IEnumerableAsync<IPackageSearchMetadata>>();
            var log = listArgs.IsDetailed ? listArgs.Logger : NullLogger.Instance;
            foreach (var feed in sourceFeeds)
            {
                var packagesFromSource =
                    await feed.ListAsync(listArgs.Arguments.FirstOrDefault(), listArgs.Prerelease, listArgs.AllVersions,
                        listArgs.IncludeDelisted, log, listArgs.CancellationToken);
                allPackages.Add(packagesFromSource);
            }
            ComparePackageSearchMetadata comparer = new ComparePackageSearchMetadata();
            await PrintPackages(listArgs, new AggregateEnumerableAsync<IPackageSearchMetadata>(allPackages, comparer, comparer).GetEnumeratorAsync());
        }

        private static void WarnForHTTPSources(ListArgs listArgs)
        {
            List<PackageSource> httpPackageSources = null;
            foreach (PackageSource packageSource in listArgs.ListEndpoints)
            {
                if (packageSource.IsHttp && !packageSource.IsHttps && !packageSource.AllowInsecureConnections)
                {
                    if (httpPackageSources == null)
                    {
                        httpPackageSources = new();
                    }
                    httpPackageSources.Add(packageSource);
                }
            }

            if (httpPackageSources != null && httpPackageSources.Count != 0)
            {
                if (httpPackageSources.Count == 1)
                {
                    listArgs.Logger.LogWarning(
                        string.Format(CultureInfo.CurrentCulture,
                        Strings.Warning_HttpServerUsage,
                        "list",
                        httpPackageSources[0]));
                }
                else
                {
                    listArgs.Logger.LogWarning(
                        string.Format(CultureInfo.CurrentCulture,
                        Strings.Warning_HttpServerUsage_MultipleSources,
                        "list",
                        Environment.NewLine + string.Join(Environment.NewLine, httpPackageSources.Select(e => e.Name))));
                }
            }
        }

        private class ComparePackageSearchMetadata : IComparer<IPackageSearchMetadata>, IEqualityComparer<IPackageSearchMetadata>
        {
            public PackageIdentityComparer _comparer { get; set; } = PackageIdentityComparer.Default;
            public int Compare(IPackageSearchMetadata x, IPackageSearchMetadata y)
            {
                if (ReferenceEquals(x, y))
                {
                    return 0;
                }

                if (ReferenceEquals(x, null))
                {
                    return -1;
                }

                if (ReferenceEquals(y, null))
                {
                    return 1;
                }
                return _comparer.Compare(x.Identity, y.Identity);
            }

            public bool Equals(IPackageSearchMetadata x, IPackageSearchMetadata y)
            {
                if (ReferenceEquals(x, y))
                {
                    return true;
                }

                if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
                {
                    return false;
                }
                return _comparer.Equals(x.Identity, y.Identity);
            }

            public int GetHashCode(IPackageSearchMetadata obj)
            {
                if (ReferenceEquals(obj, null))
                {
                    return 0;
                }
                return _comparer.GetHashCode(obj.Identity);
            }
        }

        private async Task PrintPackages(ListArgs listArgs, IEnumeratorAsync<IPackageSearchMetadata> asyncEnumerator)
        {
            bool hasPackages = false;
            if (asyncEnumerator != null)
            {
                if (listArgs.IsDetailed)
                {
                    /***********************************************
                     * Package-Name
                     *  1.0.0.2010
                     *  This is the package Description
                     * 
                     * Package-Name-Two
                     *  2.0.0.2010
                     *  This is the second package Description
                     ***********************************************/
                    while (await asyncEnumerator.MoveNextAsync())
                    {
                        var p = asyncEnumerator.Current;
                        listArgs.PrintJustified(0, p.Identity.Id);
                        listArgs.PrintJustified(1, p.Identity.Version.ToFullString());
                        listArgs.PrintJustified(1, p.Description);
                        if (!string.IsNullOrEmpty(p.LicenseUrl?.OriginalString))
                        {
                            listArgs.PrintJustified(1,
                                string.Format(
                                    CultureInfo.InvariantCulture,
                                    listArgs.ListCommandLicenseUrl,
                                    p.LicenseUrl.OriginalString));
                        }
                        Console.WriteLine();
                        hasPackages = true;
                    }
                }
                else
                {
                    /***********************************************
                     * Package-Name 1.0.0.2010
                     * Package-Name-Two 2.0.0.2010
                     ***********************************************/
                    while (await asyncEnumerator.MoveNextAsync())
                    {
                        var p = asyncEnumerator.Current;
                        listArgs.PrintJustified(0, p.Identity.Id + " " + p.Identity.Version.ToFullString());
                        hasPackages = true;
                    }
                }
            }
            if (!hasPackages)
            {
                Console.WriteLine(listArgs.ListCommandNoPackages);
            }
        }
    }
}
