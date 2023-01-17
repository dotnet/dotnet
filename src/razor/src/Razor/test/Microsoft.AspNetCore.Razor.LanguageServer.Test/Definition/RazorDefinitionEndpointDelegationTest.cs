// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.AspNetCore.Razor.LanguageServer.Test;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces.Extensions;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Moq;
using Xunit;
using Xunit.Abstractions;
using DefinitionResult = Microsoft.VisualStudio.LanguageServer.Protocol.SumType<
    Microsoft.VisualStudio.LanguageServer.Protocol.VSInternalLocation,
    Microsoft.VisualStudio.LanguageServer.Protocol.VSInternalLocation[],
    Microsoft.VisualStudio.LanguageServer.Protocol.DocumentLink[]>;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Definition;

public class RazorDefinitionEndpointDelegationTest : SingleServerDelegatingEndpointTestBase
{
    public RazorDefinitionEndpointDelegationTest(ITestOutputHelper testOutput)
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

    private async Task VerifyCSharpGoToDefinitionAsync(string input)
    {
        // Arrange
        TestFileMarkupParser.GetPositionAndSpan(input, out var output, out var cursorPosition, out var expectedSpan);

        var codeDocument = CreateCodeDocument(output);
        var razorFilePath = "C:/path/to/file.razor";

        // Act
        var result = await GetDefinitionResultAsync(codeDocument, razorFilePath, cursorPosition);

        // Assert
        Assert.NotNull(result.Value.Second);
        var locations = result.Value.Second;
        var location = Assert.Single(locations);
        Assert.Equal(new Uri(razorFilePath), location.Uri);

        var expectedRange = expectedSpan.AsRange(codeDocument.GetSourceText());
        Assert.Equal(expectedRange, location.Range);
    }

    private async Task<DefinitionResult?> GetDefinitionResultAsync(RazorCodeDocument codeDocument, string razorFilePath, int cursorPosition)
    {
        await CreateLanguageServerAsync(codeDocument, razorFilePath);

        var projectSnapshotManager = Mock.Of<ProjectSnapshotManagerBase>(p => p.Projects == new[] { Mock.Of<ProjectSnapshot>(MockBehavior.Strict) }, MockBehavior.Strict);
        var projectSnapshotManagerAccessor = new TestProjectSnapshotManagerAccessor(projectSnapshotManager);
        var projectSnapshotManagerDispatcher = new LSPProjectSnapshotManagerDispatcher(LoggerFactory);
        var searchEngine = new DefaultRazorComponentSearchEngine(Dispatcher, projectSnapshotManagerAccessor, LoggerFactory);

        var razorUri = new Uri(razorFilePath);
        var documentContext = await DocumentContextFactory.TryCreateAsync(razorUri, DisposalToken);
        var requestContext = CreateRazorRequestContext(documentContext);

        var endpoint = new RazorDefinitionEndpoint(searchEngine, DocumentMappingService, LanguageServerFeatureOptions, LanguageServer, LoggerFactory);

        codeDocument.GetSourceText().GetLineAndOffset(cursorPosition, out var line, out var offset);
        var request = new TextDocumentPositionParamsBridge
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
