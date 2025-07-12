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
            var insertTextSpan = displayText.AsSpan();
            var originalInsertTextSpan = insertTextSpan;

            // Strip off the @ from the insertion text. This change is here to align the insertion text with the
            // completion hooks into VS and VSCode. Basically, completion triggers when `@` is typed so we don't
            // want to insert `@bind` because `@` already exists.
            if (SpanExtensions.StartsWith(insertTextSpan, '@'))
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
                if (TryGetSnippetText(containingAttribute, insertTextSpan, razorCompletionOptions, out var snippetTextSpan))
                {
                    insertTextSpan = snippetTextSpan;
                    isSnippet = true;
                }
            }

            // Don't create another string annecessarily, even thouth ReadOnlySpan.ToString() special-cases the string to avoid allocation
            var insertText = insertTextSpan == originalInsertTextSpan ? displayText : insertTextSpan.ToString();

            using var razorCommitCharacters = new PooledArrayBuilder<RazorCommitCharacter>(capacity: commitCharacters.Count);

            foreach (var c in commitCharacters)
            {
                razorCommitCharacters.Add(new(c));
            }

            var razorCompletionItem = RazorCompletionItem.CreateDirectiveAttribute(
                displayText,
                insertText,
                descriptionInfo: new([.. attributeDescriptions]),
                commitCharacters: razorCommitCharacters.ToImmutableAndClear(),
                isSnippet);

            completionItems.Add(razorCompletionItem);
        }

        return completionItems.ToImmutableAndClear();

        static bool TryGetSnippetText(
            RazorSyntaxNode owner,
            ReadOnlySpan<char> baseTextSpan,
            RazorCompletionOptions razorCompletionOptions,
            out ReadOnlySpan<char> snippetTextSpan)
        {
            if (razorCompletionOptions.SnippetsSupported
                // Don't create snippet text when attribute is already in the tag and we are trying to replace it
                // Otherwise you could have something like @onabort=""=""
                && owner is not (MarkupTagHelperDirectiveAttributeSyntax or MarkupAttributeBlockSyntax)
                && owner.Parent is not (MarkupTagHelperDirectiveAttributeSyntax or MarkupAttributeBlockSyntax))
            {
                var suffixTextSpan = razorCompletionOptions.AutoInsertAttributeQuotes ? QuotedAttributeValueSnippet : UnquotedAttributeValueSnippet;

                var buffer = new char[baseTextSpan.Length + suffixTextSpan.Length];
                baseTextSpan.CopyTo(buffer);
                suffixTextSpan.CopyTo(buffer.AsMemory()[baseTextSpan.Length..]);

                snippetTextSpan = buffer.AsSpan();
                return true;
            }

            snippetTextSpan = [];
            return false;
        }

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
