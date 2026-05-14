// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using Xunit;

namespace NuGet.Indexing.Test
{
    public class TokenizingHelperTests
    {
        [Theory]
        [InlineData("Test", new string[] { "Test" })]
        [InlineData("TestSeparator", new string[] { "Test", "Separator" })]
        [InlineData("1TestSeparator", new string[] { "1Test", "Separator" })]
        public void TestCamelCaseSeparator(string input, IEnumerable<string> expected)
        {
            var words = TokenizingHelper.CamelCaseSplit(input);
            Assert.Equal(words, expected);
        }

    }
}
