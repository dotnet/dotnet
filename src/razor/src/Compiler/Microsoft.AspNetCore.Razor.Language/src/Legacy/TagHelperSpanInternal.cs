// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Immutable;

namespace Microsoft.AspNetCore.Razor.Language.Legacy;

internal readonly record struct TagHelperSpanInternal(SourceSpan Span, TagHelperBinding Binding)
{
    public ImmutableArray<TagHelperDescriptor> TagHelpers => Binding.Descriptors;
}
