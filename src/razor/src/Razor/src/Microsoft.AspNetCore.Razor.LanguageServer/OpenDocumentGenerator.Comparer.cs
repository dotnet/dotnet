// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal partial class OpenDocumentGenerator
{
    private sealed class Comparer : IEqualityComparer<IDocumentSnapshot>
    {
        public static readonly Comparer Instance = new();

        private Comparer()
        {
        }

        public bool Equals(IDocumentSnapshot? x, IDocumentSnapshot? y)
        {
            if (x is null)
            {
                return y is null;
            }
            else if (y is null)
            {
                return false;
            }

            return FilePathComparer.Instance.Equals(x.FilePath, y.FilePath);
        }

        public int GetHashCode(IDocumentSnapshot obj)
        {
            return FilePathComparer.Instance.GetHashCode(obj);
        }
    }
}
