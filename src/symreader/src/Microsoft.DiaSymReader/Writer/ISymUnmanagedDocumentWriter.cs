// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the License.txt file in the project root for more information.

#pragma warning disable 436 // SuppressUnmanagedCodeSecurityAttribute defined in source and mscorlib 

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Microsoft.DiaSymReader
{
    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("B01FAFEB-C450-3A4D-BEEC-B4CEEC01E006"), SuppressUnmanagedCodeSecurity]
    internal interface ISymUnmanagedDocumentWriter
    {
        void SetSource(uint sourceSize, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] byte[] source);
        void SetCheckSum(Guid algorithmId, uint checkSumSize, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] byte[] checkSum);
    }
}
