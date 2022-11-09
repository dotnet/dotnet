// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the License.txt file in the project root for more information.

using System;
using System.Runtime.InteropServices;

namespace Microsoft.DiaSymReader
{
    [ComImport]
    [Guid("9F60EEBE-2D9A-3F7C-BF58-80BC991C60BB")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComVisible(false)]
    public interface ISymUnmanagedVariable
    {
        [PreserveSig]
        int GetName(
            int bufferLength,
            out int count,
            [In, Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] char[] name);

        [PreserveSig]
        int GetAttributes(out int attributes);

        [PreserveSig]
        int GetSignature(
            int bufferLength,
            out int count,
            [In, Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] byte[] signature);

        [PreserveSig]
        int GetAddressKind(out int kind);

        [PreserveSig]
        int GetAddressField1(out int value);

        [PreserveSig]
        int GetAddressField2(out int value);

        [PreserveSig]
        int GetAddressField3(out int value);

        [PreserveSig]
        int GetStartOffset(out int offset);

        [PreserveSig]
        int GetEndOffset(out int offset);
    }
}
