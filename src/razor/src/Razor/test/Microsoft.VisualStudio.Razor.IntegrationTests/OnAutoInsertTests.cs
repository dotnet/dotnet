// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor.Editor;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.Razor.IntegrationTests;

public class OnAutoInsertTests(ITestOutputHelper testOutputHelper) : AbstractRazorEditorTest(testOutputHelper)
{
    [IdeFact]
    public async Task CSharp_DocumentationComments()
    {
        // Open the file
        await TestServices.SolutionExplorer.OpenFileAsync(RazorProjectConstants.BlazorProjectName, RazorProjectConstants.ErrorCshtmlFile, ControlledHangMitigatingCancellationToken);

        await TestServices.Editor.SetTextAsync(@"
<div>
</div>

@functions
{
    //
    public void M()
    {
    }
}

", ControlledHangMitigatingCancellationToken);
        await TestServices.Editor.PlaceCaretAsync("//", charsOffset: 1, ControlledHangMitigatingCancellationToken);

        // Act
        TestServices.Input.Send("/");

        // Assert
        await TestServices.Editor.WaitForCurrentLineTextAsync("/// ", ControlledHangMitigatingCancellationToken);
    }

    [IdeFact]
    public async Task Html_AutoCloseTag()
    {
        // Open the file
        await TestServices.SolutionExplorer.OpenFileAsync(RazorProjectConstants.BlazorProjectName, RazorProjectConstants.ErrorCshtmlFile, ControlledHangMitigatingCancellationToken);

        await TestServices.Editor.SetTextAsync(@"
<div>
    <p
</div>

", ControlledHangMitigatingCancellationToken);
        await TestServices.Editor.PlaceCaretAsync("<p", charsOffset: 1, ControlledHangMitigatingCancellationToken);

        await TestServices.Editor.SetAdvancedSettingsAsync(ClientAdvancedSettings.Default with { AutoClosingTags = true }, ControlledHangMitigatingCancellationToken);

        // Act
        TestServices.Input.Send(">");

        // Assert
        await TestServices.Editor.WaitForCurrentLineTextAsync("<p></p>", ControlledHangMitigatingCancellationToken);
    }

    [IdeFact]
    public async Task Html_AutoCloseTag_Off()
    {
        await TestServices.SolutionExplorer.OpenFileAsync(RazorProjectConstants.BlazorProjectName, RazorProjectConstants.ErrorCshtmlFile, ControlledHangMitigatingCancellationToken);

        await TestServices.Editor.SetTextAsync(@"
<div>
    <p
</div>

", ControlledHangMitigatingCancellationToken);
        await TestServices.Editor.PlaceCaretAsync("<p", charsOffset: 1, ControlledHangMitigatingCancellationToken);

        await TestServices.Editor.SetAdvancedSettingsAsync(ClientAdvancedSettings.Default with { AutoClosingTags = false }, ControlledHangMitigatingCancellationToken);

        // Act
        TestServices.Input.Send(">");

        // Assert
        await TestServices.Editor.WaitForCurrentLineTextAsync("<p>", ControlledHangMitigatingCancellationToken);
    }
}
