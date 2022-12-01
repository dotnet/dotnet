// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Xunit;

namespace Microsoft.VisualStudio.Razor.IntegrationTests;

public class RazorCodeActionsTests : AbstractRazorEditorTest
{
    [IdeFact]
    public async Task RazorCodeActions_AddUsing()
    {
        // Create Warnings by removing usings
        await TestServices.SolutionExplorer.OpenFileAsync(RazorProjectConstants.BlazorProjectName, RazorProjectConstants.ImportsRazorFile, ControlledHangMitigatingCancellationToken);
        await TestServices.Editor.SetTextAsync("", ControlledHangMitigatingCancellationToken);

        // Open the file
        await TestServices.SolutionExplorer.OpenFileAsync(RazorProjectConstants.BlazorProjectName, RazorProjectConstants.CounterRazorFile, ControlledHangMitigatingCancellationToken);

        await TestServices.Editor.SetTextAsync("<SurveyPrompt></SurveyPrompt>", ControlledHangMitigatingCancellationToken);
        await TestServices.Editor.MoveCaretAsync(3, ControlledHangMitigatingCancellationToken);

        // Act
        var codeActions = await TestServices.Editor.InvokeCodeActionListAsync(ControlledHangMitigatingCancellationToken);

        // Assert
        var codeActionSet = Assert.Single(codeActions);
        var usingString = $"@using {RazorProjectConstants.BlazorProjectName}.Shared";
        var codeAction = Assert.Single(codeActionSet.Actions, a => a.DisplayText.Equals(usingString));

        await TestServices.Editor.InvokeCodeActionAsync(codeAction, ControlledHangMitigatingCancellationToken);

        await TestServices.Editor.VerifyTextContainsAsync(usingString, ControlledHangMitigatingCancellationToken);
    }

    [IdeFact]
    public async Task RazorCodeActions_ExtractToCodeBehind()
    {
        // Open the file
        await TestServices.SolutionExplorer.OpenFileAsync(RazorProjectConstants.BlazorProjectName, RazorProjectConstants.CounterRazorFile, ControlledHangMitigatingCancellationToken);

        await TestServices.Editor.PlaceCaretAsync("@code", 1, ControlledHangMitigatingCancellationToken);

        // Act
        var codeActions = await TestServices.Editor.InvokeCodeActionListAsync(ControlledHangMitigatingCancellationToken);

        // Assert
        var codeActionSet = Assert.Single(codeActions);
        var codeAction = Assert.Single(codeActionSet.Actions, a => a.DisplayText.Equals("Extract block to code behind"));

        await TestServices.Editor.InvokeCodeActionAsync(codeAction, ControlledHangMitigatingCancellationToken);

        await TestServices.Editor.WaitForActiveWindowByFileAsync("Counter.razor.cs", ControlledHangMitigatingCancellationToken);
    }
}
