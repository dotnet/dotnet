// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.VisualStudio.Text.Tagging;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.Razor.IntegrationTests;

public class DocumentHighlightTests(ITestOutputHelper testOutputHelper) : AbstractRazorEditorTest(testOutputHelper)
{
    [IdeFact]
    public async Task Html()
    {
        await TestServices.SolutionExplorer.OpenFileAsync(RazorProjectConstants.BlazorProjectName, RazorProjectConstants.ErrorCshtmlFile, ControlledHangMitigatingCancellationToken);

        // The 5th <p> happens to be one that was problematic, but is otherwise not special. See https://github.com/dotnet/razor/issues/9212
        await TestServices.Editor.PlaceCaretAsync("<p", charsOffset: 1, occurrence: 5, extendSelection: false, selectBlock: false, ControlledHangMitigatingCancellationToken);

        var tags = await TestServices.Editor.WaitForTagsAsync<ITextMarkerTag>(ControlledHangMitigatingCancellationToken);

        Assert.Collection(tags,
            t => Assert.Equal("<p>", t.Span.GetText()),
            t => Assert.Equal("</p>", t.Span.GetText()));
    }

    [IdeFact]
    public async Task CSharp()
    {
        await TestServices.SolutionExplorer.OpenFileAsync(RazorProjectConstants.BlazorProjectName, RazorProjectConstants.CounterRazorFile, ControlledHangMitigatingCancellationToken);

        await TestServices.Editor.PlaceCaretAsync("current", charsOffset: 1, occurrence: 5, extendSelection: false, selectBlock: false, ControlledHangMitigatingCancellationToken);

        var tags = await TestServices.Editor.WaitForTagsAsync<ITextMarkerTag>(ControlledHangMitigatingCancellationToken);

        Assert.Equal(3, tags.Length);
        Assert.All(tags, t => Assert.Equal("currentCount", t.Span.GetText()));
    }
}
