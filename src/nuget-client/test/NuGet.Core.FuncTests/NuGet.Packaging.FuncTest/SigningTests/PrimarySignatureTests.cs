// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#if IS_SIGNING_SUPPORTED

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NuGet.Common;
using NuGet.Packaging.Signing;
using NuGet.Test.Utility;
using Org.BouncyCastle.Cms;
using Org.BouncyCastle.X509.Store;
using Test.Utility.Signing;
using Xunit;

namespace NuGet.Packaging.FuncTest
{
    [Collection(SigningTestCollection.Name)]
    public class PrimarySignatureTests
    {
        private SigningTestFixture _testFixture;
        private TrustedTestCert<TestCertificate> _trustedTestCert;
        private IList<ISignatureVerificationProvider> _trustProviders;
        private SigningSpecifications _signingSpecifications;

        public PrimarySignatureTests(SigningTestFixture fixture)
        {
            _testFixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
            _trustedTestCert = _testFixture.TrustedTestCertificate;
            _trustProviders = _testFixture.TrustProviders;
            _signingSpecifications = _testFixture.SigningSpecifications;
        }

        [CIOnlyFact]
        public async Task Signature_HasTimestampAsync()
        {
            // Arrange
            var nupkg = new SimpleTestPackageContext();
            var timestampService = await _testFixture.GetDefaultTrustedTimestampServiceAsync();

            using (var cert = new X509Certificate2(_trustedTestCert.Source.Cert))
            using (var dir = TestDirectory.Create())
            {
                // Act
                var signedPackagePath = await SignedArchiveTestUtility.AuthorSignPackageAsync(
                    cert,
                    nupkg,
                    dir,
                    timestampService.Url);

                // Assert
                using (var stream = File.OpenRead(signedPackagePath))
                using (var reader = new PackageArchiveReader(stream))
                {
                    var signature = await reader.GetPrimarySignatureAsync(CancellationToken.None);

                    signature.Should().NotBeNull();
                    signature.Timestamps.Should().NotBeEmpty();
                }
            }
        }

        [CIOnlyFact]
        public async Task Signature_HasNoTimestampAsync()
        {
            // Arrange
            var nupkg = new SimpleTestPackageContext();

            using (var dir = TestDirectory.Create())
            using (var testCertificate = new X509Certificate2(_trustedTestCert.Source.Cert))
            {
                // Act
                var signedPackagePath = await SignedArchiveTestUtility.AuthorSignPackageAsync(testCertificate, nupkg, dir);

                // Assert
                using (var stream = File.OpenRead(signedPackagePath))
                using (var reader = new PackageArchiveReader(stream))
                {
                    var signature = await reader.GetPrimarySignatureAsync(CancellationToken.None);

                    signature.Should().NotBeNull();
                    signature.Timestamps.Should().BeEmpty();
                }
            }
        }

        [CIOnlyFact]
        public async Task Load_WithPrimarySignatureWithNoCertificates_ThrowsAsync()
        {
            var packageContext = new SimpleTestPackageContext();

            using (var directory = TestDirectory.Create())
            using (var certificate = new X509Certificate2(_trustedTestCert.Source.Cert))
            {
                var packageFilePath = await SignedArchiveTestUtility.AuthorSignPackageAsync(
                    certificate,
                    packageContext,
                    directory);

                var signatureFileBytes = ReadSignatureFile(packageFilePath);
                var signedCms = new SignedCms();

                signedCms.Decode(signatureFileBytes);

                var certificateStore = X509StoreFactory.Create(
                    "Certificate/Collection",
                    new X509CollectionStoreParameters(Array.Empty<Org.BouncyCastle.X509.X509Certificate>()));
                var crlStore = X509StoreFactory.Create(
                    "CRL/Collection",
                    new X509CollectionStoreParameters(Array.Empty<Org.BouncyCastle.X509.X509Crl>()));

                using (var readStream = new MemoryStream(signedCms.Encode()))
                using (var writeStream = new MemoryStream())
                {
                    CmsSignedDataParser.ReplaceCertificatesAndCrls(
                        readStream,
                        certificateStore,
                        crlStore,
                        certificateStore,
                        writeStream);

                    signedCms.Decode(writeStream.ToArray());
                }

                Assert.Empty(signedCms.Certificates);

                var exception = Assert.Throws<SignatureException>(
                    () => PrimarySignature.Load(signedCms));

                Assert.Equal(NuGetLogCode.NU3010, exception.Code);
                Assert.Contains("The primary signature does not have a signing certificate.", exception.Message);
            }
        }

        [CIOnlyFact]
        public async Task Load_WithReissuedSigningCertificate_ThrowsAsync()
        {
            var certificates = _testFixture.TrustedTestCertificateWithReissuedCertificate;
            var packageContext = new SimpleTestPackageContext();

            using (var directory = TestDirectory.Create())
            using (var certificate1 = new X509Certificate2(certificates[0].Source.Cert))
            using (var certificate2 = new X509Certificate2(certificates[1].Source.Cert))
            {
                var packageFilePath = await SignedArchiveTestUtility.AuthorSignPackageAsync(
                    certificate1,
                    packageContext,
                    directory);

                using (var packageReader = new PackageArchiveReader(packageFilePath))
                {
                    var signature = (await packageReader.GetPrimarySignatureAsync(CancellationToken.None));

                    var certificateStore = X509StoreFactory.Create(
                        "Certificate/Collection",
                        new X509CollectionStoreParameters(
                            new[] { Org.BouncyCastle.Security.DotNetUtilities.FromX509Certificate(certificate2) }));
                    var emptyCertificateStore = X509StoreFactory.Create(
                        "Certificate/Collection",
                        new X509CollectionStoreParameters(Array.Empty<Org.BouncyCastle.X509.X509Certificate>()));
                    var crlStore = X509StoreFactory.Create(
                        "CRL/Collection",
                        new X509CollectionStoreParameters(Array.Empty<Org.BouncyCastle.X509.X509Crl>()));
                    var bytes = signature.SignedCms.Encode();

                    using (var readStream = new MemoryStream(bytes))
                    using (var writeStream = new MemoryStream())
                    {
                        CmsSignedDataParser.ReplaceCertificatesAndCrls(
                            readStream,
                            certificateStore,
                            crlStore,
                            emptyCertificateStore,
                            writeStream);

                        var exception = Assert.Throws<SignatureException>(
                            () => PrimarySignature.Load(writeStream.ToArray()));

                        Assert.Equal(NuGetLogCode.NU3011, exception.Code);
                        Assert.Equal("A certificate referenced by the signing-certificate-v2 attribute could not be found.", exception.Message);
                    }
                }
            }
        }

        private static byte[] ReadSignatureFile(string packageFilePath)
        {
            using (var stream = File.OpenRead(packageFilePath))
            using (var zip = new ZipArchive(stream))
            {
                var signatureFile = zip.GetEntry(SigningSpecifications.V1.SignaturePath);
                byte[] signatureBytes;

                using (var entryStream = signatureFile.Open())
                using (var reader = new BinaryReader(entryStream))
                {
                    signatureBytes = new byte[entryStream.Length];
                    reader.Read(signatureBytes, index: 0, count: signatureBytes.Length);
                }

                return signatureBytes;
            }
        }
    }
}
#endif
