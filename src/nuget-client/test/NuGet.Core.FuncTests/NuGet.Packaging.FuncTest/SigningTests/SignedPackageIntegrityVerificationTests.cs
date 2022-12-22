// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#if IS_SIGNING_SUPPORTED

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NuGet.Common;
using NuGet.Packaging.Signing;
using NuGet.Test.Utility;
using Test.Utility.Signing;
using Xunit;

namespace NuGet.Packaging.FuncTest
{
    [Collection(SigningTestCollection.Name)]
    public class SignedPackageIntegrityVerificationTests
    {
        private readonly UTF8Encoding _readerEncoding = new UTF8Encoding();
        private readonly UTF8Encoding _writerEncoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true);

        private readonly SigningSpecifications _specification = SigningSpecifications.V1;

        private SigningTestFixture _testFixture;
        private TrustedTestCert<TestCertificate> _trustedTestCert;
        private readonly IList<ISignatureVerificationProvider> _trustProviders;

        private readonly SignedPackageVerifierSettings _settings;

        public SignedPackageIntegrityVerificationTests(SigningTestFixture fixture)
        {
            _testFixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
            _trustedTestCert = _testFixture.TrustedTestCertificate;
            _trustProviders = new List<ISignatureVerificationProvider>()
            {
                new IntegrityVerificationProvider()
            };

            _settings = new SignedPackageVerifierSettings(
                allowUnsigned: true,
                allowIllegal: false,
                allowUntrusted: false,
                allowIgnoreTimestamp: true,
                allowMultipleTimestamps: true,
                allowNoTimestamp: true,
                allowUnknownRevocation: true,
                reportUnknownRevocation: true,
                verificationTarget: VerificationTarget.All,
                signaturePlacement: SignaturePlacement.Any,
                repositoryCountersignatureVerificationBehavior: SignatureVerificationBehavior.IfExistsAndIsNecessary,
                revocationMode: RevocationMode.Online);
        }

        [CIOnlyFact]
        public async Task VerifyOnShiftedSignaturePackage_WhenCentralDirectoryIsLastAndFileHeaderIsLastAsync()
        {
            // Arrange
            var nupkg = new SimpleTestPackageContext();
            using (var dir = TestDirectory.Create())
            using (var testCertificate = new X509Certificate2(_trustedTestCert.Source.Cert))
            {
                var signedPackagePath = await SignedArchiveTestUtility.AuthorSignPackageAsync(testCertificate, nupkg, dir);
                var entryCount = 0;

                using (var stream = File.Open(signedPackagePath, FileMode.Open))
                using (var zip = new ZipArchive(stream, ZipArchiveMode.Read))
                {
                    entryCount = zip.Entries.Count;
                }

                await SignatureTestUtility.ShiftSignatureMetadataAsync(_specification, signedPackagePath, dir, entryCount - 1, entryCount - 1);

                var verifier = new PackageSignatureVerifier(_trustProviders);

                using (var packageReader = new PackageArchiveReader(signedPackagePath))
                {
                    // Act
                    var result = await verifier.VerifySignaturesAsync(packageReader, _settings, CancellationToken.None);
                    var resultsWithErrors = result.Results.Where(r => r.GetErrorIssues().Any());

                    // Assert
                    result.IsValid.Should().BeTrue();
                    resultsWithErrors.Count().Should().Be(0);
                }
            }
        }

        [CIOnlyFact]
        public async Task VerifyOnShiftedSignaturePackage_WhenCentralDirectoryIsFirstAndFileHeaderIsFirstAsync()
        {
            // Arrange
            var nupkg = new SimpleTestPackageContext();
            using (var dir = TestDirectory.Create())
            using (var testCertificate = new X509Certificate2(_trustedTestCert.Source.Cert))
            {
                var signedPackagePath = await SignedArchiveTestUtility.AuthorSignPackageAsync(testCertificate, nupkg, dir);

                await SignatureTestUtility.ShiftSignatureMetadataAsync(_specification, signedPackagePath, dir, 0, 0);

                var verifier = new PackageSignatureVerifier(_trustProviders);

                using (var packageReader = new PackageArchiveReader(signedPackagePath))
                {
                    // Act
                    var result = await verifier.VerifySignaturesAsync(packageReader, _settings, CancellationToken.None);
                    var resultsWithErrors = result.Results.Where(r => r.GetErrorIssues().Any());

                    // Assert
                    result.IsValid.Should().BeTrue();
                    resultsWithErrors.Count().Should().Be(0);
                }
            }
        }

        [CIOnlyFact]
        public async Task VerifyOnShiftedSignaturePackage_WhenCentralDirectoryIsLastAndFileHeaderIsFirstAsync()
        {
            // Arrange
            var nupkg = new SimpleTestPackageContext();
            using (var dir = TestDirectory.Create())
            using (var testCertificate = new X509Certificate2(_trustedTestCert.Source.Cert))
            {
                var signedPackagePath = await SignedArchiveTestUtility.AuthorSignPackageAsync(testCertificate, nupkg, dir);
                var entryCount = 0;

                using (var stream = File.Open(signedPackagePath, FileMode.Open))
                using (var zip = new ZipArchive(stream, ZipArchiveMode.Read))
                {
                    entryCount = zip.Entries.Count;
                }

                await SignatureTestUtility.ShiftSignatureMetadataAsync(_specification, signedPackagePath, dir, entryCount - 1, 0);

                var verifier = new PackageSignatureVerifier(_trustProviders);

                using (var packageReader = new PackageArchiveReader(signedPackagePath))
                {
                    // Act
                    var result = await verifier.VerifySignaturesAsync(packageReader, _settings, CancellationToken.None);
                    var resultsWithErrors = result.Results.Where(r => r.GetErrorIssues().Any());

                    // Assert
                    result.IsValid.Should().BeTrue();
                    resultsWithErrors.Count().Should().Be(0);
                }
            }
        }

        [CIOnlyFact]
        public async Task VerifyOnShiftedSignaturePackage_WhenCentralDirectoryIsFirstAndFileHeaderIsLastAsync()
        {
            // Arrange
            var nupkg = new SimpleTestPackageContext();
            using (var dir = TestDirectory.Create())
            using (var testCertificate = new X509Certificate2(_trustedTestCert.Source.Cert))
            {
                var signedPackagePath = await SignedArchiveTestUtility.AuthorSignPackageAsync(testCertificate, nupkg, dir);
                var entryCount = 0;

                using (var stream = File.Open(signedPackagePath, FileMode.Open))
                using (var zip = new ZipArchive(stream, ZipArchiveMode.Read))
                {
                    entryCount = zip.Entries.Count;
                }

                await SignatureTestUtility.ShiftSignatureMetadataAsync(_specification, signedPackagePath, dir, 0, entryCount - 1);

                var verifier = new PackageSignatureVerifier(_trustProviders);

                using (var packageReader = new PackageArchiveReader(signedPackagePath))
                {
                    // Act
                    var result = await verifier.VerifySignaturesAsync(packageReader, _settings, CancellationToken.None);
                    var resultsWithErrors = result.Results.Where(r => r.GetErrorIssues().Any());

                    // Assert
                    result.IsValid.Should().BeTrue();
                    resultsWithErrors.Count().Should().Be(0);
                }
            }
        }

        [CIOnlyFact]
        public async Task VerifyOnShiftedSignaturePackage_WhenCentralDirectoryIsMiddleAndFileHeaderIsMiddleAsync()
        {
            // Arrange
            var nupkg = new SimpleTestPackageContext();
            using (var dir = TestDirectory.Create())
            using (var testCertificate = new X509Certificate2(_trustedTestCert.Source.Cert))
            {
                var signedPackagePath = await SignedArchiveTestUtility.AuthorSignPackageAsync(testCertificate, nupkg, dir);
                var entryCount = 0;

                using (var stream = File.Open(signedPackagePath, FileMode.Open))
                using (var zip = new ZipArchive(stream, ZipArchiveMode.Read))
                {
                    entryCount = zip.Entries.Count;
                }

                var middleEntry = (entryCount - 1) / 2;
                await SignatureTestUtility.ShiftSignatureMetadataAsync(_specification, signedPackagePath, dir, middleEntry - 1, middleEntry + 1);

                var verifier = new PackageSignatureVerifier(_trustProviders);

                using (var packageReader = new PackageArchiveReader(signedPackagePath))
                {
                    // Act
                    var result = await verifier.VerifySignaturesAsync(packageReader, _settings, CancellationToken.None);
                    var resultsWithErrors = result.Results.Where(r => r.GetErrorIssues().Any());

                    // Assert
                    result.IsValid.Should().BeTrue();
                    resultsWithErrors.Count().Should().Be(0);
                }
            }
        }

        [CIOnlyFact]
        public async Task VerifyOnInvalidSignatureFileEntry_SignatureIsCreatedUsingZipArchiveAsync()
        {
            // Arrange
            var nupkg = new SimpleTestPackageContext();

            using (var dir = TestDirectory.Create())
            using (var packageStream = await nupkg.CreateAsStreamAsync())
            using (var testCertificate = new X509Certificate2(_trustedTestCert.Source.Cert))
            {
                AuthorPrimarySignature signature = await SignedArchiveTestUtility.CreateAuthorSignatureForPackageAsync(testCertificate, packageStream);

                using (var package = new ZipArchive(packageStream, ZipArchiveMode.Update, leaveOpen: true))
                {
                    var signatureEntry = package.CreateEntry(_specification.SignaturePath);
                    using (var signatureStream = new MemoryStream(signature.GetBytes()))
                    using (var signatureEntryStream = signatureEntry.Open())
                    {
                        signatureStream.CopyTo(signatureEntryStream);
                    }
                }

                var verifier = new PackageSignatureVerifier(_trustProviders);
                using (var packageReader = new PackageArchiveReader(packageStream))
                {
                    // Act
                    var result = await verifier.VerifySignaturesAsync(packageReader, _settings, CancellationToken.None);
                    var resultsWithErrors = result.Results.Where(r => r.GetErrorIssues().Any());
                    var totalErrorIssues = resultsWithErrors.SelectMany(r => r.GetErrorIssues());

                    // Assert
                    result.IsValid.Should().BeFalse();
                    resultsWithErrors.Count().Should().Be(1);
                    totalErrorIssues.Count().Should().Be(1);
                    totalErrorIssues.First().Code.Should().Be(NuGetLogCode.NU3005);
                }
            }
        }

        [CIOnlyFact]
        public async Task VerifyOnInvalidSignatureFileEntry_SignatureIsCreatedUsingZipArchiveAssertExceptionAsync()
        {
            // Arrange
            var nupkg = new SimpleTestPackageContext();

            using (var dir = TestDirectory.Create())
            using (var packageStream = await nupkg.CreateAsStreamAsync())
            using (var testCertificate = new X509Certificate2(_trustedTestCert.Source.Cert))
            {
                AuthorPrimarySignature signature = await SignedArchiveTestUtility.CreateAuthorSignatureForPackageAsync(testCertificate, packageStream);

                using (var package = new ZipArchive(packageStream, ZipArchiveMode.Update, leaveOpen: true))
                {
                    var signatureEntry = package.CreateEntry(_specification.SignaturePath);
                    using (var signatureStream = new MemoryStream(signature.GetBytes()))
                    using (var signatureEntryStream = signatureEntry.Open())
                    {
                        signatureStream.CopyTo(signatureEntryStream);
                    }
                }

                // Assert
                AssertSignatureEntryMetadataThrowsException(packageStream);
            }
        }

        [CIOnlyFact]
        public async Task VerifyOnInvalidSignatureFileEntry_SignatureIsCreatedUsingZipArchiveAndCompressedAsync()
        {
            // Arrange
            var nupkg = new SimpleTestPackageContext();

            using (var dir = TestDirectory.Create())
            using (var packageStream = await nupkg.CreateAsStreamAsync())
            using (var testCertificate = new X509Certificate2(_trustedTestCert.Source.Cert))
            {
                AuthorPrimarySignature signature = await SignedArchiveTestUtility.CreateAuthorSignatureForPackageAsync(testCertificate, packageStream);

                using (var package = new ZipArchive(packageStream, ZipArchiveMode.Update, leaveOpen: true))
                {
                    var signatureEntry = package.CreateEntry(_specification.SignaturePath, CompressionLevel.Optimal);
                    using (var signatureStream = new MemoryStream(signature.GetBytes()))
                    using (var signatureEntryStream = signatureEntry.Open())
                    {
                        signatureStream.CopyTo(signatureEntryStream);
                    }
                }

                var verifier = new PackageSignatureVerifier(_trustProviders);
                using (var packageReader = new PackageArchiveReader(packageStream))
                {
                    // Act
                    var result = await verifier.VerifySignaturesAsync(packageReader, _settings, CancellationToken.None);
                    var resultsWithErrors = result.Results.Where(r => r.GetErrorIssues().Any());
                    var totalErrorIssues = resultsWithErrors.SelectMany(r => r.GetErrorIssues());

                    // Assert
                    result.IsValid.Should().BeFalse();
                    resultsWithErrors.Count().Should().Be(1);
                    totalErrorIssues.Count().Should().Be(1);
                    totalErrorIssues.First().Code.Should().Be(NuGetLogCode.NU3005);
                }
            }
        }

        [CIOnlyFact]
        public async Task VerifyOnInvalidSignatureFileEntry_SignatureIsCreatedUsingZipArchiveAndCompressedAssertExceptionAsync()
        {
            // Arrange
            var nupkg = new SimpleTestPackageContext();

            using (var dir = TestDirectory.Create())
            using (var packageStream = await nupkg.CreateAsStreamAsync())
            using (var testCertificate = new X509Certificate2(_trustedTestCert.Source.Cert))
            {
                AuthorPrimarySignature signature = await SignedArchiveTestUtility.CreateAuthorSignatureForPackageAsync(testCertificate, packageStream);

                using (var package = new ZipArchive(packageStream, ZipArchiveMode.Update, leaveOpen: true))
                {
                    var signatureEntry = package.CreateEntry(_specification.SignaturePath, CompressionLevel.Optimal);
                    using (var signatureStream = new MemoryStream(signature.GetBytes()))
                    using (var signatureEntryStream = signatureEntry.Open())
                    {
                        signatureStream.CopyTo(signatureEntryStream);
                    }
                }

                // Assert
                AssertSignatureEntryMetadataThrowsException(packageStream);
            }
        }

        [CIOnlyFact]
        public async Task VerifyOnInvalidSignatureFileEntry_SignatureCentralDirectoryHeaderHasInvalidExternalFileAttributesAsync()
        {
            var nupkg = new SimpleTestPackageContext();

            using (var dir = TestDirectory.Create())
            using (var testCertificate = new X509Certificate2(_trustedTestCert.Source.Cert))
            {
                var signedPackagePath = await SignedArchiveTestUtility.AuthorSignPackageAsync(testCertificate, nupkg, dir);

                using (var packageStream = new FileStream(signedPackagePath, FileMode.Open))
                {
                    using (var reader = new BinaryReader(packageStream, _readerEncoding, leaveOpen: true))
                    using (var writer = new BinaryWriter(packageStream, _writerEncoding, leaveOpen: true))
                    {
                        var metadata = SignedPackageArchiveIOUtility.ReadSignedArchiveMetadata(reader);
                        var signatureCentralDirectoryHeader = metadata.CentralDirectoryHeaders[metadata.SignatureCentralDirectoryHeaderIndex];

                        writer.BaseStream.Seek(offset: signatureCentralDirectoryHeader.Position + 38L, origin: SeekOrigin.Begin);

                        // change external file attributes
                        writer.Write(0x20U);
                    }

                    AssertSignatureEntryMetadataThrowsException(packageStream);
                }
            }
        }

        [CIOnlyFact]
        public async Task VerifyOnInvalidSignatureFileEntry_SignatureCentralDirectoryHeaderHasInvalidGeneralPurposeFlagBitsAsync()
        {
            // Arrange
            var nupkg = new SimpleTestPackageContext();

            using (var dir = TestDirectory.Create())
            using (var testCertificate = new X509Certificate2(_trustedTestCert.Source.Cert))
            {
                var signedPackagePath = await SignedArchiveTestUtility.AuthorSignPackageAsync(testCertificate, nupkg, dir);

                using (var packageWriteStream = new FileStream(signedPackagePath, FileMode.Open))
                {
                    using (var reader = new BinaryReader(packageWriteStream, _readerEncoding, leaveOpen: true))
                    using (var writer = new BinaryWriter(packageWriteStream, _writerEncoding, leaveOpen: true))
                    {
                        var metadata = SignedPackageArchiveIOUtility.ReadSignedArchiveMetadata(reader);
                        var signatureCentralDirectoryHeader = metadata.CentralDirectoryHeaders[metadata.SignatureCentralDirectoryHeaderIndex];

                        // skip header signature (4 bytes) and 2 version fields (2 bytes each)
                        writer.BaseStream.Seek(offset: signatureCentralDirectoryHeader.Position + 8L, origin: SeekOrigin.Begin);

                        // change general purpose bit flag
                        writer.Write((ushort)1);
                    }

                    var verifier = new PackageSignatureVerifier(_trustProviders);
                    using (var packageReader = new PackageArchiveReader(packageWriteStream))
                    {
                        // Act
                        var result = await verifier.VerifySignaturesAsync(packageReader, _settings, CancellationToken.None);
                        var resultsWithErrors = result.Results.Where(r => r.GetErrorIssues().Any());
                        var totalErrorIssues = resultsWithErrors.SelectMany(r => r.GetErrorIssues());

                        // Assert
                        result.IsValid.Should().BeFalse();
                        resultsWithErrors.Count().Should().Be(1);
                        totalErrorIssues.Count().Should().Be(1);
                        totalErrorIssues.First().Code.Should().Be(NuGetLogCode.NU3005);
                    }
                }
            }
        }

        [CIOnlyFact]
        public async Task VerifyOnInvalidSignatureFileEntry_SignatureCentralDirectoryHeaderHasInvalidGeneralPurposeFlagBitsAssertExceptionAsync()
        {
            // Arrange
            var nupkg = new SimpleTestPackageContext();

            using (var dir = TestDirectory.Create())
            using (var testCertificate = new X509Certificate2(_trustedTestCert.Source.Cert))
            {
                var signedPackagePath = await SignedArchiveTestUtility.AuthorSignPackageAsync(testCertificate, nupkg, dir);

                using (var packageWriteStream = new FileStream(signedPackagePath, FileMode.Open))
                {
                    using (var reader = new BinaryReader(packageWriteStream, _readerEncoding, leaveOpen: true))
                    using (var writer = new BinaryWriter(packageWriteStream, _writerEncoding, leaveOpen: true))
                    {
                        var metadata = SignedPackageArchiveIOUtility.ReadSignedArchiveMetadata(reader);
                        var signatureCentralDirectoryHeader = metadata.CentralDirectoryHeaders[metadata.SignatureCentralDirectoryHeaderIndex];

                        // skip header signature (4 bytes) and 2 version fields (2 bytes each)
                        writer.BaseStream.Seek(offset: signatureCentralDirectoryHeader.Position + 8L, origin: SeekOrigin.Begin);

                        // change general purpose bit flag
                        writer.Write((ushort)1);
                    }

                    // Assert
                    AssertSignatureEntryMetadataThrowsException(packageWriteStream);
                }
            }
        }

        [CIOnlyFact]
        public async Task VerifyOnInvalidSignatureFileEntry_SignatureCentralDirectoryHeaderHasInvalidCompressionMethodAsync()
        {
            // Arrange
            var nupkg = new SimpleTestPackageContext();

            using (var dir = TestDirectory.Create())
            using (var testCertificate = new X509Certificate2(_trustedTestCert.Source.Cert))
            {
                var signedPackagePath = await SignedArchiveTestUtility.AuthorSignPackageAsync(testCertificate, nupkg, dir);

                using (var packageWriteStream = new FileStream(signedPackagePath, FileMode.Open))
                {
                    using (var reader = new BinaryReader(packageWriteStream, _readerEncoding, leaveOpen: true))
                    using (var writer = new BinaryWriter(packageWriteStream, _writerEncoding, leaveOpen: true))
                    {
                        var metadata = SignedPackageArchiveIOUtility.ReadSignedArchiveMetadata(reader);
                        var signatureCentralDirectoryHeader = metadata.CentralDirectoryHeaders[metadata.SignatureCentralDirectoryHeaderIndex];

                        // skip header signature (4 bytes), 2 version fields (2 bytes each) and general purpose bit field (2 bytes)
                        writer.BaseStream.Seek(offset: signatureCentralDirectoryHeader.Position + 10L, origin: SeekOrigin.Begin);

                        // change compression method
                        writer.Write((ushort)8);

                        // Assert
                        AssertSignatureEntryMetadataThrowsException(packageWriteStream);
                    }
                }
            }
        }

        [CIOnlyFact]
        public async Task VerifyOnInvalidSignatureFileEntry_SignatureCentralDirectoryHeaderHasInvalidCompressedSizeAsync()
        {
            // Arrange
            var nupkg = new SimpleTestPackageContext();

            using (var dir = TestDirectory.Create())
            using (var testCertificate = new X509Certificate2(_trustedTestCert.Source.Cert))
            {
                var signedPackagePath = await SignedArchiveTestUtility.AuthorSignPackageAsync(testCertificate, nupkg, dir);

                using (var packageWriteStream = new FileStream(signedPackagePath, FileMode.Open))
                {
                    using (var reader = new BinaryReader(packageWriteStream, _readerEncoding, leaveOpen: true))
                    using (var writer = new BinaryWriter(packageWriteStream, _writerEncoding, leaveOpen: true))
                    {
                        var metadata = SignedPackageArchiveIOUtility.ReadSignedArchiveMetadata(reader);
                        var signatureCentralDirectoryHeader = metadata.CentralDirectoryHeaders[metadata.SignatureCentralDirectoryHeaderIndex];

                        // skip till uncompressed size (20 bytes total)
                        writer.BaseStream.Seek(offset: signatureCentralDirectoryHeader.Position + 24L, origin: SeekOrigin.Begin);

                        // change uncompressed size
                        writer.Write((uint)1);
                    }

                    var verifier = new PackageSignatureVerifier(_trustProviders);
                    using (var packageReader = new PackageArchiveReader(packageWriteStream))
                    {
                        // Act
                        var result = await verifier.VerifySignaturesAsync(packageReader, _settings, CancellationToken.None);
                        var resultsWithErrors = result.Results.Where(r => r.GetErrorIssues().Any());
                        var totalErrorIssues = resultsWithErrors.SelectMany(r => r.GetErrorIssues());

                        // Assert
                        result.IsValid.Should().BeFalse();
                        resultsWithErrors.Count().Should().Be(1);
                        totalErrorIssues.Count().Should().Be(1);
                        totalErrorIssues.First().Code.Should().Be(NuGetLogCode.NU3005);
                    }
                }
            }
        }

        [CIOnlyFact]
        public async Task VerifyOnInvalidSignatureFileEntry_SignatureCentralDirectoryHeaderHasInvalidCompressedSizeAssertExceptionAsync()
        {
            // Arrange
            var nupkg = new SimpleTestPackageContext();

            using (var dir = TestDirectory.Create())
            using (var testCertificate = new X509Certificate2(_trustedTestCert.Source.Cert))
            {
                var signedPackagePath = await SignedArchiveTestUtility.AuthorSignPackageAsync(testCertificate, nupkg, dir);

                using (var packageWriteStream = new FileStream(signedPackagePath, FileMode.Open))
                {
                    using (var reader = new BinaryReader(packageWriteStream, _readerEncoding, leaveOpen: true))
                    using (var writer = new BinaryWriter(packageWriteStream, _writerEncoding, leaveOpen: true))
                    {
                        var metadata = SignedPackageArchiveIOUtility.ReadSignedArchiveMetadata(reader);
                        var signatureCentralDirectoryHeader = metadata.CentralDirectoryHeaders[metadata.SignatureCentralDirectoryHeaderIndex];

                        // skip till compressed size (20 bytes total)
                        writer.BaseStream.Seek(offset: signatureCentralDirectoryHeader.Position + 20L, origin: SeekOrigin.Begin);

                        // change compressed size
                        writer.Write((uint)1);
                    }

                    // Assert
                    AssertSignatureEntryMetadataThrowsException(packageWriteStream);
                }
            }
        }

        [CIOnlyFact]
        public async Task VerifyOnInvalidSignatureFileEntry_SignatureCentralDirectoryHeaderHasInvalidUncompressedSizeAsync()
        {
            // Arrange
            var nupkg = new SimpleTestPackageContext();

            using (var dir = TestDirectory.Create())
            using (var testCertificate = new X509Certificate2(_trustedTestCert.Source.Cert))
            {
                var signedPackagePath = await SignedArchiveTestUtility.AuthorSignPackageAsync(testCertificate, nupkg, dir);

                using (var packageWriteStream = new FileStream(signedPackagePath, FileMode.Open))
                {
                    using (var reader = new BinaryReader(packageWriteStream, _readerEncoding, leaveOpen: true))
                    using (var writer = new BinaryWriter(packageWriteStream, _writerEncoding, leaveOpen: true))
                    {
                        var metadata = SignedPackageArchiveIOUtility.ReadSignedArchiveMetadata(reader);
                        var signatureCentralDirectoryHeader = metadata.CentralDirectoryHeaders[metadata.SignatureCentralDirectoryHeaderIndex];

                        // skip till uncompressed size (24 bytes total)
                        writer.BaseStream.Seek(offset: signatureCentralDirectoryHeader.Position + 24L, origin: SeekOrigin.Begin);

                        // change uncompressed size
                        writer.Write((uint)1);
                    }

                    var verifier = new PackageSignatureVerifier(_trustProviders);
                    using (var packageReader = new PackageArchiveReader(packageWriteStream))
                    {
                        // Act
                        var result = await verifier.VerifySignaturesAsync(packageReader, _settings, CancellationToken.None);
                        var resultsWithErrors = result.Results.Where(r => r.GetErrorIssues().Any());
                        var totalErrorIssues = resultsWithErrors.SelectMany(r => r.GetErrorIssues());

                        // Assert
                        result.IsValid.Should().BeFalse();
                        resultsWithErrors.Count().Should().Be(1);
                        totalErrorIssues.Count().Should().Be(1);
                        totalErrorIssues.First().Code.Should().Be(NuGetLogCode.NU3005);
                    }
                }
            }
        }

        [CIOnlyFact]
        public async Task VerifyOnInvalidSignatureFileEntry_SignatureCentralDirectoryHeaderHasInvalidUncompressedSizeAssertExceptionAsync()
        {
            // Arrange
            var nupkg = new SimpleTestPackageContext();

            using (var dir = TestDirectory.Create())
            using (var testCertificate = new X509Certificate2(_trustedTestCert.Source.Cert))
            {
                var signedPackagePath = await SignedArchiveTestUtility.AuthorSignPackageAsync(testCertificate, nupkg, dir);

                using (var packageWriteStream = new FileStream(signedPackagePath, FileMode.Open))
                {
                    using (var reader = new BinaryReader(packageWriteStream, _readerEncoding, leaveOpen: true))
                    using (var writer = new BinaryWriter(packageWriteStream, _writerEncoding, leaveOpen: true))
                    {
                        var metadata = SignedPackageArchiveIOUtility.ReadSignedArchiveMetadata(reader);
                        var signatureCentralDirectoryHeader = metadata.CentralDirectoryHeaders[metadata.SignatureCentralDirectoryHeaderIndex];

                        // skip till uncompressed size (24 bytes total)
                        writer.BaseStream.Seek(offset: signatureCentralDirectoryHeader.Position + 24L, origin: SeekOrigin.Begin);

                        // change uncompressed size
                        writer.Write((uint)1);
                    }

                    // Assert
                    AssertSignatureEntryMetadataThrowsException(packageWriteStream);
                }
            }
        }

        [CIOnlyFact]
        public async Task VerifyOnInvalidSignatureFileEntry_SignatureLocalFileHeaderHasInvalidGeneralPurposeFlagBitsAsync()
        {
            // Arrange
            var nupkg = new SimpleTestPackageContext();

            using (var dir = TestDirectory.Create())
            using (var testCertificate = new X509Certificate2(_trustedTestCert.Source.Cert))
            {
                var signedPackagePath = await SignedArchiveTestUtility.AuthorSignPackageAsync(testCertificate, nupkg, dir);

                using (var packageWriteStream = new FileStream(signedPackagePath, FileMode.Open))
                {
                    using (var reader = new BinaryReader(packageWriteStream, _readerEncoding, leaveOpen: true))
                    using (var writer = new BinaryWriter(packageWriteStream, _writerEncoding, leaveOpen: true))
                    {
                        var metadata = SignedPackageArchiveIOUtility.ReadSignedArchiveMetadata(reader);
                        var signatureCentralDirectoryHeader = metadata.CentralDirectoryHeaders[metadata.SignatureCentralDirectoryHeaderIndex];

                        // skip header signature (4 bytes) and version field (2 bytes)
                        writer.BaseStream.Seek(offset: signatureCentralDirectoryHeader.OffsetToLocalFileHeader + 6L, origin: SeekOrigin.Begin);

                        // change general purpose bit flag
                        writer.Write((ushort)1);
                    }

                    var verifier = new PackageSignatureVerifier(_trustProviders);
                    using (var packageReader = new PackageArchiveReader(packageWriteStream))
                    {
                        // Act
                        var result = await verifier.VerifySignaturesAsync(packageReader, _settings, CancellationToken.None);
                        var resultsWithErrors = result.Results.Where(r => r.GetErrorIssues().Any());
                        var totalErrorIssues = resultsWithErrors.SelectMany(r => r.GetErrorIssues());

                        // Assert
                        result.IsValid.Should().BeFalse();
                        resultsWithErrors.Count().Should().Be(1);
                        totalErrorIssues.Count().Should().Be(1);
                        totalErrorIssues.First().Code.Should().Be(NuGetLogCode.NU3005);
                    }
                }
            }
        }

        [CIOnlyFact]
        public async Task VerifyOnInvalidSignatureFileEntry_SignatureLocalFileHeaderHasInvalidGeneralPurposeFlagBitsAssertExceptionAsync()
        {
            // Arrange
            var nupkg = new SimpleTestPackageContext();

            using (var dir = TestDirectory.Create())
            using (var testCertificate = new X509Certificate2(_trustedTestCert.Source.Cert))
            {
                var signedPackagePath = await SignedArchiveTestUtility.AuthorSignPackageAsync(testCertificate, nupkg, dir);

                using (var packageWriteStream = new FileStream(signedPackagePath, FileMode.Open))
                {
                    using (var reader = new BinaryReader(packageWriteStream, _readerEncoding, leaveOpen: true))
                    using (var writer = new BinaryWriter(packageWriteStream, _writerEncoding, leaveOpen: true))
                    {
                        var metadata = SignedPackageArchiveIOUtility.ReadSignedArchiveMetadata(reader);
                        var signatureCentralDirectoryHeader = metadata.CentralDirectoryHeaders[metadata.SignatureCentralDirectoryHeaderIndex];

                        // skip header signature (4 bytes) and version field (2 bytes)
                        writer.BaseStream.Seek(offset: signatureCentralDirectoryHeader.OffsetToLocalFileHeader + 6L, origin: SeekOrigin.Begin);

                        // change general purpose bit flag
                        writer.Write((ushort)1);
                    }

                    // Assert
                    AssertSignatureEntryMetadataThrowsException(packageWriteStream);
                }
            }
        }

        [CIOnlyFact]
        public async Task VerifyOnInvalidSignatureFileEntry_SignatureLocalFileHeaderHasInvalidCompressionMethodAsync()
        {
            // Arrange
            var nupkg = new SimpleTestPackageContext();

            using (var dir = TestDirectory.Create())
            using (var testCertificate = new X509Certificate2(_trustedTestCert.Source.Cert))
            {
                var signedPackagePath = await SignedArchiveTestUtility.AuthorSignPackageAsync(testCertificate, nupkg, dir);

                using (var packageWriteStream = new FileStream(signedPackagePath, FileMode.Open))
                {
                    using (var reader = new BinaryReader(packageWriteStream, _readerEncoding, leaveOpen: true))
                    using (var writer = new BinaryWriter(packageWriteStream, _writerEncoding, leaveOpen: true))
                    {
                        var metadata = SignedPackageArchiveIOUtility.ReadSignedArchiveMetadata(reader);
                        var signatureCentralDirectoryHeader = metadata.CentralDirectoryHeaders[metadata.SignatureCentralDirectoryHeaderIndex];

                        // skip header signature (4 bytes), version fields(2 bytes) and general purpose bit field (2 bytes)
                        writer.BaseStream.Seek(offset: signatureCentralDirectoryHeader.OffsetToLocalFileHeader + 8L, origin: SeekOrigin.Begin);

                        // change compression method
                        writer.Write((ushort)1);
                    }

                    var verifier = new PackageSignatureVerifier(_trustProviders);
                    using (var packageReader = new PackageArchiveReader(packageWriteStream))
                    {
                        // Act
                        var result = await verifier.VerifySignaturesAsync(packageReader, _settings, CancellationToken.None);
                        var resultsWithErrors = result.Results.Where(r => r.GetErrorIssues().Any());
                        var totalErrorIssues = resultsWithErrors.SelectMany(r => r.GetErrorIssues());

                        // Assert
                        result.IsValid.Should().BeFalse();
                        resultsWithErrors.Count().Should().Be(1);
                        totalErrorIssues.Count().Should().Be(1);
                        totalErrorIssues.First().Code.Should().Be(NuGetLogCode.NU3005);
                    }
                }
            }
        }

        [CIOnlyFact]
        public async Task VerifyOnInvalidSignatureFileEntry_SignatureLocalFileHeaderHasInvalidCompressionMethodAssertExceptionAsync()
        {
            // Arrange
            var nupkg = new SimpleTestPackageContext();

            using (var dir = TestDirectory.Create())
            using (var testCertificate = new X509Certificate2(_trustedTestCert.Source.Cert))
            {
                var signedPackagePath = await SignedArchiveTestUtility.AuthorSignPackageAsync(testCertificate, nupkg, dir);

                using (var packageWriteStream = new FileStream(signedPackagePath, FileMode.Open))
                {
                    using (var reader = new BinaryReader(packageWriteStream, _readerEncoding, leaveOpen: true))
                    using (var writer = new BinaryWriter(packageWriteStream, _writerEncoding, leaveOpen: true))
                    {
                        var metadata = SignedPackageArchiveIOUtility.ReadSignedArchiveMetadata(reader);
                        var signatureCentralDirectoryHeader = metadata.CentralDirectoryHeaders[metadata.SignatureCentralDirectoryHeaderIndex];

                        // skip header signature (4 bytes), version fields(2 bytes) and general purpose bit field (2 bytes)
                        writer.BaseStream.Seek(offset: signatureCentralDirectoryHeader.OffsetToLocalFileHeader + 8L, origin: SeekOrigin.Begin);

                        // change compression method
                        writer.Write((ushort)1);
                    }

                    // Assert
                    AssertSignatureEntryMetadataThrowsException(packageWriteStream);
                }
            }
        }

        [CIOnlyFact]
        public async Task VerifyOnInvalidSignatureFileEntry_SignatureLocalFileHeaderHasInvalidCompressedSizeAsync()
        {
            // Arrange
            var nupkg = new SimpleTestPackageContext();

            using (var dir = TestDirectory.Create())
            using (var testCertificate = new X509Certificate2(_trustedTestCert.Source.Cert))
            {
                var signedPackagePath = await SignedArchiveTestUtility.AuthorSignPackageAsync(testCertificate, nupkg, dir);

                using (var packageWriteStream = new FileStream(signedPackagePath, FileMode.Open))
                {
                    using (var reader = new BinaryReader(packageWriteStream, _readerEncoding, leaveOpen: true))
                    using (var writer = new BinaryWriter(packageWriteStream, _writerEncoding, leaveOpen: true))
                    {
                        var metadata = SignedPackageArchiveIOUtility.ReadSignedArchiveMetadata(reader);
                        var signatureCentralDirectoryHeader = metadata.CentralDirectoryHeaders[metadata.SignatureCentralDirectoryHeaderIndex];

                        // skip till compressed size (18 bytes total)
                        writer.BaseStream.Seek(offset: signatureCentralDirectoryHeader.OffsetToLocalFileHeader + 18L, origin: SeekOrigin.Begin);

                        // change compressed size
                        writer.Write((uint)1);
                    }

                    var verifier = new PackageSignatureVerifier(_trustProviders);
                    using (var packageReader = new PackageArchiveReader(packageWriteStream))
                    {
                        // Act
                        var result = await verifier.VerifySignaturesAsync(packageReader, _settings, CancellationToken.None);
                        var resultsWithErrors = result.Results.Where(r => r.GetErrorIssues().Any());
                        var totalErrorIssues = resultsWithErrors.SelectMany(r => r.GetErrorIssues());

                        // Assert
                        result.IsValid.Should().BeFalse();
                        resultsWithErrors.Count().Should().Be(1);
                        totalErrorIssues.Count().Should().Be(1);
                        totalErrorIssues.First().Code.Should().Be(NuGetLogCode.NU3005);
                    }
                }
            }
        }

        [CIOnlyFact]
        public async Task VerifyOnInvalidSignatureFileEntry_SignatureLocalFileHeaderHasInvalidCompressedSizeAssertExceptionAsync()
        {
            // Arrange
            var nupkg = new SimpleTestPackageContext();

            using (var dir = TestDirectory.Create())
            using (var testCertificate = new X509Certificate2(_trustedTestCert.Source.Cert))
            {
                var signedPackagePath = await SignedArchiveTestUtility.AuthorSignPackageAsync(testCertificate, nupkg, dir);

                using (var packageWriteStream = new FileStream(signedPackagePath, FileMode.Open))
                {
                    using (var reader = new BinaryReader(packageWriteStream, _readerEncoding, leaveOpen: true))
                    using (var writer = new BinaryWriter(packageWriteStream, _writerEncoding, leaveOpen: true))
                    {
                        var metadata = SignedPackageArchiveIOUtility.ReadSignedArchiveMetadata(reader);
                        var signatureCentralDirectoryHeader = metadata.CentralDirectoryHeaders[metadata.SignatureCentralDirectoryHeaderIndex];

                        // skip till compressed size (18 bytes total)
                        writer.BaseStream.Seek(offset: signatureCentralDirectoryHeader.OffsetToLocalFileHeader + 18L, origin: SeekOrigin.Begin);

                        // change compressed size
                        writer.Write((uint)1);
                    }

                    // Assert
                    AssertSignatureEntryMetadataThrowsException(packageWriteStream);
                }
            }
        }

        [CIOnlyFact]
        public async Task VerifyOnInvalidSignatureFileEntry_SignatureLocalFileHeaderHasInvalidUncompressedSizeAsync()
        {
            // Arrange
            var nupkg = new SimpleTestPackageContext();

            using (var dir = TestDirectory.Create())
            using (var testCertificate = new X509Certificate2(_trustedTestCert.Source.Cert))
            {
                var signedPackagePath = await SignedArchiveTestUtility.AuthorSignPackageAsync(testCertificate, nupkg, dir);

                using (var packageWriteStream = new FileStream(signedPackagePath, FileMode.Open))
                {
                    using (var reader = new BinaryReader(packageWriteStream, _readerEncoding, leaveOpen: true))
                    using (var writer = new BinaryWriter(packageWriteStream, _writerEncoding, leaveOpen: true))
                    {
                        var metadata = SignedPackageArchiveIOUtility.ReadSignedArchiveMetadata(reader);
                        var signatureCentralDirectoryHeader = metadata.CentralDirectoryHeaders[metadata.SignatureCentralDirectoryHeaderIndex];

                        // skip till uncompressed size (22 bytes total)
                        writer.BaseStream.Seek(offset: signatureCentralDirectoryHeader.OffsetToLocalFileHeader + 22L, origin: SeekOrigin.Begin);

                        // change uncompressed size
                        writer.Write((uint)1);
                    }
                }

                using (var packageStream = new FileStream(signedPackagePath, FileMode.Open))
                {
                    var verifier = new PackageSignatureVerifier(_trustProviders);
                    using (var packageReader = new PackageArchiveReader(packageStream))
                    {
                        // Act
                        var result = await verifier.VerifySignaturesAsync(packageReader, _settings, CancellationToken.None);
                        var resultsWithErrors = result.Results.Where(r => r.GetErrorIssues().Any());
                        var totalErrorIssues = resultsWithErrors.SelectMany(r => r.GetErrorIssues());

                        // Assert
                        result.IsValid.Should().BeFalse();
                        resultsWithErrors.Count().Should().Be(1);
                        totalErrorIssues.Count().Should().Be(1);
                        totalErrorIssues.First().Code.Should().Be(NuGetLogCode.NU3005);
                    }
                }
            }
        }

        [CIOnlyFact]
        public async Task VerifyOnInvalidSignatureFileEntry_SignatureLocalFileHeaderHasInvalidUncompressedSizeAssertExceptionAsync()
        {
            // Arrange
            var nupkg = new SimpleTestPackageContext();

            using (var dir = TestDirectory.Create())
            using (var testCertificate = new X509Certificate2(_trustedTestCert.Source.Cert))
            {
                var signedPackagePath = await SignedArchiveTestUtility.AuthorSignPackageAsync(testCertificate, nupkg, dir);
                using (var packageWriteStream = new FileStream(signedPackagePath, FileMode.Open))
                {
                    using (var reader = new BinaryReader(packageWriteStream, _readerEncoding, leaveOpen: true))
                    using (var writer = new BinaryWriter(packageWriteStream, _writerEncoding, leaveOpen: true))
                    {
                        var metadata = SignedPackageArchiveIOUtility.ReadSignedArchiveMetadata(reader);
                        var signatureCentralDirectoryHeader = metadata.CentralDirectoryHeaders[metadata.SignatureCentralDirectoryHeaderIndex];

                        // skip till uncompressed size (22 bytes total)
                        writer.BaseStream.Seek(offset: signatureCentralDirectoryHeader.OffsetToLocalFileHeader + 22L, origin: SeekOrigin.Begin);

                        // change uncompressed size
                        writer.Write((uint)1);
                    }

                    // Assert
                    AssertSignatureEntryMetadataThrowsException(packageWriteStream);
                }
            }
        }

        [CIOnlyFact]
        public async Task VerifyPackageContentHash_SignedPackagesAsync()
        {
            // Arrange
            var nupkg = new SimpleTestPackageContext();

            using (MemoryStream packageStream = await nupkg.CreateAsStreamAsync())
            {
                string expectedHash = Convert.ToBase64String(new CryptoHashProvider("SHA512").CalculateHash(packageStream));

                using (var dir = TestDirectory.Create())
                using (var testCertificate = new X509Certificate2(_trustedTestCert.Source.Cert))
                {
                    var signedPackagePath = await SignedArchiveTestUtility.AuthorSignPackageAsync(testCertificate, packageStream, nupkg.Id, nupkg.Version, dir);

                    var verifier = new PackageSignatureVerifier(_trustProviders);
                    using (var stream = File.OpenRead(signedPackagePath))
                    using (var packageReader = new PackageArchiveReader(stream, leaveStreamOpen: true))
                    {
                        // Act
                        var contentHash = packageReader.GetContentHash(CancellationToken.None);

                        stream.Seek(offset: 0, origin: SeekOrigin.Begin);
                        var wholePackageHash = Convert.ToBase64String(new CryptoHashProvider("SHA512").CalculateHash(stream));

                        // Assert
                        contentHash.Should().NotBeNullOrEmpty();
                        Assert.Equal(expectedHash, contentHash);
                        Assert.NotEqual(wholePackageHash, contentHash);
                    }
                }
            }
        }

        [CIOnlyFact]
        public async Task ReadSignedArchiveMetadata_InvalidSignatureFileEntry_IgnoreVerifySignatureEntry()
        {
            // Arrange
            var nupkg = new SimpleTestPackageContext();

            using (var packageStream = await nupkg.CreateAsStreamAsync())
            using (var testCertificate = new X509Certificate2(_trustedTestCert.Source.Cert))
            {
                AuthorPrimarySignature signature = await SignedArchiveTestUtility.CreateAuthorSignatureForPackageAsync(testCertificate, packageStream);

                using (var package = new ZipArchive(packageStream, ZipArchiveMode.Update, leaveOpen: true))
                {
                    var signatureEntry = package.CreateEntry(_specification.SignaturePath, CompressionLevel.Optimal);
                    using (var signatureStream = new MemoryStream(signature.GetBytes()))
                    using (var signatureEntryStream = signatureEntry.Open())
                    {
                        signatureStream.CopyTo(signatureEntryStream);
                    }
                }

                using (var reader = new BinaryReader(packageStream, _readerEncoding, leaveOpen: true))
                {
                    var metadata = SignedPackageArchiveIOUtility.ReadSignedArchiveMetadata(reader, validateSignatureEntry: false);

                    Assert.NotNull(metadata);
                }
            }
        }

        private void AssertSignatureEntryMetadataThrowsException(Stream packageStream)
        {
            using (var reader = new BinaryReader(packageStream, _readerEncoding, leaveOpen: true))
            {
                var exception = Assert.Throws<SignatureException>(
                    () => SignedPackageArchiveIOUtility.ReadSignedArchiveMetadata(reader));

                exception.AsLogMessage().Code.Should().Be(NuGetLogCode.NU3005);
            }
        }
    }
}

#endif
