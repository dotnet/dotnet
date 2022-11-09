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
using NuGet.Configuration;
using NuGet.DependencyResolver;
using NuGet.LibraryModel;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;

namespace NuGet.Commands
{
    /// <summary>
    /// Log errors for packages and projects that were missing.
    /// </summary>
    internal static class UnresolvedMessages
    {
        /// <summary>
        /// Log errors for missing dependencies.
        /// </summary>
        internal static async Task LogAsync(IEnumerable<IRestoreTargetGraph> graphs, RemoteWalkContext context, CancellationToken token)
        {
            var tasks = graphs.SelectMany(graph => graph.Unresolved.Select(e =>
            GetMessageAsync(
                graph.TargetGraphName,
                e,
                context.FilterDependencyProvidersForLibrary(e),
                context.PackageSourceMapping.IsEnabled,
                context.RemoteLibraryProviders,
                context.CacheContext,
                context.Logger,
                token))).ToArray();

            var messages = await Task.WhenAll(tasks);

            await context.Logger.LogMessagesAsync(DiagnosticUtility.MergeOnTargetGraph(messages));
        }

        internal static async Task LogAsync(IList<DownloadDependencyResolutionResult> downloadDependencyResults, RemoteWalkContext context, CancellationToken token)
        {
            var messageTasks = new List<Task<RestoreLogMessage>>();

            foreach (var ddi in downloadDependencyResults)
            {
                foreach (var unresolved in ddi.Unresolved)
                {
                    messageTasks.Add(GetMessageAsync(
                        ddi.Framework.ToString(),
                        unresolved,
                        context.FilterDependencyProvidersForLibrary(unresolved),
                        context.PackageSourceMapping.IsEnabled,
                        context.RemoteLibraryProviders,
                        context.CacheContext,
                        context.Logger,
                        token));
                }
            }

            var messages = await Task.WhenAll(messageTasks);
            await context.Logger.LogMessagesAsync(DiagnosticUtility.MergeOnTargetGraph(messages));
        }

        /// <summary>
        /// Create a specific error message for the unresolved dependency.
        /// </summary>
        internal static async Task<RestoreLogMessage> GetMessageAsync(string targetGraphName,
            LibraryRange unresolved,
            IList<IRemoteDependencyProvider> applicableRemoteLibraryProviders,
            bool isPackageSourceMappingEnabled,
            IList<IRemoteDependencyProvider> allRemoteLibraryProviders,
            SourceCacheContext sourceCacheContext,
            ILogger logger,
            CancellationToken token)
        {
            // Default to using the generic unresolved error code, this will be overridden later.
            var code = NuGetLogCode.NU1100;
            var message = string.Empty;

            if (unresolved.TypeConstraintAllows(LibraryDependencyTarget.ExternalProject)
                && !unresolved.TypeConstraintAllows(LibraryDependencyTarget.Package))
            {
                // Project
                // Check if the name is a path and if it exists. All project paths should have been normalized and converted to full paths before this.
#if NETCOREAPP
                if (unresolved.Name.IndexOf(Path.DirectorySeparatorChar, StringComparison.Ordinal) > -1 && File.Exists(unresolved.Name))
#else
                if (unresolved.Name.IndexOf(Path.DirectorySeparatorChar) > -1 && File.Exists(unresolved.Name))
#endif
                {
                    // File exists but the dg spec did not contain the spec
                    code = NuGetLogCode.NU1105;
                    message = string.Format(CultureInfo.CurrentCulture, Strings.Error_UnableToFindProjectInfo, unresolved.Name);
                }
                else
                {
                    // Generic missing project error
                    code = NuGetLogCode.NU1104;
                    message = string.Format(CultureInfo.CurrentCulture, Strings.Error_ProjectDoesNotExist, unresolved.Name);
                }
            }
            else if (unresolved.TypeConstraintAllows(LibraryDependencyTarget.Package) && applicableRemoteLibraryProviders.Count > 0)
            {
                // Package
                var range = unresolved.VersionRange ?? VersionRange.All;
                var sourceInfo = await GetSourceInfosForIdAsync(unresolved.Name, applicableRemoteLibraryProviders, sourceCacheContext, logger, token);
                var allVersions = new SortedSet<NuGetVersion>(sourceInfo.SelectMany(e => e.Value));

                if (allVersions.Count == 0)
                {
                    // No versions found
                    code = NuGetLogCode.NU1101;
                    var sourceList = string.Join(", ",
                                        sourceInfo.Select(e => e.Key.Name)
                                                  .OrderBy(e => e, StringComparer.OrdinalIgnoreCase));

                    message = string.Format(CultureInfo.CurrentCulture, Strings.Error_NoPackageVersionsExist, unresolved.Name, sourceList);

                    if (isPackageSourceMappingEnabled && applicableRemoteLibraryProviders.Count != allRemoteLibraryProviders.Count)
                    {
                        string sourcesNotConsidered = FormatProviderNames(GetUnusedLibraryProviders(applicableRemoteLibraryProviders, allRemoteLibraryProviders));

                        message += ". " + string.Format(CultureInfo.CurrentCulture,
                            Strings.Log_SourceMappingEnabledNoMatchingPackageSources,
                            sourcesNotConsidered);
                    }
                }
                else
                {
                    // At least one version found
                    var firstLine = string.Empty;
                    var rangeString = range.ToNonSnapshotRange().PrettyPrint();

                    if (!IsPrereleaseAllowed(range) && HasPrereleaseVersionsOnly(range, allVersions))
                    {
                        code = NuGetLogCode.NU1103;
                        firstLine = string.Format(CultureInfo.CurrentCulture, Strings.Error_NoStablePackageVersionsExist, unresolved.Name, rangeString);
                    }
                    else
                    {
                        code = NuGetLogCode.NU1102;
                        firstLine = string.Format(CultureInfo.CurrentCulture, Strings.Error_NoPackageVersionsExistInRange, unresolved.Name, rangeString);
                    }

                    var lines = new List<string>()
                    {
                        firstLine
                    };

                    lines.AddRange(sourceInfo.Select(e => FormatSourceInfo(e, range)));

                    if (isPackageSourceMappingEnabled && allRemoteLibraryProviders.Count != applicableRemoteLibraryProviders.Count)
                    {
                        lines.AddRange(GetUnusedLibraryProviders(applicableRemoteLibraryProviders, allRemoteLibraryProviders)
                            .OrderBy(e => e.Source.Name)
                            .Select(packageSource => string.Format(CultureInfo.CurrentCulture,
                                            Strings.SourceNotConsidered,
                                            packageSource.Source.Name)));
                    }

                    message = DiagnosticUtility.GetMultiLineMessage(lines);
                }
            }
            else
            {
                // Unknown or non-specific.
                // Also shown when no sources exist.
                message = string.Format(CultureInfo.CurrentCulture,
                    Strings.Log_UnresolvedDependency,
                    unresolved.ToString(),
                    targetGraphName);

                if (isPackageSourceMappingEnabled && applicableRemoteLibraryProviders.Count != allRemoteLibraryProviders.Count)
                {
                    message += " " + string.Format(CultureInfo.CurrentCulture,
                            Strings.Log_SourceMappingEnabledNoMatchingPackageSources,
                            FormatProviderNames(allRemoteLibraryProviders));
                }

                // Set again for clarity
                code = NuGetLogCode.NU1100;
            }

            return RestoreLogMessage.CreateError(code, message, unresolved.Name, targetGraphName);
        }

        private static IEnumerable<IRemoteDependencyProvider> GetUnusedLibraryProviders(IList<IRemoteDependencyProvider> applicableRemoteLibraryProviders, IList<IRemoteDependencyProvider> allRemoteLibraryProviders)
        {
            return allRemoteLibraryProviders
                    .Where(e => !applicableRemoteLibraryProviders.Contains(e))
                    .Select(e => e);
        }

        private static string FormatProviderNames(IEnumerable<IRemoteDependencyProvider> allRemoteLibraryProviders)
        {
            return string.Join(", ", allRemoteLibraryProviders.Select(e => e.Source.Name)
                    .OrderBy(e => e, StringComparer.OrdinalIgnoreCase));
        }

        /// <summary>
        /// True if no stable versions satisfy the range 
        /// but a pre-release version is found.
        /// </summary>
        internal static bool HasPrereleaseVersionsOnly(VersionRange range, IEnumerable<NuGetVersion> versions)
        {
            var currentRange = range ?? VersionRange.All;
            var currentVersions = versions ?? Enumerable.Empty<NuGetVersion>();

            return (versions.Any(e => e.IsPrerelease && currentRange.Satisfies(e))
                && !versions.Any(e => !e.IsPrerelease && currentRange.Satisfies(e)));
        }

        /// <summary>
        /// True if the range allows pre-release versions.
        /// </summary>
        internal static bool IsPrereleaseAllowed(VersionRange range)
        {
            return (range?.MaxVersion?.IsPrerelease == true
                || range?.MinVersion?.IsPrerelease == true);
        }

        /// <summary>
        /// Found 2839 version(s) in nuget-build [ Nearest version: 1.0.0-beta ]
        /// </summary>
        internal static string FormatSourceInfo(KeyValuePair<PackageSource, SortedSet<NuGetVersion>> sourceInfo, VersionRange range)
        {
            var bestMatch = GetBestMatch(sourceInfo.Value, range);

            if (bestMatch != null)
            {
                return string.Format(CultureInfo.CurrentCulture,
                    Strings.FoundVersionsInSource,
                    sourceInfo.Value.Count,
                    sourceInfo.Key.Name,
                    bestMatch.ToNormalizedString());
            }

            return string.Format(CultureInfo.CurrentCulture,
                                Strings.FoundVersionsInSourceWithoutMatch,
                                sourceInfo.Value.Count,
                                sourceInfo.Key.Name);
        }

        /// <summary>
        /// Get the complete set of source info for a package id.
        /// </summary>
        internal static async Task<List<KeyValuePair<PackageSource, SortedSet<NuGetVersion>>>> GetSourceInfosForIdAsync(
            string id,
            IList<IRemoteDependencyProvider> remoteLibraryProviders,
            SourceCacheContext sourceCacheContext,
            ILogger logger,
            CancellationToken token)
        {
            var sources = new List<KeyValuePair<PackageSource, SortedSet<NuGetVersion>>>();

            // Get versions from all sources. These should be cached by the providers already.
            var tasks = remoteLibraryProviders
                .Select(e => GetSourceInfoForIdAsync(e, id, sourceCacheContext, logger, token))
                .ToArray();

            foreach (var task in tasks)
            {
                sources.Add(await task);
            }

            // Sort by most package versions, then by source path.
            return sources.OrderByDescending(e => e.Value.Count)
                .ThenBy(e => e.Key.Source, StringComparer.OrdinalIgnoreCase)
                .ToList();
        }

        /// <summary>
        /// Find all package versions from a source.
        /// </summary>
        internal static async Task<KeyValuePair<PackageSource, SortedSet<NuGetVersion>>> GetSourceInfoForIdAsync(
            IRemoteDependencyProvider provider,
            string id,
            SourceCacheContext cacheContext,
            ILogger logger,
            CancellationToken token)
        {
            // Find all versions from a source.
            var versions = await provider.GetAllVersionsAsync(id, cacheContext, logger, token) ?? Enumerable.Empty<NuGetVersion>();

            return new KeyValuePair<PackageSource, SortedSet<NuGetVersion>>(
                provider.Source,
                new SortedSet<NuGetVersion>(versions));
        }

        /// <summary>
        /// Find the best match on the feed.
        /// </summary>
        internal static NuGetVersion GetBestMatch(SortedSet<NuGetVersion> versions, VersionRange range)
        {
            if (versions.Count == 0)
            {
                return null;
            }

            // Find a pivot point
            var ideal = new NuGetVersion(0, 0, 0);
            NuGetVersion bestMatch = null;
            if (range != null)
            {
                if (range.HasUpperBound)
                {
                    ideal = range.MaxVersion;
                }

                if (range.HasLowerBound)
                {
                    ideal = range.MinVersion;
                }
            }
            //|      Range     |          Available         | Closest  |
            //| [1.0.0, )      | 0.7.0, 0.9.0               | 0.7.0    |
            //| (0.5.0, 1.0.0) | 0.1.0, 1.0.0               | 1.0.0    |
            //| (, 1.0.0)      | 2.0.0, 3.0.0               | 2.0.0    |
            //| [1.*,)         | 0.0.1, 0.0.5, 0.1.0, 0.9.0 | 0.9.0    |
            //| [1.*, 2.0.0]   | 0.1.0, 0.3.0, 3.0.0, 4.0.0 | 3.0.0    |
            //| *              | 0.0.1-alpha, 2.1.0-p1      | 2.1.0-p1 |
            //Floatless ranges - If there's a pivot/lower or higher bound, take the first above that pivot.
            bool floatlessRangeHasBounds = !range.IsFloating && (range.HasLowerBound || range.HasUpperBound);
            //Floating ranges - by definition they need the latest version, so unless there's an upper bound always show the highest version possible.
            bool floatingRangeHasUpperBound = range.IsFloating && range.HasUpperBound;

            if (floatlessRangeHasBounds || floatingRangeHasUpperBound)
            {
                bestMatch = versions.Where(e => e >= ideal).FirstOrDefault();
            }

            if (bestMatch == null)
            {
                // Take the highest possible version.
                bestMatch = versions.Last();
            }

            return bestMatch;
        }
    }
}
