// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using FluentAssertions;
using NuGet.ContentModel;
using Xunit;

namespace NuGet.Packaging.Test.ContentModelTests
{
    public class ReadOnlyMemoryCharComparerOrdinalTests
    {
        [Theory]
        [InlineData("ABCDEFG", "ABCDEFG", true)]
        [InlineData("ABCDEFG", "abcdefg", false)]
        [InlineData("ABCDEFGH", "ABCDEFG", false)]
        [InlineData("ZABCDEFG", "ABCDEFG", false)]
        [InlineData("ABCEFGD", "ABCDEFG", false)]
        [InlineData("12345", "123465", false)]
        [InlineData("12345", "1234765", false)]
        public void Equals_ReturnsExpectedValue(string x, string y, bool expected)
        {
            ReadOnlyMemoryCharComparerOrdinal.Instance.Equals(x.AsMemory(), y.AsMemory()).Should().Be(expected);
        }

        [Theory]
        [InlineData("ABCDEFG", "ABCDEFG", true)]
        [InlineData("ABCDEFG", "abcdefg", false)]
        [InlineData("ABCDEFGH", "ABCDEFG", false)]
        [InlineData("ZABCDEFG", "ABCDEFG", false)]
        [InlineData("ABCEFGD", "ABCDEFG", false)]
        [InlineData("12345", "123465", false)]
        [InlineData("12345", "1234765", false)]
        public void GetHashCode_ReturnsExpectedValue(string x, string y, bool expected)
        {
            int hashX = ReadOnlyMemoryCharComparerOrdinal.Instance.GetHashCode(x.AsMemory());
            int hashY = ReadOnlyMemoryCharComparerOrdinal.Instance.GetHashCode(y.AsMemory());

            hashX.Equals(hashY).Should().Be(expected, $"hashX is {hashX} and hashY is {hashY}");
        }
    }
}
