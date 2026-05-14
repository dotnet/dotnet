// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NuGet.Frameworks;
using Xunit;

namespace NuGet.ProjectModel.Test
{
    public class LockFileTargetTests
    {
        [Theory]
        [InlineData("net461", "net461", true)]
        [InlineData("net461", "net462", false)]
        public void Equals_WithTargetFramework(string left, string right, bool expected)
        {
            var leftSide = new LockFileTarget()
            {
                TargetFramework = NuGetFramework.Parse(left)
            };

            var rightSide = new LockFileTarget()
            {
                TargetFramework = NuGetFramework.Parse(right)
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
        [InlineData("win7", "win7", true)]
        [InlineData("win10", "win7", false)]
        public void Equals_WithRuntimeIdentifier(string left, string right, bool expected)
        {
            var leftSide = new LockFileTarget()
            {
                RuntimeIdentifier = left
            };

            var rightSide = new LockFileTarget()
            {
                RuntimeIdentifier = right
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
        [InlineData("net6.0", "net6.0", true)]
        [InlineData("net6.0", "net7.0", false)]
        [InlineData("", "", true)]
        [InlineData(null, null, true)]
        [InlineData("net6.0", null, false)]
        [InlineData("net6.0", "", false)]
        [InlineData("MyAlias", "MyAlias", true)]
        [InlineData("MyAlias", "MYALIAS", false)]
        public void Equals_WithTargetAlias(string left, string right, bool expected)
        {
            var leftSide = new LockFileTarget()
            {
                TargetAlias = left
            };

            var rightSide = new LockFileTarget()
            {
                TargetAlias = right
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
        [InlineData("net6.0", "net6.0", true)]
        [InlineData("net6.0", "net7.0", false)]
        [InlineData("", "", true)]
        [InlineData(null, null, true)]
        [InlineData("net6.0", null, false)]
        [InlineData("net6.0", "", false)]
        [InlineData("MyAlias", "MyAlias", true)]
        [InlineData("MyAlias", "MYALIAS", false)]
        public void GetHashCode_WithTargetAlias(string left, string right, bool expected)
        {
            var leftSide = new LockFileTarget()
            {
                TargetAlias = left
            };

            var rightSide = new LockFileTarget()
            {
                TargetAlias = right
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
        public void TargetAlias_SetAndGet_ShouldReturnCorrectValue()
        {
            var target = new LockFileTarget();
            string expectedAlias = "my-custom-alias";

            // Act
            target.TargetAlias = expectedAlias;

            // Assert
            target.TargetAlias.Should().Be(expectedAlias);
        }

        [Theory]
        [InlineData("project", "project", true)]
        [InlineData("project", "package", false)]
        [InlineData("project;project2", "project2;project", true)]
        [InlineData("project;project2", "project;project2;project3", false)]
        public void Equals_WithLockFileTargetLibraries(string left, string right, bool expected)
        {
            var leftSide = new LockFileTarget()
            {
                Libraries = left.Split(';').Select(e => new LockFileTargetLibrary() { Name = e }).ToList()
            };

            var rightSide = new LockFileTarget()
            {
                Libraries = right.Split(';').Select(e => new LockFileTargetLibrary() { Name = e }).ToList()
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
        public void Equals_WithAllPropertiesSet_IncludingTargetAlias_ShouldBeEqual()
        {
            var leftSide = new LockFileTarget()
            {
                TargetFramework = NuGetFramework.Parse("net6.0"),
                RuntimeIdentifier = "win-x64",
                TargetAlias = "net6.0-windows",
                Libraries = new List<LockFileTargetLibrary>
                {
                    new LockFileTargetLibrary() { Name = "TestPackage" }
                }
            };

            var rightSide = new LockFileTarget()
            {
                TargetFramework = NuGetFramework.Parse("net6.0"),
                RuntimeIdentifier = "win-x64",
                TargetAlias = "net6.0-windows",
                Libraries = new List<LockFileTargetLibrary>
                {
                    new LockFileTargetLibrary() { Name = "TestPackage" }
                }
            };

            // Act & Assert
            leftSide.Should().Be(rightSide);
        }

        [Fact]
        public void Equals_WithOnlyTargetAliasDifferent_ShouldNotBeEqual()
        {
            var leftSide = new LockFileTarget()
            {
                TargetFramework = NuGetFramework.Parse("net6.0"),
                RuntimeIdentifier = "win-x64",
                TargetAlias = "net6.0-windows",
                Libraries = new List<LockFileTargetLibrary>
                {
                    new LockFileTargetLibrary() { Name = "TestPackage" }
                }
            };

            var rightSide = new LockFileTarget()
            {
                TargetFramework = NuGetFramework.Parse("net6.0"),
                RuntimeIdentifier = "win-x64",
                TargetAlias = "net6.0-android", // Only this is different
                Libraries = new List<LockFileTargetLibrary>
                {
                    new LockFileTargetLibrary() { Name = "TestPackage" }
                }
            };

            // Act & Assert
            leftSide.Should().NotBe(rightSide);
        }
    }
}
