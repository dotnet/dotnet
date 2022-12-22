// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using NuGet.Test.Utility;

namespace NuGet.MSSigning.Extensions.FuncTest
{
    internal sealed class MSSignCommandTestContext : IDisposable
    {
        private bool _isDisposed;
        private readonly string _defaultProviderName = "Microsoft Enhanced RSA and AES Cryptographic Provider";

        internal TestDirectory Directory { get; }
        internal X509Certificate2 Cert { get; }
        internal string CertificatePath { get; }
        internal string CertificateCSPName { get; private set; }
        internal string CertificateKeyContainer { get; private set; }

        internal MSSignCommandTestContext(X509Certificate2 certificate, bool exportPfx = true)
        {
            Directory = TestDirectory.Create();
            Cert = new X509Certificate2(certificate);
            CertificatePath = $"{Path.Combine(Directory, Guid.NewGuid().ToString())}.pfx";

            var certData = Cert.Export(exportPfx ? X509ContentType.Pfx : X509ContentType.Cert);
            File.WriteAllBytes(CertificatePath, certData);

            GetKeyContainerInformation(Cert);
        }

        public void Dispose()
        {
            if (!_isDisposed)
            {
                Directory.Dispose();
                Cert.Dispose();

                GC.SuppressFinalize(this);

                _isDisposed = true;
            }
        }

        private void GetKeyContainerInformation(X509Certificate2 certificate)
        {
            if (certificate == null)
            {
                throw new ArgumentNullException(nameof(certificate));
            }

            var key = certificate.GetRSAPrivateKey() as ICspAsymmetricAlgorithm;
            if (key != null)
            {
                var keyContainer = key.CspKeyContainerInfo;

                CertificateCSPName = keyContainer.ProviderName;
                CertificateKeyContainer = keyContainer.KeyContainerName;
            }

            CertificateCSPName = CertificateCSPName ?? _defaultProviderName;
            CertificateKeyContainer = CertificateKeyContainer ?? new Guid().ToString();
        }
    }
}
