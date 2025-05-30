// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using FluentAssertions;
using NuGet.Versioning;
using Xunit;

namespace NuGet.LibraryModel.Tests
{
    public class PrunePackageReferenceTests
    {
        [Fact]
        public void Constructor_ValidParameters_CreatesInstance()
        {
            // Arrange
            string name = "TestPackage";
            VersionRange version = VersionRange.Parse("1.0.0");
            // Act
            var prunePackageReference = new PrunePackageReference(name, version);

            // Assert
            prunePackageReference.Name.Should().Be(name);
            prunePackageReference.VersionRange.Should().Be(version);
        }

        [Fact]
        public void Create_ValidParameters_CreatesInstance()
        {
            // Arrange
            string name = "TestPackage";
            string version = "1.0.0";
            // Act
            var prunePackageReference = PrunePackageReference.Create(name, version);

            // Assert
            prunePackageReference.Name.Should().Be(name);
            prunePackageReference.VersionRange.OriginalString.Should().Be("(," + version + "]");
            prunePackageReference.VersionRange.IsMinInclusive.Should().BeFalse();
            prunePackageReference.VersionRange.IsMaxInclusive.Should().BeTrue();
            prunePackageReference.VersionRange.MinVersion.Should().BeNull();
            prunePackageReference.VersionRange.MaxVersion.Should().Be(NuGetVersion.Parse("1.0.0"));
        }

        [Theory]
        [InlineData("TestPackage", "TestPackage", "1.0.0", "1.0.0", true)]
        [InlineData("testpackage", "TestPackage", "1.0.0", "1.0.0", true)]
        [InlineData("TestPackage", "testpackage", "1.0.0", "1.0.0", true)]
        [InlineData("TestPackage", "TestPackage", "2.0.0", "1.0.0", false)]
        [InlineData("TestPackage", "TestPackage", "1.0.0", "2.0.0", false)]
        [InlineData("TestPackage1", "TestPackage", "1.0.0", "1.0.0", false)]
        public void Equals_ComparesPackageReferences_ReturnsExpectedResult(
            string leftName,
            string rightName,
            string leftVersion,
            string rightVersion,
            bool expectedResult)
        {
            // Arrange
            var left = new PrunePackageReference(leftName, VersionRange.Parse(leftVersion));
            var right = new PrunePackageReference(rightName, VersionRange.Parse(rightVersion));

            // Act & Assert
            left.Equals(right).Should().Be(expectedResult);
        }

        [Theory]
        [InlineData("TestPackage", "TestPackage", "1.0.0", "1.0.0", true)]
        [InlineData("testpackage", "TestPackage", "1.0.0", "1.0.0", true)]
        [InlineData("TestPackage", "testpackage", "1.0.0", "1.0.0", true)]
        [InlineData("TestPackage", "TestPackage", "2.0.0", "1.0.0", false)]
        [InlineData("TestPackage", "TestPackage", "1.0.0", "2.0.0", false)]
        [InlineData("TestPackage1", "TestPackage", "1.0.0", "1.0.0", false)]
        public void HashCode_ComparesPackageReferences_ReturnsExpectedResult(
            string leftName,
            string rightName,
            string leftVersion,
            string rightVersion,
            bool expectedResult)
        {
            // Arrange
            var left = new PrunePackageReference(leftName, VersionRange.Parse(leftVersion));
            var right = new PrunePackageReference(rightName, VersionRange.Parse(rightVersion));

            // Act & Assert
            left.GetHashCode().Equals(right.GetHashCode()).Should().Be(expectedResult);
        }
    }
}
