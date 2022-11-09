// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NuGet.Packaging.Core;
using NuGet.Versioning;
using Xunit;

namespace NuGet.ProjectModel.Test
{
    public class LockFileTargetLibraryTests
    {
        [Theory]
        [InlineData("project", "project", true)]
        [InlineData("project", "PROJECT", false)]
        [InlineData("project", "package", false)]
        public void Equals_WithName(string left, string right, bool expected)
        {
            var leftSide = new LockFileTargetLibrary()
            {
                Name = left
            };

            var rightSide = new LockFileTargetLibrary()
            {
                Name = right
            };

            // Act & Assert
            if (expected)
            {
                leftSide.Should().Be(rightSide);
            }
            else
            {
                leftSide.Should().NotBe(rightSide);
            }
        }

        [Theory]
        [InlineData("2.1.0", "2.1.0", true)]
        [InlineData("2.1.0", "2.1.0-preview.2", false)]
        [InlineData("1.1.1", "2.2.2", false)]
        public void Equals_WithVersion(string left, string right, bool expected)
        {
            var leftSide = new LockFileTargetLibrary()
            {
                Version = NuGetVersion.Parse(left)
            };

            var rightSide = new LockFileTargetLibrary()
            {
                Version = NuGetVersion.Parse(right)
            };

            // Act & Assert
            if (expected)
            {
                leftSide.Should().Be(rightSide);
            }
            else
            {
                leftSide.Should().NotBe(rightSide);
            }
        }

        [Theory]
        [InlineData("net461", "net461", true)]
        [InlineData("net461", "NET461", false)]
        [InlineData("net46", "netcoreapp10", false)]
        public void Equals_WithFramework(string left, string right, bool expected)
        {
            var leftSide = new LockFileTargetLibrary()
            {
                Framework = left
            };

            var rightSide = new LockFileTargetLibrary()
            {
                Framework = right
            };

            // Act & Assert
            if (expected)
            {
                leftSide.Should().Be(rightSide);
            }
            else
            {
                leftSide.Should().NotBe(rightSide);
            }
        }

        [Theory]
        [InlineData("project", "project", true)]
        [InlineData("project", "PROJECT", false)]
        [InlineData("project", "package", false)]
        public void Equals_WithType(string left, string right, bool expected)
        {
            var leftSide = new LockFileTargetLibrary()
            {
                Type = left
            };

            var rightSide = new LockFileTargetLibrary()
            {
                Type = right
            };

            // Act & Assert
            if (expected)
            {
                leftSide.Should().Be(rightSide);
            }
            else
            {
                leftSide.Should().NotBe(rightSide);
            }
        }

        [Theory]
        [InlineData("a", "a", true)]
        [InlineData("A;b", "a;B", true)]
        [InlineData("a;b;c", "c;a;B", true)]
        [InlineData("a;b;c;d", "c;a;B", false)]
        public void Equals_WithPackageDependency(string left, string right, bool expected)
        {
            var leftSide = new LockFileTargetLibrary()
            {
                Dependencies = left.Split(';').Select(e => new PackageDependency(e, VersionRange.Parse("1.0.0"))).ToList()
            };

            var rightSide = new LockFileTargetLibrary()
            {
                Dependencies = right.Split(';').Select(e => new PackageDependency(e, VersionRange.Parse("1.0.0"))).ToList()
            };

            // Act & Assert
            if (expected)
            {
                leftSide.Should().Be(rightSide);
            }
            else
            {
                leftSide.Should().NotBe(rightSide);
            }
        }

        [Theory]
        [InlineData("a", "a", true)]
        [InlineData("A;b", "a;B", true)]
        [InlineData("a;b;c", "c;a;B", true)]
        [InlineData("a;b;c;d", "c;a;B", false)]
        public void Equals_WithFrameworkAssemblies(string left, string right, bool expected)
        {
            var leftSide = new LockFileTargetLibrary()
            {
                FrameworkAssemblies = left.Split(';').Select(e => e).ToList()
            };

            var rightSide = new LockFileTargetLibrary()
            {
                FrameworkAssemblies = right.Split(';').Select(e => e).ToList()
            };

            // Act & Assert
            if (expected)
            {
                leftSide.Should().Be(rightSide);
            }
            else
            {
                leftSide.Should().NotBe(rightSide);
            }
        }

        [Theory]
        [InlineData("a", "a", true)]
        [InlineData("A;b", "a;B", true)]
        [InlineData("a;b;c", "c;a;B", true)]
        [InlineData("a;b;c;d", "c;a;B", false)]
        public void Equals_WithFrameworkReferences(string left, string right, bool expected)
        {
            var leftSide = new LockFileTargetLibrary()
            {
                FrameworkReferences = left.Split(';').Select(e => e).ToList()
            };

            var rightSide = new LockFileTargetLibrary()
            {
                FrameworkReferences = right.Split(';').Select(e => e).ToList()
            };

            // Act & Assert
            if (expected)
            {
                leftSide.Should().Be(rightSide);
            }
            else
            {
                leftSide.Should().NotBe(rightSide);
            }
        }

        [Theory]
        [InlineData("a", "a", true)]
        [InlineData("A;b", "a;B", true)]
        [InlineData("a;b;c", "c;a;B", true)]
        [InlineData("a;b;c;d", "c;a;B", false)]
        public void Equals_WithRuntimeAssemblies(string left, string right, bool expected)
        {
            var leftSide = new LockFileTargetLibrary()
            {
                RuntimeAssemblies = left.Split(';').Select(e => new LockFileItem(e)).ToList()
            };

            var rightSide = new LockFileTargetLibrary()
            {
                RuntimeAssemblies = right.Split(';').Select(e => new LockFileItem(e)).ToList()
            };

            // Act & Assert
            if (expected)
            {
                leftSide.Should().Be(rightSide);
            }
            else
            {
                leftSide.Should().NotBe(rightSide);
            }
        }

        [Theory]
        [InlineData("a", "a", true)]
        [InlineData("A;b", "a;B", true)]
        [InlineData("a;b;c", "c;a;B", true)]
        [InlineData("a;b;c;d", "c;a;B", false)]
        public void Equals_WithResourceAssemblies(string left, string right, bool expected)
        {
            var leftSide = new LockFileTargetLibrary()
            {
                ResourceAssemblies = left.Split(';').Select(e => new LockFileItem(e)).ToList()
            };

            var rightSide = new LockFileTargetLibrary()
            {
                ResourceAssemblies = right.Split(';').Select(e => new LockFileItem(e)).ToList()
            };

            // Act & Assert
            if (expected)
            {
                leftSide.Should().Be(rightSide);
            }
            else
            {
                leftSide.Should().NotBe(rightSide);
            }
        }

        [Theory]
        [InlineData("a", "a", true)]
        [InlineData("A;b", "a;B", true)]
        [InlineData("a;b;c", "c;a;B", true)]
        [InlineData("a;b;c;d", "c;a;B", false)]
        public void Equals_WithCompileTimeAssemblies(string left, string right, bool expected)
        {
            var leftSide = new LockFileTargetLibrary()
            {
                CompileTimeAssemblies = left.Split(';').Select(e => new LockFileItem(e)).ToList()
            };

            var rightSide = new LockFileTargetLibrary()
            {
                CompileTimeAssemblies = right.Split(';').Select(e => new LockFileItem(e)).ToList()
            };

            // Act & Assert
            if (expected)
            {
                leftSide.Should().Be(rightSide);
            }
            else
            {
                leftSide.Should().NotBe(rightSide);
            }
        }

        [Theory]
        [InlineData("a", "a", true)]
        [InlineData("A;b", "a;B", true)]
        [InlineData("a;b;c", "c;a;B", true)]
        [InlineData("a;b;c;d", "c;a;B", false)]
        public void Equals_WithNativeLibraries(string left, string right, bool expected)
        {
            var leftSide = new LockFileTargetLibrary()
            {
                NativeLibraries = left.Split(';').Select(e => new LockFileItem(e)).ToList()
            };

            var rightSide = new LockFileTargetLibrary()
            {
                NativeLibraries = right.Split(';').Select(e => new LockFileItem(e)).ToList()
            };

            // Act & Assert
            if (expected)
            {
                leftSide.Should().Be(rightSide);
            }
            else
            {
                leftSide.Should().NotBe(rightSide);
            }
        }

        [Theory]
        [InlineData("a", "a", true)]
        [InlineData("A;b", "a;B", true)]
        [InlineData("a;b;c", "c;a;B", true)]
        [InlineData("a;b;c;d", "c;a;B", false)]
        public void Equals_WithBuild(string left, string right, bool expected)
        {
            var leftSide = new LockFileTargetLibrary()
            {
                Build = left.Split(';').Select(e => new LockFileItem(e)).ToList()
            };

            var rightSide = new LockFileTargetLibrary()
            {
                Build = right.Split(';').Select(e => new LockFileItem(e)).ToList()
            };

            // Act & Assert
            if (expected)
            {
                leftSide.Should().Be(rightSide);
            }
            else
            {
                leftSide.Should().NotBe(rightSide);
            }
        }

        [Theory]
        [InlineData("a", "a", true)]
        [InlineData("A;b", "a;B", true)]
        [InlineData("a;b;c", "c;a;B", true)]
        [InlineData("a;b;c;d", "c;a;B", false)]
        public void Equals_WithBuildMultiTargeting(string left, string right, bool expected)
        {
            var leftSide = new LockFileTargetLibrary()
            {
                BuildMultiTargeting = left.Split(';').Select(e => new LockFileItem(e)).ToList()
            };

            var rightSide = new LockFileTargetLibrary()
            {
                BuildMultiTargeting = right.Split(';').Select(e => new LockFileItem(e)).ToList()
            };

            // Act & Assert
            if (expected)
            {
                leftSide.Should().Be(rightSide);
            }
            else
            {
                leftSide.Should().NotBe(rightSide);
            }
        }

        [Theory]
        [InlineData("a", "a", true)]
        [InlineData("A;b", "a;B", true)]
        [InlineData("a;b;c", "c;a;B", true)]
        [InlineData("a;b;c;d", "c;a;B", false)]
        public void Equals_WithToolsAssemblies(string left, string right, bool expected)
        {
            var leftSide = new LockFileTargetLibrary()
            {
                ToolsAssemblies = left.Split(';').Select(e => new LockFileItem(e)).ToList()
            };

            var rightSide = new LockFileTargetLibrary()
            {
                ToolsAssemblies = right.Split(';').Select(e => new LockFileItem(e)).ToList()
            };

            // Act & Assert
            if (expected)
            {
                leftSide.Should().Be(rightSide);
            }
            else
            {
                leftSide.Should().NotBe(rightSide);
            }
        }

        [Theory]
        [InlineData("a", "a", true)]
        [InlineData("A;b", "a;B", true)]
        [InlineData("a;b;c", "c;a;B", true)]
        [InlineData("a;b;c;d", "c;a;B", false)]
        public void Equals_WithEmbedAssemblies(string left, string right, bool expected)
        {
            var leftSide = new LockFileTargetLibrary()
            {
                EmbedAssemblies = left.Split(';').Select(e => new LockFileItem(e)).ToList()
            };

            var rightSide = new LockFileTargetLibrary()
            {
                EmbedAssemblies = right.Split(';').Select(e => new LockFileItem(e)).ToList()
            };

            // Act & Assert
            if (expected)
            {
                leftSide.Should().Be(rightSide);
            }
            else
            {
                leftSide.Should().NotBe(rightSide);
            }
        }

        [Theory]
        [InlineData("a", "a", true)]
        [InlineData("A;b", "a;B", true)]
        [InlineData("a;b;c", "c;a;B", true)]
        [InlineData("a;b;c;d", "c;a;B", false)]
        public void Equals_WithContentFiles(string left, string right, bool expected)
        {
            var leftSide = new LockFileTargetLibrary()
            {
                ContentFiles = left.Split(';').Select(e => new LockFileContentFile(e)).ToList()
            };

            var rightSide = new LockFileTargetLibrary()
            {
                ContentFiles = right.Split(';').Select(e => new LockFileContentFile(e)).ToList()
            };

            // Act & Assert
            if (expected)
            {
                leftSide.Should().Be(rightSide);
            }
            else
            {
                leftSide.Should().NotBe(rightSide);
            }
        }

        [Theory]
        [InlineData("a", "a", true)]
        [InlineData("A;b", "a;B", true)]
        [InlineData("a;b;c", "c;a;B", true)]
        [InlineData("a;b;c;d", "c;a;B", false)]
        public void Equals_WithRuntimeTargets(string left, string right, bool expected)
        {
            var leftSide = new LockFileTargetLibrary()
            {
                RuntimeTargets = left.Split(';').Select(e => new LockFileRuntimeTarget(e)).ToList()
            };

            var rightSide = new LockFileTargetLibrary()
            {
                RuntimeTargets = right.Split(';').Select(e => new LockFileRuntimeTarget(e)).ToList()
            };

            // Act & Assert
            if (expected)
            {
                leftSide.Should().Be(rightSide);
            }
            else
            {
                leftSide.Should().NotBe(rightSide);
            }
        }

        [Fact]
        public void Equals_WithPackageType_IsIgnored()
        {
            var leftSide = new LockFileTargetLibrary()
            {
                PackageType = new List<PackageType>() { PackageType.Dependency }
            };

            var rightSide = new LockFileTargetLibrary()
            {
                PackageType = new List<PackageType>() { PackageType.DotnetCliTool }
            };

            leftSide.Should().Be(rightSide);
        }

        [Theory]
        [InlineData("project", "project", true)]
        [InlineData("project", "PROJECT", false)]
        [InlineData("project", "package", false)]
        public void HashCode_WithName(string left, string right, bool expected)
        {
            var leftSide = new LockFileTargetLibrary()
            {
                Name = left
            };

            var rightSide = new LockFileTargetLibrary()
            {
                Name = right
            };

            // Act & Assert
            if (expected)
            {
                leftSide.GetHashCode().Should().Be(rightSide.GetHashCode());
            }
            else
            {
                leftSide.GetHashCode().Should().NotBe(rightSide.GetHashCode());
            }
        }

        [Theory]
        [InlineData("2.1.0", "2.1.0", true)]
        [InlineData("2.1.0", "2.1.0-preview.2", false)]
        [InlineData("1.1.1", "2.2.2", false)]
        public void HashCode_WithVersion(string left, string right, bool expected)
        {
            var leftSide = new LockFileTargetLibrary()
            {
                Version = NuGetVersion.Parse(left)
            };

            var rightSide = new LockFileTargetLibrary()
            {
                Version = NuGetVersion.Parse(right)
            };

            // Act & Assert
            if (expected)
            {
                leftSide.GetHashCode().Should().Be(rightSide.GetHashCode());
            }
            else
            {
                leftSide.GetHashCode().Should().NotBe(rightSide.GetHashCode());
            }
        }

        [Theory]
        [InlineData("net461", "net461", true)]
        [InlineData("net461", "NET461", false)]
        [InlineData("net46", "netcoreapp10", false)]
        public void HashCode_WithFramework(string left, string right, bool expected)
        {
            var leftSide = new LockFileTargetLibrary()
            {
                Framework = left
            };

            var rightSide = new LockFileTargetLibrary()
            {
                Framework = right
            };

            // Act & Assert
            if (expected)
            {
                leftSide.GetHashCode().Should().Be(rightSide.GetHashCode());
            }
            else
            {
                leftSide.GetHashCode().Should().NotBe(rightSide.GetHashCode());
            }
        }

        [Theory]
        [InlineData("project", "project", true)]
        [InlineData("project", "PROJECT", false)]
        [InlineData("project", "package", false)]
        public void HashCode_WithType(string left, string right, bool expected)
        {
            var leftSide = new LockFileTargetLibrary()
            {
                Type = left
            };

            var rightSide = new LockFileTargetLibrary()
            {
                Type = right
            };

            // Act & Assert
            if (expected)
            {
                leftSide.GetHashCode().Should().Be(rightSide.GetHashCode());
            }
            else
            {
                leftSide.GetHashCode().Should().NotBe(rightSide.GetHashCode());
            }
        }

        [Theory]
        [InlineData("a", "a", true)]
        [InlineData("A;b", "a;B", true)]
        [InlineData("a;b;c", "c;a;B", true)]
        [InlineData("a;b;c;d", "c;a;B", false)]
        public void HashCode_WithPackageDependency(string left, string right, bool expected)
        {
            var leftSide = new LockFileTargetLibrary()
            {
                Dependencies = left.Split(';').Select(e => new PackageDependency(e, VersionRange.Parse("1.0.0"))).ToList()
            };

            var rightSide = new LockFileTargetLibrary()
            {
                Dependencies = right.Split(';').Select(e => new PackageDependency(e, VersionRange.Parse("1.0.0"))).ToList()
            };

            // Act & Assert
            if (expected)
            {
                leftSide.GetHashCode().Should().Be(rightSide.GetHashCode());
            }
            else
            {
                leftSide.GetHashCode().Should().NotBe(rightSide.GetHashCode());
            }
        }

        [Theory]
        [InlineData("a", "a", true)]
        [InlineData("A;b", "a;B", true)]
        [InlineData("a;b;c", "c;a;B", true)]
        [InlineData("a;b;c;d", "c;a;B", false)]
        public void HashCode_WithFrameworkAssemblies(string left, string right, bool expected)
        {
            var leftSide = new LockFileTargetLibrary()
            {
                FrameworkAssemblies = left.Split(';').Select(e => e).ToList()
            };

            var rightSide = new LockFileTargetLibrary()
            {
                FrameworkAssemblies = right.Split(';').Select(e => e).ToList()
            };

            // Act & Assert
            if (expected)
            {
                leftSide.GetHashCode().Should().Be(rightSide.GetHashCode());
            }
            else
            {
                leftSide.GetHashCode().Should().NotBe(rightSide.GetHashCode());
            }
        }

        [Theory]
        [InlineData("a", "a", true)]
        [InlineData("A;b", "a;B", true)]
        [InlineData("a;b;c", "c;a;B", true)]
        [InlineData("a;b;c;d", "c;a;B", false)]
        public void HashCode_WithFrameworkReferences(string left, string right, bool expected)
        {
            var leftSide = new LockFileTargetLibrary()
            {
                FrameworkReferences = left.Split(';').Select(e => e).ToList()
            };

            var rightSide = new LockFileTargetLibrary()
            {
                FrameworkReferences = right.Split(';').Select(e => e).ToList()
            };

            // Act & Assert
            if (expected)
            {
                leftSide.GetHashCode().Should().Be(rightSide.GetHashCode());
            }
            else
            {
                leftSide.GetHashCode().Should().NotBe(rightSide.GetHashCode());
            }
        }

        [Theory]
        [InlineData("a", "a", true)]
        [InlineData("A;b", "a;B", true)]
        [InlineData("a;b;c", "c;a;B", true)]
        [InlineData("a;b;c;d", "c;a;B", false)]
        public void HashCode_WithRuntimeAssemblies(string left, string right, bool expected)
        {
            var leftSide = new LockFileTargetLibrary()
            {
                RuntimeAssemblies = left.Split(';').Select(e => new LockFileItem(e)).ToList()
            };

            var rightSide = new LockFileTargetLibrary()
            {
                RuntimeAssemblies = right.Split(';').Select(e => new LockFileItem(e)).ToList()
            };

            // Act & Assert
            if (expected)
            {
                leftSide.GetHashCode().Should().Be(rightSide.GetHashCode());
            }
            else
            {
                leftSide.GetHashCode().Should().NotBe(rightSide.GetHashCode());
            }
        }

        [Theory]
        [InlineData("a", "a", true)]
        [InlineData("A;b", "a;B", true)]
        [InlineData("a;b;c", "c;a;B", true)]
        [InlineData("a;b;c;d", "c;a;B", false)]
        public void HashCode_WithResourceAssemblies(string left, string right, bool expected)
        {
            var leftSide = new LockFileTargetLibrary()
            {
                ResourceAssemblies = left.Split(';').Select(e => new LockFileItem(e)).ToList()
            };

            var rightSide = new LockFileTargetLibrary()
            {
                ResourceAssemblies = right.Split(';').Select(e => new LockFileItem(e)).ToList()
            };

            // Act & Assert
            if (expected)
            {
                leftSide.GetHashCode().Should().Be(rightSide.GetHashCode());
            }
            else
            {
                leftSide.GetHashCode().Should().NotBe(rightSide.GetHashCode());
            }
        }

        [Theory]
        [InlineData("a", "a", true)]
        [InlineData("A;b", "a;B", true)]
        [InlineData("a;b;c", "c;a;B", true)]
        [InlineData("a;b;c;d", "c;a;B", false)]
        public void HashCode_WithCompileTimeAssemblies(string left, string right, bool expected)
        {
            var leftSide = new LockFileTargetLibrary()
            {
                CompileTimeAssemblies = left.Split(';').Select(e => new LockFileItem(e)).ToList()
            };

            var rightSide = new LockFileTargetLibrary()
            {
                CompileTimeAssemblies = right.Split(';').Select(e => new LockFileItem(e)).ToList()
            };

            // Act & Assert
            if (expected)
            {
                leftSide.GetHashCode().Should().Be(rightSide.GetHashCode());
            }
            else
            {
                leftSide.GetHashCode().Should().NotBe(rightSide.GetHashCode());
            }
        }

        [Theory]
        [InlineData("a", "a", true)]
        [InlineData("A;b", "a;B", true)]
        [InlineData("a;b;c", "c;a;B", true)]
        [InlineData("a;b;c;d", "c;a;B", false)]
        public void HashCode_WithNativeLibraries(string left, string right, bool expected)
        {
            var leftSide = new LockFileTargetLibrary()
            {
                NativeLibraries = left.Split(';').Select(e => new LockFileItem(e)).ToList()
            };

            var rightSide = new LockFileTargetLibrary()
            {
                NativeLibraries = right.Split(';').Select(e => new LockFileItem(e)).ToList()
            };

            // Act & Assert
            if (expected)
            {
                leftSide.GetHashCode().Should().Be(rightSide.GetHashCode());
            }
            else
            {
                leftSide.GetHashCode().Should().NotBe(rightSide.GetHashCode());
            }
        }

        [Theory]
        [InlineData("a", "a", true)]
        [InlineData("A;b", "a;B", true)]
        [InlineData("a;b;c", "c;a;B", true)]
        [InlineData("a;b;c;d", "c;a;B", false)]
        public void HashCode_WithBuild(string left, string right, bool expected)
        {
            var leftSide = new LockFileTargetLibrary()
            {
                Build = left.Split(';').Select(e => new LockFileItem(e)).ToList()
            };

            var rightSide = new LockFileTargetLibrary()
            {
                Build = right.Split(';').Select(e => new LockFileItem(e)).ToList()
            };

            // Act & Assert
            if (expected)
            {
                leftSide.GetHashCode().Should().Be(rightSide.GetHashCode());
            }
            else
            {
                leftSide.GetHashCode().Should().NotBe(rightSide.GetHashCode());
            }
        }

        [Theory]
        [InlineData("a", "a", true)]
        [InlineData("A;b", "a;B", true)]
        [InlineData("a;b;c", "c;a;B", true)]
        [InlineData("a;b;c;d", "c;a;B", false)]
        public void HashCode_WithBuildMultiTargeting(string left, string right, bool expected)
        {
            var leftSide = new LockFileTargetLibrary()
            {
                BuildMultiTargeting = left.Split(';').Select(e => new LockFileItem(e)).ToList()
            };

            var rightSide = new LockFileTargetLibrary()
            {
                BuildMultiTargeting = right.Split(';').Select(e => new LockFileItem(e)).ToList()
            };

            // Act & Assert
            if (expected)
            {
                leftSide.GetHashCode().Should().Be(rightSide.GetHashCode());
            }
            else
            {
                leftSide.GetHashCode().Should().NotBe(rightSide.GetHashCode());
            }
        }

        [Theory]
        [InlineData("a", "a", true)]
        [InlineData("A;b", "a;B", true)]
        [InlineData("a;b;c", "c;a;B", true)]
        [InlineData("a;b;c;d", "c;a;B", false)]
        public void HashCode_WithToolsAssemblies(string left, string right, bool expected)
        {
            var leftSide = new LockFileTargetLibrary()
            {
                ToolsAssemblies = left.Split(';').Select(e => new LockFileItem(e)).ToList()
            };

            var rightSide = new LockFileTargetLibrary()
            {
                ToolsAssemblies = right.Split(';').Select(e => new LockFileItem(e)).ToList()
            };

            // Act & Assert
            if (expected)
            {
                leftSide.GetHashCode().Should().Be(rightSide.GetHashCode());
            }
            else
            {
                leftSide.GetHashCode().Should().NotBe(rightSide.GetHashCode());
            }
        }

        [Theory]
        [InlineData("a", "a", true)]
        [InlineData("A;b", "a;B", true)]
        [InlineData("a;b;c", "c;a;B", true)]
        [InlineData("a;b;c;d", "c;a;B", false)]
        public void HashCode_WithEmbedAssemblies(string left, string right, bool expected)
        {
            var leftSide = new LockFileTargetLibrary()
            {
                EmbedAssemblies = left.Split(';').Select(e => new LockFileItem(e)).ToList()
            };

            var rightSide = new LockFileTargetLibrary()
            {
                EmbedAssemblies = right.Split(';').Select(e => new LockFileItem(e)).ToList()
            };

            // Act & Assert
            if (expected)
            {
                leftSide.GetHashCode().Should().Be(rightSide.GetHashCode());
            }
            else
            {
                leftSide.GetHashCode().Should().NotBe(rightSide.GetHashCode());
            }
        }

        [Theory]
        [InlineData("a", "a", true)]
        [InlineData("A;b", "a;B", true)]
        [InlineData("a;b;c", "c;a;B", true)]
        [InlineData("a;b;c;d", "c;a;B", false)]
        public void HashCode_WithContentFiles(string left, string right, bool expected)
        {
            var leftSide = new LockFileTargetLibrary()
            {
                ContentFiles = left.Split(';').Select(e => new LockFileContentFile(e)).ToList()
            };

            var rightSide = new LockFileTargetLibrary()
            {
                ContentFiles = right.Split(';').Select(e => new LockFileContentFile(e)).ToList()
            };

            // Act & Assert
            if (expected)
            {
                leftSide.GetHashCode().Should().Be(rightSide.GetHashCode());
            }
            else
            {
                leftSide.GetHashCode().Should().NotBe(rightSide.GetHashCode());
            }
        }

        [Theory]
        [InlineData("a", "a", true)]
        [InlineData("A;b", "a;B", true)]
        [InlineData("a;b;c", "c;a;B", true)]
        [InlineData("a;b;c;d", "c;a;B", false)]
        public void HashCode_WithRuntimeTargets(string left, string right, bool expected)
        {
            var leftSide = new LockFileTargetLibrary()
            {
                RuntimeTargets = left.Split(';').Select(e => new LockFileRuntimeTarget(e)).ToList()
            };

            var rightSide = new LockFileTargetLibrary()
            {
                RuntimeTargets = right.Split(';').Select(e => new LockFileRuntimeTarget(e)).ToList()
            };

            // Act & Assert
            if (expected)
            {
                leftSide.GetHashCode().Should().Be(rightSide.GetHashCode());
            }
            else
            {
                leftSide.GetHashCode().Should().NotBe(rightSide.GetHashCode());
            }
        }

        [Fact]
        public void HashCode_WithPackageType_IsIgnored()
        {
            var leftSide = new LockFileTargetLibrary()
            {
                PackageType = new List<PackageType>() { PackageType.Dependency }
            };

            var rightSide = new LockFileTargetLibrary()
            {
                PackageType = new List<PackageType>() { PackageType.DotnetCliTool }
            };

            leftSide.GetHashCode().Should().Be(rightSide.GetHashCode());
        }

        [Theory]
        [InlineData("a", "a", true)]
        [InlineData("a .pdb;.xml", "a .pdb;.xml", true)]
        [InlineData("a .pdb;.xml", "a;", false)]
        [InlineData("a .pdb", "b .pdb", false)]
        [InlineData("a .pdb;.xml", "a .xml;.pdb", false)]
        public void Equals_WithRuntimeAssembliesAndRelatedFiles(string left, string right, bool expected)
        {
            string[] leftParts = left.Trim().Split(' ');
            var leftRuntimeAssembly = new LockFileItem(leftParts[0]);
            if (leftParts.Length > 1)
            {
                leftRuntimeAssembly.Properties.Add("related", leftParts[1]);
            }
            var leftSide = new LockFileTargetLibrary()
            {
                RuntimeAssemblies = new List<LockFileItem>() { leftRuntimeAssembly }
            };

            string[] rightParts = right.Split(' ');
            var rightRuntimeAssembly = new LockFileItem(rightParts[0]);
            if (rightParts.Length > 1)
            {
                rightRuntimeAssembly.Properties.Add("related", rightParts[1]);
            }
            var rightSide = new LockFileTargetLibrary()
            {
                RuntimeAssemblies = new List<LockFileItem>() { rightRuntimeAssembly }
            };

            // Act & Assert
            if (expected)
            {
                leftSide.Should().Be(rightSide);
            }
            else
            {
                leftSide.Should().NotBe(rightSide);
            }
        }
    }
}
