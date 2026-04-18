// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Linq;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// A class which contains useful methods for processing collections.
    /// </summary>
    internal static class CollectionUtilities
    {
        /// <summary>
        /// Checks whether the specified <paramref name="enumerable"/> is null or empty.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the <paramref name="enumerable"/>.</typeparam>
        /// <param name="enumerable">The <see cref="IEnumerable{T}"/> to be checked.</param>
        /// <returns>True if the <paramref name="enumerable"/> is null or contains no elements; otherwise, false.</returns>
        internal static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
        {
            return enumerable == null || !enumerable.Any();
        }
    }
}
