// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Formatting;
using Microsoft.AspNetCore.Razor.LanguageServer.Hover;
using Microsoft.AspNetCore.Razor.LanguageServer.Test;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.CodeAnalysis.Razor.Completion;
using Microsoft.CodeAnalysis.Razor.Formatting;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Tooltip;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.Threading;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Completion.Delegation;

public class DelegatedCompletionItemResolverTest : LanguageServerTestBase
{
    private readonly VSInternalClientCapabilities _clientCapabilities;
    private readonly DelegatedCompletionParams _csharpCompletionParams;
    private readonly DelegatedCompletionParams _htmlCompletionParams;
    private readonly IDocumentContextFactory _documentContextFactory;
    private readonly AsyncLazy<IRazorFormattingService> _formattingService;
    private readonly RazorCompletionOptions _defaultRazorCompletionOptions;
    private readonly IComponentAvailabilityService _componentAvailabilityService;

    public DelegatedCompletionItemResolverTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        _clientCapabilities = new VSInternalClientCapabilities()
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

        var documentContext = TestDocumentContext.Create("C:/path/to/file.cshtml");
        _csharpCompletionParams = new DelegatedCompletionParams(
            documentContext.GetTextDocumentIdentifierAndVersion(),
            VsLspFactory.CreatePosition(10, 6),
            RazorLanguageKind.CSharp,
            new VSInternalCompletionContext(),
            ProvisionalTextEdit: null,
            ShouldIncludeSnippets: false,
            CorrelationId: Guid.Empty);

        _htmlCompletionParams = new DelegatedCompletionParams(
            documentContext.GetTextDocumentIdentifierAndVersion(),
            VsLspFactory.DefaultPosition,
            RazorLanguageKind.Html,
            new VSInternalCompletionContext(),
            ProvisionalTextEdit: null,
            ShouldIncludeSnippets: false,
            CorrelationId: Guid.Empty);

        _documentContextFactory = new TestDocumentContextFactory();
        _formattingService = new AsyncLazy<IRazorFormattingService>(() => TestRazorFormattingService.CreateWithFullSupportAsync(LoggerFactory));
        _defaultRazorCompletionOptions = new RazorCompletionOptions(
            SnippetsSupported: true,
            AutoInsertAttributeQuotes: true,
            CommitElementsWithSpace: true);

        var projectManager = CreateProjectSnapshotManager();
        _componentAvailabilityService = new ComponentAvailabilityService(projectManager);
    }

    [Fact]
    public async Task ResolveAsync_CanNotFindCompletionItem_Noops()
    {
        // Arrange
        var server = TestDelegatedCompletionItemResolverServer.Create();
        var optionsMonitor = TestRazorLSPOptionsMonitor.Create();
        var resolver = new DelegatedCompletionItemResolver(_documentContextFactory, _formattingService.GetValue(), optionsMonitor, server);
        var item = new VSInternalCompletionItem();
        var notContainingCompletionList = new VSInternalCompletionList();
        var originalRequestContext = StrictMock.Of<ICompletionResolveContext>();

        // Act
        var resolvedItem = await resolver.ResolveAsync(
            item, notContainingCompletionList, originalRequestContext, _clientCapabilities, _componentAvailabilityService, DisposalToken);

        // Assert
        Assert.Null(resolvedItem);
    }

    [Fact]
    public async Task ResolveAsync_UnknownRequestContext_Noops()
    {
        // Arrange
        var server = TestDelegatedCompletionItemResolverServer.Create();
        var optionsMonitor = TestRazorLSPOptionsMonitor.Create();
        var resolver = new DelegatedCompletionItemResolver(_documentContextFactory, _formattingService.GetValue(), optionsMonitor, server);
        var item = new VSInternalCompletionItem();
        var containingCompletionList = new VSInternalCompletionList() { Items = [item] };
        var originalRequestContext = StrictMock.Of<ICompletionResolveContext>();

        // Act
        var resolvedItem = await resolver.ResolveAsync(
            item, containingCompletionList, originalRequestContext, _clientCapabilities, _componentAvailabilityService, DisposalToken);

        // Assert
        Assert.Null(resolvedItem);
    }

    [Fact]
    public async Task ResolveAsync_UsesItemsData()
    {
        // Arrange
        var server = TestDelegatedCompletionItemResolverServer.Create();
        var optionsMonitor = TestRazorLSPOptionsMonitor.Create();
        var resolver = new DelegatedCompletionItemResolver(_documentContextFactory, _formattingService.GetValue(), optionsMonitor, server);
        var expectedData = new object();
        var item = new VSInternalCompletionItem()
        {
            Data = expectedData,
        };
        var containingCompletionList = new VSInternalCompletionList() { Items = [item], Data = new object() };
        var originalRequestContext = new DelegatedCompletionResolutionContext(_csharpCompletionParams, new object());

        // Act
        await resolver.ResolveAsync(
            item, containingCompletionList, originalRequestContext, _clientCapabilities, _componentAvailabilityService, DisposalToken);

        // Assert
        Assert.Same(expectedData, server.DelegatedParams.CompletionItem.Data);
    }

    [Fact]
    public async Task ResolveAsync_InheritsOriginalCompletionListData()
    {
        // Arrange
        var server = TestDelegatedCompletionItemResolverServer.Create();
        var optionsMonitor = TestRazorLSPOptionsMonitor.Create();
        var resolver = new DelegatedCompletionItemResolver(_documentContextFactory, _formattingService.GetValue(), optionsMonitor, server);
        var item = new VSInternalCompletionItem();
        var containingCompletionList = new VSInternalCompletionList() { Items = [item], Data = new object() };
        var expectedData = new object();
        var originalRequestContext = new DelegatedCompletionResolutionContext(_csharpCompletionParams, expectedData);

        // Act
        await resolver.ResolveAsync(item, containingCompletionList, originalRequestContext, _clientCapabilities, _componentAvailabilityService, DisposalToken);

        // Assert
        Assert.Same(expectedData, server.DelegatedParams.CompletionItem.Data);
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
        var server = TestDelegatedCompletionItemResolverServer.Create(expectedResolvedItem);
        var optionsMonitor = TestRazorLSPOptionsMonitor.Create();
        var resolver = new DelegatedCompletionItemResolver(_documentContextFactory, _formattingService.GetValue(), optionsMonitor, server);
        var item = new VSInternalCompletionItem();
        var containingCompletionList = new VSInternalCompletionList() { Items = [item] };
        var originalRequestContext = new DelegatedCompletionResolutionContext(_htmlCompletionParams, new object());

        // Act
        var resolvedItem = await resolver.ResolveAsync(
            item, containingCompletionList, originalRequestContext, _clientCapabilities, _componentAvailabilityService, DisposalToken);

        // Assert
        Assert.Same(_htmlCompletionParams.Identifier, server.DelegatedParams.Identifier);
        Assert.Equal(RazorLanguageKind.Html, server.DelegatedParams.OriginatingKind);
        Assert.Same(expectedResolvedItem, resolvedItem);
    }

    private async Task<VSInternalCompletionItem> ResolveCompletionItemAsync(string content, string itemToResolve, CancellationToken cancellationToken)
    {
        TestFileMarkupParser.GetPosition(content, out var documentContent, out var cursorPosition);
        var codeDocument = CreateCodeDocument(documentContent, filePath: "C:/path/to/file.razor");
        await using var csharpServer = await CreateCSharpServerAsync(codeDocument);

        var server = TestDelegatedCompletionItemResolverServer.Create(csharpServer, DisposalToken);
        var documentContextFactory = new TestDocumentContextFactory("C:/path/to/file.razor", codeDocument);
        var optionsMonitor = TestRazorLSPOptionsMonitor.Create();
        var resolver = new DelegatedCompletionItemResolver(documentContextFactory, _formattingService.GetValue(), optionsMonitor, server);
        var (containingCompletionList, csharpCompletionParams) = await GetCompletionListAndOriginalParamsAsync(
            cursorPosition, codeDocument, csharpServer);

        var originalRequestContext = new DelegatedCompletionResolutionContext(csharpCompletionParams, containingCompletionList.Data);
        var item = (VSInternalCompletionItem)containingCompletionList.Items.FirstOrDefault(item => item.Label == itemToResolve);

        if (item is null)
        {
            throw new XunitException($"Could not locate completion item '{item.Label}' for completion resolve test");
        }

        var resolvedItem = await resolver.ResolveAsync(
            item, containingCompletionList, originalRequestContext, _clientCapabilities, _componentAvailabilityService, cancellationToken);

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

        var csharpServer = await CSharpTestLspServerHelpers.CreateCSharpLspServerAsync(
            csharpSourceText, csharpDocumentUri, serverCapabilities, DisposalToken);

        await csharpServer.OpenDocumentAsync(csharpDocumentUri, csharpSourceText.ToString());

        return csharpServer;
    }

    private async Task<(VSInternalCompletionList, DelegatedCompletionParams)> GetCompletionListAndOriginalParamsAsync(
        int cursorPosition,
        RazorCodeDocument codeDocument,
        CSharpTestLspServer csharpServer)
    {
        var completionContext = new VSInternalCompletionContext() { TriggerKind = CompletionTriggerKind.Invoked };
        var documentContext = TestDocumentContext.Create("C:/path/to/file.razor", codeDocument);
        var provider = TestDelegatedCompletionListProvider.Create(csharpServer, LoggerFactory, DisposalToken);

        var completionList = await provider.GetCompletionListAsync(
            codeDocument,
            cursorPosition,
            completionContext,
            documentContext,
            _clientCapabilities,
            _defaultRazorCompletionOptions,
            correlationId: Guid.Empty,
            cancellationToken: DisposalToken);

        return (completionList, provider.DelegatedParams);
    }

    internal class TestDelegatedCompletionItemResolverServer : TestLanguageServer
    {
        private readonly CompletionResolveRequestResponseFactory _requestHandler;

        private TestDelegatedCompletionItemResolverServer(CompletionResolveRequestResponseFactory requestHandler)
            : base(new Dictionary<string, Func<object, Task<object>>>()
            {
                [LanguageServerConstants.RazorCompletionResolveEndpointName] = requestHandler.OnCompletionResolveDelegationAsync,
                [LanguageServerConstants.RazorGetFormattingOptionsEndpointName] = requestHandler.OnGetFormattingOptionsAsync,
            })
        {
            _requestHandler = requestHandler;
        }

        public DelegatedCompletionItemResolveParams DelegatedParams => _requestHandler.DelegatedParams;

        public static TestDelegatedCompletionItemResolverServer Create(
            CSharpTestLspServer csharpServer,
            CancellationToken cancellationToken)
        {
            var requestHandler = new DelegatedCSharpCompletionRequestHandler(csharpServer, cancellationToken);
            var provider = new TestDelegatedCompletionItemResolverServer(requestHandler);
            return provider;
        }

        public static TestDelegatedCompletionItemResolverServer Create(VSInternalCompletionItem resolveResponse = null)
        {
            resolveResponse ??= new VSInternalCompletionItem();
            var requestResponseFactory = new StaticCompletionResolveRequestHandler(resolveResponse);
            var provider = new TestDelegatedCompletionItemResolverServer(requestResponseFactory);
            return provider;
        }

        private class StaticCompletionResolveRequestHandler(VSInternalCompletionItem resolveResponse) : CompletionResolveRequestResponseFactory
        {
            private readonly VSInternalCompletionItem _resolveResponse = resolveResponse;
            private DelegatedCompletionItemResolveParams _delegatedParams;

            public override DelegatedCompletionItemResolveParams DelegatedParams => _delegatedParams;

            public override Task<object> OnCompletionResolveDelegationAsync(object parameters)
            {
                var resolveParams = (DelegatedCompletionItemResolveParams)parameters;
                _delegatedParams = resolveParams;

                return Task.FromResult<object>(_resolveResponse);
            }
        }

        private class DelegatedCSharpCompletionRequestHandler(
            CSharpTestLspServer csharpServer,
            CancellationToken cancellationToken) : CompletionResolveRequestResponseFactory
        {
            private readonly CSharpTestLspServer _csharpServer = csharpServer;
            private readonly CancellationToken _cancellationToken = cancellationToken;
            private DelegatedCompletionItemResolveParams _delegatedParams;

            public override DelegatedCompletionItemResolveParams DelegatedParams => _delegatedParams;

            public override async Task<object> OnCompletionResolveDelegationAsync(object parameters)
            {
                var resolveParams = (DelegatedCompletionItemResolveParams)parameters;
                _delegatedParams = resolveParams;

                var resolvedCompletionItem = await _csharpServer.ExecuteRequestAsync<VSInternalCompletionItem, VSInternalCompletionItem>(
                    Methods.TextDocumentCompletionResolveName,
                    _delegatedParams.CompletionItem,
                    _cancellationToken);

                return resolvedCompletionItem;
            }
        }

        private abstract class CompletionResolveRequestResponseFactory
        {
            public abstract DelegatedCompletionItemResolveParams DelegatedParams { get; }

            public abstract Task<object> OnCompletionResolveDelegationAsync(object parameters);

#pragma warning disable IDE0060 // Remove unused parameter
            public Task<object> OnGetFormattingOptionsAsync(object parameters)
#pragma warning restore IDE0060 // Remove unused parameter
            {
                var formattingOptions = new FormattingOptions()
                {
                    InsertSpaces = true,
                    TabSize = 4,
                };

                return Task.FromResult<object>(formattingOptions);
            }
        }
    }
}
