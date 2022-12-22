// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Razor.Extensions;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.CodeActions.Models;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Moq;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.CodeActions;

public class AddUsingsCSharpCodeActionResolverTest : LanguageServerTestBase
{
    private static readonly CodeAction s_defaultUnresolvedCodeAction = new()
    {
        Title = "@using System.Net"
    };

    public AddUsingsCSharpCodeActionResolverTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
    }

    [Fact]
    public async Task ResolveAsync_ReturnsResolvedCodeAction()
    {
        // Arrange
        var resolvedCodeAction = new CodeAction()
        {
            Title = "@using System.Net",
            Data = null,
            Edit = new WorkspaceEdit()
            {
                DocumentChanges = new TextDocumentEdit[] {
                        new TextDocumentEdit()
                        {
                            Edits = new TextEdit[] {
                                new TextEdit()
                                {
                                    Range = new Range
                                    {
                                        Start = new Position(0, 0),
                                        End = new Position(0, 0)
                                    },
                                    NewText = "using System.Net;"
                                }
                            }
                        }
                    }
            }
        };
        CreateCodeActionResolver(out var codeActionParams, out var csharpCodeActionResolver, resolvedCodeAction);

        // Act
        var returnedCodeAction = await csharpCodeActionResolver.ResolveAsync(codeActionParams, s_defaultUnresolvedCodeAction, default);

        // Assert
        Assert.Equal(resolvedCodeAction.Title, returnedCodeAction.Title);
        Assert.Equal(resolvedCodeAction.Data, returnedCodeAction.Data);

        Assert.NotNull(returnedCodeAction.Edit?.DocumentChanges);
        Assert.Equal(1, returnedCodeAction.Edit.DocumentChanges.Value.Count());
        var returnedEdits = returnedCodeAction.Edit.DocumentChanges.Value.First();
        Assert.True(returnedEdits.TryGetFirst(out var textDocumentEdit));
        var returnedTextDocumentEdit = Assert.Single(textDocumentEdit.Edits);
        Assert.Equal($"@using System.Net;{Environment.NewLine}", returnedTextDocumentEdit.NewText);
    }

    [Fact]
    public async Task ResolveAsync_FragmentedEdit_ReturnsResolvedCodeAction()
    {
        // Arrange
        var resolvedCodeAction = new CodeAction()
        {
            Title = "@using System.Net",
            Data = null,
            Edit = new WorkspaceEdit()
            {
                DocumentChanges = new TextDocumentEdit[] {
                        new TextDocumentEdit()
                        {
                            Edits = new TextEdit[] {
                                new TextEdit()
                                {
                                    Range = new Range
                                    {
                                        // This puts it just after another "using" keyword
                                        Start = new Position(8, 9),
                                        End = new Position(8, 9)
                                    },
                                    NewText = " System.Net;\r\n    using"
                                }
                            }
                        }
                    }
            }
        };
        CreateCodeActionResolver(out var codeActionParams, out var csharpCodeActionResolver, resolvedCodeAction);

        // Act
        var returnedCodeAction = await csharpCodeActionResolver.ResolveAsync(codeActionParams, s_defaultUnresolvedCodeAction, default);

        // Assert
        Assert.Equal(resolvedCodeAction.Title, returnedCodeAction.Title);
        Assert.Equal(resolvedCodeAction.Data, returnedCodeAction.Data);

        Assert.NotNull(returnedCodeAction.Edit?.DocumentChanges);
        Assert.Equal(1, returnedCodeAction.Edit.DocumentChanges.Value.Count());
        var returnedEdits = returnedCodeAction.Edit.DocumentChanges.Value.First();
        Assert.True(returnedEdits.TryGetFirst(out var textDocumentEdit));
        var returnedTextDocumentEdit = Assert.Single(textDocumentEdit.Edits);
        Assert.Equal($"@using System.Net;{Environment.NewLine}", returnedTextDocumentEdit.NewText);
    }

    [Fact]
    public async Task ResolveAsync_GlobalUsing_ReturnsResolvedCodeAction()
    {
        // Arrange
        var resolvedCodeAction = new CodeAction()
        {
            Title = "@using System.Net",
            Data = null,
            Edit = new WorkspaceEdit()
            {
                DocumentChanges = new TextDocumentEdit[] {
                        new TextDocumentEdit()
                        {
                            Edits = new TextEdit[] {
                                new TextEdit()
                                {
                                    Range = new Range
                                    {
                                        Start = new Position(0, 0),
                                        End = new Position(0, 0)
                                    },
                                    NewText = "using global::System.Net;"
                                }
                            }
                        }
                    }
            }
        };
        CreateCodeActionResolver(out var codeActionParams, out var csharpCodeActionResolver, resolvedCodeAction);

        // Act
        var returnedCodeAction = await csharpCodeActionResolver.ResolveAsync(codeActionParams, s_defaultUnresolvedCodeAction, default);

        // Assert
        Assert.Equal(resolvedCodeAction.Title, returnedCodeAction.Title);
        Assert.Equal(resolvedCodeAction.Data, returnedCodeAction.Data);

        Assert.NotNull(returnedCodeAction.Edit?.DocumentChanges);
        Assert.Equal(1, returnedCodeAction.Edit.DocumentChanges.Value.Count());
        var returnedEdits = returnedCodeAction.Edit.DocumentChanges.Value.First();
        Assert.True(returnedEdits.TryGetFirst(out var textDocumentEdit));
        var returnedTextDocumentEdit = Assert.Single(textDocumentEdit.Edits);
        Assert.Equal($"@using global::System.Net;{Environment.NewLine}", returnedTextDocumentEdit.NewText);
    }

    private static void CreateCodeActionResolver(
        out CodeActionResolveParams codeActionParams,
        out AddUsingsCSharpCodeActionResolver addUsingResolver,
        CodeAction resolvedCodeAction)
    {
        var documentUri = new Uri("c:/Test.razor");
        var contents = string.Empty;
        var codeDocument = CreateCodeDocument(contents, documentUri.AbsolutePath);

        codeActionParams = new CodeActionResolveParams()
        {
            Data = new JObject(),
            RazorFileUri = documentUri
        };

        var languageServer = CreateLanguageServer(resolvedCodeAction);

        addUsingResolver = new AddUsingsCSharpCodeActionResolver(
            CreateDocumentContextFactory(documentUri, codeDocument),
            languageServer);
    }

    private static ClientNotifierServiceBase CreateLanguageServer(CodeAction resolvedCodeAction)
    {
        var languageServer = new Mock<ClientNotifierServiceBase>(MockBehavior.Strict);
        languageServer
            .Setup(l => l.SendRequestAsync<RazorResolveCodeActionParams, CodeAction>(RazorLanguageServerCustomMessageTargets.RazorResolveCodeActionsEndpoint, It.IsAny<RazorResolveCodeActionParams>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(resolvedCodeAction);

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
