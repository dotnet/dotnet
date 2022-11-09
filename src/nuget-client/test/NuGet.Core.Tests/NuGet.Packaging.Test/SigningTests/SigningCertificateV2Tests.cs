// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Text;
using NuGet.Common;
using NuGet.Packaging.Signing;
using NuGet.Test.Utility;
using Org.BouncyCastle.Asn1;
using Test.Utility.Signing;
using Xunit;
using BcAlgorithmIdentifier = Org.BouncyCastle.Asn1.X509.AlgorithmIdentifier;
using BcEssCertIdV2 = Org.BouncyCastle.Asn1.Ess.EssCertIDv2;
using BcPolicyInformation = Org.BouncyCastle.Asn1.X509.PolicyInformation;
using BcSigningCertificateV2 = Org.BouncyCastle.Asn1.Ess.SigningCertificateV2;
using SigningCertificateV2 = NuGet.Packaging.Signing.SigningCertificateV2;

namespace NuGet.Packaging.Test
{
    [Collection(SigningTestsCollection.Name)]
    public class SigningCertificateV2Tests
    {
        private readonly CertificatesFixture _fixture;

        public SigningCertificateV2Tests(CertificatesFixture fixture)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            _fixture = fixture;
        }

        [Fact]
        public void Create_WhenCertificateNull_Throws()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => SigningCertificateV2.Create(certificate: null, hashAlgorithmName: HashAlgorithmName.SHA256));

            Assert.Equal("certificate", exception.ParamName);
        }

        [Theory]
        [InlineData(HashAlgorithmName.SHA256)]
        [InlineData(HashAlgorithmName.SHA384)]
        [InlineData(HashAlgorithmName.SHA512)]
        public void Create_WithValidInput_ReturnsSigningCertificateV2(HashAlgorithmName hashAlgorithmName)
        {
            using (var certificate = _fixture.GetDefaultCertificate())
            {
                var signingCertificateV2 = SigningCertificateV2.Create(certificate, hashAlgorithmName);

                Assert.Equal(1, signingCertificateV2.Certificates.Count);

                var essCertIdV2 = signingCertificateV2.Certificates[0];

                Assert.Equal(hashAlgorithmName, CryptoHashUtility.OidToHashAlgorithmName(essCertIdV2.HashAlgorithm.Algorithm.Value));
                Assert.Equal(SigningTestUtility.GetHash(certificate, hashAlgorithmName), essCertIdV2.CertificateHash);
                Assert.Equal(1, essCertIdV2.IssuerSerial.GeneralNames.Count);
                Assert.Equal(certificate.IssuerName.Name, essCertIdV2.IssuerSerial.GeneralNames[0].DirectoryName.Name);
                SigningTestUtility.VerifySerialNumber(certificate, essCertIdV2.IssuerSerial);
                Assert.Null(signingCertificateV2.Policies);
            }
        }

        [Fact]
        public void Read_WithInvalidAsn1_Throws()
        {
            Assert.Throws<System.Security.Cryptography.CryptographicException>(
                () => SigningCertificateV2.Read(new byte[] { 0x30, 0x0b }));
        }

        [Theory]
        [InlineData(HashAlgorithmName.SHA256)]
        [InlineData(HashAlgorithmName.SHA384)]
        [InlineData(HashAlgorithmName.SHA512)]
        public void Read_WithValidInput_ReturnsSigningCertificateV2(HashAlgorithmName hashAlgorithmName)
        {
            using (var certificate = _fixture.GetDefaultCertificate())
            {
                var expectedSigningCertificateV2 = SigningCertificateV2.Create(certificate, hashAlgorithmName);
                var bytes = expectedSigningCertificateV2.Encode();

                var actualSigningCertificateV2 = SigningCertificateV2.Read(bytes);

                Assert.Equal(
                    expectedSigningCertificateV2.Certificates.Count,
                    actualSigningCertificateV2.Certificates.Count);

                for (var i = 0; i < expectedSigningCertificateV2.Certificates.Count; ++i)
                {
                    var expectedEssCertIdV2 = expectedSigningCertificateV2.Certificates[i];
                    var actualEssCertIdV2 = actualSigningCertificateV2.Certificates[i];

                    Assert.Equal(
                        expectedEssCertIdV2.HashAlgorithm.Algorithm.Value,
                        actualEssCertIdV2.HashAlgorithm.Algorithm.Value);
                    SigningTestUtility.VerifyByteArrays(
                        expectedEssCertIdV2.CertificateHash,
                        actualEssCertIdV2.CertificateHash);
                    Assert.Equal(
                        expectedEssCertIdV2.IssuerSerial.GeneralNames[0].DirectoryName.Name,
                        actualEssCertIdV2.IssuerSerial.GeneralNames[0].DirectoryName.Name);
                    SigningTestUtility.VerifyByteArrays(expectedEssCertIdV2.IssuerSerial.SerialNumber,
                        actualEssCertIdV2.IssuerSerial.SerialNumber);
                }
            }
        }

        [Theory]
        [InlineData(HashAlgorithmName.SHA256)]
        [InlineData(HashAlgorithmName.SHA384)]
        [InlineData(HashAlgorithmName.SHA512)]
        public void Read_WithMultipleEssCertIds_ReturnsSigningCertificateV2(HashAlgorithmName hashAlgorithmName)
        {
            var bcEssCertIdV2_1 = CreateBcEssCertIdV2(hashAlgorithmName, "1");
            var bcEssCertIdV2_2 = CreateBcEssCertIdV2(hashAlgorithmName, "2");
            var bcEssCertIdV2_3 = CreateBcEssCertIdV2(hashAlgorithmName, "3");
            var bcSigningCertificateV2 = new BcSigningCertificateV2(
                new[] { bcEssCertIdV2_1, bcEssCertIdV2_2, bcEssCertIdV2_3 });
            var bytes = bcSigningCertificateV2.GetDerEncoded();

            var signingCertificate = SigningCertificateV2.Read(bytes);

            Assert.Equal(3, signingCertificate.Certificates.Count);
            Assert.Null(signingCertificate.Policies);

            SigningTestUtility.VerifyByteArrays(
                bcEssCertIdV2_1.GetCertHash(),
                signingCertificate.Certificates[0].CertificateHash);
            Assert.Null(signingCertificate.Certificates[0].IssuerSerial);

            SigningTestUtility.VerifyByteArrays(
                bcEssCertIdV2_2.GetCertHash(),
                signingCertificate.Certificates[1].CertificateHash);
            Assert.Null(signingCertificate.Certificates[1].IssuerSerial);

            SigningTestUtility.VerifyByteArrays(
                bcEssCertIdV2_3.GetCertHash(),
                signingCertificate.Certificates[2].CertificateHash);
            Assert.Null(signingCertificate.Certificates[2].IssuerSerial);
        }

        [Fact]
        public void Read_WithNoEssCertIds_ReturnsSigningCertificateV2()
        {
            var bcSigningCertificateV2 = new BcSigningCertificateV2(new BcEssCertIdV2[0]);
            var bytes = bcSigningCertificateV2.GetDerEncoded();

            var signingCertificate = SigningCertificateV2.Read(bytes);

            Assert.Equal(0, signingCertificate.Certificates.Count);
            Assert.Null(signingCertificate.Policies);
        }

        [Fact]
        public void Read_WithPolicyInformation_ReturnsSigningCertificateV2()
        {
            var bcEssCertIdV2 = CreateBcEssCertIdV2(HashAlgorithmName.SHA256, "1");
            var bcPolicyInfo = new BcPolicyInformation(new DerObjectIdentifier(Oids.AnyPolicy));
            var bcSigningCertificateV2 = new BcSigningCertificateV2(
                new[] { bcEssCertIdV2 }, new[] { bcPolicyInfo });
            var bytes = bcSigningCertificateV2.GetDerEncoded();

            var signingCertificate = SigningCertificateV2.Read(bytes);

            Assert.Equal(1, signingCertificate.Certificates.Count);
            Assert.Equal(1, signingCertificate.Policies.Count);

            var policyInfo = signingCertificate.Policies[0];

            Assert.Equal(bcPolicyInfo.PolicyIdentifier.ToString(), policyInfo.PolicyIdentifier.Value);
            Assert.Null(policyInfo.PolicyQualifiers);
        }

        private static BcEssCertIdV2 CreateBcEssCertIdV2(HashAlgorithmName hashAlgorithmName, string text)
        {
            var hash = CryptoHashUtility.ComputeHash(hashAlgorithmName, Encoding.UTF8.GetBytes(text));
            var bcAlgorithmIdentifier = new BcAlgorithmIdentifier(
                new DerObjectIdentifier(hashAlgorithmName.ConvertToOidString()));

            return new BcEssCertIdV2(bcAlgorithmIdentifier, hash);
        }
    }
}
