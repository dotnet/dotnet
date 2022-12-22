// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using NuGet.CommandLine.Test;
using NuGet.Packaging.Signing;
using NuGet.Test.Utility;
using Test.Utility;
using Test.Utility.Signing;

namespace NuGet.CommandLine.FuncTest.Commands
{
    using X509StorePurpose = global::Test.Utility.Signing.X509StorePurpose;

    /// <summary>
    /// Used to bootstrap functional tests for signing.
    /// </summary>
    public class SignCommandTestFixture : IDisposable
    {
        private const int _validCertChainLength = 3;
        private const int _invalidCertChainLength = 2;

        private TrustedTestCert<TestCertificate> _trustedTestCert;
        private TrustedTestCert<TestCertificate> _trustedTestCertWithInvalidEku;
        private TrustedTestCert<TestCertificate> _trustedTestCertExpired;
        private TrustedTestCert<TestCertificate> _trustedTestCertNotYetValid;
        private TrustedTestCert<X509Certificate2> _trustedTimestampRoot;
        private TrustedTestCert<X509Certificate2> _untrustedSelfIssuedCertificateInCertificateStore;
        private TrustedTestCertificateChain _trustedTestCertChain;
        private TrustedTestCertificateChain _revokedTestCertChain;
        private TrustedTestCertificateChain _revocationUnknownTestCertChain;
        private IList<ISignatureVerificationProvider> _trustProviders;
        private SigningSpecifications _signingSpecifications;
        private MockServer _crlServer;
        private bool _crlServerRunning;
        private object _crlServerRunningLock = new object();
        private TestDirectory _testDirectory;
        private string _nugetExePath;
        private Lazy<Task<SigningTestServer>> _testServer;
        private Lazy<Task<CertificateAuthority>> _defaultTrustedCertificateAuthority;
        private Lazy<Task<TimestampService>> _defaultTrustedTimestampService;
        private readonly DisposableList<IDisposable> _responders;

        public TrustedTestCert<TestCertificate> TrustedTestCertificate
        {
            get
            {
                if (_trustedTestCert == null)
                {
                    var actionGenerator = SigningTestUtility.CertificateModificationGeneratorForCodeSigningEkuCert;

                    // Code Sign EKU needs trust to a root authority
                    // Add the cert to Root CA list in LocalMachine as it does not prompt a dialog
                    // This makes all the associated tests to require admin privilege
                    _trustedTestCert = TestCertificate.Generate(X509StorePurpose.CodeSigning, actionGenerator)
                        .WithPrivateKeyAndTrust(StoreName.Root);
                }

                return _trustedTestCert;
            }
        }

        public TrustedTestCert<TestCertificate> TrustedTestCertificateWithInvalidEku
        {
            get
            {
                if (_trustedTestCertWithInvalidEku == null)
                {
                    var actionGenerator = SigningTestUtility.CertificateModificationGeneratorForInvalidEkuCert;

                    // Add the cert to Root CA list in LocalMachine as it does not prompt a dialog
                    // This makes all the associated tests to require admin privilege
                    _trustedTestCertWithInvalidEku = TestCertificate.Generate(X509StorePurpose.CodeSigning, actionGenerator)
                        .WithPrivateKeyAndTrust(StoreName.Root);
                }

                return _trustedTestCertWithInvalidEku;
            }
        }

        public TrustedTestCert<TestCertificate> TrustedTestCertificateExpired
        {
            get
            {
                if (_trustedTestCertExpired == null)
                {
                    var actionGenerator = SigningTestUtility.CertificateModificationGeneratorExpiredCert;

                    // Code Sign EKU needs trust to a root authority
                    // Add the cert to Root CA list in LocalMachine as it does not prompt a dialog
                    // This makes all the associated tests to require admin privilege
                    _trustedTestCertExpired = TestCertificate.Generate(X509StorePurpose.CodeSigning, actionGenerator)
                        .WithPrivateKeyAndTrust(StoreName.Root);
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
                    var actionGenerator = SigningTestUtility.CertificateModificationGeneratorNotYetValidCert;

                    // Code Sign EKU needs trust to a root authority
                    // Add the cert to Root CA list in LocalMachine as it does not prompt a dialog
                    // This makes all the associated tests to require admin privilege
                    _trustedTestCertNotYetValid = TestCertificate.Generate(X509StorePurpose.CodeSigning, actionGenerator)
                        .WithPrivateKeyAndTrust(StoreName.Root);
                }

                return _trustedTestCertNotYetValid;
            }
        }

        public TrustedTestCertificateChain TrustedTestCertificateChain
        {
            get
            {
                if (_trustedTestCertChain == null)
                {
                    var certChain = SigningTestUtility.GenerateCertificateChain(_validCertChainLength, CrlServer.Uri, TestDirectory.Path);

                    _trustedTestCertChain = new TrustedTestCertificateChain()
                    {
                        Certificates = certChain
                    };

                    SetUpCrlDistributionPoint();
                }

                return _trustedTestCertChain;
            }
        }

        public TrustedTestCert<TestCertificate> RevokedTestCertificateWithChain
        {
            get
            {
                if (_revokedTestCertChain == null)
                {
                    var certChain = SigningTestUtility.GenerateCertificateChain(_invalidCertChainLength, CrlServer.Uri, TestDirectory.Path);

                    _revokedTestCertChain = new TrustedTestCertificateChain()
                    {
                        Certificates = certChain
                    };

                    // mark leaf certificate as revoked
                    _revokedTestCertChain.Certificates[0].Source.Crl.RevokeCertificate(_revokedTestCertChain.Leaf.Source.Cert);

                    SetUpCrlDistributionPoint();
                }

                return _revokedTestCertChain.Leaf;
            }
        }

        public TrustedTestCert<TestCertificate> RevocationUnknownTestCertificateWithChain
        {
            get
            {
                if (_revocationUnknownTestCertChain == null)
                {
                    var certChain = SigningTestUtility.GenerateCertificateChain(_invalidCertChainLength, CrlServer.Uri, TestDirectory.Path, configureLeafCrl: false);

                    _revocationUnknownTestCertChain = new TrustedTestCertificateChain()
                    {
                        Certificates = certChain
                    };

                    SetUpCrlDistributionPoint();
                }

                return _revocationUnknownTestCertChain.Leaf;
            }
        }

        public X509Certificate2 UntrustedSelfIssuedCertificateInCertificateStore
        {
            get
            {
                if (_untrustedSelfIssuedCertificateInCertificateStore == null)
                {
                    var certificate = SigningTestUtility.GenerateSelfIssuedCertificate(isCa: false);

                    _untrustedSelfIssuedCertificateInCertificateStore = TrustedTestCert.Create(
                        certificate,
                        X509StorePurpose.CodeSigning,
                        StoreName.My,
                        StoreLocation.CurrentUser);
                }

                return new X509Certificate2(_untrustedSelfIssuedCertificateInCertificateStore.Source);
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

        public string NuGetExePath
        {
            get
            {
                if (_nugetExePath == null)
                {
                    _nugetExePath = Util.GetNuGetExePath();
                }

                return _nugetExePath;
            }
        }

        public MockServer CrlServer
        {
            get
            {
                if (_crlServer == null)
                {
                    _crlServer = new MockServer();
                }

                return _crlServer;
            }
        }

        public TestDirectory TestDirectory
        {
            get
            {
                if (_testDirectory == null)
                {
                    _testDirectory = TestDirectory.Create();
                }

                return _testDirectory;
            }
        }

        public SignCommandTestFixture()
        {
            _testServer = new Lazy<Task<SigningTestServer>>(SigningTestServer.CreateAsync);
            _defaultTrustedCertificateAuthority = new Lazy<Task<CertificateAuthority>>(CreateDefaultTrustedCertificateAuthorityAsync);
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

        public async Task<CertificateAuthority> GetDefaultTrustedCertificateAuthorityAsync()
        {
            return await _defaultTrustedCertificateAuthority.Value;
        }

        public async Task<TimestampService> GetDefaultTrustedTimestampServiceAsync()
        {
            return await _defaultTrustedTimestampService.Value;
        }

        public void Dispose()
        {
            _trustedTestCert?.Dispose();
            _trustedTestCertWithInvalidEku?.Dispose();
            _trustedTestCertExpired?.Dispose();
            _trustedTestCertNotYetValid?.Dispose();
            _trustedTimestampRoot?.Dispose();
            _untrustedSelfIssuedCertificateInCertificateStore?.Dispose();
            _trustedTestCertChain?.Dispose();
            _revokedTestCertChain?.Dispose();
            _revocationUnknownTestCertChain?.Dispose();
            _crlServer?.Stop();
            _crlServer?.Dispose();
            _testDirectory?.Dispose();
            _responders.Dispose();

            if (_testServer.IsValueCreated)
            {
                _testServer.Value.Result.Dispose();
            }
        }

        private async Task<CertificateAuthority> CreateDefaultTrustedCertificateAuthorityAsync()
        {
            var testServer = await _testServer.Value;
            var rootCa = CertificateAuthority.Create(testServer.Url);
            var intermediateCa = rootCa.CreateIntermediateCertificateAuthority();
            var rootCertificate = new X509Certificate2(rootCa.Certificate.GetEncoded());

            _trustedTimestampRoot = TrustedTestCert.Create(
                rootCertificate,
                X509StorePurpose.CodeSigning,
                StoreName.Root,
                StoreLocation.LocalMachine);

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
    }
}
