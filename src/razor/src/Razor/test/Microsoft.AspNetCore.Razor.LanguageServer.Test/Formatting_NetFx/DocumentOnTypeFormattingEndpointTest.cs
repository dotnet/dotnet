// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Text;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Formatting;

public class DocumentOnTypeFormattingEndpointTest(ITestOutputHelper testOutput) : FormattingLanguageServerTestBase(testOutput)
{
    [Fact]
    public async Task Handle_OnTypeFormatting_FormattingDisabled_ReturnsNull()
    {
        // Arrange
        var uri = new Uri("file://path/test.razor");
        var formattingService = new DummyRazorFormattingService();

        var optionsMonitor = GetOptionsMonitor(enableFormatting: false);
        var htmlFormatter = new TestHtmlFormatter();
        var endpoint = new DocumentOnTypeFormattingEndpoint(
            formattingService, htmlFormatter, optionsMonitor, LoggerFactory);
        var @params = new DocumentOnTypeFormattingParams { TextDocument = new TextDocumentIdentifier { DocumentUri = new(uri), } };
        var requestContext = CreateRazorRequestContext(documentContext: null);

        // Act
        var result = await endpoint.HandleRequestAsync(@params, requestContext, DisposalToken);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Handle_OnTypeFormatting_DocumentNotFound_ReturnsNull()
    {
        // Arrange
        var content = @"
@{
 if(true){}
}";
        var codeDocument = CreateCodeDocument(content, sourceMappings: [new SourceMapping(new SourceSpan(17, 0), new SourceSpan(17, 0))]);
        var uri = new Uri("file://path/test.razor");

        var documentContext = CreateDocumentContext(new Uri("file://path/testDifferentFile.razor"), codeDocument);
        var formattingService = new DummyRazorFormattingService();

        var optionsMonitor = GetOptionsMonitor(enableFormatting: true);
        var htmlFormatter = new TestHtmlFormatter();
        var endpoint = new DocumentOnTypeFormattingEndpoint(
            formattingService, htmlFormatter, optionsMonitor, LoggerFactory);
        var @params = new DocumentOnTypeFormattingParams()
        {
            TextDocument = new TextDocumentIdentifier { DocumentUri = new(uri), },
            Character = ".",
            Position = LspFactory.CreatePosition(2, 11),
            Options = new FormattingOptions { InsertSpaces = true, TabSize = 4 }
        };
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(@params, requestContext, DisposalToken);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Handle_OnTypeFormatting_RemapFailed_ReturnsNull()
    {
        // Arrange
        var content = @"
@{
 if(true){}
}";
        var codeDocument = CreateCodeDocument(content, sourceMappings: []);
        var uri = new Uri("file://path/test.razor");

        var documentContext = CreateDocumentContext(uri, codeDocument);
        var formattingService = new DummyRazorFormattingService(RazorLanguageKind.CSharp);

        var optionsMonitor = GetOptionsMonitor(enableFormatting: true);
        var htmlFormatter = new TestHtmlFormatter();
        var endpoint = new DocumentOnTypeFormattingEndpoint(
            formattingService, htmlFormatter, optionsMonitor, LoggerFactory);
        var @params = new DocumentOnTypeFormattingParams()
        {
            TextDocument = new TextDocumentIdentifier { DocumentUri = new(uri), },
            Character = ".",
            Position = LspFactory.CreatePosition(2, 11),
            Options = new FormattingOptions { InsertSpaces = true, TabSize = 4 },
        };
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(@params, requestContext, DisposalToken);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Handle_OnTypeFormatting_HtmlLanguageKind_ReturnsNull()
    {
        // Arrange
        var content = @"
@{
 if(true){}
}";
        var codeDocument = CreateCodeDocument(content, sourceMappings: [new SourceMapping(new SourceSpan(17, 0), new SourceSpan(17, 0))]);
        var uri = new Uri("file://path/test.razor");

        var documentContext = CreateDocumentContext(uri, codeDocument);
        var formattingService = new DummyRazorFormattingService(RazorLanguageKind.Html);

        var optionsMonitor = GetOptionsMonitor(enableFormatting: true);
        var htmlFormatter = new TestHtmlFormatter();
        var endpoint = new DocumentOnTypeFormattingEndpoint(
            formattingService, htmlFormatter, optionsMonitor, LoggerFactory);
        var @params = new DocumentOnTypeFormattingParams()
        {
            TextDocument = new TextDocumentIdentifier { DocumentUri = new(uri), },
            Character = "}",
            Position = LspFactory.CreatePosition(2, 11),
            Options = new FormattingOptions { InsertSpaces = true, TabSize = 4 },
        };
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(@params, requestContext, DisposalToken);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Handle_OnTypeFormatting_RazorLanguageKind_ReturnsNull()
    {
        // Arrange
        var content = @"
@{
 if(true){}
}";
        var codeDocument = CreateCodeDocument(content, sourceMappings: [new SourceMapping(new SourceSpan(17, 0), new SourceSpan(17, 0))]);
        var uri = new Uri("file://path/test.razor");

        var documentContext = CreateDocumentContext(uri, codeDocument);
        var formattingService = new DummyRazorFormattingService(RazorLanguageKind.Razor);

        var optionsMonitor = GetOptionsMonitor(enableFormatting: true);
        var htmlFormatter = new TestHtmlFormatter();
        var endpoint = new DocumentOnTypeFormattingEndpoint(
            formattingService, htmlFormatter, optionsMonitor, LoggerFactory);
        var @params = new DocumentOnTypeFormattingParams()
        {
            TextDocument = new TextDocumentIdentifier { DocumentUri = new(uri), },
            Character = "}",
            Position = LspFactory.CreatePosition(2, 11),
            Options = new FormattingOptions { InsertSpaces = true, TabSize = 4 }
        };
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(@params, requestContext, DisposalToken);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Handle_OnTypeFormatting_UnexpectedTriggerCharacter_ReturnsNull()
    {
        // Arrange
        var content = @"
@{
 if(true){}
}";
        var codeDocument = CreateCodeDocument(content, [new SourceMapping(new SourceSpan(17, 0), new SourceSpan(17, 0))]);
        var uri = new Uri("file://path/test.razor");

        var documentContextFactory = CreateDocumentContextFactory(uri, codeDocument);
        var formattingService = new DummyRazorFormattingService(RazorLanguageKind.CSharp);

        var optionsMonitor = GetOptionsMonitor(enableFormatting: true);
        var htmlFormatter = new TestHtmlFormatter();
        var endpoint = new DocumentOnTypeFormattingEndpoint(
            formattingService, htmlFormatter, optionsMonitor, LoggerFactory);
        var @params = new DocumentOnTypeFormattingParams()
        {
            TextDocument = new TextDocumentIdentifier { DocumentUri = new(uri), },
            Character = ".",
            Position = LspFactory.CreatePosition(2, 11),
            Options = new FormattingOptions { InsertSpaces = true, TabSize = 4 }
        };
        Assert.True(documentContextFactory.TryCreate(uri, out var documentContext));
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(@params, requestContext, DisposalToken);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Handle_OnTypeFormatting_ExpectedTriggerCharacter_ReturnsNotNull()
    {
        // Arrange
        TestCode content = """
            @code {
               private string Goo {get;$$}
            }
            """;
        var codeDocument = CreateCodeDocument(content.Text, [new SourceMapping(new SourceSpan(17, 0), new SourceSpan(17, 0))]);
        var sourceText = SourceText.From(content.Text);
        var uri = new Uri("file://path/test.razor");

        var documentContextFactory = CreateDocumentContextFactory(uri, codeDocument);
        var formattingService = new DummyRazorFormattingService(RazorLanguageKind.CSharp);

        var optionsMonitor = GetOptionsMonitor(enableFormatting: true);
        var htmlFormatter = new TestHtmlFormatter();
        var endpoint = new DocumentOnTypeFormattingEndpoint(
            formattingService, htmlFormatter, optionsMonitor, LoggerFactory);
        var @params = new DocumentOnTypeFormattingParams()
        {
            TextDocument = new TextDocumentIdentifier { DocumentUri = new(uri), },
            Character = ";",
            Position = sourceText.GetPosition(content.Position),
            Options = new FormattingOptions { InsertSpaces = true, TabSize = 4 }
        };
        Assert.True(documentContextFactory.TryCreate(uri, out var documentContext));
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        await endpoint.HandleRequestAsync(@params, requestContext, DisposalToken);

        // Assert
        Assert.True(formattingService.Called);
    }
}
