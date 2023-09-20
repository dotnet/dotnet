// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading.Tasks;
using Roslyn.Test.Utilities;
using Xunit;

namespace Microsoft.NET.Sdk.Razor.SourceGenerators;

public sealed class RazorSourceGeneratorCshtmlTests : RazorSourceGeneratorTestsBase
{
    [Fact, WorkItem("https://github.com/dotnet/razor/issues/9034")]
    public async Task CssScoping()
    {
        // Arrange
        var project = CreateTestProject(new()
        {
            ["Pages/Index.cshtml"] = """
                @page
                <h1>heading</h1>
                <header>header</header>
                <thead>table head</thead>
                <form>
                    <select>
                        <option>choose</option>
                    </select>
                </form>
                < div />
                <div
                >multiline</div>
                <!div>bang</div>
                <input multiple value="test" disabled="@("<input".Length > 0)" />
                <div></div>
                @* <div>Razor comment</div> *@
                <!-- <div>HTML comment</div> -->
                @{
                    <div>code block</div>
                    //<div>C# comment</div>
                    Func<dynamic, object> template = @<div>C# template</div>;
                    string x = "attr";
                }
                @template(null!)
                <div @x="1"></div>
                <div@x="1"></div>

                Ignored:
                - <head />
                - <meta />
                - <title />
                - <link />
                - <base />
                - <script />
                - <style />
                - <html />
                """,
        });
        var compilation = await project.GetCompilationAsync();
        var driver = await GetDriverAsync(project, options =>
        {
            options.AdditionalTextOptions["Pages/Index.cshtml"]["build_metadata.AdditionalFiles.CssScope"] = "test-css-scope";
        });

        // Act
        var result = RunGenerator(compilation!, ref driver, out compilation);

        // Assert
        Assert.Empty(result.Diagnostics);
        Assert.Single(result.GeneratedSources);
        await VerifyRazorPageMatchesBaselineAsync(compilation, "Pages_Index");
    }
}
