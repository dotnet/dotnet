// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.AspNetCore.Razor.LanguageServer.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Debugging;

public class RazorBreakpointSpanEndpointTest : LanguageServerTestBase
{
    private readonly IRazorDocumentMappingService _mappingService;

    public RazorBreakpointSpanEndpointTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        _mappingService = new RazorDocumentMappingService(
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
            Position = new Position(1, 0)
        };
        codeDocument.SetUnsupported();
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var response = await diagnosticsEndpoint.HandleRequestAsync(request, requestContext, default);

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
            Position = new Position(1, 0)
        };
        var expectedRange = new Range { Start = new Position(1, 5), End = new Position(1, 19) };
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var response = await diagnosticsEndpoint.HandleRequestAsync(request, requestContext, default);

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
            Position = new Position(1, 0)
        };
        var expectedRange = new Range { Start = new Position(1, 4), End = new Position(1, 16) };
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var response = await diagnosticsEndpoint.HandleRequestAsync(request, requestContext, default);

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
            Position = new Position(1, 0)
        };
        var expectedRange = new Range { Start = new Position(1, 5), End = new Position(1, 19) };
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var response = await diagnosticsEndpoint.HandleRequestAsync(request, requestContext, default);

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
            Position = new Position(1, 0)
        };
        var expectedRange = new Range { Start = new Position(1, 4), End = new Position(1, 16) };
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var response = await diagnosticsEndpoint.HandleRequestAsync(request, requestContext, default);

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
            Position = new Position(1, 0)
        };
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var response = await diagnosticsEndpoint.HandleRequestAsync(request, requestContext, default);

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
            Position = new Position(1, 0)
        };
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var response = await diagnosticsEndpoint.HandleRequestAsync(request, requestContext, default);

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
            Position = new Position(1, 0)
        };
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var response = await diagnosticsEndpoint.HandleRequestAsync(request, requestContext, default);

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
            Position = new Position(2, 0)
        };
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var response = await diagnosticsEndpoint.HandleRequestAsync(request, requestContext, default);

        // Assert
        Assert.Null(response);
    }

    private static RazorCodeDocument CreateCodeDocument(string text, string? fileKind = null)
    {
        var sourceDocument = TestRazorSourceDocument.Create(text);
        var projectEngine = RazorProjectEngine.Create(builder => { });
        var codeDocument = projectEngine.ProcessDesignTime(sourceDocument, fileKind ?? FileKinds.Legacy, Array.Empty<RazorSourceDocument>(), Array.Empty<TagHelperDescriptor>());
        return codeDocument;
    }
}
