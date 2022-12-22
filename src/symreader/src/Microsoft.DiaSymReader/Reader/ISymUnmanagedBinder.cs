// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the License.txt file in the project root for more information.

using System;
using System.Runtime.InteropServices;

namespace Microsoft.DiaSymReader
{
    [ComImport]
    [Guid("AA544D42-28CB-11d3-BD22-0000F80849BD")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComVisible(false)]
    public interface ISymUnmanagedBinder
    {
        /// <summary>
        /// Given a metadata interface and a file name, returns a new instance of <see cref="ISymUnmanagedReader"/> 
        /// that will read the debugging symbols associated with the specified PE file.
        /// </summary>
        /// <param name="metadataImporter">An instance of IMetadataImport providing metadata for the specified PE file.</param>
        /// <param name="fileName">PE file path.</param>
        /// <param name="searchPath">Alternate path to search for debug data.</param>
        /// <param name="reader">The new reader instance.</param>
        [PreserveSig]
        int GetReaderForFile(
            [MarshalAs(UnmanagedType.Interface)]object metadataImporter,
            [MarshalAs(UnmanagedType.LPWStr)]string fileName,
            [MarshalAs(UnmanagedType.LPWStr)]string searchPath,
            [MarshalAs(UnmanagedType.Interface)]out ISymUnmanagedReader reader);

        /// <summary>
        /// Given a metadata interface and a stream that contains
        /// the symbol store, returns the <see cref="ISymUnmanagedReader"/>
        /// that will read the debugging symbols from the given
        /// symbol store.
        /// </summary>
        /// <param name="metadataImporter">An instance of IMetadataImport providing metadata for the corresponding PE file.</param>
        /// <param name="stream">PDB stream.</param>
        /// <param name="reader">The new reader instance.</param>
        [PreserveSig]
        int GetReaderFromStream(
            [MarshalAs(UnmanagedType.Interface)]object metadataImporter,
            [MarshalAs(UnmanagedType.Interface)]object stream,
            [MarshalAs(UnmanagedType.Interface)]out ISymUnmanagedReader reader);
    };
}
