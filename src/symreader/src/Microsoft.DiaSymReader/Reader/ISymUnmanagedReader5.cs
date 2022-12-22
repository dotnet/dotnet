// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the License.txt file in the project root for more information.

using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace Microsoft.DiaSymReader
{
    [ComImport]
    [Guid("6576c987-7e8d-4298-a6e1-6f9783165f07")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComVisible(false)]
    public interface ISymUnmanagedReader5 : ISymUnmanagedReader4
    {
        #region ISymUnmanagedReader methods

        [PreserveSig]
        new int GetDocument(
            [MarshalAs(UnmanagedType.LPWStr)] string url,
            Guid language,
            Guid languageVendor,
            Guid documentType,
            [MarshalAs(UnmanagedType.Interface)] out ISymUnmanagedDocument document);

        [PreserveSig]
        new int GetDocuments(
            int bufferLength,
            out int count,
            [In, Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] ISymUnmanagedDocument[] documents);

        [PreserveSig]
        new int GetUserEntryPoint(out int methodToken);

        [PreserveSig]
        new int GetMethod(int methodToken, [MarshalAs(UnmanagedType.Interface)] out ISymUnmanagedMethod method);

        [PreserveSig]
        new int GetMethodByVersion(
            int methodToken,
            int version,
            [MarshalAs(UnmanagedType.Interface)] out ISymUnmanagedMethod method);

        [PreserveSig]
        new int GetVariables(
            int methodToken,
            int bufferLength,
            out int count,
            [In, Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] ISymUnmanagedVariable[] variables);

        [PreserveSig]
        new int GetGlobalVariables(
            int bufferLength,
            out int count,
            [In, Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] ISymUnmanagedVariable[] variables);

        [PreserveSig]
        new int GetMethodFromDocumentPosition(
            ISymUnmanagedDocument document,
            int line,
            int column,
            [MarshalAs(UnmanagedType.Interface)] out ISymUnmanagedMethod method);

        [PreserveSig]
        new int GetSymAttribute(
            int methodToken,
            [MarshalAs(UnmanagedType.LPWStr)] string name,
            int bufferLength,
            out int count,
            [In, Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] byte[] customDebugInformation);

        [PreserveSig]
        new int GetNamespaces(
            int bufferLength,
            out int count,
            [In, Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] ISymUnmanagedNamespace[] namespaces);

        [PreserveSig]
        new int Initialize(
            [MarshalAs(UnmanagedType.Interface)] object metadataImporter,
            [MarshalAs(UnmanagedType.LPWStr)] string fileName,
            [MarshalAs(UnmanagedType.LPWStr)] string searchPath,
            IStream stream);

        [PreserveSig]
        new int UpdateSymbolStore([MarshalAs(UnmanagedType.LPWStr)] string fileName, IStream stream);

        [PreserveSig]
        new int ReplaceSymbolStore([MarshalAs(UnmanagedType.LPWStr)] string fileName, IStream stream);

        [PreserveSig]
        new int GetSymbolStoreFileName(
            int bufferLength,
            out int count,
            [In, Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] char[] name);

        [PreserveSig]
        new int GetMethodsFromDocumentPosition(
            ISymUnmanagedDocument document,
            int line,
            int column,
            int bufferLength,
            out int count,
            [In, Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] ISymUnmanagedMethod[] methods);

        [PreserveSig]
        new int GetDocumentVersion(ISymUnmanagedDocument document, out int version, [MarshalAs(UnmanagedType.Bool)]out bool isCurrent);

        [PreserveSig]
        new int GetMethodVersion(ISymUnmanagedMethod method, out int version);

        #endregion

        #region ISymUnmanagedReader2 methods

        [PreserveSig]
        new int GetMethodByVersionPreRemap(
            int methodToken,
            int version,
            [MarshalAs(UnmanagedType.Interface)] out ISymUnmanagedMethod method);

        [PreserveSig]
        new int GetSymAttributePreRemap(
            int methodToken,
            [MarshalAs(UnmanagedType.LPWStr)] string name,
            int bufferLength,
            out int count,
            [In, Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] byte[] customDebugInformation);

        [PreserveSig]
        new int GetMethodsInDocument(
            ISymUnmanagedDocument document,
            int bufferLength,
            out int count,
            [In, Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] ISymUnmanagedMethod[] methods);

        #endregion

        #region ISymUnmanagedReader3 methods

        /// <summary>
        /// Gets a custom debug information based upon its name and an EnC 1-based version number. 
        /// </summary>
        [PreserveSig]
        new int GetSymAttributeByVersion(
            int methodToken,
            int version,
            [MarshalAs(UnmanagedType.LPWStr)] string name,
            int bufferLength,
            out int count,
            [In, Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] byte[] customDebugInformation);

        /// <summary>
        /// Gets a custom debug information based upon its name and an EnC 1-based version number. 
        /// </summary>
        [PreserveSig]
        new int GetSymAttributeByVersionPreRemap(
            int methodToken,
            int version,
            [MarshalAs(UnmanagedType.LPWStr)] string name,
            int bufferLength,
            out int count,
            [In, Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] byte[] customDebugInformation);

        #endregion

        #region ISymUnmanagedReader4 methods

        /// <summary>
        /// Checks whether the id stored in the PDB matches the PDB ID stored in the PE/COFF Debug Directory.
        /// </summary>
        [PreserveSig]
        new int MatchesModule(Guid guid, uint stamp, int age, [MarshalAs(UnmanagedType.Bool)]out bool result);

        /// <summary>
        /// Returns a pointer to Portable Debug Metadata. Only available for Portable PDBs.
        /// </summary>
        /// <param name="metadata">
        /// A pointer to memory where Portable Debug Metadata start. The memory is owned by the SymReader and 
        /// valid until <see cref="ISymUnmanagedDispose.Destroy"/> is invoked. 
        /// 
        /// Null if the PDB is not portable.
        /// </param>
        /// <param name="size">Size of the metadata block.</param>
        /// <returns>
        /// S_OK if the PDB is portable, S_FALSE if it isn't.
        /// </returns>
        /// <remarks>
        /// If the store was updated via <see cref="UpdateSymbolStore(string, IStream)"/> 
        /// returns the metadata of the latest update.
        /// </remarks>
        [PreserveSig]
        new unsafe int GetPortableDebugMetadata(out byte* metadata, out int size);

        /// <summary>
        /// Returns a pointer to Source Server data stored in the PDB.
        /// </summary>
        /// <param name="data">
        /// A pointer to memory where Source Server data start. The memory is owned by the SymReader and 
        /// valid until <see cref="ISymUnmanagedDispose.Destroy"/> is invoked. 
        /// 
        /// Null if the PDB doesn't contain Source Server data.
        /// </param>
        /// <param name="size">Size of the data in bytes.</param>
        /// <returns>
        /// S_OK if the PDB contains Source Server data, S_FALSE if it doesn't.
        /// </returns>
        [PreserveSig]
        new unsafe int GetSourceServerData(out byte* data, out int size);

        #endregion

        #region ISymUnmanagedReader5 methods

        /// <summary>
        /// Returns a pointer to Portable Debug Metadata of the specified version (EnC generation). Only available for Portable PDBs.
        /// </summary>
        /// <param name="version">
        /// EnC 1-based version number. Version 1 corresponds to the baseline.
        /// </param>
        /// <param name="metadata">
        /// A pointer to memory where Portable Debug Metadata start. The memory is owned by the SymReader and 
        /// valid until <see cref="ISymUnmanagedDispose.Destroy"/> is invoked. 
        /// 
        /// Null if the PDB is not portable.
        /// </param>
        /// <param name="size">Size of the metadata block.</param>
        [PreserveSig]
        unsafe int GetPortableDebugMetadataByVersion(int version, out byte* metadata, out int size);

        #endregion
    }
}
