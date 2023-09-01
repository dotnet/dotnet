// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;

namespace Microsoft.CodeAnalysis.Razor.Tooltip;

internal sealed record AggregateBoundAttributeDescription(ImmutableArray<BoundAttributeDescriptionInfo> DescriptionInfos)
{
    public static readonly AggregateBoundAttributeDescription Empty = new(ImmutableArray<BoundAttributeDescriptionInfo>.Empty);
}
