// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using Microsoft.AspNetCore.Razor.Language;
using TagStructureInternal = Microsoft.AspNetCore.Razor.Language.TagStructure;

namespace Microsoft.AspNetCore.Razor.ExternalAccess.LegacyEditor;

internal static partial class RazorWrapperFactory
{
    private class TagMatchingRuleDescriptorWrapper(TagMatchingRuleDescriptor obj) : Wrapper<TagMatchingRuleDescriptor>(obj), IRazorTagMatchingRuleDescriptor
    {
        private ImmutableArray<IRazorRequiredAttributeDescriptor> _attributes;

        public TagStructure TagStructure
        {
            get
            {
                return Object.TagStructure switch
                {
                    TagStructureInternal.Unspecified => TagStructure.Unspecified,
                    TagStructureInternal.NormalOrSelfClosing => TagStructure.NormalOrSelfClosing,
                    TagStructureInternal.WithoutEndTag => TagStructure.WithoutEndTag,
                    _ => throw new NotSupportedException()
                };
            }
        }

        public ImmutableArray<IRazorRequiredAttributeDescriptor> Attributes
            => InitializeArrayWithWrappedItems(ref _attributes, Object.Attributes, Wrap);
    }
}
