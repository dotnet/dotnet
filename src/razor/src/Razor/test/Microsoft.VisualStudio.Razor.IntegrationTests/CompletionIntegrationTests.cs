// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Threading.Tasks;
using Roslyn.Test.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.Razor.IntegrationTests;

public class CompletionIntegrationTests(ITestOutputHelper testOutputHelper) : AbstractRazorEditorTest(testOutputHelper)
{
    private static readonly TimeSpan SnippetTimeout = TimeSpan.FromSeconds(10);

    [IdeFact]
    public async Task SnippetCompletion_Html()
    {
        await VerifyTypeAndCommitCompletionAsync(
            input: """
                @page "Test"

                <PageTitle>Test</PageTitle>

                <h1>Test</h1>

                @code {
                    private int currentCount = 0;

                    private void IncrementCount()
                    {
                        currentCount++;
                    }
                }
                """,
            output: """
                @page "Test"

                <PageTitle>Test</PageTitle>

                <h1>Test</h1>
                <dl>
                    <dt></dt>
                    <dd></dd>
                </dl>

                @code {
                    private int currentCount = 0;

                    private void IncrementCount()
                    {
                        currentCount++;
                    }
                }
                """,
            search: "<h1>Test</h1>",
            stringsToType: ["{ENTER}", "d", "d"]);
    }

    [IdeFact, WorkItem("https://github.com/dotnet/razor/issues/10787")]
    public async Task CompletionCommit_HtmlAttributeWithoutValue()
    {
        await VerifyTypeAndCommitCompletionAsync(
            input: """
                @page "Test"

                <PageTitle>Test</PageTitle>

                <button></button>

                @code {
                    private int currentCount = 0;

                    private void IncrementCount()
                    {
                        currentCount++;
                    }
                }
                """,
            output: """
                @page "Test"

                <PageTitle>Test</PageTitle>

                <button disabled></button>

                @code {
                    private int currentCount = 0;

                    private void IncrementCount()
                    {
                        currentCount++;
                    }
                }
                """,
            search: "<button",
            stringsToType: [" ", "d", "i", "s"]);
    }

    [IdeFact]
    public async Task CompletionCommit_HtmlAttributeWithValue()
    {
        await VerifyTypeAndCommitCompletionAsync(
            input: """
                @page "Test"

                <PageTitle>Test</PageTitle>

                <button></button>

                @code {
                    private int currentCount = 0;

                    private void IncrementCount()
                    {
                        currentCount++;
                    }
                }
                """,
            output: """
                @page "Test"

                <PageTitle>Test</PageTitle>

                <button style=""></button>

                @code {
                    private int currentCount = 0;

                    private void IncrementCount()
                    {
                        currentCount++;
                    }
                }
                """,
            search: "<button",
            stringsToType: [" ", "s", "t", "y"]);
    }

    [IdeFact]
    public async Task CompletionCommit_HtmlTag()
    {
        await VerifyTypeAndCommitCompletionAsync(
            input: """
                @page "Test"

                <PageTitle>Test</PageTitle>

                @code {
                    private int currentCount = 0;

                    private void IncrementCount()
                    {
                        currentCount++;
                    }
                }
                """,
            output: """
                @page "Test"

                <PageTitle>Test</PageTitle>

                <span

                @code {
                    private int currentCount = 0;

                    private void IncrementCount()
                    {
                        currentCount++;
                    }
                }
                """,
            search: "</PageTitle>",
            stringsToType: ["{ENTER}", "{ENTER}", "<", "s", "p", "a"]);
    }

    [IdeFact]
    public async Task CompletionCommit_WithAngleBracket_HtmlTag()
    {
        await VerifyTypeAndCommitCompletionAsync(
            input: """
                @page "Test"

                <PageTitle>Test</PageTitle>

                @code {
                    private int currentCount = 0;

                    private void IncrementCount()
                    {
                        currentCount++;
                    }
                }
                """,
            output: """
                @page "Test"

                <PageTitle>Test</PageTitle>

                <span></span>

                @code {
                    private int currentCount = 0;

                    private void IncrementCount()
                    {
                        currentCount++;
                    }
                }
                """,
            search: "</PageTitle>",
            stringsToType: ["{ENTER}", "{ENTER}", "<", "s", "p", "a"],
            commitChar: '>');
    }

    [IdeFact]
    public async Task CompletionCommit_CSharp()
    {
        await VerifyTypeAndCommitCompletionAsync(
            input: """
                @page "Test"

                <PageTitle>Test</PageTitle>

                @code {
                    private int myCurrentCount = 0;

                    private void IncrementCount()
                    {
                        myCurrentCount++;
                    }
                }
                """,
            output: """
                @page "Test"

                <PageTitle>Test</PageTitle>

                @code {
                    private int myCurrentCount = 0;

                    private void IncrementCount()
                    {
                        myCurrentCount++;

                        myCurrentCount
                    }
                }
                """,
            search: "myCurrentCount++;",
            stringsToType: ["{ENTER}", "{ENTER}", "m", "y", "C", "u", "r"]);
    }

    private async Task VerifyTypeAndCommitCompletionAsync(string input, string output, string search, string[] stringsToType, char? commitChar = null)
    {
        await TestServices.SolutionExplorer.AddFileAsync(
            RazorProjectConstants.BlazorProjectName,
            "Test.razor",
            input,
            open: true,
            ControlledHangMitigatingCancellationToken);

        await TestServices.Editor.WaitForComponentClassificationAsync(ControlledHangMitigatingCancellationToken);

        await TestServices.Editor.PlaceCaretAsync(search, charsOffset: 1, ControlledHangMitigatingCancellationToken);
        foreach (var stringToType in stringsToType)
        {
            TestServices.Input.Send(stringToType);
        }

        await CommitCompletionAndVerifyAsync(output, commitChar);
    }

    [IdeFact]
    public async Task SnippetCompletion_DoesntCommitOnSpace()
    {
        await TestServices.SolutionExplorer.AddFileAsync(
            RazorProjectConstants.BlazorProjectName,
            "Test.razor",
            """
            @page "Test"

            <PageTitle>Test</PageTitle>

            <div></div>
            """,
            open: true,
            ControlledHangMitigatingCancellationToken);

        var textView = await TestServices.Editor.GetActiveTextViewAsync(HangMitigatingCancellationToken);
        await TestServices.Editor.WaitForComponentClassificationAsync(ControlledHangMitigatingCancellationToken);

        await TestServices.Editor.PlaceCaretAsync("<div></div>", charsOffset: 1, ControlledHangMitigatingCancellationToken);
        TestServices.Input.Send("{ENTER}");
        TestServices.Input.Send("i");
        TestServices.Input.Send("f");
        TestServices.Input.Send("r");

        // Wait until completion comes up before validating
        // that space does not commit
        await TestServices.Editor.WaitForCompletionSessionAsync(HangMitigatingCancellationToken);

        TestServices.Input.Send(" ");

        var text = textView.TextBuffer.CurrentSnapshot.GetText();

        var expected = """
            @page "Test"
            
            <PageTitle>Test</PageTitle>
            
            <div></div>
            ifr
            """;

        AssertEx.EqualOrDiff(expected, text);
    }

    [IdeFact]
    [WorkItem("https://github.com/dotnet/razor/issues/9427")]
    public async Task Snippets_DoNotTrigger_OnDelete()
    {
        await TestServices.SolutionExplorer.AddFileAsync(
            RazorProjectConstants.BlazorProjectName,
            "Test.razor",
            """
            @page "Test"

            <PageTitle>Test</PageTitle>

            <div>Hello</div>
            """,
            open: true,
            ControlledHangMitigatingCancellationToken);

        var textView = await TestServices.Editor.GetActiveTextViewAsync(HangMitigatingCancellationToken);
        await TestServices.Editor.WaitForComponentClassificationAsync(ControlledHangMitigatingCancellationToken);

        await TestServices.Editor.PlaceCaretAsync("Hel", charsOffset: 1, ControlledHangMitigatingCancellationToken);
        TestServices.Input.Send("{DELETE}");

        // Make sure completion doesn't come up for 15 seconds
        var completionSession = await TestServices.Editor.WaitForCompletionSessionAsync(SnippetTimeout, HangMitigatingCancellationToken);
        Assert.Null(completionSession);
    }

    [IdeTheory]
    [InlineData("<PageTitle")]
    [InlineData("</PageTitle")]
    [InlineData("<div")]
    [InlineData("</div")]
    [WorkItem("https://github.com/dotnet/razor/issues/9427")]
    public async Task Snippets_DoNotTrigger_InsideTag(string tag)
    {
        await TestServices.SolutionExplorer.AddFileAsync(
            RazorProjectConstants.BlazorProjectName,
            "Test.razor",
            """
            @page "Test"

            <PageTitle>Test</PageTitle>

            <div></div>
            """,
            open: true,
            ControlledHangMitigatingCancellationToken);

        var textView = await TestServices.Editor.GetActiveTextViewAsync(HangMitigatingCancellationToken);
        await TestServices.Editor.WaitForComponentClassificationAsync(ControlledHangMitigatingCancellationToken);

        await TestServices.Editor.PlaceCaretAsync(tag, charsOffset: 1, ControlledHangMitigatingCancellationToken);
        TestServices.Input.Send(" ");
        TestServices.Input.Send("dd");

        // Make sure completion doesn't come up for 15 seconds
        var completionSession = await TestServices.Editor.WaitForCompletionSessionAsync(SnippetTimeout, HangMitigatingCancellationToken);
        var items = completionSession?.GetComputedItems(HangMitigatingCancellationToken);

        if (items is null)
        {
            // No items to check, we're good
            return;
        }

        Assert.DoesNotContain("dd", items.Items.Select(i => i.DisplayText));
    }

    [IdeFact, WorkItem("https://github.com/dotnet/razor/issues/9346")]
    public async Task Completion_EnumDot()
    {
        await TestServices.SolutionExplorer.AddFileAsync(
            RazorProjectConstants.BlazorProjectName,
            "Test.razor",
            """
            <Test Param="@MyEnum." />

            @code {
                [Parameter] public string Param { get; set; }

                public enum MyEnum
                {
                    One
                }
            }
            """,
            open: true,
            ControlledHangMitigatingCancellationToken);

        await TestServices.Editor.WaitForComponentClassificationAsync(ControlledHangMitigatingCancellationToken);

        await TestServices.Editor.PlaceCaretAsync("@MyEnum.", charsOffset: 1, ControlledHangMitigatingCancellationToken);

        await Task.Delay(500, HangMitigatingCancellationToken);

        TestServices.Input.Send("O");

        await CommitCompletionAndVerifyAsync("""
            <Test Param="@MyEnum.One" />
            
            @code {
                [Parameter] public string Param { get; set; }
            
                public enum MyEnum
                {
                    One
                }
            }
            """);
    }

    private async Task CommitCompletionAndVerifyAsync(string expected, char? commitChar = null)
    {
        var session = await TestServices.Editor.WaitForCompletionSessionAsync(HangMitigatingCancellationToken);

        Assert.NotNull(session);
        if (commitChar.HasValue)
        {
            // Commit using the specified commit character
            session.Commit(commitChar.Value, HangMitigatingCancellationToken);

            // session.Commit call above commits as if the commit character was typed,
            // but doesn't actually insert the character into the buffer.
            // So we still need to insert the character into the buffer ourselves.
            TestServices.Input.Send(commitChar.Value.ToString());
        }
        else
        {
            Assert.True(session.CommitIfUnique(HangMitigatingCancellationToken));
        }

        var textView = await TestServices.Editor.GetActiveTextViewAsync(HangMitigatingCancellationToken);
        var text = textView.TextBuffer.CurrentSnapshot.GetText();

        // Snippets may have slight whitespace differences due to line endings. These
        // tests allow for it as long as the content is correct
        AssertEx.AssertEqualToleratingWhitespaceDifferences(expected, text);
    }
}
