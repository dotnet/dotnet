// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Formats.Asn1;
using System.Numerics;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Internal.NuGet.Testing.SignedPackages;
using Xunit;
using IssuerSerial = NuGet.Packaging.Signing.IssuerSerial;
using TestGeneralName = Microsoft.Internal.NuGet.Testing.SignedPackages.Asn1.GeneralName;
using TestIssuerSerial = Microsoft.Internal.NuGet.Testing.SignedPackages.Asn1.IssuerSerial;

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
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(
                () => IssuerSerial.Create(certificate: null));

            Assert.Equal("certificate", exception.ParamName);
        }

        [Fact]
        public void Create_WithCertificate_InitializesFields()
        {
            using (X509Certificate2 certificate = _fixture.GetDefaultCertificate())
            {
                IssuerSerial issuerSerial = IssuerSerial.Create(certificate);

                Assert.Equal(1, issuerSerial.GeneralNames.Count);
                Assert.Equal(certificate.IssuerName.Name, issuerSerial.GeneralNames[0].DirectoryName.Name);
                SigningTestUtility.VerifySerialNumber(certificate, issuerSerial);
            }
        }

        [Fact]
        public void Create_WithSmallSerialNumber_ReturnsIssuerSerial()
        {
            using (X509Certificate2 certificate = SigningTestUtility.GenerateCertificate("test", generator =>
                {
                    byte[] bytes = BitConverter.GetBytes(1);
                    Array.Reverse(bytes);
                    generator.SetSerialNumber(bytes);
                }))
            {
                IssuerSerial issuerSerial = IssuerSerial.Create(certificate);

                SigningTestUtility.VerifySerialNumber(certificate, issuerSerial);
            }
        }

        [Fact]
        public void Create_WithLargePositiveSerialNumber_ReturnsIssuerSerial()
        {
            using (X509Certificate2 certificate = SigningTestUtility.GenerateCertificate("test", generator =>
                {
                    byte[] bytes = BitConverter.GetBytes(long.MaxValue);
                    Array.Reverse(bytes);
                    generator.SetSerialNumber(bytes);
                }))
            {
                IssuerSerial issuerSerial = IssuerSerial.Create(certificate);

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
            X500DistinguishedName directoryName = new("CN=test");
            TestGeneralName testGeneralName = new(directoryName: directoryName.RawData);
            TestIssuerSerial testIssuerSerial = new(new[] { testGeneralName }, BigInteger.One);
            AsnWriter writer = new(AsnEncodingRules.DER);

            testIssuerSerial.Encode(writer);

            byte[] bytes = writer.Encode();

            IssuerSerial issuerSerial = IssuerSerial.Read(bytes);

            Assert.Equal(1, issuerSerial.GeneralNames.Count);
            Assert.Equal(directoryName.Name, issuerSerial.GeneralNames[0].DirectoryName.Name);
            Assert.Equal(testIssuerSerial.SerialNumber.ToByteArray(), issuerSerial.SerialNumber);
        }
    }
}
