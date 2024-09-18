// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis.Razor.Formatting;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Formatting;

[Collection(HtmlFormattingCollection.Name)]
public class CodeDirectiveFormattingTest(HtmlFormattingFixture fixture, ITestOutputHelper testOutput)
    : FormattingTestBase(fixture.Service, testOutput)
{
    internal override bool UseTwoPhaseCompilation => true;

    internal override bool DesignTime => true;

    [Fact]
    public async Task FormatsCodeBlockDirective()
    {
        await RunFormattingTestAsync(
            input: """
                    @code {
                     public class Foo{}
                            public interface Bar {
                    }
                    }
                    """,
            expected: """
                    @code {
                        public class Foo { }
                        public interface Bar
                        {
                        }
                    }
                    """);
    }

    [Fact]
    public async Task FormatCSharpInsideHtmlTag()
    {
        await RunFormattingTestAsync(
            input: """
                    <html>
                    <body>
                    <div>
                    @{
                    <span>foo</span>
                    <span>foo</span>
                    }
                    </div>
                    </body>
                    </html>
                    """,
            expected: """
                    <html>
                    <body>
                        <div>
                            @{
                                <span>foo</span>
                                <span>foo</span>
                            }
                        </div>
                    </body>
                    </html>
                    """);
    }

    [Fact]
    public async Task Format_DocumentWithDiagnostics()
    {
        await RunFormattingTestAsync(
            input: """
                    @page
                    @model BlazorApp58.Pages.Index2Model
                    @{
                    }

                    <section class="section">
                        <div class="container">
                            <h1 class="title">Managed pohotos</h1>
                            <p class="subtitle">@Model.ReferenceNumber</p>
                        </div>
                    </section>
                    <section class="section">
                        <div class="container">
                            @foreach       (var item in Model.Images)
                            {
                                <div><div>
                            }
                        </div>
                    </section>
                    """,
            expected: """
                    @page
                    @model BlazorApp58.Pages.Index2Model
                    @{
                    }

                    <section class="section">
                        <div class="container">
                            <h1 class="title">Managed pohotos</h1>
                            <p class="subtitle">@Model.ReferenceNumber</p>
                        </div>
                    </section>
                    <section class="section">
                        <div class="container">
                            @foreach (var item in Model.Images)
                            {
                                <div>
                                    <div>
                                        }
                                    </div>
                        </section>
                    """,
            fileKind: FileKinds.Legacy,
            allowDiagnostics: true);
    }

    [Fact]
    public async Task Formats_MultipleBlocksInADirective()
    {
        await RunFormattingTestAsync(
            input: """
                    @{
                    void Method(){
                    var x = "foo";
                    @(DateTime.Now)
                        <p></p>
                    var y= "fooo";
                    }
                    }
                    <div>
                            </div>
                    """,
            expected: """
                    @{
                        void Method()
                        {
                            var x = "foo";
                            @(DateTime.Now)
                            <p></p>
                            var y = "fooo";
                        }
                    }
                    <div>
                    </div>
                    """);
    }

    [Fact]
    public async Task Formats_NonCodeBlockDirectives()
    {
        await RunFormattingTestAsync(
            input: """
                    @{
                    var x = "foo";
                    }
                    <div>
                            </div>
                    """,
            expected: """
                    @{
                        var x = "foo";
                    }
                    <div>
                    </div>
                    """);
    }

    [Fact]
    public async Task Formats_CodeBlockDirectiveWithMarkup_NonBraced()
    {
        await RunFormattingTestAsync(
            input: """
                    @functions {
                     public class Foo{
                    void Method() { var x = "t"; <div></div> var y = "t";}
                    }
                    }
                    """,
            expected: """
                    @functions {
                        public class Foo
                        {
                            void Method()
                            {
                                var x = "t";
                                <div></div>
                                var y = "t";
                            }
                        }
                    }
                    """);
    }

    [Fact]
    public async Task Formats_CodeBlockDirectiveWithMarkup()
    {
        await RunFormattingTestAsync(
            input: """
                    @functions {
                     public class Foo{
                    void Method() { <div></div> }
                    }
                    }
                    """,
            expected: """
                    @functions {
                        public class Foo
                        {
                            void Method()
                            {
                                <div></div>
                            }
                        }
                    }
                    """);
    }

    [Fact]
    public async Task Formats_CodeBlockDirectiveWithImplicitExpressions()
    {
        await RunFormattingTestAsync(
            input: """
                    @code {
                     public class Foo{
                    void Method() { @DateTime.Now }
                        }
                    }
                    """,
            expected: """
                    @code {
                        public class Foo
                        {
                            void Method()
                            {
                                @DateTime.Now
                            }
                        }
                    }
                    """);
    }

    [Fact]
    public async Task DoesNotFormat_CodeBlockDirectiveWithExplicitExpressions()
    {
        await RunFormattingTestAsync(
            input: """
                    @functions {
                     public class Foo{
                    void Method() { @(DateTime.Now) }
                        }
                    }
                    """,
            expected: """
                    @functions {
                        public class Foo
                        {
                            void Method()
                            {
                                @(DateTime.Now)
                            }
                        }
                    }
                    """,
            fileKind: FileKinds.Legacy);
    }

    [Fact]
    public async Task Format_SectionDirectiveBlock1()
    {
        await RunFormattingTestAsync(
            input: """
                    @functions {
                     public class Foo{
                    void Method() {  }
                        }
                    }

                    @section Scripts {
                    <script></script>
                    }
                    """,
            expected: """
                    @functions {
                        public class Foo
                        {
                            void Method() { }
                        }
                    }

                    @section Scripts {
                        <script></script>
                    }
                    """,
            fileKind: FileKinds.Legacy);
    }

    [Fact]
    public async Task Format_SectionDirectiveBlock2()
    {
        await RunFormattingTestAsync(
            input: """
                    @functions {
                     public class Foo{
                    void Method() {  }
                        }
                    }

                    @section Scripts {
                    <script>
                        function f() {
                        }
                    </script>
                    }
                    """,
            expected: """
                    @functions {
                        public class Foo
                        {
                            void Method() { }
                        }
                    }

                    @section Scripts {
                        <script>
                            function f() {
                            }
                        </script>
                    }
                    """,
            fileKind: FileKinds.Legacy);
    }

    [Fact]
    public async Task Format_SectionDirectiveBlock3()
    {
        await RunFormattingTestAsync(
            input: """
                    @functions {
                     public class Foo{
                    void Method() {  }
                        }
                    }

                    @section Scripts {
                    <p>this is a para</p>
                    @if(true)
                    {
                    <p>and so is this</p>
                    }
                    }
                    """,
            expected: """
                    @functions {
                        public class Foo
                        {
                            void Method() { }
                        }
                    }

                    @section Scripts {
                        <p>this is a para</p>
                        @if (true)
                        {
                            <p>and so is this</p>
                        }
                    }
                    """,
            fileKind: FileKinds.Legacy);
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/razor-tooling/issues/6401")]
    public async Task Format_SectionDirectiveBlock4()
    {
        await RunFormattingTestAsync(
            input: """
                    @functions {
                     public class Foo{
                    void Method() {  }
                        }
                    }

                    @section Scripts {
                    <script></script>
                    }

                    @if (true)
                    {
                        <p></p>
                    }
                    """,
            expected: """
                    @functions {
                        public class Foo
                        {
                            void Method() { }
                        }
                    }

                    @section Scripts {
                        <script></script>
                    }

                    @if (true)
                    {
                        <p></p>
                    }
                    """,
            fileKind: FileKinds.Legacy);
    }

    [Fact]
    public async Task Format_SectionDirectiveBlock5()
    {
        await RunFormattingTestAsync(
            input: """
                    @functions {
                     public class Foo{
                    void Method() {  }
                        }
                    }

                    @section Foo {
                        @{ var test = 1; }
                    }

                    <p></p>

                    @section Scripts {
                    <script></script>
                    }

                    <p></p>
                    """,
            expected: """
                    @functions {
                        public class Foo
                        {
                            void Method() { }
                        }
                    }

                    @section Foo {
                        @{
                            var test = 1;
                        }
                    }

                    <p></p>

                    @section Scripts {
                        <script></script>
                    }

                    <p></p>
                    """,
            fileKind: FileKinds.Legacy);
    }

    [Fact]
    public async Task Format_SectionDirectiveBlock6()
    {
        await RunFormattingTestAsync(
            input: """
                    @functions {
                     public class Foo{
                    void Method() {  }
                        }
                    }

                    @section Scripts {
                    <meta property="a" content="b">
                    <meta property="a" content="b"/>
                    <meta property="a" content="b">

                    @if(true)
                    {
                    <p>this is a paragraph</p>
                    }
                    }
                    """,
            expected: """
                    @functions {
                        public class Foo
                        {
                            void Method() { }
                        }
                    }

                    @section Scripts {
                        <meta property="a" content="b">
                        <meta property="a" content="b" />
                        <meta property="a" content="b">

                        @if (true)
                        {
                            <p>this is a paragraph</p>
                        }
                    }
                    """,
            fileKind: FileKinds.Legacy);
    }

    [Fact]
    public async Task Format_SectionDirectiveBlock7()
    {
        await RunFormattingTestAsync(
            input: """
                    @functions {
                     public class Foo{
                    void Method() {  }
                        }
                    }

                    @section Scripts
                    {
                    <meta property="a" content="b">
                    <meta property="a" content="b"/>
                    <meta property="a" content="b">

                    @if(true)
                    {
                    <p>this is a paragraph</p>
                    }
                    }
                    """,
            expected: """
                    @functions {
                        public class Foo
                        {
                            void Method() { }
                        }
                    }

                    @section Scripts
                    {
                        <meta property="a" content="b">
                        <meta property="a" content="b" />
                        <meta property="a" content="b">

                        @if (true)
                        {
                            <p>this is a paragraph</p>
                        }
                    }
                    """,
            fileKind: FileKinds.Legacy);
    }

    [Fact]
    public async Task Formats_CodeBlockDirectiveWithRazorComments()
    {
        await RunFormattingTestAsync(
            input: """
                    @functions {
                     public class Foo{
                    @* This is a Razor Comment *@
                    void Method() {  }
                    }
                    }
                    """,
            expected: """
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
    public async Task Formats_CodeBlockDirectiveWithRazorStatements()
    {
        await RunFormattingTestAsync(
            input: """
                    @functions {
                     public class Foo{
                    @* This is a Razor Comment *@
                        }
                    }
                    """,
            expected: """
                    @functions {
                        public class Foo
                        {
                            @* This is a Razor Comment *@
                        }
                    }
                    """);
    }

    [Fact]
    public async Task DoesNotFormat_CodeBlockDirective_NotInSelectedRange()
    {
        await RunFormattingTestAsync(
            input: """
                    [|<div>Foo</div>|]
                    @functions {
                     public class Foo{}
                            public interface Bar {
                    }
                    }
                    """,
            expected: """
                    <div>Foo</div>
                    @functions {
                     public class Foo{}
                            public interface Bar {
                    }
                    }
                    """);
    }

    [Fact]
    public async Task OnlyFormatsWithinRange()
    {
        await RunFormattingTestAsync(
            input: """
                    @functions {
                     public class Foo{}
                            [|public interface Bar {
                    }|]
                    }
                    """,
            expected: """
                    @functions {
                     public class Foo{}
                        public interface Bar
                        {
                        }
                    }
                    """);
    }

    [Fact]
    public async Task MultipleCodeBlockDirectives()
    {
        await RunFormattingTestAsync(
            input: """
                    @functions {
                     public class Foo{}
                            public interface Bar {
                    }
                    }
                    Hello World
                    @functions {
                          public class Baz    {
                              void Method ( )
                              { }
                              }
                    }
                    """,
            expected: """
                    @functions {
                        public class Foo { }
                        public interface Bar
                        {
                        }
                    }
                    Hello World
                    @functions {
                        public class Baz
                        {
                            void Method()
                            { }
                        }
                    }
                    """,
            fileKind: FileKinds.Legacy);
    }

    [Fact]
    public async Task MultipleCodeBlockDirectives2()
    {
        await RunFormattingTestAsync(
            input: """
                    Hello World
                    @code {
                    public class HelloWorld
                    {
                    }
                    }

                    @functions{

                        public class Bar {}
                    }
                    """,
            expected: """
                    Hello World
                    @code {
                        public class HelloWorld
                        {
                        }
                    }

                    @functions {

                        public class Bar { }
                    }
                    """);
    }

    [Fact]
    public async Task CodeOnTheSameLineAsCodeBlockDirectiveStart()
    {
        await RunFormattingTestAsync(
            input: """
                    @functions {public class Foo{
                    }
                    }
                    """,
            expected: """
                    @functions {
                        public class Foo
                        {
                        }
                    }
                    """);
    }

    [Fact]
    public async Task CodeOnTheSameLineAsCodeBlockDirectiveEnd()
    {
        await RunFormattingTestAsync(
            input: """
                    @functions {
                    public class Foo{
                    }}
                    """,
            expected: """
                    @functions {
                        public class Foo
                        {
                        }
                    }
                    """);
    }

    [Fact]
    public async Task SingleLineCodeBlockDirective()
    {
        await RunFormattingTestAsync(
        input: """
                @functions {public class Foo{}
                }
                """,
        expected: """
                @functions {
                    public class Foo { }
                }
                """);
    }

    [Fact]
    public async Task IndentsCodeBlockDirectiveStart()
    {
        await RunFormattingTestAsync(
            input: """
                    Hello World
                         @functions {public class Foo{}
                    }
                    """,
            expected: """
                    Hello World
                    @functions {
                        public class Foo { }
                    }
                    """);
    }

    [Fact]
    public async Task IndentsCodeBlockDirectiveEnd()
    {
        await RunFormattingTestAsync(
            input: """
                    @functions {
                    public class Foo{}
                         }
                    """,
            expected: """
                    @functions {
                        public class Foo { }
                    }
                    """);
    }

    [Fact]
    public async Task ComplexCodeBlockDirective()
    {
        await RunFormattingTestAsync(
            input: """
                    @using System.Buffers
                    @functions{
                         public class Foo
                                {
                                    public Foo()
                                    {
                                        var arr = new string[ ] { "One", "two","three" };
                                        var str = @"
                    This should
                    not
                    be indented.
                    ";
                                    }
                    public int MyProperty { get
                    {
                    return 0 ;
                    } set {} }

                    void Method(){

                    }
                                        }
                    }
                    """,
            expected: """
                    @using System.Buffers
                    @functions {
                        public class Foo
                        {
                            public Foo()
                            {
                                var arr = new string[] { "One", "two", "three" };
                                var str = @"
                    This should
                    not
                    be indented.
                    ";
                            }
                            public int MyProperty
                            {
                                get
                                {
                                    return 0;
                                }
                                set { }
                            }

                            void Method()
                            {

                            }
                        }
                    }
                    """);
    }

    [Fact]
    public async Task Strings()
    {
        await RunFormattingTestAsync(
            input: """
                    @functions{
                    private string str1 = "hello world";
                    private string str2 = $"hello world";
                    private string str3 = @"hello world";
                    private string str4 = $@"hello world";
                    private string str5 = @"
                        One
                            Two
                                Three
                    ";
                    private string str6 = $@"
                        One
                            Two
                                Three
                    ";
                    // This looks wrong, but matches what the C# formatter does. Try it and see!
                    private string str7 = "One" +
                        "Two" +
                            "Three" +
                    "";
                    }
                    """,
            expected: """
                    @functions {
                        private string str1 = "hello world";
                        private string str2 = $"hello world";
                        private string str3 = @"hello world";
                        private string str4 = $@"hello world";
                        private string str5 = @"
                        One
                            Two
                                Three
                    ";
                        private string str6 = $@"
                        One
                            Two
                                Three
                    ";
                        // This looks wrong, but matches what the C# formatter does. Try it and see!
                        private string str7 = "One" +
                            "Two" +
                                "Three" +
                        "";
                    }
                    """);
    }

    [Fact]
    public async Task CodeBlockDirective_UseTabs()
    {
        await RunFormattingTestAsync(
            input: """
                    @code {
                     public class Foo{}
                            void Method(  ) {
                    }
                    }
                    """,
            expected: """
                    @code {
                    	public class Foo { }
                    	void Method()
                    	{
                    	}
                    }
                    """,
            insertSpaces: false);

    }
    [Fact]
    public async Task CodeBlockDirective_UseTabsWithTabSize8_HTML()
    {
        await RunFormattingTestAsync(
            input: """
                    @code {
                     public class Foo{}
                            void Method(  ) {<div></div>
                    }
                    }
                    """,
            expected: """
                    @code {
                    	public class Foo { }
                    	void Method()
                    	{
                    		<div></div>
                    	}
                    }
                    """,
            tabSize: 8,
            insertSpaces: false);
    }

    [Fact]
    public async Task CodeBlockDirective_UseTabsWithTabSize8()
    {
        await RunFormattingTestAsync(
            input: """
                    @code {
                     public class Foo{}
                            void Method(  ) {
                    }
                    }
                    """,
            expected: """
                    @code {
                    	public class Foo { }
                    	void Method()
                    	{
                    	}
                    }
                    """,
            tabSize: 8,
            insertSpaces: false);
    }

    [Fact]
    public async Task CodeBlockDirective_WithTabSize3()
    {
        await RunFormattingTestAsync(
            input: """
                    @code {
                     public class Foo{}
                            void Method(  ) {
                    }
                    }
                    """,
            expected: """
                    @code {
                       public class Foo { }
                       void Method()
                       {
                       }
                    }
                    """,
            tabSize: 3);
    }

    [Fact]
    public async Task CodeBlockDirective_WithTabSize8()
    {
        await RunFormattingTestAsync(
            input: """
                    @code {
                     public class Foo{}
                            void Method(  ) {
                    }
                    }
                    """,
            expected: """
                    @code {
                            public class Foo { }
                            void Method()
                            {
                            }
                    }
                    """,
            tabSize: 8);
    }

    [Fact]
    public async Task CodeBlockDirective_WithTabSize12()
    {
        await RunFormattingTestAsync(
            input: """
                    @code {
                     public class Foo{}
                            void Method(  ) {
                    }
                    }
                    """,
            expected: """
                    @code {
                                public class Foo { }
                                void Method()
                                {
                                }
                    }
                    """,
            tabSize: 12);
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/aspnetcore/issues/27102")]
    public async Task CodeBlock_SemiColon_SingleLine()
    {
        await RunFormattingTestAsync(
            input: """
                    <div></div>
                    @{ Debugger.Launch()$$;}
                    <div></div>
                    """,
            expected: """
                    <div></div>
                    @{
                        Debugger.Launch();
                    }
                    <div></div>
                    """);
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/aspnetcore/issues/29837")]
    public async Task CodeBlock_NestedComponents()
    {
        await RunFormattingTestAsync(
            input: """
                    @code {
                        private WeatherForecast[] forecasts;

                        protected override async Task OnInitializedAsync()
                        {
                            <Counter>
                                @{
                                        var t = DateTime.Now;
                                        t.ToString();
                                    }
                                </Counter>
                            forecasts = await ForecastService.GetForecastAsync(DateTime.Now);
                        }
                    }
                    """,
            expected: """
                    @code {
                        private WeatherForecast[] forecasts;

                        protected override async Task OnInitializedAsync()
                        {
                            <Counter>
                                @{
                                    var t = DateTime.Now;
                                    t.ToString();
                                }
                            </Counter>
                            forecasts = await ForecastService.GetForecastAsync(DateTime.Now);
                        }
                    }
                    """);
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/aspnetcore/issues/34320")]
    public async Task CodeBlock_ObjectCollectionArrayInitializers()
    {
        // The C# Formatter doesn't touch these types of initializers, so nor do we. This test
        // just verifies we don't regress things and start moving code around.
        await RunFormattingTestAsync(
            input: """
                    @code {
                        public List<object> AList = new List<object>()
                        {
                            new
                            {
                                Name = "One",
                                Goo = new
                                {
                                    First = 1,
                                    Second = 2
                                },
                                Bar = new string[] {
                                    "Hello",
                                    "There"
                                },
                                Baz = new string[]
                                {
                                    "Hello",
                                    "There"
                                }
                            }
                        };
                    }
                    """,
            expected: """
                    @code {
                        public List<object> AList = new List<object>()
                        {
                            new
                            {
                                Name = "One",
                                Goo = new
                                {
                                    First = 1,
                                    Second = 2
                                },
                                Bar = new string[] {
                                    "Hello",
                                    "There"
                                },
                                Baz = new string[]
                                {
                                    "Hello",
                                    "There"
                                }
                            }
                        };
                    }
                    """);
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/razor-tooling/issues/6548")]
    public async Task CodeBlock_ImplicitObjectArrayInitializers()
    {
        // The C# Formatter doesn't touch these types of initializers, so nor do we. This test
        // just verifies we don't regress things and start moving code around.
        await RunFormattingTestAsync(
            input: """
                    @code {
                        private object _x = new()
                            {
                                Name = "One",
                                Goo = new
                                {
                                    First = 1,
                                    Second = 2
                                },
                                Bar = new string[]
                                {
                                    "Hello",
                                    "There"
                                },
                            };
                    }
                    """,
            expected: """
                    @code {
                        private object _x = new()
                            {
                                Name = "One",
                                Goo = new
                                {
                                    First = 1,
                                    Second = 2
                                },
                                Bar = new string[]
                                {
                                    "Hello",
                                    "There"
                                },
                            };
                    }
                    """);
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/razor/issues/7058")]
    public async Task CodeBlock_ImplicitArrayInitializers()
    {
        await RunFormattingTestAsync(
            input: """
                    @code {
                        private void M()
                        {
                            var entries = new[]
                            {
                                "a",
                                "b",
                                "c"
                            };
                        }
                    }
                    """,
            expected: """
                    @code {
                        private void M()
                        {
                            var entries = new[]
                            {
                                "a",
                                "b",
                                "c"
                            };
                        }
                    }
                    """);
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/razor-tooling/issues/6092")]
    public async Task CodeBlock_ArrayInitializers()
    {
        // The C# Formatter doesn't touch these types of initializers, so nor do we. This test
        // just verifies we don't regress things and start moving code around.
        await RunFormattingTestAsync(
            input: """
                    @code {
                        private void M()
                        {
                            var entries = new string[]
                            {
                                "a",
                                "b",
                                "c"
                            };
                        }
                    }
                    """,
            expected: """
                    @code {
                        private void M()
                        {
                            var entries = new string[]
                            {
                                "a",
                                "b",
                                "c"
                            };
                        }
                    }
                    """);
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/razor-tooling/issues/6548")]
    public async Task CodeBlock_ArrayInitializers2()
    {
        // The C# Formatter doesn't touch these types of initializers, so nor do we. This test
        // just verifies we don't regress things and start moving code around.
        await RunFormattingTestAsync(
            input: """
                    <p></p>

                    @code {
                        private void M()
                        {
                            var entries = new string[]
                            {
                                "a",
                                "b",
                                "c"
                            };

                            object gridOptions = new()
                                {
                                    Columns = new GridColumn<WorkOrderModel>[]
                                    {
                                        new TextColumn<WorkOrderModel>(e => e.Name) { Label = "Work Order #" },
                                        new TextColumn<WorkOrderModel>(e => e.PartNumber) { Label = "Part #" },
                                        new TextColumn<WorkOrderModel>(e => e.Lot) { Label = "Lot #" },
                                                new DateTimeColumn<WorkOrderModel>(e => e.TargetStartOn) { Label = "Target Start" },
                                    },
                                    Data = Model.WorkOrders,
                                    Title = "Work Orders"
                                };
                        }
                    }
                    """,
            expected: """
                    <p></p>
                    
                    @code {
                        private void M()
                        {
                            var entries = new string[]
                            {
                                "a",
                                "b",
                                "c"
                            };
                    
                            object gridOptions = new()
                                {
                                    Columns = new GridColumn<WorkOrderModel>[]
                                    {
                                        new TextColumn<WorkOrderModel>(e => e.Name) { Label = "Work Order #" },
                                        new TextColumn<WorkOrderModel>(e => e.PartNumber) { Label = "Part #" },
                                        new TextColumn<WorkOrderModel>(e => e.Lot) { Label = "Lot #" },
                                                new DateTimeColumn<WorkOrderModel>(e => e.TargetStartOn) { Label = "Target Start" },
                                    },
                                    Data = Model.WorkOrders,
                                    Title = "Work Orders"
                                };
                        }
                    }
                    """);
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/razor-tooling/issues/6092")]
    public async Task CodeBlock_CollectionArrayInitializers()
    {
        // The C# Formatter doesn't touch these types of initializers, so nor do we. This test
        // just verifies we don't regress things and start moving code around.
        await RunFormattingTestAsync(
            input: """
                    @code {
                        private void M()
                        {
                            var entries = new List<string[]>()
                            {
                                new string[]
                                {
                                    "Hello",
                                    "There"
                                },
                                new string[] {
                                    "Hello",
                                    "There"
                                },
                                new string[]
                                {
                                    "Hello",
                                    "There"
                                }
                            };
                        }
                    }
                    """,
            expected: """
                    @code {
                        private void M()
                        {
                            var entries = new List<string[]>()
                            {
                                new string[]
                                {
                                    "Hello",
                                    "There"
                                },
                                new string[] {
                                    "Hello",
                                    "There"
                                },
                                new string[]
                                {
                                    "Hello",
                                    "There"
                                }
                            };
                        }
                    }
                    """);
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/razor-tooling/issues/6092")]
    public async Task CodeBlock_ObjectInitializers()
    {
        // The C# Formatter doesn't touch these types of initializers, so nor do we. This test
        // just verifies we don't regress things and start moving code around.
        await RunFormattingTestAsync(
            input: """
                    @code {
                        private void M()
                        {
                            var entries = new
                            {
                                First = 1,
                                Second = 2
                            };
                        }
                    }
                    """,
            expected: """
                    @code {
                        private void M()
                        {
                            var entries = new
                            {
                                First = 1,
                                Second = 2
                            };
                        }
                    }
                    """);
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/razor-tooling/issues/6092")]
    public async Task CodeBlock_ImplicitObjectInitializers()
    {
        // The C# Formatter doesn't touch these types of initializers, so nor do we. This test
        // just verifies we don't regress things and start moving code around.
        await RunFormattingTestAsync(
            input: """
                    @code {
                        private void M()
                        {
                            object entries = new()
                                {
                                    First = 1,
                                    Second = 2
                                };
                        }
                    }
                    """,
            expected: """
                    @code {
                        private void M()
                        {
                            object entries = new()
                                {
                                    First = 1,
                                    Second = 2
                                };
                        }
                    }
                    """);
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/razor-tooling/issues/6092")]
    public async Task CodeBlock_CollectionInitializers()
    {
        // The C# Formatter doesn't touch these types of initializers, so nor do we. This test
        // just verifies we don't regress things and start moving code around.
        await RunFormattingTestAsync(
            input: """
                    @code {
                        private void M()
                        {
                            var entries = new List<string>()
                            {
                                "a",
                                "b",
                                "c"
                            };
                        }
                    }
                    """,
            expected: """
                    @code {
                        private void M()
                        {
                            var entries = new List<string>()
                            {
                                "a",
                                "b",
                                "c"
                            };
                        }
                    }
                    """);
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/razor-tooling/issues/5618")]
    public async Task CodeBlock_EmptyObjectCollectionInitializers()
    {
        // The C# Formatter _does_ touch these types of initializers if they're empty. Who knew ¯\_(ツ)_/¯
        await RunFormattingTestAsync(
            input: """
                    @code {
                        public void Foo()
                        {
                            SomeMethod(new List<string>()
                                {

                                });

                            SomeMethod(new Exception
                                {

                                });
                        }
                    }
                    """,
            expected: """
                    @code {
                        public void Foo()
                        {
                            SomeMethod(new List<string>()
                            {

                            });

                            SomeMethod(new Exception
                            {

                            });
                        }
                    }
                    """);
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/aspnetcore/issues/4498")]
    public async Task IfBlock_TopLevel()
    {
        await RunFormattingTestAsync(
            input: """
                            @if (true)
                    {
                    }
                    """,
            expected: """
                    @if (true)
                    {
                    }
                    """);
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/aspnetcore/issues/4498")]
    public async Task IfBlock_TopLevel_WithOtherCode()
    {
        await RunFormattingTestAsync(
            input: """
                    @{
                        // foo
                    }

                            @if (true)
                    {
                    }
                    """,
            expected: """
                    @{
                        // foo
                    }

                    @if (true)
                    {
                    }
                    """);
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/aspnetcore/issues/4498")]
    public async Task IfBlock_Nested()
    {
        await RunFormattingTestAsync(
            input: """
                    <div>
                            @if (true)
                    {
                    }
                    </div>
                    """,
            expected: """
                    <div>
                        @if (true)
                        {
                        }
                    </div>
                    """);
    }

    [Fact]
    public async Task IfBlock_Nested_Contents()
    {
        await RunFormattingTestAsync(
            input: """
                    <div>
                    <div></div>
                            @if (true)
                    {
                    <div></div>
                    }
                    <div></div>
                    </div>
                    """,
            expected: """
                    <div>
                        <div></div>
                        @if (true)
                        {
                            <div></div>
                        }
                        <div></div>
                    </div>
                    """);
    }

    [Fact]
    public async Task IfBlock_SingleLine_Nested_Contents()
    {
        await RunFormattingTestAsync(
            input: """
                    <div>
                    <div></div>
                            @if (true) { <div></div> }
                    <div></div>
                    </div>
                    """,
            expected: """
                    <div>
                        <div></div>
                        @if (true)
                        {
                            <div></div>
                        }
                        <div></div>
                    </div>
                    """);
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/razor-tooling/issues/5648")]
    public async Task GenericComponentWithCascadingTypeParameter()
    {
        await RunFormattingTestAsync(
            input: """
                    @page "/counter"

                    @if(true)
                        {
                                    // indented
                            }

                    <TestGeneric Items="_items">
                        @foreach (var v in System.Linq.Enumerable.Range(1, 10))
                            {
                                <div></div>
                            }
                        </TestGeneric>

                    @if(true)
                        {
                                    // indented
                                }

                    @code
                        {
                        private IEnumerable<int> _items = new[] { 1, 2, 3, 4, 5 };
                    }
                    """,
            expected: """
                    @page "/counter"

                    @if (true)
                    {
                        // indented
                    }

                    <TestGeneric Items="_items">
                        @foreach (var v in System.Linq.Enumerable.Range(1, 10))
                        {
                            <div></div>
                        }
                    </TestGeneric>

                    @if (true)
                    {
                        // indented
                    }

                    @code
                    {
                        private IEnumerable<int> _items = new[] { 1, 2, 3, 4, 5 };
                    }
                    """,
            tagHelpers: GetComponentWithCascadingTypeParameter(),
            skipFlipLineEndingTest: true);
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/razor-tooling/issues/5648")]
    public async Task GenericComponentWithCascadingTypeParameter_Nested()
    {
        await RunFormattingTestAsync(
            input: """
                    @page "/counter"

                    <TestGeneric Items="_items">
                        @foreach (var v in System.Linq.Enumerable.Range(1, 10))
                            {
                                <div></div>
                            }
                    <TestGeneric Items="_items">
                        @foreach (var v in System.Linq.Enumerable.Range(1, 10))
                            {
                                <div></div>
                            }
                        </TestGeneric>
                        </TestGeneric>

                    @code
                        {
                        private IEnumerable<int> _items = new[] { 1, 2, 3, 4, 5 };
                    }
                    """,
            expected: """
                    @page "/counter"

                    <TestGeneric Items="_items">
                        @foreach (var v in System.Linq.Enumerable.Range(1, 10))
                        {
                            <div></div>
                        }
                        <TestGeneric Items="_items">
                            @foreach (var v in System.Linq.Enumerable.Range(1, 10))
                            {
                                <div></div>
                            }
                        </TestGeneric>
                    </TestGeneric>

                    @code
                    {
                        private IEnumerable<int> _items = new[] { 1, 2, 3, 4, 5 };
                    }
                    """,
            tagHelpers: GetComponentWithCascadingTypeParameter());
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/razor-tooling/issues/5648")]
    public async Task GenericComponentWithCascadingTypeParameter_MultipleParameters()
    {
        await RunFormattingTestAsync(
            input: """
                    @page "/counter"

                    <TestGenericTwo Items="_items" ItemsTwo="_items2">
                        @foreach (var v in System.Linq.Enumerable.Range(1, 10))
                            {
                                <div></div>
                            }
                        </TestGenericTwo>

                    @code
                        {
                        private IEnumerable<int> _items = new[] { 1, 2, 3, 4, 5 };
                        private IEnumerable<long> _items2 = new long[] { 1, 2, 3, 4, 5 };
                    }
                    """,
            expected: """
                    @page "/counter"

                    <TestGenericTwo Items="_items" ItemsTwo="_items2">
                        @foreach (var v in System.Linq.Enumerable.Range(1, 10))
                        {
                            <div></div>
                        }
                    </TestGenericTwo>

                    @code
                    {
                        private IEnumerable<int> _items = new[] { 1, 2, 3, 4, 5 };
                        private IEnumerable<long> _items2 = new long[] { 1, 2, 3, 4, 5 };
                    }
                    """,
            tagHelpers: GetComponentWithTwoCascadingTypeParameter(),
            skipFlipLineEndingTest: true); // tracked by https://github.com/dotnet/razor/issues/10836
    }

    [Fact]
    public async Task Formats_MultilineExpressions()
    {
        await RunFormattingTestAsync(
            input: """
                    @{
                        var icon = "/images/bootstrap-icons.svg#"
                            + GetIconName(login.ProviderDisplayName!);

                        var x = DateTime
                                .Now
                            .ToString();
                    }

                    @code
                    {
                        public void M()
                        {
                            var icon2 = "/images/bootstrap-icons.svg#"
                                + GetIconName(login.ProviderDisplayName!);
                    
                            var x2 = DateTime
                                    .Now
                                .ToString();
                        }
                    }
                    """,
            expected: """
                    @{
                        var icon = "/images/bootstrap-icons.svg#"
                            + GetIconName(login.ProviderDisplayName!);

                        var x = DateTime
                                .Now
                            .ToString();
                    }
                    
                    @code
                    {
                        public void M()
                        {
                            var icon2 = "/images/bootstrap-icons.svg#"
                                + GetIconName(login.ProviderDisplayName!);
                    
                            var x2 = DateTime
                                    .Now
                                .ToString();
                        }
                    }
                    """);
    }

    [Fact]
    public async Task Formats_MultilineExpressionAtStartOfBlock()
    {
        await RunFormattingTestAsync(
            input: """
                    @{
                        var x = DateTime
                            .Now
                            .ToString();
                    }
                    """,
            expected: """
                    @{
                        var x = DateTime
                            .Now
                            .ToString();
                    }
                    """);
    }

    [Fact]
    public async Task Formats_MultilineExpressionAfterWhitespaceAtStartOfBlock()
    {
        await RunFormattingTestAsync(
            input: """
                    @{



                        var x = DateTime
                            .Now
                            .ToString();
                    }
                    """,
            expected: """
                    @{



                        var x = DateTime
                            .Now
                            .ToString();
                    }
                    """);
    }

    [Fact]
    public async Task Formats_MultilineExpressionNotAtStartOfBlock()
    {
        await RunFormattingTestAsync(
            input: """
                    @{
                        //
                        var x = DateTime
                            .Now
                            .ToString();
                    }
                    """,
            expected: """
                    @{
                        //
                        var x = DateTime
                            .Now
                            .ToString();
                    }
                    """);
    }

    [Fact]
    public async Task Formats_MultilineRazorComment()
    {
        await RunFormattingTestAsync(
            input: """
                    <div></div>
                        @*
                    line 1
                      line 2
                        line 3
                                *@
                    @code
                    {
                        void M()
                        {
                        @*
                    line 1
                      line 2
                        line 3
                                    *@
                        }
                    }
                    """,
            expected: """
                    <div></div>
                    @*
                    line 1
                      line 2
                        line 3
                                *@
                    @code
                    {
                        void M()
                        {
                            @*
                    line 1
                      line 2
                        line 3
                                    *@
                        }
                    }
                    """);
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/razor-tooling/issues/6192")]
    public async Task Formats_NoEditsForNoChanges()
    {
        var input = """
                @code {
                    public void M()
                    {
                        Console.WriteLine("Hello");
                        Console.WriteLine("World"); // <-- type/replace semicolon here
                    }
                }

                """;

        await RunFormattingTestAsync(input, input, fileKind: FileKinds.Component);
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/razor-tooling/issues/6158")]
    public async Task Format_NestedLambdas()
    {
        await RunFormattingTestAsync(
            input: """
                    @code {

                        protected Action Goo(string input)
                        {
                            return async () =>
                            {
                            foreach (var x in input)
                            {
                            if (true)
                            {
                            await Task.Delay(1);

                            if (true)
                            {
                            // do some stufff
                            if (true)
                            {
                            }
                            }
                            }
                            }
                            };
                        }
                    }
                    """,
            expected: """
                    @code {

                        protected Action Goo(string input)
                        {
                            return async () =>
                            {
                                foreach (var x in input)
                                {
                                    if (true)
                                    {
                                        await Task.Delay(1);

                                        if (true)
                                        {
                                            // do some stufff
                                            if (true)
                                            {
                                            }
                                        }
                                    }
                                }
                            };
                        }
                    }
                    """);
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/razor-tooling/issues/5693")]
    public async Task Format_NestedLambdasWithAtIf()
    {
        await RunFormattingTestAsync(
            input: """
                    @code {

                        public RenderFragment RenderFoo()
                        {
                            return (__builder) =>
                            {
                                @if (true) { }
                            };
                        }
                    }
                    """,
            expected: """
                    @code {

                        public RenderFragment RenderFoo()
                        {
                            return (__builder) =>
                            {
                                @if (true) { }
                            };
                        }
                    }
                    """);
    }

    private ImmutableArray<TagHelperDescriptor> GetComponentWithCascadingTypeParameter()
    {
        var input = """
                @using System.Collections.Generic
                @using Microsoft.AspNetCore.Components
                @typeparam TItem
                @attribute [CascadingTypeParameter(nameof(TItem))]

                <h3>TestGeneric</h3>

                @code
                {
                    [Parameter] public IEnumerable<TItem> Items { get; set; }
                    [Parameter] public RenderFragment ChildContent { get; set; }
                }
                """;

        var generated = CompileToCSharp("TestGeneric.razor", input, throwOnFailure: true, fileKind: FileKinds.Component);

        return generated.CodeDocument.GetTagHelperContext().TagHelpers.ToImmutableArray();
    }

    private ImmutableArray<TagHelperDescriptor> GetComponentWithTwoCascadingTypeParameter()
    {
        var input = """
                @using System.Collections.Generic
                @using Microsoft.AspNetCore.Components
                @typeparam TItem
                @typeparam TItemTwo
                @attribute [CascadingTypeParameter(nameof(TItem))]
                @attribute [CascadingTypeParameter(nameof(TItemTwo))]

                <h3>TestGeneric</h3>

                @code
                {
                    [Parameter] public IEnumerable<TItem> Items { get; set; }
                    [Parameter] public IEnumerable<TItemTwo> ItemsTwo { get; set; }
                    [Parameter] public RenderFragment ChildContent { get; set; }
                }
                """;

        var generated = CompileToCSharp("TestGenericTwo.razor", input, throwOnFailure: true, fileKind: FileKinds.Component);

        return generated.CodeDocument.GetTagHelperContext().TagHelpers.ToImmutableArray();
    }
}
