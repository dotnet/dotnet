// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
#if IS_SIGNING_SUPPORTED
using System.Formats.Asn1;
#endif
using System.Security.Cryptography;
#if IS_SIGNING_SUPPORTED
using System.Security.Cryptography.X509Certificates;
using Microsoft.Internal.NuGet.Testing.SignedPackages;

#endif
using Xunit;
using EssCertId = NuGet.Packaging.Signing.EssCertId;
#if IS_SIGNING_SUPPORTED
using TestGeneralName = Microsoft.Internal.NuGet.Testing.SignedPackages.Asn1.GeneralName;
#endif

namespace NuGet.Packaging.Test
{
    [Collection(SigningTestsCollection.Name)]
    public class EssCertIdTests
    {
        private readonly CertificatesFixture _fixture;

        public EssCertIdTests(CertificatesFixture fixture)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            _fixture = fixture;
        }

        [Fact]
        public void Read_WithInvalidAsn1_Throws()
        {
            Assert.Throws<CryptographicException>(
                () => EssCertId.Read(new byte[] { 0x30, 0x0b }));
        }

#if IS_SIGNING_SUPPORTED
        [Fact]
        public void Read_WithValidInput_ReturnsEssCertId()
        {
            using (X509Certificate2 certificate = _fixture.GetDefaultCertificate())
            {
                byte[] bytes = CreateEssCertId(certificate);
                EssCertId essCertId = EssCertId.Read(bytes);

                Assert.Equal(1, essCertId.IssuerSerial.GeneralNames.Count);
                Assert.Equal(certificate.IssuerName.Name, essCertId.IssuerSerial.GeneralNames[0].DirectoryName.Name);

                byte[] serialNumber = HexConverter.ToByteArray(certificate.SerialNumber);

                SigningTestUtility.VerifyByteArrays(certificate.GetCertHash(), essCertId.CertificateHash);
                SigningTestUtility.VerifyByteArrays(serialNumber, essCertId.IssuerSerial.SerialNumber);
            }
        }

        private static byte[] CreateEssCertId(X509Certificate2 certificate)
        {
            AsnWriter writer = new(AsnEncodingRules.DER);

            using (writer.PushSequence())
            {
                writer.WriteOctetString(certificate.GetCertHash());

                using (writer.PushSequence())
                {
                    using (writer.PushSequence())
                    {
                        TestGeneralName generalName = new(directoryName: certificate.IssuerName.RawData);

                        generalName.Encode(writer);
                    }

                    byte[] serialNumber = HexConverter.ToByteArray(certificate.SerialNumber);

                    writer.WriteInteger(serialNumber);
                }
            }

            return writer.Encode();
        }
#endif
    }
}
