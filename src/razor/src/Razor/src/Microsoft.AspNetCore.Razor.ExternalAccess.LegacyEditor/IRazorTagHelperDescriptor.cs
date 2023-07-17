// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

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
