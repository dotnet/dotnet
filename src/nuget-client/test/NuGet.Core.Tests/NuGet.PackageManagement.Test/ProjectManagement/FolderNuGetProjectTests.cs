// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Test.Utility;
using Xunit;

namespace NuGet.ProjectManagement.Test
{
    public class FolderNuGetProjectTests
    {
        private static readonly string Root = Path.GetFullPath("a");

        [Fact]
        public void Constructor_String_ThrowsForNullRoot()
        {
            var exception = Assert.Throws<ArgumentException>(() => new FolderNuGetProject(root: null));

            Assert.Equal("rootDirectory", exception.ParamName);
        }

        [Fact]
        public void Constructor_String_InitializesRootProperty()
        {
            var project = new FolderNuGetProject(Root);

            Assert.Equal(Root, project.Root);
        }

        [Fact]
        public void Constructor_StringPackagePathResolver_ThrowsForNullRoot()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new FolderNuGetProject(
                    root: null,
                    packagePathResolver: new PackagePathResolver(rootDirectory: Root)));

            Assert.Equal("root", exception.ParamName);
        }

        [Fact]
        public void Constructor_StringPackagePathResolver_ThrowsForNullPackagePathResolver()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new FolderNuGetProject(Root, packagePathResolver: null));

            Assert.Equal("packagePathResolver", exception.ParamName);
        }

        [Fact]
        public void Constructor_StringPackagePathResolver_InitializesRootProperty()
        {
            var project = new FolderNuGetProject(
                root: Root,
                packagePathResolver: new PackagePathResolver(rootDirectory: Root));

            Assert.Equal(Root, project.Root);
        }

        [Fact]
        public async Task GetInstalledPackagesAsync_DoesNotThrowIfCancelled()
        {
            var project = new FolderNuGetProject(root: Root);

            await project.GetInstalledPackagesAsync(new CancellationToken(canceled: true));
        }

        [Fact]
        public async Task GetInstalledPackagesAsync_ReturnsEmptyEnumerable()
        {
            var project = new FolderNuGetProject(root: Root);

            var packages = await project.GetInstalledPackagesAsync(CancellationToken.None);

            Assert.Empty(packages);
        }

        [Fact]
        public async Task InstallPackageAsync_InstallsPackage()
        {
            // Arrange
            var packageIdentity = new PackageIdentity("packageA", new NuGetVersion("1.0.0"));

            using (var randomTestSourcePath = TestDirectory.Create())
            using (var randomTestDestinationPath = TestDirectory.Create())
            {
                var packageFileInfo = TestPackagesGroupedByFolder.GetLegacyTestPackage(
                    randomTestSourcePath,
                    packageIdentity.Id,
                    packageIdentity.Version.ToNormalizedString());
                var folderNuGetProject = new FolderNuGetProject(randomTestDestinationPath);
                var packagePathResolver = new PackagePathResolver(randomTestDestinationPath);
                var packageInstallPath = packagePathResolver.GetInstallPath(packageIdentity);
                var nupkgFilePath = Path.Combine(packageInstallPath, packagePathResolver.GetPackageFileName(packageIdentity));
                var testNuGetProjectContext = new TestNuGetProjectContext();
                var token = CancellationToken.None;
                using (var packageStream = GetDownloadResourceResult(randomTestSourcePath, packageFileInfo))
                {
                    // Act
                    await folderNuGetProject.InstallPackageAsync(packageIdentity, packageStream, testNuGetProjectContext, token);
                }

                // Assert
                Assert.True(File.Exists(nupkgFilePath));
                Assert.True(File.Exists(Path.Combine(packageInstallPath, "lib/test.dll")));
                using (var packageStream = File.OpenRead(nupkgFilePath))
                {
                    var zipArchive = new ZipArchive(packageStream);
                    Assert.Equal(5, zipArchive.Entries.Count);
                }
            }
        }

        [Fact]
        public void TryGetMetadata_GetsProjectMetadata()
        {
            // Arrange
            using (var randomTestFolder = TestDirectory.Create())
            {
                var folderNuGetProject = new FolderNuGetProject(randomTestFolder);

                // Act & Assert
                NuGetFramework targetFramework;
                Assert.True(folderNuGetProject.TryGetMetadata(NuGetProjectMetadataKeys.TargetFramework, out targetFramework));
                string name;
                Assert.True(folderNuGetProject.TryGetMetadata(NuGetProjectMetadataKeys.Name, out name));
                Assert.Equal(NuGetFramework.AnyFramework, targetFramework);
                Assert.Equal(randomTestFolder, name);
                Assert.Equal(2, folderNuGetProject.Metadata.Count);
            }
        }

        [Fact]
        public void GetInstalledManifestFilePath_ThrowsForNullPackageIdentity()
        {
            using (var test = new FolderNuGetProjectTest())
            {
                var exception = Assert.Throws<ArgumentNullException>(
                    () => test.Project.GetInstalledManifestFilePath(packageIdentity: null));

                Assert.Equal("packageIdentity", exception.ParamName);
            }
        }

        [Fact]
        public void GetInstalledPackageFilePath_ThrowsForNullPackageIdentity()
        {
            using (var test = new FolderNuGetProjectTest())
            {
                var exception = Assert.Throws<ArgumentNullException>(
                    () => test.Project.GetInstalledPackageFilePath(packageIdentity: null));

                Assert.Equal("packageIdentity", exception.ParamName);
            }
        }

        [Fact]
        public async Task GetInstalledPackageFilePath_ReturnsInstalledPackageFilePath()
        {
            // Arrange
            using (var randomTestSourcePath = TestDirectory.Create())
            using (var randomTestDestinationPath = TestDirectory.Create())
            {
                var packageIdentity = new PackageIdentity("packageA", new NuGetVersion("1.0.0"));
                var packageFileInfo = TestPackagesGroupedByFolder.GetLegacyTestPackage(
                    randomTestSourcePath,
                    packageIdentity.Id,
                    packageIdentity.Version.ToNormalizedString());
                var folderNuGetProject = new FolderNuGetProject(randomTestDestinationPath);
                var packagePathResolver = new PackagePathResolver(randomTestDestinationPath);
                var packageInstallPath = packagePathResolver.GetInstallPath(packageIdentity);
                var nupkgFilePath = Path.Combine(packageInstallPath, packagePathResolver.GetPackageFileName(packageIdentity));
                var testNuGetProjectContext = new TestNuGetProjectContext();
                var token = CancellationToken.None;
                using (var packageStream = GetDownloadResourceResult(randomTestSourcePath, packageFileInfo))
                {
                    // Act
                    await folderNuGetProject.InstallPackageAsync(packageIdentity, packageStream, testNuGetProjectContext, token);
                }

                // Assert
                Assert.True(File.Exists(nupkgFilePath));
                Assert.True(File.Exists(Path.Combine(packageInstallPath, "lib/test.dll")));
                using (var packageStream = File.OpenRead(nupkgFilePath))
                {
                    var zipArchive = new ZipArchive(packageStream);
                    Assert.Equal(5, zipArchive.Entries.Count);
                }

                // Main Act
                var installedPackageFilePath = folderNuGetProject.GetInstalledPackageFilePath(
                    new PackageIdentity(packageIdentity.Id,
                    new NuGetVersion(packageIdentity.Version + ".0")));

                // Assert
                Assert.NotNull(installedPackageFilePath);
                Assert.True(File.Exists(installedPackageFilePath));
                Assert.Equal(nupkgFilePath, installedPackageFilePath);
            }
        }

        [Fact]
        public void GetInstalledPath_ThrowsForNullPackageIdentity()
        {
            using (var test = new FolderNuGetProjectTest())
            {
                var exception = Assert.Throws<ArgumentNullException>(
                    () => test.Project.GetInstalledPath(packageIdentity: null));

                Assert.Equal("packageIdentity", exception.ParamName);
            }
        }

        [Fact]
        public async Task GetInstalledPath_ReturnsInstalledPath()
        {
            // Arrange
            using (var randomTestSourcePath = TestDirectory.Create())
            using (var randomTestDestinationPath = TestDirectory.Create())
            {
                var packageIdentity = new PackageIdentity("packageA", new NuGetVersion("1.0.0"));
                var packageFileInfo = TestPackagesGroupedByFolder.GetLegacyTestPackage(
                    randomTestSourcePath,
                    packageIdentity.Id,
                    packageIdentity.Version.ToNormalizedString());
                var folderNuGetProject = new FolderNuGetProject(randomTestDestinationPath);
                var packagePathResolver = new PackagePathResolver(randomTestDestinationPath);
                var packageInstallPath = packagePathResolver.GetInstallPath(packageIdentity);
                var nupkgFilePath = Path.Combine(packageInstallPath, packagePathResolver.GetPackageFileName(packageIdentity));
                var testNuGetProjectContext = new TestNuGetProjectContext();
                var token = CancellationToken.None;
                using (var packageStream = GetDownloadResourceResult(randomTestSourcePath, packageFileInfo))
                {
                    // Act
                    await folderNuGetProject.InstallPackageAsync(packageIdentity, packageStream, testNuGetProjectContext, token);
                }

                // Assert
                Assert.True(File.Exists(nupkgFilePath));
                Assert.True(File.Exists(Path.Combine(packageInstallPath, "lib/test.dll")));
                using (var packageStream = File.OpenRead(nupkgFilePath))
                {
                    var zipArchive = new ZipArchive(packageStream);
                    Assert.Equal(5, zipArchive.Entries.Count);
                }

                // Main Act
                var installedPath = folderNuGetProject.GetInstalledPath(
                    new PackageIdentity(
                        packageIdentity.Id,
                        new NuGetVersion(packageIdentity.Version + ".0")));

                // Assert
                Assert.NotNull(installedPath);
                Assert.True(Directory.Exists(installedPath));
                Assert.Equal(packageInstallPath, installedPath);
            }
        }

        [Fact]
        public void PackageExists_PackageIdentity_ThrowsForNullPackageIdentity()
        {
            var project = new FolderNuGetProject(root: Root);

            var exception = Assert.Throws<ArgumentNullException>(() => project.PackageExists(packageIdentity: null));

            Assert.Equal("packageIdentity", exception.ParamName);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void PackageExists_PackageIdentity_ReturnsFalseIfPackageDoesNotExist(bool useSideBySidePaths)
        {
            using (var test = new FolderNuGetProjectTest(useSideBySidePaths))
            {
                Assert.False(test.Project.PackageExists(test.PackageIdentity));
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task PackageExists_PackageIdentity_ReturnsTrueIfPackageExists(bool useSideBySidePaths)
        {
            using (var test = new FolderNuGetProjectTest(useSideBySidePaths))
            {
                await test.InstallAsync(PackageSaveMode.Nupkg);

                Assert.True(test.Project.PackageExists(test.PackageIdentity));
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void PackageExists_PackageIdentity_ReturnsTrueIfPackageDownloadMarkerFileExists(bool useSideBySidePaths)
        {
            using (var test = new FolderNuGetProjectTest(useSideBySidePaths))
            {
                test.CreatePackageDownloadMarkerFile();

                Assert.True(test.Project.PackageExists(test.PackageIdentity));
            }
        }

        [Fact]
        public async Task PackageExists_ReturnsTrueIfPackageExists()
        {
            // Arrange
            using (var randomTestSourcePath = TestDirectory.Create())
            using (var randomTestDestinationPath = TestDirectory.Create())
            {
                var packageIdentity = new PackageIdentity("packageA", new NuGetVersion("1.0.0"));
                var packageFileInfo = TestPackagesGroupedByFolder.GetLegacyTestPackage(
                    randomTestSourcePath,
                    packageIdentity.Id,
                    packageIdentity.Version.ToNormalizedString());
                var folderNuGetProject = new FolderNuGetProject(randomTestDestinationPath);
                var packagePathResolver = new PackagePathResolver(randomTestDestinationPath);
                var packageInstallPath = packagePathResolver.GetInstallPath(packageIdentity);
                var nupkgFilePath = Path.Combine(packageInstallPath, packagePathResolver.GetPackageFileName(packageIdentity));
                var testNuGetProjectContext = new TestNuGetProjectContext();
                var token = CancellationToken.None;
                using (var packageStream = GetDownloadResourceResult(randomTestSourcePath, packageFileInfo))
                {
                    // Act
                    await folderNuGetProject.InstallPackageAsync(packageIdentity, packageStream, testNuGetProjectContext, token);
                }

                // Assert
                Assert.True(File.Exists(nupkgFilePath));
                Assert.True(File.Exists(Path.Combine(packageInstallPath, "lib/test.dll")));
                using (var packageStream = File.OpenRead(nupkgFilePath))
                {
                    var zipArchive = new ZipArchive(packageStream);
                    Assert.Equal(5, zipArchive.Entries.Count);
                }

                // Main Act
                var packageExists = folderNuGetProject.PackageExists(
                    new PackageIdentity(packageIdentity.Id,
                    new NuGetVersion(packageIdentity.Version + ".0")));

                // Assert
                Assert.True(packageExists);
            }
        }

        [Fact]
        public void PackageExists_PackageIdentityPackageSaveMode_ThrowsForNullPackageIdentity()
        {
            var project = new FolderNuGetProject(root: Root);

            var exception = Assert.Throws<ArgumentNullException>(
                () => project.PackageExists(packageIdentity: null, packageSaveMode: PackageSaveMode.Nupkg));

            Assert.Equal("packageIdentity", exception.ParamName);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void PackageExists_PackageIdentityPackageSaveMode_ReturnsFalseIfPackageDoesNotExist(
            bool useSideBySidePaths)
        {
            using (var test = new FolderNuGetProjectTest(useSideBySidePaths))
            {
                Assert.False(test.Project.PackageExists(test.PackageIdentity, PackageSaveMode.Nupkg));
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task PackageExists_PackageIdentityPackageSaveMode_ReturnsTrueIfPackageExists(bool useSideBySidePaths)
        {
            using (var test = new FolderNuGetProjectTest(useSideBySidePaths))
            {
                await test.InstallAsync(PackageSaveMode.Nupkg);

                Assert.True(test.Project.PackageExists(test.PackageIdentity, PackageSaveMode.Nupkg));
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void PackageExists_PackageIdentityPackageSaveMode_ReturnsTrueIfPackageDownloadMarkerFileExists(
            bool useSideBySidePaths)
        {
            using (var test = new FolderNuGetProjectTest(useSideBySidePaths))
            {
                test.CreatePackageDownloadMarkerFile();

                Assert.True(test.Project.PackageExists(test.PackageIdentity, PackageSaveMode.Nupkg));
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void PackageExists_PackageIdentityPackageSaveMode_ReturnsFalseIfNuspecDoesNotExist(
            bool useSideBySidePaths)
        {
            using (var test = new FolderNuGetProjectTest(useSideBySidePaths))
            {
                Assert.False(test.Project.PackageExists(test.PackageIdentity, PackageSaveMode.Nuspec));
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task PackageExists_PackageIdentityPackageSaveMode_ReturnsTrueIfNuspecExists(
            bool useSideBySidePaths)
        {
            using (var test = new FolderNuGetProjectTest(useSideBySidePaths))
            {
                await test.InstallAsync(PackageSaveMode.Nuspec);

                Assert.True(test.Project.PackageExists(test.PackageIdentity, PackageSaveMode.Nuspec));
            }
        }

        [Theory]
        [InlineData(PackageSaveMode.None)]
        [InlineData(PackageSaveMode.Nuspec)]
        [InlineData(PackageSaveMode.Files)]
        public async Task PackageExists_PackageIdentityPackageSaveMode_ReturnsFalseIfDefaultv2NotInstalled(
            PackageSaveMode packageSaveMode)
        {
            using (var test = new FolderNuGetProjectTest())
            {
                await test.InstallAsync(packageSaveMode);

                Assert.False(test.Project.PackageExists(test.PackageIdentity, PackageSaveMode.Defaultv2));
            }
        }

        [Theory]
        [InlineData(PackageSaveMode.Nupkg)]
        [InlineData(PackageSaveMode.Defaultv2)]
        public async Task PackageExists_PackageIdentityPackageSaveMode_ReturnsTrueIfDefaultv2Installed(
            PackageSaveMode packageSaveMode)
        {
            using (var test = new FolderNuGetProjectTest())
            {
                await test.InstallAsync(packageSaveMode);

                Assert.True(test.Project.PackageExists(test.PackageIdentity, PackageSaveMode.Defaultv2));
            }
        }

        [Theory]
        [InlineData(PackageSaveMode.None)]
        [InlineData(PackageSaveMode.Nuspec)]
        [InlineData(PackageSaveMode.Nupkg)]
        [InlineData(PackageSaveMode.Files)]
        public async Task PackageExists_PackageIdentityPackageSaveMode_ReturnsFalseIfDefaultv3NotInstalled(
            PackageSaveMode packageSaveMode)
        {
            using (var test = new FolderNuGetProjectTest())
            {
                await test.InstallAsync(packageSaveMode);

                Assert.False(test.Project.PackageExists(test.PackageIdentity, PackageSaveMode.Defaultv3));
            }
        }

        [Theory]
        [InlineData(PackageSaveMode.Nupkg | PackageSaveMode.Nuspec)]
        [InlineData(PackageSaveMode.Defaultv3)]
        public async Task PackageExists_PackageIdentityPackageSaveMode_ReturnsTrueIfDefaultv3Installed(
            PackageSaveMode packageSaveMode)
        {
            using (var test = new FolderNuGetProjectTest())
            {
                await test.InstallAsync(packageSaveMode);

                Assert.True(test.Project.PackageExists(test.PackageIdentity, PackageSaveMode.Defaultv3));
            }
        }

        [Fact]
        public void ManifestExists_ThrowsForNullPackageIdentity()
        {
            using (var test = new FolderNuGetProjectTest())
            {
                var exception = Assert.Throws<ArgumentNullException>(
                    () => test.Project.ManifestExists(packageIdentity: null));

                Assert.Equal("packageIdentity", exception.ParamName);
            }
        }

        [Fact]
        public void ManifestExists_ReturnsFalseIfNuspecNotInstalled()
        {
            using (var test = new FolderNuGetProjectTest())
            {
                var exists = test.Project.ManifestExists(test.PackageIdentity);

                Assert.False(exists);
            }
        }

        [Fact]
        public async Task ManifestExists_ReturnsTrueIfNuspecInstalled()
        {
            using (var test = new FolderNuGetProjectTest())
            {
                await test.InstallAsync(PackageSaveMode.Nuspec);

                var exists = test.Project.ManifestExists(test.PackageIdentity);

                Assert.True(exists);
            }
        }

        [Fact]
        public void PackageAndManifestExists_ThrowsForNullPackageIdentity()
        {
            using (var test = new FolderNuGetProjectTest())
            {
                var exception = Assert.Throws<ArgumentNullException>(
                    () => test.Project.PackageAndManifestExists(packageIdentity: null));

                Assert.Equal("packageIdentity", exception.ParamName);
            }
        }

        [Fact]
        public void PackageAndManifestExists_ReturnsFalseIfNuspecNotInstalled()
        {
            using (var test = new FolderNuGetProjectTest())
            {
                var exists = test.Project.PackageAndManifestExists(test.PackageIdentity);

                Assert.False(exists);
            }
        }

        [Fact]
        public async Task PackageAndManifestExist_ReturnsTrueIfNuspecInstalled()
        {
            using (var test = new FolderNuGetProjectTest())
            {
                await test.InstallAsync(PackageSaveMode.Nupkg | PackageSaveMode.Nuspec);

                var exists = test.Project.PackageAndManifestExists(test.PackageIdentity);

                Assert.True(exists);
            }
        }

        [Fact]
        public async Task DeletePackage_ThrowsForNullPackageIdentity()
        {
            using (var test = new FolderNuGetProjectTest())
            {
                var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                    () => test.Project.DeletePackage(
                        packageIdentity: null,
                        nuGetProjectContext: Mock.Of<INuGetProjectContext>(),
                        token: CancellationToken.None));

                Assert.Equal("packageIdentity", exception.ParamName);
            }
        }

        [Fact]
        public async Task DeletePackage_ThrowsForNullNuGetProjectContext()
        {
            using (var test = new FolderNuGetProjectTest())
            {
                var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                    () => test.Project.DeletePackage(
                        test.PackageIdentity,
                        nuGetProjectContext: null,
                        token: CancellationToken.None));

                Assert.Equal("nuGetProjectContext", exception.ParamName);
            }
        }

        [Fact]
        public async Task DeletePackage_DeletesPackage()
        {
            // Arrange
            using (var randomTestSourcePath = TestDirectory.Create())
            using (var randomTestDestinationPath = TestDirectory.Create())
            {
                var packageIdentity = new PackageIdentity("packageA", new NuGetVersion("1.0.0"));
                var packageFileInfo = TestPackagesGroupedByFolder.GetLegacyContentPackage(
                    randomTestSourcePath,
                    packageIdentity.Id,
                    packageIdentity.Version.ToNormalizedString());
                var folderNuGetProject = new FolderNuGetProject(randomTestDestinationPath);
                var packagePathResolver = new PackagePathResolver(randomTestDestinationPath);
                var packageInstallPath = packagePathResolver.GetInstallPath(packageIdentity);
                var nupkgFilePath = Path.Combine(packageInstallPath, packagePathResolver.GetPackageFileName(packageIdentity));
                var testNuGetProjectContext = new TestNuGetProjectContext();
                var token = CancellationToken.None;
                using (var packageStream = GetDownloadResourceResult(randomTestSourcePath, packageFileInfo))
                {
                    // Act
                    await folderNuGetProject.InstallPackageAsync(packageIdentity, packageStream, testNuGetProjectContext, token);
                }

                var unNormalizedPackageIdentity = new PackageIdentity(packageIdentity.Id,
                    new NuGetVersion(packageIdentity.Version + ".0"));

                // Assert
                Assert.True(File.Exists(nupkgFilePath));
                Assert.True(File.Exists(Path.Combine(packageInstallPath, "Content/Scripts/test1.js")));
                using (var packageStream = File.OpenRead(nupkgFilePath))
                {
                    var zipArchive = new ZipArchive(packageStream);
                    Assert.Equal(6, zipArchive.Entries.Count);
                }
                Assert.True(folderNuGetProject.PackageExists(packageIdentity));
                var packageDirectoryPath = folderNuGetProject.GetInstalledPath(unNormalizedPackageIdentity);
                Assert.True(!string.IsNullOrEmpty(packageDirectoryPath));
                Assert.True(Directory.Exists(packageDirectoryPath));

                // Main Act
                await folderNuGetProject.DeletePackage(packageIdentity, testNuGetProjectContext, CancellationToken.None);

                // Assert
                Assert.False(folderNuGetProject.PackageExists(unNormalizedPackageIdentity));
                // Check that the package directories are deleted
                Assert.False(Directory.Exists(packageDirectoryPath));
            }
        }

        [Fact]
        public async Task InstallPackageAsync_WithSourceControlEnabled()
        {
            // Arrange
            var packageIdentity = new PackageIdentity("packageA", new NuGetVersion("1.0.0"));

            using (var randomTestSourcePath = TestDirectory.Create())
            using (var randomTestDestinationPath = TestDirectory.Create())
            {
                var packageFileInfo = TestPackagesGroupedByFolder.GetLegacyTestPackage(
                    randomTestSourcePath,
                    packageIdentity.Id,
                    packageIdentity.Version.ToNormalizedString());

                var folderNuGetProject = new FolderNuGetProject(randomTestDestinationPath);
                var packagePathResolver = new PackagePathResolver(randomTestDestinationPath);
                var packageInstallPath = packagePathResolver.GetInstallPath(packageIdentity);
                var nupkgFilePath
                    = Path.Combine(packageInstallPath, packagePathResolver.GetPackageFileName(packageIdentity));
                var testSourceControlManager = new TestSourceControlManager();
                var testNuGetProjectContext = new TestNuGetProjectContext()
                {
                    SourceControlManagerProvider = new TestSourceControlManagerProvider(testSourceControlManager)
                };

                var token = CancellationToken.None;
                using (var packageStream = GetDownloadResourceResult(randomTestSourcePath, packageFileInfo))
                {
                    // Act
                    await folderNuGetProject.InstallPackageAsync(
                        packageIdentity,
                        packageStream,
                        testNuGetProjectContext,
                        token);
                }

                // Assert
                Assert.True(File.Exists(nupkgFilePath));

                Assert.Equal(5, testSourceControlManager.PendAddedFiles.Count);
                Assert.Contains(nupkgFilePath, testSourceControlManager.PendAddedFiles);
                var expectedEntries = new[]
                {
                    "lib/test.dll",
                    "lib/net40/test40.dll",
                    "lib/net40/test40b.dll",
                    "lib/net45/test45.dll"
                };

                Assert.All(
                    expectedEntries.Select(e => Path.Combine(packageInstallPath, e.Replace('/', Path.DirectorySeparatorChar))),
                    item => Assert.Contains(item, testSourceControlManager.PendAddedFiles));
            }
        }

        [Fact]
        public async Task InstallPackageAsync_WithSourceControlDisabled()
        {
            // Arrange
            var packageIdentity = new PackageIdentity("packageA", new NuGetVersion("1.0.0"));

            using (var randomTestSourcePath = TestDirectory.Create())
            using (var randomTestDestinationPath = TestDirectory.Create())
            {
                var packageFileInfo = TestPackagesGroupedByFolder.GetLegacyTestPackage(
                randomTestSourcePath,
                packageIdentity.Id,
                packageIdentity.Version.ToNormalizedString());

                // Create a nuget.config file with source control disabled
                File.WriteAllText(
                    Path.Combine(randomTestSourcePath, "nuget.config"),
                    @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
  <solution>
    <add key=""disableSourceControlIntegration"" value=""true"" />
  </solution >
</configuration>");

                var settings = new Settings(randomTestSourcePath);

                var folderNuGetProject = new FolderNuGetProject(randomTestDestinationPath);
                var packagePathResolver = new PackagePathResolver(randomTestDestinationPath);
                var packageInstallPath = packagePathResolver.GetInstallPath(packageIdentity);
                var nupkgFilePath
                    = Path.Combine(packageInstallPath, packagePathResolver.GetPackageFileName(packageIdentity));
                var testSourceControlManager = new TestSourceControlManager(settings);
                var testNuGetProjectContext = new TestNuGetProjectContext()
                {
                    SourceControlManagerProvider = new TestSourceControlManagerProvider(testSourceControlManager)
                };

                var token = CancellationToken.None;
                using (var packageStream = GetDownloadResourceResult(randomTestSourcePath, packageFileInfo))
                {
                    // Act
                    await folderNuGetProject.InstallPackageAsync(
                        packageIdentity,
                        packageStream,
                        testNuGetProjectContext,
                        token);
                }

                // Assert
                Assert.True(File.Exists(nupkgFilePath));

                Assert.Equal(0, testSourceControlManager.PendAddedFiles.Count);
            }
        }

        [Fact]
        public async Task UninstallPackageAsync_DoesNothing()
        {
            var project = new FolderNuGetProject(root: Root);

            var wasUninstalled = await project.UninstallPackageAsync(
                new PackageIdentity(id: "a", version: NuGetVersion.Parse("1.0.0")),
                new Mock<INuGetProjectContext>(MockBehavior.Strict).Object,
                CancellationToken.None);

            Assert.True(wasUninstalled);
        }

        [Fact]
        public void GetPackageDownloadMarkerFilePath_ThrowsForNullPackageIdentity()
        {
            using (var test = new FolderNuGetProjectTest())
            {
                var exception = Assert.Throws<ArgumentNullException>(
                    () => test.Project.GetPackageDownloadMarkerFilePath(packageIdentity: null));

                Assert.Equal("packageIdentity", exception.ParamName);
            }
        }

        [Fact]
        public void GetPackageDownloadMarkerFilePath_ReturnsNullIfFileDoesNotExist()
        {
            using (var test = new FolderNuGetProjectTest())
            {
                var filePath = test.Project.GetPackageDownloadMarkerFilePath(test.PackageIdentity);

                Assert.Null(filePath);
            }
        }

        [Fact]
        public void GetPackageDownloadMarkerFilePath_ReturnsFilePathIfFileExists()
        {
            using (var test = new FolderNuGetProjectTest())
            {
                test.CreatePackageDownloadMarkerFile();

                var filePath = test.Project.GetPackageDownloadMarkerFilePath(test.PackageIdentity);

                Assert.NotNull(filePath);
                Assert.True(File.Exists(filePath));
            }
        }

        [Fact]
        public async Task CopySatelliteFilesAsync_ThrowsForNullPackageIdentity()
        {
            using (var test = new FolderNuGetProjectTest())
            {
                var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                    () => test.Project.CopySatelliteFilesAsync(
                        packageIdentity: null,
                        nuGetProjectContext: Mock.Of<INuGetProjectContext>(),
                        token: CancellationToken.None));

                Assert.Equal("packageIdentity", exception.ParamName);
            }
        }

        [Fact]
        public async Task CopySatelliteFilesAsync_ThrowsForNullNuGetProjectContext()
        {
            using (var test = new FolderNuGetProjectTest())
            {
                var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                    () => test.Project.CopySatelliteFilesAsync(
                        test.PackageIdentity,
                        nuGetProjectContext: null,
                        token: CancellationToken.None));

                Assert.Equal("nuGetProjectContext", exception.ParamName);
            }
        }

        [Fact]
        public async Task CopySatelliteFilesAsync_ThrowsIfCancelled()
        {
            using (var test = new FolderNuGetProjectTest())
            {
                await Assert.ThrowsAsync<OperationCanceledException>(
                    () => test.Project.CopySatelliteFilesAsync(
                        test.PackageIdentity,
                        Mock.Of<INuGetProjectContext>(),
                        new CancellationToken(canceled: true)));
            }
        }

        private static DownloadResourceResult GetDownloadResourceResult(string source, FileInfo fileInfo)
        {
            return new DownloadResourceResult(fileInfo.OpenRead(), source);
        }

        private sealed class FolderNuGetProjectTest : IDisposable
        {
            internal FileInfo Package { get; }
            internal PackageIdentity PackageIdentity { get; }
            internal FolderNuGetProject Project { get; }
            internal DirectoryInfo ProjectDirectory { get; }
            internal PackagePathResolver Resolver { get; }
            internal TestDirectory TestDirectory { get; }

            internal string Source { get; }

            internal FolderNuGetProjectTest(bool useSideBySidePaths = true)
            {
                PackageIdentity = new PackageIdentity(id: "a", version: NuGetVersion.Parse("1.0.0"));
                TestDirectory = TestDirectory.Create();
                ProjectDirectory = Directory.CreateDirectory(Path.Combine(TestDirectory.Path, "project"));
                Resolver = new PackagePathResolver(ProjectDirectory.FullName, useSideBySidePaths);
                Project = new FolderNuGetProject(ProjectDirectory.FullName, Resolver);

                Source = Path.Combine(TestDirectory.Path, "source");

                Directory.CreateDirectory(Source);

                Package = TestPackagesGroupedByFolder.GetLegacyTestPackage(
                    Source,
                    PackageIdentity.Id,
                    PackageIdentity.Version.ToNormalizedString());
            }

            public void Dispose()
            {
                TestDirectory.Dispose();
                GC.SuppressFinalize(this);
            }

            internal void CreatePackageDownloadMarkerFile()
            {
                var packageDirectory = Resolver.GetInstallPath(PackageIdentity);
                var markerFileName = Resolver.GetPackageDownloadMarkerFileName(PackageIdentity);

                Directory.CreateDirectory(packageDirectory);

                File.WriteAllText(Path.Combine(packageDirectory, markerFileName), string.Empty);
            }

            internal async Task InstallAsync(PackageSaveMode packageSaveMode)
            {
                using (var result = GetDownloadResourceResult(Source, Package))
                {
                    var projectContext = new TestNuGetProjectContext();

                    projectContext.PackageExtractionContext.PackageSaveMode = packageSaveMode;

                    await Project.InstallPackageAsync(
                        PackageIdentity,
                        result,
                        projectContext,
                        CancellationToken.None);
                }
            }
        }
    }
}
