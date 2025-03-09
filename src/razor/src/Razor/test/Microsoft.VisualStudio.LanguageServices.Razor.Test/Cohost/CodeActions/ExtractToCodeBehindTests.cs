// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using WorkspacesSR = Microsoft.CodeAnalysis.Razor.Workspaces.Resources.SR;

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
            codeActionName: WorkspacesSR.ExtractTo_CodeBehind_Title,
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
