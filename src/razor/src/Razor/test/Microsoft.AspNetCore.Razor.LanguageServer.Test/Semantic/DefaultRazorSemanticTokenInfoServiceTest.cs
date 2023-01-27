// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.Formatting;
using Microsoft.AspNetCore.Razor.LanguageServer.Semantic;
using Microsoft.AspNetCore.Razor.Test.Common.Mef;
using Microsoft.CommonLanguageServerProtocol.Framework;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Test.Semantic;

// Sets the FileName static variable.
// Finds the test method name using reflection, and uses
// that to find the expected input/output test files as Embedded resources.
[IntializeTestFile]
[UseExportProvider]
public class DefaultRazorSemanticTokenInfoServiceTest : SemanticTokenTestBase
{
    public DefaultRazorSemanticTokenInfoServiceTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
    }

    #region CSharp
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
        var csharpTokens = new ProvideSemanticTokensResponse(tokens: Array.Empty<int>(), hostDocumentSyncVersion: null);
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
    }

    [Fact]
    public async Task GetSemanticTokens_CSharp_Implicit()
    {
        var documentText =
            """
                @addTagHelper *, TestAssembly
                @{ var d = "txt";}
                @d
                """;

        var razorRange = GetRange(documentText);
        var csharpTokens = await GetCSharpSemanticTokensResponseAsync(documentText, razorRange, isRazorFile: false);
        await AssertSemanticTokensAsync(documentText, isRazorFile: false, razorRange, csharpTokens: csharpTokens);
    }

    [Fact]
    public async Task GetSemanticTokens_CSharp_VersionMismatch()
    {
        var documentText =
            """
                @addTagHelper *, TestAssembly
                @{ var d = }
                """;

        var razorRange = GetRange(documentText);
        var csharpTokens = await GetCSharpSemanticTokensResponseAsync(documentText, razorRange, isRazorFile: false);
        await AssertSemanticTokensAsync(documentText, isRazorFile: false, razorRange, csharpTokens: csharpTokens, documentVersion: 21);
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
    }
    #endregion

    #region HTML
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
    }
    #endregion

    #region TagHelpers
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
    }
    #endregion TagHelpers

    #region DirectiveAttributes
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
    }
    #endregion DirectiveAttributes

    #region Directive
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
    }
    #endregion

    [Fact]
    public async Task GetSemanticTokens_Razor_CommentAsync()
    {
        var documentText = """
                @* A comment *@
                """;

        var razorRange = GetRange(documentText);
        var csharpTokens = await GetCSharpSemanticTokensResponseAsync(documentText, razorRange, isRazorFile: true);
        await AssertSemanticTokensAsync(documentText, isRazorFile: true, razorRange, csharpTokens: csharpTokens);
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
    }

    private async Task AssertSemanticTokensAsync(
        string documentText,
        bool isRazorFile,
        Range range,
        RazorSemanticTokensInfoService? service = null,
        ProvideSemanticTokensResponse? csharpTokens = null,
        int documentVersion = 0)
    {
        await AssertSemanticTokensAsync(new DocumentContentVersion[]
        {
            new DocumentContentVersion(documentText, documentVersion)
        },
        isRazorArray: new bool[] { isRazorFile },
        range,
        service,
        csharpTokens,
        documentVersion);
    }

    private async Task AssertSemanticTokensAsync(
        DocumentContentVersion[] documentTexts,
        bool[] isRazorArray,
        Range range,
        RazorSemanticTokensInfoService? service = null,
        ProvideSemanticTokensResponse? csharpTokens = null,
        int documentVersion = 0)
    {
        // Arrange
        if (csharpTokens is null)
        {
            csharpTokens = new ProvideSemanticTokensResponse(tokens: null, csharpTokens?.HostDocumentSyncVersion);
        }

        var (documentContexts, textDocumentIdentifiers) = CreateDocumentContext(
            documentTexts, isRazorArray, DefaultTagHelpers, documentVersion: documentVersion);

        if (service is null)
        {
            service = GetDefaultRazorSemanticTokenInfoService(documentContexts, csharpTokens);
        }

        var textDocumentIdentifier = textDocumentIdentifiers.Dequeue();
        var documentContext = documentContexts.Peek();

        // Act
        var tokens = await service.GetSemanticTokensAsync(textDocumentIdentifier, range, documentContext, DisposalToken);

        // Assert
        AssertSemanticTokensMatchesBaseline(tokens?.Data);
    }

    private RazorSemanticTokensInfoService GetDefaultRazorSemanticTokenInfoService(
        Queue<VersionedDocumentContext> documentSnapshots,
        ProvideSemanticTokensResponse? csharpTokens = null)
    {
        var languageServer = new Mock<ClientNotifierServiceBase>(MockBehavior.Strict);
        languageServer
            .Setup(l => l.SendRequestAsync<SemanticTokensParams, ProvideSemanticTokensResponse?>(
                RazorLanguageServerCustomMessageTargets.RazorProvideSemanticTokensRangeEndpoint,
                It.IsAny<SemanticTokensParams>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(csharpTokens);

        var documentContextFactory = new TestDocumentContextFactory(documentSnapshots);
        var documentMappingService = new DefaultRazorDocumentMappingService(TestLanguageServerFeatureOptions.Instance, documentContextFactory, LoggerFactory);

        var testClient = new TestClient();
        var errorReporter = new LanguageServerErrorReporter(LoggerFactory);
        var settingsManager = new TestInitializeManager();
        var semanticTokensRefreshPublisher = new DefaultWorkspaceSemanticTokensRefreshPublisher(settingsManager, testClient, errorReporter);

        return new DefaultRazorSemanticTokensInfoService(
            languageServer.Object,
            documentMappingService,
            LoggerFactory);
    }

    private static Range GetRange(string text)
    {
        var lines = text.Split(Environment.NewLine);
        var lastLineIndex = lines.Length - 1;
        var lastLineCharacterIndex = lines[lastLineIndex].Length;

        var range = new Range
        {
            Start = new Position { Line = 0, Character = 0 },
            End = new Position { Line = lastLineIndex, Character = lastLineCharacterIndex }
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

        public override Task<VersionedDocumentContext?> TryCreateForOpenDocumentAsync(Uri documentFilePath, CancellationToken _)
        {
            var document = _documentContexts.Count == 1 ? _documentContexts.Peek() : _documentContexts.Dequeue();

            return Task.FromResult<VersionedDocumentContext?>(document);
        }

        public override Task<DocumentContext?> TryCreateAsync(Uri documentUri, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
