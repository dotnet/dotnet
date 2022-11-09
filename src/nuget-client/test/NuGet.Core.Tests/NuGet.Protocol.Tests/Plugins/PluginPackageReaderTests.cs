// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Moq;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Xunit;

namespace NuGet.Protocol.Plugins.Tests
{
    public class PluginPackageReaderTests
    {
        [Fact]
        public void Constructor_ThrowsForNullPlugin()
        {
            using (var test = new PluginPackageReaderTest())
            {
                var exception = Assert.Throws<ArgumentNullException>(
                    () => new PluginPackageReader(
                        plugin: null,
                        packageIdentity: test.PackageIdentity,
                        packageSourceRepository: "b"));

                Assert.Equal("plugin", exception.ParamName);
            }
        }

        [Fact]
        public void Constructor_ThrowsForNullPackageIdentity()
        {
            using (var test = new PluginPackageReaderTest())
            {
                var exception = Assert.Throws<ArgumentNullException>(
                    () => new PluginPackageReader(
                        test.Plugin.Object,
                        packageIdentity: null,
                        packageSourceRepository: "b"));

                Assert.Equal("packageIdentity", exception.ParamName);
            }
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Constructor_ThrowsForNullOrEmptyPackageSourceRepository(string packageSourceRepository)
        {
            using (var test = new PluginPackageReaderTest())
            {
                var exception = Assert.Throws<ArgumentException>(
                    () => new PluginPackageReader(
                        test.Plugin.Object,
                        test.PackageIdentity,
                        packageSourceRepository));

                Assert.Equal("packageSourceRepository", exception.ParamName);
            }
        }

        [Fact]
        public void GetStream_Throws()
        {
            using (var test = new PluginPackageReaderTest())
            {
                Assert.Throws<NotSupportedException>(() => test.Reader.GetStream(path: "a"));
            }
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task GetStreamAsync_ThrowsForNullOrEmptyPath(string path)
        {
            using (var test = new PluginPackageReaderTest())
            {
                var exception = await Assert.ThrowsAsync<ArgumentException>(
                    () => test.Reader.GetStreamAsync(path, CancellationToken.None));

                Assert.Equal("path", exception.ParamName);
            }
        }

        [Fact]
        public async Task GetStreamAsync_ThrowsIfCancelled()
        {
            using (var test = new PluginPackageReaderTest())
            {
                await Assert.ThrowsAsync<OperationCanceledException>(
                    () => test.Reader.GetStreamAsync(
                        path: "a",
                        cancellationToken: new CancellationToken(canceled: true)));
            }
        }

        [Fact]
        public async Task GetStreamAsync_ReturnsNullOnNotFound()
        {
            const string fileInPackage = "a";

            using (var test = new PluginPackageReaderTest())
            {
                test.Connection.Setup(x => x.SendRequestAndReceiveResponseAsync<CopyFilesInPackageRequest, CopyFilesInPackageResponse>(
                        It.Is<MessageMethod>(m => m == MessageMethod.CopyFilesInPackage),
                        It.Is<CopyFilesInPackageRequest>(c => c.PackageSourceRepository == test.PackageSource.Source
                            && c.PackageId == test.PackageIdentity.Id
                            && c.PackageVersion == test.PackageIdentity.Version.ToNormalizedString()
                            && c.FilesInPackage.Single() == fileInPackage),
                        It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new CopyFilesInPackageResponse(MessageResponseCode.NotFound, copiedFiles: null));

                var stream = await test.Reader.GetStreamAsync(fileInPackage, CancellationToken.None);

                Assert.Null(stream);
            }
        }

        [Fact]
        public async Task GetStreamAsync_ThrowsOnError()
        {
            const string fileInPackage = "a";

            using (var test = new PluginPackageReaderTest())
            {
                test.Connection.Setup(x => x.SendRequestAndReceiveResponseAsync<CopyFilesInPackageRequest, CopyFilesInPackageResponse>(
                        It.Is<MessageMethod>(m => m == MessageMethod.CopyFilesInPackage),
                        It.Is<CopyFilesInPackageRequest>(c => c.PackageSourceRepository == test.PackageSource.Source
                            && c.PackageId == test.PackageIdentity.Id
                            && c.PackageVersion == test.PackageIdentity.Version.ToNormalizedString()
                            && c.FilesInPackage.Single() == fileInPackage),
                        It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new CopyFilesInPackageResponse(MessageResponseCode.Error, copiedFiles: null));

                var exception = await Assert.ThrowsAsync<PluginException>(
                    () => test.Reader.GetStreamAsync(fileInPackage, CancellationToken.None));

                Assert.Equal($"Plugin '{test.PluginName}' failed a CopyFilesInPackage operation for package {test.PackageIdentity.Id}.{test.PackageIdentity.Version.ToNormalizedString()}.", exception.Message);
            }
        }

        [Fact]
        public async Task GetStreamAsync_ReturnsStreamOnSuccess()
        {
            const string fileInPackage = "a";
            const string fileContent = "b";

            using (var test = new PluginPackageReaderTest())
            {
                CopyFilesInPackageResponse response = null;

                test.Connection.Setup(x => x.SendRequestAndReceiveResponseAsync<CopyFilesInPackageRequest, CopyFilesInPackageResponse>(
                        It.Is<MessageMethod>(m => m == MessageMethod.CopyFilesInPackage),
                        It.Is<CopyFilesInPackageRequest>(c => c.PackageSourceRepository == test.PackageSource.Source
                            && c.PackageId == test.PackageIdentity.Id
                            && c.PackageVersion == test.PackageIdentity.Version.ToNormalizedString()
                            && c.FilesInPackage.Single() == fileInPackage),
                        It.IsAny<CancellationToken>()))
                    .Callback<MessageMethod, CopyFilesInPackageRequest, CancellationToken>(
                        (method, request, cancellationToken) =>
                        {
                            var filePath = Path.Combine(request.DestinationFolderPath, request.FilesInPackage.Single());

                            File.WriteAllText(filePath, fileContent);

                            response = new CopyFilesInPackageResponse(MessageResponseCode.Success, new[] { filePath });
                        }
                    )
                    .ReturnsAsync(() => response);

                using (var stream = await test.Reader.GetStreamAsync(fileInPackage, CancellationToken.None))
                using (var reader = new StreamReader(stream))
                {
                    var streamContent = reader.ReadToEnd();

                    Assert.Equal(fileContent, streamContent);
                }
            }
        }

        [Fact]
        public void GetFiles_Throws()
        {
            using (var test = new PluginPackageReaderTest())
            {
                Assert.Throws<NotSupportedException>(() => test.Reader.GetFiles());
            }
        }

        [Fact]
        public async Task GetFilesAsync_ThrowsIfCancelled()
        {
            using (var test = new PluginPackageReaderTest())
            {
                await Assert.ThrowsAsync<OperationCanceledException>(
                    () => test.Reader.GetFilesAsync(new CancellationToken(canceled: true)));
            }
        }

        [Fact]
        public async Task GetFilesAsync_ReturnsEmptyEnumerableOnNotFound()
        {
            using (var test = new PluginPackageReaderTest())
            {
                test.Connection.Setup(x => x.SendRequestAndReceiveResponseAsync<GetFilesInPackageRequest, GetFilesInPackageResponse>(
                        It.Is<MessageMethod>(m => m == MessageMethod.GetFilesInPackage),
                        It.Is<GetFilesInPackageRequest>(c => c.PackageSourceRepository == test.PackageSource.Source
                            && c.PackageId == test.PackageIdentity.Id
                            && c.PackageVersion == test.PackageIdentity.Version.ToNormalizedString()),
                        It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new GetFilesInPackageResponse(MessageResponseCode.NotFound, files: null));

                var files = await test.Reader.GetFilesAsync(CancellationToken.None);

                Assert.Empty(files);
            }
        }

        [Fact]
        public async Task GetFilesAsync_ThrowsOnError()
        {
            using (var test = new PluginPackageReaderTest())
            {
                test.Connection.Setup(x => x.SendRequestAndReceiveResponseAsync<GetFilesInPackageRequest, GetFilesInPackageResponse>(
                        It.Is<MessageMethod>(m => m == MessageMethod.GetFilesInPackage),
                        It.Is<GetFilesInPackageRequest>(c => c.PackageSourceRepository == test.PackageSource.Source
                            && c.PackageId == test.PackageIdentity.Id
                            && c.PackageVersion == test.PackageIdentity.Version.ToNormalizedString()),
                        It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new GetFilesInPackageResponse(MessageResponseCode.Error, files: null));

                var exception = await Assert.ThrowsAsync<PluginException>(
                    () => test.Reader.GetFilesAsync(CancellationToken.None));

                Assert.Equal($"Plugin '{test.PluginName}' failed a GetFilesInPackage operation for package {test.PackageIdentity.Id}.{test.PackageIdentity.Version.ToNormalizedString()}.", exception.Message);
            }
        }

        [Fact]
        public async Task GetFilesAsync_ReturnsFilesOnSuccess()
        {
            var expectedFiles = new[] { "a", "b", "c" };

            using (var test = new PluginPackageReaderTest())
            {
                test.Connection.Setup(x => x.SendRequestAndReceiveResponseAsync<GetFilesInPackageRequest, GetFilesInPackageResponse>(
                        It.Is<MessageMethod>(m => m == MessageMethod.GetFilesInPackage),
                        It.Is<GetFilesInPackageRequest>(c => c.PackageSourceRepository == test.PackageSource.Source
                            && c.PackageId == test.PackageIdentity.Id
                            && c.PackageVersion == test.PackageIdentity.Version.ToNormalizedString()),
                        It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new GetFilesInPackageResponse(MessageResponseCode.Success, expectedFiles));

                var actualFiles = await test.Reader.GetFilesAsync(CancellationToken.None);

                Assert.Equal(expectedFiles, actualFiles);
            }
        }

        [Fact]
        public async Task GetFilesAsync_IsIdempotent()
        {
            var expectedFiles = new[] { "a", "b", "c" };

            using (var test = new PluginPackageReaderTest())
            {
                test.Connection.Setup(x => x.SendRequestAndReceiveResponseAsync<GetFilesInPackageRequest, GetFilesInPackageResponse>(
                        It.Is<MessageMethod>(m => m == MessageMethod.GetFilesInPackage),
                        It.Is<GetFilesInPackageRequest>(c => c.PackageSourceRepository == test.PackageSource.Source
                            && c.PackageId == test.PackageIdentity.Id
                            && c.PackageVersion == test.PackageIdentity.Version.ToNormalizedString()),
                        It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new GetFilesInPackageResponse(MessageResponseCode.Success, expectedFiles));

                var actualFiles = await test.Reader.GetFilesAsync(CancellationToken.None);
                actualFiles = await test.Reader.GetFilesAsync(CancellationToken.None);

                test.Connection.Verify(x => x.SendRequestAndReceiveResponseAsync<GetFilesInPackageRequest, GetFilesInPackageResponse>(
                    It.IsAny<MessageMethod>(),
                    It.IsAny<GetFilesInPackageRequest>(),
                    It.IsAny<CancellationToken>()), Times.Once);
            }
        }

        [Fact]
        public void GetFiles_String_Throws()
        {
            using (var test = new PluginPackageReaderTest())
            {
                Assert.Throws<NotSupportedException>(() => test.Reader.GetFiles(folder: "a"));
            }
        }

        [Fact]
        public async Task GetFilesAsync_String_ThrowsForNullFolder()
        {
            using (var test = new PluginPackageReaderTest())
            {
                var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                    () => test.Reader.GetFilesAsync(folder: null, cancellationToken: CancellationToken.None));

                Assert.Equal("folder", exception.ParamName);
            }
        }

        [Fact]
        public async Task GetFilesAsync_String_ThrowsIfCancelled()
        {
            using (var test = new PluginPackageReaderTest())
            {
                await Assert.ThrowsAsync<OperationCanceledException>(
                    () => test.Reader.GetFilesAsync(
                        folder: "a",
                        cancellationToken: new CancellationToken(canceled: true)));
            }
        }

        [Fact]
        public async Task GetFilesAsync_String_ReturnsFilesInFolder()
        {
            var filesInPackage = new[] { "a", "b/c", "b/d", "e" };
            var expectedFiles = new[] { "b/c", "b/d" };

            using (var test = new PluginPackageReaderTest())
            {
                test.Connection.Setup(x => x.SendRequestAndReceiveResponseAsync<GetFilesInPackageRequest, GetFilesInPackageResponse>(
                        It.Is<MessageMethod>(m => m == MessageMethod.GetFilesInPackage),
                        It.Is<GetFilesInPackageRequest>(c => c.PackageSourceRepository == test.PackageSource.Source
                            && c.PackageId == test.PackageIdentity.Id
                            && c.PackageVersion == test.PackageIdentity.Version.ToNormalizedString()),
                        It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new GetFilesInPackageResponse(MessageResponseCode.Success, filesInPackage));

                var actualFiles = await test.Reader.GetFilesAsync(
                    folder: "b",
                    cancellationToken: CancellationToken.None);

                Assert.Equal(expectedFiles, actualFiles);
            }
        }

        [Fact]
        public void CopyFiles_Throws()
        {
            using (var test = new PluginPackageReaderTest())
            {
                Assert.Throws<NotSupportedException>(
                    () => test.Reader.CopyFiles(
                        destination: "a",
                        packageFiles: Enumerable.Empty<string>(),
                        extractFile: ExtractPackageFile,
                        logger: NullLogger.Instance,
                        token: CancellationToken.None));
            }
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task CopyFilesAsync_ThrowsForNullOrEmptyDestination(string destination)
        {
            using (var test = new PluginPackageReaderTest())
            {
                var exception = await Assert.ThrowsAsync<ArgumentException>(
                    () => test.Reader.CopyFilesAsync(
                        destination,
                        Enumerable.Empty<string>(),
                        ExtractPackageFile,
                        NullLogger.Instance,
                        CancellationToken.None));

                Assert.Equal("destination", exception.ParamName);
            }
        }

        [Fact]
        public async Task CopyFilesAsync_ThrowsForNullPackageFiles()
        {
            using (var test = new PluginPackageReaderTest())
            {
                var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                    () => test.Reader.CopyFilesAsync(
                        destination: "a",
                        packageFiles: null,
                        extractFile: null,
                        logger: NullLogger.Instance,
                        cancellationToken: CancellationToken.None));

                Assert.Equal("packageFiles", exception.ParamName);
            }
        }

        [Fact]
        public async Task CopyFilesAsync_ThrowsForNullLogger()
        {
            using (var test = new PluginPackageReaderTest())
            {
                var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                    () => test.Reader.CopyFilesAsync(
                        destination: "a",
                        packageFiles: Enumerable.Empty<string>(),
                        extractFile: null,
                        logger: null,
                        cancellationToken: CancellationToken.None));

                Assert.Equal("logger", exception.ParamName);
            }
        }

        [Fact]
        public async Task CopyFilesAsync_ThrowsIfCancelled()
        {
            using (var test = new PluginPackageReaderTest())
            {
                await Assert.ThrowsAsync<OperationCanceledException>(
                    () => test.Reader.CopyFilesAsync(
                        destination: "a",
                        packageFiles: Enumerable.Empty<string>(),
                        extractFile: null,
                        logger: NullLogger.Instance,
                        cancellationToken: new CancellationToken(canceled: true)));
            }
        }

        [Fact]
        public async Task CopyFilesAsync_ReturnsEmptyEnumerableOnNotFound()
        {
            const string fileInPackage = "a";

            using (var test = new PluginPackageReaderTest())
            {
                test.Connection.Setup(x => x.SendRequestAndReceiveResponseAsync<CopyFilesInPackageRequest, CopyFilesInPackageResponse>(
                        It.Is<MessageMethod>(m => m == MessageMethod.CopyFilesInPackage),
                        It.Is<CopyFilesInPackageRequest>(c => c.PackageSourceRepository == test.PackageSource.Source
                            && c.PackageId == test.PackageIdentity.Id
                            && c.PackageVersion == test.PackageIdentity.Version.ToNormalizedString()
                            && c.FilesInPackage.Single() == fileInPackage),
                        It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new CopyFilesInPackageResponse(MessageResponseCode.NotFound, copiedFiles: null));

                var copiedFiles = await test.Reader.CopyFilesAsync(
                    destination: "a",
                    packageFiles: new[] { fileInPackage },
                    extractFile: null,
                    logger: NullLogger.Instance,
                    cancellationToken: CancellationToken.None);

                Assert.Empty(copiedFiles);
            }
        }

        [Fact]
        public async Task CopyFilesAsync_ThrowsOnError()
        {
            const string fileInPackage = "a";

            using (var test = new PluginPackageReaderTest())
            {
                test.Connection.Setup(x => x.SendRequestAndReceiveResponseAsync<CopyFilesInPackageRequest, CopyFilesInPackageResponse>(
                        It.Is<MessageMethod>(m => m == MessageMethod.CopyFilesInPackage),
                        It.Is<CopyFilesInPackageRequest>(c => c.PackageSourceRepository == test.PackageSource.Source
                            && c.PackageId == test.PackageIdentity.Id
                            && c.PackageVersion == test.PackageIdentity.Version.ToNormalizedString()
                            && c.FilesInPackage.Single() == fileInPackage),
                        It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new CopyFilesInPackageResponse(MessageResponseCode.Error, copiedFiles: null));

                var exception = await Assert.ThrowsAsync<PluginException>(
                    () => test.Reader.CopyFilesAsync(
                        destination: "a",
                        packageFiles: new[] { fileInPackage },
                        extractFile: null,
                        logger: NullLogger.Instance,
                        cancellationToken: CancellationToken.None));

                Assert.Equal($"Plugin '{test.PluginName}' failed a CopyFilesInPackage operation for package {test.PackageIdentity.Id}.{test.PackageIdentity.Version.ToNormalizedString()}.", exception.Message);
            }
        }

        [Fact]
        public async Task CopyFilesAsync_ReturnsFilesOnSuccess()
        {
            var expectedFiles = new[] { "a", "b", "c" };

            using (var testDirectory = TestDirectory.Create())
            using (var test = new PluginPackageReaderTest())
            {
                CopyFilesInPackageResponse response = null;

                test.Connection.Setup(x => x.SendRequestAndReceiveResponseAsync<CopyFilesInPackageRequest, CopyFilesInPackageResponse>(
                        It.Is<MessageMethod>(m => m == MessageMethod.CopyFilesInPackage),
                        It.Is<CopyFilesInPackageRequest>(c => c.PackageSourceRepository == test.PackageSource.Source
                            && c.PackageId == test.PackageIdentity.Id
                            && c.PackageVersion == test.PackageIdentity.Version.ToNormalizedString()
                            && c.FilesInPackage.Count() == expectedFiles.Length),
                        It.IsAny<CancellationToken>()))
                    .Callback<MessageMethod, CopyFilesInPackageRequest, CancellationToken>(
                        (method, request, cancellationToken) =>
                        {
                            var copiedFiles = new List<string>();

                            foreach (var fileInPackage in request.FilesInPackage)
                            {
                                var filePath = Path.Combine(request.DestinationFolderPath, fileInPackage);

                                File.WriteAllText(filePath, fileInPackage);

                                copiedFiles.Add(filePath);
                            }

                            response = new CopyFilesInPackageResponse(MessageResponseCode.Success, copiedFiles);
                        }
                    )
                    .ReturnsAsync(() => response);

                var actualCopiedFilePaths = await test.Reader.CopyFilesAsync(
                    testDirectory.Path,
                    expectedFiles,
                    extractFile: null,
                    logger: NullLogger.Instance,
                    cancellationToken: CancellationToken.None);

                var expectedCopiedFilePaths = expectedFiles.Select(f => Path.Combine(testDirectory.Path, f));

                Assert.Equal(expectedCopiedFilePaths, actualCopiedFilePaths);
            }
        }

        [Fact]
        public async Task CopyFilesAsync_UnsafeEnty_Fail()
        {
            var expectedFiles = new[] { "../a", "b", "c" };

            using (var testDirectory = TestDirectory.Create())
            using (var test = new PluginPackageReaderTest())
            {
                await Assert.ThrowsAsync<UnsafePackageEntryException>(() => test.Reader.CopyFilesAsync(
                   testDirectory.Path,
                   expectedFiles,
                   extractFile: null,
                   logger: NullLogger.Instance,
                   cancellationToken: CancellationToken.None));
            }
        }

        [Fact]
        public async Task CopyFilesAsync_UnsafeRootEnty_Fail()
        {
            var rootPath = RuntimeEnvironmentHelper.IsWindows ? @"C:" : @"/";
            var expectedFiles = new[] { $"{rootPath}/a", "b", "c" };

            using (var testDirectory = TestDirectory.Create())
            using (var test = new PluginPackageReaderTest())
            {
                await Assert.ThrowsAsync<UnsafePackageEntryException>(() => test.Reader.CopyFilesAsync(
                   testDirectory.Path,
                   expectedFiles,
                   extractFile: null,
                   logger: NullLogger.Instance,
                   cancellationToken: CancellationToken.None));
            }
        }

        [Fact]
        public async Task CopyFilesAsync_UnsafeCurrentEnty_Fail()
        {
            var expectedFiles = new[] { ".", "b", "c" };

            using (var testDirectory = TestDirectory.Create())
            using (var test = new PluginPackageReaderTest())
            {
                await Assert.ThrowsAsync<UnsafePackageEntryException>(() => test.Reader.CopyFilesAsync(
                   testDirectory.Path,
                   expectedFiles,
                   extractFile: null,
                   logger: NullLogger.Instance,
                   cancellationToken: CancellationToken.None));
            }
        }

        [Fact]
        public void GetIdentity_Throws()
        {
            using (var test = new PluginPackageReaderTest())
            {
                Assert.Throws<NotSupportedException>(() => test.Reader.GetIdentity());
            }
        }

        [Fact]
        public async Task GetIdentityAsync_ThrowsIfCancelled()
        {
            using (var test = new PluginPackageReaderTest())
            {
                await Assert.ThrowsAsync<OperationCanceledException>(
                    () => test.Reader.GetIdentityAsync(new CancellationToken(canceled: true)));
            }
        }

        [Fact]
        public async Task GetIdentityAsync_ReturnsIdentity()
        {
            using (var test = new NuspecPluginPackageReaderTest())
            {
                var packageIdentity = await test.Reader.GetIdentityAsync(CancellationToken.None);

                Assert.Equal(test.PackageIdentity, packageIdentity);
            }
        }

        [Fact]
        public void GetMinClientVersion_Throws()
        {
            using (var test = new PluginPackageReaderTest())
            {
                Assert.Throws<NotSupportedException>(() => test.Reader.GetMinClientVersion());
            }
        }

        [Fact]
        public async Task GetMinClientVersionAsync_ThrowsIfCancelled()
        {
            using (var test = new PluginPackageReaderTest())
            {
                await Assert.ThrowsAsync<OperationCanceledException>(
                    () => test.Reader.GetMinClientVersionAsync(new CancellationToken(canceled: true)));
            }
        }

        [Fact]
        public async Task GetMinClientVersionAsync_ReturnsMinClientVersion()
        {
            using (var test = new NuspecPluginPackageReaderTest())
            {
                var minClientVersion = await test.Reader.GetMinClientVersionAsync(CancellationToken.None);

                Assert.Equal(NuGetVersion.Parse("1.2.3"), minClientVersion);
            }
        }

        [Fact]
        public void GetPackageTypes_Throws()
        {
            using (var test = new PluginPackageReaderTest())
            {
                Assert.Throws<NotSupportedException>(() => test.Reader.GetPackageTypes());
            }
        }

        [Fact]
        public async Task GetPackageTypesAsync_ThrowsIfCancelled()
        {
            using (var test = new PluginPackageReaderTest())
            {
                await Assert.ThrowsAsync<OperationCanceledException>(
                    () => test.Reader.GetPackageTypesAsync(new CancellationToken(canceled: true)));
            }
        }

        [Fact]
        public async Task GetPackageTypesAsync_ReturnsPackageTypes()
        {
            using (var test = new NuspecPluginPackageReaderTest())
            {
                var packageTypes = await test.Reader.GetPackageTypesAsync(CancellationToken.None);

                Assert.NotNull(packageTypes);
                Assert.Equal(1, packageTypes.Count);
                Assert.Equal("f", packageTypes[0].Name);
                Assert.Equal("4.5.6", packageTypes[0].Version.ToString());
            }
        }

        [Fact]
        public void GetNuspec_Throws()
        {
            using (var test = new PluginPackageReaderTest())
            {
                Assert.Throws<NotSupportedException>(() => test.Reader.GetNuspec());
            }
        }

        [Fact]
        public async Task GetNuspecAsync_ThrowsIfCancelled()
        {
            using (var test = new PluginPackageReaderTest())
            {
                await Assert.ThrowsAsync<OperationCanceledException>(
                    () => test.Reader.GetNuspecAsync(new CancellationToken(canceled: true)));
            }
        }

        [Fact]
        public async Task GetNuspecAsync_ReturnsNuspec()
        {
            using (var test = new NuspecPluginPackageReaderTest())
            using (var stream = await test.Reader.GetNuspecAsync(CancellationToken.None))
            using (var streamReader = new StreamReader(stream))
            {
                var content = streamReader.ReadToEnd();

                var expectedResult = XDocument.Parse(test.NuspecFileInPackage.Content).ToString();
                var actualResult = XDocument.Parse(content).ToString();

                Assert.Equal(expectedResult, actualResult);
            }
        }

        [Fact]
        public void GetNuspecFile_Throws()
        {
            using (var test = new PluginPackageReaderTest())
            {
                Assert.Throws<NotSupportedException>(() => test.Reader.GetNuspecFile());
            }
        }

        [Fact]
        public async Task GetNuspecFileAsync_ThrowsIfCancelled()
        {
            using (var test = new PluginPackageReaderTest())
            {
                await Assert.ThrowsAsync<OperationCanceledException>(
                    () => test.Reader.GetNuspecFileAsync(new CancellationToken(canceled: true)));
            }
        }

        [Fact]
        public async Task GetNuspecFileAsync_ReturnsNuspecPathInPackage()
        {
            using (var test = new NuspecPluginPackageReaderTest())
            {
                var nuspecPathInPackage = await test.Reader.GetNuspecFileAsync(CancellationToken.None);

                Assert.Equal(test.NuspecFileInPackage.Path, nuspecPathInPackage);
            }
        }

        [Fact]
        public void NuspecReader_Throws()
        {
            using (var test = new PluginPackageReaderTest())
            {
                Assert.Throws<NotSupportedException>(() => test.Reader.NuspecReader);
            }
        }

        [Fact]
        public async Task GetNuspecReaderAsync_ThrowsIfCancelled()
        {
            using (var test = new PluginPackageReaderTest())
            {
                await Assert.ThrowsAsync<OperationCanceledException>(
                    () => test.Reader.GetNuspecReaderAsync(new CancellationToken(canceled: true)));
            }
        }

        [Fact]
        public async Task GetNuspecReaderAsync_ReturnsNuspecPathInPackage()
        {
            using (var test = new NuspecPluginPackageReaderTest())
            {
                var reader = await test.Reader.GetNuspecReaderAsync(CancellationToken.None);

                Assert.NotNull(reader);
            }
        }

        [Fact]
        public void GetSupportedFrameworks_Throws()
        {
            using (var test = new PluginPackageReaderTest())
            {
                Assert.Throws<NotSupportedException>(() => test.Reader.GetSupportedFrameworks());
            }
        }

        [Fact]
        public async Task GetSupportedFrameworksAsync_ThrowsIfCancelled()
        {
            using (var test = new PluginPackageReaderTest())
            {
                await Assert.ThrowsAsync<OperationCanceledException>(
                    () => test.Reader.GetSupportedFrameworksAsync(new CancellationToken(canceled: true)));
            }
        }

        [Fact]
        public async Task GetSupportedFrameworksAsync_ReturnsSupportedFrameworks()
        {
            using (var test = new NuspecPluginPackageReaderTest(addFiles: true))
            {
                var frameworks = await test.Reader.GetSupportedFrameworksAsync(CancellationToken.None);

                Assert.NotEmpty(frameworks);
                Assert.Equal(new[]
                    {
                        ".NETFramework,Version=v4.0",
                        ".NETFramework,Version=v4.5",
                        "Silverlight,Version=v3.0",
                    }, frameworks.Select(f => f.DotNetFrameworkName));
            }
        }

        [Fact]
        public void GetFrameworkItems_Throws()
        {
            using (var test = new PluginPackageReaderTest())
            {
                Assert.Throws<NotSupportedException>(() => test.Reader.GetFrameworkItems());
            }
        }

        [Fact]
        public async Task GetFrameworkItemsAsync_ThrowsIfCancelled()
        {
            using (var test = new PluginPackageReaderTest())
            {
                await Assert.ThrowsAsync<OperationCanceledException>(
                    () => test.Reader.GetSupportedFrameworksAsync(new CancellationToken(canceled: true)));
            }
        }

        [Fact]
        public async Task GetFrameworkItemsAsync_ReturnsFrameworkItems()
        {
            using (var test = new NuspecPluginPackageReaderTest())
            {
                var groups = await test.Reader.GetFrameworkItemsAsync(CancellationToken.None);

                Assert.NotEmpty(groups);
                Assert.Equal(new[]
                    {
                        ".NETFramework,Version=v4.0",
                        "Silverlight,Version=v3.0"
                    }, groups.Select(g => g.TargetFramework.DotNetFrameworkName));
            }
        }

        [Fact]
        public void IsServiceable_Throws()
        {
            using (var test = new PluginPackageReaderTest())
            {
                Assert.Throws<NotSupportedException>(() => test.Reader.IsServiceable());
            }
        }

        [Fact]
        public async Task IsServiceableAsync_ThrowsIfCancelled()
        {
            using (var test = new PluginPackageReaderTest())
            {
                await Assert.ThrowsAsync<OperationCanceledException>(
                    () => test.Reader.IsServiceableAsync(new CancellationToken(canceled: true)));
            }
        }

        [Fact]
        public async Task IsServiceableAsync_ReturnsIsServiceable()
        {
            using (var test = new NuspecPluginPackageReaderTest())
            {
                var isServiceable = await test.Reader.IsServiceableAsync(CancellationToken.None);

                Assert.True(isServiceable);
            }
        }

        [Fact]
        public void GetBuildItems_Throws()
        {
            using (var test = new PluginPackageReaderTest())
            {
                Assert.Throws<NotSupportedException>(() => test.Reader.GetBuildItems());
            }
        }

        [Fact]
        public async Task GetBuildItemsAsync_ThrowsIfCancelled()
        {
            using (var test = new PluginPackageReaderTest())
            {
                await Assert.ThrowsAsync<OperationCanceledException>(
                    () => test.Reader.GetBuildItemsAsync(new CancellationToken(canceled: true)));
            }
        }

        [Fact]
        public async Task GetBuildItemsAsync_ReturnsBuildItems()
        {
            using (var test = new NuspecPluginPackageReaderTest(addFiles: true))
            {
                var groups = await test.Reader.GetBuildItemsAsync(CancellationToken.None);

                Assert.NotEmpty(groups);
                Assert.Equal(new[]
                    {
                        ".NETFramework,Version=v4.5"
                    }, groups.Select(g => g.TargetFramework.DotNetFrameworkName));
                Assert.Equal(new[]
                    {
                        "build/net45/a.props",
                        "build/net45/a.targets"
                    }, groups.Single().Items);
            }
        }

        [Fact]
        public void GetToolItems_Throws()
        {
            using (var test = new PluginPackageReaderTest())
            {
                Assert.Throws<NotSupportedException>(() => test.Reader.GetToolItems());
            }
        }

        [Fact]
        public async Task GetToolItemsAsync_ThrowsIfCancelled()
        {
            using (var test = new PluginPackageReaderTest())
            {
                await Assert.ThrowsAsync<OperationCanceledException>(
                    () => test.Reader.GetToolItemsAsync(new CancellationToken(canceled: true)));
            }
        }

        [Fact]
        public async Task GetToolItemsAsync_ReturnsToolItems()
        {
            using (var test = new NuspecPluginPackageReaderTest(addFiles: true))
            {
                var groups = await test.Reader.GetToolItemsAsync(CancellationToken.None);

                Assert.NotEmpty(groups);
                Assert.Equal(new[]
                    {
                        ".NETFramework,Version=v4.5"
                    }, groups.Select(g => g.TargetFramework.DotNetFrameworkName));
                Assert.Equal(new[]
                    {
                        "tools/net45/j",
                        "tools/net45/k"
                    }, groups.Single().Items);
            }
        }

        [Fact]
        public void GetContentItems_Throws()
        {
            using (var test = new PluginPackageReaderTest())
            {
                Assert.Throws<NotSupportedException>(() => test.Reader.GetContentItems());
            }
        }

        [Fact]
        public async Task GetContentItemsAsync_ThrowsIfCancelled()
        {
            using (var test = new PluginPackageReaderTest())
            {
                await Assert.ThrowsAsync<OperationCanceledException>(
                    () => test.Reader.GetContentItemsAsync(new CancellationToken(canceled: true)));
            }
        }

        [Fact]
        public async Task GetContentItemsAsync_ReturnsContentItems()
        {
            using (var test = new NuspecPluginPackageReaderTest(addFiles: true))
            {
                var groups = await test.Reader.GetContentItemsAsync(CancellationToken.None);

                Assert.NotEmpty(groups);
                Assert.Equal(new[]
                    {
                        ".NETFramework,Version=v4.5"
                    }, groups.Select(g => g.TargetFramework.DotNetFrameworkName));
                Assert.Equal(new[]
                    {
                        "content/net45/b",
                        "content/net45/c"
                    }, groups.Single().Items);
            }
        }

        [Fact]
        public void GetItems_Throws()
        {
            using (var test = new PluginPackageReaderTest())
            {
                Assert.Throws<NotSupportedException>(() => test.Reader.GetItems(folderName: "a"));
            }
        }

        [Fact]
        public async Task GetItemsAsync_ThrowsForNullFolderName()
        {
            using (var test = new PluginPackageReaderTest())
            {
                var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                    () => test.Reader.GetItemsAsync(
                        folderName: null,
                        cancellationToken: CancellationToken.None));

                Assert.Equal("folderName", exception.ParamName);
            }
        }

        [Fact]
        public async Task GetItemsAsync_ThrowsIfCancelled()
        {
            using (var test = new PluginPackageReaderTest())
            {
                await Assert.ThrowsAsync<OperationCanceledException>(
                    () => test.Reader.GetItemsAsync(
                        folderName: "a",
                        cancellationToken: new CancellationToken(canceled: true)));
            }
        }

        [Fact]
        public async Task GetItemsAsync_ReturnsItems()
        {
            using (var test = new NuspecPluginPackageReaderTest(addFiles: true))
            {
                var groups = await test.Reader.GetItemsAsync(
                    folderName: "lib",
                    cancellationToken: CancellationToken.None);

                Assert.NotEmpty(groups);
                Assert.Equal(new[]
                    {
                        ".NETFramework,Version=v4.5"
                    }, groups.Select(g => g.TargetFramework.DotNetFrameworkName));
                Assert.Equal(new[]
                    {
                        "lib/net45/d",
                        "lib/net45/e",
                        "lib/net45/f.dll",
                        "lib/net45/g.dll"
                    }, groups.Single().Items);
            }
        }

        [Fact]
        public void GetPackageDependencies_Throws()
        {
            using (var test = new PluginPackageReaderTest())
            {
                Assert.Throws<NotSupportedException>(() => test.Reader.GetPackageDependencies());
            }
        }

        [Fact]
        public async Task GetPackageDependenciesAsync_ThrowsIfCancelled()
        {
            using (var test = new PluginPackageReaderTest())
            {
                await Assert.ThrowsAsync<OperationCanceledException>(
                    () => test.Reader.GetPackageDependenciesAsync(new CancellationToken(canceled: true)));
            }
        }

        [Fact]
        public async Task GetPackageDependenciesAsync_ReturnsPackageDependencies()
        {
            using (var test = new NuspecPluginPackageReaderTest(addFiles: true))
            {
                var groups = await test.Reader.GetPackageDependenciesAsync(CancellationToken.None);

                Assert.NotEmpty(groups);
                Assert.Equal(new[]
                    {
                        ".NETFramework,Version=v4.0",
                        ".NETFramework,Version=v4.5"
                    }, groups.Select(g => g.TargetFramework.DotNetFrameworkName));
                Assert.Equal(new[] { "b", "c" }, groups.First().Packages.Select(p => p.Id));
            }
        }

        [Fact]
        public void GetLibItems_Throws()
        {
            using (var test = new PluginPackageReaderTest())
            {
                Assert.Throws<NotSupportedException>(() => test.Reader.GetLibItems());
            }
        }

        [Fact]
        public async Task GetLibItemsAsync_ThrowsIfCancelled()
        {
            using (var test = new PluginPackageReaderTest())
            {
                await Assert.ThrowsAsync<OperationCanceledException>(
                    () => test.Reader.GetLibItemsAsync(new CancellationToken(canceled: true)));
            }
        }

        [Fact]
        public async Task GetLibItemsAsync_ReturnsLibItems()
        {
            using (var test = new NuspecPluginPackageReaderTest(addFiles: true))
            {
                var groups = await test.Reader.GetLibItemsAsync(CancellationToken.None);

                Assert.NotEmpty(groups);
                Assert.Equal(
                    new[] { ".NETFramework,Version=v4.5" },
                    groups.Select(g => g.TargetFramework.DotNetFrameworkName));
                Assert.Equal(new[]
                    {
                        "lib/net45/d",
                        "lib/net45/e",
                        "lib/net45/f.dll",
                        "lib/net45/g.dll"
                    }, groups.First().Items);
            }
        }

        [Fact]
        public void GetReferenceItems_Throws()
        {
            using (var test = new PluginPackageReaderTest())
            {
                Assert.Throws<NotSupportedException>(() => test.Reader.GetReferenceItems());
            }
        }

        [Fact]
        public async Task GetReferenceItemsAsync_ThrowsIfCancelled()
        {
            using (var test = new PluginPackageReaderTest())
            {
                await Assert.ThrowsAsync<OperationCanceledException>(
                    () => test.Reader.GetReferenceItemsAsync(new CancellationToken(canceled: true)));
            }
        }

        [Fact]
        public async Task GetReferenceItemsAsync_ReturnsReferenceItems()
        {
            using (var test = new NuspecPluginPackageReaderTest(addFiles: true))
            {
                var groups = await test.Reader.GetReferenceItemsAsync(CancellationToken.None);

                Assert.NotEmpty(groups);
                Assert.Equal(
                    new[] { ".NETFramework,Version=v4.5" },
                    groups.Select(g => g.TargetFramework.DotNetFrameworkName));
                Assert.Equal(new[]
                    {
                        "lib/net45/g.dll"
                    }, groups.First().Items);
            }
        }

        [Fact]
        public void GetDevelopmentDependency_Throws()
        {
            using (var test = new PluginPackageReaderTest())
            {
                Assert.Throws<NotSupportedException>(() => test.Reader.GetDevelopmentDependency());
            }
        }

        [Fact]
        public async Task GetDevelopmentDependencyAsync_ThrowsIfCancelled()
        {
            using (var test = new PluginPackageReaderTest())
            {
                await Assert.ThrowsAsync<OperationCanceledException>(
                    () => test.Reader.GetDevelopmentDependencyAsync(new CancellationToken(canceled: true)));
            }
        }

        [Fact]
        public async Task GetDevelopmentDependencyAsync_ReturnsDevelopmentDependency()
        {
            using (var test = new NuspecPluginPackageReaderTest())
            {
                var isDevelopmentDependency = await test.Reader.GetDevelopmentDependencyAsync(CancellationToken.None);

                Assert.True(isDevelopmentDependency);
            }
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task CopyNupkgAsync_ThrowsForNullOrEmptyNupkgFilePath(string nupkgFilePath)
        {
            using (var test = new PluginPackageReaderTest())
            {
                var exception = await Assert.ThrowsAsync<ArgumentException>(
                    () => test.Reader.CopyNupkgAsync(nupkgFilePath, CancellationToken.None));

                Assert.Equal("nupkgFilePath", exception.ParamName);
            }
        }

        [Fact]
        public async Task CopyNupkgAsync_ThrowsIfCancelled()
        {
            using (var test = new PluginPackageReaderTest())
            {
                await Assert.ThrowsAsync<OperationCanceledException>(
                    () => test.Reader.CopyNupkgAsync(
                        nupkgFilePath: "a",
                        cancellationToken: new CancellationToken(canceled: true)));
            }
        }

        [Fact]
        public async Task CopyNupkgAsync_ReturnsNullAndCreatesPackageDownloadMarkerFileOnNotFound()
        {
            using (var testDirectory = TestDirectory.Create())
            using (var test = new PluginPackageReaderTest())
            {
                var pathResolver = new VersionFolderPathResolver(testDirectory.Path);
                var nupkgFilePath = Path.Combine(
                    testDirectory.Path,
                    $"{test.PackageIdentity.Id}.${test.PackageIdentity.Version.ToNormalizedString()}.nupkg");
                var markerFilePath = Path.Combine(
                    testDirectory.Path,
                    pathResolver.GetPackageDownloadMarkerFileName(test.PackageIdentity.Id));

                test.Connection.Setup(x => x.SendRequestAndReceiveResponseAsync<CopyNupkgFileRequest, CopyNupkgFileResponse>(
                        It.Is<MessageMethod>(m => m == MessageMethod.CopyNupkgFile),
                        It.Is<CopyNupkgFileRequest>(c => c.PackageSourceRepository == test.PackageSource.Source
                            && c.PackageId == test.PackageIdentity.Id
                            && c.PackageVersion == test.PackageIdentity.Version.ToNormalizedString()),
                        It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new CopyNupkgFileResponse(MessageResponseCode.NotFound));

                var actualFilePath = await test.Reader.CopyNupkgAsync(nupkgFilePath, CancellationToken.None);

                Assert.Null(actualFilePath);
                Assert.False(File.Exists(nupkgFilePath));
                Assert.True(File.Exists(markerFilePath));
            }
        }

        [Fact]
        public async Task CopyNupkgAsync_ReturnsNullOnError()
        {
            using (var testDirectory = TestDirectory.Create())
            using (var test = new PluginPackageReaderTest())
            {
                var pathResolver = new VersionFolderPathResolver(testDirectory.Path);
                var nupkgFilePath = Path.Combine(
                    testDirectory.Path,
                    $"{test.PackageIdentity.Id}.${test.PackageIdentity.Version.ToNormalizedString()}.nupkg");
                var markerFilePath = Path.Combine(
                    testDirectory.Path,
                    pathResolver.GetPackageDownloadMarkerFileName(test.PackageIdentity.Id));

                test.Connection.Setup(x => x.SendRequestAndReceiveResponseAsync<CopyNupkgFileRequest, CopyNupkgFileResponse>(
                        It.Is<MessageMethod>(m => m == MessageMethod.CopyNupkgFile),
                        It.Is<CopyNupkgFileRequest>(c => c.PackageSourceRepository == test.PackageSource.Source
                            && c.PackageId == test.PackageIdentity.Id
                            && c.PackageVersion == test.PackageIdentity.Version.ToNormalizedString()),
                        It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new CopyNupkgFileResponse(MessageResponseCode.Error));

                var actualFilePath = await test.Reader.CopyNupkgAsync(nupkgFilePath, CancellationToken.None);

                Assert.Null(actualFilePath);
                Assert.False(File.Exists(nupkgFilePath));
                Assert.False(File.Exists(markerFilePath));
            }
        }

        [Fact]
        public async Task CopyNupkgAsync_CopiesNupkgOnSuccess()
        {
            using (var testDirectory = TestDirectory.Create())
            using (var test = new PluginPackageReaderTest())
            {
                var pathResolver = new VersionFolderPathResolver(testDirectory.Path);
                var nupkgFilePath = Path.Combine(
                    testDirectory.Path,
                    $"{test.PackageIdentity.Id}.${test.PackageIdentity.Version.ToNormalizedString()}.nupkg");
                var markerFilePath = Path.Combine(
                    testDirectory.Path,
                    pathResolver.GetPackageDownloadMarkerFileName(test.PackageIdentity.Id));

                test.Connection.Setup(x => x.SendRequestAndReceiveResponseAsync<CopyNupkgFileRequest, CopyNupkgFileResponse>(
                        It.Is<MessageMethod>(m => m == MessageMethod.CopyNupkgFile),
                        It.Is<CopyNupkgFileRequest>(c => c.PackageSourceRepository == test.PackageSource.Source
                            && c.PackageId == test.PackageIdentity.Id
                            && c.PackageVersion == test.PackageIdentity.Version.ToNormalizedString()),
                        It.IsAny<CancellationToken>()))
                    .Callback<MessageMethod, CopyNupkgFileRequest, CancellationToken>(
                        (method, request, cancellationToken) =>
                        {
                            File.WriteAllText(request.DestinationFilePath, string.Empty);
                        })
                    .ReturnsAsync(new CopyNupkgFileResponse(MessageResponseCode.Success));

                var actualFilePath = await test.Reader.CopyNupkgAsync(nupkgFilePath, CancellationToken.None);

                Assert.Equal(nupkgFilePath, actualFilePath);
                Assert.True(File.Exists(actualFilePath));
                Assert.False(File.Exists(markerFilePath));
            }
        }

        private string ExtractPackageFile(string sourceFile, string targetPath, Stream fileStream)
        {
            throw new NotImplementedException();
        }

        private class PluginPackageReaderTest : IDisposable
        {
            internal Mock<IConnection> Connection { get; }
            internal PackageIdentity PackageIdentity { get; }
            internal PackageSource PackageSource { get; }
            internal Mock<IPlugin> Plugin { get; }
            internal string PluginName { get; }
            internal PluginPackageReader Reader { get; }

            internal PluginPackageReaderTest()
            {
                Connection = new Mock<IConnection>(MockBehavior.Strict);

                PackageIdentity = new PackageIdentity(id: "a", version: NuGetVersion.Parse("1.0.0"));
                PackageSource = new PackageSource("https://unit.test");
                PluginName = "b";
                Plugin = new Mock<IPlugin>(MockBehavior.Strict);

                Plugin.Setup(x => x.Dispose());
                Plugin.SetupGet(x => x.Name)
                    .Returns(PluginName);
                Plugin.SetupGet(x => x.Connection)
                    .Returns(Connection.Object);

                Reader = new PluginPackageReader(Plugin.Object, PackageIdentity, PackageSource.Source);
            }

            public void Dispose()
            {
                Dispose(disposing: true);
                GC.SuppressFinalize(this);

                Connection.Verify();
                Plugin.Verify();
            }

            protected virtual void Dispose(bool disposing)
            {
                Reader.Dispose();
            }
        }

        private sealed class NuspecPluginPackageReaderTest : PluginPackageReaderTest
        {
            private static readonly IEnumerable<FileInPackage> _additionalFilesInPackage;

            internal FileInPackage NuspecFileInPackage { get; }

            static NuspecPluginPackageReaderTest()
            {
                _additionalFilesInPackage = new FileInPackage[]
                {
                    new FileInPackage(path: "build/net45/a.dll", content: string.Empty),
                    new FileInPackage(path: "build/net45/a.props", content: string.Empty),
                    new FileInPackage(path: "build/net45/a.targets", content: string.Empty),
                    new FileInPackage(path: "content/net45/b", content: string.Empty),
                    new FileInPackage(path: "content/net45/c", content: string.Empty),
                    new FileInPackage(path: "lib/net45/d", content: string.Empty),
                    new FileInPackage(path: "lib/net45/e", content: string.Empty),
                    new FileInPackage(path: "lib/net45/f.dll", content: string.Empty),
                    new FileInPackage(path: "lib/net45/g.dll", content: string.Empty),
                    new FileInPackage(path: "other/net45/h", content: string.Empty),
                    new FileInPackage(path: "other/net45/i", content: string.Empty),
                    new FileInPackage(path: "tools/net45/j", content: string.Empty),
                    new FileInPackage(path: "tools/net45/k", content: string.Empty)
                };
            }

            internal NuspecPluginPackageReaderTest(bool addFiles = false)
                : base()
            {
                var additionalFilesInPackage = addFiles ? _additionalFilesInPackage : Enumerable.Empty<FileInPackage>();

                NuspecFileInPackage = new FileInPackage(
                    path: $"{PackageIdentity.Id}.nuspec",
                    content: $@"<?xml version=""1.0"" encoding=""utf-8""?>
                                <package>
                                    <metadata minClientVersion=""1.2.3"">
                                        <id>{PackageIdentity.Id}</id>
                                        <version>{PackageIdentity.Version.ToNormalizedString()}</version>
                                        <title />
                                        <dependencies>
                                            <group targetFramework=""net40"">
                                                <dependency id=""b"" />
                                                <dependency id=""c"" />
                                            </group>
                                            <group targetFramework=""net45"" />
                                        </dependencies>
                                        <developmentDependency>true</developmentDependency>
                                        <frameworkAssemblies>
                                            <frameworkAssembly assemblyName=""d"" targetFramework=""net40"" />
                                            <frameworkAssembly assemblyName=""e"" targetFramework=""sl3"" />
                                        </frameworkAssemblies>
                                        <packageTypes>
                                            <packageType name=""f"" version=""4.5.6"" />
                                        </packageTypes>
                                        <references>
                                            <group targetFramework=""net45"">
                                              <reference file=""g.dll"" />
                                              <reference file=""h.dll"" />
                                            </group>
                                        </references>
                                        <serviceable>true</serviceable>
                                    </metadata>
                                </package>");
                var filesInPackage = new[] { NuspecFileInPackage }.Concat(additionalFilesInPackage);

                Connection.Setup(x => x.SendRequestAndReceiveResponseAsync<GetFilesInPackageRequest, GetFilesInPackageResponse>(
                        It.Is<MessageMethod>(m => m == MessageMethod.GetFilesInPackage),
                        It.Is<GetFilesInPackageRequest>(c => c.PackageSourceRepository == PackageSource.Source
                            && c.PackageId == PackageIdentity.Id
                            && c.PackageVersion == PackageIdentity.Version.ToNormalizedString()),
                        It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new GetFilesInPackageResponse(MessageResponseCode.Success, filesInPackage.Select(f => f.Path)));

                CopyFilesInPackageResponse response = null;

                Connection.Setup(x => x.SendRequestAndReceiveResponseAsync<CopyFilesInPackageRequest, CopyFilesInPackageResponse>(
                        It.Is<MessageMethod>(m => m == MessageMethod.CopyFilesInPackage),
                        It.Is<CopyFilesInPackageRequest>(c => c.PackageSourceRepository == PackageSource.Source
                            && c.PackageId == PackageIdentity.Id
                            && c.PackageVersion == PackageIdentity.Version.ToNormalizedString()
                            && c.FilesInPackage.Count() == 1),
                        It.IsAny<CancellationToken>()))
                    .Callback<MessageMethod, CopyFilesInPackageRequest, CancellationToken>(
                        (method, request, cancellationToken) =>
                        {
                            var copiedFiles = new List<string>();

                            foreach (var fileInPackage in request.FilesInPackage)
                            {
                                var filePath = Path.Combine(request.DestinationFolderPath, fileInPackage);
                                var content = filesInPackage.Single(f => f.Path == fileInPackage).Content;

                                File.WriteAllText(filePath, content);

                                copiedFiles.Add(filePath);
                            }

                            response = new CopyFilesInPackageResponse(MessageResponseCode.Success, copiedFiles);
                        }
                    )
                    .ReturnsAsync(() => response);
            }
        }

        private sealed class FileInPackage
        {
            internal string Content { get; }
            internal string Path { get; }

            internal FileInPackage(string path, string content)
            {
                Path = path;
                Content = content;
            }
        }
    }
}
