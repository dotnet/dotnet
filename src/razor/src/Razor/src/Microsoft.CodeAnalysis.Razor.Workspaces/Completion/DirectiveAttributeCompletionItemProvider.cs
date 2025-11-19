// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Syntax;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis.Razor.Tooltip;
using Microsoft.VisualStudio.Editor.Razor;
using RazorSyntaxNode = Microsoft.AspNetCore.Razor.Language.Syntax.SyntaxNode;

namespace Microsoft.CodeAnalysis.Razor.Completion;

internal class DirectiveAttributeCompletionItemProvider : DirectiveAttributeCompletionItemProviderBase
{
    private static ReadOnlyMemory<char> QuotedAttributeValueSnippet => "=\"$0\"".AsMemory();
    private static ReadOnlyMemory<char> UnquotedAttributeValueSnippet => "=$0".AsMemory();

    private static readonly ImmutableArray<RazorCommitCharacter> EqualsCommitCharacters = [new("=")];
    private static readonly ImmutableArray<RazorCommitCharacter> EqualsAndColonCommitCharacters = [new("="), new(":")];
    private static readonly ImmutableArray<RazorCommitCharacter> SnippetEqualsCommitCharacters = [new("=", Insert: false)];
    private static readonly ImmutableArray<RazorCommitCharacter> SnippetEqualsAndColonCommitCharacters = [new("=", Insert: false), new(":")];

    public override ImmutableArray<RazorCompletionItem> GetCompletionItems(RazorCompletionContext context)
    {
        if (!context.SyntaxTree.Options.FileKind.IsComponent())
        {
            // Directive attributes are only supported in components
            return [];
        }

        var owner = context.Owner;
        if (owner is null)
        {
            return [];
        }

        if (!TryGetAttributeInfo(owner, out _, out var attributeName, out var attributeNameLocation, out _, out _))
        {
            // Either we're not in an attribute or the attribute is so malformed that we can't provide proper completions.
            return [];
        }

        if (!attributeNameLocation.IntersectsWith(context.AbsoluteIndex))
        {
            // We're trying to retrieve completions on a portion of the name that is not supported (such as a parameter).
            return [];
        }

        if (!TryGetElementInfo(owner.Parent.Parent, out var containingTagName, out var attributes))
        {
            // This should never be the case, it means that we're operating on an attribute that doesn't have a tag.
            return [];
        }

        // At this point we've determined that completions have been requested for the name portion of the selected attribute.

        var completionItems = GetAttributeCompletions(owner, attributeName, containingTagName, attributes, context.TagHelperDocumentContext, context.Options);

        // We don't provide Directive Attribute completions when we're in the middle of
        // another unrelated (doesn't start with @) partially completed attribute.
        // <svg xml:| ></svg> (attributeName = "xml:") should not get any directive attribute completions.
        if (attributeName.IsNullOrWhiteSpace() || attributeName.StartsWith('@'))
        {
            return completionItems;
        }

        return [];
    }

    // Internal for testing
    internal static ImmutableArray<RazorCompletionItem> GetAttributeCompletions(
        RazorSyntaxNode containingAttribute,
        string selectedAttributeName,
        string containingTagName,
        ImmutableArray<string> attributes,
        TagHelperDocumentContext tagHelperDocumentContext,
        RazorCompletionOptions razorCompletionOptions)
    {
        var descriptorsForTag = TagHelperFacts.GetTagHelpersGivenTag(tagHelperDocumentContext, containingTagName, parentTag: null);
        if (descriptorsForTag.Length == 0)
        {
            // If the current tag has no possible descriptors then we can't have any directive attributes.
            return [];
        }

        // Use ordinal dictionary because attributes are case sensitive when matching
        using var _ = SpecializedPools.GetPooledStringDictionary<(ImmutableArray<BoundAttributeDescriptionInfo>, ImmutableArray<RazorCommitCharacter>)>(out var attributeCompletions);
        var inSnippetContext = InSnippetContext(containingAttribute, razorCompletionOptions);

        foreach (var descriptor in descriptorsForTag)
        {
            foreach (var attributeDescriptor in descriptor.BoundAttributes)
            {
                if (!attributeDescriptor.IsDirectiveAttribute)
                {
                    // We don't care about non-directive attributes
                    continue;
                }

                if (!TryAddCompletion(attributeDescriptor.Name, attributeDescriptor, descriptor, razorCompletionOptions, selectedAttributeName, attributes, inSnippetContext, attributeCompletions) && attributeDescriptor.Parameters.Length > 0)
                {
                    // This attribute has parameters and the base attribute name (@bind) is already satisfied. We need to check if there are any valid
                    // parameters left to be provided, if so, we need to still represent the base attribute name in the completion list.

                    foreach (var parameterDescriptor in attributeDescriptor.Parameters)
                    {
                        if (!attributes.Any(name => TagHelperMatchingConventions.SatisfiesBoundAttributeWithParameter(parameterDescriptor, name, attributeDescriptor)))
                        {
                            // This bound attribute parameter has not had a completion entry added for it, re-represent the base attribute name in the completion list
                            AddCompletion(attributeDescriptor.Name, attributeDescriptor, descriptor, razorCompletionOptions, inSnippetContext, attributeCompletions);
                            break;
                        }
                    }
                }

                if (!attributeDescriptor.IndexerNamePrefix.IsNullOrEmpty())
                {
                    TryAddCompletion(attributeDescriptor.IndexerNamePrefix + "...", attributeDescriptor, descriptor, razorCompletionOptions, selectedAttributeName, attributes, inSnippetContext, attributeCompletions);
                }
            }
        }

        using var completionItems = new PooledArrayBuilder<RazorCompletionItem>(capacity: attributeCompletions.Count);

        foreach (var (displayText, (attributeDescriptions, commitCharacters)) in attributeCompletions)
        {
            var insertTextSpan = displayText.AsSpan();
            var originalInsertTextSpan = insertTextSpan;

            // Strip off the @ from the insertion text. This change is here to align the insertion text with the
            // completion hooks into VS and VSCode. Basically, completion triggers when `@` is typed so we don't
            // want to insert `@bind` because `@` already exists.
            if (insertTextSpan.StartsWith('@'))
            {
                insertTextSpan = insertTextSpan[1..];
            }

            var isSnippet = false;
            // Indexer attribute, we don't want to insert with the triple dot.
            if (MemoryExtensions.EndsWith(insertTextSpan, "...".AsSpan()))
            {
                insertTextSpan = insertTextSpan[..^3];
            }
            else
            {
                // We are trying for snippet text only for non-indexer attributes, e.g. *not* something like "@bind-..."
                if (inSnippetContext)
                {
                    GetSnippetText(insertTextSpan, razorCompletionOptions, out insertTextSpan);
                    isSnippet = true;
                }
            }

            // Don't create another string unnecessarily, even though ReadOnlySpan.ToString() special-cases the string to avoid allocation
            var insertText = insertTextSpan == originalInsertTextSpan ? displayText : insertTextSpan.ToString();

            var razorCompletionItem = RazorCompletionItem.CreateDirectiveAttribute(
                displayText,
                insertText,
                descriptionInfo: new(attributeDescriptions),
                commitCharacters,
                isSnippet);

            completionItems.Add(razorCompletionItem);
        }

        return completionItems.ToImmutableAndClear();

        static bool InSnippetContext(
            RazorSyntaxNode owner,
            RazorCompletionOptions razorCompletionOptions)
        {
            return razorCompletionOptions.SnippetsSupported
                // Don't create snippet text when attribute is already in the tag and we are trying to replace it
                // Otherwise you could have something like @onabort=""=""
                && owner is not (MarkupTagHelperDirectiveAttributeSyntax or MarkupAttributeBlockSyntax)
                && owner.Parent is not (MarkupTagHelperDirectiveAttributeSyntax or MarkupAttributeBlockSyntax);
        }

        static void GetSnippetText(
            ReadOnlySpan<char> baseTextSpan,
            RazorCompletionOptions razorCompletionOptions,
            out ReadOnlySpan<char> snippetTextSpan)
        {
            var suffixTextSpan = razorCompletionOptions.AutoInsertAttributeQuotes ? QuotedAttributeValueSnippet : UnquotedAttributeValueSnippet;

            var buffer = new char[baseTextSpan.Length + suffixTextSpan.Length];
            baseTextSpan.CopyTo(buffer);
            suffixTextSpan.CopyTo(buffer.AsMemory()[baseTextSpan.Length..]);

            snippetTextSpan = buffer.AsSpan();
        }

        static bool TryAddCompletion(
            string attributeName,
            BoundAttributeDescriptor boundAttributeDescriptor,
            TagHelperDescriptor tagHelperDescriptor,
            RazorCompletionOptions razorCompletionOptions,
            string selectedAttributeName,
            ImmutableArray<string> attributes,
            bool inSnippetContext,
            Dictionary<string, (ImmutableArray<BoundAttributeDescriptionInfo>, ImmutableArray<RazorCommitCharacter>)> attributeCompletions)
        {
            if (selectedAttributeName != attributeName &&
                attributes.Any(attributeName, static (name, attributeName) => name == attributeName))
            {
                // Attribute is already present on this element and it is not the selected attribute.
                // It shouldn't exist in the completion list.
                return false;
            }

            AddCompletion(attributeName, boundAttributeDescriptor, tagHelperDescriptor, razorCompletionOptions, inSnippetContext, attributeCompletions);
            return true;
        }

        static void AddCompletion(
            string attributeName,
            BoundAttributeDescriptor boundAttributeDescriptor,
            TagHelperDescriptor tagHelperDescriptor,
            RazorCompletionOptions razorCompletionOptions,
            bool inSnippetContext,
            Dictionary<string, (ImmutableArray<BoundAttributeDescriptionInfo>, ImmutableArray<RazorCommitCharacter>)> attributeCompletions)
        {
            if (!attributeCompletions.TryGetValue(attributeName, out var attributeDetails))
            {
                attributeDetails = ([], []);
            }

            (var attributeDescriptions, var commitCharacters) = attributeDetails;

            var indexerCompletion = attributeName.EndsWith("...", StringComparison.Ordinal);
            var tagHelperTypeName = tagHelperDescriptor.TypeName;
            var descriptionInfo = BoundAttributeDescriptionInfo.From(boundAttributeDescriptor, isIndexer: indexerCompletion, tagHelperTypeName);

            if (!attributeDescriptions.Contains(descriptionInfo))
            {
                attributeDescriptions = attributeDescriptions.Add(descriptionInfo);
            }

            // Verify not an indexer attribute, as those don't commit with standard chars
            if (!indexerCompletion)
            {
                var equalsAdded = commitCharacters.Any(static c => c.Character == "=");
                var spaceAdded = commitCharacters.Any(static c => c.Character == " ");
                var colonAdded = commitCharacters.Any(static c => c.Character == ":");

                // We don't add "=" as a commit character when using VSCode trigger characters.
                equalsAdded |= !razorCompletionOptions.UseVsCodeCompletionCommitCharacters;

                foreach (var boundAttribute in tagHelperDescriptor.BoundAttributes)
                {
                    spaceAdded |= boundAttribute.IsBooleanProperty;
                    colonAdded |= boundAttribute.Parameters.Length > 0;

                    if (spaceAdded && colonAdded)
                    {
                        break;
                    }
                }

                // Determine if we have a common commit character set
                commitCharacters = (equalsAdded, spaceAdded, colonAdded, inSnippetContext) switch
                {
                    (true, false, false, false) => EqualsCommitCharacters,
                    (true, false, true, false) => EqualsAndColonCommitCharacters,
                    (true, false, false, true) => SnippetEqualsCommitCharacters,
                    (true, false, true, true) => SnippetEqualsAndColonCommitCharacters,
                    _ => []
                };

                if (commitCharacters.IsEmpty)
                {
                    if (equalsAdded)
                    {
                        commitCharacters = commitCharacters.Add(new("=", Insert: !inSnippetContext));
                    }

                    if (spaceAdded)
                    {
                        commitCharacters = commitCharacters.Add(new(" "));
                    }

                    if (colonAdded)
                    {
                        commitCharacters = commitCharacters.Add(new(":"));
                    }
                }
            }

            attributeCompletions[attributeName] = (attributeDescriptions, commitCharacters);
        }
    }
}
