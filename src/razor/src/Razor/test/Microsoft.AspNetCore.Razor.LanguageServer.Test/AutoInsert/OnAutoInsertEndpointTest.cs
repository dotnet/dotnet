// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Formatting;
using Microsoft.AspNetCore.Razor.LanguageServer.Test;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.AutoInsert;

public partial class OnAutoInsertEndpointTest(ITestOutputHelper testOutput) : SingleServerDelegatingEndpointTestBase(testOutput)
{
    [Fact]
    public async Task Handle_MultipleProviderUnmatchingTrigger_ReturnsNull()
    {
        // Arrange
        var codeDocument = CreateCodeDocument();
        var razorFilePath = "file://path/test.razor";
        var uri = new Uri(razorFilePath);
        var languageServer = await CreateLanguageServerAsync(codeDocument, razorFilePath);
        var documentContext = CreateDocumentContext(uri, codeDocument);
        var optionsMonitor = GetOptionsMonitor();
        var insertProvider1 = new TestOnAutoInsertProvider(">", canResolve: true);
        var insertProvider2 = new TestOnAutoInsertProvider("<", canResolve: true);
        var endpoint = new OnAutoInsertEndpoint(LanguageServerFeatureOptions, DocumentMappingService, languageServer, [insertProvider1, insertProvider2], optionsMonitor, TestAdhocWorkspaceFactory.Instance, null!, LoggerFactory);
        var @params = new VSInternalDocumentOnAutoInsertParams()
        {
            TextDocument = new TextDocumentIdentifier { Uri = uri, },
            Position = new Position(0, 0),
            Character = "!",
            Options = new FormattingOptions
            {
                TabSize = 4,
                InsertSpaces = true
            },
        };
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(@params, requestContext, DisposalToken);

        // Assert
        Assert.Null(result);
        Assert.False(insertProvider1.Called);
        Assert.False(insertProvider2.Called);
        Assert.Equal(0, languageServer.RequestCount);
    }

    [Fact]
    public async Task Handle_DocumentNotFound_ReturnsNull()
    {
        // Arrange
        var codeDocument = CreateCodeDocument();
        var razorFilePath = "file://path/test.razor";
        var languageServer = await CreateLanguageServerAsync(codeDocument, razorFilePath);

        var optionsMonitor = GetOptionsMonitor();
        var insertProvider = new TestOnAutoInsertProvider(">", canResolve: true);
        var endpoint = new OnAutoInsertEndpoint(LanguageServerFeatureOptions, DocumentMappingService, languageServer, [insertProvider], optionsMonitor, TestAdhocWorkspaceFactory.Instance, null!, LoggerFactory);
        var uri = new Uri("file://path/test.razor");
        var @params = new VSInternalDocumentOnAutoInsertParams()
        {
            TextDocument = new TextDocumentIdentifier { Uri = uri, },
            Position = new Position(0, 0),
            Character = ">",
            Options = new FormattingOptions
            {
                TabSize = 4,
                InsertSpaces = true
            },
        };
        var requestContext = CreateRazorRequestContext(documentContext: null);

        // Act
        var result = await endpoint.HandleRequestAsync(@params, requestContext, DisposalToken);

        // Assert
        Assert.Null(result);
        Assert.False(insertProvider.Called);
        Assert.Equal(0, languageServer.RequestCount);
    }

    [Fact]
    public async Task Handle_UnsupportedCodeDocument_ReturnsNull()
    {
        // Arrange
        var codeDocument = CreateCodeDocument();
        codeDocument.SetUnsupported();
        var razorFilePath = "file://path/test.razor";
        var uri = new Uri(razorFilePath);
        var languageServer = await CreateLanguageServerAsync(codeDocument, razorFilePath);
        var documentContext = CreateDocumentContext(uri, codeDocument);
        var optionsMonitor = GetOptionsMonitor();
        var insertProvider = new TestOnAutoInsertProvider(">", canResolve: true);
        var endpoint = new OnAutoInsertEndpoint(LanguageServerFeatureOptions, DocumentMappingService, languageServer, [insertProvider], optionsMonitor, TestAdhocWorkspaceFactory.Instance, null!, LoggerFactory);
        var @params = new VSInternalDocumentOnAutoInsertParams()
        {
            TextDocument = new TextDocumentIdentifier { Uri = uri, },
            Position = new Position(0, 0),
            Character = ">",
            Options = new FormattingOptions
            {
                TabSize = 4,
                InsertSpaces = true
            },
        };
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(@params, requestContext, DisposalToken);

        // Assert
        Assert.Null(result);
        Assert.False(insertProvider.Called);
        Assert.Equal(0, languageServer.RequestCount);
    }

    [Fact]
    public async Task Handle_OnTypeFormattingOff_CSharp_ReturnsNull()
    {
        // Arrange
        var codeDocument = CreateCodeDocument();
        var razorFilePath = "file://path/test.razor";
        var uri = new Uri(razorFilePath);
        var languageServer = await CreateLanguageServerAsync(codeDocument, razorFilePath);
        var documentContext = CreateDocumentContext(uri, codeDocument);
        var optionsMonitor = GetOptionsMonitor(formatOnType: false);
        var insertProvider = new TestOnAutoInsertProvider(">", canResolve: false);
        var endpoint = new OnAutoInsertEndpoint(LanguageServerFeatureOptions, DocumentMappingService, languageServer, [insertProvider], optionsMonitor, TestAdhocWorkspaceFactory.Instance, null!, LoggerFactory);
        var @params = new VSInternalDocumentOnAutoInsertParams()
        {
            TextDocument = new TextDocumentIdentifier { Uri = uri, },
            Position = new Position(1, 3),
            Character = "/",
            Options = new FormattingOptions
            {
                TabSize = 4,
                InsertSpaces = true
            },
        };
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(@params, requestContext, DisposalToken);

        // Assert
        Assert.Null(result);
        Assert.Equal(0, languageServer.RequestCount);
    }

    private class TestOnAutoInsertProvider(string triggerCharacter, bool canResolve) : IOnAutoInsertProvider
    {
        public bool Called { get; private set; }

        public TextEdit? ResolvedTextEdit { get; set; }

        public string TriggerCharacter { get; } = triggerCharacter;

        // Disabling because [NotNullWhen] is available in two Assemblies and causes warnings
        public bool TryResolveInsertion(Position position, FormattingContext context, [NotNullWhen(true)] out TextEdit? edit, out InsertTextFormat format)
        {
            Called = true;
            edit = ResolvedTextEdit!;
            format = default;
            return canResolve;
        }
    }

    private static RazorCodeDocument CreateCodeDocument()
    {
        return CreateCodeDocument("""

            @{ }
            """);
    }
}
