// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.DocumentHighlighting;

public class DocumentHighlightEndpointTest(ITestOutputHelper testOutput) : LanguageServerTestBase(testOutput)
{
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
        var serverCapabilities = new VSInternalServerCapabilities()
        {
            DocumentHighlightProvider = true
        };
        await using var csharpServer = await CSharpTestLspServerHelpers.CreateCSharpLspServerAsync(
            csharpSourceText, csharpDocumentUri, serverCapabilities, razorMappingService: null, capabilitiesUpdater: null, DisposalToken);
        await csharpServer.OpenDocumentAsync(csharpDocumentUri, csharpSourceText.ToString());

        var razorFilePath = "C:/path/to/file.razor";
        var documentContextFactory = new TestDocumentContextFactory(razorFilePath, codeDocument);
        var languageServerFeatureOptions = Mock.Of<LanguageServerFeatureOptions>(options =>
            options.SupportsFileManipulation == true &&
            options.SingleServerSupport == true &&
            options.CSharpVirtualDocumentSuffix == ".g.cs" &&
            options.HtmlVirtualDocumentSuffix == ".g.html",
            MockBehavior.Strict);

        var languageServer = new DocumentHighlightServer(csharpServer, csharpDocumentUri);
        var documentMappingService = new LspDocumentMappingService(FilePathService, documentContextFactory, LoggerFactory);

        var endpoint = new DocumentHighlightEndpoint(
            languageServerFeatureOptions, documentMappingService, languageServer, LoggerFactory);

        var request = new DocumentHighlightParams
        {
            TextDocument = new TextDocumentIdentifier
            {
                Uri = new Uri(razorFilePath)
            },
            Position = codeDocument.Source.Text.GetPosition(cursorPosition)
        };

        var documentContext = CreateDocumentContext(request.TextDocument.Uri, codeDocument);
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(request, requestContext, DisposalToken);

        // Assert
        var sourceText = codeDocument.Source.Text;
        var expected = spans
            .Select(sourceText.GetRange)
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

    private class DocumentHighlightServer(CSharpTestLspServer csharpServer, Uri csharpDocumentUri) : IClientConnection
    {
        public Task SendNotificationAsync<TParams>(string method, TParams @params, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SendNotificationAsync(string method, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<TResponse> SendRequestAsync<TParams, TResponse>(string method, TParams @params, CancellationToken cancellationToken)
        {
            Assert.Equal(CustomMessageNames.RazorDocumentHighlightEndpointName, method);
            var highlightParams = Assert.IsType<DelegatedPositionParams>(@params);

            var highlightRequest = new DocumentHighlightParams()
            {
                TextDocument = new TextDocumentIdentifier()
                {
                    Uri = csharpDocumentUri
                },
                Position = highlightParams.ProjectedPosition,
            };

            var result = await csharpServer.ExecuteRequestAsync<DocumentHighlightParams, DocumentHighlight[]>(
                Methods.TextDocumentDocumentHighlightName, highlightRequest, cancellationToken);

            return (TResponse)(object)result;
        }
    }
}
