// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.AspNetCore.Razor.LanguageServer.Test;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces.Extensions;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Moq;
using Xunit;
using Xunit.Abstractions;
using DefinitionResult = Microsoft.VisualStudio.LanguageServer.Protocol.SumType<
    Microsoft.VisualStudio.LanguageServer.Protocol.VSInternalLocation,
    Microsoft.VisualStudio.LanguageServer.Protocol.VSInternalLocation[],
    Microsoft.VisualStudio.LanguageServer.Protocol.DocumentLink[]>;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Definition;

public class DefinitionEndpointDelegationTest : SingleServerDelegatingEndpointTestBase
{
    public DefinitionEndpointDelegationTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
    }

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
        Assert.NotNull(result.Value.Second);
        var locations = result.Value.Second;
        var location = Assert.Single(locations);
        Assert.EndsWith("String.cs", location.Uri.ToString());
        Assert.Equal(21, location.Range.Start.Line);
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
        Assert.NotNull(result.Value.Second);
        var locations = result.Value.Second;
        var location = Assert.Single(locations);

        // Our tests don't currently support mapping multiple documents, so we just need to verify Roslyn sent back the right info.
        // Other tests verify mapping behavior
        Assert.EndsWith("SurveyPrompt.razor.ide.g.cs", location.Uri.ToString());

        // We can still expect the character to be correct, even if the line won't match
        var surveyPromptSourceText = SourceText.From(surveyPrompt);
        var range = expectedSpan.ToRange(surveyPromptSourceText);
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
        Assert.NotNull(result.Value.Second);
        var locations = result.Value.Second;
        var location = Assert.Single(locations);
        Assert.Equal(new Uri(razorFilePath), location.Uri);

        var expectedRange = expectedSpan.ToRange(codeDocument.GetSourceText());
        Assert.Equal(expectedRange, location.Range);
    }

    private async Task<DefinitionResult?> GetDefinitionResultAsync(RazorCodeDocument codeDocument, string razorFilePath, int cursorPosition, IEnumerable<(string filePath, string contents)>? additionalRazorDocuments = null)
    {
        await CreateLanguageServerAsync(codeDocument, razorFilePath, additionalRazorDocuments);

        var projectSnapshotManager = Mock.Of<ProjectSnapshotManagerBase>(p => p.GetProjects() == new[] { Mock.Of<IProjectSnapshot>(MockBehavior.Strict) }.ToImmutableArray(), MockBehavior.Strict);
        var projectSnapshotManagerAccessor = new TestProjectSnapshotManagerAccessor(projectSnapshotManager);
        var projectSnapshotManagerDispatcher = new LSPProjectSnapshotManagerDispatcher(LoggerFactory);
        var searchEngine = new DefaultRazorComponentSearchEngine(projectSnapshotManagerAccessor, LoggerFactory);

        var razorUri = new Uri(razorFilePath);
        var documentContext = DocumentContextFactory.TryCreateForOpenDocument(razorUri);
        var requestContext = CreateRazorRequestContext(documentContext);

        var endpoint = new DefinitionEndpoint(searchEngine, DocumentMappingService, LanguageServerFeatureOptions, LanguageServer, LoggerFactory);

        codeDocument.GetSourceText().GetLineAndOffset(cursorPosition, out var line, out var offset);
        var request = new TextDocumentPositionParams
        {
            TextDocument = new TextDocumentIdentifier
            {
                Uri = new Uri(razorFilePath)
            },
            Position = new Position(line, offset)
        };

        return await endpoint.HandleRequestAsync(request, requestContext, DisposalToken);
    }
}
