// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using NuGet.Packaging.Signing;
using NuGet.Test.Utility;
using Org.BouncyCastle.Asn1.X509;
using Test.Utility.Signing;

namespace NuGet.Signing.CrossFramework.Test
{
    public class CrossVerifyTestFixture : IDisposable
    {
#if IS_DESKTOP
        private const string DotnetExe = "dotnet.exe";
        //In net472 code path, the SDK version and TFM could not be detected automatically, so we manually specified according to the sdk version we're testing against.
        //https://github.com/NuGet/Home/issues/12187
        private const string SdkVersion = "7";
        private const string SdkTfm = "net5.0";
        internal string _dotnetExePath;
#else
        private const string NuGetExe = "NuGet.exe";
        internal string _nugetExePath;
#endif
        private TrustedTestCert<X509Certificate2> _trustedTimestampRoot;
        private Lazy<Task<SigningTestServer>> _testServer;
        private Lazy<Task<CertificateAuthority>> _defaultTrustedTimestampingRootCertificateAuthority;
        private Lazy<Task<TimestampService>> _defaultTrustedTimestampService;
        private readonly DisposableList<IDisposable> _responders;
        private Lazy<Task<X509Certificate2>> _defaultAuthorSigningCertficate;
        private Lazy<Task<X509Certificate2>> _defaultRepositorySigningCertficate;
        private TrustedTestCert<TestCertificate> _trustedTestCert;

        public CrossVerifyTestFixture()
        {
#if IS_DESKTOP
            var patchedCliFolder = TestDotnetCLiUtility.CopyAndPatchLatestDotnetCli(sdkVersion: SdkVersion, sdkTfm: SdkTfm);
            _dotnetExePath = Path.Combine(patchedCliFolder, DotnetExe);
#else
            var nugetExeFolder = TestFileSystemUtility.GetNuGetExeDirectoryInRepo();
            _nugetExePath = Path.Combine(nugetExeFolder, NuGetExe);
#endif
            _testServer = new Lazy<Task<SigningTestServer>>(SigningTestServer.CreateAsync);
            _defaultTrustedTimestampingRootCertificateAuthority = new Lazy<Task<CertificateAuthority>>(CreateDefaultTrustedTimestampingRootCertificateAuthorityAsync);
            _defaultTrustedTimestampService = new Lazy<Task<TimestampService>>(CreateDefaultTrustedTimestampServiceAsync);
            _responders = new DisposableList<IDisposable>();
            _defaultAuthorSigningCertficate = new Lazy<Task<X509Certificate2>>(CreateDefaultAuthorSigningCertificateAsync);
            _defaultRepositorySigningCertficate = new Lazy<Task<X509Certificate2>>(CreateDefaultRepositorySigningCertificateAsync);
        }

        public async Task<ISigningTestServer> GetSigningTestServerAsync()
        {
            return await _testServer.Value;
        }

        public async Task<CertificateAuthority> GetDefaultTrustedCertificateAuthorityAsync()
        {
            return await _defaultTrustedTimestampingRootCertificateAuthority.Value;
        }

        public async Task<TimestampService> GetDefaultTrustedTimestampServiceAsync()
        {
            return await _defaultTrustedTimestampService.Value;
        }

        public async Task<X509Certificate2> GetDefaultAuthorSigningCertificateAsync()
        {
            return await _defaultAuthorSigningCertficate.Value;
        }

        public async Task<X509Certificate2> GetDefaultRepositorySigningCertificateAsync()
        {
            return await _defaultRepositorySigningCertficate.Value;
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

        private async Task<CertificateAuthority> CreateDefaultTrustedTimestampingRootCertificateAuthorityAsync()
        {
            var testServer = await _testServer.Value;
            var rootCa = CertificateAuthority.Create(testServer.Url);
            var intermediateCa = rootCa.CreateIntermediateCertificateAuthority();
            var rootCertificate = new X509Certificate2(rootCa.Certificate.GetEncoded());

            _trustedTimestampRoot = TrustedTestCert.Create(
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

        private async Task<TimestampService> CreateDefaultTrustedTimestampServiceAsync()
        {
            var testServer = await _testServer.Value;
            var ca = await _defaultTrustedTimestampingRootCertificateAuthority.Value;
            var timestampService = TimestampService.Create(ca);

            _responders.Add(testServer.RegisterResponder(timestampService));

            return timestampService;
        }

        private async Task<X509Certificate2> CreateDefaultAuthorSigningCertificateAsync()
        {
            var ca = await CreateDefaultTrustedTimestampingRootCertificateAuthorityAsync();
            var keyPair = SigningTestUtility.GenerateKeyPair(publicKeyLength: 2048);
            var issueOptions = new IssueCertificateOptions()
            {
                KeyPair = keyPair,
                NotBefore = DateTimeOffset.UtcNow.AddSeconds(-2),
                NotAfter = DateTimeOffset.UtcNow.AddHours(1),
                SubjectName = new X509Name("CN=NuGet Cross Verify Test Author Signning Certificate")
            };
            var bcCertificate = ca.IssueCertificate(issueOptions);
            return CertificateUtilities.GetCertificateWithPrivateKey(bcCertificate, keyPair);
        }

        private async Task<X509Certificate2> CreateDefaultRepositorySigningCertificateAsync()
        {
            var ca = await CreateDefaultTrustedTimestampingRootCertificateAuthorityAsync();
            var keyPair = SigningTestUtility.GenerateKeyPair(publicKeyLength: 2048);
            var issueOptions = new IssueCertificateOptions()
            {
                KeyPair = keyPair,
                NotBefore = DateTimeOffset.UtcNow.AddSeconds(-2),
                NotAfter = DateTimeOffset.UtcNow.AddHours(1),
                SubjectName = new X509Name("CN=NuGet Cross Verify Test Repository Signning Certificate")
            };
            var bcCertificate = ca.IssueCertificate(issueOptions);
            return CertificateUtilities.GetCertificateWithPrivateKey(bcCertificate, keyPair);
        }

        public void Dispose()
        {
            _trustedTimestampRoot?.Dispose();

            if (_testServer.IsValueCreated)
            {
                _testServer.Value.Result.Dispose();
            }

            if (_defaultAuthorSigningCertficate.IsValueCreated)
            {
                _defaultAuthorSigningCertficate.Value.Result.Dispose();
            }

            if (_defaultRepositorySigningCertficate.IsValueCreated)
            {
                _defaultRepositorySigningCertficate.Value.Result.Dispose();
            }

            _responders.Dispose();
        }
    }
}
