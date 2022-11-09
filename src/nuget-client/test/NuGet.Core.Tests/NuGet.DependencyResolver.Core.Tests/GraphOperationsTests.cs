// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using FluentAssertions;
using NuGet.ContentModel;
using NuGet.LibraryModel;
using NuGet.Versioning;
using Xunit;

namespace NuGet.DependencyResolver.Core.Tests
{
    public class GraphOperationsTests
    {
        [Fact]
        public void GraphOperations_GetIdForResolvedNodeVerifyResolvedIdUsed()
        {
            var node = GetPackageNode("a", "1.0.0", "1.0.0");
            node.GetId().Should().Be("a");
        }

        [Fact]
        public void GraphOperations_GetIdForUnResolvedNodeVerifyResult()
        {
            var node = GetUnresolvedNode("a", "1.0.0", LibraryDependencyTarget.Package);
            node.GetId().Should().Be("A");
        }

        [Fact]
        public void GraphOperations_GetVersionRangeVerifyResult()
        {
            var node = GetPackageNode("a", "[1.0.0]", "2.0.0");
            node.GetVersionRange().PrettyPrint().Should().Be("(= 1.0.0)");
        }

        [Fact]
        public void GraphOperations_GetVersionVerifyResult()
        {
            var node = GetPackageNode("a", "1.0.0", "2.0.0");
            node.GetVersionOrDefault().ToNormalizedString().Should().Be("2.0.0");
        }

        [Fact]
        public void GraphOperations_GetVersionFromUnresolvedReturnsNull()
        {
            var node = GetUnresolvedNode("a", "1.0.0", LibraryDependencyTarget.Package);
            node.GetVersionOrDefault().Should().BeNull();
        }

        [Fact]
        public void GraphOperations_GetIdAndRangeVerifyResult()
        {
            var node = GetPackageNode("a", "1.0.0", "2.0.0");
            node.GetIdAndRange().Should().Be("a (>= 1.0.0)");
        }

        [Fact]
        public void GraphOperations_GetIdAndRangeForUnresolvedVerifyResult()
        {
            var node = GetUnresolvedNode("a", "1.0.0", LibraryDependencyTarget.Package);
            node.GetIdAndRange().Should().Be("A (>= 1.0.0)");
        }

        [Fact]
        public void GraphOperations_GetIdAndRangeForUnresolvedProjectVerifyResult()
        {
            var node = GetUnresolvedNode("a", "1.0.0", LibraryDependencyTarget.Project);
            node.GetIdAndRange().Should().Be("A");
        }

        [Fact]
        public void GraphOperations_GetIdAndRangeForProjectVerifyResult()
        {
            var node = GetProjectNode("a");
            node.GetIdAndRange().Should().Be("a");
        }

        [Fact]
        public void GraphOperations_IsPackageForProject()
        {
            var node = GetProjectNode("a");
            node.IsPackage().Should().BeFalse();
        }

        [Fact]
        public void GraphOperations_IsPackageForPackage()
        {
            var node = GetPackageNode("a", "1.0.0", "2.0.0");
            node.IsPackage().Should().BeTrue();
        }

        [Fact]
        public void GraphOperations_IsPackageForUnresolvedPackage()
        {
            var node = GetUnresolvedNode("a", "1.0.0", LibraryDependencyTarget.Package);
            node.IsPackage().Should().BeTrue();
        }

        [Fact]
        public void GraphOperations_IsPackageForUnresolvedProject()
        {
            var node = GetUnresolvedNode("a", "1.0.0", LibraryDependencyTarget.ExternalProject);
            node.IsPackage().Should().BeFalse();
        }

        [Fact]
        public void GraphOperations_GetIdAndVersionOrRangeVerifyResult()
        {
            var node = GetPackageNode("a", "1.0.0", "2.0");
            node.GetIdAndVersionOrRange().Should().Be("a 2.0.0");
        }

        [Fact]
        public void GraphOperations_GetIdAndVersionOrRangeForProjectVerifyResult()
        {
            var node = GetProjectNode("a");
            node.GetIdAndVersionOrRange().Should().Be("a");
        }

        [Fact]
        public void GraphOperations_GetIdAndVersionOrRangeForUnresolvedVerifyResult()
        {
            var node = GetUnresolvedNode("a", "1.0.0", LibraryDependencyTarget.Package);
            node.GetIdAndVersionOrRange().Should().Be("A (>= 1.0.0)");
        }

        [Fact]
        public void GraphOperations_GetPathForUnresolvedNode()
        {
            var node = GetUnresolvedNode("a", "1.0.0", LibraryDependencyTarget.Package);
            node.GetPath().Should().Be("A (>= 1.0.0)");
        }

        [Fact]
        public void GraphOperations_GetPathWithParent()
        {
            var node = GetPackageNode("b", "1.0.0", "2.0.0");
            var parent = GetPackageNode("a", "1.*", "3.0.0");
            node.OuterNode = parent;

            node.GetPath().Should().Be("a 3.0.0 -> b 2.0.0");
        }

        [Fact]
        public void GraphOperations_GetPathWithProjectParent()
        {
            var node = GetPackageNode("b", "1.*", "2.0.0");
            var parent = GetProjectNode("a");
            node.OuterNode = parent;

            node.GetPath().Should().Be("a -> b 2.0.0");
        }

        [Fact]
        public void GraphOperations_GetPathWithProjectParentAndUnresolved()
        {
            var node = GetUnresolvedNode("b", "1.*", LibraryDependencyTarget.Package);
            var parent = GetProjectNode("a");
            node.OuterNode = parent;

            node.GetPath().Should().Be("a -> B (>= 1.0.0)");
        }

        [Fact]
        public void GraphOperations_GetPathWithLastRangeWithParent()
        {
            var node = GetPackageNode("b", "1.0.0", "2.0.0");
            var parent = GetPackageNode("a", "1.*", "3.0.0");
            node.OuterNode = parent;

            node.GetPathWithLastRange().Should().Be("a 3.0.0 -> b (>= 1.0.0)");
        }

        [Fact]
        public void GraphOperations_GetPathWithLastRangeForProject()
        {
            var node = GetProjectNode("b");
            var parent = GetProjectNode("a");
            node.OuterNode = parent;

            node.GetPathWithLastRange().Should().Be("a -> b");
        }

        [Theory]
        [InlineData(true, false)]
        [InlineData(false, false)]
        [InlineData(true, true)]
        [InlineData(false, true)]

        public void GraphOperations_GraphItem_EqualObjects(bool nullIdentity, bool isCentralTransitive)
        {
            var libraryIdentity = nullIdentity ? null : new LibraryIdentity("a", new NuGetVersion("2.0.0"), LibraryType.Package);

            var graphItem = new GraphItem<RemoteResolveResult>(libraryIdentity);
            graphItem.IsCentralTransitive = isCentralTransitive;

            var graphItem2 = new GraphItem<RemoteResolveResult>(libraryIdentity);
            graphItem2.IsCentralTransitive = isCentralTransitive;

            Assert.True(graphItem.Equals(graphItem));
            Assert.True(graphItem.Equals(graphItem2));
            Assert.Equal(graphItem.GetHashCode(), graphItem2.GetHashCode());
        }

        [Fact]

        public void GraphOperations_GraphItem_NotEqualObjects()
        {
            var libraryIdentity1 = new LibraryIdentity("a", new NuGetVersion("2.0.0"), LibraryType.Package);
            var libraryIdentity2 = new LibraryIdentity("b", new NuGetVersion("2.0.0"), LibraryType.Package);

            var graphItem_1_1 = new GraphItem<RemoteResolveResult>(libraryIdentity1);
            graphItem_1_1.IsCentralTransitive = true;

            var graphItem_1_2 = new GraphItem<RemoteResolveResult>(libraryIdentity1);
            graphItem_1_2.IsCentralTransitive = false;

            var graphItem_2_1 = new GraphItem<RemoteResolveResult>(libraryIdentity2);
            graphItem_2_1.IsCentralTransitive = true;

            var graphItem_2_2 = new GraphItem<RemoteResolveResult>(libraryIdentity2);
            graphItem_2_2.IsCentralTransitive = false;


            var graphItem_null_1 = new GraphItem<RemoteResolveResult>(null);
            graphItem_null_1.IsCentralTransitive = true;

            var graphItem_null_2 = new GraphItem<RemoteResolveResult>(null);
            graphItem_null_2.IsCentralTransitive = false;

            Assert.False(graphItem_1_1.Equals(graphItem_1_2));
            Assert.False(graphItem_1_1.Equals(graphItem_2_1));
            Assert.False(graphItem_1_1.Equals(graphItem_2_2));
            Assert.False(graphItem_1_1.Equals(graphItem_null_1));
            Assert.False(graphItem_1_1.Equals(graphItem_null_2));

            Assert.False(graphItem_1_2.Equals(graphItem_2_1));
            Assert.False(graphItem_1_2.Equals(graphItem_2_2));
            Assert.False(graphItem_1_2.Equals(graphItem_null_1));
            Assert.False(graphItem_1_2.Equals(graphItem_null_2));

            Assert.False(graphItem_2_1.Equals(graphItem_2_2));
            Assert.False(graphItem_2_1.Equals(graphItem_null_1));
            Assert.False(graphItem_2_1.Equals(graphItem_null_2));

            Assert.False(graphItem_2_2.Equals(graphItem_null_1));
            Assert.False(graphItem_2_2.Equals(graphItem_null_2));

            Assert.False(graphItem_null_1.Equals(graphItem_null_2));
        }

        public GraphNode<RemoteResolveResult> GetNode(string id, string range, LibraryDependencyTarget target, string version, LibraryType type)
        {
            return new GraphNode<RemoteResolveResult>(new LibraryRange(id.ToUpperInvariant(), VersionRange.Parse(range), LibraryDependencyTarget.All))
            {
                Item = new GraphItem<RemoteResolveResult>(
                    new LibraryIdentity(
                        id.ToLowerInvariant(),
                        new NuGetVersion(version),
                        type))
            };
        }

        public GraphNode<RemoteResolveResult> GetPackageNode(string id, string range, string version)
        {
            return new GraphNode<RemoteResolveResult>(new LibraryRange(id.ToUpperInvariant(), VersionRange.Parse(range), LibraryDependencyTarget.Package))
            {
                Item = new GraphItem<RemoteResolveResult>(
                    new LibraryIdentity(
                        id.ToLowerInvariant(),
                        new NuGetVersion(version),
                        LibraryType.Package))
            };
        }

        public GraphNode<RemoteResolveResult> GetProjectNode(string id)
        {
            return new GraphNode<RemoteResolveResult>(new LibraryRange(id.ToUpperInvariant(), VersionRange.Parse("1.0.0"), LibraryDependencyTarget.ExternalProject))
            {
                Item = new GraphItem<RemoteResolveResult>(
                    new LibraryIdentity(
                        id.ToLowerInvariant(),
                        new NuGetVersion("1.0.0"),
                        LibraryType.Project))
            };
        }

        public GraphNode<RemoteResolveResult> GetUnresolvedNode(string id, string range, LibraryDependencyTarget target)
        {
            return new GraphNode<RemoteResolveResult>(new LibraryRange(id.ToUpperInvariant(), VersionRange.Parse(range), target));
        }
    }
}
