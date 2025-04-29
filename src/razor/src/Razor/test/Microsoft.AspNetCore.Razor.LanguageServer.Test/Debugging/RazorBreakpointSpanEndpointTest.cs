// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.Protocol.Debugging;
using Microsoft.VisualStudio.LanguageServer.Protocol;
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
    public async Task Handle_UnsupportedDocument_ReturnsNull()
    {
        // Arrange
        var documentPath = new Uri("C:/path/to/document.cshtml");
        var codeDocument = CreateCodeDocument(@"
<p>@DateTime.Now</p>");
        var documentContext = CreateDocumentContext(documentPath, codeDocument);

        var diagnosticsEndpoint = new RazorBreakpointSpanEndpoint(_mappingService, LoggerFactory);
        var request = new RazorBreakpointSpanParams()
        {
            Uri = documentPath,
            Position = VsLspFactory.CreatePosition(1, 0),
            HostDocumentSyncVersion = 0,
        };
        codeDocument.SetUnsupported();
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var response = await diagnosticsEndpoint.HandleRequestAsync(request, requestContext, DisposalToken);

        // Assert
        Assert.Null(response);
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
            Position = VsLspFactory.CreatePosition(1, 0),
            HostDocumentSyncVersion = 1,
        };
        var expectedRange = VsLspFactory.CreateSingleLineRange(line: 1, character: 5, length: 14);
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
            Position = VsLspFactory.CreatePosition(1, 0),
            HostDocumentSyncVersion = 1,
        };
        var expectedRange = VsLspFactory.CreateSingleLineRange(line: 1, character: 4, length: 12);
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
<p>@{var abc = 123;}</p>", FileKinds.Component);
        var documentContext = CreateDocumentContext(documentPath, codeDocument);

        var diagnosticsEndpoint = new RazorBreakpointSpanEndpoint(_mappingService, LoggerFactory);
        var request = new RazorBreakpointSpanParams()
        {
            Uri = documentPath,
            Position = VsLspFactory.CreatePosition(1, 0),
            HostDocumentSyncVersion = 1,
        };
        var expectedRange = VsLspFactory.CreateSingleLineRange(line: 1, character: 5, length: 14);
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
<p>@currentCount</p>", FileKinds.Component);
        var documentContext = CreateDocumentContext(documentPath, codeDocument);

        var diagnosticsEndpoint = new RazorBreakpointSpanEndpoint(_mappingService, LoggerFactory);
        var request = new RazorBreakpointSpanParams()
        {
            Uri = documentPath,
            Position = VsLspFactory.CreatePosition(1, 0),
            HostDocumentSyncVersion = 1,
        };
        var expectedRange = VsLspFactory.CreateSingleLineRange(line: 1, character: 4, length: 12);
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
            Position = VsLspFactory.CreatePosition(1, 0),
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
            Position = VsLspFactory.CreatePosition(1, 0),
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
            Position = VsLspFactory.CreatePosition(1, 0),
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
            Position = VsLspFactory.CreatePosition(2, 0),
            HostDocumentSyncVersion = 0,
        };
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var response = await diagnosticsEndpoint.HandleRequestAsync(request, requestContext, DisposalToken);

        // Assert
        Assert.Null(response);
    }

    private static RazorCodeDocument CreateCodeDocument(string text, string? fileKind = null)
    {
        var sourceDocument = TestRazorSourceDocument.Create(text);
        var projectEngine = RazorProjectEngine.Create(builder =>
        {
            builder.ConfigureParserOptions(builder =>
            {
                builder.UseRoslynTokenizer = true;
            });
        });
        var codeDocument = projectEngine.ProcessDesignTime(sourceDocument, fileKind ?? FileKinds.Legacy, importSources: default, tagHelpers: []);
        return codeDocument;
    }
}
