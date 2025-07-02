// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Text;
using Xunit;
using Xunit.Abstractions;
using TextDocument = Microsoft.CodeAnalysis.TextDocument;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

public class CohostGoToDefinitionEndpointTest(ITestOutputHelper testOutputHelper) : CohostEndpointTestBase(testOutputHelper)
{
    [Fact]
    public async Task CSharp_Method()
    {
        var input = """
            <div></div>
            @{
                var x = Ge$$tX();
            }
            @functions
            {
                void [|GetX|]()
                {
                }
            }
            """;

        await VerifyGoToDefinitionAsync(input);
    }

    [Fact]
    public async Task CSharp_Local()
    {
        var input = """
            <div></div>
            @{
                var x = GetX();
            }
            @functions
            {
                private string [|_name|];
                string GetX()
                {
                    return _na$$me;
                }
            }
            """;

        await VerifyGoToDefinitionAsync(input);
    }

    [Fact]
    public async Task CSharp_MetadataReference()
    {
        var input = """
            <div></div>
            @functions
            {
                private stri$$ng _name;
            }
            """;

        var result = await GetGoToDefinitionResultAsync(input);

        Assert.NotNull(result.Value.Second);
        var locations = result.Value.Second;
        var location = Assert.Single(locations);
        Assert.EndsWith("String.cs", location.DocumentUri.UriString);

        // Note: The location is in a generated C# "metadata-as-source" file, which has a different
        // number of using directives in .NET Framework vs. .NET Core, so rather than relying on line
        // numbers we do some vague notion of actual navigation and test the actual source line that
        // the user would see.
        var line = File.ReadLines(location.DocumentUri.GetRequiredParsedUri().LocalPath).ElementAt(location.Range.Start.Line);
        Assert.Contains("public sealed class String", line);
    }

    [Theory]
    [InlineData("$$IncrementCount")]
    [InlineData("In$$crementCount")]
    [InlineData("IncrementCount$$")]
    public async Task Attribute_SameFile(string method)
    {
        var input = $$"""
            <button @onclick="{{method}}"></div>

            @code
            {
                void [|IncrementCount|]()
                {
                }
            }
            """;

        await VerifyGoToDefinitionAsync(input, RazorFileKind.Component);
    }

    [Fact]
    public async Task AttributeValue_BindAfter()
    {
        var input = """
            <input type="text" @bind="InputValue" @bind:after="() => Af$$ter()">

            @code
            {
                public string InputValue { get; set; }

                public void [|After|]()
                {
                }
            }
            """;

        await VerifyGoToDefinitionAsync(input, RazorFileKind.Component);
    }

    [Fact]
    public async Task Component()
    {
        TestCode input = """
            <Surv$$eyPrompt Title="InputValue" />
            """;

        TestCode surveyPrompt = """
            [||]@namespace SomeProject

            <div></div>

            @code
            {
                [Parameter]
                public string Title { get; set; }
            }
            """;

        var result = await GetGoToDefinitionResultAsync(input, RazorFileKind.Component,
            (FileName("SurveyPrompt.razor"), surveyPrompt.Text));

        Assert.NotNull(result.Value.Second);
        var locations = result.Value.Second;
        var location = Assert.Single(locations);

        var text = SourceText.From(surveyPrompt.Text);
        var range = text.GetRange(surveyPrompt.Span);
        Assert.Equal(range, location.Range);
    }

    [Theory]
    [InlineData("Ti$$tle")]
    [InlineData("$$@bind-Title")]
    [InlineData("@$$bind-Title")]
    [InlineData("@bi$$nd-Title")]
    [InlineData("@bind$$-Title")]
    [InlineData("@bind-Ti$$tle")]
    public async Task OtherRazorFile(string attribute)
    {
        TestCode input = $$"""
            <SurveyPrompt {{attribute}}="InputValue" />

            @code
            {
                private string? InputValue { get; set; }

                private void BindAfter()
                {
                }
            }
            """;

        TestCode surveyPrompt = """
            @namespace SomeProject

            <div></div>

            @code
            {
                [Parameter]
                public string [|Title|] { get; set; }
            }
            """;

        var result = await GetGoToDefinitionResultAsync(input, RazorFileKind.Component,
            (FileName("SurveyPrompt.razor"), surveyPrompt.Text));

        Assert.NotNull(result.Value.Second);
        var locations = result.Value.Second;
        var location = Assert.Single(locations);

        var text = SourceText.From(surveyPrompt.Text);
        var range = text.GetRange(surveyPrompt.Span);
        Assert.Equal(range, location.Range);
    }

    [Fact]
    public async Task Html()
    {
        // This really just validates Uri remapping, the actual response is largely arbitrary

        TestCode input = """
            <div></div>

            <script>
                function [|foo|]() {
                    f$$oo();
                }
            </script>
            """;

        var document = CreateProjectAndRazorDocument(input.Text);
        var inputText = await document.GetTextAsync(DisposalToken);

        var htmlResponse = new SumType<LspLocation, LspLocation[], DocumentLink[]>?(new LspLocation[]
        {
            new() {
                DocumentUri = new(new Uri(document.CreateUri(), document.Name + FeatureOptions.HtmlVirtualDocumentSuffix)),
                Range = inputText.GetRange(input.Span),
            },
        });

        await VerifyGoToDefinitionAsync(input, htmlResponse: htmlResponse);
    }

    private static string FileName(string projectRelativeFileName)
        => Path.Combine(TestProjectData.SomeProjectPath, projectRelativeFileName);

    private async Task VerifyGoToDefinitionAsync(
        TestCode input,
        RazorFileKind? fileKind = null,
        SumType<LspLocation, LspLocation[], DocumentLink[]>? htmlResponse = null)
    {
        var document = CreateProjectAndRazorDocument(input.Text, fileKind);
        var result = await GetGoToDefinitionResultCoreAsync(document, input, htmlResponse);

        Assumes.NotNull(result);

        Assert.NotNull(result.Value.Second);
        var locations = result.Value.Second;
        var location = Assert.Single(locations);

        var text = SourceText.From(input.Text);
        var range = text.GetRange(input.Span);
        Assert.Equal(range, location.Range);

        Assert.Equal(document.CreateUri(), location.DocumentUri.GetRequiredParsedUri());
    }

    private async Task<SumType<LspLocation, LspLocation[], DocumentLink[]>?> GetGoToDefinitionResultAsync(
        TestCode input,
        RazorFileKind? fileKind = null,
        params (string fileName, string contents)[]? additionalFiles)
    {
        var document = CreateProjectAndRazorDocument(input.Text, fileKind, additionalFiles);
        return await GetGoToDefinitionResultCoreAsync(document, input, htmlResponse: null);
    }

    private async Task<SumType<LspLocation, LspLocation[], DocumentLink[]>?> GetGoToDefinitionResultCoreAsync(
        TextDocument document, TestCode input, SumType<LspLocation, LspLocation[], DocumentLink[]>? htmlResponse)
    {
        var inputText = await document.GetTextAsync(DisposalToken);
        var position = inputText.GetPosition(input.Position);

        var requestInvoker = new TestHtmlRequestInvoker([(Methods.TextDocumentDefinitionName, htmlResponse)]);

        var filePathService = new VisualStudioFilePathService(FeatureOptions);
        var endpoint = new CohostGoToDefinitionEndpoint(IncompatibleProjectService, RemoteServiceInvoker, requestInvoker, filePathService);

        var textDocumentPositionParams = new TextDocumentPositionParams
        {
            Position = position,
            TextDocument = new TextDocumentIdentifier { DocumentUri = document.CreateDocumentUri() },
        };

        return await endpoint.GetTestAccessor().HandleRequestAsync(textDocumentPositionParams, document, DisposalToken);
    }
}
