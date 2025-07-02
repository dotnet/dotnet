// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Immutable;
using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.AspNetCore.Razor.ExternalAccess.LegacyEditor;

internal static partial class RazorWrapperFactory
{
    private class TagHelperDescriptorWrapper(TagHelperDescriptor obj) : Wrapper<TagHelperDescriptor>(obj), IRazorTagHelperDescriptor
    {
        private ImmutableArray<IRazorBoundAttributeDescriptor> _boundAttributes;
        private ImmutableArray<IRazorTagMatchingRuleDescriptor> _tagMatchingRules;

        public string DisplayName => Object.DisplayName;
        public string? Documentation => Object.Documentation;
        public bool CaseSensitive => Object.CaseSensitive;
        public string? TagOutputHint => Object.TagOutputHint;

        public ImmutableArray<IRazorBoundAttributeDescriptor> BoundAttributes
            => InitializeArrayWithWrappedItems(ref _boundAttributes, Object.BoundAttributes, Wrap);

        public ImmutableArray<IRazorTagMatchingRuleDescriptor> TagMatchingRules
            => InitializeArrayWithWrappedItems(ref _tagMatchingRules, Object.TagMatchingRules, Wrap);

        public override bool Equals(object obj)
            => obj is IRazorTagHelperDescriptor other &&
               Equals(other);

        public bool Equals(IRazorTagHelperDescriptor other)
            => other is not null &&
               Object.Equals(Unwrap(other));

        public override int GetHashCode()
            => Object.GetHashCode();

        public bool IsComponentOrChildContentTagHelper()
            => Object.IsComponentOrChildContentTagHelper;
    }
}
