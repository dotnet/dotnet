// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis.Razor.CodeActions.Models;
using Microsoft.CodeAnalysis.Razor.Formatting;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.CodeAnalysis.Razor.CodeActions;

internal class WrapAttributesCodeActionResolver : IRazorCodeActionResolver
{
    public string Action => LanguageServerConstants.CodeActions.WrapAttributes;

    public async Task<WorkspaceEdit?> ResolveAsync(DocumentContext documentContext, JsonElement data, RazorFormattingOptions options, CancellationToken cancellationToken)
    {
        var actionParams = data.Deserialize<WrapAttributesCodeActionParams>();
        if (actionParams is null)
        {
            return null;
        }

        var indentationString = FormattingUtilities.GetIndentationString(actionParams.IndentSize, options.InsertSpaces, options.TabSize);
        var sourceText = await documentContext.GetSourceTextAsync(cancellationToken).ConfigureAwait(false);
        using var edits = new PooledArrayBuilder<TextEdit>();

        foreach (var position in actionParams.NewLinePositions)
        {
            var start = sourceText.GetLinePosition(FindPreviousNonWhitespacePosition(sourceText, position) + 1);
            var end = sourceText.GetLinePosition(position);
            edits.Add(VsLspFactory.CreateTextEdit(start, end, Environment.NewLine + indentationString));
        }

        var tde = new TextDocumentEdit
        {
            TextDocument = new OptionalVersionedTextDocumentIdentifier() { Uri = documentContext.Uri },
            Edits = edits.ToArray()
        };

        return new WorkspaceEdit
        {
            DocumentChanges = new SumType<TextDocumentEdit[], SumType<TextDocumentEdit, CreateFile, RenameFile, DeleteFile>[]>([tde])
        };
    }

    private int FindPreviousNonWhitespacePosition(SourceText sourceText, int position)
    {
        for (var i = position - 1; i >= 0; i--)
        {
            if (!char.IsWhiteSpace(sourceText[i]))
            {
                return i;
            }
        }

        return 0;
    }
}
