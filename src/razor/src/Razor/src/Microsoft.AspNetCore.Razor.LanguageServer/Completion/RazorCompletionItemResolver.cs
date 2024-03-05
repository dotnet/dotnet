// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
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

    public override async Task<VSInternalCompletionItem?> ResolveAsync(
        VSInternalCompletionItem completionItem,
        VSInternalCompletionList containingCompletionList,
        object? originalRequestContext,
        VSInternalClientCapabilities? clientCapabilities,
        CancellationToken cancellationToken)
    {
        if (originalRequestContext is not RazorCompletionResolveContext razorCompletionResolveContext)
        {
            // Can't recognize the original request context, bail.
            return null;
        }

        var associatedRazorCompletion = razorCompletionResolveContext.CompletionItems.FirstOrDefault(completion =>
        {
            if (completion.DisplayText != completionItem.Label)
            {
                return false;
            }

            // We may have items of different types with the same label (e.g. snippet and keyword)
            if (clientCapabilities is not null)
            {
                // CompletionItem.Kind and RazorCompletionItem.Kind are not compatible/comparable, so we need to convert
                // Razor completion item to VS completion item (as logic to convert just the kind is not easy to separate from
                // the rest of the conversion logic) prior to comparing them
                if (RazorCompletionListProvider.TryConvert(completion, clientCapabilities, out var convertedRazorCompletionItem))
                {
                    return completionItem.Kind == convertedRazorCompletionItem.Kind;
                }
            }

            // If display text matches but we couldn't convert razor completion item to VS completion item for some reason,
            // do what previous version of the code did and return true.
            return true;
        });
        if (associatedRazorCompletion is null)
        {
            return null;
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
                        tagHelperClassifiedTextTooltip = await _vsLspTagHelperTooltipFactory.TryCreateTooltipAsync(razorCompletionResolveContext.FilePath, descriptionInfo, cancellationToken).ConfigureAwait(false);
                    }
                    else
                    {
                        tagHelperMarkupTooltip = await _lspTagHelperTooltipFactory.TryCreateTooltipAsync(razorCompletionResolveContext.FilePath, descriptionInfo, documentationKind, cancellationToken).ConfigureAwait(false);
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

        return completionItem;
    }
}
