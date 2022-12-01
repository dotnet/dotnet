// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.Extensions.Internal;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Common;

internal class HostDocumentComparer : IEqualityComparer<HostDocument>
{
    public static readonly HostDocumentComparer Instance = new();

    private HostDocumentComparer()
    {
    }

    public bool Equals(HostDocument? x, HostDocument? y)
    {
        if (x is null)
        {
            return y is null;
        }
        else if (y is null)
        {
            return false;
        }

        return x.FileKind == y.FileKind &&
               FilePathComparer.Instance.Equals(x.FilePath, y.FilePath) &&
               FilePathComparer.Instance.Equals(x.TargetPath, y.TargetPath);
    }

    public int GetHashCode(HostDocument hostDocument)
    {
        var combiner = HashCodeCombiner.Start();
        combiner.Add(hostDocument.FilePath, FilePathComparer.Instance);
        combiner.Add(hostDocument.TargetPath, FilePathComparer.Instance);
        combiner.Add(hostDocument.FileKind);

        return combiner.CombinedHash;
    }
}
