// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.Packaging.Core;
using NuGet.Shared;
using NuGet.Versioning;
using Xunit;

namespace NuGet.Packaging.Tests
{
    public class PackageDependencyInfoComparerTests
    {
        private static readonly PackageDependencyInfoComparer Comparer = PackageDependencyInfoComparer.Default;
        private static readonly NuGetVersion Version100 = NuGetVersion.Parse("1.0.0");
        private static readonly PackageIdentity PackageIdentity = new PackageIdentity(id: "a", Version100);
        private static readonly PackageDependencyInfo DependencyInfoA = new PackageDependencyInfo(
            PackageIdentity,
            new PackageDependency[] { new PackageDependency(id: "c") });

        [Fact]
        public void Constructor_WhenIdentityComparerIsNull_Throws()
        {
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(
                () => new PackageDependencyInfoComparer(
                    identityComparer: null,
                    PackageDependencyComparer.Default));

            Assert.Equal("identityComparer", exception.ParamName);
        }

        [Fact]
        public void Constructor_WhenDependencyComparerIsNull_Throws()
        {
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(
                () => new PackageDependencyInfoComparer(
                    PackageIdentityComparer.Default,
                    dependencyComparer: null));

            Assert.Equal("dependencyComparer", exception.ParamName);
        }

        [Fact]
        public void Default_Always_ReturnsSameInstance()
        {
            PackageDependencyInfoComparer instance0 = PackageDependencyInfoComparer.Default;
            PackageDependencyInfoComparer instance1 = PackageDependencyInfoComparer.Default;

            Assert.Same(instance0, instance1);
        }

        [Fact]
        public void Equals_WhenArgumentsAreNull_ReturnsTrue()
        {
            Assert.True(Comparer.Equals(x: null, y: null));
        }

        [Fact]
        public void Equals_WhenArgumentsAreSame_ReturnsTrue()
        {
            Assert.True(Comparer.Equals(DependencyInfoA, DependencyInfoA));
        }

        [Fact]
        public void Equals_WhenArgumentsAreEqual_ReturnsTrue()
        {
            var dependencyInfoA1 = new PackageDependencyInfo(PackageIdentity, DependencyInfoA.Dependencies);

            Assert.True(Comparer.Equals(DependencyInfoA, dependencyInfoA1));
        }

        [Fact]
        public void Equals_WhenOneArgumentIsNull_ReturnsFalse()
        {
            Assert.False(Comparer.Equals(DependencyInfoA, y: null));
            Assert.False(Comparer.Equals(x: null, DependencyInfoA));
        }

        [Fact]
        public void Equals_WhenArgumentsHaveUnequalVersion_ReturnsFalse()
        {
            var dependencyInfoA1 = new PackageDependencyInfo(
                new PackageIdentity(DependencyInfoA.Id, NuGetVersion.Parse("2.0.0")),
                DependencyInfoA.Dependencies);

            Assert.False(Comparer.Equals(DependencyInfoA, dependencyInfoA1));
        }

        [Fact]
        public void Equals_WhenArgumentsHaveUnequalDependencies_ReturnsFalse()
        {
            var dependencyInfoA1 = new PackageDependencyInfo(
                PackageIdentity,
                new[] { new PackageDependency(id: "d") });

            Assert.False(Comparer.Equals(DependencyInfoA, dependencyInfoA1));
        }

        [Fact]
        public void Equals_WhenArgumentsHaveUnequalPackageIds_ReturnsFalse()
        {
            var dependencyInfoB = new PackageDependencyInfo(
                new PackageIdentity(id: "b", DependencyInfoA.Version),
                DependencyInfoA.Dependencies);

            Assert.False(Comparer.Equals(DependencyInfoA, dependencyInfoB));
        }

        [Fact]
        public void GetHashCode_WhenArgumentIsNull_ReturnsZero()
        {
            int hashCode = Comparer.GetHashCode(obj: null);

            Assert.Equal(0, hashCode);
        }

        [Fact]
        public void GetHashCode_WhenArgumentIsValid_ReturnsHashCode()
        {
            var combiner = new HashCodeCombiner();

            combiner.AddObject(DependencyInfoA, PackageIdentityComparer.Default);
            combiner.AddUnorderedSequence(DependencyInfoA.Dependencies);

            int expectedResult = combiner.CombinedHash;
            int actualResult = Comparer.GetHashCode(DependencyInfoA);

            Assert.Equal(expectedResult, actualResult);
        }
    }
}
