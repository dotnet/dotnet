// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Diagnostics;

namespace NuGet.ContentModel
{
    [DebuggerDisplay("Items: {Items.Count}, Properties: {Properties.Count}")]
    public class ContentItemGroup
    {
        public ContentItemGroup()
        {
            Properties = new Dictionary<string, object>();
            Items = new List<ContentItem>();
        }

        internal ContentItemGroup(IDictionary<string, object> properties, IList<ContentItem> items)
        {
            Properties = properties;
            Items = items;
        }

        public IDictionary<string, object> Properties { get; }

        public IList<ContentItem> Items { get; }
    }
}
