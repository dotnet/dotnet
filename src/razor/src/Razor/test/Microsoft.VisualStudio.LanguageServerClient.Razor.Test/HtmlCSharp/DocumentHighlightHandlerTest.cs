// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.AspNetCore.Razor.LanguageServer.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.Mef;
using Microsoft.CodeAnalysis.Razor.Workspaces.Extensions;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.LanguageServerClient.Razor.Test;
using Microsoft.VisualStudio.Text;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor.HtmlCSharp;

[UseExportProvider]
public class DocumentHighlightHandlerTest : HandlerTestBase
{
    private readonly Uri _uri;
    private readonly LSPDocumentSnapshot _documentSnapshot;
    private readonly TestDocumentManager _documentManager;
    private readonly ServerCapabilities _documentHighlightServerCapabilities;

    public DocumentHighlightHandlerTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        _uri = new Uri("C:/path/to/file.razor");
        var htmlVirtualDocument = new HtmlVirtualDocumentSnapshot(
            new Uri("C:/path/to/file.razor__virtual.html"),
            new StringTextSnapshot(string.Empty),
            hostDocumentSyncVersion: 0);
        _documentSnapshot = new TestLSPDocumentSnapshot(_uri, version: 0, "Some Content", htmlVirtualDocument);
        _documentManager = new TestDocumentManager();
        _documentManager.AddDocument(_uri, _documentSnapshot);

        _documentHighlightServerCapabilities = new()
        {
            DocumentHighlightProvider = true
        };
    }

    [Fact]
    public async Task HandleRequestAsync_DocumentNotFound_ReturnsNull()
    {
        // Arrange
        var requestInvoker = new TestLSPRequestInvoker();
        var projectionProvider = new TestLSPProjectionProvider(LoggerFactory);
        var documentMappingProvider = new TestLSPDocumentMappingProvider(LoggerFactory);
        var highlightHandler = new DocumentHighlightHandler(requestInvoker, new TestDocumentManager(), projectionProvider, documentMappingProvider, LoggerProvider);
        var highlightRequest = new DocumentHighlightParams()
        {
            TextDocument = new TextDocumentIdentifier() { Uri = _uri },
            Position = new Position(0, 1)
        };

        // Act
        var result = await highlightHandler.HandleRequestAsync(highlightRequest, new ClientCapabilities(), DisposalToken);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task HandleRequestAsync_ProjectionNotFound_ReturnsNull()
    {
        // Arrange
        var requestInvoker = new TestLSPRequestInvoker();
        var projectionProvider = new TestLSPProjectionProvider(LoggerFactory);
        var documentMappingProvider = new TestLSPDocumentMappingProvider(LoggerFactory);
        var highlightHandler = new DocumentHighlightHandler(requestInvoker, _documentManager, projectionProvider, documentMappingProvider, LoggerProvider);
        var highlightRequest = new DocumentHighlightParams()
        {
            TextDocument = new TextDocumentIdentifier() { Uri = _uri },
            Position = new Position(0, 1)
        };

        // Act
        var result = await highlightHandler.HandleRequestAsync(highlightRequest, new ClientCapabilities(), DisposalToken);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task HandleRequestAsync_CSharpProjection_RemapsHighlightRange()
    {
        // Arrange
        var text =
            """
                @code
                {
                    void Method()
                    {
                        Method();
                    }
                }
                """;

        var cursorPosition = new Position(2, 9);
        var documentUri = new Uri("C:/path/to/file.razor");
        var csharpDocumentUri = new Uri("C:/path/to/file.razor__virtual.cs");
        var codeDocument = CreateCodeDocument(text, documentUri.AbsolutePath);
        var razorSourceText = codeDocument.GetSourceText();
        var csharpSourceText = codeDocument.GetCSharpSourceText();

        var csharpDocumentSnapshot = CreateCSharpVirtualDocumentSnapshot(codeDocument, csharpDocumentUri.AbsoluteUri);
        var documentSnapshot = new TestLSPDocumentSnapshot(
            documentUri,
            version: 1,
            snapshotContent: razorSourceText.ToString(),
            csharpDocumentSnapshot);

        var uriToCodeDocumentMap = new Dictionary<Uri, (int hostDocumentVersion, RazorCodeDocument codeDocument)>
        {
            { documentUri, (hostDocumentVersion: 1, codeDocument) }
        };

        var mappingProvider = new TestLSPDocumentMappingProvider(uriToCodeDocumentMap, LoggerFactory);
        var razorSpanMappingService = new TestRazorLSPSpanMappingService(
            mappingProvider, documentUri, razorSourceText, csharpSourceText, DisposalToken);

        await using var csharpServer = await CSharpTestLspServerHelpers.CreateCSharpLspServerAsync(
            csharpSourceText, csharpDocumentUri, _documentHighlightServerCapabilities, razorSpanMappingService, DisposalToken);

        var requestInvoker = new TestLSPRequestInvoker(csharpServer);
        var documentManager = new TestDocumentManager();
        documentManager.AddDocument(documentUri, documentSnapshot);
        var projectionProvider = new TestLSPProjectionProvider(LoggerFactory);

        var highlightHandler = new DocumentHighlightHandler(requestInvoker, documentManager, projectionProvider, mappingProvider, LoggerProvider);
        var highlightRequest = new DocumentHighlightParams()
        {
            TextDocument = new TextDocumentIdentifier { Uri = documentUri },
            Position = cursorPosition
        };

        var firstExpectedRange = new Range { Start = new Position { Line = 2, Character = 9 }, End = new Position { Line = 2, Character = 15 } };
        var secondExpectedRange = new Range { Start = new Position { Line = 4, Character = 8 }, End = new Position { Line = 4, Character = 14 } };

        // Act
        var result = await highlightHandler.HandleRequestAsync(highlightRequest, new ClientCapabilities(), DisposalToken);

        // Assert
        Assert.Equal(2, result.Length);

        var actualRanges = result.Select(r => r.Range);
        Assert.Contains(firstExpectedRange, actualRanges);
        Assert.Contains(secondExpectedRange, actualRanges);
    }

    [Fact]
    public async Task HandleRequestAsync_HtmlProjection_RemapsHighlightRange()
    {
        // Arrange
        var called = false;
        var expectedHighlight = GetHighlight(5, 5, 5, 5);

        var htmlHighlight = GetHighlight(100, 100, 100, 100);
        var requestInvoker = GetMockRequestInvoker<DocumentHighlightParams, DocumentHighlight[]>(
            new[] { htmlHighlight },
            (textBuffer, method, clientName, highlightParams, ct) =>
            {
                Assert.Equal(Methods.TextDocumentDocumentHighlightName, method);
                Assert.Equal(RazorLSPConstants.HtmlLanguageServerName, clientName);
                called = true;
            });

        var projectionResult = new ProjectionResult()
        {
            Uri = null,
            Position = null,
            LanguageKind = RazorLanguageKind.Html,
        };
        var projectionProvider = GetMockProjectionProvider(projectionResult);

        var documentMappingProvider = GetMockDocumentMappingProvider(expectedHighlight.Range, 0, RazorLanguageKind.Html);

        var highlightHandler = new DocumentHighlightHandler(requestInvoker, _documentManager, projectionProvider, documentMappingProvider, LoggerProvider);
        var highlightRequest = new DocumentHighlightParams()
        {
            TextDocument = new TextDocumentIdentifier() { Uri = _uri },
            Position = new Position(10, 5)
        };

        // Act
        var result = await highlightHandler.HandleRequestAsync(highlightRequest, new ClientCapabilities(), DisposalToken);

        // Assert
        Assert.True(called);
        var actualHighlight = Assert.Single(result);
        Assert.Equal(expectedHighlight.Range, actualHighlight.Range);
    }

    [Fact]
    public async Task HandleRequestAsync_VersionMismatch_DiscardsLocation()
    {
        // Arrange
        var text =
            """
                @code
                {
                    void Method()
                    {
                        Method();
                    }
                }
                """;

        var cursorPosition = new Position(2, 9);
        var documentUri = new Uri("C:/path/to/file.razor");
        var csharpDocumentUri = new Uri("C:/path/to/file.razor__virtual.cs");
        var codeDocument = CreateCodeDocument(text, documentUri.AbsolutePath);
        var razorSourceText = codeDocument.GetSourceText();
        var csharpSourceText = codeDocument.GetCSharpSourceText();

        var csharpDocumentSnapshot = CreateCSharpVirtualDocumentSnapshot(codeDocument, csharpDocumentUri.AbsoluteUri);
        var documentSnapshot = new TestLSPDocumentSnapshot(
            documentUri,
            version: 1,
            snapshotContent: razorSourceText.ToString(),
            csharpDocumentSnapshot);

        var uriToCodeDocumentMap = new Dictionary<Uri, (int hostDocumentVersion, RazorCodeDocument codeDocument)>
        {
            { documentUri, (hostDocumentVersion: 2, codeDocument) }
        };

        var mappingProvider = new TestLSPDocumentMappingProvider(uriToCodeDocumentMap, LoggerFactory);
        var razorSpanMappingService = new TestRazorLSPSpanMappingService(
            mappingProvider, documentUri, razorSourceText, csharpSourceText, DisposalToken);

        await using var csharpServer = await CSharpTestLspServerHelpers.CreateCSharpLspServerAsync(
            csharpSourceText, csharpDocumentUri, _documentHighlightServerCapabilities, razorSpanMappingService, DisposalToken);

        var requestInvoker = new TestLSPRequestInvoker(csharpServer);
        var documentManager = new TestDocumentManager();
        documentManager.AddDocument(documentUri, documentSnapshot);
        var projectionProvider = new TestLSPProjectionProvider(LoggerFactory);

        var highlightHandler = new DocumentHighlightHandler(requestInvoker, documentManager, projectionProvider, mappingProvider, LoggerProvider);
        var highlightRequest = new DocumentHighlightParams()
        {
            TextDocument = new TextDocumentIdentifier { Uri = documentUri },
            Position = cursorPosition
        };

        // Act
        var result = await highlightHandler.HandleRequestAsync(highlightRequest, new ClientCapabilities(), DisposalToken);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task HandleRequestAsync_RemapFailure_DiscardsLocation()
    {
        // Arrange
        var text =
            """
                @code
                {
                    void Method()
                    {
                        Method();
                    }
                }
                """;

        var cursorPosition = new Position(2, 9);
        var documentUri = new Uri("C:/path/to/file.razor");
        var csharpDocumentUri = new Uri("C:/path/to/file.razor__virtual.cs");
        var codeDocument = CreateCodeDocument(text, documentUri.AbsolutePath);
        var razorSourceText = codeDocument.GetSourceText();
        var csharpSourceText = codeDocument.GetCSharpSourceText();

        var csharpDocumentSnapshot = CreateCSharpVirtualDocumentSnapshot(codeDocument, csharpDocumentUri.AbsoluteUri);
        var documentSnapshot = new TestLSPDocumentSnapshot(
            documentUri,
            version: 1,
            snapshotContent: razorSourceText.ToString(),
            csharpDocumentSnapshot);

        var mappingProvider = new TestLSPDocumentMappingProvider(LoggerFactory);
        var razorSpanMappingService = new TestRazorLSPSpanMappingService(
            mappingProvider, documentUri, razorSourceText, csharpSourceText, DisposalToken);

        await using var csharpServer = await CSharpTestLspServerHelpers.CreateCSharpLspServerAsync(
            csharpSourceText, csharpDocumentUri, _documentHighlightServerCapabilities, razorSpanMappingService, DisposalToken);

        var requestInvoker = new TestLSPRequestInvoker(csharpServer);
        var documentManager = new TestDocumentManager();
        documentManager.AddDocument(documentUri, documentSnapshot);
        var projectionProvider = new TestLSPProjectionProvider(LoggerFactory);

        var highlightHandler = new DocumentHighlightHandler(requestInvoker, documentManager, projectionProvider, mappingProvider, LoggerProvider);
        var highlightRequest = new DocumentHighlightParams()
        {
            TextDocument = new TextDocumentIdentifier { Uri = documentUri },
            Position = cursorPosition
        };

        // Act
        var result = await highlightHandler.HandleRequestAsync(highlightRequest, new ClientCapabilities(), DisposalToken);

        // Assert
        Assert.Empty(result);
    }

    private static LSPProjectionProvider GetMockProjectionProvider(ProjectionResult expectedResult)
    {
        var projectionProvider = new Mock<LSPProjectionProvider>(MockBehavior.Strict);
        projectionProvider
            .Setup(p => p.GetProjectionAsync(It.IsAny<LSPDocumentSnapshot>(), It.IsAny<Position>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        return projectionProvider.Object;
    }

    private static LSPRequestInvoker GetMockRequestInvoker<TParams, TResult>(TResult expectedResponse, Action<ITextBuffer, string, string, TParams, CancellationToken> callback)
    {
        var requestInvoker = new Mock<LSPRequestInvoker>(MockBehavior.Strict);
        requestInvoker
            .Setup(r => r.ReinvokeRequestOnServerAsync<TParams, TResult>(
                It.IsAny<ITextBuffer>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<TParams>(),
                It.IsAny<CancellationToken>()))
            .Callback(callback)
            .ReturnsAsync(new ReinvocationResponse<TResult>("LanguageClient", expectedResponse));

        return requestInvoker.Object;
    }

    private LSPDocumentMappingProvider GetMockDocumentMappingProvider(Range expectedRange, int expectedVersion, RazorLanguageKind languageKind)
    {
        var remappingResult = new RazorMapToDocumentRangesResponse()
        {
            Ranges = new[] { expectedRange },
            HostDocumentVersion = expectedVersion
        };
        var documentMappingProvider = new Mock<LSPDocumentMappingProvider>(MockBehavior.Strict);
        documentMappingProvider
            .Setup(d => d.MapToDocumentRangesAsync(languageKind, _uri, It.IsAny<Range[]>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(remappingResult);

        return documentMappingProvider.Object;
    }

    private static DocumentHighlight GetHighlight(int startLine, int startCharacter, int endLine, int endCharacter)
    {
        return new DocumentHighlight()
        {
            Range = new Range()
            {
                Start = new Position(startLine, startCharacter),
                End = new Position(endLine, endCharacter)
            }
        };
    }
}
