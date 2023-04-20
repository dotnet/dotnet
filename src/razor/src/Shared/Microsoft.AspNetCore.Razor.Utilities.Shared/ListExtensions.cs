// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
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

    /// <summary>
    ///  Copies the elements of the <see cref="List{T}"/> to a new array, or returns an
    ///  empty array if the <see cref="List{T}"/> is null.
    /// </summary>
    /// <remarks>
    ///  On .NET Framework, <see cref="List{T}.ToArray()"/> will create a new empty array for any
    ///  empty <see cref="List{T}"/>. This method avoids that extra allocation.
    /// </remarks>
    public static T[] ToArrayOrEmpty<T>(this List<T>? list)
        => list?.Count > 0
            ? list.ToArray()
            : Array.Empty<T>();
}
