// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Razor.LanguageServer.Formatting;
using Microsoft.AspNetCore.Razor.Test.Common.Mef;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Semantic;

// Sets the FileName static variable.
// Finds the test method name using reflection, and uses
// that to find the expected input/output test files as Embedded resources.
[IntializeTestFile]
[UseExportProvider]
public class RangesParamsRazorSemanticTokensInfoServiceTest : RazorSemanticTokensInfoServiceTest
{
    public RangesParamsRazorSemanticTokensInfoServiceTest(ITestOutputHelper testOutput)
        : base(testOutput, usePreciseSemanticTokenRanges: true)
    {
    }

    [Fact]
    public void StitchSemanticTokenResponsesTogether_OnNullInput_ReturnsEmptyResponseData()
    {
        // Arrange
        int[][]? responseData = null;

        // Act
        var result = RazorSemanticTokensInfoService.StitchSemanticTokenResponsesTogether(responseData);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public void StitchSemanticTokenResponsesTogether_OnEmptyInput_ReturnsEmptyResponseData()
    {
        // Arrange
        var responseData = Array.Empty<int[]>();

        // Act
        var result = RazorSemanticTokensInfoService.StitchSemanticTokenResponsesTogether(responseData);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public void StitchSemanticTokenResponsesTogether_ReturnsCombinedResponseData()
    {
        // Arrange
        var responseData = new int[][] {
             new int[] { 0, 0, 0, 0, 0,
                         1, 0, 0, 0, 0,
                         1, 0, 0, 0, 0,
                         0, 5, 0, 0, 0,
                         0, 3, 0, 0, 0,
                         2, 2, 0, 0, 0,
                         0, 3, 0, 0, 0 },
             new int[] { 10, 0, 0, 0, 0,
                         1, 0, 0, 0, 0,
                         1, 0, 0, 0, 0,
                         0, 5, 0, 0, 0,
                         0, 3, 0, 0, 0,
                         2, 2, 0, 0, 0,
                         0, 3, 0, 0, 0 },
             new int[] { 14, 7, 0, 0, 0,
                         1, 0, 0, 0, 0,
                         1, 0, 0, 0, 0,
                         0, 5, 0, 0, 0,
                         0, 3, 0, 0, 0,
                         2, 2, 0, 0, 0,
                         0, 3, 0, 0, 0 },
         };

        var expectedResponseData = new int[] {
            0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 3, 0, 0, 0, 2, 2, 0, 0, 0, 0, 3, 0, 0, 0,
            6, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 3, 0, 0, 0, 2, 2, 0, 0, 0, 0, 3, 0, 0, 0,
            0, 2, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 3, 0, 0, 0, 2, 2, 0, 0, 0, 0, 3, 0, 0, 0
        };

        // Act
        var result = RazorSemanticTokensInfoService.StitchSemanticTokenResponsesTogether(responseData);

        // Assert
        Assert.Equal(expectedResponseData, result);
    }
}
