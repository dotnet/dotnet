// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System.Collections.Immutable;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis.Razor.Formatting;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Formatting;

[Collection(HtmlFormattingCollection.Name)]
public class HtmlFormattingTest(HtmlFormattingFixture fixture, ITestOutputHelper testOutput)
    : FormattingTestBase(fixture.Service, testOutput)
{
    internal override bool UseTwoPhaseCompilation => true;

    internal override bool DesignTime => true;

    [Fact]
    public async Task FormatsSimpleHtmlTag()
    {
        await RunFormattingTestAsync(
            input: """
                       <html>
                    <head>
                       <title>Hello</title></head>
                    <body><div>
                    </div>
                            </body>
                     </html>
                    """,
            expected: """
                    <html>
                    <head>
                        <title>Hello</title>
                    </head>
                    <body>
                        <div>
                        </div>
                    </body>
                    </html>
                    """);
    }

    [Fact]
    public async Task FormatsSimpleHtmlTag_Range()
    {
        await RunFormattingTestAsync(
            input: """
                    <html>
                    <head>
                        <title>Hello</title>
                    </head>
                    <body>
                            [|<div>
                            </div>|]
                    </body>
                    </html>
                    """,
            expected: """
                    <html>
                    <head>
                        <title>Hello</title>
                    </head>
                    <body>
                        <div>
                        </div>
                    </body>
                    </html>
                    """);
    }

    [Fact]
    public async Task FormatsSimpleHtmlTag_OnType()
    {
        await RunOnTypeFormattingTestAsync(
            input: """
                    <html>
                    <head>
                        <title>Hello</title>
                            <script>
                                var x = 2;$$
                            </script>
                    </head>
                    </html>
                    """,
            expected: """
                    <html>
                    <head>
                        <title>Hello</title>
                        <script>
                            var x = 2;
                        </script>
                    </head>
                    </html>
                    """,
            triggerCharacter: ';',
            fileKind: FileKinds.Legacy);
    }

    [Fact]
    public async Task FormatsRazorHtmlBlock()
    {
        await RunFormattingTestAsync(
            input: """
                    @page "/error"

                            <h1 class=
                    "text-danger">Error.</h1>
                        <h2 class="text-danger">An error occurred while processing your request.</h2>

                                <h3>Development Mode</h3>
                    <p>
                        Swapping to <strong>Development</strong> environment will display more detailed information about the error that occurred.</p>
                    <p>
                        <strong>The Development environment shouldn't be enabled for deployed applications.
                    </strong>
                                <div>
                     <div>
                        <div>
                    <div>
                            This is heavily nested
                    </div>
                     </div>
                        </div>
                            </div>
                    </p>
                    """,
            expected: """
                    @page "/error"

                    <h1 class="text-danger">
                        Error.
                    </h1>
                    <h2 class="text-danger">An error occurred while processing your request.</h2>

                    <h3>Development Mode</h3>
                    <p>
                        Swapping to <strong>Development</strong> environment will display more detailed information about the error that occurred.
                    </p>
                    <p>
                        <strong>
                            The Development environment shouldn't be enabled for deployed applications.
                        </strong>
                        <div>
                            <div>
                                <div>
                                    <div>
                                        This is heavily nested
                                    </div>
                                </div>
                            </div>
                        </div>
                    </p>
                    """);
    }

    [Fact]
    public async Task FormatsMixedHtmlBlock()
    {
        await RunFormattingTestAsync(
            input: """
                    @page "/test"
                    @{
                    <p>
                            @{
                                    var t = 1;
                    if (true)
                    {

                                }
                            }
                            </p>
                    <div>
                     @{
                        <div>
                    <div>
                            This is heavily nested
                    </div>
                     </div>
                        }
                            </div>
                    }
                    """,
            expected: """
                    @page "/test"
                    @{
                        <p>
                            @{
                                var t = 1;
                                if (true)
                                {

                                }
                            }
                        </p>
                        <div>
                            @{
                                <div>
                                    <div>
                                        This is heavily nested
                                    </div>
                                </div>
                            }
                        </div>
                    }
                    """);
    }

    [Fact]
    public async Task FormatAttributeStyles()
    {
        await RunFormattingTestAsync(
            input: """
                    <div class=@className>Some Text</div>
                    <div class=@className style=@style>Some Text</div>
                    <div class=@className style="@style">Some Text</div>
                    <div class='@className'>Some Text</div>
                    <div class="@className">Some Text</div>
                    
                    <br class=@className/>
                    <br class=@className style=@style/>
                    <br class=@className style="@style"/>
                    <br class='@className'/>
                    <br class="@className"/>
                    """,
            expected: """
                    <div class=@className>Some Text</div>
                    <div class=@className style=@style>Some Text</div>
                    <div class=@className style="@style">Some Text</div>
                    <div class='@className'>Some Text</div>
                    <div class="@className">Some Text</div>

                    <br class=@className/>
                    <br class=@className style=@style/>
                    <br class=@className style="@style"/>
                    <br class='@className'/>
                    <br class="@className"/>
                    """);
    }

    [Fact]
    public async Task FormatsMixedRazorBlock()
    {
        await RunFormattingTestAsync(
            input: """
                    @page "/test"

                    <div class=@className>Some Text</div>

                    @{
                    @: Hi!
                    var x = 123;
                    <p>
                            @if (true) {
                                    var t = 1;
                    if (true)
                    {
                    <div>@DateTime.Now</div>
                                }

                                @while(true){
                     }
                            }
                            </p>
                    }
                    """,
            expected: """
                    @page "/test"

                    <div class=@className>Some Text</div>

                    @{
                        @: Hi!
                        var x = 123;
                        <p>
                            @if (true)
                            {
                                var t = 1;
                                if (true)
                                {
                                    <div>@DateTime.Now</div>
                                }

                                @while (true)
                                {
                                }
                            }
                        </p>
                    }
                    """);
    }

    [Fact]
    public async Task FormatsMixedContentWithMultilineExpressions()
    {
        await RunFormattingTestAsync(
            input: """
                    @page "/test"

                    <div
                    attr='val'
                    class=@className>Some Text</div>

                    @{
                    @: Hi!
                    var x = DateTime
                        .Now.ToString();
                    <p>
                            @if (true) {
                                    var t = 1;
                            }
                            </p>
                    }

                    @(DateTime
                        .Now
                    .ToString())

                    @(
                        Foo.Values.Select(f =>
                        {
                            return f.ToString();
                        })
                    )
                    """,
            expected: """
                    @page "/test"

                    <div attr='val'
                         class=@className>
                        Some Text
                    </div>

                    @{
                        @: Hi!
                        var x = DateTime
                            .Now.ToString();
                        <p>
                            @if (true)
                            {
                                var t = 1;
                            }
                        </p>
                    }

                    @(DateTime
                        .Now
                    .ToString())

                    @(
                        Foo.Values.Select(f =>
                        {
                            return f.ToString();
                        })
                    )
                    """);
    }

    [Fact]
    public async Task FormatsComplexBlock()
    {
        await RunFormattingTestAsync(
            input: """
                    @page "/"

                    <h1>Hello, world!</h1>

                            Welcome to your new app.

                    <SurveyPrompt Title="How is Blazor working for you?" />

                    <div class="FF"
                         id="ERT">
                         asdf
                        <div class="3"
                             id="3">
                                 @if(true){<p></p>}
                             </div>
                    </div>

                    @{
                    <div class="FF"
                        id="ERT">
                        asdf
                        <div class="3"
                            id="3">
                                @if(true){<p></p>}
                            </div>
                    </div>
                    }

                    @functions {
                            public class Foo
                        {
                            @* This is a Razor Comment *@
                            void Method() { }
                        }
                    }
                    """,
            expected: """
                    @page "/"

                    <h1>Hello, world!</h1>

                            Welcome to your new app.

                    <SurveyPrompt Title="How is Blazor working for you?" />

                    <div class="FF"
                         id="ERT">
                        asdf
                        <div class="3"
                             id="3">
                            @if (true)
                            {
                                <p></p>
                            }
                        </div>
                    </div>

                    @{
                        <div class="FF"
                             id="ERT">
                            asdf
                            <div class="3"
                                 id="3">
                                @if (true)
                                {
                                    <p></p>
                                }
                            </div>
                        </div>
                    }

                    @functions {
                        public class Foo
                        {
                            @* This is a Razor Comment *@
                            void Method() { }
                        }
                    }
                    """);
    }

    [Fact]
    public async Task FormatsComponentTags()
    {
        var tagHelpers = GetComponents();
        await RunFormattingTestAsync(
            input: """
                       <Counter>
                        @if(true){
                            <p>@DateTime.Now</p>
                    }
                    </Counter>

                        <GridTable>
                        @foreach (var row in rows){
                            <GridRow @onclick="SelectRow(row)">
                            @foreach (var cell in row){
                        <GridCell>@cell</GridCell>}</GridRow>
                        }
                    </GridTable>
                    """,
            expected: """
                    <Counter>
                        @if (true)
                        {
                            <p>@DateTime.Now</p>
                        }
                    </Counter>

                    <GridTable>
                        @foreach (var row in rows)
                        {
                            <GridRow @onclick="SelectRow(row)">
                                @foreach (var cell in row)
                                {
                                    <GridCell>@cell</GridCell>
                                }
                            </GridRow>
                        }
                    </GridTable>
                    """,
            tagHelpers: tagHelpers,
            skipFlipLineEndingTest: true); // tracked by https://github.com/dotnet/razor/issues/10836
    }

    [Fact]
    public async Task FormatsComponentTag_WithImplicitExpression()
    {
        var tagHelpers = GetComponents();
        await RunFormattingTestAsync(
            input: """
                        <GridTable>
                            <GridRow >
                        <GridCell>@cell</GridCell>
                    <GridCell>cell</GridCell>
                        </GridRow>
                    </GridTable>
                    """,
            expected: """
                    <GridTable>
                        <GridRow>
                            <GridCell>@cell</GridCell>
                            <GridCell>cell</GridCell>
                        </GridRow>
                    </GridTable>
                    """,
            tagHelpers: tagHelpers);
    }

    [Fact]
    public async Task FormatsComponentTag_WithExplicitExpression()
    {
        var tagHelpers = GetComponents();
        await RunFormattingTestAsync(
            input: """
                        <GridTable>
                            <GridRow >
                        <GridCell>@(cell)</GridCell>
                        </GridRow>
                    </GridTable>
                    """,
            expected: """
                    <GridTable>
                        <GridRow>
                            <GridCell>@(cell)</GridCell>
                        </GridRow>
                    </GridTable>
                    """,
            tagHelpers: tagHelpers);
    }

    [Fact]
    public async Task FormatsComponentTag_WithExplicitExpression_FormatsInside()
    {
        var tagHelpers = GetComponents();
        await RunFormattingTestAsync(
            input: """
                        <GridTable>
                            <GridRow >
                        <GridCell>@(""  +    "")</GridCell>
                        </GridRow>
                    </GridTable>
                    """,
            expected: """
                    <GridTable>
                        <GridRow>
                            <GridCell>@("" + "")</GridCell>
                        </GridRow>
                    </GridTable>
                    """,
            tagHelpers: tagHelpers);
    }

    [Fact]
    public async Task FormatsComponentTag_WithExplicitExpression_MovesStart()
    {
        var tagHelpers = GetComponents();
        await RunFormattingTestAsync(
            input: """
                        <GridTable>
                            <GridRow >
                        <GridCell>
                        @(""  +    "")
                        </GridCell>
                        </GridRow>
                    </GridTable>
                    """,
            expected: """
                    <GridTable>
                        <GridRow>
                            <GridCell>
                                @("" + "")
                            </GridCell>
                        </GridRow>
                    </GridTable>
                    """,
            tagHelpers: tagHelpers);
    }

    [Fact]
    public async Task FormatsShortBlock()
    {
        await RunFormattingTestAsync(
            input: """
                    @{<p></p>}
                    """,
            expected: """
                    @{
                        <p></p>
                    }
                    """,
            skipFlipLineEndingTest: true); // tracked by https://github.com/dotnet/razor/issues/10836
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/aspnetcore/issues/26836")]
    public async Task FormatNestedBlock()
    {
        await RunFormattingTestAsync(
            input: """
                    @code {
                        public string DoSomething()
                        {
                            <strong>
                                @DateTime.Now.ToString()
                            </strong>

                            return String.Empty;
                        }
                    }
                    """,
            expected: """
                    @code {
                        public string DoSomething()
                        {
                            <strong>
                                @DateTime.Now.ToString()
                            </strong>

                            return String.Empty;
                        }
                    }
                    """);
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/aspnetcore/issues/26836")]
    public async Task FormatNestedBlock_Tabs()
    {
        await RunFormattingTestAsync(
            input: """
                    @code {
                        public string DoSomething()
                        {
                            <strong>
                                @DateTime.Now.ToString()
                            </strong>

                            return String.Empty;
                        }
                    }
                    """,
            expected: """
                    @code {
                    	public string DoSomething()
                    	{
                    		<strong>
                    			@DateTime.Now.ToString()
                    		</strong>

                    		return String.Empty;
                    	}
                    }
                    """,
            tabSize: 4, // Due to a bug in the HTML formatter, this needs to be 4
            insertSpaces: false);
    }

    [Fact]
    [WorkItem("https://devdiv.visualstudio.com/DevDiv/_workitems/edit/1273468/")]
    public async Task FormatHtmlWithTabs1()
    {
        await RunFormattingTestAsync(
            input: """
                    @page "/"
                    @{
                     ViewData["Title"] = "Create";
                     <hr />
                     <div class="row">
                      <div class="col-md-4">
                       <form method="post">
                        <div class="form-group">
                         <label asp-for="Movie.Title" class="control-label"></label>
                         <input asp-for="Movie.Title" class="form-control" />
                         <span asp-validation-for="Movie.Title" class="text-danger"></span>
                        </div>
                       </form>
                      </div>
                     </div>
                    }
                    """,
            expected: """
                    @page "/"
                    @{
                    	ViewData["Title"] = "Create";
                    	<hr />
                    	<div class="row">
                    		<div class="col-md-4">
                    			<form method="post">
                    				<div class="form-group">
                    					<label asp-for="Movie.Title" class="control-label"></label>
                    					<input asp-for="Movie.Title" class="form-control" />
                    					<span asp-validation-for="Movie.Title" class="text-danger"></span>
                    				</div>
                    			</form>
                    		</div>
                    	</div>
                    }
                    """,
            tabSize: 4, // Due to a bug in the HTML formatter, this needs to be 4
            insertSpaces: false,
            fileKind: FileKinds.Legacy);
    }

    [Fact]
    [WorkItem("https://devdiv.visualstudio.com/DevDiv/_workitems/edit/1273468/")]
    public async Task FormatHtmlWithTabs2()
    {
        await RunFormattingTestAsync(
            input: """
                    @page "/"

                     <hr />
                     <div class="row">
                      <div class="col-md-4">
                       <form method="post">
                        <div class="form-group">
                         <label asp-for="Movie.Title" class="control-label"></label>
                         <input asp-for="Movie.Title" class="form-control" />
                         <span asp-validation-for="Movie.Title" class="text-danger"></span>
                        </div>
                       </form>
                      </div>
                     </div>
                    """,
            expected: """
                    @page "/"

                    <hr />
                    <div class="row">
                    	<div class="col-md-4">
                    		<form method="post">
                    			<div class="form-group">
                    				<label asp-for="Movie.Title" class="control-label"></label>
                    				<input asp-for="Movie.Title" class="form-control" />
                    				<span asp-validation-for="Movie.Title" class="text-danger"></span>
                    			</div>
                    		</form>
                    	</div>
                    </div>
                    """,
            tabSize: 4, // Due to a bug in the HTML formatter, this needs to be 4
            insertSpaces: false,
            fileKind: FileKinds.Legacy);
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/aspnetcore/issues/30382")]
    public async Task FormatNestedComponents()
    {
        await RunFormattingTestAsync(
            input: """
                    <CascadingAuthenticationState>
                    <Router AppAssembly="@typeof(Program).Assembly">
                        <Found Context="routeData">
                            <RouteView RouteData="@routeData" DefaultLayout="@typeof(MainLayout)" />
                        </Found>
                        <NotFound>
                            <LayoutView Layout="@typeof(MainLayout)">
                                <p>Sorry, there's nothing at this address.</p>

                                @if (true)
                                        {
                                            <strong></strong>
                                    }
                            </LayoutView>
                        </NotFound>
                    </Router>
                    </CascadingAuthenticationState>
                    """,
            expected: """
                    <CascadingAuthenticationState>
                        <Router AppAssembly="@typeof(Program).Assembly">
                            <Found Context="routeData">
                                <RouteView RouteData="@routeData" DefaultLayout="@typeof(MainLayout)" />
                            </Found>
                            <NotFound>
                                <LayoutView Layout="@typeof(MainLayout)">
                                    <p>Sorry, there's nothing at this address.</p>

                                    @if (true)
                                    {
                                        <strong></strong>
                                    }
                                </LayoutView>
                            </NotFound>
                        </Router>
                    </CascadingAuthenticationState>
                    """);
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/aspnetcore/issues/30382")]
    public async Task FormatNestedComponents2()
    {
        await RunFormattingTestAsync(
            input: """
                    <GridTable>
                    <ChildContent>
                    <GridRow>
                    <ChildContent>
                    <GridCell>
                    <ChildContent>
                    <strong></strong>
                    @if (true)
                    {
                    <strong></strong>
                    }
                    <strong></strong>
                    </ChildContent>
                    </GridCell>
                    </ChildContent>
                    </GridRow>
                    </ChildContent>
                    </GridTable>
                    """,
            expected: """
                    <GridTable>
                        <ChildContent>
                            <GridRow>
                                <ChildContent>
                                    <GridCell>
                                        <ChildContent>
                                            <strong></strong>
                                            @if (true)
                                            {
                                                <strong></strong>
                                            }
                                            <strong></strong>
                                        </ChildContent>
                                    </GridCell>
                                </ChildContent>
                            </GridRow>
                        </ChildContent>
                    </GridTable>
                    """,
            tagHelpers: GetComponents());
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/razor/issues/8227")]
    public async Task FormatNestedComponents3()
    {
        await RunFormattingTestAsync(
            input: """
                    @if (true)
                    {
                        <Component1 Id="comp1"
                                Caption="Title" />
                    <Component1 Id="comp2"
                                Caption="Title">
                                <Frag>
                    <Component1 Id="comp3"
                                Caption="Title" />
                                </Frag>
                                </Component1>
                    }

                    @if (true)
                    {
                        <a_really_long_tag_name Id="comp1"
                                Caption="Title" />
                    <a_really_long_tag_name Id="comp2"
                                Caption="Title">
                                <a_really_long_tag_name>
                    <a_really_long_tag_name Id="comp3"
                                Caption="Title" />
                                </a_really_long_tag_name>
                                </a_really_long_tag_name>
                    }
                    """,
            expected: """
                    @if (true)
                    {
                        <Component1 Id="comp1"
                                    Caption="Title" />
                        <Component1 Id="comp2"
                                    Caption="Title">
                            <Frag>
                                <Component1 Id="comp3"
                                            Caption="Title" />
                            </Frag>
                        </Component1>
                    }

                    @if (true)
                    {
                        <a_really_long_tag_name Id="comp1"
                                                Caption="Title" />
                        <a_really_long_tag_name Id="comp2"
                                                Caption="Title">
                            <a_really_long_tag_name>
                                <a_really_long_tag_name Id="comp3"
                                                        Caption="Title" />
                            </a_really_long_tag_name>
                        </a_really_long_tag_name>
                    }
                    """,
            tagHelpers: GetComponents());
    }

    [Fact(Skip = "Requires fix")]
    [WorkItem("https://github.com/dotnet/razor/issues/8228")]
    public async Task FormatNestedComponents4()
    {
        await RunFormattingTestAsync(
            input: """
                    @{
                        RenderFragment fragment =
                          @<Component1 Id="Comp1"
                                     Caption="Title">
                        </Component1>;
                    }
                    """,
            expected: """
                    @{
                        RenderFragment fragment =
                        @<Component1 Id="Comp1"
                                     Caption="Title">
                        </Component1>;
                    }
                    """,
            tagHelpers: GetComponents());
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/razor/issues/8229")]
    public async Task FormatNestedComponents5()
    {
        await RunFormattingTestAsync(
            input: """
                    <Component1>
                        @{
                            RenderFragment fragment =
                            @<Component1 Id="Comp1"
                                     Caption="Title">
                            </Component1>;
                        }
                    </Component1>
                    """,
            expected: """
                    <Component1>
                        @{
                            RenderFragment fragment =
                            @<Component1 Id="Comp1"
                                         Caption="Title">
                            </Component1>;
                        }
                    </Component1>
                    """,
            tagHelpers: GetComponents());
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/aspnetcore/issues/30382")]
    public async Task FormatNestedComponents2_Range()
    {
        await RunFormattingTestAsync(
            input: """
                    <GridTable>
                    <ChildContent>
                    <GridRow>
                    <ChildContent>
                    <GridCell>
                    <ChildContent>
                    <strong></strong>
                    @if (true)
                    {
                    [|<strong></strong>|]
                    }
                    <strong></strong>
                    </ChildContent>
                    </GridCell>
                    </ChildContent>
                    </GridRow>
                    </ChildContent>
                    </GridTable>
                    """,
            expected: """
                    <GridTable>
                    <ChildContent>
                    <GridRow>
                    <ChildContent>
                    <GridCell>
                    <ChildContent>
                    <strong></strong>
                    @if (true)
                    {
                                                <strong></strong>
                    }
                    <strong></strong>
                    </ChildContent>
                    </GridCell>
                    </ChildContent>
                    </GridRow>
                    </ChildContent>
                    </GridTable>
                    """,
            tagHelpers: GetComponents());
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/aspnetcore/issues/29645")]
    public async Task FormatHtmlInIf()
    {
        await RunFormattingTestAsync(
            input: """
                    @if (true)
                    {
                        <p><em>Loading...</em></p>
                    }
                    else
                    {
                        <table class="table">
                            <thead>
                                <tr>
                            <th>Date</th>
                            <th>Temp. (C)</th>
                            <th>Temp. (F)</th>
                            <th>Summary</th>
                                </tr>
                            </thead>
                        </table>
                    }
                    """,
            expected: """
                    @if (true)
                    {
                        <p><em>Loading...</em></p>
                    }
                    else
                    {
                        <table class="table">
                            <thead>
                                <tr>
                                    <th>Date</th>
                                    <th>Temp. (C)</th>
                                    <th>Temp. (F)</th>
                                    <th>Summary</th>
                                </tr>
                            </thead>
                        </table>
                    }
                    """);
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/aspnetcore/issues/29645")]
    public async Task FormatHtmlInIf_Range()
    {
        await RunFormattingTestAsync(
            input: """
                    @if (true)
                    {
                        <p><em>Loading...</em></p>
                    }
                    else
                    {
                        <table class="table">
                            <thead>
                                <tr>
                    [|      <th>Date</th>
                            <th>Temp. (C)</th>
                            <th>Temp. (F)</th>
                            <th>Summary</th>|]
                                </tr>
                            </thead>
                        </table>
                    }
                    """,
            expected: """
                    @if (true)
                    {
                        <p><em>Loading...</em></p>
                    }
                    else
                    {
                        <table class="table">
                            <thead>
                                <tr>
                                    <th>Date</th>
                                    <th>Temp. (C)</th>
                                    <th>Temp. (F)</th>
                                    <th>Summary</th>
                                </tr>
                            </thead>
                        </table>
                    }
                    """);
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/razor/issues/5749")]
    public async Task FormatRenderFragmentInCSharpCodeBlock()
    {
        // Sadly the first thing the HTML formatter does with this input
        // is put a newline after the @, which means <SurveyPrompt /> won't be
        // seen as a component any more, so we have to turn off our validation,
        // or the test fails before we have a chance to fix the formatting.
        FormattingContext.SkipValidateComponents = true;

        await RunFormattingTestAsync(
            input: """
                    @code
                    {
                        public void DoStuff(RenderFragment renderFragment)
                        {
                            renderFragment(@<SurveyPrompt Title="Foo" />);

                            @* comment *@
                    <div></div>

                            @* comment *@<div></div>
                        }
                    }
                    """,
            expected: """
                    @code
                    {
                        public void DoStuff(RenderFragment renderFragment)
                        {
                            renderFragment(@<SurveyPrompt Title="Foo" />);

                            @* comment *@
                            <div></div>

                            @* comment *@
                            <div></div>
                        }
                    }
                    """);
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/razor/issues/6090")]
    public async Task FormatHtmlCommentsInsideCSharp1()
    {
        await RunFormattingTestAsync(
            input: """
                    @foreach (var num in Enumerable.Range(1, 10))
                    {
                        <span class="skill_result btn">
                            <!--asdfasd-->
                            <span style="margin-left:0px">
                                <svg>
                                    <rect width="1" height="1" />
                                </svg>
                            </span>
                            <!--adfasfd-->
                        </span>
                    }
                    """,
            expected: """
                    @foreach (var num in Enumerable.Range(1, 10))
                    {
                        <span class="skill_result btn">
                            <!--asdfasd-->
                            <span style="margin-left:0px">
                                <svg>
                                    <rect width="1" height="1" />
                                </svg>
                            </span>
                            <!--adfasfd-->
                        </span>
                    }
                    """);
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/razor/issues/6090")]
    public async Task FormatHtmlCommentsInsideCSharp2()
    {
        await RunFormattingTestAsync(
            input: """
                    @foreach (var num in Enumerable.Range(1, 10))
                    {
                        <span class="skill_result btn">
                            <!--asdfasd-->
                            <input type="text" />
                            <!--adfasfd-->
                        </span>
                    }
                    """,
            expected: """
                    @foreach (var num in Enumerable.Range(1, 10))
                    {
                        <span class="skill_result btn">
                            <!--asdfasd-->
                            <input type="text" />
                            <!--adfasfd-->
                        </span>
                    }
                    """);
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/razor/issues/6001")]
    public async Task FormatNestedCascadingValue()
    {
        await RunFormattingTestAsync(
            input: """
                    @using Microsoft.AspNetCore.Components.Forms;

                    @if (Object1!= null)
                    {
                        <CascadingValue Value="Variable1">
                            <CascadingValue Value="Variable2">
                                <SurveyPrompt  />
                                @if (VarBool)
                            {
                                <div class="mb-16">
                                    <SurveyPrompt  />
                                    <SurveyPrompt  />
                                </div>
                            }
                        </CascadingValue>
                    </CascadingValue>
                    }

                    @code
                    {
                        public object Object1 {get;set;}
                        public object Variable1 {get;set;}
                    public object Variable2 {get;set;}
                    public bool VarBool {get;set;}
                    }
                    """,
            expected: """
                    @using Microsoft.AspNetCore.Components.Forms;

                    @if (Object1 != null)
                    {
                        <CascadingValue Value="Variable1">
                            <CascadingValue Value="Variable2">
                                <SurveyPrompt />
                                @if (VarBool)
                                {
                                    <div class="mb-16">
                                        <SurveyPrompt />
                                        <SurveyPrompt />
                                    </div>
                                }
                            </CascadingValue>
                        </CascadingValue>
                    }

                    @code
                    {
                        public object Object1 { get; set; }
                        public object Variable1 { get; set; }
                        public object Variable2 { get; set; }
                        public bool VarBool { get; set; }
                    }
                    """,
            fileKind: FileKinds.Component);
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/razor/issues/6001")]
    public async Task FormatNestedCascadingValue2()
    {
        await RunFormattingTestAsync(
            input: """
                    @using Microsoft.AspNetCore.Components.Forms;

                    @if (Object1!= null)
                    {
                        <CascadingValue Value="Variable1">
                                <SurveyPrompt  />
                                @if (VarBool)
                            {
                                <div class="mb-16">
                                    <SurveyPrompt  />
                                    <SurveyPrompt  />
                                </div>
                            }
                    </CascadingValue>
                    }

                    @code
                    {
                        public object Object1 {get;set;}
                        public object Variable1 {get;set;}
                    public object Variable2 {get;set;}
                    public bool VarBool {get;set;}
                    }
                    """,
            expected: """
                    @using Microsoft.AspNetCore.Components.Forms;

                    @if (Object1 != null)
                    {
                        <CascadingValue Value="Variable1">
                            <SurveyPrompt />
                            @if (VarBool)
                            {
                                <div class="mb-16">
                                    <SurveyPrompt />
                                    <SurveyPrompt />
                                </div>
                            }
                        </CascadingValue>
                    }

                    @code
                    {
                        public object Object1 { get; set; }
                        public object Variable1 { get; set; }
                        public object Variable2 { get; set; }
                        public bool VarBool { get; set; }
                    }
                    """,
            fileKind: FileKinds.Component,
            skipFlipLineEndingTest: true); // tracked by https://github.com/dotnet/razor/issues/10836
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/razor/issues/6001")]
    public async Task FormatNestedCascadingValue3()
    {
        await RunFormattingTestAsync(
            input: """
                    @using Microsoft.AspNetCore.Components.Forms;

                    @if (Object1!= null)
                    {
                        @if (VarBool)
                        {
                                <SurveyPrompt  />
                                @if (VarBool)
                            {
                                <div class="mb-16">
                                    <SurveyPrompt  />
                                    <SurveyPrompt  />
                                </div>
                            }
                    }
                    }

                    @code
                    {
                        public object Object1 {get;set;}
                        public object Variable1 {get;set;}
                    public object Variable2 {get;set;}
                    public bool VarBool {get;set;}
                    }
                    """,
            expected: """
                    @using Microsoft.AspNetCore.Components.Forms;

                    @if (Object1 != null)
                    {
                        @if (VarBool)
                        {
                            <SurveyPrompt />
                            @if (VarBool)
                            {
                                <div class="mb-16">
                                    <SurveyPrompt />
                                    <SurveyPrompt />
                                </div>
                            }
                        }
                    }

                    @code
                    {
                        public object Object1 { get; set; }
                        public object Variable1 { get; set; }
                        public object Variable2 { get; set; }
                        public bool VarBool { get; set; }
                    }
                    """,
            fileKind: FileKinds.Component);
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/razor/issues/6001")]
    public async Task FormatNestedCascadingValue4()
    {
        await RunFormattingTestAsync(
            input: """
                    @using Microsoft.AspNetCore.Components.Forms;

                        <CascadingValue Value="Variable1">
                                <SurveyPrompt  />
                                @if (VarBool)
                            {
                                <div class="mb-16">
                                    <SurveyPrompt  />
                                    <SurveyPrompt  />
                                </div>
                            }
                    </CascadingValue>

                    @code
                    {
                        public object Object1 {get;set;}
                        public object Variable1 {get;set;}
                    public object Variable2 {get;set;}
                    public bool VarBool {get;set;}
                    }
                    """,
            expected: """
                    @using Microsoft.AspNetCore.Components.Forms;

                    <CascadingValue Value="Variable1">
                        <SurveyPrompt />
                        @if (VarBool)
                        {
                            <div class="mb-16">
                                <SurveyPrompt />
                                <SurveyPrompt />
                            </div>
                        }
                    </CascadingValue>

                    @code
                    {
                        public object Object1 { get; set; }
                        public object Variable1 { get; set; }
                        public object Variable2 { get; set; }
                        public bool VarBool { get; set; }
                    }
                    """,
            fileKind: FileKinds.Component,
            skipFlipLineEndingTest: true); // tracked by https://github.com/dotnet/razor/issues/10836
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/razor/issues/6001")]
    public async Task FormatNestedCascadingValue5()
    {
        await RunFormattingTestAsync(
            input: """
                    @using Microsoft.AspNetCore.Components.Forms;

                    @if (Object1!= null)
                    {
                        <PageTitle>
                                <SurveyPrompt  />
                                @if (VarBool)
                            {
                                <div class="mb-16">
                                    <SurveyPrompt  />
                                    <SurveyPrompt  />
                                </div>
                            }
                    </PageTitle>
                    }

                    @code
                    {
                        public object Object1 {get;set;}
                        public object Variable1 {get;set;}
                    public object Variable2 {get;set;}
                    public bool VarBool {get;set;}
                    }
                    """,
            expected: """
                    @using Microsoft.AspNetCore.Components.Forms;

                    @if (Object1 != null)
                    {
                        <PageTitle>
                            <SurveyPrompt />
                            @if (VarBool)
                            {
                                <div class="mb-16">
                                    <SurveyPrompt />
                                    <SurveyPrompt />
                                </div>
                            }
                        </PageTitle>
                    }

                    @code
                    {
                        public object Object1 { get; set; }
                        public object Variable1 { get; set; }
                        public object Variable2 { get; set; }
                        public bool VarBool { get; set; }
                    }
                    """,
            fileKind: FileKinds.Component,
            skipFlipLineEndingTest: true); // tracked by https://github.com/dotnet/razor/issues/10836
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/razor/issues/5676")]
    public async Task FormatInputSelect()
    {
        await RunFormattingTestAsync(
            input: """
                    @using Microsoft.AspNetCore.Components.Forms;

                    @code {
                        private string _id {get;set;}
                    }

                    <div>
                        @if (true)
                        {
                            <div>
                                <InputSelect @bind-Value="_id">
                                    @if (true)
                                    {
                                        <option>goo</option>
                                    }
                                </InputSelect>
                            </div>
                        }
                    </div>
                    """,
            expected: """
                    @using Microsoft.AspNetCore.Components.Forms;

                    @code {
                        private string _id { get; set; }
                    }

                    <div>
                        @if (true)
                        {
                            <div>
                                <InputSelect @bind-Value="_id">
                                    @if (true)
                                    {
                                        <option>goo</option>
                                    }
                                </InputSelect>
                            </div>
                        }
                    </div>
                    """,
            fileKind: FileKinds.Component);
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/razor/issues/5676")]
    public async Task FormatInputSelect2()
    {
        await RunFormattingTestAsync(
            input: """
                    @using Microsoft.AspNetCore.Components.Forms;

                    @code {
                        private string _id {get;set;}
                    }

                    <div>
                            <div>
                                <InputSelect @bind-Value="_id">
                                    @if (true)
                                    {
                                        <option>goo</option>
                                    }
                                </InputSelect>
                            </div>
                    </div>
                    """,
            expected: """
                    @using Microsoft.AspNetCore.Components.Forms;

                    @code {
                        private string _id { get; set; }
                    }

                    <div>
                        <div>
                            <InputSelect @bind-Value="_id">
                                @if (true)
                                {
                                    <option>goo</option>
                                }
                            </InputSelect>
                        </div>
                    </div>
                    """,
            fileKind: FileKinds.Component);
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/razor/issues/5676")]
    public async Task FormatInputSelect3()
    {
        await RunFormattingTestAsync(
            input: """
                    @using Microsoft.AspNetCore.Components.Forms;

                    @code {
                        private string _id {get;set;}
                    }

                    <div>
                            <div>
                                <InputSelect @bind-Value="_id">
                                        <option>goo</option>
                                </InputSelect>
                            </div>
                    </div>
                    """,
            expected: """
                    @using Microsoft.AspNetCore.Components.Forms;

                    @code {
                        private string _id { get; set; }
                    }

                    <div>
                        <div>
                            <InputSelect @bind-Value="_id">
                                <option>goo</option>
                            </InputSelect>
                        </div>
                    </div>
                    """,
            fileKind: FileKinds.Component);
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/razor/issues/5676")]
    public async Task FormatInputSelect4()
    {
        await RunFormattingTestAsync(
            input: """
                    @using Microsoft.AspNetCore.Components.Forms;

                    @code {
                        private string _id {get;set;}
                    }

                    <div>
                        @if (true)
                        {
                            <div>
                                <InputSelect @bind-Value="_id">
                                        <option>goo</option>
                                </InputSelect>
                            </div>
                        }
                    </div>
                    """,
            expected: """
                    @using Microsoft.AspNetCore.Components.Forms;

                    @code {
                        private string _id { get; set; }
                    }

                    <div>
                        @if (true)
                        {
                            <div>
                                <InputSelect @bind-Value="_id">
                                    <option>goo</option>
                                </InputSelect>
                            </div>
                        }
                    </div>
                    """,
            fileKind: FileKinds.Component);
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/razor/issues/8606")]
    public async Task FormatAttributesWithTransition()
    {
        await RunFormattingTestAsync(
            input: """
                    @using Microsoft.AspNetCore.Components.Forms;

                    @code {
                        private string _id {get;set;}
                    }

                    <div>
                        @if (true)
                        {
                            <div>
                                <InputSelect CssClass="goo"
                                     @bind-Value="_id"
                                   @ref="elem"
                                    CurrentValue="boo">
                                        <option>goo</option>
                                </InputSelect>
                            </div>
                        }
                    </div>
                    """,
            expected: """
                    @using Microsoft.AspNetCore.Components.Forms;

                    @code {
                        private string _id { get; set; }
                    }

                    <div>
                        @if (true)
                        {
                            <div>
                                <InputSelect CssClass="goo"
                                             @bind-Value="_id"
                                             @ref="elem"
                                             CurrentValue="boo">
                                    <option>goo</option>
                                </InputSelect>
                            </div>
                        }
                    </div>
                    """,
            fileKind: FileKinds.Component);
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/razor/issues/9337")]
    public async Task FormatMinimizedTagHelperAttributes()
    {
        await RunFormattingTestAsync(
            input: """
                    @using Microsoft.AspNetCore.Components.Forms;

                    @code {
                        private bool _id {get;set;}
                    }

                    <div>
                        @if (true)
                        {
                            <div>
                                <InputCheckbox CssClass="goo"
                                   Value
                                   accesskey="F" />
                            </div>
                        }
                    </div>
                    """,
            expected: """
                    @using Microsoft.AspNetCore.Components.Forms;

                    @code {
                        private bool _id { get; set; }
                    }

                    <div>
                        @if (true)
                        {
                            <div>
                                <InputCheckbox CssClass="goo"
                                               Value
                                               accesskey="F" />
                            </div>
                        }
                    </div>
                    """,
            fileKind: FileKinds.Component);
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/razor/issues/6211")]
    public async Task FormatCascadingValueWithCascadingTypeParameter()
    {
        await RunFormattingTestAsync(
            input: """

                    <div>
                        @foreach ( var i in new int[] { 1, 23 } )
                        {
                            <div></div>
                        }
                    </div>
                    <Select TValue="string">
                        @foreach ( var i in new int[] { 1, 23 } )
                        {
                            <SelectItem Value="@i">@i</SelectItem>
                        }
                    </Select>
                    """,
            expected: """

                    <div>
                        @foreach (var i in new int[] { 1, 23 })
                        {
                            <div></div>
                        }
                    </div>
                    <Select TValue="string">
                        @foreach (var i in new int[] { 1, 23 })
                        {
                            <SelectItem Value="@i">@i</SelectItem>
                        }
                    </Select>
                    """,
            tagHelpers: CreateTagHelpers(),
            skipFlipLineEndingTest: true); // tracked by https://github.com/dotnet/razor/issues/10836

        ImmutableArray<TagHelperDescriptor> CreateTagHelpers()
        {
            var select = """
                    @typeparam TValue
                    @attribute [CascadingTypeParameter(nameof(TValue))]
                    <CascadingValue Value="@this" IsFixed>
                        <select>
                            @ChildContent
                        </select>
                    </CascadingValue>

                    @code
                    {
                        [Parameter] public TValue SelectedValue { get; set; }
                    }
                    """;
            var selectItem = """
                    @typeparam TValue
                    <option value="@StringValue">@ChildContent</option>

                    @code
                    {
                        [Parameter] public TValue Value { get; set; }
                        [Parameter] public RenderFragment ChildContent { get; set; }

                        protected string StringValue => Value?.ToString();
                    }
                    """;

            var selectComponent = CompileToCSharp("Select.razor", select, throwOnFailure: true, fileKind: FileKinds.Component);
            var selectItemComponent = CompileToCSharp("SelectItem.razor", selectItem, throwOnFailure: true, fileKind: FileKinds.Component);

            using var _ = ArrayBuilderPool<TagHelperDescriptor>.GetPooledObject(out var tagHelpers);
            tagHelpers.AddRange(selectComponent.CodeDocument.GetTagHelperContext().TagHelpers);
            tagHelpers.AddRange(selectItemComponent.CodeDocument.GetTagHelperContext().TagHelpers);

            return tagHelpers.ToImmutable();
        }
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/razor/issues/6110")]
    public async Task FormatExplicitCSharpInsideHtml()
    {
        await RunFormattingTestAsync(
            input: """
                    @using System.Text;

                    <div>
                        @(new C()
                                .M("Hello")
                            .M("World")
                            .M(source =>
                            {
                            if (source.Length > 0)
                            {
                            source.ToString();
                            }
                            }))

                        @(DateTime.Now)

                        @(DateTime
                    .Now
                    .ToString())

                                    @(   Html.DisplayNameFor (@<text>
                            <p >
                            <h2 ></h2>
                            </p>
                            </text>)
                            .ToString())

                    @{
                    var x = @<p>Hi there!</p>
                    }
                    @x()
                    @(@x())
                    </div>

                    @functions {
                        class C
                        {
                            C M(string a) => this;
                            C M(Func<string, C> a) => this;
                        }
                    }
                    """,
            expected: """
                    @using System.Text;

                    <div>
                        @(new C()
                            .M("Hello")
                            .M("World")
                            .M(source =>
                            {
                                if (source.Length > 0)
                                {
                                    source.ToString();
                                }
                            }))

                        @(DateTime.Now)

                        @(DateTime
                            .Now
                            .ToString())

                        @(Html.DisplayNameFor(@<text>
                            <p>
                                <h2></h2>
                            </p>
                        </text>)
                            .ToString())

                        @{
                            var x = @<p>Hi there!</p>
                        }
                        @x()
                        @(@x())
                    </div>

                    @functions {
                        class C
                        {
                            C M(string a) => this;
                            C M(Func<string, C> a) => this;
                        }
                    }
                    """,
            fileKind: FileKinds.Legacy);
    }

    [Fact]
    public async Task RazorDiagnostics_SkipRangeFormatting()
    {
        await RunFormattingTestAsync(
            input: """
                    @page "Goo"

                    <div></div>

                    [|<button|]
                    @functions {
                     void M() { }
                    }
                    """,
            expected: """
                    @page "Goo"

                    <div></div>

                    <button
                    @functions {
                     void M() { }
                    }
                    """,
            allowDiagnostics: true);
    }

    [Fact]
    public async Task RazorDiagnostics_DontSkipDocumentFormatting()
    {
        // Yes this format result looks wrong, but this is only done in direct response
        // to user action, and they can always undo it.
        await RunFormattingTestAsync(
            input: """
                    <button
                    @functions {
                     void M() { }
                    }
                    """,
            expected: """
                    <button @functions {
                            void M() { }
                            }
                    """,
            allowDiagnostics: true);
    }

    [Fact]
    public async Task RazorDiagnostics_SkipRangeFormatting_WholeDocumentRange()
    {
        await RunFormattingTestAsync(
            input: """
                    [|<button
                    @functions {
                     void M() { }
                    }|]
                    """,
            expected: """
                    <button
                    @functions {
                     void M() { }
                    }
                    """,
            allowDiagnostics: true);
    }

    [Fact]
    public async Task RazorDiagnostics_DontSkipWhenOutsideOfRange()
    {
        await RunFormattingTestAsync(
            input: """
                    @page "Goo"

                    [|      <div></div>|]

                    <button
                    @functions {
                     void M() { }
                    }
                    """,
            expected: """
                    @page "Goo"

                    <div></div>

                    <button
                    @functions {
                     void M() { }
                    }
                    """,
            allowDiagnostics: true);
    }

    [Fact]
    public async Task FormatIndentedElementAttributes()
    {
        await RunFormattingTestAsync(
            input: """
                    Welcome.

                    <div class="goo"
                     align="center">
                    </div>

                    <SurveyPrompt Title="How is Blazor working for you?"
                     Color="Red" />

                    @if (true)
                    {
                    <div class="goo"
                     align="center">
                    </div>

                    <SurveyPrompt Title="How is Blazor working for you?"
                       Color="Red" />

                       <tag attr1="value1"
                       attr2="value2"
                       attr3="value3"
                       />

                       @if (true)
                       {
                       @if (true)
                       {
                       @if(true)
                       {
                       <table width="10"
                       height="10"
                       cols="3"
                       rows="3">
                       </table>
                       }
                       }
                       }
                    }
                    """,
            expected: """
                    Welcome.

                    <div class="goo"
                         align="center">
                    </div>

                    <SurveyPrompt Title="How is Blazor working for you?"
                                  Color="Red" />

                    @if (true)
                    {
                        <div class="goo"
                             align="center">
                        </div>

                        <SurveyPrompt Title="How is Blazor working for you?"
                                      Color="Red" />

                        <tag attr1="value1"
                             attr2="value2"
                             attr3="value3" />

                        @if (true)
                        {
                            @if (true)
                            {
                                @if (true)
                                {
                                    <table width="10"
                                           height="10"
                                           cols="3"
                                           rows="3">
                                    </table>
                                }
                            }
                        }
                    }
                    """);
    }

    private ImmutableArray<TagHelperDescriptor> GetComponents()
    {
        AdditionalSyntaxTrees.Add(Parse("""
                using Microsoft.AspNetCore.Components;
                namespace Test
                {
                    public class GridTable : ComponentBase
                    {
                        [Parameter]
                        public RenderFragment ChildContent { get; set; }
                    }

                    public class GridRow : ComponentBase
                    {
                        [Parameter]
                        public RenderFragment ChildContent { get; set; }
                    }

                    public class GridCell : ComponentBase
                    {
                        [Parameter]
                        public RenderFragment ChildContent { get; set; }
                    }

                    public class Component1 : ComponentBase
                    {
                        [Parameter]
                        public string Id { get; set; }

                        [Parameter]
                        public string Caption { get; set; }

                        [Parameter]
                        public RenderFragment Frag {get;set;}
                    }
                }
                """));

        var generated = CompileToCSharp("Test.razor", string.Empty, throwOnFailure: false, fileKind: FileKinds.Component);

        return generated.CodeDocument.GetTagHelperContext().TagHelpers.ToImmutableArray();
    }
}
