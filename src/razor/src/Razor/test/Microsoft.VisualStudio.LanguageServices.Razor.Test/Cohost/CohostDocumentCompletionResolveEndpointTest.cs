// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

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
                """);
    }

    private async Task VerifyCompletionItemResolveAsync(TestCode input)
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
            completionListCache,
            RemoteServiceInvoker,
            clientSettingsManager,
            requestInvoker,
            LoggerFactory);

        var textDocumentIdentifier = new TextDocumentIdentifierAndVersion(new TextDocumentIdentifier { DocumentUri = new(document.CreateUri()) }, Version: 0);

        var context = new DelegatedCompletionResolutionContext(
            textDocumentIdentifier,
            OriginalCompletionListData: null,
            ProjectedKind: RazorLanguageKind.Html);

        var request = new VSInternalCompletionItem()
        {
            Data = JsonSerializer.SerializeToElement(context),
            Label = "TestItem"
        };
        var list = new RazorVSInternalCompletionList
        {
            Items = [request]
        };

        var resultId = completionListCache.Add(list, context);
        list.SetResultId(resultId, null);
        RazorCompletionResolveData.Wrap(list, textDocumentIdentifier.TextDocumentIdentifier, supportsCompletionListData: false);

        // We expect data to be a JsonElement, so for tests we have to _not_ strongly type
        request.Data = JsonSerializer.SerializeToElement(request.Data, JsonHelpers.JsonSerializerOptions);

        var result = await endpoint.GetTestAccessor().HandleRequestAsync(request, document, DisposalToken);

        Assert.NotNull(result);
        Assert.NotSame(result, request);
        Assert.Equal(response.Label, result.Label);
    }
}
