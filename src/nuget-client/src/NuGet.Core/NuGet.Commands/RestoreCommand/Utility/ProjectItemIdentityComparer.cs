// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;

namespace NuGet.Commands.Restore.Utility
{
    internal class ProjectItemIdentityComparer : IEqualityComparer<IItem>
    {
        internal static ProjectItemIdentityComparer Default { get; } = new();

        private IEqualityComparer<string> _identityComparer = StringComparer.OrdinalIgnoreCase;

        public bool Equals(IItem x, IItem y)
        {
            return _identityComparer.Equals(x.Identity, y.Identity);
        }
        public int GetHashCode(IItem obj)
        {
            return _identityComparer.GetHashCode(obj.Identity);
        }
    }
}
