// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Text;
using Xunit;
using Xunit.Abstractions;
using DefinitionResult = Roslyn.LanguageServer.Protocol.SumType<
    Roslyn.LanguageServer.Protocol.Location,
    Roslyn.LanguageServer.Protocol.VSInternalLocation,
    Roslyn.LanguageServer.Protocol.VSInternalLocation[],
    Roslyn.LanguageServer.Protocol.DocumentLink[]>;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Definition;

public class DefinitionEndpointDelegationTest(ITestOutputHelper testOutput) : SingleServerDelegatingEndpointTestBase(testOutput)
{
    [Fact]
    public async Task Handle_SingleServer_CSharp_Method()
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

        await VerifyCSharpGoToDefinitionAsync(input);
    }

    [Fact]
    public async Task Handle_SingleServer_CSharp_Local()
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

        await VerifyCSharpGoToDefinitionAsync(input);
    }

    [Fact]
    public async Task Handle_SingleServer_CSharp_MetadataReference()
    {
        var input = """
            <div></div>
            @functions
            {
                private stri$$ng _name;
            }
            """;

        // Arrange
        TestFileMarkupParser.GetPosition(input, out var output, out var cursorPosition);

        var codeDocument = CreateCodeDocument(output);
        var razorFilePath = "C:/path/to/file.razor";

        // Act
        var result = await GetDefinitionResultAsync(codeDocument, razorFilePath, cursorPosition);

        // Assert
        Assert.NotNull(result.Value.Third);
        var locations = result.Value.Third;
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
    public async Task Handle_SingleServer_Attribute_SameFile(string method)
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

        await VerifyCSharpGoToDefinitionAsync(input, "test.razor");
    }

    [Fact]
    public async Task Handle_SingleServer_AttributeValue_BindAfter()
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

        await VerifyCSharpGoToDefinitionAsync(input, "test.razor");
    }

    [Theory]
    [InlineData("Ti$$tle")]
    [InlineData("$$@bind-Title")]
    [InlineData("@$$bind-Title")]
    [InlineData("@bi$$nd-Title")]
    [InlineData("@bind$$-Title")]
    [InlineData("@bind-Ti$$tle")]
    public async Task Handle_SingleServer_ComponentAttribute_OtherRazorFile(string attribute)
    {
        var input = $$"""
            <SurveyPrompt {{attribute}}="InputValue" />

            @code
            {
                private string? InputValue { get; set; }

                private void BindAfter()
                {
                }
            }
            """;

        // Need to put this in the right namespace, to match the tag helper defined in our test json
        var surveyPrompt = """
            @namespace BlazorApp1.Shared

            <div></div>

            @code
            {
                [Parameter]
                public string [|Title|] { get; set; }
            }
            """;

        TestFileMarkupParser.GetSpan(surveyPrompt, out surveyPrompt, out var expectedSpan);
        var additionalRazorDocuments = new[]
        {
            ("SurveyPrompt.razor", surveyPrompt)
        };

        // Arrange
        TestFileMarkupParser.GetPosition(input, out var output, out var cursorPosition);

        var codeDocument = CreateCodeDocument(output, filePath: "Test.razor");
        var razorFilePath = "C:/path/to/file.razor";

        // Act
        var result = await GetDefinitionResultAsync(codeDocument, razorFilePath, cursorPosition, additionalRazorDocuments);

        // Assert
        Assert.NotNull(result.Value.Third);
        var locations = result.Value.Third;
        var location = Assert.Single(locations);

        // Our tests don't currently support mapping multiple documents, so we just need to verify Roslyn sent back the right info.
        // Other tests verify mapping behavior
        Assert.EndsWith("SurveyPrompt.razor.ide.g.cs", location.DocumentUri.UriString);

        // We can still expect the character to be correct, even if the line won't match
        var surveyPromptSourceText = SourceText.From(surveyPrompt);
        var range = surveyPromptSourceText.GetRange(expectedSpan);
        Assert.Equal(range.Start.Character, location.Range.Start.Character);
    }

    private async Task VerifyCSharpGoToDefinitionAsync(string input, string? filePath = null)
    {
        // Arrange
        TestFileMarkupParser.GetPositionAndSpan(input, out var output, out var cursorPosition, out var expectedSpan);

        var codeDocument = CreateCodeDocument(output, filePath: filePath);
        var razorFilePath = "C:/path/to/file.razor";

        // Act
        var result = await GetDefinitionResultAsync(codeDocument, razorFilePath, cursorPosition);

        // Assert
        Assert.NotNull(result.Value.Third);
        var locations = result.Value.Third;
        var location = Assert.Single(locations);
        Assert.Equal(new Uri(razorFilePath), location.DocumentUri.GetRequiredParsedUri());

        var expectedRange = codeDocument.Source.Text.GetRange(expectedSpan);
        Assert.Equal(expectedRange, location.Range);
    }

    private async Task<DefinitionResult?> GetDefinitionResultAsync(RazorCodeDocument codeDocument, string razorFilePath, int cursorPosition, IEnumerable<(string filePath, string contents)>? additionalRazorDocuments = null)
    {
        await using var languageServer = await CreateLanguageServerAsync(codeDocument, razorFilePath, additionalRazorDocuments);

        var projectManager = CreateProjectSnapshotManager();

        await projectManager.UpdateAsync(updater =>
        {
            updater.AddProject(new(
                filePath: "C:/path/to/project.csproj",
                intermediateOutputPath: "C:/path/to/obj",
                configuration: RazorConfiguration.Default,
                rootNamespace: "project"));
        });

        var componentSearchEngine = new RazorComponentSearchEngine(LoggerFactory);
        var componentDefinitionService = new RazorComponentDefinitionService(componentSearchEngine, DocumentMappingService, LoggerFactory);

        var razorUri = new Uri(razorFilePath);
        Assert.True(DocumentContextFactory.TryCreate(razorUri, out var documentContext));
        var requestContext = CreateRazorRequestContext(documentContext);

        var endpoint = new DefinitionEndpoint(componentDefinitionService, DocumentMappingService, projectManager, LanguageServerFeatureOptions, languageServer, LoggerFactory);

        var request = new TextDocumentPositionParams
        {
            TextDocument = new TextDocumentIdentifier
            {
                DocumentUri = new(new Uri(razorFilePath))
            },
            Position = codeDocument.Source.Text.GetPosition(cursorPosition)
        };

        return await endpoint.HandleRequestAsync(request, requestContext, DisposalToken);
    }
}
