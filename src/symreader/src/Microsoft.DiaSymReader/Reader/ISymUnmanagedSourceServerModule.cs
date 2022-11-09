// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the License.txt file in the project root for more information.

using System;
using System.Runtime.InteropServices;

namespace Microsoft.DiaSymReader
{
    [ComImport]
    [Guid("997DD0CC-A76F-4c82-8D79-EA87559D27AD")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComVisible(false)]
    public interface ISymUnmanagedSourceServerModule
    {
        /// <summary>
        /// Returns the source server data for the module.
        /// </summary>
        /// <param name="length">Length of the data.</param>
        /// <param name="data">
        /// Pointer to a newly allocated memory containing the data. 
        /// Caller must free using <see cref="Marshal.FreeCoTaskMem(IntPtr)"/>.
        /// </param>
        /// <returns>
        /// S_OK on success.
        /// </returns>
        [PreserveSig]
        unsafe int GetSourceServerData(out int length, out byte* data);
    }
}
