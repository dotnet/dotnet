// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.CodeAnalysis.Razor;

namespace Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;

internal partial class FileWatcherBasedRazorProjectInfoDriver
{
    private sealed class Comparer : IEqualityComparer<(string FilePath, ChangeKind Kind)>
    {
        public static readonly Comparer Instance = new();

        private Comparer()
        {
        }

        public bool Equals((string FilePath, ChangeKind Kind) x, (string FilePath, ChangeKind Kind) y)
        {
            // We just want the most recent change to each file path. It's ok if there's a remove followed by an add,
            // or an add followed by a remove for the same path.
            return FilePathComparer.Instance.Equals(x.FilePath, y.FilePath);
        }

        public int GetHashCode((string FilePath, ChangeKind Kind) obj)
        {
            return FilePathComparer.Instance.GetHashCode(obj.FilePath);
        }
    }
}
