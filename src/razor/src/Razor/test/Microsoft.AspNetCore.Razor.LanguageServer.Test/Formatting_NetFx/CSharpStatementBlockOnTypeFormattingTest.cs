// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor.Formatting;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Formatting;

[Collection(HtmlFormattingCollection.Name)]
public class CSharpStatementBlockOnTypeFormattingTest(HtmlFormattingFixture fixture, ITestOutputHelper testOutput)
    : FormattingTestBase(fixture.Service, testOutput)
{
    [Fact]
    public async Task CloseCurly_IfBlock_SingleLineAsync()
    {
        await RunOnTypeFormattingTestAsync(
            input: """
                    @{
                     if(true){}$$
                    }
                    """,
            expected: """
                    @{
                        if (true) { }
                    }
                    """,
            triggerCharacter: '}');
    }

    [Theory, CombinatorialData]
    public async Task CloseCurly_IfBlock_MultiLineAsync(bool inGlobalNamespace)
    {
        await RunOnTypeFormattingTestAsync(
            input: """
                    @{
                     if(true)
                    {
                     }$$
                    }
                    """,
            expected: """
                    @{
                        if (true)
                        {
                        }
                    }
                    """,
            triggerCharacter: '}',
            inGlobalNamespace: inGlobalNamespace);
    }

    [Theory, CombinatorialData]
    public async Task CloseCurly_MultipleStatementBlocksAsync(bool inGlobalNamespace)
    {
        await RunOnTypeFormattingTestAsync(
            input: """
                    <div>
                        @{
                          if(true) { }
                        }
                    </div>

                    @{
                     if(true)
                    {
                     }$$
                    }
                    """,
            expected: """
                    <div>
                        @{
                            if(true) { }
                        }
                    </div>

                    @{
                        if (true)
                        {
                        }
                    }
                    """,
            triggerCharacter: '}',
            inGlobalNamespace: inGlobalNamespace);
    }

    [Fact]
    public async Task Semicolon_Variable_SingleLineAsync()
    {
        await RunOnTypeFormattingTestAsync(
            input: """
                    @{
                     var x = 'foo';$$
                    }
                    """,
            expected: """
                    @{
                        var x = 'foo';
                    }
                    """,
            triggerCharacter: ';');
    }

    [Fact]
    public async Task Semicolon_Variable_MultiLineAsync()
    {
        await RunOnTypeFormattingTestAsync(
            input: """
                    @{
                     var x = @"
                    foo";$$
                    }
                    """,
            expected: """
                    @{
                        var x = @"
                    foo";
                    }
                    """,
            triggerCharacter: ';');
    }

    [Fact]
    public async Task Semicolon_AddsLineAtEndOfDocument()
    {
        await RunOnTypeFormattingTestAsync(
            input: """
                    @{ var x = new HtmlString("sdf");$$ }
                    """,
            expected: """
                    @{
                        var x = new HtmlString("sdf"); 
                    }
                    """,
            triggerCharacter: ';');
    }
}
