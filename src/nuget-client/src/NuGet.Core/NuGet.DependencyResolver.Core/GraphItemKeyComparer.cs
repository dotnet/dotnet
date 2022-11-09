// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace NuGet.DependencyResolver
{
    /// <summary>
    /// A <see cref="GraphItem{T}"/> Key based comparer. Two instances are equal only if the Keys are equal.
    /// </summary>
    public sealed class GraphItemKeyComparer<T> : IEqualityComparer<GraphItem<T>>
    {
        private static readonly Lazy<GraphItemKeyComparer<T>> DefaultComparer = new Lazy<GraphItemKeyComparer<T>>(() => new GraphItemKeyComparer<T>());

        /// <summary>
        /// Returns a singleton instance for the <see cref="GraphItemKeyComparer"/>.
        /// </summary>
        public static GraphItemKeyComparer<T> Instance
        {
            get
            {
                return DefaultComparer.Value;
            }
        }

        /// <summary>
        /// Get a singleton instance only through the <see cref="GraphItemKeyComparer.Instance"/>.
        /// </summary>
        private GraphItemKeyComparer()
        {
        }

        public bool Equals(GraphItem<T> x, GraphItem<T> y)
        {
            if (x == null)
            {
                return y == null;
            }
            return x.Key.Equals(y.Key);
        }

        public int GetHashCode(GraphItem<T> obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }
            return obj.Key.GetHashCode();
        }
    }
}
