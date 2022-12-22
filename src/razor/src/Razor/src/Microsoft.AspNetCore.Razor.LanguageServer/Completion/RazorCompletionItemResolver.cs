// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Tooltip;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.Completion;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.Text.Adornments;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Completion;

internal class RazorCompletionItemResolver : CompletionItemResolver
{
    private readonly LSPTagHelperTooltipFactory _lspTagHelperTooltipFactory;
    private readonly VSLSPTagHelperTooltipFactory _vsLspTagHelperTooltipFactory;

    public RazorCompletionItemResolver(
        LSPTagHelperTooltipFactory lspTagHelperTooltipFactory,
        VSLSPTagHelperTooltipFactory vsLspTagHelperTooltipFactory)
    {
        _lspTagHelperTooltipFactory = lspTagHelperTooltipFactory;
        _vsLspTagHelperTooltipFactory = vsLspTagHelperTooltipFactory;
    }

    public override Task<VSInternalCompletionItem?> ResolveAsync(
        VSInternalCompletionItem completionItem,
        VSInternalCompletionList containingCompletionList,
        object? originalRequestContext,
        VSInternalClientCapabilities? clientCapabilities,
        CancellationToken cancellationToken)
    {
        if (originalRequestContext is not IReadOnlyList<RazorCompletionItem> razorCompletionItems)
        {
            // Can't recognize the original request context, bail.
            return Task.FromResult<VSInternalCompletionItem?>(null);
        }

        var associatedRazorCompletion = razorCompletionItems.FirstOrDefault(completion => string.Equals(completion.DisplayText, completionItem.Label, StringComparison.Ordinal));
        if (associatedRazorCompletion is null)
        {
            return Task.FromResult<VSInternalCompletionItem?>(null);
        }

        // If the client is VS, also fill in the Description property.
        var useDescriptionProperty = clientCapabilities?.SupportsVisualStudioExtensions ?? false;
        var completionSupportedKinds = clientCapabilities?.TextDocument?.Completion?.CompletionItem?.DocumentationFormat;
        var documentationKind = completionSupportedKinds?.Contains(MarkupKind.Markdown) == true ? MarkupKind.Markdown : MarkupKind.PlainText;

        MarkupContent? tagHelperMarkupTooltip = null;
        ClassifiedTextElement? tagHelperClassifiedTextTooltip = null;

        switch (associatedRazorCompletion.Kind)
        {
            case RazorCompletionItemKind.Directive:
            {
                var descriptionInfo = associatedRazorCompletion.GetDirectiveCompletionDescription();
                if (descriptionInfo is not null)
                {
                    completionItem.Documentation = descriptionInfo.Description;
                }

                break;
            }
            case RazorCompletionItemKind.MarkupTransition:
            {
                var descriptionInfo = associatedRazorCompletion.GetMarkupTransitionCompletionDescription();
                if (descriptionInfo is not null)
                {
                    completionItem.Documentation = descriptionInfo.Description;
                }

                break;
            }
            case RazorCompletionItemKind.DirectiveAttribute:
            case RazorCompletionItemKind.DirectiveAttributeParameter:
            case RazorCompletionItemKind.TagHelperAttribute:
            {
                var descriptionInfo = associatedRazorCompletion.GetAttributeCompletionDescription();
                if (descriptionInfo == null)
                {
                    break;
                }

                if (useDescriptionProperty)
                {
                    _vsLspTagHelperTooltipFactory.TryCreateTooltip(descriptionInfo, out tagHelperClassifiedTextTooltip);
                }
                else
                {
                    _lspTagHelperTooltipFactory.TryCreateTooltip(descriptionInfo, documentationKind, out tagHelperMarkupTooltip);
                }

                break;
            }
            case RazorCompletionItemKind.TagHelperElement:
            {
                var descriptionInfo = associatedRazorCompletion.GetTagHelperElementDescriptionInfo();
                if (descriptionInfo == null)
                {
                    break;
                }

                if (useDescriptionProperty)
                {
                    _vsLspTagHelperTooltipFactory.TryCreateTooltip(descriptionInfo, out tagHelperClassifiedTextTooltip);
                }
                else
                {
                    _lspTagHelperTooltipFactory.TryCreateTooltip(descriptionInfo, documentationKind, out tagHelperMarkupTooltip);
                }

                break;
            }
        }

        if (tagHelperMarkupTooltip != null)
        {
            completionItem.Documentation = tagHelperMarkupTooltip;
        }

        if (tagHelperClassifiedTextTooltip != null)
        {
            completionItem.Description = tagHelperClassifiedTextTooltip;
        }

        return Task.FromResult<VSInternalCompletionItem?>(completionItem);
    }
}
