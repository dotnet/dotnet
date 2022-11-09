// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace NuGet.Commands
{
    /// <summary>
    /// A comparer for the <see cref="IMSBuildItem"/> based on the <see cref="IMSBuildItem.Identity"/>.
    /// </summary>
    internal sealed class MSBuildItemIdentityComparer : IEqualityComparer<IMSBuildItem>
    {
        public static MSBuildItemIdentityComparer Default { get; } = new MSBuildItemIdentityComparer();

        private MSBuildItemIdentityComparer()
        {
        }

        public bool Equals(IMSBuildItem x, IMSBuildItem y)
        {
            return string.Equals(x?.Identity, y?.Identity, StringComparison.OrdinalIgnoreCase);
        }

        public int GetHashCode(IMSBuildItem obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }
            return StringComparer.OrdinalIgnoreCase.GetHashCode(obj.Identity);
        }
    }
}
