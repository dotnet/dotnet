// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace NuGet.Protocol
{
    /// <summary>
    /// A page of items from a V2 feed as well as a link to get the next page.
    /// </summary>
    public class V2FeedPage
    {
        public V2FeedPage(List<V2FeedPackageInfo> items, string nextUri)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            Items = items;
            NextUri = nextUri;
        }

        public IReadOnlyList<V2FeedPackageInfo> Items { get; }
        public string NextUri { get; }
    }
}
