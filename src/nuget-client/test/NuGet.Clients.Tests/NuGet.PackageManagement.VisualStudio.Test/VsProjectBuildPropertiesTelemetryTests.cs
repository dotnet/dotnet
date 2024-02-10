// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections;
using NuGet.Common;
using NuGet.PackageManagement.VisualStudio.Telemetry;
using Xunit;

namespace NuGet.PackageManagement.VisualStudio.Test
{
    public class VsProjectBuildPropertiesTelemetryTests
    {
        [Fact]
        public void AddEventsOnShutdown_WithTwoProjectTypes_ContainsComplexDataTelemetry()
        {
            // Arrange
            var target = new VsProjectBuildPropertiesTelemetry();

            Guid projectType1 = Guid.NewGuid();
            target.OnPropertyStorageUsed("Property1", [projectType1.ToString()]);
            target.OnPropertyStorageUsed("Property2", [projectType1.ToString()]);

            Guid projectType2 = Guid.NewGuid();
            target.OnDteUsed("Property1", [projectType2.ToString()]);

            var telemetryEvent = new TelemetryEvent("test");

            // Act
            target.AddEventsOnShutdown(sender: null, telemetryEvent);

            // Assert
            object data = Assert.Contains("ProjectBuildProperties", telemetryEvent.ComplexData);
            IList list = Assert.IsAssignableFrom<IList>(data);
            Assert.Equal(3, list.Count);
        }
    }
}
