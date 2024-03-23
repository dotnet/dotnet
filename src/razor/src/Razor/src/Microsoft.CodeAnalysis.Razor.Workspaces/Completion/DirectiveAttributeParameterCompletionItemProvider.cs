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

internal class DirectiveAttributeParameterCompletionItemProvider : DirectiveAttributeCompletionItemProviderBase
{
    public override ImmutableArray<RazorCompletionItem> GetCompletionItems(RazorCompletionContext context)
    {
        if (context is null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        if (context.TagHelperDocumentContext is null)
        {
            throw new ArgumentNullException(nameof(context.TagHelperDocumentContext));
        }

        if (!FileKinds.IsComponent(context.SyntaxTree.Options.FileKind))
        {
            // Directive attribute parameters are only supported in components
            return ImmutableArray<RazorCompletionItem>.Empty;
        }

        var owner = context.Owner;
        if (owner is null)
        {
            return ImmutableArray<RazorCompletionItem>.Empty;
        }

        if (!TryGetAttributeInfo(owner, out _, out var attributeName, out _, out var parameterName, out var parameterNameLocation))
        {
            // Either we're not in an attribute or the attribute is so malformed that we can't provide proper completions.
            return ImmutableArray<RazorCompletionItem>.Empty;
        }

        if (!parameterNameLocation.IntersectsWith(context.AbsoluteIndex))
        {
            // We're trying to retrieve completions on a portion of the name that is not supported (such as the name, i.e., |@bind|:format).
            return ImmutableArray<RazorCompletionItem>.Empty;
        }

        if (!TryGetElementInfo(owner.Parent.Parent, out var containingTagName, out var attributes))
        {
            // This should never be the case, it means that we're operating on an attribute that doesn't have a tag.
            return ImmutableArray<RazorCompletionItem>.Empty;
        }

        return GetAttributeParameterCompletions(attributeName, parameterName, containingTagName, attributes, context.TagHelperDocumentContext);
    }

    // Internal for testing
    internal ImmutableArray<RazorCompletionItem> GetAttributeParameterCompletions(
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
            return ImmutableArray<RazorCompletionItem>.Empty;
        }

        // Attribute parameters are case sensitive when matching
        var attributeCompletions = new Dictionary<string, HashSet<BoundAttributeDescriptionInfo>>(StringComparer.Ordinal);

        foreach (var descriptor in descriptorsForTag)
        {
            for (var i = 0; i < descriptor.BoundAttributes.Length; i++)
            {
                var attributeDescriptor = descriptor.BoundAttributes[i];
                var boundAttributeParameters = attributeDescriptor.Parameters;
                if (boundAttributeParameters.Length == 0)
                {
                    continue;
                }

                if (TagHelperMatchingConventions.CanSatisfyBoundAttribute(attributeName, attributeDescriptor))
                {
                    foreach (var parameterDescriptor in boundAttributeParameters)
                    {
                        if (attributes.Any(name => TagHelperMatchingConventions.SatisfiesBoundAttributeWithParameter(parameterDescriptor, name, attributeDescriptor)))
                        {
                            // There's already an existing attribute that satisfies this parameter, don't show it in the completion list.
                            continue;
                        }

                        if (!attributeCompletions.TryGetValue(parameterDescriptor.Name, out var attributeDescriptionInfos))
                        {
                            attributeDescriptionInfos = new HashSet<BoundAttributeDescriptionInfo>();
                            attributeCompletions[parameterDescriptor.Name] = attributeDescriptionInfos;
                        }

                        var tagHelperTypeName = descriptor.GetTypeName();
                        var descriptionInfo = BoundAttributeDescriptionInfo.From(parameterDescriptor, tagHelperTypeName);
                        attributeDescriptionInfos.Add(descriptionInfo);
                    }
                }
            }
        }

        using var completionItems = new PooledArrayBuilder<RazorCompletionItem>();

        foreach (var completion in attributeCompletions)
        {
            if (string.Equals(completion.Key, parameterName, StringComparison.Ordinal))
            {
                // This completion is identical to the selected parameter, don't provide for completions for what's already
                // present in the document.
                continue;
            }

            var razorCompletionItem = new RazorCompletionItem(
                completion.Key,
                completion.Key,
                RazorCompletionItemKind.DirectiveAttributeParameter);
            var completionDescription = new AggregateBoundAttributeDescription(completion.Value.ToImmutableArray());
            razorCompletionItem.SetAttributeCompletionDescription(completionDescription);

            completionItems.Add(razorCompletionItem);
        }

        return completionItems.DrainToImmutable();
    }
}
