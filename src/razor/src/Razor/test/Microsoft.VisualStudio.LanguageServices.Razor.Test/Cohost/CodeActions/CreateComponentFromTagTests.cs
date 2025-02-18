// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Test.Common;
using Xunit.Abstractions;
using WorkspacesSR = Microsoft.CodeAnalysis.Razor.Workspaces.Resources.SR;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost.CodeActions;

public class CreateComponentFromTagTests(FuseTestContext context, ITestOutputHelper testOutputHelper) : CohostCodeActionsEndpointTestBase(context, testOutputHelper)
{
    [FuseFact]
    public async Task CreateComponentFromTag()
    {
        await VerifyCodeActionAsync(
            input: """
                <div></div>

                <He[||]llo></Hello>
                """,
            expected: """
                <div></div>

                <Hello><Hello>
                """,
            codeActionName: WorkspacesSR.Create_Component_FromTag_Title,
            additionalExpectedFiles: [
                (FileUri("Hello.razor"), "")]);
    }

    [FuseFact]
    public async Task Attribute()
    {
        await VerifyCodeActionAsync(
            input: """
                <div></div>

                <Hello wor[||]ld="true"></Hello>
                """,
            expected: """
                <div></div>

                <Hello><Hello>
                """,
            codeActionName: WorkspacesSR.Create_Component_FromTag_Title,
            additionalExpectedFiles: [
                (FileUri("Hello.razor"), "")]);
    }
}
