// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

internal abstract partial class AbstractRazorProjectInfoDriver
{
    private sealed class Comparer : IEqualityComparer<Work>
    {
        public static readonly Comparer Instance = new();

        private Comparer()
        {
        }

        public bool Equals(Work? x, Work? y)
        {
            if (x is null)
            {
                return y is null;
            }
            else if (y is null)
            {
                return false;
            }

            return x.ProjectKey.Equals(y.ProjectKey);
        }

        public int GetHashCode(Work work)
        {
            return work.ProjectKey.GetHashCode();
        }
    }
}
