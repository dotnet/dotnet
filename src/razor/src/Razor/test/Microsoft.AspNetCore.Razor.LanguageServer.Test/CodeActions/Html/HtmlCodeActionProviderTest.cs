// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.CodeAnalysis.Razor.CodeActions;
using Microsoft.CodeAnalysis.Razor.CodeActions.Models;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Protocol.CodeActions;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.CodeActions;

public class HtmlCodeActionProviderTest(ITestOutputHelper testOutput) : LanguageServerTestBase(testOutput)
{
    [Fact]
    public async Task ProvideAsync_WrapsResolvableCodeActions()
    {
        // Arrange
        var contents = "<$$h1>Goo</h1>";
        TestFileMarkupParser.GetPosition(contents, out contents, out var cursorPosition);

        var documentPath = "c:/Test.razor";
        var request = new VSCodeActionParams()
        {
            TextDocument = new VSTextDocumentIdentifier { Uri = new Uri(documentPath) },
            Range = VsLspFactory.DefaultRange,
            Context = new VSInternalCodeActionContext()
        };

        var context = CreateRazorCodeActionContext(request, cursorPosition, documentPath, contents);
        context.CodeDocument.SetFileKind(FileKinds.Legacy);

        var documentMappingService = StrictMock.Of<IEditMappingService>();
        var provider = new HtmlCodeActionProvider(documentMappingService);

        ImmutableArray<RazorVSInternalCodeAction> codeActions = [new RazorVSInternalCodeAction() { Name = "Test" }];

        // Act
        var providedCodeActions = await provider.ProvideAsync(context, codeActions, DisposalToken);

        // Assert
        var action = Assert.Single(providedCodeActions);
        Assert.Equal("Test", action.Name);
        Assert.Equal(RazorLanguageKind.Html, (RazorLanguageKind)((JsonElement)action.Data!).GetProperty("language").GetUInt32());
    }

    [Fact]
    public async Task ProvideAsync_RemapsAndFixesEdits()
    {
        // Arrange
        var contents = "[|<$$h1>Goo @(DateTime.Now) Bar</h1>|]";
        TestFileMarkupParser.GetPositionAndSpan(contents, out contents, out var cursorPosition, out var span);

        var documentPath = "c:/Test.razor";
        var request = new VSCodeActionParams()
        {
            TextDocument = new VSTextDocumentIdentifier { Uri = new Uri(documentPath) },
            Range = VsLspFactory.DefaultRange,
            Context = new VSInternalCodeActionContext()
        };

        var context = CreateRazorCodeActionContext(request, cursorPosition, documentPath, contents);
        context.CodeDocument.SetFileKind(FileKinds.Legacy);

        var remappedEdit = new WorkspaceEdit
        {
            DocumentChanges = new TextDocumentEdit[]
            {
                new() {
                    TextDocument = new OptionalVersionedTextDocumentIdentifier
                    {
                        Uri = new Uri(documentPath),
                    },
                    Edits = [VsLspFactory.CreateTextEdit(context.SourceText.GetRange(span), "Goo /*~~~~~~~~~~~*/ Bar")]
                }
            }
        };

        var editMappingServiceMock = new StrictMock<IEditMappingService>();
        editMappingServiceMock
            .Setup(x => x.RemapWorkspaceEditAsync(It.IsAny<IDocumentSnapshot>(), It.IsAny<WorkspaceEdit>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(remappedEdit);

        var provider = new HtmlCodeActionProvider(editMappingServiceMock.Object);

        ImmutableArray<RazorVSInternalCodeAction> codeActions =
        [
            new RazorVSInternalCodeAction()
            {
                Name = "Test",
                Edit = new WorkspaceEdit
                {
                    DocumentChanges = new TextDocumentEdit[]
                    {
                        new() {
                            TextDocument = new OptionalVersionedTextDocumentIdentifier
                            {
                                Uri = new Uri("c:/Test.razor.html"),
                            },
                            Edits = [VsLspFactory.CreateTextEdit(position: (0, 0), "Goo")]
                        }
                    }
                }
            }
        ];

        // Act
        var providedCodeActions = await provider.ProvideAsync(context, codeActions, DisposalToken);

        // Assert
        var action = Assert.Single(providedCodeActions);
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

    private static RazorCodeActionContext CreateRazorCodeActionContext(
        VSCodeActionParams request,
        int absoluteIndex,
        string filePath,
        string text,
        bool supportsFileCreation = true,
        bool supportsCodeActionResolve = true)
    {
        var tagHelpers = ImmutableArray<TagHelperDescriptor>.Empty;
        var sourceDocument = TestRazorSourceDocument.Create(text, filePath: filePath, relativePath: filePath);
        var projectEngine = RazorProjectEngine.Create(builder =>
        {
            builder.AddTagHelpers(tagHelpers);

            builder.ConfigureParserOptions(builder =>
            {
                builder.UseRoslynTokenizer = true;
            });
        });
        var codeDocument = projectEngine.ProcessDesignTime(sourceDocument, FileKinds.Component, importSources: default, tagHelpers);

        var documentSnapshotMock = new StrictMock<IDocumentSnapshot>();
        documentSnapshotMock
            .Setup(x => x.GetGeneratedOutputAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(codeDocument);
        documentSnapshotMock
            .Setup(x => x.GetTextAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(codeDocument.Source.Text);
        documentSnapshotMock
            .Setup(x => x.Project.GetTagHelpersAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(tagHelpers);

        return new RazorCodeActionContext(
            request,
            documentSnapshotMock.Object,
            codeDocument,
            DelegatedDocumentUri: null,
            StartAbsoluteIndex: absoluteIndex,
            EndAbsoluteIndex: absoluteIndex,
            RazorLanguageKind.Html,
            codeDocument.Source.Text,
            supportsFileCreation,
            supportsCodeActionResolve);

    }
}
