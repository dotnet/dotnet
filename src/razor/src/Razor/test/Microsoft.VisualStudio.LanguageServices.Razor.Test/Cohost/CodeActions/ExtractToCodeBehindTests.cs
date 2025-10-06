// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

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
            codeActionName: LanguageServerConstants.CodeActions.ExtractToCodeBehind,
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
            codeActionName: LanguageServerConstants.CodeActions.ExtractToCodeBehind,
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
