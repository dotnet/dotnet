// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Runtime.Versioning;
using Moq;
using NuGet.VisualStudio.Implementation.Extensibility;
using NuGet.VisualStudio.Telemetry;
using Xunit;

namespace NuGet.VisualStudio.Implementation.Test.Extensibility.VsFrameworkCompatibilityTests
{
    public class IVsFrameworkCompatibilityTests
    {
        // known/expected errors should not be reported to telemetry, hence use MockBehavior.Strict
        private Mock<INuGetTelemetryProvider> _telemetryProvider = new Mock<INuGetTelemetryProvider>(MockBehavior.Strict);

        [Fact]
        public void VsFrameworkCompatibility_GetNearestRejectsNullTargetFramework()
        {
            // Arrange
            var target = new VsFrameworkCompatibility(_telemetryProvider.Object);
            FrameworkName targetFramework = null;
            var frameworks = new[]
            {
                new FrameworkName(".NETFramework,Version=v4.5.1"),
                new FrameworkName(".NETFramework,Version=v4.5.2"),
            };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => target.GetNearest(targetFramework, frameworks));
        }

        [Fact]
        public void VsFrameworkCompatibility_GetNearestRejectsNullFrameworks()
        {
            // Arrange
            var target = new VsFrameworkCompatibility(_telemetryProvider.Object);
            var targetFramework = new FrameworkName(".NETFramework,Version=v4.5");
            FrameworkName[] frameworks = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => target.GetNearest(targetFramework, frameworks));
        }

        [Fact]
        public void VsFrameworkCompatibility_GetNearestWithNoneCompatible()
        {
            // Arrange
            var target = new VsFrameworkCompatibility(_telemetryProvider.Object);
            var targetFramework = new FrameworkName(".NETFramework,Version=v4.5");
            var frameworks = new[]
            {
                new FrameworkName(".NETFramework,Version=v4.5.1"),
                new FrameworkName(".NETFramework,Version=v4.5.2"),
            };

            // Act
            var actual = target.GetNearest(targetFramework, frameworks);

            // Assert
            Assert.Null(actual);
        }

        [Fact]
        public void VsFrameworkCompatibility_GetNearestWithMultipleCompatible()
        {
            // Arrange
            var target = new VsFrameworkCompatibility(_telemetryProvider.Object);
            var targetFramework = new FrameworkName(".NETFramework,Version=v4.5.1");
            var frameworks = new[]
            {
                new FrameworkName(".NETFramework,Version=v3.5"),
                new FrameworkName(".NETFramework,Version=v4.0"),
                new FrameworkName(".NETFramework,Version=v4.5"),
                new FrameworkName(".NETFramework,Version=v4.5.2"),
            };

            // Act
            var actual = target.GetNearest(targetFramework, frameworks);

            // Assert
            Assert.Equal(".NETFramework,Version=v4.5", actual.ToString());
        }

        [Fact]
        public void VsFrameworkCompatibility_GetNearestWithWithEmptyFallbackList()
        {
            // Arrange
            var target = new VsFrameworkCompatibility(_telemetryProvider.Object);
            var targetFramework = new FrameworkName(".NETFramework,Version=v4.5.1");
            var fallbackTargetFrameworks = new FrameworkName[0];
            var frameworks = new[]
            {
                new FrameworkName(".NETFramework,Version=v3.5"),
                new FrameworkName(".NETFramework,Version=v4.0"),
                new FrameworkName(".NETFramework,Version=v4.5"),
                new FrameworkName(".NETFramework,Version=v4.5.2"),
            };

            // Act
            var actual = target.GetNearest(targetFramework, frameworks);

            // Assert
            Assert.Equal(".NETFramework,Version=v4.5", actual.ToString());
        }

        [Fact]
        public void VsFrameworkCompatibility_GetNetStandardVersions()
        {
            // Arrange
            var target = new VsFrameworkCompatibility(_telemetryProvider.Object);

            // Act
            var actual = target.GetNetStandardFrameworks().ToArray();

            // Assert
            Assert.Equal(".NETStandard,Version=v1.0", actual[0].ToString());
            Assert.Equal(".NETStandard,Version=v1.1", actual[1].ToString());
            Assert.Equal(".NETStandard,Version=v1.2", actual[2].ToString());
            Assert.Equal(".NETStandard,Version=v1.3", actual[3].ToString());
            Assert.Equal(".NETStandard,Version=v1.4", actual[4].ToString());
            Assert.Equal(".NETStandard,Version=v1.5", actual[5].ToString());
            Assert.Equal(".NETStandard,Version=v1.6", actual[6].ToString());
            Assert.Equal(".NETStandard,Version=v1.7", actual[7].ToString());
            Assert.Equal(".NETStandard,Version=v2.0", actual[8].ToString());
            Assert.Equal(".NETStandard,Version=v2.1", actual[9].ToString());
            Assert.Equal(10, actual.Length);
        }
    }
}
