// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace Microsoft.AspNetCore.Razor.Serialization.Json;

internal sealed partial class StringCache
{
    /// <summary>
    /// This is a gross hack to do a sneaky and get the value inside the HashSet out given the lack of any Get operations in netstandard2.0.
    /// If we ever upgrade to 2.1 delete this and just use the built in TryGetValue method.
    /// </summary>
    /// <remarks>
    /// This is fragile on the ordering of the values passed to the EqualityComparer by HashSet.
    /// If that ever switches we have to react, if it becomes indeterminate we have to abandon this strategy.
    /// </remarks>
    private sealed class ExfiltratingEqualityComparer : IEqualityComparer<Entry>
    {
        public Entry? LastEqualValue { get; private set; }

        public bool Equals(Entry x, Entry y)
        {
            if (x.Equals(y))
            {
                LastEqualValue = x;
                return true;
            }
            else
            {
                LastEqualValue = null;
                return false;
            }
        }

        public int GetHashCode(Entry obj)
            => obj.GetHashCode();
    }
}
