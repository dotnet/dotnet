// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Formatting;
using Microsoft.AspNetCore.Razor.LanguageServer.Hover;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.AspNetCore.Razor.Threading;
using Microsoft.CodeAnalysis.Razor.Completion;
using Microsoft.CodeAnalysis.Razor.Formatting;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Tooltip;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Text;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Completion.Delegation;

public class DelegatedCompletionItemResolverTest : CompletionTestBase
{
    private static readonly VSInternalClientCapabilities s_clientCapabilities = new()
    {
        TextDocument = new()
        {
            Completion = new VSInternalCompletionSetting()
            {
                CompletionList = new()
                {
                    Data = true,
                }
            }
        }
    };

    private static readonly RazorCompletionOptions s_defaultRazorCompletionOptions = new(
        SnippetsSupported: true,
        AutoInsertAttributeQuotes: true,
        CommitElementsWithSpace: true);

    private readonly DelegatedCompletionParams _csharpCompletionParams;
    private readonly DelegatedCompletionParams _htmlCompletionParams;
    private readonly AsyncLazy<IRazorFormattingService> _lazyFormattingService;
    private readonly IComponentAvailabilityService _componentAvailabilityService;

    public DelegatedCompletionItemResolverTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        var documentContext = TestDocumentContext.Create("C:/path/to/file.cshtml");

        _csharpCompletionParams = new DelegatedCompletionParams(
            documentContext.GetTextDocumentIdentifierAndVersion(),
            LspFactory.CreatePosition(10, 6),
            RazorLanguageKind.CSharp,
            new VSInternalCompletionContext(),
            ProvisionalTextEdit: null,
            ShouldIncludeSnippets: false,
            CorrelationId: Guid.Empty);

        _htmlCompletionParams = new DelegatedCompletionParams(
            documentContext.GetTextDocumentIdentifierAndVersion(),
            LspFactory.DefaultPosition,
            RazorLanguageKind.Html,
            new VSInternalCompletionContext(),
            ProvisionalTextEdit: null,
            ShouldIncludeSnippets: false,
            CorrelationId: Guid.Empty);

        _lazyFormattingService = AsyncLazy.Create(_ => TestRazorFormattingService.CreateWithFullSupportAsync(LoggerFactory));

        var projectManager = CreateProjectSnapshotManager();
        _componentAvailabilityService = new ComponentAvailabilityService(projectManager);
    }

    [Fact]
    public async Task ResolveAsync_CanNotFindCompletionItem_Noops()
    {
        // Arrange
        var clientConnection = CreateClientConnectionForResolve(response: null);
        var optionsMonitor = TestRazorLSPOptionsMonitor.Create();
        var formattingService = await _lazyFormattingService.GetValueAsync(DisposalToken);
        var resolver = new DelegatedCompletionItemResolver(DocumentContextFactory, formattingService, DocumentMappingService, optionsMonitor, clientConnection, LoggerFactory);
        var item = new VSInternalCompletionItem();
        var notContainingCompletionList = new RazorVSInternalCompletionList() { Items = [] };
        var originalRequestContext = StrictMock.Of<ICompletionResolveContext>();

        // Act
        var resolvedItem = await resolver.ResolveAsync(
            item, notContainingCompletionList, originalRequestContext, s_clientCapabilities, _componentAvailabilityService, DisposalToken);

        // Assert
        Assert.Null(resolvedItem);
    }

    [Fact]
    public async Task ResolveAsync_UnknownRequestContext_Noops()
    {
        // Arrange
        var clientConnection = CreateClientConnectionForResolve(response: null);
        var optionsMonitor = TestRazorLSPOptionsMonitor.Create();
        var formattingService = await _lazyFormattingService.GetValueAsync(DisposalToken);
        var resolver = new DelegatedCompletionItemResolver(DocumentContextFactory, formattingService, DocumentMappingService, optionsMonitor, clientConnection, LoggerFactory);
        var item = new VSInternalCompletionItem();
        var containingCompletionList = new RazorVSInternalCompletionList() { Items = [item] };
        var originalRequestContext = StrictMock.Of<ICompletionResolveContext>();

        // Act
        var resolvedItem = await resolver.ResolveAsync(
            item, containingCompletionList, originalRequestContext, s_clientCapabilities, _componentAvailabilityService, DisposalToken);

        // Assert
        Assert.Null(resolvedItem);
    }

    [Fact]
    public async Task ResolveAsync_UsesItemsData()
    {
        // Arrange
        var expectedData = new object();
        var clientConnection = CreateClientConnectionForResolve(response: null, ValidateResolveParams);
        var optionsMonitor = TestRazorLSPOptionsMonitor.Create();
        var formattingService = await _lazyFormattingService.GetValueAsync(DisposalToken);
        var resolver = new DelegatedCompletionItemResolver(DocumentContextFactory, formattingService, DocumentMappingService, optionsMonitor, clientConnection, LoggerFactory);
        var item = new VSInternalCompletionItem()
        {
            Data = expectedData,
        };
        var containingCompletionList = new RazorVSInternalCompletionList() { Items = [item], Data = new object() };
        var originalRequestContext = new DelegatedCompletionResolutionContext(_csharpCompletionParams.Identifier, _csharpCompletionParams.ProjectedKind, new object());

        // Act
        await resolver.ResolveAsync(
            item, containingCompletionList, originalRequestContext, s_clientCapabilities, _componentAvailabilityService, DisposalToken);

        // Assert
        void ValidateResolveParams(DelegatedCompletionItemResolveParams @params)
        {
            Assert.Same(expectedData, @params.CompletionItem.Data);
        }
    }

    [Fact]
    public async Task ResolveAsync_InheritsOriginalCompletionListData()
    {
        // Arrange
        var expectedData = new object();
        var clientConnection = CreateClientConnectionForResolve(response: null, ValidateResolveParams);

        var optionsMonitor = TestRazorLSPOptionsMonitor.Create();
        var formattingService = await _lazyFormattingService.GetValueAsync(DisposalToken);
        var resolver = new DelegatedCompletionItemResolver(DocumentContextFactory, formattingService, DocumentMappingService, optionsMonitor, clientConnection, LoggerFactory);
        var item = new VSInternalCompletionItem();
        var containingCompletionList = new RazorVSInternalCompletionList() { Items = [item], Data = new object() };
        var originalRequestContext = new DelegatedCompletionResolutionContext(_csharpCompletionParams.Identifier, _csharpCompletionParams.ProjectedKind, expectedData);

        // Act
        await resolver.ResolveAsync(item, containingCompletionList, originalRequestContext, s_clientCapabilities, _componentAvailabilityService, DisposalToken);

        // Assert
        void ValidateResolveParams(DelegatedCompletionItemResolveParams @params)
        {
            Assert.Same(expectedData, @params.CompletionItem.Data);
        }
    }

    [Fact]
    public async Task ResolveAsync_CSharp_Resolves()
    {
        // Arrange & Act
        var resolvedItem = await ResolveCompletionItemAsync("@$$", itemToResolve: "typeof", DisposalToken);

        // Assert
        Assert.NotNull(resolvedItem.Description);
    }

    [Fact]
    public async Task ResolveAsync_CSharp_RemapAndFormatsTextEdit()
    {
        // Arrange
        var input =
            """
                @{
                    Task FooAsync()
                    {
                    awai$$
                    }
                }
                """;
        TestFileMarkupParser.GetPosition(input, out var documentContent, out _);
        var originalSourceText = SourceText.From(documentContent);
        var expectedSourceText = SourceText.From(
            """
                @{
                    async Task FooAsync()
                    {
                        await
                    }
                }
                """);

        // Act
        var resolvedItem = await ResolveCompletionItemAsync(input, itemToResolve: "await", DisposalToken);

        // Assert
        var textChange = originalSourceText.GetTextChange(resolvedItem.TextEdit.Value.First);
        var actualSourceText = originalSourceText.WithChanges(textChange);
        Assert.True(expectedSourceText.ContentEquals(actualSourceText));
    }

    [Fact]
    public async Task ResolveAsync_Html_Resolves()
    {
        // Arrange
        var expectedResolvedItem = new VSInternalCompletionItem();
        var clientConnection = CreateClientConnectionForResolve(expectedResolvedItem, ValidateResolveParams);

        var optionsMonitor = TestRazorLSPOptionsMonitor.Create();
        var formattingService = await _lazyFormattingService.GetValueAsync(DisposalToken);
        var resolver = new DelegatedCompletionItemResolver(DocumentContextFactory, formattingService, DocumentMappingService, optionsMonitor, clientConnection, LoggerFactory);
        var item = new VSInternalCompletionItem();
        var containingCompletionList = new RazorVSInternalCompletionList() { Items = [item] };
        var originalRequestContext = new DelegatedCompletionResolutionContext(_htmlCompletionParams.Identifier, _htmlCompletionParams.ProjectedKind, new object());

        // Act
        var resolvedItem = await resolver.ResolveAsync(
            item, containingCompletionList, originalRequestContext, s_clientCapabilities, _componentAvailabilityService, DisposalToken);

        // Assert
        Assert.Same(expectedResolvedItem, resolvedItem);

        void ValidateResolveParams(DelegatedCompletionItemResolveParams @params)
        {
            Assert.Same(_htmlCompletionParams.Identifier, @params.Identifier);
            Assert.Equal(RazorLanguageKind.Html, @params.OriginatingKind);
        }
    }

    private async Task<VSInternalCompletionItem> ResolveCompletionItemAsync(string content, string itemToResolve, CancellationToken cancellationToken)
    {
        TestFileMarkupParser.GetPosition(content, out var documentContent, out var cursorPosition);
        var codeDocument = CreateCodeDocument(documentContent, filePath: "C:/path/to/file.razor");
        await using var csharpServer = await CreateCSharpServerAsync(codeDocument);

        var clientConnection = CreateClientConnectionForResolve(csharpServer);
        var documentContextFactory = new TestDocumentContextFactory("C:/path/to/file.razor", codeDocument);
        var optionsMonitor = TestRazorLSPOptionsMonitor.Create();
        var formattingService = await _lazyFormattingService.GetValueAsync(DisposalToken);
        var resolver = new DelegatedCompletionItemResolver(documentContextFactory, formattingService, DocumentMappingService, optionsMonitor, clientConnection, LoggerFactory);
        var (containingCompletionList, csharpCompletionParams) = await GetCompletionListAndOriginalParamsAsync(
            cursorPosition, codeDocument, csharpServer);

        var originalRequestContext = new DelegatedCompletionResolutionContext(_csharpCompletionParams.Identifier, _csharpCompletionParams.ProjectedKind, containingCompletionList.Data);
        var item = containingCompletionList.Items.FirstOrDefault(item => item.Label == itemToResolve);

        if (item is null)
        {
            throw new XunitException($"Could not locate completion item '{item.Label}' for completion resolve test");
        }

        var resolvedItem = await resolver.ResolveAsync(
            item, containingCompletionList, originalRequestContext, s_clientCapabilities, _componentAvailabilityService, cancellationToken);

        return resolvedItem;
    }

    private async Task<CSharpTestLspServer> CreateCSharpServerAsync(RazorCodeDocument codeDocument)
    {
        var csharpSourceText = codeDocument.GetCSharpSourceText();
        var csharpDocumentUri = new Uri("C:/path/to/file.razor__virtual.g.cs");
        var serverCapabilities = new VSInternalServerCapabilities()
        {
            CompletionProvider = new CompletionOptions
            {
                ResolveProvider = true,
                TriggerCharacters = [" ", "(", "=", "#", ".", "<", "[", "{", "\"", "/", ":", "~"]
            }
        };

        // Don't declare this with an 'await using'. The caller owns the lifetime of this C# LSP server.
        var csharpServer = await CSharpTestLspServerHelpers.CreateCSharpLspServerAsync(
            csharpSourceText, csharpDocumentUri, serverCapabilities, DisposalToken);

        await csharpServer.OpenDocumentAsync(csharpDocumentUri, csharpSourceText.ToString(), DisposalToken);

        return csharpServer;
    }

    private async Task<(RazorVSInternalCompletionList, DelegatedCompletionParams)> GetCompletionListAndOriginalParamsAsync(
        int cursorPosition,
        RazorCodeDocument codeDocument,
        CSharpTestLspServer csharpServer)
    {
        var completionContext = new VSInternalCompletionContext() { TriggerKind = CompletionTriggerKind.Invoked };
        var documentContext = TestDocumentContext.Create("C:/path/to/file.razor", codeDocument);

        DelegatedCompletionParams? delegatedParams = null;
        var clientConnection = CreateClientConnectionForCompletion(csharpServer, processParams: @params =>
        {
            delegatedParams = @params;
        });

        var provider = CreateDelegatedCompletionListProvider(clientConnection);

        var completionList = await provider.GetCompletionListAsync(
            codeDocument,
            cursorPosition,
            completionContext,
            documentContext,
            s_clientCapabilities,
            s_defaultRazorCompletionOptions,
            correlationId: Guid.Empty,
            cancellationToken: DisposalToken);

        return (completionList, delegatedParams);
    }
}
