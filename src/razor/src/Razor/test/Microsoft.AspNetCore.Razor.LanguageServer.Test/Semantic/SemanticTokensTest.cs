// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

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
using Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.SemanticTokens;
using Microsoft.CodeAnalysis.Razor.Workspaces.Protocol.SemanticTokens;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Text;
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
        SemanticTokensOptions = new SemanticTokensOptions()
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
    public async Task GetSemanticTokens_CSharp_RazorIfNotReady(bool supportsVSExtensions)
    {
        var documentText = """
            <p></p>@{
                var d = "t";
            }
            """;

        var csharpTokens = new ProvideSemanticTokensResponse(tokens: [], hostDocumentSyncVersion: 1);
        await AssertSemanticTokensAsync(documentText, supportsVSExtensions: supportsVSExtensions, csharpTokens: csharpTokens, documentVersion: 1);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_CSharpBlock_HTML(bool supportsVSExtensions)
    {
        var documentText = """
            @{
                var d = "t";
                <p>HTML @d</p>
            }
            """;

        await VerifySemanticTokensAsync(documentText, supportsVSExtensions: supportsVSExtensions);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_CSharp_Nested_HTML(bool supportsVSExtensions)
    {
        var documentText = """
            @addTagHelper *, TestAssembly
            <!--@{var d = "string";@<a></a>}-->
            """;

        await VerifySemanticTokensAsync(documentText, supportsVSExtensions: supportsVSExtensions);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_CSharp_VSCodeWorks(bool supportsVSExtensions)
    {
        var documentText = """
            @addTagHelper *, TestAssembly
            @{ var d = }
            """;

        var csharpTokens = new ProvideSemanticTokensResponse(tokens: [], hostDocumentSyncVersion: 1);
        await AssertSemanticTokensAsync(documentText, supportsVSExtensions: supportsVSExtensions, csharpTokens: csharpTokens, documentVersion: 1);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_CSharp_Explicit(bool supportsVSExtensions)
    {
        var documentText = """
            @using System
            @addTagHelper *, TestAssembly
            @(DateTime.Now)
            """;

        await VerifySemanticTokensAsync(documentText, supportsVSExtensions: supportsVSExtensions);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_CSharp_Attribute(bool supportsVSExtensions)
    {
        var documentText = """
            @using System.Diagnostics
            @code {
                [DebuggerDisplay($"{GetDebuggerDisplay,nq}")]
                public class MyClass
                {
                }
            }
            """;

        await VerifySemanticTokensAsync(documentText, isRazorFile: true, supportsVSExtensions: supportsVSExtensions);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_CSharp_Implicit(bool supportsVSExtensions)
    {
        var documentText = """
            @addTagHelper *, TestAssembly
            @{ var d = "txt";}
            @d
            """;

        var csharpTokens = await GetCSharpSemanticTokensResponseAsync(documentText, supportsVSExtensions: supportsVSExtensions);
        await AssertSemanticTokensAsync(documentText, supportsVSExtensions: supportsVSExtensions, csharpTokens: csharpTokens);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_CSharp_VersionMismatch(bool supportsVSExtensions)
    {
        var documentText = """
            @addTagHelper *, TestAssembly
            @{ var d = }
            """;

        var csharpTokens = await GetCSharpSemanticTokensResponseAsync(documentText, supportsVSExtensions: supportsVSExtensions);
        await AssertSemanticTokensAsync(documentText, csharpTokens: csharpTokens, documentVersion: 21);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_CSharp_FunctionAsync(bool supportsVSExtensions)
    {
        var documentText = """
            @addTagHelper *, TestAssembly
            @{ var d = }
            """;

        await VerifySemanticTokensAsync(documentText, supportsVSExtensions: supportsVSExtensions);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_CSharp_StaticModifier(bool supportsVSExtensions)
    {
        var documentText = """
            @code
            {
                static int x = 1;
            }
            """;

        await VerifySemanticTokensAsync(documentText, supportsVSExtensions: supportsVSExtensions, isRazorFile: true);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_MultipleBlankLines(bool supportsVSExtensions)
    {
        var documentText = """
            @addTagHelper *, TestAssembly

            <p>first
            second</p>
            """;

        await VerifySemanticTokensAsync(documentText, supportsVSExtensions: supportsVSExtensions);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_IncompleteTag(bool supportsVSExtensions)
    {
        var documentText = """
            <str class='
            """;

        await VerifySemanticTokensAsync(documentText, supportsVSExtensions: supportsVSExtensions);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_MinimizedHTMLAttribute(bool supportsVSExtensions)
    {
        var documentText = """
            <p attr />
            """;

        await VerifySemanticTokensAsync(documentText, supportsVSExtensions: supportsVSExtensions);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_MinimizedHTMLAsync(bool supportsVSExtensions)
    {
        var documentText = """
            @addTagHelper *, TestAssembly
            <input/>
            """;

        await VerifySemanticTokensAsync(documentText, supportsVSExtensions: supportsVSExtensions);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_HTMLCommentAsync(bool supportsVSExtensions)
    {
        var documentText = """
            @addTagHelper *, TestAssembly
            <!-- comment with comma's -->
            """;

        await VerifySemanticTokensAsync(documentText, supportsVSExtensions: supportsVSExtensions);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_PartialHTMLCommentAsync(bool supportsVSExtensions)
    {
        var documentText = """
            @addTagHelper *, TestAssembly
            <!-- comment
            """;

        await VerifySemanticTokensAsync(documentText, supportsVSExtensions: supportsVSExtensions, isRazorFile: true);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_HTMLIncludesBang(bool supportsVSExtensions)
    {
        var documentText = """
            @addTagHelper *, TestAssembly
            <!input/>
            """;

        await VerifySemanticTokensAsync(documentText, supportsVSExtensions: supportsVSExtensions);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_HalfOfCommentAsync(bool supportsVSExtensions)
    {
        var documentText = """
            @addTagHelper *, TestAssembly
            @* comment
            """;

        await VerifySemanticTokensAsync(documentText, supportsVSExtensions: supportsVSExtensions, isRazorFile: true);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_NoAttributesAsync(bool supportsVSExtensions)
    {
        var documentText = """
            @addTagHelper *, TestAssembly
            <test1></test1>
            """;

        await VerifySemanticTokensAsync(documentText, supportsVSExtensions: supportsVSExtensions);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_WithAttributeAsync(bool supportsVSExtensions)
    {
        var documentText = """
            @addTagHelper *, TestAssembly
            <test1 bool-val='true'></test1>
            """;

        await VerifySemanticTokensAsync(documentText, supportsVSExtensions: supportsVSExtensions);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_MinimizedAttribute_BoundAsync(bool supportsVSExtensions)
    {
        var documentText = """
            @addTagHelper *, TestAssembly
            <test1 bool-val></test1>
            """;

        await VerifySemanticTokensAsync(documentText, supportsVSExtensions: supportsVSExtensions);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_MinimizedAttribute_NotBoundAsync(bool supportsVSExtensions)
    {
        var documentText = """
            @addTagHelper *, TestAssembly
            <test1 notbound></test1>
            """;

        await VerifySemanticTokensAsync(documentText, supportsVSExtensions: supportsVSExtensions);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_IgnoresNonTagHelperAttributesAsync(bool supportsVSExtensions)
    {
        var documentText = """
            @addTagHelper *, TestAssembly
            <test1 bool-val='true' class='display:none'></test1>
            """;

        await VerifySemanticTokensAsync(documentText, supportsVSExtensions: supportsVSExtensions);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_TagHelpersNotAvailableInRazorAsync(bool supportsVSExtensions)
    {
        var documentText = """
            @addTagHelper *, TestAssembly
            <test1 bool-val='true' class='display:none'></test1>
            """;

        await VerifySemanticTokensAsync(documentText, supportsVSExtensions: supportsVSExtensions, isRazorFile: true);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_DoesNotApplyOnNonTagHelpersAsync(bool supportsVSExtensions)
    {
        var documentText = """
            @addTagHelper *, TestAssembly
            <p bool-val='true'></p>
            """;

        await VerifySemanticTokensAsync(documentText, supportsVSExtensions: supportsVSExtensions);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_Razor_MinimizedDirectiveAttributeParameters(bool supportsVSExtensions)
    {
        // Capitalized, non-well-known-HTML elements should not be marked as TagHelpers
        var documentText = """
            @addTagHelper *, TestAssembly
            }<NotATagHelp @minimized:something />
            """;

        await VerifySemanticTokensAsync(documentText, supportsVSExtensions: supportsVSExtensions, isRazorFile: true);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_Razor_ComponentAttribute_DoesntGetABackground(bool supportsVSExtensions)
    {
        // Need C# around the component for the C# range to be valid, to correctly validate the attribute handling
        var documentText = """
            @DateTime.Now

            <Component1 Title=""Hi there I'm a string""></Component1>

            @DateTime.Now
            """;

        await VerifySemanticTokensAsync(documentText, supportsVSExtensions: supportsVSExtensions, isRazorFile: true, withCSharpBackground: true);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_Razor_DirectiveAttributesParametersAsync(bool supportsVSExtensions)
    {
        var documentText = """
            @addTagHelper *, TestAssembly
            <Component1 @test:something='Function'></Component1>
            """;

        await VerifySemanticTokensAsync(documentText, supportsVSExtensions: supportsVSExtensions, isRazorFile: true);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_Razor_NonComponentsDoNotShowInRazorAsync(bool supportsVSExtensions)
    {
        var documentText = """
            @addTagHelper *, TestAssembly
            <test1 bool-val='true'></test1>
            """;

        await VerifySemanticTokensAsync(documentText, supportsVSExtensions: supportsVSExtensions, isRazorFile: true);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_Razor_DirectivesAsync(bool supportsVSExtensions)
    {
        var documentText = """
            @addTagHelper *, TestAssembly
            <Component1 @test='Function'></Component1>
            """;

        await VerifySemanticTokensAsync(documentText, supportsVSExtensions: supportsVSExtensions, isRazorFile: true);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_Razor_DirectiveWithExplicitStatementAsync(bool supportsVSExtensions)
    {
        var documentText = """
            @addTagHelper *, TestAssembly
            <Component1 @onclick="@(Function())"></Component1>
            """;

        await VerifySemanticTokensAsync(documentText, supportsVSExtensions: supportsVSExtensions, isRazorFile: true);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_HandleTransitionEscape(bool supportsVSExtensions)
    {
        var documentText = """
            @@text
            """;

        await VerifySemanticTokensAsync(documentText, supportsVSExtensions: supportsVSExtensions, isRazorFile: true);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_Razor_DoNotColorNonTagHelpersAsync(bool supportsVSExtensions)
    {
        var documentText = """
            <p @test='Function'></p>
            """;

        await VerifySemanticTokensAsync(documentText, supportsVSExtensions: supportsVSExtensions, isRazorFile: true);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_Razor_DoesNotApplyOnNonTagHelpersAsync(bool supportsVSExtensions)
    {
        var documentText = """
            @addTagHelpers *, TestAssembly
            <p></p>
            """;

        await VerifySemanticTokensAsync(documentText, supportsVSExtensions: supportsVSExtensions, isRazorFile: true);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_Razor_CodeDirectiveAsync(bool supportsVSExtensions)
    {
        var documentText = """
            @code {}
            """;

        await VerifySemanticTokensAsync(documentText, supportsVSExtensions: supportsVSExtensions, isRazorFile: true);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_Razor_CodeDirectiveBodyAsync(bool supportsVSExtensions)
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

        await VerifySemanticTokensAsync(documentText, supportsVSExtensions: supportsVSExtensions, isRazorFile: true);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_Razor_UsingDirective(bool supportsVSExtensions)
    {
        var documentText = """
            @using System.Threading
            """;

        await VerifySemanticTokensAsync(documentText, supportsVSExtensions: supportsVSExtensions, isRazorFile: true);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_Razor_FunctionsDirectiveAsync(bool supportsVSExtensions)
    {
        var documentText = """
            @functions {}
            """;

        await VerifySemanticTokensAsync(documentText, supportsVSExtensions: supportsVSExtensions, isRazorFile: true);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_Razor_NestedTextDirectives(bool supportsVSExtensions)
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

        await VerifySemanticTokensAsync(documentText, supportsVSExtensions: supportsVSExtensions);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_Razor_NestedTransitions(bool supportsVSExtensions)
    {
        var documentText = """
            @using System
            @functions {
                Action<object> abc = @<span></span>;
            }
            """;

        await VerifySemanticTokensAsync(documentText, supportsVSExtensions: supportsVSExtensions, isRazorFile: true);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_Razor_CommentAsync(bool supportsVSExtensions)
    {
        var documentText = """
            @* A comment *@
            """;

        await VerifySemanticTokensAsync(documentText, supportsVSExtensions: supportsVSExtensions, isRazorFile: true);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_Razor_MultiLineCommentMidlineAsync(bool supportsVSExtensions)
    {
        var documentText = """
            <a />@* kdl
            skd
            slf*@
            """;

        await VerifySemanticTokensAsync(documentText, supportsVSExtensions: supportsVSExtensions);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_Razor_MultiLineCommentWithBlankLines(bool supportsVSExtensions)
    {
        var documentText = """
            @* kdl

            skd
                
                    sdfasdfasdf
            slf*@
            """;

        await VerifySemanticTokensAsync(documentText, supportsVSExtensions: supportsVSExtensions);
    }

    [Theory]
    [CombinatorialData]
    [WorkItem("https://github.com/dotnet/razor/issues/8176")]
    public async Task GetSemanticTokens_Razor_MultiLineCommentWithBlankLines_LF(bool supportsVSExtensions)
    {
        var documentText = "@* kdl\n\nskd\n    \n        sdfasdfasdf\nslf*@";

        await VerifySemanticTokensAsync(documentText, supportsVSExtensions: supportsVSExtensions);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_Razor_MultiLineCommentAsync(bool supportsVSExtensions)
    {
        var documentText = """
            @*stuff
            things *@
            """;

        await VerifySemanticTokensAsync(documentText, supportsVSExtensions: supportsVSExtensions);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_CSharp_Static(bool supportsVSExtensions)
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

        await VerifySemanticTokensAsync(documentText, supportsVSExtensions: supportsVSExtensions, isRazorFile: true);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_Legacy_Model(bool supportsVSExtensions)
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

        await VerifySemanticTokensAsync(documentText, supportsVSExtensions: supportsVSExtensions, isRazorFile: false);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_CSharp_LargeFile(bool supportsVSExtensions)
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

        await VerifySemanticTokensAsync(documentText, supportsVSExtensions: supportsVSExtensions, isRazorFile: true);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_CSharp_Static_WithBackground(bool supportsVSExtensions)
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

        await VerifySemanticTokensAsync(documentText, supportsVSExtensions: supportsVSExtensions, isRazorFile: true, withCSharpBackground: true);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_CSharp_Tabs_Static_WithBackground(bool supportsVSExtensions)
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

        await VerifySemanticTokensAsync(documentText, supportsVSExtensions: supportsVSExtensions, isRazorFile: true, withCSharpBackground: true);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_CSharp_WithBackground(bool supportsVSExtensions)
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

        await VerifySemanticTokensAsync(documentText, supportsVSExtensions: supportsVSExtensions, isRazorFile: true, withCSharpBackground: true);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_CSharp_WitRenderFragment(bool supportsVSExtensions)
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

        await VerifySemanticTokensAsync(documentText, supportsVSExtensions: supportsVSExtensions, isRazorFile: true);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_CSharp_WitRenderFragmentAndBackground(bool supportsVSExtensions)
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

        await VerifySemanticTokensAsync(documentText, supportsVSExtensions: supportsVSExtensions, isRazorFile: true, withCSharpBackground: true);
    }

    [Theory]
    [CombinatorialData]
    public async Task GetSemanticTokens_CSharp_ExplicitStatement_WithBackground(bool supportsVSExtensions)
    {
        var documentText = """
            @DateTime.Now

            @("hello" + "\\n" + "world" + Environment.NewLine + "how are you?")
            """;

        await VerifySemanticTokensAsync(documentText, supportsVSExtensions: supportsVSExtensions, isRazorFile: true, withCSharpBackground: true);
    }

    [Fact]
    public void GetMappedCSharpRanges_MinimalRangeVsSmallDisjointRanges_DisjointRangesAreSmaller()
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

        Assert.True(RazorSemanticTokensInfoService.TryGetSortedCSharpRanges(codeDocument, razorRange, out var csharpRanges));
        Assert.Equal(spans.Length, csharpRanges.Length);
        for (var i = 0; i < csharpRanges.Length; i++)
        {
            var csharpRange = csharpRanges[i];
            var textSpan = csharpSourceText.GetTextSpan(csharpRange);
            Assert.Equal(spans[i].Length, textSpan.Length);
        }
    }

    private async Task VerifySemanticTokensAsync(string documentText, bool isRazorFile = false, bool withCSharpBackground = false, bool supportsVSExtensions = true, [CallerMemberName] string? testName = null)
    {
        var csharpTokens = await GetCSharpSemanticTokensResponseAsync(documentText, isRazorFile, supportsVSExtensions);
        await AssertSemanticTokensAsync(documentText, csharpTokens, isRazorFile, withCSharpBackground: withCSharpBackground, supportsVSExtensions: supportsVSExtensions, testName: testName);
    }

    private async Task AssertSemanticTokensAsync(
        string documentText,
        ProvideSemanticTokensResponse csharpTokens,
        bool isRazorFile = false,
        int documentVersion = 0,
        bool withCSharpBackground = false,
        bool supportsVSExtensions = true,
        [CallerMemberName] string? testName = null)
    {
        var documentContext = CreateDocumentContext(documentText, isRazorFile, DefaultTagHelpers, documentVersion);

        var service = await CreateServiceAsync(documentContext, csharpTokens, withCSharpBackground, supportsVSExtensions);

        var range = GetSpan(documentText);
        var tokens = await service.GetSemanticTokensAsync(documentContext, range, withCSharpBackground, Guid.Empty, DisposalToken);

        var sourceText = await documentContext.GetSourceTextAsync(DisposalToken);

        AssertSemanticTokensMatchesBaseline(sourceText, tokens, supportsVSExtensions, testName.AssumeNotNull());
    }

    private static DocumentContext CreateDocumentContext(
        string documentText,
        bool isRazorFile,
        TagHelperCollection tagHelpers,
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
        bool supportsVSExtensions)
    {
        _clientConnection
            .Setup(l => l.SendRequestAsync<SemanticTokensParams, ProvideSemanticTokensResponse?>(
                CustomMessageNames.RazorProvidePreciseRangeSemanticTokensEndpoint,
                It.IsAny<SemanticTokensParams>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(csharpTokens);

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

        var csharpSemanticTokensProvider = new LSPCSharpSemanticTokensProvider(_clientConnection.Object, LoggerFactory);

        var service = new RazorSemanticTokensInfoService(
            documentMappingService,
            TestRazorSemanticTokensLegendService.GetInstance(supportsVSExtensions),
            csharpSemanticTokensProvider,
            LoggerFactory);

        return service;
    }

    private async Task<ProvideSemanticTokensResponse> GetCSharpSemanticTokensResponseAsync(string documentText, bool isRazorFile = false, bool supportsVSExtensions = true)
    {
        var codeDocument = CreateCodeDocument(documentText, isRazorFile, DefaultTagHelpers);
        var csharpDocumentUri = new Uri("C:\\TestSolution\\TestProject\\TestDocument.cs");
        var csharpSourceText = codeDocument.GetCSharpSourceText();

        await using var csharpServer = await CSharpTestLspServerHelpers.CreateCSharpLspServerAsync(
            csharpSourceText,
            csharpDocumentUri,
            s_semanticTokensServerCapabilities,
            SpanMappingService,
            capabilitiesUpdater: c =>
            {
                c.SupportsVisualStudioExtensions = supportsVSExtensions;
            },
            DisposalToken);

        var razorRange = GetSpan(documentText);
        var csharpRanges = GetMappedCSharpRanges(codeDocument, razorRange);
        if (csharpRanges == null)
        {
            return new ProvideSemanticTokensResponse(tokens: [], hostDocumentSyncVersion: 0);
        }

        var result = await csharpServer.ExecuteRequestAsync<SemanticTokensRangesParams, SemanticTokens>(
            "roslyn/semanticTokenRanges",
            CreateVSSemanticTokensRangesParams(csharpRanges.Value, csharpDocumentUri),
            DisposalToken);

        return new ProvideSemanticTokensResponse(tokens: result?.Data, hostDocumentSyncVersion: 0);
    }

    private static LinePositionSpan GetSpan(string text)
    {
        var lineCount = text.Count(c => c == '\n') + 1;

        return new LinePositionSpan(new LinePosition(0, 0), new LinePosition(lineCount, 0));
    }

    private void AssertSemanticTokensMatchesBaseline(SourceText sourceText, int[]? actualSemanticTokens, bool supportsVSExtensions, string testName)
    {
        if (!supportsVSExtensions)
        {
            testName += "_VSCode";
        }

        var fileName = $"Semantic\\TestFiles\\{testName}";

        var baselineFileName = Path.ChangeExtension(fileName, ".semantic.txt");

        var actualFileContents = GetFileRepresentationOfTokens(sourceText, actualSemanticTokens, supportsVSExtensions);

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

    private ImmutableArray<LinePositionSpan>? GetMappedCSharpRanges(RazorCodeDocument codeDocument, LinePositionSpan razorRange)
    {
        return RazorSemanticTokensInfoService.TryGetSortedCSharpRanges(codeDocument, razorRange, out var csharpRanges)
            ? csharpRanges
            : null;
    }

    private static SemanticTokensRangesParams CreateVSSemanticTokensRangesParams(ImmutableArray<LinePositionSpan> ranges, Uri uri)
        => new()
        {
            TextDocument = new TextDocumentIdentifier { DocumentUri = new(uri) },
            Ranges = ranges.Select(s => s.ToRange()).ToArray()
        };

    private static void GenerateSemanticBaseline(string actualFileContents, string baselineFileName)
    {
        var semanticBaselinePath = Path.Combine(s_projectPath, baselineFileName);
        File.WriteAllText(semanticBaselinePath, actualFileContents);
    }

    private static string GetFileRepresentationOfTokens(SourceText sourceText, int[]? data, bool supportsVSExtensions)
    {
        if (data == null)
        {
            return string.Empty;
        }

        using var _ = StringBuilderPool.GetPooledObject(out var builder);
        builder.AppendLine("//line,characterPos,length,tokenType,modifier,text");
        var legendArray = TestRazorSemanticTokensLegendService.GetInstance(supportsVSExtensions).TokenTypes.All;
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
