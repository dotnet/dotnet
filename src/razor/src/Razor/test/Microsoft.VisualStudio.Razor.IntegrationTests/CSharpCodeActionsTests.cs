// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Xunit;

namespace Microsoft.VisualStudio.Razor.IntegrationTests;

public class CSharpCodeActionsTests : AbstractRazorEditorTest
{
    [IdeFact]
    public async Task CSharpCodeActionsTests_MakeExpressionBodiedMethod()
    {
        // Open the file
        await TestServices.SolutionExplorer.OpenFileAsync(RazorProjectConstants.BlazorProjectName, RazorProjectConstants.CounterRazorFile, ControlledHangMitigatingCancellationToken);

        await TestServices.Editor.PlaceCaretAsync("IncrementCount", charsOffset: 2, occurrence: 2, extendSelection: false, selectBlock: false, ControlledHangMitigatingCancellationToken);

        // Act
        var codeActions = await TestServices.Editor.InvokeCodeActionListAsync(ControlledHangMitigatingCancellationToken);

        // Assert
        var codeActionSet = Assert.Single(codeActions);
        var codeAction = Assert.Single(codeActionSet.Actions, a => a.DisplayText.Equals("Use expression body for method"));

        await TestServices.Editor.InvokeCodeActionAsync(codeAction, ControlledHangMitigatingCancellationToken);

        await TestServices.Editor.WaitForCurrentLineTextAsync("private void IncrementCount() => currentCount++;", ControlledHangMitigatingCancellationToken);
    }
}
