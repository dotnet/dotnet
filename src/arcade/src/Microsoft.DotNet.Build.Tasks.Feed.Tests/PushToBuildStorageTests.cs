// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using FluentAssertions;
using Microsoft.Arcade.Common;
using Microsoft.Arcade.Test.Common;
using Microsoft.Build.Utilities;
using Microsoft.DotNet.Build.Manifest;
using Microsoft.DotNet.Internal.DependencyInjection.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using Xunit;

namespace Microsoft.DotNet.Build.Tasks.Feed.Tests
{
    /// <summary>
    /// Tests for the PushToBuildStorage task. This set of tests is intended to validate the
    /// behavior of the task, how it handles artifacts (copying, writing of manifests, etc.)
    /// and in some cases, some limited manifest content.
    /// 
    /// However, it is not intended to validate the content of the manifest itself. There is
    /// quite a bit of validation of the generation of the manifest from the input items in the
    /// BuildModelFactoryTests.
    /// </summary>
    public class PushToBuildStorageTests
    {
        // Use forward slashes here for compatibility with how the mock file system works.
        private static string TARGET_MANIFEST_PATH = "C:/manifests/TestManifest.xml";
        private static string PACKAGE_A = "C:/packages/test-package-a.6.0.492.nupkg";
        private static string PACKAGE_B = "C:/packages/test-package-b.6.0.492.nupkg";
        private static string BLOB_A = "C:/assets/my/zip/file.zip";
        private static string PDB_A = "C:/pdbs/my/pdb/file.pdb";
        private static string PDB_B = "C:/pdbs/my/otherpdb/file.pdb";
        private const string NUPKG_VERSION = "6.0.492";

        private TaskItem[] DefaultTaskItems =
        [
            new TaskItem(PACKAGE_A, new Dictionary<string, string>
            {
                { "RelativeBlobPath", PACKAGE_A },
                { "IsShipping", "false" },
                { "ManifestArtifactData", "Nonshipping=true" },
                { "Kind", "Package" }
            }),
            new TaskItem(PACKAGE_B, new Dictionary<string, string>
            {
                { "IsShipping", "true" },
                { "ManifestArtifactData", "Nonshipping=false" },
                { "Kind", "Package" }
            }),
            new TaskItem(BLOB_A, new Dictionary<string, string>
            {
                { "RelativeBlobPath", "path/to/blob/file.zip" },
                { "IsShipping", "true" },
                { "ManifestArtifactData", "Nonshipping=false" },
                { "Kind", "Blob" }
            })
        ];

        private PushToBuildStorage ConstructPushToBuildStorageTask(bool setAdditionalData, TaskItem[] taskItems)
        {
            var task = new PushToBuildStorage
            {
                BuildEngine = new MockBuildEngine(),
                AssetManifestPath = TARGET_MANIFEST_PATH,
                IsStableBuild = true,
                IsReleaseOnlyPackageVersion = false,
                ItemsToPush = taskItems,
                ManifestBuildData = new string[] { $"InitialAssetsLocation=cloud" },
                ManifestBuildId = "12345.6"
            };

            if (setAdditionalData)
            {
                task.AzureDevOpsBuildId = 123456;
                task.AzureDevOpsCollectionUri = "https://dev.azure.com/dnceng/";
                task.AzureDevOpsProject = "internal";
                task.ManifestBranch = "/refs/heads/branch";
                task.ManifestCommit = "1234567890abcdef";
                task.ManifestRepoUri = "https://dnceng@dev.azure.com/dnceng/internal/test-repo";
            }

            return task;
        }

        [Fact]
        public void AreDependenciesRegistered()
        {
            PushToBuildStorage task = new PushToBuildStorage();
        
            var collection = new ServiceCollection();
            task.ConfigureServices(collection);
            var provider = collection.BuildServiceProvider();
        
            DependencyInjectionValidation.IsDependencyResolutionCoherent(
                    s =>
                    {
                        task.ConfigureServices(s);
                    },
                    out string message,
                    additionalSingletonTypes: task.GetExecuteParameterTypes()
                )
                .Should()
                .BeTrue(message);
        }

        /// <summary>
        /// Validate that the task fails if the publishing version is invalid.
        /// </summary>
        [Fact]
        public void InvalidPublishingVersion_LogsError()
        {
            // Arrange
            var task = ConstructPushToBuildStorageTask(setAdditionalData: false, taskItems: DefaultTaskItems);
            task.PublishingVersion = 99;

            // Mocks
            Mock<IFileSystem> fileSystemMock = new Mock<IFileSystem>();
            Mock<IBuildModelFactory> buildModelFactoryMock = new Mock<IBuildModelFactory>();

            var collection = new ServiceCollection()
                .AddSingleton(fileSystemMock.Object)
                .AddSingleton(buildModelFactoryMock.Object);

            task.ConfigureServices(collection);
            using var provider = collection.BuildServiceProvider();

            // Act
            task.InvokeExecute(provider).Should().BeFalse();

            // Assert
            task.Log.HasLoggedErrors.Should().BeTrue();

            // Check the error
            MockBuildEngine mockBuildEngine = (MockBuildEngine)task.BuildEngine;
            mockBuildEngine.BuildErrorEvents.Should().ContainSingle(e =>
                e.Message.Contains($"Invalid publishing version '{task.PublishingVersion}'"));
        }

        /// <summary>
        /// Verify that if the various locaL storage directories are not specified,
        /// the appropriate error message is returned and the task fails
        /// </summary>
        [Fact]
        public void V4_PublishingMustRequireAllInputDirs()
        {
            // Arrange
            var task = ConstructPushToBuildStorageTask(setAdditionalData: false, taskItems: DefaultTaskItems);
            task.PublishingVersion = 4;
            var collection = new ServiceCollection();
            task.ConfigureServices(collection);
            using var provider = collection.BuildServiceProvider();
            // Act
            task.InvokeExecute(provider).Should().BeFalse();
            task.Log.HasLoggedErrors.Should().BeTrue();
            // Assert
            MockBuildEngine mockBuildEngine = (MockBuildEngine)task.BuildEngine;
            mockBuildEngine.BuildErrorEvents.Select(e => e.Message).Should().Contain(
                "AssetsLocalStorageDir, ShippingPackagesLocalStorageDir, NonShippingPackagesLocalStorageDir, PdbArtifactsLocalStorageDir and " +
                "AssetManifestsLocalStorageDir need to be specified if PublishToLocalStorage is set to true or V4 publishing is enabled");
        }

        /// <summary>
        /// Validate that V4 publishing generates an expected manifest.
        /// PushToLocalStorage should not matter.
        /// </summary>
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void V4_WithoutPipelineArtifactNamesShouldEmitExpectedManifest(bool pushToLocalStorage)
        {
            MockFileSystem mockFileSystem = CreateMockFileSystemForTaskItems(DefaultTaskItems);

            PushToBuildStorage task = ConstructPushToBuildStorageTask(setAdditionalData: true, taskItems: DefaultTaskItems);
            task.PublishingVersion = 4;
            task.PushToLocalStorage = pushToLocalStorage;
            CreateOutputDirectoriesForV4OrPushLocal(mockFileSystem, task);

            // Dependency Injection setup
            var collection = new ServiceCollection()
                .AddSingleton<IFileSystem>(mockFileSystem)
                .AddSingleton(CreateNupkgInfoFactoryMock(mockFileSystem).Object);
            task.ConfigureServices(collection);
            using var provider = collection.BuildServiceProvider();

            // Act and Assert
            task.InvokeExecute(provider);
            task.Log.HasLoggedErrors.Should().BeFalse();

            // Validate that the manifest shows up in the manifest target location
            var manifestContent = mockFileSystem.Files[TARGET_MANIFEST_PATH];
            var model = LoadModel(manifestContent);

            // Validate a few invariants: IsStable should be false.
            model.Artifacts.Packages.Count.Should().Be(DefaultTaskItems.Count(i => i.GetMetadata("Kind") == "Package"));
            model.Artifacts.Blobs.Count.Should().Be(DefaultTaskItems.Count(i => i.GetMetadata("Kind") == "Blob"));
            model.Artifacts.Pdbs.Count.Should().Be(DefaultTaskItems.Count(i => i.GetMetadata("Kind") == "Pdb"));
        }

        private static void CreateOutputDirectoriesForV4OrPushLocal(MockFileSystem mockFileSystem, PushToBuildStorage task, bool backslashSeparator = false)
        {
            // Output directories for local storage
            string shippingPackagesDir = mockFileSystem.PathCombine("C:", "artifacts", "shipping-packages");
            string nonShippingPackagesDir = mockFileSystem.PathCombine("C:", "artifacts", "nonshipping-packages");
            string assetsDir = mockFileSystem.PathCombine("C:", "artifacts", "blobs");
            string manifestsDir = mockFileSystem.PathCombine("C:", "artifacts", "manifests");
            string pdbsDir = mockFileSystem.PathCombine("C:", "artifacts", "pdbs");

            if (backslashSeparator)
            {
                shippingPackagesDir = shippingPackagesDir.Replace('/', '\\');
                nonShippingPackagesDir = nonShippingPackagesDir.Replace('/', '\\');
                assetsDir = assetsDir.Replace('/', '\\');
                manifestsDir = manifestsDir.Replace('/', '\\');
                pdbsDir = pdbsDir.Replace('/', '\\');
            }

            // Create output directories using the mock file system
            mockFileSystem.CreateDirectory(shippingPackagesDir);
            mockFileSystem.CreateDirectory(nonShippingPackagesDir);
            mockFileSystem.CreateDirectory(assetsDir);
            mockFileSystem.CreateDirectory(manifestsDir);
            mockFileSystem.CreateDirectory(pdbsDir);

            task.PdbArtifactsLocalStorageDir = pdbsDir;
            task.ShippingPackagesLocalStorageDir = shippingPackagesDir;
            task.NonShippingPackagesLocalStorageDir = nonShippingPackagesDir;
            task.AssetsLocalStorageDir = assetsDir;
            task.AssetManifestsLocalStorageDir = manifestsDir;
        }

        [Fact]
        public void V4_WithPipelineArtifactNamesShouldEmitFutureAssetLocationsInManifest()
        {
            MockFileSystem mockFileSystem = CreateMockFileSystemForTaskItems(DefaultTaskItems);

            PushToBuildStorage task = ConstructPushToBuildStorageTask(setAdditionalData: true, taskItems: DefaultTaskItems);
            task.PublishingVersion = 4;
            task.FutureArtifactName = "TestFutureArtifactName";
            task.FutureArtifactPublishBasePath = "C:/artifacts";
            CreateOutputDirectoriesForV4OrPushLocal(mockFileSystem, task);

            // Dependency Injection setup
            var collection = new ServiceCollection()
                .AddSingleton<IFileSystem>(mockFileSystem)
                .AddSingleton(CreateNupkgInfoFactoryMock(mockFileSystem).Object);
            task.ConfigureServices(collection);
            using var provider = collection.BuildServiceProvider();

            // Act and Assert
            task.InvokeExecute(provider);
            task.Log.HasLoggedErrors.Should().BeFalse();

            // Validate that the manifest shows up in the manifest target location
            var manifestContent = mockFileSystem.Files[TARGET_MANIFEST_PATH];
            var model = LoadModel(manifestContent);

            // Validate a few invariants: IsStable should be false.
            model.Artifacts.Packages.Count.Should().Be(DefaultTaskItems.Count(i => i.GetMetadata("Kind") == "Package"));
            model.Artifacts.Blobs.Count.Should().Be(DefaultTaskItems.Count(i => i.GetMetadata("Kind") == "Blob"));
            model.Artifacts.Pdbs.Count.Should().Be(DefaultTaskItems.Count(i => i.GetMetadata("Kind") == "Pdb"));

            // Validate that the future artifact name is set correctly
            model.Artifacts.Packages[0].PipelineArtifactName.Should().Be("TestFutureArtifactName");
            model.Artifacts.Packages[0].PipelineArtifactPath.Should().Be("nonshipping-packages/test-package-a.6.0.492.nupkg");
            model.Artifacts.Packages[1].PipelineArtifactName.Should().Be("TestFutureArtifactName");
            model.Artifacts.Packages[1].PipelineArtifactPath.Should().Be("shipping-packages/test-package-b.6.0.492.nupkg");
            // Note that blobs are copied to the output directory based on their relative blob path,
            // rather than the original blob path
            model.Artifacts.Blobs[0].PipelineArtifactName.Should().Be("TestFutureArtifactName");
            model.Artifacts.Blobs[0].PipelineArtifactPath.Should().Be("blobs/path/to/blob/file.zip");
        }

        [Fact]
        public void V4_FuturePathsShouldOnlyUseForwardSlash()
        {
            var mockFileSystem = new MockFileSystem(directorySeparator: @"\");
            mockFileSystem.Files[PDB_A] = nameof(PDB_A);
            mockFileSystem.Files[PDB_B] = nameof(PDB_B);

            var taskItems = new TaskItem[]
            {
                new TaskItem(PDB_A, new Dictionary<string, string>
                {
                    { "RelativePdbPath", @"path\to\a\pdb.pdb"},
                    { "Kind", "Pdb" }
                }),
                new TaskItem(PDB_B, new Dictionary<string, string>
                {
                    { "RelativePdbPath", @"path\to\b\pdb.pdb" },
                    { "Kind", "Pdb" }
                }),
            };

            PushToBuildStorage task = ConstructPushToBuildStorageTask(setAdditionalData: true, taskItems: taskItems);
            task.PublishingVersion = 4;
            task.FutureArtifactName = "TestFutureArtifactName";
            task.FutureArtifactPublishBasePath = @"C:\artifacts";
            CreateOutputDirectoriesForV4OrPushLocal(mockFileSystem, task, backslashSeparator: true);

            // Dependency Injection setup
            var collection = new ServiceCollection()
                .AddSingleton<IFileSystem>(mockFileSystem);
            task.ConfigureServices(collection);
            using var provider = collection.BuildServiceProvider();

            // Act and Assert
            task.InvokeExecute(provider);
            task.Log.HasLoggedErrors.Should().BeFalse();

            // Validate that the manifest shows up in the manifest target location
            var manifestContent = mockFileSystem.Files[TARGET_MANIFEST_PATH];
            var model = LoadModel(manifestContent);

            // Validate a few invariants: IsStable should be false.
            model.Artifacts.Pdbs.Count.Should().Be(2);

            // Validate that the future artifact name is set correctly
            model.Artifacts.Pdbs[0].PipelineArtifactName.Should().Be("TestFutureArtifactName");
            model.Artifacts.Pdbs[0].PipelineArtifactPath.Should().Be("pdbs/path/to/a/pdb.pdb");
            model.Artifacts.Pdbs[1].PipelineArtifactName.Should().Be("TestFutureArtifactName");
            model.Artifacts.Pdbs[1].PipelineArtifactPath.Should().Be("pdbs/path/to/b/pdb.pdb");

            // Validate that we didn't upload PDBs via the VSO upload command
            var mockBuildEngine = (MockBuildEngine)task.BuildEngine;
            mockBuildEngine.BuildMessageEvents.Select(m => m.Message.Should().NotContain("##vso[artifact.upload containerfolder=PdbArtifacts;artifactname=PdbArtifacts"));
        }

        [Fact]
        public void V4_FuturePathsNotRelativeToAssetDirectoryShouldThrowError()
        {
            var mockFileSystem = new MockFileSystem();
            mockFileSystem.Files[PDB_A] = nameof(PDB_A);
            mockFileSystem.Files[PDB_B] = nameof(PDB_B);

            var taskItems = new TaskItem[]
            {
                new TaskItem(PDB_A, new Dictionary<string, string>
                {
                    { "RelativePdbPath", @"path/to/a/pdb.pdb"},
                    { "Kind", "Pdb" }
                }),
                new TaskItem(PDB_B, new Dictionary<string, string>
                {
                    { "RelativePdbPath", @"path/to/b/pdb.pdb" },
                    { "Kind", "Pdb" }
                }),
            };

            PushToBuildStorage task = ConstructPushToBuildStorageTask(setAdditionalData: true, taskItems: taskItems);
            task.PublishingVersion = 4;
            task.FutureArtifactName = "TestFutureArtifactName";
            task.FutureArtifactPublishBasePath = @"C:/artifacts2";
            CreateOutputDirectoriesForV4OrPushLocal(mockFileSystem, task);

            // Dependency Injection setup
            var collection = new ServiceCollection()
                .AddSingleton<IFileSystem>(mockFileSystem);
            task.ConfigureServices(collection);
            using var provider = collection.BuildServiceProvider();

            // Act and Assert
            task.InvokeExecute(provider);
            task.Log.HasLoggedErrors.Should().BeTrue();

            var engine = (MockBuildEngine)task.BuildEngine;
            engine.BuildErrorEvents.Select(m => m.Message).Should().Contain([
                $"Could not determine relative path from '{task.FutureArtifactPublishBasePath}' to 'C:/artifacts/pdbs/path/to/a/pdb.pdb'.",
                $"Could not determine relative path from '{task.FutureArtifactPublishBasePath}' to 'C:/artifacts/pdbs/path/to/b/pdb.pdb'."]);
        }

        /// <summary>
        /// Verify that when generating a manifest, the value IsStable or
        /// IsReleaseOnlyPackageVersion does not influence the value on the output manifest, which
        /// should be false.
        /// </summary>
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void V4_IsStableBuildAndIsReleaseOnlyPackageVersionShouldBeIgnored(bool stableOrROSpecified)
        {
            MockFileSystem mockFileSystem = CreateMockFileSystemForTaskItems(DefaultTaskItems);

            // Arrange
            var task = ConstructPushToBuildStorageTask(setAdditionalData: true, taskItems: DefaultTaskItems);
            task.PublishingVersion = 4;
            task.IsStableBuild = stableOrROSpecified;
            task.IsReleaseOnlyPackageVersion = stableOrROSpecified;

            CreateOutputDirectoriesForV4OrPushLocal(mockFileSystem, task);

            // Dependency Injection setup
            var collection = new ServiceCollection()
                .AddSingleton<IFileSystem>(mockFileSystem)
                .AddSingleton(CreateNupkgInfoFactoryMock(mockFileSystem).Object);
            task.ConfigureServices(collection);
            using var provider = collection.BuildServiceProvider();

            // Act
            task.InvokeExecute(provider);
            task.Log.HasLoggedErrors.Should().BeFalse();

            // Load the manifest using the manifest APIs, and ensure that IsStable and 
            // IsReleaseOnlyPackageVersion are set to false
            var manifestContent = mockFileSystem.Files[TARGET_MANIFEST_PATH];

            var reader = new StringReader(manifestContent);
            BuildModel model = BuildModel.Parse(XElement.Load(reader));
            model.Identity.IsStable.Should().BeFalse();
            model.Identity.IsReleaseOnlyPackageVersion.Should().BeFalse();
        }

        /// <summary>
        /// Creates a nupkg info mock for all of the well known nupkgs used in these tests
        /// </summary>
        /// <param name="mockFileSystem"></param>
        /// <returns></returns>
        private static Mock<INupkgInfoFactory> CreateNupkgInfoFactoryMock(MockFileSystem mockFileSystem)
        {
            Mock<INupkgInfoFactory> nupkgInfoFactoryMock = new Mock<INupkgInfoFactory>(MockBehavior.Strict);
            nupkgInfoFactoryMock.Setup(m => m.CreateNupkgInfo(PACKAGE_A)).Returns(new NupkgInfo(new PackageIdentity(
                id: mockFileSystem.GetFileNameWithoutExtension(PACKAGE_A),
                version: NUPKG_VERSION
            )));
            nupkgInfoFactoryMock.Setup(m => m.CreateNupkgInfo(PACKAGE_B)).Returns(new NupkgInfo(new PackageIdentity(
                id: mockFileSystem.GetFileNameWithoutExtension(PACKAGE_B),
                version: NUPKG_VERSION
            )));
            return nupkgInfoFactoryMock;
        }

        /// <summary>
        /// Validates that when using V3 publishing with PushToLocalStorage enabled:
        /// - Artifacts (packages, blobs, and PDBs) are copied to the correct local storage directories.
        /// - No upload commands (e.g., VSO artifact upload commands) are logged.
        /// 
        /// This test ensures that the task correctly handles local storage scenarios
        /// without attempting to upload artifacts to Azure DevOps.
        /// </summary>
        [Fact]
        public void V3_WithPushToLocalStorageShouldCopyButNotUpload()
        {
            // Arrange
            var mockFileSystem = new MockFileSystem();

            // Set up files in the "source" locations
            mockFileSystem.Files[PACKAGE_A] = "PackageA content";
            mockFileSystem.Files[PACKAGE_B] = "PackageB content";
            mockFileSystem.Files[BLOB_A] = "BlobA content";
            mockFileSystem.Files[PDB_A] = "PdbA content"; // Include a PDB for publishing

            // Output directories for local storage
            string shippingPackagesDir = mockFileSystem.PathCombine("C:", "artifacts", "shipping-packages");
            string nonShippingPackagesDir = mockFileSystem.PathCombine("C:", "artifacts", "nonshipping-packages");
            string assetsDir = mockFileSystem.PathCombine("C:", "artifacts", "blobs");
            string manifestsDir = mockFileSystem.PathCombine("C:", "artifacts", "manifests");
            string pdbsDir = mockFileSystem.PathCombine("C:", "artifacts", "pdbs");


            // Create output directories using the mock file system
            mockFileSystem.CreateDirectory(shippingPackagesDir);
            mockFileSystem.CreateDirectory(nonShippingPackagesDir);
            mockFileSystem.CreateDirectory(assetsDir);
            mockFileSystem.CreateDirectory(manifestsDir);
            mockFileSystem.CreateDirectory(pdbsDir);

            var taskItems = new TaskItem[]
            {
                new TaskItem(PACKAGE_A, new Dictionary<string, string>
                {
                    { "ManifestArtifactData", "NonShipping=true" },
                    { "Kind", "Package" }
                }),
                new TaskItem(PACKAGE_B, new Dictionary<string, string>
                {
                    { "Kind", "Package" }
                }),
                new TaskItem(BLOB_A, new Dictionary<string, string>
                {
                    { "RelativeBlobPath", "assets/myrepo/file.zip" },
                    { "IsShipping", "true" },
                    { "Kind", "Blob" }
                }),
                new TaskItem(PDB_A, new Dictionary<string, string>
                {
                    { "RelativePdbPath", "pdbs/my/pdb/file.pdb" },
                    { "Kind", "Pdb" }
                }),
            };

            var task = ConstructPushToBuildStorageTask(setAdditionalData: true, taskItems: taskItems);
            task.PublishingVersion = 3;
            task.PushToLocalStorage = true;
            task.ShippingPackagesLocalStorageDir = shippingPackagesDir;
            task.NonShippingPackagesLocalStorageDir = nonShippingPackagesDir;
            task.AssetsLocalStorageDir = assetsDir;
            task.AssetManifestsLocalStorageDir = manifestsDir;
            task.PdbArtifactsLocalStorageDir = pdbsDir;

            // Mocks
            Mock<INupkgInfoFactory> nupkgInfoFactoryMock = new Mock<INupkgInfoFactory>(MockBehavior.Strict);
            nupkgInfoFactoryMock.Setup(m => m.CreateNupkgInfo(PACKAGE_A)).Returns(new NupkgInfo(new PackageIdentity(
                id: mockFileSystem.GetFileNameWithoutExtension(PACKAGE_A),
                version: NUPKG_VERSION
            )));
            nupkgInfoFactoryMock.Setup(m => m.CreateNupkgInfo(PACKAGE_B)).Returns(new NupkgInfo(new PackageIdentity(
                id: mockFileSystem.GetFileNameWithoutExtension(PACKAGE_B),
                version: NUPKG_VERSION
            )));

            // Dependency Injection setup
            var collection = new ServiceCollection()
                .AddSingleton<IFileSystem>(mockFileSystem)
                .AddSingleton(nupkgInfoFactoryMock.Object);

            task.ConfigureServices(collection);
            using var provider = collection.BuildServiceProvider();

            // Act
            bool result = task.InvokeExecute(provider);

            // Assert
            result.Should().BeTrue();
            task.Log.HasLoggedErrors.Should().BeFalse();

            // Check that files were copied to the correct locations
            string expectedNonShippingPackagePath = mockFileSystem.PathCombine(nonShippingPackagesDir, mockFileSystem.GetFileName(PACKAGE_A));
            string expectedShippingPackagePath = mockFileSystem.PathCombine(shippingPackagesDir, mockFileSystem.GetFileName(PACKAGE_B));
            string expectedBlobPath = mockFileSystem.PathCombine(assetsDir, "assets/myrepo/file.zip");
            string expectedPdbPath = mockFileSystem.PathCombine(pdbsDir, "pdbs/my/pdb/file.pdb");
            string expectedManifestPath = TARGET_MANIFEST_PATH;

            mockFileSystem.Files.Should().ContainKey(expectedNonShippingPackagePath);
            mockFileSystem.Files.Should().ContainKey(expectedShippingPackagePath);
            mockFileSystem.Files.Should().ContainKey(expectedBlobPath);
            mockFileSystem.Files.Should().ContainKey(expectedPdbPath);
            mockFileSystem.Files.Should().ContainKey(expectedManifestPath);

            // Verify content was copied correctly
            mockFileSystem.Files[expectedNonShippingPackagePath].Should().Be("PackageA content");
            mockFileSystem.Files[expectedShippingPackagePath].Should().Be("PackageB content");
            mockFileSystem.Files[expectedBlobPath].Should().Be("BlobA content");
            mockFileSystem.Files[expectedPdbPath].Should().Be("PdbA content");

            // Verify that no VSO upload commands were logged
            MockBuildEngine mockBuildEngine = (MockBuildEngine)task.BuildEngine;
            mockBuildEngine.BuildMessageEvents.Should().NotContain(e =>
                e.Message.Contains("##vso[artifact.upload"));
        }

        /// <summary>
        /// Packages pushed with PreserveRepoOrigin should get the repo origin
        /// added to the shipping/nonshipping path, but not blobs or pdbs.
        /// </summary>
        [Theory]
        [InlineData(3)]
        [InlineData(4)]
        public void V3AndV4_PreserveRepoOriginShouldAddRepoToPackageLocations(int publishingVersion)
        {
            // Arrange
            var mockFileSystem = new MockFileSystem();

            // Set up files in the "source" locations
            mockFileSystem.Files[PACKAGE_A] = "PackageA content";
            mockFileSystem.Files[PACKAGE_B] = "PackageB content";
            mockFileSystem.Files[BLOB_A] = "BlobA content";
            mockFileSystem.Files[PDB_A] = "PdbA content"; // Include a PDB for publishing

            // Output directories for local storage
            string shippingPackagesDir = mockFileSystem.PathCombine("C:", "artifacts", "shipping-packages");
            string nonShippingPackagesDir = mockFileSystem.PathCombine("C:", "artifacts", "nonshipping-packages");
            string assetsDir = mockFileSystem.PathCombine("C:", "artifacts", "blobs");
            string manifestsDir = mockFileSystem.PathCombine("C:", "artifacts", "manifests");
            string pdbsDir = mockFileSystem.PathCombine("C:", "artifacts", "pdbs");

            // Create output directories using the mock file system
            mockFileSystem.CreateDirectory(shippingPackagesDir);
            mockFileSystem.CreateDirectory(nonShippingPackagesDir);
            mockFileSystem.CreateDirectory(assetsDir);
            mockFileSystem.CreateDirectory(manifestsDir);
            mockFileSystem.CreateDirectory(pdbsDir);

            var taskItems = new TaskItem[]
            {
                new TaskItem(PACKAGE_A, new Dictionary<string, string>
                {
                    { "ManifestArtifactData", "NonShipping=true" },
                    { "Kind", "Package" }
                }),
                new TaskItem(PACKAGE_B, new Dictionary<string, string>
                {
                    { "Kind", "Package" }
                }),
                new TaskItem(BLOB_A, new Dictionary<string, string>
                {
                    { "RelativeBlobPath", "assets/myrepo/file.zip" },
                    { "IsShipping", "true" },
                    { "Kind", "Blob" }
                }),
                new TaskItem(PDB_A, new Dictionary<string, string>
                {
                    { "RelativePdbPath", "pdbs/my/pdb/file.pdb" },
                    { "Kind", "Pdb" }
                }),
            };

            var task = ConstructPushToBuildStorageTask(setAdditionalData: true, taskItems: taskItems);
            task.PublishingVersion = publishingVersion;
            task.PushToLocalStorage = true;
            task.ShippingPackagesLocalStorageDir = shippingPackagesDir;
            task.NonShippingPackagesLocalStorageDir = nonShippingPackagesDir;
            task.AssetsLocalStorageDir = assetsDir;
            task.AssetManifestsLocalStorageDir = manifestsDir;
            task.PdbArtifactsLocalStorageDir = pdbsDir;
            task.ManifestRepoOrigin = "TestRepoOrigin";
            task.PreserveRepoOrigin = true;

            // Mocks
            Mock<INupkgInfoFactory> nupkgInfoFactoryMock = new Mock<INupkgInfoFactory>(MockBehavior.Strict);
            nupkgInfoFactoryMock.Setup(m => m.CreateNupkgInfo(PACKAGE_A)).Returns(new NupkgInfo(new PackageIdentity(
                id: mockFileSystem.GetFileNameWithoutExtension(PACKAGE_A),
                version: NUPKG_VERSION
            )));
            nupkgInfoFactoryMock.Setup(m => m.CreateNupkgInfo(PACKAGE_B)).Returns(new NupkgInfo(new PackageIdentity(
                id: mockFileSystem.GetFileNameWithoutExtension(PACKAGE_B),
                version: NUPKG_VERSION
            )));

            // Dependency Injection setup
            var collection = new ServiceCollection()
                .AddSingleton<IFileSystem>(mockFileSystem)
                .AddSingleton(nupkgInfoFactoryMock.Object);

            task.ConfigureServices(collection);
            using var provider = collection.BuildServiceProvider();

            // Act
            bool result = task.InvokeExecute(provider);

            // Assert
            result.Should().BeTrue();
            task.Log.HasLoggedErrors.Should().BeFalse();

            // Check that files were copied to the correct locations
            string expectedNonShippingPackagePath = mockFileSystem.PathCombine(nonShippingPackagesDir, task.ManifestRepoOrigin, mockFileSystem.GetFileName(PACKAGE_A));
            string expectedShippingPackagePath = mockFileSystem.PathCombine(shippingPackagesDir, task.ManifestRepoOrigin, mockFileSystem.GetFileName(PACKAGE_B));
            string expectedBlobPath = mockFileSystem.PathCombine(assetsDir, "assets/myrepo/file.zip");
            string expectedPdbPath = mockFileSystem.PathCombine(pdbsDir, "pdbs/my/pdb/file.pdb");
            string expectedManifestPath = TARGET_MANIFEST_PATH;

            mockFileSystem.Files.Should().ContainKey(expectedNonShippingPackagePath);
            mockFileSystem.Files.Should().ContainKey(expectedShippingPackagePath);
            mockFileSystem.Files.Should().ContainKey(expectedBlobPath);
            mockFileSystem.Files.Should().ContainKey(expectedPdbPath);
            mockFileSystem.Files.Should().ContainKey(expectedManifestPath);

            // Verify content was copied correctly
            mockFileSystem.Files[expectedNonShippingPackagePath].Should().Be("PackageA content");
            mockFileSystem.Files[expectedShippingPackagePath].Should().Be("PackageB content");
            mockFileSystem.Files[expectedBlobPath].Should().Be("BlobA content");
            mockFileSystem.Files[expectedPdbPath].Should().Be("PdbA content");
        }

        /// <summary>
        /// Validate that pushing a series of blobs and packages,
        /// emits the correct logging commands. Does not validate the manifest content.
        /// 
        /// This also validates that a V3 push without PDBs does not require a PDB path.
        /// </summary>
        [Fact]
        public void V3_PushToBuildStorageUsesCorrectLoggingCommands()
        {
            PushToBuildStorage task = ConstructPushToBuildStorageTask(setAdditionalData: true, taskItems: DefaultTaskItems);
            task.PublishingVersion = 3;
            MockFileSystem mockFileSystem = CreateMockFileSystemForTaskItems(DefaultTaskItems);

            // Dependency Injection setup
            var collection = new ServiceCollection()
                .AddSingleton<IFileSystem>(mockFileSystem)
                .AddSingleton(CreateNupkgInfoFactoryMock(mockFileSystem).Object);
            task.ConfigureServices(collection);
            using var provider = collection.BuildServiceProvider();

            // Act
            task.InvokeExecute(provider);
            task.Log.HasLoggedErrors.Should().BeFalse();

            // Assert
            MockBuildEngine mockBuildEngine = (MockBuildEngine)task.BuildEngine;
            mockBuildEngine.BuildMessageEvents.Select(m => m.Message).Should().Contain([
                $"##vso[artifact.upload containerfolder=PackageArtifacts;artifactname=PackageArtifacts]{PACKAGE_A}",
                $"##vso[artifact.upload containerfolder=BlobArtifacts;artifactname=BlobArtifacts]{BLOB_A}",
                $"##vso[artifact.upload containerfolder=PackageArtifacts;artifactname=PackageArtifacts]{PACKAGE_B}",
                $"##vso[artifact.upload containerfolder=AssetManifests;artifactname=AssetManifests]{TARGET_MANIFEST_PATH}"]);
        }

        [Theory]
        [InlineData(true, false)]
        [InlineData(false, true)]
        [InlineData(false, false)]
        [InlineData(true, true)]
        public void V3_BasicManifestGeneration(bool isStable, bool isReleaseOnlyPackageVersion)
        {
            PushToBuildStorage task = ConstructPushToBuildStorageTask(setAdditionalData: true, taskItems: DefaultTaskItems);
            task.IsStableBuild = isStable;
            task.IsReleaseOnlyPackageVersion = isReleaseOnlyPackageVersion;
            MockFileSystem mockFileSystem = CreateMockFileSystemForTaskItems(DefaultTaskItems);

            // Dependency Injection setup
            var collection = new ServiceCollection()
                .AddSingleton<IFileSystem>(mockFileSystem)
                .AddSingleton(CreateNupkgInfoFactoryMock(mockFileSystem).Object);
            task.ConfigureServices(collection);
            using var provider = collection.BuildServiceProvider();

            // Act and Assert
            task.InvokeExecute(provider);
            task.Log.HasLoggedErrors.Should().BeFalse();

            // Validate that the manifest shows up in the manifest target location
            var manifestContent = mockFileSystem.Files[TARGET_MANIFEST_PATH];
            var model = LoadModel(manifestContent);

            // Validate a few invariants: IsStable should be false.
            model.Artifacts.Packages.Count.Should().Be(DefaultTaskItems.Count(i => i.GetMetadata("Kind") == "Package"));
            model.Artifacts.Blobs.Count.Should().Be(DefaultTaskItems.Count(i => i.GetMetadata("Kind") == "Blob"));
            model.Artifacts.Pdbs.Count.Should().Be(DefaultTaskItems.Count(i => i.GetMetadata("Kind") == "Pdb"));
            model.Identity.IsStable.Should().Be(isStable);
            model.Identity.IsReleaseOnlyPackageVersion.Should().Be(isReleaseOnlyPackageVersion);
        }

        private static MockFileSystem CreateMockFileSystemForTaskItems(TaskItem[] taskItems)
        {
            var mockFileSystem = new MockFileSystem();
            foreach (var item in taskItems)
            {
                mockFileSystem.Files[item.ItemSpec] = $"Content of {mockFileSystem.GetFileName(item.ItemSpec)}";
            }
            return mockFileSystem;
        }

        private static BuildModel LoadModel(string manifestContent)
        {
            var reader = new StringReader(manifestContent);
            return BuildModel.Parse(XElement.Load(reader));
        }

        /// <summary>
        /// Validate that when PDBs are pushed without a PDB path specified,
        /// we get a failure.
        /// </summary>
        [Fact]
        public void V3_PublishingWithPDBsFailsWithoutPDBPath()
        {
            // Arrange
            var taskItems = new TaskItem[]
            {
                new TaskItem(PDB_A, new Dictionary<string, string>
                {
                    { "RelativePdbPath", "path/to/pdb/in/artifacts/file.pdb" },
                    { "Kind", "Pdb" }
                })
            };

            var task = ConstructPushToBuildStorageTask(setAdditionalData: false, taskItems: taskItems);
            task.PublishingVersion = 3;

            var collection = new ServiceCollection();
            task.ConfigureServices(collection);
            using var provider = collection.BuildServiceProvider();

            // Act
            task.InvokeExecute(provider).Should().BeFalse();
            task.Log.HasLoggedErrors.Should().BeTrue();

            // Assert
            MockBuildEngine mockBuildEngine = (MockBuildEngine)task.BuildEngine;
            mockBuildEngine.BuildErrorEvents.Select(e => e.Message).Should().Contain([
                $"PdbArtifactsLocalStorageDir must be specified if PDBs are present."]);
        }

        /// <summary>
        /// Validate that when PDBs are pushed, we see a single PDB
        /// logging command to upload a temp folder containing all PDBs.
        /// </summary>
        [Fact]
        public void V3_PublishingWithPdbsRequiresAPDBPathAndUploadsWholeFolder()
        {
            // Arrange
            const string pdbARelativePath = "path/to/pdb/in/artifacts/file.pdb";
            const string pdbBRelativePath = "path/to/other/pdb/in/artifacts/file.pdb";

            var mockFileSystem = new MockFileSystem();
            mockFileSystem.Files[PDB_A] = nameof(PDB_A);
            mockFileSystem.Files[PDB_B] = nameof(PDB_B);

            var taskItems = new TaskItem[]
            {
                new TaskItem(PDB_A, new Dictionary<string, string>
                {
                    { "RelativePdbPath", pdbARelativePath },
                    { "Kind", "Pdb" }
                }),
                new TaskItem(PDB_B, new Dictionary<string, string>
                {
                    { "RelativePdbPath", pdbBRelativePath },
                    { "Kind", "Pdb" }
                }),
            };

            var task = ConstructPushToBuildStorageTask(setAdditionalData: false, taskItems: taskItems);
            task.PublishingVersion = 3;
            task.PdbArtifactsLocalStorageDir = "local/storage/directory";

            var collection = new ServiceCollection();
            collection.TryAddSingleton<IFileSystem>(mockFileSystem);
            task.ConfigureServices(collection);
            using var provider = collection.BuildServiceProvider();

            // Act
            task.InvokeExecute(provider);

            // Assert
            task.Log.HasLoggedErrors.Should().BeFalse();

            // Check that the PDBs were copied to the correct location.
            var expectedPdbALocation = mockFileSystem.PathCombine(task.PdbArtifactsLocalStorageDir, pdbARelativePath);
            var expectedPdbBLocation = mockFileSystem.PathCombine(task.PdbArtifactsLocalStorageDir, pdbBRelativePath);
            mockFileSystem.Files.Should().Contain(expectedPdbALocation, nameof(PDB_A));
            mockFileSystem.Files.Should().Contain(expectedPdbBLocation, nameof(PDB_B));

            MockBuildEngine mockBuildEngine = (MockBuildEngine)task.BuildEngine;
            mockBuildEngine.BuildMessageEvents.Select(e => e.Message).Should().Contain([
                $"##vso[artifact.upload containerfolder=PdbArtifacts;artifactname=PdbArtifacts]{task.PdbArtifactsLocalStorageDir}"]);
        }
    }
}
