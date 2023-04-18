// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace NuGet.Versioning
{
    /// <summary>
    /// Version extension methods.
    /// </summary>
    public static class VersionExtensions
    {
        /// <summary>
        /// Find the version that best matches the VersionRange and the floating behavior.
        /// </summary>
        public static T? FindBestMatch<T>(this IEnumerable<T> items,
            VersionRange? ideal,
            Func<T, NuGetVersion> selector) where T : class
        {
            if (ideal == null)
            {
                return items.FirstOrDefault();
            }

            using IEnumerator<T> enumerator = items.GetEnumerator();
            T? bestMatch = null;

            while (bestMatch == null && enumerator.MoveNext())
            {
                T current = enumerator.Current;
                if (ideal.IsBetter(
                    current: null,
                    considering: selector(current)))
                {
                    bestMatch = current;
                }
            }

            if (bestMatch != null)
            {
                while (enumerator.MoveNext())
                {
                    T current = enumerator.Current;
                    if (ideal.IsBetter(
                        current: selector(bestMatch),
                        considering: selector(current)))
                    {
                        bestMatch = current;
                    }
                }
            }

            return bestMatch;
        }

        /// <summary>
        /// Find the version that best matches the VersionRange and the floating behavior.
        /// </summary>
        public static INuGetVersionable? FindBestMatch(this IEnumerable<INuGetVersionable> items, VersionRange ideal)
        {
            return FindBestMatch<INuGetVersionable>(items, ideal, (e => e.Version));
        }
    }
}
