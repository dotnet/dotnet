// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.LanguageServer.Test;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis.Razor.Diagnostics;
using Microsoft.CodeAnalysis.Razor.Telemetry;
using Microsoft.CodeAnalysis.Text;
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
    public Task FilterRazorCommentsFromCss()
    {
        TestCode input = """
            <div>

            <style>
                @* This is a Razor comment *@
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
                        Code = CSSErrorCodes.MissingSelectorBeforeCombinatorCode,
                        Range = SourceText.From(input.Text).GetRange(new TextSpan(input.Text.IndexOf("@*"), 1))
                    }
                ]
            }]);
    }

    [Fact]
    public Task FilterRazorCommentsFromCss_Inside()
    {
        TestCode input = """
            <div>

            <style>
                @* This is a Razor comment *@
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
                        Code = CSSErrorCodes.MissingSelectorBeforeCombinatorCode,
                        Range = SourceText.From(input.Text).GetRange(new TextSpan(input.Text.IndexOf("Ra"), 1))
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

        var requestInvoker = new TestHtmlRequestInvoker([(VSInternalMethods.DocumentPullDiagnosticName, htmlResponse)]);

        var clientSettingsManager = new ClientSettingsManager([]);
        var clientCapabilitiesService = new TestClientCapabilitiesService(new VSInternalClientCapabilities { SupportsVisualStudioExtensions = true });
        var endpoint = new CohostDocumentPullDiagnosticsEndpoint(IncompatibleProjectService, RemoteServiceInvoker, requestInvoker, clientSettingsManager, clientCapabilitiesService, NoOpTelemetryReporter.Instance, LoggerFactory);

        var result = taskListRequest
            ? await endpoint.GetTestAccessor().HandleTaskListItemRequestAsync(document, ["TODO"], DisposalToken)
            : [new()
                {
                    Diagnostics = await endpoint.GetTestAccessor().HandleRequestAsync(document, DisposalToken)
                }];

        Assert.NotNull(result);

        if (result is [{ Diagnostics: null }])
        {
            // No diagnostics found, make sure none were expected
            AssertEx.Equal(input.OriginalInput, input.Text);
            return;
        }

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
