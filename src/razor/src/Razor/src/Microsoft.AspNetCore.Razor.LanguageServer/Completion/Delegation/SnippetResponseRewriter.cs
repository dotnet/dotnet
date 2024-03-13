// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Frozen;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces.Protocol;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Completion.Delegation;

/// <summary>
/// Modifies delegated snippet completion items
/// </summary>
/// <remarks>
/// At the moment primarily used to modify C# "using" snippet to "using statement" snippet
/// in order to disambiguate it from Razor "using directive" snippet
/// </remarks>
internal class SnippetResponseRewriter : DelegatedCompletionResponseRewriter
{
    private static readonly FrozenDictionary<string, (string Label, string SortText)> s_snippetToCompletionData = new Dictionary<string, (string Label, string SortText)>()
    {
        // Modifying label of the C# using snippet to "using statement" to disambiguate from
        // Razor @using directive, and also appending a space to sort text to make sure it's sorted
        // after Razor "using" keyword and "using directive ..." entries (which use "using" as sort text)
        ["using"] = (Label:$"using {SR.Statement}", SortText:"using ")
    }
    .ToFrozenDictionary();

    public override int Order => ExecutionBehaviorOrder.ChangesCompletionItems;

    public override Task<VSInternalCompletionList> RewriteAsync(VSInternalCompletionList completionList, int hostDocumentIndex, DocumentContext hostDocumentContext, DelegatedCompletionParams delegatedParameters, CancellationToken cancellationToken)
    {
        foreach (var item in completionList.Items)
        {
            if (item.Kind == CompletionItemKind.Snippet)
            {
                if (item.Label is null)
                {
                    continue;
                }

                if (s_snippetToCompletionData.TryGetValue(item.Label, out var completionData))
                {
                    item.Label = completionData.Label;
                    item.SortText = completionData.SortText;
                }
            }
        }

        return Task.FromResult(completionList);
    }
}
