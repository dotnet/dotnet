// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the License.txt file in the project root for more information.

using System.Reflection;

namespace Microsoft.DiaSymReader
{
    public interface ISymReaderMetadataProvider
    {
        /// <summary>
        /// Returns a pointer to signature blob corresponding to the specified token.
        /// </summary>
        /// <returns>Bytes or null, if the signature is not available.</returns>
        unsafe bool TryGetStandaloneSignature(int standaloneSignatureToken, out byte* signature, out int length);

        // only needed for portable:
        bool TryGetTypeDefinitionInfo(int typeDefinitionToken, out string namespaceName, out string typeName, out TypeAttributes attributes);
        bool TryGetTypeReferenceInfo(int typeReferenceToken, out string namespaceName, out string typeName);
    }
}
