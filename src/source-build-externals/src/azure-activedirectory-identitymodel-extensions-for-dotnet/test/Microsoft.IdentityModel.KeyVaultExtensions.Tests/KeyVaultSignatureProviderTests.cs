// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Linq;
using Microsoft.Azure.KeyVault;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.KeyVaultExtensions;
using Xunit;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant

namespace Microsoft.IdentityModel.KeyVaultExtensions.Tests
{
    public class KeyVaultSignatureProviderTests
    {
        private readonly MockKeyVaultClient _client;
        private readonly SecurityKey _key;

        public KeyVaultSignatureProviderTests()
        {
            _client = new MockKeyVaultClient();
            _key = new KeyVaultSecurityKey(KeyVaultUtilities.CreateKeyIdentifier(), keySize: default);
        }

        [Theory, MemberData(nameof(DisposeProviderTheoryData))]
        public void DisposeProviderTest(SignatureProviderTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.DisposeProviderTest", theoryData);

            try
            {
                var provider = new KeyVaultSignatureProvider(_key, theoryData.Algorithm, willCreateSignatures: true, _client);
                _key.CryptoProviderFactory.ReleaseSignatureProvider(provider);

                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception exception)
            {
                theoryData.ExpectedException.ProcessException(exception, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<SignatureProviderTheoryData> DisposeProviderTheoryData
        {
            get => new TheoryData<SignatureProviderTheoryData>
            {
                new SignatureProviderTheoryData
                {
                    Algorithm = SecurityAlgorithms.RsaSha256,
                    ExpectedException = ExpectedException.NoExceptionExpected,
                    First = true,
                    TestId = nameof(SecurityAlgorithms.RsaSha256),
                },
                new SignatureProviderTheoryData
                {
                    Algorithm = SecurityAlgorithms.RsaSha384,
                    ExpectedException = ExpectedException.NoExceptionExpected,
                    TestId = nameof(SecurityAlgorithms.RsaSha384),
                },
                new SignatureProviderTheoryData
                {
                    Algorithm = SecurityAlgorithms.RsaSha512,
                    ExpectedException = ExpectedException.NoExceptionExpected,
                    TestId = nameof(SecurityAlgorithms.RsaSha512),
                },
            };
        }

        [Theory, MemberData(nameof(SignatureProviderTheoryData))]
        public void SignatureTest(SignatureProviderTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.SignatureTest", theoryData);

            try
            {
                var provider = new KeyVaultSignatureProvider(_key, theoryData.Algorithm, willCreateSignatures: true, _client);
                if (provider == null)
                    context.AddDiff("(provider == null)");

                var input = Guid.NewGuid().ToByteArray();
                var signature = provider.Sign(input);

                if (signature == null)
                    context.AddDiff("(signature == null)");

                if (_client.ExpectedSignatureLength != signature.Length)
                    context.AddDiff($"_client.ExpectedSignatureLength != signature.Length. == {_client.ExpectedSignatureLength}, {signature.Length}.");

                if (!provider.Verify(input, signature))
                    context.AddDiff("!provider.Verify(input, signature)");

                var tamperedInput = new byte[input.Length];
                input.CopyTo(tamperedInput, 0);
                if (tamperedInput[0] == byte.MaxValue)
                    tamperedInput[0]--;
                else
                    tamperedInput[0]++;

                if (provider.Verify(tamperedInput, signature))
                    context.AddDiff("provider.Verify(tamperedInput, signature)");

                foreach (var data in SignatureProviderTheoryData)
                {
                    var newAlgorithm = (data.Single() as SignatureProviderTheoryData)?.Algorithm;
                    if (string.IsNullOrEmpty(newAlgorithm))
                        continue; // Skip invalid input

                    // Check that a given Security Key will only validate a signature using the same hash algorithm.
                    var isValidSignature = new KeyVaultSignatureProvider(_key, newAlgorithm, willCreateSignatures: false, _client).Verify(input, signature);
                    if (StringComparer.Ordinal.Equals(theoryData.Algorithm, newAlgorithm))
                    {
                        if (!isValidSignature)
                            context.AddDiff("Signature should have been valid, isValidSignature == false");
                    }
                    else if (isValidSignature)
                        context.AddDiff("Signature should NOT have been valid, isValidSignature == true");
                }

                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception exception)
            {
                theoryData.ExpectedException.ProcessException(exception, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<SignatureProviderTheoryData> SignatureProviderTheoryData
        {
            get => new TheoryData<SignatureProviderTheoryData>
            {
                new SignatureProviderTheoryData
                {
                    Algorithm = null,
                    ExpectedException = ExpectedException.ArgumentNullException(),
                    First = true,
                    TestId = "NullAlgorithm",
                },
                new SignatureProviderTheoryData
                {
                    Algorithm = string.Empty,
                    ExpectedException = ExpectedException.ArgumentNullException(),
                    TestId = "EmptyAlgorithm",
                },
                new SignatureProviderTheoryData
                {
                    Algorithm = SecurityAlgorithms.RsaSha256,
                    TestId = nameof(SecurityAlgorithms.RsaSha256),
                },
                new SignatureProviderTheoryData
                {
                    Algorithm = SecurityAlgorithms.RsaSha384,
                    TestId = nameof(SecurityAlgorithms.RsaSha384),
                },
                new SignatureProviderTheoryData
                {
                    Algorithm = SecurityAlgorithms.RsaSha512,
                    TestId = nameof(SecurityAlgorithms.RsaSha512),
                },
            };
        }
    }
}

#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
