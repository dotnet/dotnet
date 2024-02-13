// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Semantic;

using Modifiers = RazorSemanticTokensLegendService.RazorTokenModifiers;

public class RazorSemanticTokensLegendTest(ITestOutputHelper testOutput) : ToolingTestBase(testOutput)
{
    [Fact]
    public void RazorModifiers_MustStartAfterRoslyn()
    {
        var razorModifiers = Enum.GetValues(typeof(Modifiers));

        var expected = Math.Pow(RazorSemanticTokensAccessor.GetTokenModifiers().Length, 2);

        foreach (Modifiers modifier in razorModifiers)
        {
            Assert.Equal(expected, (int)modifier);

            expected *= 2;
        }
    }
}
