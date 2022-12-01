// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using RazorSyntaxNode = Microsoft.AspNetCore.Razor.Language.Syntax.SyntaxNode;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Completion.Delegation;

internal class DesignTimeHelperResponseRewriter : DelegatedCompletionResponseRewriter
{
    private static readonly IReadOnlyList<string> s_designTimeHelpers = new string[]
    {
        "__builder",
        "__o",
        "__RazorDirectiveTokenHelpers__",
        "__tagHelperExecutionContext",
        "__tagHelperRunner",
        "__typeHelper",
        "_Imports",
        "BuildRenderTree"
    };

    private static readonly IReadOnlyList<CompletionItem> s_designTimeHelpersCompletionItems =
        s_designTimeHelpers
            .Select(item => new CompletionItem { Label = item })
            .ToArray();

    public override int Order => ExecutionBehaviorOrder.FiltersCompletionItems;

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

        var syntaxTree = await hostDocumentContext.GetSyntaxTreeAsync(cancellationToken).ConfigureAwait(false);
        var owner = syntaxTree.GetOwner(hostDocumentIndex);
        if (owner is null)
        {
            Debug.Fail("Owner should never be null.");
            return completionList;
        }

        var sourceText = await hostDocumentContext.GetSourceTextAsync(cancellationToken).ConfigureAwait(false);

        // We should remove Razor design time helpers from C#'s completion list. If the current identifier being targeted does not start with a double
        // underscore, we trim out all items starting with "__" from the completion list. If the current identifier does start with a double underscore
        // (e.g. "__ab[||]"), we only trim out common design time helpers from the completion list.

        var filteredItems = completionList.Items.Except(s_designTimeHelpersCompletionItems, CompletionItemComparer.Instance).ToArray();

        if (ShouldRemoveAllDesignTimeItems(owner, sourceText))
        {
            filteredItems = filteredItems.Where(item => item.Label != null && !item.Label.StartsWith("__", StringComparison.Ordinal)).ToArray();
        }

        completionList.Items = filteredItems;
        return completionList;
    }

    // If the current identifier starts with "__", only trim out common design time helpers from the list.
    // In all other cases, trim out both common design time helpers and all completion items starting with "__".
    private static bool ShouldRemoveAllDesignTimeItems(RazorSyntaxNode owner, SourceText sourceText)
    {
        if (owner.Span.Length < 2)
        {
            return true;
        }

        if (sourceText[owner.Span.Start] == '_' && sourceText[owner.Span.Start + 1] == '_')
        {
            return false;
        }

        return true;
    }

    private class CompletionItemComparer : IEqualityComparer<CompletionItem>
    {
        public static CompletionItemComparer Instance = new();

        public bool Equals(CompletionItem? x, CompletionItem? y)
        {
            if (x is null)
            {
                return y is null;
            }
            else if (y is null)
            {
                return false;
            }

            return x.Label == y.Label;
        }

        public int GetHashCode(CompletionItem obj)
            => obj?.Label?.GetHashCode() ?? 0;
    }
}
