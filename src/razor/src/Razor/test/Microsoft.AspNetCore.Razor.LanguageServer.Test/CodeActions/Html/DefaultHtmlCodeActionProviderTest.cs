// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.CodeActions.Models;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces.Extensions;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Moq;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.CodeActions;

public class DefaultHtmlCodeActionProviderTest : LanguageServerTestBase
{
    public DefaultHtmlCodeActionProviderTest(ITestOutputHelper testOutput) : base(testOutput)
    {
    }

    [Fact]
    public async Task ProvideAsync_WrapsResolvableCodeActions()
    {
        // Arrange
        var contents = "<$$h1>Goo</h1>";
        TestFileMarkupParser.GetPosition(contents, out contents, out var cursorPosition);

        var documentPath = "c:/Test.razor";
        var request = new CodeActionParams()
        {
            TextDocument = new TextDocumentIdentifier { Uri = new Uri(documentPath) },
            Range = new Range(),
            Context = new CodeActionContext()
        };

        var location = new SourceLocation(cursorPosition, -1, -1);
        var context = CreateRazorCodeActionContext(request, location, documentPath, contents);
        context.CodeDocument.SetFileKind(FileKinds.Legacy);

        var documentMappingService = Mock.Of<RazorDocumentMappingService>(MockBehavior.Strict);
        var provider = new DefaultHtmlCodeActionProvider(documentMappingService);

        var codeActions = new[] { new RazorVSInternalCodeAction() { Name = "Test" } };

        // Act
        var providedCodeActions = await provider.ProvideAsync(context, codeActions, DisposalToken);

        // Assert
        Assert.NotNull(providedCodeActions);
        var action = Assert.Single(providedCodeActions);
        Assert.Equal("Test", action.Name);
        Assert.Equal("Html", ((JObject)action.Data!)["language"]!.ToString());
    }

    [Fact]
    public async Task ProvideAsync_RemapsAndFixesEdits()
    {
        // Arrange
        var contents = "[|<$$h1>Goo @(DateTime.Now) Bar</h1>|]";
        TestFileMarkupParser.GetPositionAndSpan(contents, out contents, out var cursorPosition, out var span);

        var documentPath = "c:/Test.razor";
        var request = new CodeActionParams()
        {
            TextDocument = new TextDocumentIdentifier { Uri = new Uri(documentPath) },
            Range = new Range(),
            Context = new CodeActionContext()
        };

        var location = new SourceLocation(cursorPosition, -1, -1);
        var context = CreateRazorCodeActionContext(request, location, documentPath, contents);
        context.CodeDocument.SetFileKind(FileKinds.Legacy);

        var remappedEdit = new WorkspaceEdit
        {
            DocumentChanges = new TextDocumentEdit[]
            {
                new TextDocumentEdit
                {
                    TextDocument = new OptionalVersionedTextDocumentIdentifier { Uri= new Uri(documentPath), Version = 1 },
                    Edits = new TextEdit[]
                    {
                        new TextEdit { NewText = "Goo ~~~~~~~~~~~~~~~ Bar", Range = span.AsRange(context.SourceText) }
                    }
                }
            }
        };

        var documentMappingServiceMock = new Mock<RazorDocumentMappingService>(MockBehavior.Strict);
        documentMappingServiceMock
            .Setup(c => c.RemapWorkspaceEditAsync(It.IsAny<WorkspaceEdit>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(remappedEdit);

        var provider = new DefaultHtmlCodeActionProvider(documentMappingServiceMock.Object);

        var codeActions = new[]
            {
                new RazorVSInternalCodeAction()
                {
                    Name = "Test",
                    Edit = new WorkspaceEdit
                    {
                        DocumentChanges = new TextDocumentEdit[]
                        {
                            new TextDocumentEdit
                            {
                                TextDocument = new OptionalVersionedTextDocumentIdentifier { Uri= new Uri("c:/Test.razor.html"), Version = 1 },
                                Edits = new TextEdit[]
                                {
                                    new TextEdit { NewText = "Goo" }
                                }
                            }
                        }
                    }
                }
            };

        // Act
        var providedCodeActions = await provider.ProvideAsync(context, codeActions, DisposalToken);

        // Assert
        Assert.NotNull(providedCodeActions);
        var action = Assert.Single(providedCodeActions);
        Assert.NotNull(action.Edit);
        Assert.True(action.Edit.TryGetDocumentChanges(out var changes));
        Assert.Equal(documentPath, changes[0].TextDocument.Uri.AbsolutePath);
        // Edit should be converted to 2 edits, to remove the tags
        Assert.Collection(changes[0].Edits,
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
        CodeActionParams request,
        SourceLocation location,
        string filePath,
        string text,
        bool supportsFileCreation = true,
        bool supportsCodeActionResolve = true)
    {
        var tagHelpers = Array.Empty<TagHelperDescriptor>();
        var sourceDocument = TestRazorSourceDocument.Create(text, filePath: filePath, relativePath: filePath);
        var projectEngine = RazorProjectEngine.Create(builder => builder.AddTagHelpers(tagHelpers));
        var codeDocument = projectEngine.ProcessDesignTime(sourceDocument, FileKinds.Component, Array.Empty<RazorSourceDocument>(), tagHelpers);

        var documentSnapshot = Mock.Of<DocumentSnapshot>(document =>
            document.GetGeneratedOutputAsync() == Task.FromResult(codeDocument) &&
            document.GetTextAsync() == Task.FromResult(codeDocument.GetSourceText()) &&
            document.Project.TagHelpers == tagHelpers, MockBehavior.Strict);

        var sourceText = SourceText.From(text);

        var context = new RazorCodeActionContext(request, documentSnapshot, codeDocument, location, sourceText, supportsFileCreation, supportsCodeActionResolve);

        return context;
    }
}
