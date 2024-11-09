// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Microsoft.AspNetCore.Razor.Utilities;

namespace Microsoft.CodeAnalysis.Razor;

internal static class FilePathComparer
{
    private static StringComparer? _instance;

    public static StringComparer Instance
    {
        get
        {
            return _instance ??= PlatformInformation.IsLinux
                ? StringComparer.Ordinal
                : StringComparer.OrdinalIgnoreCase;
        }
    }
}
