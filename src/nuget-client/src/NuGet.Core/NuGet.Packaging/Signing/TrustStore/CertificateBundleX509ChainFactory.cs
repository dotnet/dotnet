// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#if NET5_0_OR_GREATER

using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace NuGet.Packaging.Signing
{
    internal abstract class CertificateBundleX509ChainFactory : IX509ChainFactory
    {
        public X509Certificate2Collection Certificates { get; }
        public string FilePath { get; }

        protected CertificateBundleX509ChainFactory(X509Certificate2Collection certificates, string filePath = null)
        {
            Certificates = certificates;
            FilePath = filePath;
        }

        public X509Chain Create()
        {
            X509Chain x509Chain = new();

            x509Chain.ChainPolicy.TrustMode = X509ChainTrustMode.CustomRootTrust;

            if (Certificates is not null && Certificates.Count > 0)
            {
                x509Chain.ChainPolicy.CustomTrustStore.AddRange(Certificates);
            }

            return x509Chain;
        }

        protected static bool TryImportFromPemFile(string filePath, out X509Certificate2Collection certificates)
        {
            certificates = new X509Certificate2Collection();

            try
            {
                certificates.ImportFromPemFile(filePath);

                return true;
            }
            catch (Exception ex) when
            (
                ex is CryptographicException ||
                ex is FileNotFoundException ||
                ex is DirectoryNotFoundException
            )
            {
                certificates.Clear();
            }

            return false;
        }
    }
}

#endif
