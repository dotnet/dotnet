// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using NuGet.Test.Utility;
using Test.Utility.Signing;

namespace NuGet.Tests.Apex
{
    public class SignedPackagesTestsApexFixture : VisualStudioHostFixtureFactory
    {
        private const string _testPackageName = "TestPackage";
        private const string _packageVersion = "1.0.0";

        private TrustedTestCert<TestCertificate> _trustedAuthorTestCert;
        private TrustedTestCert<TestCertificate> _trustedRepositoryTestCert;
        private TrustedTestCert<X509Certificate2> _trustedServerRoot;

        private SimpleTestPackageContext _authorSignedTestPackageV1;
        private SimpleTestPackageContext _repoSignedTestPackageV1;
        private SimpleTestPackageContext _repoCountersignedTestPackageV1;

        private Lazy<Task<SigningTestServer>> _testServer;
        private Lazy<Task<CertificateAuthority>> _defaultTrustedTimestampRootCertificateAuthority;
        private Lazy<Task<TimestampService>> _defaultTrustedTimestampService;
        private readonly DisposableList<IDisposable> _responders;

        public SignedPackagesTestsApexFixture()
        {
            _testServer = new Lazy<Task<SigningTestServer>>(SigningTestServer.CreateAsync);
            _defaultTrustedTimestampRootCertificateAuthority = new Lazy<Task<CertificateAuthority>>(CreateDefaultTrustedTimestampRootCertificateAuthorityAsync);
            _defaultTrustedTimestampService = new Lazy<Task<TimestampService>>(CreateDefaultTrustedTimestampServiceAsync);
            _responders = new DisposableList<IDisposable>();
        }

        public TrustedTestCert<TestCertificate> TrustedAuthorTestCertificate
        {
            get
            {
                if (_trustedAuthorTestCert == null)
                {
                    _trustedAuthorTestCert = SigningTestUtility.GenerateTrustedTestCertificate();
                }

                return _trustedAuthorTestCert;
            }
        }

        public TrustedTestCert<TestCertificate> TrustedRepositoryTestCertificate
        {
            get
            {
                if (_trustedRepositoryTestCert == null)
                {
                    _trustedRepositoryTestCert = SigningTestUtility.GenerateTrustedTestCertificate();
                }

                return _trustedRepositoryTestCert;
            }
        }

        public SimpleTestPackageContext AuthorSignedTestPackage
        {
            get
            {
                if (_authorSignedTestPackageV1 == null)
                {
                    _authorSignedTestPackageV1 = CommonUtility.CreateAuthorSignedPackage(
                        _testPackageName,
                        _packageVersion,
                        TrustedAuthorTestCertificate.Source.Cert);
                }

                return _authorSignedTestPackageV1;
            }
        }

        public SimpleTestPackageContext RepositorySignedTestPackage
        {
            get
            {
                if (_repoSignedTestPackageV1 == null)
                {
                    _repoSignedTestPackageV1 = CommonUtility.CreateRepositorySignedPackage(
                        _testPackageName,
                        _packageVersion,
                        TrustedRepositoryTestCertificate.Source.Cert,
                        new Uri("https://v3serviceIndex.test/api/index.json"));
                }

                return _repoSignedTestPackageV1;
            }
        }

        public SimpleTestPackageContext RepositoryCountersignedTestPackage
        {
            get
            {
                if (_repoCountersignedTestPackageV1 == null)
                {
                    _repoCountersignedTestPackageV1 = CommonUtility.CreateRepositoryCountersignedPackage(
                        _testPackageName,
                        _packageVersion,
                        TrustedAuthorTestCertificate.Source.Cert,
                        TrustedRepositoryTestCertificate.Source.Cert,
                        new Uri("https://v3serviceIndex.test/api/index.json"));
                }

                return _repoCountersignedTestPackageV1;
            }
        }

        public async Task<TimestampService> GetDefaultTrustedTimestampServiceAsync()
        {
            return await _defaultTrustedTimestampService.Value;
        }

        private async Task<TimestampService> CreateDefaultTrustedTimestampServiceAsync()
        {
            var testServer = await _testServer.Value;
            var ca = await _defaultTrustedTimestampRootCertificateAuthority.Value;
            var timestampService = TimestampService.Create(ca);

            _responders.Add(testServer.RegisterResponder(timestampService));

            return timestampService;
        }

        private async Task<CertificateAuthority> CreateDefaultTrustedTimestampRootCertificateAuthorityAsync()
        {
            var testServer = await _testServer.Value;
            var rootCa = CertificateAuthority.Create(testServer.Url);
            var intermediateCa = rootCa.CreateIntermediateCertificateAuthority();
            var rootCertificate = new X509Certificate2(rootCa.Certificate.GetEncoded());

            _trustedServerRoot = TrustedTestCert.Create(
                rootCertificate,
                X509StorePurpose.Timestamping,
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

        public override void Dispose()
        {
            _trustedAuthorTestCert?.Dispose();
            _trustedRepositoryTestCert?.Dispose();
            _trustedServerRoot?.Dispose();
            _responders.Dispose();

            if (_testServer.IsValueCreated)
            {
                _testServer.Value.Result.Dispose();
            }

            base.Dispose();
        }
    }
}
