// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost.CodeActions;

public class PromoteUsingDirectiveTests(ITestOutputHelper testOutputHelper) : CohostCodeActionsEndpointTestBase(testOutputHelper)
{
    [Fact]
    public async Task PromoteUsingDirective()
    {
        await VerifyCodeActionAsync(
            input: """
                @using [||]System

                <div>
                    Hello World
                </div>
                """,
            expected: """

                <div>
                    Hello World
                </div>
                """,
            codeActionName: LanguageServerConstants.CodeActions.PromoteUsingDirective,
            additionalExpectedFiles: [
                (FileUri(@"..\_Imports.razor"), """
                    @using System
                    """)]);
    }

    [Fact]
    public async Task Indented()
    {
        await VerifyCodeActionAsync(
            input: """
                <div>
                    @using [||]System
                </div>

                <div>
                    Hello World
                </div>
                """,
            expected: """
                <div>
                </div>

                <div>
                    Hello World
                </div>
                """,
            codeActionName: LanguageServerConstants.CodeActions.PromoteUsingDirective,
            additionalExpectedFiles: [
                (FileUri(@"..\_Imports.razor"), """
                    @using System
                    """)]);
    }

    [Fact]
    public async Task Mvc()
    {
        await VerifyCodeActionAsync(
            input: """
                @using [||]System

                <div>
                    Hello World
                </div>
                """,
            expected: """

                <div>
                    Hello World
                </div>
                """,
            codeActionName: LanguageServerConstants.CodeActions.PromoteUsingDirective,
            fileKind: FileKinds.Legacy,
            additionalExpectedFiles: [
                (FileUri(@"..\_ViewImports.cshtml"), """
                    @using System
                    """)]);
    }

    [Fact]
    public async Task ExistingImports()
    {
        await VerifyCodeActionAsync(
            input: """
                @using [||]System

                <div>
                    Hello World
                </div>
                """,
            additionalFiles: [
                (FilePath(@"..\_Imports.razor"), """
                    @using System.Text
                    @using Foo.Bar
                    """)],
            expected: """

                <div>
                    Hello World
                </div>
                """,
            codeActionName: LanguageServerConstants.CodeActions.PromoteUsingDirective,
            additionalExpectedFiles: [
                (FileUri(@"..\_Imports.razor"), """
                    @using System.Text
                    @using Foo.Bar
                    @using System
                    """)]);
    }

    [Fact]
    public async Task ExistingImports_BlankLineAtEnd()
    {
        await VerifyCodeActionAsync(
            input: """
                @using [||]System

                <div>
                    Hello World
                </div>
                """,
            additionalFiles: [
                (FilePath(@"..\_Imports.razor"), """
                    @using System.Text
                    @using Foo.Bar
                    
                    """)],
            expected: """

                <div>
                    Hello World
                </div>
                """,
            codeActionName: LanguageServerConstants.CodeActions.PromoteUsingDirective,
            additionalExpectedFiles: [
                (FileUri(@"..\_Imports.razor"), """
                    @using System.Text
                    @using Foo.Bar
                    @using System
                    """)]);
    }

    [Fact]
    public async Task ExistingImports_WhitespaceLineAtEnd()
    {
        await VerifyCodeActionAsync(
            input: """
                @using [||]System

                <div>
                    Hello World
                </div>
                """,
            additionalFiles: [
                (FilePath(@"..\_Imports.razor"), """
                    @using System.Text
                    @using Foo.Bar
                        
                    """)],
            expected: """

                <div>
                    Hello World
                </div>
                """,
            codeActionName: LanguageServerConstants.CodeActions.PromoteUsingDirective,
            additionalExpectedFiles: [
                (FileUri(@"..\_Imports.razor"), """
                    @using System.Text
                    @using Foo.Bar
                    @using System    
                    """)]);
    }
}
