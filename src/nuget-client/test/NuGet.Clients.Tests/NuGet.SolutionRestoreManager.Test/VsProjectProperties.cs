// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace NuGet.SolutionRestoreManager.Test
{
    internal class VsProjectProperties : VsItemList<IVsProjectProperty>, IVsProjectProperties
    {
        public VsProjectProperties() : base() { }

        public VsProjectProperties(IEnumerable<IVsProjectProperty> collection) : base(collection) { }

        public VsProjectProperties(params IVsProjectProperty[] collection) : base(collection) { }

        protected override string GetKeyForItem(IVsProjectProperty value) => value.Name;

        public void Add(string propertyName, string propertyValue)
        {
            Add(new VsProjectProperty(propertyName, propertyValue));
        }
    }
}
