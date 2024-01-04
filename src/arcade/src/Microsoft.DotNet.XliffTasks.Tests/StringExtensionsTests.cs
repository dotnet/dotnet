// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Xunit;

namespace XliffTasks.Tests
{
    public class StringExtensionsTests
    {
        [Fact]
        public void GetReplacementCount_NoPlaceholders()
        {
            string text = "Alpha";
            int replacementCount = text.GetReplacementCount();

            Assert.Equal(expected: 0, actual: replacementCount);
        }

        [Fact]
        public void GetReplacementCount_OneSimplePlaceholder()
        {
            string text = "{0}";
            int replacementCount = text.GetReplacementCount();

            Assert.Equal(expected: 1, actual: replacementCount);
        }

        [Fact]
        public void GetReplacementCount_PlaceholderWithAlignment()
        {
            string text = "{0,-3}";
            int replacementCount = text.GetReplacementCount();

            Assert.Equal(expected: 1, actual: replacementCount);
        }

        [Fact]
        public void GetReplacementCount_PlaceholderWithFormatString()
        {
            string text = "{0:N}";
            int replacementCount = text.GetReplacementCount();

            Assert.Equal(expected: 1, actual: replacementCount);
        }

        [Fact]
        public void GetReplacementCount_WithAlignmentAndFormatString()
        {
            string text = "{0,-10:G1}";
            int replacementCount = text.GetReplacementCount();

            Assert.Equal(expected: 1, actual: replacementCount);
        }

        [Fact]
        public void GetReplacementCount_MultiplePlaceholders()
        {
            string text = "{0} {1}";
            int replacementCount = text.GetReplacementCount();

            Assert.Equal(expected: 2, actual: replacementCount);
        }

        [Fact]
        public void GetReplacementCount_MultipleIdenticalPlaceholders()
        {
            string text = "{2} {2}";
            int replacementCount = text.GetReplacementCount();

            Assert.Equal(expected: 3, actual: replacementCount);
        }
    }
}