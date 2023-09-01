// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using Microsoft.VisualStudio.Editor.Razor;

namespace Microsoft.AspNetCore.Razor.ExternalAccess.LegacyEditor;

internal static partial class RazorWrapperFactory
{
    private class TagHelperFactsServiceWrapper(ITagHelperFactsService obj) : Wrapper<ITagHelperFactsService>(obj), IRazorTagHelperFactsService
    {
        public IRazorTagHelperBinding? GetTagHelperBinding(
            IRazorTagHelperDocumentContext documentContext,
            string? tagName,
            IEnumerable<KeyValuePair<string, string>> attributes,
            string? parentTag,
            bool parentIsTagHelper)
        {
            var binding = Object.GetTagHelperBinding(Unwrap(documentContext), tagName, attributes.ToImmutableArray(), parentTag, parentIsTagHelper);

            return binding is not null
                ? WrapTagHelperBinding(binding)
                : null;
        }

        public ImmutableArray<IRazorBoundAttributeDescriptor> GetBoundTagHelperAttributes(IRazorTagHelperDocumentContext documentContext, string attributeName, IRazorTagHelperBinding binding)
        {
            var result = Object.GetBoundTagHelperAttributes(Unwrap(documentContext), attributeName, Unwrap(binding));

            return WrapAll(result, Wrap);
        }

        public ImmutableArray<IRazorTagHelperDescriptor> GetTagHelpersGivenTag(
            IRazorTagHelperDocumentContext documentContext,
            string tagName,
            string? parentTag)
        {
            var result = Object.GetTagHelpersGivenTag(Unwrap(documentContext), tagName, parentTag);

            return WrapAll(result, Wrap);
        }
    }
}
