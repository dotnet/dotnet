// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost.CodeActions;

public class ExtractToCodeBehindTests(ITestOutputHelper testOutputHelper) : CohostCodeActionsEndpointTestBase(testOutputHelper)
{
    [Fact]
    public async Task ExtractToCodeBehind()
    {
        await VerifyCodeActionAsync(
            input: """
                <div></div>

                @co[||]de
                {
                    private int x = 1;
                }
                """,
            expected: """
                <div></div>


                """,
            codeActionName: LanguageServerConstants.CodeActions.ExtractToCodeBehindAction,
            additionalExpectedFiles: [
                (FileUri("File1.razor.cs"), $$"""
                    namespace SomeProject
                    {
                        public partial class File1
                        {
                            private int x = 1;
                        }
                    }
                    """)]);
    }

    [Theory]
    [InlineData("[||]@code {")]
    [InlineData("@[||]code {")]
    [InlineData("@c[||]ode {")]
    [InlineData("@co[||]de {")]
    [InlineData("@cod[||]e {")]
    [InlineData("@code[||] {")]
    [InlineData("@code[||]{")]
    public async Task WorkAtAnyCursorPosition(string codeBlockStart)
    {
        await VerifyCodeActionAsync(
            input: $$"""
                <div></div>

                {{codeBlockStart}}
                    private int x = 1;
                }
                """,
            expected: """
                <div></div>


                """,
            codeActionName: LanguageServerConstants.CodeActions.ExtractToCodeBehindAction,
            additionalExpectedFiles: [
                (FileUri("File1.razor.cs"), $$"""
                    namespace SomeProject
                    {
                        public partial class File1
                        {
                            private int x = 1;
                        }
                    }
                    """)]);
    }
}
