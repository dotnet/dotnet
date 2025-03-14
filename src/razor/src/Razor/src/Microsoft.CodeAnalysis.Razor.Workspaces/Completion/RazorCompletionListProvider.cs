// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Syntax;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.CodeAnalysis.Razor.Completion;

internal class RazorCompletionListProvider(
    IRazorCompletionFactsService completionFactsService,
    CompletionListCache completionListCache,
    ILoggerFactory loggerFactory)
{
    private readonly IRazorCompletionFactsService _completionFactsService = completionFactsService;
    private readonly CompletionListCache _completionListCache = completionListCache;
    private readonly ILogger _logger = loggerFactory.GetOrCreateLogger<RazorCompletionListProvider>();
    private static readonly Command s_retriggerCompletionCommand = new()
    {
        CommandIdentifier = "editor.action.triggerSuggest",
        Title = SR.ReTrigger_Completions_Title,
    };

    // virtual for tests
    public virtual VSInternalCompletionList? GetCompletionList(
        RazorCodeDocument codeDocument,
        int absoluteIndex,
        VSInternalCompletionContext completionContext,
        VSInternalClientCapabilities clientCapabilities,
        HashSet<string>? existingCompletions,
        RazorCompletionOptions completionOptions)
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

        var syntaxTree = codeDocument.GetRequiredSyntaxTree();
        var tagHelperContext = codeDocument.GetRequiredTagHelperContext();

        var owner = syntaxTree.Root.FindInnermostNode(absoluteIndex, includeWhitespace: true, walkMarkersBack: true);
        owner = AbstractRazorCompletionFactsService.AdjustSyntaxNodeForWordBoundary(owner, absoluteIndex);

        var razorCompletionContext = new RazorCompletionContext(
            absoluteIndex,
            owner,
            syntaxTree,
            tagHelperContext,
            reason,
            completionOptions,
            existingCompletions);

        var razorCompletionItems = _completionFactsService.GetCompletionItems(razorCompletionContext);

        _logger.LogTrace($"Resolved {razorCompletionItems.Length} completion items.");

        var completionList = CreateLSPCompletionList(razorCompletionItems, clientCapabilities);

        var completionCapability = clientCapabilities?.TextDocument?.Completion as VSInternalCompletionSetting;

        // The completion list is cached and can be retrieved via this result id to enable the resolve completion functionality.
        var filePath = codeDocument.Source.FilePath.AssumeNotNull();
        var razorResolveContext = new RazorCompletionResolveContext(filePath, razorCompletionItems);
        var resultId = _completionListCache.Add(completionList, razorResolveContext);
        completionList.SetResultId(resultId, completionCapability);

        return completionList;
    }

    // Internal for benchmarking and testing
    internal static VSInternalCompletionList CreateLSPCompletionList(
        ImmutableArray<RazorCompletionItem> razorCompletionItems,
        VSInternalClientCapabilities clientCapabilities)
    {
        using var items = new PooledArrayBuilder<CompletionItem>();

        foreach (var razorCompletionItem in razorCompletionItems)
        {
            if (TryConvert(razorCompletionItem, clientCapabilities, out var completionItem))
            {
                items.Add(completionItem);
            }
        }

        var completionList = new VSInternalCompletionList()
        {
            Items = items.ToArray(),
            IsIncomplete = false,
        };

        var completionCapability = clientCapabilities.TextDocument?.Completion as VSInternalCompletionSetting;

        return CompletionListOptimizer.Optimize(completionList, completionCapability);
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
        var supportedItemKinds = clientCapabilities.TextDocument?.Completion?.CompletionItemKind?.ValueSet ?? [];
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
                        Kind = razorCompletionItem.IsSnippet ? CompletionItemKind.Snippet : CompletionItemKind.Keyword,
                    };

                    directiveCompletionItem.UseCommitCharactersFrom(razorCompletionItem, clientCapabilities);

                    if (DirectiveAttributeTransitionCompletionItemProvider.IsTransitionCompletionItem(razorCompletionItem))
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
