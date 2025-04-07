// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Razor.Workspaces.DocumentMapping;

internal sealed class RangeComparer : IComparer<LspRange>
{
    public static readonly RangeComparer Instance = new();

    public int Compare(LspRange? x, LspRange? y)
    {
        if (x is null)
        {
            return y is null ? 0 : 1;
        }

        if (y is null)
        {
            return -1;
        }

        return x.CompareTo(y);
    }
}
