// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#if IS_SIGNING_SUPPORTED

using System;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Internal.NuGet.Testing.SignedPackages;
using Microsoft.Internal.NuGet.Testing.SignedPackages.Asn1;
using NuGet.Common;
using NuGet.Packaging.Signing;
using NuGet.Test.Utility;
using Test.Utility.Signing;
using Xunit;

namespace NuGet.Packaging.FuncTest
{
    [Collection(SigningTestCollection.Name)]
    public class PrimarySignatureTests
    {
        private readonly SigningTestFixture _testFixture;
        private readonly TrustedTestCert<TestCertificate> _trustedTestCert;

        public PrimarySignatureTests(SigningTestFixture fixture)
        {
            _testFixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
            _trustedTestCert = _testFixture.TrustedTestCertificate;
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

            using (TestDirectory directory = TestDirectory.Create())
            using (var certificate = new X509Certificate2(_trustedTestCert.Source.Cert))
            {
                string packageFilePath = await SignedArchiveTestUtility.AuthorSignPackageAsync(
                    certificate,
                    packageContext,
                    directory);

                byte[] signatureFileBytes = ReadSignatureFile(packageFilePath);
                TestSignedCms testSignedCms = TestSignedCms.Decode(signatureFileBytes);

                testSignedCms.Certificates.Clear();

                SignedCms signedCms = testSignedCms.Encode();

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
                    PrimarySignature signature = (await packageReader.GetPrimarySignatureAsync(CancellationToken.None));
                    TestSignedCms testSignedCms = TestSignedCms.Decode(signature.SignedCms.Encode());

                    testSignedCms.Certificates.Clear();
                    testSignedCms.Certificates.Add(certificate2);

                    SignedCms signedCms = testSignedCms.Encode();

                    SignatureException exception = Assert.Throws<SignatureException>(
                        () => PrimarySignature.Load(signedCms.Encode()));

                    Assert.Equal(NuGetLogCode.NU3011, exception.Code);
                    Assert.Equal("A certificate referenced by the signing-certificate-v2 attribute could not be found.", exception.Message);
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
