﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Text;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

using static HoverAssertions;

public class CohostHoverEndpointTest(ITestOutputHelper testOutputHelper) : CohostEndpointTestBase(testOutputHelper)
{
    [Fact]
    public async Task Razor()
    {
        TestCode code = """
            <[|PageTi$$tle|]></PageTitle>
            <div></div>
            
            @{
                var myVariable = "Hello";
            
                var length = myVariable.Length;
            }
            """;

        await VerifyHoverAsync(code, async (hover, document) =>
        {
            await hover.VerifyRangeAsync(code.Span, document);

            hover.VerifyRawContent(
                Container(
                    Container(
                        Image,
                        ClassifiedText( // Microsoft.AspNetCore.Components.Web.PageTitle
                            Text("Microsoft"),
                            Punctuation("."),
                            Text("AspNetCore"),
                            Punctuation("."),
                            Text("Components"),
                            Punctuation("."),
                            Text("Web"),
                            Punctuation("."),
                            Type("PageTitle")))));
        });
    }

    [Fact]
    public async Task Html()
    {
        TestCode code = """
            <PageTitle></PageTitle>
            <div$$></div>
            
            @{
                var myVariable = "Hello";
            
                var length = myVariable.Length;
            }
            """;

        // This simply verifies that Hover will call into HTML.
        var htmlResponse = new VSInternalHover();

        await VerifyHoverAsync(code, htmlResponse, h => Assert.Same(htmlResponse, h));
    }

    [Fact]
    public async Task CSharp()
    {
        TestCode code = """
            <PageTitle></PageTitle>
            <div></div>

            @{
                var $$[|myVariable|] = "Hello";

                var length = myVariable.Length;
            }
            """;

        await VerifyHoverAsync(code, async (hover, document) =>
        {
            await hover.VerifyRangeAsync(code.Span, document);

            hover.VerifyRawContent(
                Container(
                    Container(
                        Image,
                        ClassifiedText( // (local variable) string myVariable
                            Punctuation("("),
                            Text("local variable"),
                            Punctuation(")"),
                            WhiteSpace(" "),
                            Keyword("string"),
                            WhiteSpace(" "),
                            LocalName("myVariable")))));
        });
    }

    [Fact]
    public async Task ComponentAttribute()
    {
        // Component attributes are within HTML but actually map to C#.
        // In this situation, Hover prefers treating the position as C# and calls
        // Roslyn rather than HTML.

        TestCode code = """
            <EditForm [|Form$$Name|]="Hello" />
            """;

        await VerifyHoverAsync(code, async (hover, document) =>
        {
            await hover.VerifyRangeAsync(code.Span, document);

            hover.VerifyRawContent(
                Container(
                    Container(
                        Image,
                        ClassifiedText( // string? EditForm.FormName { get; set; }
                            Keyword("string"),
                            Punctuation("?"),
                            WhiteSpace(" "),
                            ClassName("EditForm"),
                            Punctuation("."),
                            PropertyName("FormName"),
                            WhiteSpace(" "),
                            Punctuation("{"),
                            WhiteSpace(" "),
                            Keyword("get"),
                            Punctuation(";"),
                            WhiteSpace(" "),
                            Keyword("set"),
                            Punctuation(";"),
                            WhiteSpace(" "),
                            Punctuation("}")))));
        });
    }

    [Fact]
    public async Task Component_WithCallbacks()
    {
        TestCode code = """
            <[|Inpu$$tText|] ValueChanged="Foo"
                       DisplayName="Foo"
                       @onchange="Foo"
                       @onfocus="Foo"
                       @onblur="Foo" />

            @code {
                private void Foo()
                {
                }
            }
            """;

        await VerifyHoverAsync(code, async (hover, document) =>
        {
            await hover.VerifyRangeAsync(code.Span, document);

            hover.VerifyRawContent(
                Container(
                    Container(
                        Image,
                        ClassifiedText( // Microsoft.ApsNetCore.Components.Forms.InputText
                            Text("Microsoft"),
                            Punctuation("."),
                            Text("AspNetCore"),
                            Punctuation("."),
                            Text("Components"),
                            Punctuation("."),
                            Text("Forms"),
                            Punctuation("."),
                            Type("InputText")))));
        });
    }

    private async Task VerifyHoverAsync(TestCode input, Func<Hover, TextDocument, Task> verifyHover)
    {
        var document = CreateProjectAndRazorDocument(input.Text);
        var result = await GetHoverResultAsync(document, input);

        Assert.NotNull(result);
        await verifyHover(result, document);
    }

    private async Task VerifyHoverAsync(TestCode input, Hover htmlResponse, Action<Hover?> verifyHover)
    {
        var document = CreateProjectAndRazorDocument(input.Text);
        var result = await GetHoverResultAsync(document, input, htmlResponse);

        Assert.NotNull(result);
        verifyHover(result);
    }

    private async Task<Hover?> GetHoverResultAsync(TextDocument document, TestCode input, Hover? htmlResponse = null)
    {
        var inputText = await document.GetTextAsync(DisposalToken);
        var linePosition = inputText.GetLinePosition(input.Position);

        var requestInvoker = new TestHtmlRequestInvoker([(Methods.TextDocumentHoverName, htmlResponse)]);
        var endpoint = new CohostHoverEndpoint(IncompatibleProjectService, RemoteServiceInvoker, requestInvoker);

        var textDocumentPositionParams = new TextDocumentPositionParams
        {
            Position = LspFactory.CreatePosition(linePosition),
            TextDocument = new TextDocumentIdentifier { DocumentUri = document.CreateDocumentUri() },
        };

        return await endpoint.GetTestAccessor().HandleRequestAsync(textDocumentPositionParams, document, DisposalToken);
    }
}
