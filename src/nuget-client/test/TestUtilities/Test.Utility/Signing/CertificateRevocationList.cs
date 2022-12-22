// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto.Operators;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.X509.Extension;

namespace Test.Utility.Signing
{
    public class CertificateRevocationList : IDisposable
    {
        public X509Crl Crl { get; set; }

        public X509CertificateWithKeyInfo IssuerCert { get; private set; }

        public string CrlLocalPath { get; private set; }

        public BigInteger Version { get; private set; }

#if IS_SIGNING_SUPPORTED
        public static CertificateRevocationList CreateCrl(
            X509CertificateWithKeyInfo issuerCert,
            string crlLocalUri)
        {
            var version = BigInteger.One;
            var crl = CreateCrl(issuerCert, version);

            return new CertificateRevocationList()
            {
                Crl = crl,
                IssuerCert = issuerCert,
                CrlLocalPath = Path.Combine(crlLocalUri, $"{issuerCert.Certificate.Subject}.crl"),
                Version = version
            };
        }

        private static X509Crl CreateCrl(
            X509CertificateWithKeyInfo issuerCert,
            BigInteger version,
            X509Certificate2 revokedCertificate = null)
        {
            var bcIssuerCert = DotNetUtilities.FromX509Certificate(issuerCert.Certificate);
            var crlGen = new X509V2CrlGenerator();
            crlGen.SetIssuerDN(bcIssuerCert.SubjectDN);
            crlGen.SetThisUpdate(DateTime.Now);
            crlGen.SetNextUpdate(DateTime.Now.AddYears(1));
            crlGen.AddExtension(X509Extensions.AuthorityKeyIdentifier, false, new AuthorityKeyIdentifierStructure(bcIssuerCert));
            crlGen.AddExtension(X509Extensions.CrlNumber, false, new CrlNumber(version));

            if (revokedCertificate != null)
            {
                var bcRevokedCert = DotNetUtilities.FromX509Certificate(revokedCertificate);
                crlGen.AddCrlEntry(bcRevokedCert.SerialNumber, DateTime.Now, CrlReason.PrivilegeWithdrawn);
            }

            var random = new SecureRandom();
            var issuerPrivateKey = DotNetUtilities.GetKeyPair(issuerCert.KeyPair).Private;
            var signatureFactory = new Asn1SignatureFactory(bcIssuerCert.SigAlgOid, issuerPrivateKey, random);
            var crl = crlGen.Generate(signatureFactory);
            return crl;
        }

        public void RevokeCertificate(X509Certificate2 revokedCertificate)
        {
            UpdateVersion();
            Crl = CreateCrl(IssuerCert, Version, revokedCertificate);
            ExportCrl();
        }

        public void ExportCrl()
        {
            using (var streamWriter = new StreamWriter(File.Open(CrlLocalPath, FileMode.Create)))
            {
                var pemWriter = new PemWriter(streamWriter);
                pemWriter.WriteObject(Crl);
                pemWriter.Writer.Flush();
                pemWriter.Writer.Close();
            }
        }

        private void UpdateVersion()
        {
            Version = Version?.Add(BigInteger.One) ?? BigInteger.One;
        }
#else
        public static CertificateRevocationList CreateCrl(X509CertificateWithKeyInfo certCA, string crlLocalUri)
        {
            throw new NotImplementedException();
        }

        public void RevokeCertificate(X509Certificate2 revokedCertificate)
        {
            throw new NotImplementedException();
        }

        public void ExportCrl()
        {
            throw new NotImplementedException();
        }
#endif

        public void Dispose()
        {
            if (!string.IsNullOrEmpty(CrlLocalPath) && File.Exists(CrlLocalPath))
            {
                File.Delete(CrlLocalPath);
            }
        }
    }
}
