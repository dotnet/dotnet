// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.AspNetCore.Razor.LanguageServer.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.Mef;
using Microsoft.CodeAnalysis.Razor.Workspaces.Extensions;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.LanguageServerClient.Razor.Test;
using Microsoft.VisualStudio.Test;
using Microsoft.VisualStudio.Text;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor.HtmlCSharp;

[UseExportProvider]
public class RenameHandlerTest : HandlerTestBase
{
    private readonly Uri _uri;
    private readonly TestDocumentManager _documentManager;
    private readonly ServerCapabilities _renameServerCapabilities;

    public RenameHandlerTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        _uri = new Uri("C:/path/to/file.razor");
        var htmlVirtualDocument = new HtmlVirtualDocumentSnapshot(
            new Uri("C:/path/to/file.razor__virtual.html"),
            new TestTextBuffer(new StringTextSnapshot(string.Empty)).CurrentSnapshot,
            hostDocumentSyncVersion: 0);
        var documentSnapshot = new TestLSPDocumentSnapshot(_uri, version: 0, htmlVirtualDocument);
        _documentManager = new TestDocumentManager();
        _documentManager.AddDocument(_uri, documentSnapshot);

        _renameServerCapabilities = new()
        {
            RenameProvider = true
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
        var renameHandler = new RenameHandler(requestInvoker, documentManager, projectionProvider, documentMappingProvider, LoggerProvider);
        var renameRequest = new RenameParams()
        {
            Position = new Position(0, 1),
            NewName = "NewName",
            TextDocument = new TextDocumentIdentifier() { Uri = _uri },
        };

        // Act
        var result = await renameHandler.HandleRequestAsync(renameRequest, new ClientCapabilities(), DisposalToken);

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
        var renameHandler = new RenameHandler(requestInvoker, _documentManager, projectionProvider, documentMappingProvider, LoggerProvider);
        var renameRequest = new RenameParams()
        {
            Position = new Position(0, 1),
            NewName = "NewName",
            TextDocument = new TextDocumentIdentifier() { Uri = _uri },
        };

        // Act
        var result = await renameHandler.HandleRequestAsync(renameRequest, new ClientCapabilities(), DisposalToken);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task HandleRequestAsync_HtmlProjection_RemapsWorkspaceEdit()
    {
        // Arrange
        var called = false;
        var expectedEdit = new WorkspaceEdit();

        var requestInvoker = GetMockRequestInvoker<RenameParams, WorkspaceEdit>(
            new WorkspaceEdit(),
            (textBuffer, method, clientName, renameParams, ct) =>
            {
                Assert.Equal(Methods.TextDocumentRenameName, method);
                Assert.Equal(RazorLSPConstants.HtmlLanguageServerName, clientName);
                called = true;
            });

        var projectionProvider = GetMockProjectionProvider(new ProjectionResult() { Uri = null, Position = null, LanguageKind = RazorLanguageKind.Html });
        var documentMappingProvider = GetMockDocumentMappingProvider(expectedEdit);

        var renameHandler = new RenameHandler(requestInvoker, _documentManager, projectionProvider, documentMappingProvider, LoggerProvider);
        var renameRequest = new RenameParams()
        {
            Position = new Position(0, 1),
            NewName = "NewName",
            TextDocument = new TextDocumentIdentifier() { Uri = _uri },
        };

        // Act
        var result = await renameHandler.HandleRequestAsync(renameRequest, new ClientCapabilities(), DisposalToken);

        // Assert
        Assert.True(called);
        Assert.Equal(expectedEdit, result);

        // Actual remapping behavior is tested in LSPDocumentMappingProvider tests.
    }

    [Fact]
    public async Task HandleRequestAsync_CSharpProjection_BasicRename()
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
            csharpSourceText, csharpDocumentUri, _renameServerCapabilities, razorSpanMappingService, DisposalToken);

        var requestInvoker = new TestLSPRequestInvoker(csharpServer);
        var documentManager = new TestDocumentManager();
        documentManager.AddDocument(documentUri, documentSnapshot);
        var projectionProvider = new TestLSPProjectionProvider(LoggerFactory);

        var documentMappingProvider = new DefaultLSPDocumentMappingProvider(requestInvoker, new Lazy<LSPDocumentManager>(() => documentManager), new RazorLSPConventions(TestLanguageServerFeatureOptions.Instance));
        var renameHandler = new RenameHandler(requestInvoker, documentManager, projectionProvider, documentMappingProvider, LoggerProvider);
        var renameRequest = new RenameParams()
        {
            Position = cursorPosition,
            NewName = "NewName",
            TextDocument = new TextDocumentIdentifier() { Uri = documentUri },
        };

        var firstExpectedRange = new Range { Start = new Position { Line = 2, Character = 9 }, End = new Position { Line = 2, Character = 15 } };
        var secondExpectedRange = new Range { Start = new Position { Line = 4, Character = 8 }, End = new Position { Line = 4, Character = 14 } };

        // Act
        var result = await renameHandler.HandleRequestAsync(renameRequest, new ClientCapabilities(), DisposalToken);

        // Assert
        var textDocumentEdits = Assert.IsType<TextDocumentEdit[]>(result.DocumentChanges.Value.Value);
        var textDocumentEdit = textDocumentEdits.Single();
        Assert.Equal(documentUri, textDocumentEdit.TextDocument.Uri);
        Assert.Equal(2, textDocumentEdit.Edits.Length);
        Assert.Equal(firstExpectedRange, textDocumentEdit.Edits[0].Range);
        Assert.Equal(secondExpectedRange, textDocumentEdit.Edits[1].Range);
        Assert.Equal("NewName", textDocumentEdit.Edits[0].NewText);
        Assert.Equal("NewName", textDocumentEdit.Edits[1].NewText);
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
            .ReturnsAsync(new ReinvocationResponse<TResult>("LanguageClientName", expectedResponse));

        return requestInvoker.Object;
    }

    private static LSPDocumentMappingProvider GetMockDocumentMappingProvider(WorkspaceEdit expectedEdit)
    {
        var documentMappingProvider = new Mock<LSPDocumentMappingProvider>(MockBehavior.Strict);
        documentMappingProvider
            .Setup(d => d.RemapWorkspaceEditAsync(It.IsAny<WorkspaceEdit>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedEdit);

        return documentMappingProvider.Object;
    }
}
