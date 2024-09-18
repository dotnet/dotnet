// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis.Text;
using Roslyn.Test.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

public class CohostDocumentSpellCheckEndpointTest(ITestOutputHelper testOutputHelper) : CohostEndpointTestBase(testOutputHelper)
{
    [Fact]
    public async Task Handle()
    {
        var input = """
            @page [|"this is csharp"|]

            <div>[|

                Eat more chickin.

            |]</div>

            <script>
                // no spell checking of script tags
                @([|"unless they contain csharp"|])
            </script>

            <style>
                // no spell checking of style tags
                @([|"unless they contain csharp"|])
            </style>

            @{ var [|x|] = [|"csharp"|];

            @*[| Eat more chickin. |]*@

            <div class="[|fush|]" />

            @code
            {
                void [|M|]()
                {
                    [|// Eat more chickin|]
                }
            }
            """;

        await VerifySemanticTokensAsync(input);
    }

    private async Task VerifySemanticTokensAsync(TestCode input)
    {
        var document = await CreateProjectAndRazorDocumentAsync(input.Text);
        var sourceText = await document.GetTextAsync(DisposalToken);

        var endpoint = new CohostDocumentSpellCheckEndpoint(RemoteServiceInvoker);

        var span = new LinePositionSpan(new(0, 0), new(sourceText.Lines.Count, 0));

        var result = await endpoint.GetTestAccessor().HandleRequestAsync(document, DisposalToken);

        var ranges = result.First().Ranges.AssumeNotNull();

        // To make for easier test failure analysis, we convert the ranges back to the test input, so we can show a diff
        // rather than "Expected 23, got 53" and leave the developer to deal with what that means.
        // As a bonus, this also ensures the ranges array has the right number of elements (ie, multiple of 3)
        var absoluteRanges = new List<(int Start, int End)>();
        var absoluteStart = 0;
        for (var i = 0; i < ranges.Length; i += 3)
        {
            var kind = ranges[i];
            var start = ranges[i + 1];
            var length = ranges[i + 2];

            absoluteStart += start;
            absoluteRanges.Add((absoluteStart, absoluteStart + length));
            absoluteStart += length;
        }

        // Make sure the response is sorted correctly, or the IDE will complain
        Assert.True(absoluteRanges.SequenceEqual(absoluteRanges.OrderBy(r => r.Start)), "Results are not in order!");

        absoluteRanges.Reverse();

        var actual = input.Text;
        foreach (var (start, end) in absoluteRanges)
        {
            actual = actual.Insert(end, "|]").Insert(start, "[|");
        }

        AssertEx.EqualOrDiff(input.OriginalInput, actual);
    }
}
