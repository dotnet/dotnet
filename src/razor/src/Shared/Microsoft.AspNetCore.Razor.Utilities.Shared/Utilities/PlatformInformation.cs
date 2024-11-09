// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Runtime.InteropServices;

namespace Microsoft.AspNetCore.Razor.Utilities;

internal static class PlatformInformation
{
    public static bool IsWindows { get; } = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
    public static bool IsLinux { get; } = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
    public static bool IsMacOS { get; } = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

#if NET
    public static bool IsFreeBSD { get; } = RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD);
#else
    public static bool IsFreeBSD { get; } = false;
#endif
}
