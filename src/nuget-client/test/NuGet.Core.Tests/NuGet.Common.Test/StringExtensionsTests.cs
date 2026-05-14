// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Xunit;

namespace NuGet.Common.Test
{
    public class StringExtensionsTests
    {
        [Fact]
        public void FormatWithDoubleQuotes_WhenNullIsPassedReturnsNull_Success()
        {
            string? actual = null;
            string? formatted = actual.FormatWithDoubleQuotes();
            Assert.Equal(actual, formatted);
        }

        [Theory]
        [InlineData("")]
        [InlineData("/home/user/NuGet AppData/share")]
        [InlineData("/home/user/NuGet/share")]
        [InlineData(@"c:\users\NuGet App\Share")]
        [InlineData(@"c:\users\NuGet\Share")]
        public void FormatWithDoubleQuotes_WhenNonNullStringIsPassedReturnsFormattedString_Success(string actual)
        {
            string? formatted = actual.FormatWithDoubleQuotes();
            Assert.Equal($@"""{actual}""", formatted);
        }
    }
}
