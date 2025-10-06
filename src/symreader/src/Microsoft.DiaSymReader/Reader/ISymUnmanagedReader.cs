﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the License.txt file in the project root for more information.

using System;
using System.Runtime.InteropServices;
#if NET9_0_OR_GREATER
using System.Runtime.InteropServices.Marshalling;
#endif

namespace Microsoft.DiaSymReader
{
    [Guid("B4CE6286-2A6B-3712-A3B7-1EE1DAD467B5")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComVisible(false)]
    [GeneratedWhenPossibleComInterface]
    public partial interface ISymUnmanagedReader
    {
        [PreserveSig]
        int GetDocument(
            [MarshalAs(UnmanagedType.LPWStr)] string url,
            Guid language,
            Guid languageVendor,
            Guid documentType,
            [MarshalAs(UnmanagedType.Interface)] out ISymUnmanagedDocument document);

        [PreserveSig]
        int GetDocuments(
            int bufferLength,
            out int count,
            [In, Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] ISymUnmanagedDocument[] documents);

        [PreserveSig]
        int GetUserEntryPoint(out int methodToken);

        [PreserveSig]
        int GetMethod(int methodToken, [MarshalAs(UnmanagedType.Interface)] out ISymUnmanagedMethod method);

        [PreserveSig]
        int GetMethodByVersion(
            int methodToken,
            int version,
            [MarshalAs(UnmanagedType.Interface)] out ISymUnmanagedMethod method);

        [PreserveSig]
        int GetVariables(
            int methodToken,
            int bufferLength,
            out int count,
            [In, Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] ISymUnmanagedVariable[] variables);

        [PreserveSig]
        int GetGlobalVariables(
            int bufferLength,
            out int count,
            [In, Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] ISymUnmanagedVariable[] variables);

        [PreserveSig]
        int GetMethodFromDocumentPosition(
            ISymUnmanagedDocument document,
            int line,
            int column,
            [MarshalAs(UnmanagedType.Interface)] out ISymUnmanagedMethod method);

        [PreserveSig]
        int GetSymAttribute(
            int methodToken,
            [MarshalAs(UnmanagedType.LPWStr)] string name,
            int bufferLength,
            out int count,
            [In, Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] byte[] customDebugInformation);

        [PreserveSig]
        int GetNamespaces(
            int bufferLength,
            out int count,
            [In, Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] ISymUnmanagedNamespace[] namespaces);

        [PreserveSig]
        int Initialize(
            [MarshalAs(UnmanagedType.Interface)] object metadataImporter,
            [MarshalAs(UnmanagedType.LPWStr)] string fileName,
            [MarshalAs(UnmanagedType.LPWStr)] string searchPath,
#if NET9_0_OR_GREATER
            [MarshalUsing(typeof(ComStreamWrapper.Marshaller))]
#endif
            System.Runtime.InteropServices.ComTypes.IStream stream);

        [PreserveSig]
        int UpdateSymbolStore([MarshalAs(UnmanagedType.LPWStr)] string fileName,
#if NET9_0_OR_GREATER
            [MarshalUsing(typeof(ComStreamWrapper.Marshaller))]
#endif
            System.Runtime.InteropServices.ComTypes.IStream stream);

        [PreserveSig]
        int ReplaceSymbolStore([MarshalAs(UnmanagedType.LPWStr)] string fileName,
#if NET9_0_OR_GREATER
            [MarshalUsing(typeof(ComStreamWrapper.Marshaller))]
#endif
            System.Runtime.InteropServices.ComTypes.IStream stream);

        [PreserveSig]
        int GetSymbolStoreFileName(
            int bufferLength,
            out int count,
            [In, Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] char[] name);

        [PreserveSig]
        int GetMethodsFromDocumentPosition(
            ISymUnmanagedDocument document,
            int line,
            int column,
            int bufferLength,
            out int count,
            [In, Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] ISymUnmanagedMethod[] methods);

        [PreserveSig]
        int GetDocumentVersion(ISymUnmanagedDocument document, out int version, [MarshalAs(UnmanagedType.Bool)]out bool isCurrent);

        [PreserveSig]
        int GetMethodVersion(ISymUnmanagedMethod method, out int version);
    }
}
