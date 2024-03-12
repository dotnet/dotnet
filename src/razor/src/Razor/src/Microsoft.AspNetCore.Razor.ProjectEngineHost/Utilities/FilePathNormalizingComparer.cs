// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Microsoft.AspNetCore.Razor.Utilities;

internal sealed class FilePathNormalizingComparer : IEqualityComparer<string>
{
    public static readonly FilePathNormalizingComparer Instance = new FilePathNormalizingComparer();

    private static readonly Func<char, char> _charConverter = RuntimeInformation.IsOSPlatform(OSPlatform.Linux)
        ? c => c
        : char.ToLowerInvariant;

    private FilePathNormalizingComparer()
    {
    }

    public bool Equals(string? x, string? y) => FilePathNormalizer.FilePathsEquivalent(x, y);

    public int GetHashCode(string obj) => FilePathNormalizer.GetHashCode(obj, _charConverter);
}
