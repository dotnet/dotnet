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
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.CodeActions;

/// <summary>
/// Resolves and remaps the code action, without running formatting passes.
/// </summary>
internal class UnformattedRemappingCSharpCodeActionResolver : CSharpCodeActionResolver
{
    private readonly DocumentContextFactory _documentContextFactory;
    private readonly RazorDocumentMappingService _documentMappingService;

    public UnformattedRemappingCSharpCodeActionResolver(
        DocumentContextFactory documentContextFactory,
        ClientNotifierServiceBase languageServer,
        RazorDocumentMappingService documentMappingService)
        : base(languageServer)
    {
        _documentContextFactory = documentContextFactory ?? throw new ArgumentNullException(nameof(documentContextFactory));
        _documentMappingService = documentMappingService ?? throw new ArgumentNullException(nameof(documentMappingService));
    }

    public override string Action => LanguageServerConstants.CodeActions.UnformattedRemap;

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

        var documentContext = await _documentContextFactory.TryCreateAsync(csharpParams.RazorFileUri, cancellationToken).ConfigureAwait(false);
        if (documentContext is null)
        {
            return codeAction;
        }

        var resolvedCodeAction = await ResolveCodeActionWithServerAsync(csharpParams.RazorFileUri, documentContext.Version, RazorLanguageKind.CSharp, codeAction, cancellationToken).ConfigureAwait(false);
        if (resolvedCodeAction?.Edit?.DocumentChanges is null)
        {
            // Unable to resolve code action with server, return original code action
            return codeAction;
        }

        if (resolvedCodeAction.Edit.DocumentChanges.Value.Count() != 1)
        {
            // We don't yet support multi-document code actions, return original code action
            Debug.Fail($"Encountered an unsupported multi-document code action edit with ${codeAction.Title}.");
            return codeAction;
        }

        var documentChanged = resolvedCodeAction.Edit.DocumentChanges.Value.First();
        if (!documentChanged.TryGetFirst(out var textDocumentEdit))
        {
            // Only Text Document Edit changes are supported currently, return original code action
            return codeAction;
        }

        var textEdit = textDocumentEdit.Edits.FirstOrDefault();
        if (textEdit is null)
        {
            // No text edit available
            return codeAction;
        }

        var codeDocument = await documentContext.Snapshot.GetGeneratedOutputAsync().ConfigureAwait(false);
        if (codeDocument.IsUnsupported())
        {
            return codeAction;
        }

        if (!_documentMappingService.TryMapFromProjectedDocumentRange(codeDocument, textEdit.Range, MappingBehavior.Inclusive, out var originalRange))
        {
            // Text edit failed to map
            return codeAction;
        }

        textEdit.Range = originalRange;

        var codeDocumentIdentifier = new OptionalVersionedTextDocumentIdentifier()
        {
            Uri = csharpParams.RazorFileUri,
            Version = documentContext.Version,
        };
        resolvedCodeAction.Edit = new WorkspaceEdit()
        {
            DocumentChanges = new[] {
                        new TextDocumentEdit()
                        {
                            TextDocument = codeDocumentIdentifier,
                            Edits = new[] { textEdit },
                        }
                },
        };

        return resolvedCodeAction;
    }
}
