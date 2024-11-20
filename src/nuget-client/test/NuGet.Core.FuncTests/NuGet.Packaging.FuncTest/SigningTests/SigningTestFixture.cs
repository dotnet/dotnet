// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.Internal.NuGet.Testing.SignedPackages;
using NuGet.Packaging.Signing;

namespace NuGet.Packaging.FuncTest
{
    using X509StorePurpose = Microsoft.Internal.NuGet.Testing.SignedPackages.X509StorePurpose;

    /// <summary>
    /// Used to bootstrap functional tests for signing.
    /// </summary>
    public class SigningTestFixture : IDisposable
    {
        private static readonly TimeSpan SoonDuration = TimeSpan.FromSeconds(20);

        private TrustedTestCert<TestCertificate> _trustedTestCert;
        private TrustedTestCert<TestCertificate> _trustedRepositoryCertificate;
        private TrustedTestCert<TestCertificate> _trustedTestCertExpired;
        private TrustedTestCert<TestCertificate> _trustedTestCertNotYetValid;
        private TrustedTestCert<X509Certificate2> _trustedServerRoot;
        private TestCertificate _untrustedTestCert;
        private IReadOnlyList<TrustedTestCert<TestCertificate>> _trustedTestCertificateWithReissuedCertificate;
        private IList<ISignatureVerificationProvider> _trustProviders;
        private SigningSpecifications _signingSpecifications;
        private Lazy<Task<SigningTestServer>> _testServer;
        private Lazy<Task<CertificateAuthority>> _defaultTrustedCertificateAuthority;
        private Lazy<Task<TimestampService>> _defaultTrustedTimestampService;
        private readonly DisposableList<IDisposable> _responders;

        public SigningTestFixture()
        {
            _testServer = new Lazy<Task<SigningTestServer>>(SigningTestServer.CreateAsync);
            _defaultTrustedCertificateAuthority = new Lazy<Task<CertificateAuthority>>(CreateDefaultTrustedCertificateAuthorityAsync);
            _defaultTrustedTimestampService = new Lazy<Task<TimestampService>>(CreateDefaultTrustedTimestampServiceAsync);
            _responders = new DisposableList<IDisposable>();
        }

        public TrustedTestCert<TestCertificate> TrustedTestCertificate
        {
            get
            {
                if (_trustedTestCert == null)
                {
                    _trustedTestCert = SigningTestUtility.GenerateTrustedTestCertificate();
                }

                return _trustedTestCert;
            }
        }

        // This certificate is interchangeable with TrustedTestCertificate and exists only
        // to provide certificate independence in author + repository signing scenarios.
        public TrustedTestCert<TestCertificate> TrustedRepositoryCertificate
        {
            get
            {
                if (_trustedRepositoryCertificate == null)
                {
                    _trustedRepositoryCertificate = SigningTestUtility.GenerateTrustedTestCertificate();
                }

                return _trustedRepositoryCertificate;
            }
        }

        public TrustedTestCert<TestCertificate> TrustedTestCertificateExpired
        {
            get
            {
                if (_trustedTestCertExpired == null)
                {
                    _trustedTestCertExpired = SigningTestUtility.GenerateTrustedTestCertificateExpired();
                }

                return _trustedTestCertExpired;
            }
        }

        public TrustedTestCert<TestCertificate> TrustedTestCertificateNotYetValid
        {
            get
            {
                if (_trustedTestCertNotYetValid == null)
                {
                    _trustedTestCertNotYetValid = SigningTestUtility.GenerateTrustedTestCertificateNotYetValid();
                }

                return _trustedTestCertNotYetValid;
            }
        }

        public IReadOnlyList<TrustedTestCert<TestCertificate>> TrustedTestCertificateWithReissuedCertificate
        {
            get
            {
                if (_trustedTestCertificateWithReissuedCertificate == null)
                {
                    using (var rsa = RSA.Create(keySizeInBits: 2048))
                    {
                        var certificateName = TestCertificate.GenerateCertificateName();
                        var certificate1 = SigningTestUtility.GenerateCertificate(certificateName, rsa);
                        var certificate2 = SigningTestUtility.GenerateCertificate(certificateName, rsa);

                        var testCertificate1 = new TestCertificate(X509StorePurpose.CodeSigning) { Cert = certificate1 }
                            .WithTrust();
                        var testCertificate2 = new TestCertificate(X509StorePurpose.CodeSigning) { Cert = certificate2 }
                            .WithTrust();

                        _trustedTestCertificateWithReissuedCertificate = new[]
                        {
                            testCertificate1,
                            testCertificate2
                        };
                    }
                }

                return _trustedTestCertificateWithReissuedCertificate;
            }
        }

        public TestCertificate UntrustedTestCertificate
        {
            get
            {
                if (_untrustedTestCert == null)
                {
                    _untrustedTestCert = TestCertificate.Generate(
                        X509StorePurpose.CodeSigning,
                        SigningTestUtility.CertificateModificationGeneratorForCodeSigningEkuCert);
                }

                return _untrustedTestCert;
            }
        }

        public IList<ISignatureVerificationProvider> TrustProviders
        {
            get
            {
                if (_trustProviders == null)
                {
                    _trustProviders = new List<ISignatureVerificationProvider>()
                    {
                        new SignatureTrustAndValidityVerificationProvider(),
                        new IntegrityVerificationProvider()
                    };
                }

                return _trustProviders;
            }
        }

        public SigningSpecifications SigningSpecifications
        {
            get
            {
                if (_signingSpecifications == null)
                {
                    _signingSpecifications = SigningSpecifications.V1;
                }

                return _signingSpecifications;
            }
        }

        public TrustedTestCert<TestCertificate> CreateTrustedTestCertificateThatWillExpireSoon()
        {
            return SigningTestUtility.GenerateTrustedTestCertificateThatWillExpireSoon(SoonDuration);
        }

        public TestCertificate CreateUntrustedTestCertificateThatWillExpireSoon()
        {
            Action<TestCertificateGenerator> actionGenerator = SigningTestUtility.CertificateModificationGeneratorForCertificateThatWillExpireSoon(SoonDuration);

            return TestCertificate.Generate(X509StorePurpose.CodeSigning, actionGenerator);
        }

        public async Task<ISigningTestServer> GetSigningTestServerAsync()
        {
            return await _testServer.Value;
        }

        public async Task<CertificateAuthority> GetDefaultTrustedCertificateAuthorityAsync()
        {
            return await _defaultTrustedCertificateAuthority.Value;
        }

        public async Task<TimestampService> GetDefaultTrustedTimestampServiceAsync()
        {
            return await _defaultTrustedTimestampService.Value;
        }

        private async Task<CertificateAuthority> CreateDefaultTrustedCertificateAuthorityAsync()
        {
            var testServer = await _testServer.Value;
            var rootCa = CertificateAuthority.Create(testServer.Url);
            var intermediateCa = rootCa.CreateIntermediateCertificateAuthority();
            var rootCertificate = new X509Certificate2(rootCa.Certificate);
            StoreLocation storeLocation = CertificateStoreUtilities.GetTrustedCertificateStoreLocation();

            _trustedServerRoot = TrustedTestCert.Create(
                rootCertificate,
                new X509StorePurpose[] { X509StorePurpose.CodeSigning, X509StorePurpose.Timestamping },
                StoreName.Root,
                storeLocation);

            var ca = intermediateCa;

            while (ca != null)
            {
                _responders.Add(testServer.RegisterResponder(ca));
                _responders.Add(testServer.RegisterResponder(ca.OcspResponder));

                ca = ca.Parent;
            }

            return intermediateCa;
        }

        private async Task<TimestampService> CreateDefaultTrustedTimestampServiceAsync()
        {
            var testServer = await _testServer.Value;
            var ca = await _defaultTrustedCertificateAuthority.Value;
            var timestampService = TimestampService.Create(ca);

            _responders.Add(testServer.RegisterResponder(timestampService));

            return timestampService;
        }

        public void Dispose()
        {
            _trustedTestCert?.Dispose();
            _trustedRepositoryCertificate?.Dispose();
            _trustedTestCertExpired?.Dispose();
            _trustedTestCertNotYetValid?.Dispose();
            _trustedServerRoot?.Dispose();
            _responders.Dispose();

            if (_trustedTestCertificateWithReissuedCertificate != null)
            {
                foreach (var certificate in _trustedTestCertificateWithReissuedCertificate)
                {
                    certificate.Dispose();
                }
            }

            if (_testServer.IsValueCreated)
            {
                _testServer.Value.Result.Dispose();
            }
        }
    }
}
