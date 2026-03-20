// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace NuGet.Packaging
{
    public static class CollectionExtensions
    {
        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            if (collection is List<T> list)
            {
                // use List's AddRange which has optimizations to minimize resizing
                list.AddRange(items);
            }
            else
            {
                foreach (var item in items)
                {
                    collection.Add(item);
                }
            }
        }
    }
}
