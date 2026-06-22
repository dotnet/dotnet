// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Reflection;
using Xunit;

namespace Microsoft.DotNet.XUnitExtensions.Tests
{
    public class ConditionalAttributeTests
    {
        // These tests validate the xunit v3 conditional attributes without relying on
        // execution order, which the v3 runner does not guarantee for this scenario.

        public static bool AlwaysTrue => true;
        public static bool AlwaysFalse => false;

        [ConditionalFact(typeof(ConditionalAttributeTests), nameof(AlwaysTrue))]
        public void ConditionalAttributeTrue()
        {
            Assert.True(AlwaysTrue);
        }

        [ConditionalFact(typeof(ConditionalAttributeTests), nameof(AlwaysFalse))]
        public void ConditionalAttributeFalse()
        {
            Assert.Fail("This test should have been skipped.");
        }

        [ConditionalTheory(typeof(ConditionalAttributeTests), nameof(AlwaysTrue))]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void ConditionalTheoryTrue(int value)
        {
            // Verify the argument was actually passed through (the bug being tested).
            Assert.True(value > 0, $"Expected a positive value but got {value}");
        }

        [ConditionalTheory(typeof(ConditionalAttributeTests), nameof(AlwaysFalse))]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
#pragma warning disable xUnit1026 // Theory methods should use all of their parameters
        public void ConditionalTheoryFalse(int value)
#pragma warning restore xUnit1026
        {
            Assert.Fail($"This test should have been skipped, but ran with value {value}.");
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
        public void ValidateConditionalFactSkipState()
        {
            Assert.Null(GetConditionalFactAttribute(nameof(ConditionalAttributeTrue)).Skip);
            Assert.Equal("Condition(s) not met: \"AlwaysFalse\"", GetConditionalFactAttribute(nameof(ConditionalAttributeFalse)).Skip);
        }

        [Fact]
        public void ValidateConditionalTheorySkipState()
        {
            Assert.Null(GetConditionalTheoryAttribute(nameof(ConditionalTheoryTrue)).Skip);
            Assert.Equal("Condition(s) not met: \"AlwaysFalse\"", GetConditionalTheoryAttribute(nameof(ConditionalTheoryFalse)).Skip);
        }

        [Fact]
        public void ValidateConditionalTheoryTrueReceivedArgs()
        {
            Assert.NotNull(GetConditionalTheoryAttribute(nameof(ConditionalTheoryTrue)));
        }

        private static ConditionalFactAttribute GetConditionalFactAttribute(string methodName)
        {
            return (ConditionalFactAttribute)typeof(ConditionalAttributeTests)
                .GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public)!
                .GetCustomAttribute(typeof(ConditionalFactAttribute), inherit: false)!;
        }

        private static ConditionalTheoryAttribute GetConditionalTheoryAttribute(string methodName)
        {
            return (ConditionalTheoryAttribute)typeof(ConditionalAttributeTests)
                .GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public)!
                .GetCustomAttribute(typeof(ConditionalTheoryAttribute), inherit: false)!;
        }
    }
}
