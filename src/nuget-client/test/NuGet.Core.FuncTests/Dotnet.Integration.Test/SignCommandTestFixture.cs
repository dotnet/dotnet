// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.Internal.NuGet.Testing.SignedPackages;
using NuGet.Packaging.Signing;
using NuGet.Test.Utility;
using Test.Utility;

namespace Dotnet.Integration.Test
{
    using X509StorePurpose = Microsoft.Internal.NuGet.Testing.SignedPackages.X509StorePurpose;

    /// <summary>
    /// Used to bootstrap functional tests for signing.
    /// </summary>
    public class SignCommandTestFixture : IDisposable
    {
        private const int _normalCertChainLength = 3;
        //setting up a short cert chain then it's easier to make it invalid.
        private const int _shortCertChainLength = 2;

        private X509StoreCertificate _trustedTimestampRoot;
        private X509StoreCertificate _untrustedSelfIssuedCertificateInCertificateStore;
        private List<X509StoreCertificate> _defaultCertificateChain;
        private List<X509StoreCertificate> _invalidEkuCertificateChain;
        private List<X509StoreCertificate> _expiredCertificateChain;
        private List<X509StoreCertificate> _notYetValidCertificateChain;
        private List<X509StoreCertificate> _revokedCertificateChain;
        private List<X509StoreCertificate> _revocationUnknownCertificateChain;
        private IList<ISignatureVerificationProvider> _trustProviders;
        private SigningSpecifications _signingSpecifications;
        private MockServer _crlServer;
        private bool _crlServerRunning;
        private object _crlServerRunningLock = new();
        private TestDirectory _testDirectory;
        private Lazy<Task<SigningTestServer>> _testServer;
        private Lazy<Task<CertificateAuthority>> _defaultTrustedTimestampingRootCertificateAuthority;
        private Lazy<Task<TimestampService>> _defaultTrustedTimestampService;
        private readonly DisposableList<IDisposable> _responders;
        private FileInfo _codeSigningFallbackCertificateBundle;
        private FileInfo _timestampingFallbackCertificateBundle;

        public IX509StoreCertificate DefaultCertificate
        {
            get
            {
                if (_defaultCertificateChain is null)
                {
                    _defaultCertificateChain = new List<X509StoreCertificate>();

                    using (IX509CertificateChain chain = SigningTestUtility.GenerateCertificateChainWithoutTrust(
                        _normalCertChainLength,
                        CrlServer.Uri,
                        TestDirectory.Path))
                    {
                        StoreLocation rootStoreLocation = CertificateStoreUtilities.GetTrustedCertificateStoreLocation(readOnly: false);

                        _defaultCertificateChain.Add(CreateX509StoreCertificate(StoreLocation.CurrentUser, StoreName.My, chain[0], X509StorePurpose.CodeSigning));
                        _defaultCertificateChain.Add(CreateX509StoreCertificate(StoreLocation.CurrentUser, StoreName.My, chain[1], X509StorePurpose.CodeSigning));
                        _defaultCertificateChain.Add(CreateX509StoreCertificate(rootStoreLocation, StoreName.Root, chain[2], X509StorePurpose.CodeSigning));
                    }

                    SetUpCrlDistributionPoint();
                }

                return _defaultCertificateChain[0];
            }
        }

        public IX509StoreCertificate CertificateWithInvalidEku
        {
            get
            {
                if (_invalidEkuCertificateChain is null)
                {
                    _invalidEkuCertificateChain = new List<X509StoreCertificate>();

                    using (IX509CertificateChain chain = SigningTestUtility.GenerateCertificateChainWithoutTrust(
                        _normalCertChainLength,
                        CrlServer.Uri,
                        TestDirectory.Path,
                        configureLeafCrl: true,
                        leafCertificateActionGenerator: SigningTestUtility.CertificateModificationGeneratorForInvalidEkuCert))
                    {
                        StoreLocation rootStoreLocation = CertificateStoreUtilities.GetTrustedCertificateStoreLocation(readOnly: false);

                        _invalidEkuCertificateChain.Add(CreateX509StoreCertificate(StoreLocation.CurrentUser, StoreName.My, chain[0], X509StorePurpose.CodeSigning));
                        _invalidEkuCertificateChain.Add(CreateX509StoreCertificate(StoreLocation.CurrentUser, StoreName.My, chain[1], X509StorePurpose.CodeSigning));
                        _invalidEkuCertificateChain.Add(CreateX509StoreCertificate(rootStoreLocation, StoreName.Root, chain[2], X509StorePurpose.CodeSigning));
                    }

                    SetUpCrlDistributionPoint();
                }

                return _invalidEkuCertificateChain[0];
            }
        }

        public IX509StoreCertificate ExpiredCertificate
        {
            get
            {
                if (_expiredCertificateChain is null)
                {
                    _expiredCertificateChain = new List<X509StoreCertificate>();

                    Action<TestCertificateGenerator> actionGenerator = SigningTestUtility.CertificateModificationGeneratorForCertificateThatOnlyValidInSpecifiedPeriod(
                        notBefore: DateTime.UtcNow.AddSeconds(-10),
                        notAfter: DateTime.UtcNow.AddSeconds(-9));

                    using (IX509CertificateChain chain = SigningTestUtility.GenerateCertificateChainWithoutTrust(
                        _normalCertChainLength,
                        CrlServer.Uri,
                        TestDirectory.Path,
                        configureLeafCrl: true,
                        leafCertificateActionGenerator: actionGenerator))
                    {
                        StoreLocation rootStoreLocation = CertificateStoreUtilities.GetTrustedCertificateStoreLocation(readOnly: false);

                        _expiredCertificateChain.Add(CreateX509StoreCertificate(StoreLocation.CurrentUser, StoreName.My, chain[0], X509StorePurpose.CodeSigning));
                        _expiredCertificateChain.Add(CreateX509StoreCertificate(StoreLocation.CurrentUser, StoreName.My, chain[1], X509StorePurpose.CodeSigning));
                        _expiredCertificateChain.Add(CreateX509StoreCertificate(rootStoreLocation, StoreName.Root, chain[2], X509StorePurpose.CodeSigning));
                    }

                    SetUpCrlDistributionPoint();
                }

                return _expiredCertificateChain[0];
            }
        }

        public IX509StoreCertificate NotYetValidCertificate
        {
            get
            {
                if (_notYetValidCertificateChain is null)
                {
                    _notYetValidCertificateChain = new List<X509StoreCertificate>();

                    Action<TestCertificateGenerator> actionGenerator = SigningTestUtility.CertificateModificationGeneratorForCertificateThatOnlyValidInSpecifiedPeriod(
                        notBefore: DateTime.UtcNow.AddMinutes(10),
                        notAfter: DateTime.UtcNow.AddMinutes(15));

                    using (IX509CertificateChain chain = SigningTestUtility.GenerateCertificateChainWithoutTrust(
                        _normalCertChainLength,
                        CrlServer.Uri,
                        TestDirectory.Path,
                        configureLeafCrl: true,
                        leafCertificateActionGenerator: actionGenerator))
                    {
                        StoreLocation rootStoreLocation = CertificateStoreUtilities.GetTrustedCertificateStoreLocation(readOnly: false);

                        _notYetValidCertificateChain.Add(CreateX509StoreCertificate(StoreLocation.CurrentUser, StoreName.My, chain[0], X509StorePurpose.CodeSigning));
                        _notYetValidCertificateChain.Add(CreateX509StoreCertificate(StoreLocation.CurrentUser, StoreName.My, chain[1], X509StorePurpose.CodeSigning));
                        _notYetValidCertificateChain.Add(CreateX509StoreCertificate(rootStoreLocation, StoreName.Root, chain[2], X509StorePurpose.CodeSigning));
                    }

                    SetUpCrlDistributionPoint();
                }

                return _notYetValidCertificateChain[0];
            }
        }

        public IX509StoreCertificate RevokedCertificate
        {
            get
            {
                if (_revokedCertificateChain is null)
                {
                    _revokedCertificateChain = new List<X509StoreCertificate>();

                    using (IX509CertificateChain chain = SigningTestUtility.GenerateCertificateChainWithoutTrust(
                        _shortCertChainLength,
                        CrlServer.Uri,
                        TestDirectory.Path,
                        revokeEndCertificate: true))
                    {
                        StoreLocation rootStoreLocation = CertificateStoreUtilities.GetTrustedCertificateStoreLocation(readOnly: false);

                        _revokedCertificateChain.Add(CreateX509StoreCertificate(StoreLocation.CurrentUser, StoreName.My, chain[0], X509StorePurpose.CodeSigning));
                        _revokedCertificateChain.Add(CreateX509StoreCertificate(rootStoreLocation, StoreName.Root, chain[1], X509StorePurpose.CodeSigning));
                    }

                    SetUpCrlDistributionPoint();
                }

                return _revokedCertificateChain[0];
            }
        }

        public IX509StoreCertificate RevocationUnknownCertificate
        {
            get
            {
                if (_revocationUnknownCertificateChain is null)
                {
                    _revocationUnknownCertificateChain = new List<X509StoreCertificate>();

                    using (IX509CertificateChain chain = SigningTestUtility.GenerateCertificateChainWithoutTrust(
                        _shortCertChainLength,
                        CrlServer.Uri,
                        TestDirectory.Path,
                        configureLeafCrl: false))
                    {
                        StoreLocation rootStoreLocation = CertificateStoreUtilities.GetTrustedCertificateStoreLocation(readOnly: false);

                        _revocationUnknownCertificateChain.Add(CreateX509StoreCertificate(StoreLocation.CurrentUser, StoreName.My, chain[0], X509StorePurpose.CodeSigning));
                        _revocationUnknownCertificateChain.Add(CreateX509StoreCertificate(rootStoreLocation, StoreName.Root, chain[1], X509StorePurpose.CodeSigning));
                    }

                    SetUpCrlDistributionPoint();
                }

                return _revocationUnknownCertificateChain[0];
            }
        }

        public IX509StoreCertificate UntrustedSelfIssuedCertificateInCertificateStore
        {
            get
            {
                if (_untrustedSelfIssuedCertificateInCertificateStore is null)
                {
                    X509Certificate2 certificate = SigningTestUtility.GenerateSelfIssuedCertificate(isCa: false);

                    _untrustedSelfIssuedCertificateInCertificateStore = new X509StoreCertificate(
                        StoreLocation.CurrentUser,
                        StoreName.My,
                        certificate,
                        _codeSigningFallbackCertificateBundle,
                        X509StorePurpose.CodeSigning);
                }

                return _untrustedSelfIssuedCertificateInCertificateStore;
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
                _signingSpecifications ??= SigningSpecifications.V1;

                return _signingSpecifications;
            }
        }

        internal MockServer CrlServer
        {
            get
            {
                _crlServer ??= new MockServer();

                return _crlServer;
            }
        }

        public TestDirectory TestDirectory
        {
            get
            {
                _testDirectory ??= TestDirectory.Create();

                return _testDirectory;
            }
        }

        public SignCommandTestFixture()
        {
            _testServer = new Lazy<Task<SigningTestServer>>(SigningTestServer.CreateAsync);
            _defaultTrustedTimestampingRootCertificateAuthority = new Lazy<Task<CertificateAuthority>>(CreateDefaultTrustedTimestampingRootCertificateAuthorityAsync);
            _defaultTrustedTimestampService = new Lazy<Task<TimestampService>>(CreateDefaultTrustedTimestampServiceAsync);
            _responders = new DisposableList<IDisposable>();
        }

        private void SetUpCrlDistributionPoint()
        {
            lock (_crlServerRunningLock)
            {
                if (!_crlServerRunning)
                {
                    CrlServer.Get.Add(
                        "/",
                        request =>
                        {
                            var urlSplits = request.RawUrl.Split('/').Where(s => !string.IsNullOrEmpty(s)).ToArray();
                            if (urlSplits.Length != 2 || !urlSplits[1].EndsWith(".crl"))
                            {
                                return new Action<HttpListenerResponse>(response =>
                                {
                                    response.StatusCode = 404;
                                });
                            }
                            else
                            {
                                var crlName = urlSplits[1];
                                var crlPath = Path.Combine(TestDirectory, crlName);
                                if (File.Exists(crlPath))
                                {
                                    return new Action<HttpListenerResponse>(response =>
                                    {
                                        response.ContentType = "application/pkix-crl";
                                        response.StatusCode = 200;
                                        var content = File.ReadAllBytes(crlPath);
                                        MockServer.SetResponseContent(response, content);
                                    });
                                }
                                else
                                {
                                    return new Action<HttpListenerResponse>(response =>
                                    {
                                        response.StatusCode = 404;
                                    });
                                }
                            }
                        });

                    CrlServer.Start();
                    _crlServerRunning = true;
                }
            }
        }

        public async Task<ISigningTestServer> GetSigningTestServerAsync()
        {
            return await _testServer.Value;
        }

        public async Task<CertificateAuthority> GetDefaultTrustedTimestampingRootCertificateAuthorityAsync()
        {
            return await _defaultTrustedTimestampingRootCertificateAuthority.Value;
        }

        public async Task<TimestampService> GetDefaultTrustedTimestampServiceAsync()
        {
            return await _defaultTrustedTimestampService.Value;
        }

        public void Dispose()
        {
            _trustedTimestampRoot?.Dispose();
            _untrustedSelfIssuedCertificateInCertificateStore?.Dispose();

            DisposeX509StoreCertificates(_defaultCertificateChain);
            DisposeX509StoreCertificates(_invalidEkuCertificateChain);
            DisposeX509StoreCertificates(_expiredCertificateChain);
            DisposeX509StoreCertificates(_notYetValidCertificateChain);
            DisposeX509StoreCertificates(_revokedCertificateChain);
            DisposeX509StoreCertificates(_revocationUnknownCertificateChain);

            _crlServer?.Stop();
            _crlServer?.Dispose();
            _testDirectory?.Dispose();
            _responders.Dispose();

            if (_testServer.IsValueCreated)
            {
                _testServer.Value.Result.Dispose();
            }

            GC.SuppressFinalize(this);
        }

        private static void DisposeX509StoreCertificates(List<X509StoreCertificate> storeCertificates)
        {
            if (storeCertificates is not null)
            {
                foreach (X509StoreCertificate storeCertificate in storeCertificates)
                {
                    storeCertificate.Dispose();
                }
            }
        }

        private async Task<CertificateAuthority> CreateDefaultTrustedTimestampingRootCertificateAuthorityAsync()
        {
            var testServer = await _testServer.Value;
            var rootCa = CertificateAuthority.Create(testServer.Url);
            var intermediateCa = rootCa.CreateIntermediateCertificateAuthority();
            var rootCertificate = new X509Certificate2(rootCa.Certificate);
            StoreLocation storeLocation = CertificateStoreUtilities.GetTrustedCertificateStoreLocation();

            _trustedTimestampRoot = new X509StoreCertificate(
                storeLocation,
                StoreName.Root,
                rootCertificate,
                _timestampingFallbackCertificateBundle,
                X509StorePurpose.Timestamping);

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
            var ca = await _defaultTrustedTimestampingRootCertificateAuthority.Value;
            var timestampService = TimestampService.Create(ca);

            _responders.Add(testServer.RegisterResponder(timestampService));

            return timestampService;
        }

        internal void SetFallbackCertificateBundles(DirectoryInfo sdkDirectory)
        {
            ArgumentNullException.ThrowIfNull(sdkDirectory, nameof(sdkDirectory));

            _codeSigningFallbackCertificateBundle = new FileInfo(
                Path.Combine(
                    sdkDirectory.FullName,
                    FallbackCertificateBundleX509ChainFactory.SubdirectoryName,
                    FallbackCertificateBundleX509ChainFactory.CodeSigningFileName));

            _timestampingFallbackCertificateBundle = new FileInfo(
                Path.Combine(
                    sdkDirectory.FullName,
                    FallbackCertificateBundleX509ChainFactory.SubdirectoryName,
                    FallbackCertificateBundleX509ChainFactory.TimestampingFileName));
        }

        private X509StoreCertificate CreateX509StoreCertificate(
            StoreLocation storeLocation,
            StoreName storeName,
            X509Certificate2 certificate,
            X509StorePurpose storePurpose)
        {
            FileInfo certificateBundle;

            switch (storePurpose)
            {
                case X509StorePurpose.CodeSigning:
                    certificateBundle = _codeSigningFallbackCertificateBundle;
                    break;

                case X509StorePurpose.Timestamping:
                    certificateBundle = _timestampingFallbackCertificateBundle;
                    break;

                default:
                    throw new ArgumentException("Unsupported store purpose.", nameof(storePurpose));
            }

            // Clone the source certificate because the source certificate will be disposed.
            return new X509StoreCertificate(
                storeLocation,
                storeName,
                new X509Certificate2(certificate),
                certificateBundle,
                storePurpose);
        }
    }
}
