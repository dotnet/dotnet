// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NuGet.PackageManagement.VisualStudio;
using NuGet.ProjectManagement;
using NuGet.VisualStudio;
using NuGet.VisualStudio.Telemetry;
using Xunit;

namespace NuGet.SolutionRestoreManager.Test
{
    public class VsSolutionRestoreStatusProviderTests
    {
        [Fact]
        public async Task IsRestoreCompleteAsync_WithCancelledToken_DoesNotLogFault()
        {
            // Arrange
            Mock<ISolutionRestoreWorker> worker = new();

            Mock<NuGetProject> project = new();

            Mock<IVsSolutionManager> solutionManager = new();
            solutionManager.Setup(sm => sm.IsSolutionOpenAsync())
                .Returns(Task.FromResult(true));
            solutionManager.SetupGet(sm => sm.IsSolutionOpen)
                .Returns(true);
            solutionManager.Setup(sm => sm.GetNuGetProjectsAsync())
                .Returns(Task.FromResult<IEnumerable<NuGetProject>>(new[] { project.Object }));

            Mock<INuGetTelemetryProvider> telemetryProvider = new();

            VsSolutionRestoreStatusProvider target =
                new(
                    new Lazy<ISolutionRestoreWorker>(() => worker.Object),
                    new Lazy<IVsSolutionManager>(() => solutionManager.Object),
                    telemetryProvider.Object);

            // Act
            var cancellationToken = new CancellationToken(canceled: true);
            bool exceptionCaught = false;
            try
            {
                _ = await target.IsRestoreCompleteAsync(cancellationToken);
            }
            catch (OperationCanceledException)
            {
                exceptionCaught = true;
            }

            // Assert
            exceptionCaught.Should().BeTrue();
            telemetryProvider
                .Verify(tp => tp.PostFault(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IDictionary<string, object>>()),
                    Times.Never);
            telemetryProvider
                .Verify(tp => tp.PostFaultAsync(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IDictionary<string, object>>()),
                    Times.Never);
        }
    }
}
