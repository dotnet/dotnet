// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
using Microsoft.VisualStudio.Test;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Threading;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor.HtmlCSharp;

[UseExportProvider]
public class HoverHandlerTest : HandlerTestBase
{
    private readonly Uri _uri;
    private readonly TestDocumentManager _documentManager;
    private readonly ServerCapabilities _hoverServerCapabilities;

    public HoverHandlerTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        _uri = new Uri("C:/path/to/file.razor");
        var csharpVirtualDocument = new CSharpVirtualDocumentSnapshot(
            new Uri("C:/path/to/file.razor.g.cs"),
            new TestTextBuffer(new StringTextSnapshot(string.Empty)).CurrentSnapshot,
            hostDocumentSyncVersion: 0);
        var htmlVirtualDocument = new HtmlVirtualDocumentSnapshot(
            new Uri("C:/path/to/file.razor__virtual.html"),
            new TestTextBuffer(new StringTextSnapshot(string.Empty)).CurrentSnapshot,
            hostDocumentSyncVersion: 0);
        LSPDocumentSnapshot documentSnapshot = new TestLSPDocumentSnapshot(_uri, version: 0, htmlVirtualDocument, csharpVirtualDocument);
        _documentManager = new TestDocumentManager();
        _documentManager.AddDocument(_uri, documentSnapshot);

        _hoverServerCapabilities = new()
        {
            HoverProvider = true
        };
    }

    [Fact]
    public async Task HandleRequestAsync_DocumentNotFound_ReturnsNull()
    {
        // Arrange
        var documentManager = new TestDocumentManager();
        var requestInvoker = new TestLSPRequestInvoker();
        var projectionProvider = new TestLSPProjectionProvider(LoggerFactory);
        var documentMappingProvider = new TestLSPDocumentMappingProvider(LoggerFactory);
        var hoverHandler = new HoverHandler(requestInvoker, documentManager, projectionProvider, documentMappingProvider, LoggerProvider);
        var hoverRequest = new TextDocumentPositionParams()
        {
            TextDocument = new TextDocumentIdentifier() { Uri = _uri },
            Position = new Position(0, 1)
        };

        // Act
        var result = await hoverHandler.HandleRequestAsync(hoverRequest, new ClientCapabilities(), DisposalToken);

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
        var hoverHandler = new HoverHandler(requestInvoker, _documentManager, projectionProvider, documentMappingProvider, LoggerProvider);
        var hoverRequest = new TextDocumentPositionParams()
        {
            TextDocument = new TextDocumentIdentifier() { Uri = _uri },
            Position = new Position(0, 1)
        };

        // Act
        var result = await hoverHandler.HandleRequestAsync(hoverRequest, new ClientCapabilities(), DisposalToken);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task HandleRequestAsync_CSharpProjection_InvokesCSharpLanguageServer()
    {
        // Arrange
        var text = """
                @code
                {
                    void M(int a, int b, int c)
                    {
                    }
                }
                """;
        var cursorPosition = new Position { Line = 2, Character = 10 };

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
            csharpSourceText, csharpDocumentUri, _hoverServerCapabilities, razorSpanMappingService, DisposalToken);

        var requestInvoker = new TestLSPRequestInvoker(csharpServer);
        var documentManager = new TestDocumentManager();
        documentManager.AddDocument(documentUri, documentSnapshot);
        var projectionProvider = new TestLSPProjectionProvider(LoggerFactory);

        var hoverHandler = new HoverHandler(requestInvoker, documentManager, projectionProvider, mappingProvider, LoggerProvider);
        var hoverRequest = new TextDocumentPositionParams()
        {
            TextDocument = new TextDocumentIdentifier() { Uri = documentUri },
            Position = cursorPosition
        };

        var expectedRange = new Range { Start = new Position { Line = 2, Character = 9 }, End = new Position { Line = 2, Character = 10 } };

        // Act
        var result = await hoverHandler.HandleRequestAsync(hoverRequest, new ClientCapabilities(), DisposalToken);

        // Assert
        AssertVSHover(result, expectedRegex: """void .+\.M\(int a, int b, int c\)""", expectedRange);
    }

    [Fact]
    public async Task HandleRequestAsync_RazorProjection_InvokesHtmlLanguageServer()
    {
        // Arrange
        var called = false;

        var expectedContents = new SumType<string, MarkedString, SumType<string, MarkedString>[], MarkupContent>(
            new MarkedString()
            {
                Language = "markdown",
                Value = "HTML Hover Details"
            }
        );

        var lspResponse = new Hover()
        {
            Range = new Range()
            {
                Start = new Position(10, 0),
                End = new Position(10, 1)
            },
            Contents = expectedContents
        };

        var expectedItem = new Hover()
        {
            Range = new Range()
            {
                Start = new Position(0, 0),
                End = new Position(0, 1)
            },
            Contents = expectedContents
        };

        var hoverRequest = new TextDocumentPositionParams()
        {
            TextDocument = new TextDocumentIdentifier() { Uri = _uri },
            Position = new Position(0, 1)
        };

        var requestInvoker = new Mock<LSPRequestInvoker>(MockBehavior.Strict);
        requestInvoker
            .Setup(r => r.ReinvokeRequestOnServerAsync<TextDocumentPositionParams, Hover>(
                It.IsAny<ITextBuffer>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<TextDocumentPositionParams>(),
                It.IsAny<CancellationToken>()))
            .Callback<ITextBuffer, string, string, TextDocumentPositionParams, CancellationToken>((textBuffer, method, clientName, hoverParams, ct) =>
            {
                Assert.Equal(Methods.TextDocumentHoverName, method);
                Assert.Equal(RazorLSPConstants.HtmlLanguageServerName, clientName);
                called = true;
            })
            .ReturnsAsync(new ReinvocationResponse<Hover>("LanguageClientName", lspResponse));

        var projectionResult = new ProjectionResult()
        {
            Uri = null,
            Position = null,
            LanguageKind = RazorLanguageKind.Html,
        };

        var projectionProvider = new Mock<LSPProjectionProvider>(MockBehavior.Strict);
        projectionProvider
            .Setup(p => p.GetProjectionAsync(It.IsAny<LSPDocumentSnapshot>(), It.IsAny<Position>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(projectionResult);

        var remappingResult = new RazorMapToDocumentRangesResponse()
        {
            Ranges = new[] {
                new Range()
                {
                    Start = new Position(0, 0),
                    End = new Position(0, 1)
                }
            },
            HostDocumentVersion = 0
        };
        var documentMappingProvider = new Mock<LSPDocumentMappingProvider>(MockBehavior.Strict);
        documentMappingProvider
            .Setup(d => d.MapToDocumentRangesAsync(RazorLanguageKind.Html, It.IsAny<Uri>(), It.IsAny<Range[]>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(remappingResult);

        var hoverHandler = new HoverHandler(requestInvoker.Object, _documentManager, projectionProvider.Object, documentMappingProvider.Object, LoggerProvider);

        // Act
        var result = await hoverHandler.HandleRequestAsync(hoverRequest, new ClientCapabilities(), DisposalToken);

        // Assert
        Assert.True(called);
        Assert.Equal(expectedItem.Contents, result.Contents);
        Assert.Equal(expectedItem.Range, result.Range);
    }

    [Fact]
    public async Task HandleRequestAsync_CSharpProjection_InvokesCSharpLanguageServerWithNoResult()
    {
        // Arrange
        var text = """
                @code
                {
                    void M(int a, int b, int c)
                    {
                    }
                }
                """;
        var cursorPosition = new Position { Line = 2, Character = 4 };

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
            csharpSourceText, csharpDocumentUri, _hoverServerCapabilities, razorSpanMappingService, DisposalToken);

        var requestInvoker = new TestLSPRequestInvoker(csharpServer);
        var documentManager = new TestDocumentManager();
        documentManager.AddDocument(documentUri, documentSnapshot);
        var projectionProvider = new TestLSPProjectionProvider(LoggerFactory);

        var hoverHandler = new HoverHandler(requestInvoker, documentManager, projectionProvider, mappingProvider, LoggerProvider);
        var hoverRequest = new TextDocumentPositionParams()
        {
            TextDocument = new TextDocumentIdentifier() { Uri = documentUri },
            Position = cursorPosition
        };

        // Act
        var result = await hoverHandler.HandleRequestAsync(hoverRequest, new ClientCapabilities(), DisposalToken);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task HandleRequestAsync_CSharpProjection_InvokesCSharpLanguageServer_FailsRemappingResultReturnsHoverWithInitialPosition()
    {
        // Arrange
        var text = """
                @code
                {
                    void M(int a, int b, int c)
                    {
                    }
                }
                """;
        var cursorPosition = new Position { Line = 2, Character = 10 };

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

        var uriToVersionAndCodeDocumentMap = new Dictionary<Uri, (int hostDocumentVersion, RazorCodeDocument codeDocument)>();
        var mappingProvider = new TestLSPDocumentMappingProvider(uriToVersionAndCodeDocumentMap, LoggerFactory);
        var razorSpanMappingService = new TestRazorLSPSpanMappingService(
            mappingProvider, documentUri, razorSourceText, csharpSourceText, DisposalToken);

        await using var csharpServer = await CSharpTestLspServerHelpers.CreateCSharpLspServerAsync(
            csharpSourceText, csharpDocumentUri, _hoverServerCapabilities, razorSpanMappingService, DisposalToken);

        var requestInvoker = new TestLSPRequestInvoker(csharpServer);
        var documentManager = new TestDocumentManager();
        documentManager.AddDocument(documentUri, documentSnapshot);
        var projectionProvider = new TestLSPProjectionProvider(LoggerFactory);

        var hoverHandler = new HoverHandler(requestInvoker, documentManager, projectionProvider, mappingProvider, LoggerProvider);
        var hoverRequest = new TextDocumentPositionParams()
        {
            TextDocument = new TextDocumentIdentifier() { Uri = documentUri },
            Position = cursorPosition
        };

        var expectedRange = new Range
        {
            Start = cursorPosition,
            End = cursorPosition
        };

        // Act
        var result = await hoverHandler.HandleRequestAsync(hoverRequest, new ClientCapabilities(), DisposalToken);

        // Assert
        Assert.Equal(expectedRange, result.Range);
    }

    [Fact]
    public async Task HandleRequestAsync_CSharpProjection_InvokesCSharpLanguageServer_FailsRemappingResultRangeWithHostVersionChanged()
    {
        // Arrange
        var text = """
                @code
                {
                    void M(int a, int b, int c)
                    {
                    }
                }
                """;
        var cursorPosition = new Position { Line = 2, Character = 10 };

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

        var uriToVersionAndCodeDocumentMap = new Dictionary<Uri, (int hostDocumentVersion, RazorCodeDocument codeDocument)>
        {
            { documentUri, (hostDocumentVersion: 2, codeDocument) }
        };

        var mappingProvider = new TestLSPDocumentMappingProvider(uriToVersionAndCodeDocumentMap, LoggerFactory);
        var razorSpanMappingService = new TestRazorLSPSpanMappingService(
            mappingProvider, documentUri, razorSourceText, csharpSourceText, DisposalToken);

        await using var csharpServer = await CSharpTestLspServerHelpers.CreateCSharpLspServerAsync(
            csharpSourceText, csharpDocumentUri, _hoverServerCapabilities, razorSpanMappingService, DisposalToken);

        var requestInvoker = new TestLSPRequestInvoker(csharpServer);
        var documentManager = new TestDocumentManager();
        documentManager.AddDocument(documentUri, documentSnapshot);
        var projectionProvider = new TestLSPProjectionProvider(LoggerFactory);

        var hoverHandler = new HoverHandler(requestInvoker, documentManager, projectionProvider, mappingProvider, LoggerProvider);
        var hoverRequest = new TextDocumentPositionParams()
        {
            TextDocument = new TextDocumentIdentifier() { Uri = documentUri },
            Position = cursorPosition
        };

        // Act
        var result = await hoverHandler.HandleRequestAsync(hoverRequest, new ClientCapabilities(), DisposalToken);

        // Assert
        Assert.Null(result);
    }

    private static void AssertVSHover(Hover hover, string expectedRegex, Range expectedRange)
    {
        // 1. Assert type is VSInternalHover
        var vsHover = Assert.IsType<VSInternalHover>(hover);

        // 2. Assert matching range
        Assert.Equal(expectedRange, vsHover.Range);

        var containerElement = vsHover.RawContent as ContainerElement;
        var classifiedTextElements = new List<ClassifiedTextElement>();

        GetClassifiedTextElements(containerElement, classifiedTextElements);

        var content = string.Join("|", classifiedTextElements.Select(
            cte => string.Join(string.Empty, cte.Runs.Select(ctr => ctr.Text))));
        var isMatch = Regex.IsMatch(content, expectedRegex);

        // 3. Assert matching content string
        Assert.True(isMatch);

        static void GetClassifiedTextElements(ContainerElement container, List<ClassifiedTextElement> classifiedTextElements)
        {
            foreach (var element in container.Elements)
            {
                if (element is ClassifiedTextElement classifiedTextElement)
                {
                    classifiedTextElements.Add(classifiedTextElement);
                }
                else if (element is ContainerElement containerElement)
                {
                    GetClassifiedTextElements(containerElement, classifiedTextElements);
                }
            }
        }
    }
}
