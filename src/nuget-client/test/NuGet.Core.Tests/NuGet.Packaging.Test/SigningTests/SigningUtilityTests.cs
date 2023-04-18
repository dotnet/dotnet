// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
#if IS_SIGNING_SUPPORTED
using System.Security.Cryptography.Pkcs;
#endif
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NuGet.Common;
using NuGet.Packaging.Signing;
using NuGet.Test.Utility;
using Test.Utility.Signing;
using Xunit;

namespace NuGet.Packaging.Test
{
    [Collection(SigningTestsCollection.Name)]
    public class SigningUtilityTests
    {
        private readonly CertificatesFixture _fixture;

        public SigningUtilityTests(CertificatesFixture fixture)
        {
            _fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
        }

        [Fact]
        public void Verify_WhenRequestNull_Throws()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => SigningUtility.Verify(request: null, logger: NullLogger.Instance));

            Assert.Equal("request", exception.ParamName);
        }

        [Fact]
        public void Verify_WhenLoggerNull_Throws()
        {
            using (var certificate = _fixture.GetDefaultCertificate())
            using (var request = CreateRequest(certificate))
            {
                var exception = Assert.Throws<ArgumentNullException>(
                    () => SigningUtility.Verify(request, logger: null));

                Assert.Equal("logger", exception.ParamName);
            }
        }

        [Fact]
        public void Verify_WithCertificateWithUnsupportedSignatureAlgorithm_Throws()
        {
            using (var certificate = _fixture.GetRsaSsaPssCertificate())
            using (var request = CreateRequest(certificate))
            {
                var exception = Assert.Throws<SignatureException>(
                    () => SigningUtility.Verify(request, NullLogger.Instance));

                Assert.Equal(NuGetLogCode.NU3013, exception.Code);
                Assert.Contains("The signing certificate has an unsupported signature algorithm.", exception.Message);
            }
        }

        [Fact]
        public void Verify_WithCertificateWithLifetimeSigningEku_Throws()
        {
            using (var certificate = _fixture.GetLifetimeSigningCertificate())
            using (var request = CreateRequest(certificate))
            {
                var exception = Assert.Throws<SignatureException>(
                    () => SigningUtility.Verify(request, NullLogger.Instance));

                Assert.Equal(NuGetLogCode.NU3015, exception.Code);
                Assert.Contains("The lifetime signing EKU in the signing certificate is not supported.", exception.Message);
            }
        }

        [Fact]
        public void Verify_WithNotYetValidCertificate_Throws()
        {
            using (var certificate = _fixture.GetNotYetValidCertificate())
            using (var request = CreateRequest(certificate))
            {
                var exception = Assert.Throws<SignatureException>(
                    () => SigningUtility.Verify(request, NullLogger.Instance));

                Assert.Equal(NuGetLogCode.NU3017, exception.Code);
                Assert.Contains("The signing certificate is not yet valid.", exception.Message);
            }
        }

        [Fact]
        public void Verify_WhenChainBuildingFails_Throws()
        {
            using (var certificate = _fixture.GetExpiredCertificate())
            using (var request = CreateRequest(certificate))
            {
                var logger = new TestLogger();

                var exception = Assert.Throws<SignatureException>(
                    () => SigningUtility.Verify(request, logger));

                Assert.Equal(NuGetLogCode.NU3018, exception.Code);
                Assert.Equal("Certificate chain validation failed.", exception.Message);

                Assert.Equal(1, logger.Errors);

#if !NETCORE5_0
                if (RuntimeEnvironmentHelper.IsLinux)
                {
                    SigningTestUtility.AssertRevocationStatusUnknown(logger.LogMessages, LogLevel.Warning);
                }
#endif

                SigningTestUtility.AssertNotTimeValid(logger.LogMessages, LogLevel.Error);
                SigningTestUtility.AssertUntrustedRoot(logger.LogMessages, LogLevel.Warning);
            }
        }

        [Fact]
        public void Verify_WithUntrustedSelfSignedCertificate_Succeeds()
        {
            using (var certificate = _fixture.GetDefaultCertificate())
            using (var request = CreateRequest(certificate))
            {
                var logger = new TestLogger();

                SigningUtility.Verify(request, logger);

                Assert.Equal(0, logger.Errors);
                SigningTestUtility.AssertUntrustedRoot(logger.LogMessages, LogLevel.Warning);


#if !NETCORE5_0
                if (RuntimeEnvironmentHelper.IsLinux)
                {
                    SigningTestUtility.AssertRevocationStatusUnknown(logger.LogMessages, LogLevel.Warning);
                }
#endif
            }

        }

#if IS_SIGNING_SUPPORTED
        [Fact]
        public void CreateSignedAttributes_SignPackageRequest_WhenRequestNull_Throws()
        {
            using (var certificate = _fixture.GetDefaultCertificate())
            {
                var exception = Assert.Throws<ArgumentNullException>(
                    () => SigningUtility.CreateSignedAttributes((SignPackageRequest)null, new[] { certificate }));

                Assert.Equal("request", exception.ParamName);
            }
        }

        [Fact]
        public void CreateSignedAttributes_SignPackageRequest_WhenChainListNull_Throws()
        {
            using (var certificate = _fixture.GetDefaultCertificate())
            using (var request = CreateRequest(certificate))
            {
                var exception = Assert.Throws<ArgumentException>(
                    () => SigningUtility.CreateSignedAttributes(request, chainList: null));

                Assert.Equal("chainList", exception.ParamName);
                Assert.StartsWith("The argument cannot be null or empty.", exception.Message);
            }
        }

        [Fact]
        public void CreateSignedAttributes_SignPackageRequest_WhenChainListEmpty_Throws()
        {
            using (var certificate = _fixture.GetDefaultCertificate())
            using (var request = CreateRequest(certificate))
            {
                var exception = Assert.Throws<ArgumentException>(
                    () => SigningUtility.CreateSignedAttributes(request, new X509Certificate2[0]));

                Assert.Equal("chainList", exception.ParamName);
                Assert.StartsWith("The argument cannot be null or empty.", exception.Message);
            }
        }

        [Fact]
        public void CreateSignedAttributes_SignPackageRequest_WithValidInput_ReturnsAttributes()
        {
            using (var rootCertificate = SigningTestUtility.GetCertificate("root.crt"))
            using (var intermediateCertificate = SigningTestUtility.GetCertificate("intermediate.crt"))
            using (var leafCertificate = SigningTestUtility.GetCertificate("leaf.crt"))
            using (var request = CreateRequest(leafCertificate))
            {
                var certList = new[] { leafCertificate, intermediateCertificate, rootCertificate };
                var attributes = SigningUtility.CreateSignedAttributes(request, certList);

                Assert.Equal(3, attributes.Count);

                VerifyAttributes(attributes, request);
            }
        }

        [Fact]
        public void CreateSignedAttributes_RepositorySignPackageRequest_WhenRequestNull_Throws()
        {
            using (var certificate = _fixture.GetDefaultCertificate())
            {
                var exception = Assert.Throws<ArgumentNullException>(
                    () => SigningUtility.CreateSignedAttributes(
                        (RepositorySignPackageRequest)null,
                        new[] { certificate }));

                Assert.Equal("request", exception.ParamName);
            }
        }

        [Fact]
        public void CreateSignedAttributes_RepositorySignPackageRequest_WhenChainListNull_Throws()
        {
            using (var certificate = _fixture.GetDefaultCertificate())
            using (var request = CreateRequestRepository(certificate, new Uri("https://test.test"), new[] { "a" }))
            {
                var exception = Assert.Throws<ArgumentException>(
                    () => SigningUtility.CreateSignedAttributes(request, chainList: null));

                Assert.Equal("chainList", exception.ParamName);
                Assert.StartsWith("The argument cannot be null or empty.", exception.Message);
            }
        }

        [Fact]
        public void CreateSignedAttributes_RepositorySignPackageRequest_WhenChainListEmpty_Throws()
        {
            using (var certificate = _fixture.GetDefaultCertificate())
            using (var request = CreateRequestRepository(certificate, new Uri("https://test.test"), new[] { "a" }))
            {
                var exception = Assert.Throws<ArgumentException>(
                    () => SigningUtility.CreateSignedAttributes(request, new X509Certificate2[0]));

                Assert.Equal("chainList", exception.ParamName);
                Assert.StartsWith("The argument cannot be null or empty.", exception.Message);
            }
        }

        [Fact]
        public void CreateSignedAttributes_RepositorySignPackageRequest_WhenPackageOwnersNull_ReturnsAttributes()
        {
            var v3ServiceIndexUrl = new Uri("https://test.test", UriKind.Absolute);
            IReadOnlyList<string> packageOwners = null;

            using (var certificate = _fixture.GetDefaultCertificate())
            using (var request = CreateRequestRepository(certificate, v3ServiceIndexUrl, packageOwners))
            {
                var attributes = SigningUtility.CreateSignedAttributes(request, new[] { certificate });

                Assert.Equal(4, attributes.Count);

                VerifyAttributesRepository(attributes, request);
            }
        }

        [Fact]
        public void CreateSignedAttributes_RepositorySignPackageRequest_WhenPackageOwnersEmpty_ReturnsAttributes()
        {
            var v3ServiceIndexUrl = new Uri("https://test.test", UriKind.Absolute);
            var packageOwners = Array.Empty<string>();

            using (var certificate = _fixture.GetDefaultCertificate())
            using (var request = CreateRequestRepository(certificate, v3ServiceIndexUrl, packageOwners))
            {
                var attributes = SigningUtility.CreateSignedAttributes(request, new[] { certificate });

                Assert.Equal(4, attributes.Count);

                VerifyAttributesRepository(attributes, request);
            }
        }

        [Fact]
        public void CreateSignedAttributes_RepositorySignPackageRequest_WhenPackageOwnersNonEmpty_ReturnsAttributes()
        {
            var v3ServiceIndexUrl = new Uri("https://test.test", UriKind.Absolute);
            var packageOwners = new[] { "a" };

            using (var certificate = _fixture.GetDefaultCertificate())
            using (var request = CreateRequestRepository(certificate, v3ServiceIndexUrl, packageOwners))
            {
                var attributes = SigningUtility.CreateSignedAttributes(request, new[] { certificate });

                Assert.Equal(5, attributes.Count);

                VerifyAttributesRepository(attributes, request);
            }
        }

        [Fact]
        public void CreateCmsSigner_WhenRequestNull_Throws()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => SigningUtility.CreateCmsSigner(request: null, logger: NullLogger.Instance));

            Assert.Equal("request", exception.ParamName);
        }

        [Fact]
        public void CreateCmsSigner_WhenLoggerNull_Throws()
        {
            using (var request = new AuthorSignPackageRequest(new X509Certificate2(), Common.HashAlgorithmName.SHA256))
            {
                var exception = Assert.Throws<ArgumentNullException>(
                    () => SigningUtility.CreateCmsSigner(request, logger: null));

                Assert.Equal("logger", exception.ParamName);
            }
        }

        [Fact]
        public void CreateCmsSigner_WithAuthorSignPackageRequest_ReturnsInstance()
        {
            using (var certificate = _fixture.GetDefaultCertificate())
            using (var request = new AuthorSignPackageRequest(certificate, Common.HashAlgorithmName.SHA256))
            {
                var signer = SigningUtility.CreateCmsSigner(request, NullLogger.Instance);

                Assert.Equal(request.Certificate, signer.Certificate);
                Assert.Equal(request.SignatureHashAlgorithm.ConvertToOidString(), signer.DigestAlgorithm.Value);

                VerifyAttributes(signer.SignedAttributes, request);
            }
        }

        [Fact]
        public void CreateCmsSigner_WithRepositorySignPackageRequest_ReturnsInstance()
        {
            var v3ServiceIndexUrl = new Uri("https://test.test", UriKind.Absolute);
            var packageOwners = new[] { "a", "b", "c" };

            using (var certificate = _fixture.GetDefaultCertificate())
            using (var request = new RepositorySignPackageRequest(
                certificate,
                Common.HashAlgorithmName.SHA256,
                Common.HashAlgorithmName.SHA256,
                v3ServiceIndexUrl,
                packageOwners))
            {
                var signer = SigningUtility.CreateCmsSigner(request, NullLogger.Instance);

                Assert.Equal(request.Certificate, signer.Certificate);
                Assert.Equal(request.SignatureHashAlgorithm.ConvertToOidString(), signer.DigestAlgorithm.Value);

                VerifyAttributesRepository(signer.SignedAttributes, request);
            }
        }

        private static void VerifyAttributes(
            System.Security.Cryptography.CryptographicAttributeObjectCollection attributes,
            SignPackageRequest request)
        {
            var pkcs9SigningTimeAttributeFound = false;
            var commitmentTypeIndicationAttributeFound = false;
            var signingCertificateV2AttributeFound = false;

            foreach (var attribute in attributes)
            {
                Assert.Equal(1, attribute.Values.Count);

                switch (attribute.Oid.Value)
                {
                    case "1.2.840.113549.1.9.5": // PKCS #9 signing time
                        Assert.IsType<Pkcs9SigningTime>(attribute.Values[0]);

                        pkcs9SigningTimeAttributeFound = true;
                        break;

                    case Oids.CommitmentTypeIndication:
                        var qualifier = CommitmentTypeQualifier.Read(attribute.Values[0].RawData);
                        var expectedCommitmentType = AttributeUtility.GetSignatureTypeOid(request.SignatureType);

                        Assert.Equal(expectedCommitmentType, qualifier.CommitmentTypeIdentifier.Value);

                        commitmentTypeIndicationAttributeFound = true;
                        break;

                    case Oids.SigningCertificateV2:
                        var signingCertificateV2 = SigningCertificateV2.Read(attribute.Values[0].RawData);

                        Assert.Equal(1, signingCertificateV2.Certificates.Count);

                        var essCertIdV2 = signingCertificateV2.Certificates[0];

                        Assert.Equal(SigningTestUtility.GetHash(request.Certificate, request.SignatureHashAlgorithm), essCertIdV2.CertificateHash);
                        Assert.Equal(request.SignatureHashAlgorithm.ConvertToOidString(), essCertIdV2.HashAlgorithm.Algorithm.Value);
                        Assert.Equal(request.Certificate.IssuerName.Name, essCertIdV2.IssuerSerial.GeneralNames[0].DirectoryName.Name);
                        SigningTestUtility.VerifySerialNumber(request.Certificate, essCertIdV2.IssuerSerial);
                        Assert.Null(signingCertificateV2.Policies);

                        signingCertificateV2AttributeFound = true;
                        break;
                }
            }

            Assert.True(pkcs9SigningTimeAttributeFound);
            Assert.True(commitmentTypeIndicationAttributeFound);
            Assert.True(signingCertificateV2AttributeFound);
        }

        private static void VerifyAttributesRepository(
            System.Security.Cryptography.CryptographicAttributeObjectCollection attributes,
            RepositorySignPackageRequest request)
        {
            VerifyAttributes(attributes, request);

            var nugetV3ServiceIndexUrlAttributeFound = false;
            var nugetPackageOwnersAttributeFound = false;

            foreach (var attribute in attributes)
            {
                Assert.Equal(1, attribute.Values.Count);

                switch (attribute.Oid.Value)
                {
                    case Oids.NuGetV3ServiceIndexUrl:
                        var nugetV3ServiceIndexUrl = NuGetV3ServiceIndexUrl.Read(attribute.Values[0].RawData);

                        Assert.True(nugetV3ServiceIndexUrl.V3ServiceIndexUrl.IsAbsoluteUri);
                        Assert.Equal(request.V3ServiceIndexUrl.OriginalString, nugetV3ServiceIndexUrl.V3ServiceIndexUrl.OriginalString);

                        nugetV3ServiceIndexUrlAttributeFound = true;
                        break;

                    case Oids.NuGetPackageOwners:
                        var nugetPackageOwners = NuGetPackageOwners.Read(attribute.Values[0].RawData);

                        Assert.Equal(request.PackageOwners, nugetPackageOwners.PackageOwners);

                        nugetPackageOwnersAttributeFound = true;
                        break;
                }
            }

            Assert.True(nugetV3ServiceIndexUrlAttributeFound);
            Assert.Equal(request.PackageOwners != null && request.PackageOwners.Count > 0, nugetPackageOwnersAttributeFound);
        }

        [Fact]
        public async Task SignAsync_WhenCancellationTokenIsCancelled_ThrowsAsync()
        {
            using (var test = SignTest.Create(new X509Certificate2(), HashAlgorithmName.SHA256))
            {
                await Assert.ThrowsAsync<OperationCanceledException>(
                    () => SigningUtility.SignAsync(test.Options, test.Request, new CancellationToken(canceled: true)));
            }
        }

        [Fact]
        public async Task SignAsync_WhenCertificateSignatureAlgorithmIsUnsupported_ThrowsAsync()
        {
            using (var certificate = SigningTestUtility.GenerateCertificate(
                "test",
                generator => { },
                Common.HashAlgorithmName.SHA256,
                System.Security.Cryptography.RSASignaturePaddingMode.Pss))
            using (var test = SignTest.Create(certificate, HashAlgorithmName.SHA256))
            {
                var exception = await Assert.ThrowsAsync<SignatureException>(
                    () => SigningUtility.SignAsync(test.Options, test.Request, CancellationToken.None));

                Assert.Equal(NuGetLogCode.NU3013, exception.Code);
                Assert.Contains("The signing certificate has an unsupported signature algorithm.", exception.Message);
            }
        }

        [Fact]
        public async Task SignAsync_WhenCertificatePublicKeyLengthIsUnsupported_ThrowsAsync()
        {
            using (var certificate = SigningTestUtility.GenerateCertificate(
                "test",
                generator => { },
                publicKeyLength: 1024))
            using (var test = SignTest.Create(certificate, HashAlgorithmName.SHA256))
            {
                var exception = await Assert.ThrowsAsync<SignatureException>(
                    () => SigningUtility.SignAsync(test.Options, test.Request, CancellationToken.None));

                Assert.Equal(NuGetLogCode.NU3014, exception.Code);
                Assert.Contains("The signing certificate does not meet a minimum public key length requirement.", exception.Message);
            }
        }

        [Fact]
        public async Task SignAsync_WhenPackageIsZip64_ThrowsAsync()
        {
            using (var test = SignTest.Create(
                _fixture.GetDefaultCertificate(),
                HashAlgorithmName.SHA256,
                SigningTestUtility.GetResourceBytes("CentralDirectoryHeaderWithZip64ExtraField.zip")))
            {
                var exception = await Assert.ThrowsAsync<SignatureException>(
                    () => SigningUtility.SignAsync(test.Options, test.Request, CancellationToken.None));

                Assert.Equal(NuGetLogCode.NU3006, exception.Code);
                Assert.Equal("Signed Zip64 packages are not supported.", exception.Message);
            }
        }

        [Fact]
        public async Task SignAsync_WhenChainBuildingFails_ThrowsAsync()
        {
            var package = new SimpleTestPackageContext();
            using (var packageStream = await package.CreateAsStreamAsync())
            using (var test = SignTest.Create(
                 _fixture.GetExpiredCertificate(),
                HashAlgorithmName.SHA256,
                packageStream.ToArray(),
                new X509SignatureProvider(timestampProvider: null)))
            {
                var exception = await Assert.ThrowsAsync<SignatureException>(
                    () => SigningUtility.SignAsync(test.Options, test.Request, CancellationToken.None));

                Assert.Equal(NuGetLogCode.NU3018, exception.Code);
                Assert.Equal("Certificate chain validation failed.", exception.Message);

                Assert.Equal(1, test.Logger.Errors);
                SigningTestUtility.AssertNotTimeValid(test.Logger.LogMessages, LogLevel.Error);
                SigningTestUtility.AssertUntrustedRoot(test.Logger.LogMessages, LogLevel.Warning);
            }
        }

        [Fact]
        public async Task SignAsync_WithUntrustedSelfSignedCertificate_SucceedsAsync()
        {
            var package = new SimpleTestPackageContext();

            using (var packageStream = await package.CreateAsStreamAsync())
            using (var test = SignTest.Create(
                 _fixture.GetDefaultCertificate(),
                HashAlgorithmName.SHA256,
                packageStream.ToArray(),
                new X509SignatureProvider(timestampProvider: null)))
            {
                await SigningUtility.SignAsync(test.Options, test.Request, CancellationToken.None);

                Assert.True(await SignedArchiveTestUtility.IsSignedAsync(test.Options.OutputPackageStream));

                Assert.Equal(0, test.Logger.Errors);
                SigningTestUtility.AssertUntrustedRoot(test.Logger.LogMessages, LogLevel.Warning);
            }
        }

        [Fact]
        public async Task SignAsync_WhenPackageEntryCountWouldRequireZip64_FailsAsync()
        {
            const ushort desiredFileCount = 0xFFFF - 1;

            var package = new SimpleTestPackageContext();

            var requiredFileCount = desiredFileCount - package.Files.Count;

            for (var i = 0; i < requiredFileCount - 1 /*nuspec*/; ++i)
            {
                package.AddFile(i.ToString());
            }

            using (var packageStream = await package.CreateAsStreamAsync())
            {
                using (var zipArchive = new ZipArchive(packageStream, ZipArchiveMode.Read, leaveOpen: true))
                {
                    // Sanity check before testing.
                    Assert.Equal(desiredFileCount, zipArchive.Entries.Count());
                }

                packageStream.Position = 0;

                using (var test = SignTest.Create(
                     _fixture.GetDefaultCertificate(),
                    HashAlgorithmName.SHA256,
                    packageStream.ToArray(),
                    new X509SignatureProvider(timestampProvider: null)))
                {
                    var exception = await Assert.ThrowsAsync<SignatureException>(
                        () => SigningUtility.SignAsync(test.Options, test.Request, CancellationToken.None));

                    Assert.Equal(NuGetLogCode.NU3039, exception.Code);
                    Assert.Equal("The package cannot be signed as it would require the Zip64 format.", exception.Message);

                    Assert.Equal(0, test.Options.OutputPackageStream.Length);
                    Assert.Equal(0, test.Logger.Errors);
                    SigningTestUtility.AssertUntrustedRoot(test.Logger.LogMessages, LogLevel.Warning);
                }
            }
        }

        private sealed class SignTest : IDisposable
        {
            private bool _isDisposed = false;
            private readonly TestDirectory _directory;

            internal SigningOptions Options { get; }
            internal SignPackageRequest Request { get; }
            internal TestLogger Logger { get; }

            private SignTest(SignPackageRequest request,
                TestDirectory directory,
                SigningOptions options,
                TestLogger logger)
            {
                Request = request;
                Options = options;
                Logger = logger;
                _directory = directory;
            }

            public void Dispose()
            {
                if (!_isDisposed)
                {
                    Request?.Dispose();
                    Options.Dispose();
                    _directory?.Dispose();

                    GC.SuppressFinalize(this);

                    _isDisposed = true;
                }
            }

            internal static SignTest Create(
                X509Certificate2 certificate,
                HashAlgorithmName hashAlgorithm,
                byte[] package = null,
                ISignatureProvider signatureProvider = null)
            {
                var directory = TestDirectory.Create();
                var signedPackageFile = new FileInfo(Path.Combine(directory, Guid.NewGuid().ToString()));
                var outputPackageFile = new FileInfo(Path.Combine(directory, Guid.NewGuid().ToString()));

                if (package == null)
                {
                    File.WriteAllBytes(signedPackageFile.FullName, Array.Empty<byte>());
                }
                else
                {
                    using (var fileStream = signedPackageFile.Create())
                    {
                        fileStream.Write(package, 0, package.Length);
                    }
                }

                signatureProvider = signatureProvider ?? Mock.Of<ISignatureProvider>();
                var logger = new TestLogger();
                var request = new AuthorSignPackageRequest(certificate, hashAlgorithm);
                var overwrite = false;
                var options = SigningOptions.CreateFromFilePaths(
                    signedPackageFile.FullName,
                    outputPackageFile.FullName,
                    overwrite,
                    signatureProvider,
                    logger);

                return new SignTest(
                    request,
                    directory,
                    options,
                    logger);
            }
        }
#endif


        private static AuthorSignPackageRequest CreateRequest(X509Certificate2 certificate)
        {
            return new AuthorSignPackageRequest(
                certificate,
                Common.HashAlgorithmName.SHA256,
                Common.HashAlgorithmName.SHA256);
        }

        private static RepositorySignPackageRequest CreateRequestRepository(
            X509Certificate2 certificate,
            Uri v3ServiceIndexUrl,
            IReadOnlyList<string> packageOwners)
        {
            return new RepositorySignPackageRequest(
                certificate,
                Common.HashAlgorithmName.SHA256,
                Common.HashAlgorithmName.SHA256,
                v3ServiceIndexUrl,
                packageOwners);
        }
    }
}
