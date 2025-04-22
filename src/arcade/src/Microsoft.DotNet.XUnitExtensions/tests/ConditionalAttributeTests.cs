// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Xunit;
using Xunit.Sdk;

namespace Microsoft.DotNet.XUnitExtensions.Tests
{
    [TestCaseOrderer("Microsoft.DotNet.XUnitExtensions.Tests.AlphabeticalOrderer", "Microsoft.DotNet.XUnitExtensions.Tests")]
    public class ConditionalAttributeTests
    {
        // The tests under this class are to validate that ConditionalFact and ConditionalAttribute tests are being executed correctly
        // In the past we have had cases where infrastructure changes broke this feature and tests where not running in certain framework.
        // This test class is test order dependent so do not rename the tests.
        // If new tests need to be added, follow the same naming pattern ConditionalAttribute{LetterToOrderTest} and then add a Validate{TestName}.

        private static bool s_conditionalFactTrueExecuted;
        private static bool s_conditionalFactFalseExecuted;
        private static int s_conditionalTheoryTrueCount;
        private static int s_conditionalTheoryFalseCount;

        public static bool AlwaysTrue => true;
        public static bool AlwaysFalse => false;

        [ConditionalFact(nameof(AlwaysTrue))]
        public void ConditionalAttributeTrue()
        {
            s_conditionalFactTrueExecuted = true;
        }

        [ConditionalFact(nameof(AlwaysFalse))]
        public void ConditionalAttributeFalse()
        {
            s_conditionalFactFalseExecuted = true;
        }

        [Fact]
        [OuterLoop("never outer loop", TestPlatforms.Any & ~TestPlatforms.Any)]
        public void NeverConditionalOuterLoopAttribute()
        {
            var method = System.Reflection.MethodBase.GetCurrentMethod();
            var res = TraitHelper.GetTraits(method);

            Assert.Empty(res);
        }

        [Fact]
        [OuterLoop("always outer loop", TestPlatforms.Any)]
        public void AlwaysConditionalOuterLoopAttribute()
        {
            var method = System.Reflection.MethodBase.GetCurrentMethod();
            var res = TraitHelper.GetTraits(method);

            Assert.Single(res);
            Assert.Equal("outerloop", res[0].Value);
        }

        [Fact]
        [OuterLoop("always outer loop")]
        public void AlwaysOuterLoopAttribute()
        {
            var method = System.Reflection.MethodBase.GetCurrentMethod();
            var res = TraitHelper.GetTraits(method);

            Assert.Single(res);
            Assert.Equal("outerloop", res[0].Value);
        }

        [ConditionalTheory(nameof(AlwaysTrue))]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
#pragma warning disable xUnit1026 // Theory methods should use all of their parameters
        public void ConditionalTheoryTrue(int _)
#pragma warning restore xUnit1026 // Theory methods should use all of their parameters
        {
            s_conditionalTheoryTrueCount++;
        }

        [ConditionalTheory(nameof(AlwaysFalse))]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
#pragma warning disable xUnit1026 // Theory methods should use all of their parameters
        public void ConditionalTheoryFalse(int _)
#pragma warning restore xUnit1026 // Theory methods should use all of their parameters
        {
            s_conditionalTheoryFalseCount++;
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
        public void ValidateConditionalTheoryFalse()
        {
            Assert.Equal(0, s_conditionalTheoryFalseCount);
        }
    }
}
