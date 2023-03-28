// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the License.txt file in the project root for more information.

using System;
using System.Runtime.InteropServices;

namespace Microsoft.DiaSymReader
{
    [ComImport]
    [Guid("2d7babeb-4415-4a19-8be0-dfacc7611594")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComVisible(false)]
    public interface ISymUnmanagedCompilerInfoReader
    {
        /// <summary>
        /// Returns compiler version number and name.
        /// </summary>
        [PreserveSig]
        int GetCompilerInfo(
            out ushort major,
            out ushort minor,
            out ushort build,
            out ushort revision,
            int bufferLength,
            out int count,
            [In, Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] char[] name);
    }
}
