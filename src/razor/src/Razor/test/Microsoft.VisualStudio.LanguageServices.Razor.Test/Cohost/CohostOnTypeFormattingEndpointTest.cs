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
public class CohostOnTypeFormattingEndpointTest(HtmlFormattingFixture htmlFormattingFixture, ITestOutputHelper testOutputHelper)
    : CohostEndpointTestBase(testOutputHelper)
{
    [Fact]
    public async Task InvalidTrigger()
    {
        await VerifyOnTypeFormattingAsync(
            input: """
                    @{
                     if(true){}$$
                    }
                    """,
            expected: """
                    @{
                     if(true){}
                    }
                    """,
            triggerCharacter: 'h');
    }

    [Fact]
    public async Task CSharp_InvalidTrigger()
    {
        await VerifyOnTypeFormattingAsync(
            input: """
                    @{
                     if(true){}$$
                    }
                    """,
            expected: """
                    @{
                     if(true){}
                    }
                    """,
            triggerCharacter: '\n');
    }

    [Fact]
    public async Task CSharp()
    {
        await VerifyOnTypeFormattingAsync(
            input: """
                    @{
                     if(true){}$$
                    }
                    """,
            expected: """
                    @{
                        if (true) { }
                    }
                    """,
            triggerCharacter: '}');
    }

    [Fact]
    public async Task FormatsSimpleHtmlTag_OnType()
    {
        await VerifyOnTypeFormattingAsync(
            input: """
                    <html>
                    <head>
                        <title>Hello</title>
                            <script>
                                var x = 2;$$
                            </script>
                    </head>
                    </html>
                    """,
            expected: """
                    <html>
                    <head>
                        <title>Hello</title>
                        <script>
                            var x = 2;
                        </script>
                    </head>
                    </html>
                    """,
            triggerCharacter: ';',
            html: true);
    }

    private async Task VerifyOnTypeFormattingAsync(TestCode input, string expected, char triggerCharacter, bool html = false)
    {
        var document = CreateProjectAndRazorDocument(input.Text);
        var inputText = await document.GetTextAsync(DisposalToken);
        var position = inputText.GetPosition(input.Position);

        LSPRequestInvoker requestInvoker;
        if (html)
        {
            var htmlDocumentPublisher = new HtmlDocumentPublisher(RemoteServiceInvoker, StrictMock.Of<TrackingLSPDocumentManager>(), StrictMock.Of<JoinableTaskContext>(), LoggerFactory);
            var generatedHtml = await htmlDocumentPublisher.GetHtmlSourceFromOOPAsync(document, DisposalToken);
            Assert.NotNull(generatedHtml);

            var uri = new Uri(document.CreateUri(), $"{document.FilePath}{FeatureOptions.HtmlVirtualDocumentSuffix}");
            var htmlEdits = await htmlFormattingFixture.Service.GetOnTypeFormattingEditsAsync(LoggerFactory, uri, generatedHtml, position, insertSpaces: true, tabSize: 4);

            requestInvoker = new TestLSPRequestInvoker([(Methods.TextDocumentOnTypeFormattingName, htmlEdits)]);
        }
        else
        {
            // We use a mock here so that it will throw if called
            requestInvoker = StrictMock.Of<LSPRequestInvoker>();
        }

        var clientSettingsManager = new ClientSettingsManager(changeTriggers: []);

        var endpoint = new CohostOnTypeFormattingEndpoint(RemoteServiceInvoker, TestHtmlDocumentSynchronizer.Instance, requestInvoker, clientSettingsManager, LoggerFactory);

        var request = new DocumentOnTypeFormattingParams()
        {
            TextDocument = new TextDocumentIdentifier() { Uri = document.CreateUri() },
            Options = new FormattingOptions()
            {
                TabSize = 4,
                InsertSpaces = true
            },
            Character = triggerCharacter.ToString(),
            Position = position
        };

        var edits = await endpoint.GetTestAccessor().HandleRequestAsync(request, document, DisposalToken);

        if (edits is null)
        {
            Assert.Equal(expected, input.Text);
            return;
        }

        var changes = edits.Select(inputText.GetTextChange);
        var finalText = inputText.WithChanges(changes);

        AssertEx.EqualOrDiff(expected, finalText.ToString());
    }
}
