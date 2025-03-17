// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis.Razor.Formatting;
using Xunit;
using Xunit.Abstractions;

#if COHOSTING
namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;
#else
namespace Microsoft.AspNetCore.Razor.LanguageServer.Formatting;
#endif

[Collection(HtmlFormattingCollection.Name)]
public class DocumentFormattingTest(FormattingTestContext context, HtmlFormattingFixture fixture, ITestOutputHelper testOutput)
    : FormattingTestBase(context, fixture.Service, testOutput), IClassFixture<FormattingTestContext>
{
    private readonly bool _useNewFormattingEngine = context.UseNewFormattingEngine;

    [FormattingTestFact]
    public async Task EmptyDocument()
    {
        await RunFormattingTestAsync(
            input: "",
            expected: "");
    }

    [FormattingTestFact]
    public async Task AllWhitespaceDocument()
    {
        // The Html formatter shrinks this down to one line
        await RunFormattingTestAsync(
            input: """

                
                

            """,
            expected: """

            """);
    }

    [FormattingTestFact]
    public async Task StartsWithWhitespace()
    {
        await RunFormattingTestAsync(
            input: """

                

            <div></div>

            """,
            expected: """
            
            
            
            <div></div>
            
            """);
    }

    [FormattingTestFact]
    public async Task EndsWithWhitespace()
    {
        await RunFormattingTestAsync(
            input: """
                <div></div>

                

                """,
            expected: """
                <div></div>
                
                
                
                """);
    }

    [FormattingTestFact]
    public async Task Section_BraceOnNextLine()
    {
        await RunFormattingTestAsync(
            input: """
                    @section    Scripts
                        {
                    <meta property="a" content="b">
                    }
                    """,
            expected: """
                    @section Scripts
                    {
                        <meta property="a" content="b">
                    }
                    """,
            fileKind: FileKinds.Legacy);
    }

    [FormattingTestFact]
    public async Task Section_BraceOnSameLine()
    {
        await RunFormattingTestAsync(
            input: """
                    @section        Scripts                         {
                    <meta property="a" content="b">
                    }
                    """,
            expected: """
                    @section Scripts {
                        <meta property="a" content="b">
                    }
                    """,
            fileKind: FileKinds.Legacy);
    }

    [FormattingTestTheory, CombinatorialData]
    public async Task CodeBlock_SpansMultipleLines(bool inGlobalNamespace)
    {
        await RunFormattingTestAsync(
            input: """
                    @code
                            {
                        private int currentCount = 0;

                        private void IncrementCount()
                        {
                            currentCount++;
                        }
                    }
                    """,
            expected: """
                    @code
                    {
                        private int currentCount = 0;

                        private void IncrementCount()
                        {
                            currentCount++;
                        }
                    }
                    """,
            inGlobalNamespace: inGlobalNamespace);
    }

    [FormattingTestTheory, CombinatorialData]
    public async Task CodeBlock_IndentedBlock_MaintainsIndent(bool inGlobalNamespace)
    {
        await RunFormattingTestAsync(
            input: """
                    <boo>
                        @code
                                {
                            private int currentCount = 0;

                            private void IncrementCount()
                            {
                                currentCount++;
                            }
                        }
                    </boo>
                    """,
            expected: """
                    <boo>
                        @code
                        {
                            private int currentCount = 0;

                            private void IncrementCount()
                            {
                                currentCount++;
                            }
                        }
                    </boo>
                    """,
            inGlobalNamespace: inGlobalNamespace);
    }

    [FormattingTestFact]
    public async Task CodeBlock_IndentedBlock_FixCloseBrace()
    {
        await RunFormattingTestAsync(
            input: """
                    <boo>
                        @code
                                {
                            private int currentCount = 0;

                            private void IncrementCount()
                            {
                                currentCount++;
                            }
                                            }
                    </boo>
                    """,
            expected: """
                    <boo>
                        @code
                        {
                            private int currentCount = 0;

                            private void IncrementCount()
                            {
                                currentCount++;
                            }
                        }
                    </boo>
                    """);
    }

    [FormattingTestFact]
    public async Task CodeBlock_IndentedBlock_FixCloseBrace2()
    {
        await RunFormattingTestAsync(
            input: """
                    <boo>
                    @code
                            {
                        private int currentCount = 0;

                        private void IncrementCount()
                        {
                            currentCount++;
                        }
                    }
                    </boo>
                    """,
            expected: """
                    <boo>
                        @code
                        {
                            private int currentCount = 0;

                            private void IncrementCount()
                            {
                                currentCount++;
                            }
                        }
                    </boo>
                    """);
    }

    [FormattingTestFact]
    public async Task CodeBlock_FixCloseBrace()
    {
        await RunFormattingTestAsync(
            input: """
                    @code        {
                        private int currentCount = 0;

                        private void IncrementCount()
                        {
                            currentCount++;
                        }
                        }
                    """,
            expected: """
                    @code {
                        private int currentCount = 0;

                        private void IncrementCount()
                        {
                            currentCount++;
                        }
                    }
                    """);
    }

    [FormattingTestFact]
    public async Task CodeBlock_FixCloseBrace2()
    {
        await RunFormattingTestAsync(
            input: """
                    @code        {
                        private int currentCount = 0;

                        private void IncrementCount()
                        {
                            currentCount++;
                        }                        }
                    """,
            expected: """
                    @code {
                        private int currentCount = 0;

                        private void IncrementCount()
                        {
                            currentCount++;
                        }
                    }
                    """);
    }

    [FormattingTestFact]
    public async Task CodeBlock_FixCloseBrace3()
    {
        await RunFormattingTestAsync(
            input: """
                    @code        {
                        private int currentCount = 0;

                        private void IncrementCount()
                        {
                            currentCount++;
                        }
                        }
                    """,
            expected: """
                    @code
                    {
                        private int currentCount = 0;

                        private void IncrementCount()
                        {
                            currentCount++;
                        }
                    }
                    """,
            codeBlockBraceOnNextLine: true);
    }

    [FormattingTestFact]
    public async Task CodeBlock_FixCloseBrace4()
    {
        await RunFormattingTestAsync(
            input: """
                    @code        {
                        private int currentCount = 0;

                        private void IncrementCount()
                        {
                            currentCount++;
                        }                        }
                    """,
            expected: """
                    @code
                    {
                        private int currentCount = 0;

                        private void IncrementCount()
                        {
                            currentCount++;
                        }
                    }
                    """,
            codeBlockBraceOnNextLine: true);
    }

    [FormattingTestFact]
    public async Task CodeBlock_TooMuchWhitespace()
    {
        await RunFormattingTestAsync(
            input: """
                    @code        {
                        private int currentCount = 0;

                        private void IncrementCount()
                        {
                            currentCount++;
                        }
                    }
                    """,
            expected: """
                    @code {
                        private int currentCount = 0;

                        private void IncrementCount()
                        {
                            currentCount++;
                        }
                    }
                    """);
    }

    [FormattingTestFact]
    public async Task CodeBlock_NonSpaceWhitespace()
    {
        await RunFormattingTestAsync(
            input: """
                    @code	{
                        private int currentCount = 0;

                        private void IncrementCount()
                        {
                            currentCount++;
                        }
                    }
                    """,
            expected: """
                    @code {
                        private int currentCount = 0;

                        private void IncrementCount()
                        {
                            currentCount++;
                        }
                    }
                    """);
    }

    [FormattingTestFact]
    public async Task CodeBlock_NonSpaceWhitespace2()
    {
        await RunFormattingTestAsync(
            input: """
                    @code	{
                        private int currentCount = 0;

                        private void IncrementCount()
                        {
                            currentCount++;
                        }
                    }
                    """,
            expected: """
                    @code
                    {
                        private int currentCount = 0;

                        private void IncrementCount()
                        {
                            currentCount++;
                        }
                    }
                    """,
            codeBlockBraceOnNextLine: true);
    }

    [FormattingTestFact]
    public async Task CodeBlock_NoWhitespace()
    {
        await RunFormattingTestAsync(
            input: """
                    @code{
                        private int currentCount = 0;

                        private void IncrementCount()
                        {
                            currentCount++;
                        }
                    }
                    """,
            expected: """
                    @code {
                        private int currentCount = 0;

                        private void IncrementCount()
                        {
                            currentCount++;
                        }
                    }
                    """);
    }

    [FormattingTestFact]
    public async Task CodeBlock_NoWhitespace2()
    {
        await RunFormattingTestAsync(
            input: """
                    @code{
                        private int currentCount = 0;

                        private void IncrementCount()
                        {
                            currentCount++;
                        }
                    }
                    """,
            expected: """
                    @code
                    {
                        private int currentCount = 0;

                        private void IncrementCount()
                        {
                            currentCount++;
                        }
                    }
                    """,
            codeBlockBraceOnNextLine: true);
    }

    [FormattingTestFact]
    public async Task FunctionsBlock_BraceOnNewLine()
    {
        await RunFormattingTestAsync(
            input: """
                    @functions
                            {
                        private int currentCount = 0;

                        private void IncrementCount()
                        {
                            currentCount++;
                        }
                    }
                    """,
            expected: """
                    @functions
                    {
                        private int currentCount = 0;

                        private void IncrementCount()
                        {
                            currentCount++;
                        }
                    }
                    """,
            fileKind: FileKinds.Legacy);
    }

    [FormattingTestFact]
    public async Task FunctionsBlock_TooManySpaces()
    {
        await RunFormattingTestAsync(
            input: """
                    @functions        {
                        private int currentCount = 0;

                        private void IncrementCount()
                        {
                            currentCount++;
                        }
                    }
                    """,
            expected: """
                    @functions {
                        private int currentCount = 0;

                        private void IncrementCount()
                        {
                            currentCount++;
                        }
                    }
                    """,
            fileKind: FileKinds.Legacy);
    }

    [FormattingTestFact]
    public async Task FunctionsBlock_TooManySpaces2()
    {
        await RunFormattingTestAsync(
            input: """
                    @functions        {
                        private int currentCount = 0;

                        private void IncrementCount()
                        {
                            currentCount++;
                        }
                    }
                    """,
            expected: """
                    @functions
                    {
                        private int currentCount = 0;

                        private void IncrementCount()
                        {
                            currentCount++;
                        }
                    }
                    """,
            fileKind: FileKinds.Legacy,
            codeBlockBraceOnNextLine: true);
    }

    [FormattingTestFact]
    public async Task FunctionsBlock_FixCloseBrace()
    {
        await RunFormattingTestAsync(
            input: """
                    @functions        {
                        private int currentCount = 0;

                        private void IncrementCount()
                        {
                            currentCount++;
                        }
                             }
                    """,
            expected: """
                    @functions {
                        private int currentCount = 0;

                        private void IncrementCount()
                        {
                            currentCount++;
                        }
                    }
                    """,
            fileKind: FileKinds.Legacy);
    }

    [FormattingTestFact]
    public async Task FunctionsBlock_FixCloseBrace2()
    {
        await RunFormattingTestAsync(
            input: """
                    @functions        {
                        private int currentCount = 0;

                        private void IncrementCount()
                        {
                            currentCount++;
                        }                             }
                    """,
            expected: """
                    @functions {
                        private int currentCount = 0;

                        private void IncrementCount()
                        {
                            currentCount++;
                        }
                    }
                    """,
            fileKind: FileKinds.Legacy);
    }

    [FormattingTestFact]
    public async Task FunctionsBlock_Tabs_FixCloseBrace()
    {
        await RunFormattingTestAsync(
            input: """
                    @functions        {
                    	private int currentCount = 0;

                    	private void IncrementCount()
                    	{
                    		currentCount++;
                    	}
                    				}
                    """,
            expected: """
                    @functions {
                    	private int currentCount = 0;

                    	private void IncrementCount()
                    	{
                    		currentCount++;
                    	}
                    }
                    """,
            insertSpaces: false,
            tabSize: 8,
            fileKind: FileKinds.Legacy);
    }

    [FormattingTestFact]
    public async Task Layout()
    {
        await RunFormattingTestAsync(
            input: """
                    @layout    MyLayout
                    """,
            expected: """
                    @layout MyLayout
                    """);
    }

    [FormattingTestFact]
    public async Task Inherits()
    {
        await RunFormattingTestAsync(
            input: """
                    @inherits    MyBaseClass
                    """,
            expected: """
                    @inherits MyBaseClass
                    """);
    }

    [FormattingTestFact]
    public async Task Implements()
    {
        await RunFormattingTestAsync(
            input: """
                    @implements    IDisposable
                    """,
            expected: """
                    @implements IDisposable
                    """);
    }

    [FormattingTestFact]
    public async Task PreserveWhitespace()
    {
        await RunFormattingTestAsync(
            input: """
                    @preservewhitespace    true
                    """,
            expected: """
                    @preservewhitespace true
                    """);
    }

    [FormattingTestFact]
    public async Task Inject()
    {
        await RunFormattingTestAsync(
            input: """
                    @inject    MyClass     myClass
                    """,
            expected: """
                    @inject MyClass myClass
                    """);
    }

    [FormattingTestFact]
    public async Task Inject_TrailingWhitespace()
    {
        await RunFormattingTestAsync(
            input: """
                    @inject    MyClass     myClass
                    """,
            expected: """
                    @inject MyClass myClass
                    """);
    }

    [FormattingTestFact]
    public async Task Attribute1()
    {
        await RunFormattingTestAsync(
            input: """
                    @attribute     [Obsolete(   "asdf"   , error:    false)]
                    """,
            expected: """
                    @attribute [Obsolete("asdf", error: false)]
                    """);
    }

    [FormattingTestFact]
    public async Task Attribute2()
    {
        await RunFormattingTestAsync(
            input: """
                    @attribute     [Attr(   "asdf"   , error:    false)]
                    @attribute   [Attribute(   "asdf"   , error:    false)]
                    @attribute [ALongAttributeName(   "asdf"   , error:    false)]
                    """,
            expected: """
                    @attribute [Attr("asdf", error: false)]
                    @attribute [Attribute("asdf", error: false)]
                    @attribute [ALongAttributeName("asdf", error: false)]
                    """);
    }

    [FormattingTestFact]
    public async Task Attribute3()
    {
        await RunFormattingTestAsync(
            input: """
                    <div></div>
                    @attribute     [Obsolete(   "asdf"   , error:    false)]
                    <div></div>
                    @attribute     [Obsolete(   "asdf"   , error:    false)]
                    <div></div>
                    @attribute     [Obsolete(   "asdf"   , error:    false)]
                    <div></div>
                    """,
            expected: """
                    <div></div>
                    @attribute [Obsolete("asdf", error: false)]
                    <div></div>
                    @attribute [Obsolete("asdf", error: false)]
                    <div></div>
                    @attribute [Obsolete("asdf", error: false)]
                    <div></div>
                    """);
    }

    [FormattingTestFact]
    public async Task TypeParam_Unconstrained()
    {
        await RunFormattingTestAsync(
            input: """
                    @typeparam     T
                    """,
            expected: """
                    @typeparam T
                    """);
    }

    [FormattingTestFact]
    public async Task TypeParam1()
    {
        await RunFormattingTestAsync(
            input: """
                    @typeparam     T     where    T    :   IDisposable
                    """,
            expected: """
                    @typeparam T where T : IDisposable
                    """);
    }

    [FormattingTestFact]
    public async Task TypeParam2()
    {
        await RunFormattingTestAsync(
            input: """
                    @typeparam     TItem     where    TItem    :   IDisposable
                    """,
            expected: """
                    @typeparam TItem where TItem : IDisposable
                    """);
    }

    [FormattingTestFact]
    public async Task TypeParam3()
    {
        await RunFormattingTestAsync(
            input: """
                    @using System
                    @typeparam     TItem     where    TItem    :   IDisposable

                    <div>
                    @{
                    if (true)
                    {
                    // Hello
                    }
                    }
                    </div>
                    """,
            expected: """
                    @using System
                    @typeparam TItem where TItem : IDisposable
                    
                    <div>
                        @{
                            if (true)
                            {
                                // Hello
                            }
                        }
                    </div>
                    """);
    }

    [FormattingTestFact]
    public async Task TypeParam4()
    {
        await RunFormattingTestAsync(
            input: """
                    @using System
                    @typeparam     TItem     where    TItem    :   IDisposable
                    @typeparam TParent where TParent : string

                    @if (true)
                    {
                    // Hello
                    }
                    """,
            expected: """
                    @using System
                    @typeparam TItem where TItem : IDisposable
                    @typeparam TParent where TParent : string
                    
                    @if (true)
                    {
                        // Hello
                    }
                    """);
    }

    [FormattingTestFact]
    public async Task Model()
    {
        await RunFormattingTestAsync(
            input: """
                    @model    MyModel
                    """,
            expected: """
                    @model MyModel
                    """,
            fileKind: FileKinds.Legacy);
    }

    [FormattingTestFact]
    public async Task Page()
    {
        await RunFormattingTestAsync(
            input: """
                    @page    "MyPage"
                    """,
            expected: """
                    @page "MyPage"
                    """,
            fileKind: FileKinds.Legacy);
    }

    [FormattingTestFact]
    public async Task MultiLineComment_WithinHtml1()
    {
        await RunFormattingTestAsync(
            input: """
                    <div>
                    @* <div>
                    This comment's opening at-star will be aligned, and the
                    indentation of the rest of its lines will be preserved.
                            </div>
                        *@
                    </div>
                    """,
            expected: """
                    <div>
                        @* <div>
                    This comment's opening at-star will be aligned, and the
                    indentation of the rest of its lines will be preserved.
                            </div>
                        *@
                    </div>
                    """,
            fileKind: FileKinds.Legacy);
    }

    [FormattingTestFact]
    public async Task MultiLineComment_WithinHtml2()
    {
        await RunFormattingTestAsync(
            input: """
                    <div>
                    @* <div>
                    This comment's opening at-star will be aligned, and the
                    indentation of the rest of its lines will be preserved.
                            </div>                        *@
                    </div>
                    """,
            expected: """
                    <div>
                        @* <div>
                    This comment's opening at-star will be aligned, and the
                    indentation of the rest of its lines will be preserved.
                            </div>                        *@
                    </div>
                    """,
            fileKind: FileKinds.Legacy);
    }

    [FormattingTestFact]
    public async Task MultiLineComment_WithinHtml3()
    {
        await RunFormattingTestAsync(
            input: """
                    <div>
                    @* <div>
                    This comment's opening at-star will be aligned, and the
                    indentation of the rest of its lines will be preserved.
                            </div>
                    *@
                    </div>
                    """,
            expected: """
                    <div>
                        @* <div>
                    This comment's opening at-star will be aligned, and the
                    indentation of the rest of its lines will be preserved.
                            </div>
                    *@
                    </div>
                    """,
            fileKind: FileKinds.Legacy);
    }

    [FormattingTestFact]
    public async Task Using()
    {
        await RunFormattingTestAsync(
            input: """
                    @using   System;
                    """,
            expected: """
                    @using System;
                    """);
    }

    [FormattingTestFact]
    public async Task UsingStatic()
    {
        await RunFormattingTestAsync(
            input: """
                    @using  static   System.Math;
                    """,
            expected: """
                    @using static System.Math;
                    """);
    }

    [FormattingTestFact]
    public async Task UsingAlias()
    {
        await RunFormattingTestAsync(
            input: """
                    @using  M   =    System.Math;
                    """,
            expected: """
                    @using M = System.Math;
                    """);
    }

    [FormattingTestFact]
    public async Task TagHelpers()
    {
        await RunFormattingTestAsync(
            input: """
                    @addTagHelper    *,    Microsoft.AspNetCore.Mvc.TagHelpers
                    @removeTagHelper    *,     Microsoft.AspNetCore.Mvc.TagHelpers
                    @addTagHelper    "*,  Microsoft.AspNetCore.Mvc.TagHelpers"
                    @removeTagHelper    "*,  Microsoft.AspNetCore.Mvc.TagHelpers"
                    @tagHelperPrefix    th:
                    """,
            expected: """
                    @addTagHelper    *,    Microsoft.AspNetCore.Mvc.TagHelpers
                    @removeTagHelper    *,     Microsoft.AspNetCore.Mvc.TagHelpers
                    @addTagHelper    "*,  Microsoft.AspNetCore.Mvc.TagHelpers"
                    @removeTagHelper    "*,  Microsoft.AspNetCore.Mvc.TagHelpers"
                    @tagHelperPrefix    th:
                    """,
            fileKind: FileKinds.Legacy);
    }

    [FormattingTestFact]
    public async Task LargeFile()
    {
        await RunFormattingTestAsync(
            input: RazorTestResources.GetResourceText("FormattingTest.razor"),
            expected: RazorTestResources.GetResourceText("FormattingTest_Expected.razor"),
            allowDiagnostics: true);
    }

    [FormattingTestFact]
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

    [FormattingTestFact]
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

    [FormattingTestFact]
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

    [FormattingTestFact]
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

    [FormattingTestFact]
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

    [FormattingTestFact]
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

    [FormattingTestFact]
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

    [FormattingTestFact]
    public async Task FormatsComplexBlock()
    {
        await RunFormattingTestAsync(
            input: """
                    @page "/"

                    <h1>Hello, world!</h1>

                            Welcome to your new app.

                    <PageTitle Title="How is Blazor working for you?" />

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

                    @{
                    <div class="FF"
                        id="ERT">
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

                    <PageTitle Title="How is Blazor working for you?" />

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

                    @{
                        <div class="FF"
                             id="ERT">
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

    [FormattingTestFact(SkipFlipLineEnding = true)] // tracked by https://github.com/dotnet/razor/issues/10836
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
                    """);
    }

    [FormattingTestFact]
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

    [FormattingTestFact]
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

    [FormattingTestFact]
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

    [FormattingTestFact]
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

    [FormattingTestFact]
    [WorkItem("https://devdiv.visualstudio.com/DevDiv/_workitems/edit/1273468/")]
    public async Task FormatHtmlWithTabs3()
    {
        await RunFormattingTestAsync(
            input: """
                    @page "/"

                     <hr />
                     <div class="row">
                      <div class="col-md-4"
                      label="label">
                       <form method="post">
                        <div class="form-group">
                         <label asp-for="Movie.Title"
                         class="control-label"></label>
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
                    	<div class="col-md-4"
                    		 label="label">
                    		<form method="post">
                    			<div class="form-group">
                    				<label asp-for="Movie.Title"
                    					   class="control-label"></label>
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

    [FormattingTestFact]
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

    [FormattingTestFact]
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

    [FormattingTestFact]
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

    [FormattingTestFact]
    [WorkItem("https://github.com/dotnet/razor/issues/5749")]
    public async Task FormatRenderFragmentInCSharpCodeBlock1()
    {
        // Sadly the first thing the HTML formatter does with this input
        // is put a newline after the @, which means <PageTitle /> won't be
        // seen as a component any more, so we have to turn off our validation,
        // or the test fails before we have a chance to fix the formatting.
        FormattingContext.SkipValidateComponents = true;

        await RunFormattingTestAsync(
            input: """
                    @code
                    {
                        public void DoStuff(RenderFragment renderFragment)
                        {
                            DoThings();
                            renderFragment(@<PageTitle Title="Foo" />);
                    DoThings();
                    renderFragment(@<PageTitle          Title="Foo"             />);

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
                            DoThings();
                            renderFragment(@<PageTitle Title="Foo" />);
                            DoThings();
                            renderFragment(@<PageTitle Title="Foo" />);

                            @* comment *@
                            <div></div>

                            @* comment *@
                            <div></div>
                        }
                    }
                    """);
    }

    [FormattingTestFact]
    [WorkItem("https://github.com/dotnet/razor/issues/5749")]
    public async Task FormatRenderFragmentInCSharpCodeBlock2()
    {
        // Sadly the first thing the HTML formatter does with this input
        // is put a newline after the @, which means <PageTitle /> won't be
        // seen as a component any more, so we have to turn off our validation,
        // or the test fails before we have a chance to fix the formatting.
        FormattingContext.SkipValidateComponents = true;

        await RunFormattingTestAsync(
            input: """
                    <div>
                    @{
                        renderFragment(@<PageTitle Title="Foo" />);

                            @* comment *@
                    <div></div>

                            @* comment *@<div></div>
                        }
                    </div>
                    """,
            expected: """
                    <div>
                        @{
                            renderFragment(@<PageTitle Title="Foo" />);

                            @* comment *@
                            <div></div>

                            @* comment *@
                            <div></div>
                        }
                    </div>
                    """);
    }

    [FormattingTestFact]
    [WorkItem("https://github.com/dotnet/razor/issues/5749")]
    public async Task FormatRenderFragmentInCSharpCodeBlock3()
    {
        // Sadly the first thing the HTML formatter does with this input
        // is put a newline after the @, which means <PageTitle /> won't be
        // seen as a component any more, so we have to turn off our validation,
        // or the test fails before we have a chance to fix the formatting.
        FormattingContext.SkipValidateComponents = true;

        await RunFormattingTestAsync(
            input: """
                    <div>
                    @{
                        renderFragment    (@<PageTitle      Title=  "Foo"     />);

                            @* comment *@
                    <div></div>

                            @* comment *@<div></div>
                        }
                    </div>
                    """,
            expected: """
                    <div>
                        @{
                            renderFragment(@<PageTitle Title="Foo" />);

                            @* comment *@
                            <div></div>

                            @* comment *@
                            <div></div>
                        }
                    </div>
                    """);
    }

    [FormattingTestFact]
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

    [FormattingTestFact]
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

    [FormattingTestFact]
    [WorkItem("https://github.com/dotnet/razor/issues/6090")]
    public async Task FormatHtmlCommentsInsideCSharp3()
    {
        await RunFormattingTestAsync(
            input: """
                    @foreach (var num in Enumerable.Range(1, 10))
                    {
                        <span class="skill_result btn">
                                <!-- this is a
                                very long
                            comment in Html -->
                            <input type="text" />
                                    <!-- this is a
                            very long
                            comment in Html
                                -->
                        </span>
                    }
                    """,
            expected: """
                    @foreach (var num in Enumerable.Range(1, 10))
                    {
                        <span class="skill_result btn">
                            <!-- this is a
                                very long
                            comment in Html -->
                            <input type="text" />
                            <!-- this is a
                            very long
                            comment in Html
                                -->
                        </span>
                    }
                    """);
    }

    [FormattingTestFact]
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
                                <PageTitle  />
                                @if (VarBool)
                            {
                                <div class="mb-16">
                                    <PageTitle  />
                                    <PageTitle  />
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
                                <PageTitle />
                                @if (VarBool)
                                {
                                    <div class="mb-16">
                                        <PageTitle />
                                        <PageTitle />
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

    [FormattingTestFact(SkipFlipLineEndingInOldEngine = true)]
    [WorkItem("https://github.com/dotnet/razor/issues/6001")]
    public async Task FormatNestedCascadingValue2()
    {
        await RunFormattingTestAsync(
            input: """
                    @using Microsoft.AspNetCore.Components.Forms;

                    @if (Object1!= null)
                    {
                        <CascadingValue Value="Variable1">
                                <PageTitle  />
                                @if (VarBool)
                            {
                                <div class="mb-16">
                                    <PageTitle  />
                                    <PageTitle  />
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
                            <PageTitle />
                            @if (VarBool)
                            {
                                <div class="mb-16">
                                    <PageTitle />
                                    <PageTitle />
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
            fileKind: FileKinds.Component); // tracked by https://github.com/dotnet/razor/issues/10836
    }

    [FormattingTestFact]
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
                                <PageTitle  />
                                @if (VarBool)
                            {
                                <div class="mb-16">
                                    <PageTitle  />
                                    <PageTitle  />
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
                            <PageTitle />
                            @if (VarBool)
                            {
                                <div class="mb-16">
                                    <PageTitle />
                                    <PageTitle />
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

    [FormattingTestFact(SkipFlipLineEndingInOldEngine = true)]
    [WorkItem("https://github.com/dotnet/razor/issues/6001")]
    public async Task FormatNestedCascadingValue4()
    {
        await RunFormattingTestAsync(
            input: """
                    @using Microsoft.AspNetCore.Components.Forms;

                        <CascadingValue Value="Variable1">
                                <PageTitle  />
                                @if (VarBool)
                            {
                                <div class="mb-16">
                                    <PageTitle  />
                                    <PageTitle  />
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
                        <PageTitle />
                        @if (VarBool)
                        {
                            <div class="mb-16">
                                <PageTitle />
                                <PageTitle />
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
            fileKind: FileKinds.Component);
    }

    [FormattingTestFact(SkipFlipLineEndingInOldEngine = true)]
    [WorkItem("https://github.com/dotnet/razor/issues/6001")]
    public async Task FormatNestedCascadingValue5()
    {
        await RunFormattingTestAsync(
            input: """
                    @using Microsoft.AspNetCore.Components.Forms;

                    @if (Object1!= null)
                    {
                        <PageTitle>
                                <PageTitle  />
                                @if (VarBool)
                            {
                                <div class="mb-16">
                                    <PageTitle  />
                                    <PageTitle  />
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
                            <PageTitle />
                            @if (VarBool)
                            {
                                <div class="mb-16">
                                    <PageTitle />
                                    <PageTitle />
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
            fileKind: FileKinds.Component);
    }

    [FormattingTestFact]
    [WorkItem("https://github.com/dotnet/razor/issues/6001")]
    public async Task FormatNestedCascadingValue6()
    {
        await RunFormattingTestAsync(
            input: """
                    @using Microsoft.AspNetCore.Components.Forms;

                    @if (Object1!= null)
                    {
                        <CascadingValue Value="Variable1">
                        <div>
                                <PageTitle  />
                                @if (VarBool)
                            {
                                <div class="mb-16">
                                    <PageTitle  />
                                    <PageTitle  />
                                </div>
                            }
                            </div>
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
                            <div>
                                <PageTitle />
                                @if (VarBool)
                                {
                                    <div class="mb-16">
                                        <PageTitle />
                                        <PageTitle />
                                    </div>
                                }
                            </div>
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

    [FormattingTestFact]
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

    [FormattingTestFact]
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

    [FormattingTestFact]
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

    [FormattingTestFact]
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

    [FormattingTestFact]
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

    [FormattingTestFact(SkipOldFormattingEngine = true)]
    public async Task FormatEventHandlerAttributes()
    {
        await RunFormattingTestAsync(
            input: """
                    <p>Current count: @currentCount</p>

                    <button @onclick="IncrementCount">Increment</button>
                    <button @onclick="@(e=>currentCount=4)">Update to 4</button>
                    <button @onclick="e=>currentCount=5">Update to 5</button>

                    @code {
                        private int currentCount=0;

                        private void IncrementCount()
                        {
                            currentCount++;
                        }
                    }
                    """,
            expected: """
                    <p>Current count: @currentCount</p>

                    <button @onclick="IncrementCount">Increment</button>
                    <button @onclick="@(e => currentCount = 4)">Update to 4</button>
                    <button @onclick="e => currentCount = 5">Update to 5</button>

                    @code {
                        private int currentCount = 0;

                        private void IncrementCount()
                        {
                            currentCount++;
                        }
                    }
                    """,
            fileKind: FileKinds.Component);
    }

    [FormattingTestFact(SkipOldFormattingEngine = true)]
    public async Task FormatEventCallbackAttributes()
    {
        await RunFormattingTestAsync(
            input: """
                    @using Microsoft.AspNetCore.Components.Forms;

                    <p>Current count: @currentCount</p>

                    <InputText ValueChanged="IncrementCount">Increment</InputText>
                    <InputText ValueChanged="@(e=>currentCount=4)">Update to 4</InputText>
                    <InputText ValueChanged="e=>currentCount=5">Update to 5</InputText>

                    @code {
                        private int currentCount=0;

                        private void IncrementCount()
                        {
                            currentCount++;
                        }
                    }
                    """,
            expected: """
                    @using Microsoft.AspNetCore.Components.Forms;

                    <p>Current count: @currentCount</p>

                    <InputText ValueChanged="IncrementCount">Increment</InputText>
                    <InputText ValueChanged="@(e => currentCount = 4)">Update to 4</InputText>
                    <InputText ValueChanged="e => currentCount = 5">Update to 5</InputText>

                    @code {
                        private int currentCount = 0;

                        private void IncrementCount()
                        {
                            currentCount++;
                        }
                    }
                    """,
            fileKind: FileKinds.Component);
    }

    [FormattingTestFact(SkipOldFormattingEngine = true)]
    public async Task FormatBindAttributes()
    {
        await RunFormattingTestAsync(
            input: """
                    @using Microsoft.AspNetCore.Components.Forms;

                    <p>Current count: @currentCount</p>

                    <InputText @bind-Value="currentCount" @bind-Value:after="IncrementCount">Increment</InputText>
                    <InputText @bind-Value="currentCount" @bind-Value:after="e=>currentCount=5">Update to 5</InputText>

                    @code {
                        private int currentCount=0;

                        private void IncrementCount()
                        {
                            currentCount++;
                        }
                    }
                    """,
            expected: """
                    @using Microsoft.AspNetCore.Components.Forms;

                    <p>Current count: @currentCount</p>

                    <InputText @bind-Value="currentCount" @bind-Value:after="IncrementCount">Increment</InputText>
                    <InputText @bind-Value="currentCount" @bind-Value:after="e => currentCount = 5">Update to 5</InputText>

                    @code {
                        private int currentCount = 0;

                        private void IncrementCount()
                        {
                            currentCount++;
                        }
                    }
                    """,
            fileKind: FileKinds.Component);
    }

    [FormattingTestFact]
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

    [FormattingTestFact]
    [WorkItem("https://github.com/dotnet/razor/issues/6110")]
    public async Task FormatExplicitCSharpInsideHtml1()
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

    [FormattingTestFact]
    [WorkItem("https://github.com/dotnet/razor/issues/6110")]
    public async Task FormatExplicitCSharpInsideHtml2()
    {
        await RunFormattingTestAsync(
            input: """
                    <div>
                         @(   Html.DisplayNameFor (@<text>
                            <p >
                            <h2 ></h2>
                            </p>
                            </text>)
                            .ToString())

                         @(   Html.DisplayNameFor (@<div></div>,
                            1,   3,    4))

                         @(   Html.DisplayNameFor (@<div></div>,
                            1,   3, @<div></div>,
                            2, 4))

                         @(   Html.DisplayNameFor (
                            1,   3, @<div></div>,
                            2, 4))

                         @(   Html.DisplayNameFor (
                            1,   3,
                            2,  4))

                         @(   Html.DisplayNameFor (
                            2, 4,
                            1,   3, @<div></div>,
                            2, 4,
                            1,   3, @<div></div>,
                            4))
                    </div>
                    """,
            expected: """
                    <div>
                        @(Html.DisplayNameFor(@<text>
                            <p>
                                <h2></h2>
                            </p>
                        </text>)
                            .ToString())

                        @(Html.DisplayNameFor(@<div></div>,
                            1, 3, 4))
                    
                        @(Html.DisplayNameFor(@<div></div>,
                            1, 3, @<div></div>,
                            2, 4))

                        @(Html.DisplayNameFor(
                            1, 3, @<div></div>,
                            2, 4))

                        @(Html.DisplayNameFor(
                            1, 3,
                            2, 4))
                    
                        @(Html.DisplayNameFor(
                            2, 4,
                            1, 3, @<div></div>,
                            2, 4,
                            1, 3, @<div></div>,
                            4))
                    </div>
                    """,
            fileKind: FileKinds.Legacy);
    }

    [FormattingTestFact]
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

    [FormattingTestFact]
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

    [FormattingTestFact]
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

    [FormattingTestFact]
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

    [FormattingTestFact]
    public async Task FormatIndentedElementAttributes()
    {
        await RunFormattingTestAsync(
            input: """
                    Welcome.

                    <div class="goo"
                     align="center">
                    </div>

                    <PageTitle Title="How is Blazor working for you?"
                     Color="Red" />

                    <PageTitle Title="How is Blazor working for you?"
                     Color="Red"></PageTitle>

                    <PageTitle Title="How is Blazor working for you?"
                     Color="Red">
                     Hello
                     </PageTitle>

                    @if (true)
                    {
                    <div class="goo"
                     align="center">
                    </div>

                    <PageTitle Title="How is Blazor working for you?"
                       Color="Red" />

                       <tag attr1="value1"
                       attr2="value2"
                       attr3="value3"
                       />

                     <tag attr1="value1"
                       attr2="value2"
                       attr3="value3"></tag>

                     <tag attr1="value1"
                       attr2="value2"
                       attr3="value3">
                    Hello
                        </tag>

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

                    <PageTitle Title="How is Blazor working for you?"
                               Color="Red" />

                    <PageTitle Title="How is Blazor working for you?"
                               Color="Red"></PageTitle>
                    
                    <PageTitle Title="How is Blazor working for you?"
                               Color="Red">
                        Hello
                    </PageTitle>

                    @if (true)
                    {
                        <div class="goo"
                             align="center">
                        </div>

                        <PageTitle Title="How is Blazor working for you?"
                                   Color="Red" />

                        <tag attr1="value1"
                             attr2="value2"
                             attr3="value3" />

                        <tag attr1="value1"
                             attr2="value2"
                             attr3="value3"></tag>
                    
                        <tag attr1="value1"
                             attr2="value2"
                             attr3="value3">
                            Hello
                        </tag>

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
    [FormattingTestFact]
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

    [FormattingTestFact]
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

    [FormattingTestFact]
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
            expected: _useNewFormattingEngine
                ? """
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
                    """
                : """
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

    [FormattingTestFact]
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

    [FormattingTestFact]
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

    [FormattingTestFact]
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

    [FormattingTestFact]
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

    [FormattingTestFact]
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

    [FormattingTestFact]
    public async Task Formats_ImplicitExpressions()
    {
        await RunFormattingTestAsync(
            input: """
                    <div>
                        It is @DateTime.Now.ToString(   "d MM yyy"   ). Or is it @DateTime.Now.ToString(   "d MM yyy"   ).

                        @DateTime.Now.ToString(   "d MM yyy"   ) it is.

                        @DateTime.Now.ToString(   "d MM yyy"   ). Is what it is today. Or is it @DateTime.Now.ToString(   "d MM yyy"   ).

                        @DateTime.Now.ToString(   "d MM yyy"   ) <span>Today!</span>
                    </div>
                    """,
            expected: """
                    <div>
                        It is @DateTime.Now.ToString("d MM yyy"). Or is it @DateTime.Now.ToString("d MM yyy").
                    
                        @DateTime.Now.ToString("d MM yyy") it is.
                    
                        @DateTime.Now.ToString("d MM yyy"). Is what it is today. Or is it @DateTime.Now.ToString("d MM yyy").
                    
                        @DateTime.Now.ToString("d MM yyy") <span>Today!</span>
                    </div>
                    """);
    }

    [FormattingTestFact]
    public async Task Formats_ExplicitExpressions()
    {
        await RunFormattingTestAsync(
            input: """
                    <div>
                        It is @(DateTime.    Now). Or is it @(DateTime.    Now).

                        @(DateTime.    Now) it is.

                        @(DateTime.    Now). Is what it is today. Or is it @(DateTime.    Now).

                        @(DateTime.    Now) <span>Today!</span>
                    </div>
                    """,
            expected: """
                    <div>
                        It is @(DateTime.Now). Or is it @(DateTime.Now).
                    
                        @(DateTime.Now) it is.
                    
                        @(DateTime.Now). Is what it is today. Or is it @(DateTime.Now).
                    
                        @(DateTime.Now) <span>Today!</span>
                    </div>
                    """);
    }

    [FormattingTestFact]
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

    [FormattingTestFact]
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

    [FormattingTestFact]
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

    [FormattingTestFact]
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

    [FormattingTestFact]
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

    [FormattingTestFact]
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

    [FormattingTestFact]
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

    [FormattingTestFact]
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

    [FormattingTestFact]
    public async Task Format_SectionDirectiveBlock8()
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
                    <p>and finally this</p>
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
                        <p>and finally this</p>
                    }
                    """,
            fileKind: FileKinds.Legacy);
    }

    [FormattingTestFact]
    public async Task Format_SectionDirectiveBlock9()
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
                    <p>and finally this</p>
                    }

                    <p>I lied when I said finally</p>

                    @functions {
                     public class Foo2{
                    void Method() {  }
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
                        <p>and finally this</p>
                    }

                    <p>I lied when I said finally</p>

                    @functions {
                        public class Foo2
                        {
                            void Method() { }
                        }
                    }
                    """,
            fileKind: FileKinds.Legacy);
    }

    [FormattingTestFact]
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

    [FormattingTestFact]
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

    [FormattingTestFact]
    public async Task Formats_ExplicitStatements1()
    {
        await RunFormattingTestAsync(
            input: """
                   @{
                    <text>Hello</text>
                   }

                   @{ <text>Hello</text> }

                   <div></div>

                   @{ }

                   <div></div>
                   """,
            expected: """
                    @{
                        <text>Hello</text>
                    }
                    
                    @{
                        <text>Hello</text>
                    }
                    
                    <div></div>
                    
                    @{ }
                    
                    <div></div>
                    """);
    }

    [FormattingTestFact]
    public async Task Formats_ExplicitStatements2()
    {
        await RunFormattingTestAsync(
            input: """
                   <div>
                   @{
                    <text>Hello</text>
                   }

                   @{ <text>Hello</text> }

                   <div></div>

                   @{ }

                   <div></div>
                   </div>
                   """,
            expected: """
                    <div>
                        @{
                            <text>Hello</text>
                        }
                    
                        @{
                            <text>Hello</text>
                        }
                    
                        <div></div>
                    
                        @{ }
                    
                        <div></div>
                    </div>
                    """);
    }

    [FormattingTestFact]
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

    [FormattingTestFact]
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

    [FormattingTestFact]
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

    [FormattingTestFact]
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

    [FormattingTestFact]
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

    [FormattingTestFact]
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

    [FormattingTestFact]
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

    [FormattingTestFact]
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

    [FormattingTestFact]
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

    [FormattingTestFact]
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

    [FormattingTestFact]
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

    [FormattingTestFact]
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
    [FormattingTestFact]
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

    [FormattingTestFact]
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

    [FormattingTestFact]
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

    [FormattingTestFact]
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

    [FormattingTestFact]
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

    [FormattingTestFact]
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

    [FormattingTestFact]
    [WorkItem("https://github.com/dotnet/aspnetcore/issues/29837")]
    public async Task CodeBlock_NestedComponents()
    {
        await RunFormattingTestAsync(
            input: """
                    @code {
                        private WeatherForecast[] forecasts;

                        protected override async Task OnInitializedAsync()
                        {
                            <PageTitle>
                                @{
                                        var t = DateTime.Now;
                                        t.ToString();
                                    }
                                </PageTitle>
                            forecasts = await ForecastService.GetForecastAsync(DateTime.Now);
                        }
                    }
                    """,
            expected: """
                    @code {
                        private WeatherForecast[] forecasts;

                        protected override async Task OnInitializedAsync()
                        {
                            <PageTitle>
                                @{
                                    var t = DateTime.Now;
                                    t.ToString();
                                }
                            </PageTitle>
                            forecasts = await ForecastService.GetForecastAsync(DateTime.Now);
                        }
                    }
                    """);
    }

    [FormattingTestFact]
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

    [FormattingTestFact]
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
            expected: _useNewFormattingEngine
                ? """
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
                    """
                : """
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

    [FormattingTestFact]
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

    [FormattingTestFact]
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

    [FormattingTestFact]
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
            expected: _useNewFormattingEngine
                ? """
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
                    """
                : """
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

    [FormattingTestFact]
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

    [FormattingTestFact]
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

    [FormattingTestFact]
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
            expected: _useNewFormattingEngine
                ? """
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
                    """
                : """
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

    [FormattingTestFact]
    [WorkItem("https://github.com/dotnet/razor-tooling/issues/6092")]
    public async Task CodeBlock_CollectionInitializers1()
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

    [FormattingTestFact]
    [WorkItem("https://github.com/dotnet/razor-tooling/issues/6092")]
    public async Task CodeBlock_CollectionInitializers2()
    {
        // The C# Formatter doesn't touch these types of initializers, so nor do we. This test
        // just verifies we don't regress things and start moving code around.
        await RunFormattingTestAsync(
            input: """
                    @code
                    {
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
                    @code
                    {
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

    [FormattingTestFact]
    [WorkItem("https://github.com/dotnet/razor/issues/11325")]
    public async Task CodeBlock_CollectionExpression1()
    {
        // The C# Formatter doesn't touch these types of initializers, so nor do we. This test
        // just verifies we don't regress things and start moving code around.
        await RunFormattingTestAsync(
            input: """
                    @code {
                        private void M()
                        {
                            List<string> entries = [
                                "a",
                                "b",
                                "c"
                            ];
                        }
                    }
                    """,
            expected: """
                    @code {
                        private void M()
                        {
                            List<string> entries = [
                                "a",
                                "b",
                                "c"
                            ];
                        }
                    }
                    """);
    }

    [FormattingTestFact]
    [WorkItem("https://github.com/dotnet/razor/issues/11325")]
    public async Task CodeBlock_CollectionExpression2()
    {
        // The C# Formatter doesn't touch these types of initializers, so nor do we. This test
        // just verifies we don't regress things and start moving code around.
        await RunFormattingTestAsync(
            input: """
                    @code {
                        private void M()
                        {
                            List<string> entries = [
                                    "a",
                            "b",
                                "c"
                            ];
                        }
                    }
                    """,
            expected: """
                    @code {
                        private void M()
                        {
                            List<string> entries = [
                                    "a",
                            "b",
                                "c"
                            ];
                        }
                    }
                    """);
    }

    [FormattingTestFact]
    [WorkItem("https://github.com/dotnet/razor/issues/11325")]
    public async Task CodeBlock_CollectionExpression3()
    {
        // The C# Formatter doesn't touch these types of initializers, so nor do we. This test
        // just verifies we don't regress things and start moving code around.
        await RunFormattingTestAsync(
            input: """
                    @code {
                        private void M()
                        {
                            List<string> entries = [
                            ];
                        }
                    }
                    """,
            expected: """
                    @code {
                        private void M()
                        {
                            List<string> entries = [
                            ];
                        }
                    }
                    """);
    }

    [FormattingTestFact]
    [WorkItem("https://github.com/dotnet/razor/issues/11325")]
    public async Task CodeBlock_CollectionExpression4()
    {
        await RunFormattingTestAsync(
            input: """
                    @code {
                        private void M(string[] strings)
                        {
                            List<string> entries = [  ..     strings,    "a",      "b",         "c"    ];
                        }
                    }
                    """,
            expected: """
                    @code {
                        private void M(string[] strings)
                        {
                            List<string> entries = [.. strings, "a", "b", "c"];
                        }
                    }
                    """);
    }

    [FormattingTestFact]
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

    [FormattingTestFact]
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

    [FormattingTestFact]
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

    [FormattingTestFact]
    [WorkItem("https://github.com/dotnet/aspnetcore/issues/4498")]
    public async Task IfBlock_TopLevel_WithOtherCode2()
    {
        await RunFormattingTestAsync(
            input: """
                    @{

                        // foo

                            // foo

                    }

                            @if (true)
                    {
                    }
                    """,
            expected: """
                    @{

                        // foo

                        // foo

                    }

                    @if (true)
                    {
                    }
                    """);
    }

    [FormattingTestFact]
    [WorkItem("https://github.com/dotnet/aspnetcore/issues/4498")]
    public async Task IfBlock_TopLevel_WithOtherCode3()
    {
        await RunFormattingTestAsync(
            input: """
                    @{
                        var x = 3;

                        // foo
                    }

                            @if (true)
                    {
                    }
                    """,
            expected: """
                    @{
                        var x = 3;

                        // foo
                    }

                    @if (true)
                    {
                    }
                    """);
    }

    [FormattingTestFact]
    [WorkItem("https://github.com/dotnet/aspnetcore/issues/4498")]
    public async Task IfBlock_TopLevel_WithOtherCode4()
    {
        await RunFormattingTestAsync(
            input: """
                    @{
                        var x = 3;
                    }

                            @if (true)
                    {
                    }
                    """,
            expected: """
                    @{
                        var x = 3;
                    }

                    @if (true)
                    {
                    }
                    """);
    }

    [FormattingTestFact]
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

    [FormattingTestFact]
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

    [FormattingTestFact]
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

    [FormattingTestFact]
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

    [FormattingTestFact]
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

    [FormattingTestFact]
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

    [FormattingTestFact]
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

    [FormattingTestFact]
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

    [FormattingTestFact]
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

    [FormattingTestFact]
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

    [FormattingTestFact]
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

    [FormattingTestFact(SkipOldFormattingEngine = true)]
    [WorkItem("https://github.com/dotnet/razor/issues/9254")]
    public async Task RenderFragmentPresent()
    {
        await RunFormattingTestAsync(
            input: """
                    @page "/"
                    @code
                    {
                        void T()
                        {
                            S("first"
                                + "second"
                                + "third");
                        }

                    string[] S(string s) =>
                            s.Split(',')
                            . Select(s => s.Trim())
                            . ToArray();

                    RenderFragment R => @<div></div>;
                    }
                    """,
            expected: """
                    @page "/"
                    @code
                    {
                        void T()
                        {
                            S("first"
                                + "second"
                                + "third");
                        }

                        string[] S(string s) =>
                                s.Split(',')
                                .Select(s => s.Trim())
                                .ToArray();

                        RenderFragment R => @<div></div>;
                    }
                    """);
    }

    [FormattingTestFact(SkipOldFormattingEngine = true)]
    [WorkItem("https://github.com/dotnet/razor/issues/6150")]
    public async Task RenderFragment_InLambda()
    {
        // Formatting result here is not necessarily perfect, but in the new engine is stable
        await RunFormattingTestAsync(
            input: """
                    @page "/"
                    @using RazorClassLibrary2.Models

                    @code{
                        private DateTime? date1;

                        Gopt<int> gopt = new Gopt<int>()
                        {
                            Name = "hi"
                        }
                        .Editor(m =>
                        {
                        return
                        @<text>hi</text>
                        ; }
                        );    
                    }
                    """,
            expected: """
                    @page "/"
                    @using RazorClassLibrary2.Models

                    @code {
                        private DateTime? date1;

                        Gopt<int> gopt = new Gopt<int>()
                        {
                            Name = "hi"
                        }
                        .Editor(m =>
                        {
                            return
                            @<text>hi</text>
                            ;
                        }
                        );
                    }
                    """);
    }

    [FormattingTestFact]
    [WorkItem("https://github.com/dotnet/razor/issues/9119")]
    public async Task CollectionInitializers()
    {
        await RunFormattingTestAsync(
            input: """
                    @{
                        // Stable
                        var formatMe = new string[] {
                            "One",
                            "Two",
                            "Three",
                        };

                        // Closing brace advances to the right
                        var formatMeTwo = new string[]
                        {
                            "One",
                            "Two",
                            "Three",
                        };

                        // Stable
                        var formatMeThree = new List<string> {
                            "One",
                            "Two",
                            "Three",
                        };
                    
                        // Opening brace advances to the right
                        var formatMeFour = new List<string>
                        {
                            "One",
                            "Two",
                            "Three",
                        };
                    }
                    """,
            expected: """
                    @{
                        // Stable
                        var formatMe = new string[] {
                            "One",
                            "Two",
                            "Three",
                        };
                    
                        // Closing brace advances to the right
                        var formatMeTwo = new string[]
                        {
                            "One",
                            "Two",
                            "Three",
                        };
                    
                        // Stable
                        var formatMeThree = new List<string> {
                            "One",
                            "Two",
                            "Three",
                        };
                    
                        // Opening brace advances to the right
                        var formatMeFour = new List<string>
                        {
                            "One",
                            "Two",
                            "Three",
                        };
                    }
                    """);
    }

    [FormattingTestFact(SkipOldFormattingEngine = true)]
    [WorkItem("https://github.com/dotnet/razor/issues/9711")]
    public async Task Directives()
    {
        await RunFormattingTestAsync(
            input: """
                            @page "/"

                            @using System
                            @inject object Foo


                    """,
            expected: """
                    @page "/"
                    
                    @using System
                    @inject object Foo
                    
                    
                    """);
    }

    [FormattingTestFact]
    [WorkItem("https://devdiv.visualstudio.com/DevDiv/_workitems/edit/2347107")]
    public async Task ImplicitExpressionAtEndOfCodeBlock()
    {
        await RunFormattingTestAsync(
            input: """
                @page "/"
                @model IndexModel

                <div>
                </div>

                @functions {void Foo() { }}@Foo()
                """,
            expected: """
                @page "/"
                @model IndexModel
                
                <div>
                </div>
                
                @functions {
                    void Foo() { }
                }
                @Foo()
                """,
            fileKind: FileKinds.Legacy);
    }

    [FormattingTestFact]
    public async Task LineBreakAtTheEndOfBlocks()
    {
        await RunFormattingTestAsync(
            input: """
                @page "/"
                @model IndexModel

                <div>
                </div>

                @code {void Foo() { }}@Foo.ToString(   1  )
                """,
            expected: """
                @page "/"
                @model IndexModel
                
                <div>
                </div>
                
                @code {
                    void Foo() { }
                }
                @Foo.ToString(1)
                """);
    }

    [FormattingTestFact]
    public async Task EscapedAtSignsInCSS()
    {
        await RunFormattingTestAsync(
            input: """
                @page "/"
                @model IndexModel

                <style>
                    @@media only screen and (max-width: 600px) {
                        body {
                            background-color: lightblue;
                        }
                    }
                </style>

                <style>
                    @@font-face {
                        src: url();
                    }
                </style>

                @if (RendererInfo.IsInteractive)
                {
                <button />
                }
                """,
            expected: """
                @page "/"
                @model IndexModel
                
                <style>
                    @@media only screen and (max-width: 600px) {
                        body {
                            background-color: lightblue;
                        }
                    }
                </style>

                <style>
                    @@font-face {
                        src: url();
                    }
                </style>

                @if (RendererInfo.IsInteractive)
                {
                    <button />
                }
                """);
    }

    [FormattingTestFact(SkipOldFormattingEngine = true)]
    public async Task PartialTagHelper()
    {
        await RunFormattingTestAsync(
            input: """
                @page "/"
                
                <div>
                    <partial name="~/Views/Shared/_TestimonialRow.cshtml"
                    model="new DefaultTitleContentAreaViewModel
                    {
                    Title = Model.CurrentPage.TestimonialsTitle,
                    ContentArea = Model.CurrentPage.TestimonialsContentArea,
                    ChildCssClass = string.Empty
                    }" />
                </div>
                """,
            expected: """
                @page "/"
                
                <div>
                    <partial name="~/Views/Shared/_TestimonialRow.cshtml"
                             model="new DefaultTitleContentAreaViewModel
                             {
                                 Title = Model.CurrentPage.TestimonialsTitle,
                                 ContentArea = Model.CurrentPage.TestimonialsContentArea,
                                 ChildCssClass = string.Empty
                             }" />
                </div>
                """,
            fileKind: FileKinds.Legacy);
    }

    [FormattingTestFact(SkipOldFormattingEngine = true)]
    public async Task MultilineExplicitExpression()
    {
        await RunFormattingTestAsync(
            input: """
                @page "/"

                <partial name="~/Views/Shared/_TestimonialRow.cshtml"
                    model="@(new DefaultTitleContentAreaViewModel
                    {
                        Title = Model.CurrentPage.TestimonialsTitle,
                        ContentArea = Model.CurrentPage.TestimonialsContentArea,
                        ChildCssClass = string.Empty
                    })" />

                <partial model="@(new DefaultTitleContentAreaViewModel
                    {
                        Title = Model.CurrentPage.TestimonialsTitle,
                        ContentArea = Model.CurrentPage.TestimonialsContentArea,
                        ChildCssClass = string.Empty
                    })" />
                
                <div>
                    <partial name="~/Views/Shared/_TestimonialRow.cshtml"
                    model="@(new DefaultTitleContentAreaViewModel
                    {
                    Title = Model.CurrentPage.TestimonialsTitle,
                    ContentArea = Model.CurrentPage.TestimonialsContentArea,
                    ChildCssClass = string.Empty
                    })" />
                </div>
                """,
            expected: """
                @page "/"

                <partial name="~/Views/Shared/_TestimonialRow.cshtml"
                         model="@(new DefaultTitleContentAreaViewModel
                         {
                             Title = Model.CurrentPage.TestimonialsTitle,
                             ContentArea = Model.CurrentPage.TestimonialsContentArea,
                             ChildCssClass = string.Empty
                         })" />

                <partial model="@(new DefaultTitleContentAreaViewModel
                         {
                             Title = Model.CurrentPage.TestimonialsTitle,
                             ContentArea = Model.CurrentPage.TestimonialsContentArea,
                             ChildCssClass = string.Empty
                         })" />

                <div>
                    <partial name="~/Views/Shared/_TestimonialRow.cshtml"
                             model="@(new DefaultTitleContentAreaViewModel
                             {
                                 Title = Model.CurrentPage.TestimonialsTitle,
                                 ContentArea = Model.CurrentPage.TestimonialsContentArea,
                                 ChildCssClass = string.Empty
                             })" />
                </div>
                """);
    }

    [FormattingTestFact]
    public async Task MultilineExplicitExpression_IsStable()
    {
        // This test explicitly validates that the expected output from the above test results in stable formatting.
        var code = """
                @page "/"

                <partial name="~/Views/Shared/_TestimonialRow.cshtml"
                         model="@(new DefaultTitleContentAreaViewModel
                         {
                             Title = Model.CurrentPage.TestimonialsTitle,
                             ContentArea = Model.CurrentPage.TestimonialsContentArea,
                             ChildCssClass = string.Empty
                         })" />

                <partial model="@(new DefaultTitleContentAreaViewModel
                         {
                             Title = Model.CurrentPage.TestimonialsTitle,
                             ContentArea = Model.CurrentPage.TestimonialsContentArea,
                             ChildCssClass = string.Empty
                         })" />

                <div>
                    <partial name="~/Views/Shared/_TestimonialRow.cshtml"
                             model="@(new DefaultTitleContentAreaViewModel
                             {
                                 Title = Model.CurrentPage.TestimonialsTitle,
                                 ContentArea = Model.CurrentPage.TestimonialsContentArea,
                                 ChildCssClass = string.Empty
                             })" />
                </div>
                """;
        await RunFormattingTestAsync(
            input: code,
            expected: code);
    }

    [FormattingTestFact(SkipOldFormattingEngine = true)]
    [WorkItem("https://github.com/dotnet/razor/issues/11622")]
    public async Task TextArea()
    {
        var code = """
                @page "/"
                
                @if (true)
                {
                    <textarea id="textarea1">
                    a
                        @if (true)
                        {
                        b
                            }
                            c
                    </textarea>
                }
                
                <textarea id="textarea2">
                    a
                        @if (true)
                        {
                        b
                            }
                            c
                    </textarea>
                
                <div>
                    <textarea id="textarea3">
                            a
                                @if (true)
                                {
                                b
                                    }
                                    c
                        </textarea>
                </div>
                """;
        await RunFormattingTestAsync(
            input: code,
            expected: code);
    }
}
