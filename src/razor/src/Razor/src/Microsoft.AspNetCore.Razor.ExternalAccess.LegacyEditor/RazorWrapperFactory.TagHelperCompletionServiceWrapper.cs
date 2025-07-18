﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Immutable;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Razor.Completion;

namespace Microsoft.AspNetCore.Razor.ExternalAccess.LegacyEditor;

internal static partial class RazorWrapperFactory
{
    private class TagHelperCompletionServiceWrapper(ITagHelperCompletionService obj) : Wrapper<ITagHelperCompletionService>(obj), IRazorTagHelperCompletionService
    {
        public IRazorElementCompletionContext CreateContext(IRazorTagHelperDocumentContext tagHelperContext, IEnumerable<string>? existingCompletions, string? containingTagName, IEnumerable<KeyValuePair<string, string>> attributes, string? containingParentTagName, bool containingParentIsTagHelper, Func<string, bool> inHTMLSchema)
            => Wrap(
                new ElementCompletionContext(
                    Unwrap(tagHelperContext),
                    existingCompletions,
                    containingTagName,
                    attributes.ToImmutableArray(),
                    containingParentTagName,
                    containingParentIsTagHelper,
                    inHTMLSchema));

        public ImmutableDictionary<string, ImmutableArray<IRazorTagHelperDescriptor>> GetElementCompletions(IRazorElementCompletionContext completionContext)
        {
            var result = Object.GetElementCompletions(Unwrap(completionContext));

            var completions = (Dictionary<string, IEnumerable<TagHelperDescriptor>>)result.Completions;

            return completions.ToImmutableDictionary(
                keySelector: kvp => kvp.Key,
                elementSelector: kvp => WrapAll(kvp.Value, Wrap),
                keyComparer: completions.Comparer);
        }
    }
}
