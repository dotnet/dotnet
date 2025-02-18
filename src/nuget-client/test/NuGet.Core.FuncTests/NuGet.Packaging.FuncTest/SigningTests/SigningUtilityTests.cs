// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#if IS_SIGNING_SUPPORTED

using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Packaging.Signing;
using NuGet.Test.Utility;
using Test.Utility.Signing;
using Xunit;

namespace NuGet.Packaging.FuncTest
{
    [Collection(SigningTestCollection.Name)]
    public class SigningUtilityTests
    {
        private readonly SigningTestFixture _testFixture;

        public SigningUtilityTests(SigningTestFixture fixture)
        {
            _testFixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
        }

        [CIOnlyFact]
        public void Verify_WithValidInput_DoesNotThrow()
        {
#if NET9_0_OR_GREATER
            using (X509Certificate2 certificate = X509CertificateLoader.LoadCertificate(_testFixture.TrustedTestCertificate.Source.PublicCert.RawData))
#else
            using (var certificate = new X509Certificate2(_testFixture.TrustedTestCertificate.Source.PublicCert.RawData))
#endif
            using (var request = new AuthorSignPackageRequest(certificate, HashAlgorithmName.SHA256, HashAlgorithmName.SHA256))
            {
                SigningUtility.Verify(request, NullLogger.Instance);
            }
        }

        [CIOnlyFact]
        public async Task SignAsync_AddsPackageAuthorSignatureAsync()
        {
            using (var test = await Test.CreateAsync(_testFixture.TrustedTestCertificate.Source.Cert))
            {
                await SigningUtility.SignAsync(test.Options, test.AuthorRequest, CancellationToken.None);

                using (var package = new PackageArchiveReader(test.Options.OutputPackageStream))
                {
                    var primarySignature = await package.GetPrimarySignatureAsync(CancellationToken.None);

                    Assert.IsType<AuthorPrimarySignature>(primarySignature);
                }
            }
        }

        [CIOnlyFact]
        public async Task SignAsync_AddsPackageRepositorySignatureAsync()
        {
            using (var test = await Test.CreateAsync(_testFixture.TrustedTestCertificate.Source.Cert))
            {
                await SigningUtility.SignAsync(test.Options, test.RepositoryRequest, CancellationToken.None);

                using (var package = new PackageArchiveReader(test.Options.OutputPackageStream))
                {
                    var primarySignature = await package.GetPrimarySignatureAsync(CancellationToken.None);

                    Assert.IsType<RepositoryPrimarySignature>(primarySignature);
                }
            }
        }

        [CIOnlyFact]
        public async Task SignAsync_WhenRepositoryCountersigningPrimarySignature_SucceedsAsync()
        {
            using (var test = await Test.CreateAsync(_testFixture.TrustedTestCertificate.Source.Cert))
            {
                await SigningUtility.SignAsync(test.Options, test.AuthorRequest, CancellationToken.None);

                using (var package = new PackageArchiveReader(test.Options.OutputPackageStream))
                {
                    var primarySignature = await package.GetPrimarySignatureAsync(CancellationToken.None);

                    Assert.IsType<AuthorPrimarySignature>(primarySignature);
                }

                using (var countersignatureOptions = SigningOptions.CreateFromFilePaths(
                    test.OutputFile.FullName,
                    test.GetNewTempFilePath(),
                    overwrite: false,
                    signatureProvider: new X509SignatureProvider(timestampProvider: null),
                    logger: NullLogger.Instance))
                {
                    await SigningUtility.SignAsync(countersignatureOptions, test.RepositoryRequest, CancellationToken.None);

                    var isRepositoryCountersigned = await SignedArchiveTestUtility.IsRepositoryCountersignedAsync(countersignatureOptions.OutputPackageStream);
                    Assert.True(isRepositoryCountersigned);
                }
            }
        }

        [CIOnlyFact]
        public async Task SignAsync_WithExpiredCertificate_ThrowsAsync()
        {
            using (var test = await Test.CreateAsync(_testFixture.TrustedTestCertificateExpired.Source.Cert))
            {
                var exception = await Assert.ThrowsAsync<SignatureException>(
                    () => SigningUtility.SignAsync(test.Options, test.AuthorRequest, CancellationToken.None));

                Assert.Equal(NuGetLogCode.NU3018, exception.Code);
                Assert.Contains("Certificate chain validation failed.", exception.Message);

                var isSigned = await SignedArchiveTestUtility.IsSignedAsync(test.Options.InputPackageStream);
                Assert.False(isSigned);

                Assert.False(test.OutputFile.Exists);
            }
        }

        [CIOnlyFact]
        public async Task SignAsync_WithNotYetValidCertificate_ThrowsAsync()
        {
            using (var test = await Test.CreateAsync(_testFixture.TrustedTestCertificateNotYetValid.Source.Cert))
            {
                var exception = await Assert.ThrowsAsync<SignatureException>(
                    () => SigningUtility.SignAsync(test.Options, test.AuthorRequest, CancellationToken.None));

                Assert.Equal(NuGetLogCode.NU3017, exception.Code);
                Assert.Contains("The signing certificate is not yet valid", exception.Message);

                var isSigned = await SignedArchiveTestUtility.IsSignedAsync(test.Options.InputPackageStream);
                Assert.False(isSigned);

                Assert.False(test.OutputFile.Exists);
            }
        }

        [CIOnlyFact]
        public async Task SignAsync_WhenRepositoryCountersigningRepositorySignedPackage_ThrowsAsync()
        {
            using (var test = await Test.CreateAsync(_testFixture.TrustedTestCertificate.Source.Cert))
            {
                await SigningUtility.SignAsync(test.Options, test.RepositoryRequest, CancellationToken.None);

                using (var package = new PackageArchiveReader(test.Options.OutputPackageStream))
                {
                    var primarySignature = await package.GetPrimarySignatureAsync(CancellationToken.None);

                    Assert.IsType<RepositoryPrimarySignature>(primarySignature);
                }

                using (var countersignatureOptions = SigningOptions.CreateFromFilePaths(
                    test.OutputFile.FullName,
                    test.GetNewTempFilePath(),
                    overwrite: false,
                    signatureProvider: new X509SignatureProvider(timestampProvider: null),
                    logger: NullLogger.Instance))
                {
                    var exception = await Assert.ThrowsAsync<SignatureException>(
                        () => SigningUtility.SignAsync(countersignatureOptions, test.RepositoryRequest, CancellationToken.None));

                    Assert.Equal(NuGetLogCode.NU3033, exception.Code);
                    Assert.Contains("A repository primary signature must not have a repository countersignature", exception.Message);
                }
            }
        }

        [CIOnlyFact]
        public async Task SignAsync_WhenRepositoryCountersigningRepositoryCountersignedPackage_ThrowsAsync()
        {
            using (var test = await Test.CreateAsync(_testFixture.TrustedTestCertificate.Source.Cert))
            {
                await SigningUtility.SignAsync(test.Options, test.AuthorRequest, CancellationToken.None);

                using (var package = new PackageArchiveReader(test.Options.OutputPackageStream))
                {
                    var isSigned = await SignedArchiveTestUtility.IsSignedAsync(test.Options.OutputPackageStream);
                    Assert.True(isSigned);
                }

                var countersignedPackageOutputPath = test.GetNewTempFilePath();
                using (var countersignatureOptions = SigningOptions.CreateFromFilePaths(
                    test.OutputFile.FullName,
                    countersignedPackageOutputPath,
                    overwrite: false,
                    signatureProvider: new X509SignatureProvider(timestampProvider: null),
                    logger: NullLogger.Instance))
                {
                    await SigningUtility.SignAsync(countersignatureOptions, test.RepositoryRequest, CancellationToken.None);

                    var isRepositoryCountersigned = await SignedArchiveTestUtility.IsRepositoryCountersignedAsync(countersignatureOptions.OutputPackageStream);
                    Assert.True(isRepositoryCountersigned);
                }

                using (var countersignatureOptions = SigningOptions.CreateFromFilePaths(
                    countersignedPackageOutputPath,
                    test.GetNewTempFilePath(),
                    overwrite: false,
                    signatureProvider: new X509SignatureProvider(timestampProvider: null),
                    logger: NullLogger.Instance))
                {
                    var exception = await Assert.ThrowsAsync<SignatureException>(
                        () => SigningUtility.SignAsync(countersignatureOptions, test.RepositoryRequest, CancellationToken.None));

                    Assert.Equal(NuGetLogCode.NU3032, exception.Code);
                    Assert.Contains("The package already contains a repository countersignature", exception.Message);
                }
            }
        }

        private sealed class Test : IDisposable
        {
            private readonly X509Certificate2 _authorCertificate;
            private readonly X509Certificate2 _repositoryCertificate;

            private readonly TestDirectory _directory;

            internal SigningOptions Options { get; }
            internal FileInfo OutputFile { get; }
            internal AuthorSignPackageRequest AuthorRequest { get; }
            internal RepositorySignPackageRequest RepositoryRequest { get; }

            private bool _isDisposed;

            private Test(X509Certificate2 certificate, TestDirectory dir, FileInfo package)
            {
                _authorCertificate = new X509Certificate2(certificate);
                _repositoryCertificate = new X509Certificate2(certificate);
                _directory = dir;

                var outputPath = GetNewTempFilePath();

                OutputFile = new FileInfo(outputPath);

                var signatureProvider = new X509SignatureProvider(timestampProvider: null);

                AuthorRequest = new AuthorSignPackageRequest(_authorCertificate, HashAlgorithmName.SHA256);
                RepositoryRequest = new RepositorySignPackageRequest(_repositoryCertificate, HashAlgorithmName.SHA256, HashAlgorithmName.SHA256, new Uri("https://test/api/index.json"), packageOwners: null);

                var overwrite = true;

                Options = SigningOptions.CreateFromFilePaths(package.FullName, outputPath, overwrite, signatureProvider, NullLogger.Instance);
            }

            internal static async Task<Test> CreateAsync(X509Certificate2 certificate)
            {
                var dir = TestDirectory.Create();
                var packageContext = new SimpleTestPackageContext();

                var packageFileName = Guid.NewGuid().ToString();
                var package = await packageContext.CreateAsFileAsync(dir, packageFileName);

                return new Test(certificate, dir, package);
            }

            internal string GetNewTempFilePath()
            {
                return Path.Combine(_directory, Guid.NewGuid().ToString());
            }

            public void Dispose()
            {
                if (!_isDisposed)
                {
                    AuthorRequest?.Dispose();
                    RepositoryRequest?.Dispose();
                    _authorCertificate?.Dispose();
                    _repositoryCertificate?.Dispose();
                    Options.Dispose();
                    _directory?.Dispose();

                    GC.SuppressFinalize(this);

                    _isDisposed = true;
                }
            }
        }
    }
}
#endif
