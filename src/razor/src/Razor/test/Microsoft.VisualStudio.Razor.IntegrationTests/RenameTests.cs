// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Microsoft.VisualStudio.Razor.IntegrationTests;

public class RenameTests : AbstractRazorEditorTest
{
    [IdeFact(Skip = "https://github.com/dotnet/razor/issues/8121")]
    public async Task Rename_ComponentAttribute_FromRazor()
    {
        // Open the file
        await TestServices.SolutionExplorer.OpenFileAsync(RazorProjectConstants.BlazorProjectName, RazorProjectConstants.IndexRazorFile, ControlledHangMitigatingCancellationToken);

        await TestServices.Editor.PlaceCaretAsync("Title=", charsOffset: -1, ControlledHangMitigatingCancellationToken);

        await TestServices.Editor.WaitForComponentClassificationAsync(ControlledHangMitigatingCancellationToken);

        await Task.Delay(500);

        // Act
        await TestServices.Editor.InvokeRenameAsync(ControlledHangMitigatingCancellationToken);
        TestServices.Input.Send("ZooperDooper{ENTER}");

        // Assert
        // The rename operation causes SurveyPrompt.razor to be opened
        await TestServices.Editor.WaitForActiveWindowByFileAsync("SurveyPrompt.razor", ControlledHangMitigatingCancellationToken);
        await TestServices.Editor.VerifyTextContainsAsync("public string? ZooperDooper { get; set; }", ControlledHangMitigatingCancellationToken);
        await TestServices.Editor.VerifyTextContainsAsync("@ZooperDooper", ControlledHangMitigatingCancellationToken);

        await TestServices.SolutionExplorer.OpenFileAsync(RazorProjectConstants.BlazorProjectName, RazorProjectConstants.IndexRazorFile, ControlledHangMitigatingCancellationToken);
        await TestServices.Editor.VerifyTextContainsAsync("<SurveyPrompt ZooperDooper=", ControlledHangMitigatingCancellationToken);
    }

    [IdeFact(Skip = "https://github.com/dotnet/razor/issues/8121")]
    public async Task Rename_ComponentAttribute_FromCSharpInRazor()
    {
        // Open the file
        await TestServices.SolutionExplorer.OpenFileAsync(RazorProjectConstants.BlazorProjectName, RazorProjectConstants.SurveyPromptFile, ControlledHangMitigatingCancellationToken);

        await TestServices.Editor.PlaceCaretAsync("Title", charsOffset: 0, occurrence: 2, extendSelection: false, selectBlock: false, ControlledHangMitigatingCancellationToken);

        await Task.Delay(1500);

        // Act
        await TestServices.Editor.InvokeRenameAsync(ControlledHangMitigatingCancellationToken);
        TestServices.Input.Send("ZooperDooper{ENTER}");

        // Assert
        // The rename operation causes Index.razor to be opened
        await TestServices.Editor.WaitForActiveWindowByFileAsync("Index.razor", ControlledHangMitigatingCancellationToken);
        await TestServices.SolutionExplorer.OpenFileAsync(RazorProjectConstants.BlazorProjectName, RazorProjectConstants.IndexRazorFile, ControlledHangMitigatingCancellationToken);
        await TestServices.Editor.VerifyTextContainsAsync("<SurveyPrompt ZooperDooper=", ControlledHangMitigatingCancellationToken);

        await TestServices.SolutionExplorer.OpenFileAsync(RazorProjectConstants.BlazorProjectName, RazorProjectConstants.SurveyPromptFile, ControlledHangMitigatingCancellationToken);
        await TestServices.Editor.VerifyTextContainsAsync("public string? ZooperDooper { get; set; }", ControlledHangMitigatingCancellationToken);
        await TestServices.Editor.VerifyTextContainsAsync("@ZooperDooper", ControlledHangMitigatingCancellationToken);
    }

    [IdeFact(Skip = "https://github.com/dotnet/razor/issues/8121")]
    public async Task Rename_ComponentAttribute_FromCSharpInCSharp()
    {
        // Create the file
        const string MyComponentRazorPath = "MyComponent.razor";
        await TestServices.SolutionExplorer.AddFileAsync(RazorProjectConstants.BlazorProjectName,
            MyComponentRazorPath,
            """
                @MyProperty
                """,
            open: true, // We create these open and then close them to try to force Component initialization while testing edits of closed documents
            cancellationToken: ControlledHangMitigatingCancellationToken);
        await TestServices.Editor.CloseCodeFileAsync(RazorProjectConstants.BlazorProjectName, MyComponentRazorPath, saveFile: true, ControlledHangMitigatingCancellationToken);

        const string MyComponentCSharpPath = "MyComponent.razor.cs";
        await TestServices.SolutionExplorer.AddFileAsync(RazorProjectConstants.BlazorProjectName,
            MyComponentCSharpPath,
            """
                namespace BlazorProject;

                public partial class MyComponent
                {
                    [Microsoft.AspNetCore.Components.ParameterAttribute]
                    public string? MyProperty { get; set; }
                }
            
                """,
            open: true,
            cancellationToken: ControlledHangMitigatingCancellationToken);
        await WaitForComponentInitializeAsync(RazorProjectConstants.BlazorProjectName, "MyComponent", ControlledHangMitigatingCancellationToken);
        await TestServices.Editor.CloseCodeFileAsync(RazorProjectConstants.BlazorProjectName, MyComponentCSharpPath, saveFile: true, ControlledHangMitigatingCancellationToken);

        await TestServices.SolutionExplorer.AddFileAsync(RazorProjectConstants.BlazorProjectName,
            "MyPage.razor",
            """
                <MyComponent MyProperty="123" />
                """,
            open: true,
            cancellationToken: ControlledHangMitigatingCancellationToken);

        await TestServices.Editor.WaitForComponentClassificationAsync(ControlledHangMitigatingCancellationToken);

        await TestServices.SolutionExplorer.OpenFileAsync(RazorProjectConstants.BlazorProjectName, "MyComponent.razor.cs", ControlledHangMitigatingCancellationToken);

        await TestServices.Editor.PlaceCaretAsync("MyProperty", charsOffset: 0, occurrence: 2, extendSelection: false, selectBlock: false, ControlledHangMitigatingCancellationToken);

        // Act
        await TestServices.Editor.InvokeRenameAsync(ControlledHangMitigatingCancellationToken);
        TestServices.Input.Send("ZooperDooper{ENTER}");

        // Assert
        // The rename operation causes MyPage.razor to be opened
        await TestServices.Editor.WaitForActiveWindowByFileAsync("MyComponent.razor.cs", ControlledHangMitigatingCancellationToken);
        await TestServices.Editor.VerifyTextContainsAsync("public string? ZooperDooper { get; set; }", ControlledHangMitigatingCancellationToken);

        await TestServices.SolutionExplorer.OpenFileAsync(RazorProjectConstants.BlazorProjectName, "MyPage.razor", ControlledHangMitigatingCancellationToken);
        await TestServices.Editor.VerifyTextContainsAsync("<MyComponent ZooperDooper=", ControlledHangMitigatingCancellationToken);

        await TestServices.SolutionExplorer.OpenFileAsync(RazorProjectConstants.BlazorProjectName, "MyComponent.razor", ControlledHangMitigatingCancellationToken);
        await TestServices.Editor.VerifyTextContainsAsync("@ZooperDooper", ControlledHangMitigatingCancellationToken);
    }

    [IdeFact(Skip = "https://github.com/dotnet/razor/issues/8121")]
    public async Task Rename_ComponentAttribute_BoundAttribute()
    {
        // Create the files
        const string MyComponentPath = "MyComponent.razor";
        await TestServices.SolutionExplorer.AddFileAsync(RazorProjectConstants.BlazorProjectName,
            MyComponentPath,
            """
            <div></div>

            @code
            {
                [Parameter]
                public string? Value { get; set; }

                [Parameter]
                public EventCallback<string?> ValueChanged { get; set; }
            }
            """,
            open: true,
            cancellationToken: ControlledHangMitigatingCancellationToken);

        await WaitForComponentInitializeAsync(RazorProjectConstants.BlazorProjectName, "MyComponent", ControlledHangMitigatingCancellationToken);
        await TestServices.Editor.CloseCodeFileAsync(RazorProjectConstants.BlazorProjectName, MyComponentPath, saveFile: true, ControlledHangMitigatingCancellationToken);

        await TestServices.SolutionExplorer.AddFileAsync(RazorProjectConstants.BlazorProjectName,
            "MyPage.razor",
            """
            <MyComponent @bind-Value="value"></MyComponent>

            @code{
                string? value = "";
            }
            """,
            open: true,
            cancellationToken: ControlledHangMitigatingCancellationToken);

        await TestServices.Editor.WaitForComponentClassificationAsync(ControlledHangMitigatingCancellationToken);
        await TestServices.Editor.WaitForOutlineRegionsAsync(ControlledHangMitigatingCancellationToken);

        await TestServices.Editor.PlaceCaretAsync("Value=", charsOffset: -1, ControlledHangMitigatingCancellationToken);

        // Act
        await TestServices.Editor.InvokeRenameAsync(ControlledHangMitigatingCancellationToken);
        TestServices.Input.Send("ZooperDooper{ENTER}");

        // Assert
        // The rename operation causes MyPage.razor to be opened
        await TestServices.Editor.WaitForActiveWindowByFileAsync("MyComponent.razor", ControlledHangMitigatingCancellationToken);
        await TestServices.Editor.VerifyTextContainsAsync("public string? ZooperDooper { get; set; }", ControlledHangMitigatingCancellationToken);

        await TestServices.SolutionExplorer.OpenFileAsync(RazorProjectConstants.BlazorProjectName, "MyPage.razor", ControlledHangMitigatingCancellationToken);
        await TestServices.Editor.VerifyTextContainsAsync("<MyComponent @bind-ZooperDooper=\"value\"></MyComponent>", ControlledHangMitigatingCancellationToken);
    }

    private async Task WaitForComponentInitializeAsync(string projectName, string componentName, CancellationToken cancellationToken)
    {
        // Wait for it to initialize by building
        await TestServices.SolutionExplorer.WaitForComponentAsync(projectName, componentName, cancellationToken);
    }
}
