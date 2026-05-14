// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Linq;
using FluentAssertions;
using Xunit;

namespace NuGet.ProjectModel.Test
{
    public class ProjectFileDependencyGroupTests
    {
        [Fact]
        public void Equals_WithDifferentFrameworkName_ReturnsFalse()
        {
            var leftSide = new ProjectFileDependencyGroup("frameworkName", Enumerable.Empty<string>());
            var rightSide = new ProjectFileDependencyGroup("differetName", Enumerable.Empty<string>());
            leftSide.Should().NotBe(rightSide);
        }

        [Fact]
        public void Equals_WithCaseInsensitiveFrameworkName_ReturnsTrue()
        {
            var leftSide = new ProjectFileDependencyGroup("frameworkName", Enumerable.Empty<string>());
            var rightSide = new ProjectFileDependencyGroup("FrameworkName", Enumerable.Empty<string>());
            leftSide.Should().Be(rightSide);
        }

        [Fact]
        public void Equals_WithDifferentCountDependencies_ReturnsFalse()
        {
            var leftSide = new ProjectFileDependencyGroup("frameworkName", new string[] { "a", "c", "b", "extra" });
            var rightSide = new ProjectFileDependencyGroup("frameworkName", new string[] { "a", "c", "b" });
            leftSide.Should().NotBe(rightSide);
        }

        [Fact]
        public void Equals_WithCaseInsensitiveDependencies_ReturnsTrue()
        {
            var leftSide = new ProjectFileDependencyGroup("frameworkName", new string[] { "a", "c", "b" });
            var rightSide = new ProjectFileDependencyGroup("frameworkName", new string[] { "B", "a", "C" });
            leftSide.Should().Be(rightSide);
        }

        [Fact]
        public void HashCode_WithDifferentFrameworkName_IsNotEqual()
        {
            var leftSide = new ProjectFileDependencyGroup("frameworkName", Enumerable.Empty<string>());
            var rightSide = new ProjectFileDependencyGroup("differetName", Enumerable.Empty<string>());
            leftSide.GetHashCode().Should().NotBe(rightSide.GetHashCode());
        }

        [Fact]
        public void HashCode_WithCaseInsensitiveFrameworkName_IsEqual()
        {
            var leftSide = new ProjectFileDependencyGroup("frameworkName", Enumerable.Empty<string>());
            var rightSide = new ProjectFileDependencyGroup("FrameworkName", Enumerable.Empty<string>());
            leftSide.GetHashCode().Should().Be(rightSide.GetHashCode());
        }

        [Fact]
        public void HashCode_WithDifferentCountDependencies_IsNotEqual()
        {
            var leftSide = new ProjectFileDependencyGroup("frameworkName", new string[] { "a", "c", "b", "extra" });
            var rightSide = new ProjectFileDependencyGroup("frameworkName", new string[] { "a", "c", "b" });
            leftSide.GetHashCode().Should().NotBe(rightSide.GetHashCode());
        }

        [Fact]
        public void HashCode_WithCaseInsensitiveDependencies_IsEqual()
        {
            var leftSide = new ProjectFileDependencyGroup("frameworkName", new string[] { "a", "c", "b" });
            var rightSide = new ProjectFileDependencyGroup("frameworkName", new string[] { "B", "a", "C" });
            leftSide.GetHashCode().Should().Be(rightSide.GetHashCode());
        }
    }
}
