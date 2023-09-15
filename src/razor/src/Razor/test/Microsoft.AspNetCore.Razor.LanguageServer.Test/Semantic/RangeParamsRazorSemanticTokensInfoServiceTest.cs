// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.LanguageServer.Formatting;
using Microsoft.AspNetCore.Razor.Test.Common.Mef;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Semantic;

// Sets the FileName static variable.
// Finds the test method name using reflection, and uses
// that to find the expected input/output test files as Embedded resources.
[IntializeTestFile]
[UseExportProvider]
public class RangeParamsRazorSemanticTokensInfoServiceTest : RazorSemanticTokensInfoServiceTest
{
    public RangeParamsRazorSemanticTokensInfoServiceTest(ITestOutputHelper testOutput)
        : base(testOutput, usePreciseSemanticTokenRanges: false)
    {
    }
}
