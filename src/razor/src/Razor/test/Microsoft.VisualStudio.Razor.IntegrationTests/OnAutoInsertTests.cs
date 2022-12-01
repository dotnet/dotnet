// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Xunit;

namespace Microsoft.VisualStudio.Razor.IntegrationTests;

public class OnAutoInsertTests : AbstractRazorEditorTest
{
    [IdeFact]
    public async Task CSharp_DocumentationComments()
    {
        // Open the file
        await TestServices.SolutionExplorer.OpenFileAsync(RazorProjectConstants.BlazorProjectName, RazorProjectConstants.ErrorCshtmlFile, ControlledHangMitigatingCancellationToken);

        await TestServices.Editor.SetTextAsync(@"
<div>
</div>

@functions
{
    //
    public void M()
    {
    }
}

", ControlledHangMitigatingCancellationToken);
        await TestServices.Editor.PlaceCaretAsync("//", charsOffset: 1, ControlledHangMitigatingCancellationToken);

        // Act
        TestServices.Input.Send("/");

        // Assert
        await TestServices.Editor.WaitForCurrentLineTextAsync("/// ", ControlledHangMitigatingCancellationToken);
    }
}
