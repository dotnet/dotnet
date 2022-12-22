//------------------------------------------------------------------------------
//
// Copyright (c) Microsoft Corporation.
// All rights reserved.
//
// This code is licensed under the MIT License.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files(the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and / or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions :
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
//------------------------------------------------------------------------------

using System;
using Microsoft.Azure.KeyVault;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens;
using Xunit;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant

namespace Microsoft.IdentityModel.KeyVaultExtensions.Tests
{
    public class KeyVaultKeyWrapProviderTests
    {
        private readonly MockKeyVaultClient _client;
        private readonly SecurityKey _key;

        public KeyVaultKeyWrapProviderTests()
        {
            _client = new MockKeyVaultClient();
            _key = new KeyVaultSecurityKey(KeyVaultUtilities.CreateKeyIdentifier(), keySize: default);
        }

        [Theory, MemberData(nameof(DisposeProviderTheoryData))]
        public void DisposeProviderTest(KeyWrapProviderTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.DisposeProviderTest", theoryData);

            try
            {
                var provider = new KeyVaultKeyWrapProvider(_key, theoryData.Algorithm, _client);
                _key.CryptoProviderFactory.ReleaseKeyWrapProvider(provider);

                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception exception)
            {
                theoryData.ExpectedException.ProcessException(exception, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<KeyWrapProviderTheoryData> DisposeProviderTheoryData
        {
            get => new TheoryData<KeyWrapProviderTheoryData>
            {
                new KeyWrapProviderTheoryData
                {
                    Algorithm = SecurityAlgorithms.RsaPKCS1,
                    ExpectedException = ExpectedException.NoExceptionExpected,
                    First = true,
                    TestId = nameof(SecurityAlgorithms.RsaPKCS1),
                },
                new KeyWrapProviderTheoryData
                {
                    Algorithm = SecurityAlgorithms.RsaOAEP,
                    ExpectedException = ExpectedException.NoExceptionExpected,
                    TestId = nameof(SecurityAlgorithms.RsaOAEP),
                },
            };
        }

        [Theory, MemberData(nameof(KeyWrapProviderTheoryData))]
        public void WrapUnwrapKeyTest(KeyWrapProviderTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.WrapUnwrapKeyTest", theoryData);

            try
            {
                var provider = new KeyVaultKeyWrapProvider(_key, theoryData.Algorithm, _client);
                if (provider == null)
                    context.AddDiff("(provider == null)");

                var keyBytes = Guid.NewGuid().ToByteArray();
                var wrappedKey = provider.WrapKey(keyBytes);
                if (wrappedKey == null)
                    context.AddDiff("(wrappedKey == null)");

                if (_client.ExpectedKeyWrapLength != wrappedKey.Length)
                    context.AddDiff($"_client.ExpectedKeyWrapLength != wrappedKey.Length. {_client.ExpectedKeyWrapLength} != {wrappedKey.Length}");

                if (Utility.AreEqual(keyBytes, wrappedKey))
                    context.AddDiff("Utility.AreEqual(keyBytes, wrappedKey)");

                var unwrappedKey = provider.UnwrapKey(wrappedKey);
                if (unwrappedKey == null)
                    context.AddDiff("(unwrappedKey == null)");

                IdentityComparer.AreBytesEqual(keyBytes, unwrappedKey, context);

                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception exception)
            {
                theoryData.ExpectedException.ProcessException(exception, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<KeyWrapProviderTheoryData> KeyWrapProviderTheoryData
        {
            get => new TheoryData<KeyWrapProviderTheoryData>
            {
                new KeyWrapProviderTheoryData
                {
                    Algorithm = null,
                    ExpectedException = ExpectedException.ArgumentNullException(),
                    First = true,
                    TestId = "NullAlgorithm",
                },
                new KeyWrapProviderTheoryData
                {
                    Algorithm = string.Empty,
                    ExpectedException = ExpectedException.ArgumentNullException(),
                    TestId = "EmptyAlgorithm",
                },
                new KeyWrapProviderTheoryData
                {
                    Algorithm = SecurityAlgorithms.RsaPKCS1,
                    TestId = nameof(SecurityAlgorithms.RsaPKCS1),
                },
                new KeyWrapProviderTheoryData
                {
                    Algorithm = SecurityAlgorithms.RsaOAEP,
                    TestId = nameof(SecurityAlgorithms.RsaOAEP),
                },
            };
        }
    }
}

#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
