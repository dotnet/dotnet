// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

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

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

public partial class CohostDocumentPullDiagnosticsTest
{
    [Fact]
    public Task OneOfEachDiagnostic()
    {
        TestCode input = """
            <div>

            {|HTM1337:<not_a_tag />|}

            {|RZ10012:<NonExistentComponent />|}

            </div>

            <script>
                {|TS2304:let foo: string = 42;|}
            </script>

            <style>
                {|CSS002:f|}oo
                {
                    bar: baz;
                }
            </style>

            @code
            {
                public void IJustMetYou()
                {
                    {|CS0103:CallMeMaybe|}();
                }
            }
            """;

        return VerifyDiagnosticsAsync(input,
           htmlResponse: [new VSInternalDiagnosticReport
            {
                Diagnostics =
                [
                    new VSDiagnostic
                    {
                        Code = "HTM1337",
                        Range = SourceText.From(input.Text).GetRange(input.NamedSpans["HTM1337"].First()),
                        Projects = [new VSDiagnosticProjectInformation()
                        {
                            ProjectIdentifier = "Html"
                        }]
                    },
                    new VSDiagnostic
                    {
                        Code = "TS2304",
                        Range = SourceText.From(input.Text).GetRange(input.NamedSpans["TS2304"].First()),
                        Projects = [new VSDiagnosticProjectInformation()
                        {
                            ProjectIdentifier = "TypeScript"
                        }]
                    },
                    new VSDiagnostic
                    {
                        Code = "CSS002",
                        Range = SourceText.From(input.Text).GetRange(input.NamedSpans["CSS002"].First()),
                        Projects = [new VSDiagnosticProjectInformation()
                        {
                            ProjectIdentifier = "CSS"
                        }]
                    },
                ]
            }]);
    }

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
        const string CSharpExpression = """@(someBool ? "width: 100%" : "width: 50%")""";
        TestCode input = $$"""
            <div style="{|CSS024:/****/|}"></div>
            <div style="{{CSharpExpression}}">

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
                        Range = SourceText.From(input.Text).GetRange(new TextSpan(input.Text.IndexOf("/"), "/****/".Length))
                    },
                    new LspDiagnostic
                    {
                        Code = CSSErrorCodes.MissingPropertyName,
                        Range = SourceText.From(input.Text).GetRange(new TextSpan(input.Text.IndexOf("@"), CSharpExpression.Length))
                    },
                ]
            }]);
    }

    [Fact]
    public Task FilterFromMultilineComponentAttributes()
    {
        var firstLine = "Hello this is a";
        TestCode input = $$"""
            <File1 Title="{{firstLine}}
                          multiline attribute" />

            @code
            {
                [Parameter]
                public string Title { get; set; }
            }
            """;

        return VerifyDiagnosticsAsync(input,
            htmlResponse: [new VSInternalDiagnosticReport
            {
                Diagnostics =
                [
                    new LspDiagnostic
                    {
                        Code = HtmlErrorCodes.MismatchedAttributeQuotesErrorCode,
                        Range = SourceText.From(input.Text).GetRange(new TextSpan(input.Text.IndexOf(firstLine), firstLine.Length))
                    },
                ]
            }]);
    }

    [Fact]
    public Task DontFilterFromMultilineHtmlAttributes()
    {
        var firstLine = "Hello this is a";
        TestCode input = $$"""
            <div class="{|HTML0005:{{firstLine}}|}
                        multiline attribute" />
            """;

        return VerifyDiagnosticsAsync(input,
            htmlResponse: [new VSInternalDiagnosticReport
            {
                Diagnostics =
                [
                    new LspDiagnostic
                    {
                        Code = HtmlErrorCodes.MismatchedAttributeQuotesErrorCode,
                        Range = SourceText.From(input.Text).GetRange(new TextSpan(input.Text.IndexOf(firstLine), firstLine.Length))
                    },
                ]
            }]);
    }

    [Theory]
    [InlineData("", "\"")]
    [InlineData("", "'")]
    [InlineData("@onclick=\"Send\"", "\"")] // The @onclick makes the disabled attribute a TagHelperAttributeSyntax
    [InlineData("@onclick='Send'", "'")]
    public Task FilterBadAttributeValueInHtml(string extraTagContent, string quoteChar)
    {
        TestCode input = $$"""
            <button {{extraTagContent}} disabled={{quoteChar}}@(!EnableMyButton){{quoteChar}}>Send</button>
            <button disabled={{quoteChar}}{|HTML0209:ThisIsNotValid|}{{quoteChar}} />

            @code
            {
                private bool EnableMyButton => true;

                Task Send() =>
                    Task.CompletedTask;
            }
            """;

        return VerifyDiagnosticsAsync(input,
            htmlResponse: [new VSInternalDiagnosticReport
            {
                Diagnostics =
                [
                    new LspDiagnostic
                    {
                        Code = HtmlErrorCodes.UnknownAttributeValueErrorCode,
                        Range = SourceText.From(input.Text).GetRange(new TextSpan(input.Text.IndexOf("@("), "@(!EnableMyButton)".Length))
                    },
                    new LspDiagnostic
                    {
                        Code = HtmlErrorCodes.UnknownAttributeValueErrorCode,
                        Range = SourceText.From(input.Text).GetRange(new TextSpan(input.Text.IndexOf("T"), "ThisIsNotValid".Length))
                    },
                ]
            }]);
    }

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

        if (!taskListRequest)
        {
            Assert.NotNull(report.Diagnostics);
            Assert.All(report.Diagnostics,
                d =>
                {
                    var vsDiagnostic = Assert.IsType<VSDiagnostic>(d);
                    Assert.NotNull(vsDiagnostic.Identifier);
                    Assert.NotNull(vsDiagnostic.Projects);
                    var project = Assert.Single(vsDiagnostic.Projects);
                    Assert.NotNull(project.ProjectIdentifier);
                    // We always report the same project info for all diagnostics
                    Assert.Same(project, ((VSDiagnostic)report.Diagnostics.First()).Projects.Single());
                });
        }
    }
}
