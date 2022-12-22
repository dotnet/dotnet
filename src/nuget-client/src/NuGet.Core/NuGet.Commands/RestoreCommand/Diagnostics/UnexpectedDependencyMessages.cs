// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.LibraryModel;
using NuGet.ProjectModel;
using NuGet.Shared;
using NuGet.Versioning;

namespace NuGet.Commands
{
    /// <summary>
    /// Log warnings for packages that did not resolve to the minimum version of the dependency range.
    /// </summary>
    public static class UnexpectedDependencyMessages
    {
        /// <summary>
        /// Log warnings for all project issues related to unexpected dependencies.
        /// </summary>
        public static async Task LogAsync(IEnumerable<IRestoreTargetGraph> graphs, PackageSpec project, ILogger logger)
        {
            var ignoreIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var graphList = graphs.AsList();

            // Index the flattened graph for faster lookups.
            var indexedGraphs = graphList.Select(IndexedRestoreTargetGraph.Create).ToList();

            // 1. Detect project dependency authoring issues in the current project.
            //    The user can fix these themselves.
            var projectMissingVersions = GetProjectDependenciesMissingVersion(project);
            ignoreIds.UnionWith(projectMissingVersions.Select(e => e.LibraryId));
            await logger.LogMessagesAsync(DiagnosticUtility.MergeOnTargetGraph(projectMissingVersions));

            var projectMissingLowerBounds = GetProjectDependenciesMissingLowerBounds(project);
            ignoreIds.UnionWith(projectMissingLowerBounds.Select(e => e.LibraryId));
            await logger.LogMessagesAsync(DiagnosticUtility.MergeOnTargetGraph(projectMissingLowerBounds));

            // Ignore generating NU1603/NU1602 across entire graph if lock file is enabled. Because
            // lock file enforce a fixed resolved version for all the different requests for the same package ID.
            if (!PackagesLockFileUtilities.IsNuGetLockFileEnabled(project))
            {
                // 2. Detect dependency and source issues across the entire graph 
                //    where the minimum version was not matched exactly.
                //    Ignore packages already logged by #1
                var missingMinimums = GetMissingLowerBounds(graphList, ignoreIds);
                ignoreIds.UnionWith(missingMinimums.Select(e => e.LibraryId));
                await logger.LogMessagesAsync(DiagnosticUtility.MergeOnTargetGraph(missingMinimums));
            }

            // 3. Detect top level dependencies that have a version different from the specified version.
            //    Ignore packages already logged in #1 and #2 since those errors are more specific.
            var bumpedUp = GetBumpedUpDependencies(indexedGraphs, project, ignoreIds);
            await logger.LogMessagesAsync(DiagnosticUtility.MergeOnTargetGraph(bumpedUp));

            // 4. Detect dependencies that are higher than the upper bound of a version range.
            var aboveUpperBounds = GetDependenciesAboveUpperBounds(indexedGraphs, logger);
            await logger.LogMessagesAsync(aboveUpperBounds);
        }

        /// <summary>
        /// Get warnings for packages that have dependencies on non-existant versions of packages
        /// and also for packages with ranges that have missing minimum versions.
        /// </summary>
        public static IEnumerable<RestoreLogMessage> GetMissingLowerBounds(IEnumerable<IRestoreTargetGraph> graphs, ISet<string> ignoreIds)
        {
            var messages = new List<RestoreLogMessage>();

            foreach (var graph in graphs)
            {
                messages.AddRange(graph.ResolvedDependencies
                                            .Distinct()
                                            .Where(e => !ignoreIds.Contains(e.Child.Name, StringComparer.OrdinalIgnoreCase)
                                                        && DependencyRangeHasMissingExactMatch(e))
                                            .OrderBy(e => e.Child.Name, StringComparer.OrdinalIgnoreCase)
                                            .ThenBy(e => e.Child.Version)
                                            .ThenBy(e => e.Parent.Name, StringComparer.OrdinalIgnoreCase)
                                            .Select(e => GetMissingLowerBoundMessage(e, graph.TargetGraphName)));
            }

            return messages;
        }

        /// <summary>
        /// Get warning message for missing minimum dependencies.
        /// </summary>
        public static RestoreLogMessage GetMissingLowerBoundMessage(ResolvedDependencyKey dependency, params string[] targetGraphs)
        {
            NuGetLogCode code;
            var message = string.Empty;
            var parent = DiagnosticUtility.FormatIdentity(dependency.Parent);
            var dependencyRange = DiagnosticUtility.FormatDependency(dependency.Child.Name, dependency.Range);
            var missingChild = DiagnosticUtility.FormatExpectedIdentity(dependency.Child.Name, dependency.Range);
            var resolvedChild = DiagnosticUtility.FormatIdentity(dependency.Child);

            if (HasMissingLowerBound(dependency.Range))
            {
                // Range does not have a lower bound, the best match can only be approximate.
                message = string.Format(CultureInfo.CurrentCulture, Strings.Warning_MinVersionNonInclusive,
                    parent,
                    dependencyRange,
                    resolvedChild);

                code = NuGetLogCode.NU1602;
            }
            else
            {
                // The minimum version does not exist.
                message = string.Format(CultureInfo.CurrentCulture, Strings.Warning_MinVersionDoesNotExist,
                    parent,
                    dependencyRange,
                    missingChild,
                    resolvedChild);

                code = NuGetLogCode.NU1603;
            }

            return RestoreLogMessage.CreateWarning(code, message, dependency.Child.Name, targetGraphs);
        }

        /// <summary>
        /// Warn for dependencies that have been bumped up.
        /// </summary>
        public static IEnumerable<RestoreLogMessage> GetBumpedUpDependencies(
            List<IndexedRestoreTargetGraph> graphs,
            PackageSpec project,
            ISet<string> ignoreIds)
        {
            var messages = new List<RestoreLogMessage>();

            // Group by framework to get project dependencies, then check each graph.
            foreach (var frameworkGroup in graphs.GroupBy(e => e.Graph.Framework))
            {
                // Get dependencies from the project
                var dependencies = project.GetPackageDependenciesForFramework(frameworkGroup.Key)
                                              .Where(e => !ignoreIds.Contains(e.Name, StringComparer.OrdinalIgnoreCase))
                                              .Where(IsNonFloatingPackageDependency);

                foreach (var dependency in dependencies)
                {
                    // Graphs may have different versions of the resolved package
                    foreach (var indexedGraph in frameworkGroup)
                    {
                        var minVersion = dependency.LibraryRange.VersionRange?.MinVersion;
                        if (minVersion != null && dependency.LibraryRange.VersionRange.IsMinInclusive)
                        {
                            // Ignore floating or version-less (project) dependencies
                            // Avoid warnings for non-packages
                            var match = indexedGraph.GetItemById(dependency.Name, LibraryType.Package);

                            if (match != null && match.Key.Version > minVersion)
                            {
                                var message = string.Format(CultureInfo.CurrentCulture, Strings.Log_DependencyBumpedUp,
                                    dependency.LibraryRange.Name,
                                    dependency.LibraryRange.VersionRange.PrettyPrint(),
                                    match.Key.Name,
                                    match.Key.Version);

                                var graphName = indexedGraph.Graph.TargetGraphName;

                                messages.Add(RestoreLogMessage.CreateWarning(NuGetLogCode.NU1601, message, match.Key.Name, graphName));
                            }
                        }
                    }
                }
            }

            return messages;
        }

        /// <summary>
        /// Warn for project dependencies that do not have a version.
        /// </summary>
        internal static IEnumerable<RestoreLogMessage> GetProjectDependenciesMissingVersion(PackageSpec project)
        {
            return project.GetAllPackageDependencies()
                    .Where(e => e.LibraryRange.VersionRange == null)
                    .OrderBy(e => e.Name, StringComparer.OrdinalIgnoreCase)
                    .Select(e => RestoreLogMessage.CreateWarning(
                       code: NuGetLogCode.NU1604,
                       message: string.Format(CultureInfo.CurrentCulture, Strings.Warning_ProjectDependencyMissingVersion,
                                              DiagnosticUtility.FormatDependency(e.Name, e.LibraryRange.VersionRange)),
                       libraryId: e.Name,
                       targetGraphs: GetDependencyTargetGraphs(project, e)));
        }

        /// <summary>
        /// Warn for project dependencies that do not include a lower bound on the version range.
        /// </summary>
        public static IEnumerable<RestoreLogMessage> GetProjectDependenciesMissingLowerBounds(PackageSpec project)
        {
            return project.GetAllPackageDependencies()
                   .Where(e => e.LibraryRange.VersionRange != null && HasMissingLowerBound(e.LibraryRange.VersionRange))
                   .OrderBy(e => e.Name, StringComparer.OrdinalIgnoreCase)
                   .Select(e => RestoreLogMessage.CreateWarning(
                       code: NuGetLogCode.NU1604,
                       message: string.Format(CultureInfo.CurrentCulture, Strings.Warning_ProjectDependencyMissingLowerBound,
                                              DiagnosticUtility.FormatDependency(e.Name, e.LibraryRange.VersionRange)),
                       libraryId: e.Name,
                       targetGraphs: GetDependencyTargetGraphs(project, e)));
        }

        /// <summary>
        /// True if the dependency version range has a min version that matches the resolved version.
        /// </summary>
        public static bool DependencyRangeHasMissingExactMatch(ResolvedDependencyKey dependency)
        {
            // Ignore floating
            if (dependency.Range.IsFloating)
            {
                return false;
            }

            // Ignore projects
            if (dependency.Child.Type != LibraryType.Package)
            {
                return false;
            }

            return (!dependency.Range.IsMinInclusive || dependency.Range.MinVersion != dependency.Child.Version);
        }

        /// <summary>
        /// True if the range has an obtainable version for the lower bound.
        /// </summary>
        public static bool HasMissingLowerBound(VersionRange range)
        {
            // Ignore floating
            if (range.IsFloating)
            {
                return false;
            }

            return !range.IsMinInclusive || !range.HasLowerBound;
        }

        /// <summary>
        /// Log upgrade warnings from the graphs.
        /// </summary>
        public static IEnumerable<RestoreLogMessage> GetDependenciesAboveUpperBounds(List<IndexedRestoreTargetGraph> graphs, ILogger logger)
        {
            var messages = new List<RestoreLogMessage>();

            foreach (var indexedGraph in graphs)
            {
                var graph = indexedGraph.Graph;

                foreach (var node in graph.Flattened)
                {
                    List<LibraryDependency> dependencies = node.Data?.Dependencies;
                    if (dependencies == null)
                    {
                        continue;
                    }

                    foreach (var dependency in dependencies)
                    {
                        // Check if the dependency has an upper bound
                        var dependencyRange = dependency.LibraryRange.VersionRange;
                        var upperBound = dependencyRange?.MaxVersion;
                        if (upperBound != null)
                        {
                            var dependencyId = dependency.Name;

                            // If the version does not exist then it was not resolved or is a project and should be skipped.
                            var match = indexedGraph.GetItemById(dependencyId, LibraryType.Package);
                            if (match != null)
                            {
                                var actualVersion = match.Key.Version;

                                // If the upper bound is included then require that the version be higher than the upper bound to fail
                                // If the upper bound is not included, then an exact match on the upperbound is a failure
                                var compare = dependencyRange.IsMaxInclusive ? 1 : 0;

                                if (VersionComparer.VersionRelease.Compare(actualVersion, upperBound) >= compare)
                                {
                                    // True if the package already has an NU1107 error, NU1608 would be redundant here.
                                    if (!indexedGraph.HasErrors(dependencyId))
                                    {
                                        var parent = DiagnosticUtility.FormatIdentity(node.Key);
                                        var child = DiagnosticUtility.FormatDependency(dependencyId, dependencyRange);
                                        var actual = DiagnosticUtility.FormatIdentity(match.Key);

                                        var message = string.Format(CultureInfo.CurrentCulture,
                                            Strings.Warning_VersionAboveUpperBound,
                                            parent,
                                            child,
                                            actual);

                                        messages.Add(RestoreLogMessage.CreateWarning(NuGetLogCode.NU1608, message, dependencyId, graph.TargetGraphName));
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // Merge log messages
            return DiagnosticUtility.MergeOnTargetGraph(messages);
        }

        private static bool IsNonFloatingPackageDependency(this LibraryDependency dependency)
        {
            return (dependency.LibraryRange.TypeConstraintAllows(LibraryDependencyTarget.Package)
                && dependency.LibraryRange.VersionRange != null && !dependency.LibraryRange.VersionRange.IsFloating);
        }

        /// <summary>
        /// Create target graph names for each framework the dependency exists under.
        /// </summary>
        private static string[] GetDependencyTargetGraphs(PackageSpec spec, LibraryDependency dependency)
        {
            var infos = new List<TargetFrameworkInformation>();

            if (spec.Dependencies.Contains(dependency))
            {
                // If the dependency is top level add it for all tfms
                infos.AddRange(spec.TargetFrameworks);
            }
            else
            {
                // Add all tfms where the dependency is found
                infos.AddRange(spec.TargetFrameworks.Where(e => e.Dependencies.Contains(dependency)));
            }

            // Convert framework to target graph name.
            return infos.Select(e => e.FrameworkName.DotNetFrameworkName).ToArray();
        }

    }
}
