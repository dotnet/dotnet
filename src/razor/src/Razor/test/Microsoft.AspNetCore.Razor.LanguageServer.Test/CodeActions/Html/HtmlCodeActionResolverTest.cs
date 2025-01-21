// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.CodeAnalysis.Razor.CodeActions;
using Microsoft.CodeAnalysis.Razor.CodeActions.Models;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.CodeActions;

public class HtmlCodeActionResolverTest(ITestOutputHelper testOutput) : LanguageServerTestBase(testOutput)
{
    [Fact]
    public async Task ResolveAsync_RemapsAndFixesEdits()
    {
        // Arrange
        var contents = "[|<$$h1>Goo @(DateTime.Now) Bar</h1>|]";
        TestFileMarkupParser.GetPositionAndSpan(contents, out contents, out var cursorPosition, out var span);

        var documentPath = "c:/Test.razor";
        var documentUri = new Uri(documentPath);
        var documentContextFactory = CreateDocumentContextFactory(documentUri, contents);
        Assert.True(documentContextFactory.TryCreate(documentUri, out var context));
        var sourceText = await context.GetSourceTextAsync(DisposalToken);
        var remappedEdit = new WorkspaceEdit
        {
            DocumentChanges = new TextDocumentEdit[]
            {
                new() {
                    TextDocument = new OptionalVersionedTextDocumentIdentifier
                    {
                        Uri = documentUri,
                    },
                    Edits = [VsLspFactory.CreateTextEdit(sourceText.GetRange(span), "Goo /*~~~~~~~~~~~*/ Bar")]
                }
           }
        };

        var resolvedCodeAction = new RazorVSInternalCodeAction
        {
            Edit = remappedEdit
        };

        var editMappingServiceMock = new StrictMock<IEditMappingService>();
        editMappingServiceMock
            .Setup(x => x.RemapWorkspaceEditAsync(It.IsAny<IDocumentSnapshot>(), It.IsAny<WorkspaceEdit>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(remappedEdit);

        var resolver = new HtmlCodeActionResolver(editMappingServiceMock.Object);

        var codeAction = new RazorVSInternalCodeAction()
        {
            Name = "Test",
            Edit = new WorkspaceEdit
            {
                DocumentChanges = new TextDocumentEdit[]
                        {
                            new()
                            {
                                TextDocument = new OptionalVersionedTextDocumentIdentifier
                                {
                                    Uri = new Uri("c:/Test.razor.html"),
                                },
                                Edits = [VsLspFactory.CreateTextEdit(position: (0, 0), "Goo")]
                            }
                        }
            }
        };

        // Act
        var action = await resolver.ResolveAsync(context, codeAction, DisposalToken);

        // Assert
        Assert.NotNull(action.Edit);
        Assert.True(action.Edit.TryGetTextDocumentEdits(out var documentEdits));
        Assert.Equal(documentPath, documentEdits[0].TextDocument.Uri.AbsolutePath);
        // Edit should be converted to 2 edits, to remove the tags
        Assert.Collection(documentEdits[0].Edits,
            e =>
            {
                Assert.Equal("", e.NewText);
            },
            e =>
            {
                Assert.Equal("", e.NewText);
            });
    }
}
