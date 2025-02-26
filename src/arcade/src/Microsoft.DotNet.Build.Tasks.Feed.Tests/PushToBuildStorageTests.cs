// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using FluentAssertions;
using Microsoft.Arcade.Common;
using Microsoft.Arcade.Test.Common;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.DotNet.Internal.DependencyInjection.Testing;
using Microsoft.DotNet.VersionTools.Automation;
using Microsoft.DotNet.VersionTools.BuildManifest.Model;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using NuGet.Versioning;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace Microsoft.DotNet.Build.Tasks.Feed.Tests
{
    public class PushToBuildStorageTests
    {
        private static string TARGET_MANIFEST_PATH = Path.Combine("C:", "manifests", "TestManifest.xml");
        private static string PACKAGE_A = Path.Combine("C:", "packages", "test-package-a.6.0.492.nupkg");
        private static string PACKAGE_B = Path.Combine("C:", "packages", "test-package-b.6.0.492.nupkg");
        private static string SAMPLE_MANIFEST = Path.Combine("C:", "manifests", "SampleManifest.xml");
        private const string NUPKG_VERSION = "6.0.492";

        private TaskItem[] TaskItemsWithPublishFlatContainer = new TaskItem[]
        {
            new TaskItem(PACKAGE_A, new Dictionary<string, string>
            {
                { "RelativeBlobPath", PACKAGE_A },
                { "IsShipping", "false" },
                { "ManifestArtifactData", "Nonshipping=true" },
                { "Kind", "Package" }
            }),
            new TaskItem(PACKAGE_B, new Dictionary<string, string>
            {
                { "RelativeBlobPath", PACKAGE_B },
                { "IsShipping", "true" },
                { "ManifestArtifactData", "Nonshipping=false" },
                { "Kind", "Package" }
            }),
            new TaskItem(SAMPLE_MANIFEST, new Dictionary<string, string>
            {
                { "IsShipping", "false" },
                { "RelativeBlobPath", SAMPLE_MANIFEST },
                { "ManifestArtifactData", "Nonshipping=false" },
                { "PublishFlatContainer", "true" },
                { "Kind", "Blob" }
            }),
        };

        private TaskItem[] TaskItemsWithKindAndPublishFlatContainer = new TaskItem[]
        {
            new TaskItem(PACKAGE_A, new Dictionary<string, string>
            {
                { "RelativeBlobPath", PACKAGE_A },
                { "IsShipping", "false" },
                { "ManifestArtifactData", "Nonshipping=true" },
                { "Kind", "Package" },
            }),
            new TaskItem(PACKAGE_B, new Dictionary<string, string>
            {
                { "RelativeBlobPath", PACKAGE_B },
                { "IsShipping", "true" },
                { "ManifestArtifactData", "Nonshipping=false" },
                { "Kind", "Package" }, // Package overrides PublishFlatContainer value
            }),
            new TaskItem(SAMPLE_MANIFEST, new Dictionary<string, string>
            {
                { "IsShipping", "false" },
                { "RelativeBlobPath", SAMPLE_MANIFEST },
                { "ManifestArtifactData", "Nonshipping=false" },
                // Keep PublishFlatContainer to ensure that it does not affect the manifest generation.
                { "PublishFlatContainer", "false" },
                { "Kind", "Blob" },
            }),
        };

        private TaskItem[] TaskItemsWithKind = new TaskItem[]
        {
            new TaskItem(PACKAGE_A, new Dictionary<string, string>
            {
                { "RelativeBlobPath", PACKAGE_A },
                { "IsShipping", "false" },
                { "ManifestArtifactData", "Nonshipping=true" },
                // Should default to package.
            }),
            new TaskItem(PACKAGE_B, new Dictionary<string, string>
            {
                { "RelativeBlobPath", PACKAGE_B },
                { "IsShipping", "true" },
                { "ManifestArtifactData", "Nonshipping=false" },
                { "Kind", "Package" },
            }),
            new TaskItem(SAMPLE_MANIFEST, new Dictionary<string, string>
            {
                { "IsShipping", "false" },
                { "RelativeBlobPath", SAMPLE_MANIFEST },
                { "ManifestArtifactData", "Nonshipping=false" },
                { "Kind", "Blob" },
            }),
        };

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

        private void CreateMockServiceCollection(IServiceCollection collection)
        {
            Mock<IFileSystem> fileSystemMock = new();
            Mock<IBlobArtifactModelFactory> blobArtifactModelFactoryMock = new();
            Mock<IPdbArtifactModelFactory> pdbArtifactModelFactoryMock = new();
            Mock<IPackageArtifactModelFactory> packageArtifactModelFactoryMock = new();
            Mock<IBuildModelFactory> buildModelFactoryMock = new();
            Mock<IPackageArchiveReaderFactory> packageArchiveReaderFactoryMock = new();
            Mock<INupkgInfoFactory> nupkgInfoFactoryMock = new();

            collection.TryAddSingleton(blobArtifactModelFactoryMock.Object);
            collection.TryAddSingleton(pdbArtifactModelFactoryMock.Object);
            collection.TryAddSingleton(packageArtifactModelFactoryMock.Object);
            collection.TryAddSingleton(buildModelFactoryMock.Object);
            collection.TryAddSingleton(fileSystemMock.Object);
            collection.TryAddSingleton(nupkgInfoFactoryMock.Object);
            collection.TryAddSingleton(packageArchiveReaderFactoryMock.Object);
        }

        private string BuildExpectedManifestContent(
            string publishingInfraVersion = null,
            string name = null,
            string branch = null,
            string commit = null,
            bool isReleaseOnlyPackageVersion = false,
            bool isStable = false,
            bool includePackages = false)
        {
            publishingInfraVersion ??= ((int)PublishingInfraVersion.Latest).ToString();

            StringBuilder sb = new StringBuilder();

            sb.Append($"<Build ");
            
            sb.Append($"PublishingVersion=\"{publishingInfraVersion}\" ");
            
            if (!string.IsNullOrEmpty(name))
            {
                sb.Append($"Name=\"{name}\" ");
            }
            
            sb.Append($"BuildId=\"12345.6\" ");
            
            if (!string.IsNullOrEmpty(branch))
            {
                sb.Append($"Branch=\"{branch}\" ");
            }

            if (!string.IsNullOrEmpty(commit))
            {
                sb.Append($"Commit=\"{commit}\" ");
            }

            sb.Append($"InitialAssetsLocation=\"cloud\" ");

            sb.Append($"IsReleaseOnlyPackageVersion=\"{isReleaseOnlyPackageVersion.ToString().ToLower()}\" ");

            sb.Append($"IsStable=\"{isStable.ToString().ToLower()}\"");

            if(!includePackages)
            {
                sb.Append($" />");
            }
            
            if(includePackages)
            {
                sb.Append($">");

                sb.Append($"<Package Id=\"{Path.GetFileNameWithoutExtension(PACKAGE_A)}\" Version=\"{NUPKG_VERSION}\" Nonshipping=\"true\" />");
                sb.Append($"<Package Id=\"{Path.GetFileNameWithoutExtension(PACKAGE_B)}\" Version=\"{NUPKG_VERSION}\" Nonshipping=\"false\" />");
                sb.Append($"<Blob Id=\"{SAMPLE_MANIFEST}\" Nonshipping=\"false\" />");

                sb.Append($"</Build>");
            }

            return sb.ToString();
        }

        [Fact]
        public void HasRecordedPublishingVersion()
        {
            var expectedManifestContent = BuildExpectedManifestContent();
            var task = ConstructPushToBuildStorageTask(setAdditionalData: false, TaskItemsWithPublishFlatContainer);
            task.ItemsToPush = new TaskItem[0];
            task.IsStableBuild = false;

            // Mocks
            Mock<IFileSystem> fileSystemMock = new Mock<IFileSystem>();
            IList<string> actualPath = new List<string>();
            IList<string> actualBuildModel = new List<string>();
            fileSystemMock.Setup(m => m.WriteToFile(Capture.In(actualPath), Capture.In(actualBuildModel))).Verifiable();

            // Dependency Injection setup
            var collection = new ServiceCollection()
                .AddSingleton(fileSystemMock.Object)
                .AddSingleton<IBuildModelFactory, BuildModelFactory>();
            CreateMockServiceCollection(collection);
            task.ConfigureServices(collection);
            using var provider = collection.BuildServiceProvider();

            // Act and Assert
            task.InvokeExecute(provider).Should().BeTrue();
            actualPath[0].Should().Be(TARGET_MANIFEST_PATH);
            actualBuildModel[0].Should().Be(expectedManifestContent);
        }

        [Fact]
        public void UsesCustomPublishingVersion()
        {
            var publishingInfraVersion = "456";
            var expectedManifestContent = BuildExpectedManifestContent(
                publishingInfraVersion: publishingInfraVersion);
            var task = ConstructPushToBuildStorageTask(setAdditionalData: false, taskItems: TaskItemsWithKind);
            task.ItemsToPush = new TaskItem[0];
            task.IsStableBuild = false;
            task.PublishingVersion = publishingInfraVersion;

            // Mocks
            Mock<IFileSystem> fileSystemMock = new Mock<IFileSystem>();
            IList<string> actualPath = new List<string>();
            IList<string> actualBuildModel = new List<string>();
            fileSystemMock.Setup(m => m.WriteToFile(Capture.In(actualPath), Capture.In(actualBuildModel))).Verifiable();

            // Dependency Injection setup
            var collection = new ServiceCollection()
                .AddSingleton(fileSystemMock.Object)
                .AddSingleton<IBuildModelFactory, BuildModelFactory>();
            CreateMockServiceCollection(collection);
            task.ConfigureServices(collection);
            using var provider = collection.BuildServiceProvider();

            // Act and Assert
            task.InvokeExecute(provider).Should().BeTrue();
            actualPath[0].Should().Be(TARGET_MANIFEST_PATH);
            actualBuildModel[0].Should().Be(expectedManifestContent);
        }

        [Fact]
        public void ProducesBasicManifest()
        {
            string expectedManifestContent = BuildExpectedManifestContent(
                name: "https://dnceng@dev.azure.com/dnceng/internal/test-repo",
                branch: "/refs/heads/branch",
                commit: "1234567890abcdef",
                isStable: true,
                includePackages: true);

            PushToBuildStorage task = ConstructPushToBuildStorageTask(setAdditionalData: true, taskItems: TaskItemsWithPublishFlatContainer);

            // Mocks
            Mock<IFileSystem> fileSystemMock = new Mock<IFileSystem>();
            IList<string> actualPath = new List<string>();
            IList<string> actualBuildModel = new List<string>();
            IList<string> files = new List<string> { PACKAGE_A, PACKAGE_B, SAMPLE_MANIFEST };
            fileSystemMock.Setup(m => m.WriteToFile(Capture.In(actualPath), Capture.In(actualBuildModel))).Verifiable();
            fileSystemMock.Setup(m => m.FileExists(Capture.In(files))).Returns(true);

            Mock<INupkgInfoFactory> nupkgInfoFactoryMock = new Mock<INupkgInfoFactory>();
            IList<string> actualNupkgInfoPath = new List<string>();
            nupkgInfoFactoryMock.Setup(m => m.CreateNupkgInfo(PACKAGE_A)).Returns(new NupkgInfo(new PackageIdentity(
                id: Path.GetFileNameWithoutExtension(PACKAGE_A),
                version: NUPKG_VERSION
            )));
            nupkgInfoFactoryMock.Setup(m => m.CreateNupkgInfo(PACKAGE_B)).Returns(new NupkgInfo(new PackageIdentity(
                id: Path.GetFileNameWithoutExtension(PACKAGE_B),
                version: NUPKG_VERSION
            )));

            // Dependency Injection setup
            var collection = new ServiceCollection()
                .AddSingleton(fileSystemMock.Object)
                .AddSingleton<IBuildModelFactory, BuildModelFactory>()
                .AddSingleton<IPackageArtifactModelFactory, PackageArtifactModelFactory>()
                .AddSingleton<IBlobArtifactModelFactory, BlobArtifactModelFactory>()
                .AddSingleton<IPdbArtifactModelFactory, PdbArtifactModelFactory>()
                .AddSingleton(nupkgInfoFactoryMock.Object);
            CreateMockServiceCollection(collection);
            task.ConfigureServices(collection);
            using var provider = collection.BuildServiceProvider();

            // Act and Assert
            task.InvokeExecute(provider).Should().BeTrue();
            actualPath[0].Should().Be(TARGET_MANIFEST_PATH);
            actualBuildModel[0].Should().Be(expectedManifestContent);
        }

        [Fact]
        public void IsNotStableBuildPath()
        {
            PushToBuildStorage task = ConstructPushToBuildStorageTask(setAdditionalData: true, taskItems: TaskItemsWithKindAndPublishFlatContainer);
            task.IsStableBuild = false;

            string expectedManifestContent = BuildExpectedManifestContent(
                name: "https://dnceng@dev.azure.com/dnceng/internal/test-repo",
                branch: "/refs/heads/branch",
                commit: "1234567890abcdef",
                includePackages: true);

            // Mocks
            Mock<IFileSystem> fileSystemMock = new Mock<IFileSystem>();
            IList<string> actualPath = new List<string>();
            IList<string> actualBuildModel = new List<string>();
            IList<string> files = new List<string> { PACKAGE_A, PACKAGE_B, SAMPLE_MANIFEST };
            fileSystemMock.Setup(m => m.WriteToFile(Capture.In(actualPath), Capture.In(actualBuildModel))).Verifiable();
            fileSystemMock.Setup(m => m.FileExists(Capture.In(files))).Returns(true);

            Mock<INupkgInfoFactory> nupkgInfoFactoryMock = new Mock<INupkgInfoFactory>();
            nupkgInfoFactoryMock.Setup(m => m.CreateNupkgInfo(PACKAGE_A)).Returns(new NupkgInfo(new PackageIdentity(
                id: Path.GetFileNameWithoutExtension(PACKAGE_A),
                version: NUPKG_VERSION
            )));
            nupkgInfoFactoryMock.Setup(m => m.CreateNupkgInfo(PACKAGE_B)).Returns(new NupkgInfo(new PackageIdentity(
                id: Path.GetFileNameWithoutExtension(PACKAGE_B),
                version: NUPKG_VERSION
            )));

            // Dependency Injection setup
            var collection = new ServiceCollection()
                .AddSingleton(fileSystemMock.Object)
                .AddSingleton<IBuildModelFactory, BuildModelFactory>()
                .AddSingleton<IPackageArtifactModelFactory, PackageArtifactModelFactory>()
                .AddSingleton<IPdbArtifactModelFactory, PdbArtifactModelFactory>()
                .AddSingleton<IBlobArtifactModelFactory, BlobArtifactModelFactory>()
                .AddSingleton(nupkgInfoFactoryMock.Object);
            CreateMockServiceCollection(collection);
            task.ConfigureServices(collection);
            using var provider = collection.BuildServiceProvider();

            // Act and Assert
            task.InvokeExecute(provider).Should().BeTrue();
            actualPath[0].Should().Be(TARGET_MANIFEST_PATH);
            actualBuildModel[0].Should().Be(expectedManifestContent);
        }

        [Fact]
        public void IsReleaseOnlyPackageVersionPath()
        {
            PushToBuildStorage task = ConstructPushToBuildStorageTask(setAdditionalData: true, taskItems: TaskItemsWithKindAndPublishFlatContainer);
            task.IsReleaseOnlyPackageVersion = true;

            string expectedManifestContent = BuildExpectedManifestContent(
                name: "https://dnceng@dev.azure.com/dnceng/internal/test-repo",
                branch: "/refs/heads/branch",
                commit: "1234567890abcdef",
                isReleaseOnlyPackageVersion: true,
                isStable: true,
                includePackages: true);

            // Mocks
            Mock<IFileSystem> fileSystemMock = new Mock<IFileSystem>();
            IList<string> actualPath = new List<string>();
            IList<string> actualBuildModel = new List<string>();
            IList<string> files = new List<string> { PACKAGE_A, PACKAGE_B, SAMPLE_MANIFEST };
            fileSystemMock.Setup(m => m.WriteToFile(Capture.In(actualPath), Capture.In(actualBuildModel))).Verifiable();
            fileSystemMock.Setup(m => m.FileExists(Capture.In(files))).Returns(true);

            Mock<INupkgInfoFactory> nupkgInfoFactoryMock = new Mock<INupkgInfoFactory>();
            IList<string> actualNupkgInfoPath = new List<string>();
            nupkgInfoFactoryMock.Setup(m => m.CreateNupkgInfo(PACKAGE_A)).Returns(new NupkgInfo(new PackageIdentity(
                id: Path.GetFileNameWithoutExtension(PACKAGE_A),
                version: NUPKG_VERSION
            )));
            nupkgInfoFactoryMock.Setup(m => m.CreateNupkgInfo(PACKAGE_B)).Returns(new NupkgInfo(new PackageIdentity(
                id: Path.GetFileNameWithoutExtension(PACKAGE_B),
                version: NUPKG_VERSION
            )));

            // Dependency Injection setup
            var collection = new ServiceCollection()
                .AddSingleton(fileSystemMock.Object)
                .AddSingleton<IBuildModelFactory, BuildModelFactory>()
                .AddSingleton<IPackageArtifactModelFactory, PackageArtifactModelFactory>()
                .AddSingleton<IBlobArtifactModelFactory, BlobArtifactModelFactory>()
                .AddSingleton<IPdbArtifactModelFactory, PdbArtifactModelFactory>()
                .AddSingleton(nupkgInfoFactoryMock.Object);
            CreateMockServiceCollection(collection);
            task.ConfigureServices(collection);
            using var provider = collection.BuildServiceProvider();

            // Act and Assert
            task.InvokeExecute(provider).Should().BeTrue();
            actualPath[0].Should().Be(TARGET_MANIFEST_PATH);
            actualBuildModel[0].Should().Be(expectedManifestContent);
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
    }
}
