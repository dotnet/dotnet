// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Globalization;
using System.Reflection;
using Xunit;

namespace Microsoft.DotNet.XUnitExtensions.Tests
{
    public class CulturedConditionalAttributeTests
    {
        // These tests validate the xunit v3 cultured conditional attributes without relying on
        // execution order, which the v3 runner does not guarantee for this scenario.

        private static readonly string[] s_cultures = new[] { "en-US", "fr-FR" };

        public static bool AlwaysTrue => true;
        public static bool AlwaysFalse => false;

        [CulturedConditionalFact(new[] { "en-US", "fr-FR" }, typeof(CulturedConditionalAttributeTests), new[] { nameof(AlwaysTrue) })]
        public void CulturedConditionalFactTrue()
        {
            // The current culture must be one of the requested cultures when the test runs.
            Assert.Contains(CultureInfo.CurrentCulture.Name, s_cultures);
        }

        [CulturedConditionalFact(new[] { "en-US", "fr-FR" }, typeof(CulturedConditionalAttributeTests), new[] { nameof(AlwaysFalse) })]
        public void CulturedConditionalFactFalse()
        {
            Assert.Fail("This test should have been skipped.");
        }

        [CulturedConditionalTheory(new[] { "en-US", "fr-FR" }, typeof(CulturedConditionalAttributeTests), new[] { nameof(AlwaysTrue) })]
        [InlineData(1, "one")]
        [InlineData(2, "two")]
        public void CulturedConditionalTheoryTrue(int number, string name)
        {
            // Verify the arguments were actually passed through and the culture was set.
            Assert.True(number > 0);
            Assert.False(string.IsNullOrEmpty(name));
            Assert.Contains(CultureInfo.CurrentCulture.Name, s_cultures);
        }

        [CulturedConditionalTheory(new[] { "en-US", "fr-FR" }, typeof(CulturedConditionalAttributeTests), new[] { nameof(AlwaysFalse) })]
        [InlineData(1)]
        [InlineData(2)]
#pragma warning disable xUnit1026 // Theory methods should use all of their parameters
        public void CulturedConditionalTheoryFalse(int value)
#pragma warning restore xUnit1026
        {
            Assert.Fail($"This test should have been skipped, but ran with value {value}.");
        }

        [Fact]
        public void ValidateCulturedConditionalFactSkipState()
        {
            Assert.Null(GetCulturedConditionalFactAttribute(nameof(CulturedConditionalFactTrue)).Skip);
            Assert.Equal("Condition(s) not met: \"AlwaysFalse\"", GetCulturedConditionalFactAttribute(nameof(CulturedConditionalFactFalse)).Skip);
        }

        [Fact]
        public void ValidateCulturedConditionalTheorySkipState()
        {
            Assert.Null(GetCulturedConditionalTheoryAttribute(nameof(CulturedConditionalTheoryTrue)).Skip);
            Assert.Equal("Condition(s) not met: \"AlwaysFalse\"", GetCulturedConditionalTheoryAttribute(nameof(CulturedConditionalTheoryFalse)).Skip);
        }

        [Fact]
        public void ValidateCulturedConditionalFactProperties()
        {
            CulturedConditionalFactAttribute attribute = GetCulturedConditionalFactAttribute(nameof(CulturedConditionalFactTrue));
            Assert.Equal(typeof(CulturedConditionalAttributeTests), attribute.CalleeType);
            Assert.Equal(new[] { nameof(AlwaysTrue) }, attribute.ConditionMemberNames);
            Assert.Equal(s_cultures, attribute.Cultures);
        }

        [Fact]
        public void ValidateCulturedConditionalTheoryProperties()
        {
            CulturedConditionalTheoryAttribute attribute = GetCulturedConditionalTheoryAttribute(nameof(CulturedConditionalTheoryTrue));
            Assert.Equal(typeof(CulturedConditionalAttributeTests), attribute.CalleeType);
            Assert.Equal(new[] { nameof(AlwaysTrue) }, attribute.ConditionMemberNames);
            Assert.Equal(s_cultures, attribute.Cultures);
        }

        private static CulturedConditionalFactAttribute GetCulturedConditionalFactAttribute(string methodName)
        {
            return (CulturedConditionalFactAttribute)typeof(CulturedConditionalAttributeTests)
                .GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public)!
                .GetCustomAttribute(typeof(CulturedConditionalFactAttribute), inherit: false)!;
        }

        private static CulturedConditionalTheoryAttribute GetCulturedConditionalTheoryAttribute(string methodName)
        {
            return (CulturedConditionalTheoryAttribute)typeof(CulturedConditionalAttributeTests)
                .GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public)!
                .GetCustomAttribute(typeof(CulturedConditionalTheoryAttribute), inherit: false)!;
        }
    }
}
