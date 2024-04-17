// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.Diagnostics;
using Microsoft.AspNetCore.Razor.Language.Legacy;
using Microsoft.AspNetCore.Razor.Language.Syntax;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.Completion;
using Microsoft.VisualStudio.Editor.Razor;
using RazorSyntaxNode = Microsoft.AspNetCore.Razor.Language.Syntax.SyntaxNode;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Completion;

internal class MarkupTransitionCompletionItemProvider : IRazorCompletionItemProvider
{
    private static readonly ImmutableArray<RazorCommitCharacter> s_elementCommitCharacters = RazorCommitCharacter.CreateArray([">"]);

    private static RazorCompletionItem? s_markupTransitionCompletionItem;
    public static RazorCompletionItem MarkupTransitionCompletionItem
    {
        get
        {
            if (s_markupTransitionCompletionItem is null)
            {
                var completionDisplayText = SyntaxConstants.TextTagName;
                s_markupTransitionCompletionItem = new RazorCompletionItem(
                    completionDisplayText,
                    completionDisplayText,
                    RazorCompletionItemKind.MarkupTransition,
                    commitCharacters: s_elementCommitCharacters);
                var completionDescription = new MarkupTransitionCompletionDescription(CodeAnalysisResources.MarkupTransition_Description);
                s_markupTransitionCompletionItem.SetMarkupTransitionCompletionDescription(completionDescription);
            }

            return s_markupTransitionCompletionItem;
        }
    }

    public ImmutableArray<RazorCompletionItem> GetCompletionItems(RazorCompletionContext context)
    {
        if (context is null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        var owner = context.Owner;
        if (owner is null)
        {
            Debug.Fail("Owner should never be null.");
            return ImmutableArray<RazorCompletionItem>.Empty;
        }

        // If we're at the edge of a razor code block, FindInnermostNode will have returned that edge. However,
        // the cursor, from the user's perspective, may still be on a markup element, so we want to walk back
        // one token.
        if (owner is RazorMetaCodeSyntax { SpanStart: var spanStart, MetaCode: [var metaCodeToken, ..] } && spanStart == context.AbsoluteIndex)
        {
            var previousToken = metaCodeToken.GetPreviousToken();
            owner = previousToken.Parent;
        }

        if (!AtMarkupTransitionCompletionPoint(owner))
        {
            return ImmutableArray<RazorCompletionItem>.Empty;
        }

        // Also helps filter out edge cases like `< te` and `< te=""`
        // (see comment in AtMarkupTransitionCompletionPoint)
        if (!HtmlFacts.TryGetElementInfo(owner, out var containingTagNameToken, out _, closingForwardSlashOrCloseAngleToken: out _) ||
            !containingTagNameToken.Span.IntersectsWith(context.AbsoluteIndex))
        {
            return ImmutableArray<RazorCompletionItem>.Empty;
        }

        return ImmutableArray.Create(MarkupTransitionCompletionItem);
    }

    private static bool AtMarkupTransitionCompletionPoint(RazorSyntaxNode owner)
    {
        /* Only provide IntelliSense for C# code blocks, of the form:
            @{ }, @code{ }, @functions{ }, @if(true){ }

           Note for the `< te` and `< te=""` cases:
           The cases are not handled by AtMarkupTransitionCompletionPoint but
           rather by the HtmlFactsService which purposely prohibits the completion
           when it's unable to extract the tag contents. This ensures we aren't
           providing incorrect completion in the above two syntactically invalid
           scenarios.
        */
        var encapsulatingMarkupElementNodeSeen = false;

        foreach (var ancestor in owner.Ancestors())
        {
            if (ancestor is MarkupElementSyntax markupNode)
            {
                if (encapsulatingMarkupElementNodeSeen)
                {
                    return false;
                }

                encapsulatingMarkupElementNodeSeen = true;
            }

            if (ancestor is CSharpCodeBlockSyntax)
            {
                return true;
            }
        }

        return false;
    }
}
