// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;

namespace NuGet.Shared
{
    internal static class Extensions
    {
        /// <summary>
        /// Return the enumerable as a List of T, copying if required. Optimized for common case where it is an List of T.
        /// Avoid mutating the return value.
        /// </summary>
        /// <remarks>https://aspnetwebstack.codeplex.com/SourceControl/latest#src/Common/CollectionExtensions.cs</remarks>
        public static List<T> AsList<T>(this IEnumerable<T> enumerable)
        {
            var list = enumerable as List<T>;
            if (list != null)
            {
                return list;
            }

            return new List<T>(enumerable);
        }

        /// <summary>
        /// Return the ISet as a HashSet of T, copying if required. Optimized for common case where it is a HashSet of T.
        /// Avoid mutating the return value.
        /// </summary>
        public static HashSet<T>? AsHashSet<T>(this ISet<T> enumerable, IEqualityComparer<T>? comparer = null)
        {
            if (enumerable == null)
            {
                return null;
            }

            var set = enumerable as HashSet<T>;
            if (set != null)
            {
                return set;
            }
            else
            {
                if (comparer == null)
                {
                    comparer = EqualityComparer<T>.Default;
                }

                return new HashSet<T>(enumerable, comparer);
            }
        }

        public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
        {
            foreach (var item in enumeration)
            {
                action(item);
            }
        }

    }
}
