// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using Xunit;

namespace NuGet.Versioning.Test
{
    public class VersionRangeSetTests
    {
        [Theory]
        [InlineData("[1.0.0, )", "[1.0.0, )")]
        [InlineData("[1.0.0, )", "[1.0.1, )")]
        [InlineData("[1.0.0-alpha, )", "[1.0.0, )")]
        [InlineData("[1.0.0]", "[1.0.0]")]
        [InlineData("[1.0.0, 2.0.0]", "(1.1.0, 1.5.0)")]
        [InlineData("(, )", "[1.0.0, )")]
        [InlineData("(0.0.0, )", "[1.0.0, )")]
        [InlineData("(0.0.0, 0.0.0)", "(0.0.0, 0.0.0)")]
        [InlineData("(1.0.0-alpha, 2.0.0]", "[2.0.0]")]
        [InlineData("(1.0.0-alpha, 2.0.0]", "(2.0.0, 2.0.0)")]
        [InlineData("(2.0.0, 2.0.0)", "(2.0.0, 2.0.0)")]
        public void VersionRangeSet_SubSetTest(string superSet, string subSet)
        {
            var superSetRange = VersionRange.Parse(superSet);
            var subSetRange = VersionRange.Parse(subSet);

            Assert.True(subSetRange.IsSubSetOrEqualTo(superSetRange));
        }

        [Theory]
        [InlineData("[1.0.1, )", "[1.0.0, )")]
        [InlineData("[1.0.1, )", "[1.0.1-alpha, )")]
        [InlineData("[1.0.0, 2.0.0)", "[1.0.0, 2.0.0]")]
        [InlineData("[1.0.0, 2.0.0)", "[1.0.0-alpha, 2.0.0)")]
        [InlineData("[1.0.0, 2.0.0)", "[ , 2.0.0)")]
        [InlineData("(1.0.0, 2.0.0)", "[1.0.0, 2.0.0)")]
        [InlineData("(1.0.0, 2.0.0)", "[1.0.0]")]
        [InlineData("(1.0.0, 2.0.0)", "[1.0.0-beta]")]
        [InlineData("(1.0.0-alpha, 2.0.0]", "(3.0.0, 3.0.0)")]
        [InlineData("(3.0.0, 3.0.0)", "[3.0.0]")]
        public void VersionRangeSet_SubSetTestNeg(string superSet, string subSet)
        {
            var superSetRange = VersionRange.Parse(superSet);
            var subSetRange = VersionRange.Parse(subSet);

            Assert.False(subSetRange.IsSubSetOrEqualTo(superSetRange));
        }

        [Theory]
        [InlineData("[1.0.0, )", "[1.0.0, )", "[1.0.0, )")]
        [InlineData("(, 1.0.0)", "[0.0.0, 1.0.0)", "(, 1.0.0)")]
        [InlineData("[1.0.0, )", "[1.0.0, )", "(1.0.0, )")]
        [InlineData("[1.0.0-alpha, )", "[1.0.0-alpha, )", "[1.0.0, )")]
        [InlineData("[1.0.0, 2.0.0]", "[1.0.0]", "[2.0.0]")]
        [InlineData("[1.0.0, 2.0.0-beta-1]", "[1.0.0]", "[2.0.0-beta-1]")]
        [InlineData("[1.0.0, 3.0.0]", "[1.0.0, 2.0.0]", "[1.5.0, 3.0.0]")]
        [InlineData("(1.0.0, 3.0.0)", "(1.0.0, 2.0.0]", "[1.5.0, 3.0.0)")]
        [InlineData("[1.0.0, 2.0.0]", "(1.0.0, 2.0.0)", "[1.0.0, 2.0.0]")]
        [InlineData("[1.0.0, 2.0.0]", "[1.0.0, 1.5.0)", "(1.5.0, 2.0.0]")]
        [InlineData("(, )", "[1.0.0, 1.5.0)", "[, ]")]
        [InlineData("(, )", "[1.0.0, 1.5.0)", "(, )")]
        [InlineData("(, )", "[1.0.0-alpha, )", "(, 2.0.0-beta]")]
        [InlineData("[0.0.0-alpha-1, 9000.0.0.1]", "[0.0.0-alpha-1, 0.0.0-alpha-2]", "[10.0.0.0, 9000.0.0.1]")]
        public void VersionRangeSet_CombineTwoRanges(string expected, string rangeA, string rangeB)
        {
            // Arrange
            var a = VersionRange.Parse(rangeA);
            var b = VersionRange.Parse(rangeB);

            // Act
            var ranges = new List<VersionRange>() { a, b };
            var combined = VersionRange.Combine(ranges);

            var rangesRev = new List<VersionRange>() { b, a };
            var combinedRev = VersionRange.Combine(rangesRev);

            // Assert
            Assert.Equal(expected, combined.ToNormalizedString());

            // Verify the order has no effect
            Assert.Equal(expected, combinedRev.ToNormalizedString());
        }

        [Fact]
        public void VersionRangeSet_CombineSingleRangeList()
        {
            // Arrange
            var a = VersionRange.Parse("[1.0.0, )");
            var ranges = new List<VersionRange>() { a };

            // Act
            var combined = VersionRange.Combine(ranges);

            // Assert
            Assert.Equal(a.ToNormalizedString(), combined.ToNormalizedString());
        }

        [Fact]
        public void VersionRangeSet_CombineEmptyRangeList()
        {
            // Arrange
            var ranges = new List<VersionRange>() { };

            // Act
            var combined = VersionRange.Combine(ranges);

            // Assert
            Assert.Equal(VersionRange.None.ToNormalizedString(), combined.ToNormalizedString());
        }

        [Fact]
        public void VersionRangeSet_SpecialCaseRangeCombine_Nones()
        {
            // Arrange
            var ranges = new List<VersionRange>() { VersionRange.None, VersionRange.None, VersionRange.None };

            // Act
            var combined = VersionRange.Combine(ranges);

            // Assert
            Assert.Equal(VersionRange.None.ToNormalizedString(), combined.ToNormalizedString());
        }

        [Fact]
        public void VersionRangeSet_SpecialCaseRangeCombine_NoneAll()
        {
            // Arrange
            var ranges = new List<VersionRange>() { VersionRange.None, VersionRange.All };

            // Act
            var combined = VersionRange.Combine(ranges);

            // Assert
            Assert.Equal(VersionRange.All.ToNormalizedString(), combined.ToNormalizedString());
        }

        [Fact]
        public void VersionRangeSet_SpecialCaseRangeCombine_NonePlusOne()
        {
            // Arrange
            var ranges = new List<VersionRange>() { VersionRange.None, VersionRange.Parse("[1.0.0]") };

            // Act
            var combined = VersionRange.Combine(ranges);

            // Assert
            Assert.Equal("[1.0.0, 1.0.0]", combined.ToNormalizedString());
        }

        [Fact]
        public void VersionRangeSet_RemoveEmptyRanges()
        {
            // Arrange
            var ranges = new List<VersionRange>()
                {
                    VersionRange.None,
                    VersionRange.Parse("(5.0.0, 5.0.0)"),
                    VersionRange.Parse("(3.0.0-alpha, 3.0.0-alpha)"),
                    VersionRange.Parse("[1.0.0, 2.0.0]")
                };

            // Act
            var combined = VersionRange.Combine(ranges);

            // Assert
            Assert.Equal("[1.0.0, 2.0.0]", combined.ToNormalizedString());
        }

        [Fact]
        public void VersionRangeSet_SpecialCaseRangeCombine_All()
        {
            // Arrange
            var ranges = new List<VersionRange>()
                {
#pragma warning disable CS0618 // Type or member is obsolete
                VersionRange.AllStable, VersionRange.All,
                VersionRange.AllFloating, VersionRange.AllStableFloating, VersionRange.None
#pragma warning restore CS0618 // Type or member is obsolete

                };

            // Act
            var combined = VersionRange.Combine(ranges);

            // Assert
            Assert.Equal(VersionRange.All.ToNormalizedString(), combined.ToNormalizedString());
        }

        [Fact]
        public void VersionRangeSet_SpecialCaseRangeCombine_AllStablePlusPre()
        {
            // Arrange
            var pre = new VersionRange(new NuGetVersion("1.0.0"), true, new NuGetVersion("2.0.0"), true);
            var ranges = new List<VersionRange>() { VersionRange.AllStable, pre };

            // Act
            var combined = VersionRange.Combine(ranges);

            // Assert
            Assert.Equal(VersionRange.All.ToNormalizedString(), combined.ToNormalizedString());
        }

        [Fact]
        public void VersionRangeSet_SpecialCaseRangeCombine_AllStablePlusStable()
        {
            // Arrange
            var stable = new VersionRange(new NuGetVersion("1.0.0"), true, new NuGetVersion("2.0.0"), true);
            var ranges = new List<VersionRange>() { VersionRange.AllStable, stable };

            // Act
            var combined = VersionRange.Combine(ranges);

            // Assert
            Assert.Equal(VersionRange.AllStable.ToNormalizedString(), combined.ToNormalizedString());
        }

        [Fact]
        public void VersionRangeSet_CombineMultipleRanges()
        {
            // Arrange 
            var ranges = new List<VersionRange>()
                {
                    VersionRange.Parse("[1.0.0]"),
                    VersionRange.Parse("[2.0.0]"),
                    VersionRange.Parse("[3.0.0]"),
                    VersionRange.Parse("[4.0.0-beta-1]"),
                    VersionRange.Parse("[5.0.1-rc4]"),
                };

            // Act
            var combined = VersionRange.Combine(ranges);

            ranges.Reverse();

            // Assert
            Assert.Equal("[1.0.0, 5.0.1-rc4]", combined.ToNormalizedString());
        }

        [Fact]
        public void VersionRangeSet_CommonSubSet_SingleRangeList()
        {
            // Arrange
            var a = VersionRange.Parse("[1.0.0, )");
            var ranges = new List<VersionRange>() { a };

            // Act
            var combined = VersionRange.CommonSubSet(ranges);

            // Assert
            Assert.Equal(a.ToNormalizedString(), combined.ToNormalizedString());
        }

        [Fact]
        public void VersionRangeSet_CommonSubSet_EmptyRangeList()
        {
            // Arrange
            var ranges = new List<VersionRange>() { };

            // Act
            var combined = VersionRange.CommonSubSet(ranges);

            // Assert
            Assert.Equal(VersionRange.None.ToNormalizedString(), combined.ToNormalizedString());
        }

        [Theory]
        [InlineData("[2.0.0, )", "[1.0.0, )", "[2.0.0, )")]
        [InlineData("[0.0.0, 1.0.0)", "[0.0.0, 2.0.0)", "(, 1.0.0)")]
        [InlineData("[2.0.0, 3.0.0]", "(1.0.0, 3.0.0]", "[2.0.0, 4.0.0)")]
        [InlineData("(0.0.0, 0.0.0)", "[1.0.0, 3.0.0]", "[4.0.0, 5.0.0)")]
        [InlineData("(2.0.0, 3.0.0]", "(2.0.0, 3.0.0]", "[2.0.0, 4.0.0)")]
        [InlineData("(0.0.0, 0.0.0)", "[1.0.0, 3.0.0)", "[3.0.0, 5.0.0)")]
        [InlineData("(0.0.0, 0.0.0)", "[1.0.0, 3.0.0)", "[4.0.0, 5.0.0)")]
        [InlineData("(1.5.0, 2.0.0]", "[1.0.0, 2.0.0]", "(1.5.0, 3.0.0]")]
        [InlineData("[1.0.0, 1.5.0)", "[1.0.0, 1.5.0)", "[, ]")]
        [InlineData("(0.0.0, 0.0.0)", "[1.0.0]", "[2.0.0]")]
        public void VersionRangeSet_CommonSubSet(string expected, string rangeA, string rangeB)
        {
            // Arrange
            var a = VersionRange.Parse(rangeA);
            var b = VersionRange.Parse(rangeB);

            // Act
            var ranges = new List<VersionRange>() { a, b };
            var combined = VersionRange.CommonSubSet(ranges);

            var rangesRev = new List<VersionRange>() { b, a };
            var combinedRev = VersionRange.CommonSubSet(rangesRev);

            // Assert
            Assert.Equal(expected, combined.ToNormalizedString());

            // Verify the order has no effect
            Assert.Equal(expected, combinedRev.ToNormalizedString());
        }

        [Fact]
        public void VersionRangeSet_CommonSubSetInMultipleRanges()
        {
            // Arrange 
            var ranges = new List<VersionRange>()
                {
                    VersionRange.Parse("[1.0.0, 5.0.0)"),
                    VersionRange.Parse("[2.0.0, 6.0.0]"),
                    VersionRange.Parse("[4.0.0, 5.0.0]"),
                };

            // Act
            var combined = VersionRange.CommonSubSet(ranges);

            // Assert
            Assert.Equal("[4.0.0, 5.0.0)", combined.ToNormalizedString());
        }
    }
}
