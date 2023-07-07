// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Razor.IntegrationTests.InProcess;
using Xunit;

namespace Microsoft.VisualStudio.Razor.IntegrationTests;

public class ProjectTests : AbstractRazorEditorTest
{
    [IdeFact]
    public async Task CreateFromTemplateAsync()
    {
        await TestServices.SolutionExplorer.OpenFileAsync(RazorProjectConstants.BlazorProjectName, RazorProjectConstants.CounterRazorFile, ControlledHangMitigatingCancellationToken);
        await TestServices.SolutionExplorer.CloseSolutionAsync(ControlledHangMitigatingCancellationToken);
    }

    [IdeFact]
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
        await TestServices.Workspace.WaitForAsyncOperationsAsync(FeatureAttribute.LanguageServer, ControlledHangMitigatingCancellationToken);
        await TestServices.Workspace.WaitForAsyncOperationsAsync(FeatureAttribute.Workspace, ControlledHangMitigatingCancellationToken);

        // We open the Index.razor file, and wait for 3 RazorComponentElement's to be classified, as that
        // way we know the LSP server is up, running, and has processed both local and library-sourced Components
        await TestServices.SolutionExplorer.OpenFileAsync(RazorProjectConstants.BlazorProjectName, RazorProjectConstants.IndexRazorFile, ControlledHangMitigatingCancellationToken);

        // Razor extension doesn't launch until a razor file is opened, so wait for it to equalize
        await TestServices.Workspace.WaitForAsyncOperationsAsync(FeatureAttribute.LanguageServer, ControlledHangMitigatingCancellationToken);
        await TestServices.Workspace.WaitForAsyncOperationsAsync(FeatureAttribute.Workspace, ControlledHangMitigatingCancellationToken);
        await TestServices.Workspace.WaitForProjectSystemAsync(ControlledHangMitigatingCancellationToken);

        await TestServices.Editor.PlaceCaretAsync("</PageTitle>", charsOffset: 1, ControlledHangMitigatingCancellationToken);
        await TestServices.Editor.WaitForComponentClassificationAsync(ControlledHangMitigatingCancellationToken, count: 3);

        // This is a little odd, but there is no "real" way to check this via VS, and one of the most important things this test can do
        // is ensure that each target framework gets its own project.razor.json file, and doesn't share one from a cache or anything.
        Assert.Equal(2, GetProjectRazorJsonFileCount());

        int GetProjectRazorJsonFileCount()
            => Directory.EnumerateFiles(solutionPath, "project.razor.*.json", SearchOption.AllDirectories).Count();
    }
}
