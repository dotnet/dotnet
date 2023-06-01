// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.AspNetCore.Razor.Serialization;

internal sealed class TagHelperResolutionResult
{
    public static readonly TagHelperResolutionResult Empty = new(ImmutableArray<TagHelperDescriptor>.Empty);

    public ImmutableArray<TagHelperDescriptor> Descriptors { get; }

    public TagHelperResolutionResult(ImmutableArray<TagHelperDescriptor> descriptors)
    {
        Descriptors = descriptors.NullToEmpty();
    }
}
