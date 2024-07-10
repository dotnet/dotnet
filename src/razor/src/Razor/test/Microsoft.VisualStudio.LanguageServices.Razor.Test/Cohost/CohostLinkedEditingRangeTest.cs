// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Razor.LinkedEditingRange;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.Razor.LanguageClient.Cohost;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.LanguageServices.Razor.Test.Cohost;

public class CohostLinkedEditingRangeTest(ITestOutputHelper testOutputHelper) : CohostTestBase(testOutputHelper)
{
    [Theory]
    [InlineData("$$div")]
    [InlineData("di$$v")]
    [InlineData("div$$")]
    public async Task Html_StartTag(string startTagAndCursorLocation)
    {
        var input = $"""
            This is a Razor document.

            <[|{startTagAndCursorLocation}|]>
                Here is some content.
            </[|div|]>

            The end.
            """;

        await VerifyLinkedEditingRangeAsync(input);
    }

    [Theory]
    [InlineData("$$div")]
    [InlineData("di$$v")]
    [InlineData("div$$")]
    public async Task Html_EndTag(string endTagAndCursorLocation)
    {
        var input = $"""
            This is a Razor document.

            <[|div|]>
                Here is some content.
            </[|{endTagAndCursorLocation}|]>

            The end.
            """;

        await VerifyLinkedEditingRangeAsync(input);
    }

    [Fact]
    public async Task Html_EndTag_BeforeSlash()
    {
        var input = $"""
            This is a Razor document.

            <div>
                Here is some content.
            <$$/div>

            The end.
            """;

        await VerifyLinkedEditingRangeAsync(input);
    }

    [Fact]
    public async Task Html_NotATag()
    {
        var input = $"""
            This is a $$Razor document.

            <div>
                Here is some content.
            </div>

            The end.
            """;

        await VerifyLinkedEditingRangeAsync(input);
    }

    [Fact]
    public async Task Html_NestedTags_Outer()
    {
        var input = $"""
            This is a Razor document.

            <[|d$$iv|]>
                <div>
                    Here is some content.
                </div>
            </[|div|]>

            The end.
            """;

        await VerifyLinkedEditingRangeAsync(input);
    }

    [Fact]
    public async Task Html_NestedTags_Inner()
    {
        var input = $"""
            This is a Razor document.

            <div>
                <[|d$$iv|]>
                    Here is some content.
                </[|div|]>
            </div>

            The end.
            """;

        await VerifyLinkedEditingRangeAsync(input);
    }

    [Fact]
    public async Task Html_SelfClosingTag()
    {
        var input = $"""
            This is a Razor document.

            <b$$r />
            Here is some content.

            The end.
            """;

        await VerifyLinkedEditingRangeAsync(input);
    }

    private async Task VerifyLinkedEditingRangeAsync(string input)
    {
        TestFileMarkupParser.GetPositionAndSpans(input, out input, out int cursorPosition, out ImmutableArray<TextSpan> spans);
        var document = CreateRazorDocument(input);
        var sourceText = await document.GetTextAsync(DisposalToken);
        sourceText.GetLineAndOffset(cursorPosition, out var lineIndex, out var characterIndex);

        var endpoint = new CohostLinkedEditingRangeEndpoint(RemoteServiceProvider, LoggerFactory);

        var request = new LinkedEditingRangeParams()
        {
            TextDocument = new TextDocumentIdentifier()
            {
                Uri = document.CreateUri()
            },
            Position = new Position()
            {
                Line = lineIndex,
                Character = characterIndex
            }
        };

        var result = await endpoint.GetTestAccessor().HandleRequestAsync(request, document, DisposalToken);

        if (spans.Length == 0)
        {
            Assert.Null(result);
            return;
        }

        Assert.NotNull(result);
        Assert.Equal(LinkedEditingRangeHelper.WordPattern, result.WordPattern);
        Assert.Equal(spans[0], result.Ranges[0].ToTextSpan(sourceText));
        Assert.Equal(spans[1], result.Ranges[1].ToTextSpan(sourceText));
    }
}
