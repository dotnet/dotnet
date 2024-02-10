// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using Microsoft.VisualStudio.Editor.Razor;

namespace Microsoft.AspNetCore.Razor.ExternalAccess.LegacyEditor;

internal static partial class RazorWrapperFactory
{
    /// <summary>
    ///  This isn't exactly a "wrapper", since it doesn't wrap an existing object. Instead, it provides
    ///  an implementation of <see cref="IRazorTagHelperFactsService"/> that delegates to the static
    ///  <see cref="TagHelperFacts"/> class.
    /// </summary>
    private sealed class TagHelperFactsServiceWrapper : IRazorTagHelperFactsService
    {
        public static readonly TagHelperFactsServiceWrapper Instance = new();

        private TagHelperFactsServiceWrapper()
        {
        }

        public ImmutableArray<IRazorBoundAttributeDescriptor> GetBoundTagHelperAttributes(
            IRazorTagHelperDocumentContext documentContext,
            string attributeName,
            IRazorTagHelperBinding binding)
        {
            var result = TagHelperFacts.GetBoundTagHelperAttributes(Unwrap(documentContext), attributeName, Unwrap(binding));

            return WrapAll(result, Wrap);
        }

        public IRazorTagHelperBinding? GetTagHelperBinding(
            IRazorTagHelperDocumentContext documentContext,
            string? tagName,
            IEnumerable<KeyValuePair<string, string>> attributes,
            string? parentTag,
            bool parentIsTagHelper)
        {
            var binding = TagHelperFacts.GetTagHelperBinding(Unwrap(documentContext), tagName, attributes.ToImmutableArray(), parentTag, parentIsTagHelper);

            return binding is not null
                ? WrapTagHelperBinding(binding)
                : null;
        }

        public ImmutableArray<IRazorTagHelperDescriptor> GetTagHelpersGivenTag(IRazorTagHelperDocumentContext documentContext, string tagName, string? parentTag)
        {
            var result = TagHelperFacts.GetTagHelpersGivenTag(Unwrap(documentContext), tagName, parentTag);

            return WrapAll(result, Wrap);
        }
    }
}
