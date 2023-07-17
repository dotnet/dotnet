// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.AspNetCore.Razor.ExternalAccess.LegacyEditor;

internal static partial class RazorWrapperFactory
{
    private class TagHelperBindingWrapper(TagHelperBinding obj) : Wrapper<TagHelperBinding>(obj), IRazorTagHelperBinding
    {
        private ImmutableArray<IRazorTagHelperDescriptor> _descriptors;

        public ImmutableArray<IRazorTagHelperDescriptor> Descriptors
            => InitializeArrayWithWrappedItems(ref _descriptors, Object.Descriptors, Wrap);

        public ImmutableArray<IRazorTagMatchingRuleDescriptor> GetBoundRules(IRazorTagHelperDescriptor descriptor)
        {
            return WrapAll(Object.GetBoundRules(Unwrap(descriptor)), Wrap);
        }
    }
}
