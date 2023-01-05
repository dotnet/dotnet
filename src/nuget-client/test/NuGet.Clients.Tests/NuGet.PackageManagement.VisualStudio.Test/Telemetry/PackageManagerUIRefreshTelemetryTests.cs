// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Moq;
using NuGet.PackageManagement.Telemetry;
using NuGet.VisualStudio.Internal.Contracts;
using Xunit;

namespace NuGet.PackageManagement.VisualStudio.Test
{
    public class PackageManagerUIRefreshTelemetryTests
    {
        [Fact]
        public void ForSolution_SimulatedData_NoProjectKindIfIsSolutionLevelSetToTrue()
        {
            var telemetryEvent = PackageManagerUIRefreshEvent.ForSolution(
                It.IsAny<Guid>(),
                It.IsAny<RefreshOperationSource>(),
                It.IsAny<RefreshOperationStatus>(),
                tab: It.IsAny<ItemFilter>(),
                isUIFiltering: It.IsAny<bool>(),
                It.IsAny<TimeSpan>(),
                It.IsAny<double?>());

            Assert.Null(telemetryEvent["ProjectKind"]);
            Assert.Null(telemetryEvent["ProjectId"]);
        }

        [Fact]
        public void ForProject_SimulatedData_ProjectKindAndProjectIdSet()
        {
            NuGetProjectKind kind = NuGetProjectKind.PackageReference;
            string projectId = "simulated-project-id-Guid";
            var telemetryEvent = PackageManagerUIRefreshEvent.ForProject(
                It.IsAny<Guid>(),
                It.IsAny<RefreshOperationSource>(),
                It.IsAny<RefreshOperationStatus>(),
                tab: It.IsAny<ItemFilter>(),
                isUIFiltering: It.IsAny<bool>(),
                It.IsAny<TimeSpan>(),
                It.IsAny<double?>(),
                projectId,
                kind);

            Assert.NotNull(telemetryEvent["ProjectKind"]);
            Assert.NotNull(telemetryEvent["ProjectId"]);
        }
    }
}
