// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Packaging.Core;
using NuGet.Packaging.PackageExtraction;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Xunit;

namespace NuGet.Packaging.Test
{
    public class PackageHelperTests
    {
        [Theory]
        [InlineData(".dll")]
        [InlineData("a.dll")]
        [InlineData("A.DLL")]
        [InlineData(".exe")]
        [InlineData("a.exe")]
        [InlineData("A.EXE")]
        [InlineData(".winmd")]
        [InlineData("a.winmd")]
        [InlineData("A.WINMD")]
        [InlineData("./a.dll")]
        [InlineData(".\\a.dll")]
        public void IsAssembly_ReturnsTrueForAssembly(string filePath)
        {
            Assert.True(PackageHelper.IsAssembly(filePath));
        }

        [Theory]
        [InlineData("")]
        [InlineData("dll")]
        [InlineData("adll")]
        [InlineData("a.xml")]
        public void IsAssembly_ReturnsFalseForNonAssembly(string filePath)
        {
            Assert.False(PackageHelper.IsAssembly(filePath));
        }

        [Theory]
        [InlineData(".nuspec")]
        [InlineData("a.nuspec")]
        [InlineData("A.NUSPEC")]
        [InlineData("./a.nuspec")]
        [InlineData(".\\a.nuspec")]
        public void IsNuspec_ReturnsTrueForNuspec(string filePath)
        {
            Assert.True(PackageHelper.IsNuspec(filePath));
        }

        [Theory]
        [InlineData("")]
        [InlineData("nuspec")]
        [InlineData("anuspec")]
        [InlineData("a.xml")]
        public void IsNuspec_ReturnsFalseForNonNuspec(string filePath)
        {
            Assert.False(PackageHelper.IsNuspec(filePath));
        }

        [Theory]
        [InlineData(".nuspec")]
        [InlineData("a.nuspec")]
        [InlineData("A.NUSPEC")]
        public void IsManifest_ReturnsTrueForManifest(string filePath)
        {
            Assert.True(PackageHelper.IsManifest(filePath));
        }

        [Theory]
        [InlineData("")]
        [InlineData("/a.nuspec")]
        [InlineData("\\a.nuspec")]
        public void IsManifest_ReturnsFalseForNonManifest(string filePath)
        {
            Assert.False(PackageHelper.IsManifest(filePath));
        }

        [Theory]
        [InlineData("")]
        [InlineData("a")]
        public void IsRoot_ReturnsTrueForRoot(string filePath)
        {
            Assert.True(PackageHelper.IsRoot(filePath));
        }

        [Theory]
        [InlineData("/")]
        [InlineData("\\")]
        public void IsRoot_ReturnsFalseForNonRoot(string filePath)
        {
            Assert.False(PackageHelper.IsRoot(filePath));
        }

        [Theory]
        [InlineData("packageA.nuspec", PackageSaveMode.Nuspec)]
        [InlineData("package.txt", PackageSaveMode.Files)]
        [InlineData("lib/net40/a.dll", PackageSaveMode.Files)]
        [InlineData(@"content\net45\a.js", PackageSaveMode.Files)]
        [InlineData(@"content/[Content_Types].xml", PackageSaveMode.Files)]
        public void IsPackageFile_ReturnsTrueForPackageFile(string file, PackageSaveMode packageSaveMode)
        {
            // Arrange & Act
            var isPackageFile = PackageHelper.IsPackageFile(file, packageSaveMode);

            // Assert
            Assert.True(isPackageFile);
        }

        [Theory]
        [InlineData("packageA.nuspec", PackageSaveMode.Files)]
        [InlineData(@"package\services\metadata\core-properties\blahblah.psmdcp", PackageSaveMode.Files)]
        [InlineData(@"package/services/metadata/core-properties/blahblah.psmdcp", PackageSaveMode.Files)]
        [InlineData(@"_rels\._rels", PackageSaveMode.Files)]
        [InlineData("_rels/._rels", PackageSaveMode.Files)]
        [InlineData(@"[Content_Types].xml", PackageSaveMode.Files)]
        public void IsPackageFile_ReturnsFalseForNonPackageFile(string file, PackageSaveMode packageSaveMode)
        {
            // Arrange & Act
            var isPackageFile = PackageHelper.IsPackageFile(file, packageSaveMode);

            // Assert
            Assert.False(isPackageFile);
        }

        [Fact]
        public async Task GetSatelliteFilesAsync_ReturnsEmptyEnumerableIfRuntimePackageDoesNotExist()
        {
            using (var testDirectory = TestDirectory.Create())
            {
                var packageIdentity = new PackageIdentity("A", new NuGetVersion("1.2.3"));
                var packagePathResolver = new PackagePathResolver(testDirectory.Path);
                var satellitePackageInfo = await TestPackagesCore.GetSatellitePackageAsync(
                    testDirectory.Path,
                    packageIdentity.Id,
                    packageIdentity.Version.ToNormalizedString(),
                    language: "fr");

                using (var packageReader = new PackageArchiveReader(File.OpenRead(satellitePackageInfo.FullName)))
                {
                    var result = await PackageHelper.GetSatelliteFilesAsync(packageReader, packagePathResolver, CancellationToken.None);

                    var runtimePackageDirectory = result.Item1;
                    var satelliteFiles = result.Item2;

                    Assert.Null(runtimePackageDirectory);
                    Assert.Empty(satelliteFiles);
                }
            }
        }

        [Fact]
        public async Task GetSatelliteFilesAsync_ReturnsSatelliteFilesForSatellitePackage()
        {
            using (var testDirectory = TestDirectory.Create())
            {
                var packageIdentity = new PackageIdentity("A", new NuGetVersion("1.2.3"));
                var packagePathResolver = new PackagePathResolver(testDirectory.Path);
                var packageFileInfo = await TestPackagesCore.GetRuntimePackageAsync(
                    testDirectory.Path,
                    packageIdentity.Id,
                    packageIdentity.Version.ToNormalizedString());
                var satellitePackageInfo = await TestPackagesCore.GetSatellitePackageAsync(
                    testDirectory.Path,
                    packageIdentity.Id,
                    packageIdentity.Version.ToNormalizedString(),
                    language: "fr");

                using (var packageReader = new PackageArchiveReader(File.OpenRead(satellitePackageInfo.FullName)))
                {
                    var result = await PackageHelper.GetSatelliteFilesAsync(packageReader, packagePathResolver, CancellationToken.None);

                    var runtimePackageDirectory = result.Item1;
                    var satelliteFiles = result.Item2;

                    Assert.Equal(testDirectory.Path, runtimePackageDirectory);
                    Assert.Equal(new[] { "lib/net45/fr/A.resources.dll" }, satelliteFiles);
                }
            }
        }

        [Fact]
        public async Task GetInstalledPackageFilesAsync_ReturnsEmptyEnumerableIfNoPackageFilesInstalled()
        {
            using (var test = PackageHelperTest.Create(TestPackagesCore.GetPackageContentReaderTestPackage()))
            {
                var files = await PackageHelper.GetInstalledPackageFilesAsync(
                    test.Reader,
                    test.Reader.GetIdentity(),
                    test.Resolver,
                    PackageSaveMode.Defaultv3,
                    CancellationToken.None);

                Assert.Empty(files);
            }
        }

        [Fact]
        public async Task GetInstalledPackageFilesAsync_ReturnsInstalledPackageFiles()
        {
            using (var test = PackageHelperTest.Create(TestPackagesCore.GetPackageContentReaderTestPackage()))
            {
                using (var packageStream = test.GetPackageStream())
                {
                    await PackageExtractor.ExtractPackageAsync(
                        test.Root,
                        test.Reader,
                        packageStream,
                        test.Resolver,
                        new PackageExtractionContext(
                            PackageSaveMode.Defaultv2,
                            PackageExtractionBehavior.XmlDocFileSaveMode,
                            clientPolicyContext: null,
                            logger: NullLogger.Instance),
                        CancellationToken.None);
                }

                var packageIdentity = test.Reader.GetIdentity();
                var packageDirectoryName = $"{packageIdentity.Id}.{packageIdentity.Version.ToNormalizedString()}";

                // Delete some files so that the set of installed files is incomplete.
                Directory.Delete(Path.Combine(test.Root, packageDirectoryName, "lib"), recursive: true);

                var files = (await PackageHelper.GetInstalledPackageFilesAsync(
                    test.Reader,
                    test.Reader.GetIdentity(),
                    test.Resolver,
                    PackageSaveMode.Defaultv3,
                    CancellationToken.None)).ToArray();

                Assert.Equal(9, files.Length);
                Assert.Equal(Path.Combine(test.Root, packageDirectoryName, "build/net45/a.dll"), files[0].FileFullPath);
                Assert.Equal(Path.Combine(test.Root, packageDirectoryName, "build/net45/a.props"), files[1].FileFullPath);
                Assert.Equal(Path.Combine(test.Root, packageDirectoryName, "build/net45/a.targets"), files[2].FileFullPath);
                Assert.Equal(Path.Combine(test.Root, packageDirectoryName, "content/net45/b"), files[3].FileFullPath);
                Assert.Equal(Path.Combine(test.Root, packageDirectoryName, "content/net45/c"), files[4].FileFullPath);
                Assert.Equal(Path.Combine(test.Root, packageDirectoryName, "other/net45/h"), files[5].FileFullPath);
                Assert.Equal(Path.Combine(test.Root, packageDirectoryName, "other/net45/i"), files[6].FileFullPath);
                Assert.Equal(Path.Combine(test.Root, packageDirectoryName, "tools/net45/j"), files[7].FileFullPath);
                Assert.Equal(Path.Combine(test.Root, packageDirectoryName, "tools/net45/k"), files[8].FileFullPath);
            }
        }

        [Fact]
        public async Task GetInstalledSatelliteFilesAsync_ReturnsNullRuntimePackageDirectoryIfRuntimePackageDoesNotExist()
        {
            using (var test = PackageHelperTest.Create(TestPackagesCore.GetPackageContentReaderTestPackage()))
            {
                using (var testDirectory = TestDirectory.Create())
                {
                    var packageIdentity = new PackageIdentity("A", new NuGetVersion("1.2.3"));
                    var packagePathResolver = new PackagePathResolver(testDirectory.Path);
                    var satellitePackageInfo = await TestPackagesCore.GetSatellitePackageAsync(
                        testDirectory.Path,
                        packageIdentity.Id,
                        packageIdentity.Version.ToNormalizedString(),
                        language: "fr");

                    using (var packageReader = new PackageArchiveReader(File.OpenRead(satellitePackageInfo.FullName)))
                    {
                        var result = await PackageHelper.GetInstalledSatelliteFilesAsync(
                            packageReader,
                            packagePathResolver,
                            PackageSaveMode.Defaultv3,
                            CancellationToken.None);

                        var runtimePackageDirectory = result.Item1;
                        var satelliteFiles = result.Item2;

                        Assert.Null(runtimePackageDirectory);
                        Assert.Empty(satelliteFiles);
                    }
                }
            }
        }

        [Fact]
        public async Task GetInstalledSatelliteFilesAsync_ReturnsRuntimePackageDirectoryIfRuntimePackageExistsButIsNotInstalled()
        {
            using (var test = PackageHelperTest.Create(TestPackagesCore.GetPackageContentReaderTestPackage()))
            {
                using (var testDirectory = TestDirectory.Create())
                {
                    var packageIdentity = new PackageIdentity("A", new NuGetVersion("1.2.3"));
                    var packagePathResolver = new PackagePathResolver(testDirectory.Path);
                    var packageFileInfo = await TestPackagesCore.GetRuntimePackageAsync(
                        testDirectory.Path,
                        packageIdentity.Id,
                        packageIdentity.Version.ToNormalizedString());
                    var satellitePackageInfo = await TestPackagesCore.GetSatellitePackageAsync(
                        testDirectory.Path,
                        packageIdentity.Id,
                        packageIdentity.Version.ToNormalizedString(),
                        language: "fr");

                    using (var packageReader = new PackageArchiveReader(File.OpenRead(satellitePackageInfo.FullName)))
                    {
                        var result = await PackageHelper.GetInstalledSatelliteFilesAsync(
                            packageReader,
                            packagePathResolver,
                            PackageSaveMode.Defaultv3,
                            CancellationToken.None);

                        var runtimePackageDirectory = result.Item1;
                        var satelliteFiles = result.Item2;

                        Assert.Equal(testDirectory.Path, runtimePackageDirectory);
                        Assert.Empty(satelliteFiles);
                    }
                }
            }
        }

        [Fact]
        public async Task GetInstalledSatelliteFilesAsync_ReturnsEmptyEnumerableForNoInstalledSatelliteFiles()
        {
            using (var test = PackageHelperTest.Create(TestPackagesCore.GetPackageContentReaderTestPackage()))
            {
                using (var testDirectory = TestDirectory.Create())
                {
                    var packageIdentity = new PackageIdentity("A", new NuGetVersion("1.2.3"));
                    var packagePathResolver = new PackagePathResolver(testDirectory.Path);
                    var packageFileInfo = await TestPackagesCore.GetRuntimePackageAsync(
                        testDirectory.Path,
                        packageIdentity.Id,
                        packageIdentity.Version.ToNormalizedString());
                    var satellitePackageInfo = await TestPackagesCore.GetSatellitePackageAsync(
                        testDirectory.Path,
                        packageIdentity.Id,
                        packageIdentity.Version.ToNormalizedString(),
                        language: "fr");

                    // Install runtime package
                    using (var packageReader = new PackageArchiveReader(File.OpenRead(packageFileInfo.FullName)))
                    using (var packageStream = File.OpenRead(packageFileInfo.FullName))
                    {
                        await PackageExtractor.ExtractPackageAsync(
                            test.Root,
                            packageReader,
                            packageStream,
                            packagePathResolver,
                            new PackageExtractionContext(
                                PackageSaveMode.Defaultv2,
                                PackageExtractionBehavior.XmlDocFileSaveMode,
                                clientPolicyContext: null,
                                logger: NullLogger.Instance),
                            CancellationToken.None);
                    }

                    using (var packageReader = new PackageArchiveReader(File.OpenRead(satellitePackageInfo.FullName)))
                    {
                        var result = await PackageHelper.GetInstalledSatelliteFilesAsync(
                            packageReader,
                            packagePathResolver,
                            PackageSaveMode.Defaultv3,
                            CancellationToken.None);

                        var runtimePackageDirectory = result.Item1;
                        var satelliteFiles = result.Item2;
                        var packageDirectoryName = $"{packageIdentity.Id}.{packageIdentity.Version.ToNormalizedString()}";

                        Assert.Equal(Path.Combine(testDirectory.Path, packageDirectoryName), runtimePackageDirectory);
                        Assert.Empty(satelliteFiles);
                    }
                }
            }
        }

        [Fact]
        public async Task GetInstalledSatelliteFilesAsync_ReturnsInstalledSatelliteFiles()
        {
            using (var testDirectory = TestDirectory.Create())
            {
                var packageIdentity = new PackageIdentity("A", new NuGetVersion("1.2.3"));
                var packagePathResolver = new PackagePathResolver(testDirectory.Path);
                var packageFileInfo = await TestPackagesCore.GetRuntimePackageAsync(
                    testDirectory.Path,
                    packageIdentity.Id,
                    packageIdentity.Version.ToNormalizedString());
                var satellitePackageInfo = await TestPackagesCore.GetSatellitePackageAsync(
                    testDirectory.Path,
                    packageIdentity.Id,
                    packageIdentity.Version.ToNormalizedString(),
                    language: "fr");

                // Install runtime package
                using (var packageReader = new PackageArchiveReader(File.OpenRead(packageFileInfo.FullName)))
                using (var packageStream = File.OpenRead(packageFileInfo.FullName))
                {
                    await PackageExtractor.ExtractPackageAsync(
                        testDirectory.Path,
                        packageStream,
                        packagePathResolver,
                        new PackageExtractionContext(
                            PackageSaveMode.Defaultv2,
                            PackageExtractionBehavior.XmlDocFileSaveMode,
                            clientPolicyContext: null,
                            logger: NullLogger.Instance),
                        CancellationToken.None);
                }

                // Install satellite package
                using (var packageReader = new PackageArchiveReader(File.OpenRead(satellitePackageInfo.FullName)))
                using (var packageStream = File.OpenRead(satellitePackageInfo.FullName))
                {
                    await PackageExtractor.ExtractPackageAsync(
                        testDirectory.Path,
                        packageReader,
                        packageStream,
                        packagePathResolver,
                        new PackageExtractionContext(
                            PackageSaveMode.Defaultv2,
                            PackageExtractionBehavior.XmlDocFileSaveMode,
                            clientPolicyContext: null,
                            logger: NullLogger.Instance),
                        CancellationToken.None);
                }

                using (var packageReader = new PackageArchiveReader(File.OpenRead(satellitePackageInfo.FullName)))
                {
                    var result = await PackageHelper.GetInstalledSatelliteFilesAsync(
                        packageReader,
                        packagePathResolver,
                        PackageSaveMode.Defaultv3,
                        CancellationToken.None);

                    var runtimePackageDirectory = result.Item1;
                    var satelliteFiles = result.Item2.ToArray();
                    var packageDirectoryName = $"{packageIdentity.Id}.{packageIdentity.Version.ToNormalizedString()}";

                    Assert.Equal(Path.Combine(testDirectory.Path, packageDirectoryName), runtimePackageDirectory);
                    Assert.Equal(1, satelliteFiles.Length);
                    Assert.Equal(Path.Combine(testDirectory.Path, packageDirectoryName, "lib/net45/fr/A.resources.dll"), satelliteFiles[0].FileFullPath);
                }
            }
        }

        private sealed class PackageHelperTest : IDisposable
        {
            private bool _isDisposed;
            private readonly TestPackagesCore.TempFile _tempFile;
            private readonly TestDirectory _testDirectory;

            internal PackageArchiveReader Reader { get; }
            internal PackagePathResolver Resolver { get; }
            internal string Root { get; }

            private PackageHelperTest(
                TestDirectory testDirectory,
                PackageArchiveReader reader,
                TestPackagesCore.TempFile tempFile)
            {
                Reader = reader;
                _testDirectory = testDirectory;
                _tempFile = tempFile;
                Resolver = new PackagePathResolver(_testDirectory.Path);
                Root = testDirectory.Path;
            }

            public void Dispose()
            {
                if (!_isDisposed)
                {
                    Reader.Dispose();
                    _tempFile.Dispose();
                    _testDirectory.Dispose();

                    GC.SuppressFinalize(this);

                    _isDisposed = true;
                }
            }

            internal Stream GetPackageStream()
            {
                return File.OpenRead(_tempFile);
            }

            internal static PackageHelperTest Create(TestPackagesCore.TempFile tempFile)
            {
                var testDirectory = TestDirectory.Create();
                var zip = TestPackagesCore.GetZip(tempFile);
                var reader = new PackageArchiveReader(zip);

                return new PackageHelperTest(testDirectory, reader, tempFile);
            }
        }
    }
}
