// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.AspNetCore.Razor.LanguageServer.Formatting;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.WrapWithTag;

public class WrapWithTagEndpointTest : LanguageServerTestBase
{
    public WrapWithTagEndpointTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
    }

    [Fact]
    public async Task Handle_Html_ReturnsResult()
    {
        // Arrange
        var codeDocument = CreateCodeDocument("<div></div>");
        var uri = new Uri("file://path/test.razor");
        var documentContext = CreateDocumentContext(uri, codeDocument);
        var response = new WrapWithTagResponse();

        var languageServer = new Mock<ClientNotifierServiceBase>(MockBehavior.Strict);
        languageServer
            .Setup(l => l.SendRequestAsync<WrapWithTagParams, WrapWithTagResponse>(LanguageServerConstants.RazorWrapWithTagEndpoint, It.IsAny<WrapWithTagParams>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var documentMappingService = Mock.Of<IRazorDocumentMappingService>(
            s => s.GetLanguageKind(codeDocument, It.IsAny<int>(), It.IsAny<bool>()) == RazorLanguageKind.Html, MockBehavior.Strict);
        var endpoint = new WrapWithTagEndpoint(
            languageServer.Object,
            documentMappingService);

        var wrapWithDivParams = new WrapWithTagParams(new TextDocumentIdentifier { Uri = uri })
        {
            Range = new Range { Start = new Position(0, 0), End = new Position(0, 2) },
        };
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(wrapWithDivParams, requestContext, DisposalToken);

        // Assert
        Assert.NotNull(result);
        languageServer.Verify();
    }

    [Fact]
    public async Task Handle_CSharp_ReturnsNull()
    {
        // Arrange
        var codeDocument = CreateCodeDocument("@(counter)");
        var uri = new Uri("file://path/test.razor");
        var documentContext = CreateDocumentContext(uri, codeDocument);
        var response = new WrapWithTagResponse();

        var languageServer = new Mock<ClientNotifierServiceBase>(MockBehavior.Strict);
        languageServer
            .Setup(l => l.SendRequestAsync<WrapWithTagParams, WrapWithTagResponse>(LanguageServerConstants.RazorWrapWithTagEndpoint, It.IsAny<WrapWithTagParams>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var documentMappingService = Mock.Of<IRazorDocumentMappingService>(
            s => s.GetLanguageKind(codeDocument, It.IsAny<int>(), It.IsAny<bool>()) == RazorLanguageKind.CSharp, MockBehavior.Strict);
        var endpoint = new WrapWithTagEndpoint(
            languageServer.Object,
            documentMappingService);

        var wrapWithDivParams = new WrapWithTagParams(new TextDocumentIdentifier { Uri = uri })
        {
            Range = new Range { Start = new Position(0, 0), End = new Position(0, 2) },
        };
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(wrapWithDivParams, requestContext, DisposalToken);

        // Assert
        Assert.Null(result);
        languageServer.Verify();
    }

    [Fact]
    public async Task Handle_CSharp_WholeImplicitStatement_ReturnsResult()
    {
        // Arrange
        var codeDocument = CreateCodeDocument("@counter");
        var uri = new Uri("file://path/test.razor");
        var documentContext = CreateDocumentContext(uri, codeDocument);
        var response = new WrapWithTagResponse();

        var languageServer = new Mock<ClientNotifierServiceBase>(MockBehavior.Strict);
        languageServer
            .Setup(l => l.SendRequestAsync<WrapWithTagParams, WrapWithTagResponse>(LanguageServerConstants.RazorWrapWithTagEndpoint, It.IsAny<WrapWithTagParams>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var documentMappingService = Mock.Of<IRazorDocumentMappingService>(
            s => s.GetLanguageKind(codeDocument, It.IsAny<int>(), It.IsAny<bool>()) == RazorLanguageKind.CSharp, MockBehavior.Strict);
        var endpoint = new WrapWithTagEndpoint(
            languageServer.Object,
            documentMappingService);

        var wrapWithDivParams = new WrapWithTagParams(new TextDocumentIdentifier { Uri = uri })
        {
            Range = new Range { Start = new Position(0, 0), End = new Position(0, 8) },
        };
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(wrapWithDivParams, requestContext, DisposalToken);

        // Assert
        Assert.NotNull(result);
        languageServer.Verify();
    }

    [Fact]
    public async Task Handle_CSharp_PartOfImplicitStatement_ReturnsNull()
    {
        // Arrange
        var codeDocument = CreateCodeDocument("@counter");
        var uri = new Uri("file://path/test.razor");
        var documentContext = CreateDocumentContext(uri, codeDocument);
        var response = new WrapWithTagResponse();

        var languageServer = new Mock<ClientNotifierServiceBase>(MockBehavior.Strict);
        languageServer
            .Setup(l => l.SendRequestAsync<WrapWithTagParams, WrapWithTagResponse>(LanguageServerConstants.RazorWrapWithTagEndpoint, It.IsAny<WrapWithTagParams>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var documentMappingService = Mock.Of<IRazorDocumentMappingService>(
            s => s.GetLanguageKind(codeDocument, It.IsAny<int>(), It.IsAny<bool>()) == RazorLanguageKind.CSharp, MockBehavior.Strict);
        var endpoint = new WrapWithTagEndpoint(
            languageServer.Object,
            documentMappingService);

        var wrapWithDivParams = new WrapWithTagParams(new TextDocumentIdentifier { Uri = uri })
        {
            Range = new Range { Start = new Position(0, 2), End = new Position(0, 4) },
        };
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(wrapWithDivParams, requestContext, DisposalToken);

        // Assert
        Assert.Null(result);
        languageServer.Verify();
    }

    [Fact]
    public async Task Handle_CSharp_InImplicitStatement_ReturnsResult()
    {
        // Arrange
        var codeDocument = CreateCodeDocument("@counter");
        var uri = new Uri("file://path/test.razor");
        var documentContext = CreateDocumentContext(uri, codeDocument);
        var response = new WrapWithTagResponse();

        var languageServer = new Mock<ClientNotifierServiceBase>(MockBehavior.Strict);
        languageServer
            .Setup(l => l.SendRequestAsync<WrapWithTagParams, WrapWithTagResponse>(LanguageServerConstants.RazorWrapWithTagEndpoint, It.IsAny<WrapWithTagParams>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var documentMappingService = Mock.Of<IRazorDocumentMappingService>(
            s => s.GetLanguageKind(codeDocument, It.IsAny<int>(), It.IsAny<bool>()) == RazorLanguageKind.CSharp, MockBehavior.Strict);
        var endpoint = new WrapWithTagEndpoint(
            languageServer.Object,
            documentMappingService);

        var wrapWithDivParams = new WrapWithTagParams(new TextDocumentIdentifier { Uri = uri })
        {
            Range = new Range { Start = new Position(0, 4), End = new Position(0, 4) },
        };
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(wrapWithDivParams, requestContext, DisposalToken);

        // Assert
        Assert.NotNull(result);
        languageServer.Verify();
    }

    [Fact]
    public async Task Handle_DocumentNotFound_ReturnsNull()
    {
        // Arrange
        var codeDocument = CreateCodeDocument("<div></div>");
        var realUri = new Uri("file://path/test.razor");
        var missingUri = new Uri("file://path/nottest.razor");

        var languageServer = new Mock<ClientNotifierServiceBase>(MockBehavior.Strict);

        var documentMappingService = Mock.Of<IRazorDocumentMappingService>(
            s => s.GetLanguageKind(codeDocument, It.IsAny<int>(), It.IsAny<bool>()) == RazorLanguageKind.Html, MockBehavior.Strict);
        var endpoint = new WrapWithTagEndpoint(languageServer.Object, documentMappingService);

        var wrapWithDivParams = new WrapWithTagParams(new TextDocumentIdentifier { Uri = missingUri })
        {
            Range = new Range { Start = new Position(0, 0), End = new Position(0, 2) },
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

        var languageServer = new Mock<ClientNotifierServiceBase>(MockBehavior.Strict);

        var documentMappingService = Mock.Of<IRazorDocumentMappingService>(
            s => s.GetLanguageKind(codeDocument, It.IsAny<int>(), It.IsAny<bool>()) == RazorLanguageKind.Html, MockBehavior.Strict);
        var endpoint = new WrapWithTagEndpoint(languageServer.Object, documentMappingService);

        var wrapWithDivParams = new WrapWithTagParams(new TextDocumentIdentifier { Uri = uri })
        {
            Range = new Range { Start = new Position(0, 0), End = new Position(0, 2) },
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
        var context = factory.TryCreate(uri);
        Assert.NotNull(context);
        var inputSourceText = await context!.GetSourceTextAsync(DisposalToken);

        var computedEdits = new TextEdit[]
        {
                new TextEdit
                {
                    NewText="<div>\r\n    ",
                    Range = new Range { Start= new Position(0, 0), End = new Position(0, 0) }
                },
                new TextEdit
                {
                    NewText="    ",
                    Range = new Range { Start= new Position(1, 0), End = new Position(1, 0) }
                },
                new TextEdit
                {
                    NewText="    }\r\n</div>",
                    Range = new Range { Start= new Position(2, 0), End = new Position(2, 1) }
                }
        };

        var htmlSourceText = await context!.GetHtmlSourceTextAsync(DisposalToken);
        var edits = HtmlFormatter.FixHtmlTestEdits(htmlSourceText, computedEdits);
        Assert.Same(computedEdits, edits);

        var finalText = inputSourceText.WithChanges(edits.Select(e => e.ToTextChange(inputSourceText)));
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
        var context = factory.TryCreate(uri);
        Assert.NotNull(context);
        var inputSourceText = await context!.GetSourceTextAsync(DisposalToken);

        var computedEdits = new TextEdit[]
        {
                new TextEdit
                {
                    NewText="<div>\r\n    ",
                    Range = new Range { Start= new Position(0, 0), End = new Position(0, 0) }
                },
                new TextEdit
                {
                    NewText="    ",
                    Range = new Range { Start= new Position(1, 0), End = new Position(1, 0) }
                },
                new TextEdit
                {
                    // This is the problematic edit.. the close brace has been replaced with a tilde
                    NewText="    ~\r\n</div>",
                    Range = new Range { Start= new Position(2, 0), End = new Position(2, 1) }
                }
        };

        var htmlSourceText = await context!.GetHtmlSourceTextAsync(DisposalToken);
        var edits = HtmlFormatter.FixHtmlTestEdits(htmlSourceText, computedEdits);
        Assert.NotSame(computedEdits, edits);

        var finalText = inputSourceText.WithChanges(edits.Select(e => e.ToTextChange(inputSourceText)));
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
        var context = factory.TryCreate(uri);
        Assert.NotNull(context);
        var inputSourceText = await context!.GetSourceTextAsync(DisposalToken);

        var computedEdits = new TextEdit[]
        {
                new TextEdit
                {
                    NewText="<div>\r\n    ",
                    Range = new Range { Start= new Position(0, 0), End = new Position(0, 0) }
                },
                new TextEdit
                {
                    NewText="    ",
                    Range = new Range { Start= new Position(1, 0), End = new Position(1, 0) }
                },
                new TextEdit
                {
                    // This looks like a bad edit, but the original source document had a tilde
                    NewText="    ~\r\n</div>",
                    Range = new Range { Start= new Position(2, 0), End = new Position(2, 1) }
                }
        };

        var htmlSourceText = await context!.GetHtmlSourceTextAsync(DisposalToken);
        var edits = HtmlFormatter.FixHtmlTestEdits(htmlSourceText, computedEdits);
        Assert.NotSame(computedEdits, edits);

        var finalText = inputSourceText.WithChanges(edits.Select(e => e.ToTextChange(inputSourceText)));
        Assert.Equal(expected, finalText.ToString());
    }
}
