// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Test.Common;
using Xunit.Abstractions;
using WorkspacesSR = Microsoft.CodeAnalysis.Razor.Workspaces.Resources.SR;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost.CodeActions;

public class ExtractToComponentTests(FuseTestContext context, ITestOutputHelper testOutputHelper) : CohostCodeActionsEndpointTestBase(context, testOutputHelper)
{
    [FuseFact]
    public async Task ExtractToComponent()
    {
        await VerifyCodeActionAsync(
            input: """
                <div></div>

                [|<div>
                    Hello World
                </div>|]

                <div></div>
                """,
            expected: """
                <div></div>

                <Component />

                <div></div>
                """,
            codeActionName: WorkspacesSR.ExtractTo_Component_Title,
            additionalExpectedFiles: [
                (FileUri("Component.razor"), """
                    <div>
                        Hello World
                    </div>
                    """)]);
    }
}
