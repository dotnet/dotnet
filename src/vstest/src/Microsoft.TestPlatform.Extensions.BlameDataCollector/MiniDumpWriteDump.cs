// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

using Microsoft.Win32.SafeHandles;

namespace Microsoft.TestPlatform.Extensions.BlameDataCollector;

internal enum MiniDumpTypeOption
{
    // This is a copy of DumpTypeOption, we need both of those types, because this
    // dumper is included as file in both MiniDumpTool, and BlameDataCollector, and
    // blame references MiniDumpTool, so we get the enum defined twice otherwise.
    Full,
    WithHeap,
    Mini,
}

internal class MiniDumpWriteDump
{
    public static void CollectDumpUsingMiniDumpWriteDump(Process process, string outputFile, MiniDumpTypeOption type)
    {
        // Open the file for writing
        using var stream = new FileStream(outputFile, FileMode.Create, FileAccess.ReadWrite, FileShare.None);
        NativeMethods.MinidumpExceptionInformation exceptionInfo = default;

        var dumpType = type switch
        {
            MiniDumpTypeOption.Full => NativeMethods.MinidumpType.MiniDumpWithFullMemory |
                                       NativeMethods.MinidumpType.MiniDumpWithDataSegs |
                                       NativeMethods.MinidumpType.MiniDumpWithHandleData |
                                       NativeMethods.MinidumpType.MiniDumpWithUnloadedModules |
                                       NativeMethods.MinidumpType.MiniDumpWithFullMemoryInfo |
                                       NativeMethods.MinidumpType.MiniDumpWithThreadInfo |
                                       NativeMethods.MinidumpType.MiniDumpWithTokenInformation,
            MiniDumpTypeOption.WithHeap => NativeMethods.MinidumpType.MiniDumpWithPrivateReadWriteMemory |
                                           NativeMethods.MinidumpType.MiniDumpWithDataSegs |
                                           NativeMethods.MinidumpType.MiniDumpWithHandleData |
                                           NativeMethods.MinidumpType.MiniDumpWithUnloadedModules |
                                           NativeMethods.MinidumpType.MiniDumpWithFullMemoryInfo |
                                           NativeMethods.MinidumpType.MiniDumpWithThreadInfo |
                                           NativeMethods.MinidumpType.MiniDumpWithTokenInformation,
            MiniDumpTypeOption.Mini => NativeMethods.MinidumpType.MiniDumpWithThreadInfo,
            _ => NativeMethods.MinidumpType.MiniDumpNormal,
        };

        // Retry the write dump on ERROR_PARTIAL_COPY
        for (int i = 0; i < 5; i++)
        {
            // Dump the process!
            if (NativeMethods.MiniDumpWriteDump(process.Handle, (uint)process.Id, stream.SafeFileHandle, dumpType, ref exceptionInfo, IntPtr.Zero, IntPtr.Zero))
            {
                break;
            }

            int err = Marshal.GetHRForLastWin32Error();
            if (err != NativeMethods.ErrorPartialCopy)
            {
                Marshal.ThrowExceptionForHR(err);
            }
        }
    }

    private static class NativeMethods
    {
        public const int ErrorPartialCopy = unchecked((int)0x8007012b);

        [DllImport("Dbghelp.dll", SetLastError = true)]
        public static extern bool MiniDumpWriteDump(IntPtr hProcess, uint processId, SafeFileHandle hFile, MinidumpType dumpType, ref MinidumpExceptionInformation exceptionParam, IntPtr userStreamParam, IntPtr callbackParam);

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct MinidumpExceptionInformation
        {
            public readonly uint ThreadId;
            public readonly IntPtr ExceptionPointers;
            public readonly int ClientPointers;
        }

        [Flags]
        public enum MinidumpType : uint
        {
            MiniDumpNormal = 0,
            MiniDumpWithDataSegs = 1 << 0,
            MiniDumpWithFullMemory = 1 << 1,
            MiniDumpWithHandleData = 1 << 2,
            MiniDumpFilterMemory = 1 << 3,
            MiniDumpScanMemory = 1 << 4,
            MiniDumpWithUnloadedModules = 1 << 5,
            MiniDumpWithIndirectlyReferencedMemory = 1 << 6,
            MiniDumpFilterModulePaths = 1 << 7,
            MiniDumpWithProcessThreadData = 1 << 8,
            MiniDumpWithPrivateReadWriteMemory = 1 << 9,
            MiniDumpWithoutOptionalData = 1 << 10,
            MiniDumpWithFullMemoryInfo = 1 << 11,
            MiniDumpWithThreadInfo = 1 << 12,
            MiniDumpWithCodeSegs = 1 << 13,
            MiniDumpWithoutAuxiliaryState = 1 << 14,
            MiniDumpWithFullAuxiliaryState = 1 << 15,
            MiniDumpWithPrivateWriteCopyMemory = 1 << 16,
            MiniDumpIgnoreInaccessibleMemory = 1 << 17,
            MiniDumpWithTokenInformation = 1 << 18,
            MiniDumpWithModuleHeaders = 1 << 19,
            MiniDumpFilterTriage = 1 << 20,
            MiniDumpWithAvxXStateContext = 1 << 21,
            MiniDumpWithIptTrace = 1 << 22,
            MiniDumpValidTypeFlags = (-1) ^ ((~1) << 22)
        }
    }
}
