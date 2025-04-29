// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Hover;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.CodeAnalysis.Razor.Completion;
using Microsoft.CodeAnalysis.Razor.Tooltip;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Completion;

public class RazorCompletionResolveEndpointTest : LanguageServerTestBase
{
    private readonly RazorCompletionResolveEndpoint _endpoint;
    private readonly CompletionListCache _completionListCache;
    private readonly VSInternalClientCapabilities _clientCapabilities;

    public RazorCompletionResolveEndpointTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        _completionListCache = new CompletionListCache();

        var projectManager = CreateProjectSnapshotManager();
        var componentAvailabilityService = new ComponentAvailabilityService(projectManager);

        _endpoint = new RazorCompletionResolveEndpoint(
            new AggregateCompletionItemResolver(
                [new TestCompletionItemResolver()],
                LoggerFactory),
            _completionListCache,
            componentAvailabilityService);
        _clientCapabilities = new VSInternalClientCapabilities()
        {
            TextDocument = new TextDocumentClientCapabilities()
            {
                Completion = new VSInternalCompletionSetting()
                {
                    CompletionItem = new CompletionItemSetting()
                    {
                        DocumentationFormat = [MarkupKind.Markdown],
                    }
                }
            }
        };
        _endpoint.ApplyCapabilities(new(), _clientCapabilities);
    }

    [Fact]
    public async Task Handle_UncachedCompletionItem_NoChange()
    {
        // Arrange
        var completionItem = new VSInternalCompletionItem() { Label = "Test" };
        var parameters = ConvertToBridgedItem(completionItem);
        var requestContext = CreateRazorRequestContext(documentContext: null);

        // Act
        var resolvedItem = await _endpoint.HandleRequestAsync(parameters, requestContext, DisposalToken);

        // Assert
        Assert.Null(resolvedItem.Documentation);
    }

    [Fact]
    public async Task Handle_EvictedCachedCompletionItem_NoChange()
    {
        // Arrange
        var completionItem = new VSInternalCompletionItem() { Label = "Test" };
        var completionList = new VSInternalCompletionList() { Items = [completionItem] };
        completionList.SetResultId(1337, completionSetting: null);
        var parameters = ConvertToBridgedItem(completionItem);
        var requestContext = CreateRazorRequestContext(documentContext: null);

        // Act
        var resolvedItem = await _endpoint.HandleRequestAsync(parameters, requestContext, DisposalToken);

        // Assert
        Assert.Null(resolvedItem.Documentation);
    }

    [Fact]
    public async Task Handle_CachedCompletionItem_Resolves()
    {
        // Arrange
        var completionItem = new VSInternalCompletionItem() { Label = "Test" };
        var completionList = new VSInternalCompletionList() { Items = [completionItem] };
        var resultId = _completionListCache.Add(completionList, StrictMock.Of<ICompletionResolveContext>());
        completionList.SetResultId(resultId, completionSetting: null);
        var parameters = ConvertToBridgedItem(completionItem);
        var requestContext = CreateRazorRequestContext(documentContext: null);

        // Act
        var resolvedItem = await _endpoint.HandleRequestAsync(parameters, requestContext, DisposalToken);

        // Assert
        Assert.NotNull(resolvedItem);
        Assert.NotNull(resolvedItem.Documentation);
        Assert.Equal("I was resolved using markdown", resolvedItem.Documentation.Value.First);
    }

    [Fact]
    public async Task Handle_MultipleResultIdsIgnoresEvictedResultIds_Resolves()
    {
        // Arrange
        await InitializeAsync();
        var completionItem = new VSInternalCompletionItem() { Label = "Test" };
        var completionList = new VSInternalCompletionList() { Items = [completionItem] };
        completionList.SetResultId(/* Invalid */ 1337, completionSetting: null);
        var resultId = _completionListCache.Add(completionList, StrictMock.Of<ICompletionResolveContext>());
        completionList.SetResultId(resultId, completionSetting: null);
        var parameters = ConvertToBridgedItem(completionItem);
        var requestContext = CreateRazorRequestContext(documentContext: null);

        // Act
        var resolvedItem = await _endpoint.HandleRequestAsync(parameters, requestContext, DisposalToken);

        // Assert
        Assert.NotNull(resolvedItem);
        Assert.NotNull(resolvedItem.Documentation);
        Assert.Equal("I was resolved using markdown", resolvedItem.Documentation.Value.First);
    }

    [Fact]
    public async Task Handle_MergedCompletionListFindsProperCompletionList_Resolves()
    {
        // Arrange
        await InitializeAsync();
        var completionSetting = new VSInternalCompletionSetting() { CompletionList = new VSInternalCompletionListSetting() { Data = true } };
        var completionList1 = new VSInternalCompletionList() { Items = [] };
        var completion1Context = StrictMock.Of<ICompletionResolveContext>();
        var resultId1 = _completionListCache.Add(completionList1, completion1Context);
        completionList1.SetResultId(resultId1, completionSetting);

        var completionItem = new VSInternalCompletionItem() { Label = "Test" };
        var completionList2 = new VSInternalCompletionList() { Items = [completionItem] };
        var completion2Context = StrictMock.Of<ICompletionResolveContext>();
        var resultId2 = _completionListCache.Add(completionList2, completion2Context);
        completionList2.SetResultId(resultId2, completionSetting);
        var mergedCompletionList = CompletionListMerger.Merge(completionList1, completionList2);
        var mergedCompletionItem = mergedCompletionList.Items.Single();
        mergedCompletionItem.Data = mergedCompletionList.Data;
        var parameters = ConvertToBridgedItem(mergedCompletionItem);
        var requestContext = CreateRazorRequestContext(documentContext: null);

        // Act
        var resolvedItem = await _endpoint.HandleRequestAsync(parameters, requestContext, DisposalToken);

        // Assert
        Assert.NotNull(resolvedItem);
        Assert.NotNull(resolvedItem.Documentation);
        Assert.Equal("I was resolved using markdown", resolvedItem.Documentation.Value.First);
        Assert.Same(completion2Context, resolvedItem.Data);
    }

    private VSInternalCompletionItem ConvertToBridgedItem(CompletionItem completionItem)
    {
        var serialized = JsonSerializer.Serialize(completionItem, SerializerOptions);
        var bridgedItem = JsonSerializer.Deserialize<VSInternalCompletionItem>(serialized, SerializerOptions);
        return bridgedItem.AssumeNotNull();
    }

    private class TestCompletionItemResolver : CompletionItemResolver
    {
        public override Task<VSInternalCompletionItem?> ResolveAsync(
            VSInternalCompletionItem item,
            VSInternalCompletionList containingCompletionList,
            ICompletionResolveContext originalRequestContext,
            VSInternalClientCapabilities? clientCapabilities,
            IComponentAvailabilityService componentAvailabilityService,
            CancellationToken cancellationToken)
        {
            var completionSupportedKinds = clientCapabilities?.TextDocument?.Completion?.CompletionItem?.DocumentationFormat;
            var documentationKind = completionSupportedKinds?.Contains(MarkupKind.Markdown) == true ? MarkupKind.Markdown : MarkupKind.PlainText;
            item.Documentation = "I was resolved using " + documentationKind.Value;
            item.Data = originalRequestContext;
            return Task.FromResult<VSInternalCompletionItem?>(item);
        }
    }
}
