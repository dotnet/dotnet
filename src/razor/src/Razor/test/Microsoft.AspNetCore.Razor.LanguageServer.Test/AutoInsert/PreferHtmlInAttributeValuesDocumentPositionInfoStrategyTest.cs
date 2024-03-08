// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.AutoInsert;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Razor.Workspaces.Protocol;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Test.AutoInsert;

public class PreferHtmlInAttributeValuesDocumentPositionInfoStrategyTest(ITestOutputHelper testOutput) : SingleServerDelegatingEndpointTestBase(testOutput)
{
    [Theory]
    [InlineData(
        """
        @page "/"
        @using Microsoft.AspNetCore.Components.Forms
        <InputText ValueChanged=$$></InputText>
        """,
        RazorLanguageKind.Html)]
    [InlineData(
        """
        @page "/"
        @using Microsoft.AspNetCore.Components.Forms
        <InputText ValueChanged="$$"></InputText>
        """,
        RazorLanguageKind.Razor)]
    [InlineData(
        """
        @page "/"
        @using Microsoft.AspNetCore.Components.Forms
        <InputText ValueChanged="@DateTime.$$"></InputText>
        """,
        RazorLanguageKind.CSharp)]
    internal async Task TryGetPositionInfoAsync_AtVariousPosition_ReturnsCorrectLanguage(string documentText, RazorLanguageKind expectedLanguage)
    {
        // Arrange
        TestFileMarkupParser.GetPosition(documentText, out documentText, out var cursorPosition);
        var razorFilePath = "file://path/test.razor";
        var codeDocument = CreateCodeDocument(documentText, filePath: razorFilePath);
        codeDocument.Source.Text.GetLineAndOffset(cursorPosition, out var line, out var offset);
        var position = new Position(line, offset);
        var uri = new Uri(razorFilePath);
        _ = await CreateLanguageServerAsync(codeDocument, razorFilePath);
        var documentContext = CreateDocumentContext(uri, codeDocument);

        // Act
        var result = await PreferHtmlInAttributeValuesDocumentPositionInfoStrategy.Instance.TryGetPositionInfoAsync(DocumentMappingService, documentContext, position, Logger, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedLanguage, result.LanguageKind);
        if (expectedLanguage != RazorLanguageKind.CSharp)
        {
            Assert.Equal(cursorPosition, result.HostDocumentIndex);
            Assert.Equal(position, result.Position);
        }
    }
}
