// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Configuration;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using NuGet.Packaging.Signing;
using Test.Utility.Signing;

namespace NuGet.MSSigning.Extensions.FuncTest.Commands
{
    /// <summary>
    /// Used to bootstrap functional tests for signing.
    /// </summary>
    public class MSSignCommandTestFixture : IDisposable
    {
        private TrustedTestCert<TestCertificate> _trustedTestCertWithPrivateKey;
        private TrustedTestCert<TestCertificate> _trustedTestCertWithoutPrivateKey;
        private TrustedTestCert<X509Certificate2> _trustedTimestampRoot;

        private string _nugetExePath;
        private Lazy<Task<SigningTestServer>> _testServer;
        private Lazy<Task<CertificateAuthority>> _defaultTrustedTimestampingRootCertificateAuthority;
        private Lazy<Task<TimestampService>> _defaultTrustedTimestampService;
        private readonly DisposableList<IDisposable> _responders;

        public MSSignCommandTestFixture()
        {
            _testServer = new Lazy<Task<SigningTestServer>>(SigningTestServer.CreateAsync);
            _defaultTrustedTimestampingRootCertificateAuthority = new Lazy<Task<CertificateAuthority>>(CreateDefaultTrustedTimestampingRootCertificateAuthorityAsync);
            _defaultTrustedTimestampService = new Lazy<Task<TimestampService>>(CreateDefaultTrustedTimestampServiceAsync);
            _responders = new DisposableList<IDisposable>();
        }

        public TrustedTestCert<TestCertificate> TrustedTestCertificateWithPrivateKey
        {
            get
            {
                if (_trustedTestCertWithPrivateKey == null)
                {
                    var actionGenerator = SigningTestUtility.CertificateModificationGeneratorForCodeSigningEkuCert;

                    // Code Sign EKU needs trust to a root authority
                    // Add the cert to Root CA list in LocalMachine as it does not prompt a dialog
                    // This makes all the associated tests to require admin privilege
                    _trustedTestCertWithPrivateKey = TestCertificate.Generate(X509StorePurpose.CodeSigning, actionGenerator)
                        .WithPrivateKeyAndTrust(StoreName.Root);
                }

                return _trustedTestCertWithPrivateKey;
            }
        }

        public TrustedTestCert<TestCertificate> TrustedTestCertificateWithoutPrivateKey
        {
            get
            {
                if (_trustedTestCertWithoutPrivateKey == null)
                {
                    var actionGenerator = SigningTestUtility.CertificateModificationGeneratorForCodeSigningEkuCert;

                    // Code Sign EKU needs trust to a root authority
                    // Add the cert to Root CA list in LocalMachine as it does not prompt a dialog
                    // This makes all the associated tests to require admin privilege
                    _trustedTestCertWithoutPrivateKey = TestCertificate.Generate(X509StorePurpose.CodeSigning, actionGenerator)
                        .WithTrust();
                }

                return _trustedTestCertWithoutPrivateKey;
            }
        }

        public string NuGetExePath
        {
            get
            {
                if (_nugetExePath == null)
                {
                    // Use the non-ILMerged version
                    var targetDir = ConfigurationManager.AppSettings["TestTargetDir"] ?? Directory.GetCurrentDirectory();
                    _nugetExePath = Path.Combine(targetDir, "NuGet.exe");
                }

                return _nugetExePath;
            }
        }

        public async Task<TimestampService> GetDefaultTrustedTimestampServiceAsync()
        {
            return await _defaultTrustedTimestampService.Value;
        }

        private async Task<CertificateAuthority> CreateDefaultTrustedTimestampingRootCertificateAuthorityAsync()
        {
            var testServer = await _testServer.Value;
            var rootCa = CertificateAuthority.Create(testServer.Url);
            var intermediateCa = rootCa.CreateIntermediateCertificateAuthority();
            var rootCertificate = new X509Certificate2(rootCa.Certificate.GetEncoded());
            StoreLocation storeLocation = CertificateStoreUtilities.GetTrustedCertificateStoreLocation();

            _trustedTimestampRoot = TrustedTestCert.Create(
                rootCertificate,
                X509StorePurpose.Timestamping,
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
            var ca = await _defaultTrustedTimestampingRootCertificateAuthority.Value;
            var timestampService = TimestampService.Create(ca);

            _responders.Add(testServer.RegisterResponder(timestampService));

            return timestampService;
        }

        public void Dispose()
        {
            _trustedTestCertWithPrivateKey?.Dispose();
            _trustedTestCertWithoutPrivateKey?.Dispose();
            _trustedTimestampRoot?.Dispose();
            _responders.Dispose();

            if (_testServer.IsValueCreated)
            {
                _testServer.Value.Result.Dispose();
            }
        }
    }
}
