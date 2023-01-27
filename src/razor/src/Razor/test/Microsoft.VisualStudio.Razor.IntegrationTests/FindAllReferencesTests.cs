// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Shell.TableControl;
using Microsoft.VisualStudio.Shell.TableManager;
using Xunit;

namespace Microsoft.VisualStudio.Razor.IntegrationTests;

public class FindAllReferencesTests : AbstractRazorEditorTest
{
    [IdeFact]
    public async Task FindAllReferences_CSharpInRazor()
    {
        // Open the file
        await TestServices.SolutionExplorer.OpenFileAsync(RazorProjectConstants.BlazorProjectName, RazorProjectConstants.CounterRazorFile, ControlledHangMitigatingCancellationToken);

        await TestServices.Editor.PlaceCaretAsync("IncrementCount", charsOffset: -1, ControlledHangMitigatingCancellationToken);

        await TestServices.Editor.WaitForComponentClassificationAsync(ControlledHangMitigatingCancellationToken);

        // Act
        await TestServices.Editor.InvokeFindAllReferencesAsync(ControlledHangMitigatingCancellationToken);

        // Assert
        var results = await TestServices.FindReferencesWindow.WaitForContentsAsync(ControlledHangMitigatingCancellationToken, expected: 2);

        var orderedResults = OrderResults(results);

        Assert.Collection(
            orderedResults,
            new Action<TableEntry>[]
            {
                reference =>
                {
                    Assert.Equal(expected: "IncrementCount", actual: reference.Code);
                    Assert.Equal(expected: "Counter.razor", Path.GetFileName(reference.DocumentName));
                },
                reference =>
                {
                    Assert.Equal(expected: "private void IncrementCount()", actual: reference.Code);
                    Assert.Equal(expected: "Counter.razor", Path.GetFileName(reference.DocumentName));
                },
            });
    }

    [IdeFact]
    public async Task FindAllReferences_ComponentAttribute_FromRazor()
    {
        // Open the file
        await TestServices.SolutionExplorer.OpenFileAsync(RazorProjectConstants.BlazorProjectName, RazorProjectConstants.IndexRazorFile, ControlledHangMitigatingCancellationToken);

        await TestServices.Editor.PlaceCaretAsync("Title=", charsOffset: -1, ControlledHangMitigatingCancellationToken);

        await TestServices.Editor.WaitForComponentClassificationAsync(ControlledHangMitigatingCancellationToken);

        // Act
        await TestServices.Editor.InvokeFindAllReferencesAsync(ControlledHangMitigatingCancellationToken);

        // Assert
        var results = await TestServices.FindReferencesWindow.WaitForContentsAsync(ControlledHangMitigatingCancellationToken, expected: 3);

        // Don't care about order, but Assert.Collection does
        var orderedResults = OrderResults(results);

        Assert.Collection(
            orderedResults,
            reference =>
            {
                Assert.Equal("Index.razor", reference.DocumentName);
                Assert.Equal("Title", reference.Code);
            },
            reference =>
            {
                Assert.Equal("SurveyPrompt.razor", reference.DocumentName);
                Assert.Equal("public string? Title { get; set; }", reference.Code);
            },
            reference =>
            {
                Assert.Equal("SurveyPrompt.razor", reference.DocumentName);
                Assert.Equal("Title", reference.Code);
            }
        );
    }

    [IdeFact(Skip = "https://github.com/dotnet/razor/issues/8036")]
    public async Task FindAllReferences_ComponentAttribute_FromCSharpInRazor()
    {
        // Open the file
        await TestServices.SolutionExplorer.OpenFileAsync(RazorProjectConstants.BlazorProjectName, RazorProjectConstants.SurveyPromptFile, ControlledHangMitigatingCancellationToken);

        await TestServices.Editor.PlaceCaretAsync("Title", charsOffset: 0, occurrence: 2, extendSelection: false, selectBlock: false, ControlledHangMitigatingCancellationToken);

        // This is annoying, but if we do the FAR too quickly, we just get results from the current file
        await Task.Delay(500);

        // Act
        await TestServices.Editor.InvokeFindAllReferencesAsync(ControlledHangMitigatingCancellationToken);

        // Assert
        var results = await TestServices.FindReferencesWindow.WaitForContentsAsync(ControlledHangMitigatingCancellationToken, expected: 3);

        var orderedResults = OrderResults(results);

        Assert.Collection(
            orderedResults,
            reference =>
            {
                Assert.Equal(expected: "Index.razor", reference.DocumentName);
                Assert.Equal(expected: "Title", reference.Code);
            },
            reference =>
            {
                Assert.Equal(expected: "SurveyPrompt.razor", reference.DocumentName);
                Assert.Equal(expected: "public string? Title { get; set; }", reference.Code);
            },
            reference =>
            {
                Assert.Equal(expected: "SurveyPrompt.razor", reference.DocumentName);
                Assert.Equal(expected: "Title", reference.Code);
            }
        );
    }

    [IdeFact]
    public async Task FindAllReferences_ComponentAttribute_FromCSharpInCSharp()
    {
        // Create the file
        await TestServices.SolutionExplorer.AddFileAsync(RazorProjectConstants.BlazorProjectName,
            "MyComponent.razor",
            """
                @MyProperty
                """,
            open: false,
            cancellationToken: ControlledHangMitigatingCancellationToken);

        await TestServices.SolutionExplorer.AddFileAsync(RazorProjectConstants.BlazorProjectName,
            "MyComponent.razor.cs",
            """
                namespace BlazorProject;

                public partial class MyComponent
                {
                    [Microsoft.AspNetCore.Components.ParameterAttribute]
                    public string? MyProperty { get; set; }
                }

                """,
            open: false,
            cancellationToken: ControlledHangMitigatingCancellationToken);

        await TestServices.SolutionExplorer.AddFileAsync(RazorProjectConstants.BlazorProjectName,
            "MyPage.razor",
            """
                <MyComponent MyProperty="123" />
                """,
            open: true,
            cancellationToken: ControlledHangMitigatingCancellationToken);

        await TestServices.Editor.WaitForComponentClassificationAsync(ControlledHangMitigatingCancellationToken);

        await TestServices.SolutionExplorer.OpenFileAsync(RazorProjectConstants.BlazorProjectName, "MyComponent.razor.cs", ControlledHangMitigatingCancellationToken);

        await TestServices.Editor.PlaceCaretAsync("MyProperty", charsOffset: -1, ControlledHangMitigatingCancellationToken);

        // This is annoying, but if we do the FAR too quickly, we just get one result from the current file
        await Task.Delay(500);

        // Act
        await TestServices.Editor.InvokeFindAllReferencesAsync(ControlledHangMitigatingCancellationToken);

        // Assert
        var results = await TestServices.FindReferencesWindow.WaitForContentsAsync(ControlledHangMitigatingCancellationToken, expected: 3);

        var orderedResults = OrderResults(results);

        Assert.Collection(
            orderedResults,
            new Action<TableEntry>[]
            {
                reference =>
                {
                    Assert.Equal(expected: "@MyProperty", actual: reference.Code);
                    Assert.Equal(expected: "MyComponent.razor", Path.GetFileName(reference.DocumentName));
                },
                reference =>
                {
                    Assert.Equal(expected: "public string? MyProperty { get; set; }", actual: reference.Code);
                    Assert.Equal(expected: "MyComponent.razor.cs", Path.GetFileName(reference.DocumentName));
                },
                reference =>
                {
                    Assert.Equal(expected: "<MyComponent MyProperty=\"123\" />", actual: reference.Code);
                    Assert.Equal(expected: "MyPage.razor", Path.GetFileName(reference.DocumentName));
                },
            });
    }

    private static IEnumerable<TableEntry> OrderResults(ImmutableArray<ITableEntryHandle2> results)
    {
        var orderedResults = results.Select(r =>
        {
            Assert.True(r.TryGetValue(StandardTableKeyNames.Text, out string code));
            Assert.True(r.TryGetValue(StandardTableKeyNames.DocumentName, out string documentName));

            return new TableEntry(code, Path.GetFileName(documentName));
        }).OrderBy(r => r.DocumentName).ThenBy(r => r.Code).ToArray();

        return orderedResults;
    }

    internal record TableEntry(string Code, string DocumentName);
}
