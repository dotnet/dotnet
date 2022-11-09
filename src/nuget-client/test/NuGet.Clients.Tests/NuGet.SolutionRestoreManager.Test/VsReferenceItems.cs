// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NuGet.SolutionRestoreManager.Test
{
    internal class VsReferenceItems : Collection<IVsReferenceItem>, IVsReferenceItems
    {
        public VsReferenceItems() : base() { }

        public VsReferenceItems(IEnumerable<IVsReferenceItem> collection)
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

        public IVsReferenceItem Item(Object index)
        {
            if (index is int)
            {
                return this[(int)index];
            }
            else
            {
                return null;
            }
        }
    }
}
