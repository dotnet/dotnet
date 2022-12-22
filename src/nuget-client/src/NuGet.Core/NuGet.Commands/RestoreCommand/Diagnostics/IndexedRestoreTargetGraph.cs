// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using NuGet.DependencyResolver;
using NuGet.LibraryModel;

namespace NuGet.Commands
{
    /// <summary>
    /// Contains a RestoreTargetGraph with the flattened graph indexed on id.
    /// </summary>
    public class IndexedRestoreTargetGraph
    {
        private readonly Dictionary<string, GraphItem<RemoteResolveResult>> _lookup
            = new Dictionary<string, GraphItem<RemoteResolveResult>>(StringComparer.OrdinalIgnoreCase);

        private readonly HashSet<string> _idsWithErrors = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// RestoreTargetGraph
        /// </summary>
        public IRestoreTargetGraph Graph { get; }

        private IndexedRestoreTargetGraph(IRestoreTargetGraph graph)
        {
            Graph = graph ?? throw new ArgumentNullException(nameof(graph));

            // Id -> Node
            foreach (var node in graph.Flattened)
            {
                var id = node.Key.Name;
                if (!_lookup.ContainsKey(id))
                {
                    _lookup.Add(id, node);
                }
            }

            // Index NU1107 version conflict errors
            var versionConflicts = graph.AnalyzeResult?.VersionConflicts;
            if (versionConflicts != null)
            {
                foreach (var node in versionConflicts)
                {
                    // Selected and conflicting will have the same name.
                    _idsWithErrors.Add(node.Conflicting.Key.Name);
                }
            }

            // Index cycles
            var cycles = graph.AnalyzeResult?.Cycles;
            if (cycles != null)
            {
                foreach (var node in cycles)
                {
                    _idsWithErrors.Add(node.Key.Name);
                }
            }
        }

        public static IndexedRestoreTargetGraph Create(IRestoreTargetGraph graph)
        {
            return new IndexedRestoreTargetGraph(graph);
        }

        /// <summary>
        /// Returns the item or null if the id does not exist.
        /// </summary>
        public GraphItem<RemoteResolveResult> GetItemById(string id)
        {
            if (_lookup.TryGetValue(id, out var node))
            {
                return node;
            }

            return null;
        }

        /// <summary>
        /// Returns the item or null if the id does not exist or does not match the type.
        /// </summary>
        public GraphItem<RemoteResolveResult> GetItemById(string id, LibraryType libraryType)
        {
            if (_lookup.TryGetValue(id, out var node)
                && node.Key.Type == libraryType)
            {
                return node;
            }

            return null;
        }

        /// <summary>
        /// True if an id has a conflict or cycle error associated with it.
        /// </summary>
        public bool HasErrors(string id)
        {
            return _idsWithErrors.Contains(id);
        }
    }
}
