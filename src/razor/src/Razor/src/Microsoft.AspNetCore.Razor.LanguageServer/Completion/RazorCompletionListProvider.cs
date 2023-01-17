// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Legacy;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.CodeAnalysis.Razor.Completion;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Completion;

internal class RazorCompletionListProvider
{
    private readonly RazorCompletionFactsService _completionFactsService;
    private readonly CompletionListCache _completionListCache;
    private readonly ILogger<RazorCompletionListProvider> _logger;
    private static readonly Command s_retriggerCompletionCommand = new()
    {
        CommandIdentifier = "editor.action.triggerSuggest",
        Title = SR.ReTrigger_Completions_Title,
    };

    public RazorCompletionListProvider(
        RazorCompletionFactsService completionFactsService,
        CompletionListCache completionListCache,
        ILoggerFactory loggerFactory)
    {
        _completionFactsService = completionFactsService;
        _completionListCache = completionListCache;
        _logger = loggerFactory.CreateLogger<RazorCompletionListProvider>();
    }

    // virtual for tests
    public virtual ImmutableHashSet<string> TriggerCharacters => new[] { "@", "<", ":", " " }.ToImmutableHashSet();

    // virtual for tests
    public virtual async Task<VSInternalCompletionList?> GetCompletionListAsync(
        int absoluteIndex,
        VSInternalCompletionContext completionContext,
        DocumentContext documentContext,
        VSInternalClientCapabilities clientCapabilities,
        HashSet<string>? existingCompletions,
        CancellationToken cancellationToken)
    {
        if (!IsApplicableTriggerContext(completionContext))
        {
            return null;
        }

        var reason = completionContext.TriggerKind switch
        {
            CompletionTriggerKind.TriggerForIncompleteCompletions => CompletionReason.Invoked,
            CompletionTriggerKind.Invoked => CompletionReason.Invoked,
            CompletionTriggerKind.TriggerCharacter => CompletionReason.Typing,
            _ => CompletionReason.Typing,
        };
        var completionOptions = new RazorCompletionOptions(SnippetsSupported: true);
        var syntaxTree = await documentContext.GetSyntaxTreeAsync(cancellationToken).ConfigureAwait(false);
        var tagHelperContext = await documentContext.GetTagHelperContextAsync(cancellationToken).ConfigureAwait(false);
        var queryableChange = new SourceChange(absoluteIndex, length: 0, newText: string.Empty);
        var owner = syntaxTree.Root.LocateOwner(queryableChange);
        var razorCompletionContext = new RazorCompletionContext(
            absoluteIndex,
            owner,
            syntaxTree,
            tagHelperContext,
            reason,
            completionOptions,
            existingCompletions);

        var razorCompletionItems = _completionFactsService.GetCompletionItems(razorCompletionContext);

        _logger.LogTrace("Resolved {razorCompletionItemsCount} completion items.", razorCompletionItems.Count);

        var completionList = CreateLSPCompletionList(razorCompletionItems, clientCapabilities);

        var completionCapability = clientCapabilities?.TextDocument?.Completion as VSInternalCompletionSetting;

        // The completion list is cached and can be retrieved via this result id to enable the resolve completion functionality.
        var resultId = _completionListCache.Add(completionList, razorCompletionItems);
        completionList.SetResultId(resultId, completionCapability);

        return completionList;
    }

    // Internal for benchmarking and testing
    internal static VSInternalCompletionList CreateLSPCompletionList(
        IReadOnlyList<RazorCompletionItem> razorCompletionItems,
        VSInternalClientCapabilities clientCapabilities)
    {
        var completionItems = new List<CompletionItem>();
        foreach (var razorCompletionItem in razorCompletionItems)
        {
            if (TryConvert(razorCompletionItem, clientCapabilities, out var completionItem))
            {
                completionItems.Add(completionItem);
            }
        }

        var completionList = new VSInternalCompletionList()
        {
            Items = completionItems.ToArray(),
            IsIncomplete = false,
        };

        var completionCapability = clientCapabilities.TextDocument?.Completion as VSInternalCompletionSetting;
        var optimizedCompletionList = CompletionListOptimizer.Optimize(completionList, completionCapability);
        return optimizedCompletionList;
    }

    // Internal for testing
    internal static bool TryConvert(
        RazorCompletionItem razorCompletionItem,
        VSInternalClientCapabilities clientCapabilities,
        [NotNullWhen(true)] out VSInternalCompletionItem? completionItem)
    {
        if (razorCompletionItem is null)
        {
            throw new ArgumentNullException(nameof(razorCompletionItem));
        }

        var tagHelperCompletionItemKind = CompletionItemKind.TypeParameter;
        var supportedItemKinds = clientCapabilities.TextDocument?.Completion?.CompletionItemKind?.ValueSet ?? Array.Empty<CompletionItemKind>();
        if (supportedItemKinds?.Contains(CompletionItemKind.TagHelper) == true)
        {
            tagHelperCompletionItemKind = CompletionItemKind.TagHelper;
        }

        var insertTextFormat = razorCompletionItem.IsSnippet ? InsertTextFormat.Snippet : InsertTextFormat.Plaintext;

        switch (razorCompletionItem.Kind)
        {
            case RazorCompletionItemKind.Directive:
            {
                var directiveCompletionItem = new VSInternalCompletionItem()
                {
                    Label = razorCompletionItem.DisplayText,
                    InsertText = razorCompletionItem.InsertText,
                    FilterText = razorCompletionItem.DisplayText,
                    SortText = razorCompletionItem.SortText,
                    InsertTextFormat = insertTextFormat,
                    Kind = razorCompletionItem.IsSnippet ? CompletionItemKind.Snippet : CompletionItemKind.Struct, // TODO: Make separate CompletionItemKind for razor directives. See https://github.com/dotnet/razor-tooling/issues/6504 and https://github.com/dotnet/razor-tooling/issues/6505
                };

                directiveCompletionItem.UseCommitCharactersFrom(razorCompletionItem, clientCapabilities);

                if (razorCompletionItem == DirectiveAttributeTransitionCompletionItemProvider.TransitionCompletionItem)
                {
                    directiveCompletionItem.Command = s_retriggerCompletionCommand;
                    directiveCompletionItem.Kind = tagHelperCompletionItemKind;
                }

                completionItem = directiveCompletionItem;
                return true;
            }
            case RazorCompletionItemKind.DirectiveAttribute:
            {
                var directiveAttributeCompletionItem = new VSInternalCompletionItem()
                {
                    Label = razorCompletionItem.DisplayText,
                    InsertText = razorCompletionItem.InsertText,
                    FilterText = razorCompletionItem.InsertText,
                    SortText = razorCompletionItem.SortText,
                    InsertTextFormat = insertTextFormat,
                    Kind = tagHelperCompletionItemKind,
                };

                directiveAttributeCompletionItem.UseCommitCharactersFrom(razorCompletionItem, clientCapabilities);

                completionItem = directiveAttributeCompletionItem;
                return true;
            }
            case RazorCompletionItemKind.DirectiveAttributeParameter:
            {
                var parameterCompletionItem = new VSInternalCompletionItem()
                {
                    Label = razorCompletionItem.DisplayText,
                    InsertText = razorCompletionItem.InsertText,
                    FilterText = razorCompletionItem.InsertText,
                    SortText = razorCompletionItem.SortText,
                    InsertTextFormat = insertTextFormat,
                    Kind = tagHelperCompletionItemKind,
                };

                parameterCompletionItem.UseCommitCharactersFrom(razorCompletionItem, clientCapabilities);

                completionItem = parameterCompletionItem;
                return true;
            }
            case RazorCompletionItemKind.MarkupTransition:
            {
                var markupTransitionCompletionItem = new VSInternalCompletionItem()
                {
                    Label = razorCompletionItem.DisplayText,
                    InsertText = razorCompletionItem.InsertText,
                    FilterText = razorCompletionItem.DisplayText,
                    SortText = razorCompletionItem.SortText,
                    InsertTextFormat = insertTextFormat,
                    Kind = tagHelperCompletionItemKind,
                };

                markupTransitionCompletionItem.UseCommitCharactersFrom(razorCompletionItem, clientCapabilities);

                completionItem = markupTransitionCompletionItem;
                return true;
            }
            case RazorCompletionItemKind.TagHelperElement:
            {
                var tagHelperElementCompletionItem = new VSInternalCompletionItem()
                {
                    Label = razorCompletionItem.DisplayText,
                    InsertText = razorCompletionItem.InsertText,
                    FilterText = razorCompletionItem.DisplayText,
                    SortText = razorCompletionItem.SortText,
                    InsertTextFormat = insertTextFormat,
                    Kind = tagHelperCompletionItemKind,
                };

                tagHelperElementCompletionItem.UseCommitCharactersFrom(razorCompletionItem, clientCapabilities);

                completionItem = tagHelperElementCompletionItem;
                return true;
            }
            case RazorCompletionItemKind.TagHelperAttribute:
            {
                var tagHelperAttributeCompletionItem = new VSInternalCompletionItem()
                {
                    Label = razorCompletionItem.DisplayText,
                    InsertText = razorCompletionItem.InsertText,
                    FilterText = razorCompletionItem.DisplayText,
                    SortText = razorCompletionItem.SortText,
                    InsertTextFormat = insertTextFormat,
                    Kind = tagHelperCompletionItemKind,
                };

                tagHelperAttributeCompletionItem.UseCommitCharactersFrom(razorCompletionItem, clientCapabilities);

                completionItem = tagHelperAttributeCompletionItem;
                return true;
            }
        }

        completionItem = null;
        return false;
    }

    // Internal for testing
    internal static bool IsApplicableTriggerContext(CompletionContext context)
    {
        if (context is not VSInternalCompletionContext vsCompletionContext)
        {
            Debug.Fail("Completion context should always be converted into a VSCompletionContext (even in VSCode).");

            // We do not support providing completions on delete.
            return false;
        }

        if (vsCompletionContext.TriggerKind == CompletionTriggerKind.TriggerForIncompleteCompletions)
        {
            // For incomplete completions we want to re-provide information if we would have originally.
            return true;
        }

        if (vsCompletionContext.InvokeKind == VSInternalCompletionInvokeKind.Deletion)
        {
            // We do not support providing completions on delete.
            return false;
        }

        return true;
    }
}
