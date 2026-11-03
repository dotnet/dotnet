// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.Versioning;
using Xunit;

namespace NuGet.LibraryModel
{
    public class DownloadDependencyTests
    {
        [Theory]
        [InlineData("PACKAGE", "package", true)]
        [InlineData("package", "another", false)]
        public void Equals_WithNames_ReturnsExpected(string name, string otherName, bool expected)
        {
            var versionRange = VersionRange.Parse("[1.0.0]");
            var dependency = new DownloadDependency(name, versionRange);
            var other = new DownloadDependency(otherName, versionRange);

            Assert.Equal(expected, dependency.Equals(other));
            if (expected)
            {
                Assert.Equal(dependency.GetHashCode(), other.GetHashCode());
                Assert.Equal(0, dependency.CompareTo(other));
            }
        }

        [Fact]
        public void Constructor_WithNullName_Throws()
        {
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(
                () => new DownloadDependency(null!, VersionRange.Parse("[1.0.0]")));

            Assert.Equal("name", exception.ParamName);
        }

        [Fact]
        public void Constructor_WithNullVersionRange_Throws()
        {
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(
                () => new DownloadDependency("package", null!));

            Assert.Equal("versionRange", exception.ParamName);
        }

        [Fact]
        public void ImplicitConversion_WithNullDependency_Throws()
        {
            DownloadDependency dependency = null!;

            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(
                () => _ = (LibraryRange)dependency);

            Assert.Equal("library", exception.ParamName);
        }

        [Fact]
        public void ImplicitConversion_WithName_PreservesNameAndVersion()
        {
            var dependency = new DownloadDependency("package", VersionRange.Parse("[1.0.0]"));

            LibraryRange libraryRange = dependency;

            Assert.Equal(dependency.Name, libraryRange.Name);
            Assert.Equal(dependency.VersionRange, libraryRange.VersionRange);
            Assert.Equal(LibraryDependencyTarget.Package, libraryRange.TypeConstraint);
        }
    }
}
