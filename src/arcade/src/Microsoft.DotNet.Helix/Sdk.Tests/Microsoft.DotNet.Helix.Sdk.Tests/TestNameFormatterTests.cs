// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.DotNet.Helix.AzureDevOpsTestPublisher;
using Xunit;

namespace Microsoft.DotNet.Helix.Sdk.Tests
{
    public class TestNameFormatterTests
    {
        [Fact]
        public void DisplayName_IsMethodName_ReturnsFullyQualifiedName()
        {
            // MSTest default: display name is just the method name.
            string result = TestNameFormatter.FormatDisplayName("Ns.MyTests.MyMethod", "MyMethod");
            Assert.Equal("Ns.MyTests.MyMethod", result);
        }

        [Fact]
        public void DisplayName_EqualsFullyQualifiedName_ReturnsFullyQualifiedName()
        {
            // xUnit default: display name is already the fully qualified name.
            string result = TestNameFormatter.FormatDisplayName("Ns.MyTests.MyMethod", "Ns.MyTests.MyMethod");
            Assert.Equal("Ns.MyTests.MyMethod", result);
        }

        [Fact]
        public void ParameterizedRow_WithSpaceBeforeArgs_QualifiesWithoutDuplicatingMethod()
        {
            // The scenario from dotnet/sdk#55123: FQN does not end with the display name because of the args part.
            string result = TestNameFormatter.FormatDisplayName(
                "Ns.NativeAotTests.NativeAotTests_WillRunWithExitCodeZero",
                "NativeAotTests_WillRunWithExitCodeZero (\"net10.0\")");

            Assert.Equal("Ns.NativeAotTests.NativeAotTests_WillRunWithExitCodeZero (\"net10.0\")", result);
        }

        [Fact]
        public void ParameterizedRow_WithoutSpaceBeforeArgs_QualifiesWithoutDuplicatingMethod()
        {
            string result = TestNameFormatter.FormatDisplayName("Ns.MyTests.Theory", "Theory(value: 1)");
            Assert.Equal("Ns.MyTests.Theory (value: 1)", result);
        }

        [Fact]
        public void CustomDisplayName_KeepsBothFullyQualifiedNameAndDisplayName()
        {
            // xUnit [Fact(DisplayName = "...")] with an arbitrary, non-unique name.
            string result = TestNameFormatter.FormatDisplayName("Ns.MyTests.MyMethod", "My friendly scenario");
            Assert.Equal("Ns.MyTests.MyMethod (My friendly scenario)", result);
        }

        [Fact]
        public void CustomDisplayName_WithParentheses_KeepsWholeDisplayName()
        {
            string result = TestNameFormatter.FormatDisplayName("Ns.MyTests.MyMethod", "Scenario (special case)");
            Assert.Equal("Ns.MyTests.MyMethod (Scenario (special case))", result);
        }

        [Fact]
        public void EmptyFullyQualifiedName_FallsBackToDisplayName()
        {
            string result = TestNameFormatter.FormatDisplayName("", "MyMethod");
            Assert.Equal("MyMethod", result);
        }
    }
}
