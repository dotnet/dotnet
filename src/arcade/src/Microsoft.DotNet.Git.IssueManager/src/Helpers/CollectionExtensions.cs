// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Microsoft.DotNet.Git.IssueManager.Helpers
{
    internal static class CollectionExtensions
    {
        public static void AddRange<T>(this Collection<T> collection, IEnumerable<T> items)
        {
            foreach (T item in items)
            {
                collection.Add(item);
            }
        }
    }
}
