// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.FoldingRanges;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Text;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Folding;

public class FoldingEndpointTest(ITestOutputHelper testOutput) : SingleServerDelegatingEndpointTestBase(testOutput)
{
    [Fact]
    public Task IfStatements()
        => VerifyRazorFoldsAsync("""
            <div>
              [|@if (true) {
                <div>
                  Hello World
                </div>
              }|]
            </div>

            [|@if (true) {
              <div>
                Hello World
              </div>
            }|]

            [|@if (true) {
            }|]
            """);

    [Fact]
    public Task LockStatement()
        => VerifyRazorFoldsAsync("""
            [|@lock (new object()) {
            }|]
            """);

    [Fact]
    public Task UsingStatement()
      => VerifyRazorFoldsAsync("""
            [|@using (new object()) {
            }|]
            """);

    [Fact]
    public Task IfElseStatements()
        // This is not great, but I'm parking it here to demonstrate current behaviour. The Razor syntax tree is really
        // not doing us any favours with this. The "else" token is not even the first child of its parent!
        // Would be good to get the compiler to revisit this.
        => VerifyRazorFoldsAsync("""
            <div>
              [|@if (true) {
                <div>
                  Hello World
                </div>
                else {
                <div>
                    Goodbye World
                </div>
                }
              }|]
            </div>
            """);

    [Fact]
    public Task Usings()
        => VerifyRazorFoldsAsync("""
            [|@using System
            @using System.Text|]

            <p>hello!</p>

            [|@using System.Buffers
            @using System.Drawing
            @using System.CodeDom|]

            <p>hello!</p>
            """);

    [Fact]
    public Task CSharpStatement()
        => VerifyRazorFoldsAsync("""
            <p>hello!</p>

            [|@{
                var helloWorld = "";
            }|]

            @(DateTime
                .Now)

            <p>hello!</p>
            """);

    [Fact]
    public Task CSharpStatement_Nested()
        => VerifyRazorFoldsAsync("""
            <p>hello!</p>

            <div>

                [|@{
                    var helloWorld = "";
                }|]

            </div>

            @(DateTime
                .Now)

            <p>hello!</p>
            """);

    [Fact]
    public Task CSharpStatement_NotSingleLine()
        => VerifyRazorFoldsAsync("""
            <p>hello!</p>

            @{ var helloWorld = ""; }

            <p>hello!</p>
            """);

    [Fact]
    public Task CodeBlock()
        => VerifyRazorFoldsAsync("""
            <p>hello!</p>

            [|@code {
                var helloWorld = "";
            }|]

            <p>hello!</p>
            """);

    [Fact]
    public Task CodeBlock_Mvc()
        => VerifyRazorFoldsAsync("""
            <p>hello!</p>

            [|@functions {
                var helloWorld = "";
            }|]

            <p>hello!</p>
            """,
            filePath: "C:/path/to/file.cshtml");

    [Fact]
    public Task Section()
        => VerifyRazorFoldsAsync("""
            <p>hello!</p>

            [|@section Hello {
                <p>Hello</p>
            }|]

            <p>hello!</p>
            """,
            filePath: "C:/path/to/file.cshtml");

    [Fact]
    public Task Section_Invalid()
        => VerifyRazorFoldsAsync("""
            <p>hello!</p>

            @section {
                <p>Hello</p>
            }

            <p>hello!</p>
            """,
            filePath: "C:/path/to/file.cshtml");

    private async Task VerifyRazorFoldsAsync(string input, string? filePath = null)
    {
        filePath ??= "C:/path/to/file.razor";

        TestFileMarkupParser.GetSpans(input, out input, out ImmutableArray<TextSpan> expected);

        var codeDocument = CreateCodeDocument(input, filePath: filePath);

        await using var languageServer = await CreateLanguageServerAsync(codeDocument, filePath);

        var foldingRangeService = new FoldingRangeService(
            DocumentMappingService,
            [
                new UsingsFoldingRangeProvider(),
                new RazorCodeBlockFoldingProvider(),
                new RazorCSharpStatementFoldingProvider(),
                new SectionDirectiveFoldingProvider(),
                new RazorCSharpStatementKeywordFoldingProvider(),
            ],
            LoggerFactory);

        var endpoint = new FoldingRangeEndpoint(
            languageServer,
            foldingRangeService,
            LoggerFactory);

        var request = new FoldingRangeParams()
        {
            TextDocument = new VSTextDocumentIdentifier
            {
                DocumentUri = new(new Uri(filePath)),
                ProjectContext = new VSProjectContext()
                {
                    Label = "test",
                    Kind = VSProjectKind.CSharp,
                    Id = "test"
                }
            }
        };
        Assert.True(DocumentContextFactory.TryCreate(request.TextDocument, out var documentContext));
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(request, requestContext, DisposalToken);

        // Assert
        Assert.NotNull(result);
        var resultArray = result.ToArray();
        Assert.Equal(expected.Length, resultArray.Length);

        var inputText = SourceText.From(input);

        for (var i = 0; i < expected.Length; i++)
        {
            var expectedRange = inputText.GetRange(expected[i]);
            var foldingRange = resultArray[i];
            Assert.Equal(expectedRange.Start.Line, foldingRange.StartLine);
            Assert.Equal(expectedRange.End.Line, foldingRange.EndLine);
        }
    }
}
