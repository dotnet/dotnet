// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Microsoft.DotNet.XUnitExtensions.Tests
{
    [TestCaseOrderer(typeof(AlphabeticalOrderer))]
    public class ConditionalAttributeTests
    {
        // The tests under this class validate that ConditionalFact and ConditionalTheory
        // tests are discovered and executed correctly under xunit v3.
        // This test class is test-order dependent so do not rename the tests.

        private static bool s_conditionalFactTrueExecuted;
        private static bool s_conditionalFactFalseExecuted;
        private static int s_conditionalTheoryTrueCount;
        private static int s_conditionalTheoryFalseCount;
        private static readonly List<int> s_conditionalTheoryTrueArgs = new();

        public static bool AlwaysTrue => true;
        public static bool AlwaysFalse => false;

        [ConditionalFact(typeof(ConditionalAttributeTests), nameof(AlwaysTrue))]
        public void ConditionalAttributeTrue()
        {
            s_conditionalFactTrueExecuted = true;
        }

        [ConditionalFact(typeof(ConditionalAttributeTests), nameof(AlwaysFalse))]
        public void ConditionalAttributeFalse()
        {
            s_conditionalFactFalseExecuted = true;
        }

        [ConditionalTheory(typeof(ConditionalAttributeTests), nameof(AlwaysTrue))]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void ConditionalTheoryTrue(int value)
        {
            // Verify the argument was actually passed through (the bug being tested).
            Assert.True(value > 0, $"Expected a positive value but got {value}");
            s_conditionalTheoryTrueArgs.Add(value);
            s_conditionalTheoryTrueCount++;
        }

        [ConditionalTheory(typeof(ConditionalAttributeTests), nameof(AlwaysFalse))]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
#pragma warning disable xUnit1026 // Theory methods should use all of their parameters
        public void ConditionalTheoryFalse(int value)
#pragma warning restore xUnit1026
        {
            s_conditionalTheoryFalseCount++;
        }

        [ConditionalTheory(typeof(ConditionalAttributeTests), nameof(AlwaysTrue))]
        [InlineData("hello")]
        [InlineData("world")]
        public void ConditionalTheoryTrueStringArgs(string text)
        {
            // Verify string arguments are passed through correctly.
            Assert.False(string.IsNullOrEmpty(text), "Expected a non-empty string argument");
        }

        [ConditionalTheory(typeof(ConditionalAttributeTests), nameof(AlwaysTrue))]
        [InlineData(10, "ten")]
        [InlineData(20, "twenty")]
        public void ConditionalTheoryTrueMultipleArgs(int number, string name)
        {
            // Verify multiple arguments are passed through correctly.
            Assert.True(number > 0);
            Assert.False(string.IsNullOrEmpty(name));
        }

        [Fact]
        public void ValidateConditionalFactTrue()
        {
            Assert.True(s_conditionalFactTrueExecuted);
        }

        [Fact]
        public void ValidateConditionalFactFalse()
        {
            Assert.False(s_conditionalFactFalseExecuted);
        }

        [Fact]
        public void ValidateConditionalTheoryTrue()
        {
            Assert.Equal(3, s_conditionalTheoryTrueCount);
        }

        [Fact]
        public void ValidateConditionalTheoryTrueReceivedArgs()
        {
            // This is the key test: if testMethodArguments were dropped,
            // the data row values would not reach the test method.
            Assert.Equal(new[] { 1, 2, 3 }, s_conditionalTheoryTrueArgs.OrderBy(x => x).ToArray());
        }

        [Fact]
        public void ValidateConditionalTheoryFalse()
        {
            Assert.Equal(0, s_conditionalTheoryFalseCount);
        }
    }
}
