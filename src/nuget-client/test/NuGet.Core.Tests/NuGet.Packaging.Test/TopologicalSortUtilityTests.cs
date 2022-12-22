// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Linq;
using FluentAssertions;
using NuGet.Packaging.Core;
using NuGet.Versioning;
using Xunit;

namespace NuGet.Packaging.Test
{
    public class TopologicalSortUtilityTests
    {
        [Fact]
        public void TopologicalSortUtility_GivenUnrelatedPackagesVerifySortOrder()
        {
            var packages = new[]
            {
                new PackageDependencyInfo(new PackageIdentity("b", NuGetVersion.Parse("1.0.0")), Enumerable.Empty<PackageDependency>()),
                new PackageDependencyInfo(new PackageIdentity("a", NuGetVersion.Parse("1.0.0")), Enumerable.Empty<PackageDependency>()),
                new PackageDependencyInfo(new PackageIdentity("C", NuGetVersion.Parse("1.0.0")), Enumerable.Empty<PackageDependency>()),
            };

            var sorted = TopologicalSortUtility.SortPackagesByDependencyOrder(packages);

            sorted.Select(e => e.Id).Should().ContainInOrder(new[] { "C", "b", "a" });
        }

        [Fact]
        public void TopologicalSortUtility_GivenUnrelatedPackagesWithMissingDepsVerifySortOrder()
        {
            var packages = new[]
            {
                new PackageDependencyInfo(new PackageIdentity("b", NuGetVersion.Parse("1.0.0")), new[] { new PackageDependency("x") }),
                new PackageDependencyInfo(new PackageIdentity("a", NuGetVersion.Parse("1.0.0")), new[] { new PackageDependency("y") }),
                new PackageDependencyInfo(new PackageIdentity("C", NuGetVersion.Parse("1.0.0")), new[] { new PackageDependency("z") }),
            };

            var sorted = TopologicalSortUtility.SortPackagesByDependencyOrder(packages);

            sorted.Select(e => e.Id).Should().ContainInOrder(new[] { "C", "b", "a" });
        }

        [Fact]
        public void TopologicalSortUtility_GivenASingleDepChainVerifyOrder()
        {
            var packages = new[]
            {
                new PackageDependencyInfo(new PackageIdentity("b", NuGetVersion.Parse("1.0.0")), new[] { new PackageDependency("c") }),
                new PackageDependencyInfo(new PackageIdentity("a", NuGetVersion.Parse("1.0.0")), new[] { new PackageDependency("b") }),
                new PackageDependencyInfo(new PackageIdentity("c", NuGetVersion.Parse("1.0.0")), Enumerable.Empty<PackageDependency>()),
            };

            var sorted = TopologicalSortUtility.SortPackagesByDependencyOrder(packages);

            sorted.Select(e => e.Id).Should().ContainInOrder(new[] { "c", "b", "a" });
        }

        [Fact]
        public void TopologicalSortUtility_GivenASingleDepChainVerifyReverseOrder()
        {
            var packages = new[]
            {
                new PackageDependencyInfo(new PackageIdentity("b", NuGetVersion.Parse("1.0.0")), new[] { new PackageDependency("a") }),
                new PackageDependencyInfo(new PackageIdentity("c", NuGetVersion.Parse("1.0.0")), new[] { new PackageDependency("b") }),
                new PackageDependencyInfo(new PackageIdentity("a", NuGetVersion.Parse("1.0.0")), Enumerable.Empty<PackageDependency>()),
            };

            var sorted = TopologicalSortUtility.SortPackagesByDependencyOrder(packages);

            sorted.Select(e => e.Id).Should().ContainInOrder(new[] { "a", "b", "c" });
        }

        [Fact]
        public void TopologicalSortUtility_GivenMultipleDependencyChainsVerifyOrder()
        {
            var packages = new[]
            {
                new PackageDependencyInfo(new PackageIdentity("x", NuGetVersion.Parse("1.0.0")), new[] { new PackageDependency("y") }),
                new PackageDependencyInfo(new PackageIdentity("y", NuGetVersion.Parse("1.0.0")), new[] { new PackageDependency("z") }),
                new PackageDependencyInfo(new PackageIdentity("z", NuGetVersion.Parse("1.0.0")), Enumerable.Empty<PackageDependency>()),
                new PackageDependencyInfo(new PackageIdentity("a", NuGetVersion.Parse("1.0.0")), new[] { new PackageDependency("b") }),
                new PackageDependencyInfo(new PackageIdentity("b", NuGetVersion.Parse("1.0.0")), Enumerable.Empty<PackageDependency>()),
            };

            var sorted = TopologicalSortUtility.SortPackagesByDependencyOrder(packages);

            sorted.Select(e => e.Id).Should().ContainInOrder(new[] { "z", "y", "x", "b", "a" });
        }

        [Fact]
        public void TopologicalSortUtility_GivenMissingDepsMixVerifySortOrder()
        {
            var packages = new[]
            {
                new PackageDependencyInfo(new PackageIdentity("a", NuGetVersion.Parse("1.0.0")), new[] { new PackageDependency("x") }),
                new PackageDependencyInfo(new PackageIdentity("b", NuGetVersion.Parse("1.0.0")), Enumerable.Empty<PackageDependency>()),
                new PackageDependencyInfo(new PackageIdentity("c", NuGetVersion.Parse("1.0.0")), Enumerable.Empty<PackageDependency>()),
                new PackageDependencyInfo(new PackageIdentity("d", NuGetVersion.Parse("1.0.0")), new[] { new PackageDependency("x") }),
            };

            var sorted = TopologicalSortUtility.SortPackagesByDependencyOrder(packages);

            sorted.Select(e => e.Id).Should().ContainInOrder(new[] { "d", "c", "b", "a" });
        }

        [Fact]
        public void TopologicalSortUtility_SortOrderOfDiamondDependency()
        {
            var packages = new[]
            {
                new PackageDependencyInfo(new PackageIdentity("a", NuGetVersion.Parse("1.0.0")), new[] { new PackageDependency("b"), new PackageDependency("c") }),
                new PackageDependencyInfo(new PackageIdentity("b", NuGetVersion.Parse("1.0.0")), new[] { new PackageDependency("d") }),
                new PackageDependencyInfo(new PackageIdentity("c", NuGetVersion.Parse("1.0.0")), new[] { new PackageDependency("d") }),
                new PackageDependencyInfo(new PackageIdentity("d", NuGetVersion.Parse("1.0.0")), Enumerable.Empty<PackageDependency>()),
            };

            var sorted = TopologicalSortUtility.SortPackagesByDependencyOrder(packages);

            sorted.Select(e => e.Id).Should().ContainInOrder(new[] { "d", "c", "b", "a" });
        }

        [Fact]
        public void TopologicalSortUtility_SortOrderOfDiamondDependencyWithMissingVerifyNoChange()
        {
            var packages = new[]
            {
                new PackageDependencyInfo(new PackageIdentity("a", NuGetVersion.Parse("1.0.0")), new[] { new PackageDependency("b"), new PackageDependency("c"), new PackageDependency("x") }),
                new PackageDependencyInfo(new PackageIdentity("b", NuGetVersion.Parse("1.0.0")), new[] { new PackageDependency("d"), new PackageDependency("x") }),
                new PackageDependencyInfo(new PackageIdentity("c", NuGetVersion.Parse("1.0.0")), new[] { new PackageDependency("d") }),
                new PackageDependencyInfo(new PackageIdentity("d", NuGetVersion.Parse("1.0.0")), Enumerable.Empty<PackageDependency>()),
            };

            var sorted = TopologicalSortUtility.SortPackagesByDependencyOrder(packages);

            sorted.Select(e => e.Id).Should().ContainInOrder(new[] { "d", "c", "b", "a" });
        }

        [Fact]
        public void TopologicalSortUtility_GivenInterlinkedDepsVerifySortOrder()
        {
            var packages = new[]
            {
                CreateInfo("a", "b", "c", "d", "e", "f"),
                CreateInfo("b", "c", "d", "e", "f"),
                CreateInfo("c", "d", "e", "f"),
                CreateInfo("d", "e", "f"),
                CreateInfo("e", "f"),
                CreateInfo("f")
            };

            var sorted = TopologicalSortUtility.SortPackagesByDependencyOrder(packages);

            sorted.Select(e => e.Id).Should().ContainInOrder(new[] { "f", "e", "d", "c", "b", "a" });
        }

        [Fact]
        public void TopologicalSortUtility_GivenInterlinkedDepsVerifyRevSortOrder()
        {
            var packages = new[]
            {
                CreateInfo("f", "e", "d", "c", "b", "a"),
                CreateInfo("e", "d", "c", "b", "a"),
                CreateInfo("d", "c", "b", "a"),
                CreateInfo("c", "b", "a"),
                CreateInfo("b", "a"),
                CreateInfo("a")
            };

            var sorted = TopologicalSortUtility.SortPackagesByDependencyOrder(packages);

            sorted.Select(e => e.Id).Should().ContainInOrder(new[] { "a", "b", "c", "d", "e", "f" });
        }

        private static PackageDependencyInfo CreateInfo(string id, params string[] depIds)
        {
            var identity = new PackageIdentity(id, NuGetVersion.Parse("1.0.0"));
            var deps = depIds.Select(e => new PackageDependency(e, VersionRange.Parse("1.0.0")));

            return new PackageDependencyInfo(identity, deps);
        }
    }
}
