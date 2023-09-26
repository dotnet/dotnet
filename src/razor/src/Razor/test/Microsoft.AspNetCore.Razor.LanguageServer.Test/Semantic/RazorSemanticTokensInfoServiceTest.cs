// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Razor.Workspaces.Extensions;
using Microsoft.CommonLanguageServerProtocol.Framework;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Semantic;

public abstract class RazorSemanticTokensInfoServiceTest : SemanticTokenTestBase
{
    private protected readonly Mock<ClientNotifierServiceBase> _languageServer;
    public RazorSemanticTokensInfoServiceTest(ITestOutputHelper testOutput, bool usePreciseSemanticTokenRanges)
        : base(testOutput, usePreciseSemanticTokenRanges)
    {
        _languageServer = new Mock<ClientNotifierServiceBase>(MockBehavior.Strict);
    }

    [Fact]
    public async Task GetSemanticTokens_CSharp_RazorIfNotReady()
    {
        var documentText =
            """
                <p></p>@{
                    var d = "t";
                }
                """;

        var razorRange = GetRange(documentText);
        var csharpTokens = new ProvideSemanticTokensResponse(tokens: Array.Empty<int>(), hostDocumentSyncVersion: 1);
        await AssertSemanticTokensAsync(documentText, isRazorFile: false, razorRange, csharpTokens: csharpTokens, documentVersion: 1);
    }

    [Fact]
    public async Task GetSemanticTokens_CSharpBlock_HTML()
    {
        var documentText =
            """
                @{
                    var d = "t";
                    <p>HTML @d</p>
                }
                """;

        var razorRange = GetRange(documentText);
        var csharpTokens = await GetCSharpSemanticTokensResponseAsync(documentText, razorRange, isRazorFile: false);
        await AssertSemanticTokensAsync(documentText, isRazorFile: false, razorRange, csharpTokens: csharpTokens);
        Assert.NotNull(csharpTokens.Tokens);
        Assert.NotEmpty(csharpTokens.Tokens);
    }

    [Fact]
    public async Task GetSemanticTokens_CSharp_Nested_HTML()
    {
        var documentText =
            """
                @addTagHelper *, TestAssembly
                <!--@{var d = "string";@<a></a>}-->
                """;

        var razorRange = GetRange(documentText);
        var csharpTokens = await GetCSharpSemanticTokensResponseAsync(documentText, razorRange, isRazorFile: false);
        await AssertSemanticTokensAsync(documentText, isRazorFile: false, razorRange, csharpTokens: csharpTokens);
        Assert.NotNull(csharpTokens.Tokens);
        Assert.NotEmpty(csharpTokens.Tokens);
    }

    [Fact]
    public async Task GetSemanticTokens_CSharp_VSCodeWorks()
    {
        var documentText =
            """
                @addTagHelper *, TestAssembly
                @{ var d = }
                """;

        var razorRange = GetRange(documentText);
        var csharpTokens = new ProvideSemanticTokensResponse(tokens: Array.Empty<int>(), hostDocumentSyncVersion: 1);
        await AssertSemanticTokensAsync(documentText, isRazorFile: false, razorRange, csharpTokens: csharpTokens, documentVersion: 1);
    }

    [Fact]
    public async Task GetSemanticTokens_CSharp_Explicit()
    {
        var documentText =
            """
                @using System
                @addTagHelper *, TestAssembly
                @(DateTime.Now)
                """;

        var razorRange = GetRange(documentText);
        var csharpTokens = await GetCSharpSemanticTokensResponseAsync(documentText, razorRange, isRazorFile: false);
        await AssertSemanticTokensAsync(documentText, isRazorFile: false, razorRange, csharpTokens: csharpTokens);
        Assert.NotNull(csharpTokens.Tokens);
        Assert.NotEmpty(csharpTokens.Tokens);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task GetSemanticTokens_CSharp_Implicit(bool serverSupportsPreciseRanges)
    {
        var documentText =
            """
                @addTagHelper *, TestAssembly
                @{ var d = "txt";}
                @d
                """;

        var razorRange = GetRange(documentText);
        var csharpTokens = await GetCSharpSemanticTokensResponseAsync(documentText, razorRange, isRazorFile: false);
        await AssertSemanticTokensAsync(documentText, isRazorFile: false, razorRange, csharpTokens: csharpTokens, serverSupportsPreciseRanges: serverSupportsPreciseRanges);
        VerifyTimesLanguageServerCalled(serverSupportsPreciseRanges);
        Assert.NotNull(csharpTokens.Tokens);
        Assert.NotEmpty(csharpTokens.Tokens);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task GetSemanticTokens_CSharp_VersionMismatch(bool serverSupportsPreciseRanges)
    {
        var documentText =
            """
                @addTagHelper *, TestAssembly
                @{ var d = }
                """;

        var razorRange = GetRange(documentText);
        var csharpTokens = await GetCSharpSemanticTokensResponseAsync(documentText, razorRange, isRazorFile: false);
        await AssertSemanticTokensAsync(documentText, isRazorFile: false, razorRange, csharpTokens: csharpTokens, documentVersion: 21, serverSupportsPreciseRanges: serverSupportsPreciseRanges);
        VerifyTimesLanguageServerCalled(serverSupportsPreciseRanges);
        Assert.NotNull(csharpTokens.Tokens);
        Assert.NotEmpty(csharpTokens.Tokens);
    }

    [Fact]
    public async Task GetSemanticTokens_CSharp_FunctionAsync()
    {
        var documentText =
            """
                @addTagHelper *, TestAssembly
                @{ var d = }
                """;

        var razorRange = GetRange(documentText);
        var csharpTokens = await GetCSharpSemanticTokensResponseAsync(documentText, razorRange, isRazorFile: false);
        await AssertSemanticTokensAsync(documentText, isRazorFile: false, razorRange, csharpTokens: csharpTokens);
        Assert.NotNull(csharpTokens.Tokens);
        Assert.NotEmpty(csharpTokens.Tokens);
    }

    [Fact]
    public async Task GetSemanticTokens_CSharp_StaticModifier()
    {
        var documentText =
            """
                @code
                {
                    static int x = 1;
                }
                """;

        var razorRange = GetRange(documentText);
        var csharpTokens = await GetCSharpSemanticTokensResponseAsync(documentText, razorRange, isRazorFile: true);
        await AssertSemanticTokensAsync(documentText, isRazorFile: true, razorRange, csharpTokens: csharpTokens);
        Assert.NotNull(csharpTokens.Tokens);
        Assert.NotEmpty(csharpTokens.Tokens);
    }

    [Fact]
    public async Task GetSemanticTokens_MultipleBlankLines()
    {
        var documentText =
            """
                @addTagHelper *, TestAssembly

                <p>first
                second</p>
                """;

        var razorRange = GetRange(documentText);
        var csharpTokens = await GetCSharpSemanticTokensResponseAsync(documentText, razorRange, isRazorFile: false);
        await AssertSemanticTokensAsync(documentText, isRazorFile: false, razorRange, csharpTokens: csharpTokens);
        Assert.NotNull(csharpTokens.Tokens);
        Assert.NotEmpty(csharpTokens.Tokens);
    }

    [Fact]
    public async Task GetSemanticTokens_IncompleteTag()
    {
        var documentText =
            """
                <str class='
                """;

        var razorRange = GetRange(documentText);
        var csharpTokens = await GetCSharpSemanticTokensResponseAsync(documentText, razorRange, isRazorFile: false);
        await AssertSemanticTokensAsync(documentText, isRazorFile: false, razorRange, csharpTokens: csharpTokens);
        Assert.NotNull(csharpTokens.Tokens);
        Assert.Empty(csharpTokens.Tokens);
    }

    [Fact]
    public async Task GetSemanticTokens_MinimizedHTMLAttribute()
    {
        var documentText =
            """
                <p attr />
                """;

        var razorRange = GetRange(documentText);
        var csharpTokens = await GetCSharpSemanticTokensResponseAsync(documentText, razorRange, isRazorFile: false);
        await AssertSemanticTokensAsync(documentText, isRazorFile: false, razorRange, csharpTokens: csharpTokens);
        Assert.NotNull(csharpTokens.Tokens);
        Assert.Empty(csharpTokens.Tokens);
    }

    [Fact]
    public async Task GetSemanticTokens_MinimizedHTMLAsync()
    {
        var documentText = """
                @addTagHelper *, TestAssembly
                <input/>
                """;

        var razorRange = GetRange(documentText);
        var csharpTokens = await GetCSharpSemanticTokensResponseAsync(documentText, razorRange, isRazorFile: false);
        await AssertSemanticTokensAsync(documentText, isRazorFile: false, razorRange, csharpTokens: csharpTokens);
        Assert.NotNull(csharpTokens.Tokens);
        Assert.NotEmpty(csharpTokens.Tokens);
    }

    [Fact]
    public async Task GetSemanticTokens_HTMLCommentAsync()
    {
        var documentText =
            """
                @addTagHelper *, TestAssembly
                <!-- comment with comma's -->
                """;

        var razorRange = GetRange(documentText);
        var csharpTokens = await GetCSharpSemanticTokensResponseAsync(documentText, razorRange, isRazorFile: false);
        await AssertSemanticTokensAsync(documentText, isRazorFile: false, razorRange, csharpTokens: csharpTokens);
        Assert.NotNull(csharpTokens.Tokens);
        Assert.NotEmpty(csharpTokens.Tokens);
    }

    [Fact]
    public async Task GetSemanticTokens_PartialHTMLCommentAsync()
    {
        var documentText =
            """
                @addTagHelper *, TestAssembly
                <!-- comment
                """;

        var razorRange = GetRange(documentText);
        var csharpTokens = await GetCSharpSemanticTokensResponseAsync(documentText, razorRange, isRazorFile: true);
        await AssertSemanticTokensAsync(documentText, isRazorFile: true, razorRange, csharpTokens: csharpTokens);
        Assert.NotNull(csharpTokens.Tokens);
        Assert.NotEmpty(csharpTokens.Tokens);
    }

    [Fact]
    public async Task GetSemanticTokens_HTMLIncludesBang()
    {
        var documentText =
            """
                @addTagHelper *, TestAssembly
                <!input/>
                """;

        var razorRange = GetRange(documentText);
        var csharpTokens = await GetCSharpSemanticTokensResponseAsync(documentText, razorRange, isRazorFile: false);
        await AssertSemanticTokensAsync(documentText, isRazorFile: false, razorRange, csharpTokens: csharpTokens);
        Assert.NotNull(csharpTokens.Tokens);
        Assert.NotEmpty(csharpTokens.Tokens);
    }

    [Fact]
    public async Task GetSemanticTokens_HalfOfCommentAsync()
    {
        var documentText =
            """
                @addTagHelper *, TestAssembly
                @* comment
                """;

        var razorRange = GetRange(documentText);
        var csharpTokens = await GetCSharpSemanticTokensResponseAsync(documentText, razorRange, isRazorFile: true);
        await AssertSemanticTokensAsync(documentText, isRazorFile: true, razorRange, csharpTokens: csharpTokens);
        Assert.NotNull(csharpTokens.Tokens);
        Assert.NotEmpty(csharpTokens.Tokens);
    }

    [Fact]
    public async Task GetSemanticTokens_NoAttributesAsync()
    {
        var documentText =
            """
                @addTagHelper *, TestAssembly
                <test1></test1>
                """;

        var razorRange = GetRange(documentText);
        var csharpTokens = await GetCSharpSemanticTokensResponseAsync(documentText, razorRange, isRazorFile: false);
        await AssertSemanticTokensAsync(documentText, isRazorFile: false, razorRange, csharpTokens: csharpTokens);
        Assert.NotNull(csharpTokens.Tokens);
        Assert.NotEmpty(csharpTokens.Tokens);
    }

    [Fact]
    public async Task GetSemanticTokens_WithAttributeAsync()
    {
        var documentText =
            """
                @addTagHelper *, TestAssembly
                <test1 bool-val='true'></test1>
                """;

        var razorRange = GetRange(documentText);
        var csharpTokens = await GetCSharpSemanticTokensResponseAsync(documentText, razorRange, isRazorFile: false);
        await AssertSemanticTokensAsync(documentText, isRazorFile: false, razorRange, csharpTokens: csharpTokens);
        Assert.NotNull(csharpTokens.Tokens);
        Assert.NotEmpty(csharpTokens.Tokens);
    }

    [Fact]
    public async Task GetSemanticTokens_MinimizedAttribute_BoundAsync()
    {
        var documentText =
            """
                @addTagHelper *, TestAssembly
                <test1 bool-val></test1>
                """;

        var razorRange = GetRange(documentText);
        var csharpTokens = await GetCSharpSemanticTokensResponseAsync(documentText, razorRange, isRazorFile: false);
        await AssertSemanticTokensAsync(documentText, isRazorFile: false, razorRange, csharpTokens: csharpTokens);
        Assert.NotNull(csharpTokens.Tokens);
        Assert.NotEmpty(csharpTokens.Tokens);
    }

    [Fact]
    public async Task GetSemanticTokens_MinimizedAttribute_NotBoundAsync()
    {
        var documentText =
            """
                @addTagHelper *, TestAssembly
                <test1 notbound></test1>
                """;

        var razorRange = GetRange(documentText);
        var csharpTokens = await GetCSharpSemanticTokensResponseAsync(documentText, razorRange, isRazorFile: false);
        await AssertSemanticTokensAsync(documentText, isRazorFile: false, razorRange, csharpTokens: csharpTokens);
        Assert.NotNull(csharpTokens.Tokens);
        Assert.NotEmpty(csharpTokens.Tokens);
    }

    [Fact]
    public async Task GetSemanticTokens_IgnoresNonTagHelperAttributesAsync()
    {
        var documentText =
            """
                @addTagHelper *, TestAssembly
                <test1 bool-val='true' class='display:none'></test1>
                """;

        var razorRange = GetRange(documentText);
        var csharpTokens = await GetCSharpSemanticTokensResponseAsync(documentText, razorRange, isRazorFile: false);
        await AssertSemanticTokensAsync(documentText, isRazorFile: false, razorRange, csharpTokens: csharpTokens);
        Assert.NotNull(csharpTokens.Tokens);
        Assert.NotEmpty(csharpTokens.Tokens);
    }

    [Fact]
    public async Task GetSemanticTokens_TagHelpersNotAvailableInRazorAsync()
    {
        var documentText =
            """
                @addTagHelper *, TestAssembly
                <test1 bool-val='true' class='display:none'></test1>
                """;

        var razorRange = GetRange(documentText);
        var csharpTokens = await GetCSharpSemanticTokensResponseAsync(documentText, razorRange, isRazorFile: true);
        await AssertSemanticTokensAsync(documentText, isRazorFile: true, razorRange, csharpTokens: csharpTokens);
        Assert.NotNull(csharpTokens.Tokens);
        Assert.NotEmpty(csharpTokens.Tokens);
    }

    [Fact]
    public async Task GetSemanticTokens_DoesNotApplyOnNonTagHelpersAsync()
    {
        var documentText =
            """
                @addTagHelper *, TestAssembly
                <p bool-val='true'></p>
                """;

        var razorRange = GetRange(documentText);
        var csharpTokens = await GetCSharpSemanticTokensResponseAsync(documentText, razorRange, isRazorFile: false);
        await AssertSemanticTokensAsync(documentText, isRazorFile: false, razorRange, csharpTokens: csharpTokens);
        Assert.NotNull(csharpTokens.Tokens);
        Assert.NotEmpty(csharpTokens.Tokens);
    }

    [Fact]
    public async Task GetSemanticTokens_Razor_MinimizedDirectiveAttributeParameters()
    {
        // Capitalized, non-well-known-HTML elements should not be marked as TagHelpers
        var documentText =
            """
                @addTagHelper *, TestAssembly
                }<NotATagHelp @minimized:something />
                """;

        var razorRange = GetRange(documentText);
        var csharpTokens = await GetCSharpSemanticTokensResponseAsync(documentText, razorRange, isRazorFile: true);
        await AssertSemanticTokensAsync(documentText, isRazorFile: true, razorRange, csharpTokens: csharpTokens);
        Assert.NotNull(csharpTokens.Tokens);
        Assert.NotEmpty(csharpTokens.Tokens);
    }

    [Fact]
    public async Task GetSemanticTokens_Razor_ComponentAttributeAsync()
    {
        var documentText =
            """
                @addTagHelper *, TestAssembly
                <Component1 bool-val=""true""></Component1>
                """;

        var razorRange = GetRange(documentText);
        var csharpTokens = await GetCSharpSemanticTokensResponseAsync(documentText, razorRange, isRazorFile: true);
        await AssertSemanticTokensAsync(documentText, isRazorFile: true, razorRange, csharpTokens: csharpTokens);
        Assert.NotNull(csharpTokens.Tokens);
        Assert.NotEmpty(csharpTokens.Tokens);
    }

    [Fact]
    public async Task GetSemanticTokens_Razor_ComponentAttribute_DoesntGetABackground()
    {
        // Need C# around the component for the C# range to be valid, to correctly validate the attribute handling
        var documentText =
            """
                @DateTime.Now

                <Component1 Title=""Hi there I'm a string""></Component1>

                @DateTime.Now
                """;

        var razorRange = GetRange(documentText);
        var csharpTokens = await GetCSharpSemanticTokensResponseAsync(documentText, razorRange, isRazorFile: true);
        await AssertSemanticTokensAsync(documentText, isRazorFile: true, razorRange, csharpTokens: csharpTokens, withCSharpBackground: true);
        Assert.NotNull(csharpTokens.Tokens);
        Assert.NotEmpty(csharpTokens.Tokens);
    }

    [Fact]
    public async Task GetSemanticTokens_Razor_DirectiveAttributesParametersAsync()
    {
        var documentText =
            """
                @addTagHelper *, TestAssembly
                <Component1 @test:something='Function'></Component1>
                """;

        var razorRange = GetRange(documentText);
        var csharpTokens = await GetCSharpSemanticTokensResponseAsync(documentText, razorRange, isRazorFile: true);
        await AssertSemanticTokensAsync(documentText, isRazorFile: true, razorRange, csharpTokens: csharpTokens);
        Assert.NotNull(csharpTokens.Tokens);
        Assert.NotEmpty(csharpTokens.Tokens);
    }

    [Fact]
    public async Task GetSemanticTokens_Razor_NonComponentsDoNotShowInRazorAsync()
    {
        var documentText =
            """
                @addTagHelper *, TestAssembly
                <test1 bool-val='true'></test1>
                """;

        var razorRange = GetRange(documentText);
        var csharpTokens = await GetCSharpSemanticTokensResponseAsync(documentText, razorRange, isRazorFile: true);
        await AssertSemanticTokensAsync(documentText, isRazorFile: true, razorRange, csharpTokens: csharpTokens);
        Assert.NotNull(csharpTokens.Tokens);
        Assert.NotEmpty(csharpTokens.Tokens);
    }

    [Fact]
    public async Task GetSemanticTokens_Razor_DirectivesAsync()
    {
        var documentText =
            """
                @addTagHelper *, TestAssembly
                <Component1 @test='Function'></Component1>
                """;

        var razorRange = GetRange(documentText);
        var csharpTokens = await GetCSharpSemanticTokensResponseAsync(documentText, razorRange, isRazorFile: true);
        await AssertSemanticTokensAsync(documentText, isRazorFile: true, razorRange, csharpTokens: csharpTokens);
        Assert.NotNull(csharpTokens.Tokens);
        Assert.NotEmpty(csharpTokens.Tokens);
    }

    [Fact]
    public async Task GetSemanticTokens_HandleTransitionEscape()
    {
        var documentText =
            """
                @@text
                """;

        var razorRange = GetRange(documentText);
        var csharpTokens = await GetCSharpSemanticTokensResponseAsync(documentText, razorRange, isRazorFile: true);
        await AssertSemanticTokensAsync(documentText, isRazorFile: true, razorRange, csharpTokens: csharpTokens);
        Assert.NotNull(csharpTokens.Tokens);
        Assert.Empty(csharpTokens.Tokens);
    }

    [Fact]
    public async Task GetSemanticTokens_Razor_DoNotColorNonTagHelpersAsync()
    {
        var documentText =
            """
                <p @test='Function'></p>
                """;

        var razorRange = GetRange(documentText);
        var csharpTokens = await GetCSharpSemanticTokensResponseAsync(documentText, razorRange, isRazorFile: true);
        await AssertSemanticTokensAsync(documentText, isRazorFile: true, razorRange, csharpTokens: csharpTokens);
        Assert.NotNull(csharpTokens.Tokens);
        Assert.Empty(csharpTokens.Tokens);
    }

    [Fact]
    public async Task GetSemanticTokens_Razor_DoesNotApplyOnNonTagHelpersAsync()
    {
        var documentText = """
                @addTagHelpers *, TestAssembly
                <p></p>
                """;

        var razorRange = GetRange(documentText);
        var csharpTokens = await GetCSharpSemanticTokensResponseAsync(documentText, razorRange, isRazorFile: true);
        await AssertSemanticTokensAsync(documentText, isRazorFile: true, razorRange, csharpTokens: csharpTokens);
        Assert.NotNull(csharpTokens.Tokens);
        Assert.NotEmpty(csharpTokens.Tokens);
    }

    [Fact]
    public async Task GetSemanticTokens_Razor_CodeDirectiveAsync()
    {
        var documentText =
            """
                @code {}
                """;

        var razorRange = GetRange(documentText);
        var csharpTokens = await GetCSharpSemanticTokensResponseAsync(documentText, razorRange, isRazorFile: true);
        await AssertSemanticTokensAsync(documentText, isRazorFile: true, razorRange, csharpTokens: csharpTokens);
        Assert.NotNull(csharpTokens.Tokens);
        Assert.Empty(csharpTokens.Tokens);
    }

    [Fact]
    public async Task GetSemanticTokens_Razor_CodeDirectiveBodyAsync()
    {
        var documentText = """
                @using System
                @code {
                    public void SomeMethod()
                    {
                        @DateTime.Now
                    }
                }
                """;

        var razorRange = GetRange(documentText);
        var csharpTokens = await GetCSharpSemanticTokensResponseAsync(documentText, razorRange, isRazorFile: true);
        await AssertSemanticTokensAsync(documentText, isRazorFile: true, razorRange, csharpTokens: csharpTokens);
        Assert.NotNull(csharpTokens.Tokens);
        Assert.NotEmpty(csharpTokens.Tokens);
    }

    [Fact]
    public async Task GetSemanticTokens_Razor_UsingDirective()
    {
        var documentText =
            """
                @using System.Threading
                """;

        var razorRange = GetRange(documentText);
        var csharpTokens = await GetCSharpSemanticTokensResponseAsync(documentText, razorRange, isRazorFile: true);
        await AssertSemanticTokensAsync(documentText, isRazorFile: true, razorRange, csharpTokens: csharpTokens);
        Assert.NotNull(csharpTokens.Tokens);
        Assert.NotEmpty(csharpTokens.Tokens);
    }

    [Fact]
    public async Task GetSemanticTokens_Razor_FunctionsDirectiveAsync()
    {
        var documentText =
            """
                @functions {}
                """;

        var razorRange = GetRange(documentText);
        var csharpTokens = await GetCSharpSemanticTokensResponseAsync(documentText, razorRange, isRazorFile: true);
        await AssertSemanticTokensAsync(documentText, isRazorFile: true, razorRange, csharpTokens: csharpTokens);
        Assert.NotNull(csharpTokens.Tokens);
        Assert.Empty(csharpTokens.Tokens);
    }

    [Fact]
    public async Task GetSemanticTokens_Razor_NestedTextDirectives()
    {
        var documentText =
            """
                @using System
                @functions {
                    private void BidsByShipment(string generatedId, int bids)
                    {
                        if (bids > 0)
                        {
                            <a class=""Thing"">
                                @if(bids > 0)
                                {
                                    <text>@DateTime.Now</text>
                                }
                            </a>
                        }
                    }
                }
                """;

        var razorRange = GetRange(documentText);
        var csharpTokens = await GetCSharpSemanticTokensResponseAsync(documentText, razorRange, isRazorFile: false);
        await AssertSemanticTokensAsync(documentText, isRazorFile: false, razorRange, csharpTokens: csharpTokens);
        Assert.NotNull(csharpTokens.Tokens);
        Assert.NotEmpty(csharpTokens.Tokens);
    }

    [Fact]
    public async Task GetSemanticTokens_Razor_NestedTransitions()
    {
        var documentText =
            """
                @using System
                @functions {
                    Action<object> abc = @<span></span>;
                }
                """;

        var razorRange = GetRange(documentText);
        var csharpTokens = await GetCSharpSemanticTokensResponseAsync(documentText, razorRange, isRazorFile: true);
        await AssertSemanticTokensAsync(documentText, isRazorFile: true, razorRange, csharpTokens: csharpTokens);
        Assert.NotNull(csharpTokens.Tokens);
        Assert.NotEmpty(csharpTokens.Tokens);
    }

    [Fact]
    public async Task GetSemanticTokens_Razor_CommentAsync()
    {
        var documentText = """
                @* A comment *@
                """;

        var razorRange = GetRange(documentText);
        var csharpTokens = await GetCSharpSemanticTokensResponseAsync(documentText, razorRange, isRazorFile: true);
        await AssertSemanticTokensAsync(documentText, isRazorFile: true, razorRange, csharpTokens: csharpTokens);
        Assert.NotNull(csharpTokens.Tokens);
        Assert.Empty(csharpTokens.Tokens);
    }

    [Fact]
    public async Task GetSemanticTokens_Razor_MultiLineCommentMidlineAsync()
    {
        var documentText =
            """
                <a />@* kdl
                skd
                slf*@
                """;

        var razorRange = GetRange(documentText);
        var csharpTokens = await GetCSharpSemanticTokensResponseAsync(documentText, razorRange, isRazorFile: false);
        await AssertSemanticTokensAsync(documentText, isRazorFile: false, razorRange, csharpTokens: csharpTokens);
        Assert.NotNull(csharpTokens.Tokens);
        Assert.Empty(csharpTokens.Tokens);
    }

    [Fact]
    public async Task GetSemanticTokens_Razor_MultiLineCommentWithBlankLines()
    {
        var documentText =
            """
                @* kdl

                skd
                    
                        sdfasdfasdf
                slf*@
                """;

        var razorRange = GetRange(documentText);
        var csharpTokens = await GetCSharpSemanticTokensResponseAsync(documentText, razorRange, isRazorFile: false);
        await AssertSemanticTokensAsync(documentText, isRazorFile: false, razorRange, csharpTokens: csharpTokens);
        Assert.NotNull(csharpTokens.Tokens);
        Assert.Empty(csharpTokens.Tokens);
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/razor/issues/8176")]
    public async Task GetSemanticTokens_Razor_MultiLineCommentWithBlankLines_LF()
    {
        var documentText = "@* kdl\n\nskd\n    \n        sdfasdfasdf\nslf*@";

        var razorRange = GetRange(documentText);
        var csharpTokens = await GetCSharpSemanticTokensResponseAsync(documentText, razorRange, isRazorFile: false);
        await AssertSemanticTokensAsync(documentText, isRazorFile: false, razorRange, csharpTokens: csharpTokens);
        Assert.NotNull(csharpTokens.Tokens);
        Assert.Empty(csharpTokens.Tokens);
    }

    [Fact]
    public async Task GetSemanticTokens_Razor_MultiLineCommentAsync()
    {
        var documentText =
            """
                @*stuff
                things *@
                """;

        var razorRange = GetRange(documentText);
        var csharpTokens = await GetCSharpSemanticTokensResponseAsync(documentText, razorRange, isRazorFile: false);
        await AssertSemanticTokensAsync(documentText, isRazorFile: false, razorRange, csharpTokens: csharpTokens);
        Assert.NotNull(csharpTokens.Tokens);
        Assert.Empty(csharpTokens.Tokens);
    }

    [Fact]
    public async Task GetSemanticTokens_CSharp_Static()
    {
        var documentText = """
                @using System
                @code
                {
                    private static bool _isStatic;

                    public void M()
                    {
                        if (_isStatic)
                        {
                        }
                    }
                }
                """;

        var razorRange = GetRange(documentText);
        var csharpTokens = await GetCSharpSemanticTokensResponseAsync(documentText, razorRange, isRazorFile: true);
        await AssertSemanticTokensAsync(documentText, isRazorFile: true, razorRange, csharpTokens: csharpTokens);
        Assert.NotNull(csharpTokens.Tokens);
        Assert.NotEmpty(csharpTokens.Tokens);
    }

    [Fact]
    public async Task GetSemanticTokens_CSharp_LargeFile()
    {
        var start = """
                @page
                @model SampleApp.Pages.ErrorModel
                @using System

                <!DOCTYPE html>
                <html lang="en">

                <head>
                    <meta charset="utf-8" />
                    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no" />
                    <title>Error</title>
                    <link href="~/css/bootstrap/bootstrap.min.css" rel="stylesheet" />
                    <link href="~/css/site.css" rel="stylesheet" asp-append-version="true" />
                </head>

                <body>
                """;
        var middle = """
                    <div class="@cssClass">
                        <div class="content px-4">
                            @using System
                            <h1 class="text-danger">Error.</h1>
                            <h2 class="text-danger">An error occurred while processing your request.</h2>

                            @if (Model.ShowRequestId)
                            {
                                <p>
                                    <strong>Request ID:</strong> <code>@Model.RequestId</code>
                                </p>
                            }

                            <h3>Development Mode</h3>
                            @if (true)
                            {
                                <p>
                                    Swapping to the <strong>@DateTime.Now</strong> environment displays detailed information about the error that occurred.
                                </p>
                            }
                            <p>
                                @if (false)
                                {
                                    <strong>The Development environment shouldn't be enabled for deployed applications.</strong>
                                }
                                It can result in displaying sensitive information from exceptions to end users.
                                @if (true)
                                {
                                    <text>For local debugging, enable the <strong>@Environment.NewLine</strong> environment by setting the <strong>ASPNETCORE_ENVIRONMENT</strong> environment variable to <strong>Development</strong>
                                    and restarting the app.</text>
                                }
                            </p>
                        </div>
                    </div>
                """;
        var end = """
                </body>

                </html>
                """;

        var builder = new StringBuilder();
        builder.AppendLine(start);
        for (var i = 0; i < 100; i++)
        {
            builder.AppendLine(middle);
        }

        builder.AppendLine(end);

        var documentText = builder.ToString();
        var razorRange = GetRange(documentText);
        var csharpTokens = await GetCSharpSemanticTokensResponseAsync(documentText, razorRange, isRazorFile: true);
        await AssertSemanticTokensAsync(documentText, isRazorFile: true, razorRange, csharpTokens: csharpTokens);
        Assert.NotNull(csharpTokens.Tokens);
        Assert.NotEmpty(csharpTokens.Tokens);
    }

    [Fact]
    public async Task GetSemanticTokens_CSharp_Static_WithBackground()
    {
        var documentText = """
                @using System
                @code
                {
                    private static bool
                        _isStatic;

                    public void M()
                    {
                        if (_isStatic)
                        {
                        }
                    }
                }
                """;

        var razorRange = GetRange(documentText);
        var csharpTokens = await GetCSharpSemanticTokensResponseAsync(documentText, razorRange, isRazorFile: true);
        await AssertSemanticTokensAsync(documentText, isRazorFile: true, razorRange, csharpTokens: csharpTokens, withCSharpBackground: true);
        Assert.NotNull(csharpTokens.Tokens);
        Assert.NotEmpty(csharpTokens.Tokens);
    }

    [Fact]
    public async Task GetSemanticTokens_CSharp_Tabs_Static_WithBackground()
    {
        var documentText = """
                @using System
                @code
                {
                	private static bool
                		_isStatic;

                	public void M()
                	{
                		if (_isStatic)
                		{
                		}
                	}
                }
                """;

        var razorRange = GetRange(documentText);
        var csharpTokens = await GetCSharpSemanticTokensResponseAsync(documentText, razorRange, isRazorFile: true);
        await AssertSemanticTokensAsync(documentText, isRazorFile: true, razorRange, csharpTokens: csharpTokens, withCSharpBackground: true);
        Assert.NotNull(csharpTokens.Tokens);
        Assert.NotEmpty(csharpTokens.Tokens);
    }

    [Fact]
    public async Task GetSemanticTokens_CSharp_WithBackground()
    {
        var documentText = """
                @using System
                @code
                {
                    private static bool _isStatic;

                    public void M()
                    {
                        if (_isStatic)
                        {
                        }
                    }
                }
                """;

        var razorRange = GetRange(documentText);
        var csharpTokens = await GetCSharpSemanticTokensResponseAsync(documentText, razorRange, isRazorFile: true);
        await AssertSemanticTokensAsync(documentText, isRazorFile: true, razorRange, csharpTokens: csharpTokens, withCSharpBackground: true);
        Assert.NotNull(csharpTokens.Tokens);
        Assert.NotEmpty(csharpTokens.Tokens);
    }

    [Fact]
    public async Task GetSemanticTokens_CSharp_WitRenderFragment()
    {
        var documentText = """
                <div>This is some HTML</div>
                @code
                {
                    public void M()
                    {
                        RenderFragment x = @<div>This is some HTML</div>;
                    }
                }
                """;

        var razorRange = GetRange(documentText);
        var csharpTokens = await GetCSharpSemanticTokensResponseAsync(documentText, razorRange, isRazorFile: true);
        await AssertSemanticTokensAsync(documentText, isRazorFile: true, razorRange, csharpTokens: csharpTokens);
        Assert.NotNull(csharpTokens.Tokens);
        Assert.NotEmpty(csharpTokens.Tokens);
    }

    [Fact]
    public async Task GetSemanticTokens_CSharp_WitRenderFragmentAndBackground()
    {
        var documentText = """
                <div>This is some HTML</div>
                @code
                {
                    public void M()
                    {
                        RenderFragment x = @<div>This is some HTML</div>;
                    }
                }
                """;

        var razorRange = GetRange(documentText);
        var csharpTokens = await GetCSharpSemanticTokensResponseAsync(documentText, razorRange, isRazorFile: true);
        await AssertSemanticTokensAsync(documentText, isRazorFile: true, razorRange, csharpTokens: csharpTokens, withCSharpBackground: true);
        Assert.NotNull(csharpTokens.Tokens);
        Assert.NotEmpty(csharpTokens.Tokens);
    }

    [Fact]
    public async Task GetSemanticTokens_CSharp_ExplicitStatement_WithBackground()
    {
        var documentText = """
            @DateTime.Now

            @("hello" + "\\n" + "world" + Environment.NewLine + "how are you?")
            """;

        var razorRange = GetRange(documentText);
        var csharpTokens = await GetCSharpSemanticTokensResponseAsync(documentText, razorRange, isRazorFile: true);
        await AssertSemanticTokensAsync(documentText, isRazorFile: true, razorRange, csharpTokens: csharpTokens, withCSharpBackground: true);
        Assert.NotNull(csharpTokens.Tokens);
        Assert.NotEmpty(csharpTokens.Tokens);
    }

    [Fact]
    public void GetMappedCSharpRanges_MinimalRangeVsSmallDisjointRanges_DisjointRangesAreSmaller()
    {
        var documentText =
            """
                @using System
                @functions {
                    Action<object> abc = @<span></span>;
                }
                """;

        var razorRange = GetRange(documentText);
        var codeDocument = CreateCodeDocument(documentText, isRazorFile: true, DefaultTagHelpers);
        var csharpDocumentUri = new Uri("C:\\TestSolution\\TestProject\\TestDocument.cs");
        var csharpSourceText = codeDocument.GetCSharpSourceText();
        var sourceText = codeDocument.GetSourceText();

        if (UsePreciseSemanticTokenRanges)
        {
            var expectedCsharpRangeLengths = new int[] { 12, 27, 3 };
            Assert.True(RazorSemanticTokensInfoService.TryGetSortedCSharpRanges(codeDocument, razorRange, out var csharpRanges));
            Assert.Equal(3, csharpRanges.Length);
            for (var i = 0; i < csharpRanges.Length; i++)
            {
                var csharpRange = csharpRanges[i];
                var textSpan = csharpRange.ToTextSpan(csharpSourceText);
                Assert.Equal(expectedCsharpRangeLengths[i], textSpan.Length);
            }
        }
        else
        {
            var expectedCsharpRangeLength = 970;
            Assert.True(RazorSemanticTokensInfoService.TryGetMinimalCSharpRange(codeDocument, razorRange, out var csharpRange));
            var textSpan = csharpRange.ToTextSpan(csharpSourceText);
            Assert.Equal(expectedCsharpRangeLength, textSpan.Length);
        }
    }

    private async Task AssertSemanticTokensAsync(
        string documentText,
        bool isRazorFile,
        Range range,
        IRazorSemanticTokensInfoService? service = null,
        ProvideSemanticTokensResponse? csharpTokens = null,
        int documentVersion = 0,
        bool withCSharpBackground = false,
        bool serverSupportsPreciseRanges = true)
    {
        await AssertSemanticTokensAsync(new DocumentContentVersion[]
        {
            new DocumentContentVersion(documentText, documentVersion)
        },
        isRazorArray: new bool[] { isRazorFile },
        range,
        service,
        csharpTokens,
        documentVersion,
        withCSharpBackground,
        serverSupportsPreciseRanges);
    }

    private async Task AssertSemanticTokensAsync(
        DocumentContentVersion[] documentTexts,
        bool[] isRazorArray,
        Range range,
        IRazorSemanticTokensInfoService? service,
        ProvideSemanticTokensResponse? csharpTokens,
        int documentVersion,
        bool withCSharpBackground,
        bool serverSupportsPreciseRanges)
    {
        // Arrange
        if (csharpTokens is null)
        {
            csharpTokens = new ProvideSemanticTokensResponse(tokens: null, -1);
        }

        var (documentContexts, textDocumentIdentifiers) = CreateDocumentContext(
            documentTexts, isRazorArray, DefaultTagHelpers, documentVersion: documentVersion);

        if (service is null)
        {
            service = await GetDefaultRazorSemanticTokenInfoServiceAsync(documentContexts, csharpTokens, withCSharpBackground, serverSupportsPreciseRanges);
        }

        var textDocumentIdentifier = textDocumentIdentifiers.Dequeue();
        var documentContext = documentContexts.Peek();
        var correlationId = Guid.Empty;

        // Act
        var tokens = await service.GetSemanticTokensAsync(textDocumentIdentifier, range, documentContext, TestRazorSemanticTokensLegend.Instance, correlationId, DisposalToken);

        // Assert
        var sourceText = await documentContext.GetSourceTextAsync(DisposalToken);
        AssertSemanticTokensMatchesBaseline(sourceText, tokens?.Data);
    }

    private async Task<IRazorSemanticTokensInfoService> GetDefaultRazorSemanticTokenInfoServiceAsync(
        Queue<VersionedDocumentContext> documentSnapshots,
        ProvideSemanticTokensResponse? csharpTokens,
        bool withCSharpBackground,
        bool serverSupportsPreciseRanges)
    {
        _languageServer
            .Setup(l => l.SendRequestAsync<SemanticTokensParams, ProvideSemanticTokensResponse?>(
                CustomMessageNames.RazorProvideSemanticTokensRangeEndpoint,
                It.IsAny<SemanticTokensParams>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(csharpTokens);

        _languageServer
            .Setup(l => l.SendRequestAsync<SemanticTokensParams, ProvideSemanticTokensResponse?>(
                CustomMessageNames.RazorProvidePreciseRangeSemanticTokensEndpoint,
                It.IsAny<SemanticTokensParams>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(serverSupportsPreciseRanges ?
                csharpTokens :
                It.Is<ProvideSemanticTokensResponse>(x => x.Tokens == null));

        var documentContextFactory = new TestDocumentContextFactory(documentSnapshots);
        var documentMappingService = new RazorDocumentMappingService(FilePathService, documentContextFactory, LoggerFactory);

        var configurationSyncService = new Mock<IConfigurationSyncService>(MockBehavior.Strict);

        var options = RazorLSPOptions.Default with { ColorBackground = withCSharpBackground };
        configurationSyncService
            .Setup(c => c.GetLatestOptionsAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult<RazorLSPOptions?>(options));

        var optionsMonitorCache = new OptionsCache<RazorLSPOptions>();

        var optionsMonitor = TestRazorLSPOptionsMonitor.Create(
            configurationSyncService.Object,
            optionsMonitorCache);

        await optionsMonitor.UpdateAsync(CancellationToken.None);

        var featureOptions = Mock.Of<LanguageServerFeatureOptions>(options =>
            options.DelegateToCSharpOnDiagnosticPublish == true &&
            options.UsePreciseSemanticTokenRanges == UsePreciseSemanticTokenRanges &&
            options.CSharpVirtualDocumentSuffix == ".ide.g.cs" &&
            options.HtmlVirtualDocumentSuffix == "__virtual.html",
            MockBehavior.Strict);

        return new RazorSemanticTokensInfoService(
            _languageServer.Object,
            documentMappingService,
            optionsMonitor,
            featureOptions,
            LoggerFactory);
    }

    private void VerifyTimesLanguageServerCalled(bool serverSupportsPreciseRanges)
    {
        _languageServer
            .Verify(l => l.SendRequestAsync<SemanticTokensParams, ProvideSemanticTokensResponse?>(
                CustomMessageNames.RazorProvidePreciseRangeSemanticTokensEndpoint,
                It.IsAny<SemanticTokensParams>(),
                It.IsAny<CancellationToken>()), Times.Exactly(UsePreciseSemanticTokenRanges ? 1 : 0));

        _languageServer
            .Verify(l => l.SendRequestAsync<SemanticTokensParams, ProvideSemanticTokensResponse?>(
                CustomMessageNames.RazorProvideSemanticTokensRangeEndpoint,
                It.IsAny<SemanticTokensParams>(),
                It.IsAny<CancellationToken>()), Times.Exactly(
                    !UsePreciseSemanticTokenRanges || !serverSupportsPreciseRanges ? 1 : 0));
    }

    private static Range GetRange(string text)
    {
        var lineCount = text.Count(c => c == '\n') + 1;

        var range = new Range
        {
            Start = new Position { Line = 0, Character = 0 },
            End = new Position { Line = lineCount, Character = 0 }
        };

        return range;
    }

    private class TestInitializeManager : IInitializeManager<InitializeParams, InitializeResult>
    {
        public InitializeParams GetInitializeParams()
        {
            throw new NotImplementedException();
        }

        public InitializeResult GetInitializeResult()
        {
            throw new NotImplementedException();
        }

        public void SetInitializeParams(InitializeParams request)
        {
            throw new NotImplementedException();
        }
    }

    private class TestDocumentContextFactory : DocumentContextFactory
    {
        private readonly Queue<VersionedDocumentContext> _documentContexts;

        public TestDocumentContextFactory(Queue<VersionedDocumentContext> documentContexts)
        {
            _documentContexts = documentContexts;
        }

        protected override DocumentContext? TryCreateCore(Uri documentUri, VSProjectContext? projectContext, bool versioned)
        {
            var document = _documentContexts.Count == 1 ? _documentContexts.Peek() : _documentContexts.Dequeue();

            return document;
        }
    }
}
