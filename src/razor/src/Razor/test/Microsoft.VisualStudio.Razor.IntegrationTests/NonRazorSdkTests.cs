// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.Razor.IntegrationTests;

public class NonRazorSdkTests(ITestOutputHelper testOutputHelper) : AbstractRazorEditorTest(testOutputHelper)
{
    protected override bool ComponentClassificationExpected => false;

    protected override void PrepareProjectForFirstOpen(string projectFileName)
    {
        var sb = new StringBuilder();
        foreach (var line in File.ReadAllLines(projectFileName))
        {
            if (line.Contains("Sdk="))
            {
                sb.AppendLine("""<Project Sdk="Microsoft.NET.Sdk">""");
            }
            else
            {
                sb.AppendLine(line);
            }
        }

        File.WriteAllText(projectFileName, sb.ToString());

        base.PrepareProjectForFirstOpen(projectFileName);
    }

    [IdeFact]
    public async Task Completion_DateTime()
    {
        // We open the Index.razor file, and wait for 3 RazorComponentElement's to be classified, as that
        // way we know the LSP server is up, running, and has processed both local and library-sourced Components
        await TestServices.SolutionExplorer.OpenFileAsync(RazorProjectConstants.BlazorProjectName, RazorProjectConstants.IndexRazorFile, ControlledHangMitigatingCancellationToken);

        await TestServices.RazorProjectSystem.WaitForLSPServerActivatedAsync(ControlledHangMitigatingCancellationToken);

        await TestServices.Workspace.WaitForProjectSystemAsync(ControlledHangMitigatingCancellationToken);

        await TestServices.Editor.PlaceCaretAsync("</PageTitle>", charsOffset: 1, ControlledHangMitigatingCancellationToken);

        await Task.Delay(500, HangMitigatingCancellationToken);

        TestServices.Input.Send("@");

        var completionSession = await TestServices.Editor.WaitForCompletionSessionAsync(HangMitigatingCancellationToken);
        var items = completionSession?.GetComputedItems(HangMitigatingCancellationToken);

        Assert.Contains("DateTime", items.AssumeNotNull().Items.Select(i => i.DisplayText));
    }
}
