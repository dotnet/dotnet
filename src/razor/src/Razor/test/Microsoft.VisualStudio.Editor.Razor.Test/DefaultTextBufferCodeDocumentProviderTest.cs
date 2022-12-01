// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Utilities;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.Editor.Razor;

public class DefaultTextBufferCodeDocumentProviderTest : TestBase
{
    public DefaultTextBufferCodeDocumentProviderTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
    }

    [Fact]
    public void TryGetFromBuffer_SucceedsIfParserHasCodeDocument()
    {
        // Arrange
        var expectedCodeDocument = TestRazorCodeDocument.Create("Hello World");
#pragma warning disable CS0618 // Type or member is obsolete
        VisualStudioRazorParser parser = new DefaultVisualStudioRazorParser(expectedCodeDocument);
#pragma warning restore CS0618 // Type or member is obsolete
        var properties = new PropertyCollection()
        {
            [typeof(VisualStudioRazorParser)] = parser
        };
        var textBuffer = Mock.Of<ITextBuffer>(buffer => buffer.Properties == properties, MockBehavior.Strict);
        var provider = new DefaultTextBufferCodeDocumentProvider();

        // Act
        var result = provider.TryGetFromBuffer(textBuffer, out var codeDocument);

        // Assert
        Assert.True(result);
        Assert.Same(expectedCodeDocument, codeDocument);
    }

    [Fact]
    public void TryGetFromBuffer_FailsIfParserMissingCodeDocument()
    {
        // Arrange
#pragma warning disable CS0618 // Type or member is obsolete
        VisualStudioRazorParser parser = new DefaultVisualStudioRazorParser(codeDocument: null);
#pragma warning restore CS0618 // Type or member is obsolete
        var properties = new PropertyCollection()
        {
            [typeof(VisualStudioRazorParser)] = parser
        };
        var textBuffer = Mock.Of<ITextBuffer>(buffer => buffer.Properties == properties, MockBehavior.Strict);
        var provider = new DefaultTextBufferCodeDocumentProvider();

        // Act
        var result = provider.TryGetFromBuffer(textBuffer, out var codeDocument);

        // Assert
        Assert.False(result);
        Assert.Null(codeDocument);
    }

    [Fact]
    public void TryGetFromBuffer_FailsIfNoParserIsAvailable()
    {
        // Arrange
        var textBuffer = Mock.Of<ITextBuffer>(buffer => buffer.Properties == new PropertyCollection(), MockBehavior.Strict);
        var provider = new DefaultTextBufferCodeDocumentProvider();

        // Act
        var result = provider.TryGetFromBuffer(textBuffer, out var codeDocument);

        // Assert
        Assert.False(result);
        Assert.Null(codeDocument);
    }
}
