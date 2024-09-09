// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using NuGet.Client;
using NuGet.Common;
using NuGet.DependencyResolver;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.RuntimeModel;

namespace NuGet.Commands
{
    public class RestoreTargetGraph : IRestoreTargetGraph
    {
        /// <summary>
        /// Gets the runtime identifier used during the restore operation on this graph
        /// </summary>
        public string RuntimeIdentifier { get; }

        /// <summary>
        /// Gets the <see cref="NuGetFramework" /> used during the restore operation on this graph
        /// </summary>
        public NuGetFramework Framework { get; }

        /// <summary>
        /// Gets the <see cref="ManagedCodeConventions" /> used to resolve assets from packages in this graph
        /// </summary>
        public ManagedCodeConventions Conventions { get; }

        /// <summary>
        /// Gets the <see cref="RuntimeGraph" /> that defines runtimes and their relationships for this graph
        /// </summary>
        public RuntimeGraph RuntimeGraph { get; }

        /// <summary>
        /// Gets the resolved dependency graph
        /// </summary>
        public IEnumerable<GraphNode<RemoteResolveResult>> Graphs { get; }

        public ISet<RemoteMatch> Install { get; }

        public ISet<GraphItem<RemoteResolveResult>> Flattened { get; }

        public ISet<LibraryRange> Unresolved { get; }

        public bool InConflict { get; }

        public string Name { get; }

        public string TargetGraphName { get; }

        // TODO: Move conflicts to AnalyzeResult
        public IEnumerable<ResolverConflict> Conflicts { get; }

        public AnalyzeResult<RemoteResolveResult> AnalyzeResult { get; }

        public ISet<ResolvedDependencyKey> ResolvedDependencies { get; }

        internal RestoreTargetGraph(IEnumerable<ResolverConflict> conflicts,
                                   NuGetFramework framework,
                                   string runtimeIdentifier,
                                   RuntimeGraph runtimeGraph,
                                   IEnumerable<GraphNode<RemoteResolveResult>> graphs,
                                   ISet<RemoteMatch> install,
                                   ISet<GraphItem<RemoteResolveResult>> flattened,
                                   ISet<LibraryRange> unresolved,
                                   AnalyzeResult<RemoteResolveResult> analyzeResult,
                                   ISet<ResolvedDependencyKey> resolvedDependencies)
        {
            Conflicts = conflicts.ToArray();
            RuntimeIdentifier = runtimeIdentifier;
            RuntimeGraph = runtimeGraph;
            Framework = framework;
            Graphs = graphs;
            Name = FrameworkRuntimePair.GetName(Framework, RuntimeIdentifier);
            TargetGraphName = FrameworkRuntimePair.GetTargetGraphName(Framework, RuntimeIdentifier);


            Conventions = new ManagedCodeConventions(runtimeGraph);

            Install = install;
            Flattened = flattened;
            AnalyzeResult = analyzeResult;
            Unresolved = unresolved;
            ResolvedDependencies = resolvedDependencies;
        }

        public static RestoreTargetGraph Create(IEnumerable<GraphNode<RemoteResolveResult>> graphs, RemoteWalkContext context, ILogger logger, NuGetFramework framework)
        {
            return Create(RuntimeGraph.Empty, graphs, context, logger, framework, runtimeIdentifier: null);
        }

        public static RestoreTargetGraph Create(
            RuntimeGraph runtimeGraph,
            IEnumerable<GraphNode<RemoteResolveResult>> graphs,
            RemoteWalkContext context,
            ILogger log,
            NuGetFramework framework,
            string runtimeIdentifier)
        {
            var install = new HashSet<RemoteMatch>();
            var flattened = new HashSet<GraphItem<RemoteResolveResult>>();
            var unresolved = new HashSet<LibraryRange>();

            var conflicts = new Dictionary<string, HashSet<ResolverRequest>>();
            var analyzeResult = new AnalyzeResult<RemoteResolveResult>();
            var resolvedDependencies = new HashSet<ResolvedDependencyKey>();

            foreach (var graph in graphs)
            {
                var result = graph.Analyze();

                analyzeResult.Combine(result);
            }

            graphs.ForEach(node =>
                {
                    if (node == null || node.Key == null)
                    {
                        return;
                    }

                    if (node.Disposition != Disposition.Rejected)
                    {
                        if (node.Disposition == Disposition.Acceptable)
                        {
                            // This wasn't resolved. It's a conflict.
                            HashSet<ResolverRequest> ranges;
                            if (!conflicts.TryGetValue(node.Key.Name, out ranges))
                            {
                                ranges = new HashSet<ResolverRequest>();
                                conflicts[node.Key.Name] = ranges;
                            }

                            // OuterNode may be null if the project itself conflicts with a package name
                            var requestor = node.OuterNode == null ? node.Item.Key : node.OuterNode.Item.Key;

                            ranges.Add(new ResolverRequest(requestor, node.Key));
                        }

                        if (node?.Item?.Key?.Type == LibraryType.Unresolved)
                        {
                            if (node.Key.VersionRange != null)
                            {
                                unresolved.Add(node.Key);
                            }

                            return;
                        }

                        // Don't add rejected nodes since we only want to write reduced nodes
                        // to the lock file
                        flattened.Add(node.Item);
                    }

                    if (node?.OuterNode != null && node.Item.Key.Type != LibraryType.Unresolved)
                    {
                        var dependencyKey = new ResolvedDependencyKey(
                            parent: node.OuterNode.Item.Key,
                            range: node.Key.VersionRange,
                            child: node.Item.Key);

                        resolvedDependencies.Add(dependencyKey);
                    }

                    // If the package came from a remote library provider, it needs to be installed locally
                    // Rejected nodes are included here to avoid downloading them from remote sources
                    // each time the lock file is generated.
                    var isRemote = context.RemoteLibraryProviders.Contains(node.Item.Data.Match.Provider);
                    if (isRemote)
                    {
                        install.Add(node.Item.Data.Match);
                    }
                });

            return new RestoreTargetGraph(
                conflicts.Select(p => new ResolverConflict(p.Key, p.Value)),
                framework,
                runtimeIdentifier,
                runtimeGraph,
                graphs,
                install,
                flattened,
                unresolved,
                analyzeResult,
                resolvedDependencies);
        }
    }
}
