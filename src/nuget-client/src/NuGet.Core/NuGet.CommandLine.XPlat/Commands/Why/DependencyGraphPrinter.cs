// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using NuGet.Shared;
using NuGet.Versioning;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace NuGet.CommandLine.XPlat.Commands.Why
{
    internal static class DependencyGraphPrinter
    {
        private static readonly Color TargetPackageColor = Color.Cyan;

        /// <summary>
        /// Prints the dependency graphs for all target frameworks.
        /// </summary>
        /// <param name="dependencyGraphPerFramework">A dictionary mapping target frameworks to their dependency graphs.</param>
        /// <param name="targetPackage">The package we want the dependency paths for.</param>
        /// <param name="logger"></param>
        public static void PrintAllDependencyGraphs(Dictionary<string, List<DependencyNode>?> dependencyGraphPerFramework, string targetPackage, IAnsiConsole logger)
        {
            // print empty line
            logger.WriteLine();

            // deduplicate the dependency graphs
            List<List<string>> deduplicatedFrameworks = GetDeduplicatedFrameworks(dependencyGraphPerFramework);

            foreach (var frameworks in deduplicatedFrameworks)
            {
                if (frameworks.Count > 0)
                {
                    PrintDependencyGraphPerFramework(frameworks, dependencyGraphPerFramework[frameworks.First()], targetPackage, logger);
                }
            }
        }

        /// <summary>
        /// Prints the dependency graph for a given framework/list of frameworks.
        /// </summary>
        /// <param name="frameworks">The list of frameworks that share this dependency graph.</param>
        /// <param name="topLevelNodes">The top-level package nodes of the dependency graph.</param>
        /// <param name="targetPackage">The package we want the dependency paths for.</param>
        /// <param name="logger"></param>
        private static void PrintDependencyGraphPerFramework(List<string> frameworks, List<DependencyNode>? topLevelNodes, string targetPackage, IAnsiConsole logger)
        {
            var tree = new Tree(string.Join("\n", frameworks.Select(f => $"[[{f}]]")));

            if (topLevelNodes == null || topLevelNodes.Count == 0)
            {
                tree.AddNode(Strings.WhyCommand_Message_NoDependencyGraphsFoundForFramework);
                logger.Write(PadTree(tree));
                return;
            }

            var stack = new Stack<StackOutputData>();

            // initialize the stack with all top-level nodes
            foreach (var node in topLevelNodes.OrderByDescending(c => c.Id, StringComparer.OrdinalIgnoreCase))
            {
                stack.Push(new StackOutputData
                {
                    Node = node,
                    ParentNode = tree
                });
            }

            // print the dependency graph
            while (stack.Count > 0)
            {
                var current = stack.Pop();

                var treeNodeText = GetNodeText(current.Node, targetPackage);
                var treeNode = current.ParentNode.AddNode(treeNodeText);

                if (current.Node.Children?.Count > 0)
                {
                    foreach (var child in current.Node.Children.OrderByDescending(c => c.Id, StringComparer.OrdinalIgnoreCase))
                    {
                        stack.Push(new StackOutputData
                        {
                            Node = child,
                            ParentNode = treeNode
                        });
                    }
                }
            }

            logger.Write(PadTree(tree));
            logger.WriteLine();
        }

        private static IRenderable GetNodeText(DependencyNode node, string targetPackage)
        {
            string text;

            if (node is PackageNode pkgNode)
            {
                string resolved = pkgNode.ResolvedVersion.OriginalVersion ?? pkgNode.ResolvedVersion.ToString();
                string requested = pkgNode.RequestedVersion.ToString("f", VersionRangeFormatter.Instance);
                text = $"{node.Id}@{resolved} ({requested})";
            }
            else
            {
                text = node.Id;
            }

            Style? style = node.Id.Equals(targetPackage, StringComparison.OrdinalIgnoreCase)
                ? new Style(foreground: TargetPackageColor)
                : null;

            return new Text(text, style);
        }

        private static IRenderable PadTree(Tree tree)
        {
            return new Padder(tree, new Padding(left: 2, 0, 0, 0));
        }

        /// <summary>
        /// Deduplicates dependency graphs, and returns groups of frameworks that share the same graph.
        /// </summary>
        /// <param name="dependencyGraphPerFramework">A dictionary mapping target frameworks to their dependency graphs.</param>
        /// <returns>
        /// eg. { { "net6.0", "netcoreapp3.1" }, { "net472" } }
        /// </returns>
        private static List<List<string>> GetDeduplicatedFrameworks(Dictionary<string, List<DependencyNode>?> dependencyGraphPerFramework)
        {
            List<string>? frameworksWithoutGraphs = null;
            var dependencyGraphHashes = new Dictionary<int, List<string>>(dependencyGraphPerFramework.Count);

            foreach (var framework in dependencyGraphPerFramework.Keys)
            {
                if (dependencyGraphPerFramework[framework] == null)
                {
                    frameworksWithoutGraphs ??= [];
                    frameworksWithoutGraphs.Add(framework);
                    continue;
                }

                int hash = GetDependencyGraphHashCode(dependencyGraphPerFramework[framework]);
                if (dependencyGraphHashes.ContainsKey(hash))
                {
                    dependencyGraphHashes[hash].Add(framework);
                }
                else
                {
                    dependencyGraphHashes.Add(hash, [framework]);
                }
            }

            var deduplicatedFrameworks = dependencyGraphHashes.Values.ToList();

            if (frameworksWithoutGraphs != null)
            {
                deduplicatedFrameworks.Add(frameworksWithoutGraphs);
            }

            return deduplicatedFrameworks;
        }

        /// <summary>
        /// Returns a hash for a given dependency graph. Used to deduplicate dependency graphs for different frameworks.
        /// </summary>
        /// <param name="graph">The dependency graph for a given framework.</param>
        private static int GetDependencyGraphHashCode(List<DependencyNode>? graph)
        {
            var hashCodeCombiner = new HashCodeCombiner();
            hashCodeCombiner.AddUnorderedSequence(graph);
            return hashCodeCombiner.CombinedHash;
        }

        private class StackOutputData
        {
            public required DependencyNode Node { get; init; }

            public required IHasTreeNodes ParentNode { get; init; }
        }
    }
}
