// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.CodeGeneration;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Mapping;

public class RazorLanguageQueryEndpointTest : LanguageServerTestBase
{
    private readonly IRazorDocumentMappingService _mappingService;

    public RazorLanguageQueryEndpointTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        _mappingService = new RazorDocumentMappingService(
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
        var languageEndpoint = new RazorLanguageQueryEndpoint(_mappingService, LoggerFactory);
        var request = new RazorLanguageQueryParams()
        {
            Uri = documentPath,
            Position = new Position(0, 1),
        };

        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var response = await languageEndpoint.HandleRequestAsync(request, requestContext, default);

        // Assert
        Assert.Equal(RazorLanguageKind.Razor, response.Kind);
        Assert.Equal(request.Position, response.Position);
        Assert.Equal(1337, response.HostDocumentVersion);
    }

    // This is more of an integration test to validate that all the pieces work together
    [Fact]
    public async Task Handle_ResolvesLanguageRequest_Html()
    {
        // Arrange
        var documentPath = new Uri("C:/path/to/document.cshtml");
        var codeDocument = CreateCodeDocument("<s");
        var documentContext = CreateDocumentContext(documentPath, codeDocument);
        var languageEndpoint = new RazorLanguageQueryEndpoint(_mappingService, LoggerFactory);
        var request = new RazorLanguageQueryParams()
        {
            Uri = documentPath,
            Position = new Position(0, 2),
        };

        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var response = await languageEndpoint.HandleRequestAsync(request, requestContext, default);

        // Assert
        Assert.Equal(RazorLanguageKind.Html, response.Kind);
        Assert.Equal(request.Position, response.Position);
        Assert.Equal(1337, response.HostDocumentVersion);
    }

    // This is more of an integration test to validate that all the pieces work together
    [Fact]
    public async Task Handle_ResolvesLanguageRequest_CSharp()
    {
        // Arrange
        var documentPath = new Uri("C:/path/to/document.cshtml");
        var codeDocument = CreateCodeDocumentWithCSharpProjection(
            "@",
            "/* CSharp */",
            new[] { new SourceMapping(new SourceSpan(0, 1), new SourceSpan(0, 12)) });
        var documentContext = CreateDocumentContext(documentPath, codeDocument);
        var languageEndpoint = new RazorLanguageQueryEndpoint(_mappingService, LoggerFactory);
        var request = new RazorLanguageQueryParams()
        {
            Uri = documentPath,
            Position = new Position(0, 1),
        };
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var response = await languageEndpoint.HandleRequestAsync(request, requestContext, default);

        // Assert
        Assert.Equal(RazorLanguageKind.CSharp, response.Kind);
        Assert.Equal(0, response.Position.Line);
        Assert.Equal(1, response.Position.Character);
        Assert.Equal(1337, response.HostDocumentVersion);
    }

    // This is more of an integration test to validate that all the pieces work together
    [Fact]
    public async Task Handle_Unsupported_ResolvesLanguageRequest_Html()
    {
        // Arrange
        var documentPath = new Uri("C:/path/to/document.cshtml");
        var codeDocument = CreateCodeDocumentWithCSharpProjection(
            "@",
            "/* CSharp */",
            new[] { new SourceMapping(new SourceSpan(0, 1), new SourceSpan(0, 12)) });
        codeDocument.SetUnsupported();
        var documentContext = CreateDocumentContext(documentPath, codeDocument);
        var languageEndpoint = new RazorLanguageQueryEndpoint(_mappingService, LoggerFactory);
        var request = new RazorLanguageQueryParams()
        {
            Uri = documentPath,
            Position = new Position(0, 1),
        };

        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var response = await languageEndpoint.HandleRequestAsync(request, requestContext, default);

        // Assert
        Assert.Equal(RazorLanguageKind.Html, response.Kind);
        Assert.Equal(0, response.Position.Line);
        Assert.Equal(1, response.Position.Character);
        Assert.Equal(1337, response.HostDocumentVersion);
    }

    private static RazorCodeDocument CreateCodeDocumentWithCSharpProjection(string razorSource, string projectedCSharpSource, IEnumerable<SourceMapping> sourceMappings)
    {
        var codeDocument = CreateCodeDocument(razorSource, ImmutableArray<TagHelperDescriptor>.Empty);
        var csharpDocument = RazorCSharpDocument.Create(
            codeDocument,
            projectedCSharpSource,
            RazorCodeGenerationOptions.CreateDefault(),
            Enumerable.Empty<RazorDiagnostic>(),
            sourceMappings.ToImmutableArray(),
            Enumerable.Empty<LinePragma>());
        codeDocument.SetCSharpDocument(csharpDocument);
        return codeDocument;
    }
}
