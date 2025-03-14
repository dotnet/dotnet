// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.CodeAnalysis.Razor.Completion.Delegation;

/// <summary>
/// Modifies delegated snippet completion items
/// </summary>
/// <remarks>
/// At the moment primarily used to remove the C# "using" snippet because we have our own
/// </remarks>
internal class SnippetResponseRewriter : IDelegatedCSharpCompletionResponseRewriter
{
    public VSInternalCompletionList Rewrite(
        VSInternalCompletionList completionList,
        RazorCodeDocument codeDocument,
        int hostDocumentIndex,
        Position projectedPosition,
        RazorCompletionOptions completionOptionsn)
    {
        using var items = new PooledArrayBuilder<CompletionItem>(completionList.Items.Length);

        foreach (var item in completionList.Items)
        {
            if (item is { Kind: CompletionItemKind.Snippet, Label: "using" })
            {
                continue;
            }

            items.Add(item);
        }

        // If we didn't remove anything, then don't bother materializing the array
        if (completionList.Items.Length != items.Count)
        {
            completionList.Items = items.ToArray();
        }

        return completionList;
    }
}
