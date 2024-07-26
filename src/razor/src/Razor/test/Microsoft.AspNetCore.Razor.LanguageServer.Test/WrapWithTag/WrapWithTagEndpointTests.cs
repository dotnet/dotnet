// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Formatting;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.WrapWithTag;

public class WrapWithTagEndpointTest(ITestOutputHelper testOutput) : LanguageServerTestBase(testOutput)
{
    [Fact]
    public async Task Handle_Html_ReturnsResult()
    {
        // Arrange
        var codeDocument = CreateCodeDocument("<div></div>");
        var uri = new Uri("file://path/test.razor");
        var documentContext = CreateDocumentContext(uri, codeDocument);
        var response = new WrapWithTagResponse();

        var clientConnection = new Mock<IClientConnection>(MockBehavior.Strict);
        clientConnection
            .Setup(l => l.SendRequestAsync<WrapWithTagParams, WrapWithTagResponse>(LanguageServerConstants.RazorWrapWithTagEndpoint, It.IsAny<WrapWithTagParams>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var documentMappingService = Mock.Of<IRazorDocumentMappingService>(
            s => s.GetLanguageKind(codeDocument, It.IsAny<int>(), It.IsAny<bool>()) == RazorLanguageKind.Html, MockBehavior.Strict);
        var endpoint = new WrapWithTagEndpoint(
            clientConnection.Object,
            documentMappingService,
            LoggerFactory);

        var wrapWithDivParams = new WrapWithTagParams(new TextDocumentIdentifier { Uri = uri })
        {
            Range = VsLspFactory.CreateSingleLineRange(start: (0, 0), length: 2),
        };
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(wrapWithDivParams, requestContext, DisposalToken);

        // Assert
        Assert.NotNull(result);
        clientConnection.Verify();
    }

    [Fact]
    public async Task Handle_CSharp_ReturnsNull()
    {
        // Arrange
        var codeDocument = CreateCodeDocument("@(counter)");
        var uri = new Uri("file://path/test.razor");
        var documentContext = CreateDocumentContext(uri, codeDocument);
        var response = new WrapWithTagResponse();

        var clientConnection = new Mock<IClientConnection>(MockBehavior.Strict);
        clientConnection
            .Setup(l => l.SendRequestAsync<WrapWithTagParams, WrapWithTagResponse>(LanguageServerConstants.RazorWrapWithTagEndpoint, It.IsAny<WrapWithTagParams>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var documentMappingService = Mock.Of<IRazorDocumentMappingService>(
            s => s.GetLanguageKind(codeDocument, It.IsAny<int>(), It.IsAny<bool>()) == RazorLanguageKind.CSharp, MockBehavior.Strict);
        var endpoint = new WrapWithTagEndpoint(
            clientConnection.Object,
            documentMappingService,
            LoggerFactory);

        var wrapWithDivParams = new WrapWithTagParams(new TextDocumentIdentifier { Uri = uri })
        {
            Range = VsLspFactory.CreateSingleLineRange(start: (0, 0), length: 2),
        };
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(wrapWithDivParams, requestContext, DisposalToken);

        // Assert
        Assert.Null(result);
        clientConnection.Verify();
    }

    [Fact]
    public async Task Handle_CSharp_WholeImplicitStatement_ReturnsResult()
    {
        // Arrange
        var codeDocument = CreateCodeDocument("@counter");
        var uri = new Uri("file://path/test.razor");
        var documentContext = CreateDocumentContext(uri, codeDocument);
        var response = new WrapWithTagResponse();

        var clientConnection = new Mock<IClientConnection>(MockBehavior.Strict);
        clientConnection
            .Setup(l => l.SendRequestAsync<WrapWithTagParams, WrapWithTagResponse>(LanguageServerConstants.RazorWrapWithTagEndpoint, It.IsAny<WrapWithTagParams>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var documentMappingService = Mock.Of<IRazorDocumentMappingService>(
            s => s.GetLanguageKind(codeDocument, It.IsAny<int>(), It.IsAny<bool>()) == RazorLanguageKind.CSharp, MockBehavior.Strict);
        var endpoint = new WrapWithTagEndpoint(
            clientConnection.Object,
            documentMappingService,
            LoggerFactory);

        var wrapWithDivParams = new WrapWithTagParams(new TextDocumentIdentifier { Uri = uri })
        {
            Range = VsLspFactory.CreateSingleLineRange(start: (0, 0), length: 8),
        };
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(wrapWithDivParams, requestContext, DisposalToken);

        // Assert
        Assert.NotNull(result);
        clientConnection.Verify();
    }

    [Fact]
    public async Task Handle_CSharp_PartOfImplicitStatement_ReturnsNull()
    {
        // Arrange
        var codeDocument = CreateCodeDocument("@counter");
        var uri = new Uri("file://path/test.razor");
        var documentContext = CreateDocumentContext(uri, codeDocument);
        var response = new WrapWithTagResponse();

        var clientConnection = new Mock<IClientConnection>(MockBehavior.Strict);
        clientConnection
            .Setup(l => l.SendRequestAsync<WrapWithTagParams, WrapWithTagResponse>(LanguageServerConstants.RazorWrapWithTagEndpoint, It.IsAny<WrapWithTagParams>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var documentMappingService = Mock.Of<IRazorDocumentMappingService>(
            s => s.GetLanguageKind(codeDocument, It.IsAny<int>(), It.IsAny<bool>()) == RazorLanguageKind.CSharp, MockBehavior.Strict);
        var endpoint = new WrapWithTagEndpoint(
            clientConnection.Object,
            documentMappingService,
            LoggerFactory);

        var wrapWithDivParams = new WrapWithTagParams(new TextDocumentIdentifier { Uri = uri })
        {
            Range = VsLspFactory.CreateSingleLineRange(line: 0, character: 2, length: 2),
        };
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(wrapWithDivParams, requestContext, DisposalToken);

        // Assert
        Assert.Null(result);
        clientConnection.Verify();
    }

    [Fact]
    public async Task Handle_CSharp_InImplicitStatement_ReturnsResult()
    {
        // Arrange
        var codeDocument = CreateCodeDocument("@counter");
        var uri = new Uri("file://path/test.razor");
        var documentContext = CreateDocumentContext(uri, codeDocument);
        var response = new WrapWithTagResponse();

        var clientConnection = new Mock<IClientConnection>(MockBehavior.Strict);
        clientConnection
            .Setup(l => l.SendRequestAsync<WrapWithTagParams, WrapWithTagResponse>(LanguageServerConstants.RazorWrapWithTagEndpoint, It.IsAny<WrapWithTagParams>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var documentMappingService = Mock.Of<IRazorDocumentMappingService>(
            s => s.GetLanguageKind(codeDocument, It.IsAny<int>(), It.IsAny<bool>()) == RazorLanguageKind.CSharp, MockBehavior.Strict);
        var endpoint = new WrapWithTagEndpoint(
            clientConnection.Object,
            documentMappingService,
            LoggerFactory);

        var wrapWithDivParams = new WrapWithTagParams(new TextDocumentIdentifier { Uri = uri })
        {
            Range = VsLspFactory.CreateZeroWidthRange(0, 4),
        };
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(wrapWithDivParams, requestContext, DisposalToken);

        // Assert
        Assert.NotNull(result);
        clientConnection.Verify();
    }

    [Fact]
    public async Task Handle_DocumentNotFound_ReturnsNull()
    {
        // Arrange
        var codeDocument = CreateCodeDocument("<div></div>");
        var realUri = new Uri("file://path/test.razor");
        var missingUri = new Uri("file://path/nottest.razor");

        var clientConnection = new Mock<IClientConnection>(MockBehavior.Strict);

        var documentMappingService = Mock.Of<IRazorDocumentMappingService>(
            s => s.GetLanguageKind(codeDocument, It.IsAny<int>(), It.IsAny<bool>()) == RazorLanguageKind.Html, MockBehavior.Strict);
        var endpoint = new WrapWithTagEndpoint(clientConnection.Object, documentMappingService, LoggerFactory);

        var wrapWithDivParams = new WrapWithTagParams(new TextDocumentIdentifier { Uri = missingUri })
        {
            Range = VsLspFactory.CreateSingleLineRange(start: (0, 0), length: 2),
        };
        var requestContext = CreateRazorRequestContext(documentContext: null);

        // Act
        var result = await endpoint.HandleRequestAsync(wrapWithDivParams, requestContext, DisposalToken);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Handle_UnsupportedCodeDocument_ReturnsNull()
    {
        // Arrange
        var codeDocument = CreateCodeDocument("<div></div>");
        codeDocument.SetUnsupported();
        var uri = new Uri("file://path/test.razor");
        var documentContext = CreateDocumentContext(uri, codeDocument);

        var clientConnection = new Mock<IClientConnection>(MockBehavior.Strict);

        var documentMappingService = Mock.Of<IRazorDocumentMappingService>(
            s => s.GetLanguageKind(codeDocument, It.IsAny<int>(), It.IsAny<bool>()) == RazorLanguageKind.Html, MockBehavior.Strict);
        var endpoint = new WrapWithTagEndpoint(clientConnection.Object, documentMappingService, LoggerFactory);

        var wrapWithDivParams = new WrapWithTagParams(new TextDocumentIdentifier { Uri = uri })
        {
            Range = VsLspFactory.CreateSingleLineRange(start: (0, 0), length: 2),
        };
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(wrapWithDivParams, requestContext, DisposalToken);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task CleanUpTextEdits_NoTilde()
    {
        var input = """
                @if (true)
                {
                }
                """;
        var expected = """
                <div>
                    @if (true)
                    {
                    }
                </div>
                """;

        var uri = new Uri("file://path.razor");
        var factory = CreateDocumentContextFactory(uri, input);
        Assert.True(factory.TryCreate(uri, out var context));
        var inputSourceText = await context!.GetSourceTextAsync(DisposalToken);

        var computedEdits = new TextEdit[]
        {
            VsLspFactory.CreateTextEdit(position: (0, 0), "<div>" + Environment.NewLine + "    "),
            VsLspFactory.CreateTextEdit(line: 1, character: 0, "    "),
            VsLspFactory.CreateTextEdit(
                range: VsLspFactory.CreateSingleLineRange(line: 2, character: 0, length: 1),
                newText: "    }" + Environment.NewLine + "</div>"),
        };

        var htmlSourceText = await context!.GetHtmlSourceTextAsync(DisposalToken);
        var edits = HtmlFormatter.FixHtmlTestEdits(htmlSourceText, computedEdits);
        Assert.Same(computedEdits, edits);

        var finalText = inputSourceText.WithChanges(edits.Select(inputSourceText.GetTextChange));
        Assert.Equal(expected, finalText.ToString());
    }

    [Fact]
    public async Task CleanUpTextEdits_BadEditWithTilde()
    {
        var input = """
                @if (true)
                {
                }
                """;

        var expected = """
                <div>
                    @if (true)
                    {
                    }
                </div>
                """;

        var uri = new Uri("file://path.razor");
        var factory = CreateDocumentContextFactory(uri, input);
        Assert.True(factory.TryCreate(uri, out var context));
        var inputSourceText = await context!.GetSourceTextAsync(DisposalToken);

        var computedEdits = new TextEdit[]
        {
            VsLspFactory.CreateTextEdit(position: (0, 0), "<div>" + Environment.NewLine + "    "),
            VsLspFactory.CreateTextEdit(line: 1, character: 0, "    "),
            // This is the problematic edit.. the close brace has been replaced with a tilde
            VsLspFactory.CreateTextEdit(
                range: VsLspFactory.CreateSingleLineRange(line: 2, character: 0, length: 1),
                newText: "    ~" + Environment.NewLine + "</div>")
        };

        var htmlSourceText = await context!.GetHtmlSourceTextAsync(DisposalToken);
        var edits = HtmlFormatter.FixHtmlTestEdits(htmlSourceText, computedEdits);
        Assert.NotSame(computedEdits, edits);

        var finalText = inputSourceText.WithChanges(edits.Select(inputSourceText.GetTextChange));
        Assert.Equal(expected, finalText.ToString());
    }

    [Fact]
    public async Task CleanUpTextEdits_GoodEditWithTilde()
    {
        var input = """
                @if (true)
                {
                ~
                """;

        var expected = """
                <div>
                    @if (true)
                    {
                    ~
                </div>
                """;

        var uri = new Uri("file://path.razor");
        var factory = CreateDocumentContextFactory(uri, input);
        Assert.True(factory.TryCreate(uri, out var context));
        var inputSourceText = await context.GetSourceTextAsync(DisposalToken);

        var computedEdits = new[]
        {
            VsLspFactory.CreateTextEdit(position: (0, 0), "<div>" + Environment.NewLine + "    "),
            VsLspFactory.CreateTextEdit(line: 1, character: 0, "    "),
            // This looks like a bad edit, but the original source document had a tilde
            VsLspFactory.CreateTextEdit(
                range: VsLspFactory.CreateSingleLineRange(line: 2, character: 0, length: 1),
                newText: "    ~" + Environment.NewLine + "</div>")
        };

        var htmlSourceText = await context.GetHtmlSourceTextAsync(DisposalToken);
        var edits = HtmlFormatter.FixHtmlTestEdits(htmlSourceText, computedEdits);
        Assert.NotSame(computedEdits, edits);

        var finalText = inputSourceText.WithChanges(edits.Select(inputSourceText.GetTextChange));
        Assert.Equal(expected, finalText.ToString());
    }
}
