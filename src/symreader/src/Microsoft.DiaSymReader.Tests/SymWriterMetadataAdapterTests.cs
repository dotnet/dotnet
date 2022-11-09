// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the License.txt file in the project root for more information.

using System;
using System.Reflection;
using Roslyn.Test.Utilities;
using Xunit;

namespace Microsoft.DiaSymReader.UnitTests
{
    public class SymWriterMetadataAdapterTests
    {
        public sealed class SymWriterMetadataProvider1 : ISymWriterMetadataProvider
        {
            public bool TryGetEnclosingType(int nestedTypeToken, out int enclosingTypeToken)
            {
                enclosingTypeToken = 11;
                return true;
            }

            public bool TryGetMethodInfo(int methodDefinitionToken, out string methodName, out int declaringTypeToken)
            {
                methodName = "Method";
                declaringTypeToken = 123;
                return true;
            }

            public bool TryGetTypeDefinitionInfo(int typeDefinitionToken, out string namespaceName, out string typeName, out TypeAttributes attributes)
            {
                namespaceName = "Namespace";
                typeName = "Type";
                attributes = TypeAttributes.Abstract;
                return true;
            }
        }

        [Fact]
        public unsafe void GetTypeDefProps()
        {
            var provider = new SymWriterMetadataProvider1();
            var adapter = new SymWriterMetadataAdapter(provider);

            var buffer = new char[15];
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = 'x';
            }

            fixed (char* bufferPtr = buffer)
            {
                int actualLength;
                TypeAttributes attributes;
                Assert.Equal(HResult.S_OK, adapter.GetTypeDefProps(1, bufferPtr + 2, 4, &actualLength, &attributes, null));
                AssertEx.Equal(new char[] { 'x', 'x', 'N', 'a', 'm', '\0', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x' }, buffer);
                Assert.Equal(3, actualLength);
                Assert.Equal(TypeAttributes.Abstract, attributes);
            }
        }

        [Fact]
        public unsafe void GetMethodProps()
        {
            var provider = new SymWriterMetadataProvider1();
            var adapter = new SymWriterMetadataAdapter(provider);

            var buffer = new char[15];
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = 'x';
            }

            fixed (char* bufferPtr = buffer)
            {
                int actualLength;
                int declaringTypeDef;
                Assert.Equal(HResult.S_OK, adapter.GetMethodProps(1, &declaringTypeDef, bufferPtr + 2, 4, &actualLength, null, null, null, null, null));
                AssertEx.Equal(new char[] { 'x', 'x', 'M', 'e', 't', '\0', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x' }, buffer);
                Assert.Equal(3, actualLength);
                Assert.Equal(123, declaringTypeDef);
            }
        }

        [Fact]
        public unsafe void GetNestedClassProps()
        {
            var provider = new SymWriterMetadataProvider1();
            var adapter = new SymWriterMetadataAdapter(provider);

            var buffer = new char[15];
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = 'x';
            }

            fixed (char* bufferPtr = buffer)
            {
                Assert.Equal(HResult.S_OK, adapter.GetNestedClassProps(1, out int enclosingClass));
                Assert.Equal(11, enclosingClass);
            }
        }
    }
}
