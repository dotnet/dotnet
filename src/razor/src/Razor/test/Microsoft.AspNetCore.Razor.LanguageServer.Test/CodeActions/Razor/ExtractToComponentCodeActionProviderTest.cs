// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Components;
using Microsoft.AspNetCore.Razor.Language.Extensions;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.CodeAnalysis.Razor.CodeActions;
using Microsoft.CodeAnalysis.Razor.CodeActions.Models;
using Microsoft.CodeAnalysis.Razor.CodeActions.Razor;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Protocol.CodeActions;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Moq;
using Roslyn.Test.Utilities;
using Xunit;
using Xunit.Abstractions;
using WorkItemAttribute = Roslyn.Test.Utilities.WorkItemAttribute;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Test.CodeActions.Razor;

public class ExtractToComponentCodeActionProviderTest(ITestOutputHelper testOutput) : LanguageServerTestBase(testOutput)
{
    [Fact]
    public async Task Handle_InvalidFileKind()
    {
        // Arrange
        var documentPath = "c:/Test.razor";
        var contents = """
            @page "/"

            <PageTitle>Home</PageTitle>

            <div id="parent">
                <div>
                    <h1>Div a title</h1>
                    <p>Div [||]a par</p>
                </div>
                <div>
                    <h1>Div b title</h1>
                    <p>Div b par</p>
                </div>
            </div>

            <h1>Hello, world!</h1>

            Welcome to your new app.
            """;
        TestFileMarkupParser.GetSpan(contents, out contents, out var selectionSpan);

        var request = new VSCodeActionParams()
        {
            TextDocument = new VSTextDocumentIdentifier { Uri = new Uri(documentPath) },
            Range = VsLspFactory.DefaultRange,
            Context = new VSInternalCodeActionContext()
        };

        var context = CreateRazorCodeActionContext(request, selectionSpan, documentPath, contents);
        context.CodeDocument.SetFileKind(FileKinds.Legacy);

        var provider = new ExtractToComponentCodeActionProvider();

        // Act
        var commandOrCodeActionContainer = await provider.ProvideAsync(context, DisposalToken);

        // Assert
        Assert.Empty(commandOrCodeActionContainer);
    }

    [Fact]
    public Task Handle_SinglePointSelection_ReturnsNotEmpty()
        => TestAsync("""
            @page "/"

            <PageTitle>Home</PageTitle>

            <div id="parent">
                {|result:<{|selection:|}div>
                    <h1>Div a title</h1>
                    <p>Div a par</p>
                </div>|}
                <div>
                    <h1>Div b title</h1>
                    <p>Div b par</p>
                </div>
            </div>

            <h1>Hello, world!</h1>

            Welcome to your new app.
            """);

    [Fact]
    public Task Handle_InProperMarkup_ReturnsEmpty()
        => TestAsync("""
            @page "/"
            
            <PageTitle>Home</PageTitle>
            
            <div id="parent">
                <div>
                    <h1>Div a title</h1>
                    <p>Div {|selection:|}a par</p>
                </div>
                <div>
                    <h1>Div b title</h1>
                    <p>Div b par</p>
                </div>
            </div>

            <h1>Hello, world!</h1>

            Welcome to your new app.
            """);

    [Fact]
    public Task Handle_IsInCodeBlock_ReturnsEmpty()
        => TestAsync("""
            @page "/"

            @code
            {
                {|selection:public int I { get; set; }
                public void M()
                {
                }|}
            }
            """);

    [Fact]
    public Task Handle_MultiPointSelection_ReturnsNotEmpty()
       => TestAsync("""
            @page "/"

            <PageTitle>Home</PageTitle>

            <div id="parent">
                {|result:{|selection:<div>
                    |}<h1>Div a title</h1>
                    <p>Div a par</p>
                </div>|}
                <div>
                    <h1>Div b title</h1>
                    <p>Div b par</p>
                </div>
            </div>

            <h1>Hello, world!</h1>

            Welcome to your new app.
            """);

    [Fact]
    public Task Handle_MultiPointSelection_WithEndAfterElement()
        => TestAsync("""
            @page "/"
            @namespace MarketApp.Pages.Product.Home

            namespace MarketApp.Pages.Product.Home

            <PageTitle>Home</PageTitle>

            <div id="parent">
                {|result:{|selection:<div>
                    <h1>Div a title</h1>
                    <p>Div a par</p>
                </div>
                <div>
                    <h1>Div b title</h1>
                    <p>Div b par</p>
                </div>|}|}
            </div>

            <h1>Hello, world!</h1>

            Welcome to your new app.
            """);

    [Fact]
    public Task Handle_MultiPointSelection_WithEndInsideSiblingElement()
        => TestAsync("""
            @page "/"
            @namespace MarketApp.Pages.Product.Home

            namespace MarketApp.Pages.Product.Home

            <PageTitle>Home</PageTitle>

            <div id="parent">
                {|result:{|selection:<div>
                    <h1>Div a title</h1>
                    <p>Div a par</p>
                </div>
                <div>
                    <h1>Div b title</h1>|}
                    <p>Div b par</p>
                </div>|}
            </div>

            <h1>Hello, world!</h1>

            Welcome to your new app.
            """);

    [Fact]
    public Task Handle_MultiPointSelection_WithEndInsideElement()
        => TestAsync("""
            @page "/"
            @namespace MarketApp.Pages.Product.Home

            namespace MarketApp.Pages.Product.Home

            <PageTitle>Home</PageTitle>

            <div id="parent">
                {|result:{|selection:<div>
                    <h1>Div a title</h1>
                    <p>Div a par</p>|}
                </div>|}
                <div>
                    <h1>Div b title</h1>
                    <p>Div b par</p>
                </div>
            </div>

            <h1>Hello, world!</h1>

            Welcome to your new app.
            """);

    [Fact]
    public Task Handle_MultiPointSelection_WithNestedEnd()
        => TestAsync("""
            @page "/"
            @namespace MarketApp.Pages.Product.Home

            namespace MarketApp.Pages.Product.Home

            <PageTitle>Home</PageTitle>

            <div id="parent">
                {|result:{|selection:<div>
                    <h1>Div a title</h1>
                    <p>Div a par</p>
                </div>
                <div>
                    <div>
                        <div>
                            <h1>Div b title|}</h1>
                            <p>Div b par</p>
                        </div>
                    </div>
                </div>|}
            </div>

            <h1>Hello, world!</h1>

            Welcome to your new app.
            """);

    [Fact]
    public Task Handle_MultiPointSelection_WithNestedStart()
        => TestAsync("""
            @page "/"
            @namespace MarketApp.Pages.Product.Home

            namespace MarketApp.Pages.Product.Home

            <PageTitle>Home</PageTitle>

            <div id="parent">
                {|result:<div>
                    <div>
                        <div>
                            <h1>{|selection:Div a title</h1>
                            <p>Div a par</p>
                        </div>
                    </div>
                </div>
                <div>
                    <div>
                        <div>
                            <h1>Div b title</h1>
                            <p>Div b par</p>
                        </div>
                    </div>
                </div>|}|}
            </div>

            <h1>Hello, world!</h1>

            Welcome to your new app.
            """);

    [Fact]
    public Task Handle_MultiPointSelection_WithNestedStartAndEnd()
        => TestAsync("""
            @page "/"
            @namespace MarketApp.Pages.Product.Home

            namespace MarketApp.Pages.Product.Home

            <PageTitle>Home</PageTitle>

            <div id="parent">
                {|result:<div>
                    <div>
                        <div>
                            <h1>{|selection:Div a title</h1>
                            <p>Div a par</p>
                        </div>
                    </div>
                </div>
                <div>
                    <div>
                        <div>
                            <h1>Div b title</h1>
                            <p>Div b par|}</p>
                        </div>
                    </div>
                </div>|}
            </div>

            <h1>Hello, world!</h1>

            Welcome to your new app.
            """);

    [Fact]
    public Task Handle_MultiPointSelection_StartSelfClosing()
       => TestAsync("""
            @page "/"

            <PageTitle>Home</PageTitle>

            <div id="parent">
                {|result:{|selection:<img src="/myimg.png" />
                <div>
                    <h1>Div a title</h1>
                    <p>Div a par</p>
                </div>|}|}
                <div>
                    <h1>Div b title</h1>
                    <p>Div b par</p>
                </div>
            </div>

            <h1>Hello, world!</h1>

            Welcome to your new app.
            """);

    [Fact]
    public Task Handle_MultipointSelection_CodeBlock()
        => TestAsync("""
            {|result:{|selection:<p>Hello</p>

            @code {
              |}
            }|}
            """);

    [Fact]
    public Task Handle_MultipointSelection_IfBlock()
        => TestAsync("""
            {|result:{|selection:<p>Hello</p>

            @if (true) {
              |}
            }|}
            """);

    [Fact]
    public Task Handle_MultipointSelection_EmbeddedIfBlock()
        => TestAsync("""
            {|result:{|selection:<p>Hello</p>

            <div>
                <div>
                    @if (true) {
                      |}
                    }
                </div>
            </div>|}
            """);

    [Fact]
    public Task Handle_MultipointSelection_CSharpBlock()
        => TestAsync(
            """
            {|result:<div{|selection:>blah</div>

            @{
                RenderFragment fragment = @<Component1 Id="Comp1" Caption="Title">|} </Component1>;
            }|}
            """);

    [Fact]
    public Task Handle_MultipointSelection_SurroundingH1()
        => TestAsync("""
            @page "/"

            <PageTitle>Home</PageTitle>

            {|result:{|selection:<h1>Hello</h1>|}|}

            Welcome to your new app.
            """);

    [Fact]
    public Task Handle_MultipointSelection_TextOnly()
        => TestAsync("""
            @page "/"

            <PageTitle>Home</PageTitle>
            <h1>Hello</h1>

            {|result:{|selection:Welcome to your new app|}|}
            """);

    [Fact]
    public Task Handle_MultipointSelection_TextOnly2()
        => TestAsync("""
            @page "/"

            <PageTitle>Home</PageTitle>
            <h1>Hello</h1>

            Welcome to {|result:{|selection:your new app|}|}
            """);

    [Fact]
    public Task Handle_MultipointSelection_TextInNested()
        => TestAsync("""
            {|result:<div>
                <div>
                    <p>
                        Hello {|selection:there!
                    </p>
                </div>
            </div>

            Welcome to your|}|} new app
            """);

    [Fact]
    public Task Handle_MultipointSelection_TextInNested2()
        => TestAsync("""
            Welcome to your{|result:{|selection: new app

            <div>
                <div>
                    <p>
                        Hello |}there!
                    </p>
                </div>
            </div>|}
            """);

    [Fact]
    [WorkItem("https://github.com/dotnet/razor/issues/11261")]
    public Task Handle_Inside_ElseBlock()
        => TestAsync("""
            @page "/weather"

            <PageTitle>Weather</PageTitle>

            <h1>Weather</h1>

            <p>This component demonstrates showing data.</p>

            @if (forecasts == null)
            {
                <p><em>Loading...</em></p>
            }
            else
            {
            {|selection:    {|result:<table class="table">
                    <thead>
                        <tr>
                            <th>Date</th>
                            <th aria-label="Temperature in Celsius">Temp. (C)</th>
                            <th aria-label="Temperature in Farenheit">Temp. (F)</th>
                            <th>Summary</th>
                        </tr>
                    </thead>
                    <tbody>
                        @foreach (var forecast in forecasts)
                        {
                            <tr>
                                <td>@forecast.Date.ToShortDateString()</td>
                                <td>@forecast.TemperatureC</td>
                                <td>@forecast.TemperatureF</td>
                                <td>@forecast.Summary</td>
                            </tr>
                        }
                    </tbody>
                </table>|}|}
            }

            @code {
                private WeatherForecast[]? forecasts;

                protected override async Task OnInitializedAsync()
                {
                    // Simulate asynchronous loading to demonstrate a loading indicator
                    await Task.Delay(500);

                    var startDate = DateOnly.FromDateTime(DateTime.Now);
                    var summaries = new[] { "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" };
                    forecasts = Enumerable.Range(1, 5).Select(index => new WeatherForecast
                    {
                        Date = startDate.AddDays(index),
                        TemperatureC = Random.Shared.Next(-20, 55),
                        Summary = summaries[Random.Shared.Next(summaries.Length)]
                    }).ToArray();
                }

                private class WeatherForecast
                {
                    public DateOnly Date { get; set; }
                    public int TemperatureC { get; set; }
                    public string? Summary { get; set; }
                    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
                }
            }
            """);

    [Fact]
    [WorkItem("https://github.com/dotnet/razor/issues/11261")]
    public Task Handle_Inside_ElseBlock_NoSelection()
        => TestAsync("""
            @page "/weather"

            <PageTitle>Weather</PageTitle>

            <h1>Weather</h1>

            <p>This component demonstrates showing data.</p>

            @if (forecasts == null)
            {
                <p><em>Loading...</em></p>
            }
            else
            {
            {|selection:|}  <table class="table">
                    <thead>
                        <tr>
                            <th>Date</th>
                            <th aria-label="Temperature in Celsius">Temp. (C)</th>
                            <th aria-label="Temperature in Farenheit">Temp. (F)</th>
                            <th>Summary</th>
                        </tr>
                    </thead>
                    <tbody>
                        @foreach (var forecast in forecasts)
                        {
                            <tr>
                                <td>@forecast.Date.ToShortDateString()</td>
                                <td>@forecast.TemperatureC</td>
                                <td>@forecast.TemperatureF</td>
                                <td>@forecast.Summary</td>
                            </tr>
                        }
                    </tbody>
                </table>
            }

            @code {
                private WeatherForecast[]? forecasts;

                protected override async Task OnInitializedAsync()
                {
                    // Simulate asynchronous loading to demonstrate a loading indicator
                    await Task.Delay(500);

                    var startDate = DateOnly.FromDateTime(DateTime.Now);
                    var summaries = new[] { "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" };
                    forecasts = Enumerable.Range(1, 5).Select(index => new WeatherForecast
                    {
                        Date = startDate.AddDays(index),
                        TemperatureC = Random.Shared.Next(-20, 55),
                        Summary = summaries[Random.Shared.Next(summaries.Length)]
                    }).ToArray();
                }

                private class WeatherForecast
                {
                    public DateOnly Date { get; set; }
                    public int TemperatureC { get; set; }
                    public string? Summary { get; set; }
                    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
                }
            }
            """);

    private static RazorCodeActionContext CreateRazorCodeActionContext(
        VSCodeActionParams request,
        TextSpan selectionSpan,
        string filePath,
        string text,
        string? relativePath = null,
        bool supportsFileCreation = true)
    {
        relativePath ??= filePath;

        var sourceDocument = RazorSourceDocument.Create(text, RazorSourceDocumentProperties.Create(filePath, relativePath));
        var options = RazorParserOptions.Create(o =>
        {
            o.Directives.Add(ComponentCodeDirective.Directive);
            o.Directives.Add(FunctionsDirective.Directive);
        });
        var syntaxTree = RazorSyntaxTree.Parse(sourceDocument, options);

        var codeDocument = TestRazorCodeDocument.Create(sourceDocument, imports: default);
        codeDocument.SetFileKind(FileKinds.Component);
        codeDocument.SetCodeGenerationOptions(RazorCodeGenerationOptions.Create(o =>
        {
            o.RootNamespace = "ExtractToComponentTest";
        }));
        codeDocument.SetSyntaxTree(syntaxTree);

        var documentSnapshot = new StrictMock<IDocumentSnapshot>();
        documentSnapshot
            .Setup(document => document.GetTextAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(codeDocument.Source.Text);

        var sourceText = SourceText.From(text);

        var context = new RazorCodeActionContext(
            request,
            documentSnapshot.Object,
            codeDocument,
            DelegatedDocumentUri: null,
            selectionSpan.Start,
            selectionSpan.End,
            RazorLanguageKind.Razor,
            sourceText,
            supportsFileCreation,
            SupportsCodeActionResolve: true);

        return context;
    }

    /// <summary>
    /// Tests the contents where the expected start/end are marked by '[|' and '$$'
    /// </summary>
    private async Task TestAsync(string contents)
    {
        // Arrange
        var documentPath = "c:/Test.razor";
        TestFileMarkupParser.GetSpans(contents, out contents, out ImmutableDictionary<string, ImmutableArray<TextSpan>> spans);

        var selectionSpan = spans["selection"].Single();
        var resultSpan = spans.ContainsKey("result")
            ? spans["result"].Single()
            : default;

        var request = new VSCodeActionParams()
        {
            TextDocument = new VSTextDocumentIdentifier { Uri = new Uri(documentPath) },
            Range = VsLspFactory.DefaultRange,
            Context = new VSInternalCodeActionContext()
        };

        var context = CreateRazorCodeActionContext(request, selectionSpan, documentPath, contents);

        var lineSpan = context.SourceText.GetLinePositionSpan(selectionSpan);
        request.Range = VsLspFactory.CreateRange(lineSpan);

        var provider = new ExtractToComponentCodeActionProvider();

        // Act
        var commandOrCodeActionContainer = await provider.ProvideAsync(context, DisposalToken);

        // Assert
        if (resultSpan.IsEmpty)
        {
            Assert.Empty(commandOrCodeActionContainer);
            return;
        }

        Assert.NotEmpty(commandOrCodeActionContainer);
        var codeAction = Assert.Single(commandOrCodeActionContainer);
        var razorCodeActionResolutionParams = ((JsonElement)codeAction.Data!).Deserialize<RazorCodeActionResolutionParams>();
        Assert.NotNull(razorCodeActionResolutionParams);
        var actionParams = ((JsonElement)razorCodeActionResolutionParams.Data!).Deserialize<ExtractToComponentCodeActionParams>();
        Assert.NotNull(actionParams);

        if (resultSpan.Start != actionParams.Start || resultSpan.End != actionParams.End)
        {
            var resultText = context.SourceText.GetSubTextString(resultSpan);
            var actualText = context.SourceText.GetSubTextString(TextSpan.FromBounds(actionParams.Start, actionParams.End));
            AssertEx.EqualOrDiff(resultText, actualText, "Code action span does not match expected");
        }
    }
}
