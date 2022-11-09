// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NuGet.ProjectModel;
using NuGet.VisualStudio;
using Xunit;

namespace NuGet.PackageManagement.VisualStudio.Test
{
    public class ProjectSystemCacheTests
    {
        private static readonly string _projectGuid1 = Guid.NewGuid().ToString();
        private static readonly string _projectGuid2 = Guid.NewGuid().ToString();

        [Fact]
        public void TryGetVsProjectAdapter_ReturnsProjectByFullName()
        {
            // Arrange
            var target = new ProjectSystemCache();
            var projectNames = GetTestProjectNames();
            var vsProjectAdapter = new Mock<IVsProjectAdapter>();

            target.AddProject(projectNames, vsProjectAdapter.Object, nuGetProject: null);
            IVsProjectAdapter actual;

            // Act
            var success = target.TryGetVsProjectAdapter(projectNames.FullName, out actual);

            // Assert
            Assert.True(success, "The project should have been fetched from the cache by full name.");
            Assert.Same(vsProjectAdapter.Object, actual);
        }

        [Fact]
        public void TryGetVsProjectAdapter_ReturnsProjectByCustomUniqueName()
        {
            // Arrange
            var target = new ProjectSystemCache();
            var projectNames = GetTestProjectNames();
            var vsProjectAdapter = new Mock<IVsProjectAdapter>();

            target.AddProject(projectNames, vsProjectAdapter.Object, nuGetProject: null);
            IVsProjectAdapter actual;

            // Act
            var success = target.TryGetVsProjectAdapter(projectNames.CustomUniqueName, out actual);

            // Assert
            Assert.True(success, "The project should have been fetched from the cache by custom unique name.");
            Assert.Same(vsProjectAdapter.Object, actual);
        }

        [Fact]
        public void TryGetVsProjectAdapter_ReturnsProjectByUniqueName()
        {
            // Arrange
            var target = new ProjectSystemCache();
            var projectNames = GetTestProjectNames();
            var vsProjectAdapter = new Mock<IVsProjectAdapter>();

            target.AddProject(projectNames, vsProjectAdapter.Object, nuGetProject: null);
            IVsProjectAdapter actual;

            // Act
            var success = target.TryGetVsProjectAdapter(projectNames.UniqueName, out actual);

            // Assert
            Assert.True(success, "The project should have been fetched from the cache by unique name.");
            Assert.Same(vsProjectAdapter.Object, actual);
        }

        [Fact]
        public void TryGetDTEProject_ReturnsProjectWhenShortNameIsNotAmbiguous()
        {
            // Arrange
            var target = new ProjectSystemCache();
            var projectNames = GetTestProjectNames();
            var vsProjectAdapter = new Mock<IVsProjectAdapter>();

            target.AddProject(projectNames, vsProjectAdapter.Object, nuGetProject: null);
            IVsProjectAdapter actual;

            // Act
            var success = target.TryGetVsProjectAdapter(projectNames.ShortName, out actual);

            // Assert
            Assert.True(success, "The project should have been fetched from the cache by short name.");
            Assert.Same(vsProjectAdapter.Object, actual);
        }

        [Fact]
        public void TryGetDTEProject_ReturnsNullWhenShortNameIsAmbiguous()
        {
            // Arrange
            var target = new ProjectSystemCache();

            var projectNamesA = new ProjectNames(
                fullName: @"C:\src\projectA\project.csproj",
                uniqueName: @"folderA\project",
                shortName: "project",
                customUniqueName: @"folderA\project",
                projectId: _projectGuid1);

            var projectNamesB = new ProjectNames(
                fullName: @"C:\src\projectB\project.csproj",
                uniqueName: @"folderB\project",
                shortName: projectNamesA.ShortName,
                customUniqueName: @"folderB\project",
                projectId: _projectGuid2);

            target.AddProject(projectNamesA, vsProjectAdapter: null, nuGetProject: null);
            target.AddProject(projectNamesB, vsProjectAdapter: null, nuGetProject: null);

            IVsProjectAdapter actual;

            // Act
            var success = target.TryGetVsProjectAdapter(projectNamesA.ShortName, out actual);

            // Assert
            Assert.False(success, "The project should not have been fetched from the cache by short name.");
            Assert.Null(actual);
        }

        [Fact]
        public void AddProjectRestoreInfo_AfterAddProject_UpdatesCacheEntry()
        {
            // Arrange
            var target = new ProjectSystemCache();
            var projectNames = GetTestProjectNames();
            var projectRestoreInfo = new DependencyGraphSpec();

            target.AddProject(projectNames, vsProjectAdapter: null, nuGetProject: null);

            // Act
            target.AddProjectRestoreInfo(projectNames, projectRestoreInfo, additionalMessages: null);

            // Assert
            var getPackageSpecSuccess = target.TryGetProjectRestoreInfo(projectNames.FullName, out var actual, out _);
            var getProjectNameFromUniqueNameSuccess = target.TryGetProjectNames(projectNames.UniqueName, out var names1);
            var getProjectNameFromFullNameSuccess = target.TryGetProjectNames(projectNames.FullName, out var names2);

            Assert.True(getPackageSpecSuccess);
            Assert.True(getProjectNameFromUniqueNameSuccess);
            Assert.True(getProjectNameFromFullNameSuccess);
            Assert.Same(projectRestoreInfo, actual);
            Assert.Equal(@"folder\project", names1.CustomUniqueName);
            Assert.Equal(@"folder\project", names2.CustomUniqueName);
        }

        [Fact]
        public void AddProject_AfterAddProjectRestoreInfo_UpdatesCacheEntry()
        {
            // Arrange
            var target = new ProjectSystemCache();
            var projectNames = GetTestProjectNames();
            var projectRestoreInfo = new DependencyGraphSpec();

            target.AddProjectRestoreInfo(projectNames, projectRestoreInfo, additionalMessages: null);

            // Act
            target.AddProject(projectNames, vsProjectAdapter: null, nuGetProject: null);

            // Assert
            DependencyGraphSpec actual;
            ProjectNames names;

            var getPackageSpecSuccess = target.TryGetProjectRestoreInfo(projectNames.FullName, out actual, out _);
            var getProjectNameSuccess = target.TryGetProjectNames(projectNames.UniqueName, out names);

            Assert.True(getPackageSpecSuccess);
            Assert.True(getProjectNameSuccess);
            Assert.Same(projectRestoreInfo, actual);
            Assert.Equal(@"folder\project", names.CustomUniqueName);
        }

        [Fact]
        public void AddProjectRestoreInfo_TriggersNoEvent_NoEventHandler()
        {
            // Arrange
            var target = new ProjectSystemCache();
            var projectNames = GetTestProjectNames();
            var projectRestoreInfo = new DependencyGraphSpec();

            // Act
            target.AddProjectRestoreInfo(projectNames, projectRestoreInfo, additionalMessages: null);
            target.AddProject(projectNames, vsProjectAdapter: null, nuGetProject: null);

            // Assert
            DependencyGraphSpec actual;
            ProjectNames names;
            var getPackageSpecSuccess = target.TryGetProjectRestoreInfo(projectNames.FullName, out actual, out _);
            var getProjectNameSuccess = target.TryGetProjectNames(projectNames.UniqueName, out names);

            Assert.True(getPackageSpecSuccess);
            Assert.True(getProjectNameSuccess);
            Assert.Same(projectRestoreInfo, actual);
            Assert.Equal(@"folder\project", names.CustomUniqueName);
            // Cache remains clean since no one is listening to the cache events
            Assert.Equal(target.IsCacheDirty, 0);
        }

        [Fact]
        public void AddProjectRestoreInfo_TriggersEvent_WithEventHandler_WithReset()
        {
            // Arrange
            var target = new ProjectSystemCache();
            var projectNames = GetTestProjectNames();
            var projectRestoreInfo = new DependencyGraphSpec();
            var eventCount = 0;
            target.CacheUpdated += delegate (object sender, NuGetEventArgs<string> e)
            {
                if (target.TestResetDirtyFlag())
                {
                    eventCount++;
                }
            };

            // Act
            target.AddProjectRestoreInfo(projectNames, projectRestoreInfo, additionalMessages: null);
            target.AddProject(projectNames, vsProjectAdapter: null, nuGetProject: null);

            // Assert
            DependencyGraphSpec actual;
            ProjectNames names;
            var getPackageSpecSuccess = target.TryGetProjectRestoreInfo(projectNames.FullName, out actual, out _);
            var getProjectNameSuccess = target.TryGetProjectNames(projectNames.UniqueName, out names);

            Assert.True(getPackageSpecSuccess);
            Assert.True(getProjectNameSuccess);
            Assert.Same(projectRestoreInfo, actual);
            Assert.Equal(@"folder\project", names.CustomUniqueName);
            Assert.Equal(target.IsCacheDirty, 0);
            Assert.Equal(eventCount, 1);
        }

        [Fact]
        public void AddProjectRestoreInfo_TriggersEvent_WithEventHandler_NoReset()
        {
            // Arrange
            var target = new ProjectSystemCache();
            var projectNames = GetTestProjectNames();
            var projectRestoreInfo = new DependencyGraphSpec();
            var eventCount = 0;
            target.CacheUpdated += delegate (object sender, NuGetEventArgs<string> e)
            {
                eventCount++;
            };

            // Act
            target.AddProjectRestoreInfo(projectNames, projectRestoreInfo, additionalMessages: null);
            target.AddProjectRestoreInfo(projectNames, projectRestoreInfo, additionalMessages: null);
            target.AddProject(projectNames, vsProjectAdapter: null, nuGetProject: null);

            // Assert
            DependencyGraphSpec actual;
            ProjectNames names;
            var getPackageSpecSuccess = target.TryGetProjectRestoreInfo(projectNames.FullName, out actual, out _);
            var getProjectNameSuccess = target.TryGetProjectNames(projectNames.UniqueName, out names);

            Assert.True(getPackageSpecSuccess);
            Assert.True(getProjectNameSuccess);
            Assert.Same(projectRestoreInfo, actual);
            Assert.Equal(@"folder\project", names.CustomUniqueName);

            // Since no listener resets the dirty flag, the cache remains dirty and only 1 event is raised.
            Assert.Equal(target.IsCacheDirty, 1);
            Assert.Equal(eventCount, 1);
        }

        [Fact]
        public void AddProjectRestoreInfo_TriggersMultipleEvent_WithEventHandler_WithReset()
        {
            // Arrange
            var target = new ProjectSystemCache();
            var projectNames = GetTestProjectNames();
            var projectRestoreInfo = new DependencyGraphSpec();
            var eventCount = 0;
            target.CacheUpdated += delegate (object sender, NuGetEventArgs<string> e)
            {
                if (target.TestResetDirtyFlag())
                {
                    eventCount++;
                }
            };

            // Act
            target.AddProjectRestoreInfo(projectNames, projectRestoreInfo, additionalMessages: null);
            target.AddProjectRestoreInfo(projectNames, projectRestoreInfo, additionalMessages: null);
            target.AddProjectRestoreInfo(projectNames, projectRestoreInfo, additionalMessages: null);
            target.AddProjectRestoreInfo(projectNames, projectRestoreInfo, additionalMessages: null);

            // Assert
            Assert.Equal(target.IsCacheDirty, 0);
            Assert.Equal(eventCount, 4);
        }

        [Fact]
        public void AddProjectRestoreInfo_WithAdditionalMessages_ReturnsMessagesOnGet()
        {
            // Arrange
            var target = new ProjectSystemCache();
            var projectNames = GetTestProjectNames();
            var additionalMessages = new List<IAssetsLogMessage>()
            {
                new AssetsLogMessage(Common.LogLevel.Error, Common.NuGetLogCode.NU1000, "Test error")
            };
            var projectRestoreInfo = new DependencyGraphSpec();

            // Act
            target.AddProjectRestoreInfo(projectNames, projectRestoreInfo, additionalMessages);

            // Assert
            target.TryGetProjectRestoreInfo(projectNames.FullName, out _, out var projectAdditionalMessages);
            Assert.NotNull(projectAdditionalMessages);
            Assert.Equal(additionalMessages.Count, projectAdditionalMessages.Count);
        }

        [Fact]
        public void AddProject_RemoveProject_Clear_TriggerNoEvent_WithEventHandler()
        {
            // Arrange
            var target = new ProjectSystemCache();
            var projectNames = GetTestProjectNames();
            var projectRestoreInfo = new DependencyGraphSpec();
            var eventCount = 0;
            target.CacheUpdated += delegate (object sender, NuGetEventArgs<string> e)
            {
                if (target.TestResetDirtyFlag())
                {
                    eventCount++;
                }
            };

            // Act
            target.AddProject(projectNames, vsProjectAdapter: null, nuGetProject: null);
            target.RemoveProject(projectNames.FullName);
            target.Clear();

            // Assert
            Assert.Equal(target.IsCacheDirty, 0);
            Assert.Equal(eventCount, 0);
        }

        [Fact]
        public void AddProjectRestoreInfoSource_AfterAddProject_UpdatesCacheEntry()
        {
            // Arrange
            var target = new ProjectSystemCache();
            var projectNames = GetTestProjectNames();
            object projectRestoreInfoSource = new();

            target.AddProject(projectNames, vsProjectAdapter: null, nuGetProject: null);

            // Act
            target.AddProjectRestoreInfoSource(projectNames, projectRestoreInfoSource);

            // Assert
            var getProjectRestoreInfoSources = target.GetProjectRestoreInfoSources();
            var getProjectNameFromUniqueNameSuccess = target.TryGetProjectNames(projectNames.UniqueName, out var names1);
            var getProjectNameFromFullNameSuccess = target.TryGetProjectNames(projectNames.FullName, out var names2);

            Assert.True(getProjectRestoreInfoSources.Any());
            Assert.Equal(projectRestoreInfoSource, getProjectRestoreInfoSources.Single());
            Assert.True(getProjectNameFromUniqueNameSuccess);
            Assert.True(getProjectNameFromFullNameSuccess);
            Assert.Equal(@"folder\project", names1.CustomUniqueName);
            Assert.Equal(@"folder\project", names2.CustomUniqueName);
        }

        [Fact]
        public void AddProjectRestoreInfoSource_Succeeds()
        {
            // Arrange
            var target = new ProjectSystemCache();
            var projectNames = GetTestProjectNames();
            object projectRestoreInfoSource = new();

            // Act
            var result = target.AddProjectRestoreInfoSource(projectNames, projectRestoreInfoSource);

            // Assert
            Assert.True(result);

            var getProjectRestoreInfoSources = target.GetProjectRestoreInfoSources();
            var getProjectNameFromUniqueNameSuccess = target.TryGetProjectNames(projectNames.UniqueName, out var names1);
            var getProjectNameFromFullNameSuccess = target.TryGetProjectNames(projectNames.FullName, out var names2);

            Assert.True(getProjectRestoreInfoSources.Any());
            Assert.Equal(projectRestoreInfoSource, getProjectRestoreInfoSources.Single());
            Assert.True(getProjectNameFromUniqueNameSuccess);
            Assert.True(getProjectNameFromFullNameSuccess);
            Assert.Equal(@"folder\project", names1.CustomUniqueName);
            Assert.Equal(@"folder\project", names2.CustomUniqueName);
        }

        private ProjectNames GetTestProjectNames()
        {
            var projectNames = new ProjectNames(
                fullName: @"C:\src\project\project.csproj",
                uniqueName: @"folder\project",
                shortName: "project",
                customUniqueName: @"folder\project",
                projectId: _projectGuid1);
            return projectNames;
        }
    }
}
