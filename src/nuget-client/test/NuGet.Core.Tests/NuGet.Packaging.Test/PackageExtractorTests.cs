// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using FluentAssertions;
using Moq;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Packaging.Core;
using NuGet.Packaging.PackageExtraction;
using NuGet.Packaging.Signing;
using NuGet.Protocol.Plugins;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Test.Utility.Signing;
using Xunit;

namespace NuGet.Packaging.Test
{
    using LocalPackageArchiveDownloader = NuGet.Protocol.LocalPackageArchiveDownloader;

    [Collection(SigningTestsCollection.Name)]
    public class PackageExtractorTests
    {
        private static ClientPolicyContext _defaultContext = ClientPolicyContext.GetClientPolicy(NullSettings.Instance, NullLogger.Instance);

        private const string _emptyTrustedSignersList = "signatureValidationMode is set to require, so packages are allowed only if signed by trusted signers; however, no trusted signers were specified.";
        private const string _emptyRepoAllowList = "This repository indicated that all its packages are repository signed; however, it listed no signing certificates.";
        private const string _noMatchInTrustedSignersList = "This package is signed but not by a trusted signer.";
        private const string _noMatchInRepoAllowList = "This package was not repository signed with a certificate listed by this repository.";
        private const string _notSignedPackageRepo = "This repository indicated that all its packages are repository signed; however, this package is unsigned.";
        private const string _notSignedPackageRequire = "signatureValidationMode is set to require, so packages are allowed only if signed by trusted signers; however, this package is unsigned.";
        private const string SignatureVerificationEnvironmentVariable = "DOTNET_NUGET_SIGNATURE_VERIFICATION";
        private const string SignatureVerificationEnvironmentVariableTypo = "DOTNET_NUGET_SIGNATURE_VERIFICATIOn";
        private const string UntrustedChainCertError = "The author primary signature's signing certificate is not trusted by the trust provider.";

        [Fact]
        public async Task InstallFromSourceAsync_StressTestAsync()
        {
            // Arrange
            using (var root = TestDirectory.Create())
            {
                var identity = new PackageIdentity("PackageA", new NuGetVersion("2.0.0"));

                var sourcePath = Path.Combine(root, "source");
                var packagesPath = Path.Combine(root, "packages");
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(sourcePath, identity);
                var sourcePathResolver = new VersionFolderPathResolver(sourcePath);

                var sem = new ManualResetEventSlim(false);
                var installedBag = new ConcurrentBag<bool>();
                var hashBag = new ConcurrentBag<bool>();
                var nupkgMetadataBag = new ConcurrentBag<bool>();
                var tasks = new List<Task>();

                var limit = 100;

                for (var i = 0; i < limit; i++)
                {
                    var task = Task.Run(async () =>
                    {
                        using (var packageStream = File.OpenRead(sourcePathResolver.GetPackageFilePath(identity.Id, identity.Version)))
                        {
                            var pathContext = new PackageExtractionContext(
                                    packageSaveMode: PackageSaveMode.Nupkg,
                                    xmlDocFileSaveMode: XmlDocFileSaveMode.None,
                                    clientPolicyContext: null,
                                    logger: NullLogger.Instance);

                            var pathResolver = new VersionFolderPathResolver(packagesPath);
                            var hashPath = pathResolver.GetHashPath(identity.Id, identity.Version);
                            var nupkgMetadataPath = pathResolver.GetNupkgMetadataPath(identity.Id, identity.Version);

                            sem.Wait();

                            using (var packageDownloader = new LocalPackageArchiveDownloader(
                                sourcePath,
                                sourcePathResolver.GetPackageFilePath(identity.Id, identity.Version),
                                identity,
                                NullLogger.Instance))
                            {
                                var installed = await PackageExtractor.InstallFromSourceAsync(
                                    identity,
                                    packageDownloader,
                                    pathResolver,
                                    pathContext,
                                    CancellationToken.None);

                                var exists = File.Exists(hashPath);

                                installedBag.Add(installed);
                                hashBag.Add(exists);
                                nupkgMetadataBag.Add(File.Exists(nupkgMetadataPath));
                            }
                        }
                    });

                    tasks.Add(task);
                }

                // Act
                sem.Set();
                await Task.WhenAll(tasks);

                // Assert
                Assert.Equal(limit, installedBag.Count);
                Assert.Equal(limit, hashBag.Count);
                Assert.Equal(limit, nupkgMetadataBag.Count);
                Assert.Equal(1, installedBag.Count(b => b == true));
                Assert.Equal(limit, hashBag.Count(b => b == true));
                Assert.Equal(limit, nupkgMetadataBag.Count(b => b == true));
            }
        }

        [Fact]
        public async Task InstallFromSourceAsync_ReturnsFalseWhenAlreadyInstalledAsync()
        {
            // Arrange
            using (var root = TestDirectory.Create())
            {
                var identity = new PackageIdentity("PackageA", new NuGetVersion("2.0.3-Beta"));

                var sourcePath = Path.Combine(root, "source");
                Directory.CreateDirectory(sourcePath);
                var packageFileInfo = await TestPackagesCore.GeneratePackageAsync(
                   sourcePath,
                   identity.Id,
                   identity.Version.ToString(),
                   DateTimeOffset.UtcNow.LocalDateTime,
                   "lib/net45/A.dll");

                var packagesPath = Path.Combine(root, "packages");
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(packagesPath, identity);
                var pathResolver = new VersionFolderPathResolver(packagesPath);

                using (var packageDownloader = new LocalPackageArchiveDownloader(
                    sourcePath,
                    packageFileInfo.FullName,
                    identity,
                    NullLogger.Instance))
                {
                    // Act
                    var installed = await PackageExtractor.InstallFromSourceAsync(
                        identity,
                        packageDownloader,
                        pathResolver,
                        new PackageExtractionContext(
                            packageSaveMode: PackageSaveMode.Nupkg,
                            xmlDocFileSaveMode: XmlDocFileSaveMode.None,
                            clientPolicyContext: null,
                            logger: NullLogger.Instance),
                        CancellationToken.None);

                    // Assert
                    Assert.False(installed);
                }
            }
        }

        [Fact]
        public async Task InstallFromSourceAsync_ReturnsTrueAfterNewInstallAsync()
        {
            // Arrange
            using (var root = TestDirectory.Create())
            {
                var identity = new PackageIdentity("PackageA", new NuGetVersion("2.0.3-Beta"));

                var sourcePath = Path.Combine(root, "source");
                Directory.CreateDirectory(sourcePath);
                var packageFileInfo = await TestPackagesCore.GeneratePackageAsync(
                    sourcePath,
                    identity.Id,
                    identity.Version.ToString(),
                    DateTimeOffset.UtcNow.LocalDateTime,
                    "lib/net45/A.dll");

                var packagesPath = Path.Combine(root, "packages");
                var pathResolver = new VersionFolderPathResolver(packagesPath);

                using (var packageDownloader = new LocalPackageArchiveDownloader(
                    sourcePath,
                    packageFileInfo.FullName,
                    identity,
                    NullLogger.Instance))
                {
                    // Act
                    var installed = await PackageExtractor.InstallFromSourceAsync(
                         identity,
                         packageDownloader,
                         pathResolver,
                         new PackageExtractionContext(
                             packageSaveMode: PackageSaveMode.Nupkg,
                             xmlDocFileSaveMode: XmlDocFileSaveMode.None,
                             clientPolicyContext: null,
                             logger: NullLogger.Instance),
                         CancellationToken.None);

                    // Assert
                    Assert.True(installed);
                }
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task InstallFromSourceAsync_WithLowercaseSpecified_ExtractsToSpecifiedCaseAsync(bool isLowercase)
        {
            // Arrange
            using (var root = TestDirectory.Create())
            {
                var identity = new PackageIdentity("PackageA", new NuGetVersion("2.0.3-Beta"));

                var sourcePath = Path.Combine(root, "source");
                Directory.CreateDirectory(sourcePath);
                var packageFileInfo = await TestPackagesCore.GeneratePackageAsync(
                   sourcePath,
                   identity.Id,
                   identity.Version.ToString(),
                   DateTimeOffset.UtcNow.LocalDateTime,
                   "lib/net45/A.dll");

                var packagesPath = Path.Combine(root, "packages");
                var pathResolver = new VersionFolderPathResolver(packagesPath, isLowercase);

                using (var packageDownloader = new LocalPackageArchiveDownloader(
                    sourcePath,
                    packageFileInfo.FullName,
                    identity,
                    NullLogger.Instance))
                {
                    // Act
                    await PackageExtractor.InstallFromSourceAsync(
                          identity,
                          packageDownloader,
                          pathResolver,
                          new PackageExtractionContext(
                              packageSaveMode: PackageSaveMode.Nupkg,
                              xmlDocFileSaveMode: XmlDocFileSaveMode.None,
                              clientPolicyContext: null,
                              logger: NullLogger.Instance),
                          CancellationToken.None);

                    // Assert
                    Assert.True(File.Exists(pathResolver.GetPackageFilePath(identity.Id, identity.Version)), "The .nupkg should exist.");
                }
            }
        }

        [Fact]
        public async Task InstallFromSourceAsync_NuspecWithDifferentName_InstallsForV3Async()
        {
            // Arrange
            using (var root = TestDirectory.Create())
            {
                var packageA = new SimpleTestPackageContext()
                {
                    Id = "a",
                    Version = "1.0.0"
                };

                packageA.AddFile("lib/net45/a.dll");

                await SimpleTestPackageUtility.CreatePackagesAsync(root, packageA);

                var packageFile = Path.Combine(root, "a.1.0.0.nupkg");

                // Move a.nuspec to b.nuspec
                using (var stream = File.Open(packageFile, FileMode.Open))
                using (var zipFile = new ZipArchive(stream, ZipArchiveMode.Update))
                {
                    var nuspecEntry = zipFile.Entries.Where(e => e.FullName.EndsWith(".nuspec")).Single();

                    using (var nuspecStream = nuspecEntry.Open())
                    using (var reader = new StreamReader(nuspecStream))
                    {
                        zipFile.AddEntry("b.nuspec", reader.ReadToEnd());
                    }

                    nuspecEntry.Delete();
                }

                var packageIdentity = new PackageIdentity("a", NuGetVersion.Parse("1.0.0"));
                var pathResolver = new VersionFolderPathResolver(root);

                using (var packageDownloader = new LocalPackageArchiveDownloader(
                    root,
                    packageFile,
                    packageIdentity,
                    NullLogger.Instance))
                {
                    // Act
                    await PackageExtractor.InstallFromSourceAsync(
                        packageIdentity,
                        packageDownloader,
                        pathResolver,
                        new PackageExtractionContext(
                            packageSaveMode: PackageSaveMode.Defaultv3,
                            xmlDocFileSaveMode: XmlDocFileSaveMode.None,
                            clientPolicyContext: null,
                            logger: NullLogger.Instance),
                        CancellationToken.None);

                    // Assert
                    Assert.False(File.Exists(Path.Combine(root, "a", "1.0.0", "b.nuspec")));
                    Assert.True(File.Exists(Path.Combine(root, "a", "1.0.0", "a.nuspec")));
                }
            }
        }

        [Fact]
        public async Task InstallFromSourceAsync_NupkgWithDifferentName_InstallsForV3Async()
        {
            // Arrange
            using (var root = TestDirectory.Create())
            {
                var packageA = new SimpleTestPackageContext()
                {
                    Id = "a",
                    Version = "1.0.0"
                };

                packageA.AddFile("lib/net45/a.dll");
                packageA.AddFile("b.1.0.0.nupkg");

                await SimpleTestPackageUtility.CreatePackagesAsync(root, packageA);

                var packageFile = Path.Combine(root, "a.1.0.0.nupkg");
                var packageIdentity = new PackageIdentity("a", NuGetVersion.Parse("1.0.0"));
                var pathResolver = new VersionFolderPathResolver(root);

                using (var packageDownloader = new LocalPackageArchiveDownloader(
                    root,
                    packageFile,
                    packageIdentity,
                    NullLogger.Instance))
                {
                    // Act
                    await PackageExtractor.InstallFromSourceAsync(
                        packageIdentity,
                        packageDownloader,
                        pathResolver,
                        new PackageExtractionContext(
                            packageSaveMode: PackageSaveMode.Defaultv3,
                            xmlDocFileSaveMode: XmlDocFileSaveMode.None,
                            clientPolicyContext: null,
                            logger: NullLogger.Instance),
                        CancellationToken.None);

                    // Assert
                    Assert.False(File.Exists(Path.Combine(root, "a", "1.0.0", "b.1.0.0.nupkg")));
                    Assert.True(File.Exists(Path.Combine(root, "a", "1.0.0", "a.1.0.0.nupkg")));
                }
            }
        }

        [Fact]
        public async Task ExtractPackageAsync_WithContentXmlFileAsync()
        {
            // Arrange
            using (var packageStream = TestPackagesCore.GetTestPackageWithContentXmlFile())
            using (var root = TestDirectory.Create())
            using (var packageReader = new PackageArchiveReader(packageStream))
            {
                var resolver = new PackagePathResolver(root);
                var identity = new PackageIdentity("packageA", new NuGetVersion("2.0.3"));

                // Act
                var files = await PackageExtractor.ExtractPackageAsync(
                    null,
                    packageReader,
                    packageStream,
                    resolver,
                    new PackageExtractionContext(
                        PackageSaveMode.Defaultv2,
                        PackageExtractionBehavior.XmlDocFileSaveMode,
                        clientPolicyContext: null,
                        logger: NullLogger.Instance),
                    CancellationToken.None);

                // Assert
                var packagePath = resolver.GetInstallPath(identity);
                Assert.DoesNotContain(Path.Combine(packagePath, "[Content_Types].xml"), files);
                Assert.Contains(Path.Combine(packagePath, "content", "[Content_Types].xml"), files);
            }
        }

        [Fact]
        public async Task ExtractPackageAsync_DuplicateNupkgAsync()
        {
            using (var packageFile = TestPackagesCore.GetLegacyTestPackage())
            {
                using (var root = TestDirectory.Create())
                using (var packageFolder = TestDirectory.Create())
                {
                    using (var stream = File.OpenRead(packageFile))
                    using (var zipFile = new ZipArchive(stream))
                    {
                        zipFile.ExtractAll(packageFolder);
                    }

                    using (var stream = File.OpenRead(packageFile))
                    using (var folderReader = new PackageFolderReader(packageFolder))
                    {
                        // Act
                        var files = await PackageExtractor.ExtractPackageAsync(
                            packageFolder.Path,
                            folderReader,
                            stream,
                            new PackagePathResolver(root),
                            new PackageExtractionContext(
                                PackageSaveMode.Defaultv2,
                                PackageExtractionBehavior.XmlDocFileSaveMode,
                                clientPolicyContext: null,
                                logger: NullLogger.Instance),
                            CancellationToken.None);

                        // Assert
                        Assert.Equal(1, files.Where(p => p.EndsWith(".nupkg")).Count());
                    }
                }
            }
        }

        [Fact]
        public async Task ExtractPackageAsync_NupkgContentAsync()
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
                   DateTimeOffset.UtcNow.LocalDateTime,
                   "content/A.nupkg");

                using (var packageStream = File.OpenRead(packageFileInfo.FullName))
                {
                    var packageExtractionContext = new PackageExtractionContext(
                        PackageSaveMode.Defaultv2,
                        XmlDocFileSaveMode.None,
                        clientPolicyContext: null,
                        logger: NullLogger.Instance);

                    // Act
                    var packageFiles = PackageExtractor.ExtractPackageAsync(
                        root,
                        packageStream,
                        resolver,
                        packageExtractionContext,
                        CancellationToken.None);

                    // Assert
                    var installPath = resolver.GetInstallPath(identity);
                    Assert.True(File.Exists(Path.Combine(installPath, "content", "A.nupkg")));
                }
            }
        }

        [Fact]
        public async Task ExtractPackageAsync_PackageSaveModeNupkg_FolderReaderAsync()
        {
            // Arrange
            using (var packageFile = TestPackagesCore.GetLegacyTestPackage())
            using (var root = TestDirectory.Create())
            using (var packageFolder = TestDirectory.Create())
            {
                using (var packageStream = File.OpenRead(packageFile))
                using (var zipFile = new ZipArchive(packageStream))
                using (var folderReader = new PackageFolderReader(packageFolder))
                {
                    zipFile.ExtractAll(packageFolder);

                    var packageExtractionContext = new PackageExtractionContext(
                        PackageSaveMode.Nupkg,
                        PackageExtractionBehavior.XmlDocFileSaveMode,
                        clientPolicyContext: null,
                        logger: NullLogger.Instance);

                    // Act
                    var files = await PackageExtractor.ExtractPackageAsync(packageFolder,
                                                                           folderReader,
                                                                           packageStream,
                                                                           new PackagePathResolver(root),
                                                                           packageExtractionContext,
                                                                           CancellationToken.None);

                    // Assert
                    Assert.Contains(files, p => p.EndsWith(".nupkg"));
                    Assert.DoesNotContain(files, p => p.EndsWith(".nuspec"));
                }
            }
        }

        [Fact]
        public async Task ExtractPackageAsync_PackageSaveModeNuspec_FolderReaderAsync()
        {
            // Arrange
            using (var packageFile = TestPackagesCore.GetLegacyTestPackage())
            using (var root = TestDirectory.Create())
            using (var packageFolder = TestDirectory.Create())
            {
                using (var packageStream = File.OpenRead(packageFile))
                using (var zipFile = new ZipArchive(packageStream))
                using (var folderReader = new PackageFolderReader(packageFolder))
                {
                    zipFile.ExtractAll(packageFolder);

                    var packageExtractionContext = new PackageExtractionContext(
                        PackageSaveMode.Nuspec,
                        PackageExtractionBehavior.XmlDocFileSaveMode,
                        clientPolicyContext: null,
                        logger: NullLogger.Instance);

                    // Act
                    var files = await PackageExtractor.ExtractPackageAsync(packageFolder,
                                                                         folderReader,
                                                                         packageStream,
                                                                         new PackagePathResolver(root),
                                                                         packageExtractionContext,
                                                                         CancellationToken.None);

                    // Assert
                    Assert.DoesNotContain(files, p => p.EndsWith(".nupkg"));
                    Assert.Contains(files, p => p.EndsWith(".nuspec"));
                }
            }
        }

        [Fact]
        public async Task ExtractPackageAsync_PackageSaveModeNuspecAndNupkg_PackageStreamAsync()
        {
            // Arrange
            using (var packageFile = TestPackagesCore.GetLegacyTestPackage())
            using (var root = TestDirectory.Create())
            using (var packageFolder = TestDirectory.Create())
            {
                using (var packageStream = File.OpenRead(packageFile))
                using (var zipFile = new ZipArchive(packageStream))
                using (var folderReader = new PackageFolderReader(packageFolder))
                {
                    zipFile.ExtractAll(packageFolder);

                    var packageExtractionContext = new PackageExtractionContext(
                        PackageSaveMode.Nuspec | PackageSaveMode.Nupkg,
                        PackageExtractionBehavior.XmlDocFileSaveMode,
                        clientPolicyContext: null,
                        logger: NullLogger.Instance);

                    // Act
                    var files = await PackageExtractor.ExtractPackageAsync(packageFolder,
                                                                         folderReader,
                                                                         packageStream,
                                                                         new PackagePathResolver(root),
                                                                         packageExtractionContext,
                                                                         CancellationToken.None);

                    // Assert
                    Assert.Contains(files, p => p.EndsWith(".nupkg"));
                    Assert.Contains(files, p => p.EndsWith(".nuspec"));
                }
            }
        }

        [Fact]
        public async Task ExtractPackageAsync_DefaultPackageExtractionContextAsync()
        {
            // Arrange
            using (var root = TestDirectory.Create())
            {
                var resolver = new PackagePathResolver(root);
                var identity = new PackageIdentity("A", new NuGetVersion("2.0.3"));
                var satelliteIdentity = new PackageIdentity(identity.Id + ".fr", identity.Version);
                var packageFileInfo = await TestPackagesCore.GetRuntimePackageAsync(root, identity.Id, identity.Version.ToString());
                var satellitePackageInfo = await TestPackagesCore.GetSatellitePackageAsync(root, identity.Id, identity.Version.ToString(), "fr");

                using (var packageStream = File.OpenRead(packageFileInfo.FullName))
                using (var satellitePackageStream = File.OpenRead(satellitePackageInfo.FullName))
                {
                    var packageExtractionContext = new PackageExtractionContext(
                                PackageSaveMode.Defaultv2,
                                PackageExtractionBehavior.XmlDocFileSaveMode,
                                clientPolicyContext: null,
                                logger: NullLogger.Instance);

                    // Act
                    var packageFiles = PackageExtractor.ExtractPackageAsync(root,
                                                                     packageStream,
                                                                     resolver,
                                                                     packageExtractionContext,
                                                                     CancellationToken.None);

                    var satellitePackageFiles = PackageExtractor.ExtractPackageAsync(root,
                                                                     satellitePackageStream,
                                                                     resolver,
                                                                     packageExtractionContext,
                                                                     CancellationToken.None);

                    var pathToAFrDllInSatellitePackage
                        = Path.Combine(resolver.GetInstallPath(satelliteIdentity), "lib", "net45", "fr", "A.resources.dll");
                    var pathToAFrDllInRunTimePackage
                        = Path.Combine(resolver.GetInstallPath(identity), "lib", "net45", "fr", "A.resources.dll");

                    Assert.True(File.Exists(pathToAFrDllInSatellitePackage));
                    Assert.True(File.Exists(pathToAFrDllInRunTimePackage));
                }
            }
        }

        [Fact]
        public async Task ExtractPackageAsync_ExtractsXmlFiles_IfXmlSaveModeIsSetToNoneAsync()
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
                   DateTimeOffset.UtcNow.LocalDateTime,
                   "lib/net45/A.dll",
                   "lib/net45/A.xml",
                   "lib/net45/appconfig.xml");

                using (var packageStream = File.OpenRead(packageFileInfo.FullName))
                {
                    var packageExtractionContext = new PackageExtractionContext(
                                PackageSaveMode.Defaultv2,
                                XmlDocFileSaveMode.None,
                                clientPolicyContext: null,
                                logger: NullLogger.Instance);

                    // Act
                    var packageFiles = PackageExtractor.ExtractPackageAsync(
                        root,
                        packageStream,
                        resolver,
                        packageExtractionContext,
                        CancellationToken.None);

                    // Assert
                    var installPath = resolver.GetInstallPath(identity);
                    Assert.True(File.Exists(Path.Combine(installPath, "lib", "net45", "A.dll")));
                    Assert.True(File.Exists(Path.Combine(installPath, "lib", "net45", "A.xml")));
                    Assert.True(File.Exists(Path.Combine(installPath, "lib", "net45", "appconfig.xml")));
                }
            }
        }

        [Fact]
        public async Task ExtractPackageAsync_CompressesXmlFiles_IfXmlSaveModeIsSetToCompressAsync()
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
                   DateTimeOffset.UtcNow.LocalDateTime,
                   "lib/net45/A.dll",
                   "lib/net45/A.xml",
                   "lib/net45/appconfig.xml");

                using (var packageStream = File.OpenRead(packageFileInfo.FullName))
                {
                    var packageExtractionContext = new PackageExtractionContext(
                                PackageSaveMode.Defaultv2,
                                XmlDocFileSaveMode.Compress,
                                clientPolicyContext: null,
                                logger: NullLogger.Instance);

                    // Act
                    var packageFiles = PackageExtractor.ExtractPackageAsync(
                        root,
                        packageStream,
                        resolver,
                        packageExtractionContext,
                        CancellationToken.None);

                    // Assert
                    var installPath = resolver.GetInstallPath(identity);
                    var xmlZip = Path.Combine(installPath, "lib", "net45", "A.xml.zip");
                    Assert.True(File.Exists(Path.Combine(installPath, "lib", "net45", "A.dll")));
                    Assert.False(File.Exists(Path.Combine(installPath, "lib", "net45", "A.xml")));
                    Assert.True(File.Exists(xmlZip));
                    Assert.True(File.Exists(Path.Combine(installPath, "lib", "net45", "appconfig.xml")));

                    // Verify the zip has a A.xml in it.
                    using (var fileStream = File.OpenRead(xmlZip))
                    using (var archive = new ZipArchive(fileStream))
                    {
                        var entry = Assert.Single(archive.Entries);
                        Assert.Equal("A.xml", entry.Name);
                    }
                }
            }
        }

        [Fact]
        public async Task ExtractPackageAsync_CompressesXmlFilesForLanguageSpecificDirectoriesAsync()
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
                   DateTimeOffset.UtcNow.LocalDateTime,
                   "ref/dotnet5.4/A.xml",
                   "ref/dotnet5.4/fr/B.xml",
                   "ref/dotnet5.4/fr/B.resources.dll",
                   "ref/dotnet5.4/zh-hans/A.xml",
                   "ref/dotnet5.4/ru/C.xml");

                using (var packageStream = File.OpenRead(packageFileInfo.FullName))
                {
                    var packageExtractionContext = new PackageExtractionContext(
                                PackageSaveMode.Defaultv2,
                                XmlDocFileSaveMode.Compress,
                                clientPolicyContext: null,
                                logger: NullLogger.Instance);

                    // Act
                    var packageFiles = PackageExtractor.ExtractPackageAsync(
                        root,
                        packageStream,
                        resolver,
                        packageExtractionContext,
                        CancellationToken.None);

                    // Assert
                    var packageRoot = Path.Combine(resolver.GetInstallPath(identity), "ref", "dotnet5.4");
                    Assert.False(File.Exists(Path.Combine(packageRoot, "fr", "B.xml")));
                    Assert.True(File.Exists(Path.Combine(packageRoot, "fr", "B.xml.zip")));
                    Assert.False(File.Exists(Path.Combine(packageRoot, "zh-hans", "A.xml.zip")));
                    Assert.True(File.Exists(Path.Combine(packageRoot, "ru", "C.xml")));
                    Assert.False(File.Exists(Path.Combine(packageRoot, "ru", "C.xml.zip")));
                }
            }
        }

        [Fact]
        public async Task ExtractPackageAsync_SkipsXmlFiles_IfXmlSaveModeIsSetToSkipAsync()
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
                   DateTimeOffset.UtcNow.LocalDateTime,
                   "lib/net45/A.dll",
                   "lib/net45/A.xml",
                   "lib/net45/appconfig.xml");

                using (var packageStream = File.OpenRead(packageFileInfo.FullName))
                {
                    var packageExtractionContext = new PackageExtractionContext(
                                PackageSaveMode.Defaultv2,
                                XmlDocFileSaveMode.Skip,
                                clientPolicyContext: null,
                                logger: NullLogger.Instance);

                    // Act
                    var packageFiles = PackageExtractor.ExtractPackageAsync(
                        root,
                        packageStream,
                        resolver,
                        packageExtractionContext,
                        CancellationToken.None);

                    // Assert
                    var installPath = resolver.GetInstallPath(identity);
                    Assert.True(File.Exists(Path.Combine(installPath, "lib", "net45", "A.dll")));
                    Assert.False(File.Exists(Path.Combine(installPath, "lib", "net45", "A.xml")));
                    Assert.False(File.Exists(Path.Combine(installPath, "lib", "net45", "A.xml.zip")));
                    Assert.True(File.Exists(Path.Combine(installPath, "lib", "net45", "appconfig.xml")));
                }
            }
        }

        [Fact]
        public async Task ExtractPackageAsync_SkipsXmlFiles_ForLanguageSpecificDirectoriesAsync()
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
                   DateTimeOffset.UtcNow.LocalDateTime,
                   "ref/portable-net40+win8+netcore/A.dll",
                   "ref/portable-net40+win8+netcore/A.xml",
                   "ref/portable-net40+win8+netcore/fr/B.xml",
                   "ref/portable-net40+win8+netcore/fr/B.resources.dll",
                   "ref/portable-net40+win8+netcore/zh-hans/A.xml");

                using (var packageStream = File.OpenRead(packageFileInfo.FullName))
                {
                    var packageExtractionContext = new PackageExtractionContext(
                               PackageSaveMode.Defaultv2,
                               XmlDocFileSaveMode.Skip,
                               clientPolicyContext: null,
                               logger: NullLogger.Instance);

                    // Act
                    var packageFiles = PackageExtractor.ExtractPackageAsync(
                        root,
                        packageStream,
                        resolver,
                        packageExtractionContext,
                        CancellationToken.None);

                    // Assert
                    var packageRoot = Path.Combine(resolver.GetInstallPath(identity), "ref", "portable-net40+win8+netcore");
                    Assert.True(File.Exists(Path.Combine(packageRoot, "A.dll")));
                    Assert.False(File.Exists(Path.Combine(packageRoot, "A.xml")));
                    Assert.False(File.Exists(Path.Combine(packageRoot, "A.xml.zip")));
                    Assert.False(File.Exists(Path.Combine(packageRoot, "B.xml")));
                    Assert.False(File.Exists(Path.Combine(packageRoot, "B.xml.zip")));
                    Assert.True(File.Exists(Path.Combine(packageRoot, "fr", "B.resources.dll")));
                    Assert.False(File.Exists(Path.Combine(packageRoot, "fr", "B.xml")));
                    Assert.False(File.Exists(Path.Combine(packageRoot, "A.xml")));
                    Assert.False(File.Exists(Path.Combine(packageRoot, "A.xml.zip")));
                }
            }
        }

        [Fact]
        public async Task ExtractPackageAsync_SkipsSatelliteXmlFilesAsync()
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
                   DateTimeOffset.UtcNow.LocalDateTime,
                   "lib/net45/A.dll",
                   "lib/net45/A.xml");
                var satelliteIdentity = new PackageIdentity(identity.Id + ".fr", identity.Version);
                var satellitePackageInfo = await TestPackagesCore.GeneratePackageAsync(
                   root,
                   satelliteIdentity.Id,
                   satelliteIdentity.Version.ToString(),
                   language: "fr",
                   entryModifiedTime: DateTimeOffset.UtcNow.LocalDateTime,
                   zipEntries: new[] { "lib/net45/fr/A.resources.dll", "lib/net45/fr/A.xml" });

                using (var packageStream = File.OpenRead(packageFileInfo.FullName))
                using (var satellitePackageStream = File.OpenRead(satellitePackageInfo.FullName))
                {
                    var packageExtractionContext = new PackageExtractionContext(
                               PackageSaveMode.Defaultv2,
                               XmlDocFileSaveMode.Skip,
                               clientPolicyContext: null,
                               logger: NullLogger.Instance);

                    // Act
                    var packageFiles = PackageExtractor.ExtractPackageAsync(
                        root,
                        packageStream,
                        resolver,
                        packageExtractionContext,
                        CancellationToken.None);

                    var satellitePackageFiles = PackageExtractor.ExtractPackageAsync(
                        root,
                        satellitePackageStream,
                        resolver,
                        packageExtractionContext,
                        CancellationToken.None);

                    // Assert
                    var installPath = resolver.GetInstallPath(identity);
                    var satelliteInstallPath = resolver.GetInstallPath(satelliteIdentity);
                    Assert.False(File.Exists(Path.Combine(installPath, "lib", "net45", "A.xml")));
                    Assert.False(File.Exists(Path.Combine(installPath, "lib", "net45", "A.xml.zip")));
                    Assert.False(File.Exists(Path.Combine(satelliteInstallPath, "lib", "net45", "fr", "A.xml")));
                    Assert.False(File.Exists(Path.Combine(satelliteInstallPath, "lib", "net45", "fr", "A.xml.zip")));
                    Assert.True(File.Exists(Path.Combine(installPath, "lib", "net45", "A.dll")));
                    Assert.True(File.Exists(Path.Combine(installPath, "lib", "net45", "fr", "A.resources.dll")));
                }
            }
        }

        [Fact]
        public async Task ExtractPackageAsync_WithXmlModeCompress_DoesNotThrowIfPackageAlreadyContainsAXmlZipFileAsync()
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
                   DateTimeOffset.UtcNow.LocalDateTime,
                   "lib/net45/A.dll",
                   "lib/net45/A.xml",
                   "lib/net45/A.xml.zip");

                using (var packageStream = File.OpenRead(packageFileInfo.FullName))
                {
                    var packageExtractionContext = new PackageExtractionContext(
                               PackageSaveMode.Defaultv2,
                               XmlDocFileSaveMode.Compress,
                               clientPolicyContext: null,
                               logger: NullLogger.Instance);

                    // Act
                    var packageFiles = PackageExtractor.ExtractPackageAsync(
                        root,
                        packageStream,
                        resolver,
                        packageExtractionContext,
                        CancellationToken.None);

                    // Assert
                    var installPath = resolver.GetInstallPath(identity);
                    Assert.True(File.Exists(Path.Combine(installPath, "lib", "net45", "A.xml.zip")));
                    Assert.True(File.Exists(Path.Combine(installPath, "lib", "net45", "A.dll")));

                    // If we got this far, extraction did not throw.
                }
            }
        }

        [Fact]
        public async Task ExtractPackageAsync_WithXmlModeSkip_DoesNotSkipXmlZipFileAsync()
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
                   DateTimeOffset.UtcNow.LocalDateTime,
                   "lib/net45/A.dll",
                   "lib/net45/A.xml",
                   "lib/net45/A.xml.zip");

                using (var packageStream = File.OpenRead(packageFileInfo.FullName))
                {
                    var packageExtractionContext = new PackageExtractionContext(
                               PackageSaveMode.Defaultv2,
                               XmlDocFileSaveMode.Compress,
                               clientPolicyContext: null,
                               logger: NullLogger.Instance);

                    // Act
                    var packageFiles = PackageExtractor.ExtractPackageAsync(
                        root,
                        packageStream,
                        resolver,
                        packageExtractionContext,
                        CancellationToken.None);

                    // Assert
                    var installPath = resolver.GetInstallPath(identity);
                    Assert.True(File.Exists(Path.Combine(installPath, "lib", "net45", "A.xml.zip")));
                    Assert.True(File.Exists(Path.Combine(installPath, "lib", "net45", "A.dll")));
                }
            }
        }

        [Fact]
        public async Task ExtractPackageAsync_WithoutPackageSaveModeFile_DoesNotExtractFilesAsync()
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
                   DateTimeOffset.UtcNow.LocalDateTime,
                   "lib/net45/A.dll",
                   "content/net40/B.txt");

                using (var packageStream = File.OpenRead(packageFileInfo.FullName))
                {
                    var packageExtractionContext = new PackageExtractionContext(
                               PackageSaveMode.Nupkg | PackageSaveMode.Nuspec,
                               PackageExtractionBehavior.XmlDocFileSaveMode,
                               clientPolicyContext: null,
                               logger: NullLogger.Instance);

                    // Act
                    var packageFiles = PackageExtractor.ExtractPackageAsync(
                        root,
                        packageStream,
                        resolver,
                        packageExtractionContext,
                        CancellationToken.None);

                    // Assert
                    var installPath = resolver.GetInstallPath(identity);
                    Assert.True(File.Exists(Path.Combine(installPath, resolver.GetPackageFileName(identity))));
                    Assert.True(File.Exists(Path.Combine(installPath, resolver.GetManifestFileName(identity))));
                    Assert.False(File.Exists(Path.Combine(installPath, "lib", "net45", "A.dll")));
                    Assert.False(File.Exists(Path.Combine(installPath, "content", "net40", "B.txt")));
                }
            }
        }

        [Fact]
        public async Task ExtractPackageAsync_WithoutPackageSaveModeNuspec_DoesNotExtractNuspecAsync()
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
                   DateTimeOffset.UtcNow.LocalDateTime,
                   "lib/net45/A.dll",
                   "content/net40/B.txt");

                using (var packageStream = File.OpenRead(packageFileInfo.FullName))
                {
                    var packageExtractionContext = new PackageExtractionContext(
                               PackageSaveMode.Nupkg | PackageSaveMode.Files,
                               PackageExtractionBehavior.XmlDocFileSaveMode,
                               clientPolicyContext: null,
                               logger: NullLogger.Instance);

                    // Act
                    var packageFiles = PackageExtractor.ExtractPackageAsync(
                        root,
                        packageStream,
                        resolver,
                        packageExtractionContext,
                        CancellationToken.None);

                    // Assert
                    var installPath = resolver.GetInstallPath(identity);
                    Assert.True(File.Exists(Path.Combine(installPath, resolver.GetPackageFileName(identity))));
                    Assert.False(File.Exists(Path.Combine(installPath, resolver.GetManifestFileName(identity))));
                    Assert.True(File.Exists(Path.Combine(installPath, "lib", "net45", "A.dll")));
                    Assert.True(File.Exists(Path.Combine(installPath, "content", "net40", "B.txt")));
                }
            }
        }

        [Fact]
        public async Task ExtractPackageAsync_WithPackageSaveModeNuspec_ExtractsInnerNuspecAsync()
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
                   DateTimeOffset.UtcNow.LocalDateTime,
                   "lib/net45/A.dll",
                   "content/net40/B.nuspec");

                using (var packageStream = File.OpenRead(packageFileInfo.FullName))
                {
                    var packageExtractionContext = new PackageExtractionContext(
                               PackageSaveMode.Nupkg | PackageSaveMode.Files | PackageSaveMode.Nuspec,
                               PackageExtractionBehavior.XmlDocFileSaveMode,
                               clientPolicyContext: null,
                               logger: NullLogger.Instance);

                    // Act
                    var packageFiles = PackageExtractor.ExtractPackageAsync(
                        root,
                        packageStream,
                        resolver,
                        packageExtractionContext,
                        CancellationToken.None);

                    // Assert
                    var installPath = resolver.GetInstallPath(identity);
                    Assert.True(File.Exists(Path.Combine(installPath, resolver.GetPackageFileName(identity))));
                    Assert.True(File.Exists(Path.Combine(installPath, resolver.GetManifestFileName(identity))));
                    Assert.True(File.Exists(Path.Combine(installPath, "lib", "net45", "A.dll")));
                    Assert.True(File.Exists(Path.Combine(installPath, "content", "net40", "B.nuspec")));
                }
            }
        }

        [Fact]
        public async Task ExtractPackageAsync_WithoutPackageSaveModeNuspec_ExtractsInnerNuspecAsync()
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
                   DateTimeOffset.UtcNow.LocalDateTime,
                   "lib/net45/A.dll",
                   "content/net40/B.nuspec");

                using (var packageStream = File.OpenRead(packageFileInfo.FullName))
                {
                    var packageExtractionContext = new PackageExtractionContext(
                                PackageSaveMode.Nupkg | PackageSaveMode.Files,
                                PackageExtractionBehavior.XmlDocFileSaveMode,
                                clientPolicyContext: null,
                                logger: NullLogger.Instance);

                    // Act
                    var packageFiles = PackageExtractor.ExtractPackageAsync(
                        root,
                        packageStream,
                        resolver,
                        packageExtractionContext,
                        CancellationToken.None);

                    // Assert
                    var installPath = resolver.GetInstallPath(identity);
                    Assert.True(File.Exists(Path.Combine(installPath, resolver.GetPackageFileName(identity))));
                    Assert.False(File.Exists(Path.Combine(installPath, resolver.GetManifestFileName(identity))));
                    Assert.True(File.Exists(Path.Combine(installPath, "lib", "net45", "A.dll")));
                    Assert.True(File.Exists(Path.Combine(installPath, "content", "net40", "B.nuspec")));
                }
            }
        }

        [Fact]
        public async Task ExtractPackageAsync_WithoutPackageSaveModeNupkg_DoesNotExtractNupkgAsync()
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
                   DateTimeOffset.UtcNow.LocalDateTime,
                   "lib/net45/A.dll",
                   "content/net40/B.txt");

                using (var packageStream = File.OpenRead(packageFileInfo.FullName))
                {
                    var packageExtractionContext = new PackageExtractionContext(
                               PackageSaveMode.Nuspec | PackageSaveMode.Files,
                               PackageExtractionBehavior.XmlDocFileSaveMode,
                               clientPolicyContext: null,
                               logger: NullLogger.Instance);

                    // Act
                    var packageFiles = PackageExtractor.ExtractPackageAsync(
                        root,
                        packageStream,
                        resolver,
                        packageExtractionContext,
                        CancellationToken.None);

                    // Assert
                    var installPath = resolver.GetInstallPath(identity);
                    Assert.False(File.Exists(Path.Combine(installPath, resolver.GetPackageFileName(identity))));
                    Assert.True(File.Exists(Path.Combine(installPath, resolver.GetManifestFileName(identity))));
                    Assert.True(File.Exists(Path.Combine(installPath, "lib", "net45", "A.dll")));
                    Assert.True(File.Exists(Path.Combine(installPath, "content", "net40", "B.txt")));
                }
            }
        }

        [CIOnlyFact]
        public async Task ExtractPackageAsync_PreservesZipEntryTimeAsync()
        {
            // Arrange
            using (var root = TestDirectory.Create())
            {
                var time = DateTime.Parse("2014-09-26T01:23:00Z",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.AdjustToUniversal);

                var resolver = new PackagePathResolver(root);
                var identity = new PackageIdentity("A", new NuGetVersion("2.0.3"));
                var packageFileInfo = await TestPackagesCore.GeneratePackageAsync(
                        root,
                        identity.Id,
                        identity.Version.ToString(),
                        time.ToLocalTime(), "lib/net45/A.dll");

                using (var packageStream = File.OpenRead(packageFileInfo.FullName))
                {
                    var packageExtractionContext = new PackageExtractionContext(
                               PackageSaveMode.Nuspec | PackageSaveMode.Files,
                               PackageExtractionBehavior.XmlDocFileSaveMode,
                               clientPolicyContext: null,
                               logger: NullLogger.Instance);

                    // Act
                    await PackageExtractor.ExtractPackageAsync(
                        root,
                        packageStream,
                        resolver,
                        packageExtractionContext,
                        CancellationToken.None);

                    var installPath = resolver.GetInstallPath(identity);
                    var outputDll = Path.Combine(installPath, "lib", "net45", "A.dll");
                    var outputTime = File.GetLastWriteTimeUtc(outputDll);

                    // Assert
                    Assert.True(File.Exists(outputDll));
                    Assert.Equal(time, outputTime);
                }
            }
        }

        [Fact]
        public async Task ExtractPackageAsync_IgnoresFutureZipEntryTimeAsync()
        {
            // Arrange
            using (var root = TestDirectory.Create())
            {
                var testStartTime = DateTime.UtcNow;
                var time = DateTime.Parse("2084-09-26T01:23:00Z",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.AdjustToUniversal);

                var resolver = new PackagePathResolver(root);
                var identity = new PackageIdentity("A", new NuGetVersion("2.0.3"));
                var packageFileInfo = await TestPackagesCore.GeneratePackageAsync(
                        root,
                        identity.Id,
                        identity.Version.ToString(),
                        time.ToLocalTime(), "lib/net45/A.dll");

                using (var packageStream = File.OpenRead(packageFileInfo.FullName))
                {
                    var packageExtractionContext = new PackageExtractionContext(
                               PackageSaveMode.Nuspec | PackageSaveMode.Files,
                               PackageExtractionBehavior.XmlDocFileSaveMode,
                               clientPolicyContext: null,
                               logger: NullLogger.Instance);

                    // Act
                    await PackageExtractor.ExtractPackageAsync(
                        root,
                        packageStream,
                        resolver,
                        packageExtractionContext,
                        CancellationToken.None);

                    var installPath = resolver.GetInstallPath(identity);
                    var outputDll = Path.Combine(installPath, "lib", "net45", "A.dll");
                    var outputTime = File.GetLastWriteTimeUtc(outputDll);
                    var testEndTime = DateTime.UtcNow;

                    // Assert
                    Assert.True(File.Exists(outputDll));
                    // Allow some slop with the time to deal with file system accuracy limits
                    Assert.InRange(outputTime, testStartTime.AddMinutes(-1), testEndTime.AddMinutes(1));
                }
            }
        }

        [Fact(Skip = "https://github.com/NuGet/Home/issues/13339")]
        public async Task ExtractPackageAsync_SetsFilePermissionsAsync()
        {
            if (RuntimeEnvironmentHelper.IsWindows)
            {
                return;
            }
            // Arrange
            using (var root = TestDirectory.Create())
            {
                var resolver = new PackagePathResolver(root);
                var identity = new PackageIdentity("A", new NuGetVersion("2.0.3"));
                var packageFileInfo = await TestPackagesCore.GeneratePackageAsync(
                        root,
                        identity.Id,
                        identity.Version.ToString(),
                        DateTimeOffset.UtcNow.LocalDateTime, "lib/net45/A.dll");

                using (var packageStream = File.OpenRead(packageFileInfo.FullName))
                {
                    var packageExtractionContext = new PackageExtractionContext(
                               PackageSaveMode.Nuspec | PackageSaveMode.Files,
                               PackageExtractionBehavior.XmlDocFileSaveMode,
                               clientPolicyContext: null,
                               logger: NullLogger.Instance);

                    // Act
                    await PackageExtractor.ExtractPackageAsync(
                        root,
                        packageStream,
                        resolver,
                        packageExtractionContext,
                        CancellationToken.None);

                    var installPath = resolver.GetInstallPath(identity);
                    var outputDll = Path.Combine(installPath, "lib", "net45", "A.dll");

                    // Assert
                    var expected = PermissionWithUMaskApplied("766");
                    Assert.Equal(expected, StatPermissions(outputDll));
                }
            }
        }

        [Fact]
        public async Task ExtractPackageAsync_WithoutPackageStream_ThrowsForNullPackageReaderAsync()
        {
            using (var test = new ExtractPackageAsyncTest())
            {
                await test.CreatePackageAsync();

                var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                    () => PackageExtractor.ExtractPackageAsync(
                        null,
                        packageReader: null,
                        packagePathResolver: test.Resolver,
                        packageExtractionContext: test.Context,
                        token: CancellationToken.None));

                Assert.Equal("packageReader", exception.ParamName);
            }
        }

        [Fact]
        public async Task ExtractPackageAsync_WithoutPackageStream_ThrowsForNullPackagePathResolverAsync()
        {
            using (var test = new ExtractPackageAsyncTest())
            {
                await test.CreatePackageAsync();

                var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                    () => PackageExtractor.ExtractPackageAsync(
                        null,
                        test.Reader,
                        packagePathResolver: null,
                        packageExtractionContext: test.Context,
                        token: CancellationToken.None));

                Assert.Equal("packagePathResolver", exception.ParamName);
            }
        }

        [Fact]
        public async Task ExtractPackageAsync_WithoutPackageStream_ThrowsForNullPackageExtractionContextAsync()
        {
            using (var test = new ExtractPackageAsyncTest())
            {
                await test.CreatePackageAsync();

                var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                    () => PackageExtractor.ExtractPackageAsync(
                        null,
                        test.Reader,
                        test.Resolver,
                        packageExtractionContext: null,
                        token: CancellationToken.None));

                Assert.Equal("packageExtractionContext", exception.ParamName);
            }
        }

        [Fact]
        public async Task ExtractPackageAsync_WithoutPackageStream_ThrowsIfCancelledAsync()
        {
            using (var test = new ExtractPackageAsyncTest())
            {
                await test.CreatePackageAsync();

                await Assert.ThrowsAsync<OperationCanceledException>(
                    () => PackageExtractor.ExtractPackageAsync(
                        null,
                        test.Reader,
                        test.Resolver,
                        test.Context,
                        new CancellationToken(canceled: true)));
            }
        }

        [Fact]
        public async Task ExtractPackageAsync_WithoutPackageStream_PackageSaveModeNoneAsync()
        {
            using (var test = new ExtractPackageAsyncTest())
            {
                await test.CreatePackageAsync();

                test.Context.PackageSaveMode = PackageSaveMode.None;

                var files = await PackageExtractor.ExtractPackageAsync(
                    test.Source,
                    test.Reader,
                    test.Resolver,
                    test.Context,
                    CancellationToken.None);

                var packageId = test.PackageIdentity.Id;
                var packageVersion = test.PackageIdentity.Version.ToNormalizedString();
                var packageDirectoryPath = Path.Combine(
                    test.DestinationDirectory.FullName,
                    $"{packageId}.{packageVersion}");
                var nupkgFilePath = Path.Combine(
                    packageDirectoryPath,
                    $"{packageId}.{packageVersion}{PackagingCoreConstants.NupkgExtension}");
                var nuspecFilePath = Path.Combine(packageDirectoryPath, $"{packageId}{PackagingCoreConstants.NuspecExtension}");
                var libFilePath = Path.Combine(packageDirectoryPath, "lib", "net45", $"{packageId}.dll");

                var comparer = PathUtility.GetStringComparerBasedOnOS();

                Assert.DoesNotContain(nupkgFilePath, files, comparer);
                Assert.DoesNotContain(nuspecFilePath, files, comparer);
                Assert.DoesNotContain(libFilePath, files, comparer);
                Assert.False(File.Exists(nupkgFilePath));
                Assert.False(File.Exists(nuspecFilePath));
                Assert.False(File.Exists(libFilePath));
            }
        }

        [Fact]
        public async Task ExtractPackageAsync_WithoutPackageStream_PackageSaveModeFilesAsync()
        {
            using (var test = new ExtractPackageAsyncTest())
            {
                await test.CreatePackageAsync();

                test.Context.PackageSaveMode = PackageSaveMode.Files;

                var files = await PackageExtractor.ExtractPackageAsync(
                    test.Source,
                    test.Reader,
                    test.Resolver,
                    test.Context,
                    CancellationToken.None);

                var packageId = test.PackageIdentity.Id;
                var packageVersion = test.PackageIdentity.Version.ToNormalizedString();
                var packageDirectoryPath = Path.Combine(
                    test.DestinationDirectory.FullName,
                    $"{packageId}.{packageVersion}");
                var nupkgFilePath = Path.Combine(
                    packageDirectoryPath,
                    $"{packageId}.{packageVersion}{PackagingCoreConstants.NupkgExtension}");
                var nuspecFilePath = Path.Combine(packageDirectoryPath, $"{packageId}{PackagingCoreConstants.NuspecExtension}");
                var libFilePath = Path.Combine(packageDirectoryPath, "lib", "net45", $"{packageId}.dll");

                var comparer = PathUtility.GetStringComparerBasedOnOS();

                Assert.DoesNotContain(nupkgFilePath, files, comparer);
                Assert.DoesNotContain(nuspecFilePath, files, comparer);
                Assert.Contains(libFilePath, files, comparer);
                Assert.False(File.Exists(nupkgFilePath));
                Assert.False(File.Exists(nuspecFilePath));
                Assert.True(File.Exists(libFilePath));
            }
        }

        [Fact]
        public async Task ExtractPackageAsync_WithoutPackageStream_PackageSaveModeNuspecAsync()
        {
            using (var test = new ExtractPackageAsyncTest())
            {
                await test.CreatePackageAsync();

                test.Context.PackageSaveMode = PackageSaveMode.Nuspec;

                var files = await PackageExtractor.ExtractPackageAsync(
                    test.Source,
                    test.Reader,
                    test.Resolver,
                    test.Context,
                    CancellationToken.None);

                var packageId = test.PackageIdentity.Id;
                var packageVersion = test.PackageIdentity.Version.ToNormalizedString();
                var packageDirectoryPath = Path.Combine(
                    test.DestinationDirectory.FullName,
                    $"{packageId}.{packageVersion}");
                var nupkgFilePath = Path.Combine(
                    packageDirectoryPath,
                    $"{packageId}.{packageVersion}{PackagingCoreConstants.NupkgExtension}");
                var nuspecFilePath = Path.Combine(packageDirectoryPath, $"{packageId}{PackagingCoreConstants.NuspecExtension}");
                var libFilePath = Path.Combine(packageDirectoryPath, "lib", "net45", $"{packageId}.dll");

                var comparer = PathUtility.GetStringComparerBasedOnOS();

                Assert.DoesNotContain(nupkgFilePath, files, comparer);
                Assert.Contains(nuspecFilePath, files, comparer);
                Assert.DoesNotContain(libFilePath, files, comparer);
                Assert.False(File.Exists(nupkgFilePath));
                Assert.True(File.Exists(nuspecFilePath));
                Assert.False(File.Exists(libFilePath));
            }
        }

        [Fact]
        public async Task ExtractPackageAsync_WithoutPackageStream_PackageSaveModeNupkgAsync()
        {
            using (var test = new ExtractPackageAsyncTest())
            {
                await test.CreatePackageAsync();

                test.Context.PackageSaveMode = PackageSaveMode.Nupkg;

                var files = await PackageExtractor.ExtractPackageAsync(
                    test.Source,
                    test.Reader,
                    test.Resolver,
                    test.Context,
                    CancellationToken.None);

                var packageId = test.PackageIdentity.Id;
                var packageVersion = test.PackageIdentity.Version.ToNormalizedString();
                var packageDirectoryPath = Path.Combine(
                    test.DestinationDirectory.FullName,
                    $"{packageId}.{packageVersion}");
                var nupkgFilePath = Path.Combine(
                    packageDirectoryPath,
                    $"{packageId}.{packageVersion}{PackagingCoreConstants.NupkgExtension}");
                var nuspecFilePath = Path.Combine(packageDirectoryPath, $"{packageId}{PackagingCoreConstants.NuspecExtension}");
                var libFilePath = Path.Combine(packageDirectoryPath, "lib", "net45", $"{packageId}.dll");

                var comparer = PathUtility.GetStringComparerBasedOnOS();

                Assert.Contains(nupkgFilePath, files, comparer);
                Assert.DoesNotContain(nuspecFilePath, files, comparer);
                Assert.DoesNotContain(libFilePath, files, comparer);
                Assert.True(File.Exists(nupkgFilePath));
                Assert.False(File.Exists(nuspecFilePath));
                Assert.False(File.Exists(libFilePath));
            }
        }

        [Fact]
        public async Task ExtractPackageAsync_WithoutPackageStream_PackageSaveModeDefaultV2Async()
        {
            using (var test = new ExtractPackageAsyncTest())
            {
                await test.CreatePackageAsync();

                test.Context.PackageSaveMode = PackageSaveMode.Defaultv2;

                var files = await PackageExtractor.ExtractPackageAsync(
                    test.Source,
                    test.Reader,
                    test.Resolver,
                    test.Context,
                    CancellationToken.None);

                var packageId = test.PackageIdentity.Id;
                var packageVersion = test.PackageIdentity.Version.ToNormalizedString();
                var packageDirectoryPath = Path.Combine(
                    test.DestinationDirectory.FullName,
                    $"{packageId}.{packageVersion}");
                var nupkgFilePath = Path.Combine(
                    packageDirectoryPath,
                    $"{packageId}.{packageVersion}{PackagingCoreConstants.NupkgExtension}");
                var nuspecFilePath = Path.Combine(packageDirectoryPath, $"{packageId}{PackagingCoreConstants.NuspecExtension}");
                var libFilePath = Path.Combine(packageDirectoryPath, "lib", "net45", $"{packageId}.dll");

                var comparer = PathUtility.GetStringComparerBasedOnOS();

                Assert.Contains(nupkgFilePath, files, comparer);
                Assert.DoesNotContain(nuspecFilePath, files, comparer);
                Assert.Contains(libFilePath, files, comparer);
                Assert.True(File.Exists(nupkgFilePath));
                Assert.False(File.Exists(nuspecFilePath));
                Assert.True(File.Exists(libFilePath));
            }
        }

        [Fact]
        public async Task ExtractPackageAsync_WithoutPackageStream_PackageSaveModeDefaultV3Async()
        {
            using (var test = new ExtractPackageAsyncTest())
            {
                await test.CreatePackageAsync();

                test.Context.PackageSaveMode = PackageSaveMode.Defaultv3;

                var files = await PackageExtractor.ExtractPackageAsync(
                    test.Source,
                    test.Reader,
                    test.Resolver,
                    test.Context,
                    CancellationToken.None);

                var packageId = test.PackageIdentity.Id;
                var packageVersion = test.PackageIdentity.Version.ToNormalizedString();
                var packageDirectoryPath = Path.Combine(
                    test.DestinationDirectory.FullName,
                    $"{packageId}.{packageVersion}");
                var nupkgFilePath = Path.Combine(
                    packageDirectoryPath,
                    $"{packageId}.{packageVersion}{PackagingCoreConstants.NupkgExtension}");
                var nuspecFilePath = Path.Combine(packageDirectoryPath, $"{packageId}{PackagingCoreConstants.NuspecExtension}");
                var libFilePath = Path.Combine(packageDirectoryPath, "lib", "net45", $"{packageId}.dll");

                var comparer = PathUtility.GetStringComparerBasedOnOS();

                Assert.Contains(nupkgFilePath, files, comparer);
                Assert.Contains(nuspecFilePath, files, comparer);
                Assert.Contains(libFilePath, files, comparer);
                Assert.True(File.Exists(nupkgFilePath));
                Assert.True(File.Exists(nuspecFilePath));
                Assert.True(File.Exists(libFilePath));
            }
        }

        [Fact]
        public async Task ExtractPackageAsync_WithoutPackageStream_XmlDocFileSaveModeNoneAsync()
        {
            using (var test = new ExtractPackageAsyncTest())
            {
                await test.CreatePackageAsync();

                test.Context.PackageSaveMode = PackageSaveMode.Files;
                test.Context.XmlDocFileSaveMode = XmlDocFileSaveMode.None;

                var files = await PackageExtractor.ExtractPackageAsync(
                    test.Source,
                    test.Reader,
                    test.Resolver,
                    test.Context,
                    CancellationToken.None);

                var packageId = test.PackageIdentity.Id;
                var packageVersion = test.PackageIdentity.Version.ToNormalizedString();
                var packageDirectoryPath = Path.Combine(
                    test.DestinationDirectory.FullName,
                    $"{packageId}.{packageVersion}");
                var xmlFilePath = Path.Combine(packageDirectoryPath, "lib", "net45", $"{packageId}.xml");

                var comparer = PathUtility.GetStringComparerBasedOnOS();

                Assert.Contains(xmlFilePath, files, comparer);
                Assert.True(File.Exists(xmlFilePath));
            }
        }

        [Fact]
        public async Task ExtractPackageAsync_WithoutPackageStream_XmlDocFileSaveModeSkipAsync()
        {
            using (var test = new ExtractPackageAsyncTest())
            {
                await test.CreatePackageAsync();

                test.Context.PackageSaveMode = PackageSaveMode.Files;
                test.Context.XmlDocFileSaveMode = XmlDocFileSaveMode.Skip;

                var files = await PackageExtractor.ExtractPackageAsync(
                    test.Source,
                    test.Reader,
                    test.Resolver,
                    test.Context,
                    CancellationToken.None);

                var packageId = test.PackageIdentity.Id;
                var packageVersion = test.PackageIdentity.Version.ToNormalizedString();
                var packageDirectoryPath = Path.Combine(
                    test.DestinationDirectory.FullName,
                    $"{packageId}.{packageVersion}");
                var xmlFilePath = Path.Combine(packageDirectoryPath, "lib", "net45", $"{packageId}.xml");

                var comparer = PathUtility.GetStringComparerBasedOnOS();

                Assert.DoesNotContain(xmlFilePath, files, comparer);
                Assert.False(File.Exists(xmlFilePath));
            }
        }

        [Fact]
        public async Task ExtractPackageAsync_WithoutPackageStream_XmlDocFileSaveModeCompressAsync()
        {
            using (var test = new ExtractPackageAsyncTest())
            {
                await test.CreatePackageAsync();

                test.Context.PackageSaveMode = PackageSaveMode.Files;
                test.Context.XmlDocFileSaveMode = XmlDocFileSaveMode.Compress;

                var files = await PackageExtractor.ExtractPackageAsync(
                    test.Source,
                    test.Reader,
                    test.Resolver,
                    test.Context,
                    CancellationToken.None);

                var packageId = test.PackageIdentity.Id;
                var packageVersion = test.PackageIdentity.Version.ToNormalizedString();
                var packageDirectoryPath = Path.Combine(
                    test.DestinationDirectory.FullName,
                    $"{packageId}.{packageVersion}");
                var xmlZipFilePath = Path.Combine(packageDirectoryPath, "lib", "net45", $"{packageId}.xml.zip");

                var comparer = PathUtility.GetStringComparerBasedOnOS();

                Assert.Contains(xmlZipFilePath, files, comparer);
                Assert.True(File.Exists(xmlZipFilePath));
            }
        }

#if IS_SIGNING_SUPPORTED
        [PlatformFact(Platform.Windows)]
        public async Task ExtractPackageAsync_UnsignedPackage_WhenRepositorySaysAllPackagesSigned_ErrorAsync()
        {
            var extractionContext = new PackageExtractionContext(
                PackageSaveMode.Defaultv2,
                PackageExtractionBehavior.XmlDocFileSaveMode,
                new ClientPolicyContext(SignatureValidationMode.Accept, new List<TrustedSignerAllowListEntry>()),
                logger: NullLogger.Instance);

            using (var test = new ExtractPackageAsyncTest(extractionContext))
            {
                await test.CreatePackageAsync();

                var repositorySignatureInfo = CreateTestRepositorySignatureInfo(new List<X509Certificate2>(), allSigned: true);
                var repositorySignatureInfoProvider = RepositorySignatureInfoProvider.Instance;

                repositorySignatureInfoProvider.AddOrUpdateRepositorySignatureInfo(test.Source, repositorySignatureInfo);
                test.Context.PackageSaveMode = PackageSaveMode.Defaultv3;

                SignatureException exception = null;

                try
                {
                    await PackageExtractor.ExtractPackageAsync(
                         test.Source,
                         test.Reader,
                         test.Resolver,
                         test.Context,
                         CancellationToken.None);
                }
                catch (SignatureException e)
                {
                    exception = e;
                }

                // Assert
                exception.Should().NotBeNull();
                exception.Results.Count.Should().Be(1);

                exception.Results.First().Issues.Count().Should().Be(1);
                exception.Results.First().Issues.First().Code.Should().Be(NuGetLogCode.NU3004);
                exception.Results.First().Issues.First().Message.Should()
                    .Be(SigningTestUtility.AddSignatureLogPrefix(_notSignedPackageRepo, test.Reader.GetIdentity(), test.Source));
            }
        }

        [PlatformFact(Platform.Darwin)]
        public async Task ExtractPackageAsync_UnsignedPackage_WhenRepositorySaysAllPackagesSigned_SuccessAsync()
        {
            var extractionContext = new PackageExtractionContext(
                PackageSaveMode.Defaultv2,
                PackageExtractionBehavior.XmlDocFileSaveMode,
                new ClientPolicyContext(SignatureValidationMode.Accept, new List<TrustedSignerAllowListEntry>()),
                logger: NullLogger.Instance);

            using (var test = new ExtractPackageAsyncTest(extractionContext))
            {
                await test.CreatePackageAsync();

                RepositorySignatureInfo repositorySignatureInfo = CreateTestRepositorySignatureInfo(new List<X509Certificate2>(), allSigned: true);
                var repositorySignatureInfoProvider = RepositorySignatureInfoProvider.Instance;

                repositorySignatureInfoProvider.AddOrUpdateRepositorySignatureInfo(test.Source, repositorySignatureInfo);
                test.Context.PackageSaveMode = PackageSaveMode.Defaultv3;

                // Act
                IEnumerable<string> files = await PackageExtractor.ExtractPackageAsync(
                     test.Source,
                     test.Stream,
                     test.Resolver,
                     test.Context,
                     CancellationToken.None);

                // Assert
                files.Should().NotBeNull();
                Directory.Exists(Path.Combine(test.DestinationDirectory.FullName,
                    $"{test.PackageIdentity.Id}.{test.PackageIdentity.Version.ToNormalizedString()}")).Should().BeTrue();
            }
        }

        [CIOnlyFact]
        public async Task ExtractPackageAsync_UnsignedPackage_WhenRepositorySaysAllPackagesSigned_WithEnvVar_Error()
        {
            // Arrange
            var environment = new Mock<IEnvironmentVariableReader>(MockBehavior.Strict);
            environment.Setup(s => s.GetEnvironmentVariable(SignatureVerificationEnvironmentVariable)).Returns("TRUE");

            var extractionContext = new PackageExtractionContext(
                PackageSaveMode.Defaultv2,
                PackageExtractionBehavior.XmlDocFileSaveMode,
                new ClientPolicyContext(SignatureValidationMode.Accept, new List<TrustedSignerAllowListEntry>()),
                logger: NullLogger.Instance);

            using (var test = new ExtractPackageAsyncTest(extractionContext, environmentVariableReader: environment.Object))
            {
                await test.CreatePackageAsync();

                var repositorySignatureInfo = CreateTestRepositorySignatureInfo(new List<X509Certificate2>(), allSigned: true);
                var repositorySignatureInfoProvider = RepositorySignatureInfoProvider.Instance;

                repositorySignatureInfoProvider.AddOrUpdateRepositorySignatureInfo(test.Source, repositorySignatureInfo);
                test.Context.PackageSaveMode = PackageSaveMode.Defaultv3;

                SignatureException exception = null;

                // Act
                try
                {
                    await PackageExtractor.ExtractPackageAsync(
                         test.Source,
                         test.Reader,
                         test.Resolver,
                         test.Context,
                         CancellationToken.None);
                }
                catch (SignatureException e)
                {
                    exception = e;
                }

                // Assert
                exception.Should().NotBeNull();
                exception.Results.Count.Should().Be(1);

                exception.Results.First().Issues.Count().Should().Be(1);
                exception.Results.First().Issues.First().Code.Should().Be(NuGetLogCode.NU3004);
                exception.Results.First().Issues.First().Message.Should()
                    .Be(SigningTestUtility.AddSignatureLogPrefix(_notSignedPackageRepo, test.Reader.GetIdentity(), test.Source));
            }
        }

        [PlatformFact(Platform.Darwin)]
        public async Task ExtractPackageAsync_UnsignedPackage_WhenRepositorySaysAllPackagesSigned_WithEnvVarNameCaseSensitive_Success()
        {
            // Arrange
            string envVarName = SignatureVerificationEnvironmentVariableTypo;
            var extractionContext = new PackageExtractionContext(
                PackageSaveMode.Defaultv2,
                PackageExtractionBehavior.XmlDocFileSaveMode,
                new ClientPolicyContext(SignatureValidationMode.Accept, new List<TrustedSignerAllowListEntry>()),
                logger: NullLogger.Instance);

            var environment = new Mock<IEnvironmentVariableReader>(MockBehavior.Loose);
            environment.Setup(s => s.GetEnvironmentVariable(envVarName)).Returns("true");

            using (var test = new ExtractPackageAsyncTest(extractionContext, environmentVariableReader: environment.Object))
            {
                await test.CreatePackageAsync();

                var repositorySignatureInfo = CreateTestRepositorySignatureInfo(new List<X509Certificate2>(), allSigned: true);
                var repositorySignatureInfoProvider = RepositorySignatureInfoProvider.Instance;

                repositorySignatureInfoProvider.AddOrUpdateRepositorySignatureInfo(test.Source, repositorySignatureInfo);
                test.Context.PackageSaveMode = PackageSaveMode.Defaultv3;

                // Act
                IEnumerable<string> files = await PackageExtractor.ExtractPackageAsync(
                     test.Source,
                     test.Reader,
                     test.Stream,
                     test.Resolver,
                     test.Context,
                     CancellationToken.None);

                // Assert
                files.Should().NotBeNull();
                Directory.Exists(Path.Combine(test.DestinationDirectory.FullName,
                    $"{test.PackageIdentity.Id}.{test.PackageIdentity.Version.ToNormalizedString()}")).Should().BeTrue();
            }
        }

        [PlatformFact(Platform.Windows)]
        public async Task ExtractPackageAsync_UnsignedPackage_RequireMode_ErrorAsync()
        {
            var extractionContext = new PackageExtractionContext(
                PackageSaveMode.Defaultv2,
                PackageExtractionBehavior.XmlDocFileSaveMode,
                new ClientPolicyContext(SignatureValidationMode.Require, new List<TrustedSignerAllowListEntry>()
                {
                    new TrustedSignerAllowListEntry(VerificationTarget.Repository, SignaturePlacement.Any, "abc", HashAlgorithmName.SHA256)
                }),
                logger: NullLogger.Instance);

            using (var test = new ExtractPackageAsyncTest(extractionContext))
            {
                await test.CreatePackageAsync();

                var repositorySignatureInfo = CreateTestRepositorySignatureInfo(new List<X509Certificate2>(), allSigned: true);
                var repositorySignatureInfoProvider = RepositorySignatureInfoProvider.Instance;

                repositorySignatureInfoProvider.AddOrUpdateRepositorySignatureInfo(test.Source, repositorySignatureInfo);
                test.Context.PackageSaveMode = PackageSaveMode.Defaultv3;

                SignatureException exception = null;

                try
                {
                    await PackageExtractor.ExtractPackageAsync(
                         test.Source,
                         test.Reader,
                         test.Resolver,
                         test.Context,
                         CancellationToken.None);
                }
                catch (SignatureException e)
                {
                    exception = e;
                }

                // Assert
                exception.Should().NotBeNull();
                exception.Results.Count.Should().Be(1);

                exception.Results.First().Issues.Count().Should().Be(1);
                exception.Results.First().Issues.First().Code.Should().Be(NuGetLogCode.NU3004);
                exception.Results.First().Issues.First().Message.Should()
                    .Be(SigningTestUtility.AddSignatureLogPrefix(_notSignedPackageRequire, test.Reader.GetIdentity(), test.Source));
            }
        }

        [PlatformFact(Platform.Darwin)]
        public async Task ExtractPackageAsync_UnsignedPackage_RequireMode_SuccessAsync()
        {
            var extractionContext = new PackageExtractionContext(
                PackageSaveMode.Defaultv2,
                PackageExtractionBehavior.XmlDocFileSaveMode,
                new ClientPolicyContext(SignatureValidationMode.Require, new List<TrustedSignerAllowListEntry>()
                {
                    new TrustedSignerAllowListEntry(VerificationTarget.Repository, SignaturePlacement.Any, "abc", HashAlgorithmName.SHA256)
                }),
                logger: NullLogger.Instance);

            using (var test = new ExtractPackageAsyncTest(extractionContext))
            {
                await test.CreatePackageAsync();

                RepositorySignatureInfo repositorySignatureInfo = CreateTestRepositorySignatureInfo(new List<X509Certificate2>(), allSigned: true);
                var repositorySignatureInfoProvider = RepositorySignatureInfoProvider.Instance;

                repositorySignatureInfoProvider.AddOrUpdateRepositorySignatureInfo(test.Source, repositorySignatureInfo);
                test.Context.PackageSaveMode = PackageSaveMode.Defaultv3;

                // Act
                IEnumerable<string> files = await PackageExtractor.ExtractPackageAsync(
                     test.Source,
                     test.Stream,
                     test.Resolver,
                     test.Context,
                     CancellationToken.None);

                // Assert
                files.Should().NotBeNull();
            }
        }

        [CIOnlyFact]
        public async Task ExtractPackageAsync_UnsignedPackage_RequireMode_WithEnvVar_Error()
        {
            // Arrange
            var environment = new Mock<IEnvironmentVariableReader>(MockBehavior.Strict);
            environment.Setup(s => s.GetEnvironmentVariable(SignatureVerificationEnvironmentVariable)).Returns("TRUE");

            var extractionContext = new PackageExtractionContext(
                PackageSaveMode.Defaultv2,
                PackageExtractionBehavior.XmlDocFileSaveMode,
                new ClientPolicyContext(SignatureValidationMode.Require, new List<TrustedSignerAllowListEntry>()
                {
                    new TrustedSignerAllowListEntry(VerificationTarget.Repository, SignaturePlacement.Any, "abc", HashAlgorithmName.SHA256)
                }),
                logger: NullLogger.Instance);

            using (var test = new ExtractPackageAsyncTest(extractionContext, environmentVariableReader: environment.Object))
            {
                await test.CreatePackageAsync();

                var repositorySignatureInfo = CreateTestRepositorySignatureInfo(new List<X509Certificate2>(), allSigned: true);
                var repositorySignatureInfoProvider = RepositorySignatureInfoProvider.Instance;

                repositorySignatureInfoProvider.AddOrUpdateRepositorySignatureInfo(test.Source, repositorySignatureInfo);
                test.Context.PackageSaveMode = PackageSaveMode.Defaultv3;

                SignatureException exception = null;

                try
                {
                    await PackageExtractor.ExtractPackageAsync(
                         test.Source,
                         test.Reader,
                         test.Resolver,
                         test.Context,
                         CancellationToken.None);
                }
                catch (SignatureException e)
                {
                    exception = e;
                }

                // Assert
                exception.Should().NotBeNull();
                exception.Results.Count.Should().Be(1);

                exception.Results.First().Issues.Count().Should().Be(1);
                exception.Results.First().Issues.First().Code.Should().Be(NuGetLogCode.NU3004);
                exception.Results.First().Issues.First().Message.Should()
                    .Be(SigningTestUtility.AddSignatureLogPrefix(_notSignedPackageRequire, test.Reader.GetIdentity(), test.Source));
            }
        }

        [PlatformFact(Platform.Darwin)]
        public async Task ExtractPackageAsync_RequireMode_EmptyRepoAllowList_SuccessAsync()
        {
            using (var dir = TestDirectory.Create())
            using (TrustedTestCert<TestCertificate> repoCertificate = SigningTestUtility.GenerateTrustedTestCertificate())
            {
                var nupkg = new SimpleTestPackageContext();
                var resolver = new PackagePathResolver(dir);
                var certificateFingerprint = SignatureTestUtility.GetFingerprint(repoCertificate.Source.Cert, HashAlgorithmName.SHA256);

                RepositorySignatureInfo repositorySignatureInfo = CreateTestRepositorySignatureInfo(new List<X509Certificate2>(), allSigned: true);
                var repositorySignatureInfoProvider = RepositorySignatureInfoProvider.Instance;
                var repoSignedPackagePath = await SignedArchiveTestUtility.RepositorySignPackageAsync(repoCertificate.Source.Cert, nupkg, dir, new Uri(@"https://v3serviceIndex.test/api"));

                repositorySignatureInfoProvider.AddOrUpdateRepositorySignatureInfo(dir, repositorySignatureInfo);

                var extractionContext = new PackageExtractionContext(
                    PackageSaveMode.Defaultv3,
                    PackageExtractionBehavior.XmlDocFileSaveMode,
                    new ClientPolicyContext(SignatureValidationMode.Require, new List<TrustedSignerAllowListEntry>()
                    {
                        new TrustedSignerAllowListEntry(VerificationTarget.Repository, SignaturePlacement.Any, certificateFingerprint, HashAlgorithmName.SHA256)
                    }),
                    logger: NullLogger.Instance);

                using (var packageStream = File.OpenRead(repoSignedPackagePath))
                {
                    // Act
                    IEnumerable<string> files = await PackageExtractor.ExtractPackageAsync(
                         dir,
                         packageStream,
                         resolver,
                         extractionContext,
                         CancellationToken.None);

                    // Assert
                    files.Should().NotBeNull();
                    Directory.Exists(Path.Combine(dir.Path,
                        $"{nupkg.Identity.Id}.{nupkg.Identity.Version.ToNormalizedString()}"))
                        .Should()
                        .BeTrue();
                }
            }
        }

        [PlatformFact(Platform.Windows, CIOnly = true)]
        public async Task ExtractPackageAsync_RequireMode_NoMatchInClientAllowList_Error()
        {
            // Arrange
            using (var dir = TestDirectory.Create())
            using (TrustedTestCert<TestCertificate> repoCertificate = SigningTestUtility.GenerateTrustedTestCertificate())
            {
                var nupkg = new SimpleTestPackageContext();
                var resolver = new PackagePathResolver(dir);

                RepositorySignatureInfo repositorySignatureInfo = CreateTestRepositorySignatureInfo(new List<X509Certificate2>() { repoCertificate.Source.Cert }, allSigned: true);
                var repositorySignatureInfoProvider = RepositorySignatureInfoProvider.Instance;
                var repoSignedPackagePath = await SignedArchiveTestUtility.RepositorySignPackageAsync(repoCertificate.Source.Cert, nupkg, dir, new Uri(@"https://v3serviceIndex.test/api"));

                repositorySignatureInfoProvider.AddOrUpdateRepositorySignatureInfo(dir, repositorySignatureInfo);

                var extractionContext = new PackageExtractionContext(
                    PackageSaveMode.Defaultv3,
                    PackageExtractionBehavior.XmlDocFileSaveMode,
                    new ClientPolicyContext(SignatureValidationMode.Require, new List<TrustedSignerAllowListEntry>()
                    {
                        new TrustedSignerAllowListEntry(VerificationTarget.Repository, SignaturePlacement.Any, "abc", HashAlgorithmName.SHA256)
                    }),
                    logger: NullLogger.Instance);

                using (var packageStream = File.OpenRead(repoSignedPackagePath))
                using (var packageReader = new PackageArchiveReader(packageStream))
                {
                    // Act
                    SignatureException exception = null;

                    try
                    {
                        await PackageExtractor.ExtractPackageAsync(
                             dir,
                             packageStream,
                             resolver,
                             extractionContext,
                             CancellationToken.None);
                    }
                    catch (SignatureException e)
                    {
                        exception = e;
                    }

                    // Assert
                    exception.Should().NotBeNull();
                    exception.Results.Count.Should().Be(4);

                    // allowListVerificationProvider result is the only one that throws NU3034
                    var issues = exception.Results.SelectMany(r => r.Issues.Where(i => i.Code == NuGetLogCode.NU3034));

                    issues.Count().Should().Be(1);
                    issues.First().Code.Should().Be(NuGetLogCode.NU3034);
                    issues.First().Message.Should().Be(SigningTestUtility.AddSignatureLogPrefix(_noMatchInTrustedSignersList, packageReader.GetIdentity(), dir));
                }
            }
        }

        [PlatformFact(Platform.Darwin)]
        public async Task ExtractPackageAsync_RequireMode_NoMatchInClientAllowList_SuccessAsync()
        {
            using (var dir = TestDirectory.Create())
            using (TrustedTestCert<TestCertificate> repoCertificate = SigningTestUtility.GenerateTrustedTestCertificate())
            {
                var nupkg = new SimpleTestPackageContext();
                var resolver = new PackagePathResolver(dir);

                RepositorySignatureInfo repositorySignatureInfo = CreateTestRepositorySignatureInfo(new List<X509Certificate2>() { repoCertificate.Source.Cert }, allSigned: true);
                var repositorySignatureInfoProvider = RepositorySignatureInfoProvider.Instance;
                var repoSignedPackagePath = await SignedArchiveTestUtility.RepositorySignPackageAsync(repoCertificate.Source.Cert, nupkg, dir, new Uri(@"https://v3serviceIndex.test/api"));

                repositorySignatureInfoProvider.AddOrUpdateRepositorySignatureInfo(dir, repositorySignatureInfo);

                var extractionContext = new PackageExtractionContext(
                    PackageSaveMode.Defaultv3,
                    PackageExtractionBehavior.XmlDocFileSaveMode,
                    new ClientPolicyContext(SignatureValidationMode.Require, new List<TrustedSignerAllowListEntry>()
                    {
                        new TrustedSignerAllowListEntry(VerificationTarget.Repository, SignaturePlacement.Any, "abc", HashAlgorithmName.SHA256)
                    }),
                    logger: NullLogger.Instance);

                using (var packageStream = File.OpenRead(repoSignedPackagePath))
                {
                    // Act
                    IEnumerable<string> files = await PackageExtractor.ExtractPackageAsync(
                         dir,
                         packageStream,
                         resolver,
                         extractionContext,
                         CancellationToken.None);

                    // Assert
                    files.Should().NotBeNull();
                    Directory.Exists(Path.Combine(dir.Path,
                        $"{nupkg.Identity.Id}.{nupkg.Identity.Version.ToNormalizedString()}")).Should().BeTrue();
                }
            }
        }

        [CIOnlyFact]
        public async Task ExtractPackageAsync_RequireMode_NoMatchInClientAllowList_WithEnvVar_Error()
        {
            // Arrange
            var environment = new Mock<IEnvironmentVariableReader>(MockBehavior.Strict);
            environment.Setup(s => s.GetEnvironmentVariable(SignatureVerificationEnvironmentVariable)).Returns("TRUE");

            using (var dir = TestDirectory.Create())
            using (TrustedTestCert<TestCertificate> repoCertificate = SigningTestUtility.GenerateTrustedTestCertificate())
            {
                var nupkg = new SimpleTestPackageContext();
                var resolver = new PackagePathResolver(dir);

                RepositorySignatureInfo repositorySignatureInfo = CreateTestRepositorySignatureInfo(new List<X509Certificate2>() { repoCertificate.Source.Cert }, allSigned: true);
                var repositorySignatureInfoProvider = RepositorySignatureInfoProvider.Instance;
                var repoSignedPackagePath = await SignedArchiveTestUtility.RepositorySignPackageAsync(repoCertificate.Source.Cert, nupkg, dir, new Uri(@"https://v3serviceIndex.test/api"));

                repositorySignatureInfoProvider.AddOrUpdateRepositorySignatureInfo(dir, repositorySignatureInfo);

                var extractionContext = new PackageExtractionContext(
                    PackageSaveMode.Defaultv3,
                    PackageExtractionBehavior.XmlDocFileSaveMode,
                    new ClientPolicyContext(SignatureValidationMode.Require, new List<TrustedSignerAllowListEntry>()
                    {
                        new TrustedSignerAllowListEntry(VerificationTarget.Repository, SignaturePlacement.Any, "abc", HashAlgorithmName.SHA256)
                    }),
                    logger: NullLogger.Instance);

                using (var packageStream = File.OpenRead(repoSignedPackagePath))
                using (var packageReader = new PackageArchiveReader(packageStream, environmentVariableReader: environment.Object))
                {
                    // Act
                    SignatureException exception = null;

                    try
                    {
                        await PackageExtractor.ExtractPackageAsync(
                             dir,
                             packageReader,
                             packageStream,
                             resolver,
                             extractionContext,
                             CancellationToken.None);
                    }
                    catch (SignatureException e)
                    {
                        exception = e;
                    }

                    // Assert
                    exception.Should().NotBeNull();
                    exception.Results.Count.Should().Be(4);

                    // allowListVerificationProvider result is the only one that throws NU3034
                    var issues = exception.Results.SelectMany(r => r.Issues.Where(i => i.Code == NuGetLogCode.NU3034));

                    issues.Count().Should().Be(1);
                    issues.First().Code.Should().Be(NuGetLogCode.NU3034);
                    issues.First().Message.Should().Be(SigningTestUtility.AddSignatureLogPrefix(_noMatchInTrustedSignersList, packageReader.GetIdentity(), dir));
                }
            }
        }

        [CIOnlyTheory(Skip = "https://github.com/NuGet/Home/issues/11700")]
        [MemberData(nameof(KnownClientPolicyModesList))]
        public async Task ExtractPackageAsync_RepositoryPrimarySignedPackage_PackageSignedWithCertFromRepositoryAllowList_SuccessAsync(SignatureValidationMode clientPolicyMode)
        {
            // Arrange
            using (var dir = TestDirectory.Create())
            using (TrustedTestCert<TestCertificate> repoCertificate = SigningTestUtility.GenerateTrustedTestCertificate())
            {
                var nupkg = new SimpleTestPackageContext();
                var certificateFingerprint = SignatureTestUtility.GetFingerprint(repoCertificate.Source.Cert, HashAlgorithmName.SHA256);
                var clientPolicy = new ClientPolicyContext(clientPolicyMode, new List<TrustedSignerAllowListEntry>()
                {
                    new TrustedSignerAllowListEntry(VerificationTarget.Repository, SignaturePlacement.Any, certificateFingerprint, HashAlgorithmName.SHA256)
                });

                var resolver = new PackagePathResolver(dir);
                var repositorySignatureInfo = CreateTestRepositorySignatureInfo(new List<X509Certificate2> { repoCertificate.Source.Cert }, allSigned: true);
                var repositorySignatureInfoContentUrl = repositorySignatureInfo.RepositoryCertificateInfos.Select(c => c.ContentUrl).First();
                var repositorySignatureInfoProvider = RepositorySignatureInfoProvider.Instance;
                var repoSignedPackagePath = await SignedArchiveTestUtility.RepositorySignPackageAsync(repoCertificate.Source.Cert, nupkg, dir, new Uri(repositorySignatureInfoContentUrl));

                repositorySignatureInfoProvider.AddOrUpdateRepositorySignatureInfo(dir, repositorySignatureInfo);

                using (var packageStream = File.OpenRead(repoSignedPackagePath))
                using (var packageReader = new PackageArchiveReader(packageStream))
                {
                    var packageExtractionContext = new PackageExtractionContext(
                        PackageSaveMode.Nuspec | PackageSaveMode.Files,
                        PackageExtractionBehavior.XmlDocFileSaveMode,
                        clientPolicy,
                        NullLogger.Instance);

                    await PackageExtractor.ExtractPackageAsync(
                        dir,
                        packageReader,
                        packageStream,
                        resolver,
                        packageExtractionContext,
                        CancellationToken.None);
                }
            }
        }

        [PlatformTheory(Platform.Darwin)]
        [MemberData(nameof(KnownClientPoliciesList))]
        public async Task GetTrustResultAsync_RepositoryPrimarySignedPackage_PackageSignedWithCertNotFromRepositoryAllowList_SuccessAsync(ClientPolicyContext clientPolicy)
        {
            // Arrange
            using (var dir = TestDirectory.Create())
            using (TrustedTestCert<TestCertificate> repoCertificate = SigningTestUtility.GenerateTrustedTestCertificate())
            using (TrustedTestCert<TestCertificate> packageSignatureCertificate = SigningTestUtility.GenerateTrustedTestCertificate())
            {
                var nupkg = new SimpleTestPackageContext();

                var resolver = new PackagePathResolver(dir);
                var repositorySignatureInfo = CreateTestRepositorySignatureInfo(new List<X509Certificate2> { repoCertificate.Source.Cert }, allSigned: true);
                var repositorySignatureInfoContentUrl = repositorySignatureInfo.RepositoryCertificateInfos.Select(c => c.ContentUrl).First();
                var repositorySignatureInfoProvider = RepositorySignatureInfoProvider.Instance;
                var repoSignedPackagePath = await SignedArchiveTestUtility.RepositorySignPackageAsync(packageSignatureCertificate.Source.Cert, nupkg, dir, new Uri(repositorySignatureInfoContentUrl));

                repositorySignatureInfoProvider.AddOrUpdateRepositorySignatureInfo(dir, repositorySignatureInfo);

                using (var packageStream = File.OpenRead(repoSignedPackagePath))
                using (var packageReader = new PackageArchiveReader(packageStream))
                {
                    var packageExtractionContext = new PackageExtractionContext(
                        PackageSaveMode.Nuspec | PackageSaveMode.Files,
                        PackageExtractionBehavior.XmlDocFileSaveMode,
                        clientPolicy,
                        NullLogger.Instance);

                    // Act
                    IEnumerable<string> files = await PackageExtractor.ExtractPackageAsync(
                            dir,
                            packageReader,
                            packageStream,
                            resolver,
                            packageExtractionContext,
                            CancellationToken.None);

                    // Assert
                    files.Should().NotBeNull();
                    Directory.Exists(Path.Combine(dir.Path,
                        $"{nupkg.Identity.Id}.{nupkg.Identity.Version.ToNormalizedString()}"))
                        .Should()
                        .BeTrue();
                }
            }
        }

        [PlatformFact(Platform.Windows)]
        public async Task ExtractPackageAsync_WithAllowUntrusted_SucceedsWithoutSigningWarningsOrErrors()
        {
            // Arrange
            using (var dir = TestDirectory.Create())
            using (var repoCertificate = SigningTestUtility.GenerateSelfIssuedCertificate(isCa: false))
            {
                var nupkg = new SimpleTestPackageContext();
                var resolver = new PackagePathResolver(dir);
                var fingerprintString = SignatureTestUtility.GetFingerprint(repoCertificate, HashAlgorithmName.SHA256);

                var repoSignedPackagePath = await SignedArchiveTestUtility.RepositorySignPackageAsync(
                    repoCertificate, nupkg, dir, new Uri(@"https://api.serviceindex.test/json"));

                var allowList = new List<TrustedSignerAllowListEntry>()
                {
                    new TrustedSignerAllowListEntry(VerificationTarget.Repository, SignaturePlacement.Any, fingerprintString, HashAlgorithmName.SHA256, allowUntrustedRoot: true)
                };
                var clientPolicy = new ClientPolicyContext(SignatureValidationMode.Require, allowList);

                var logger = new Mock<ILogger>();

                using (var packageStream = File.OpenRead(repoSignedPackagePath))
                using (var packageReader = new PackageArchiveReader(packageStream))
                {
                    var packageExtractionContext = new PackageExtractionContext(
                        PackageSaveMode.Nuspec | PackageSaveMode.Files,
                        PackageExtractionBehavior.XmlDocFileSaveMode,
                        clientPolicy,
                        logger.Object);

                    // Act
                    await PackageExtractor.ExtractPackageAsync(
                        dir,
                        packageReader,
                        packageStream,
                        resolver,
                        packageExtractionContext,
                        CancellationToken.None);

                    // Assert
                    logger.Verify(l => l.LogAsync(It.Is<ILogMessage>(m =>
                        m.Level == LogLevel.Warning &&
                        m.Code != NuGetLogCode.NU3018 &&
                        !m.Message.Contains(UntrustedChainCertError))), Times.AtLeastOnce);

                    logger.Verify(l => l.LogAsync(It.Is<ILogMessage>(m =>
                        m.Code == NuGetLogCode.NU3018 &&
                        (m.Level == LogLevel.Warning || m.Level == LogLevel.Error))), Times.Never);
                }
            }
        }

        [CIOnlyFact]
        public async Task ExtractPackageAsync_WithAllowUntrusted_SucceedsWithoutSigningWarningsOrErrors_WithEnvVar()
        {
            // Arrange
            var environment = new Mock<IEnvironmentVariableReader>(MockBehavior.Strict);
            environment.Setup(s => s.GetEnvironmentVariable(SignatureVerificationEnvironmentVariable)).Returns("TRUE");

            using (var dir = TestDirectory.Create())
            using (var repoCertificate = SigningTestUtility.GenerateSelfIssuedCertificate(isCa: false))
            {
                var nupkg = new SimpleTestPackageContext();
                var resolver = new PackagePathResolver(dir);
                var fingerprintString = SignatureTestUtility.GetFingerprint(repoCertificate, HashAlgorithmName.SHA256);

                var repoSignedPackagePath = await SignedArchiveTestUtility.RepositorySignPackageAsync(
                    repoCertificate, nupkg, dir, new Uri(@"https://api.serviceindex.test/json"));

                var allowList = new List<TrustedSignerAllowListEntry>()
                {
                    new TrustedSignerAllowListEntry(VerificationTarget.Repository, SignaturePlacement.Any, fingerprintString, HashAlgorithmName.SHA256, allowUntrustedRoot: true)
                };
                var clientPolicy = new ClientPolicyContext(SignatureValidationMode.Require, allowList);

                var logger = new Mock<ILogger>();

                using (var packageStream = File.OpenRead(repoSignedPackagePath))
                using (var packageReader = new PackageArchiveReader(packageStream, environmentVariableReader: environment.Object))
                {
                    var packageExtractionContext = new PackageExtractionContext(
                        PackageSaveMode.Nuspec | PackageSaveMode.Files,
                        PackageExtractionBehavior.XmlDocFileSaveMode,
                        clientPolicy,
                        logger.Object);

                    // Act
                    await PackageExtractor.ExtractPackageAsync(
                        dir,
                        packageReader,
                        packageStream,
                        resolver,
                        packageExtractionContext,
                        CancellationToken.None);

                    // Assert
                    logger.Verify(l => l.LogAsync(It.Is<ILogMessage>(m =>
                        m.Level == LogLevel.Warning &&
                        m.Code != NuGetLogCode.NU3018 &&
                        !m.Message.Contains(UntrustedChainCertError))), Times.AtLeastOnce);

                    logger.Verify(l => l.LogAsync(It.Is<ILogMessage>(m =>
                        m.Code == NuGetLogCode.NU3018 &&
                        (m.Level == LogLevel.Warning || m.Level == LogLevel.Error))), Times.Never);
                }
            }
        }

        [PlatformFact(Platform.Darwin)]
        public async Task ExtractPackageAsync_WithAllowUntrusted_SucceedsNoWarning()
        {
            // Arrange
            using (var dir = TestDirectory.Create())
            using (var repoCertificate = SigningTestUtility.GenerateSelfIssuedCertificate(isCa: false))
            {
                var nupkg = new SimpleTestPackageContext();
                var resolver = new PackagePathResolver(dir);
                string fingerprintString = SignatureTestUtility.GetFingerprint(repoCertificate, HashAlgorithmName.SHA256);

                var repoSignedPackagePath = await SignedArchiveTestUtility.RepositorySignPackageAsync(
                    repoCertificate, nupkg, dir, new Uri(@"https://api.serviceindex.test/json"));

                var allowList = new List<TrustedSignerAllowListEntry>()
                {
                    new TrustedSignerAllowListEntry(VerificationTarget.Repository, SignaturePlacement.Any, fingerprintString, HashAlgorithmName.SHA256, allowUntrustedRoot: true)
                };
                var clientPolicy = new ClientPolicyContext(SignatureValidationMode.Require, allowList);

                var logger = new Mock<ILogger>();

                using (var packageStream = File.OpenRead(repoSignedPackagePath))
                using (var packageReader = new PackageArchiveReader(packageStream))
                {
                    var packageExtractionContext = new PackageExtractionContext(
                        PackageSaveMode.Nuspec | PackageSaveMode.Files,
                        PackageExtractionBehavior.XmlDocFileSaveMode,
                        clientPolicy,
                        logger.Object);

                    // Act
                    await PackageExtractor.ExtractPackageAsync(
                        dir,
                        packageReader,
                        packageStream,
                        resolver,
                        packageExtractionContext,
                        CancellationToken.None);

                    // Assert
                    logger.Verify(l => l.LogAsync(It.Is<ILogMessage>(m =>
                        m.Level == LogLevel.Warning || m.Level == LogLevel.Error)), Times.Never);
                }
            }
        }

        [PlatformFact(Platform.Windows)]
        public async Task ExtractPackageAsync_RequireMode_UnsignedPackage_PackageArchiveReader_WhenUnsignedPackagesDisallowed_ErrorsAsync()
        {
            // Arrange
            var signedPackageVerifier = new Mock<IPackageSignatureVerifier>(MockBehavior.Strict);

            signedPackageVerifier.Setup(x => x.VerifySignaturesAsync(
                It.IsAny<ISignedPackageReader>(),
                It.IsAny<SignedPackageVerifierSettings>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<Guid>())).
                ReturnsAsync(new VerifySignaturesResult(isValid: false, isSigned: false));

            var extractionContext = new PackageExtractionContext(
                packageSaveMode: PackageSaveMode.Nuspec | PackageSaveMode.Files,
                xmlDocFileSaveMode: XmlDocFileSaveMode.None,
                clientPolicyContext: new ClientPolicyContext(SignatureValidationMode.Require, allowList: null),
                logger: NullLogger.Instance)
            {
                SignedPackageVerifier = signedPackageVerifier.Object
            };

            using (var test = new ExtractPackageAsyncTest(extractionContext))
            {
                var packageContext = new SimpleTestPackageContext();
                await SimpleTestPackageUtility.CreatePackagesAsync(test.Source, packageContext);

                var packageFile = new FileInfo(Path.Combine(test.Source,
                    $"{packageContext.Identity.Id}.{packageContext.Identity.Version.ToNormalizedString()}.nupkg"));

                using (var packageReader = new PackageArchiveReader(File.OpenRead(packageFile.FullName)))
                {
                    // Act
                    SignatureException exception = null;
                    IEnumerable<string> files = null;

                    try
                    {
                        files = await PackageExtractor.ExtractPackageAsync(
                            test.Source,
                            packageReader,
                            test.Resolver,
                            test.Context,
                            CancellationToken.None);
                    }
                    catch (SignatureException e)
                    {
                        exception = e;
                    }

                    // Assert
                    exception.Should().NotBeNull();
                    files.Should().BeNull();
                    Directory.Exists(Path.Combine(test.DestinationDirectory.FullName,
                        $"{packageContext.Identity.Id}.{packageContext.Identity.Version.ToNormalizedString()}"))
                        .Should()
                        .BeFalse();
                }
            }
        }

        [PlatformFact(Platform.Darwin)]
        public async Task ExtractPackageAsync_RequireMode_UnsignedPackage_PackageArchiveReader_WhenUnsignedPackagesDisallowed_SuccessAsync()
        {
            // Arrange
            var signedPackageVerifier = new Mock<IPackageSignatureVerifier>(MockBehavior.Strict);

            signedPackageVerifier.Setup(x => x.VerifySignaturesAsync(
                It.IsAny<ISignedPackageReader>(),
                It.IsAny<SignedPackageVerifierSettings>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<Guid>())).
                ReturnsAsync(new VerifySignaturesResult(isValid: false, isSigned: false));

            var extractionContext = new PackageExtractionContext(
                packageSaveMode: PackageSaveMode.Nuspec | PackageSaveMode.Files,
                xmlDocFileSaveMode: XmlDocFileSaveMode.None,
                clientPolicyContext: new ClientPolicyContext(SignatureValidationMode.Require, allowList: null),
                logger: NullLogger.Instance)
            {
                SignedPackageVerifier = signedPackageVerifier.Object
            };

            using (var test = new ExtractPackageAsyncTest(extractionContext))
            {
                var packageContext = new SimpleTestPackageContext();
                await SimpleTestPackageUtility.CreatePackagesAsync(test.Source, packageContext);

                var packageFile = new FileInfo(Path.Combine(test.Source,
                    $"{packageContext.Identity.Id}.{packageContext.Identity.Version.ToNormalizedString()}.nupkg"));

                using (var packageStream = File.OpenRead(packageFile.FullName))
                {
                    // Act
                    IEnumerable<string> files = await PackageExtractor.ExtractPackageAsync(
                            test.Source,
                            packageStream,
                            test.Resolver,
                            test.Context,
                            CancellationToken.None);

                    // Assert
                    files.Should().NotBeNull();
                    Directory.Exists(Path.Combine(test.DestinationDirectory.FullName,
                        $"{packageContext.Identity.Id}.{packageContext.Identity.Version.ToNormalizedString()}"))
                        .Should()
                        .BeTrue();
                }
            }
        }

        [CIOnlyFact]
        public async Task ExtractPackageAsync_RequireMode_UnsignedPackage_PackageArchiveReader_WhenUnsignedPackagesDisallowed_WithEnvVar_Errors()
        {
            // Arrange
            var environment = new Mock<IEnvironmentVariableReader>(MockBehavior.Strict);
            environment.Setup(s => s.GetEnvironmentVariable(SignatureVerificationEnvironmentVariable)).Returns("TRUE");
            var signedPackageVerifier = new Mock<IPackageSignatureVerifier>(MockBehavior.Strict);

            signedPackageVerifier.Setup(x => x.VerifySignaturesAsync(
                It.IsAny<ISignedPackageReader>(),
                It.IsAny<SignedPackageVerifierSettings>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<Guid>())).
                ReturnsAsync(new VerifySignaturesResult(isValid: false, isSigned: false));

            var extractionContext = new PackageExtractionContext(
                packageSaveMode: PackageSaveMode.Nuspec | PackageSaveMode.Files,
                xmlDocFileSaveMode: XmlDocFileSaveMode.None,
                clientPolicyContext: new ClientPolicyContext(SignatureValidationMode.Require, allowList: null),
                logger: NullLogger.Instance)
            {
                SignedPackageVerifier = signedPackageVerifier.Object
            };

            using (var test = new ExtractPackageAsyncTest(extractionContext))
            {
                var packageContext = new SimpleTestPackageContext();
                await SimpleTestPackageUtility.CreatePackagesAsync(test.Source, packageContext);

                var packageFile = new FileInfo(Path.Combine(test.Source,
                    $"{packageContext.Identity.Id}.{packageContext.Identity.Version.ToNormalizedString()}.nupkg"));

                using (var packageReader = new PackageArchiveReader(File.OpenRead(packageFile.FullName), environmentVariableReader: environment.Object))
                {
                    // Act
                    SignatureException exception = null;
                    IEnumerable<string> files = null;

                    try
                    {
                        files = await PackageExtractor.ExtractPackageAsync(
                            test.Source,
                            packageReader,
                            test.Resolver,
                            test.Context,
                            CancellationToken.None);
                    }
                    catch (SignatureException e)
                    {
                        exception = e;
                    }

                    // Assert
                    exception.Should().NotBeNull();
                    files.Should().BeNull();
                    Directory.Exists(Path.Combine(test.DestinationDirectory.FullName,
                        $"{packageContext.Identity.Id}.{packageContext.Identity.Version.ToNormalizedString()}"))
                        .Should()
                        .BeFalse();
                }
            }
        }

        [Fact]
        public async Task ExtractPackageAsync_RequireMode_UnsignedPackage_PackageFolderReader_WhenUnsignedPackagesDisallowed_SkipsSigningVerificationAsync()
        {
            // Arrange
            var signedPackageVerifier = new Mock<IPackageSignatureVerifier>(MockBehavior.Strict);

            signedPackageVerifier.Setup(x => x.VerifySignaturesAsync(
                It.IsAny<ISignedPackageReader>(),
                It.IsAny<SignedPackageVerifierSettings>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<Guid>())).
                ReturnsAsync(new VerifySignaturesResult(isValid: false, isSigned: false));

            var extractionContext = new PackageExtractionContext(
                packageSaveMode: PackageSaveMode.Nuspec | PackageSaveMode.Files,
                xmlDocFileSaveMode: XmlDocFileSaveMode.None,
                clientPolicyContext: new ClientPolicyContext(SignatureValidationMode.Require, allowList: null),
                logger: NullLogger.Instance)
            {
                SignedPackageVerifier = signedPackageVerifier.Object
            };

            using (var test = new ExtractPackageAsyncTest(extractionContext))
            using (var packageDir = TestDirectory.Create())
            {
                var packageContext = new SimpleTestPackageContext();
                await SimpleTestPackageUtility.CreatePackagesAsync(test.Source, packageContext);

                var packageFile = new FileInfo(Path.Combine(test.Source,
                    $"{packageContext.Identity.Id}.{packageContext.Identity.Version.ToNormalizedString()}.nupkg"));

                using (var zipFile = new ZipArchive(File.OpenRead(packageFile.FullName)))
                {
                    zipFile.ExtractAll(packageDir.Path);
                }

                using (var packageReader = new PackageFolderReader(packageDir))
                {
                    // Act
                    IEnumerable<string> files = await PackageExtractor.ExtractPackageAsync(
                            test.Source,
                            packageReader,
                            test.Resolver,
                            test.Context,
                            CancellationToken.None);

                    // Assert
                    files.Should().NotBeNull();
                    files.Count().Should().Be(8);
                    var packagePath = Path.Combine(test.DestinationDirectory.FullName,
                        $"{packageContext.Identity.Id}.{packageContext.Identity.Version.ToNormalizedString()}");

                    Directory.Exists(packagePath).Should().BeTrue();
                    File.Exists(Path.Combine(packagePath,
                        $"{packageContext.Id}.nuspec")).Should().BeTrue();
                    File.Exists(Path.Combine(packagePath,
                        "contentFiles/any/any/config.xml")).Should().BeTrue();
                    File.Exists(Path.Combine(packagePath,
                        "contentFiles/cs/net45/code.cs")).Should().BeTrue();
                    File.Exists(Path.Combine(packagePath,
                        "lib/net45/a.dll")).Should().BeTrue();
                    File.Exists(Path.Combine(packagePath,
                        "lib/netstandard1.0/a.dll")).Should().BeTrue();
                    File.Exists(Path.Combine(packagePath,
                        $"build/net45/{packageContext.Id}.targets")).Should().BeTrue();
                    File.Exists(Path.Combine(packagePath,
                        "runtimes/any/native/a.dll")).Should().BeTrue();
                    File.Exists(Path.Combine(packagePath,
                        "tools/a.exe")).Should().BeTrue();
                }
            }
        }

        [Fact]
        public async Task ExtractPackageAsync_RequireMode_UnsignedPackage_PluginPackageReader_WhenUnsignedPackagesDisallowed_ErrorsAsync()
        {
            // Arrange
            var signedPackageVerifier = new Mock<IPackageSignatureVerifier>(MockBehavior.Strict);

            signedPackageVerifier.Setup(x => x.VerifySignaturesAsync(
                It.IsAny<ISignedPackageReader>(),
                It.IsAny<SignedPackageVerifierSettings>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<Guid>())).
                ReturnsAsync(new VerifySignaturesResult(isValid: false, isSigned: false));

            var extractionContext = new PackageExtractionContext(
                packageSaveMode: PackageSaveMode.Nuspec | PackageSaveMode.Files,
                xmlDocFileSaveMode: XmlDocFileSaveMode.None,
                clientPolicyContext: new ClientPolicyContext(SignatureValidationMode.Require, allowList: null),
                logger: NullLogger.Instance)
            {
                SignedPackageVerifier = signedPackageVerifier.Object
            };

            using (var test = new ExtractPackageAsyncTest(extractionContext))
            using (var packageDir = TestDirectory.Create())
            {
                var packageIdentity = new PackageIdentity(id: "a", version: NuGetVersion.Parse("1.0.0"));
                var packageSource = new PackageSource("https://unit.test");

                var connection = new Mock<IConnection>(MockBehavior.Strict);

                connection.Setup(x => x.SendRequestAndReceiveResponseAsync<GetFilesInPackageRequest, GetFilesInPackageResponse>(
                       It.Is<MessageMethod>(m => m == MessageMethod.GetFilesInPackage),
                       It.Is<GetFilesInPackageRequest>(c => c.PackageSourceRepository == packageSource.Source
                           && c.PackageId == packageIdentity.Id
                           && c.PackageVersion == packageIdentity.Version.ToNormalizedString()),
                       It.IsAny<CancellationToken>()))
                   .ReturnsAsync(new GetFilesInPackageResponse(MessageResponseCode.Success, new[] { $"{packageIdentity.Id}.nuspec" }));

                CopyFilesInPackageResponse response = null;

                connection.Setup(x => x.SendRequestAndReceiveResponseAsync<CopyFilesInPackageRequest, CopyFilesInPackageResponse>(
                        It.Is<MessageMethod>(m => m == MessageMethod.CopyFilesInPackage),
                        It.Is<CopyFilesInPackageRequest>(c => c.PackageSourceRepository == packageSource.Source
                            && c.PackageId == packageIdentity.Id
                            && c.PackageVersion == packageIdentity.Version.ToNormalizedString()
                            && c.FilesInPackage.Count() == 1),
                        It.IsAny<CancellationToken>()))
                    .Callback<MessageMethod, CopyFilesInPackageRequest, CancellationToken>(
                        (method, request, cancellationToken) =>
                        {
                            var copiedFiles = new List<string>();

                            foreach (var fileInPackage in request.FilesInPackage)
                            {
                                var filePath = Path.Combine(request.DestinationFolderPath, fileInPackage);
                                var content = $@"<?xml version=""1.0"" encoding=""utf-8""?>
                                <package>
                                    <metadata minClientVersion=""1.2.3"">
                                        <id>{packageIdentity.Id}</id>
                                        <version>{packageIdentity.Version.ToNormalizedString()}</version>
                                    </metadata>
                                </package>";

                                File.WriteAllText(filePath, content);

                                copiedFiles.Add(filePath);
                            }

                            response = new CopyFilesInPackageResponse(MessageResponseCode.Success, copiedFiles);
                        }
                    )
                    .ReturnsAsync(() => response);

                var plugin = new Mock<IPlugin>(MockBehavior.Strict);

                plugin.Setup(x => x.Dispose());
                plugin.SetupGet(x => x.Name)
                    .Returns("b");
                plugin.SetupGet(x => x.Connection)
                    .Returns(connection.Object);

                using (var packageReader = new PluginPackageReader(plugin.Object, packageIdentity, packageSource.Source))
                {
                    // Act
                    SignatureException exception = null;
                    IEnumerable<string> files = null;

                    try
                    {
                        files = await PackageExtractor.ExtractPackageAsync(
                            test.Source,
                            packageReader,
                            test.Resolver,
                            test.Context,
                            CancellationToken.None);
                    }
                    catch (SignatureException e)
                    {
                        exception = e;
                    }

                    // Assert
                    exception.Should().NotBeNull();
                    files.Should().BeNull();
                    Directory.Exists(Path.Combine(test.DestinationDirectory.FullName,
                        $"{packageIdentity.Id}.{packageIdentity.Version.ToNormalizedString()}")).Should().BeFalse();
                }
            }
        }
#endif

#if IS_CORECLR && !IS_SIGNING_SUPPORTED
        [Fact]
        public async Task ExtractPackageAsync_RequireMode_UnsignedPackage_InCoreCLR_SkipsSigningVerificationAsync()
        {
            // Arrange
            var signedPackageVerifier = new Mock<IPackageSignatureVerifier>(MockBehavior.Strict);

            signedPackageVerifier.Setup(x => x.VerifySignaturesAsync(
                It.IsAny<ISignedPackageReader>(),
                It.IsAny<SignedPackageVerifierSettings>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<Guid>())).
                ReturnsAsync(new VerifySignaturesResult(isValid: false, isSigned: false));

            var extractionContext = new PackageExtractionContext(
                packageSaveMode: PackageSaveMode.Nuspec | PackageSaveMode.Files,
                xmlDocFileSaveMode: XmlDocFileSaveMode.None,
                clientPolicyContext: new ClientPolicyContext(SignatureValidationMode.Require, allowList: null),
                logger: NullLogger.Instance)
            {
                SignedPackageVerifier = signedPackageVerifier.Object
            };

            using (var test = new ExtractPackageAsyncTest(extractionContext))
            {

                var packageContext = new SimpleTestPackageContext();
                await SimpleTestPackageUtility.CreatePackagesAsync(test.Source, packageContext);

                var packageFile = new FileInfo(Path.Combine(test.Source,
                    $"{packageContext.Identity.Id}.{packageContext.Identity.Version.ToNormalizedString()}.nupkg"));

                using (var packageReader = new PackageArchiveReader(File.OpenRead(packageFile.FullName)))
                {
                    // Act
                    IEnumerable<string> files = await PackageExtractor.ExtractPackageAsync(
                            test.Source,
                            packageReader,
                            test.Resolver,
                            test.Context,
                            CancellationToken.None);

                    // Assert
                    files.Should().NotBeNull();
                    files.Count().Should().Be(8);
                    var packagePath = Path.Combine(test.DestinationDirectory.FullName,
                        $"{packageContext.Identity.Id}.{packageContext.Identity.Version.ToNormalizedString()}");

                    Directory.Exists(packagePath).Should().BeTrue();
                    File.Exists(Path.Combine(packagePath,
                        $"{packageContext.Id}.nuspec")).Should().BeTrue();
                    File.Exists(Path.Combine(packagePath,
                        "contentFiles/any/any/config.xml")).Should().BeTrue();
                    File.Exists(Path.Combine(packagePath,
                        "contentFiles/cs/net45/code.cs")).Should().BeTrue();
                    File.Exists(Path.Combine(packagePath,
                        "lib/net45/a.dll")).Should().BeTrue();
                    File.Exists(Path.Combine(packagePath,
                        "lib/netstandard1.0/a.dll")).Should().BeTrue();
                    File.Exists(Path.Combine(packagePath,
                        $"build/net45/{packageContext.Id}.targets")).Should().BeTrue();
                    File.Exists(Path.Combine(packagePath,
                        "runtimes/any/native/a.dll")).Should().BeTrue();
                    File.Exists(Path.Combine(packagePath,
                        "tools/a.exe")).Should().BeTrue();
                }
            }
        }
#endif

        [Fact]
        public async Task InstallFromSourceAsync_WithoutPackageSaveModeNuspec_DoesNotExtractNuspecAsync()
        {
            // Arrange
            using (var root = TestDirectory.Create())
            {
                var resolver = new VersionFolderPathResolver(root);
                var identity = new PackageIdentity("A", new NuGetVersion("2.0.3"));
                var packageFileInfo = await TestPackagesCore.GeneratePackageAsync(
                   root,
                   identity.Id,
                   identity.Version.ToString(),
                   DateTimeOffset.UtcNow.LocalDateTime,
                   "lib/net45/A.dll");

                var pathResolver = new VersionFolderPathResolver(root);

                using (var packageDownloader = new LocalPackageArchiveDownloader(
                    root,
                    packageFileInfo.FullName,
                    identity,
                    NullLogger.Instance))
                {
                    // Act
                    await PackageExtractor.InstallFromSourceAsync(
                        identity,
                        packageDownloader,
                        pathResolver,
                        new PackageExtractionContext(
                            packageSaveMode: PackageSaveMode.Nupkg | PackageSaveMode.Files,
                            xmlDocFileSaveMode: XmlDocFileSaveMode.None,
                            clientPolicyContext: null,
                            logger: NullLogger.Instance),
                        CancellationToken.None);

                    // Assert
                    Assert.True(File.Exists(resolver.GetPackageFilePath(identity.Id, identity.Version)), "The .nupkg should exist.");
                    Assert.False(File.Exists(resolver.GetManifestFilePath(identity.Id, identity.Version)), "The .nuspec should not exist.");
                    Assert.True(File.Exists(Path.Combine(resolver.GetInstallPath(identity.Id, identity.Version), "lib", "net45", "A.dll")), "The asset should exist.");
                }
            }
        }

        [Fact]
        public async Task InstallFromSourceAsync_WithoutPackageSaveModeNupkg_DoesNotExtractNupkgAsync()
        {
            // Arrange
            using (var root = TestDirectory.Create())
            {
                var resolver = new VersionFolderPathResolver(root);
                var identity = new PackageIdentity("A", new NuGetVersion("2.0.3"));
                var packageFileInfo = await TestPackagesCore.GeneratePackageAsync(
                   root,
                   identity.Id,
                   identity.Version.ToString(),
                   DateTimeOffset.UtcNow.LocalDateTime,
                   "lib/net45/A.dll");
                var pathResolver = new VersionFolderPathResolver(root);

                using (var packageDownloader = new LocalPackageArchiveDownloader(
                    root,
                    packageFileInfo.FullName,
                    identity,
                    NullLogger.Instance))
                {
                    // Act
                    await PackageExtractor.InstallFromSourceAsync(
                        identity,
                        packageDownloader,
                        pathResolver,
                        new PackageExtractionContext(
                            packageSaveMode: PackageSaveMode.Nuspec | PackageSaveMode.Files,
                            xmlDocFileSaveMode: XmlDocFileSaveMode.None,
                            clientPolicyContext: null,
                            logger: NullLogger.Instance),
                        CancellationToken.None);

                    // Assert
                    Assert.False(File.Exists(resolver.GetPackageFilePath(identity.Id, identity.Version)), "The .nupkg should not exist.");
                    Assert.True(File.Exists(resolver.GetManifestFilePath(identity.Id, identity.Version)), "The .nuspec should exist.");
                    Assert.True(File.Exists(Path.Combine(resolver.GetInstallPath(identity.Id, identity.Version), "lib", "net45", "A.dll")), "The asset should exist.");

                }
            }
        }

        [Fact]
        public async Task InstallFromSourceAsync_WithoutPackageSaveModeFiles_DoesNotExtractFilesAsync()
        {
            // Arrange
            using (var root = TestDirectory.Create())
            {
                var resolver = new VersionFolderPathResolver(root);
                var identity = new PackageIdentity("A", new NuGetVersion("2.0.3"));
                var packageFileInfo = await TestPackagesCore.GeneratePackageAsync(
                   root,
                   identity.Id,
                   identity.Version.ToString(),
                   DateTimeOffset.UtcNow.LocalDateTime,
                   "lib/net45/A.dll");
                var pathResolver = new VersionFolderPathResolver(root);

                using (var packageDownloader = new LocalPackageArchiveDownloader(
                    root,
                    packageFileInfo.FullName,
                    identity,
                    NullLogger.Instance))
                {
                    // Act
                    await PackageExtractor.InstallFromSourceAsync(
                        identity,
                        packageDownloader,
                        pathResolver,
                        new PackageExtractionContext(
                            packageSaveMode: PackageSaveMode.Nupkg | PackageSaveMode.Nuspec,
                            xmlDocFileSaveMode: XmlDocFileSaveMode.None,
                            clientPolicyContext: null,
                            logger: NullLogger.Instance),
                        CancellationToken.None);

                    // Assert
                    Assert.True(File.Exists(resolver.GetPackageFilePath(identity.Id, identity.Version)), "The .nupkg should exist.");
                    Assert.True(File.Exists(resolver.GetManifestFilePath(identity.Id, identity.Version)), "The .nuspec should exist.");
                    Assert.False(File.Exists(Path.Combine(resolver.GetInstallPath(identity.Id, identity.Version), "lib", "net45", "A.dll")), "The asset should not exist.");
                }
            }
        }

        [Fact]
        public async Task InstallFromSourceAsync_DoesNotMoveNuspecFileIfFileNameIsSameAsExpectedAsync()
        {
            var nuspecFileName = "a.nuspec";
            var packageDownloader = new Mock<IPackageDownloader>(MockBehavior.Strict);
            var coreReader = new Mock<IAsyncPackageCoreReader>(MockBehavior.Strict);
            var signedReader = new Mock<ISignedPackageReader>(MockBehavior.Strict);

            packageDownloader.Setup(x => x.CopyNupkgFileToAsync(It.IsNotNull<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);
            packageDownloader.Setup(x => x.GetPackageHashAsync(It.IsNotNull<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync("hash");
            packageDownloader.SetupGet(x => x.CoreReader)
                .Returns(coreReader.Object);
            packageDownloader.SetupGet(x => x.SignedPackageReader)
                .Returns(signedReader.Object);
            packageDownloader.Setup(x => x.Dispose());
            packageDownloader.SetupGet(x => x.Source)
                .Returns(string.Empty);

            coreReader.Setup(x => x.GetNuspecFileAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(nuspecFileName);

            IEnumerable<string> copiedFilePaths = null;

            coreReader.Setup(x => x.CopyFilesAsync(
                    It.IsNotNull<string>(),
                    It.IsNotNull<IEnumerable<string>>(),
                    It.IsNotNull<ExtractPackageFileDelegate>(),
                    It.IsNotNull<ILogger>(),
                    It.IsAny<CancellationToken>()))
                .Callback<string, IEnumerable<string>, ExtractPackageFileDelegate, ILogger, CancellationToken>(
                    (destination, packageFiles, extractFileDelegate, logger, cancellationToken) =>
                    {
                        var copiedFilePath = Path.Combine(destination, nuspecFileName);

                        File.WriteAllText(copiedFilePath, null);

                        copiedFilePaths = new[] { copiedFilePath };
                    })
                .ReturnsAsync(() => copiedFilePaths);

            signedReader.Setup(x => x.GetContentHash(It.IsAny<CancellationToken>(), It.IsAny<Func<string>>()))
                .Returns(string.Empty);

            var packageIdentity = new PackageIdentity(id: "a", version: NuGetVersion.Parse("1.0.0"));

            using (var testDirectory = TestDirectory.Create())
            {
                var packageExtractionContext = new PackageExtractionContext(
                    PackageSaveMode.Nuspec,
                    XmlDocFileSaveMode.None,
                    clientPolicyContext: null,
                    logger: NullLogger.Instance);
                var versionFolderPathResolver = new VersionFolderPathResolver(testDirectory.Path);
                var installDirectoryPath = versionFolderPathResolver.GetInstallPath(packageIdentity.Id, packageIdentity.Version);
                var expectedNuspecFilePath = Path.Combine(installDirectoryPath, nuspecFileName);

                var wasInstalled = await PackageExtractor.InstallFromSourceAsync(
                    packageIdentity,
                    packageDownloader.Object,
                    versionFolderPathResolver,
                    packageExtractionContext,
                    CancellationToken.None);

                Assert.True(wasInstalled);
                Assert.True(File.Exists(expectedNuspecFilePath));
                Assert.True(FileExistsCaseSensitively(expectedNuspecFilePath));
            }

            coreReader.Verify();
            packageDownloader.Verify();
        }

        [Fact]
        public async Task InstallFromSourceAsync_MovesNuspecFileIfFileNameIsDifferentThanExpectedAsync()
        {
            var intendedNuspecFileName = "a.nuspec";
            var actualNuspecFileName = intendedNuspecFileName.ToUpperInvariant();
            var packageDownloader = new Mock<IPackageDownloader>(MockBehavior.Strict);
            var coreReader = new Mock<IAsyncPackageCoreReader>(MockBehavior.Strict);
            var signedReader = new Mock<ISignedPackageReader>(MockBehavior.Strict);

            packageDownloader.Setup(x => x.CopyNupkgFileToAsync(It.IsNotNull<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);
            packageDownloader.Setup(x => x.GetPackageHashAsync(It.IsNotNull<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync("hash");
            packageDownloader.SetupGet(x => x.CoreReader)
                .Returns(coreReader.Object);
            packageDownloader.SetupGet(x => x.SignedPackageReader)
                .Returns(signedReader.Object);
            packageDownloader.Setup(x => x.Dispose());
            packageDownloader.SetupGet(x => x.Source)
                .Returns(string.Empty);

            coreReader.Setup(x => x.GetNuspecFileAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(actualNuspecFileName);

            IEnumerable<string> copiedFilePaths = null;

            coreReader.Setup(x => x.CopyFilesAsync(
                    It.IsNotNull<string>(),
                    It.IsNotNull<IEnumerable<string>>(),
                    It.IsNotNull<ExtractPackageFileDelegate>(),
                    It.IsNotNull<ILogger>(),
                    It.IsAny<CancellationToken>()))
                .Callback<string, IEnumerable<string>, ExtractPackageFileDelegate, ILogger, CancellationToken>(
                    (destination, packageFiles, extractFileDelegate, logger, cancellationToken) =>
                    {
                        var copiedFilePath = Path.Combine(destination, actualNuspecFileName);

                        File.WriteAllText(copiedFilePath, null);

                        copiedFilePaths = new[] { copiedFilePath };
                    })
                .ReturnsAsync(() => copiedFilePaths);

            signedReader.Setup(x => x.GetContentHash(It.IsAny<CancellationToken>(), It.IsAny<Func<string>>()))
                .Returns(string.Empty);

            var packageIdentity = new PackageIdentity(id: "a", version: NuGetVersion.Parse("1.0.0"));

            using (var testDirectory = TestDirectory.Create())
            {
                var packageExtractionContext = new PackageExtractionContext(
                    PackageSaveMode.Nuspec,
                    XmlDocFileSaveMode.None,
                    clientPolicyContext: null,
                    logger: NullLogger.Instance);

                var versionFolderPathResolver = new VersionFolderPathResolver(testDirectory.Path);
                var installDirectoryPath = versionFolderPathResolver.GetInstallPath(packageIdentity.Id, packageIdentity.Version);
                var intendedNuspecFilePath = Path.Combine(installDirectoryPath, intendedNuspecFileName);

                var wasInstalled = await PackageExtractor.InstallFromSourceAsync(
                    packageIdentity,
                    packageDownloader.Object,
                    versionFolderPathResolver,
                    packageExtractionContext,
                    CancellationToken.None);

                Assert.True(wasInstalled);
                Assert.True(File.Exists(intendedNuspecFilePath));
                Assert.True(FileExistsCaseSensitively(intendedNuspecFilePath));
            }

            coreReader.Verify();
            packageDownloader.Verify();
        }

#if IS_SIGNING_SUPPORTED
        [Fact]
        public async Task InstallFromSourceAsyncByPackageDownloader_TrustedSignPackageAsync()
        {
            // Arrange
            using (var root = TestDirectory.Create())
            {
                var resolver = new VersionFolderPathResolver(root);

                var nupkg = new SimpleTestPackageContext("A", "1.0.0");

                var signedPackageVerifier = new Mock<IPackageSignatureVerifier>(MockBehavior.Strict);

                signedPackageVerifier.Setup(x => x.VerifySignaturesAsync(
                    It.IsAny<ISignedPackageReader>(),
                    It.Is<SignedPackageVerifierSettings>(s => SigningTestUtility.AreVerifierSettingsEqual(s, _defaultContext.VerifierSettings)),
                    It.IsAny<CancellationToken>(),
                    It.IsAny<Guid>())).
                    ReturnsAsync(new VerifySignaturesResult(isValid: true, isSigned: true));

                var packageFileInfo = await SimpleTestPackageUtility.CreateFullPackageAsync(root, nupkg);

                var identity = new PackageIdentity("A", new NuGetVersion("1.0.0"));

                using (var packageDownloader = new LocalPackageArchiveDownloader(
                    root,
                    packageFileInfo.FullName,
                    identity,
                    NullLogger.Instance))
                {
                    var extractionContext = new PackageExtractionContext(
                            packageSaveMode: PackageSaveMode.Nuspec | PackageSaveMode.Files,
                            xmlDocFileSaveMode: XmlDocFileSaveMode.None,
                            clientPolicyContext: _defaultContext,
                            logger: NullLogger.Instance)
                    {
                        SignedPackageVerifier = signedPackageVerifier.Object
                    };

                    // Act
                    await PackageExtractor.InstallFromSourceAsync(
                        identity,
                        packageDownloader,
                        resolver,
                        extractionContext,
                        CancellationToken.None);

                    // Assert
                    Assert.False(File.Exists(resolver.GetPackageFilePath(identity.Id, identity.Version)), "The .nupkg should not exist.");
                    Assert.True(File.Exists(resolver.GetManifestFilePath(identity.Id, identity.Version)), "The .nuspec should exist.");
                    Assert.True(File.Exists(Path.Combine(resolver.GetInstallPath(identity.Id, identity.Version), "lib", "net45", "a.dll")), "The asset should exist.");
                }
            }
        }

        [PlatformFact(Platform.Windows)]
        public async Task InstallFromSourceAsyncByPackageDownloader_InvalidSignPackageWithUnzip_ThrowsAsync()
        {
            // Arrange
            using (var root = TestDirectory.Create())
            {
                var resolver = new VersionFolderPathResolver(root);

                var nupkg = new SimpleTestPackageContext("A", "1.0.0");
                var packageFileInfo = await SimpleTestPackageUtility.CreateFullPackageAsync(root, nupkg);

                var signedPackageVerifier = new Mock<IPackageSignatureVerifier>(MockBehavior.Strict);

                signedPackageVerifier.Setup(x => x.VerifySignaturesAsync(
                    It.IsAny<ISignedPackageReader>(),
                    It.Is<SignedPackageVerifierSettings>(s => SigningTestUtility.AreVerifierSettingsEqual(s, _defaultContext.VerifierSettings)),
                    It.IsAny<CancellationToken>(),
                    It.IsAny<Guid>())).
                    ReturnsAsync(new VerifySignaturesResult(isValid: false, isSigned: true));

                var identity = new PackageIdentity("A", new NuGetVersion("1.0.0"));

                using (var packageDownloader = new LocalPackageArchiveDownloader(
                    root,
                    packageFileInfo.FullName,
                    identity,
                    NullLogger.Instance))
                {
                    var packageInstallPath = resolver.GetInstallPath(identity.Id, identity.Version);
                    var packageInstallDirectory = Directory.GetParent(packageInstallPath);

                    var extractionContext = new PackageExtractionContext(
                     packageSaveMode: PackageSaveMode.Nuspec | PackageSaveMode.Files,
                     xmlDocFileSaveMode: XmlDocFileSaveMode.None,
                     clientPolicyContext: _defaultContext,
                     logger: NullLogger.Instance)
                    {
                        SignedPackageVerifier = signedPackageVerifier.Object
                    };

                    // Act & Assert
                    await Assert.ThrowsAsync<SignatureException>(
                     () => PackageExtractor.InstallFromSourceAsync(
                        identity,
                        packageDownloader,
                        resolver,
                        extractionContext,
                        CancellationToken.None));

                    // Assert that no footprint is left
                    Directory.Exists(packageInstallPath).Should().BeFalse();
                    Directory.Exists(packageInstallDirectory.FullName).Should().BeFalse();
                    File.Exists(Path.Combine(resolver.GetInstallPath(identity.Id, identity.Version), "lib", "net45", "a.dll")).Should().BeFalse();
                }
            }
        }

        [PlatformFact(Platform.Darwin)]
        public async Task InstallFromSourceAsyncByPackageDownloader_InvalidSignPackageWithUnzip_SuccessAsync()
        {
            // Arrange
            using (var root = TestDirectory.Create())
            {
                var resolver = new VersionFolderPathResolver(root);

                var nupkg = new SimpleTestPackageContext("A", "1.0.0");
                var packageFileInfo = await SimpleTestPackageUtility.CreateFullPackageAsync(root, nupkg);

                var signedPackageVerifier = new Mock<IPackageSignatureVerifier>(MockBehavior.Strict);

                signedPackageVerifier.Setup(x => x.VerifySignaturesAsync(
                    It.IsAny<ISignedPackageReader>(),
                    It.Is<SignedPackageVerifierSettings>(s => SigningTestUtility.AreVerifierSettingsEqual(s, _defaultContext.VerifierSettings)),
                    It.IsAny<CancellationToken>(),
                    It.IsAny<Guid>())).
                    ReturnsAsync(new VerifySignaturesResult(isValid: false, isSigned: true));

                var identity = new PackageIdentity("A", new NuGetVersion("1.0.0"));

                using (var packageDownloader = new LocalPackageArchiveDownloader(
                    root,
                    packageFileInfo.FullName,
                    identity,
                    NullLogger.Instance))
                {
                    var packageInstallPath = resolver.GetInstallPath(identity.Id, identity.Version);
                    var packageInstallDirectory = Directory.GetParent(packageInstallPath);

                    var extractionContext = new PackageExtractionContext(
                     packageSaveMode: PackageSaveMode.Nuspec | PackageSaveMode.Files,
                     xmlDocFileSaveMode: XmlDocFileSaveMode.None,
                     clientPolicyContext: _defaultContext,
                     logger: NullLogger.Instance)
                    {
                        SignedPackageVerifier = signedPackageVerifier.Object
                    };

                    // Act
                    await PackageExtractor.InstallFromSourceAsync(
                        identity,
                        packageDownloader,
                        resolver,
                        extractionContext,
                        CancellationToken.None);

                    // Assert that footprint is left
                    Directory.Exists(packageInstallPath).Should().BeTrue();
                    Directory.Exists(packageInstallDirectory.FullName).Should().BeTrue();
                    File.Exists(Path.Combine(resolver.GetInstallPath(identity.Id, identity.Version), "lib", "net45", "a.dll")).Should().BeTrue();
                }
            }
        }

        [Fact]
        public async Task InstallFromSourceAsyncByPackageDownloader_InvalidSignPackageWithoutUnzipAsync()
        {
            // Arrange
            using (var root = TestDirectory.Create())
            {
                var resolver = new VersionFolderPathResolver(root);

                var nupkg = new SimpleTestPackageContext("A", "1.0.0");

                var signedPackageVerifier = new Mock<IPackageSignatureVerifier>(MockBehavior.Strict);

                signedPackageVerifier.Setup(x => x.VerifySignaturesAsync(
                    It.IsAny<ISignedPackageReader>(),
                    It.Is<SignedPackageVerifierSettings>(s => SigningTestUtility.AreVerifierSettingsEqual(s, _defaultContext.VerifierSettings)),
                    It.IsAny<CancellationToken>(),
                    It.IsAny<Guid>())).
                    ReturnsAsync(new VerifySignaturesResult(isValid: false, isSigned: true));

                var packageFileInfo = await SimpleTestPackageUtility.CreateFullPackageAsync(root, nupkg);

                var identity = new PackageIdentity("A", new NuGetVersion("1.0.0"));

                using (var packageDownloader = new LocalPackageArchiveDownloader(
                    root,
                    packageFileInfo.FullName,
                    identity,
                    NullLogger.Instance))
                {
                    var extractionContext = new PackageExtractionContext(
                         packageSaveMode: PackageSaveMode.Nupkg,
                         xmlDocFileSaveMode: XmlDocFileSaveMode.None,
                         clientPolicyContext: _defaultContext,
                         logger: NullLogger.Instance)
                    {
                        SignedPackageVerifier = signedPackageVerifier.Object
                    };

                    // Act
                    await PackageExtractor.InstallFromSourceAsync(
                        identity,
                        packageDownloader,
                        resolver,
                        extractionContext,
                        CancellationToken.None);

                    // Assert
                    Assert.True(File.Exists(resolver.GetPackageFilePath(identity.Id, identity.Version)), "The .nupkg should not exist.");
                    Assert.False(File.Exists(resolver.GetManifestFilePath(identity.Id, identity.Version)), "The .nuspec should exist.");
                    Assert.False(File.Exists(Path.Combine(resolver.GetInstallPath(identity.Id, identity.Version), "lib", "net45", "a.dll")), "The asset should exist.");
                }
            }
        }

        [Fact]
        public async Task InstallFromSourceAsyncByStream_TrustedSignPackageAsync()
        {
            // Arrange
            using (var root = TestDirectory.Create())
            {
                var resolver = new VersionFolderPathResolver(root);

                var nupkg = new SimpleTestPackageContext("A", "1.0.0");

                var signedPackageVerifier = new Mock<IPackageSignatureVerifier>(MockBehavior.Strict);

                signedPackageVerifier.Setup(x => x.VerifySignaturesAsync(
                    It.IsAny<ISignedPackageReader>(),
                    It.Is<SignedPackageVerifierSettings>(s => SigningTestUtility.AreVerifierSettingsEqual(s, _defaultContext.VerifierSettings)),
                    It.IsAny<CancellationToken>(),
                    It.IsAny<Guid>())).
                    ReturnsAsync(new VerifySignaturesResult(isValid: true, isSigned: true));

                var packageFileInfo = await SimpleTestPackageUtility.CreateFullPackageAsync(root, nupkg);

                var identity = new PackageIdentity("A", new NuGetVersion("1.0.0"));

                using (var fileStream = File.OpenRead(packageFileInfo.FullName))
                {
                    var extractionContext = new PackageExtractionContext(
                         packageSaveMode: PackageSaveMode.Nuspec | PackageSaveMode.Files,
                         xmlDocFileSaveMode: XmlDocFileSaveMode.None,
                         clientPolicyContext: _defaultContext,
                         logger: NullLogger.Instance)
                    {
                        SignedPackageVerifier = signedPackageVerifier.Object
                    };

                    // Act
                    await PackageExtractor.InstallFromSourceAsync(
                        root,
                        identity,
                        (stream) => fileStream.CopyToAsync(stream, 4096, CancellationToken.None),
                        resolver,
                        extractionContext,
                        CancellationToken.None);

                    // Assert
                    Assert.False(File.Exists(resolver.GetPackageFilePath(identity.Id, identity.Version)), "The .nupkg should not exist.");
                    Assert.True(File.Exists(resolver.GetManifestFilePath(identity.Id, identity.Version)), "The .nuspec should exist.");
                    Assert.True(File.Exists(Path.Combine(resolver.GetInstallPath(identity.Id, identity.Version), "lib", "net45", "a.dll")), "The asset should exist.");
                    Assert.True(File.Exists(resolver.GetNupkgMetadataPath(identity.Id, identity.Version)), "The .nupkg.metadata should exist.");
                }
            }
        }

        [PlatformFact(Platform.Windows)]
        public async Task InstallFromSourceAsyncByStream_InvalidSignPackageWithoutUnzipAsync()
        {
            // Arrange
            using (var root = TestDirectory.Create())
            {
                var resolver = new VersionFolderPathResolver(root);

                var nupkg = new SimpleTestPackageContext("A", "1.0.0");

                var signedPackageVerifier = new Mock<IPackageSignatureVerifier>(MockBehavior.Strict);

                signedPackageVerifier.Setup(x => x.VerifySignaturesAsync(
                    It.IsAny<ISignedPackageReader>(),
                    It.Is<SignedPackageVerifierSettings>(s => SigningTestUtility.AreVerifierSettingsEqual(s, _defaultContext.VerifierSettings)),
                    It.IsAny<CancellationToken>(),
                    It.IsAny<Guid>())).
                    ReturnsAsync(new VerifySignaturesResult(isValid: false, isSigned: true));

                var packageFileInfo = await SimpleTestPackageUtility.CreateFullPackageAsync(root, nupkg);

                var identity = new PackageIdentity("A", new NuGetVersion("1.0.0"));

                using (var fileStream = File.OpenRead(packageFileInfo.FullName))
                {
                    var extractionContext = new PackageExtractionContext(
                         packageSaveMode: PackageSaveMode.Nupkg,
                         xmlDocFileSaveMode: XmlDocFileSaveMode.None,
                         clientPolicyContext: _defaultContext,
                         logger: NullLogger.Instance)
                    {
                        SignedPackageVerifier = signedPackageVerifier.Object
                    };

                    // Act
                    await PackageExtractor.InstallFromSourceAsync(
                        root,
                        identity,
                        (stream) => fileStream.CopyToAsync(stream, 4096, CancellationToken.None),
                        resolver,
                        extractionContext,
                        CancellationToken.None);

                    // Assert
                    Assert.True(File.Exists(resolver.GetPackageFilePath(identity.Id, identity.Version)), "The .nupkg should not exist.");
                    Assert.False(File.Exists(resolver.GetManifestFilePath(identity.Id, identity.Version)), "The .nuspec should exist.");
                    Assert.False(File.Exists(Path.Combine(resolver.GetInstallPath(identity.Id, identity.Version), "lib", "net45", "a.dll")), "The asset should exist.");
                }
            }
        }

        [PlatformFact(Platform.Windows)]
        public async Task InstallFromSourceAsyncByStream_InvalidSignPackageWithUnzip_ThrowsAsync()
        {
            // Arrange
            using (var root = TestDirectory.Create())
            {
                var resolver = new VersionFolderPathResolver(root);

                var nupkg = new SimpleTestPackageContext("A", "1.0.0");

                var signedPackageVerifier = new Mock<IPackageSignatureVerifier>(MockBehavior.Strict);

                signedPackageVerifier.Setup(x => x.VerifySignaturesAsync(
                    It.IsAny<ISignedPackageReader>(),
                    It.Is<SignedPackageVerifierSettings>(s => SigningTestUtility.AreVerifierSettingsEqual(s, _defaultContext.VerifierSettings)),
                    It.IsAny<CancellationToken>(),
                    It.IsAny<Guid>())).
                    ReturnsAsync(new VerifySignaturesResult(isValid: false, isSigned: true));

                FileInfo packageFileInfo = await SimpleTestPackageUtility.CreateFullPackageAsync(root, nupkg);

                var identity = new PackageIdentity("A", new NuGetVersion("1.0.0"));

                using (var fileStream = File.OpenRead(packageFileInfo.FullName))
                {
                    var packageInstallPath = resolver.GetInstallPath(identity.Id, identity.Version);
                    var packageInstallDirectory = Directory.GetParent(packageInstallPath);

                    var extractionContext = new PackageExtractionContext(
                         packageSaveMode: PackageSaveMode.Nuspec | PackageSaveMode.Files,
                         xmlDocFileSaveMode: XmlDocFileSaveMode.None,
                         clientPolicyContext: _defaultContext,
                         logger: NullLogger.Instance)
                    {
                        SignedPackageVerifier = signedPackageVerifier.Object
                    };

                    // Act & Assert
                    await Assert.ThrowsAsync<SignatureException>(
                     () => PackageExtractor.InstallFromSourceAsync(
                         root,
                         identity,
                        (stream) => fileStream.CopyToAsync(stream, 4096, CancellationToken.None),
                        resolver,
                        extractionContext,
                        CancellationToken.None));

                    // Assert that no footprint is left
                    Directory.Exists(packageInstallPath).Should().BeFalse();
                    Directory.Exists(packageInstallDirectory.FullName).Should().BeFalse();
                    File.Exists(Path.Combine(resolver.GetInstallPath(identity.Id, identity.Version), "lib", "net45", "a.dll"))
                        .Should()
                        .BeFalse();

                }
            }
        }

        [PlatformFact(Platform.Darwin)]
        public async Task InstallFromSourceAsyncByStream_InvalidSignPackageWithUnzip_SuccessAsync()
        {
            // Arrange
            using (var root = TestDirectory.Create())
            {
                var resolver = new VersionFolderPathResolver(root);

                var nupkg = new SimpleTestPackageContext("A", "1.0.0");

                var signedPackageVerifier = new Mock<IPackageSignatureVerifier>(MockBehavior.Strict);

                signedPackageVerifier.Setup(x => x.VerifySignaturesAsync(
                    It.IsAny<ISignedPackageReader>(),
                    It.Is<SignedPackageVerifierSettings>(s => SigningTestUtility.AreVerifierSettingsEqual(s, _defaultContext.VerifierSettings)),
                    It.IsAny<CancellationToken>(),
                    It.IsAny<Guid>())).
                    ReturnsAsync(new VerifySignaturesResult(isValid: false, isSigned: true));

                FileInfo packageFileInfo = await SimpleTestPackageUtility.CreateFullPackageAsync(root, nupkg);

                var identity = new PackageIdentity("A", new NuGetVersion("1.0.0"));

                using (var fileStream = File.OpenRead(packageFileInfo.FullName))
                {
                    var packageInstallPath = resolver.GetInstallPath(identity.Id, identity.Version);
                    var packageInstallDirectory = Directory.GetParent(packageInstallPath);

                    var extractionContext = new PackageExtractionContext(
                         packageSaveMode: PackageSaveMode.Nuspec | PackageSaveMode.Files,
                         xmlDocFileSaveMode: XmlDocFileSaveMode.None,
                         clientPolicyContext: _defaultContext,
                         logger: NullLogger.Instance)
                    {
                        SignedPackageVerifier = signedPackageVerifier.Object
                    };

                    // Act & Assert
                    await PackageExtractor.InstallFromSourceAsync(
                         root,
                         identity,
                        (stream) => fileStream.CopyToAsync(stream, 4096, CancellationToken.None),
                        resolver,
                        extractionContext,
                        CancellationToken.None);

                    // Assert
                    Directory.Exists(packageInstallPath).Should().BeTrue();
                    Directory.Exists(packageInstallDirectory.FullName).Should().BeTrue();
                    File.Exists(Path.Combine(resolver.GetInstallPath(identity.Id, identity.Version), "lib", "net45", "a.dll"))
                        .Should()
                        .BeTrue();
                }
            }
        }

        [Fact]
        public async Task ExtractPackageAsyncByStream_TrustedSignPackageAsync()
        {
            // Arrange
            using (var root = TestDirectory.Create())
            {
                var nupkg = new SimpleTestPackageContext("A", "1.0.0");

                var signedPackageVerifier = new Mock<IPackageSignatureVerifier>(MockBehavior.Strict);

                signedPackageVerifier.Setup(x => x.VerifySignaturesAsync(
                    It.IsAny<ISignedPackageReader>(),
                    It.Is<SignedPackageVerifierSettings>(s => SigningTestUtility.AreVerifierSettingsEqual(s, _defaultContext.VerifierSettings)),
                    It.IsAny<CancellationToken>(),
                    It.IsAny<Guid>())).
                    ReturnsAsync(new VerifySignaturesResult(isValid: true, isSigned: true));

                var resolver = new PackagePathResolver(root);
                var identity = new PackageIdentity("A", new NuGetVersion("1.0.0"));

                var packageFileInfo = await SimpleTestPackageUtility.CreateFullPackageAsync(root, nupkg);

                using (var packageStream = File.OpenRead(packageFileInfo.FullName))
                {
                    var extractionContext = new PackageExtractionContext(
                     packageSaveMode: PackageSaveMode.Nuspec | PackageSaveMode.Files,
                     xmlDocFileSaveMode: PackageExtractionBehavior.XmlDocFileSaveMode,
                     clientPolicyContext: _defaultContext,
                     logger: NullLogger.Instance)
                    {
                        SignedPackageVerifier = signedPackageVerifier.Object
                    };

                    // Act
                    var packageFiles = await PackageExtractor.ExtractPackageAsync(root,
                                                                     packageStream,
                                                                     resolver,
                                                                     extractionContext,
                                                                     CancellationToken.None);

                    // Assert
                    var installPath = resolver.GetInstallPath(identity);
                    Assert.False(File.Exists(Path.Combine(installPath, resolver.GetPackageFileName(identity))));
                    Assert.True(File.Exists(Path.Combine(installPath, resolver.GetManifestFileName(identity))));
                    Assert.True(File.Exists(Path.Combine(installPath, "lib", "net45", "a.dll")));
                }
            }
        }

        [PlatformFact(Platform.Windows)]
        public async Task ExtractPackageAsyncByStream_InvalidSignPackageWithUnzip_ThrowsAsync()
        {
            // Arrange
            using (var root = TestDirectory.Create())
            {
                var nupkg = new SimpleTestPackageContext("A", "1.0.0");

                var signedPackageVerifier = new Mock<IPackageSignatureVerifier>(MockBehavior.Strict);

                signedPackageVerifier.Setup(x => x.VerifySignaturesAsync(
                    It.IsAny<ISignedPackageReader>(),
                    It.Is<SignedPackageVerifierSettings>(s => SigningTestUtility.AreVerifierSettingsEqual(s, _defaultContext.VerifierSettings)),
                    It.IsAny<CancellationToken>(),
                    It.IsAny<Guid>())).
                    ReturnsAsync(new VerifySignaturesResult(isValid: false, isSigned: true));

                var resolver = new PackagePathResolver(root);
                var identity = new PackageIdentity("A", new NuGetVersion("1.0.0"));

                var packageFileInfo = await SimpleTestPackageUtility.CreateFullPackageAsync(root, nupkg);

                using (var packageStream = File.OpenRead(packageFileInfo.FullName))
                {
                    var packageExtractionContext = new PackageExtractionContext(
                        PackageSaveMode.Nupkg,
                        PackageExtractionBehavior.XmlDocFileSaveMode,
                        _defaultContext,
                        NullLogger.Instance)
                    {
                        SignedPackageVerifier = signedPackageVerifier.Object
                    };

                    // Act & Assert
                    await Assert.ThrowsAsync<SignatureException>(
                        () => PackageExtractor.ExtractPackageAsync(
                            root,
                            packageStream,
                            resolver,
                            packageExtractionContext,
                            CancellationToken.None));
                }
            }
        }

        [PlatformFact(Platform.Darwin)]
        public async Task ExtractPackageAsyncByStream_InvalidSignPackageWithUnzip_SuccessAsync()
        {
            // Arrange
            using (var root = TestDirectory.Create())
            {
                var nupkg = new SimpleTestPackageContext("A", "1.0.0");

                var signedPackageVerifier = new Mock<IPackageSignatureVerifier>(MockBehavior.Strict);

                signedPackageVerifier.Setup(x => x.VerifySignaturesAsync(
                    It.IsAny<ISignedPackageReader>(),
                    It.Is<SignedPackageVerifierSettings>(s => SigningTestUtility.AreVerifierSettingsEqual(s, _defaultContext.VerifierSettings)),
                    It.IsAny<CancellationToken>(),
                    It.IsAny<Guid>())).
                    ReturnsAsync(new VerifySignaturesResult(isValid: false, isSigned: true));

                var resolver = new PackagePathResolver(root);
                var identity = new PackageIdentity("A", new NuGetVersion("1.0.0"));

                var packageFileInfo = await SimpleTestPackageUtility.CreateFullPackageAsync(root, nupkg);

                using (var packageStream = File.OpenRead(packageFileInfo.FullName))
                {
                    var packageInstallPath = resolver.GetInstallPath(identity);
                    var packageInstallDirectory = Directory.GetParent(packageInstallPath);

                    var packageExtractionContext = new PackageExtractionContext(
                        PackageSaveMode.Files,
                        PackageExtractionBehavior.XmlDocFileSaveMode,
                        _defaultContext,
                        NullLogger.Instance)
                    {
                        SignedPackageVerifier = signedPackageVerifier.Object
                    };

                    // Act
                    await PackageExtractor.ExtractPackageAsync(
                            root,
                            packageStream,
                            resolver,
                            packageExtractionContext,
                            CancellationToken.None);

                    // Assert
                    Directory.Exists(packageInstallPath).Should().BeTrue();
                    Directory.Exists(packageInstallDirectory.FullName).Should().BeTrue();
                    File.Exists(Path.Combine(packageInstallPath, "lib", "net45", "a.dll")).Should().BeTrue();
                }
            }
        }

        [CIOnlyFact]
        public async Task ExtractPackageAsyncByStream_InvalidSignPackageWithUnzip_WithOptInEnvVar_Throws()
        {
            // Arrange
            var environment = new Mock<IEnvironmentVariableReader>(MockBehavior.Strict);
            environment.Setup(s => s.GetEnvironmentVariable(SignatureVerificationEnvironmentVariable)).Returns("TRUE");

            using (var root = TestDirectory.Create())
            {
                var nupkg = new SimpleTestPackageContext("A", "1.0.0");

                var signedPackageVerifier = new Mock<IPackageSignatureVerifier>(MockBehavior.Strict);

                signedPackageVerifier.Setup(x => x.VerifySignaturesAsync(
                    It.IsAny<ISignedPackageReader>(),
                    It.Is<SignedPackageVerifierSettings>(s => SigningTestUtility.AreVerifierSettingsEqual(s, _defaultContext.VerifierSettings)),
                    It.IsAny<CancellationToken>(),
                    It.IsAny<Guid>())).
                    ReturnsAsync(new VerifySignaturesResult(isValid: false, isSigned: true));

                var resolver = new PackagePathResolver(root);
                var identity = new PackageIdentity("A", new NuGetVersion("1.0.0"));

                var packageFileInfo = await SimpleTestPackageUtility.CreateFullPackageAsync(root, nupkg);

                using (var packageStream = File.OpenRead(packageFileInfo.FullName))
                using (var packageReader = new PackageArchiveReader(packageStream, environmentVariableReader: environment.Object))
                {
                    var packageExtractionContext = new PackageExtractionContext(
                        PackageSaveMode.Nupkg,
                        PackageExtractionBehavior.XmlDocFileSaveMode,
                        _defaultContext,
                        NullLogger.Instance)
                    {
                        SignedPackageVerifier = signedPackageVerifier.Object
                    };

                    // Act & Assert
                    await Assert.ThrowsAsync<SignatureException>(
                        () => PackageExtractor.ExtractPackageAsync(
                            root,
                            packageReader,
                            packageStream,
                            resolver,
                            packageExtractionContext,
                            CancellationToken.None));
                }
            }
        }

        [Fact]
        public async Task ExtractPackageAsyncByPackageReader_TrustedSignPackageAsync()
        {
            // Arrange
            using (var root = TestDirectory.Create())
            {
                var nupkg = new SimpleTestPackageContext("A", "1.0.0");

                var signedPackageVerifier = new Mock<IPackageSignatureVerifier>(MockBehavior.Strict);

                signedPackageVerifier.Setup(x => x.VerifySignaturesAsync(
                    It.IsAny<ISignedPackageReader>(),
                    It.Is<SignedPackageVerifierSettings>(s => SigningTestUtility.AreVerifierSettingsEqual(s, _defaultContext.VerifierSettings)),
                    It.IsAny<CancellationToken>(),
                    It.IsAny<Guid>())).
                    ReturnsAsync(new VerifySignaturesResult(isValid: true, isSigned: true));

                var resolver = new PackagePathResolver(root);
                var identity = new PackageIdentity("A", new NuGetVersion("1.0.0"));

                var packageFileInfo = await SimpleTestPackageUtility.CreateFullPackageAsync(root, nupkg);

                using (var packageStream = File.OpenRead(packageFileInfo.FullName))
                using (var packageReader = new PackageArchiveReader(packageStream))
                {
                    var packageExtractionContext = new PackageExtractionContext(
                        PackageSaveMode.Nuspec | PackageSaveMode.Files,
                        PackageExtractionBehavior.XmlDocFileSaveMode,
                        _defaultContext,
                        NullLogger.Instance)
                    {
                        SignedPackageVerifier = signedPackageVerifier.Object
                    };

                    // Act
                    var packageFiles = await PackageExtractor.ExtractPackageAsync(root,
                                                                     packageReader,
                                                                     resolver,
                                                                     packageExtractionContext,
                                                                     CancellationToken.None);

                    // Assert
                    var installPath = resolver.GetInstallPath(identity);
                    Assert.False(File.Exists(Path.Combine(installPath, resolver.GetPackageFileName(identity))));
                    Assert.True(File.Exists(Path.Combine(installPath, resolver.GetManifestFileName(identity))));
                    Assert.True(File.Exists(Path.Combine(installPath, "lib", "net45", "a.dll")));
                }
            }
        }

        [Fact]
        public async Task ExtractPackageAsyncByPackageReader_NupkgSaveMode_TrustedSignPackageAsync()
        {
            // Arrange
            using (var root = TestDirectory.Create())
            {
                var nupkg = new SimpleTestPackageContext("A", "1.0.0");

                var signedPackageVerifier = new Mock<IPackageSignatureVerifier>(MockBehavior.Strict);

                signedPackageVerifier.Setup(x => x.VerifySignaturesAsync(
                    It.IsAny<ISignedPackageReader>(),
                    It.Is<SignedPackageVerifierSettings>(s => SigningTestUtility.AreVerifierSettingsEqual(s, _defaultContext.VerifierSettings)),
                    It.IsAny<CancellationToken>(),
                    It.IsAny<Guid>())).
                    ReturnsAsync(new VerifySignaturesResult(isValid: true, isSigned: true));

                var resolver = new PackagePathResolver(root);
                var identity = new PackageIdentity("A", new NuGetVersion("1.0.0"));

                var packageFileInfo = await SimpleTestPackageUtility.CreateFullPackageAsync(root, nupkg);

                using (var packageStream = File.OpenRead(packageFileInfo.FullName))
                using (var packageReader = new PackageArchiveReader(packageStream))
                {
                    var packageExtractionContext = new PackageExtractionContext(
                        PackageSaveMode.Nupkg | PackageSaveMode.Nuspec | PackageSaveMode.Files,
                        PackageExtractionBehavior.XmlDocFileSaveMode,
                        _defaultContext,
                        NullLogger.Instance)
                    {
                        SignedPackageVerifier = signedPackageVerifier.Object
                    };

                    // Act
                    var packageFiles = await PackageExtractor.ExtractPackageAsync(root,
                                                                     packageReader,
                                                                     resolver,
                                                                     packageExtractionContext,
                                                                     CancellationToken.None);

                    // Assert
                    var installPath = resolver.GetInstallPath(identity);
                    Assert.True(File.Exists(Path.Combine(installPath, resolver.GetPackageFileName(identity))));
                    Assert.True(File.Exists(Path.Combine(installPath, resolver.GetManifestFileName(identity))));
                    Assert.True(File.Exists(Path.Combine(installPath, "lib", "net45", "a.dll")));
                }
            }
        }

        [PlatformFact(Platform.Windows)]
        public async Task ExtractPackageAsyncByPackageReader_InvalidSignPackageWithUnzip_ThrowsAsync()
        {
            // Arrange
            using (var root = TestDirectory.Create())
            {
                var nupkg = new SimpleTestPackageContext("A", "1.0.0");
                var signedPackageVerifier = new Mock<IPackageSignatureVerifier>(MockBehavior.Strict);

                signedPackageVerifier.Setup(x => x.VerifySignaturesAsync(
                    It.IsAny<ISignedPackageReader>(),
                    It.Is<SignedPackageVerifierSettings>(s => SigningTestUtility.AreVerifierSettingsEqual(s, _defaultContext.VerifierSettings)),
                    It.IsAny<CancellationToken>(),
                    It.IsAny<Guid>())).
                    ReturnsAsync(new VerifySignaturesResult(isValid: false, isSigned: true));

                var resolver = new PackagePathResolver(root);
                var identity = new PackageIdentity("A", new NuGetVersion("1.0.0"));

                var packageFileInfo = await SimpleTestPackageUtility.CreateFullPackageAsync(root, nupkg);

                using (var packageStream = File.OpenRead(packageFileInfo.FullName))
                using (var packageReader = new PackageArchiveReader(packageStream))
                {
                    var packageExtractionContext = new PackageExtractionContext(
                        PackageSaveMode.Nupkg,
                        PackageExtractionBehavior.XmlDocFileSaveMode,
                        _defaultContext,
                        NullLogger.Instance)
                    {
                        SignedPackageVerifier = signedPackageVerifier.Object
                    };

                    // Act & Assert
                    await Assert.ThrowsAsync<SignatureException>(
                        () => PackageExtractor.ExtractPackageAsync(
                            root,
                            packageReader,
                            resolver,
                            packageExtractionContext,
                            CancellationToken.None));
                }
            }
        }

        [PlatformFact(Platform.Darwin)]
        public async Task ExtractPackageAsyncByPackageReader_InvalidSignPackageWithUnzip_SucceedAsync()
        {
            // Arrange
            using (var root = TestDirectory.Create())
            {
                var nupkg = new SimpleTestPackageContext("A", "1.0.0");
                var signedPackageVerifier = new Mock<IPackageSignatureVerifier>(MockBehavior.Strict);

                signedPackageVerifier.Setup(x => x.VerifySignaturesAsync(
                    It.IsAny<ISignedPackageReader>(),
                    It.Is<SignedPackageVerifierSettings>(s => SigningTestUtility.AreVerifierSettingsEqual(s, _defaultContext.VerifierSettings)),
                    It.IsAny<CancellationToken>(),
                    It.IsAny<Guid>())).
                    ReturnsAsync(new VerifySignaturesResult(isValid: false, isSigned: true));

                var resolver = new PackagePathResolver(root);
                var identity = new PackageIdentity("A", new NuGetVersion("1.0.0"));

                var packageFileInfo = await SimpleTestPackageUtility.CreateFullPackageAsync(root, nupkg);

                using (var packageStream = File.OpenRead(packageFileInfo.FullName))
                {
                    var packageExtractionContext = new PackageExtractionContext(
                        PackageSaveMode.Nupkg,
                        PackageExtractionBehavior.XmlDocFileSaveMode,
                        _defaultContext,
                        NullLogger.Instance)
                    {
                        SignedPackageVerifier = signedPackageVerifier.Object
                    };

                    // Act
                    IEnumerable<string> files = await PackageExtractor.ExtractPackageAsync(
                            root,
                            packageStream,
                            resolver,
                            packageExtractionContext,
                            CancellationToken.None);

                    // Assert
                    files.Should().NotBeNull();
                    Directory.Exists(Path.Combine(root.Path,
                        $"{nupkg.Identity.Id}.{nupkg.Identity.Version.ToNormalizedString()}")).Should().BeTrue();
                }
            }
        }

        [CIOnlyFact]
        public async Task ExtractPackageAsyncByPackageReader_InvalidSignPackageWithUnzip_WithEnvVar_Throws()
        {
            // Arrange
            var environment = new Mock<IEnvironmentVariableReader>(MockBehavior.Strict);
            environment.Setup(s => s.GetEnvironmentVariable(SignatureVerificationEnvironmentVariable)).Returns("TRUE");

            using (var root = TestDirectory.Create())
            {
                var nupkg = new SimpleTestPackageContext("A", "1.0.0");
                var signedPackageVerifier = new Mock<IPackageSignatureVerifier>(MockBehavior.Strict);

                signedPackageVerifier.Setup(x => x.VerifySignaturesAsync(
                    It.IsAny<ISignedPackageReader>(),
                    It.Is<SignedPackageVerifierSettings>(s => SigningTestUtility.AreVerifierSettingsEqual(s, _defaultContext.VerifierSettings)),
                    It.IsAny<CancellationToken>(),
                    It.IsAny<Guid>())).
                    ReturnsAsync(new VerifySignaturesResult(isValid: false, isSigned: true));

                var resolver = new PackagePathResolver(root);
                var identity = new PackageIdentity("A", new NuGetVersion("1.0.0"));

                var packageFileInfo = await SimpleTestPackageUtility.CreateFullPackageAsync(root, nupkg);

                using (var packageStream = File.OpenRead(packageFileInfo.FullName))
                using (var packageReader = new PackageArchiveReader(packageStream, environmentVariableReader: environment.Object))
                {
                    var packageExtractionContext = new PackageExtractionContext(
                        PackageSaveMode.Nupkg,
                        PackageExtractionBehavior.XmlDocFileSaveMode,
                        _defaultContext,
                        NullLogger.Instance)
                    {
                        SignedPackageVerifier = signedPackageVerifier.Object
                    };

                    // Act & Assert
                    await Assert.ThrowsAsync<SignatureException>(
                        () => PackageExtractor.ExtractPackageAsync(
                            root,
                            packageReader,
                            resolver,
                            packageExtractionContext,
                            CancellationToken.None));
                }
            }
        }

        [Fact]
        public async Task ExtractPackageAsyncByPackageReaderAndStream_TrustedSignPackageAsync()
        {
            // Arrange
            using (var root = TestDirectory.Create())
            {
                var nupkg = new SimpleTestPackageContext("A", "1.0.0");
                var signedPackageVerifier = new Mock<IPackageSignatureVerifier>(MockBehavior.Strict);

                signedPackageVerifier.Setup(x => x.VerifySignaturesAsync(
                    It.IsAny<ISignedPackageReader>(),
                    It.Is<SignedPackageVerifierSettings>(s => SigningTestUtility.AreVerifierSettingsEqual(s, _defaultContext.VerifierSettings)),
                    It.IsAny<CancellationToken>(),
                    It.IsAny<Guid>())).
                    ReturnsAsync(new VerifySignaturesResult(isValid: true, isSigned: true));

                var resolver = new PackagePathResolver(root);
                var identity = new PackageIdentity("A", new NuGetVersion("1.0.0"));

                var packageFileInfo = await SimpleTestPackageUtility.CreateFullPackageAsync(root, nupkg);

                using (var packageStream = File.OpenRead(packageFileInfo.FullName))
                using (var packageReader = new PackageArchiveReader(packageStream))
                {
                    var packageExtractionContext = new PackageExtractionContext(
                        PackageSaveMode.Nuspec | PackageSaveMode.Files,
                        PackageExtractionBehavior.XmlDocFileSaveMode,
                        _defaultContext,
                        NullLogger.Instance)
                    {
                        SignedPackageVerifier = signedPackageVerifier.Object
                    };

                    // Act
                    var packageFiles = await PackageExtractor.ExtractPackageAsync(root,
                                                                     packageReader,
                                                                     packageStream,
                                                                     resolver,
                                                                     packageExtractionContext,
                                                                     CancellationToken.None);

                    // Assert
                    var installPath = resolver.GetInstallPath(identity);
                    Assert.False(File.Exists(Path.Combine(installPath, resolver.GetPackageFileName(identity))));
                    Assert.True(File.Exists(Path.Combine(installPath, resolver.GetManifestFileName(identity))));
                    Assert.True(File.Exists(Path.Combine(installPath, "lib", "net45", "a.dll")));
                }
            }
        }

        [PlatformFact(Platform.Windows)]
        public async Task ExtractPackageAsyncByPackageReaderAndStream_InvalidSignPackageWithUnzip_ThrowsAsync()
        {
            // Arrange
            using (var root = TestDirectory.Create())
            {
                var nupkg = new SimpleTestPackageContext("A", "1.0.0");
                var signedPackageVerifier = new Mock<IPackageSignatureVerifier>(MockBehavior.Strict);

                signedPackageVerifier.Setup(x => x.VerifySignaturesAsync(
                    It.IsAny<ISignedPackageReader>(),
                    It.Is<SignedPackageVerifierSettings>(s => SigningTestUtility.AreVerifierSettingsEqual(s, _defaultContext.VerifierSettings)),
                    It.IsAny<CancellationToken>(),
                    It.IsAny<Guid>())).
                    ReturnsAsync(new VerifySignaturesResult(isValid: false, isSigned: true));

                var resolver = new PackagePathResolver(root);
                var identity = new PackageIdentity("A", new NuGetVersion("1.0.0"));

                var packageFileInfo = await SimpleTestPackageUtility.CreateFullPackageAsync(root, nupkg);

                using (var packageStream = File.OpenRead(packageFileInfo.FullName))
                using (var packageReader = new PackageArchiveReader(packageStream))
                {
                    var packageExtractionContext = new PackageExtractionContext(
                        PackageSaveMode.Nupkg,
                        PackageExtractionBehavior.XmlDocFileSaveMode,
                        _defaultContext,
                        NullLogger.Instance)
                    {
                        SignedPackageVerifier = signedPackageVerifier.Object
                    };

                    // Act & Assert
                    await Assert.ThrowsAsync<SignatureException>(
                        () => PackageExtractor.ExtractPackageAsync(
                            root,
                            packageReader,
                            packageStream,
                            resolver,
                            packageExtractionContext,
                            CancellationToken.None));
                }
            }
        }

        [PlatformFact(Platform.Darwin)]
        public async Task ExtractPackageAsyncByPackageReaderAndStream_InvalidSignPackageWithUnzip_SuccessAsync()
        {
            // Arrange
            using (var root = TestDirectory.Create())
            {
                var nupkg = new SimpleTestPackageContext("A", "1.0.0");
                var signedPackageVerifier = new Mock<IPackageSignatureVerifier>(MockBehavior.Strict);

                signedPackageVerifier.Setup(x => x.VerifySignaturesAsync(
                    It.IsAny<ISignedPackageReader>(),
                    It.Is<SignedPackageVerifierSettings>(s => SigningTestUtility.AreVerifierSettingsEqual(s, _defaultContext.VerifierSettings)),
                    It.IsAny<CancellationToken>(),
                    It.IsAny<Guid>())).
                    ReturnsAsync(new VerifySignaturesResult(isValid: false, isSigned: true));

                var resolver = new PackagePathResolver(root);
                var identity = new PackageIdentity("A", new NuGetVersion("1.0.0"));

                var packageFileInfo = await SimpleTestPackageUtility.CreateFullPackageAsync(root, nupkg);

                using (var packageStream = File.OpenRead(packageFileInfo.FullName))
                using (var packageReader = new PackageArchiveReader(packageStream))
                {
                    var packageExtractionContext = new PackageExtractionContext(
                        PackageSaveMode.Nupkg,
                        PackageExtractionBehavior.XmlDocFileSaveMode,
                        _defaultContext,
                        NullLogger.Instance)
                    {
                        SignedPackageVerifier = signedPackageVerifier.Object
                    };

                    // Act
                    IEnumerable<string> files = await PackageExtractor.ExtractPackageAsync(
                                                root,
                                                packageReader,
                                                packageStream,
                                                resolver,
                                                packageExtractionContext,
                                                CancellationToken.None);

                    // Assert
                    files.Should().NotBeNull();
                    Directory.Exists(Path.Combine(root.Path,
                        $"{nupkg.Identity.Id}.{nupkg.Identity.Version.ToNormalizedString()}"))
                        .Should()
                        .BeTrue();
                }
            }
        }

        [CIOnlyFact]
        public async Task ExtractPackageAsyncByPackageReaderAndStream_InvalidSignPackageWithUnzip_WithEnvVar_Throws()
        {
            // Arrange
            var environment = new Mock<IEnvironmentVariableReader>(MockBehavior.Strict);
            environment.Setup(s => s.GetEnvironmentVariable(SignatureVerificationEnvironmentVariable)).Returns("TRUE");

            using (var root = TestDirectory.Create())
            {
                var nupkg = new SimpleTestPackageContext("A", "1.0.0");
                var signedPackageVerifier = new Mock<IPackageSignatureVerifier>(MockBehavior.Strict);

                signedPackageVerifier.Setup(x => x.VerifySignaturesAsync(
                    It.IsAny<ISignedPackageReader>(),
                    It.Is<SignedPackageVerifierSettings>(s => SigningTestUtility.AreVerifierSettingsEqual(s, _defaultContext.VerifierSettings)),
                    It.IsAny<CancellationToken>(),
                    It.IsAny<Guid>())).
                    ReturnsAsync(new VerifySignaturesResult(isValid: false, isSigned: true));

                var resolver = new PackagePathResolver(root);
                var identity = new PackageIdentity("A", new NuGetVersion("1.0.0"));

                var packageFileInfo = await SimpleTestPackageUtility.CreateFullPackageAsync(root, nupkg);

                using (var packageStream = File.OpenRead(packageFileInfo.FullName))
                using (var packageReader = new PackageArchiveReader(packageStream, environmentVariableReader: environment.Object))
                {
                    var packageExtractionContext = new PackageExtractionContext(
                        PackageSaveMode.Nupkg,
                        PackageExtractionBehavior.XmlDocFileSaveMode,
                        _defaultContext,
                        NullLogger.Instance)
                    {
                        SignedPackageVerifier = signedPackageVerifier.Object
                    };

                    // Act & Assert
                    await Assert.ThrowsAsync<SignatureException>(
                        () => PackageExtractor.ExtractPackageAsync(
                            root,
                            packageReader,
                            packageStream,
                            resolver,
                            packageExtractionContext,
                            CancellationToken.None));
                }
            }
        }

        [PlatformFact(Platform.Windows)]
        public async Task VerifyPackageSignatureAsync_PassesCommonSettingsWhenNoRepoSignatureInfo_DoVerifyAsync()
        {
            // Arrange
            using (var root = TestDirectory.Create())
            {
                var nupkg = new SimpleTestPackageContext("A", "1.0.0");
                var signedPackageVerifier = new Mock<IPackageSignatureVerifier>();
                var resolver = new PackagePathResolver(root);
                var identity = new PackageIdentity("A", new NuGetVersion("1.0.0"));
                var packageFileInfo = await SimpleTestPackageUtility.CreateFullPackageAsync(root, nupkg);

                signedPackageVerifier.Setup(x => x.VerifySignaturesAsync(
                    It.IsAny<ISignedPackageReader>(),
                    It.Is<SignedPackageVerifierSettings>(s => SigningTestUtility.AreVerifierSettingsEqual(s, _defaultContext.VerifierSettings)),
                    It.IsAny<CancellationToken>(),
                    It.IsAny<Guid>())).
                    ReturnsAsync(new VerifySignaturesResult(isValid: true, isSigned: true));

                using (var packageStream = File.OpenRead(packageFileInfo.FullName))
                using (var packageReader = new PackageArchiveReader(packageStream))
                {
                    var packageExtractionContext = new PackageExtractionContext(
                        PackageSaveMode.Nuspec | PackageSaveMode.Files,
                        PackageExtractionBehavior.XmlDocFileSaveMode,
                        _defaultContext,
                        NullLogger.Instance)
                    {
                        SignedPackageVerifier = signedPackageVerifier.Object
                    };

                    // Act
                    await PackageExtractor.ExtractPackageAsync(
                        root,
                        packageReader,
                        packageStream,
                        resolver,
                        packageExtractionContext,
                        CancellationToken.None);

                    // Assert
                    signedPackageVerifier.Verify(mock => mock.VerifySignaturesAsync(
                        It.Is<ISignedPackageReader>(p => p.Equals(packageReader)),
                        It.Is<SignedPackageVerifierSettings>(s => SigningTestUtility.AreVerifierSettingsEqual(s, _defaultContext.VerifierSettings)),
                        It.Is<CancellationToken>(t => t.Equals(CancellationToken.None)),
                        It.IsAny<Guid>()));
                }
            }
        }

        [PlatformFact(Platform.Darwin)]
        public async Task VerifyPackageSignatureAsync_PassesCommonSettingsWhenNoRepoSignatureInfo_DonotVerifyAsync()
        {
            // Arrange
            using (var root = TestDirectory.Create())
            {
                var nupkg = new SimpleTestPackageContext("A", "1.0.0");
                var signedPackageVerifier = new Mock<IPackageSignatureVerifier>();
                var resolver = new PackagePathResolver(root);
                var identity = new PackageIdentity("A", new NuGetVersion("1.0.0"));
                var packageFileInfo = await SimpleTestPackageUtility.CreateFullPackageAsync(root, nupkg);

                signedPackageVerifier.Setup(x => x.VerifySignaturesAsync(
                    It.IsAny<ISignedPackageReader>(),
                    It.Is<SignedPackageVerifierSettings>(s => SigningTestUtility.AreVerifierSettingsEqual(s, _defaultContext.VerifierSettings)),
                    It.IsAny<CancellationToken>(),
                    It.IsAny<Guid>())).
                    ReturnsAsync(new VerifySignaturesResult(isValid: true, isSigned: true));

                using (var packageStream = File.OpenRead(packageFileInfo.FullName))
                using (var packageReader = new PackageArchiveReader(packageStream))
                {
                    var packageExtractionContext = new PackageExtractionContext(
                        PackageSaveMode.Nuspec | PackageSaveMode.Files,
                        PackageExtractionBehavior.XmlDocFileSaveMode,
                        _defaultContext,
                        NullLogger.Instance)
                    {
                        SignedPackageVerifier = signedPackageVerifier.Object
                    };

                    // Act
                    await PackageExtractor.ExtractPackageAsync(
                        root,
                        packageReader,
                        packageStream,
                        resolver,
                        packageExtractionContext,
                        CancellationToken.None);

                    // Assert
                    signedPackageVerifier.Verify(mock => mock.VerifySignaturesAsync(
                        It.Is<ISignedPackageReader>(p => p.Equals(packageReader)),
                        It.Is<SignedPackageVerifierSettings>(s => SigningTestUtility.AreVerifierSettingsEqual(s, _defaultContext.VerifierSettings)),
                        It.Is<CancellationToken>(t => t.Equals(CancellationToken.None)),
                        It.IsAny<Guid>()), Times.Never);
                }
            }
        }

        [CIOnlyFact]
        public async Task VerifyPackageSignatureAsync_PassesCommonSettingsWhenNoRepoSignatureInfo_WithEnvVar_DoVerify()
        {
            // Arrange
            var environment = new Mock<IEnvironmentVariableReader>(MockBehavior.Strict);
            environment.Setup(s => s.GetEnvironmentVariable(SignatureVerificationEnvironmentVariable)).Returns("TRUE");

            using (var root = TestDirectory.Create())
            {
                var nupkg = new SimpleTestPackageContext("A", "1.0.0");
                var signedPackageVerifier = new Mock<IPackageSignatureVerifier>();
                var resolver = new PackagePathResolver(root);
                var identity = new PackageIdentity("A", new NuGetVersion("1.0.0"));
                var packageFileInfo = await SimpleTestPackageUtility.CreateFullPackageAsync(root, nupkg);

                signedPackageVerifier.Setup(x => x.VerifySignaturesAsync(
                    It.IsAny<ISignedPackageReader>(),
                    It.Is<SignedPackageVerifierSettings>(s => SigningTestUtility.AreVerifierSettingsEqual(s, _defaultContext.VerifierSettings)),
                    It.IsAny<CancellationToken>(),
                    It.IsAny<Guid>())).
                    ReturnsAsync(new VerifySignaturesResult(isValid: true, isSigned: true));

                using (var packageStream = File.OpenRead(packageFileInfo.FullName))
                using (var packageReader = new PackageArchiveReader(packageStream, environmentVariableReader: environment.Object))
                {
                    var packageExtractionContext = new PackageExtractionContext(
                        PackageSaveMode.Nuspec | PackageSaveMode.Files,
                        PackageExtractionBehavior.XmlDocFileSaveMode,
                        _defaultContext,
                        NullLogger.Instance)
                    {
                        SignedPackageVerifier = signedPackageVerifier.Object
                    };

                    // Act
                    await PackageExtractor.ExtractPackageAsync(
                        root,
                        packageReader,
                        packageStream,
                        resolver,
                        packageExtractionContext,
                        CancellationToken.None);

                    // Assert
                    signedPackageVerifier.Verify(mock => mock.VerifySignaturesAsync(
                        It.Is<ISignedPackageReader>(p => p.Equals(packageReader)),
                        It.Is<SignedPackageVerifierSettings>(s => SigningTestUtility.AreVerifierSettingsEqual(s, _defaultContext.VerifierSettings)),
                        It.Is<CancellationToken>(t => t.Equals(CancellationToken.None)),
                        It.IsAny<Guid>()));
                }
            }
        }

        [PlatformTheory(Platform.Windows)]
        [MemberData(nameof(KnownClientPoliciesList))]
        public async Task VerifyPackageSignatureAsync_PassesModifiedSettingsWhenRepoSignatureInfo_DefaultSettings_DoVerifyAsync(ClientPolicyContext clientPolicyContext)
        {
            // Arrange
            using (var root = TestDirectory.Create())
            {
                var nupkg = new SimpleTestPackageContext("A", "1.0.0");
                var signedPackageVerifier = new Mock<IPackageSignatureVerifier>();
                var resolver = new PackagePathResolver(root);
                var identity = new PackageIdentity("A", new NuGetVersion("1.0.0"));
                var packageFileInfo = await SimpleTestPackageUtility.CreateFullPackageAsync(root, nupkg);

                signedPackageVerifier.Setup(x => x.VerifySignaturesAsync(
                    It.IsAny<ISignedPackageReader>(),
                    It.IsAny<SignedPackageVerifierSettings>(),
                    It.IsAny<CancellationToken>(),
                    It.IsAny<Guid>())).
                    ReturnsAsync(new VerifySignaturesResult(isValid: true, isSigned: true));

                var repositorySignatureInfoAndAllowList = CreateTestRepositorySignatureInfoAndExpectedAllowList();
                var repositorySignatureInfo = repositorySignatureInfoAndAllowList.Item1;
                var expectedAllowList = repositorySignatureInfoAndAllowList.Item2;
                var repositorySignatureInfoProvider = RepositorySignatureInfoProvider.Instance;
                var signedPackageVerifierSettings = clientPolicyContext.VerifierSettings;

                repositorySignatureInfoProvider.AddOrUpdateRepositorySignatureInfo(root, repositorySignatureInfo);

                var expectedVerifierSettings = new SignedPackageVerifierSettings(
                    allowUnsigned: false,
                    allowIllegal: signedPackageVerifierSettings.AllowIllegal,
                    allowUntrusted: false,
                    allowIgnoreTimestamp: signedPackageVerifierSettings.AllowIgnoreTimestamp,
                    allowMultipleTimestamps: signedPackageVerifierSettings.AllowMultipleTimestamps,
                    allowNoTimestamp: signedPackageVerifierSettings.AllowNoTimestamp,
                    allowUnknownRevocation: signedPackageVerifierSettings.AllowUnknownRevocation,
                    reportUnknownRevocation: signedPackageVerifierSettings.ReportUnknownRevocation,
                    verificationTarget: signedPackageVerifierSettings.VerificationTarget,
                    signaturePlacement: signedPackageVerifierSettings.SignaturePlacement,
                    repositoryCountersignatureVerificationBehavior: signedPackageVerifierSettings.RepositoryCountersignatureVerificationBehavior,
                    revocationMode: signedPackageVerifierSettings.RevocationMode);

                using (var packageStream = File.OpenRead(packageFileInfo.FullName))
                using (var packageReader = new PackageArchiveReader(packageStream))
                {
                    var packageExtractionContext = new PackageExtractionContext(
                        PackageSaveMode.Nuspec | PackageSaveMode.Files,
                        PackageExtractionBehavior.XmlDocFileSaveMode,
                        clientPolicyContext,
                        NullLogger.Instance)
                    {
                        SignedPackageVerifier = signedPackageVerifier.Object
                    };

                    // Act
                    await PackageExtractor.ExtractPackageAsync(
                        root,
                        packageReader,
                        packageStream,
                        resolver,
                        packageExtractionContext,
                        CancellationToken.None);

                    // Assert
                    signedPackageVerifier.Verify(mock => mock.VerifySignaturesAsync(
                        It.Is<ISignedPackageReader>(p => p.Equals(packageReader)),
                        It.Is<SignedPackageVerifierSettings>(s => SigningTestUtility.AreVerifierSettingsEqual(s, expectedVerifierSettings)),
                        It.Is<CancellationToken>(t => t.Equals(CancellationToken.None)),
                        It.IsAny<Guid>()), Times.AtLeastOnce);
                }
            }
        }

        [PlatformTheory(Platform.Darwin)]
        [MemberData(nameof(KnownClientPoliciesList))]
        public async Task VerifyPackageSignatureAsync_PassesModifiedSettingsWhenRepoSignatureInfo_DefaultSettings_DonotVerifyAsync(ClientPolicyContext clientPolicyContext)
        {
            // Arrange
            using (var root = TestDirectory.Create())
            {
                var nupkg = new SimpleTestPackageContext("A", "1.0.0");
                var signedPackageVerifier = new Mock<IPackageSignatureVerifier>();
                var resolver = new PackagePathResolver(root);
                var identity = new PackageIdentity("A", new NuGetVersion("1.0.0"));
                var packageFileInfo = await SimpleTestPackageUtility.CreateFullPackageAsync(root, nupkg);

                signedPackageVerifier.Setup(x => x.VerifySignaturesAsync(
                    It.IsAny<ISignedPackageReader>(),
                    It.IsAny<SignedPackageVerifierSettings>(),
                    It.IsAny<CancellationToken>(),
                    It.IsAny<Guid>())).
                    ReturnsAsync(new VerifySignaturesResult(isValid: true, isSigned: true));

                Tuple<RepositorySignatureInfo, List<CertificateHashAllowListEntry>> repositorySignatureInfoAndAllowList = CreateTestRepositorySignatureInfoAndExpectedAllowList();
                RepositorySignatureInfo repositorySignatureInfo = repositorySignatureInfoAndAllowList.Item1;
                List<CertificateHashAllowListEntry> expectedAllowList = repositorySignatureInfoAndAllowList.Item2;
                var repositorySignatureInfoProvider = RepositorySignatureInfoProvider.Instance;
                SignedPackageVerifierSettings signedPackageVerifierSettings = clientPolicyContext.VerifierSettings;

                repositorySignatureInfoProvider.AddOrUpdateRepositorySignatureInfo(root, repositorySignatureInfo);

                var expectedVerifierSettings = new SignedPackageVerifierSettings(
                    allowUnsigned: false,
                    allowIllegal: signedPackageVerifierSettings.AllowIllegal,
                    allowUntrusted: false,
                    allowIgnoreTimestamp: signedPackageVerifierSettings.AllowIgnoreTimestamp,
                    allowMultipleTimestamps: signedPackageVerifierSettings.AllowMultipleTimestamps,
                    allowNoTimestamp: signedPackageVerifierSettings.AllowNoTimestamp,
                    allowUnknownRevocation: signedPackageVerifierSettings.AllowUnknownRevocation,
                    reportUnknownRevocation: signedPackageVerifierSettings.ReportUnknownRevocation,
                    verificationTarget: signedPackageVerifierSettings.VerificationTarget,
                    signaturePlacement: signedPackageVerifierSettings.SignaturePlacement,
                    repositoryCountersignatureVerificationBehavior: signedPackageVerifierSettings.RepositoryCountersignatureVerificationBehavior,
                    revocationMode: signedPackageVerifierSettings.RevocationMode);

                using (var packageStream = File.OpenRead(packageFileInfo.FullName))
                using (var packageReader = new PackageArchiveReader(packageStream))
                {
                    var packageExtractionContext = new PackageExtractionContext(
                        PackageSaveMode.Nuspec | PackageSaveMode.Files,
                        PackageExtractionBehavior.XmlDocFileSaveMode,
                        clientPolicyContext,
                        NullLogger.Instance)
                    {
                        SignedPackageVerifier = signedPackageVerifier.Object
                    };

                    // Act
                    await PackageExtractor.ExtractPackageAsync(
                        root,
                        packageReader,
                        packageStream,
                        resolver,
                        packageExtractionContext,
                        CancellationToken.None);

                    // Assert
                    signedPackageVerifier.Verify(mock => mock.VerifySignaturesAsync(
                        It.Is<ISignedPackageReader>(p => p.Equals(packageReader)),
                        It.Is<SignedPackageVerifierSettings>(s => SigningTestUtility.AreVerifierSettingsEqual(s, expectedVerifierSettings)),
                        It.Is<CancellationToken>(t => t.Equals(CancellationToken.None)),
                        It.IsAny<Guid>()), Times.Never);
                }
            }
        }

        [PlatformTheory(Platform.Windows)]
        [MemberData(nameof(KnownClientPoliciesList))]
        public async Task VerifyPackageSignatureAsync_PassesModifiedSettingsWhenRepoSignatureInfo_DefaultVerifyCommandSettings_DoVerifyAsync(ClientPolicyContext clientPolicyContext)
        {
            // Arrange
            using (var root = TestDirectory.Create())
            {
                var nupkg = new SimpleTestPackageContext("A", "1.0.0");
                var signedPackageVerifier = new Mock<IPackageSignatureVerifier>();
                var resolver = new PackagePathResolver(root);
                var identity = new PackageIdentity("A", new NuGetVersion("1.0.0"));
                var packageFileInfo = await SimpleTestPackageUtility.CreateFullPackageAsync(root, nupkg);

                signedPackageVerifier.Setup(x => x.VerifySignaturesAsync(
                    It.IsAny<ISignedPackageReader>(),
                    It.IsAny<SignedPackageVerifierSettings>(),
                    It.IsAny<CancellationToken>(),
                    It.IsAny<Guid>())).
                    ReturnsAsync(new VerifySignaturesResult(isValid: true, isSigned: true));

                var repositorySignatureInfoAndAllowList = CreateTestRepositorySignatureInfoAndExpectedAllowList();
                var repositorySignatureInfo = repositorySignatureInfoAndAllowList.Item1;
                var expectedAllowList = repositorySignatureInfoAndAllowList.Item2;
                var repositorySignatureInfoProvider = RepositorySignatureInfoProvider.Instance;
                var signedPackageVerifierSettings = clientPolicyContext.VerifierSettings;
                repositorySignatureInfoProvider.AddOrUpdateRepositorySignatureInfo(root, repositorySignatureInfo);

                var expectedVerifierSettings = new SignedPackageVerifierSettings(
                    allowUnsigned: false,
                    allowIllegal: signedPackageVerifierSettings.AllowIllegal,
                    allowUntrusted: false,
                    allowIgnoreTimestamp: signedPackageVerifierSettings.AllowIgnoreTimestamp,
                    allowMultipleTimestamps: signedPackageVerifierSettings.AllowMultipleTimestamps,
                    allowNoTimestamp: signedPackageVerifierSettings.AllowNoTimestamp,
                    allowUnknownRevocation: signedPackageVerifierSettings.AllowUnknownRevocation,
                    reportUnknownRevocation: signedPackageVerifierSettings.ReportUnknownRevocation,
                    verificationTarget: signedPackageVerifierSettings.VerificationTarget,
                    signaturePlacement: signedPackageVerifierSettings.SignaturePlacement,
                    repositoryCountersignatureVerificationBehavior: signedPackageVerifierSettings.RepositoryCountersignatureVerificationBehavior,
                    revocationMode: signedPackageVerifierSettings.RevocationMode);

                using (var packageStream = File.OpenRead(packageFileInfo.FullName))
                using (var packageReader = new PackageArchiveReader(packageStream))
                {
                    var packageExtractionContext = new PackageExtractionContext(
                        PackageSaveMode.Nuspec | PackageSaveMode.Files,
                        PackageExtractionBehavior.XmlDocFileSaveMode,
                        clientPolicyContext,
                        NullLogger.Instance)
                    {
                        SignedPackageVerifier = signedPackageVerifier.Object
                    };

                    // Act
                    await PackageExtractor.ExtractPackageAsync(
                        root,
                        packageReader,
                        packageStream,
                        resolver,
                        packageExtractionContext,
                        CancellationToken.None);

                    // Assert
                    signedPackageVerifier.Verify(mock => mock.VerifySignaturesAsync(
                        It.Is<ISignedPackageReader>(p => p.Equals(packageReader)),
                        It.Is<SignedPackageVerifierSettings>(s => SigningTestUtility.AreVerifierSettingsEqual(s, expectedVerifierSettings)),
                        It.Is<CancellationToken>(t => t.Equals(CancellationToken.None)),
                        It.IsAny<Guid>()), Times.AtLeastOnce);
                }
            }
        }

        [PlatformTheory(Platform.Darwin)]
        [MemberData(nameof(KnownClientPoliciesList))]
        public async Task VerifyPackageSignatureAsync_PassesModifiedSettingsWhenRepoSignatureInfo_DefaultVerifyCommandSettings_DonotVerifyAsync(ClientPolicyContext clientPolicyContext)
        {
            // Arrange
            using (var root = TestDirectory.Create())
            {
                var nupkg = new SimpleTestPackageContext("A", "1.0.0");
                var signedPackageVerifier = new Mock<IPackageSignatureVerifier>();
                var resolver = new PackagePathResolver(root);
                var identity = new PackageIdentity("A", new NuGetVersion("1.0.0"));
                var packageFileInfo = await SimpleTestPackageUtility.CreateFullPackageAsync(root, nupkg);

                signedPackageVerifier.Setup(x => x.VerifySignaturesAsync(
                    It.IsAny<ISignedPackageReader>(),
                    It.IsAny<SignedPackageVerifierSettings>(),
                    It.IsAny<CancellationToken>(),
                    It.IsAny<Guid>())).
                    ReturnsAsync(new VerifySignaturesResult(isValid: true, isSigned: true));

                Tuple<RepositorySignatureInfo, List<CertificateHashAllowListEntry>> repositorySignatureInfoAndAllowList = CreateTestRepositorySignatureInfoAndExpectedAllowList();
                RepositorySignatureInfo repositorySignatureInfo = repositorySignatureInfoAndAllowList.Item1;
                List<CertificateHashAllowListEntry> expectedAllowList = repositorySignatureInfoAndAllowList.Item2;
                var repositorySignatureInfoProvider = RepositorySignatureInfoProvider.Instance;
                SignedPackageVerifierSettings signedPackageVerifierSettings = clientPolicyContext.VerifierSettings;
                repositorySignatureInfoProvider.AddOrUpdateRepositorySignatureInfo(root, repositorySignatureInfo);

                var expectedVerifierSettings = new SignedPackageVerifierSettings(
                    allowUnsigned: false,
                    allowIllegal: signedPackageVerifierSettings.AllowIllegal,
                    allowUntrusted: false,
                    allowIgnoreTimestamp: signedPackageVerifierSettings.AllowIgnoreTimestamp,
                    allowMultipleTimestamps: signedPackageVerifierSettings.AllowMultipleTimestamps,
                    allowNoTimestamp: signedPackageVerifierSettings.AllowNoTimestamp,
                    allowUnknownRevocation: signedPackageVerifierSettings.AllowUnknownRevocation,
                    reportUnknownRevocation: signedPackageVerifierSettings.ReportUnknownRevocation,
                    verificationTarget: signedPackageVerifierSettings.VerificationTarget,
                    signaturePlacement: signedPackageVerifierSettings.SignaturePlacement,
                    repositoryCountersignatureVerificationBehavior: signedPackageVerifierSettings.RepositoryCountersignatureVerificationBehavior,
                    revocationMode: signedPackageVerifierSettings.RevocationMode);

                using (var packageStream = File.OpenRead(packageFileInfo.FullName))
                using (var packageReader = new PackageArchiveReader(packageStream))
                {
                    var packageExtractionContext = new PackageExtractionContext(
                        PackageSaveMode.Nuspec | PackageSaveMode.Files,
                        PackageExtractionBehavior.XmlDocFileSaveMode,
                        clientPolicyContext,
                        NullLogger.Instance)
                    {
                        SignedPackageVerifier = signedPackageVerifier.Object
                    };

                    // Act
                    await PackageExtractor.ExtractPackageAsync(
                        root,
                        packageReader,
                        packageStream,
                        resolver,
                        packageExtractionContext,
                        CancellationToken.None);

                    // Assert
                    signedPackageVerifier.Verify(mock => mock.VerifySignaturesAsync(
                        It.Is<ISignedPackageReader>(p => p.Equals(packageReader)),
                        It.Is<SignedPackageVerifierSettings>(s => SigningTestUtility.AreVerifierSettingsEqual(s, expectedVerifierSettings)),
                        It.Is<CancellationToken>(t => t.Equals(CancellationToken.None)),
                        It.IsAny<Guid>()), Times.Never);
                }
            }
        }
#endif

        private string PermissionWithUMaskApplied(string permission)
        {
            var permissionBits = Convert.ToInt32(permission, 8);
            string umask;

            var startInfo = new ProcessStartInfo
            {
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardInput = true,
                UseShellExecute = false,
                FileName = "sh",
                Arguments = "-c umask"
            };

            using (var process = new Process())
            {
                process.StartInfo = startInfo;

                process.Start();
                umask = process.StandardOutput.ReadLine();

                process.WaitForExit();
            }

            var umaskBits = Convert.ToInt32(umask, 8);
            permissionBits = permissionBits & ~umaskBits;

            return Convert.ToString(permissionBits, 8);
        }

        private string StatPermissions(string path)
        {
            string permissions;

            var startInfo = new ProcessStartInfo
            {
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardInput = true,
                UseShellExecute = false,
                FileName = "stat"
            };
            if (RuntimeEnvironmentHelper.IsLinux)
            {
                startInfo.Arguments = "-c %a " + path;
            }
            else
            {
                startInfo.Arguments = "-f %A " + path;
            }

            using (var process = new Process())
            {
                process.StartInfo = startInfo;

                process.Start();
                permissions = process.StandardOutput.ReadLine();

                process.WaitForExit();
            }

            return permissions;
        }

        [Fact]
        public async Task CopySatelliteFilesAsync_ThrowsForNullPackageIdentityAsync()
        {
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                () => PackageExtractor.CopySatelliteFilesAsync(
                    packageIdentity: null,
                    packagePathResolver: new PackagePathResolver(rootDirectory: Path.GetFullPath("a")),
                    packageSaveMode: PackageSaveMode.Defaultv3,
                    packageExtractionContext: new PackageExtractionContext(
                        PackageSaveMode.Defaultv2,
                        PackageExtractionBehavior.XmlDocFileSaveMode,
                        clientPolicyContext: null,
                        logger: NullLogger.Instance),
                    token: CancellationToken.None));

            Assert.Equal("packageIdentity", exception.ParamName);
        }

        [Fact]
        public async Task CopySatelliteFilesAsync_ThrowsForNullPackagePathResolverAsync()
        {
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                () => PackageExtractor.CopySatelliteFilesAsync(
                    new PackageIdentity("a", new NuGetVersion(major: 1, minor: 2, patch: 3)),
                    packagePathResolver: null,
                    packageSaveMode: PackageSaveMode.Defaultv3,
                    packageExtractionContext: new PackageExtractionContext(
                        PackageSaveMode.Defaultv2,
                        PackageExtractionBehavior.XmlDocFileSaveMode,
                        clientPolicyContext: null,
                        logger: NullLogger.Instance),
                    token: CancellationToken.None));

            Assert.Equal("packagePathResolver", exception.ParamName);
        }

        [Fact]
        public async Task CopySatelliteFilesAsync_ThrowsForNullPackageExtractionContextAsync()
        {
            using (var testDirectory = TestDirectory.Create())
            {
                var packageIdentity = new PackageIdentity("A", new NuGetVersion("1.2.3"));
                var packagePathResolver = new PackagePathResolver(testDirectory.Path);
                var satelliteIdentity = new PackageIdentity(packageIdentity.Id + ".fr", packageIdentity.Version);
                var satellitePackageInfo = await TestPackagesCore.GetSatellitePackageAsync(
                    testDirectory.Path,
                    packageIdentity.Id,
                    packageIdentity.Version.ToNormalizedString(),
                    language: "fr");

                var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                    () => PackageExtractor.CopySatelliteFilesAsync(
                        satelliteIdentity,
                        packagePathResolver,
                        PackageSaveMode.Defaultv3,
                        packageExtractionContext: null,
                        token: CancellationToken.None));

                Assert.Equal("packageExtractionContext", exception.ParamName);
            }
        }

        [Fact]
        public async Task CopySatelliteFilesAsync_ReturnsEmptyEnumerableIfSatelliteFileDoesNotExistAsync()
        {
            using (var testDirectory = TestDirectory.Create())
            {
                var packageIdentity = new PackageIdentity("A", new NuGetVersion("1.2.3"));
                var packagePathResolver = new PackagePathResolver(testDirectory.Path);
                var satelliteIdentity = new PackageIdentity(packageIdentity.Id + ".fr", packageIdentity.Version);

                var files = await PackageExtractor.CopySatelliteFilesAsync(
                    satelliteIdentity,
                    packagePathResolver,
                    PackageSaveMode.Defaultv3,
                    new PackageExtractionContext(
                        PackageSaveMode.Defaultv2,
                        PackageExtractionBehavior.XmlDocFileSaveMode,
                        clientPolicyContext: null,
                        logger: NullLogger.Instance),
                    CancellationToken.None);

                Assert.Empty(files);
            }
        }

        [Fact]
        public async Task CopySatelliteFilesAsync_ReturnsDestinationFilePathAsync()
        {
            using (var testDirectory = TestDirectory.Create())
            {
                var packageIdentity = new PackageIdentity("A", new NuGetVersion("1.2.3"));
                var packagePathResolver = new PackagePathResolver(testDirectory.Path);
                var packageFileInfo = await TestPackagesCore.GetRuntimePackageAsync(
                    testDirectory.Path,
                    packageIdentity.Id,
                    packageIdentity.Version.ToNormalizedString());
                var satelliteIdentity = new PackageIdentity(packageIdentity.Id + ".fr", packageIdentity.Version);
                var satellitePackageInfo = await TestPackagesCore.GetSatellitePackageAsync(
                    testDirectory.Path,
                    packageIdentity.Id,
                    packageIdentity.Version.ToNormalizedString(),
                    language: "fr");

                var files = (await PackageExtractor.CopySatelliteFilesAsync(
                    satelliteIdentity,
                    packagePathResolver,
                    PackageSaveMode.Defaultv3,
                    new PackageExtractionContext(
                        PackageSaveMode.Defaultv2,
                        PackageExtractionBehavior.XmlDocFileSaveMode,
                        clientPolicyContext: null,
                        logger: NullLogger.Instance),
                    CancellationToken.None)).ToArray();

                Assert.Equal(1, files.Length);
                Assert.Equal(Path.Combine(testDirectory.Path, "lib", "net45", "fr", "A.resources.dll"), files[0]);
            }
        }

        [Fact]
        public async Task InstallFromSourceAsync_PluginPackageDownloader()
        {
            // Arrange
            // Arrange
            using (var root = TestDirectory.Create())
            {
                var packagesPath = Path.Combine(root, "packages");
                var pathResolver = new VersionFolderPathResolver(packagesPath);
                var identity = new PackageIdentity("PackageA", new NuGetVersion("1.0.0"));
                var packageSourceRepository = "https://unit.test";

                var connection = new Mock<IConnection>();
                connection.Setup(x => x.SendRequestAndReceiveResponseAsync<CopyNupkgFileRequest, CopyNupkgFileResponse>(
                        It.Is<MessageMethod>(m => m == MessageMethod.CopyNupkgFile),
                        It.Is<CopyNupkgFileRequest>(c => c.PackageId == identity.Id &&
                            c.PackageVersion == identity.Version.ToNormalizedString() &&
                            c.PackageSourceRepository == packageSourceRepository &&
                            c.DestinationFilePath == pathResolver.GetPackageDirectory(identity.Id, identity.Version)),
                        It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new CopyNupkgFileResponse(MessageResponseCode.Success));

                var plugin = new Mock<IPlugin>();
                plugin.Setup(x => x.Dispose());
                plugin.SetupGet(x => x.Connection)
                    .Returns(connection.Object);

                var packageReader = new PluginPackageReader(plugin.Object, identity, packageSourceRepository);

                using (var pluginDownloader = new PluginPackageDownloader(
                    plugin.Object,
                    identity,
                    packageReader,
                    packageSourceRepository))
                {
                    // Act
                    var installed = await PackageExtractor.InstallFromSourceAsync(
                            identity,
                            pluginDownloader,
                            pathResolver,
                            new PackageExtractionContext(
                                packageSaveMode: PackageSaveMode.Nupkg,
                                xmlDocFileSaveMode: XmlDocFileSaveMode.None,
                                clientPolicyContext: null,
                                logger: NullLogger.Instance),
                            CancellationToken.None);

                    // Assert
                    Assert.True(installed);
                }
            }
        }

        [Fact]
        public async Task InstallFromSourceAsync_LogsSourceOnNormalLevel()
        {
            // Arrange
            using (var testDirectory = TestDirectory.Create())
            {
                var source = Path.Combine(testDirectory, "source");
                Directory.CreateDirectory(source);
                var resolver = new VersionFolderPathResolver(Path.Combine(testDirectory, "gpf"));
                var identity = new PackageIdentity("A", new NuGetVersion("1.2.3"));
                var testLogger = new TestLogger();

                var packageFileInfo = await TestPackagesCore.GeneratePackageAsync(
                   source,
                   identity.Id,
                   identity.Version.ToString(),
                   DateTimeOffset.UtcNow.LocalDateTime,
                   "content/A.nupkg");

                using (var packageStream = File.OpenRead(packageFileInfo.FullName))
                {
                    var packageExtractionContext = new PackageExtractionContext(
                        PackageSaveMode.Defaultv3,
                        XmlDocFileSaveMode.None,
                        clientPolicyContext: null,
                        testLogger);

                    // Act
                    await PackageExtractor.InstallFromSourceAsync(
                        source,
                        identity,
                        (stream) => packageStream.CopyToAsync(stream, 4096, CancellationToken.None),
                        resolver,
                        packageExtractionContext,
                        CancellationToken.None);

                    // Assert
                    File.Exists(resolver.GetPackageFilePath(identity.Id, identity.Version)).Should().BeTrue();
                    var nupkgMetadata = NupkgMetadataFileFormat.Read(resolver.GetNupkgMetadataPath(identity.Id, identity.Version));
                    testLogger.InformationMessages.Should().Contain($"Installed {identity.Id} {identity.Version} from {source} to {Path.Combine(resolver.RootPath, resolver.GetPackageDirectory(identity.Id, identity.Version))} with content hash {nupkgMetadata.ContentHash}.");
                }
            }
        }

        [Fact]
        public async Task InstallFromSourceAsync_WriteSourceToNupkgMetadata()
        {
            // Arrange
            using (var testDirectory = TestDirectory.Create())
            {
                var source = Path.Combine(testDirectory, "source");
                Directory.CreateDirectory(source);
                var resolver = new VersionFolderPathResolver(Path.Combine(testDirectory, "gpf"));
                var identity = new PackageIdentity("A", new NuGetVersion("1.2.3"));

                var packageFileInfo = await TestPackagesCore.GeneratePackageAsync(
                   source,
                   identity.Id,
                   identity.Version.ToString(),
                   DateTimeOffset.UtcNow.LocalDateTime,
                   "content/A.nupkg");

                using (var packageStream = File.OpenRead(packageFileInfo.FullName))
                {
                    var packageExtractionContext = new PackageExtractionContext(
                        PackageSaveMode.Defaultv3,
                        XmlDocFileSaveMode.None,
                        clientPolicyContext: null,
                        NullLogger.Instance);

                    // Act
                    await PackageExtractor.InstallFromSourceAsync(
                        source,
                        identity,
                        (stream) => packageStream.CopyToAsync(stream, 4096, CancellationToken.None),
                        resolver,
                        packageExtractionContext,
                        CancellationToken.None);

                    // Assert
                    var nupkgMetadataPath = resolver.GetNupkgMetadataPath(identity.Id, identity.Version);
                    Assert.True(File.Exists(nupkgMetadataPath));

                    var nupkgMetadata = NupkgMetadataFileFormat.Read(nupkgMetadataPath);
                    Assert.Equal(source, nupkgMetadata.Source);
                }
            }
        }

        [Fact]
        public async Task InstallFromSourceAsync_WithPackageDownloader_LogsSourceOnNormalLevel()
        {
            // Arrange
            using (var testDirectory = TestDirectory.Create())
            {
                var source = Path.Combine(testDirectory, "source");
                Directory.CreateDirectory(source);
                var resolver = new VersionFolderPathResolver(Path.Combine(testDirectory, "gpf"));
                var identity = new PackageIdentity("A", new NuGetVersion("1.2.3"));
                var testLogger = new TestLogger();

                var packageFileInfo = await TestPackagesCore.GeneratePackageAsync(
                   source,
                   identity.Id,
                   identity.Version.ToString(),
                   DateTimeOffset.UtcNow.LocalDateTime,
                   "content/A.nupkg");

                using (var packageStream = File.OpenRead(packageFileInfo.FullName))
                {
                    var packageExtractionContext = new PackageExtractionContext(
                        PackageSaveMode.Defaultv3,
                        XmlDocFileSaveMode.None,
                        clientPolicyContext: null,
                        testLogger);

                    using (var packageDownloader = new LocalPackageArchiveDownloader(
                        source,
                        Path.Combine(source, $"{identity.Id}.{identity.Version}.nupkg"),
                        identity,
                        testLogger))
                    {
                        var installed = await PackageExtractor.InstallFromSourceAsync(
                            identity,
                            packageDownloader,
                            resolver,
                            packageExtractionContext,
                            CancellationToken.None);

                    }

                    // Assert
                    File.Exists(resolver.GetPackageFilePath(identity.Id, identity.Version)).Should().BeTrue();
                    var nupkgMetadata = NupkgMetadataFileFormat.Read(resolver.GetNupkgMetadataPath(identity.Id, identity.Version));
                    testLogger.InformationMessages.Should().Contain($"Installed {identity.Id} {identity.Version} from {source} to {Path.Combine(resolver.RootPath, resolver.GetPackageDirectory(identity.Id, identity.Version))} with content hash {nupkgMetadata.ContentHash}.");
                }
            }
        }

        private static bool FileExistsCaseSensitively(string expectedFilePath)
        {
            var directoryPath = Path.GetDirectoryName(expectedFilePath);

            return Directory
                .GetFiles(directoryPath, "*", SearchOption.TopDirectoryOnly)
                .Any(filePath => string.Equals(filePath, expectedFilePath, StringComparison.Ordinal));
        }

        private sealed class ExtractPackageAsyncTest : IDisposable
        {
            private readonly TestDirectory _testDirectory;

            internal PackageExtractionContext Context { get; }
            internal DirectoryInfo DestinationDirectory { get; }
            internal FileInfo Package { get; private set; }
            internal PackageIdentity PackageIdentity { get; }
            internal Stream Stream { get; private set; }
            internal PackageReader Reader { get; private set; }
            internal PackagePathResolver Resolver { get; }
            internal IEnvironmentVariableReader EnvironmentVariableReader { get; }

            internal string Source { get; }

            internal ExtractPackageAsyncTest()
                : this(new PackageExtractionContext(
                        PackageSaveMode.Defaultv2,
                        PackageExtractionBehavior.XmlDocFileSaveMode,
                        clientPolicyContext: null,
                        logger: NullLogger.Instance))
            {
            }

            internal ExtractPackageAsyncTest(PackageExtractionContext extractionContext, IEnvironmentVariableReader environmentVariableReader)
                : this(extractionContext)
            {
                EnvironmentVariableReader = environmentVariableReader;
            }

            internal ExtractPackageAsyncTest(PackageExtractionContext extractionContext)
            {
                Context = extractionContext ?? throw new ArgumentNullException(nameof(extractionContext));
                PackageIdentity = new PackageIdentity(id: "a", version: NuGetVersion.Parse("1.0.0"));
                _testDirectory = TestDirectory.Create();

                Source = Path.Combine(_testDirectory.Path, "source");

                Directory.CreateDirectory(Source);

                DestinationDirectory = Directory.CreateDirectory(Path.Combine(_testDirectory.Path, "destination"));

                Resolver = new PackagePathResolver(DestinationDirectory.FullName);
            }

            public void Dispose()
            {
                Reader?.Dispose();
                _testDirectory.Dispose();

                GC.SuppressFinalize(this);
            }

            public async Task CreatePackageAsync()
            {
                var packageContext = new SimpleTestPackageContext()
                {
                    Id = PackageIdentity.Id,
                    Version = PackageIdentity.Version.ToNormalizedString(),
                    Nuspec = XDocument.Parse($@"<?xml version=""1.0"" encoding=""utf-8""?>
                        <package>
                            <metadata>
                                <id>{PackageIdentity.Id}</id>
                                <version>{PackageIdentity.Version.ToNormalizedString()}</version>
                                <title />
                                <language>en-US</language>
                                <contentFiles>
                                    <files include=""lib/net45/{PackageIdentity.Id}.dll"" copyToOutput=""true"" flatten=""false"" />
                                </contentFiles>
                            </metadata>
                        </package>")
                };

                packageContext.AddFile($"lib/net45/{PackageIdentity.Id}.dll");
                packageContext.AddFile($"lib/net45/{PackageIdentity.Id}.xml");

                await SimpleTestPackageUtility.CreatePackagesAsync(Source, packageContext);

                Package = new FileInfo(
                    Path.Combine(
                        Source,
                        $"{PackageIdentity.Id}.{PackageIdentity.Version.ToNormalizedString()}.nupkg"));

                Stream = File.OpenRead(Package.FullName);
                Reader = EnvironmentVariableReader == null ? new PackageReader(File.OpenRead(Package.FullName)) : new PackageReader(File.OpenRead(Package.FullName), environmentalVariableReader: EnvironmentVariableReader);
            }
        }

        private sealed class PackageReader : PackageArchiveReader
        {
            private readonly Stream _stream;

            public PackageReader(Stream stream, IEnvironmentVariableReader environmentalVariableReader)
                : base(stream, environmentalVariableReader)
            {
                _stream = stream;
            }

            public PackageReader(Stream stream)
                : base(stream)
            {
                _stream = stream;
            }

            public override async Task<string> CopyNupkgAsync(string nupkgFilePath, CancellationToken cancellationToken)
            {
                _stream.Seek(offset: 0, origin: SeekOrigin.Begin);

                using (var destination = File.OpenWrite(nupkgFilePath))
                {
#if NETCOREAPP2_0_OR_GREATER
                    await _stream.CopyToAsync(destination, cancellationToken);

#else
                    const int BufferSize = 8192;
                    await _stream.CopyToAsync(destination, BufferSize, cancellationToken);
#endif
                }

                return nupkgFilePath;
            }
        }

        private static Tuple<RepositorySignatureInfo, List<CertificateHashAllowListEntry>> CreateTestRepositorySignatureInfoAndExpectedAllowList()
        {
            var target = VerificationTarget.Repository;
            var placement = SignaturePlacement.PrimarySignature | SignaturePlacement.Countersignature;
            var allSigned = true;
            var firstCertFingerprints = new Dictionary<string, string>()
            {
                { HashAlgorithmName.SHA256.ConvertToOidString(), $"{HashAlgorithmName.SHA256.ToString()}_first" },
                { HashAlgorithmName.SHA384.ConvertToOidString(), $"{HashAlgorithmName.SHA384.ToString()}_first" },
                { HashAlgorithmName.SHA512.ConvertToOidString(), $"{HashAlgorithmName.SHA512.ToString()}_first" }
            };

            var secondCertFingerprints = new Dictionary<string, string>()
            {
                { HashAlgorithmName.SHA256.ConvertToOidString(), $"{HashAlgorithmName.SHA256.ToString()}_second"},
            };

            var repoCertificateInfo = new List<IRepositoryCertificateInfo>()
            {
                new TestRepositoryCertificateInfo()
                {
                    ContentUrl = @"http://unit.test/1",
                    Fingerprints = new Fingerprints(firstCertFingerprints),
                    Issuer = "CN=Issuer1",
                    Subject = "CN=Subject1",
                    NotBefore = DateTimeOffset.UtcNow,
                    NotAfter = DateTimeOffset.UtcNow
                },
                new TestRepositoryCertificateInfo()
                {
                    ContentUrl = @"http://unit.test/2",
                    Fingerprints = new Fingerprints(secondCertFingerprints),
                    Issuer = "CN=Issuer2",
                    Subject = "CN=Subject2",
                    NotBefore = DateTimeOffset.UtcNow,
                    NotAfter = DateTimeOffset.UtcNow
                }
            };

            var repositorySignatureInfo = new RepositorySignatureInfo(allSigned, repoCertificateInfo);

            var expectedAllowList = new List<CertificateHashAllowListEntry>()
            {
                new CertificateHashAllowListEntry(target, placement, $"{HashAlgorithmName.SHA256.ToString()}_first", HashAlgorithmName.SHA256),
                new CertificateHashAllowListEntry(target, placement, $"{HashAlgorithmName.SHA384.ToString()}_first", HashAlgorithmName.SHA384),
                new CertificateHashAllowListEntry(target, placement, $"{HashAlgorithmName.SHA512.ToString()}_first", HashAlgorithmName.SHA512),
                new CertificateHashAllowListEntry(target, placement, $"{HashAlgorithmName.SHA256.ToString()}_second", HashAlgorithmName.SHA256)
            };

            return Tuple.Create(repositorySignatureInfo, expectedAllowList);
        }

        public static IEnumerable<object[]> KnownClientPoliciesList()
        {
            yield return new object[] { new ClientPolicyContext(SignatureValidationMode.Accept, new List<TrustedSignerAllowListEntry>()) };
            yield return new object[] { new ClientPolicyContext(SignatureValidationMode.Require, new List<TrustedSignerAllowListEntry>()) };
        }

        public static IEnumerable<object[]> KnownClientPolicyModesList()
        {
            yield return new object[] { SignatureValidationMode.Accept };
            yield return new object[] { SignatureValidationMode.Require };
        }

#if IS_SIGNING_SUPPORTED
        private static RepositorySignatureInfo CreateTestRepositorySignatureInfo(List<X509Certificate2> certificates, bool allSigned)
        {
            var repoCertificateInfo = new List<IRepositoryCertificateInfo>();

            foreach (var cert in certificates)
            {
                var fingerprintString = SignatureTestUtility.GetFingerprint(cert, HashAlgorithmName.SHA256);

                repoCertificateInfo.Add(new TestRepositoryCertificateInfo()
                {
                    ContentUrl = @"https://v3serviceIndex.test/api/index.json",
                    Fingerprints = new Fingerprints(new Dictionary<string, string>()
                    {
                        { HashAlgorithmName.SHA256.ConvertToOidString(), fingerprintString }
                    }),
                    Issuer = cert.Issuer,
                    Subject = cert.Subject,
                    NotBefore = cert.NotBefore,
                    NotAfter = cert.NotAfter
                });
            }

            return new RepositorySignatureInfo(allSigned, repoCertificateInfo);
        }
#endif
    }
}
