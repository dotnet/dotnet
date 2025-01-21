// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Completion;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.SemanticTokens;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Razor.Workspaces.Protocol.SemanticTokens;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Semantic;

public partial class SemanticTokensTest(ITestOutputHelper testOutput) : TagHelperServiceTestBase(testOutput)
{
    private readonly Mock<IClientConnection> _clientConnection = new(MockBehavior.Strict);
    private static readonly string s_projectPath = TestProject.GetProjectDirectory(typeof(TagHelperServiceTestBase), layer: TestProject.Layer.Tooling);

    private static readonly VSInternalServerCapabilities s_semanticTokensServerCapabilities = new()
    {
        SemanticTokensOptions = new()
        {
            Full = false,
            Range = true
        }
    };

    private static readonly Regex s_matchNewLines = NewLineRegex();

#if NET
    [GeneratedRegex("\r\n")]
    private static partial Regex NewLineRegex();
#else
    private static Regex NewLineRegex() => new Regex("\r\n|\r|\n");
#endif

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_CSharp_RazorIfNotReady(bool precise)
    {
        var documentText = """
            <p></p>@{
                var d = "t";
            }
            """;

        var csharpTokens = new ProvideSemanticTokensResponse(tokens: [], hostDocumentSyncVersion: 1);
        await AssertSemanticTokensAsync(documentText, precise, csharpTokens: csharpTokens, documentVersion: 1);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_CSharpBlock_HTML(bool precise)
    {
        var documentText = """
            @{
                var d = "t";
                <p>HTML @d</p>
            }
            """;

        await VerifySemanticTokensAsync(documentText, precise);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_CSharp_Nested_HTML(bool precise)
    {
        var documentText = """
            @addTagHelper *, TestAssembly
            <!--@{var d = "string";@<a></a>}-->
            """;

        await VerifySemanticTokensAsync(documentText, precise);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_CSharp_VSCodeWorks(bool precise)
    {
        var documentText = """
            @addTagHelper *, TestAssembly
            @{ var d = }
            """;

        var csharpTokens = new ProvideSemanticTokensResponse(tokens: [], hostDocumentSyncVersion: 1);
        await AssertSemanticTokensAsync(documentText, precise, csharpTokens: csharpTokens, documentVersion: 1);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_CSharp_Explicit(bool precise)
    {
        var documentText = """
            @using System
            @addTagHelper *, TestAssembly
            @(DateTime.Now)
            """;

        await VerifySemanticTokensAsync(documentText, precise);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_CSharp_Implicit(bool serverSupportsPreciseRanges, bool precise)
    {
        var documentText = """
            @addTagHelper *, TestAssembly
            @{ var d = "txt";}
            @d
            """;

        var csharpTokens = await GetCSharpSemanticTokensResponseAsync(documentText, precise);
        await AssertSemanticTokensAsync(documentText, precise, csharpTokens: csharpTokens, serverSupportsPreciseRanges: serverSupportsPreciseRanges);
        VerifyTimesLanguageServerCalled(serverSupportsPreciseRanges, precise);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_CSharp_VersionMismatch(bool serverSupportsPreciseRanges, bool precise)
    {
        var documentText = """
            @addTagHelper *, TestAssembly
            @{ var d = }
            """;

        var csharpTokens = await GetCSharpSemanticTokensResponseAsync(documentText, precise);
        await AssertSemanticTokensAsync(documentText, precise, csharpTokens: csharpTokens, documentVersion: 21, serverSupportsPreciseRanges: serverSupportsPreciseRanges);
        VerifyTimesLanguageServerCalled(serverSupportsPreciseRanges, precise);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_CSharp_FunctionAsync(bool precise)
    {
        var documentText = """
            @addTagHelper *, TestAssembly
            @{ var d = }
            """;

        await VerifySemanticTokensAsync(documentText, precise);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_CSharp_StaticModifier(bool precise)
    {
        var documentText = """
            @code
            {
                static int x = 1;
            }
            """;

        await VerifySemanticTokensAsync(documentText, precise, isRazorFile: true);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_MultipleBlankLines(bool precise)
    {
        var documentText = """
            @addTagHelper *, TestAssembly

            <p>first
            second</p>
            """;

        await VerifySemanticTokensAsync(documentText, precise);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_IncompleteTag(bool precise)
    {
        var documentText = """
            <str class='
            """;

        await VerifySemanticTokensAsync(documentText, precise);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_MinimizedHTMLAttribute(bool precise)
    {
        var documentText = """
            <p attr />
            """;

        await VerifySemanticTokensAsync(documentText, precise);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_MinimizedHTMLAsync(bool precise)
    {
        var documentText = """
            @addTagHelper *, TestAssembly
            <input/>
            """;

        await VerifySemanticTokensAsync(documentText, precise);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_HTMLCommentAsync(bool precise)
    {
        var documentText = """
            @addTagHelper *, TestAssembly
            <!-- comment with comma's -->
            """;

        await VerifySemanticTokensAsync(documentText, precise);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_PartialHTMLCommentAsync(bool precise)
    {
        var documentText = """
            @addTagHelper *, TestAssembly
            <!-- comment
            """;

        await VerifySemanticTokensAsync(documentText, precise, isRazorFile: true);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_HTMLIncludesBang(bool precise)
    {
        var documentText = """
            @addTagHelper *, TestAssembly
            <!input/>
            """;

        await VerifySemanticTokensAsync(documentText, precise);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_HalfOfCommentAsync(bool precise)
    {
        var documentText = """
            @addTagHelper *, TestAssembly
            @* comment
            """;

        await VerifySemanticTokensAsync(documentText, precise, isRazorFile: true);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_NoAttributesAsync(bool precise)
    {
        var documentText = """
            @addTagHelper *, TestAssembly
            <test1></test1>
            """;

        await VerifySemanticTokensAsync(documentText, precise);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_WithAttributeAsync(bool precise)
    {
        var documentText = """
            @addTagHelper *, TestAssembly
            <test1 bool-val='true'></test1>
            """;

        await VerifySemanticTokensAsync(documentText, precise);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_MinimizedAttribute_BoundAsync(bool precise)
    {
        var documentText = """
            @addTagHelper *, TestAssembly
            <test1 bool-val></test1>
            """;

        await VerifySemanticTokensAsync(documentText, precise);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_MinimizedAttribute_NotBoundAsync(bool precise)
    {
        var documentText = """
            @addTagHelper *, TestAssembly
            <test1 notbound></test1>
            """;

        await VerifySemanticTokensAsync(documentText, precise);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_IgnoresNonTagHelperAttributesAsync(bool precise)
    {
        var documentText = """
            @addTagHelper *, TestAssembly
            <test1 bool-val='true' class='display:none'></test1>
            """;

        await VerifySemanticTokensAsync(documentText, precise);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_TagHelpersNotAvailableInRazorAsync(bool precise)
    {
        var documentText = """
            @addTagHelper *, TestAssembly
            <test1 bool-val='true' class='display:none'></test1>
            """;

        await VerifySemanticTokensAsync(documentText, precise, isRazorFile: true);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_DoesNotApplyOnNonTagHelpersAsync(bool precise)
    {
        var documentText = """
            @addTagHelper *, TestAssembly
            <p bool-val='true'></p>
            """;

        await VerifySemanticTokensAsync(documentText, precise);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_Razor_MinimizedDirectiveAttributeParameters(bool precise)
    {
        // Capitalized, non-well-known-HTML elements should not be marked as TagHelpers
        var documentText = """
            @addTagHelper *, TestAssembly
            }<NotATagHelp @minimized:something />
            """;

        await VerifySemanticTokensAsync(documentText, precise, isRazorFile: true);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_Razor_ComponentAttributeAsync(bool precise)
    {
        var documentText = """
            @addTagHelper *, TestAssembly
            <Component1 bool-val=""true""></Component1>
            """;

        await VerifySemanticTokensAsync(documentText, precise, isRazorFile: true);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_Razor_ComponentAttribute_DoesntGetABackground(bool precise)
    {
        // Need C# around the component for the C# range to be valid, to correctly validate the attribute handling
        var documentText = """
            @DateTime.Now

            <Component1 Title=""Hi there I'm a string""></Component1>

            @DateTime.Now
            """;

        await VerifySemanticTokensAsync(documentText, precise, isRazorFile: true, withCSharpBackground: true);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_Razor_DirectiveAttributesParametersAsync(bool precise)
    {
        var documentText = """
            @addTagHelper *, TestAssembly
            <Component1 @test:something='Function'></Component1>
            """;

        await VerifySemanticTokensAsync(documentText, precise, isRazorFile: true);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_Razor_NonComponentsDoNotShowInRazorAsync(bool precise)
    {
        var documentText = """
            @addTagHelper *, TestAssembly
            <test1 bool-val='true'></test1>
            """;

        await VerifySemanticTokensAsync(documentText, precise, isRazorFile: true);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_Razor_DirectivesAsync(bool precise)
    {
        var documentText = """
            @addTagHelper *, TestAssembly
            <Component1 @test='Function'></Component1>
            """;

        await VerifySemanticTokensAsync(documentText, precise, isRazorFile: true);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_Razor_DirectiveWithExplicitStatementAsync(bool precise)
    {
        var documentText = """
            @addTagHelper *, TestAssembly
            <Component1 @onclick="@(Function())"></Component1>
            """;

        await VerifySemanticTokensAsync(documentText, precise, isRazorFile: true);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_HandleTransitionEscape(bool precise)
    {
        var documentText = """
            @@text
            """;

        await VerifySemanticTokensAsync(documentText, precise, isRazorFile: true);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_Razor_DoNotColorNonTagHelpersAsync(bool precise)
    {
        var documentText = """
            <p @test='Function'></p>
            """;

        await VerifySemanticTokensAsync(documentText, precise, isRazorFile: true);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_Razor_DoesNotApplyOnNonTagHelpersAsync(bool precise)
    {
        var documentText = """
            @addTagHelpers *, TestAssembly
            <p></p>
            """;

        await VerifySemanticTokensAsync(documentText, precise, isRazorFile: true);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_Razor_CodeDirectiveAsync(bool precise)
    {
        var documentText = """
            @code {}
            """;

        await VerifySemanticTokensAsync(documentText, precise, isRazorFile: true);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_Razor_CodeDirectiveBodyAsync(bool precise)
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

        await VerifySemanticTokensAsync(documentText, precise, isRazorFile: true);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_Razor_UsingDirective(bool precise)
    {
        var documentText = """
            @using System.Threading
            """;

        await VerifySemanticTokensAsync(documentText, precise, isRazorFile: true);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_Razor_FunctionsDirectiveAsync(bool precise)
    {
        var documentText = """
            @functions {}
            """;

        await VerifySemanticTokensAsync(documentText, precise, isRazorFile: true);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_Razor_NestedTextDirectives(bool precise)
    {
        var documentText = """
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

        await VerifySemanticTokensAsync(documentText, precise);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_Razor_NestedTransitions(bool precise)
    {
        var documentText = """
            @using System
            @functions {
                Action<object> abc = @<span></span>;
            }
            """;

        await VerifySemanticTokensAsync(documentText, precise, isRazorFile: true);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_Razor_CommentAsync(bool precise)
    {
        var documentText = """
            @* A comment *@
            """;

        await VerifySemanticTokensAsync(documentText, precise, isRazorFile: true);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_Razor_MultiLineCommentMidlineAsync(bool precise)
    {
        var documentText = """
            <a />@* kdl
            skd
            slf*@
            """;

        await VerifySemanticTokensAsync(documentText, precise);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_Razor_MultiLineCommentWithBlankLines(bool precise)
    {
        var documentText = """
            @* kdl

            skd
                
                    sdfasdfasdf
            slf*@
            """;

        await VerifySemanticTokensAsync(documentText, precise);
    }

    [Theory]
    [CombinatorialData]
    [WorkItem("https://github.com/dotnet/razor/issues/8176")]
    public async Task GetSemanticTokens_Razor_MultiLineCommentWithBlankLines_LF(bool precise)
    {
        var documentText = "@* kdl\n\nskd\n    \n        sdfasdfasdf\nslf*@";

        await VerifySemanticTokensAsync(documentText, precise);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_Razor_MultiLineCommentAsync(bool precise)
    {
        var documentText = """
            @*stuff
            things *@
            """;

        await VerifySemanticTokensAsync(documentText, precise);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_CSharp_Static(bool precise)
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

        await VerifySemanticTokensAsync(documentText, precise, isRazorFile: true);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_Legacy_Model(bool precise)
    {
        var documentText = """
            @using System
            @model SampleApp.Pages.ErrorModel

            <div>

                @{
                    @Model.ToString();
                }

            </div>
            """;

        await VerifySemanticTokensAsync(documentText, precise, isRazorFile: false);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_CSharp_LargeFile(bool precise)
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

        await VerifySemanticTokensAsync(documentText, precise, isRazorFile: true);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_CSharp_Static_WithBackground(bool precise)
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

        await VerifySemanticTokensAsync(documentText, precise, isRazorFile: true, withCSharpBackground: true);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_CSharp_Tabs_Static_WithBackground(bool precise)
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

        await VerifySemanticTokensAsync(documentText, precise, isRazorFile: true, withCSharpBackground: true);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_CSharp_WithBackground(bool precise)
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

        await VerifySemanticTokensAsync(documentText, precise, isRazorFile: true, withCSharpBackground: true);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_CSharp_WitRenderFragment(bool precise)
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

        await VerifySemanticTokensAsync(documentText, precise, isRazorFile: true);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_CSharp_WitRenderFragmentAndBackground(bool precise)
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

        await VerifySemanticTokensAsync(documentText, precise, isRazorFile: true, withCSharpBackground: true);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_CSharp_ExplicitStatement_WithBackground(bool precise)
    {
        var documentText = """
            @DateTime.Now

            @("hello" + "\\n" + "world" + Environment.NewLine + "how are you?")
            """;

        await VerifySemanticTokensAsync(documentText, precise, isRazorFile: true, withCSharpBackground: true);
    }

    [Theory]
    [CombinatorialData]
    public void GetMappedCSharpRanges_MinimalRangeVsSmallDisjointRanges_DisjointRangesAreSmaller(bool precise)
    {
        var documentText = """
            @[|using System|]
            @functions {[|
                Action<object> abc = |]@<span></span>[|;
            |]}
            """;

        TestFileMarkupParser.GetSpans(documentText, out documentText,
            out ImmutableArray<TextSpan> spans);

        var codeDocument = CreateCodeDocument(documentText, isRazorFile: true, DefaultTagHelpers);
        var csharpSourceText = codeDocument.GetCSharpSourceText();
        var razorRange = GetSpan(documentText);

        if (precise)
        {
            Assert.True(RazorSemanticTokensInfoService.TryGetSortedCSharpRanges(codeDocument, razorRange, out var csharpRanges));
            Assert.Equal(spans.Length, csharpRanges.Length);
            for (var i = 0; i < csharpRanges.Length; i++)
            {
                var csharpRange = csharpRanges[i];
                var textSpan = csharpSourceText.GetTextSpan(csharpRange);
                Assert.Equal(spans[i].Length, textSpan.Length);
            }
        }
        else
        {
            // Note that the expected lengths are different on Windows vs. Unix.
            var expectedCsharpRangeLength = PlatformInformation.IsWindows ? 945 : 911;
            Assert.True(codeDocument.TryGetMinimalCSharpRange(razorRange, out var csharpRange));
            var textSpan = csharpSourceText.GetTextSpan(csharpRange);
            Assert.Equal(expectedCsharpRangeLength, textSpan.Length);
        }
    }

    private async Task VerifySemanticTokensAsync(string documentText, bool precise, bool isRazorFile = false, bool withCSharpBackground = false, [CallerMemberName] string? testName = null)
    {
        var csharpTokens = await GetCSharpSemanticTokensResponseAsync(documentText, precise, isRazorFile);
        await AssertSemanticTokensAsync(documentText, precise, csharpTokens, isRazorFile, withCSharpBackground: withCSharpBackground, testName: testName);
    }

    private async Task AssertSemanticTokensAsync(
        string documentText,
        bool precise,
        ProvideSemanticTokensResponse csharpTokens,
        bool isRazorFile = false,
        int documentVersion = 0,
        bool withCSharpBackground = false,
        bool serverSupportsPreciseRanges = true,
        [CallerMemberName] string? testName = null)
    {
        var documentContext = CreateDocumentContext(documentText, isRazorFile, DefaultTagHelpers, documentVersion);

        var service = await CreateServiceAsync(documentContext, csharpTokens, withCSharpBackground, serverSupportsPreciseRanges, precise);

        var range = GetSpan(documentText);
        var tokens = await service.GetSemanticTokensAsync(documentContext, range, withCSharpBackground, Guid.Empty, DisposalToken);

        var sourceText = await documentContext.GetSourceTextAsync(DisposalToken);
        AssertSemanticTokensMatchesBaseline(sourceText, tokens, testName.AssumeNotNull());
    }

    private static DocumentContext CreateDocumentContext(
        string documentText,
        bool isRazorFile,
        ImmutableArray<TagHelperDescriptor> tagHelpers,
        int version)
    {
        var document = CreateCodeDocument(documentText, isRazorFile, tagHelpers);

        var projectSnapshot = StrictMock.Of<IProjectSnapshot>();

        var documentSnapshotMock = new StrictMock<IDocumentSnapshot>();
        documentSnapshotMock
            .SetupGet(x => x.Project)
            .Returns(projectSnapshot);
        documentSnapshotMock
            .Setup(x => x.GetGeneratedOutputAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(document);
        documentSnapshotMock
            .Setup(x => x.GetTextAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(document.Source.Text);
        documentSnapshotMock
            .SetupGet(x => x.Version)
            .Returns(version);

        return new DocumentContext(
            uri: new Uri($@"c:\${GetFileName(isRazorFile)}"),
            snapshot: documentSnapshotMock.Object,
            projectContext: null);
    }

    private async Task<IRazorSemanticTokensInfoService> CreateServiceAsync(
        DocumentContext documentSnapshot,
        ProvideSemanticTokensResponse? csharpTokens,
        bool withCSharpBackground,
        bool serverSupportsPreciseRanges,
        bool precise)
    {
        _clientConnection
            .Setup(l => l.SendRequestAsync<SemanticTokensParams, ProvideSemanticTokensResponse?>(
                CustomMessageNames.RazorProvideSemanticTokensRangeEndpoint,
                It.IsAny<SemanticTokensParams>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(csharpTokens);

        _clientConnection
            .Setup(l => l.SendRequestAsync<SemanticTokensParams, ProvideSemanticTokensResponse?>(
                CustomMessageNames.RazorProvidePreciseRangeSemanticTokensEndpoint,
                It.IsAny<SemanticTokensParams>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(serverSupportsPreciseRanges
                ? csharpTokens
                : It.Is<ProvideSemanticTokensResponse>(x => x.Tokens == null));

        var documentContextFactory = new TestDocumentContextFactory(documentSnapshot);
        var documentMappingService = new LspDocumentMappingService(FilePathService, documentContextFactory, LoggerFactory);

        var configurationSyncService = new Mock<IConfigurationSyncService>(MockBehavior.Strict);

        var options = RazorLSPOptions.Default with { ColorBackground = withCSharpBackground };
        configurationSyncService
            .Setup(c => c.GetLatestOptionsAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult<RazorLSPOptions?>(options));

        var optionsMonitor = TestRazorLSPOptionsMonitor.Create(
            configurationSyncService.Object);

        await optionsMonitor.UpdateAsync(DisposalToken);

        var featureOptions = Mock.Of<LanguageServerFeatureOptions>(options =>
            options.DelegateToCSharpOnDiagnosticPublish == true &&
            options.UsePreciseSemanticTokenRanges == precise &&
            options.CSharpVirtualDocumentSuffix == ".ide.g.cs" &&
            options.HtmlVirtualDocumentSuffix == "__virtual.html",
            MockBehavior.Strict);

        var csharpSemanticTokensProvider = new LSPCSharpSemanticTokensProvider(featureOptions, _clientConnection.Object, LoggerFactory);

        var service = new RazorSemanticTokensInfoService(
            documentMappingService,
            TestRazorSemanticTokensLegendService.Instance,
            csharpSemanticTokensProvider,
            featureOptions,
            LoggerFactory);

        return service;
    }

    private async Task<ProvideSemanticTokensResponse> GetCSharpSemanticTokensResponseAsync(string documentText, bool precise, bool isRazorFile = false)
    {
        var codeDocument = CreateCodeDocument(documentText, isRazorFile, DefaultTagHelpers);
        var csharpDocumentUri = new Uri("C:\\TestSolution\\TestProject\\TestDocument.cs");
        var csharpSourceText = codeDocument.GetCSharpSourceText();

        await using var csharpServer = await CSharpTestLspServerHelpers.CreateCSharpLspServerAsync(
            csharpSourceText,
            csharpDocumentUri,
            s_semanticTokensServerCapabilities,
            SpanMappingService,
            capabilitiesUpdater: null,
            DisposalToken);

        var razorRange = GetSpan(documentText);
        var csharpRanges = GetMappedCSharpRanges(codeDocument, razorRange, precise);
        if (csharpRanges == null)
        {
            return new ProvideSemanticTokensResponse(tokens: [], hostDocumentSyncVersion: 0);
        }

        if (precise)
        {
            var result = await csharpServer.ExecuteRequestAsync<SemanticTokensRangesParams, SemanticTokens>(
                "roslyn/semanticTokenRanges",
                CreateVSSemanticTokensRangesParams(csharpRanges.Value, csharpDocumentUri),
                DisposalToken);

            return new ProvideSemanticTokensResponse(tokens: result?.Data, hostDocumentSyncVersion: 0);
        }
        else
        {
            var range = Assert.Single(csharpRanges.Value);
            var result = await csharpServer.ExecuteRequestAsync<SemanticTokensRangeParams, SemanticTokens>(
                "textDocument/semanticTokens/range",
                CreateVSSemanticTokensRangeParams(range, csharpDocumentUri),
                DisposalToken);

            return new ProvideSemanticTokensResponse(tokens: result?.Data, hostDocumentSyncVersion: 0);
        }
    }

    private void VerifyTimesLanguageServerCalled(bool serverSupportsPreciseRanges, bool precise)
    {
        _clientConnection
            .Verify(l => l.SendRequestAsync<SemanticTokensParams, ProvideSemanticTokensResponse?>(
                CustomMessageNames.RazorProvidePreciseRangeSemanticTokensEndpoint,
                It.IsAny<SemanticTokensParams>(),
                It.IsAny<CancellationToken>()), Times.Exactly(precise ? 1 : 0));

        _clientConnection
            .Verify(l => l.SendRequestAsync<SemanticTokensParams, ProvideSemanticTokensResponse?>(
                CustomMessageNames.RazorProvideSemanticTokensRangeEndpoint,
                It.IsAny<SemanticTokensParams>(),
                It.IsAny<CancellationToken>()), Times.Exactly(
                    !precise || !serverSupportsPreciseRanges ? 1 : 0));
    }

    private static LinePositionSpan GetSpan(string text)
    {
        var lineCount = text.Count(c => c == '\n') + 1;

        return new LinePositionSpan(new LinePosition(0, 0), new LinePosition(lineCount, 0));
    }

    private void AssertSemanticTokensMatchesBaseline(SourceText sourceText, int[]? actualSemanticTokens, string testName)
    {
        var fileName = $"Semantic\\TestFiles\\{testName}";

        var baselineFileName = Path.ChangeExtension(fileName, ".semantic.txt");

        var actualFileContents = GetFileRepresentationOfTokens(sourceText, actualSemanticTokens);

        if (GenerateBaselines.ShouldGenerate)
        {
            GenerateSemanticBaseline(actualFileContents, baselineFileName);
        }

        var expectedFileContents = GetBaselineFileContents(baselineFileName);

        Roslyn.Test.Utilities.AssertEx.EqualOrDiff(expectedFileContents, actualFileContents);
    }

    private string GetBaselineFileContents(string baselineFileName)
    {
        var semanticFile = TestFile.Create(baselineFileName, GetType().GetTypeInfo().Assembly);
        if (!semanticFile.Exists())
        {
            return string.Empty;
        }

        var baselineContents = semanticFile.ReadAllText();

        if (!PlatformInformation.IsWindows)
        {
            baselineContents = s_matchNewLines.Replace(baselineContents, "\n");
        }

        return baselineContents;
    }

    private ImmutableArray<LinePositionSpan>? GetMappedCSharpRanges(RazorCodeDocument codeDocument, LinePositionSpan razorRange, bool precise)
    {
        var documentMappingService = new LspDocumentMappingService(FilePathService, new TestDocumentContextFactory(), LoggerFactory);

        if (precise)
        {
            if (!RazorSemanticTokensInfoService.TryGetSortedCSharpRanges(codeDocument, razorRange, out var csharpRanges))
            {
                // No C# in the range.
                return null;
            }

            return csharpRanges;
        }

        if (!documentMappingService.TryMapToGeneratedDocumentRange(codeDocument.GetCSharpDocument(), razorRange, out var range) &&
            !codeDocument.TryGetMinimalCSharpRange(razorRange, out range))
        {
            // No C# in the range.
            return null;
        }

        return [range];
    }

    private static SemanticTokensRangesParams CreateVSSemanticTokensRangesParams(ImmutableArray<LinePositionSpan> ranges, Uri uri)
        => new()
        {
            TextDocument = new TextDocumentIdentifier { Uri = uri },
            Ranges = ranges.Select(s => s.ToRange()).ToArray()
        };

    private static SemanticTokensRangeParams CreateVSSemanticTokensRangeParams(LinePositionSpan range, Uri uri)
        => new()
        {
            TextDocument = new TextDocumentIdentifier { Uri = uri },
            Range = range.ToRange()
        };

    private static void GenerateSemanticBaseline(string actualFileContents, string baselineFileName)
    {
        var semanticBaselinePath = Path.Combine(s_projectPath, baselineFileName);
        File.WriteAllText(semanticBaselinePath, actualFileContents);
    }

    private static string GetFileRepresentationOfTokens(SourceText sourceText, int[]? data)
    {
        if (data == null)
        {
            return string.Empty;
        }

        using var _ = StringBuilderPool.GetPooledObject(out var builder);
        builder.AppendLine("//line,characterPos,length,tokenType,modifier,text");
        var legendArray = TestRazorSemanticTokensLegendService.Instance.TokenTypes.All;
        var prevLength = 0;
        var lineIndex = 0;
        var lineOffset = 0;
        for (var i = 0; i < data.Length; i += 5)
        {
            var lineDelta = data[i];
            var charDelta = data[i + 1];
            var length = data[i + 2];

            Assert.False(i != 0 && lineDelta == 0 && charDelta == 0, "line delta and character delta are both 0, which is invalid as we shouldn't be producing overlapping tokens");
            Assert.False(i != 0 && lineDelta == 0 && charDelta < prevLength, "Previous length is longer than char offset from previous start, meaning tokens will overlap");

            if (lineDelta != 0)
            {
                lineOffset = 0;
            }

            lineIndex += lineDelta;
            lineOffset += charDelta;

            var typeString = legendArray[data[i + 3]];
            builder.Append(lineDelta).Append(' ');
            builder.Append(charDelta).Append(' ');
            builder.Append(length).Append(' ');
            builder.Append(typeString).Append(' ');
            builder.Append(data[i + 4]).Append(' ');
            builder.Append('[').Append(sourceText.GetSubTextString(new TextSpan(sourceText.Lines[lineIndex].Start + lineOffset, length))).Append(']');
            builder.AppendLine();

            prevLength = length;
        }

        return builder.ToString();
    }

    private class TestDocumentContextFactory(DocumentContext? documentContext = null) : IDocumentContextFactory
    {
        public bool TryCreate(
            Uri documentUri,
            VSProjectContext? projectContext,
            [NotNullWhen(true)] out DocumentContext? context)
        {
            context = documentContext;
            return context is not null;
        }
    }
}
