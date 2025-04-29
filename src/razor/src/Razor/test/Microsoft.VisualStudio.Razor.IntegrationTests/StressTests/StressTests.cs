// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WebTools.Languages.Shared.Editor.EditorHelpers;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.Razor.IntegrationTests;

public class StressTests(ITestOutputHelper testOutputHelper) : AbstractStressTest(testOutputHelper)
{
    [ManualRunOnlyIdeFact]
    public async Task AddAndRemoveComponent()
    {
        await TestServices.SolutionExplorer.OpenFileAsync(RazorProjectConstants.BlazorProjectName, RazorProjectConstants.CounterRazorFile, ControlledHangMitigatingCancellationToken);

        await TestServices.Editor.PlaceCaretAsync("h1", charsOffset: -1, ControlledHangMitigatingCancellationToken);

        await TestServices.Editor.InvokeDeleteLineAsync(ControlledHangMitigatingCancellationToken);

        await RunStressTestAsync(RunIterationAsync);

        async Task RunIterationAsync(int index, CancellationToken cancellationToken)
        {
            await TestServices.Editor.InsertTextAsync($"<h1>Iteration {index}</h1>{Environment.NewLine}", cancellationToken);

            await TestServices.Editor.PlaceCaretAsync("h1", charsOffset: -1, cancellationToken);

            await TestServices.Editor.WaitForComponentClassificationAsync(cancellationToken, count: 2, exact: true);

            await TestServices.Editor.InvokeCodeActionAsync("Extract element to new component", cancellationToken);

            await TestServices.Editor.WaitForActiveWindowByFileAsync("Component.razor", cancellationToken);

            var componentFileName = (await TestServices.Editor.GetActiveTextViewAsync(cancellationToken)).TextBuffer.GetFileName();

            await TestServices.Editor.CloseCurrentlyFocusedWindowAsync(cancellationToken, save: true);

            await TestServices.Editor.WaitForActiveWindowByFileAsync("Counter.razor", cancellationToken);

            await TestServices.Editor.WaitForComponentClassificationAsync(cancellationToken, count: 3, exact: true);

            await Task.Delay(500);

            File.Delete(componentFileName);

            await TestServices.Editor.WaitForComponentClassificationAsync(cancellationToken, count: 2, exact: true);

            await Task.Delay(500);

            await TestServices.Editor.PlaceCaretAsync("Component", charsOffset: -1, cancellationToken);

            await TestServices.Editor.InvokeDeleteLineAsync(cancellationToken);
        }
    }
}
