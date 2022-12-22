// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Razor.Tooltip;

internal class AggregateBoundElementDescription
{
    public static readonly AggregateBoundElementDescription Default = new(Array.Empty<BoundElementDescriptionInfo>());

    public AggregateBoundElementDescription(IReadOnlyList<BoundElementDescriptionInfo> associatedTagHelperDescriptions)
    {
        if (associatedTagHelperDescriptions is null)
        {
            throw new ArgumentNullException(nameof(associatedTagHelperDescriptions));
        }

        AssociatedTagHelperDescriptions = associatedTagHelperDescriptions;
    }

    public IReadOnlyList<BoundElementDescriptionInfo> AssociatedTagHelperDescriptions { get; }
}
