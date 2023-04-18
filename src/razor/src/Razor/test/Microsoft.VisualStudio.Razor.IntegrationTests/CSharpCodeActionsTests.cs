// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Microsoft.VisualStudio.Razor.IntegrationTests;

public class CSharpCodeActionsTests : AbstractRazorEditorTest
{
    [IdeFact(Skip = "https://github.com/dotnet/razor/issues/8409")]
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

    [IdeFact]
    public async Task CSharpCodeActionsTests_FullyQualify()
    {
        // Open the file
        await TestServices.SolutionExplorer.OpenFileAsync(RazorProjectConstants.BlazorProjectName, RazorProjectConstants.CounterRazorFile, ControlledHangMitigatingCancellationToken);
        await TestServices.Editor.SetTextAsync("""

            @{
                var x = ConflictOption.CompareAllSearchableValues;
            }

            """, ControlledHangMitigatingCancellationToken);

        await TestServices.Editor.PlaceCaretAsync("ConflictOption", charsOffset: 0, occurrence: 1, extendSelection: false, selectBlock: false, ControlledHangMitigatingCancellationToken);

        // Act
        var codeActions = await TestServices.Editor.InvokeCodeActionListAsync(ControlledHangMitigatingCancellationToken);

        // Assert
        Assert.Collection(codeActions,
            a => Assert.Equal("@using System.Data", a.Actions.Single().DisplayText),
            a => Assert.Equal("System.Data.ConflictOption", a.Actions.Single().DisplayText));

        var codeAction = codeActions.ElementAt(1).Actions.First();

        await TestServices.Editor.InvokeCodeActionAsync(codeAction, ControlledHangMitigatingCancellationToken);

        await TestServices.Editor.WaitForCurrentLineTextAsync("var x = System.Data.ConflictOption.CompareAllSearchableValues;", ControlledHangMitigatingCancellationToken);
    }

    [IdeFact]
    public async Task CSharpCodeActionsTests_AddUsing()
    {
        // Open the file
        await TestServices.SolutionExplorer.OpenFileAsync(RazorProjectConstants.BlazorProjectName, RazorProjectConstants.CounterRazorFile, ControlledHangMitigatingCancellationToken);
        await TestServices.Editor.SetTextAsync("""

            @{
                var x = ConflictOption.CompareAllSearchableValues;
            }

            """, ControlledHangMitigatingCancellationToken);

        await TestServices.Editor.PlaceCaretAsync("ConflictOption", charsOffset: 0, occurrence: 1, extendSelection: false, selectBlock: false, ControlledHangMitigatingCancellationToken);

        // Act
        var codeActions = await TestServices.Editor.InvokeCodeActionListAsync(ControlledHangMitigatingCancellationToken);

        // Assert
        Assert.Collection(codeActions,
            a => Assert.Equal("@using System.Data", a.Actions.Single().DisplayText),
            a => Assert.Equal("System.Data.ConflictOption", a.Actions.Single().DisplayText));

        var codeAction = codeActions.First().Actions.First();

        await TestServices.Editor.InvokeCodeActionAsync(codeAction, ControlledHangMitigatingCancellationToken);

        await TestServices.Editor.WaitForTextChangeAsync("""
            @using System.Data;

            @{
                var x = ConflictOption.CompareAllSearchableValues;
            }

            """, ControlledHangMitigatingCancellationToken);
    }

    [IdeFact]
    public async Task CSharpCodeActionsTests_AddUsing_WithTypo()
    {
        // Open the file
        await TestServices.SolutionExplorer.OpenFileAsync(RazorProjectConstants.BlazorProjectName, RazorProjectConstants.CounterRazorFile, ControlledHangMitigatingCancellationToken);
        await TestServices.Editor.SetTextAsync("""

            @{
                var x = Conflictoption.CompareAllSearchableValues;
            }

            """, ControlledHangMitigatingCancellationToken);

        await TestServices.Editor.PlaceCaretAsync("Conflictoption", charsOffset: 0, occurrence: 1, extendSelection: false, selectBlock: false, ControlledHangMitigatingCancellationToken);

        // Act
        var codeActions = await TestServices.Editor.InvokeCodeActionListAsync(ControlledHangMitigatingCancellationToken);

        // Assert
        Assert.Collection(codeActions,
            a => Assert.Equal("ConflictOption - @using System.Data", a.Actions.Single().DisplayText));

        var codeAction = codeActions.First().Actions.First();

        await TestServices.Editor.InvokeCodeActionAsync(codeAction, ControlledHangMitigatingCancellationToken);

        await TestServices.Editor.WaitForTextChangeAsync("""
            @using System.Data;

            @{
                var x = ConflictOption.CompareAllSearchableValues;
            }

            """, ControlledHangMitigatingCancellationToken);
    }
}
