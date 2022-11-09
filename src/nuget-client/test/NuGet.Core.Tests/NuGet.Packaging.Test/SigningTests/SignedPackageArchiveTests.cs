// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#if IS_SIGNING_SUPPORTED
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Packaging.Signing;
using Test.Utility.Signing;
using Xunit;

namespace NuGet.Packaging.Test
{
    public class SignedPackageArchiveTests
    {
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
#endif
