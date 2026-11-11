// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Microsoft.Deployment.DotNet.Releases.Tests
{
    public class UtilsTests : TestBase
    {
        [Theory]
        [InlineData(-60, true, false)]
        [InlineData(0, true, true)]
        [InlineData(60, true, true)]
        [InlineData(0, false, false)]
        public async Task IsLatestFileAsyncComparesModificationTimes(int fileOffsetSeconds, bool includeLastModified, bool expected)
        {
            string fileName = Path.GetTempFileName();
            var lastModified = new DateTimeOffset(2024, 1, 15, 12, 0, 0, TimeSpan.Zero);
            using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            using var listener = new TcpListener(IPAddress.Loopback, 0);

            try
            {
                File.SetLastWriteTimeUtc(fileName, lastModified.UtcDateTime.AddSeconds(fileOffsetSeconds));
                listener.Start();
                var endpoint = (IPEndPoint)listener.LocalEndpoint;
                var address = new Uri($"http://127.0.0.1:{endpoint.Port}/releases.json");

                Task responseTask = RespondToHeadRequestAsync(listener, includeLastModified ? lastModified : null, timeout.Token);
                Task<bool> isLatestTask = Utils.IsLatestFileAsync(fileName, address);
                await Task.WhenAll(responseTask, isLatestTask).WaitAsync(timeout.Token);

                Assert.Equal(expected, await isLatestTask);
            }
            finally
            {
                File.Delete(fileName);
            }
        }

        private static async Task RespondToHeadRequestAsync(TcpListener listener, DateTimeOffset? lastModified, CancellationToken cancellationToken)
        {
            using TcpClient client = await listener.AcceptTcpClientAsync(cancellationToken);
            using NetworkStream stream = client.GetStream();
            using var reader = new StreamReader(stream, Encoding.ASCII, detectEncodingFromByteOrderMarks: false, leaveOpen: true);

            Assert.StartsWith("HEAD ", await reader.ReadLineAsync(cancellationToken));
            while (!string.IsNullOrEmpty(await reader.ReadLineAsync(cancellationToken)))
            {
            }

            string lastModifiedHeader = lastModified.HasValue
                ? $"Last-Modified: {lastModified.Value.ToString("R", CultureInfo.InvariantCulture)}\r\n"
                : string.Empty;
            byte[] response = Encoding.ASCII.GetBytes($"HTTP/1.1 200 OK\r\n{lastModifiedHeader}Content-Length: 0\r\nConnection: close\r\n\r\n");
            await stream.WriteAsync(response, cancellationToken);
        }

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
            await Utils.DownloadFileAsync(sourceAddress, destinationFile);

            HashAlgorithm sha256 = SHA256.Create();
            string sourceHash = Utils.GetFileHash(sourceFile, sha256);
            string destinationHash = Utils.GetFileHash(destinationFile, sha256);

            Assert.True(File.Exists(destinationFile));
            Assert.Equal(sourceHash, destinationHash);
        }
    }
}
