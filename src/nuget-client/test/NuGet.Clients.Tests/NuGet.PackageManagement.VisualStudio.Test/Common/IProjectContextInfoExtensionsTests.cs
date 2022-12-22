// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceHub.Framework;
using Moq;
using NuGet.Packaging.Core;
using NuGet.Versioning;
using NuGet.VisualStudio.Internal.Contracts;
using Xunit;

namespace NuGet.PackageManagement.VisualStudio.Test
{
    public class IProjectContextInfoExtensionsTests
    {
        private static readonly PackageIdentity PackageIdentity = new PackageIdentity(id: "a", NuGetVersion.Parse("1.0.0"));

        [Fact]
        public async Task IsUpgradeableAsync_WhenProjectContextInfoIsNull_Throws()
        {
            await VerifyMicrosoftAssumesExceptionAsync(
                () => IProjectContextInfoExtensions.IsUpgradeableAsync(
                    projectContextInfo: null,
                    Mock.Of<IServiceBroker>(),
                    CancellationToken.None)
                .AsTask());
        }

        [Fact]
        public async Task IsUpgradeableAsync_WhenServiceBrokerIsNull_Throws()
        {
            await VerifyMicrosoftAssumesExceptionAsync(
                () => IProjectContextInfoExtensions.IsUpgradeableAsync(
                    Mock.Of<IProjectContextInfo>(),
                    serviceBroker: null,
                    CancellationToken.None)
                .AsTask());
        }

        [Fact]
        public async Task IsUpgradeableAsync_WhenCancellationTokenIsCancelled_Throws()
        {
            await Assert.ThrowsAsync<OperationCanceledException>(
                () => IProjectContextInfoExtensions.IsUpgradeableAsync(
                    Mock.Of<IProjectContextInfo>(),
                    Mock.Of<IServiceBroker>(),
                    new CancellationToken(canceled: true))
                .AsTask());
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task IsUpgradeableAsync_WhenArgumentsAreValid_ReturnsBoolean(bool expectedResult)
        {
            var serviceBroker = new Mock<IServiceBroker>();
            var projectUpgraderService = new Mock<INuGetProjectUpgraderService>();
            var project = new Mock<IProjectContextInfo>();
            string projectId = Guid.NewGuid().ToString();

            project.SetupGet(x => x.ProjectId)
                .Returns(projectId);

            projectUpgraderService.Setup(
                x => x.IsProjectUpgradeableAsync(
                    It.Is<string>(id => string.Equals(projectId, id)),
                    It.IsAny<CancellationToken>()))
                .Returns(new ValueTask<bool>(expectedResult));

            serviceBroker.Setup(
#pragma warning disable ISB001 // Dispose of proxies
                x => x.GetProxyAsync<INuGetProjectUpgraderService>(
                    It.Is<ServiceRpcDescriptor>(descriptor => descriptor == NuGetServices.ProjectUpgraderService),
                    It.IsAny<ServiceActivationOptions>(),
                    It.IsAny<CancellationToken>()))
#pragma warning restore ISB001 // Dispose of proxies
                .Returns(new ValueTask<INuGetProjectUpgraderService>(projectUpgraderService.Object));

            bool actualResult = await IProjectContextInfoExtensions.IsUpgradeableAsync(
                project.Object,
                serviceBroker.Object,
                CancellationToken.None);

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public async Task GetInstalledPackagesAsync_WhenProjectContextInfoIsNull_Throws()
        {
            await VerifyMicrosoftAssumesExceptionAsync(
                () => IProjectContextInfoExtensions.GetInstalledPackagesAsync(
                    projectContextInfo: null,
                    Mock.Of<IServiceBroker>(),
                    CancellationToken.None)
                .AsTask());
        }

        [Fact]
        public async Task GetInstalledPackagesAsync_WhenServiceBrokerIsNull_Throws()
        {
            await VerifyMicrosoftAssumesExceptionAsync(
                () => IProjectContextInfoExtensions.GetInstalledPackagesAsync(
                    Mock.Of<IProjectContextInfo>(),
                    serviceBroker: null,
                    CancellationToken.None)
                .AsTask());
        }

        [Fact]
        public async Task GetInstalledPackagesAsync_WhenCancellationTokenIsCancelled_Throws()
        {
            await Assert.ThrowsAsync<OperationCanceledException>(
                () => IProjectContextInfoExtensions.GetInstalledPackagesAsync(
                    Mock.Of<IProjectContextInfo>(),
                    Mock.Of<IServiceBroker>(),
                    new CancellationToken(canceled: true))
                .AsTask());
        }

        [Fact]
        public async Task GetInstalledPackagesAsync_WhenArgumentsAreValid_ReturnsInstalledPackages()
        {
            var serviceBroker = new Mock<IServiceBroker>();
            var projectManagerService = new Mock<INuGetProjectManagerService>();
            var project = new Mock<IProjectContextInfo>();
            string projectId = Guid.NewGuid().ToString();
            var expectedResult = new List<IPackageReferenceContextInfo>();

            project.SetupGet(x => x.ProjectId)
                .Returns(projectId);

            projectManagerService.Setup(
                x => x.GetInstalledPackagesAsync(
                    It.Is<string[]>(projectIds => projectIds.Length == 1 && string.Equals(projectId, projectIds[0])),
                    It.IsAny<CancellationToken>()))
                .Returns(new ValueTask<IReadOnlyCollection<IPackageReferenceContextInfo>>(expectedResult));

            serviceBroker.Setup(
#pragma warning disable ISB001 // Dispose of proxies
                x => x.GetProxyAsync<INuGetProjectManagerService>(
                    It.Is<ServiceRpcDescriptor>(descriptor => descriptor == NuGetServices.ProjectManagerService),
                    It.IsAny<ServiceActivationOptions>(),
                    It.IsAny<CancellationToken>()))
#pragma warning restore ISB001 // Dispose of proxies
                .Returns(new ValueTask<INuGetProjectManagerService>(projectManagerService.Object));

            IReadOnlyCollection<IPackageReferenceContextInfo> actualResult = await IProjectContextInfoExtensions.GetInstalledPackagesAsync(
                project.Object,
                serviceBroker.Object,
                CancellationToken.None);

            Assert.Same(expectedResult, actualResult);
        }

        [Fact]
        public async Task GetMetadataAsync_WhenProjectContextInfoIsNull_Throws()
        {
            await VerifyMicrosoftAssumesExceptionAsync(
                () => IProjectContextInfoExtensions.GetMetadataAsync(
                    projectContextInfo: null,
                    Mock.Of<IServiceBroker>(),
                    CancellationToken.None)
                .AsTask());
        }

        [Fact]
        public async Task GetMetadataAsync_WhenServiceBrokerIsNull_Throws()
        {
            await VerifyMicrosoftAssumesExceptionAsync(
                () => IProjectContextInfoExtensions.GetMetadataAsync(
                    Mock.Of<IProjectContextInfo>(),
                    serviceBroker: null,
                    CancellationToken.None)
                .AsTask());
        }

        [Fact]
        public async Task GetMetadataAsync_WhenCancellationTokenIsCancelled_Throws()
        {
            await Assert.ThrowsAsync<OperationCanceledException>(
                () => IProjectContextInfoExtensions.GetMetadataAsync(
                    Mock.Of<IProjectContextInfo>(),
                    Mock.Of<IServiceBroker>(),
                    new CancellationToken(canceled: true))
                .AsTask());
        }

        [Fact]
        public async Task GetMetadataAsync_WhenArgumentsAreValid_ReturnsProjectMetadata()
        {
            var serviceBroker = new Mock<IServiceBroker>();
            var projectManagerService = new Mock<INuGetProjectManagerService>();
            var project = new Mock<IProjectContextInfo>();
            string projectId = Guid.NewGuid().ToString();
            var expectedResult = Mock.Of<IProjectMetadataContextInfo>();

            project.SetupGet(x => x.ProjectId)
                .Returns(projectId);

            projectManagerService.Setup(
                x => x.GetMetadataAsync(
                    It.Is<string>(id => string.Equals(projectId, id)),
                    It.IsAny<CancellationToken>()))
                .Returns(new ValueTask<IProjectMetadataContextInfo>(expectedResult));

            serviceBroker.Setup(
#pragma warning disable ISB001 // Dispose of proxies
                x => x.GetProxyAsync<INuGetProjectManagerService>(
                    It.Is<ServiceRpcDescriptor>(descriptor => descriptor == NuGetServices.ProjectManagerService),
                    It.IsAny<ServiceActivationOptions>(),
                    It.IsAny<CancellationToken>()))
#pragma warning restore ISB001 // Dispose of proxies
                .Returns(new ValueTask<INuGetProjectManagerService>(projectManagerService.Object));

            IProjectMetadataContextInfo actualResult = await IProjectContextInfoExtensions.GetMetadataAsync(
                project.Object,
                serviceBroker.Object,
                CancellationToken.None);

            Assert.Same(expectedResult, actualResult);
        }

        [Fact]
        public async Task GetUniqueNameOrNameAsync_WhenProjectContextInfoIsNull_Throws()
        {
            await VerifyMicrosoftAssumesExceptionAsync(
                () => IProjectContextInfoExtensions.GetUniqueNameOrNameAsync(
                    projectContextInfo: null,
                    Mock.Of<IServiceBroker>(),
                    CancellationToken.None)
                .AsTask());
        }

        [Fact]
        public async Task GetUniqueNameOrNameAsync_WhenServiceBrokerIsNull_Throws()
        {
            await VerifyMicrosoftAssumesExceptionAsync(
                () => IProjectContextInfoExtensions.GetUniqueNameOrNameAsync(
                    Mock.Of<IProjectContextInfo>(),
                    serviceBroker: null,
                    CancellationToken.None)
                .AsTask());
        }

        [Fact]
        public async Task GetUniqueNameOrNameAsync_WhenCancellationTokenIsCancelled_Throws()
        {
            await Assert.ThrowsAsync<OperationCanceledException>(
                () => IProjectContextInfoExtensions.GetUniqueNameOrNameAsync(
                    Mock.Of<IProjectContextInfo>(),
                    Mock.Of<IServiceBroker>(),
                    new CancellationToken(canceled: true))
                .AsTask());
        }

        [Theory]
        [InlineData("unique", null)]
        [InlineData(null, "name")]
        [InlineData("unique", "name")]
        public async Task GetUniqueNameOrNameAsync_WhenArgumentsAreValid_ReturnsString(
            string uniqueName,
            string name)
        {
            var serviceBroker = new Mock<IServiceBroker>();
            var projectManagerService = new Mock<INuGetProjectManagerService>();
            var project = new Mock<IProjectContextInfo>();
            string projectId = Guid.NewGuid().ToString();
            var projectMetadata = new Mock<IProjectMetadataContextInfo>();

            projectMetadata.SetupGet(x => x.Name)
                .Returns(name);

            projectMetadata.SetupGet(x => x.UniqueName)
                .Returns(uniqueName);

            string expectedResult = uniqueName ?? name;

            project.SetupGet(x => x.ProjectId)
                .Returns(projectId);

            projectManagerService.Setup(
                x => x.GetMetadataAsync(
                    It.Is<string>(id => string.Equals(projectId, id)),
                    It.IsAny<CancellationToken>()))
                .Returns(new ValueTask<IProjectMetadataContextInfo>(projectMetadata.Object));

            serviceBroker.Setup(
#pragma warning disable ISB001 // Dispose of proxies
                x => x.GetProxyAsync<INuGetProjectManagerService>(
                    It.Is<ServiceRpcDescriptor>(descriptor => descriptor == NuGetServices.ProjectManagerService),
                    It.IsAny<ServiceActivationOptions>(),
                    It.IsAny<CancellationToken>()))
#pragma warning restore ISB001 // Dispose of proxies
                .Returns(new ValueTask<INuGetProjectManagerService>(projectManagerService.Object));

            string actualResult = await IProjectContextInfoExtensions.GetUniqueNameOrNameAsync(
                project.Object,
                serviceBroker.Object,
                CancellationToken.None);

            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [InlineData(new string[] { }, 0)]
        [InlineData(new string[] { "folder1" }, 1)]
        [InlineData(new string[] { "folder1", "folder2" }, 2)]
        public async Task GetPackageFoldersAsync_OneProject_ReturnsPackageFolderAsync(IReadOnlyCollection<string> folderCollection, int expected)
        {
            var serviceBroker = Mock.Of<IServiceBroker>();
            var projectManagerService = Mock.Of<INuGetProjectManagerService>();
            var project = Mock.Of<IProjectContextInfo>();

            _ = Mock.Get(serviceBroker)
                .Setup(sb => sb.GetProxyAsync<INuGetProjectManagerService>(
                    It.Is<ServiceRpcDescriptor>(descriptor => descriptor == NuGetServices.ProjectManagerService),
                    It.IsAny<ServiceActivationOptions>(),
                    It.IsAny<CancellationToken>()))
                .Returns(new ValueTask<INuGetProjectManagerService>(projectManagerService));

            Mock.Get(projectManagerService)
                .Setup(prj => prj.GetPackageFoldersAsync(It.IsAny<IReadOnlyCollection<string>>(), It.IsAny<CancellationToken>()))
                .Returns(new ValueTask<IReadOnlyCollection<string>>(folderCollection));

            IReadOnlyCollection<string> folders = await IProjectContextInfoExtensions.GetPackageFoldersAsync(project, serviceBroker, CancellationToken.None);

            Assert.NotNull(folders);
            Assert.Equal(expected, folders.Count);
        }

        [Fact]
        public async Task TryGetInstalledPackageFilePathAsync_WhenProjectContextInfoIsNull_Throws()
        {
            await VerifyMicrosoftAssumesExceptionAsync(
                () => IProjectContextInfoExtensions.TryGetInstalledPackageFilePathAsync(
                    projectContextInfo: null,
                    Mock.Of<IServiceBroker>(),
                    PackageIdentity,
                    CancellationToken.None)
                .AsTask());
        }

        [Fact]
        public async Task TryGetInstalledPackageFilePathAsync_WhenServiceBrokerIsNull_Throws()
        {
            await VerifyMicrosoftAssumesExceptionAsync(
                () => IProjectContextInfoExtensions.TryGetInstalledPackageFilePathAsync(
                    Mock.Of<IProjectContextInfo>(),
                    serviceBroker: null,
                    PackageIdentity,
                    CancellationToken.None)
                .AsTask());
        }

        [Fact]
        public async Task TryGetInstalledPackageFilePathAsync_WhenPackageIdentityIsNull_Throws()
        {
            await VerifyMicrosoftAssumesExceptionAsync(
                () => IProjectContextInfoExtensions.TryGetInstalledPackageFilePathAsync(
                    Mock.Of<IProjectContextInfo>(),
                    Mock.Of<IServiceBroker>(),
                    packageIdentity: null,
                    CancellationToken.None)
                .AsTask());
        }

        [Fact]
        public async Task TryGetInstalledPackageFilePathAsync_WhenCancellationTokenIsCancelled_Throws()
        {
            await Assert.ThrowsAsync<OperationCanceledException>(
                () => IProjectContextInfoExtensions.TryGetInstalledPackageFilePathAsync(
                    Mock.Of<IProjectContextInfo>(),
                    Mock.Of<IServiceBroker>(),
                    PackageIdentity,
                    new CancellationToken(canceled: true))
                .AsTask());
        }

        [Fact]
        public async Task GetAllPackagesFolderAsync_WhenCancellationTokenIsCancelled_ThrowsAsync()
        {
            await Assert.ThrowsAsync<OperationCanceledException>(
                () => IProjectContextInfoExtensions.GetPackageFoldersAsync(
                    Mock.Of<IProjectContextInfo>(),
                    Mock.Of<IServiceBroker>(),
                    new CancellationToken(canceled: true))
                .AsTask());
        }

        [Fact]
        public async Task TryGetInstalledPackageFilePathAsync_WhenArgumentsAreValid_ReturnsString()
        {
            var serviceBroker = new Mock<IServiceBroker>();
            var projectManagerService = new Mock<INuGetProjectManagerService>();
            var project = new Mock<IProjectContextInfo>();
            string projectId = Guid.NewGuid().ToString();
            (bool, string) expectedResult = (true, "a");

            project.SetupGet(x => x.ProjectId)
                .Returns(projectId);

            projectManagerService.Setup(
                x => x.TryGetInstalledPackageFilePathAsync(
                    It.Is<string>(id => string.Equals(projectId, id)),
                    It.Is<PackageIdentity>(pi => ReferenceEquals(PackageIdentity, pi)),
                    It.IsAny<CancellationToken>()))
                .Returns(new ValueTask<(bool, string)>(expectedResult));

            serviceBroker.Setup(
#pragma warning disable ISB001 // Dispose of proxies
                x => x.GetProxyAsync<INuGetProjectManagerService>(
                    It.Is<ServiceRpcDescriptor>(descriptor => descriptor == NuGetServices.ProjectManagerService),
                    It.IsAny<ServiceActivationOptions>(),
                    It.IsAny<CancellationToken>()))
#pragma warning restore ISB001 // Dispose of proxies
                .Returns(new ValueTask<INuGetProjectManagerService>(projectManagerService.Object));

            (bool, string) actualResult = await IProjectContextInfoExtensions.TryGetInstalledPackageFilePathAsync(
                project.Object,
                serviceBroker.Object,
                PackageIdentity,
                CancellationToken.None);

            Assert.Equal(expectedResult.Item1, actualResult.Item1);
            Assert.Equal(expectedResult.Item2, actualResult.Item2);
        }

        private static async Task VerifyMicrosoftAssumesExceptionAsync(Func<Task> test)
        {
            Exception exception = await Assert.ThrowsAnyAsync<Exception>(test);

            Assert.Equal(typeof(Microsoft.Assumes), exception.GetType().DeclaringType);
        }
    }
}
