// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Packaging.Signing;
using Test.Utility.Signing;
using Xunit;

namespace NuGet.Packaging.Test
{
    [Collection(SigningTestsCollection.Name)]
    public class SignedPackageArchiveUtilityTests
    {
        private static readonly byte[] _signatureFileName = Encoding.ASCII.GetBytes(SigningSpecifications.V1.SignaturePath);
        private readonly CertificatesFixture _fixture;

        public SignedPackageArchiveUtilityTests(CertificatesFixture fixture)
        {
            _fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
        }

        [Fact]
        public void OpenPackageSignatureFileStream_WhenReaderIsNull_Throws()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => SignedPackageArchiveUtility.OpenPackageSignatureFileStream(reader: null));

            Assert.Equal("reader", exception.ParamName);
        }

        [Fact]
        public void OpenPackageSignatureFileStream_WithEmptyZip_Throws()
        {
            using (var test = new Test(GetEmptyZip()))
            {
                Assert.Throws<InvalidDataException>(
                    () => SignedPackageArchiveUtility.OpenPackageSignatureFileStream(test.Reader));
            }
        }

        [Fact]
        public void OpenPackageSignatureFileStream_WithIncorrectSignatureFileName_Throws()
        {
            using (var test = new Test(SigningTestUtility.GetResourceBytes("SignatureFileWithUppercaseFileName.zip")))
            {
                var exception = Assert.Throws<SignatureException>(
                    () => SignedPackageArchiveUtility.OpenPackageSignatureFileStream(test.Reader));

                Assert.Equal("The package does not contain a valid package signature file.", exception.Message);
                Assert.Equal(NuGetLogCode.NU3005, exception.Code);
            }
        }

        [Fact]
        public void OpenPackageSignatureFileStream_WithCompressedSignatureFileEntry_Throws()
        {
            using (var test = new Test(SigningTestUtility.GetResourceBytes("SignatureFileWithDeflateCompressionMethodAndDefaultCompressionLevel.zip")))
            {
                var exception = Assert.Throws<SignatureException>(
                    () => SignedPackageArchiveUtility.OpenPackageSignatureFileStream(test.Reader));

                Assert.Equal("The package signature file entry is invalid. The central directory header field 'compression method' has an invalid value (8).", exception.Message);
                Assert.Equal(NuGetLogCode.NU3005, exception.Code);
            }
        }

        [Fact]
        public void OpenPackageSignatureFileStream_WithFakeContent_ReturnsContent()
        {
            using (var test = new Test(SigningTestUtility.GetResourceBytes("SignatureFileWithFakeContent.zip")))
            using (var stream = SignedPackageArchiveUtility.OpenPackageSignatureFileStream(test.Reader))
            {
                Assert.False(stream.CanWrite);

                using (var reader = new BinaryReader(stream))
                {
                    var expectedBytes = Encoding.ASCII.GetBytes("content");
                    var actualBytes = reader.ReadBytes((int)reader.BaseStream.Length);

                    Assert.Equal(expectedBytes, actualBytes);
                }
            }
        }

        [Fact]
        public void IsSigned_WhenReaderIsNull_Throws()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => SignedPackageArchiveUtility.IsSigned(reader: null));

            Assert.Equal("reader", exception.ParamName);
        }

        [Fact]
        public void IsSigned_WithEmptyZip_ReturnsFalse()
        {
            using (var test = new Test(GetEmptyZip()))
            {
                var isSigned = SignedPackageArchiveUtility.IsSigned(test.Reader);

                Assert.False(isSigned);
            }
        }

        [Fact]
        public void IsSigned_WithCorrectSignatureFileEntry_ReturnsTrue()
        {
            using (var test = new Test(SigningTestUtility.GetResourceBytes("SignatureFileEntry.zip")))
            {
                var isSigned = SignedPackageArchiveUtility.IsSigned(test.Reader);

                Assert.True(isSigned);
            }
        }

        [Fact]
        public void IsSigned_WithLocalFileHeaderUsingUtf8_ReturnsFalse()
        {
            var zipBytes = SigningTestUtility.GetResourceBytes("SignatureFileEntry.zip");

            zipBytes[7] = 0x08;

            using (var test = new Test(zipBytes))
            {
                var isSigned = SignedPackageArchiveUtility.IsSigned(test.Reader);

                Assert.False(isSigned);
            }
        }

        [Fact]
        public void IsSigned_WithCentralDirectoryHeaderUsingUtf8_ReturnsFalse()
        {
            var zipBytes = SigningTestUtility.GetResourceBytes("SignatureFileEntry.zip");

            zipBytes[35] = 0x08;

            using (var test = new Test(zipBytes))
            {
                var isSigned = SignedPackageArchiveUtility.IsSigned(test.Reader);

                Assert.False(isSigned);
            }
        }

        [Fact]
        public void IsSigned_WithIncorrectSignatureFileNameInLocalFileHeader_ReturnsFalse()
        {
            var zipBytes = SigningTestUtility.GetResourceBytes("SignatureFileEntry.zip");
            var fileName = Encoding.ASCII.GetBytes(SigningSpecifications.V1.SignaturePath.ToUpper());

            Array.Copy(fileName, sourceIndex: 0, destinationArray: zipBytes, destinationIndex: 0x1e, length: fileName.Length);

            using (var test = new Test(zipBytes))
            {
                var isSigned = SignedPackageArchiveUtility.IsSigned(test.Reader);

                Assert.False(isSigned);
            }
        }

        [Fact]
        public void IsSigned_WithIncorrectSignatureFileNameInCentralDirectoryHeader_ReturnsFalse()
        {
            var zipBytes = SigningTestUtility.GetResourceBytes("SignatureFileEntry.zip");
            var fileName = Encoding.ASCII.GetBytes(SigningSpecifications.V1.SignaturePath.ToUpper());

            Array.Copy(fileName, sourceIndex: 0, destinationArray: zipBytes, destinationIndex: 0x5a, length: fileName.Length);

            using (var test = new Test(zipBytes))
            {
                var isSigned = SignedPackageArchiveUtility.IsSigned(test.Reader);

                Assert.False(isSigned);
            }
        }

        [Fact]
        public void IsZip64_WhenReaderIsNull_Throws()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => SignedPackageArchiveUtility.IsZip64(reader: null));

            Assert.Equal("reader", exception.ParamName);
        }

        [Fact]
        public void IsZip64_WithEmptyZip_ReturnsFalse()
        {
            using (var test = new Test(GetEmptyZip()))
            {
                var isZip64 = SignedPackageArchiveUtility.IsZip64(test.Reader);

                Assert.False(isZip64);
            }
        }

        [Fact]
        public void IsZip64_WithNonEmptyZip_ReturnsFalse()
        {
            using (var test = new Test(GetNonEmptyZip()))
            {
                var isZip64 = SignedPackageArchiveUtility.IsZip64(test.Reader);

                Assert.False(isZip64);
            }
        }

        [Fact]
        public void IsZip64_WithEmptyZip64_ReturnsTrue()
        {
            using (var test = new Test(SigningTestUtility.GetResourceBytes("EmptyZip64.zip")))
            {
                var isZip64 = SignedPackageArchiveUtility.IsZip64(test.Reader);

                Assert.True(isZip64);
            }
        }

        [Fact]
        public void IsZip64_WithLocalFileHeaderWithZip64ExtraField_ReturnsTrue()
        {
            using (var test = new Test(SigningTestUtility.GetResourceBytes("LocalFileHeaderWithZip64ExtraField.zip")))
            {
                var isZip64 = SignedPackageArchiveUtility.IsZip64(test.Reader);

                Assert.True(isZip64);
            }
        }

        [Fact]
        public void IsZip64_WithCentralDirectoryHeaderWithZip64ExtraField_ReturnsTrue()
        {
            using (var test = new Test(SigningTestUtility.GetResourceBytes("CentralDirectoryHeaderWithZip64ExtraField.zip")))
            {
                var isZip64 = SignedPackageArchiveUtility.IsZip64(test.Reader);

                Assert.True(isZip64);
            }
        }

        [Fact]
        public void GetPackageContentHash_WithCompressedSignatureFileEntry_NotThrows()
        {
            using (var test = new Test(SigningTestUtility.GetResourceBytes("SignatureFileWithDeflateCompressionMethodAndDefaultCompressionLevel.zip")))
            {
                var contentHash = SignedPackageArchiveUtility.GetPackageContentHash(test.Reader);

                Assert.NotNull(contentHash);
            }
        }

#if IS_SIGNING_SUPPORTED
        [Fact]
        public async Task RemoveRepositorySignaturesAsync_WithNullInput_Throws()
        {
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                () => SignedPackageArchiveUtility.RemoveRepositorySignaturesAsync(
                    input: null,
                    output: Stream.Null,
                    cancellationToken: new CancellationToken()));

            Assert.Equal("input", exception.ParamName);
        }

        [Fact]
        public async Task RemoveRepositorySignaturesAsync_WithNullOutput_Throws()
        {
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                () => SignedPackageArchiveUtility.RemoveRepositorySignaturesAsync(
                    Stream.Null,
                    output: null,
                    cancellationToken: new CancellationToken()));

            Assert.Equal("output", exception.ParamName);
        }

        [Fact]
        public async Task RemoveRepositorySignaturesAsync_WithCancelledToken_Throws()
        {
            await Assert.ThrowsAsync<OperationCanceledException>(
                () => SignedPackageArchiveUtility.RemoveRepositorySignaturesAsync(
                    Stream.Null,
                    Stream.Null,
                    new CancellationToken(canceled: true)));
        }

        [Fact]
        public async Task RemoveRepositorySignaturesAsync_WithUnsignedPackage_DoesNotChangePackage()
        {
            using (var test = new RemoveTest(_fixture))
            using (var input = new MemoryStream(test.Zip.ToByteArray(), writable: false))
            {
                var wasSomethingRemoved = await SignedPackageArchiveUtility.RemoveRepositorySignaturesAsync(
                    input,
                    test.UnsignedPackage,
                    CancellationToken.None);

                Assert.False(wasSomethingRemoved);
                Assert.Equal(0, test.UnsignedPackage.Length);
            }
        }

        [Fact(Skip="https://github.com/NuGet/Home/issues/12687")]
        public async Task RemoveRepositorySignaturesAsync_WithAuthorPrimarySignature_DoesNotChangePackage()
        {
            using (var test = new RemoveTest(_fixture))
            {
                await test.AuthorSignAsync();

                var wasSomethingRemoved = await SignedPackageArchiveUtility.RemoveRepositorySignaturesAsync(
                    test.SignedPackage,
                    test.UnsignedPackage,
                    CancellationToken.None);

                Assert.False(wasSomethingRemoved);
                Assert.Equal(0, test.UnsignedPackage.Length);
            }
        }

        [Fact]
        public async Task RemoveRepositorySignaturesAsync_WithRepositoryPrimarySignature_ReturnsUnsignedPackage()
        {
            using (var test = new RemoveTest(_fixture))
            {
                await test.RepositorySignAsync();

                var wasSomethingRemoved = await SignedPackageArchiveUtility.RemoveRepositorySignaturesAsync(
                    test.SignedPackage,
                    test.UnsignedPackage,
                    CancellationToken.None);

                Assert.True(wasSomethingRemoved);
                Assert.Equal(test.Zip.ToByteArray(), test.UnsignedPackage.ToArray());
            }
        }

        [Fact]
        public async Task RemoveRepositorySignaturesAsync_WithRepositoryCountersignature_ReturnsPrimarySignedPackage()
        {
            using (var test = new RemoveTest(_fixture))
            {
                await test.AuthorSignAsync();

                var expectedPackage = test.SignedPackage.ToArray();
                var originalLastWriteTime = GetLastModifiedDateTimeOfPackageSignatureFile(test.SignedPackage);

                await test.RepositoryCountersignAsync();

                var wasSomethingRemoved = await SignedPackageArchiveUtility.RemoveRepositorySignaturesAsync(
                    test.SignedPackage,
                    test.UnsignedPackage,
                    CancellationToken.None);

                var actualPackage = test.UnsignedPackage.ToArray();
                var newLastWriteTime = GetLastModifiedDateTimeOfPackageSignatureFile(test.UnsignedPackage);

                Assert.True(wasSomethingRemoved);
                Assert.InRange(newLastWriteTime, originalLastWriteTime, originalLastWriteTime.Add(TimeSpan.FromMinutes(5)));

                ZeroPackageSignatureFileLastModifiedDateTimes(expectedPackage, actualPackage);

                Assert.Equal(expectedPackage, actualPackage);
            }
        }

        [Fact]
        public async Task RemoveRepositorySignaturesAsync_WithMultipleCountersignatures_ReturnsSignedPackageWithoutRepositorySignatures()
        {
            using (var test = new RemoveTest(_fixture))
            {
                await test.AuthorSignAsync();
                await test.CountersignAsync();

                var expectedPackage = test.SignedPackage.ToArray();
                var originalLastWriteTime = GetLastModifiedDateTimeOfPackageSignatureFile(test.SignedPackage);

                await test.RepositoryCountersignAsync();
                await test.RepositoryCountersignAsync();

                var wasSomethingRemoved = await SignedPackageArchiveUtility.RemoveRepositorySignaturesAsync(
                    test.SignedPackage,
                    test.UnsignedPackage,
                    CancellationToken.None);

                var actualPackage = test.UnsignedPackage.ToArray();
                var newLastWriteTime = GetLastModifiedDateTimeOfPackageSignatureFile(test.UnsignedPackage);

                Assert.True(wasSomethingRemoved);
                Assert.InRange(newLastWriteTime, originalLastWriteTime, originalLastWriteTime.Add(TimeSpan.FromMinutes(5)));

                ZeroPackageSignatureFileLastModifiedDateTimes(expectedPackage, actualPackage);

                Assert.Equal(expectedPackage, actualPackage);
            }
        }

        private static void ZeroPackageSignatureFileLastModifiedDateTimes(byte[] package1, byte[] package2)
        {
            // The two packages should be identical except for last modified datetime values
            // in the package signature file's local file header and central directory header.
            Assert.Equal(package1.Length, package2.Length);

            ZeroPackageSignatureFileLastModifiedDateTime(package1);
            ZeroPackageSignatureFileLastModifiedDateTime(package2);
        }

        private static void ZeroPackageSignatureFileLastModifiedDateTime(byte[] package)
        {
            GetPackageSignatureFileHeaderOffsets(
                package,
                out var centralDirectoryHeaderOffset,
                out var localFileHeaderOffset);

            var offsetOfCentralDirectoryHeaderLastModifiedDateTime = centralDirectoryHeaderOffset
                + sizeof(uint)        // Signature field
                + 4 * sizeof(ushort); // VersionMadeBy, VersionNeededToExtract, GeneralPurposeBitFlag, and CompressionMethod fields

            var offsetOfLocalFileHeaderLastModifiedDateTime = localFileHeaderOffset
                + sizeof(uint)        // Signature field
                + 3 * sizeof(ushort); // VersionNeededToExtract, GeneralPurposeBitFlag, and CompressionMethod fields

            for (var i = 0; i < 4; ++i)
            {
                package.SetValue((byte)0, offsetOfLocalFileHeaderLastModifiedDateTime + i);
                package.SetValue((byte)0, offsetOfCentralDirectoryHeaderLastModifiedDateTime + i);
            }
        }
#endif

        private static byte[] GetEmptyZip()
        {
            using (var stream = new MemoryStream())
            {
                using (var zip = new ZipArchive(stream, ZipArchiveMode.Create))
                {
                }

                return stream.ToArray();
            }
        }

        private static byte[] GetNonEmptyZip()
        {
            using (var stream = new MemoryStream())
            {
                using (var zip = new ZipArchive(stream, ZipArchiveMode.Create))
                {
                    var entry = zip.CreateEntry("file.txt");

                    using (var entryStream = entry.Open())
                    using (var streamWriter = new StreamWriter(entryStream))
                    {
                        streamWriter.Write("peach");
                    }
                }

                return stream.ToArray();
            }
        }

        private static DateTimeOffset GetLastModifiedDateTimeOfPackageSignatureFile(MemoryStream package)
        {
            using (var zipArchive = new ZipArchive(package, ZipArchiveMode.Read, leaveOpen: true))
            {
                var packageSignatureFile = zipArchive.GetEntry(SigningSpecifications.V1.SignaturePath);

                return packageSignatureFile.LastWriteTime;
            }
        }

        private static void GetPackageSignatureFileHeaderOffsets(
            byte[] package,
            out long centralDirectoryHeaderOffset,
            out long localFileHeaderOffset)
        {
            using (var stream = new MemoryStream(package))
            using (var reader = new BinaryReader(stream))
            {
                var endOfCentralDirectoryRecord = Signing.EndOfCentralDirectoryRecord.Read(reader);

                reader.BaseStream.Seek(endOfCentralDirectoryRecord.OffsetOfStartOfCentralDirectory, SeekOrigin.Begin);

                Signing.CentralDirectoryHeader centralDirectoryHeader;

                while (Signing.CentralDirectoryHeader.TryRead(reader, out centralDirectoryHeader))
                {
                    if (_signatureFileName.SequenceEqual(centralDirectoryHeader.FileName))
                    {
                        centralDirectoryHeaderOffset = centralDirectoryHeader.OffsetFromStart;

                        reader.BaseStream.Seek(centralDirectoryHeader.RelativeOffsetOfLocalHeader, SeekOrigin.Begin);

                        Signing.LocalFileHeader localFileHeader;

                        if (!Signing.LocalFileHeader.TryRead(reader, out localFileHeader))
                        {
                            throw new InvalidDataException("Could not find local file header for the package signature file.");
                        }

                        localFileHeaderOffset = centralDirectoryHeader.RelativeOffsetOfLocalHeader;

                        return;
                    }
                }

                throw new InvalidDataException("Could not find central directory header for the package signature file.");
            }
        }

        private sealed class Test : IDisposable
        {
            private readonly MemoryStream _stream;
            private bool _isDisposed;

            internal BinaryReader Reader { get; }

            internal Test(byte[] bytes)
            {
                _stream = new MemoryStream(bytes);
                Reader = new BinaryReader(_stream);
            }

            public void Dispose()
            {
                if (!_isDisposed)
                {
                    Reader.Dispose();
                    _stream.Dispose();

                    GC.SuppressFinalize(this);

                    _isDisposed = true;
                }
            }
        }

#if IS_SIGNING_SUPPORTED
        private sealed class RemoveTest : IDisposable
        {
            private bool _isDisposed;

            internal X509Certificate2 Certificate { get; }
            internal Zip Zip { get; }
            internal MemoryStream SignedPackage { get; private set; }
            internal MemoryStream UnsignedPackage { get; }

            internal RemoveTest(CertificatesFixture fixture)
            {
                Certificate = fixture.GetDefaultCertificate();

                Zip = new Zip();

                Zip.LocalFileHeaders.Add(Zip.ContentLocalFileHeader);
                Zip.LocalFileHeaders.Add(Zip.NuspecLocalFileHeader);

                Zip.CentralDirectoryHeaders.Add(Zip.ContentCentralDirectoryHeader);
                Zip.CentralDirectoryHeaders.Add(Zip.NuspecCentralDirectoryHeader);

                UnsignedPackage = new MemoryStream();
            }

            internal async Task AuthorSignAsync()
            {
                using (var request = new AuthorSignPackageRequest(
                    new X509Certificate2(Certificate),
                    HashAlgorithmName.SHA256,
                    HashAlgorithmName.SHA256))
                using (var originalPackage = new MemoryStream(Zip.ToByteArray(), writable: false))
                using (var signedPackage = new MemoryStream())
                {
                    await SignedArchiveTestUtility.CreateSignedPackageAsync(request, originalPackage, signedPackage);

                    SignedPackage = new MemoryStream(signedPackage.ToArray(), writable: false);
                }

                var isSigned = await SignedArchiveTestUtility.IsSignedAsync(SignedPackage);

                Assert.True(isSigned);
            }

            internal async Task RepositorySignAsync()
            {
                using (var request = new RepositorySignPackageRequest(
                    new X509Certificate2(Certificate),
                    HashAlgorithmName.SHA256,
                    HashAlgorithmName.SHA256,
                    new Uri("https://test.test"),
                    packageOwners: null))
                using (var originalPackage = new MemoryStream(Zip.ToByteArray(), writable: false))
                using (var signedPackage = new MemoryStream())
                {
                    await SignedArchiveTestUtility.CreateSignedPackageAsync(request, originalPackage, signedPackage);

                    SignedPackage = new MemoryStream(signedPackage.ToArray(), writable: false);
                }

                var isSigned = await SignedArchiveTestUtility.IsSignedAsync(SignedPackage);

                Assert.True(isSigned);
            }

            internal async Task RepositoryCountersignAsync()
            {
                PrimarySignature primarySignature;

                using (var archiveReader = new PackageArchiveReader(SignedPackage))
                {
                    primarySignature = await archiveReader.GetPrimarySignatureAsync(CancellationToken.None);
                }

                using (var request = new RepositorySignPackageRequest(
                    new X509Certificate2(Certificate),
                    HashAlgorithmName.SHA256,
                    HashAlgorithmName.SHA256,
                    new Uri("https://test.test"),
                    packageOwners: null))
                {
                    var cmsSigner = SigningUtility.CreateCmsSigner(request, NullLogger.Instance);
                    var signedCms = primarySignature.SignedCms;

                    signedCms.SignerInfos[0].ComputeCounterSignature(cmsSigner);

                    primarySignature = PrimarySignature.Load(signedCms.Encode());
                }

                using (var originalPackage = new MemoryStream(Zip.ToByteArray(), writable: false))
                using (var signedPackage = new MemoryStream())
                using (var archive = new SignedPackageArchive(originalPackage, signedPackage))
                using (var signatureStream = new MemoryStream(primarySignature.GetBytes()))
                {
                    await archive.AddSignatureAsync(signatureStream, CancellationToken.None);

                    SignedPackage = new MemoryStream(signedPackage.ToArray(), writable: false);
                }

                var isSigned = await SignedArchiveTestUtility.IsSignedAsync(SignedPackage);

                Assert.True(isSigned);
            }

            internal async Task CountersignAsync()
            {
                PrimarySignature primarySignature;

                using (var archiveReader = new PackageArchiveReader(SignedPackage))
                {
                    primarySignature = await archiveReader.GetPrimarySignatureAsync(CancellationToken.None);
                }

                using (var request = new UnknownSignPackageRequest(
                    new X509Certificate2(Certificate),
                    HashAlgorithmName.SHA256))
                {
                    var cmsSigner = SigningUtility.CreateCmsSigner(request, NullLogger.Instance);
                    var signedCms = primarySignature.SignedCms;

                    signedCms.SignerInfos[0].ComputeCounterSignature(cmsSigner);

                    primarySignature = PrimarySignature.Load(signedCms.Encode());
                }

                using (var originalPackage = new MemoryStream(Zip.ToByteArray(), writable: false))
                using (var signedPackage = new MemoryStream())
                using (var archive = new SignedPackageArchive(originalPackage, signedPackage))
                using (var signatureStream = new MemoryStream(primarySignature.GetBytes()))
                {
                    await archive.AddSignatureAsync(signatureStream, CancellationToken.None);

                    SignedPackage = new MemoryStream(signedPackage.ToArray(), writable: false);
                }

                var isSigned = await SignedArchiveTestUtility.IsSignedAsync(SignedPackage);

                Assert.True(isSigned);
            }

            public void Dispose()
            {
                if (!_isDisposed)
                {
                    Certificate.Dispose();
                    SignedPackage?.Dispose();
                    UnsignedPackage?.Dispose();

                    GC.SuppressFinalize(this);

                    _isDisposed = true;
                }
            }

            private sealed class UnknownSignPackageRequest : SignPackageRequest
            {
                public override SignatureType SignatureType => SignatureType.Unknown;

                internal UnknownSignPackageRequest(X509Certificate2 certificate, HashAlgorithmName hashAlgorithm) :
                    base(certificate, hashAlgorithm, hashAlgorithm)
                {
                }
            }
        }
#endif
    }
}
