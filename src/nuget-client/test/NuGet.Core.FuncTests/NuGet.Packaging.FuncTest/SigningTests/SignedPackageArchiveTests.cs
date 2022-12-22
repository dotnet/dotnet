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
    public class SignedPackageArchiveTests
    {
        private readonly SigningTestFixture _fixture;

        public SignedPackageArchiveTests(SigningTestFixture fixture)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            _fixture = fixture;
        }

        [CIOnlyFact]
        public async Task RemoveSignatureAsync_RemovesPackageSignatureAsync()
        {
            using (var test = await Test.CreateAsync(_fixture))
            {
                await test.Package.RemoveSignatureAsync(CancellationToken.None);

                var isSigned = await SignedArchiveTestUtility.IsSignedAsync(test.OutputPackageStream);

                Assert.False(isSigned);
            }
        }

        [CIOnlyFact]
        public async Task RemoveSignatureAsync_WithCancelledToken_ThrowsAsync()
        {
            using (var test = await Test.CreateAsync(_fixture))
            {
                await Assert.ThrowsAsync<OperationCanceledException>(
                    () => test.Package.RemoveSignatureAsync(new CancellationToken(canceled: true)));
            }
        }

        [CIOnlyFact]
        public async Task RemoveSignatureAsync_WithUnsignedPackage_ThrowsAsync()
        {
            using (var test = await Test.CreateUnsignedAsync())
            {
                var isSigned = await SignedArchiveTestUtility.IsSignedAsync(test.InputPackageStream);

                Assert.False(isSigned);

                var exception = await Assert.ThrowsAsync<SignatureException>(
                    () => test.Package.RemoveSignatureAsync(CancellationToken.None));

                Assert.Equal(NuGetLogCode.NU3000, exception.Code);
                Assert.Equal("The package is not signed. Unable to remove signature from an unsigned package.", exception.Message);
            }
        }

        private sealed class Test : IDisposable
        {
            internal MemoryStream InputPackageStream { get; }
            internal MemoryStream OutputPackageStream { get; }
            internal SignedPackageArchive Package { get; }

            private bool _isDisposed;

            private Test(MemoryStream inputPackageStream)
            {
                InputPackageStream = inputPackageStream;
                OutputPackageStream = new MemoryStream();

                Package = new SignedPackageArchive(InputPackageStream, OutputPackageStream);
            }

            internal static async Task<Test> CreateAsync(SigningTestFixture fixture, Stream unsignedPackage = null)
            {
                using (var certificate = new X509Certificate2(fixture.TrustedTestCertificate.Source.Cert))
                {
                    var signedPackageFile = await CreateSignedPackageAsync(certificate, unsignedPackage);

                    return new Test(signedPackageFile);
                }
            }

            internal static async Task<Test> CreateUnsignedAsync()
            {
                var packageContext = new SimpleTestPackageContext();
                var package = await packageContext.CreateAsStreamAsync();

                return new Test(package);
            }

            public void Dispose()
            {
                if (!_isDisposed)
                {
                    InputPackageStream.Dispose();
                    OutputPackageStream.Dispose();
                    Package?.Dispose();

                    GC.SuppressFinalize(this);

                    _isDisposed = true;
                }
            }

            private static async Task<MemoryStream> CreateSignedPackageAsync(
                X509Certificate2 certificate,
                Stream unsignedPackage = null)
            {
                if (unsignedPackage == null)
                {
                    var packageContext = new SimpleTestPackageContext();
                    unsignedPackage = await packageContext.CreateAsStreamAsync();
                }

                var signatureProvider = new X509SignatureProvider(timestampProvider: null);
                var overwrite = true;

                using (var request = new AuthorSignPackageRequest(certificate, HashAlgorithmName.SHA256))
                using (var outputPackageStream = new MemoryStream())
                using (var options = new SigningOptions(
                    new Lazy<Stream>(() => unsignedPackage),
                    new Lazy<Stream>(() => outputPackageStream),
                    overwrite,
                    signatureProvider,
                    NullLogger.Instance))
                {
                    await SigningUtility.SignAsync(options, request, CancellationToken.None);

                    var isSigned = await SignedArchiveTestUtility.IsSignedAsync(options.OutputPackageStream);

                    Assert.True(isSigned);

                    return new MemoryStream(outputPackageStream.ToArray());
                }
            }
        }
    }
}
#endif
