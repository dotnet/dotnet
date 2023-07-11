// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using NuGet.LibraryModel;
using NuGet.Shared;
using NuGet.Versioning;

namespace NuGet.DependencyResolver
{
    public static class GraphOperations
    {
        private const string NodeArrow = " -> ";

        private enum WalkState
        {
            Walking,
            Rejected,
            Ambiguous
        }

        public static AnalyzeResult<RemoteResolveResult> Analyze(this GraphNode<RemoteResolveResult> root)
        {
            var result = new AnalyzeResult<RemoteResolveResult>();

            root.CheckCycleAndNearestWins(result.Downgrades, result.Cycles);
            root.TryResolveConflicts(result.VersionConflicts);

            // Remove all downgrades that didn't result in selecting the node we actually downgraded to
            result.Downgrades.RemoveAll(d => !IsRelevantDowngrade(d));

            return result;
        }

        /// <summary>
        /// A downgrade is relevant if the node itself was `Accepted`.
        /// A node that itself wasn't `Accepted`, or has a parent that wasn't accepted is not relevant.
        /// </summary>
        /// <param name="d">Downgrade result to analyze</param>
        /// <returns>Whether the downgrade is relevant.</returns>
        private static bool IsRelevantDowngrade(DowngradeResult<RemoteResolveResult> d)
        {
            return d.DowngradedTo.Disposition == Disposition.Accepted && AreAllParentsAccepted(d);

            static bool AreAllParentsAccepted(DowngradeResult<RemoteResolveResult> d)
            {
                GraphNode<RemoteResolveResult> resultToCheck = d.DowngradedFrom.OuterNode;

                while (resultToCheck != null)
                {
                    if (resultToCheck.Disposition != Disposition.Accepted)
                    {
                        return false;
                    }
                    resultToCheck = resultToCheck.OuterNode;
                }
                return true;
            }
        }


        private static void CheckCycleAndNearestWins(
            this GraphNode<RemoteResolveResult> root,
            List<DowngradeResult<RemoteResolveResult>> downgrades,
            List<GraphNode<RemoteResolveResult>> cycles)
        {
            var workingDowngrades = RentDowngradesDictionary();

            root.ForEach((node, context) => WalkTreeCheckCycleAndNearestWins(context, node), CreateState(cycles, workingDowngrades));

            // Increase List size for items to be added, if too small
            var requiredCapacity = downgrades.Count + workingDowngrades.Count;
            if (downgrades.Capacity < requiredCapacity)
            {
                downgrades.Capacity = requiredCapacity;
            }
            foreach (var p in workingDowngrades)
            {
                downgrades.Add(new DowngradeResult<RemoteResolveResult>
                {
                    DowngradedFrom = p.Key,
                    DowngradedTo = p.Value
                });
            }

            ReleaseDowngradesDictionary(workingDowngrades);
        }

        private static void WalkTreeCheckCycleAndNearestWins(CyclesAndDowngrades context, GraphNode<RemoteResolveResult> node)
        {
            // Cycle:
            //
            // A -> B -> A (cycle)
            //
            // Downgrade:
            //
            // A -> B -> C -> D 2.0 (downgrade)
            //        -> D 1.0
            //
            // Potential downgrades that turns out to not be downgrades:
            //
            // 1. This should never happen in practice since B would have never been valid to begin with.
            //
            //    A -> B -> C -> D 2.0
            //           -> D 1.0
            //      -> D 2.0
            //
            // 2. This occurs if none of the sources have version C 1.0 so C 1.0 is bumped up to C 2.0.
            //
            //   A -> B -> C 2.0
            //     -> C 1.0

            var cycles = context.Cycles;
            var workingDowngrades = context.Downgrades;

            if (node.Disposition == Disposition.Cycle)
            {
                cycles.Add(node);

                // Remove this node from the tree so the nothing else evaluates this.
                // This is ok since we have a parent pointer and we can still print the path
                node.OuterNode.InnerNodes.Remove(node);

                return;
            }

            if (node.Disposition != Disposition.PotentiallyDowngraded)
            {
                return;
            }

            // REVIEW: This could probably be done in a single pass where we keep track
            // of what is nearer as we walk down the graph (BFS)
            for (var n = node.OuterNode; n != null; n = n.OuterNode)
            {
                var innerNodes = n.InnerNodes;
                var count = innerNodes.Count;
                for (var i = 0; i < count; i++)
                {
                    var sideNode = innerNodes[i];
                    if (sideNode != node && StringComparer.OrdinalIgnoreCase.Equals(sideNode.Key.Name, node.Key.Name))
                    {
                        // Nodes that have no version range should be ignored as potential downgrades e.g. framework reference
                        if (sideNode.Key.VersionRange != null &&
                            node.Key.VersionRange != null &&
                            !RemoteDependencyWalker.IsGreaterThanOrEqualTo(sideNode.Key.VersionRange, node.Key.VersionRange))
                        {
                            // Is the resolved version actually within node's version range? This happen if there
                            // was a different request for a lower version of the library than this version range
                            // allows but no matching library was found, so the library is bumped up into this
                            // version range.
                            var resolvedVersion = sideNode?.Item?.Data?.Match?.Library?.Version;
                            if (resolvedVersion != null && node.Key.VersionRange.Satisfies(resolvedVersion))
                            {
                                continue;
                            }

                            workingDowngrades[node] = sideNode;
                        }
                        else
                        {
                            workingDowngrades.Remove(node);
                        }
                    }
                }
            }

            // Remove this node from the tree so the nothing else evaluates this.
            // This is ok since we have a parent pointer and we can still print the path
            node.OuterNode.InnerNodes.Remove(node);
        }

        /// <summary>
        /// A 1.0.0 -> B 1.0.0 -> C 2.0.0
        /// </summary>
        public static string GetPath<TItem>(this GraphNode<TItem> node)
        {
            var nodeStrings = new Stack<string>();
            var current = node;

            while (current != null)
            {
                nodeStrings.Push(current.GetIdAndVersionOrRange());
                current = current.OuterNode;
            }

            return string.Join(NodeArrow, nodeStrings);
        }

        /// <summary>
        /// A 1.0.0 -> B 1.0.0 -> C (= 2.0.0)
        /// </summary>
        public static string GetPathWithLastRange<TItem>(this GraphNode<TItem> node)
        {
            var nodeStrings = new Stack<string>();
            var current = node;

            while (current != null)
            {
                // Display the range for the last node, show the version of all parents.
                var nodeString = nodeStrings.Count == 0 ? current.GetIdAndRange() : current.GetIdAndVersionOrRange();
                nodeStrings.Push(nodeString);
                current = current.OuterNode;
            }

            return string.Join(NodeArrow, nodeStrings);
        }

        // A helper to navigate the graph nodes
        public static GraphNode<TItem> Path<TItem>(this GraphNode<TItem> node, params string[] path)
        {
            foreach (var item in path)
            {
                GraphNode<TItem> childNode = null;
                var innerNodes = node.InnerNodes;
                var count = innerNodes.Count;
                for (var i = 0; i < count; i++)
                {
                    var candidateNode = innerNodes[i];
                    if (StringComparer.OrdinalIgnoreCase.Equals(candidateNode.Key.Name, item))
                    {
                        childNode = candidateNode;
                        break;
                    }
                }

                if (childNode == null)
                {
                    return null;
                }

                node = childNode;
            }

            return node;
        }

        /// <summary>
        /// Prints the id and version constraint for a node.
        /// </summary>
        /// <remarks>Projects will not display a range.</remarks>
        public static string GetIdAndRange<TItem>(this GraphNode<TItem> node)
        {
            var id = node.GetId();

            // Ignore constraints for projects, they are not useful since
            // only one instance of the id may exist in the graph.
            if (node.IsPackage())
            {
                // Remove floating versions
                var range = node.GetVersionRange().ToNonSnapshotRange();

                // Print the version range if it has an upper or lower bound to display.
                if (range.HasLowerBound || range.HasUpperBound)
                {
                    return $"{id} {range.PrettyPrint()}";
                }
            }

            return id;
        }

        /// <summary>
        /// Prints the id and version of a node. If the version does not exist use the range.
        /// </summary>
        /// <remarks>Projects will not display a version or range.</remarks>
        public static string GetIdAndVersionOrRange<TItem>(this GraphNode<TItem> node)
        {
            var id = node.GetId();

            // Ignore versions for projects, they are not useful since
            // only one instance of the id may exist in the graph.
            if (node.IsPackage())
            {
                var version = node.GetVersionOrDefault();

                // Print the version if it exists, otherwise use the id.
                if (version != null)
                {
                    return $"{id} {version.ToNormalizedString()}";
                }
                else
                {
                    // The node was unresolved, use the range instead.
                    return node.GetIdAndRange();
                }
            }

            return id;
        }

        /// <summary>
        /// Id of the node.
        /// </summary>
        public static string GetId<TItem>(this GraphNode<TItem> node)
        {
            // Prefer the name of the resolved item, this will have
            // the correct casing. If it was not resolved use the parent
            // dependency for the name.
            return node.Item?.Key?.Name ?? node.Key.Name;
        }

        /// <summary>
        /// Version of the resolved node version if it exists.
        /// </summary>
        public static NuGetVersion GetVersionOrDefault<TItem>(this GraphNode<TItem> node)
        {
            // Prefer the name of the resolved item, this will have
            // the correct casing. If it was not resolved use the parent
            // dependency for the name.
            return node.Item?.Key?.Version;
        }

        /// <summary>
        /// Dependency range for the node.
        /// </summary>
        /// <remarks>Defaults to All</remarks>
        public static VersionRange GetVersionRange<TItem>(this GraphNode<TItem> node)
        {
            return node.Key.VersionRange ?? VersionRange.All;
        }

        /// <summary>
        /// True if the node is resolved to a package or allows a package if unresolved.
        /// </summary>
        public static bool IsPackage<TItem>(this GraphNode<TItem> node)
        {
            if ((node.Item?.Key?.Type == LibraryType.Package) == true)
            {
                // The resolved item is a package.
                return true;
            }

            // The node was unresolved but allows packages.
            return node.Key.TypeConstraintAllowsAnyOf(LibraryDependencyTarget.Package);
        }

        private static bool TryResolveConflicts<TItem>(this GraphNode<TItem> root, List<VersionConflictResult<TItem>> versionConflicts)
        {
            // now we walk the tree as often as it takes to determine
            // which paths are accepted or rejected, based on conflicts occuring
            // between cousin packages

            var acceptedLibraries = Cache<TItem>.RentDictionary();

            var patience = 1000;
            var incomplete = true;

            var tracker = Cache<TItem>.RentTracker();
            Func<GraphNode<TItem>, bool> skipNode = null;

            var centralTransitiveNodes = root.InnerNodes.Where(n => n.Item.IsCentralTransitive).ToList();
            var hasCentralTransitiveDependencies = centralTransitiveNodes.Count > 0;
            if (hasCentralTransitiveDependencies)
            {
                skipNode = (node) => { return node.Item.IsCentralTransitive; };
            }

            while (incomplete && --patience != 0)
            {
                // Create a picture of what has not been rejected yet
                root.ForEach(true, (node, state, context) => WalkTreeRejectNodesOfRejectedNodes(state, node, context), tracker, skipNode);

                if (hasCentralTransitiveDependencies)
                {
                    // Some of the central transitive nodes may be rejected now because their parents were rejected
                    // Reject them accordingly
                    root.RejectCentralTransitiveBecauseOfRejectedParents(tracker, centralTransitiveNodes);
                }

                // Inform tracker of ambiguity beneath nodes that are not resolved yet
                root.ForEach(WalkState.Walking, (node, state, context) => WalkTreeMarkAmbiguousNodes(node, state, context), tracker);

                if (hasCentralTransitiveDependencies)
                {
                    DetectAndMarkAmbiguousCentralTransitiveDependencies(tracker, centralTransitiveNodes);
                }

                root.ForEach(true, (node, state, context) => WalkTreeAcceptOrRejectNodes(context, state, node), CreateState(tracker, acceptedLibraries));

                incomplete = root.ForEachGlobalState(false, (node, state) => state || node.Disposition == Disposition.Acceptable);

                tracker.Clear();
            }

            Cache<TItem>.ReleaseTracker(tracker);

            root.ForEach((node, context) => WalkTreeDectectConflicts(node, context), CreateState(versionConflicts, acceptedLibraries));

            Cache<TItem>.ReleaseDictionary(acceptedLibraries);

            return !incomplete;
        }

        private static void WalkTreeDectectConflicts<TItem>(GraphNode<TItem> node, ConflictsAndAccepted<TItem> context)
        {
            if (node.Disposition != Disposition.Accepted)
            {
                return;
            }

            var versionConflicts = context.VersionConflicts;
            var acceptedLibraries = context.AcceptedLibraries;

            // For all accepted nodes, find dependencies that aren't satisfied by the version
            // of the package that we have selected
            var innerNodes = node.InnerNodes;
            var count = innerNodes.Count;
            for (var i = 0; i < count; i++)
            {
                var childNode = innerNodes[i];
                GraphNode<TItem> acceptedNode;
                if (acceptedLibraries.TryGetValue(childNode.Key.Name, out acceptedNode) &&
                    childNode != acceptedNode &&
                    childNode.Key.VersionRange != null &&
                    acceptedNode.Item.Key.Version != null)
                {
                    var acceptedType = LibraryDependencyTargetUtils.Parse(acceptedNode.Item.Key.Type);
                    var childType = childNode.Key.TypeConstraint;

                    // Skip the check if a project reference override a package dependency
                    // Check the type constraints, if there is any overlap check for conflict
                    if ((acceptedType & (LibraryDependencyTarget.Project | LibraryDependencyTarget.ExternalProject)) == LibraryDependencyTarget.None
                        && (childType & acceptedType) != LibraryDependencyTarget.None)
                    {
                        var versionRange = childNode.Key.VersionRange;
                        var checkVersion = acceptedNode.Item.Key.Version;

                        if (!versionRange.Satisfies(checkVersion))
                        {
                            versionConflicts.Add(new VersionConflictResult<TItem>
                            {
                                Selected = acceptedNode,
                                Conflicting = childNode
                            });
                        }
                    }
                }
            }
        }

        private static WalkState WalkTreeMarkAmbiguousNodes<TItem>(GraphNode<TItem> node, WalkState state, Tracker<TItem> context)
        {
            // between:
            // a1->b1->d1->x1
            // a1->c1->d2->z1
            // first attempt
            //  d1/d2 are considered disputed
            //  x1 and z1 are considered ambiguous
            //  d1 is rejected
            // second attempt
            //  d1 is rejected, d2 is accepted
            //  x1 is no longer seen, and z1 is not ambiguous
            //  z1 is accepted
            if (node.Disposition == Disposition.Rejected)
            {
                return WalkState.Rejected;
            }

            if (state == WalkState.Walking
                && context.IsDisputed(node.Item))
            {
                return WalkState.Ambiguous;
            }

            if (state == WalkState.Ambiguous)
            {
                context.MarkAmbiguous(node.Item);
            }

            return state;
        }

        private static bool WalkTreeRejectNodesOfRejectedNodes<TItem>(bool state, GraphNode<TItem> node, Tracker<TItem> context)
        {
            if (!state || node.Disposition == Disposition.Rejected)
            {
                // Mark all nodes as rejected if they aren't already marked
                node.Disposition = Disposition.Rejected;
                return false;
            }

            context.Track(node.Item);
            return true;
        }

        private static bool WalkTreeAcceptOrRejectNodes<TItem>(TrackerAndAccepted<TItem> context, bool state, GraphNode<TItem> node)
        {
            var tracker = context.Tracker;
            var acceptedLibraries = context.AcceptedLibraries;

            if (!state
                || node.Disposition == Disposition.Rejected)
            {
                return false;
            }

            if (tracker.IsAmbiguous(node.Item))
            {
                return false;
            }

            if (node.Disposition == Disposition.Acceptable)
            {
                if (tracker.IsBestVersion(node.Item))
                {
                    node.Disposition = Disposition.Accepted;
                    acceptedLibraries[node.Key.Name] = node;
                }
                else
                {
                    node.Disposition = Disposition.Rejected;
                }
            }

            return node.Disposition == Disposition.Accepted;
        }

        private static TState ForEachGlobalState<TItem, TState>(this GraphNode<TItem> root, TState state, Func<GraphNode<TItem>, TState, TState> visitor, Func<GraphNode<TItem>, bool> skipNode = null)
        {
            var queue = Cache<TItem>.RentQueue();
            // breadth-first walk of Node tree

            queue.Enqueue(root);
            while (queue.Count > 0)
            {
                var work = queue.Dequeue();
                if (skipNode == null || !skipNode(work))
                {
                    state = visitor(work, state);

                    AddInnerNodesToQueue(work.InnerNodes, queue);
                }
            }

            Cache<TItem>.ReleaseQueue(queue);

            return state;
        }

        private static void ForEach<TItem, TState, TContext>(this GraphNode<TItem> root, TState state, Func<GraphNode<TItem>, TState, TContext, TState> visitor, TContext context, Func<GraphNode<TItem>, bool> skipNode = null)
        {
            var queue = Cache<TItem, TState>.RentQueue();

            // breadth-first walk of Node tree
            queue.Enqueue(NodeWithState.Create(root, state));
            while (queue.Count > 0)
            {
                var work = queue.Dequeue();
                if (skipNode == null || !skipNode(work.Node))
                {
                    state = visitor(work.Node, work.State, context);

                    AddInnerNodesToQueue(work.Node.InnerNodes, queue, state);
                }
            }

            Cache<TItem, TState>.ReleaseQueue(queue);
        }

        public static void ForEach<TItem>(this IEnumerable<GraphNode<TItem>> roots, Action<GraphNode<TItem>> visitor)
        {
            var queue = Cache<TItem>.RentQueue();

            var graphNodes = roots.AsList();
            var count = graphNodes.Count;
            for (var g = 0; g < count; g++)
            {
                queue.Enqueue(graphNodes[g]);
                while (queue.Count > 0)
                {
                    var node = queue.Dequeue();
                    visitor(node);

                    AddInnerNodesToQueue(node.InnerNodes, queue);
                }
            }

            Cache<TItem>.ReleaseQueue(queue);
        }

        private static void ForEach<TItem>(this GraphNode<TItem> root, Action<GraphNode<TItem>> visitor, Func<GraphNode<TItem>, bool> skipNode)
        {
            var queue = Cache<TItem>.RentQueue();

            // breadth-first walk of Node tree, no state
            queue.Enqueue(root);
            while (queue.Count > 0)
            {
                var node = queue.Dequeue();
                if (skipNode == null || !skipNode(node))
                {
                    visitor(node);

                    AddInnerNodesToQueue(node.InnerNodes, queue);
                }
            }

            Cache<TItem>.ReleaseQueue(queue);
        }

        public static void ForEach<TItem>(this GraphNode<TItem> root, Action<GraphNode<TItem>> visitor)
        {
            ForEach(root, visitor, skipNode: null);
        }

        private static void ForEach<TItem, TContext>(this GraphNode<TItem> root, Action<GraphNode<TItem>, TContext> visitor, TContext context, Func<GraphNode<TItem>, bool> skipNode)
        {
            var queue = Cache<TItem>.RentQueue();

            // breadth-first walk of Node tree, no state
            queue.Enqueue(root);
            while (queue.Count > 0)
            {
                var node = queue.Dequeue();
                if (skipNode == null || !skipNode(node))
                {
                    visitor(node, context);

                    AddInnerNodesToQueue(node.InnerNodes, queue);
                }
            }

            Cache<TItem>.ReleaseQueue(queue);
        }

        public static void ForEach<TItem, TContext>(this GraphNode<TItem> root, Action<GraphNode<TItem>, TContext> visitor, TContext context)
        {
            ForEach(root, visitor, context, skipNode: null);
        }

        private static void AddInnerNodesToQueue<TItem, TState>(IList<GraphNode<TItem>> innerNodes, Queue<NodeWithState<TItem, TState>> queue, TState innerState)
        {
            var count = innerNodes.Count;
            for (var i = 0; i < count; i++)
            {
                var innerNode = innerNodes[i];
                queue.Enqueue(NodeWithState.Create(innerNode, innerState));
            }
        }

        private static void AddInnerNodesToQueue<TItem>(IList<GraphNode<TItem>> innerNodes, Queue<GraphNode<TItem>> queue)
        {
            var count = innerNodes.Count;
            for (var i = 0; i < count; i++)
            {
                var innerNode = innerNodes[i];
                queue.Enqueue(innerNode);
            }
        }

        [ThreadStatic]
        private static Dictionary<GraphNode<RemoteResolveResult>, GraphNode<RemoteResolveResult>> _tempDowngrades;

        public static Dictionary<GraphNode<RemoteResolveResult>, GraphNode<RemoteResolveResult>> RentDowngradesDictionary()
        {
            var dictionary = _tempDowngrades;
            if (dictionary != null)
            {
                _tempDowngrades = null;
                return dictionary;
            }

            return new Dictionary<GraphNode<RemoteResolveResult>, GraphNode<RemoteResolveResult>>();
        }

        public static void ReleaseDowngradesDictionary(Dictionary<GraphNode<RemoteResolveResult>, GraphNode<RemoteResolveResult>> dictionary)
        {
            if (_tempDowngrades == null)
            {
                dictionary.Clear();
                _tempDowngrades = dictionary;
            }
        }

        private static class Cache<TItem, TState>
        {
            [ThreadStatic]
            private static Queue<NodeWithState<TItem, TState>> _queue;


            public static Queue<NodeWithState<TItem, TState>> RentQueue()
            {
                var queue = _queue;
                if (queue != null)
                {
                    _queue = null;
                    return queue;
                }

                return new Queue<NodeWithState<TItem, TState>>();
            }

            public static void ReleaseQueue(Queue<NodeWithState<TItem, TState>> queue)
            {
                if (_queue == null)
                {
                    queue.Clear();
                    _queue = queue;
                }
            }

        }

        private static class Cache<TItem>
        {
            [ThreadStatic]
            private static Queue<GraphNode<TItem>> _queue;
            [ThreadStatic]
            private static Dictionary<string, GraphNode<TItem>> _dictionary;
            [ThreadStatic]
            private static Tracker<TItem> _tracker;

            public static Queue<GraphNode<TItem>> RentQueue()
            {
                var queue = _queue;
                if (queue != null)
                {
                    _queue = null;
                    return queue;
                }

                return new Queue<GraphNode<TItem>>();
            }

            public static void ReleaseQueue(Queue<GraphNode<TItem>> queue)
            {
                if (_queue == null)
                {
                    queue.Clear();
                    _queue = queue;
                }
            }

            public static Tracker<TItem> RentTracker()
            {
                var tracker = _tracker;
                if (tracker != null)
                {
                    _tracker = null;
                    return tracker;
                }

                return new Tracker<TItem>();
            }

            public static void ReleaseTracker(Tracker<TItem> tracker)
            {
                if (_tracker == null)
                {
                    tracker.Clear();
                    _tracker = tracker;
                }
            }

            public static Dictionary<string, GraphNode<TItem>> RentDictionary()
            {
                var dictionary = _dictionary;
                if (dictionary != null)
                {
                    _dictionary = null;
                    return dictionary;
                }

                return new Dictionary<string, GraphNode<TItem>>(StringComparer.OrdinalIgnoreCase);
            }

            public static void ReleaseDictionary(Dictionary<string, GraphNode<TItem>> dictionary)
            {
                if (_dictionary == null)
                {
                    dictionary.Clear();
                    _dictionary = dictionary;
                }
            }
        }

        private struct NodeWithState<TItem, TState>
        {
            public GraphNode<TItem> Node;
            public TState State;
        }

        private static class NodeWithState
        {
            public static NodeWithState<TItem, TState> Create<TItem, TState>(GraphNode<TItem> node, TState state)
            {
                return new NodeWithState<TItem, TState>
                {
                    Node = node,
                    State = state
                };
            }
        }

        private struct ConflictsAndAccepted<TItem>
        {
            public List<VersionConflictResult<TItem>> VersionConflicts;
            public Dictionary<string, GraphNode<TItem>> AcceptedLibraries;
        }
        private static ConflictsAndAccepted<TItem> CreateState<TItem>(List<VersionConflictResult<TItem>> versionConflicts, Dictionary<string, GraphNode<TItem>> acceptedLibraries)
        {
            return new ConflictsAndAccepted<TItem>
            {
                VersionConflicts = versionConflicts,
                AcceptedLibraries = acceptedLibraries
            };
        }

        private struct TrackerAndAccepted<TItem>
        {
            public Tracker<TItem> Tracker;
            public Dictionary<string, GraphNode<TItem>> AcceptedLibraries;
        }

        private static TrackerAndAccepted<TItem> CreateState<TItem>(Tracker<TItem> tracker, Dictionary<string, GraphNode<TItem>> acceptedLibraries)
        {
            return new TrackerAndAccepted<TItem>
            {
                Tracker = tracker,
                AcceptedLibraries = acceptedLibraries
            };
        }

        private struct CyclesAndDowngrades
        {
            public List<GraphNode<RemoteResolveResult>> Cycles;
            public Dictionary<GraphNode<RemoteResolveResult>, GraphNode<RemoteResolveResult>> Downgrades;
        }

        private static CyclesAndDowngrades CreateState(List<GraphNode<RemoteResolveResult>> cycles, Dictionary<GraphNode<RemoteResolveResult>, GraphNode<RemoteResolveResult>> downgrades)
        {
            return new CyclesAndDowngrades
            {
                Cycles = cycles,
                Downgrades = downgrades
            };
        }

        private static void DetectAndMarkAmbiguousCentralTransitiveDependencies<TItem>(Tracker<TItem> tracker, List<GraphNode<TItem>> centralTransitiveNodes)
        {
            // if a central transitive node has all parents disputed or ambiguous mark it and its children ambiguous
            int ctdCount = centralTransitiveNodes.Count;
            while (true)
            {
                bool nodeMarkedAmbiguous = false;
                for (int i = 0; i < ctdCount; i++)
                {
                    if (centralTransitiveNodes[i].Disposition == Disposition.Acceptable)
                    {
                        bool allParentsAreDisputedOrAmbiguous = !centralTransitiveNodes[i].ParentNodes
                            .Any(p => p.Disposition != Disposition.Rejected && !(tracker.IsDisputed(p.Item) || tracker.IsAmbiguous(p.Item)));

                        if (allParentsAreDisputedOrAmbiguous && !tracker.IsAmbiguous(centralTransitiveNodes[i].Item))
                        {
                            nodeMarkedAmbiguous = true;

                            // children of ambiguous nodes were already marked as ambiguous, skip them
                            centralTransitiveNodes[i].ForEach(x => tracker.MarkAmbiguous(x.Item), pn => tracker.IsAmbiguous(pn.Item));
                        }
                    }
                }

                // Some node were marked ambiguous, thus we need another run to check if nodes previously not marked ambiguous should be marked ambiguous this time.
                if (!nodeMarkedAmbiguous)
                    break;
            };
        }

        private static void RejectCentralTransitiveBecauseOfRejectedParents<TItem>(this GraphNode<TItem> root, Tracker<TItem> tracker, List<GraphNode<TItem>> centralTransitiveNodes)
        {
            HashSet<GraphNode<TItem>> internalContext = new HashSet<GraphNode<TItem>>();

            // reject nodes of rejected nodes for the central transitive nodes and track nodes that were not yet rejected
            // as more nodes can be rejected do not track the nodes until all the rejects are completed
            int ctdCount = centralTransitiveNodes.Count;
            for (int i = 0; i < ctdCount; i++)
            {
                centralTransitiveNodes[i].ForEach(root.Disposition != Disposition.Rejected, (node, state, context) => WalkTreeRejectNodesOfRejectedNodes(state, node, context), internalContext);
            }

            // If a node has its parents rejected, reject the node and its children
            // Need to do this in a loop because more nodes can be rejected as their parents become rejected
            bool pendingRejections = true;
            while (pendingRejections)
            {
                pendingRejections = false;
                for (int i = 0; i < ctdCount; i++)
                {
                    if (centralTransitiveNodes[i].Disposition == Disposition.Acceptable && centralTransitiveNodes[i].AreAllParentsRejected())
                    {
                        centralTransitiveNodes[i].ForEach(n => n.Disposition = Disposition.Rejected);
                        pendingRejections = true;
                    }
                }
            }

            // now add all the not rejected nodes to the tracker
            foreach (var node in internalContext)
            {
                if (node.Disposition != Disposition.Rejected)
                {
                    tracker.Track(node.Item);
                }
            }
        }

        private static bool WalkTreeRejectNodesOfRejectedNodes<TItem>(bool state, GraphNode<TItem> node, HashSet<GraphNode<TItem>> context)
        {
            if (!state || node.Disposition == Disposition.Rejected)
            {
                // Mark all nodes as rejected if they aren't already marked
                node.Disposition = Disposition.Rejected;
                return false;
            }
            context.Add(node);
            return true;
        }

        // Box Drawing Unicode characters:
        // http://www.unicode.org/charts/PDF/U2500.pdf
        private const char LIGHT_HORIZONTAL = '\u2500';
        private const char LIGHT_VERTICAL_AND_RIGHT = '\u251C';

        [Conditional("DEBUG")]
        public static void Dump<TItem>(this GraphNode<TItem> root, Action<string> write)
        {
            DumpNode(root, write, level: 0);
            DumpChildren(root, write, level: 0);
        }

        private static void DumpChildren<TItem>(GraphNode<TItem> root, Action<string> write, int level)
        {
            var children = root.InnerNodes;
            for (var i = 0; i < children.Count; i++)
            {
                DumpNode(children[i], write, level + 1);
                DumpChildren(children[i], write, level + 1);
            }
        }

        private static void DumpNode<TItem>(GraphNode<TItem> node, Action<string> write, int level)
        {
            var output = new StringBuilder();
            if (level > 0)
            {
                output.Append(LIGHT_VERTICAL_AND_RIGHT);
                output.Append(new string(LIGHT_HORIZONTAL, level));
                output.Append(" ");
            }

            output.Append($"{node.GetIdAndRange()} ({node.Disposition})");

            if (node.Item != null
                && node.Item.Key != null)
            {
                output.Append($" => {node.Item.Key.ToString()}");
            }
            else
            {
                output.Append($" => ???");
            }
            write(output.ToString());
        }
    }
}
