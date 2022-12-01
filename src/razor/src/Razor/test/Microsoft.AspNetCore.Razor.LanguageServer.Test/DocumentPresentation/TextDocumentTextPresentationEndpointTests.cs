// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.Common.Extensions;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.DocumentPresentation;

public class TextDocumentTextPresentationEndpointTests : LanguageServerTestBase
{
    public TextDocumentTextPresentationEndpointTests(ITestOutputHelper testOutput)
        : base(testOutput)
    {
    }

    [Fact]
    public async Task Handle_Html_MakesRequest()
    {
        // Arrange
        var codeDocument = TestRazorCodeDocument.Create("<div></div>");
        var documentMappingService = Mock.Of<RazorDocumentMappingService>(
            s => s.GetLanguageKind(codeDocument, It.IsAny<int>(), It.IsAny<bool>()) == RazorLanguageKind.Html, MockBehavior.Strict);

        var uri = new Uri("file://path/test.razor");
        var documentContext = CreateDocumentContext(uri, codeDocument);

        var response = (WorkspaceEdit?)null;

        var languageServer = new Mock<ClientNotifierServiceBase>(MockBehavior.Strict);
        languageServer
            .Setup(l => l.SendRequestAsync<IRazorPresentationParams, WorkspaceEdit?>(RazorLanguageServerCustomMessageTargets.RazorTextPresentationEndpoint, It.IsAny<IRazorPresentationParams>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var endpoint = new TextDocumentTextPresentationEndpoint(
            documentMappingService,
            languageServer.Object,
            TestLanguageServerFeatureOptions.Instance);

        var parameters = new TextPresentationParams()
        {
            TextDocument = new TextDocumentIdentifier
            {
                Uri = uri
            },
            Range = new Range
            {
                Start = new Position(0, 1),
                End = new Position(0, 2)
            },
            Text = "Hi there"
        };
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(parameters, requestContext, DisposalToken);

        // Assert
        languageServer.Verify();
    }

    [Fact]
    public async Task Handle_CSharp_MakesRequest()
    {
        // Arrange
        var codeDocument = TestRazorCodeDocument.Create("@counter");
        var uri = new Uri("file://path/test.razor");
        var documentContext = CreateDocumentContext(uri, codeDocument);
        var projectedRange = It.IsAny<Range>();
        var documentMappingService = Mock.Of<RazorDocumentMappingService>(
            s => s.GetLanguageKind(codeDocument, It.IsAny<int>(), It.IsAny<bool>()) == RazorLanguageKind.CSharp &&
            s.TryMapToProjectedDocumentRange(codeDocument, It.IsAny<Range>(), out projectedRange) == true, MockBehavior.Strict);

        var response = (WorkspaceEdit?)null;

        var languageServer = new Mock<ClientNotifierServiceBase>(MockBehavior.Strict);
        languageServer
            .Setup(l => l.SendRequestAsync<IRazorPresentationParams, WorkspaceEdit?>(RazorLanguageServerCustomMessageTargets.RazorTextPresentationEndpoint, It.IsAny<IRazorPresentationParams>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var endpoint = new TextDocumentTextPresentationEndpoint(
            documentMappingService,
            languageServer.Object,
            TestLanguageServerFeatureOptions.Instance);

        var parameters = new TextPresentationParams()
        {
            TextDocument = new TextDocumentIdentifier
            {
                Uri = uri
            },
            Range = new Range
            {
                Start = new Position(0, 1),
                End = new Position(0, 2)
            },
            Text = "Hi there"
        };
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(parameters, requestContext, DisposalToken);

        // Assert
        languageServer.Verify();
    }

    [Fact]
    public async Task Handle_DocumentNotFound_ReturnsNull()
    {
        // Arrange
        var codeDocument = TestRazorCodeDocument.Create("<div></div>");
        var uri = new Uri("file://path/test.razor");
        var documentContext = CreateDocumentContext(uri, codeDocument);
        var documentMappingService = Mock.Of<RazorDocumentMappingService>(
            s => s.GetLanguageKind(codeDocument, It.IsAny<int>(), It.IsAny<bool>()) == RazorLanguageKind.Html, MockBehavior.Strict);

        var response = (WorkspaceEdit?)null;

        var languageServer = new Mock<ClientNotifierServiceBase>(MockBehavior.Strict);
        languageServer
            .Setup(l => l.SendRequestAsync<IRazorPresentationParams, WorkspaceEdit?>(RazorLanguageServerCustomMessageTargets.RazorTextPresentationEndpoint, It.IsAny<IRazorPresentationParams>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var endpoint = new TextDocumentTextPresentationEndpoint(
            documentMappingService,
            languageServer.Object,
            TestLanguageServerFeatureOptions.Instance);

        var parameters = new TextPresentationParams()
        {
            TextDocument = new TextDocumentIdentifier
            {
                Uri = uri
            },
            Range = new Range
            {
                Start = new Position(0, 1),
                End = new Position(0, 2)
            },
            Text = "Hi there"
        };
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(parameters, requestContext, DisposalToken);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Handle_UnsupportedCodeDocument_ReturnsNull()
    {
        // Arrange
        var codeDocument = TestRazorCodeDocument.Create("<div></div>");
        codeDocument.SetUnsupported();
        var uri = new Uri("file://path/test.razor");
        var documentContext = CreateDocumentContext(uri, codeDocument);
        var documentMappingService = Mock.Of<RazorDocumentMappingService>(
            s => s.GetLanguageKind(codeDocument, It.IsAny<int>(), It.IsAny<bool>()) == RazorLanguageKind.Html, MockBehavior.Strict);

        var response = new WorkspaceEdit();

        var languageServer = new Mock<ClientNotifierServiceBase>(MockBehavior.Strict);
        languageServer
            .Setup(l => l.SendRequestAsync<IRazorPresentationParams, WorkspaceEdit?>(RazorLanguageServerCustomMessageTargets.RazorTextPresentationEndpoint, It.IsAny<IRazorPresentationParams>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var endpoint = new TextDocumentTextPresentationEndpoint(
            documentMappingService,
            languageServer.Object,
            TestLanguageServerFeatureOptions.Instance);

        var parameters = new TextPresentationParams()
        {
            TextDocument = new TextDocumentIdentifier
            {
                Uri = uri
            },
            Range = new Range
            {
                Start = new Position(0, 1),
                End = new Position(0, 2)
            },
            Text = "Hi there"
        };
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(parameters, requestContext, DisposalToken);

        // Assert
        Assert.Null(result);
    }
}
