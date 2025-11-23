// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
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
    private static readonly string s_quotedAttributeValueSnippet = "=\"$0\"";
    private static readonly string s_unquotedAttributeValueSnippet = "=$0";

    private static readonly ImmutableArray<RazorCommitCharacter> s_equalsCommitCharacters = [new("=")];
    private static readonly ImmutableArray<RazorCommitCharacter> s_snippetEqualsCommitCharacters = [new("=", Insert: false)];

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

        if (!TryGetAttributeInfo(owner, out _, out var attributeName, out var attributeNameLocation, out var parameterName, out var parameterNameLocation))
        {
            // Either we're not in an attribute or the attribute is so malformed that we can't provide proper completions.
            return [];
        }

        if (!TryGetElementInfo(owner.Parent.Parent, out var containingTagName, out var attributes))
        {
            // This should never be the case, it means that we're operating on an attribute that doesn't have a tag.
            return [];
        }

        // We don't provide Directive Attribute completions when we're in the middle of
        // another unrelated (doesn't start with @) partially completed attribute.
        // <svg xml:| ></svg> (attributeName = "xml:") should not get any directive attribute completions.
        if (!attributeName.IsNullOrWhiteSpace() && !attributeName.StartsWith('@'))
        {
            return [];
        }

        var isAttributeRequest = attributeNameLocation.IntersectsWith(context.AbsoluteIndex);
        var isParameterRequest = parameterNameLocation.IntersectsWith(context.AbsoluteIndex);

        if (!isAttributeRequest && !isParameterRequest)
        {
            // This class only provides completions on attribute/parameter names.
            return [];
        }

        var inSnippetContext = InSnippetContext(owner, context.Options);
        var directiveAttributeCompletionContext = new DirectiveAttributeCompletionContext()
        {
            SelectedAttributeName = attributeName,
            SelectedParameterName = parameterName,
            ExistingAttributes = attributes,
            UseSnippets = inSnippetContext,
            InAttributeName = isAttributeRequest,
            InParameterName = isParameterRequest,
            Options = context.Options
        };

        return GetAttributeCompletions(containingTagName, directiveAttributeCompletionContext, context.TagHelperDocumentContext);

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
    }

    // Internal for testing
    internal static ImmutableArray<RazorCompletionItem> GetAttributeCompletions(
        string containingTagName,
        DirectiveAttributeCompletionContext context,
        TagHelperDocumentContext tagHelperDocumentContext)
    {
        var descriptorsForTag = TagHelperFacts.GetTagHelpersGivenTag(tagHelperDocumentContext, containingTagName, parentTag: null);
        if (descriptorsForTag.Length == 0)
        {
            // If the current tag has no possible descriptors then we can't have any directive attributes.
            return [];
        }

        // Use ordinal dictionary because attributes are case sensitive when matching
        using var _ = SpecializedPools.GetPooledStringDictionary<(ImmutableArray<BoundAttributeDescriptionInfo>, ImmutableArray<RazorCommitCharacter>, RazorCompletionItemKind kind)>(out var attributeCompletions);

        // Collect indexer descriptors and their parent tag helper type names. Indexer descriptors indicate an attribute prefix
        // for which they apply. That can be used in an attribute name context to determine potential parameters. Eg,
        // there exists an indexer indicating it applies to attributes that start with "@bind-" and specifies six different
        // parameters applicable for those attributes (":format", ":event", ":culture", ":get", ":set", ":after")
        var indexerDescriptors = CollectIndexerDescriptors(descriptorsForTag, context);

        foreach (var descriptor in descriptorsForTag)
        {
            foreach (var attributeDescriptor in descriptor.BoundAttributes)
            {
                if (!attributeDescriptor.IsDirectiveAttribute)
                {
                    // We don't care about non-directive attributes
                    continue;
                }

                AddAttributeNameCompletions(descriptor, attributeDescriptor, context, attributeCompletions);
                AddParameterNameCompletions(descriptor, attributeDescriptor, indexerDescriptors, context, attributeCompletions);
            }
        }

        // Use the mapping populated above to create completion items
        return CreateCompletionItems(context, attributeCompletions);
    }

    private static ImmutableArray<(BoundAttributeDescriptor, string)> CollectIndexerDescriptors(
        ImmutableArray<TagHelperDescriptor> descriptorsForTag,
        DirectiveAttributeCompletionContext context)
    {
        if (context.InParameterName)
        {
            // No need to calculate the indexers When in a parameter name
            return [];
        }

        using var allIndexers = new PooledArrayBuilder<(BoundAttributeDescriptor, string)>();

        foreach (var descriptor in descriptorsForTag)
        {
            foreach (var attributeDescriptor in descriptor.BoundAttributes)
            {
                if (!attributeDescriptor.IsDirectiveAttribute)
                {
                    // We don't care about non-directive attributes
                    continue;
                }

                if (!attributeDescriptor.IndexerNamePrefix.IsNullOrEmpty())
                {
                    allIndexers.Add((attributeDescriptor, descriptor.TypeName));
                }
            }
        }

        return allIndexers.ToImmutableAndClear();
    }

    private static void AddAttributeNameCompletions(
        TagHelperDescriptor descriptor,
        BoundAttributeDescriptor attributeDescriptor,
        DirectiveAttributeCompletionContext context,
        Dictionary<string, (ImmutableArray<BoundAttributeDescriptionInfo>, ImmutableArray<RazorCommitCharacter>, RazorCompletionItemKind kind)> attributeCompletions)
    {
        if (!context.InAttributeName)
        {
            // Only add attribute name completions when in an attribute name context
            return;
        }

        var isIndexer = context.SelectedAttributeName.EndsWith("...", StringComparison.Ordinal);
        var descriptionInfo = BoundAttributeDescriptionInfo.From(attributeDescriptor, isIndexer, descriptor.TypeName);

        if (!TryAddCompletion(attributeDescriptor.Name, descriptionInfo, descriptor, context, RazorCompletionItemKind.DirectiveAttribute, attributeCompletions) && attributeDescriptor.Parameters.Length > 0)
        {
            // This attribute has parameters and the base attribute name (@bind) is already satisfied. We need to check if there are any valid
            // parameters left to be provided, if so, we need to still represent the base attribute name in the completion list.

            foreach (var parameterDescriptor in attributeDescriptor.Parameters)
            {
                if (!context.ExistingAttributes.Any(name => TagHelperMatchingConventions.SatisfiesBoundAttributeWithParameter(parameterDescriptor, name, attributeDescriptor)))
                {
                    // This bound attribute parameter has not had a completion entry added for it, re-represent the base attribute name in the completion list
                    AddCompletion(attributeDescriptor.Name, descriptionInfo, descriptor, context, RazorCompletionItemKind.DirectiveAttribute, attributeCompletions);
                    break;
                }
            }
        }

        if (!attributeDescriptor.IndexerNamePrefix.IsNullOrEmpty())
        {
            TryAddCompletion(attributeDescriptor.IndexerNamePrefix + "...", descriptionInfo, descriptor, context, RazorCompletionItemKind.DirectiveAttribute, attributeCompletions);
        }
    }

    private static void AddParameterNameCompletions(
        TagHelperDescriptor descriptor,
        BoundAttributeDescriptor attributeDescriptor,
        ImmutableArray<(BoundAttributeDescriptor, string)> indexerDescriptors,
        DirectiveAttributeCompletionContext context,
        Dictionary<string, (ImmutableArray<BoundAttributeDescriptionInfo>, ImmutableArray<RazorCommitCharacter>, RazorCompletionItemKind)> attributeCompletions)
    {
        if (context.InAttributeName && !attributeDescriptor.IndexerNamePrefix.IsNullOrEmpty())
        {
            // Don't add parameters on indexers in attribute name contexts
            return;
        }
        else if (context.InParameterName && !TagHelperMatchingConventions.CanSatisfyBoundAttribute(context.SelectedAttributeName, attributeDescriptor))
        {
            // Don't add parameters when the selected attribute name can't satisfy the given attribute descriptor in parameter name contexts
            return;
        }

        // Add indexer parameter completions first so they display first in completion descriptions.
        foreach (var (indexerDescriptor, parentTagHelperTypeName) in indexerDescriptors)
        {
            if (!attributeDescriptor.Name.StartsWith(indexerDescriptor.IndexerNamePrefix!))
            {
                continue;
            }

            AddCompletionsForParameters(indexerDescriptor.Parameters, descriptor, attributeDescriptor, parentTagHelperTypeName, context, attributeCompletions);
        }

        // Then add regular parameter completions
        AddCompletionsForParameters(attributeDescriptor.Parameters, descriptor, attributeDescriptor, descriptor.TypeName, context, attributeCompletions);

        return;

        static void AddCompletionsForParameters(
            ImmutableArray<BoundAttributeParameterDescriptor> parameterDescriptors,
            TagHelperDescriptor descriptor,
            BoundAttributeDescriptor attributeDescriptor,
            string parentTagHelperTypeName,
            DirectiveAttributeCompletionContext context,
            Dictionary<string, (ImmutableArray<BoundAttributeDescriptionInfo>, ImmutableArray<RazorCommitCharacter>, RazorCompletionItemKind)> attributeCompletions)
        {
            foreach (var parameterDescriptor in parameterDescriptors)
            {
                if (context.ExistingAttributes.Any(
                    (parameterDescriptor, attributeDescriptor),
                    static (name, arg) =>
                        TagHelperMatchingConventions.SatisfiesBoundAttributeWithParameter(arg.parameterDescriptor, name, arg.attributeDescriptor)))
                {
                    // There's already an existing attribute that satisfies this parameter, don't show it in the completion list.
                    continue;
                }

                var descriptionInfo = BoundAttributeDescriptionInfo.From(parameterDescriptor, parentTagHelperTypeName);
                var displayName = context.InParameterName
                    ? parameterDescriptor.Name
                    : $"{attributeDescriptor.Name}:{parameterDescriptor.Name}";

                AddCompletion(displayName, descriptionInfo, descriptor, context, RazorCompletionItemKind.DirectiveAttributeParameter, attributeCompletions);
            }
        }
    }

    private static ImmutableArray<RazorCompletionItem> CreateCompletionItems(DirectiveAttributeCompletionContext context, Dictionary<string, (ImmutableArray<BoundAttributeDescriptionInfo>, ImmutableArray<RazorCommitCharacter>, RazorCompletionItemKind kind)> attributeCompletions)
    {
        using var completionItems = new PooledArrayBuilder<RazorCompletionItem>(capacity: attributeCompletions.Count);

        foreach (var (displayText, (attributeDescriptions, commitCharacters, kind)) in attributeCompletions)
        {
            var originalInsertTextMemory = displayText.AsMemory();

            // Strip off the @ from the insertion text. This change is here to align the insertion text with the
            // completion hooks into VS and VSCode. Basically, completion triggers when `@` is typed so we don't
            // want to insert `@bind` because `@` already exists.
            var insertTextMemory = originalInsertTextMemory.Span.StartsWith('@')
                ? originalInsertTextMemory[1..]
                : originalInsertTextMemory;

            var isSnippet = false;
            string insertText;

            // Indexer attribute, we don't want to insert with the triple dot.
            if (MemoryExtensions.EndsWith(insertTextMemory.Span, "...".AsSpan()))
            {
                insertText = insertTextMemory[..^3].ToString();
            }
            else if (context.UseSnippets)
            {
                var suffixText = context.Options.AutoInsertAttributeQuotes ? s_quotedAttributeValueSnippet : s_unquotedAttributeValueSnippet;

                // We are trying for snippet text only for non-indexer attributes, e.g. *not* something like "@bind-..."
                insertText = string.Create(
                    length: insertTextMemory.Length + suffixText.Length,
                    state: (insertTextMemory, suffixText),
                    static (desination, state) =>
                    {
                        var (baseTextMemory, suffixText) = state;

                        baseTextMemory.Span.CopyTo(desination);
                        suffixText.AsSpan().CopyTo(desination[baseTextMemory.Length..]);
                    });

                isSnippet = true;
            }
            else
            {
                // Don't create another string unnecessarily, even though ReadOnlySpan.ToString() special-cases the string to avoid allocation
                insertText = insertTextMemory.Span == originalInsertTextMemory.Span ? displayText : insertTextMemory.ToString();
            }

            Debug.Assert(kind is RazorCompletionItemKind.DirectiveAttribute or RazorCompletionItemKind.DirectiveAttributeParameter);
            var razorCompletionItem = (kind == RazorCompletionItemKind.DirectiveAttribute)
                ? RazorCompletionItem.CreateDirectiveAttribute(displayText, insertText, descriptionInfo: new(attributeDescriptions), commitCharacters, isSnippet)
                : RazorCompletionItem.CreateDirectiveAttributeParameter(displayText, insertText, descriptionInfo: new(attributeDescriptions), commitCharacters, isSnippet);

            completionItems.Add(razorCompletionItem);
        }

        return completionItems.ToImmutableAndClear();
    }

    private static bool TryAddCompletion(
        string attributeName,
        BoundAttributeDescriptionInfo descriptionInfo,
        TagHelperDescriptor tagHelperDescriptor,
        DirectiveAttributeCompletionContext context,
        RazorCompletionItemKind kind,
        Dictionary<string, (ImmutableArray<BoundAttributeDescriptionInfo>, ImmutableArray<RazorCommitCharacter>, RazorCompletionItemKind kind)> attributeCompletions)
    {
        if (context.SelectedAttributeName != attributeName &&
            context.ExistingAttributes.Any(attributeName, static (name, attributeName) => name == attributeName))
        {
            // Attribute is already present on this element and it is not the selected attribute.
            // It shouldn't exist in the completion list.
            return false;
        }

        AddCompletion(attributeName, descriptionInfo, tagHelperDescriptor, context, kind, attributeCompletions);
        return true;
    }

    private static void AddCompletion(
        string attributeName,
        BoundAttributeDescriptionInfo descriptionInfo,
        TagHelperDescriptor tagHelperDescriptor,
        DirectiveAttributeCompletionContext context,
        RazorCompletionItemKind kind,
        Dictionary<string, (ImmutableArray<BoundAttributeDescriptionInfo>, ImmutableArray<RazorCommitCharacter>, RazorCompletionItemKind kind)> attributeCompletions)
    {
        if (!attributeCompletions.TryGetValue(attributeName, out var attributeDetails))
        {
            attributeDetails = ([], [], RazorCompletionItemKind.Attribute);
        }

        (var attributeDescriptions, var commitCharacters, _) = attributeDetails;

        if (!attributeDescriptions.Contains(descriptionInfo))
        {
            attributeDescriptions = attributeDescriptions.Add(descriptionInfo);
        }

        // Verify not an indexer attribute, as those don't commit with standard chars
        if (!attributeName.EndsWith("...", StringComparison.Ordinal))
        {
            var isEqualCommitChar = commitCharacters.Any(static c => c.Character == "=");
            var isSpaceCommitChar = commitCharacters.Any(static c => c.Character == " ");

            // We don't add "=" as a commit character when using VSCode trigger characters.
            isEqualCommitChar |= !context.Options.UseVsCodeCompletionCommitCharacters;

            foreach (var boundAttribute in tagHelperDescriptor.BoundAttributes)
            {
                isSpaceCommitChar |= boundAttribute.IsBooleanProperty;

                if (isSpaceCommitChar)
                {
                    break;
                }
            }

            // Determine if we have a common commit character set
            commitCharacters = (isEqualCommitChar, isSpaceCommitChar, context.UseSnippets) switch
            {
                (true, false, false) => s_equalsCommitCharacters,
                (true, false, true) => s_snippetEqualsCommitCharacters,
                _ => []
            };

            if (commitCharacters.IsEmpty)
            {
                if (isEqualCommitChar)
                {
                    commitCharacters = commitCharacters.Add(new("=", Insert: !context.UseSnippets));
                }

                if (isSpaceCommitChar)
                {
                    commitCharacters = commitCharacters.Add(new(" "));
                }
            }
        }

        attributeCompletions[attributeName] = (attributeDescriptions, commitCharacters, kind);
    }
}
