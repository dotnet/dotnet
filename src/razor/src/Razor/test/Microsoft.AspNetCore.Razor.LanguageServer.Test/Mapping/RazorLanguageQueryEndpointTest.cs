// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Mapping;

public class RazorLanguageQueryEndpointTest : LanguageServerTestBase
{
    private readonly IDocumentMappingService _documentMappingService;

    public RazorLanguageQueryEndpointTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        _documentMappingService = new LspDocumentMappingService(
            FilePathService,
            new TestDocumentContextFactory(),
            LoggerFactory);
    }

    [Fact]
    public async Task Handle_ResolvesLanguageRequest_Razor()
    {
        // Arrange
        var documentPath = new Uri("C:/path/to/document.cshtml");
        var codeDocument = CreateCodeDocument("@{}");
        var documentContext = CreateDocumentContext(documentPath, codeDocument);
        var languageEndpoint = new RazorLanguageQueryEndpoint(_documentMappingService, LoggerFactory);
        var request = new RazorLanguageQueryParams()
        {
            Uri = documentPath,
            Position = VsLspFactory.CreatePosition(0, 1),
        };

        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var response = await languageEndpoint.HandleRequestAsync(request, requestContext, DisposalToken);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(RazorLanguageKind.Razor, response.Kind);
        Assert.Equal(request.Position, response.Position);
    }

    // This is more of an integration test to validate that all the pieces work together
    [Fact]
    public async Task Handle_ResolvesLanguageRequest_Html()
    {
        // Arrange
        var documentPath = new Uri("C:/path/to/document.cshtml");
        var codeDocument = CreateCodeDocument("<s");
        var documentContext = CreateDocumentContext(documentPath, codeDocument);
        var languageEndpoint = new RazorLanguageQueryEndpoint(_documentMappingService, LoggerFactory);
        var request = new RazorLanguageQueryParams()
        {
            Uri = documentPath,
            Position = VsLspFactory.CreatePosition(0, 2),
        };

        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var response = await languageEndpoint.HandleRequestAsync(request, requestContext, DisposalToken);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(RazorLanguageKind.Html, response.Kind);
        Assert.Equal(request.Position, response.Position);
    }

    // This is more of an integration test to validate that all the pieces work together
    [Fact]
    public async Task Handle_ResolvesLanguageRequest_CSharp()
    {
        // Arrange
        var documentPath = new Uri("C:/path/to/document.cshtml");
        var codeDocument = CreateCodeDocumentWithCSharpProjection(
            razorSource: "@",
            projectedCSharpSource: "/* CSharp */",
            sourceMappings: [new SourceMapping(new SourceSpan(0, 1), new SourceSpan(0, 12))]);
        var documentContext = CreateDocumentContext(documentPath, codeDocument);
        var languageEndpoint = new RazorLanguageQueryEndpoint(_documentMappingService, LoggerFactory);
        var request = new RazorLanguageQueryParams()
        {
            Uri = documentPath,
            Position = VsLspFactory.CreatePosition(0, 1),
        };
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var response = await languageEndpoint.HandleRequestAsync(request, requestContext, DisposalToken);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(RazorLanguageKind.CSharp, response.Kind);
        Assert.Equal(0, response.Position.Line);
        Assert.Equal(1, response.Position.Character);
    }

    // This is more of an integration test to validate that all the pieces work together
    [Fact]
    public async Task Handle_Unsupported_ResolvesLanguageRequest_Html()
    {
        // Arrange
        var documentPath = new Uri("C:/path/to/document.cshtml");
        var codeDocument = CreateCodeDocumentWithCSharpProjection(
            razorSource: "@",
            projectedCSharpSource: "/* CSharp */",
            sourceMappings: [new SourceMapping(new SourceSpan(0, 1), new SourceSpan(0, 12))]);
        codeDocument.SetUnsupported();
        var documentContext = CreateDocumentContext(documentPath, codeDocument);
        var languageEndpoint = new RazorLanguageQueryEndpoint(_documentMappingService, LoggerFactory);
        var request = new RazorLanguageQueryParams()
        {
            Uri = documentPath,
            Position = VsLspFactory.CreatePosition(0, 1),
        };

        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var response = await languageEndpoint.HandleRequestAsync(request, requestContext, DisposalToken);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(RazorLanguageKind.Html, response.Kind);
        Assert.Equal(0, response.Position.Line);
        Assert.Equal(1, response.Position.Character);
    }

    [Fact]
    public async Task Handle_AfterLastLineCharacterZero()
    {
        // Arrange
        var documentPath = new Uri("C:/path/to/document.cshtml");
        var codeDocument = CreateCodeDocumentWithCSharpProjection(
            razorSource: "@",
            projectedCSharpSource: "/* CSharp */",
            sourceMappings: [new SourceMapping(new SourceSpan(0, 1), new SourceSpan(0, 12))]);
        codeDocument.SetUnsupported();
        var documentContext = CreateDocumentContext(documentPath, codeDocument);
        var languageEndpoint = new RazorLanguageQueryEndpoint(_documentMappingService, LoggerFactory);
        var request = new RazorLanguageQueryParams()
        {
            Uri = documentPath,
            Position = VsLspFactory.CreatePosition(1, 0),
        };

        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var response = await languageEndpoint.HandleRequestAsync(request, requestContext, DisposalToken);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(RazorLanguageKind.Html, response.Kind);
        Assert.Equal(1, response.Position.Line);
        Assert.Equal(0, response.Position.Character);
    }

    private static RazorCodeDocument CreateCodeDocumentWithCSharpProjection(string razorSource, string projectedCSharpSource, ImmutableArray<SourceMapping> sourceMappings)
    {
        var codeDocument = CreateCodeDocument(razorSource, tagHelpers: []);
        var csharpDocument = TestRazorCSharpDocument.Create(
            codeDocument,
            projectedCSharpSource,
            sourceMappings);
        codeDocument.SetCSharpDocument(csharpDocument);
        return codeDocument;
    }
}
