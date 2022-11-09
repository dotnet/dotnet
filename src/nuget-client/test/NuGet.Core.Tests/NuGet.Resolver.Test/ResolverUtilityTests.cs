// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Versioning;
using Xunit;

namespace NuGet.Resolver.Test
{
    public class ResolverUtilityTests
    {
        [Fact]
        public void ResolverUtility_TopologicalSort()
        {
            // Arrange
            var packages = new List<ResolverPackage>()
            {
                CreatePackage("A", "1.0.0", "B", "1.0.0"),
                CreatePackage("B", "1.0.0", "C", "1.0.0"),
                CreatePackage("C", "1.0.0", "D", "1.0.0"),
                CreatePackage("D", "1.0.0"),
                CreatePackage("E1", "1.0.0"),
                CreatePackage("E3", "1.0.0"),
                CreatePackage("E2", "1.0.0")
            };

            // Act
            var sorted = ResolverUtility.TopologicalSort(packages).ToList();

            // Assert
            Assert.Equal(7, sorted.Count);
            Assert.Equal("D", sorted[0].Id);
            Assert.Equal("C", sorted[1].Id);
            Assert.Equal("B", sorted[2].Id);
            Assert.Equal("A", sorted[3].Id);
            Assert.Equal("E1", sorted[4].Id);
            Assert.Equal("E2", sorted[5].Id);
            Assert.Equal("E3", sorted[6].Id);
        }

        [Fact]
        public void ResolverUtility_GetLowestDistanceFromTargetMultiplePaths()
        {
            // Arrange
            var targets = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            targets.Add("A");
            targets.Add("C");

            var packages = new List<ResolverPackage>()
            {
                CreatePackage("A", "1.0.0", "B", "1.0.0"),
                CreatePackage("B", "1.0.0", "C", "1.0.0"),
                CreatePackage("C", "1.0.0", "D", "1.0.0"),
                CreatePackage("D", "1.0.0"),
            };

            // Act
            var distanceA = ResolverUtility.GetLowestDistanceFromTarget("A", targets, packages);
            var distanceB = ResolverUtility.GetLowestDistanceFromTarget("B", targets, packages);
            var distanceC = ResolverUtility.GetLowestDistanceFromTarget("C", targets, packages);
            var distanceD = ResolverUtility.GetLowestDistanceFromTarget("D", targets, packages);
            var distanceE = ResolverUtility.GetLowestDistanceFromTarget("E", targets, packages);

            // Assert
            Assert.Equal(0, distanceA);
            Assert.Equal(1, distanceB);
            Assert.Equal(0, distanceC);
            Assert.Equal(1, distanceD);
            Assert.Equal(20, distanceE); // max, not found
        }

        [Fact]
        public void ResolverUtility_GetLowestDistanceFromTarget()
        {
            // Arrange
            var targets = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            targets.Add("A");

            var packages = new List<ResolverPackage>()
            {
                CreatePackage("A", "1.0.0", "B", "1.0.0"),
                CreatePackage("B", "1.0.0", "C", "1.0.0"),
                CreatePackage("C", "1.0.0", "D", "1.0.0"),
                CreatePackage("D", "1.0.0"),
            };

            // Act
            var distanceA = ResolverUtility.GetLowestDistanceFromTarget("A", targets, packages);
            var distanceB = ResolverUtility.GetLowestDistanceFromTarget("B", targets, packages);
            var distanceC = ResolverUtility.GetLowestDistanceFromTarget("C", targets, packages);
            var distanceD = ResolverUtility.GetLowestDistanceFromTarget("D", targets, packages);
            var distanceE = ResolverUtility.GetLowestDistanceFromTarget("E", targets, packages);

            // Assert
            Assert.Equal(0, distanceA);
            Assert.Equal(1, distanceB);
            Assert.Equal(2, distanceC);
            Assert.Equal(3, distanceD);
            Assert.Equal(20, distanceE); // max, not found
        }

        [Fact]
        public void ResolverUtility_GetLowestDistanceFromTarget_Basic()
        {
            // Arrange
            var targets = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            targets.Add("A");

            var packages = new List<ResolverPackage>()
            {
                CreatePackage("A", "1.0.0", "B", "1.0.0"),
                CreatePackage("B", "1.0.0", "C", "1.0.0"),
                CreatePackage("C", "1.0.0", "D", "1.0.0"),
                CreatePackage("D", "1.0.0"),
            };

            // Act
            var distanceC = ResolverUtility.GetLowestDistanceFromTarget("C", targets, packages);

            // Assert
            Assert.Equal(2, distanceC);
        }

        [Fact]
        public void ResolverUtility_CircularDependencyCheckAbsentPackages()
        {
            // Arrange
            var solution = new List<ResolverPackage>();
            solution.Add(CreatePackage("z", "1.0.0",
                new NuGet.Packaging.Core.PackageDependency("a", VersionRange.Parse("[1.0.0]")),
                new NuGet.Packaging.Core.PackageDependency("b", VersionRange.Parse("[1.0.0]"))));
            solution.Add(CreateAbsentPackage("a"));
            solution.Add(CreateAbsentPackage("b"));
            solution.Add(CreateAbsentPackage("y"));

            // Act
            var result = ResolverUtility.FindFirstCircularDependency(solution);

            // Assert
            Assert.False(result.Any());
        }

        [Fact]
        public void ResolverUtility_CircularDependencyCheckIndirectWithOthers()
        {
            // Arrange
            var solution = new List<ResolverPackage>();
            solution.Add(CreatePackage("z", "1.0.0",
                new NuGet.Packaging.Core.PackageDependency("y", VersionRange.Parse("[1.0.0]"))));
            solution.Add(CreatePackage("y", "1.0.0",
                new NuGet.Packaging.Core.PackageDependency("c", VersionRange.Parse("[1.0.0]"))));
            solution.Add(CreatePackage("c", "1.0.0",
                new NuGet.Packaging.Core.PackageDependency("d", VersionRange.Parse("[1.0.0]"))));
            solution.Add(CreatePackage("d", "2.0.0",
                new NuGet.Packaging.Core.PackageDependency("z", VersionRange.Parse("[1.0.0]"))));
            solution.Add(CreateAbsentPackage("a"));
            solution.Add(CreatePackage("x", "1.0.0",
             new NuGet.Packaging.Core.PackageDependency("z", VersionRange.Parse("[1.0.0]"))));
            solution.Add(CreatePackage("t", "1.0.0",
             new NuGet.Packaging.Core.PackageDependency("a", VersionRange.Parse("[1.0.0]"))));

            // Act
            var result = ResolverUtility.FindFirstCircularDependency(solution);

            // Assert
            var message = String.Join(" => ", result);
            Assert.Equal("z 1.0.0 => y 1.0.0 => c 1.0.0 => d 2.0.0 => z 1.0.0", message);
        }

        [Fact]
        public void ResolverUtility_CircularDependencyCheckIndirect()
        {
            // Arrange
            var solution = new List<ResolverPackage>();
            solution.Add(CreatePackage("a", "1.0.0",
                new NuGet.Packaging.Core.PackageDependency("b", VersionRange.Parse("[1.0.0]"))));
            solution.Add(CreatePackage("b", "1.0.0",
                new NuGet.Packaging.Core.PackageDependency("c", VersionRange.Parse("[1.0.0]"))));
            solution.Add(CreatePackage("c", "1.0.0",
                new NuGet.Packaging.Core.PackageDependency("d", VersionRange.Parse("[1.0.0]"))));
            solution.Add(CreatePackage("d", "1.0.0",
                new NuGet.Packaging.Core.PackageDependency("a", VersionRange.Parse("[1.0.0]"))));

            // Act
            var result = ResolverUtility.FindFirstCircularDependency(solution);

            // Assert
            Assert.Equal("a 1.0.0 => b 1.0.0 => c 1.0.0 => d 1.0.0 => a 1.0.0", String.Join(" => ", result));
        }

        [Fact]
        public void ResolverUtility_CircularDependencyCheckBasic()
        {
            // Arrange
            var solution = new List<ResolverPackage>();
            solution.Add(CreatePackage("a", "1.0.0",
                new NuGet.Packaging.Core.PackageDependency("b", VersionRange.Parse("[1.0.0]"))));
            solution.Add(CreatePackage("b", "1.0.0",
                new NuGet.Packaging.Core.PackageDependency("a", VersionRange.Parse("[1.0.0]"))));

            // Act
            var result = ResolverUtility.FindFirstCircularDependency(solution);

            // Assert
            Assert.Equal("a 1.0.0 => b 1.0.0 => a 1.0.0", String.Join(" => ", result));
        }

        [Fact]
        public void ResolverUtility_MultipleCircularDependencyCheck()
        {
            // Arrange
            var solution = new List<ResolverPackage>();
            solution.Add(CreatePackage("x", "1.0.0",
                new NuGet.Packaging.Core.PackageDependency("a", VersionRange.Parse("[1.0.0]"))));
            solution.Add(CreatePackage("a", "1.0.0",
                new NuGet.Packaging.Core.PackageDependency("b", VersionRange.Parse("[1.0.0]")),
                new NuGet.Packaging.Core.PackageDependency("e", VersionRange.Parse("[1.0.0]")),
                new NuGet.Packaging.Core.PackageDependency("f", VersionRange.Parse("[1.0.0]"))));
            solution.Add(CreatePackage("b", "1.0.0",
                new NuGet.Packaging.Core.PackageDependency("c", VersionRange.Parse("[1.0.0]"))));
            solution.Add(CreatePackage("c", "1.0.0",
                new NuGet.Packaging.Core.PackageDependency("d", VersionRange.Parse("[1.0.0]"))));
            solution.Add(CreatePackage("d", "1.0.0",
                new NuGet.Packaging.Core.PackageDependency("a", VersionRange.Parse("[1.0.0]"))));
            solution.Add(CreatePackage("e", "1.0.0", null));

            solution.Add(CreatePackage("f", "1.0.0",
                new NuGet.Packaging.Core.PackageDependency("g", VersionRange.Parse("[1.0.0]"))));
            solution.Add(CreatePackage("g", "1.0.0",
                new NuGet.Packaging.Core.PackageDependency("h", VersionRange.Parse("[1.0.0]"))));
            solution.Add(CreatePackage("h", "1.0.0",
                new NuGet.Packaging.Core.PackageDependency("i", VersionRange.Parse("[1.0.0]"))));
            solution.Add(CreatePackage("i", "1.0.0",
                new NuGet.Packaging.Core.PackageDependency("f", VersionRange.Parse("[1.0.0]"))));

            // Act
            var result = ResolverUtility.FindFirstCircularDependency(solution);

            // Assert
            Assert.Equal("x 1.0.0 => a 1.0.0 => b 1.0.0 => c 1.0.0 => d 1.0.0 => a 1.0.0", String.Join(" => ", result));
        }

        [Fact]
        public void ResolverUtility_NoCircularDependencyCheck()
        {
            // Arrange
            var solution = new List<ResolverPackage>();
            solution.Add(CreatePackage("a", "1.0.0",
                new NuGet.Packaging.Core.PackageDependency("b", VersionRange.Parse("[1.0.0]"))));
            solution.Add(CreatePackage("b", "1.0.0",
                new NuGet.Packaging.Core.PackageDependency("c", VersionRange.Parse("[1.0.0]")),
                new NuGet.Packaging.Core.PackageDependency("g", VersionRange.Parse("[1.0.0]")),
                new NuGet.Packaging.Core.PackageDependency("j", VersionRange.Parse("[1.0.0]"))));
            solution.Add(CreatePackage("c", "1.0.0",
                new NuGet.Packaging.Core.PackageDependency("d", VersionRange.Parse("[1.0.0]"))));
            solution.Add(CreatePackage("d", "1.0.0", null));
            solution.Add(CreatePackage("g", "1.0.0",
                new NuGet.Packaging.Core.PackageDependency("h", VersionRange.Parse("[1.0.0]"))));
            solution.Add(CreatePackage("h", "1.0.0", null));
            solution.Add(CreatePackage("j", "1.0.0", null));

            // Act
            var result = ResolverUtility.FindFirstCircularDependency(solution);

            // Assert
            Assert.False(result.Any());
        }

        [Fact]
        public void ResolverUtility_GetDiagnosticMessageVerifyDiamondDependencySortsById()
        {
            // Arrange
            var solution = new List<ResolverPackage>();
            solution.Add(CreatePackage("a", "1.0.0",
                new NuGet.Packaging.Core.PackageDependency("b", VersionRange.Parse("[2.0.0]")),
                new NuGet.Packaging.Core.PackageDependency("c", VersionRange.Parse("[1.0.0]"))));
            solution.Add(CreatePackage("c", "1.0.0", "d", "[1.0.0]"));
            solution.Add(CreateAbsentPackage("d"));
            solution.Add(CreatePackage("b", "2.0.0", "d", "[1.0.0]"));

            var installed = new List<PackageReference>();

            var available = solution.ToList();

            // Act
            var message = ResolverUtility.GetDiagnosticMessage(solution, available, installed, new string[] { "a" }, Enumerable.Empty<PackageSource>());

            // Assert
            Assert.Equal("Unable to find a version of 'd' that is compatible with 'b 2.0.0 constraint: d (= 1.0.0)', 'c 1.0.0 constraint: d (= 1.0.0)'.", message);
        }

        [Fact]
        public void ResolverUtility_GetDiagnosticMessageVerifyDependencyDistanceIsUsed()
        {
            // Arrange
            var solution = new List<ResolverPackage>();
            solution.Add(CreatePackage("a", "1.0.0", "y", "[1.0.0]"));
            solution.Add(CreatePackage("y", "1.0.0", "z", "[1.0.0]"));
            solution.Add(CreateAbsentPackage("z"));
            solution.Add(CreatePackage("a", "1.0.0", "b", "[1.0.0]"));
            solution.Add(CreatePackage("b", "1.0.0", "c", "[1.0.0]"));
            solution.Add(CreateAbsentPackage("c"));

            var installed = new List<PackageReference>();

            var available = solution.ToList();

            // Act
            var message = ResolverUtility.GetDiagnosticMessage(solution, available, installed, new string[] { "a" }, Enumerable.Empty<PackageSource>());

            // Assert
            Assert.Equal("Unable to find a version of 'z' that is compatible with 'y 1.0.0 constraint: z (= 1.0.0)'.", message);
        }

        [Fact]
        public void ResolverUtility_GetDiagnosticMessageForIncompatibleTargetWithAllowedVersion()
        {
            // Install b 2.0.0 - a requires b 1.0.0

            // Arrange
            var solution = new List<ResolverPackage>();
            solution.Add(CreatePackage("a", "1.0.0", "b", "[1.0.0]"));
            solution.Add(CreatePackage("b", "2.0.0"));

            var installed = new List<PackageReference>();
            installed.Add(new PackageReference(new PackageIdentity("b",
                NuGetVersion.Parse("2.0.0")),
                NuGetFramework.Parse("net45"),
                true, false, false, VersionRange.Parse("[2.0.0]")));

            var available = solution.ToList();

            // Act
            var message = ResolverUtility.GetDiagnosticMessage(solution, available, installed, new string[] { "b" }, Enumerable.Empty<PackageSource>());

            // Assert
            Assert.Equal("Unable to find a version of 'b' that is compatible with 'a 1.0.0 constraint: b (= 1.0.0)'. 'b' has an additional constraint (= 2.0.0) defined in packages.config.", message);
        }

        public void ResolverUtility_GetDiagnosticMessageForIncompatibleDependencyWithAllowedVersion()
        {
            // Install a 1.0.0 - which requires d 1.0.0 but d 2.0.0 is already installed.

            // Arrange
            var solution = new List<ResolverPackage>();
            solution.Add(CreatePackage("a", "1.0.0-1111", "b", "[1.0.0]"));
            solution.Add(CreatePackage("b", "1.0.0",
                new NuGet.Packaging.Core.PackageDependency("c", VersionRange.Parse("[1.0.0]")),
                new NuGet.Packaging.Core.PackageDependency("d", VersionRange.Parse("[1.0.0-1234]")),
                new NuGet.Packaging.Core.PackageDependency("e", VersionRange.Parse("[1.0.0]"))));
            solution.Add(CreatePackage("c", "2.0.0"));
            solution.Add(CreatePackage("d", "2.0.0"));
            solution.Add(CreatePackage("e", "1.0.0"));
            solution.Add(CreatePackage("f", "1.0.0", "d", "[2.0.0]"));

            var installed = new List<PackageReference>();
            installed.Add(CreateInstalledPackage("d", "2.0.0"));

            var available = solution.ToList();
            available.Add(CreatePackage("c", "1.0.0"));

            // Act
            var message = ResolverUtility.GetDiagnosticMessage(solution, available, installed, new string[] { "a" }, Enumerable.Empty<PackageSource>());

            // Assert
            Assert.Equal("Unable to resolve dependencies. 'd 2.0.0' is not compatible with 'a 1.0.0 constraint: d (= 1.0.0)'.", message);
        }

        public void ResolverUtility_GetDiagnosticMessageForMissingTargetDependency()
        {
            // Install b 1.1.0 - which requires d 1.0.0 which is missing from any source.

            // Arrange
            var solution = new List<ResolverPackage>();
            solution.Add(CreatePackage("b", "1.1.0.0",
                new NuGet.Packaging.Core.PackageDependency("c", VersionRange.Parse("[2.0.0]")),
                new NuGet.Packaging.Core.PackageDependency("d", VersionRange.Parse("[1.0.0.0]")),
                new NuGet.Packaging.Core.PackageDependency("e", VersionRange.Parse("[1.0.0]"))));
            solution.Add(CreatePackage("c", "3.0.0"));
            solution.Add(CreatePackage("e", "1.0.0"));

            var installed = new List<PackageReference>();
            installed.Add(CreateInstalledPackage("c", "1.0.0"));

            var available = solution.ToList();
            available.Add(CreatePackage("c", "1.0.0"));
            available.Add(CreatePackage("c", "2.0.0"));

            // Act
            var message = ResolverUtility.GetDiagnosticMessage(solution, available, installed, new string[] { "b" }, Enumerable.Empty<PackageSource>());

            // Assert
            Assert.Equal("Unable to find a version of 'd' that is compatible with 'b 1.1.0 constraint: d (= 1.0.0)'.", message);
        }

        [Fact]
        public void ResolverUtility_GetDiagnosticMessageForIncompatibleTargetInstalledNoAllowedVersion()
        {
            // Install b 2.0.0 - a requires b 1.0.0

            // Arrange
            var solution = new List<ResolverPackage>();
            solution.Add(CreatePackage("a", "1.0.0", "b", "[1.0.0]"));
            solution.Add(CreatePackage("b", "2.0.0"));

            var installed = new List<PackageReference>();
            installed.Add(new PackageReference(new PackageIdentity("b",
                NuGetVersion.Parse("2.0.0")),
                NuGetFramework.Parse("net45"),
                true, false, false, null));

            var available = solution.ToList();

            // Act
            var message = ResolverUtility.GetDiagnosticMessage(solution, available, installed, new string[] { "b" }, Enumerable.Empty<PackageSource>());

            // Assert
            Assert.Equal("Unable to resolve dependencies. 'b 2.0.0' is not compatible with 'a 1.0.0 constraint: b (= 1.0.0)'.", message);
        }

        [Fact]
        public void ResolverUtility_GetDiagnosticMessageForIncompatibleTarget()
        {
            // Install b 2.0.0 - a requires b 1.0.0

            // Arrange
            var solution = new List<ResolverPackage>();
            solution.Add(CreatePackage("a", "1.0.0", "b", "[1.0.0]"));
            solution.Add(CreatePackage("b", "2.0.0"));

            var installed = new List<PackageReference>();
            installed.Add(CreateInstalledPackage("a", "1.0.0"));

            var available = solution.ToList();

            // Act
            var message = ResolverUtility.GetDiagnosticMessage(solution, available, installed, new string[] { "b" }, Enumerable.Empty<PackageSource>());

            // Assert
            Assert.Equal("Unable to resolve dependencies. 'b 2.0.0' is not compatible with 'a 1.0.0 constraint: b (= 1.0.0)'.", message);
        }

        [Fact]
        public void ResolverUtility_GetDiagnosticMessageForTargetIncompatibleDependency()
        {
            // Install a, b 1.0.0 cannot be found

            // Arrange
            var solution = new List<ResolverPackage>();
            solution.Add(CreatePackage("a", "1.0.0", "b", "[1.0.0]"));
            solution.Add(CreatePackage("b", "2.0.0"));

            var available = solution.ToList();

            // Act
            var message = ResolverUtility.GetDiagnosticMessage(solution, available, Enumerable.Empty<PackageReference>(), new string[] { "a" }, Enumerable.Empty<PackageSource>());

            // Assert
            Assert.Equal("Unable to resolve dependencies. 'b 2.0.0' is not compatible with 'a 1.0.0 constraint: b (= 1.0.0)'.", message);
        }

        [Fact]
        public void ResolverUtility_GetDiagnosticMessageForTargetMissingDependency()
        {
            // Install a, b cannot be found

            // Arrange
            var solution = new List<ResolverPackage>();
            solution.Add(CreatePackage("a", "1.0.0", "b", "[1.0.0, )"));

            var available = solution.ToList();

            // Act
            var message = ResolverUtility.GetDiagnosticMessage(solution, available, Enumerable.Empty<PackageReference>(), new string[] { "a" }, Enumerable.Empty<PackageSource>());

            // Assert
            Assert.Equal("Unable to resolve dependency 'b'.", message);
        }

        [Fact]
        public void ResolverUtility_GetDiagnosticMessageForTargetAbsentDependency()
        {
            // Install a, b cannot be found

            // Arrange
            var solution = new List<ResolverPackage>();
            solution.Add(CreatePackage("a", "1.0.0", "b", "[1.0.0, )"));
            solution.Add(CreateAbsentPackage("b"));

            var available = solution.ToList();

            // Act
            var message = ResolverUtility.GetDiagnosticMessage(solution, available, Enumerable.Empty<PackageReference>(), new[] { "a" }, Enumerable.Empty<PackageSource>());

            // Assert
            Assert.Equal("Unable to find a version of 'b' that is compatible with 'a 1.0.0 constraint: b (>= 1.0.0)'.", message);
        }

        [Fact]
        public void ResolverUtility_GetDiagnosticMessageNoPackages()
        {
            // Arrange
            var solution = new List<ResolverPackage>();

            var available = solution.ToList();

            // Act
            var message = ResolverUtility.GetDiagnosticMessage(solution, available, Enumerable.Empty<PackageReference>(), new string[] { }, Enumerable.Empty<PackageSource>());

            // Assert
            Assert.Equal("Unable to resolve dependencies.", message);
        }

        [Fact]
        public void ResolverUtility_GetDiagnosticMessageAllAbsentPackages()
        {
            // Arrange
            var solution = new List<ResolverPackage>();
            solution.Add(CreateAbsentPackage("a"));
            solution.Add(CreateAbsentPackage("b"));
            solution.Add(CreateAbsentPackage("c"));

            var available = solution.ToList();

            // Act
            var message = ResolverUtility.GetDiagnosticMessage(solution, available, Enumerable.Empty<PackageReference>(), new string[] { "a" }, Enumerable.Empty<PackageSource>());

            // Assert
            Assert.Equal("Unable to resolve dependencies.", message);
        }

        [Fact]
        public void ResolverUtility_GetDiagnosticMessageForTargetMissingDependencyWithOneSource()
        {

            // Install a, b cannot be found

            // Arrange
            var solution = new List<ResolverPackage>();
            solution.Add(CreatePackage("a", "1.0.0", "b", "[1.0.0, )"));

            var available = solution.ToList();

            // Act
            var message = ResolverUtility.GetDiagnosticMessage(solution, available, Enumerable.Empty<PackageReference>(), new string[] { "a" },
                new List<PackageSource>() { new PackageSource("http://test", "test") });

            // Assert
            Assert.Equal("Unable to resolve dependency 'b'. Source(s) used: 'test'.", message);
        }

        [Fact]
        public void ResolverUtility_GetDiagnosticMessageForTargetMissingDependencyWithMultipleSources()
        {

            // Install a, b cannot be found

            // Arrange
            var solution = new List<ResolverPackage>();
            solution.Add(CreatePackage("a", "1.0.0", "b", "[1.0.0, )"));

            var available = solution.ToList();

            // Act
            var message = ResolverUtility.GetDiagnosticMessage(solution, available, Enumerable.Empty<PackageReference>(), new string[] { "a" },
                new List<PackageSource>() { new PackageSource("http://test", "test"),
                                            new PackageSource("http://test1","test1")});

            // Assert
            Assert.Equal("Unable to resolve dependency 'b'. Source(s) used: 'test', 'test1'.", message);
        }

        private static ResolverPackage CreatePackage(string id, string version, string dependencyId, string dependencyVersionRange)
        {
            return new ResolverPackage(id, NuGetVersion.Parse(version),
                new NuGet.Packaging.Core.PackageDependency[] { new Packaging.Core.PackageDependency(dependencyId, VersionRange.Parse(dependencyVersionRange)) }, true, false);
        }

        private static ResolverPackage CreatePackage(string id, string version)
        {
            return new ResolverPackage(id, NuGetVersion.Parse(version), Enumerable.Empty<NuGet.Packaging.Core.PackageDependency>(), true, false);
        }

        private static ResolverPackage CreatePackage(string id, string version, params NuGet.Packaging.Core.PackageDependency[] dependencies)
        {
            return new ResolverPackage(id, NuGetVersion.Parse(version), dependencies, true, false);
        }

        private static ResolverPackage CreateAbsentPackage(string id)
        {
            return new ResolverPackage(id, null, null, true, true);
        }

        private static PackageReference CreateInstalledPackage(string id, string version)
        {
            return new PackageReference(new Packaging.Core.PackageIdentity(id, NuGetVersion.Parse(version)), NuGetFramework.Parse("net45"));
        }

    }
}
