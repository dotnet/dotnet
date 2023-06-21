// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Completion.Delegation;

internal class TextEditResponseRewriter : DelegatedCompletionResponseRewriter
{
    public override int Order => ExecutionBehaviorOrder.ChangesCompletionItems;

    public override async Task<VSInternalCompletionList> RewriteAsync(
        VSInternalCompletionList completionList,
        int hostDocumentIndex,
        DocumentContext hostDocumentContext,
        DelegatedCompletionParams delegatedParameters,
        CancellationToken cancellationToken)
    {
        if (delegatedParameters.ProjectedKind != RazorLanguageKind.CSharp)
        {
            return completionList;
        }

        var sourceText = await hostDocumentContext.GetSourceTextAsync(cancellationToken).ConfigureAwait(false);

        sourceText.GetLineAndOffset(hostDocumentIndex, out var lineNumber, out var characterOffset);
        var hostDocumentPosition = new Position(lineNumber, characterOffset);
        completionList = TranslateTextEdits(hostDocumentPosition, delegatedParameters.ProjectedPosition, completionList);

        if (completionList.ItemDefaults?.EditRange is { } editRange)
        {
            if (editRange.TryGetFirst(out var range))
            {
                completionList.ItemDefaults.EditRange = TranslateRange(hostDocumentPosition, delegatedParameters.ProjectedPosition, range);
            }
            else
            {
                // TO-DO: Handle InsertReplaceEdit type
                // https://github.com/dotnet/razor/issues/8829
                Debug.Fail("Unsupported edit type.");
            }
        }

        return completionList;
    }

    private static VSInternalCompletionList TranslateTextEdits(
        Position hostDocumentPosition,
        Position projectedPosition,
        VSInternalCompletionList completionList)
    {
        // The TextEdit positions returned to us from the C#/HTML language servers are positions correlating to the virtual document.
        // We need to translate these positions to apply to the Razor document instead. Performance is a big concern here, so we want to
        // make the logic as simple as possible, i.e. no asynchronous calls.
        // The current logic takes the approach of assuming the original request's position (Razor doc) correlates directly to the positions
        // returned by the C#/HTML language servers. We use this assumption (+ math) to map from the virtual (projected) doc positions ->
        // Razor doc positions.

        foreach (var item in completionList.Items)
        {
            if (item.TextEdit is { } edit)
            {
                if (edit.TryGetFirst(out var textEdit))
                {
                    var translatedRange = TranslateRange(hostDocumentPosition, projectedPosition, textEdit.Range);
                    textEdit.Range = translatedRange;
                }
                else
                {
                    // TO-DO: Handle InsertReplaceEdit type
                    // https://github.com/dotnet/razor/issues/8829
                    Debug.Fail("Unsupported edit type.");
                }
            }
            else if (item.AdditionalTextEdits is not null)
            {
                // Additional text edits should typically only be provided at resolve time. We don't support them in the normal completion flow.
                item.AdditionalTextEdits = null;
            }
        }

        return completionList;
    }

    private static Range TranslateRange(Position hostDocumentPosition, Position projectedPosition, Range textEditRange)
    {
        var offset = projectedPosition.Character - hostDocumentPosition.Character;

        var editStartPosition = textEditRange.Start;
        var translatedStartPosition = TranslatePosition(offset, hostDocumentPosition, editStartPosition);
        var editEndPosition = textEditRange.End;
        var translatedEndPosition = TranslatePosition(offset, hostDocumentPosition, editEndPosition);
        var translatedRange = new Range()
        {
            Start = translatedStartPosition,
            End = translatedEndPosition,
        };

        return translatedRange;
    }

    private static Position TranslatePosition(int offset, Position hostDocumentPosition, Position editPosition)
    {
        var translatedCharacter = editPosition.Character - offset;

        // Note: If this completion handler ever expands to deal with multi-line TextEdits, this logic will likely need to change since
        // it assumes we're only dealing with single-line TextEdits.
        var translatedPosition = new Position(hostDocumentPosition.Line, translatedCharacter);
        return translatedPosition;
    }
}
