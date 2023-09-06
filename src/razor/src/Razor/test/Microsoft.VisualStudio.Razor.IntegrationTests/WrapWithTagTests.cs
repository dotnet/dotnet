// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.Razor.IntegrationTests;

public class WrapWithTagTests(ITestOutputHelper testOutputHelper) : AbstractRazorEditorTest(testOutputHelper)
{
    [IdeFact]
    public async Task WrapWithTag_RootLevelElement()
    {
        // Open the file
        await TestServices.SolutionExplorer.OpenFileAsync(RazorProjectConstants.BlazorProjectName, RazorProjectConstants.CounterRazorFile, ControlledHangMitigatingCancellationToken);

        await TestServices.Editor.PlaceCaretAsync("h1", charsOffset: -1, ControlledHangMitigatingCancellationToken);

        await TestServices.Editor.WaitForComponentClassificationAsync(ControlledHangMitigatingCancellationToken);

        // Act
        WrapWithTag();

        // Assert
        await TestServices.Editor.WaitForCurrentLineTextAsync("<div><h1>Counter</h1></div>", ControlledHangMitigatingCancellationToken);
    }

    [IdeFact]
    public async Task WrapWithTag_CSharpImplicitStatement()
    {
        // Open the file
        await TestServices.SolutionExplorer.OpenFileAsync(RazorProjectConstants.BlazorProjectName, RazorProjectConstants.CounterRazorFile, ControlledHangMitigatingCancellationToken);

        await TestServices.Editor.PlaceCaretAsync("@current", charsOffset: 2, ControlledHangMitigatingCancellationToken);

        await TestServices.Editor.WaitForComponentClassificationAsync(ControlledHangMitigatingCancellationToken);

        // Act
        WrapWithTag();

        // Assert
        await TestServices.Editor.WaitForCurrentLineTextAsync("<p role=\"status\">Current count: <div>@currentCount</div></p>", ControlledHangMitigatingCancellationToken);
    }

    [IdeFact]
    public async Task WrapWithTag_Whole_CSharpImplicitStatement()
    {
        // Open the file
        await TestServices.SolutionExplorer.OpenFileAsync(RazorProjectConstants.BlazorProjectName, RazorProjectConstants.CounterRazorFile, ControlledHangMitigatingCancellationToken);

        await TestServices.Editor.PlaceCaretAsync("@currentCount", charsOffset: 0, occurrence: 1, extendSelection: false, selectBlock: true, ControlledHangMitigatingCancellationToken);

        await TestServices.Editor.WaitForComponentClassificationAsync(ControlledHangMitigatingCancellationToken);

        // Act
        WrapWithTag();

        // Assert
        await TestServices.Editor.WaitForCurrentLineTextAsync("<p role=\"status\">Current count: <div>@currentCount</div></p>", ControlledHangMitigatingCancellationToken);
    }

    [IdeFact]
    public async Task WrapWithTag_ChildElement()
    {
        // Open the file
        await TestServices.SolutionExplorer.OpenFileAsync(RazorProjectConstants.BlazorProjectName, RazorProjectConstants.FetchDataRazorFile, ControlledHangMitigatingCancellationToken);

        await TestServices.Editor.PlaceCaretAsync("<em", charsOffset: 1, ControlledHangMitigatingCancellationToken);

        await TestServices.Editor.WaitForComponentClassificationAsync(ControlledHangMitigatingCancellationToken);

        // Act
        WrapWithTag();

        // Assert
        await TestServices.Editor.WaitForCurrentLineTextAsync("<p><div><em>Loading...</em></div></p>", ControlledHangMitigatingCancellationToken);
    }

    [IdeFact]
    public async Task WrapWithTag_Multiline()
    {
        // Open the file
        await TestServices.SolutionExplorer.OpenFileAsync(RazorProjectConstants.BlazorProjectName, RazorProjectConstants.IndexRazorFile, ControlledHangMitigatingCancellationToken);

        await TestServices.Editor.SetTextAsync("""
            @{
                var items = new[] { 1, 2, 3, 4 };
            }

            <PageTitle>Temp</PageTitle>

            <div>
                <table>
                    @foreach (var item in items) {
                        <tr>
                            <td>@item</td>
                        </tr>
                    }
                </table>
            </div>
            """, ControlledHangMitigatingCancellationToken);

        await TestServices.Editor.PlaceCaretAsync("table", charsOffset: -1, ControlledHangMitigatingCancellationToken);

        await TestServices.Editor.WaitForComponentClassificationAsync(ControlledHangMitigatingCancellationToken);

        // Act
        WrapWithTag();

        // Assert
        await TestServices.Editor.WaitForTextChangeAsync("""
            @{
                var items = new[] { 1, 2, 3, 4 };
            }

            <PageTitle>Temp</PageTitle>

            <div>
                <div>
                    <table>
                        @foreach (var item in items) {
                            <tr>
                                <td>@item</td>
                            </tr>
                        }
                    </table>
                </div>
            </div>
            """, ControlledHangMitigatingCancellationToken);
    }

    [ConditionalSkipIdeFact(Issue = "https://github.com/dotnet/razor/issues/8856")]
    public async Task WrapWithTag_SelfClosingTag()
    {
        // Open the file
        await TestServices.SolutionExplorer.OpenFileAsync(RazorProjectConstants.BlazorProjectName, RazorProjectConstants.IndexRazorFile, ControlledHangMitigatingCancellationToken);

        await TestServices.Editor.PlaceCaretAsync("SurveyPrompt", charsOffset: -1, ControlledHangMitigatingCancellationToken);

        await TestServices.Editor.WaitForComponentClassificationAsync(ControlledHangMitigatingCancellationToken);

        // Act
        WrapWithTag();

        // Assert
        await TestServices.Editor.WaitForCurrentLineTextAsync("<div><SurveyPrompt Title=\"How is Blazor working for you?\" /></div>", ControlledHangMitigatingCancellationToken);
    }

    private void WrapWithTag()
    {
        // % == Alt, + == Shift, so this is Alt+Shift+W
        TestServices.Input.Send("%+w");
    }
}
