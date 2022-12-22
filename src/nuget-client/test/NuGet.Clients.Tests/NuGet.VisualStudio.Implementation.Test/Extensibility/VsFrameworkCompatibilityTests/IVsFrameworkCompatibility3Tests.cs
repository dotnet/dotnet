// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Moq;
using NuGet.VisualStudio.Implementation.Extensibility;
using NuGet.VisualStudio.Telemetry;
using Xunit;

namespace NuGet.VisualStudio.Implementation.Test.Extensibility.VsFrameworkCompatibilityTests
{
    public class IVsFrameworkCompatibility3Tests
    {
        // known/expected errors should not be reported to telemetry, hence use MockBehavior.Strict
        private Mock<INuGetTelemetryProvider> _telemetryProvider = new Mock<INuGetTelemetryProvider>(MockBehavior.Strict);

        [Fact]
        public void GetNearest_WithCompatibleFramework_Succeeds()
        {
            // Arrange
            var target = new VsFrameworkCompatibility(_telemetryProvider.Object);
            var net5 = new VsNuGetFramework(".NETCoreApp,Version=v5.0", targetPlatformMoniker: null, targetPlatformMinVersion: null);
            var net472 = new VsNuGetFramework(".NETFramework,Version=v4.7.2", targetPlatformMoniker: null, targetPlatformMinVersion: null);
            var netcoreapp31 = new VsNuGetFramework(".NETCoreApp,Version=v3.1", targetPlatformMoniker: null, targetPlatformMinVersion: null);
            var frameworks = new IVsNuGetFramework[] { net472, netcoreapp31 };

            // Act
            var actual = target.GetNearest(net5, frameworks);

            // Assert
            Assert.Equal(netcoreapp31, actual);
        }

        [Fact]
        public void GetNearest_WithFallbackTfm_Succeeds()
        {
            // Arrange
            var target = new VsFrameworkCompatibility(_telemetryProvider.Object);

            var net5 = new VsNuGetFramework(".NETCoreApp,Version=v5.0", targetPlatformMoniker: null, targetPlatformMinVersion: null);
            var net472 = new VsNuGetFramework(".NETFramework,Version=v4.7.2", targetPlatformMoniker: null, targetPlatformMinVersion: null);
            var fallbackFrameworks = new IVsNuGetFramework[] { net472 };
            var frameworks = new IVsNuGetFramework[] { net472 };

            // Act
            var actual = target.GetNearest(net5, fallbackFrameworks, frameworks);

            // Assert
            Assert.Equal(net472, actual);
        }

        [Fact]
        public void GetNearest_NoCompatibleFramework_ReturnsNull()
        {
            // Arrange
            var target = new VsFrameworkCompatibility(_telemetryProvider.Object);

            var net5 = new VsNuGetFramework(".NETCoreApp,Version=v5.0", targetPlatformMoniker: null, targetPlatformMinVersion: null);
            var net472 = new VsNuGetFramework(".NETFramework,Version=v4.7.2", targetPlatformMoniker: null, targetPlatformMinVersion: null);
            var frameworks = new IVsNuGetFramework[] { net472 };

            // Act
            var actual = target.GetNearest(net5, frameworks);

            // Assert
            Assert.Null(actual);
        }

        [Fact]
        public void GetNearest_NullArguments_ThrowsArgumentNullException()
        {
            // Arrange
            var target = new VsFrameworkCompatibility(_telemetryProvider.Object);

            var net5 = new VsNuGetFramework(".NETCoreApp,Version=v5.0", targetPlatformMoniker: null, targetPlatformMinVersion: null);
            var netstandard2_0 = new VsNuGetFramework(".NETStandard,Version=v2.0", targetPlatformMoniker: null, targetPlatformMinVersion: null);
            var frameworks = new IVsNuGetFramework[] { netstandard2_0 };

            // Act & Asset
            Assert.Throws<ArgumentNullException>(() => target.GetNearest(targetFramework: null, frameworks: frameworks));
            Assert.Throws<ArgumentNullException>(() => target.GetNearest(targetFramework: net5, frameworks: null));
            Assert.Throws<ArgumentNullException>(() => target.GetNearest(targetFramework: null, fallbackTargetFrameworks: Array.Empty<IVsNuGetFramework>(), frameworks: frameworks));
            Assert.Throws<ArgumentNullException>(() => target.GetNearest(targetFramework: net5, fallbackTargetFrameworks: null, frameworks: frameworks));
            Assert.Throws<ArgumentNullException>(() => target.GetNearest(targetFramework: net5, fallbackTargetFrameworks: Array.Empty<IVsNuGetFramework>(), frameworks: null));
        }

        [Fact]
        public void GetNearest_NullItemInEnumerable_ThrowsArgumentException()
        {
            // Arrange
            var target = new VsFrameworkCompatibility(_telemetryProvider.Object);

            var net5 = new VsNuGetFramework(".NETCoreApp,Version=v5.0", targetPlatformMoniker: null, targetPlatformMinVersion: null);
            var netstandard2_0 = new VsNuGetFramework(".NETStandard,Version=v2.0", targetPlatformMoniker: null, targetPlatformMinVersion: null);
            var goodFrameworks = new IVsNuGetFramework[] { netstandard2_0 };
            var nullFrameworks = new IVsNuGetFramework[] { null };

            // Act & Asset
            Assert.Throws<ArgumentException>(() => target.GetNearest(targetFramework: net5, frameworks: nullFrameworks));
            Assert.Throws<ArgumentException>(() => target.GetNearest(targetFramework: net5, fallbackTargetFrameworks: goodFrameworks, frameworks: nullFrameworks));
            Assert.Throws<ArgumentException>(() => target.GetNearest(targetFramework: net5, fallbackTargetFrameworks: nullFrameworks, frameworks: goodFrameworks));
        }

        [Fact]
        public void GetNearest_InvalidFramework_ThrowsArgumentException()
        {
            // Arrange
            var target = new VsFrameworkCompatibility(_telemetryProvider.Object);

            var invalidFramework = new VsNuGetFramework("Invalid,Version=vWrong", targetPlatformMoniker: null, targetPlatformMinVersion: null);
            var netstandard2_0 = new VsNuGetFramework(".NETStandard,Version=v2.0", targetPlatformMoniker: null, targetPlatformMinVersion: null);
            var frameworks = new IVsNuGetFramework[] { netstandard2_0 };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => target.GetNearest(invalidFramework, frameworks));
        }
    }
}
