// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Roslyn.Test.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

public class CohostFoldingRangeEndpointTest(ITestOutputHelper testOutputHelper) : CohostEndpointTestBase(testOutputHelper)
{
    [Fact]
    public Task IfStatements()
        => VerifyFoldingRangesAsync("""
            <div>
              @if (true) {[|
                <div>
                  Hello World
                </div>
              }|]
            </div>

            @if (true) {[|
              <div>
                Hello World
              </div>
            }|]

            @if (true) {[|
            }|]
            """);

    [Fact]
    public Task LockStatement()
        => VerifyFoldingRangesAsync("""
            @lock (new object()) {[|
            }|]
            """);

    [Fact]
    public Task UsingStatement()
      => VerifyFoldingRangesAsync("""
            @using (new object()) {[|
            }|]
            """);

    [Fact]
    public Task IfElseStatements()
        => VerifyFoldingRangesAsync("""
            <div>
              @if (true) {[|
                <div>
                  Hello World
                </div>
                else {[|
                <div>
                    Goodbye World
                </div>
                }|]
              }|]
            </div>
            """);

    [Fact]
    public Task Usings()
        => VerifyFoldingRangesAsync("""
            @using System[|
            @using System.Text|]

            <p>hello!</p>

            @using System.Buffers[|
            @using System.Drawing
            @using System.CodeDom|]

            <p>hello!</p>
            """);

    [Fact]
    public Task CSharpStatement()
        => VerifyFoldingRangesAsync("""
            <p>hello!</p>

            @{[|
                var helloWorld = "";
            }|]

            @(DateTime
                .Now)

            <p>hello!</p>
            """);

    [Fact]
    public Task CSharpStatement_Nested()
        => VerifyFoldingRangesAsync("""
            <p>hello!</p>

            <div>

                @{[|
                    var helloWorld = "";
                }|]

            </div>

            @(DateTime
                .Now)

            <p>hello!</p>
            """);

    [Fact]
    public Task CSharpStatement_NotSingleLine()
        => VerifyFoldingRangesAsync("""
            <p>hello!</p>

            @{ var helloWorld = ""; }

            <p>hello!</p>
            """);

    [Fact]
    public Task CodeBlock()
        => VerifyFoldingRangesAsync("""
            <p>hello!</p>

            @code {[|
                var helloWorld = "";
            }|]

            <p>hello!</p>
            """);

    [Fact]
    public Task CodeBlock_Mvc()
        => VerifyFoldingRangesAsync("""
            <p>hello!</p>

            @functions {[|
                var helloWorld = "";
            }|]

            <p>hello!</p>
            """,
            fileKind: FileKinds.Legacy);

    [Fact]
    public Task Section()
        => VerifyFoldingRangesAsync("""
            <p>hello!</p>

            @section Hello {[|
                <p>Hello</p>
            }|]

            <p>hello!</p>
            """,
            fileKind: FileKinds.Legacy);

    [Fact]
    public Task Section_Invalid()
        => VerifyFoldingRangesAsync("""
            <p>hello!</p>

            @section {
                <p>Hello</p>
            }

            <p>hello!</p>
            """,
            fileKind: FileKinds.Legacy);

    [Fact]
    public Task CSharpCodeInCodeBlocks()
       => VerifyFoldingRangesAsync("""
            <div>
              Hello @_name
            </div>

            @code {[|
                private string _name = "Dave";

                public void M() {[|
                }|]
            }|]
            """);

    [Fact]
    public Task HtmlAndCSharp()
      => VerifyFoldingRangesAsync("""
            <div>{|html:
              Hello @_name

                <div>{|html:
                    Nests aren't just for birds!
                </div>|}
            </div>|}

            @code {[|
                private string _name = "Dave";

                public void M() {[|
                }|]
            }|]
            """);

    private async Task VerifyFoldingRangesAsync(string input, string? fileKind = null)
    {
        TestFileMarkupParser.GetSpans(input, out var source, out ImmutableDictionary<string, ImmutableArray<TextSpan>> spans);
        var document = await CreateProjectAndRazorDocumentAsync(source, fileKind);
        var inputText = await document.GetTextAsync(DisposalToken);

        var htmlSpans = spans.GetValueOrDefault("html").NullToEmpty();
        var htmlRanges = htmlSpans
            .Select(span =>
                {
                    var (start, end) = inputText.GetLinePositionSpan(span);
                    return new FoldingRange()
                    {
                        StartLine = start.Line,
                        StartCharacter = start.Character,
                        EndLine = end.Line,
                        EndCharacter = end.Character
                    };
                })
            .ToArray();

        var requestInvoker = new TestLSPRequestInvoker([(Methods.TextDocumentFoldingRangeName, htmlRanges)]);

        var endpoint = new CohostFoldingRangeEndpoint(RemoteServiceInvoker, TestHtmlDocumentSynchronizer.Instance, requestInvoker, LoggerFactory);

        var result = await endpoint.GetTestAccessor().HandleRequestAsync(document, DisposalToken);

        if (spans.Count == 0)
        {
            Assert.Null(result);
            return;
        }

        var actual = GenerateTestInput(inputText, htmlSpans, result.AssumeNotNull());

        AssertEx.EqualOrDiff(input, actual);
    }

    private static string GenerateTestInput(SourceText inputText, ImmutableArray<TextSpan> htmlSpans, FoldingRange[] result)
    {
        var markerPositions = result
            .SelectMany(r =>
                new[] {
                    (index: inputText.GetRequiredAbsoluteIndex(r.StartLine, r.StartCharacter.AssumeNotNull()), isStart: true),
                    (index: inputText.GetRequiredAbsoluteIndex(r.EndLine, r.EndCharacter.AssumeNotNull()), isStart: false)
                });

        var actual = new StringBuilder(inputText.ToString());
        foreach (var marker in markerPositions.OrderByDescending(p => p.index))
        {
            actual.Insert(marker.index, GetMarker(marker.index, marker.isStart, htmlSpans));
        }

        static string GetMarker(int index, bool isStart, ImmutableArray<TextSpan> htmlSpans)
        {
            if (isStart)
            {
                return htmlSpans.Any(r => r.Start == index)
                    ? "{|html:"
                    : "[|";
            }

            return htmlSpans.Any(r => r.End == index)
                ? "|}"
                : "|]";
        }

        return actual.ToString();
    }
}
