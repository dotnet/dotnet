// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceHub.Framework;
using Moq;
using NuGet.PackageManagement.VisualStudio;
using NuGet.VisualStudio.Internal.Contracts;
using Xunit;

namespace NuGet.PackageManagement.UI.Test
{
    public class ProjectUtilityTests
    {
        [Fact]
        public async Task GetSortedProjectIdsAsync_WhenServiceBrokerIsNull_Throws()
        {
            ArgumentNullException exception = await Assert.ThrowsAsync<ArgumentNullException>(
                () => ProjectUtility.GetSortedProjectIdsAsync(
                    serviceBroker: null,
                    projects: null,
                    CancellationToken.None).AsTask());

            Assert.Equal("serviceBroker", exception.ParamName);
        }

        [Fact]
        public async Task GetSortedProjectIdsAsync_WhenProjectsIsNull_Throws()
        {
            ArgumentNullException exception = await Assert.ThrowsAsync<ArgumentNullException>(
                () => ProjectUtility.GetSortedProjectIdsAsync(
                    Mock.Of<IServiceBroker>(),
                    projects: null,
                    CancellationToken.None).AsTask());

            Assert.Equal("projects", exception.ParamName);
        }

        [Fact]
        public async Task GetSortedProjectIdsAsync_WhenCancellationTokenIsCancelled_Throws()
        {
            await Assert.ThrowsAsync<OperationCanceledException>(
                () => ProjectUtility.GetSortedProjectIdsAsync(
                    Mock.Of<IServiceBroker>(),
                    Enumerable.Empty<IProjectContextInfo>(),
                    new CancellationToken(canceled: true)).AsTask());
        }

        [Fact]
        public async Task GetSortedProjectIdsAsync_WhenProjectsUnsorted_ReturnsSortedProjectIds()
        {
            string projectIdA = Guid.NewGuid().ToString();
            string projectIdB = Guid.NewGuid().ToString();
            string projectIdC = Guid.NewGuid().ToString();
            var projectA = new Mock<IProjectContextInfo>();
            var projectB = new Mock<IProjectContextInfo>();
            var projectC = new Mock<IProjectContextInfo>();
            var projectAMetadata = new Mock<IProjectMetadataContextInfo>();
            var projectBMetadata = new Mock<IProjectMetadataContextInfo>();
            var projectCMetadata = new Mock<IProjectMetadataContextInfo>();
            var serviceBroker = new Mock<IServiceBroker>();
            Mock<INuGetProjectManagerService> projectManagerService = SetupProjectManagerService(serviceBroker);

            projectManagerService.Setup(
                    x => x.GetMetadataAsync(
                        It.Is<string>(projectId => projectId == projectIdA),
                        It.IsAny<CancellationToken>()))
                .Returns(new ValueTask<IProjectMetadataContextInfo>(projectAMetadata.Object));
            projectManagerService.Setup(
                    x => x.GetMetadataAsync(
                        It.Is<string>(projectId => projectId == projectIdB),
                        It.IsAny<CancellationToken>()))
                .Returns(new ValueTask<IProjectMetadataContextInfo>(projectBMetadata.Object));
            projectManagerService.Setup(
                    x => x.GetMetadataAsync(
                        It.Is<string>(projectId => projectId == projectIdC),
                        It.IsAny<CancellationToken>()))
                .Returns(new ValueTask<IProjectMetadataContextInfo>(projectCMetadata.Object));

            projectA.SetupGet(x => x.ProjectId)
                .Returns(projectIdA);
            projectB.SetupGet(x => x.ProjectId)
                .Returns(projectIdB);
            projectC.SetupGet(x => x.ProjectId)
                .Returns(projectIdC);

            projectAMetadata.SetupGet(x => x.UniqueName)
                .Returns("a");
            projectBMetadata.SetupGet(x => x.UniqueName)
                .Returns("b");
            projectCMetadata.SetupGet(x => x.UniqueName)
                .Returns("c");

            // ProjectId values were picked to help detect if the test method incorrectly sorts on project ID.
            var expectedResults = new List<string>() { "2", "0", "1" };

            projectAMetadata.SetupGet(x => x.ProjectId)
                .Returns(expectedResults[0]);
            projectBMetadata.SetupGet(x => x.ProjectId)
                .Returns(expectedResults[1]);
            projectCMetadata.SetupGet(x => x.ProjectId)
                .Returns(expectedResults[2]);

            IEnumerable<string> actualResults = await ProjectUtility.GetSortedProjectIdsAsync(
                serviceBroker.Object,
                new[] { projectB.Object, projectA.Object, projectC.Object },
                CancellationToken.None);

            Assert.Equal(expectedResults, actualResults.ToArray());
        }

        private static Mock<INuGetProjectManagerService> SetupProjectManagerService(Mock<IServiceBroker> serviceBroker)
        {
            var projectManagerService = new Mock<INuGetProjectManagerService>();

            serviceBroker.Setup(
#pragma warning disable ISB001 // Dispose of proxies
                    x => x.GetProxyAsync<INuGetProjectManagerService>(
                        It.Is<ServiceRpcDescriptor>(s => s == NuGetServices.ProjectManagerService),
                        It.IsAny<ServiceActivationOptions>(),
                        It.IsAny<CancellationToken>()))
#pragma warning restore ISB001 // Dispose of proxies
                .Returns(new ValueTask<INuGetProjectManagerService>(projectManagerService.Object));

            return projectManagerService;
        }
    }
}
