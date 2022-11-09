// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Configuration;
using NuGet.Packaging.Core;
using NuGet.ProjectManagement;
using NuGet.Protocol.Core.Types;
using NuGet.Protocol.VisualStudio;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Xunit;

namespace NuGet.PackageManagement.Test
{
    public class PackagePreFetcherTests
    {
        [Fact]
        public async Task PackagePreFetcher_NoActionsInput()
        {
            using (var packagesFolderDir = TestDirectory.Create())
            using (var globalPackagesFolder = TestDirectory.Create())
            {
                // Arrange
                var actions = new List<NuGetProjectAction>();
                var packagesFolder = new FolderNuGetProject(packagesFolderDir);
                var testSettings = NullSettings.Instance;
                var logger = new TestLogger();

                // Act
                using (var cacheContext = new SourceCacheContext())
                {
                    var result = await PackagePreFetcher.GetPackagesAsync(
                        actions,
                        packagesFolder,
                        new PackageDownloadContext(cacheContext),
                        globalPackagesFolder,
                        logger,
                        CancellationToken.None);
                    // Assert
                    Assert.Equal(0, result.Count);
                }
            }
        }

        [Fact]
        public async Task PackagePreFetcher_NoInstallActionsInput()
        {
            using (var packagesFolderDir = TestDirectory.Create())
            using (var globalPackagesFolder = TestDirectory.Create())
            {
                // Arrange
                var actions = new List<NuGetProjectAction>();
                var packagesFolder = new FolderNuGetProject(packagesFolderDir);
                var testSettings = NullSettings.Instance;
                var logger = new TestLogger();

                var target = new PackageIdentity("packageA", NuGetVersion.Parse("1.0.0"));
                var target2 = new PackageIdentity("packageB", NuGetVersion.Parse("1.0.0"));

                actions.Add(NuGetProjectAction.CreateUninstallProjectAction(target, packagesFolder));
                actions.Add(NuGetProjectAction.CreateUninstallProjectAction(target2, packagesFolder));

                // Act
                using (var cacheContext = new SourceCacheContext())
                {
                    var result = await PackagePreFetcher.GetPackagesAsync(
                        actions,
                        packagesFolder,
                        new PackageDownloadContext(cacheContext),
                        globalPackagesFolder,
                        logger,
                        CancellationToken.None);

                    // Assert
                    Assert.Equal(0, result.Count);
                }
            }
        }

        [Fact]
        public async Task PackagePreFetcher_PackageAlreadyExists()
        {
            using (var sourceDir = TestDirectory.Create())
            using (var packagesFolderDir = TestDirectory.Create())
            using (var globalPackagesFolder = TestDirectory.Create())
            {
                // Arrange
                var actions = new List<NuGetProjectAction>();
                var packagesFolder = new FolderNuGetProject(packagesFolderDir);
                var testSettings = NullSettings.Instance;
                var logger = new TestLogger();
                var target = new PackageIdentity("packageA", NuGetVersion.Parse("1.0.0"));
                var source = Repository.Factory.GetVisualStudio(new Configuration.PackageSource(sourceDir.Path));

                // Add package
                AddToPackagesFolder(target, packagesFolderDir);
                actions.Add(NuGetProjectAction.CreateInstallProjectAction(target, source, packagesFolder));

                AddToSource(target, sourceDir);

                // Act
                using (var cacheContext = new SourceCacheContext())
                {
                    var result = await PackagePreFetcher.GetPackagesAsync(
                        actions,
                        packagesFolder,
                        new PackageDownloadContext(cacheContext),
                        globalPackagesFolder,
                        logger,
                        CancellationToken.None);

                    using (var downloadResult = await result[target].GetResultAsync())
                    {
                        // Assert
                        Assert.Equal(1, result.Count);
                        Assert.True(result[target].InPackagesFolder);
                        Assert.Null(result[target].Source);
                        Assert.Equal(target, result[target].Package);
                        Assert.True(result[target].IsComplete);
                        Assert.Equal(target, downloadResult.PackageReader.GetIdentity());
                        Assert.NotNull(downloadResult.PackageStream);
                        Assert.Equal(DownloadResourceResultStatus.Available, downloadResult.Status);
                    }
                }
            }
        }

        [Fact]
        public async Task PackagePreFetcher_PackageAlreadyExistsReinstall()
        {
            using (var sourceDir = TestDirectory.Create())
            using (var packagesFolderDir = TestDirectory.Create())
            using (var globalPackagesFolder = TestDirectory.Create())
            {
                // Arrange
                var actions = new List<NuGetProjectAction>();
                var packagesFolder = new FolderNuGetProject(packagesFolderDir);
                var testSettings = NullSettings.Instance;
                var logger = new TestLogger();
                var target = new PackageIdentity("packageA", NuGetVersion.Parse("1.0.0"));
                var source = Repository.Factory.GetVisualStudio(new Configuration.PackageSource(sourceDir.Path));

                // Add package
                AddToPackagesFolder(target, packagesFolderDir);
                actions.Add(NuGetProjectAction.CreateUninstallProjectAction(target, packagesFolder));
                actions.Add(NuGetProjectAction.CreateInstallProjectAction(target, source, packagesFolder));

                AddToSource(target, sourceDir);

                // Act
                using (var cacheContext = new SourceCacheContext())
                {
                    var result = await PackagePreFetcher.GetPackagesAsync(
                        actions,
                        packagesFolder,
                        new PackageDownloadContext(cacheContext),
                        globalPackagesFolder,
                        logger,
                        CancellationToken.None);

                    using (var downloadResult = await result[target].GetResultAsync())
                    {
                        // Assert
                        Assert.Equal(1, result.Count);
                        Assert.False(result[target].InPackagesFolder);
                        Assert.Equal(source.PackageSource, result[target].Source);
                        Assert.Equal(target, result[target].Package);
                        Assert.True(result[target].IsComplete);
                        Assert.Equal(target, downloadResult.PackageReader.GetIdentity());
                        Assert.NotNull(downloadResult.PackageStream);
                        Assert.Equal(DownloadResourceResultStatus.Available, downloadResult.Status);
                    }
                }
            }
        }

        [Fact]
        public async Task PackagePreFetcher_UpdateMultiplePackages()
        {
            using (var sourceDir = TestDirectory.Create())
            using (var packagesFolderDir = TestDirectory.Create())
            using (var globalPackagesFolder = TestDirectory.Create())
            {
                // Arrange
                var actions = new List<NuGetProjectAction>();
                var packagesFolder = new FolderNuGetProject(packagesFolderDir);
                var testSettings = NullSettings.Instance;
                var logger = new TestLogger();


                var targetA1 = new PackageIdentity("packageA", NuGetVersion.Parse("1.0.0"));
                var targetA2 = new PackageIdentity("packageA", NuGetVersion.Parse("2.0.0"));
                var targetB1 = new PackageIdentity("packageB", NuGetVersion.Parse("1.0.0"));
                var targetB2 = new PackageIdentity("packageB", NuGetVersion.Parse("2.0.0"));
                var targetC2 = new PackageIdentity("packageC", NuGetVersion.Parse("2.0.0"));

                var source = Repository.Factory.GetVisualStudio(new Configuration.PackageSource(sourceDir.Path));

                // Add packages
                AddToPackagesFolder(targetA1, packagesFolderDir);
                AddToPackagesFolder(targetB1, packagesFolderDir);
                AddToPackagesFolder(targetA2, packagesFolderDir);

                // Update A and B, install C, A already exists
                actions.Add(NuGetProjectAction.CreateUninstallProjectAction(targetA1, packagesFolder));
                actions.Add(NuGetProjectAction.CreateUninstallProjectAction(targetB1, packagesFolder));

                actions.Add(NuGetProjectAction.CreateInstallProjectAction(targetC2, source, packagesFolder));
                actions.Add(NuGetProjectAction.CreateInstallProjectAction(targetB2, source, packagesFolder));
                actions.Add(NuGetProjectAction.CreateInstallProjectAction(targetA2, source, packagesFolder));

                AddToSource(targetA2, sourceDir);
                AddToSource(targetB2, sourceDir);
                AddToSource(targetC2, sourceDir);

                // Act
                using (var cacheContext = new SourceCacheContext())
                {
                    var result = await PackagePreFetcher.GetPackagesAsync(
                        actions,
                        packagesFolder,
                        new PackageDownloadContext(cacheContext),
                        globalPackagesFolder,
                        logger,
                        CancellationToken.None);

                    using (var resultA2 = await result[targetA2].GetResultAsync())
                    using (var resultB2 = await result[targetB2].GetResultAsync())
                    using (var resultC2 = await result[targetC2].GetResultAsync())
                    {
                        // Assert
                        Assert.Equal(3, result.Count);

                        Assert.True(result[targetA2].InPackagesFolder);
                        Assert.Null(result[targetA2].Source);
                        Assert.Equal(targetA2, result[targetA2].Package);
                        Assert.True(result[targetA2].IsComplete);
                        Assert.Equal(targetA2, resultA2.PackageReader.GetIdentity());
                        Assert.NotNull(resultA2.PackageStream);
                        Assert.Equal(DownloadResourceResultStatus.Available, resultA2.Status);

                        Assert.False(result[targetB2].InPackagesFolder);
                        Assert.Equal(source.PackageSource, result[targetB2].Source);
                        Assert.Equal(targetB2, result[targetB2].Package);
                        Assert.True(result[targetB2].IsComplete);
                        Assert.Equal(targetB2, resultB2.PackageReader.GetIdentity());
                        Assert.NotNull(resultB2.PackageStream);
                        Assert.Equal(DownloadResourceResultStatus.Available, resultB2.Status);

                        Assert.False(result[targetC2].InPackagesFolder);
                        Assert.Equal(source.PackageSource, result[targetC2].Source);
                        Assert.Equal(targetC2, result[targetC2].Package);
                        Assert.True(result[targetC2].IsComplete);
                        Assert.Equal(targetC2, resultC2.PackageReader.GetIdentity());
                        Assert.NotNull(resultC2.PackageStream);
                        Assert.Equal(DownloadResourceResultStatus.Available, resultC2.Status);
                    }
                }
            }
        }

        [Fact]
        public async Task PackagePreFetcher_PackageAlreadyExists_NonNormalizedVersionInPackages()
        {
            using (var sourceDir = TestDirectory.Create())
            using (var packagesFolderDir = TestDirectory.Create())
            using (var globalPackagesFolder = TestDirectory.Create())
            {
                // Arrange
                var actions = new List<NuGetProjectAction>();
                var packagesFolder = new FolderNuGetProject(packagesFolderDir);
                var testSettings = NullSettings.Instance;
                var logger = new TestLogger();
                var target = new PackageIdentity("packageA", NuGetVersion.Parse("1.0.0"));
                var targetNonNormalized = new PackageIdentity("packageA", NuGetVersion.Parse("1.0"));
                var source = Repository.Factory.GetVisualStudio(new Configuration.PackageSource(sourceDir.Path));

                // Add package
                AddToPackagesFolder(targetNonNormalized, packagesFolderDir);
                actions.Add(NuGetProjectAction.CreateInstallProjectAction(target, source, packagesFolder));

                AddToSource(targetNonNormalized, sourceDir);

                // Act
                using (var cacheContext = new SourceCacheContext())
                {
                    var result = await PackagePreFetcher.GetPackagesAsync(
                        actions,
                        packagesFolder,
                        new PackageDownloadContext(cacheContext),
                        globalPackagesFolder,
                        logger,
                        CancellationToken.None);

                    using (var downloadResult = await result[target].GetResultAsync())
                    {
                        // Assert
                        Assert.Equal(1, result.Count);
                        Assert.True(result[target].InPackagesFolder);
                        Assert.Null(result[target].Source);
                        Assert.Equal(target, result[target].Package);
                        Assert.True(result[target].IsComplete);
                        Assert.NotNull(downloadResult.PackageStream);
                        Assert.Equal(DownloadResourceResultStatus.Available, downloadResult.Status);
                    }
                }
            }
        }

        [Fact]
        public async Task PackagePreFetcher_PackageAlreadyExists_NonNormalizedVersionInput()
        {
            using (var sourceDir = TestDirectory.Create())
            using (var packagesFolderDir = TestDirectory.Create())
            using (var globalPackagesFolder = TestDirectory.Create())
            {
                // Arrange
                var actions = new List<NuGetProjectAction>();
                var packagesFolder = new FolderNuGetProject(packagesFolderDir);
                var testSettings = NullSettings.Instance;
                var logger = new TestLogger();
                var target = new PackageIdentity("packageA", NuGetVersion.Parse("1.0.0"));
                var targetNonNormalized = new PackageIdentity("packageA", NuGetVersion.Parse("1.0"));
                var source = Repository.Factory.GetVisualStudio(new Configuration.PackageSource(sourceDir.Path));

                // Add package
                AddToPackagesFolder(target, packagesFolderDir);
                actions.Add(NuGetProjectAction.CreateInstallProjectAction(targetNonNormalized, source, packagesFolder));

                AddToSource(target, sourceDir);

                // Act
                using (var cacheContext = new SourceCacheContext())
                {
                    var result = await PackagePreFetcher.GetPackagesAsync(
                        actions,
                        packagesFolder,
                        new PackageDownloadContext(cacheContext),
                        globalPackagesFolder,
                        logger,
                        CancellationToken.None);

                    using (var downloadResult = await result[target].GetResultAsync())
                    {
                        // Assert
                        Assert.Equal(1, result.Count);
                        Assert.True(result[target].InPackagesFolder);
                        Assert.Null(result[target].Source);
                        Assert.Equal(target, result[target].Package);
                        Assert.True(result[target].IsComplete);
                        Assert.NotNull(downloadResult.PackageStream);
                        Assert.Equal(DownloadResourceResultStatus.Available, downloadResult.Status);
                    }
                }
            }
        }

        [Fact]
        public async Task PackagePreFetcher_PackageDoesNotExistsInPackagesFolder()
        {
            using (var sourceDir = TestDirectory.Create())
            using (var packagesFolderDir = TestDirectory.Create())
            using (var globalPackagesFolder = TestDirectory.Create())
            {
                // Arrange
                var actions = new List<NuGetProjectAction>();
                var packagesFolder = new FolderNuGetProject(packagesFolderDir);
                var testSettings = NullSettings.Instance;
                var logger = new TestLogger();
                var target = new PackageIdentity("packageA", NuGetVersion.Parse("1.0.0"));
                var source = Repository.Factory.GetVisualStudio(new Configuration.PackageSource(sourceDir.Path));

                actions.Add(NuGetProjectAction.CreateInstallProjectAction(target, source, packagesFolder));
                AddToSource(target, sourceDir);

                // Act
                using (var cacheContext = new SourceCacheContext())
                {
                    var result = await PackagePreFetcher.GetPackagesAsync(
                        actions,
                        packagesFolder,
                        new PackageDownloadContext(cacheContext),
                        globalPackagesFolder,
                        logger,
                        CancellationToken.None);

                    using (var downloadResult = await result[target].GetResultAsync())
                    {
                        // Assert
                        Assert.Equal(1, result.Count);
                        Assert.False(result[target].InPackagesFolder);
                        Assert.Equal(source.PackageSource, result[target].Source);
                        Assert.Equal(target, result[target].Package);
                        Assert.True(result[target].IsComplete);
                        Assert.Equal(target, downloadResult.PackageReader.GetIdentity());
                        Assert.NotNull(downloadResult.PackageStream);
                        Assert.Equal(DownloadResourceResultStatus.Available, downloadResult.Status);
                    }
                }
            }
        }

        [Fact]
        public async Task PackagePreFetcher_PackageDoesNotExistAnywhere()
        {
            using (var sourceDir = TestDirectory.Create())
            using (var packagesFolderDir = TestDirectory.Create())
            using (var globalPackagesFolder = TestDirectory.Create())
            {
                // Arrange
                var actions = new List<NuGetProjectAction>();
                var packagesFolder = new FolderNuGetProject(packagesFolderDir);
                var testSettings = NullSettings.Instance;
                var logger = new TestLogger();
                var target = new PackageIdentity("packageA", NuGetVersion.Parse("1.0.0"));
                var source = Repository.Factory.GetVisualStudio(new Configuration.PackageSource(sourceDir.Path));

                actions.Add(NuGetProjectAction.CreateInstallProjectAction(target, source, packagesFolder));

                // Act
                using (var cacheContext = new SourceCacheContext())
                {
                    var result = await PackagePreFetcher.GetPackagesAsync(
                        actions,
                        packagesFolder,
                        new PackageDownloadContext(cacheContext),
                        globalPackagesFolder,
                        logger,
                        CancellationToken.None);

                    Exception exception = null;

                    try
                    {
                        using (await result[target].GetResultAsync())
                        {
                        }

                        Assert.True(false);
                    }
                    catch (Exception ex)
                    {
                        exception = ex;
                    }

                    // Assert
                    Assert.StartsWith("Package 'packageA.1.0.0' is not found on source", exception.Message);
                }
            }
        }

        private static void AddToPackagesFolder(PackageIdentity package, string root)
        {
            var dir = Path.Combine(root, $"{package.Id}.{package.Version.ToString()}");
            Directory.CreateDirectory(dir);

            var context = new SimpleTestPackageContext()
            {
                Id = package.Id,
                Version = package.Version.ToString()
            };

            context.AddFile("lib/net45/a.dll");
            SimpleTestPackageUtility.CreateOPCPackage(context, dir);
        }

        private static void AddToSource(PackageIdentity package, string root)
        {
            Directory.CreateDirectory(root);

            var context = new SimpleTestPackageContext()
            {
                Id = package.Id,
                Version = package.Version.ToString()
            };

            context.AddFile("lib/net45/a.dll");
            SimpleTestPackageUtility.CreateOPCPackage(context, root);
        }
    }
}
