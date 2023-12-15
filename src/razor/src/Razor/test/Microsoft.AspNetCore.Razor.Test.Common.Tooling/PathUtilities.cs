// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.IO;
using System.Runtime.InteropServices;

namespace Microsoft.AspNetCore.Razor.Test.Common;

public static class PathUtilities
{
    public static string CreateRootedPath(params string[] parts)
    {
        var result = Path.Combine(parts);

        if (!Path.IsPathRooted(result))
        {
            result = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                ? @"C:\" + result
                : "/" + result;
        }

        return result;
    }
}
