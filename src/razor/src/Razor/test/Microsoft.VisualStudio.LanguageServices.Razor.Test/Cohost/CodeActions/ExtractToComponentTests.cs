// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost.CodeActions;

public class ExtractToComponentTests(ITestOutputHelper testOutputHelper) : CohostCodeActionsEndpointTestBase(testOutputHelper)
{
    [Fact]
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
            codeActionName: LanguageServerConstants.CodeActions.ExtractToNewComponentAction,
            additionalExpectedFiles: [
                (FileUri("Component.razor"), """
                    <div>
                        Hello World
                    </div>
                    """)]);
    }

    [Fact]
    public async Task DontOfferOnNonExistentComponent()
    {
        await VerifyCodeActionAsync(
            input: """
                <div></div>

                <div>
                    Hello World
                </div>

                <{|RZ10012:Not$$AComponent|} />

                <div></div>
                """,
            expected: null,
            codeActionName: LanguageServerConstants.CodeActions.ExtractToNewComponentAction);
    }

    [Fact]
    public async Task ExtractNamespace()
    {
        await VerifyCodeActionAsync(
            input: """
                @namespace ILoveYou

                <div></div>

                [|<div>
                    Hello World
                </div>|]

                <div></div>
                """,
            expected: """
                @namespace ILoveYou

                <div></div>

                <Component />

                <div></div>
                """,
            codeActionName: LanguageServerConstants.CodeActions.ExtractToNewComponentAction,
            additionalExpectedFiles: [
                (FileUri("Component.razor"), """
                    @namespace ILoveYou

                    <div>
                        Hello World
                    </div>
                    """)]);
    }

    [Fact]
    public async Task ExtractNamespace_Pathological()
    {
        await VerifyCodeActionAsync(
            input: """
                @namespace DidYouEverKnow
                @namespace ThatYoure
                @namespace MyHero

                <div></div>

                [|<div>
                    Hello World
                </div>|]

                <div></div>
                """,
            expected: """
                @namespace DidYouEverKnow
                @namespace ThatYoure
                @namespace MyHero

                <div></div>

                <Component />

                <div></div>
                """,
            codeActionName: LanguageServerConstants.CodeActions.ExtractToNewComponentAction,
            additionalExpectedFiles: [
                (FileUri("Component.razor"), """
                    @namespace MyHero

                    <div>
                        Hello World
                    </div>
                    """)]);
    }
}
