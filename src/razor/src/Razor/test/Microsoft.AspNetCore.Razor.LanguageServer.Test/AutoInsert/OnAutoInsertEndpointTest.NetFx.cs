// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Formatting;
using Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.AutoInsert;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Text;
using Xunit;

namespace Microsoft.AspNetCore.Razor.LanguageServer.AutoInsert;

public partial class OnAutoInsertEndpointTest
{
    [Fact]
    public async Task Handle_SingleProvider_InvokesProvider()
    {
        // Arrange
        var codeDocument = CreateCodeDocument();
        var razorFilePath = "file://path/test.razor";
        var uri = new Uri(razorFilePath);
        await using var languageServer = await CreateLanguageServerAsync(codeDocument, razorFilePath);
        var documentContext = CreateDocumentContext(uri, codeDocument);
        var optionsMonitor = GetOptionsMonitor();
        var insertProvider = new TestOnAutoInsertProvider(">", canResolve: true);
        var formattingService = await TestRazorFormattingService.CreateWithFullSupportAsync(LoggerFactory);
        var endpoint = new OnAutoInsertEndpoint(
            LanguageServerFeatureOptions,
            DocumentMappingService,
            languageServer,
            new AutoInsertService([insertProvider]),
            optionsMonitor,
            formattingService,
            LoggerFactory);
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
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(@params, requestContext, DisposalToken);

        // Assert
        Assert.NotNull(result);
        Assert.True(insertProvider.Called);
        Assert.Equal(0, languageServer.RequestCount);
    }

    [Fact]
    public async Task Handle_MultipleProviderSameTrigger_UsesSuccessful()
    {
        // Arrange
        var codeDocument = CreateCodeDocument();
        var razorFilePath = "file://path/test.razor";
        var uri = new Uri(razorFilePath);
        await using var languageServer = await CreateLanguageServerAsync(codeDocument, razorFilePath);
        var documentContext = CreateDocumentContext(uri, codeDocument);
        var optionsMonitor = GetOptionsMonitor();
        var insertProvider1 = new TestOnAutoInsertProvider(">", canResolve: false)
        {
            ResolvedTextEdit = LspFactory.CreateTextEdit(position: (0, 0), string.Empty)
        };
        var insertProvider2 = new TestOnAutoInsertProvider(">", canResolve: true)
        {
            ResolvedTextEdit = LspFactory.CreateTextEdit(position: (0, 0), string.Empty)
        };
        var formattingService = await TestRazorFormattingService.CreateWithFullSupportAsync(LoggerFactory);
        var endpoint = new OnAutoInsertEndpoint(
            LanguageServerFeatureOptions,
            DocumentMappingService,
            languageServer,
            new AutoInsertService([insertProvider1, insertProvider2]),
            optionsMonitor,
            formattingService,
            LoggerFactory);
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

        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(@params, requestContext, DisposalToken);

        // Assert
        Assert.NotNull(result);
        Assert.True(insertProvider1.Called);
        Assert.True(insertProvider2.Called);
        Assert.Same(insertProvider2.ResolvedTextEdit, result?.TextEdit);
        Assert.Equal(0, languageServer.RequestCount);
    }

    [Fact]
    public async Task Handle_MultipleProviderSameTrigger_UsesFirstSuccessful()
    {
        // Arrange
        var codeDocument = CreateCodeDocument();
        var razorFilePath = "file://path/test.razor";
        var uri = new Uri(razorFilePath);
        await using var languageServer = await CreateLanguageServerAsync(codeDocument, razorFilePath);
        var documentContext = CreateDocumentContext(uri, codeDocument);
        var optionsMonitor = GetOptionsMonitor();
        var insertProvider1 = new TestOnAutoInsertProvider(">", canResolve: true)
        {
            ResolvedTextEdit = LspFactory.CreateTextEdit(position: (0, 0), string.Empty)
        };
        var insertProvider2 = new TestOnAutoInsertProvider(">", canResolve: true)
        {
            ResolvedTextEdit = LspFactory.CreateTextEdit(position: (0, 0), string.Empty)
        };
        var formattingService = await TestRazorFormattingService.CreateWithFullSupportAsync(LoggerFactory);
        var endpoint = new OnAutoInsertEndpoint(
            LanguageServerFeatureOptions,
            DocumentMappingService,
            languageServer,
            new AutoInsertService([insertProvider1, insertProvider2]),
            optionsMonitor,
            formattingService,
            LoggerFactory);
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

        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(@params, requestContext, DisposalToken);

        // Assert
        Assert.NotNull(result);
        Assert.True(insertProvider1.Called);
        Assert.False(insertProvider2.Called);
        Assert.Same(insertProvider1.ResolvedTextEdit, result?.TextEdit);
    }

    [Fact]
    public async Task Handle_NoApplicableProvider_CallsProviderAndReturnsNull()
    {
        // Arrange
        var codeDocument = CreateCodeDocument();
        var razorFilePath = "file://path/test.razor";
        var uri = new Uri(razorFilePath);
        await using var languageServer = await CreateLanguageServerAsync(codeDocument, razorFilePath);
        var documentContext = CreateDocumentContext(uri, codeDocument);
        var optionsMonitor = GetOptionsMonitor();
        var insertProvider = new TestOnAutoInsertProvider(">", canResolve: false);
        var formattingService = await TestRazorFormattingService.CreateWithFullSupportAsync(LoggerFactory);
        var endpoint = new OnAutoInsertEndpoint(
            LanguageServerFeatureOptions,
            DocumentMappingService,
            languageServer,
            new AutoInsertService([insertProvider]),
            optionsMonitor,
            formattingService,
            LoggerFactory);
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
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(@params, requestContext, DisposalToken);

        // Assert
        Assert.Null(result);
        Assert.True(insertProvider.Called);
        Assert.Equal(0, languageServer.RequestCount);
    }

    [Fact]
    public async Task Handle_OnTypeFormattingOff_Html_CallsLanguageServer()
    {
        // Arrange
        var codeDocument = CreateCodeDocument();
        var razorFilePath = "file://path/test.razor";
        var uri = new Uri(razorFilePath);
        await using var languageServer = await CreateLanguageServerAsync(codeDocument, razorFilePath);
        var documentContext = CreateDocumentContext(uri, codeDocument);
        var optionsMonitor = GetOptionsMonitor(formatOnType: false);
        var insertProvider = new TestOnAutoInsertProvider("<", canResolve: false);
        var formattingService = await TestRazorFormattingService.CreateWithFullSupportAsync(LoggerFactory);
        var endpoint = new OnAutoInsertEndpoint(
            LanguageServerFeatureOptions,
            DocumentMappingService,
            languageServer,
            new AutoInsertService([insertProvider]),
            optionsMonitor,
            formattingService,
            LoggerFactory);
        var @params = new VSInternalDocumentOnAutoInsertParams()
        {
            TextDocument = new TextDocumentIdentifier { DocumentUri = new(uri), },
            Position = LspFactory.DefaultPosition,
            Character = "=",
            Options = new FormattingOptions
            {
                TabSize = 4,
                InsertSpaces = true
            },
        };
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        _ = await endpoint.HandleRequestAsync(@params, requestContext, DisposalToken);

        // Assert
        Assert.Equal(1, languageServer.RequestCount);
    }

    [Fact]
    public async Task Handle_AutoInsertAttributeQuotesOff_Html_DoesNotCallLanguageServer()
    {
        // Arrange
        var codeDocument = CreateCodeDocument();
        var razorFilePath = "file://path/test.razor";
        var uri = new Uri(razorFilePath);
        await using var languageServer = await CreateLanguageServerAsync(codeDocument, razorFilePath);
        var documentContext = CreateDocumentContext(uri, codeDocument);
        var optionsMonitor = GetOptionsMonitor(autoInsertAttributeQuotes: false);
        var insertProvider = new TestOnAutoInsertProvider("<", canResolve: false);
        var formattingService = await TestRazorFormattingService.CreateWithFullSupportAsync(LoggerFactory);
        var endpoint = new OnAutoInsertEndpoint(
            LanguageServerFeatureOptions,
            DocumentMappingService,
            languageServer,
            new AutoInsertService([insertProvider]),
            optionsMonitor,
            formattingService,
            LoggerFactory);
        var @params = new VSInternalDocumentOnAutoInsertParams()
        {
            TextDocument = new TextDocumentIdentifier { DocumentUri = new(uri), },
            Position = LspFactory.DefaultPosition,
            Character = "=",
            Options = new FormattingOptions
            {
                TabSize = 4,
                InsertSpaces = true
            },
        };
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        _ = await endpoint.HandleRequestAsync(@params, requestContext, DisposalToken);

        // Assert
        Assert.Equal(0, languageServer.RequestCount);
    }

    [Fact]
    public async Task Handle_SingleServer_CSharpDocCommentSnippet()
    {
        // Arrange
        var input = """
                <div>
                </div>

                @functions {
                    ///$$
                    public void M()
                    {
                    }
                }
                """;

        var expected = """
                <div>
                </div>

                @functions {
                    /// <summary>
                    /// $0
                    /// </summary>
                    public void M()
                    {
                    }
                }
                """;

        var character = "/";

        await VerifyCSharpOnAutoInsertAsync(input, expected, character);
    }

    [Fact]
    public async Task Handle_SingleServer_CSharpDocCommentNewLine()
    {
        // Arrange
        var input = """
                <div>
                </div>

                @functions {
                    /// <summary>
                    /// This is some text
                    $$
                    /// </summary>
                    public void M()
                    {
                    }
                }
                """;

        var expected = """
                <div>
                </div>

                @functions {
                    /// <summary>
                    /// This is some text
                    /// $0
                    /// </summary>
                    public void M()
                    {
                    }
                }
                """;

        var character = "\n";

        await VerifyCSharpOnAutoInsertAsync(input, expected, character);
    }

    [Fact]
    public async Task Handle_SingleServer_CSharpBraceMatching()
    {
        // Arrange
        var input = """
                <div>
                </div>

                @functions {
                    public void M()
                    {
                    $$}
                }
                """;

        var expected = """
                <div>
                </div>

                @functions {
                    public void M()
                    {
                        $0
                    }
                }
                """;

        var character = "\n";

        await VerifyCSharpOnAutoInsertAsync(input, expected, character);
    }

    private async Task VerifyCSharpOnAutoInsertAsync(string input, string expected, string character)
    {
        TestFileMarkupParser.GetPosition(input, out input, out var cursorPosition);

        var codeDocument = CreateCodeDocument(input);
        var razorFilePath = "C:/path/test.razor";
        await using var languageServer = await CreateLanguageServerAsync(codeDocument, razorFilePath);

        var optionsMonitor = GetOptionsMonitor();
        var insertProvider = new TestOnAutoInsertProvider("!!!", canResolve: false);
        var formattingService = await TestRazorFormattingService.CreateWithFullSupportAsync(LoggerFactory);
        var endpoint = new OnAutoInsertEndpoint(
            LanguageServerFeatureOptions,
            DocumentMappingService,
            languageServer,
            new AutoInsertService([insertProvider]),
            optionsMonitor,
            formattingService,
            LoggerFactory);

        var text = codeDocument.Source.Text;
        var @params = new VSInternalDocumentOnAutoInsertParams()
        {
            TextDocument = new TextDocumentIdentifier { DocumentUri = new(new Uri(razorFilePath)), },
            Position = text.GetPosition(cursorPosition),
            Character = character,
            Options = new FormattingOptions
            {
                TabSize = 4,
                InsertSpaces = true
            },
        };
        Assert.True(DocumentContextFactory.TryCreate(@params.TextDocument, out var documentContext));

        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(@params, requestContext, DisposalToken);

        // Assert
        Assert.NotNull(result);
        Assert.False(insertProvider.Called);
        Assert.Equal(1, languageServer.RequestCount);

        var edits = new[] { text.GetTextChange(result.TextEdit) };
        var newText = text.WithChanges(edits).ToString();
        Assert.Equal(expected, newText);
    }
}
