// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.AspNetCore.Razor.ExternalAccess.LegacyEditor;

internal static partial class RazorWrapperFactory
{
    private class TagHelperDocumentContextWrapper(TagHelperDocumentContext obj) : Wrapper<TagHelperDocumentContext>(obj), IRazorTagHelperDocumentContext
    {
        private ImmutableArray<IRazorTagHelperDescriptor> _tagHelpers;

        public string Prefix => Object.Prefix;

        public ImmutableArray<IRazorTagHelperDescriptor> TagHelpers
            => InitializeArrayWithWrappedItems(ref _tagHelpers, Object.TagHelpers, Wrap);
    }
}
