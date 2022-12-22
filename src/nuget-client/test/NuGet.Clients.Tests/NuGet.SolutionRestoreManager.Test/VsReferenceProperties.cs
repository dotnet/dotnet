// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace NuGet.SolutionRestoreManager.Test
{
    internal class VsReferenceProperties : VsItemList<IVsReferenceProperty>, IVsReferenceProperties
    {
        public VsReferenceProperties() : base() { }

        public VsReferenceProperties(IEnumerable<IVsReferenceProperty> collection) : base(collection) { }

        protected override String GetKeyForItem(IVsReferenceProperty value) => value.Name;

        public void Add(string propertyName, string propertyValue)
        {
            Add(new VsReferenceProperty(propertyName, propertyValue));
        }
    }
}
