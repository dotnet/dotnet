// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace NuGet.SolutionRestoreManager.Test
{
    internal class VsTargetFrameworks : VsItemList<IVsTargetFrameworkInfo>, IVsTargetFrameworks
    {
        public VsTargetFrameworks() : base() { }

        public VsTargetFrameworks(IEnumerable<IVsTargetFrameworkInfo> collection) : base(collection) { }

        protected override String GetKeyForItem(IVsTargetFrameworkInfo value) => value.TargetFrameworkMoniker;
    }
}
