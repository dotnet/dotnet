// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis.Razor.Diagnostics;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.Razor.Settings;
using Roslyn.Test.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

public class CohostDocumentPullDiagnosticsTest(ITestOutputHelper testOutputHelper) : CohostEndpointTestBase(testOutputHelper)
{
    [Fact]
    public Task CSharp()
        => VerifyDiagnosticsAsync("""
            <div></div>

            @code
            {
                public void IJustMetYou()
                {
                    {|CS0103:CallMeMaybe|}();
                }
            }
            """);

    [Fact]
    public Task Razor()
        => VerifyDiagnosticsAsync("""
            <div>

            {|RZ10012:<NonExistentComponent />|}

            </div>
            """);

    [Fact]
    public Task Html()
    {
        TestCode input = """
            <div>

            {|HTM1337:<not_a_tag />|}

            </div>
            """;

        return VerifyDiagnosticsAsync(input,
            htmlResponse: [new VSInternalDiagnosticReport
            {
                Diagnostics =
                [
                    new Diagnostic
                    {
                        Code = "HTM1337",
                        Range = SourceText.From(input.Text).GetRange(input.NamedSpans.First().Value.First())
                    }
                ]
            }]);
    }

    [Fact]
    public Task FilterEscapedAtFromCss()
    {
        TestCode input = """
            <div>

            <style>
              @@media (max-width: 600px) {
                body {
                  background-color: lightblue;
                }
              }

              {|CSS002:f|}oo
              {
                bar: baz;
              }
            </style>

            </div>
            """;

        return VerifyDiagnosticsAsync(input,
            htmlResponse: [new VSInternalDiagnosticReport
            {
                Diagnostics =
                [
                    new Diagnostic
                    {
                        Code = CSSErrorCodes.UnrecognizedBlockType,
                        Range = SourceText.From(input.Text).GetRange(new TextSpan(input.Text.IndexOf("@@") + 1, 1))
                    },
                    new Diagnostic
                    {
                        Code = CSSErrorCodes.UnrecognizedBlockType,
                        Range = SourceText.From(input.Text).GetRange(new TextSpan(input.Text.IndexOf("f"), 1))
                    }
                ]
            }]);
    }

    [Fact]
    public Task CombinedAndNestedDiagnostics()
        => VerifyDiagnosticsAsync("""
            @using System.Threading.Tasks;

            <div>

            {|RZ10012:<NonExistentComponent />|}

            @code
            {
                public void IJustMetYou()
                {
                    {|CS0103:CallMeMaybe|}();
                }
            }

            <div>
                @{
                    {|CS4033:await Task.{|CS1501:Delay|}()|};
                }

                {|RZ9980:<p>|}
            </div>

            </div>
            """);

    [Fact]
    public Task TODOComments()
        => VerifyDiagnosticsAsync("""
            @using System.Threading.Tasks;

            <div>

                @*{|TODO: TODO: This does |}*@

                @* TODONT: This doesn't *@

            </div>
            """,
            taskListRequest: true);

    private async Task VerifyDiagnosticsAsync(TestCode input, VSInternalDiagnosticReport[]? htmlResponse = null, bool taskListRequest = false)
    {
        var document = CreateProjectAndRazorDocument(input.Text, createSeparateRemoteAndLocalWorkspaces: true);
        var inputText = await document.GetTextAsync(DisposalToken);

        var requestInvoker = new TestLSPRequestInvoker([(VSInternalMethods.DocumentPullDiagnosticName, htmlResponse)]);

        var clientSettingsManager = new ClientSettingsManager([]);
        var endpoint = new CohostDocumentPullDiagnosticsEndpoint(RemoteServiceInvoker, TestHtmlDocumentSynchronizer.Instance, requestInvoker, clientSettingsManager, LoggerFactory);

        var result = taskListRequest
            ? await endpoint.GetTestAccessor().HandleTaskListItemRequestAsync(document, ["TODO"], DisposalToken)
            : await endpoint.GetTestAccessor().HandleRequestAsync(document, DisposalToken);

        var markers = result!.SelectMany(d => d.Diagnostics.AssumeNotNull()).SelectMany(d =>
            new[] {
                (index: inputText.GetTextSpan(d.Range).Start, text: $"{{|{d.Code!.Value.Second}:"),
                (index: inputText.GetTextSpan(d.Range).End, text:"|}")
            });

        var testOutput = input.Text;
        foreach (var (index, text) in markers.OrderByDescending(i => i.index))
        {
            testOutput = testOutput.Insert(index, text);
        }

        AssertEx.EqualOrDiff(input.OriginalInput, testOutput);
    }
}
