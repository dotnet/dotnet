// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Text;
using NuGet.Common;
using NuGet.Packaging.Signing;
using NuGet.Test.Utility;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using Test.Utility.Signing;
using Xunit;
using BcAlgorithmIdentifier = Org.BouncyCastle.Asn1.X509.AlgorithmIdentifier;
using BcEssCertIdV2 = Org.BouncyCastle.Asn1.Ess.EssCertIDv2;
using BcGeneralName = Org.BouncyCastle.Asn1.X509.GeneralName;
using BcIssuerSerial = Org.BouncyCastle.Asn1.X509.IssuerSerial;
using EssCertIdV2 = NuGet.Packaging.Signing.EssCertIdV2;

namespace NuGet.Packaging.Test
{
    [Collection(SigningTestsCollection.Name)]
    public class EssCertIdV2Tests
    {
        private readonly CertificatesFixture _fixture;

        public EssCertIdV2Tests(CertificatesFixture fixture)
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
                () => EssCertIdV2.Create(certificate: null, hashAlgorithmName: HashAlgorithmName.SHA256));

            Assert.Equal("certificate", exception.ParamName);
        }

        [Fact]
        public void Create_WithUnknownHashAlgorithmName_Throws()
        {
            using (var certificate = _fixture.GetDefaultCertificate())
            {
                var exception = Assert.Throws<ArgumentException>(
                    () => EssCertIdV2.Create(certificate, HashAlgorithmName.Unknown));

                Assert.Equal("hashAlgorithmName", exception.ParamName);
            }
        }

        [Fact]
        public void Create_WithSha256_ReturnsEssCertIdV2()
        {
            var hashAlgorithmName = HashAlgorithmName.SHA256;

            using (var certificate = _fixture.GetDefaultCertificate())
            {
                var essCertIdV2 = EssCertIdV2.Create(certificate, hashAlgorithmName);

                Assert.Equal(SigningTestUtility.GetHash(certificate, hashAlgorithmName), essCertIdV2.CertificateHash);
                Assert.Equal(Oids.Sha256, essCertIdV2.HashAlgorithm.Algorithm.Value);
                Assert.Equal(1, essCertIdV2.IssuerSerial.GeneralNames.Count);
                Assert.Equal(certificate.IssuerName.Name, essCertIdV2.IssuerSerial.GeneralNames[0].DirectoryName.Name);
                SigningTestUtility.VerifySerialNumber(certificate, essCertIdV2.IssuerSerial);
            }
        }

        [Fact]
        public void Create_WithSha384_ReturnsEssCertIdV2()
        {
            var hashAlgorithmName = HashAlgorithmName.SHA384;

            using (var certificate = _fixture.GetDefaultCertificate())
            {
                var essCertIdV2 = EssCertIdV2.Create(certificate, hashAlgorithmName);

                Assert.Equal(SigningTestUtility.GetHash(certificate, hashAlgorithmName), essCertIdV2.CertificateHash);
                Assert.Equal(Oids.Sha384, essCertIdV2.HashAlgorithm.Algorithm.Value);
                Assert.Equal(1, essCertIdV2.IssuerSerial.GeneralNames.Count);
                Assert.Equal(certificate.IssuerName.Name, essCertIdV2.IssuerSerial.GeneralNames[0].DirectoryName.Name);
                SigningTestUtility.VerifySerialNumber(certificate, essCertIdV2.IssuerSerial);
            }
        }

        [Fact]
        public void Create_WithSha512_ReturnsEssCertIdV2()
        {
            var hashAlgorithmName = HashAlgorithmName.SHA512;

            using (var certificate = _fixture.GetDefaultCertificate())
            {
                var essCertIdV2 = EssCertIdV2.Create(certificate, hashAlgorithmName);

                Assert.Equal(SigningTestUtility.GetHash(certificate, hashAlgorithmName), essCertIdV2.CertificateHash);
                Assert.Equal(Oids.Sha512, essCertIdV2.HashAlgorithm.Algorithm.Value);
                Assert.Equal(1, essCertIdV2.IssuerSerial.GeneralNames.Count);
                Assert.Equal(certificate.IssuerName.Name, essCertIdV2.IssuerSerial.GeneralNames[0].DirectoryName.Name);
                SigningTestUtility.VerifySerialNumber(certificate, essCertIdV2.IssuerSerial);
            }
        }

        [Fact]
        public void Read_WithInvalidAsn1_Throws()
        {
            Assert.Throws<System.Security.Cryptography.CryptographicException>(
                () => EssCertIdV2.Read(new byte[] { 0x30, 0x0b }));
        }

        [Fact]
        public void Read_WithOnlyCertificateHash_ReturnsEssCertIdV2()
        {
            var hash = CryptoHashUtility.ComputeHash(HashAlgorithmName.SHA256, Encoding.UTF8.GetBytes("peach"));
            var bcEssCertId = new BcEssCertIdV2(hash);
            var bytes = bcEssCertId.GetDerEncoded();

            var essCertIdV2 = EssCertIdV2.Read(bytes);

            Assert.Equal(Oids.Sha256, essCertIdV2.HashAlgorithm.Algorithm.Value);
            SigningTestUtility.VerifyByteArrays(hash, essCertIdV2.CertificateHash);
            Assert.Null(essCertIdV2.IssuerSerial);
        }

        [Fact]
        public void Read_WithDefaultAlgorithmIdentifier_ReturnsEssCertIdV2()
        {
            var directoryName = new X509Name("CN=test");
            var generalNames = new GeneralNames(
                new BcGeneralName(BcGeneralName.DirectoryName, directoryName));
            var bcIssuerSerial = new BcIssuerSerial(generalNames, new DerInteger(BigInteger.One));
            var hash = CryptoHashUtility.ComputeHash(HashAlgorithmName.SHA256, Encoding.UTF8.GetBytes("peach"));
            var bcEssCertId = new BcEssCertIdV2(hash, bcIssuerSerial);
            var bytes = bcEssCertId.GetDerEncoded();

            var essCertIdV2 = EssCertIdV2.Read(bytes);

            Assert.Equal(Oids.Sha256, essCertIdV2.HashAlgorithm.Algorithm.Value);
            Assert.Equal(1, essCertIdV2.IssuerSerial.GeneralNames.Count);
            Assert.Equal(directoryName.ToString(), essCertIdV2.IssuerSerial.GeneralNames[0].DirectoryName.Name);
            SigningTestUtility.VerifyByteArrays(hash, essCertIdV2.CertificateHash);
            SigningTestUtility.VerifyByteArrays(bcIssuerSerial.Serial.Value.ToByteArray(), essCertIdV2.IssuerSerial.SerialNumber);
        }

        [Fact]
        public void Read_WithNonDefaultAlgorithmIdentifier_ReturnsEssCertIdV2()
        {
            var directoryName = new X509Name("CN=test");
            var generalNames = new GeneralNames(
                new BcGeneralName(BcGeneralName.DirectoryName, directoryName));
            var bcIssuerSerial = new BcIssuerSerial(generalNames, new DerInteger(BigInteger.One));
            var hash = CryptoHashUtility.ComputeHash(HashAlgorithmName.SHA512, Encoding.UTF8.GetBytes("peach"));
            var bcAlgorithmId = new BcAlgorithmIdentifier(new DerObjectIdentifier(Oids.Sha512));
            var bcEssCertId = new BcEssCertIdV2(bcAlgorithmId, hash, bcIssuerSerial);
            var bytes = bcEssCertId.GetDerEncoded();

            var essCertIdV2 = EssCertIdV2.Read(bytes);

            Assert.Equal(Oids.Sha512, essCertIdV2.HashAlgorithm.Algorithm.Value);
            Assert.Equal(1, essCertIdV2.IssuerSerial.GeneralNames.Count);
            Assert.Equal(directoryName.ToString(), essCertIdV2.IssuerSerial.GeneralNames[0].DirectoryName.Name);
            SigningTestUtility.VerifyByteArrays(hash, essCertIdV2.CertificateHash);
            SigningTestUtility.VerifyByteArrays(bcIssuerSerial.Serial.Value.ToByteArray(), essCertIdV2.IssuerSerial.SerialNumber);
        }

#if IS_SIGNING_SUPPORTED
        [Fact]
        public void Read_WithValidInput_ReturnsEssCertId()
        {
            using (var certificate = _fixture.GetDefaultCertificate())
            {
                var bcCertificate = DotNetUtilities.FromX509Certificate(certificate);
                var bcGeneralNames = new GeneralNames(
                    new BcGeneralName(BcGeneralName.DirectoryName, bcCertificate.IssuerDN));
                var bcIssuerSerial = new BcIssuerSerial(bcGeneralNames, new DerInteger(bcCertificate.SerialNumber));
                var hash = SigningTestUtility.GetHash(certificate, HashAlgorithmName.SHA384);
                var bcAlgorithmId = new BcAlgorithmIdentifier(new DerObjectIdentifier(Oids.Sha384));
                var bcEssCertId = new BcEssCertIdV2(bcAlgorithmId, hash, bcIssuerSerial);
                var bytes = bcEssCertId.GetDerEncoded();

                var essCertIdV2 = EssCertIdV2.Read(bytes);

                Assert.Equal(Oids.Sha384, essCertIdV2.HashAlgorithm.Algorithm.Value);
                Assert.Equal(1, essCertIdV2.IssuerSerial.GeneralNames.Count);
                Assert.Equal(certificate.IssuerName.Name, essCertIdV2.IssuerSerial.GeneralNames[0].DirectoryName.Name);
                SigningTestUtility.VerifyByteArrays(hash, essCertIdV2.CertificateHash);
                SigningTestUtility.VerifyByteArrays(bcIssuerSerial.Serial.Value.ToByteArray(), essCertIdV2.IssuerSerial.SerialNumber);
            }
        }
#endif
    }
}
