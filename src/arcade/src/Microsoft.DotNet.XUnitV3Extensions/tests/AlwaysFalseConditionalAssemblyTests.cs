// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Xunit;

// The assembly-level ConditionalAssembly attribute below references a condition member
// that always returns false. As a result, every test in this assembly is tagged with the
// "category=failing" trait, and the test runner is configured (via the project's
// TestRunnerAdditionalArguments) to skip tests with that trait. If the attribute were
// ever broken and stopped contributing the trait, the deliberately failing test below
// would run and fail the build, catching the regression.
[assembly: ConditionalAssembly(typeof(Microsoft.DotNet.XUnitV3Extensions.AlwaysFalseConditionalAssemblyTests.Conditions),
    nameof(Microsoft.DotNet.XUnitV3Extensions.AlwaysFalseConditionalAssemblyTests.Conditions.AlwaysFalse))]

namespace Microsoft.DotNet.XUnitV3Extensions.AlwaysFalseConditionalAssemblyTests
{
    public static class Conditions
    {
        public static bool AlwaysFalse => false;
    }

    public class FailingTests
    {
        [Fact]
        public void AlwaysFails()
        {
            Assert.Fail("This test is expected to be skipped via [assembly: ConditionalAssembly].");
        }
    }
}
