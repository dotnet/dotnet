// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Microsoft.DotNet.Arcade.Sdk.Tests
{
    public static class CollectionExtensions
    {
        public static Collection<T> AddRange<T>(this Collection<T> collection, IEnumerable<T> items)
        {
            foreach(var item in items)
            {
                collection.Add(item);
            }
            return collection;
        }
    }
}
