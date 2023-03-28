// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Runtime.InteropServices;

namespace Microsoft.Deployment.Utilities
{
    internal static class NativeMethods
    {
        // Warning - Launcher resource type and name should never be changed
        public static readonly IntPtr Launcher_CustomResourceTypePtr = new IntPtr(50);
        public const string Launcher_ResourceName = "FILENAME";

        internal static class Kernel32
        {
            public const UInt32 LOAD_LIBRARY_AS_DATAFILE = 0x00000002;
            public const int ERROR_SHARING_VIOLATION = -2147024864;
            public const ushort IMAGE_FILE_MACHINE_ARM64 = 0xAA64;

            [DllImport(nameof(Kernel32), SetLastError = true, CallingConvention = CallingConvention.Winapi)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool IsWow64Process2([In] IntPtr process, [Out] out ushort processMachine, [Out] out ushort nativeMachine);

            [DllImport(nameof(Kernel32), CharSet = CharSet.Unicode, SetLastError = true)]
            public static extern IntPtr BeginUpdateResourceW(String fileName, bool deleteExistingResource);

            [DllImport(nameof(Kernel32), CharSet = CharSet.Unicode, SetLastError = true)]
            public static extern bool UpdateResourceW(IntPtr hUpdate, IntPtr lpType, String lpName, short wLanguage, byte[] data, int cbData);

            [DllImport(nameof(Kernel32), CharSet = CharSet.Auto, SetLastError = true)]
            public static extern bool EndUpdateResource(IntPtr hUpdate, bool fDiscard);

            [DllImport(nameof(Kernel32), CharSet = CharSet.Unicode)]
            public static extern IntPtr GetModuleHandle(string moduleName);

            [DllImport(nameof(Kernel32), CharSet = CharSet.Unicode, SetLastError = true)]
            public static extern IntPtr FindResource(IntPtr hModule, string lpName, IntPtr lpType);

            [DllImport(nameof(Kernel32), CharSet = CharSet.Unicode, SetLastError = true)]
            public static extern uint SizeofResource(IntPtr hModule, IntPtr hResInfo);

            [DllImport(nameof(Kernel32), CharSet = CharSet.Unicode, SetLastError = true)]
            public static extern IntPtr LoadResource(IntPtr hModule, IntPtr hResInfo);

            [DllImport(nameof(Kernel32), CharSet = CharSet.Unicode, SetLastError = true)]
            public static extern IntPtr LockResource(IntPtr hResData);

            [DllImport(nameof(Kernel32), CharSet = CharSet.Unicode, SetLastError = true)]
            public static extern bool FreeLibrary(IntPtr hModule);

            [DllImport(nameof(Kernel32), SetLastError = true, CharSet = CharSet.Unicode)]
            public static extern IntPtr LoadLibraryExW(string strFileName, IntPtr hFile, UInt32 ulFlags);
        }

        internal static class Clr
        {
            [DllImport(nameof(Clr), CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = false)]
            [return: MarshalAs(UnmanagedType.IUnknown)]
            public static extern object GetAssemblyIdentityFromFile([In, MarshalAs(UnmanagedType.LPWStr)] string filePath, [In] ref Guid riid);

            [ComImport]
            [Guid("6eaf5ace-7917-4f3c-b129-e046a9704766")]
            [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
            public interface IReferenceIdentity
            {
                [return: MarshalAs(UnmanagedType.LPWStr)]
                string GetAttribute([In, MarshalAs(UnmanagedType.LPWStr)] string Namespace, [In, MarshalAs(UnmanagedType.LPWStr)] string Name);
                void SetAttribute();
                void EnumAttributes();
                void Clone();
            }
        }
    }
}
