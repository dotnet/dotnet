// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.Imaging;
using Moq;
using NuGet.Configuration;
using NuGet.PackageManagement.UI.ViewModels;
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
            Assert.Equal(false, target.IsPackageSourceMappingEnabled);
            Assert.Equal(false, target.IsPackageMapped);
            Assert.Equal(Resources.Text_PackageMappingsDisabled, target.MappingStatus);
            Assert.Equal(KnownMonikers.StatusInformation, target.MappingStatusIcon);
            Assert.Null(target.PackageId);
        }

        [Fact]
        public void PackageSourceMappingActionViewModel_SelectedPackageNotMapped_PropertiesMatchState()
        {
            // Arrange
            var mockUiController = new Mock<INuGetUI>();
            // Enable Package Source Mapping by creating at least 1 source and pattern.
            var dictionary = new Dictionary<string, IReadOnlyList<string>>
            {
                { "sourceA", new List<string>() { "a" } }
            };
            var patterns = new ReadOnlyDictionary<string, IReadOnlyList<string>>(dictionary);
            var mockPackageSourceMapping = new Mock<PackageSourceMapping>(patterns);
            mockUiController.Setup(_ => _.UIContext.PackageSourceMapping).Returns(mockPackageSourceMapping.Object);

            // Act
            var target = PackageSourceMappingActionViewModel.Create(mockUiController.Object);

            // Assert
            Assert.Equal(mockUiController.Object, target.UIController);
            Assert.Equal(true, target.IsPackageSourceMappingEnabled);
            Assert.Equal(false, target.IsPackageMapped);
            Assert.Equal(Resources.Text_PackageMappingsNotFound, target.MappingStatus);
            Assert.Equal(KnownMonikers.StatusError, target.MappingStatusIcon);
            Assert.Null(target.PackageId);
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
            mockUiController.Setup(_ => _.UIContext.PackageSourceMapping).Returns(mockPackageSourceMapping.Object);

            // Act
            var target = PackageSourceMappingActionViewModel.Create(mockUiController.Object);
            target.PackageId = packageId;

            // Assert
            Assert.Equal(mockUiController.Object, target.UIController);
            Assert.Equal(true, target.IsPackageSourceMappingEnabled);
            Assert.Equal(true, target.IsPackageMapped);
            Assert.Equal(Resources.Text_PackageMappingsFound, target.MappingStatus);
            Assert.Equal(KnownMonikers.StatusOK, target.MappingStatusIcon);
            Assert.Equal(packageId, target.PackageId);
        }
    }
}
