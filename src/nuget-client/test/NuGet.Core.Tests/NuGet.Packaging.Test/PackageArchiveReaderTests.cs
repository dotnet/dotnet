// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
#if IS_SIGNING_SUPPORTED
using System.Text;
#endif
using System.Threading;
using System.Threading.Tasks;
#if IS_SIGNING_SUPPORTED
using Microsoft.Internal.NuGet.Testing.SignedPackages;
#endif
using Moq;
using NuGet.Common;
using NuGet.Frameworks;
using NuGet.Packaging.Core;
using NuGet.Packaging.Signing;
using NuGet.Test.Utility;
using NuGet.Versioning;
#if IS_SIGNING_SUPPORTED
using Test.Utility.Signing;
#endif
using Xunit;

namespace NuGet.Packaging.Test
{
    public class PackageArchiveReaderTests
    {
        private const string SignatureVerificationEnvironmentVariable = "DOTNET_NUGET_SIGNATURE_VERIFICATION";
        private const string SignatureVerificationEnvironmentVariableTypo = "DOTNET_NUGET_SIGNATURE_VERIFICATIOn";

        [Fact]
        public void Constructor_WithStringPathParameter_DisposesInvalidStream()
        {
            // Arrange
            using (var testDirectory = TestDirectory.Create())
            {
                var path = Path.Combine(testDirectory, "invalid.nupkg");
                File.WriteAllText(path, "This is not a valid .zip archive.");

                // Act & Assert
                Assert.Throws<InvalidDataException>(() =>
                    new PackageArchiveReader(path).Dispose());

                File.Delete(path);
                Assert.False(File.Exists(path), "The invalid .zip archive should not exist.");
            }
        }

        [Fact]
        public void Dispose_WithStringPathConstructorParameter_DisposesStream()
        {
            // Arrange
            using (var test = TestPackagesCore.GetPackageContentReaderTestPackage())
            {
                var packageArchiveReader = new PackageArchiveReader(test);
                var identity = packageArchiveReader.GetIdentity();

                // Act
                packageArchiveReader.Dispose();

                // Assert
                File.Delete(test);
                Assert.False(File.Exists(test), "The .zip archive should not exist.");
            }
        }

        [Fact]
        public void GetReferenceItems_RespectsReferencesAccordingToDifferentFrameworks()
        {
            // Copy of the InstallPackageRespectReferencesAccordingToDifferentFrameworks functional test

            // Arrange
            using (var path = TestPackagesCore.GetNearestReferenceFilteringPackage())
            using (var zip = TestPackagesCore.GetZip(path.File))
            using (var reader = new PackageArchiveReader(zip))
            {
                // Act
                var references = reader.GetReferenceItems();
                var netResult = NuGetFrameworkUtility.GetNearest(references, NuGetFramework.Parse("net45"));
                var slResult = NuGetFrameworkUtility.GetNearest(references, NuGetFramework.Parse("sl5"));

                // Assert
                Assert.Equal(2, netResult.Items.Count());
                Assert.Equal(1, slResult.Items.Count());
                Assert.Equal("lib/sl40/a.dll", slResult.Items.First());
                Assert.Equal("lib/net40/one.dll", netResult.Items.First());
                Assert.Equal("lib/net40/three.dll", netResult.Items.Skip(1).First());
            }
        }

        [Fact]
        public void GetReferenceItems_ReturnsLegacyFolders()
        {
            // Verify legacy folder names such as 40 and 35 parse to frameworks
            using (var packageFile = TestPackagesCore.GetLegacyFolderPackage())
            {
                var zip = TestPackagesCore.GetZip(packageFile);

                using (var reader = new PackageArchiveReader(zip))
                {
                    var groups = reader.GetReferenceItems().ToArray();

                    Assert.Equal(4, groups.Count());

                    Assert.Equal(NuGetFramework.AnyFramework, groups[0].TargetFramework);
                    Assert.Equal("lib/a.dll", groups[0].Items.ToArray()[0]);

                    Assert.Equal(NuGetFramework.Parse("net35"), groups[1].TargetFramework);
                    Assert.Equal("lib/35/b.dll", groups[1].Items.ToArray()[0]);

                    Assert.Equal(NuGetFramework.Parse("net4"), groups[2].TargetFramework);
                    Assert.Equal("lib/40/test40.dll", groups[2].Items.ToArray()[0]);
                    Assert.Equal("lib/40/x86/testx86.dll", groups[2].Items.ToArray()[1]);

                    Assert.Equal(NuGetFramework.Parse("net45"), groups[3].TargetFramework);
                    Assert.Equal("lib/45/a.dll", groups[3].Items.ToArray()[0]);
                }
            }
        }

        [Fact]
        public void GetReferenceItems_ReturnsNestedReferenceItemsMixed()
        {
            using (var packageFile = TestPackagesCore.GetLibEmptyFolderPackage())
            {
                var zip = TestPackagesCore.GetZip(packageFile);

                using (var reader = new PackageArchiveReader(zip))
                {
                    var groups = reader.GetReferenceItems().ToArray();

                    Assert.Equal(3, groups.Count());

                    Assert.Equal(NuGetFramework.AnyFramework, groups[0].TargetFramework);
                    Assert.Equal(2, groups[0].Items.Count());
                    Assert.Equal("lib/a.dll", groups[0].Items.ToArray()[0]);
                    Assert.Equal("lib/x86/b.dll", groups[0].Items.ToArray()[1]);

                    Assert.Equal(NuGetFramework.Parse("net40"), groups[1].TargetFramework);
                    Assert.Equal(2, groups[1].Items.Count());
                    Assert.Equal("lib/net40/test40.dll", groups[1].Items.ToArray()[0]);
                    Assert.Equal("lib/net40/x86/testx86.dll", groups[1].Items.ToArray()[1]);

                    Assert.Equal(NuGetFramework.Parse("net45"), groups[2].TargetFramework);
                    Assert.Equal(0, groups[2].Items.Count());
                }
            }
        }

        // Verify empty target framework folders under lib are returned
        [Fact]
        public void GetReferenceItems_ReturnsEmptyLibFolder()
        {
            using (var packageFile = TestPackagesCore.GetLibEmptyFolderPackage())
            {
                var zip = TestPackagesCore.GetZip(packageFile);

                using (var reader = new PackageArchiveReader(zip))
                {
                    var groups = reader.GetReferenceItems().ToArray();

                    var emptyGroup = groups.Single(g => g.TargetFramework == NuGetFramework.ParseFolder("net45"));

                    Assert.Equal(0, emptyGroup.Items.Count());
                }
            }
        }

        [Fact]
        public void GetReferenceItems_ReturnsNestedReferenceItems()
        {
            using (var packageFile = TestPackagesCore.GetLibSubFolderPackage())
            {
                var zip = TestPackagesCore.GetZip(packageFile);

                using (var reader = new PackageArchiveReader(zip))
                {
                    var groups = reader.GetReferenceItems().ToArray();

                    Assert.Equal(1, groups.Count());

                    Assert.Equal(NuGetFramework.Parse("net40"), groups[0].TargetFramework);
                    Assert.Equal(2, groups[0].Items.Count());
                    Assert.Equal("lib/net40/test40.dll", groups[0].Items.ToArray()[0]);
                    Assert.Equal("lib/net40/x86/testx86.dll", groups[0].Items.ToArray()[1]);
                }
            }
        }

        [Theory]
        [InlineData("3.0.5-beta", "3.0.5-beta")]
        [InlineData("2.5", "2.5.0")]
        [InlineData("2.5-beta", "2.5.0-beta")]
        public void GetMinClientVersion_ReturnsNormalizedString(string minClientVersion, string expected)
        {
            using (var packageFile = TestPackagesCore.GetLegacyTestPackageMinClient(minClientVersion))
            {
                var zip = TestPackagesCore.GetZip(packageFile);

                using (var reader = new PackageArchiveReader(zip))
                {
                    var version = reader.GetMinClientVersion();

                    Assert.Equal(expected, version.ToNormalizedString());
                }
            }
        }

        [Fact]
        public void GetContentItems_ReturnsContentWithMixedFrameworks()
        {
            using (var packageFile = TestPackagesCore.GetLegacyContentPackageMixed())
            {
                var zip = TestPackagesCore.GetZip(packageFile);

                using (var reader = new PackageArchiveReader(zip))
                {
                    var groups = reader.GetContentItems().ToArray();

                    Assert.Equal(3, groups.Count());
                }
            }
        }

        [Fact]
        public void GetContentItems_ReturnsContentWithFrameworks()
        {
            using (var packageFile = TestPackagesCore.GetLegacyContentPackageWithFrameworks())
            {
                var zip = TestPackagesCore.GetZip(packageFile);

                using (var reader = new PackageArchiveReader(zip))
                {
                    var groups = reader.GetContentItems().ToArray();

                    Assert.Equal(3, groups.Count());
                }
            }
        }

        [Fact]
        public void GetContentItems_ReturnsContentWithNoFrameworks()
        {
            using (var packageFile = TestPackagesCore.GetLegacyContentPackage())
            {
                var zip = TestPackagesCore.GetZip(packageFile);

                using (var reader = new PackageArchiveReader(zip))
                {
                    var groups = reader.GetContentItems().ToArray();

                    Assert.Equal(1, groups.Count());

                    Assert.Equal(NuGetFramework.AnyFramework, groups.Single().TargetFramework);

                    Assert.Equal(3, groups.Single().Items.Count());
                }
            }
        }

        // get reference items without any nuspec entries
        [Fact]
        public void GetReferenceItems_ReturnsItemsWithNoNuspecEntries()
        {
            using (var packageFile = TestPackagesCore.GetLegacyTestPackage())
            {
                var zip = TestPackagesCore.GetZip(packageFile);

                using (var reader = new PackageArchiveReader(zip))
                {
                    var groups = reader.GetReferenceItems().ToArray();

                    Assert.Equal(3, groups.Count());

                    Assert.Equal(4, groups.SelectMany(e => e.Items).Count());
                }
            }
        }

        // normal reference group filtering
        [Fact]
        public void GetReferenceItems_ReturnsReferencesWithGroups()
        {
            using (var packageFile = TestPackagesCore.GetLegacyTestPackageWithReferenceGroups())
            {
                var zip = TestPackagesCore.GetZip(packageFile);

                using (var reader = new PackageArchiveReader(zip))
                {
                    var groups = reader.GetReferenceItems().ToArray();

                    Assert.Equal(2, groups.Count());

                    Assert.Equal(NuGetFramework.AnyFramework, groups[0].TargetFramework);
                    Assert.Equal(1, groups[0].Items.Count());
                    Assert.Equal("lib/test.dll", groups[0].Items.Single());

                    Assert.Equal(NuGetFramework.Parse("net45"), groups[1].TargetFramework);
                    Assert.Equal(1, groups[1].Items.Count());
                    Assert.Equal("lib/net45/test45.dll", groups[1].Items.Single());
                }
            }
        }

        // v1.5 reference flat list applied to a 2.5+ nupkg with frameworks
        [Fact]
        public void GetReferenceItems_ReturnsReferencesWithoutGroups()
        {
            using (var packageFile = TestPackagesCore.GetLegacyTestPackageWithPre25References())
            {
                var zip = TestPackagesCore.GetZip(packageFile);

                using (var reader = new PackageArchiveReader(zip))
                {
                    var groups = reader.GetReferenceItems().ToArray();

                    Assert.Equal(3, groups.Count());

                    Assert.Equal(NuGetFramework.AnyFramework, groups[0].TargetFramework);
                    Assert.Equal(1, groups[0].Items.Count());
                    Assert.Equal("lib/test.dll", groups[0].Items.Single());

                    Assert.Equal(NuGetFramework.Parse("net40"), groups[1].TargetFramework);
                    Assert.Equal(1, groups[1].Items.Count());
                    Assert.Equal("lib/net40/test.dll", groups[1].Items.Single());

                    Assert.Equal(NuGetFramework.Parse("net451"), groups[2].TargetFramework);
                    Assert.Equal(1, groups[1].Items.Count());
                    Assert.Equal("lib/net451/test.dll", groups[2].Items.Single());
                }
            }
        }

        [Fact]
        public void GetSupportedFrameworks_ReturnsSupportedFrameworks()
        {
            using (var packageFile = TestPackagesCore.GetLegacyTestPackage())
            {
                var zip = TestPackagesCore.GetZip(packageFile);

                using (var reader = new PackageArchiveReader(zip))
                {
                    var frameworks = reader.GetSupportedFrameworks().Select(f => f.DotNetFrameworkName).ToArray();

                    Assert.Equal("Any,Version=v0.0", frameworks[0]);
                    Assert.Equal(".NETFramework,Version=v4.0", frameworks[1]);
                    Assert.Equal(".NETFramework,Version=v4.5", frameworks[2]);
                    Assert.Equal(3, frameworks.Length);
                }
            }
        }

        [Fact]
        public void IsServiceable_ReturnsTrueForServiceablePackage()
        {
            // Arrange
            using (var packageFile = TestPackagesCore.GetServiceablePackage())
            {
                var zip = TestPackagesCore.GetZip(packageFile);

                using (var reader = new PackageArchiveReader(zip))
                {
                    // Act
                    var actual = reader.IsServiceable();

                    // Assert
                    Assert.True(actual);
                }
            }
        }

        [Fact]
        public void GetPackageTypes_ReturnsMultiplePackageTypes()
        {
            // Arrange
            using (var packageFile = TestPackagesCore.GetPackageWithPackageTypes())
            {
                var zip = TestPackagesCore.GetZip(packageFile);

                using (var reader = new PackageArchiveReader(zip))
                {
                    // Act
                    var actual = reader.GetPackageTypes();

                    // Assert
                    Assert.Equal(2, actual.Count);
                    Assert.Equal("foo", actual[0].Name);
                    Assert.Equal(new Version(0, 0), actual[0].Version);
                    Assert.Equal("bar", actual[1].Name);
                    Assert.Equal(new Version(2, 0, 0), actual[1].Version);
                }
            }
        }

        [Fact]
        public void GetSupportedFrameworks_ThrowsForSupportedFrameworksForInvalidPortableFramework()
        {
            using (var packageFile = TestPackagesCore.GetLegacyTestPackageWithInvalidPortableFrameworkFolderName())
            {
                var zip = TestPackagesCore.GetZip(packageFile);

                using (var reader = new PackageArchiveReader(zip))
                {
                    var ex = Assert.Throws<PackagingException>(
                        () => reader.GetSupportedFrameworks());
                    Assert.Equal(
                        "The framework in the folder name of '" +
                        "lib/portable-net+win+wpa+wp+sl+net-cf+netmf+MonoAndroid+MonoTouch+Xamarin.iOS/test.dll" +
                        "' in package 'packageA.2.0.3' could not be parsed.",
                        ex.Message);
                    Assert.NotNull(ex.InnerException);
                    Assert.IsType<ArgumentException>(ex.InnerException);
                    Assert.Equal(
                        "Invalid portable frameworks '" +
                        "net+win+wpa+wp+sl+net-cf+netmf+MonoAndroid+MonoTouch+Xamarin.iOS" +
                        "'. A hyphen may not be in any of the portable framework names.",
                        ex.InnerException.Message);
                }
            }
        }

        [Fact]
        public void GetIdentity_ReturnsPackageIdentity()
        {
            using (var test = PackageReaderTest.Create(TestPackagesCore.GetPackageCoreReaderTestPackage()))
            {
                var identity = test.Reader.GetIdentity();

                Assert.NotNull(identity);
                Assert.Equal("Aa", identity.Id);
                Assert.Equal("4.5.6", identity.Version.ToNormalizedString());
            }
        }

        [Fact]
        public async Task GetIdentityAsync_ReturnsPackageIdentity()
        {
            using (var test = PackageReaderTest.Create(TestPackagesCore.GetPackageCoreReaderTestPackage()))
            {
                var identity = await test.Reader.GetIdentityAsync(CancellationToken.None);

                Assert.NotNull(identity);
                Assert.Equal("Aa", identity.Id);
                Assert.Equal("4.5.6", identity.Version.ToNormalizedString());
            }
        }

        [Fact]
        public void GetMinClientVersion_ReturnsNullIfNoMinClientVersion()
        {
            using (var test = PackageReaderTest.Create(TestPackagesCore.GetPackageCoreAndContentReaderMinimalTestPackage()))
            {
                var version = test.Reader.GetMinClientVersion();

                Assert.Null(version);
            }
        }

        [Fact]
        public async Task GetMinClientVersionAsync_ReturnsNullIfNoMinClientVersion()
        {
            using (var test = PackageReaderTest.Create(TestPackagesCore.GetPackageCoreAndContentReaderMinimalTestPackage()))
            {
                var version = await test.Reader.GetMinClientVersionAsync(CancellationToken.None);

                Assert.Null(version);
            }
        }

        [Fact]
        public void GetMinClientVersion_ReturnsMinClientVersion()
        {
            using (var test = PackageReaderTest.Create(TestPackagesCore.GetPackageCoreReaderTestPackage()))
            {
                var version = test.Reader.GetMinClientVersion();

                Assert.NotNull(version);
                Assert.Equal("1.2.3", version.ToNormalizedString());
            }
        }

        [Fact]
        public async Task GetMinClientVersionAsync_ReturnsMinClientVersion()
        {
            using (var test = PackageReaderTest.Create(TestPackagesCore.GetPackageCoreReaderTestPackage()))
            {
                var version = await test.Reader.GetMinClientVersionAsync(CancellationToken.None);

                Assert.NotNull(version);
                Assert.Equal("1.2.3", version.ToNormalizedString());
            }
        }

        [Fact]
        public void GetPackageTypes_ReturnsEmptyEnumerableIfNoPackageTypes()
        {
            using (var test = PackageReaderTest.Create(TestPackagesCore.GetPackageCoreAndContentReaderMinimalTestPackage()))
            {
                var packageTypes = test.Reader.GetPackageTypes();

                Assert.Empty(packageTypes);
            }
        }

        [Fact]
        public async Task GetPackageTypesAsync_ReturnsEmptyEnumerableIfNoPackageTypes()
        {
            using (var test = PackageReaderTest.Create(TestPackagesCore.GetPackageCoreAndContentReaderMinimalTestPackage()))
            {
                var packageTypes = await test.Reader.GetPackageTypesAsync(CancellationToken.None);

                Assert.Empty(packageTypes);
            }
        }

        [Fact]
        public void GetPackageTypes_ReturnsPackageTypes()
        {
            using (var test = PackageReaderTest.Create(TestPackagesCore.GetPackageCoreReaderTestPackage()))
            {
                var packageTypes = test.Reader.GetPackageTypes();

                Assert.NotEmpty(packageTypes);
                Assert.Equal(2, packageTypes.Count);
                Assert.Equal("Bb", packageTypes[0].Name);
                Assert.Equal("0.0", packageTypes[0].Version.ToString());
                Assert.Equal("Cc", packageTypes[1].Name);
                Assert.Equal("7.8.9", packageTypes[1].Version.ToString());
            }
        }

        [Fact]
        public async Task GetPackageTypesAsync_ReturnsPackageTypes()
        {
            using (var test = PackageReaderTest.Create(TestPackagesCore.GetPackageCoreReaderTestPackage()))
            {
                var packageTypes = await test.Reader.GetPackageTypesAsync(CancellationToken.None);

                Assert.NotEmpty(packageTypes);
                Assert.Equal(2, packageTypes.Count);
                Assert.Equal("Bb", packageTypes[0].Name);
                Assert.Equal("0.0", packageTypes[0].Version.ToString());
                Assert.Equal("Cc", packageTypes[1].Name);
                Assert.Equal("7.8.9", packageTypes[1].Version.ToString());
            }
        }

        [Fact]
        public void GetStream_ReturnsReadableStream()
        {
            using (var test = PackageReaderTest.Create(TestPackagesCore.GetPackageCoreReaderTestPackage()))
            using (var stream = test.Reader.GetStream("Aa.nuspec"))
            {
                Assert.NotNull(stream);
                Assert.True(stream.CanRead);
            }
        }

        [Fact]
        public async Task GetStreamAsync_ReturnsReadableStream()
        {
            using (var test = PackageReaderTest.Create(TestPackagesCore.GetPackageCoreReaderTestPackage()))
            using (var stream = await test.Reader.GetStreamAsync("Aa.nuspec", CancellationToken.None))
            {
                Assert.NotNull(stream);
                Assert.True(stream.CanRead);
            }
        }

        [Fact]
        public void GetFiles_ReturnsFiles()
        {
            using (var test = PackageReaderTest.Create(TestPackagesCore.GetPackageCoreReaderTestPackage()))
            {
                var files = test.Reader.GetFiles();

                Assert.NotNull(files);
                Assert.Equal(3, files.Count());
            }
        }

        [Fact]
        public async Task GetFilesAsync_ReturnsFiles()
        {
            using (var test = PackageReaderTest.Create(TestPackagesCore.GetPackageCoreReaderTestPackage()))
            {
                var files = (await test.Reader.GetFilesAsync(CancellationToken.None))
                    .OrderBy(file => file)
                    .ToArray();

                Assert.NotNull(files);
                Assert.Equal(3, files.Length);
                Assert.Equal("Aa.nuspec", files[0]);
                Assert.Equal("lib/net45/a.dll", files[1]);
                Assert.Equal("lib/net45/b.dll", files[2]);
            }
        }

        [Fact]
        public void GetFiles_WithFolder_ReturnsEmptyEnumerableIfNoFiles()
        {
            using (var test = PackageReaderTest.Create(TestPackagesCore.GetPackageCoreAndContentReaderMinimalTestPackage()))
            {
                var files = test.Reader.GetFiles("lib");

                Assert.Empty(files);
            }
        }

        [Fact]
        public async Task GetFilesAsync_WithFolder_ReturnsEmptyEnumerableIfNoFiles()
        {
            using (var test = PackageReaderTest.Create(TestPackagesCore.GetPackageCoreAndContentReaderMinimalTestPackage()))
            {
                var files = await test.Reader.GetFilesAsync("lib", CancellationToken.None);

                Assert.Empty(files);
            }
        }

        [Fact]
        public void GetFiles_WithFolder_ReturnsFiles()
        {
            using (var test = PackageReaderTest.Create(TestPackagesCore.GetPackageCoreReaderTestPackage()))
            {
                var files = test.Reader.GetFiles("lib");

                Assert.NotNull(files);
                Assert.Equal(2, files.Count());
            }
        }

        [Fact]
        public async Task GetFilesAsync_WithFolder_ReturnsFiles()
        {
            using (var test = PackageReaderTest.Create(TestPackagesCore.GetPackageCoreReaderTestPackage()))
            {
                var files = (await test.Reader.GetFilesAsync("lib", CancellationToken.None))
                    .OrderBy(file => file)
                    .ToArray();

                Assert.NotNull(files);
                Assert.Equal(2, files.Length);
                Assert.Equal("lib/net45/a.dll", files[0]);
                Assert.Equal("lib/net45/b.dll", files[1]);
            }
        }

        [Fact]
        public void GetNuspec_ReturnsReadableStream()
        {
            using (var test = PackageReaderTest.Create(TestPackagesCore.GetPackageCoreReaderTestPackage()))
            {
                var stream = test.Reader.GetNuspec();

                Assert.NotNull(stream);
                Assert.True(stream.CanRead);
            }
        }

        [Fact]
        public async Task GetNuspecAsync_ReturnsReadableStream()
        {
            using (var test = PackageReaderTest.Create(TestPackagesCore.GetPackageCoreReaderTestPackage()))
            {
                var stream = await test.Reader.GetNuspecAsync(CancellationToken.None);

                Assert.NotNull(stream);
                Assert.True(stream.CanRead);
            }
        }

        [Fact]
        public void GetNuspec_ReturnsStream()
        {
            // Arrange
            using (var stream = new MemoryStream())
            {
                using (var zip = new ZipArchive(stream, ZipArchiveMode.Create, leaveOpen: true))
                {
                    zip.AddEntry("lib/net45/a.dll", new byte[0]);
                    zip.AddEntry("package.nuspec", new byte[5]);
                }

                stream.Seek(0, SeekOrigin.Begin);

                using (var reader = new PackageArchiveReader(stream))
                {
                    // Act
                    using (var nuspec = new BinaryReader(reader.GetNuspec()))
                    {
                        // Assert
                        Assert.NotNull(nuspec);
                        Assert.Equal(5, nuspec.ReadBytes(4096).Length);
                    }
                }
            }
        }

        [Fact]
        public void GetNuspec_ReturnsStreamForRootNuspec()
        {
            // Arrange
            using (var stream = new MemoryStream())
            {
                using (var zip = new ZipArchive(stream, ZipArchiveMode.Create, leaveOpen: true))
                {
                    zip.AddEntry("lib/net45/a.dll", new byte[0]);
                    zip.AddEntry("package.nuspec", new byte[5]);
                    zip.AddEntry("content/package.nuspec", new byte[0]);
                }

                // Act
                using (var reader = new PackageArchiveReader(stream))
                using (var nuspec = new BinaryReader(reader.GetNuspec()))
                {
                    // Assert
                    Assert.NotNull(nuspec);
                    Assert.Equal(5, nuspec.ReadBytes(4096).Length);
                }
            }
        }

        [Fact]
        public void GetNuspec_ThrowsForNoRootNuspec()
        {
            // Arrange
            using (var stream = new MemoryStream())
            {
                using (var zip = new ZipArchive(stream, ZipArchiveMode.Create, leaveOpen: true))
                {
                    zip.AddEntry("lib/net45/a.dll", new byte[0]);
                    zip.AddEntry("content/package.nuspec", new byte[0]);
                }

                using (var reader = new PackageArchiveReader(stream))
                {
                    // Act
                    var exception = Assert.Throws<PackagingException>(() => reader.GetNuspec());

                    // Assert
                    var log = exception.AsLogMessage();
                    Assert.Equal(NuGetLogCode.NU5037, log.Code);
                    Assert.Contains("The package is missing the required nuspec file.", log.Message);
                }
            }
        }

        [Fact]
        public void GetNuspec_ThrowsForMultipleRootNuspecs()
        {
            // Arrange
            using (var stream = new MemoryStream())
            {
                using (var zip = new ZipArchive(stream, ZipArchiveMode.Create, leaveOpen: true))
                {
                    zip.AddEntry("lib/net45/a.dll", new byte[0]);
                    zip.AddEntry("package.NUSPEC", new byte[0]);
                    zip.AddEntry("package2.nuspec", new byte[0]);
                }

                using (var reader = new PackageArchiveReader(stream))
                {
                    // Act
                    var exception = Assert.Throws<PackagingException>(() => reader.GetNuspec());

                    // Assert
                    Assert.Equal("Package contains multiple nuspec files.", exception.Message);
                }
            }
        }

        [Fact]
        public void GetNuspec_ThrowsForNoNuspec()
        {
            // Arrange
            using (var stream = new MemoryStream())
            {
                using (var zip = new ZipArchive(stream, ZipArchiveMode.Create, leaveOpen: true))
                {
                    zip.AddEntry("lib/net45/a.dll", new byte[0]);
                }

                using (var reader = new PackageArchiveReader(stream))
                {
                    // Act
                    var exception = Assert.Throws<PackagingException>(() => reader.GetNuspec());

                    // Assert
                    var log = exception.AsLogMessage();
                    Assert.Equal(NuGetLogCode.NU5037, log.Code);
                    Assert.Contains("The package is missing the required nuspec file.", log.Message);
                }
            }
        }

        [Fact]
        public void GetNuspec_ThrowsForNoNuspecWithCorrectExtension()
        {
            // Arrange
            using (var stream = new MemoryStream())
            {
                using (var zip = new ZipArchive(stream, ZipArchiveMode.Create, leaveOpen: true))
                {
                    zip.AddEntry("lib/net45/a.dll", new byte[0]);
                    zip.AddEntry("nuspec.blah", new byte[0]);
                    zip.AddEntry("blahnuspec", new byte[0]);
                    zip.AddEntry("blah/nuspec", new byte[0]);
                    zip.AddEntry("blah-nuspec", new byte[0]);
                    zip.AddEntry("blah.nuspecc", new byte[0]);
                }

                using (var reader = new PackageArchiveReader(stream))
                {
                    // Act
                    var exception = Assert.Throws<PackagingException>(() => reader.GetNuspec());

                    // Assert
                    var log = exception.AsLogMessage();
                    Assert.Equal(NuGetLogCode.NU5037, log.Code);
                    Assert.Contains("The package is missing the required nuspec file.", log.Message);
                }
            }
        }

        [Fact]
        public void GetNuspec_SupportsEscapingInFileName()
        {
            // Arrange
            using (var stream = new MemoryStream())
            {
                using (var zip = new ZipArchive(stream, ZipArchiveMode.Create, leaveOpen: true))
                {
                    zip.AddEntry("lib/net45/a.dll", new byte[0]);
                    zip.AddEntry("package%20.nuspec", new byte[5]);
                }

                using (var reader = new PackageArchiveReader(stream))
                {
                    // Act
                    var nuspec = new BinaryReader(reader.GetNuspec());

                    // Assert
                    Assert.NotNull(nuspec);
                    Assert.Equal(5, nuspec.ReadBytes(4096).Length);
                }
            }
        }

        [Fact]
        public void GetNuspecFile_ReturnsNuspecPathInPackage()
        {
            using (var test = PackageReaderTest.Create(TestPackagesCore.GetPackageCoreReaderTestPackage()))
            {
                var filePath = test.Reader.GetNuspecFile();

                Assert.Equal("Aa.nuspec", filePath);
            }
        }

        [Fact]
        public async Task GetNuspecAsyncFile_ReturnsNuspecPathInPackage()
        {
            using (var test = PackageReaderTest.Create(TestPackagesCore.GetPackageCoreReaderTestPackage()))
            {
                var filePath = await test.Reader.GetNuspecFileAsync(CancellationToken.None);

                Assert.Equal("Aa.nuspec", filePath);
            }
        }

        [Fact]
        public void CopyFiles_ReturnsCopiedFilePaths()
        {
            using (var testDirectory = TestDirectory.Create())
            using (var test = PackageReaderTest.Create(TestPackagesCore.GetPackageCoreReaderTestPackage()))
            {
                var files = test.Reader.CopyFiles(
                    testDirectory.Path,
                    new[] { "Aa.nuspec" },
                    ExtractFile,
                    NullLogger.Instance,
                    CancellationToken.None);

                var expectedFilePath = Path.Combine(testDirectory.Path, "Aa.nuspec");

                Assert.Equal(1, files.Count());
                Assert.Equal(expectedFilePath, files.Single());
                Assert.True(File.Exists(expectedFilePath));
            }
        }

        [Fact]
        public async Task CopyFiles_ContainsInvalidEntry_Fail()
        {
            // Arrange
            using (var root = TestDirectory.Create())
            {
                var resolver = new PackagePathResolver(root);
                var identity = new PackageIdentity("A", new NuGetVersion("2.0.3"));
                var packageFileInfo = await TestPackagesCore.GeneratePackageAsync(
                   root,
                   identity.Id,
                   identity.Version.ToString(),
                   entryModifiedTime: DateTimeOffset.Now,
                   "../../A.dll",
                   "content/net40/B.nuspec");

                using (var packageStream = File.OpenRead(packageFileInfo.FullName))
                using (var packageReader = new PackageArchiveReader(packageStream))
                {
                    // Act & Assert
                    Assert.Throws<UnsafePackageEntryException>(() => packageReader.CopyFiles(
                        root.Path,
                        new[] { "../../A.dll", "content/net40/B.nuspec" },
                        ExtractFile,
                        NullLogger.Instance,
                        CancellationToken.None));
                }
            }
        }

        [Fact]
        public async Task CopyFiles_ContainsRootEntry_Fail()
        {
            // Arrange
            using (var root = TestDirectory.Create())
            {
                var resolver = new PackagePathResolver(root);
                var identity = new PackageIdentity("A", new NuGetVersion("2.0.3"));
                var rootPath = RuntimeEnvironmentHelper.IsWindows ? @"C:" : @"/";

                var packageFileInfo = await TestPackagesCore.GeneratePackageAsync(
                   root,
                   identity.Id,
                   identity.Version.ToString(),
                   entryModifiedTime: DateTimeOffset.Now,
                   $"{rootPath}/A.dll",
                   "content/net40/B.nuspec");

                using (var packageStream = File.OpenRead(packageFileInfo.FullName))
                using (var packageReader = new PackageArchiveReader(packageStream))
                {
                    // Act & Assert
                    Assert.Throws<UnsafePackageEntryException>(() => packageReader.CopyFiles(
                        root.Path,
                        new[] { $"{rootPath}/A.dll", "content/net40/B.nuspec" },
                        ExtractFile,
                        NullLogger.Instance,
                        CancellationToken.None));
                }
            }
        }

        [Fact]
        public async Task CopyFiles_ContainsCurrentEntry_Fail()
        {
            // Arrange
            using (var root = TestDirectory.Create())
            {
                var resolver = new PackagePathResolver(root);
                var identity = new PackageIdentity("A", new NuGetVersion("2.0.3"));

                var packageFileInfo = await TestPackagesCore.GeneratePackageAsync(
                   root,
                   identity.Id,
                   identity.Version.ToString(),
                   entryModifiedTime: DateTimeOffset.Now,
                   ".",
                   "content/net40/B.nuspec");

                using (var packageStream = File.OpenRead(packageFileInfo.FullName))
                using (var packageReader = new PackageArchiveReader(packageStream))
                {
                    // Act & Assert
                    Assert.Throws<UnsafePackageEntryException>(() => packageReader.CopyFiles(
                        root.Path,
                        new[] { ".", "content/net40/B.nuspec" },
                        ExtractFile,
                        NullLogger.Instance,
                        CancellationToken.None));
                }
            }
        }

        [Fact]
        public async Task CopyFilesAsync_ReturnsCopiedFilePaths()
        {
            using (var testDirectory = TestDirectory.Create())
            using (var test = PackageReaderTest.Create(TestPackagesCore.GetPackageCoreReaderTestPackage()))
            {
                var files = await test.Reader.CopyFilesAsync(
                    testDirectory.Path,
                    new[] { "Aa.nuspec" },
                    ExtractFile,
                    NullLogger.Instance,
                    CancellationToken.None);

                var expectedFilePath = Path.Combine(testDirectory.Path, "Aa.nuspec");

                Assert.Equal(1, files.Count());
                Assert.Equal(expectedFilePath, files.Single());
                Assert.True(File.Exists(expectedFilePath));
            }
        }

        [PlatformFact(Platform.Windows, Platform.Darwin)]
        public async Task CopyFilesAsync_PathWithDifferentCasingOnWindowsAndMac_Succeeds()
        {
            // Arrange
            using (var root = TestDirectory.Create())
            {
                using (var destination = TestDirectory.Create())
                {
                    var resolver = new PackagePathResolver(root);
                    var identity = new PackageIdentity("A", new NuGetVersion("2.0.3"));

                    var packageFileInfo = await TestPackagesCore.GeneratePackageAsync(
                       root,
                       identity.Id,
                       identity.Version.ToString(),
                       entryModifiedTime: DateTimeOffset.Now,
                       @"readme~.txt");

                    using (var packageStream = File.OpenRead(packageFileInfo.FullName))
                    using (var packageReader = new PackageArchiveReader(packageStream))
                    {
                        // Act & Assert
                        var files = await packageReader.CopyFilesAsync(
                            destination.Path.ToUpper(),
                            new[] { @"readme~.txt" },
                            ExtractFile,
                            NullLogger.Instance,
                            CancellationToken.None);

                        var expectedFilePath = Path.Combine(destination.Path.ToUpper(), "readme~.txt");

                        Assert.Equal(1, files.Count());
                        Assert.Equal(expectedFilePath, files.Single());
                        Assert.True(File.Exists(expectedFilePath));
                    }
                }
            }
        }

        [PlatformFact(Platform.Linux)]
        public async Task CopyFilesAsync_PathWithDifferentCasingOnLinux_Fails()
        {
            // Arrange
            using (var root = TestDirectory.Create())
            {
                using (var destination = TestDirectory.Create())
                {
                    var resolver = new PackagePathResolver(root);
                    var identity = new PackageIdentity("A", new NuGetVersion("2.0.3"));

                    var packageFileInfo = await TestPackagesCore.GeneratePackageAsync(
                       root,
                       identity.Id,
                       identity.Version.ToString(),
                       entryModifiedTime: DateTimeOffset.Now,
                       @"readme~.txt");

                    using (var packageStream = File.OpenRead(packageFileInfo.FullName))
                    using (var packageReader = new PackageArchiveReader(packageStream))
                    {
                        // Act & Assert
                        //The return value of Path.GetFullPath() varies based on OS. Hence DirectoryNotFoundException is thrown instead of UnsafePackageEntryException
                        await Assert.ThrowsAsync<DirectoryNotFoundException>(async () => await packageReader.CopyFilesAsync(
                             destination.Path.ToUpper(),
                             new[] { "readme~.txt" },
                             ExtractFile,
                             NullLogger.Instance,
                             CancellationToken.None));
                    }
                }
            }
        }

        [Fact]
        public void GetFrameworkItems_ReturnsEmptyEnumerableIfNoFrameworkItems()
        {
            using (var test = PackageReaderTest.Create(TestPackagesCore.GetPackageCoreAndContentReaderMinimalTestPackage()))
            {
                var groups = test.Reader.GetFrameworkItems();

                Assert.Empty(groups);
            }
        }

        [Fact]
        public void GetFrameworkItems_ReturnsFrameworkItems()
        {
            using (var test = PackageReaderTest.Create(TestPackagesCore.GetPackageContentReaderTestPackage()))
            {
                var groups = test.Reader.GetFrameworkItems().ToArray();

                Assert.Equal(2, groups.Length);
                Assert.Equal(new[] { "Z" }, groups[0].Items);
                Assert.Equal(".NETFramework,Version=v4.0", groups[0].TargetFramework.DotNetFrameworkName);
                Assert.Equal(new[] { "Y" }, groups[1].Items);
                Assert.Equal("Silverlight,Version=v3.0", groups[1].TargetFramework.DotNetFrameworkName);
            }
        }

        [Fact]
        public async Task GetFrameworkItemsAsync_ReturnsEmptyEnumerableIfNoFrameworkItems()
        {
            using (var test = PackageReaderTest.Create(TestPackagesCore.GetPackageCoreAndContentReaderMinimalTestPackage()))
            {
                var groups = await test.Reader.GetFrameworkItemsAsync(CancellationToken.None);

                Assert.Empty(groups);
            }
        }

        [Fact]
        public async Task GetFrameworkItemsAsync_ReturnsFrameworkItems()
        {
            using (var test = PackageReaderTest.Create(TestPackagesCore.GetPackageContentReaderTestPackage()))
            {
                var groups = (await test.Reader.GetFrameworkItemsAsync(CancellationToken.None)).ToArray();

                Assert.Equal(2, groups.Length);
                Assert.Equal(new[] { "Z" }, groups[0].Items);
                Assert.Equal(".NETFramework,Version=v4.0", groups[0].TargetFramework.DotNetFrameworkName);
                Assert.Equal(new[] { "Y" }, groups[1].Items);
                Assert.Equal("Silverlight,Version=v3.0", groups[1].TargetFramework.DotNetFrameworkName);
            }
        }

        [Fact]
        public void GetBuildItems_ReturnsEmptyEnumerableIfNoBuildItems()
        {
            using (var test = PackageReaderTest.Create(TestPackagesCore.GetPackageCoreAndContentReaderMinimalTestPackage()))
            {
                var groups = test.Reader.GetBuildItems();

                Assert.Empty(groups);
            }
        }

        [Fact]
        public void GetBuildItems_ReturnsBuildItems()
        {
            using (var test = PackageReaderTest.Create(TestPackagesCore.GetPackageContentReaderTestPackage()))
            {
                var groups = test.Reader.GetBuildItems().ToArray();

                Assert.Equal(1, groups.Length);
                Assert.Equal(new[] { "build/net45/a.props", "build/net45/a.targets" }, groups[0].Items);
                Assert.Equal(".NETFramework,Version=v4.5", groups[0].TargetFramework.DotNetFrameworkName);
            }
        }

        [Fact]
        public async Task GetBuildItemsAsync_EmptyEnumerableIfNoBuildItems()
        {
            using (var test = PackageReaderTest.Create(TestPackagesCore.GetPackageCoreAndContentReaderMinimalTestPackage()))
            {
                var groups = await test.Reader.GetBuildItemsAsync(CancellationToken.None);

                Assert.Empty(groups);
            }
        }

        [Fact]
        public async Task GetBuildItemsAsync_ReturnsBuildItems()
        {
            using (var test = PackageReaderTest.Create(TestPackagesCore.GetPackageContentReaderTestPackage()))
            {
                var groups = (await test.Reader.GetBuildItemsAsync(CancellationToken.None)).ToArray();

                Assert.Equal(1, groups.Length);
                Assert.Equal(new[] { "build/net45/a.props", "build/net45/a.targets" }, groups[0].Items);
                Assert.Equal(".NETFramework,Version=v4.5", groups[0].TargetFramework.DotNetFrameworkName);
            }
        }

        [Fact]
        public void GetToolItems_ReturnsEmptyEnumerableIfNoToolItems()
        {
            using (var test = PackageReaderTest.Create(TestPackagesCore.GetPackageCoreAndContentReaderMinimalTestPackage()))
            {
                var groups = test.Reader.GetToolItems();

                Assert.Empty(groups);
            }
        }

        [Fact]
        public void GetToolItems_ReturnsToolItems()
        {
            using (var test = PackageReaderTest.Create(TestPackagesCore.GetPackageContentReaderTestPackage()))
            {
                var groups = test.Reader.GetToolItems().ToArray();

                Assert.Equal(1, groups.Length);
                Assert.Equal(new[] { "tools/net45/j", "tools/net45/k" }, groups[0].Items);
                Assert.Equal(".NETFramework,Version=v4.5", groups[0].TargetFramework.DotNetFrameworkName);
            }
        }

        [Fact]
        public async Task GetToolItemsAsync_EmptyEnumerableIfNoToolItems()
        {
            using (var test = PackageReaderTest.Create(TestPackagesCore.GetPackageCoreAndContentReaderMinimalTestPackage()))
            {
                var groups = await test.Reader.GetToolItemsAsync(CancellationToken.None);

                Assert.Empty(groups);
            }
        }

        [Fact]
        public async Task GetToolItemsAsync_ReturnsToolItems()
        {
            using (var test = PackageReaderTest.Create(TestPackagesCore.GetPackageContentReaderTestPackage()))
            {
                var groups = (await test.Reader.GetToolItemsAsync(CancellationToken.None)).ToArray();

                Assert.Equal(1, groups.Length);
                Assert.Equal(new[] { "tools/net45/j", "tools/net45/k" }, groups[0].Items);
                Assert.Equal(".NETFramework,Version=v4.5", groups[0].TargetFramework.DotNetFrameworkName);
            }
        }

        [Fact]
        public void GetContentItems_ReturnsEmptyEnumerableIfNoContentItems()
        {
            using (var test = PackageReaderTest.Create(TestPackagesCore.GetPackageCoreAndContentReaderMinimalTestPackage()))
            {
                var groups = test.Reader.GetContentItems();

                Assert.Empty(groups);
            }
        }

        [Fact]
        public void GetContentItems_ReturnsContentItems()
        {
            using (var test = PackageReaderTest.Create(TestPackagesCore.GetPackageContentReaderTestPackage()))
            {
                var groups = test.Reader.GetContentItems().ToArray();

                Assert.Equal(1, groups.Length);
                Assert.Equal(new[] { "content/net45/b", "content/net45/c" }, groups[0].Items);
                Assert.Equal(".NETFramework,Version=v4.5", groups[0].TargetFramework.DotNetFrameworkName);
            }
        }

        [Fact]
        public async Task GetContentItemsAsync_ReturnsEmptyEnumerableIfNoContentItems()
        {
            using (var test = PackageReaderTest.Create(TestPackagesCore.GetPackageCoreAndContentReaderMinimalTestPackage()))
            {
                var groups = await test.Reader.GetContentItemsAsync(CancellationToken.None);

                Assert.Empty(groups);
            }
        }

        [Fact]
        public async Task GetContentItemsAsync_ReturnsContentItems()
        {
            using (var test = PackageReaderTest.Create(TestPackagesCore.GetPackageContentReaderTestPackage()))
            {
                var groups = (await test.Reader.GetContentItemsAsync(CancellationToken.None)).ToArray();

                Assert.Equal(1, groups.Length);
                Assert.Equal(new[] { "content/net45/b", "content/net45/c" }, groups[0].Items);
                Assert.Equal(".NETFramework,Version=v4.5", groups[0].TargetFramework.DotNetFrameworkName);
            }
        }

        [Fact]
        public void GetLibItems_ReturnsEmptyEnumerableIfNoLibItems()
        {
            using (var test = PackageReaderTest.Create(TestPackagesCore.GetPackageCoreAndContentReaderMinimalTestPackage()))
            {
                var groups = test.Reader.GetLibItems();

                Assert.Empty(groups);
            }
        }

        [Fact]
        public void GetLibItems_ReturnsLibItems()
        {
            using (var test = PackageReaderTest.Create(TestPackagesCore.GetPackageContentReaderTestPackage()))
            {
                var groups = test.Reader.GetLibItems().ToArray();

                Assert.Equal(1, groups.Length);
                Assert.Equal(new[] { "lib/net45/d", "lib/net45/e", "lib/net45/f.dll", "lib/net45/g.dll" }, groups[0].Items);
                Assert.Equal(".NETFramework,Version=v4.5", groups[0].TargetFramework.DotNetFrameworkName);
            }
        }

        [Fact]
        public async Task GetLibItemsAsync_ReturnsEmptyEnumerableIfNoLibItems()
        {
            using (var test = PackageReaderTest.Create(TestPackagesCore.GetPackageCoreAndContentReaderMinimalTestPackage()))
            {
                var groups = await test.Reader.GetLibItemsAsync(CancellationToken.None);

                Assert.Empty(groups);
            }
        }

        [Fact]
        public async Task GetLibItemsAsync_ReturnsLibItems()
        {
            using (var test = PackageReaderTest.Create(TestPackagesCore.GetPackageContentReaderTestPackage()))
            {
                var groups = (await test.Reader.GetLibItemsAsync(CancellationToken.None)).ToArray();

                Assert.Equal(1, groups.Length);
                Assert.Equal(new[] { "lib/net45/d", "lib/net45/e", "lib/net45/f.dll", "lib/net45/g.dll" }, groups[0].Items);
                Assert.Equal(".NETFramework,Version=v4.5", groups[0].TargetFramework.DotNetFrameworkName);
            }
        }

        [Fact]
        public void GetReferenceItems_ReturnsEmptyEnumerableIfNoReferenceItems()
        {
            using (var test = PackageReaderTest.Create(TestPackagesCore.GetPackageCoreAndContentReaderMinimalTestPackage()))
            {
                var groups = test.Reader.GetReferenceItems();

                Assert.Empty(groups);
            }
        }

        [Fact]
        public void GetReferenceItems_ReturnsReferenceItems()
        {
            using (var test = PackageReaderTest.Create(TestPackagesCore.GetPackageContentReaderTestPackage()))
            {
                var groups = test.Reader.GetReferenceItems().ToArray();

                Assert.Equal(1, groups.Length);
                Assert.Equal(new[] { "lib/net45/f.dll", "lib/net45/g.dll" }, groups[0].Items);
                Assert.Equal(".NETFramework,Version=v4.5", groups[0].TargetFramework.DotNetFrameworkName);
            }
        }

        [Fact]
        public async Task GetReferenceItemsAsync_ReturnsEmptyEnumerableIfNoReferenceItems()
        {
            using (var test = PackageReaderTest.Create(TestPackagesCore.GetPackageCoreAndContentReaderMinimalTestPackage()))
            {
                var groups = await test.Reader.GetReferenceItemsAsync(CancellationToken.None);

                Assert.Empty(groups);
            }
        }

        [Fact]
        public async Task GetReferenceItemsAsync_ReturnsReferenceItems()
        {
            using (var test = PackageReaderTest.Create(TestPackagesCore.GetPackageContentReaderTestPackage()))
            {
                var groups = (await test.Reader.GetReferenceItemsAsync(CancellationToken.None)).ToArray();

                Assert.Equal(1, groups.Length);
                Assert.Equal(new[] { "lib/net45/f.dll", "lib/net45/g.dll" }, groups[0].Items);
                Assert.Equal(".NETFramework,Version=v4.5", groups[0].TargetFramework.DotNetFrameworkName);
            }
        }

        [Fact]
        public void GetPackageDependencies_ReturnsEmptyEnumerableIfNoPackageDependencies()
        {
            using (var test = PackageReaderTest.Create(TestPackagesCore.GetPackageCoreAndContentReaderMinimalTestPackage()))
            {
                var groups = test.Reader.GetPackageDependencies();

                Assert.Empty(groups);
            }
        }

        [Fact]
        public void GetPackageDependencies_ReturnsPackageDependencies()
        {
            using (var test = PackageReaderTest.Create(TestPackagesCore.GetPackageContentReaderTestPackage()))
            {
                var groups = test.Reader.GetPackageDependencies().ToArray();

                Assert.Equal(2, groups.Length);
                Assert.Equal(".NETFramework,Version=v4.0", groups[0].TargetFramework.DotNetFrameworkName);

                var packages = groups[0].Packages.ToArray();

                Assert.Equal(2, packages.Length);
                Assert.Equal("l", packages[0].Id);
                Assert.Equal("m", packages[1].Id);

                Assert.Empty(groups[1].Packages);
                Assert.Equal(".NETFramework,Version=v4.5", groups[1].TargetFramework.DotNetFrameworkName);
            }
        }

        [Fact]
        public async Task GetPackageDependenciesAsync_ReturnsEmptyEnumerableIfNoPackageDependencies()
        {
            using (var test = PackageReaderTest.Create(TestPackagesCore.GetPackageCoreAndContentReaderMinimalTestPackage()))
            {
                var groups = await test.Reader.GetPackageDependenciesAsync(CancellationToken.None);

                Assert.Empty(groups);
            }
        }

        [Fact]
        public async Task GetPackageDependenciesAsync_ReturnsPackageDependencies()
        {
            using (var test = PackageReaderTest.Create(TestPackagesCore.GetPackageContentReaderTestPackage()))
            {
                var groups = (await test.Reader.GetPackageDependenciesAsync(CancellationToken.None)).ToArray();

                Assert.Equal(2, groups.Length);
                Assert.Equal(".NETFramework,Version=v4.0", groups[0].TargetFramework.DotNetFrameworkName);

                var packages = groups[0].Packages.ToArray();

                Assert.Equal(2, packages.Length);
                Assert.Equal("l", packages[0].Id);
                Assert.Equal("m", packages[1].Id);

                Assert.Empty(groups[1].Packages);
                Assert.Equal(".NETFramework,Version=v4.5", groups[1].TargetFramework.DotNetFrameworkName);
            }
        }

        [Fact]
        public void IsServiceable_ReturnsTrueIfServiceable()
        {
            using (var test = PackageReaderTest.Create(TestPackagesCore.GetServiceablePackage()))
            {
                var isServiceable = test.Reader.IsServiceable();

                Assert.True(isServiceable);
            }
        }

        [Fact]
        public async Task IsServiceableAsync_ReturnsTrueIfServiceable()
        {
            using (var test = PackageReaderTest.Create(TestPackagesCore.GetServiceablePackage()))
            {
                var isServiceable = await test.Reader.IsServiceableAsync(CancellationToken.None);

                Assert.True(isServiceable);
            }
        }

        [Fact]
        public void IsServiceable_ReturnsFalseIfNotServiceable()
        {
            using (var test = PackageReaderTest.Create(TestPackagesCore.GetPackageCoreAndContentReaderMinimalTestPackage()))
            {
                var isServiceable = test.Reader.IsServiceable();

                Assert.False(isServiceable);
            }
        }

        [Fact]
        public async Task IsServiceableAsync_ReturnsFalseIfNotServiceable()
        {
            using (var test = PackageReaderTest.Create(TestPackagesCore.GetPackageCoreAndContentReaderMinimalTestPackage()))
            {
                var isServiceable = await test.Reader.IsServiceableAsync(CancellationToken.None);

                Assert.False(isServiceable);
            }
        }

        [Fact]
        public void GetItems_ReturnsEmptyEnumerableIfNoItems()
        {
            using (var test = PackageReaderTest.Create(TestPackagesCore.GetPackageCoreAndContentReaderMinimalTestPackage()))
            {
                var groups = test.Reader.GetItems("other");

                Assert.Empty(groups);
            }
        }

        [Fact]
        public async Task GetItemsAsync_ReturnsEmptyEnumerableIfNoItems()
        {
            using (var test = PackageReaderTest.Create(TestPackagesCore.GetPackageCoreAndContentReaderMinimalTestPackage()))
            {
                var groups = await test.Reader.GetItemsAsync("other", CancellationToken.None);

                Assert.Empty(groups);
            }
        }

        [Fact]
        public void GetItems_ReturnsItems()
        {
            using (var test = PackageReaderTest.Create(TestPackagesCore.GetPackageContentReaderTestPackage()))
            {
                var groups = test.Reader.GetItems("other").ToArray();

                Assert.Equal(1, groups.Length);
                Assert.Equal(new[] { "other/net45/h", "other/net45/i" }, groups[0].Items);
                Assert.Equal(".NETFramework,Version=v4.5", groups[0].TargetFramework.DotNetFrameworkName);
            }
        }

        [Fact]
        public async Task GetItemsAsync_ReturnsItems()
        {
            using (var test = PackageReaderTest.Create(TestPackagesCore.GetPackageContentReaderTestPackage()))
            {
                var groups = (await test.Reader.GetItemsAsync("other", CancellationToken.None)).ToArray();

                Assert.Equal(1, groups.Length);
                Assert.Equal(new[] { "other/net45/h", "other/net45/i" }, groups[0].Items);
                Assert.Equal(".NETFramework,Version=v4.5", groups[0].TargetFramework.DotNetFrameworkName);
            }
        }

        [Fact]
        public void GetDevelopmentDependency_ReturnsTrueIfDevelopmentDependency()
        {
            using (var test = PackageReaderTest.Create(TestPackagesCore.GetDevelopmentDependencyPackage()))
            {
                var isDevelopmentDependency = test.Reader.GetDevelopmentDependency();

                Assert.True(isDevelopmentDependency);
            }
        }

        [Fact]
        public async Task GetDevelopmentDependencyAsync_ReturnsTrueIfDevelopmentDependency()
        {
            using (var test = PackageReaderTest.Create(TestPackagesCore.GetDevelopmentDependencyPackage()))
            {
                var isDevelopmentDependency = await test.Reader.GetDevelopmentDependencyAsync(CancellationToken.None);

                Assert.True(isDevelopmentDependency);
            }
        }

        [Fact]
        public void GetDevelopmentDependency_ReturnsFalseIfNotDevelopmentDependency()
        {
            using (var test = PackageReaderTest.Create(TestPackagesCore.GetPackageCoreAndContentReaderMinimalTestPackage()))
            {
                var isDevelopmentDependency = test.Reader.GetDevelopmentDependency();

                Assert.False(isDevelopmentDependency);
            }
        }

        [Fact]
        public async Task GetDevelopmentDependencyAsync_ReturnsFalseIfNotDevelopmentDependency()
        {
            using (var test = PackageReaderTest.Create(TestPackagesCore.GetPackageCoreAndContentReaderMinimalTestPackage()))
            {
                var isDevelopmentDependency = await test.Reader.GetDevelopmentDependencyAsync(CancellationToken.None);

                Assert.False(isDevelopmentDependency);
            }
        }

        [Fact]
        public void NuspecReader_ReturnsNuspecReader()
        {
            using (var test = PackageReaderTest.Create(TestPackagesCore.GetPackageCoreReaderTestPackage()))
            {
                var nuspecReader = test.Reader.NuspecReader;

                Assert.NotNull(nuspecReader);
            }
        }

        [Fact]
        public async Task GetNuspecReaderAsync_ReturnsNuspecReader()
        {
            using (var test = PackageReaderTest.Create(TestPackagesCore.GetPackageCoreReaderTestPackage()))
            {
                var nuspecReader = await test.Reader.GetNuspecReaderAsync(CancellationToken.None);

                Assert.NotNull(nuspecReader);
            }
        }

        [Fact]
        public async Task CopyNupkgAsync_SucceedsAsync()
        {
            // Arrange
            using (var test = PackageReaderTest.Create(TestPackagesCore.GetPackageCoreReaderTestPackage()))
            {
                // Act
                var result = await test.Reader.CopyNupkgAsync(
                        nupkgFilePath: "a",
                        cancellationToken: CancellationToken.None);

                // Assert
                Assert.Equal("a", result);
            }
        }

        /// <summary>
        /// The NuGet Client SDK shipped many versions where PackageArchiveReader.ExtractFile would accept null
        /// for the ILogger parameter, and would not throw. The .NET SDK make use of this and therefore throwing
        /// ArgumentNullException is a breaking ABI change.
        /// </summary>
        [Fact]
        public void ExtractFile_WithNullLogger_DoesNotThrow()
        {
            // Arrange
            using (PackageReaderTest package = PackageReaderTest.Create(TestPackagesCore.GetPackageCoreReaderTestPackage()))
            using (TestDirectory testDirectory = TestDirectory.Create())
            {
                string firstFile = package.Reader.GetFiles().First();
                string destination = Path.Combine(testDirectory, firstFile);

                // Act
                package.Reader.ExtractFile(firstFile, destination, logger: null);

                // Assert is just that no exception was thrown.
            }

        }

#if IS_SIGNING_SUPPORTED
        [Fact]
        public async Task ValidateIntegrityAsync_WhenSignatureContentNull_Throws()
        {
            using (var stream = new MemoryStream(SigningTestUtility.GetResourceBytes("SignedPackage.1.0.0.nupkg")))
            using (var reader = new PackageArchiveReader(stream))
            {
                var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                    () => reader.ValidateIntegrityAsync(signatureContent: null, token: CancellationToken.None));

                Assert.Equal("signatureContent", exception.ParamName);
            }
        }

        [Fact]
        public async Task ValidateIntegrityAsync_WhenCancellationTokenCancelled_Throws()
        {
            using (var stream = new MemoryStream(SigningTestUtility.GetResourceBytes("SignedPackage.1.0.0.nupkg")))
            using (var reader = new PackageArchiveReader(stream))
            {
                var content = new SignatureContent(SigningSpecifications.V1, HashAlgorithmName.SHA256, "hash");

                await Assert.ThrowsAsync<OperationCanceledException>(
                    () => reader.ValidateIntegrityAsync(content, new CancellationToken(canceled: true)));
            }
        }

        [Fact]
        public async Task ValidateIntegrityAsync_WhenPackageNotSigned_Throws()
        {
            using (var test = PackageReaderTest.Create(TestPackagesCore.GetPackageCoreReaderTestPackage()))
            {
                var content = new SignatureContent(SigningSpecifications.V1, HashAlgorithmName.SHA256, "hash");

                var exception = await Assert.ThrowsAsync<SignatureException>(
                    () => test.Reader.ValidateIntegrityAsync(content, CancellationToken.None));

                Assert.Equal("The package is not signed. Unable to verify signature from an unsigned package.", exception.Message);
            }
        }

        [Fact]
        public async Task ValidateIntegrityAsync_WithNestedZipFiles_Succeeds()
        {
            var zip = CreateZipWithNestedStoredZipArchives();
            var zipHash = HashAlgorithmName.SHA256.ComputeHash(zip.ToByteArray());
            var signatureContent = new SignatureContent(
                SigningSpecifications.V1,
                HashAlgorithmName.SHA256,
                Convert.ToBase64String(zipHash));

            // Now mock sign the ZIP.
            zip.LocalFileHeaders.Add(zip.SignatureLocalFileHeader);
            zip.CentralDirectoryHeaders.Add(zip.SignatureCentralDirectoryHeader);

            using (var stream = new MemoryStream(zip.ToByteArray()))
            using (var reader = new PackageArchiveReader(stream))
            {
                await reader.ValidateIntegrityAsync(signatureContent, CancellationToken.None);
            }
        }

        [Fact]
        public async Task ValidatePackageEntriesAsync_InvalidPackageFiles_Fails()
        {
            // Arrange
            using (var root = TestDirectory.Create())
            {
                var resolver = new PackagePathResolver(root);
                var identity = new PackageIdentity("A", new NuGetVersion("2.0.3"));
                var packageFileInfo = await TestPackagesCore.GeneratePackageAsync(
                   root,
                   identity.Id,
                   identity.Version.ToString(),
                   entryModifiedTime: DateTimeOffset.Now,
                   "../../A.dll",
                   "content/net40/B.nuspec");

                using (var packageStream = File.OpenRead(packageFileInfo.FullName))
                using (var packageReader = new PackageArchiveReader(packageStream))
                {
                    // Act & Assert
                    await Assert.ThrowsAsync<UnsafePackageEntryException>(() => packageReader.ValidatePackageEntriesAsync(CancellationToken.None));
                }
            }
        }

        [Fact]
        public async Task ValidatePackageEntriesAsync_InvalidPackageFilesContainsRootPath_Fails()
        {
            // Arrange
            using (var root = TestDirectory.Create())
            {
                var resolver = new PackagePathResolver(root);
                var identity = new PackageIdentity("A", new NuGetVersion("2.0.3"));
                var rootPath = RuntimeEnvironmentHelper.IsWindows ? @"C:" : @"/";

                var packageFileInfo = await TestPackagesCore.GeneratePackageAsync(
                   root,
                   identity.Id,
                   identity.Version.ToString(),
                   entryModifiedTime: DateTimeOffset.Now,
                   $"{rootPath}/A.dll",
                   "content/net40/B.nuspec");

                using (var packageStream = File.OpenRead(packageFileInfo.FullName))
                using (var packageReader = new PackageArchiveReader(packageStream))
                {
                    // Act & Assert
                    await Assert.ThrowsAsync<UnsafePackageEntryException>(() => packageReader.ValidatePackageEntriesAsync(CancellationToken.None));
                }
            }
        }

        [Fact]
        public async Task ValidatePackageEntriesAsync_InvalidPackageFilesContainsCurrentPath_Fails()
        {
            // Arrange
            using (var root = TestDirectory.Create())
            {
                var resolver = new PackagePathResolver(root);
                var identity = new PackageIdentity("A", new NuGetVersion("2.0.3"));
                var rootPath = RuntimeEnvironmentHelper.IsWindows ? @"C:" : @"/";

                var packageFileInfo = await TestPackagesCore.GeneratePackageAsync(
                   root,
                   identity.Id,
                   identity.Version.ToString(),
                   entryModifiedTime: DateTimeOffset.Now,
                   ".",
                   "content/net40/B.nuspec");

                using (var packageStream = File.OpenRead(packageFileInfo.FullName))
                using (var packageReader = new PackageArchiveReader(packageStream))
                {
                    // Act & Assert
                    await Assert.ThrowsAsync<UnsafePackageEntryException>(() => packageReader.ValidatePackageEntriesAsync(CancellationToken.None));
                }
            }
        }

        [Fact]
        public async Task ValidatePackageEntriesAsync_PackageFilesWithSpecialCharacters_DoesNotThrow()
        {
            // Arrange
            using (var root = TestDirectory.Create())
            {
                var resolver = new PackagePathResolver(root);
                var identity = new PackageIdentity("A", new NuGetVersion("2.0.3"));
                var packageFileInfo = await TestPackagesCore.GeneratePackageAsync(
                   root,
                   identity.Id,
                   identity.Version.ToString(),
                   entryModifiedTime: DateTimeOffset.Now,
                   "C++.dll",
                   "content/net40/B&#A.txt",
                   "content/net40/B.nuspec");

                using (var packageStream = File.OpenRead(packageFileInfo.FullName))
                using (var packageReader = new PackageArchiveReader(packageStream))
                {
                    // Act & Assert
                    // This shouldn't throw
                    await packageReader.ValidatePackageEntriesAsync(CancellationToken.None);
                }
            }
        }

        [Fact]
        public async Task ValidatePackageEntriesAsync_ValidPackageFiles_Succeeds()
        {
            // Arrange
            using (var root = TestDirectory.Create())
            {
                var resolver = new PackagePathResolver(root);
                var identity = new PackageIdentity("A", new NuGetVersion("2.0.3"));
                var packageFileInfo = await TestPackagesCore.GeneratePackageAsync(
                   root,
                   identity.Id,
                   identity.Version.ToString(),
                   entryModifiedTime: DateTimeOffset.Now,
                   "lib/net40/A.dll",
                   "content/net40/B.nuspec");

                using (var packageStream = File.OpenRead(packageFileInfo.FullName))
                using (var packageReader = new PackageArchiveReader(packageStream))
                {
                    // Act & Assert
                    await packageReader.ValidatePackageEntriesAsync(CancellationToken.None);
                }
            }
        }

        [Fact]
        public void GetContentHash_WhenPackageNotSigned_ReturnsHashOfWholePackage()
        {
            using (var test = PackageReaderTest.Create(TestPackagesCore.GetPackageCoreReaderTestPackage()))
            {
                var result = test.Reader.GetContentHash(CancellationToken.None);

                test.Stream.Seek(offset: 0, origin: SeekOrigin.Begin);

                var expectedResult = Convert.ToBase64String(new CryptoHashProvider("SHA512").CalculateHash(test.Stream));

                Assert.Equal(expectedResult, result);
            }
        }


        [Fact]
        public void GetContentHash_UnsignedPackage_WhenGivingAFallbackFunctionThatReturnsANonEmptyString_ReturnsGivenString()
        {
            using (var root = TestDirectory.Create())
            using (var test = PackageReaderTest.Create(TestPackagesCore.GetPackageCoreReaderTestPackage()))
            {
                var result = test.Reader.GetContentHash(CancellationToken.None, GetUnsignedPackageHash: () => "abcde");

                Assert.Equal("abcde", result);
            }
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void GetContentHash_UnsignedPackage_WhenGivingAFallbackFunctionThatReturnsANullOrEmptyString_ReturnsHashOfWholePackage(string data)
        {
            using (var test = PackageReaderTest.Create(TestPackagesCore.GetPackageCoreReaderTestPackage()))
            {
                var result = test.Reader.GetContentHash(CancellationToken.None, GetUnsignedPackageHash: () => data);

                test.Stream.Seek(offset: 0, origin: SeekOrigin.Begin);

                var expectedResult = Convert.ToBase64String(new CryptoHashProvider("SHA512").CalculateHash(test.Stream));

                Assert.Equal(expectedResult, result);
            }
        }

#if IS_SIGNING_SUPPORTED
        [CIOnlyFact]
        public async Task GetContentHash_IsSameForUnsignedAndSignedPackageAsync()
        {
            // this test will create an unsigned package, copy it, then sign it. then compare the contentHash
            var nupkg = new SimpleTestPackageContext("Package.Content.Hash.Test", "1.0.0");

            using (var unsignedDir = TestDirectory.Create())
            {
                var nupkgFileName = $"{nupkg.Identity.Id}.{nupkg.Identity.Version}.nupkg";
                var nupkgFileInfo = await nupkg.CreateAsFileAsync(unsignedDir, nupkgFileName);

                using (var signedDir = TestDirectory.Create())
                {
                    Uri timestampService = null;
                    var signatureHashAlgorithm = HashAlgorithmName.SHA256;
                    var timestampHashAlgorithm = HashAlgorithmName.SHA256;

                    var signedPackagePath = Path.Combine(signedDir.Path, nupkgFileName);

                    using (var trustedCert = SigningTestUtility.GenerateTrustedTestCertificate())
                    using (var originalPackage = File.OpenRead(nupkgFileInfo.FullName))
                    using (var signedPackage = File.Open(signedPackagePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                    using (var request = new AuthorSignPackageRequest(
                        trustedCert.Source.Cert,
                        signatureHashAlgorithm,
                        timestampHashAlgorithm))
                    {
                        await SignedArchiveTestUtility.CreateSignedPackageAsync(request, originalPackage, signedPackage, timestampService);
                    }

                    using (var unsignedReader = new PackageArchiveReader(nupkgFileInfo.FullName))
                    using (var signedReader = new PackageArchiveReader(signedPackagePath))
                    {
                        var contentHashUnsigned = unsignedReader.GetContentHash(CancellationToken.None);
                        var contentHashSigned = signedReader.GetContentHash(CancellationToken.None);

                        Assert.Equal(contentHashUnsigned, contentHashSigned);
                    }
                }
            }
        }
#endif

        private static Zip CreateZipWithNestedStoredZipArchives()
        {
            var zip = new Zip();
            var nonEmptyZipArchive = CreateNonEmptyZipArchive();

            var localFileHeader1 = new LocalFileHeader()
            {
                VersionNeededToExtract = 0x14,
                LastModFileTime = 0x3811,
                LastModFileDate = 0x4c58,
                FileName = Encoding.UTF8.GetBytes("1"),
                FileData = nonEmptyZipArchive
            };
            var localFileHeader2 = new LocalFileHeader()
            {
                VersionNeededToExtract = 0x14,
                LastModFileTime = 0x36a0,
                LastModFileDate = 0x4c58,
                FileName = Encoding.UTF8.GetBytes("2"),
                FileData = nonEmptyZipArchive
            };
            var centralDirectoryHeader1 = new CentralDirectoryHeader()
            {
                VersionMadeBy = 0x14,
                VersionNeededToExtract = 0x14,
                FileName = localFileHeader1.FileName,
                LocalFileHeader = localFileHeader1
            };
            var centralDirectoryHeader2 = new CentralDirectoryHeader()
            {
                VersionMadeBy = 0x14,
                VersionNeededToExtract = 0x14,
                FileName = localFileHeader2.FileName,
                LocalFileHeader = localFileHeader2
            };

            zip.LocalFileHeaders.Add(localFileHeader1);
            zip.LocalFileHeaders.Add(localFileHeader2);
            zip.LocalFileHeaders.Add(zip.NuspecLocalFileHeader);
            zip.CentralDirectoryHeaders.Add(centralDirectoryHeader1);
            zip.CentralDirectoryHeaders.Add(centralDirectoryHeader2);
            zip.CentralDirectoryHeaders.Add(zip.NuspecCentralDirectoryHeader);

            return zip;
        }

        private static byte[] CreateNonEmptyZipArchive()
        {
            using (var stream = new MemoryStream())
            {
                using (var zipArchive = new ZipArchive(stream, ZipArchiveMode.Create))
                {
                    var entry = zipArchive.CreateEntry("entry");

                    using (var entryStream = entry.Open())
                    {
                        var data = Encoding.UTF8.GetBytes("data");

                        entryStream.Write(data, offset: 0, count: data.Length);
                    }
                }

                return stream.ToArray();
            }
        }
#endif

        [Fact]
        public void CanVerifySignedPackages_Always_ReturnsValueBasedOnOperatingSystemAndFramework()
        {
            // Arrange
            using (var test = TestPackagesCore.GetPackageContentReaderTestPackage())
            using (var packageArchiveReader = new PackageArchiveReader(test))
            {
                bool expectedResult = CanVerifySignedPackages();

                // Act
                bool actualResult = packageArchiveReader.CanVerifySignedPackages(null);

                Assert.Equal(expectedResult, actualResult);
            }
        }

        [PlatformFact(Platform.Darwin)]
        public void CanVerifySignedPackages_OnMacOs_ReturnsValueBasedOnOperatingSystemAndFramework()
        {
            // Arrange
            using (var test = TestPackagesCore.GetPackageContentReaderTestPackage())
            using (var packageArchiveReader = new PackageArchiveReader(test))
            {
                // Act
                bool result = packageArchiveReader.CanVerifySignedPackages(null);

                // Assert
                Assert.False(result);
            }
        }

        [Theory]
        [InlineData("TRUE")]
        [InlineData("True")]
        public void CanVerifySignedPackages_WhenTrue_ReturnsValueBasedOnOperatingSystemAndFramework(string envVar)
        {
            // Arrange
            var environment = new Mock<IEnvironmentVariableReader>(MockBehavior.Strict);
            environment.Setup(s => s.GetEnvironmentVariable(SignatureVerificationEnvironmentVariable)).Returns(envVar);

            using (var test = TestPackagesCore.GetPackageContentReaderTestPackage())
            using (var packageStream = File.OpenRead(test))
            using (var packageArchiveReader = new PackageArchiveReader(packageStream, environmentVariableReader: environment.Object))
            {
                bool result = packageArchiveReader.CanVerifySignedPackages(null);

                // Assert
#if IS_SIGNING_SUPPORTED
                // Verify package signature when signing is supported
                Assert.True(result);
#else
                // Cannot verify package signature when signing is not supported
                Assert.False(result);
#endif
            }
        }

        [PlatformTheory(Platform.Linux, Platform.Darwin)]
        [InlineData("FALSE")]
        [InlineData("false")]
        public void CanVerifySignedPackages_WhenFalseOnNonWindows_ReturnsValueBasedOnOperatingSystemAndFramework(string envVar)
        {
            // Arrange
            var environment = new Mock<IEnvironmentVariableReader>(MockBehavior.Strict);
            environment.Setup(s => s.GetEnvironmentVariable(SignatureVerificationEnvironmentVariable)).Returns(envVar);

            using (TestPackagesCore.TempFile test = TestPackagesCore.GetPackageContentReaderTestPackage())
            using (FileStream packageStream = File.OpenRead(test))
            using (var packageArchiveReader = new PackageArchiveReader(packageStream, environmentVariableReader: environment.Object))
            {
                bool result = packageArchiveReader.CanVerifySignedPackages(null);

                Assert.False(result);
            }
        }

        [PlatformTheory(Platform.Windows)]
        [InlineData("FALSE")]
        [InlineData("false")]
        public void CanVerifySignedPackages_WhenFalseOnWindows_ReturnsValueBasedOnOperatingSystemAndFramework(string envVar)
        {
            // Arrange
            var environment = new Mock<IEnvironmentVariableReader>(MockBehavior.Strict);
            environment.Setup(s => s.GetEnvironmentVariable(SignatureVerificationEnvironmentVariable)).Returns(envVar);

            using (TestPackagesCore.TempFile test = TestPackagesCore.GetPackageContentReaderTestPackage())
            using (FileStream packageStream = File.OpenRead(test))
            using (var packageArchiveReader = new PackageArchiveReader(packageStream, environmentVariableReader: environment.Object))
            {
                // Act
                bool result = packageArchiveReader.CanVerifySignedPackages(null);
                // Assert
#if IS_SIGNING_SUPPORTED
                // Verify package signature when signing is supported
                Assert.True(result);
#else
                // Cannot verify package signature when signing is not supported
                Assert.False(result);
#endif
            }
        }

        [Fact]
        public void CanVerifySignedPackages_ReturnsValueBasedOnOperatingSystemAndFramework_WithEnvVarNameCaseSensitive_Fails()
        {
            // Arrange
            string envVarName = SignatureVerificationEnvironmentVariableTypo;
            string envVarValue = "true";
            var environment = new Mock<IEnvironmentVariableReader>(MockBehavior.Loose);
            environment.Setup(s => s.GetEnvironmentVariable(envVarName)).Returns(envVarValue);

            using (var test = TestPackagesCore.GetPackageContentReaderTestPackage())
            using (var packageStream = File.OpenRead(test))
            using (var packageArchiveReader = new PackageArchiveReader(packageStream, environmentVariableReader: environment.Object))
            {
                // Act
                bool expectedResult = CanVerifySignedPackages(environment.Object);
                bool actualResult = packageArchiveReader.CanVerifySignedPackages(null);

                // Assert
                Assert.Equal(expectedResult, actualResult);
            }
        }

        private static bool CanVerifySignedPackages(IEnvironmentVariableReader environmentVariableReader = null)
        {
            return (RuntimeEnvironmentHelper.IsWindows ||
                IsVerificationEnabledByEnvironmentVariable(environmentVariableReader)) &&
#if IS_SIGNING_SUPPORTED
                true;
#else
                false;
#endif
        }

        private static bool IsVerificationEnabledByEnvironmentVariable(
            IEnvironmentVariableReader environmentVariableReader = null)
        {
            IEnvironmentVariableReader reader = environmentVariableReader ?? EnvironmentVariableWrapper.Instance;

            string value = reader.GetEnvironmentVariable(
                EnvironmentVariableConstants.DotNetNuGetSignatureVerification);

            return string.Equals(bool.TrueString, value, StringComparison.OrdinalIgnoreCase);
        }

        private static string ExtractFile(string sourcePath, string targetPath, Stream sourceStream)
        {
            using (var targetStream = File.OpenWrite(targetPath))
            {
                sourceStream.CopyTo(targetStream);
            }

            return targetPath;
        }

        // For testing the following implementations:
        //      IPackageCoreReader
        //      IAsyncPackageCoreReader
        //      IPackageContentReader
        //      IAsyncPackageContentReader
        private sealed class PackageReaderTest : IDisposable
        {
            private bool _isDisposed;
            private readonly TestPackagesCore.TempFile _tempFile;

            internal FileStream Stream { get; }

            internal PackageArchiveReader Reader { get; }

            private PackageReaderTest(FileStream stream, TestPackagesCore.TempFile tempFile)
            {
                Reader = new PackageArchiveReader(stream);
                Stream = stream;
                _tempFile = tempFile;
            }

            internal static PackageReaderTest Create(TestPackagesCore.TempFile tempFile)
            {
                var stream = File.OpenRead(tempFile);

                return new PackageReaderTest(stream, tempFile);
            }

            public void Dispose()
            {
                if (!_isDisposed)
                {
                    Reader.Dispose();
                    _tempFile.Dispose();

                    GC.SuppressFinalize(this);

                    _isDisposed = true;
                }
            }
        }
    }
}
