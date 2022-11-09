// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.OLE.Interop;

namespace Microsoft.SignCheck.Interop
{
    public static class Ole32
    {
        [DllImport("Ole32.dll")]
        public static extern int StgOpenStorage(
            [MarshalAs(UnmanagedType.LPWStr)] string wcsName,
            IStorage pstgPriority,
            uint grfMode,            // access method
            IntPtr snbExclude,       // must be NULL
            int reserved,            // reserved
            out IStorage storage     // returned storage
            );

        [DllImport("OLE32.DLL")]
        public static extern int StgCreateDocfile(
            [MarshalAs(UnmanagedType.LPWStr)]string pwcsName,
            uint grfMode,
            uint reserved,
            out IStorage ppstgOpen);
    }
}
