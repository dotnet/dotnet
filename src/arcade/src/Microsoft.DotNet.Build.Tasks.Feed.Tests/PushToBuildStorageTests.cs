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

        private TaskItem[] TaskItems = new TaskItem[]
        {
            new TaskItem(PACKAGE_A, new Dictionary<string, string>
            {
                { "RelativeBlobPath", PACKAGE_A },
                { "IsShipping", "false" },
                { "ManifestArtifactData", "Nonshipping=true" },
            }),
            new TaskItem(PACKAGE_B, new Dictionary<string, string>
            {
                { "RelativeBlobPath", PACKAGE_B },
                { "IsShipping", "true" },
                { "ManifestArtifactData", "Nonshipping=false" },
            }),
            new TaskItem(SAMPLE_MANIFEST, new Dictionary<string, string>
            {
                { "IsShipping", "false" },
                { "RelativeBlobPath", SAMPLE_MANIFEST },
                { "ManifestArtifactData", "Nonshipping=false" },
                { "PublishFlatContainer", "true" },
            }),
        };

        private PushToBuildStorage ConstructPushToBuildStorageTask(bool setAdditionalData = true)
        {
            var task = new PushToBuildStorage
            {
                BuildEngine = new MockBuildEngine(),
                AssetManifestPath = TARGET_MANIFEST_PATH,                
                IsStableBuild = true,
                IsReleaseOnlyPackageVersion = false,
                ItemsToPush = TaskItems,
                ManifestBuildData = new string[] { $"InitialAssetsLocation=cloud" },
                ManifestBuildId = "12345.6"
            };

            if(setAdditionalData)
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
            Mock<ISigningInformationModelFactory> signingInformationModelFactoryMock = new();
            Mock<IBlobArtifactModelFactory> blobArtifactModelFactoryMock = new();
            Mock<IPackageArtifactModelFactory> packageArtifactModelFactoryMock = new();
            Mock<IBuildModelFactory> buildModelFactoryMock = new();
            Mock<IPackageArchiveReaderFactory> packageArchiveReaderFactoryMock = new();
            Mock<INupkgInfoFactory> nupkgInfoFactoryMock = new();

            collection.TryAddSingleton(signingInformationModelFactoryMock.Object);
            collection.TryAddSingleton(blobArtifactModelFactoryMock.Object);
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
            bool includePackages = false,
            bool publishFlatContainer = false,
            bool includeSigningInfo = false)
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

            if(!includePackages && !publishFlatContainer && !includeSigningInfo)
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

            if(publishFlatContainer)
            {
                sb.Append($">");

                sb.Append($"<Blob Id=\"{SAMPLE_MANIFEST}\" Nonshipping=\"false\" />");
                sb.Append($"<Blob Id=\"{PACKAGE_A}\" Nonshipping=\"true\" />");
                sb.Append($"<Blob Id=\"{PACKAGE_B}\" Nonshipping=\"false\" />");
                
                sb.Append($"</Build>");
            }

            if (includeSigningInfo)
            {
                sb.Append($">");

                sb.Append($"<Package Id=\"test-package-a\" Version=\"{NUPKG_VERSION}\" Nonshipping=\"true\" />");
                sb.Append($"<Package Id=\"test-package-b\" Version=\"{NUPKG_VERSION}\" Nonshipping=\"false\" />");
                sb.Append($"<Blob Id=\"{SAMPLE_MANIFEST}\" Nonshipping=\"false\" />");

                sb.Append($"<SigningInformation>");
                
                sb.Append($"<FileExtensionSignInfo Include=\".dll\" CertificateName=\"TestSigningCert\" />");
                sb.Append($"<FileExtensionSignInfo Include=\".nupkg\" CertificateName=\"TestNupkg\" />");
                sb.Append($"<FileExtensionSignInfo Include=\".zip\" CertificateName=\"None\" />");
                sb.Append($"<FileSignInfo Include=\"Best.dll\" CertificateName=\"BestCert\" />");
                sb.Append($"<FileSignInfo Include=\"Worst.dll\" CertificateName=\"WorstCert\" />");
                sb.Append($"<CertificatesSignInfo Include=\"BestCert\" DualSigningAllowed=\"true\" />");
                sb.Append($"<CertificatesSignInfo Include=\"WorstCert\" DualSigningAllowed=\"false\" />");
                sb.Append($"<ItemsToSign Include=\"test-package-a.6.0.492.nupkg\" />");
                sb.Append($"<ItemsToSign Include=\"test-package-b.6.0.492.nupkg\" />");
                sb.Append($"<StrongNameSignInfo Include=\"VeryCoolStrongName\" PublicKeyToken=\"123456789ABCDEF0\" CertificateName=\"BestCert\" />");
                sb.Append($"<StrongNameSignInfo Include=\"VeryTrashStrongName\" PublicKeyToken=\"00FEDCBA98765432\" CertificateName=\"WorstCert\" />");
                
                sb.Append($"</SigningInformation>");

                sb.Append($"</Build>");
            }

            return sb.ToString();
        }

        [Fact]
        public void HasRecordedPublishingVersion()
        {
            var expectedManifestContent = BuildExpectedManifestContent();
            var task = ConstructPushToBuildStorageTask(setAdditionalData: false);
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
            var task = ConstructPushToBuildStorageTask(setAdditionalData: false);
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

            PushToBuildStorage task = ConstructPushToBuildStorageTask();

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
        public void PublishFlatContainerManifest()
        {
            PushToBuildStorage task = ConstructPushToBuildStorageTask();
            task.PublishFlatContainer = true;

            string expectedManifestContent = BuildExpectedManifestContent(
                name: "https://dnceng@dev.azure.com/dnceng/internal/test-repo",
                branch: "/refs/heads/branch",
                commit: "1234567890abcdef",
                isStable: true,
                publishFlatContainer: true);

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
            PushToBuildStorage task = ConstructPushToBuildStorageTask();
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
            PushToBuildStorage task = ConstructPushToBuildStorageTask();
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
        public void SigningInfoInManifest()
        {
            PushToBuildStorage task = ConstructPushToBuildStorageTask();
            task.FileExtensionSignInfo = new ITaskItem[]
            {
                new TaskItem(".dll", new Dictionary<string, string>
                {
                    { "CertificateName", "TestSigningCert" }
                }),
                new TaskItem(".nupkg", new Dictionary<string, string>
                {
                    { "CertificateName", "TestNupkg" }
                }),
                new TaskItem(".zip", new Dictionary<string, string>
                {
                    { "CertificateName", "None" }
                }),
            };
            task.FileSignInfo = new ITaskItem[]
            {
                new TaskItem("Best.dll", new Dictionary<string, string>
                {
                    { "CertificateName", "BestCert" }
                }),
                new TaskItem("Worst.dll", new Dictionary<string, string>
                {
                    { "CertificateName", "WorstCert" }
                }),
            };
            task.CertificatesSignInfo = new ITaskItem[]
            {
                new TaskItem("BestCert", new Dictionary<string, string>
                {
                    { "DualSigningAllowed", "true" }
                }),
                new TaskItem("WorstCert", new Dictionary<string, string>
                {
                    { "DualSigningAllowed", "false" }
                }),
            };
            task.StrongNameSignInfo = new ITaskItem[]
            {
                new TaskItem("VeryCoolStrongName", new Dictionary<string, string>
                {
                    { "PublicKeyToken", "123456789ABCDEF0" },
                    { "CertificateName", "BestCert" }
                }),
                new TaskItem("VeryTrashStrongName", new Dictionary<string, string>
                {
                    { "PublicKeyToken", "00FEDCBA98765432" },
                    { "CertificateName", "WorstCert" }
                }),
            };
            task.ItemsToSign = new ITaskItem[]
            {
                new TaskItem(PACKAGE_A),
                new TaskItem(PACKAGE_B),
            };

            string expectedManifestContent = BuildExpectedManifestContent(
                name: "https://dnceng@dev.azure.com/dnceng/internal/test-repo",
                branch: "/refs/heads/branch",
                commit: "1234567890abcdef",
                isStable: true,
                includeSigningInfo: true);

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
                id: "test-package-a",
                version: NUPKG_VERSION
            )));
            nupkgInfoFactoryMock.Setup(m => m.CreateNupkgInfo(PACKAGE_B)).Returns(new NupkgInfo(new PackageIdentity(
                id: "test-package-b",
                version: NUPKG_VERSION
            )));

            // Dependency Injection setup
            var collection = new ServiceCollection()
                .AddSingleton(fileSystemMock.Object)
                .AddSingleton<IBuildModelFactory, BuildModelFactory>()
                .AddSingleton<IPackageArtifactModelFactory, PackageArtifactModelFactory>()
                .AddSingleton<IBlobArtifactModelFactory, BlobArtifactModelFactory>()
                .AddSingleton<ISigningInformationModelFactory, SigningInformationModelFactory>()
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
