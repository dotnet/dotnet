// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.CodeGeneration;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.AspNetCore.Razor.LanguageServer.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Mapping;

public class RazorMapToDocumentRangesEndpointTest : LanguageServerTestBase
{
    private readonly IRazorDocumentMappingService _mappingService;

    public RazorMapToDocumentRangesEndpointTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        _mappingService = new RazorDocumentMappingService(
            FilePathService,
            new TestDocumentContextFactory(),
            LoggerFactory);
    }

    // These are more integration tests to validate that all the pieces work together
    [Fact]
    public async Task Handle_MapToDocumentRanges_CSharp()
    {
        // Arrange
        var documentPath = new Uri("C:/path/to/document.cshtml");
        var codeDocument = CreateCodeDocumentWithCSharpProjection(
            "<p>@DateTime.Now</p>",
            "var __o = DateTime.Now",
            new[] {
                new SourceMapping(
                    new SourceSpan(4, 12),
                    new SourceSpan(10, 12))
            });
        var documentContext = CreateDocumentContext(documentPath, codeDocument);
        var languageEndpoint = new RazorMapToDocumentRangesEndpoint(_mappingService);
        var request = new RazorMapToDocumentRangesParams()
        {
            Kind = RazorLanguageKind.CSharp,
            ProjectedRanges = new[] { new Range { Start = new Position(0, 10), End = new Position(0, 22) }, },
            RazorDocumentUri = documentPath,
        };
        var expectedRange = new Range { Start = new Position(0, 4), End = new Position(0, 16) };

        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var response = await languageEndpoint.HandleRequestAsync(request, requestContext, default);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(expectedRange, response!.Ranges[0]);
        Assert.Equal(1337, response.HostDocumentVersion);
    }

    [Fact]
    public async Task Handle_MapToDocumentRanges_CSharp_Unmapped()
    {
        // Arrange
        var documentPath = new Uri("C:/path/to/document.cshtml");
        var codeDocument = CreateCodeDocumentWithCSharpProjection(
            "<p>@DateTime.Now</p>",
            "var __o = DateTime.Now",
            new[] {
                new SourceMapping(
                    new SourceSpan(4, 12),
                    new SourceSpan(10, 12))
            });
        var documentContext = CreateDocumentContext(documentPath, codeDocument);
        var languageEndpoint = new RazorMapToDocumentRangesEndpoint(_mappingService);
        var request = new RazorMapToDocumentRangesParams()
        {
            Kind = RazorLanguageKind.CSharp,
            ProjectedRanges = new[] { new Range { Start = new Position(0, 0), End = new Position(0, 3) } },
            RazorDocumentUri = documentPath,
        };

        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var response = await languageEndpoint.HandleRequestAsync(request, requestContext, default);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(RangeExtensions.UndefinedRange, response!.Ranges[0]);
        Assert.Equal(1337, response.HostDocumentVersion);
    }

    [Fact]
    public async Task Handle_MapToDocumentRanges_CSharp_LeadingOverlapsUnmapped()
    {
        // Arrange
        var documentPath = new Uri("C:/path/to/document.cshtml");
        var codeDocument = CreateCodeDocumentWithCSharpProjection(
            "<p>@DateTime.Now</p>",
            "var __o = DateTime.Now",
            new[] {
                new SourceMapping(
                    new SourceSpan(4, 12),
                    new SourceSpan(10, 12))
            });
        var documentContext = CreateDocumentContext(documentPath, codeDocument);
        var languageEndpoint = new RazorMapToDocumentRangesEndpoint(_mappingService);
        var request = new RazorMapToDocumentRangesParams()
        {
            Kind = RazorLanguageKind.CSharp,
            ProjectedRanges = new[] { new Range { Start = new Position(0, 0), End = new Position(0, 22) } },
            RazorDocumentUri = documentPath,
        };

        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var response = await languageEndpoint.HandleRequestAsync(request, requestContext, default);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(RangeExtensions.UndefinedRange, response!.Ranges[0]);
        Assert.Equal(1337, response.HostDocumentVersion);
    }

    [Fact]
    public async Task Handle_MapToDocumentRanges_CSharp_TrailingOverlapsUnmapped()
    {
        // Arrange
        var documentPath = new Uri("C:/path/to/document.cshtml");
        var codeDocument = CreateCodeDocumentWithCSharpProjection(
            "<p>@DateTime.Now</p>",
            "var __o = DateTime.Now",
            new[] {
                new SourceMapping(
                    new SourceSpan(4, 12),
                    new SourceSpan(10, 12))
            });
        var documentContext = CreateDocumentContext(documentPath, codeDocument);
        var languageEndpoint = new RazorMapToDocumentRangesEndpoint(_mappingService);
        var request = new RazorMapToDocumentRangesParams()
        {
            Kind = RazorLanguageKind.CSharp,
            ProjectedRanges = new[] { new Range { Start = new Position(0, 10), End = new Position(0, 23) } },
            RazorDocumentUri = documentPath,
        };

        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var response = await languageEndpoint.HandleRequestAsync(request, requestContext, default);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(RangeExtensions.UndefinedRange, response!.Ranges[0]);
        Assert.Equal(1337, response.HostDocumentVersion);
    }

    [Fact]
    public async Task Handle_MapToDocumentRanges_Html()
    {
        // Arrange
        var documentPath = new Uri("C:/path/to/document.cshtml");
        var codeDocument = CreateCodeDocument("<p>@DateTime.Now</p>");
        var documentContext = CreateDocumentContext(documentPath, codeDocument);
        var languageEndpoint = new RazorMapToDocumentRangesEndpoint(_mappingService);
        var request = new RazorMapToDocumentRangesParams()
        {
            Kind = RazorLanguageKind.Html,
            ProjectedRanges = new[] { new Range { Start = new Position(0, 16), End = new Position(0, 20) } },
            RazorDocumentUri = documentPath,
        };

        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var response = await languageEndpoint.HandleRequestAsync(request, requestContext, default);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(request.ProjectedRanges[0], response!.Ranges[0]);
        Assert.Equal(1337, response.HostDocumentVersion);
    }

    [Fact]
    public async Task Handle_MapToDocumentRanges_Razor()
    {
        // Arrange
        var documentPath = new Uri("C:/path/to/document.cshtml");
        var codeDocument = CreateCodeDocument("<p>@DateTime.Now</p>");
        var documentContext = CreateDocumentContext(documentPath, codeDocument);
        var languageEndpoint = new RazorMapToDocumentRangesEndpoint(_mappingService);
        var request = new RazorMapToDocumentRangesParams()
        {
            Kind = RazorLanguageKind.Razor,
            ProjectedRanges = new[] { new Range { Start = new Position(0, 3), End = new Position(0, 4) } },
            RazorDocumentUri = documentPath,
        };

        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var response = await languageEndpoint.HandleRequestAsync(request, requestContext, default);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(request.ProjectedRanges[0], response!.Ranges[0]);
        Assert.Equal(1337, response.HostDocumentVersion);
    }

    [Fact]
    public async Task Handle_MapToDocumentRanges_Unsupported()
    {
        // Arrange
        var documentPath = new Uri("C:/path/to/document.cshtml");
        var codeDocument = CreateCodeDocumentWithCSharpProjection(
            "<p>@DateTime.Now</p>",
            "var __o = DateTime.Now",
            new[] {
                new SourceMapping(
                    new SourceSpan(4, 12),
                    new SourceSpan(10, 12))
            });
        codeDocument.SetUnsupported();
        var documentContext = CreateDocumentContext(documentPath, codeDocument);
        var languageEndpoint = new RazorMapToDocumentRangesEndpoint(_mappingService);
        var request = new RazorMapToDocumentRangesParams()
        {
            Kind = RazorLanguageKind.CSharp,
            ProjectedRanges = new[] { new Range { Start = new Position(0, 10), End = new Position(0, 22) } },
            RazorDocumentUri = documentPath,
        };

        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var response = await languageEndpoint.HandleRequestAsync(request, requestContext, default);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(RangeExtensions.UndefinedRange, response!.Ranges[0]);
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
            sourceMappings,
            Enumerable.Empty<LinePragma>());
        codeDocument.SetCSharpDocument(csharpDocument);
        return codeDocument;
    }
}
