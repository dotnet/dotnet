// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the License.txt file in the project root for more information.

using System;
using System.Runtime.InteropServices;

namespace Microsoft.DiaSymReader
{
    [ComImport]
    [Guid("5da320c8-9c2c-4e5a-b823-027e0677b359")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComVisible(false)]
    public interface ISymUnmanagedMethod2 : ISymUnmanagedMethod
    {
        #region ISymUnmanagedMethod methods

        [PreserveSig]
        new int GetToken(out int methodToken);

        [PreserveSig]
        new int GetSequencePointCount(out int count);

        [PreserveSig]
        new int GetRootScope([MarshalAs(UnmanagedType.Interface)] out ISymUnmanagedScope scope);

        [PreserveSig]
        new int GetScopeFromOffset(int offset, [MarshalAs(UnmanagedType.Interface)] out ISymUnmanagedScope scope);

        /// <summary>
        /// Gets the IL offset within the method that corresponds to the specified position.
        /// </summary>
        /// <param name="document">The document for which the offset is requested. </param>
        /// <param name="line">The document line corresponding to the offset. </param>
        /// <param name="column">The document column corresponding to the offset. </param>
        /// <param name="offset">The offset within the specified document.</param>
        /// <returns>HResult.</returns>
        [PreserveSig]
        new int GetOffset(ISymUnmanagedDocument document, int line, int column, out int offset);

        /// <summary>
        /// Gets an array of start and end offset pairs that correspond to the ranges of IL that a given position covers within this method.
        /// </summary>
        [PreserveSig]
        new int GetRanges(
            ISymUnmanagedDocument document,
            int line,
            int column,
            int bufferLength,
            out int count,
            [In, Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] int[] ranges);

        /// <summary>
        /// Gets method parameters.
        /// </summary>
        [PreserveSig]
        new int GetParameters(
            int bufferLength,
            out int count,
            [In, Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] ISymUnmanagedVariable[] parameters);

        [PreserveSig]
        new int GetNamespace([MarshalAs(UnmanagedType.Interface)] out ISymUnmanagedNamespace @namespace);

        /// <summary>
        /// Gets the start and end positions for the source of the current method.
        /// </summary>
        /// <param name="documents">The starting and ending source documents.</param>
        /// <param name="lines">The starting and ending lines in the corresponding source documents. </param>
        /// <param name="columns">The starting and ending columns in the corresponding source documents. </param>
        /// <param name="defined">true if the positions were defined; otherwise, false.</param>
        /// <returns>HResult</returns>
        [PreserveSig]
        new int GetSourceStartEnd(
            ISymUnmanagedDocument[] documents,
            [In, Out, MarshalAs(UnmanagedType.LPArray)] int[] lines,
            [In, Out, MarshalAs(UnmanagedType.LPArray)] int[] columns,
            [MarshalAs(UnmanagedType.Bool)]out bool defined);

        [PreserveSig]
        new int GetSequencePoints(
            int bufferLength,
            out int count,
            [In, Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] int[] offsets,
            [In, Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] ISymUnmanagedDocument[] documents,
            [In, Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] int[] startLines,
            [In, Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] int[] startColumns,
            [In, Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] int[] endLines,
            [In, Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] int[] endColumns);

        #endregion

        #region ISymUnmanagedMethod2 methods

        /// <summary>
        /// Get the token of the local signature.
        /// </summary>
        /// <param name="localSignatureToken">Local signature token (StandAloneSig), or 0 if the method doesn't have any local variables.</param>
        /// <returns>
        /// S_OK if the method has a local signature,
        /// S_FALSE if the method doesn't have a local signature, 
        /// E_* if an error occurs while reading the signature.
        /// </returns>
        [PreserveSig]
        int GetLocalSignatureToken(out int localSignatureToken);

        #endregion
    }
}
