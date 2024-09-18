// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.AspNetCore.Razor.Test.Common;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Formatting;

public class RazorFormattingTest(ITestOutputHelper testOutput) : FormattingTestBase(testOutput)
{
    [Fact]
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

    [Fact]
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

    [Theory, CombinatorialData]
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

    [Theory, CombinatorialData]
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

    [Fact]
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

    [Fact]
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

    [Fact]
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

    [Fact]
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

    [Fact]
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
            razorLSPOptions: RazorLSPOptions.Default with { CodeBlockBraceOnNextLine = true });
    }

    [Fact]
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
            razorLSPOptions: RazorLSPOptions.Default with { CodeBlockBraceOnNextLine = true });
    }

    [Fact]
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

    [Fact]
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

    [Fact]
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
            razorLSPOptions: RazorLSPOptions.Default with { CodeBlockBraceOnNextLine = true });
    }

    [Fact]
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

    [Fact]
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
            razorLSPOptions: RazorLSPOptions.Default with { CodeBlockBraceOnNextLine = true });
    }

    [Fact]
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

    [Fact]
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

    [Fact]
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
            razorLSPOptions: RazorLSPOptions.Default with { CodeBlockBraceOnNextLine = true });
    }

    [Fact]
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

    [Fact]
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

    [Fact]
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

    [Fact]
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

    [Fact]
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

    [Fact]
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

    [Fact]
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

    [Fact]
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

    [Fact]
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

    [Fact]
    public async Task Attribute()
    {
        await RunFormattingTestAsync(
            input: """
                    @attribute     [Obsolete(   "asdf"   , error:    false)]
                    """,
            expected: """
                    @attribute [Obsolete("asdf", error: false)]
                    """);
    }

    [Fact]
    public async Task TypeParam()
    {
        await RunFormattingTestAsync(
            input: """
                    @typeparam     T     where    T    :   IDisposable
                    """,
            expected: """
                    @typeparam T where T : IDisposable
                    """);
    }

    [Fact]
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

    [Fact]
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

    [Fact]
    public async Task MultiLineComment_WithinHtml ()
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

    // Regression prevention tests:
    [Fact]
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

    [Fact]
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

    [Fact]
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

    [Fact]
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

    [Fact]
    public async Task OnTypeFormatting_Enabled()
    {
        await RunOnTypeFormattingTestAsync(
            input: """
            @functions {
            	private int currentCount = 0;
            
            	private void IncrementCount (){
            		currentCount++;
            	}$$
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
            triggerCharacter: '}',
            razorLSPOptions: RazorLSPOptions.Default with { FormatOnType = true });
    }

    [Fact]
    public async Task LargeFile()
    {
        await RunFormattingTestAsync(
            input: RazorTestResources.GetResourceText("FormattingTest.razor"),
            expected: RazorTestResources.GetResourceText("FormattingTest_Expected.razor"),
            allowDiagnostics: true);
    }
}
