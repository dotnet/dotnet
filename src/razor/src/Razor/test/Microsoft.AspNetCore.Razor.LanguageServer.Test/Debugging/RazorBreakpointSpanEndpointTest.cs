// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.Protocol.Debugging;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Debugging;

public class RazorBreakpointSpanEndpointTest : LanguageServerTestBase
{
    private readonly IDocumentMappingService _mappingService;

    public RazorBreakpointSpanEndpointTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        _mappingService = new LspDocumentMappingService(
            FilePathService,
            new TestDocumentContextFactory(),
            LoggerFactory);
    }

    [Fact]
    public async Task Handle_StartsInHtml_BreakpointMoved()
    {
        // Arrange
        var documentPath = new Uri("C:/path/to/document.cshtml");
        var codeDocument = CreateCodeDocument(@"
<p>@{var abc = 123;}</p>");
        var documentContext = CreateDocumentContext(documentPath, codeDocument);

        var diagnosticsEndpoint = new RazorBreakpointSpanEndpoint(_mappingService, LoggerFactory);
        var request = new RazorBreakpointSpanParams()
        {
            Uri = documentPath,
            Position = LspFactory.CreatePosition(1, 0),
            HostDocumentSyncVersion = 1,
        };
        var expectedRange = LspFactory.CreateSingleLineRange(line: 1, character: 5, length: 14);
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var response = await diagnosticsEndpoint.HandleRequestAsync(request, requestContext, DisposalToken);

        // Assert
        Assert.Equal(expectedRange, response!.Range);
    }

    [Fact]
    public async Task Handle_ImplicitExpression_StartsInHtml_BreakpointMoved()
    {
        // Arrange
        var documentPath = new Uri("C:/path/to/document.cshtml");
        var codeDocument = CreateCodeDocument(@"
<p>@currentCount</p>");
        var documentContext = CreateDocumentContext(documentPath, codeDocument);

        var diagnosticsEndpoint = new RazorBreakpointSpanEndpoint(_mappingService, LoggerFactory);
        var request = new RazorBreakpointSpanParams()
        {
            Uri = documentPath,
            Position = LspFactory.CreatePosition(1, 0),
            HostDocumentSyncVersion = 1,
        };
        var expectedRange = LspFactory.CreateSingleLineRange(line: 1, character: 4, length: 12);
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var response = await diagnosticsEndpoint.HandleRequestAsync(request, requestContext, DisposalToken);

        // Assert
        Assert.Equal(expectedRange, response!.Range);
    }

    [Fact]
    public async Task Handle_StartsInHtml_BreakpointMoved_Razor()
    {
        // Arrange
        var documentPath = new Uri("C:/path/to/document.razor");
        var codeDocument = CreateCodeDocument(@"
<p>@{var abc = 123;}</p>", RazorFileKind.Component);
        var documentContext = CreateDocumentContext(documentPath, codeDocument);

        var diagnosticsEndpoint = new RazorBreakpointSpanEndpoint(_mappingService, LoggerFactory);
        var request = new RazorBreakpointSpanParams()
        {
            Uri = documentPath,
            Position = LspFactory.CreatePosition(1, 0),
            HostDocumentSyncVersion = 1,
        };
        var expectedRange = LspFactory.CreateSingleLineRange(line: 1, character: 5, length: 14);
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var response = await diagnosticsEndpoint.HandleRequestAsync(request, requestContext, DisposalToken);

        // Assert
        Assert.Equal(expectedRange, response!.Range);
    }

    [Fact]
    public async Task Handle_ImplicitExpression_StartsInHtml_BreakpointMoved_Razor()
    {
        // Arrange
        var documentPath = new Uri("C:/path/to/document.razor");
        var codeDocument = CreateCodeDocument(@"
<p>@currentCount</p>", RazorFileKind.Component);
        var documentContext = CreateDocumentContext(documentPath, codeDocument);

        var diagnosticsEndpoint = new RazorBreakpointSpanEndpoint(_mappingService, LoggerFactory);
        var request = new RazorBreakpointSpanParams()
        {
            Uri = documentPath,
            Position = LspFactory.CreatePosition(1, 0),
            HostDocumentSyncVersion = 1,
        };
        var expectedRange = LspFactory.CreateSingleLineRange(line: 1, character: 4, length: 12);
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var response = await diagnosticsEndpoint.HandleRequestAsync(request, requestContext, DisposalToken);

        // Assert
        Assert.Equal(expectedRange, response!.Range);
    }

    [Fact]
    public async Task Handle_StartsInHtml_InvalidBreakpointSpan_ReturnsNull()
    {
        // Arrange
        var documentPath = new Uri("C:/path/to/document.cshtml");

        var codeDocument = CreateCodeDocument(@"
<p>@{var abc;}</p>");
        var documentContext = CreateDocumentContext(documentPath, codeDocument);

        var diagnosticsEndpoint = new RazorBreakpointSpanEndpoint(_mappingService, LoggerFactory);
        var request = new RazorBreakpointSpanParams()
        {
            Uri = documentPath,
            Position = LspFactory.CreatePosition(1, 0),
            HostDocumentSyncVersion = 1,
        };
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var response = await diagnosticsEndpoint.HandleRequestAsync(request, requestContext, DisposalToken);

        // Assert
        Assert.Null(response);
    }

    [Fact]
    public async Task Handle_StartInHtml_NoCSharpOnLine_ReturnsNull()
    {
        // Arrange
        var documentPath = new Uri("C:/path/to/document.cshtml");
        var codeDocument = CreateCodeDocument(@"
<p></p>");
        var documentContext = CreateDocumentContext(documentPath, codeDocument);

        var diagnosticsEndpoint = new RazorBreakpointSpanEndpoint(_mappingService, LoggerFactory);
        var request = new RazorBreakpointSpanParams()
        {
            Uri = documentPath,
            Position = LspFactory.CreatePosition(1, 0),
            HostDocumentSyncVersion = 0,
        };
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var response = await diagnosticsEndpoint.HandleRequestAsync(request, requestContext, DisposalToken);

        // Assert
        Assert.Null(response);
    }

    [Fact]
    public async Task Handle_StartInHtml_NoActualCSharp_ReturnsNull()
    {
        // Arrange
        var documentPath = new Uri("C:/path/to/document.cshtml");
        var codeDocument = CreateCodeDocument(
            @"
<p>@{
    var abc = 123;
}</p>");
        var documentContext = CreateDocumentContext(documentPath, codeDocument);

        var diagnosticsEndpoint = new RazorBreakpointSpanEndpoint(_mappingService, LoggerFactory);
        var request = new RazorBreakpointSpanParams()
        {
            Uri = documentPath,
            Position = LspFactory.CreatePosition(1, 0),
            HostDocumentSyncVersion = 0,
        };
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var response = await diagnosticsEndpoint.HandleRequestAsync(request, requestContext, DisposalToken);

        // Assert
        Assert.Null(response);
    }

    [Fact]
    public async Task Handle_InvalidBreakpointSpan_ReturnsNull()
    {
        // Arrange
        var documentPath = new Uri("C:/path/to/document.cshtml");
        var codeDocument = CreateCodeDocument(@"
<p>@{

    var abc = 123;
}</p>");
        var documentContext = CreateDocumentContext(documentPath, codeDocument);

        var diagnosticsEndpoint = new RazorBreakpointSpanEndpoint(_mappingService, LoggerFactory);
        var request = new RazorBreakpointSpanParams()
        {
            Uri = documentPath,
            Position = LspFactory.CreatePosition(2, 0),
            HostDocumentSyncVersion = 0,
        };
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var response = await diagnosticsEndpoint.HandleRequestAsync(request, requestContext, DisposalToken);

        // Assert
        Assert.Null(response);
    }

    private static RazorCodeDocument CreateCodeDocument(string text, RazorFileKind? fileKind = null)
    {
        var sourceDocument = TestRazorSourceDocument.Create(text);
        var projectEngine = RazorProjectEngine.Create(builder =>
        {
            builder.ConfigureParserOptions(builder =>
            {
                builder.UseRoslynTokenizer = true;
            });
        });

        return projectEngine.ProcessDesignTime(sourceDocument, fileKind ?? RazorFileKind.Legacy, importSources: default, tagHelpers: []);
    }
}
