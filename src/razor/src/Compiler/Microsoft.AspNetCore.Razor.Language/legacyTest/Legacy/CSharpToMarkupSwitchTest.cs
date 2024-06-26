// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using Microsoft.AspNetCore.Razor.Language.Components;
using Roslyn.Test.Utilities;
using Xunit;

namespace Microsoft.AspNetCore.Razor.Language.Legacy;

public class CSharpToMarkupSwitchTest() : ParserTestBase(layer: TestProject.Layer.Compiler, validateSpanEditHandlers: true)
{
    [Fact]
    public void SingleAngleBracketDoesNotCauseSwitchIfOuterBlockIsTerminated()
    {
        ParseDocumentTest("@{ List< }");
    }

    [Fact]
    public void GivesSpacesToCodeOnAtTagTemplateTransitionInDesignTimeMode()
    {
        ParseDocumentTest("@Foo(    @<p>Foo</p>    )", designTime: true);
    }

    [Fact]
    public void GivesSpacesToCodeOnAtColonTemplateTransitionInDesignTimeMode()
    {
        ParseDocumentTest("""
            @Foo(    
            @:<p>Foo</p>    
            )
            """, designTime: true);
    }

    [Fact]
    public void GivesSpacesToCodeOnTagTransitionInDesignTimeMode()
    {
        ParseDocumentTest("""
            @{
                <p>Foo</p>    
            }
            """, designTime: true);
    }

    [Fact]
    public void GivesSpacesToCodeOnInvalidAtTagTransitionInDesignTimeMode()
    {
        ParseDocumentTest("""
            @{
                @<p>Foo</p>    
            }
            """, designTime: true);
    }

    [Fact]
    public void GivesSpacesToCodeOnAtColonTransitionInDesignTimeMode()
    {
        ParseDocumentTest("""
            @{
                @:<p>Foo</p>    
            }
            """, designTime: true);
    }

    [Fact]
    public void ShouldSupportSingleLineMarkupContainingStatementBlock()
    {
        ParseDocumentTest("""
            @Repeat(10,
                @: @{}
            )
            """);
    }

    [Fact]
    public void ShouldSupportMarkupWithoutPreceedingWhitespace()
    {
        ParseDocumentTest("""
            @foreach(var file in files){
            
            
            @:Baz
            <br/>
            <a>Foo</a>
            @:Bar
            }
            """);
    }

    [Fact]
    public void GivesAllWhitespaceOnSameLineWithTrailingNewLineToMarkupExclPreceedingNewline()
    {
        // ParseBlockGivesAllWhitespaceOnSameLineExcludingPreceedingNewlineButIncludingTrailingNewLineToMarkup
        ParseDocumentTest("""
            @if(foo) {
                var foo = "After this statement there are 10 spaces";          
                <p>
                    Foo
                    @bar
                </p>
                @:Hello!
                var biz = boz;
            }
            """);
    }

    [Fact]
    public void AllowsMarkupInIfBodyWithBraces()
    {
        ParseDocumentTest("@if(foo) { <p>Bar</p> } else if(bar) { <p>Baz</p> } else { <p>Boz</p> }");
    }

    [Fact]
    public void AllowsMarkupInIfBodyWithBracesWithinCodeBlock()
    {
        ParseDocumentTest("@{ if(foo) { <p>Bar</p> } else if(bar) { <p>Baz</p> } else { <p>Boz</p> } }");
    }

    [Fact]
    public void SupportsMarkupInCaseAndDefaultBranchesOfSwitch()
    {
        // Arrange
        ParseDocumentTest("""
            @switch(foo) {
                case 0:
                    <p>Foo</p>
                    break;
                case 1:
                    <p>Bar</p>
                    return;
                case 2:
                    {
                        <p>Baz</p>
                        <p>Boz</p>
                    }
                default:
                    <p>Biz</p>
            }
            """);
    }

    [Fact]
    public void SupportsMarkupInCaseAndDefaultBranchesOfSwitchInCodeBlock()
    {
        // Arrange
        ParseDocumentTest("""
            @{ switch(foo) {
                case 0:
                    <p>Foo</p>
                    break;
                case 1:
                    <p>Bar</p>
                    return;
                case 2:
                    {
                        <p>Baz</p>
                        <p>Boz</p>
                    }
                default:
                    <p>Biz</p>
            } }
            """);
    }

    [Fact]
    public void ParsesMarkupStatementOnOpenAngleBracket()
    {
        ParseDocumentTest("@for(int i = 0; i < 10; i++) { <p>Foo</p> }");
    }

    [Fact]
    public void ParsesMarkupStatementOnOpenAngleBracketInCodeBlock()
    {
        ParseDocumentTest("@{ for(int i = 0; i < 10; i++) { <p>Foo</p> } }");
    }

    [Fact]
    public void ParsesMarkupStatementOnSwitchCharacterFollowedByColon()
    {
        // Arrange
        ParseDocumentTest("""
            @if(foo) { @:Bar
            } zoop
            """);
    }

    [Fact]
    public void ParsesMarkupStatementOnSwitchCharacterFollowedByDoubleColon()
    {
        // Arrange
        ParseDocumentTest("""
            @if(foo) { @::Sometext
            }
            """);
    }


    [Fact]
    public void ParsesMarkupStatementOnSwitchCharacterFollowedByTripleColon()
    {
        // Arrange
        ParseDocumentTest("""
            @if(foo) { @:::Sometext
            }
            """);
    }

    [Fact]
    public void ParsesMarkupStatementOnSwitchCharacterFollowedByColonInCodeBlock()
    {
        // Arrange
        ParseDocumentTest("""
            @{ if(foo) { @:Bar
            } } zoop
            """);
    }

    [Fact]
    public void CorrectlyReturnsFromMarkupBlockWithPseudoTag()
    {
        ParseDocumentTest("@if (i > 0) { <text>;</text> }");
    }

    [Fact]
    public void CorrectlyReturnsFromMarkupBlockWithPseudoTagInCodeBlock()
    {
        ParseDocumentTest("@{ if (i > 0) { <text>;</text> } }");
    }

    [Fact]
    public void SupportsAllKindsOfImplicitMarkupInCodeBlock()
    {
        ParseDocumentTest("""
            @{
                if(true) {
                    @:Single Line Markup
                }
                foreach (var p in Enumerable.Range(1, 10)) {
                    <text>The number is @p</text>
                }
                if(!false) {
                    <p>A real tag!</p>
                }
            }
            """);
    }

    [Fact, WorkItem("https://github.com/dotnet/razor/issues/10358")]
    public void CodeBlocksTrailingWhitespace_01()
    {
        ParseDocumentTest("""
            @code {
            }

            """, [ComponentCodeDirective.Directive]);
    }

    [Fact, WorkItem("https://github.com/dotnet/razor/issues/10358")]
    public void CodeBlocksTrailingWhitespace_02()
    {
        ParseDocumentTest("""
            @code{
            }                    

            """, [ComponentCodeDirective.Directive]);
    }

    [Fact, WorkItem("https://github.com/dotnet/razor/issues/10358")]
    public void CodeBlocksTrailingWhitespace_03()
    {
        ParseDocumentTest("""
            @code{
            }                    @* comment *@

            """, [ComponentCodeDirective.Directive]);
    }

    [Fact, WorkItem("https://github.com/dotnet/razor/issues/10358")]
    public void CodeBlocksTrailingWhitespace_04()
    {
        ParseDocumentTest("""
            @code{
            }
            @* comment *@

            """, [ComponentCodeDirective.Directive]);
    }

    [Fact, WorkItem("https://github.com/dotnet/razor/issues/10358")]
    public void CodeBlocksTrailingWhitespace_05()
    {
        ParseDocumentTest("""
            @code {
            }

            @code {
            }

            """, [ComponentCodeDirective.Directive]);
    }

    [Fact, WorkItem("https://github.com/dotnet/razor/issues/10358")]
    public void CodeBlocksTrailingWhitespace_06()
    {
        ParseDocumentTest("""
            @code {
            }

            <div></div>

            """, [ComponentCodeDirective.Directive]);
    }

    [Fact, WorkItem("https://github.com/dotnet/razor/issues/10358")]
    public void CodeBlocksTrailingWhitespace_07()
    {
        ParseDocumentTest("""
            @code {
               
            }
            <div></div>

            """, [ComponentCodeDirective.Directive]);
    }

    [Fact, WorkItem("https://github.com/dotnet/razor/issues/10358")]
    public void CodeBlocksTrailingWhitespace_08()
    {
        ParseDocumentTest("""
            @code {
               
            }      <div></div>

            """, [ComponentCodeDirective.Directive]);
    }

    [Fact, WorkItem("https://github.com/dotnet/razor/issues/10358")]
    public void CodeBlocksTrailingWhitespace_09()
    {
        ParseDocumentTest("""
            @code {
               
            }<div></div>

            """, [ComponentCodeDirective.Directive]);
    }

    [Fact, WorkItem("https://github.com/dotnet/razor/issues/10358")]
    public void CodeBlocksTrailingWhitespace_10()
    {
        var tree1 = ParseDocument("""
            @code{
            }

            """,
            directives: [ComponentCodeDirective.Directive]);

        var codeBlock = tree1.Root.ChildNodes()[0].ChildNodes()[1];
        Assert.Equal("CSharpCodeBlockSyntax<CSharpCodeBlock> at 0::11", codeBlock.ToString());

        var children = codeBlock.ChildNodes();
        Assert.Equal(2, children.Count);

        var directive = children[0];
        Assert.Equal("RazorDirectiveSyntax<RazorDirective> at 0::9", directive.ToString());

        var whitespace = children[1];
        Assert.Equal("RazorMetaCodeSyntax<RazorMetaCode> at 9::2", whitespace.ToString());
    }
}
