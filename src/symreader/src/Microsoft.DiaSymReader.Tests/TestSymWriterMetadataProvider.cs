// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the License.txt file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Reflection;

namespace Microsoft.DiaSymReader.UnitTests
{
    internal class TestSymWriterMetadataProvider : ISymWriterMetadataProvider, ISymReaderMetadataProvider
    {
        private readonly Dictionary<int, (string Name, int DeclaringType)> _methods;
        private readonly Dictionary<int, (string Namespace, string Name, TypeAttributes Attributes)> _types;

        public TestSymWriterMetadataProvider(
            Dictionary<int, (string Name, int DeclaringType)> methods, 
            Dictionary<int, (string Namespace, string Name, TypeAttributes Attributes)> types)
        {
            _methods = methods;
            _types = types;
        }

        public bool TryGetEnclosingType(int nestedTypeToken, out int enclosingTypeToken)
        {
            enclosingTypeToken = 0;
            return false;
        }

        public bool TryGetMethodInfo(int methodDefinitionToken, out string methodName, out int declaringTypeToken)
        {
            var result = _methods.TryGetValue(methodDefinitionToken, out var entry);
            methodName = entry.Name;
            declaringTypeToken = entry.DeclaringType;
            return result;
        }

        public bool TryGetTypeDefinitionInfo(int typeDefinitionToken, out string namespaceName, out string typeName, out TypeAttributes attributes)
        {
            var result = _types.TryGetValue(typeDefinitionToken, out var entry);
            namespaceName = entry.Namespace;
            typeName = entry.Name;
            attributes = entry.Attributes;
            return result;
        }

        public unsafe bool TryGetStandaloneSignature(int standaloneSignatureToken, out byte* signature, out int length)
        {
            throw new NotImplementedException();
        }

        public bool TryGetTypeReferenceInfo(int typeReferenceToken, out string namespaceName, out string typeName)
        {
            throw new NotImplementedException();
        }
    }
}
