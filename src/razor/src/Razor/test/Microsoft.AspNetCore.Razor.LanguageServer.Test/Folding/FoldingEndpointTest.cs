// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Folding;

public class FoldingEndpointTest(ITestOutputHelper testOutput) : SingleServerDelegatingEndpointTestBase(testOutput)
{
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

        var languageServer = await CreateLanguageServerAsync(codeDocument, filePath);

        var endpoint = new FoldingRangeEndpoint(
            DocumentMappingService,
            languageServer,
            [
                new UsingsFoldingRangeProvider(),
                new RazorCodeBlockFoldingProvider(),
                new RazorCSharpStatementFoldingProvider(),
                new SectionDirectiveFoldingProvider()
            ],
            LoggerFactory);

        var request = new FoldingRangeParams()
        {
            TextDocument = new VSTextDocumentIdentifier
            {
                Uri = new Uri(filePath),
                ProjectContext = new VSProjectContext()
                {
                    Label = "test",
                    Kind = VSProjectKind.CSharp,
                    Id = "test"
                }
            }
        };
        Assert.True(DocumentContextFactory.TryCreateForOpenDocument(request.TextDocument, out var documentContext));
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
            var expectedRange = expected[i].ToRange(inputText);
            var foldingRange = resultArray[i];
            Assert.Equal(expectedRange.Start.Line, foldingRange.StartLine);
            Assert.Equal(expectedRange.End.Line, foldingRange.EndLine);
        }
    }
}
