// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Frameworks;
using NuGet.Packaging.Core;
using NuGet.ProjectManagement;
using NuGet.Protocol.Core.Types;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Test.Utility;
using Xunit;

namespace ProjectManagement.Test
{
    public class PackagesConfigNuGetProjectTests
    {
        [Fact]
        public async Task TestInstallPackage()
        {
            // Arrange
            using (var randomTestFolder = TestDirectory.Create())
            {
                var targetFramework = NuGetFramework.Parse("net45");
                var metadata = GetTestMetadata(targetFramework);
                var packagesConfigNuGetProject = new PackagesConfigNuGetProject(randomTestFolder, metadata);
                var packageIdentity = new PackageIdentity("A", new NuGetVersion("1.0.0"));
                var token = CancellationToken.None;
                MakeFileReadOnly(randomTestFolder);

                // Act
                await packagesConfigNuGetProject.InstallPackageAsync(packageIdentity, GetDownloadResourceResult(), new TestNuGetProjectContext(), token);
                MakeFileReadOnly(randomTestFolder);

                // Assert
                var installedPackagesList = (await packagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(1, installedPackagesList.Count);
                Assert.Equal(packageIdentity, installedPackagesList[0].PackageIdentity);
                Assert.Equal(targetFramework, installedPackagesList[0].TargetFramework);
            }
        }

        [Fact]
        public async Task TestInstallPackageUnsupportedFx()
        {
            // Arrange
            using (var randomTestFolder = TestDirectory.Create())
            {
                var targetFramework = NuGetFramework.UnsupportedFramework;
                var metadata = GetTestMetadata(targetFramework);
                var packagesConfigNuGetProject = new PackagesConfigNuGetProject(randomTestFolder, metadata);
                var packageIdentity = new PackageIdentity("A", new NuGetVersion("1.0.0"));
                var token = CancellationToken.None;
                MakeFileReadOnly(randomTestFolder);

                // Act
                await packagesConfigNuGetProject.InstallPackageAsync(packageIdentity, GetDownloadResourceResult(), new TestNuGetProjectContext(), token);
                MakeFileReadOnly(randomTestFolder);

                // Assert
                var installedPackagesList = (await packagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(1, installedPackagesList.Count);
                Assert.Equal(packageIdentity, installedPackagesList[0].PackageIdentity);
                Assert.True(installedPackagesList[0].TargetFramework.IsUnsupported);
            }
        }

        [Fact]
        public async Task TestUninstallLastPackage()
        {
            // Arrange
            using (var randomTestFolder = TestDirectory.Create())
            {
                var targetFramework = NuGetFramework.Parse("net45");
                var metadata = GetTestMetadata(targetFramework);
                var packagesConfigNuGetProject = new PackagesConfigNuGetProject(randomTestFolder, metadata);
                var packageIdentity = new PackageIdentity("A", new NuGetVersion("1.0.0"));
                var testNuGetProjectContext = new TestNuGetProjectContext();
                var token = CancellationToken.None;
                MakeFileReadOnly(randomTestFolder);

                // Act
                await packagesConfigNuGetProject.InstallPackageAsync(packageIdentity, GetDownloadResourceResult(), testNuGetProjectContext, token);
                MakeFileReadOnly(randomTestFolder);

                // Assert
                var installedPackagesList = (await packagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(1, installedPackagesList.Count);
                Assert.Equal(packageIdentity, installedPackagesList[0].PackageIdentity);
                Assert.Equal(targetFramework, installedPackagesList[0].TargetFramework);

                // Main Act
                await packagesConfigNuGetProject.UninstallPackageAsync(packageIdentity, testNuGetProjectContext, token);

                // Main Assert
                installedPackagesList = (await packagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(0, installedPackagesList.Count);
            }
        }

        [Fact]
        public async Task TestInstallSecondPackage()
        {
            // Arrange
            using (var randomTestFolder = TestDirectory.Create())
            {
                var targetFramework = NuGetFramework.Parse("net45");
                var metadata = GetTestMetadata(targetFramework);
                var packagesConfigNuGetProject = new PackagesConfigNuGetProject(randomTestFolder, metadata);

                var packageA = new PackageIdentity("A", new NuGetVersion("1.0.0"));
                var packageB = new PackageIdentity("B", new NuGetVersion("1.0.0"));
                var testNuGetProjectContext = new TestNuGetProjectContext();
                var token = CancellationToken.None;
                MakeFileReadOnly(randomTestFolder);

                // Act
                await packagesConfigNuGetProject.InstallPackageAsync(packageA, GetDownloadResourceResult(), testNuGetProjectContext, token);
                MakeFileReadOnly(randomTestFolder);

                // Assert
                var installedPackagesList = (await packagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(1, installedPackagesList.Count);
                Assert.Equal(packageA, installedPackagesList[0].PackageIdentity);
                Assert.Equal(targetFramework, installedPackagesList[0].TargetFramework);

                // Main Act
                await packagesConfigNuGetProject.InstallPackageAsync(packageB, GetDownloadResourceResult(), testNuGetProjectContext, token);
                // Assert
                installedPackagesList = (await packagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(2, installedPackagesList.Count);
                Assert.Equal(packageA, installedPackagesList[0].PackageIdentity);
                Assert.Equal(packageB, installedPackagesList[1].PackageIdentity);
                Assert.Equal(targetFramework, installedPackagesList[0].TargetFramework);
                Assert.Equal(targetFramework, installedPackagesList[1].TargetFramework);
            }
        }

        [Fact]
        public async Task TestUninstallPenultimatePackage()
        {
            // Arrange
            using (var randomTestFolder = TestDirectory.Create())
            {
                var targetFramework = NuGetFramework.Parse("net45");
                var metadata = GetTestMetadata(targetFramework);
                var packagesConfigNuGetProject = new PackagesConfigNuGetProject(randomTestFolder, metadata);
                var packageA = new PackageIdentity("A", new NuGetVersion("1.0.0"));
                var packageB = new PackageIdentity("B", new NuGetVersion("1.0.0"));
                var testNuGetProjectContext = new TestNuGetProjectContext();
                var token = CancellationToken.None;
                MakeFileReadOnly(randomTestFolder);

                // Act
                await packagesConfigNuGetProject.InstallPackageAsync(packageA, GetDownloadResourceResult(), testNuGetProjectContext, token);
                MakeFileReadOnly(randomTestFolder);

                // Assert
                var installedPackagesList = (await packagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(1, installedPackagesList.Count);
                Assert.Equal(packageA, installedPackagesList[0].PackageIdentity);
                Assert.Equal(targetFramework, installedPackagesList[0].TargetFramework);

                // Act
                await packagesConfigNuGetProject.InstallPackageAsync(packageB, GetDownloadResourceResult(), testNuGetProjectContext, token);
                // Assert
                installedPackagesList = (await packagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(2, installedPackagesList.Count);
                Assert.Equal(packageA, installedPackagesList[0].PackageIdentity);
                Assert.Equal(packageB, installedPackagesList[1].PackageIdentity);
                Assert.Equal(targetFramework, installedPackagesList[0].TargetFramework);
                Assert.Equal(targetFramework, installedPackagesList[1].TargetFramework);

                // Main Act
                await packagesConfigNuGetProject.UninstallPackageAsync(packageA, testNuGetProjectContext, token);

                // Main Assert
                installedPackagesList = (await packagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(1, installedPackagesList.Count);
                Assert.Equal(packageB, installedPackagesList[0].PackageIdentity);
                Assert.Equal(targetFramework, installedPackagesList[0].TargetFramework);
            }
        }

        [Fact]
        public async Task TestInstallHigherVersionPackage()
        {
            // Arrange
            using (var randomTestFolder = TestDirectory.Create())
            {
                var targetFramework = NuGetFramework.Parse("net45");
                var metadata = GetTestMetadata(targetFramework);
                var packagesConfigNuGetProject = new PackagesConfigNuGetProject(randomTestFolder, metadata);
                var packageA1 = new PackageIdentity("A", new NuGetVersion("1.0.0"));
                var packageA2 = new PackageIdentity("A", new NuGetVersion("2.0.0"));
                var token = CancellationToken.None;
                MakeFileReadOnly(randomTestFolder);

                // Act
                await packagesConfigNuGetProject.InstallPackageAsync(packageA1, GetDownloadResourceResult(), new TestNuGetProjectContext(), token);
                MakeFileReadOnly(randomTestFolder);

                // Assert
                var installedPackagesList = (await packagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(1, installedPackagesList.Count);
                Assert.Equal(packageA1, installedPackagesList[0].PackageIdentity);
                Assert.Equal(targetFramework, installedPackagesList[0].TargetFramework);

                // Main Act
                await packagesConfigNuGetProject.InstallPackageAsync(packageA2, GetDownloadResourceResult(), new TestNuGetProjectContext(), token);

                // Assert
                installedPackagesList = (await packagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(1, installedPackagesList.Count);
                Assert.Equal(packageA2, installedPackagesList[0].PackageIdentity);
                Assert.Equal(targetFramework, installedPackagesList[0].TargetFramework);
            }
        }

        [Fact]
        public async Task TestRenameOfPackagesConfigToIncludeProjectName()
        {
            // Arrange
            using (var randomTestFolder = TestDirectory.Create())
            {
                var targetFramework = NuGetFramework.Parse("net45");
                var projectName = "TestProject";
                var metadata = GetTestMetadata(targetFramework, projectName);
                var packagesConfigNuGetProject = new PackagesConfigNuGetProject(randomTestFolder, metadata);
                var packageIdentity = new PackageIdentity("A", new NuGetVersion("1.0.0"));
                var token = CancellationToken.None;
                MakeFileReadOnly(randomTestFolder);

                // Act
                await packagesConfigNuGetProject.InstallPackageAsync(packageIdentity, GetDownloadResourceResult(), new TestNuGetProjectContext(), token);
                MakeFileReadOnly(randomTestFolder);

                // Assert
                var installedPackagesList = (await packagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(1, installedPackagesList.Count);
                Assert.Equal(packageIdentity, installedPackagesList[0].PackageIdentity);
                Assert.Equal(targetFramework, installedPackagesList[0].TargetFramework);

                // Main Act
                var packagesConfigPath = Path.Combine(randomTestFolder, "packages.config");
                var packagesProjectNameConfigPath = Path.Combine(randomTestFolder, "packages." + projectName + ".config");
                File.Move(packagesConfigPath, packagesProjectNameConfigPath);

                // Assert
                installedPackagesList = (await packagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(1, installedPackagesList.Count);
                Assert.Equal(packageIdentity, installedPackagesList[0].PackageIdentity);
                Assert.Equal(targetFramework, installedPackagesList[0].TargetFramework);
            }
        }

        [Fact]
        public async Task TestRenameOfPackagesProjectConfigToExcludeProjectName()
        {
            // Arrange
            using (var randomTestFolder = TestDirectory.Create())
            {
                var targetFramework = NuGetFramework.Parse("net45");
                var projectName = "TestProject";
                var metadata = GetTestMetadata(targetFramework, projectName);
                var packagesConfigNuGetProject = new PackagesConfigNuGetProject(randomTestFolder, metadata);
                var packageIdentity = new PackageIdentity("A", new NuGetVersion("1.0.0"));
                var token = CancellationToken.None;
                MakeFileReadOnly(randomTestFolder);

                // Act
                await packagesConfigNuGetProject.InstallPackageAsync(packageIdentity, GetDownloadResourceResult(), new TestNuGetProjectContext(), token);
                MakeFileReadOnly(randomTestFolder);

                // Assert
                var installedPackagesList = (await packagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(1, installedPackagesList.Count);
                Assert.Equal(packageIdentity, installedPackagesList[0].PackageIdentity);
                Assert.Equal(targetFramework, installedPackagesList[0].TargetFramework);

                // Act
                var packagesConfigPath = Path.Combine(randomTestFolder, "packages.config");
                var packagesProjectNameConfigPath = Path.Combine(randomTestFolder, "packages." + projectName + ".config");
                File.Move(packagesConfigPath, packagesProjectNameConfigPath);

                var packagesConfigNuGetProject2 = new PackagesConfigNuGetProject(randomTestFolder, metadata);

                // Assert
                var installedPackagesList2 = (await packagesConfigNuGetProject2.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(1, installedPackagesList2.Count);
                Assert.Equal(packageIdentity, installedPackagesList2[0].PackageIdentity);
                Assert.Equal(targetFramework, installedPackagesList2[0].TargetFramework);
            }
        }

        private Dictionary<string, object> GetTestMetadata(NuGetFramework targetFramework, string projectName = "TestProject")
        {
            var dict = new Dictionary<string, object>
                {
                    { NuGetProjectMetadataKeys.Name, projectName },
                    { NuGetProjectMetadataKeys.TargetFramework, targetFramework }
                };

            return dict;
        }

        private static void MakeFileReadOnly(string fullPath)
        {
            if (File.Exists(fullPath))
            {
                File.SetAttributes(fullPath, File.GetAttributes(fullPath) | FileAttributes.ReadOnly);
            }
        }

        private static DownloadResourceResult GetDownloadResourceResult()
        {
            return new DownloadResourceResult(Stream.Null, null);
        }
    }
}
