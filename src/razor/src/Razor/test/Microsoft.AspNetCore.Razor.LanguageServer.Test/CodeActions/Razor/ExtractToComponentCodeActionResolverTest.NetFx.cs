// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Formatting;
using Microsoft.AspNetCore.Razor.Test.Common.Workspaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.CodeActions.Razor;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Roslyn.Test.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.CodeActions;

public class ExtractToComponentCodeActionResolverTest(ITestOutputHelper testOutput) : CodeActionEndToEndTestBase(testOutput)
{
    private const string ExtractToComponentTitle = "Extract element to new component";

    [Fact]
    public async Task Handle_SingleElement()
    {
        var input = """
            <[||]div id="a">
                <h1>Div a title</h1>
                <Book Title="To Kill a Mockingbird" Author="Harper Lee" Year="Long ago" />
                <p>Div a par</p>
            </div>
            <div id="shouldSkip">
                <Movie Title="Aftersun" Director="Charlotte Wells" Year="2022" />
            </div>
            """;

        var expectedRazorComponent = """
            <div id="a">
                <h1>Div a title</h1>
                <Book Title="To Kill a Mockingbird" Author="Harper Lee" Year="Long ago" />
                <p>Div a par</p>
            </div>
            """;

        var expectedOriginalDocument = """
            <Component />
            <div id="shouldSkip">
                <Movie Title="Aftersun" Director="Charlotte Wells" Year="2022" />
            </div>
            """;

        await TestAsync(
            input,
            expectedOriginalDocument,
            expectedRazorComponent);
    }

    [Fact]
    public async Task Handle_SiblingElement()
    {
        var input = """
            <[|div id="a">
                <h1>Div a title</h1>
                <Book Title="To Kill a Mockingbird" Author="Harper Lee" Year="Long ago" />
                <p>Div a par</p>
            </div>
            <div id="b">
                <Movie Title="Aftersun" Director="Charlotte Wells" Year="2022" />
            </div|]>
            """;

        var expectedRazorComponent = """
            <div id="a">
                <h1>Div a title</h1>
                <Book Title="To Kill a Mockingbird" Author="Harper Lee" Year="Long ago" />
                <p>Div a par</p>
            </div>
            <div id="b">
                <Movie Title="Aftersun" Director="Charlotte Wells" Year="2022" />
            </div>
            """;

        var expectedOriginalDocument = """
            <Component />
            """;

        await TestAsync(
            input,
            expectedOriginalDocument,
            expectedRazorComponent);
    }

    [Fact]
    public async Task Handle_AddsUsings()
    {
        var input = """
            @using MyApp.Data
            @using MyApp.Models

            <[|div id="parent">
                <div>
                    <div>
                        <div>
                            <p>Deeply nested par</p|]>
                        </div>
                    </div>
                </div>
            </div>
            """;

        var expectedRazorComponent = """
            @using MyApp.Data
            @using MyApp.Models
            
            <div id="parent">
                <div>
                    <div>
                        <div>
                            <p>Deeply nested par</p>
                        </div>
                    </div>
                </div>
            </div>
            """;

        var expectedOriginalDocument = """
            @using MyApp.Data
            @using MyApp.Models
            
            <Component />
            """;

        await TestAsync(
            input,
            expectedOriginalDocument,
            expectedRazorComponent);
    }

    [Fact]
    public async Task Handle_NestedNodes()
    {
        var input = """
            @page "/"
            @namespace MarketApp.Pages.Product.Home

            namespace MarketApp.Pages.Product.Home

            <PageTitle>Home</PageTitle>

            <div id="parent">
                <div>
                    <div>
                        <div>
                            <h1>[|Div a title</h1>
                            <p>Div a par</p>
                        </div>
                    </div>
                </div>
                <div>
                    <div>
                        <div>
                            <h1>Div b title</h1>
                            <p>Div b par|]</p>
                        </div>
                    </div>
                </div>
            </div>

            <h1>Hello, world!</h1>

            Welcome to your new app.
            """;

        var expectedRazorComponent = """
            <div>
                <div>
                    <div>
                        <h1>Div a title</h1>
                        <p>Div a par</p>
                    </div>
                </div>
            </div>
            <div>
                <div>
                    <div>
                        <h1>Div b title</h1>
                        <p>Div b par</p>
                    </div>
                </div>
            </div>
            """;

        var expectedOriginalDocument = """
            @page "/"
            @namespace MarketApp.Pages.Product.Home
        
            namespace MarketApp.Pages.Product.Home
        
            <PageTitle>Home</PageTitle>
        
            <div id="parent">
                <Component />
            </div>
        
            <h1>Hello, world!</h1>
        
            Welcome to your new app.
            """;

        await TestAsync(
            input,
            expectedOriginalDocument,
            expectedRazorComponent);
    }

    [Fact]
    public async Task Handle_StartNodeContainsEndNode()
    {
        var input = """
            <[|div id="parent">
                <div>
                    <div>
                        <div>
                            <p>Deeply nested par</p|]>
                        </div>
                    </div>
                </div>
            </div>
            """;

        var expectedRazorComponent = """
            <div id="parent">
                <div>
                    <div>
                        <div>
                            <p>Deeply nested par</p>
                        </div>
                    </div>
                </div>
            </div>
            """;

        var expectedOriginalDocument = """
            <Component />
            """;

        await TestAsync(
            input,
            expectedOriginalDocument,
            expectedRazorComponent);
    }

    [Fact]
    public async Task Handle_TextOnlySelection()
    {
        var input = """
            <h1> Hello </h1>

            Welcome to [|your new app|]
            """;

        var expectedRazorComponent = """
            your new app
            """;

        var expectedOriginalDocument = """
            <h1> Hello </h1>

            Welcome to <Component />
            """;

        await TestAsync(
            input,
            expectedOriginalDocument,
            expectedRazorComponent);
    }

    private async Task TestAsync(
        string input,
        string expectedOriginalDocument,
        string? expectedNewComponent)
    {
        TestFileMarkupParser.GetSpan(input, out input, out var textSpan);

        var razorFilePath = "C:/path/to/test.razor";
        var componentFilePath = "C:/path/to/Component.razor";
        var codeDocument = CreateCodeDocument(input, filePath: razorFilePath);
        var sourceText = codeDocument.Source.Text;
        var uri = new Uri(razorFilePath);
        var languageServer = await CreateLanguageServerAsync(codeDocument, razorFilePath);
        var documentContext = CreateDocumentContext(uri, codeDocument);
        var requestContext = new RazorRequestContext(documentContext, null!, "lsp/method", uri: null);

        var result = await GetCodeActionsAsync(
            uri,
            textSpan,
            sourceText,
            requestContext,
            languageServer,
            razorProviders: [new ExtractToComponentCodeActionProvider()],
            null);

        Assert.NotEmpty(result);
        var codeActionToRun = GetCodeActionToRun(ExtractToComponentTitle, 0, result);

        if (expectedNewComponent is null)
        {
            Assert.Null(codeActionToRun);
            return;
        }

        Assert.NotNull(codeActionToRun);

        var resolver = new ExtractToComponentCodeActionResolver(
            TestLanguageServerFeatureOptions.Instance);

        var formattingService = await TestRazorFormattingService.CreateWithFullSupportAsync(LoggerFactory, codeDocument, null);
        var changes = await GetEditsAsync(
            codeActionToRun,
            requestContext,
            languageServer,
            optionsMonitor: null,
            [resolver]
            );

        var edits = changes.Where(change => change.TextDocument.Uri.AbsolutePath == componentFilePath).Single();
        var actual = edits.Edits.Select(edit => edit.NewText).Single();

        AssertEx.EqualOrDiff(expectedNewComponent, actual);

        var originalDocumentEdits = changes
            .Where(change => change.TextDocument.Uri.AbsolutePath == razorFilePath)
            .SelectMany(change => change.Edits.Select(sourceText.GetTextChange));
        var documentText = sourceText.WithChanges(originalDocumentEdits).ToString();
        AssertEx.EqualOrDiff(expectedOriginalDocument, documentText);
    }
}
