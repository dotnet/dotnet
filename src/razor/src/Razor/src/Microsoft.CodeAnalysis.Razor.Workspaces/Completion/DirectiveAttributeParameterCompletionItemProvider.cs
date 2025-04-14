// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis.Razor.Tooltip;
using Microsoft.VisualStudio.Editor.Razor;

namespace Microsoft.CodeAnalysis.Razor.Completion;

internal class DirectiveAttributeParameterCompletionItemProvider : DirectiveAttributeCompletionItemProviderBase
{
    public override ImmutableArray<RazorCompletionItem> GetCompletionItems(RazorCompletionContext context)
    {
        if (!context.SyntaxTree.Options.FileKind.IsComponent())
        {
            // Directive attribute parameters are only supported in components
            return [];
        }

        var owner = context.Owner;
        if (owner is null)
        {
            return [];
        }

        if (!TryGetAttributeInfo(owner, out _, out var attributeName, out _, out var parameterName, out var parameterNameLocation))
        {
            // Either we're not in an attribute or the attribute is so malformed that we can't provide proper completions.
            return [];
        }

        if (!parameterNameLocation.IntersectsWith(context.AbsoluteIndex))
        {
            // We're trying to retrieve completions on a portion of the name that is not supported (such as the name, i.e., |@bind|:format).
            return [];
        }

        if (!TryGetElementInfo(owner.Parent.Parent, out var containingTagName, out var attributes))
        {
            // This should never be the case, it means that we're operating on an attribute that doesn't have a tag.
            return [];
        }

        return GetAttributeParameterCompletions(attributeName, parameterName, containingTagName, attributes, context.TagHelperDocumentContext);
    }

    // Internal for testing
    internal static ImmutableArray<RazorCompletionItem> GetAttributeParameterCompletions(
        string attributeName,
        string? parameterName,
        string containingTagName,
        ImmutableArray<string> attributes,
        TagHelperDocumentContext tagHelperDocumentContext)
    {
        var descriptorsForTag = TagHelperFacts.GetTagHelpersGivenTag(tagHelperDocumentContext, containingTagName, parentTag: null);
        if (descriptorsForTag.Length == 0)
        {
            // If the current tag has no possible descriptors then we can't have any additional attributes.
            return [];
        }

        // Use ordinal dictionary because attributes are case sensitive when matching
        using var _ = StringDictionaryPool<HashSet<BoundAttributeDescriptionInfo>>.Ordinal.GetPooledObject(out var attributeCompletions);

        foreach (var descriptor in descriptorsForTag)
        {
            foreach (var attributeDescriptor in descriptor.BoundAttributes)
            {
                var boundAttributeParameters = attributeDescriptor.Parameters;
                if (boundAttributeParameters.Length == 0)
                {
                    continue;
                }

                if (TagHelperMatchingConventions.CanSatisfyBoundAttribute(attributeName, attributeDescriptor))
                {
                    foreach (var parameterDescriptor in boundAttributeParameters)
                    {
                        if (attributes.Any(
                                (parameterDescriptor, attributeDescriptor),
                                static (name, arg) =>
                                    TagHelperMatchingConventions.SatisfiesBoundAttributeWithParameter(arg.parameterDescriptor, name, arg.attributeDescriptor)))
                        {
                            // There's already an existing attribute that satisfies this parameter, don't show it in the completion list.
                            continue;
                        }

                        if (!attributeCompletions.TryGetValue(parameterDescriptor.Name, out var attributeDescriptions))
                        {
                            attributeDescriptions = [];
                            attributeCompletions[parameterDescriptor.Name] = attributeDescriptions;
                        }

                        var tagHelperTypeName = descriptor.GetTypeName();
                        var descriptionInfo = BoundAttributeDescriptionInfo.From(parameterDescriptor, tagHelperTypeName);
                        attributeDescriptions.Add(descriptionInfo);
                    }
                }
            }
        }

        using var completionItems = new PooledArrayBuilder<RazorCompletionItem>(capacity: attributeCompletions.Count);

        foreach (var (displayText, value) in attributeCompletions)
        {
            if (displayText == parameterName)
            {
                // This completion is identical to the selected parameter, don't provide for completions for what's already
                // present in the document.
                continue;
            }

            var razorCompletionItem = RazorCompletionItem.CreateDirectiveAttributeParameter(
                displayText: displayText,
                insertText: displayText,
                descriptionInfo: new([.. value]));

            completionItems.Add(razorCompletionItem);
        }

        return completionItems.DrainToImmutable();
    }
}
