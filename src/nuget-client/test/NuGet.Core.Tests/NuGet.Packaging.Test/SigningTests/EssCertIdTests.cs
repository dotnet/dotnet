// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Security.Cryptography;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Security;
using Test.Utility.Signing;
using Xunit;
using BcEssCertId = Org.BouncyCastle.Asn1.Ess.EssCertID;
using BcGeneralName = Org.BouncyCastle.Asn1.X509.GeneralName;
using BcIssuerSerial = Org.BouncyCastle.Asn1.X509.IssuerSerial;
using EssCertId = NuGet.Packaging.Signing.EssCertId;

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
        [Fact(Skip="https://github.com/NuGet/Home/issues/12687")]
        public void Read_WithValidInput_ReturnsEssCertId()
        {
            using (var certificate = _fixture.GetDefaultCertificate())
            {
                var bcCertificate = DotNetUtilities.FromX509Certificate(certificate);
                var bcGeneralNames = new GeneralNames(
                    new BcGeneralName(BcGeneralName.DirectoryName, bcCertificate.IssuerDN));
                var bcIssuerSerial = new BcIssuerSerial(bcGeneralNames, new DerInteger(bcCertificate.SerialNumber));
                var hash = SigningTestUtility.GetHash(certificate, Common.HashAlgorithmName.SHA256);
                var bcEssCertId = new BcEssCertId(hash, bcIssuerSerial);
                var bytes = bcEssCertId.GetDerEncoded();

                var essCertId = EssCertId.Read(bytes);

                Assert.Equal(1, essCertId.IssuerSerial.GeneralNames.Count);
                Assert.Equal(certificate.IssuerName.Name, essCertId.IssuerSerial.GeneralNames[0].DirectoryName.Name);
                SigningTestUtility.VerifyByteArrays(hash, essCertId.CertificateHash);
                SigningTestUtility.VerifyByteArrays(bcIssuerSerial.Serial.Value.ToByteArray(), essCertId.IssuerSerial.SerialNumber);
            }
        }
#endif
    }
}
