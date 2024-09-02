// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.CodeActions.Models;
using Microsoft.AspNetCore.Razor.LanguageServer.Formatting;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.CodeAnalysis.Razor.Formatting;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.CodeActions;

internal sealed class DefaultCSharpCodeActionResolver(
    IDocumentContextFactory documentContextFactory,
    IClientConnection clientConnection,
    IRazorFormattingService razorFormattingService) : CSharpCodeActionResolver(clientConnection)
{
    private readonly IDocumentContextFactory _documentContextFactory = documentContextFactory;
    private readonly IRazorFormattingService _razorFormattingService = razorFormattingService;

    public override string Action => LanguageServerConstants.CodeActions.Default;

    public async override Task<CodeAction> ResolveAsync(
        CodeActionResolveParams csharpParams,
        CodeAction codeAction,
        CancellationToken cancellationToken)
    {
        if (!_documentContextFactory.TryCreate(csharpParams.RazorFileIdentifier, out var documentContext))
        {
            return codeAction;
        }

        var resolvedCodeAction = await ResolveCodeActionWithServerAsync(csharpParams.RazorFileIdentifier, documentContext.Snapshot.Version, RazorLanguageKind.CSharp, codeAction, cancellationToken).ConfigureAwait(false);
        if (resolvedCodeAction?.Edit?.DocumentChanges is null)
        {
            // Unable to resolve code action with server, return original code action
            return codeAction;
        }

        if (resolvedCodeAction.Edit.DocumentChanges.Value.Count() != 1)
        {
            // We don't yet support multi-document code actions, return original code action
            return codeAction;
        }

        cancellationToken.ThrowIfCancellationRequested();

        var documentChanged = resolvedCodeAction.Edit.DocumentChanges.Value.First();
        if (!documentChanged.TryGetFirst(out var textDocumentEdit))
        {
            // Only Text Document Edit changes are supported currently, return original code action
            return codeAction;
        }

        cancellationToken.ThrowIfCancellationRequested();

        var csharpTextEdits = textDocumentEdit.Edits;

        // Remaps the text edits from the generated C# to the razor file,
        // as well as applying appropriate formatting.
        var formattedEdit = await _razorFormattingService.GetCSharpCodeActionEditAsync(
            documentContext,
            csharpTextEdits,
            RazorFormattingOptions.Default,
            cancellationToken).ConfigureAwait(false);

        cancellationToken.ThrowIfCancellationRequested();

        var codeDocumentIdentifier = new OptionalVersionedTextDocumentIdentifier()
        {
            Uri = csharpParams.RazorFileIdentifier.Uri
        };
        resolvedCodeAction.Edit = new WorkspaceEdit()
        {
            DocumentChanges = new TextDocumentEdit[] {
                new TextDocumentEdit()
                {
                    TextDocument = codeDocumentIdentifier,
                    Edits = formattedEdit is null ? [] : [formattedEdit],
                }
            }
        };

        return resolvedCodeAction;
    }
}
