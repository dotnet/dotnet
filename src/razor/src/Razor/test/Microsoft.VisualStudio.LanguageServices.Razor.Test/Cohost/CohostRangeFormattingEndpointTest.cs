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
public class CohostRangeFormattingEndpointTest(HtmlFormattingFixture htmlFormattingFixture, ITestOutputHelper testOutputHelper)
    : CohostEndpointTestBase(testOutputHelper)
{
    [Fact]
    public Task RangeFormatting()
        => VerifyRangeFormattingAsync(
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
            [|<div>
             @{
                <div>
            <div>
                    This is heavily nested
            </div>
             </div>
                }
                    </div>|]
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
            """);

    private async Task VerifyRangeFormattingAsync(TestCode input, string expected)
    {
        var document = await CreateProjectAndRazorDocumentAsync(input.Text);
        var inputText = await document.GetTextAsync(DisposalToken);

        var htmlDocumentPublisher = new HtmlDocumentPublisher(RemoteServiceInvoker, StrictMock.Of<TrackingLSPDocumentManager>(), StrictMock.Of<JoinableTaskContext>(), LoggerFactory);
        var generatedHtml = await htmlDocumentPublisher.GetHtmlSourceFromOOPAsync(document, DisposalToken);
        Assert.NotNull(generatedHtml);

        var uri = new Uri(document.CreateUri(), $"{document.FilePath}{FeatureOptions.HtmlVirtualDocumentSuffix}");
        var htmlEdits = await htmlFormattingFixture.Service.GetDocumentFormattingEditsAsync(LoggerFactory, uri, generatedHtml, insertSpaces: true, tabSize: 4);

        var requestInvoker = new TestLSPRequestInvoker([(Methods.TextDocumentFormattingName, htmlEdits)]);

        var clientSettingsManager = new ClientSettingsManager(changeTriggers: []);

        var endpoint = new CohostRangeFormattingEndpoint(RemoteServiceInvoker, TestHtmlDocumentSynchronizer.Instance, requestInvoker, clientSettingsManager, LoggerFactory);

        var request = new DocumentRangeFormattingParams()
        {
            TextDocument = new TextDocumentIdentifier() { Uri = document.CreateUri() },
            Options = new FormattingOptions()
            {
                TabSize = 4,
                InsertSpaces = true
            },
            Range = inputText.GetRange(input.Span)
        };

        var edits = await endpoint.GetTestAccessor().HandleRequestAsync(request, document, DisposalToken);

        var changes = edits.Select(inputText.GetTextChange);
        var finalText = inputText.WithChanges(changes);

        AssertEx.EqualOrDiff(expected, finalText.ToString());
    }
}
