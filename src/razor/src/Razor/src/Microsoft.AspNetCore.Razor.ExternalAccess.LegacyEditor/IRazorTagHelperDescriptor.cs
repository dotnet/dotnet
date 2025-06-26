// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Immutable;

namespace Microsoft.AspNetCore.Razor.ExternalAccess.LegacyEditor;

internal interface IRazorTagHelperDescriptor : IEquatable<IRazorTagHelperDescriptor>
{
    string DisplayName { get; }
    string? Documentation { get; }
    bool CaseSensitive { get; }
    string? TagOutputHint { get; }

    ImmutableArray<IRazorBoundAttributeDescriptor> BoundAttributes { get; }
    ImmutableArray<IRazorTagMatchingRuleDescriptor> TagMatchingRules { get; }

    bool IsComponentOrChildContentTagHelper();
}
