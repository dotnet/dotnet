// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WebTools.Languages.Shared.Editor.EditorHelpers;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.Razor.IntegrationTests;

public class RCLStressTests(ITestOutputHelper testOutputHelper) : AbstractStressTest(testOutputHelper)
{
    protected override string TargetFramework => "net9.0";

    protected override string ProjectZipFile => "Microsoft.VisualStudio.Razor.IntegrationTests.TestFiles.BlazorProjectWithRCL.zip";

    [ManualRunOnlyIdeFact]
    public async Task AddAndRemoveComponentInRCL()
    {
        await TestServices.SolutionExplorer.OpenFileAsync("RazorClassLibrary", @"Components\RCLComponent.razor", ControlledHangMitigatingCancellationToken);

        await TestServices.Editor.PlaceCaretAsync("<div", charsOffset: -1, ControlledHangMitigatingCancellationToken);

        await RunStressTestAsync(RunIterationAsync);

        async Task RunIterationAsync(int index, CancellationToken cancellationToken)
        {
            await TestServices.Editor.InsertTextAsync($"<h1>Iteration {index}</h1>{Environment.NewLine}", cancellationToken);

            await TestServices.Editor.PlaceCaretAsync("h1", charsOffset: -1, cancellationToken);

            await TestServices.Editor.WaitForComponentClassificationAsync(cancellationToken, count: 1, exact: true);

            await TestServices.Editor.InvokeCodeActionAsync("Extract element to new component", cancellationToken);

            await TestServices.Editor.WaitForActiveWindowByFileAsync("Component.razor", cancellationToken);

            await TestServices.Editor.PlaceCaretAsync("<h1", charsOffset: -1, cancellationToken);

            // TODO: Remove once https://github.com/dotnet/razor/issues/11478 is fixed
            await TestServices.Editor.InsertTextAsync($"@namespace MyCoolNamespace{Environment.NewLine}{Environment.NewLine}", cancellationToken);

            var componentFileName = (await TestServices.Editor.GetActiveTextViewAsync(cancellationToken)).TextBuffer.GetFileName();

            await TestServices.Editor.CloseCurrentlyFocusedWindowAsync(cancellationToken, save: true);

            await TestServices.Editor.WaitForActiveWindowByFileAsync("RCLComponent.razor", cancellationToken);

            await TestServices.Editor.WaitForComponentClassificationAsync(cancellationToken, count: 2, exact: true);

            await TestServices.SolutionExplorer.OpenFileAsync(RazorProjectConstants.BlazorProjectName, RazorProjectConstants.IndexRazorFile, ControlledHangMitigatingCancellationToken);

            await TestServices.Editor.PlaceCaretAsync("h1", charsOffset: -1, cancellationToken);

            await TestServices.Editor.InvokeDeleteLineAsync(cancellationToken);

            await TestServices.Editor.InsertTextAsync($"<Component />{Environment.NewLine}", cancellationToken);

            await TestServices.Editor.WaitForComponentClassificationAsync(cancellationToken, count: 5, exact: true);

            File.Delete(componentFileName);

            await TestServices.Editor.WaitForComponentClassificationAsync(cancellationToken, count: 4, exact: true);

            await TestServices.Editor.PlaceCaretAsync("<Component />", charsOffset: -1, cancellationToken);

            await TestServices.Editor.InvokeDeleteLineAsync(cancellationToken);

            await TestServices.Editor.InsertTextAsync($"<h1>Iteration {index}</h1>{Environment.NewLine}", cancellationToken);

            await TestServices.SolutionExplorer.OpenFileAsync("RazorClassLibrary", @"Components\RCLComponent.razor", ControlledHangMitigatingCancellationToken);

            await TestServices.Editor.WaitForComponentClassificationAsync(cancellationToken, count: 1, exact: true);

            await TestServices.Editor.PlaceCaretAsync("<Component />", charsOffset: -1, cancellationToken);

            await TestServices.Editor.InvokeDeleteLineAsync(cancellationToken);
        }
    }
}
