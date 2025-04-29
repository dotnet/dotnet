// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Roslyn.LanguageServer.Protocol;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

public class CohostValidateBreakableRangeEndpointTest(ITestOutputHelper testOutputHelper) : CohostEndpointTestBase(testOutputHelper)
{
    [Fact]
    public async Task Handle_CSharpInHtml_ValidBreakpoint()
    {
        var input = """
                <div></div>

                @{
                    var currentCount = 1;
                }

                <p>@{|breakpoint:{|expected:currentCount|}|}</p>
                """;

        await VerifyBreakableRangeAsync(input);
    }

    [Fact]
    public async Task Handle_CSharpInAttribute_ValidBreakpoint()
    {
        var input = """
                <div></div>

                @{
                    var currentCount = 1;
                }

                <button class="btn btn-primary" disabled="@({|breakpoint:{|expected:currentCount > 3|}|})">Click me</button>
                """;

        await VerifyBreakableRangeAsync(input);
    }

    [Fact]
    public async Task Handle_CSharp_ValidBreakpoint()
    {
        var input = """
                <div></div>

                @{
                    {|breakpoint:{|expected:var x = GetX();|}|}
                }
                """;

        await VerifyBreakableRangeAsync(input);
    }

    [Fact]
    public async Task Handle_CSharp_InvalidBreakpointRemoved()
    {
        var input = """
                <div></div>

                @{
                    //{|breakpoint:var x = GetX();|}
                }
                """;

        await VerifyBreakableRangeAsync(input);
    }

    [Fact]
    public async Task Handle_CSharp_ValidBreakpointMoved()
    {
        var input = """
                <div></div>

                @{
                    {|breakpoint:var x = Goo;
                    {|expected:Goo;|}|}
                }
                """;

        await VerifyBreakableRangeAsync(input);
    }

    [Fact]
    public async Task Handle_Html_BreakpointRemoved()
    {
        var input = """
                {|breakpoint:<div></div>|}

                @{
                    var x = GetX();
                }
                """;

        await VerifyBreakableRangeAsync(input);
    }

    private async Task VerifyBreakableRangeAsync(TestCode input)
    {
        var document = CreateProjectAndRazorDocument(input.Text);
        var inputText = await document.GetTextAsync(DisposalToken);

        Assert.True(input.TryGetNamedSpans("breakpoint", out var breakpointSpans), "Test authoring failure: Expected at least one span named 'breakpoint'.");
        Assert.True(breakpointSpans.Length == 1, "Test authoring failure: Expected only one 'breakpoint' span.");

        var span = inputText.GetLinePositionSpan(breakpointSpans.Single());

        var endpoint = new CohostValidateBreakableRangeEndpoint(RemoteServiceInvoker);

        var result = await endpoint.GetTestAccessor().HandleRequestAsync(document, span, DisposalToken);

        if (!input.TryGetNamedSpans("expected", out var expected))
        {
            Assert.Null(result);
            return;
        }

        Assert.NotNull(result);

        var expectedRange = inputText.GetRange(expected.Single());
        Assert.Equal(expectedRange, result);
    }
}
