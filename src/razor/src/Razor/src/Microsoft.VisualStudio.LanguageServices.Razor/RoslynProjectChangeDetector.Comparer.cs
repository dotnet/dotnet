// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.VisualStudio.Razor;

internal partial class RoslynProjectChangeDetector
{
    private sealed class Comparer : IEqualityComparer<(Project?, IProjectSnapshot)>
    {
        public static readonly Comparer Instance = new();

        private Comparer()
        {
        }

        public bool Equals((Project?, IProjectSnapshot) x, (Project?, IProjectSnapshot) y)
        {
            var (_, snapshotX) = x;
            var (_, snapshotY) = y;

            return snapshotX.Key.Equals(snapshotY.Key);
        }

        public int GetHashCode((Project?, IProjectSnapshot) obj)
        {
            var (_, snapshot) = obj;

            return snapshot.Key.GetHashCode();
        }
    }
}
