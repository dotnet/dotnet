// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.AspNetCore.Razor.Serialization;

internal sealed record FetchTagHelpersResult(ImmutableArray<TagHelperDescriptor> TagHelpers)
{
    public static readonly FetchTagHelpersResult Empty = new(ImmutableArray<TagHelperDescriptor>.Empty);
}
