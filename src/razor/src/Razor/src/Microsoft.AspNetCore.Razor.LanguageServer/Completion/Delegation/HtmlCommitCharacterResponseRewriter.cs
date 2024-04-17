// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Razor.Workspaces.Protocol;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Completion.Delegation;

internal class HtmlCommitCharacterResponseRewriter(RazorLSPOptionsMonitor razorLSPOptionsMonitor) : DelegatedCompletionResponseRewriter
{
    private readonly RazorLSPOptionsMonitor _razorLSPOptionsMonitor = razorLSPOptionsMonitor;

    public override int Order => ExecutionBehaviorOrder.ChangesCompletionItems;

    public override Task<VSInternalCompletionList> RewriteAsync(VSInternalCompletionList completionList, int hostDocumentIndex, DocumentContext hostDocumentContext, DelegatedCompletionParams delegatedParameters, CancellationToken cancellationToken)
    {
        if (delegatedParameters.ProjectedKind != RazorLanguageKind.Html)
        {
            return Task.FromResult(completionList);
        }

        if (_razorLSPOptionsMonitor.CurrentValue.CommitElementsWithSpace)
        {
            return Task.FromResult(completionList);
        }

        string[]? itemCommitChars = null;
        if (completionList.CommitCharacters is { } commitCharacters)
        {
            if (commitCharacters.TryGetFirst(out var commitChars))
            {
                itemCommitChars = commitChars.Where(s => s != " ").ToArray();

                // If the default commit characters didn't include " " already, then we set our list to null to avoid over-specifying commit characters
                if (itemCommitChars.Length == commitChars.Length)
                {
                    itemCommitChars = null;
                }
            }
            else if (commitCharacters.TryGetSecond(out var vsCommitChars))
            {
                itemCommitChars = vsCommitChars.Where(s => s.Character != " ").Select(s => s.Character).ToArray();

                // If the default commit characters didn't include " " already, then we set our list to null to avoid over-specifying commit characters
                if (itemCommitChars.Length == vsCommitChars.Length)
                {
                    itemCommitChars = null;
                }
            }
        }

        foreach (var item in completionList.Items)
        {
            if (item.Kind == CompletionItemKind.Element)
            {
                if (item.CommitCharacters is null)
                {
                    if (itemCommitChars is not null)
                    {
                        // This item wants to use the default commit characters, so change it to our updated version of them, without the space
                        item.CommitCharacters = itemCommitChars;
                    }
                }
                else
                {
                    // This item has its own commit characters, so just remove spaces
                    item.CommitCharacters = item.CommitCharacters?.Where(s => s != " ").ToArray();
                }
            }
        }

        return Task.FromResult(completionList);
    }
}
