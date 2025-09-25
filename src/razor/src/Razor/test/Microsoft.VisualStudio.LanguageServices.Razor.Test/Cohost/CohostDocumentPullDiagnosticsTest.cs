// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public Task NoDiagnostics()
        => VerifyDiagnosticsAsync("""
            <div></div>

            @code
            {
                public void IJustMetYou()
                {
                }
            }
            """);

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
    public Task CSharpAndRazor_MiscellaneousFile()
        => VerifyDiagnosticsAsync("""
            <div>

            {|RZ10012:<NonExistentComponent />|}

            </div>

            @code
            {
                public void IJustMetYou()
                {
                    {|CS0103:CallMeMaybe|}();
                }
            }
            """,
            miscellaneousFile: true);

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
                    new LspDiagnostic
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
                    new LspDiagnostic
                    {
                        Code = CSSErrorCodes.UnrecognizedBlockType,
                        Range = SourceText.From(input.Text).GetRange(new TextSpan(input.Text.IndexOf("@@") + 1, 1))
                    },
                    new LspDiagnostic
                    {
                        Code = CSSErrorCodes.UnrecognizedBlockType,
                        Range = SourceText.From(input.Text).GetRange(new TextSpan(input.Text.IndexOf("f"), 1))
                    }
                ]
            }]);
    }

    [Fact]
    public Task FilterCSharpFromCss()
    {
        TestCode input = """
            <div>

            <style>
                @{ insertSomeBigBlobOfCSharp(); }

                {|CSS031:~|}~~~~
            </style>

            </div>

            @code {
                string insertSomeBigBlobOfCSharp() => "body { font-weight: bold; }";
            }
            """;

        return VerifyDiagnosticsAsync(input,
            htmlResponse: [new VSInternalDiagnosticReport
            {
                Diagnostics =
                [
                    new LspDiagnostic
                    {
                        Code = CSSErrorCodes.MissingSelectorBeforeCombinatorCode,
                        Range = SourceText.From(input.Text).GetRange(new TextSpan(input.Text.IndexOf("@{"), 1))
                    },
                    new LspDiagnostic
                    {
                        Code = CSSErrorCodes.MissingSelectorBeforeCombinatorCode,
                        Range = SourceText.From(input.Text).GetRange(new TextSpan(input.Text.IndexOf("~~"), 1))
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

                {|CSS031:~|}~~~~
            </style>

            </div>
            """;

        return VerifyDiagnosticsAsync(input,
            htmlResponse: [new VSInternalDiagnosticReport
            {
                Diagnostics =
                [
                    new LspDiagnostic
                    {
                        Code = CSSErrorCodes.MissingSelectorBeforeCombinatorCode,
                        Range = SourceText.From(input.Text).GetRange(new TextSpan(input.Text.IndexOf("@*"), 1))
                    },
                    new LspDiagnostic
                    {
                        Code = CSSErrorCodes.MissingSelectorBeforeCombinatorCode,
                        Range = SourceText.From(input.Text).GetRange(new TextSpan(input.Text.IndexOf("~~"), 1))
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

                {|CSS031:~|}~~~~
            </style>

            </div>
            """;

        return VerifyDiagnosticsAsync(input,
            htmlResponse: [new VSInternalDiagnosticReport
            {
                Diagnostics =
                [
                    new LspDiagnostic
                    {
                        Code = CSSErrorCodes.MissingSelectorBeforeCombinatorCode,
                        Range = SourceText.From(input.Text).GetRange(new TextSpan(input.Text.IndexOf("Ra"), 1))
                    },
                    new LspDiagnostic
                    {
                        Code = CSSErrorCodes.MissingSelectorBeforeCombinatorCode,
                        Range = SourceText.From(input.Text).GetRange(new TextSpan(input.Text.IndexOf("~~"), 1))
                    }
                ]
            }]);
    }

    [Fact]
    public Task FilterMissingClassNameInCss()
    {
        TestCode input = """
            <div>

            <style>
              .@(className)
                background-color: lightblue;
              }

              .{|CSS008:{|}
                bar: baz;
              }
            </style>

            </div>

            @code
            {
                private string className = "foo";
            }
            """;

        return VerifyDiagnosticsAsync(input,
            htmlResponse: [new VSInternalDiagnosticReport
            {
                Diagnostics =
                [
                    new LspDiagnostic
                    {
                        Code = CSSErrorCodes.MissingClassNameAfterDot,
                        Range = SourceText.From(input.Text).GetRange(new TextSpan(input.Text.IndexOf(".@") + 1, 1))
                    },
                    new LspDiagnostic
                    {
                        Code = CSSErrorCodes.MissingClassNameAfterDot,
                        Range = SourceText.From(input.Text).GetRange(new TextSpan(input.Text.IndexOf(".{") + 1, 1))
                    },
                ]
            }]);
    }

    [Fact]
    public Task FilterMissingClassNameInCss_WithSpace()
    {
        TestCode input = """
            <div>

            <style>
              . @(className)
                background-color: lightblue;
              }

              .{|CSS008: |}{
                bar: baz;
              }
            </style>

            </div>

            @code
            {
                private string className = "foo";
            }
            """;

        return VerifyDiagnosticsAsync(input,
            htmlResponse: [new VSInternalDiagnosticReport
            {
                Diagnostics =
                [
                    new LspDiagnostic
                    {
                        Code = CSSErrorCodes.MissingClassNameAfterDot,
                        Range = SourceText.From(input.Text).GetRange(new TextSpan(input.Text.IndexOf(". @") + 1, 1))
                    },
                    new LspDiagnostic
                    {
                        Code = CSSErrorCodes.MissingClassNameAfterDot,
                        Range = SourceText.From(input.Text).GetRange(new TextSpan(input.Text.IndexOf(". {") + 1, 1))
                    },
                ]
            }]);
    }

    [Fact]
    public Task FilterPropertyValueInCss()
    {
        TestCode input = """
            <div>

            <style>
              .goo {
                background-color: @(color);
              }

              .foo {
                background-color:{|CSS025: |}/* no value here */;
              }
            </style>

            </div>

            @code
            {
                private string color = "red";
            }
            """;

        return VerifyDiagnosticsAsync(input,
            htmlResponse: [new VSInternalDiagnosticReport
            {
                Diagnostics =
                [
                    new LspDiagnostic
                    {
                        Code = CSSErrorCodes.MissingPropertyValue,
                        Range = SourceText.From(input.Text).GetRange(new TextSpan(input.Text.IndexOf(": @") + 1, 1))
                    },
                    new LspDiagnostic
                    {
                        Code = CSSErrorCodes.MissingPropertyValue,
                        Range = SourceText.From(input.Text).GetRange(new TextSpan(input.Text.IndexOf(": /") + 1, 1))
                    },
                ]
            }]);
    }

    [Fact]
    public Task FilterPropertyNameInCss()
    {
        TestCode input = """
            <div style="{|CSS024:/|}****/"></div>
            <div style="@(someBool ? "width: 100%" : "width: 50%")">

            </div>

            @code
            {
                private bool someBool = false;
            }
            """;

        return VerifyDiagnosticsAsync(input,
            htmlResponse: [new VSInternalDiagnosticReport
            {
                Diagnostics =
                [
                    new LspDiagnostic
                    {
                        Code = CSSErrorCodes.MissingPropertyName,
                        Range = SourceText.From(input.Text).GetRange(new TextSpan(input.Text.IndexOf("/"), 1))
                    },
                    new LspDiagnostic
                    {
                        Code = CSSErrorCodes.MissingPropertyName,
                        Range = SourceText.From(input.Text).GetRange(new TextSpan(input.Text.IndexOf("@"), 1))
                    },
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

            // TODO: This isn't C#

            TODO: Nor is this

            <div>

                @*{|TODO: TODO: This does |}*@

                @* TODONT: This doesn't *@

            </div>

            @code {
                // This looks different because Roslyn only reports zero width ranges for task lists
                // {|TODO:|}TODO: Write some C# code too
            }
            """,
            taskListRequest: true);

    private async Task VerifyDiagnosticsAsync(TestCode input, VSInternalDiagnosticReport[]? htmlResponse = null, bool taskListRequest = false, bool miscellaneousFile = false)
    {
        var document = CreateProjectAndRazorDocument(input.Text, miscellaneousFile: miscellaneousFile);
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
        var report = Assert.Single(result);
        Assert.NotNull(report);

        var markers = report.Diagnostics.SelectMany(d =>
            new[] {
                (index: inputText.GetTextSpan(d.Range).Start, text: $"{{|{d.Code!.Value.Second}:"),
                (index: inputText.GetTextSpan(d.Range).End, text:"|}")
            });

        var testOutput = input.Text;
        // Ordering by text last means start tags get sorted before end tags, for zero width ranges
        foreach (var (index, text) in markers.OrderByDescending(i => i.index).ThenByDescending(i => i.text))
        {
            testOutput = testOutput.Insert(index, text);
        }

        AssertEx.EqualOrDiff(input.OriginalInput, testOutput);
    }
}
