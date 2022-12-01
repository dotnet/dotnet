// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Runtime.InteropServices;

namespace Microsoft.AspNetCore.Razor.OmniSharpPlugin;

internal static class FilePathComparison
{
    private static StringComparison? s_instance;

    public static StringComparison Instance
    {
        get
        {
            if (s_instance is null && RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                s_instance = StringComparison.Ordinal;
            }
            else if (s_instance is null)
            {
                s_instance = StringComparison.OrdinalIgnoreCase;
            }

            return s_instance.Value;
        }
    }
}
