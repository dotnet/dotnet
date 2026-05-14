// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.OLE.Interop;

namespace Microsoft.SignCheck.Interop
{
    public static class Ole32
    {
        /// <summary>
        /// Opens an existing root storage object in the file system.
        /// Preferred over StgOpenStorage per Microsoft documentation.
        /// </summary>
        [DllImport("Ole32.dll")]
        public static extern int StgOpenStorageEx(
            [MarshalAs(UnmanagedType.LPWStr)] string pwcsName,
            uint grfMode,
            uint stgfmt,
            uint grfAttrs,
            IntPtr pStgOptions,
            IntPtr pSecurityDescriptor,
            [In] ref Guid riid,
            [MarshalAs(UnmanagedType.IUnknown)] out object ppObjectOpen);

        /// <summary>Indicates that the file must be a compound file.</summary>
        public const uint STGFMT_STORAGE = 0;

        [DllImport("OLE32.DLL")]
        public static extern int StgCreateDocfile(
            [MarshalAs(UnmanagedType.LPWStr)]string pwcsName,
            uint grfMode,
            uint reserved,
            out IStorage ppstgOpen);
    }
}
