// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Rename;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Refactoring;

public class RenameEndpointDelegationTest(ITestOutputHelper testOutput) : SingleServerDelegatingEndpointTestBase(testOutput)
{
    [Fact]
    public async Task Handle_Rename_SingleServer_CSharpEditsAreMapped()
    {
        var input = """
                <div></div>

                @{
                    var $$myVariable = "Hello";

                    var length = myVariable.Length;
                }
                """;

        var newName = "newVar";

        var expected = """
                <div></div>

                @{
                    var newVar = "Hello";

                    var length = newVar.Length;
                }
                """;

        // Arrange
        TestFileMarkupParser.GetPosition(input, out var output, out var cursorPosition);
        var codeDocument = CreateCodeDocument(output);
        var razorFilePath = "C:/path/to/file.razor";

        var languageServer = await CreateLanguageServerAsync(codeDocument, razorFilePath);

        var projectManager = CreateProjectSnapshotManager();

        await projectManager.UpdateAsync(updater =>
        {
            updater.ProjectAdded(new(
                projectFilePath: "C:/path/to/project.csproj",
                intermediateOutputPath: "C:/path/to/obj",
                razorConfiguration: RazorConfiguration.Default,
                rootNamespace: "project"));
        });

        var searchEngine = new RazorComponentSearchEngine(projectManager, LoggerFactory);

        var renameService = new RenameService(searchEngine, projectManager, LanguageServerFeatureOptions);

        var endpoint = new RenameEndpoint(
            renameService,
            LanguageServerFeatureOptions,
            DocumentMappingService,
            EditMappingService,
            languageServer,
            LoggerFactory);

        var request = new RenameParams
        {
            TextDocument = new TextDocumentIdentifier
            {
                Uri = new Uri(razorFilePath)
            },
            Position = codeDocument.Source.Text.GetPosition(cursorPosition),
            NewName = newName
        };
        Assert.True(DocumentContextFactory.TryCreate(request.TextDocument, out var documentContext));
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(request, requestContext, DisposalToken);

        // Assert
        var edits = result.DocumentChanges.Value.First.FirstOrDefault().Edits.Select(codeDocument.Source.Text.GetTextChange);
        var newText = codeDocument.Source.Text.WithChanges(edits).ToString();
        Assert.Equal(expected, newText);
    }
}
