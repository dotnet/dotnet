// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Linq;
using FluentAssertions;
using NuGet.Frameworks;
using Xunit;


namespace NuGet.ProjectModel.Test
{
    public class ProjectRestoreMetadataFrameworkInfoTests
    {
        [Theory]
        [InlineData("net461", "net461", true)]
        [InlineData("net461", "NET461", true)]
        [InlineData("net461", "net462", false)]
        [InlineData("", "", true)]
        public void Equals_WithTargetAlias(string left, string right, bool expected)
        {
            var leftSide = new ProjectRestoreMetadataFrameworkInfo()
            {
                TargetAlias = left
            };

            var rightSide = new ProjectRestoreMetadataFrameworkInfo()
            {
                TargetAlias = right
            };

            AssertEquality(expected, leftSide, rightSide);
        }

        [Theory]
        [InlineData("net461", "net461", true)]
        [InlineData("net461", "net462", false)]
        [InlineData("net463", "net462", false)]
        public void Equals_WithNuGetFramework(string left, string right, bool expected)
        {
            var leftSide = new ProjectRestoreMetadataFrameworkInfo()
            {
                FrameworkName = NuGetFramework.Parse(left)
            };

            var rightSide = new ProjectRestoreMetadataFrameworkInfo()
            {
                FrameworkName = NuGetFramework.Parse(right)
            };

            AssertEquality(expected, leftSide, rightSide);
        }

        [Theory]
        [InlineData("path1;path2", "path1;path2", true)]
        [InlineData("path2", "path1", false)]
        [InlineData("path1;path3;path2", "path3;path1;path2", true)]
        [InlineData("path1;path3;path2;path4", "path3;path1;path2", false)]
        public void Equals_WithProjectReferences(string left, string right, bool expected)
        {
            var leftSide = new ProjectRestoreMetadataFrameworkInfo()
            {
                FrameworkName = NuGetFramework.AnyFramework,
                ProjectReferences = left.Split(';').Select(e => new ProjectRestoreReference() { ProjectPath = e }).ToList()
            };

            var rightSide = new ProjectRestoreMetadataFrameworkInfo()
            {
                FrameworkName = NuGetFramework.AnyFramework,
                ProjectReferences = right.Split(';').Select(e => new ProjectRestoreReference() { ProjectPath = e }).ToList()
            };

            AssertEquality(expected, leftSide, rightSide);
        }

        [Theory]
        [InlineData("net461", "net461", true)]
        [InlineData("net461", "NET461", true)]
        [InlineData("net461", "net462", false)]
        [InlineData("", "", true)]
        public void HashCode_WithTargetAlias(string left, string right, bool expected)
        {
            var leftSide = new ProjectRestoreMetadataFrameworkInfo()
            {
                TargetAlias = left
            };

            var rightSide = new ProjectRestoreMetadataFrameworkInfo()
            {
                TargetAlias = right
            };

            AssertEquality(expected, leftSide, rightSide);
        }

        [Theory]
        [InlineData("net461", "net461", true)]
        [InlineData("net461", "net462", false)]
        [InlineData("net463", "net462", false)]
        public void HashCode_WithNuGetFramework(string left, string right, bool expected)
        {
            var leftSide = new ProjectRestoreMetadataFrameworkInfo()
            {
                FrameworkName = NuGetFramework.Parse(left)
            };

            var rightSide = new ProjectRestoreMetadataFrameworkInfo()
            {
                FrameworkName = NuGetFramework.Parse(right)
            };

            AssertEquality(expected, leftSide, rightSide);
        }

        [Theory]
        [InlineData("path1;path2", "path1;path2", true)]
        [InlineData("path2", "path1", false)]
        [InlineData("path1;path3;path2", "path3;path1;path2", true)]
        [InlineData("path1;path3;path2;path4", "path3;path1;path2", false)]
        public void HashCode_WithProjectReferences(string left, string right, bool expected)
        {
            var leftSide = new ProjectRestoreMetadataFrameworkInfo()
            {
                FrameworkName = NuGetFramework.AnyFramework,
                ProjectReferences = left.Split(';').Select(e => new ProjectRestoreReference() { ProjectPath = e }).ToList()
            };

            var rightSide = new ProjectRestoreMetadataFrameworkInfo()
            {
                FrameworkName = NuGetFramework.AnyFramework,
                ProjectReferences = right.Split(';').Select(e => new ProjectRestoreReference() { ProjectPath = e }).ToList()
            };

            AssertEquality(expected, leftSide, rightSide);
        }

        private static void AssertEquality(bool expected, ProjectRestoreMetadataFrameworkInfo leftSide, ProjectRestoreMetadataFrameworkInfo rightSide)
        {
            if (expected)
            {
                leftSide.Should().Be(rightSide);
            }
            else
            {
                leftSide.Should().NotBe(rightSide);
            }
        }

        private static void AssertHashCode(bool expected, ProjectRestoreMetadataFrameworkInfo leftSide, ProjectRestoreMetadataFrameworkInfo rightSide)
        {
            if (expected)
            {
                leftSide.GetHashCode().Should().Be(rightSide.GetHashCode());
            }
            else
            {
                leftSide.GetHashCode().Should().NotBe(rightSide.GetHashCode());
            }
        }
    }
}
