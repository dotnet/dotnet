// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Moq;
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
        var documentMappingService = new RazorDocumentMappingService(FilePathService, new TestDocumentContextFactory(), LoggerFactory);

        var optionsMonitor = GetOptionsMonitor(enableFormatting: false);
        var endpoint = new DocumentOnTypeFormattingEndpoint(
            formattingService, documentMappingService, optionsMonitor, LoggerFactory);
        var @params = new DocumentOnTypeFormattingParams { TextDocument = new TextDocumentIdentifier { Uri = uri, } };
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
        var sourceMappings = new List<SourceMapping> { new SourceMapping(new SourceSpan(17, 0), new SourceSpan(17, 0)) };
        var codeDocument = CreateCodeDocument(content, sourceMappings);
        var uri = new Uri("file://path/test.razor");

        var documentContext = CreateDocumentContext(new Uri("file://path/testDifferentFile.razor"), codeDocument);
        var formattingService = new DummyRazorFormattingService();
        var documentMappingService = new RazorDocumentMappingService(FilePathService, new TestDocumentContextFactory(), LoggerFactory);

        var optionsMonitor = GetOptionsMonitor(enableFormatting: true);
        var endpoint = new DocumentOnTypeFormattingEndpoint(
            formattingService, documentMappingService, optionsMonitor, LoggerFactory);
        var @params = new DocumentOnTypeFormattingParams()
        {
            TextDocument = new TextDocumentIdentifier { Uri = uri, },
            Character = ".",
            Position = new Position(2, 11),
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
        var sourceMappings = new List<SourceMapping> { };
        var codeDocument = CreateCodeDocument(content, sourceMappings);
        var uri = new Uri("file://path/test.razor");

        var documentContext = CreateDocumentContext(uri, codeDocument);
        var formattingService = new DummyRazorFormattingService();
        var documentMappingService = new RazorDocumentMappingService(FilePathService, new TestDocumentContextFactory(), LoggerFactory);

        var optionsMonitor = GetOptionsMonitor(enableFormatting: true);
        var endpoint = new DocumentOnTypeFormattingEndpoint(
            formattingService, documentMappingService, optionsMonitor, LoggerFactory);
        var @params = new DocumentOnTypeFormattingParams()
        {
            TextDocument = new TextDocumentIdentifier { Uri = uri, },
            Character = ".",
            Position = new Position(2, 11),
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
        var sourceMappings = new List<SourceMapping> { new SourceMapping(new SourceSpan(17, 0), new SourceSpan(17, 0)) };
        var codeDocument = CreateCodeDocument(content, sourceMappings);
        var uri = new Uri("file://path/test.razor");

        var documentContext = CreateDocumentContext(uri, codeDocument);
        var formattingService = new DummyRazorFormattingService();

        var documentMappingService = new Mock<IRazorDocumentMappingService>(MockBehavior.Strict);
        documentMappingService.Setup(s => s.GetLanguageKind(codeDocument, 17, false)).Returns(RazorLanguageKind.Html);
        var optionsMonitor = GetOptionsMonitor(enableFormatting: true);
        var endpoint = new DocumentOnTypeFormattingEndpoint(
            formattingService, documentMappingService.Object, optionsMonitor, LoggerFactory);
        var @params = new DocumentOnTypeFormattingParams()
        {
            TextDocument = new TextDocumentIdentifier { Uri = uri, },
            Character = "}",
            Position = new Position(2, 11),
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
        var sourceMappings = new List<SourceMapping> { new SourceMapping(new SourceSpan(17, 0), new SourceSpan(17, 0)) };
        var codeDocument = CreateCodeDocument(content, sourceMappings);
        var uri = new Uri("file://path/test.razor");

        var documentContext = CreateDocumentContext(uri, codeDocument);
        var formattingService = new DummyRazorFormattingService();

        var documentMappingService = new Mock<IRazorDocumentMappingService>(MockBehavior.Strict);
        documentMappingService.Setup(s => s.GetLanguageKind(codeDocument, 17, false)).Returns(RazorLanguageKind.Razor);
        var optionsMonitor = GetOptionsMonitor(enableFormatting: true);
        var endpoint = new DocumentOnTypeFormattingEndpoint(
            formattingService, documentMappingService.Object, optionsMonitor, LoggerFactory);
        var @params = new DocumentOnTypeFormattingParams()
        {
            TextDocument = new TextDocumentIdentifier { Uri = uri, },
            Character = "}",
            Position = new Position(2, 11),
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
        var sourceMappings = new List<SourceMapping> { new SourceMapping(new SourceSpan(17, 0), new SourceSpan(17, 0)) };
        var codeDocument = CreateCodeDocument(content, sourceMappings);
        var uri = new Uri("file://path/test.razor");

        var documentResolver = CreateDocumentContextFactory(uri, codeDocument);
        var formattingService = new DummyRazorFormattingService();
        var documentMappingService = new RazorDocumentMappingService(FilePathService, documentResolver, LoggerFactory);

        var optionsMonitor = GetOptionsMonitor(enableFormatting: true);
        var endpoint = new DocumentOnTypeFormattingEndpoint(
            formattingService, documentMappingService, optionsMonitor, LoggerFactory);
        var @params = new DocumentOnTypeFormattingParams()
        {
            TextDocument = new TextDocumentIdentifier { Uri = uri, },
            Character = ".",
            Position = new Position(2, 11),
            Options = new FormattingOptions { InsertSpaces = true, TabSize = 4 }
        };
        var documentContext = documentResolver.TryCreateForOpenDocument(uri);
        var requestContext = CreateRazorRequestContext(documentContext!);

        // Act
        var result = await endpoint.HandleRequestAsync(@params, requestContext, DisposalToken);

        // Assert
        Assert.Null(result);
    }
}
