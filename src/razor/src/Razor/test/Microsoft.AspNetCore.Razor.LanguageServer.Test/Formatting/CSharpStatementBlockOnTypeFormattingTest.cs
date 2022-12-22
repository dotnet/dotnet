// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Formatting;

public class CSharpStatementBlockOnTypeFormattingTest : FormattingTestBase
{
    public CSharpStatementBlockOnTypeFormattingTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
    }

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

    [Fact]
    public async Task CloseCurly_IfBlock_MultiLineAsync()
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
            triggerCharacter: '}');
    }

    [Fact]
    public async Task CloseCurly_MultipleStatementBlocksAsync()
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
            triggerCharacter: '}');
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
}
