// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Xunit;

namespace Microsoft.VisualStudio.Razor.IntegrationTests;

public class HoverTests : AbstractRazorEditorTest
{
    [IdeFact]
    public async Task Hover_OverTagHelperElementAsync()
    {
        // Open the file
        await TestServices.SolutionExplorer.OpenFileAsync(RazorProjectConstants.BlazorProjectName, RazorProjectConstants.CounterRazorFile, ControlledHangMitigatingCancellationToken);

        await TestServices.Editor.PlaceCaretAsync("PageTitle", charsOffset: -1, ControlledHangMitigatingCancellationToken);

        var position = await TestServices.Editor.GetCaretPositionAsync(ControlledHangMitigatingCancellationToken);

        // Act
        var hoverString = await TestServices.Editor.GetHoverStringAsync(position, ControlledHangMitigatingCancellationToken);

        // Assert
        const string ExpectedResult = "Microsoft.AspNetCore.Components.Web.PageTitleEnables rendering an HTML <title> to a HeadOutlet component.";
        Assert.Equal(ExpectedResult, hoverString);
    }
}
