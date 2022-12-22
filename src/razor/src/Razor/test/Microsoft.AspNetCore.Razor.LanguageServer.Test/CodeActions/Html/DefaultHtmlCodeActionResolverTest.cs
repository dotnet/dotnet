// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.CodeActions.Models;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Moq;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.CodeActions;

public class DefaultHtmlCodeActionResolverTest : LanguageServerTestBase
{
    public DefaultHtmlCodeActionResolverTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
    }

    [Fact]
    public async Task ResoplveAsync_RemapsAndFixesEdits()
    {
        // Arrange
        var contents = "[|<$$h1>Goo @(DateTime.Now) Bar</h1>|]";
        TestFileMarkupParser.GetPositionAndSpan(contents, out contents, out var cursorPosition, out var span);

        var documentPath = "c:/Test.razor";
        var documentUri = new Uri(documentPath);
        var documentContextFactory = CreateDocumentContextFactory(documentUri, contents);
        var context = await documentContextFactory.TryCreateAsync(documentUri, DisposalToken);
        Assert.NotNull(context);
        var sourceText = await context.GetSourceTextAsync(DisposalToken);
        var remappedEdit = new WorkspaceEdit
        {
            DocumentChanges = new TextDocumentEdit[]
           {
                new TextDocumentEdit
                {
                    TextDocument = new OptionalVersionedTextDocumentIdentifier { Uri= documentUri, Version = 1 },
                    Edits = new TextEdit[]
                    {
                        new TextEdit { NewText = "Goo ~~~~~~~~~~~~~~~ Bar", Range = span.AsRange(sourceText) }
                    }
                }
           }
        };

        var resolvedCodeAction = new RazorVSInternalCodeAction
        {
            Edit = remappedEdit
        };

        var documentMappingServiceMock = new Mock<RazorDocumentMappingService>(MockBehavior.Strict);
        documentMappingServiceMock
            .Setup(c => c.RemapWorkspaceEditAsync(It.IsAny<WorkspaceEdit>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(remappedEdit);

        var resolver = new DefaultHtmlCodeActionResolver(documentContextFactory, CreateLanguageServer(resolvedCodeAction), documentMappingServiceMock.Object);

        var codeAction = new RazorVSInternalCodeAction()
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
        };

        var codeActionParams = new CodeActionResolveParams()
        {
            Data = new JObject(),
            RazorFileUri = new Uri(documentPath)
        };

        // Act
        var action = await resolver.ResolveAsync(codeActionParams, codeAction, DisposalToken);

        // Assert
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

    private static ClientNotifierServiceBase CreateLanguageServer(CodeAction resolvedCodeAction)
    {
        var response = resolvedCodeAction;

        var languageServer = new Mock<ClientNotifierServiceBase>(MockBehavior.Strict);
        languageServer
            .Setup(l => l.SendRequestAsync<RazorResolveCodeActionParams, CodeAction>(RazorLanguageServerCustomMessageTargets.RazorResolveCodeActionsEndpoint, It.IsAny<RazorResolveCodeActionParams>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        return languageServer.Object;
    }
}
