// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace Microsoft.AspNetCore.Razor.Utilities;

internal sealed class FilePathNormalizingComparer : IEqualityComparer<string>
{
    public static readonly FilePathNormalizingComparer Instance = new();

    private FilePathNormalizingComparer()
    {
    }

    public bool Equals(string? x, string? y) => FilePathNormalizer.AreFilePathsEquivalent(x, y);

    public int GetHashCode(string obj) => FilePathNormalizer.GetHashCode(obj);
}
