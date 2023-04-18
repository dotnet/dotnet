// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.Shared;
using Xunit;

namespace NuGet.Versioning.Test
{
    public class VersionRangeComparerTests
    {
        private static readonly NuGetVersion Version100 = NuGetVersion.Parse("1.0.0");
        private static readonly NuGetVersion Version200 = NuGetVersion.Parse("2.0.0");
        private static readonly VersionRange VersionRange = new VersionRange();

        [Fact]
        public void Constructor_WhenVersionComparerIsNull_Throws()
        {
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(
                () => new VersionRangeComparer(versionComparer: null!));

            Assert.Equal("versionComparer", exception.ParamName);
        }

        [Fact]
        public void Default_Always_ReturnsSameInstance()
        {
            IVersionRangeComparer instance0 = VersionRangeComparer.Default;
            IVersionRangeComparer instance1 = VersionRangeComparer.Default;

            Assert.Same(instance0, instance1);
        }

        [Fact]
        public void VersionRelease_Always_ReturnsSameInstance()
        {
            IVersionRangeComparer instance0 = VersionRangeComparer.VersionRelease;
            IVersionRangeComparer instance1 = VersionRangeComparer.VersionRelease;

            Assert.Same(instance0, instance1);
        }

        [Fact]
        public void Equals_WhenArgumentsAreSame_ReturnsTrue()
        {
            Assert.True(VersionRangeComparer.Default.Equals(VersionRange, VersionRange));
        }

        [Fact]
        public void Equals_WhenXArgumentIsNull_ReturnsFalse()
        {
            Assert.False(VersionRangeComparer.Default.Equals(x: null!, y: VersionRange));
        }

        [Fact]
        public void Equals_WhenYArgumentIsNull_ReturnsFalse()
        {
            Assert.False(VersionRangeComparer.Default.Equals(x: VersionRange, y: null!));
        }

        [Fact]
        public void Equals_WhenBothArgumentsAreNull_ReturnsTrue()
        {
            Assert.True(VersionRangeComparer.Default.Equals(x: null!, y: null!));
        }

        [Fact]
        public void Equals_WhenIsMinInclusiveValuesAreDifferent_ReturnsFalse()
        {
            var range0 = new VersionRange(minVersion: Version100, includeMinVersion: true);
            var range1 = new VersionRange(minVersion: Version100, includeMinVersion: false);

            Assert.False(VersionRangeComparer.Default.Equals(range0, range1));
        }

        [Fact]
        public void Equals_WhenIsMaxInclusiveValuesAreDifferent_ReturnsFalse()
        {
            var range0 = new VersionRange(maxVersion: Version200, includeMaxVersion: true);
            var range1 = new VersionRange(maxVersion: Version200, includeMaxVersion: false);

            Assert.False(VersionRangeComparer.Default.Equals(range0, range1));
        }

        [Fact]
        public void Equals_WhenMinVersionValuesAreDifferent_ReturnsFalse()
        {
            var range0 = new VersionRange(minVersion: Version100);
            var range1 = new VersionRange(minVersion: Version200);

            Assert.False(VersionRangeComparer.Default.Equals(range0, range1));
        }

        [Fact]
        public void Equals_WhenMaxVersionValuesAreDifferent_ReturnsFalse()
        {
            var range0 = new VersionRange(maxVersion: Version100);
            var range1 = new VersionRange(maxVersion: Version200);

            Assert.False(VersionRangeComparer.Default.Equals(range0, range1));
        }

        [Fact]
        public void Equals_WhenValuesAreEqual_ReturnsTrue()
        {
            Assert.True(VersionRangeComparer.Default.Equals(VersionRange, new VersionRange()));
        }

        [Fact]
        public void GetHashCode_WhenArgumentIsNull_ReturnsZero()
        {
            int hashCode = VersionRangeComparer.Default.GetHashCode(obj: null!);

            Assert.Equal(0, hashCode);
        }

        [Fact]
        public void GetHashCode_WhenArgumentIsValid_ReturnsHashCode()
        {
            var range = new VersionRange(Version100, includeMinVersion: true, Version200, includeMaxVersion: true);
            var combiner = new HashCodeCombiner();
            IVersionComparer comparer = VersionComparer.Default;

            combiner.AddObject(range.IsMinInclusive);
            combiner.AddObject(range.IsMaxInclusive);
            combiner.AddObject(comparer.GetHashCode(range.MinVersion!));
            combiner.AddObject(comparer.GetHashCode(range.MaxVersion!));

            int expectedResult = combiner.CombinedHash;
            int actualResult = VersionRangeComparer.Default.GetHashCode(range);

            Assert.Equal(expectedResult, actualResult);
        }
    }
}
