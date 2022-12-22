// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Razor.Extensions;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.CodeActions.Models;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.AspNetCore.Razor.LanguageServer.Formatting;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Moq;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.CodeActions;

public class DefaultCSharpCodeActionResolverTest : LanguageServerTestBase
{
    private static readonly CodeAction s_defaultResolvedCodeAction = new()
    {
        Title = "ResolvedCodeAction",
        Data = JToken.FromObject(new object()),
        Edit = new WorkspaceEdit()
        {
            DocumentChanges = new TextDocumentEdit[] {
                new TextDocumentEdit()
                {
                    Edits = new TextEdit[]{
                        new TextEdit()
                        {
                            NewText = "Generated C# Based Edit"
                        }
                    }
                }
            }
        }
    };

    private static readonly TextEdit[] s_defaultFormattedEdits = new TextEdit[]
    {
        new TextEdit()
        {
            NewText = "Remapped & Formatted Edit"
        }
    };

    private static readonly CodeAction s_defaultUnresolvedCodeAction = new CodeAction()
    {
        Title = "Unresolved Code Action"
    };

    public DefaultCSharpCodeActionResolverTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
    }

    [Fact]
    public async Task ResolveAsync_ReturnsResolvedCodeAction()
    {
        // Arrange
        CreateCodeActionResolver(out var codeActionParams, out var csharpCodeActionResolver);

        // Act
        var returnedCodeAction = await csharpCodeActionResolver.ResolveAsync(codeActionParams, s_defaultUnresolvedCodeAction, default);

        // Assert
        Assert.Equal(s_defaultResolvedCodeAction.Title, returnedCodeAction.Title);
        Assert.Equal(s_defaultResolvedCodeAction.Data, returnedCodeAction.Data);
        Assert.NotNull(returnedCodeAction.Edit?.DocumentChanges);
        Assert.Equal(1, returnedCodeAction.Edit.DocumentChanges.Value.Count());
        var returnedEdits = returnedCodeAction.Edit.DocumentChanges.Value;
        Assert.True(returnedEdits.TryGetFirst(out var textDocumentEdits));
        var returnedTextDocumentEdit = Assert.Single(textDocumentEdits[0].Edits);
        Assert.Equal(s_defaultFormattedEdits.First(), returnedTextDocumentEdit);
    }

    [Fact]
    public async Task ResolveAsync_NoDocumentChanges_ReturnsOriginalCodeAction()
    {
        // Arrange
        var resolvedCodeAction = new CodeAction()
        {
            Title = "ResolvedCodeAction",
            Data = JToken.FromObject(new object()),
            Edit = new WorkspaceEdit()
            {
                DocumentChanges = null
            }
        };

        var languageServer = CreateLanguageServer(resolvedCodeAction);

        CreateCodeActionResolver(out var codeActionParams, out var csharpCodeActionResolver, languageServer: languageServer);

        // Act
        var returnedCodeAction = await csharpCodeActionResolver.ResolveAsync(codeActionParams, s_defaultUnresolvedCodeAction, default);

        // Assert
        Assert.Equal(s_defaultUnresolvedCodeAction.Title, returnedCodeAction.Title);
    }

    [Fact]
    public async Task ResolveAsync_MultipleDocumentChanges_ReturnsOriginalCodeAction()
    {
        // Arrange
        var resolvedCodeAction = new CodeAction()
        {
            Title = "ResolvedCodeAction",
            Data = JToken.FromObject(new object()),
            Edit = new WorkspaceEdit()
            {
                DocumentChanges = new TextDocumentEdit[] {
                        new TextDocumentEdit()
                        {
                            Edits = new TextEdit[] {
                                new TextEdit()
                                {
                                    NewText = "1. Generated C# Based Edit"
                                }
                            }
                        },
                        new TextDocumentEdit()
                        {
                            Edits = new TextEdit[] {
                                new TextEdit()
                                {
                                    NewText = "2. Generated C# Based Edit"
                                }
                            }
                        }
                    }
            }
        };

        var languageServer = CreateLanguageServer(resolvedCodeAction);

        CreateCodeActionResolver(out var codeActionParams, out var csharpCodeActionResolver, languageServer: languageServer);

        // Act
        var returnedCodeAction = await csharpCodeActionResolver.ResolveAsync(codeActionParams, s_defaultUnresolvedCodeAction, default);

        // Assert
        Assert.Equal(s_defaultUnresolvedCodeAction.Title, returnedCodeAction.Title);
    }

    [Fact]
    public async Task ResolveAsync_NonTextDocumentEdit_ReturnsOriginalCodeAction()
    {
        // Arrange
        var resolvedCodeAction = new CodeAction()
        {
            Title = "ResolvedCodeAction",
            Data = JToken.FromObject(new object()),
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

        var languageServer = CreateLanguageServer(resolvedCodeAction);

        CreateCodeActionResolver(out var codeActionParams, out var csharpCodeActionResolver, languageServer: languageServer);

        // Act
        var returnedCodeAction = await csharpCodeActionResolver.ResolveAsync(codeActionParams, s_defaultUnresolvedCodeAction, default);

        // Assert
        Assert.Equal(s_defaultUnresolvedCodeAction.Title, returnedCodeAction.Title);
    }

    private static void CreateCodeActionResolver(
        out CodeActionResolveParams codeActionParams,
        out DefaultCSharpCodeActionResolver csharpCodeActionResolver,
        ClientNotifierServiceBase? languageServer = null,
        RazorFormattingService? razorFormattingService = null)
    {
        var documentPath = "c:/Test.razor";
        var documentUri = new Uri(documentPath);
        var contents = string.Empty;
        var codeDocument = CreateCodeDocument(contents, documentPath);

        codeActionParams = new CodeActionResolveParams()
        {
            Data = new JObject(),
            RazorFileUri = documentUri
        };

        languageServer ??= CreateLanguageServer();
        razorFormattingService ??= CreateRazorFormattingService(documentUri);

        csharpCodeActionResolver = new DefaultCSharpCodeActionResolver(
            CreateDocumentContextFactory(documentUri, codeDocument),
            languageServer,
            razorFormattingService);
    }

    private static RazorFormattingService CreateRazorFormattingService(Uri documentUri)
    {
        var razorFormattingService = Mock.Of<RazorFormattingService>(
                        rfs => rfs.FormatCodeActionAsync(
                            It.Is<DocumentContext>(c => c.Uri == documentUri),
                            RazorLanguageKind.CSharp,
                            It.IsAny<TextEdit[]>(),
                            It.IsAny<FormattingOptions>(),
                            It.IsAny<CancellationToken>()) == Task.FromResult(s_defaultFormattedEdits), MockBehavior.Strict);
        return razorFormattingService;
    }

    private static ClientNotifierServiceBase CreateLanguageServer(CodeAction? resolvedCodeAction = null)
    {
        var response = resolvedCodeAction ?? s_defaultResolvedCodeAction;

        var languageServer = new Mock<ClientNotifierServiceBase>(MockBehavior.Strict);
        languageServer
            .Setup(l => l.SendRequestAsync<RazorResolveCodeActionParams, CodeAction>(RazorLanguageServerCustomMessageTargets.RazorResolveCodeActionsEndpoint, It.IsAny<RazorResolveCodeActionParams>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        return languageServer.Object;
    }

    private static RazorCodeDocument CreateCodeDocument(string text, string documentPath)
    {
        var projectItem = new TestRazorProjectItem(documentPath) { Content = text };
        var projectEngine = RazorProjectEngine.Create(RazorConfiguration.Default, TestRazorProjectFileSystem.Empty, (builder) => PageDirective.Register(builder));
        var codeDocument = projectEngine.Process(projectItem);
        codeDocument.SetFileKind(FileKinds.Component);
        return codeDocument;
    }
}
