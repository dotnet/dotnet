// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.Razor.IntegrationTests;

public class ProjectTests(ITestOutputHelper testOutputHelper) : AbstractRazorEditorTest(testOutputHelper)
{
    [IdeFact]
    public async Task CreateFromTemplateAsync()
    {
        await TestServices.SolutionExplorer.OpenFileAsync(RazorProjectConstants.BlazorProjectName, RazorProjectConstants.CounterRazorFile, ControlledHangMitigatingCancellationToken);
        await TestServices.SolutionExplorer.CloseSolutionAsync(ControlledHangMitigatingCancellationToken);
    }

    [IdeFact(Skip = "https://github.com/dotnet/razor/issues/9200")]
    public async Task ChangeTargetFramework()
    {
        var solutionPath = await TestServices.SolutionExplorer.GetDirectoryNameAsync(ControlledHangMitigatingCancellationToken);

        Assert.Equal(1, GetProjectRazorJsonFileCount());

        await TestServices.SolutionExplorer.OpenFileAsync(RazorProjectConstants.BlazorProjectName, RazorProjectConstants.ProjectFile, ControlledHangMitigatingCancellationToken);

        await TestServices.Editor.PlaceCaretAsync("<TargetFramework", charsOffset: -1, ControlledHangMitigatingCancellationToken);

        var currentLine = await TestServices.Editor.GetCurrentLineTextAsync(ControlledHangMitigatingCancellationToken);
        var tfElement = currentLine.Contains("net6.0")
            ? "<TargetFramework>net7.0</TargetFramework>"
            : "<TargetFramework>net6.0</TargetFramework>";

        await TestServices.Editor.InvokeDeleteLineAsync(ControlledHangMitigatingCancellationToken);

        await TestServices.Editor.InsertTextAsync(tfElement + Environment.NewLine, ControlledHangMitigatingCancellationToken);

        await TestServices.Editor.CloseCodeFileAsync(RazorProjectConstants.BlazorProjectName, RazorProjectConstants.ProjectFile, saveFile: true, ControlledHangMitigatingCancellationToken);

        await TestServices.Workspace.WaitForProjectSystemAsync(ControlledHangMitigatingCancellationToken);

        // We open the Index.razor file, and wait for 3 RazorComponentElement's to be classified, as that
        // way we know the LSP server is up, running, and has processed both local and library-sourced Components
        await TestServices.SolutionExplorer.OpenFileAsync(RazorProjectConstants.BlazorProjectName, RazorProjectConstants.IndexRazorFile, ControlledHangMitigatingCancellationToken);

        await TestServices.Workspace.WaitForProjectSystemAsync(ControlledHangMitigatingCancellationToken);

        await TestServices.Editor.PlaceCaretAsync("</PageTitle>", charsOffset: 1, ControlledHangMitigatingCancellationToken);
        await TestServices.Editor.WaitForComponentClassificationAsync(ControlledHangMitigatingCancellationToken, count: 3);

        // This is a little odd, but there is no "real" way to check this via VS, and one of the most important things this test can do
        // is ensure that each target framework gets its own project.razor.bin file, and doesn't share one from a cache or anything.
        Assert.Equal(2, GetProjectRazorJsonFileCount());

        int GetProjectRazorJsonFileCount()
            => Directory.EnumerateFiles(solutionPath, "project.razor.*.bin", SearchOption.AllDirectories).Count();
    }

    [IdeFact]
    public async Task MultiTargetProject()
    {
        var solutionPath = await TestServices.SolutionExplorer.GetDirectoryNameAsync(ControlledHangMitigatingCancellationToken);

        Assert.Equal(1, GetProjectRazorJsonFileCount());

        var projectFileName = await TestServices.SolutionExplorer.GetAbsolutePathForProjectRelativeFilePathAsync(RazorProjectConstants.BlazorProjectName, RazorProjectConstants.ProjectFile, ControlledHangMitigatingCancellationToken);

        // CPS doesn't support changing from single targeting to multi-targeting while a project is open
        await TestServices.SolutionExplorer.CloseSolutionAsync(ControlledHangMitigatingCancellationToken);

        await TestServices.RazorProjectSystem.WaitForLSPServerDeactivatedAsync(ControlledHangMitigatingCancellationToken);

        var sb = new StringBuilder();
        foreach (var line in File.ReadAllLines(projectFileName))
        {
            if (line.Contains("<TargetFramework>"))
            {
                sb.AppendLine("<TargetFrameworks>net6.0;net7.0</TargetFrameworks>");
            }
            else
            {
                sb.AppendLine(line);
            }
        }

        File.WriteAllText(projectFileName, sb.ToString());

        var projectFolder = Path.GetDirectoryName(projectFileName);
        // Clear out the obj folder, so we don't break the test when the default project is updated in the project template, resulting in 3 .json files
        Directory.Delete(Path.Combine(projectFolder, "obj"), recursive: true);
        var solutionFileName = Path.Combine(solutionPath, RazorProjectConstants.BlazorSolutionName + ".sln");
        await TestServices.SolutionExplorer.OpenSolutionAsync(solutionFileName, ControlledHangMitigatingCancellationToken);

        await TestServices.Workspace.WaitForProjectSystemAsync(ControlledHangMitigatingCancellationToken);

        // We open the Index.razor file, and wait for 3 RazorComponentElement's to be classified, as that
        // way we know the LSP server is up, running, and has processed both local and library-sourced Components
        await TestServices.SolutionExplorer.OpenFileAsync(RazorProjectConstants.BlazorProjectName, RazorProjectConstants.IndexRazorFile, ControlledHangMitigatingCancellationToken);

        await TestServices.RazorProjectSystem.WaitForLSPServerActivatedAsync(ControlledHangMitigatingCancellationToken);

        await TestServices.Workspace.WaitForProjectSystemAsync(ControlledHangMitigatingCancellationToken);

        await TestServices.Editor.PlaceCaretAsync("</PageTitle>", charsOffset: 1, ControlledHangMitigatingCancellationToken);
        await TestServices.Editor.WaitForComponentClassificationAsync(ControlledHangMitigatingCancellationToken, count: 3);

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

    [IdeFact]
    public async Task OpenExistingProject_WithReopenedFile()
    {
        var solutionPath = await TestServices.SolutionExplorer.GetDirectoryNameAsync(ControlledHangMitigatingCancellationToken);
        var expectedProjectFileName = await TestServices.SolutionExplorer.GetAbsolutePathForProjectRelativeFilePathAsync(RazorProjectConstants.BlazorProjectName, RazorProjectConstants.ProjectFile, ControlledHangMitigatingCancellationToken);

        // Open SurveyPrompt and make sure its all up and running
        await TestServices.SolutionExplorer.OpenFileAsync(RazorProjectConstants.BlazorProjectName, RazorProjectConstants.ErrorCshtmlFile, ControlledHangMitigatingCancellationToken);
        await TestServices.Editor.WaitForSemanticClassificationAsync("class name", ControlledHangMitigatingCancellationToken, count: 1);

        await TestServices.SolutionExplorer.CloseSolutionAsync(ControlledHangMitigatingCancellationToken);

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

    [IdeFact]
    public async Task OpenExistingProject_WithReopenedFile_NoProjectRazorJson()
    {
        var solutionPath = await TestServices.SolutionExplorer.GetDirectoryNameAsync(ControlledHangMitigatingCancellationToken);
        var expectedProjectFileName = await TestServices.SolutionExplorer.GetAbsolutePathForProjectRelativeFilePathAsync(RazorProjectConstants.BlazorProjectName, RazorProjectConstants.ProjectFile, ControlledHangMitigatingCancellationToken);

        // Open SurveyPrompt and make sure its all up and running
        await TestServices.SolutionExplorer.OpenFileAsync(RazorProjectConstants.BlazorProjectName, RazorProjectConstants.ErrorCshtmlFile, ControlledHangMitigatingCancellationToken);
        await TestServices.Editor.WaitForSemanticClassificationAsync("class name", ControlledHangMitigatingCancellationToken, count: 1);

        await TestServices.SolutionExplorer.CloseSolutionAsync(ControlledHangMitigatingCancellationToken);

        // Clear out the project.razor.bin file which ensures our restored file will have to be in the Misc Project
        var projectRazorJsonFileName = Directory.EnumerateFiles(solutionPath, "project.razor.*.bin", SearchOption.AllDirectories).First();
        File.Delete(projectRazorJsonFileName);

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
