// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the License.txt file in the project root for more information.

using System;
using System.Runtime.InteropServices;

namespace Microsoft.DiaSymReader
{
    [Guid("AE932FBA-3FD8-4dba-8232-30A2309B02DB")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [GeneratedWhenPossibleComInterface]
    public partial interface ISymUnmanagedScope2 : ISymUnmanagedScope
    {
        // .NET 8+ COM source generators respect COM interface inheritance
        // so re-declaration of inherited method is not needed.
#if NETSTANDARD2_0
        #region ISymUnmanagedScope methods

        [PreserveSig]
        new int GetMethod([MarshalAs(UnmanagedType.Interface)] out ISymUnmanagedMethod method);

        [PreserveSig]
        new int GetParent([MarshalAs(UnmanagedType.Interface)] out ISymUnmanagedScope scope);

        [PreserveSig]
        new int GetChildren(
            int bufferLength,
            out int count,
            [In, Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] ISymUnmanagedScope[] children);

        [PreserveSig]
        new int GetStartOffset(out int offset);

        [PreserveSig]
        new int GetEndOffset(out int offset);

        [PreserveSig]
        new int GetLocalCount(out int count);

        [PreserveSig]
        new int GetLocals(
            int bufferLength,
            out int count,
            [In, Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] ISymUnmanagedVariable[] locals);

        [PreserveSig]
        new int GetNamespaces(
            int bufferLength,
            out int count,
            [In, Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] ISymUnmanagedNamespace[] namespaces);

        #endregion
#endif

        #region ISymUnmanagedScope2 methods

        [PreserveSig]
        int GetConstantCount(out int count);

        [PreserveSig]
        int GetConstants(
            int bufferLength,
            out int count,
            [In, Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] ISymUnmanagedConstant[] constants);

        #endregion
    }
}
