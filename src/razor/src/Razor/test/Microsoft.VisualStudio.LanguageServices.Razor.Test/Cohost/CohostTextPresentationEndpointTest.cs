// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

public class CohostTextPresentationEndpointTest(ITestOutputHelper testOutputHelper) : CohostEndpointTestBase(testOutputHelper)
{
    [Fact]
    public async Task HtmlResponse_TranslatesVirtualDocumentUri()
    {
        await VerifyUriPresentationAsync(
            input: """
                This is a Razor document.

                <div>
                    [||]
                </div>

                The end.
                """,
            text: "Hello World",
            htmlResponse: new WorkspaceEdit
            {
                DocumentChanges = new TextDocumentEdit[]
                {
                    new()
                    {
                        TextDocument = new()
                        {
                            Uri = FileUri("File1.razor.g.html")
                        },
                        Edits = [VsLspFactory.CreateTextEdit(position: (0, 0), "Hello World")]
                    }
                }
            },
            expected: "Hello World");
    }

    private async Task VerifyUriPresentationAsync(string input, string text, string? expected, WorkspaceEdit? htmlResponse = null)
    {
        TestFileMarkupParser.GetSpan(input, out input, out var span);
        var document = CreateProjectAndRazorDocument(input);
        var sourceText = await document.GetTextAsync(DisposalToken);

        var requestInvoker = new TestLSPRequestInvoker([(VSInternalMethods.TextDocumentTextPresentationName, htmlResponse)]);

        var endpoint = new CohostTextPresentationEndpoint(TestHtmlDocumentSynchronizer.Instance, FilePathService, requestInvoker);

        var request = new VSInternalTextPresentationParams()
        {
            TextDocument = new TextDocumentIdentifier()
            {
                Uri = document.CreateUri()
            },
            Range = sourceText.GetRange(span),
            Text = text
        };

        var result = await endpoint.GetTestAccessor().HandleRequestAsync(request, document, DisposalToken);

        if (expected is null)
        {
            Assert.Null(result);
        }
        else
        {
            Assert.NotNull(result);
            Assert.NotNull(result.DocumentChanges);
            Assert.Equal(expected, result.DocumentChanges.Value.First[0].Edits[0].NewText);
            Assert.Equal(document.CreateUri(), result.DocumentChanges.Value.First[0].TextDocument.Uri);
        }
    }
}
