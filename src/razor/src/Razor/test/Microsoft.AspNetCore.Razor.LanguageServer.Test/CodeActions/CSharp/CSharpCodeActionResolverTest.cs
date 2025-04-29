// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Razor.Extensions;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.CodeAnalysis.Razor.CodeActions;
using Microsoft.CodeAnalysis.Razor.Formatting;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.CodeActions;

public class CSharpCodeActionResolverTest(ITestOutputHelper testOutput) : LanguageServerTestBase(testOutput)
{
    private static readonly CodeAction s_defaultResolvedCodeAction = new()
    {
        Title = "ResolvedCodeAction",
        Data = JsonSerializer.SerializeToElement(new object()),
        Edit = new WorkspaceEdit()
        {
            DocumentChanges = new TextDocumentEdit[] {
                new()
                {
                    Edits = [VsLspFactory.CreateTextEdit(position: (0, 0), "Generated C# Based Edit")]
                }
            }
        }
    };

    private static readonly TextEdit s_defaultFormattedEdit = VsLspFactory.CreateTextEdit(position: (0, 0), "Remapped & Formatted Edit");
    private static readonly TextChange s_defaultFormattedChange = new TextChange(new TextSpan(0, 0), s_defaultFormattedEdit.NewText);

    [Fact]
    public async Task ResolveAsync_ReturnsResolvedCodeAction()
    {
        // Arrange
        CreateCodeActionResolver(out var csharpCodeActionResolver, out var documentContext);

        // Act
        var returnedCodeAction = await csharpCodeActionResolver.ResolveAsync(documentContext, s_defaultResolvedCodeAction, DisposalToken);

        // Assert
        Assert.Equal(s_defaultResolvedCodeAction.Title, returnedCodeAction.Title);
        Assert.Equal(s_defaultResolvedCodeAction.Data, returnedCodeAction.Data);
        Assert.NotNull(returnedCodeAction.Edit?.DocumentChanges);
        Assert.Equal(1, returnedCodeAction.Edit.DocumentChanges.Value.Count());
        var returnedEdits = returnedCodeAction.Edit.DocumentChanges.Value;
        Assert.True(returnedEdits.TryGetFirst(out var textDocumentEdits));
        var returnedTextDocumentEdit = Assert.Single(textDocumentEdits[0].Edits);
        Assert.Equal(s_defaultFormattedEdit.NewText, returnedTextDocumentEdit.NewText);
        Assert.Equal(s_defaultFormattedEdit.Range, returnedTextDocumentEdit.Range);
    }

    [Fact]
    public async Task ResolveAsync_NoDocumentChanges_ReturnsOriginalCodeAction()
    {
        // Arrange
        var codeAction = new CodeAction()
        {
            Title = "ResolvedCodeAction",
            Data = JsonSerializer.SerializeToElement(new object()),
            Edit = new WorkspaceEdit()
            {
                DocumentChanges = null
            }
        };

        CreateCodeActionResolver(out var csharpCodeActionResolver, out var documentContext);

        // Act
        var returnedCodeAction = await csharpCodeActionResolver.ResolveAsync(documentContext, codeAction, DisposalToken);

        // Assert
        Assert.Same(codeAction, returnedCodeAction);
    }

    [Fact]
    public async Task ResolveAsync_MultipleDocumentChanges_ReturnsOriginalCodeAction()
    {
        // Arrange
        var codeAction = new CodeAction()
        {
            Title = "CodeAction",
            Data = JsonSerializer.SerializeToElement(new object()),
            Edit = new WorkspaceEdit()
            {
                DocumentChanges = new TextDocumentEdit[]
                {
                    new TextDocumentEdit()
                    {
                        Edits = [VsLspFactory.CreateTextEdit(position: (0, 0), "1. Generated C# Based Edit")]
                    },
                    new TextDocumentEdit()
                    {
                        Edits = [VsLspFactory.CreateTextEdit(position: (0, 0), "2. Generated C# Based Edit")]
                    }
                }
            }
        };

        CreateCodeActionResolver(out var csharpCodeActionResolver, out var documentContext);

        // Act
        var returnedCodeAction = await csharpCodeActionResolver.ResolveAsync(documentContext, codeAction, DisposalToken);

        // Assert
        Assert.Same(codeAction, returnedCodeAction);
    }

    [Fact]
    public async Task ResolveAsync_NonTextDocumentEdit_ReturnsOriginalCodeAction()
    {
        // Arrange
        var codeAction = new CodeAction()
        {
            Title = "ResolvedCodeAction",
            Data = JsonSerializer.SerializeToElement(new object()),
            Edit = new WorkspaceEdit()
            {
                DocumentChanges = new SumType<TextDocumentEdit, CreateFile, RenameFile, DeleteFile>[] {
                        new CreateFile()
                        {
                            Uri = new Uri("c:/some/uri.razor")
                        }
                    }
            }
        };

        CreateCodeActionResolver(out var csharpCodeActionResolver, out var documentContext);

        // Act
        var returnedCodeAction = await csharpCodeActionResolver.ResolveAsync(documentContext, codeAction, DisposalToken);

        // Assert
        Assert.Same(codeAction, returnedCodeAction);
    }

    private static void CreateCodeActionResolver(
        out CSharpCodeActionResolver csharpCodeActionResolver,
        out DocumentContext documentContext,
        IRazorFormattingService? razorFormattingService = null)
    {
        var documentPath = "c:/Test.razor";
        var documentUri = new Uri(documentPath);
        var contents = string.Empty;
        var codeDocument = CreateCodeDocument(contents, documentPath);

        razorFormattingService ??= CreateRazorFormattingService(documentUri);

        csharpCodeActionResolver = new CSharpCodeActionResolver(
            razorFormattingService);

        documentContext = CreateDocumentContext(documentUri, codeDocument);
    }

    private static IRazorFormattingService CreateRazorFormattingService(Uri documentUri)
    {
        var razorFormattingService = Mock.Of<IRazorFormattingService>(
                        rfs => rfs.TryGetCSharpCodeActionEditAsync(
                            It.Is<DocumentContext>(c => c.Uri == documentUri),
                            It.IsAny<ImmutableArray<TextChange>>(),
                            It.IsAny<RazorFormattingOptions>(),
                            It.IsAny<CancellationToken>()) == Task.FromResult<TextChange?>(s_defaultFormattedChange), MockBehavior.Strict);
        return razorFormattingService;
    }

    private static RazorCodeDocument CreateCodeDocument(string text, string documentPath)
    {
        var projectItem = new TestRazorProjectItem(
            filePath: documentPath,
            fileKind: FileKinds.Component)
        {
            Content = text
        };

        var projectEngine = RazorProjectEngine.Create(RazorConfiguration.Default, TestRazorProjectFileSystem.Empty, builder =>
        {
            PageDirective.Register(builder);
        });

        return projectEngine.Process(projectItem);
    }
}
