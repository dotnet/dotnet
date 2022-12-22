// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Common.Extensions;
using Microsoft.AspNetCore.Razor.Test.Common;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

public class RazorCodeDocumentExtensionsTest : TestBase
{
    public RazorCodeDocumentExtensionsTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
    }

    [Fact]
    public void IsUnsupported_Unset_ReturnsFalse()
    {
        // Arrange
        var codeDocument = TestRazorCodeDocument.CreateEmpty();

        // Act
        var result = codeDocument.IsUnsupported();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsUnsupported_Set_ReturnsTrue()
    {
        // Arrange
        var codeDocument = TestRazorCodeDocument.CreateEmpty();
        codeDocument.SetUnsupported();

        // Act
        var result = codeDocument.IsUnsupported();

        // Assert
        Assert.True(result);
    }
}
