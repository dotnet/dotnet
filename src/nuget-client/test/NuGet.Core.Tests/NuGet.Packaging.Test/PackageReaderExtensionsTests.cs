// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NuGet.Frameworks;
using NuGet.Packaging.Core;
using Xunit;

namespace NuGet.Packaging.Test
{
    public class PackageReaderExtensionsTests
    {
        private static readonly Mock<IAsyncPackageContentReader> _contentReader;
        private static readonly Mock<IAsyncPackageCoreReader> _coreReader;

        static PackageReaderExtensionsTests()
        {
            _contentReader = new Mock<IAsyncPackageContentReader>(MockBehavior.Strict);
            _coreReader = new Mock<IAsyncPackageCoreReader>(MockBehavior.Strict);

            _contentReader.Setup(x => x.GetLibItemsAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<IEnumerable<FrameworkSpecificGroup>>(new FrameworkSpecificGroup[]
                {
                    new FrameworkSpecificGroup(new NuGetFramework("net45"), new []
                    {
                        "lib/net45/fr/a.resources.dll"
                    })
                }));

            _coreReader.Setup(x => x.GetFilesAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<IEnumerable<string>>(new string[]
                {
                    null,
                    "",
                    "_rels/",
                    @"_rels\",
                    "package/",
                    @"package\",
                    "[Content_Types].xml",
                    "a.1.0.0.nupkg.sha512",
                    "a.nuspec",
                    "b",
                    "/c/a.nuspec",
                    @"\c/a.nuspec",
                    "/c/d",
                    @"\c\d",
                }));
        }

        [Fact]
        public async Task GetPackageFilesAsync_None_ReturnsEmptyEnumerable()
        {
            var packageFiles = await PackageReaderExtensions.GetPackageFilesAsync(
                _coreReader.Object,
                PackageSaveMode.None,
                CancellationToken.None);

            Assert.Empty(packageFiles);
        }

        [Fact]
        public async Task GetPackageFilesAsync_Nuspec_ReturnsNuspec()
        {
            var packageFiles = (await PackageReaderExtensions.GetPackageFilesAsync(
                _coreReader.Object,
                PackageSaveMode.Nuspec,
                CancellationToken.None)).ToArray();

            Assert.Equal(1, packageFiles.Length);
            Assert.Equal("a.nuspec", packageFiles[0]);
        }

        [Theory]
        [InlineData(PackageSaveMode.Files)]
        [InlineData(PackageSaveMode.Defaultv2)]
        public async Task GetPackageFilesAsync_ReturnsFiles(PackageSaveMode packageSaveMode)
        {
            var packageFiles = (await PackageReaderExtensions.GetPackageFilesAsync(
                _coreReader.Object,
                packageSaveMode,
                CancellationToken.None)).ToArray();

            Assert.Equal(new[]
                {
                    "b",
                    "/c/a.nuspec",
                    @"\c/a.nuspec",
                    "/c/d",
                    @"\c\d"
                }, packageFiles);
        }

        [Fact]
        public async Task GetPackageFilesAsync_NuspecFiles_ReturnsNuspecAndFiles()
        {
            var packageFiles = (await PackageReaderExtensions.GetPackageFilesAsync(
                _coreReader.Object,
                PackageSaveMode.Nuspec | PackageSaveMode.Files,
                CancellationToken.None)).ToArray();

            Assert.Equal(new[]
                {
                    "a.nuspec",
                    "b",
                    "/c/a.nuspec",
                    @"\c/a.nuspec",
                    "/c/d",
                    @"\c\d"
                }, packageFiles);
        }

        [Theory]
        [InlineData(PackageSaveMode.Nuspec | PackageSaveMode.Files)]
        [InlineData(PackageSaveMode.Defaultv3)]
        public async Task GetPackageFilesAsync_ReturnsNuspecAndFiles(PackageSaveMode packageSaveMode)
        {
            var packageFiles = (await PackageReaderExtensions.GetPackageFilesAsync(
                _coreReader.Object,
                packageSaveMode,
                CancellationToken.None)).ToArray();

            Assert.Equal(new[]
                {
                    "a.nuspec",
                    "b",
                    "/c/a.nuspec",
                    @"\c/a.nuspec",
                    "/c/d",
                    @"\c\d"
                }, packageFiles);
        }

        [Fact]
        public async Task GetSatelliteFilesAsync_ReturnsEmptyEnumerableIfNoMatchingFiles()
        {
            var files = await PackageReaderExtensions.GetSatelliteFilesAsync(
                _contentReader.Object,
                "kr",
                CancellationToken.None);

            Assert.Empty(files);
        }

        [Fact]
        public async Task GetSatelliteFilesAsync_ReturnsSatelliteFiles()
        {
            var files = await PackageReaderExtensions.GetSatelliteFilesAsync(
                _contentReader.Object,
                "fr",
                CancellationToken.None);

            Assert.Equal(new[] { "lib/net45/fr/a.resources.dll" }, files);
        }
    }
}
