// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Internal.NuGet.Testing.SignedPackages;
using NuGet.Common;

namespace Dotnet.Integration.Test
{
    public sealed class X509StoreCertificate : IX509StoreCertificate, IDisposable
    {
        private bool _isDisposed;

        private readonly FileInfo _certificateBundle;
        private readonly List<X509StorePurpose> _storePurposes;
        private readonly bool _useCertificateBundle;

        public StoreLocation StoreLocation { get; }
        public StoreName StoreName { get; }
        public X509Certificate2 Certificate { get; }

        public X509StoreCertificate(
            StoreLocation storeLocation,
            StoreName storeName,
            X509Certificate2 certificate,
            FileInfo certificateBundle = null,
            params X509StorePurpose[] storePurposes)
        {
            StoreLocation = storeLocation;
            StoreName = storeName;
            Certificate = certificate ?? throw new ArgumentNullException(nameof(certificate));
            _certificateBundle = certificateBundle;
            _storePurposes = storePurposes.Distinct().ToList();

            if (_storePurposes.Count == 0)
            {
                throw new ArgumentException("At least one X.509 store purpose is required.", nameof(storePurposes));
            }

            _useCertificateBundle = storeName == StoreName.Root && !RuntimeEnvironmentHelper.IsWindows;

            if (_useCertificateBundle)
            {
                CertificateBundleUtilities.AddCertificateToBundle(certificateBundle, certificate);
            }

            foreach (X509StorePurpose storePurpose in _storePurposes)
            {
                // Some tests perform in-process signing and verification.
                // Other tests first create a signed package in-process and then verify out of process using the dotnet CLI.
                // So, the in-process test fallback certificate store must always be updated.
                X509StoreUtilities.AddCertificateToStore(storeLocation, storeName, certificate, storePurpose);
            }
        }

        public void Dispose()
        {
            if (!_isDisposed)
            {
                if (_useCertificateBundle)
                {
                    CertificateBundleUtilities.RemoveCertificateFromBundle(_certificateBundle, Certificate);
                }

                foreach (X509StorePurpose storePurpose in _storePurposes)
                {
                    X509StoreUtilities.RemoveCertificateFromStore(StoreLocation, StoreName, Certificate, storePurpose);
                }

                Certificate.Dispose();

                GC.SuppressFinalize(this);

                _isDisposed = true;
            }
        }
    }
}
