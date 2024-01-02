// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.Razor.IntegrationTests;

public class MultiTargetProjectTests(ITestOutputHelper testOutputHelper) : AbstractRazorEditorTest(testOutputHelper)
{
    protected override string TargetFrameworkElement => $"""<TargetFrameworks>net7.0;{TargetFramework}</TargetFrameworks>""";

    [IdeFact]
    public async Task ValidateMultipleJsonFiles()
    {
        var solutionPath = await TestServices.SolutionExplorer.GetDirectoryNameAsync(ControlledHangMitigatingCancellationToken);

        // This is a little odd, but there is no "real" way to check this via VS, and one of the most important things this test can do
        // is ensure that each target framework gets its own project.razor.bin file, and doesn't share one from a cache or anything.
        Assert.Equal(2, GetProjectRazorJsonFileCount());

        int GetProjectRazorJsonFileCount()
            => Directory.EnumerateFiles(solutionPath, "project.razor.*.bin", SearchOption.AllDirectories).Count();
    }

    [IdeFact]
    public async Task OpenExistingProject()
    {
        var solutionPath = await TestServices.SolutionExplorer.GetDirectoryNameAsync(ControlledHangMitigatingCancellationToken);
        var expectedProjectFileName = await TestServices.SolutionExplorer.GetAbsolutePathForProjectRelativeFilePathAsync(RazorProjectConstants.BlazorProjectName, RazorProjectConstants.ProjectFile, ControlledHangMitigatingCancellationToken);

        await TestServices.SolutionExplorer.CloseSolutionAsync(ControlledHangMitigatingCancellationToken);

        var solutionFileName = Path.Combine(solutionPath, RazorProjectConstants.BlazorSolutionName + ".sln");
        await TestServices.SolutionExplorer.OpenSolutionAsync(solutionFileName, ControlledHangMitigatingCancellationToken);

        await TestServices.Workspace.WaitForProjectSystemAsync(ControlledHangMitigatingCancellationToken);

        // We open the Index.razor file, and wait for 3 RazorComponentElement's to be classified, as that
        // way we know the LSP server is up, running, and has processed both local and library-sourced Components
        await TestServices.SolutionExplorer.OpenFileAsync(RazorProjectConstants.BlazorProjectName, RazorProjectConstants.IndexRazorFile, ControlledHangMitigatingCancellationToken);

        await TestServices.Workspace.WaitForProjectSystemAsync(ControlledHangMitigatingCancellationToken);

        await TestServices.Editor.PlaceCaretAsync("</PageTitle>", charsOffset: 1, ControlledHangMitigatingCancellationToken);
        await TestServices.Editor.WaitForComponentClassificationAsync(ControlledHangMitigatingCancellationToken, count: 3);

        // Make sure the test framework didn't do something weird and create new project
        var actualProjectFileName = await TestServices.SolutionExplorer.GetAbsolutePathForProjectRelativeFilePathAsync(RazorProjectConstants.BlazorProjectName, RazorProjectConstants.ProjectFile, ControlledHangMitigatingCancellationToken);
        Assert.Equal(expectedProjectFileName, actualProjectFileName);
    }

    [IdeTheory]
    [CombinatorialData]
    public async Task OpenExistingProject_WithReopenedFile(bool deleteProjectRazorJson)
    {
        var solutionPath = await TestServices.SolutionExplorer.GetDirectoryNameAsync(ControlledHangMitigatingCancellationToken);
        var expectedProjectFileName = await TestServices.SolutionExplorer.GetAbsolutePathForProjectRelativeFilePathAsync(RazorProjectConstants.BlazorProjectName, RazorProjectConstants.ProjectFile, ControlledHangMitigatingCancellationToken);

        // Open SurveyPrompt and make sure its all up and running
        await TestServices.SolutionExplorer.OpenFileAsync(RazorProjectConstants.BlazorProjectName, RazorProjectConstants.ErrorCshtmlFile, ControlledHangMitigatingCancellationToken);
        await TestServices.Editor.WaitForSemanticClassificationAsync("class name", ControlledHangMitigatingCancellationToken, count: 1);

        await TestServices.SolutionExplorer.CloseSolutionAsync(ControlledHangMitigatingCancellationToken);

        if (deleteProjectRazorJson)
        {
            // Clear out the project.razor.bin file which ensures our restored file will have to be in the Misc Project
            var projectRazorJsonFileName = Directory.EnumerateFiles(solutionPath, "project.razor.*.bin", SearchOption.AllDirectories).First();
            File.Delete(projectRazorJsonFileName);
        }

        var solutionFileName = Path.Combine(solutionPath, RazorProjectConstants.BlazorSolutionName + ".sln");
        await TestServices.SolutionExplorer.OpenSolutionAsync(solutionFileName, ControlledHangMitigatingCancellationToken);

        await TestServices.Workspace.WaitForProjectSystemAsync(ControlledHangMitigatingCancellationToken);

        await TestServices.Editor.WaitForSemanticClassificationAsync("class name", ControlledHangMitigatingCancellationToken, count: 1);

        TestServices.Input.Send("1");

        // Make sure the test framework didn't do something weird and create new project
        var actualProjectFileName = await TestServices.SolutionExplorer.GetAbsolutePathForProjectRelativeFilePathAsync(RazorProjectConstants.BlazorProjectName, RazorProjectConstants.ProjectFile, ControlledHangMitigatingCancellationToken);
        Assert.Equal(expectedProjectFileName, actualProjectFileName);

        await TestServices.Editor.CloseCodeFileAsync(RazorProjectConstants.BlazorProjectName, RazorProjectConstants.ErrorCshtmlFile, saveFile: false, ControlledHangMitigatingCancellationToken);
    }
}
