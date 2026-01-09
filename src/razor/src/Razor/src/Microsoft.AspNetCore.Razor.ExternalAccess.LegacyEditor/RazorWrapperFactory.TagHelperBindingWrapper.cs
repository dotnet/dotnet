// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Immutable;
using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.AspNetCore.Razor.ExternalAccess.LegacyEditor;

internal static partial class RazorWrapperFactory
{
    private class TagHelperBindingWrapper(TagHelperBinding obj) : Wrapper<TagHelperBinding>(obj), IRazorTagHelperBinding
    {
        private ImmutableArray<IRazorTagHelperDescriptor> _tagHelpers;

        public ImmutableArray<IRazorTagHelperDescriptor> Descriptors
            => InitializeArrayWithWrappedItems(ref _tagHelpers, Object.TagHelpers, Wrap);

        public ImmutableArray<IRazorTagMatchingRuleDescriptor> GetBoundRules(IRazorTagHelperDescriptor descriptor)
        {
            return WrapAll(Object.GetBoundRules(Unwrap(descriptor)), Wrap);
        }
    }
}
