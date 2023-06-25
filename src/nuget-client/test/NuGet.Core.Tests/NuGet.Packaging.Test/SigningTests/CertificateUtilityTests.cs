// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using NuGet.Common;
using NuGet.Packaging.Signing;
using NuGet.Test.Utility;
using Org.BouncyCastle.Asn1.X509;
using Test.Utility.Signing;
using Xunit;

namespace NuGet.Packaging.Test
{
    [Collection(SigningTestsCollection.Name)]
    public class CertificateUtilityTests
    {
        private readonly CertificatesFixture _fixture;

        public CertificateUtilityTests(CertificatesFixture fixture)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            _fixture = fixture;
        }

        [Theory]
        [InlineData(Common.HashAlgorithmName.SHA256, Oids.Sha256WithRSAEncryption)]
        [InlineData(Common.HashAlgorithmName.SHA384, Oids.Sha384WithRSAEncryption)]
        [InlineData(Common.HashAlgorithmName.SHA512, Oids.Sha512WithRSAEncryption)]
        public void IsSignatureAlgorithmSupported_WhenSupported_ReturnsTrue(Common.HashAlgorithmName algorithm, string expectedSignatureAlgorithmOid)
        {
            using (var certificate = SigningTestUtility.GenerateCertificate(
                "test",
                generator => { },
                algorithm,
                RSASignaturePaddingMode.Pkcs1))
            {
                Assert.Equal(expectedSignatureAlgorithmOid, certificate.SignatureAlgorithm.Value);
                Assert.True(CertificateUtility.IsSignatureAlgorithmSupported(certificate));
            }
        }

        [Fact]
        public void IsSignatureAlgorithmSupported_WhenUnsupported_ReturnsFalse()
        {
            using (var certificate = _fixture.GetRsaSsaPssCertificate())
            {
                // RSASSA-PSS
                Assert.Equal("1.2.840.113549.1.1.10", certificate.SignatureAlgorithm.Value);
                Assert.False(CertificateUtility.IsSignatureAlgorithmSupported(certificate));
            }
        }

        [Fact]
        public void IsCertificatePublicKeyValid_WhenNotRsaSsaPkcsV1_5_ReturnsTrue()
        {
            using (var certificate = SigningTestUtility.GenerateCertificate(
                "test",
                generator => { },
                Common.HashAlgorithmName.SHA256,
                RSASignaturePaddingMode.Pss,
                publicKeyLength: 2048))
            {
                Assert.True(CertificateUtility.IsCertificatePublicKeyValid(certificate));
            }
        }

        [Fact]
        public void IsCertificatePublicKeyValid_WhenRsaSsaPkcsV1_5_1024Bits_ReturnsFalse()
        {
            using (var certificate = SigningTestUtility.GenerateCertificate(
                "test",
                generator => { },
                publicKeyLength: 1024))
            {
                Assert.False(CertificateUtility.IsCertificatePublicKeyValid(certificate));
            }
        }

        [Fact]
        public void IsCertificatePublicKeyValid_WhenRsaSsaPkcsV1_5_2048Bits_ReturnsTrue()
        {
            using (var certificate = SigningTestUtility.GenerateCertificate(
                "test",
                generator => { },
                publicKeyLength: 2048))
            {
                Assert.True(CertificateUtility.IsCertificatePublicKeyValid(certificate));
            }
        }

        [Fact]
        public void IsCertificateValidityPeriodInTheFuture_WithValidityStartInTheFuture_ReturnsTrue()
        {
            using (var certificate = SigningTestUtility.GenerateCertificate(
                "test",
                modifyGenerator: SigningTestUtility.CertificateModificationGeneratorNotYetValidCert))
            {
                Assert.True(CertificateUtility.IsCertificateValidityPeriodInTheFuture(certificate));
            }
        }

        [Fact]
        public void IsCertificateValidityPeriodInTheFuture_WithValidityStartInThePast_ReturnsFalse()
        {
            using (var certificate = SigningTestUtility.GenerateCertificate(
                "test",
                generator => { }))
            {
                Assert.False(CertificateUtility.IsCertificateValidityPeriodInTheFuture(certificate));
            }
        }

        [Fact]
        public void GetCertificateChain_ReturnsCertificatesInOrder()
        {
            using (X509ChainHolder chainHolder = X509ChainHolder.CreateForCodeSigning())
            using (var rootCertificate = SigningTestUtility.GetCertificate("root.crt"))
            using (var intermediateCertificate = SigningTestUtility.GetCertificate("intermediate.crt"))
            using (var leafCertificate = SigningTestUtility.GetCertificate("leaf.crt"))
            {
                IX509Chain chain = chainHolder.Chain2;

                chain.ChainPolicy.ExtraStore.Add(rootCertificate);
                chain.ChainPolicy.ExtraStore.Add(intermediateCertificate);

                chain.Build(leafCertificate);

                using (IX509CertificateChain certificateChain = CertificateChainUtility.GetCertificateChain(chain.PrivateReference))
                {
                    Assert.Equal(3, certificateChain.Count);
                    Assert.Equal(leafCertificate.Thumbprint, certificateChain[0].Thumbprint);
                    Assert.Equal(intermediateCertificate.Thumbprint, certificateChain[1].Thumbprint);
                    Assert.Equal(rootCertificate.Thumbprint, certificateChain[2].Thumbprint);
                }
            }
        }

        [Fact]
        public void HasLifetimeSigningEku_WithLifetimeSignerEku_ReturnsTrue()
        {
            using (var certificate = _fixture.GetLifetimeSigningCertificate())
            {
                Assert.Equal(1, GetExtendedKeyUsageCount(certificate));
                Assert.True(CertificateUtility.HasLifetimeSigningEku(certificate));
            }
        }

        [Fact]
        public void HasLifetimeSigningEku_WithoutLifetimeSignerEku_ReturnsFalse()
        {
            using (var certificate = SigningTestUtility.GenerateCertificate("test", generator => { }))
            {
                Assert.Equal(0, GetExtendedKeyUsageCount(certificate));
                Assert.False(CertificateUtility.HasLifetimeSigningEku(certificate));
            }
        }

        [Fact]
        public void HasExtendedKeyUsage_WithEku_ReturnsTrueForEku()
        {
            using (var certificate = SigningTestUtility.GenerateCertificate("test",
                generator =>
                {
                    var usages = new OidCollection { new Oid(Oids.CodeSigningEku) };

                    generator.Extensions.Add(
                        new X509EnhancedKeyUsageExtension(
                              usages,
                              critical: false));
                }))
            {
                Assert.Equal(1, GetExtendedKeyUsageCount(certificate));
                Assert.True(CertificateUtility.HasExtendedKeyUsage(certificate, Oids.CodeSigningEku));
            }
        }

        [Fact]
        public void HasExtendedKeyUsage_WithoutEku_ReturnsFalseForEku()
        {
            using (var certificate = SigningTestUtility.GenerateCertificate("test", generator => { }))
            {
                Assert.Equal(0, GetExtendedKeyUsageCount(certificate));
                Assert.False(CertificateUtility.HasExtendedKeyUsage(certificate, Oids.CodeSigningEku));
            }
        }

        [Fact]
        public void IsValidForPurposeFast_WithCodeSigningEku_ReturnsTrueForCodeSigningEku()
        {
            using (var certificate = SigningTestUtility.GenerateCertificate("test",
                generator =>
                {
                    var usages = new OidCollection { new Oid(Oids.CodeSigningEku) };

                    generator.Extensions.Add(
                        new X509EnhancedKeyUsageExtension(
                              usages,
                              critical: false));
                }))
            {
                Assert.Equal(1, GetExtendedKeyUsageCount(certificate));
                Assert.True(CertificateUtility.IsValidForPurposeFast(certificate, Oids.CodeSigningEku));
            }
        }

        [Fact]
        public void IsValidForPurposeFast_WithoutCodeSigningEku_ReturnsFalseForCodeSigningEku()
        {
            using (var certificate = SigningTestUtility.GenerateCertificate("test",
                generator =>
                {
                    var usages = new OidCollection { new Oid(TestOids.IdKpEmailProtection) };

                    generator.Extensions.Add(
                        new X509EnhancedKeyUsageExtension(
                              usages,
                              critical: false));
                }))
            {
                Assert.Equal(1, GetExtendedKeyUsageCount(certificate));
                Assert.False(CertificateUtility.IsValidForPurposeFast(certificate, Oids.CodeSigningEku));
            }
        }

        [Fact]
        public void IsValidForPurposeFast_WithAnyExtendedKeyUsage_ReturnsFalseForCodeSigningEku()
        {
            using (var certificate = SigningTestUtility.GenerateCertificate("test",
                generator =>
                {
                    var usages = new OidCollection { new Oid(TestOids.IdKpEmailProtection), new Oid(TestOids.AnyExtendedKeyUsage) };

                    generator.Extensions.Add(
                        new X509EnhancedKeyUsageExtension(
                              usages,
                              critical: false));
                }))
            {
                Assert.Equal(2, GetExtendedKeyUsageCount(certificate));
                Assert.False(CertificateUtility.IsValidForPurposeFast(certificate, Oids.CodeSigningEku));
            }
        }

        [Fact]
        public void IsValidForPurposeFast_WithNoEku_ReturnsTrueForCodeSigningEku()
        {
            using (var certificate = SigningTestUtility.GenerateCertificate("test", generator => { }))
            {
                Assert.Equal(0, GetExtendedKeyUsageCount(certificate));
                Assert.True(CertificateUtility.IsValidForPurposeFast(certificate, Oids.CodeSigningEku));
            }
        }

        [Fact]
        public void IsSelfIssued_WhenCertificateNull_Throws()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => CertificateUtility.IsSelfIssued(certificate: null));

            Assert.Equal("certificate", exception.ParamName);
        }

        [Fact]
        public void IsSelfIssued_WithPartialChain_ReturnsFalse()
        {
            using (var certificate = SigningTestUtility.GetCertificate("leaf.crt"))
            {
                Assert.False(CertificateUtility.IsSelfIssued(certificate));
            }
        }

        [Fact]
        public void IsSelfIssued_WithNonSelfSignedCertificate_ReturnsFalse()
        {
            using (var certificate = _fixture.GetNonSelfSignedCertificate())
            {
                Assert.False(CertificateUtility.IsSelfIssued(certificate));
            }
        }

        [Fact]
        public void IsSelfIssued_WithSelfSignedCertificate_ReturnsTrue()
        {
            using (var certificate = _fixture.GetDefaultCertificate())
            {
                Assert.True(CertificateUtility.IsSelfIssued(certificate));
            }
        }

        [Fact]
        public void IsSelfIssued_WithSelfIssuedCertificate_ReturnsTrue()
        {
            using (var certificate = _fixture.GetSelfIssuedCertificate())
            {
                Assert.True(CertificateUtility.IsSelfIssued(certificate));
            }
        }

        [Fact]
        public void IsSelfIssued_WithRootCertificate_ReturnsTrue()
        {
            using (var certificate = _fixture.GetRootCertificate())
            {
                Assert.True(CertificateUtility.IsSelfIssued(certificate));
            }
        }

        [Fact]
        public void GetHashString_ReturnsCorrectHashForSupportedAlgorithms()
        {
            using (var certificate = SigningTestUtility.GetCertificate("leaf.crt"))
            {
                var sha256Fingerprint = CertificateUtility.GetHashString(certificate, Common.HashAlgorithmName.SHA256);
                var sha384Fingerprint = CertificateUtility.GetHashString(certificate, Common.HashAlgorithmName.SHA384);
                var sha512Fingerprint = CertificateUtility.GetHashString(certificate, Common.HashAlgorithmName.SHA512);

                Assert.Equal("9893F4B40FD236F16C189AD8F01D8B92FE682DFA6E768354ED25F4741BF51C73", sha256Fingerprint);
                Assert.Equal("6471116F2B2A4DBA7B021A208408F53FBA2BCA1661ED006112E82850AA9DFD06EC9B5C9A50B4D2E6890B756781503FE5", sha384Fingerprint);
                Assert.Equal("5B00A6B778AF9DC19BB62BFA688556FEC0A35AEFFB63DACD8D4EF2F227EC0EF43DA8B27F3E12F8C3485D128F32E4E7CA20136AF3BB3DF21A4B47AE54137698F3", sha512Fingerprint);
            }
        }

        [Fact(Skip="https://github.com/NuGet/Home/issues/12687")]
        public void GetHashString_UnknownHashAlgorithm_Throws()
        {
            using (var certificate = _fixture.GetDefaultCertificate())
            {
                Assert.Throws(typeof(ArgumentException),
                    () => CertificateUtility.GetHashString(certificate, Common.HashAlgorithmName.Unknown));
            }
        }

        [Fact]
        public void GetHashString_UnsupportedHashAlgorithm_Throws()
        {
            using (var certificate = _fixture.GetDefaultCertificate())
            {
                Assert.Throws(typeof(ArgumentException),
                    () => CertificateUtility.GetHashString(certificate, (Common.HashAlgorithmName)46));
            }
        }

        private static int GetExtendedKeyUsageCount(X509Certificate2 certificate)
        {
            foreach (var extension in certificate.Extensions)
            {
                if (string.Equals(extension.Oid.Value, Oids.EnhancedKeyUsage))
                {
                    return ((X509EnhancedKeyUsageExtension)extension).EnhancedKeyUsages.Count;
                }
            }

            return 0;
        }
    }
}
