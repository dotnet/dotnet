// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Text;
using Roslyn.LanguageServer.Protocol;
using Roslyn.Test.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

public class CohostInlayHintEndpointTest(ITestOutputHelper testOutputHelper) : CohostEndpointTestBase(testOutputHelper)
{
    [Fact]
    public Task InlayHints()
        => VerifyInlayHintsAsync(
            input: """

            <div></div>

            @functions {
                private void M(string thisIsMyString)
                {
                    var {|int:x|} = 5;

                    var {|string:y|} = "Hello";

                    M({|thisIsMyString:"Hello"|});
                }
            }

            """,
            toolTipMap: new Dictionary<string, string>
            {
                { "int",            "struct System.Int32"            },
                { "string",         "class System.String"            },
                { "thisIsMyString", "(parameter) string thisIsMyStr" }
            },
            output: """

            <div></div>

            @functions {
                private void M(string thisIsMyString)
                {
                    int x = 5;

                    string y = "Hello";

                    M(thisIsMyString: "Hello");
                }
            }

            """);

    [Fact]
    public Task InlayHints_DisplayAllOverride()
        => VerifyInlayHintsAsync(
            input: """

            <div></div>

            @functions {
                private void M(string thisIsMyString)
                {
                    {|int:var|} x = 5;

                    {|string:var|} y = "Hello";

                    M({|thisIsMyString:"Hello"|});
                }
            }

            """,
            toolTipMap: new Dictionary<string, string>
            {
                { "int",            "struct System.Int32"            },
                { "string",         "class System.String"            },
                { "thisIsMyString", "(parameter) string thisIsMyStr" }
            },
            output: """

            <div></div>

            @functions {
                private void M(string thisIsMyString)
                {
                    int x = 5;

                    string y = "Hello";

                    M(thisIsMyString: "Hello");
                }
            }

            """,
            displayAllOverride: true);

    [Fact]
    public Task InlayHints_ComponentAttributes()
        => VerifyInlayHintsAsync(
            input: """

                <div>
                    <InputText Value="_value" />
                    <InputText Value="@_value" />
                    <InputText Value="@(_value)" />
                </div>

                """,
            toolTipMap: [],
            output: """

                <div>
                    <InputText Value="_value" />
                    <InputText Value="@_value" />
                    <InputText Value="@(_value)" />
                </div>

                """);

    private async Task VerifyInlayHintsAsync(string input, Dictionary<string, string> toolTipMap, string output, bool displayAllOverride = false)
    {
        TestFileMarkupParser.GetSpans(input, out input, out ImmutableDictionary<string, ImmutableArray<TextSpan>> spansDict);
        var document = await CreateProjectAndRazorDocumentAsync(input);
        var inputText = await document.GetTextAsync(DisposalToken);

        var endpoint = new CohostInlayHintEndpoint(RemoteServiceInvoker);
        var resolveEndpoint = new CohostInlayHintResolveEndpoint(RemoteServiceInvoker, LoggerFactory);

        var request = new InlayHintParams()
        {
            TextDocument = new TextDocumentIdentifier() { Uri = document.CreateUri() },
            Range = new()
            {
                Start = new(0, 0),
                End = new(inputText.Lines.Count, 0)
            }
        };

        var hints = await endpoint.GetTestAccessor().HandleRequestAsync(request, document, displayAllOverride, DisposalToken);

        // Assert
        Assert.NotNull(hints);
        Assert.Equal(spansDict.Values.Count(), hints.Length);

        foreach (var hint in hints)
        {
            // Because our test input data can't have colons in the input, but parameter info returned from Roslyn does, we have to strip them off.
            var label = hint.Label.First.TrimEnd(':');
            Assert.True(spansDict.TryGetValue(label, out var spans), $"Expected {label} to be in test provided markers");

            var span = Assert.Single(spans);
            var expectedRange = inputText.GetRange(span);
            // Inlay hints only have a position, so we ignore the end of the range that comes from the test input
            Assert.Equal(expectedRange.Start, hint.Position);

            // This looks weird, but its what we have to do to satisfy the compiler :)
            string? expectedTooltip = null;
            Assert.True(toolTipMap?.TryGetValue(label, out expectedTooltip));
            Assert.NotNull(expectedTooltip);

            // We need to pretend we're making real LSP requests by serializing the data blob at least
            var serializedHint = JsonSerializer.Deserialize<InlayHint>(JsonSerializer.SerializeToElement(hint)).AssumeNotNull();
            // Make sure we can resolve the document correctly
            var tdi = resolveEndpoint.GetTestAccessor().GetTextDocumentIdentifier(serializedHint);
            Assert.NotNull(tdi);
            Assert.Equal(document.CreateUri(), tdi.Uri);

            // Make sure we, or really Roslyn, can resolve the hint correctly
            var resolvedHint = await resolveEndpoint.GetTestAccessor().HandleRequestAsync(serializedHint, document, DisposalToken);
            Assert.NotNull(resolvedHint);
            Assert.NotNull(resolvedHint.ToolTip);

            if (resolvedHint.ToolTip.Value.TryGetFirst(out var plainTextTooltip))
            {
                Assert.Equal(expectedTooltip, plainTextTooltip);
            }
            else if (resolvedHint.ToolTip.Value.TryGetSecond(out var markupTooltip))
            {
                Assert.Contains(expectedTooltip, markupTooltip.Value);
            }
        }

        // To validate edits, we have to collect them all together, and apply them backwards
        var changes = hints
            .SelectMany(h => h.TextEdits ?? [])
            .OrderByDescending(e => e.Range.Start.Line)
            .ThenByDescending(e => e.Range.Start.Character)
            .Select(inputText.GetTextChange);
        inputText = inputText.WithChanges(changes);

        AssertEx.EqualOrDiff(output, inputText.ToString());
    }
}
