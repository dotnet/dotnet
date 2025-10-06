// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Completion;
using Microsoft.CodeAnalysis.Razor.LinkedEditingRange;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.LinkedEditingRange;

public class LinkedEditingRangeEndpointTest(ITestOutputHelper testOutput) : TagHelperServiceTestBase(testOutput)
{
    [Fact]
    public async Task Handle_DocumentNotFound_ReturnsNull()
    {
        // Arrange
        var uri = new Uri("file://path/test.razor");
        var endpoint = new LinkedEditingRangeEndpoint(LoggerFactory);
        var request = new LinkedEditingRangeParams
        {
            TextDocument = new TextDocumentIdentifier { DocumentUri = new(uri) },
            Position = LspFactory.CreatePosition(1, 3) // <te[||]st1></test1>
        };
        var requestContext = CreateRazorRequestContext(documentContext: null);

        // Act
        var result = await endpoint.HandleRequestAsync(request, requestContext, DisposalToken);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Handle_TagHelperStartTag_ReturnsCorrectRange()
    {
        // Arrange
        var txt = """
            @addTagHelper *, TestAssembly
            <test1></test1>
            """;
        var codeDocument = CreateCodeDocument(txt, isRazorFile: false, DefaultTagHelpers);
        var uri = new Uri("file://path/test.razor");
        var documentContext = CreateDocumentContext(uri, codeDocument);
        var endpoint = new LinkedEditingRangeEndpoint(LoggerFactory);
        var request = new LinkedEditingRangeParams
        {
            TextDocument = new TextDocumentIdentifier { DocumentUri = new(uri) },
            Position = LspFactory.CreatePosition(1, 3) // <te[||]st1></test1>
        };

        var expectedRanges = new[]
        {
            LspFactory.CreateSingleLineRange(line: 1, character: 1, length: 5),
            LspFactory.CreateSingleLineRange(line: 1, character: 9, length: 5)
        };
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(request, requestContext, DisposalToken);

        // Assert
        Assert.Equal(expectedRanges, result.Ranges);
        Assert.Equal(LinkedEditingRangeHelper.WordPattern, result.WordPattern);
    }

    [Fact]
    public async Task Handle_TagHelperStartTag_ReturnsCorrectRange_EndSpan()
    {
        // Arrange
        var txt = """
            @addTagHelper *, TestAssembly
            <test1></test1>
            """;
        var codeDocument = CreateCodeDocument(txt, isRazorFile: false, DefaultTagHelpers);
        var uri = new Uri("file://path/test.razor");
        var documentContext = CreateDocumentContext(uri, codeDocument);
        var endpoint = new LinkedEditingRangeEndpoint(LoggerFactory);
        var request = new LinkedEditingRangeParams
        {
            TextDocument = new TextDocumentIdentifier { DocumentUri = new(uri) },
            Position = LspFactory.CreatePosition(1, 6) // <test1[||]></test1>
        };

        var expectedRanges = new[]
        {
            LspFactory.CreateSingleLineRange(line: 1, character: 1, length: 5),
            LspFactory.CreateSingleLineRange(line: 1, character: 9, length: 5)
        };
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(request, requestContext, DisposalToken);

        // Assert
        Assert.Equal(expectedRanges, result.Ranges);
        Assert.Equal(LinkedEditingRangeHelper.WordPattern, result.WordPattern);
    }

    [Fact]
    public async Task Handle_TagHelperEndTag_ReturnsCorrectRange()
    {
        // Arrange
        var txt = """
            @addTagHelper *, TestAssembly
            <test1></test1>
            """;
        var codeDocument = CreateCodeDocument(txt, isRazorFile: false, DefaultTagHelpers);
        var uri = new Uri("file://path/test.razor");
        var documentContext = CreateDocumentContext(uri, codeDocument);
        var endpoint = new LinkedEditingRangeEndpoint(LoggerFactory);
        var request = new LinkedEditingRangeParams
        {
            TextDocument = new TextDocumentIdentifier { DocumentUri = new(uri) },
            Position = LspFactory.CreatePosition(1, 9) // <test1></[||]test1>
        };

        var expectedRanges = new[]
        {
            LspFactory.CreateSingleLineRange(line: 1, character: 1, length: 5),
            LspFactory.CreateSingleLineRange(line: 1, character: 9, length: 5)
        };
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(request, requestContext, DisposalToken);

        // Assert
        Assert.Equal(expectedRanges, result.Ranges);
        Assert.Equal(LinkedEditingRangeHelper.WordPattern, result.WordPattern);
    }

    [Fact]
    public async Task Handle_NoTag_ReturnsNull()
    {
        // Arrange
        var txt = """
            @addTagHelper *, TestAssembly
            <test1></test1>
            """;
        var codeDocument = CreateCodeDocument(txt, isRazorFile: false, DefaultTagHelpers);
        var uri = new Uri("file://path/test.razor");
        var documentContext = CreateDocumentContext(uri, codeDocument);
        var endpoint = new LinkedEditingRangeEndpoint(LoggerFactory);
        var request = new LinkedEditingRangeParams
        {
            TextDocument = new TextDocumentIdentifier { DocumentUri = new(uri) },
            Position = LspFactory.CreatePosition(0, 1) // @[||]addTagHelper *
        };
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(request, requestContext, DisposalToken);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Handle_SelfClosingTagHelper_ReturnsNull()
    {
        // Arrange
        var txt = """
            @addTagHelper *, TestAssembly
            <test1 />
            """;
        var codeDocument = CreateCodeDocument(txt, isRazorFile: false, DefaultTagHelpers);
        var uri = new Uri("file://path/test.razor");
        var documentContext = CreateDocumentContext(uri, codeDocument);
        var endpoint = new LinkedEditingRangeEndpoint(LoggerFactory);
        var request = new LinkedEditingRangeParams
        {
            TextDocument = new TextDocumentIdentifier { DocumentUri = new(uri) },
            Position = LspFactory.CreatePosition(1, 3) // <te[||]st1 />
        };
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(request, requestContext, DisposalToken);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Handle_NestedTagHelperStartTags_ReturnsCorrectRange()
    {
        // Arrange
        var txt = """
            @addTagHelper *, TestAssembly
            <test1><test1></test1></test1>
            """;
        var codeDocument = CreateCodeDocument(txt, isRazorFile: false, DefaultTagHelpers);
        var uri = new Uri("file://path/test.razor");
        var documentContext = CreateDocumentContext(uri, codeDocument);
        var endpoint = new LinkedEditingRangeEndpoint(LoggerFactory);
        var request = new LinkedEditingRangeParams
        {
            TextDocument = new TextDocumentIdentifier { DocumentUri = new(uri) },
            Position = LspFactory.CreatePosition(1, 1) // <[||]test1><test1></test1></test1>
        };

        var expectedRanges = new[]
        {
            LspFactory.CreateSingleLineRange(line: 1, character: 1, length: 5),
            LspFactory.CreateSingleLineRange(line: 1, character: 24, length: 5)
        };
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(request, requestContext, DisposalToken);

        // Assert
        Assert.Equal(expectedRanges, result.Ranges);
        Assert.Equal(LinkedEditingRangeHelper.WordPattern, result.WordPattern);
    }

    [Fact]
    public async Task Handle_HTMLStartTag_ReturnsCorrectRange()
    {
        // Arrange
        var txt = """
            @addTagHelper *, TestAssembly
            <body></body>
            """;
        var codeDocument = CreateCodeDocument(txt, isRazorFile: false, DefaultTagHelpers);
        var uri = new Uri("file://path/test.razor");
        var documentContext = CreateDocumentContext(uri, codeDocument);
        var endpoint = new LinkedEditingRangeEndpoint(LoggerFactory);
        var request = new LinkedEditingRangeParams
        {
            TextDocument = new TextDocumentIdentifier { DocumentUri = new(uri) },
            Position = LspFactory.CreatePosition(1, 3) // <bo[||]dy></body>
        };

        var expectedRanges = new[]
        {
            LspFactory.CreateSingleLineRange(line: 1, character: 1, length: 4),
            LspFactory.CreateSingleLineRange(line: 1, character: 8, length: 4)
        };
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(request, requestContext, DisposalToken);

        // Assert
        Assert.Equal(expectedRanges, result.Ranges);
        Assert.Equal(LinkedEditingRangeHelper.WordPattern, result.WordPattern);
    }

    [Fact]
    public async Task Handle_HTMLEndTag_ReturnsCorrectRange()
    {
        // Arrange
        var txt = """
            @addTagHelper *, TestAssembly
            <body></body>
            """;
        var codeDocument = CreateCodeDocument(txt, isRazorFile: false, DefaultTagHelpers);
        var uri = new Uri("file://path/test.razor");
        var documentContext = CreateDocumentContext(uri, codeDocument);
        var endpoint = new LinkedEditingRangeEndpoint(LoggerFactory);
        var request = new LinkedEditingRangeParams
        {
            TextDocument = new TextDocumentIdentifier { DocumentUri = new(uri) },
            Position = LspFactory.CreatePosition(1, 8) // <body></[||]body>
        };

        var expectedRanges = new[]
        {
            LspFactory.CreateSingleLineRange(line: 1, character: 1, length: 4),
            LspFactory.CreateSingleLineRange(line: 1, character: 8, length: 4)
        };
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(request, requestContext, DisposalToken);

        // Assert
        Assert.Equal(expectedRanges, result.Ranges);
        Assert.Equal(LinkedEditingRangeHelper.WordPattern, result.WordPattern);
    }

    [Fact]
    public async Task Handle_SelfClosingHTMLTag_ReturnsNull()
    {
        // Arrange
        var txt = """
            @addTagHelper *, TestAssembly
            <body />
            """;
        var codeDocument = CreateCodeDocument(txt, isRazorFile: false, DefaultTagHelpers);
        var uri = new Uri("file://path/test.razor");
        var documentContext = CreateDocumentContext(uri, codeDocument);
        var endpoint = new LinkedEditingRangeEndpoint(LoggerFactory);
        var request = new LinkedEditingRangeParams
        {
            TextDocument = new TextDocumentIdentifier { DocumentUri = new(uri) },
            Position = LspFactory.CreatePosition(1, 3) // <bo[||]dy />
        };
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(request, requestContext, DisposalToken);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void VerifyWordPatternCorrect()
    {
        // Assert
        Assert.True(Regex.Match("Test", LinkedEditingRangeHelper.WordPattern).Length == 4);
        Assert.True(Regex.Match("!Test", LinkedEditingRangeHelper.WordPattern).Length == 5);
        Assert.True(Regex.Match("!Test.Test2", LinkedEditingRangeHelper.WordPattern).Length == 11);

        Assert.True(Regex.Match("Te>st", LinkedEditingRangeHelper.WordPattern).Length != 5);
        Assert.True(Regex.Match("Te/st", LinkedEditingRangeHelper.WordPattern).Length != 5);
        Assert.True(Regex.Match("Te\\st", LinkedEditingRangeHelper.WordPattern).Length != 5);
        Assert.True(Regex.Match("Te!st", LinkedEditingRangeHelper.WordPattern).Length != 5);
        Assert.True(Regex.Match("""
            Te
            st
            """,
            LinkedEditingRangeHelper.WordPattern).Length != 4 + Environment.NewLine.Length);
    }
}
