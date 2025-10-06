// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Formats.Asn1;
using System.Security.Cryptography;
using Microsoft.Internal.NuGet.Testing.SignedPackages;
using NuGet.Packaging.Signing;
using Xunit;
using PolicyInformation = NuGet.Packaging.Signing.PolicyInformation;
using SigningCertificate = NuGet.Packaging.Signing.SigningCertificate;
using TestEssCertId = Microsoft.Internal.NuGet.Testing.SignedPackages.Asn1.EssCertId;
using TestPolicyInformation = Microsoft.Internal.NuGet.Testing.SignedPackages.Asn1.PolicyInformation;
using TestSigningCertificate = Microsoft.Internal.NuGet.Testing.SignedPackages.Asn1.SigningCertificate;

namespace NuGet.Packaging.Test
{
    public class SigningCertificateTests
    {
        [Fact]
        public void Read_WithInvalidAsn1_Throws()
        {
            Assert.Throws<CryptographicException>(
                () => SigningCertificate.Read(new byte[] { 0x30, 0x0b }));
        }

        [Fact]
        public void Read_WithPolicyInformation_ReturnsSigningCertificate()
        {
            TestEssCertId essCertId = CreateEssCertId();
            TestPolicyInformation testPolicyInfo = new(new Oid(Oids.AnyPolicy));
            TestSigningCertificate testSigningCertificate = TestSigningCertificate.Create(
                new[] { essCertId },
                testPolicyInfo);
            AsnWriter writer = new(AsnEncodingRules.DER);

            testSigningCertificate.Encode(writer);

            byte[] bytes = writer.Encode();

            SigningCertificate signingCertificate = SigningCertificate.Read(bytes);

            Assert.Equal(1, signingCertificate.Certificates.Count);
            Assert.Equal(1, signingCertificate.Policies.Count);

            PolicyInformation policyInfo = signingCertificate.Policies[0];

            Assert.Equal(testPolicyInfo.PolicyIdentifier.Value, policyInfo.PolicyIdentifier.Value);
            Assert.Null(policyInfo.PolicyQualifiers);
        }

        [Fact]
        public void Read_WithMultipleEssCertIds_ReturnsSigningCertificate()
        {
            TestEssCertId essCertId1 = CreateEssCertId();
            TestEssCertId essCertId2 = CreateEssCertId();
            TestEssCertId essCertId3 = CreateEssCertId();
            TestSigningCertificate testSigningCertificate = TestSigningCertificate.Create(
                new[] { essCertId1, essCertId2, essCertId3 });

            AsnWriter writer = new(AsnEncodingRules.DER);

            testSigningCertificate.Encode(writer);

            byte[] bytes = writer.Encode();

            SigningCertificate signingCertificate = SigningCertificate.Read(bytes);

            Assert.Equal(3, signingCertificate.Certificates.Count);
            Assert.Null(signingCertificate.Policies);

            SigningTestUtility.VerifyByteSequences(
                essCertId1.CertificateHash,
                signingCertificate.Certificates[0].CertificateHash);
            Assert.Null(signingCertificate.Certificates[0].IssuerSerial);

            SigningTestUtility.VerifyByteSequences(
                essCertId2.CertificateHash,
                signingCertificate.Certificates[1].CertificateHash);
            Assert.Null(signingCertificate.Certificates[1].IssuerSerial);

            SigningTestUtility.VerifyByteSequences(
                essCertId3.CertificateHash,
                signingCertificate.Certificates[2].CertificateHash);
            Assert.Null(signingCertificate.Certificates[2].IssuerSerial);
        }

        private static TestEssCertId CreateEssCertId()
        {
            byte[] randomHash = new byte[20];

            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomHash);

                return TestEssCertId.Create(randomHash);
            }
        }
    }
}
