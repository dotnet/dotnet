// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.CodeActions.Models;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.Common.Extensions;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.CodeAnalysis.Razor.Workspaces.Extensions;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.CodeActions;

/// <summary>
/// Resolves the C# Add Using Code Action by requesting edits from Roslyn
/// and converting them to be Razor compatible.
/// </summary>
internal class AddUsingsCSharpCodeActionResolver : CSharpCodeActionResolver
{
    private readonly DocumentContextFactory _documentContextFactory;

    public AddUsingsCSharpCodeActionResolver(
        DocumentContextFactory documentContextFactory,
        ClientNotifierServiceBase languageServer)
        : base(languageServer)
    {
        _documentContextFactory = documentContextFactory ?? throw new ArgumentNullException(nameof(documentContextFactory));
    }

    public override string Action => LanguageServerConstants.CodeActions.AddUsing;

    public async override Task<CodeAction> ResolveAsync(
        CodeActionResolveParams csharpParams,
        CodeAction codeAction,
        CancellationToken cancellationToken)
    {
        if (csharpParams is null)
        {
            throw new ArgumentNullException(nameof(csharpParams));
        }

        if (codeAction is null)
        {
            throw new ArgumentNullException(nameof(codeAction));
        }

        cancellationToken.ThrowIfCancellationRequested();

        var documentContext = await _documentContextFactory.TryCreateForOpenDocumentAsync(csharpParams.RazorFileUri, cancellationToken).ConfigureAwait(false);
        if (documentContext is null || cancellationToken.IsCancellationRequested)
        {
            return codeAction;
        }

        var resolvedCodeAction = await ResolveCodeActionWithServerAsync(csharpParams.RazorFileUri, documentContext.Version, RazorLanguageKind.CSharp, codeAction, cancellationToken).ConfigureAwait(false);

        // TODO: Move this higher, so it happens on any code action.
        //       For that though, we need a deeper understanding of applying workspace edits to documents, rather than
        //       just picking out the first one because we assume thats where it will be.
        //       Tracked by https://github.com/dotnet/razor-tooling/issues/6159
        if (resolvedCodeAction?.Edit?.TryGetDocumentChanges(out var documentChanges) != true)
        {
            return codeAction;
        }

        if (documentChanges!.Length != 1)
        {
            Debug.Fail("We don't yet support multi-document code actions! If you're seeing this, something about Roslyn changed and we should react.");
            return codeAction;
        }

        var codeDocument = await documentContext.Snapshot.GetGeneratedOutputAsync().ConfigureAwait(false);
        if (codeDocument.IsUnsupported())
        {
            return codeAction;
        }

        var csharpText = codeDocument.GetCSharpSourceText();
        var edits = documentChanges[0].Edits;
        var changes = edits.Select(e => e.AsTextChange(csharpText));
        var changedText = csharpText.WithChanges(changes);

        edits = await AddUsingsCodeActionProviderHelper.GetUsingStatementEditsAsync(codeDocument, csharpText, changedText, cancellationToken).ConfigureAwait(false);

        codeAction.Edit = new WorkspaceEdit
        {
            DocumentChanges = new TextDocumentEdit[]
            {
                    new TextDocumentEdit
                    {
                        TextDocument = new OptionalVersionedTextDocumentIdentifier()
                        {
                            Uri = csharpParams.RazorFileUri,
                            Version = documentContext.Version
                        },
                        Edits = edits
                    }
            }
        };

        return codeAction;
    }
}
