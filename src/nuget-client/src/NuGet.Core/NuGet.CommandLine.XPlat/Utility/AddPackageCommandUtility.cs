// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;

namespace NuGet.CommandLine.XPlat.Utility
{
    internal static class AddPackageCommandUtility
    {
        /// <summary>
        /// Return the latest version available in the sources
        /// </summary>
        /// <param name="sources">Sources to look at</param>
        /// <param name="logger">Logger</param>
        /// <param name="packageId">Package to look for</param>
        /// <param name="prerelease">Whether to include prerelease versions</param>
        /// <returns>Return the latest version available from multiple sources and if no version is found returns null.</returns>

        public static async Task<NuGetVersion> GetLatestVersionFromSourcesAsync(IList<PackageSource> sources, ILogger logger, string packageId, bool prerelease)
        {
            var maxTasks = Environment.ProcessorCount;
            var tasks = new List<Task<NuGetVersion>>();
            var latestReleaseList = new List<NuGetVersion>();

            foreach (var source in sources)
            {
                tasks.Add(Task.Run(() => GetLatestVersionFromSourceAsync(source, logger, packageId, prerelease)));
                if (maxTasks <= tasks.Count)
                {
                    var finishedTask = await Task.WhenAny(tasks);
                    tasks.Remove(finishedTask);
                    latestReleaseList.Add(await finishedTask);
                }
            }

            await Task.WhenAll(tasks);

            foreach (var t in tasks)
            {
                var result = await t;
                if (result != null)
                {
                    latestReleaseList.Add(result);
                }
            }

            return latestReleaseList.Max();
        }

        /// <summary>
        /// Return the latest version of the source
        /// </summary>
        /// <param name="source">Source to look at</param>
        /// <param name="logger">Logger</param>
        /// <param name="packageId">Package to look for</param>
        /// <param name="prerelease">Whether to include prerelease versions</param>
        /// <returns>Returns the latest version available from a source or a null if non is found.</returns>
        public static async Task<NuGetVersion> GetLatestVersionFromSourceAsync(PackageSource source, ILogger logger, string packageId, bool prerelease)
        {
            SourceRepository repository = Repository.Factory.GetCoreV3(source);
            PackageMetadataResource resource = await repository.GetResourceAsync<PackageMetadataResource>();

            using (var cache = new SourceCacheContext())
            {
                IEnumerable<IPackageSearchMetadata> packages = await resource.GetMetadataAsync(
                    packageId,
                    includePrerelease: prerelease,
                    includeUnlisted: false,
                    cache,
                    logger,
                    CancellationToken.None
                );

                return packages?.Max(x => x.Identity.Version);
            }
        }

        /// <summary>
        /// Returns the PackageSource with its credentials if available
        /// </summary>
        /// <param name="requestedSources">Sources to match</param>
        /// <param name="configFilePaths">Config to use for credentials</param>
        /// <returns>Return a list of package sources</returns>
        public static List<PackageSource> EvaluateSources(IList<PackageSource> requestedSources, IList<string> configFilePaths)
        {
            using (var settingsLoadingContext = new SettingsLoadingContext())
            {
                ISettings settings = Settings.LoadImmutableSettingsGivenConfigPaths(configFilePaths, settingsLoadingContext);
                var packageSources = new List<PackageSource>();

                var packageSourceProvider = new PackageSourceProvider(settings);
                IEnumerable<PackageSource> packageProviderSources = packageSourceProvider.LoadPackageSources();

                for (int i = 0; i < requestedSources.Count; i++)
                {
                    PackageSource matchedSource = packageProviderSources.FirstOrDefault(e => e.Source == requestedSources[i].Source);
                    if (matchedSource == null)
                    {
                        packageSources.Add(requestedSources[i]);
                    }
                    else
                    {
                        packageSources.Add(matchedSource);
                    }
                }

                return packageSources;
            }
        }
    }
}
