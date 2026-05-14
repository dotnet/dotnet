// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.LibraryModel;
using NuGet.Versioning;
using Xunit;

namespace NuGet.DependencyResolver.Core.Tests
{
    [UseCulture("en-US")] // We are asserting exception messages in English
    public class GraphNodeTests
    {
        [Theory]
        [InlineData(null, true)]
        [InlineData(false, false)]
        [InlineData(true, true)]
        public void GraphNode_WithDifferentHasEmptyParentNodes_CreateParentNodesCorrectly(bool? hasParentNodes, bool parentNodesHasNode)
        {
            LibraryRange libraryRangeA = GetLibraryRange("A", "1.0.0");
            LibraryRange libraryRangeB = GetLibraryRange("B", "1.0.0");

            GraphNode<RemoteResolveResult> nodeA, nodeB;

            if (!hasParentNodes.HasValue)
            {
                nodeA = new GraphNode<RemoteResolveResult>(libraryRangeA);
                nodeB = new GraphNode<RemoteResolveResult>(libraryRangeB);
            }
            else
            {
                nodeA = new GraphNode<RemoteResolveResult>(libraryRangeA, hasInnerNodes: true, hasParentNodes.Value);
                nodeB = new GraphNode<RemoteResolveResult>(libraryRangeB, hasInnerNodes: true, hasParentNodes.Value);
            }

            if (!parentNodesHasNode)
            {
                //The ParentNodes should be pointing to the static EmptyList.
                Assert.True(nodeA.ParentNodes == Array.Empty<GraphNode<RemoteResolveResult>>(), "nodeA.ParentNodes should point to the static EmptyList");
                Assert.True(nodeB.ParentNodes == Array.Empty<GraphNode<RemoteResolveResult>>(), "nodeB.ParentNodes should point to the static EmptyList");

                //EmptyList is immutable.
                var exception = Assert.ThrowsAny<NotSupportedException>(
                () => nodeA.ParentNodes.Add(nodeB));
                Assert.Contains("of a fixed size.", exception.Message);
            }
            else
            {
                Assert.False(nodeA.ParentNodes == Array.Empty<GraphNode<RemoteResolveResult>>(), "nodeA.ParentNodes should NOT point to the static EmptyList");
                Assert.False(nodeB.ParentNodes == Array.Empty<GraphNode<RemoteResolveResult>>(), "nodeB.ParentNodes should NOT point to the static EmptyList");
            }
        }

        [Theory]
        [InlineData(null, true)]
        [InlineData(false, false)]
        [InlineData(true, true)]
        public void GraphNode_WithDifferentHasEmptyInnerNodes_CreateInnerNodesCorrectly(bool? hasInnerNodes, bool innerNodesHasNode)
        {
            LibraryRange libraryRangeA = GetLibraryRange("A", "1.0.0");
            LibraryRange libraryRangeB = GetLibraryRange("B", "1.0.0");

            GraphNode<RemoteResolveResult> nodeA, nodeB;

            if (!hasInnerNodes.HasValue)
            {
                nodeA = new GraphNode<RemoteResolveResult>(libraryRangeA);
                nodeB = new GraphNode<RemoteResolveResult>(libraryRangeB);
            }
            else
            {
                nodeA = new GraphNode<RemoteResolveResult>(libraryRangeA, hasInnerNodes.Value, hasParentNodes: true);
                nodeB = new GraphNode<RemoteResolveResult>(libraryRangeB, hasInnerNodes.Value, hasParentNodes: true);
            }

            if (!innerNodesHasNode)
            {
                //The InnerNodes should be pointing to the static EmptyList.
                Assert.True(nodeA.InnerNodes == Array.Empty<GraphNode<RemoteResolveResult>>(), "nodeA.InnerNodes should point to the static EmptyList");
                Assert.True(nodeB.InnerNodes == Array.Empty<GraphNode<RemoteResolveResult>>(), "nodeB.InnerNodes should point to the static EmptyList");

                //EmptyList is immutable.
                var exception = Assert.ThrowsAny<NotSupportedException>(
                () => nodeA.InnerNodes.Add(nodeB));
                Assert.Contains("of a fixed size.", exception.Message);
            }
            else
            {
                Assert.False(nodeA.InnerNodes == Array.Empty<GraphNode<RemoteResolveResult>>(), "nodeA.InnerNodes should NOT point to the static EmptyList");
                Assert.False(nodeB.InnerNodes == Array.Empty<GraphNode<RemoteResolveResult>>(), "nodeB.InnerNodes should NOT point to the static EmptyList");
            }
        }

        public LibraryRange GetLibraryRange(string id, string range)
        {
            return new LibraryRange(id.ToUpperInvariant(), VersionRange.Parse(range), LibraryDependencyTarget.Package);
        }
    }
}
