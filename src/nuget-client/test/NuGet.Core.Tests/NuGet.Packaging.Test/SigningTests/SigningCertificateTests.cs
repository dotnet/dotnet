// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Security.Cryptography;
using System.Text;
using NuGet.Common;
using NuGet.Packaging.Signing;
using Org.BouncyCastle.Asn1;
using Test.Utility.Signing;
using Xunit;
using BcEssCertId = Org.BouncyCastle.Asn1.Ess.EssCertID;
using BcPolicyInformation = Org.BouncyCastle.Asn1.X509.PolicyInformation;
using BcSigningCertificate = Org.BouncyCastle.Asn1.Ess.SigningCertificate;
using SigningCertificate = NuGet.Packaging.Signing.SigningCertificate;

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
            var bcEssCertId = CreateBcEssCertId();
            var bcPolicyInfo = new BcPolicyInformation(new DerObjectIdentifier(Oids.AnyPolicy));
            var bcSigningCertificate = new BcSigningCertificate(
                new DerSequence(new DerSequence(bcEssCertId), new DerSequence(bcPolicyInfo)));
            var bytes = bcSigningCertificate.GetDerEncoded();

            var signingCertificate = SigningCertificate.Read(bytes);

            Assert.Equal(1, signingCertificate.Certificates.Count);
            Assert.Equal(1, signingCertificate.Policies.Count);

            var policyInfo = signingCertificate.Policies[0];

            Assert.Equal(bcPolicyInfo.PolicyIdentifier.ToString(), policyInfo.PolicyIdentifier.Value);
            Assert.Null(policyInfo.PolicyQualifiers);
        }

        [Fact]
        public void Read_WithMultipleEssCertIds_ReturnsSigningCertificate()
        {
            var bcEssCertId1 = CreateBcEssCertId();
            var bcEssCertId2 = CreateBcEssCertId();
            var bcEssCertId3 = CreateBcEssCertId();
            var bcSigningCertificate = new BcSigningCertificate(
                new DerSequence(new DerSequence(bcEssCertId1, bcEssCertId2, bcEssCertId3)));
            var bytes = bcSigningCertificate.GetDerEncoded();

            var signingCertificate = SigningCertificate.Read(bytes);

            Assert.Equal(3, signingCertificate.Certificates.Count);
            Assert.Null(signingCertificate.Policies);

            SigningTestUtility.VerifyByteArrays(
                bcEssCertId1.GetCertHash(),
                signingCertificate.Certificates[0].CertificateHash);
            Assert.Null(signingCertificate.Certificates[0].IssuerSerial);

            SigningTestUtility.VerifyByteArrays(
                bcEssCertId2.GetCertHash(),
                signingCertificate.Certificates[1].CertificateHash);
            Assert.Null(signingCertificate.Certificates[1].IssuerSerial);

            SigningTestUtility.VerifyByteArrays(
                bcEssCertId3.GetCertHash(),
                signingCertificate.Certificates[2].CertificateHash);
            Assert.Null(signingCertificate.Certificates[2].IssuerSerial);
        }

        private static BcEssCertId CreateBcEssCertId()
        {
            byte[] randomHash = new byte[20];

            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(randomHash);

                return new BcEssCertId(randomHash);
            }
        }
    }
}
