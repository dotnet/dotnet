// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Text;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Debugging;

public sealed class DataTipRangeHandlerEndpointTest(ITestOutputHelper testOutput) : SingleServerDelegatingEndpointTestBase(testOutput)
{
    [Fact]
    public async Task Handle_CSharpInHtml_DataTipRange_FirstExpression()
    {
        var input = """
                @{
                    {|expression:{|hover:a$$aa|}|}.bbb.ccc;
                }
                """;

        await VerifyDataTipRangeAsync(input);
    }

    [Fact]
    public async Task Handle_CSharpInHtml_DataTipRange_SecondExpression()
    {
        var input = """
                @{
                    {|expression:{|hover:aaa.b$$bb|}|}.ccc;
                }
                """;

        await VerifyDataTipRangeAsync(input);
    }

    [Fact]
    public async Task Handle_CSharpInHtml_DataTipRange_LastExpression()
    {
        var input = """
                @{
                    {|expression:{|hover:aaa.bbb.c$$cc|}|};
                }
                """;

        await VerifyDataTipRangeAsync(input);
    }

    [Fact]
    public async Task Handle_CSharpInHtml_DataTipRange_LinqExpression()
    {
        var input = """
                @using System.Linq;

                @{
                    int[] args;
                    var v = {|expression:{|hover:args.Se$$lect|}(a => a.ToString())|}.Where(a => a.Length >= 0);
                }
                """;

        await VerifyDataTipRangeAsync(input, VSInternalDataTipTags.LinqExpression);
    }

    private async Task VerifyDataTipRangeAsync(string input, VSInternalDataTipTags dataTipTags = 0)
    {
        // Arrange
        TestFileMarkupParser.GetPositionAndSpans(input, out var output, out int position, out ImmutableDictionary<string, ImmutableArray<TextSpan>> spans);

        Assert.True(spans.TryGetValue("expression", out var expressionSpans), "Test authoring failure: Expected at least one span named 'expression'.");
        Assert.True(expressionSpans.Length == 1, "Test authoring failure: Expected only one 'expression' span.");
        Assert.True(spans.TryGetValue("hover", out var hoverSpans), "Test authoring failure: Expected at least one span named 'hover'.");
        Assert.True(hoverSpans.Length == 1, "Test authoring failure: Expected only one 'hover' span.");

        var codeDocument = CreateCodeDocument(output);
        var razorFilePath = "C:/path/to/file.razor";

        // Act
        var result = await GetDataTipRangeAsync(codeDocument, razorFilePath, position);

        // Assert
        var expectedExpressionRange = codeDocument.Source.Text.GetRange(expressionSpans[0]);
        Assert.Equal(expectedExpressionRange, result!.ExpressionRange);

        var expectedHoverRange = codeDocument.Source.Text.GetRange(hoverSpans[0]);
        Assert.Equal(expectedHoverRange, result!.HoverRange);

        Assert.Equal(dataTipTags, result!.DataTipTags);
    }

    private async Task<VSInternalDataTip?> GetDataTipRangeAsync(RazorCodeDocument codeDocument, string razorFilePath, int position)
    {
        await using var languageServer = await CreateLanguageServerAsync(codeDocument, razorFilePath);

        var endpoint = new DataTipRangeHandlerEndpoint(DocumentMappingService, LanguageServerFeatureOptions, languageServer, LoggerFactory);

        var request = new TextDocumentPositionParams
        {
            TextDocument = new TextDocumentIdentifier
            {
                Uri = new Uri(razorFilePath)
            },
            Position = codeDocument.Source.Text.GetPosition(position)
        };

        Assert.True(DocumentContextFactory.TryCreate(request.TextDocument, out var documentContext));
        var requestContext = CreateRazorRequestContext(documentContext, LspServices.Empty);

        return await endpoint.HandleRequestAsync(request, requestContext, DisposalToken);
    }
}
