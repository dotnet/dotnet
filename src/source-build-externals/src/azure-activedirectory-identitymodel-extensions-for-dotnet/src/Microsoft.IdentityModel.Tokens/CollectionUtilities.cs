// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Linq;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// A class which contains useful methods for processing collections.
    /// </summary>
    public static class CollectionUtilities
    {
        /// <summary>
        /// Checks whether <paramref name="enumerable"/> is null or empty.
        /// </summary>
        /// <typeparam name="T">The type of the <paramref name="enumerable"/>.</typeparam>
        /// <param name="enumerable">The <see cref="IEnumerable{T}"/> to be checked.</param>
        /// <returns>True if <paramref name="enumerable"/> is null or empty, false otherwise.</returns>
        public static bool IsNullOrEmpty<T>([NotNullWhen(false)] this IEnumerable<T> enumerable)
        {
            return enumerable == null || !enumerable.Any();
        }
    }
}
