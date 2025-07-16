// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

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
            codeActionName: LanguageServerConstants.CodeActions.ExtractToNewComponent,
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
            codeActionName: LanguageServerConstants.CodeActions.ExtractToNewComponent);
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
            codeActionName: LanguageServerConstants.CodeActions.ExtractToNewComponent,
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
            codeActionName: LanguageServerConstants.CodeActions.ExtractToNewComponent,
            additionalExpectedFiles: [
                (FileUri("Component.razor"), """
                    @namespace MyHero

                    <div>
                        Hello World
                    </div>
                    """)]);
    }
}
