// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace Microsoft.VisualStudio.Razor.Discovery;

internal partial class ProjectStateChangeDetector
{
    private sealed class Comparer : IEqualityComparer<Work>
    {
        public static readonly Comparer Instance = new();

        private Comparer()
        {
        }

        public bool Equals(Work x, Work y)
            => x.Key == y.Key;

        public int GetHashCode(Work obj)
            => obj.Key.GetHashCode();
    }
}
