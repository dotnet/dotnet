// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.Packaging;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Xunit;

namespace NuGet.ProjectModel.Test
{
    public class TargetFrameworkInformationTests
    {
        [Fact]
        public void Equals_WithSameObject_ReturnsTrue()
        {
            // Arrange
            var tfi = CreateTargetFrameworkInformation();

            // Act & Assert
            tfi.Equals(tfi).Should().BeTrue();
        }

        [Fact]
        public void Equals_WithSameContent_ReturnsTrue()
        {
            // Arrange
            var tfi = CreateTargetFrameworkInformation();
            var tfiTwin = CreateTargetFrameworkInformation();

            // Act & Assert
            tfi.Equals(tfiTwin).Should().BeTrue();
        }

        [Fact]
        public void Equals_WithDifferentObject_CentralDependency_ReturnsFalse()
        {
            // Arrange
            var tfiFoo = CreateTargetFrameworkInformation(new List<CentralPackageVersion>() { new CentralPackageVersion("foo", VersionRange.All) });
            var tfiBar = CreateTargetFrameworkInformation(new List<CentralPackageVersion>() { new CentralPackageVersion("bar", VersionRange.All) });

            // Act & Assert
            tfiFoo.Equals(tfiBar).Should().BeFalse();
        }

        [Fact]
        public void Equals_OnClone_ReturnsTrue()
        {
            // Arrange
            var tfi = CreateTargetFrameworkInformation();
            var tfiClone = tfi.Clone();

            // Act & Assert
            tfi.Equals(tfiClone).Should().BeTrue();
            Assert.NotSame(tfi, tfiClone);
            Assert.NotSame(tfi.CentralPackageVersions, tfiClone.CentralPackageVersions);
            Assert.NotSame(tfi.Dependencies, tfiClone.Dependencies);
            Assert.NotSame(tfi.Imports, tfiClone.Imports);
            Assert.NotSame(tfi.DownloadDependencies, tfiClone.DownloadDependencies);
            Assert.NotSame(tfi.FrameworkReferences, tfiClone.FrameworkReferences);
        }

        [Theory]
        [InlineData("net461", "net461", true)]
        [InlineData("net461", "NET461", true)]
        [InlineData("net461", "", false)]
        public void Equals_WithTargetAlias(string left, string right, bool expected)
        {
            var leftSide = new TargetFrameworkInformation()
            {
                FrameworkName = NuGetFramework.AnyFramework,
                TargetAlias = left
            };

            var rightSide = new TargetFrameworkInformation()
            {
                FrameworkName = NuGetFramework.AnyFramework,
                TargetAlias = right
            };

            AssertEquality(expected, leftSide, rightSide);
        }

        [Theory]
        [InlineData("net461", "net461", true)]
        [InlineData("net461", "NET461", true)]
        [InlineData("net461", "netstandard2.0", false)]
        public void Equals_WithFrameworkName(string left, string right, bool expected)
        {
            var leftSide = new TargetFrameworkInformation()
            {
                FrameworkName = NuGetFramework.Parse(left)
            };

            var rightSide = new TargetFrameworkInformation()
            {
                FrameworkName = NuGetFramework.Parse(right)
            };

            AssertEquality(expected, leftSide, rightSide);
        }

        [Theory]
        [InlineData("net461", "net461", true)]
        [InlineData("net461", "NET461", true)]
        [InlineData("net472;net461", "net472;NET461", true)]
        [InlineData("net461", "", false)]
        [InlineData("net461;net472", "net472;net461", false)]
        public void Equals_WithImports(string left, string right, bool expected)
        {
            var leftSide = new TargetFrameworkInformation()
            {
                FrameworkName = NuGetFramework.AnyFramework,
                Imports = left.Split(';').Select(e => NuGetFramework.Parse(e)).ToList()
            };

            var rightSide = new TargetFrameworkInformation()
            {
                FrameworkName = NuGetFramework.AnyFramework,
                Imports = right.Split(';').Select(e => NuGetFramework.Parse(e)).ToList()
            };

            AssertEquality(expected, leftSide, rightSide);
        }

        [Theory]
        [InlineData(true, true, true)]
        [InlineData(true, false, false)]
        [InlineData(false, true, false)]
        [InlineData(false, false, true)]
        public void Equals_WithAssetTargetFallback(bool left, bool right, bool expected)
        {
            var leftSide = new TargetFrameworkInformation()
            {
                FrameworkName = NuGetFramework.AnyFramework,
                AssetTargetFallback = left
            };

            var rightSide = new TargetFrameworkInformation()
            {
                FrameworkName = NuGetFramework.AnyFramework,
                AssetTargetFallback = right
            };

            AssertEquality(expected, leftSide, rightSide);
        }

        [Theory]
        [InlineData(true, true, true)]
        [InlineData(true, false, false)]
        [InlineData(false, true, false)]
        [InlineData(false, false, true)]
        public void Equals_WithWarn(bool left, bool right, bool expected)
        {
            var leftSide = new TargetFrameworkInformation()
            {
                FrameworkName = NuGetFramework.AnyFramework,
                Warn = left
            };

            var rightSide = new TargetFrameworkInformation()
            {
                FrameworkName = NuGetFramework.AnyFramework,
                Warn = right
            };

            AssertEquality(expected, leftSide, rightSide);
        }

        [PlatformTheory(Platform = Platform.Windows)]
        [InlineData(@"C:\nugetLocation", @"C:\nugetLocation", true)]
        [InlineData(@"C:\nugetLocation", @"C:\NugetLocation", true)]
        [InlineData(@"C:\nugetLocation", @"C:\nugetLocation2", false)]
        [InlineData(@"C:\nugetLocation", "", false)]
        public void Equals_CaseInsensitive_WithRuntimeIdentifierGraphPath(string left, string right, bool expected)
        {
            var leftSide = new TargetFrameworkInformation()
            {
                FrameworkName = NuGetFramework.AnyFramework,
                RuntimeIdentifierGraphPath = left
            };

            var rightSide = new TargetFrameworkInformation()
            {
                FrameworkName = NuGetFramework.AnyFramework,
                RuntimeIdentifierGraphPath = right
            };

            AssertEquality(expected, leftSide, rightSide);
        }

        [PlatformTheory(Platform = Platform.Linux)]
        [InlineData(@"C:\nugetLocation", @"C:\nugetLocation", true)]
        [InlineData(@"C:\nugetLocation", @"C:\NugetLocation", false)]
        [InlineData(@"C:\nugetLocation", @"C:\nugetLocation2", false)]
        [InlineData(@"C:\nugetLocation", "", false)]
        public void Equals_CaseSensitive_WithRuntimeIdentifierGraphPath(string left, string right, bool expected)
        {
            var leftSide = new TargetFrameworkInformation()
            {
                RuntimeIdentifierGraphPath = left
            };

            var rightSide = new TargetFrameworkInformation()
            {
                RuntimeIdentifierGraphPath = right
            };

            AssertEquality(expected, leftSide, rightSide);
        }

        [Theory]
        [InlineData("a;b", "a;b", true)]
        [InlineData("a;B", "A;b", true)]
        [InlineData("B;a", "A;b", true)]
        [InlineData("a;B", "b;a", true)]
        [InlineData("a;c;b", "c;B;a", true)]
        [InlineData("B;a;c", "A;b", false)]
        [InlineData("B;a", "A;b;c", false)]
        public void Equals_WithDependencies(string left, string right, bool expected)
        {
            var leftSide = new TargetFrameworkInformation()
            {
                Dependencies = left.Split(';').Select(
                    e => new LibraryDependency()
                    {
                        LibraryRange = new LibraryRange()
                        {
                            Name = e,
                            VersionRange = VersionRange.Parse("1.0.0")
                        }
                    }).ToList()
            };

            var rightSide = new TargetFrameworkInformation()
            {
                Dependencies = right.Split(';').Select(
                    e => new LibraryDependency()
                    {
                        LibraryRange = new LibraryRange()
                        {
                            Name = e,
                            VersionRange = VersionRange.Parse("1.0.0")
                        }
                    }).ToList()
            };

            AssertEquality(expected, leftSide, rightSide);
        }

        [Theory]
        [InlineData("a;b", "a;b", true)]
        [InlineData("a;B", "A;b", true)]
        [InlineData("B;a", "A;b", true)]
        [InlineData("B;a;c", "A;b", false)]
        [InlineData("B;a", "A;b;c", false)]
        public void Equals_WithDownloadDependencies(string left, string right, bool expected)
        {
            var leftSide = new TargetFrameworkInformation();
            leftSide.DownloadDependencies.AddRange(left.Split(';').Select(e => new DownloadDependency(e, VersionRange.Parse("1.0.0"))));

            var rightSide = new TargetFrameworkInformation();
            rightSide.DownloadDependencies.AddRange(right.Split(';').Select(e => new DownloadDependency(e, VersionRange.Parse("1.0.0"))));

            AssertEquality(expected, leftSide, rightSide);
        }

        [Theory]
        [InlineData("a;b", "a;b", true)]
        [InlineData("a;B", "A;b", true)]
        [InlineData("B;a", "A;b", true)]
        [InlineData("B;a;c", "A;b", false)]
        [InlineData("B;a", "A;b;c", false)]
        public void Equals_WithFrameworkReferences(string left, string right, bool expected)
        {
            var leftSide = new TargetFrameworkInformation();
            leftSide.FrameworkReferences.AddRange(left.Split(';')
                .Select(e => new FrameworkDependency(e, FrameworkDependencyFlags.All)));

            var rightSide = new TargetFrameworkInformation();
            rightSide.FrameworkReferences.AddRange(right.Split(';')
                .Select(e => new FrameworkDependency(e, FrameworkDependencyFlags.All)));

            AssertEquality(expected, leftSide, rightSide);
        }

        [Theory]
        [InlineData("a;b", "a;b", true)]
        [InlineData("a;B", "A;b", true)]
        [InlineData("B;a", "A;b", true)]
        [InlineData("B;a;c", "A;b", false)]
        [InlineData("B;a", "A;b;c", false)]
        public void Equals_WithCentralDependencies(string left, string right, bool expected)
        {
            var leftSide = new TargetFrameworkInformation()
            {
                FrameworkName = NuGetFramework.AnyFramework
            };
            foreach (var entry in left.Split(';'))
            {
                leftSide.CentralPackageVersions.Add(entry, new CentralPackageVersion(entry, VersionRange.All));
            }

            var rightSide = new TargetFrameworkInformation()
            {
                FrameworkName = NuGetFramework.AnyFramework
            };
            foreach (var entry in right.Split(';'))
            {
                rightSide.CentralPackageVersions.Add(entry, new CentralPackageVersion(entry, VersionRange.All));
            }

            AssertEquality(expected, leftSide, rightSide);
        }

        [Theory]
        [InlineData("net461", "net461", true)]
        [InlineData("net461", "NET461", true)]
        [InlineData("net461", "", false)]
        public void HashCode_WithTargetAlias(string left, string right, bool expected)
        {
            var leftSide = new TargetFrameworkInformation()
            {
                FrameworkName = NuGetFramework.AnyFramework,
                TargetAlias = left
            };

            var rightSide = new TargetFrameworkInformation()
            {
                FrameworkName = NuGetFramework.AnyFramework,
                TargetAlias = right
            };

            AssertHashCode(expected, leftSide, rightSide);
        }

        [Theory]
        [InlineData("net461", "net461", true)]
        [InlineData("net461", "NET461", true)]
        [InlineData("net461", "netstandard2.0", false)]
        public void HashCode_WithFrameworkName(string left, string right, bool expected)
        {
            var leftSide = new TargetFrameworkInformation()
            {
                FrameworkName = NuGetFramework.Parse(left)
            };

            var rightSide = new TargetFrameworkInformation()
            {
                FrameworkName = NuGetFramework.Parse(right)
            };

            AssertHashCode(expected, leftSide, rightSide);
        }

        [Theory]
        [InlineData("net461", "net461", true)]
        [InlineData("net461", "NET461", true)]
        [InlineData("net472;net461", "net472;NET461", true)]
        [InlineData("net461", "", false)]
        [InlineData("net461;net472", "net472;net461", false)]
        public void HashCode_WithImports(string left, string right, bool expected)
        {
            var leftSide = new TargetFrameworkInformation()
            {
                FrameworkName = NuGetFramework.AnyFramework,
                Imports = left.Split(';').Select(e => NuGetFramework.Parse(e)).ToList()
            };

            var rightSide = new TargetFrameworkInformation()
            {
                FrameworkName = NuGetFramework.AnyFramework,
                Imports = right.Split(';').Select(e => NuGetFramework.Parse(e)).ToList()
            };

            AssertHashCode(expected, leftSide, rightSide);
        }

        [Theory]
        [InlineData(true, true, true)]
        [InlineData(true, false, false)]
        [InlineData(false, true, false)]
        [InlineData(false, false, true)]
        public void HashCode_WithAssetTargetFallback(bool left, bool right, bool expected)
        {
            var leftSide = new TargetFrameworkInformation()
            {
                FrameworkName = NuGetFramework.AnyFramework,
                AssetTargetFallback = left
            };

            var rightSide = new TargetFrameworkInformation()
            {
                FrameworkName = NuGetFramework.AnyFramework,
                AssetTargetFallback = right
            };

            AssertHashCode(expected, leftSide, rightSide);
        }

        [Theory]
        [InlineData(true, true, true)]
        [InlineData(true, false, false)]
        [InlineData(false, true, false)]
        [InlineData(false, false, true)]
        public void HashCode_WithWarn(bool left, bool right, bool expected)
        {
            var leftSide = new TargetFrameworkInformation()
            {
                FrameworkName = NuGetFramework.AnyFramework,
                Warn = left
            };

            var rightSide = new TargetFrameworkInformation()
            {
                FrameworkName = NuGetFramework.AnyFramework,
                Warn = right
            };

            AssertHashCode(expected, leftSide, rightSide);
        }

        [PlatformTheory(Platform = Platform.Windows)]
        [InlineData(@"C:\nugetLocation", @"C:\nugetLocation", true)]
        [InlineData(@"C:\nugetLocation", @"C:\NugetLocation", true)]
        [InlineData(@"C:\nugetLocation", @"C:\nugetLocation2", false)]
        [InlineData(@"C:\nugetLocation", "", false)]
        public void HashCode_CaseInsensitive_WithRuntimeIdentifierGraphPath(string left, string right, bool expected)
        {
            var leftSide = new TargetFrameworkInformation()
            {
                FrameworkName = NuGetFramework.AnyFramework,
                RuntimeIdentifierGraphPath = left
            };

            var rightSide = new TargetFrameworkInformation()
            {
                FrameworkName = NuGetFramework.AnyFramework,
                RuntimeIdentifierGraphPath = right
            };

            AssertHashCode(expected, leftSide, rightSide);
        }

        [PlatformTheory(Platform = Platform.Linux)]
        [InlineData(@"C:\nugetLocation", @"C:\nugetLocation", true)]
        [InlineData(@"C:\nugetLocation", @"C:\NugetLocation", false)]
        [InlineData(@"C:\nugetLocation", @"C:\nugetLocation2", false)]
        [InlineData(@"C:\nugetLocation", "", false)]
        public void HashCode_CaseSensitive_WithRuntimeIdentifierGraphPath(string left, string right, bool expected)
        {
            var leftSide = new TargetFrameworkInformation()
            {
                RuntimeIdentifierGraphPath = left
            };

            var rightSide = new TargetFrameworkInformation()
            {
                RuntimeIdentifierGraphPath = right
            };

            AssertHashCode(expected, leftSide, rightSide);
        }

        [Theory]
        [InlineData("a;b", "a;b", true)]
        [InlineData("a;B", "A;b", true)]
        [InlineData("B;a", "A;b", true)]
        [InlineData("B;a;c", "A;b", false)]
        [InlineData("B;a", "A;b;c", false)]
        public void HashCode_WithDependencies(string left, string right, bool expected)
        {
            var leftSide = new TargetFrameworkInformation()
            {
                Dependencies = left.Split(';').Select(
                    e => new LibraryDependency()
                    {
                        LibraryRange = new LibraryRange()
                        {
                            Name = e,
                            VersionRange = VersionRange.Parse("1.0.0")
                        }
                    }).ToList()
            };

            var rightSide = new TargetFrameworkInformation()
            {
                Dependencies = right.Split(';').Select(
                    e => new LibraryDependency()
                    {
                        LibraryRange = new LibraryRange()
                        {
                            Name = e,
                            VersionRange = VersionRange.Parse("1.0.0")
                        }
                    }).ToList()
            };

            AssertHashCode(expected, leftSide, rightSide);
        }

        [Theory]
        [InlineData("a;b", "a;b", true)]
        [InlineData("a;B", "A;b", true)]
        [InlineData("B;a", "A;b", true)]
        [InlineData("B;a;c", "A;b", false)]
        [InlineData("B;a", "A;b;c", false)]
        public void HashCode_WithDownloadDependencies(string left, string right, bool expected)
        {
            var leftSide = new TargetFrameworkInformation();
            leftSide.DownloadDependencies.AddRange(left.Split(';').Select(e => new DownloadDependency(e, VersionRange.Parse("1.0.0"))));

            var rightSide = new TargetFrameworkInformation();
            rightSide.DownloadDependencies.AddRange(right.Split(';').Select(e => new DownloadDependency(e, VersionRange.Parse("1.0.0"))));

            AssertHashCode(expected, leftSide, rightSide);
        }

        [Theory]
        [InlineData("a;b", "a;b", true)]
        [InlineData("a;B", "A;b", true)]
        [InlineData("B;a", "A;b", true)]
        [InlineData("B;a;c", "A;b", false)]
        [InlineData("B;a", "A;b;c", false)]
        public void HashCode_WithFrameworkReferences(string left, string right, bool expected)
        {
            var leftSide = new TargetFrameworkInformation();
            leftSide.FrameworkReferences.AddRange(left.Split(';')
                .Select(e => new FrameworkDependency(e, FrameworkDependencyFlags.All)));

            var rightSide = new TargetFrameworkInformation();
            rightSide.FrameworkReferences.AddRange(right.Split(';')
                .Select(e => new FrameworkDependency(e, FrameworkDependencyFlags.All)));

            AssertHashCode(expected, leftSide, rightSide);
        }

        [Theory]
        [InlineData("a;b", "a;b", true)]
        [InlineData("a;B", "A;b", true)]
        [InlineData("B;a", "A;b", true)]
        [InlineData("B;a;c", "A;b", false)]
        [InlineData("B;a", "A;b;c", false)]
        public void HashCode_WithCentralDependencies(string left, string right, bool expected)
        {
            var leftSide = new TargetFrameworkInformation()
            {
                FrameworkName = NuGetFramework.AnyFramework
            };
            foreach (var entry in left.Split(';'))
            {
                leftSide.CentralPackageVersions.Add(entry, new CentralPackageVersion(entry, VersionRange.All));
            }

            var rightSide = new TargetFrameworkInformation()
            {
                FrameworkName = NuGetFramework.AnyFramework
            };
            foreach (var entry in right.Split(';'))
            {
                rightSide.CentralPackageVersions.Add(entry, new CentralPackageVersion(entry, VersionRange.All));
            }

            AssertHashCode(expected, leftSide, rightSide);
        }

        private TargetFrameworkInformation CreateTargetFrameworkInformation()
        {
            return CreateTargetFrameworkInformation(new List<CentralPackageVersion>() { new CentralPackageVersion("foo", VersionRange.All), new CentralPackageVersion("bar", VersionRange.AllStable) });
        }

        private TargetFrameworkInformation CreateTargetFrameworkInformation(List<CentralPackageVersion> centralVersionDependencies)
        {
            NuGetFramework nugetFramework = new NuGetFramework("net40");
            var dependencyFoo = new LibraryDependency(new LibraryRange("foo", VersionRange.All, LibraryDependencyTarget.All),
                LibraryIncludeFlags.All,
                LibraryIncludeFlags.All,
                new List<Common.NuGetLogCode>(),
                autoReferenced: true,
                generatePathProperty: true,
                versionCentrallyManaged: false,
                LibraryDependencyReferenceType.Direct,
                aliases: "Alias",
                versionOverride: null);

            var downloadDependency = new DownloadDependency("foo", VersionRange.All);
            var frameworkDependency = new FrameworkDependency("framework", FrameworkDependencyFlags.All);

            var dependencies = new List<LibraryDependency>() { dependencyFoo };
            var assetTargetFallback = true;
            var warn = false;

            TargetFrameworkInformation tfi = new TargetFrameworkInformation()
            {
                AssetTargetFallback = assetTargetFallback,
                Dependencies = dependencies,
                Warn = warn,
                FrameworkName = nugetFramework,
            };

            foreach (var cdep in centralVersionDependencies)
            {
                tfi.CentralPackageVersions.Add(cdep.Name, cdep);
            }

            tfi.DownloadDependencies.Add(downloadDependency);
            tfi.FrameworkReferences.Add(frameworkDependency);

            return tfi;
        }

        private static void AssertEquality(bool expected, TargetFrameworkInformation leftSide, TargetFrameworkInformation rightSide)
        {
            if (expected)
            {
                leftSide.Should().Be(rightSide);
            }
            else
            {
                leftSide.Should().NotBe(rightSide);
            }

            AssertClone(expected, leftSide, rightSide);
        }

        private static void AssertHashCode(bool expected, TargetFrameworkInformation leftSide, TargetFrameworkInformation rightSide)
        {
            if (expected)
            {
                leftSide.GetHashCode().Should().Be(rightSide.GetHashCode());
            }
            else
            {
                leftSide.GetHashCode().Should().NotBe(rightSide.GetHashCode());
            }

            AssertClone(expected, leftSide, rightSide);
        }

        private static void AssertClone(bool expected, TargetFrameworkInformation leftSide, TargetFrameworkInformation rightSide)
        {
            var leftClone = leftSide.Clone();
            var rightClone = rightSide.Clone();

            if (expected)
            {
                leftClone.GetHashCode().Should().Be(rightClone.GetHashCode());
                leftClone.Should().Be(rightClone);
            }
            else
            {
                leftClone.GetHashCode().Should().NotBe(rightClone.GetHashCode());
                leftClone.Should().NotBe(rightClone);
            }
        }
    }
}
