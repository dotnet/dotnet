// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.AspNetCore.Razor;

internal static class HashSetExtensions
{
    // On .NET Framework, Enumerable.ToArray() will create a new empty array for any
    // empty IEnumerable<T>. This works around that extra allocation for HashSet<T>.
    public static T[] ToArray<T>(this HashSet<T> set)
        => set.Count == 0
            ? Array.Empty<T>()
            : ((IEnumerable<T>)set).ToArray();
}
