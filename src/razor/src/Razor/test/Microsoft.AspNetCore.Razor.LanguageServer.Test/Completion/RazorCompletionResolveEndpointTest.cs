// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Completion;

public class RazorCompletionResolveEndpointTest : LanguageServerTestBase
{
    private readonly RazorCompletionResolveEndpoint _endpoint;
    private readonly CompletionListCache _completionListCache;

    public RazorCompletionResolveEndpointTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        _completionListCache = new CompletionListCache();
        _endpoint = new RazorCompletionResolveEndpoint(
            new AggregateCompletionItemResolver(
                new[] { new TestCompletionItemResolver() }, LoggerFactory),
            _completionListCache);
    }

    protected override Task InitializeAsync()
    {
        return _endpoint.OnInitializedAsync(new VSInternalClientCapabilities(), DisposalToken);
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
        var completionList = new VSInternalCompletionList() { Items = new[] { completionItem } };
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
        var completionList = new VSInternalCompletionList() { Items = new[] { completionItem } };
        var resultId = _completionListCache.Set(completionList, context: null);
        completionList.SetResultId(resultId, completionSetting: null);
        var parameters = ConvertToBridgedItem(completionItem);
        var requestContext = CreateRazorRequestContext(documentContext: null);

        // Act
        var resolvedItem = await _endpoint.HandleRequestAsync(parameters, requestContext, DisposalToken);

        // Assert
        Assert.NotNull(resolvedItem.Documentation);
    }

    [Fact]
    public async Task Handle_MultipleResultIdsIgnoresEvictedResultIds_Resolves()
    {
        // Arrange
        await InitializeAsync();
        var completionItem = new VSInternalCompletionItem() { Label = "Test" };
        var completionList = new VSInternalCompletionList() { Items = new[] { completionItem } };
        completionList.SetResultId(/* Invalid */ 1337, completionSetting: null);
        var resultId = _completionListCache.Set(completionList, context: null);
        completionList.SetResultId(resultId, completionSetting: null);
        var parameters = ConvertToBridgedItem(completionItem);
        var requestContext = CreateRazorRequestContext(documentContext: null);

        // Act
        var resolvedItem = await _endpoint.HandleRequestAsync(parameters, requestContext, DisposalToken);

        // Assert
        Assert.NotNull(resolvedItem.Documentation);
    }

    [Fact]
    public async Task Handle_MergedCompletionListFindsProperCompletionList_Resolves()
    {
        // Arrange
        await InitializeAsync();
        var completionSetting = new VSInternalCompletionSetting() { CompletionList = new VSInternalCompletionListSetting() { Data = true } };
        var completionList1 = new VSInternalCompletionList() { Items = Array.Empty<CompletionItem>() };
        var completion1Context = new object();
        var resultId1 = _completionListCache.Set(completionList1, completion1Context);
        completionList1.SetResultId(resultId1, completionSetting);

        var completionItem = new VSInternalCompletionItem() { Label = "Test" };
        var completionList2 = new VSInternalCompletionList() { Items = new[] { completionItem } };
        var completion2Context = new object();
        var resultId2 = _completionListCache.Set(completionList2, completion2Context);
        completionList2.SetResultId(resultId2, completionSetting);
        var mergedCompletionList = CompletionListMerger.Merge(completionList1, completionList2);
        var mergedCompletionItem = mergedCompletionList.Items.Single();
        mergedCompletionItem.Data = mergedCompletionList.Data;
        var parameters = ConvertToBridgedItem(mergedCompletionItem);
        var requestContext = CreateRazorRequestContext(documentContext: null);

        // Act
        var resolvedItem = await _endpoint.HandleRequestAsync(parameters, requestContext, DisposalToken);

        // Assert
        Assert.NotNull(resolvedItem.Documentation);
        Assert.Same(completion2Context, resolvedItem.Data);
    }

    private VSInternalCompletionItem ConvertToBridgedItem(CompletionItem completionItem)
    {
        using var textWriter = new StringWriter();
        Serializer.Serialize(textWriter, completionItem);
        var stringBuilder = textWriter.GetStringBuilder();
        using var jsonReader = new JsonTextReader(new StringReader(stringBuilder.ToString()));
        var bridgedItem = Serializer.Deserialize<VSInternalCompletionItem>(jsonReader);
        return bridgedItem;
    }

    private class TestCompletionItemResolver : CompletionItemResolver
    {
        public override Task<VSInternalCompletionItem> ResolveAsync(
            VSInternalCompletionItem item,
            VSInternalCompletionList containingCompletionlist,
            object originalRequestContext,
            VSInternalClientCapabilities clientCapabilities,
            CancellationToken cancellationToken)
        {
            item.Documentation = "I was resolved";
            item.Data = originalRequestContext;
            return Task.FromResult(item);
        }
    }
}
