// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using FluentAssertions;
using NuGet.LibraryModel;
using Xunit;

namespace NuGet.ProjectModel.Test
{
    public class ProjectRestoreReferenceTests
    {
        [Theory]
        [InlineData("path1", "path1", true)]
        [InlineData("path1", "path2", false)]
        [InlineData("", "", true)]
        public void Equals_WithProjectPath(string left, string right, bool expected)
        {
            var leftSide = new ProjectRestoreReference()
            {
                ProjectPath = left
            };

            var rightSide = new ProjectRestoreReference()
            {
                ProjectPath = right
            };

            AssertEquality(expected, leftSide, rightSide);
        }

        [Theory]
        [InlineData("path1", "path1", true)]
        [InlineData("path1", "path2", false)]
        [InlineData("", "", true)]
        public void Equals_WithProjectUniqueName(string left, string right, bool expected)
        {
            var leftSide = new ProjectRestoreReference()
            {
                ProjectUniqueName = left
            };

            var rightSide = new ProjectRestoreReference()
            {
                ProjectUniqueName = right
            };

            AssertEquality(expected, leftSide, rightSide);
        }

        [Theory]
        [InlineData(LibraryIncludeFlags.Analyzers, LibraryIncludeFlags.Analyzers, true)]
        [InlineData(LibraryIncludeFlags.Analyzers, LibraryIncludeFlags.Compile, false)]
        [InlineData(LibraryIncludeFlags.Native, LibraryIncludeFlags.Compile, false)]
        public void Equals_WithIncludeAssets(LibraryIncludeFlags left, LibraryIncludeFlags right, bool expected)
        {
            var leftSide = new ProjectRestoreReference()
            {
                ProjectPath = "path",
                ProjectUniqueName = "path",
                IncludeAssets = left
            };

            var rightSide = new ProjectRestoreReference()
            {
                ProjectPath = "path",
                ProjectUniqueName = "path",
                IncludeAssets = right
            };

            AssertEquality(expected, leftSide, rightSide);
        }

        [Theory]
        [InlineData(LibraryIncludeFlags.Analyzers, LibraryIncludeFlags.Analyzers, true)]
        [InlineData(LibraryIncludeFlags.Analyzers, LibraryIncludeFlags.Compile, false)]
        [InlineData(LibraryIncludeFlags.Native, LibraryIncludeFlags.Compile, false)]
        public void Equals_WithPrivateAssets(LibraryIncludeFlags left, LibraryIncludeFlags right, bool expected)
        {
            var leftSide = new ProjectRestoreReference()
            {
                ProjectPath = "path",
                ProjectUniqueName = "path",
                PrivateAssets = left
            };

            var rightSide = new ProjectRestoreReference()
            {
                ProjectPath = "path",
                ProjectUniqueName = "path",
                PrivateAssets = right
            };

            AssertEquality(expected, leftSide, rightSide);
        }

        [Theory]
        [InlineData(LibraryIncludeFlags.Analyzers, LibraryIncludeFlags.Analyzers, true)]
        [InlineData(LibraryIncludeFlags.Analyzers, LibraryIncludeFlags.Compile, false)]
        [InlineData(LibraryIncludeFlags.Native, LibraryIncludeFlags.Compile, false)]
        public void Equals_WithExcludeAssets(LibraryIncludeFlags left, LibraryIncludeFlags right, bool expected)
        {
            var leftSide = new ProjectRestoreReference()
            {
                ProjectPath = "path",
                ProjectUniqueName = "path",
                ExcludeAssets = left
            };

            var rightSide = new ProjectRestoreReference()
            {
                ProjectPath = "path",
                ProjectUniqueName = "path",
                ExcludeAssets = right
            };

            AssertEquality(expected, leftSide, rightSide);
        }

        [Theory]
        [InlineData("path1", "path1", true)]
        [InlineData("path1", "path2", false)]
        [InlineData("", "", true)]
        public void HashCode_WithProjectPath(string left, string right, bool expected)
        {
            var leftSide = new ProjectRestoreReference()
            {
                ProjectUniqueName = "path",
                ProjectPath = left
            };

            var rightSide = new ProjectRestoreReference()
            {
                ProjectUniqueName = "path",
                ProjectPath = right
            };

            AssertHashCode(expected, leftSide, rightSide);
        }

        [Theory]
        [InlineData("path1", "path1", true)]
        [InlineData("path1", "path2", false)]
        [InlineData("", "", true)]
        public void HashCode_WithProjectUniqueName(string left, string right, bool expected)
        {
            var leftSide = new ProjectRestoreReference()
            {
                ProjectPath = "path",
                ProjectUniqueName = left
            };

            var rightSide = new ProjectRestoreReference()
            {
                ProjectPath = "path",
                ProjectUniqueName = right
            };

            AssertHashCode(expected, leftSide, rightSide);
        }

        [Theory]
        [InlineData(LibraryIncludeFlags.Analyzers, LibraryIncludeFlags.Analyzers, true)]
        [InlineData(LibraryIncludeFlags.Analyzers, LibraryIncludeFlags.Compile, false)]
        [InlineData(LibraryIncludeFlags.Native, LibraryIncludeFlags.Compile, false)]
        public void HashCode_WithIncludeAssets(LibraryIncludeFlags left, LibraryIncludeFlags right, bool expected)
        {
            var leftSide = new ProjectRestoreReference()
            {
                ProjectPath = "path",
                ProjectUniqueName = "path",
                IncludeAssets = left
            };

            var rightSide = new ProjectRestoreReference()
            {
                ProjectPath = "path",
                ProjectUniqueName = "path",
                IncludeAssets = right
            };

            AssertHashCode(expected, leftSide, rightSide);
        }

        [Theory]
        [InlineData(LibraryIncludeFlags.Analyzers, LibraryIncludeFlags.Analyzers, true)]
        [InlineData(LibraryIncludeFlags.Analyzers, LibraryIncludeFlags.Compile, false)]
        [InlineData(LibraryIncludeFlags.Native, LibraryIncludeFlags.Compile, false)]
        public void HashCode_WithPrivateAssets(LibraryIncludeFlags left, LibraryIncludeFlags right, bool expected)
        {
            var leftSide = new ProjectRestoreReference()
            {
                ProjectPath = "path",
                ProjectUniqueName = "path",
                PrivateAssets = left
            };

            var rightSide = new ProjectRestoreReference()
            {
                ProjectPath = "path",
                ProjectUniqueName = "path",
                PrivateAssets = right
            };

            AssertHashCode(expected, leftSide, rightSide);
        }

        [Theory]
        [InlineData(LibraryIncludeFlags.Analyzers, LibraryIncludeFlags.Analyzers, true)]
        [InlineData(LibraryIncludeFlags.Analyzers, LibraryIncludeFlags.Compile, false)]
        [InlineData(LibraryIncludeFlags.Native, LibraryIncludeFlags.Compile, false)]
        public void HashCode_WithExcludeAssets(LibraryIncludeFlags left, LibraryIncludeFlags right, bool expected)
        {
            var leftSide = new ProjectRestoreReference()
            {
                ProjectPath = "path",
                ProjectUniqueName = "path",
                ExcludeAssets = left
            };

            var rightSide = new ProjectRestoreReference()
            {
                ProjectPath = "path",
                ProjectUniqueName = "path",
                ExcludeAssets = right
            };

            AssertHashCode(expected, leftSide, rightSide);
        }

        private static void AssertEquality(bool expected, ProjectRestoreReference leftSide, ProjectRestoreReference rightSide)
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

        private static void AssertHashCode(bool expected, ProjectRestoreReference leftSide, ProjectRestoreReference rightSide)
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
