// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.InteropServices;

namespace Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;

internal class NativeMethods
{
    public const ushort IMAGE_FILE_MACHINE_ARM64 = 0xAA64;
    public const ushort IMAGE_FILE_MACHINE_UNKNOWN = 0;

    [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool IsWow64Process2([In] IntPtr process, [Out] out ushort processMachine, [Out] out ushort nativeMachine);

    // A pointer to a value that is set to TRUE if the process is running under WOW64.
    // If the process is running under 32-bit Windows, the value is set to FALSE.
    // If the process is a 64-bit application running under 64-bit Windows, the value is also set to FALSE.
    [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool IsWow64Process([In] IntPtr process, [Out] out bool wow64Process);
}
