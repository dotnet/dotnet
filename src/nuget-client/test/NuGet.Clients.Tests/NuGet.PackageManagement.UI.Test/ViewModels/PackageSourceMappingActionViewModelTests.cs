// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.Imaging;
using Moq;
using NuGet.Configuration;
using NuGet.PackageManagement.UI.ViewModels;
using NuGet.PackageManagement.VisualStudio;
using NuGet.VisualStudio.Internal.Contracts;
using Xunit;

namespace NuGet.PackageManagement.UI.Test.ViewModels
{
    public class PackageSourceMappingActionViewModelTests
    {
        [Fact]
        public void PackageSourceMappingActionViewModel_WithNullArguments_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                PackageSourceMappingActionViewModel.Create(uiController: null);
            });
        }

        [Fact]
        public void PackageSourceMappingActionViewModel_WithNullMappingObject_PropertiesSetToDefaults()
        {
            var mockUiController = new Mock<INuGetUI>();
            var target = PackageSourceMappingActionViewModel.Create(mockUiController.Object);

            Assert.Equal(mockUiController.Object, target.UIController);
            Assert.False(target.IsPackageSourceMappingEnabled);
            Assert.False(target.CanAutomaticallyCreateSourceMapping);
            Assert.False(target.IsPackageMapped);
            Assert.Equal(Resources.Text_PackageMappingsDisabled, target.MappingStatus);
            Assert.Equal(KnownMonikers.StatusInformation, target.MappingStatusIcon);
            Assert.Null(target.PackageId);
        }

        [Fact]
        public void PackageSourceMappingActionViewModel_SelectedPackageNotMapped_PropertiesMatchState()
        {
            // Arrange
            var mockUiController = new Mock<INuGetUI>();
            string packageId = "b";

            // Enable Package Source Mapping by creating at least 1 source and pattern.
            var dictionary = new Dictionary<string, IReadOnlyList<string>>
            {
                { "sourceA", new List<string>() { "a" } }
            };
            var patterns = new ReadOnlyDictionary<string, IReadOnlyList<string>>(dictionary);
            var mockPackageSourceMapping = new Mock<PackageSourceMapping>(patterns);

            var mockUIContext = new Mock<INuGetUIContext>();
            mockUIContext.Setup(uiContext => uiContext.PackageSourceMapping).Returns(mockPackageSourceMapping.Object);
            mockUiController.Setup(uiController => uiController.UIContext).Returns(mockUIContext.Object);

            // Act
            var target = PackageSourceMappingActionViewModel.Create(mockUiController.Object);
            string targetPackageIdBeforeSelecting = target.PackageId;
            target.PackageId = packageId;

            // Assert
            Assert.Equal(mockUiController.Object, target.UIController);
            Assert.True(target.IsPackageSourceMappingEnabled);
            Assert.False(target.CanAutomaticallyCreateSourceMapping, "Expected default value since Selected package source is null.");
            Assert.False(target.IsPackageMapped);
            Assert.Equal(Resources.Text_PackageMappingsNotFound, target.MappingStatus);
            Assert.Equal(KnownMonikers.StatusError, target.MappingStatusIcon);
            Assert.Null(targetPackageIdBeforeSelecting);
            Assert.Equal(packageId, target.PackageId);
        }

        [Fact]
        public void PackageSourceMappingActionViewModel_SelectedPackageMapped_PropertiesMatchState()
        {
            // Arrange
            var mockUiController = new Mock<INuGetUI>();
            string packageId = "a";
            var dictionary = new Dictionary<string, IReadOnlyList<string>>
            {
                { "sourceA", new List<string>() { packageId } }
            };
            var patterns = new ReadOnlyDictionary<string, IReadOnlyList<string>>(dictionary);
            var mockPackageSourceMapping = new Mock<PackageSourceMapping>(patterns);

            var mockUIContext = new Mock<INuGetUIContext>();
            mockUIContext.Setup(uiContext => uiContext.PackageSourceMapping).Returns(mockPackageSourceMapping.Object);
            mockUiController.Setup(uiController => uiController.UIContext).Returns(mockUIContext.Object);

            // Act
            var target = PackageSourceMappingActionViewModel.Create(mockUiController.Object);
            target.PackageId = packageId;

            // Assert
            Assert.Equal(mockUiController.Object, target.UIController);
            Assert.True(target.IsPackageSourceMappingEnabled);
            Assert.False(target.CanAutomaticallyCreateSourceMapping);
            Assert.True(target.IsPackageMapped);
            Assert.Equal(Resources.Text_PackageMappingsFound, target.MappingStatus);
            Assert.Equal(KnownMonikers.StatusOK, target.MappingStatusIcon);
            Assert.Equal(packageId, target.PackageId);
        }

        [Fact]
        public void PackageSourceMappingIsRequired_AggregateSource_CanNotAutomaticallyCreateMapping()
        {
            // Arrange
            var mockUiController = new Mock<INuGetUI>();
            string packageId = "b";
            PackageSourceMoniker singlePackageSourceMoniker = new("sourceName", new List<PackageSourceContextInfo>() { new PackageSourceContextInfo("sourceName") }, priorityOrder: 0);
            PackageSourceMoniker aggregatePackageSourceMoniker = new("sourceName",
                new List<PackageSourceContextInfo>() { new PackageSourceContextInfo("sourceName"), new PackageSourceContextInfo("sourceName2") },
                priorityOrder: 0);

            var dictionary = new Dictionary<string, IReadOnlyList<string>>
            {
                { "sourceA", new List<string>() { "a" } }
            };
            var patterns = new ReadOnlyDictionary<string, IReadOnlyList<string>>(dictionary);
            var mockPackageSourceMapping = new Mock<PackageSourceMapping>(patterns);

            var mockUIContext = new Mock<INuGetUIContext>();
            mockUIContext.Setup(uiContext => uiContext.PackageSourceMapping).Returns(mockPackageSourceMapping.Object);
            mockUiController.Setup(uiController => uiController.UIContext).Returns(mockUIContext.Object);
            mockUiController.Setup(uiController => uiController.ActivePackageSourceMoniker).Returns(aggregatePackageSourceMoniker);

            // Act
            var target = PackageSourceMappingActionViewModel.Create(mockUiController.Object);
            target.PackageId = packageId;

            // Assert
            Assert.False(target.CanAutomaticallyCreateSourceMapping);
            Assert.Equal(Resources.Text_PackageMappingsNotFound, target.MappingStatus);
            Assert.Equal(KnownMonikers.StatusError, target.MappingStatusIcon);

            Assert.Equal(mockUiController.Object, target.UIController);
            Assert.True(target.IsPackageSourceMappingEnabled);
            Assert.False(target.IsPackageMapped);
            Assert.Equal(packageId, target.PackageId);
        }

        [Fact]
        public void PackageSourceMappingIsRequired_SingleSource_CanAutomaticallyCreateMapping()
        {
            // Arrange
            var mockUiController = new Mock<INuGetUI>();
            string packageId = "b";
            PackageSourceMoniker singlePackageSourceMoniker = new("sourceName", new List<PackageSourceContextInfo>() { new PackageSourceContextInfo("sourceName") }, priorityOrder: 0);

            var dictionary = new Dictionary<string, IReadOnlyList<string>>
            {
                { "sourceA", new List<string>() { "a" } }
            };
            var patterns = new ReadOnlyDictionary<string, IReadOnlyList<string>>(dictionary);
            var mockPackageSourceMapping = new Mock<PackageSourceMapping>(patterns);

            var mockUIContext = new Mock<INuGetUIContext>();
            mockUIContext.Setup(uiContext => uiContext.PackageSourceMapping).Returns(mockPackageSourceMapping.Object);
            mockUiController.Setup(uiController => uiController.UIContext).Returns(mockUIContext.Object);
            mockUiController.Setup(uiController => uiController.ActivePackageSourceMoniker).Returns(singlePackageSourceMoniker);

            // Act
            var target = PackageSourceMappingActionViewModel.Create(mockUiController.Object);
            target.PackageId = packageId;

            // Assert
            Assert.True(target.CanAutomaticallyCreateSourceMapping);
            Assert.Equal(Resources.Text_PackageMappingsAutoCreate, target.MappingStatus);
            Assert.Equal(KnownMonikers.StatusInformation, target.MappingStatusIcon);

            Assert.Equal(mockUiController.Object, target.UIController);
            Assert.True(target.IsPackageSourceMappingEnabled);
            Assert.False(target.IsPackageMapped);
            Assert.Equal(packageId, target.PackageId);
        }

        [Fact]
        public void PackageSourceMappingIsRequired_ProjectNotPackageReference_CanNotAutomaticallyCreateMapping()
        {
            // Arrange
            var mockUiController = new Mock<INuGetUI>();
            string packageId = "b";
            PackageSourceMoniker singlePackageSourceMoniker = new("sourceName", new List<PackageSourceContextInfo>() { new PackageSourceContextInfo("sourceName") }, priorityOrder: 0);
            Mock<INuGetProjectManagerService> projectManagerService = new Mock<INuGetProjectManagerService>();
            Mock<IProjectContextInfo> project = new Mock<IProjectContextInfo>();
            project.SetupGet(p => p.ProjectStyle).Returns(ProjectModel.ProjectStyle.PackageReference);
            project.SetupGet(p => p.ProjectId).Returns("ProjectId1");

            Mock<IProjectContextInfo> project2 = new Mock<IProjectContextInfo>();
            project2.SetupGet(p => p.ProjectStyle).Returns(ProjectModel.ProjectStyle.PackagesConfig);
            project2.SetupGet(p => p.ProjectId).Returns("ProjectId2");

            ReadOnlyCollection<IProjectContextInfo> listMockProjects = new ReadOnlyCollection<IProjectContextInfo>(
                new List<IProjectContextInfo>()
                {
                        project.Object,
                        project2.Object
                });

            var dictionary = new Dictionary<string, IReadOnlyList<string>>
            {
                { "sourceA", new List<string>() { "a" } }
            };
            var patterns = new ReadOnlyDictionary<string, IReadOnlyList<string>>(dictionary);
            var mockPackageSourceMapping = new Mock<PackageSourceMapping>(patterns);

            var mockUIContext = new Mock<INuGetUIContext>();
            mockUIContext.Setup(uiContext => uiContext.PackageSourceMapping).Returns(mockPackageSourceMapping.Object);
            mockUIContext.Setup(uiContext => uiContext.Projects).Returns(listMockProjects);
            mockUiController.Setup(uiController => uiController.UIContext).Returns(mockUIContext.Object);
            mockUiController.Setup(uiController => uiController.ActivePackageSourceMoniker).Returns(singlePackageSourceMoniker);

            // Act
            var target = PackageSourceMappingActionViewModel.Create(mockUiController.Object);
            target.PackageId = packageId;

            // Assert
            Assert.False(target.CanAutomaticallyCreateSourceMapping, "At least one project is not PackageReference");
            Assert.Equal(Resources.Text_PackageMappingsNotFound, target.MappingStatus);
            Assert.Equal(KnownMonikers.StatusError, target.MappingStatusIcon);

            Assert.Equal(mockUiController.Object, target.UIController);
            Assert.True(target.IsPackageSourceMappingEnabled);
            Assert.False(target.IsPackageMapped);
            Assert.Equal(packageId, target.PackageId);
        }
    }
}
