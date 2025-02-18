// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.DependencyResolver;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.Packaging;
using NuGet.ProjectModel;
using NuGet.Repositories;
using NuGet.RuntimeModel;
using NuGet.Versioning;

namespace NuGet.Commands
{
    /// <summary>
    /// Represents a class that can resolve a dependency graph.
    /// </summary>
    internal sealed partial class DependencyGraphResolver
    {
        /// <summary>
        /// Defines the default size for the queue used to process dependency graph items.
        /// </summary>
        private const int DependencyGraphItemQueueSize = 4096;

        /// <summary>
        /// Defines the default size for the evictions queue used to process evictions.
        /// </summary>
        private const int EvictionsDictionarySize = 1024;

        /// <summary>
        /// Defines the default size for the dictionary that stores the resolved dependency graph items.
        /// </summary>
        private const int ResolvedDependencyGraphItemDictionarySize = 2048;

        /// <summary>
        /// A <see cref="DependencyGraphItemIndexer" /> used to index dependency graph items.
        /// </summary>
        private readonly DependencyGraphItemIndexer _indexingTable;

        /// <summary>
        /// A <see cref="RestoreCollectorLogger" /> used for logging.
        /// </summary>
        private readonly RestoreCollectorLogger _logger;

        /// <summary>
        /// The <see cref="RestoreRequest" /> of the current restore that provides information about the project's configuration.
        /// </summary>
        private readonly RestoreRequest _request;

        /// <summary>
        /// Represents the path for any dependency that is directly referenced by the root project.
        /// </summary>
        private readonly LibraryRangeIndex[] _rootedDependencyPath = new[] { LibraryRangeIndex.Project };

        /// <summary>
        /// A <see cref="TelemetryActivity" /> instance used for telemetry.
        /// </summary>
        private readonly TelemetryActivity _telemetryActivity;

        /// <summary>
        /// Represents the root project as a <see cref="LibraryDependency" />.
        /// </summary>
        private readonly LibraryDependency _rootProjectLibraryDependency;

        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyGraphResolver" /> class.
        /// </summary>
        /// <param name="logger">A <see cref="RestoreCollectorLogger" /> to use for logging.</param>
        /// <param name="restoreRequest">The <see cref="RestoreRequest" /> for the restore.</param>
        /// <param name="telemetryActivity">A <see cref="TelemetryActivity" /> instance to use for logging telemetry.</param>
        public DependencyGraphResolver(RestoreCollectorLogger logger, RestoreRequest restoreRequest, TelemetryActivity telemetryActivity)
        {
            _logger = logger;
            _request = restoreRequest;
            _telemetryActivity = telemetryActivity;

            _rootProjectLibraryDependency = new LibraryDependency(
                new LibraryRange()
                {
                    Name = _request.Project.Name,
                    VersionRange = new VersionRange(_request.Project.Version),
                    TypeConstraint = LibraryDependencyTarget.Project | LibraryDependencyTarget.ExternalProject
                });

            _indexingTable = new DependencyGraphItemIndexer(_rootProjectLibraryDependency);
        }

        /// <summary>
        /// Resolves the dependency graph for the project for all of its configured target frameworks and runtime identifiers.
        /// </summary>
        /// <param name="userPackageFolder">A <see cref="NuGetv3LocalRepository" /> representing the global packages folder configured for restore.</param>
        /// <param name="fallbackPackageFolders">A <see cref="IReadOnlyList{T}" /> of <see cref="NuGetv3LocalRepository" /> objects that represent the package fallback folders configured for restore.</param>
        /// <param name="context">The <see cref="RemoteWalkContext" /> to use for this restore.</param>
        /// <param name="projectRestoreCommand">A <see cref="ProjectRestoreCommand" /> instance to use for installing packages.</param>
        /// <param name="localRepositories">A <see cref="List{T}" /> of <see cref="NuGetv3LocalRepository" /> objects that represent the local package repositories configured for restore.</param>
        /// <param name="token">A <see cref="CancellationToken" /> to use for cancellation.</param>
        /// <returns>A <see cref="ValueTuple{T1, T2, T3}" /> with:<br />
        ///   <list type="bullet">
        ///     <item>A <see langword="bool" /> representing the overall success of the dependency graph resolution.</item>
        ///     <item>A <see cref="List{T}" /> of <see cref="RestoreTargetGraph" /> objects representing the resolved dependency graphs for each pair of target framework and runtime identifier.</item>
        ///     <item>A <see cref="RuntimeGraph" /> representing the runtime graph of the resolved dependency graph.</item>
        ///   </list>
        /// </returns>
        public async Task<ValueTuple<bool, List<RestoreTargetGraph>, RuntimeGraph>> ResolveAsync(
            NuGetv3LocalRepository userPackageFolder,
            IReadOnlyList<NuGetv3LocalRepository> fallbackPackageFolders,
            RemoteWalkContext context,
            ProjectRestoreCommand projectRestoreCommand,
            List<NuGetv3LocalRepository> localRepositories,
            CancellationToken token)
        {
            _telemetryActivity.StartIntervalMeasure();

            // Keeps track of the overall success of the dependency graph resolution
            bool success = true;

            // Calculate whether or not transitive pinning is enabled
            bool isCentralPackageTransitivePinningEnabled = _request.Project.RestoreMetadata != null && _request.Project.RestoreMetadata.CentralPackageVersionsEnabled & _request.Project.RestoreMetadata.CentralPackageTransitivePinningEnabled;

            // Keeps track of all of the packages that were installed
            HashSet<LibraryIdentity> installedPackages = new();

            // Stores the list of all graphs
            List<RestoreTargetGraph> allGraphs = new();

            // Stores the list of graphs without a runtime identifier by their target framework
            Dictionary<NuGetFramework, RestoreTargetGraph> graphsByTargetFramework = new();

            // Stores the list graphs that are runtime identifier specific
            List<RestoreTargetGraph> runtimeGraphs = new();

            // Keeps track of all runtime graphs that exist in the dependency graph
            RuntimeGraph allRuntimes = RuntimeGraph.Empty;

            // Keeps track of whether or not we've installed all of the RID-less packages since they contain a runtime.json and are needed before the RID-specific graphs can be resolved
            bool hasInstallBeenCalledAlready = false;

            // Stores the list of download results which contribute to the overall success of the graph resolution
            DownloadDependencyResolutionResult[]? downloadDependencyResolutionResults = default;

            // Create the list of framework/runtime pairs to resolve graphs for.  This method returns the pairs in order with the target framework pairs without runtime identifiers come first
            List<FrameworkRuntimePair> projectFrameworkRuntimePairs = RestoreCommand.CreateFrameworkRuntimePairs(_request.Project, runtimeIds: RequestRuntimeUtility.GetRestoreRuntimes(_request));

            // Loop through the target framework and runtime identifier pairs to resolve each graph.
            // The pairs are sorted with all of the RID-less framework pairs first
            foreach (FrameworkRuntimePair frameworkRuntimePair in projectFrameworkRuntimePairs.NoAllocEnumerate())
            {
                // Since the FrameworkRuntimePair objects are sorted, the packages found so far need to be installed before resolving any runtime identifier specific graphs because
                // the runtime.json is in the package which is used to determine what runtime packages to add
                if (!string.IsNullOrWhiteSpace(frameworkRuntimePair.RuntimeIdentifier) && !hasInstallBeenCalledAlready)
                {
                    downloadDependencyResolutionResults = await ProjectRestoreCommand.DownloadDependenciesAsync(_request.Project, context, _telemetryActivity, telemetryPrefix: string.Empty, token);

                    success &= await projectRestoreCommand.InstallPackagesAsync(installedPackages, allGraphs, downloadDependencyResolutionResults, userPackageFolder, token);

                    // This ensures that the packages for the RID-less graph are only installed once
                    hasInstallBeenCalledAlready = true;
                }

                // Get the corresponding TargetFrameworkInformation from the restore request
                TargetFrameworkInformation projectTargetFramework = _request.Project.GetTargetFramework(frameworkRuntimePair.Framework)!;

                // Keeps track of the unresolved packages
                HashSet<LibraryRange> unresolvedPackages = new();

                // Keeps track of the resolved packages
                HashSet<ResolvedDependencyKey> resolvedPackages = new();

                if (TryGetRuntimeGraph(localRepositories, graphsByTargetFramework, frameworkRuntimePair, projectTargetFramework, out RuntimeGraph? runtimeGraph))
                {
                    // Merge all of the runtime graphs together
                    allRuntimes = RuntimeGraph.Merge(allRuntimes, runtimeGraph);
                }

                // Stores a dictionary of central package versions by their LibraryDependencyIndex for faster lookup when using CPM
                Dictionary<LibraryDependencyIndex, VersionRange>? pinnedPackageVersions = IndexPinnedPackageVersions(isCentralPackageTransitivePinningEnabled, projectTargetFramework);

                // Create a DependencyGraphItem representing the root project to add to the queue for processing
                DependencyGraphItem rootProjectDependencyGraphItem = new()
                {
                    LibraryDependency = _rootProjectLibraryDependency,
                    LibraryDependencyIndex = LibraryDependencyIndex.Project,
                    LibraryRangeIndex = LibraryRangeIndex.Project,
                    Suppressions = new HashSet<LibraryDependencyIndex>(),
                    FindLibraryTask = ResolverUtility.FindLibraryCachedAsync(
                        _rootProjectLibraryDependency.LibraryRange,
                        frameworkRuntimePair.Framework,
                        runtimeIdentifier: string.IsNullOrWhiteSpace(frameworkRuntimePair.RuntimeIdentifier) ? null : frameworkRuntimePair.RuntimeIdentifier,
                        context,
                        token),
                    Parent = LibraryRangeIndex.None
                };

                // Resolve the entire dependency graph
                Dictionary<LibraryDependencyIndex, ResolvedDependencyGraphItem> resolvedDependencyGraphItems = await ResolveDependencyGraphItemsAsync(
                    isCentralPackageTransitivePinningEnabled,
                    frameworkRuntimePair,
                    projectTargetFramework,
                    runtimeGraph,
                    pinnedPackageVersions,
                    rootProjectDependencyGraphItem,
                    context,
                    token);

                // Now that the graph has been resolved, we need to create walk all of the defined dependencies again to detect any cycles and downgrades.  The RestoreTargetGraph stores all of the
                // information about the graph including the nodes with their parent/child relationships, cycles, downgrades, and conflicts.
                (bool wasRestoreTargetGraphCreationSuccessful, RestoreTargetGraph restoreTargetGraph) = await CreateRestoreTargetGraphAsync(frameworkRuntimePair, runtimeGraph, isCentralPackageTransitivePinningEnabled, unresolvedPackages, resolvedPackages, resolvedDependencyGraphItems, context);

                success &= wasRestoreTargetGraphCreationSuccessful;

                // Track all of the RestoreTargetGraph objects
                allGraphs.Add(restoreTargetGraph);

                if (!string.IsNullOrWhiteSpace(frameworkRuntimePair.RuntimeIdentifier))
                {
                    // Track all of the runtime specific graphs
                    runtimeGraphs.Add(restoreTargetGraph);
                }
                else
                {
                    // Track all of the RID-less graphs by their target framework
                    graphsByTargetFramework.Add(frameworkRuntimePair.Framework, restoreTargetGraph);
                }
            }

            _telemetryActivity.EndIntervalMeasure(ProjectRestoreCommand.WalkFrameworkDependencyDuration);

            // Install packages if they weren't already.  If the graph has not runtimes, installing of packages won't be called until this point
            if (!hasInstallBeenCalledAlready)
            {
                downloadDependencyResolutionResults = await ProjectRestoreCommand.DownloadDependenciesAsync(_request.Project, context, _telemetryActivity, telemetryPrefix: string.Empty, token);

                success &= await projectRestoreCommand.InstallPackagesAsync(installedPackages, allGraphs, downloadDependencyResolutionResults, userPackageFolder, token);

                hasInstallBeenCalledAlready = true;
            }

            // Install runtime specific packages if applicable
            if (runtimeGraphs.Count > 0)
            {
                success &= await projectRestoreCommand.InstallPackagesAsync(installedPackages, runtimeGraphs, Array.Empty<DownloadDependencyResolutionResult>(), userPackageFolder, token);
            }

            foreach (KeyValuePair<string, CompatibilityProfile> profile in _request.Project.RuntimeGraph.Supports)
            {
                CompatibilityProfile? compatProfile;
                if (profile.Value.RestoreContexts.Any())
                {
                    // Just use the contexts from the project definition
                    compatProfile = profile.Value;
                }
                else if (!allRuntimes.Supports.TryGetValue(profile.Value.Name, out compatProfile))
                {
                    // No definition of this profile found, so just continue to the next one
                    await _logger.LogAsync(RestoreLogMessage.CreateWarning(NuGetLogCode.NU1502, string.Format(CultureInfo.CurrentCulture, Strings.Log_UnknownCompatibilityProfile, profile.Key)));

                    continue;
                }

                foreach (FrameworkRuntimePair? frameworkRuntimePair in compatProfile.RestoreContexts)
                {
                    _logger.LogDebug($" {profile.Value.Name} -> +{frameworkRuntimePair}");
                    _request.CompatibilityProfiles.Add(frameworkRuntimePair);
                }
            }

            // Log the final results like downgrades, conflicts, and cycles
            _logger.ApplyRestoreOutput(allGraphs);

            // Log information for unexpected dependencies
            await UnexpectedDependencyMessages.LogAsync(allGraphs, _request.Project, _logger);

            // Determine if the graph resolution was successful (no conflicts or unresolved packages)
            success &= await projectRestoreCommand.ResolutionSucceeded(allGraphs, downloadDependencyResolutionResults, context, token);

            return (success, allGraphs, allRuntimes);
        }

        /// <summary>
        /// Creates a <see cref="RestoreTargetGraph" /> from the resolved dependency graph items and analyzes the graph for cycles, downgrades, and conflicts.
        /// </summary>
        /// <param name="frameworkRuntimePair">The <see cref="FrameworkRuntimePair" /> of the dependency graph.</param>
        /// <param name="runtimeGraph">The <see cref="RuntimeGraph" /> of the dependency graph.</param>
        /// <param name="isCentralPackageTransitivePinningEnabled">A <see cref="bool" /> indicating whether or not central transitive pinning is enabled.</param>
        /// <param name="unresolvedPackages">A <see cref="HashSet{T}" /> containing <see cref="LibraryRange" /> objects representing packages that could not be resolved.</param>
        /// <param name="resolvedPackages">A <see cref="HashSet{T}" /> containing <see cref="ResolvedDependencyKey" /> objects representing packages that were successfully resolved.</param>
        /// <param name="context">The <see cref="RemoteWalkContext" /> for the restore.</param>
        /// <returns>A <see cref="ValueTuple{T1, T2}" /> with:<br />
        ///   <list type="bullet">
        ///     <item>A <see langword="bool" /> indicating if the dependency graph contains no issues.</item>
        ///     <item>A <see cref="RestoreTargetGraph" /> representing the fully resolved and analyzed dependency graph.</item>
        ///   </list>
        /// </returns>
        private static async Task<(bool Success, RestoreTargetGraph RestoreTargetGraph)> CreateRestoreTargetGraphAsync(
            FrameworkRuntimePair frameworkRuntimePair,
            RuntimeGraph? runtimeGraph,
            bool isCentralPackageTransitivePinningEnabled,
            HashSet<LibraryRange> unresolvedPackages,
            HashSet<ResolvedDependencyKey> resolvedPackages,
            Dictionary<LibraryDependencyIndex, ResolvedDependencyGraphItem> resolvedDependencyGraphItems,
            RemoteWalkContext context)
        {
            bool success = true;

            // Stores results of analyzing the graph including conflicts, cycles, and downgrades
            AnalyzeResult<RemoteResolveResult> analyzeResult = new();

            // Stores the list of items in as a flat list
            HashSet<GraphItem<RemoteResolveResult>> flattenedGraphItems = new();

            // Stores the list of graph nodes which point to their outer and inner nodes which represent the graph as a tree
            List<GraphNode<RemoteResolveResult>> graphNodes = new();

            // Stores the list of nodes by their LibraryRangeIndex for faster lookup
            Dictionary<LibraryRangeIndex, GraphNode<RemoteResolveResult>> nodesById = new();

            // Keeps track of visited items to detect when we come across a dependency that was already visited.  Any time a dependency is seen again, we need to determine if there was a downgrade or conflict.
            HashSet<LibraryDependencyIndex> visitedItems = new();

            // Stores the items to process, starting with the project itself and its children
            Queue<(LibraryDependencyIndex, LibraryRangeIndex, GraphNode<RemoteResolveResult>)> itemsToFlatten = new();

            Dictionary<LibraryRangeIndex, GraphNode<RemoteResolveResult>> versionConflicts = new();

            // Stores the list of downgrades
            Dictionary<LibraryRangeIndex, (LibraryRangeIndex FromParentLibraryRangeIndex, LibraryDependency FromLibraryDependency, LibraryRangeIndex ToParentLibraryRangeIndex, LibraryDependencyIndex ToLibraryDependencyIndex, bool IsCentralTransitive)> downgrades = new();

            // Get the resolved item for the project
            ResolvedDependencyGraphItem projectResolvedDependencyGraphItem = resolvedDependencyGraphItems[LibraryDependencyIndex.Project];

            // Create a node representing the root for the project
            GraphNode<RemoteResolveResult> rootGraphNode = new GraphNode<RemoteResolveResult>(projectResolvedDependencyGraphItem.LibraryDependency.LibraryRange)
            {
                Item = projectResolvedDependencyGraphItem.Item
            };

            graphNodes.Add(rootGraphNode);

            // Enqueue the project to be processed
            itemsToFlatten.Enqueue((LibraryDependencyIndex.Project, projectResolvedDependencyGraphItem.LibraryRangeIndex, rootGraphNode));

            nodesById.Add(projectResolvedDependencyGraphItem.LibraryRangeIndex, rootGraphNode);

            while (itemsToFlatten.Count > 0)
            {
                (LibraryDependencyIndex currentLibraryDependencyIndex, LibraryRangeIndex currentLibraryRangeIndex, GraphNode<RemoteResolveResult> currentGraphNode) = itemsToFlatten.Dequeue();

                // If there was no dependency in the resolved graph with the same name, it can be skipped and left out of the final graph
                if (!resolvedDependencyGraphItems.TryGetValue(currentLibraryDependencyIndex, out ResolvedDependencyGraphItem? resolvedDependencyGraphItem))
                {
                    continue;
                }

                flattenedGraphItems.Add(resolvedDependencyGraphItem.Item);

                for (int i = 0; i < resolvedDependencyGraphItem.Item.Data.Dependencies.Count; i++)
                {
                    LibraryDependency childLibraryDependency = resolvedDependencyGraphItem.Item.Data.Dependencies[i];

                    if (childLibraryDependency.LibraryRange.VersionRange == null)
                    {
                        continue;
                    }

                    if (StringComparer.OrdinalIgnoreCase.Equals(childLibraryDependency.Name, resolvedDependencyGraphItem.Item.Key.Name) || StringComparer.OrdinalIgnoreCase.Equals(childLibraryDependency.Name, rootGraphNode.Key.Name))
                    {
                        // A cycle exists since the current child dependency has the same name as its parent or as the root node
                        GraphNode<RemoteResolveResult> nodeWithCycle = new(childLibraryDependency.LibraryRange)
                        {
                            OuterNode = currentGraphNode,
                            Disposition = Disposition.Cycle
                        };

                        analyzeResult.Cycles.Add(nodeWithCycle);

                        continue;
                    }

                    LibraryDependencyIndex childLibraryDependencyIndex = resolvedDependencyGraphItem.GetDependencyIndexForDependencyAt(i);

                    if (!resolvedDependencyGraphItems.TryGetValue(childLibraryDependencyIndex, out ResolvedDependencyGraphItem? childResolvedDependencyGraphItem))
                    {
                        // If there was no dependency in the resolved graph with the same name as this child dependency, it can be skipped and left out of the final graph
                        continue;
                    }

                    LibraryRangeIndex childResolvedLibraryRangeIndex = childResolvedDependencyGraphItem.LibraryRangeIndex;
                    LibraryDependency childResolvedLibraryDependency = childResolvedDependencyGraphItem.LibraryDependency;

                    // Determine if this dependency has already been visited
                    if (!visitedItems.Add(childLibraryDependencyIndex))
                    {
                        LibraryRangeIndex currentRangeIndex = resolvedDependencyGraphItem.GetRangeIndexForDependencyAt(i);

                        if (resolvedDependencyGraphItem.Path.Contains(currentRangeIndex))
                        {
                            // If the dependency exists in the its own path, then a cycle exists
                            analyzeResult.Cycles.Add(
                                new GraphNode<RemoteResolveResult>(childLibraryDependency.LibraryRange)
                                {
                                    OuterNode = currentGraphNode,
                                    Disposition = Disposition.Cycle
                                });

                            continue;
                        }

                        // Verify downgrades only if the resolved dependency has a lower version than what was defined
                        if (!RemoteDependencyWalker.IsGreaterThanOrEqualTo(childResolvedLibraryDependency.LibraryRange.VersionRange, childLibraryDependency.LibraryRange.VersionRange))
                        {
                            // It is not a downgrade if: the dependency is transitive and is suppressed its parent or any of those parents' parent because the suppressions is an aggregate of everything suppressed above.
                            // For example, A -> B (PrivateAssets=All) -> C
                            // When processing C, it is not suppressed but its parent is, which is tracked in ResolvedDependencyGraphItem.Suppressions
                            if ((childLibraryDependencyIndex != LibraryDependencyIndex.Project && childLibraryDependency.SuppressParent == LibraryIncludeFlags.All)
                                || resolvedDependencyGraphItem.Suppressions.Count > 0 && resolvedDependencyGraphItem.Suppressions[0].Contains(childLibraryDependencyIndex))
                            {
                                continue;
                            }

                            // Get the resolved version in case a floating version like 1.* was specified
                            NuGetVersion? resolvedVersion = childResolvedDependencyGraphItem.Item.Data.Match?.Library?.Version;

                            if (resolvedVersion != null && childLibraryDependency.LibraryRange.VersionRange.Satisfies(resolvedVersion))
                            {
                                // Ignore the lower version if the resolved version satisfies the range of the dependency. This can happen when a floating version like 1.* was specified
                                // and the resolved version is 1.2.3, which satisfies the range of 1.*
                                continue;
                            }

                            // This lower version could be a downgrade if it hasn't already been seen
                            if (!downgrades.ContainsKey(childResolvedLibraryRangeIndex))
                            {
                                // Determine if any parents have actually eclipsed this version
                                if (childResolvedDependencyGraphItem.ParentPathsThatHaveBeenEclipsed != null)
                                {
                                    bool hasBeenEclipsedByParent = false;

                                    foreach (LibraryRangeIndex parent in childResolvedDependencyGraphItem.ParentPathsThatHaveBeenEclipsed)
                                    {
                                        if (resolvedDependencyGraphItem.Path.Contains(parent))
                                        {
                                            hasBeenEclipsedByParent = true;
                                            break;
                                        }
                                    }

                                    if (hasBeenEclipsedByParent)
                                    {
                                        continue;
                                    }
                                }

                                // Look through all of the parent nodes to see if any are a downgrade
                                bool foundParentDowngrade = false;

                                if (childResolvedDependencyGraphItem.Parents != null)
                                {
                                    foreach (LibraryRangeIndex parentLibraryRangeIndex in childResolvedDependencyGraphItem.Parents)
                                    {
                                        if (resolvedDependencyGraphItem.Path.Contains(parentLibraryRangeIndex) && !resolvedDependencyGraphItem.IsRootPackageReference)
                                        {
                                            downgrades.Add(
                                                childResolvedLibraryRangeIndex,
                                                (
                                                    FromParentLibraryRangeIndex: resolvedDependencyGraphItem.LibraryRangeIndex,
                                                    FromLibraryDependency: childLibraryDependency,
                                                    ToParentLibraryRangeIndex: parentLibraryRangeIndex,
                                                    ToLibraryDependencyIndex: childLibraryDependencyIndex,
                                                    IsCentralTransitive: isCentralPackageTransitivePinningEnabled ? childResolvedDependencyGraphItem.IsCentrallyPinnedTransitivePackage : false
                                                ));

                                            foundParentDowngrade = true;
                                            break;
                                        }
                                    }
                                }

                                // It is a downgrade if central transitive pinning is not being used or if the child is not a direct package reference
                                if (!foundParentDowngrade && (!isCentralPackageTransitivePinningEnabled || !childResolvedDependencyGraphItem.IsRootPackageReference))
                                {
                                    downgrades.Add(
                                        childResolvedLibraryRangeIndex,
                                        (
                                            FromParentLibraryRangeIndex: resolvedDependencyGraphItem.LibraryRangeIndex,
                                            FromLibraryDependency: childLibraryDependency,
                                            ToParentLibraryRangeIndex: childResolvedDependencyGraphItem.Path[childResolvedDependencyGraphItem.Path.Length - 1],
                                            ToLibraryDependencyIndex: childLibraryDependencyIndex,
                                            IsCentralTransitive: isCentralPackageTransitivePinningEnabled ? childResolvedDependencyGraphItem.IsCentrallyPinnedTransitivePackage : false
                                        ));
                                }
                            }

                            // Ignore this child dependency since it was a downgrade
                            continue;
                        }

                        // If it wasn't a downgrade, then it was a version conflict like A -> B [1.0.0] but B 1.0.0 was not in the resolved graph
                        if (versionConflicts.ContainsKey(childResolvedLibraryRangeIndex) && !nodesById.ContainsKey(currentRangeIndex))
                        {
                            GraphNode<RemoteResolveResult> nodeWithConflict = new(childResolvedLibraryDependency.LibraryRange)
                            {
                                Item = childResolvedDependencyGraphItem.Item,
                                Disposition = Disposition.Acceptable,
                                OuterNode = currentGraphNode,
                            };

                            currentGraphNode.InnerNodes.Add(nodeWithConflict);

                            nodesById.Add(currentRangeIndex, nodeWithConflict);

                            continue;
                        }

                        // Ignore this child dependency since it was not a cycle, downgrade, or version conflict but was already visited
                        continue;
                    }

                    // Create a GraphNode for the item
                    GraphNode<RemoteResolveResult> newGraphNode = new(childResolvedLibraryDependency.LibraryRange)
                    {
                        Item = childResolvedDependencyGraphItem.Item
                    };

                    if (childResolvedDependencyGraphItem.IsCentrallyPinnedTransitivePackage && !childResolvedDependencyGraphItem.IsRootPackageReference)
                    {
                        // If this child is transitively pinned, the GraphNode needs to have certain properties set
                        newGraphNode.Disposition = Disposition.Accepted;
                        newGraphNode.Item.IsCentralTransitive = true;

                        // Treat the transitively pinned dependency as a child of the root node
                        newGraphNode.OuterNode = rootGraphNode;
                        rootGraphNode.InnerNodes.Add(newGraphNode);
                    }
                    else
                    {
                        // Set properties for the node to represent a parent/child relationship
                        newGraphNode.OuterNode = currentGraphNode;
                        currentGraphNode.InnerNodes.Add(newGraphNode);
                    }

                    if (!childResolvedDependencyGraphItem.IsRootPackageReference && isCentralPackageTransitivePinningEnabled && childLibraryDependency.SuppressParent != LibraryIncludeFlags.All && !downgrades.ContainsKey(childResolvedLibraryRangeIndex) && !RemoteDependencyWalker.IsGreaterThanOrEqualTo(childResolvedDependencyGraphItem.LibraryDependency.LibraryRange.VersionRange, childLibraryDependency.LibraryRange.VersionRange))
                    {
                        // This is a downgrade if:
                        // 1. This is not a direct dependency
                        // 2. This is a central transitive pinned dependency
                        // 3. This is a transitive dependency which is not PrivateAssets=All
                        // 4. This has not already been detected
                        // 5. The version is lower
                        downgrades.Add(
                            childResolvedDependencyGraphItem.LibraryRangeIndex,
                            (
                                FromParentLibraryRangeIndex: currentLibraryRangeIndex,
                                FromLibraryDependency: childLibraryDependency,
                                ToParentLibraryRangeIndex: LibraryRangeIndex.Project,
                                ToLibraryDependencyIndex: childLibraryDependencyIndex,
                                IsCentralTransitive: true
                            ));
                    }

                    // This is a version conflict if:
                    // 1. The node is not a project and isn't unresolved
                    // 2. The conflict has not already been detected
                    // 3. The dependency is transitive and doesn't have PrivateAssets=All
                    // 4. The dependency has a version specified
                    // 5. The version range is not satisfied by the resolved version
                    // 6. A corresponding downgrade was not detected
                    if (newGraphNode.Item.Key.Type != LibraryType.Project
                        && newGraphNode.Item.Key.Type != LibraryType.ExternalProject
                        && newGraphNode.Item.Key.Type != LibraryType.Unresolved
                        && !versionConflicts.ContainsKey(childResolvedLibraryRangeIndex)
                        && childLibraryDependency.SuppressParent != LibraryIncludeFlags.All
                        && childLibraryDependency.LibraryRange.VersionRange != null
                        && !childLibraryDependency.LibraryRange.VersionRange!.Satisfies(newGraphNode.Item.Key.Version)
                        && !downgrades.ContainsKey(childResolvedLibraryRangeIndex))
                    {
                        // Remove the existing node so it can be replaced with a node representing the conflict
                        currentGraphNode.InnerNodes.Remove(newGraphNode);

                        GraphNode<RemoteResolveResult> conflictingNode = new(childLibraryDependency.LibraryRange)
                        {
                            Disposition = Disposition.Acceptable,
                            Item = new GraphItem<RemoteResolveResult>(
                                new LibraryIdentity(
                                    childLibraryDependency.Name,
                                    childLibraryDependency.LibraryRange.VersionRange.MinVersion!,
                                    LibraryType.Package)),
                            OuterNode = currentGraphNode,
                        };

                        // Add the conflict node to the parent
                        currentGraphNode.InnerNodes.Add(conflictingNode);

                        // Track the version conflict for later
                        versionConflicts.Add(childResolvedLibraryRangeIndex, conflictingNode);

                        // Process the next child
                        continue;
                    }

                    // Add the node to the lookup for later
                    nodesById.Add(childResolvedLibraryRangeIndex, newGraphNode);

                    // Enqueue the child for processing
                    itemsToFlatten.Enqueue((childLibraryDependencyIndex, childResolvedLibraryRangeIndex, newGraphNode));

                    if (newGraphNode.Item.Key.Type == LibraryType.Unresolved)
                    {
                        // Keep track of unresolved packages and fail the restore
                        unresolvedPackages.Add(childResolvedLibraryDependency.LibraryRange);

                        success = false;
                    }
                    else
                    {
                        // Keep track of the resolved packages
                        resolvedPackages.Add(new ResolvedDependencyKey(
                            parent: newGraphNode.OuterNode.Item.Key,
                            range: newGraphNode.Key.VersionRange,
                            child: newGraphNode.Item.Key));
                    }
                }
            } // End of walking all declared dependencies for cycles, downgrades, and conflicts

            // Add applicable version conflicts to the analyze results
            if (versionConflicts.Count > 0)
            {
                foreach (KeyValuePair<LibraryRangeIndex, GraphNode<RemoteResolveResult>> versionConflict in versionConflicts)
                {
                    if (nodesById.TryGetValue(versionConflict.Key, out GraphNode<RemoteResolveResult>? selected))
                    {
                        analyzeResult.VersionConflicts.Add(
                            new VersionConflictResult<RemoteResolveResult>
                            {
                                Conflicting = versionConflict.Value,
                                Selected = selected,
                            });
                    }
                }
            }

            // Add applicable downgrades to the analyze results
            if (downgrades.Count > 0)
            {
                foreach ((LibraryRangeIndex FromParentLibraryRangeIndex, LibraryDependency FromLibraryDependency, LibraryRangeIndex ToParentLibraryRangeIndex, LibraryDependencyIndex ToLibraryDependencyIndex, bool IsCentralTransitive) downgrade in downgrades.Values)
                {
                    // Ignore the downgrade if a node was not created for its from or to, or if it never ended up in the resolved graph.  Sometimes a downgrade is detected but later during graph
                    // resolution it is resolved so this verifies if the downgrade ended up in the final graph
                    if (!nodesById.TryGetValue(downgrade.FromParentLibraryRangeIndex, out GraphNode<RemoteResolveResult>? fromParentNode)
                        || !nodesById.TryGetValue(downgrade.ToParentLibraryRangeIndex, out GraphNode<RemoteResolveResult>? toParentNode)
                        || !resolvedDependencyGraphItems.TryGetValue(downgrade.ToLibraryDependencyIndex, out ResolvedDependencyGraphItem? toResolvedDependencyGraphItem))
                    {
                        continue;
                    }

                    // Add the downgrade
                    analyzeResult.Downgrades.Add(new DowngradeResult<RemoteResolveResult>
                    {
                        DowngradedFrom = new GraphNode<RemoteResolveResult>(downgrade.FromLibraryDependency.LibraryRange)
                        {
                            Item = new GraphItem<RemoteResolveResult>(
                                new LibraryIdentity(
                                    downgrade.FromLibraryDependency.Name,
                                    downgrade.FromLibraryDependency.LibraryRange.VersionRange?.MinVersion!,
                                    LibraryType.Package)),
                            OuterNode = fromParentNode
                        },
                        DowngradedTo = new GraphNode<RemoteResolveResult>(toResolvedDependencyGraphItem.LibraryDependency.LibraryRange)
                        {
                            Item = new GraphItem<RemoteResolveResult>(toResolvedDependencyGraphItem.Item.Key)
                            {
                                IsCentralTransitive = downgrade.IsCentralTransitive
                            },
                            OuterNode = downgrade.IsCentralTransitive ? rootGraphNode : toParentNode,
                        }
                    });
                }
            }

            // If central transitive pinning is enabled, we need to add all of its parent nodes.  This has to happen at the end after all of the nodes in graph have been created
            if (isCentralPackageTransitivePinningEnabled)
            {
                foreach (KeyValuePair<LibraryDependencyIndex, ResolvedDependencyGraphItem> resolvedDependencyGraphItemEntry in resolvedDependencyGraphItems)
                {
                    ResolvedDependencyGraphItem resolvedDependencyGraphItem = resolvedDependencyGraphItemEntry.Value;

                    // Skip this item if:
                    // 1. It is not pinned
                    // 2. It is a direct package reference
                    // 3. It has not parents
                    // 4. A node was not created for it
                    if (!resolvedDependencyGraphItem.IsCentrallyPinnedTransitivePackage
                        || resolvedDependencyGraphItem.IsRootPackageReference
                        || resolvedDependencyGraphItem.Parents == null
                        || resolvedDependencyGraphItem.Parents.Count == 0
                        || !nodesById.TryGetValue(resolvedDependencyGraphItem.LibraryRangeIndex, out GraphNode<RemoteResolveResult>? currentNode))
                    {
                        continue;
                    }

                    // Get the corresponding node in the graph for each parent and add it the list of parent nodes
                    foreach (LibraryRangeIndex parentLibraryRangeIndex in resolvedDependencyGraphItem.Parents.NoAllocEnumerate())
                    {
                        if (!nodesById.TryGetValue(parentLibraryRangeIndex, out GraphNode<RemoteResolveResult>? parentNode))
                        {
                            // Skip nodes that weren't created
                            continue;
                        }
                        currentNode.ParentNodes.Add(parentNode);
                    }
                }
            }

            // Get the list of packages to install
            HashSet<RemoteMatch> packagesToInstall = await context.GetUnresolvedRemoteMatchesAsync();

            // Create a RestoreTargetGraph with all of the information
            RestoreTargetGraph restoreTargetGraph = new(
                Array.Empty<ResolverConflict>(),
                frameworkRuntimePair.Framework,
                string.IsNullOrWhiteSpace(frameworkRuntimePair.RuntimeIdentifier) ? null : frameworkRuntimePair.RuntimeIdentifier,
                runtimeGraph,
                graphNodes,
                install: packagesToInstall,
                flattened: flattenedGraphItems,
                unresolved: unresolvedPackages,
                analyzeResult,
                resolvedDependencies: resolvedPackages);

            return (success, restoreTargetGraph);
        }

        private static bool EvaluateRuntimeDependencies(ref LibraryDependency libraryDependency, RuntimeGraph? runtimeGraph, string? runtimeIdentifier, ref HashSet<LibraryDependency>? runtimeDependencies)
        {
            LibraryRange libraryRange = libraryDependency.LibraryRange;

            if (runtimeGraph == null || string.IsNullOrEmpty(runtimeIdentifier) || !RemoteDependencyWalker.EvaluateRuntimeDependencies(ref libraryRange, runtimeIdentifier, runtimeGraph, ref runtimeDependencies))
            {
                return false;
            }

            libraryDependency = new LibraryDependency(libraryDependency)
            {
                LibraryRange = libraryRange
            };

            return true;
        }

        private static bool HasCommonAncestor(LibraryRangeIndex[] left, LibraryRangeIndex[] right)
        {
            for (int i = 0; i < left.Length && i < right.Length; i++)
            {
                if (left[i] != right[i])
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Determine if the chosen item should be evicted based on the <see cref="LibraryRange.TypeConstraint" />.
        /// </summary>
        /// <remarks>
        /// We should evict on type constraint if the type constraint of the current item has the same version but has a more restrictive type constraint than the chosen item.
        /// This happens when the chosen item's type constraint is broader (e.g. PackageProjectExternal) than the current item's type constraint (e.g. Package).
        /// </remarks>
        /// <returns></returns>
        private static bool ShouldEvictOnTypeConstraint(DependencyGraphItem currentDependencyGraphItem, ResolvedDependencyGraphItem resolvedDependencyGraphItem)
        {
            LibraryDependency currentLibraryDependency = currentDependencyGraphItem.LibraryDependency;
            LibraryDependency chosenLibraryDependency = resolvedDependencyGraphItem.LibraryDependency;

            // We should evict the chosen item if it is a package but the current item is a project since projects should be chosen over packages
            if (chosenLibraryDependency.LibraryRange.TypeConstraint == LibraryDependencyTarget.PackageProjectExternal
                && currentLibraryDependency.LibraryRange.TypeConstraint == LibraryDependencyTarget.ExternalProject)
            {
                return true;
            }

            LibraryRangeIndex currentLibraryRangeIndex = currentDependencyGraphItem.LibraryRangeIndex;
            LibraryRangeIndex chosenLibraryRangeIndex = resolvedDependencyGraphItem.LibraryRangeIndex;

            // Do not evict if:
            // 1. The current item and chosen item are not the same version
            // 2. The current item and chosen item have the same type constraint
            // 3. The chosen item has a strict type constraint instead of the more generic "PackageProjectExternal"
            if (currentLibraryRangeIndex != chosenLibraryRangeIndex
                || currentLibraryDependency.LibraryRange.TypeConstraint == chosenLibraryDependency.LibraryRange.TypeConstraint
                || chosenLibraryDependency.LibraryRange.TypeConstraint != LibraryDependencyTarget.PackageProjectExternal)
            {
                return false;
            }

            LibraryDependencyTarget packageProjectExternalFlags = currentLibraryDependency.LibraryRange.TypeConstraint & LibraryDependencyTarget.PackageProjectExternal;
            LibraryDependencyTarget nonPackageProjectExternalFlats = currentLibraryDependency.LibraryRange.TypeConstraint & ~LibraryDependencyTarget.PackageProjectExternal;

            // Evict if the type constraint of the current item is more precise than "PackageProjectExternal" and the current item is a project
            if (packageProjectExternalFlags != LibraryDependencyTarget.None && nonPackageProjectExternalFlats == LibraryDependencyTarget.None)
            {
                return resolvedDependencyGraphItem.Item.Key.Type == LibraryType.Project;
            }

            return false;
        }

        private static bool VersionRangePreciseEquals(VersionRange a, VersionRange b)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            if ((a.MinVersion != null) != (b.MinVersion != null))
            {
                return false;
            }

            if (a.MinVersion != b.MinVersion)
            {
                return false;
            }

            if ((a.MaxVersion != null) != (b.MaxVersion != null))
            {
                return false;
            }

            if (a.MaxVersion != b.MaxVersion)
            {
                return false;
            }

            if (a.IsMinInclusive != b.IsMinInclusive)
            {
                return false;
            }

            if (a.IsMaxInclusive != b.IsMaxInclusive)
            {
                return false;
            }

            if ((a.Float != null) != (b.Float != null))
            {
                return false;
            }

            if (a.Float != b.Float)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Indexes all central package versions if central transitive pinning is enabled.
        /// </summary>
        /// <param name="isCentralPackageTransitivePinningEnabled">Indicates whether or not central transitive pinning is enabled.</param>
        /// <param name="projectTargetFramework">The <see cref="TargetFrameworkInformation" /> of the project.</param>
        /// <returns>A <see cref="Dictionary{TKey, TValue}" /> of indexed version ranges by their <see cref="LibraryDependencyIndex" /> if central transitive pinning is enabled, otherwise <see langword="null" />.</returns>
        private Dictionary<LibraryDependencyIndex, VersionRange>? IndexPinnedPackageVersions(bool isCentralPackageTransitivePinningEnabled, TargetFrameworkInformation? projectTargetFramework)
        {
            if (!isCentralPackageTransitivePinningEnabled || projectTargetFramework == null || projectTargetFramework.CentralPackageVersions == null)
            {
                return null;
            }

            Dictionary<LibraryDependencyIndex, VersionRange>? pinnedPackageVersions = new(capacity: projectTargetFramework.CentralPackageVersions.Count);

            foreach (KeyValuePair<string, CentralPackageVersion> item in projectTargetFramework.CentralPackageVersions.NoAllocEnumerate())
            {
                LibraryDependencyIndex libraryDependencyIndex = _indexingTable.Index(item.Value);

                pinnedPackageVersions[libraryDependencyIndex] = item.Value.VersionRange;
            }

            return pinnedPackageVersions;
        }

        private async Task<Dictionary<LibraryDependencyIndex, ResolvedDependencyGraphItem>> ResolveDependencyGraphItemsAsync(
            bool isCentralPackageTransitivePinningEnabled,
            FrameworkRuntimePair pair,
            TargetFrameworkInformation? projectTargetFramework,
            RuntimeGraph? runtimeGraph,
            Dictionary<LibraryDependencyIndex,
            VersionRange>? pinnedPackageVersions,
            DependencyGraphItem rootProjectDependencyGraphItem,
            RemoteWalkContext context,
            CancellationToken token)
        {
            // Stores the resolved dependency graph items
            Dictionary<LibraryDependencyIndex, ResolvedDependencyGraphItem> resolvedDependencyGraphItems = new(ResolvedDependencyGraphItemDictionarySize);

            // Stores a list of direct package references by their LibraryDependencyIndex so that we can quickly determine if a transitive package can be ignored
            HashSet<LibraryDependencyIndex>? directPackageReferences = default;

            // Stores the queue of DependencyGraphItem objects to process
            Queue<DependencyGraphItem> dependencyGraphItemQueue = new(DependencyGraphItemQueueSize);

            // Stores any evictions to process
            Dictionary<LibraryRangeIndex, (LibraryRangeIndex[], LibraryDependencyIndex, LibraryDependencyTarget)> evictions = new Dictionary<LibraryRangeIndex, (LibraryRangeIndex[], LibraryDependencyIndex, LibraryDependencyTarget)>(EvictionsDictionarySize);

        // Used to start over when a dependency has multiple descendants of an item to be evicted.
        //
        // Project
        // ├── A 1.0.0
        // │   └── B 1.0.0
        // │       └── C 1.0.0
        // │           └── D 1.0.0
        // └── X 2.0.0
        //     └── Y 2.0.0
        //         └── G 2.0.0
        //             └── B 2.0.0
        // The items are processed in the following order:
        // Chose A 1.0.0 and X 1.0.0
        // Chose B 1.0.0 and Y 2.0.0
        // Chose C 1.0.0 and G 2.0.0
        // Chose D 1.0.0 and B 2.0.0, but B 2.0.0 should evict C 1.0.0 and D 1.0.0
        //
        // In this case, the entire walk is started over and B 1.0.0 is left out of the graph, leading to C 1.0.0 and D 1.0.0 also being left out.
        //
        StartOver:

            dependencyGraphItemQueue.Clear();
            resolvedDependencyGraphItems.Clear();

            dependencyGraphItemQueue.Enqueue(rootProjectDependencyGraphItem);

            while (dependencyGraphItemQueue.Count > 0)
            {
                DependencyGraphItem currentDependencyGraphItem = dependencyGraphItemQueue.Dequeue();

                // Determine if what is being processed is the root project itself which has different rules vs a transitive dependency
                bool isRootProject = currentDependencyGraphItem.LibraryDependencyIndex == LibraryDependencyIndex.Project;

                GraphItem<RemoteResolveResult> currentGraphItem = await currentDependencyGraphItem.GetGraphItemAsync(_request.Project.RestoreMetadata, projectTargetFramework?.PackagesToPrune, isRootProject, _logger);

                LibraryDependencyTarget typeConstraint = currentDependencyGraphItem.LibraryDependency.LibraryRange.TypeConstraint;
                if (evictions.TryGetValue(currentDependencyGraphItem.LibraryRangeIndex, out (LibraryRangeIndex[], LibraryDependencyIndex, LibraryDependencyTarget) eviction))
                {
                    (LibraryRangeIndex[] evictedPath, LibraryDependencyIndex evictedDepIndex, LibraryDependencyTarget evictedTypeConstraint) = eviction;

                    // If we evicted this same version previously, but the type constraint of currentRef is more stringent (package), then do not skip the current item - this is the one we want.
                    // This is tricky. I don't really know what this means. Normally we'd key off of versions instead.
                    if (!((evictedTypeConstraint == LibraryDependencyTarget.PackageProjectExternal || evictedTypeConstraint == LibraryDependencyTarget.ExternalProject) &&
                        currentDependencyGraphItem.LibraryDependency.LibraryRange.TypeConstraint == LibraryDependencyTarget.Package))
                    {
                        continue;
                    }
                }

                // Determine if a dependency with the same name has not already been chosen
                if (!resolvedDependencyGraphItems.TryGetValue(currentDependencyGraphItem.LibraryDependencyIndex, out ResolvedDependencyGraphItem? chosenResolvedItem))
                {
                    // Create a resolved dependency graph item and add it to the list of chosen items
                    chosenResolvedItem = new ResolvedDependencyGraphItem(currentGraphItem, currentDependencyGraphItem, _indexingTable)
                    {
                        IsCentrallyPinnedTransitivePackage = currentDependencyGraphItem.IsCentrallyPinnedTransitivePackage,
                        IsRootPackageReference = currentDependencyGraphItem.IsRootPackageReference,
                        Suppressions = new List<HashSet<LibraryDependencyIndex>>
                        {
                            currentDependencyGraphItem.Suppressions!
                        }
                    };

                    resolvedDependencyGraphItems.Add(currentDependencyGraphItem.LibraryDependencyIndex, chosenResolvedItem);
                }
                else // A dependency with the same name has already been chosen so we need to decide what to do with it
                {
                    if (chosenResolvedItem.IsRootPackageReference)
                    {
                        // If the chosen dependency graph item is a direct dependency, it should always be chosen regardless of version so do not process this dependency
                        continue;
                    }

                    if (chosenResolvedItem.LibraryDependency.LibraryRange.TypeConstraint == LibraryDependencyTarget.ExternalProject
                        && currentDependencyGraphItem.LibraryDependency.LibraryRange.TypeConstraintAllows(LibraryDependencyTarget.Package)
                        && currentGraphItem.Key.Type == LibraryType.Project)
                    {
                        // If the chosen dependency graph item is a project reference, it should always be chosen over a package reference. In this case, a project has already been chosen
                        // for the graph with the same name as a transitive package reference, so this item does not need to be processed.
                        continue;
                    }

                    // Determine if the chosen item should be evicted based on type constraint.
                    bool evictOnTypeConstraint = ShouldEvictOnTypeConstraint(currentDependencyGraphItem, chosenResolvedItem);

                    VersionRange currentVersionRange = currentDependencyGraphItem.LibraryDependency.LibraryRange.VersionRange ?? VersionRange.All;
                    VersionRange chosenVersionRange = chosenResolvedItem.LibraryDependency.LibraryRange.VersionRange ?? VersionRange.All;

                    // The chosen item should be evicted or the current item has a greater version, determine if the current item should be chosen instead
                    if (evictOnTypeConstraint || !RemoteDependencyWalker.IsGreaterThanOrEqualTo(chosenVersionRange, currentVersionRange))
                    {
                        if (chosenResolvedItem.LibraryDependency.LibraryRange.TypeConstraintAllows(LibraryDependencyTarget.Package) && currentDependencyGraphItem.LibraryDependency.LibraryRange.TypeConstraintAllows(LibraryDependencyTarget.Package))
                        {
                            if (chosenResolvedItem.Parents != null)
                            {
                                bool atLeastOneCommonAncestor = false;

                                foreach (LibraryRangeIndex parentRangeIndex in chosenResolvedItem.Parents.NoAllocEnumerate())
                                {
                                    if (currentDependencyGraphItem.Path.Length > 2 && currentDependencyGraphItem.Path[currentDependencyGraphItem.Path.Length - 2] == parentRangeIndex)
                                    {
                                        atLeastOneCommonAncestor = true;
                                        break;
                                    }
                                }

                                if (atLeastOneCommonAncestor)
                                {
                                    // At least one of the parents of the chosen item is a common ancestor of the current item, so the current item should be skipped
                                    continue;
                                }
                            }

                            if (HasCommonAncestor(chosenResolvedItem.Path, currentDependencyGraphItem.Path))
                            {
                                // The current item has a common ancestor of the chosen item and should not be chosen since children in the same leaf of the graph do not eclipse each other
                                continue;
                            }

                            if (chosenResolvedItem.ParentPathsThatHaveBeenEclipsed != null)
                            {
                                // Determine if the current item is under a parent that has already been eclipsed
                                bool hasAlreadyBeenEclipsed = false;

                                foreach (LibraryRangeIndex parentRangeIndex in chosenResolvedItem.ParentPathsThatHaveBeenEclipsed)
                                {
                                    if (currentDependencyGraphItem.Path.Contains(parentRangeIndex))
                                    {
                                        hasAlreadyBeenEclipsed = true;
                                        break;
                                    }
                                }

                                if (hasAlreadyBeenEclipsed)
                                {
                                    continue;
                                }
                            }
                        }

                        // Remove the chosen item
                        resolvedDependencyGraphItems.Remove(currentDependencyGraphItem.LibraryDependencyIndex);

                        // Record an eviction for the item we are replacing.  The eviction path is for the current item.
                        LibraryRangeIndex evictedLibraryRangeIndex = chosenResolvedItem.LibraryRangeIndex;

                        bool shouldStartOver = false;

                        // To "evict" a chosen item, we need to also remove all of its transitive children from the chosen list.
                        HashSet<LibraryRangeIndex>? evicteesToRemove = default;

                        foreach (KeyValuePair<LibraryRangeIndex, (LibraryRangeIndex[], LibraryDependencyIndex, LibraryDependencyTarget)> evictee in evictions)
                        {
                            (LibraryRangeIndex[] evicteePath, LibraryDependencyIndex evicteeDepIndex, LibraryDependencyTarget evicteeTypeConstraint) = evictee.Value;

                            // See if the evictee is a descendant of the evicted item
                            if (evicteePath.Contains(evictedLibraryRangeIndex))
                            {
                                // if evictee.Key (depIndex) == currentDepIndex && evictee.TypeConstraint == ExternalProject --> Don't remove it.  It must remain evicted.
                                // If the evictee to remove is the same dependency, but the project version of said dependency, then do not remove it - it must remain evicted in favor of the package.
                                if (!(evicteeDepIndex == currentDependencyGraphItem.LibraryDependencyIndex &&
                                    (evicteeTypeConstraint == LibraryDependencyTarget.ExternalProject || evicteeTypeConstraint == LibraryDependencyTarget.PackageProjectExternal)))
                                {
                                    evicteesToRemove ??= new HashSet<LibraryRangeIndex>();

                                    evicteesToRemove.Add(evictee.Key);
                                }
                            }
                        }

                        if (evicteesToRemove != null)
                        {
                            foreach (LibraryRangeIndex evicteeToRemove in evicteesToRemove)
                            {
                                evictions.Remove(evicteeToRemove);

                                // Indicate that we can't simply evict this item and instead we need to start over knowing that this item should be skipped
                                shouldStartOver = true;
                            }
                        }

                        foreach (KeyValuePair<LibraryDependencyIndex, ResolvedDependencyGraphItem> chosenItem in resolvedDependencyGraphItems)
                        {
                            if (chosenItem.Value.Path.Contains(evictedLibraryRangeIndex))
                            {
                                // Indicate that we can't simply evict this item and instead we need to start over knowing that this item should be skipped
                                shouldStartOver = true;
                                break;
                            }
                        }

                        // Add the eviction to be used later
                        evictions[evictedLibraryRangeIndex] = (DependencyGraphItemIndexer.CreatePathToRef(currentDependencyGraphItem.Path, currentDependencyGraphItem.LibraryRangeIndex), currentDependencyGraphItem.LibraryDependencyIndex, chosenResolvedItem.LibraryDependency.LibraryRange.TypeConstraint);

                        if (shouldStartOver)
                        {
                            goto StartOver;
                        }

                        // Add the item to the list of chosen items
                        chosenResolvedItem = new ResolvedDependencyGraphItem(currentGraphItem, currentDependencyGraphItem, _indexingTable)
                        {
                            IsCentrallyPinnedTransitivePackage = currentDependencyGraphItem.IsCentrallyPinnedTransitivePackage,
                            IsRootPackageReference = currentDependencyGraphItem.IsRootPackageReference,
                            Suppressions = new List<HashSet<LibraryDependencyIndex>>
                            {
                                currentDependencyGraphItem.Suppressions!
                            },
                        };

                        resolvedDependencyGraphItems.Add(currentDependencyGraphItem.LibraryDependencyIndex, chosenResolvedItem);

                        // Recreate the queue but leave out any items that are children of the chosen item that was just removed which essentially evicts unprocessed children from the queue
                        Queue<DependencyGraphItem> newDependencyGraphItemQueue = new(DependencyGraphItemQueueSize);

                        while (dependencyGraphItemQueue.Count > 0)
                        {
                            DependencyGraphItem item = dependencyGraphItemQueue.Dequeue();

                            if (!item.Path.Contains(evictedLibraryRangeIndex))
                            {
                                newDependencyGraphItemQueue.Enqueue(item);
                            }
                        }

                        dependencyGraphItemQueue = newDependencyGraphItemQueue;
                    }
                    else if (!VersionRangePreciseEquals(chosenVersionRange, currentVersionRange)) // The current item has a lower version
                    {
                        bool hasCommonAncestor = HasCommonAncestor(chosenResolvedItem.Path, currentDependencyGraphItem.Path);

                        if (!hasCommonAncestor)
                        {
                            chosenResolvedItem.ParentPathsThatHaveBeenEclipsed ??= new HashSet<LibraryRangeIndex>();

                            // Keeps track of parents that have been eclipsed
                            chosenResolvedItem.ParentPathsThatHaveBeenEclipsed.Add(currentDependencyGraphItem.Path[currentDependencyGraphItem.Path.Length - 1]);
                        }

                        // Do not process this item
                        continue;
                    }
                    else // The current item and chosen item have the same version
                    {
                        if (!chosenResolvedItem.IsRootPackageReference)
                        {
                            // Keep track of the parents of this item
                            chosenResolvedItem.Parents?.Add(currentDependencyGraphItem.Parent);
                        }

                        if (chosenResolvedItem.Suppressions.Count == 1 && chosenResolvedItem.Suppressions[0].Count == 0 && HasCommonAncestor(chosenResolvedItem.Path, currentDependencyGraphItem.Path))
                        {
                            // Skip this item if it has no suppressions and has a common ancestor
                            continue;
                        }
                        else if (currentDependencyGraphItem.Suppressions!.Count == 0) // The current item has no suppressions
                        {
                            // Replace the chosen item with the current one since they are basically the same and process its children
                            resolvedDependencyGraphItems.Remove(currentDependencyGraphItem.LibraryDependencyIndex);

                            chosenResolvedItem = new ResolvedDependencyGraphItem(currentGraphItem, currentDependencyGraphItem, _indexingTable, chosenResolvedItem.Parents)
                            {
                                IsCentrallyPinnedTransitivePackage = chosenResolvedItem.IsCentrallyPinnedTransitivePackage,
                                IsRootPackageReference = chosenResolvedItem.IsRootPackageReference,
                                Suppressions = new List<HashSet<LibraryDependencyIndex>>
                                {
                                    currentDependencyGraphItem.Suppressions,
                                },
                            };

                            resolvedDependencyGraphItems.Add(currentDependencyGraphItem.LibraryDependencyIndex, chosenResolvedItem);
                        }
                        else // The chosen item and current item have a different set of suppressions
                        {
                            bool isEqualOrSuperSetDisposition = false;
                            foreach (HashSet<LibraryDependencyIndex> chosenDependencyGraphItemSuppression in chosenResolvedItem.Suppressions)
                            {
                                if (currentDependencyGraphItem.Suppressions.IsSupersetOf(chosenDependencyGraphItemSuppression))
                                {
                                    isEqualOrSuperSetDisposition = true;
                                }
                            }

                            if (isEqualOrSuperSetDisposition)
                            {
                                // Do not process the current item if its suppressions are identical
                                continue;
                            }
                            else
                            {
                                // Replace the chosen item with the current item with the the combined list of suppressions and process its children
                                resolvedDependencyGraphItems.Remove(currentDependencyGraphItem.LibraryDependencyIndex);

                                chosenResolvedItem = new ResolvedDependencyGraphItem(currentGraphItem, currentDependencyGraphItem, _indexingTable, chosenResolvedItem.Parents)
                                {
                                    IsCentrallyPinnedTransitivePackage = chosenResolvedItem.IsCentrallyPinnedTransitivePackage,
                                    IsRootPackageReference = chosenResolvedItem.IsRootPackageReference,
                                    Suppressions =
                                    [
                                        currentDependencyGraphItem.Suppressions,
                                        .. chosenResolvedItem.Suppressions
                                    ],
                                };

                                resolvedDependencyGraphItems.Add(currentDependencyGraphItem.LibraryDependencyIndex, chosenResolvedItem);
                            }
                        }
                    }
                }

                // Determine the list of root dependencies if the current item is the project, this is used for a faster lookup later to determine if a transitive dependency
                // should be ignored when there is a direct dependency.
                if (isRootProject)
                {

#if NETSTANDARD
                    directPackageReferences = new HashSet<LibraryDependencyIndex>();
#else
                    directPackageReferences = new HashSet<LibraryDependencyIndex>(capacity: chosenResolvedItem.Item.Data.Dependencies.Count);
#endif

                    for (int i = 0; i < chosenResolvedItem.Item.Data.Dependencies.Count; i++)
                    {
                        LibraryDependency rootLibraryDependency = chosenResolvedItem.Item.Data.Dependencies[i];

                        if (rootLibraryDependency.LibraryRange.TypeConstraintAllows(LibraryDependencyTarget.Package))
                        {
                            directPackageReferences!.Add(chosenResolvedItem.GetDependencyIndexForDependencyAt(i));
                        }
                    }
                }

                HashSet<LibraryDependencyIndex>? suppressions = default;

                // Only gather suppressed dependencies if the current item is not the root project
                if (!isRootProject)
                {
                    // If this is not the root project, loop through the dependencies and keep track of which ones have PrivateAssets=All which we consider a "suppression"
                    for (int i = 0; i < chosenResolvedItem.Item.Data.Dependencies.Count; i++)
                    {
                        LibraryDependency dependency = chosenResolvedItem.Item.Data.Dependencies[i];

                        // Skip any packages with a missing versions
                        if (dependency.LibraryRange.VersionRange == null)
                        {
                            continue;
                        }

                        LibraryDependencyIndex chosenResolvedItemChildLibraryDependencyIndex = chosenResolvedItem.GetDependencyIndexForDependencyAt(i);

                        // Suppress this dependency if PrivateAssets is set to "All"
                        if (dependency.SuppressParent == LibraryIncludeFlags.All)
                        {
                            suppressions ??= new HashSet<LibraryDependencyIndex>();

                            suppressions.Add(chosenResolvedItemChildLibraryDependencyIndex);
                        }
                    }
                }

                // The list of suppressions should be an aggregate of all parent item's suppressions so add the parent suppressions to the list, otherwise just use the current item's suppressions
                if (suppressions != null)
                {
                    suppressions.AddRange(currentDependencyGraphItem.Suppressions);
                }
                else
                {
                    suppressions = currentDependencyGraphItem.Suppressions;
                }

                // Loop through the dependencies now that we know which ones to suppress
                for (int i = 0; i < chosenResolvedItem.Item.Data.Dependencies.Count; i++)
                {
                    LibraryDependency childDependency = chosenResolvedItem.Item.Data.Dependencies[i];
                    LibraryDependencyIndex childLibraryDependencyIndex = chosenResolvedItem.GetDependencyIndexForDependencyAt(i);

                    if (childLibraryDependencyIndex == LibraryDependencyIndex.Project)
                    {
                        // Skip any dependency with the same name as the project
                        continue;
                    }

                    LibraryRangeIndex childLibraryRangeIndex = chosenResolvedItem.GetRangeIndexForDependencyAt(i);
                    bool isPackage = childDependency.LibraryRange.TypeConstraintAllows(LibraryDependencyTarget.Package);
                    bool isRootPackageReference = (currentDependencyGraphItem.LibraryDependencyIndex == LibraryDependencyIndex.Project) && isPackage;

                    // Skip this dependency if:
                    // 1. the VersionRange is null
                    // 2. It is not transitively pinned and PrivateAssets=All
                    // 3. This child is not a direct package reference and there is already a direct package reference to it
                    if (childDependency.LibraryRange.VersionRange == null
                        || (!currentDependencyGraphItem.IsCentrallyPinnedTransitivePackage && suppressions!.Contains(childLibraryDependencyIndex))
                        || (!isRootPackageReference && directPackageReferences?.Contains(childLibraryDependencyIndex) == true))
                    {
                        continue;
                    }

                    // See if a dependency with the same version and no suppressions has already been resolved.  If so, its children have already been added to the queue.
                    if (resolvedDependencyGraphItems.TryGetValue(childLibraryDependencyIndex, out ResolvedDependencyGraphItem? childResolvedDependencyGraphItem)
                        && childResolvedDependencyGraphItem.LibraryRangeIndex == childLibraryRangeIndex
                        && childResolvedDependencyGraphItem.Suppressions.Count == 1
                        && childResolvedDependencyGraphItem.Suppressions[0].Count == 0)
                    {
                        if (!childResolvedDependencyGraphItem.IsRootPackageReference)
                        {
                            // Keep track of the parents of this item
                            childResolvedDependencyGraphItem.Parents?.Add(currentDependencyGraphItem.LibraryRangeIndex);
                        }

                        continue;
                    }

                    HashSet<LibraryDependency>? runtimeDependencies = default;

                    // Evaluate the runtime dependencies if any
                    if (EvaluateRuntimeDependencies(ref childDependency, runtimeGraph, pair.RuntimeIdentifier, ref runtimeDependencies))
                    {
                        // EvaluateRuntimeDependencies() returns true if the version of the dependency was changed, which also changes the LibraryRangeIndex so that must be updated in the chosen item's array of library range indices.
                        chosenResolvedItem.SetRangeIndexForDependencyAt(i, _indexingTable.Index(childDependency.LibraryRange));
                    }

                    VersionRange? pinnedVersionRange = null;

                    // Determine if the package is transitively pinned
                    bool isCentrallyPinnedTransitiveDependency = isCentralPackageTransitivePinningEnabled
                        && isPackage
                        && pinnedPackageVersions?.TryGetValue(childLibraryDependencyIndex, out pinnedVersionRange) == true;

                    if (isCentrallyPinnedTransitiveDependency && !isRootPackageReference)
                    {
                        // If central transitive pinning is enabled the LibraryDependency must be recreated as not to mutate the in-memory copy
                        childDependency = new LibraryDependency(childDependency)
                        {
                            LibraryRange = new LibraryRange(childDependency.LibraryRange) { VersionRange = pinnedVersionRange },
                        };

                        // Since the version range could have changed, we must also update the LibraryRangeIndex
                        childLibraryRangeIndex = _indexingTable.Index(childDependency.LibraryRange);
                    }

                    // Create a DependencyGraphItem and add it to the queue for processing
                    DependencyGraphItem dependencyGraphItem = new()
                    {
                        LibraryDependency = childDependency,
                        LibraryDependencyIndex = childLibraryDependencyIndex,
                        LibraryRangeIndex = childLibraryRangeIndex,
                        Path = isCentrallyPinnedTransitiveDependency || isRootPackageReference ? _rootedDependencyPath : DependencyGraphItemIndexer.CreatePathToRef(currentDependencyGraphItem.Path, currentDependencyGraphItem.LibraryRangeIndex),
                        Parent = currentDependencyGraphItem.LibraryRangeIndex,
                        Suppressions = suppressions,
                        IsRootPackageReference = isRootPackageReference,
                        IsCentrallyPinnedTransitivePackage = isCentrallyPinnedTransitiveDependency,
                        RuntimeDependencies = runtimeDependencies,
                        FindLibraryTask = ResolverUtility.FindLibraryCachedAsync(
                            childDependency.LibraryRange,
                            pair.Framework,
                            runtimeIdentifier: string.IsNullOrWhiteSpace(pair.RuntimeIdentifier) ? null : pair.RuntimeIdentifier,
                            context,
                            token)
                    };

                    dependencyGraphItemQueue.Enqueue(dependencyGraphItem);
                }
            }

            return resolvedDependencyGraphItems;
        }

        /// <summary>
        /// Attempts to get a runtime graph for the specified target framework.
        /// </summary>
        /// <param name="localRepositories">A <see cref="List{T}" /> containing <see cref="NuGetv3LocalRepository" /> objects to find local packages in.</param>
        /// <param name="graphsByTargetFramework">A <see cref="Dictionary{TKey, TValue}" /> containing the dependency graphs by target framework.</param>
        /// <param name="frameworkRuntimePair">The <see cref="FrameworkRuntimePair" /> representing the current target framework and runtime identifier for which to get a runtime graph.</param>
        /// <param name="projectTargetFramework">The <see cref="TargetFrameworkInformation" /> containing target framework information for the project.</param>
        /// <param name="runtimeGraph">Receives the <see cref="RuntimeGraph" /> for the <see cref="FrameworkRuntimePair" /> if one was found, otherwise <see langword="null" />.</param>
        /// <returns><see langword="true" /> if a runtime was found, otherwise <see langword="false" />.</returns>
        private bool TryGetRuntimeGraph(List<NuGetv3LocalRepository> localRepositories, Dictionary<NuGetFramework, RestoreTargetGraph> graphsByTargetFramework, FrameworkRuntimePair frameworkRuntimePair, TargetFrameworkInformation? projectTargetFramework, [NotNullWhen(true)] out RuntimeGraph? runtimeGraph)
        {
            runtimeGraph = null;

            if (string.IsNullOrEmpty(frameworkRuntimePair.RuntimeIdentifier) || !graphsByTargetFramework.TryGetValue(frameworkRuntimePair.Framework, out RestoreTargetGraph? restoreTargetGraphForTargetFramework))
            {
                // If this pair has no runtime or there is no corresponding RID-less dependency graph for the target framework, there is no runtime graph
                return false;
            }

            // Get the path to the runtime graph for the project and target framework
            string? runtimeGraphPath = projectTargetFramework?.RuntimeIdentifierGraphPath;

            // Load the runtime graph for the project if one is specified
            RuntimeGraph? projectProviderRuntimeGraph = string.IsNullOrWhiteSpace(runtimeGraphPath) ? default : ProjectRestoreCommand.GetRuntimeGraph(runtimeGraphPath, _logger);

            // Gets a merged runtime graph for the project and target framework
            runtimeGraph = ProjectRestoreCommand.GetRuntimeGraph(restoreTargetGraphForTargetFramework, localRepositories, projectRuntimeGraph: projectProviderRuntimeGraph, _logger);

            return true;
        }
    }
}
