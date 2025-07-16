// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Immutable;
using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.AspNetCore.Razor.ExternalAccess.LegacyEditor;

internal static partial class RazorWrapperFactory
{
    private class TagHelperDocumentContextWrapper(TagHelperDocumentContext obj) : Wrapper<TagHelperDocumentContext>(obj), IRazorTagHelperDocumentContext
    {
        private ImmutableArray<IRazorTagHelperDescriptor> _tagHelpers;

        public string? Prefix => Object.Prefix;

        public ImmutableArray<IRazorTagHelperDescriptor> TagHelpers
            => InitializeArrayWithWrappedItems(ref _tagHelpers, Object.TagHelpers, Wrap);
    }
}
