// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.VisualStudio.LegacyEditor.Razor.Parsing;
using Microsoft.VisualStudio.Utilities;
using Xunit;
using Xunit.Abstractions;
using static Microsoft.AspNetCore.Razor.Test.Common.VisualStudio.VsMocks;

namespace Microsoft.VisualStudio.LegacyEditor.Razor;

public class TextBufferExtensionsTest(ITestOutputHelper testOutput) : ToolingTestBase(testOutput)
{
    [Fact]
    public void TryGetCodeDocument_SucceedsIfParserHasCodeDocument()
    {
        // Arrange
        var expectedCodeDocument = TestRazorCodeDocument.Create("Hello World");
        var parser = StrictMock.Of<IVisualStudioRazorParser>(p =>
            p.CodeDocument == expectedCodeDocument);
        var properties = new PropertyCollection()
        {
            [typeof(IVisualStudioRazorParser)] = parser
        };

        var textBuffer = CreateTextBuffer(ContentTypes.RazorCore, properties);

        // Act
        var result = textBuffer.TryGetCodeDocument(out var codeDocument);

        // Assert
        Assert.True(result);
        Assert.Same(expectedCodeDocument, codeDocument);
    }

    [Fact]
    public void TryGetCodeDocument_FailsIfParserMissingCodeDocument()
    {
        // Arrange
        var parser = StrictMock.Of<IVisualStudioRazorParser>(p =>
            p.CodeDocument == null);
        var properties = new PropertyCollection()
        {
            [typeof(IVisualStudioRazorParser)] = parser
        };
        var textBuffer = CreateTextBuffer(ContentTypes.RazorCore, properties);

        // Act
        var result = textBuffer.TryGetCodeDocument(out var codeDocument);

        // Assert
        Assert.False(result);
        Assert.Null(codeDocument);
    }

    [Fact]
    public void TryGetCodeDocument_FailsIfNoParserIsAvailable()
    {
        // Arrange
        var textBuffer = CreateTextBuffer(ContentTypes.RazorCore);

        // Act
        var result = textBuffer.TryGetCodeDocument(out var codeDocument);

        // Assert
        Assert.False(result);
        Assert.Null(codeDocument);
    }
}
