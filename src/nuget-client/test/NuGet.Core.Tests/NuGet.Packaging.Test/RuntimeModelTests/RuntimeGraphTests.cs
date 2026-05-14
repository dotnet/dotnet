// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NuGet.Frameworks;
using NuGet.Versioning;
using Xunit;

namespace NuGet.RuntimeModel.Test
{
    public class RuntimeGraphTests
    {
        [Fact]
        public void FindRuntimeDependencies_VerifyExpandCaching()
        {
            var graph = new RuntimeGraph(new[]
                {
                    new RuntimeDescription("a"),
                    new RuntimeDescription("d"),
                    new RuntimeDescription("b", new[] { "a" }),
                    new RuntimeDescription("e", new[] { "d" }),
                    new RuntimeDescription("f", new[] { "e" }),
                    new RuntimeDescription("c", new[] { "b", "f" }),
                });

            var a = graph.ExpandRuntime("c");
            var b = graph.ExpandRuntime("c");

            ReferenceEquals(a, b).Should().BeTrue();
        }

        [Fact]
        public void FindRuntimeDependencies_VerifyExpandOrderForTree()
        {
            // C
            // - B
            // --- A
            // - F
            // --- E
            // ----- D
            var graph = new RuntimeGraph(new[]
                {
                    new RuntimeDescription("a"),
                    new RuntimeDescription("d"),
                    new RuntimeDescription("b", new[] { "a" }),
                    new RuntimeDescription("e", new[] { "d" }),
                    new RuntimeDescription("f", new[] { "e" }),
                    new RuntimeDescription("c", new[] { "b", "f" }),
                });

            var expected = new[] { "c", "b", "f", "a", "e", "d" };
            var actual = graph.ExpandRuntime("c").ToArray();
            for (var i = 0; i < expected.Length; i++)
            {
                Assert.Equal(expected[i], actual[i]);
            }
        }

        [Fact]
        public void FindRuntimeDependencies_VerifyExpandOrderForTree2()
        {
            // C
            // - B
            // --- A
            // ----- AA
            // - F
            // --- E
            var graph = new RuntimeGraph(new[]
                {
                    new RuntimeDescription("aa"),
                    new RuntimeDescription("e"),
                    new RuntimeDescription("a", new[] { "aa" }),
                    new RuntimeDescription("b", new[] { "a" }),
                    new RuntimeDescription("f", new[] { "e" }),
                    new RuntimeDescription("c", new[] { "b", "f" }),
                });

            var expected = new[] { "c", "b", "f", "a", "e", "aa" };
            var actual = graph.ExpandRuntime("c").ToArray();
            for (var i = 0; i < expected.Length; i++)
            {
                Assert.Equal(expected[i], actual[i]);
            }
        }

        [Fact]
        public void FindRuntimeDependencies_VerifyExpandOrderForTree3()
        {
            // C
            // - B
            // --- A
            // - F
            // --- E
            // ----- D
            // --- EE
            // ----- DD
            // - X
            // --- Y
            // ----- Z
            var graph = new RuntimeGraph(new[]
                {
                    new RuntimeDescription("a"),
                    new RuntimeDescription("d"),
                    new RuntimeDescription("y", new[] { "z" }),
                    new RuntimeDescription("x", new[] { "y" }),
                    new RuntimeDescription("b", new[] { "a" }),
                    new RuntimeDescription("e", new[] { "d" }),
                    new RuntimeDescription("ee", new[] { "dd" }),
                    new RuntimeDescription("f", new[] { "e", "ee" }),
                    new RuntimeDescription("c", new[] { "b", "f", "x" }),
                });

            var expected = new[] { "c", "b", "f", "x", "a", "e", "ee", "y", "d", "dd" };
            var actual = graph.ExpandRuntime("c").ToArray();
            for (var i = 0; i < expected.Length; i++)
            {
                Assert.Equal(expected[i], actual[i]);
            }
        }

        [Fact]
        public void FindRuntimeDependencies_MultipleDependenciesWithTieInTreeVerifyResult()
        {
            // C
            // - B
            // --- A -> Y 1.0.0
            // - F
            // --- E
            // ----- D -> Z 1.0.0
            var graph = new RuntimeGraph(new[]
                {
                    new RuntimeDescription("a", new[] { new RuntimeDependencySet("x", new[] { new RuntimePackageDependency("y", VersionRange.Parse("1.0.0")) }) }),
                    new RuntimeDescription("d", new[] { new RuntimeDependencySet("x", new[] { new RuntimePackageDependency("z", VersionRange.Parse("1.0.0")) }) }),
                    new RuntimeDescription("b", new[] { "a" }),
                    new RuntimeDescription("e", new[] { "d" }),
                    new RuntimeDescription("f", new[] { "e" }),
                    new RuntimeDescription("c", new[] { "b", "f" }),
                });

            var dependencies = graph.FindRuntimeDependencies("c", "x").ToList();
            dependencies.Count.Should().Be(1);
            dependencies[0].Id.Should().Be("y");
            dependencies[0].VersionRange.ToShortString().Should().Be("1.0.0");
        }

        [Fact]
        public void FindRuntimeDependencies_MultipleDependenciesWithTieInTreeVerifyResult2()
        {
            // C
            // - B
            // --- A
            // ----- AA -> Y 1.0.0
            // - F
            // --- E -> Z 1.0.0
            var graph = new RuntimeGraph(new[]
                {
                    new RuntimeDescription("aa", new[] { new RuntimeDependencySet("x", new[] { new RuntimePackageDependency("y", VersionRange.Parse("1.0.0")) }) }),
                    new RuntimeDescription("e", new[] { new RuntimeDependencySet("x", new[] { new RuntimePackageDependency("z", VersionRange.Parse("1.0.0")) }) }),
                    new RuntimeDescription("a", new[] { "aa" }),
                    new RuntimeDescription("b", new[] { "a" }),
                    new RuntimeDescription("f", new[] { "e" }),
                    new RuntimeDescription("c", new[] { "b", "f" }),
                });

            var dependencies = graph.FindRuntimeDependencies("c", "x").ToList();
            dependencies.Count.Should().Be(1);
            dependencies[0].Id.Should().Be("z");
            dependencies[0].VersionRange.ToShortString().Should().Be("1.0.0");
        }

        [Fact]
        public void FindRuntimeDependencies_MultipleDependenciesWithTieVerifyResult()
        {
            // A -> B and A -> C where both have dependencies
            var graph = new RuntimeGraph(new[]
                {
                    new RuntimeDescription("a", new[] { new RuntimeDependencySet("x", new[] { new RuntimePackageDependency("y", VersionRange.Parse("1.0.0")) }) }),
                    new RuntimeDescription("b", new[] { new RuntimeDependencySet("x", new[] { new RuntimePackageDependency("z", VersionRange.Parse("1.0.0")) }) }),
                    new RuntimeDescription("c", new[] { "a", "b" }),
                });

            var dependencies = graph.FindRuntimeDependencies("c", "x").ToList();
            dependencies.Count.Should().Be(1);
            dependencies[0].Id.Should().Be("y");
            dependencies[0].VersionRange.ToShortString().Should().Be("1.0.0");
        }

        [Fact]
        public void FindRuntimeDependencies_MultipleDependenciesVerifyNearestTaken()
        {
            var graph = new RuntimeGraph(new[]
                {
                    new RuntimeDescription("a", new[] { new RuntimeDependencySet("x", new[] { new RuntimePackageDependency("y", VersionRange.Parse("1.0.0")) }) }),
                    new RuntimeDescription("b", new[] { "a" }),
                    new RuntimeDescription("c", new[] { "b" }, new[] { new RuntimeDependencySet("x", new[] { new RuntimePackageDependency("z", VersionRange.Parse("1.0.0")) }) }),
                    new RuntimeDescription("d", new[] { "c" }),
                });

            var dependencies = graph.FindRuntimeDependencies("d", "x").ToList();
            dependencies.Count.Should().Be(1);
            dependencies[0].Id.Should().Be("z");
            dependencies[0].VersionRange.ToShortString().Should().Be("1.0.0");
        }

        [Fact]
        public void FindRuntimeDependencies_VerifyDuplicateDependency()
        {
            var graph = new RuntimeGraph(new[]
                {
                    new RuntimeDescription("a", new[] { new RuntimeDependencySet("x", new[] { new RuntimePackageDependency("y", VersionRange.Parse("1.0.0")) }) }),
                    new RuntimeDescription("b", new[] { "a" }),
                    new RuntimeDescription("c", new[] { "b" }, new[] { new RuntimeDependencySet("x", new[] { new RuntimePackageDependency("y", VersionRange.Parse("1.0.0")) }) }),
                    new RuntimeDescription("d", new[] { "c" }),
                });

            var dependencies = graph.FindRuntimeDependencies("d", "x").ToList();
            dependencies.Count.Should().Be(1);
            dependencies[0].Id.Should().Be("y");
            dependencies[0].VersionRange.ToShortString().Should().Be("1.0.0");
        }

        [Fact]
        public void FindRuntimeDependencies_VerifyCaching()
        {
            var graph = new RuntimeGraph(new[]
                {
                    new RuntimeDescription("a", new[] { new RuntimeDependencySet("x", new[] { new RuntimePackageDependency("y", VersionRange.Parse("1.0.0")) }) }),
                    new RuntimeDescription("b", new[] { "a" }),
                    new RuntimeDescription("c", new[] { "b" }),
                    new RuntimeDescription("d", new[] { "c" }),
                });

            var dependenciesA = graph.FindRuntimeDependencies("d", "x");
            var dependenciesB = graph.FindRuntimeDependencies("d", "x");

            ReferenceEquals(dependenciesA, dependenciesB).Should().BeTrue();
        }

        [Fact]
        public void FindRuntimeDependencies_VerifyDependencyOnCompatibleRID()
        {
            var graph = new RuntimeGraph(new[]
                {
                    new RuntimeDescription("a", new[] { new RuntimeDependencySet("x", new[] { new RuntimePackageDependency("y", VersionRange.Parse("1.0.0")) }) }),
                    new RuntimeDescription("b", new[] { "a" }),
                    new RuntimeDescription("c", new[] { "b" }),
                    new RuntimeDescription("d", new[] { "c" }),
                });

            var dependencies = graph.FindRuntimeDependencies("d", "x").ToList();
            dependencies.Count.Should().Be(1);
            dependencies[0].Id.Should().Be("y");
            dependencies[0].VersionRange.ToShortString().Should().Be("1.0.0");
        }

        [Fact]
        public void FindRuntimeDependencies_VerifyPackageIdIsCaseInsensitive()
        {
            var graph = new RuntimeGraph(new[]
                {
                    new RuntimeDescription("a", new[] { new RuntimeDependencySet("x", new[] { new RuntimePackageDependency("y", VersionRange.Parse("1.0.0")) }) })
                });

            var dependencies = graph.FindRuntimeDependencies("a", "X").ToList();
            dependencies.Count.Should().Be(1);
            dependencies[0].Id.Should().Be("y");
            dependencies[0].VersionRange.ToShortString().Should().Be("1.0.0");
        }

        [Fact]
        public void ExpandRuntimes_VerifyChain()
        {
            var graph = new RuntimeGraph(new[]
                {
                    new RuntimeDescription("a"),
                    new RuntimeDescription("b", new[] { "a" }),
                    new RuntimeDescription("c", new[] { "b" }),
                    new RuntimeDescription("d", new[] { "c" })
                });

            graph.ExpandRuntime("d").Should().BeEquivalentTo(new[] { "d", "c", "b", "a" });
        }

        [Fact]
        public void ExpandRuntimes_UnknownRuntimeVerifySelf()
        {
            var graph = RuntimeGraph.Empty;

            graph.ExpandRuntime("x").Should().BeEquivalentTo(new[] { "x" });
        }

        [Fact]
        public void ExpandRuntimes_UnknownRuntimeVerifyCompat()
        {
            var graph = RuntimeGraph.Empty;

            graph.AreCompatible("x", "x").Should().BeTrue();
        }

        [Fact]
        public void AreCompatible_VerifyCompatThroughChain()
        {
            var graph = new RuntimeGraph(new[]
                {
                    new RuntimeDescription("a"),
                    new RuntimeDescription("b", new[] { "a" }),
                    new RuntimeDescription("c", new[] { "b" }),
                    new RuntimeDescription("d", new[] { "c" })
                });

            graph.AreCompatible("d", "a").Should().BeTrue();
        }

        [Fact]
        public void AreCompatible_VerifyBasicOneWayCompat()
        {
            var graph = new RuntimeGraph(new[]
                {
                    new RuntimeDescription("win7"),
                    new RuntimeDescription("win8", new[] { "win7" })
                });

            graph.AreCompatible("win8", "win7").Should().BeTrue();
            graph.AreCompatible("win7", "win8").Should().BeFalse();
        }

        [Fact]
        public void AreCompatible_VerifyCircularCompat()
        {
            var graph = new RuntimeGraph(new[]
                {
                    new RuntimeDescription("win7", new[] {"win8" }),
                    new RuntimeDescription("win8", new[] { "win7" })
                });

            graph.AreCompatible("win8", "win7").Should().BeTrue();
            graph.AreCompatible("win7", "win8").Should().BeTrue();
        }

        [Fact]
        public void AreCompatible_VerifyCaseSensitiveCheck()
        {
            var leftGraph = new RuntimeGraph(new[]
                {
                    new RuntimeDescription("win7"),
                    new RuntimeDescription("win8", new[] { "win7" })
                });

            var rightGraph = new RuntimeGraph(new[]
                {
                    new RuntimeDescription("WIN7"),
                    new RuntimeDescription("WIN8", new[] { "WIN7" })
                });

            // Merge
            var graph = RuntimeGraph.Merge(leftGraph, rightGraph);

            graph.AreCompatible("WIN8", "win7").Should().BeFalse();
            graph.AreCompatible("win8", "WIN7").Should().BeFalse();
        }

        [Fact]
        public void GivenDifferentCasingsVerifyMergeKeepsBoth()
        {
            var leftGraph = new RuntimeGraph(new[]
                {
                    new RuntimeDescription("any"),
                    new RuntimeDescription("win8", new[] { "any" })
                });

            var rightGraph = new RuntimeGraph(new[]
                {
                    new RuntimeDescription("any"),
                    new RuntimeDescription("WIN8", new[] { "any" })
                });

            // Merge
            var graph = RuntimeGraph.Merge(leftGraph, rightGraph);

            graph.Runtimes.Keys.Should().BeEquivalentTo(new List<string>() { "any", "win8", "WIN8" });
        }

        [Fact]
        public void MergingInEmptyGraphHasNoEffect()
        {
            var graph = new RuntimeGraph(new[]
                {
                    new RuntimeDescription("any"),
                    new RuntimeDescription("win8", new[] { "any" })
                });
            var newGraph = RuntimeGraph.Merge(graph, RuntimeGraph.Empty);
            Assert.Equal(new RuntimeGraph(new[]
                {
                    new RuntimeDescription("any"),
                    new RuntimeDescription("win8", new[] { "any" })
                }), newGraph);
        }

        [Fact]
        public void MergingAddsCompletelyNewRuntimes()
        {
            var leftGraph = new RuntimeGraph(new[]
                {
                    new RuntimeDescription("any"),
                    new RuntimeDescription("win8", new[] { "any", "win7" })
                });
            var rightGraph = new RuntimeGraph(new[]
                {
                    new RuntimeDescription("win7")
                });
            var graph = RuntimeGraph.Merge(leftGraph, rightGraph);
            Assert.Equal(new RuntimeGraph(new[]
                {
                    new RuntimeDescription("any"),
                    new RuntimeDescription("win8", new[] { "any", "win7" }),
                    new RuntimeDescription("win7")
                }), graph);
        }

        [Fact]
        public void MergingCombinesDependencySetsInRuntimesDefinedInBoth()
        {
            var leftGraph = new RuntimeGraph(new[]
                {
                    new RuntimeDescription("any"),
                    new RuntimeDescription("win8", new[] { "any", "win7" }, new[]
                        {
                            new RuntimeDependencySet("Foo"),
                        })
                });
            var rightGraph = new RuntimeGraph(new[]
                {
                    new RuntimeDescription("win8", new[]
                        {
                            new RuntimeDependencySet("Bar")
                        })
                });
            var graph = RuntimeGraph.Merge(leftGraph, rightGraph);
            Assert.Equal(new RuntimeGraph(new[]
                {
                    new RuntimeDescription("any"),
                    new RuntimeDescription("win8", new[] { "any", "win7" }, new[]
                        {
                            new RuntimeDependencySet("Foo"),
                            new RuntimeDependencySet("Bar")
                        }),
                }), graph);
        }

        [Fact]
        public void MergingReplacesDependencySetsDefinedInBoth()
        {
            var leftGraph = new RuntimeGraph(new[]
                {
                    new RuntimeDescription("any"),
                    new RuntimeDescription("win8", new[] { "any", "win7" }, new[]
                        {
                            new RuntimeDependencySet("Foo", new[]
                                {
                                    new RuntimePackageDependency("Foo.win8", new VersionRange(new NuGetVersion(1, 2, 3)))
                                }),
                        })
                });
            var rightGraph = new RuntimeGraph(new[]
                {
                    new RuntimeDescription("win8", new[]
                        {
                            new RuntimeDependencySet("Foo", new[]
                                {
                                    new RuntimePackageDependency("Foo.better.win8", new VersionRange(new NuGetVersion(4, 5, 6)))
                                }),
                        })
                });
            var graph = RuntimeGraph.Merge(leftGraph, rightGraph);
            Assert.Equal(new RuntimeGraph(new[]
                {
                    new RuntimeDescription("any"),
                    new RuntimeDescription("win8", new[] { "any", "win7" }, new[]
                        {
                            new RuntimeDependencySet("Foo", new[]
                                {
                                    new RuntimePackageDependency("Foo.better.win8", new VersionRange(new NuGetVersion(4, 5, 6)))
                                }),
                        }),
                }), graph);
        }

        [Theory]
        [InlineData("win7", "win7")]
        [InlineData("win8", "win8,win7")]
        [InlineData("win8-x86", "win8-x86,win8,win7-x86,win7")]
        public void ExpandShouldExpandRuntimeBasedOnGraph(string start, string expanded)
        {
            var graph = new RuntimeGraph(new[]
                {
                    new RuntimeDescription("win8-x86", new[] { "win8", "win7-x86" }),
                    new RuntimeDescription("win8", new[] { "win7" }),
                    new RuntimeDescription("win7-x86", new[] { "win7" }),
                    new RuntimeDescription("win7"),
                });
            Assert.Equal(
                expanded.Split(','),
                graph.ExpandRuntime(start).ToArray());
        }

        [Fact]
        public void MergeReplacesCompatibilityProfilesDefinedInRightWithValuesFromLeftIfLeftNonEmpty()
        {
            var leftGraph = new RuntimeGraph(new[]
            {
                new CompatibilityProfile("frob", new []
                {
                    new FrameworkRuntimePair(FrameworkConstants.CommonFrameworks.Dnx452, "frob")
                })
            });
            var rightGraph = new RuntimeGraph(new[]
            {
                new CompatibilityProfile("frob", new []
                {
                    new FrameworkRuntimePair(FrameworkConstants.CommonFrameworks.DnxCore50, "blob")
                })
            });
            var graph = RuntimeGraph.Merge(leftGraph, rightGraph);
            Assert.Equal(leftGraph, graph);
        }

        [Fact]
        public void MergeReplacesCompatibilityProfilesDefinedInRightIntoLeftIfLeftIsEmpty()
        {
            var leftGraph = new RuntimeGraph(new[]
            {
                new CompatibilityProfile("frob")
            });
            var rightGraph = new RuntimeGraph(new[]
            {
                new CompatibilityProfile("frob", new []
                {
                    new FrameworkRuntimePair(FrameworkConstants.CommonFrameworks.DnxCore50, "blob")
                })
            });
            var graph = RuntimeGraph.Merge(leftGraph, rightGraph);
            Assert.Equal(rightGraph, graph);
        }
    }
}
