// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Razor.AutoInsert;
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
        await using var languageServer = await CreateLanguageServerAsync(codeDocument, razorFilePath);
        var documentContext = CreateDocumentContext(uri, codeDocument);
        var optionsMonitor = GetOptionsMonitor();
        var insertProvider1 = new TestOnAutoInsertProvider(">", canResolve: true);
        var insertProvider2 = new TestOnAutoInsertProvider("<", canResolve: true);
        var autoInsertService = new AutoInsertService([insertProvider1, insertProvider2]);
        var endpoint = new OnAutoInsertEndpoint(
            LanguageServerFeatureOptions,
            DocumentMappingService,
            languageServer,
            autoInsertService,
            optionsMonitor,
            null!,
            LoggerFactory);
        var @params = new VSInternalDocumentOnAutoInsertParams()
        {
            TextDocument = new TextDocumentIdentifier { DocumentUri = new(uri), },
            Position = LspFactory.DefaultPosition,
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
        await using var languageServer = await CreateLanguageServerAsync(codeDocument, razorFilePath);

        var optionsMonitor = GetOptionsMonitor();
        var insertProvider = new TestOnAutoInsertProvider(">", canResolve: true);
        var endpoint = new OnAutoInsertEndpoint(
            LanguageServerFeatureOptions,
            DocumentMappingService,
            languageServer,
            new AutoInsertService([insertProvider]),
            optionsMonitor,
            null!,
            LoggerFactory);
        var uri = new Uri("file://path/test.razor");
        var @params = new VSInternalDocumentOnAutoInsertParams()
        {
            TextDocument = new TextDocumentIdentifier { DocumentUri = new(uri), },
            Position = LspFactory.DefaultPosition,
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
    public async Task Handle_OnTypeFormattingOff_CSharp_ReturnsNull()
    {
        // Arrange
        var codeDocument = CreateCodeDocument();
        var razorFilePath = "file://path/test.razor";
        var uri = new Uri(razorFilePath);
        await using var languageServer = await CreateLanguageServerAsync(codeDocument, razorFilePath);
        var documentContext = CreateDocumentContext(uri, codeDocument);
        var optionsMonitor = GetOptionsMonitor(formatOnType: false);
        var insertProvider = new TestOnAutoInsertProvider(">", canResolve: false);
        var endpoint = new OnAutoInsertEndpoint(
            LanguageServerFeatureOptions,
            DocumentMappingService,
            languageServer,
            new AutoInsertService([insertProvider]),
            optionsMonitor,
            razorFormattingService: null!,
            LoggerFactory);
        var @params = new VSInternalDocumentOnAutoInsertParams()
        {
            TextDocument = new TextDocumentIdentifier { DocumentUri = new(uri), },
            Position = LspFactory.CreatePosition(1, 3),
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

        public bool TryResolveInsertion(
            Position position,
            RazorCodeDocument codeDocument,
            bool enableAutoClosingTags,
            [NotNullWhen(true)] out VSInternalDocumentOnAutoInsertResponseItem? autoInsertEdit)
        {
            Called = true;
            autoInsertEdit = canResolve ? new() { TextEdit = ResolvedTextEdit!, TextEditFormat = InsertTextFormat.Plaintext } : null;

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
