// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NuGet.Configuration;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Versioning;
using Xunit;

namespace NuGet.Resolver.Test
{
    public class ResolverTests
    {
        [Fact]
        public void ResolveChooseBestMatchForDependencyBehavior()
        {
            // Arrange
            var target = CreatePackage("A", "1.0", new Dictionary<string, string>() {
                { "B", "1.0.0" },
            });

            var sourceRepository = new List<ResolverPackage>();
            sourceRepository.Add(target);
            sourceRepository.Add(CreatePackage("B", "4.0.0", new Dictionary<string, string>() { { "C", "2.0.0" } }));
            sourceRepository.Add(CreatePackage("B", "2.0.0", new Dictionary<string, string>() { { "C", "2.0.0" } }));
            sourceRepository.Add(CreatePackage("C", "4.0.0"));
            sourceRepository.Add(CreatePackage("C", "3.0.0"));

            string message = string.Empty;
            var resolver = new PackageResolver();

            var context = CreatePackageResolverContext(DependencyBehavior.Lowest, target, sourceRepository);

            // Act
            var packages = resolver.Resolve(context, CancellationToken.None).ToDictionary(p => p.Id);

            // Assert
            Assert.Equal(3, packages.Count());
            Assert.Equal("1.0.0", packages["A"].Version.ToNormalizedString());
            Assert.Equal("2.0.0", packages["B"].Version.ToNormalizedString());
            Assert.Equal("3.0.0", packages["C"].Version.ToNormalizedString());
        }

        [Fact]
        public void ResolveDependencyForInstallPackageWithDependencyThatDoesntMeetExactVersionThrows()
        {
            // Arrange
            var target = CreatePackage("A", "1.0", new Dictionary<string, string>() {
                { "B", "[1.5]" },
            });

            var sourceRepository = new List<ResolverPackage>();
            sourceRepository.Add(target);
            sourceRepository.Add(CreatePackage("B", "1.4"));

            string message = string.Empty;
            var resolver = new PackageResolver();

            var context = CreatePackageResolverContext(DependencyBehavior.Lowest, target, sourceRepository);

            // Act
            try
            {
                var packages = resolver.Resolve(context, CancellationToken.None);
            }
            catch (NuGetResolverConstraintException ex)
            {
                message = ex.Message;
            }

            // Assert
            Assert.Equal("Unable to resolve dependencies. 'B 1.4.0' is not compatible with 'A 1.0.0 constraint: B (= 1.5.0)'.", message);
        }

        [Fact]
        public void ResolveDependenciesWithCircularReference()
        {
            // Arrange
            var target = CreatePackage("A", "1.0", new Dictionary<string, string>() {
                { "B", "1.0.0" },
            });

            var sourceRepository = new List<ResolverPackage>();
            sourceRepository.Add(target);
            sourceRepository.Add(CreatePackage("B", "1.0", new Dictionary<string, string>() {
                { "A", "1.0.0" },
            }));

            string message = string.Empty;
            var resolver = new PackageResolver();

            var context = CreatePackageResolverContext(DependencyBehavior.Lowest, target, sourceRepository);

            // Act
            try
            {
                var packages = resolver.Resolve(context, CancellationToken.None);
            }
            catch (NuGetResolverConstraintException ex)
            {
                message = ex.Message;
            }

            // Assert
            Assert.Equal("Circular dependency detected 'A 1.0.0 => B 1.0.0 => A 1.0.0'.", message);
        }

        [Fact]
        public void ResolveDependenciesForInstallPackageWithUnknownDependencyThrows()
        {
            // Arrange
            var target = CreatePackage("A", "1.0", new Dictionary<string, string>() {
                { "B", "1.0.0" },
            });

            var sourceRepository = new List<ResolverPackage>();
            sourceRepository.Add(target);

            string message = string.Empty;
            var resolver = new PackageResolver();

            var context = CreatePackageResolverContext(DependencyBehavior.Lowest, target, sourceRepository);

            // Act
            try
            {
                var packages = resolver.Resolve(context, CancellationToken.None);
            }
            catch (NuGetResolverConstraintException ex)
            {
                message = ex.Message;
            }

            // Assert
            Assert.Equal("Unable to resolve dependency 'B'.", message);
        }

        [Fact]
        public void ResolveDependenciesForLargeSetWithFailure()
        {
            // Arrange
            var target = CreatePackage("Package0", "1.0", new Dictionary<string, string>() {
                { "Package1", "1.0.0" },
                { "Package2", "1.0.0" },
                { "Package3", "1.0.0" },
                { "Package4", "1.0.0" },
                { "Package5", "1.0.0" },
                { "Package6", "1.0.0" },
                { "Package7", "1.0.0" },
                { "Package8", "1.0.0" },
            });

            var sourceRepository = new List<ResolverPackage>();
            sourceRepository.Add(target);

            // make lots of packages
            for (int i = 0; i < 10; i++)
            {
                for (int j = 1; j < 20; j++)
                {
                    int next = j + 1;
                    sourceRepository.Add(CreatePackage($"Package{j}", $"2.0.{i}", new Dictionary<string, string>() { { $"Package{next}", "1.0.0" } }));
                }
            }

            string message = string.Empty;
            var resolver = new PackageResolver();

            var context = CreatePackageResolverContext(DependencyBehavior.Lowest, target, sourceRepository);

            // Act
            try
            {
                var packages = resolver.Resolve(context, CancellationToken.None);
            }
            catch (NuGetResolverConstraintException ex)
            {
                message = ex.Message;
            }

            // Assert
            Assert.Equal("Unable to resolve dependency 'Package20'.", message);
        }

        [Fact]
        public void ResolveDependenciesForLargeSet()
        {
            // Arrange
            var target = CreatePackage("Package0", "1.0", new Dictionary<string, string>() {
                { "Package1", "1.0.0" },
                { "Package2", "1.0.0" },
                { "Package3", "1.0.0" },
                { "Package4", "1.0.0" },
                { "Package5", "1.0.0" },
                { "Package6", "1.0.0" },
                { "Package7", "1.0.0" },
                { "Package8", "1.0.0" },
            });

            var sourceRepository = new List<ResolverPackage>();
            sourceRepository.Add(target);

            int next = -1;

            // make lots of packages
            for (int i = 0; i < 100; i++)
            {
                for (int j = 1; j < 100; j++)
                {
                    next = j + 1;
                    sourceRepository.Add(CreatePackage($"Package{j}", $"2.0.{i}", new Dictionary<string, string>() { { $"Package{next}", "1.0.0" } }));
                }
            }

            sourceRepository.Add(CreatePackage($"Package{next}", $"2.0.0"));

            var resolver = new PackageResolver();

            var context = CreatePackageResolverContext(DependencyBehavior.Lowest, target, sourceRepository);

            // Act
            var packages = resolver.Resolve(context, CancellationToken.None);

            // Assert
            Assert.Equal(101, packages.Count());
        }

        [Fact]
        public void ResolveDependenciesForInstallDiamondDependencyGraphMissingPackage()
        {
            // Arrange
            // A -> [B, C]
            // B -> [D]
            // C -> [D]
            //    A
            //   / \
            //  B   C
            //   \ /
            //    D 
            var target = CreatePackage("A", "1.0", new Dictionary<string, string>() { { "B", null }, { "C", null } });

            var sourceRepository = new List<ResolverPackage>() {
                target,
                CreatePackage("B", "1.0", new Dictionary<string, string>() { { "D", null } }),
                CreatePackage("C", "1.0", new Dictionary<string, string>() { { "D", null } }),
            };

            string message = string.Empty;
            var resolver = new PackageResolver();

            var context = CreatePackageResolverContext(DependencyBehavior.Lowest, target, sourceRepository);

            try
            {
                var packages = resolver.Resolve(context, CancellationToken.None);
            }
            catch (NuGetResolverConstraintException ex)
            {
                message = ex.Message;
            }

            Assert.Equal("Unable to resolve dependency 'D'.", message);
        }

        [Fact]
        public void Resolver_MissingPackage2()
        {
            // No version constraints for any dependency
            // A -> B 2.0 -> (C, D)
            // A -> B 1.0 -> Missing Package

            var packageA = CreatePackage("A", "2.0", new Dictionary<string, string>() { { "B", "0.0" } });
            var packageB = CreatePackage("B", "2.0", new Dictionary<string, string>() { { "C", "0.0" }, { "D", "0.0" } });
            var packageC = CreatePackage("C", "2.0");
            var packageD = CreatePackage("D", "2.0");

            // OldB is the lowest but it has a missing dependency
            var packageOldB = CreatePackage("B", "1.0", new Dictionary<string, string>() { { "E", "0.0" } });

            var sourceRepository = new List<ResolverPackage>()
                {
                    packageA,
                    packageB,
                    packageC,
                    packageD,
                    packageOldB
                };

            var resolver = new PackageResolver();
            var context = CreatePackageResolverContext(DependencyBehavior.Lowest, packageA, sourceRepository);

            var packages = resolver.Resolve(context, CancellationToken.None).ToDictionary(p => p.Id);

            // Assert
            Assert.Equal(4, packages.Count());
            Assert.NotNull(packages["A"]);
            Assert.NotNull(packages["B"]);
            Assert.NotNull(packages["C"]);
            Assert.NotNull(packages["D"]);
        }

        [Fact]
        public void Resolver_MissingPackage()
        {
            // No version constraints for any dependency
            // A -> B 2.0 -> (C, D)
            // A -> B 1.0 -> Missing Package

            var packageA = CreatePackage("A", "2.0", new Dictionary<string, string>() { { "B", null } });
            var packageB = CreatePackage("B", "2.0", new Dictionary<string, string>() { { "C", null }, { "D", null } });
            var packageC = CreatePackage("C", "2.0");
            var packageD = CreatePackage("D", "2.0");

            // OldB is the lowest but it has a missing dependency
            var packageOldB = CreatePackage("B", "1.0", new Dictionary<string, string>() { { "E", null } });

            var sourceRepository = new List<ResolverPackage>()
                {
                    packageA,
                    packageB,
                    packageC,
                    packageD,
                    packageOldB
                };

            var resolver = new PackageResolver();
            var context = CreatePackageResolverContext(DependencyBehavior.Lowest, packageA, sourceRepository);

            var packages = resolver.Resolve(context, CancellationToken.None).ToDictionary(p => p.Id);

            // Assert
            Assert.Equal(4, packages.Count());
            Assert.NotNull(packages["A"]);
            Assert.NotNull(packages["B"]);
            Assert.NotNull(packages["C"]);
            Assert.NotNull(packages["D"]);
        }

        [Fact]
        public void Resolver_IgnoreDependencies()
        {
            var target = CreatePackage("A", "1.0", new Dictionary<string, string>() { { "B", null }, { "C", null } });

            var sourceRepository = new List<ResolverPackage>()
                {
                    target,
                    CreatePackage("B", "1.0", new Dictionary<string, string>() { { "D", null } }),
                    CreatePackage("C", "1.0", new Dictionary<string, string>() { { "D", null } }),
                    CreatePackage("D", "1.0"),
                };

            var resolver = new PackageResolver();
            var context = CreatePackageResolverContext(DependencyBehavior.Ignore, target, sourceRepository);

            var packages = resolver.Resolve(context, CancellationToken.None).ToDictionary(p => p.Id);

            // Assert
            Assert.Equal(1, packages.Count());
            Assert.NotNull(packages["A"]);
        }

        [Fact]
        public void ResolveDependenciesForInstallDiamondDependencyGraph()
        {
            // Arrange
            // A -> [B, C]
            // B -> [D]
            // C -> [D]
            //    A
            //   / \
            //  B   C
            //   \ /
            //    D 
            var target = CreatePackage("A", "1.0", new Dictionary<string, string>() { { "B", null }, { "C", null } });

            var sourceRepository = new List<ResolverPackage>()
                {
                    target,
                    CreatePackage("B", "1.0", new Dictionary<string, string>() { { "D", null } }),
                    CreatePackage("C", "1.0", new Dictionary<string, string>() { { "D", null } }),
                    CreatePackage("D", "1.0"),
                };

            var resolver = new PackageResolver();
            var context = CreatePackageResolverContext(DependencyBehavior.Lowest, target, sourceRepository);

            var packages = resolver.Resolve(context, CancellationToken.None).ToDictionary(p => p.Id);

            // Assert
            Assert.Equal(4, packages.Count());
            Assert.NotNull(packages["A"]);
            Assert.NotNull(packages["B"]);
            Assert.NotNull(packages["C"]);
            Assert.NotNull(packages["D"]);
        }

        [Fact]
        public void ResolveDependenciesForInstallDiamondDependencyGraphWithDifferentVersionsOfSamePackage()
        {
            // Arrange
            var sourceRepository = new List<ResolverPackage>();
            // A -> [B, C]
            // B -> [D >= 1, E >= 2]
            // C -> [D >= 2, E >= 1]
            //     A
            //   /   \
            //  B     C
            //  | \   | \
            //  D1 E2 D2 E1

            var packageA = CreatePackage("A", "1.0", new Dictionary<string, string>() { { "B", null }, { "C", null } });
            var packageB = CreatePackage("B", "1.0", new Dictionary<string, string>() { { "D", "1.0" }, { "E", "2.0" } });
            var packageC = CreatePackage("C", "1.0", new Dictionary<string, string>() { { "D", "2.0" }, { "E", "1.0" } });
            var packageD1 = CreatePackage("D", "1.0");
            var packageD2 = CreatePackage("D", "2.0");
            var packageE1 = CreatePackage("E", "1.0");
            var packageE2 = CreatePackage("E", "2.0");

            sourceRepository.Add(packageA);
            sourceRepository.Add(packageB);
            sourceRepository.Add(packageC);
            sourceRepository.Add(packageD2);
            sourceRepository.Add(packageD1);
            sourceRepository.Add(packageE2);
            sourceRepository.Add(packageE1);

            // Act
            var resolver = new PackageResolver();
            var context = CreatePackageResolverContext(DependencyBehavior.Lowest, packageA, sourceRepository);

            var solution = resolver.Resolve(context, CancellationToken.None).ToArray();
            var packages = solution.ToDictionary(p => p.Id);

            // Assert
            Assert.Equal(5, packages.Count());

            Assert.Equal("1.0.0", packages["A"].Version.ToNormalizedString());
            Assert.Equal("1.0.0", packages["B"].Version.ToNormalizedString());
            Assert.Equal("1.0.0", packages["C"].Version.ToNormalizedString());
            Assert.Equal("2.0.0", packages["D"].Version.ToNormalizedString());
            Assert.Equal("2.0.0", packages["E"].Version.ToNormalizedString());

            //Verify that D & E are first (order doesn't matter), then B & C (order doesn't matter), then A
            Assert.True(solution.Take(2).Select(a => a.Id).All(id => id == "D" || id == "E"));
            Assert.True(solution.Skip(2).Take(2).Select(a => a.Id).All(id => id == "B" || id == "C"));
            Assert.Equal("A", solution[4].Id);
        }

        // Tests that when there is a local package that can satisfy all dependencies, it is preferred over other packages.
        [Fact]
        public void ResolveActionsPreferInstalledPackages()
        {
            // Arrange

            // Local:
            // B 1.0
            // C 1.0

            // Remote
            // A 1.0 -> B 1.0, C 1.0
            // B 1.0
            // B 1.1
            // C 1.0
            // C 2.0
            var target = CreatePackage("A", "1.0", new Dictionary<string, string>() { { "B", "1.0" }, { "C", "1.0" } });

            // Expect: Install A 1.0 (no change to B or C)
            var sourceRepository = new List<ResolverPackage>()
                {
                    target,
                    CreatePackage("B", "1.0"),
                    CreatePackage("B", "1.1"),
                    CreatePackage("C", "1.0"),
                    CreatePackage("C", "2.0"),
                };

            var install = new List<PackageReference>()
                {
                    new PackageReference(new PackageIdentity("B", NuGetVersion.Parse("1.0")), null),
                    new PackageReference(new PackageIdentity("C", NuGetVersion.Parse("1.0")), null),
                };

            List<PackageIdentity> targets = new List<PackageIdentity>();
            targets.Add(target);
            targets.AddRange(install.Select(e => e.PackageIdentity));

            // Act
            var resolver = new PackageResolver();
            var context = new PackageResolverContext(DependencyBehavior.HighestMinor,
                new string[] { "A" },
                install.Select(p => p.PackageIdentity.Id),
                install,
                targets,
                sourceRepository,
                Enumerable.Empty<PackageSource>(),
                Common.NullLogger.Instance);

            var solution = resolver.Resolve(context, CancellationToken.None).ToArray();
            var packages = solution.ToDictionary(p => p.Id);

            // Assert
            Assert.Equal(3, packages.Count);
            Assert.Equal("1.0.0", packages["A"].Version.ToNormalizedString());
            Assert.Equal("1.0.0", packages["B"].Version.ToNormalizedString());
            Assert.Equal("1.0.0", packages["C"].Version.ToNormalizedString());
        }

        [Fact]
        public void ResolveActionsForSimpleUpdate()
        {
            // Arrange
            // Installed: A, B
            // A 1.0 -> B [1.0]
            var project = new List<ResolverPackage>()
                {
                    CreatePackage("A", "1.0", new Dictionary<string, string> { { "B", "1.0" } }),
                    CreatePackage("B", "1.0"),
                };

            var target = CreatePackage("A", "2.0", new Dictionary<string, string> { { "B", "1.0" } });

            var sourceRepository = new List<ResolverPackage>()
                {
                    target,
                    CreatePackage("B", "1.0"),
                };

            // Act
            var resolver = new PackageResolver();
            var context = CreatePackageResolverContext(DependencyBehavior.HighestPatch, target, sourceRepository);

            var solution = resolver.Resolve(context, CancellationToken.None).ToArray();
            var packages = solution.ToDictionary(p => p.Id);

            // Assert
            Assert.Equal(2, solution.Length);
            Assert.Equal("2.0.0", packages["A"].Version.ToNormalizedString());
        }

        [Fact]
        public void ResolveDependenciesForUpdatePackageRequiringUpdatedDependencyThatRequiresUpdatedDependentBySeparatePath()
        {
            // Arrange
            // A -> [B, C]
            // B -> [D]
            // C -> [D]
            //    A
            //   / \
            //  B   C
            //   \ /
            //    D 

            // Local:
            // A 1.0 -> B [1.0, 2.0), C [1.0, 2.0)
            // B 1.0 -> D [1.0, 2.0)
            // C 1.0 -> D [1.0, 2.0)

            // Remote:
            // A 1.1 -> B [1.1, 2.0), C [2.0, 3.0)
            // B 1.1 -> D [2.0, 3.0)
            // C 2.0 -> D [2.0, 3.0)

            // Update initiated on B, not A

            var project = new List<PackageReference> {
                new PackageReference(new PackageIdentity("A", NuGetVersion.Parse("1.0")), null),
                new PackageReference(new PackageIdentity("B", NuGetVersion.Parse("1.0")), null),
                new PackageReference(new PackageIdentity("C", NuGetVersion.Parse("1.0")), null),
                new PackageReference(new PackageIdentity("D", NuGetVersion.Parse("1.0")), null)
            };

            var installed = new List<ResolverPackage>();
            var sourceRepository = new List<ResolverPackage>();

            var packageA1 =
                CreatePackage("A", "1.0", new Dictionary<string, string>
                    {
                        { "B", "[1.0, 2.0)" },
                        { "C", "[1.0, 2.0)" }
                    });
            installed.Add(packageA1);

            var packageB1 =
                CreatePackage("B", "1.0", new Dictionary<string, string>
                    {
                        { "D", "[1.0, 2.0)" }
                    });
            installed.Add(packageB1);

            var packageC1 =
                CreatePackage("C", "1.0", new Dictionary<string, string>
                    {
                        { "D", "[1.0, 2.0)" }
                    });
            installed.Add(packageC1);

            var packageD1 = CreatePackage("D", "1.0");
            installed.Add(packageD1);

            var packageA11 =
                CreatePackage("A", "1.1", new Dictionary<string, string>
                    {
                        { "B", "[1.1, 2.0)" },
                        { "C", "[2.0, 3.0)" }
                    });
            sourceRepository.Add(packageA11);

            var packageB11 =
                CreatePackage("B", "1.1", new Dictionary<string, string>
                    {
                        { "D", "[2.0, 3.0)"}
                    });
            sourceRepository.Add(packageB11);

            var packageC2 =
                CreatePackage("C", "2.0", new Dictionary<string, string>
                    {
                        { "D", "[2.0, 3.0)" }
                    });
            sourceRepository.Add(packageC2);

            var packageD2 = CreatePackage("D", "2.0");
            sourceRepository.Add(packageD2);

            // Act
            var resolver = new PackageResolver();
            var packageResolverContext
                = CreatePackageResolverContext(DependencyBehavior.Lowest, installed, sourceRepository);

            var solution = resolver.Resolve(packageResolverContext, CancellationToken.None).ToArray();
            var packages = solution.ToDictionary(p => p.Id);

            // Assert
            Assert.Equal(4, solution.Length);
            Assert.Equal("1.1.0", packages["A"].Version.ToNormalizedString());
            Assert.Equal("1.1.0", packages["B"].Version.ToNormalizedString());
            Assert.Equal("2.0.0", packages["C"].Version.ToNormalizedString());
            Assert.Equal("2.0.0", packages["D"].Version.ToNormalizedString());
        }

        [Fact]
        public void ResolvesLowestMajorHighestMinorHighestPatchVersionOfListedPackagesForDependencies()
        {
            // Arrange

            // A 1.0 -> B 1.0
            // B 1.0 -> C 1.1
            // C 1.1 -> D 1.0

            var target = CreatePackage("A", "1.0", new Dictionary<string, string>() { { "B", "1.0" } });

            var sourceRepository = new List<ResolverPackage>()
                {
                    target,
                    CreatePackage("B", "2.0", new Dictionary<string, string>() { { "C", "1.1" } }),
                    CreatePackage("B", "1.0", new Dictionary<string, string>() { { "C", "1.1" } }),
                    CreatePackage("B", "1.0.1"),
                    CreatePackage("D", "2.0"),
                    CreatePackage("C", "1.1.3", new Dictionary<string, string>() { { "D", "1.0" } }),
                    CreatePackage("C", "1.1.1", new Dictionary<string, string>() { { "D", "1.0" } }),
                    CreatePackage("C", "1.5.1", new Dictionary<string, string>() { { "D", "1.0" } }),
                    CreatePackage("B", "1.0.9", new Dictionary<string, string>() { { "C", "1.1" } }),
                    CreatePackage("B", "1.1", new Dictionary<string, string>() { { "C", "1.1" } })
                };

            // Act
            var resolver = new PackageResolver();
            var context = CreatePackageResolverContext(DependencyBehavior.HighestMinor, target, sourceRepository);

            var packages = resolver.Resolve(context, CancellationToken.None).ToDictionary(p => p.Id);

            // Assert
            Assert.Equal(4, packages.Count);
            Assert.Equal("2.0.0", packages["D"].Version.ToNormalizedString());
            Assert.Equal("1.5.1", packages["C"].Version.ToNormalizedString());
            Assert.Equal("1.1.0", packages["B"].Version.ToNormalizedString());
            Assert.Equal("1.0.0", packages["A"].Version.ToNormalizedString());
        }

        // Tests that when DependencyVersion is Lowest, the dependency with the lowest major minor and highest patch version
        // is picked.
        [Fact]
        public void ResolvesLowestMajorAndMinorAndPatchVersionOfListedPackagesForDependencies()
        {
            // Arrange

            // A 1.0 -> B 1.0
            // B 1.0 -> C 1.1
            // C 1.1 -> D 1.0

            var target = CreatePackage("A", "1.0", new Dictionary<string, string>() { { "B", "1.0" } });

            var sourceRepository = new List<ResolverPackage>()
                {
                    target,
                    CreatePackage("B", "2.0", new Dictionary<string, string>() { { "C", "1.1" } }),
                    CreatePackage("B", "1.0.1"),
                    CreatePackage("D", "2.0"),
                    CreatePackage("C", "1.1.3", new Dictionary<string, string>() { { "D", "1.0" } }),
                    CreatePackage("C", "1.1.1", new Dictionary<string, string>() { { "D", "1.0" } }),
                    CreatePackage("C", "1.5.1", new Dictionary<string, string>() { { "D", "1.0" } }),
                    CreatePackage("B", "1.0.9", new Dictionary<string, string>() { { "C", "1.1" } }),
                    CreatePackage("B", "1.1", new Dictionary<string, string>() { { "C", "1.1" } })
                };

            // Act
            var resolver = new PackageResolver();
            var context = CreatePackageResolverContext(DependencyBehavior.Lowest, target, sourceRepository);

            var packages = resolver.Resolve(context, CancellationToken.None).ToDictionary(p => p.Id);

            // Assert
            Assert.Equal(2, packages.Count);
            Assert.Equal("1.0.1", packages["B"].Version.ToNormalizedString());
            Assert.Equal("1.0.0", packages["A"].Version.ToNormalizedString());
        }

        [Fact]
        public void ResolvesLowestMajorAndMinorHighestPatchVersionOfListedPackagesForDependencies()
        {
            // Arrange

            var target = CreatePackage("A", "1.0", new Dictionary<string, string>() { { "B", "1.0" } });

            // A 1.0 -> B 1.0
            // B 1.0 -> C 1.1
            // C 1.1 -> D 1.0
            var sourceRepository = new List<ResolverPackage>()
                {
                    target,
                    CreatePackage("B", "2.0", new Dictionary<string, string>() { { "C", "1.1" } }),
                    CreatePackage("B", "1.0", new Dictionary<string, string>() { { "C", "1.1" } }),
                    CreatePackage("B", "1.0.1"),
                    CreatePackage("D", "2.0"),
                    CreatePackage("C", "1.1.3", new Dictionary<string, string>() { { "D", "1.0" } }),
                    CreatePackage("C", "1.1.1", new Dictionary<string, string>() { { "D", "1.0" } }),
                    CreatePackage("C", "1.5.1", new Dictionary<string, string>() { { "D", "1.0" } }),
                    CreatePackage("B", "1.0.9", new Dictionary<string, string>() { { "C", "1.1" } }),
                    CreatePackage("B", "1.1", new Dictionary<string, string>() { { "C", "1.1" } })
                };

            // Act
            var resolver = new PackageResolver();
            var context = CreatePackageResolverContext(DependencyBehavior.HighestPatch, target, sourceRepository);

            var packages = resolver.Resolve(context, CancellationToken.None).ToDictionary(p => p.Id);

            // Assert
            Assert.Equal(4, packages.Count);
            Assert.Equal("2.0.0", packages["D"].Version.ToNormalizedString());
            Assert.Equal("1.1.3", packages["C"].Version.ToNormalizedString());
            Assert.Equal("1.0.9", packages["B"].Version.ToNormalizedString());
            Assert.Equal("1.0.0", packages["A"].Version.ToNormalizedString());
        }

        [Fact]
        public void Resolver_Basic()
        {
            ResolverPackage target = new ResolverPackage("a", new NuGetVersion(1, 0, 0),
                new NuGet.Packaging.Core.PackageDependency[]
                    {
                        new NuGet.Packaging.Core.PackageDependency("b", new VersionRange(new NuGetVersion(1, 0, 0), true, new NuGetVersion(3, 0, 0), true))
                    },
                true,
                false);

            var dep1 = new ResolverPackage("b", new NuGetVersion(2, 0, 0));
            var dep2 = new ResolverPackage("b", new NuGetVersion(2, 5, 0));
            var dep3 = new ResolverPackage("b", new NuGetVersion(4, 0, 0));

            List<ResolverPackage> possible = new List<ResolverPackage>();
            possible.Add(dep1);
            possible.Add(dep2);
            possible.Add(dep3);
            possible.Add(target);

            var resolver = new PackageResolver();
            var context = CreatePackageResolverContext(DependencyBehavior.Lowest, target, possible);
            var solution = resolver.Resolve(context, CancellationToken.None).ToList();

            Assert.Equal(2, solution.Count());
        }

        [Fact]
        public void Resolver_NoSolution()
        {
            ResolverPackage target = new ResolverPackage("a", new NuGetVersion(1, 0, 0), new NuGet.Packaging.Core.PackageDependency[] { new NuGet.Packaging.Core.PackageDependency("b", null) }, true, false);

            List<ResolverPackage> possible = new List<ResolverPackage>();
            possible.Add(target);

            var resolver = new PackageResolver();
            var context = CreatePackageResolverContext(DependencyBehavior.Lowest, target, possible);

            Assert.Throws<NuGetResolverConstraintException>(() => resolver.Resolve(context, CancellationToken.None));
        }

        [Fact]
        public void Resolver_Basic_Listed()
        {
            var a100 = new ResolverPackage("a", new NuGetVersion(1, 0, 0),
                new NuGet.Packaging.Core.PackageDependency[]
                    {
                        new NuGet.Packaging.Core.PackageDependency("b",
                            new VersionRange(new NuGetVersion(1, 0, 0), true, new NuGetVersion(5, 0, 0), true))
                    },
                true,
                false);

            var b200 = new ResolverPackage("b", new NuGetVersion(2, 0, 0), null, false, false);
            var b250 = new ResolverPackage("b", new NuGetVersion(2, 5, 0), null, false, false);
            var b400 = new ResolverPackage("b", new NuGetVersion(4, 0, 0), null, true, false);
            var b500 = new ResolverPackage("b", new NuGetVersion(5, 0, 0), null, true, false);
            var b600 = new ResolverPackage("b", new NuGetVersion(6, 0, 0), null, true, false);

            List<ResolverPackage> available = new List<ResolverPackage>();
            available.Add(a100);
            available.Add(b200);
            available.Add(b250);
            available.Add(b400);
            available.Add(b500);
            available.Add(b600);

            var resolver = new PackageResolver();
            var context = CreatePackageResolverContext(DependencyBehavior.Lowest, a100, available);
            var solution = resolver.Resolve(context, CancellationToken.None)
                .OrderBy(pi => pi.Id)
                .ToList();

            //  the result includes "b" version "4.0.0" because it is the lowest listed dependency

            Assert.Equal(new PackageIdentity("a", new NuGetVersion(1, 0, 0)), solution[0], PackageIdentityComparer.Default);
            Assert.Equal(new PackageIdentity("b", new NuGetVersion(4, 0, 0)), solution[1], PackageIdentityComparer.Default);
        }

        [Fact]
        public void Resolver_Basic_AllListed()
        {
            var a100 = new ResolverPackage("a", new NuGetVersion(1, 0, 0),
                new NuGet.Packaging.Core.PackageDependency[]
                    {
                        new NuGet.Packaging.Core.PackageDependency("b",
                            new VersionRange(new NuGetVersion(1, 0, 0), true, new NuGetVersion(5, 0, 0), true))
                    },
                true,
                false);

            var b200 = new ResolverPackage("b", new NuGetVersion(2, 0, 0), null, true, false);
            var b250 = new ResolverPackage("b", new NuGetVersion(2, 5, 0), null, true, false);
            var b400 = new ResolverPackage("b", new NuGetVersion(4, 0, 0), null, true, false);
            var b500 = new ResolverPackage("b", new NuGetVersion(5, 0, 0), null, true, false);
            var b600 = new ResolverPackage("b", new NuGetVersion(6, 0, 0), null, true, false);

            List<ResolverPackage> available = new List<ResolverPackage>();
            available.Add(a100);
            available.Add(b200);
            available.Add(b250);
            available.Add(b400);
            available.Add(b500);
            available.Add(b600);

            var resolver = new PackageResolver();
            var context = CreatePackageResolverContext(DependencyBehavior.Lowest, a100, available);
            var solution = resolver.Resolve(context, CancellationToken.None)
                .OrderBy(pi => pi.Id)
                .ToList();

            Assert.Equal(new PackageIdentity("a", new NuGetVersion(1, 0, 0)), solution[0], PackageIdentityComparer.Default);
            Assert.Equal(new PackageIdentity("b", new NuGetVersion(2, 0, 0)), solution[1], PackageIdentityComparer.Default);
        }

        [Fact]
        public void Resolver_Basic_AllUnlisted()
        {
            var a100 = new ResolverPackage("a", new NuGetVersion(1, 0, 0),
                new NuGet.Packaging.Core.PackageDependency[]
                    {
                        new NuGet.Packaging.Core.PackageDependency("b",
                            new VersionRange(new NuGetVersion(1, 0, 0), true, new NuGetVersion(5, 0, 0), true))
                    },
                false,
                false);

            var b200 = new ResolverPackage("b", new NuGetVersion(2, 0, 0), null, false, false);
            var b250 = new ResolverPackage("b", new NuGetVersion(2, 5, 0), null, false, false);
            var b400 = new ResolverPackage("b", new NuGetVersion(4, 0, 0), null, false, false);
            var b500 = new ResolverPackage("b", new NuGetVersion(5, 0, 0), null, false, false);
            var b600 = new ResolverPackage("b", new NuGetVersion(6, 0, 0), null, false, false);

            List<ResolverPackage> available = new List<ResolverPackage>();
            available.Add(a100);
            available.Add(b200);
            available.Add(b250);
            available.Add(b400);
            available.Add(b500);
            available.Add(b600);

            var resolver = new PackageResolver();
            var context = CreatePackageResolverContext(DependencyBehavior.Lowest, a100, available);
            var solution = resolver.Resolve(context, CancellationToken.None)
                .OrderBy(pi => pi.Id)
                .ToList();

            Assert.Equal(new PackageIdentity("a", new NuGetVersion(1, 0, 0)), solution[0], PackageIdentityComparer.Default);
            Assert.Equal(new PackageIdentity("b", new NuGetVersion(2, 0, 0)), solution[1], PackageIdentityComparer.Default);
        }

        [Fact]
        public void Resolver_Complex()
        {
            var target = new PackageIdentity("EntityFramework", NuGetVersion.Parse("7.0.0-beta4"));
            var packages = ResolverData.CreateEntityFrameworkPackageGraph();

            var resolver = new PackageResolver();
            var context = CreatePackageResolverContext(DependencyBehavior.Lowest, target, packages);
            var solution = resolver.Resolve(context, CancellationToken.None);

            Assert.True(solution.Contains(target, PackageIdentityComparer.Default));
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

        private static PackageResolverContext CreatePackageResolverContext(DependencyBehavior behavior,
            PackageIdentity target,
            IEnumerable<ResolverPackage> availablePackages)
        {
            var targets = new PackageIdentity[] { target };

            return CreatePackageResolverContext(behavior, targets, availablePackages);
        }

        private static PackageResolverContext CreatePackageResolverContext(DependencyBehavior behavior,
            IEnumerable<PackageIdentity> targets,
            IEnumerable<ResolverPackage> availablePackages)
        {
            return new PackageResolverContext(behavior,
                targets.Select(p => p.Id), Enumerable.Empty<string>(),
                Enumerable.Empty<PackageReference>(),
                targets,
                availablePackages,
                Enumerable.Empty<PackageSource>(),
                Common.NullLogger.Instance);
        }
    }
}
