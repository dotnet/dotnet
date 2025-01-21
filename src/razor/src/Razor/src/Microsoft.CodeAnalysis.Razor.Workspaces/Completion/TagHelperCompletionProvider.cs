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
using Microsoft.CodeAnalysis.Razor.Tooltip;
using Microsoft.VisualStudio.Editor.Razor;
using RazorSyntaxNode = Microsoft.AspNetCore.Razor.Language.Syntax.SyntaxNode;

namespace Microsoft.CodeAnalysis.Razor.Completion;

internal class TagHelperCompletionProvider(ITagHelperCompletionService tagHelperCompletionService) : IRazorCompletionItemProvider
{
    // Internal for testing
    internal static readonly ImmutableArray<RazorCommitCharacter> MinimizedAttributeCommitCharacters = RazorCommitCharacter.CreateArray(["=", " "]);
    internal static readonly ImmutableArray<RazorCommitCharacter> AttributeCommitCharacters = RazorCommitCharacter.CreateArray(["="]);
    internal static readonly ImmutableArray<RazorCommitCharacter> AttributeSnippetCommitCharacters = RazorCommitCharacter.CreateArray(["="], insert: false);

    private static readonly ImmutableArray<RazorCommitCharacter> s_elementCommitCharacters = RazorCommitCharacter.CreateArray([" ", ">"]);
    private static readonly ImmutableArray<RazorCommitCharacter> s_elementCommitCharacters_WithoutSpace = RazorCommitCharacter.CreateArray([">"]);

    private readonly ITagHelperCompletionService _tagHelperCompletionService = tagHelperCompletionService;

    public ImmutableArray<RazorCompletionItem> GetCompletionItems(RazorCompletionContext context)
    {
        var owner = context.Owner;
        if (owner is null)
        {
            Debug.Fail("Owner should never be null.");
            return [];
        }

        owner = owner switch
        {
            // This provider is trying to find the nearest Start or End tag. Most of the time, that's a level up, but if the index the user is typing at
            // is a token of a start or end tag directly, we already have the node we want.
            MarkupStartTagSyntax or MarkupEndTagSyntax or MarkupTagHelperStartTagSyntax or MarkupTagHelperEndTagSyntax or MarkupTagHelperAttributeSyntax => owner,
            // Invoking completion in an empty file will give us RazorDocumentSyntax which always has null parent
            RazorDocumentSyntax => owner,
            // Either the parent is a context we can handle, or it's not and we shouldn't show completions.
            _ => owner.Parent
        };

        if (HtmlFacts.TryGetElementInfo(owner, out var containingTagNameToken, out var attributes, closingForwardSlashOrCloseAngleToken: out _) &&
            containingTagNameToken.Span.IntersectsWith(context.AbsoluteIndex))
        {
            // Trying to complete the element type
            var stringifiedAttributes = TagHelperFacts.StringifyAttributes(attributes);
            var containingElement = owner.Parent;
            var elementCompletions = GetElementCompletions(containingElement, containingTagNameToken.Content, stringifiedAttributes, context);
            return elementCompletions;
        }

        if (HtmlFacts.TryGetAttributeInfo(
                owner,
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
                !InOrAtEndOfAttribute(owner, context.AbsoluteIndex))
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
                return [];
            }

            var stringifiedAttributes = TagHelperFacts.StringifyAttributes(attributes);

            return GetAttributeCompletions(owner, containingTagNameToken.Content, selectedAttributeName, stringifiedAttributes, context.TagHelperDocumentContext, context.Options);

            static bool InOrAtEndOfAttribute(RazorSyntaxNode attributeSyntax, int absoluteIndex)
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
        return [];
    }

    private ImmutableArray<RazorCompletionItem> GetAttributeCompletions(
        RazorSyntaxNode containingAttribute,
        string containingTagName,
        string? selectedAttributeName,
        ImmutableArray<KeyValuePair<string, string>> attributes,
        TagHelperDocumentContext tagHelperDocumentContext,
        RazorCompletionOptions options)
    {
        var ancestors = containingAttribute.Parent.Ancestors();
        var nonDirectiveAttributeTagHelpers = tagHelperDocumentContext.TagHelpers.WhereAsArray(
            static tagHelper => !tagHelper.BoundAttributes.Any(static attribute => attribute.IsDirectiveAttribute));
        var filteredContext = TagHelperDocumentContext.Create(tagHelperDocumentContext.Prefix, nonDirectiveAttributeTagHelpers);
        var (ancestorTagName, ancestorIsTagHelper) = TagHelperFacts.GetNearestAncestorTagInfo(ancestors);
        var attributeCompletionContext = new AttributeCompletionContext(
            filteredContext,
            existingCompletions: [],
            containingTagName,
            selectedAttributeName,
            attributes,
            ancestorTagName,
            ancestorIsTagHelper,
            HtmlFacts.IsHtmlTagName);

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
                TryResolveInsertText(insertText, attributeContext, options.AutoInsertAttributeQuotes, out var snippetText))
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
            var sortText = HtmlFacts.IsHtmlTagName(containingTagName)
                ? CompletionSortTextHelper.DefaultSortPriority
                : CompletionSortTextHelper.HighSortPriority;

            var attributeDescriptions = boundAttributes.SelectAsArray(boundAttribute => BoundAttributeDescriptionInfo.From(boundAttribute, isIndexer));

            var razorCompletionItem = RazorCompletionItem.CreateTagHelperAttribute(
                displayText: displayText,
                insertText: insertText,
                sortText: sortText,
                descriptionInfo: new(attributeDescriptions),
                commitCharacters: attributeCommitCharacters,
                isSnippet: isSnippet);

            completionItems.Add(razorCompletionItem);
        }

        return completionItems.DrainToImmutable();
    }

    private static bool TryResolveInsertText(string baseInsertText, AttributeContext context, bool autoInsertAttributeQuotes, [NotNullWhen(true)] out string? snippetText)
    {
        if (context == AttributeContext.FullSnippet)
        {
            snippetText = autoInsertAttributeQuotes
                ? $"{baseInsertText}=\"$0\""
                : $"{baseInsertText}=$0";

            return true;
        }

        snippetText = null;
        return false;
    }

    private ImmutableArray<RazorCompletionItem> GetElementCompletions(
        RazorSyntaxNode containingElement,
        string containingTagName,
        ImmutableArray<KeyValuePair<string, string>> attributes,
        RazorCompletionContext context)
    {
        var ancestors = containingElement.Ancestors();
        var (ancestorTagName, ancestorIsTagHelper) = TagHelperFacts.GetNearestAncestorTagInfo(ancestors);
        var elementCompletionContext = new ElementCompletionContext(
            context.TagHelperDocumentContext,
            context.ExistingCompletions,
            containingTagName,
            attributes,
            ancestorTagName,
            ancestorIsTagHelper,
            HtmlFacts.IsHtmlTagName);

        var completionResult = _tagHelperCompletionService.GetElementCompletions(elementCompletionContext);
        using var completionItems = new PooledArrayBuilder<RazorCompletionItem>();

        var commitChars = context.Options.CommitElementsWithSpace
            ? s_elementCommitCharacters
            : s_elementCommitCharacters_WithoutSpace;

        foreach (var (displayText, tagHelpers) in completionResult.Completions)
        {
            var tagHelperDescriptions = tagHelpers.SelectAsArray(BoundElementDescriptionInfo.From);

            var razorCompletionItem = RazorCompletionItem.CreateTagHelperElement(
                displayText: displayText,
                insertText: displayText,
                descriptionInfo: new(tagHelperDescriptions),
                commitCharacters: commitChars);

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

    private static ImmutableArray<RazorCommitCharacter> ResolveAttributeCommitCharacters(AttributeContext attributeContext)
    {
        return attributeContext switch
        {
            AttributeContext.Indexer => [],
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
