// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Xunit;

namespace NuGet.ProjectModel.Test
{
    public class StringExtensionTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void SplitInTwo_WithNullOrEmptyString_ReturnStringAndNull(string s)
        {
            var result = s.SplitInTwo('/');
            Assert.Equal(s, result.firstPart);
            Assert.Null(result.secondPart);
        }

        [Theory]
        [InlineData("part1/part2", "part1", "part2")]
        [InlineData("part1/part2/NotPart3", "part1", "part2/NotPart3")]
        public void SplitInTwo_WithSeperator_ReturnStringInTwoParts(string s, string expectedFirstPart, string expectedSecondPart)
        {
            var results = s.SplitInTwo('/');
            Assert.Equal(expectedFirstPart, results.firstPart);
            Assert.Equal(expectedSecondPart, results.secondPart);
        }
    }
}
