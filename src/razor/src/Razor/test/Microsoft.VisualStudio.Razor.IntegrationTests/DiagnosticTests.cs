// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Xunit;

namespace Microsoft.VisualStudio.Razor.IntegrationTests;

public class DiagnosticTests : AbstractRazorEditorTest
{
    [IdeFact]
    public async Task Diagnostics_ShowErrors()
    {
        // Arrange
        await TestServices.SolutionExplorer.OpenFileAsync(RazorProjectConstants.BlazorProjectName, RazorProjectConstants.CounterRazorFile, ControlledHangMitigatingCancellationToken);

        await TestServices.Editor.SetTextAsync(@"
<h1>
<PageTitle>

@code{
    public void Function(){
        return """"
    }
}
", ControlledHangMitigatingCancellationToken);

        // Act
        var errors = await TestServices.ErrorList.WaitForErrorsAsync("Counter.razor", expectedCount: 3, ControlledHangMitigatingCancellationToken);

        // Assert
        Assert.Collection(errors,
            (error) =>
            {
                Assert.Equal("Counter.razor(2, 1): error RZ9980: Unclosed tag 'h1' with no matching end tag.", error);
            },
            (error) =>
            {
                Assert.Equal("Counter.razor(3, 2): error RZ1034: Found a malformed 'PageTitle' tag helper. Tag helpers must have a start and end tag or be self closing.", error);
            },
            (error) =>
            {
                Assert.Equal("Counter.razor(7, 18): error CS1002: ; expected", error);
            },
            (error) =>
            {
                Assert.Equal("Counter.razor(7, 9): error CS0127: Since 'Counter.Function()' returns void, a return keyword must not be followed by an object expression", error);
            });
    }
}
