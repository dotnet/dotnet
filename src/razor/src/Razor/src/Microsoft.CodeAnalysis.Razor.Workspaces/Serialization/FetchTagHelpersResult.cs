// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.CodeAnalysis.Razor.Serialization;

internal sealed record FetchTagHelpersResult(TagHelperCollection TagHelpers)
{
    public static readonly FetchTagHelpersResult Empty = new(TagHelperCollection.Empty);
}
