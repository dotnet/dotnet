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
using Microsoft.VisualStudio.Test;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Threading;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor.HtmlCSharp;

[UseExportProvider]
public class SignatureHelpHandlerTest : HandlerTestBase
{
    private readonly Uri _uri;
    private readonly TestDocumentManager _defaultDocumentManager;
    private readonly ServerCapabilities _signatureHelpServerCapabilities;

    public SignatureHelpHandlerTest(ITestOutputHelper testOutput)
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
        _defaultDocumentManager = new TestDocumentManager();
        _defaultDocumentManager.AddDocument(_uri, documentSnapshot);

        _signatureHelpServerCapabilities = new()
        {
            SignatureHelpProvider = new SignatureHelpOptions
            {
                TriggerCharacters = new string[] { "(", "," }
            }
        };
    }

    [Fact]
    public async Task HandleRequestAsync_DocumentNotFound_ReturnsNull()
    {
        // Arrange
        var documentManager = new TestDocumentManager();
        var requestInvoker = Mock.Of<LSPRequestInvoker>(MockBehavior.Strict);
        var projectionProvider = Mock.Of<LSPProjectionProvider>(MockBehavior.Strict);
        var signatureHelpHandler = new SignatureHelpHandler(requestInvoker, documentManager, projectionProvider, LoggerProvider);
        var signatureHelpRequest = new TextDocumentPositionParams()
        {
            TextDocument = new TextDocumentIdentifier() { Uri = _uri },
            Position = new Position(0, 1)
        };

        // Act
        var result = await signatureHelpHandler.HandleRequestAsync(signatureHelpRequest, new ClientCapabilities(), DisposalToken);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task HandleRequestAsync_ProjectionNotFound_ReturnsNull()
    {
        // Arrange
        var requestInvoker = Mock.Of<LSPRequestInvoker>(MockBehavior.Strict);
        var projectionProvider = new Mock<LSPProjectionProvider>(MockBehavior.Strict).Object;
        Mock.Get(projectionProvider)
            .Setup(projectionProvider => projectionProvider.GetProjectionAsync(It.IsAny<LSPDocumentSnapshot>(), It.IsAny<Position>(), DisposalToken))
            .ReturnsAsync(value: null);
        var signatureHelpHandler = new SignatureHelpHandler(requestInvoker, _defaultDocumentManager, projectionProvider, LoggerProvider);
        var signatureHelpRequest = new TextDocumentPositionParams()
        {
            TextDocument = new TextDocumentIdentifier() { Uri = _uri },
            Position = new Position(0, 1)
        };

        // Act
        var result = await signatureHelpHandler.HandleRequestAsync(signatureHelpRequest, new ClientCapabilities(), DisposalToken);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task HandleRequestAsync_HtmlProjection_InvokesHtmlLanguageServer_ReturnsItem()
    {
        // Arrange
        var called = false;
        var expectedResult = new ReinvocationResponse<SignatureHelp>("LanguageClientName", new SignatureHelp());

        var virtualHtmlUri = new Uri("C:/path/to/file.razor__virtual.html");
        var requestInvoker = new Mock<LSPRequestInvoker>(MockBehavior.Strict);
        requestInvoker
            .Setup(r => r.ReinvokeRequestOnServerAsync<TextDocumentPositionParams, SignatureHelp>(
                It.IsAny<ITextBuffer>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<TextDocumentPositionParams>(),
                It.IsAny<CancellationToken>()))
            .Callback<ITextBuffer, string, string, TextDocumentPositionParams, CancellationToken>((textBuffer, method, clientName, definitionParams, ct) =>
            {
                Assert.Equal(Methods.TextDocumentSignatureHelpName, method);
                Assert.Equal(RazorLSPConstants.HtmlLanguageServerName, clientName);
                called = true;
            })
            .ReturnsAsync(expectedResult);

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

        var signatureHelpHandler = new SignatureHelpHandler(requestInvoker.Object, _defaultDocumentManager, projectionProvider.Object, LoggerProvider);
        var signatureHelpRequest = new TextDocumentPositionParams()
        {
            TextDocument = new TextDocumentIdentifier() { Uri = _uri },
            Position = new Position(10, 5)
        };

        // Act
        var result = await signatureHelpHandler.HandleRequestAsync(signatureHelpRequest, new ClientCapabilities(), DisposalToken);

        // Assert
        Assert.True(called);
        Assert.Equal(expectedResult.Response, result);
    }

    [Fact]
    public async Task HandleRequestAsync_CSharpProjection_InvokesCSharpLanguageServer_ReturnsItem()
    {
        // Arrange
        var text = """
                @code
                {
                    void M(int a, int b, int c)
                    {
                        M(
                    }
                }
                """;
        var cursorPosition = new Position { Line = 4, Character = 10 };

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
            csharpSourceText, csharpDocumentUri, _signatureHelpServerCapabilities, razorSpanMappingService, DisposalToken);

        var requestInvoker = new TestLSPRequestInvoker(csharpServer);
        var documentManager = new TestDocumentManager();
        documentManager.AddDocument(documentUri, documentSnapshot);
        var projectionProvider = new TestLSPProjectionProvider(LoggerFactory);

        var signatureHelpHandler = new SignatureHelpHandler(requestInvoker, documentManager, projectionProvider, LoggerProvider);
        var signatureHelpRequest = new TextDocumentPositionParams()
        {
            TextDocument = new TextDocumentIdentifier() { Uri = documentUri },
            Position = cursorPosition
        };

        // Act
        var requestResult = await signatureHelpHandler.HandleRequestAsync(
            signatureHelpRequest, new ClientCapabilities(), DisposalToken);

        // Assert
        Assert.Equal("void M(int a, int b, int c)", requestResult.Signatures.First().Label);
    }

    [Fact]
    public async Task HandleRequestAsync_ReturnNullIfCSharpLanguageServerReturnsNull()
    {
        // Arrange
        var text = """
                @code
                {
                    void M(int a, int b, int c)
                    {
                        M(
                    }
                }
                """;
        var cursorPosition = new Position { Line = 2, Character = 0 };

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
            csharpSourceText, csharpDocumentUri, _signatureHelpServerCapabilities, razorSpanMappingService, DisposalToken);

        var requestInvoker = new TestLSPRequestInvoker(csharpServer);
        var documentManager = new TestDocumentManager();
        documentManager.AddDocument(documentUri, documentSnapshot);
        var projectProvider = new TestLSPProjectionProvider(LoggerFactory);

        var signatureHelpHandler = new SignatureHelpHandler(requestInvoker, documentManager, projectProvider, LoggerProvider);
        var signatureHelpRequest = new TextDocumentPositionParams()
        {
            TextDocument = new TextDocumentIdentifier() { Uri = documentUri },
            Position = cursorPosition
        };

        // Act
        var requestResult = await signatureHelpHandler.HandleRequestAsync(
            signatureHelpRequest, new ClientCapabilities(), DisposalToken);

        // Assert
        Assert.Null(requestResult);
    }
}
