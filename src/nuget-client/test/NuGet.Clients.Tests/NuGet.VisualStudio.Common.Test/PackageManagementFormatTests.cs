// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Globalization;
using Moq;
using NuGet.Configuration;
using Xunit;

namespace NuGet.VisualStudio.Common.Test
{
    public class PackageManagementFormatTests
    {
        private Mock<ISettings> _settings;
        private PackageManagementFormat _packageManagementFormat;

        public PackageManagementFormatTests()
        {
            _settings = new Mock<ISettings>();
            _packageManagementFormat = new PackageManagementFormat(_settings.Object);
        }

        [Fact]
        public void ApplyChanges_WhenPropertiesNotChanged_DoesNotThrowOrUpdateSettings()
        {
            // Act
            _packageManagementFormat.ApplyChanges();

            // Assert
            // Ensure no settings change calls are made.
            _settings.Verify(settings => settings.AddOrUpdate(It.IsAny<string>(), It.IsAny<SettingItem>()), Times.Never);

            // Calling to save settings should always happen, regardless of values.
            _settings.Verify(settings => settings.SaveToDisk(), Times.Once());
        }

        [Fact]
        public void ApplyChanges_WhenEnabledIsSet_SavesAndDoesNotThrow()
        {
            // Arrange
            _packageManagementFormat.Enabled = false;
            string expectedBooleanAsString = false.ToString(CultureInfo.InvariantCulture);

            // Act
            _packageManagementFormat.ApplyChanges();

            // Assert
            _settings.Verify(settings => settings.AddOrUpdate(ConfigurationConstants.PackageManagementSection,
                It.Is<AddItem>(addItem => addItem.Key == ConfigurationConstants.DoNotShowPackageManagementSelectionKey && addItem.Value == expectedBooleanAsString)),
                Times.Once);

            // Calling to save settings should always happen, regardless of values.
            _settings.Verify(settings => settings.SaveToDisk(), Times.Once());

            _settings.VerifyNoOtherCalls();
        }

        [Fact]
        public void ApplyChanges_WhenSelectedPackageManagementFormatIsSet_SavesAndDoesNotThrow()
        {
            // Arrange
            _packageManagementFormat.SelectedPackageManagementFormat = 0;
            string expectedIntegerAsString = 0.ToString(CultureInfo.InvariantCulture);

            // Act
            _packageManagementFormat.ApplyChanges();

            // Assert
            _settings.Verify(settings => settings.AddOrUpdate(ConfigurationConstants.PackageManagementSection,
                It.Is<AddItem>(addItem => addItem.Key == ConfigurationConstants.DefaultPackageManagementFormatKey && addItem.Value == expectedIntegerAsString)),
                Times.Once);

            // Calling to save settings should always happen, regardless of values.
            _settings.Verify(settings => settings.SaveToDisk(), Times.Once());

            _settings.VerifyNoOtherCalls();
        }
    }
}
