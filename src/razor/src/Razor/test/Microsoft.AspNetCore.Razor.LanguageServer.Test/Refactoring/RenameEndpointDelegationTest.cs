// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.AspNetCore.Razor.LanguageServer.Test;
using Microsoft.AspNetCore.Razor.Test.Common.Mef;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces.Extensions;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Refactoring;

[UseExportProvider]
public class RenameEndpointDelegationTest: SingleServerDelegatingEndpointTestBase
{
    public RenameEndpointDelegationTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
    }

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

        await CreateLanguageServerAsync(codeDocument, razorFilePath);

        var projectSnapshotManager = Mock.Of<ProjectSnapshotManagerBase>(p => p.GetProjects() == new[] { Mock.Of<IProjectSnapshot>(MockBehavior.Strict) }.ToImmutableArray(), MockBehavior.Strict);
        var projectSnapshotManagerAccessor = new TestProjectSnapshotManagerAccessor(projectSnapshotManager);
        using var projectSnapshotManagerDispatcher = new LSPProjectSnapshotManagerDispatcher(LoggerFactory);
        var searchEngine = new DefaultRazorComponentSearchEngine(projectSnapshotManagerAccessor, LoggerFactory);

        var endpoint = new RenameEndpoint(
            projectSnapshotManagerDispatcher,
            DocumentContextFactory,
            searchEngine,
            projectSnapshotManagerAccessor,
            LanguageServerFeatureOptions,
            DocumentMappingService,
            LanguageServer,
            LoggerFactory);

        codeDocument.GetSourceText().GetLineAndOffset(cursorPosition, out var line, out var offset);
        var request = new RenameParams
        {
            TextDocument = new TextDocumentIdentifier
            {
                Uri = new Uri(razorFilePath)
            },
            Position = new Position(line, offset),
            NewName = newName
        };
        var documentContext = DocumentContextFactory.TryCreateForOpenDocument(request.TextDocument);
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(request, requestContext, DisposalToken);

        // Assert
        var edits = result.DocumentChanges.Value.First.FirstOrDefault().Edits.Select(e => e.ToTextChange(codeDocument.GetSourceText()));
        var newText = codeDocument.GetSourceText().WithChanges(edits).ToString();
        Assert.Equal(expected, newText);
    }
}
