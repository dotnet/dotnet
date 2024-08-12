// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Packaging.Signing;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Xunit;

namespace Commands.Test
{
    using LocalPackageArchiveDownloader = NuGet.Protocol.LocalPackageArchiveDownloader;

    public class NugetPackageUtilsTests
    {
        private static readonly int DefaultTimeOut = (int)TimeSpan.FromMinutes(5).TotalMilliseconds;

        [Fact]
        public async Task PackageExpander_ExpandsPackage()
        {
            // Arrange
            using (var package = TestPackagesCore.GetNearestReferenceFilteringPackage())
            {
                var version = new NuGetVersion(package.Version);
                var identity = new PackageIdentity(package.Id, version);

                using (var packagesDir = TestDirectory.Create())
                {
                    var pathResolver = new VersionFolderPathResolver(packagesDir);

                    var token = CancellationToken.None;
                    var logger = NullLogger.Instance;
                    var packageExtractionContext = new PackageExtractionContext(
                        packageSaveMode: PackageSaveMode.Defaultv3,
                        xmlDocFileSaveMode: XmlDocFileSaveMode.None,
                        clientPolicyContext: null,
                        logger: logger);

                    var versionFolderPathResolver = new VersionFolderPathResolver(packagesDir);

                    // Act
                    using (var packageDownloader = new LocalPackageArchiveDownloader(
                        null,
                        package.File.FullName,
                        identity,
                        logger))
                    {
                        await PackageExtractor.InstallFromSourceAsync(
                            identity,
                            packageDownloader,
                            versionFolderPathResolver,
                            packageExtractionContext,
                            token);
                    }

                    // Assert
                    var packageDir = pathResolver.GetInstallPath(package.Id, identity.Version);
                    AssertDirectoryExists(packageDir, packageDir + " does not exist");

                    var nupkgPath = pathResolver.GetPackageFilePath(package.Id, identity.Version);
                    Assert.True(File.Exists(nupkgPath), nupkgPath + " does not exist");

                    var dllPath = Path.Combine(packageDir, "lib", "net40", "one.dll");
                    Assert.True(File.Exists(dllPath), dllPath + " does not exist");
                }
            }
        }

        [Fact]
        public async Task PackageExpander_ExpandsPackage_WithNupkgCopy()
        {
            // Arrange
            using (var package = TestPackagesCore.GetPackageWithNupkgCopy())
            {
                var version = new NuGetVersion(package.Version);
                var identity = new PackageIdentity(package.Id, version);

                using (var packagesDir = TestDirectory.Create())
                {
                    var pathResolver = new VersionFolderPathResolver(packagesDir);

                    var token = CancellationToken.None;
                    var logger = NullLogger.Instance;
                    var packageExtractionContext = new PackageExtractionContext(
                        packageSaveMode: PackageSaveMode.Defaultv3,
                        xmlDocFileSaveMode: XmlDocFileSaveMode.None,
                        clientPolicyContext: null,
                        logger: logger);

                    var versionFolderPathResolver = new VersionFolderPathResolver(packagesDir);

                    // Act
                    using (var packageDownloader = new LocalPackageArchiveDownloader(
                        null,
                        package.File.FullName,
                        identity,
                        logger))
                    {
                        await PackageExtractor.InstallFromSourceAsync(
                             identity,
                             packageDownloader,
                             versionFolderPathResolver,
                             packageExtractionContext,
                             token);
                    }

                    // Assert
                    var packageDir = pathResolver.GetInstallPath(package.Id, identity.Version);
                    AssertDirectoryExists(packageDir, packageDir + " does not exist");

                    var nupkgPath = pathResolver.GetPackageFilePath(package.Id, identity.Version);
                    Assert.True(File.Exists(nupkgPath), nupkgPath + " does not exist");

                    var dllPath = Path.Combine(packageDir, "lib", "net40", "one.dll");
                    Assert.True(File.Exists(dllPath), dllPath + " does not exist");
                }
            }
        }

        [Fact]
        public async Task PackageExpander_ExpandsPackage_SkipsIfShaIsThere()
        {
            // Arrange
            using (var package = TestPackagesCore.GetNearestReferenceFilteringPackage())
            {
                var version = new NuGetVersion(package.Version);
                var identity = new PackageIdentity(package.Id, version);

                using (var packagesDir = TestDirectory.Create())
                {
                    var pathResolver = new VersionFolderPathResolver(packagesDir);

                    var token = CancellationToken.None;
                    var logger = NullLogger.Instance;
                    var packageExtractionContext = new PackageExtractionContext(
                        packageSaveMode: PackageSaveMode.Defaultv3,
                        xmlDocFileSaveMode: XmlDocFileSaveMode.None,
                        clientPolicyContext: null,
                        logger: logger);

                    var versionFolderPathResolver = new VersionFolderPathResolver(packagesDir);

                    var packageDir = pathResolver.GetInstallPath(package.Id, identity.Version);

                    Directory.CreateDirectory(packageDir);

                    var nupkgPath = pathResolver.GetPackageFilePath(package.Id, identity.Version);
                    var shaPath = pathResolver.GetNupkgMetadataPath(package.Id, identity.Version);

                    File.WriteAllBytes(shaPath, new byte[] { });

                    Assert.True(File.Exists(shaPath));

                    // Act
                    using (var packageDownloader = new LocalPackageArchiveDownloader(
                        null,
                        package.File.FullName,
                        identity,
                        logger))
                    {
                        await PackageExtractor.InstallFromSourceAsync(
                              identity,
                              packageDownloader,
                              versionFolderPathResolver,
                              packageExtractionContext,
                              token);
                    }

                    // Assert
                    AssertDirectoryExists(packageDir, packageDir + " does not exist");

                    Assert.False(File.Exists(nupkgPath), nupkgPath + " does not exist");

                    var dllPath = Path.Combine(packageDir, "lib", "net40", "one.dll");
                    Assert.False(File.Exists(dllPath), dllPath + " does not exist");

                    Assert.Equal(1, Directory.EnumerateFiles(packageDir).Count());
                }
            }
        }

        [Fact]
        public async Task PackageExpander_CleansExtraFiles()
        {
            // Arrange
            using (var package = TestPackagesCore.GetNearestReferenceFilteringPackage())
            {
                var version = new NuGetVersion(package.Version);
                var identity = new PackageIdentity(package.Id, version);

                using (var packagesDir = TestDirectory.Create())
                {
                    var pathResolver = new VersionFolderPathResolver(packagesDir);

                    var token = CancellationToken.None;
                    var logger = NullLogger.Instance;
                    var packageExtractionContext = new PackageExtractionContext(
                        packageSaveMode: PackageSaveMode.Defaultv3,
                        xmlDocFileSaveMode: XmlDocFileSaveMode.None,
                        clientPolicyContext: null,
                        logger: logger);

                    var versionFolderPathResolver = new VersionFolderPathResolver(packagesDir);

                    var packageDir = pathResolver.GetInstallPath(package.Id, identity.Version);

                    var randomFile = Path.Combine(packageDir, package.Id + "." + package.Version + ".random");

                    Directory.CreateDirectory(packageDir);
                    File.WriteAllBytes(randomFile, new byte[] { });

                    var randomFolder = Path.Combine(packageDir, "random");
                    Directory.CreateDirectory(randomFolder);

                    Assert.True(File.Exists(randomFile), randomFile + " does not exist");
                    AssertDirectoryExists(randomFolder);

                    // Act
                    using (var packageDownloader = new LocalPackageArchiveDownloader(
                        null,
                        package.File.FullName,
                        identity,
                        logger))
                    {
                        await PackageExtractor.InstallFromSourceAsync(
                             identity,
                             packageDownloader,
                             versionFolderPathResolver,
                             packageExtractionContext,
                             token);
                    }

                    // Assert
                    AssertDirectoryExists(packageDir, packageDir + " does not exist");

                    var filePath = pathResolver.GetPackageFilePath(package.Id, identity.Version);
                    Assert.True(File.Exists(filePath), filePath + " does not exist");

                    var dllPath = Path.Combine(packageDir, "lib", "net40", "one.dll");
                    Assert.True(File.Exists(dllPath), dllPath + " does not exist");

                    Assert.False(File.Exists(randomFile), randomFile + " does exist");
                    Assert.False(Directory.Exists(randomFolder), randomFolder + " does exist");
                }
            }
        }

        [Fact]
        public async Task PackageExpander_Recovers_WhenStreamIsCorrupt()
        {
            // Arrange
            using (var package = TestPackagesCore.GetNearestReferenceFilteringPackage())
            {
                var version = new NuGetVersion(package.Version);
                var identity = new PackageIdentity(package.Id, version);

                using (var packagesDir = TestDirectory.Create())
                {
                    var pathResolver = new VersionFolderPathResolver(packagesDir);

                    var token = CancellationToken.None;
                    var logger = NullLogger.Instance;
                    var packageExtractionContext = new PackageExtractionContext(
                        packageSaveMode: PackageSaveMode.Defaultv3,
                        xmlDocFileSaveMode: XmlDocFileSaveMode.None,
                        clientPolicyContext: null,
                        logger: logger);

                    var versionFolderPathResolver = new VersionFolderPathResolver(packagesDir);

                    var packageDir = pathResolver.GetInstallPath(package.Id, identity.Version);
                    Assert.False(Directory.Exists(packageDir), packageDir + " exist");

                    // Act
                    using (var packageDownloader = new ThrowingPackageArchiveDownloader(
                        null,
                        package.File.FullName,
                        identity,
                        logger))
                    {
                        await Assert.ThrowsAnyAsync<CorruptionException>(async () =>
                           await PackageExtractor.InstallFromSourceAsync(
                             identity,
                             packageDownloader,
                             versionFolderPathResolver,
                             packageExtractionContext,
                             token));
                    }

                    AssertDirectoryExists(packageDir, packageDir + " does not exist");

                    Assert.NotEmpty(Directory.EnumerateFiles(packageDir));

                    using (var packageDownloader = new LocalPackageArchiveDownloader(
                        packagesDir,
                        package.File.FullName,
                        identity,
                        logger))
                    {
                        await PackageExtractor.InstallFromSourceAsync(
                              identity,
                              packageDownloader,
                              versionFolderPathResolver,
                              packageExtractionContext,
                              token);
                    }

                    // Assert
                    var filePath = pathResolver.GetPackageFilePath(package.Id, identity.Version);
                    Assert.True(File.Exists(filePath), filePath + " does not exist");

                    var dllPath = Path.Combine(packageDir, "lib", "net40", "one.dll");
                    Assert.True(File.Exists(dllPath), dllPath + " does not exist");
                }
            }
        }

        [Fact(Skip = "https://github.com/NuGet/Home/issues/10802")]
        public async Task PackageExpander_Recovers_WhenFileIsLocked()
        {
            // Arrange
            using (var package = TestPackagesCore.GetNearestReferenceFilteringPackage())
            {
                var version = new NuGetVersion(package.Version);
                var identity = new PackageIdentity(package.Id, version);

                using (var packagesDir = TestDirectory.Create())
                {
                    var pathResolver = new VersionFolderPathResolver(packagesDir);

                    var token = CancellationToken.None;
                    var logger = NullLogger.Instance;
                    var packageExtractionContext = new PackageExtractionContext(
                        packageSaveMode: PackageSaveMode.Defaultv3,
                        xmlDocFileSaveMode: XmlDocFileSaveMode.None,
                        clientPolicyContext: null,
                        logger: logger);

                    var versionFolderPathResolver = new VersionFolderPathResolver(packagesDir);

                    var packageDir = pathResolver.GetInstallPath(package.Id, identity.Version);
                    Assert.False(Directory.Exists(packageDir), packageDir + " exist");

                    var filePathToLock = Path.Combine(packageDir, "lib", "net40", "two.dll");

                    // Act
                    using (var packageDownloader = new LocalPackageArchiveDownloader(
                        null,
                        package.File.FullName,
                        identity,
                        logger))
                    {
                        var cts = new CancellationTokenSource(DefaultTimeOut);

                        Func<CancellationToken, Task<bool>> action = async (ct) =>
                        {
                            await Assert.ThrowsAnyAsync<IOException>(() =>
                                PackageExtractor.InstallFromSourceAsync(
                                    identity,
                                    packageDownloader,
                                    versionFolderPathResolver,
                                    packageExtractionContext,
                                    token));

                            return true;
                        };

                        await ConcurrencyUtilities.ExecuteWithFileLockedAsync(filePathToLock, action, cts.Token);
                    }

                    AssertDirectoryExists(packageDir, packageDir + " does not exist");

                    Assert.NotEmpty(Directory.EnumerateFiles(packageDir));

                    using (var packageDownloader = new LocalPackageArchiveDownloader(
                        null,
                        package.File.FullName,
                        identity,
                        logger))
                    {
                        await PackageExtractor.InstallFromSourceAsync(
                             identity,
                             packageDownloader,
                             versionFolderPathResolver,
                             packageExtractionContext,
                             token);
                    }

                    // Assert
                    var filePath = pathResolver.GetPackageFilePath(package.Id, identity.Version);
                    Assert.True(File.Exists(filePath), filePath + " does not exist");

                    var dllPath = Path.Combine(packageDir, "lib", "net40", "one.dll");
                    Assert.True(File.Exists(dllPath), dllPath + " does not exist");

                    Assert.True(File.Exists(filePathToLock));

                    // Make sure the actual file from the zip was extracted
                    Assert.Equal(new byte[] { 0 }, File.ReadAllBytes(filePathToLock));
                }
            }
        }

        [Fact]
        public async Task Test_ExtractPackage()
        {
            // Arrange
            var packageIdentity = new PackageIdentity("packageA", new NuGetVersion("2.0.3"));

            using (var packageFileInfo = TestPackagesCore.GetLegacyTestPackage())
            using (var packagesDirectory = TestDirectory.Create())
            {
                var pathResolver = new VersionFolderPathResolver(packagesDirectory);
                var packageExtractionContext = new PackageExtractionContext(
                    packageSaveMode: PackageSaveMode.Defaultv3,
                    xmlDocFileSaveMode: XmlDocFileSaveMode.None,
                    clientPolicyContext: null,
                    logger: NullLogger.Instance);

                var versionFolderPathResolver = new VersionFolderPathResolver(packagesDirectory);

                // Act
                using (var packageDownloader = new LocalPackageArchiveDownloader(
                    null,
                    packageFileInfo,
                    packageIdentity,
                    NullLogger.Instance))
                {
                    await PackageExtractor.InstallFromSourceAsync(
                         packageIdentity,
                         packageDownloader,
                         versionFolderPathResolver,
                         packageExtractionContext,
                         CancellationToken.None);
                }

                // Assert
                var packageVersionDirectory = pathResolver.GetInstallPath(packageIdentity.Id, packageIdentity.Version);

                AssertDirectoryExists(packageVersionDirectory);
                AssertFileExists(packageVersionDirectory, pathResolver.GetPackageFileName(packageIdentity.Id, packageIdentity.Version));
                AssertFileExists(packageVersionDirectory, pathResolver.GetManifestFileName(packageIdentity.Id, packageIdentity.Version));
                AssertFileExists(packageVersionDirectory, "packagea.2.0.3.nupkg.sha512");

                AssertFileExists(packageVersionDirectory, "lib", "test.dll");
            }
        }

        [Fact]
        public async Task Test_ExtractNuspecOnly()
        {
            // Arrange
            var packageIdentity = new PackageIdentity("packageA", new NuGetVersion("2.0.3"));

            using (var packageFileInfo = TestPackagesCore.GetLegacyTestPackage())
            using (var packagesDirectory = TestDirectory.Create())
            {
                var pathResolver = new VersionFolderPathResolver(packagesDirectory);
                var packageExtractionContext = new PackageExtractionContext(
                    packageSaveMode: PackageSaveMode.Nuspec | PackageSaveMode.Nupkg,
                    xmlDocFileSaveMode: XmlDocFileSaveMode.None,
                    clientPolicyContext: null,
                    logger: NullLogger.Instance);

                var versionFolderPathResolver = new VersionFolderPathResolver(packagesDirectory);

                // Act
                using (var packageDownloader = new LocalPackageArchiveDownloader(
                    null,
                    packageFileInfo,
                    packageIdentity,
                    NullLogger.Instance))
                {
                    await PackageExtractor.InstallFromSourceAsync(
                          packageIdentity,
                          packageDownloader,
                          versionFolderPathResolver,
                          packageExtractionContext,
                          CancellationToken.None);
                }

                // Assert
                var packageVersionDirectory = pathResolver.GetInstallPath(packageIdentity.Id, packageIdentity.Version);

                AssertDirectoryExists(packageVersionDirectory);
                AssertFileExists(packageVersionDirectory, pathResolver.GetPackageFileName(packageIdentity.Id, packageIdentity.Version));
                AssertFileExists(packageVersionDirectory, pathResolver.GetManifestFileName(packageIdentity.Id, packageIdentity.Version));
                AssertFileExists(packageVersionDirectory, "packagea.2.0.3.nupkg.sha512");

                Assert.False(File.Exists(Path.Combine(packageVersionDirectory, "lib", "test.dll")));
            }
        }

        [Fact]
        public async Task Test_ExtractionIgnoresNupkgHashFile()
        {
            // Arrange
            var packageIdentity = new PackageIdentity("packageA", new NuGetVersion("2.0.3"));

            using (var packagesDirectory = TestDirectory.Create())
            {
                var pathResolver = new VersionFolderPathResolver(packagesDirectory);
                var packageFileInfo = await TestPackagesCore.GetPackageWithSHA512AtRoot(
                    packagesDirectory,
                    packageIdentity.Id,
                    packageIdentity.Version.ToNormalizedString());

                var packageExtractionContext = new PackageExtractionContext(
                    packageSaveMode: PackageSaveMode.Defaultv3,
                    xmlDocFileSaveMode: XmlDocFileSaveMode.None,
                    clientPolicyContext: null,
                    logger: NullLogger.Instance);

                var versionFolderPathResolver = new VersionFolderPathResolver(packagesDirectory);
                // Act
                using (var packageDownloader = new LocalPackageArchiveDownloader(
                    null,
                    packageFileInfo.FullName,
                    packageIdentity,
                    NullLogger.Instance))
                {
                    await PackageExtractor.InstallFromSourceAsync(
                         packageIdentity,
                         packageDownloader,
                         versionFolderPathResolver,
                         packageExtractionContext,
                         CancellationToken.None);
                }

                // Assert
                var packageVersionDirectory = pathResolver.GetInstallPath(packageIdentity.Id, packageIdentity.Version);

                AssertDirectoryExists(packageVersionDirectory);
                AssertFileExists(packageVersionDirectory, pathResolver.GetPackageFilePath(packageIdentity.Id, packageIdentity.Version));
                AssertFileExists(packageVersionDirectory, pathResolver.GetManifestFileName(packageIdentity.Id, packageIdentity.Version));
                AssertFileExists(packageVersionDirectory, "lib", "net45", "A.dll");

                var hashPath = pathResolver.GetHashPath(packageIdentity.Id, packageIdentity.Version);
                var hashFileInfo = new FileInfo(hashPath);
                Assert.True(File.Exists(hashFileInfo.FullName));
                Assert.NotEqual(0, hashFileInfo.Length);

                var bsha512Path = Path.Combine(packageVersionDirectory, "lib", "net45", "B.sha512");
                var bsha512FileInfo = new FileInfo(bsha512Path);
                Assert.True(File.Exists(bsha512FileInfo.FullName));
                Assert.Equal(0, bsha512FileInfo.Length);

                var csha512Path = Path.Combine(packageVersionDirectory, "C.sha512");
                var csha512FileInfo = new FileInfo(csha512Path);
                Assert.True(File.Exists(csha512FileInfo.FullName));
                Assert.Equal(0, csha512FileInfo.Length);
            }
        }

        [Fact]
        public async Task Test_ExtractionIgnoresNupkgFile()
        {
            // Arrange
            var packageIdentity = new PackageIdentity("packageA", new NuGetVersion("2.0.3"));
            using (var packagesDirectory = TestDirectory.Create())
            {
                var pathResolver = new VersionFolderPathResolver(packagesDirectory);
                var packageFileInfo = await TestPackagesCore.GetPackageWithNupkgAtRoot(
                    packagesDirectory,
                    packageIdentity.Id,
                    packageIdentity.Version.ToNormalizedString());

                var packageExtractionContext = new PackageExtractionContext(
                    packageSaveMode: PackageSaveMode.Defaultv3,
                    xmlDocFileSaveMode: XmlDocFileSaveMode.None,
                    clientPolicyContext: null,
                    logger: NullLogger.Instance);

                var versionFolderPathResolver = new VersionFolderPathResolver(packagesDirectory);

                // Act
                using (var packageDownloader = new LocalPackageArchiveDownloader(
                    null,
                    packageFileInfo.FullName,
                    packageIdentity,
                    NullLogger.Instance))
                {
                    await PackageExtractor.InstallFromSourceAsync(
                          packageIdentity,
                          packageDownloader,
                          versionFolderPathResolver,
                          packageExtractionContext,
                          CancellationToken.None);
                }

                // Assert
                var packageVersionDirectory = pathResolver.GetInstallPath(packageIdentity.Id, packageIdentity.Version);

                AssertDirectoryExists(packageVersionDirectory);
                AssertFileExists(packageVersionDirectory, pathResolver.GetPackageFilePath(packageIdentity.Id, packageIdentity.Version));
                AssertFileExists(packageVersionDirectory, pathResolver.GetManifestFileName(packageIdentity.Id, packageIdentity.Version));
                AssertFileExists(packageVersionDirectory, "lib", "net45", "A.dll");

                var nupkgPath = pathResolver.GetPackageFilePath(packageIdentity.Id, packageIdentity.Version);
                var nupkgFileInfo = new FileInfo(nupkgPath);
                Assert.True(File.Exists(nupkgFileInfo.FullName));
                Assert.NotEqual(0, nupkgFileInfo.Length);

                var bnupkgPath = Path.Combine(packageVersionDirectory, "lib", "net45", "B.nupkg");
                var bnupkgFileInfo = new FileInfo(bnupkgPath);
                Assert.True(File.Exists(bnupkgFileInfo.FullName));
                Assert.Equal(0, bnupkgFileInfo.Length);
            }
        }

        [CIOnlyFact]
        public async Task Test_ExtractionHonorsFileTimestamp()
        {
            // Arrange
            var packageIdentity = new PackageIdentity("packageA", new NuGetVersion("2.0.3"));
            var entryModifiedTime = new DateTimeOffset(1985, 11, 20, 12, 0, 0, TimeSpan.FromHours(-7.0));
            DateTime expectedLastWriteTime = entryModifiedTime.DateTime.ToLocalTime();

            using (var packagesDirectory = TestDirectory.Create())
            {
                var pathResolver = new VersionFolderPathResolver(packagesDirectory);
                var packageFileInfo = await TestPackagesCore.GeneratePackageAsync(
                    packagesDirectory,
                    packageIdentity.Id,
                    packageIdentity.Version.ToNormalizedString(),
                    entryModifiedTime,
                    "lib/net45/A.dll");

                var packageExtractionContext = new PackageExtractionContext(
                     packageSaveMode: PackageSaveMode.Defaultv3,
                     xmlDocFileSaveMode: XmlDocFileSaveMode.None,
                     clientPolicyContext: null,
                     logger: NullLogger.Instance);

                var versionFolderPathResolver = new VersionFolderPathResolver(packagesDirectory);

                // Act
                using (var packageDownloader = new LocalPackageArchiveDownloader(
                    source: null,
                    packageFileInfo.FullName,
                    packageIdentity,
                    NullLogger.Instance))
                {
                    await PackageExtractor.InstallFromSourceAsync(
                         packageIdentity,
                         packageDownloader,
                         versionFolderPathResolver,
                         packageExtractionContext,
                         CancellationToken.None);
                }

                // Assert
                var packageVersionDirectory = pathResolver.GetInstallPath(packageIdentity.Id, packageIdentity.Version);
                AssertDirectoryExists(packageVersionDirectory);

                var dllPath = Path.Combine(packageVersionDirectory, "lib", "net45", "A.dll");
                var dllFileInfo = new FileInfo(dllPath);
                AssertFileExists(dllFileInfo.FullName);
                Assert.Equal(expectedLastWriteTime, dllFileInfo.LastWriteTime);
            }
        }

        [Fact]
        public async Task Test_ExtractionDoesNotExtractFiles_IfPackageSaveModeDoesNotIncludeFiles()
        {
            // Arrange
            var packageIdentity = new PackageIdentity("packageA", new NuGetVersion("2.0.3"));

            using (var packageFileInfo = TestPackagesCore.GetLegacyTestPackage())
            using (var packagesDirectory = TestDirectory.Create())
            {
                var pathResolver = new VersionFolderPathResolver(packagesDirectory);
                var packageExtractionContext = new PackageExtractionContext(
                     packageSaveMode: PackageSaveMode.Nuspec | PackageSaveMode.Nupkg,
                     xmlDocFileSaveMode: XmlDocFileSaveMode.None,
                     clientPolicyContext: null,
                     logger: NullLogger.Instance);

                var versionFolderPathResolver = new VersionFolderPathResolver(packagesDirectory);

                // Act
                using (var packageDownloader = new LocalPackageArchiveDownloader(
                    null,
                    packageFileInfo,
                    packageIdentity,
                    NullLogger.Instance))
                {
                    await PackageExtractor.InstallFromSourceAsync(
                          packageIdentity,
                          packageDownloader,
                          versionFolderPathResolver,
                          packageExtractionContext,
                          CancellationToken.None);
                }

                // Assert
                var packageVersionDirectory = pathResolver.GetInstallPath(packageIdentity.Id, packageIdentity.Version);

                AssertDirectoryExists(packageVersionDirectory);
                AssertFileExists(packageVersionDirectory, pathResolver.GetPackageFileName(packageIdentity.Id, packageIdentity.Version));
                AssertFileExists(packageVersionDirectory, pathResolver.GetManifestFileName(packageIdentity.Id, packageIdentity.Version));
                AssertFileExists(packageVersionDirectory, "packagea.2.0.3.nupkg.sha512");

                Assert.False(File.Exists(Path.Combine(packageVersionDirectory, "lib", "test.dll")));
            }
        }

        private static void AssertDirectoryExists(string path, string message = null)
        {
            Assert.True(Directory.Exists(path), message);
        }

        private static void AssertFileExists(params string[] paths)
        {
            Assert.True(File.Exists(Path.Combine(paths)));
        }

        private class StreamWrapperBase : Stream
        {
            protected readonly Stream _stream;

            public StreamWrapperBase(Stream stream)
            {
                _stream = stream;
            }

            public override bool CanRead
            {
                get
                {
                    return _stream.CanRead;
                }
            }

            public override bool CanSeek
            {
                get
                {
                    return _stream.CanSeek;
                }
            }

            public override bool CanWrite
            {
                get
                {
                    return _stream.CanWrite;
                }
            }

            public override long Length
            {
                get
                {
                    return _stream.Length;
                }
            }

            public override long Position
            {
                get
                {
                    return _stream.Position;
                }

                set
                {
                    _stream.Position = value;
                }
            }

            public override void Flush()
            {
                _stream.Flush();
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                return _stream.Read(buffer, offset, count);
            }

            public override long Seek(long offset, SeekOrigin origin)
            {
                return _stream.Seek(offset, origin);
            }

            public override void SetLength(long value)
            {
                _stream.SetLength(value);
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                _stream.Write(buffer, offset, count);
            }
        }

        private class CorruptionException : Exception
        {
        }

        private sealed class ThrowingPackageArchiveDownloader : IPackageDownloader
        {
            private bool _isDisposed;
            private readonly ILogger _logger;
            private readonly string _packageFilePath;
            private readonly PackageIdentity _packageIdentity;
            private Lazy<PackageArchiveReader> _packageReader;
            private Lazy<FileStream> _sourceStream;

            public IAsyncPackageContentReader ContentReader => _packageReader.Value;
            public IAsyncPackageCoreReader CoreReader => _packageReader.Value;

            public ISignedPackageReader SignedPackageReader => _packageReader.Value;

            public string Source { get; }

            internal ThrowingPackageArchiveDownloader(
                string source,
                string packageFilePath,
                PackageIdentity packageIdentity,
                ILogger logger)
            {
                _packageFilePath = packageFilePath;
                _packageIdentity = packageIdentity;
                _logger = logger;
                _packageReader = new Lazy<PackageArchiveReader>(GetPackageReader);
                _sourceStream = new Lazy<FileStream>(GetSourceStream);
                Source = source;
            }

            public void Dispose()
            {
                if (!_isDisposed)
                {
                    if (_packageReader.IsValueCreated)
                    {
                        _packageReader.Value.Dispose();
                    }

                    if (_sourceStream.IsValueCreated)
                    {
                        _sourceStream.Value.Dispose();
                    }

                    GC.SuppressFinalize(this);

                    _isDisposed = true;
                }
            }

            public async Task<bool> CopyNupkgFileToAsync(string destinationFilePath, CancellationToken cancellationToken)
            {
                using (var destination = new FileStream(
                    destinationFilePath,
                    FileMode.Create,
                    FileAccess.ReadWrite,
                    FileShare.ReadWrite | FileShare.Delete,
                    bufferSize: 4096,
                    useAsync: true))
                {
                    var maxBytes = 100;
                    var buffer = new byte[maxBytes];
                    var byteCount = _sourceStream.Value.Read(buffer, 0, maxBytes);

                    Assert.True(byteCount > 0);

                    await destination.WriteAsync(buffer, 0, byteCount, cancellationToken);

                    throw new CorruptionException();
                }
            }

            public Task<string> GetPackageHashAsync(string hashAlgorithm, CancellationToken cancellationToken)
            {
                _sourceStream.Value.Seek(0, SeekOrigin.Begin);

                var bytes = new CryptoHashProvider(hashAlgorithm).CalculateHash(_sourceStream.Value);
                var packageHash = Convert.ToBase64String(bytes);

                return Task.FromResult(packageHash);
            }

            public void SetExceptionHandler(Func<Exception, Task<bool>> handleExceptionAsync)
            {
            }

            public void SetThrottle(SemaphoreSlim throttle)
            {
            }

            private PackageArchiveReader GetPackageReader()
            {
                _sourceStream.Value.Seek(0, SeekOrigin.Begin);

                return new PackageArchiveReader(_sourceStream.Value);
            }

            private FileStream GetSourceStream()
            {
                return new FileStream(
                   _packageFilePath,
                   FileMode.Open,
                   FileAccess.Read,
                   FileShare.Read,
                   bufferSize: 4096,
                   useAsync: true);
            }
        }
    }
}
