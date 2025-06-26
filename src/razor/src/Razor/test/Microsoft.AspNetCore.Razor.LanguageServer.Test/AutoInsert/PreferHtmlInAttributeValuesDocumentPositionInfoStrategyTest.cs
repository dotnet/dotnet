// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Text;
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
        var position = codeDocument.Source.Text.GetPosition(cursorPosition);
        await using var _ = await CreateLanguageServerAsync(codeDocument, razorFilePath);

        // Act
        var result = PreferHtmlInAttributeValuesDocumentPositionInfoStrategy.Instance.GetPositionInfo(DocumentMappingService, codeDocument, cursorPosition);

        // Assert
        Assert.NotEqual(default, result);
        Assert.Equal(expectedLanguage, result.LanguageKind);

        if (expectedLanguage != RazorLanguageKind.CSharp)
        {
            Assert.Equal(cursorPosition, result.HostDocumentIndex);
            Assert.Equal(position, result.Position);
        }
    }
}
