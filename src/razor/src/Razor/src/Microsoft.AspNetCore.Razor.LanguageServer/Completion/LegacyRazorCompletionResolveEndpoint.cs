// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.AspNetCore.Razor.LanguageServer.Tooltip;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.Completion;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.Text.Adornments;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Completion;

// The intention of this class is to temporarily exist as a snapshot in time for our pre-existing completion experience.
// It will eventually be removed in favor of the non-Legacy variant at which point we'll also remove the feature flag
// for this legacy version.
internal class LegacyRazorCompletionResolveEndpoint : IVSCompletionResolveEndpoint, IOnInitialized
{
    private readonly ILogger _logger;
    private readonly LSPTagHelperTooltipFactory _lspTagHelperTooltipFactory;
    private readonly VSLSPTagHelperTooltipFactory _vsLspTagHelperTooltipFactory;
    private readonly CompletionListCache _completionListCache;
    private VSInternalCompletionSetting? _completionCapability;
    private VSInternalClientCapabilities? _clientCapabilities;
    private MarkupKind _documentationKind;

    // Guid is magically generated and doesn't mean anything. O# magic.
    public Guid Id => new("011c77cc-f90e-4f2e-b32c-dafc6587ccd6");

    public bool MutatesSolutionState => false;

    public LegacyRazorCompletionResolveEndpoint(
        LSPTagHelperTooltipFactory lspTagHelperTooltipFactory,
        VSLSPTagHelperTooltipFactory vsLspTagHelperTooltipFactory,
        CompletionListCache completionListCache,
        ILoggerFactory loggerFactory)
    {
        if (lspTagHelperTooltipFactory is null)
        {
            throw new ArgumentNullException(nameof(lspTagHelperTooltipFactory));
        }

        if (vsLspTagHelperTooltipFactory is null)
        {
            throw new ArgumentNullException(nameof(vsLspTagHelperTooltipFactory));
        }

        if (completionListCache is null)
        {
            throw new ArgumentNullException(nameof(completionListCache));
        }

        if (loggerFactory is null)
        {
            throw new ArgumentNullException(nameof(loggerFactory));
        }

        _lspTagHelperTooltipFactory = lspTagHelperTooltipFactory;
        _vsLspTagHelperTooltipFactory = vsLspTagHelperTooltipFactory;
        _logger = loggerFactory.CreateLogger<RazorCompletionEndpoint>();
        _completionListCache = completionListCache;
    }

    public Task OnInitializedAsync(VSInternalClientCapabilities clientCapabilities, CancellationToken cancellationToken)
    {
        _completionCapability = clientCapabilities.TextDocument?.Completion as VSInternalCompletionSetting;
        _clientCapabilities = clientCapabilities;

        var completionSupportedKinds = clientCapabilities.TextDocument?.Completion?.CompletionItem?.DocumentationFormat;
        _documentationKind = completionSupportedKinds?.Contains(MarkupKind.Markdown) == true ? MarkupKind.Markdown : MarkupKind.PlainText;

        return Task.CompletedTask;
    }

    public Task<VSInternalCompletionItem> HandleRequestAsync(VSInternalCompletionItem completionItem, RazorRequestContext requestContext, CancellationToken cancellationToken)
    {
        if (!completionItem.TryGetCompletionListResultIds(out var resultIds))
        {
            // Couldn't resolve.
            return Task.FromResult(completionItem);
        }

        var resultId = resultIds.First();

        if (!_completionListCache.TryGet(resultId, out var cachedCompletionItems))
        {
            return Task.FromResult(completionItem);
        }

        if (cachedCompletionItems.Context is not IReadOnlyList<RazorCompletionItem> razorCompletionItems)
        {
            // Can't recognize the original request context, bail.
            return Task.FromResult(completionItem);
        }

        var labelQuery = completionItem.Label;
        var associatedRazorCompletion = razorCompletionItems.FirstOrDefault(completion => string.Equals(labelQuery, completion.DisplayText, StringComparison.Ordinal));
        if (associatedRazorCompletion is null)
        {
            _logger.LogError("Could not find an associated razor completion item. This should never happen since we were able to look up the cached completion list.");
            Debug.Fail("Could not find an associated razor completion item. This should never happen since we were able to look up the cached completion list.");
            return Task.FromResult(completionItem);
        }

        // If the client is VS, also fill in the Description property.
        var useDescriptionProperty = _clientCapabilities?.SupportsVisualStudioExtensions ?? false;

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
                    _lspTagHelperTooltipFactory.TryCreateTooltip(descriptionInfo, _documentationKind, out tagHelperMarkupTooltip);
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
                    _lspTagHelperTooltipFactory.TryCreateTooltip(descriptionInfo, _documentationKind, out tagHelperMarkupTooltip);
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

        return Task.FromResult(completionItem);
    }
}
