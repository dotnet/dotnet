// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Formats.Asn1;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Microsoft.Internal.NuGet.Testing.SignedPackages;
using NuGet.Common;
using NuGet.Packaging.Signing;
using Xunit;
using EssCertIdV2 = NuGet.Packaging.Signing.EssCertIdV2;
using Oid = System.Security.Cryptography.Oid;
using SigningCertificateV2 = NuGet.Packaging.Signing.SigningCertificateV2;
using TestAlgorithmIdentifier = Microsoft.Internal.NuGet.Testing.SignedPackages.Asn1.AlgorithmIdentifier;
using TestEssCertIdV2 = Microsoft.Internal.NuGet.Testing.SignedPackages.Asn1.EssCertIdV2;
using TestPolicyInformation = Microsoft.Internal.NuGet.Testing.SignedPackages.Asn1.PolicyInformation;
using TestSigningCertificateV2 = Microsoft.Internal.NuGet.Testing.SignedPackages.Asn1.SigningCertificateV2;

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
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(
                () => SigningCertificateV2.Create(certificate: null, hashAlgorithmName: HashAlgorithmName.SHA256));

            Assert.Equal("certificate", exception.ParamName);
        }

        [Theory]
        [InlineData(HashAlgorithmName.SHA256)]
        [InlineData(HashAlgorithmName.SHA384)]
        [InlineData(HashAlgorithmName.SHA512)]
        public void Create_WithValidInput_ReturnsSigningCertificateV2(HashAlgorithmName hashAlgorithmName)
        {
            using (X509Certificate2 certificate = _fixture.GetDefaultCertificate())
            {
                SigningCertificateV2 signingCertificateV2 = SigningCertificateV2.Create(certificate, hashAlgorithmName);

                Assert.Equal(1, signingCertificateV2.Certificates.Count);

                EssCertIdV2 essCertIdV2 = signingCertificateV2.Certificates[0];

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
            using (X509Certificate2 certificate = _fixture.GetDefaultCertificate())
            {
                SigningCertificateV2 expectedSigningCertificateV2 = SigningCertificateV2.Create(certificate, hashAlgorithmName);
                byte[] bytes = expectedSigningCertificateV2.Encode();

                SigningCertificateV2 actualSigningCertificateV2 = SigningCertificateV2.Read(bytes);

                Assert.Equal(
                    expectedSigningCertificateV2.Certificates.Count,
                    actualSigningCertificateV2.Certificates.Count);

                for (var i = 0; i < expectedSigningCertificateV2.Certificates.Count; ++i)
                {
                    EssCertIdV2 expectedEssCertIdV2 = expectedSigningCertificateV2.Certificates[i];
                    EssCertIdV2 actualEssCertIdV2 = actualSigningCertificateV2.Certificates[i];

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
            TestEssCertIdV2 testEssCertIdV2_1 = CreateEssCertIdV2(hashAlgorithmName, "1");
            TestEssCertIdV2 testEssCertIdV2_2 = CreateEssCertIdV2(hashAlgorithmName, "2");
            TestEssCertIdV2 testEssCertIdV2_3 = CreateEssCertIdV2(hashAlgorithmName, "3");
            TestSigningCertificateV2 testSigningCertificateV2 = new TestSigningCertificateV2(
                new[] { testEssCertIdV2_1, testEssCertIdV2_2, testEssCertIdV2_3 });
            AsnWriter writer = new(AsnEncodingRules.DER);

            testSigningCertificateV2.Encode(writer);
            byte[] bytes = writer.Encode();

            SigningCertificateV2 signingCertificate = SigningCertificateV2.Read(bytes);

            Assert.Equal(3, signingCertificate.Certificates.Count);
            Assert.Null(signingCertificate.Policies);

            SigningTestUtility.VerifyByteSequences(
                testEssCertIdV2_1.CertificateHash,
                signingCertificate.Certificates[0].CertificateHash);
            Assert.Null(signingCertificate.Certificates[0].IssuerSerial);

            SigningTestUtility.VerifyByteSequences(
                testEssCertIdV2_2.CertificateHash,
                signingCertificate.Certificates[1].CertificateHash);
            Assert.Null(signingCertificate.Certificates[1].IssuerSerial);

            SigningTestUtility.VerifyByteSequences(
                testEssCertIdV2_3.CertificateHash,
                signingCertificate.Certificates[2].CertificateHash);
            Assert.Null(signingCertificate.Certificates[2].IssuerSerial);
        }

        [Fact]
        public void Read_WithNoEssCertIds_ReturnsSigningCertificateV2()
        {
            TestSigningCertificateV2 testSigningCertificateV2 = new TestSigningCertificateV2(
                Array.Empty<TestEssCertIdV2>());
            AsnWriter writer = new(AsnEncodingRules.DER);

            testSigningCertificateV2.Encode(writer);

            byte[] bytes = writer.Encode();

            SigningCertificateV2 signingCertificate = SigningCertificateV2.Read(bytes);

            Assert.Equal(0, signingCertificate.Certificates.Count);
            Assert.Null(signingCertificate.Policies);
        }

        [Fact]
        public void Read_WithPolicyInformation_ReturnsSigningCertificateV2()
        {
            TestEssCertIdV2 testEssCertIdV2 = CreateEssCertIdV2(HashAlgorithmName.SHA256, "1");
            TestPolicyInformation testPolicyInfo = new(new Oid(Oids.AnyPolicy));
            TestSigningCertificateV2 testSigningCertificateV2 = new(
                new[] { testEssCertIdV2 }, new[] { testPolicyInfo });
            AsnWriter writer = new(AsnEncodingRules.DER);

            testSigningCertificateV2.Encode(writer);
            byte[] bytes = writer.Encode();

            SigningCertificateV2 signingCertificate = SigningCertificateV2.Read(bytes);

            Assert.Equal(1, signingCertificate.Certificates.Count);
            Assert.Equal(1, signingCertificate.Policies.Count);

            Signing.PolicyInformation policyInfo = signingCertificate.Policies[0];

            Assert.Equal(testPolicyInfo.PolicyIdentifier.Value, policyInfo.PolicyIdentifier.Value);
            Assert.Null(policyInfo.PolicyQualifiers);
        }

        private static TestEssCertIdV2 CreateEssCertIdV2(HashAlgorithmName hashAlgorithmName, string text)
        {
            byte[] hash = CryptoHashUtility.ComputeHash(hashAlgorithmName, Encoding.UTF8.GetBytes(text));
            TestAlgorithmIdentifier testAlgorithmIdentifier = new(new Oid(hashAlgorithmName.ConvertToOidString()));

            return new TestEssCertIdV2(testAlgorithmIdentifier, hash);
        }
    }
}
