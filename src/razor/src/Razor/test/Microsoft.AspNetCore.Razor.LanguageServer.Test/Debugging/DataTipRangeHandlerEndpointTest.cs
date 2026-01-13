// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common;
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

    private async Task VerifyDataTipRangeAsync(TestCode input, VSInternalDataTipTags dataTipTags = 0)
    {
        // Arrange
        var codeDocument = CreateCodeDocument(input.Text);
        var razorFilePath = "C:/path/to/file.razor";

        // Act
        var result = await GetDataTipRangeAsync(codeDocument, razorFilePath, input.Position);

        // Assert
        var expectedExpressionSpan = input.GetNamedSpans("expression")[0];
        var expectedExpressionRange = codeDocument.Source.Text.GetRange(expectedExpressionSpan);
        Assert.Equal(expectedExpressionRange, result!.ExpressionRange);

        var expectedHoverSpan = input.GetNamedSpans("hover")[0];
        var expectedHoverRange = codeDocument.Source.Text.GetRange(expectedHoverSpan);
        Assert.Equal(expectedHoverRange, result.HoverRange);

        Assert.Equal(dataTipTags, result.DataTipTags);
    }

    private async Task<VSInternalDataTip?> GetDataTipRangeAsync(RazorCodeDocument codeDocument, string razorFilePath, int position)
    {
        await using var languageServer = await CreateLanguageServerAsync(codeDocument, razorFilePath);

        var endpoint = new DataTipRangeHandlerEndpoint(DocumentMappingService, languageServer, LoggerFactory);

        var request = new TextDocumentPositionParams
        {
            TextDocument = new TextDocumentIdentifier
            {
                DocumentUri = new(new Uri(razorFilePath))
            },
            Position = codeDocument.Source.Text.GetPosition(position)
        };

        Assert.True(DocumentContextFactory.TryCreate(request.TextDocument, out var documentContext));
        var requestContext = CreateRazorRequestContext(documentContext, LspServices.Empty);

        return await endpoint.HandleRequestAsync(request, requestContext, DisposalToken);
    }
}
