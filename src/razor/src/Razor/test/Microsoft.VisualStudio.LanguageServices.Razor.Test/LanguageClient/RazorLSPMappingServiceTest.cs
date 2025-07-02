// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.Editor;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Protocol.DocumentMapping;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.Razor.LanguageClient.DocumentMapping;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.Razor.LanguageClient;

public class RazorLSPMappingServiceTest(ITestOutputHelper testOutput) : ToolingTestBase(testOutput)
{
    private readonly Uri _mockDocumentUri = new("C://project/path/document.razor");

    private const string MockGeneratedContent = """
        Hello
        This is the source text in the generated C# file.
        This is some more sample text for demo purposes.
        """;

    private const string MockRazorContent = """
        Hello
        This is the
        source text
        in the generated C# file.
        This is some more sample text for demo purposes.
        """;

    private static readonly SourceText s_sourceTextGenerated = SourceText.From(MockGeneratedContent);
    private static readonly SourceText s_sourceTextRazor = SourceText.From(MockRazorContent);

    [Fact]
    public async Task MapSpans_WithinRange_ReturnsMapping()
    {
        // Arrange
        var textSpan = new TextSpan(1, 10);
        var spans = new TextSpan[] { textSpan };

        var documentSnapshot = new StrictMock<LSPDocumentSnapshot>();
        documentSnapshot.SetupGet(doc => doc.Uri).Returns(_mockDocumentUri);
        documentSnapshot.SetupGet(doc => doc.Snapshot).Returns(new StringTextSnapshot(s_sourceTextRazor.ToString()));

        var textSnapshot = new StringTextSnapshot(MockGeneratedContent, 1);

        var textSpanAsRange = s_sourceTextGenerated.GetRange(textSpan);
        var mappedRange = LspFactory.CreateSingleLineRange(2, character: 1, length: 10);

        var mappingResult = new RazorMapToDocumentRangesResponse()
        {
            Ranges = [mappedRange],
            Spans = [textSpan.ToRazorTextSpan()]
        };
        var requestInvoker = new TestLSPRequestInvoker(
        [
            (LanguageServerConstants.RazorMapToDocumentRangesEndpoint, mappingResult)
        ]);

        var lazyManager = new Lazy<LSPDocumentManager>(() =>
        {
            var manager = new TestDocumentManager();
            manager.AddDocument(_mockDocumentUri, documentSnapshot.Object);

            return manager;
        });

        var documentMappingProvider = new LSPDocumentMappingProvider(requestInvoker, lazyManager);

        var service = new RazorLSPMappingService(documentMappingProvider, documentSnapshot.Object, textSnapshot);

        var expectedSpan = s_sourceTextRazor.GetTextSpan(mappedRange);
        var expectedLinePosition = s_sourceTextRazor.GetLinePositionSpan(expectedSpan);
        var expectedFilePath = _mockDocumentUri.LocalPath;
        var expectedResult = (expectedFilePath, expectedLinePosition, expectedSpan);

        // Act
        var result = await service.GetTestAccessor().MapSpansAsync(spans, s_sourceTextGenerated, s_sourceTextRazor, DisposalToken);

        // Assert
        Assert.Single(result, expectedResult);
    }

    [Fact]
    public async Task MapSpans_OutsideRange_ReturnsEmpty()
    {
        // Arrange
        var textSpan = new TextSpan(10, 10);
        var spans = new TextSpan[] { textSpan };

        var documentSnapshot = new StrictMock<LSPDocumentSnapshot>();
        documentSnapshot.SetupGet(doc => doc.Uri).Returns(_mockDocumentUri);
        documentSnapshot.SetupGet(doc => doc.Snapshot).Returns(new StringTextSnapshot(s_sourceTextRazor.ToString()));

        var textSnapshot = new StringTextSnapshot(MockGeneratedContent, 1);

        var textSpanAsRange = s_sourceTextGenerated.GetRange(textSpan);

        var requestInvoker = new TestLSPRequestInvoker(
        [
            (LanguageServerConstants.RazorMapToDocumentRangesEndpoint, null)
        ]);

        var lazyManager = new Lazy<LSPDocumentManager>(() =>
        {
            var manager = new TestDocumentManager();
            manager.AddDocument(_mockDocumentUri, documentSnapshot.Object);

            return manager;
        });

        var documentMappingProvider = new LSPDocumentMappingProvider(requestInvoker, lazyManager);

        var service = new RazorLSPMappingService(documentMappingProvider, documentSnapshot.Object, textSnapshot);

        // Act
        var result = await service.GetTestAccessor().MapSpansAsync(spans, s_sourceTextGenerated, s_sourceTextRazor, DisposalToken);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void MapSpans_GetMappedSpanResults_MappingErrorReturnsDefaultMappedSpan()
    {
        // Arrange
        var sourceTextRazor = SourceText.From("");
        var response = new RazorMapToDocumentRangesResponse { Ranges = [LspFactory.UndefinedRange], Spans = Array.Empty<RazorTextSpan>() };

        // Act
        var results = RazorLSPMappingService.GetMappedSpanResults(_mockDocumentUri.LocalPath, sourceTextRazor, response);

        // Assert
        Assert.Collection(results,
            static mappingResult => Assert.True(mappingResult.IsDefault));
    }
}
