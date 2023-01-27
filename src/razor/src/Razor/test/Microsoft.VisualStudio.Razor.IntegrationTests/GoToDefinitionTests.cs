// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Xunit;

namespace Microsoft.VisualStudio.Razor.IntegrationTests;

public class GoToDefinitionTests : AbstractRazorEditorTest
{
    [IdeFact]
    public async Task GoToDefinition_MethodInSameFile()
    {
        // Open the file
        await TestServices.SolutionExplorer.OpenFileAsync(RazorProjectConstants.BlazorProjectName, RazorProjectConstants.CounterRazorFile, ControlledHangMitigatingCancellationToken);

        await TestServices.Editor.PlaceCaretAsync("IncrementCount", charsOffset: -1, ControlledHangMitigatingCancellationToken);

        // Act
        await TestServices.Editor.InvokeGoToDefinitionAsync(ControlledHangMitigatingCancellationToken);

        // Assert
        await TestServices.Editor.WaitForCurrentLineTextAsync("private void IncrementCount()", ControlledHangMitigatingCancellationToken);
    }

    [IdeFact]
    public async Task GoToDefinition_CSharpClass()
    {
        // Open the file
        await TestServices.SolutionExplorer.OpenFileAsync(RazorProjectConstants.BlazorProjectName, RazorProjectConstants.IndexRazorFile, ControlledHangMitigatingCancellationToken);

        // Change text to refer back to Program class
        await TestServices.Editor.SetTextAsync(@"<SurveyPrompt Title=""@nameof(Program)", ControlledHangMitigatingCancellationToken);
        await TestServices.Editor.PlaceCaretAsync("Program", charsOffset: -1, ControlledHangMitigatingCancellationToken);

        // Act
        await TestServices.Editor.InvokeGoToDefinitionAsync(ControlledHangMitigatingCancellationToken);

        // Assert
        await TestServices.Editor.WaitForActiveWindowAsync("Program.cs", ControlledHangMitigatingCancellationToken);
    }

    [IdeFact]
    public async Task GoToDefinition_Component()
    {
        // Open the file
        await TestServices.SolutionExplorer.OpenFileAsync(RazorProjectConstants.BlazorProjectName, RazorProjectConstants.IndexRazorFile, ControlledHangMitigatingCancellationToken);

        await TestServices.Editor.PlaceCaretAsync("SurveyPrompt", charsOffset: -1, ControlledHangMitigatingCancellationToken);

        // Act
        await TestServices.Editor.InvokeGoToDefinitionAsync(ControlledHangMitigatingCancellationToken);

        // Assert
        await TestServices.Editor.WaitForActiveWindowAsync("SurveyPrompt.razor", ControlledHangMitigatingCancellationToken);
    }

    [IdeFact]
    public async Task GoToDefinition_ComponentAttribute_InOtherRazorFile()
    {
        // Open the file
        await TestServices.SolutionExplorer.OpenFileAsync(RazorProjectConstants.BlazorProjectName, RazorProjectConstants.IndexRazorFile, ControlledHangMitigatingCancellationToken);

        await TestServices.Editor.PlaceCaretAsync("Title=", charsOffset: -1, ControlledHangMitigatingCancellationToken);

        // Act
        await TestServices.Editor.InvokeGoToDefinitionAsync(ControlledHangMitigatingCancellationToken);

        // Assert
        await TestServices.Editor.WaitForActiveWindowAsync("SurveyPrompt.razor", ControlledHangMitigatingCancellationToken);
        await TestServices.Editor.WaitForCurrentLineTextAsync("public string? Title { get; set; }", ControlledHangMitigatingCancellationToken);
    }

    [IdeFact]
    public async Task GoToDefinition_ComponentAttribute_InSameRazorFile()
    {
        // Create the file
        await TestServices.SolutionExplorer.AddFileAsync(RazorProjectConstants.BlazorProjectName,
            "MyComponent.razor",
            """
            <MyComponent MyProperty="123" />

            @code {
                [Microsoft.AspNetCore.Components.ParameterAttribute]
                public string? MyProperty { get; set; }
            }
            
            """,
            open: true,
            cancellationToken: ControlledHangMitigatingCancellationToken);

        await TestServices.Editor.WaitForComponentClassificationAsync(ControlledHangMitigatingCancellationToken);

        await TestServices.Editor.PlaceCaretAsync("MyProperty=", charsOffset: -1, ControlledHangMitigatingCancellationToken);

        // Act
        await TestServices.Editor.InvokeGoToDefinitionAsync(ControlledHangMitigatingCancellationToken);

        // Assert
        await TestServices.Editor.WaitForActiveWindowAsync("MyComponent.razor", ControlledHangMitigatingCancellationToken);
        await TestServices.Editor.WaitForCurrentLineTextAsync("public string? MyProperty { get; set; }", ControlledHangMitigatingCancellationToken);
    }

    [IdeFact]
    public async Task GoToDefinition_ComponentAttribute_Minimized()
    {
        // Create the file
        await TestServices.SolutionExplorer.AddFileAsync(RazorProjectConstants.BlazorProjectName,
            "MyComponent.razor",
            """
            <MyComponent MyProperty />

            @code {
                [Microsoft.AspNetCore.Components.ParameterAttribute]
                public bool MyProperty { get; set; }
            }
            
            """,
            open: true,
            cancellationToken: ControlledHangMitigatingCancellationToken);

        await TestServices.Editor.WaitForComponentClassificationAsync(ControlledHangMitigatingCancellationToken);

        await TestServices.Editor.PlaceCaretAsync("MyProperty", charsOffset: -1, ControlledHangMitigatingCancellationToken);

        // Act
        await TestServices.Editor.InvokeGoToDefinitionAsync(ControlledHangMitigatingCancellationToken);

        // Assert
        await TestServices.Editor.WaitForActiveWindowAsync("MyComponent.razor", ControlledHangMitigatingCancellationToken);
        await TestServices.Editor.WaitForCurrentLineTextAsync("public bool MyProperty { get; set; }", ControlledHangMitigatingCancellationToken);
    }

    [IdeFact]
    public async Task GoToDefinition_ComponentAttribute_InCSharpFile()
    {
        // Create the files
        const string MyComponentPath = "MyComponent.cs";
        const string MyPagePath = "MyPage.razor";

        await TestServices.SolutionExplorer.AddFileAsync(RazorProjectConstants.BlazorProjectName,
            MyComponentPath,
            """
            using Microsoft.AspNetCore.Components;
            
            namespace BlazorProject;

            public class MyComponent : ComponentBase
            {
                [Parameter] public string? MyProperty { get; set; }
            }
            """,
            cancellationToken: ControlledHangMitigatingCancellationToken);

        await TestServices.SolutionExplorer.BuildSolutionAndWaitAsync(ControlledHangMitigatingCancellationToken);
        await TestServices.Workspace.WaitForProjectSystemAsync(ControlledHangMitigatingCancellationToken);

        await TestServices.SolutionExplorer.AddFileAsync(RazorProjectConstants.BlazorProjectName,
            MyPagePath,
            """
            <MyComponent MyProperty="123" />
            """,
            open: true,
            cancellationToken: ControlledHangMitigatingCancellationToken);

        await TestServices.Editor.WaitForComponentClassificationAsync(ControlledHangMitigatingCancellationToken);

        await TestServices.Editor.PlaceCaretAsync("MyProperty=", charsOffset: -1, ControlledHangMitigatingCancellationToken);

        // Act
        await TestServices.Editor.InvokeGoToDefinitionAsync(ControlledHangMitigatingCancellationToken);

        // Assert
        await TestServices.Editor.WaitForActiveWindowAsync(MyComponentPath, ControlledHangMitigatingCancellationToken);
        await TestServices.Editor.WaitForCurrentLineTextAsync("[Parameter] public string? MyProperty { get; set; }", ControlledHangMitigatingCancellationToken);
    }

    [IdeFact]
    public async Task GoToDefinition_ComponentAttribute_InReferencedAssembly()
    {
        // Open the file
        await TestServices.SolutionExplorer.OpenFileAsync(RazorProjectConstants.BlazorProjectName, RazorProjectConstants.NavMenuFile, ControlledHangMitigatingCancellationToken);

        await TestServices.Editor.PlaceCaretAsync("Match=", charsOffset: -1, ControlledHangMitigatingCancellationToken);

        // Act
        await TestServices.Editor.InvokeGoToDefinitionAsync(ControlledHangMitigatingCancellationToken);

        // Assert
        await TestServices.Editor.WaitForActiveWindowByFileAsync("NavLink.cs", ControlledHangMitigatingCancellationToken);
    }

    [IdeFact(Skip = "Blocked by https://github.com/dotnet/razor/issues/7966")]
    public async Task GoToDefinition_ComponentAttribute_GenericComponent()
    {
        // Create the files
        await TestServices.SolutionExplorer.AddFileAsync(RazorProjectConstants.BlazorProjectName,
            "MyComponent.cs",
            """
            using Microsoft.AspNetCore.Components;
            
            namespace BlazorProject
            {
                public class MyComponent<TItem> : ComponentBase
                {
                    [Parameter] public TItem Item { get; set; }
                }
            }
            """,
            cancellationToken: ControlledHangMitigatingCancellationToken);

        await TestServices.SolutionExplorer.AddFileAsync(RazorProjectConstants.BlazorProjectName,
            "MyPage.razor",
            """
            <MyComponent TItem=string Item="@("hi")"/>
            """,
            open: true,
            cancellationToken: ControlledHangMitigatingCancellationToken);

        await TestServices.Editor.WaitForComponentClassificationAsync(ControlledHangMitigatingCancellationToken);

        await TestServices.Editor.PlaceCaretAsync(" Item=", charsOffset: -1, ControlledHangMitigatingCancellationToken);

        // Act
        await TestServices.Editor.InvokeGoToDefinitionAsync(ControlledHangMitigatingCancellationToken);

        // Assert
        await TestServices.Editor.WaitForActiveWindowAsync("MyComponent.cs", ControlledHangMitigatingCancellationToken);
        await TestServices.Editor.WaitForCurrentLineTextAsync("[Parameter] public TItem Item { get; set; }", ControlledHangMitigatingCancellationToken);
    }

    [IdeFact(Skip = "Blocked by https://github.com/dotnet/razor/issues/7966")]
    public async Task GoToDefinition_ComponentAttribute_CascadingGenericComponentWithConstraints()
    {
        // Create the files
        await TestServices.SolutionExplorer.AddFileAsync(RazorProjectConstants.BlazorProjectName,
            "MyComponent.cs",
            """
            using Microsoft.AspNetCore.Components;
            
            namespace BlazorProject
            {
                [CascadingTypeParameter(nameof(TItem))]
                public class Grid<TItem> : ComponentBase
                {
                    [Parameter] public RenderFragment ColumnsTemplate { get; set; }
                }
            
                public abstract partial class BaseColumn<TItem> : ComponentBase where TItem : class
                {
                    [CascadingParameter]
                    internal Grid<TItem> Grid { get; set; }
                }
            
                public class Column<TItem> : BaseColumn<TItem>, IGridFieldColumn<TItem> where TItem : class
                {
                    [Parameter]
                    public string FieldName { get; set; }
                }
            
                internal interface IGridFieldColumn<TItem> where TItem : class
                {
                }
            
                public class WeatherForecast { }
            }
            """,
            cancellationToken: ControlledHangMitigatingCancellationToken);

        await TestServices.SolutionExplorer.AddFileAsync(RazorProjectConstants.BlazorProjectName,
            "MyPage.razor",
            """
            <Grid TItem="WeatherForecast" Items="@(Array.Empty<WeatherForecast>())">
                <ColumnsTemplate>
                    <Column Title="Date" FieldName="Date" Format="d" Width="10rem" />
                </ColumnsTemplate>
            </Grid>
            """,
            open: true,
            cancellationToken: ControlledHangMitigatingCancellationToken);

        await TestServices.Editor.WaitForComponentClassificationAsync(ControlledHangMitigatingCancellationToken);

        await TestServices.Editor.PlaceCaretAsync(" FieldName=", charsOffset: -1, ControlledHangMitigatingCancellationToken);

        // Act
        await TestServices.Editor.InvokeGoToDefinitionAsync(ControlledHangMitigatingCancellationToken);

        // Assert
        await TestServices.Editor.WaitForActiveWindowAsync("MyComponent.cs", ControlledHangMitigatingCancellationToken);
        await TestServices.Editor.WaitForCurrentLineTextAsync("public string FieldName { get; set; }", ControlledHangMitigatingCancellationToken);
    }

    [IdeFact]
    public async Task GoToDefinition_ComponentAttribute_BoundAttribute()
    {
        // Create the files
        await TestServices.SolutionExplorer.AddFileAsync(RazorProjectConstants.BlazorProjectName,
            "MyComponent.razor",
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
            cancellationToken: ControlledHangMitigatingCancellationToken);

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

        await TestServices.Editor.PlaceCaretAsync("Value=", charsOffset: -1, ControlledHangMitigatingCancellationToken);

        // Act
        await TestServices.Editor.InvokeGoToDefinitionAsync(ControlledHangMitigatingCancellationToken);

        // Assert
        await TestServices.Editor.WaitForActiveWindowAsync("MyComponent.razor", ControlledHangMitigatingCancellationToken);
        await TestServices.Editor.WaitForCurrentLineTextAsync("public string? Value { get; set; }", ControlledHangMitigatingCancellationToken);
    }
}
