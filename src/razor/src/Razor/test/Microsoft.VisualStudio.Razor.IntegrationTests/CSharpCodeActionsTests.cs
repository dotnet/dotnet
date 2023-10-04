// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.Razor.IntegrationTests;

public class CSharpCodeActionsTests(ITestOutputHelper testOutputHelper) : AbstractRazorEditorTest(testOutputHelper)
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
            @using System.Data

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
            @using System.Data

            @{
                var x = ConflictOption.CompareAllSearchableValues;
            }

            """, ControlledHangMitigatingCancellationToken);
    }

    [IdeFact]
    public async Task CSharpCodeActionsTests_IntroduceLocal()
    {
        // Open the file
        await TestServices.SolutionExplorer.OpenFileAsync(RazorProjectConstants.BlazorProjectName, RazorProjectConstants.CounterRazorFile, ControlledHangMitigatingCancellationToken);
        await TestServices.Editor.SetTextAsync("""
            @code {
                void M(string[] args)
                {
                    if (args.First().Length == 0)
                    {
                    }

                    if (args.First().Length == 0)
                    {
                    }
                }
            }
            """, ControlledHangMitigatingCancellationToken);

        await TestServices.Editor.PlaceCaretAsync("args.First()", charsOffset: 0, occurrence: 1, extendSelection: false, selectBlock: false, ControlledHangMitigatingCancellationToken);

        // Act
        var codeActions = await TestServices.Editor.InvokeCodeActionListAsync(ControlledHangMitigatingCancellationToken);

        // Assert
        var introduceLocal = codeActions.FirstOrDefault(a => a.Actions.Single().DisplayText.Equals("Introduce local"));
        Assert.NotNull(introduceLocal);

        var codeAction = introduceLocal.Actions.First();

        Assert.True(codeAction.HasActionSets);

        codeAction = (await codeAction.GetActionSetsAsync(ControlledHangMitigatingCancellationToken)).First().Actions.First();

        await TestServices.Editor.InvokeCodeActionAsync(codeAction, ControlledHangMitigatingCancellationToken);

        await TestServices.Editor.WaitForTextChangeAsync("""
                @code {
                    void M(string[] args)
                    {
                        string v = args.First();
                        if (v.Length == 0)
                        {
                        }

                        if (args.First().Length == 0)
                        {
                        }
                    }
                }
                """, ControlledHangMitigatingCancellationToken);
    }

    [IdeFact]
    public async Task CSharpCodeActionsTests_IntroduceLocal_All()
    {
        // Open the file
        await TestServices.SolutionExplorer.OpenFileAsync(RazorProjectConstants.BlazorProjectName, RazorProjectConstants.CounterRazorFile, ControlledHangMitigatingCancellationToken);
        await TestServices.Editor.SetTextAsync("""
            @code {
                void M(string[] args)
                {
                    if (args.First().Length == 0)
                    {
                    }

                    if (args.First().Length == 0)
                    {
                    }
                }
            }
            """, ControlledHangMitigatingCancellationToken);

        await TestServices.Editor.PlaceCaretAsync("args.First()", charsOffset: 0, occurrence: 1, extendSelection: false, selectBlock: false, ControlledHangMitigatingCancellationToken);

        // Act
        var codeActions = await TestServices.Editor.InvokeCodeActionListAsync(ControlledHangMitigatingCancellationToken);

        // Assert
        var introduceLocal = codeActions.FirstOrDefault(a => a.Actions.Single().DisplayText.Equals("Introduce local"));
        Assert.NotNull(introduceLocal);

        var codeAction = introduceLocal.Actions.First();

        Assert.True(codeAction.HasActionSets);

        codeAction = (await codeAction.GetActionSetsAsync(ControlledHangMitigatingCancellationToken)).First().Actions.Skip(1).First();

        await TestServices.Editor.InvokeCodeActionAsync(codeAction, ControlledHangMitigatingCancellationToken);

        await TestServices.Editor.WaitForTextChangeAsync("""
                @code {
                    void M(string[] args)
                    {
                        string v = args.First();
                        if (v.Length == 0)
                        {
                        }

                        if (v.Length == 0)
                        {
                        }
                    }
                }
                """, ControlledHangMitigatingCancellationToken);
    }
}
