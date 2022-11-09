// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;
using Xunit;

namespace NuGet.Resolver.Test
{
    public class ResolverInputSortTests
    {
        [Fact]
        public void ResolverInputSort_TreeFlatten_UnLinkedPackages()
        {
            // Arrange
            var targets = new List<string> { "a" };
            var installed = CreateInstalledReferences("x", "y");

            var packages = new List<SourcePackageDependencyInfo>();
            packages.Add(CreatePackage("a", "1.0.0", new Dictionary<string, string>() { { "b", "1.0.0" } }));
            packages.Add(CreatePackage("b", "1.0.0"));
            packages.Add(CreatePackage("x", "1.0.0"));
            packages.Add(CreatePackage("y", "2.0.0"));

            var context = new PackageResolverContext(DependencyBehavior.Lowest,
                targets,
                installed.Select(package => package.PackageIdentity.Id),
                installed,
                Enumerable.Empty<PackageIdentity>(),
                packages,
                Enumerable.Empty<PackageSource>(),
                Common.NullLogger.Instance);

            var grouped = GroupPackages(packages);

            // Act
            var sorted = ResolverInputSort.TreeFlatten(grouped, context);
            var order = sorted.Select(group => group.First().Id.ToLowerInvariant()).ToList();

            // Assert
            Assert.Equal(4, order.Count);
            Assert.Equal("x", order[0]);
            Assert.Equal("y", order[1]);
            Assert.Equal("a", order[2]);
            Assert.Equal("b", order[3]);
        }

        [Fact]
        public void ResolverInputSort_TreeFlatten_MultipleTrees()
        {
            // Arrange
            var targets = new List<string> { "a" };
            var installed = CreateInstalledReferences("x", "y");

            var packages = new List<SourcePackageDependencyInfo>();
            packages.Add(CreatePackage("a", "1.0.0", new Dictionary<string, string>() { { "b", "1.0.0" } }));
            packages.Add(CreatePackage("x", "1.0.0", new Dictionary<string, string>() { { "y", "1.0.0" } }));
            packages.Add(CreatePackage("b", "1.0.0"));
            packages.Add(CreatePackage("y", "2.0.0"));

            var context = new PackageResolverContext(DependencyBehavior.Lowest,
                targets,
                installed.Select(package => package.PackageIdentity.Id),
                installed,
                Enumerable.Empty<PackageIdentity>(),
                packages,
                Enumerable.Empty<PackageSource>(),
                Common.NullLogger.Instance);

            var grouped = GroupPackages(packages);

            // Act
            var sorted = ResolverInputSort.TreeFlatten(grouped, context);
            var order = sorted.Select(group => group.First().Id.ToLowerInvariant()).ToList();

            // Assert
            Assert.Equal(4, order.Count);
            Assert.Equal("x", order[0]);
            Assert.Equal("y", order[1]);
            Assert.Equal("a", order[2]);
            Assert.Equal("b", order[3]);
        }

        [Fact]
        public void ResolverInputSort_TreeFlatten_OverlappingTree()
        {
            // Arrange
            var targets = new List<string> { "a" };
            var installed = CreateInstalledReferences();

            var packages = new List<SourcePackageDependencyInfo>();
            packages.Add(CreatePackage("a", "1.0.0", new Dictionary<string, string>() { { "b", "1.0.0" }, { "c", "1.0.0" } }));
            packages.Add(CreatePackage("b", "1.0.0", new Dictionary<string, string>() { { "d", "1.0.0" }, { "e", "1.0.0" } }));
            packages.Add(CreatePackage("c", "1.0.0", new Dictionary<string, string>() { { "d", "2.0.0" } }));
            packages.Add(CreatePackage("d", "1.0.0"));
            packages.Add(CreatePackage("e", "1.0.0"));
            packages.Add(CreatePackage("d", "2.0.0"));

            var context = new PackageResolverContext(DependencyBehavior.Lowest,
                targets,
                installed.Select(package => package.PackageIdentity.Id),
                installed,
                Enumerable.Empty<PackageIdentity>(),
                packages,
                Enumerable.Empty<PackageSource>(),
                Common.NullLogger.Instance);

            var grouped = GroupPackages(packages);

            // Act
            var sorted = ResolverInputSort.TreeFlatten(grouped, context);
            var order = sorted.Select(group => group.First().Id.ToLowerInvariant()).ToList();

            // Assert
            Assert.Equal(5, order.Count);
            Assert.Equal("a", order[0]);
            Assert.Equal("b", order[1]);
            Assert.Equal("e", order[2]);
            Assert.Equal("c", order[3]);
            Assert.Equal("d", order[4]);
        }

        [Fact]
        public void ResolverInputSort_TreeFlatten_DependencyOrder()
        {
            // Arrange
            var targets = new List<string> { "d" };
            var installed = CreateInstalledReferences();

            var packages = new List<SourcePackageDependencyInfo>();
            packages.Add(CreatePackage("d", "1.0.0", new Dictionary<string, string>() { { "c", "1.0.0" } }));
            packages.Add(CreatePackage("c", "1.0.0", new Dictionary<string, string>() { { "b", "1.0.0" } }));
            packages.Add(CreatePackage("b", "1.0.0", new Dictionary<string, string>() { { "a", "1.0.0" } }));
            packages.Add(CreatePackage("a", "1.0.0"));

            var context = new PackageResolverContext(DependencyBehavior.Lowest,
                targets,
                installed.Select(package => package.PackageIdentity.Id),
                installed,
                Enumerable.Empty<PackageIdentity>(),
                packages,
                Enumerable.Empty<PackageSource>(),
                Common.NullLogger.Instance);

            var grouped = GroupPackages(packages);

            // Act
            var sorted = ResolverInputSort.TreeFlatten(grouped, context);
            var order = sorted.Select(group => group.First().Id.ToLowerInvariant()).ToList();

            // Assert
            Assert.Equal(4, order.Count);
            Assert.Equal("d", order[0]);
            Assert.Equal("c", order[1]);
            Assert.Equal("b", order[2]);
            Assert.Equal("a", order[3]);
        }

        [Fact]
        public void ResolverInputSort_TreeFlatten_Priority()
        {
            // Arrange
            var targets = new List<string> { "m" };
            var installed = CreateInstalledReferences("x");

            var packages = new List<SourcePackageDependencyInfo>();
            packages.Add(CreatePackage("m", "1.0.0"));
            packages.Add(CreatePackage("x", "1.0.0"));
            packages.Add(CreatePackage("a", "1.0.0"));
            packages.Add(CreatePackage("b", "1.0.0"));

            var context = new PackageResolverContext(DependencyBehavior.Lowest,
                targets,
                installed.Select(package => package.PackageIdentity.Id),
                installed,
                Enumerable.Empty<PackageIdentity>(),
                packages,
                Enumerable.Empty<PackageSource>(),
                Common.NullLogger.Instance);

            var grouped = GroupPackages(packages);

            // Act
            var sorted = ResolverInputSort.TreeFlatten(grouped, context);
            var order = sorted.Select(group => group.First().Id.ToLowerInvariant()).ToList();

            // Assert
            Assert.Equal(4, order.Count);

            // installed first
            Assert.Equal("x", order[0]);

            // targets
            Assert.Equal("m", order[1]);

            // new target dependencies
            Assert.Equal("a", order[2]);
            Assert.Equal("b", order[3]);
        }

        [Fact]
        public void ResolverInputSort_TreeFlatten_EmptyList()
        {
            // Arrange
            var targets = new List<string> { "a" };
            var installed = CreateInstalledReferences("b");

            var packages = new List<SourcePackageDependencyInfo>();

            var context = new PackageResolverContext(DependencyBehavior.Lowest,
                targets,
                installed.Select(package => package.PackageIdentity.Id),
                installed,
                Enumerable.Empty<PackageIdentity>(),
                packages,
                Enumerable.Empty<PackageSource>(),
                Common.NullLogger.Instance);

            var grouped = GroupPackages(packages);

            // Act
            var sorted = ResolverInputSort.TreeFlatten(grouped, context);
            var order = sorted.Select(group => group.First().Id.ToLowerInvariant()).ToList();

            // Assert
            Assert.Equal(0, order.Count);
        }

        private static List<List<ResolverPackage>> GroupPackages(List<SourcePackageDependencyInfo> packages)
        {
            var result = new List<List<ResolverPackage>>();

            var grouped = packages.GroupBy(package => package.Id, StringComparer.OrdinalIgnoreCase);

            foreach (var group in grouped)
            {
                result.Add(group.Select(package => new ResolverPackage(package.Id, package.Version, package.Dependencies, true, false)).ToList());
            }

            return result;
        }

        private ResolverPackage CreatePackage(string id, string version, IDictionary<string, string> dependencies = null)
        {
            List<NuGet.Packaging.Core.PackageDependency> deps = new List<NuGet.Packaging.Core.PackageDependency>();

            if (dependencies != null)
            {
                foreach (var dep in dependencies)
                {
                    VersionRange range = null;

                    if (dep.Value != null)
                    {
                        range = VersionRange.Parse(dep.Value);
                    }

                    deps.Add(new NuGet.Packaging.Core.PackageDependency(dep.Key, range));
                }
            }

            return new ResolverPackage(id, NuGetVersion.Parse(version), deps, true, false);
        }

        private List<PackageReference> CreateInstalledReferences(params string[] ids)
        {
            var result = new List<PackageReference>();

            foreach (var id in ids)
            {
                result.Add(new PackageReference(new PackageIdentity(id, NuGetVersion.Parse("1.0.0")), NuGetFramework.Parse("net45")));
            }

            return result;
        }
    }
}
