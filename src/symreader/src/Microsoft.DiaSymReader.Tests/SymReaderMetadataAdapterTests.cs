// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the License.txt file in the project root for more information.

using System;
using System.Reflection;
using System.Runtime.InteropServices;
using Roslyn.Test.Utilities;
using Xunit;

namespace Microsoft.DiaSymReader.UnitTests
{
    public class SymReaderMetadataAdapterTests
    {
        public sealed class SymReaderMetadataProvider1 : ISymReaderMetadataProvider
        {
            public unsafe static readonly byte* SignaturePtr;

            static unsafe SymReaderMetadataProvider1()
            {
                SignaturePtr = (byte*)Marshal.AllocHGlobal(10);
                SignaturePtr[0] = 0x11;
                SignaturePtr[1] = 0x12;
            }

            public unsafe bool TryGetStandaloneSignature(int standaloneSignatureToken, out byte* signature, out int length)
            {
                signature = SignaturePtr;
                length = 2;
                return true;
            }

            public bool TryGetTypeDefinitionInfo(int typeDefinitionToken, out string namespaceName, out string typeName, out TypeAttributes attributes)
            {
                namespaceName = "TDN";
                typeName = "td";
                attributes = TypeAttributes.BeforeFieldInit;
                return true;
            }

            public bool TryGetTypeReferenceInfo(int typeReferenceToken, out string namespaceName, out string typeName)
            {
                namespaceName = "TRN";
                typeName = "tr";
                return true;
            }
        }

        [Fact]
        public unsafe void GetTypeDefProps()
        {
            var provider = new SymReaderMetadataProvider1();
            var adapter = new SymReaderMetadataAdapter(provider);

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
                AssertEx.Equal(new char[] { 'x', 'x', 'T', 'D', 'N', '\0', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x' }, buffer);
                Assert.Equal(3, actualLength);
                Assert.Equal(TypeAttributes.BeforeFieldInit, attributes);
            }
        }

        [Fact]
        public unsafe void GetTypeRefProps()
        {
            var provider = new SymReaderMetadataProvider1();
            var adapter = new SymReaderMetadataAdapter(provider);

            var buffer = new char[15];
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = 'x';
            }

            fixed (char* bufferPtr = buffer)
            {
                int actualLength;
                Assert.Equal(HResult.S_OK, adapter.GetTypeRefProps(1, null, bufferPtr + 2, 4, &actualLength));
                AssertEx.Equal(new char[] { 'x', 'x', 'T', 'R', 'N', '\0', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x' }, buffer);
                Assert.Equal(3, actualLength);
            }
        }

        [Fact]
        public unsafe void GetSigFromToken()
        {
            var provider = new SymReaderMetadataProvider1();
            var adapter = new SymReaderMetadataAdapter(provider);

            var buffer = new char[15];
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = 'x';
            }

            int actualLength;
            byte* signature;
            Assert.Equal(HResult.S_OK, adapter.GetSigFromToken(1, &signature, &actualLength));
            Assert.True(signature == SymReaderMetadataProvider1.SignaturePtr);
            Assert.Equal(2, actualLength);
        }
    }
}
