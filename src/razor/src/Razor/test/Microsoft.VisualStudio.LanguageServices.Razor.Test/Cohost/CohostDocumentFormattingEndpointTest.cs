// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Razor.Formatting;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.Razor.Settings;
using Microsoft.VisualStudio.Threading;
using Roslyn.Test.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

[Collection(HtmlFormattingCollection.Name)]
public class CohostDocumentFormattingEndpointTest(HtmlFormattingFixture htmlFormattingFixture, ITestOutputHelper testOutputHelper)
    : CohostEndpointTestBase(testOutputHelper)
{
    // All of the formatting tests in the language server exercise the formatting engine and cover various edge cases
    // and provide regression prevention. The tests here are not exhaustive, but they validate the the cohost endpoints
    // call into the formatting engine at least, and handles C#, Html and Razor formatting changes correctly.

    [Fact]
    public Task Formatting()
        => VerifyDocumentFormattingAsync(
            input: """
            @preservewhitespace    true

                        <div></div>

            @{
            <p>
                    @{
                            var t = 1;
            if (true)
            {
            
                        }
                    }
                    </p>
            <div>
             @{
                <div>
            <div>
                    This is heavily nested
            </div>
             </div>
                }
                    </div>
            }

            @code {
                            private void M(string thisIsMyString)
                {
                    var x = 5;

                                var y = "Hello";

                    M("Hello");
                }
            }

            """,
            expected: """
            @preservewhitespace true

            <div></div>

            @{
                <p>
                    @{
                        var t = 1;
                        if (true)
                        {
            
                        }
                    }
                </p>
                <div>
                    @{
                        <div>
                            <div>
                                This is heavily nested
                            </div>
                        </div>
                    }
                </div>
            }

            @code {
                private void M(string thisIsMyString)
                {
                    var x = 5;

                    var y = "Hello";

                    M("Hello");
                }
            }

            """);

    private async Task VerifyDocumentFormattingAsync(string input, string expected)
    {
        var document = await CreateProjectAndRazorDocumentAsync(input);
        var inputText = await document.GetTextAsync(DisposalToken);

        var htmlDocumentPublisher = new HtmlDocumentPublisher(RemoteServiceInvoker, StrictMock.Of<TrackingLSPDocumentManager>(), StrictMock.Of<JoinableTaskContext>(), LoggerFactory);
        var generatedHtml = await htmlDocumentPublisher.GetHtmlSourceFromOOPAsync(document, DisposalToken);
        Assert.NotNull(generatedHtml);

        var uri = new Uri(document.CreateUri(), $"{document.FilePath}{FeatureOptions.HtmlVirtualDocumentSuffix}");
        var htmlEdits = await htmlFormattingFixture.Service.GetDocumentFormattingEditsAsync(LoggerFactory, uri, generatedHtml, insertSpaces: true, tabSize: 4);

        var requestInvoker = new TestLSPRequestInvoker([(Methods.TextDocumentFormattingName, htmlEdits)]);

        var clientSettingsManager = new ClientSettingsManager(changeTriggers: []);

        var endpoint = new CohostDocumentFormattingEndpoint(RemoteServiceInvoker, TestHtmlDocumentSynchronizer.Instance, requestInvoker, clientSettingsManager, LoggerFactory);

        var request = new DocumentFormattingParams()
        {
            TextDocument = new TextDocumentIdentifier() { Uri = document.CreateUri() },
            Options = new FormattingOptions()
            {
                TabSize = 4,
                InsertSpaces = true
            }
        };

        var edits = await endpoint.GetTestAccessor().HandleRequestAsync(request, document, DisposalToken);

        var changes = edits.Select(inputText.GetTextChange);
        var finalText = inputText.WithChanges(changes);

        AssertEx.EqualOrDiff(expected, finalText.ToString());
    }
}
