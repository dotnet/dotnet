// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.TestUtils;
using Xunit;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant

namespace Microsoft.IdentityModel.Tokens.Tests
{
    /// <summary>
    /// Crypto extensibility scenarios
    /// </summary>
    public class CryptoExtensibilityTests
    {
        /// <summary>
        /// SecurityTokenDescriptor.CryptoProviderFactory has priority over SecurityKey.CryptoProviderFactory
        /// </summary>
        [Theory, MemberData(nameof(SecurityTokenDescriptorDataSet))]
        public void CryptoProviderOrderingWhenSigning(SecurityTokenDescriptor tokenDescriptor)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwt = tokenHandler.CreateEncodedJwt(tokenDescriptor);

            if (tokenDescriptor.SigningCredentials.CryptoProviderFactory == null)
            {
                Assert.True((tokenDescriptor.SigningCredentials.Key.CryptoProviderFactory as CustomCryptoProviderFactory).CreateForSigningCalled);
                Assert.True((tokenDescriptor.SigningCredentials.Key.CryptoProviderFactory as CustomCryptoProviderFactory).ReleaseSignatureProviderCalled);
                Assert.True(((tokenDescriptor.SigningCredentials.Key.CryptoProviderFactory as CustomCryptoProviderFactory).SigningSignatureProvider as CustomSignatureProvider).SignCalled);
                Assert.True(((tokenDescriptor.SigningCredentials.Key.CryptoProviderFactory as CustomCryptoProviderFactory).SigningSignatureProvider as CustomSignatureProvider).DisposeCalled);
            }
            else
            {
                Assert.True((tokenDescriptor.SigningCredentials.CryptoProviderFactory as CustomCryptoProviderFactory).CreateForSigningCalled);
                Assert.True((tokenDescriptor.SigningCredentials.CryptoProviderFactory as CustomCryptoProviderFactory).ReleaseSignatureProviderCalled);
                Assert.True(((tokenDescriptor.SigningCredentials.CryptoProviderFactory as CustomCryptoProviderFactory).SigningSignatureProvider as CustomSignatureProvider).SignCalled);
                Assert.True(((tokenDescriptor.SigningCredentials.CryptoProviderFactory as CustomCryptoProviderFactory).SigningSignatureProvider as CustomSignatureProvider).DisposeCalled);
                Assert.False((tokenDescriptor.SigningCredentials.Key.CryptoProviderFactory as CustomCryptoProviderFactory).CreateForSigningCalled);
                Assert.False((tokenDescriptor.SigningCredentials.Key.CryptoProviderFactory as CustomCryptoProviderFactory).ReleaseSignatureProviderCalled);
                Assert.False(((tokenDescriptor.SigningCredentials.Key.CryptoProviderFactory as CustomCryptoProviderFactory).SigningSignatureProvider as CustomSignatureProvider).SignCalled);
                Assert.False(((tokenDescriptor.SigningCredentials.Key.CryptoProviderFactory as CustomCryptoProviderFactory).SigningSignatureProvider as CustomSignatureProvider).DisposeCalled);
            }
        }

        public static TheoryData<SecurityTokenDescriptor> SecurityTokenDescriptorDataSet
        {
            get
            {
                var dataset = new TheoryData<SecurityTokenDescriptor>();

                var key = new SymmetricSecurityKey(new byte[256]);
                key.CryptoProviderFactory = new CustomCryptoProviderFactory()
                {
                    SigningSignatureProvider = new CustomSignatureProvider(key, "alg")
                };

                var tokenDescriptor = Default.SecurityTokenDescriptor(new SigningCredentials(key, "alg"));

                dataset.Add(tokenDescriptor);

                key = new SymmetricSecurityKey(new byte[256]);
                key.CryptoProviderFactory = new CustomCryptoProviderFactory()
                {
                    SigningSignatureProvider = new CustomSignatureProvider(key, "alg")
                };

                tokenDescriptor = Default.SecurityTokenDescriptor(new SigningCredentials(key, "alg"));
                tokenDescriptor.SigningCredentials.CryptoProviderFactory = new CustomCryptoProviderFactory()
                {
                    SigningSignatureProvider = new CustomSignatureProvider(key, "alg")
                };

                dataset.Add(tokenDescriptor);
                return dataset;
            }
        }

        /// <summary>
        /// TokenValidationParameters.CryptoProviderFactory has priority over SecurityKey.CryptoProviderFactory
        /// </summary>
        [Theory, MemberData(nameof(SigningCredentialsDataSet))]
        public void CryptoProviderOrderingWhenVerifying(string testId, TokenValidationParameters validationParameters, string jwt)
        {
            TestUtilities.WriteHeader("CryptoProviderOrderingWhenVerifying - " + testId, true);
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = null;
            tokenHandler.ValidateToken(jwt, validationParameters, out token);

            if (validationParameters.CryptoProviderFactory == null)
            {
                Assert.True((validationParameters.IssuerSigningKey.CryptoProviderFactory as CustomCryptoProviderFactory).CreateForVerifyingCalled, "IssuerSigningKey.CustomCryptoProviderFactory.CreateForVerifyingCalled was NOT called");
                Assert.True((validationParameters.IssuerSigningKey.CryptoProviderFactory as CustomCryptoProviderFactory).ReleaseSignatureProviderCalled, "IssuerSigningKey.CustomCryptoProviderFactory.ReleaseSignatureProviderCalled was NOT called");
                Assert.True(((validationParameters.IssuerSigningKey.CryptoProviderFactory as CustomCryptoProviderFactory).VerifyingSignatureProvider as CustomSignatureProvider).VerifyCalled, "IssuerSigningKey.CustomCryptoProviderFactory.VerifyCalled was NOT called");
                Assert.True(((validationParameters.IssuerSigningKey.CryptoProviderFactory as CustomCryptoProviderFactory).VerifyingSignatureProvider as CustomSignatureProvider).DisposeCalled, "IssuerSigningKey.CustomCryptoProviderFactory.DisposeCalled was NOT called");
            }
            else
            {
                Assert.True((validationParameters.CryptoProviderFactory as CustomCryptoProviderFactory).CreateForVerifyingCalled, "validationParameters.CustomCryptoProviderFactory.CreateForVerifyingCalled was NOT called");
                Assert.True((validationParameters.CryptoProviderFactory as CustomCryptoProviderFactory).ReleaseSignatureProviderCalled, "validationParameters.CustomCryptoProviderFactory.ReleaseSignatureProviderCalled was NOT called");
                Assert.True(((validationParameters.CryptoProviderFactory as CustomCryptoProviderFactory).VerifyingSignatureProvider as CustomSignatureProvider).VerifyCalled, "validationParameters.CustomSignatureProvider.VerifyCalled was NOT called");
                Assert.True(((validationParameters.CryptoProviderFactory as CustomCryptoProviderFactory).VerifyingSignatureProvider as CustomSignatureProvider).DisposeCalled, "validationParameters.CustomSignatureProvider.DisposeCalled was NOT called");
                Assert.False((validationParameters.IssuerSigningKey.CryptoProviderFactory as CustomCryptoProviderFactory).CreateForVerifyingCalled, "IssuerSigningKey.CustomCryptoProviderFactory.CreateForVerifyingCalled WAS called");
                Assert.False((validationParameters.IssuerSigningKey.CryptoProviderFactory as CustomCryptoProviderFactory).ReleaseSignatureProviderCalled, "IssuerSigningKey.CustomCryptoProviderFactory.ReleaseSignatureProviderCalled was WAS called");
                Assert.False(((validationParameters.IssuerSigningKey.CryptoProviderFactory as CustomCryptoProviderFactory).VerifyingSignatureProvider as CustomSignatureProvider).VerifyCalled, "IssuerSigningKey.CustomSignatureProvider.VerifyCalled was WAS called");
                Assert.False(((validationParameters.IssuerSigningKey.CryptoProviderFactory as CustomCryptoProviderFactory).VerifyingSignatureProvider as CustomSignatureProvider).DisposeCalled, "IssuerSigningKey.CustomSignatureProvider.DisposeCalled was WAS called");
            }
        }

        public static TheoryData<string, TokenValidationParameters, string> SigningCredentialsDataSet
        {
            get
            {
                var dataset = new TheoryData<string, TokenValidationParameters, string>();

                var validationParameters = Default.AsymmetricSignTokenValidationParameters;
                validationParameters.IssuerSigningKey.CryptoProviderFactory = new CustomCryptoProviderFactory(new string[] { "RS256" })
                {
                    SigningSignatureProvider = new CustomSignatureProvider(validationParameters.IssuerSigningKey, "alg"),
                    VerifyingSignatureProvider = new CustomSignatureProvider(validationParameters.IssuerSigningKey, "alg")
                };

                dataset.Add("Test1", validationParameters, Default.AsymmetricJwt);

                validationParameters = Default.AsymmetricSignTokenValidationParameters;
                validationParameters.CryptoProviderFactory = new CustomCryptoProviderFactory(new string[] { "RS256" })
                {
                    SigningSignatureProvider = new CustomSignatureProvider(validationParameters.IssuerSigningKey, "alg"),
                    VerifyingSignatureProvider = new CustomSignatureProvider(validationParameters.IssuerSigningKey, "alg")
                };


                // this is only set to check that it wasn't called
                validationParameters.IssuerSigningKey.CryptoProviderFactory = new CustomCryptoProviderFactory()
                {
                    SigningSignatureProvider = new CustomSignatureProvider(validationParameters.IssuerSigningKey, "alg"),
                    VerifyingSignatureProvider = new CustomSignatureProvider(validationParameters.IssuerSigningKey, "alg")
                };

                dataset.Add("Test2", validationParameters, Default.AsymmetricJwt);

                return dataset;
            }
        }

        /// <summary>
        /// Tests that Default behaviors
        /// </summary>
        [Theory]
        [InlineData(SecurityAlgorithms.Sha256, true)]
        [InlineData(SecurityAlgorithms.Sha256Digest, true)]
        [InlineData(SecurityAlgorithms.Sha384, true)]
        [InlineData(SecurityAlgorithms.Sha384Digest, true)]
        [InlineData(SecurityAlgorithms.Sha512, true)]
        [InlineData(SecurityAlgorithms.Sha512Digest, true)]
        [InlineData(SecurityAlgorithms.Aes128Encryption, false)]
        public void DefaultCryptoProviderFactoryGetHashAlgorithm(string algorithm, bool isSupported)
        {
            var ee = isSupported ? ExpectedException.NoExceptionExpected : ExpectedException.NotSupportedException("IDX10640:");
            try
            {
                CryptoProviderFactory.Default.CreateHashAlgorithm(algorithm);
                ee.ProcessNoException();
            }
            catch(Exception ex)
            {
                ee.ProcessException(ex);
            }
        }

        /// <summary>
        /// Tests that setting a <see cref="ICryptoProvider"/> does not colide with defaults.
        /// </summary>
        [Fact]
        public void CustomCryptoProvider()
        {
            var cryptoProviderFactoryDefault = CryptoProviderFactory.Default;
            var customCryptoProvider = new CustomCryptoProvider
            {
                HashAlgorithm = new CustomHashAlgorithm(),
                SignatureProvider = new CustomSignatureProvider(KeyingMaterial.RsaSecurityKey_2048, SecurityAlgorithms.RsaSha256),
                IsSupportedResult = true
            };

            var cryptoProviderFactoryWithCustomProvider = new CustomCryptoProviderFactory
            {
                CustomCryptoProvider = customCryptoProvider
            };

            var cryptoProviderFactoryDefault2 = CryptoProviderFactory.Default;

            Assert.Null(cryptoProviderFactoryDefault.CustomCryptoProvider);
            Assert.Null(cryptoProviderFactoryDefault2.CustomCryptoProvider);
            Assert.NotNull(cryptoProviderFactoryWithCustomProvider.CustomCryptoProvider);

            cryptoProviderFactoryDefault.CreateForSigning(KeyingMaterial.RsaSecurityKey_2048, SecurityAlgorithms.RsaSha256);
            var customSignatureProvider = cryptoProviderFactoryWithCustomProvider.CreateForSigning(KeyingMaterial.RsaSecurityKey_2048, SecurityAlgorithms.RsaSha256) as CustomSignatureProvider;
            var customHashAlgorithm = cryptoProviderFactoryWithCustomProvider.CreateHashAlgorithm(SecurityAlgorithms.Sha256) as CustomHashAlgorithm;

            cryptoProviderFactoryWithCustomProvider.ReleaseSignatureProvider(customSignatureProvider);
            cryptoProviderFactoryWithCustomProvider.ReleaseHashAlgorithm(customHashAlgorithm);

            Assert.NotNull(customSignatureProvider);
            Assert.NotNull(customHashAlgorithm);
            Assert.True(cryptoProviderFactoryWithCustomProvider.ReleaseSignatureProviderCalled, "cryptoProviderFactoryWithCustomProvider.ReleaseSignatureProviderCalled");
            Assert.True(cryptoProviderFactoryWithCustomProvider.ReleaseHashAlgorithmCalled, "cryptoProviderFactoryWithCustomProvider.ReleaseAlgorithmCalled");
            Assert.True(customCryptoProvider.IsSupportedAlgorithmCalled, "customCryptoProvider.IsSupportedAlgorithmCalled");
            Assert.True(customCryptoProvider.ReleaseCalled, "customCryptoProvider.ReleaseCalled");
            Assert.True(customCryptoProvider.CreateCalled, "customCryptoProvider.CreateCalled");
            Assert.True(customSignatureProvider.DisposeCalled, "customSignatureProvider.DisposeCalled");
            Assert.False(customHashAlgorithm.DisposeCalled, "customHashAlgorithm.DisposeCalled");
        }

        [Theory, MemberData(nameof(CreateSignatureProviderExtensibilityTheoryData))]
        public void CreateSignatureProviderExtensibility(CryptoProviderFactoryTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.CreateSignatureProviderExtensibility", theoryData);

            try
            {
                theoryData.CryptoProviderFactory.CreateForSigning(theoryData.SigningKey, theoryData.SigningAlgorithm);
                theoryData.CryptoProviderFactory.CreateForVerifying(theoryData.VerifyKey, theoryData.VerifyAlgorithm);
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<CryptoProviderFactoryTheoryData> CreateSignatureProviderExtensibilityTheoryData
        {
            get => new TheoryData<CryptoProviderFactoryTheoryData>
            {
                // These are in the order processed by the method

                // CustomCryptoProvider
                // IsSupported
                // !IsSupported
                // SignatureProvider==Null
                // SignatureProvider!=null
                new CryptoProviderFactoryTheoryData
                {
                    SigningAlgorithm = SecurityAlgorithms.RsaSha256Signature,
                    CryptoProviderFactory = new CryptoProviderFactory{ CustomCryptoProvider = new CustomCryptoProvider(new string[] { SecurityAlgorithms.RsaSha256Signature }) },
                    ExpectedException = ExpectedException.InvalidOperationException("IDX10646:"),
                    SigningKey = KeyingMaterial.X509SecurityKey_1024,
                    TestId = $"Extensibility1"
                },
                new CryptoProviderFactoryTheoryData
                {
                    SigningAlgorithm = SecurityAlgorithms.HmacSha256Signature,
                    CryptoProviderFactory = new CryptoProviderFactory{ CustomCryptoProvider = new CustomCryptoProvider(new string[] { SecurityAlgorithms.HmacSha256Signature }) },
                    ExpectedException = ExpectedException.InvalidOperationException("IDX10646:"),
                    SigningKey = KeyingMaterial.DefaultSymmetricSecurityKey_56,
                    TestId = $"Extensibility2",
                },
                new CryptoProviderFactoryTheoryData
                {
                    SigningAlgorithm = "SecurityAlgorithms.HmacSha256Signature",
                    CryptoProviderFactory = new CryptoProviderFactory{ CustomCryptoProvider = new CustomCryptoProvider(new string[] { "SecurityAlgorithms.HmacSha256Signature" })
                    { SignatureProvider = new SymmetricSignatureProvider(KeyingMaterial.DefaultSymmetricSecurityKey_256, SecurityAlgorithms.HmacSha256) } },
                    SigningKey = KeyingMaterial.DefaultSymmetricSecurityKey_256,
                    VerifyAlgorithm = "SecurityAlgorithms.HmacSha256Signature",
                    VerifyKey = KeyingMaterial.DefaultSymmetricSecurityKey_256,
                    TestId = $"Extensibility3",
                },
                new CryptoProviderFactoryTheoryData
                {
                    SigningAlgorithm = "SecurityAlgorithms.HmacSha256Signature",
                    CryptoProviderFactory = new CryptoProviderFactory{ CustomCryptoProvider = new CustomCryptoProvider(new string[] { "!SecurityAlgorithms.HmacSha256Signature" }) },
                    ExpectedException = ExpectedException.NotSupportedException("IDX10634:"),
                    SigningKey = KeyingMaterial.DefaultSymmetricSecurityKey_256,
                    VerifyAlgorithm = "SecurityAlgorithms.HmacSha256Signature",
                    VerifyKey = KeyingMaterial.DefaultSymmetricSecurityKey_256,
                    TestId = $"Extensibility4"
                },
            };
        }
    }
}

#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
