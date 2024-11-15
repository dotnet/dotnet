// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.Editor;
using Microsoft.AspNetCore.Razor.Test.Common.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Protocol.DocumentMapping;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.Razor.LanguageClient.DocumentMapping;
using Microsoft.VisualStudio.Text;
using Moq;
using Xunit;
using Xunit.Abstractions;
using Range = Microsoft.VisualStudio.LanguageServer.Protocol.Range;

namespace Microsoft.VisualStudio.Razor.LanguageClient;

public class RazorLSPSpanMappingServiceTest(ITestOutputHelper testOutput) : ToolingTestBase(testOutput)
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

        var documentSnapshot = new Mock<LSPDocumentSnapshot>(MockBehavior.Strict);
        documentSnapshot.SetupGet(doc => doc.Uri).Returns(_mockDocumentUri);

        var textSnapshot = new StringTextSnapshot(s_mockGeneratedContent, 1);

        var textSpanAsRange = _sourceTextGenerated.GetRange(textSpan);
        var mappedRange = VsLspFactory.CreateSingleLineRange(2, character: 1, length: 10);

        var mappingResult = new RazorMapToDocumentRangesResponse()
        {
            Ranges = [mappedRange]
        };
        var requestInvoker = new TestLSPRequestInvoker([(LanguageServerConstants.RazorMapToDocumentRangesEndpoint, mappingResult)]);

        
        var lazyDocumentManager = new Lazy<LSPDocumentManager>(() =>
        {
            var documentManager = new TestDocumentManager();
            documentManager.AddDocument(_mockDocumentUri, new TestLSPDocumentSnapshot(_mockDocumentUri, 1));
            return documentManager;
        });

        var documentMappingProvider = new LSPDocumentMappingProvider(requestInvoker, lazyDocumentManager);

        var service = new RazorLSPSpanMappingService(documentMappingProvider, documentSnapshot.Object, textSnapshot);

        var expectedSpan = _sourceTextRazor.GetTextSpan(mappedRange);
        var expectedLinePosition = _sourceTextRazor.GetLinePositionSpan(expectedSpan);
        var expectedFilePath = _mockDocumentUri.LocalPath;
        var expectedResult = (expectedFilePath, expectedLinePosition, expectedSpan);

        // Act
        var result = await service.MapSpansAsyncTest(spans, _sourceTextGenerated, _sourceTextRazor);

        // Assert
        Assert.Single(result, expectedResult);
    }

    [Fact]
    public async Task MapSpans_OutsideRange_ReturnsEmpty()
    {
        // Arrange
        var textSpan = new TextSpan(10, 10);
        var spans = new TextSpan[] { textSpan };

        var documentSnapshot = new Mock<LSPDocumentSnapshot>(MockBehavior.Strict);
        documentSnapshot.SetupGet(doc => doc.Uri).Returns(_mockDocumentUri);

        var textSnapshot = new StringTextSnapshot(s_mockGeneratedContent, 1);

        var textSpanAsRange = _sourceTextGenerated.GetRange(textSpan);

        var requestInvoker = new StrictMock<LSPRequestInvoker>();
        var lazyDocumentManager = new Lazy<LSPDocumentManager>(() => new TestDocumentManager());

        var documentMappingProvider = new LSPDocumentMappingProvider(requestInvoker.Object, lazyDocumentManager);

        var service = new RazorLSPSpanMappingService(documentMappingProvider, documentSnapshot.Object, textSnapshot);

        // Act
        var result = await service.MapSpansAsyncTest(spans, _sourceTextGenerated, _sourceTextRazor);

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
        var results = RazorLSPSpanMappingService.GetMappedSpanResults(_mockDocumentUri.LocalPath, sourceTextRazor, response);

        // Assert
        Assert.True(results.Single().IsDefault);
    }
}
