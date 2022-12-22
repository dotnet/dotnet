// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Security.Cryptography.X509Certificates;
using Test.Utility.Signing;

namespace NuGet.Commands.Test
{
    public sealed class CertificatesFixture : IDisposable
    {
        private readonly X509Certificate2 _defaultCertificate;

        private readonly TrustedTestCert<X509Certificate2> _trustedDefaultCertificate;

        private bool _isDisposed;

        public CertificatesFixture()
        {
            _defaultCertificate = SigningTestUtility.GenerateCertificate("test", generator => { });

            X509Certificate2 _defaultCertificateForTrust = SigningTestUtility.GenerateCertificate("test trusted", generator => { });

            _trustedDefaultCertificate = TrustedTestCert.Create(
                new X509Certificate2(_defaultCertificateForTrust),
                X509StorePurpose.CodeSigning,
                StoreName.My,
                StoreLocation.CurrentUser);
        }

        public void Dispose()
        {
            if (!_isDisposed)
            {
                _defaultCertificate.Dispose();

                _trustedDefaultCertificate.Dispose();

                GC.SuppressFinalize(this);

                _isDisposed = true;
            }
        }

        public X509Certificate2 GetDefaultCertificate()
        {
            X509Certificate2 certWithPrivateKey = SigningTestUtility.GetPublicCertWithPrivateKey(_defaultCertificate);
            return certWithPrivateKey;
        }

        public X509Certificate2 GetTrustedCertificate()
        {
            return _trustedDefaultCertificate.TrustedCert;
        }
    }
}
