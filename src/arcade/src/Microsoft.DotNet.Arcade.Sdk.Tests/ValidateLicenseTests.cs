// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Xunit;

namespace Microsoft.DotNet.Arcade.Sdk.Tests
{
    public class ValidateLicenseTests
    {
        [Fact]
        public void LinesEqual()
        {
            Assert.False(ValidateLicense.LinesEqual(new[] { "a" }, new[] { "b" }));
            Assert.False(ValidateLicense.LinesEqual(new[] { "a" }, new[] { "A" }));
            Assert.False(ValidateLicense.LinesEqual(new[] { "a" }, new[] { "a", "b" }));
            Assert.False(ValidateLicense.LinesEqual(new[] { "a" }, new[] { "a", "*ignore-line*" }));
            Assert.False(ValidateLicense.LinesEqual(new[] { "*ignore-line*" }, new[] { "a" }));
            Assert.True(ValidateLicense.LinesEqual(new[] { "a" }, new[] { "*ignore-line*" }));

            Assert.True(ValidateLicense.LinesEqual(new[] { "a", "    ", "   b", "xxx", "\t \t" }, new[] { "a", "b    ", "*ignore-line*" }));
        }
    }
}
