// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Microsoft.AspNetCore.Razor.Utilities;

namespace Microsoft.CodeAnalysis.Razor;

internal static class FilePathComparison
{
    private static StringComparison? _instance;

    public static StringComparison Instance
    {
        get
        {
            return _instance ??= PlatformInformation.IsLinux
                ? StringComparison.Ordinal
                : StringComparison.OrdinalIgnoreCase;
        }
    }
}
