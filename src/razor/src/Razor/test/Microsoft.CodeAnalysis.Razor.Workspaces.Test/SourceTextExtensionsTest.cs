// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System.Text;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis.Razor.Workspaces.Extensions;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.CodeAnalysis.Text;

public class SourceTextExtensionsTest : TestBase
{
    private readonly SourceText _sourceText;

    public SourceTextExtensionsTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        _sourceText = SourceText.From(@"
@addTagHelper *, SomeApplication

<p>The current time is @GetTheTime()</p>

@functions {
    public DateTime GetTheTime()
    {
        return DateTime.Now;
    }
}");
    }

    [Fact]
    public void GetRazorSourceDocument_BuildsSourceDocumentWithCorrectProperties()
    {
        // Arrange
        var sourceDocumentProperties = RazorSourceDocumentProperties.Default;
        var stringSourceDocument = new StringSourceDocument(_sourceText.ToString(), Encoding.UTF8, sourceDocumentProperties);

        // Act
        var sourceTextSourceDocument = _sourceText.GetRazorSourceDocument(sourceDocumentProperties.FilePath, sourceDocumentProperties.RelativePath);

        // Assert
        Assert.Equal(stringSourceDocument.Encoding, sourceTextSourceDocument.Encoding);
        Assert.Equal(stringSourceDocument.FilePath, sourceTextSourceDocument.FilePath);
        Assert.Equal(stringSourceDocument.RelativePath, sourceTextSourceDocument.RelativePath);
        Assert.Equal(stringSourceDocument.Length, sourceTextSourceDocument.Length);
        for (var i = 0; i < stringSourceDocument.Length; i++)
        {
            Assert.Equal(stringSourceDocument[i], sourceTextSourceDocument[i]);
        }
    }

    [Fact]
    public void SourceTextSourceDocument_CopyTo_WorksAsExpected()
    {
        // Arrange
        var sourceDocumentProperties = RazorSourceDocumentProperties.Default;
        var stringSourceDocument = new StringSourceDocument(_sourceText.ToString(), Encoding.UTF8, sourceDocumentProperties);
        var stringDocumentBuffer = new char[stringSourceDocument.Length];
        stringSourceDocument.CopyTo(0, stringDocumentBuffer, 0, stringDocumentBuffer.Length);
        var sourceTextSourceDocument = _sourceText.GetRazorSourceDocument(sourceDocumentProperties.FilePath, sourceDocumentProperties.RelativePath);
        var sourceTextDocumentBuffer = new char[sourceTextSourceDocument.Length];

        // Act
        sourceTextSourceDocument.CopyTo(0, sourceTextDocumentBuffer, 0, sourceTextDocumentBuffer.Length);

        // Assert
        Assert.Equal(stringDocumentBuffer, sourceTextDocumentBuffer);
    }

    [Fact]
    public void SourceTextSourceDocument_GetChecksum_WorksAsExpected()
    {
        // Arrange
        var sourceDocumentProperties = RazorSourceDocumentProperties.Default;
        var stringSourceDocument = new StringSourceDocument(_sourceText.ToString(), Encoding.UTF8, sourceDocumentProperties);
        var stringSourceDocumentChecksum = stringSourceDocument.GetChecksum();
        var sourceTextSourceDocument = _sourceText.GetRazorSourceDocument(sourceDocumentProperties.FilePath, sourceDocumentProperties.RelativePath);

        // Act
        var sourceTextSourceDocumentChecksum = sourceTextSourceDocument.GetChecksum();

        // Assert
        Assert.Equal(stringSourceDocumentChecksum, sourceTextSourceDocumentChecksum);
    }

    [Fact]
    public void RazorTextLineCollection_GetLineLength_WorksAsExpected()
    {
        // Arrange
        var sourceDocumentProperties = RazorSourceDocumentProperties.Default;
        var stringSourceDocument = new StringSourceDocument(_sourceText.ToString(), Encoding.UTF8, sourceDocumentProperties);
        var sourceTextSourceDocument = _sourceText.GetRazorSourceDocument(sourceDocumentProperties.FilePath, sourceDocumentProperties.RelativePath);
        var originalLineCollection = stringSourceDocument.Lines;
        var newLineCollection = sourceTextSourceDocument.Lines;

        // Act & Assert
        Assert.Equal(originalLineCollection.Count, newLineCollection.Count);
        for (var i = 0; i < originalLineCollection.Count; i++)
        {
            var originalLineLength = originalLineCollection.GetLineLength(i);
            var newLineLength = newLineCollection.GetLineLength(i);

            Assert.Equal(originalLineLength, newLineLength);
        }
    }

    [Fact]
    public void RazorTextLineCollection_GetLocation_WorksAsExpected()
    {
        // Arrange
        var sourceDocumentProperties = RazorSourceDocumentProperties.Default;
        var stringSourceDocument = new StringSourceDocument(_sourceText.ToString(), Encoding.UTF8, sourceDocumentProperties);
        var sourceTextSourceDocument = _sourceText.GetRazorSourceDocument(sourceDocumentProperties.FilePath, sourceDocumentProperties.RelativePath);
        var originalLineCollection = stringSourceDocument.Lines;
        var newLineCollection = sourceTextSourceDocument.Lines;

        // Act & Assert
        for (var i = 0; i < _sourceText.Length; i++)
        {
            var originalLocation = originalLineCollection.GetLocation(i);
            var newLocation = newLineCollection.GetLocation(i);

            Assert.Equal(originalLocation, newLocation);
        }
    }
}
