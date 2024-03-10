// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Protocol.Core.Types;
using NuGet.Protocol;
using NuGet.Commands;

namespace NuGet.CommandLine.XPlat
{
    internal static class PackageSearchRunner
    {
        /// <summary>
        /// Runs the search operation asynchronously using the provided parameters.
        /// </summary>
        /// <param name="sourceProvider">The provider for package sources.</param>
        /// <param name="packageSearchArgs">Package search arguments</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public static async Task<int> RunAsync(
            IPackageSourceProvider sourceProvider,
            PackageSearchArgs packageSearchArgs,
            CancellationToken cancellationToken)
        {
            IList<PackageSource> listEndpoints;
            IPackageSearchResultRenderer packageSearchResultRenderer;

            if (packageSearchArgs.Format == PackageSearchFormat.Json)
            {
                packageSearchResultRenderer = new PackageSearchResultJsonRenderer(packageSearchArgs.Logger, packageSearchArgs.Verbosity, packageSearchArgs.ExactMatch);
            }
            else
            {
                packageSearchResultRenderer = new PackageSearchResultTableRenderer(packageSearchArgs.SearchTerm, packageSearchArgs.Logger, packageSearchArgs.Verbosity, packageSearchArgs.ExactMatch);
            }

            packageSearchResultRenderer.Start();

            try
            {
                listEndpoints = GetPackageSources(packageSearchArgs.Sources, sourceProvider);
            }
            catch (ArgumentException ex)
            {
                packageSearchResultRenderer.Add(new PackageSearchProblem(PackageSearchProblemType.Error, ex.Message));
                packageSearchResultRenderer.Finish();

                return ExitCodes.Error;
            }

            WarnForHTTPSources(listEndpoints, packageSearchArgs.Logger);

            if (listEndpoints == null || listEndpoints.Count == 0)
            {
                packageSearchResultRenderer.Add(new PackageSearchProblem(PackageSearchProblemType.Error, Strings.Error_NoSource));
                packageSearchResultRenderer.Finish();

                return ExitCodes.Error;
            }

            Func<PackageSource, Task<IEnumerable<IPackageSearchMetadata>>> searchPackageSourceAsync =
                packageSearchArgs.ExactMatch
                ? packageSource => GetPackageAsync(packageSource, packageSearchArgs, cancellationToken)
                : packageSource => SearchAsync(packageSource, packageSearchArgs, cancellationToken);

            Dictionary<Task<IEnumerable<IPackageSearchMetadata>>, PackageSource> searchRequests = new();

            foreach (var packageSource in listEndpoints)
            {
                Task<IEnumerable<IPackageSearchMetadata>> searchTask = searchPackageSourceAsync(packageSource);
                searchRequests.Add(searchTask, packageSource);
            }


            while (searchRequests.Count > 0)
            {
                Task<IEnumerable<IPackageSearchMetadata>> completedTask = await Task.WhenAny(searchRequests.Keys);
                PackageSource source = searchRequests[completedTask];

                IEnumerable<IPackageSearchMetadata> searchResult = null;

                try
                {
                    searchResult = await completedTask;
                }
                catch (FatalProtocolException ex)
                {
                    // search
                    // Throws FatalProtocolException for JSON parsing errors as fatal metadata issues.
                    // Throws FatalProtocolException for HTTP request issues indicating critical source(v2/v3) problems.
                    packageSearchResultRenderer.Add(source, new PackageSearchProblem(PackageSearchProblemType.Error, ex.Message));
                    searchRequests.Remove(completedTask);
                    continue;
                }
                catch (OperationCanceledException ex)
                {
                    packageSearchResultRenderer.Add(source, new PackageSearchProblem(PackageSearchProblemType.Error, ex.Message));
                    searchRequests.Remove(completedTask);
                    continue;
                }
                catch (InvalidOperationException ex)
                {
                    // Thrown for a local package with an invalid source destination.
                    packageSearchResultRenderer.Add(source, new PackageSearchProblem(PackageSearchProblemType.Error, ex.Message));
                    searchRequests.Remove(completedTask);
                    continue;
                }

                if (searchResult == null)
                {
                    packageSearchResultRenderer.Add(source, new PackageSearchProblem(PackageSearchProblemType.Warning, Strings.Error_CannotObtainSearchSource));
                }
                else
                {
                    packageSearchResultRenderer.Add(source, searchResult);
                }

                searchRequests.Remove(completedTask);
            }

            packageSearchResultRenderer.Finish();
            return ExitCodes.Success;
        }

        /// <summary>
        /// Builds a task that performs package search operation.
        /// </summary>
        /// <param name="source">A package source/endpoint.</param>
        /// <param name="packageSearchArgs">Package search arguments</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A list of tasks that perform package search operations.</returns>
        private static Task<IEnumerable<IPackageSearchMetadata>> SearchAsync(
            PackageSource source,
            PackageSearchArgs packageSearchArgs,
            CancellationToken cancellationToken)
        {
            return Task.Run(async () =>
            {
                var repository = Repository.Factory.GetCoreV3(source);
                var resource = await repository.GetResourceAsync<PackageSearchResource>(cancellationToken);

                if (resource == null)
                {
                    return null;
                }

                return await resource.SearchAsync(
                    packageSearchArgs.SearchTerm,
                    new SearchFilter(includePrerelease: packageSearchArgs.Prerelease),
                    packageSearchArgs.Skip,
                    packageSearchArgs.Take,
                    NullLogger.Instance,
                    cancellationToken);
            });
        }

        /// <summary>
        /// Builds a task that perform exact match package search operation.
        /// </summary>
        /// <param name="source">A package source.</param>
        /// <param name="packageSearchArgs">Package search arguments</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A list of tasks that perform exact match package search operations.</returns>
        private static Task<IEnumerable<IPackageSearchMetadata>> GetPackageAsync(
            PackageSource source,
            PackageSearchArgs packageSearchArgs,
            CancellationToken cancellationToken)
        {
            return Task.Run(async () =>
            {
                using var cache = new SourceCacheContext();
                var repository = Repository.Factory.GetCoreV3(source);
                var resource = await repository.GetResourceAsync<PackageMetadataResource>(cancellationToken);

                if (resource == null)
                {
                    return null;
                }

                return await resource.GetMetadataAsync(
                    packageSearchArgs.SearchTerm,
                    includePrerelease: packageSearchArgs.Prerelease,
                    includeUnlisted: false,
                    cache,
                    NullLogger.Instance,
                    cancellationToken);
            });
        }

        /// <summary>
        /// Retrieves a list of package sources based on provided sources or default configuration sources.
        /// </summary>
        /// <param name="sources">The list of package sources provided.</param>
        /// <param name="sourceProvider">The provider for package sources.</param>
        /// <returns>A list of package sources.</returns>
        private static IList<PackageSource> GetPackageSources(List<string> sources, IPackageSourceProvider sourceProvider)
        {
            IEnumerable<PackageSource> configurationSources = sourceProvider.LoadPackageSources()
                .Where(p => p.IsEnabled);
            IEnumerable<PackageSource> packageSources;

            if (sources.Count > 0)
            {
                packageSources = sources
                    .Select(s => PackageSourceProviderExtensions.ResolveSource(configurationSources, s));
            }
            else
            {
                packageSources = configurationSources;
            }

            return packageSources.ToList();
        }

        /// <summary>
        /// Warns the user if the provided package sources use insecure HTTP connections.
        /// </summary>
        /// <param name="packageSources">The list of package sources to check.</param>
        /// <param name="logger">The logger instance to use for logging.</param>
        private static void WarnForHTTPSources(IList<PackageSource> packageSources, ILogger logger)
        {
            List<PackageSource> httpPackageSources = null;

            foreach (PackageSource packageSource in packageSources)
            {
                if (packageSource.IsHttp && !packageSource.IsHttps && !packageSource.AllowInsecureConnections)
                {
                    if (httpPackageSources == null)
                    {
                        httpPackageSources = new(capacity: packageSources.Count);
                    }
                    httpPackageSources.Add(packageSource);
                }
            }

            if (httpPackageSources != null && httpPackageSources.Count != 0)
            {
                if (httpPackageSources.Count == 1)
                {
                    logger.LogWarning(
                        string.Format(
                            CultureInfo.CurrentCulture,
                            Strings.Warning_HttpServerUsage,
                            "search",
                            httpPackageSources[0]));
                }
                else
                {
                    logger.LogWarning(
                        string.Format(
                            CultureInfo.CurrentCulture,
                            Strings.Warning_HttpServerUsage_MultipleSources,
                            "search",
                            Environment.NewLine + string.Join(Environment.NewLine, httpPackageSources.Select(e => e.Name))));
                }
            }
        }
    }
}
