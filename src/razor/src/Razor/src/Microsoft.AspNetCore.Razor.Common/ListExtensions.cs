// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace Microsoft.AspNetCore.Razor;

internal static class ListExtensions
{
    /// <summary>
    ///  Set the <paramref name="list"/>'s capacity if it is less than <paramref name="newCapacity"/>.
    /// </summary>
    public static void SetCapacityIfLarger<T>(this List<T> list, int newCapacity)
    {
        if (list.Capacity < newCapacity)
        {
            list.Capacity = newCapacity;
        }
    }
}
