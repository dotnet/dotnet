﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the License.txt file in the project root for more information.

using System;
using System.Runtime.InteropServices;

namespace Microsoft.DiaSymReader
{
    [Guid("B20D55B3-532E-4906-87E7-25BD5734ABD2")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComVisible(false)]
    [GeneratedWhenPossibleComInterface]
    public partial interface ISymUnmanagedAsyncMethod
    {
        [PreserveSig]
        int IsAsyncMethod([MarshalAs(UnmanagedType.Bool)]out bool value);

        [PreserveSig]
        int GetKickoffMethod(out int kickoffMethodToken);

        [PreserveSig]
        int HasCatchHandlerILOffset([MarshalAs(UnmanagedType.Bool)]out bool offset);

        [PreserveSig]
        int GetCatchHandlerILOffset(out int offset);

        [PreserveSig]
        int GetAsyncStepInfoCount(out int count);

        [PreserveSig]
        int GetAsyncStepInfo(
            int bufferLength,
            out int count,
            [In, Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] int[] yieldOffsets,
            [In, Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] int[] breakpointOffset,
            [In, Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] int[] breakpointMethod);
    }
}
