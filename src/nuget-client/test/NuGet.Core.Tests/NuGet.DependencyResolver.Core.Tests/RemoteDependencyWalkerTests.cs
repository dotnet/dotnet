// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.Versioning;
using Test.Utility;
using Xunit;

namespace NuGet.DependencyResolver.Tests
{
    public class RemoteDependencyWalkerTests
    {
        [Fact]
        public async Task AllowPreleaseVersionNoConflict()
        {
            var context = new TestRemoteWalkContext();
            var provider = new DependencyProvider();
            provider.Package("A", "1.0-beta")
                    .DependsOn("B", "1.0")
                    .DependsOn("C", "1.0-beta")
                    .DependsOn("E", "1.0");

            provider.Package("B", "1.0")
                    .DependsOn("D", "1.0");

            provider.Package("C", "1.0-beta")
                    .DependsOn("D", "1.1-beta");

            provider.Package("E", "1.0")
                    .DependsOn("D", "0.1");

            context.LocalLibraryProviders.Add(provider);
            var walker = new RemoteDependencyWalker(context);
            var node = await DoWalkAsync(walker, "A");

            var result = node.Analyze();

            Assert.Equal(0, result.VersionConflicts.Count);
        }

        [Fact]
        public async Task CyclesAreDetected()
        {
            var context = new TestRemoteWalkContext();
            var provider = new DependencyProvider();
            provider.Package("A", "1.0")
                    .DependsOn("B", "2.0");

            provider.Package("B", "2.0")
                    .DependsOn("A", "1.0");

            context.LocalLibraryProviders.Add(provider);
            var walker = new RemoteDependencyWalker(context);
            var node = await DoWalkAsync(walker, "A");

            var result = node.Analyze();

            Assert.Equal(1, result.Cycles.Count);

            var cycle = result.Cycles[0];

            AssertPath(cycle, "A 1.0", "B 2.0", "A 1.0");
        }

        [Fact]
        public async Task CyclesAreDetectedIf2VersionsOfTheSamePackageId()
        {
            var context = new TestRemoteWalkContext();
            var provider = new DependencyProvider();
            provider.Package("A", "1.0")
                    .DependsOn("B", "2.0");

            provider.Package("B", "2.0")
                    .DependsOn("A", "5.0");

            context.LocalLibraryProviders.Add(provider);
            var walker = new RemoteDependencyWalker(context);
            var node = await DoWalkAsync(walker, "A");

            var result = node.Analyze();

            Assert.Equal(1, result.Cycles.Count);

            var cycle = result.Cycles[0];

            AssertPath(cycle, "A 1.0", "B 2.0", "A 5.0");
        }

        [Fact]
        public async Task DeepCycleCheck()
        {
            var context = new TestRemoteWalkContext();
            var provider = new DependencyProvider();
            provider.Package("A", "1.0")
                    .DependsOn("B", "2.0");

            provider.Package("B", "2.0")
                    .DependsOn("C", "2.0");

            provider.Package("C", "2.0")
                .DependsOn("D", "1.0")
                .DependsOn("E", "1.0");

            provider.Package("D", "1.0");
            provider.Package("E", "1.0")
                .DependsOn("A", "1.0");

            context.LocalLibraryProviders.Add(provider);
            var walker = new RemoteDependencyWalker(context);
            var node = await DoWalkAsync(walker, "A");

            var result = node.Analyze();

            Assert.Equal(1, result.Cycles.Count);

            var cycle = result.Cycles[0];

            AssertPath(cycle, "A 1.0", "B 2.0", "C 2.0", "E 1.0", "A 1.0");
        }

        [Fact]
        public async Task DependencyRangesButNoConflict()
        {
            var context = new TestRemoteWalkContext();
            var provider = new DependencyProvider();
            provider.Package("A", "1.0")
                    .DependsOn("B", "2.0")
                    .DependsOn("C", "2.0");

            provider.Package("B", "2.0")
                    .DependsOn("D", "2.0");

            provider.Package("C", "2.0")
                    .DependsOn("D", "1.0");

            provider.Package("D", "1.0")
                    .DependsOn("E", "[1.0]");

            provider.Package("D", "2.0")
                    .DependsOn("E", "[2.0]");

            provider.Package("E", "1.0");
            provider.Package("E", "2.0");

            context.LocalLibraryProviders.Add(provider);
            var walker = new RemoteDependencyWalker(context);
            var node = await DoWalkAsync(walker, "A");

            var result = node.Analyze();

            Assert.Equal(0, result.VersionConflicts.Count);
        }

        [Fact]
        public async Task AllowProjectOverridePackageNoConflict()
        {
            var context = new TestRemoteWalkContext();
            var packageProvider = new DependencyProvider();
            var projectProvider = new DependencyProvider();
            projectProvider.Package("A", "1.0", LibraryType.Project)
                    .DependsOn("B", "2.0", LibraryDependencyTarget.Project)
                    .DependsOn("C", "2.0");

            projectProvider.Package("B", "2.0", LibraryType.Project)
                    .DependsOn("D", "[2.0]", LibraryDependencyTarget.Project);

            packageProvider.Package("C", "2.0")
                    .DependsOn("D", "[1.0]");

            packageProvider.Package("D", "1.0");
            projectProvider.Package("D", "2.0", LibraryType.Project);

            context.LocalLibraryProviders.Add(packageProvider);
            context.ProjectLibraryProviders.Add(projectProvider);

            var walker = new RemoteDependencyWalker(context);
            var node = await DoWalkAsync(walker, "A");

            var result = node.Analyze();

            Assert.Equal(0, result.VersionConflicts.Count);
        }

        [Fact]
        public async Task SingleConflict()
        {
            var context = new TestRemoteWalkContext();
            var provider = new DependencyProvider();
            provider.Package("A", "1.0")
                    .DependsOn("B", "2.0")
                    .DependsOn("C", "2.0");

            provider.Package("B", "2.0")
                    .DependsOn("D", "[2.0]");

            provider.Package("C", "2.0")
                    .DependsOn("D", "[1.0]");

            provider.Package("D", "1.0");
            provider.Package("D", "2.0");

            context.LocalLibraryProviders.Add(provider);
            var walker = new RemoteDependencyWalker(context);
            var node = await DoWalkAsync(walker, "A");

            var result = node.Analyze();

            Assert.Equal(1, result.VersionConflicts.Count);

            var conflict = result.VersionConflicts[0];
            var c1 = conflict.Selected;
            var c2 = conflict.Conflicting;

            AssertPath(c1, "A 1.0", "B 2.0", "D 2.0");
            AssertPath(c2, "A 1.0", "C 2.0", "D 1.0");
        }

        [Fact]
        public async Task SingleConflictWhereConflictingDependenyIsUnresolved()
        {
            var context = new TestRemoteWalkContext();
            var provider = new DependencyProvider();
            provider.Package("A", "1.0")
                    .DependsOn("B", "2.0")
                    .DependsOn("C", "2.0");

            provider.Package("B", "2.0")
                    .DependsOn("D", "[2.0]");

            provider.Package("C", "2.0")
                    .DependsOn("D", "[1.0]");

            provider.Package("D", "2.0");

            context.LocalLibraryProviders.Add(provider);
            var walker = new RemoteDependencyWalker(context);
            var node = await DoWalkAsync(walker, "A");

            var result = node.Analyze();

            Assert.Equal(1, result.VersionConflicts.Count);

            var conflict = result.VersionConflicts[0];
            var c1 = conflict.Selected;
            var c2 = conflict.Conflicting;

            AssertPath(c1, "A 1.0", "B 2.0", "D 2.0");
            AssertPath(c2, "A 1.0", "C 2.0", "D 1.0");
        }

        [Fact]
        public async Task StrictDependenciesButNoConflict()
        {
            var context = new TestRemoteWalkContext();
            var provider = new DependencyProvider();
            provider.Package("A", "1.0")
                    .DependsOn("B", "2.0")
                    .DependsOn("C", "2.0");

            provider.Package("B", "2.0")
                    .DependsOn("D", "[2.0]");

            provider.Package("C", "2.0")
                    .DependsOn("D", "[1.0, 3.0]");

            provider.Package("D", "1.0");
            provider.Package("D", "2.0");

            context.LocalLibraryProviders.Add(provider);
            var walker = new RemoteDependencyWalker(context);
            var node = await DoWalkAsync(walker, "A");

            var result = node.Analyze();

            Assert.Equal(0, result.VersionConflicts.Count);
        }

        [Fact]
        public async Task ConflictAtDifferentLevel()
        {
            var context = new TestRemoteWalkContext();
            var provider = new DependencyProvider();
            provider.Package("A", "1.0")
                    .DependsOn("B", "2.0")
                    .DependsOn("C", "2.0")
                    .DependsOn("F", "2.0");

            provider.Package("B", "2.0")
                    .DependsOn("D", "2.0");

            provider.Package("C", "2.0")
                    .DependsOn("D", "1.0");

            provider.Package("D", "1.0")
                    .DependsOn("E", "[1.0]");

            provider.Package("D", "2.0")
                    .DependsOn("E", "[2.0]");

            provider.Package("F", "2.0")
                    .DependsOn("E", "[1.0]");

            provider.Package("E", "1.0");
            provider.Package("E", "2.0");

            context.LocalLibraryProviders.Add(provider);
            var walker = new RemoteDependencyWalker(context);
            var node = await DoWalkAsync(walker, "A");

            var result = node.Analyze();

            Assert.Equal(1, result.VersionConflicts.Count);

            var conflict = result.VersionConflicts[0];
            var c1 = conflict.Selected;
            var c2 = conflict.Conflicting;

            AssertPath(c1, "A 1.0", "B 2.0", "D 2.0", "E 2.0");
            AssertPath(c2, "A 1.0", "F 2.0", "E 1.0");
        }

        [Fact]
        public async Task TryResolveConflicts_ThrowsIfPackageConstraintCannotBeResolved()
        {
            var context = new TestRemoteWalkContext();
            var provider = new DependencyProvider();
            provider.Package("Root", "1.0")
                    .DependsOn("A", "1.0")
                    .DependsOn("B", "2.0");

            provider.Package("A", "1.0")
                    .DependsOn("C", "(1.0, 1.4]");

            provider.Package("B", "2.0")
                    .DependsOn("C", "1.8");

            provider.Package("C", "1.3.8");
            provider.Package("C", "1.8");

            context.LocalLibraryProviders.Add(provider);
            var walker = new RemoteDependencyWalker(context);
            var node = await DoWalkAsync(walker, "Root");

            var result = node.Analyze();

            Assert.Equal(1, result.VersionConflicts.Count);

            var conflict = result.VersionConflicts[0];
            var c1 = conflict.Selected;
            var c2 = conflict.Conflicting;

            AssertPath(c1, "Root 1.0", "B 2.0", "C 1.8");
            AssertPath(c2, "Root 1.0", "A 1.0", "C 1.3.8");
        }

        [Fact]
        public async Task TryResolveConflicts_WorksWhenVersionRangeIsNotSpecified()
        {
            var context = new TestRemoteWalkContext();
            var provider = new DependencyProvider();
            provider.Package("Root", "1.0")
                    .DependsOn("A", "1.0")
                    .DependsOn("B", "2.0");

            provider.Package("A", "1.0")
                    .DependsOn("C");

            provider.Package("B", "2.0")
                    .DependsOn("C", "1.8");

            provider.Package("C", "1.8");
            provider.Package("C", "2.0");

            context.LocalLibraryProviders.Add(provider);
            var walker = new RemoteDependencyWalker(context);
            var node = await DoWalkAsync(walker, "Root");

            // Restore doesn't actually support null versions so fake a resolved dependency
            var cNode = node.Path("A", "C");
            cNode.Key.TypeConstraint = LibraryDependencyTarget.Package;
            cNode.Item = new GraphItem<RemoteResolveResult>(new LibraryIdentity
            {
                Name = "C",
                Version = new NuGetVersion("2.0"),
                Type = LibraryType.Package
            });

            var result = node.Analyze();

            Assert.Empty(result.VersionConflicts);

            Assert.Equal(Disposition.Accepted, cNode.Disposition);
            Assert.Equal(Disposition.Rejected, node.Path("B", "C").Disposition);
        }

        [Fact]
        public async Task NearestWinsOverridesStrictDependency()
        {
            var context = new TestRemoteWalkContext();
            var provider = new DependencyProvider();
            provider.Package("A", "1.0")
                    .DependsOn("B", "2.0")
                    .DependsOn("C", "2.0")
                    .DependsOn("D", "3.0");

            provider.Package("B", "2.0")
                    .DependsOn("D", "[2.0]");

            provider.Package("C", "2.0")
                    .DependsOn("D", "[1.0]");

            provider.Package("D", "1.0");
            provider.Package("D", "2.0");
            provider.Package("D", "3.0");

            context.LocalLibraryProviders.Add(provider);
            var walker = new RemoteDependencyWalker(context);
            var node = await DoWalkAsync(walker, "A");

            var result = node.Analyze();

            Assert.Equal(0, result.VersionConflicts.Count);
            Assert.Equal(0, result.Downgrades.Count);
        }

        [Fact]
        public async Task NearestWinsOverridesStrictDependencyButDowngradeWarns()
        {
            var context = new TestRemoteWalkContext();
            var provider = new DependencyProvider();
            provider.Package("A", "1.0")
                    .DependsOn("B", "2.0")
                    .DependsOn("C", "2.0")
                    .DependsOn("D", "1.0");

            provider.Package("B", "2.0")
                    .DependsOn("D", "[2.0]");

            provider.Package("C", "2.0")
                    .DependsOn("D", "[1.0]");

            provider.Package("D", "1.0");
            provider.Package("D", "2.0");
            provider.Package("D", "3.0");

            context.LocalLibraryProviders.Add(provider);
            var walker = new RemoteDependencyWalker(context);
            var node = await DoWalkAsync(walker, "A");

            var result = node.Analyze();

            Assert.Equal(0, result.VersionConflicts.Count);
            Assert.Equal(1, result.Downgrades.Count);

            var downgraded = result.Downgrades[0].DowngradedFrom;
            var downgradedBy = result.Downgrades[0].DowngradedTo;
            AssertPath(downgraded, "A 1.0", "B 2.0", "D 2.0");
            AssertPath(downgradedBy, "A 1.0", "D 1.0");
        }

        [Fact]
        public async Task DowngradeSkippedIfEqual()
        {
            var context = new TestRemoteWalkContext();
            var provider = new DependencyProvider();
            provider.Package("A", "1.0")
                    .DependsOn("B", "2.0")
                    .DependsOn("C", "2.0");

            provider.Package("B", "2.0")
                    .DependsOn("C", "2.0");

            provider.Package("C", "2.0");

            context.LocalLibraryProviders.Add(provider);
            var walker = new RemoteDependencyWalker(context);
            var node = await DoWalkAsync(walker, "A");

            var result = node.Analyze();

            Assert.Equal(0, result.Downgrades.Count);
        }

        [Fact]
        public async Task DowngradeAtRootIsDetected()
        {
            var context = new TestRemoteWalkContext();
            var provider = new DependencyProvider();
            provider.Package("A", "1.0")
                    .DependsOn("B", "2.0")
                    .DependsOn("C", "1.0");

            provider.Package("B", "2.0")
                    .DependsOn("C", "2.0");

            provider.Package("C", "1.0");
            provider.Package("C", "2.0");

            context.LocalLibraryProviders.Add(provider);
            var walker = new RemoteDependencyWalker(context);
            var node = await DoWalkAsync(walker, "A");

            var result = node.Analyze();

            Assert.Equal(1, result.Downgrades.Count);
            var downgraded = result.Downgrades[0].DowngradedFrom;
            var downgradedBy = result.Downgrades[0].DowngradedTo;

            AssertPath(downgraded, "A 1.0", "B 2.0", "C 2.0");
            AssertPath(downgradedBy, "A 1.0", "C 1.0");
        }

        [Fact]
        public async Task DowngradeNotAtRootIsDetected()
        {
            var context = new TestRemoteWalkContext();
            var provider = new DependencyProvider();
            provider.Package("A", "1.0")
                    .DependsOn("B", "2.0");

            provider.Package("B", "2.0")
                    .DependsOn("C", "2.0")
                    .DependsOn("D", "1.0");

            provider.Package("C", "2.0")
                    .DependsOn("D", "2.0");

            provider.Package("D", "1.0");

            context.LocalLibraryProviders.Add(provider);
            var walker = new RemoteDependencyWalker(context);
            var node = await DoWalkAsync(walker, "A");

            var result = node.Analyze();

            Assert.Equal(1, result.Downgrades.Count);
            var downgraded = result.Downgrades[0].DowngradedFrom;
            var downgradedBy = result.Downgrades[0].DowngradedTo;

            AssertPath(downgraded, "A 1.0", "B 2.0", "C 2.0", "D 2.0");
            AssertPath(downgradedBy, "A 1.0", "B 2.0", "D 1.0");
        }

        [Fact]
        public async Task DowngradeOverddienByCousinCheck()
        {
            var context = new TestRemoteWalkContext();
            var provider = new DependencyProvider();
            provider.Package("A", "1.0")
                    .DependsOn("B", "1.0")
                    .DependsOn("C", "1.0");

            provider.Package("B", "1.0")
                    .DependsOn("E", "1.0")
                    .DependsOn("D", "1.0");

            provider.Package("E", "1.0")
                    .DependsOn("D", "2.0");

            provider.Package("C", "1.0")
                    .DependsOn("D", "3.0");

            provider.Package("D", "1.0");
            provider.Package("D", "2.0");
            provider.Package("D", "3.0");

            context.LocalLibraryProviders.Add(provider);
            var walker = new RemoteDependencyWalker(context);
            var node = await DoWalkAsync(walker, "A");

            var result = node.Analyze();

            Assert.Equal(0, result.Downgrades.Count);
        }

        [Fact]
        public async Task PotentialDowngradeThenUpgrade()
        {
            var context = new TestRemoteWalkContext();
            var provider = new DependencyProvider();
            provider.Package("A", "1.0")
                    .DependsOn("B", "1.2")
                    .DependsOn("C", "1.0");

            provider.Package("B", "1.2");

            provider.Package("C", "1.0")
                    .DependsOn("B", "0.8")
                    .DependsOn("D", "1.0");

            provider.Package("B", "0.8");

            provider.Package("D", "1.0")
                    .DependsOn("B", "1.0");

            provider.Package("B", "1.0");

            context.LocalLibraryProviders.Add(provider);
            var walker = new RemoteDependencyWalker(context);
            var node = await DoWalkAsync(walker, "A");

            var result = node.Analyze();

            Assert.Equal(0, result.Downgrades.Count);
        }

        [Fact]
        public async Task DowngradeThenUpgradeThenDowngrade()
        {
            var context = new TestRemoteWalkContext();
            var provider = new DependencyProvider();
            provider.Package("A", "1.0")
                    .DependsOn("B", "1.0")
                    .DependsOn("C", "1.0");

            provider.Package("B", "1.0");

            provider.Package("C", "1.0")
                    .DependsOn("B", "2.0")
                    .DependsOn("D", "1.0");

            provider.Package("B", "2.0");

            provider.Package("D", "1.0")
                    .DependsOn("B", "1.0");

            context.LocalLibraryProviders.Add(provider);
            var walker = new RemoteDependencyWalker(context);
            var node = await DoWalkAsync(walker, "A");

            var result = node.Analyze();

            Assert.Equal(1, result.Downgrades.Count);
            var downgraded = result.Downgrades[0].DowngradedFrom;
            var downgradedBy = result.Downgrades[0].DowngradedTo;

            AssertPath(downgraded, "A 1.0", "C 1.0", "B 2.0");
            AssertPath(downgradedBy, "A 1.0", "B 1.0");
        }

        [Fact]
        public async Task UpgradeThenDowngradeThenEqual()
        {
            var context = new TestRemoteWalkContext();
            var provider = new DependencyProvider();
            provider.Package("A", "1.0")
                    .DependsOn("B", "2.0")
                    .DependsOn("C", "1.0");

            provider.Package("B", "1.0");

            provider.Package("C", "1.0")
                    .DependsOn("B", "1.0")
                    .DependsOn("D", "1.0");

            provider.Package("B", "2.0");

            provider.Package("D", "1.0")
                    .DependsOn("B", "2.0");

            context.LocalLibraryProviders.Add(provider);
            var walker = new RemoteDependencyWalker(context);
            var node = await DoWalkAsync(walker, "A");

            var result = node.Analyze();

            Assert.Equal(0, result.Downgrades.Count);
        }

        [Fact]
        public async Task DoubleDowngrade()
        {
            var context = new TestRemoteWalkContext();
            var provider = new DependencyProvider();
            provider.Package("A", "1.0")
                    .DependsOn("B", "0.7")
                    .DependsOn("C", "1.0");

            provider.Package("B", "0.7");

            provider.Package("C", "1.0")
                    .DependsOn("B", "0.8")
                    .DependsOn("D", "1.0");

            provider.Package("B", "0.8");

            provider.Package("D", "1.0")
                    .DependsOn("B", "1.0");

            provider.Package("B", "1.0");

            context.LocalLibraryProviders.Add(provider);
            var walker = new RemoteDependencyWalker(context);
            var node = await DoWalkAsync(walker, "A");

            var downgrades = new List<Tuple<GraphNode<RemoteResolveResult>, GraphNode<RemoteResolveResult>>>();
            var cycles = new List<GraphNode<RemoteResolveResult>>();

            var result = node.Analyze();

            Assert.Equal(2, result.Downgrades.Count);


            var d0 = result.Downgrades[0];
            var d0To = d0.DowngradedFrom;
            var d0By = d0.DowngradedTo;

            AssertPath(d0To, "A 1.0", "C 1.0", "B 0.8");
            AssertPath(d0By, "A 1.0", "B 0.7");

            var d1 = result.Downgrades[1];
            var d1To = d1.DowngradedFrom;
            var d1By = d1.DowngradedTo;

            AssertPath(d1To, "A 1.0", "C 1.0", "D 1.0", "B 1.0");
            AssertPath(d1By, "A 1.0", "B 0.7");

        }

        [Fact]
        public void IsGreaterThanEqualTo_ReturnsTrue_IfLeftVersionIsUnbound()
        {
            // Arrange
            var leftVersion = VersionRange.All;
            var rightVersion = VersionRange.Parse("1.0.0");

            // Act
            var isGreater = RemoteDependencyWalker.IsGreaterThanOrEqualTo(leftVersion, rightVersion);

            // Assert
            Assert.True(isGreater);
        }

        [Fact]
        public void IsGreaterThanEqualTo_ReturnsFalse_IfRightVersionIsUnbound()
        {
            // Arrange
            var leftVersion = VersionRange.Parse("3.1.0-*");
            var rightVersion = VersionRange.All;

            // Act
            var isGreater = RemoteDependencyWalker.IsGreaterThanOrEqualTo(leftVersion, rightVersion);

            // Assert
            Assert.False(isGreater);
        }

        [Theory]
        [InlineData("3.0", "3.0")]
        [InlineData("3.0", "3.0.0")]
        [InlineData("3.1", "3.0.0")]
        [InlineData("3.1.2", "3.1.1")]
        [InlineData("3.1.2-beta", "3.1.2-alpha")]
        [InlineData("[3.1.2-beta, 4.0)", "[3.1.1, 4.3)")]
        [InlineData("[3.1.2-*, 4.0)", "3.1.2-alpha-1002")]
        [InlineData("3.1.2-prerelease", "3.1.2-alpha-*")]
        [InlineData("3.1.2-beta-*", "3.1.2-alpha-*")]
        [InlineData("3.1.*", "3.1.2-alpha-*")]
        [InlineData("*", "3.1.2-alpha-*")]
        [InlineData("*", "*")]
        [InlineData("1.*", "1.1.*")]
        [InlineData("1.*", "1.3.*")]
        [InlineData("1.8.*", "1.8.3.*")]
        [InlineData("1.8.3.5*", "1.8.3.4-*")]
        [InlineData("1.8.3.*", "1.8.3.4-*")]
        [InlineData("1.8.3.4-alphabeta-*", "1.8.3.4-alpha*")]
        [InlineData("1.8.5.4-alpha-*", "1.8.3.4-gamma*")]
        [InlineData("1.8.3.6-alpha-*", "1.8.3.4-gamma*")]
        [InlineData("1.8.3-*", "1.8.3-alpha*")]
        [InlineData("1.8.3-*", "1.8.3-*")]
        [InlineData("1.8.4-*", "1.8.3-*")]
        [InlineData("2.8.1-*", "1.8.3-*")]
        [InlineData("3.2.0-*", "3.1.0-beta-234")]
        [InlineData("3.*", "3.1.*")]
        public void IsGreaterThanEqualTo_ReturnsTrue_IfRightVersionIsSmallerThanLeft(string leftVersionString, string rightVersionString)
        {
            // Arrange
            var leftVersion = VersionRange.Parse(leftVersionString);
            var rightVersion = VersionRange.Parse(rightVersionString);

            // Act
            var isGreater = RemoteDependencyWalker.IsGreaterThanOrEqualTo(leftVersion, rightVersion);

            // Assert
            Assert.True(isGreater);
        }

        [Theory]
        [InlineData("1.3.4", "1.4.3")]
        [InlineData("3.0", "3.1")]
        [InlineData("3.0", "*")]
        [InlineData("3.0-*", "*")]
        [InlineData("3.2.4", "3.2.7")]
        [InlineData("3.2.4-alpha", "[3.2.4-beta, 4.0)")]
        [InlineData("2.2.4-alpha", "2.2.4-beta-*")]
        [InlineData("2.2.4-beta-1", "2.2.4-beta1*")]
        [InlineData("2.2.1.*", "2.3.*")]
        [InlineData("2.*", "3.1.*")]
        [InlineData("3.4.6.*", "3.6.*")]
        [InlineData("3.4.6-alpha*", "3.4.6-beta*")]
        [InlineData("3.4.6-beta*", "3.4.6-betb*")]
        [InlineData("3.1.0-beta-234", "3.2.0-*")]
        [InlineData("3.0.0-*", "3.1.0-beta-234")]
        public void IsGreaterThanEqualTo_ReturnsFalse_IfRightVersionIsLargerThanLeft(string leftVersionString, string rightVersionString)
        {
            // Arrange
            var leftVersion = VersionRange.Parse(leftVersionString);
            var rightVersion = VersionRange.Parse(rightVersionString);

            // Act
            var isGreater = RemoteDependencyWalker.IsGreaterThanOrEqualTo(leftVersion, rightVersion);

            // Assert
            Assert.False(isGreater);
        }

        [Fact]
        public void TransitiveCentralPackageVersions_AddAndTake()
        {
            // Arrange
            var transitiveCentralPackageVersions = new RemoteDependencyWalker.TransitiveCentralPackageVersions();
            var centralPackageVersionDependency = new LibraryDependency()
            {
                LibraryRange = new LibraryRange("name1", VersionRange.Parse("1.0.0"), LibraryDependencyTarget.Package),
            };
            var parent = new GraphNode<RemoteResolveResult>(new LibraryRange("parentname1", VersionRange.Parse("1.0.0"), LibraryDependencyTarget.Package));

            transitiveCentralPackageVersions.Add(centralPackageVersionDependency, parent);
            bool resultTake1 = transitiveCentralPackageVersions.TryTake(out LibraryDependency centralPackageVersionTake1);
            // nothing more to take
            bool resultTake2 = transitiveCentralPackageVersions.TryTake(out LibraryDependency centralPackageVersionTake2);

            // Assert
            Assert.True(resultTake1);
            Assert.False(resultTake2);

            Assert.Equal(centralPackageVersionDependency, centralPackageVersionTake1);
            Assert.Null(centralPackageVersionTake2);
        }

        [Fact]
        public void TransitiveCentralPackageVersions_TryAdd_MultipleParents()
        {
            // Arrange
            var transitiveCentralPackageVersions = new RemoteDependencyWalker.TransitiveCentralPackageVersions();
            var centralLibraryRange = new LibraryRange("name1", VersionRange.Parse("1.0.0"), LibraryDependencyTarget.Package);
            var centralPackageVersionDependency = new LibraryDependency()
            {
                LibraryRange = centralLibraryRange,
            };
            var parent1 = new GraphNode<RemoteResolveResult>(new LibraryRange("parentname1", VersionRange.Parse("1.0.0"), LibraryDependencyTarget.Package));
            var parent2 = new GraphNode<RemoteResolveResult>(new LibraryRange("parentname2", VersionRange.Parse("1.0.0"), LibraryDependencyTarget.Package));
            var centralNode = new GraphNode<RemoteResolveResult>(centralPackageVersionDependency.LibraryRange)
            {
                Item = new GraphItem<RemoteResolveResult>(new LibraryIdentity(centralPackageVersionDependency.LibraryRange.Name, NuGetVersion.Parse("1.0.0"), LibraryType.Package))
            };

            transitiveCentralPackageVersions.Add(centralPackageVersionDependency, parent1);
            transitiveCentralPackageVersions.Add(centralPackageVersionDependency, parent2);
            transitiveCentralPackageVersions.AddParentsToNode(centralNode);

            // Assert
            bool resultTake1 = transitiveCentralPackageVersions.TryTake(out LibraryDependency centralPackageVersionTake1);
            bool resultTake2 = transitiveCentralPackageVersions.TryTake(out LibraryDependency centralPackageVersionTake2);

            Assert.True(resultTake1);
            Assert.False(resultTake2);
            Assert.Equal(2, centralNode.ParentNodes.Count);
            var parentLibraryNames = centralNode.ParentNodes.Select(p => p.Key.Name).OrderBy(n => n).ToArray();
            Assert.Equal("parentname1", parentLibraryNames[0]);
            Assert.Equal("parentname2", parentLibraryNames[1]);
        }

        [Theory]
        [InlineData("2.0.0", "2.0.0")]
        [InlineData("2.0.0", "1.0.0")]

        public async Task WalkAsyncAddsTransitiveCentralDependency(string centralPackageVersion, string otherVersion)
        {
            var centralPackageName = "D";
            var framework = NuGetFramework.Parse("net45");

            var context = new TestRemoteWalkContext();
            var provider = new DependencyProvider();
            // D is a transitive dependency for package A through package B -> C -> D
            // D is defined as a Central Package Version
            // In this context Package D with version centralPackageVersion will be added as inner node of Node A, next to B

            // Input
            // A -> B (version = otherVersion) -> C (version = otherVersion) -> D (version = otherVersion)
            // A ~> D (the version 2.0.0 or as defined by "centralPackageVersion" argument
            //         the dependency is not direct,
            //         it simulates the fact that there is a centrally defined "D" package
            //         the information is added to the provider)

            // The expected output graph
            //    -> B (version = otherVersion) -> C (version = otherVersion)
            // A
            //    -> D (version = 2.0.0)

            provider.Package("A", otherVersion)
                    .DependsOn("B", otherVersion);

            provider.Package("B", otherVersion)
                   .DependsOn("C", otherVersion);

            provider.Package("C", otherVersion)
                  .DependsOn(centralPackageName, otherVersion);

            // Simulates the existence of a D centrally defined package that is not direct dependency
            provider.Package("A", otherVersion)
                     .DependsOn(centralPackageName, centralPackageVersion, LibraryDependencyTarget.Package, versionCentrallyManaged: true, libraryDependencyReferenceType: LibraryDependencyReferenceType.None);

            // Add central package to the source with multiple versions
            provider.Package(centralPackageName, "1.0.0");
            provider.Package(centralPackageName, centralPackageVersion);
            provider.Package(centralPackageName, "3.0.0");

            context.LocalLibraryProviders.Add(provider);
            var walker = new RemoteDependencyWalker(context);

            // Act
            var rootNode = await DoWalkAsync(walker, "A", framework);

            // Assert
            Assert.Equal(2, rootNode.InnerNodes.Count);
            var centralVersionInGraphNode = rootNode.InnerNodes.Where(n => n.Item.Key.Name == centralPackageName).FirstOrDefault();
            Assert.NotNull(centralVersionInGraphNode);
            Assert.Equal(centralPackageVersion, centralVersionInGraphNode.Item.Key.Version.ToNormalizedString());
            Assert.True(centralVersionInGraphNode.Item.IsCentralTransitive);

            var BNode = rootNode.InnerNodes.Where(n => n.Item.Key.Name == "B").FirstOrDefault();
            Assert.NotNull(BNode);
            Assert.Equal(1, BNode.InnerNodes.Count);
            Assert.Equal(otherVersion, BNode.Item.Key.Version.ToNormalizedString());
            Assert.False(BNode.Item.IsCentralTransitive);

            var CNode = BNode.InnerNodes.Where(n => n.Item.Key.Name == "C").FirstOrDefault();
            Assert.NotNull(CNode);
            Assert.Equal(otherVersion, CNode.Item.Key.Version.ToNormalizedString());
            Assert.Equal(0, CNode.InnerNodes.Count);
            Assert.False(CNode.Item.IsCentralTransitive);
        }

        [Fact]
        public async Task WalkAsyncDowngradesBecauseOfCentralDependency()
        {
            var centralPackageName = "D";
            var centralPackageVersion = "2.0.0";
            var otherVersion = "3.0.0";
            var framework = NuGetFramework.Parse("net45");

            var context = new TestRemoteWalkContext();
            var provider = new DependencyProvider();
            // D is a transitive dependency for package A through package B -> C -> D
            // D is defined as a Central Package Version
            // In this context Package D with version centralPackageVersion will be added as inner node of Node A, next to B

            // Picture:
            // A -> B -> C -> D (version 1.0.0 or 2.0.0 as defined by "otherVersion" argument.
            // A ~> D (the version 2.0.0 or as defined by "centralPackageVersion" argument
            //         the dependency is not direct,
            //         it simulates the fact that there is a centrally defined "D" package
            //         the information is added to the provider)

            provider.Package("A", otherVersion)
                    .DependsOn("B", otherVersion);

            provider.Package("B", otherVersion)
                   .DependsOn("C", otherVersion);

            provider.Package("C", otherVersion)
                  .DependsOn(centralPackageName, otherVersion);

            // Simulates the existence of a D centrally defined package that is not direct dependency
            provider.Package("A", otherVersion)
                     .DependsOn(centralPackageName, centralPackageVersion, LibraryDependencyTarget.Package, versionCentrallyManaged: true);

            // Add central package to the source with multiple versions
            provider.Package(centralPackageName, "1.0.0");
            provider.Package(centralPackageName, centralPackageVersion);
            provider.Package(centralPackageName, "3.0.0");

            context.LocalLibraryProviders.Add(provider);
            var walker = new RemoteDependencyWalker(context);

            // Act
            var rootNode = await DoWalkAsync(walker, "A", framework);

            var analyzeResult = rootNode.Analyze();

            // Assert
            Assert.Equal(1, analyzeResult.Downgrades.Count);
            var downgrade = analyzeResult.Downgrades.First();
            Assert.Equal(centralPackageName, downgrade.DowngradedFrom.Key.Name);
            Assert.Equal("[3.0.0, )", downgrade.DowngradedFrom.Key.VersionRange.ToNormalizedString());
            Assert.Equal(centralPackageName, downgrade.DowngradedTo.Key.Name);
            Assert.Equal("[2.0.0, )", downgrade.DowngradedTo.Key.VersionRange.ToNormalizedString());
        }

        [Theory]
        [InlineData(LibraryDependencyReferenceType.Direct, true)]
        [InlineData(LibraryDependencyReferenceType.Transitive, true)]
        [InlineData(LibraryDependencyReferenceType.None, true)]
        [InlineData(LibraryDependencyReferenceType.Direct, false)]
        [InlineData(LibraryDependencyReferenceType.Transitive, false)]
        [InlineData(LibraryDependencyReferenceType.None, false)]
        public void IsDependencyValidForGraphTest(LibraryDependencyReferenceType referenceType, bool versionCentrallyManaged)
        {
            var centralPackageName = "D";
            var context = new TestRemoteWalkContext();
            var centralPackageVersion = new CentralPackageVersion(centralPackageName, VersionRange.Parse("2.0.0"));
            var centralPackageVersionDependency_VersionCentrallyManaged = new LibraryDependency()
            {
                LibraryRange = new LibraryRange(centralPackageVersion.Name, centralPackageVersion.VersionRange, LibraryDependencyTarget.Package),
                VersionCentrallyManaged = versionCentrallyManaged,
                ReferenceType = referenceType,
            };
            var walker = new RemoteDependencyWalker(context);

            // Act
            var expectedResult = walker.IsDependencyValidForGraph(centralPackageVersionDependency_VersionCentrallyManaged);

            // Assert
            if (referenceType != LibraryDependencyReferenceType.None)
            {
                Assert.True(expectedResult);
            }
            else
            {
                Assert.False(expectedResult);
            }
        }

        [Fact]
        public async Task WalkAsync_CentralTransitiveDependencyList_DoesNotHaveDuplicates()
        {
            var framework = NuGetFramework.Parse("net45");
            var context = new TestRemoteWalkContext();
            var provider = new DependencyProvider();

            // A -> centralPackage1
            //   -> centralPackage2 -> centralPackage1
            provider.Package("A", "1.0.0")
                    .DependsOn("centralPackage1", "1.0.0", target: LibraryDependencyTarget.Package, versionCentrallyManaged: true);
            provider.Package("A", "1.0.0")
                    .DependsOn("centralPackage2", "1.0.0", target: LibraryDependencyTarget.Package, versionCentrallyManaged: true);
            provider.Package("centralPackage2", "1.0.0")
                   .DependsOn("centralPackage1", "1.0.0");

            // A -> projectB -> projectC -> centralPackage1
            provider.Package("A", "1.0.0")
                   .DependsOn("B", "1.0.0");

            provider.Package("B", "1.0.0")
                   .DependsOn("C", "1.0.0");

            provider.Package("C", "1.0.0")
                  .DependsOn("centralPackage1", "1.0.0", target: LibraryDependencyTarget.Package, versionCentrallyManaged: true);

            // B ~> centralPackage1
            provider.Package("B", "1.0.0")
                   .DependsOn("centralPackage1", "1.0.0", target: LibraryDependencyTarget.Package, versionCentrallyManaged: true, libraryDependencyReferenceType: LibraryDependencyReferenceType.None);

            provider.Package("centralPackage1", "1.0.0");
            provider.Package("centralPackage2", "1.0.0");

            context.LocalLibraryProviders.Add(provider);
            var walker = new RemoteDependencyWalker(context);

            // Act
            var rootNode = await DoWalkAsync(walker, "A", framework);

            // Assert
            Assert.Equal(3, rootNode.InnerNodes.Count);
        }

        /// <summary>
        /// A -> B 1.0.0 -> C 1.0.0(this will be rejected) -> D 1.0.0 -> E 1.0.0
        ///   -> F 1.0.0 -> C 2.0.0 -> H 2.0.0
        ///   -> G 1.0.0 -> H 1.0.0(this will be rejected) -> D 1.0.0
        ///
        ///  D has version defined centrally 2.0.0
        ///  D 2.0.0 -> I 2.0.0
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task CentralTransitiveDependency_IsRejected_If_ParentsAreRejected()
        {
            var framework = NuGetFramework.Parse("net45");

            var context = new TestRemoteWalkContext();
            var provider = new DependencyProvider();
            var version100 = "1.0.0";
            var version200 = "2.0.0";

            // A -> B 1.0.0 -> C 1.0.0(this will be rejected) -> D 1.0.0 -> E 1.0.0
            provider.Package("A", version100)
                    .DependsOn("B", version100);
            provider.Package("B", version100)
                   .DependsOn("C", version100);
            provider.Package("C", version100)
                    .DependsOn("D", version100);
            provider.Package("D", version100)
                    .DependsOn("E", version100);

            // A -> F 1.0.0 -> C 2.0.0 -> H 2.0.0
            provider.Package("A", version100)
                    .DependsOn("F", version100);
            provider.Package("F", version100)
                    .DependsOn("C", version200);
            provider.Package("C", version200)
                    .DependsOn("H", version200);
            // add H 2.0.0 to the feed
            provider.Package("H", version200);

            // A -> -> G 1.0.0 -> H 1.0.0(this will be rejected) -> D 1.0.0
            provider.Package("A", version100)
                    .DependsOn("G", version100);
            provider.Package("G", version100)
                    .DependsOn("H", version100);
            provider.Package("H", version100)
                    .DependsOn("D", version100);

            // D 2.0.0 -> I 2.0.0
            provider.Package("D", version200)
                   .DependsOn("I", version200);
            provider.Package("I", version200);

            // Simulates the existence of a D centrally defined package that is not direct dependency
            provider.Package("A", version100)
                     .DependsOn("D", version200, LibraryDependencyTarget.Package, versionCentrallyManaged: true, libraryDependencyReferenceType: LibraryDependencyReferenceType.None);

            context.LocalLibraryProviders.Add(provider);
            var walker = new RemoteDependencyWalker(context);

            // Act
            var rootNode = await DoWalkAsync(walker, "A", framework);

            // Assert
            Assert.Equal(4, rootNode.InnerNodes.Count);

            AnalyzeResult<RemoteResolveResult> result = rootNode.Analyze();

            var centralTransitiveNodes = rootNode.InnerNodes.Where(n => n.Item.IsCentralTransitive).ToList();
            Assert.Equal(1, centralTransitiveNodes.Count);
            Assert.Equal(Disposition.Rejected, centralTransitiveNodes.First().Disposition);
            centralTransitiveNodes.First().ForEach((n) => { Assert.Equal(Disposition.Rejected, n.Disposition); });

            var notCentralTransitiveNodes = rootNode.InnerNodes.Where(n => !n.Item.IsCentralTransitive).ToList();
            Assert.Equal(3, notCentralTransitiveNodes.Count);
            foreach (var node in notCentralTransitiveNodes)
            {
                Assert.Equal(Disposition.Accepted, node.Disposition);
                node.ForEach((n) =>
                {
                    if ((n.Key.Name == "C" && n.Key.VersionRange.OriginalString == "1.0.0") ||
                        (n.Key.Name == "H" && n.Key.VersionRange.OriginalString == "1.0.0"))
                    {
                        Assert.Equal(Disposition.Rejected, n.Disposition);
                    }
                    else
                    {
                        Assert.Equal(Disposition.Accepted, n.Disposition);
                    }
                });
            }

            Assert.Equal(0, result.Downgrades.Count);
        }

        /// <summary>
        /// A -> B 1.0.0 -> C 1.0.0(this will be rejected) -> D 1.0.0 -> E 1.0.0
        ///   -> F 1.0.0 -> C 2.0.0 -> H 2.0.0
        ///   -> G 1.0.0 -> H 1.0.0(this will be rejected) -> D 1.0.0
        ///
        ///  D has version defined centrally 2.0.0 and E is centrally as well with version 3.0.0
        ///  and
        ///  D 2.0.0 -> I 2.0.0 -> E 2.0.0
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task CentralTransitiveDependency_IsRejected_If_ParentsAreRejected_TwoLevelCentralDependencies()
        {
            var framework = NuGetFramework.Parse("net45");

            var context = new TestRemoteWalkContext();
            var provider = new DependencyProvider();
            var version100 = "1.0.0";
            var version200 = "2.0.0";
            var version300 = "3.0.0";

            // A -> B 1.0.0 -> C 1.0.0(this will be rejected) -> D 1.0.0 -> E 1.0.0
            provider.Package("A", version100)
                    .DependsOn("B", version100);
            provider.Package("B", version100)
                   .DependsOn("C", version100);
            provider.Package("C", version100)
                    .DependsOn("D", version100);
            provider.Package("D", version100)
                    .DependsOn("E", version100);

            // A -> F 1.0.0 -> C 2.0.0 -> H 2.0.0
            provider.Package("A", version100)
                    .DependsOn("F", version100);
            provider.Package("F", version100)
                    .DependsOn("C", version200);
            provider.Package("C", version200)
                    .DependsOn("H", version200);

            // A -> -> G 1.0.0 -> H 1.0.0(this will be rejected) -> D 1.0.0
            provider.Package("A", version100)
                    .DependsOn("G", version100);
            provider.Package("G", version100)
                    .DependsOn("H", version100);
            provider.Package("H", version100)
                    .DependsOn("D", version100);

            // D 2.0.0 -> I 2.0.0 -> E 2.0.0
            provider.Package("D", version200)
                   .DependsOn("I", version200);
            provider.Package("I", version200)
                   .DependsOn("E", version200);

            // Simulates the existence of a D centrally defined package that is not direct dependency
            provider.Package("A", version100)
                     .DependsOn("D", version200, LibraryDependencyTarget.Package, versionCentrallyManaged: true, libraryDependencyReferenceType: LibraryDependencyReferenceType.None);
            provider.Package("A", version100)
                     .DependsOn("E", version300, LibraryDependencyTarget.Package, versionCentrallyManaged: true, libraryDependencyReferenceType: LibraryDependencyReferenceType.None);

            context.LocalLibraryProviders.Add(provider);
            var walker = new RemoteDependencyWalker(context);

            // Act
            var rootNode = await DoWalkAsync(walker, "A", framework);

            // Assert
            // Expected B,D, E,F,G
            var innerNodes = rootNode.InnerNodes.OrderBy(n => n.Key.Name).ToArray();
            Assert.Equal(5, innerNodes.Length);
            Assert.Equal("B", innerNodes[0].Key.Name);
            Assert.Equal("D", innerNodes[1].Key.Name);
            Assert.Equal("E", innerNodes[2].Key.Name);
            Assert.Equal("F", innerNodes[3].Key.Name);
            Assert.Equal("G", innerNodes[4].Key.Name);

            Assert.Equal(version100, innerNodes[0].Key.VersionRange.OriginalString);
            Assert.Equal(version200, innerNodes[1].Key.VersionRange.OriginalString);
            Assert.Equal(version300, innerNodes[2].Key.VersionRange.OriginalString);
            Assert.Equal(version100, innerNodes[3].Key.VersionRange.OriginalString);
            Assert.Equal(version100, innerNodes[4].Key.VersionRange.OriginalString);

            AnalyzeResult<RemoteResolveResult> result = rootNode.Analyze();

            var centralTransitiveNodes = rootNode.InnerNodes.Where(n => n.Item.IsCentralTransitive).ToList();
            Assert.Equal(2, centralTransitiveNodes.Count);
            foreach (var node in centralTransitiveNodes)
            {
                Assert.Equal(Disposition.Rejected, node.Disposition);
                node.ForEach((n) => { Assert.Equal(Disposition.Rejected, n.Disposition); });
            }

            var notCentralTransitiveNodes = rootNode.InnerNodes.Where(n => !n.Item.IsCentralTransitive).ToList();
            Assert.Equal(3, notCentralTransitiveNodes.Count);
            foreach (var node in notCentralTransitiveNodes)
            {
                Assert.Equal(Disposition.Accepted, node.Disposition);
                node.ForEach((n) =>
                {
                    if ((n.Key.Name == "C" && n.Key.VersionRange.OriginalString == "1.0.0") ||
                        (n.Key.Name == "H" && n.Key.VersionRange.OriginalString == "1.0.0"))
                    {
                        Assert.Equal(Disposition.Rejected, n.Disposition);
                    }
                    else
                    {
                        Assert.Equal(Disposition.Accepted, n.Disposition);
                    }
                });
            }

            Assert.Equal(0, result.Downgrades.Count);
        }

        /// <summary>
        /// A -> D 1.0.0 -> E 1.0.0(this will be rejected) -> B 1.0.0
        ///   -> F 1.0.0 -> G 1.0.0(this will be rejected) -> C 1.0.0
        ///   -> H 2.0.0 -> E 2.0.0
        ///   -> I 2.0.0 -> G 2.0.0
        ///
        ///  B and C has version 2.0.0 defined centrally
        ///   C 2.0.0 -> J 2.0.0 (Extra dependency not defined centrally) -> B 2.0.0
        ///   B 2.0.0
        /// </summary>
        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task CentralTransitiveDependency_IsRejected_If_CentralTransitiveParentsAreRejected(bool extraDependency)
        {
            var framework = NuGetFramework.Parse("net45");

            var context = new TestRemoteWalkContext();
            var provider = new DependencyProvider();
            var v1 = "1.0.0";
            var v2 = "2.0.0";

            provider.Package("A", v1).DependsOn("D", v1);
            provider.Package("A", v1).DependsOn("F", v1);
            provider.Package("A", v1).DependsOn("H", v2);
            provider.Package("A", v1).DependsOn("I", v2);
            provider.Package("D", v1).DependsOn("E", v1);
            provider.Package("E", v1).DependsOn("B", v1);
            provider.Package("F", v1).DependsOn("G", v1);
            provider.Package("G", v1).DependsOn("C", v1);
            provider.Package("H", v2).DependsOn("E", v2);
            provider.Package("I", v2).DependsOn("G", v2);
            provider.Package("C", v1);
            provider.Package("B", v1);
            provider.Package("E", v2);
            provider.Package("G", v2);

            if (extraDependency)
            {
                provider.Package("C", v2).DependsOn("J", v2);
                provider.Package("J", v2).DependsOn("B", v2);
            }
            else
            {
                provider.Package("C", v2).DependsOn("B", v2);
            }

            provider.Package("B", v2);


            provider.Package("A", v1)
                .DependsOn("C", v2, LibraryDependencyTarget.Package, versionCentrallyManaged: true, libraryDependencyReferenceType: LibraryDependencyReferenceType.None);

            provider.Package("A", v1)
                .DependsOn("B", v2, LibraryDependencyTarget.Package, versionCentrallyManaged: true, libraryDependencyReferenceType: LibraryDependencyReferenceType.None);

            context.LocalLibraryProviders.Add(provider);
            var walker = new RemoteDependencyWalker(context);

            // Act
            var rootNode = await DoWalkAsync(walker, "A", framework);

            // Assert
            AnalyzeResult<RemoteResolveResult> result = rootNode.Analyze();

            var centralTransitiveNodes = rootNode.InnerNodes.Where(n => n.Item.IsCentralTransitive).ToList();
            Assert.Equal(2, centralTransitiveNodes.Count);
            Assert.Equal(Disposition.Rejected, centralTransitiveNodes.First().Disposition);
            Assert.Equal(Disposition.Rejected, centralTransitiveNodes.Last().Disposition);

            rootNode.ForEach((n) =>
            {
                if (n.Key.Name == "A" ||
                    n.Key.Name == "D" && n.Key.VersionRange.OriginalString == v1 ||
                    n.Key.Name == "F" && n.Key.VersionRange.OriginalString == v1 ||
                    n.Key.Name == "E" && n.Key.VersionRange.OriginalString == v2 ||
                    n.Key.Name == "G" && n.Key.VersionRange.OriginalString == v2 ||
                    n.Key.Name == "H" && n.Key.VersionRange.OriginalString == v2 ||
                    n.Key.Name == "I" && n.Key.VersionRange.OriginalString == v2)
                {
                    Assert.Equal(Disposition.Accepted, n.Disposition);
                }
                else if (n.Key.Name == "C" && n.Key.VersionRange.OriginalString == v2 ||
                    n.Key.Name == "J" && n.Key.VersionRange.OriginalString == v2 ||
                    n.Key.Name == "B" && n.Key.VersionRange.OriginalString == v2 ||
                    n.Key.Name == "E" && n.Key.VersionRange.OriginalString == v1 ||
                    n.Key.Name == "G" && n.Key.VersionRange.OriginalString == v1)
                {
                    Assert.Equal(Disposition.Rejected, n.Disposition);
                }
                else
                {
                    Assert.Fail(n.Key.ToString());
                }
            });
        }

        /// <summary>
        /// A -> B 1.0.0 -> C 1.0.0(this will be rejected) -> D 1.0.0 -> E 1.0.0
        ///   -> F 1.0.0 -> C 2.0.0 -> H 2.0.0
        ///   -> G 1.0.0 -> H 2.0.0(this will not be rejected) -> D 1.0.0
        ///
        ///  D has version defined centrally 2.0.0
        ///  D 2.0.0 -> I 2.0.0
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task CentralTransitiveDependency_IsNotRejected_If_NotAllParentsAreRejected()
        {
            var framework = NuGetFramework.Parse("net45");

            var context = new TestRemoteWalkContext();
            var provider = new DependencyProvider();
            var version100 = "1.0.0";
            var version200 = "2.0.0";

            // A -> B 1.0.0 -> C 1.0.0(this will be rejected) -> D 1.0.0 -> E 1.0.0
            provider.Package("A", version100)
                    .DependsOn("B", version100);
            provider.Package("B", version100)
                   .DependsOn("C", version100);
            provider.Package("C", version100)
                    .DependsOn("D", version100);
            provider.Package("D", version100)
                    .DependsOn("E", version100);

            // A -> F 1.0.0 -> C 2.0.0 -> H 2.0.0
            provider.Package("A", version100)
                    .DependsOn("F", version100);
            provider.Package("F", version100)
                    .DependsOn("C", version200);
            provider.Package("C", version200)
                    .DependsOn("H", version200);

            // A -> -> G 1.0.0 -> H 2.0.0(this will not be rejected) -> D 1.0.0
            provider.Package("A", version100)
                    .DependsOn("G", version100);
            provider.Package("G", version100)
                    .DependsOn("H", version200);
            provider.Package("H", version200)
                    .DependsOn("D", version100);

            // D 2.0.0 -> I 2.0.0
            provider.Package("D", version200)
                   .DependsOn("I", version200);

            // Simulates the existence of a D centrally defined package that is not direct dependency
            provider.Package("A", version100)
                     .DependsOn("D", version200, LibraryDependencyTarget.Package, versionCentrallyManaged: true, libraryDependencyReferenceType: LibraryDependencyReferenceType.None);

            context.LocalLibraryProviders.Add(provider);
            var walker = new RemoteDependencyWalker(context);

            // Act
            var rootNode = await DoWalkAsync(walker, "A", framework);

            // Assert
            Assert.Equal(4, rootNode.InnerNodes.Count);

            AnalyzeResult<RemoteResolveResult> result = rootNode.Analyze();

            var centralTransitiveNodes = rootNode.InnerNodes.Where(n => n.Item.IsCentralTransitive).ToList();
            Assert.Equal(1, centralTransitiveNodes.Count);
            Assert.Equal(Disposition.Accepted, centralTransitiveNodes.First().Disposition);
            Assert.Equal(1, centralTransitiveNodes.First().InnerNodes.Count);
            centralTransitiveNodes.First().ForEach((n) => { Assert.Equal(Disposition.Accepted, n.Disposition); });

            var notCentralTransitiveNodes = rootNode.InnerNodes.Where(n => !n.Item.IsCentralTransitive).ToList();
            Assert.Equal(3, notCentralTransitiveNodes.Count);
            foreach (var node in notCentralTransitiveNodes)
            {
                Assert.Equal(Disposition.Accepted, node.Disposition);
                node.ForEach((n) =>
                {
                    if ((n.Key.Name == "C" && n.Key.VersionRange.OriginalString == "1.0.0"))
                    {
                        Assert.Equal(Disposition.Rejected, n.Disposition);
                    }
                    else
                    {
                        Assert.Equal(Disposition.Accepted, n.Disposition);
                    }
                });
            }

            Assert.Equal(0, result.Downgrades.Count);
        }

        /// <summary>
        /// A -> B 1.0.0 -> C 1.0.0(this will be rejected) -> D 2.0.0(this will be downgraded due to central 1.0.0) -> E 1.0.0
        ///   -> F 1.0.0 -> C 2.0.0 -> H 2.0.0
        ///   -> G 1.0.0 -> H 1.0.0(this will be rejected) -> D 1.0.0
        ///
        ///  D has version defined centrally 1.0.0
        ///  D 1.0.0 -> I 1.0.0
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task CentralTransitiveDependency_NoDowngrades_IfDowngrades_WereRejected()
        {
            var framework = NuGetFramework.Parse("net45");

            var context = new TestRemoteWalkContext();
            var provider = new DependencyProvider();
            var version100 = "1.0.0";
            var version200 = "2.0.0";

            // A -> B 1.0.0 -> C 1.0.0(this will be rejected) -> D 2.0.0 (this will be downgraded) -> E 1.0.0
            provider.Package("A", version100)
                    .DependsOn("B", version100);
            provider.Package("B", version100)
                   .DependsOn("C", version100);
            provider.Package("C", version100)
                    .DependsOn("D", version200);
            provider.Package("D", version200)
                    .DependsOn("E", version100);

            // A -> F 1.0.0 -> C 2.0.0 -> H 2.0.0
            provider.Package("A", version100)
                    .DependsOn("F", version100);
            provider.Package("F", version100)
                    .DependsOn("C", version200);
            provider.Package("C", version200)
                    .DependsOn("H", version200);

            // A -> -> G 1.0.0 -> H 1.0.0(this will be rejected) -> D 1.0.0
            provider.Package("A", version100)
                    .DependsOn("G", version100);
            provider.Package("G", version100)
                    .DependsOn("H", version100);
            provider.Package("H", version100)
                    .DependsOn("D", version100);

            // D 1.0.0 -> I 1.0.0
            provider.Package("D", version100)
                   .DependsOn("I", version100);

            // Simulates the existence of a D centrally defined package that is not direct dependency
            provider.Package("A", version100)
                     .DependsOn("D", version100, LibraryDependencyTarget.Package, versionCentrallyManaged: true, libraryDependencyReferenceType: LibraryDependencyReferenceType.None);

            context.LocalLibraryProviders.Add(provider);
            var walker = new RemoteDependencyWalker(context);

            // Act
            var rootNode = await DoWalkAsync(walker, "A", framework);

            // Assert
            Assert.Equal(4, rootNode.InnerNodes.Count);

            AnalyzeResult<RemoteResolveResult> result = rootNode.Analyze();

            var centralTransitiveNodes = rootNode.InnerNodes.Where(n => n.Item.IsCentralTransitive).ToList();
            Assert.Equal(1, centralTransitiveNodes.Count);
            Assert.Equal(Disposition.Rejected, centralTransitiveNodes.First().Disposition);
            Assert.Equal(1, centralTransitiveNodes.First().InnerNodes.Count);
            centralTransitiveNodes.First().ForEach((n) => { Assert.Equal(Disposition.Rejected, n.Disposition); });

            var notCentralTransitiveNodes = rootNode.InnerNodes.Where(n => !n.Item.IsCentralTransitive).ToList();
            Assert.Equal(3, notCentralTransitiveNodes.Count);
            foreach (var node in notCentralTransitiveNodes)
            {
                Assert.Equal(Disposition.Accepted, node.Disposition);
                node.ForEach((n) =>
                {
                    // No D node expected in the graph
                    // all downgrades shoudl have been removed
                    Assert.True(n.Key.Name != "D");

                    if ((n.Key.Name == "C" && n.Key.VersionRange.OriginalString == "1.0.0") ||
                        (n.Key.Name == "H" && n.Key.VersionRange.OriginalString == "1.0.0"))
                    {
                        Assert.Equal(Disposition.Rejected, n.Disposition);
                    }
                    else
                    {
                        Assert.Equal(Disposition.Accepted, n.Disposition);
                    }
                });
            }

            Assert.Equal(0, result.Downgrades.Count);
        }

        /// <summary>
        /// A -> B 1.0.0 -> C 1.0.0(this will be rejected) -> D 2.0.0(this will be downgraded due to central 1.0.0) -> E 1.0.0
        ///   -> F 1.0.0 -> C 2.0.0 -> H 2.0.0
        ///   -> G 1.0.0 -> H 1.0.0(this will be rejected) -> D 2.0.0 (it will be downgraded)
        ///
        ///  D has version defined centrally 1.0.0
        ///  D 1.0.0 -> I 1.0.0
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task CentralTransitiveDependency_NoDowngrades_IfTwoDowngrades_WereRejected()
        {
            var framework = NuGetFramework.Parse("net45");

            var context = new TestRemoteWalkContext();
            var provider = new DependencyProvider();
            var version100 = "1.0.0";
            var version200 = "2.0.0";

            // A -> B 1.0.0 -> C 1.0.0(this will be rejected) -> D 2.0.0 (this will be downgraded) -> E 1.0.0
            provider.Package("A", version100)
                    .DependsOn("B", version100);
            provider.Package("B", version100)
                   .DependsOn("C", version100);
            provider.Package("C", version100)
                    .DependsOn("D", version200);
            provider.Package("D", version200)
                    .DependsOn("E", version100);

            // A -> F 1.0.0 -> C 2.0.0 -> H 2.0.0
            provider.Package("A", version100)
                    .DependsOn("F", version100);
            provider.Package("F", version100)
                    .DependsOn("C", version200);
            provider.Package("C", version200)
                    .DependsOn("H", version200);

            // A -> -> G 1.0.0 -> H 1.0.0(this will be rejected) -> D 2.0.0 (it will be downgraded)
            provider.Package("A", version100)
                    .DependsOn("G", version100);
            provider.Package("G", version100)
                    .DependsOn("H", version100);
            provider.Package("H", version100)
                    .DependsOn("D", version200);

            // D 1.0.0 -> I 1.0.0
            provider.Package("D", version100)
                   .DependsOn("I", version100);

            // Simulates the existence of a D centrally defined package that is not direct dependency
            provider.Package("A", version100)
                     .DependsOn("D", version100, LibraryDependencyTarget.Package, versionCentrallyManaged: true, libraryDependencyReferenceType: LibraryDependencyReferenceType.None);

            context.LocalLibraryProviders.Add(provider);
            var walker = new RemoteDependencyWalker(context);

            // Act
            var rootNode = await DoWalkAsync(walker, "A", framework);

            // Assert
            Assert.Equal(4, rootNode.InnerNodes.Count);

            AnalyzeResult<RemoteResolveResult> result = rootNode.Analyze();

            var centralTransitiveNodes = rootNode.InnerNodes.Where(n => n.Item.IsCentralTransitive).ToList();
            Assert.Equal(1, centralTransitiveNodes.Count);
            Assert.Equal(Disposition.Rejected, centralTransitiveNodes.First().Disposition);
            Assert.Equal(1, centralTransitiveNodes.First().InnerNodes.Count);
            centralTransitiveNodes.First().ForEach((n) => { Assert.Equal(Disposition.Rejected, n.Disposition); });

            var notCentralTransitiveNodes = rootNode.InnerNodes.Where(n => !n.Item.IsCentralTransitive).ToList();
            Assert.Equal(3, notCentralTransitiveNodes.Count);
            foreach (var node in notCentralTransitiveNodes)
            {
                Assert.Equal(Disposition.Accepted, node.Disposition);
                node.ForEach((n) =>
                {
                    // No D node expected in the graph
                    // all downgrades should have been removed
                    Assert.True(n.Key.Name != "D");

                    if ((n.Key.Name == "C" && n.Key.VersionRange.OriginalString == "1.0.0") ||
                        (n.Key.Name == "H" && n.Key.VersionRange.OriginalString == "1.0.0"))
                    {
                        Assert.Equal(Disposition.Rejected, n.Disposition);
                    }
                    else
                    {
                        Assert.Equal(Disposition.Accepted, n.Disposition);
                    }
                });
            }

            Assert.Equal(0, result.Downgrades.Count);
        }

        /// <summary>
        /// A -> B 1.0.0 -> C 1.0.0(this will be rejected) -> D 2.0.0(this will be downgraded due to central 1.0.0) -> E 1.0.0
        ///   -> F 1.0.0 -> C 2.0.0
        ///   -> G 1.0.0 -> H 2.0.0(this will not be rejected) -> D 2.0.0 (this will be downgraded due to central 1.0.0)
        ///
        ///  D has version defined centrally 1.0.0
        ///  D 1.0.0 -> I 1.0.0
        /// </summary>
        [Fact]
        public async Task CentralTransitiveDependency_Downgrades_IfDowngrades_WereNotRejected()
        {
            var framework = NuGetFramework.Parse("net45");

            var context = new TestRemoteWalkContext();
            var provider = new DependencyProvider();
            var version100 = "1.0.0";
            var version200 = "2.0.0";

            // A -> B 1.0.0 -> C 1.0.0(this will be rejected) -> D 2.0.0 (this will be downgraded) -> E 1.0.0
            provider.Package("A", version100)
                    .DependsOn("B", version100);
            provider.Package("B", version100)
                   .DependsOn("C", version100);
            provider.Package("C", version100)
                    .DependsOn("D", version200);
            provider.Package("D", version200)
                    .DependsOn("E", version100);

            // A -> F 1.0.0 -> C 2.0.0 -> H 2.0.0
            provider.Package("A", version100)
                    .DependsOn("F", version100);
            provider.Package("F", version100)
                    .DependsOn("C", version200);

            // A -> -> G 1.0.0 -> H 2.0.0(this will not be rejected) -> D 2.0.0 (this will be downgraded)
            provider.Package("A", version100)
                    .DependsOn("G", version100);
            provider.Package("G", version100)
                    .DependsOn("H", version200);
            provider.Package("H", version200)
                    .DependsOn("D", version200);

            // D 1.0.0 -> I 1.0.0
            provider.Package("D", version100)
                   .DependsOn("I", version100);

            // Simulates the existence of a D centrally defined package that is not direct dependency
            provider.Package("A", version100)
                     .DependsOn("D", version100, LibraryDependencyTarget.Package, versionCentrallyManaged: true, libraryDependencyReferenceType: LibraryDependencyReferenceType.None);

            context.LocalLibraryProviders.Add(provider);
            var walker = new RemoteDependencyWalker(context);

            // Act
            var rootNode = await DoWalkAsync(walker, "A", framework);

            // Assert
            Assert.Equal(4, rootNode.InnerNodes.Count);

            AnalyzeResult<RemoteResolveResult> result = rootNode.Analyze();

            var centralTransitiveNodes = rootNode.InnerNodes.Where(n => n.Item.IsCentralTransitive).ToList();
            Assert.Equal(1, centralTransitiveNodes.Count);
            Assert.Equal(Disposition.Accepted, centralTransitiveNodes.First().Disposition);
            Assert.Equal(1, centralTransitiveNodes.First().InnerNodes.Count);
            centralTransitiveNodes.First().ForEach((n) => { Assert.Equal(Disposition.Accepted, n.Disposition); });

            var notCentralTransitiveNodes = rootNode.InnerNodes.Where(n => !n.Item.IsCentralTransitive).ToList();
            Assert.Equal(3, notCentralTransitiveNodes.Count);
            foreach (var node in notCentralTransitiveNodes)
            {
                Assert.Equal(Disposition.Accepted, node.Disposition);
                node.ForEach((n) =>
                {
                    // No D node expected in the graph
                    // all downgrades should have been removed
                    Assert.True(n.Key.Name != "D");

                    if ((n.Key.Name == "C" && n.Key.VersionRange.OriginalString == "1.0.0"))
                    {
                        Assert.Equal(Disposition.Rejected, n.Disposition);
                    }
                    else
                    {
                        Assert.Equal(Disposition.Accepted, n.Disposition);
                    }
                });
            }

            var downgrades = result.Downgrades;
            Assert.Equal(1, downgrades.Count);
            var downgrade = downgrades.First();
            Assert.Equal("D", downgrade.DowngradedTo.Key.Name);
            Assert.Equal("1.0.0", downgrade.DowngradedTo.Key.VersionRange.OriginalString);
            Assert.True(downgrade.DowngradedTo.Item.IsCentralTransitive);
            Assert.Equal("D", downgrade.DowngradedFrom.Key.Name);
            Assert.Equal("2.0.0", downgrade.DowngradedFrom.Key.VersionRange.OriginalString);
        }

        /// <summary>
        ///   -> B 1.0.0 -> D [1.0.0] (PrivateAssets1)
        /// A
        ///   -> C 1.0.0 -> D [2.0.0] (PrivateAssets2)
        /// </summary>
        [Theory]
        [InlineData(null, null, 1)]
        [InlineData(LibraryIncludeFlags.All, null, 0)]
        [InlineData(null, LibraryIncludeFlags.All, 0)]
        [InlineData(LibraryIncludeFlags.All, LibraryIncludeFlags.All, 0)]
        public async Task PrivateAssetsAll_VersionConflicts(LibraryIncludeFlags? privateAssets1,
            LibraryIncludeFlags? privateAssets2, int expectedConflicts)
        {
            var framework = NuGetFramework.Parse("net45");
            var context = new TestRemoteWalkContext();
            var provider = new DependencyProvider();

            provider.Package("A", "1.0.0")
                .DependsOn("B", "1.0.0")
                .DependsOn("C", "1.0.0");

            provider.Package("B", "1.0.0")
                .DependsOn("D", "[1.0.0]", privateAssets: privateAssets1);

            provider.Package("C", "1.0.0")
                .DependsOn("D", "[2.0.0]", privateAssets: privateAssets2);

            provider.Package("D", "1.0.0");
            provider.Package("D", "2.0.0");

            context.LocalLibraryProviders.Add(provider);
            var walker = new RemoteDependencyWalker(context);

            // Act
            GraphNode<RemoteResolveResult> rootNode = await DoWalkAsync(walker, "A", framework);
            AnalyzeResult<RemoteResolveResult> result = rootNode.Analyze();

            // Assert
            Assert.Equal(expectedConflicts, result.VersionConflicts.Count);
            Assert.Empty(result.Downgrades);

            if (expectedConflicts == 1)
            {
                AssertPath(result.VersionConflicts[0].Conflicting, "A 1.0.0", "B 1.0.0", "D 1.0.0");
                AssertPath(result.VersionConflicts[0].Selected, "A 1.0.0", "C 1.0.0", "D 2.0.0");
            }
        }

        /// <summary>
        /// A -> B 1.0.0 -> C 2.0.0 (PrivateAssets1)
        ///   -> C 1.0.0 (PrivateAssets2)
        /// </summary>
        [Theory]
        [InlineData(null, null, 1)]
        [InlineData(LibraryIncludeFlags.All, null, 0)]
        [InlineData(null, LibraryIncludeFlags.All, 1)]
        [InlineData(LibraryIncludeFlags.All, LibraryIncludeFlags.All, 0)]
        public async Task PrivateAssetsAll_VersionDowngrades(LibraryIncludeFlags? privateAssets1, LibraryIncludeFlags? privateAssets2, int expectedDowngrades)
        {
            var framework = NuGetFramework.Parse("net45");
            var context = new TestRemoteWalkContext();
            var provider = new DependencyProvider();

            provider.Package("A", "1.0.0")
                .DependsOn("B", "1.0.0")
                .DependsOn("C", "1.0.0", privateAssets: privateAssets2);

            provider.Package("B", "1.0.0")
                   .DependsOn("C", "2.0.0", privateAssets: privateAssets1);

            provider.Package("C", "1.0.0");
            provider.Package("C", "2.0.0");

            context.LocalLibraryProviders.Add(provider);
            var walker = new RemoteDependencyWalker(context);

            // Act
            GraphNode<RemoteResolveResult> rootNode = await DoWalkAsync(walker, "A", framework);
            AnalyzeResult<RemoteResolveResult> result = rootNode.Analyze();

            // Assert
            Assert.Equal(expectedDowngrades, result.Downgrades.Count);
            Assert.Empty(result.VersionConflicts);

            if (expectedDowngrades == 1)
            {
                GraphNode<RemoteResolveResult> downgraded = result.Downgrades[0].DowngradedFrom;
                GraphNode<RemoteResolveResult> downgradedBy = result.Downgrades[0].DowngradedTo;

                AssertPath(downgraded, "A 1.0.0", "B 1.0.0", "C 2.0.0");
                AssertPath(downgradedBy, "A 1.0.0", "C 1.0.0");
            }
        }

        /// <summary>
        /// A -> B 1.0.0 (PrivateAssets) -> C 2.0.0
        ///   -> C 1.0.0
        /// </summary>
        [Theory]
        [InlineData(null, 1)]
        [InlineData(LibraryIncludeFlags.All, 1)]
        public async Task PrivateAssetsAll_VersionDowngradesForTransitiveDependencies(LibraryIncludeFlags? privateAssets, int expectedDowngrades)
        {
            var framework = NuGetFramework.Parse("net45");
            var context = new TestRemoteWalkContext();
            var provider = new DependencyProvider();

            provider.Package("A", "1.0.0")
                .DependsOn("B", "1.0.0", privateAssets: privateAssets)
                .DependsOn("C", "1.0.0");

            provider.Package("B", "1.0.0")
                .DependsOn("C", "2.0.0");

            provider.Package("C", "1.0.0");
            provider.Package("C", "2.0.0");

            context.LocalLibraryProviders.Add(provider);
            var walker = new RemoteDependencyWalker(context);

            // Act
            GraphNode<RemoteResolveResult> rootNode = await DoWalkAsync(walker, "A", framework);
            AnalyzeResult<RemoteResolveResult> result = rootNode.Analyze();

            // Assert
            Assert.Equal(expectedDowngrades, result.Downgrades.Count);
            Assert.Empty(result.VersionConflicts);

            if (expectedDowngrades == 1)
            {
                GraphNode<RemoteResolveResult> downgraded = result.Downgrades[0].DowngradedFrom;
                GraphNode<RemoteResolveResult> downgradedBy = result.Downgrades[0].DowngradedTo;

                AssertPath(downgraded, "A 1.0.0", "B 1.0.0", "C 2.0.0");
                AssertPath(downgradedBy, "A 1.0.0", "C 1.0.0");
            }
        }


        /// A 1.0.0 -> C 1.0.0 -> D 1.1.0
        /// B 1.0.0 -> C 1.1.0 -> D 1.0.0
        /// D 1.0.0
        [Fact]
        public async Task WalkAsync_WithDowngradeInPrunedSubgraph_DoesNotReportDowngrades()
        {
            var context = new TestRemoteWalkContext();
            var provider = new DependencyProvider();
            provider.Package("Project", "1.0")
                    .DependsOn("A", "1.0")
                    .DependsOn("B", "1.0")
                    .DependsOn("D", "1.0");

            provider.Package("A", "1.0")
                    .DependsOn("C", "1.0");

            provider.Package("B", "1.0")
                    .DependsOn("C", "1.1");

            provider.Package("C", "1.0")
                    .DependsOn("D", "1.1");
            provider.Package("C", "1.1")
                    .DependsOn("D", "1.0");

            context.LocalLibraryProviders.Add(provider);
            var walker = new RemoteDependencyWalker(context);
            var node = await DoWalkAsync(walker, "Project");

            var result = node.Analyze();
            result.Downgrades.Should().BeEmpty();
        }

        /// <summary>
        /// A 1.0 -> D 1.0 (Central transitive)
        ///       -> B 1.0 -> D 3.0 (Central transitive - should be ignored because it is not at root)
        ///                -> C 1.0 -> D 2.0
        /// </summary>
        [Fact]
        public async Task TransitiveDependenciesFromNonRootLibraries_AreIgnored()
        {
            var context = new TestRemoteWalkContext();
            var projectProvider = new DependencyProvider();
            projectProvider.Package("A", "1.0")
                .DependsOn("B", "1.0", LibraryDependencyTarget.Project)
                .DependsOn("D", "1.0", LibraryDependencyTarget.Package, versionCentrallyManaged: true, libraryDependencyReferenceType: LibraryDependencyReferenceType.None);

            projectProvider.Package("B", "1.0", LibraryType.Project)
                .DependsOn("C", "1.0", LibraryDependencyTarget.Project)
                .DependsOn("D", "3.0", LibraryDependencyTarget.Package, versionCentrallyManaged: true, libraryDependencyReferenceType: LibraryDependencyReferenceType.None);

            projectProvider.Package("C", "1.0", LibraryType.Project)
                .DependsOn("D", "2.0", LibraryDependencyTarget.Package);

            context.ProjectLibraryProviders.Add(projectProvider);

            var libraryProvider = new DependencyProvider();
            libraryProvider.Package("D", "1.0", LibraryType.Package);
            libraryProvider.Package("D", "2.0", LibraryType.Package);
            libraryProvider.Package("D", "3.0", LibraryType.Package);

            context.LocalLibraryProviders.Add(libraryProvider);

            var walker = new RemoteDependencyWalker(context);
            var root = await DoWalkAsync(walker, "A");
            var result = root.Analyze();
            Assert.Equal(0, result.VersionConflicts.Count);
            Assert.Equal(0, result.Cycles.Count);
            Assert.Equal(1, result.Downgrades.Count);
            var d = result.Downgrades.Single();
            AssertPath(d.DowngradedFrom, "A 1.0", "B 1.0", "C 1.0", "D 2.0");
            AssertPath(d.DowngradedTo, "A 1.0", "D 1.0");
        }

        /// <summary>
        /// A -> B 1.0.0 -> C 1.0.0 -> D 1.0.0 (this will be rejected)-> E 1.0.0
        ///
        ///  D has version defined centrally 2.0.0
        ///  D 2.0.0 -> I 2.0.0 (this will be downgraded due to central I 1.0.0)
        ///  (D 2.0.0 should have parentNode C 1.0.0)
        ///
        ///  I has version defined centrally 1.0.0
        ///  I 1.0.0 -> G 1.0.0
        ///  (I 1.0.0 should have parentNode D 2.0.0)
        /// </summary>
        [Fact]
        public async Task WalkAsync_WithCentralTransitiveDependency_InnerNodesAndParentNodesCreatedCorrectly()
        {
            var framework = NuGetFramework.Parse("net45");

            var context = new TestRemoteWalkContext();
            var provider = new DependencyProvider();
            var version100 = "1.0.0";
            var version200 = "2.0.0";

            // A -> B 1.0.0 -> C 1.0.0 -> D 1.0.0 (this will be rejected) -> E 1.0.0
            provider.Package("A", version100)
                    .DependsOn("B", version100);
            provider.Package("B", version100)
                   .DependsOn("C", version100);
            provider.Package("C", version100)
                    .DependsOn("D", version100);
            provider.Package("D", version100)
                    .DependsOn("E", version100);

            // D 2.0.0 -> I 2.0.0
            provider.Package("D", version200)
                   .DependsOn("I", version200);
            provider.Package("I", version200);

            // I 1.0.0 -> G 1.0.0
            provider.Package("I", version100)
                   .DependsOn("G", version100);
            provider.Package("I", version100);

            // Simulates the existence of a D centrally defined package that is not direct dependency
            provider.Package("A", version100)
                     .DependsOn("D", version200, LibraryDependencyTarget.Package, versionCentrallyManaged: true, libraryDependencyReferenceType: LibraryDependencyReferenceType.None);

            // Simulates the existence of a I centrally defined package that is not direct dependency
            provider.Package("A", version100)
                     .DependsOn("I", version100, LibraryDependencyTarget.Package, versionCentrallyManaged: true, libraryDependencyReferenceType: LibraryDependencyReferenceType.None);

            context.LocalLibraryProviders.Add(provider);
            var walker = new RemoteDependencyWalker(context);

            // Act
            var rootNode = await DoWalkAsync(walker, "A", framework);

            // Assert
            AnalyzeResult<RemoteResolveResult> result = rootNode.Analyze();
            var allNodes = GetAllNodes(rootNode);
            // RootNode A has 3 InnerNodes: B (1.0.0), D (2.0.0), I (1.0.0). D and I are Transitive Central PackageVersion Nodes.
            Assert.Equal(3, rootNode.InnerNodes.Count);

            //check if ParentNodes of centralTranstiveNodes are added correctly
            List<GraphNode<RemoteResolveResult>> centralTransitiveNodes = allNodes.Where(n => n.Item.IsCentralTransitive).ToList();
            Assert.Equal(2, centralTransitiveNodes.Count);
            Assert.True(centralTransitiveNodes.Any(n => n.Item.Key.Name == "D"));
            Assert.True(centralTransitiveNodes.Any(n => n.Item.Key.Name == "I"));

            foreach (var node in centralTransitiveNodes)
            {
                Assert.Equal(Disposition.Accepted, node.Disposition);
                if (node.Key.Name == "D")
                {
                    Assert.Equal("2.0.0", node.Key.VersionRange.OriginalString);
                    Assert.Equal(1, node.ParentNodes.Count);
                    Assert.Equal("C (>= 1.0.0)", node.ParentNodes.First().Key.ToString());
                }
                if (node.Key.Name == "I")
                {
                    Assert.Equal("1.0.0", node.Key.VersionRange.OriginalString);
                    Assert.Equal(1, node.ParentNodes.Count);
                    Assert.Equal("D (>= 2.0.0)", node.ParentNodes.First().Key.ToString());
                }
            }

            //check if ParentNodes of nonCentralTransitiveNodes are empty and pointing to the static empty list correctly
            var staticEmptyList = Array.Empty<GraphNode<RemoteResolveResult>>();

            var nonCentralTransitiveNodes = allNodes.Where(n => !n.Item.IsCentralTransitive).ToList();
            Assert.Equal(4, nonCentralTransitiveNodes.Count);
            Assert.True(nonCentralTransitiveNodes.Any(n => n.Item.Key.Name == "A"));
            Assert.True(nonCentralTransitiveNodes.Any(n => n.Item.Key.Name == "B"));
            Assert.True(nonCentralTransitiveNodes.Any(n => n.Item.Key.Name == "C"));
            Assert.True(nonCentralTransitiveNodes.Any(n => n.Item.Key.Name == "G"));
            foreach (var node in nonCentralTransitiveNodes)
            {
                Assert.Equal(0, node.ParentNodes.Count);
                Assert.True(staticEmptyList == node.ParentNodes); //All nonCentralTransitiveNodes have ParentNodes pointing to the same empty list.
            }

            //check if InnerNodes of nodesWithEmptyInnerNodes are pointing to the static empty list correctly
            var nodesWithEmptyInnerNodes = allNodes.Where(n => n.InnerNodes.Count == 0).ToList();
            Assert.Equal(3, nodesWithEmptyInnerNodes.Count);
            Assert.True(nodesWithEmptyInnerNodes.Any(n => n.Item.Key.Name == "C"));
            Assert.True(nodesWithEmptyInnerNodes.Any(n => n.Item.Key.Name == "D"));
            Assert.True(nodesWithEmptyInnerNodes.Any(n => n.Item.Key.Name == "G"));
            Assert.True(nodesWithEmptyInnerNodes.Where(n => n.Item.Key.Name == "G").Single().InnerNodes == staticEmptyList);

            Assert.Equal(1, result.Downgrades.Count);
        }

        /// <summary>
        ///   -> B 1.0.0 -> C 1.0.0 (this would be eclipsed by C 2.0.0)
        /// A -> C 2.0.0
        ///   -> D 1.0.0 -> E 1.0.0
        /// </summary>
        [Fact]
        public async Task WalkAsync_WithNoCPM_InnerNodesAndParentNodesCreatedCorrectly()
        {
            var framework = NuGetFramework.Parse("net45");

            var context = new TestRemoteWalkContext();
            var provider = new DependencyProvider();
            var version100 = "1.0.0";
            var version200 = "2.0.0";

            // A -> B 1.0.0 -> C 1.0.0
            provider.Package("A", version100)
                    .DependsOn("B", version100);
            provider.Package("B", version100)
                   .DependsOn("C", version100);

            // A -> C 2.0.0
            provider.Package("A", version100)
                    .DependsOn("C", version200);

            // A -> D 1.0.0 -> E 1.0.0
            provider.Package("A", version100)
                    .DependsOn("D", version100);
            provider.Package("D", version100)
                    .DependsOn("E", version100);

            context.LocalLibraryProviders.Add(provider);
            var walker = new RemoteDependencyWalker(context);

            // Act
            var rootNode = await DoWalkAsync(walker, "A", framework);

            // Assert
            AnalyzeResult<RemoteResolveResult> result = rootNode.Analyze();
            var allNodes = GetAllNodes(rootNode);
            Assert.Equal(5, allNodes.Count);
            Assert.Equal(3, rootNode.InnerNodes.Count);

            //check if ParentNodes of allnodes are pointing to the static empty list correctly
            var staticEmptyList = Array.Empty<GraphNode<RemoteResolveResult>>();
            foreach (var node in allNodes)
            {
                Assert.Equal(0, node.ParentNodes.Count);
                Assert.True(staticEmptyList == node.ParentNodes);
            }

            //check if InnerNodes of  are pointing to the static empty list correctly
            var nodesWithEmptyInnerNodes = allNodes.Where(n => n.InnerNodes.Count == 0).ToList();
            Assert.Equal(3, nodesWithEmptyInnerNodes.Count);
            Assert.True(nodesWithEmptyInnerNodes.Any(n => n.Item.Key.Name == "B"));
            Assert.True(nodesWithEmptyInnerNodes.Where(n => n.Item.Key.Name == "B").Single().InnerNodes != staticEmptyList);
            Assert.True(nodesWithEmptyInnerNodes.Any(n => n.Item.Key.Name == "C"));
            Assert.True(nodesWithEmptyInnerNodes.Where(n => n.Item.Key.Name == "C").Single().InnerNodes == staticEmptyList);
            Assert.True(nodesWithEmptyInnerNodes.Any(n => n.Item.Key.Name == "E"));
            Assert.True(nodesWithEmptyInnerNodes.Where(n => n.Item.Key.Name == "E").Single().InnerNodes == staticEmptyList);
        }

        private void AssertPath<TItem>(GraphNode<TItem> node, params string[] items)
        {
            var matches = new List<string>();

            while (node != null)
            {
                matches.Insert(0, $"{node.Key.Name} {node.Item?.Key?.Version ?? node.Key.VersionRange.MinVersion}");
                node = node.OuterNode;
            }

            Assert.Equal(items, matches);
        }

        private Task<GraphNode<RemoteResolveResult>> DoWalkAsync(RemoteDependencyWalker walker, string name)
        {
            return DoWalkAsync(walker, name, NuGetFramework.Parse("net45"));
        }

        private Task<GraphNode<RemoteResolveResult>> DoWalkAsync(RemoteDependencyWalker walker, string name, NuGetFramework framework)
        {
            var range = new LibraryRange
            {
                Name = name,
                VersionRange = new VersionRange(new NuGetVersion("1.0"))
            };

            return walker.WalkAsync(range, framework, runtimeIdentifier: null, runtimeGraph: null, recursive: true);

        }

        private List<GraphNode<RemoteResolveResult>> GetAllNodes(GraphNode<RemoteResolveResult> rootNode)
        {
            var allNodes = new Dictionary<string, GraphNode<RemoteResolveResult>>();
            allNodes.Add(rootNode.Key.ToString(), rootNode);

            var queue = new Queue<GraphNode<RemoteResolveResult>>();
            queue.Enqueue(rootNode);
            while (queue.Count > 0)
            {
                var currNode = queue.Dequeue();
                foreach (var innerNode in currNode.InnerNodes)
                {
                    if (!allNodes.TryGetValue(innerNode.Key.ToString(), out _))
                    {
                        allNodes.Add(innerNode.Key.ToString(), innerNode);
                        queue.Enqueue(innerNode);
                    }
                }
            }
            return allNodes.Values.ToList();
        }
    }
}
