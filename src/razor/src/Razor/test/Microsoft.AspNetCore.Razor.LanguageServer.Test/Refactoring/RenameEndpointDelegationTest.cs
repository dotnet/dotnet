// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Rename;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Text;
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

        await using var languageServer = await CreateLanguageServerAsync(codeDocument, razorFilePath);

        var projectManager = CreateProjectSnapshotManager();

        await projectManager.UpdateAsync(updater =>
        {
            updater.AddProject(new(
                filePath: "C:/path/to/project.csproj",
                intermediateOutputPath: "C:/path/to/obj",
                configuration: RazorConfiguration.Default,
                rootNamespace: "project"));
        });

        var searchEngine = new RazorComponentSearchEngine(LoggerFactory);

        var renameService = new RenameService(searchEngine, LanguageServerFeatureOptions);

        var endpoint = new RenameEndpoint(
            renameService,
            LanguageServerFeatureOptions,
            DocumentMappingService,
            EditMappingService,
            projectManager,
            languageServer,
            LoggerFactory);

        var request = new RenameParams
        {
            TextDocument = new TextDocumentIdentifier
            {
                DocumentUri = new(new Uri(razorFilePath))
            },
            Position = codeDocument.Source.Text.GetPosition(cursorPosition),
            NewName = newName
        };
        Assert.True(DocumentContextFactory.TryCreate(request.TextDocument, out var documentContext));
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(request, requestContext, DisposalToken);

        // Assert
        var edits = result.DocumentChanges.Value.First.FirstOrDefault().Edits.Select(e => codeDocument.Source.Text.GetTextChange(((TextEdit)e)));
        var newText = codeDocument.Source.Text.WithChanges(edits).ToString();
        Assert.Equal(expected, newText);
    }
}
