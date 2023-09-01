// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Syntax;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis.Razor.Completion;
using Microsoft.CodeAnalysis.Razor.Tooltip;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.Editor.Razor;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Completion;

internal class TagHelperCompletionProvider : IRazorCompletionItemProvider
{
    // Internal for testing
    internal static readonly IReadOnlyList<RazorCommitCharacter> MinimizedAttributeCommitCharacters = RazorCommitCharacter.FromArray(new[] { "=", " " });
    internal static readonly IReadOnlyList<RazorCommitCharacter> AttributeCommitCharacters = RazorCommitCharacter.FromArray(new[] { "=" });
    internal static readonly IReadOnlyList<RazorCommitCharacter> AttributeSnippetCommitCharacters = RazorCommitCharacter.FromArray(new[] { "=" }, insert: false);

    private static readonly IReadOnlyList<RazorCommitCharacter> s_elementCommitCharacters = RazorCommitCharacter.FromArray(new[] { " ", ">" });
    private static readonly IReadOnlyList<RazorCommitCharacter> s_noCommitCharacters = Array.Empty<RazorCommitCharacter>();
    private readonly HtmlFactsService _htmlFactsService;
    private readonly TagHelperCompletionService _tagHelperCompletionService;
    private readonly ITagHelperFactsService _tagHelperFactsService;
    private readonly IOptionsMonitor<RazorLSPOptions> _optionsMonitor;

    public TagHelperCompletionProvider(
        TagHelperCompletionService tagHelperCompletionService,
        HtmlFactsService htmlFactsService,
        ITagHelperFactsService tagHelperFactsService,
        IOptionsMonitor<RazorLSPOptions> optionsMonitor)
    {
        _tagHelperCompletionService = tagHelperCompletionService ?? throw new ArgumentException(nameof(tagHelperCompletionService));
        _htmlFactsService = htmlFactsService ?? throw new ArgumentException(nameof(htmlFactsService));
        _tagHelperFactsService = tagHelperFactsService ?? throw new ArgumentException(nameof(tagHelperFactsService));
        _optionsMonitor = optionsMonitor ?? throw new ArgumentNullException(nameof(optionsMonitor));
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

        var parent = owner.Parent;

        // When overtyping a self closing tag completion happens on "<c$$ />" and the owner is a "misc attribute content"
        // so we have to navigate back up to the start tag to provide completion. Need to be careful though as we don't
        // want element completions for <c $$ /> even though the parent node is the same
        if (parent is MarkupMiscAttributeContentSyntax { Parent: MarkupStartTagSyntax startTag } &&
            context.AbsoluteIndex == startTag.Name.Span.End)
        {
            parent = startTag;
        }

        if (_htmlFactsService.TryGetElementInfo(parent, out var containingTagNameToken, out var attributes) &&
            containingTagNameToken.Span.IntersectsWith(context.AbsoluteIndex))
        {
            var stringifiedAttributes = _tagHelperFactsService.StringifyAttributes(attributes);
            var containingElement = parent.Parent;
            var elementCompletions = GetElementCompletions(containingElement, containingTagNameToken.Content, stringifiedAttributes, context);
            return elementCompletions;
        }

        if (_htmlFactsService.TryGetAttributeInfo(
                parent,
                out containingTagNameToken,
                out var prefixLocation,
                out var selectedAttributeName,
                out var selectedAttributeNameLocation,
                out attributes) &&
            (selectedAttributeName is null ||
            selectedAttributeNameLocation?.IntersectsWith(context.AbsoluteIndex) == true ||
            (prefixLocation?.IntersectsWith(context.AbsoluteIndex) ?? false)))
        {
            if (prefixLocation.HasValue &&
                prefixLocation.Value.Length == 1 &&
                selectedAttributeNameLocation.HasValue &&
                selectedAttributeNameLocation.Value.Length > 1 &&
                selectedAttributeNameLocation.Value.Start != context.AbsoluteIndex &&
                !InOrAtEndOfAttribute(parent, context.AbsoluteIndex))
            {
                // To align with HTML completion behavior we only want to provide completion items if we're trying to resolve completion at the
                // beginning of an HTML attribute name or at the end of possible partially written attribute. We do extra checks on prefix locations here in order to rule out malformed cases when the Razor
                // compiler incorrectly parses multi-line attributes while in the middle of typing out an element. For instance:
                //
                // <SurveyPrompt |
                // @code { ... }
                //
                // Will be interpreted as having an `@code` attribute name due to multi-line attributes being a thing. Ultimately this is mostly a
                // heuristic that we have to apply in order to workaround limitations of the Razor compiler.
                return ImmutableArray<RazorCompletionItem>.Empty;
            }

            var stringifiedAttributes = _tagHelperFactsService.StringifyAttributes(attributes);

            return GetAttributeCompletions(parent, containingTagNameToken.Content, selectedAttributeName, stringifiedAttributes, context.TagHelperDocumentContext, context.Options);
            
            static bool InOrAtEndOfAttribute(SyntaxNode attributeSyntax, int absoluteIndex)
            {
                // When we are in the middle of writing an attribute it is treated as a minimilized one, e.g.:
                // <form asp$$ - 'asp' is parsed as MarkupMinimizedTagHelperAttributeSyntax (tag helper)
                // <SurveyPrompt Titl$$ - 'Titl' is parsed as MarkupMinimizedTagHelperAttributeSyntax as well (razor component)
                // Need to check for MarkupMinimizedAttributeBlockSyntax in order to handle cases when html tag becomes a tag helper only with certain attributes
                // We allow the absoluteIndex to be anywhere in the attribute, and for non minimized attributes,
                // so that `<SurveyPrompt Title=""` doesn't return only the html completions, because that has the effect of overwriting the casing of the attribute.
                return attributeSyntax is MarkupMinimizedTagHelperAttributeSyntax or MarkupMinimizedAttributeBlockSyntax or MarkupTagHelperAttributeSyntax &&
                    attributeSyntax.Span.Start < absoluteIndex && attributeSyntax.Span.End >= absoluteIndex;
            }
        }

        // Invalid location for TagHelper completions.
        return ImmutableArray<RazorCompletionItem>.Empty;
    }

    private ImmutableArray<RazorCompletionItem> GetAttributeCompletions(
        SyntaxNode containingAttribute,
        string containingTagName,
        string? selectedAttributeName,
        ImmutableArray<KeyValuePair<string, string>> attributes,
        TagHelperDocumentContext tagHelperDocumentContext,
        RazorCompletionOptions options)
    {
        var ancestors = containingAttribute.Parent.Ancestors();
        var nonDirectiveAttributeTagHelpers = tagHelperDocumentContext.TagHelpers.Where(tagHelper => !tagHelper.BoundAttributes.Any(static attribute => attribute.IsDirectiveAttribute()));
        var filteredContext = TagHelperDocumentContext.Create(tagHelperDocumentContext.Prefix, nonDirectiveAttributeTagHelpers);
        var (ancestorTagName, ancestorIsTagHelper) = _tagHelperFactsService.GetNearestAncestorTagInfo(ancestors);
        var attributeCompletionContext = new AttributeCompletionContext(
            filteredContext,
            existingCompletions: Array.Empty<string>(),
            containingTagName,
            selectedAttributeName,
            attributes,
            ancestorTagName,
            ancestorIsTagHelper,
            HtmlFactsService.IsHtmlTagName);

        using var completionItems = new PooledArrayBuilder<RazorCompletionItem>();
        var completionResult = _tagHelperCompletionService.GetAttributeCompletions(attributeCompletionContext);

        foreach (var (displayText, boundAttributes) in completionResult.Completions)
        {
            var filterText = displayText;

            // This is a little bit of a hack because the information returned by _razorTagHelperCompletionService.GetAttributeCompletions
            // does not have enough information for us to determine if a completion is an indexer completion or not. Therefore we have to
            // jump through a few hoops below to:
            //   1. Determine if this specific completion is an indexer based completion
            //   2. Resolve an appropriate snippet if it is. This is more troublesome because we need to remove the ... suffix to accurately
            //      build a snippet that makes sense for the user to type.
            var isIndexer = filterText.EndsWith("...", StringComparison.Ordinal);
            if (isIndexer)
            {
                filterText = filterText[..^3];
            }

            var attributeContext = ResolveAttributeContext(boundAttributes, isIndexer, options.SnippetsSupported);
            var attributeCommitCharacters = ResolveAttributeCommitCharacters(attributeContext);
            var isSnippet = false;
            var insertText = filterText;

            // Do not turn attributes into snippets if we are in an already written full attribute (https://github.com/dotnet/razor-tooling/issues/6724)
            if (containingAttribute is not (MarkupTagHelperAttributeSyntax or MarkupAttributeBlockSyntax) &&
                TryResolveInsertText(insertText, attributeContext, out var snippetText))
            {
                isSnippet = true;
                insertText = snippetText;
            }

            // We change the sort text depending on the tag name due to TagHelper/non-TagHelper concerns. For instance lets say you have a TagHelper that binds to `input`.
            // Chances are you're expecting to get every other `input` completion item in addition to the TagHelper completion items and the sort order should be the default
            // because HTML completion items are 100% as applicable as other items.
            //
            // Next assume that we have a TagHelper that binds `custom` (or even `Custom`); this is a special scenario where the user has effectively created a new HTML tag
            // meaning they're probably expecting to provide all of the attributes necessary for that tag to operate. Meaning, HTML attribute completions are less important.
            // To make sure we prioritize our attribute completions above all other types of completions we set the priority to high so they're showed in the completion list
            // above all other completion items.
            var sortText = HtmlFactsService.IsHtmlTagName(containingTagName)
                ? CompletionSortTextHelper.DefaultSortPriority
                : CompletionSortTextHelper.HighSortPriority;

            var razorCompletionItem = new RazorCompletionItem(
                displayText: displayText,
                insertText: insertText,
                sortText: sortText,
                kind: RazorCompletionItemKind.TagHelperAttribute,
                commitCharacters: attributeCommitCharacters,
                isSnippet: isSnippet);

            var attributeDescriptions = boundAttributes.SelectAsArray(boundAttribute => BoundAttributeDescriptionInfo.From(boundAttribute, isIndexer));

            var attributeDescriptionInfo = new AggregateBoundAttributeDescription(attributeDescriptions);
            razorCompletionItem.SetAttributeCompletionDescription(attributeDescriptionInfo);

            completionItems.Add(razorCompletionItem);
        }

        return completionItems.DrainToImmutable();
    }

    private bool TryResolveInsertText(string baseInsertText, AttributeContext context, [NotNullWhen(true)] out string? snippetText)
    {
        if (context == AttributeContext.FullSnippet)
        {
            snippetText = _optionsMonitor.CurrentValue.AutoInsertAttributeQuotes
                ? $"{baseInsertText}=\"$0\""
                : $"{baseInsertText}=$0";

            return true;
        }

        snippetText = null;
        return false;
    }

    private ImmutableArray<RazorCompletionItem> GetElementCompletions(
        SyntaxNode containingElement,
        string containingTagName,
        ImmutableArray<KeyValuePair<string, string>> attributes,
        RazorCompletionContext context)
    {
        var ancestors = containingElement.Ancestors();
        var (ancestorTagName, ancestorIsTagHelper) = _tagHelperFactsService.GetNearestAncestorTagInfo(ancestors);
        var elementCompletionContext = new ElementCompletionContext(
            context.TagHelperDocumentContext,
            context.ExistingCompletions,
            containingTagName,
            attributes,
            ancestorTagName,
            ancestorIsTagHelper,
            HtmlFactsService.IsHtmlTagName);

        var completionResult = _tagHelperCompletionService.GetElementCompletions(elementCompletionContext);
        using var completionItems = new PooledArrayBuilder<RazorCompletionItem>();

        foreach (var (displayText, tagHelpers) in completionResult.Completions)
        {
            var razorCompletionItem = new RazorCompletionItem(
                displayText: displayText,
                insertText: displayText,
                kind: RazorCompletionItemKind.TagHelperElement,
                commitCharacters: s_elementCommitCharacters);

            var tagHelperDescriptions = tagHelpers.SelectAsArray(BoundElementDescriptionInfo.From);
            var elementDescription = new AggregateBoundElementDescription(tagHelperDescriptions);
            razorCompletionItem.SetTagHelperElementDescriptionInfo(elementDescription);

            completionItems.Add(razorCompletionItem);
        }

        return completionItems.DrainToImmutable();
    }

    private const string BooleanTypeString = "System.Boolean";

    private static AttributeContext ResolveAttributeContext(
        IEnumerable<BoundAttributeDescriptor> boundAttributes,
        bool indexerCompletion,
        bool snippetsSupported)
    {
        if (indexerCompletion)
        {
            return AttributeContext.Indexer;
        }
        else if (boundAttributes.Any(static b => b.TypeName == BooleanTypeString))
        {
            // Have to use string type because IsBooleanProperty isn't set
            return AttributeContext.Minimized;
        }
        else if (snippetsSupported)
        {
            return AttributeContext.FullSnippet;
        }

        return AttributeContext.Full;
    }

    private static IReadOnlyList<RazorCommitCharacter> ResolveAttributeCommitCharacters(AttributeContext attributeContext)
    {
        return attributeContext switch
        {
            AttributeContext.Indexer => s_noCommitCharacters,
            AttributeContext.Minimized => MinimizedAttributeCommitCharacters,
            AttributeContext.Full => AttributeCommitCharacters,
            AttributeContext.FullSnippet => AttributeSnippetCommitCharacters,
            _ => throw new InvalidOperationException("Unexpected context"),
        };
    }

    private enum AttributeContext
    {
        Indexer,
        Minimized,
        Full,
        FullSnippet
    }
}
