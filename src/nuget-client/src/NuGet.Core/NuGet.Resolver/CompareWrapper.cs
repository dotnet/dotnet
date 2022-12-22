// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace NuGet.Resolver
{
    /// <summary>
    /// Simple helper class to provide an IComparer instance based on a comparison function
    /// </summary>
    /// <typeparam name="T">The type to compare.</typeparam>
    public class CompareWrapper<T> : IComparer<T>
    {
        private readonly Func<T, T, int> compareImpl;

        public CompareWrapper(Func<T, T, int> compareImpl)
        {
            if (compareImpl == null)
            {
                throw new ArgumentNullException(nameof(compareImpl));
            }
            this.compareImpl = compareImpl;
        }

        public int Compare(T x, T y)
        {
            return compareImpl(x, y);
        }
    }
}
