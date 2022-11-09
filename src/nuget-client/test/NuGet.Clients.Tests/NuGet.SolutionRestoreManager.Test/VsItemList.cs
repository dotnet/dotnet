// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NuGet.SolutionRestoreManager.Test
{
    /// <summary>
    /// Abstract list with Item method for getting members by index or name
    /// </summary>
    internal abstract class VsItemList<T> : KeyedCollection<string, T>
    {
        public VsItemList() : base() { }

        public VsItemList(IEnumerable<T> collection) : base()
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            foreach (var item in collection)
            {
                Add(item);
            }
        }

        public T Item(Object index)
        {
            if (index is string)
            {
                return this[(string)index];
            }
            else if (index is int)
            {
                return this[(int)index];
            }
            else
            {
                return default(T);
            }
        }
    }
}
