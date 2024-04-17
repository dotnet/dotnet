// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis.Razor.Completion;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Completion;

// The intention of this class is to temporarily exist as a snapshot in time for our pre-existing completion experience.
// It will eventually be removed in favor of the non-Legacy variant at which point we'll also remove the feature flag
// for this legacy version.
internal class LegacyRazorCompletionEndpoint(
    IRazorCompletionFactsService completionFactsService,
    CompletionListCache completionListCache,
    IRazorLoggerFactory loggerFactory)
    : IVSCompletionEndpoint
{
    private readonly IRazorCompletionFactsService _completionFactsService = completionFactsService;
    private readonly CompletionListCache _completionListCache = completionListCache;
    private readonly ILogger _logger = loggerFactory.CreateLogger<LegacyRazorCompletionEndpoint>();

    private static readonly Command s_retriggerCompletionCommand = new()
    {
        CommandIdentifier = "editor.action.triggerSuggest",
        Title = SR.ReTrigger_Completions_Title,
    };
    private VSInternalClientCapabilities? _clientCapabilities;

    public bool MutatesSolutionState => false;

    public void ApplyCapabilities(VSInternalServerCapabilities serverCapabilities, VSInternalClientCapabilities clientCapabilities)
    {
        _clientCapabilities = clientCapabilities;

        serverCapabilities.CompletionProvider = new CompletionOptions()
        {
            ResolveProvider = true,
            TriggerCharacters = ["@", "<", ":"],
            AllCommitCharacters = [":", ">", " ", "="],
        };
    }

    public TextDocumentIdentifier GetTextDocumentIdentifier(CompletionParams request)
    {
        return request.TextDocument;
    }

    public async Task<VSInternalCompletionList?> HandleRequestAsync(CompletionParams request, RazorRequestContext requestContext, CancellationToken cancellationToken)
    {
        var documentContext = requestContext.GetRequiredDocumentContext();

        if (request.Context is null || !IsApplicableTriggerContext(request.Context))
        {
            return null;
        }

        var codeDocument = await documentContext.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);
        if (codeDocument.IsUnsupported())
        {
            return null;
        }

        var syntaxTree = codeDocument.GetSyntaxTree();
        var tagHelperDocumentContext = codeDocument.GetTagHelperContext();

        var sourceText = await documentContext.GetSourceTextAsync(cancellationToken).ConfigureAwait(false);
        if (!request.Position.TryGetAbsoluteIndex(sourceText, _logger, out var hostDocumentIndex))
        {
            return null;
        }

        var reason = request.Context.TriggerKind switch
        {
            CompletionTriggerKind.TriggerForIncompleteCompletions => CompletionReason.Invoked,
            CompletionTriggerKind.Invoked => CompletionReason.Invoked,
            CompletionTriggerKind.TriggerCharacter => CompletionReason.Typing,
            _ => CompletionReason.Typing,
        };
        var completionOptions = new RazorCompletionOptions(SnippetsSupported: true);
        var owner = syntaxTree.Root.FindInnermostNode(hostDocumentIndex, includeWhitespace: true, walkMarkersBack: true);
        owner = AbstractRazorCompletionFactsService.AdjustSyntaxNodeForWordBoundary(owner, hostDocumentIndex);
        var completionContext = new RazorCompletionContext(hostDocumentIndex, owner, syntaxTree, tagHelperDocumentContext, reason, completionOptions);

        var razorCompletionItems = _completionFactsService.GetCompletionItems(completionContext);

        _logger.LogTrace("Resolved {razorCompletionItemsCount} completion items.", razorCompletionItems.Length);

        var completionList = CreateLSPCompletionList(razorCompletionItems);
        var completionCapability = _clientCapabilities?.TextDocument?.Completion as VSInternalCompletionSetting;

        // The completion items are cached and can be retrieved via this result id to enable the "resolve" completion functionality.
        var razorResolveContext = new RazorCompletionResolveContext(documentContext.FilePath, razorCompletionItems);
        var resultId = _completionListCache.Add(completionList, razorResolveContext);
        completionList.SetResultId(resultId, completionCapability);

        return completionList;
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

        if (vsCompletionContext.InvokeKind == VSInternalCompletionInvokeKind.Deletion)
        {
            // We do not support providing completions on delete.
            return false;
        }

        return true;
    }

    // Internal for testing
    internal VSInternalCompletionList CreateLSPCompletionList(ImmutableArray<RazorCompletionItem> razorCompletionItems)
        => CreateLSPCompletionList(razorCompletionItems, _clientCapabilities!);

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

        var completionCapability = clientCapabilities?.TextDocument?.Completion as VSInternalCompletionSetting;

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
}
