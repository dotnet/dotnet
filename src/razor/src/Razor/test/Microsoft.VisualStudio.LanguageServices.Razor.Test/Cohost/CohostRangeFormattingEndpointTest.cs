// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Razor.Formatting;
using Microsoft.CodeAnalysis.Razor.Remote;
using Microsoft.VisualStudio.Razor.Settings;
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
        var document = CreateProjectAndRazorDocument(input.Text);
        var inputText = await document.GetTextAsync(DisposalToken);

        var generatedHtml = await RemoteServiceInvoker.TryInvokeAsync<IRemoteHtmlDocumentService, string?>(document.Project.Solution,
            (service, solutionInfo, ct) => service.GetHtmlDocumentTextAsync(solutionInfo, document.Id, ct),
            DisposalToken).ConfigureAwait(false);
        Assert.NotNull(generatedHtml);

        var uri = new Uri(document.CreateUri(), $"{document.FilePath}{FeatureOptions.HtmlVirtualDocumentSuffix}");
        var htmlEdits = await htmlFormattingFixture.Service.GetDocumentFormattingEditsAsync(LoggerFactory, uri, generatedHtml, insertSpaces: true, tabSize: 4);

        var requestInvoker = new TestHtmlRequestInvoker([(Methods.TextDocumentFormattingName, htmlEdits)]);

        var clientSettingsManager = new ClientSettingsManager(changeTriggers: []);

        var endpoint = new CohostRangeFormattingEndpoint(IncompatibleProjectService, RemoteServiceInvoker, requestInvoker, clientSettingsManager, LoggerFactory);

        var request = new DocumentRangeFormattingParams()
        {
            TextDocument = new TextDocumentIdentifier() { DocumentUri = document.CreateDocumentUri() },
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
