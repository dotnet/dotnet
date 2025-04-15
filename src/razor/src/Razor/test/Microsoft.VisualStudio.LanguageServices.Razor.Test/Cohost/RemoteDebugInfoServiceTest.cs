// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.Remote;
using Microsoft.CodeAnalysis.Text;
using Roslyn.Test.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

public class RemoteDebugInfoServiceTest(ITestOutputHelper testOutputHelper) : CohostEndpointTestBase(testOutputHelper)
{
    [Fact]
    public async Task ResolveProximityExpressionsAsync_Html()
    {
        var input = """
                $$<div></div>

                @{
                    var currentCount = 1;
                }

                <p>@currentCount</p>
                """;

        await VerifyProximityExpressionsAsync(input, ["__builder", "this"]);
    }

    [Fact]
    public async Task ResolveProximityExpressionsAsync_ExplicitExpression()
    {
        var input = """
                <div></div>

                @{
                    var currentC$$ount = 1;
                }

                <p>@[|currentCount|]</p>
                """;

        await VerifyProximityExpressionsAsync(input, ["__builder", "this"]);
    }

    [Fact]
    public async Task ResolveProximityExpressionsAsync_OutsideImplicitExpression()
    {
        var input = """
                <div></div>

                @{
                    var [|currentCount|] = 1;
                }

                $$<p>@currentCount</p>
                """;

        await VerifyProximityExpressionsAsync(input, ["__builder", "this"]);
    }

    [Fact]
    public async Task ResolveProximityExpressionsAsync_ImplicitExpression()
    {
        var input = """
                <div></div>

                @{
                    var [|currentCount|] = 1;
                }

                <p>@curr$$entCount</p>
                """;

        await VerifyProximityExpressionsAsync(input, ["__builder", "this"]);
    }

    [Fact]
    public async Task ResolveProximityExpressionsAsync_CodeBlock()
    {
        var input = """
                <div></div>

                <p>@currentCount</p>

                @code
                {
                    private int [|currentCount|];
                    private bool hasBeenClicked;

                    private void M()
                    {
                        current$$Count++;
                    }
                }
                """;

        await VerifyProximityExpressionsAsync(input, ["this"]);
    }

    [Fact]
    public async Task ResolveBreakpointRangeAsync_Html()
    {
        var input = """
                $$<div></div>

                @{
                    var currentCount = 1;
                }

                <p>@currentCount</p>
                """;

        await VerifyBreakpointRangeAsync(input);
    }

    [Fact]
    public async Task ResolveBreakpointRangeAsync_CodeBlock()
    {
        var input = """
                <div></div>

                <p>@currentCount</p>

                @code
                {
                    private int currentCount;

                    private void M()
                    {
                        [|current$$Count++;|]
                    }
                }
                """;

        await VerifyBreakpointRangeAsync(input);
    }

    [Fact]
    public async Task ResolveBreakpointRangeAsync_CodeBlock_InvalidLocation()
    {
        var input = """
                <div></div>

                <p>@currentCount</p>

                @code
                {
                    private bool hasBeen$$Clicked;
                }
                """;

        await VerifyBreakpointRangeAsync(input);
    }

    [Fact]
    public async Task ResolveBreakpointRangeAsync_OutsideImplicitExpression()
    {
        var input = """
                <div></div>

                @{
                    var currentCount = 1;
                }

                $$<p>@[|currentCount|]</p>
                """;

        await VerifyBreakpointRangeAsync(input);
    }

    private async Task VerifyProximityExpressionsAsync(TestCode input, string[] extraExpressions)
    {
        var document = CreateProjectAndRazorDocument(input.Text);
        var inputText = await document.GetTextAsync(DisposalToken);

        var span = inputText.GetLinePosition(input.Position);

        var result = await RemoteServiceInvoker
            .TryInvokeAsync<IRemoteDebugInfoService, string[]?>(
                document.Project.Solution,
                (service, solutionInfo, cancellationToken) =>
                    service.ResolveProximityExpressionsAsync(solutionInfo, document.Id, span, cancellationToken),
                DisposalToken);

        if (!input.HasSpans)
        {
            Assert.Null(result);
            return;
        }

        Assert.NotNull(result);

        var expected = input.Spans.Select(inputText.GetSubTextString).Concat(extraExpressions).OrderAsArray();
        AssertEx.SequenceEqual(expected, result.OrderAsArray());
    }

    private async Task VerifyBreakpointRangeAsync(TestCode input)
    {
        var document = CreateProjectAndRazorDocument(input.Text);
        var inputText = await document.GetTextAsync(DisposalToken);

        var span = inputText.GetLinePosition(input.Position);

        var result = await RemoteServiceInvoker
            .TryInvokeAsync<IRemoteDebugInfoService, LinePositionSpan?>(
                document.Project.Solution,
                (service, solutionInfo, cancellationToken) =>
                    service.ResolveBreakpointRangeAsync(solutionInfo, document.Id, span, cancellationToken),
                DisposalToken);

        if (result is not { } breakpoint)
        {
            Assert.False(input.HasSpans);
            return;
        }

        var expected = inputText.GetLinePositionSpan(input.Span);
        Assert.Equal(expected, breakpoint);
    }
}
