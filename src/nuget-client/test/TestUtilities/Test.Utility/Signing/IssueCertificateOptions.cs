// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.X509;

namespace Test.Utility.Signing
{
    public sealed class IssueCertificateOptions
    {
        public Action<X509V3CertificateGenerator> CustomizeCertificate { get; set; }
        public DateTimeOffset NotAfter { get; set; }
        public DateTimeOffset NotBefore { get; set; }

        /// <summary>
        /// Gets or sets the private key for signing the new certificate.
        /// </summary>
        /// <remarks>
        /// Typically:
        ///
        ///     *  If the issue certificate request is for a self-signed root certificate, <see cref="IssuerPrivateKey" />
        ///        should be the private key of <see cref="KeyPair" />.
        ///     *  If the issue certificate request is for any other (non-root) certificate, <see cref="IssuerPrivateKey" />
        ///        should be null, indicating that the private key for the issuing certificate authority should be used.
        /// </remarks>
        public AsymmetricKeyParameter IssuerPrivateKey { get; set; }

        public AsymmetricCipherKeyPair KeyPair { get; set; }

        public X509Name SubjectName { get; set; }

        public string SignatureAlgorithmName { get; set; }

        public IssueCertificateOptions()
        {
            NotBefore = DateTimeOffset.UtcNow;
            NotAfter = NotBefore.AddHours(2);
            SignatureAlgorithmName = "SHA256WITHRSA";
        }

        public static IssueCertificateOptions CreateDefaultForRootCertificateAuthority()
        {
            var keyPair = CertificateUtilities.CreateKeyPair();
            var id = CertificateUtilities.GenerateRandomId();
            var subjectName = new X509Name($"C=US,ST=WA,L=Redmond,O=NuGet,CN=NuGet Test Root Certificate Authority ({id})");

            return new IssueCertificateOptions()
            {
                KeyPair = keyPair,
                IssuerPrivateKey = keyPair.Private,
                SubjectName = subjectName
            };
        }

        public static IssueCertificateOptions CreateDefaultForIntermediateCertificateAuthority()
        {
            var keyPair = CertificateUtilities.CreateKeyPair();
            var id = CertificateUtilities.GenerateRandomId();
            var subjectName = new X509Name($"C=US,ST=WA,L=Redmond,O=NuGet,CN=NuGet Test Intermediate Certificate Authority ({id})");

            return new IssueCertificateOptions()
            {
                KeyPair = keyPair,
                SubjectName = subjectName
            };
        }

        public static IssueCertificateOptions CreateDefaultForEndCertificate()
        {
            var keyPair = CertificateUtilities.CreateKeyPair();
            var id = CertificateUtilities.GenerateRandomId();
            var subjectName = new X509Name($"C=US,ST=WA,L=Redmond,O=NuGet,CN=NuGet Test Certificate ({id})");

            return new IssueCertificateOptions()
            {
                KeyPair = keyPair,
                SubjectName = subjectName
            };
        }

        public static IssueCertificateOptions CreateDefaultForTimestampService()
        {
            var keyPair = CertificateUtilities.CreateKeyPair();
            var id = Guid.NewGuid().ToString();
            var subjectName = new X509Name($"C=US,ST=WA,L=Redmond,O=NuGet,CN=NuGet Test Timestamp Service ({id})");

            return new IssueCertificateOptions()
            {
                KeyPair = keyPair,
                SubjectName = subjectName
            };
        }
    }
}
