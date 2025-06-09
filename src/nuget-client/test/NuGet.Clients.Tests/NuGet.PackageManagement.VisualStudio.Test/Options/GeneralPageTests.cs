// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using FluentAssertions;
using NuGet.PackageManagement.VisualStudio.Options;
using Xunit;

namespace NuGet.PackageManagement.VisualStudio.Test.Options
{
    public class GeneralPageTests : NuGetExternalSettingsProviderTests<GeneralPage>
    {
        protected override GeneralPage CreateInstance(VSSettings? vsSettings)
        {
            return new GeneralPage(vsSettings!);
        }

        [Fact]
        public void VsSettings_SettingsChanged_WhenCalled_ClearsCachedSettings()
        {
            // Arrange
            GeneralPage instance = CreateInstance(_vsSettings);

            // Access each cached setting to ensure it's created.
            var bindingRedirectBehavior = instance.BindingRedirectBehavior;
            bindingRedirectBehavior.Should().NotBeNull();

            var packageRestoreConsent = instance.PackageRestoreConsent;
            packageRestoreConsent.Should().NotBeNull();

            var packageManagementFormat = instance.PackageManagementFormat;
            packageManagementFormat.Should().NotBeNull();

            // Act
            instance.VsSettings_SettingsChanged(this, EventArgs.Empty);

            // Assert
            instance._bindingRedirectBehavior.Should().BeNull();
            instance._packageRestoreConsent.Should().BeNull();
            instance._packageManagementFormat.Should().BeNull();
        }
    }
}
