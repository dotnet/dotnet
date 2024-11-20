// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#if IS_SIGNING_SUPPORTED

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Internal.NuGet.Testing.SignedPackages;
using NuGet.Common;
using NuGet.Packaging.Signing;
using NuGet.Test.Utility;
using Test.Utility;
using Test.Utility.Signing;
using Xunit;

namespace NuGet.Packaging.FuncTest
{
    [Collection(SigningTestCollection.Name)]
    public class IntegrityVerificationProviderTests
    {
        private const string _packageTamperedError = "The package integrity check failed. The package has changed since it was signed. Try clearing the local http-cache and run nuget operation again.";
        private const string _packageUnsignedError = "The package is not signed.";
        private const string _packageInvalidSignatureError = "The package signature is invalid or cannot be verified on this platform.";

        private SigningTestFixture _testFixture;
        private TrustedTestCert<TestCertificate> _trustedTestCert;
        private IList<ISignatureVerificationProvider> _trustProviders;

        public IntegrityVerificationProviderTests(SigningTestFixture fixture)
        {
            _testFixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
            _trustedTestCert = _testFixture.TrustedTestCertificate;
            _trustProviders = new List<ISignatureVerificationProvider>()
            {
                new IntegrityVerificationProvider()
            };
        }

        private static SignedPackageVerifierSettings GetSettingsPolicy(string policyString)
        {
            if (StringComparer.Ordinal.Equals(policyString, "command"))
            {
                return SignedPackageVerifierSettings.GetVerifyCommandDefaultPolicy(TestEnvironmentVariableReader.EmptyInstance);
            }

            if (StringComparer.Ordinal.Equals(policyString, "vs"))
            {
                return SignedPackageVerifierSettings.GetAcceptModeDefaultPolicy(TestEnvironmentVariableReader.EmptyInstance);
            }

            return SignedPackageVerifierSettings.GetDefault(TestEnvironmentVariableReader.EmptyInstance);
        }

        [CIOnlyTheory]
        [InlineData("command")]
        [InlineData("vs")]
        public async Task Signer_VerifyOnSignedPackageAsync(string policyString)
        {
            // Arrange
            var nupkg = new SimpleTestPackageContext();
            var policy = GetSettingsPolicy(policyString);

            using (var dir = TestDirectory.Create())
            using (var testCertificate = new X509Certificate2(_trustedTestCert.Source.Cert))
            {
                var signedPackagePath = await SignedArchiveTestUtility.AuthorSignPackageAsync(testCertificate, nupkg, dir);
                var verifier = new PackageSignatureVerifier(_trustProviders);

                using (var packageReader = new PackageArchiveReader(signedPackagePath))
                {
                    // Act
                    var result = await verifier.VerifySignaturesAsync(packageReader, policy, CancellationToken.None);

                    // Assert
                    result.IsValid.Should().BeTrue();
                }
            }
        }

        [CIOnlyTheory]
        [InlineData("command")]
        [InlineData("vs")]
        public async Task Signer_VerifyOnTamperedPackage_FileDeletedAsync(string policyString)
        {
            // Arrange
            var nupkg = new SimpleTestPackageContext();
            var policy = GetSettingsPolicy(policyString);

            using (var dir = TestDirectory.Create())
            using (var testCertificate = new X509Certificate2(_trustedTestCert.Source.Cert))
            {
                var signedPackagePath = await SignedArchiveTestUtility.AuthorSignPackageAsync(testCertificate, nupkg, dir);
                SignedArchiveTestUtility.TamperWithPackage(signedPackagePath);

                var verifier = new PackageSignatureVerifier(_trustProviders);

                using (var packageReader = new PackageArchiveReader(signedPackagePath))
                {
                    // Act
                    var result = await verifier.VerifySignaturesAsync(packageReader, policy, CancellationToken.None);
                    var resultsWithErrors = result.Results.Where(r => r.GetErrorIssues().Any());
                    var totalErrorIssues = resultsWithErrors.SelectMany(r => r.GetErrorIssues());

                    // Assert
                    result.IsValid.Should().BeFalse();
                    resultsWithErrors.Count().Should().Be(1);
                    totalErrorIssues.Count().Should().Be(1);
                    totalErrorIssues.First().Code.Should().Be(NuGetLogCode.NU3008);
                    totalErrorIssues.First().Message.Should().Be(_packageTamperedError);
                }
            }
        }

        [CIOnlyTheory]
        [InlineData("command")]
        [InlineData("vs")]
        public async Task Signer_VerifyOnTamperedPackage_FileAddedAsync(string policyString)
        {
            // Arrange
            var nupkg = new SimpleTestPackageContext();
            var policy = GetSettingsPolicy(policyString);
            var newEntryData = "malicious code";
            var newEntryName = "malicious file";

            using (var dir = TestDirectory.Create())
            using (var testCertificate = new X509Certificate2(_trustedTestCert.Source.Cert))
            {
                var signedPackagePath = await SignedArchiveTestUtility.AuthorSignPackageAsync(testCertificate, nupkg, dir);

                // tamper with the package
                using (var stream = File.Open(signedPackagePath, FileMode.Open))
                using (var zip = new ZipArchive(stream, ZipArchiveMode.Update))
                using (var newEntryStream = zip.CreateEntry(newEntryName).Open())
                using (var newEntryDataStream = new MemoryStream(Encoding.UTF8.GetBytes(newEntryData)))
                {
                    newEntryStream.Seek(offset: 0, origin: SeekOrigin.End);
                    newEntryDataStream.CopyTo(newEntryStream);
                }

                var verifier = new PackageSignatureVerifier(_trustProviders);

                using (var packageReader = new PackageArchiveReader(signedPackagePath))
                {
                    // Act
                    var result = await verifier.VerifySignaturesAsync(packageReader, policy, CancellationToken.None);
                    var resultsWithErrors = result.Results.Where(r => r.GetErrorIssues().Any());
                    var totalErrorIssues = resultsWithErrors.SelectMany(r => r.GetErrorIssues());

                    // Assert
                    result.IsValid.Should().BeFalse();
                    resultsWithErrors.Count().Should().Be(1);
                    totalErrorIssues.Count().Should().Be(1);
                    totalErrorIssues.First().Code.Should().Be(NuGetLogCode.NU3008);
                    totalErrorIssues.First().Message.Should().Be(_packageTamperedError);
                }
            }
        }

        [CIOnlyTheory]
        [InlineData("command")]
        [InlineData("vs")]
        public async Task Signer_VerifyOnTamperedPackage_FileAppendedAsync(string policyString)
        {
            // Arrange
            var nupkg = new SimpleTestPackageContext();
            var policy = GetSettingsPolicy(policyString);
            var extraData = "tampering data";

            using (var dir = TestDirectory.Create())
            using (var testCertificate = new X509Certificate2(_trustedTestCert.Source.Cert))
            {
                var signedPackagePath = await SignedArchiveTestUtility.AuthorSignPackageAsync(testCertificate, nupkg, dir);

                // tamper with the package
                using (var stream = File.Open(signedPackagePath, FileMode.Open))
                using (var zip = new ZipArchive(stream, ZipArchiveMode.Update))
                using (var entryStream = zip.Entries.First().Open())
                using (var extraStream = new MemoryStream(Encoding.UTF8.GetBytes(extraData)))
                {
                    entryStream.Seek(offset: 0, origin: SeekOrigin.End);
                    extraStream.CopyTo(entryStream);
                }

                var verifier = new PackageSignatureVerifier(_trustProviders);

                using (var packageReader = new PackageArchiveReader(signedPackagePath))
                {
                    // Act
                    var result = await verifier.VerifySignaturesAsync(packageReader, policy, CancellationToken.None);
                    var resultsWithErrors = result.Results.Where(r => r.GetErrorIssues().Any());
                    var totalErrorIssues = resultsWithErrors.SelectMany(r => r.GetErrorIssues());

                    // Assert
                    result.IsValid.Should().BeFalse();
                    resultsWithErrors.Count().Should().Be(1);
                    totalErrorIssues.Count().Should().Be(1);
                    totalErrorIssues.First().Code.Should().Be(NuGetLogCode.NU3008);
                    totalErrorIssues.First().Message.Should().Be(_packageTamperedError);
                }
            }
        }

        [CIOnlyTheory]
        [InlineData("command")]
        [InlineData("vs")]
        public async Task Signer_VerifyOnTamperedPackage_FileTruncatedAsync(string policyString)
        {
            // Arrange
            var nupkg = new SimpleTestPackageContext();
            var policy = GetSettingsPolicy(policyString);

            using (var dir = TestDirectory.Create())
            using (var testCertificate = new X509Certificate2(_trustedTestCert.Source.Cert))
            {
                var signedPackagePath = await SignedArchiveTestUtility.AuthorSignPackageAsync(testCertificate, nupkg, dir);

                // tamper with the package
                using (var stream = File.Open(signedPackagePath, FileMode.Open))
                using (var zip = new ZipArchive(stream, ZipArchiveMode.Update))
                using (var entryStream = zip.Entries.First().Open())
                {
                    entryStream.SetLength(entryStream.Length - 1);
                }

                var verifier = new PackageSignatureVerifier(_trustProviders);

                using (var packageReader = new PackageArchiveReader(signedPackagePath))
                {
                    // Act
                    var result = await verifier.VerifySignaturesAsync(packageReader, policy, CancellationToken.None);
                    var resultsWithErrors = result.Results.Where(r => r.GetErrorIssues().Any());
                    var totalErrorIssues = resultsWithErrors.SelectMany(r => r.GetErrorIssues());

                    // Assert
                    result.IsValid.Should().BeFalse();
                    resultsWithErrors.Count().Should().Be(1);
                    totalErrorIssues.Count().Should().Be(1);
                    totalErrorIssues.First().Code.Should().Be(NuGetLogCode.NU3008);
                    totalErrorIssues.First().Message.Should().Be(_packageTamperedError);
                }
            }
        }

        [CIOnlyTheory]
        [InlineData("command")]
        [InlineData("vs")]
        public async Task Signer_VerifyOnTamperedPackage_FileMetadataModifiedAsync(string policyString)
        {
            // Arrange
            var nupkg = new SimpleTestPackageContext();
            var policy = GetSettingsPolicy(policyString);

            using (var dir = TestDirectory.Create())
            using (var testCertificate = new X509Certificate2(_trustedTestCert.Source.Cert))
            {
                var signedPackagePath = await SignedArchiveTestUtility.AuthorSignPackageAsync(testCertificate, nupkg, dir);

                // tamper with the package
                using (var stream = File.Open(signedPackagePath, FileMode.Open))
                using (var zip = new ZipArchive(stream, ZipArchiveMode.Update))
                {
                    var entry = zip.Entries.First();

                    // ZipArchiveEntry.LastWriteTime supports a resolution of two seconds.
                    // https://msdn.microsoft.com/en-us/library/system.io.compression.ziparchiveentry.lastwritetime(v=vs.110).aspx
                    entry.LastWriteTime = entry.LastWriteTime.AddSeconds(2);
                }

                var verifier = new PackageSignatureVerifier(_trustProviders);

                using (var packageReader = new PackageArchiveReader(signedPackagePath))
                {
                    // Act
                    var result = await verifier.VerifySignaturesAsync(packageReader, policy, CancellationToken.None);
                    var resultsWithErrors = result.Results.Where(r => r.GetErrorIssues().Any());
                    var totalErrorIssues = resultsWithErrors.SelectMany(r => r.GetErrorIssues());

                    // Assert
                    result.IsValid.Should().BeFalse();
                    resultsWithErrors.Count().Should().Be(1);
                    totalErrorIssues.Count().Should().Be(1);
                    totalErrorIssues.First().Code.Should().Be(NuGetLogCode.NU3008);
                    totalErrorIssues.First().Message.Should().Be(_packageTamperedError);
                }
            }
        }

        [CIOnlyTheory]
        [InlineData("command", false)]
        [InlineData("vs", true)]
        public async Task Signer_VerifyOnTamperedPackage_SignatureRemovedAsync(string policyString, bool expectedValidity)
        {
            // Arrange
            var nupkg = new SimpleTestPackageContext();
            var policy = GetSettingsPolicy(policyString);

            using (var dir = TestDirectory.Create())
            using (var testCertificate = new X509Certificate2(_trustedTestCert.Source.Cert))
            {
                var signedPackagePath = await SignedArchiveTestUtility.AuthorSignPackageAsync(testCertificate, nupkg, dir);

                // unsign the package
                using (var stream = File.Open(signedPackagePath, FileMode.Open))
                using (var zip = new ZipArchive(stream, ZipArchiveMode.Update))
                {
                    var entry = zip.GetEntry(SigningSpecifications.V1.SignaturePath);
                    entry.Delete();
                }

                var verifier = new PackageSignatureVerifier(_trustProviders);

                using (var packageReader = new PackageArchiveReader(signedPackagePath))
                {
                    // Act
                    var result = await verifier.VerifySignaturesAsync(packageReader, policy, CancellationToken.None);
                    var resultsWithErrors = result.Results.Where(r => r.GetErrorIssues().Any());
                    var totalErrorIssues = resultsWithErrors.SelectMany(r => r.GetErrorIssues());

                    // Assert
                    result.IsValid.Should().Be(expectedValidity);
                    if (!expectedValidity)
                    {
                        resultsWithErrors.Count().Should().Be(1);
                        totalErrorIssues.Count().Should().Be(1);
                        totalErrorIssues.First().Code.Should().Be(NuGetLogCode.NU3004);
                        totalErrorIssues.First().Message.Should().Be(_packageUnsignedError);
                    }
                }
            }
        }

        [CIOnlyTheory]
        [InlineData("command", false)]
        [InlineData("vs", true)]
        public async Task Signer_VerifyOnTamperedPackage_SignatureTamperedAsync(string policyString, bool expectedValidity)
        {
            // Arrange
            var nupkg = new SimpleTestPackageContext();
            var policy = GetSettingsPolicy(policyString);

            using (var dir = TestDirectory.Create())
            using (var testCertificate = new X509Certificate2(_trustedTestCert.Source.Cert))
            {
                var signedPackagePath = await SignedArchiveTestUtility.AuthorSignPackageAsync(testCertificate, nupkg, dir);

                TamperSignature(signedPackagePath);

                var verifier = new PackageSignatureVerifier(_trustProviders);

                using (var packageReader = new PackageArchiveReader(signedPackagePath))
                {
                    // Act
                    var result = await verifier.VerifySignaturesAsync(packageReader, policy, CancellationToken.None);
                    var resultsWithErrors = result.Results.Where(r => r.GetErrorIssues().Any());
                    var totalErrorIssues = resultsWithErrors.SelectMany(r => r.GetErrorIssues());

                    // Assert
                    result.IsValid.Should().Be(expectedValidity);
                    if (!expectedValidity)
                    {
                        resultsWithErrors.Count().Should().Be(1);
                        totalErrorIssues.Count().Should().Be(1);
                        totalErrorIssues.First().Code.Should().Be(NuGetLogCode.NU3003);
                        totalErrorIssues.First().Message.Should().Be(_packageInvalidSignatureError);
                    }
                }
            }
        }

        private static void TamperSignature(string packageFilePath)
        {
            var packageSignature = ReadPackageSignature(packageFilePath);
            var randomBytes = new byte[packageSignature.Length];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }

            var fileBytes = File.ReadAllBytes(packageFilePath);
            var newFileBytes = SignatureTestUtility.FindAndReplaceSequence(
                fileBytes,
                packageSignature,
                randomBytes);

            File.WriteAllBytes(packageFilePath, newFileBytes);
        }

        private static byte[] ReadPackageSignature(string packageFilePath)
        {
            using (var stream = File.OpenRead(packageFilePath))
            using (var zip = new ZipArchive(stream, ZipArchiveMode.Read, leaveOpen: true))
            {
                var entry = zip.GetEntry(SigningSpecifications.V1.SignaturePath);

                using (var entryStream = entry.Open())
                using (var reader = new BinaryReader(entryStream))
                {
                    return reader.ReadBytes((int)entryStream.Length);
                }
            }
        }
    }
}
#endif
