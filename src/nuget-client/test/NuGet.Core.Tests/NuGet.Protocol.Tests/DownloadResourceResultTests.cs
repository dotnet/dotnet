// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Frameworks;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Packaging.Signing;
using NuGet.Protocol.Core.Types;
using Xunit;

namespace NuGet.Protocol.Tests
{
    public class DownloadResourceResultTests
    {
        [Theory]
        [InlineData(DownloadResourceResultStatus.Available)]
        [InlineData(DownloadResourceResultStatus.AvailableWithoutStream)]
        public void Constructor_Status_ThrowsForInvalidStatus(DownloadResourceResultStatus status)
        {
            var exception = Assert.Throws<ArgumentException>(() => new DownloadResourceResult(status));

            var expectedMessage = "A stream should be provided when the result is available.";

            Assert.Equal("status", exception.ParamName);
            Assert.Contains(expectedMessage, exception.Message);

            //Remove the expected message from the exception message, the rest part should have param info.
            //Background of this change: System.ArgumentException(string message, string paramName) used to generate two lines of message before, but changed to generate one line
            //in PR: https://github.com/dotnet/coreclr/pull/25185/files#diff-0365d5690376ef849bf908dfc225b8e8
            var paramPart = exception.Message.Substring(exception.Message.IndexOf(expectedMessage) + expectedMessage.Length);
            Assert.Contains("status", paramPart);
        }

        [Theory]
        [InlineData(DownloadResourceResultStatus.Cancelled)]
        [InlineData(DownloadResourceResultStatus.NotFound)]
        public void Constructor_Status_InitializesProperties(DownloadResourceResultStatus status)
        {
            using (var result = new DownloadResourceResult(status))
            {
                Assert.Null(result.PackageReader);
                Assert.Null(result.PackageSource);
                Assert.Null(result.PackageStream);
                Assert.Equal(status, result.Status);
            }
        }

        [Fact]
        public void Constructor_PackageReaderSource_ThrowsForNullPackageReader()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new DownloadResourceResult(packageReader: null, source: "a"));

            Assert.Equal("packageReader", exception.ParamName);
        }

        [Fact]
        public void Constructor_PackageReaderSource_AllowsNullSource()
        {
            using (var packageReader = new TestPackageReader())
            using (var result = new DownloadResourceResult(packageReader, source: null))
            {
                Assert.Null(result.PackageSource);
            }
        }

        [Fact]
        public void Constructor_PackageReaderSource_InitializesProperties()
        {
            using (var packageReader = new TestPackageReader())
            using (var result = new DownloadResourceResult(packageReader, source: "a"))
            {
                Assert.Same(packageReader, result.PackageReader);
                Assert.Equal("a", result.PackageSource);
                Assert.Null(result.PackageStream);
                Assert.Equal(DownloadResourceResultStatus.AvailableWithoutStream, result.Status);
            }
        }

        [Fact]
        public void Constructor_Stream_ThrowsForNullStream()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new DownloadResourceResult(stream: null, source: null));

            Assert.Equal("stream", exception.ParamName);
        }

        [Fact]
        public void Constructor_Stream_InitializesProperties()
        {
            using (var result = new DownloadResourceResult(Stream.Null, source: null))
            {
                Assert.Null(result.PackageReader);
                Assert.Null(result.PackageSource);
                Assert.Same(Stream.Null, result.PackageStream);
                Assert.Equal(DownloadResourceResultStatus.Available, result.Status);
            }
        }

        [Fact]
        public void Constructor_StreamSource_ThrowsForNullStream()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new DownloadResourceResult(stream: null, source: "a"));

            Assert.Equal("stream", exception.ParamName);
        }

        [Fact]
        public void Constructor_StreamSource_AllowsNullSource()
        {
            using (var result = new DownloadResourceResult(Stream.Null, source: null))
            {
                Assert.Null(result.PackageSource);
            }
        }

        [Fact]
        public void Constructor_StreamSource_InitializesProperties()
        {
            using (var result = new DownloadResourceResult(Stream.Null, source: "a"))
            {
                Assert.Null(result.PackageReader);
                Assert.Equal("a", result.PackageSource);
                Assert.Same(Stream.Null, result.PackageStream);
                Assert.Equal(DownloadResourceResultStatus.Available, result.Status);
            }
        }

        [Fact]
        public void Constructor_StreamPackageReaderBase_ThrowsForNullStream()
        {
            using (var packageReader = new TestPackageReader())
            {
                var exception = Assert.Throws<ArgumentNullException>(
                    () => new DownloadResourceResult(stream: null, packageReader: packageReader, source: null));

                Assert.Equal("stream", exception.ParamName);
            }
        }

        [Fact]
        public void Constructor_StreamPackageReaderBase_AllowsNullPackageReader()
        {
            using (var result = new DownloadResourceResult(Stream.Null, packageReader: null, source: null))
            {
                Assert.Null(result.PackageReader);
            }
        }

        [Fact]
        public void Constructor_StreamPackageReaderBase_InitializesProperties()
        {
            using (var packageReader = new TestPackageReader())
            using (var result = new DownloadResourceResult(Stream.Null, packageReader, source: null))
            {
                Assert.Same(packageReader, result.PackageReader);
                Assert.Null(result.PackageSource);
                Assert.Same(Stream.Null, result.PackageStream);
                Assert.Equal(DownloadResourceResultStatus.Available, result.Status);
            }
        }

        [Fact]
        public void Constructor_StreamPackageReaderBaseSource_ThrowsForNullStream()
        {
            using (var packageReader = new TestPackageReader())
            {
                var exception = Assert.Throws<ArgumentNullException>(
                    () => new DownloadResourceResult(stream: null, packageReader: packageReader, source: "a"));

                Assert.Equal("stream", exception.ParamName);
            }
        }

        [Fact]
        public void Constructor_StreamPackageReaderBaseSource_AllowsNullPackageReaderAndSource()
        {
            using (var result = new DownloadResourceResult(Stream.Null, packageReader: null, source: null))
            {
                Assert.Null(result.PackageReader);
                Assert.Null(result.PackageSource);
            }
        }

        [Fact]
        public void Constructor_StreamPackageReaderBaseSource_InitializesProperties()
        {
            using (var packageReader = new TestPackageReader())
            using (var result = new DownloadResourceResult(Stream.Null, packageReader, source: "a"))
            {
                Assert.Same(packageReader, result.PackageReader);
                Assert.Equal("a", result.PackageSource);
                Assert.Same(Stream.Null, result.PackageStream);
                Assert.Equal(DownloadResourceResultStatus.Available, result.Status);
            }
        }

        [Fact]
        public void Dispose_IsIdempotent()
        {
            using (var stream = new TestStream())
            using (var result = new DownloadResourceResult(stream, source: null))
            {
                result.Dispose();
                result.Dispose();

                Assert.Equal(1, stream.DisposeCallCount);
            }
        }

        private sealed class TestPackageReader : PackageReaderBase
        {
            public TestPackageReader()
                : base(DefaultFrameworkNameProvider.Instance, DefaultCompatibilityProvider.Instance)
            {
            }

            public override IEnumerable<string> CopyFiles(
                string destination,
                IEnumerable<string> packageFiles,
                ExtractPackageFileDelegate extractFile,
                ILogger logger,
                CancellationToken token)
            {
                throw new NotImplementedException();
            }

            public override string GetContentHash(CancellationToken token, Func<string> GetUnsignedPackageHash = null)
            {
                throw new NotImplementedException();
            }

            public override Task<byte[]> GetArchiveHashAsync(HashAlgorithmName hashAlgorithm, CancellationToken token)
            {
                throw new NotImplementedException();
            }

            public override IEnumerable<string> GetFiles()
            {
                throw new NotImplementedException();
            }

            public override IEnumerable<string> GetFiles(string folder)
            {
                throw new NotImplementedException();
            }

            public override Task<PrimarySignature> GetPrimarySignatureAsync(CancellationToken token)
            {
                return TaskResult.Null<PrimarySignature>();
            }

            public override Stream GetStream(string path)
            {
                throw new NotImplementedException();
            }

            public override Task<bool> IsSignedAsync(CancellationToken token)
            {
                return TaskResult.False;
            }

            public override Task ValidateIntegrityAsync(SignatureContent signatureContent, CancellationToken token)
            {
                throw new NotImplementedException();
            }

            public override bool CanVerifySignedPackages(SignedPackageVerifierSettings verifierSettings)
            {
                return false;
            }

            protected override void Dispose(bool disposing)
            {
            }
        }

        private sealed class TestStream : MemoryStream
        {
            internal int DisposeCallCount { get; private set; }

            protected override void Dispose(bool disposing)
            {
                base.Dispose(disposing);

                ++DisposeCallCount;
            }
        }
    }
}
