// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using Moq;
using NuGet.Common;
using NuGet.PackageManagement.Telemetry;
using NuGet.VisualStudio;
using NuGet.VisualStudio.Internal.Contracts;
using Xunit;

namespace NuGet.PackageManagement.VisualStudio.Test
{
    public class NuGetTelemetryServiceTests
    {
        [Theory]
        [InlineData(NuGetProjectType.Unsupported)]
        [InlineData(NuGetProjectType.Unknown)]
        [InlineData(NuGetProjectType.PackagesConfig)]
        [InlineData(NuGetProjectType.UwpProjectJson)]
        [InlineData(NuGetProjectType.CPSBasedPackageRefs)]
        [InlineData(NuGetProjectType.LegacyProjectSystemWithPackageRefs)]
        [InlineData(NuGetProjectType.UnconfiguredNuGetType)]
        public void NuGetTelemetryService_EmitProjectInformation(NuGetProjectType projectType)
        {
            // Arrange
            var telemetrySession = new Mock<ITelemetrySession>();
            TelemetryEvent lastTelemetryEvent = null;
            telemetrySession
                .Setup(x => x.PostEvent(It.IsAny<TelemetryEvent>()))
                .Callback<TelemetryEvent>(x => lastTelemetryEvent = x);

            var projectInformation = new ProjectTelemetryEvent(
                "3.5.0-beta2",
                "15e9591f-9391-4ddf-a246-ca9e0351277d",
                projectType,
                true,
                @"C:\path\to\project.csproj");
            var target = new NuGetVSTelemetryService(telemetrySession.Object);

            // Act
            target.EmitTelemetryEvent(projectInformation);

            // Assert
            telemetrySession.Verify(x => x.PostEvent(It.IsAny<TelemetryEvent>()), Times.Once);
            Assert.NotNull(lastTelemetryEvent);
            Assert.Equal("ProjectInformation", lastTelemetryEvent.Name);
            Assert.Equal(4, lastTelemetryEvent.Count);

            var nuGetVersion = lastTelemetryEvent["NuGetVersion"];
            Assert.NotNull(nuGetVersion);
            Assert.IsType<string>(nuGetVersion);
            Assert.Equal(projectInformation.NuGetVersion, nuGetVersion);

            var projectId = lastTelemetryEvent["ProjectId"];
            Assert.NotNull(projectId);
            Assert.IsType<string>(projectId);
            Assert.Equal(projectInformation.ProjectId.ToString(), projectId);

            var actualProjectType = lastTelemetryEvent["NuGetProjectType"];
            Assert.NotNull(actualProjectType);
            Assert.IsType<NuGetProjectType>(actualProjectType);
            Assert.Equal(projectInformation.NuGetProjectType, actualProjectType);

            var isPRUpgradable = lastTelemetryEvent["IsPRUpgradable"];
            Assert.NotNull(isPRUpgradable);
            Assert.IsType<bool>(isPRUpgradable);
            Assert.Equal(projectInformation.IsProjectPRUpgradable, isPRUpgradable);

            var projectFilePath = lastTelemetryEvent
                .GetPiiData()
                .First(kv => kv.Key == ProjectTelemetryEvent.ProjectFilePath)
                .Value;
            Assert.IsType<string>(projectFilePath);
            Assert.True(!string.IsNullOrEmpty((string)projectFilePath));
        }

        [Theory]
        [InlineData(RefreshOperationSource.ActionsExecuted, RefreshOperationStatus.Success)]
        [InlineData(RefreshOperationSource.CacheUpdated, RefreshOperationStatus.Success)]
        [InlineData(RefreshOperationSource.CheckboxPrereleaseChanged, RefreshOperationStatus.NoOp)]
        [InlineData(RefreshOperationSource.ClearSearch, RefreshOperationStatus.NoOp)]
        [InlineData(RefreshOperationSource.ExecuteAction, RefreshOperationStatus.NotApplicable)]
        [InlineData(RefreshOperationSource.FilterSelectionChanged, RefreshOperationStatus.NotApplicable, false)]
        [InlineData(RefreshOperationSource.FilterSelectionChanged, RefreshOperationStatus.NotApplicable, true)]
        [InlineData(RefreshOperationSource.PackageManagerLoaded, RefreshOperationStatus.Success)]
        [InlineData(RefreshOperationSource.PackagesMissingStatusChanged, RefreshOperationStatus.Success)]
        [InlineData(RefreshOperationSource.PackageSourcesChanged, RefreshOperationStatus.Success)]
        [InlineData(RefreshOperationSource.ProjectsChanged, RefreshOperationStatus.Success)]
        [InlineData(RefreshOperationSource.ProjectsChanged, RefreshOperationStatus.Failed)]
        [InlineData(RefreshOperationSource.RestartSearchCommand, RefreshOperationStatus.Success)]
        [InlineData(RefreshOperationSource.SourceSelectionChanged, RefreshOperationStatus.Success)]
        public void NuGetTelemetryService_EmitsPMUIRefreshEvent(RefreshOperationSource expectedRefreshSource, RefreshOperationStatus expectedRefreshStatus, bool expectedUiFiltering = false)
        {
            // Arrange
            var telemetrySession = new Mock<ITelemetrySession>();
            TelemetryEvent lastTelemetryEvent = null;
            telemetrySession
                .Setup(x => x.PostEvent(It.IsAny<TelemetryEvent>()))
                .Callback<TelemetryEvent>(x => lastTelemetryEvent = x);

            var expectedGuid = Guid.NewGuid();
            var expectedIsSolutionLevel = true;
            var expectedTab = ItemFilter.All;
            var expectedTimeSinceLastRefresh = TimeSpan.FromSeconds(1);
            var expectedDuration = TimeSpan.FromSeconds(1);
            var refreshEvent = PackageManagerUIRefreshEvent.ForSolution(expectedGuid, expectedRefreshSource, expectedRefreshStatus, expectedTab, isUIFiltering: expectedUiFiltering, expectedTimeSinceLastRefresh, expectedDuration.TotalMilliseconds);
            var target = new NuGetVSTelemetryService(telemetrySession.Object);

            // Act
            target.EmitTelemetryEvent(refreshEvent);

            // Assert
            telemetrySession.Verify(x => x.PostEvent(It.IsAny<TelemetryEvent>()), Times.Once);
            Assert.NotNull(lastTelemetryEvent);
            Assert.Equal("PMUIRefresh", lastTelemetryEvent.Name);
            Assert.Equal(8, lastTelemetryEvent.Count);

            var parentIdGuid = lastTelemetryEvent["ParentId"];
            Assert.NotNull(parentIdGuid);
            Assert.IsType<string>(parentIdGuid);
            Assert.Equal(expectedGuid.ToString(), parentIdGuid);

            var solutionLevel = lastTelemetryEvent["IsSolutionLevel"];
            Assert.NotNull(solutionLevel);
            Assert.IsType<bool>(solutionLevel);
            Assert.Equal(expectedIsSolutionLevel, solutionLevel);

            var refreshSource = lastTelemetryEvent["RefreshSource"];
            Assert.NotNull(refreshSource);
            Assert.IsType<RefreshOperationSource>(refreshSource);
            Assert.Equal(expectedRefreshSource, refreshSource);

            var refreshStatus = lastTelemetryEvent["RefreshStatus"];
            Assert.NotNull(refreshStatus);
            Assert.IsType<RefreshOperationStatus>(refreshStatus);
            Assert.Equal(expectedRefreshStatus, refreshStatus);

            var tab = lastTelemetryEvent["Tab"];
            Assert.NotNull(tab);
            Assert.IsType<ItemFilter>(tab);
            Assert.Equal(expectedTab, tab);

            var isUIFiltering = lastTelemetryEvent["IsUIFiltering"];
            Assert.NotNull(isUIFiltering);
            Assert.IsType<bool>(isUIFiltering);
            Assert.Equal(expectedUiFiltering, isUIFiltering);

            var timeSinceLastRefresh = lastTelemetryEvent["TimeSinceLastRefresh"];
            Assert.NotNull(timeSinceLastRefresh);
            Assert.IsType<double>(timeSinceLastRefresh);
            Assert.Equal(expectedTimeSinceLastRefresh.TotalMilliseconds, timeSinceLastRefresh);
            var duration = lastTelemetryEvent["Duration"];
            Assert.NotNull(duration);
            Assert.IsType<double>(duration);
            Assert.Equal(expectedDuration.TotalMilliseconds, timeSinceLastRefresh);
        }
    }
}
