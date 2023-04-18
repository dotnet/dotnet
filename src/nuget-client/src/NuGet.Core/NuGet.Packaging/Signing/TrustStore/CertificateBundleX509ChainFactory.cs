// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#if NET5_0_OR_GREATER

using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using NuGet.Common;

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

        public IX509Chain Create()
        {
            X509Chain x509Chain = new();

            x509Chain.ChainPolicy.TrustMode = X509ChainTrustMode.CustomRootTrust;

            if (Certificates is not null && Certificates.Count > 0)
            {
                x509Chain.ChainPolicy.CustomTrustStore.AddRange(Certificates);
            }

            return new X509ChainWrapper(x509Chain, GetAdditionalContext);
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

        private ILogMessage GetAdditionalContext(X509Chain chain)
        {
            if (chain is null)
            {
                throw new ArgumentNullException(nameof(chain));
            }

            ILogMessage logMessage = null;
            int lastIndex = chain.ChainElements.Count - 1;

            if (lastIndex < 0)
            {
                return logMessage;
            }

            X509ChainElement root = chain.ChainElements[lastIndex];

            // If a certificate chain is untrusted simply the root certificate is untrusted, then create
            // an additional message explaining how one might resolve this lack of trust.
            if (root.ChainElementStatus.Any(status => status.Status.HasFlag(X509ChainStatusFlags.UntrustedRoot)) &&
                !Certificates.Contains(root.Certificate))
            {
                string subject = root.Certificate.Subject;
                string fingerprint = CertificateUtility.GetHashString(root.Certificate, Common.HashAlgorithmName.SHA256);
                string pem = GetPemEncodedCertificate(root.Certificate);
                string message;

                if (string.IsNullOrEmpty(FilePath))
                {
                    message = string.Format(
                        CultureInfo.CurrentCulture,
                        Strings.UntrustedRoot_WithoutCertificateBundle,
                        subject,
                        fingerprint,
                        pem);
                }
                else
                {
                    message = string.Format(
                        CultureInfo.CurrentCulture,
                        Strings.UntrustedRoot_WithCertificateBundle,
                        FilePath,
                        subject,
                        fingerprint,
                        pem);
                }

                logMessage = new LogMessage(LogLevel.Warning, message, NuGetLogCode.NU3042);
            }

            return logMessage;
        }

        private static string GetPemEncodedCertificate(X509Certificate2 certificate)
        {
            ReadOnlyMemory<char> pem = PemEncoding.Write("CERTIFICATE", certificate.RawData);

            return new string(pem.Span);
        }
    }
}

#endif
