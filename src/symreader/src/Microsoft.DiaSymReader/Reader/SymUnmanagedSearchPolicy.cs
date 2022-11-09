// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the License.txt file in the project root for more information.
    
namespace Microsoft.DiaSymReader
{
    public enum SymUnmanagedSearchPolicy
    {
        /// <summary>
        /// Query the registry for symbol search paths.
        /// </summary>
        AllowRegistryAccess = 0x1,

        /// <summary>
        /// Access a symbol server.
        /// </summary>
        AllowSymbolServerAccess = 0x2,

        /// <summary>
        /// Look at the path specified in Debug Directory.
        /// </summary>
        AllowOriginalPathAccess = 0x4,

        /// <summary>
        /// Look for PDB in the place where the exe is.
        /// </summary>
        AllowReferencePathAccess = 0x8      
    }
}
