// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.AspNetCore.Razor.LanguageServer.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.Mef;
using Microsoft.CodeAnalysis.Completion;
using Microsoft.CodeAnalysis.Razor.Workspaces.Extensions;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.LanguageServerClient.Razor.Extensions;
using Microsoft.VisualStudio.LanguageServerClient.Razor.Test;
using Microsoft.VisualStudio.Test;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Operations;
using Microsoft.VisualStudio.Threading;
using Moq;
using Xunit;
using Xunit.Abstractions;
using CompletionContext = Microsoft.VisualStudio.LanguageServer.Protocol.CompletionContext;
using CompletionItem = Microsoft.VisualStudio.LanguageServer.Protocol.CompletionItem;
using CompletionList = Microsoft.VisualStudio.LanguageServer.Protocol.CompletionList;
using CompletionOptions = Microsoft.VisualStudio.LanguageServer.Protocol.CompletionOptions;
using CompletionTriggerKind = Microsoft.VisualStudio.LanguageServer.Protocol.CompletionTriggerKind;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor.HtmlCSharp;

[UseExportProvider]
public class CompletionHandlerTest : HandlerTestBase
{
    private const string LanguageClient = "languageClient";

    private readonly ITextStructureNavigatorSelectorService _textStructureNavigatorSelectorService;
    private readonly CompletionRequestContextCache _completionRequestContextCache;
    private readonly FormattingOptionsProvider _formattingOptionsProvider;
    private readonly TestTextBuffer _textBuffer;
    private readonly CSharpVirtualDocumentSnapshot _csharpVirtualDocumentSnapshot;
    private readonly HtmlVirtualDocumentSnapshot _htmlVirtualDocumentSnapshot;
    private readonly Uri _uri;
    private readonly ServerCapabilities _completionServerCapabilities;

    public CompletionHandlerTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        _textStructureNavigatorSelectorService = new TestTextStructureNavigatorSelectorService();
        _uri = new Uri("C:/path/to/file.razor");
        _completionRequestContextCache = new CompletionRequestContextCache();
        _formattingOptionsProvider = TestFormattingOptionsProvider.Default;
        _textBuffer = new TestTextBuffer(new StringTextSnapshot(string.Empty));
        _csharpVirtualDocumentSnapshot = new CSharpVirtualDocumentSnapshot(new Uri("C:/path/to/file.razor.g.cs"), _textBuffer.CurrentSnapshot, 0);
        _htmlVirtualDocumentSnapshot = new HtmlVirtualDocumentSnapshot(new Uri("C:/path/to/file.razor__virtual.html"), _textBuffer.CurrentSnapshot, 0);

        _completionServerCapabilities = new()
        {
            CompletionProvider = new CompletionOptions
            {
                ResolveProvider = true,
                AllCommitCharacters = CompletionRules.Default.DefaultCommitCharacters.Select(c => c.ToString()).ToArray(),
                TriggerCharacters = CompletionHandler.AllTriggerCharacters.ToArray(),
            }
        };
    }

    [Fact]
    public async Task HandleRequestAsync_DocumentNotFound_ReturnsNull()
    {
        // Arrange
        var documentManager = new TestDocumentManager();
        var requestInvoker = new TestLSPRequestInvoker();
        var projectionProvider = new TestLSPProjectionProvider(LoggerFactory);
        var completionHandler = new CompletionHandler(
            JoinableTaskContext, requestInvoker, documentManager, projectionProvider, _textStructureNavigatorSelectorService, _completionRequestContextCache, _formattingOptionsProvider, LoggerProvider);
        var completionRequest = new CompletionParams()
        {
            TextDocument = new TextDocumentIdentifier() { Uri = _uri },
            Context = new CompletionContext() { TriggerKind = CompletionTriggerKind.Invoked },
            Position = new Position(0, 1)
        };

        // Act
        var result = await completionHandler.HandleRequestAsync(completionRequest, new ClientCapabilities(), DisposalToken);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task HandleRequestAsync_HtmlProjection_InvokesHtmlLanguageServer()
    {
        // Arrange
        var called = false;
        var expectedItem = new CompletionItem() { InsertText = "Sample" };
        var completionRequest = new CompletionParams()
        {
            TextDocument = new TextDocumentIdentifier() { Uri = _uri },
            Context = new VSInternalCompletionContext() { TriggerKind = CompletionTriggerKind.TriggerCharacter, TriggerCharacter = "<", InvokeKind = VSInternalCompletionInvokeKind.Typing },
            Position = new Position(0, 1)
        };

        var documentManager = new TestDocumentManager();
        documentManager.AddDocument(_uri, new TestLSPDocumentSnapshot(new Uri("C:/path/file.razor"), 0, _htmlVirtualDocumentSnapshot));

        var requestInvoker = new Mock<LSPRequestInvoker>(MockBehavior.Strict);
        requestInvoker
            .Setup(r => r.ReinvokeRequestOnServerAsync<CompletionParams, SumType<CompletionItem[], CompletionList>?>(_textBuffer, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CompletionParams>(), It.IsAny<CancellationToken>()))
            .Callback<ITextBuffer, string, string, CompletionParams, CancellationToken>((textBuffer, method, clientName, completionParams, ct) =>
            {
                Assert.Equal(Methods.TextDocumentCompletionName, method);
                Assert.Equal(RazorLSPConstants.HtmlLanguageServerName, clientName);
                var vsCompletionContext = Assert.IsType<VSInternalCompletionContext>(completionParams.Context);
                Assert.Equal(VSInternalCompletionInvokeKind.Typing, vsCompletionContext.InvokeKind);
                called = true;
            })
            .ReturnsAsync(new ReinvocationResponse<SumType<CompletionItem[], CompletionList>?>(LanguageClient, new[] { expectedItem }));

        var projectionResult = new ProjectionResult()
        {
            Uri = null,
            Position = null,
            LanguageKind = RazorLanguageKind.Html,
        };
        var projectionProvider = new Mock<LSPProjectionProvider>(MockBehavior.Strict);
        projectionProvider
            .Setup(p => p.GetProjectionForCompletionAsync(
                It.IsAny<LSPDocumentSnapshot>(),
                It.IsAny<Position>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(projectionResult);

        var completionHandler = new CompletionHandler(JoinableTaskContext, requestInvoker.Object, documentManager, projectionProvider.Object, _textStructureNavigatorSelectorService, _completionRequestContextCache, _formattingOptionsProvider, LoggerProvider);

        // Act
        var result = await completionHandler.HandleRequestAsync(completionRequest, new ClientCapabilities(), DisposalToken);

        // Assert
        Assert.True(called);
        var item = Assert.Single(((CompletionList)result.Value).Items);
        Assert.Equal(expectedItem.InsertText, item.InsertText);
    }

    [Fact]
    public async Task HandleRequestAsync_HtmlProjection_NoCompletions()
    {
        // Arrange
        var completionRequest = new CompletionParams()
        {
            TextDocument = new TextDocumentIdentifier() { Uri = _uri },
            Context = new VSInternalCompletionContext() { TriggerKind = CompletionTriggerKind.TriggerCharacter, TriggerCharacter = "<", InvokeKind = VSInternalCompletionInvokeKind.Typing },
            Position = new Position(0, 1)
        };

        var documentManager = new TestDocumentManager();
        documentManager.AddDocument(_uri, new TestLSPDocumentSnapshot(new Uri("C:/path/file.razor"), 0, _htmlVirtualDocumentSnapshot));

        var requestInvoker = new Mock<LSPRequestInvoker>(MockBehavior.Strict);
        requestInvoker
            .Setup(r => r.ReinvokeRequestOnServerAsync<CompletionParams, SumType<CompletionItem[], CompletionList>?>(_textBuffer, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CompletionParams>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ReinvocationResponse<SumType<CompletionItem[], CompletionList>?>(string.Empty, null))
            .Verifiable();

        var projectionResult = new ProjectionResult()
        {
            Uri = null,
            Position = null,
            LanguageKind = RazorLanguageKind.Html,
        };
        var projectionProvider = new Mock<LSPProjectionProvider>(MockBehavior.Strict);
        projectionProvider
            .Setup(p => p.GetProjectionForCompletionAsync(
                It.IsAny<LSPDocumentSnapshot>(),
                It.IsAny<Position>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(projectionResult);

        var completionHandler = new CompletionHandler(JoinableTaskContext, requestInvoker.Object, documentManager, projectionProvider.Object, _textStructureNavigatorSelectorService, _completionRequestContextCache, _formattingOptionsProvider, LoggerProvider);

        // Act
        var result = await completionHandler.HandleRequestAsync(completionRequest, new ClientCapabilities(), DisposalToken);

        // Assert
        requestInvoker.VerifyAll();
        Assert.Null(result);
    }

    [Fact]
    public async Task HandleRequestAsync_CSharpProjection_InvokesCSharpLanguageServer()
    {
        // Arrange
        var text =
            """
                @using System
                @DateT
                """;

        var cursorPosition = new Position(1, 6);
        var documentUri = new Uri("C:/path/to/file.razor");
        var completionParams = new CompletionParams()
        {
            TextDocument = new TextDocumentIdentifier() { Uri = documentUri },
            Context = new VSInternalCompletionContext()
            {
                TriggerKind = CompletionTriggerKind.Invoked,
                InvokeKind = VSInternalCompletionInvokeKind.Explicit
            },
            Position = cursorPosition
        };

        // Act
        var result = await ExecuteCSharpCompletionRequestAsync(documentUri, text, completionParams);

        // Assert
        var vsCompletionList = Assert.IsType<OptimizedVSCompletionList>(result.Value.Value);
        var item = vsCompletionList.Items.First();
        Assert.Equal("DateTime", item.Label);
    }

    [Fact]
    public async Task HandleRequestAsync_CSharpProjection_DoNotReturnCSharpCompletionsInNonCSharpContextAsync()
    {
        // Arrange
        var documentManager = new TestDocumentManager();
        documentManager.AddDocument(_uri, new TestLSPDocumentSnapshot(new Uri("C:/path/file.razor"), 0, _csharpVirtualDocumentSnapshot, _htmlVirtualDocumentSnapshot));
        var requestInvoker = new TestLSPRequestInvoker();
        var projectionProvider = new TestLSPProjectionProvider(LoggerFactory);
        var completionHandler = new CompletionHandler(
            JoinableTaskContext, requestInvoker, documentManager, projectionProvider, _textStructureNavigatorSelectorService, _completionRequestContextCache, _formattingOptionsProvider, LoggerProvider);
        var completionRequest = new CompletionParams()
        {
            TextDocument = new TextDocumentIdentifier() { Uri = _uri },
            Context = new CompletionContext() { TriggerKind = CompletionTriggerKind.Invoked },
            Position = new Position(0, 1)
        };

        // Act
        var result = await completionHandler.HandleRequestAsync(completionRequest, new ClientCapabilities(), DisposalToken);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void HandleRequestAsync_CSharpProjection_RewriteCompletionContext()
    {
        var completionRequest = new CompletionParams()
        {
            TextDocument = new TextDocumentIdentifier() { Uri = _uri },
            Context = new CompletionContext()
            {
                TriggerKind = CompletionTriggerKind.TriggerCharacter,
                TriggerCharacter = "@",
            },
            Position = new Position(0, 1),
        };

        var rewrittenContext = CompletionHandler.RewriteContext(completionRequest.Context, RazorLanguageKind.CSharp);
        Assert.True(rewrittenContext.TriggerKind == CompletionTriggerKind.Invoked);
        Assert.True(((VSInternalCompletionContext)rewrittenContext).InvokeKind == VSInternalCompletionInvokeKind.Explicit);
    }

    [Fact]
    public async Task HandleRequestAsync_CSharpProjection_DoNotPreselectAfterAt()
    {
        // Arrange
        var text =
            """
                @using System
                @
                """;

        var cursorPosition = new Position(1, 1);
        var documentUri = new Uri("C:/path/to/file.razor");
        var completionParams = new CompletionParams()
        {
            TextDocument = new TextDocumentIdentifier() { Uri = documentUri },
            Context = new VSInternalCompletionContext()
            {
                TriggerKind = CompletionTriggerKind.Invoked,
                InvokeKind = VSInternalCompletionInvokeKind.Explicit
            },
            Position = cursorPosition
        };

        // Act
        var result = await ExecuteCSharpCompletionRequestAsync(documentUri, text, completionParams);

        // Assert
        var vsCompletionList = Assert.IsType<OptimizedVSCompletionList>(result.Value.Value);
        Assert.DoesNotContain(true, vsCompletionList.Items.Select(c => c.Preselect));
    }

    [Fact]
    public async Task HandleRequestAsync_CSharpProjection_ReturnsCSharpSnippets()
    {
        // Arrange
        var text =
            """
                @
                """;

        var cursorPosition = new Position(0, 1);
        var documentUri = new Uri("C:/path/to/file.razor");
        var completionParams = new CompletionParams()
        {
            TextDocument = new TextDocumentIdentifier() { Uri = documentUri },
            Context = new VSInternalCompletionContext()
            {
                TriggerKind = CompletionTriggerKind.TriggerCharacter,
                TriggerCharacter = "@",
                InvokeKind = VSInternalCompletionInvokeKind.Typing
            },
            Position = cursorPosition
        };

        // Act
        var result = await ExecuteCSharpCompletionRequestAsync(documentUri, text, completionParams);

        // Assert
        var vsCompletionList = Assert.IsType<OptimizedVSCompletionList>(result.Value.Value);
        var itemLabels = vsCompletionList.Items.Select(c => c.Label);
        Assert.Contains("for (...)", itemLabels);
    }

    [Fact]
    public async Task HandleRequestAsync_HtmlProjection_IncompatibleTriggerCharacter_ReturnsNull()
    {
        // Arrange
        var completionRequest = new CompletionParams()
        {
            TextDocument = new TextDocumentIdentifier() { Uri = _uri },
            Context = new CompletionContext() { TriggerKind = CompletionTriggerKind.TriggerCharacter, TriggerCharacter = "~" },
            Position = new Position(0, 1)
        };

        var documentManager = new TestDocumentManager();
        documentManager.AddDocument(_uri, new TestLSPDocumentSnapshot(new Uri("C:/path/file.razor"), 0, _csharpVirtualDocumentSnapshot));

        var requestInvoker = new Mock<LSPRequestInvoker>(MockBehavior.Strict);

        var projectionResult = new ProjectionResult()
        {
            Uri = null,
            Position = null,
            LanguageKind = RazorLanguageKind.Html,
        };
        var projectionProvider = new Mock<LSPProjectionProvider>(MockBehavior.Strict);
        projectionProvider
            .Setup(p => p.GetProjectionForCompletionAsync(
                It.IsAny<LSPDocumentSnapshot>(),
                It.IsAny<Position>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(projectionResult);

        var completionHandler = new CompletionHandler(JoinableTaskContext, requestInvoker.Object, documentManager, projectionProvider.Object, _textStructureNavigatorSelectorService, _completionRequestContextCache, _formattingOptionsProvider, LoggerProvider);

        // Act
        var result = await completionHandler.HandleRequestAsync(completionRequest, new ClientCapabilities(), DisposalToken);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task HandleRequestAsync_CSharpProjection_IncompatibleTriggerCharacter_ReturnsNull()
    {
        // Arrange
        var text =
            """
                @code
                {
                    ;
                }
                """;

        var cursorPosition = new Position(2, 5);
        var documentUri = new Uri("C:/path/to/file.razor");
        var completionParams = new CompletionParams()
        {
            TextDocument = new TextDocumentIdentifier() { Uri = documentUri },
            Context = new VSInternalCompletionContext()
            {
                TriggerKind = CompletionTriggerKind.TriggerCharacter,
                TriggerCharacter = ";",
                InvokeKind = VSInternalCompletionInvokeKind.Typing
            },
            Position = cursorPosition
        };

        // Act
        var result = await ExecuteCSharpCompletionRequestAsync(documentUri, text, completionParams);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task HandleRequestAsync_CSharpProjection_IdentifierTriggerCharacter_InvokesCSharpLanguageServerNull()
    {
        // Arrange
        var text =
            """
                @using System;
                @Da
                """;

        var cursorPosition = new Position(1, 3);
        var documentUri = new Uri("C:/path/to/file.razor");
        var completionParams = new CompletionParams()
        {
            TextDocument = new TextDocumentIdentifier() { Uri = documentUri },
            Context = new VSInternalCompletionContext()
            {
                TriggerKind = CompletionTriggerKind.TriggerCharacter,
                TriggerCharacter = "a",
                InvokeKind = VSInternalCompletionInvokeKind.Typing
            },
            Position = cursorPosition
        };

        // Act
        var result = await ExecuteCSharpCompletionRequestAsync(documentUri, text, completionParams);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task HandleRequestAsync_CSharpProjection_TransitionTriggerCharacter_InvokesCSharpLanguageServerWithInvoke()
    {
        // Arrange
        var text =
            """
                @using System;
                @Da
                """;

        var cursorPosition = new Position(1, 3);
        var documentUri = new Uri("C:/path/to/file.razor");
        var completionParams = new CompletionParams()
        {
            TextDocument = new TextDocumentIdentifier() { Uri = documentUri },
            Context = new VSInternalCompletionContext()
            {
                TriggerKind = CompletionTriggerKind.Invoked,
                InvokeKind = VSInternalCompletionInvokeKind.Explicit
            },
            Position = cursorPosition
        };

        // Act
        var result = await ExecuteCSharpCompletionRequestAsync(documentUri, text, completionParams);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task HandleRequestAsync_CSharpProjection_ReturnOnlyPropertiesIfInsideInitializer()
    {
        // Arrange
        var text =
            """
                @using System
                @{
                    var date = new DateTime()
                    {
                        
                    };
                }
                """;

        var cursorPosition = new Position(4, 8);
        var documentUri = new Uri("C:/path/to/file.razor");
        var completionParams = new CompletionParams()
        {
            TextDocument = new TextDocumentIdentifier() { Uri = documentUri },
            Context = new VSInternalCompletionContext()
            {
                TriggerKind = CompletionTriggerKind.Invoked,
                InvokeKind = VSInternalCompletionInvokeKind.Explicit
            },
            Position = cursorPosition
        };

        // Act
        var result = await ExecuteCSharpCompletionRequestAsync(documentUri, text, completionParams);

        // Assert
        var vsCompletionList = Assert.IsType<OptimizedVSCompletionList>(result.Value.Value);
        Assert.Equal(2, vsCompletionList.Items.Length);

        var itemLabels = vsCompletionList.Items.Select(c => c.Label);
        Assert.Contains("DayOfWeek", itemLabels);
        Assert.Contains("Kind", itemLabels);
    }

    [Fact]
    public async Task HandleRequestAsync_CSharpProjection_RemoveAllDesignTimeHelpers()
    {
        // Arrange
        var text =
            """
                @using System
                @code
                {
                    
                }
                """;

        var cursorPosition = new Position(3, 4);
        var documentUri = new Uri("C:/path/to/file.razor");
        var completionParams = new CompletionParams()
        {
            TextDocument = new TextDocumentIdentifier() { Uri = documentUri },
            Context = new VSInternalCompletionContext()
            {
                TriggerKind = CompletionTriggerKind.Invoked,
                InvokeKind = VSInternalCompletionInvokeKind.Explicit
            },
            Position = cursorPosition
        };

        // Act
        var result = await ExecuteCSharpCompletionRequestAsync(documentUri, text, completionParams);

        // Assert
        var vsCompletionList = Assert.IsType<OptimizedVSCompletionList>(result.Value.Value);

        var itemLabels = vsCompletionList.Items.Select(c => c.Label);
        Assert.DoesNotContain("BuildRenderTree", itemLabels);
        Assert.DoesNotContain("__o", itemLabels);
        Assert.DoesNotContain("__RazorDirectiveTokenHelpers__", itemLabels);
        Assert.DoesNotContain("_Imports", itemLabels);
        Assert.Contains("DateTime", itemLabels);
    }

    [Fact]
    public async Task HandleRequestAsync_CSharpProjection_OnlyRemoveCommonDesignTimeHelpers()
    {
        // Arrange
        var text =
            """
                @using System
                @{ void M() { var foo = 1; f } }
                """;

        var cursorPosition = new Position(1, 28);
        var documentUri = new Uri("C:/path/to/file.razor");
        var completionParams = new CompletionParams()
        {
            TextDocument = new TextDocumentIdentifier() { Uri = documentUri },
            Context = new VSInternalCompletionContext()
            {
                TriggerKind = CompletionTriggerKind.TriggerCharacter,
                TriggerCharacter = "f",
                InvokeKind = VSInternalCompletionInvokeKind.Typing
            },
            Position = cursorPosition
        };

        // Act
        var result = await ExecuteCSharpCompletionRequestAsync(documentUri, text, completionParams);

        // Assert
        var vsCompletionList = Assert.IsType<OptimizedVSCompletionList>(result.Value.Value);

        var itemLabels = vsCompletionList.Items.Select(c => c.Label);
        Assert.DoesNotContain("BuildRenderTree", itemLabels);
        Assert.DoesNotContain("__o", itemLabels);
        Assert.DoesNotContain("__RazorDirectiveTokenHelpers__", itemLabels);
        Assert.DoesNotContain("_Imports", itemLabels);
        Assert.Contains("foo", itemLabels);
    }

    [Fact]
    public async Task HandleRequestAsync_HtmlProjection_IdentifierTriggerCharacter_InvokesHtmlLanguageServer()
    {
        // Arrange
        var called = false;
        var expectedItem = new CompletionItem() { InsertText = "Sample" };
        var completionRequest = new CompletionParams()
        {
            TextDocument = new TextDocumentIdentifier() { Uri = _uri },
            Context = new CompletionContext() { TriggerKind = CompletionTriggerKind.TriggerCharacter, TriggerCharacter = "h" },
            Position = new Position(0, 1)
        };

        var documentManager = new TestDocumentManager();
        documentManager.AddDocument(_uri, new TestLSPDocumentSnapshot(new Uri("C:/path/file.razor"), 0, _htmlVirtualDocumentSnapshot));

        var requestInvoker = new Mock<LSPRequestInvoker>(MockBehavior.Strict);
        requestInvoker
            .Setup(r => r.ReinvokeRequestOnServerAsync<CompletionParams, SumType<CompletionItem[], CompletionList>?>(_textBuffer, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CompletionParams>(), It.IsAny<CancellationToken>()))
            .Callback<ITextBuffer, string, string, CompletionParams, CancellationToken>((textBuffer, method, clientName, completionParams, ct) =>
            {
                Assert.Equal(Methods.TextDocumentCompletionName, method);
                Assert.Equal(RazorLSPConstants.HtmlLanguageServerName, clientName);
                called = true;
            })
            .ReturnsAsync(new ReinvocationResponse<SumType<CompletionItem[], CompletionList>?>(LanguageClient, new[] { expectedItem }));

        var projectionResult = new ProjectionResult()
        {
            Uri = null,
            Position = null,
            LanguageKind = RazorLanguageKind.Html,
        };

        var projectionProvider = new Mock<LSPProjectionProvider>(MockBehavior.Strict);
        projectionProvider
            .Setup(p => p.GetProjectionForCompletionAsync(
                It.IsAny<LSPDocumentSnapshot>(),
                It.IsAny<Position>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(projectionResult);

        var completionHandler = new CompletionHandler(JoinableTaskContext, requestInvoker.Object, documentManager, projectionProvider.Object, _textStructureNavigatorSelectorService, _completionRequestContextCache, _formattingOptionsProvider, LoggerProvider);

        // Act
        var result = await completionHandler.HandleRequestAsync(completionRequest, new ClientCapabilities(), DisposalToken);

        // Assert
        Assert.True(called);
        var item = Assert.Single(((CompletionList)result.Value).Items);
        Assert.Equal(expectedItem.InsertText, item.InsertText);
    }

    [Fact]
    public void SetResolveData_RewritesData()
    {
        // Arrange
        var originalData = new object();
        var items = new[]
        {
            new CompletionItem() { InsertText = "Hello", Data = originalData }
        };
        var completionList = new CompletionList()
        {
            Items = items,
        };

        // Act
        CompletionHandler.SetResolveData(123, completionList);

        // Assert
        var item = Assert.Single(completionList.Items);
        var newData = Assert.IsType<CompletionResolveData>(item.Data);
        Assert.Same(originalData, newData.OriginalData);
    }

    [Fact]
    public void SetResolveData_RewritesCompletionListData()
    {
        // Arrange
        var originalData = new object();
        var completionList = new VSInternalCompletionList()
        {
            Items = new[]
            {
                new CompletionItem() { InsertText = "Hello" }
            },
            Data = originalData
        };

        // Act
        CompletionHandler.SetResolveData(123, completionList);

        // Assert
        var newData = Assert.IsType<CompletionResolveData>(completionList.Data);
        Assert.Same(originalData, newData.OriginalData);
    }

    [Fact]
    public async Task HandleRequestAsync_ProvisionalCompletion()
    {
        // Arrange
        var text =
            """
                @using System
                @DateTime.
                """;

        var cursorPosition = new Position(1, 10);
        var documentUri = new Uri("C:/path/to/file.razor");
        var completionParams = new CompletionParams()
        {
            TextDocument = new TextDocumentIdentifier() { Uri = documentUri },
            Context = new VSInternalCompletionContext()
            {
                TriggerKind = CompletionTriggerKind.TriggerCharacter,
                TriggerCharacter = ".",
                InvokeKind = VSInternalCompletionInvokeKind.Typing
            },
            Position = cursorPosition
        };

        // Act
        var result = await ExecuteCSharpCompletionRequestAsync(documentUri, text, completionParams);

        // Assert
        var vsCompletionList = Assert.IsType<OptimizedVSCompletionList>(result.Value.Value);

        var itemLabels = vsCompletionList.Items.Select(c => c.Label);
        Assert.Contains("Now", itemLabels);
    }

    [Fact]
    public async Task TryGetProvisionalCompletionsAsync_CSharpProjection_ReturnsFalse()
    {
        // Arrange
        var completionRequest = new CompletionParams()
        {
            TextDocument = new TextDocumentIdentifier() { Uri = _uri },
            Context = new CompletionContext()
            {
                TriggerKind = CompletionTriggerKind.TriggerCharacter,
                TriggerCharacter = "."
            },
            Position = new Position(0, 1)
        };

        var documentManager = new TestDocumentManager();
        var documentSnapshot = new TestLSPDocumentSnapshot(new Uri("C:/path/file.razor"), 0, _csharpVirtualDocumentSnapshot);
        documentManager.AddDocument(_uri, documentSnapshot);
        var requestInvoker = new TestLSPRequestInvoker();
        var projectionProvider = new TestLSPProjectionProvider(LoggerFactory);
        var completionHandler = new CompletionHandler(
            JoinableTaskContext, requestInvoker, documentManager, projectionProvider, _textStructureNavigatorSelectorService, _completionRequestContextCache, _formattingOptionsProvider, LoggerProvider);

        var projectionResult = new ProjectionResult()
        {
            Uri = null,
            Position = null,
            LanguageKind = RazorLanguageKind.CSharp,
        };

        // Act
        var provisionalCompletionResult = await completionHandler.TryGetProvisionalCompletionsAsync(completionRequest, documentSnapshot, projectionResult, DisposalToken);

        // Assert
        Assert.False(provisionalCompletionResult.Success);
        Assert.Null(provisionalCompletionResult.Result);
    }

    [Fact]
    public async Task TryGetProvisionalCompletionsAsync_RevertsDotOnFailures()
    {
        // Arrange
        var text =
            """
                @using System
                @Date.
                """;

        var cursorPosition = new Position(1, 6);
        var documentUri = new Uri("C:/path/to/file.razor");
        var completionParams = new CompletionParams()
        {
            TextDocument = new TextDocumentIdentifier() { Uri = documentUri },
            Context = new VSInternalCompletionContext()
            {
                TriggerKind = CompletionTriggerKind.TriggerCharacter,
                TriggerCharacter = ".",
                InvokeKind = VSInternalCompletionInvokeKind.Typing
            },
            Position = cursorPosition
        };

        var csharpDocumentUri = new Uri("C:/path/to/file.razor__virtual.cs");
        var codeDocument = CreateCodeDocument(text, documentUri.AbsolutePath);
        var csharpSourceText = codeDocument.GetCSharpSourceText();

        var csharpDocumentSnapshot = CreateCSharpVirtualDocumentSnapshot(codeDocument, csharpDocumentUri.AbsoluteUri);
        var razorSourceText = codeDocument.GetSourceText();
        var documentSnapshot = new TestLSPDocumentSnapshot(
            documentUri,
            version: 1,
            snapshotContent: razorSourceText.ToString(),
            csharpDocumentSnapshot);

        var uriToCodeDocumentMap = new Dictionary<Uri, (int hostDocumentVersion, RazorCodeDocument codeDocument)>
        {
            { documentUri, (hostDocumentVersion: 1, codeDocument) }
        };

        var mappingProvider = new TestLSPDocumentMappingProvider(uriToCodeDocumentMap, LoggerFactory);
        var razorSpanMappingService = new TestRazorLSPSpanMappingService(
            mappingProvider, documentUri, razorSourceText, csharpSourceText, DisposalToken);

        await using var csharpServer = await CSharpTestLspServerHelpers.CreateCSharpLspServerAsync(
            csharpSourceText, csharpDocumentUri, _completionServerCapabilities, razorSpanMappingService, DisposalToken);

        await csharpServer.OpenDocumentAsync(csharpDocumentUri, csharpSourceText.ToString());

        var requestInvoker = new TestLSPRequestInvoker(csharpServer);
        var documentManager = new TestDocumentManager(csharpServer);
        documentManager.AddDocument(documentUri, documentSnapshot);
        var projectionProvider = new TestLSPProjectionProvider(LoggerFactory);
        var completionHandler = new CompletionHandler(
            JoinableTaskContext, requestInvoker, documentManager, projectionProvider, _textStructureNavigatorSelectorService, _completionRequestContextCache, _formattingOptionsProvider, LoggerProvider);

        var projectionResult = new ProjectionResult()
        {
            Uri = null,
            Position = cursorPosition,
            LanguageKind = RazorLanguageKind.Html,
        };

        // Act
        var provisionalCompletionResult = await completionHandler.TryGetProvisionalCompletionsAsync(completionParams, documentSnapshot, projectionResult, DisposalToken);

        // Assert
        Assert.False(provisionalCompletionResult.Success);
        Assert.Equal(2, documentManager.UpdateVirtualDocumentCallCount);
        Assert.False(provisionalCompletionResult.Result.HasValue);
    }

    [Fact]
    public async Task TryGetProvisionalCompletionsAsync_TriggerCharacterNotDot_ReturnsFalse()
    {
        // Arrange
        var completionRequest = new CompletionParams()
        {
            TextDocument = new TextDocumentIdentifier() { Uri = _uri },
            Context = new CompletionContext()
            {
                TriggerKind = CompletionTriggerKind.TriggerCharacter,
                TriggerCharacter = "D"
            },
            Position = new Position(0, 1)
        };

        var documentManager = new TestDocumentManager();
        var documentSnapshot = new TestLSPDocumentSnapshot(new Uri("C:/path/file.razor"), 0, _csharpVirtualDocumentSnapshot);
        documentManager.AddDocument(_uri, documentSnapshot);
        var requestInvoker = new TestLSPRequestInvoker();
        var projectionProvider = new TestLSPProjectionProvider(LoggerFactory);
        var completionHandler = new CompletionHandler(
            JoinableTaskContext, requestInvoker, documentManager, projectionProvider, _textStructureNavigatorSelectorService, _completionRequestContextCache, _formattingOptionsProvider, LoggerProvider);

        var projectionResult = new ProjectionResult()
        {
            Uri = null,
            Position = null,
            LanguageKind = RazorLanguageKind.CSharp,
        };

        // Act
        var provisionalCompletionResult = await completionHandler.TryGetProvisionalCompletionsAsync(completionRequest, documentSnapshot, projectionResult, DisposalToken);

        // Assert
        Assert.False(provisionalCompletionResult.Success);
        Assert.Null(provisionalCompletionResult.Result);
    }

    [Fact]
    public async Task TryGetProvisionalCompletionsAsync_PreviousCharacterHtml_ReturnsFalse()
    {
        // Arrange
        var text =
            """
                Test.
                """;

        var cursorPosition = new Position(0, 5);
        var documentUri = new Uri("C:/path/to/file.razor");
        var completionRequest = new CompletionParams()
        {
            TextDocument = new TextDocumentIdentifier() { Uri = _uri },
            Context = new CompletionContext()
            {
                TriggerKind = CompletionTriggerKind.TriggerCharacter,
                TriggerCharacter = "."
            },
            Position = new Position(0, 1)
        };

        var documentSnapshot = new TestLSPDocumentSnapshot(
            documentUri,
            version: 1,
            snapshotContent: text);
        var documentManager = new TestDocumentManager();
        documentManager.AddDocument(documentUri, documentSnapshot);
        var requestInvoker = new TestLSPRequestInvoker();
        var projectionProvider = new TestLSPProjectionProvider(LoggerFactory);
        var completionHandler = new CompletionHandler(
            JoinableTaskContext, requestInvoker, documentManager, projectionProvider, _textStructureNavigatorSelectorService, _completionRequestContextCache, _formattingOptionsProvider, LoggerProvider);

        var projectionResult = new ProjectionResult()
        {
            Uri = null,
            Position = cursorPosition,
            LanguageKind = RazorLanguageKind.Html,
        };

        // Act
        var provisionalCompletionResult = await completionHandler.TryGetProvisionalCompletionsAsync(completionRequest, documentSnapshot, projectionResult, DisposalToken);

        // Assert
        Assert.False(provisionalCompletionResult.Success);
        Assert.Null(provisionalCompletionResult.Result);
    }

    [Fact]
    public async Task TryGetProvisionalCompletionsAsync_ProjectionAtStartOfLine_ReturnsFalse()
    {
        // Arrange
        var text =
            """
                .
                """;

        var cursorPosition = new Position(0, 1);
        var documentUri = new Uri("C:/path/to/file.razor");
        var completionParams = new CompletionParams()
        {
            TextDocument = new TextDocumentIdentifier() { Uri = documentUri },
            Context = new VSInternalCompletionContext()
            {
                TriggerKind = CompletionTriggerKind.TriggerCharacter,
                TriggerCharacter = ".",
                InvokeKind = VSInternalCompletionInvokeKind.Typing
            },
            Position = cursorPosition
        };

        var csharpDocumentUri = new Uri("C:/path/to/file.razor__virtual.cs");
        var codeDocument = CreateCodeDocument(text, documentUri.AbsolutePath);
        var csharpSourceText = codeDocument.GetCSharpSourceText();

        var csharpDocumentSnapshot = CreateCSharpVirtualDocumentSnapshot(codeDocument, csharpDocumentUri.AbsoluteUri);
        var razorSourceText = codeDocument.GetSourceText();
        var documentSnapshot = new TestLSPDocumentSnapshot(
            documentUri,
            version: 1,
            snapshotContent: razorSourceText.ToString(),
            csharpDocumentSnapshot);

        var uriToCodeDocumentMap = new Dictionary<Uri, (int hostDocumentVersion, RazorCodeDocument codeDocument)>
        {
            { documentUri, (hostDocumentVersion: 1, codeDocument) }
        };
        var mappingProvider = new TestLSPDocumentMappingProvider(uriToCodeDocumentMap, LoggerFactory);
        var razorSpanMappingService = new TestRazorLSPSpanMappingService(
            mappingProvider, documentUri, razorSourceText, csharpSourceText, DisposalToken);

        await using var csharpServer = await CSharpTestLspServerHelpers.CreateCSharpLspServerAsync(
            csharpSourceText, csharpDocumentUri, _completionServerCapabilities, razorSpanMappingService, DisposalToken);

        await csharpServer.OpenDocumentAsync(csharpDocumentUri, csharpSourceText.ToString());

        var requestInvoker = new TestLSPRequestInvoker(csharpServer);
        var documentManager = new TestDocumentManager(csharpServer);
        documentManager.AddDocument(documentUri, documentSnapshot);
        var projectionProvider = new TestLSPProjectionProvider(LoggerFactory);
        var completionHandler = new CompletionHandler(
            JoinableTaskContext, requestInvoker, documentManager, projectionProvider, _textStructureNavigatorSelectorService, _completionRequestContextCache, _formattingOptionsProvider, LoggerProvider);

        var projectionResult = new ProjectionResult()
        {
            Uri = null,
            Position = cursorPosition,
            LanguageKind = RazorLanguageKind.Html,
        };

        // Act
        var provisionalCompletionResult = await completionHandler.TryGetProvisionalCompletionsAsync(completionParams, documentSnapshot, projectionResult, DisposalToken);

        // Assert
        Assert.False(provisionalCompletionResult.Success);
        Assert.Null(provisionalCompletionResult.Result);
    }

    [Fact]
    public async Task TryGetProvisionalCompletionsAsync_NullHostDocumentVersion_ReturnsFalse()
    {
        // Arrange
        var text =
            """
                @using System
                @DateTime.
                """;

        var cursorPosition = new Position(1, 10);
        var documentUri = new Uri("C:/path/to/file.razor");
        var completionParams = new CompletionParams()
        {
            TextDocument = new TextDocumentIdentifier() { Uri = documentUri },
            Context = new VSInternalCompletionContext()
            {
                TriggerKind = CompletionTriggerKind.TriggerCharacter,
                TriggerCharacter = ".",
                InvokeKind = VSInternalCompletionInvokeKind.Typing
            },
            Position = cursorPosition
        };

        var csharpDocumentUri = new Uri("C:/path/to/file.razor__virtual.cs");
        var codeDocument = CreateCodeDocument(text, documentUri.AbsolutePath);
        var csharpSourceText = codeDocument.GetCSharpSourceText();

        var csharpDocumentSnapshot = CreateCSharpVirtualDocumentSnapshot(codeDocument, csharpDocumentUri.AbsoluteUri, hostDocumentSyncVersion: null);
        var razorSourceText = codeDocument.GetSourceText();
        var documentSnapshot = new TestLSPDocumentSnapshot(
            documentUri,
            version: 1,
            snapshotContent: razorSourceText.ToString(),
            csharpDocumentSnapshot);

        var uriToCodeDocumentMap = new Dictionary<Uri, (int hostDocumentVersion, RazorCodeDocument codeDocument)>
        {
            { documentUri, (hostDocumentVersion: 1, codeDocument) }
        };
        var mappingProvider = new TestLSPDocumentMappingProvider(uriToCodeDocumentMap, LoggerFactory);
        var razorSpanMappingService = new TestRazorLSPSpanMappingService(
            mappingProvider, documentUri, razorSourceText, csharpSourceText, DisposalToken);

        await using var csharpServer = await CSharpTestLspServerHelpers.CreateCSharpLspServerAsync(
            csharpSourceText, csharpDocumentUri, _completionServerCapabilities, razorSpanMappingService, DisposalToken);

        await csharpServer.OpenDocumentAsync(csharpDocumentUri, csharpSourceText.ToString());

        var requestInvoker = new TestLSPRequestInvoker(csharpServer);
        var documentManager = new TestDocumentManager(csharpServer);
        documentManager.AddDocument(documentUri, documentSnapshot);
        var projectionProvider = new TestLSPProjectionProvider(LoggerFactory);
        var completionHandler = new CompletionHandler(
            JoinableTaskContext, requestInvoker, documentManager, projectionProvider, _textStructureNavigatorSelectorService, _completionRequestContextCache, _formattingOptionsProvider, LoggerProvider);

        var projectionResult = new ProjectionResult()
        {
            Uri = null,
            Position = cursorPosition,
            LanguageKind = RazorLanguageKind.Html,
        };

        // Act
        var provisionalCompletionResult = await completionHandler.TryGetProvisionalCompletionsAsync(completionParams, documentSnapshot, projectionResult, DisposalToken);

        // Assert
        Assert.False(provisionalCompletionResult.Success);
        Assert.Equal(0, documentManager.UpdateVirtualDocumentCallCount);
        Assert.Null(provisionalCompletionResult.Result);
    }

    [Fact]
    public void TriggerAppliedToProjection_Razor_ReturnsFalse()
    {
        // Arrange
        var context = new CompletionContext();

        // Act
        var result = CompletionHandler.TriggerAppliesToProjection(context, RazorLanguageKind.Razor);

        // Assert
        Assert.False(result);
    }

    [Theory]
    [InlineData(" ", CompletionTriggerKind.TriggerCharacter, true)]
    [InlineData("<", CompletionTriggerKind.TriggerCharacter, true)]
    [InlineData("&", CompletionTriggerKind.TriggerCharacter, true)]
    [InlineData("\\", CompletionTriggerKind.TriggerCharacter, true)]
    [InlineData("/", CompletionTriggerKind.TriggerCharacter, true)]
    [InlineData("'", CompletionTriggerKind.TriggerCharacter, true)]
    [InlineData("=", CompletionTriggerKind.TriggerCharacter, true)]
    [InlineData(":", CompletionTriggerKind.TriggerCharacter, true)]
    [InlineData("\"", CompletionTriggerKind.TriggerCharacter, true)]
    [InlineData(".", CompletionTriggerKind.TriggerCharacter, true)]
    [InlineData(".", CompletionTriggerKind.Invoked, true)]
    [InlineData("@", CompletionTriggerKind.TriggerCharacter, true)]
    [InlineData("@", CompletionTriggerKind.Invoked, true)]
    [InlineData("a", CompletionTriggerKind.TriggerCharacter, true)] // Auto-invoked from VS platform
    [InlineData("a", CompletionTriggerKind.Invoked, true)]
    public void TriggerAppliedToProjection_Html_ReturnsExpectedResult(string character, CompletionTriggerKind kind, bool expected)
    {
        // Arrange
        var context = new CompletionContext()
        {
            TriggerCharacter = character,
            TriggerKind = kind
        };

        // Act
        var result = CompletionHandler.TriggerAppliesToProjection(context, RazorLanguageKind.Html);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(".", CompletionTriggerKind.TriggerCharacter, true)]
    [InlineData("@", CompletionTriggerKind.TriggerCharacter, true)]
    [InlineData(" ", CompletionTriggerKind.TriggerCharacter, true)]
    [InlineData("&", CompletionTriggerKind.TriggerCharacter, false)]
    [InlineData("a", CompletionTriggerKind.TriggerCharacter, true)] // Auto-invoked from VS platform
    [InlineData("a", CompletionTriggerKind.Invoked, true)]
    public void TriggerAppliedToProjection_CSharp_ReturnsExpectedResult(string character, CompletionTriggerKind kind, bool expected)
    {
        // Arrange
        var context = new CompletionContext()
        {
            TriggerCharacter = character,
            TriggerKind = kind
        };

        // Act
        var result = CompletionHandler.TriggerAppliesToProjection(context, RazorLanguageKind.CSharp);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void TranslateTextEdits()
    {
        var razorDocPosition = new Position(line: 4, character: 9);
        var cSharpDocPosition = new Position(line: 99, character: 5);

        var documentSnapshot = new TestLSPDocumentSnapshot(new Uri("C:/path/file.razor"), 0, snapshotContent: @"@code
{
    void M()
    {
        M
    }
}");

        // Word 'M'
        var wordSnapshotSpan = new SnapshotSpan(documentSnapshot.Snapshot, new Span(39, 1));
        var wordRange = new TextExtent(wordSnapshotSpan, isSignificant: true);

        var completionList = new VSInternalCompletionList
        {
            Items = new CompletionItem[]
            {
                new CompletionItem
                {
                    TextEdit = new TextEdit
                    {
                        NewText = "M",
                        Range = new Range
                        {
                            Start = new Position
                            {
                                Line = 99,
                                Character = 4
                            },
                            End = new Position
                            {
                                Line = 99,
                                Character = 5
                            }
                        }
                    }
                }
            }
        };

        var expectedRange = new Range
        {
            Start = new Position
            {
                Line = 4,
                Character = 8
            },
            End = new Position
            {
                Line = 4,
                Character = 9
            }
        };

        var result = CompletionHandler.TranslateTextEdits(razorDocPosition, cSharpDocPosition, wordRange.Span.AsRange(), completionList);
        var actualRange = result.Items.First().TextEdit.Range;
        Assert.Equal(expectedRange, actualRange);
    }

    [Fact]
    public void GetBaseIndentation_Spaces()
    {
        // Arrange
        var snapshot = new StringTextSnapshot("    @i");
        var snapshotSpan = new SnapshotSpan(snapshot, new Span(5, 1));
        var wordExtent = new TextExtent(snapshotSpan, isSignificant: true);
        var formattingOptions = new FormattingOptions()
        {
            InsertSpaces = true,
            TabSize = 2,
        };

        // Assert
        var indentation = CompletionHandler.GetBaseIndentation(wordExtent, formattingOptions);

        // Act
        Assert.Equal(5, indentation);
    }

    [Fact]
    public void GetBaseIndentation_Tabs_ImplicitExpression()
    {
        // Arrange
        var snapshot = new StringTextSnapshot("\t\t@i");
        var snapshotSpan = new SnapshotSpan(snapshot, new Span(3, 1));
        var wordExtent = new TextExtent(snapshotSpan, isSignificant: true);
        var formattingOptions = new FormattingOptions()
        {
            InsertSpaces = false,
            TabSize = 3,
        };

        // Assert
        var indentation = CompletionHandler.GetBaseIndentation(wordExtent, formattingOptions);

        // Act
        Assert.Equal(7, indentation);
    }

    [Fact]
    public void GetBaseIndentation_Tabs_Text()
    {
        // Arrange
        var snapshot = new StringTextSnapshot("\t\ti");
        var snapshotSpan = new SnapshotSpan(snapshot, new Span(2, 1));
        var wordExtent = new TextExtent(snapshotSpan, isSignificant: true);
        var formattingOptions = new FormattingOptions()
        {
            InsertSpaces = false,
            TabSize = 3,
        };

        // Assert
        var indentation = CompletionHandler.GetBaseIndentation(wordExtent, formattingOptions);

        // Act
        Assert.Equal(6, indentation);
    }

    [Fact]
    public void GetBaseIndentation_Tabs_Mixed()
    {
        // Arrange
        var snapshot = new StringTextSnapshot("\t\t  i");
        var snapshotSpan = new SnapshotSpan(snapshot, new Span(4, 1));
        var wordExtent = new TextExtent(snapshotSpan, isSignificant: true);
        var formattingOptions = new FormattingOptions()
        {
            InsertSpaces = false,
            TabSize = 3,
        };

        // Assert
        var indentation = CompletionHandler.GetBaseIndentation(wordExtent, formattingOptions);

        // Act
        Assert.Equal(8, indentation);
    }

    [Fact]
    public async Task HandleRequestAsync_CSharpProjection_ItemDefault()
    {
        // Arrange
        var text =
            """
                @using System
                @DateTime.
                """;

        var cursorPosition = new Position(1, 10);
        var documentUri = new Uri("C:/path/to/file.razor");
        var completionParams = new CompletionParams()
        {
            TextDocument = new TextDocumentIdentifier() { Uri = documentUri },
            Context = new VSInternalCompletionContext()
            {
                TriggerKind = CompletionTriggerKind.TriggerCharacter,
                TriggerCharacter = ".",
                InvokeKind = VSInternalCompletionInvokeKind.Typing
            },
            Position = cursorPosition
        };

        var expectedRange = new Range
        {
            Start = cursorPosition,
            End = cursorPosition
        };

        // Act
        var result = await ExecuteCSharpCompletionRequestAsync(documentUri, text, completionParams);

        // Assert
        var vsCompletionList = Assert.IsType<OptimizedVSCompletionList>(result.Value.Value);

        Assert.Equal(expectedRange, vsCompletionList.ItemDefaults.EditRange);
    }

    private async Task<SumType<CompletionItem[], CompletionList>?> ExecuteCSharpCompletionRequestAsync(Uri documentUri, string text, CompletionParams completionParams)
    {
        var csharpDocumentUri = new Uri("C:/path/to/file.razor__virtual.cs");
        var codeDocument = CreateCodeDocument(text, documentUri.AbsolutePath);
        var csharpSourceText = codeDocument.GetCSharpSourceText();

        var csharpDocumentSnapshot = CreateCSharpVirtualDocumentSnapshot(codeDocument, csharpDocumentUri.AbsoluteUri);
        var razorSourceText = codeDocument.GetSourceText();
        var documentSnapshot = new TestLSPDocumentSnapshot(
            documentUri,
            version: 1,
            snapshotContent: razorSourceText.ToString(),
            csharpDocumentSnapshot);

        var uriToCodeDocumentMap = new Dictionary<Uri, (int hostDocumentVersion, RazorCodeDocument codeDocument)>
        {
            { documentUri, (hostDocumentVersion: 1, codeDocument) }
        };
        var mappingProvider = new TestLSPDocumentMappingProvider(uriToCodeDocumentMap, LoggerFactory);
        var razorSpanMappingService = new TestRazorLSPSpanMappingService(
            mappingProvider, documentUri, razorSourceText, csharpSourceText, DisposalToken);

        await using var csharpServer = await CSharpTestLspServerHelpers.CreateCSharpLspServerAsync(
            csharpSourceText, csharpDocumentUri, _completionServerCapabilities, razorSpanMappingService, DisposalToken);

        await csharpServer.OpenDocumentAsync(csharpDocumentUri, csharpSourceText.ToString());

        var requestInvoker = new TestLSPRequestInvoker(csharpServer);
        var documentManager = new TestDocumentManager(csharpServer);
        documentManager.AddDocument(documentUri, documentSnapshot);
        var projectionProvider = new TestLSPProjectionProvider(LoggerFactory);

        var completionHandler = new CompletionHandler(
            JoinableTaskContext, requestInvoker, documentManager, projectionProvider, _textStructureNavigatorSelectorService, _completionRequestContextCache, _formattingOptionsProvider, LoggerProvider);

        var result = await completionHandler.HandleRequestAsync(completionParams, new ClientCapabilities(), DisposalToken);

        return result;
    }
}
