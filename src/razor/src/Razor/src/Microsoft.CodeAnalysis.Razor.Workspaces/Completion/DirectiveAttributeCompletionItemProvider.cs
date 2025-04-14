// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis.Razor.Tooltip;
using Microsoft.VisualStudio.Editor.Razor;

namespace Microsoft.CodeAnalysis.Razor.Completion;

internal class DirectiveAttributeCompletionItemProvider : DirectiveAttributeCompletionItemProviderBase
{
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

        var completionItems = GetAttributeCompletions(attributeName, containingTagName, attributes, context.TagHelperDocumentContext);

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
        string selectedAttributeName,
        string containingTagName,
        ImmutableArray<string> attributes,
        TagHelperDocumentContext tagHelperDocumentContext)
    {
        var descriptorsForTag = TagHelperFacts.GetTagHelpersGivenTag(tagHelperDocumentContext, containingTagName, parentTag: null);
        if (descriptorsForTag.Length == 0)
        {
            // If the current tag has no possible descriptors then we can't have any directive attributes.
            return [];
        }

        // Use ordinal dictionary because attributes are case sensitive when matching
        using var _ = StringDictionaryPool<(HashSet<BoundAttributeDescriptionInfo>, HashSet<string>)>.Ordinal.GetPooledObject(out var attributeCompletions);

        foreach (var descriptor in descriptorsForTag)
        {
            foreach (var attributeDescriptor in descriptor.BoundAttributes)
            {
                if (!attributeDescriptor.IsDirectiveAttribute)
                {
                    // We don't care about non-directive attributes
                    continue;
                }

                if (!TryAddCompletion(attributeDescriptor.Name, attributeDescriptor, descriptor) && attributeDescriptor.Parameters.Length > 0)
                {
                    // This attribute has parameters and the base attribute name (@bind) is already satisfied. We need to check if there are any valid
                    // parameters left to be provided, if so, we need to still represent the base attribute name in the completion list.

                    foreach (var parameterDescriptor in attributeDescriptor.Parameters)
                    {
                        if (!attributes.Any(name => TagHelperMatchingConventions.SatisfiesBoundAttributeWithParameter(parameterDescriptor, name, attributeDescriptor)))
                        {
                            // This bound attribute parameter has not had a completion entry added for it, re-represent the base attribute name in the completion list
                            AddCompletion(attributeDescriptor.Name, attributeDescriptor, descriptor);
                            break;
                        }
                    }
                }

                if (!attributeDescriptor.IndexerNamePrefix.IsNullOrEmpty())
                {
                    TryAddCompletion(attributeDescriptor.IndexerNamePrefix + "...", attributeDescriptor, descriptor);
                }
            }
        }

        using var completionItems = new PooledArrayBuilder<RazorCompletionItem>(capacity: attributeCompletions.Count);

        foreach (var (displayText, (attributeDescriptions, commitCharacters)) in attributeCompletions)
        {
            var insertText = displayText;

            // Strip off the @ from the insertion text. This change is here to align the insertion text with the
            // completion hooks into VS and VSCode. Basically, completion triggers when `@` is typed so we don't
            // want to insert `@bind` because `@` already exists.
            var startIndex = insertText.StartsWith('@') ? 1 : 0;

            // Indexer attribute, we don't want to insert with the triple dot.
            var endIndex = insertText.EndsWith("...", StringComparison.Ordinal) ? ^3 : ^0;

            // Don't allocate a new string unless we need to make a change.
            if (startIndex > 0 || endIndex.Value > 0)
            {
                insertText = insertText[startIndex..endIndex];
            }

            using var razorCommitCharacters = new PooledArrayBuilder<RazorCommitCharacter>(capacity: commitCharacters.Count);

            foreach (var c in commitCharacters)
            {
                razorCommitCharacters.Add(new(c));
            }

            var razorCompletionItem = RazorCompletionItem.CreateDirectiveAttribute(
                displayText,
                insertText,
                descriptionInfo: new([.. attributeDescriptions]),
                commitCharacters: razorCommitCharacters.DrainToImmutable());

            completionItems.Add(razorCompletionItem);
        }

        return completionItems.DrainToImmutable();

        bool TryAddCompletion(string attributeName, BoundAttributeDescriptor boundAttributeDescriptor, TagHelperDescriptor tagHelperDescriptor)
        {
            if (selectedAttributeName != attributeName &&
                attributes.Any(attributeName, static (name, attributeName) => name == attributeName))
            {
                // Attribute is already present on this element and it is not the selected attribute.
                // It shouldn't exist in the completion list.
                return false;
            }

            AddCompletion(attributeName, boundAttributeDescriptor, tagHelperDescriptor);
            return true;
        }

        void AddCompletion(string attributeName, BoundAttributeDescriptor boundAttributeDescriptor, TagHelperDescriptor tagHelperDescriptor)
        {
            if (!attributeCompletions.TryGetValue(attributeName, out var attributeDetails))
            {
                attributeDetails = ([], []);
                attributeCompletions[attributeName] = attributeDetails;
            }

            (var attributeDescriptions, var commitCharacters) = attributeDetails;

            var indexerCompletion = attributeName.EndsWith("...", StringComparison.Ordinal);
            var tagHelperTypeName = tagHelperDescriptor.GetTypeName();
            var descriptionInfo = BoundAttributeDescriptionInfo.From(boundAttributeDescriptor, isIndexer: indexerCompletion, tagHelperTypeName);
            attributeDescriptions.Add(descriptionInfo);

            if (indexerCompletion)
            {
                // Indexer attribute, we don't want to commit with standard chars
                return;
            }

            commitCharacters.Add("=");

            var spaceAdded = commitCharacters.Contains(" ");
            var colonAdded = commitCharacters.Contains(":");

            if (!spaceAdded || !colonAdded)
            {
                foreach (var boundAttribute in tagHelperDescriptor.BoundAttributes)
                {
                    if (!spaceAdded && boundAttribute.IsBooleanProperty)
                    {
                        commitCharacters.Add(" ");
                        spaceAdded = true;
                    }
                    else if (!colonAdded && boundAttribute.Parameters.Length > 0)
                    {
                        commitCharacters.Add(":");
                        colonAdded = true;
                    }
                    else if (spaceAdded && colonAdded)
                    {
                        break;
                    }
                }
            }
        }
    }
}
