// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Microsoft.IdentityModel.TestUtils;
using Xunit;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant
#pragma warning disable SYSLIB0028 // Type or member is obsolete
#pragma warning disable SYSLIB0027 // Type or member is obsolete

namespace Microsoft.IdentityModel.Tokens.Tests
{
    public class AsymmetricSignatureTests
    {
        [Fact]
        public void UnsupportedRSATypes()
        {
            var context = new CompareContext("UnsupportedRSATypes");
            TestUtilities.WriteHeader($"{this}.UnsupportedRSATypes");

#if NET461 || NET462 || NET472 || NET_CORE
            var expectedException = ExpectedException.NoExceptionExpected;
#endif
            try
            {
                new AsymmetricAdapter(new RsaSecurityKey(new DerivedRsa(2048)), SecurityAlgorithms.RsaSha256, false);
                expectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                expectedException.ProcessException(ex, context);
            }

#if NET461 || NET462 || NET472 || NET_CORE
            expectedException = ExpectedException.NoExceptionExpected;
#endif

            try
            {
                new AsymmetricAdapter(new RsaSecurityKey(new DerivedRsa(2048)), SecurityAlgorithms.RsaSha256, false);
                expectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                expectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        [Theory, MemberData(nameof(SignVerifyTheoryData))]
        public void SignVerify(SignatureProviderTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.SignVerify", theoryData);
            var bytes = Guid.NewGuid().ToByteArray();
            try
            {
                var providerForSigningDirect = new AsymmetricSignatureProvider(theoryData.SigningKey, theoryData.SigningAlgorithm, true);
                providerForSigningDirect.ValidKeySize();

                var providerForVerifyingDirect = new AsymmetricSignatureProvider(theoryData.VerifyKey, theoryData.VerifyAlgorithm, false);
                providerForVerifyingDirect.ValidKeySize();

                var providerForSigningFromFactory = theoryData.SigningKey.CryptoProviderFactory.CreateForSigning(theoryData.SigningKey, theoryData.SigningAlgorithm);
                var providerForVerifyingFromFactory = theoryData.VerifyKey.CryptoProviderFactory.CreateForVerifying(theoryData.VerifyKey, theoryData.VerifyAlgorithm);

                byte[] signatureDirect = providerForSigningDirect.Sign(bytes);
                byte[] signatureFromFactory = providerForSigningFromFactory.Sign(bytes);

                if (!providerForVerifyingDirect.Verify(bytes, signatureDirect))
                    context.AddDiff($"providerForVerifyingDirect.Verify (signatureDirect) - FAILED. signingKey : signingAlgorithm '{theoryData.SigningKey}' : '{theoryData.SigningAlgorithm}. verifyKey : verifyAlgorithm '{theoryData.VerifyKey}' : '{theoryData.VerifyAlgorithm}");

                if (!providerForVerifyingDirect.Verify(bytes, signatureFromFactory))
                    context.AddDiff($"providerForVerifyingDirect.Verify (signatureFromFactory) - FAILED. signingKey : signingAlgorithm '{theoryData.SigningKey}' : '{theoryData.SigningAlgorithm}. verifyKey : verifyAlgorithm '{theoryData.VerifyKey}' : '{theoryData.VerifyAlgorithm}");

                if (!providerForVerifyingFromFactory.Verify(bytes, signatureDirect))
                    context.AddDiff($"providerForVerifyingFromFactory.Verify (signatureDirect) - FAILED. signingKey : signingAlgorithm '{theoryData.SigningKey}' : '{theoryData.SigningAlgorithm}. verifyKey : verifyAlgorithm '{theoryData.VerifyKey}' : '{theoryData.VerifyAlgorithm}");

                if (!providerForVerifyingFromFactory.Verify(bytes, signatureFromFactory))
                    context.AddDiff($"providerForVerifyingFromFactory.Verify (signatureFromFactory) - FAILED. signingKey : signingAlgorithm '{theoryData.SigningKey}' : '{theoryData.SigningAlgorithm}. verifyKey : verifyAlgorithm '{theoryData.VerifyKey}' : '{theoryData.VerifyAlgorithm}");

                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<SignatureProviderTheoryData> SignVerifyTheoryData
        {
            get
            {
                var theoryData = new TheoryData<SignatureProviderTheoryData>();

                foreach (var certTuple in AsymmetricSignatureTestData.Certificates)
                    AsymmetricSignatureTestData.AddRsaAlgorithmVariations(new SignatureProviderTheoryData
                    {
                        SigningKey = new RsaSecurityKey(certTuple.Item1.PrivateKey as RSA),
                        TestId = "CapiCapi" + certTuple.Item3,
                        VerifyKey = new RsaSecurityKey(certTuple.Item2.PublicKey.Key as RSA)
                    },
                    theoryData);

#if NET461 || NET462 || NET472 || NET_CORE
                theoryData.Add(new SignatureProviderTheoryData()
                {
                    SigningAlgorithm = SecurityAlgorithms.RsaSsaPssSha512,
                    SigningKey = KeyingMaterial.RsaSecurityKey_1024,
                    VerifyKey = KeyingMaterial.RsaSecurityKey_1024_Public,
                    VerifyAlgorithm = SecurityAlgorithms.RsaSha512,
                    ExpectedException = ExpectedException.ArgumentOutOfRangeException(),
                    TestId = "KeySizeSmallerThanRequiredSize"
                });

                foreach (var certTuple in AsymmetricSignatureTestData.Certificates)
                    AsymmetricSignatureTestData.AddRsaAlgorithmVariations(new SignatureProviderTheoryData
                    {
                        SigningKey = new RsaSecurityKey(certTuple.Item1.PrivateKey as RSA),
                        TestId = "CapiCng" + certTuple.Item3,
                        VerifyKey = new RsaSecurityKey(certTuple.Item2.GetRSAPublicKey())
                    },
                    theoryData);

                foreach (var certTuple in AsymmetricSignatureTestData.Certificates)
                    AsymmetricSignatureTestData.AddRsaAlgorithmVariations(new SignatureProviderTheoryData
                    {
                        SigningKey = new RsaSecurityKey(certTuple.Item1.GetRSAPrivateKey()),
                        TestId = "CngCapi" + certTuple.Item3,
                        VerifyKey = new RsaSecurityKey(certTuple.Item2.PublicKey.Key as RSA)
                    },
                    theoryData);

                foreach (var certTuple in AsymmetricSignatureTestData.Certificates)
                    AsymmetricSignatureTestData.AddRsaAlgorithmVariations(new SignatureProviderTheoryData
                    {
                        SigningKey = new RsaSecurityKey(certTuple.Item1.GetRSAPrivateKey()),
                        TestId = "CngCng" + certTuple.Item3,
                        VerifyKey = new RsaSecurityKey(certTuple.Item2.GetRSAPublicKey())
                    },
                    theoryData);

                 foreach (var certTuple in AsymmetricSignatureTestData.Certificates)
                    AsymmetricSignatureTestData.AddRsaPssAlgorithmVariations(new SignatureProviderTheoryData
                    {
                        SigningKey = new RsaSecurityKey(certTuple.Item1.PrivateKey as RSA),
                        TestId = "CapiCapi" + certTuple.Item3,
                        VerifyKey = new RsaSecurityKey(certTuple.Item2.PublicKey.Key as RSA),
#if NET461 || NET462 || NET472
                        ExpectedException = ExpectedException.NotSupportedException("IDX10634:"),
#elif NET_CORE
                        ExpectedException = ExpectedException.NoExceptionExpected,
#endif
                    },
                    theoryData);

                foreach (var certTuple in AsymmetricSignatureTestData.Certificates)
                    AsymmetricSignatureTestData.AddRsaPssAlgorithmVariations(new SignatureProviderTheoryData
                    {
                        SigningKey = new RsaSecurityKey(certTuple.Item1.PrivateKey as RSA),
                        TestId = "CapiCng" + certTuple.Item3,
                        VerifyKey = new RsaSecurityKey(certTuple.Item2.GetRSAPublicKey()),
#if NET461 || NET462 || NET472
                        ExpectedException = ExpectedException.NotSupportedException("IDX10634:"),
#elif NET_CORE
                        ExpectedException = ExpectedException.NoExceptionExpected,
#endif
                    },
                    theoryData);

                foreach (var certTuple in AsymmetricSignatureTestData.Certificates)
                    AsymmetricSignatureTestData.AddRsaPssAlgorithmVariations(new SignatureProviderTheoryData
                    {
                        SigningKey = new RsaSecurityKey(certTuple.Item1.GetRSAPrivateKey()),
                        TestId = "CngCapi" + certTuple.Item3,
                        VerifyKey = new RsaSecurityKey(certTuple.Item2.PublicKey.Key as RSA),
#if NET461 || NET462 || NET472
                        ExpectedException = ExpectedException.NotSupportedException("IDX10634:"),
#elif NET_CORE
                        ExpectedException = ExpectedException.NoExceptionExpected,
#endif
                    },
                    theoryData);

                foreach (var certTuple in AsymmetricSignatureTestData.Certificates)
                    AsymmetricSignatureTestData.AddRsaPssAlgorithmVariations(new SignatureProviderTheoryData
                    {
                        SigningKey = new RsaSecurityKey(certTuple.Item1.GetRSAPrivateKey()),
                        TestId = "CngCng" + certTuple.Item3,
                        VerifyKey = new RsaSecurityKey(certTuple.Item2.GetRSAPublicKey())
                    },
                    theoryData);

                foreach (var jsonKeyTuple in AsymmetricSignatureTestData.JsonRsaSecurityKeys)
                    AsymmetricSignatureTestData.AddRsaPssAlgorithmVariations(new SignatureProviderTheoryData
                    {
                        SigningKey = jsonKeyTuple.Item1,
                        TestId = jsonKeyTuple.Item3,
                        VerifyKey = jsonKeyTuple.Item2
                    },
                    theoryData);

                foreach (var rsaKeyTuple in AsymmetricSignatureTestData.RsaSecurityKeys)
                    AsymmetricSignatureTestData.AddRsaPssAlgorithmVariations(new SignatureProviderTheoryData
                    {
                        SigningKey = rsaKeyTuple.Item1,
                        TestId = rsaKeyTuple.Item3,
                        VerifyKey = rsaKeyTuple.Item2
                    },
                    theoryData);

                foreach (var x509KeyTuple in AsymmetricSignatureTestData.X509SecurityKeys)
                    AsymmetricSignatureTestData.AddRsaPssAlgorithmVariations(new SignatureProviderTheoryData
                    {
                        SigningKey = x509KeyTuple.Item1,
                        TestId = x509KeyTuple.Item3,
                        VerifyKey = x509KeyTuple.Item2
                    },
                    theoryData);
#endif

                foreach (var ecdsaKeyTuple in AsymmetricSignatureTestData.ECDsaSecurityKeys)
                    AsymmetricSignatureTestData.AddECDsaAlgorithmVariations(new SignatureProviderTheoryData
                    {
                        SigningKey = ecdsaKeyTuple.Item1,
                        TestId = ecdsaKeyTuple.Item3,
                        VerifyKey = ecdsaKeyTuple.Item2
                    },
                    theoryData);

                foreach (var jsonKeyTuple in AsymmetricSignatureTestData.JsonECDsaSecurityKeys)
                    AsymmetricSignatureTestData.AddECDsaAlgorithmVariations(new SignatureProviderTheoryData
                    {
                        SigningKey = jsonKeyTuple.Item1,
                        TestId = jsonKeyTuple.Item3,
                        VerifyKey = jsonKeyTuple.Item2
                    },
                    theoryData);

                foreach (var jsonKeyTuple in AsymmetricSignatureTestData.JsonRsaSecurityKeys)
                    AsymmetricSignatureTestData.AddRsaAlgorithmVariations(new SignatureProviderTheoryData
                    {
                        SigningKey = jsonKeyTuple.Item1,
                        TestId = jsonKeyTuple.Item3,
                        VerifyKey = jsonKeyTuple.Item2
                    },
                    theoryData);

                foreach (var rsaKeyTuple in AsymmetricSignatureTestData.RsaSecurityKeys)
                    AsymmetricSignatureTestData.AddRsaAlgorithmVariations(new SignatureProviderTheoryData
                    {
                        SigningKey = rsaKeyTuple.Item1,
                        TestId = rsaKeyTuple.Item3,
                        VerifyKey = rsaKeyTuple.Item2
                    },
                    theoryData);

                foreach (var x509KeyTuple in AsymmetricSignatureTestData.X509SecurityKeys)
                    AsymmetricSignatureTestData.AddRsaAlgorithmVariations(new SignatureProviderTheoryData
                    {
                        SigningKey = x509KeyTuple.Item1,
                        TestId = x509KeyTuple.Item3,
                        VerifyKey = x509KeyTuple.Item2
                    },
                    theoryData);

                return theoryData;
            }
        }

        [Theory, MemberData(nameof(ValidateAsymmetricKeySizeTheoryData))]
        public void VerifyAsymmetricKeySize(AsymmetricSignatureProviderTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.VerifyAsymmetricKeySize", theoryData);

            try
            {
                theoryData.AsymmetricSignatureProvider.ValidateAsymmetricSecurityKeySize(theoryData.SecurityKey, theoryData.Algorithm, theoryData.WillCreateSignatures);
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<AsymmetricSignatureProviderTheoryData> ValidateAsymmetricKeySizeTheoryData
        {
            get => new TheoryData<AsymmetricSignatureProviderTheoryData>
            {
                new AsymmetricSignatureProviderTheoryData
                {
                    Algorithm = SecurityAlgorithms.RsaSha256,
                    ExpectedException = ExpectedException.ArgumentOutOfRangeException("IDX10630:"),
                    SecurityKey = KeyingMaterial.RsaSecurityKey_1024,
                    TestId = nameof(KeyingMaterial.RsaSecurityKey_1024),
                    WillCreateSignatures = true
                },
                new AsymmetricSignatureProviderTheoryData
                {
                    Algorithm = SecurityAlgorithms.RsaSha256,
                    SecurityKey = KeyingMaterial.RsaSecurityKey_1024_Public,
                    TestId = nameof(KeyingMaterial.RsaSecurityKey_1024_Public),
                    WillCreateSignatures = false
                },
                new AsymmetricSignatureProviderTheoryData
                {
                    Algorithm = SecurityAlgorithms.RsaSha256,
                    ExpectedException = ExpectedException.ArgumentOutOfRangeException("IDX10630:"),
                    SecurityKey = KeyingMaterial.JsonWebKeyRsa_1024,
                    TestId = nameof(KeyingMaterial.JsonWebKeyRsa_1024),
                    WillCreateSignatures = true
                },
                new AsymmetricSignatureProviderTheoryData
                {
                    Algorithm = SecurityAlgorithms.RsaSha256,
                    SecurityKey = KeyingMaterial.JsonWebKeyRsa_1024_Public,
                    TestId = nameof(KeyingMaterial.JsonWebKeyRsa_1024_Public),
                    WillCreateSignatures = false
                },
                new AsymmetricSignatureProviderTheoryData
                {
                    Algorithm = SecurityAlgorithms.RsaSha256,
                    SecurityKey = KeyingMaterial.JsonWebKeyRsa_2048,
                    TestId = nameof(KeyingMaterial.JsonWebKeyRsa_2048),
                    WillCreateSignatures = true
                },
                new AsymmetricSignatureProviderTheoryData
                {
                    Algorithm = SecurityAlgorithms.RsaSha256,
                    SecurityKey = KeyingMaterial.JsonWebKeyRsa_2048_Public,
                    TestId = nameof(KeyingMaterial.JsonWebKeyRsa_2048_Public),
                    WillCreateSignatures = false
                },
                new AsymmetricSignatureProviderTheoryData
                {
                    Algorithm = SecurityAlgorithms.RsaSha256,
                    ExpectedException = ExpectedException.NotSupportedException("IDX10704:"),
                    SecurityKey = KeyingMaterial.SymmetricSecurityKey2_1024,
                    TestId = nameof(KeyingMaterial.SymmetricSecurityKey2_1024),
                    WillCreateSignatures = false
                },
                new AsymmetricSignatureProviderTheoryData
                {
                    Algorithm = SecurityAlgorithms.RsaSha256,
                    ExpectedException = ExpectedException.NotSupportedException("IDX10704:"),
                    SecurityKey = KeyingMaterial.JsonWebKeySymmetric128,
                    TestId = nameof(KeyingMaterial.JsonWebKeySymmetric128),
                    WillCreateSignatures = false
                },
                new AsymmetricSignatureProviderTheoryData
                {
                    Algorithm = SecurityAlgorithms.EcdsaSha256,
                    SecurityKey = KeyingMaterial.Ecdsa256Key,
                    TestId = nameof(KeyingMaterial.Ecdsa256Key),
                    WillCreateSignatures = true
                },
                new AsymmetricSignatureProviderTheoryData
                {
                    Algorithm = SecurityAlgorithms.EcdsaSha256,
                    SecurityKey = KeyingMaterial.Ecdsa256Key_Public,
                    TestId = nameof(KeyingMaterial.Ecdsa256Key_Public),
                    WillCreateSignatures = false
                }
            };
        }

        /// <summary>
        /// This test ensures that if every algorithm in SupportedAlgorithms has a value in our maps that validate key sizes
        /// </summary>
        /// <param name="theoryData"></param>
        [Theory, MemberData(nameof(VerifyAlgorithmsInDefaultMinimumAsymmetricKeySizeTests))]
        public void VerifyAlgorithmsInDefaultMinimumAsymmetricKeySize(AsymmetricSignatureProviderTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.VerifyAlgorithmsInDefaultMinimumAsymmetricKeySize", theoryData);
            if (!AsymmetricSignatureProvider.DefaultMinimumAsymmetricKeySizeInBitsForSigningMap.ContainsKey(theoryData.Algorithm))
                context.AddDiff($"!AsymmetricSignatureProvider.DefaultMinimumAsymmetricKeySizeInBitsForSigningMap.ContainsKey(theoryData.Algorithm)) algorithm: '{theoryData.Algorithm}'.");

            if (!AsymmetricSignatureProvider.DefaultMinimumAsymmetricKeySizeInBitsForVerifyingMap.ContainsKey(theoryData.Algorithm))
                context.AddDiff($"!AsymmetricSignatureProvider.DefaultMinimumAsymmetricKeySizeInBitsForVerifyingMap.ContainsKey(theoryData.Algorithm)): algorithm: '{theoryData.Algorithm}'.");

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<AsymmetricSignatureProviderTheoryData> VerifyAlgorithmsInDefaultMinimumAsymmetricKeySizeTests
        {
            get
            {
                var theoryData = new TheoryData<AsymmetricSignatureProviderTheoryData>();

                foreach (var algorithm in SupportedAlgorithms.EcdsaSigningAlgorithms)
                    theoryData.Add(
                        new AsymmetricSignatureProviderTheoryData
                        {
                            Algorithm = algorithm,
                            SecurityKey = KeyingMaterial.RsaSecurityKey_4096,
                            TestId = algorithm
                        });

                foreach (var algorithm in SupportedAlgorithms.RsaPssSigningAlgorithms)
                    theoryData.Add(
                        new AsymmetricSignatureProviderTheoryData
                        {
                            Algorithm = algorithm,
                            SecurityKey = KeyingMaterial.RsaSecurityKey_4096,
                            TestId = algorithm
                        });


                foreach (var algorithm in SupportedAlgorithms.RsaSigningAlgorithms)
                    theoryData.Add(
                        new AsymmetricSignatureProviderTheoryData
                        {
                            Algorithm = algorithm,
                            SecurityKey = KeyingMaterial.RsaSecurityKey_4096,
                            TestId = algorithm
                        });

                return theoryData;
            }
        }

        /// <summary>
        /// This test ensures that if new keys sizes are added to the dictionaries that check for default supported algorithms, we have those algorithms in SupportedAlgorithms
        /// </summary>
        [Fact]
        public void VerifyDefaultMinimumAsymmetricKeySizeAreSupported()
        {
            var theoryData = new TheoryDataBase
            {
                TestId = "VerifyDefaultMinimumAsymmetricKeySizeAreSupported"
            };

            var context = TestUtilities.WriteHeader($"{this}.VerifyDefaultMinimumAsymmetricKeySizeAreSupported", theoryData);

            foreach (var algorithm in AsymmetricSignatureProvider.DefaultMinimumAsymmetricKeySizeInBitsForSigningMap.Keys)
                if (!(SupportedAlgorithms.EcdsaSigningAlgorithms.Contains(algorithm) || SupportedAlgorithms.RsaPssSigningAlgorithms.Contains(algorithm) || SupportedAlgorithms.RsaSigningAlgorithms.Contains(algorithm)))
                {
                    context.AddDiff($"DefaultMinimumAsymmetricKeySizeInBitsForSigningMap, algorithm: '{algorithm}' not found in (SupportedAlgorithms.EcdsaSigningAlgorithms || SupportedAlgorithms.RsaPssSigningAlgorithms || SupportedAlgorithms.RsaSigningAlgorithms.");
                    context.AddDiff($"seems like algorithm was added somewhere: '{algorithm}'.");
                }

            foreach (var algorithm in AsymmetricSignatureProvider.DefaultMinimumAsymmetricKeySizeInBitsForVerifyingMap.Keys)
                if (!(SupportedAlgorithms.EcdsaSigningAlgorithms.Contains(algorithm) || SupportedAlgorithms.RsaPssSigningAlgorithms.Contains(algorithm) || SupportedAlgorithms.RsaSigningAlgorithms.Contains(algorithm)))
                {
                    context.AddDiff($"DefaultMinimumAsymmetricKeySizeInBitsForVerifyingMap, algorithm: '{algorithm}' not found in (SupportedAlgorithms.EcdsaSigningAlgorithms || SupportedAlgorithms.RsaPssSigningAlgorithms || SupportedAlgorithms.RsaSigningAlgorithms");
                    context.AddDiff($"seems like algorithm was added somewhere: '{algorithm}'.");
                }

            TestUtilities.AssertFailIfErrors(context);
        }
    }

    public class AsymmetricSignatureProviderTheoryData : TheoryDataBase
    {
        public string Algorithm { get; set; }

        public AsymmetricSignatureProvider AsymmetricSignatureProvider { get; set; } = new AsymmetricSignatureProvider(KeyingMaterial.RsaSecurityKey_2048, SecurityAlgorithms.RsaSha256);

        public SecurityKey SecurityKey { get; set; }

        public bool WillCreateSignatures { get; set; }
    }
}

#pragma warning restore SYSLIB0027 // Type or member is obsolete
#pragma warning restore SYSLIB0028 // Type or member is obsolete
#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
