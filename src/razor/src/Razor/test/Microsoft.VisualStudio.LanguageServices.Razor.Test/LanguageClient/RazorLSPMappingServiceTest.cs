// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.Editor;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Protocol.DocumentMapping;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.Razor.LanguageClient.DocumentMapping;
using Xunit;
using Xunit.Abstractions;
using Range = Microsoft.VisualStudio.LanguageServer.Protocol.Range;

namespace Microsoft.VisualStudio.Razor.LanguageClient;

public class RazorLSPMappingServiceTest(ITestOutputHelper testOutput) : ToolingTestBase(testOutput)
{
    private readonly Uri _mockDocumentUri = new("C://project/path/document.razor");

    const string s_mockGeneratedContent = """
            Hello
             This is the source text in the generated C# file.
             This is some more sample text for demo purposes.
            """;

    const string s_mockRazorContent = """
            Hello
             This is the
             source text
             in the generated C# file.
             This is some more sample text for demo purposes.
            """;

    private static readonly SourceText _sourceTextGenerated = SourceText.From(s_mockGeneratedContent);
    private static readonly SourceText _sourceTextRazor = SourceText.From(s_mockRazorContent);

    [Fact]
    public async Task MapSpans_WithinRange_ReturnsMapping()
    {
        // Arrange
        var textSpan = new TextSpan(1, 10);
        var spans = new TextSpan[] { textSpan };

        var documentSnapshot = new StrictMock<LSPDocumentSnapshot>();
        documentSnapshot.SetupGet(doc => doc.Uri).Returns(_mockDocumentUri);
        documentSnapshot.SetupGet(doc => doc.Snapshot).Returns(new StringTextSnapshot(_sourceTextRazor.ToString()));

        var textSnapshot = new StringTextSnapshot(s_mockGeneratedContent, 1);

        var textSpanAsRange = _sourceTextGenerated.GetRange(textSpan);
        var mappedRange = VsLspFactory.CreateSingleLineRange(2, character: 1, length: 10);

        var mappingResult = new RazorMapToDocumentRangesResponse()
        {
            Ranges = [mappedRange]
        };
        var requestInvoker = new TestLSPRequestInvoker(new List<(string, object)>()
        {
            (LanguageServerConstants.RazorMapToDocumentRangesEndpoint, mappingResult)
        });

        var lazyManager = new Lazy<LSPDocumentManager>(() =>
        {
            var manager = new TestDocumentManager();
            manager.AddDocument(_mockDocumentUri, documentSnapshot.Object);

            return manager;
        });

        var documentMappingProvider = new LSPDocumentMappingProvider(requestInvoker, lazyManager);

        var service = new RazorLSPMappingService(documentMappingProvider, documentSnapshot.Object, textSnapshot);

        var expectedSpan = _sourceTextRazor.GetTextSpan(mappedRange);
        var expectedLinePosition = _sourceTextRazor.GetLinePositionSpan(expectedSpan);
        var expectedFilePath = _mockDocumentUri.LocalPath;
        var expectedResult = (expectedFilePath, expectedLinePosition, expectedSpan);

        // Act
        var result = await service.GetTestAccessor().MapSpansAsync(spans, _sourceTextGenerated, _sourceTextRazor, DisposalToken);

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
        documentSnapshot.SetupGet(doc => doc.Snapshot).Returns(new StringTextSnapshot(_sourceTextRazor.ToString()));

        var textSnapshot = new StringTextSnapshot(s_mockGeneratedContent, 1);

        var textSpanAsRange = _sourceTextGenerated.GetRange(textSpan);

        var requestInvoker = new TestLSPRequestInvoker(new List<(string, object?)>()
        {
            (LanguageServerConstants.RazorMapToDocumentRangesEndpoint, null)
        });

        var lazyManager = new Lazy<LSPDocumentManager>(() =>
        {
            var manager = new TestDocumentManager();
            manager.AddDocument(_mockDocumentUri, documentSnapshot.Object);

            return manager;
        });

        var documentMappingProvider = new LSPDocumentMappingProvider(requestInvoker, lazyManager);

        var service = new RazorLSPMappingService(documentMappingProvider, documentSnapshot.Object, textSnapshot);

        // Act
        var result = await service.GetTestAccessor().MapSpansAsync(spans, _sourceTextGenerated, _sourceTextRazor, DisposalToken);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void MapSpans_GetMappedSpanResults_MappingErrorReturnsDefaultMappedSpan()
    {
        // Arrange
        var sourceTextRazor = SourceText.From("");
        var response = new RazorMapToDocumentRangesResponse { Ranges = new Range[] { VsLspFactory.UndefinedRange } };

        // Act
        var results = RazorLSPMappingService.GetMappedSpanResults(_mockDocumentUri.LocalPath, sourceTextRazor, response);

        // Assert
        Assert.Collection(results,
            static mappingResult => Assert.True(mappingResult.IsDefault));
    }
}
