// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the License.txt file in the project root for more information.

using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace Microsoft.DiaSymReader
{
    [ComImport]
    [Guid("E502D2DD-8671-4338-8F2A-FC08229628C4")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComVisible(false)]
    public interface ISymUnmanagedEncUpdate
    {
        /// <summary>
        /// Applies EnC edit.
        /// </summary>
        [PreserveSig]
        int UpdateSymbolStore2(
            IStream stream,
            [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)]SymUnmanagedLineDelta[] lineDeltas,
            int lineDeltaCount);

        /// <summary>
        /// Gets the number of local variables of the latest version of the specified method.
        /// </summary>
        [PreserveSig]
        int GetLocalVariableCount(int methodToken, out int count);

        /// <summary>
        /// Gets local variables of the latest version of the specified method.
        /// </summary>
        [PreserveSig]
        int GetLocalVariables(
            int methodToken,
            int bufferLength,
            [In, Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] ISymUnmanagedVariable[] variables,
            out int count);

        [PreserveSig]
        int InitializeForEnc();

        /// <summary>
        /// Allows updating the line info for a method that has not been recompiled,
        /// but whose lines have moved independently.  A delta for each statement is allowed.
        /// </summary>
        [PreserveSig]
        int UpdateMethodLines(
            int methodToken,
            [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] int[] deltas,
            int count);
    }
}
