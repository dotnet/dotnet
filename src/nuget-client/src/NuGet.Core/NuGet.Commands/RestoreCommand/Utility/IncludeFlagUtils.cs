// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NuGet.DependencyResolver;
using NuGet.LibraryModel;
using NuGet.ProjectModel;

namespace NuGet.Commands
{
    internal static class IncludeFlagUtils
    {
        internal static Dictionary<string, LibraryIncludeFlags> FlattenDependencyTypes(
            Dictionary<RestoreTargetGraph, Dictionary<string, LibraryIncludeFlags>> includeFlagGraphs,
            PackageSpec project,
            RestoreTargetGraph graph)
        {
            Dictionary<string, LibraryIncludeFlags> flattenedFlags;
            if (!includeFlagGraphs.TryGetValue(graph, out flattenedFlags))
            {
                flattenedFlags = FlattenDependencyTypes(graph, project);
                includeFlagGraphs.Add(graph, flattenedFlags);
            }

            return flattenedFlags;
        }

        internal static Dictionary<string, LibraryIncludeFlags> FlattenDependencyTypes(
            RestoreTargetGraph targetGraph,
            PackageSpec spec)
        {
            var result = new Dictionary<string, LibraryIncludeFlags>(StringComparer.OrdinalIgnoreCase);

            // Walk dependencies
            FlattenDependencyTypesUnified(targetGraph, result);

            // Override flags for direct dependencies
            var directDependencies = spec.Dependencies.ToList();

            // Add dependencies defined under the framework node
            var specFramework = spec.GetTargetFramework(targetGraph.Framework);
            if (specFramework?.Dependencies != null)
            {
                directDependencies.AddRange(specFramework.Dependencies);
            }

            // Override the flags for direct dependencies. This lets the
            // user take control when needed.
            foreach (var dependency in directDependencies)
            {
                if (result.ContainsKey(dependency.Name))
                {
                    result[dependency.Name] = dependency.IncludeType;
                }
                else
                {
                    result.Add(dependency.Name, dependency.IncludeType);
                }
            }

            return result;
        }

        private static void FlattenDependencyTypesUnified(
            RestoreTargetGraph targetGraph,
            Dictionary<string, LibraryIncludeFlags> result)
        {
            var nodeQueue = new Queue<DependencyNode>(1);
            DependencyNode node = null;

            var unifiedNodes = new Dictionary<string, GraphItem<RemoteResolveResult>>(StringComparer.OrdinalIgnoreCase);

            // Create a look up table of id -> library
            // This should contain only packages and projects. If there is a project with the
            // same name as a package, use the project.
            // Projects take precedence over packages here to match the resolver behavior.
            foreach (var item in targetGraph.Flattened
                .OrderBy(lib => OrderType(lib)))
            {
                // Include flags only apply to packages and projects
                if (IsPackageOrProject(item) && !unifiedNodes.ContainsKey(item.Key.Name))
                {
                    unifiedNodes.Add(item.Key.Name, item);
                }
            }

            // Queue all direct references
            foreach (var graph in targetGraph.Graphs)
            {
                foreach (var root in graph.InnerNodes.Where(n => !n.Item.IsCentralTransitive))
                {
                    // Walk only the projects and packages
                    GraphItem<RemoteResolveResult> unifiedRoot;
                    if (unifiedNodes.TryGetValue(root.Key.Name, out unifiedRoot))
                    {
                        // Find the initial project -> dependency flags
                        var typeIntersection = GetDependencyType(graph, root);

                        node = new DependencyNode(root.Item, typeIntersection);

                        nodeQueue.Enqueue(node);
                    }
                }
            }

            // Walk the graph using BFS
            // During the walk find the intersection of the include type flags.
            // Dependencies can only have less flags the deeper down the graph
            // we move. Using this we can no-op when a node is encountered that
            // has already been assigned at least as many flags as the current
            // node. We can also assume that all dependencies under it are
            // already correct. If the existing node has less flags then the
            // walk must continue and all new flags found combined with the
            // existing ones.
            while (nodeQueue.Count > 0)
            {
                node = nodeQueue.Dequeue();
                var rootId = node.Item.Key.Name;

                // Combine results on the way up
                LibraryIncludeFlags currentTypes;
                if (result.TryGetValue(rootId, out currentTypes))
                {
                    if ((node.DependencyType & currentTypes) == node.DependencyType)
                    {
                        // Noop, this is done
                        // Circular dependencies end up stopping here also
                        continue;
                    }

                    // Combine the results
                    result[rootId] = (currentTypes | node.DependencyType);
                }
                else
                {
                    // Add the flags we have to the results
                    result.Add(rootId, node.DependencyType);
                }

                foreach (var dependency in node.Item.Data.Dependencies)
                {
                    if (dependency.ReferenceType != LibraryDependencyReferenceType.Direct)
                        continue;

                    // Any nodes that are not in unifiedNodes are types that should be ignored
                    // We should also ignore dependencies that are excluded to match the dependency
                    // resolution phase.
                    // Note that we cannot stop here if there are no flags since we still need to mark
                    // the child nodes as having no flags. SuppressParent=all is a special case.
                    GraphItem<RemoteResolveResult> child;
                    if (unifiedNodes.TryGetValue(dependency.Name, out child)
                        && dependency.SuppressParent != LibraryIncludeFlags.All)
                    {
                        // intersect the edges and remove any suppressParent flags
                        LibraryIncludeFlags typeIntersection =
                            (node.DependencyType
                            & dependency.IncludeType
                            & (~dependency.SuppressParent));

                        var childNode = new DependencyNode(child, typeIntersection);
                        nodeQueue.Enqueue(childNode);
                    }
                }
            }
        }

        /// <summary>
        /// Find the flags for a node.
        /// Include - Exclude - ParentExclude
        /// </summary>
        private static LibraryIncludeFlags GetDependencyType(
            GraphNode<RemoteResolveResult> parent,
            GraphNode<RemoteResolveResult> child)
        {
            var match = parent.Item.Data.Dependencies.FirstOrDefault(dependency =>
                dependency.Name.Equals(child.Key.Name, StringComparison.OrdinalIgnoreCase));

            Debug.Assert(match != null, "The graph contains a dependency that the node does not list");

            var flags = match.IncludeType;

            // Unless the root project is the grand parent here, the suppress flag should be applied directly to the
            // child since it has no effect on the parent.
            if (parent.OuterNode != null)
            {
                // Remove excluded flags from the include list
                flags &= ~match.SuppressParent;
            }

            return flags;
        }

        private static bool IsPackageOrProject(GraphItem<RemoteResolveResult> item)
        {
            return item.Key.Type == LibraryType.Package
                || item.Key.Type == LibraryType.Project
                || item.Key.Type == LibraryType.ExternalProject;
        }

        /// <summary>
        /// Prefer projects over packages
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private static int OrderType(GraphItem<RemoteResolveResult> item)
        {
            if (item.Key.Type == LibraryType.Project)
            {
                return 0;
            }
            else if (item.Key.Type == LibraryType.ExternalProject)
            {
                return 1;
            }
            else if (item.Key.Type == LibraryType.Package)
            {
                return 2;
            }

            return 5;
        }

        /// <summary>
        /// A simple node class to hold the incoming dependency edge during the graph walk.
        /// </summary>
        private class DependencyNode
        {
            public DependencyNode(GraphItem<RemoteResolveResult> item, LibraryIncludeFlags dependencyType)
            {
                DependencyType = dependencyType;
                Item = item;
            }

            /// <summary>
            /// Incoming edge
            /// </summary>
            public LibraryIncludeFlags DependencyType { get; }

            /// <summary>
            /// Node item
            /// </summary>
            public GraphItem<RemoteResolveResult> Item { get; }
        }
    }
}
