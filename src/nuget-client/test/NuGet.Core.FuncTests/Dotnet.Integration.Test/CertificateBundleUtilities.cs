// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Dotnet.Integration.Test
{
    internal static class CertificateBundleUtilities
    {
        internal static void AddCertificateToBundle(FileInfo certificateBundle, X509Certificate2 certificate)
        {
            ArgumentNullException.ThrowIfNull(certificateBundle, nameof(certificateBundle));
            ArgumentNullException.ThrowIfNull(certificate, nameof(certificate));

            X509Certificate2Collection certificates = new();

            certificates.ImportFromPemFile(certificateBundle.FullName);

            if (!certificates.Contains(certificate))
            {
                certificates.Add(certificate);

                WriteCertificateBundle(certificateBundle, certificates);
            }
        }

        internal static void RemoveCertificateFromBundle(FileInfo certificateBundle, X509Certificate2 certificate)
        {
            ArgumentNullException.ThrowIfNull(certificateBundle, nameof(certificateBundle));
            ArgumentNullException.ThrowIfNull(certificate, nameof(certificate));

            X509Certificate2Collection certificates = new();

            try
            {
                certificates.ImportFromPemFile(certificateBundle.FullName);

                if (certificates.Contains(certificate))
                {
                    certificates.Remove(certificate);

                    WriteCertificateBundle(certificateBundle, certificates);
                }
            }
            catch (DirectoryNotFoundException)
            {
                // Reads/writes will throw if the test .NET SDK directory has been deleted already.
            }
        }

        private static void WriteCertificateBundle(FileInfo certificateBundle, X509Certificate2Collection certificates)
        {
            FileInfo file = new(Path.GetTempFileName());

            try
            {
                using (StreamWriter writer = new(file.FullName))
                {
                    foreach (X509Certificate2 certificate in certificates)
                    {
                        char[] pem = PemEncoding.Write("CERTIFICATE", certificate.RawData);

                        writer.WriteLine(pem);
                        writer.WriteLine();
                    }
                }

                File.Copy(file.FullName, certificateBundle.FullName, overwrite: true);
            }
            finally
            {
                file.Delete();
            }
        }
    }
}
