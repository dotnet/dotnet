// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Common.Extensions;
using Microsoft.AspNetCore.Razor.LanguageServer.Debugging;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.AspNetCore.Razor.LanguageServer.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Test.Debugging;

public class RazorBreakpointSpanEndpointTest : LanguageServerTestBase
{
    private readonly RazorDocumentMappingService _mappingService;

    public RazorBreakpointSpanEndpointTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        _mappingService = new DefaultRazorDocumentMappingService(
            TestLanguageServerFeatureOptions.Instance,
            new TestDocumentContextFactory(),
            LoggerFactory);
    }

    [Fact]
    public void GetMappingBehavior_CSHTML()
    {
        // Arrange
        var documentPath = new Uri("C:/path/to/document.cshtml");
        var documentContext = TestDocumentContext.Create(documentPath);

        // Act
        var result = RazorBreakpointSpanEndpoint.GetMappingBehavior(documentContext);

        // Assert
        Assert.Equal(MappingBehavior.Inclusive, result);
    }

    [Fact]
    public void GetMappingBehavior_Razor()
    {
        // Arrange
        var documentPath = new Uri("C:/path/to/document.razor");
        var documentContext = TestDocumentContext.Create(documentPath);

        // Act
        var result = RazorBreakpointSpanEndpoint.GetMappingBehavior(documentContext);

        // Assert
        Assert.Equal(MappingBehavior.Strict, result);
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

    private static RazorCodeDocument CreateCodeDocument(string text)
    {
        var sourceDocument = TestRazorSourceDocument.Create(text);
        var projectEngine = RazorProjectEngine.Create(builder => { });
        var codeDocument = projectEngine.ProcessDesignTime(sourceDocument, FileKinds.Legacy, Array.Empty<RazorSourceDocument>(), Array.Empty<TagHelperDescriptor>());
        return codeDocument;
    }
}
