// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.CodeAnalysis.Razor.Formatting;
using Microsoft.CodeAnalysis.Razor.Protocol;
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

        var clientConnection = TestMocks.CreateClientConnection(builder =>
        {
            builder.SetupSendRequest<WrapWithTagParams, WrapWithTagResponse>(LanguageServerConstants.RazorWrapWithTagEndpoint, response: new(), verifiable: true);
        });

        var endpoint = new WrapWithTagEndpoint(clientConnection, LoggerFactory);

        var wrapWithDivParams = new WrapWithTagParams(new() { DocumentUri = new(uri) })
        {
            Range = LspFactory.CreateSingleLineRange(start: (0, 0), length: 2),
        };

        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(wrapWithDivParams, requestContext, DisposalToken);

        // Assert
        Assert.NotNull(result);
        Mock.Get(clientConnection).Verify();
    }

    [Fact]
    public async Task Handle_CSharp_ReturnsNull()
    {
        // Arrange
        var codeDocument = CreateCodeDocument("@(counter)");
        var uri = new Uri("file://path/test.razor");
        var documentContext = CreateDocumentContext(uri, codeDocument);

        var clientConnection = TestMocks.CreateClientConnection(builder =>
        {
            builder.SetupSendRequest<WrapWithTagParams, WrapWithTagResponse>(LanguageServerConstants.RazorWrapWithTagEndpoint, response: new(), verifiable: true);
        });

        var endpoint = new WrapWithTagEndpoint(clientConnection, LoggerFactory);

        var wrapWithDivParams = new WrapWithTagParams(new() { DocumentUri = new(uri) })
        {
            Range = LspFactory.CreateSingleLineRange(start: (0, 1), length: 2),
        };

        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(wrapWithDivParams, requestContext, DisposalToken);

        // Assert
        Assert.Null(result);
        Mock.Get(clientConnection)
            .VerifySendRequest<WrapWithTagParams, WrapWithTagResponse>(LanguageServerConstants.RazorWrapWithTagEndpoint, Times.Never);
    }

    [Fact]
    public async Task Handle_CSharp_WholeImplicitStatement_ReturnsResult()
    {
        // Arrange
        var codeDocument = CreateCodeDocument("@counter");
        var uri = new Uri("file://path/test.razor");
        var documentContext = CreateDocumentContext(uri, codeDocument);

        var clientConnection = TestMocks.CreateClientConnection(builder =>
        {
            builder.SetupSendRequest<WrapWithTagParams, WrapWithTagResponse>(LanguageServerConstants.RazorWrapWithTagEndpoint, response: new(), verifiable: true);
        });

        var endpoint = new WrapWithTagEndpoint(clientConnection, LoggerFactory);

        var wrapWithDivParams = new WrapWithTagParams(new() { DocumentUri = new(uri) })
        {
            Range = LspFactory.CreateSingleLineRange(start: (0, 0), length: 8),
        };

        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(wrapWithDivParams, requestContext, DisposalToken);

        // Assert
        Assert.NotNull(result);
        Mock.Get(clientConnection).Verify();
    }

    [Fact]
    public async Task Handle_RazorBlockStart_ReturnsResult()
    {
        // Arrange
        var input = new TestCode("""
            [|@if (true) { }
            <div>
            </div>|]
            """);
        var codeDocument = CreateCodeDocument(input.Text);
        var uri = new Uri("file://path/test.razor");
        var documentContext = CreateDocumentContext(uri, codeDocument);
        var response = new WrapWithTagResponse();

        var clientConnection = TestMocks.CreateClientConnection(builder =>
        {
            builder.SetupSendRequest<WrapWithTagParams, WrapWithTagResponse>(LanguageServerConstants.RazorWrapWithTagEndpoint, response: new(), verifiable: true);
        });

        var endpoint = new WrapWithTagEndpoint(clientConnection, LoggerFactory);

        var range = codeDocument.Source.Text.GetRange(input.Span);
        var wrapWithDivParams = new WrapWithTagParams(new TextDocumentIdentifier { DocumentUri = new(uri) })
        {
            Range = range
        };
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(wrapWithDivParams, requestContext, DisposalToken);

        // Assert
        Assert.NotNull(result);
        Mock.Get(clientConnection).Verify();
    }

    [Fact]
    public async Task Handle_HtmlInCSharp()
    {
        // Arrange
        var input = new TestCode("""
            @if (true)
            {
                [|<p></p>|]
            }
            """);
        var codeDocument = CreateCodeDocument(input.Text);
        var uri = new Uri("file://path/test.razor");
        var documentContext = CreateDocumentContext(uri, codeDocument);
        var response = new WrapWithTagResponse();

        var clientConnection = TestMocks.CreateClientConnection(builder =>
        {
            builder.SetupSendRequest<WrapWithTagParams, WrapWithTagResponse>(LanguageServerConstants.RazorWrapWithTagEndpoint, response: new(), verifiable: true);
        });

        var endpoint = new WrapWithTagEndpoint(clientConnection, LoggerFactory);

        var range = codeDocument.Source.Text.GetRange(input.Span);
        var wrapWithDivParams = new WrapWithTagParams(new TextDocumentIdentifier { DocumentUri = new(uri) })
        {
            Range = range
        };
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(wrapWithDivParams, requestContext, DisposalToken);

        // Assert
        Assert.NotNull(result);
        Mock.Get(clientConnection).Verify();
    }

    [Fact]
    public async Task Handle_HtmlInCSharp_WithWhitespace()
    {
        // Arrange
        var input = new TestCode("""
            @if (true)
            {
               [| <p></p>|]
            }
            """);
        var codeDocument = CreateCodeDocument(input.Text);
        var uri = new Uri("file://path/test.razor");
        var documentContext = CreateDocumentContext(uri, codeDocument);
        var response = new WrapWithTagResponse();

        var clientConnection = TestMocks.CreateClientConnection(builder =>
        {
            builder.SetupSendRequest<WrapWithTagParams, WrapWithTagResponse>(LanguageServerConstants.RazorWrapWithTagEndpoint, response: new(), verifiable: true);
        });

        var endpoint = new WrapWithTagEndpoint(clientConnection, LoggerFactory);

        var range = codeDocument.Source.Text.GetRange(input.Span);
        var wrapWithDivParams = new WrapWithTagParams(new TextDocumentIdentifier { DocumentUri = new(uri) })
        {
            Range = range
        };
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(wrapWithDivParams, requestContext, DisposalToken);

        // Assert
        Assert.NotNull(result);
        Mock.Get(clientConnection).Verify();
    }

    [Fact]
    public async Task Handle_HtmlInCSharp_WithNewline()
    {
        // Arrange
        var input = new TestCode("""
            @if (true)
            {[|
                <p></p>|]
            }
            """);
        var codeDocument = CreateCodeDocument(input.Text);
        var uri = new Uri("file://path/test.razor");
        var documentContext = CreateDocumentContext(uri, codeDocument);
        var response = new WrapWithTagResponse();

        var clientConnection = TestMocks.CreateClientConnection(builder => { });

        var endpoint = new WrapWithTagEndpoint(clientConnection, LoggerFactory);

        var range = codeDocument.Source.Text.GetRange(input.Span);
        var wrapWithDivParams = new WrapWithTagParams(new TextDocumentIdentifier { DocumentUri = new(uri) })
        {
            Range = range
        };
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(wrapWithDivParams, requestContext, DisposalToken);

        // Assert
        Assert.Null(result);
        Mock.Get(clientConnection).Verify();
    }

    [Fact]
    public async Task Handle_CSharp_PartOfImplicitStatement_ReturnsNull()
    {
        // Arrange
        var codeDocument = CreateCodeDocument("@counter");
        var uri = new Uri("file://path/test.razor");
        var documentContext = CreateDocumentContext(uri, codeDocument);

        var clientConnection = TestMocks.CreateClientConnection(builder =>
        {
            builder.SetupSendRequest<WrapWithTagParams, WrapWithTagResponse>(LanguageServerConstants.RazorWrapWithTagEndpoint, response: new(), verifiable: true);
        });

        var endpoint = new WrapWithTagEndpoint(clientConnection, LoggerFactory);

        var wrapWithDivParams = new WrapWithTagParams(new() { DocumentUri = new(uri) })
        {
            Range = LspFactory.CreateSingleLineRange(line: 0, character: 2, length: 2),
        };

        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(wrapWithDivParams, requestContext, DisposalToken);

        // Assert
        Assert.Null(result);
        Mock.Get(clientConnection)
            .VerifySendRequest<WrapWithTagParams, WrapWithTagResponse>(LanguageServerConstants.RazorWrapWithTagEndpoint, Times.Never);
    }

    [Fact]
    public async Task Handle_CSharp_InImplicitStatement_ReturnsResult()
    {
        // Arrange
        var codeDocument = CreateCodeDocument("@counter");
        var uri = new Uri("file://path/test.razor");
        var documentContext = CreateDocumentContext(uri, codeDocument);

        var clientConnection = TestMocks.CreateClientConnection(builder =>
        {
            builder.SetupSendRequest<WrapWithTagParams, WrapWithTagResponse>(LanguageServerConstants.RazorWrapWithTagEndpoint, response: new(), verifiable: true);
        });

        var endpoint = new WrapWithTagEndpoint(clientConnection, LoggerFactory);

        var wrapWithDivParams = new WrapWithTagParams(new() { DocumentUri = new(uri) })
        {
            Range = LspFactory.CreateZeroWidthRange(0, 4),
        };

        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(wrapWithDivParams, requestContext, DisposalToken);

        // Assert
        Assert.NotNull(result);
        Mock.Get(clientConnection).Verify();
    }

    [Fact]
    public async Task Handle_DocumentNotFound_ReturnsNull()
    {
        // Arrange
        var missingUri = new Uri("file://path/nottest.razor");

        var clientConnection = TestMocks.CreateClientConnection(builder =>
        {
            builder.SetupSendRequest<WrapWithTagParams, WrapWithTagResponse>(LanguageServerConstants.RazorWrapWithTagEndpoint, response: new(), verifiable: true);
        });

        var endpoint = new WrapWithTagEndpoint(clientConnection, LoggerFactory);

        var wrapWithDivParams = new WrapWithTagParams(new() { DocumentUri = new(missingUri) })
        {
            Range = LspFactory.CreateSingleLineRange(start: (0, 0), length: 2),
        };

        var requestContext = CreateRazorRequestContext(documentContext: null);

        // Act
        var result = await endpoint.HandleRequestAsync(wrapWithDivParams, requestContext, DisposalToken);

        // Assert
        Assert.Null(result);
        Mock.Get(clientConnection)
            .VerifySendRequest<WrapWithTagParams, WrapWithTagResponse>(LanguageServerConstants.RazorWrapWithTagEndpoint, Times.Never);
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
        var inputSourceText = await context.GetSourceTextAsync(DisposalToken);

        var computedEdits = new TextEdit[]
        {
            LspFactory.CreateTextEdit(position: (0, 0), "<div>" + Environment.NewLine + "    "),
            LspFactory.CreateTextEdit(line: 1, character: 0, "    "),
            LspFactory.CreateTextEdit(
                range: LspFactory.CreateSingleLineRange(line: 2, character: 0, length: 1),
                newText: "    }" + Environment.NewLine + "</div>"),
        };

        var htmlSourceText = await context.GetHtmlSourceTextAsync(DisposalToken);
        var edits = FormattingUtilities.FixHtmlTextEdits(htmlSourceText, computedEdits);
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
        var inputSourceText = await context.GetSourceTextAsync(DisposalToken);

        var computedEdits = new TextEdit[]
        {
            LspFactory.CreateTextEdit(position: (0, 0), "<div>" + Environment.NewLine + "    "),
            LspFactory.CreateTextEdit(line: 1, character: 0, "    "),
            // This is the problematic edit.. the close brace has been replaced with a tilde
            LspFactory.CreateTextEdit(
                range: LspFactory.CreateSingleLineRange(line: 2, character: 0, length: 1),
                newText: "    ~" + Environment.NewLine + "</div>")
        };

        var htmlSourceText = await context.GetHtmlSourceTextAsync(DisposalToken);
        var edits = FormattingUtilities.FixHtmlTextEdits(htmlSourceText, computedEdits);
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
            LspFactory.CreateTextEdit(position: (0, 0), "<div>" + Environment.NewLine + "    "),
            LspFactory.CreateTextEdit(line: 1, character: 0, "    "),
            // This looks like a bad edit, but the original source document had a tilde
            LspFactory.CreateTextEdit(
                range: LspFactory.CreateSingleLineRange(line: 2, character: 0, length: 1),
                newText: "    ~" + Environment.NewLine + "</div>")
        };

        var htmlSourceText = await context.GetHtmlSourceTextAsync(DisposalToken);
        var edits = FormattingUtilities.FixHtmlTextEdits(htmlSourceText, computedEdits);
        Assert.NotSame(computedEdits, edits);

        var finalText = inputSourceText.WithChanges(edits.Select(inputSourceText.GetTextChange));
        Assert.Equal(expected, finalText.ToString());
    }
}
