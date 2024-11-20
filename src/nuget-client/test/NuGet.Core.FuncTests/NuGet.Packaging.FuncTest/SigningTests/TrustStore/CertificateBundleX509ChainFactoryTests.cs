// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#if NET5_0_OR_GREATER

using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Internal.NuGet.Testing.SignedPackages;
using NuGet.Test.Utility;

namespace NuGet.Packaging.FuncTest.SigningTests
{
    public abstract class CertificateBundleX509ChainFactoryTests
    {
        protected SigningTestFixture Fixture { get; }

        public CertificateBundleX509ChainFactoryTests(SigningTestFixture fixture)
        {
            Fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
        }

        protected static FileInfo CreateCertificateBundle(TestDirectory directory)
        {
            FileInfo file = new(Path.Combine(directory.Path, "certificate.bundle"));

            TestCertificate otherRootCertificate = TestCertificate.Generate(
                X509StorePurpose.CodeSigning,
                SigningTestUtility.CertificateModificationGeneratorForCodeSigningEkuCert);

            using (otherRootCertificate.Cert)
            {
                string pem = GetPemEncodedCertificate(otherRootCertificate.Cert);

                using (StreamWriter writer = new(file.FullName))
                {
                    writer.WriteLine(pem);
                }
            }

            return file;
        }

        protected static string GetCertificateFingerprint(X509Certificate2 certificate)
        {
            return certificate.GetCertHashString(HashAlgorithmName.SHA256);
        }

        protected static string GetPemEncodedCertificate(X509Certificate2 certificate)
        {
            ReadOnlyMemory<char> pem = PemEncoding.Write("CERTIFICATE", certificate.RawData);

            return new string(pem.Span);
        }
    }
}

#endif
