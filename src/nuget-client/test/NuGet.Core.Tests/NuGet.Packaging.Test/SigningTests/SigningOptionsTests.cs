// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#if IS_SIGNING_SUPPORTED
using System;
using System.IO;
using Moq;
using NuGet.Common;
using NuGet.Packaging.Signing;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Packaging.Test
{
    public class SigningOptionsTests
    {
        [Fact]
        public void Constructor_WhenInputPackageStreamIsNull_Throws()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new SigningOptions(
                    inputPackageStream: null,
                    outputPackageStream: new Lazy<Stream>(() => Stream.Null),
                    overwrite: true,
                    signatureProvider: Mock.Of<ISignatureProvider>(),
                    logger: Mock.Of<ILogger>()));

            Assert.Equal("inputPackageStream", exception.ParamName);
        }

        [Fact]
        public void Constructor_WhenOutputPackageStreamIsNull_Throws()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new SigningOptions(
                    inputPackageStream: new Lazy<Stream>(() => Stream.Null),
                    outputPackageStream: null,
                    overwrite: true,
                    signatureProvider: Mock.Of<ISignatureProvider>(),
                    logger: Mock.Of<ILogger>()));

            Assert.Equal("outputPackageStream", exception.ParamName);
        }

        [Fact]
        public void Constructor_WhenSignatureProviderIsNull_Throws()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new SigningOptions(
                    inputPackageStream: new Lazy<Stream>(() => Stream.Null),
                    outputPackageStream: new Lazy<Stream>(() => Stream.Null),
                    overwrite: true,
                    signatureProvider: null,
                    logger: Mock.Of<ILogger>()));

            Assert.Equal("signatureProvider", exception.ParamName);
        }

        [Fact]
        public void Constructor_WhenLoggerIsNull_Throws()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new SigningOptions(
                    inputPackageStream: new Lazy<Stream>(() => Stream.Null),
                    outputPackageStream: new Lazy<Stream>(() => Stream.Null),
                    overwrite: true,
                    signatureProvider: Mock.Of<ISignatureProvider>(),
                    logger: null));

            Assert.Equal("logger", exception.ParamName);
        }

        [Fact]
        public void Constructor_WithValidInput_InitializesProperties()
        {
            var inputPackageStream = new Lazy<Stream>(() => Stream.Null);
            var outputPackageStream = new Lazy<Stream>(() => Stream.Null);
            var overwrite = true;
            var signatureProvider = Mock.Of<ISignatureProvider>();
            var logger = Mock.Of<ILogger>();

            using (var options = new SigningOptions(inputPackageStream, outputPackageStream, overwrite, signatureProvider, logger))
            {
                Assert.Same(inputPackageStream.Value, options.InputPackageStream);
                Assert.Same(outputPackageStream.Value, options.OutputPackageStream);
                Assert.Equal(overwrite, options.Overwrite);
                Assert.Same(signatureProvider, options.SignatureProvider);
                Assert.Same(logger, options.Logger);
            }
        }

        [Fact]
        public void Dispose_DisposesStreams()
        {
            var inputPackageStream = new Lazy<Stream>(() => new MemoryStream());
            var outputPackageStream = new Lazy<Stream>(() => new MemoryStream());
            var overwrite = true;
            var signatureProvider = Mock.Of<ISignatureProvider>();
            var logger = Mock.Of<ILogger>();

            using (var options = new SigningOptions(inputPackageStream, outputPackageStream, overwrite, signatureProvider, logger))
            {
                Assert.True(inputPackageStream.Value.CanWrite);
                Assert.True(outputPackageStream.Value.CanWrite);
            }

            Assert.False(inputPackageStream.Value.CanWrite);
            Assert.False(outputPackageStream.Value.CanWrite);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void CreateFromFilePaths_WhenInputPackageFilePathIsNullOrEmpty_Throws(string inputPackageFilePath)
        {
            var exception = Assert.Throws<ArgumentException>(
                () => SigningOptions.CreateFromFilePaths(
                    inputPackageFilePath,
                    outputPackageFilePath: "a",
                    overwrite: true,
                    signatureProvider: Mock.Of<ISignatureProvider>(),
                    logger: Mock.Of<ILogger>()));

            Assert.Equal("inputPackageFilePath", exception.ParamName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void CreateFromFilePaths_WhenOutputPackageFilePathIsNullOrEmpty_Throws(string outputPackageFilePath)
        {
            var exception = Assert.Throws<ArgumentException>(
                () => SigningOptions.CreateFromFilePaths(
                    inputPackageFilePath: "a",
                    outputPackageFilePath: outputPackageFilePath,
                    overwrite: true,
                    signatureProvider: Mock.Of<ISignatureProvider>(),
                    logger: Mock.Of<ILogger>()));

            Assert.Equal("outputPackageFilePath", exception.ParamName);
        }

        [Fact]
        public void CreateFromFilePaths_WhenInputPackageFilePathAndOutputPackageFilePathAreEquivalent_Throws()
        {
            var exception = Assert.Throws<ArgumentException>(
                () => SigningOptions.CreateFromFilePaths(
                    inputPackageFilePath: Path.Combine(Path.GetTempPath(), "file"),
                    outputPackageFilePath: Path.Combine(Path.GetTempPath(), "FILE"),
                    overwrite: true,
                    signatureProvider: Mock.Of<ISignatureProvider>(),
                    logger: Mock.Of<ILogger>()));

            Assert.Equal("inputPackageFilePath and outputPackageFilePath should be different. Package signing cannot be done in place.", exception.Message);
        }

        [Fact]
        public void CreateFromFilePaths_WhenSignatureProviderIsNull_Throws()
        {
            using (var directory = TestDirectory.Create())
            {
                var exception = Assert.Throws<ArgumentNullException>(
                    () => SigningOptions.CreateFromFilePaths(
                        inputPackageFilePath: Path.Combine(Path.GetTempPath(), "a"),
                        outputPackageFilePath: Path.Combine(Path.GetTempPath(), "b"),
                        overwrite: true,
                        signatureProvider: null,
                        logger: Mock.Of<ILogger>()));

                Assert.Equal("signatureProvider", exception.ParamName);
            }
        }

        [Fact]
        public void CreateFromFilePaths_WhenLoggerIsNull_Throws()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => SigningOptions.CreateFromFilePaths(
                    inputPackageFilePath: Path.Combine(Path.GetTempPath(), "a"),
                    outputPackageFilePath: Path.Combine(Path.GetTempPath(), "b"),
                    overwrite: true,
                    signatureProvider: Mock.Of<ISignatureProvider>(),
                    logger: null));

            Assert.Equal("logger", exception.ParamName);
        }

        [Fact]
        public void CreateFromFilePaths_WithValidInput_InitializesProperties()
        {
            using (var directory = TestDirectory.Create())
            {
                var inputPackageFilePath = Path.Combine(directory, "a");
                var outputPackageFilePath = Path.Combine(directory, "b");
                var overwrite = false;
                var signatureProvider = Mock.Of<ISignatureProvider>();
                var logger = Mock.Of<ILogger>();

                File.WriteAllBytes(inputPackageFilePath, Array.Empty<byte>());

                using (var options = SigningOptions.CreateFromFilePaths(
                    inputPackageFilePath,
                    outputPackageFilePath,
                    overwrite,
                    signatureProvider,
                    logger))
                {
                    Assert.NotNull(options.InputPackageStream);
                    Assert.NotNull(options.OutputPackageStream);
                    Assert.Equal(overwrite, options.Overwrite);
                    Assert.Same(signatureProvider, options.SignatureProvider);
                    Assert.Same(logger, options.Logger);
                }
            }
        }
    }
}
#endif
