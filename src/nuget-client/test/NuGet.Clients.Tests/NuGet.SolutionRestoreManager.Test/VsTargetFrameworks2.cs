// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace NuGet.SolutionRestoreManager.Test
{
    internal class VsTargetFrameworks2 : VsItemList<IVsTargetFrameworkInfo2>, IVsTargetFrameworks2
    {
        public VsTargetFrameworks2() : base() { }

        public VsTargetFrameworks2(IEnumerable<IVsTargetFrameworkInfo2> collection) : base(collection) { }

        protected override string GetKeyForItem(IVsTargetFrameworkInfo2 value) => value.TargetFrameworkMoniker;
    }
}
