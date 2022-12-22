// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.LanguageServerClient.Razor.Extensions;
using Microsoft.VisualStudio.LanguageServerClient.Razor.HtmlCSharp;
using Microsoft.VisualStudio.Text;
using Moq;
using Xunit;
using Xunit.Abstractions;
using RazorMapToDocumentRangesResponse = Microsoft.VisualStudio.LanguageServerClient.Razor.HtmlCSharp.RazorMapToDocumentRangesResponse;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor;

public class RazorLSPSpanMappingServiceTest : TestBase
{
    private readonly Uri _mockDocumentUri = new("C://project/path/document.razor");

    private static readonly string s_mockGeneratedContent = $"Hello {Environment.NewLine} This is the source text in the generated C# file. {Environment.NewLine} This is some more sample text for demo purposes.";
    private static readonly string s_mockRazorContent = $"Hello {Environment.NewLine} This is the {Environment.NewLine} source text {Environment.NewLine} in the generated C# file. {Environment.NewLine} This is some more sample text for demo purposes.";

    private readonly SourceText _sourceTextGenerated;
    private readonly SourceText _sourceTextRazor;

    public RazorLSPSpanMappingServiceTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        _sourceTextGenerated = SourceText.From(s_mockGeneratedContent);
        _sourceTextRazor = SourceText.From(s_mockRazorContent);
    }

    [Fact]
    public async Task MapSpans_WithinRange_ReturnsMapping()
    {
        // Arrange
        var called = false;

        var textSpan = new TextSpan(1, 10);
        var spans = new TextSpan[] { textSpan };

        var documentSnapshot = new Mock<LSPDocumentSnapshot>(MockBehavior.Strict);
        documentSnapshot.SetupGet(doc => doc.Uri).Returns(_mockDocumentUri);

        var textSnapshot = new StringTextSnapshot(s_mockGeneratedContent, 1);

        var textSpanAsRange = textSpan.AsRange(_sourceTextGenerated);
        var mappedRange = new Range()
        {
            Start = new Position(2, 1),
            End = new Position(2, 11)
        };

        var documentMappingProvider = new Mock<LSPDocumentMappingProvider>(MockBehavior.Strict);
        var mappingResult = new RazorMapToDocumentRangesResponse()
        {
            Ranges = new Range[] { mappedRange }
        };
        documentMappingProvider.Setup(dmp => dmp.MapToDocumentRangesAsync(It.IsAny<RazorLanguageKind>(), It.IsAny<Uri>(), It.IsAny<Range[]>(), It.IsAny<CancellationToken>()))
            .Callback<RazorLanguageKind, Uri, Range[], CancellationToken>((languageKind, uri, ranges, ct) =>
            {
                Assert.Equal(RazorLanguageKind.CSharp, languageKind);
                Assert.Equal(_mockDocumentUri, uri);
                Assert.Single(ranges, textSpanAsRange);
                called = true;
            })
            .ReturnsAsync(mappingResult);

        var service = new RazorLSPSpanMappingService(documentMappingProvider.Object, documentSnapshot.Object, textSnapshot);

        var expectedSpan = mappedRange.AsTextSpan(_sourceTextRazor);
        var expectedLinePosition = _sourceTextRazor.Lines.GetLinePositionSpan(expectedSpan);
        var expectedFilePath = _mockDocumentUri.LocalPath;
        var expectedResult = (expectedFilePath, expectedLinePosition, expectedSpan);

        // Act
        var result = await service.MapSpansAsyncTest(spans, _sourceTextGenerated, _sourceTextRazor).ConfigureAwait(false);

        // Assert
        Assert.True(called);
        Assert.Single(result, expectedResult);
    }

    [Fact]
    public async Task MapSpans_OutsideRange_ReturnsEmpty()
    {
        // Arrange
        var called = false;

        var textSpan = new TextSpan(10, 10);
        var spans = new TextSpan[] { textSpan };

        var documentSnapshot = new Mock<LSPDocumentSnapshot>(MockBehavior.Strict);
        documentSnapshot.SetupGet(doc => doc.Uri).Returns(_mockDocumentUri);

        var textSnapshot = new StringTextSnapshot(s_mockGeneratedContent, 1);

        var textSpanAsRange = textSpan.AsRange(_sourceTextGenerated);

        var documentMappingProvider = new Mock<LSPDocumentMappingProvider>(MockBehavior.Strict);
        documentMappingProvider.Setup(dmp => dmp.MapToDocumentRangesAsync(It.IsAny<RazorLanguageKind>(), It.IsAny<Uri>(), It.IsAny<Range[]>(), It.IsAny<CancellationToken>()))
            .Callback<RazorLanguageKind, Uri, Range[], CancellationToken>((languageKind, uri, ranges, ct) =>
            {
                Assert.Equal(RazorLanguageKind.CSharp, languageKind);
                Assert.Equal(_mockDocumentUri, uri);
                Assert.Single(ranges, textSpanAsRange);
                called = true;
            })
            .ReturnsAsync(value: null);

        var service = new RazorLSPSpanMappingService(documentMappingProvider.Object, documentSnapshot.Object, textSnapshot);

        // Act
        var result = await service.MapSpansAsyncTest(spans, _sourceTextGenerated, _sourceTextRazor).ConfigureAwait(false);

        // Assert
        Assert.True(called);
        Assert.Empty(result);
    }

    [Fact]
    public void MapSpans_GetMappedSpanResults_MappingErrorReturnsDefaultMappedSpan()
    {
        // Arrange
        var sourceTextRazor = SourceText.From("");
        var response = new RazorMapToDocumentRangesResponse { Ranges = new Range[] { Extensions.RangeExtensions.UndefinedRange } };

        // Act
        var results = RazorLSPSpanMappingService.GetMappedSpanResults(_mockDocumentUri.LocalPath, sourceTextRazor, response);

        // Assert
        Assert.True(results.Single().IsDefault);
    }
}
