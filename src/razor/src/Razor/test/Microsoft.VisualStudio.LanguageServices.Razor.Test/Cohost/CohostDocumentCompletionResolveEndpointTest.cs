// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Razor.Completion;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.VisualStudio.Razor.Settings;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

public class CohostDocumentCompletionResolveEndpointTest(ITestOutputHelper testOutputHelper) : CohostEndpointTestBase(testOutputHelper)
{
    [Fact]
    public async Task HtmlResolve()
    {
        await VerifyCompletionItemResolveAsync(
            input: """
                This is a Razor document.

                <div st$$></div>

                The end.
                """,
            supportsVisualStudioExtensions: true);
    }

    [Fact]
    public async Task HtmlResolve_VSCode()
    {
        await VerifyCompletionItemResolveAsync(
            input: """
                This is a Razor document.

                <div st$$></div>

                The end.
                """,
            supportsVisualStudioExtensions: false);
    }

    private async Task VerifyCompletionItemResolveAsync(TestCode input, bool supportsVisualStudioExtensions)
    {
        var document = CreateProjectAndRazorDocument(input.Text);

        var response = new VSInternalCompletionItem()
        {
            Label = "ResolvedItem"
        };
        var requestInvoker = new TestHtmlRequestInvoker([(Methods.TextDocumentCompletionResolveName, response)]);

        var completionListCache = new CompletionListCache();
        var clientSettingsManager = new ClientSettingsManager(changeTriggers: []);
        var endpoint = new CohostDocumentCompletionResolveEndpoint(
            IncompatibleProjectService,
            completionListCache,
            RemoteServiceInvoker,
            clientSettingsManager,
            requestInvoker,
            LoggerFactory);

        var textDocumentIdentifier = new TextDocumentIdentifierAndVersion(new TextDocumentIdentifier { DocumentUri = document.CreateDocumentUri() }, Version: 0);

        var context = new DelegatedCompletionResolutionContext(
            textDocumentIdentifier,
            OriginalCompletionListData: null,
            ProjectedKind: RazorLanguageKind.Html);

        var list = new RazorVSInternalCompletionList
        {
            Data = supportsVisualStudioExtensions ? JsonSerializer.SerializeToElement(context) : null,
            ItemDefaults = new()
            {
                Data = supportsVisualStudioExtensions ? null : JsonSerializer.SerializeToElement(context),
            },
            Items = [new VSInternalCompletionItem()
            {
                Label = "TestItem"
            }]
        };

        var clientCapabilities = new VSInternalClientCapabilities
        {
            SupportsVisualStudioExtensions = supportsVisualStudioExtensions,
            TextDocument = new TextDocumentClientCapabilities()
            {
                Completion = new VSInternalCompletionSetting()
                {
                    CompletionList = new()
                    {
                        Data = supportsVisualStudioExtensions
                    },
                    CompletionListSetting = new()
                    {
                        ItemDefaults = supportsVisualStudioExtensions ? null : ["data"]
                    }
                }
            }
        };

        var resultId = completionListCache.Add(list, context);
        list.SetResultId(resultId, clientCapabilities);
        RazorCompletionResolveData.Wrap(list, textDocumentIdentifier.TextDocumentIdentifier, clientCapabilities);

        var request = list.Items[0];
        // Simulate the LSP client, which would receive all of the items and the list data, and send the item back to us with
        // data filled in.
        request.Data = JsonSerializer.SerializeToElement(list.Data ?? list.ItemDefaults.Data, JsonHelpers.JsonSerializerOptions);

        var tdi = endpoint.GetTestAccessor().GetRazorTextDocumentIdentifier(request);
        Assert.NotNull(tdi);
        Assert.Equal(document.CreateUri(), tdi.Value.Uri);

        var result = await endpoint.GetTestAccessor().HandleRequestAsync(request, document, DisposalToken);

        Assert.NotNull(result);
        Assert.NotSame(result, request);
        Assert.Equal(response.Label, result.Label);
    }
}
