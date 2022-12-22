// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the License.txt file in the project root for more information.

using System.Runtime.InteropServices;
using System.Reflection;
using System.Diagnostics;

namespace Microsoft.DiaSymReader
{
    /// <summary>
    /// Minimal implementation of IMetadataImport that implements APIs used by SymReader.
    /// </summary>
    internal unsafe sealed class SymReaderMetadataAdapter : MetadataAdapterBase
    {
        private readonly ISymReaderMetadataProvider _metadataProvider;

        public SymReaderMetadataAdapter(ISymReaderMetadataProvider metadataProvider)
        {
            Debug.Assert(metadataProvider != null);
            _metadataProvider = metadataProvider;
        }

        public override int GetSigFromToken(
            int standaloneSignature, 
            [Out]byte** signature,
            [Out]int* signatureLength)
        {
            // Happens when a constant doesn't have a signature:
            // The caller expect the signature to have at least one byte on success, 
            // so we need to fail here. Otherwise AV happens.
            var hr = _metadataProvider.TryGetStandaloneSignature(standaloneSignature, out byte* sig, out int length) ? HResult.S_OK : HResult.E_INVALIDARG;

            if (signature != null)
            {
                *signature = sig;
            }

            if (signatureLength != null)
            {
                *signatureLength = length;
            }

            return hr;
        }

        public override int GetTypeDefProps(
            int typeDef,
            [Out]char* qualifiedName,
            int qualifiedNameBufferLength,
            [Out]int* qualifiedNameLength,
            [Out]TypeAttributes* attributes,
            [Out]int* baseType)
        {
            if (!_metadataProvider.TryGetTypeDefinitionInfo(typeDef, out var namespaceName, out var typeName, out var typeAttributes))
            {
                return HResult.E_INVALIDARG;
            }

            if (qualifiedNameLength != null || qualifiedName != null)
            {
                InteropUtilities.CopyQualifiedTypeName(
                    qualifiedName,
                    qualifiedNameBufferLength,
                    qualifiedNameLength,
                    namespaceName,
                    typeName);
            }

            if (attributes != null)
            {
                *attributes = typeAttributes;
            }

            if (baseType != null)
            {
                // unused
                *baseType = 0;
            }

            return HResult.S_OK;
        }

        public override int GetTypeRefProps(
            int typeRef,
            [Out]int* resolutionScope, // ModuleRef or AssemblyRef
            [Out]char* qualifiedName,
            int qualifiedNameBufferLength,
            [Out]int* qualifiedNameLength)
        {
            if (!_metadataProvider.TryGetTypeReferenceInfo(typeRef, out var namespaceName, out var typeName))
            {
                return HResult.E_INVALIDARG;
            }

            if (qualifiedNameLength != null || qualifiedName != null)
            {
                InteropUtilities.CopyQualifiedTypeName(
                    qualifiedName,
                    qualifiedNameBufferLength,
                    qualifiedNameLength,
                    namespaceName,
                    typeName);
            }

            if (resolutionScope != null)
            {
                // unused
                *resolutionScope = 0;
            }

            return HResult.S_OK;
        }
    }
}
