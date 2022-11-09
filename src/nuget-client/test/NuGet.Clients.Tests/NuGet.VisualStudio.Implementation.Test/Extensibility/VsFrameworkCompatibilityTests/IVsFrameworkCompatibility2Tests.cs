// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Runtime.Versioning;
using Moq;
using NuGet.VisualStudio.Implementation.Extensibility;
using NuGet.VisualStudio.Telemetry;
using Xunit;

namespace NuGet.VisualStudio.Implementation.Test.Extensibility.VsFrameworkCompatibilityTests
{
    public class IVsFrameworkCompatibility2Tests
    {
        // known/expected errors should not be reported to telemetry, hence use MockBehavior.Strict
        private Mock<INuGetTelemetryProvider> _telemetryProvider = new Mock<INuGetTelemetryProvider>(MockBehavior.Strict);

        [Fact]
        public void GetNearest_NullArguments_ThrowsArgumentNullException()
        {
            // Arrange
            var target = new VsFrameworkCompatibility(_telemetryProvider.Object);
            var targetFramework = new FrameworkName(".NETCoreApp,Version=v3.1");
            FrameworkName[] fallbackTargetFrameworks = new[]
            {
                new FrameworkName(".NETFramework,Version=v4.7.2")
            };
            var frameworks = new[]
            {
                new FrameworkName(".NETCoreApp,Version=v3.0"),
                new FrameworkName(".NETStandard,Version=v2.0"),
            };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => target.GetNearest(null, fallbackTargetFrameworks, frameworks));
            Assert.Throws<ArgumentNullException>(() => target.GetNearest(targetFramework, null, frameworks));
            Assert.Throws<ArgumentNullException>(() => target.GetNearest(targetFramework, fallbackTargetFrameworks, null));
        }

        [Fact]
        public void GetNearest_NullItemInArray_ThrowsArgumentException()
        {
            // Arrange
            var target = new VsFrameworkCompatibility(_telemetryProvider.Object);
            var targetFramework = new FrameworkName(".NETCoreApp,Version=v3.1");
            var goodFrameworks = new[]
            {
                new FrameworkName(".NETStandard,Version=v2.0")
            };
            var nullFrameworks = new FrameworkName[] { null };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => target.GetNearest(targetFramework, goodFrameworks, nullFrameworks));
            Assert.Throws<ArgumentException>(() => target.GetNearest(targetFramework, nullFrameworks, goodFrameworks));
        }

        [Fact]
        public void GetNearest_WithCompatibleFallback_Succeeds()
        {
            // Arrange
            var target = new VsFrameworkCompatibility(_telemetryProvider.Object);
            var targetFramework = new FrameworkName(".NETFramework,Version=v4.5");
            var fallbackTargetFrameworks = new[]
            {
                new FrameworkName(".NETFramework,Version=v4.5.2")
            };
            var frameworks = new[]
            {
                new FrameworkName(".NETFramework,Version=v4.5.1"),
                new FrameworkName(".NETFramework,Version=v4.6.1"),
            };

            // Act
            var actual = target.GetNearest(targetFramework, fallbackTargetFrameworks, frameworks);

            // Assert
            Assert.Equal(".NETFramework,Version=v4.5.1", actual.ToString());
        }

        [Fact]
        public void GetNearest_WithIncompatibleFallback_ReturnsNull()
        {
            // Arrange
            var target = new VsFrameworkCompatibility(_telemetryProvider.Object);
            var targetFramework = new FrameworkName(".NETFramework,Version=v4.5");
            var fallbackTargetFrameworks = new[]
            {
                new FrameworkName(".NETFramework,Version=v4.5.2")
            };
            var frameworks = new[]
            {
                new FrameworkName(".NETFramework,Version=v4.6"),
                new FrameworkName(".NETFramework,Version=v4.6.1"),
            };

            // Act
            var actual = target.GetNearest(targetFramework, fallbackTargetFrameworks, frameworks);

            // Assert
            Assert.Null(actual);
        }
    }
}
