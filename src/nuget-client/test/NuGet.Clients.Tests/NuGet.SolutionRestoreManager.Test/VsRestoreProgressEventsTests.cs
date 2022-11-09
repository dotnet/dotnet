// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Moq;
using NuGet.PackageManagement;
using NuGet.VisualStudio.Telemetry;
using Xunit;

namespace NuGet.SolutionRestoreManager.Test
{
    public class VsRestoreProgressEventsTests
    {
        private Mock<IPackageProjectEventsProvider> _packageProjectProvider;
        private PackageProjectEvents _packageProjectEvents;

        public VsRestoreProgressEventsTests()
        {
            _packageProjectProvider = new Mock<IPackageProjectEventsProvider>();
            _packageProjectEvents = new PackageProjectEvents();
            _packageProjectProvider.Setup(e => e.GetPackageProjectEvents()).Returns(_packageProjectEvents);
        }

        [Fact]
        public void Constructor_WithNonNullParameters_Succeeds()
        {
            _ = new VsRestoreProgressEvents(_packageProjectProvider.Object, new Mock<INuGetTelemetryProvider>().Object);
        }

        [Fact]
        public void Constructor_WithNullPackageProjectEventsProvider_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new VsRestoreProgressEvents(eventProvider: null, new Mock<INuGetTelemetryProvider>().Object));
        }

        [Fact]
        public void Constructor_WithNullTelemetryProvider_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new VsRestoreProgressEvents(_packageProjectProvider.Object, telemetryProvider: null));
        }

        [Fact]
        public void StartProjectUpdate_WithNullProjectName_ThrowsArgumentNullException()
        {
            var restoreProgressEvents = new VsRestoreProgressEvents(_packageProjectProvider.Object, new Mock<INuGetTelemetryProvider>().Object);
            Assert.Throws<ArgumentNullException>(() => restoreProgressEvents.StartProjectUpdate(null, new string[] { }));
        }

        [Fact]
        public void StartProjectUpdate_WithValidProjectName_FiresProjectUpdateEvent()
        {
            var restoreProgressEvents = new VsRestoreProgressEvents(_packageProjectProvider.Object, new Mock<INuGetTelemetryProvider>().Object);

            var expectedProjectName = "projectName.csproj";
            var expectedFileListName = new List<string>() { "project.assets.json" };

            string actualProjectName = null;
            IReadOnlyList<string> actualFileListName = null;

            restoreProgressEvents.ProjectUpdateStarted += (projectUniqueName, updatedFiles) =>
            {
                actualProjectName = projectUniqueName;
                actualFileListName = updatedFiles;
            };

            restoreProgressEvents.StartProjectUpdate(expectedProjectName, expectedFileListName);

            Assert.Equal(expectedProjectName, actualProjectName);
            Assert.Equal(expectedFileListName, actualFileListName);
        }

        [Fact]
        public void EndProjectUpdate_WithNullProjectName_ThrowsArgumentNullException()
        {
            var restoreProgressEvents = new VsRestoreProgressEvents(_packageProjectProvider.Object, new Mock<INuGetTelemetryProvider>().Object);
            Assert.Throws<ArgumentNullException>(() => restoreProgressEvents.EndProjectUpdate(null, new string[] { }));
        }

        [Fact]
        public void EndProjectUpdate_WithValidProjectName_FiresProjectUpdateEvent()
        {
            var restoreProgressEvents = new VsRestoreProgressEvents(_packageProjectProvider.Object, new Mock<INuGetTelemetryProvider>().Object);

            var expectedProjectName = "projectName.csproj";
            var expectedFileListName = new List<string>() { "project.assets.json" };

            string actualProjectName = null;
            IReadOnlyList<string> actualFileListName = null;

            restoreProgressEvents.ProjectUpdateFinished += (projectUniqueName, updatedFiles) =>
            {
                actualProjectName = projectUniqueName;
                actualFileListName = updatedFiles;
            };

            restoreProgressEvents.EndProjectUpdate(expectedProjectName, expectedFileListName);

            Assert.Equal(expectedProjectName, actualProjectName);
            Assert.Equal(expectedFileListName, actualFileListName);
        }


        [Fact]
        public void StartSolutionRestore_WithNullProjectList_ThrowsArgumentException()
        {
            var restoreProgressEvents = new VsRestoreProgressEvents(_packageProjectProvider.Object, new Mock<INuGetTelemetryProvider>().Object);
            Assert.Throws<ArgumentException>(() => restoreProgressEvents.StartSolutionRestore(null));
        }

        [Fact]
        public void StartSolutionRestore_WithEmptyProjectList_ThrowsArgumentException()
        {
            var restoreProgressEvents = new VsRestoreProgressEvents(_packageProjectProvider.Object, new Mock<INuGetTelemetryProvider>().Object);
            Assert.Throws<ArgumentException>(() => restoreProgressEvents.StartSolutionRestore(new List<string>()));
        }

        [Fact]
        public void StartSolutionRestore_WithValidProjectName_FiresProjectUpdateEvent()
        {
            var restoreProgressEvents = new VsRestoreProgressEvents(_packageProjectProvider.Object, new Mock<INuGetTelemetryProvider>().Object);

            var expectedProjectList = new List<string>() { "projectName.csproj" };

            IReadOnlyList<string> actualProjectList = null;

            restoreProgressEvents.SolutionRestoreStarted += (updatedFiles) =>
            {
                actualProjectList = updatedFiles;
            };

            restoreProgressEvents.StartSolutionRestore(expectedProjectList);

            Assert.Equal(expectedProjectList, actualProjectList);
        }


        [Fact]
        public void EndSolutionRestore_WithNullProjectList_ThrowsArgumentException()
        {
            var restoreProgressEvents = new VsRestoreProgressEvents(_packageProjectProvider.Object, new Mock<INuGetTelemetryProvider>().Object);
            Assert.Throws<ArgumentException>(() => restoreProgressEvents.EndSolutionRestore(null));
        }

        [Fact]
        public void EndSolutionRestore_WithEmptyProjectList_ThrowsArgumentException()
        {
            var restoreProgressEvents = new VsRestoreProgressEvents(_packageProjectProvider.Object, new Mock<INuGetTelemetryProvider>().Object);
            Assert.Throws<ArgumentException>(() => restoreProgressEvents.EndSolutionRestore(new List<string>()));
        }

        [Fact]
        public void EndSolutionRestore_WithValidProjectName_FiresProjectUpdateEvent()
        {
            var restoreProgressEvents = new VsRestoreProgressEvents(_packageProjectProvider.Object, new Mock<INuGetTelemetryProvider>().Object);
            var expectedProjectList = new List<string>() { "projectName.csproj" };

            IReadOnlyList<string> actualProjectList = null;

            restoreProgressEvents.SolutionRestoreFinished += (updatedFiles) =>
            {
                actualProjectList = updatedFiles;
            };

            restoreProgressEvents.EndSolutionRestore(expectedProjectList);

            Assert.Equal(expectedProjectList, actualProjectList);
        }

        [Fact]
        public void StartProjectUpdate_WhenBatchEventStartIsRaised_FiresStartProjectUpdateEvent()
        {
            var restoreProgressEvents = new VsRestoreProgressEvents(_packageProjectProvider.Object, new Mock<INuGetTelemetryProvider>().Object);

            var expectedProjectName = "projectName.csproj";
            var expectedFileListName = new List<string>() { expectedProjectName };

            string actualProjectName = null;
            IReadOnlyList<string> actualFileListName = null;

            restoreProgressEvents.ProjectUpdateStarted += (projectUniqueName, updatedFiles) =>
            {
                actualProjectName = projectUniqueName;
                actualFileListName = updatedFiles;
            };

            _packageProjectEvents.NotifyBatchStart(new PackageProjectEventArgs("id", "name", expectedProjectName));

            Assert.Equal(expectedProjectName, actualProjectName);
            Assert.Equal(expectedFileListName, actualFileListName);
        }

        [Fact]
        public void EndProjectUpdate_WhenBatchEventEndIsRaised_FiresStartProjectUpdateEvent()
        {
            var restoreProgressEvents = new VsRestoreProgressEvents(_packageProjectProvider.Object, new Mock<INuGetTelemetryProvider>().Object);

            var expectedProjectName = "projectName.csproj";
            var expectedFileListName = new List<string>() { expectedProjectName };

            string actualProjectName = null;
            IReadOnlyList<string> actualFileListName = null;

            restoreProgressEvents.ProjectUpdateFinished += (projectUniqueName, updatedFiles) =>
            {
                actualProjectName = projectUniqueName;
                actualFileListName = updatedFiles;
            };

            _packageProjectEvents.NotifyBatchEnd(new PackageProjectEventArgs("id", "name", expectedProjectName));

            Assert.Equal(expectedProjectName, actualProjectName);
            Assert.Equal(expectedFileListName, actualFileListName);
        }

        [Fact]
        public void StartProjectUpdate_WhenBatchEventStartIsRaisedWithNonProject_DoesNotFireStartProjectUpdateEvent()
        {
            var restoreProgressEvents = new VsRestoreProgressEvents(_packageProjectProvider.Object, new Mock<INuGetTelemetryProvider>().Object);
            int invocations = 0;

            restoreProgressEvents.ProjectUpdateStarted += (projectUniqueName, updatedFiles) =>
            {
                invocations++;
            };

            _packageProjectEvents.NotifyBatchStart(new PackageProjectEventArgs("id", "name", null));

            Assert.Equal(0, invocations);
        }

        [Fact]
        public void EndProjectUpdate_WhenBatchEventEndIsRaisedWithNonProject_DoesNotFireStartProjectUpdateEvent()
        {
            var restoreProgressEvents = new VsRestoreProgressEvents(_packageProjectProvider.Object, new Mock<INuGetTelemetryProvider>().Object);
            int invocations = 0;

            restoreProgressEvents.ProjectUpdateFinished += (projectUniqueName, updatedFiles) =>
            {
                invocations++;
            };

            _packageProjectEvents.NotifyBatchEnd(new PackageProjectEventArgs("id", "name", null));

            Assert.Equal(0, invocations);
        }
    }
}
