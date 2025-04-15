// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.Razor.IntegrationTests;

public class TaskListTests(ITestOutputHelper testOutputHelper) : AbstractRazorEditorTest(testOutputHelper)
{
    [IdeFact]
    public async Task ShowsTasks()
    {
        // Arrange
        await TestServices.SolutionExplorer.OpenFileAsync(RazorProjectConstants.BlazorProjectName, RazorProjectConstants.CounterRazorFile, ControlledHangMitigatingCancellationToken);

        await TestServices.Editor.SetTextAsync("""
            <PageTitle>Title</PageTitle>

            @* TODO: Fill in more content *@

            @code
            {
                // TODO: Fill in more code
            }
            """, ControlledHangMitigatingCancellationToken);

        var tasks = await TestServices.TaskList.WaitForTasksAsync(expectedCount: 2, ControlledHangMitigatingCancellationToken);

        Assert.NotNull(tasks);
        Assert.Collection(tasks.OrderAsArray(),
            static task => Assert.Contains("TODO: Fill in more code", task),
            static task => Assert.Contains("TODO: Fill in more content", task));
    }
}
