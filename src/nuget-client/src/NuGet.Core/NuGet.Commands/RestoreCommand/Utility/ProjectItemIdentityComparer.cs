// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace NuGet.Commands.Restore.Utility
{
    internal class ProjectItemIdentityComparer : IEqualityComparer<IItem>
    {
        internal static ProjectItemIdentityComparer Default { get; } = new();

        public bool Equals(IItem x, IItem y)
        {
            return StringComparer.InvariantCultureIgnoreCase.Equals(x.Identity, y.Identity);
        }
        public int GetHashCode(IItem obj)
        {
            return obj.Identity.GetHashCode();
        }
    }
}
