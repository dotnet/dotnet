// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Packaging.Signing;
using Test.Utility.Signing;
using Xunit;

namespace NuGet.Packaging.Test
{
    public class SignedPackageArchiveTests
    {
        [Theory]
        [InlineData(false, 0)]
        [InlineData(true, 0)]
        [InlineData(false, 4)]
        [InlineData(true, 4)]
        [InlineData(false, 18)]
        [InlineData(true, 18)]
        [InlineData(false, 4093)]
        [InlineData(true, 4093)]
        [InlineData(false, 4094)]
        [InlineData(true, 4094)]
        [InlineData(false, 4095)]
        [InlineData(true, 4095)]
        [InlineData(false, ushort.MaxValue)]
        [InlineData(true, ushort.MaxValue)]
        public void Read_WithTwoCentralDirectories_UsesCorrectDirectory(bool isSigned, int commentLength)
        {
            var zip = new Zip();
            zip.ContentLocalFileHeader.FileData = new byte[8192];
            zip.LocalFileHeaders.Add(zip.ContentLocalFileHeader);
            zip.LocalFileHeaders.Add(zip.SignatureLocalFileHeader);
            zip.CentralDirectoryHeaders.Add(zip.ContentCentralDirectoryHeader);
            if (!isSigned)
            {
                zip.CentralDirectoryHeaders.Add(zip.SignatureCentralDirectoryHeader);
            }

            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream, Encoding.UTF8, leaveOpen: true))
            using (var reader = new BinaryReader(stream, Encoding.UTF8, leaveOpen: true))
            {
                writer.Write(zip.ToByteArray());

                zip.CentralDirectoryHeaders.Clear();
                zip.CentralDirectoryHeaders.Add(zip.ContentCentralDirectoryHeader);
                if (isSigned)
                {
                    zip.CentralDirectoryHeaders.Add(zip.SignatureCentralDirectoryHeader);
                }

                foreach (var header in zip.CentralDirectoryHeaders)
                {
                    header.Write(writer);
                }

                var comment = new byte[commentLength];
                if (commentLength >= 4)
                {
                    // These bytes are too close to EOF to begin a complete EOCD record.
                    new byte[] { 0x50, 0x4b, 0x05, 0x06 }.CopyTo(comment, commentLength - 4);
                }

                zip.EndOfCentralDirectoryRecord.FileComment = comment;
                zip.EndOfCentralDirectoryRecord.Write(writer);
                stream.Position = 0;

                var record = Signing.EndOfCentralDirectoryRecord.Read(reader);

                Assert.Equal((long)zip.EndOfCentralDirectoryRecord.OffsetFromStart, record.OffsetFromStart);
                Assert.Equal(zip.EndOfCentralDirectoryRecord.OffsetOfStartOfCentralDirectory,
                    record.OffsetOfStartOfCentralDirectory);
                Assert.Equal(zip.EndOfCentralDirectoryRecord.SizeOfCentralDirectory, record.SizeOfCentralDirectory);
                Assert.Equal(zip.EndOfCentralDirectoryRecord.CountOfEntriesInCentralDirectory,
                    record.CountOfEntriesInCentralDirectory);
                Assert.Equal(comment, record.FileComment);
                Assert.Equal(isSigned, SignedPackageArchiveUtility.IsSigned(reader));
            }
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public void Read_WithSignatureLastByteRepeatedAfterSignature_ReturnsRecord(int commentLength)
        {
            var bytes = new byte[22 + commentLength];
            new byte[] { 0x50, 0x4b, 0x05, 0x06 }.CopyTo(bytes, 0);
            bytes[4] = 6; // Number of this disk.
            bytes[6] = 6; // Number of the disk with the central directory.
            bytes[20] = (byte)commentLength;

            using (var stream = new MemoryStream(bytes))
            using (var reader = new BinaryReader(stream))
            {
                var record = Signing.EndOfCentralDirectoryRecord.Read(reader);

                Assert.Equal(0, record.OffsetFromStart);
                Assert.Equal(6, record.NumberOfThisDisk);
                Assert.Equal(6, record.NumberOfTheDiskWithTheStartOfTheCentralDirectory);
                Assert.Equal(new byte[commentLength], record.FileComment);
            }
        }

        [Theory]
        [InlineData(0)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(21)]
        public void Read_WithTruncatedRecord_ThrowsInvalidDataException(int length)
        {
            var bytes = new byte[length];
            if (length >= 4)
            {
                new byte[] { 0x50, 0x4b, 0x05, 0x06 }.CopyTo(bytes, 0);
            }

            using (var stream = new MemoryStream(bytes))
            using (var reader = new BinaryReader(stream))
            {
                Assert.Throws<InvalidDataException>(() => Signing.EndOfCentralDirectoryRecord.Read(reader));
            }
        }

        [Fact]
        public void Read_WithRecordOutsideMaximumCommentLength_ThrowsInvalidDataException()
        {
            var zip = new Zip();
            zip.LocalFileHeaders.Add(zip.ContentLocalFileHeader);
            zip.CentralDirectoryHeaders.Add(zip.ContentCentralDirectoryHeader);
            zip.EndOfCentralDirectoryRecord.FileComment = new byte[ushort.MaxValue + 1];

            using (var stream = new MemoryStream(zip.ToByteArray()))
            using (var reader = new BinaryReader(stream))
            {
                Assert.Throws<InvalidDataException>(() => Signing.EndOfCentralDirectoryRecord.Read(reader));
            }
        }

        [Fact]
        public async Task RemoveSignatureAsync_WithDefaults_ReturnsOriginalPackage()
        {
            var zip = new Zip();

            zip.LocalFileHeaders.Add(zip.ContentLocalFileHeader);
            zip.LocalFileHeaders.Add(zip.NuspecLocalFileHeader);

            zip.CentralDirectoryHeaders.Add(zip.ContentCentralDirectoryHeader);
            zip.CentralDirectoryHeaders.Add(zip.NuspecCentralDirectoryHeader);

            var expectedPackage = zip.ToByteArray();

            zip.LocalFileHeaders.Add(zip.SignatureLocalFileHeader);
            zip.CentralDirectoryHeaders.Add(zip.SignatureCentralDirectoryHeader);

            var signedPackage = zip.ToByteArray();

            await TestRemoveSignatureAsync(expectedPackage, signedPackage);
        }

        [Fact]
        public async Task RemoveSignatureAsync_WithNonDefaultVersionMadeBy_ReturnsOriginalPackage()
        {
            var zip = new Zip();

            zip.ContentCentralDirectoryHeader.VersionMadeBy = 0x3f; // 6.3
            zip.NuspecCentralDirectoryHeader.VersionMadeBy = 0x2d; // 4.5

            zip.LocalFileHeaders.Add(zip.ContentLocalFileHeader);
            zip.LocalFileHeaders.Add(zip.NuspecLocalFileHeader);

            zip.CentralDirectoryHeaders.Add(zip.ContentCentralDirectoryHeader);
            zip.CentralDirectoryHeaders.Add(zip.NuspecCentralDirectoryHeader);

            var expectedPackage = zip.ToByteArray();

            zip.LocalFileHeaders.Add(zip.SignatureLocalFileHeader);
            zip.CentralDirectoryHeaders.Add(zip.SignatureCentralDirectoryHeader);

            var signedPackage = zip.ToByteArray();

            await TestRemoveSignatureAsync(expectedPackage, signedPackage);
        }

        [Fact]
        public async Task RemoveSignatureAsync_WithNonDefaultExternalFileAttributes_ReturnsOriginalPackage()
        {
            var zip = new Zip();

            zip.ContentCentralDirectoryHeader.ExternalFileAttributes = 0x20; // archived file
            zip.NuspecCentralDirectoryHeader.ExternalFileAttributes = 0x80; // normal file

            zip.LocalFileHeaders.Add(zip.ContentLocalFileHeader);
            zip.LocalFileHeaders.Add(zip.NuspecLocalFileHeader);

            zip.CentralDirectoryHeaders.Add(zip.ContentCentralDirectoryHeader);
            zip.CentralDirectoryHeaders.Add(zip.NuspecCentralDirectoryHeader);

            var expectedPackage = zip.ToByteArray();

            zip.LocalFileHeaders.Add(zip.SignatureLocalFileHeader);
            zip.CentralDirectoryHeaders.Add(zip.SignatureCentralDirectoryHeader);

            var signedPackage = zip.ToByteArray();

            await TestRemoveSignatureAsync(expectedPackage, signedPackage);
        }

        private static async Task TestRemoveSignatureAsync(byte[] expectedPackage, byte[] signedPackage)
        {
            using (var readStream = new MemoryStream(signedPackage))
            using (var writeStream = new MemoryStream())
            using (var signedArchive = new SignedPackageArchive(readStream, writeStream))
            {
                var isSigned = await SignedArchiveTestUtility.IsSignedAsync(readStream);

                Assert.True(isSigned);

                await signedArchive.RemoveSignatureAsync(CancellationToken.None);

                isSigned = await SignedArchiveTestUtility.IsSignedAsync(writeStream);

                Assert.False(isSigned);

                var unsignedPackage = writeStream.ToArray();

                Assert.Equal(expectedPackage, unsignedPackage);
            }
        }
    }
}
