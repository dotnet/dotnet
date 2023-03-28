// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Xunit;

namespace Microsoft.Deployment.DotNet.Releases.Tests
{
    public class UtilsTests : TestBase
    {
        [Fact]
        public void GetFileHashThrowsIfFileNameIsNull()
        {
            ArgumentNullException e = Assert.Throws<ArgumentNullException>(() =>
            {
                Utils.GetFileHash(null, SHA512.Create());
            });
        }

        [Fact]
        public void GetFileHashThrowsIfFileNameIsEmpty()
        {
            ArgumentException e = Assert.Throws<ArgumentException>(() =>
            {
                Utils.GetFileHash("", SHA512.Create());
            });

            Assert.Equal($"Value cannot be empty. (Parameter 'fileName')", e.Message);
        }

        [Fact]
        public void GetFileHashThrowsIfHashAlgorithmIsNull()
        {
            ArgumentNullException e = Assert.Throws<ArgumentNullException>(() =>
            {
                Utils.GetFileHash("File.txt", null);
            });

            Assert.Equal($"Value cannot be null. (Parameter 'hashAlgorithm')", e.Message);
        }

        [Fact]
        public async Task DownloadFileAsyncCanCopyFiles()
        {
            string sourceFile = Path.GetTempFileName();

            using (FileStream fs = new FileStream(Path.GetTempFileName(), FileMode.Create, FileAccess.Write, FileShare.None))
            {
                fs.SetLength(1024);
            }

            Uri sourceAddress = new Uri(Path.GetFullPath(sourceFile));
            string destinationFile = Path.GetFullPath(Path.GetTempFileName());
            await Utils.DownloadFileAsync(sourceAddress, destinationFile).ConfigureAwait(false);

            HashAlgorithm sha256 = SHA256.Create();
            string sourceHash = Utils.GetFileHash(sourceFile, sha256);
            string destinationHash = Utils.GetFileHash(destinationFile, sha256);

            Assert.True(File.Exists(destinationFile));
            Assert.Equal(sourceHash, destinationHash);
        }
    }
}
