// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the License.txt file in the project root for more information.

using System;
using System.Runtime.InteropServices;

namespace Microsoft.DiaSymReader
{
    /// <summary>
    /// <see cref="ISymUnmanagedReader"/> creation options.
    /// </summary>
    [Flags]
    public enum SymUnmanagedReaderCreationOptions
    {
        /// <summary>
        /// Default options.
        /// </summary>
        Default = 0,

        /// <summary>
        /// Use environment variable MICROSOFT_DIASYMREADER_NATIVE_ALT_LOAD_PATH, which specifies a load directory to locate Microsoft.DiaSymReader.Native.{platform}.dll
        /// if it fails to load from <see cref="DllImportSearchPath.AssemblyDirectory"/> or <see cref="DllImportSearchPath.SafeDirectories"/>.
        /// </summary>
        /// <remarks>
        /// Loads the native library only from the alternative load directory if environment variable MICROSOFT_DIASYMREADER_NATIVE_USE_ALT_LOAD_PATH_ONLY is also set to 1.
        /// </remarks>
        UseAlternativeLoadPath = 1 << 1,

        /// <summary>
        /// Use COM registry to locate an implementation of the reader.
        /// </summary>
        UseComRegistry = 1 << 2,
    }
}
