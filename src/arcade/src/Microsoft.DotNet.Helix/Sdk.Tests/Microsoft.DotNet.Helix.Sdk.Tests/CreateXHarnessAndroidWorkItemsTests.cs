// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Arcade.Common;
using Microsoft.Arcade.Test.Common;
using Microsoft.Build.Framework;
using Microsoft.DotNet.Internal.DependencyInjection.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

#nullable enable
namespace Microsoft.DotNet.Helix.Sdk.Tests
{
    public class CreateXHarnessAndroidWorkItemsTests
    {
        private readonly MockFileSystem _fileSystem;
        private readonly Mock<IZipArchiveManager> _zipArchiveManager;
        private readonly CreateXHarnessAndroidWorkItems _task;

        public CreateXHarnessAndroidWorkItemsTests()
        {
            _fileSystem = new MockFileSystem();
            _zipArchiveManager = new();
            _zipArchiveManager.SetReturnsDefault(Task.CompletedTask);
            _zipArchiveManager
                .Setup(x => x.ArchiveFile(It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string>((folder, zipPath) =>
                {
                    _fileSystem.Files.Add(zipPath, "zip of " + folder);
                });

            _task = new CreateXHarnessAndroidWorkItems()
            {                
                BuildEngine = new MockBuildEngine(),
            };
        }

        [Fact]
        public void MissingApkNamePropertyIsCaught()
        {
            var collection = CreateMockServiceCollection();
            _task.ConfigureServices(collection);
            _task.Apks = new[]
            {
                CreateApk("/apks/System.Foo.app", null!)
            };

            // Act
            using var provider = collection.BuildServiceProvider();
            _task.InvokeExecute(provider).Should().BeFalse();

            // Verify
            _task.WorkItems.Length.Should().Be(0);
        }

        [Fact]
        public void AndroidXHarnessWorkItemIsCreated()
        {
            var collection = CreateMockServiceCollection();
            _task.ConfigureServices(collection);
            _task.Apks = new[]
            {
                CreateApk("/apks/System.Foo.apk", "System.Foo", "00:15:42", "00:08:55")
            };

            // Act
            using var provider = collection.BuildServiceProvider();
            _task.InvokeExecute(provider).Should().BeTrue();

            // Verify
            _task.WorkItems.Length.Should().Be(1);

            var workItem = _task.WorkItems.First();
            workItem.GetMetadata("Identity").Should().Be("System.Foo");
            workItem.GetMetadata("Timeout").Should().Be("00:17:42");

            var payloadArchive = workItem.GetMetadata("PayloadArchive");
            payloadArchive.Should().NotBeNullOrEmpty();
            _fileSystem.FileExists(payloadArchive).Should().BeTrue();

            var command = workItem.GetMetadata("Command");
            command.Should().Contain("-timeout \"00:08:55\"");

            _zipArchiveManager
                .Verify(x => x.ArchiveFile("/apks/System.Foo.apk", payloadArchive), Times.Once);
            _zipArchiveManager
                .Verify(x => x.AddResourceFileToArchive<XHarnessTaskBase>(payloadArchive, It.Is<string>(s => s.Contains("xharness-helix-job.android.sh")), "xharness-helix-job.android.sh"), Times.AtLeastOnce);
            _zipArchiveManager
                .Verify(x => x.AddResourceFileToArchive<XHarnessTaskBase>(payloadArchive, It.Is<string>(s => s.Contains("xharness-helix-job.android.ps1")), "xharness-helix-job.android.ps1"), Times.AtLeastOnce);
        }

        [Fact]
        public void ArchivePayloadIsOverwritten()
        {
            var collection = CreateMockServiceCollection();
            _task.ConfigureServices(collection);
            _task.Apks = new[]
            {
                CreateApk("apks/System.Foo.apk", "System.Foo"),
                CreateApk("apks/System.Bar.apk", "System.Bar"),
            };

            _fileSystem.Files.Add("apks/xharness-payload-system.foo.zip", "archive");

            // Act
            using var provider = collection.BuildServiceProvider();
            _task.InvokeExecute(provider).Should().BeTrue();

            // Verify
            _task.WorkItems.Length.Should().Be(2);

            var workItem = _task.WorkItems.Last();
            workItem.GetMetadata("Identity").Should().Be("System.Bar");
            
            workItem = _task.WorkItems.First();
            workItem.GetMetadata("Identity").Should().Be("System.Foo");

            var payloadArchive = workItem.GetMetadata("PayloadArchive");
            payloadArchive.Should().NotBeNullOrEmpty();
            _fileSystem.FileExists(payloadArchive).Should().BeTrue();
            _fileSystem.RemovedFiles.Should().Contain(payloadArchive);
        }

        [Fact]
        public void ApkIsReused()
        {
            var collection = CreateMockServiceCollection();
            _task.ConfigureServices(collection);
            _task.Apks = new[]
            {
                CreateApk("item-1", "System.Foo", apkPath: "apks/System.Foo.apk"),
                CreateApk("item-2", "System.Foo", apkPath: "apks/System.Foo.apk"),
            };

            // Act
            using var provider = collection.BuildServiceProvider();
            _task.InvokeExecute(provider).Should().BeTrue();

            // Verify
            _task.WorkItems.Length.Should().Be(2);
            _fileSystem.RemovedFiles.Should().BeEmpty();

            var workItem1 = _task.WorkItems.Last();
            workItem1.GetMetadata("Identity").Should().Be("item-2");

            var payloadArchive = workItem1.GetMetadata("PayloadArchive");
            payloadArchive.Should().NotBeNullOrEmpty();
            _fileSystem.FileExists(payloadArchive).Should().BeTrue();

            var workItem2 = _task.WorkItems.First();
            workItem2.GetMetadata("Identity").Should().Be("item-1");

            payloadArchive = workItem2.GetMetadata("PayloadArchive");
            payloadArchive.Should().NotBeNullOrEmpty();
            _fileSystem.FileExists(payloadArchive).Should().BeTrue();
        }

        [Fact]
        public void ZippedApkIsProvided()
        {
            var collection = CreateMockServiceCollection();
            _task.ConfigureServices(collection);
            _task.Apks = new[]
            {
                CreateApk("/apks/System.Foo.zip", "System.Foo", "00:15:42", "00:08:55")
            };

            // Act
            using var provider = collection.BuildServiceProvider();
            _task.InvokeExecute(provider).Should().BeTrue();

            // Verify
            _task.WorkItems.Length.Should().Be(1);

            var workItem = _task.WorkItems.First();
            workItem.GetMetadata("Identity").Should().Be("System.Foo");
            workItem.GetMetadata("Timeout").Should().Be("00:17:42");

            var payloadArchive = workItem.GetMetadata("PayloadArchive");
            payloadArchive.Should().NotBeNullOrEmpty();
            _fileSystem.FileExists(payloadArchive).Should().BeTrue();

            var command = workItem.GetMetadata("Command");
            command.Should().Contain("-timeout \"00:08:55\"");

            _zipArchiveManager
                .Verify(x => x.ArchiveFile(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            _zipArchiveManager
                .Verify(x => x.AddResourceFileToArchive<XHarnessTaskBase>(payloadArchive, It.Is<string>(s => s.Contains("xharness-helix-job.android.sh")), "xharness-helix-job.android.sh"), Times.AtLeastOnce);
            _zipArchiveManager
                .Verify(x => x.AddResourceFileToArchive<XHarnessTaskBase>(payloadArchive, It.Is<string>(s => s.Contains("xharness-helix-job.android.ps1")), "xharness-helix-job.android.ps1"), Times.AtLeastOnce);
        }

        [Fact]
        public void AreDependenciesRegistered()
        {
            var task = new CreateXHarnessAndroidWorkItems();

            var collection = new ServiceCollection();
            task.ConfigureServices(collection);
            var provider = collection.BuildServiceProvider();

            DependencyInjectionValidation.IsDependencyResolutionCoherent(
                s => task.ConfigureServices(s),
                out string message,
                additionalSingletonTypes: task.GetExecuteParameterTypes()
            )
            .Should()
            .BeTrue(message);
        }

        private ITaskItem CreateApk(
            string itemSpec,
            string apkName,
            string? workItemTimeout = null,
            string? testTimeout = null,
            int expectedExitCode = 0,
            string? apkPath = null)
        {
            var mockBundle = new Mock<ITaskItem>();
            mockBundle.SetupGet(x => x.ItemSpec).Returns(itemSpec);
            mockBundle.Setup(x => x.GetMetadata(CreateXHarnessAndroidWorkItems.MetadataNames.AndroidPackageName)).Returns(apkName);

            if (workItemTimeout != null)
            {
                mockBundle.Setup(x => x.GetMetadata(XHarnessTaskBase.MetadataName.WorkItemTimeout)).Returns(workItemTimeout);
            }

            if (testTimeout != null)
            {
                mockBundle.Setup(x => x.GetMetadata(XHarnessTaskBase.MetadataName.TestTimeout)).Returns(testTimeout);
            }

            if (expectedExitCode != 0)
            {
                mockBundle.Setup(x => x.GetMetadata(XHarnessTaskBase.MetadataName.ExpectedExitCode)).Returns(expectedExitCode.ToString());
            }

            if (apkPath != null)
            {
                mockBundle.Setup(x => x.GetMetadata(CreateXHarnessAndroidWorkItems.MetadataNames.ApkPath)).Returns(apkPath);
            }

            _fileSystem.WriteToFile(apkPath ?? itemSpec, "apk");

            return mockBundle.Object;
        }

        private IServiceCollection CreateMockServiceCollection()
        {
            var collection = new ServiceCollection();
            collection.AddSingleton<IFileSystem>(_fileSystem);
            collection.AddSingleton(_zipArchiveManager.Object);
            return collection;
        }
    }
}
