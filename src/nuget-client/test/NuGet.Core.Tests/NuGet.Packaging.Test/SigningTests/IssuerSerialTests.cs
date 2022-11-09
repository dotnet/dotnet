// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Security.Cryptography;
using NuGet.Test.Utility;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Math;
using Test.Utility.Signing;
using Xunit;
using BcGeneralName = Org.BouncyCastle.Asn1.X509.GeneralName;
using BcIssuerSerial = Org.BouncyCastle.Asn1.X509.IssuerSerial;
using IssuerSerial = NuGet.Packaging.Signing.IssuerSerial;

namespace NuGet.Packaging.Test
{
    [Collection(SigningTestsCollection.Name)]
    public class IssuerSerialTests
    {
        private readonly CertificatesFixture _fixture;

        public IssuerSerialTests(CertificatesFixture fixture)
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
                () => IssuerSerial.Create(certificate: null));

            Assert.Equal("certificate", exception.ParamName);
        }

        [Fact]
        public void Create_WithCertificate_InitializesFields()
        {
            using (var certificate = _fixture.GetDefaultCertificate())
            {
                var issuerSerial = IssuerSerial.Create(certificate);

                Assert.Equal(1, issuerSerial.GeneralNames.Count);
                Assert.Equal(certificate.IssuerName.Name, issuerSerial.GeneralNames[0].DirectoryName.Name);
                SigningTestUtility.VerifySerialNumber(certificate, issuerSerial);
            }
        }

        [Fact]
        public void Create_WithSmallSerialNumber_ReturnsIssuerSerial()
        {
            using (var certificate = SigningTestUtility.GenerateCertificate("test", generator =>
                {
                    var bytes = BitConverter.GetBytes(1);
                    Array.Reverse(bytes);
                    generator.SetSerialNumber(bytes);
                }))
            {
                var issuerSerial = IssuerSerial.Create(certificate);

                SigningTestUtility.VerifySerialNumber(certificate, issuerSerial);
            }
        }

        [Fact]
        public void Create_WithLargePositiveSerialNumber_ReturnsIssuerSerial()
        {
            using (var certificate = SigningTestUtility.GenerateCertificate("test", generator =>
                {
                    var bytes = BitConverter.GetBytes(long.MaxValue);
                    Array.Reverse(bytes);
                    generator.SetSerialNumber(bytes);
                }))
            {
                var issuerSerial = IssuerSerial.Create(certificate);

                SigningTestUtility.VerifySerialNumber(certificate, issuerSerial);
            }
        }

        [Fact]
        public void Read_WithInvalidAsn1_Throws()
        {
            Assert.Throws<CryptographicException>(
                () => IssuerSerial.Read(new byte[] { 0x30, 0x07 }));
        }

        [Fact]
        public void Read_WithValidInput_ReturnsIssuerSerial()
        {
            var directoryName = new X509Name("CN=test");
            var generalNames = new GeneralNames(
                new BcGeneralName(BcGeneralName.DirectoryName, directoryName));
            var bcIssuerSerial = new BcIssuerSerial(generalNames, new DerInteger(BigInteger.One));
            var bytes = bcIssuerSerial.GetDerEncoded();

            var issuerSerial = IssuerSerial.Read(bytes);

            Assert.Equal(1, issuerSerial.GeneralNames.Count);
            Assert.Equal(directoryName.ToString(), issuerSerial.GeneralNames[0].DirectoryName.Name);
            Assert.Equal(bcIssuerSerial.Serial.Value.ToByteArray(), issuerSerial.SerialNumber);
        }
    }
}
