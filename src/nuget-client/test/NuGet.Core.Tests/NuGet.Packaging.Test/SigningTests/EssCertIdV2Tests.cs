// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Formats.Asn1;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Microsoft.Internal.NuGet.Testing.SignedPackages;
using NuGet.Common;
using NuGet.Packaging.Signing;
using Xunit;
using EssCertIdV2 = NuGet.Packaging.Signing.EssCertIdV2;
using TestAlgorithmIdentifier = Microsoft.Internal.NuGet.Testing.SignedPackages.Asn1.AlgorithmIdentifier;
using TestEssCertIdV2 = Microsoft.Internal.NuGet.Testing.SignedPackages.Asn1.EssCertIdV2;
using TestGeneralName = Microsoft.Internal.NuGet.Testing.SignedPackages.Asn1.GeneralName;
using TestIssuerSerial = Microsoft.Internal.NuGet.Testing.SignedPackages.Asn1.IssuerSerial;

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
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(
                () => EssCertIdV2.Create(certificate: null, hashAlgorithmName: HashAlgorithmName.SHA256));

            Assert.Equal("certificate", exception.ParamName);
        }

        [Fact]
        public void Create_WithUnknownHashAlgorithmName_Throws()
        {
            using (X509Certificate2 certificate = _fixture.GetDefaultCertificate())
            {
                ArgumentException exception = Assert.Throws<ArgumentException>(
                    () => EssCertIdV2.Create(certificate, HashAlgorithmName.Unknown));

                Assert.Equal("hashAlgorithmName", exception.ParamName);
            }
        }

        [Fact]
        public void Create_WithSha256_ReturnsEssCertIdV2()
        {
            HashAlgorithmName hashAlgorithmName = HashAlgorithmName.SHA256;

            using (X509Certificate2 certificate = _fixture.GetDefaultCertificate())
            {
                EssCertIdV2 essCertIdV2 = EssCertIdV2.Create(certificate, hashAlgorithmName);

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
            HashAlgorithmName hashAlgorithmName = HashAlgorithmName.SHA384;

            using (X509Certificate2 certificate = _fixture.GetDefaultCertificate())
            {
                EssCertIdV2 essCertIdV2 = EssCertIdV2.Create(certificate, hashAlgorithmName);

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
            HashAlgorithmName hashAlgorithmName = HashAlgorithmName.SHA512;

            using (X509Certificate2 certificate = _fixture.GetDefaultCertificate())
            {
                EssCertIdV2 essCertIdV2 = EssCertIdV2.Create(certificate, hashAlgorithmName);

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
            byte[] hash = CryptoHashUtility.ComputeHash(HashAlgorithmName.SHA256, Encoding.UTF8.GetBytes("peach"));
            TestEssCertIdV2 testEssCertIdV2 = new(new TestAlgorithmIdentifier(TestOids.Sha256), hash);
            AsnWriter writer = new(AsnEncodingRules.DER);
            byte[] bytes = Encode(testEssCertIdV2);

            EssCertIdV2 essCertIdV2 = EssCertIdV2.Read(bytes);

            Assert.Equal(Oids.Sha256, essCertIdV2.HashAlgorithm.Algorithm.Value);
            SigningTestUtility.VerifyByteArrays(hash, essCertIdV2.CertificateHash);
            Assert.Null(essCertIdV2.IssuerSerial);
        }

        [Fact]
        public void Read_WithDefaultAlgorithmIdentifier_ReturnsEssCertIdV2()
        {
            X500DistinguishedName directoryName = new("CN=test");
            TestGeneralName testGeneralName = new(directoryName: directoryName.RawData);
            TestIssuerSerial testIssuerSerial = new(new[] { testGeneralName }, BigInteger.One);
            byte[] hash = CryptoHashUtility.ComputeHash(HashAlgorithmName.SHA256, Encoding.UTF8.GetBytes("peach"));
            TestEssCertIdV2 testEssCertIdV2 = new(new TestAlgorithmIdentifier(TestOids.Sha256), hash, testIssuerSerial);
            byte[] bytes = Encode(testEssCertIdV2);

            EssCertIdV2 essCertIdV2 = EssCertIdV2.Read(bytes);

            Assert.Equal(Oids.Sha256, essCertIdV2.HashAlgorithm.Algorithm.Value);
            Assert.Equal(1, essCertIdV2.IssuerSerial.GeneralNames.Count);
            Assert.Equal(directoryName.Name, essCertIdV2.IssuerSerial.GeneralNames[0].DirectoryName.Name);
            SigningTestUtility.VerifyByteArrays(hash, essCertIdV2.CertificateHash);
            SigningTestUtility.VerifyByteArrays(testIssuerSerial.SerialNumber.ToByteArray(), essCertIdV2.IssuerSerial.SerialNumber);
        }

        [Fact]
        public void Read_WithNonDefaultAlgorithmIdentifier_ReturnsEssCertIdV2()
        {
            X500DistinguishedName directoryName = new("CN=test");
            TestGeneralName testGeneralName = new(directoryName: directoryName.RawData);
            TestIssuerSerial testIssuerSerial = new(new[] { testGeneralName }, BigInteger.One);
            byte[] hash = CryptoHashUtility.ComputeHash(HashAlgorithmName.SHA512, Encoding.UTF8.GetBytes("peach"));
            TestEssCertIdV2 testEssCertIdV2 = new(new TestAlgorithmIdentifier(TestOids.Sha512), hash, testIssuerSerial);
            byte[] bytes = Encode(testEssCertIdV2);

            EssCertIdV2 essCertIdV2 = EssCertIdV2.Read(bytes);

            Assert.Equal(Oids.Sha512, essCertIdV2.HashAlgorithm.Algorithm.Value);
            Assert.Equal(1, essCertIdV2.IssuerSerial.GeneralNames.Count);
            Assert.Equal(directoryName.Name, essCertIdV2.IssuerSerial.GeneralNames[0].DirectoryName.Name);
            SigningTestUtility.VerifyByteArrays(hash, essCertIdV2.CertificateHash);
            SigningTestUtility.VerifySerialNumber(testIssuerSerial.SerialNumber, essCertIdV2.IssuerSerial.SerialNumber);
        }

#if IS_SIGNING_SUPPORTED
        [Fact]
        public void Read_WithValidInput_ReturnsEssCertId()
        {
            using (X509Certificate2 certificate = _fixture.GetDefaultCertificate())
            {
                TestGeneralName testGeneralName = new(directoryName: certificate.IssuerName.RawData);
                TestIssuerSerial testIssuerSerial = new(new[] { testGeneralName }, new BigInteger(certificate.GetSerialNumber()));
                byte[] hash = SigningTestUtility.GetHash(certificate, HashAlgorithmName.SHA384);
                TestEssCertIdV2 testEssCertIdV2 = new(new TestAlgorithmIdentifier(TestOids.Sha384), hash, testIssuerSerial);
                byte[] bytes = Encode(testEssCertIdV2);

                EssCertIdV2 essCertIdV2 = EssCertIdV2.Read(bytes);

                Assert.Equal(Oids.Sha384, essCertIdV2.HashAlgorithm.Algorithm.Value);
                Assert.Equal(1, essCertIdV2.IssuerSerial.GeneralNames.Count);
                Assert.Equal(certificate.IssuerName.Name, essCertIdV2.IssuerSerial.GeneralNames[0].DirectoryName.Name);
                SigningTestUtility.VerifyByteArrays(hash, essCertIdV2.CertificateHash);
                SigningTestUtility.VerifySerialNumber(testIssuerSerial.SerialNumber, essCertIdV2.IssuerSerial.SerialNumber);
            }
        }
#endif

        private static byte[] Encode(TestEssCertIdV2 testEssCertIdV2)
        {
            AsnWriter writer = new(AsnEncodingRules.DER);

            testEssCertIdV2.Encode(writer);

            return writer.Encode();
        }
    }
}
