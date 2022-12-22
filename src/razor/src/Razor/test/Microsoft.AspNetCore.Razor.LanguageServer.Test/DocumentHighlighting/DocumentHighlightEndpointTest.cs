// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.AspNetCore.Razor.LanguageServer.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.Mef;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Razor.Workspaces.Extensions;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.DocumentHighlighting.Test;

[UseExportProvider]
public class DocumentHighlightEndpointTest : LanguageServerTestBase
{
    public DocumentHighlightEndpointTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
    }

    [Fact]
    public async Task Handle_SingleServer_Local()
    {
        var input = """
                <div></div>

                @{
                    var [|$$myVariable|] = "Hello";

                    var length = [|myVariable|].Length;
                }
                """;

        await VerifyHighlightingRangesAsync(input);
    }

    [Fact]
    public async Task Handle_SingleServer_Method()
    {
        var input = """
                <div></div>

                @functions
                {
                    void [|Method|]()
                    {
                        [|$$Method|]();
                    }
                }
                """;

        await VerifyHighlightingRangesAsync(input);
    }

    [Fact]
    public async Task Handle_SingleServer_AttributeToField()
    {
        var input = """
                <div>
                    <div class="@[|$$_className|]">
                    </div>
                </div>

                @functions
                {
                    private string [|_className|] = "hello";
                }
                """;

        await VerifyHighlightingRangesAsync(input);
    }

    [Fact]
    public async Task Handle_SingleServer_FieldToAttribute()
    {
        var input = """
                <div>
                    <div class="@[|_className|]">
                    </div>
                </div>

                @functions
                {
                    private string [|$$_className|] = "hello";
                }
                """;

        await VerifyHighlightingRangesAsync(input);
    }

    private async Task VerifyHighlightingRangesAsync(string input)
    {
        // Arrange
        TestFileMarkupParser.GetPositionAndSpans(input, out var output, out int cursorPosition, out ImmutableArray<TextSpan> spans);
        var codeDocument = CreateCodeDocument(output);
        var csharpSourceText = codeDocument.GetCSharpSourceText();
        var csharpDocumentUri = new Uri("C:/path/to/file.razor__virtual.g.cs");
        var serverCapabilities = new ServerCapabilities()
        {
            DocumentHighlightProvider = true
        };
        var csharpServer = await CSharpTestLspServerHelpers.CreateCSharpLspServerAsync(
            csharpSourceText, csharpDocumentUri, serverCapabilities, razorSpanMappingService: null, DisposalToken);
        await csharpServer.OpenDocumentAsync(csharpDocumentUri, csharpSourceText.ToString());

        var razorFilePath = "C:/path/to/file.razor";
        var documentContextFactory = new TestDocumentContextFactory(razorFilePath, codeDocument, version: 1337);
        var languageServerFeatureOptions = Mock.Of<LanguageServerFeatureOptions>(options =>
            options.SupportsFileManipulation == true &&
            options.SingleServerSupport == true &&
            options.CSharpVirtualDocumentSuffix == ".g.cs" &&
            options.HtmlVirtualDocumentSuffix == ".g.html",
            MockBehavior.Strict);

        var languageServer = new DocumentHighlightServer(csharpServer, csharpDocumentUri);
        var documentMappingService = new DefaultRazorDocumentMappingService(languageServerFeatureOptions, documentContextFactory, LoggerFactory);

        var endpoint = new DocumentHighlightEndpoint(
            languageServerFeatureOptions, documentMappingService, languageServer, LoggerFactory);

        codeDocument.GetSourceText().GetLineAndOffset(cursorPosition, out var line, out var offset);
        var request = new DocumentHighlightParamsBridge
        {
            TextDocument = new TextDocumentIdentifier
            {
                Uri = new Uri(razorFilePath)
            },
            Position = new Position(line, offset)
        };

        var documentContext = CreateDocumentContext(request.TextDocument.Uri, codeDocument);
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(request, requestContext, DisposalToken);

        // Assert
        var sourceText = codeDocument.GetSourceText();
        var expected = spans
            .Select(s => s.AsRange(sourceText))
            .OrderBy(s => s.Start.Line)
            .ThenBy(s => s.Start.Character)
            .ToArray();
        var actual = result
            .Select(r => r.Range)
            .OrderBy(s => s.Start.Line)
            .ThenBy(s => s.Start.Character)
            .ToArray();
        Assert.Equal(actual, expected);
    }

    private class DocumentHighlightServer : ClientNotifierServiceBase
    {
        private readonly CSharpTestLspServer _csharpServer;
        private readonly Uri _csharpDocumentUri;

        public DocumentHighlightServer(CSharpTestLspServer csharpServer, Uri csharpDocumentUri)
        {
            _csharpServer = csharpServer;
            _csharpDocumentUri = csharpDocumentUri;
        }

        public override Task OnInitializedAsync(VSInternalClientCapabilities clientCapabilities, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public override Task SendNotificationAsync<TParams>(string method, TParams @params, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override Task SendNotificationAsync(string method, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override async Task<TResponse> SendRequestAsync<TParams, TResponse>(string method, TParams @params, CancellationToken cancellationToken)
        {
            Assert.Equal(RazorLanguageServerCustomMessageTargets.RazorDocumentHighlightEndpointName, method);
            var highlightParams = Assert.IsType<DelegatedPositionParams>(@params);

            var highlightRequest = new DocumentHighlightParams()
            {
                TextDocument = new TextDocumentIdentifier()
                {
                    Uri = _csharpDocumentUri
                },
                Position = highlightParams.ProjectedPosition,
            };

            var result = await _csharpServer.ExecuteRequestAsync<DocumentHighlightParams, DocumentHighlight[]>(
                Methods.TextDocumentDocumentHighlightName, highlightRequest, cancellationToken);

            return (TResponse)(object)result;
        }
    }
}
