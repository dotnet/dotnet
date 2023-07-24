// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

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
